using System;

namespace LeapInternal
{
	// Token: 0x020005F2 RID: 1522
	public enum eLeapRS : uint
	{
		// Token: 0x04001FF1 RID: 8177
		eLeapRS_Success,
		// Token: 0x04001FF2 RID: 8178
		eLeapRS_UnknownError = 3791716352U,
		// Token: 0x04001FF3 RID: 8179
		eLeapRS_InvalidArgument,
		// Token: 0x04001FF4 RID: 8180
		eLeapRS_InsufficientResources,
		// Token: 0x04001FF5 RID: 8181
		eLeapRS_InsufficientBuffer,
		// Token: 0x04001FF6 RID: 8182
		eLeapRS_Timeout,
		// Token: 0x04001FF7 RID: 8183
		eLeapRS_NotConnected,
		// Token: 0x04001FF8 RID: 8184
		eLeapRS_HandshakeIncomplete,
		// Token: 0x04001FF9 RID: 8185
		eLeapRS_BufferSizeOverflow,
		// Token: 0x04001FFA RID: 8186
		eLeapRS_ProtocolError,
		// Token: 0x04001FFB RID: 8187
		eLeapRS_InvalidClientID,
		// Token: 0x04001FFC RID: 8188
		eLeapRS_UnexpectedClosed,
		// Token: 0x04001FFD RID: 8189
		eLeapRS_UnknownImageFrameRequest,
		// Token: 0x04001FFE RID: 8190
		eLeapRS_UnknownTrackingFrameID,
		// Token: 0x04001FFF RID: 8191
		eLeapRS_RoutineIsNotSeer,
		// Token: 0x04002000 RID: 8192
		eLeapRS_TimestampTooEarly,
		// Token: 0x04002001 RID: 8193
		eLeapRS_ConcurrentPoll,
		// Token: 0x04002002 RID: 8194
		eLeapRS_NotAvailable = 3875602434U,
		// Token: 0x04002003 RID: 8195
		eLeapRS_NotStreaming = 3875602436U,
		// Token: 0x04002004 RID: 8196
		eLeapRS_CannotOpenDevice
	}
}
