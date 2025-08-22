using System;
using UnityEngine;

// Token: 0x02000B11 RID: 2833
public class DAZSkinV2MeshSelection : DAZMeshSelection
{
	// Token: 0x06004D27 RID: 19751 RVA: 0x001AE60F File Offset: 0x001ACA0F
	public DAZSkinV2MeshSelection()
	{
	}

	// Token: 0x17000AF8 RID: 2808
	// (get) Token: 0x06004D28 RID: 19752 RVA: 0x001AE617 File Offset: 0x001ACA17
	// (set) Token: 0x06004D29 RID: 19753 RVA: 0x001AE61F File Offset: 0x001ACA1F
	public DAZSkinV2 skin
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

	// Token: 0x04003CC3 RID: 15555
	[SerializeField]
	private DAZSkinV2 _skin;
}
