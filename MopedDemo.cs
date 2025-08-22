using System;
using mset;
using UnityEngine;

// Token: 0x02000319 RID: 793
public class MopedDemo : MonoBehaviour
{
	// Token: 0x060012A2 RID: 4770 RVA: 0x00069B14 File Offset: 0x00067F14
	public MopedDemo()
	{
	}

	// Token: 0x060012A3 RID: 4771 RVA: 0x00069BA0 File Offset: 0x00067FA0
	private void Start()
	{
		this.firstFrame = true;
		this.setDiffuse(true);
		this.setSpecular(true);
		this.currentSky = 0;
		for (int i = this.skies.Length - 1; i >= 0; i--)
		{
			this.setSky(i);
		}
		this.setBackground(this.background);
		this.greyTex = new Texture2D(16, 16);
		Color color = new Color(0.95f, 0.95f, 0.95f, 1f);
		Color[] pixels = this.greyTex.GetPixels();
		for (int j = 0; j < pixels.Length; j++)
		{
			pixels[j] = color;
		}
		this.greyTex.SetPixels(pixels);
		this.greyTex.Apply(true);
		this.blackTex = new Texture2D(16, 16);
		pixels = this.blackTex.GetPixels();
		Color color2 = new Color(0f, 0f, 0f, 0f);
		for (int k = 0; k < pixels.Length; k++)
		{
			pixels[k] = color2;
		}
		this.blackTex.SetPixels(pixels);
		this.blackTex.Apply(true);
		if (this.meshes != null)
		{
			this.diffTextures = new Texture[this.meshes.Length];
			this.specTextures = new Texture[this.meshes.Length];
			this.glowTextures = new Texture[this.meshes.Length];
			for (int l = 0; l < this.meshes.Length; l++)
			{
				if (this.meshes[l].material.HasProperty("_MainTex"))
				{
					this.diffTextures[l] = this.meshes[l].material.GetTexture("_MainTex");
				}
				if (this.meshes[l].material.HasProperty("_SpecTex"))
				{
					this.specTextures[l] = this.meshes[l].material.GetTexture("_SpecTex");
				}
				if (this.meshes[l].material.HasProperty("_Illum"))
				{
					this.glowTextures[l] = this.meshes[l].material.GetTexture("_Illum");
				}
			}
		}
		this.setGrey(false);
		this.setGlow(this.glow);
		this.setExposures(1f);
	}

	// Token: 0x060012A4 RID: 4772 RVA: 0x00069E14 File Offset: 0x00068214
	private void setDiffuse(bool yes)
	{
		this.showDiffuse = yes;
		for (int i = 0; i < this.skies.Length; i++)
		{
			if (this.skies[i])
			{
				this.skies[i].DiffIntensity = ((!yes) ? 0f : 1f);
			}
		}
	}

	// Token: 0x060012A5 RID: 4773 RVA: 0x00069E78 File Offset: 0x00068278
	private void setSpecular(bool yes)
	{
		this.showSpecular = yes;
		for (int i = 0; i < this.skies.Length; i++)
		{
			if (this.skies[i])
			{
				this.skies[i].SpecIntensity = ((!yes) ? 0f : 1f);
			}
		}
	}

	// Token: 0x060012A6 RID: 4774 RVA: 0x00069EDC File Offset: 0x000682DC
	private void setExposures(float val)
	{
		this.exposure = val;
		for (int i = 0; i < this.skies.Length; i++)
		{
			if (this.skies[i])
			{
				this.skies[i].CamExposure = val;
			}
		}
	}

	// Token: 0x060012A7 RID: 4775 RVA: 0x00069F2C File Offset: 0x0006832C
	private void setSky(int index)
	{
		this.currentSky = index;
		SkyManager skyManager = SkyManager.Get();
		if (skyManager)
		{
			skyManager.BlendToGlobalSky(this.skies[this.currentSky], 1f);
		}
	}

	// Token: 0x060012A8 RID: 4776 RVA: 0x00069F69 File Offset: 0x00068369
	private void setBackground(bool yes)
	{
		this.background = yes;
		SkyManager.Get().ShowSkybox = yes;
	}

