using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x02000901 RID: 2305
public class OVROverlay : MonoBehaviour
{
	// Token: 0x06003A08 RID: 14856 RVA: 0x0011B424 File Offset: 0x00119824
	public OVROverlay()
	{
	}

	// Token: 0x06003A09 RID: 14857 RVA: 0x0011B498 File Offset: 0x00119898
	public void OverrideOverlayTextureInfo(Texture srcTexture, IntPtr nativePtr, XRNode node)
	{
		int num = (node != XRNode.RightEye) ? 0 : 1;
		if (this.textures.Length <= num)
		{
			return;
		}
		this.textures[num] = srcTexture;
		this.texturePtrs[num] = nativePtr;
		this.isOverridePending = true;
	}

	// Token: 0x1700065B RID: 1627
	// (get) Token: 0x06003A0A RID: 14858 RVA: 0x0011B4E1 File Offset: 0x001198E1
	private OVRPlugin.LayerLayout layout
	{
		get
		{
			return OVRPlugin.LayerLayout.Mono;
		}
	}

	// Token: 0x1700065C RID: 1628
	// (get) Token: 0x06003A0B RID: 14859 RVA: 0x0011B4E4 File Offset: 0x001198E4
	private int texturesPerStage
	{
		get
		{
			return (this.layout != OVRPlugin.LayerLayout.Stereo) ? 1 : 2;
		}
	}

	// Token: 0x06003A0C RID: 14860 RVA: 0x0011B4F8 File Offset: 0x001198F8
	private bool CreateLayer(int mipLevels, int sampleCount, OVRPlugin.EyeTextureFormat etFormat, int flags, OVRPlugin.Sizei size, OVRPlugin.OverlayShape shape)
	{
		if (!this.layerIdHandle.IsAllocated || this.layerIdPtr == IntPtr.Zero)
		{
			this.layerIdHandle = GCHandle.Alloc(this.layerId, GCHandleType.Pinned);
			this.layerIdPtr = this.layerIdHandle.AddrOfPinnedObject();
		}
		if (this.layerIndex == -1)
		{
			for (int i = 0; i < 15; i++)
			{
				if (OVROverlay.instances[i] == null || OVROverlay.instances[i] == this)
				{
					this.layerIndex = i;
					OVROverlay.instances[i] = this;
					break;
				}
			}
		}
		if (!this.isOverridePending && this.layerDesc.MipLevels == mipLevels && this.layerDesc.SampleCount == sampleCount && this.layerDesc.Format == etFormat && this.layerDesc.Layout == this.layout && this.layerDesc.LayerFlags == flags && this.layerDesc.TextureSize.Equals(size) && this.layerDesc.Shape == shape)
		{
			return false;
		}
		OVRPlugin.LayerDesc desc = OVRPlugin.CalculateLayerDesc(shape, this.layout, size, mipLevels, sampleCount, etFormat, flags);
		OVRPlugin.EnqueueSetupLayer(desc, this.layerIdPtr);
		this.layerId = (int)this.layerIdHandle.Target;
		if (this.layerId > 0)
		{
			this.layerDesc = desc;
			this.stageCount = OVRPlugin.GetLayerTextureStageCount(this.layerId);
		}
		this.isOverridePending = false;
		return true;
	}

	// Token: 0x06003A0D RID: 14861 RVA: 0x0011B6B4 File Offset: 0x00119AB4
	private bool CreateLayerTextures(bool useMipmaps, OVRPlugin.Sizei size, bool isHdr)
	{
		bool result = false;
		if (this.stageCount <= 0)
		{
			return false;
		}
		if (this.layerTextures == null)
		{
			this.frameIndex = 0;
			this.layerTextures = new OVROverlay.LayerTexture[this.texturesPerStage];
		}
		for (int i = 0; i < this.texturesPerStage; i++)
		{
			if (this.layerTextures[i].swapChain == null)
			{
				this.layerTextures[i].swapChain = new Texture[this.stageCount];
			}
			if (this.layerTextures[i].swapChainPtr == null)
			{
				this.layerTextures[i].swapChainPtr = new IntPtr[this.stageCount];
			}
			for (int j = 0; j < this.stageCount; j++)
			{
				Texture texture = this.layerTextures[i].swapChain[j];
				IntPtr intPtr = this.layerTextures[i].swapChainPtr[j];
				if (!(texture != null) || !(intPtr != IntPtr.Zero))
				{
					if (intPtr == IntPtr.Zero)
					{
						intPtr = OVRPlugin.GetLayerTexture(this.layerId, j, (OVRPlugin.Eye)i);
					}
					if (!(intPtr == IntPtr.Zero))
					{
						TextureFormat format = (!isHdr) ? TextureFormat.RGBA32 : TextureFormat.RGBAHalf;
						if (this.currentOverlayShape != OVROverlay.OverlayShape.Cubemap && this.currentOverlayShape != OVROverlay.OverlayShape.OffcenterCubemap)
						{
							texture = Texture2D.CreateExternalTexture(size.w, size.h, format, useMipmaps, true, intPtr);
						}
						else
						{
							texture = Cubemap.CreateExternalTexture(size.w, format, useMipmaps, intPtr);
						}
						this.layerTextures[i].swapChain[j] = texture;
						this.layerTextures[i].swapChainPtr[j] = intPtr;
						result = true;
					}
				}
			}
		}
		return result;
	}

