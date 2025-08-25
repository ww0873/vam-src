using System;
using UnityEngine;

// Token: 0x02000312 RID: 786
[ExecuteInEditMode]
public class Water_DistortionAndBloom : MonoBehaviour
{
	// Token: 0x0600127C RID: 4732 RVA: 0x000673E4 File Offset: 0x000657E4
	public Water_DistortionAndBloom()
	{
	}

	// Token: 0x0600127D RID: 4733 RVA: 0x00067404 File Offset: 0x00065804
	public static Material CheckShaderAndCreateMaterial(Shader s)
	{
		if (s == null || !s.isSupported)
		{
			return null;
		}
		return new Material(s)
		{
			hideFlags = HideFlags.DontSave
		};
	}

	// Token: 0x0600127E RID: 4734 RVA: 0x0006743A File Offset: 0x0006583A
	private void OnDisable()
	{
		if (this.tempGO != null)
		{
			UnityEngine.Object.DestroyImmediate(this.tempGO);
		}
		Shader.DisableKeyword("DISTORT_OFF");
		Shader.DisableKeyword("_MOBILEDEPTH_ON");
	}

	// Token: 0x0600127F RID: 4735 RVA: 0x0006746C File Offset: 0x0006586C
	private void Start()
	{
		this.InitializeRenderTarget();
	}

	// Token: 0x06001280 RID: 4736 RVA: 0x00067474 File Offset: 0x00065874
	private void LateUpdate()
	{
		if (this.previuosFrameWidth != Screen.width || this.previuosFrameHeight != Screen.height || Mathf.Abs(this.previousScale - this.RenderTextureResolutoinFactor) > 0.01f)
		{
			this.InitializeRenderTarget();
			this.previuosFrameWidth = Screen.width;
			this.previuosFrameHeight = Screen.height;
			this.previousScale = this.RenderTextureResolutoinFactor;
		}
		Shader.EnableKeyword("DISTORT_OFF");
		Shader.EnableKeyword("_MOBILEDEPTH_ON");
		this.GrabImage();
		Shader.SetGlobalTexture("_GrabTexture", this.source);
		Shader.SetGlobalTexture("_CameraDepthTexture", this.depth);
		Shader.SetGlobalFloat("_GrabTextureScale", this.RenderTextureResolutoinFactor);
		Shader.DisableKeyword("DISTORT_OFF");
	}

	// Token: 0x06001281 RID: 4737 RVA: 0x0006753C File Offset: 0x0006593C
	private void InitializeRenderTarget()
	{
		int width = (int)((float)Screen.width * this.RenderTextureResolutoinFactor);
		int height = (int)((float)Screen.height * this.RenderTextureResolutoinFactor);
		this.source = new RenderTexture(width, height, 0, RenderTextureFormat.RGB565);
		this.depth = new RenderTexture(width, height, 8, RenderTextureFormat.Depth);
	}

	// Token: 0x06001282 RID: 4738 RVA: 0x00067588 File Offset: 0x00065988
	private void GrabImage()
	{
		Camera camera = Camera.current;
		if (camera == null)
		{
			camera = Camera.main;
		}
		if (this.tempGO == null)
		{
			this.tempGO = new GameObject();
			this.tempGO.hideFlags = HideFlags.HideAndDontSave;
			this.addCamera = this.tempGO.AddComponent<Camera>();
			this.addCamera.enabled = false;
		}
		else
		{
			this.addCamera = this.tempGO.GetComponent<Camera>();
		}
		this.addCamera.CopyFrom(camera);
		this.addCamera.SetTargetBuffers(this.source.colorBuffer, this.depth.depthBuffer);
		this.addCamera.depth -= 1f;
		this.addCamera.cullingMask = this.CullingMask;
		this.addCamera.Render();
	}

	// Token: 0x04000FF4 RID: 4084
	[Range(0.05f, 1f)]
	[Tooltip("Camera render texture resolution")]
	public float RenderTextureResolutoinFactor = 0.5f;

	// Token: 0x04000FF5 RID: 4085
	public LayerMask CullingMask = -17;

	// Token: 0x04000FF6 RID: 4086
	private RenderTexture source;

	// Token: 0x04000FF7 RID: 4087
	private RenderTexture depth;

	// Token: 0x04000FF8 RID: 4088
	private RenderTexture destination;

	// Token: 0x04000FF9 RID: 4089
	private int previuosFrameWidth;

	// Token: 0x04000FFA RID: 4090
	private int previuosFrameHeight;

	// Token: 0x04000FFB RID: 4091
	private float previousScale;

	// Token: 0x04000FFC RID: 4092
	private Camera addCamera;

	// Token: 0x04000FFD RID: 4093
	private GameObject tempGO;

	// Token: 0x04000FFE RID: 4094
	private const int kMaxIterations = 16;
}
