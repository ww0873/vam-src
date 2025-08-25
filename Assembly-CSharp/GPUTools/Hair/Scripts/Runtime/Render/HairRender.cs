using System;
using GPUTools.Hair.Scripts.Runtime.Data;
using GPUTools.Hair.Scripts.Utils;
using UnityEngine;
using UnityEngine.Rendering;

namespace GPUTools.Hair.Scripts.Runtime.Render
{
	// Token: 0x02000A20 RID: 2592
	public class HairRender : MonoBehaviour
	{
		// Token: 0x06004312 RID: 17170 RVA: 0x0013AD6B File Offset: 0x0013916B
		public HairRender()
		{
		}

		// Token: 0x06004313 RID: 17171 RVA: 0x0013AD73 File Offset: 0x00139173
		private void Awake()
		{
			this.mesh = new Mesh();
			this.rend = base.gameObject.AddComponent<MeshRenderer>();
			base.gameObject.AddComponent<MeshFilter>().mesh = this.mesh;
		}

		// Token: 0x06004314 RID: 17172 RVA: 0x0013ADA7 File Offset: 0x001391A7
		public void Initialize(HairDataFacade data)
		{
			this.data = data;
			this.InitializeMaterial();
			this.InitializeMesh();
		}

		// Token: 0x06004315 RID: 17173 RVA: 0x0013ADBC File Offset: 0x001391BC
		public void InitializeMesh()
		{
			this.mesh.triangles = null;
			this.mesh.vertices = new Vector3[(int)this.data.Size.x];
			this.mesh.SetIndices(this.data.Indices, MeshTopology.Triangles, 0);
		}

		// Token: 0x06004316 RID: 17174 RVA: 0x0013AE14 File Offset: 0x00139214
		private void InitializeMaterial()
		{
			if (this.data.material != null)
			{
				this.rend.material = this.data.material;
			}
			else
			{
				this.rend.material = Resources.Load<Material>("Materials/Hair");
			}
			if (this.data.StyleMode)
			{
				if (this.data.BarycentricsFixed != null)
				{
					this.rend.material.SetBuffer("_Barycentrics", this.data.BarycentricsFixed.ComputeBuffer);
				}
				else
				{
					this.rend.material.SetBuffer("_Barycentrics", null);
				}
			}
			else if (this.data.Barycentrics != null)
			{
				this.rend.material.SetBuffer("_Barycentrics", this.data.Barycentrics.ComputeBuffer);
			}
			else
			{
				this.rend.material.SetBuffer("_Barycentrics", null);
			}
			if (this.data.TessRenderParticles != null)
			{
				this.rend.material.SetBuffer("_Particles", this.data.TessRenderParticles.ComputeBuffer);
			}
			else
			{
				this.rend.material.SetBuffer("_Particles", null);
			}
		}

		// Token: 0x06004317 RID: 17175 RVA: 0x0013AF6C File Offset: 0x0013936C
		public void Dispatch()
		{
			this.UpdateBounds();
			this.UpdateMaterial();
			this.UpdateRenderer();
		}

		// Token: 0x06004318 RID: 17176 RVA: 0x0013AF80 File Offset: 0x00139380
		public Shader GetShader()
		{
			if (this.rend != null && this.rend.material != null)
			{
				return this.rend.material.shader;
			}
			if (this.data.material != null)
			{
				return this.data.material.shader;
			}
			return null;
		}

		// Token: 0x06004319 RID: 17177 RVA: 0x0013AFED File Offset: 0x001393ED
		public void SetShader(Shader s)
		{
			if (this.rend.material.shader != s)
			{
				this.rend.material.shader = s;
			}
		}

		// Token: 0x0600431A RID: 17178 RVA: 0x0013B01B File Offset: 0x0013941B
		private void UpdateBounds()
		{
			this.mesh.bounds = base.transform.InverseTransformBounds(this.data.Bounds);
		}

