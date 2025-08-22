using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace mset
{
	// Token: 0x02000338 RID: 824
	public class SkyProbe
	{
		// Token: 0x060013FA RID: 5114 RVA: 0x00073008 File Offset: 0x00071408
		public SkyProbe()
		{
			SkyProbe.buildRandomValueTable();
		}

		// Token: 0x060013FB RID: 5115 RVA: 0x00073058 File Offset: 0x00071458
		public static void buildRandomValueTable()
		{
			if (SkyProbe.randomValues == null)
			{
				float num = (float)SkyProbe.sampleCount;
				SkyProbe.randomValues = new Vector4[SkyProbe.sampleCount];
				float[] array = new float[SkyProbe.sampleCount];
				for (int i = 0; i < SkyProbe.sampleCount; i++)
				{
					SkyProbe.randomValues[i] = default(Vector4);
					array[i] = (SkyProbe.randomValues[i].x = (float)(i + 1) / num);
				}
				int num2 = SkyProbe.sampleCount;
				for (int j = 0; j < SkyProbe.sampleCount; j++)
				{
					int num3 = UnityEngine.Random.Range(0, num2 - 1);
					float num4 = array[num3];
					array[num3] = array[--num2];
					SkyProbe.randomValues[j].y = num4;
					SkyProbe.randomValues[j].z = Mathf.Cos(6.2831855f * num4);
					SkyProbe.randomValues[j].w = Mathf.Sin(6.2831855f * num4);
				}
			}
		}

		// Token: 0x060013FC RID: 5116 RVA: 0x0007316C File Offset: 0x0007156C
		public static void bindRandomValueTable(Material mat, string paramName, int inputFaceSize)
		{
			for (int i = 0; i < SkyProbe.sampleCount; i++)
			{
				mat.SetVector(paramName + i, SkyProbe.randomValues[i]);
			}
			float num = (float)(inputFaceSize * inputFaceSize) / (float)SkyProbe.sampleCount;
			num = 0.5f * Mathf.Log(num, 2f) + 0.5f;
			mat.SetFloat("_ImportantLog", num);
		}

		// Token: 0x060013FD RID: 5117 RVA: 0x000731E1 File Offset: 0x000715E1
		public static void buildRandomValueCode()
		{
		}

		// Token: 0x060013FE RID: 5118 RVA: 0x000731E4 File Offset: 0x000715E4
		public void blur(Cubemap targetCube, Texture sourceCube, bool dstRGBM, bool srcRGBM, bool linear)
		{
			if (sourceCube == null || targetCube == null)
			{
				return;
			}
			GameObject gameObject = new GameObject("_temp_probe");
			gameObject.hideFlags = HideFlags.HideAndDontSave;
			gameObject.SetActive(true);
			Camera camera = gameObject.AddComponent<Camera>();
			camera.renderingPath = this.renderPath;
			camera.useOcclusionCulling = false;
			Material material = new Material(Shader.Find("Hidden/Marmoset/RGBM Cube"));
			Matrix4x4 identity = Matrix4x4.identity;
			int num = this.maxExponent;
			bool flag = this.generateMipChain;
			this.maxExponent = 16 * num;
			this.generateMipChain = false;
			this.convolve_internal(targetCube, sourceCube, dstRGBM, srcRGBM, linear, camera, material, identity);
			this.maxExponent = num;
			this.generateMipChain = flag;
			SkyManager skyManager = SkyManager.Get();
			if (skyManager)
			{
				skyManager.GlobalSky = skyManager.GlobalSky;
			}
			UnityEngine.Object.DestroyImmediate(material);
			UnityEngine.Object.DestroyImmediate(gameObject);
		}

		// Token: 0x060013FF RID: 5119 RVA: 0x000732C4 File Offset: 0x000716C4
		public void convolve(Cubemap targetCube, Texture sourceCube, bool dstRGBM, bool srcRGBM, bool linear)
		{
			if (targetCube == null)
			{
				return;
			}
			GameObject gameObject = new GameObject("_temp_probe");
			gameObject.hideFlags = HideFlags.HideAndDontSave;
			gameObject.SetActive(true);
			Camera camera = gameObject.AddComponent<Camera>();
			camera.renderingPath = this.renderPath;
			camera.useOcclusionCulling = false;
			Material material = new Material(Shader.Find("Hidden/Marmoset/RGBM Cube"));
			Matrix4x4 identity = Matrix4x4.identity;
			this.copy_internal(targetCube, sourceCube, dstRGBM, srcRGBM, linear, camera, material, identity);
			int num = this.maxExponent;
			this.maxExponent = 2 * num;
			this.convolve_internal(targetCube, sourceCube, dstRGBM, srcRGBM, linear, camera, material, identity);
			this.maxExponent = 8 * num;
			this.convolve_internal(targetCube, targetCube, dstRGBM, dstRGBM, linear, camera, material, identity);
			this.maxExponent = num;
			SkyManager skyManager = SkyManager.Get();
			if (skyManager)
			{
				skyManager.GlobalSky = skyManager.GlobalSky;
			}
			UnityEngine.Object.DestroyImmediate(material);
			UnityEngine.Object.DestroyImmediate(gameObject);
		}

		// Token: 0x06001400 RID: 5120 RVA: 0x000733A8 File Offset: 0x000717A8
		public bool capture(Texture targetCube, Vector3 position, Quaternion rotation, bool HDR, bool linear, bool convolve)
		{
			if (targetCube == null)
			{
				return false;
			}
			bool flag = false;
			if (this.cubeRT == null)
			{
				flag = true;
				this.cubeRT = RenderTexture.GetTemporary(targetCube.width, targetCube.width, 24, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear);
				this.cubeRT.Release();
				this.cubeRT.dimension = TextureDimension.Cube;
				this.cubeRT.useMipMap = true;
				this.cubeRT.autoGenerateMips = true;
				this.cubeRT.Create();
				if (!this.cubeRT.IsCreated() && !this.cubeRT.Create())
				{
					this.cubeRT = RenderTexture.GetTemporary(targetCube.width, targetCube.width, 24, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
					this.cubeRT.Release();
					this.cubeRT.dimension = TextureDimension.Cube;
					this.cubeRT.useMipMap = true;
					this.cubeRT.autoGenerateMips = true;
					this.cubeRT.Create();
				}
			}
			if (!this.cubeRT.IsCreated() && !this.cubeRT.Create())
			{
				return false;
			}
			GameObject gameObject = new GameObject("_temp_probe");
			Camera camera = gameObject.AddComponent<Camera>();
			SkyManager skyManager = SkyManager.Get();
			if (skyManager && skyManager.ProbeCamera)
			{
				camera.CopyFrom(skyManager.ProbeCamera);
			}
			else if (Camera.main)
			{
				camera.CopyFrom(Camera.main);
			}
			camera.renderingPath = this.renderPath;
			camera.useOcclusionCulling = false;
			camera.allowHDR = true;
			gameObject.hideFlags = HideFlags.HideAndDontSave;
			gameObject.SetActive(true);
			gameObject.transform.position = position;
			Shader.SetGlobalVector("_UniformOcclusion", this.exposures);
			camera.RenderToCubemap(this.cubeRT);
			Shader.SetGlobalVector("_UniformOcclusion", Vector4.one);
			Matrix4x4 identity = Matrix4x4.identity;
			identity.SetTRS(position, rotation, Vector3.one);
			Material material = new Material(Shader.Find("Hidden/Marmoset/RGBM Cube"));
			bool srcRGBM = false;
			this.copy_internal(targetCube, this.cubeRT, HDR, srcRGBM, linear, camera, material, identity);
			if (convolve)
			{
				this.convolve_internal(targetCube, this.cubeRT, HDR, false, linear, camera, material, identity);
			}
			if (skyManager)
			{
				skyManager.GlobalSky = skyManager.GlobalSky;
			}
			UnityEngine.Object.DestroyImmediate(material);
			UnityEngine.Object.DestroyImmediate(gameObject);
			if (flag)
			{
				RenderTexture.ReleaseTemporary(this.cubeRT);
			}
			return true;
		}

		// Token: 0x06001401 RID: 5121 RVA: 0x00073622 File Offset: 0x00071A22
		private static void toggleKeywordPair(string on, string off, bool yes)
		{
			if (yes)
			{
				Shader.EnableKeyword(on);
				Shader.DisableKeyword(off);
			}
			else
			{
				Shader.EnableKeyword(off);
				Shader.DisableKeyword(on);
			}
		}

		// Token: 0x06001402 RID: 5122 RVA: 0x00073647 File Offset: 0x00071A47
		private static void toggleKeywordPair(Material mat, string on, string off, bool yes)
		{
			if (yes)
			{
				mat.EnableKeyword(on);
				mat.DisableKeyword(off);
			}
			else
			{
				mat.EnableKeyword(off);
				mat.DisableKeyword(on);
			}
		}

		// Token: 0x06001403 RID: 5123 RVA: 0x00073670 File Offset: 0x00071A70
		private void copy_internal(Texture dstCube, Texture srcCube, bool dstRGBM, bool srcRGBM, bool linear, Camera cam, Material skyMat, Matrix4x4 matrix)
		{
			bool allowHDR = cam.allowHDR;
			CameraClearFlags clearFlags = cam.clearFlags;
			int cullingMask = cam.cullingMask;
			cam.clearFlags = CameraClearFlags.Skybox;
			cam.cullingMask = 0;
			cam.allowHDR = !dstRGBM;
			skyMat.name = "Internal HDR to RGBM Skybox";
			skyMat.shader = Shader.Find("Hidden/Marmoset/RGBM Cube");
			SkyProbe.toggleKeywordPair("MARMO_RGBM_INPUT_ON", "MARMO_RGBM_INPUT_OFF", srcRGBM);
			SkyProbe.toggleKeywordPair("MARMO_RGBM_OUTPUT_ON", "MARMO_RGBM_OUTPUT_OFF", dstRGBM);
			skyMat.SetMatrix("_SkyMatrix", matrix);
			skyMat.SetTexture("_CubeHDR", srcCube);
			Material skybox = RenderSettings.skybox;
			RenderSettings.skybox = skyMat;
			RenderTexture renderTexture = dstCube as RenderTexture;
			Cubemap cubemap = dstCube as Cubemap;
			if (renderTexture)
			{
				cam.RenderToCubemap(renderTexture);
			}
			else if (cubemap)
			{
				cam.RenderToCubemap(cubemap);
			}
			cam.allowHDR = allowHDR;
			cam.clearFlags = clearFlags;
			cam.cullingMask = cullingMask;
			RenderSettings.skybox = skybox;
		}

		// Token: 0x06001404 RID: 5124 RVA: 0x00073778 File Offset: 0x00071B78
		private void convolve_internal(Texture dstTex, Texture srcCube, bool dstRGBM, bool srcRGBM, bool linear, Camera cam, Material skyMat, Matrix4x4 matrix)
		{
			bool allowHDR = cam.allowHDR;
			CameraClearFlags clearFlags = cam.clearFlags;
			int cullingMask = cam.cullingMask;
			cam.clearFlags = CameraClearFlags.Skybox;
			cam.cullingMask = 0;
			cam.allowHDR = !dstRGBM;
			skyMat.name = "Internal Convolve Skybox";
			skyMat.shader = Shader.Find("Hidden/Marmoset/RGBM Convolve");
			SkyProbe.toggleKeywordPair("MARMO_RGBM_INPUT_ON", "MARMO_RGBM_INPUT_OFF", srcRGBM);
			SkyProbe.toggleKeywordPair("MARMO_RGBM_OUTPUT_ON", "MARMO_RGBM_OUTPUT_OFF", dstRGBM);
			skyMat.SetMatrix("_SkyMatrix", matrix);
			skyMat.SetTexture("_CubeHDR", srcCube);
			SkyProbe.bindRandomValueTable(skyMat, "_PhongRands", srcCube.width);
			Material skybox = RenderSettings.skybox;
			RenderSettings.skybox = skyMat;
			Cubemap cubemap = dstTex as Cubemap;
			RenderTexture renderTexture = dstTex as RenderTexture;
			if (cubemap)
			{
				if (this.generateMipChain)
				{
					int num = QPow.Log2i(cubemap.width) - 1;
					for (int i = (!this.highestMipIsMirror) ? 0 : 1; i < num; i++)
					{
						int ext = 1 << num - i;
						float value = (float)QPow.clampedDownShift(this.maxExponent, (!this.highestMipIsMirror) ? i : (i - 1), 1);
						skyMat.SetFloat("_SpecularExp", value);
						skyMat.SetFloat("_SpecularScale", this.convolutionScale);
						Cubemap cubemap2 = new Cubemap(ext, cubemap.format, false);
						cam.RenderToCubemap(cubemap2);
						for (int j = 0; j < 6; j++)
						{
							CubemapFace face = (CubemapFace)j;
							cubemap.SetPixels(cubemap2.GetPixels(face), face, i);
						}
						UnityEngine.Object.DestroyImmediate(cubemap2);
					}
					cubemap.Apply(false);
				}
				else
				{
					skyMat.SetFloat("_SpecularExp", (float)this.maxExponent);
					skyMat.SetFloat("_SpecularScale", this.convolutionScale);
					cam.RenderToCubemap(cubemap);
				}
			}
			else if (renderTexture)
			{
				skyMat.SetFloat("_SpecularExp", (float)this.maxExponent);
				skyMat.SetFloat("_SpecularScale", this.convolutionScale);
				cam.RenderToCubemap(renderTexture);
			}
			cam.clearFlags = clearFlags;
			cam.cullingMask = cullingMask;
			cam.allowHDR = allowHDR;
			RenderSettings.skybox = skybox;
		}

		// Token: 0x06001405 RID: 5125 RVA: 0x000739CA File Offset: 0x00071DCA
		// Note: this type is marked as 'beforefieldinit'.
		static SkyProbe()
		{
		}

		// Token: 0x04001153 RID: 4435
		public RenderTexture cubeRT;

		// Token: 0x04001154 RID: 4436
		public int maxExponent = 512;

		// Token: 0x04001155 RID: 4437
		public Vector4 exposures = Vector4.one;

		// Token: 0x04001156 RID: 4438
		public bool generateMipChain = true;

		// Token: 0x04001157 RID: 4439
		public bool highestMipIsMirror = true;

		// Token: 0x04001158 RID: 4440
		public float convolutionScale = 1f;

		// Token: 0x04001159 RID: 4441
		public RenderingPath renderPath = RenderingPath.Forward;

		// Token: 0x0400115A RID: 4442
		private static int sampleCount = 128;

		// Token: 0x0400115B RID: 4443
		private static Vector4[] randomValues;
	}
}
