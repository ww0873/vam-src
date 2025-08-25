using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AD8 RID: 2776
public class DAZHairMeshMaterialOptions : MaterialOptions
{
	// Token: 0x060049E5 RID: 18917 RVA: 0x0017D0C9 File Offset: 0x0017B4C9
	public DAZHairMeshMaterialOptions()
	{
	}

	// Token: 0x17000A5A RID: 2650
	// (get) Token: 0x060049E6 RID: 18918 RVA: 0x0017D0D4 File Offset: 0x0017B4D4
	// (set) Token: 0x060049E7 RID: 18919 RVA: 0x0017D12B File Offset: 0x0017B52B
	public DAZHairMesh mesh
	{
		get
		{
			if (this._mesh == null)
			{
				DAZHairMesh[] components = base.GetComponents<DAZHairMesh>();
				foreach (DAZHairMesh dazhairMesh in components)
				{
					if (dazhairMesh.enabled)
					{
						this._mesh = dazhairMesh;
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

	// Token: 0x060049E8 RID: 18920 RVA: 0x0017D14C File Offset: 0x0017B54C
	protected override void SetMaterialParam(string name, float value)
	{
		if (this._mesh != null)
		{
			Material hairMaterialRuntime = this._mesh.hairMaterialRuntime;
			if (hairMaterialRuntime != null && hairMaterialRuntime.HasProperty(name))
			{
				hairMaterialRuntime.SetFloat(name, value);
			}
		}
	}

	// Token: 0x060049E9 RID: 18921 RVA: 0x0017D198 File Offset: 0x0017B598
	protected override void SetMaterialColor(string name, Color c)
	{
		if (this._mesh != null)
		{
			Material hairMaterialRuntime = this._mesh.hairMaterialRuntime;
			if (hairMaterialRuntime != null && hairMaterialRuntime.HasProperty(name))
			{
				hairMaterialRuntime.SetColor(name, c);
			}
		}
	}

	// Token: 0x060049EA RID: 18922 RVA: 0x0017D1E4 File Offset: 0x0017B5E4
	protected override void SetMaterialTexture(int slot, string propName, Texture texture)
	{
		if (this._mesh != null)
		{
			Material hairMaterialRuntime = this._mesh.hairMaterialRuntime;
			this.SetMaterialTexture(hairMaterialRuntime, propName, texture);
		}
	}

	// Token: 0x060049EB RID: 18923 RVA: 0x0017D218 File Offset: 0x0017B618
	protected override void SetMaterialTextureScale(int slot, string propName, Vector2 scale)
	{
		if (this._mesh != null)
		{
			Material hairMaterialRuntime = this._mesh.hairMaterialRuntime;
			this.SetMaterialTextureScale(hairMaterialRuntime, propName, scale);
		}
	}

	// Token: 0x060049EC RID: 18924 RVA: 0x0017D24C File Offset: 0x0017B64C
	protected override void SetMaterialTextureOffset(int slot, string propName, Vector2 offset)
	{
		if (this._mesh != null)
		{
			Material hairMaterialRuntime = this._mesh.hairMaterialRuntime;
			this.SetMaterialTextureOffset(hairMaterialRuntime, propName, offset);
		}
	}

	// Token: 0x060049ED RID: 18925 RVA: 0x0017D280 File Offset: 0x0017B680
	public override void SetStartingValues(Dictionary<Texture2D, string> textureToSourcePath)
	{
		if (this._mesh != null)
		{
			this._mesh.MaterialInit();
			if (this.materialForDefaults == null)
			{
				this.materialForDefaults = this._mesh.hairMaterial;
			}
		}
		base.SetStartingValues(textureToSourcePath);
	}

	// Token: 0x04003888 RID: 14472
	[HideInInspector]
	[SerializeField]
	protected DAZHairMesh _mesh;
}
