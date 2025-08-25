using System;
using UnityEngine;

// Token: 0x02000B04 RID: 2820
public class DAZSkinMeshSelection : DAZMeshSelection
{
	// Token: 0x06004CAE RID: 19630 RVA: 0x001AE5B4 File Offset: 0x001AC9B4
	public DAZSkinMeshSelection()
	{
	}

	// Token: 0x17000ADE RID: 2782
	// (get) Token: 0x06004CAF RID: 19631 RVA: 0x001AE5BC File Offset: 0x001AC9BC
	// (set) Token: 0x06004CB0 RID: 19632 RVA: 0x001AE5C4 File Offset: 0x001AC9C4
	public DAZSkin skin
	{
		get
		{
			return this._skin;
		}
		set
		{
			this._skin = value;
			if (this._skin != null)
			{
				this.mesh = this._skin.dazMesh;
			}
		}
	}

	// Token: 0x04003B9A RID: 15258
	[SerializeField]
	private DAZSkin _skin;
}
