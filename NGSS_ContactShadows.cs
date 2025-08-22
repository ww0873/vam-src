using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000415 RID: 1045
[ImageEffectAllowedInSceneView]
[ExecuteInEditMode]
public class NGSS_ContactShadows : MonoBehaviour
{
	// Token: 0x06001A5F RID: 6751 RVA: 0x00093400 File Offset: 0x00091800
	public NGSS_ContactShadows()
	{
	}

	// Token: 0x170002F4 RID: 756
	// (get) Token: 0x06001A60 RID: 6752 RVA: 0x00093454 File Offset: 0x00091854
	private Camera mCamera
	{
		get
		{
			if (this._mCamera == null)
			{
				this._mCamera = base.GetComponent<Camera>();
				if (this._mCamera == null)
				{
					this._mCamera = Camera.main;
				}
				if (this._mCamera == null)
				{
					Debug.LogError("NGSS Error: No MainCamera found, please provide one.", this);
				}
				else
				{
					this._mCamera.depthTextureMode |= DepthTextureMode.Depth;
				}
			}
			return this._mCamera;
		}
	}

	// Token: 0x170002F5 RID: 757
	// (get) Token: 0x06001A61 RID: 6753 RVA: 0x000934D4 File Offset: 0x000918D4
	private Material mMaterial
	{
		get
		{
			if (this._mMaterial == null)
			{
				if (this.contactShadowsShader == null)
				{
					Shader.Find("Hidden/NGSS_ContactShadows");
				}
				this._mMaterial = new Material(this.contactShadowsShader);
				if (this._mMaterial == null)
				{
					Debug.LogWarning("NGSS Warning: can't find NGSS_ContactShadows shader, make sure it's on your project.", this);
					base.enabled = false;
					return null;
				}
			}
			return this._mMaterial;
		}
	}

	// Token: 0x06001A62 RID: 6754 RVA: 0x0009354C File Offset: 0x0009194C
	private void AddCommandBuffers()
	{
		this.computeShadowsCB = new CommandBuffer
		{
			name = "NGSS ContactShadows: Compute"
		};
		this.blendShadowsCB = new CommandBuffer
		{
			name = "NGSS ContactShadows: Mix"
		};
		bool flag = this.mCamera.actualRenderingPath == RenderingPath.Forward;
		if (this.mCamera)
		{
			foreach (CommandBuffer commandBuffer in this.mCamera.GetCommandBuffers((!flag) ? CameraEvent.BeforeLighting : CameraEvent.AfterDepthTexture))
			{
				if (commandBuffer.name == this.computeShadowsCB.name)
				{
					return;
				}
			}
			this.mCamera.AddCommandBuffer((!flag) ? CameraEvent.BeforeLighting : CameraEvent.AfterDepthTexture, this.computeShadowsCB);
		}
		if (this.mainDirectionalLight)
		{
			foreach (CommandBuffer commandBuffer2 in this.mainDirectionalLight.GetCommandBuffers(LightEvent.AfterScreenspaceMask))
			{
				if (commandBuffer2.name == this.blendShadowsCB.name)
				{
					return;
				}
			}
			this.mainDirectionalLight.AddCommandBuffer(LightEvent.AfterScreenspaceMask, this.blendShadowsCB);
		}
	}

	// Token: 0x06001A63 RID: 6755 RVA: 0x00093688 File Offset: 0x00091A88
	private void RemoveCommandBuffers()
	{
		this._mMaterial = null;
		bool flag = this.mCamera.actualRenderingPath == RenderingPath.Forward;
		if (this.mCamera)
		{
			this.mCamera.RemoveCommandBuffer((!flag) ? CameraEvent.BeforeLighting : CameraEvent.AfterDepthTexture, this.computeShadowsCB);
		}
		if (this.mainDirectionalLight)
		{
			this.mainDirectionalLight.RemoveCommandBuffer(LightEvent.AfterScreenspaceMask, this.blendShadowsCB);
		}
		this.isInitialized = false;
	}

	// Token: 0x06001A64 RID: 6756 RVA: 0x00093704 File Offset: 0x00091B04
	private void Init()
	{
		if (this.isInitialized || this.mainDirectionalLight == null)
		{
			return;
		}
		if (this.mCamera.renderingPath == RenderingPath.UsePlayerSettings || this.mCamera.renderingPath == RenderingPath.VertexLit)
		{
			Debug.LogWarning("Please set your camera rendering path to either Forward or Deferred and re-enable this component.", this);
			base.enabled = false;
			return;
		}
		this.AddCommandBuffers();
		int nameID = Shader.PropertyToID("NGSS_ContactShadowRT");
		int nameID2 = Shader.PropertyToID("NGSS_DepthSourceRT");
		this.computeShadowsCB.GetTemporaryRT(nameID, -1, -1, 0, FilterMode.Bilinear, RenderTextureFormat.R8);
		this.computeShadowsCB.GetTemporaryRT(nameID2, -1, -1, 0, FilterMode.Point, RenderTextureFormat.RFloat);
		this.computeShadowsCB.Blit(nameID, nameID2, this.mMaterial, 0);
		this.computeShadowsCB.Blit(nameID2, nameID, this.mMaterial, 1);
		this.computeShadowsCB.Blit(nameID, nameID2, this.mMaterial, 2);
		this.blendShadowsCB.Blit(BuiltinRenderTextureType.CurrentActive, BuiltinRenderTextureType.CurrentActive, this.mMaterial, 3);
		this.computeShadowsCB.SetGlobalTexture("NGSS_ContactShadowsTexture", nameID2);
		this.isInitialized = true;
	}

