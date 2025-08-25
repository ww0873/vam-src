using System;

namespace LeapInternal
{
	// Token: 0x020005F3 RID: 1523
	public enum eLeapEventType
	{
		// Token: 0x04002006 RID: 8198
		eLeapEventType_None,
		// Token: 0x04002007 RID: 8199
		eLeapEventType_Connection,
		// Token: 0x04002008 RID: 8200
		eLeapEventType_ConnectionLost,
		// Token: 0x04002009 RID: 8201
		eLeapEventType_Device,
		// Token: 0x0400200A RID: 8202
		eLeapEventType_DeviceFailure,
		// Token: 0x0400200B RID: 8203
		eLeapEventType_Policy,
		// Token: 0x0400200C RID: 8204
		eLeapEventType_Tracking = 256,
		// Token: 0x0400200D RID: 8205
		eLeapEventType_ImageRequestError,
		// Token: 0x0400200E RID: 8206
		eLeapEventType_ImageComplete,
		// Token: 0x0400200F RID: 8207
		eLeapEventType_LogEvent,
		// Token: 0x04002010 RID: 8208
		eLeapEventType_DeviceLost,
		// Token: 0x04002011 RID: 8209
		eLeapEventType_ConfigResponse,
		// Token: 0x04002012 RID: 8210
		eLeapEventType_ConfigChange,
		// Token: 0x04002013 RID: 8211
		eLeapEventType_DeviceStatusChange,
		// Token: 0x04002014 RID: 8212
		eLeapEventType_DroppedFrame,
		// Token: 0x04002015 RID: 8213
		eLeapEventType_Image,
		// Token: 0x04002016 RID: 8214
		eLeapEventType_PointMappingChange,
		// Token: 0x04002017 RID: 8215
		eLeapEventType_LogEvents,
		// Token: 0x04002018 RID: 8216
		eLeapEventType_HeadPose
	}
}