		// Token: 0x0600431B RID: 17179 RVA: 0x0013B040 File Offset: 0x00139440
		private void UpdateMaterial()
		{
			if (this.data.StyleMode)
			{
				if (this.data.BarycentricsFixed != null)
				{
					this.rend.material.SetBuffer("_Barycentrics", this.data.BarycentricsFixed.ComputeBuffer);
				}
				else
				{
					this.rend.material.SetBuffer("_Barycentrics", null);
				}
			}
			else if (this.data.Barycentrics != null)
			{
				this.rend.material.SetBuffer("_Barycentrics", this.data.Barycentrics.ComputeBuffer);
			}
			else
			{
				this.rend.material.SetBuffer("_Barycentrics", null);
			}
			if (this.data.TessRenderParticles != null)
			{
				this.rend.material.SetBuffer("_Particles", this.data.TessRenderParticles.ComputeBuffer);
			}
			else
			{
				this.rend.material.SetBuffer("_Particles", null);
			}
			this.rend.material.SetVector("_LightCenter", this.data.LightCenter);
			if (this.data.StyleMode)
			{
				Vector2 v;
				v.x = 4f;
				v.y = this.data.TessFactor.y;
				this.rend.material.SetFloat("_RandomBarycentric", 0f);
				this.rend.material.SetVector("_TessFactor", v);
				this.rend.material.SetFloat("_StandWidth", 0.001f * this.data.WorldScale);
				this.rend.material.SetFloat("_MaxSpread", 1f);
			}
			else
			{
				this.rend.material.SetFloat("_RandomBarycentric", 1f);
				this.rend.material.SetVector("_TessFactor", this.data.TessFactor);
				this.rend.material.SetFloat("_StandWidth", this.data.StandWidth * this.data.WorldScale);
				this.rend.material.SetFloat("_MaxSpread", this.data.MaxSpread * this.data.WorldScale);
			}
			this.rend.material.SetFloat("_SpecularShift", this.data.SpecularShift);
			this.rend.material.SetFloat("_PrimarySpecular", this.data.PrimarySpecular);
			this.rend.material.SetFloat("_SecondarySpecular", this.data.SecondarySpecular);
			this.rend.material.SetColor("_SpecularColor", this.data.SpecularColor);
			this.rend.material.SetFloat("_Diffuse", this.data.Diffuse);
			this.rend.material.SetFloat("_FresnelPower", this.data.FresnelPower);
			this.rend.material.SetFloat("_FresnelAtten", this.data.FresnelAttenuation);
			this.rend.material.SetVector("_WavinessAxis", this.data.WavinessAxis);
			this.rend.material.SetVector("_Length", this.data.Length);
			this.rend.material.SetFloat("_Volume", this.data.Volume);
			this.rend.material.SetVector("_Size", this.data.Size);
			this.rend.material.SetFloat("_RandomTexColorPower", this.data.RandomTexColorPower);
			this.rend.material.SetFloat("_RandomTexColorOffset", this.data.RandomTexColorOffset);
			this.rend.material.SetFloat("_IBLFactor", this.data.IBLFactor);
		}

		// Token: 0x0600431C RID: 17180 RVA: 0x0013B488 File Offset: 0x00139888
		private void UpdateRenderer()
		{
			this.rend.shadowCastingMode = ((!this.data.CastShadows) ? ShadowCastingMode.Off : ShadowCastingMode.On);
			this.rend.receiveShadows = this.data.ReseiveShadows;
		}

		// Token: 0x17000888 RID: 2184
		// (get) Token: 0x0600431D RID: 17181 RVA: 0x0013B4C2 File Offset: 0x001398C2
		public bool IsVisible
		{
			get
			{
				return this.rend.isVisible;
			}
		}

		// Token: 0x040031CB RID: 12747
		private Mesh mesh;

		// Token: 0x040031CC RID: 12748
		private HairDataFacade data;

		// Token: 0x040031CD RID: 12749
		private MeshRenderer rend;
	}
}
