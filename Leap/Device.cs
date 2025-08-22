using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using LeapInternal;

namespace Leap
{
	// Token: 0x020005BE RID: 1470
	public class Device : IEquatable<Device>
	{
		// Token: 0x06002545 RID: 9541 RVA: 0x000D673D File Offset: 0x000D4B3D
		public Device()
		{
		}

		// Token: 0x06002546 RID: 9542 RVA: 0x000D6748 File Offset: 0x000D4B48
		public Device(IntPtr deviceHandle, float horizontalViewAngle, float verticalViewAngle, float range, float baseline, Device.DeviceType type, bool isStreaming, string serialNumber)
		{
			this.Handle = deviceHandle;
			this.HorizontalViewAngle = horizontalViewAngle;
			this.VerticalViewAngle = verticalViewAngle;
			this.Range = range;
			this.Baseline = baseline;
			this.Type = type;
			this.IsStreaming = isStreaming;
			this.SerialNumber = serialNumber;
		}

		// Token: 0x06002547 RID: 9543 RVA: 0x000D6798 File Offset: 0x000D4B98
		public void Update(float horizontalViewAngle, float verticalViewAngle, float range, float baseline, bool isStreaming, string serialNumber)
		{
			this.HorizontalViewAngle = horizontalViewAngle;
			this.VerticalViewAngle = verticalViewAngle;
			this.Range = range;
			this.Baseline = baseline;
			this.IsStreaming = isStreaming;
			this.SerialNumber = serialNumber;
		}

		// Token: 0x06002548 RID: 9544 RVA: 0x000D67C8 File Offset: 0x000D4BC8
		public void Update(Device updatedDevice)
		{
			this.HorizontalViewAngle = updatedDevice.HorizontalViewAngle;
			this.VerticalViewAngle = updatedDevice.VerticalViewAngle;
			this.Range = updatedDevice.Range;
			this.Baseline = updatedDevice.Baseline;
			this.IsStreaming = updatedDevice.IsStreaming;
			this.SerialNumber = updatedDevice.SerialNumber;
		}

		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x06002549 RID: 9545 RVA: 0x000D681D File Offset: 0x000D4C1D
		// (set) Token: 0x0600254A RID: 9546 RVA: 0x000D6825 File Offset: 0x000D4C25
		public IntPtr Handle
		{
			[CompilerGenerated]
			get
			{
				return this.<Handle>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Handle>k__BackingField = value;
			}
		}

		// Token: 0x0600254B RID: 9547 RVA: 0x000D6830 File Offset: 0x000D4C30
		public bool SetPaused(bool pause)
		{
			ulong num = 0UL;
			ulong set = 0UL;
			ulong clear = 0UL;
			if (pause)
			{
				set = 1UL;
			}
			else
			{
				clear = 1UL;
			}
			eLeapRS eLeapRS = LeapC.SetDeviceFlags(this.Handle, set, clear, out num);
			return eLeapRS == eLeapRS.eLeapRS_Success;
		}

		// Token: 0x0600254C RID: 9548 RVA: 0x000D686B File Offset: 0x000D4C6B
		public bool Equals(Device other)
		{
			return this.SerialNumber == other.SerialNumber;
		}

