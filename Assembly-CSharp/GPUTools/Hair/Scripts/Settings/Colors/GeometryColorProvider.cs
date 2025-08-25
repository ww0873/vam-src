using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Settings.Colors
{
	// Token: 0x02000A24 RID: 2596
	[Serializable]
	public class GeometryColorProvider : IColorProvider
	{
		// Token: 0x06004323 RID: 17187 RVA: 0x0013B4E4 File Offset: 0x001398E4
		public GeometryColorProvider()
		{
		}

		// Token: 0x06004324 RID: 17188 RVA: 0x0013B4EC File Offset: 0x001398EC
		public Color GetColor(HairSettings settings, int x, int y, int sizeY)
		{
			List<Color> colors = settings.StandsSettings.Provider.GetColors();
			int index = x * sizeY + y;
			return colors[index];
		}
	}
}