	// Token: 0x060012A9 RID: 4777 RVA: 0x00069F80 File Offset: 0x00068380
	private void setGrey(bool yes)
	{
		if (this.meshes != null)
		{
			for (int i = 0; i < this.meshes.Length; i++)
			{
				if (yes)
				{
					if (this.diffTextures[i])
					{
						this.meshes[i].material.SetTexture("_MainTex", this.greyTex);
					}
					if (this.glowTextures[i])
					{
						this.meshes[i].material.SetTexture("_Illum", this.blackTex);
					}
				}
				else
				{
					if (this.diffTextures[i])
					{
						this.meshes[i].material.SetTexture("_MainTex", this.diffTextures[i]);
					}
					if (this.glowTextures[i])
					{
						this.meshes[i].material.SetTexture("_Illum", this.glowTextures[i]);
					}
				}
			}
		}
	}

	// Token: 0x060012AA RID: 4778 RVA: 0x0006A07C File Offset: 0x0006847C
	private void setGlow(float val)
	{
		this.glow = val;
		if (this.glowMeshes != null)
		{
			for (int i = 0; i < this.glowMeshes.Length; i++)
			{
				Material material = this.glowMeshes[i].material;
				if (material.HasProperty("_GlowStrength"))
				{
					material.SetFloat("_GlowStrength", 12f * this.glow);
				}
			}
		}
		if (this.specGlowMeshes != null)
		{
			for (int j = 0; j < this.specGlowMeshes.Length; j++)
			{
				Material material2 = this.specGlowMeshes[j].material;
				if (material2.HasProperty("_SpecInt"))
				{
					material2.SetFloat("_SpecInt", this.glow);
				}
			}
		}
	}

