using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000E84 RID: 3716
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	public class PostEffectsBase : MonoBehaviour
	{
		// Token: 0x0600710F RID: 28943 RVA: 0x002A72B1 File Offset: 0x002A56B1
		public PostEffectsBase()
		{
		}

		// Token: 0x06007110 RID: 28944 RVA: 0x002A72D4 File Offset: 0x002A56D4
		protected Material CheckShaderAndCreateMaterial(Shader s, Material m2Create)
		{
			if (!s)
			{
				Debug.Log("Missing shader in " + this.ToString());
				base.enabled = false;
				return null;
			}
			if (s.isSupported && m2Create && m2Create.shader == s)
			{
				return m2Create;
			}
			if (!s.isSupported)
			{
				this.NotSupported();
				Debug.Log(string.Concat(new string[]
				{
					"The shader ",
					s.ToString(),
					" on effect ",
					this.ToString(),
					" is not supported on this platform!"
				}));
				return null;
			}
			m2Create = new Material(s);
			this.createdMaterials.Add(m2Create);
			m2Create.hideFlags = HideFlags.DontSave;
			return m2Create;
		}

		// Token: 0x06007111 RID: 28945 RVA: 0x002A73A0 File Offset: 0x002A57A0
		protected Material CreateMaterial(Shader s, Material m2Create)
		{
			if (!s)
			{
				Debug.Log("Missing shader in " + this.ToString());
				return null;
			}
			if (m2Create && m2Create.shader == s && s.isSupported)
			{
				return m2Create;
			}
			if (!s.isSupported)
			{
				return null;
			}
			m2Create = new Material(s);
			this.createdMaterials.Add(m2Create);
			m2Create.hideFlags = HideFlags.DontSave;
			return m2Create;
		}

		// Token: 0x06007112 RID: 28946 RVA: 0x002A7422 File Offset: 0x002A5822
		private void OnEnable()
		{
			this.isSupported = true;
		}

		// Token: 0x06007113 RID: 28947 RVA: 0x002A742B File Offset: 0x002A582B
		private void OnDestroy()
		{
			this.RemoveCreatedMaterials();
		}

		// Token: 0x06007114 RID: 28948 RVA: 0x002A7434 File Offset: 0x002A5834
		private void RemoveCreatedMaterials()
		{
			while (this.createdMaterials.Count > 0)
			{
				Material obj = this.createdMaterials[0];
				this.createdMaterials.RemoveAt(0);
				UnityEngine.Object.Destroy(obj);
			}
		}

		// Token: 0x06007115 RID: 28949 RVA: 0x002A7476 File Offset: 0x002A5876
		protected bool CheckSupport()
		{
			return this.CheckSupport(false);
		}

		// Token: 0x06007116 RID: 28950 RVA: 0x002A747F File Offset: 0x002A587F
		public virtual bool CheckResources()
		{
			Debug.LogWarning("CheckResources () for " + this.ToString() + " should be overwritten.");
			return this.isSupported;
		}

		// Token: 0x06007117 RID: 28951 RVA: 0x002A74A1 File Offset: 0x002A58A1
		protected void Start()
		{
			this.CheckResources();
		}

		// Token: 0x06007118 RID: 28952 RVA: 0x002A74AC File Offset: 0x002A58AC
		protected bool CheckSupport(bool needDepth)
		{
			this.isSupported = true;
			this.supportHDRTextures = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf);
			this.supportDX11 = (SystemInfo.graphicsShaderLevel >= 50 && SystemInfo.supportsComputeShaders);
			if (!SystemInfo.supportsImageEffects)
			{
				this.NotSupported();
				return false;
			}
			if (needDepth && !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
			{
				this.NotSupported();
				return false;
			}
			if (needDepth)
			{
				base.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
			}
			return true;
		}

		// Token: 0x06007119 RID: 28953 RVA: 0x002A752B File Offset: 0x002A592B
		protected bool CheckSupport(bool needDepth, bool needHdr)
		{
			if (!this.CheckSupport(needDepth))
			{
				return false;
			}
			if (needHdr && !this.supportHDRTextures)
			{
				this.NotSupported();
				return false;
			}
			return true;
		}

		// Token: 0x0600711A RID: 28954 RVA: 0x002A7555 File Offset: 0x002A5955
		public bool Dx11Support()
		{
			return this.supportDX11;
		}

		// Token: 0x0600711B RID: 28955 RVA: 0x002A755D File Offset: 0x002A595D
		protected void ReportAutoDisable()
		{
			Debug.LogWarning("The image effect " + this.ToString() + " has been disabled as it's not supported on the current platform.");
		}

		// Token: 0x0600711C RID: 28956 RVA: 0x002A757C File Offset: 0x002A597C
		private bool CheckShader(Shader s)
		{
			Debug.Log(string.Concat(new string[]
			{
				"The shader ",
				s.ToString(),
				" on effect ",
				this.ToString(),
				" is not part of the Unity 3.2+ effects suite anymore. For best performance and quality, please ensure you are using the latest Standard Assets Image Effects (Pro only) package."
			}));
			if (!s.isSupported)
			{
				this.NotSupported();
				return false;
			}
			return false;
		}

		// Token: 0x0600711D RID: 28957 RVA: 0x002A75D7 File Offset: 0x002A59D7
		protected void NotSupported()
		{
			base.enabled = false;
			this.isSupported = false;
		}

		// Token: 0x0600711E RID: 28958 RVA: 0x002A75E8 File Offset: 0x002A59E8
		protected void DrawBorder(RenderTexture dest, Material material)
		{
			RenderTexture.active = dest;
			bool flag = true;
			GL.PushMatrix();
			GL.LoadOrtho();
			for (int i = 0; i < material.passCount; i++)
			{
				material.SetPass(i);
				float y;
				float y2;
				if (flag)
				{
					y = 1f;
					y2 = 0f;
				}
				else
				{
					y = 0f;
					y2 = 1f;
				}
				float x = 0f;
				float x2 = 1f / ((float)dest.width * 1f);
				float y3 = 0f;
				float y4 = 1f;
				GL.Begin(7);
				GL.TexCoord2(0f, y);
				GL.Vertex3(x, y3, 0.1f);
				GL.TexCoord2(1f, y);
				GL.Vertex3(x2, y3, 0.1f);
				GL.TexCoord2(1f, y2);
				GL.Vertex3(x2, y4, 0.1f);
				GL.TexCoord2(0f, y2);
				GL.Vertex3(x, y4, 0.1f);
				x = 1f - 1f / ((float)dest.width * 1f);
				x2 = 1f;
				y3 = 0f;
				y4 = 1f;
				GL.TexCoord2(0f, y);
				GL.Vertex3(x, y3, 0.1f);
				GL.TexCoord2(1f, y);
				GL.Vertex3(x2, y3, 0.1f);
				GL.TexCoord2(1f, y2);
				GL.Vertex3(x2, y4, 0.1f);
				GL.TexCoord2(0f, y2);
				GL.Vertex3(x, y4, 0.1f);
				x = 0f;
				x2 = 1f;
				y3 = 0f;
				y4 = 1f / ((float)dest.height * 1f);
				GL.TexCoord2(0f, y);
				GL.Vertex3(x, y3, 0.1f);
				GL.TexCoord2(1f, y);
				GL.Vertex3(x2, y3, 0.1f);
				GL.TexCoord2(1f, y2);
				GL.Vertex3(x2, y4, 0.1f);
				GL.TexCoord2(0f, y2);
				GL.Vertex3(x, y4, 0.1f);
				x = 0f;
				x2 = 1f;
				y3 = 1f - 1f / ((float)dest.height * 1f);
				y4 = 1f;
				GL.TexCoord2(0f, y);
				GL.Vertex3(x, y3, 0.1f);
				GL.TexCoord2(1f, y);
				GL.Vertex3(x2, y3, 0.1f);
				GL.TexCoord2(1f, y2);
				GL.Vertex3(x2, y4, 0.1f);
				GL.TexCoord2(0f, y2);
				GL.Vertex3(x, y4, 0.1f);
				GL.End();
			}
			GL.PopMatrix();
		}

		// Token: 0x0400647A RID: 25722
		protected bool supportHDRTextures = true;

		// Token: 0x0400647B RID: 25723
		protected bool supportDX11;

		// Token: 0x0400647C RID: 25724
		protected bool isSupported = true;

		// Token: 0x0400647D RID: 25725
		private List<Material> createdMaterials = new List<Material>();
	}
}
