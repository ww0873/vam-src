using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;

// Token: 0x020008F8 RID: 2296
public class OVRManager : MonoBehaviour
{
	// Token: 0x060039A9 RID: 14761 RVA: 0x00119C9C File Offset: 0x0011809C
	public OVRManager()
	{
	}

	// Token: 0x1700063C RID: 1596
	// (get) Token: 0x060039AA RID: 14762 RVA: 0x00119D59 File Offset: 0x00118159
	// (set) Token: 0x060039AB RID: 14763 RVA: 0x00119D60 File Offset: 0x00118160
	public static OVRManager instance
	{
		[CompilerGenerated]
		get
		{
			return OVRManager.<instance>k__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			OVRManager.<instance>k__BackingField = value;
		}
	}

	// Token: 0x1700063D RID: 1597
	// (get) Token: 0x060039AC RID: 14764 RVA: 0x00119D68 File Offset: 0x00118168
	// (set) Token: 0x060039AD RID: 14765 RVA: 0x00119D6F File Offset: 0x0011816F
	public static OVRDisplay display
	{
		[CompilerGenerated]
		get
		{
			return OVRManager.<display>k__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			OVRManager.<display>k__BackingField = value;
		}
	}

	// Token: 0x1700063E RID: 1598
	// (get) Token: 0x060039AE RID: 14766 RVA: 0x00119D77 File Offset: 0x00118177
	// (set) Token: 0x060039AF RID: 14767 RVA: 0x00119D7E File Offset: 0x0011817E
	public static OVRTracker tracker
	{
		[CompilerGenerated]
		get
		{
			return OVRManager.<tracker>k__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			OVRManager.<tracker>k__BackingField = value;
		}
	}

	// Token: 0x1700063F RID: 1599
	// (get) Token: 0x060039B0 RID: 14768 RVA: 0x00119D86 File Offset: 0x00118186
	// (set) Token: 0x060039B1 RID: 14769 RVA: 0x00119D8D File Offset: 0x0011818D
	public static OVRBoundary boundary
	{
		[CompilerGenerated]
		get
		{
			return OVRManager.<boundary>k__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			OVRManager.<boundary>k__BackingField = value;
		}
	}

	// Token: 0x17000640 RID: 1600
	// (get) Token: 0x060039B2 RID: 14770 RVA: 0x00119D95 File Offset: 0x00118195
	public static OVRProfile profile
	{
		get
		{
			if (OVRManager._profile == null)
			{
				OVRManager._profile = new OVRProfile();
			}
			return OVRManager._profile;
		}
	}

