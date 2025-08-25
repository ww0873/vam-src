using System;
using System.Runtime.InteropServices;

namespace LeapInternal
{
	// Token: 0x0200061A RID: 1562
	public class LeapC
	{
		// Token: 0x0600262C RID: 9772 RVA: 0x000D787F File Offset: 0x000D5C7F
		private LeapC()
		{
		}

		// Token: 0x0600262D RID: 9773
		[DllImport("LeapC", EntryPoint = "LeapGetNow")]
		public static extern long GetNow();

		// Token: 0x0600262E RID: 9774
		[DllImport("LeapC", EntryPoint = "LeapCreateClockRebaser")]
		public static extern eLeapRS CreateClockRebaser(out IntPtr phClockRebaser);

		// Token: 0x0600262F RID: 9775
		[DllImport("LeapC", EntryPoint = "LeapDestroyClockRebaser")]
		public static extern eLeapRS DestroyClockRebaser(IntPtr hClockRebaser);

		// Token: 0x06002630 RID: 9776
		[DllImport("LeapC", EntryPoint = "LeapUpdateRebase")]
		public static extern eLeapRS UpdateRebase(IntPtr hClockRebaser, long userClock, long leapClock);

		// Token: 0x06002631 RID: 9777
		[DllImport("LeapC", EntryPoint = "LeapRebaseClock")]
		public static extern eLeapRS RebaseClock(IntPtr hClockRebaser, long userClock, out long leapClock);

		// Token: 0x06002632 RID: 9778
		[DllImport("LeapC", EntryPoint = "LeapCreateConnection")]
		public static extern eLeapRS CreateConnection(ref LEAP_CONNECTION_CONFIG pConfig, out IntPtr pConnection);

		// Token: 0x06002633 RID: 9779
		[DllImport("LeapC", EntryPoint = "LeapCreateConnection")]
		private static extern eLeapRS CreateConnection(IntPtr nulled, out IntPtr pConnection);

		// Token: 0x06002634 RID: 9780 RVA: 0x000D7887 File Offset: 0x000D5C87
		public static eLeapRS CreateConnection(out IntPtr pConnection)
		{
			return LeapC.CreateConnection(IntPtr.Zero, out pConnection);
		}

		// Token: 0x06002635 RID: 9781
		[DllImport("LeapC", EntryPoint = "LeapGetConnectionInfo")]
		public static extern eLeapRS GetConnectionInfo(IntPtr hConnection, ref LEAP_CONNECTION_INFO pInfo);

		// Token: 0x06002636 RID: 9782
		[DllImport("LeapC", EntryPoint = "LeapOpenConnection")]
		public static extern eLeapRS OpenConnection(IntPtr hConnection);

		// Token: 0x06002637 RID: 9783
		[DllImport("LeapC", EntryPoint = "LeapSetAllocator")]
		public static extern eLeapRS SetAllocator(IntPtr hConnection, ref LEAP_ALLOCATOR pAllocator);

		// Token: 0x06002638 RID: 9784
		[DllImport("LeapC", EntryPoint = "LeapGetDeviceList")]
		public static extern eLeapRS GetDeviceList(IntPtr hConnection, [In] [Out] LEAP_DEVICE_REF[] pArray, out uint pnArray);

		// Token: 0x06002639 RID: 9785
		[DllImport("LeapC", EntryPoint = "LeapGetDeviceList")]
		private static extern eLeapRS GetDeviceList(IntPtr hConnection, [In] [Out] IntPtr pArray, out uint pnArray);

		// Token: 0x0600263A RID: 9786 RVA: 0x000D7894 File Offset: 0x000D5C94
		public static eLeapRS GetDeviceCount(IntPtr hConnection, out uint deviceCount)
		{
			return LeapC.GetDeviceList(hConnection, IntPtr.Zero, out deviceCount);
		}

		// Token: 0x0600263B RID: 9787
		[DllImport("LeapC", EntryPoint = "LeapOpenDevice")]
		public static extern eLeapRS OpenDevice(LEAP_DEVICE_REF rDevice, out IntPtr pDevice);

		// Token: 0x0600263C RID: 9788
		[DllImport("LeapC", CharSet = CharSet.Ansi, EntryPoint = "LeapGetDeviceInfo")]
		public static extern eLeapRS GetDeviceInfo(IntPtr hDevice, ref LEAP_DEVICE_INFO info);

		// Token: 0x0600263D RID: 9789
		[DllImport("LeapC", EntryPoint = "LeapSetPolicyFlags")]
		public static extern eLeapRS SetPolicyFlags(IntPtr hConnection, ulong set, ulong clear);

