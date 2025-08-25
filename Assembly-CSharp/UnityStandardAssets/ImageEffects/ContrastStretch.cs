using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000E70 RID: 3696
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Color Adjustments/Contrast Stretch")]
	public class ContrastStretch : MonoBehaviour
	{
		// Token: 0x060070C3 RID: 28867 RVA: 0x002AB51C File Offset: 0x002A991C
		public ContrastStretch()
		{
		}

		// Token: 0x1700109B RID: 4251
		// (get) Token: 0x060070C4 RID: 28868 RVA: 0x002AB551 File Offset: 0x002A9951
		protected Material materialLum
		{
			get
			{
				if (this.m_materialLum == null)
				{
					this.m_materialLum = new Material(this.shaderLum);
					this.m_materialLum.hideFlags = HideFlags.HideAndDontSave;
				}
				return this.m_materialLum;
			}
		}

		// Token: 0x1700109C RID: 4252
		// (get) Token: 0x060070C5 RID: 28869 RVA: 0x002AB588 File Offset: 0x002A9988
		protected Material materialReduce
		{
			get
			{
				if (this.m_materialReduce == null)
				{
					this.m_materialReduce = new Material(this.shaderReduce);
					this.m_materialReduce.hideFlags = HideFlags.HideAndDontSave;
				}
				return this.m_materialReduce;
			}
		}

		// Token: 0x1700109D RID: 4253
		// (get) Token: 0x060070C6 RID: 28870 RVA: 0x002AB5BF File Offset: 0x002A99BF
		protected Material materialAdapt
		{
			get
			{
				if (this.m_materialAdapt == null)
				{
					this.m_materialAdapt = new Material(this.shaderAdapt);
					this.m_materialAdapt.hideFlags = HideFlags.HideAndDontSave;
				}
				return this.m_materialAdapt;
			}
		}

		// Token: 0x1700109E RID: 4254
		// (get) Token: 0x060070C7 RID: 28871 RVA: 0x002AB5F6 File Offset: 0x002A99F6
		protected Material materialApply
		{
			get
			{
				if (this.m_materialApply == null)
				{
					this.m_materialApply = new Material(this.shaderApply);
					this.m_materialApply.hideFlags = HideFlags.HideAndDontSave;
				}
				return this.m_materialApply;
			}
		}

		// Token: 0x060070C8 RID: 28872 RVA: 0x002AB630 File Offset: 0x002A9A30
		private void Start()
		{
			if (!SystemInfo.supportsImageEffects)
			{
				base.enabled = false;
				return;
			}
			if (!this.shaderAdapt.isSupported || !this.shaderApply.isSupported || !this.shaderLum.isSupported || !this.shaderReduce.isSupported)
			{
				base.enabled = false;
				return;
			}
		}

		// Token: 0x060070C9 RID: 28873 RVA: 0x002AB698 File Offset: 0x002A9A98
		private void OnEnable()
		{
			for (int i = 0; i < 2; i++)
			{
				if (!this.adaptRenderTex[i])
				{
					this.adaptRenderTex[i] = new RenderTexture(1, 1, 0);
					this.adaptRenderTex[i].hideFlags = HideFlags.HideAndDontSave;
				}
			}
		}

		// Token: 0x060070CA RID: 28874 RVA: 0x002AB6E8 File Offset: 0x002A9AE8
		private void OnDisable()
		{
			for (int i = 0; i < 2; i++)
			{
				UnityEngine.Object.DestroyImmediate(this.adaptRenderTex[i]);
				this.adaptRenderTex[i] = null;
			}
			if (this.m_materialLum)
			{
				UnityEngine.Object.DestroyImmediate(this.m_materialLum);
			}
			if (this.m_materialReduce)
			{
				UnityEngine.Object.DestroyImmediate(this.m_materialReduce);
			}
			if (this.m_materialAdapt)
			{
				UnityEngine.Object.DestroyImmediate(this.m_materialAdapt);
			}
			if (this.m_materialApply)
			{
				UnityEngine.Object.DestroyImmediate(this.m_materialApply);
			}
		}

		// Token: 0x060070CB RID: 28875 RVA: 0x002AB78C File Offset: 0x002A9B8C
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			RenderTexture renderTexture = RenderTexture.GetTemporary(source.width, source.height);
			Graphics.Blit(source, renderTexture, this.materialLum);
			while (renderTexture.width > 1 || renderTexture.height > 1)
			{
				int num = renderTexture.width / 2;
				if (num < 1)
				{
					num = 1;
				}
				int num2 = renderTexture.height / 2;
				if (num2 < 1)
				{
					num2 = 1;
				}
				RenderTexture temporary = RenderTexture.GetTemporary(num, num2);
				Graphics.Blit(renderTexture, temporary, this.materialReduce);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = temporary;
			}
			this.CalculateAdaptation(renderTexture);
			this.materialApply.SetTexture("_AdaptTex", this.adaptRenderTex[this.curAdaptIndex]);
			Graphics.Blit(source, destination, this.materialApply);
			RenderTexture.ReleaseTemporary(renderTexture);
		}

		// Token: 0x060070CC RID: 28876 RVA: 0x002AB850 File Offset: 0x002A9C50
		private void CalculateAdaptation(Texture curTexture)
		{
			int num = this.curAdaptIndex;
			this.curAdaptIndex = (this.curAdaptIndex + 1) % 2;
			float num2 = 1f - Mathf.Pow(1f - this.adaptationSpeed, 30f * Time.deltaTime);
			num2 = Mathf.Clamp(num2, 0.01f, 1f);
			this.materialAdapt.SetTexture("_CurTex", curTexture);
			this.materialAdapt.SetVector("_AdaptParams", new Vector4(num2, this.limitMinimum, this.limitMaximum, 0f));
			Graphics.SetRenderTarget(this.adaptRenderTex[this.curAdaptIndex]);
			GL.Clear(false, true, Color.black);
			Graphics.Blit(this.adaptRenderTex[num], this.adaptRenderTex[this.curAdaptIndex], this.materialAdapt);
		}

		// Token: 0x040063C1 RID: 25537
		[Range(0.0001f, 1f)]
		public float adaptationSpeed = 0.02f;

		// Token: 0x040063C2 RID: 25538
		[Range(0f, 1f)]
		public float limitMinimum = 0.2f;

		// Token: 0x040063C3 RID: 25539
		[Range(0f, 1f)]
		public float limitMaximum = 0.6f;

		// Token: 0x040063C4 RID: 25540
		private RenderTexture[] adaptRenderTex = new RenderTexture[2];

		// Token: 0x040063C5 RID: 25541
		private int curAdaptIndex;

		// Token: 0x040063C6 RID: 25542
		public Shader shaderLum;

		// Token: 0x040063C7 RID: 25543
		private Material m_materialLum;

		// Token: 0x040063C8 RID: 25544
		public Shader shaderReduce;

		// Token: 0x040063C9 RID: 25545
		private Material m_materialReduce;

		// Token: 0x040063CA RID: 25546
		public Shader shaderAdapt;

		// Token: 0x040063CB RID: 25547
		private Material m_materialAdapt;

		// Token: 0x040063CC RID: 25548
		public Shader shaderApply;

		// Token: 0x040063CD RID: 25549
		private Material m_materialApply;
	}
}
