using System;
using UnityEngine;

namespace PrefabEvolution
{
	// Token: 0x02000409 RID: 1033
	internal static class PEHideFlagsUtility
	{
		// Token: 0x06001A41 RID: 6721 RVA: 0x0009323B File Offset: 0x0009163B
		internal static void HideFlagsSet(this UnityEngine.Object obj, HideFlags flags, bool value)
		{
			if (value)
			{
				obj.AddHideFlags(flags);
			}
			else
			{
				obj.RemoveHideFlags(flags);
			}
		}

		// Token: 0x06001A42 RID: 6722 RVA: 0x00093256 File Offset: 0x00091656
		internal static void AddHideFlags(this UnityEngine.Object obj, HideFlags flags)
		{
			obj.hideFlags |= flags;
		}

		// Token: 0x06001A43 RID: 6723 RVA: 0x00093266 File Offset: 0x00091666
		internal static void RemoveHideFlags(this UnityEngine.Object obj, HideFlags flags)
		{
			obj.hideFlags &= ~flags;
		}
	}
}
