using System;

namespace LeapInternal
{
	// Token: 0x020005EA RID: 1514
	public enum eLeapDeviceStatus : uint
	{
		// Token: 0x04001FC0 RID: 8128
		eLeapDeviceStatus_Streaming = 1U,
		// Token: 0x04001FC1 RID: 8129
		eLeapDeviceStatus_Paused,
		// Token: 0x04001FC2 RID: 8130
		eLeapDeviceStatus_Robust = 4U,
		// Token: 0x04001FC3 RID: 8131
		eLeapDeviceStatus_Smudged = 8U,
		// Token: 0x04001FC4 RID: 8132
		eLeapDeviceStatus_LowResource = 16U,
		// Token: 0x04001FC5 RID: 8133
		eLeapDeviceStatus_UnknownFailure = 3892379648U,
		// Token: 0x04001FC6 RID: 8134
		eLeapDeviceStatus_BadCalibration,
		// Token: 0x04001FC7 RID: 8135
		eLeapDeviceStatus_BadFirmware,
		// Token: 0x04001FC8 RID: 8136
		eLeapDeviceStatus_BadTransport,
		// Token: 0x04001FC9 RID: 8137
		eLeapDeviceStatus_BadControl
	}
}
