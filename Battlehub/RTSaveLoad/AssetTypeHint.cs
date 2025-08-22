using System;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x0200023B RID: 571
	[Flags]
	public enum AssetTypeHint
	{
		// Token: 0x04000CAC RID: 3244
		Prefab = 1,
		// Token: 0x04000CAD RID: 3245
		Material = 2,
		// Token: 0x04000CAE RID: 3246
		ProceduralMaterial = 4,
		// Token: 0x04000CAF RID: 3247
		Mesh = 8,
		// Token: 0x04000CB0 RID: 3248
		Texture = 16,
		// Token: 0x04000CB1 RID: 3249
		All = 31
	}
}
