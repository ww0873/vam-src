using System;
using System.Collections.Generic;
using UnityEngine;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x02000257 RID: 599
	public static class ProjectItemTypes
	{
		// Token: 0x06000C8C RID: 3212 RVA: 0x0004CCB8 File Offset: 0x0004B0B8
		public static int GetProjectItemType(Type type)
		{
			while (type != null)
			{
				int result;
				if (ProjectItemTypes.Type.TryGetValue(type, out result))
				{
					return result;
				}
				type = type.BaseType();
			}
			return 0;
		}

		// Token: 0x06000C8D RID: 3213 RVA: 0x0004CCF0 File Offset: 0x0004B0F0
		// Note: this type is marked as 'beforefieldinit'.
		static ProjectItemTypes()
		{
		}

		// Token: 0x04000CFD RID: 3325
		public const int None = 0;

		// Token: 0x04000CFE RID: 3326
		public const int Folder = 1;

		// Token: 0x04000CFF RID: 3327
		public const int Scene = 2;

		// Token: 0x04000D00 RID: 3328
		public const int Obj = 1073741824;

		// Token: 0x04000D01 RID: 3329
		public const int Material = 1073741825;

		// Token: 0x04000D02 RID: 3330
		public const int Mesh = 1073741826;

		// Token: 0x04000D03 RID: 3331
		public const int Prefab = 1073741827;

		// Token: 0x04000D04 RID: 3332
		public const int Texture = 1073741828;

		// Token: 0x04000D05 RID: 3333
		public const int ProceduralMaterial = 1073741829;

		// Token: 0x04000D06 RID: 3334
		public static readonly Dictionary<int, string> Ext = new Dictionary<int, string>
		{
			{
				1,
				string.Empty
			},
			{
				2,
				"rtsc"
			},
			{
				1073741825,
				"rtmat"
			},
			{
				1073741829,
				"rtpmat"
			},
			{
				1073741826,
				"rtmesh"
			},
			{
				1073741827,
				"rtprefab"
			},
			{
				1073741828,
				"rtimg"
			},
			{
				1073741824,
				"rtobj"
			}
		};

		// Token: 0x04000D07 RID: 3335
		public static readonly Dictionary<Type, int> Type = new Dictionary<Type, int>
		{
			{
				typeof(GameObject),
				1073741827
			},
			{
				typeof(Mesh),
				1073741826
			},
			{
				typeof(Material),
				1073741825
			},
			{
				typeof(Texture),
				1073741828
			},
			{
				typeof(UnityEngine.Object),
				1073741824
			}
		};
	}
}
