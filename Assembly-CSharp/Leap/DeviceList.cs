using System;
using System.Collections.Generic;

namespace Leap
{
	// Token: 0x020005C0 RID: 1472
	public class DeviceList : List<Device>
	{
		// Token: 0x0600255E RID: 9566 RVA: 0x000D6915 File Offset: 0x000D4D15
		public DeviceList()
		{
		}

		// Token: 0x0600255F RID: 9567 RVA: 0x000D6920 File Offset: 0x000D4D20
		public Device FindDeviceByHandle(IntPtr deviceHandle)
		{
			for (int i = 0; i < base.Count; i++)
			{
				if (base[i].Handle == deviceHandle)
				{
					return base[i];
				}
			}
			return null;
		}

		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x06002560 RID: 9568 RVA: 0x000D6964 File Offset: 0x000D4D64
		public Device ActiveDevice
		{
			get
			{
				if (base.Count == 1)
				{
					return base[0];
				}
				for (int i = 0; i < base.Count; i++)
				{
					if (base[i].IsStreaming)
					{
						return base[i];
					}
				}
				return null;
			}
		}

		// Token: 0x06002561 RID: 9569 RVA: 0x000D69B8 File Offset: 0x000D4DB8
		public void AddOrUpdate(Device device)
		{
			Device device2 = this.FindDeviceByHandle(device.Handle);
			if (device2 != null)
			{
				device2.Update(device);
			}
			else
			{
				base.Add(device);
			}
		}

		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x06002562 RID: 9570 RVA: 0x000D69EB File Offset: 0x000D4DEB
		public bool IsEmpty
		{
			get
			{
				return base.Count == 0;
			}
		}
	}
}
