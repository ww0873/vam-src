using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace mset
{
	// Token: 0x02000331 RID: 817
	public class Sky : MonoBehaviour
	{
		// Token: 0x06001350 RID: 4944 RVA: 0x0006F2B4 File Offset: 0x0006D6B4
		public Sky()
		{
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06001351 RID: 4945 RVA: 0x0006F3BE File Offset: 0x0006D7BE
		// (set) Token: 0x06001352 RID: 4946 RVA: 0x0006F3C6 File Offset: 0x0006D7C6
		public Texture SpecularCube
		{
			get
			{
				return this.specularCube;
			}
			set
			{
				this.specularCube = value;
			}
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06001353 RID: 4947 RVA: 0x0006F3CF File Offset: 0x0006D7CF
		// (set) Token: 0x06001354 RID: 4948 RVA: 0x0006F3D7 File Offset: 0x0006D7D7
		public Texture SkyboxCube
		{
			get
			{
				return this.skyboxCube;
			}
			set
			{
				this.skyboxCube = value;
			}
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06001355 RID: 4949 RVA: 0x0006F3E0 File Offset: 0x0006D7E0
		// (set) Token: 0x06001356 RID: 4950 RVA: 0x0006F3E8 File Offset: 0x0006D7E8
		public Bounds Dimensions
		{
			get
			{
				return this.dimensions;
			}
			set
			{
				this._dirty = true;
				this.dimensions = value;
			}
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06001357 RID: 4951 RVA: 0x0006F3F8 File Offset: 0x0006D7F8
		// (set) Token: 0x06001358 RID: 4952 RVA: 0x0006F400 File Offset: 0x0006D800
		public bool Dirty
		{
			get
			{
				return this._dirty;
			}
			set
			{
				this._dirty = value;
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06001359 RID: 4953 RVA: 0x0006F409 File Offset: 0x0006D809
		// (set) Token: 0x0600135A RID: 4954 RVA: 0x0006F411 File Offset: 0x0006D811
		public float MasterIntensity
		{
			get
			{
				return this.masterIntensity;
			}
			set
			{
				this._dirty = true;
				this.masterIntensity = value;
			}
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x0600135B RID: 4955 RVA: 0x0006F421 File Offset: 0x0006D821
		// (set) Token: 0x0600135C RID: 4956 RVA: 0x0006F429 File Offset: 0x0006D829
		public float SkyIntensity
		{
			get
			{
				return this.skyIntensity;
			}
			set
			{
				this._dirty = true;
				this.skyIntensity = value;
			}
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x0600135D RID: 4957 RVA: 0x0006F439 File Offset: 0x0006D839
		// (set) Token: 0x0600135E RID: 4958 RVA: 0x0006F441 File Offset: 0x0006D841
		public float SpecIntensity
		{
			get
			{
				return this.specIntensity;
			}
			set
			{
				this._dirty = true;
				this.specIntensity = value;
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x0600135F RID: 4959 RVA: 0x0006F451 File Offset: 0x0006D851
		// (set) Token: 0x06001360 RID: 4960 RVA: 0x0006F459 File Offset: 0x0006D859
		public float DiffMultiplier
		{
			get
			{
				return this.diffMultiplier;
			}
			set
			{
				this._dirty = true;
				this.diffMultiplier = value;
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06001361 RID: 4961 RVA: 0x0006F469 File Offset: 0x0006D869
		// (set) Token: 0x06001362 RID: 4962 RVA: 0x0006F471 File Offset: 0x0006D871
		public float DiffIntensity
		{
			get
			{
				return this.diffIntensity;
			}
			set
			{
				this._dirty = true;
				this.diffIntensity = value;
			}
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06001363 RID: 4963 RVA: 0x0006F481 File Offset: 0x0006D881
		// (set) Token: 0x06001364 RID: 4964 RVA: 0x0006F489 File Offset: 0x0006D889
		public float CamExposure
		{
			get
			{
				return this.camExposure;
			}
			set
			{
				this._dirty = true;
				this.camExposure = value;
			}
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06001365 RID: 4965 RVA: 0x0006F499 File Offset: 0x0006D899
		// (set) Token: 0x06001366 RID: 4966 RVA: 0x0006F4A1 File Offset: 0x0006D8A1
		public float SpecIntensityLM
		{
			get
			{
				return this.specIntensityLM;
			}
			set
			{
				this._dirty = true;
				this.specIntensityLM = value;
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06001367 RID: 4967 RVA: 0x0006F4B1 File Offset: 0x0006D8B1
		// (set) Token: 0x06001368 RID: 4968 RVA: 0x0006F4B9 File Offset: 0x0006D8B9
		public float DiffIntensityLM
		{
			get
			{
				return this.diffIntensityLM;
			}
			set
			{
				this._dirty = true;
				this.diffIntensityLM = value;
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06001369 RID: 4969 RVA: 0x0006F4C9 File Offset: 0x0006D8C9
		// (set) Token: 0x0600136A RID: 4970 RVA: 0x0006F4D1 File Offset: 0x0006D8D1
		public bool HDRSky
		{
			get
			{
				return this.hdrSky;
			}
			set
			{
				this._dirty = true;
				this.hdrSky = value;
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x0600136B RID: 4971 RVA: 0x0006F4E1 File Offset: 0x0006D8E1
		// (set) Token: 0x0600136C RID: 4972 RVA: 0x0006F4E9 File Offset: 0x0006D8E9
		public bool HDRSpec
		{
			get
			{
				return this.hdrSpec;
			}
			set
			{
				this._dirty = true;
				this.hdrSpec = value;
			}
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x0600136D RID: 4973 RVA: 0x0006F4F9 File Offset: 0x0006D8F9
		// (set) Token: 0x0600136E RID: 4974 RVA: 0x0006F501 File Offset: 0x0006D901
		public bool LinearSpace
		{
			get
			{
				return this.linearSpace;
			}
			set
			{
				this._dirty = true;
				this.linearSpace = value;
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x0600136F RID: 4975 RVA: 0x0006F511 File Offset: 0x0006D911
		// (set) Token: 0x06001370 RID: 4976 RVA: 0x0006F519 File Offset: 0x0006D919
		public bool AutoDetectColorSpace
		{
			get
			{
				return this.autoDetectColorSpace;
			}
			set
			{
				this._dirty = true;
				this.autoDetectColorSpace = value;
			}
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06001371 RID: 4977 RVA: 0x0006F529 File Offset: 0x0006D929
		// (set) Token: 0x06001372 RID: 4978 RVA: 0x0006F531 File Offset: 0x0006D931
		public bool HasDimensions
		{
			get
			{
				return this.hasDimensions;
			}
			set
			{
				this._dirty = true;
				this.hasDimensions = value;
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06001373 RID: 4979 RVA: 0x0006F541 File Offset: 0x0006D941
		private Cubemap blackCube
		{
			get
			{
				if (this._blackCube == null)
				{
					this._blackCube = Resources.Load<Cubemap>("blackCube");
				}
				return this._blackCube;
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06001374 RID: 4980 RVA: 0x0006F56A File Offset: 0x0006D96A
		private Material SkyboxMaterial
		{
			get
			{
				if (this._SkyboxMaterial == null)
				{
					this._SkyboxMaterial = Resources.Load<Material>("skyboxMat");
				}
				return this._SkyboxMaterial;
			}
		}

		// Token: 0x06001375 RID: 4981 RVA: 0x0006F594 File Offset: 0x0006D994
		private static Material[] getTargetMaterials(Renderer target)
		{
			SkyAnchor component = target.gameObject.GetComponent<SkyAnchor>();
			if (component != null)
			{
				return component.materials;
			}
			return target.sharedMaterials;
		}

		// Token: 0x06001376 RID: 4982 RVA: 0x0006F5C6 File Offset: 0x0006D9C6
		public void Apply()
		{
			this.Apply(0);
		}

		// Token: 0x06001377 RID: 4983 RVA: 0x0006F5D0 File Offset: 0x0006D9D0
		public void Apply(int blendIndex)
		{
			ShaderIDs bids = this.blendIDs[blendIndex];
			this.ApplyGlobally(bids);
		}

		// Token: 0x06001378 RID: 4984 RVA: 0x0006F5ED File Offset: 0x0006D9ED
		public void Apply(Renderer target)
		{
			this.Apply(target, 0);
		}

		// Token: 0x06001379 RID: 4985 RVA: 0x0006F5F7 File Offset: 0x0006D9F7
		public void Apply(Renderer target, int blendIndex)
		{
			if (target && base.enabled && base.gameObject.activeInHierarchy)
			{
				this.ApplyFast(target, blendIndex);
			}
		}

		// Token: 0x0600137A RID: 4986 RVA: 0x0006F628 File Offset: 0x0006DA28
		public void ApplyFast(Renderer target, int blendIndex)
		{
			if (Sky.propBlock == null)
			{
				Sky.propBlock = new MaterialPropertyBlock();
			}
			if (blendIndex == 0)
			{
				Sky.propBlock.Clear();
			}
			else
			{
				target.GetPropertyBlock(Sky.propBlock);
			}
			this.ApplyToBlock(ref Sky.propBlock, this.blendIDs[blendIndex]);
			target.SetPropertyBlock(Sky.propBlock);
		}

		// Token: 0x0600137B RID: 4987 RVA: 0x0006F687 File Offset: 0x0006DA87
		public void Apply(Material target)
		{
			this.Apply(target, 0);
		}

		// Token: 0x0600137C RID: 4988 RVA: 0x0006F691 File Offset: 0x0006DA91
		public void Apply(Material target, int blendIndex)
		{
			if (target && base.enabled && base.gameObject.activeInHierarchy)
			{
				this.ApplyToMaterial(target, this.blendIDs[blendIndex]);
			}
		}

		// Token: 0x0600137D RID: 4989 RVA: 0x0006F6C8 File Offset: 0x0006DAC8
		private void ApplyToBlock(ref MaterialPropertyBlock block, ShaderIDs bids)
		{
			block.SetVector(bids.exposureIBL, this.exposures);
			block.SetVector(bids.exposureLM, this.exposuresLM);
			block.SetMatrix(bids.skyMatrix, this.skyMatrix);
			block.SetMatrix(bids.invSkyMatrix, this.invMatrix);
			block.SetVector(bids.skyMin, this.skyMin);
			block.SetVector(bids.skyMax, this.skyMax);
			if (this.specularCube)
			{
				block.SetTexture(bids.specCubeIBL, this.specularCube);
			}
			else
			{
				block.SetTexture(bids.specCubeIBL, this.blackCube);
			}
			block.SetVector(bids.SH[0], this.SH.cBuffer[0]);
			block.SetVector(bids.SH[1], this.SH.cBuffer[1]);
			block.SetVector(bids.SH[2], this.SH.cBuffer[2]);
			block.SetVector(bids.SH[3], this.SH.cBuffer[3]);
			block.SetVector(bids.SH[4], this.SH.cBuffer[4]);
			block.SetVector(bids.SH[5], this.SH.cBuffer[5]);
			block.SetVector(bids.SH[6], this.SH.cBuffer[6]);
			block.SetVector(bids.SH[7], this.SH.cBuffer[7]);
			block.SetVector(bids.SH[8], this.SH.cBuffer[8]);
		}

		// Token: 0x0600137E RID: 4990 RVA: 0x0006F8D0 File Offset: 0x0006DCD0
		private void ApplyToMaterial(Material mat, ShaderIDs bids)
		{
			mat.SetVector(bids.exposureIBL, this.exposures);
			mat.SetVector(bids.exposureLM, this.exposuresLM);
			mat.SetMatrix(bids.skyMatrix, this.skyMatrix);
			mat.SetMatrix(bids.invSkyMatrix, this.invMatrix);
			mat.SetVector(bids.skyMin, this.skyMin);
			mat.SetVector(bids.skyMax, this.skyMax);
			if (this.specularCube)
			{
				mat.SetTexture(bids.specCubeIBL, this.specularCube);
			}
			else
			{
				mat.SetTexture(bids.specCubeIBL, this.blackCube);
			}
			if (this.skyboxCube)
			{
				mat.SetTexture(bids.skyCubeIBL, this.skyboxCube);
			}
			for (int i = 0; i < 9; i++)
			{
				mat.SetVector(bids.SH[i], this.SH.cBuffer[i]);
			}
		}

		// Token: 0x0600137F RID: 4991 RVA: 0x0006F9DC File Offset: 0x0006DDDC
		private void ApplySkyTransform(ShaderIDs bids)
		{
			Shader.SetGlobalMatrix(bids.skyMatrix, this.skyMatrix);
			Shader.SetGlobalMatrix(bids.invSkyMatrix, this.invMatrix);
			Shader.SetGlobalVector(bids.skyMin, this.skyMin);
			Shader.SetGlobalVector(bids.skyMax, this.skyMax);
		}

		// Token: 0x06001380 RID: 4992 RVA: 0x0006FA30 File Offset: 0x0006DE30
		private void ApplyGlobally(ShaderIDs bids)
		{
			if (bids != null)
			{
				Shader.SetGlobalMatrix(bids.skyMatrix, this.skyMatrix);
				Shader.SetGlobalMatrix(bids.invSkyMatrix, this.invMatrix);
				Shader.SetGlobalVector(bids.skyMin, this.skyMin);
				Shader.SetGlobalVector(bids.skyMax, this.skyMax);
				Shader.SetGlobalVector(bids.exposureIBL, this.exposures);
				Shader.SetGlobalVector(bids.exposureLM, this.exposuresLM);
				Shader.SetGlobalFloat("_EmissionLM", 1f);
				Shader.SetGlobalVector("_UniformOcclusion", Vector4.one);
				if (this.specularCube)
				{
					Shader.SetGlobalTexture(bids.specCubeIBL, this.specularCube);
				}
				else
				{
					Shader.SetGlobalTexture(bids.specCubeIBL, this.blackCube);
				}
				if (this.skyboxCube)
				{
					Shader.SetGlobalTexture(bids.skyCubeIBL, this.skyboxCube);
				}
				if (bids.SH != null && this.SH != null)
				{
					for (int i = 0; i < 9; i++)
					{
						Shader.SetGlobalVector(bids.SH[i], this.SH.cBuffer[i]);
					}
				}
			}
		}

		// Token: 0x06001381 RID: 4993 RVA: 0x0006FB6B File Offset: 0x0006DF6B
		public static void ScrubGlobalKeywords()
		{
			Shader.DisableKeyword("MARMO_SKY_BLEND_ON");
			Shader.DisableKeyword("MARMO_SKY_BLEND_OFF");
			Shader.DisableKeyword("MARMO_BOX_PROJECTION_ON");
			Shader.DisableKeyword("MARMO_BOX_PROJECTION_OFF");
			Shader.DisableKeyword("MARMO_TERRAIN_BLEND_ON");
			Shader.DisableKeyword("MARMO_TERRAIN_BLEND_OFF");
		}

		// Token: 0x06001382 RID: 4994 RVA: 0x0006FBAC File Offset: 0x0006DFAC
		public static void ScrubKeywords(Material[] materials)
		{
			foreach (Material material in materials)
			{
				if (material != null)
				{
					material.DisableKeyword("MARMO_SKY_BLEND_ON");
					material.DisableKeyword("MARMO_SKY_BLEND_OFF");
					material.DisableKeyword("MARMO_BOX_PROJECTION_ON");
					material.DisableKeyword("MARMO_BOX_PROJECTION_OFF");
					material.DisableKeyword("MARMO_TERRAIN_BLEND_ON");
					material.DisableKeyword("MARMO_TERRAIN_BLEND_OFF");
				}
			}
		}

		// Token: 0x06001383 RID: 4995 RVA: 0x0006FC21 File Offset: 0x0006E021
		public static void EnableProjectionSupport(bool enable)
		{
			if (enable)
			{
				Shader.DisableKeyword("MARMO_BOX_PROJECTION_OFF");
			}
			else
			{
				Shader.EnableKeyword("MARMO_BOX_PROJECTION_OFF");
			}
			Sky.internalProjectionSupport = enable;
		}

		// Token: 0x06001384 RID: 4996 RVA: 0x0006FC48 File Offset: 0x0006E048
		public static void EnableGlobalProjection(bool enable)
		{
			if (!Sky.internalProjectionSupport)
			{
				return;
			}
			if (enable)
			{
				Shader.EnableKeyword("MARMO_BOX_PROJECTION_ON");
			}
			else
			{
				Shader.DisableKeyword("MARMO_BOX_PROJECTION_ON");
			}
		}

		// Token: 0x06001385 RID: 4997 RVA: 0x0006FC74 File Offset: 0x0006E074
		public static void EnableProjection(Renderer target, Material[] mats, bool enable)
		{
			if (!Sky.internalProjectionSupport)
			{
				return;
			}
			if (mats == null)
			{
				return;
			}
			if (enable)
			{
				foreach (Material material in mats)
				{
					if (material)
					{
						material.EnableKeyword("MARMO_BOX_PROJECTION_ON");
						material.DisableKeyword("MARMO_BOX_PROJECTION_OFF");
					}
				}
			}
			else
			{
				foreach (Material material2 in mats)
				{
					if (material2)
					{
						material2.DisableKeyword("MARMO_BOX_PROJECTION_ON");
						material2.EnableKeyword("MARMO_BOX_PROJECTION_OFF");
					}
				}
			}
		}

		// Token: 0x06001386 RID: 4998 RVA: 0x0006FD1C File Offset: 0x0006E11C
		public static void EnableProjection(Material mat, bool enable)
		{
			if (!Sky.internalProjectionSupport)
			{
				return;
			}
			if (enable)
			{
				mat.EnableKeyword("MARMO_BOX_PROJECTION_ON");
				mat.DisableKeyword("MARMO_BOX_PROJECTION_OFF");
			}
			else
			{
				mat.DisableKeyword("MARMO_BOX_PROJECTION_ON");
				mat.EnableKeyword("MARMO_BOX_PROJECTION_OFF");
			}
		}

		// Token: 0x06001387 RID: 4999 RVA: 0x0006FD6B File Offset: 0x0006E16B
		public static void EnableBlendingSupport(bool enable)
		{
			if (enable)
			{
				Shader.DisableKeyword("MARMO_SKY_BLEND_OFF");
			}
			else
			{
				Shader.EnableKeyword("MARMO_SKY_BLEND_OFF");
			}
			Sky.internalBlendingSupport = enable;
		}

		// Token: 0x06001388 RID: 5000 RVA: 0x0006FD92 File Offset: 0x0006E192
		public static void EnableTerrainBlending(bool enable)
		{
			if (!Sky.internalBlendingSupport)
			{
				return;
			}
			if (enable)
			{
				Shader.EnableKeyword("MARMO_TERRAIN_BLEND_ON");
				Shader.DisableKeyword("MARMO_TERRAIN_BLEND_OFF");
			}
			else
			{
				Shader.DisableKeyword("MARMO_TERRAIN_BLEND_ON");
				Shader.EnableKeyword("MARMO_TERRAIN_BLEND_OFF");
			}
		}

		// Token: 0x06001389 RID: 5001 RVA: 0x0006FDD2 File Offset: 0x0006E1D2
		public static void EnableGlobalBlending(bool enable)
		{
			if (!Sky.internalBlendingSupport)
			{
				return;
			}
			if (enable)
			{
				Shader.EnableKeyword("MARMO_SKY_BLEND_ON");
			}
			else
			{
				Shader.DisableKeyword("MARMO_SKY_BLEND_ON");
			}
		}

		// Token: 0x0600138A RID: 5002 RVA: 0x0006FE00 File Offset: 0x0006E200
		public static void EnableBlending(Renderer target, Material[] mats, bool enable)
		{
			if (!Sky.internalBlendingSupport)
			{
				return;
			}
			if (mats == null)
			{
				return;
			}
			if (enable)
			{
				foreach (Material material in mats)
				{
					if (material)
					{
						material.EnableKeyword("MARMO_SKY_BLEND_ON");
						material.DisableKeyword("MARMO_SKY_BLEND_OFF");
					}
				}
			}
			else
			{
				foreach (Material material2 in mats)
				{
					if (material2)
					{
						material2.DisableKeyword("MARMO_SKY_BLEND_ON");
						material2.EnableKeyword("MARMO_SKY_BLEND_OFF");
					}
				}
			}
		}

		// Token: 0x0600138B RID: 5003 RVA: 0x0006FEA8 File Offset: 0x0006E2A8
		public static void EnableBlending(Material mat, bool enable)
		{
			if (!Sky.internalBlendingSupport)
			{
				return;
			}
			if (enable)
			{
				mat.EnableKeyword("MARMO_SKY_BLEND_ON");
				mat.DisableKeyword("MARMO_SKY_BLEND_OFF");
			}
			else
			{
				mat.DisableKeyword("MARMO_SKY_BLEND_ON");
				mat.EnableKeyword("MARMO_SKY_BLEND_OFF");
			}
		}

		// Token: 0x0600138C RID: 5004 RVA: 0x0006FEF7 File Offset: 0x0006E2F7
		public static void SetBlendWeight(float weight)
		{
			Shader.SetGlobalFloat("_BlendWeightIBL", weight);
		}

		// Token: 0x0600138D RID: 5005 RVA: 0x0006FF04 File Offset: 0x0006E304
		public static void SetBlendWeight(Renderer target, float weight)
		{
			if (Sky.propBlock == null)
			{
				Sky.propBlock = new MaterialPropertyBlock();
			}
			else
			{
				Sky.propBlock.Clear();
			}
			target.GetPropertyBlock(Sky.propBlock);
			Sky.propBlock.SetFloat("_BlendWeightIBL", weight);
			target.SetPropertyBlock(Sky.propBlock);
		}

		// Token: 0x0600138E RID: 5006 RVA: 0x0006FF5A File Offset: 0x0006E35A
		public static void SetBlendWeight(Material mat, float weight)
		{
			mat.SetFloat("_BlendWeightIBL", weight);
		}

		// Token: 0x0600138F RID: 5007 RVA: 0x0006FF68 File Offset: 0x0006E368
		public static void SetUniformOcclusion(Renderer target, float diffuse, float specular)
		{
			if (target != null)
			{
				Vector4 one = Vector4.one;
				one.x = diffuse;
				one.y = specular;
				Material[] targetMaterials = Sky.getTargetMaterials(target);
				foreach (Material material in targetMaterials)
				{
					material.SetVector("_UniformOcclusion", one);
				}
			}
		}

		// Token: 0x06001390 RID: 5008 RVA: 0x0006FFC9 File Offset: 0x0006E3C9
		public void SetCustomExposure(float diffInt, float specInt, float skyInt, float camExpo)
		{
			this.SetCustomExposure(null, diffInt, specInt, skyInt, camExpo);
		}

		// Token: 0x06001391 RID: 5009 RVA: 0x0006FFD8 File Offset: 0x0006E3D8
		public void SetCustomExposure(Renderer target, float diffInt, float specInt, float skyInt, float camExpo)
		{
			Vector4 one = Vector4.one;
			this.ComputeExposureVector(ref one, diffInt, specInt, skyInt, camExpo);
			if (target == null)
			{
				Shader.SetGlobalVector(this.blendIDs[0].exposureIBL, one);
			}
			else
			{
				Material[] targetMaterials = Sky.getTargetMaterials(target);
				foreach (Material material in targetMaterials)
				{
					material.SetVector(this.blendIDs[0].exposureIBL, one);
				}
			}
		}

		// Token: 0x06001392 RID: 5010 RVA: 0x00070058 File Offset: 0x0006E458
		public void ToggleChildLights(bool enable)
		{
			Light[] componentsInChildren = base.GetComponentsInChildren<Light>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = enable;
			}
		}

		// Token: 0x06001393 RID: 5011 RVA: 0x0007008C File Offset: 0x0006E48C
		private void UpdateSkySize()
		{
			if (this.HasDimensions)
			{
				this.skyMin = this.Dimensions.center - this.Dimensions.extents;
				this.skyMax = this.Dimensions.center + this.Dimensions.extents;
				Vector3 localScale = base.transform.localScale;
				this.skyMin.x = this.skyMin.x * localScale.x;
				this.skyMin.y = this.skyMin.y * localScale.y;
				this.skyMin.z = this.skyMin.z * localScale.z;
				this.skyMax.x = this.skyMax.x * localScale.x;
				this.skyMax.y = this.skyMax.y * localScale.y;
				this.skyMax.z = this.skyMax.z * localScale.z;
			}
			else
			{
				this.skyMax = Vector4.one * 100000f;
				this.skyMin = Vector4.one * -100000f;
			}
		}

		// Token: 0x06001394 RID: 5012 RVA: 0x000701CE File Offset: 0x0006E5CE
		private void UpdateSkyTransform()
		{
			this.skyMatrix.SetTRS(base.transform.position, base.transform.rotation, Vector3.one);
			this.invMatrix = this.skyMatrix.inverse;
		}

		// Token: 0x06001395 RID: 5013 RVA: 0x00070208 File Offset: 0x0006E608
		private void ComputeExposureVector(ref Vector4 result, float diffInt, float specInt, float skyInt, float camExpo)
		{
			result.x = this.masterIntensity * diffInt;
			result.y = this.masterIntensity * specInt;
			result.z = skyInt * camExpo;
			result.w = camExpo;
			float num = 6f;
			if (this.linearSpace)
			{
				num = Mathf.Pow(num, 2.2f);
			}
			if (!this.hdrSpec)
			{
				result.y /= num;
			}
			if (!this.hdrSky)
			{
				result.z /= num;
			}
		}

		// Token: 0x06001396 RID: 5014 RVA: 0x00070294 File Offset: 0x0006E694
		private void UpdateExposures()
		{
			this.ComputeExposureVector(ref this.exposures, this.diffIntensity * this.diffMultiplier, this.specIntensity, this.skyIntensity, this.camExposure);
			this.exposuresLM.x = this.diffIntensityLM;
			this.exposuresLM.y = this.specIntensityLM;
		}

		// Token: 0x06001397 RID: 5015 RVA: 0x000702EE File Offset: 0x0006E6EE
		private void UpdatePropertyIDs()
		{
			this.blendIDs[0].Link();
			this.blendIDs[1].Link("1");
		}

		// Token: 0x06001398 RID: 5016 RVA: 0x0007030F File Offset: 0x0006E70F
		public void Awake()
		{
			this.UpdatePropertyIDs();
			Sky.propBlock = new MaterialPropertyBlock();
		}

		// Token: 0x06001399 RID: 5017 RVA: 0x00070324 File Offset: 0x0006E724
		private void Reset()
		{
			this.skyMatrix = (this.invMatrix = Matrix4x4.identity);
			this.exposures = Vector4.one;
			this.exposuresLM = Vector4.one;
			this.specularCube = (this.skyboxCube = null);
			this.masterIntensity = (this.skyIntensity = (this.specIntensity = (this.diffIntensity = 1f)));
			this.hdrSky = (this.hdrSpec = false);
		}

		// Token: 0x0600139A RID: 5018 RVA: 0x000703A4 File Offset: 0x0006E7A4
		private void OnEnable()
		{
			if (this.SH == null)
			{
				this.SH = new SHEncoding();
			}
			if (this.CustomSH != null)
			{
				this.SH.copyFrom(this.CustomSH.SH);
			}
			this.SH.copyToBuffer();
			SceneManager.sceneLoaded += this.SceneStart;
		}

		// Token: 0x0600139B RID: 5019 RVA: 0x0007040A File Offset: 0x0006E80A
		private void OnDisable()
		{
			SceneManager.sceneLoaded -= this.SceneStart;
		}

		// Token: 0x0600139C RID: 5020 RVA: 0x0007041D File Offset: 0x0006E81D
		private void SceneStart(Scene scene, LoadSceneMode mode)
		{
			this.UpdateExposures();
			this.UpdateSkyTransform();
			this.UpdateSkySize();
		}

		// Token: 0x0600139D RID: 5021 RVA: 0x00070431 File Offset: 0x0006E831
		private void Start()
		{
			this.UpdateExposures();
			this.UpdateSkyTransform();
			this.UpdateSkySize();
		}

		// Token: 0x0600139E RID: 5022 RVA: 0x00070445 File Offset: 0x0006E845
		private void Update()
		{
			if (base.transform.hasChanged)
			{
				this.Dirty = true;
				this.UpdateSkyTransform();
				this.UpdateSkySize();
				base.transform.hasChanged = false;
			}
			this.UpdateExposures();
		}

		// Token: 0x0600139F RID: 5023 RVA: 0x0007047C File Offset: 0x0006E87C
		private void OnDestroy()
		{
			this.SH = null;
			this._blackCube = null;
			this.specularCube = null;
			this.skyboxCube = null;
		}

		// Token: 0x060013A0 RID: 5024 RVA: 0x0007049C File Offset: 0x0006E89C
		public void DrawProjectionCube(Vector3 center, Vector3 radius)
		{
			if (this.projMaterial == null)
			{
				this.projMaterial = Resources.Load<Material>("projectionMat");
				if (!this.projMaterial)
				{
					Debug.LogError("Failed to find projectionMat material in Resources folder!");
				}
			}
			Vector4 vector = Vector4.one;
			vector.z = this.CamExposure;
			vector *= this.masterIntensity;
			ShaderIDs shaderIDs = this.blendIDs[0];
			this.projMaterial.color = new Color(0.7f, 0.7f, 0.7f, 1f);
			this.projMaterial.SetVector(shaderIDs.skyMin, -this.Dimensions.extents);
			this.projMaterial.SetVector(shaderIDs.skyMax, this.Dimensions.extents);
			this.projMaterial.SetVector(shaderIDs.exposureIBL, vector);
			this.projMaterial.SetTexture(shaderIDs.skyCubeIBL, this.specularCube);
			this.projMaterial.SetMatrix(shaderIDs.skyMatrix, this.skyMatrix);
			this.projMaterial.SetMatrix(shaderIDs.invSkyMatrix, this.invMatrix);
			this.projMaterial.SetPass(0);
			GL.PushMatrix();
			GL.MultMatrix(base.transform.localToWorldMatrix);
			GLUtil.DrawCube(center, -radius);
			GL.End();
			GL.PopMatrix();
		}

		// Token: 0x060013A1 RID: 5025 RVA: 0x0007060D File Offset: 0x0006EA0D
		private void OnTriggerEnter(Collider other)
		{
			if (other.GetComponent<Renderer>())
			{
				this.Apply(other.GetComponent<Renderer>(), 0);
			}
		}

		// Token: 0x060013A2 RID: 5026 RVA: 0x0007062C File Offset: 0x0006EA2C
		private void OnPostRender()
		{
		}

		// Token: 0x060013A3 RID: 5027 RVA: 0x0007062E File Offset: 0x0006EA2E
		// Note: this type is marked as 'beforefieldinit'.
		static Sky()
		{
		}

		// Token: 0x040010E2 RID: 4322
		[SerializeField]
		private Texture specularCube;

		// Token: 0x040010E3 RID: 4323
		[SerializeField]
		private Texture skyboxCube;

		// Token: 0x040010E4 RID: 4324
		public bool IsProbe;

		// Token: 0x040010E5 RID: 4325
		[SerializeField]
		private Bounds dimensions = new Bounds(Vector3.zero, Vector3.one);

		// Token: 0x040010E6 RID: 4326
		private bool _dirty;

		// Token: 0x040010E7 RID: 4327
		[SerializeField]
		private float masterIntensity = 1f;

		// Token: 0x040010E8 RID: 4328
		[SerializeField]
		private float skyIntensity = 1f;

		// Token: 0x040010E9 RID: 4329
		[SerializeField]
		private float specIntensity = 1f;

		// Token: 0x040010EA RID: 4330
		[SerializeField]
		private float diffMultiplier = 1f;

		// Token: 0x040010EB RID: 4331
		[SerializeField]
		private float diffIntensity = 1f;

		// Token: 0x040010EC RID: 4332
		[SerializeField]
		private float camExposure = 1f;

		// Token: 0x040010ED RID: 4333
		[SerializeField]
		private float specIntensityLM = 1f;

		// Token: 0x040010EE RID: 4334
		[SerializeField]
		private float diffIntensityLM = 1f;

		// Token: 0x040010EF RID: 4335
		[SerializeField]
		private bool hdrSky = true;

		// Token: 0x040010F0 RID: 4336
		[SerializeField]
		private bool hdrSpec = true;

		// Token: 0x040010F1 RID: 4337
		[SerializeField]
		private bool linearSpace = true;

		// Token: 0x040010F2 RID: 4338
		[SerializeField]
		private bool autoDetectColorSpace = true;

		// Token: 0x040010F3 RID: 4339
		[SerializeField]
		private bool hasDimensions;

		// Token: 0x040010F4 RID: 4340
		public SHEncoding SH = new SHEncoding();

		// Token: 0x040010F5 RID: 4341
		public SHEncodingFile CustomSH;

		// Token: 0x040010F6 RID: 4342
		private Matrix4x4 skyMatrix = Matrix4x4.identity;

		// Token: 0x040010F7 RID: 4343
		private Matrix4x4 invMatrix = Matrix4x4.identity;

		// Token: 0x040010F8 RID: 4344
		private Vector4 exposures = Vector4.one;

		// Token: 0x040010F9 RID: 4345
		private Vector4 exposuresLM = Vector4.one;

		// Token: 0x040010FA RID: 4346
		private Vector4 skyMin = -Vector4.one;

		// Token: 0x040010FB RID: 4347
		private Vector4 skyMax = Vector4.one;

		// Token: 0x040010FC RID: 4348
		private ShaderIDs[] blendIDs = new ShaderIDs[]
		{
			new ShaderIDs(),
			new ShaderIDs()
		};

		// Token: 0x040010FD RID: 4349
		private static MaterialPropertyBlock propBlock;

		// Token: 0x040010FE RID: 4350
		[SerializeField]
		private Cubemap _blackCube;

		// Token: 0x040010FF RID: 4351
		[SerializeField]
		private Material _SkyboxMaterial;

		// Token: 0x04001100 RID: 4352
		private static bool internalProjectionSupport;

		// Token: 0x04001101 RID: 4353
		private static bool internalBlendingSupport;

		// Token: 0x04001102 RID: 4354
		private Material projMaterial;
	}
}
