using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AEB RID: 2795
public class DAZMeshMaterialOptions : MaterialOptions
{
	// Token: 0x06004ABF RID: 19135 RVA: 0x0019D850 File Offset: 0x0019BC50
	public DAZMeshMaterialOptions()
	{
	}

	// Token: 0x17000A9E RID: 2718
	// (get) Token: 0x06004AC0 RID: 19136 RVA: 0x0019D858 File Offset: 0x0019BC58
	// (set) Token: 0x06004AC1 RID: 19137 RVA: 0x0019D8BA File Offset: 0x0019BCBA
	public DAZMesh mesh
	{
		get
		{
			if (this._mesh == null)
			{
				DAZMesh[] components = base.GetComponents<DAZMesh>();
				foreach (DAZMesh dazmesh in components)
				{
					if (dazmesh.enabled && dazmesh.drawMorphedUVMappedMesh)
					{
						this._mesh = dazmesh;
					}
				}
			}
			return this._mesh;
		}
		set
		{
			if (this._mesh != value)
			{
				this._mesh = value;
				this.SetAllParameters();
			}
		}
	}

	// Token: 0x06004AC2 RID: 19138 RVA: 0x0019D8DC File Offset: 0x0019BCDC
	protected void SetMaterialHideForMesh(DAZMesh dmesh, bool b)
	{
		if (dmesh != null)
		{
			dmesh.InitMaterials();
			if (this.useSimpleMaterial)
			{
				Material simpleMaterial = dmesh.simpleMaterial;
				this.SetMaterialHide(simpleMaterial, b);
			}
			else if (this.paramMaterialSlots != null)
			{
				for (int i = 0; i < this.paramMaterialSlots.Length; i++)
				{
					int num = this.paramMaterialSlots[i];
					if (num < dmesh.numMaterials)
					{
						Material m = dmesh.materials[num];
						this.SetMaterialHide(m, b);
					}
				}
			}
		}
	}

	// Token: 0x06004AC3 RID: 19139 RVA: 0x0019D964 File Offset: 0x0019BD64
	protected override void SetMaterialHide(bool b)
	{
		if (this.useAllMeshes)
		{
			DAZMesh[] components = base.GetComponents<DAZMesh>();
			foreach (DAZMesh dazmesh in components)
			{
				if (dazmesh.enabled && dazmesh.drawMorphedUVMappedMesh)
				{
					this.SetMaterialHideForMesh(dazmesh, b);
				}
			}
		}
		else
		{
			this.SetMaterialHideForMesh(this._mesh, b);
		}
	}

	// Token: 0x06004AC4 RID: 19140 RVA: 0x0019D9D0 File Offset: 0x0019BDD0
	protected void SetMaterialRenderQueueForMesh(DAZMesh dmesh, int q)
	{
		if (dmesh != null)
		{
			dmesh.InitMaterials();
			if (this.useSimpleMaterial)
			{
				Material simpleMaterial = dmesh.simpleMaterial;
				simpleMaterial.renderQueue = q;
			}
			else if (this.paramMaterialSlots != null)
			{
				for (int i = 0; i < this.paramMaterialSlots.Length; i++)
				{
					int num = this.paramMaterialSlots[i];
					if (num < dmesh.numMaterials)
					{
						Material material = dmesh.materials[num];
						material.renderQueue = q;
					}
				}
			}
		}
	}

	// Token: 0x06004AC5 RID: 19141 RVA: 0x0019DA58 File Offset: 0x0019BE58
	protected override void SetMaterialRenderQueue(int q)
	{
		if (this.useAllMeshes)
		{
			DAZMesh[] components = base.GetComponents<DAZMesh>();
			foreach (DAZMesh dazmesh in components)
			{
				if (dazmesh.enabled && dazmesh.drawMorphedUVMappedMesh)
				{
					this.SetMaterialRenderQueueForMesh(dazmesh, q);
				}
			}
		}
		else
		{
			this.SetMaterialRenderQueueForMesh(this._mesh, q);
		}
	}