	// Token: 0x06003A0E RID: 14862 RVA: 0x0011B894 File Offset: 0x00119C94
	private void DestroyLayerTextures()
	{
		int num = 0;
		while (this.layerTextures != null && num < this.texturesPerStage)
		{
			if (this.layerTextures[num].swapChain != null)
			{
				for (int i = 0; i < this.stageCount; i++)
				{
					UnityEngine.Object.DestroyImmediate(this.layerTextures[num].swapChain[i]);
				}
			}
			num++;
		}
		this.layerTextures = null;
	}

	// Token: 0x06003A0F RID: 14863 RVA: 0x0011B910 File Offset: 0x00119D10
	private void DestroyLayer()
	{
		if (this.layerIndex != -1)
		{
			OVRPlugin.EnqueueSubmitLayer(true, false, IntPtr.Zero, IntPtr.Zero, -1, 0, OVRPose.identity.ToPosef(), Vector3.one.ToVector3f(), this.layerIndex, (OVRPlugin.OverlayShape)this.prevOverlayShape);
			OVROverlay.instances[this.layerIndex] = null;
			this.layerIndex = -1;
		}
		if (this.layerIdPtr != IntPtr.Zero)
		{
			OVRPlugin.EnqueueDestroyLayer(this.layerIdPtr);
			this.layerIdPtr = IntPtr.Zero;
			this.layerIdHandle.Free();
			this.layerId = 0;
		}
		this.layerDesc = default(OVRPlugin.LayerDesc);
	}

	// Token: 0x06003A10 RID: 14864 RVA: 0x0011B9C4 File Offset: 0x00119DC4
	private bool LatchLayerTextures()
	{
		for (int i = 0; i < this.texturesPerStage; i++)
		{
			if ((this.textures[i] != this.layerTextures[i].appTexture || this.layerTextures[i].appTexturePtr == IntPtr.Zero) && this.textures[i] != null)
			{
				RenderTexture renderTexture = this.textures[i] as RenderTexture;
				if (renderTexture && !renderTexture.IsCreated())
				{
					renderTexture.Create();
				}
				this.layerTextures[i].appTexturePtr = ((!(this.texturePtrs[i] != IntPtr.Zero)) ? this.textures[i].GetNativeTexturePtr() : this.texturePtrs[i]);
				if (this.layerTextures[i].appTexturePtr != IntPtr.Zero)
				{
					this.layerTextures[i].appTexture = this.textures[i];
				}
			}
			if (this.currentOverlayShape == OVROverlay.OverlayShape.Cubemap && this.textures[i] as Cubemap == null)
			{
				Debug.LogError("Need Cubemap texture for cube map overlay");
				return false;
			}
		}
		if (this.currentOverlayShape == OVROverlay.OverlayShape.OffcenterCubemap)
		{
			Debug.LogWarning("Overlay shape " + this.currentOverlayShape + " is not supported on current platform");
			return false;
		}
		return !(this.layerTextures[0].appTexture == null) && !(this.layerTextures[0].appTexturePtr == IntPtr.Zero);
	}

