using System;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Settings.Colors
{
	// Token: 0x02000A25 RID: 2597
	public interface IColorProvider
	{
		// Token: 0x06004325 RID: 17189
		Color GetColor(HairSettings settings, int x, int y, int sizeY);
	}
}