	// Token: 0x06004AC6 RID: 19142 RVA: 0x0019DAC4 File Offset: 0x0019BEC4
	protected void SetMaterialParamForMesh(DAZMesh dmesh, string name, float value)
	{
		if (dmesh != null)
		{
			dmesh.InitMaterials();
			if (this.useSimpleMaterial)
			{
				Material simpleMaterial = dmesh.simpleMaterial;
				if (simpleMaterial.HasProperty(name))
				{
					simpleMaterial.SetFloat(name, value);
				}
			}
			else if (this.paramMaterialSlots != null)
			{
				for (int i = 0; i < this.paramMaterialSlots.Length; i++)
				{
					int num = this.paramMaterialSlots[i];
					if (num < dmesh.numMaterials)
					{
						Material material = dmesh.materials[num];
						if (material != null && material.HasProperty(name))
						{
							material.SetFloat(name, value);
						}
					}
				}
			}
		}
	}

	// Token: 0x06004AC7 RID: 19143 RVA: 0x0019DB70 File Offset: 0x0019BF70
	protected override void SetMaterialParam(string name, float value)
	{
		if (this.useAllMeshes)
		{
			DAZMesh[] components = base.GetComponents<DAZMesh>();
			foreach (DAZMesh dazmesh in components)
			{
				if (dazmesh.enabled && dazmesh.drawMorphedUVMappedMesh)
				{
					this.SetMaterialParamForMesh(dazmesh, name, value);
				}
			}
		}
		else
		{
			this.SetMaterialParamForMesh(this._mesh, name, value);
		}
	}

	// Token: 0x06004AC8 RID: 19144 RVA: 0x0019DBDC File Offset: 0x0019BFDC
	protected void SetMaterialColorForMesh(DAZMesh dmesh, string name, Color c)
	{
		if (dmesh != null)
		{
			dmesh.InitMaterials();
			if (this.useSimpleMaterial)
			{
				Material simpleMaterial = dmesh.simpleMaterial;
				if (simpleMaterial.HasProperty(name))
				{
					simpleMaterial.SetColor(name, c);
				}
			}
			else if (this.paramMaterialSlots != null)
			{
				for (int i = 0; i < this.paramMaterialSlots.Length; i++)
				{
					int num = this.paramMaterialSlots[i];
					if (num < dmesh.numMaterials)
					{
						Material material = dmesh.materials[num];
						if (material != null && material.HasProperty(name))
						{
							material.SetColor(name, c);
						}
					}
				}
			}
		}
	}

	// Token: 0x06004AC9 RID: 19145 RVA: 0x0019DC88 File Offset: 0x0019C088
	protected override void SetMaterialColor(string name, Color c)
	{
		if (this.useAllMeshes)
		{
			DAZMesh[] components = base.GetComponents<DAZMesh>();
			foreach (DAZMesh dazmesh in components)
			{
				if (dazmesh.enabled && dazmesh.drawMorphedUVMappedMesh)
				{
					this.SetMaterialColorForMesh(dazmesh, name, c);
				}
			}
		}
		else
		{
			this.SetMaterialColorForMesh(this._mesh, name, c);
		}
	}

	// Token: 0x06004ACA RID: 19146 RVA: 0x0019DCF4 File Offset: 0x0019C0F4
	protected override void SetMaterialTexture(int slot, string propName, Texture texture)
	{
		if (this.useAllMeshes)
		{
			DAZMesh[] components = base.GetComponents<DAZMesh>();
			foreach (DAZMesh dazmesh in components)
			{
				if (dazmesh != null)
				{
					dazmesh.InitMaterials();
					if (slot < dazmesh.numMaterials)
					{
						Material m = dazmesh.materials[slot];
						this.SetMaterialTexture(m, propName, texture);
					}
				}
			}
		}
		else if (this.mesh != null)
		{
			this.mesh.InitMaterials();
			if (slot < this.mesh.numMaterials)
			{
				Material m2 = this.mesh.materials[slot];
				this.SetMaterialTexture(m2, propName, texture);
			}
		}
	}

	// Token: 0x06004ACB RID: 19147 RVA: 0x0019DDAA File Offset: 0x0019C1AA
	protected override void SetMaterialTexture2(int slot, string propName, Texture texture)
	{
		Debug.LogError("SetMaterialTexture2 for DAZMeshMaterialOptions should not be used");
	}

