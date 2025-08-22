using System;

namespace Obi
{
	// Token: 0x020003E3 RID: 995
	[Flags]
	public enum ParticleData
	{
		// Token: 0x04001488 RID: 5256
		NONE = 0,
		// Token: 0x04001489 RID: 5257
		ACTIVE_STATUS = 1,
		// Token: 0x0400148A RID: 5258
		ACTOR_ID = 2,
		// Token: 0x0400148B RID: 5259
		POSITIONS = 4,
		// Token: 0x0400148C RID: 5260
		VELOCITIES = 8,
		// Token: 0x0400148D RID: 5261
		INV_MASSES = 16,
		// Token: 0x0400148E RID: 5262
		VORTICITIES = 32,
		// Token: 0x0400148F RID: 5263
		SOLID_RADII = 64,
		// Token: 0x04001490 RID: 5264
		PHASES = 128,
		// Token: 0x04001491 RID: 5265
		REST_POSITIONS = 256,
		// Token: 0x04001492 RID: 5266
		COLLISION_MATERIAL = 512,
		// Token: 0x04001493 RID: 5267
		ALL = -1
	}
}
