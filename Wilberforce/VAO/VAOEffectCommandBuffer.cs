using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;

namespace Wilberforce.VAO
{
	// Token: 0x02000572 RID: 1394
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[HelpURL("https://projectwilberforce.github.io/vaomanual/")]
	public class VAOEffectCommandBuffer : MonoBehaviour
	{
		// Token: 0x0600234D RID: 9037 RVA: 0x000C9194 File Offset: 0x000C7594
		public VAOEffectCommandBuffer()
		{
		}

		// Token: 0x0600234E RID: 9038 RVA: 0x000C9354 File Offset: 0x000C7754
		private void Start()
		{
			if (this.vaoShader == null)
			{
				this.vaoShader = Shader.Find("Hidden/Wilberforce/VAOShader");
			}
			if (this.vaoShader == null)
			{
				this.ReportError("Could not locate VAO Shader. Make sure there is 'VAOShader.shader' file added to the project.");
				this.isSupported = false;
				base.enabled = false;
				return;
			}
			if (!SystemInfo.supportsImageEffects || !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth) || SystemInfo.graphicsShaderLevel < 30)
			{
				if (!SystemInfo.supportsImageEffects)
				{
					this.ReportError("System does not support image effects.");
				}
				if (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
				{
					this.ReportError("System does not support depth texture.");
				}
				if (SystemInfo.graphicsShaderLevel < 30)
				{
					this.ReportError("This effect needs at least Shader Model 3.0.");
				}
				this.isSupported = false;
				base.enabled = false;
				return;
			}
			this.EnsureMaterials();
			if (!this.VAOMaterial || this.VAOMaterial.passCount != Enum.GetValues(typeof(VAOEffectCommandBuffer.ShaderPass)).Length)
			{
				this.ReportError("Could not create shader.");
				this.isSupported = false;
				base.enabled = false;
				return;
			}
			this.EnsureNoiseTexture();
			if (this.adaptiveSamples == null)
			{
				this.adaptiveSamples = this.GenerateAdaptiveSamples();
			}
			this.isSupported = true;
		}

		// Token: 0x0600234F RID: 9039 RVA: 0x000C9497 File Offset: 0x000C7897
		private void OnEnable()
		{
			this.myCamera = base.GetComponent<Camera>();
			this.TeardownCommandBuffer();
			this.isSPSR = this.isCameraSPSR(this.myCamera);
		}

		// Token: 0x06002350 RID: 9040 RVA: 0x000C94BD File Offset: 0x000C78BD
		private void OnValidate()
		{
			this.Radius = Mathf.Clamp(this.Radius, 0.001f, float.MaxValue);
			this.Power = Mathf.Clamp(this.Power, 0f, float.MaxValue);
		}

		// Token: 0x06002351 RID: 9041 RVA: 0x000C94F8 File Offset: 0x000C78F8
		private void OnPreRender()
		{
			this.EnsureVAOVersion();
			bool flag = false;
			bool flag2 = false;
			DepthTextureMode depthTextureMode = this.myCamera.depthTextureMode;
			if (this.myCamera.actualRenderingPath == RenderingPath.DeferredShading && this.UseGBuffer)
			{
				flag = true;
			}
			else
			{
				flag2 = true;
			}
			if (this.UsePreciseDepthBuffer && (this.myCamera.actualRenderingPath == RenderingPath.Forward || this.myCamera.actualRenderingPath == RenderingPath.VertexLit))
			{
				flag = true;
				flag2 = true;
			}
			if (flag && (depthTextureMode & DepthTextureMode.Depth) != DepthTextureMode.Depth)
			{
				this.myCamera.depthTextureMode |= DepthTextureMode.Depth;
			}
			if (flag2 && (depthTextureMode & DepthTextureMode.DepthNormals) != DepthTextureMode.DepthNormals)
			{
				this.myCamera.depthTextureMode |= DepthTextureMode.DepthNormals;
			}
			this.EnsureMaterials();
			this.EnsureNoiseTexture();
			this.TrySetUniforms();
			this.EnsureCommandBuffer(this.CheckSettingsChanges());
		}

		// Token: 0x06002352 RID: 9042 RVA: 0x000C95D4 File Offset: 0x000C79D4
		private void OnPostRender()
		{
			if (this.myCamera == null || this.myCamera.activeTexture == null)
			{
				return;
			}
			if (this.destinationWidth != this.myCamera.activeTexture.width || this.destinationHeight != this.myCamera.activeTexture.height || !this.isCommandBufferAlive)
			{
				this.destinationWidth = this.myCamera.activeTexture.width;
				this.destinationHeight = this.myCamera.activeTexture.height;
				this.TeardownCommandBuffer();
				this.EnsureCommandBuffer(false);
			}
			else
			{
				this.destinationWidth = this.myCamera.activeTexture.width;
				this.destinationHeight = this.myCamera.activeTexture.height;
			}
		}

		// Token: 0x06002353 RID: 9043 RVA: 0x000C96B4 File Offset: 0x000C7AB4
		private void OnDisable()
		{
			this.TeardownCommandBuffer();
		}

		// Token: 0x06002354 RID: 9044 RVA: 0x000C96BC File Offset: 0x000C7ABC
		private void OnDestroy()
		{
			this.TeardownCommandBuffer();
			this.onDestroyCalled = true;
		}

