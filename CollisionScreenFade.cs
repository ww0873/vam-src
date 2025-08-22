using System;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

// Token: 0x02000D3B RID: 3387
public class CollisionScreenFade : MonoBehaviour
{
	// Token: 0x06006787 RID: 26503 RVA: 0x0026EDD0 File Offset: 0x0026D1D0
	public CollisionScreenFade()
	{
	}

	// Token: 0x06006788 RID: 26504 RVA: 0x0026EE38 File Offset: 0x0026D238
	private void Awake()
	{
		this.fadeMaterial = ((!(this.fadeShader != null)) ? new Material(Shader.Find("Transparent/Diffuse")) : new Material(this.fadeShader));
		this.blurEffect = base.GetComponent<Blur>();
	}

	// Token: 0x06006789 RID: 26505 RVA: 0x0026EE87 File Offset: 0x0026D287
	private void OnDestroy()
	{
		if (this.fadeMaterial != null)
		{
			UnityEngine.Object.Destroy(this.fadeMaterial);
		}
	}

	// Token: 0x0600678A RID: 26506 RVA: 0x0026EEA5 File Offset: 0x0026D2A5
	private void OnCollisionStay()
	{
		this.isFadingOut = true;
	}

	// Token: 0x0600678B RID: 26507 RVA: 0x0026EEAE File Offset: 0x0026D2AE
	private void OnCollisionExit()
	{
		this.isFadingOut = false;
	}

	// Token: 0x0600678C RID: 26508 RVA: 0x0026EEB8 File Offset: 0x0026D2B8
	private void FixedUpdate()
	{
		if (this.isFadingOut)
		{
			float num = this.fadeOutRate * Time.fixedDeltaTime / Time.timeScale;
			this.fadeColor.a = this.fadeColor.a + num * this.fadeAlpha;
			this.fadeColor.a = Mathf.Clamp(this.fadeColor.a, 0f, this.fadeAlpha);
			this.fadeMaterial.color = this.fadeColor;
			if (this.useBlur && this.blurEffect != null)
			{
				this.blurEffect.enabled = true;
				this.blurEffect.blurSpread += num * this.maxBlurSpread;
				this.blurEffect.blurSpread = Mathf.Clamp(this.blurEffect.blurSpread, 0f, this.maxBlurSpread);
			}
			this.isFading = true;
		}
		else if (this.fadeColor.a != 0f)
		{
			float num2 = this.fadeInRate * Time.fixedDeltaTime / Time.timeScale;
			this.fadeColor.a = this.fadeColor.a - num2 * this.fadeAlpha;
			this.fadeColor.a = Mathf.Clamp01(this.fadeColor.a);
			this.fadeMaterial.color = this.fadeColor;
			if (this.useBlur && this.blurEffect != null)
			{
				this.blurEffect.enabled = true;
				this.blurEffect.blurSpread -= num2 * this.maxBlurSpread;
				this.blurEffect.blurSpread = Mathf.Clamp(this.blurEffect.blurSpread, 0f, this.maxBlurSpread);
			}
			this.isFading = true;
		}
		else
		{
			if (this.useBlur && this.blurEffect != null)
			{
				this.blurEffect.blurSpread = 0f;
				this.blurEffect.enabled = false;
			}
			this.isFading = false;
		}
	}

	// Token: 0x0600678D RID: 26509 RVA: 0x0026F0CC File Offset: 0x0026D4CC
	private void OnPostRender()
	{
		if (this.isFading)
		{
			this.fadeMaterial.SetPass(0);
			GL.PushMatrix();
			GL.LoadOrtho();
			GL.Color(this.fadeMaterial.color);
			GL.Begin(7);
			GL.Vertex3(0f, 0f, -12f);
			GL.Vertex3(0f, 1f, -12f);
			GL.Vertex3(1f, 1f, -12f);
			GL.Vertex3(1f, 0f, -12f);
			GL.End();
			GL.PopMatrix();
		}
	}

	// Token: 0x0400589B RID: 22683
	public Color fadeColor = new Color(0.01f, 0.01f, 0.01f, 0f);

	// Token: 0x0400589C RID: 22684
	public Shader fadeShader;

	// Token: 0x0400589D RID: 22685
	public float fadeAlpha = 0.2f;

	// Token: 0x0400589E RID: 22686
	public float fadeOutRate = 3f;

	// Token: 0x0400589F RID: 22687
	public float fadeInRate = 3f;

	// Token: 0x040058A0 RID: 22688
	public bool useBlur = true;

	// Token: 0x040058A1 RID: 22689
	public float maxBlurSpread = 1f;

	// Token: 0x040058A2 RID: 22690
	private Material fadeMaterial;

	// Token: 0x040058A3 RID: 22691
	private bool isFading;

	// Token: 0x040058A4 RID: 22692
	private bool isFadingOut;

	// Token: 0x040058A5 RID: 22693
	private Blur blurEffect;
}