		// Token: 0x0600263E RID: 9790
		[DllImport("LeapC", EntryPoint = "LeapSetDeviceFlags")]
		public static extern eLeapRS SetDeviceFlags(IntPtr hDevice, ulong set, ulong clear, out ulong prior);

		// Token: 0x0600263F RID: 9791
		[DllImport("LeapC", EntryPoint = "LeapPollConnection")]
		public static extern eLeapRS PollConnection(IntPtr hConnection, uint timeout, ref LEAP_CONNECTION_MESSAGE msg);

		// Token: 0x06002640 RID: 9792
		[DllImport("LeapC", EntryPoint = "LeapGetFrameSize")]
		public static extern eLeapRS GetFrameSize(IntPtr hConnection, long timestamp, out ulong pncbEvent);

		// Token: 0x06002641 RID: 9793
		[DllImport("LeapC", EntryPoint = "LeapInterpolateFrame")]
		public static extern eLeapRS InterpolateFrame(IntPtr hConnection, long timestamp, IntPtr pEvent, ulong ncbEvent);

		// Token: 0x06002642 RID: 9794
		[DllImport("LeapC", EntryPoint = "LeapInterpolateFrameFromTime")]
		public static extern eLeapRS InterpolateFrameFromTime(IntPtr hConnection, long timestamp, long sourceTimestamp, IntPtr pEvent, ulong ncbEvent);

		// Token: 0x06002643 RID: 9795
		[DllImport("LeapC", EntryPoint = "LeapInterpolateHeadPose")]
		public static extern eLeapRS InterpolateHeadPose(IntPtr hConnection, long timestamp, ref LEAP_HEAD_POSE_EVENT headPose);

		// Token: 0x06002644 RID: 9796
		[DllImport("LeapC")]
		public static extern LEAP_VECTOR LeapPixelToRectilinear(IntPtr hConnection, eLeapPerspectiveType camera, LEAP_VECTOR pixel);

		// Token: 0x06002645 RID: 9797
		[DllImport("LeapC")]
		public static extern LEAP_VECTOR LeapRectilinearToPixel(IntPtr hConnection, eLeapPerspectiveType camera, LEAP_VECTOR rectilinear);

		// Token: 0x06002646 RID: 9798
		[DllImport("LeapC", EntryPoint = "LeapCloseDevice")]
		public static extern void CloseDevice(IntPtr pDevice);

		// Token: 0x06002647 RID: 9799
		[DllImport("LeapC", EntryPoint = "LeapCloseConnection")]
		public static extern eLeapRS CloseConnection(IntPtr hConnection);

		// Token: 0x06002648 RID: 9800
		[DllImport("LeapC", EntryPoint = "LeapDestroyConnection")]
		public static extern void DestroyConnection(IntPtr connection);

		// Token: 0x06002649 RID: 9801
		[DllImport("LeapC", EntryPoint = "LeapSaveConfigValue")]
		private static extern eLeapRS SaveConfigValue(IntPtr hConnection, string key, IntPtr value, out uint requestId);

		// Token: 0x0600264A RID: 9802
		[DllImport("LeapC", EntryPoint = "LeapRequestConfigValue")]
		public static extern eLeapRS RequestConfigValue(IntPtr hConnection, string name, out uint request_id);

		// Token: 0x0600264B RID: 9803 RVA: 0x000D78A4 File Offset: 0x000D5CA4
		public static eLeapRS SaveConfigValue(IntPtr hConnection, string key, bool value, out uint requestId)
		{
			return LeapC.SaveConfigWithValueType(hConnection, key, new LEAP_VARIANT_VALUE_TYPE
			{
				type = eLeapValueType.eLeapValueType_Boolean,
				boolValue = ((!value) ? 0 : 1)
			}, out requestId);
		}

		// Token: 0x0600264C RID: 9804 RVA: 0x000D78E0 File Offset: 0x000D5CE0
		public static eLeapRS SaveConfigValue(IntPtr hConnection, string key, int value, out uint requestId)
		{
			return LeapC.SaveConfigWithValueType(hConnection, key, new LEAP_VARIANT_VALUE_TYPE
			{
				type = eLeapValueType.eLeapValueType_Int32,
				intValue = value
			}, out requestId);
		}

		// Token: 0x0600264D RID: 9805 RVA: 0x000D7910 File Offset: 0x000D5D10
		public static eLeapRS SaveConfigValue(IntPtr hConnection, string key, float value, out uint requestId)
		{
			return LeapC.SaveConfigWithValueType(hConnection, key, new LEAP_VARIANT_VALUE_TYPE
			{
				type = eLeapValueType.eLeapValueType_Float,
				floatValue = value
			}, out requestId);
		}