		// Token: 0x06002355 RID: 9045 RVA: 0x000C96CC File Offset: 0x000C7ACC
		protected void PerformOnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.isSupported || !this.vaoShader.isSupported)
			{
				base.enabled = false;
				return;
			}
			if (this.CommandBufferEnabled)
			{
				return;
			}
			this.TeardownCommandBuffer();
			int width = source.width / this.Downsampling;
			int height = source.height / this.Downsampling;
			RenderTexture renderTexture = null;
			RenderTexture renderTexture2 = null;
			if (this.HierarchicalBufferEnabled)
			{
				RenderTextureFormat format = RenderTextureFormat.RHalf;
				if (this.Mode == VAOEffectCommandBuffer.EffectMode.ColorBleed)
				{
					format = RenderTextureFormat.ARGBHalf;
				}
				renderTexture = RenderTexture.GetTemporary(source.width / 2, source.height / 2, 0, format);
				renderTexture.filterMode = FilterMode.Bilinear;
				renderTexture2 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0, format);
				renderTexture2.filterMode = FilterMode.Bilinear;
				Graphics.Blit(null, renderTexture, this.VAOMaterial, 13);
				this.DoShaderBlitCopy(renderTexture, renderTexture2);
				if (renderTexture != null)
				{
					this.VAOMaterial.SetTexture("depthNormalsTexture2", renderTexture);
				}
				if (renderTexture2 != null)
				{
					this.VAOMaterial.SetTexture("depthNormalsTexture4", renderTexture2);
				}
			}
			RenderTextureFormat format2 = RenderTextureFormat.RGHalf;
			if (this.Mode == VAOEffectCommandBuffer.EffectMode.ColorBleed)
			{
				format2 = ((!this.isHDR) ? RenderTextureFormat.Default : RenderTextureFormat.DefaultHDR);
			}
			RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0, format2);
			temporary.filterMode = FilterMode.Bilinear;
			this.VAOMaterial.SetTexture("noiseTexture", this.noiseTexture);
			RenderTexture renderTexture3 = null;
			RenderTexture renderTexture4 = null;
			if (this.CullingPrepassMode != VAOEffectCommandBuffer.CullingPrepassModeType.Off)
			{
				RenderTextureFormat format3 = RenderTextureFormat.R8;
				renderTexture3 = RenderTexture.GetTemporary(source.width / this.CullingPrepassDownsamplingFactor, source.height / this.CullingPrepassDownsamplingFactor, 0, format3);
				renderTexture3.filterMode = FilterMode.Bilinear;
				renderTexture4 = RenderTexture.GetTemporary(source.width / (this.CullingPrepassDownsamplingFactor * 2), source.height / (this.CullingPrepassDownsamplingFactor * 2), 0, format3);
				renderTexture4.filterMode = FilterMode.Bilinear;
				Graphics.Blit(source, renderTexture3, this.VAOMaterial, 0);
				this.DoShaderBlitCopy(renderTexture3, renderTexture4);
			}
			if (renderTexture4 != null)
			{
				this.VAOMaterial.SetTexture("cullingPrepassTexture", renderTexture4);
			}
			Graphics.Blit(source, temporary, this.VAOMaterial, 1);
			this.VAOMaterial.SetTexture("textureAO", temporary);
			if (this.BlurMode != VAOEffectCommandBuffer.BlurModeType.Disabled)
			{
				int width2 = source.width;
				int num = source.height;
				if (this.BlurQuality == VAOEffectCommandBuffer.BlurQualityType.Fast)
				{
					num /= 2;
				}
				if (this.BlurMode == VAOEffectCommandBuffer.BlurModeType.Enhanced)
				{
					RenderTexture temporary2 = RenderTexture.GetTemporary(width2, num, 0, format2);
					temporary2.filterMode = FilterMode.Bilinear;
					Graphics.Blit(null, temporary2, this.VAOMaterial, 6);
					this.VAOMaterial.SetTexture("textureAO", temporary2);
					Graphics.Blit(source, destination, this.VAOMaterial, 7);
					RenderTexture.ReleaseTemporary(temporary2);
				}
				else
				{
					int pass = (this.BlurQuality != VAOEffectCommandBuffer.BlurQualityType.Fast) ? 2 : 4;
					Graphics.Blit(source, destination, this.VAOMaterial, pass);
				}
			}
			else
			{
				Graphics.Blit(source, destination, this.VAOMaterial, 9);
			}
			if (temporary != null)
			{
				RenderTexture.ReleaseTemporary(temporary);
			}
			if (renderTexture3 != null)
			{
				RenderTexture.ReleaseTemporary(renderTexture3);
			}
			if (renderTexture4 != null)
			{
				RenderTexture.ReleaseTemporary(renderTexture4);
			}
			if (renderTexture != null)
			{
				RenderTexture.ReleaseTemporary(renderTexture);
			}
			if (renderTexture2 != null)
			{
				RenderTexture.ReleaseTemporary(renderTexture2);
			}
		}

		// Token: 0x06002356 RID: 9046 RVA: 0x000C9A18 File Offset: 0x000C7E18
		private void EnsureCommandBuffer(bool settingsDirty = false)
		{
			if ((!settingsDirty && this.isCommandBufferAlive) || !this.CommandBufferEnabled)
			{
				return;
			}
			if (this.onDestroyCalled)
			{
				return;
			}
			try
			{
				this.CreateCommandBuffer();
				this.lastCameraEvent = this.GetCameraEvent(this.VaoCameraEvent);
				this.isCommandBufferAlive = true;
			}
			catch (Exception ex)
			{
				this.ReportError("There was an error while trying to create command buffer. " + ex.Message);
			}
		}

		// Token: 0x06002357 RID: 9047 RVA: 0x000C9AA0 File Offset: 0x000C7EA0
		private void CreateCommandBuffer()
		{
			this.VAOMaterial = null;
			this.EnsureMaterials();
			this.TrySetUniforms();
			CameraEvent cameraEvent = this.GetCameraEvent(this.VaoCameraEvent);
			CommandBuffer commandBuffer;
			if (this.cameraEventsRegistered.TryGetValue(cameraEvent, out commandBuffer))
			{
				commandBuffer.Clear();
			}
			else
			{
				commandBuffer = new CommandBuffer();
				this.myCamera.AddCommandBuffer(cameraEvent, commandBuffer);
				commandBuffer.name = "Volumetric Ambient Occlusion";
				this.cameraEventsRegistered[cameraEvent] = commandBuffer;
			}
			bool flag = !this.OutputAOOnly && this.Mode != VAOEffectCommandBuffer.EffectMode.ColorBleed;
			RenderTargetIdentifier renderTargetIdentifier = BuiltinRenderTextureType.CameraTarget;
			int? num = null;
			int? primaryTarget = null;
			int pixelWidth = this.destinationWidth;
			int pixelHeight = this.destinationHeight;
			if (pixelWidth <= 0)
			{
				pixelWidth = this.myCamera.pixelWidth;
			}
			if (pixelHeight <= 0)
			{
				pixelHeight = this.myCamera.pixelHeight;
			}
			int num2 = pixelWidth / this.Downsampling;
			int num3 = pixelHeight / this.Downsampling;
			if (!this.OutputAOOnly)
			{
				if (!this.isHDR && (cameraEvent == CameraEvent.AfterLighting || cameraEvent == CameraEvent.BeforeReflections))
				{
					renderTargetIdentifier = BuiltinRenderTextureType.GBuffer3;
					num = new int?(Shader.PropertyToID("emissionTextureRT"));
					commandBuffer.GetTemporaryRT(num.Value, pixelWidth, pixelHeight, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB2101010, RenderTextureReadWrite.Linear);
					commandBuffer.Blit(BuiltinRenderTextureType.GBuffer3, num.Value, this.VAOMaterial, 14);
					commandBuffer.SetGlobalTexture("emissionTexture", num.Value);
					flag = false;
				}
				if (cameraEvent == CameraEvent.BeforeReflections || (cameraEvent == CameraEvent.AfterLighting && !this.isHDR && this.isSPSR))
				{
					primaryTarget = new int?(Shader.PropertyToID("occlusionTextureRT"));
					commandBuffer.GetTemporaryRT(primaryTarget.Value, pixelWidth, pixelHeight, 0, FilterMode.Bilinear, RenderTextureFormat.RHalf, RenderTextureReadWrite.Linear);
					commandBuffer.SetGlobalTexture("occlusionTexture", primaryTarget.Value);
					flag = false;
				}
			}
			int? source = null;
			if (this.Mode == VAOEffectCommandBuffer.EffectMode.ColorBleed)
			{
				RenderTextureFormat renderTextureFormat = this.GetRenderTextureFormat(this.IntermediateScreenTextureFormat, this.isHDR);
				source = new int?(Shader.PropertyToID("screenTextureRT"));
				commandBuffer.GetTemporaryRT(source.Value, pixelWidth, pixelHeight, 0, FilterMode.Bilinear, renderTextureFormat, RenderTextureReadWrite.Linear);
				commandBuffer.Blit(BuiltinRenderTextureType.CameraTarget, source.Value);
			}
			int nameID = Shader.PropertyToID("vaoTextureRT");
			RenderTextureFormat format = RenderTextureFormat.RGHalf;
			if (this.Mode == VAOEffectCommandBuffer.EffectMode.ColorBleed)
			{
				format = ((!this.isHDR) ? RenderTextureFormat.Default : RenderTextureFormat.DefaultHDR);
			}
			commandBuffer.GetTemporaryRT(nameID, num2, num3, 0, FilterMode.Bilinear, format, RenderTextureReadWrite.Linear);
			int? num4 = null;
			int? num5 = null;
			int? num6 = null;
			int? num7 = null;
			if (this.HierarchicalBufferEnabled)
			{
				RenderTextureFormat format2 = RenderTextureFormat.RHalf;
				if (this.Mode == VAOEffectCommandBuffer.EffectMode.ColorBleed)
				{
					format2 = RenderTextureFormat.ARGBHalf;
				}
				num6 = new int?(Shader.PropertyToID("downscaled2TextureRT"));
				num7 = new int?(Shader.PropertyToID("downscaled4TextureRT"));
				commandBuffer.GetTemporaryRT(num6.Value, pixelWidth / 2, pixelHeight / 2, 0, FilterMode.Bilinear, format2, RenderTextureReadWrite.Linear);
				commandBuffer.GetTemporaryRT(num7.Value, pixelWidth / 4, pixelHeight / 4, 0, FilterMode.Bilinear, format2, RenderTextureReadWrite.Linear);
				commandBuffer.Blit(null, num6.Value, this.VAOMaterial, 13);
				commandBuffer.Blit(num6.Value, num7.Value);
				if (num6 != null)
				{
					commandBuffer.SetGlobalTexture("depthNormalsTexture2", num6.Value);
				}
				if (num7 != null)
				{
					commandBuffer.SetGlobalTexture("depthNormalsTexture4", num7.Value);
				}
			}
			if (this.CullingPrepassMode != VAOEffectCommandBuffer.CullingPrepassModeType.Off)
			{
				num4 = new int?(Shader.PropertyToID("cullingPrepassTextureRT"));
				num5 = new int?(Shader.PropertyToID("cullingPrepassTextureHalfResRT"));
				RenderTextureFormat format3 = RenderTextureFormat.R8;
				commandBuffer.GetTemporaryRT(num4.Value, num2 / this.CullingPrepassDownsamplingFactor, num3 / this.CullingPrepassDownsamplingFactor, 0, FilterMode.Bilinear, format3, RenderTextureReadWrite.Linear);
				commandBuffer.GetTemporaryRT(num5.Value, num2 / (this.CullingPrepassDownsamplingFactor * 2), num3 / (this.CullingPrepassDownsamplingFactor * 2), 0, FilterMode.Bilinear, format3, RenderTextureReadWrite.Linear);
				if (this.Mode == VAOEffectCommandBuffer.EffectMode.ColorBleed)
				{
					commandBuffer.Blit(source.Value, num4.Value, this.VAOMaterial, 0);
				}
				else
				{
					commandBuffer.Blit(renderTargetIdentifier, num4.Value, this.VAOMaterial, 0);
				}
				commandBuffer.Blit(num4.Value, num5.Value);
				commandBuffer.SetGlobalTexture("cullingPrepassTexture", num5.Value);
			}
			commandBuffer.SetGlobalTexture("noiseTexture", this.noiseTexture);
			if (this.Mode == VAOEffectCommandBuffer.EffectMode.ColorBleed)
			{
				commandBuffer.Blit(source.Value, nameID, this.VAOMaterial, 1);
			}
			else
			{
				commandBuffer.Blit(renderTargetIdentifier, nameID, this.VAOMaterial, 1);
			}
			commandBuffer.SetGlobalTexture("textureAO", nameID);
			if (this.BlurMode != VAOEffectCommandBuffer.BlurModeType.Disabled)
			{
				int width = pixelWidth;
				int num8 = pixelHeight;
				if (this.BlurQuality == VAOEffectCommandBuffer.BlurQualityType.Fast)
				{
					num8 /= 2;
				}
				if (this.BlurMode == VAOEffectCommandBuffer.BlurModeType.Enhanced)
				{
					int nameID2 = Shader.PropertyToID("tempTextureRT");
					commandBuffer.GetTemporaryRT(nameID2, width, num8, 0, FilterMode.Bilinear, format, RenderTextureReadWrite.Linear);
					commandBuffer.Blit(null, nameID2, this.VAOMaterial, 6);
					commandBuffer.SetGlobalTexture("textureAO", nameID2);
					this.DoMixingBlit(commandBuffer, source, primaryTarget, renderTargetIdentifier, (!flag) ? 7 : 8);
					commandBuffer.ReleaseTemporaryRT(nameID2);
				}
				else
				{
					int num9 = (this.BlurQuality != VAOEffectCommandBuffer.BlurQualityType.Fast) ? 2 : 4;
					int num10 = (this.BlurQuality != VAOEffectCommandBuffer.BlurQualityType.Fast) ? 3 : 5;
					this.DoMixingBlit(commandBuffer, source, primaryTarget, renderTargetIdentifier, (!flag) ? num9 : num10);
				}
			}
			else
			{
				this.DoMixingBlit(commandBuffer, source, primaryTarget, renderTargetIdentifier, (!flag) ? 9 : 10);
			}
			if (cameraEvent == CameraEvent.BeforeReflections)
			{
				commandBuffer.SetRenderTarget(new RenderTargetIdentifier[]
				{
					BuiltinRenderTextureType.GBuffer0,
					renderTargetIdentifier
				}, BuiltinRenderTextureType.GBuffer0);
				commandBuffer.DrawMesh(this.GetScreenQuad(), Matrix4x4.identity, this.VAOMaterial, 0, (!this.isHDR) ? 12 : 11);
			}
			else if (cameraEvent == CameraEvent.AfterLighting && !this.isHDR && this.isSPSR)
			{
				commandBuffer.SetRenderTarget(renderTargetIdentifier);
				commandBuffer.DrawMesh(this.GetScreenQuad(), Matrix4x4.identity, this.VAOMaterial, 0, 15);
			}
			commandBuffer.ReleaseTemporaryRT(nameID);
			if (source != null)
			{
				commandBuffer.ReleaseTemporaryRT(source.Value);
			}
			if (num != null)
			{
				commandBuffer.ReleaseTemporaryRT(num.Value);
			}
			if (primaryTarget != null)
			{
				commandBuffer.ReleaseTemporaryRT(primaryTarget.Value);
			}
			if (num4 != null)
			{
				commandBuffer.ReleaseTemporaryRT(num4.Value);
			}
			if (num5 != null)
			{
				commandBuffer.ReleaseTemporaryRT(num5.Value);
			}
			if (num6 != null)
			{
				commandBuffer.ReleaseTemporaryRT(num6.Value);
			}
			if (num7 != null)
			{
				commandBuffer.ReleaseTemporaryRT(num7.Value);
			}
		}

		// Token: 0x06002358 RID: 9048 RVA: 0x000CA230 File Offset: 0x000C8630
		private void TeardownCommandBuffer()
		{
			if (!this.isCommandBufferAlive)
			{
				return;
			}
			try
			{
				this.isCommandBufferAlive = false;
				if (this.myCamera != null)
				{
					foreach (KeyValuePair<CameraEvent, CommandBuffer> keyValuePair in this.cameraEventsRegistered)
					{
						this.myCamera.RemoveCommandBuffer(keyValuePair.Key, keyValuePair.Value);
					}
				}
				this.cameraEventsRegistered.Clear();
				this.VAOMaterial = null;
				this.EnsureMaterials();
			}
			catch (Exception ex)
			{
				this.ReportError("There was an error while trying to destroy command buffer. " + ex.Message);
			}
		}

		// Token: 0x06002359 RID: 9049 RVA: 0x000CA308 File Offset: 0x000C8708
		protected Mesh GetScreenQuad()
		{
			if (this.screenQuad == null)
			{
				this.screenQuad = new Mesh
				{
					vertices = new Vector3[]
					{
						new Vector3(-1f, -1f, 0f),
						new Vector3(-1f, 1f, 0f),
						new Vector3(1f, 1f, 0f),
						new Vector3(1f, -1f, 0f)
					},
					triangles = new int[]
					{
						0,
						1,
						2,
						0,
						2,
						3
					},
					uv = new Vector2[]
					{
						new Vector2(0f, 1f),
						new Vector2(0f, 0f),
						new Vector2(1f, 0f),
						new Vector2(1f, 1f)
					}
				};
			}
			return this.screenQuad;
		}

		// Token: 0x0600235A RID: 9050 RVA: 0x000CA454 File Offset: 0x000C8854
		private CameraEvent GetCameraEvent(VAOEffectCommandBuffer.VAOCameraEventType vaoCameraEvent)
		{
			if (this.myCamera == null)
			{
				return CameraEvent.BeforeImageEffectsOpaque;
			}
			if (this.OutputAOOnly)
			{
				return CameraEvent.BeforeImageEffectsOpaque;
			}
			if (this.Mode == VAOEffectCommandBuffer.EffectMode.ColorBleed)
			{
				return CameraEvent.BeforeImageEffectsOpaque;
			}
			if (this.myCamera.actualRenderingPath != RenderingPath.DeferredShading)
			{
				return CameraEvent.BeforeImageEffectsOpaque;
			}
			switch (vaoCameraEvent)
			{
			case VAOEffectCommandBuffer.VAOCameraEventType.AfterLighting:
				return CameraEvent.AfterLighting;
			case VAOEffectCommandBuffer.VAOCameraEventType.BeforeReflections:
				return CameraEvent.BeforeReflections;
			case VAOEffectCommandBuffer.VAOCameraEventType.BeforeImageEffectsOpaque:
				return CameraEvent.BeforeImageEffectsOpaque;
			default:
				return CameraEvent.BeforeImageEffectsOpaque;
			}
		}

		// Token: 0x0600235B RID: 9051 RVA: 0x000CA4C8 File Offset: 0x000C88C8
		private void DoShaderBlitCopy(Texture sourceTexture, RenderTexture destinationTexture)
		{
			if (this.isSPSR && !this.CommandBufferEnabled)
			{
				this.VAOMaterial.SetTexture("texCopySource", sourceTexture);
				Graphics.Blit(sourceTexture, destinationTexture, this.VAOMaterial, 16);
			}
			else
			{
				Graphics.Blit(sourceTexture, destinationTexture);
			}
		}

		// Token: 0x0600235C RID: 9052 RVA: 0x000CA517 File Offset: 0x000C8917
		protected void DoMixingBlit(CommandBuffer commandBuffer, int? source, int? primaryTarget, RenderTargetIdentifier secondaryTarget, int pass)
		{
			if (primaryTarget != null)
			{
				this.DoBlit(commandBuffer, source, primaryTarget.Value, pass);
			}
			else
			{
				this.DoBlit(commandBuffer, source, secondaryTarget, pass);
			}
		}

		// Token: 0x0600235D RID: 9053 RVA: 0x000CA548 File Offset: 0x000C8948
		protected void DoBlit(CommandBuffer commandBuffer, int? source, int target, int pass)
		{
			if (source != null)
			{
				commandBuffer.Blit(source.Value, target, this.VAOMaterial, pass);
			}
			else
			{
				commandBuffer.Blit(null, target, this.VAOMaterial, pass);
			}
		}

		// Token: 0x0600235E RID: 9054 RVA: 0x000CA59B File Offset: 0x000C899B
		protected void DoBlit(CommandBuffer commandBuffer, int? source, RenderTargetIdentifier target, int pass)
		{
			if (source != null)
			{
				commandBuffer.Blit(source.Value, target, this.VAOMaterial, pass);
			}
			else
			{
				commandBuffer.Blit(null, target, this.VAOMaterial, pass);
			}
		}

		// Token: 0x0600235F RID: 9055 RVA: 0x000CA5DC File Offset: 0x000C89DC
		private void TrySetUniforms()
		{
			if (this.VAOMaterial == null)
			{
				return;
			}
			int num = this.myCamera.pixelWidth / this.Downsampling;
			int num2 = this.myCamera.pixelHeight / this.Downsampling;
			int quality = this.Quality;
			Vector4[] samples;
			switch (quality)
			{
			case 2:
				this.VAOMaterial.SetInt("minLevelIndex", 15);
				samples = VAOEffectCommandBuffer.samp2;
				break;
			default:
				if (quality != 8)
				{
					if (quality != 16)
					{
						if (quality != 32)
						{
							if (quality != 64)
							{
								this.ReportError("Unsupported quality setting " + this.Quality + " encountered. Reverting to low setting");
								this.VAOMaterial.SetInt("minLevelIndex", 1);
								this.Quality = 16;
								samples = VAOEffectCommandBuffer.samp16;
							}
							else
							{
								this.VAOMaterial.SetInt("minLevelIndex", 0);
								samples = VAOEffectCommandBuffer.samp64;
							}
						}
						else
						{
							this.VAOMaterial.SetInt("minLevelIndex", 0);
							samples = VAOEffectCommandBuffer.samp32;
						}
					}
					else
					{
						this.VAOMaterial.SetInt("minLevelIndex", 1);
						samples = VAOEffectCommandBuffer.samp16;
					}
				}
				else
				{
					this.VAOMaterial.SetInt("minLevelIndex", 3);
					samples = VAOEffectCommandBuffer.samp8;
				}
				break;
			case 4:
				this.VAOMaterial.SetInt("minLevelIndex", 7);
				samples = VAOEffectCommandBuffer.samp4;
				break;
			}
			if (this.AdaptiveType != VAOEffectCommandBuffer.AdaptiveSamplingType.Disabled)
			{
				int quality2 = this.Quality;
				switch (quality2)
				{
				case 2:
					this.AdaptiveQuality = 0.4f;
					break;
				default:
					if (quality2 != 8)
					{
						if (quality2 != 16)
						{
							if (quality2 != 32)
							{
								if (quality2 == 64)
								{
									this.AdaptiveQuality = 0.025f;
								}
							}
							else
							{
								this.AdaptiveQuality = 0.025f;
							}
						}
						else
						{
							this.AdaptiveQuality = 0.05f;
						}
					}
					else
					{
						this.AdaptiveQuality = 0.1f;
					}
					break;
				case 4:
					this.AdaptiveQuality = 0.2f;
					break;
				}
				if (this.AdaptiveType == VAOEffectCommandBuffer.AdaptiveSamplingType.EnabledManual)
				{
					this.AdaptiveQuality *= this.AdaptiveQualityCoefficient;
				}
				else
				{
					this.AdaptiveQualityCoefficient = 1f;
				}
			}
			this.AdaptiveMax = this.GetDepthForScreenSize(this.myCamera, this.AdaptiveQuality);
			Vector2 vector = new Vector2(1f / (float)num, 1f / (float)num2);
			float depthForScreenSize = this.GetDepthForScreenSize(this.myCamera, Mathf.Max(vector.x, vector.y));
			bool flag = this.GetCameraEvent(this.VaoCameraEvent) == CameraEvent.AfterLighting && this.isSPSR && !this.isHDR;
			this.VAOMaterial.SetInt("isImageEffectMode", (!this.CommandBufferEnabled) ? 1 : 0);
			this.VAOMaterial.SetInt("useSPSRFriendlyTransform", (!this.isSPSR || this.CommandBufferEnabled) ? 0 : 1);
			this.VAOMaterial.SetMatrix("invProjMatrix", this.myCamera.projectionMatrix.inverse);
			this.VAOMaterial.SetVector("screenProjection", -0.5f * new Vector4(this.myCamera.projectionMatrix.m00, this.myCamera.projectionMatrix.m11, this.myCamera.projectionMatrix.m02, this.myCamera.projectionMatrix.m12));
			this.VAOMaterial.SetFloat("halfRadiusSquared", this.Radius * 0.5f * (this.Radius * 0.5f));
			this.VAOMaterial.SetFloat("halfRadius", this.Radius * 0.5f);
			this.VAOMaterial.SetFloat("radius", this.Radius);
			this.VAOMaterial.SetInt("sampleCount", this.Quality);
			this.VAOMaterial.SetInt("fourSamplesStartIndex", this.Quality);
			this.VAOMaterial.SetFloat("aoPower", this.Power);
			this.VAOMaterial.SetFloat("aoPresence", this.Presence);
			this.VAOMaterial.SetFloat("giPresence", 1f - this.ColorBleedPresence);
			this.VAOMaterial.SetFloat("LumaThreshold", this.LumaThreshold);
			this.VAOMaterial.SetFloat("LumaKneeWidth", this.LumaKneeWidth);
			this.VAOMaterial.SetFloat("LumaTwiceKneeWidthRcp", 1f / (this.LumaKneeWidth * 2f));
			this.VAOMaterial.SetFloat("LumaKneeLinearity", this.LumaKneeLinearity);
			this.VAOMaterial.SetInt("giBackfaces", (!this.GiBackfaces) ? 1 : 0);
			this.VAOMaterial.SetFloat("adaptiveMin", this.AdaptiveMin);
			this.VAOMaterial.SetFloat("adaptiveMax", this.AdaptiveMax);
			this.VAOMaterial.SetVector("texelSize", (this.BlurMode != VAOEffectCommandBuffer.BlurModeType.Basic || this.BlurQuality != VAOEffectCommandBuffer.BlurQualityType.Fast) ? vector : (vector * 0.5f));
			this.VAOMaterial.SetFloat("blurDepthThreshold", this.Radius);
			this.VAOMaterial.SetInt("cullingPrepassMode", (int)this.CullingPrepassMode);
			this.VAOMaterial.SetVector("cullingPrepassTexelSize", new Vector2(0.5f / (float)(this.myCamera.pixelWidth / this.CullingPrepassDownsamplingFactor), 0.5f / (float)(this.myCamera.pixelHeight / this.CullingPrepassDownsamplingFactor)));
			this.VAOMaterial.SetInt("giSelfOcclusionFix", (int)this.ColorBleedSelfOcclusionFixLevel);
			this.VAOMaterial.SetInt("adaptiveMode", (int)this.AdaptiveType);
			this.VAOMaterial.SetInt("LumaMode", (int)this.LuminanceMode);
			this.VAOMaterial.SetFloat("cameraFarPlane", this.myCamera.farClipPlane);
			this.VAOMaterial.SetInt("UseCameraFarPlane", (this.FarPlaneSource != VAOEffectCommandBuffer.FarPlaneSourceType.Camera) ? 0 : 1);
			this.VAOMaterial.SetFloat("maxRadiusEnabled", (float)((!this.MaxRadiusEnabled) ? 0 : 1));
			this.VAOMaterial.SetFloat("maxRadiusCutoffDepth", this.GetDepthForScreenSize(this.myCamera, this.MaxRadius));
			this.VAOMaterial.SetFloat("projMatrix11", this.myCamera.projectionMatrix.m11);
			this.VAOMaterial.SetFloat("maxRadiusOnScreen", this.MaxRadius);
			this.VAOMaterial.SetFloat("enhancedBlurSize", (float)(this.EnhancedBlurSize / 2));
			this.VAOMaterial.SetInt("flipY", (!this.MustForceFlip(this.myCamera)) ? 0 : 1);
			this.VAOMaterial.SetInt("useGBuffer", (!this.ShouldUseGBuffer()) ? 0 : 1);
			this.VAOMaterial.SetInt("hierarchicalBufferEnabled", (!this.HierarchicalBufferEnabled) ? 0 : 1);
			this.VAOMaterial.SetInt("hwBlendingEnabled", (!this.CommandBufferEnabled || this.Mode == VAOEffectCommandBuffer.EffectMode.ColorBleed || this.GetCameraEvent(this.VaoCameraEvent) == CameraEvent.BeforeReflections || flag) ? 0 : 1);
			this.VAOMaterial.SetInt("useLogEmissiveBuffer", (!this.CommandBufferEnabled || this.isHDR || this.GetCameraEvent(this.VaoCameraEvent) != CameraEvent.AfterLighting || this.isSPSR) ? 0 : 1);
			this.VAOMaterial.SetInt("useLogBufferInput", (!this.CommandBufferEnabled || this.isHDR || (this.GetCameraEvent(this.VaoCameraEvent) != CameraEvent.AfterLighting && this.GetCameraEvent(this.VaoCameraEvent) != CameraEvent.BeforeReflections)) ? 0 : 1);
			this.VAOMaterial.SetInt("outputAOOnly", (!this.OutputAOOnly) ? 0 : 1);
			this.VAOMaterial.SetInt("isLumaSensitive", (!this.IsLumaSensitive) ? 0 : 1);
			this.VAOMaterial.SetInt("useFastBlur", (this.BlurQuality != VAOEffectCommandBuffer.BlurQualityType.Fast) ? 0 : 1);
			this.VAOMaterial.SetInt("useDedicatedDepthBuffer", (!this.UsePreciseDepthBuffer || (this.myCamera.actualRenderingPath != RenderingPath.Forward && this.myCamera.actualRenderingPath != RenderingPath.VertexLit)) ? 0 : 1);
			float num3 = this.GetDepthForScreenSize(this.myCamera, Mathf.Max(vector.x, vector.y) * this.HierarchicalBufferPixelsPerLevel * 2f);
			float num4 = this.GetDepthForScreenSize(this.myCamera, Mathf.Max(vector.x, vector.y) * this.HierarchicalBufferPixelsPerLevel);
			num3 /= -this.myCamera.farClipPlane;
			num4 /= -this.myCamera.farClipPlane;
			this.VAOMaterial.SetFloat("quarterResBufferMaxDistance", num3);
			this.VAOMaterial.SetFloat("halfResBufferMaxDistance", num4);
			this.VAOMaterial.SetInt("minRadiusEnabled", (int)this.DistanceFalloffMode);
			this.VAOMaterial.SetFloat("minRadiusCutoffDepth", (this.DistanceFalloffMode != VAOEffectCommandBuffer.DistanceFalloffModeType.Relative) ? (-this.DistanceFalloffStartAbsolute) : (Mathf.Abs(depthForScreenSize) * -(this.DistanceFalloffStartRelative * this.DistanceFalloffStartRelative)));
			this.VAOMaterial.SetFloat("minRadiusSoftness", (this.DistanceFalloffMode != VAOEffectCommandBuffer.DistanceFalloffModeType.Relative) ? this.DistanceFalloffSpeedAbsolute : (Mathf.Abs(depthForScreenSize) * (this.DistanceFalloffSpeedRelative * this.DistanceFalloffSpeedRelative)));
			this.VAOMaterial.SetInt("giSameHueAttenuationEnabled", (!this.ColorbleedHueSuppresionEnabled) ? 0 : 1);
			this.VAOMaterial.SetFloat("giSameHueAttenuationThreshold", this.ColorBleedHueSuppresionThreshold);
			this.VAOMaterial.SetFloat("giSameHueAttenuationWidth", this.ColorBleedHueSuppresionWidth);
			this.VAOMaterial.SetFloat("giSameHueAttenuationSaturationThreshold", this.ColorBleedHueSuppresionSaturationThreshold);
			this.VAOMaterial.SetFloat("giSameHueAttenuationSaturationWidth", this.ColorBleedHueSuppresionSaturationWidth);
			this.VAOMaterial.SetFloat("giSameHueAttenuationBrightness", this.ColorBleedHueSuppresionBrightness);
			this.VAOMaterial.SetFloat("subpixelRadiusCutoffDepth", Mathf.Min(0.99f, depthForScreenSize / -this.myCamera.farClipPlane));
			this.VAOMaterial.SetVector("noiseTexelSizeRcp", new Vector2((float)num / 3f, (float)num2 / 3f));
			if (this.Quality == 4 || (this.Quality == 8 && (this.ColorBleedQuality == 2 || this.ColorBleedQuality == 4)))
			{
				this.VAOMaterial.SetInt("giBlur", 3);
			}
			else
			{
				this.VAOMaterial.SetInt("giBlur", 2);
			}
			if (this.Mode == VAOEffectCommandBuffer.EffectMode.ColorBleed)
			{
				this.VAOMaterial.SetFloat("giPower", this.ColorBleedPower);
				if (this.Quality == 2 && this.ColorBleedQuality == 4)
				{
					this.VAOMaterial.SetInt("giQuality", 2);
				}
				else
				{
					this.VAOMaterial.SetInt("giQuality", this.ColorBleedQuality);
				}
			}
			if (this.CullingPrepassMode != VAOEffectCommandBuffer.CullingPrepassModeType.Off)
			{
				this.SetVectorArrayNoBuffer("eightSamples", this.VAOMaterial, VAOEffectCommandBuffer.samp8);
			}
			if (this.AdaptiveType != VAOEffectCommandBuffer.AdaptiveSamplingType.Disabled)
			{
				this.SetSampleSet("samples", this.VAOMaterial, this.GetAdaptiveSamples());
			}
			else if (this.CullingPrepassMode == VAOEffectCommandBuffer.CullingPrepassModeType.Careful)
			{
				this.SetSampleSet("samples", this.VAOMaterial, this.GetCarefulCullingPrepassSamples(samples, VAOEffectCommandBuffer.samp4));
			}
			else
			{
				this.SetSampleSet("samples", this.VAOMaterial, samples);
			}
			if (this.Mode == VAOEffectCommandBuffer.EffectMode.Simple)
			{
				this.VAOMaterial.SetColor("colorTint", Color.black);
			}
			else
			{
				this.VAOMaterial.SetColor("colorTint", this.ColorTint);
			}
			if (this.BlurMode == VAOEffectCommandBuffer.BlurModeType.Enhanced)
			{
				if (this.gaussian == null || this.gaussian.Length != this.EnhancedBlurSize || this.EnhancedBlurDeviation != this.lastDeviation)
				{
					this.gaussian = VAOEffectCommandBuffer.GenerateGaussian(this.EnhancedBlurSize, this.EnhancedBlurDeviation, out this.gaussianWeight, false);
					this.lastDeviation = this.EnhancedBlurDeviation;
				}
				this.VAOMaterial.SetFloat("gaussWeight", this.gaussianWeight);
				this.SetVectorArray("gauss", this.VAOMaterial, this.gaussian, ref this.gaussianBuffer, ref this.lastEnhancedBlurSize, true);
			}
			this.VAOMaterial.SetFloatArray("adaptiveLengths", VAOEffectCommandBuffer.adaptiveLengths);
			this.VAOMaterial.SetFloatArray("adaptiveStarts", VAOEffectCommandBuffer.adaptiveStarts);
			this.SetKeywords("WFORCE_VAO_COLORBLEED_OFF", "WFORCE_VAO_COLORBLEED_ON", this.Mode == VAOEffectCommandBuffer.EffectMode.ColorBleed);
		}

		// Token: 0x06002360 RID: 9056 RVA: 0x000CB346 File Offset: 0x000C9746
		private void SetKeywords(string offState, string onState, bool state)
		{
			if (state)
			{
				this.VAOMaterial.DisableKeyword(offState);
				this.VAOMaterial.EnableKeyword(onState);
			}
			else
			{
				this.VAOMaterial.DisableKeyword(onState);
				this.VAOMaterial.EnableKeyword(offState);
			}
		}

		// Token: 0x06002361 RID: 9057 RVA: 0x000CB384 File Offset: 0x000C9784
		private void EnsureMaterials()
		{
			if (this.vaoShader == null)
			{
				this.vaoShader = Shader.Find("Hidden/Wilberforce/VAOShader");
			}
			if (!this.VAOMaterial && this.vaoShader.isSupported)
			{
				this.VAOMaterial = VAOEffectCommandBuffer.CreateMaterial(this.vaoShader);
			}
			if (!this.vaoShader.isSupported)
			{
				this.ReportError("Could not create shader (Shader not supported).");
			}
		}

		// Token: 0x06002362 RID: 9058 RVA: 0x000CB400 File Offset: 0x000C9800
		private static Material CreateMaterial(Shader shader)
		{
			if (!shader)
			{
				return null;
			}
			return new Material(shader)
			{
				hideFlags = HideFlags.HideAndDontSave
			};
		}

		// Token: 0x06002363 RID: 9059 RVA: 0x000CB42A File Offset: 0x000C982A
		private static void DestroyMaterial(Material mat)
		{
			if (mat)
			{
				UnityEngine.Object.DestroyImmediate(mat);
				mat = null;
			}
		}

		// Token: 0x06002364 RID: 9060 RVA: 0x000CB440 File Offset: 0x000C9840
		private void SetVectorArrayNoBuffer(string name, Material VAOMaterial, Vector4[] samples)
		{
			VAOMaterial.SetVectorArray(name, samples);
		}

		// Token: 0x06002365 RID: 9061 RVA: 0x000CB44A File Offset: 0x000C984A
		private void SetVectorArray(string name, Material Material, Vector4[] samples, ref Vector4[] samplesBuffer, ref int lastBufferLength, bool needsUpdate)
		{
			if (needsUpdate || lastBufferLength != samples.Length)
			{
				Array.Copy(samples, samplesBuffer, samples.Length);
				lastBufferLength = samples.Length;
			}
			Material.SetVectorArray(name, samplesBuffer);
		}

		// Token: 0x06002366 RID: 9062 RVA: 0x000CB47A File Offset: 0x000C987A
		private void SetSampleSet(string name, Material VAOMaterial, Vector4[] samples)
		{
			this.SetVectorArray(name, VAOMaterial, samples, ref this.samplesLarge, ref this.lastSamplesLength, false);
		}

		// Token: 0x06002367 RID: 9063 RVA: 0x000CB492 File Offset: 0x000C9892
		private Vector4[] GetAdaptiveSamples()
		{
			if (this.adaptiveSamples == null)
			{
				this.adaptiveSamples = this.GenerateAdaptiveSamples();
			}
			return this.adaptiveSamples;
		}

		// Token: 0x06002368 RID: 9064 RVA: 0x000CB4B4 File Offset: 0x000C98B4
		private Vector4[] GetCarefulCullingPrepassSamples(Vector4[] samples, Vector4[] carefulSamples)
		{
			if (this.carefulCache != null && this.carefulCache.Length == samples.Length + carefulSamples.Length)
			{
				return this.carefulCache;
			}
			this.carefulCache = new Vector4[samples.Length + carefulSamples.Length];
			Array.Copy(samples, 0, this.carefulCache, 0, samples.Length);
			Array.Copy(carefulSamples, 0, this.carefulCache, samples.Length, carefulSamples.Length);
			return this.carefulCache;
		}

		// Token: 0x06002369 RID: 9065 RVA: 0x000CB524 File Offset: 0x000C9924
		private Vector4[] GenerateAdaptiveSamples()
		{
			Vector4[] array = new Vector4[62];
			Array.Copy(VAOEffectCommandBuffer.samp32, 0, array, 0, 32);
			Array.Copy(VAOEffectCommandBuffer.samp16, 0, array, 32, 16);
			Array.Copy(VAOEffectCommandBuffer.samp8, 0, array, 48, 8);
			Array.Copy(VAOEffectCommandBuffer.samp4, 0, array, 56, 4);
			Array.Copy(VAOEffectCommandBuffer.samp2, 0, array, 60, 2);
			return array;
		}

		// Token: 0x0600236A RID: 9066 RVA: 0x000CB588 File Offset: 0x000C9988
		private void EnsureNoiseTexture()
		{
			if (this.noiseTexture == null)
			{
				this.noiseTexture = new Texture2D(3, 3, TextureFormat.RGFloat, false, true);
				this.noiseTexture.SetPixels(VAOEffectCommandBuffer.noiseSamples);
				this.noiseTexture.filterMode = FilterMode.Point;
				this.noiseTexture.wrapMode = TextureWrapMode.Repeat;
				this.noiseTexture.Apply();
			}
		}

		// Token: 0x0600236B RID: 9067 RVA: 0x000CB5EC File Offset: 0x000C99EC
		private static Vector4[] GenerateGaussian(int size, float d, out float weight, bool normalize = true)
		{
			Vector4[] array = new Vector4[size];
			float num = 0f;
			double num2 = 2.0 * (double)d * (double)d;
			double num3 = Math.Sqrt(num2 * 3.141592653589793);
			float num4 = 1f / (float)(size + 1);
			for (int i = 0; i < size; i++)
			{
				float num5 = (float)i / (float)(size + 1);
				num5 += num4;
				num5 *= 6f;
				float num6 = num5 - 3f;
				float num7 = -(float)(-(float)Math.Exp((double)(-(double)(num6 * num6)) / num2) / num3);
				array[i].x = num7;
				num += num7;
			}
			if (normalize)
			{
				for (int j = 0; j < size; j++)
				{
					Vector4[] array2 = array;
					int num8 = j;
					array2[num8].x = array2[num8].x / num;
				}
			}
			weight = num;
			return array;
		}

		// Token: 0x0600236C RID: 9068 RVA: 0x000CB6CC File Offset: 0x000C9ACC
		private float GetDepthForScreenSize(Camera camera, float sizeOnScreen)
		{
			return -(this.Radius * camera.projectionMatrix.m11) / sizeOnScreen;
		}

		// Token: 0x0600236D RID: 9069 RVA: 0x000CB6F4 File Offset: 0x000C9AF4
		public bool ShouldUseHierarchicalBuffer()
		{
			if (this.myCamera == null)
			{
				return false;
			}
			Vector2 vector = new Vector2(1f / (float)this.myCamera.pixelWidth, 1f / (float)this.myCamera.pixelHeight);
			float num = this.GetDepthForScreenSize(this.myCamera, Mathf.Max(vector.x, vector.y) * this.HierarchicalBufferPixelsPerLevel * 2f);
			num /= -this.myCamera.farClipPlane;
			return num > 0.1f;
		}

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x0600236E RID: 9070 RVA: 0x000CB783 File Offset: 0x000C9B83
		public bool HierarchicalBufferEnabled
		{
			get
			{
				return this.HierarchicalBufferState == VAOEffectCommandBuffer.HierarchicalBufferStateType.On || (this.HierarchicalBufferState == VAOEffectCommandBuffer.HierarchicalBufferStateType.Auto && this.ShouldUseHierarchicalBuffer());
			}
		}

		// Token: 0x0600236F RID: 9071 RVA: 0x000CB7A8 File Offset: 0x000C9BA8
		public bool ShouldUseGBuffer()
		{
			if (this.myCamera == null)
			{
				return this.UseGBuffer;
			}
			return this.myCamera.actualRenderingPath == RenderingPath.DeferredShading && (this.VaoCameraEvent != VAOEffectCommandBuffer.VAOCameraEventType.BeforeImageEffectsOpaque || this.UseGBuffer);
		}

		// Token: 0x06002370 RID: 9072 RVA: 0x000CB7F4 File Offset: 0x000C9BF4
		protected void EnsureVAOVersion()
		{
			if (this.CommandBufferEnabled && this != null && !(this is VAOEffect))
			{
				return;
			}
			if (!this.CommandBufferEnabled && this is VAOEffect)
			{
				return;
			}
			Component[] components = base.GetComponents<Component>();
			List<KeyValuePair<FieldInfo, object>> parameters = this.GetParameters();
			int num = -1;
			Component component = null;
			for (int i = 0; i < components.Length; i++)
			{
				if (this.CommandBufferEnabled && components[i] == this)
				{
					GameObject gameObject = base.gameObject;
					UnityEngine.Object.DestroyImmediate(this);
					component = gameObject.AddComponent<VAOEffectCommandBuffer>();
					(component as VAOEffectCommandBuffer).SetParameters(parameters);
					num = i;
					break;
				}
				if (!this.CommandBufferEnabled && components[i] == this)
				{
					GameObject gameObject2 = base.gameObject;
					this.TeardownCommandBuffer();
					UnityEngine.Object.DestroyImmediate(this);
					component = gameObject2.AddComponent<VAOEffect>();
					(component as VAOEffect).SetParameters(parameters);
					num = i;
					break;
				}
			}
			if (num < 0 || component != null)
			{
			}
		}

		// Token: 0x06002371 RID: 9073 RVA: 0x000CB900 File Offset: 0x000C9D00
		private bool CheckSettingsChanges()
		{
			bool result = false;
			if (this.GetCameraEvent(this.VaoCameraEvent) != this.lastCameraEvent)
			{
				this.TeardownCommandBuffer();
				result = true;
			}
			if (this.Downsampling != this.lastDownsampling)
			{
				this.lastDownsampling = this.Downsampling;
				result = true;
			}
			if (this.CullingPrepassMode != this.lastcullingPrepassType)
			{
				this.lastcullingPrepassType = this.CullingPrepassMode;
				result = true;
			}
			if (this.CullingPrepassDownsamplingFactor != this.lastCullingPrepassDownsamplingFactor)
			{
				this.lastCullingPrepassDownsamplingFactor = this.CullingPrepassDownsamplingFactor;
				result = true;
			}
			if (this.BlurMode != this.lastBlurMode)
			{
				this.lastBlurMode = this.BlurMode;
				result = true;
			}
			if (this.Mode != this.lastMode)
			{
				this.lastMode = this.Mode;
				result = true;
			}
			if (this.UseGBuffer != this.lastUseGBuffer)
			{
				this.lastUseGBuffer = this.UseGBuffer;
				result = true;
			}
			if (this.OutputAOOnly != this.lastOutputAOOnly)
			{
				this.lastOutputAOOnly = this.OutputAOOnly;
				result = true;
			}
			this.isHDR = this.isCameraHDR(this.myCamera);
			if (this.isHDR != this.lastIsHDR)
			{
				this.lastIsHDR = this.isHDR;
				result = true;
			}
			if (this.lastIntermediateScreenTextureFormat != this.IntermediateScreenTextureFormat)
			{
				this.lastIntermediateScreenTextureFormat = this.IntermediateScreenTextureFormat;
				result = true;
			}
			if (this.lastCmdBufferEnhancedBlurSize != this.EnhancedBlurSize)
			{
				this.lastCmdBufferEnhancedBlurSize = this.EnhancedBlurSize;
				result = true;
			}
			if (this.lastHierarchicalBufferEnabled != this.HierarchicalBufferEnabled)
			{
				this.lastHierarchicalBufferEnabled = this.HierarchicalBufferEnabled;
				result = true;
			}
			if (this.lastBlurQuality != this.BlurQuality)
			{
				this.lastBlurQuality = this.BlurQuality;
				result = true;
			}
			return result;
		}

		// Token: 0x06002372 RID: 9074 RVA: 0x000CBAB8 File Offset: 0x000C9EB8
		private void WarmupPass(KeyValuePair<string, string>[] input, int i, RenderTexture tempTarget, int passNumber)
		{
			if (i >= 0)
			{
				this.SetKeywords(input[i].Key, input[i].Value, false);
				this.WarmupPass(input, i - 1, tempTarget, passNumber);
				this.SetKeywords(input[i].Key, input[i].Value, true);
				this.WarmupPass(input, i - 1, tempTarget, passNumber);
			}
			Graphics.Blit(null, tempTarget, this.VAOMaterial, passNumber);
		}

		// Token: 0x06002373 RID: 9075 RVA: 0x000CBB33 File Offset: 0x000C9F33
		private RenderTextureFormat GetRenderTextureFormat(VAOEffectCommandBuffer.ScreenTextureFormat format, bool isHDR)
		{
			switch (format)
			{
			case VAOEffectCommandBuffer.ScreenTextureFormat.ARGB32:
				return RenderTextureFormat.ARGB32;
			case VAOEffectCommandBuffer.ScreenTextureFormat.ARGBHalf:
				return RenderTextureFormat.ARGBHalf;
			case VAOEffectCommandBuffer.ScreenTextureFormat.ARGBFloat:
				return RenderTextureFormat.ARGBFloat;
			case VAOEffectCommandBuffer.ScreenTextureFormat.Default:
				return RenderTextureFormat.Default;
			case VAOEffectCommandBuffer.ScreenTextureFormat.DefaultHDR:
				return RenderTextureFormat.DefaultHDR;
			default:
				return (!isHDR) ? RenderTextureFormat.Default : RenderTextureFormat.DefaultHDR;
			}
		}

		// Token: 0x06002374 RID: 9076 RVA: 0x000CBB70 File Offset: 0x000C9F70
		private void ReportError(string error)
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogError("VAO Effect Error: " + error);
			}
		}

		// Token: 0x06002375 RID: 9077 RVA: 0x000CBB8C File Offset: 0x000C9F8C
		private void ReportWarning(string error)
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogWarning("VAO Effect Warning: " + error);
			}
		}

		// Token: 0x06002376 RID: 9078 RVA: 0x000CBBA8 File Offset: 0x000C9FA8
		private bool isCameraSPSR(Camera camera)
		{
			return !(camera == null) && camera.stereoEnabled && XRSettings.eyeTextureDesc.vrUsage == VRTextureUsage.TwoEyes;
		}

		// Token: 0x06002377 RID: 9079 RVA: 0x000CBBE0 File Offset: 0x000C9FE0
		private bool isCameraHDR(Camera camera)
		{
			return camera != null && camera.allowHDR;
		}

		// Token: 0x06002378 RID: 9080 RVA: 0x000CBBF6 File Offset: 0x000C9FF6
		private bool MustForceFlip(Camera camera)
		{
			return false;
		}

		// Token: 0x06002379 RID: 9081 RVA: 0x000CBBFC File Offset: 0x000C9FFC
		protected List<KeyValuePair<FieldInfo, object>> GetParameters()
		{
			List<KeyValuePair<FieldInfo, object>> list = new List<KeyValuePair<FieldInfo, object>>();
			FieldInfo[] fields = base.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
			foreach (FieldInfo fieldInfo in fields)
			{
				list.Add(new KeyValuePair<FieldInfo, object>(fieldInfo, fieldInfo.GetValue(this)));
			}
			return list;
		}

		// Token: 0x0600237A RID: 9082 RVA: 0x000CBC50 File Offset: 0x000CA050
		protected void SetParameters(List<KeyValuePair<FieldInfo, object>> parameters)
		{
			foreach (KeyValuePair<FieldInfo, object> keyValuePair in parameters)
			{
				keyValuePair.Key.SetValue(this, keyValuePair.Value);
			}
		}

		// Token: 0x0600237B RID: 9083 RVA: 0x000CBCB4 File Offset: 0x000CA0B4
		// Note: this type is marked as 'beforefieldinit'.
		static VAOEffectCommandBuffer()
		{
		}

		// Token: 0x04001D41 RID: 7489
		public float Radius = 0.5f;

		// Token: 0x04001D42 RID: 7490
		public float Power = 1f;

		// Token: 0x04001D43 RID: 7491
		public float Presence = 0.1f;

		// Token: 0x04001D44 RID: 7492
		public int Quality = 16;

		// Token: 0x04001D45 RID: 7493
		public bool MaxRadiusEnabled = true;

		// Token: 0x04001D46 RID: 7494
		public float MaxRadius = 0.4f;

		// Token: 0x04001D47 RID: 7495
		public VAOEffectCommandBuffer.DistanceFalloffModeType DistanceFalloffMode;

		// Token: 0x04001D48 RID: 7496
		public float DistanceFalloffStartAbsolute = 100f;

		// Token: 0x04001D49 RID: 7497
		public float DistanceFalloffStartRelative = 0.1f;

		// Token: 0x04001D4A RID: 7498
		public float DistanceFalloffSpeedAbsolute = 30f;

		// Token: 0x04001D4B RID: 7499
		public float DistanceFalloffSpeedRelative = 0.1f;

		// Token: 0x04001D4C RID: 7500
		public VAOEffectCommandBuffer.AdaptiveSamplingType AdaptiveType = VAOEffectCommandBuffer.AdaptiveSamplingType.EnabledAutomatic;

		// Token: 0x04001D4D RID: 7501
		public float AdaptiveQualityCoefficient = 1f;

		// Token: 0x04001D4E RID: 7502
		public VAOEffectCommandBuffer.CullingPrepassModeType CullingPrepassMode = VAOEffectCommandBuffer.CullingPrepassModeType.Careful;

		// Token: 0x04001D4F RID: 7503
		public int Downsampling = 1;

		// Token: 0x04001D50 RID: 7504
		public VAOEffectCommandBuffer.HierarchicalBufferStateType HierarchicalBufferState = VAOEffectCommandBuffer.HierarchicalBufferStateType.Auto;

		// Token: 0x04001D51 RID: 7505
		public bool CommandBufferEnabled = true;

		// Token: 0x04001D52 RID: 7506
		public bool UseGBuffer = true;

		// Token: 0x04001D53 RID: 7507
		public bool UsePreciseDepthBuffer = true;

		// Token: 0x04001D54 RID: 7508
		public VAOEffectCommandBuffer.VAOCameraEventType VaoCameraEvent;

		// Token: 0x04001D55 RID: 7509
		public VAOEffectCommandBuffer.FarPlaneSourceType FarPlaneSource = VAOEffectCommandBuffer.FarPlaneSourceType.Camera;

		// Token: 0x04001D56 RID: 7510
		public bool IsLumaSensitive;

		// Token: 0x04001D57 RID: 7511
		public VAOEffectCommandBuffer.LuminanceModeType LuminanceMode = VAOEffectCommandBuffer.LuminanceModeType.Luma;

		// Token: 0x04001D58 RID: 7512
		public float LumaThreshold = 0.7f;

		// Token: 0x04001D59 RID: 7513
		public float LumaKneeWidth = 0.3f;

		// Token: 0x04001D5A RID: 7514
		public float LumaKneeLinearity = 3f;

		// Token: 0x04001D5B RID: 7515
		public VAOEffectCommandBuffer.EffectMode Mode = VAOEffectCommandBuffer.EffectMode.ColorTint;

		// Token: 0x04001D5C RID: 7516
		public Color ColorTint = Color.black;

		// Token: 0x04001D5D RID: 7517
		public float ColorBleedPower = 5f;

		// Token: 0x04001D5E RID: 7518
		public float ColorBleedPresence = 1f;

		// Token: 0x04001D5F RID: 7519
		public VAOEffectCommandBuffer.ScreenTextureFormat IntermediateScreenTextureFormat;

		// Token: 0x04001D60 RID: 7520
		public bool ColorbleedHueSuppresionEnabled;

		// Token: 0x04001D61 RID: 7521
		public float ColorBleedHueSuppresionThreshold = 7f;

		// Token: 0x04001D62 RID: 7522
		public float ColorBleedHueSuppresionWidth = 2f;

		// Token: 0x04001D63 RID: 7523
		public float ColorBleedHueSuppresionSaturationThreshold = 0.5f;

		// Token: 0x04001D64 RID: 7524
		public float ColorBleedHueSuppresionSaturationWidth = 0.2f;

		// Token: 0x04001D65 RID: 7525
		public float ColorBleedHueSuppresionBrightness;

		// Token: 0x04001D66 RID: 7526
		public int ColorBleedQuality = 2;

		// Token: 0x04001D67 RID: 7527
		public VAOEffectCommandBuffer.ColorBleedSelfOcclusionFixLevelType ColorBleedSelfOcclusionFixLevel = VAOEffectCommandBuffer.ColorBleedSelfOcclusionFixLevelType.Hard;

		// Token: 0x04001D68 RID: 7528
		public bool GiBackfaces;

		// Token: 0x04001D69 RID: 7529
		public VAOEffectCommandBuffer.BlurQualityType BlurQuality = VAOEffectCommandBuffer.BlurQualityType.Precise;

		// Token: 0x04001D6A RID: 7530
		public VAOEffectCommandBuffer.BlurModeType BlurMode = VAOEffectCommandBuffer.BlurModeType.Enhanced;

		// Token: 0x04001D6B RID: 7531
		public int EnhancedBlurSize = 5;

		// Token: 0x04001D6C RID: 7532
		public float EnhancedBlurDeviation = 1.8f;

		// Token: 0x04001D6D RID: 7533
		public bool OutputAOOnly;

		// Token: 0x04001D6E RID: 7534
		public float HierarchicalBufferPixelsPerLevel = 150f;

		// Token: 0x04001D6F RID: 7535
		private int CullingPrepassDownsamplingFactor = 8;

		// Token: 0x04001D70 RID: 7536
		private float AdaptiveQuality = 0.2f;

		// Token: 0x04001D71 RID: 7537
		private float AdaptiveMin;

		// Token: 0x04001D72 RID: 7538
		private float AdaptiveMax = -10f;

		// Token: 0x04001D73 RID: 7539
		private Dictionary<CameraEvent, CommandBuffer> cameraEventsRegistered = new Dictionary<CameraEvent, CommandBuffer>();

		// Token: 0x04001D74 RID: 7540
		private bool isCommandBufferAlive;

		// Token: 0x04001D75 RID: 7541
		private Mesh screenQuad;

		// Token: 0x04001D76 RID: 7542
		private int destinationWidth;

		// Token: 0x04001D77 RID: 7543
		private int destinationHeight;

		// Token: 0x04001D78 RID: 7544
		private bool onDestroyCalled;

		// Token: 0x04001D79 RID: 7545
		public Shader vaoShader;

		// Token: 0x04001D7A RID: 7546
		private Camera myCamera;

		// Token: 0x04001D7B RID: 7547
		private bool isSupported;

		// Token: 0x04001D7C RID: 7548
		private Material VAOMaterial;

		// Token: 0x04001D7D RID: 7549
		public bool ForcedSwitchPerformed;

		// Token: 0x04001D7E RID: 7550
		public bool ForcedSwitchPerformedSinglePassStereo;

		// Token: 0x04001D7F RID: 7551
		public bool ForcedSwitchPerformedSinglePassStereoGBuffer;

		// Token: 0x04001D80 RID: 7552
		private int lastDownsampling;

		// Token: 0x04001D81 RID: 7553
		private VAOEffectCommandBuffer.CullingPrepassModeType lastcullingPrepassType;

		// Token: 0x04001D82 RID: 7554
		private int lastCullingPrepassDownsamplingFactor;

		// Token: 0x04001D83 RID: 7555
		private VAOEffectCommandBuffer.BlurModeType lastBlurMode;

		// Token: 0x04001D84 RID: 7556
		private VAOEffectCommandBuffer.BlurQualityType lastBlurQuality;

		// Token: 0x04001D85 RID: 7557
		private VAOEffectCommandBuffer.EffectMode lastMode;

		// Token: 0x04001D86 RID: 7558
		private bool lastUseGBuffer;

		// Token: 0x04001D87 RID: 7559
		private bool lastOutputAOOnly;

		// Token: 0x04001D88 RID: 7560
		private CameraEvent lastCameraEvent;

		// Token: 0x04001D89 RID: 7561
		private bool lastIsHDR;

		// Token: 0x04001D8A RID: 7562
		private bool lastIsSPSR;

		// Token: 0x04001D8B RID: 7563
		private bool isHDR;

		// Token: 0x04001D8C RID: 7564
		public bool isSPSR;

		// Token: 0x04001D8D RID: 7565
		private VAOEffectCommandBuffer.ScreenTextureFormat lastIntermediateScreenTextureFormat;

		// Token: 0x04001D8E RID: 7566
		private int lastCmdBufferEnhancedBlurSize;

		// Token: 0x04001D8F RID: 7567
		private bool lastHierarchicalBufferEnabled;

		// Token: 0x04001D90 RID: 7568
		private Texture2D noiseTexture;

		// Token: 0x04001D91 RID: 7569
		private Vector4[] adaptiveSamples;

		// Token: 0x04001D92 RID: 7570
		private Vector4[] carefulCache;

		// Token: 0x04001D93 RID: 7571
		private Vector4[] gaussian;

		// Token: 0x04001D94 RID: 7572
		private Vector4[] gaussianBuffer = new Vector4[17];

		// Token: 0x04001D95 RID: 7573
		private Vector4[] samplesLarge = new Vector4[70];

		// Token: 0x04001D96 RID: 7574
		private int lastSamplesLength;

		// Token: 0x04001D97 RID: 7575
		private int lastEnhancedBlurSize;

		// Token: 0x04001D98 RID: 7576
		private float gaussianWeight;

		// Token: 0x04001D99 RID: 7577
		private float lastDeviation = 0.5f;

		// Token: 0x04001D9A RID: 7578
		private static float[] adaptiveLengths = new float[]
		{
			32f,
			16f,
			16f,
			8f,
			8f,
			8f,
			8f,
			4f,
			4f,
			4f,
			4f,
			4f,
			4f,
			4f,
			4f,
			4f
		};

		// Token: 0x04001D9B RID: 7579
		private static float[] adaptiveStarts = new float[]
		{
			0f,
			32f,
			32f,
			48f,
			48f,
			48f,
			48f,
			56f,
			56f,
			56f,
			56f,
			56f,
			56f,
			56f,
			56f,
			56f
		};

		// Token: 0x04001D9C RID: 7580
		private static Color[] noiseSamples = new Color[]
		{
			new Color(1f, 0f, 0f),
			new Color(-0.939692f, 0.342022f, 0f),
			new Color(0.173644f, -0.984808f, 0f),
			new Color(0.173649f, 0.984808f, 0f),
			new Color(-0.500003f, -0.866024f, 0f),
			new Color(0.766045f, 0.642787f, 0f),
			new Color(-0.939694f, -0.342017f, 0f),
			new Color(0.766042f, -0.642791f, 0f),
			new Color(-0.499999f, 0.866026f, 0f)
		};

		// Token: 0x04001D9D RID: 7581
		private static Vector4[] samp2 = new Vector4[]
		{
			new Vector4(0.4392292f, 0.0127914f, 0.898284f),
			new Vector4(-0.894406f, -0.162116f, 0.41684f)
		};

		// Token: 0x04001D9E RID: 7582
		private static Vector4[] samp4 = new Vector4[]
		{
			new Vector4(-0.07984404f, -0.2016976f, 0.976188f),
			new Vector4(0.4685118f, -0.8404996f, 0.272135f),
			new Vector4(-0.793633f, 0.293059f, 0.533164f),
			new Vector4(0.2998218f, 0.4641494f, 0.83347f)
		};

		// Token: 0x04001D9F RID: 7583
		private static Vector4[] samp8 = new Vector4[]
		{
			new Vector4(-0.4999112f, -0.571184f, 0.651028f),
			new Vector4(0.2267525f, -0.668142f, 0.708639f),
			new Vector4(0.0657284f, -0.123769f, 0.990132f),
			new Vector4(0.9259827f, -0.2030669f, 0.318307f),
			new Vector4(-0.9850165f, 0.1247843f, 0.119042f),
			new Vector4(-0.2988613f, 0.2567392f, 0.919112f),
			new Vector4(0.4734727f, 0.2830991f, 0.834073f),
			new Vector4(0.1319883f, 0.9544416f, 0.267621f)
		};

		// Token: 0x04001DA0 RID: 7584
		private static Vector4[] samp16 = new Vector4[]
		{
			new Vector4(-0.6870962f, -0.7179669f, 0.111458f),
			new Vector4(-0.2574025f, -0.6144419f, 0.745791f),
			new Vector4(-0.408366f, -0.162244f, 0.898284f),
			new Vector4(-0.07098053f, 0.02052395f, 0.997267f),
			new Vector4(0.2019972f, -0.760972f, 0.616538f),
			new Vector4(0.706282f, -0.6368136f, 0.309248f),
			new Vector4(0.169605f, -0.2892981f, 0.942094f),
			new Vector4(0.7644456f, -0.05826119f, 0.64205f),
			new Vector4(-0.745912f, 0.0501786f, 0.664152f),
			new Vector4(-0.7588732f, 0.4313389f, 0.487911f),
			new Vector4(-0.3806622f, 0.3446409f, 0.85809f),
			new Vector4(-0.1296651f, 0.8794711f, 0.45795f),
			new Vector4(0.1557318f, 0.137468f, 0.978187f),
			new Vector4(0.5990864f, 0.2485375f, 0.761133f),
			new Vector4(0.1727637f, 0.5753375f, 0.799462f),
			new Vector4(0.5883294f, 0.7348878f, 0.337355f)
		};

		// Token: 0x04001DA1 RID: 7585
		private static Vector4[] samp32 = new Vector4[]
		{
			new Vector4(-0.626056f, -0.7776781f, 0.0571977f),
			new Vector4(-0.1335098f, -0.9164876f, 0.377127f),
			new Vector4(-0.2668636f, -0.5663173f, 0.779787f),
			new Vector4(-0.5712572f, -0.4639561f, 0.67706f),
			new Vector4(-0.6571807f, -0.2969118f, 0.692789f),
			new Vector4(-0.8896923f, -0.1314662f, 0.437223f),
			new Vector4(-0.5037534f, -0.03057539f, 0.863306f),
			new Vector4(-0.1773856f, -0.2664998f, 0.947371f),
			new Vector4(-0.02786797f, -0.02453661f, 0.99931f),
			new Vector4(0.173095f, -0.964425f, 0.199805f),
			new Vector4(0.280491f, -0.716259f, 0.638982f),
			new Vector4(0.7610048f, -0.4987299f, 0.414898f),
			new Vector4(0.135136f, -0.388973f, 0.911284f),
			new Vector4(0.4836829f, -0.4782286f, 0.73304f),
			new Vector4(0.1905736f, -0.1039435f, 0.976154f),
			new Vector4(0.4855643f, 0.01388972f, 0.87409f),
			new Vector4(0.5684234f, -0.2864941f, 0.771243f),
			new Vector4(0.8165832f, 0.01384446f, 0.577062f),
			new Vector4(-0.9814694f, 0.18555f, 0.0478435f),
			new Vector4(-0.5357604f, 0.3316899f, 0.776494f),
			new Vector4(-0.1238877f, 0.03315933f, 0.991742f),
			new Vector4(-0.1610546f, 0.3801286f, 0.910804f),
			new Vector4(-0.5923722f, 0.628729f, 0.503781f),
			new Vector4(-0.05504921f, 0.5483891f, 0.834409f),
			new Vector4(-0.3805041f, 0.8377199f, 0.391717f),
			new Vector4(-0.101651f, 0.9530866f, 0.285119f),
			new Vector4(0.1613653f, 0.2561041f, 0.953085f),
			new Vector4(0.4533991f, 0.2896196f, 0.842941f),
			new Vector4(0.6665574f, 0.4639243f, 0.583503f),
			new Vector4(0.8873722f, 0.4278904f, 0.1717f),
			new Vector4(0.2869751f, 0.732805f, 0.616962f),
			new Vector4(0.4188429f, 0.7185978f, 0.555147f)
		};

		// Token: 0x04001DA2 RID: 7586
		private static Vector4[] samp64 = new Vector4[]
		{
			new Vector4(-0.6700248f, -0.6370129f, 0.381157f),
			new Vector4(-0.7385408f, -0.6073685f, 0.292679f),
			new Vector4(-0.4108568f, -0.8852778f, 0.2179f),
			new Vector4(-0.3058583f, -0.8047022f, 0.508828f),
			new Vector4(0.01087609f, -0.7610992f, 0.648545f),
			new Vector4(-0.3629634f, -0.5480431f, 0.753595f),
			new Vector4(-0.1480379f, -0.6927805f, 0.70579f),
			new Vector4(-0.9533184f, -0.276674f, 0.12098f),
			new Vector4(-0.6387863f, -0.3999016f, 0.65729f),
			new Vector4(-0.891588f, -0.115146f, 0.437964f),
			new Vector4(-0.775663f, 0.0194654f, 0.630848f),
			new Vector4(-0.5360528f, -0.1828935f, 0.824134f),
			new Vector4(-0.513927f, -0.000130296f, 0.857834f),
			new Vector4(-0.4368436f, -0.2831443f, 0.853813f),
			new Vector4(-0.1794069f, -0.4226944f, 0.888337f),
			new Vector4(-0.00183062f, -0.4371257f, 0.899398f),
			new Vector4(-0.2598701f, -0.1719497f, 0.950211f),
			new Vector4(-0.08650014f, -0.004176182f, 0.996243f),
			new Vector4(0.006921067f, -0.001478712f, 0.999975f),
			new Vector4(0.05654667f, -0.9351676f, 0.349662f),
			new Vector4(0.1168661f, -0.754741f, 0.64553f),
			new Vector4(0.3534952f, -0.7472929f, 0.562667f),
			new Vector4(0.1635596f, -0.5863093f, 0.793404f),
			new Vector4(0.5910167f, -0.786864f, 0.177609f),
			new Vector4(0.5820105f, -0.5659724f, 0.5839f),
			new Vector4(0.7254612f, -0.5323696f, 0.436221f),
			new Vector4(0.4016336f, -0.4329237f, 0.807012f),
			new Vector4(0.5287027f, -0.4064075f, 0.745188f),
			new Vector4(0.314015f, -0.2375291f, 0.919225f),
			new Vector4(0.02922117f, -0.2097672f, 0.977315f),
			new Vector4(0.4201531f, -0.1445212f, 0.895871f),
			new Vector4(0.2821195f, -0.01079273f, 0.959319f),
			new Vector4(0.7152653f, -0.1972963f, 0.670425f),
			new Vector4(0.8167331f, -0.1217311f, 0.564029f),
			new Vector4(0.8517836f, 0.01290532f, 0.523735f),
			new Vector4(-0.657816f, 0.134013f, 0.74116f),
			new Vector4(-0.851676f, 0.321285f, 0.414033f),
			new Vector4(-0.603183f, 0.361627f, 0.710912f),
			new Vector4(-0.6607267f, 0.5282444f, 0.533289f),
			new Vector4(-0.323619f, 0.182656f, 0.92839f),
			new Vector4(-0.2080927f, 0.1494067f, 0.966631f),
			new Vector4(-0.4205947f, 0.4184987f, 0.804959f),
			new Vector4(-0.06831062f, 0.3712724f, 0.926008f),
			new Vector4(-0.165943f, 0.5029928f, 0.84821f),
			new Vector4(-0.6137413f, 0.7001954f, 0.364758f),
			new Vector4(-0.3009551f, 0.6550035f, 0.693107f),
			new Vector4(-0.1356791f, 0.6460465f, 0.751143f),
			new Vector4(-0.3677429f, 0.7920387f, 0.487278f),
			new Vector4(-0.08688695f, 0.9677781f, 0.236338f),
			new Vector4(0.07250954f, 0.1327261f, 0.988497f),
			new Vector4(0.5244588f, 0.05565827f, 0.849615f),
			new Vector4(0.2498424f, 0.3364912f, 0.907938f),
			new Vector4(0.2608168f, 0.5340923f, 0.804189f),
			new Vector4(0.3888291f, 0.3207975f, 0.863655f),
			new Vector4(0.6413552f, 0.1619097f, 0.749966f),
			new Vector4(0.8523082f, 0.2647078f, 0.451111f),
			new Vector4(0.5591328f, 0.3038472f, 0.771393f),
			new Vector4(0.9147445f, 0.3917669f, 0.0987938f),
			new Vector4(0.08110893f, 0.7317293f, 0.676752f),
			new Vector4(0.3154335f, 0.7388063f, 0.59554f),
			new Vector4(0.1677455f, 0.9625717f, 0.212877f),
			new Vector4(0.3015989f, 0.9509261f, 0.069128f),
			new Vector4(0.5600207f, 0.5649592f, 0.605969f),
			new Vector4(0.6455291f, 0.7387806f, 0.193637f)
		};

		// Token: 0x02000573 RID: 1395
		public enum EffectMode
		{
			// Token: 0x04001DA4 RID: 7588
			Simple = 1,
			// Token: 0x04001DA5 RID: 7589
			ColorTint,
			// Token: 0x04001DA6 RID: 7590
			ColorBleed
		}

		// Token: 0x02000574 RID: 1396
		public enum LuminanceModeType
		{
			// Token: 0x04001DA8 RID: 7592
			Luma = 1,
			// Token: 0x04001DA9 RID: 7593
			HSVValue
		}

		// Token: 0x02000575 RID: 1397
		public enum GiBlurAmmount
		{
			// Token: 0x04001DAB RID: 7595
			Auto = 1,
			// Token: 0x04001DAC RID: 7596
			Less,
			// Token: 0x04001DAD RID: 7597
			More
		}

		// Token: 0x02000576 RID: 1398
		public enum CullingPrepassModeType
		{
			// Token: 0x04001DAF RID: 7599
			Off,
			// Token: 0x04001DB0 RID: 7600
			Greedy,
			// Token: 0x04001DB1 RID: 7601
			Careful
		}

		// Token: 0x02000577 RID: 1399
		public enum AdaptiveSamplingType
		{
			// Token: 0x04001DB3 RID: 7603
			Disabled,
			// Token: 0x04001DB4 RID: 7604
			EnabledAutomatic,
			// Token: 0x04001DB5 RID: 7605
			EnabledManual
		}

		// Token: 0x02000578 RID: 1400
		public enum BlurModeType
		{
			// Token: 0x04001DB7 RID: 7607
			Disabled,
			// Token: 0x04001DB8 RID: 7608
			Basic,
			// Token: 0x04001DB9 RID: 7609
			Enhanced
		}

		// Token: 0x02000579 RID: 1401
		public enum BlurQualityType
		{
			// Token: 0x04001DBB RID: 7611
			Fast,
			// Token: 0x04001DBC RID: 7612
			Precise
		}

		// Token: 0x0200057A RID: 1402
		public enum ColorBleedSelfOcclusionFixLevelType
		{
			// Token: 0x04001DBE RID: 7614
			Off,
			// Token: 0x04001DBF RID: 7615
			Soft,
			// Token: 0x04001DC0 RID: 7616
			Hard
		}

		// Token: 0x0200057B RID: 1403
		public enum ScreenTextureFormat
		{
			// Token: 0x04001DC2 RID: 7618
			Auto,
			// Token: 0x04001DC3 RID: 7619
			ARGB32,
			// Token: 0x04001DC4 RID: 7620
			ARGBHalf,
			// Token: 0x04001DC5 RID: 7621
			ARGBFloat,
			// Token: 0x04001DC6 RID: 7622
			Default,
			// Token: 0x04001DC7 RID: 7623
			DefaultHDR
		}

		// Token: 0x0200057C RID: 1404
		public enum FarPlaneSourceType
		{
			// Token: 0x04001DC9 RID: 7625
			ProjectionParams,
			// Token: 0x04001DCA RID: 7626
			Camera
		}

		// Token: 0x0200057D RID: 1405
		public enum DistanceFalloffModeType
		{
			// Token: 0x04001DCC RID: 7628
			Off,
			// Token: 0x04001DCD RID: 7629
			Absolute,
			// Token: 0x04001DCE RID: 7630
			Relative
		}

		// Token: 0x0200057E RID: 1406
		public enum VAOCameraEventType
		{
			// Token: 0x04001DD0 RID: 7632
			AfterLighting,
			// Token: 0x04001DD1 RID: 7633
			BeforeReflections,
			// Token: 0x04001DD2 RID: 7634
			BeforeImageEffectsOpaque
		}

		// Token: 0x0200057F RID: 1407
		public enum HierarchicalBufferStateType
		{
			// Token: 0x04001DD4 RID: 7636
			Off,
			// Token: 0x04001DD5 RID: 7637
			On,
			// Token: 0x04001DD6 RID: 7638
			Auto
		}

		// Token: 0x02000580 RID: 1408
		private enum ShaderPass
		{
			// Token: 0x04001DD8 RID: 7640
			CullingPrepass,
			// Token: 0x04001DD9 RID: 7641
			MainPass,
			// Token: 0x04001DDA RID: 7642
			StandardBlurUniform,
			// Token: 0x04001DDB RID: 7643
			StandardBlurUniformMultiplyBlend,
			// Token: 0x04001DDC RID: 7644
			StandardBlurUniformFast,
			// Token: 0x04001DDD RID: 7645
			StandardBlurUniformFastMultiplyBlend,
			// Token: 0x04001DDE RID: 7646
			EnhancedBlurFirstPass,
			// Token: 0x04001DDF RID: 7647
			EnhancedBlurSecondPass,
			// Token: 0x04001DE0 RID: 7648
			EnhancedBlurSecondPassMultiplyBlend,
			// Token: 0x04001DE1 RID: 7649
			Mixing,
			// Token: 0x04001DE2 RID: 7650
			MixingMultiplyBlend,
			// Token: 0x04001DE3 RID: 7651
			BlendBeforeReflections,
			// Token: 0x04001DE4 RID: 7652
			BlendBeforeReflectionsLog,
			// Token: 0x04001DE5 RID: 7653
			DownscaleDepthNormalsPass,
			// Token: 0x04001DE6 RID: 7654
			Copy,
			// Token: 0x04001DE7 RID: 7655
			BlendAfterLightingLog,
			// Token: 0x04001DE8 RID: 7656
			TexCopyImageEffectSPSR
		}
	}
}