	// Token: 0x140000BC RID: 188
	// (add) Token: 0x060039B3 RID: 14771 RVA: 0x00119DB8 File Offset: 0x001181B8
	// (remove) Token: 0x060039B4 RID: 14772 RVA: 0x00119DEC File Offset: 0x001181EC
	public static event Action HMDAcquired
	{
		add
		{
			Action action = OVRManager.HMDAcquired;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref OVRManager.HMDAcquired, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = OVRManager.HMDAcquired;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref OVRManager.HMDAcquired, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	// Token: 0x140000BD RID: 189
	// (add) Token: 0x060039B5 RID: 14773 RVA: 0x00119E20 File Offset: 0x00118220
	// (remove) Token: 0x060039B6 RID: 14774 RVA: 0x00119E54 File Offset: 0x00118254
	public static event Action HMDLost
	{
		add
		{
			Action action = OVRManager.HMDLost;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref OVRManager.HMDLost, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = OVRManager.HMDLost;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref OVRManager.HMDLost, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	// Token: 0x140000BE RID: 190
	// (add) Token: 0x060039B7 RID: 14775 RVA: 0x00119E88 File Offset: 0x00118288
	// (remove) Token: 0x060039B8 RID: 14776 RVA: 0x00119EBC File Offset: 0x001182BC
	public static event Action HMDMounted
	{
		add
		{
			Action action = OVRManager.HMDMounted;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref OVRManager.HMDMounted, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = OVRManager.HMDMounted;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref OVRManager.HMDMounted, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	// Token: 0x140000BF RID: 191
	// (add) Token: 0x060039B9 RID: 14777 RVA: 0x00119EF0 File Offset: 0x001182F0
	// (remove) Token: 0x060039BA RID: 14778 RVA: 0x00119F24 File Offset: 0x00118324
	public static event Action HMDUnmounted
	{
		add
		{
			Action action = OVRManager.HMDUnmounted;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref OVRManager.HMDUnmounted, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = OVRManager.HMDUnmounted;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref OVRManager.HMDUnmounted, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	// Token: 0x140000C0 RID: 192
	// (add) Token: 0x060039BB RID: 14779 RVA: 0x00119F58 File Offset: 0x00118358
	// (remove) Token: 0x060039BC RID: 14780 RVA: 0x00119F8C File Offset: 0x0011838C
	public static event Action VrFocusAcquired
	{
		add
		{
			Action action = OVRManager.VrFocusAcquired;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref OVRManager.VrFocusAcquired, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = OVRManager.VrFocusAcquired;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref OVRManager.VrFocusAcquired, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	// Token: 0x140000C1 RID: 193
	// (add) Token: 0x060039BD RID: 14781 RVA: 0x00119FC0 File Offset: 0x001183C0
	// (remove) Token: 0x060039BE RID: 14782 RVA: 0x00119FF4 File Offset: 0x001183F4
	public static event Action VrFocusLost
	{
		add
		{
			Action action = OVRManager.VrFocusLost;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref OVRManager.VrFocusLost, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = OVRManager.VrFocusLost;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref OVRManager.VrFocusLost, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	// Token: 0x140000C2 RID: 194
	// (add) Token: 0x060039BF RID: 14783 RVA: 0x0011A028 File Offset: 0x00118428
	// (remove) Token: 0x060039C0 RID: 14784 RVA: 0x0011A05C File Offset: 0x0011845C
	public static event Action InputFocusAcquired
	{
		add
		{
			Action action = OVRManager.InputFocusAcquired;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref OVRManager.InputFocusAcquired, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = OVRManager.InputFocusAcquired;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref OVRManager.InputFocusAcquired, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	// Token: 0x140000C3 RID: 195
	// (add) Token: 0x060039C1 RID: 14785 RVA: 0x0011A090 File Offset: 0x00118490
	// (remove) Token: 0x060039C2 RID: 14786 RVA: 0x0011A0C4 File Offset: 0x001184C4
	public static event Action InputFocusLost
	{
		add
		{
			Action action = OVRManager.InputFocusLost;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref OVRManager.InputFocusLost, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = OVRManager.InputFocusLost;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref OVRManager.InputFocusLost, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	// Token: 0x140000C4 RID: 196
	// (add) Token: 0x060039C3 RID: 14787 RVA: 0x0011A0F8 File Offset: 0x001184F8
	// (remove) Token: 0x060039C4 RID: 14788 RVA: 0x0011A12C File Offset: 0x0011852C
	public static event Action AudioOutChanged
	{
		add
		{
			Action action = OVRManager.AudioOutChanged;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref OVRManager.AudioOutChanged, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = OVRManager.AudioOutChanged;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref OVRManager.AudioOutChanged, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	// Token: 0x140000C5 RID: 197
	// (add) Token: 0x060039C5 RID: 14789 RVA: 0x0011A160 File Offset: 0x00118560
	// (remove) Token: 0x060039C6 RID: 14790 RVA: 0x0011A194 File Offset: 0x00118594
	public static event Action AudioInChanged
	{
		add
		{
			Action action = OVRManager.AudioInChanged;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref OVRManager.AudioInChanged, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = OVRManager.AudioInChanged;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref OVRManager.AudioInChanged, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	// Token: 0x140000C6 RID: 198
	// (add) Token: 0x060039C7 RID: 14791 RVA: 0x0011A1C8 File Offset: 0x001185C8
	// (remove) Token: 0x060039C8 RID: 14792 RVA: 0x0011A1FC File Offset: 0x001185FC
	public static event Action TrackingAcquired
	{
		add
		{
			Action action = OVRManager.TrackingAcquired;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref OVRManager.TrackingAcquired, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = OVRManager.TrackingAcquired;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref OVRManager.TrackingAcquired, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	// Token: 0x140000C7 RID: 199
	// (add) Token: 0x060039C9 RID: 14793 RVA: 0x0011A230 File Offset: 0x00118630
	// (remove) Token: 0x060039CA RID: 14794 RVA: 0x0011A264 File Offset: 0x00118664
	public static event Action TrackingLost
	{
		add
		{
			Action action = OVRManager.TrackingLost;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref OVRManager.TrackingLost, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = OVRManager.TrackingLost;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref OVRManager.TrackingLost, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	// Token: 0x140000C8 RID: 200
	// (add) Token: 0x060039CB RID: 14795 RVA: 0x0011A298 File Offset: 0x00118698
	// (remove) Token: 0x060039CC RID: 14796 RVA: 0x0011A2CC File Offset: 0x001186CC
	[Obsolete]
	public static event Action HSWDismissed
	{
		add
		{
			Action action = OVRManager.HSWDismissed;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref OVRManager.HSWDismissed, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = OVRManager.HSWDismissed;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref OVRManager.HSWDismissed, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	// Token: 0x17000641 RID: 1601
	// (get) Token: 0x060039CD RID: 14797 RVA: 0x0011A300 File Offset: 0x00118700
	// (set) Token: 0x060039CE RID: 14798 RVA: 0x0011A321 File Offset: 0x00118721
	public static bool isHmdPresent
	{
		get
		{
			if (!OVRManager._isHmdPresentCached)
			{
				OVRManager._isHmdPresentCached = true;
				OVRManager._isHmdPresent = OVRPlugin.hmdPresent;
			}
			return OVRManager._isHmdPresent;
		}
		private set
		{
			OVRManager._isHmdPresentCached = true;
			OVRManager._isHmdPresent = value;
		}
	}

	// Token: 0x17000642 RID: 1602
	// (get) Token: 0x060039CF RID: 14799 RVA: 0x0011A32F File Offset: 0x0011872F
	public static string audioOutId
	{
		get
		{
			return OVRPlugin.audioOutId;
		}
	}

	// Token: 0x17000643 RID: 1603
	// (get) Token: 0x060039D0 RID: 14800 RVA: 0x0011A336 File Offset: 0x00118736
	public static string audioInId
	{
		get
		{
			return OVRPlugin.audioInId;
		}
	}

	// Token: 0x17000644 RID: 1604
	// (get) Token: 0x060039D1 RID: 14801 RVA: 0x0011A33D File Offset: 0x0011873D
	// (set) Token: 0x060039D2 RID: 14802 RVA: 0x0011A35E File Offset: 0x0011875E
	public static bool hasVrFocus
	{
		get
		{
			if (!OVRManager._hasVrFocusCached)
			{
				OVRManager._hasVrFocusCached = true;
				OVRManager._hasVrFocus = OVRPlugin.hasVrFocus;
			}
			return OVRManager._hasVrFocus;
		}
		private set
		{
			OVRManager._hasVrFocusCached = true;
			OVRManager._hasVrFocus = value;
		}
	}

	// Token: 0x17000645 RID: 1605
	// (get) Token: 0x060039D3 RID: 14803 RVA: 0x0011A36C File Offset: 0x0011876C
	public static bool hasInputFocus
	{
		get
		{
			return OVRPlugin.hasInputFocus;
		}
	}

	// Token: 0x17000646 RID: 1606
	// (get) Token: 0x060039D4 RID: 14804 RVA: 0x0011A373 File Offset: 0x00118773
	// (set) Token: 0x060039D5 RID: 14805 RVA: 0x0011A386 File Offset: 0x00118786
	public bool chromatic
	{
		get
		{
			return OVRManager.isHmdPresent && OVRPlugin.chromatic;
		}
		set
		{
			if (!OVRManager.isHmdPresent)
			{
				return;
			}
			OVRPlugin.chromatic = value;
		}
	}

	// Token: 0x17000647 RID: 1607
	// (get) Token: 0x060039D6 RID: 14806 RVA: 0x0011A399 File Offset: 0x00118799
	// (set) Token: 0x060039D7 RID: 14807 RVA: 0x0011A3AC File Offset: 0x001187AC
	public bool monoscopic
	{
		get
		{
			return !OVRManager.isHmdPresent || OVRPlugin.monoscopic;
		}
		set
		{
			if (!OVRManager.isHmdPresent)
			{
				return;
			}
			OVRPlugin.monoscopic = value;
		}
	}

	// Token: 0x060039D8 RID: 14808 RVA: 0x0011A3BF File Offset: 0x001187BF
	public static bool IsAdaptiveResSupportedByEngine()
	{
		return Application.unityVersion != "2017.1.0f1";
	}

	// Token: 0x17000648 RID: 1608
	// (get) Token: 0x060039D9 RID: 14809 RVA: 0x0011A3D0 File Offset: 0x001187D0
	// (set) Token: 0x060039DA RID: 14810 RVA: 0x0011A3E3 File Offset: 0x001187E3
	public int vsyncCount
	{
		get
		{
			if (!OVRManager.isHmdPresent)
			{
				return 1;
			}
			return OVRPlugin.vsyncCount;
		}
		set
		{
			if (!OVRManager.isHmdPresent)
			{
				return;
			}
			OVRPlugin.vsyncCount = value;
		}
	}

	// Token: 0x17000649 RID: 1609
	// (get) Token: 0x060039DB RID: 14811 RVA: 0x0011A3F6 File Offset: 0x001187F6
	public static float batteryLevel
	{
		get
		{
			if (!OVRManager.isHmdPresent)
			{
				return 1f;
			}
			return OVRPlugin.batteryLevel;
		}
	}

	// Token: 0x1700064A RID: 1610
	// (get) Token: 0x060039DC RID: 14812 RVA: 0x0011A40D File Offset: 0x0011880D
	public static float batteryTemperature
	{
		get
		{
			if (!OVRManager.isHmdPresent)
			{
				return 0f;
			}
			return OVRPlugin.batteryTemperature;
		}
	}

	// Token: 0x1700064B RID: 1611
	// (get) Token: 0x060039DD RID: 14813 RVA: 0x0011A424 File Offset: 0x00118824
	public static int batteryStatus
	{
		get
		{
			if (!OVRManager.isHmdPresent)
			{
				return -1;
			}
			return (int)OVRPlugin.batteryStatus;
		}
	}

	// Token: 0x1700064C RID: 1612
	// (get) Token: 0x060039DE RID: 14814 RVA: 0x0011A437 File Offset: 0x00118837
	public static float volumeLevel
	{
		get
		{
			if (!OVRManager.isHmdPresent)
			{
				return 0f;
			}
			return OVRPlugin.systemVolume;
		}
	}

	// Token: 0x1700064D RID: 1613
	// (get) Token: 0x060039DF RID: 14815 RVA: 0x0011A44E File Offset: 0x0011884E
	// (set) Token: 0x060039E0 RID: 14816 RVA: 0x0011A461 File Offset: 0x00118861
	public static int cpuLevel
	{
		get
		{
			if (!OVRManager.isHmdPresent)
			{
				return 2;
			}
			return OVRPlugin.cpuLevel;
		}
		set
		{
			if (!OVRManager.isHmdPresent)
			{
				return;
			}
			OVRPlugin.cpuLevel = value;
		}
	}

	// Token: 0x1700064E RID: 1614
	// (get) Token: 0x060039E1 RID: 14817 RVA: 0x0011A474 File Offset: 0x00118874
	// (set) Token: 0x060039E2 RID: 14818 RVA: 0x0011A487 File Offset: 0x00118887
	public static int gpuLevel
	{
		get
		{
			if (!OVRManager.isHmdPresent)
			{
				return 2;
			}
			return OVRPlugin.gpuLevel;
		}
		set
		{
			if (!OVRManager.isHmdPresent)
			{
				return;
			}
			OVRPlugin.gpuLevel = value;
		}
	}

	// Token: 0x1700064F RID: 1615
	// (get) Token: 0x060039E3 RID: 14819 RVA: 0x0011A49A File Offset: 0x0011889A
	public static bool isPowerSavingActive
	{
		get
		{
			return OVRManager.isHmdPresent && OVRPlugin.powerSaving;
		}
	}

	// Token: 0x17000650 RID: 1616
	// (get) Token: 0x060039E4 RID: 14820 RVA: 0x0011A4AD File Offset: 0x001188AD
	// (set) Token: 0x060039E5 RID: 14821 RVA: 0x0011A4B4 File Offset: 0x001188B4
	public static OVRManager.EyeTextureFormat eyeTextureFormat
	{
		get
		{
			return (OVRManager.EyeTextureFormat)OVRPlugin.GetDesiredEyeTextureFormat();
		}
		set
		{
			OVRPlugin.SetDesiredEyeTextureFormat((OVRPlugin.EyeTextureFormat)value);
		}
	}

	// Token: 0x17000651 RID: 1617
	// (get) Token: 0x060039E6 RID: 14822 RVA: 0x0011A4BD File Offset: 0x001188BD
	public static bool tiledMultiResSupported
	{
		get
		{
			return OVRPlugin.tiledMultiResSupported;
		}
	}

	// Token: 0x17000652 RID: 1618
	// (get) Token: 0x060039E7 RID: 14823 RVA: 0x0011A4C4 File Offset: 0x001188C4
	// (set) Token: 0x060039E8 RID: 14824 RVA: 0x0011A4DF File Offset: 0x001188DF
	public static OVRManager.TiledMultiResLevel tiledMultiResLevel
	{
		get
		{
			if (!OVRPlugin.tiledMultiResSupported)
			{
				UnityEngine.Debug.LogWarning("Tiled-based Multi-resolution feature is not supported");
			}
			return (OVRManager.TiledMultiResLevel)OVRPlugin.tiledMultiResLevel;
		}
		set
		{
			if (!OVRPlugin.tiledMultiResSupported)
			{
				UnityEngine.Debug.LogWarning("Tiled-based Multi-resolution feature is not supported");
			}
			OVRPlugin.tiledMultiResLevel = (OVRPlugin.TiledMultiResLevel)value;
		}
	}

	// Token: 0x17000653 RID: 1619
	// (get) Token: 0x060039E9 RID: 14825 RVA: 0x0011A4FB File Offset: 0x001188FB
	public static bool gpuUtilSupported
	{
		get
		{
			return OVRPlugin.gpuUtilSupported;
		}
	}

	// Token: 0x17000654 RID: 1620
	// (get) Token: 0x060039EA RID: 14826 RVA: 0x0011A502 File Offset: 0x00118902
	public static float gpuUtilLevel
	{
		get
		{
			if (!OVRPlugin.gpuUtilSupported)
			{
				UnityEngine.Debug.LogWarning("GPU Util is not supported");
			}
			return OVRPlugin.gpuUtilLevel;
		}
	}

	// Token: 0x17000655 RID: 1621
	// (get) Token: 0x060039EB RID: 14827 RVA: 0x0011A51D File Offset: 0x0011891D
	// (set) Token: 0x060039EC RID: 14828 RVA: 0x0011A535 File Offset: 0x00118935
	public OVRManager.TrackingOrigin trackingOriginType
	{
		get
		{
			if (!OVRManager.isHmdPresent)
			{
				return this._trackingOriginType;
			}
			return (OVRManager.TrackingOrigin)OVRPlugin.GetTrackingOriginType();
		}
		set
		{
			if (!OVRManager.isHmdPresent)
			{
				return;
			}
			if (OVRPlugin.SetTrackingOriginType((OVRPlugin.TrackingOrigin)value))
			{
				this._trackingOriginType = value;
			}
		}
	}

	// Token: 0x17000656 RID: 1622
	// (get) Token: 0x060039ED RID: 14829 RVA: 0x0011A554 File Offset: 0x00118954
	// (set) Token: 0x060039EE RID: 14830 RVA: 0x0011A55C File Offset: 0x0011895C
	public bool isSupportedPlatform
	{
		[CompilerGenerated]
		get
		{
			return this.<isSupportedPlatform>k__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			this.<isSupportedPlatform>k__BackingField = value;
		}
	}

	// Token: 0x17000657 RID: 1623
	// (get) Token: 0x060039EF RID: 14831 RVA: 0x0011A565 File Offset: 0x00118965
	// (set) Token: 0x060039F0 RID: 14832 RVA: 0x0011A586 File Offset: 0x00118986
	public bool isUserPresent
	{
		get
		{
			if (!OVRManager._isUserPresentCached)
			{
				OVRManager._isUserPresentCached = true;
				OVRManager._isUserPresent = OVRPlugin.userPresent;
			}
			return OVRManager._isUserPresent;
		}
		private set
		{
			OVRManager._isUserPresentCached = true;
			OVRManager._isUserPresent = value;
		}
	}

	// Token: 0x17000658 RID: 1624
	// (get) Token: 0x060039F1 RID: 14833 RVA: 0x0011A594 File Offset: 0x00118994
	public static Version utilitiesVersion
	{
		get
		{
			return OVRPlugin.wrapperVersion;
		}
	}

	// Token: 0x17000659 RID: 1625
	// (get) Token: 0x060039F2 RID: 14834 RVA: 0x0011A59B File Offset: 0x0011899B
	public static Version pluginVersion
	{
		get
		{
			return OVRPlugin.version;
		}
	}

	// Token: 0x1700065A RID: 1626
	// (get) Token: 0x060039F3 RID: 14835 RVA: 0x0011A5A2 File Offset: 0x001189A2
	public static Version sdkVersion
	{
		get
		{
			return OVRPlugin.nativeSDKVersion;
		}
	}

	// Token: 0x060039F4 RID: 14836 RVA: 0x0011A5AC File Offset: 0x001189AC
	private static bool MixedRealityEnabledFromCmd()
	{
		string[] commandLineArgs = Environment.GetCommandLineArgs();
		for (int i = 0; i < commandLineArgs.Length; i++)
		{
			if (commandLineArgs[i].ToLower() == "-mixedreality")
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060039F5 RID: 14837 RVA: 0x0011A5F0 File Offset: 0x001189F0
	private static bool UseDirectCompositionFromCmd()
	{
		string[] commandLineArgs = Environment.GetCommandLineArgs();
		for (int i = 0; i < commandLineArgs.Length; i++)
		{
			if (commandLineArgs[i].ToLower() == "-directcomposition")
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060039F6 RID: 14838 RVA: 0x0011A634 File Offset: 0x00118A34
	private static bool UseExternalCompositionFromCmd()
	{
		string[] commandLineArgs = Environment.GetCommandLineArgs();
		for (int i = 0; i < commandLineArgs.Length; i++)
		{
			if (commandLineArgs[i].ToLower() == "-externalcomposition")
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060039F7 RID: 14839 RVA: 0x0011A678 File Offset: 0x00118A78
	private static bool CreateMixedRealityCaptureConfigurationFileFromCmd()
	{
		string[] commandLineArgs = Environment.GetCommandLineArgs();
		for (int i = 0; i < commandLineArgs.Length; i++)
		{
			if (commandLineArgs[i].ToLower() == "-create_mrc_config")
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060039F8 RID: 14840 RVA: 0x0011A6BC File Offset: 0x00118ABC
	private static bool LoadMixedRealityCaptureConfigurationFileFromCmd()
	{
		string[] commandLineArgs = Environment.GetCommandLineArgs();
		for (int i = 0; i < commandLineArgs.Length; i++)
		{
			if (commandLineArgs[i].ToLower() == "-load_mrc_config")
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060039F9 RID: 14841 RVA: 0x0011A700 File Offset: 0x00118B00
	private void Awake()
	{
		if (OVRManager.instance != null)
		{
			base.enabled = false;
			UnityEngine.Object.DestroyImmediate(this);
			return;
		}
		OVRManager.instance = this;
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"Unity v",
			Application.unityVersion,
			", Oculus Utilities v",
			OVRPlugin.wrapperVersion,
			", OVRPlugin v",
			OVRPlugin.version,
			", SDK v",
			OVRPlugin.nativeSDKVersion,
			"."
		}));
		string text = GraphicsDeviceType.Direct3D11.ToString() + ", " + GraphicsDeviceType.Direct3D12.ToString();
		if (!text.Contains(SystemInfo.graphicsDeviceType.ToString()))
		{
			UnityEngine.Debug.LogWarning("VR rendering requires one of the following device types: (" + text + "). Your graphics device: " + SystemInfo.graphicsDeviceType.ToString());
		}
		RuntimePlatform platform = Application.platform;
		this.isSupportedPlatform |= (platform == RuntimePlatform.Android);
		this.isSupportedPlatform |= (platform == RuntimePlatform.OSXEditor);
		this.isSupportedPlatform |= (platform == RuntimePlatform.OSXPlayer);
		this.isSupportedPlatform |= (platform == RuntimePlatform.WindowsEditor);
		this.isSupportedPlatform |= (platform == RuntimePlatform.WindowsPlayer);
		if (!this.isSupportedPlatform)
		{
			UnityEngine.Debug.LogWarning("This platform is unsupported");
			return;
		}
		this.enableMixedReality = false;
		bool flag = OVRManager.LoadMixedRealityCaptureConfigurationFileFromCmd();
		bool flag2 = OVRManager.CreateMixedRealityCaptureConfigurationFileFromCmd();
		if (flag || flag2)
		{
			OVRMixedRealityCaptureSettings ovrmixedRealityCaptureSettings = ScriptableObject.CreateInstance<OVRMixedRealityCaptureSettings>();
			ovrmixedRealityCaptureSettings.ReadFrom(this);
			if (flag)
			{
				ovrmixedRealityCaptureSettings.CombineWithConfigurationFile();
				ovrmixedRealityCaptureSettings.ApplyTo(this);
			}
			if (flag2)
			{
				ovrmixedRealityCaptureSettings.WriteToConfigurationFile();
			}
			UnityEngine.Object.Destroy(ovrmixedRealityCaptureSettings);
		}
		if (OVRManager.MixedRealityEnabledFromCmd())
		{
			this.enableMixedReality = true;
		}
		if (this.enableMixedReality)
		{
			UnityEngine.Debug.Log("OVR: Mixed Reality mode enabled");
			if (OVRManager.UseDirectCompositionFromCmd())
			{
				this.compositionMethod = OVRManager.CompositionMethod.Direct;
			}
			if (OVRManager.UseExternalCompositionFromCmd())
			{
				this.compositionMethod = OVRManager.CompositionMethod.External;
			}
			UnityEngine.Debug.Log("OVR: CompositionMethod : " + this.compositionMethod);
		}
		if (this.enableAdaptiveResolution && !OVRManager.IsAdaptiveResSupportedByEngine())
		{
			this.enableAdaptiveResolution = false;
			UnityEngine.Debug.LogError("Your current Unity Engine " + Application.unityVersion + " might have issues to support adaptive resolution, please disable it under OVRManager");
		}
		this.Initialize();
		if (this.resetTrackerOnLoad)
		{
			OVRManager.display.RecenterPose();
		}
		OVRPlugin.occlusionMesh = true;
	}

	// Token: 0x060039FA RID: 14842 RVA: 0x0011A988 File Offset: 0x00118D88
	private void Initialize()
	{
		if (OVRManager.display == null)
		{
			OVRManager.display = new OVRDisplay();
		}
		if (OVRManager.tracker == null)
		{
			OVRManager.tracker = new OVRTracker();
		}
		if (OVRManager.boundary == null)
		{
			OVRManager.boundary = new OVRBoundary();
		}
	}

	// Token: 0x060039FB RID: 14843 RVA: 0x0011A9C8 File Offset: 0x00118DC8
	private void Update()
	{
		if (OVRPlugin.shouldQuit)
		{
			Application.Quit();
		}
		if (this.AllowRecenter && OVRPlugin.shouldRecenter)
		{
			OVRManager.display.RecenterPose();
		}
		if (this.trackingOriginType != this._trackingOriginType)
		{
			this.trackingOriginType = this._trackingOriginType;
		}
		OVRManager.tracker.isEnabled = this.usePositionTracking;
		OVRPlugin.rotation = this.useRotationTracking;
		OVRPlugin.useIPDInPositionTracking = this.useIPDInPositionTracking;
		OVRManager.isHmdPresent = OVRPlugin.hmdPresent;
		if (this.useRecommendedMSAALevel && QualitySettings.antiAliasing != OVRManager.display.recommendedMSAALevel)
		{
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"The current MSAA level is ",
				QualitySettings.antiAliasing,
				", but the recommended MSAA level is ",
				OVRManager.display.recommendedMSAALevel,
				". Switching to the recommended level."
			}));
			QualitySettings.antiAliasing = OVRManager.display.recommendedMSAALevel;
		}
		if (OVRManager._wasHmdPresent && !OVRManager.isHmdPresent)
		{
			try
			{
				if (OVRManager.HMDLost != null)
				{
					OVRManager.HMDLost();
				}
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("Caught Exception: " + arg);
			}
		}
		if (!OVRManager._wasHmdPresent && OVRManager.isHmdPresent)
		{
			try
			{
				if (OVRManager.HMDAcquired != null)
				{
					OVRManager.HMDAcquired();
				}
			}
			catch (Exception arg2)
			{
				UnityEngine.Debug.LogError("Caught Exception: " + arg2);
			}
		}
		OVRManager._wasHmdPresent = OVRManager.isHmdPresent;
		this.isUserPresent = OVRPlugin.userPresent;
		if (OVRManager._wasUserPresent && !this.isUserPresent)
		{
			try
			{
				if (OVRManager.HMDUnmounted != null)
				{
					OVRManager.HMDUnmounted();
				}
			}
			catch (Exception arg3)
			{
				UnityEngine.Debug.LogError("Caught Exception: " + arg3);
			}
		}
		if (!OVRManager._wasUserPresent && this.isUserPresent)
		{
			try
			{
				if (OVRManager.HMDMounted != null)
				{
					OVRManager.HMDMounted();
				}
			}
			catch (Exception arg4)
			{
				UnityEngine.Debug.LogError("Caught Exception: " + arg4);
			}
		}
		OVRManager._wasUserPresent = this.isUserPresent;
		OVRManager.hasVrFocus = OVRPlugin.hasVrFocus;
		if (OVRManager._hadVrFocus && !OVRManager.hasVrFocus)
		{
			try
			{
				if (OVRManager.VrFocusLost != null)
				{
					OVRManager.VrFocusLost();
				}
			}
			catch (Exception arg5)
			{
				UnityEngine.Debug.LogError("Caught Exception: " + arg5);
			}
		}
		if (!OVRManager._hadVrFocus && OVRManager.hasVrFocus)
		{
			try
			{
				if (OVRManager.VrFocusAcquired != null)
				{
					OVRManager.VrFocusAcquired();
				}
			}
			catch (Exception arg6)
			{
				UnityEngine.Debug.LogError("Caught Exception: " + arg6);
			}
		}
		OVRManager._hadVrFocus = OVRManager.hasVrFocus;
		bool hasInputFocus = OVRPlugin.hasInputFocus;
		if (OVRManager._hadInputFocus && !hasInputFocus)
		{
			try
			{
				if (OVRManager.InputFocusLost != null)
				{
					OVRManager.InputFocusLost();
				}
			}
			catch (Exception arg7)
			{
				UnityEngine.Debug.LogError("Caught Exception: " + arg7);
			}
		}
		if (!OVRManager._hadInputFocus && hasInputFocus)
		{
			try
			{
				if (OVRManager.InputFocusAcquired != null)
				{
					OVRManager.InputFocusAcquired();
				}
			}
			catch (Exception arg8)
			{
				UnityEngine.Debug.LogError("Caught Exception: " + arg8);
			}
		}
		OVRManager._hadInputFocus = hasInputFocus;
		if (this.enableAdaptiveResolution)
		{
			if (XRSettings.eyeTextureResolutionScale < this.maxRenderScale)
			{
				XRSettings.eyeTextureResolutionScale = this.maxRenderScale;
			}
			else
			{
				this.maxRenderScale = Mathf.Max(this.maxRenderScale, XRSettings.eyeTextureResolutionScale);
			}
			this.minRenderScale = Mathf.Min(this.minRenderScale, this.maxRenderScale);
			float min = this.minRenderScale / XRSettings.eyeTextureResolutionScale;
			float num = OVRPlugin.GetEyeRecommendedResolutionScale() / XRSettings.eyeTextureResolutionScale;
			num = Mathf.Clamp(num, min, 1f);
			XRSettings.renderViewportScale = num;
		}
		string audioOutId = OVRPlugin.audioOutId;
		if (!OVRManager.prevAudioOutIdIsCached)
		{
			OVRManager.prevAudioOutId = audioOutId;
			OVRManager.prevAudioOutIdIsCached = true;
		}
		else if (audioOutId != OVRManager.prevAudioOutId)
		{
			try
			{
				if (OVRManager.AudioOutChanged != null)
				{
					OVRManager.AudioOutChanged();
				}
			}
			catch (Exception arg9)
			{
				UnityEngine.Debug.LogError("Caught Exception: " + arg9);
			}
			OVRManager.prevAudioOutId = audioOutId;
		}
		string audioInId = OVRPlugin.audioInId;
		if (!OVRManager.prevAudioInIdIsCached)
		{
			OVRManager.prevAudioInId = audioInId;
			OVRManager.prevAudioInIdIsCached = true;
		}
		else if (audioInId != OVRManager.prevAudioInId)
		{
			try
			{
				if (OVRManager.AudioInChanged != null)
				{
					OVRManager.AudioInChanged();
				}
			}
			catch (Exception arg10)
			{
				UnityEngine.Debug.LogError("Caught Exception: " + arg10);
			}
			OVRManager.prevAudioInId = audioInId;
		}
		if (OVRManager.wasPositionTracked && !OVRManager.tracker.isPositionTracked)
		{
			try
			{
				if (OVRManager.TrackingLost != null)
				{
					OVRManager.TrackingLost();
				}
			}
			catch (Exception arg11)
			{
				UnityEngine.Debug.LogError("Caught Exception: " + arg11);
			}
		}
		if (!OVRManager.wasPositionTracked && OVRManager.tracker.isPositionTracked)
		{
			try
			{
				if (OVRManager.TrackingAcquired != null)
				{
					OVRManager.TrackingAcquired();
				}
			}
			catch (Exception arg12)
			{
				UnityEngine.Debug.LogError("Caught Exception: " + arg12);
			}
		}
		OVRManager.wasPositionTracked = OVRManager.tracker.isPositionTracked;
		OVRManager.display.Update();
		OVRInput.Update();
		if (this.enableMixedReality || OVRManager.prevEnableMixedReality)
		{
			Camera mainCamera = this.FindMainCamera();
			if (Camera.main != null)
			{
				this.suppressDisableMixedRealityBecauseOfNoMainCameraWarning = false;
				if (this.enableMixedReality)
				{
					OVRMixedReality.Update(base.gameObject, mainCamera, this.compositionMethod, this.useDynamicLighting, this.capturingCameraDevice, this.depthQuality);
				}
				if (OVRManager.prevEnableMixedReality && !this.enableMixedReality)
				{
					OVRMixedReality.Cleanup();
				}
				OVRManager.prevEnableMixedReality = this.enableMixedReality;
			}
			else if (!this.suppressDisableMixedRealityBecauseOfNoMainCameraWarning)
			{
				UnityEngine.Debug.LogWarning("Main Camera is not set, Mixed Reality disabled");
				this.suppressDisableMixedRealityBecauseOfNoMainCameraWarning = true;
			}
		}
	}

	// Token: 0x060039FC RID: 14844 RVA: 0x0011B078 File Offset: 0x00119478
	private Camera FindMainCamera()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("MainCamera");
		List<Camera> list = new List<Camera>(4);
		foreach (GameObject gameObject in array)
		{
			Camera component = gameObject.GetComponent<Camera>();
			if (component != null && component.enabled)
			{
				OVRCameraRig componentInParent = component.GetComponentInParent<OVRCameraRig>();
				if (componentInParent != null && componentInParent.trackingSpace != null)
				{
					list.Add(component);
				}
			}
		}
		if (list.Count == 0)
		{
			return Camera.main;
		}
		if (list.Count == 1)
		{
			return list[0];
		}
		if (!this.multipleMainCameraWarningPresented)
		{
			UnityEngine.Debug.LogWarning("Multiple MainCamera found. Assume the real MainCamera is the camera with the least depth");
			this.multipleMainCameraWarningPresented = true;
		}
		List<Camera> list2 = list;
		if (OVRManager.<>f__am$cache0 == null)
		{
			OVRManager.<>f__am$cache0 = new Comparison<Camera>(OVRManager.<FindMainCamera>m__0);
		}
		list2.Sort(OVRManager.<>f__am$cache0);
		return list[0];
	}

	// Token: 0x060039FD RID: 14845 RVA: 0x0011B172 File Offset: 0x00119572
	private void OnDisable()
	{
		OVRMixedReality.Cleanup();
	}

	// Token: 0x060039FE RID: 14846 RVA: 0x0011B179 File Offset: 0x00119579
	private void LateUpdate()
	{
		OVRHaptics.Process();
	}

	// Token: 0x060039FF RID: 14847 RVA: 0x0011B180 File Offset: 0x00119580
	private void FixedUpdate()
	{
		OVRInput.FixedUpdate();
	}

	// Token: 0x06003A00 RID: 14848 RVA: 0x0011B187 File Offset: 0x00119587
	public void ReturnToLauncher()
	{
		OVRManager.PlatformUIConfirmQuit();
	}

	// Token: 0x06003A01 RID: 14849 RVA: 0x0011B18E File Offset: 0x0011958E
	public static void PlatformUIConfirmQuit()
	{
		if (!OVRManager.isHmdPresent)
		{
			return;
		}
		OVRPlugin.ShowUI(OVRPlugin.PlatformUI.ConfirmQuit);
	}

	// Token: 0x06003A02 RID: 14850 RVA: 0x0011B1A4 File Offset: 0x001195A4
	// Note: this type is marked as 'beforefieldinit'.
	static OVRManager()
	{
	}

	// Token: 0x06003A03 RID: 14851 RVA: 0x0011B219 File Offset: 0x00119619
	[CompilerGenerated]
	private static int <FindMainCamera>m__0(Camera c0, Camera c1)
	{
		return (c0.depth >= c1.depth) ? ((c0.depth <= c1.depth) ? 0 : 1) : -1;
	}

	// Token: 0x04002B23 RID: 11043
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static OVRManager <instance>k__BackingField;

	// Token: 0x04002B24 RID: 11044
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static OVRDisplay <display>k__BackingField;

	// Token: 0x04002B25 RID: 11045
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static OVRTracker <tracker>k__BackingField;

	// Token: 0x04002B26 RID: 11046
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static OVRBoundary <boundary>k__BackingField;

	// Token: 0x04002B27 RID: 11047
	private static OVRProfile _profile;

	// Token: 0x04002B28 RID: 11048
	private IEnumerable<Camera> disabledCameras;

	// Token: 0x04002B29 RID: 11049
	private float prevTimeScale;

	// Token: 0x04002B2A RID: 11050
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action HMDAcquired;

	// Token: 0x04002B2B RID: 11051
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action HMDLost;

	// Token: 0x04002B2C RID: 11052
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action HMDMounted;

	// Token: 0x04002B2D RID: 11053
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action HMDUnmounted;

	// Token: 0x04002B2E RID: 11054
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action VrFocusAcquired;

	// Token: 0x04002B2F RID: 11055
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action VrFocusLost;

	// Token: 0x04002B30 RID: 11056
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action InputFocusAcquired;

	// Token: 0x04002B31 RID: 11057
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action InputFocusLost;

	// Token: 0x04002B32 RID: 11058
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action AudioOutChanged;

	// Token: 0x04002B33 RID: 11059
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action AudioInChanged;

	// Token: 0x04002B34 RID: 11060
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action TrackingAcquired;

	// Token: 0x04002B35 RID: 11061
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action TrackingLost;

	// Token: 0x04002B36 RID: 11062
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action HSWDismissed;

	// Token: 0x04002B37 RID: 11063
	private static bool _isHmdPresentCached = false;

	// Token: 0x04002B38 RID: 11064
	private static bool _isHmdPresent = false;

	// Token: 0x04002B39 RID: 11065
	private static bool _wasHmdPresent = false;

	// Token: 0x04002B3A RID: 11066
	private static bool _hasVrFocusCached = false;

	// Token: 0x04002B3B RID: 11067
	private static bool _hasVrFocus = false;

	// Token: 0x04002B3C RID: 11068
	private static bool _hadVrFocus = false;

	// Token: 0x04002B3D RID: 11069
	private static bool _hadInputFocus = true;

	// Token: 0x04002B3E RID: 11070
	[Header("Performance/Quality")]
	[Tooltip("If true, distortion rendering work is submitted a quarter-frame early to avoid pipeline stalls and increase CPU-GPU parallelism.")]
	public bool queueAhead = true;

	// Token: 0x04002B3F RID: 11071
	[Tooltip("If true, Unity will use the optimal antialiasing level for quality/performance on the current hardware.")]
	public bool useRecommendedMSAALevel;

	// Token: 0x04002B40 RID: 11072
	[Tooltip("If true, dynamic resolution will be enabled On PC")]
	public bool enableAdaptiveResolution;

	// Token: 0x04002B41 RID: 11073
	[Range(0.5f, 2f)]
	[Tooltip("Min RenderScale the app can reach under adaptive resolution mode")]
	public float minRenderScale = 0.7f;

	// Token: 0x04002B42 RID: 11074
	[Range(0.5f, 2f)]
	[Tooltip("Max RenderScale the app can reach under adaptive resolution mode")]
	public float maxRenderScale = 1f;

	// Token: 0x04002B43 RID: 11075
	[HideInInspector]
	public bool expandMixedRealityCapturePropertySheet;

	// Token: 0x04002B44 RID: 11076
	[HideInInspector]
	[Tooltip("If true, Mixed Reality mode will be enabled. It would be always set to false when the game is launching without editor")]
	public bool enableMixedReality;

	// Token: 0x04002B45 RID: 11077
	[HideInInspector]
	public OVRManager.CompositionMethod compositionMethod;

	// Token: 0x04002B46 RID: 11078
	[HideInInspector]
	[Tooltip("Extra hidden layers")]
	public LayerMask extraHiddenLayers;

	// Token: 0x04002B47 RID: 11079
	[HideInInspector]
	[Tooltip("The camera device for direct composition")]
	public OVRManager.CameraDevice capturingCameraDevice;

	// Token: 0x04002B48 RID: 11080
	[HideInInspector]
	[Tooltip("Flip the camera frame horizontally")]
	public bool flipCameraFrameHorizontally;

	// Token: 0x04002B49 RID: 11081
	[HideInInspector]
	[Tooltip("Flip the camera frame vertically")]
	public bool flipCameraFrameVertically;

	// Token: 0x04002B4A RID: 11082
	[HideInInspector]
	[Tooltip("Delay the touch controller pose by a short duration (0 to 0.5 second) to match the physical camera latency")]
	public float handPoseStateLatency;

	// Token: 0x04002B4B RID: 11083
	[HideInInspector]
	[Tooltip("Delay the foreground / background image in the sandwich composition to match the physical camera latency. The maximum duration is sandwichCompositionBufferedFrames / {Game FPS}")]
	public float sandwichCompositionRenderLatency;

	// Token: 0x04002B4C RID: 11084
	[HideInInspector]
	[Tooltip("The number of frames are buffered in the SandWich composition. The more buffered frames, the more memory it would consume.")]
	public int sandwichCompositionBufferedFrames = 8;

	// Token: 0x04002B4D RID: 11085
	[HideInInspector]
	[Tooltip("Chroma Key Color")]
	public Color chromaKeyColor = Color.green;

	// Token: 0x04002B4E RID: 11086
	[HideInInspector]
	[Tooltip("Chroma Key Similarity")]
	public float chromaKeySimilarity = 0.6f;

	// Token: 0x04002B4F RID: 11087
	[HideInInspector]
	[Tooltip("Chroma Key Smooth Range")]
	public float chromaKeySmoothRange = 0.03f;

	// Token: 0x04002B50 RID: 11088
	[HideInInspector]
	[Tooltip("Chroma Key Spill Range")]
	public float chromaKeySpillRange = 0.06f;

	// Token: 0x04002B51 RID: 11089
	[HideInInspector]
	[Tooltip("Use dynamic lighting (Depth sensor required)")]
	public bool useDynamicLighting;

	// Token: 0x04002B52 RID: 11090
	[HideInInspector]
	[Tooltip("The quality level of depth image. The lighting could be more smooth and accurate with high quality depth, but it would also be more costly in performance.")]
	public OVRManager.DepthQuality depthQuality = OVRManager.DepthQuality.Medium;

	// Token: 0x04002B53 RID: 11091
	[HideInInspector]
	[Tooltip("Smooth factor in dynamic lighting. Larger is smoother")]
	public float dynamicLightingSmoothFactor = 8f;

	// Token: 0x04002B54 RID: 11092
	[HideInInspector]
	[Tooltip("The maximum depth variation across the edges. Make it smaller to smooth the lighting on the edges.")]
	public float dynamicLightingDepthVariationClampingValue = 0.001f;

	// Token: 0x04002B55 RID: 11093
	[HideInInspector]
	[Tooltip("Type of virutal green screen ")]
	public OVRManager.VirtualGreenScreenType virtualGreenScreenType;

	// Token: 0x04002B56 RID: 11094
	[HideInInspector]
	[Tooltip("Top Y of virtual green screen")]
	public float virtualGreenScreenTopY = 10f;

	// Token: 0x04002B57 RID: 11095
	[HideInInspector]
	[Tooltip("Bottom Y of virtual green screen")]
	public float virtualGreenScreenBottomY = -10f;

	// Token: 0x04002B58 RID: 11096
	[HideInInspector]
	[Tooltip("When using a depth camera (e.g. ZED), whether to use the depth in virtual green screen culling.")]
	public bool virtualGreenScreenApplyDepthCulling;

	// Token: 0x04002B59 RID: 11097
	[HideInInspector]
	[Tooltip("The tolerance value (in meter) when using the virtual green screen with a depth camera. Make it bigger if the foreground objects got culled incorrectly.")]
	public float virtualGreenScreenDepthTolerance = 0.2f;

	// Token: 0x04002B5A RID: 11098
	[Header("Tracking")]
	[SerializeField]
	[Tooltip("Defines the current tracking origin type.")]
	private OVRManager.TrackingOrigin _trackingOriginType;

	// Token: 0x04002B5B RID: 11099
	[Tooltip("If true, head tracking will affect the position of each OVRCameraRig's cameras.")]
	public bool usePositionTracking = true;

	// Token: 0x04002B5C RID: 11100
	[HideInInspector]
	public bool useRotationTracking = true;

	// Token: 0x04002B5D RID: 11101
	[Tooltip("If true, the distance between the user's eyes will affect the position of each OVRCameraRig's cameras.")]
	public bool useIPDInPositionTracking = true;

	// Token: 0x04002B5E RID: 11102
	[Tooltip("If true, each scene load will cause the head pose to reset.")]
	public bool resetTrackerOnLoad;

	// Token: 0x04002B5F RID: 11103
	[Tooltip("If true, the Reset View in the universal menu will cause the pose to be reset. This should generally be enabled for applications with a stationary position in the virtual world and will allow the View Reset command to place the person back to a predefined location (such as a cockpit seat). Set this to false if you have a locomotion system because resetting the view would effectively teleport the player to potentially invalid locations.")]
	public bool AllowRecenter = true;

	// Token: 0x04002B60 RID: 11104
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private bool <isSupportedPlatform>k__BackingField;

	// Token: 0x04002B61 RID: 11105
	private static bool _isUserPresentCached = false;

	// Token: 0x04002B62 RID: 11106
	private static bool _isUserPresent = false;

	// Token: 0x04002B63 RID: 11107
	private static bool _wasUserPresent = false;

	// Token: 0x04002B64 RID: 11108
	private static bool prevAudioOutIdIsCached = false;

	// Token: 0x04002B65 RID: 11109
	private static bool prevAudioInIdIsCached = false;

	// Token: 0x04002B66 RID: 11110
	private static string prevAudioOutId = string.Empty;

	// Token: 0x04002B67 RID: 11111
	private static string prevAudioInId = string.Empty;

	// Token: 0x04002B68 RID: 11112
	private static bool wasPositionTracked = false;

	// Token: 0x04002B69 RID: 11113
	private static bool prevEnableMixedReality = false;

	// Token: 0x04002B6A RID: 11114
	private bool suppressDisableMixedRealityBecauseOfNoMainCameraWarning;

	// Token: 0x04002B6B RID: 11115
	private bool multipleMainCameraWarningPresented;

	// Token: 0x04002B6C RID: 11116
	[CompilerGenerated]
	private static Comparison<Camera> <>f__am$cache0;

	// Token: 0x020008F9 RID: 2297
	public enum TrackingOrigin
	{
		// Token: 0x04002B6E RID: 11118
		EyeLevel,
		// Token: 0x04002B6F RID: 11119
		FloorLevel
	}

	// Token: 0x020008FA RID: 2298
	public enum EyeTextureFormat
	{
		// Token: 0x04002B71 RID: 11121
		Default,
		// Token: 0x04002B72 RID: 11122
		R16G16B16A16_FP = 2,
		// Token: 0x04002B73 RID: 11123
		R11G11B10_FP
	}

	// Token: 0x020008FB RID: 2299
	public enum TiledMultiResLevel
	{
		// Token: 0x04002B75 RID: 11125
		Off,
		// Token: 0x04002B76 RID: 11126
		LMSLow,
		// Token: 0x04002B77 RID: 11127
		LMSMedium,
		// Token: 0x04002B78 RID: 11128
		LMSHigh
	}

	// Token: 0x020008FC RID: 2300
	public enum CompositionMethod
	{
		// Token: 0x04002B7A RID: 11130
		External,
		// Token: 0x04002B7B RID: 11131
		Direct,
		// Token: 0x04002B7C RID: 11132
		Sandwich
	}

	// Token: 0x020008FD RID: 2301
	public enum CameraDevice
	{
		// Token: 0x04002B7E RID: 11134
		WebCamera0,
		// Token: 0x04002B7F RID: 11135
		WebCamera1,
		// Token: 0x04002B80 RID: 11136
		ZEDCamera
	}

	// Token: 0x020008FE RID: 2302
	public enum DepthQuality
	{
		// Token: 0x04002B82 RID: 11138
		Low,
		// Token: 0x04002B83 RID: 11139
		Medium,
		// Token: 0x04002B84 RID: 11140
		High
	}

	// Token: 0x020008FF RID: 2303
	public enum VirtualGreenScreenType
	{
		// Token: 0x04002B86 RID: 11142
		Off,
		// Token: 0x04002B87 RID: 11143
		OuterBoundary,
		// Token: 0x04002B88 RID: 11144
		PlayArea
	}
}
