using System;
using UnityEngine;

// Token: 0x020007BC RID: 1980
public sealed class OvrAvatarSettings : ScriptableObject
{
	// Token: 0x06003255 RID: 12885 RVA: 0x001068AF File Offset: 0x00104CAF
	public OvrAvatarSettings()
	{
	}

	// Token: 0x170005F1 RID: 1521
	// (get) Token: 0x06003256 RID: 12886 RVA: 0x001068CD File Offset: 0x00104CCD
	// (set) Token: 0x06003257 RID: 12887 RVA: 0x001068D9 File Offset: 0x00104CD9
	public static string AppID
	{
		get
		{
			return OvrAvatarSettings.Instance.ovrAppID;
		}
		set
		{
			OvrAvatarSettings.Instance.ovrAppID = value;
		}
	}

	// Token: 0x170005F2 RID: 1522
	// (get) Token: 0x06003258 RID: 12888 RVA: 0x001068E6 File Offset: 0x00104CE6
	// (set) Token: 0x06003259 RID: 12889 RVA: 0x001068F2 File Offset: 0x00104CF2
	public static string GearAppID
	{
		get
		{
			return OvrAvatarSettings.Instance.ovrGearAppID;
		}
		set
		{
			OvrAvatarSettings.Instance.ovrGearAppID = value;
		}
	}

	// Token: 0x170005F3 RID: 1523
	// (get) Token: 0x0600325A RID: 12890 RVA: 0x001068FF File Offset: 0x00104CFF
	// (set) Token: 0x0600325B RID: 12891 RVA: 0x0010693F File Offset: 0x00104D3F
	public static OvrAvatarSettings Instance
	{
		get
		{
			if (OvrAvatarSettings.instance == null)
			{
				OvrAvatarSettings.instance = Resources.Load<OvrAvatarSettings>("OvrAvatarSettings");
				if (OvrAvatarSettings.instance == null)
				{
					OvrAvatarSettings.instance = ScriptableObject.CreateInstance<OvrAvatarSettings>();
				}
			}
			return OvrAvatarSettings.instance;
		}
		set
		{
			OvrAvatarSettings.instance = value;
		}
	}

	// Token: 0x04002684 RID: 9860
	private static OvrAvatarSettings instance;

	// Token: 0x04002685 RID: 9861
	[SerializeField]
	private string ovrAppID = string.Empty;

	// Token: 0x04002686 RID: 9862
	[SerializeField]
	private string ovrGearAppID = string.Empty;
}