	// Token: 0x06003A11 RID: 14865 RVA: 0x0011BB80 File Offset: 0x00119F80
	private OVRPlugin.LayerDesc GetCurrentLayerDesc()
	{
		OVRPlugin.LayerDesc result = new OVRPlugin.LayerDesc
		{
			Format = OVRPlugin.EyeTextureFormat.Default,
			LayerFlags = 8,
			Layout = this.layout,
			MipLevels = 1,
			SampleCount = 1,
			Shape = (OVRPlugin.OverlayShape)this.currentOverlayShape,
			TextureSize = new OVRPlugin.Sizei
			{
				w = this.textures[0].width,
				h = this.textures[0].height
			}
		};
		Texture2D texture2D = this.textures[0] as Texture2D;
		if (texture2D != null)
		{
			if (texture2D.format == TextureFormat.RGBAHalf || texture2D.format == TextureFormat.RGBAFloat)
			{
				result.Format = OVRPlugin.EyeTextureFormat.R16G16B16A16_FP;
			}
			result.MipLevels = texture2D.mipmapCount;
		}
		Cubemap cubemap = this.textures[0] as Cubemap;
		if (cubemap != null)
		{
			if (cubemap.format == TextureFormat.RGBAHalf || cubemap.format == TextureFormat.RGBAFloat)
			{
				result.Format = OVRPlugin.EyeTextureFormat.R16G16B16A16_FP;
			}
			result.MipLevels = cubemap.mipmapCount;
		}
		RenderTexture renderTexture = this.textures[0] as RenderTexture;
		if (renderTexture != null)
		{
			result.SampleCount = renderTexture.antiAliasing;
			if (renderTexture.format == RenderTextureFormat.ARGBHalf || renderTexture.format == RenderTextureFormat.ARGBFloat || renderTexture.format == RenderTextureFormat.RGB111110Float)
			{
				result.Format = OVRPlugin.EyeTextureFormat.R16G16B16A16_FP;
			}
		}
		if (this.isProtectedContent)
		{
			result.LayerFlags |= 64;
		}
		return result;
	}

	// Token: 0x06003A12 RID: 14866 RVA: 0x0011BD18 File Offset: 0x0011A118
	private bool PopulateLayer(int mipLevels, bool isHdr, OVRPlugin.Sizei size, int sampleCount)
	{
		bool result = false;
		RenderTextureFormat colorFormat = (!isHdr) ? RenderTextureFormat.ARGB32 : RenderTextureFormat.ARGBHalf;
		for (int i = 0; i < this.texturesPerStage; i++)
		{
			int num = this.frameIndex % this.stageCount;
			Texture texture = this.layerTextures[i].swapChain[num];
			if (!(texture == null))
			{
				for (int j = 0; j < mipLevels; j++)
				{
					int num2 = size.w >> j;
					if (num2 < 1)
					{
						num2 = 1;
					}
					int num3 = size.h >> j;
					if (num3 < 1)
					{
						num3 = 1;
					}
					RenderTexture temporary = RenderTexture.GetTemporary(new RenderTextureDescriptor(num2, num3, colorFormat, 0)
					{
						msaaSamples = sampleCount,
						useMipMap = true,
						autoGenerateMips = false,
						sRGB = false
					});
					if (!temporary.IsCreated())
					{
						temporary.Create();
					}
					temporary.DiscardContents();
					RenderTexture x = this.textures[i] as RenderTexture;
					bool flag = isHdr || (x != null && QualitySettings.activeColorSpace == ColorSpace.Linear);
					if (this.currentOverlayShape != OVROverlay.OverlayShape.Cubemap && this.currentOverlayShape != OVROverlay.OverlayShape.OffcenterCubemap)
					{
						OVROverlay.tex2DMaterial.SetInt("_linearToSrgb", (isHdr || !flag) ? 0 : 1);
						OVROverlay.tex2DMaterial.SetInt("_premultiply", 1);
						Graphics.Blit(this.textures[i], temporary, OVROverlay.tex2DMaterial);
						Graphics.CopyTexture(temporary, 0, 0, texture, 0, j);
					}
					else
					{
						for (int k = 0; k < 6; k++)
						{
							OVROverlay.cubeMaterial.SetInt("_linearToSrgb", (isHdr || !flag) ? 0 : 1);
							OVROverlay.cubeMaterial.SetInt("_premultiply", 1);
							OVROverlay.cubeMaterial.SetInt("_face", k);
							Graphics.Blit(this.textures[i], temporary, OVROverlay.cubeMaterial);
							Graphics.CopyTexture(temporary, 0, 0, texture, k, j);
						}
					}
					RenderTexture.ReleaseTemporary(temporary);
					result = true;
				}
			}
		}
		return result;
	}

	// Token: 0x06003A13 RID: 14867 RVA: 0x0011BF48 File Offset: 0x0011A348
	private bool SubmitLayer(bool overlay, bool headLocked, OVRPose pose, Vector3 scale)
	{
		int num = (this.texturesPerStage < 2) ? 0 : 1;
		bool result = OVRPlugin.EnqueueSubmitLayer(overlay, headLocked, this.layerTextures[0].appTexturePtr, this.layerTextures[num].appTexturePtr, this.layerId, this.frameIndex, pose.flipZ().ToPosef(), scale.ToVector3f(), this.layerIndex, (OVRPlugin.OverlayShape)this.currentOverlayShape);
		if (this.isDynamic)
		{
			this.frameIndex++;
		}
		this.prevOverlayShape = this.currentOverlayShape;
		return result;
	}

