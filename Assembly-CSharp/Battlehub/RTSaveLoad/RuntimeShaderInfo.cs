using System;
using UnityEngine.Rendering;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x02000246 RID: 582
	[Serializable]
	public class RuntimeShaderInfo
	{
		// Token: 0x06000C2B RID: 3115 RVA: 0x0004AB65 File Offset: 0x00048F65
		public RuntimeShaderInfo()
		{
		}

		// Token: 0x04000CC8 RID: 3272
		public int dummy;

		// Token: 0x04000CC9 RID: 3273
		public string Name;

		// Token: 0x04000CCA RID: 3274
		public long InstanceId;

		// Token: 0x04000CCB RID: 3275
		public int PropertyCount;

		// Token: 0x04000CCC RID: 3276
		public string[] PropertyDescriptions;

		// Token: 0x04000CCD RID: 3277
		public string[] PropertyNames;

		// Token: 0x04000CCE RID: 3278
		public RTShaderPropertyType[] PropertyTypes;

		// Token: 0x04000CCF RID: 3279
		public RuntimeShaderInfo.RangeLimits[] PropertyRangeLimits;

		// Token: 0x04000CD0 RID: 3280
		public TextureDimension[] PropertyTexDims;

		// Token: 0x04000CD1 RID: 3281
		public bool[] IsHidden;

		// Token: 0x02000247 RID: 583
		[Serializable]
		public struct RangeLimits
		{
			// Token: 0x06000C2C RID: 3116 RVA: 0x0004AB6D File Offset: 0x00048F6D
			public RangeLimits(float def, float min, float max)
			{
				this.Def = def;
				this.Min = min;
				this.Max = max;
			}

			// Token: 0x04000CD2 RID: 3282
			public float Def;

			// Token: 0x04000CD3 RID: 3283
			public float Min;

			// Token: 0x04000CD4 RID: 3284
			public float Max;
		}
	}
}