	// Token: 0x06001A65 RID: 6757 RVA: 0x00093837 File Offset: 0x00091C37
	private bool IsNotSupported()
	{
		return SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES2 || SystemInfo.graphicsDeviceType == GraphicsDeviceType.PlayStationVita || SystemInfo.graphicsDeviceType == GraphicsDeviceType.N3DS;
	}

	// Token: 0x06001A66 RID: 6758 RVA: 0x0009385C File Offset: 0x00091C5C
	private void OnEnable()
	{
		if (this.IsNotSupported())
		{
			Debug.LogWarning("Unsupported graphics API, NGSS requires at least SM3.0 or higher and DX9 is not supported.", this);
			base.enabled = false;
			return;
		}
		this.Init();
	}

	// Token: 0x06001A67 RID: 6759 RVA: 0x00093882 File Offset: 0x00091C82
	private void OnDisable()
	{
		if (this.isInitialized)
		{
			this.RemoveCommandBuffers();
		}
	}

	// Token: 0x06001A68 RID: 6760 RVA: 0x00093895 File Offset: 0x00091C95
	private void OnApplicationQuit()
	{
		if (this.isInitialized)
		{
			this.RemoveCommandBuffers();
		}
	}

	// Token: 0x06001A69 RID: 6761 RVA: 0x000938A8 File Offset: 0x00091CA8
	private void OnPreRender()
	{
		this.Init();
		if (!this.isInitialized || this.mainDirectionalLight == null)
		{
			return;
		}
		this.mMaterial.SetVector("LightDir", this.mCamera.transform.InverseTransformDirection(this.mainDirectionalLight.transform.forward));
		this.mMaterial.SetFloat("ShadowsOpacity", 1f - this.mainDirectionalLight.shadowStrength);
		this.mMaterial.SetFloat("ShadowsSoftness", this.shadowsSoftness);
		this.mMaterial.SetFloat("ShadowsDistance", this.shadowsDistance);
		this.mMaterial.SetFloat("ShadowsFade", this.shadowsFade);
		this.mMaterial.SetFloat("ShadowsBias", this.shadowsOffset * 0.02f);
		this.mMaterial.SetFloat("RayWidth", this.rayWidth);
		this.mMaterial.SetInt("RaySamples", this.raySamples);
		if (this.noiseFilter)
		{
			this.mMaterial.EnableKeyword("NGSS_CONTACT_SHADOWS_USE_NOISE");
		}
		else
		{
			this.mMaterial.DisableKeyword("NGSS_CONTACT_SHADOWS_USE_NOISE");
		}
	}

	// Token: 0x0400155B RID: 5467
	public Light mainDirectionalLight;

	// Token: 0x0400155C RID: 5468
	public Shader contactShadowsShader;

	// Token: 0x0400155D RID: 5469
	[Header("SHADOWS SETTINGS")]
	public bool noiseFilter;

	// Token: 0x0400155E RID: 5470
	[Range(0f, 3f)]
	public float shadowsSoftness = 1f;

	// Token: 0x0400155F RID: 5471
	[Range(1f, 4f)]
	public float shadowsDistance = 2f;

	// Token: 0x04001560 RID: 5472
	[Range(0.1f, 4f)]
	public float shadowsFade = 1f;

	// Token: 0x04001561 RID: 5473
	[Range(0f, 2f)]
	public float shadowsOffset = 0.325f;

	// Token: 0x04001562 RID: 5474
	[Range(0f, 1f)]
	public float rayWidth = 0.1f;

	// Token: 0x04001563 RID: 5475
	[Range(16f, 128f)]
	public int raySamples = 64;

	// Token: 0x04001564 RID: 5476
	private CommandBuffer blendShadowsCB;

	// Token: 0x04001565 RID: 5477
	private CommandBuffer computeShadowsCB;

	// Token: 0x04001566 RID: 5478
	private bool isInitialized;

	// Token: 0x04001567 RID: 5479
	private Camera _mCamera;

	// Token: 0x04001568 RID: 5480
	private Material _mMaterial;
}
