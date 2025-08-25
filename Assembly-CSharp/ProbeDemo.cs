using System;
using mset;
using UnityEngine;

// Token: 0x0200031B RID: 795
public class ProbeDemo : MonoBehaviour
{
	// Token: 0x060012B2 RID: 4786 RVA: 0x0006A93C File Offset: 0x00068D3C
	public ProbeDemo()
	{
	}

	// Token: 0x060012B3 RID: 4787 RVA: 0x0006A9B0 File Offset: 0x00068DB0
	private void Start()
	{
	}

	// Token: 0x060012B4 RID: 4788 RVA: 0x0006A9B4 File Offset: 0x00068DB4
	private void Update()
	{
		SkyManager skyManager = SkyManager.Get();
		if (skyManager && skyManager.GlobalSky)
		{
			Sky globalSky = skyManager.GlobalSky;
			if (Input.GetKeyDown(KeyCode.S))
			{
				this.spinning = !this.spinning;
			}
			if (Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.KeypadEquals))
			{
				this.targetExposure = Mathf.Min(this.targetExposure + 0.2f, 2f);
			}
			if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
			{
				this.targetExposure = Mathf.Max(0.05f, this.targetExposure - 0.2f);
			}
			if (Mathf.Abs(globalSky.CamExposure - this.targetExposure) > 0.01f)
			{
				globalSky.CamExposure = 0.95f * globalSky.CamExposure + 0.05f * this.targetExposure;
			}
			else
			{
				globalSky.CamExposure = this.targetExposure;
			}
		}
	}

	// Token: 0x060012B5 RID: 4789 RVA: 0x0006AABE File Offset: 0x00068EBE
	private void FixedUpdate()
	{
		if (this.spinning && this.mesh)
		{
			this.mesh.transform.Rotate(this.angularVel * Time.fixedDeltaTime);
		}
	}

	// Token: 0x060012B6 RID: 4790 RVA: 0x0006AAFC File Offset: 0x00068EFC
	private void OnGUI()
	{
		Rect pixelRect = base.GetComponent<Camera>().pixelRect;
		this.helpColor.a = this.guiAlpha;
		if (this.helpTex)
		{
			pixelRect.width = 0.75f * (float)this.helpTex.width;
			pixelRect.height = 0.75f * (float)this.helpTex.height;
			pixelRect.y = -50f;
			pixelRect.x = (float)base.GetComponent<Camera>().pixelWidth - pixelRect.width;
			Rect rect = pixelRect;
			rect.x += 0.5f * rect.width;
			rect.width *= 0.5f;
			Vector3 mousePosition = Input.mousePosition;
			mousePosition.y = (float)base.GetComponent<Camera>().pixelHeight - mousePosition.y;
			if (rect.Contains(mousePosition))
			{
				this.helpAlpha = Mathf.Lerp(this.helpAlpha, 1f, 0.01f);
			}
			else
			{
				this.helpAlpha = Mathf.Lerp(this.helpAlpha, 0.25f, 0.01f);
			}
			this.helpColor.a = this.helpAlpha * this.guiAlpha;
			GUI.color = this.helpColor;
			GUI.DrawTexture(pixelRect, this.helpTex);
		}
	}

	// Token: 0x0400106A RID: 4202
	private bool spinning = true;

	// Token: 0x0400106B RID: 4203
	public float guiAlpha = 0.8f;

	// Token: 0x0400106C RID: 4204
	private float helpAlpha = 0.25f;

	// Token: 0x0400106D RID: 4205
	public Transform mesh;

	// Token: 0x0400106E RID: 4206
	public Texture2D helpTex;

	// Token: 0x0400106F RID: 4207
	private Color helpColor = new Color(1f, 1f, 1f, 0f);

	// Token: 0x04001070 RID: 4208
	private float targetExposure = 1f;

	// Token: 0x04001071 RID: 4209
	private Vector3 angularVel = new Vector3(0f, 6f, 0f);
}