	// Token: 0x06003A14 RID: 14868 RVA: 0x0011BFE8 File Offset: 0x0011A3E8
	private void Awake()
	{
		Debug.Log("Overlay Awake");
		if (OVROverlay.tex2DMaterial == null)
		{
			OVROverlay.tex2DMaterial = new Material(Shader.Find("Oculus/Texture2D Blit"));
		}
		if (OVROverlay.cubeMaterial == null)
		{
			OVROverlay.cubeMaterial = new Material(Shader.Find("Oculus/Cubemap Blit"));
		}
		this.rend = base.GetComponent<Renderer>();
		if (this.textures.Length == 0)
		{
			this.textures = new Texture[1];
		}
		if (this.rend != null && this.textures[0] == null)
		{
			this.textures[0] = this.rend.material.mainTexture;
		}
	}

	// Token: 0x06003A15 RID: 14869 RVA: 0x0011C0A8 File Offset: 0x0011A4A8
	private void OnEnable()
	{
		if (!OVRManager.isHmdPresent)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06003A16 RID: 14870 RVA: 0x0011C0BC File Offset: 0x0011A4BC
	private void OnDisable()
	{
		this.DestroyLayerTextures();
		this.DestroyLayer();
	}

	// Token: 0x06003A17 RID: 14871 RVA: 0x0011C0CA File Offset: 0x0011A4CA
	private void OnDestroy()
	{
		this.DestroyLayerTextures();
		this.DestroyLayer();
	}

	// Token: 0x06003A18 RID: 14872 RVA: 0x0011C0D8 File Offset: 0x0011A4D8
	private bool ComputeSubmit(ref OVRPose pose, ref Vector3 scale, ref bool overlay, ref bool headLocked)
	{
		Camera main = Camera.main;
		overlay = (this.currentOverlayType == OVROverlay.OverlayType.Overlay);
		headLocked = false;
		Transform transform = base.transform;
		while (transform != null && !headLocked)
		{
			headLocked |= (transform == main.transform);
			transform = transform.parent;
		}
		pose = ((!headLocked) ? base.transform.ToTrackingSpacePose(main) : base.transform.ToHeadSpacePose(main));
		scale = base.transform.lossyScale;
		for (int i = 0; i < 3; i++)
		{
			ref Vector3 ptr = ref scale;
			int index;
			scale[index = i] = ptr[index] / main.transform.lossyScale[i];
		}
		if (this.currentOverlayShape == OVROverlay.OverlayShape.Cubemap)
		{
			pose.position = main.transform.position;
		}
		if (this.currentOverlayShape == OVROverlay.OverlayShape.OffcenterCubemap)
		{
			pose.position = base.transform.position;
			if (pose.position.magnitude > 1f)
			{
				Debug.LogWarning("Your cube map center offset's magnitude is greater than 1, which will cause some cube map pixel always invisible .");
				return false;
			}
		}
		if (this.currentOverlayShape == OVROverlay.OverlayShape.Cylinder)
		{
			float num = scale.x / scale.z / 3.1415927f * 180f;
			if (num > 180f)
			{
				Debug.LogWarning("Cylinder overlay's arc angle has to be below 180 degree, current arc angle is " + num + " degree.");
				return false;
			}
		}
		return true;
	}

	// Token: 0x06003A19 RID: 14873 RVA: 0x0011C258 File Offset: 0x0011A658
	private void LateUpdate()
	{
		if (this.currentOverlayType == OVROverlay.OverlayType.None || this.textures.Length < this.texturesPerStage || this.textures[0] == null)
		{
			return;
		}
		if (Time.frameCount <= this.prevFrameIndex)
		{
			return;
		}
		this.prevFrameIndex = Time.frameCount;
		OVRPose identity = OVRPose.identity;
		Vector3 one = Vector3.one;
		bool overlay = false;
		bool headLocked = false;
		if (!this.ComputeSubmit(ref identity, ref one, ref overlay, ref headLocked))
		{
			return;
		}
		OVRPlugin.LayerDesc currentLayerDesc = this.GetCurrentLayerDesc();
		bool isHdr = currentLayerDesc.Format == OVRPlugin.EyeTextureFormat.R16G16B16A16_FP;
		bool flag = this.CreateLayer(currentLayerDesc.MipLevels, currentLayerDesc.SampleCount, currentLayerDesc.Format, currentLayerDesc.LayerFlags, currentLayerDesc.TextureSize, currentLayerDesc.Shape);
		if (this.layerIndex == -1 || this.layerId <= 0)
		{
			return;
		}
		bool useMipmaps = currentLayerDesc.MipLevels > 1;
		flag |= this.CreateLayerTextures(useMipmaps, currentLayerDesc.TextureSize, isHdr);
		if (this.layerTextures[0].appTexture as RenderTexture != null)
		{
			this.isDynamic = true;
		}
		if (!this.LatchLayerTextures())
		{
			return;
		}
		if (!this.PopulateLayer(currentLayerDesc.MipLevels, isHdr, currentLayerDesc.TextureSize, currentLayerDesc.SampleCount))
		{
			return;
		}
		bool flag2 = this.SubmitLayer(overlay, headLocked, identity, one);
		if (this.rend)
		{
			this.rend.enabled = !flag2;
		}
	}

	// Token: 0x06003A1A RID: 14874 RVA: 0x0011C3DB File Offset: 0x0011A7DB
	// Note: this type is marked as 'beforefieldinit'.
	static OVROverlay()
	{
	}

	// Token: 0x04002B90 RID: 11152
	public OVROverlay.OverlayType currentOverlayType = OVROverlay.OverlayType.Overlay;

	// Token: 0x04002B91 RID: 11153
	public bool isDynamic;

	// Token: 0x04002B92 RID: 11154
	public bool isProtectedContent;

	// Token: 0x04002B93 RID: 11155
	public OVROverlay.OverlayShape currentOverlayShape;

	// Token: 0x04002B94 RID: 11156
	private OVROverlay.OverlayShape prevOverlayShape;

	// Token: 0x04002B95 RID: 11157
	public Texture[] textures = new Texture[2];

	// Token: 0x04002B96 RID: 11158
	protected IntPtr[] texturePtrs = new IntPtr[]
	{
		IntPtr.Zero,
		IntPtr.Zero
	};

	// Token: 0x04002B97 RID: 11159
	protected bool isOverridePending;

	// Token: 0x04002B98 RID: 11160
	internal const int maxInstances = 15;

	// Token: 0x04002B99 RID: 11161
	internal static OVROverlay[] instances = new OVROverlay[15];

	// Token: 0x04002B9A RID: 11162
	private static Material tex2DMaterial;

	// Token: 0x04002B9B RID: 11163
	private static Material cubeMaterial;

	// Token: 0x04002B9C RID: 11164
	private OVROverlay.LayerTexture[] layerTextures;

	// Token: 0x04002B9D RID: 11165
	private OVRPlugin.LayerDesc layerDesc;

	// Token: 0x04002B9E RID: 11166
	private int stageCount = -1;

	// Token: 0x04002B9F RID: 11167
	private int layerIndex = -1;

	// Token: 0x04002BA0 RID: 11168
	private int layerId;

	// Token: 0x04002BA1 RID: 11169
	private GCHandle layerIdHandle;

	// Token: 0x04002BA2 RID: 11170
	private IntPtr layerIdPtr = IntPtr.Zero;

	// Token: 0x04002BA3 RID: 11171
	private int frameIndex;

	// Token: 0x04002BA4 RID: 11172
	private int prevFrameIndex = -1;

	// Token: 0x04002BA5 RID: 11173
	private Renderer rend;

	// Token: 0x02000902 RID: 2306
	public enum OverlayShape
	{
		// Token: 0x04002BA7 RID: 11175
		Quad,
		// Token: 0x04002BA8 RID: 11176
		Cylinder,
		// Token: 0x04002BA9 RID: 11177
		Cubemap,
		// Token: 0x04002BAA RID: 11178
		OffcenterCubemap = 4,
		// Token: 0x04002BAB RID: 11179
		Equirect
	}

	// Token: 0x02000903 RID: 2307
	public enum OverlayType
	{
		// Token: 0x04002BAD RID: 11181
		None,
		// Token: 0x04002BAE RID: 11182
		Underlay,
		// Token: 0x04002BAF RID: 11183
		Overlay
	}

	// Token: 0x02000904 RID: 2308
	private struct LayerTexture
	{
		// Token: 0x04002BB0 RID: 11184
		public Texture appTexture;

		// Token: 0x04002BB1 RID: 11185
		public IntPtr appTexturePtr;

		// Token: 0x04002BB2 RID: 11186
		public Texture[] swapChain;

		// Token: 0x04002BB3 RID: 11187
		public IntPtr[] swapChainPtr;
	}
}