	// Token: 0x060012AB RID: 4779 RVA: 0x0006A13C File Offset: 0x0006853C
	private void Update()
	{
		if (this.firstFrame)
		{
			this.firstFrame = false;
			this.setSky(0);
		}
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			this.setExposures(0.25f);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			this.setExposures(0.5f);
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			this.setExposures(0.75f);
		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			this.setExposures(1f);
		}
		if (this.skies.Length > 0)
		{
			if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				this.setSky((this.currentSky + 1) % this.skies.Length);
			}
			if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				this.setSky((this.currentSky + this.skies.Length - 1) % this.skies.Length);
			}
		}
		if (Input.GetKeyDown(KeyCode.B))
		{
			this.setBackground(!this.background);
		}
		if (Input.GetKeyDown(KeyCode.S))
		{
			this.spinning = !this.spinning;
		}
		if (Input.multiTouchEnabled && Input.touchCount == 2)
		{
			float x = Input.GetTouch(0).position.x;
			float x2 = Input.GetTouch(1).position.x;
			float num = Input.GetTouch(0).deltaPosition.x;
			float num2 = Input.GetTouch(1).deltaPosition.x;
			if (num != 0f && num2 != 0f)
			{
				if (x > x2)
				{
					float num3 = num;
					num = num2;
					num2 = num3;
				}
				float num4 = num2 - num;
				this.setExposures(Mathf.Clamp(this.exposure + num4 * 0.0025f, 0.01f, 10f));
			}
		}
	}

	// Token: 0x060012AC RID: 4780 RVA: 0x0006A328 File Offset: 0x00068728
	private void FixedUpdate()
	{
		if (this.spinning)
		{
			SkyManager skyManager = SkyManager.Get();
			if (skyManager && skyManager.GlobalSky)
			{
				skyManager.GlobalSky.transform.Rotate(this.angularVel * Time.fixedDeltaTime);
			}
		}
	}

	// Token: 0x060012AD RID: 4781 RVA: 0x0006A384 File Offset: 0x00068784
	private void OnGUI()
	{
		Rect pixelRect = base.GetComponent<Camera>().pixelRect;
		pixelRect.y = base.GetComponent<Camera>().pixelRect.height * 0.87f;
		pixelRect.height = base.GetComponent<Camera>().pixelRect.height * 0.06f;
		if (Input.mousePosition.y < (float)base.GetComponent<Camera>().pixelHeight * 0.13f)
		{
			this.helpColor.a = Mathf.Min(1f, this.helpColor.a + 0.1f);
		}
		else
		{
			this.helpColor.a = Mathf.Min(1f, 0.9f * this.helpColor.a);
		}
		this.drawHelp(pixelRect.width - (float)this.helpTex.width, pixelRect.y - (float)this.helpTex.height - 10f);
		GUI.color = Color.white;
		if (this.showGUI)
		{
			Rect rect = pixelRect;
			rect.x = 10f;
			rect.y += 3f;
			rect.height = 20f;
			rect.width = 100f;
			bool flag = this.showDiffuse;
			this.showDiffuse = GUI.Toggle(rect, this.showDiffuse, "Diffuse IBL");
			if (flag != this.showDiffuse)
			{
				this.setDiffuse(this.showDiffuse);
			}
			rect.y += 15f;
			flag = this.showSpecular;
			this.showSpecular = GUI.Toggle(rect, this.showSpecular, "Specular IBL");
			if (flag != this.showSpecular)
			{
				this.setSpecular(this.showSpecular);
			}
			rect.x += rect.width;
			rect.y -= 15f;
			flag = this.background;
			rect.x += rect.width;
			this.background = GUI.Toggle(rect, this.background, "Skybox");
			if (flag != this.background)
			{
				this.setBackground(this.background);
			}
			rect.y += 15f;
			this.spinning = GUI.Toggle(rect, this.spinning, "Spinning");
			rect.x += rect.width;
			rect.y -= 15f;
			flag = this.untextured;
			this.untextured = GUI.Toggle(rect, this.untextured, "Untextured");
			if (flag != this.untextured)
			{
				this.setGrey(this.untextured);
			}
			Rect position = rect;
			position.x = 15f;
			position.y = pixelRect.yMax - 10f;
			position.height = 20f;
			position.width = pixelRect.width * 0.28f;
			GUI.Label(position, "Exposure: " + Mathf.CeilToInt(this.exposure * 100f) + "%");
			position.y += 18f;
			float num = Mathf.Sqrt(this.exposure);
			num = GUI.HorizontalSlider(position, num, 0f, 2f);
			this.exposure = num * num;
			this.setExposures(this.exposure);
			position.x = base.GetComponent<Camera>().pixelRect.width - position.width - 15f;
			position.y -= 18f;
			GUI.Label(position, "Moped Lights");
			position.y += 18f;
			float num2 = this.glow * this.glow;
			float num3 = num2;
			num2 = GUI.HorizontalSlider(position, num2, 0f, 1f);
			if (num2 != num3)
			{
				this.setGlow(Mathf.Sqrt(num2));
			}
		}
	}

	// Token: 0x060012AE RID: 4782 RVA: 0x0006A7B4 File Offset: 0x00068BB4
	private void drawHelp(float x, float y)
	{
		if (this.helpTex)
		{
			Rect position = new Rect(x, y, (float)this.helpTex.width, (float)this.helpTex.height);
			GUI.color = this.helpColor;
			GUI.DrawTexture(position, this.helpTex);
		}
	}

	// Token: 0x04001050 RID: 4176
	public Sky[] skies;

	// Token: 0x04001051 RID: 4177
	private bool spinning = true;

	// Token: 0x04001052 RID: 4178
	private bool background = true;

	// Token: 0x04001053 RID: 4179
	private int currentSky;

	// Token: 0x04001054 RID: 4180
	private bool showDiffuse = true;

	// Token: 0x04001055 RID: 4181
	private bool showSpecular = true;

	// Token: 0x04001056 RID: 4182
	private bool untextured;

	// Token: 0x04001057 RID: 4183
	private float exposure = 1f;

	// Token: 0x04001058 RID: 4184
	private float glow = 0.5f;

	// Token: 0x04001059 RID: 4185
	public Renderer[] meshes;

	// Token: 0x0400105A RID: 4186
	public Renderer[] glowMeshes;

	// Token: 0x0400105B RID: 4187
	public Renderer[] specGlowMeshes;

	// Token: 0x0400105C RID: 4188
	private Texture[] diffTextures;

	// Token: 0x0400105D RID: 4189
	private Texture[] specTextures;

	// Token: 0x0400105E RID: 4190
	private Texture[] glowTextures;

	// Token: 0x0400105F RID: 4191
	private Texture2D greyTex;

	// Token: 0x04001060 RID: 4192
	private Texture2D blackTex;

	// Token: 0x04001061 RID: 4193
	public Texture2D helpTex;

	// Token: 0x04001062 RID: 4194
	private Color helpColor = new Color(1f, 1f, 1f, 0f);

	// Token: 0x04001063 RID: 4195
	public bool showGUI = true;

	// Token: 0x04001064 RID: 4196
	private bool firstFrame = true;

	// Token: 0x04001065 RID: 4197
	private Vector3 angularVel = new Vector3(0f, 6f, 0f);
}