		// Token: 0x0600264E RID: 9806 RVA: 0x000D7940 File Offset: 0x000D5D40
		public static eLeapRS SaveConfigValue(IntPtr hConnection, string key, string value, out uint requestId)
		{
			LEAP_VARIANT_REF_TYPE valueStruct;
			valueStruct.type = eLeapValueType.eLeapValueType_String;
			valueStruct.stringValue = value;
			return LeapC.SaveConfigWithRefType(hConnection, key, valueStruct, out requestId);
		}

		// Token: 0x0600264F RID: 9807 RVA: 0x000D7968 File Offset: 0x000D5D68
		private static eLeapRS SaveConfigWithValueType(IntPtr hConnection, string key, LEAP_VARIANT_VALUE_TYPE valueStruct, out uint requestId)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(valueStruct));
			eLeapRS result = (eLeapRS)3791716352U;
			try
			{
				Marshal.StructureToPtr(valueStruct, intPtr, false);
				result = LeapC.SaveConfigValue(hConnection, key, intPtr, out requestId);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return result;
		}

		// Token: 0x06002650 RID: 9808 RVA: 0x000D79C0 File Offset: 0x000D5DC0
		private static eLeapRS SaveConfigWithRefType(IntPtr hConnection, string key, LEAP_VARIANT_REF_TYPE valueStruct, out uint requestId)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(valueStruct));
			eLeapRS result = (eLeapRS)3791716352U;
			try
			{
				Marshal.StructureToPtr(valueStruct, intPtr, false);
				result = LeapC.SaveConfigValue(hConnection, key, intPtr, out requestId);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return result;
		}

		// Token: 0x06002651 RID: 9809
		[DllImport("LeapC", EntryPoint = "LeapGetPointMappingSize")]
		public static extern eLeapRS GetPointMappingSize(IntPtr hConnection, ref ulong pSize);

		// Token: 0x06002652 RID: 9810
		[DllImport("LeapC", EntryPoint = "LeapGetPointMapping")]
		public static extern eLeapRS GetPointMapping(IntPtr hConnection, IntPtr pointMapping, ref ulong pSize);

		// Token: 0x06002653 RID: 9811
		[DllImport("LeapC", EntryPoint = "LeapRecordingOpen")]
		public static extern eLeapRS RecordingOpen(ref IntPtr ppRecording, string userPath, LeapC.LEAP_RECORDING_PARAMETERS parameters);

		// Token: 0x06002654 RID: 9812
		[DllImport("LeapC", EntryPoint = "LeapRecordingClose")]
		public static extern eLeapRS RecordingClose(ref IntPtr ppRecording);

		// Token: 0x06002655 RID: 9813
		[DllImport("LeapC")]
		public static extern eLeapRS LeapRecordingGetStatus(IntPtr pRecording, ref LeapC.LEAP_RECORDING_STATUS status);

		// Token: 0x06002656 RID: 9814
		[DllImport("LeapC", EntryPoint = "LeapRecordingReadSize")]
		public static extern eLeapRS RecordingReadSize(IntPtr pRecording, ref ulong pncbEvent);

		// Token: 0x06002657 RID: 9815
		[DllImport("LeapC", EntryPoint = "LeapRecordingRead")]
		public static extern eLeapRS RecordingRead(IntPtr pRecording, ref LEAP_TRACKING_EVENT pEvent, ulong ncbEvent);

		// Token: 0x06002658 RID: 9816
		[DllImport("LeapC", EntryPoint = "LeapRecordingWrite")]
		public static extern eLeapRS RecordingWrite(IntPtr pRecording, ref LEAP_TRACKING_EVENT pEvent, ref ulong pnBytesWritten);

		// Token: 0x06002659 RID: 9817
		[DllImport("LeapC")]
		public static extern eLeapRS LeapTelemetryProfiling(IntPtr hConnection, ref LEAP_TELEMETRY_DATA telemetryData);

		// Token: 0x0600265A RID: 9818
		[DllImport("LeapC", EntryPoint = "LeapTelemetryGetNow")]
		public static extern ulong TelemetryGetNow();

		// Token: 0x0600265B RID: 9819 RVA: 0x000D7A18 File Offset: 0x000D5E18
		// Note: this type is marked as 'beforefieldinit'.
		static LeapC()
		{
		}

		// Token: 0x040020AA RID: 8362
		public static int DistortionSize = 64;

		// Token: 0x0200061B RID: 1563
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct LEAP_RECORDING_PARAMETERS
		{
			// Token: 0x040020AB RID: 8363
			public uint mode;
		}

		// Token: 0x0200061C RID: 1564
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct LEAP_RECORDING_STATUS
		{
			// Token: 0x040020AC RID: 8364
			public uint mode;
		}
	}
}
