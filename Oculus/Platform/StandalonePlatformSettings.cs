using System;
using UnityEngine;

namespace Oculus.Platform
{
	// Token: 0x020008A5 RID: 2213
	public sealed class StandalonePlatformSettings : ScriptableObject
	{
		// Token: 0x060037D5 RID: 14293 RVA: 0x0010EBE5 File Offset: 0x0010CFE5
		public StandalonePlatformSettings()
		{
		}

		// Token: 0x17000615 RID: 1557
		// (get) Token: 0x060037D6 RID: 14294 RVA: 0x0010EBED File Offset: 0x0010CFED
		// (set) Token: 0x060037D7 RID: 14295 RVA: 0x0010EBF4 File Offset: 0x0010CFF4
		public static string OculusPlatformTestUserEmail
		{
			get
			{
				return string.Empty;
			}
			set
			{
			}
		}

		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x060037D8 RID: 14296 RVA: 0x0010EBF6 File Offset: 0x0010CFF6
		// (set) Token: 0x060037D9 RID: 14297 RVA: 0x0010EBFD File Offset: 0x0010CFFD
		public static string OculusPlatformTestUserPassword
		{
			get
			{
				return string.Empty;
			}
			set
			{
			}
		}
	}
}
