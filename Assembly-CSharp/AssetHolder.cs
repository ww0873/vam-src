using System;
using UnityEngine;

// Token: 0x02000BFC RID: 3068
public class AssetHolder : MonoBehaviour
{
	// Token: 0x060058A5 RID: 22693 RVA: 0x00207A63 File Offset: 0x00205E63
	public AssetHolder()
	{
	}

	// Token: 0x040048F6 RID: 18678
	public string prefabFolderPath;

	// Token: 0x040048F7 RID: 18679
	public Transform[] prefabsToHold;

	// Token: 0x040048F8 RID: 18680
	public Material[] materialsToHold;

	// Token: 0x040048F9 RID: 18681
	public string textureFolderPath;

	// Token: 0x040048FA RID: 18682
	public Texture[] texturesToHold;
}
