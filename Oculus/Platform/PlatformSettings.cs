using System;
using UnityEngine;

namespace Oculus.Platform
{
	// Token: 0x02000898 RID: 2200
	public sealed class PlatformSettings : ScriptableObject
	{
		// Token: 0x060037B4 RID: 14260 RVA: 0x0010E94F File Offset: 0x0010CD4F
		public PlatformSettings()
		{
		}

		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x060037B5 RID: 14261 RVA: 0x0010E974 File Offset: 0x0010CD74
		// (set) Token: 0x060037B6 RID: 14262 RVA: 0x0010E980 File Offset: 0x0010CD80
		public static string AppID
		{
			get
			{
				return PlatformSettings.Instance.ovrAppID;
			}
			set
			{
				PlatformSettings.Instance.ovrAppID = value;
			}
		}

		// Token: 0x17000611 RID: 1553
		// (get) Token: 0x060037B7 RID: 14263 RVA: 0x0010E98D File Offset: 0x0010CD8D
		// (set) Token: 0x060037B8 RID: 14264 RVA: 0x0010E999 File Offset: 0x0010CD99
		public static string MobileAppID
		{
			get
			{
				return PlatformSettings.Instance.ovrMobileAppID;
			}
			set
			{
				PlatformSettings.Instance.ovrMobileAppID = value;
			}
		}

		// Token: 0x17000612 RID: 1554
		// (get) Token: 0x060037B9 RID: 14265 RVA: 0x0010E9A6 File Offset: 0x0010CDA6
		// (set) Token: 0x060037BA RID: 14266 RVA: 0x0010E9B2 File Offset: 0x0010CDB2
		public static bool UseStandalonePlatform
		{
			get
			{
				return PlatformSettings.Instance.ovrUseStandalonePlatform;
			}
			set
			{
				PlatformSettings.Instance.ovrUseStandalonePlatform = value;
			}
		}

		// Token: 0x17000613 RID: 1555
		// (get) Token: 0x060037BB RID: 14267 RVA: 0x0010E9BF File Offset: 0x0010CDBF
		// (set) Token: 0x060037BC RID: 14268 RVA: 0x0010E9FF File Offset: 0x0010CDFF
		public static PlatformSettings Instance
		{
			get
			{
				if (PlatformSettings.instance == null)
				{
					PlatformSettings.instance = Resources.Load<PlatformSettings>("OculusPlatformSettings");
					if (PlatformSettings.instance == null)
					{
						PlatformSettings.instance = ScriptableObject.CreateInstance<PlatformSettings>();
					}
				}
				return PlatformSettings.instance;
			}
			set
			{
				PlatformSettings.instance = value;
			}
		}

		// Token: 0x040028D2 RID: 10450
		[SerializeField]
		private string ovrAppID = string.Empty;

		// Token: 0x040028D3 RID: 10451
		[SerializeField]
		private string ovrMobileAppID = string.Empty;

		// Token: 0x040028D4 RID: 10452
		[SerializeField]
		private bool ovrUseStandalonePlatform = true;

		// Token: 0x040028D5 RID: 10453
		private static PlatformSettings instance;
	}
}
