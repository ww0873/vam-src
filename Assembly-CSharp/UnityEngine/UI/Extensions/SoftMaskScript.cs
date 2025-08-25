using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000504 RID: 1284
	[ExecuteInEditMode]
	[AddComponentMenu("UI/Effects/Extensions/SoftMaskScript")]
	public class SoftMaskScript : MonoBehaviour
	{
		// Token: 0x06002060 RID: 8288 RVA: 0x000B9363 File Offset: 0x000B7763
		public SoftMaskScript()
		{
		}

		// Token: 0x06002061 RID: 8289 RVA: 0x000B939C File Offset: 0x000B779C
		private void Start()
		{
			if (this.MaskArea == null)
			{
				this.MaskArea = base.GetComponent<RectTransform>();
			}
			Text component = base.GetComponent<Text>();
			if (component != null)
			{
				this.mat = new Material(Shader.Find("UI Extensions/SoftMaskShader"));
				component.material = this.mat;
				this.cachedCanvas = component.canvas;
				this.cachedCanvasTransform = this.cachedCanvas.transform;
				if (base.transform.parent.GetComponent<Mask>() == null)
				{
					base.transform.parent.gameObject.AddComponent<Mask>();
				}
				base.transform.parent.GetComponent<Mask>().enabled = false;
				return;
			}
			Graphic component2 = base.GetComponent<Graphic>();
			if (component2 != null)
			{
				this.mat = new Material(Shader.Find("UI Extensions/SoftMaskShader"));
				component2.material = this.mat;
				this.cachedCanvas = component2.canvas;
				this.cachedCanvasTransform = this.cachedCanvas.transform;
			}
		}

		// Token: 0x06002062 RID: 8290 RVA: 0x000B94B0 File Offset: 0x000B78B0
		private void Update()
		{
			if (this.cachedCanvas != null)
			{
				this.SetMask();
			}
		}

		// Token: 0x06002063 RID: 8291 RVA: 0x000B94CC File Offset: 0x000B78CC
		private void SetMask()
		{
			Rect canvasRect = this.GetCanvasRect();
			Vector2 size = canvasRect.size;
			this.maskScale.Set(1f / size.x, 1f / size.y);
			this.maskOffset = -canvasRect.min;
			this.maskOffset.Scale(this.maskScale);
			this.mat.SetTextureOffset("_AlphaMask", this.maskOffset);
			this.mat.SetTextureScale("_AlphaMask", this.maskScale);
			this.mat.SetTexture("_AlphaMask", this.AlphaMask);
			this.mat.SetFloat("_HardBlend", (float)((!this.HardBlend) ? 0 : 1));
			this.mat.SetInt("_FlipAlphaMask", (!this.FlipAlphaMask) ? 0 : 1);
			this.mat.SetInt("_NoOuterClip", (!this.DontClipMaskScalingRect) ? 0 : 1);
			this.mat.SetFloat("_CutOff", this.CutOff);
		}

		// Token: 0x06002064 RID: 8292 RVA: 0x000B95F0 File Offset: 0x000B79F0
		public Rect GetCanvasRect()
		{
			if (this.cachedCanvas == null)
			{
				return default(Rect);
			}
			this.MaskArea.GetWorldCorners(this.m_WorldCorners);
			for (int i = 0; i < 4; i++)
			{
				this.m_CanvasCorners[i] = this.cachedCanvasTransform.InverseTransformPoint(this.m_WorldCorners[i]);
			}
			return new Rect(this.m_CanvasCorners[0].x, this.m_CanvasCorners[0].y, this.m_CanvasCorners[2].x - this.m_CanvasCorners[0].x, this.m_CanvasCorners[2].y - this.m_CanvasCorners[0].y);
		}

		// Token: 0x04001B1C RID: 6940
		private Material mat;

		// Token: 0x04001B1D RID: 6941
		private Canvas cachedCanvas;

		// Token: 0x04001B1E RID: 6942
		private Transform cachedCanvasTransform;

		// Token: 0x04001B1F RID: 6943
		private readonly Vector3[] m_WorldCorners = new Vector3[4];

		// Token: 0x04001B20 RID: 6944
		private readonly Vector3[] m_CanvasCorners = new Vector3[4];

		// Token: 0x04001B21 RID: 6945
		[Tooltip("The area that is to be used as the container.")]
		public RectTransform MaskArea;

		// Token: 0x04001B22 RID: 6946
		[Tooltip("Texture to be used to do the soft alpha")]
		public Texture AlphaMask;

		// Token: 0x04001B23 RID: 6947
		[Tooltip("At what point to apply the alpha min range 0-1")]
		[Range(0f, 1f)]
		public float CutOff;

		// Token: 0x04001B24 RID: 6948
		[Tooltip("Implement a hard blend based on the Cutoff")]
		public bool HardBlend;

		// Token: 0x04001B25 RID: 6949
		[Tooltip("Flip the masks alpha value")]
		public bool FlipAlphaMask;

		// Token: 0x04001B26 RID: 6950
		[Tooltip("If a different Mask Scaling Rect is given, and this value is true, the area around the mask will not be clipped")]
		public bool DontClipMaskScalingRect;

		// Token: 0x04001B27 RID: 6951
		private Vector2 maskOffset = Vector2.zero;

		// Token: 0x04001B28 RID: 6952
		private Vector2 maskScale = Vector2.one;
	}
}