	// Token: 0x06004ACC RID: 19148 RVA: 0x0019DDB8 File Offset: 0x0019C1B8
	protected override void SetMaterialTextureScale(int slot, string propName, Vector2 scale)
	{
		if (this.useAllMeshes)
		{
			DAZMesh[] components = base.GetComponents<DAZMesh>();
			foreach (DAZMesh dazmesh in components)
			{
				if (dazmesh != null)
				{
					dazmesh.InitMaterials();
					if (slot < dazmesh.numMaterials)
					{
						Material m = dazmesh.materials[slot];
						this.SetMaterialTextureScale(m, propName, scale);
					}
				}
			}
		}
		else if (this.mesh != null)
		{
			this.mesh.InitMaterials();
			if (slot < this.mesh.numMaterials)
			{
				Material m2 = this.mesh.materials[slot];
				this.SetMaterialTextureScale(m2, propName, scale);
			}
		}
	}

	// Token: 0x06004ACD RID: 19149 RVA: 0x0019DE6E File Offset: 0x0019C26E
	protected override void SetMaterialTextureScale2(int slot, string propName, Vector2 scale)
	{
		Debug.LogError("SetMaterialTextureScale2 for DAZMeshMaterialOptions should not be used");
	}

	// Token: 0x06004ACE RID: 19150 RVA: 0x0019DE7C File Offset: 0x0019C27C
	protected override void SetMaterialTextureOffset(int slot, string propName, Vector2 offset)
	{
		if (this.useAllMeshes)
		{
			DAZMesh[] components = base.GetComponents<DAZMesh>();
			foreach (DAZMesh dazmesh in components)
			{
				if (dazmesh != null)
				{
					dazmesh.InitMaterials();
					if (slot < dazmesh.numMaterials)
					{
						Material m = dazmesh.materials[slot];
						this.SetMaterialTextureOffset(m, propName, offset);
					}
				}
			}
		}
		else if (this.mesh != null)
		{
			this.mesh.InitMaterials();
			if (slot < this.mesh.numMaterials)
			{
				Material m2 = this.mesh.materials[slot];
				this.SetMaterialTextureOffset(m2, propName, offset);
			}
		}
	}

	// Token: 0x06004ACF RID: 19151 RVA: 0x0019DF32 File Offset: 0x0019C332
	protected override void SetMaterialTextureOffset2(int slot, string propName, Vector2 offset)
	{
		Debug.LogError("SetMaterialTextureOffset2 for DAZMeshMaterialOptions should not be used");
	}

	// Token: 0x06004AD0 RID: 19152 RVA: 0x0019DF40 File Offset: 0x0019C340
	public override Mesh GetMesh()
	{
		Mesh result = null;
		if (this._mesh != null)
		{
			result = this._mesh.morphedUVMappedMesh;
		}
		return result;
	}

	// Token: 0x06004AD1 RID: 19153 RVA: 0x0019DF70 File Offset: 0x0019C370
	public override void SetStartingValues(Dictionary<Texture2D, string> textureToSourcePath)
	{
		if (this.materialForDefaults == null)
		{
			if (this.useAllMeshes)
			{
				DAZMesh[] components = base.GetComponents<DAZMesh>();
				if (components.Length > 0)
				{
					this._mesh = components[0];
				}
				else
				{
					this._mesh = null;
				}
			}
			if (this.mesh != null)
			{
				this.mesh.InitMaterials();
				if (this.paramMaterialSlots != null && this.paramMaterialSlots.Length > 0)
				{
					int num = this.paramMaterialSlots[0];
					if (num < this.mesh.numMaterials)
					{
						this.materialForDefaults = this.mesh.materials[num];
					}
				}
			}
		}
		base.SetStartingValues(textureToSourcePath);
	}

	// Token: 0x040039A3 RID: 14755
	[HideInInspector]
	public bool useAllMeshes;

	// Token: 0x040039A4 RID: 14756
	[HideInInspector]
	[SerializeField]
	protected DAZMesh _mesh;

	// Token: 0x040039A5 RID: 14757
	[HideInInspector]
	public bool useSimpleMaterial;
}