		// Token: 0x0600254D RID: 9549 RVA: 0x000D687E File Offset: 0x000D4C7E
		public override string ToString()
		{
			return "Device serial# " + this.SerialNumber;
		}

		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x0600254E RID: 9550 RVA: 0x000D6890 File Offset: 0x000D4C90
		// (set) Token: 0x0600254F RID: 9551 RVA: 0x000D6898 File Offset: 0x000D4C98
		public float HorizontalViewAngle
		{
			[CompilerGenerated]
			get
			{
				return this.<HorizontalViewAngle>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<HorizontalViewAngle>k__BackingField = value;
			}
		}

		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x06002550 RID: 9552 RVA: 0x000D68A1 File Offset: 0x000D4CA1
		// (set) Token: 0x06002551 RID: 9553 RVA: 0x000D68A9 File Offset: 0x000D4CA9
		public float VerticalViewAngle
		{
			[CompilerGenerated]
			get
			{
				return this.<VerticalViewAngle>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<VerticalViewAngle>k__BackingField = value;
			}
		}

		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x06002552 RID: 9554 RVA: 0x000D68B2 File Offset: 0x000D4CB2
		// (set) Token: 0x06002553 RID: 9555 RVA: 0x000D68BA File Offset: 0x000D4CBA
		public float Range
		{
			[CompilerGenerated]
			get
			{
				return this.<Range>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Range>k__BackingField = value;
			}
		}

		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x06002554 RID: 9556 RVA: 0x000D68C3 File Offset: 0x000D4CC3
		// (set) Token: 0x06002555 RID: 9557 RVA: 0x000D68CB File Offset: 0x000D4CCB
		public float Baseline
		{
			[CompilerGenerated]
			get
			{
				return this.<Baseline>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Baseline>k__BackingField = value;
			}
		}

		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x06002556 RID: 9558 RVA: 0x000D68D4 File Offset: 0x000D4CD4
		// (set) Token: 0x06002557 RID: 9559 RVA: 0x000D68DC File Offset: 0x000D4CDC
		public bool IsStreaming
		{
			[CompilerGenerated]
			get
			{
				return this.<IsStreaming>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<IsStreaming>k__BackingField = value;
			}
		}

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x06002558 RID: 9560 RVA: 0x000D68E5 File Offset: 0x000D4CE5
		// (set) Token: 0x06002559 RID: 9561 RVA: 0x000D68ED File Offset: 0x000D4CED
		public Device.DeviceType Type
		{
			[CompilerGenerated]
			get
			{
				return this.<Type>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Type>k__BackingField = value;
			}
		}

		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x0600255A RID: 9562 RVA: 0x000D68F6 File Offset: 0x000D4CF6
		// (set) Token: 0x0600255B RID: 9563 RVA: 0x000D68FE File Offset: 0x000D4CFE
		public string SerialNumber
		{
			[CompilerGenerated]
			get
			{
				return this.<SerialNumber>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<SerialNumber>k__BackingField = value;
			}
		}

		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x0600255C RID: 9564 RVA: 0x000D6907 File Offset: 0x000D4D07
		public bool IsSmudged
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x0600255D RID: 9565 RVA: 0x000D690E File Offset: 0x000D4D0E
		public bool IsLightingBad
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x04001F1B RID: 7963
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private IntPtr <Handle>k__BackingField;

		// Token: 0x04001F1C RID: 7964
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private float <HorizontalViewAngle>k__BackingField;

		// Token: 0x04001F1D RID: 7965
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private float <VerticalViewAngle>k__BackingField;

		// Token: 0x04001F1E RID: 7966
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private float <Range>k__BackingField;

		// Token: 0x04001F1F RID: 7967
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private float <Baseline>k__BackingField;

		// Token: 0x04001F20 RID: 7968
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool <IsStreaming>k__BackingField;

		// Token: 0x04001F21 RID: 7969
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Device.DeviceType <Type>k__BackingField;

		// Token: 0x04001F22 RID: 7970
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <SerialNumber>k__BackingField;

		// Token: 0x020005BF RID: 1471
		public enum DeviceType
		{
			// Token: 0x04001F24 RID: 7972
			TYPE_INVALID = -1,
			// Token: 0x04001F25 RID: 7973
			TYPE_PERIPHERAL = 3,
			// Token: 0x04001F26 RID: 7974
			TYPE_DRAGONFLY = 4354,
			// Token: 0x04001F27 RID: 7975
			TYPE_NIGHTCRAWLER = 4609,
			// Token: 0x04001F28 RID: 7976
			TYPE_RIGEL,
			// Token: 0x04001F29 RID: 7977
			[Obsolete]
			TYPE_LAPTOP,
			// Token: 0x04001F2A RID: 7978
			[Obsolete]
			TYPE_KEYBOARD
		}
	}
}
