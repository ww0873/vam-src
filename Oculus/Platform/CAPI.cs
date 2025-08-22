using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Oculus.Platform
{
	// Token: 0x020007E7 RID: 2023
	public class CAPI
	{
		// Token: 0x0600331B RID: 13083 RVA: 0x0010936D File Offset: 0x0010776D
		public CAPI()
		{
		}

		// Token: 0x0600331C RID: 13084 RVA: 0x00109378 File Offset: 0x00107778
		public static IntPtr ArrayOfStructsToIntPtr(Array ar)
		{
			int num = 0;
			for (int i = 0; i < ar.Length; i++)
			{
				num += Marshal.SizeOf(ar.GetValue(i));
			}
			IntPtr intPtr = Marshal.AllocHGlobal(num);
			IntPtr intPtr2 = intPtr;
			for (int j = 0; j < ar.Length; j++)
			{
				Marshal.StructureToPtr(ar.GetValue(j), intPtr2, false);
				intPtr2 = (IntPtr)((long)intPtr2 + (long)Marshal.SizeOf(ar.GetValue(j)));
			}
			return intPtr;
		}

		// Token: 0x0600331D RID: 13085 RVA: 0x001093FC File Offset: 0x001077FC
		public static CAPI.ovrKeyValuePair[] DictionaryToOVRKeyValuePairs(Dictionary<string, object> dict)
		{
			if (dict == null || dict.Count == 0)
			{
				return null;
			}
			CAPI.ovrKeyValuePair[] array = new CAPI.ovrKeyValuePair[dict.Count];
			int num = 0;
			foreach (KeyValuePair<string, object> keyValuePair in dict)
			{
				if (keyValuePair.Value.GetType() == typeof(int))
				{
					array[num] = new CAPI.ovrKeyValuePair(keyValuePair.Key, (int)keyValuePair.Value);
				}
				else if (keyValuePair.Value.GetType() == typeof(string))
				{
					array[num] = new CAPI.ovrKeyValuePair(keyValuePair.Key, (string)keyValuePair.Value);
				}
				else
				{
					if (keyValuePair.Value.GetType() != typeof(double))
					{
						throw new Exception("Only int, double or string are allowed types in CustomQuery.data");
					}
					array[num] = new CAPI.ovrKeyValuePair(keyValuePair.Key, (double)keyValuePair.Value);
				}
				num++;
			}
			return array;
		}

		// Token: 0x0600331E RID: 13086 RVA: 0x0010954C File Offset: 0x0010794C
		public static byte[] IntPtrToByteArray(IntPtr data, ulong size)
		{
			byte[] array = new byte[size];
			Marshal.Copy(data, array, 0, (int)size);
			return array;
		}

		// Token: 0x0600331F RID: 13087 RVA: 0x0010956C File Offset: 0x0010796C
		public static Dictionary<string, string> DataStoreFromNative(IntPtr pointer)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			int num = (int)((uint)CAPI.ovr_DataStore_GetNumKeys(pointer));
			for (int i = 0; i < num; i++)
			{
				string key = CAPI.ovr_DataStore_GetKey(pointer, i);
				dictionary[key] = CAPI.ovr_DataStore_GetValue(pointer, key);
			}
			return dictionary;
		}

		// Token: 0x06003320 RID: 13088 RVA: 0x001095B4 File Offset: 0x001079B4
		public static string StringFromNative(IntPtr pointer)
		{
			if (pointer == IntPtr.Zero)
			{
				return null;
			}
			int nativeStringLengthNotIncludingNullTerminator = CAPI.GetNativeStringLengthNotIncludingNullTerminator(pointer);
			byte[] array = new byte[nativeStringLengthNotIncludingNullTerminator];
			Marshal.Copy(pointer, array, 0, nativeStringLengthNotIncludingNullTerminator);
			return CAPI.nativeStringEncoding.GetString(array);
		}

		// Token: 0x06003321 RID: 13089 RVA: 0x001095F8 File Offset: 0x001079F8
		public static int GetNativeStringLengthNotIncludingNullTerminator(IntPtr pointer)
		{
			int num = 0;
			while (Marshal.ReadByte(pointer, num) != 0)
			{
				num++;
			}
			return num;
		}

		// Token: 0x06003322 RID: 13090 RVA: 0x00109620 File Offset: 0x00107A20
		public static DateTime DateTimeFromNative(ulong seconds_since_the_one_true_epoch)
		{
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
			return dateTime.AddSeconds(seconds_since_the_one_true_epoch).ToLocalTime();
		}

		// Token: 0x06003323 RID: 13091 RVA: 0x00109654 File Offset: 0x00107A54
		public static byte[] BlobFromNative(uint size, IntPtr pointer)
		{
			byte[] array = new byte[size];
			for (int i = 0; i < (int)size; i++)
			{
				array[i] = Marshal.ReadByte(pointer, i);
			}
			return array;
		}

		// Token: 0x06003324 RID: 13092 RVA: 0x00109688 File Offset: 0x00107A88
		public static byte[] FiledataFromNative(uint size, IntPtr pointer)
		{
			byte[] array = new byte[size];
			Marshal.Copy(pointer, array, 0, (int)size);
			return array;
		}

		// Token: 0x06003325 RID: 13093 RVA: 0x001096A8 File Offset: 0x00107AA8
		public static IntPtr StringToNative(string s)
		{
			if (s == null)
			{
				throw new Exception("StringFromNative: null argument");
			}
			int byteCount = CAPI.nativeStringEncoding.GetByteCount(s);
			byte[] array = new byte[byteCount + 1];
			CAPI.nativeStringEncoding.GetBytes(s, 0, s.Length, array, 0);
			IntPtr intPtr = Marshal.AllocCoTaskMem(byteCount + 1);
			Marshal.Copy(array, 0, intPtr, byteCount + 1);
			return intPtr;
		}

		// Token: 0x06003326 RID: 13094
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool ovr_UnityInitWrapper(string appId);

		// Token: 0x06003327 RID: 13095
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_UnityInitGlobals(IntPtr loggingCB);

		// Token: 0x06003328 RID: 13096
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_UnityInitWrapperAsynchronous(string appId);

		// Token: 0x06003329 RID: 13097
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool ovr_UnityInitWrapperStandalone(string accessToken, IntPtr loggingCB);

		// Token: 0x0600332A RID: 13098
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Platform_InitializeStandaloneOculus(ref CAPI.OculusInitParams init);

		// Token: 0x0600332B RID: 13099
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool ovr_UnityInitWrapperWindows(string appId, IntPtr loggingCB);

		// Token: 0x0600332C RID: 13100
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_UnityInitWrapperWindowsAsynchronous(string appId, IntPtr loggingCB);

		// Token: 0x0600332D RID: 13101
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool ovr_SetDeveloperAccessToken(string accessToken);

		// Token: 0x0600332E RID: 13102 RVA: 0x00109708 File Offset: 0x00107B08
		public static string ovr_GetLoggedInUserLocale()
		{
			return CAPI.StringFromNative(CAPI.ovr_GetLoggedInUserLocale_Native());
		}

		// Token: 0x0600332F RID: 13103
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_GetLoggedInUserLocale")]
		private static extern IntPtr ovr_GetLoggedInUserLocale_Native();

		// Token: 0x06003330 RID: 13104
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_PopMessage();

		// Token: 0x06003331 RID: 13105
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_FreeMessage(IntPtr message);

		// Token: 0x06003332 RID: 13106
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ovr_NetworkingPeer_GetSendPolicy(IntPtr networkingPeer);

		// Token: 0x06003333 RID: 13107
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Voip_CreateEncoder();

		// Token: 0x06003334 RID: 13108
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_Voip_DestroyEncoder(IntPtr encoder);

		// Token: 0x06003335 RID: 13109
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Voip_CreateDecoder();

		// Token: 0x06003336 RID: 13110
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_Voip_DestroyDecoder(IntPtr decoder);

		// Token: 0x06003337 RID: 13111
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_VoipDecoder_Decode(IntPtr obj, byte[] compressedData, ulong compressedSize);

		// Token: 0x06003338 RID: 13112
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Microphone_Create();

		// Token: 0x06003339 RID: 13113
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_Microphone_Destroy(IntPtr obj);

		// Token: 0x0600333A RID: 13114
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_Voip_SetSystemVoipPassthrough(bool passthrough);

		// Token: 0x0600333B RID: 13115
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_Voip_SetSystemVoipMicrophoneMuted(VoipMuteState muted);

		// Token: 0x0600333C RID: 13116
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_UnityResetTestPlatform();

		// Token: 0x0600333D RID: 13117
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_HTTP_GetWithMessageType(string url, int messageType);

		// Token: 0x0600333E RID: 13118
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_CrashApplication();

		// Token: 0x0600333F RID: 13119
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_Voip_SetMicrophoneFilterCallback(CAPI.FilterCallback cb);

		// Token: 0x06003340 RID: 13120
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_Voip_SetMicrophoneFilterCallbackWithFixedSizeBuffer(CAPI.FilterCallback cb, UIntPtr bufferSizeElements);

		// Token: 0x06003341 RID: 13121 RVA: 0x00109724 File Offset: 0x00107B24
		public static void LogNewEvent(string eventName, Dictionary<string, string> values)
		{
			IntPtr intPtr = CAPI.StringToNative(eventName);
			int num = (values != null) ? values.Count : 0;
			IntPtr[] array = new IntPtr[num * 2];
			if (num > 0)
			{
				int num2 = 0;
				foreach (KeyValuePair<string, string> keyValuePair in values)
				{
					array[num2 * 2] = CAPI.StringToNative(keyValuePair.Key);
					array[num2 * 2 + 1] = CAPI.StringToNative(keyValuePair.Value);
					num2++;
				}
			}
			CAPI.ovr_Log_NewEvent(intPtr, array, (UIntPtr)((ulong)((long)num)));
			Marshal.FreeCoTaskMem(intPtr);
			foreach (IntPtr ptr in array)
			{
				Marshal.FreeCoTaskMem(ptr);
			}
		}

		// Token: 0x06003342 RID: 13122
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_Log_NewEvent(IntPtr eventName, IntPtr[] values, UIntPtr length);

		// Token: 0x06003343 RID: 13123
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_ApplicationLifecycle_GetLaunchDetails();

		// Token: 0x06003344 RID: 13124 RVA: 0x00109814 File Offset: 0x00107C14
		public static ulong ovr_HTTP_StartTransfer(string url, CAPI.ovrKeyValuePair[] headers)
		{
			IntPtr intPtr = CAPI.StringToNative(url);
			UIntPtr numItems = (UIntPtr)((ulong)((long)headers.Length));
			ulong result = CAPI.ovr_HTTP_StartTransfer_Native(intPtr, headers, numItems);
			Marshal.FreeCoTaskMem(intPtr);
			return result;
		}

		// Token: 0x06003345 RID: 13125
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_HTTP_StartTransfer")]
		private static extern ulong ovr_HTTP_StartTransfer_Native(IntPtr url, CAPI.ovrKeyValuePair[] headers, UIntPtr numItems);

		// Token: 0x06003346 RID: 13126
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool ovr_HTTP_Write(ulong transferId, byte[] bytes, UIntPtr length);

		// Token: 0x06003347 RID: 13127
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_HTTP_WriteEOM(ulong transferId);

		// Token: 0x06003348 RID: 13128 RVA: 0x00109844 File Offset: 0x00107C44
		public static string ovr_Message_GetStringForJavascript(IntPtr message)
		{
			return CAPI.StringFromNative(CAPI.ovr_Message_GetStringForJavascript_Native(message));
		}

		// Token: 0x06003349 RID: 13129
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_Message_GetStringForJavascript")]
		private static extern IntPtr ovr_Message_GetStringForJavascript_Native(IntPtr message);

		// Token: 0x0600334A RID: 13130
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_Net_Accept(ulong peerID);

		// Token: 0x0600334B RID: 13131
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool ovr_Net_AcceptForCurrentRoom();

		// Token: 0x0600334C RID: 13132
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_Net_Close(ulong peerID);

		// Token: 0x0600334D RID: 13133
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_Net_CloseForCurrentRoom();

		// Token: 0x0600334E RID: 13134
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_Net_Connect(ulong peerID);

		// Token: 0x0600334F RID: 13135
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool ovr_Net_IsConnected(ulong peerID);

		// Token: 0x06003350 RID: 13136
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Net_Ping(ulong peerID);

		// Token: 0x06003351 RID: 13137
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Net_ReadPacket();

		// Token: 0x06003352 RID: 13138
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool ovr_Net_SendPacket(ulong userID, UIntPtr length, byte[] bytes, SendPolicy policy);

		// Token: 0x06003353 RID: 13139
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool ovr_Net_SendPacketToCurrentRoom(UIntPtr length, byte[] bytes, SendPolicy policy);

		// Token: 0x06003354 RID: 13140
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_Voip_Accept(ulong userID);

		// Token: 0x06003355 RID: 13141
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr ovr_Voip_GetOutputBufferMaxSize();

		// Token: 0x06003356 RID: 13142
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr ovr_Voip_GetPCM(ulong senderID, short[] outputBuffer, UIntPtr outputBufferNumElements);

		// Token: 0x06003357 RID: 13143
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr ovr_Voip_GetPCMFloat(ulong senderID, float[] outputBuffer, UIntPtr outputBufferNumElements);

		// Token: 0x06003358 RID: 13144
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr ovr_Voip_GetPCMSize(ulong senderID);

		// Token: 0x06003359 RID: 13145
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr ovr_Voip_GetPCMWithTimestamp(ulong senderID, short[] outputBuffer, UIntPtr outputBufferNumElements, uint[] timestamp);

		// Token: 0x0600335A RID: 13146
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr ovr_Voip_GetPCMWithTimestampFloat(ulong senderID, float[] outputBuffer, UIntPtr outputBufferNumElements, uint[] timestamp);

		// Token: 0x0600335B RID: 13147
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ovr_Voip_GetSyncTimestamp(ulong userID);

		// Token: 0x0600335C RID: 13148
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern long ovr_Voip_GetSyncTimestampDifference(uint lhs, uint rhs);

		// Token: 0x0600335D RID: 13149
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern VoipMuteState ovr_Voip_GetSystemVoipMicrophoneMuted();

		// Token: 0x0600335E RID: 13150
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern SystemVoipStatus ovr_Voip_GetSystemVoipStatus();

		// Token: 0x0600335F RID: 13151
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_Voip_SetMicrophoneMuted(VoipMuteState state);

		// Token: 0x06003360 RID: 13152
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_Voip_SetOutputSampleRate(VoipSampleRate rate);

		// Token: 0x06003361 RID: 13153
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_Voip_Start(ulong userID);

		// Token: 0x06003362 RID: 13154
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_Voip_Stop(ulong userID);

		// Token: 0x06003363 RID: 13155 RVA: 0x00109860 File Offset: 0x00107C60
		public static ulong ovr_Achievements_AddCount(string name, ulong count)
		{
			IntPtr intPtr = CAPI.StringToNative(name);
			ulong result = CAPI.ovr_Achievements_AddCount_Native(intPtr, count);
			Marshal.FreeCoTaskMem(intPtr);
			return result;
		}

		// Token: 0x06003364 RID: 13156
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_Achievements_AddCount")]
		private static extern ulong ovr_Achievements_AddCount_Native(IntPtr name, ulong count);

		// Token: 0x06003365 RID: 13157 RVA: 0x00109884 File Offset: 0x00107C84
		public static ulong ovr_Achievements_AddFields(string name, string fields)
		{
			IntPtr intPtr = CAPI.StringToNative(name);
			IntPtr intPtr2 = CAPI.StringToNative(fields);
			ulong result = CAPI.ovr_Achievements_AddFields_Native(intPtr, intPtr2);
			Marshal.FreeCoTaskMem(intPtr);
			Marshal.FreeCoTaskMem(intPtr2);
			return result;
		}

		// Token: 0x06003366 RID: 13158
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_Achievements_AddFields")]
		private static extern ulong ovr_Achievements_AddFields_Native(IntPtr name, IntPtr fields);

		// Token: 0x06003367 RID: 13159
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Achievements_GetAllDefinitions();

		// Token: 0x06003368 RID: 13160
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Achievements_GetAllProgress();

		// Token: 0x06003369 RID: 13161
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Achievements_GetDefinitionsByName(string[] names, int count);

		// Token: 0x0600336A RID: 13162
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Achievements_GetProgressByName(string[] names, int count);

		// Token: 0x0600336B RID: 13163 RVA: 0x001098B4 File Offset: 0x00107CB4
		public static ulong ovr_Achievements_Unlock(string name)
		{
			IntPtr intPtr = CAPI.StringToNative(name);
			ulong result = CAPI.ovr_Achievements_Unlock_Native(intPtr);
			Marshal.FreeCoTaskMem(intPtr);
			return result;
		}

		// Token: 0x0600336C RID: 13164
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_Achievements_Unlock")]
		private static extern ulong ovr_Achievements_Unlock_Native(IntPtr name);

		// Token: 0x0600336D RID: 13165
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Application_ExecuteCoordinatedLaunch(ulong appID, ulong roomID);

		// Token: 0x0600336E RID: 13166
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Application_GetInstalledApplications();

		// Token: 0x0600336F RID: 13167
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Application_GetVersion();

		// Token: 0x06003370 RID: 13168
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Application_LaunchOtherApp(ulong appID, IntPtr deeplink_options);

		// Token: 0x06003371 RID: 13169
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_ApplicationLifecycle_GetRegisteredPIDs();

		// Token: 0x06003372 RID: 13170
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_ApplicationLifecycle_GetSessionKey();

		// Token: 0x06003373 RID: 13171 RVA: 0x001098D8 File Offset: 0x00107CD8
		public static ulong ovr_ApplicationLifecycle_RegisterSessionKey(string sessionKey)
		{
			IntPtr intPtr = CAPI.StringToNative(sessionKey);
			ulong result = CAPI.ovr_ApplicationLifecycle_RegisterSessionKey_Native(intPtr);
			Marshal.FreeCoTaskMem(intPtr);
			return result;
		}

		// Token: 0x06003374 RID: 13172
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_ApplicationLifecycle_RegisterSessionKey")]
		private static extern ulong ovr_ApplicationLifecycle_RegisterSessionKey_Native(IntPtr sessionKey);

		// Token: 0x06003375 RID: 13173
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_AssetFile_Delete(ulong assetFileID);

		// Token: 0x06003376 RID: 13174
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_AssetFile_Download(ulong assetFileID);

		// Token: 0x06003377 RID: 13175
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_AssetFile_DownloadCancel(ulong assetFileID);

		// Token: 0x06003378 RID: 13176 RVA: 0x001098FC File Offset: 0x00107CFC
		public static ulong ovr_Avatar_UpdateMetaData(string avatarMetaData, string imageFilePath)
		{
			IntPtr intPtr = CAPI.StringToNative(avatarMetaData);
			IntPtr intPtr2 = CAPI.StringToNative(imageFilePath);
			ulong result = CAPI.ovr_Avatar_UpdateMetaData_Native(intPtr, intPtr2);
			Marshal.FreeCoTaskMem(intPtr);
			Marshal.FreeCoTaskMem(intPtr2);
			return result;
		}

		// Token: 0x06003379 RID: 13177
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_Avatar_UpdateMetaData")]
		private static extern ulong ovr_Avatar_UpdateMetaData_Native(IntPtr avatarMetaData, IntPtr imageFilePath);

		// Token: 0x0600337A RID: 13178 RVA: 0x0010992C File Offset: 0x00107D2C
		public static ulong ovr_CloudStorage_Delete(string bucket, string key)
		{
			IntPtr intPtr = CAPI.StringToNative(bucket);
			IntPtr intPtr2 = CAPI.StringToNative(key);
			ulong result = CAPI.ovr_CloudStorage_Delete_Native(intPtr, intPtr2);
			Marshal.FreeCoTaskMem(intPtr);
			Marshal.FreeCoTaskMem(intPtr2);
			return result;
		}

		// Token: 0x0600337B RID: 13179
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_CloudStorage_Delete")]
		private static extern ulong ovr_CloudStorage_Delete_Native(IntPtr bucket, IntPtr key);

		// Token: 0x0600337C RID: 13180 RVA: 0x0010995C File Offset: 0x00107D5C
		public static ulong ovr_CloudStorage_Load(string bucket, string key)
		{
			IntPtr intPtr = CAPI.StringToNative(bucket);
			IntPtr intPtr2 = CAPI.StringToNative(key);
			ulong result = CAPI.ovr_CloudStorage_Load_Native(intPtr, intPtr2);
			Marshal.FreeCoTaskMem(intPtr);
			Marshal.FreeCoTaskMem(intPtr2);
			return result;
		}

		// Token: 0x0600337D RID: 13181
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_CloudStorage_Load")]
		private static extern ulong ovr_CloudStorage_Load_Native(IntPtr bucket, IntPtr key);

		// Token: 0x0600337E RID: 13182 RVA: 0x0010998C File Offset: 0x00107D8C
		public static ulong ovr_CloudStorage_LoadBucketMetadata(string bucket)
		{
			IntPtr intPtr = CAPI.StringToNative(bucket);
			ulong result = CAPI.ovr_CloudStorage_LoadBucketMetadata_Native(intPtr);
			Marshal.FreeCoTaskMem(intPtr);
			return result;
		}

		// Token: 0x0600337F RID: 13183
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_CloudStorage_LoadBucketMetadata")]
		private static extern ulong ovr_CloudStorage_LoadBucketMetadata_Native(IntPtr bucket);

		// Token: 0x06003380 RID: 13184 RVA: 0x001099B0 File Offset: 0x00107DB0
		public static ulong ovr_CloudStorage_LoadConflictMetadata(string bucket, string key)
		{
			IntPtr intPtr = CAPI.StringToNative(bucket);
			IntPtr intPtr2 = CAPI.StringToNative(key);
			ulong result = CAPI.ovr_CloudStorage_LoadConflictMetadata_Native(intPtr, intPtr2);
			Marshal.FreeCoTaskMem(intPtr);
			Marshal.FreeCoTaskMem(intPtr2);
			return result;
		}

		// Token: 0x06003381 RID: 13185
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_CloudStorage_LoadConflictMetadata")]
		private static extern ulong ovr_CloudStorage_LoadConflictMetadata_Native(IntPtr bucket, IntPtr key);

		// Token: 0x06003382 RID: 13186 RVA: 0x001099E0 File Offset: 0x00107DE0
		public static ulong ovr_CloudStorage_LoadHandle(string handle)
		{
			IntPtr intPtr = CAPI.StringToNative(handle);
			ulong result = CAPI.ovr_CloudStorage_LoadHandle_Native(intPtr);
			Marshal.FreeCoTaskMem(intPtr);
			return result;
		}

		// Token: 0x06003383 RID: 13187
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_CloudStorage_LoadHandle")]
		private static extern ulong ovr_CloudStorage_LoadHandle_Native(IntPtr handle);

		// Token: 0x06003384 RID: 13188 RVA: 0x00109A04 File Offset: 0x00107E04
		public static ulong ovr_CloudStorage_LoadMetadata(string bucket, string key)
		{
			IntPtr intPtr = CAPI.StringToNative(bucket);
			IntPtr intPtr2 = CAPI.StringToNative(key);
			ulong result = CAPI.ovr_CloudStorage_LoadMetadata_Native(intPtr, intPtr2);
			Marshal.FreeCoTaskMem(intPtr);
			Marshal.FreeCoTaskMem(intPtr2);
			return result;
		}

		// Token: 0x06003385 RID: 13189
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_CloudStorage_LoadMetadata")]
		private static extern ulong ovr_CloudStorage_LoadMetadata_Native(IntPtr bucket, IntPtr key);

		// Token: 0x06003386 RID: 13190 RVA: 0x00109A34 File Offset: 0x00107E34
		public static ulong ovr_CloudStorage_ResolveKeepLocal(string bucket, string key, string remoteHandle)
		{
			IntPtr intPtr = CAPI.StringToNative(bucket);
			IntPtr intPtr2 = CAPI.StringToNative(key);
			IntPtr intPtr3 = CAPI.StringToNative(remoteHandle);
			ulong result = CAPI.ovr_CloudStorage_ResolveKeepLocal_Native(intPtr, intPtr2, intPtr3);
			Marshal.FreeCoTaskMem(intPtr);
			Marshal.FreeCoTaskMem(intPtr2);
			Marshal.FreeCoTaskMem(intPtr3);
			return result;
		}

		// Token: 0x06003387 RID: 13191
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_CloudStorage_ResolveKeepLocal")]
		private static extern ulong ovr_CloudStorage_ResolveKeepLocal_Native(IntPtr bucket, IntPtr key, IntPtr remoteHandle);

		// Token: 0x06003388 RID: 13192 RVA: 0x00109A74 File Offset: 0x00107E74
		public static ulong ovr_CloudStorage_ResolveKeepRemote(string bucket, string key, string remoteHandle)
		{
			IntPtr intPtr = CAPI.StringToNative(bucket);
			IntPtr intPtr2 = CAPI.StringToNative(key);
			IntPtr intPtr3 = CAPI.StringToNative(remoteHandle);
			ulong result = CAPI.ovr_CloudStorage_ResolveKeepRemote_Native(intPtr, intPtr2, intPtr3);
			Marshal.FreeCoTaskMem(intPtr);
			Marshal.FreeCoTaskMem(intPtr2);
			Marshal.FreeCoTaskMem(intPtr3);
			return result;
		}

		// Token: 0x06003389 RID: 13193
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_CloudStorage_ResolveKeepRemote")]
		private static extern ulong ovr_CloudStorage_ResolveKeepRemote_Native(IntPtr bucket, IntPtr key, IntPtr remoteHandle);

		// Token: 0x0600338A RID: 13194 RVA: 0x00109AB4 File Offset: 0x00107EB4
		public static ulong ovr_CloudStorage_Save(string bucket, string key, byte[] data, uint dataSize, long counter, string extraData)
		{
			IntPtr intPtr = CAPI.StringToNative(bucket);
			IntPtr intPtr2 = CAPI.StringToNative(key);
			IntPtr intPtr3 = CAPI.StringToNative(extraData);
			ulong result = CAPI.ovr_CloudStorage_Save_Native(intPtr, intPtr2, data, dataSize, counter, intPtr3);
			Marshal.FreeCoTaskMem(intPtr);
			Marshal.FreeCoTaskMem(intPtr2);
			Marshal.FreeCoTaskMem(intPtr3);
			return result;
		}

		// Token: 0x0600338B RID: 13195
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_CloudStorage_Save")]
		private static extern ulong ovr_CloudStorage_Save_Native(IntPtr bucket, IntPtr key, byte[] data, uint dataSize, long counter, IntPtr extraData);

		// Token: 0x0600338C RID: 13196
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Entitlement_GetIsViewerEntitled();

		// Token: 0x0600338D RID: 13197 RVA: 0x00109AF8 File Offset: 0x00107EF8
		public static ulong ovr_GraphAPI_Get(string url)
		{
			IntPtr intPtr = CAPI.StringToNative(url);
			ulong result = CAPI.ovr_GraphAPI_Get_Native(intPtr);
			Marshal.FreeCoTaskMem(intPtr);
			return result;
		}

		// Token: 0x0600338E RID: 13198
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_GraphAPI_Get")]
		private static extern ulong ovr_GraphAPI_Get_Native(IntPtr url);

		// Token: 0x0600338F RID: 13199 RVA: 0x00109B1C File Offset: 0x00107F1C
		public static ulong ovr_GraphAPI_Post(string url)
		{
			IntPtr intPtr = CAPI.StringToNative(url);
			ulong result = CAPI.ovr_GraphAPI_Post_Native(intPtr);
			Marshal.FreeCoTaskMem(intPtr);
			return result;
		}

		// Token: 0x06003390 RID: 13200
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_GraphAPI_Post")]
		private static extern ulong ovr_GraphAPI_Post_Native(IntPtr url);

		// Token: 0x06003391 RID: 13201 RVA: 0x00109B40 File Offset: 0x00107F40
		public static ulong ovr_HTTP_Get(string url)
		{
			IntPtr intPtr = CAPI.StringToNative(url);
			ulong result = CAPI.ovr_HTTP_Get_Native(intPtr);
			Marshal.FreeCoTaskMem(intPtr);
			return result;
		}

		// Token: 0x06003392 RID: 13202
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_HTTP_Get")]
		private static extern ulong ovr_HTTP_Get_Native(IntPtr url);

		// Token: 0x06003393 RID: 13203 RVA: 0x00109B64 File Offset: 0x00107F64
		public static ulong ovr_HTTP_GetToFile(string url, string diskFile)
		{
			IntPtr intPtr = CAPI.StringToNative(url);
			IntPtr intPtr2 = CAPI.StringToNative(diskFile);
			ulong result = CAPI.ovr_HTTP_GetToFile_Native(intPtr, intPtr2);
			Marshal.FreeCoTaskMem(intPtr);
			Marshal.FreeCoTaskMem(intPtr2);
			return result;
		}

		// Token: 0x06003394 RID: 13204
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_HTTP_GetToFile")]
		private static extern ulong ovr_HTTP_GetToFile_Native(IntPtr url, IntPtr diskFile);

		// Token: 0x06003395 RID: 13205 RVA: 0x00109B94 File Offset: 0x00107F94
		public static ulong ovr_HTTP_MultiPartPost(string url, string filepath_param_name, string filepath, string access_token, CAPI.ovrKeyValuePair[] post_params)
		{
			IntPtr intPtr = CAPI.StringToNative(url);
			IntPtr intPtr2 = CAPI.StringToNative(filepath_param_name);
			IntPtr intPtr3 = CAPI.StringToNative(filepath);
			IntPtr intPtr4 = CAPI.StringToNative(access_token);
			UIntPtr numItems = (UIntPtr)((ulong)((long)post_params.Length));
			ulong result = CAPI.ovr_HTTP_MultiPartPost_Native(intPtr, intPtr2, intPtr3, intPtr4, post_params, numItems);
			Marshal.FreeCoTaskMem(intPtr);
			Marshal.FreeCoTaskMem(intPtr2);
			Marshal.FreeCoTaskMem(intPtr3);
			Marshal.FreeCoTaskMem(intPtr4);
			return result;
		}

		// Token: 0x06003396 RID: 13206
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_HTTP_MultiPartPost")]
		private static extern ulong ovr_HTTP_MultiPartPost_Native(IntPtr url, IntPtr filepath_param_name, IntPtr filepath, IntPtr access_token, CAPI.ovrKeyValuePair[] post_params, UIntPtr numItems);

		// Token: 0x06003397 RID: 13207 RVA: 0x00109BF4 File Offset: 0x00107FF4
		public static ulong ovr_HTTP_Post(string url)
		{
			IntPtr intPtr = CAPI.StringToNative(url);
			ulong result = CAPI.ovr_HTTP_Post_Native(intPtr);
			Marshal.FreeCoTaskMem(intPtr);
			return result;
		}

		// Token: 0x06003398 RID: 13208
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_HTTP_Post")]
		private static extern ulong ovr_HTTP_Post_Native(IntPtr url);

		// Token: 0x06003399 RID: 13209 RVA: 0x00109C18 File Offset: 0x00108018
		public static ulong ovr_IAP_ConsumePurchase(string sku)
		{
			IntPtr intPtr = CAPI.StringToNative(sku);
			ulong result = CAPI.ovr_IAP_ConsumePurchase_Native(intPtr);
			Marshal.FreeCoTaskMem(intPtr);
			return result;
		}

		// Token: 0x0600339A RID: 13210
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_IAP_ConsumePurchase")]
		private static extern ulong ovr_IAP_ConsumePurchase_Native(IntPtr sku);

		// Token: 0x0600339B RID: 13211
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_IAP_GetProductsBySKU(string[] skus, int count);

		// Token: 0x0600339C RID: 13212
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_IAP_GetViewerPurchases();

		// Token: 0x0600339D RID: 13213 RVA: 0x00109C3C File Offset: 0x0010803C
		public static ulong ovr_IAP_LaunchCheckoutFlow(string sku)
		{
			IntPtr intPtr = CAPI.StringToNative(sku);
			ulong result = CAPI.ovr_IAP_LaunchCheckoutFlow_Native(intPtr);
			Marshal.FreeCoTaskMem(intPtr);
			return result;
		}

		// Token: 0x0600339E RID: 13214
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_IAP_LaunchCheckoutFlow")]
		private static extern ulong ovr_IAP_LaunchCheckoutFlow_Native(IntPtr sku);

		// Token: 0x0600339F RID: 13215 RVA: 0x00109C60 File Offset: 0x00108060
		public static ulong ovr_Leaderboard_GetEntries(string leaderboardName, int limit, LeaderboardFilterType filter, LeaderboardStartAt startAt)
		{
			IntPtr intPtr = CAPI.StringToNative(leaderboardName);
			ulong result = CAPI.ovr_Leaderboard_GetEntries_Native(intPtr, limit, filter, startAt);
			Marshal.FreeCoTaskMem(intPtr);
			return result;
		}

		// Token: 0x060033A0 RID: 13216
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_Leaderboard_GetEntries")]
		private static extern ulong ovr_Leaderboard_GetEntries_Native(IntPtr leaderboardName, int limit, LeaderboardFilterType filter, LeaderboardStartAt startAt);

		// Token: 0x060033A1 RID: 13217 RVA: 0x00109C88 File Offset: 0x00108088
		public static ulong ovr_Leaderboard_GetEntriesAfterRank(string leaderboardName, int limit, ulong afterRank)
		{
			IntPtr intPtr = CAPI.StringToNative(leaderboardName);
			ulong result = CAPI.ovr_Leaderboard_GetEntriesAfterRank_Native(intPtr, limit, afterRank);
			Marshal.FreeCoTaskMem(intPtr);
			return result;
		}

		// Token: 0x060033A2 RID: 13218
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_Leaderboard_GetEntriesAfterRank")]
		private static extern ulong ovr_Leaderboard_GetEntriesAfterRank_Native(IntPtr leaderboardName, int limit, ulong afterRank);

		// Token: 0x060033A3 RID: 13219
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Leaderboard_GetNextEntries(IntPtr handle);

		// Token: 0x060033A4 RID: 13220
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Leaderboard_GetPreviousEntries(IntPtr handle);

		// Token: 0x060033A5 RID: 13221 RVA: 0x00109CAC File Offset: 0x001080AC
		public static ulong ovr_Leaderboard_WriteEntry(string leaderboardName, long score, byte[] extraData, uint extraDataLength, bool forceUpdate)
		{
			IntPtr intPtr = CAPI.StringToNative(leaderboardName);
			ulong result = CAPI.ovr_Leaderboard_WriteEntry_Native(intPtr, score, extraData, extraDataLength, forceUpdate);
			Marshal.FreeCoTaskMem(intPtr);
			return result;
		}

		// Token: 0x060033A6 RID: 13222
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_Leaderboard_WriteEntry")]
		private static extern ulong ovr_Leaderboard_WriteEntry_Native(IntPtr leaderboardName, long score, byte[] extraData, uint extraDataLength, bool forceUpdate);

		// Token: 0x060033A7 RID: 13223
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Livestreaming_GetStatus();

		// Token: 0x060033A8 RID: 13224 RVA: 0x00109CD4 File Offset: 0x001080D4
		public static ulong ovr_Livestreaming_IsAllowedForApplication(string packageName)
		{
			IntPtr intPtr = CAPI.StringToNative(packageName);
			ulong result = CAPI.ovr_Livestreaming_IsAllowedForApplication_Native(intPtr);
			Marshal.FreeCoTaskMem(intPtr);
			return result;
		}

		// Token: 0x060033A9 RID: 13225
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_Livestreaming_IsAllowedForApplication")]
		private static extern ulong ovr_Livestreaming_IsAllowedForApplication_Native(IntPtr packageName);

		// Token: 0x060033AA RID: 13226
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Livestreaming_PauseStream();

		// Token: 0x060033AB RID: 13227
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Livestreaming_ResumeStream();

		// Token: 0x060033AC RID: 13228
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Livestreaming_StartPartyStream();

		// Token: 0x060033AD RID: 13229
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Livestreaming_StartStream(LivestreamingAudience audience, LivestreamingMicrophoneStatus micStatus);

		// Token: 0x060033AE RID: 13230
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Livestreaming_StopPartyStream();

		// Token: 0x060033AF RID: 13231
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Livestreaming_StopStream();

		// Token: 0x060033B0 RID: 13232
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Livestreaming_UpdateCommentsOverlayVisibility(bool isVisible);

		// Token: 0x060033B1 RID: 13233
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Livestreaming_UpdateMicStatus(LivestreamingMicrophoneStatus micStatus);

		// Token: 0x060033B2 RID: 13234 RVA: 0x00109CF8 File Offset: 0x001080F8
		public static ulong ovr_Matchmaking_Browse(string pool, IntPtr customQueryData)
		{
			IntPtr intPtr = CAPI.StringToNative(pool);
			ulong result = CAPI.ovr_Matchmaking_Browse_Native(intPtr, customQueryData);
			Marshal.FreeCoTaskMem(intPtr);
			return result;
		}

		// Token: 0x060033B3 RID: 13235
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_Matchmaking_Browse")]
		private static extern ulong ovr_Matchmaking_Browse_Native(IntPtr pool, IntPtr customQueryData);

		// Token: 0x060033B4 RID: 13236 RVA: 0x00109D1C File Offset: 0x0010811C
		public static ulong ovr_Matchmaking_Browse2(string pool, IntPtr matchmakingOptions)
		{
			IntPtr intPtr = CAPI.StringToNative(pool);
			ulong result = CAPI.ovr_Matchmaking_Browse2_Native(intPtr, matchmakingOptions);
			Marshal.FreeCoTaskMem(intPtr);
			return result;
		}

		// Token: 0x060033B5 RID: 13237
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_Matchmaking_Browse2")]
		private static extern ulong ovr_Matchmaking_Browse2_Native(IntPtr pool, IntPtr matchmakingOptions);

		// Token: 0x060033B6 RID: 13238 RVA: 0x00109D40 File Offset: 0x00108140
		public static ulong ovr_Matchmaking_Cancel(string pool, string requestHash)
		{
			IntPtr intPtr = CAPI.StringToNative(pool);
			IntPtr intPtr2 = CAPI.StringToNative(requestHash);
			ulong result = CAPI.ovr_Matchmaking_Cancel_Native(intPtr, intPtr2);
			Marshal.FreeCoTaskMem(intPtr);
			Marshal.FreeCoTaskMem(intPtr2);
			return result;
		}

		// Token: 0x060033B7 RID: 13239
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_Matchmaking_Cancel")]
		private static extern ulong ovr_Matchmaking_Cancel_Native(IntPtr pool, IntPtr requestHash);

		// Token: 0x060033B8 RID: 13240
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Matchmaking_Cancel2();

		// Token: 0x060033B9 RID: 13241 RVA: 0x00109D70 File Offset: 0x00108170
		public static ulong ovr_Matchmaking_CreateAndEnqueueRoom(string pool, uint maxUsers, bool subscribeToUpdates, IntPtr customQueryData)
		{
			IntPtr intPtr = CAPI.StringToNative(pool);
			ulong result = CAPI.ovr_Matchmaking_CreateAndEnqueueRoom_Native(intPtr, maxUsers, subscribeToUpdates, customQueryData);
			Marshal.FreeCoTaskMem(intPtr);
			return result;
		}

		// Token: 0x060033BA RID: 13242
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_Matchmaking_CreateAndEnqueueRoom")]
		private static extern ulong ovr_Matchmaking_CreateAndEnqueueRoom_Native(IntPtr pool, uint maxUsers, bool subscribeToUpdates, IntPtr customQueryData);

		// Token: 0x060033BB RID: 13243 RVA: 0x00109D98 File Offset: 0x00108198
		public static ulong ovr_Matchmaking_CreateAndEnqueueRoom2(string pool, IntPtr matchmakingOptions)
		{
			IntPtr intPtr = CAPI.StringToNative(pool);
			ulong result = CAPI.ovr_Matchmaking_CreateAndEnqueueRoom2_Native(intPtr, matchmakingOptions);
			Marshal.FreeCoTaskMem(intPtr);
			return result;
		}

		// Token: 0x060033BC RID: 13244
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_Matchmaking_CreateAndEnqueueRoom2")]
		private static extern ulong ovr_Matchmaking_CreateAndEnqueueRoom2_Native(IntPtr pool, IntPtr matchmakingOptions);

		// Token: 0x060033BD RID: 13245 RVA: 0x00109DBC File Offset: 0x001081BC
		public static ulong ovr_Matchmaking_CreateRoom(string pool, uint maxUsers, bool subscribeToUpdates)
		{
			IntPtr intPtr = CAPI.StringToNative(pool);
			ulong result = CAPI.ovr_Matchmaking_CreateRoom_Native(intPtr, maxUsers, subscribeToUpdates);
			Marshal.FreeCoTaskMem(intPtr);
			return result;
		}

		// Token: 0x060033BE RID: 13246
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_Matchmaking_CreateRoom")]
		private static extern ulong ovr_Matchmaking_CreateRoom_Native(IntPtr pool, uint maxUsers, bool subscribeToUpdates);

		// Token: 0x060033BF RID: 13247 RVA: 0x00109DE0 File Offset: 0x001081E0
		public static ulong ovr_Matchmaking_CreateRoom2(string pool, IntPtr matchmakingOptions)
		{
			IntPtr intPtr = CAPI.StringToNative(pool);
			ulong result = CAPI.ovr_Matchmaking_CreateRoom2_Native(intPtr, matchmakingOptions);
			Marshal.FreeCoTaskMem(intPtr);
			return result;
		}

		// Token: 0x060033C0 RID: 13248
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_Matchmaking_CreateRoom2")]
		private static extern ulong ovr_Matchmaking_CreateRoom2_Native(IntPtr pool, IntPtr matchmakingOptions);

		// Token: 0x060033C1 RID: 13249 RVA: 0x00109E04 File Offset: 0x00108204
		public static ulong ovr_Matchmaking_Enqueue(string pool, IntPtr customQueryData)
		{
			IntPtr intPtr = CAPI.StringToNative(pool);
			ulong result = CAPI.ovr_Matchmaking_Enqueue_Native(intPtr, customQueryData);
			Marshal.FreeCoTaskMem(intPtr);
			return result;
		}

		// Token: 0x060033C2 RID: 13250
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_Matchmaking_Enqueue")]
		private static extern ulong ovr_Matchmaking_Enqueue_Native(IntPtr pool, IntPtr customQueryData);

		// Token: 0x060033C3 RID: 13251 RVA: 0x00109E28 File Offset: 0x00108228
		public static ulong ovr_Matchmaking_Enqueue2(string pool, IntPtr matchmakingOptions)
		{
			IntPtr intPtr = CAPI.StringToNative(pool);
			ulong result = CAPI.ovr_Matchmaking_Enqueue2_Native(intPtr, matchmakingOptions);
			Marshal.FreeCoTaskMem(intPtr);
			return result;
		}

		// Token: 0x060033C4 RID: 13252
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_Matchmaking_Enqueue2")]
		private static extern ulong ovr_Matchmaking_Enqueue2_Native(IntPtr pool, IntPtr matchmakingOptions);

		// Token: 0x060033C5 RID: 13253
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Matchmaking_EnqueueRoom(ulong roomID, IntPtr customQueryData);

		// Token: 0x060033C6 RID: 13254
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Matchmaking_EnqueueRoom2(ulong roomID, IntPtr matchmakingOptions);

		// Token: 0x060033C7 RID: 13255
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Matchmaking_GetAdminSnapshot();

		// Token: 0x060033C8 RID: 13256 RVA: 0x00109E4C File Offset: 0x0010824C
		public static ulong ovr_Matchmaking_GetStats(string pool, uint maxLevel, MatchmakingStatApproach approach)
		{
			IntPtr intPtr = CAPI.StringToNative(pool);
			ulong result = CAPI.ovr_Matchmaking_GetStats_Native(intPtr, maxLevel, approach);
			Marshal.FreeCoTaskMem(intPtr);
			return result;
		}

		// Token: 0x060033C9 RID: 13257
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_Matchmaking_GetStats")]
		private static extern ulong ovr_Matchmaking_GetStats_Native(IntPtr pool, uint maxLevel, MatchmakingStatApproach approach);

		// Token: 0x060033CA RID: 13258
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Matchmaking_JoinRoom(ulong roomID, bool subscribeToUpdates);

		// Token: 0x060033CB RID: 13259 RVA: 0x00109E70 File Offset: 0x00108270
		public static ulong ovr_Matchmaking_ReportResultInsecure(ulong roomID, CAPI.ovrKeyValuePair[] data)
		{
			UIntPtr numItems = (UIntPtr)((ulong)((long)data.Length));
			return CAPI.ovr_Matchmaking_ReportResultInsecure_Native(roomID, data, numItems);
		}

		// Token: 0x060033CC RID: 13260
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_Matchmaking_ReportResultInsecure")]
		private static extern ulong ovr_Matchmaking_ReportResultInsecure_Native(ulong roomID, CAPI.ovrKeyValuePair[] data, UIntPtr numItems);

		// Token: 0x060033CD RID: 13261
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Matchmaking_StartMatch(ulong roomID);

		// Token: 0x060033CE RID: 13262 RVA: 0x00109E94 File Offset: 0x00108294
		public static ulong ovr_Media_ShareToFacebook(string postTextSuggestion, string filePath, MediaContentType contentType)
		{
			IntPtr intPtr = CAPI.StringToNative(postTextSuggestion);
			IntPtr intPtr2 = CAPI.StringToNative(filePath);
			ulong result = CAPI.ovr_Media_ShareToFacebook_Native(intPtr, intPtr2, contentType);
			Marshal.FreeCoTaskMem(intPtr);
			Marshal.FreeCoTaskMem(intPtr2);
			return result;
		}

		// Token: 0x060033CF RID: 13263
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_Media_ShareToFacebook")]
		private static extern ulong ovr_Media_ShareToFacebook_Native(IntPtr postTextSuggestion, IntPtr filePath, MediaContentType contentType);

		// Token: 0x060033D0 RID: 13264
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Notification_GetRoomInvites();

		// Token: 0x060033D1 RID: 13265
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Notification_MarkAsRead(ulong notificationID);

		// Token: 0x060033D2 RID: 13266
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Party_Create();

		// Token: 0x060033D3 RID: 13267
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Party_GatherInApplication(ulong partyID, ulong appID);

		// Token: 0x060033D4 RID: 13268
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Party_Get(ulong partyID);

		// Token: 0x060033D5 RID: 13269
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Party_GetCurrent();

		// Token: 0x060033D6 RID: 13270
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Party_GetCurrentForUser(ulong userID);

		// Token: 0x060033D7 RID: 13271
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Party_Invite(ulong partyID, ulong userID);

		// Token: 0x060033D8 RID: 13272
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Party_Join(ulong partyID);

		// Token: 0x060033D9 RID: 13273
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Party_Leave(ulong partyID);

		// Token: 0x060033DA RID: 13274
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Room_CreateAndJoinPrivate(RoomJoinPolicy joinPolicy, uint maxUsers, bool subscribeToUpdates);

		// Token: 0x060033DB RID: 13275
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Room_CreateAndJoinPrivate2(RoomJoinPolicy joinPolicy, uint maxUsers, IntPtr roomOptions);

		// Token: 0x060033DC RID: 13276
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Room_Get(ulong roomID);

		// Token: 0x060033DD RID: 13277
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Room_GetCurrent();

		// Token: 0x060033DE RID: 13278
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Room_GetCurrentForUser(ulong userID);

		// Token: 0x060033DF RID: 13279
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Room_GetInvitableUsers();

		// Token: 0x060033E0 RID: 13280
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Room_GetInvitableUsers2(IntPtr roomOptions);

		// Token: 0x060033E1 RID: 13281
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Room_GetModeratedRooms();

		// Token: 0x060033E2 RID: 13282
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Room_GetSocialRooms(ulong appID);

		// Token: 0x060033E3 RID: 13283 RVA: 0x00109EC8 File Offset: 0x001082C8
		public static ulong ovr_Room_InviteUser(ulong roomID, string inviteToken)
		{
			IntPtr intPtr = CAPI.StringToNative(inviteToken);
			ulong result = CAPI.ovr_Room_InviteUser_Native(roomID, intPtr);
			Marshal.FreeCoTaskMem(intPtr);
			return result;
		}

		// Token: 0x060033E4 RID: 13284
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_Room_InviteUser")]
		private static extern ulong ovr_Room_InviteUser_Native(ulong roomID, IntPtr inviteToken);

		// Token: 0x060033E5 RID: 13285
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Room_Join(ulong roomID, bool subscribeToUpdates);

		// Token: 0x060033E6 RID: 13286
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Room_Join2(ulong roomID, IntPtr roomOptions);

		// Token: 0x060033E7 RID: 13287
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Room_KickUser(ulong roomID, ulong userID, int kickDurationSeconds);

		// Token: 0x060033E8 RID: 13288
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Room_LaunchInvitableUserFlow(ulong roomID);

		// Token: 0x060033E9 RID: 13289
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Room_Leave(ulong roomID);

		// Token: 0x060033EA RID: 13290 RVA: 0x00109EEC File Offset: 0x001082EC
		public static ulong ovr_Room_SetDescription(ulong roomID, string description)
		{
			IntPtr intPtr = CAPI.StringToNative(description);
			ulong result = CAPI.ovr_Room_SetDescription_Native(roomID, intPtr);
			Marshal.FreeCoTaskMem(intPtr);
			return result;
		}

		// Token: 0x060033EB RID: 13291
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_Room_SetDescription")]
		private static extern ulong ovr_Room_SetDescription_Native(ulong roomID, IntPtr description);

		// Token: 0x060033EC RID: 13292 RVA: 0x00109F10 File Offset: 0x00108310
		public static ulong ovr_Room_UpdateDataStore(ulong roomID, CAPI.ovrKeyValuePair[] data)
		{
			UIntPtr numItems = (UIntPtr)((ulong)((long)data.Length));
			return CAPI.ovr_Room_UpdateDataStore_Native(roomID, data, numItems);
		}

		// Token: 0x060033ED RID: 13293
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_Room_UpdateDataStore")]
		private static extern ulong ovr_Room_UpdateDataStore_Native(ulong roomID, CAPI.ovrKeyValuePair[] data, UIntPtr numItems);

		// Token: 0x060033EE RID: 13294
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Room_UpdateMembershipLockStatus(ulong roomID, RoomMembershipLockStatus membershipLockStatus);

		// Token: 0x060033EF RID: 13295
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Room_UpdateOwner(ulong roomID, ulong userID);

		// Token: 0x060033F0 RID: 13296
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Room_UpdatePrivateRoomJoinPolicy(ulong roomID, RoomJoinPolicy newJoinPolicy);

		// Token: 0x060033F1 RID: 13297
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_SystemPermissions_GetStatus(PermissionType permType);

		// Token: 0x060033F2 RID: 13298
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_SystemPermissions_LaunchDeeplink(PermissionType permType);

		// Token: 0x060033F3 RID: 13299
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_User_Get(ulong userID);

		// Token: 0x060033F4 RID: 13300
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_User_GetAccessToken();

		// Token: 0x060033F5 RID: 13301
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_User_GetLoggedInUser();

		// Token: 0x060033F6 RID: 13302
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_User_GetLoggedInUserFriends();

		// Token: 0x060033F7 RID: 13303
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_User_GetLoggedInUserFriendsAndRooms();

		// Token: 0x060033F8 RID: 13304
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_User_GetLoggedInUserRecentlyMetUsersAndRooms(IntPtr userOptions);

		// Token: 0x060033F9 RID: 13305
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_User_GetOrgScopedID(ulong userID);

		// Token: 0x060033FA RID: 13306
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_User_GetSdkAccounts();

		// Token: 0x060033FB RID: 13307
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_User_GetUserProof();

		// Token: 0x060033FC RID: 13308
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_User_LaunchBlockFlow(ulong userID);

		// Token: 0x060033FD RID: 13309
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_User_LaunchProfile(ulong userID);

		// Token: 0x060033FE RID: 13310
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_User_LaunchReportFlow(ulong userID);

		// Token: 0x060033FF RID: 13311
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_User_NewEntitledTestUser();

		// Token: 0x06003400 RID: 13312
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_User_NewTestUser();

		// Token: 0x06003401 RID: 13313
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_User_NewTestUserFriends();

		// Token: 0x06003402 RID: 13314
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Voip_SetSystemVoipSuppressed(bool suppressed);

		// Token: 0x06003403 RID: 13315
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ovr_AchievementDefinition_GetBitfieldLength(IntPtr obj);

		// Token: 0x06003404 RID: 13316 RVA: 0x00109F34 File Offset: 0x00108334
		public static string ovr_AchievementDefinition_GetName(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_AchievementDefinition_GetName_Native(obj));
		}

		// Token: 0x06003405 RID: 13317
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_AchievementDefinition_GetName")]
		private static extern IntPtr ovr_AchievementDefinition_GetName_Native(IntPtr obj);

		// Token: 0x06003406 RID: 13318
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_AchievementDefinition_GetTarget(IntPtr obj);

		// Token: 0x06003407 RID: 13319
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern AchievementType ovr_AchievementDefinition_GetType(IntPtr obj);

		// Token: 0x06003408 RID: 13320
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_AchievementDefinitionArray_GetElement(IntPtr obj, UIntPtr index);

		// Token: 0x06003409 RID: 13321 RVA: 0x00109F50 File Offset: 0x00108350
		public static string ovr_AchievementDefinitionArray_GetNextUrl(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_AchievementDefinitionArray_GetNextUrl_Native(obj));
		}

		// Token: 0x0600340A RID: 13322
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_AchievementDefinitionArray_GetNextUrl")]
		private static extern IntPtr ovr_AchievementDefinitionArray_GetNextUrl_Native(IntPtr obj);

		// Token: 0x0600340B RID: 13323
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr ovr_AchievementDefinitionArray_GetSize(IntPtr obj);

		// Token: 0x0600340C RID: 13324
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool ovr_AchievementDefinitionArray_HasNextPage(IntPtr obj);

		// Token: 0x0600340D RID: 13325 RVA: 0x00109F6C File Offset: 0x0010836C
		public static string ovr_AchievementProgress_GetBitfield(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_AchievementProgress_GetBitfield_Native(obj));
		}

		// Token: 0x0600340E RID: 13326
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_AchievementProgress_GetBitfield")]
		private static extern IntPtr ovr_AchievementProgress_GetBitfield_Native(IntPtr obj);

		// Token: 0x0600340F RID: 13327
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_AchievementProgress_GetCount(IntPtr obj);

		// Token: 0x06003410 RID: 13328
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool ovr_AchievementProgress_GetIsUnlocked(IntPtr obj);

		// Token: 0x06003411 RID: 13329 RVA: 0x00109F88 File Offset: 0x00108388
		public static string ovr_AchievementProgress_GetName(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_AchievementProgress_GetName_Native(obj));
		}

		// Token: 0x06003412 RID: 13330
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_AchievementProgress_GetName")]
		private static extern IntPtr ovr_AchievementProgress_GetName_Native(IntPtr obj);

		// Token: 0x06003413 RID: 13331 RVA: 0x00109FA4 File Offset: 0x001083A4
		public static DateTime ovr_AchievementProgress_GetUnlockTime(IntPtr obj)
		{
			return CAPI.DateTimeFromNative(CAPI.ovr_AchievementProgress_GetUnlockTime_Native(obj));
		}

		// Token: 0x06003414 RID: 13332
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_AchievementProgress_GetUnlockTime")]
		private static extern ulong ovr_AchievementProgress_GetUnlockTime_Native(IntPtr obj);

		// Token: 0x06003415 RID: 13333
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_AchievementProgressArray_GetElement(IntPtr obj, UIntPtr index);

		// Token: 0x06003416 RID: 13334 RVA: 0x00109FC0 File Offset: 0x001083C0
		public static string ovr_AchievementProgressArray_GetNextUrl(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_AchievementProgressArray_GetNextUrl_Native(obj));
		}

		// Token: 0x06003417 RID: 13335
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_AchievementProgressArray_GetNextUrl")]
		private static extern IntPtr ovr_AchievementProgressArray_GetNextUrl_Native(IntPtr obj);

		// Token: 0x06003418 RID: 13336
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr ovr_AchievementProgressArray_GetSize(IntPtr obj);

		// Token: 0x06003419 RID: 13337
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool ovr_AchievementProgressArray_HasNextPage(IntPtr obj);

		// Token: 0x0600341A RID: 13338
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool ovr_AchievementUpdate_GetJustUnlocked(IntPtr obj);

		// Token: 0x0600341B RID: 13339 RVA: 0x00109FDC File Offset: 0x001083DC
		public static string ovr_AchievementUpdate_GetName(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_AchievementUpdate_GetName_Native(obj));
		}

		// Token: 0x0600341C RID: 13340
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_AchievementUpdate_GetName")]
		private static extern IntPtr ovr_AchievementUpdate_GetName_Native(IntPtr obj);

		// Token: 0x0600341D RID: 13341
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Application_GetID(IntPtr obj);

		// Token: 0x0600341E RID: 13342
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ovr_ApplicationVersion_GetCurrentCode(IntPtr obj);

		// Token: 0x0600341F RID: 13343 RVA: 0x00109FF8 File Offset: 0x001083F8
		public static string ovr_ApplicationVersion_GetCurrentName(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_ApplicationVersion_GetCurrentName_Native(obj));
		}

		// Token: 0x06003420 RID: 13344
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_ApplicationVersion_GetCurrentName")]
		private static extern IntPtr ovr_ApplicationVersion_GetCurrentName_Native(IntPtr obj);

		// Token: 0x06003421 RID: 13345
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ovr_ApplicationVersion_GetLatestCode(IntPtr obj);

		// Token: 0x06003422 RID: 13346 RVA: 0x0010A014 File Offset: 0x00108414
		public static string ovr_ApplicationVersion_GetLatestName(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_ApplicationVersion_GetLatestName_Native(obj));
		}

		// Token: 0x06003423 RID: 13347
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_ApplicationVersion_GetLatestName")]
		private static extern IntPtr ovr_ApplicationVersion_GetLatestName_Native(IntPtr obj);

		// Token: 0x06003424 RID: 13348
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_AssetFileDeleteResult_GetAssetFileId(IntPtr obj);

		// Token: 0x06003425 RID: 13349
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool ovr_AssetFileDeleteResult_GetSuccess(IntPtr obj);

		// Token: 0x06003426 RID: 13350
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_AssetFileDownloadCancelResult_GetAssetFileId(IntPtr obj);

		// Token: 0x06003427 RID: 13351
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool ovr_AssetFileDownloadCancelResult_GetSuccess(IntPtr obj);

		// Token: 0x06003428 RID: 13352 RVA: 0x0010A030 File Offset: 0x00108430
		public static string ovr_AssetFileDownloadResult_GetFilepath(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_AssetFileDownloadResult_GetFilepath_Native(obj));
		}

		// Token: 0x06003429 RID: 13353
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_AssetFileDownloadResult_GetFilepath")]
		private static extern IntPtr ovr_AssetFileDownloadResult_GetFilepath_Native(IntPtr obj);

		// Token: 0x0600342A RID: 13354
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_AssetFileDownloadUpdate_GetAssetFileId(IntPtr obj);

		// Token: 0x0600342B RID: 13355
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ovr_AssetFileDownloadUpdate_GetBytesTotal(IntPtr obj);

		// Token: 0x0600342C RID: 13356
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ovr_AssetFileDownloadUpdate_GetBytesTransferred(IntPtr obj);

		// Token: 0x0600342D RID: 13357
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool ovr_AssetFileDownloadUpdate_GetCompleted(IntPtr obj);

		// Token: 0x0600342E RID: 13358
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_CloudStorageConflictMetadata_GetLocal(IntPtr obj);

		// Token: 0x0600342F RID: 13359
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_CloudStorageConflictMetadata_GetRemote(IntPtr obj);

		// Token: 0x06003430 RID: 13360 RVA: 0x0010A04C File Offset: 0x0010844C
		public static string ovr_CloudStorageData_GetBucket(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_CloudStorageData_GetBucket_Native(obj));
		}

		// Token: 0x06003431 RID: 13361
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_CloudStorageData_GetBucket")]
		private static extern IntPtr ovr_CloudStorageData_GetBucket_Native(IntPtr obj);

		// Token: 0x06003432 RID: 13362 RVA: 0x0010A068 File Offset: 0x00108468
		public static byte[] ovr_CloudStorageData_GetData(IntPtr obj)
		{
			return CAPI.FiledataFromNative(CAPI.ovr_CloudStorageData_GetDataSize(obj), CAPI.ovr_CloudStorageData_GetData_Native(obj));
		}

		// Token: 0x06003433 RID: 13363
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_CloudStorageData_GetData")]
		private static extern IntPtr ovr_CloudStorageData_GetData_Native(IntPtr obj);

		// Token: 0x06003434 RID: 13364
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ovr_CloudStorageData_GetDataSize(IntPtr obj);

		// Token: 0x06003435 RID: 13365 RVA: 0x0010A088 File Offset: 0x00108488
		public static string ovr_CloudStorageData_GetKey(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_CloudStorageData_GetKey_Native(obj));
		}

		// Token: 0x06003436 RID: 13366
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_CloudStorageData_GetKey")]
		private static extern IntPtr ovr_CloudStorageData_GetKey_Native(IntPtr obj);

		// Token: 0x06003437 RID: 13367 RVA: 0x0010A0A4 File Offset: 0x001084A4
		public static string ovr_CloudStorageMetadata_GetBucket(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_CloudStorageMetadata_GetBucket_Native(obj));
		}

		// Token: 0x06003438 RID: 13368
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_CloudStorageMetadata_GetBucket")]
		private static extern IntPtr ovr_CloudStorageMetadata_GetBucket_Native(IntPtr obj);

		// Token: 0x06003439 RID: 13369
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern long ovr_CloudStorageMetadata_GetCounter(IntPtr obj);

		// Token: 0x0600343A RID: 13370
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ovr_CloudStorageMetadata_GetDataSize(IntPtr obj);

		// Token: 0x0600343B RID: 13371 RVA: 0x0010A0C0 File Offset: 0x001084C0
		public static string ovr_CloudStorageMetadata_GetExtraData(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_CloudStorageMetadata_GetExtraData_Native(obj));
		}

		// Token: 0x0600343C RID: 13372
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_CloudStorageMetadata_GetExtraData")]
		private static extern IntPtr ovr_CloudStorageMetadata_GetExtraData_Native(IntPtr obj);

		// Token: 0x0600343D RID: 13373 RVA: 0x0010A0DC File Offset: 0x001084DC
		public static string ovr_CloudStorageMetadata_GetKey(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_CloudStorageMetadata_GetKey_Native(obj));
		}

		// Token: 0x0600343E RID: 13374
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_CloudStorageMetadata_GetKey")]
		private static extern IntPtr ovr_CloudStorageMetadata_GetKey_Native(IntPtr obj);

		// Token: 0x0600343F RID: 13375
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_CloudStorageMetadata_GetSaveTime(IntPtr obj);

		// Token: 0x06003440 RID: 13376
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern CloudStorageDataStatus ovr_CloudStorageMetadata_GetStatus(IntPtr obj);

		// Token: 0x06003441 RID: 13377 RVA: 0x0010A0F8 File Offset: 0x001084F8
		public static string ovr_CloudStorageMetadata_GetVersionHandle(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_CloudStorageMetadata_GetVersionHandle_Native(obj));
		}

		// Token: 0x06003442 RID: 13378
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_CloudStorageMetadata_GetVersionHandle")]
		private static extern IntPtr ovr_CloudStorageMetadata_GetVersionHandle_Native(IntPtr obj);

		// Token: 0x06003443 RID: 13379
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_CloudStorageMetadataArray_GetElement(IntPtr obj, UIntPtr index);

		// Token: 0x06003444 RID: 13380 RVA: 0x0010A114 File Offset: 0x00108514
		public static string ovr_CloudStorageMetadataArray_GetNextUrl(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_CloudStorageMetadataArray_GetNextUrl_Native(obj));
		}

		// Token: 0x06003445 RID: 13381
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_CloudStorageMetadataArray_GetNextUrl")]
		private static extern IntPtr ovr_CloudStorageMetadataArray_GetNextUrl_Native(IntPtr obj);

		// Token: 0x06003446 RID: 13382
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr ovr_CloudStorageMetadataArray_GetSize(IntPtr obj);

		// Token: 0x06003447 RID: 13383
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool ovr_CloudStorageMetadataArray_HasNextPage(IntPtr obj);

		// Token: 0x06003448 RID: 13384 RVA: 0x0010A130 File Offset: 0x00108530
		public static string ovr_CloudStorageUpdateResponse_GetBucket(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_CloudStorageUpdateResponse_GetBucket_Native(obj));
		}

		// Token: 0x06003449 RID: 13385
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_CloudStorageUpdateResponse_GetBucket")]
		private static extern IntPtr ovr_CloudStorageUpdateResponse_GetBucket_Native(IntPtr obj);

		// Token: 0x0600344A RID: 13386 RVA: 0x0010A14C File Offset: 0x0010854C
		public static string ovr_CloudStorageUpdateResponse_GetKey(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_CloudStorageUpdateResponse_GetKey_Native(obj));
		}

		// Token: 0x0600344B RID: 13387
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_CloudStorageUpdateResponse_GetKey")]
		private static extern IntPtr ovr_CloudStorageUpdateResponse_GetKey_Native(IntPtr obj);

		// Token: 0x0600344C RID: 13388
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern CloudStorageUpdateStatus ovr_CloudStorageUpdateResponse_GetStatus(IntPtr obj);

		// Token: 0x0600344D RID: 13389 RVA: 0x0010A168 File Offset: 0x00108568
		public static string ovr_CloudStorageUpdateResponse_GetVersionHandle(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_CloudStorageUpdateResponse_GetVersionHandle_Native(obj));
		}

		// Token: 0x0600344E RID: 13390
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_CloudStorageUpdateResponse_GetVersionHandle")]
		private static extern IntPtr ovr_CloudStorageUpdateResponse_GetVersionHandle_Native(IntPtr obj);

		// Token: 0x0600344F RID: 13391 RVA: 0x0010A184 File Offset: 0x00108584
		public static uint ovr_DataStore_Contains(IntPtr obj, string key)
		{
			IntPtr intPtr = CAPI.StringToNative(key);
			uint result = CAPI.ovr_DataStore_Contains_Native(obj, intPtr);
			Marshal.FreeCoTaskMem(intPtr);
			return result;
		}

		// Token: 0x06003450 RID: 13392
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_DataStore_Contains")]
		private static extern uint ovr_DataStore_Contains_Native(IntPtr obj, IntPtr key);

		// Token: 0x06003451 RID: 13393 RVA: 0x0010A1A8 File Offset: 0x001085A8
		public static string ovr_DataStore_GetKey(IntPtr obj, int index)
		{
			return CAPI.StringFromNative(CAPI.ovr_DataStore_GetKey_Native(obj, index));
		}

		// Token: 0x06003452 RID: 13394
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_DataStore_GetKey")]
		private static extern IntPtr ovr_DataStore_GetKey_Native(IntPtr obj, int index);

		// Token: 0x06003453 RID: 13395
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr ovr_DataStore_GetNumKeys(IntPtr obj);

		// Token: 0x06003454 RID: 13396 RVA: 0x0010A1C4 File Offset: 0x001085C4
		public static string ovr_DataStore_GetValue(IntPtr obj, string key)
		{
			IntPtr intPtr = CAPI.StringToNative(key);
			string result = CAPI.StringFromNative(CAPI.ovr_DataStore_GetValue_Native(obj, intPtr));
			Marshal.FreeCoTaskMem(intPtr);
			return result;
		}

		// Token: 0x06003455 RID: 13397
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_DataStore_GetValue")]
		private static extern IntPtr ovr_DataStore_GetValue_Native(IntPtr obj, IntPtr key);

		// Token: 0x06003456 RID: 13398
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ovr_Error_GetCode(IntPtr obj);

		// Token: 0x06003457 RID: 13399 RVA: 0x0010A1EC File Offset: 0x001085EC
		public static string ovr_Error_GetDisplayableMessage(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_Error_GetDisplayableMessage_Native(obj));
		}

		// Token: 0x06003458 RID: 13400
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_Error_GetDisplayableMessage")]
		private static extern IntPtr ovr_Error_GetDisplayableMessage_Native(IntPtr obj);

		// Token: 0x06003459 RID: 13401
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ovr_Error_GetHttpCode(IntPtr obj);

		// Token: 0x0600345A RID: 13402 RVA: 0x0010A208 File Offset: 0x00108608
		public static string ovr_Error_GetMessage(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_Error_GetMessage_Native(obj));
		}

		// Token: 0x0600345B RID: 13403
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_Error_GetMessage")]
		private static extern IntPtr ovr_Error_GetMessage_Native(IntPtr obj);

		// Token: 0x0600345C RID: 13404
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_HttpTransferUpdate_GetBytes(IntPtr obj);

		// Token: 0x0600345D RID: 13405
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_HttpTransferUpdate_GetID(IntPtr obj);

		// Token: 0x0600345E RID: 13406
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr ovr_HttpTransferUpdate_GetSize(IntPtr obj);

		// Token: 0x0600345F RID: 13407
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool ovr_HttpTransferUpdate_IsCompleted(IntPtr obj);

		// Token: 0x06003460 RID: 13408 RVA: 0x0010A224 File Offset: 0x00108624
		public static string ovr_InstalledApplication_GetApplicationId(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_InstalledApplication_GetApplicationId_Native(obj));
		}

		// Token: 0x06003461 RID: 13409
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_InstalledApplication_GetApplicationId")]
		private static extern IntPtr ovr_InstalledApplication_GetApplicationId_Native(IntPtr obj);

		// Token: 0x06003462 RID: 13410 RVA: 0x0010A240 File Offset: 0x00108640
		public static string ovr_InstalledApplication_GetPackageName(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_InstalledApplication_GetPackageName_Native(obj));
		}

		// Token: 0x06003463 RID: 13411
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_InstalledApplication_GetPackageName")]
		private static extern IntPtr ovr_InstalledApplication_GetPackageName_Native(IntPtr obj);

		// Token: 0x06003464 RID: 13412 RVA: 0x0010A25C File Offset: 0x0010865C
		public static string ovr_InstalledApplication_GetStatus(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_InstalledApplication_GetStatus_Native(obj));
		}

		// Token: 0x06003465 RID: 13413
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_InstalledApplication_GetStatus")]
		private static extern IntPtr ovr_InstalledApplication_GetStatus_Native(IntPtr obj);

		// Token: 0x06003466 RID: 13414
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ovr_InstalledApplication_GetVersionCode(IntPtr obj);

		// Token: 0x06003467 RID: 13415 RVA: 0x0010A278 File Offset: 0x00108678
		public static string ovr_InstalledApplication_GetVersionName(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_InstalledApplication_GetVersionName_Native(obj));
		}

		// Token: 0x06003468 RID: 13416
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_InstalledApplication_GetVersionName")]
		private static extern IntPtr ovr_InstalledApplication_GetVersionName_Native(IntPtr obj);

		// Token: 0x06003469 RID: 13417
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_InstalledApplicationArray_GetElement(IntPtr obj, UIntPtr index);

		// Token: 0x0600346A RID: 13418
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr ovr_InstalledApplicationArray_GetSize(IntPtr obj);

		// Token: 0x0600346B RID: 13419 RVA: 0x0010A294 File Offset: 0x00108694
		public static string ovr_LaunchDetails_GetDeeplinkMessage(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_LaunchDetails_GetDeeplinkMessage_Native(obj));
		}

		// Token: 0x0600346C RID: 13420
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_LaunchDetails_GetDeeplinkMessage")]
		private static extern IntPtr ovr_LaunchDetails_GetDeeplinkMessage_Native(IntPtr obj);

		// Token: 0x0600346D RID: 13421 RVA: 0x0010A2B0 File Offset: 0x001086B0
		public static string ovr_LaunchDetails_GetLaunchSource(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_LaunchDetails_GetLaunchSource_Native(obj));
		}

		// Token: 0x0600346E RID: 13422
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_LaunchDetails_GetLaunchSource")]
		private static extern IntPtr ovr_LaunchDetails_GetLaunchSource_Native(IntPtr obj);

		// Token: 0x0600346F RID: 13423
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern LaunchType ovr_LaunchDetails_GetLaunchType(IntPtr obj);

		// Token: 0x06003470 RID: 13424
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_LaunchDetails_GetRoomID(IntPtr obj);

		// Token: 0x06003471 RID: 13425
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_LaunchDetails_GetUsers(IntPtr obj);

		// Token: 0x06003472 RID: 13426 RVA: 0x0010A2CC File Offset: 0x001086CC
		public static byte[] ovr_LeaderboardEntry_GetExtraData(IntPtr obj)
		{
			return CAPI.BlobFromNative(CAPI.ovr_LeaderboardEntry_GetExtraDataLength(obj), CAPI.ovr_LeaderboardEntry_GetExtraData_Native(obj));
		}

		// Token: 0x06003473 RID: 13427
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_LeaderboardEntry_GetExtraData")]
		private static extern IntPtr ovr_LeaderboardEntry_GetExtraData_Native(IntPtr obj);

		// Token: 0x06003474 RID: 13428
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ovr_LeaderboardEntry_GetExtraDataLength(IntPtr obj);

		// Token: 0x06003475 RID: 13429
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ovr_LeaderboardEntry_GetRank(IntPtr obj);

		// Token: 0x06003476 RID: 13430
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern long ovr_LeaderboardEntry_GetScore(IntPtr obj);

		// Token: 0x06003477 RID: 13431 RVA: 0x0010A2EC File Offset: 0x001086EC
		public static DateTime ovr_LeaderboardEntry_GetTimestamp(IntPtr obj)
		{
			return CAPI.DateTimeFromNative(CAPI.ovr_LeaderboardEntry_GetTimestamp_Native(obj));
		}

		// Token: 0x06003478 RID: 13432
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_LeaderboardEntry_GetTimestamp")]
		private static extern ulong ovr_LeaderboardEntry_GetTimestamp_Native(IntPtr obj);

		// Token: 0x06003479 RID: 13433
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_LeaderboardEntry_GetUser(IntPtr obj);

		// Token: 0x0600347A RID: 13434
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_LeaderboardEntryArray_GetElement(IntPtr obj, UIntPtr index);

		// Token: 0x0600347B RID: 13435 RVA: 0x0010A308 File Offset: 0x00108708
		public static string ovr_LeaderboardEntryArray_GetNextUrl(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_LeaderboardEntryArray_GetNextUrl_Native(obj));
		}

		// Token: 0x0600347C RID: 13436
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_LeaderboardEntryArray_GetNextUrl")]
		private static extern IntPtr ovr_LeaderboardEntryArray_GetNextUrl_Native(IntPtr obj);

		// Token: 0x0600347D RID: 13437 RVA: 0x0010A324 File Offset: 0x00108724
		public static string ovr_LeaderboardEntryArray_GetPreviousUrl(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_LeaderboardEntryArray_GetPreviousUrl_Native(obj));
		}

		// Token: 0x0600347E RID: 13438
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_LeaderboardEntryArray_GetPreviousUrl")]
		private static extern IntPtr ovr_LeaderboardEntryArray_GetPreviousUrl_Native(IntPtr obj);

		// Token: 0x0600347F RID: 13439
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr ovr_LeaderboardEntryArray_GetSize(IntPtr obj);

		// Token: 0x06003480 RID: 13440
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_LeaderboardEntryArray_GetTotalCount(IntPtr obj);

		// Token: 0x06003481 RID: 13441
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool ovr_LeaderboardEntryArray_HasNextPage(IntPtr obj);

		// Token: 0x06003482 RID: 13442
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool ovr_LeaderboardEntryArray_HasPreviousPage(IntPtr obj);

		// Token: 0x06003483 RID: 13443
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool ovr_LeaderboardUpdateStatus_GetDidUpdate(IntPtr obj);

		// Token: 0x06003484 RID: 13444
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool ovr_LivestreamingApplicationStatus_GetStreamingEnabled(IntPtr obj);

		// Token: 0x06003485 RID: 13445
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern LivestreamingStartStatus ovr_LivestreamingStartResult_GetStreamingResult(IntPtr obj);

		// Token: 0x06003486 RID: 13446
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool ovr_LivestreamingStatus_GetCommentsVisible(IntPtr obj);

		// Token: 0x06003487 RID: 13447
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool ovr_LivestreamingStatus_GetIsPaused(IntPtr obj);

		// Token: 0x06003488 RID: 13448
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool ovr_LivestreamingStatus_GetLivestreamingEnabled(IntPtr obj);

		// Token: 0x06003489 RID: 13449
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ovr_LivestreamingStatus_GetLivestreamingType(IntPtr obj);

		// Token: 0x0600348A RID: 13450
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool ovr_LivestreamingStatus_GetMicEnabled(IntPtr obj);

		// Token: 0x0600348B RID: 13451
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ovr_LivestreamingVideoStats_GetCommentCount(IntPtr obj);

		// Token: 0x0600348C RID: 13452
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ovr_LivestreamingVideoStats_GetReactionCount(IntPtr obj);

		// Token: 0x0600348D RID: 13453 RVA: 0x0010A340 File Offset: 0x00108740
		public static string ovr_LivestreamingVideoStats_GetTotalViews(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_LivestreamingVideoStats_GetTotalViews_Native(obj));
		}

		// Token: 0x0600348E RID: 13454
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_LivestreamingVideoStats_GetTotalViews")]
		private static extern IntPtr ovr_LivestreamingVideoStats_GetTotalViews_Native(IntPtr obj);

		// Token: 0x0600348F RID: 13455
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_MatchmakingAdminSnapshot_GetCandidates(IntPtr obj);

		// Token: 0x06003490 RID: 13456
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern double ovr_MatchmakingAdminSnapshot_GetMyCurrentThreshold(IntPtr obj);

		// Token: 0x06003491 RID: 13457
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool ovr_MatchmakingAdminSnapshotCandidate_GetCanMatch(IntPtr obj);

		// Token: 0x06003492 RID: 13458
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern double ovr_MatchmakingAdminSnapshotCandidate_GetMyTotalScore(IntPtr obj);

		// Token: 0x06003493 RID: 13459
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern double ovr_MatchmakingAdminSnapshotCandidate_GetTheirCurrentThreshold(IntPtr obj);

		// Token: 0x06003494 RID: 13460
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern double ovr_MatchmakingAdminSnapshotCandidate_GetTheirTotalScore(IntPtr obj);

		// Token: 0x06003495 RID: 13461 RVA: 0x0010A35C File Offset: 0x0010875C
		public static string ovr_MatchmakingAdminSnapshotCandidate_GetTraceId(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_MatchmakingAdminSnapshotCandidate_GetTraceId_Native(obj));
		}

		// Token: 0x06003496 RID: 13462
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_MatchmakingAdminSnapshotCandidate_GetTraceId")]
		private static extern IntPtr ovr_MatchmakingAdminSnapshotCandidate_GetTraceId_Native(IntPtr obj);

		// Token: 0x06003497 RID: 13463
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_MatchmakingAdminSnapshotCandidateArray_GetElement(IntPtr obj, UIntPtr index);

		// Token: 0x06003498 RID: 13464
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr ovr_MatchmakingAdminSnapshotCandidateArray_GetSize(IntPtr obj);

		// Token: 0x06003499 RID: 13465
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_MatchmakingBrowseResult_GetEnqueueResult(IntPtr obj);

		// Token: 0x0600349A RID: 13466
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_MatchmakingBrowseResult_GetRooms(IntPtr obj);

		// Token: 0x0600349B RID: 13467 RVA: 0x0010A378 File Offset: 0x00108778
		public static string ovr_MatchmakingCandidate_GetEntryHash(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_MatchmakingCandidate_GetEntryHash_Native(obj));
		}

		// Token: 0x0600349C RID: 13468
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_MatchmakingCandidate_GetEntryHash")]
		private static extern IntPtr ovr_MatchmakingCandidate_GetEntryHash_Native(IntPtr obj);

		// Token: 0x0600349D RID: 13469
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_MatchmakingCandidate_GetUserId(IntPtr obj);

		// Token: 0x0600349E RID: 13470
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_MatchmakingCandidateArray_GetElement(IntPtr obj, UIntPtr index);

		// Token: 0x0600349F RID: 13471 RVA: 0x0010A394 File Offset: 0x00108794
		public static string ovr_MatchmakingCandidateArray_GetNextUrl(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_MatchmakingCandidateArray_GetNextUrl_Native(obj));
		}

		// Token: 0x060034A0 RID: 13472
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_MatchmakingCandidateArray_GetNextUrl")]
		private static extern IntPtr ovr_MatchmakingCandidateArray_GetNextUrl_Native(IntPtr obj);

		// Token: 0x060034A1 RID: 13473
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr ovr_MatchmakingCandidateArray_GetSize(IntPtr obj);

		// Token: 0x060034A2 RID: 13474
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool ovr_MatchmakingCandidateArray_HasNextPage(IntPtr obj);

		// Token: 0x060034A3 RID: 13475
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_MatchmakingEnqueueResult_GetAdminSnapshot(IntPtr obj);

		// Token: 0x060034A4 RID: 13476
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ovr_MatchmakingEnqueueResult_GetAverageWait(IntPtr obj);

		// Token: 0x060034A5 RID: 13477
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ovr_MatchmakingEnqueueResult_GetMatchesInLastHourCount(IntPtr obj);

		// Token: 0x060034A6 RID: 13478
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ovr_MatchmakingEnqueueResult_GetMaxExpectedWait(IntPtr obj);

		// Token: 0x060034A7 RID: 13479 RVA: 0x0010A3B0 File Offset: 0x001087B0
		public static string ovr_MatchmakingEnqueueResult_GetPool(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_MatchmakingEnqueueResult_GetPool_Native(obj));
		}

		// Token: 0x060034A8 RID: 13480
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_MatchmakingEnqueueResult_GetPool")]
		private static extern IntPtr ovr_MatchmakingEnqueueResult_GetPool_Native(IntPtr obj);

		// Token: 0x060034A9 RID: 13481
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ovr_MatchmakingEnqueueResult_GetRecentMatchPercentage(IntPtr obj);

		// Token: 0x060034AA RID: 13482 RVA: 0x0010A3CC File Offset: 0x001087CC
		public static string ovr_MatchmakingEnqueueResult_GetRequestHash(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_MatchmakingEnqueueResult_GetRequestHash_Native(obj));
		}

		// Token: 0x060034AB RID: 13483
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_MatchmakingEnqueueResult_GetRequestHash")]
		private static extern IntPtr ovr_MatchmakingEnqueueResult_GetRequestHash_Native(IntPtr obj);

		// Token: 0x060034AC RID: 13484
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_MatchmakingEnqueueResultAndRoom_GetMatchmakingEnqueueResult(IntPtr obj);

		// Token: 0x060034AD RID: 13485
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_MatchmakingEnqueueResultAndRoom_GetRoom(IntPtr obj);

		// Token: 0x060034AE RID: 13486
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_MatchmakingEnqueuedUser_GetAdditionalUserID(IntPtr obj, uint index);

		// Token: 0x060034AF RID: 13487
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ovr_MatchmakingEnqueuedUser_GetAdditionalUserIDsSize(IntPtr obj);

		// Token: 0x060034B0 RID: 13488
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_MatchmakingEnqueuedUser_GetCustomData(IntPtr obj);

		// Token: 0x060034B1 RID: 13489
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_MatchmakingEnqueuedUser_GetUser(IntPtr obj);

		// Token: 0x060034B2 RID: 13490
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_MatchmakingEnqueuedUserArray_GetElement(IntPtr obj, UIntPtr index);

		// Token: 0x060034B3 RID: 13491
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr ovr_MatchmakingEnqueuedUserArray_GetSize(IntPtr obj);

		// Token: 0x060034B4 RID: 13492
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_MatchmakingNotification_GetAddedByUserId(IntPtr obj);

		// Token: 0x060034B5 RID: 13493
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_MatchmakingNotification_GetRoom(IntPtr obj);

		// Token: 0x060034B6 RID: 13494 RVA: 0x0010A3E8 File Offset: 0x001087E8
		public static string ovr_MatchmakingNotification_GetTraceId(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_MatchmakingNotification_GetTraceId_Native(obj));
		}

		// Token: 0x060034B7 RID: 13495
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_MatchmakingNotification_GetTraceId")]
		private static extern IntPtr ovr_MatchmakingNotification_GetTraceId_Native(IntPtr obj);

		// Token: 0x060034B8 RID: 13496
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ovr_MatchmakingRoom_GetPingTime(IntPtr obj);

		// Token: 0x060034B9 RID: 13497
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_MatchmakingRoom_GetRoom(IntPtr obj);

		// Token: 0x060034BA RID: 13498
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool ovr_MatchmakingRoom_HasPingTime(IntPtr obj);

		// Token: 0x060034BB RID: 13499
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_MatchmakingRoomArray_GetElement(IntPtr obj, UIntPtr index);

		// Token: 0x060034BC RID: 13500
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr ovr_MatchmakingRoomArray_GetSize(IntPtr obj);

		// Token: 0x060034BD RID: 13501
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ovr_MatchmakingStats_GetDrawCount(IntPtr obj);

		// Token: 0x060034BE RID: 13502
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ovr_MatchmakingStats_GetLossCount(IntPtr obj);

		// Token: 0x060034BF RID: 13503
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ovr_MatchmakingStats_GetSkillLevel(IntPtr obj);

		// Token: 0x060034C0 RID: 13504
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ovr_MatchmakingStats_GetWinCount(IntPtr obj);

		// Token: 0x060034C1 RID: 13505
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetAchievementDefinitionArray(IntPtr obj);

		// Token: 0x060034C2 RID: 13506
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetAchievementProgressArray(IntPtr obj);

		// Token: 0x060034C3 RID: 13507
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetAchievementUpdate(IntPtr obj);

		// Token: 0x060034C4 RID: 13508
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetApplicationVersion(IntPtr obj);

		// Token: 0x060034C5 RID: 13509
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetAssetFileDeleteResult(IntPtr obj);

		// Token: 0x060034C6 RID: 13510
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetAssetFileDownloadCancelResult(IntPtr obj);

		// Token: 0x060034C7 RID: 13511
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetAssetFileDownloadResult(IntPtr obj);

		// Token: 0x060034C8 RID: 13512
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetAssetFileDownloadUpdate(IntPtr obj);

		// Token: 0x060034C9 RID: 13513
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetCloudStorageConflictMetadata(IntPtr obj);

		// Token: 0x060034CA RID: 13514
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetCloudStorageData(IntPtr obj);

		// Token: 0x060034CB RID: 13515
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetCloudStorageMetadata(IntPtr obj);

		// Token: 0x060034CC RID: 13516
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetCloudStorageMetadataArray(IntPtr obj);

		// Token: 0x060034CD RID: 13517
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetCloudStorageUpdateResponse(IntPtr obj);

		// Token: 0x060034CE RID: 13518
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetError(IntPtr obj);

		// Token: 0x060034CF RID: 13519
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetHttpTransferUpdate(IntPtr obj);

		// Token: 0x060034D0 RID: 13520
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetInstalledApplicationArray(IntPtr obj);

		// Token: 0x060034D1 RID: 13521
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetLeaderboardEntryArray(IntPtr obj);

		// Token: 0x060034D2 RID: 13522
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetLeaderboardUpdateStatus(IntPtr obj);

		// Token: 0x060034D3 RID: 13523
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetLivestreamingApplicationStatus(IntPtr obj);

		// Token: 0x060034D4 RID: 13524
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetLivestreamingStartResult(IntPtr obj);

		// Token: 0x060034D5 RID: 13525
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetLivestreamingStatus(IntPtr obj);

		// Token: 0x060034D6 RID: 13526
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetLivestreamingVideoStats(IntPtr obj);

		// Token: 0x060034D7 RID: 13527
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetMatchmakingAdminSnapshot(IntPtr obj);

		// Token: 0x060034D8 RID: 13528
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetMatchmakingBrowseResult(IntPtr obj);

		// Token: 0x060034D9 RID: 13529
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetMatchmakingEnqueueResult(IntPtr obj);

		// Token: 0x060034DA RID: 13530
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetMatchmakingEnqueueResultAndRoom(IntPtr obj);

		// Token: 0x060034DB RID: 13531
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetMatchmakingRoomArray(IntPtr obj);

		// Token: 0x060034DC RID: 13532
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetMatchmakingStats(IntPtr obj);

		// Token: 0x060034DD RID: 13533
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetNativeMessage(IntPtr obj);

		// Token: 0x060034DE RID: 13534
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetNetworkingPeer(IntPtr obj);

		// Token: 0x060034DF RID: 13535
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetOrgScopedID(IntPtr obj);

		// Token: 0x060034E0 RID: 13536
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetParty(IntPtr obj);

		// Token: 0x060034E1 RID: 13537
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetPartyID(IntPtr obj);

		// Token: 0x060034E2 RID: 13538
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetPidArray(IntPtr obj);

		// Token: 0x060034E3 RID: 13539
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetPingResult(IntPtr obj);

		// Token: 0x060034E4 RID: 13540
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetPlatformInitialize(IntPtr obj);

		// Token: 0x060034E5 RID: 13541
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetProductArray(IntPtr obj);

		// Token: 0x060034E6 RID: 13542
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetPurchase(IntPtr obj);

		// Token: 0x060034E7 RID: 13543
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetPurchaseArray(IntPtr obj);

		// Token: 0x060034E8 RID: 13544
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Message_GetRequestID(IntPtr obj);

		// Token: 0x060034E9 RID: 13545
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetRoom(IntPtr obj);

		// Token: 0x060034EA RID: 13546
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetRoomArray(IntPtr obj);

		// Token: 0x060034EB RID: 13547
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetRoomInviteNotification(IntPtr obj);

		// Token: 0x060034EC RID: 13548
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetRoomInviteNotificationArray(IntPtr obj);

		// Token: 0x060034ED RID: 13549
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetSdkAccountArray(IntPtr obj);

		// Token: 0x060034EE RID: 13550
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetShareMediaResult(IntPtr obj);

		// Token: 0x060034EF RID: 13551 RVA: 0x0010A404 File Offset: 0x00108804
		public static string ovr_Message_GetString(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_Message_GetString_Native(obj));
		}

		// Token: 0x060034F0 RID: 13552
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_Message_GetString")]
		private static extern IntPtr ovr_Message_GetString_Native(IntPtr obj);

		// Token: 0x060034F1 RID: 13553
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetSystemPermission(IntPtr obj);

		// Token: 0x060034F2 RID: 13554
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetSystemVoipState(IntPtr obj);

		// Token: 0x060034F3 RID: 13555
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern Message.MessageType ovr_Message_GetType(IntPtr obj);

		// Token: 0x060034F4 RID: 13556
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetUser(IntPtr obj);

		// Token: 0x060034F5 RID: 13557
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetUserAndRoomArray(IntPtr obj);

		// Token: 0x060034F6 RID: 13558
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetUserArray(IntPtr obj);

		// Token: 0x060034F7 RID: 13559
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetUserProof(IntPtr obj);

		// Token: 0x060034F8 RID: 13560
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Message_GetUserReportID(IntPtr obj);

		// Token: 0x060034F9 RID: 13561
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool ovr_Message_IsError(IntPtr obj);

		// Token: 0x060034FA RID: 13562
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr ovr_Microphone_GetNumSamplesAvailable(IntPtr obj);

		// Token: 0x060034FB RID: 13563
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr ovr_Microphone_GetOutputBufferMaxSize(IntPtr obj);

		// Token: 0x060034FC RID: 13564
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr ovr_Microphone_GetPCM(IntPtr obj, short[] outputBuffer, UIntPtr outputBufferNumElements);

		// Token: 0x060034FD RID: 13565
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr ovr_Microphone_GetPCMFloat(IntPtr obj, float[] outputBuffer, UIntPtr outputBufferNumElements);

		// Token: 0x060034FE RID: 13566
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr ovr_Microphone_ReadData(IntPtr obj, float[] outputBuffer, UIntPtr outputBufferSize);

		// Token: 0x060034FF RID: 13567
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_Microphone_SetAcceptableRecordingDelayHint(IntPtr obj, UIntPtr delayMs);

		// Token: 0x06003500 RID: 13568
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_Microphone_Start(IntPtr obj);

		// Token: 0x06003501 RID: 13569
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_Microphone_Stop(IntPtr obj);

		// Token: 0x06003502 RID: 13570
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_NetworkingPeer_GetID(IntPtr obj);

		// Token: 0x06003503 RID: 13571
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern PeerConnectionState ovr_NetworkingPeer_GetState(IntPtr obj);

		// Token: 0x06003504 RID: 13572
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_OrgScopedID_GetID(IntPtr obj);

		// Token: 0x06003505 RID: 13573
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_Packet_Free(IntPtr obj);

		// Token: 0x06003506 RID: 13574
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Packet_GetBytes(IntPtr obj);

		// Token: 0x06003507 RID: 13575
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern SendPolicy ovr_Packet_GetSendPolicy(IntPtr obj);

		// Token: 0x06003508 RID: 13576
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Packet_GetSenderID(IntPtr obj);

		// Token: 0x06003509 RID: 13577
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr ovr_Packet_GetSize(IntPtr obj);

		// Token: 0x0600350A RID: 13578
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Party_GetID(IntPtr obj);

		// Token: 0x0600350B RID: 13579
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Party_GetInvitedUsers(IntPtr obj);

		// Token: 0x0600350C RID: 13580
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Party_GetLeader(IntPtr obj);

		// Token: 0x0600350D RID: 13581
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Party_GetRoom(IntPtr obj);

		// Token: 0x0600350E RID: 13582
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Party_GetUsers(IntPtr obj);

		// Token: 0x0600350F RID: 13583
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_PartyID_GetID(IntPtr obj);

		// Token: 0x06003510 RID: 13584 RVA: 0x0010A420 File Offset: 0x00108820
		public static string ovr_Pid_GetId(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_Pid_GetId_Native(obj));
		}

		// Token: 0x06003511 RID: 13585
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_Pid_GetId")]
		private static extern IntPtr ovr_Pid_GetId_Native(IntPtr obj);

		// Token: 0x06003512 RID: 13586
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_PidArray_GetElement(IntPtr obj, UIntPtr index);

		// Token: 0x06003513 RID: 13587
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr ovr_PidArray_GetSize(IntPtr obj);

		// Token: 0x06003514 RID: 13588
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_PingResult_GetID(IntPtr obj);

		// Token: 0x06003515 RID: 13589
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_PingResult_GetPingTimeUsec(IntPtr obj);

		// Token: 0x06003516 RID: 13590
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool ovr_PingResult_IsTimeout(IntPtr obj);

		// Token: 0x06003517 RID: 13591
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern PlatformInitializeResult ovr_PlatformInitialize_GetResult(IntPtr obj);

		// Token: 0x06003518 RID: 13592
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ovr_Price_GetAmountInHundredths(IntPtr obj);

		// Token: 0x06003519 RID: 13593 RVA: 0x0010A43C File Offset: 0x0010883C
		public static string ovr_Price_GetCurrency(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_Price_GetCurrency_Native(obj));
		}

		// Token: 0x0600351A RID: 13594
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_Price_GetCurrency")]
		private static extern IntPtr ovr_Price_GetCurrency_Native(IntPtr obj);

		// Token: 0x0600351B RID: 13595 RVA: 0x0010A458 File Offset: 0x00108858
		public static string ovr_Price_GetFormatted(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_Price_GetFormatted_Native(obj));
		}

		// Token: 0x0600351C RID: 13596
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_Price_GetFormatted")]
		private static extern IntPtr ovr_Price_GetFormatted_Native(IntPtr obj);

		// Token: 0x0600351D RID: 13597 RVA: 0x0010A474 File Offset: 0x00108874
		public static string ovr_Product_GetDescription(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_Product_GetDescription_Native(obj));
		}

		// Token: 0x0600351E RID: 13598
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_Product_GetDescription")]
		private static extern IntPtr ovr_Product_GetDescription_Native(IntPtr obj);

		// Token: 0x0600351F RID: 13599 RVA: 0x0010A490 File Offset: 0x00108890
		public static string ovr_Product_GetFormattedPrice(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_Product_GetFormattedPrice_Native(obj));
		}

		// Token: 0x06003520 RID: 13600
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_Product_GetFormattedPrice")]
		private static extern IntPtr ovr_Product_GetFormattedPrice_Native(IntPtr obj);

		// Token: 0x06003521 RID: 13601 RVA: 0x0010A4AC File Offset: 0x001088AC
		public static string ovr_Product_GetName(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_Product_GetName_Native(obj));
		}

		// Token: 0x06003522 RID: 13602
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_Product_GetName")]
		private static extern IntPtr ovr_Product_GetName_Native(IntPtr obj);

		// Token: 0x06003523 RID: 13603 RVA: 0x0010A4C8 File Offset: 0x001088C8
		public static string ovr_Product_GetSKU(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_Product_GetSKU_Native(obj));
		}

		// Token: 0x06003524 RID: 13604
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_Product_GetSKU")]
		private static extern IntPtr ovr_Product_GetSKU_Native(IntPtr obj);

		// Token: 0x06003525 RID: 13605
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_ProductArray_GetElement(IntPtr obj, UIntPtr index);

		// Token: 0x06003526 RID: 13606 RVA: 0x0010A4E4 File Offset: 0x001088E4
		public static string ovr_ProductArray_GetNextUrl(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_ProductArray_GetNextUrl_Native(obj));
		}

		// Token: 0x06003527 RID: 13607
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_ProductArray_GetNextUrl")]
		private static extern IntPtr ovr_ProductArray_GetNextUrl_Native(IntPtr obj);

		// Token: 0x06003528 RID: 13608
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr ovr_ProductArray_GetSize(IntPtr obj);

		// Token: 0x06003529 RID: 13609
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool ovr_ProductArray_HasNextPage(IntPtr obj);

		// Token: 0x0600352A RID: 13610 RVA: 0x0010A500 File Offset: 0x00108900
		public static DateTime ovr_Purchase_GetExpirationTime(IntPtr obj)
		{
			return CAPI.DateTimeFromNative(CAPI.ovr_Purchase_GetExpirationTime_Native(obj));
		}

		// Token: 0x0600352B RID: 13611
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_Purchase_GetExpirationTime")]
		private static extern ulong ovr_Purchase_GetExpirationTime_Native(IntPtr obj);

		// Token: 0x0600352C RID: 13612 RVA: 0x0010A51C File Offset: 0x0010891C
		public static DateTime ovr_Purchase_GetGrantTime(IntPtr obj)
		{
			return CAPI.DateTimeFromNative(CAPI.ovr_Purchase_GetGrantTime_Native(obj));
		}

		// Token: 0x0600352D RID: 13613
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_Purchase_GetGrantTime")]
		private static extern ulong ovr_Purchase_GetGrantTime_Native(IntPtr obj);

		// Token: 0x0600352E RID: 13614
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Purchase_GetPurchaseID(IntPtr obj);

		// Token: 0x0600352F RID: 13615 RVA: 0x0010A538 File Offset: 0x00108938
		public static string ovr_Purchase_GetSKU(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_Purchase_GetSKU_Native(obj));
		}

		// Token: 0x06003530 RID: 13616
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_Purchase_GetSKU")]
		private static extern IntPtr ovr_Purchase_GetSKU_Native(IntPtr obj);

		// Token: 0x06003531 RID: 13617
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_PurchaseArray_GetElement(IntPtr obj, UIntPtr index);

		// Token: 0x06003532 RID: 13618 RVA: 0x0010A554 File Offset: 0x00108954
		public static string ovr_PurchaseArray_GetNextUrl(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_PurchaseArray_GetNextUrl_Native(obj));
		}

		// Token: 0x06003533 RID: 13619
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_PurchaseArray_GetNextUrl")]
		private static extern IntPtr ovr_PurchaseArray_GetNextUrl_Native(IntPtr obj);

		// Token: 0x06003534 RID: 13620
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr ovr_PurchaseArray_GetSize(IntPtr obj);

		// Token: 0x06003535 RID: 13621
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool ovr_PurchaseArray_HasNextPage(IntPtr obj);

		// Token: 0x06003536 RID: 13622
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Room_GetApplicationID(IntPtr obj);

		// Token: 0x06003537 RID: 13623
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Room_GetDataStore(IntPtr obj);

		// Token: 0x06003538 RID: 13624 RVA: 0x0010A570 File Offset: 0x00108970
		public static string ovr_Room_GetDescription(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_Room_GetDescription_Native(obj));
		}

		// Token: 0x06003539 RID: 13625
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_Room_GetDescription")]
		private static extern IntPtr ovr_Room_GetDescription_Native(IntPtr obj);

		// Token: 0x0600353A RID: 13626
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_Room_GetID(IntPtr obj);

		// Token: 0x0600353B RID: 13627
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Room_GetInvitedUsers(IntPtr obj);

		// Token: 0x0600353C RID: 13628
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool ovr_Room_GetIsMembershipLocked(IntPtr obj);

		// Token: 0x0600353D RID: 13629
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern RoomJoinPolicy ovr_Room_GetJoinPolicy(IntPtr obj);

		// Token: 0x0600353E RID: 13630
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern RoomJoinability ovr_Room_GetJoinability(IntPtr obj);

		// Token: 0x0600353F RID: 13631
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Room_GetMatchedUsers(IntPtr obj);

		// Token: 0x06003540 RID: 13632
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ovr_Room_GetMaxUsers(IntPtr obj);

		// Token: 0x06003541 RID: 13633 RVA: 0x0010A58C File Offset: 0x0010898C
		public static string ovr_Room_GetName(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_Room_GetName_Native(obj));
		}

		// Token: 0x06003542 RID: 13634
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_Room_GetName")]
		private static extern IntPtr ovr_Room_GetName_Native(IntPtr obj);

		// Token: 0x06003543 RID: 13635
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Room_GetOwner(IntPtr obj);

		// Token: 0x06003544 RID: 13636
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern RoomType ovr_Room_GetType(IntPtr obj);

		// Token: 0x06003545 RID: 13637
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_Room_GetUsers(IntPtr obj);

		// Token: 0x06003546 RID: 13638
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ovr_Room_GetVersion(IntPtr obj);

		// Token: 0x06003547 RID: 13639
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_RoomArray_GetElement(IntPtr obj, UIntPtr index);

		// Token: 0x06003548 RID: 13640 RVA: 0x0010A5A8 File Offset: 0x001089A8
		public static string ovr_RoomArray_GetNextUrl(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_RoomArray_GetNextUrl_Native(obj));
		}

		// Token: 0x06003549 RID: 13641
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_RoomArray_GetNextUrl")]
		private static extern IntPtr ovr_RoomArray_GetNextUrl_Native(IntPtr obj);

		// Token: 0x0600354A RID: 13642
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr ovr_RoomArray_GetSize(IntPtr obj);

		// Token: 0x0600354B RID: 13643
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool ovr_RoomArray_HasNextPage(IntPtr obj);

		// Token: 0x0600354C RID: 13644
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_RoomInviteNotification_GetID(IntPtr obj);

		// Token: 0x0600354D RID: 13645
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_RoomInviteNotification_GetRoomID(IntPtr obj);

		// Token: 0x0600354E RID: 13646 RVA: 0x0010A5C4 File Offset: 0x001089C4
		public static DateTime ovr_RoomInviteNotification_GetSentTime(IntPtr obj)
		{
			return CAPI.DateTimeFromNative(CAPI.ovr_RoomInviteNotification_GetSentTime_Native(obj));
		}

		// Token: 0x0600354F RID: 13647
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_RoomInviteNotification_GetSentTime")]
		private static extern ulong ovr_RoomInviteNotification_GetSentTime_Native(IntPtr obj);

		// Token: 0x06003550 RID: 13648
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_RoomInviteNotificationArray_GetElement(IntPtr obj, UIntPtr index);

		// Token: 0x06003551 RID: 13649 RVA: 0x0010A5E0 File Offset: 0x001089E0
		public static string ovr_RoomInviteNotificationArray_GetNextUrl(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_RoomInviteNotificationArray_GetNextUrl_Native(obj));
		}

		// Token: 0x06003552 RID: 13650
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_RoomInviteNotificationArray_GetNextUrl")]
		private static extern IntPtr ovr_RoomInviteNotificationArray_GetNextUrl_Native(IntPtr obj);

		// Token: 0x06003553 RID: 13651
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr ovr_RoomInviteNotificationArray_GetSize(IntPtr obj);

		// Token: 0x06003554 RID: 13652
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool ovr_RoomInviteNotificationArray_HasNextPage(IntPtr obj);

		// Token: 0x06003555 RID: 13653
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern SdkAccountType ovr_SdkAccount_GetAccountType(IntPtr obj);

		// Token: 0x06003556 RID: 13654
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_SdkAccount_GetUserId(IntPtr obj);

		// Token: 0x06003557 RID: 13655
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_SdkAccountArray_GetElement(IntPtr obj, UIntPtr index);

		// Token: 0x06003558 RID: 13656
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr ovr_SdkAccountArray_GetSize(IntPtr obj);

		// Token: 0x06003559 RID: 13657
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ShareMediaStatus ovr_ShareMediaResult_GetStatus(IntPtr obj);

		// Token: 0x0600355A RID: 13658
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool ovr_SystemPermission_GetHasPermission(IntPtr obj);

		// Token: 0x0600355B RID: 13659
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern PermissionGrantStatus ovr_SystemPermission_GetPermissionGrantStatus(IntPtr obj);

		// Token: 0x0600355C RID: 13660
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern VoipMuteState ovr_SystemVoipState_GetMicrophoneMuted(IntPtr obj);

		// Token: 0x0600355D RID: 13661
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern SystemVoipStatus ovr_SystemVoipState_GetStatus(IntPtr obj);

		// Token: 0x0600355E RID: 13662 RVA: 0x0010A5FC File Offset: 0x001089FC
		public static string ovr_TestUser_GetAccessToken(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_TestUser_GetAccessToken_Native(obj));
		}

		// Token: 0x0600355F RID: 13663
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_TestUser_GetAccessToken")]
		private static extern IntPtr ovr_TestUser_GetAccessToken_Native(IntPtr obj);

		// Token: 0x06003560 RID: 13664
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_TestUser_GetAppAccessArray(IntPtr obj);

		// Token: 0x06003561 RID: 13665
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_TestUser_GetFbAppAccessArray(IntPtr obj);

		// Token: 0x06003562 RID: 13666 RVA: 0x0010A618 File Offset: 0x00108A18
		public static string ovr_TestUser_GetFriendAccessToken(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_TestUser_GetFriendAccessToken_Native(obj));
		}

		// Token: 0x06003563 RID: 13667
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_TestUser_GetFriendAccessToken")]
		private static extern IntPtr ovr_TestUser_GetFriendAccessToken_Native(IntPtr obj);

		// Token: 0x06003564 RID: 13668
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_TestUser_GetFriendAppAccessArray(IntPtr obj);

		// Token: 0x06003565 RID: 13669 RVA: 0x0010A634 File Offset: 0x00108A34
		public static string ovr_TestUser_GetUserAlias(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_TestUser_GetUserAlias_Native(obj));
		}

		// Token: 0x06003566 RID: 13670
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_TestUser_GetUserAlias")]
		private static extern IntPtr ovr_TestUser_GetUserAlias_Native(IntPtr obj);

		// Token: 0x06003567 RID: 13671
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_TestUser_GetUserId(IntPtr obj);

		// Token: 0x06003568 RID: 13672 RVA: 0x0010A650 File Offset: 0x00108A50
		public static string ovr_TestUserAppAccess_GetAccessToken(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_TestUserAppAccess_GetAccessToken_Native(obj));
		}

		// Token: 0x06003569 RID: 13673
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_TestUserAppAccess_GetAccessToken")]
		private static extern IntPtr ovr_TestUserAppAccess_GetAccessToken_Native(IntPtr obj);

		// Token: 0x0600356A RID: 13674
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_TestUserAppAccess_GetAppId(IntPtr obj);

		// Token: 0x0600356B RID: 13675
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_TestUserAppAccess_GetUserId(IntPtr obj);

		// Token: 0x0600356C RID: 13676
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_TestUserAppAccessArray_GetElement(IntPtr obj, UIntPtr index);

		// Token: 0x0600356D RID: 13677
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr ovr_TestUserAppAccessArray_GetSize(IntPtr obj);

		// Token: 0x0600356E RID: 13678
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_User_GetID(IntPtr obj);

		// Token: 0x0600356F RID: 13679 RVA: 0x0010A66C File Offset: 0x00108A6C
		public static string ovr_User_GetImageUrl(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_User_GetImageUrl_Native(obj));
		}

		// Token: 0x06003570 RID: 13680
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_User_GetImageUrl")]
		private static extern IntPtr ovr_User_GetImageUrl_Native(IntPtr obj);

		// Token: 0x06003571 RID: 13681 RVA: 0x0010A688 File Offset: 0x00108A88
		public static string ovr_User_GetInviteToken(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_User_GetInviteToken_Native(obj));
		}

		// Token: 0x06003572 RID: 13682
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_User_GetInviteToken")]
		private static extern IntPtr ovr_User_GetInviteToken_Native(IntPtr obj);

		// Token: 0x06003573 RID: 13683 RVA: 0x0010A6A4 File Offset: 0x00108AA4
		public static string ovr_User_GetOculusID(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_User_GetOculusID_Native(obj));
		}

		// Token: 0x06003574 RID: 13684
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_User_GetOculusID")]
		private static extern IntPtr ovr_User_GetOculusID_Native(IntPtr obj);

		// Token: 0x06003575 RID: 13685 RVA: 0x0010A6C0 File Offset: 0x00108AC0
		public static string ovr_User_GetPresence(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_User_GetPresence_Native(obj));
		}

		// Token: 0x06003576 RID: 13686
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_User_GetPresence")]
		private static extern IntPtr ovr_User_GetPresence_Native(IntPtr obj);

		// Token: 0x06003577 RID: 13687
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern UserPresenceStatus ovr_User_GetPresenceStatus(IntPtr obj);

		// Token: 0x06003578 RID: 13688 RVA: 0x0010A6DC File Offset: 0x00108ADC
		public static string ovr_User_GetSmallImageUrl(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_User_GetSmallImageUrl_Native(obj));
		}

		// Token: 0x06003579 RID: 13689
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_User_GetSmallImageUrl")]
		private static extern IntPtr ovr_User_GetSmallImageUrl_Native(IntPtr obj);

		// Token: 0x0600357A RID: 13690
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_UserAndRoom_GetRoom(IntPtr obj);

		// Token: 0x0600357B RID: 13691
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_UserAndRoom_GetUser(IntPtr obj);

		// Token: 0x0600357C RID: 13692
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_UserAndRoomArray_GetElement(IntPtr obj, UIntPtr index);

		// Token: 0x0600357D RID: 13693 RVA: 0x0010A6F8 File Offset: 0x00108AF8
		public static string ovr_UserAndRoomArray_GetNextUrl(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_UserAndRoomArray_GetNextUrl_Native(obj));
		}

		// Token: 0x0600357E RID: 13694
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_UserAndRoomArray_GetNextUrl")]
		private static extern IntPtr ovr_UserAndRoomArray_GetNextUrl_Native(IntPtr obj);

		// Token: 0x0600357F RID: 13695
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr ovr_UserAndRoomArray_GetSize(IntPtr obj);

		// Token: 0x06003580 RID: 13696
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool ovr_UserAndRoomArray_HasNextPage(IntPtr obj);

		// Token: 0x06003581 RID: 13697
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_UserArray_GetElement(IntPtr obj, UIntPtr index);

		// Token: 0x06003582 RID: 13698 RVA: 0x0010A714 File Offset: 0x00108B14
		public static string ovr_UserArray_GetNextUrl(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_UserArray_GetNextUrl_Native(obj));
		}

		// Token: 0x06003583 RID: 13699
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_UserArray_GetNextUrl")]
		private static extern IntPtr ovr_UserArray_GetNextUrl_Native(IntPtr obj);

		// Token: 0x06003584 RID: 13700
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr ovr_UserArray_GetSize(IntPtr obj);

		// Token: 0x06003585 RID: 13701
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool ovr_UserArray_HasNextPage(IntPtr obj);

		// Token: 0x06003586 RID: 13702 RVA: 0x0010A730 File Offset: 0x00108B30
		public static string ovr_UserProof_GetNonce(IntPtr obj)
		{
			return CAPI.StringFromNative(CAPI.ovr_UserProof_GetNonce_Native(obj));
		}

		// Token: 0x06003587 RID: 13703
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_UserProof_GetNonce")]
		private static extern IntPtr ovr_UserProof_GetNonce_Native(IntPtr obj);

		// Token: 0x06003588 RID: 13704
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ovr_UserReportID_GetID(IntPtr obj);

		// Token: 0x06003589 RID: 13705
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_VoipDecoder_Decode(IntPtr obj, byte[] compressedData, UIntPtr compressedSize);

		// Token: 0x0600358A RID: 13706
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr ovr_VoipDecoder_GetDecodedPCM(IntPtr obj, float[] outputBuffer, UIntPtr outputBufferSize);

		// Token: 0x0600358B RID: 13707
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_VoipEncoder_AddPCM(IntPtr obj, float[] inputData, uint inputSize);

		// Token: 0x0600358C RID: 13708
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr ovr_VoipEncoder_GetCompressedData(IntPtr obj, byte[] outputBuffer, UIntPtr intputSize);

		// Token: 0x0600358D RID: 13709
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern UIntPtr ovr_VoipEncoder_GetCompressedDataSize(IntPtr obj);

		// Token: 0x0600358E RID: 13710
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_ApplicationOptions_Create();

		// Token: 0x0600358F RID: 13711
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_ApplicationOptions_Destroy(IntPtr handle);

		// Token: 0x06003590 RID: 13712 RVA: 0x0010A74C File Offset: 0x00108B4C
		public static void ovr_ApplicationOptions_SetDeeplinkMessage(IntPtr handle, string value)
		{
			IntPtr intPtr = CAPI.StringToNative(value);
			CAPI.ovr_ApplicationOptions_SetDeeplinkMessage_Native(handle, intPtr);
			Marshal.FreeCoTaskMem(intPtr);
		}

		// Token: 0x06003591 RID: 13713
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_ApplicationOptions_SetDeeplinkMessage")]
		private static extern void ovr_ApplicationOptions_SetDeeplinkMessage_Native(IntPtr handle, IntPtr value);

		// Token: 0x06003592 RID: 13714
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_MatchmakingOptions_Create();

		// Token: 0x06003593 RID: 13715
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_MatchmakingOptions_Destroy(IntPtr handle);

		// Token: 0x06003594 RID: 13716 RVA: 0x0010A770 File Offset: 0x00108B70
		public static void ovr_MatchmakingOptions_SetCreateRoomDataStoreString(IntPtr handle, string key, string value)
		{
			IntPtr intPtr = CAPI.StringToNative(key);
			IntPtr intPtr2 = CAPI.StringToNative(value);
			CAPI.ovr_MatchmakingOptions_SetCreateRoomDataStoreString_Native(handle, intPtr, intPtr2);
			Marshal.FreeCoTaskMem(intPtr);
			Marshal.FreeCoTaskMem(intPtr2);
		}

		// Token: 0x06003595 RID: 13717
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_MatchmakingOptions_SetCreateRoomDataStoreString")]
		private static extern void ovr_MatchmakingOptions_SetCreateRoomDataStoreString_Native(IntPtr handle, IntPtr key, IntPtr value);

		// Token: 0x06003596 RID: 13718
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_MatchmakingOptions_ClearCreateRoomDataStore(IntPtr handle);

		// Token: 0x06003597 RID: 13719
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_MatchmakingOptions_SetCreateRoomJoinPolicy(IntPtr handle, RoomJoinPolicy value);

		// Token: 0x06003598 RID: 13720
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_MatchmakingOptions_SetCreateRoomMaxUsers(IntPtr handle, uint value);

		// Token: 0x06003599 RID: 13721
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_MatchmakingOptions_AddEnqueueAdditionalUser(IntPtr handle, ulong value);

		// Token: 0x0600359A RID: 13722
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_MatchmakingOptions_ClearEnqueueAdditionalUsers(IntPtr handle);

		// Token: 0x0600359B RID: 13723 RVA: 0x0010A7A0 File Offset: 0x00108BA0
		public static void ovr_MatchmakingOptions_SetEnqueueDataSettingsInt(IntPtr handle, string key, int value)
		{
			IntPtr intPtr = CAPI.StringToNative(key);
			CAPI.ovr_MatchmakingOptions_SetEnqueueDataSettingsInt_Native(handle, intPtr, value);
			Marshal.FreeCoTaskMem(intPtr);
		}

		// Token: 0x0600359C RID: 13724
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_MatchmakingOptions_SetEnqueueDataSettingsInt")]
		private static extern void ovr_MatchmakingOptions_SetEnqueueDataSettingsInt_Native(IntPtr handle, IntPtr key, int value);

		// Token: 0x0600359D RID: 13725 RVA: 0x0010A7C4 File Offset: 0x00108BC4
		public static void ovr_MatchmakingOptions_SetEnqueueDataSettingsDouble(IntPtr handle, string key, double value)
		{
			IntPtr intPtr = CAPI.StringToNative(key);
			CAPI.ovr_MatchmakingOptions_SetEnqueueDataSettingsDouble_Native(handle, intPtr, value);
			Marshal.FreeCoTaskMem(intPtr);
		}

		// Token: 0x0600359E RID: 13726
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_MatchmakingOptions_SetEnqueueDataSettingsDouble")]
		private static extern void ovr_MatchmakingOptions_SetEnqueueDataSettingsDouble_Native(IntPtr handle, IntPtr key, double value);

		// Token: 0x0600359F RID: 13727 RVA: 0x0010A7E8 File Offset: 0x00108BE8
		public static void ovr_MatchmakingOptions_SetEnqueueDataSettingsString(IntPtr handle, string key, string value)
		{
			IntPtr intPtr = CAPI.StringToNative(key);
			IntPtr intPtr2 = CAPI.StringToNative(value);
			CAPI.ovr_MatchmakingOptions_SetEnqueueDataSettingsString_Native(handle, intPtr, intPtr2);
			Marshal.FreeCoTaskMem(intPtr);
			Marshal.FreeCoTaskMem(intPtr2);
		}

		// Token: 0x060035A0 RID: 13728
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_MatchmakingOptions_SetEnqueueDataSettingsString")]
		private static extern void ovr_MatchmakingOptions_SetEnqueueDataSettingsString_Native(IntPtr handle, IntPtr key, IntPtr value);

		// Token: 0x060035A1 RID: 13729
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_MatchmakingOptions_ClearEnqueueDataSettings(IntPtr handle);

		// Token: 0x060035A2 RID: 13730
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_MatchmakingOptions_SetEnqueueIsDebug(IntPtr handle, bool value);

		// Token: 0x060035A3 RID: 13731 RVA: 0x0010A818 File Offset: 0x00108C18
		public static void ovr_MatchmakingOptions_SetEnqueueQueryKey(IntPtr handle, string value)
		{
			IntPtr intPtr = CAPI.StringToNative(value);
			CAPI.ovr_MatchmakingOptions_SetEnqueueQueryKey_Native(handle, intPtr);
			Marshal.FreeCoTaskMem(intPtr);
		}

		// Token: 0x060035A4 RID: 13732
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_MatchmakingOptions_SetEnqueueQueryKey")]
		private static extern void ovr_MatchmakingOptions_SetEnqueueQueryKey_Native(IntPtr handle, IntPtr value);

		// Token: 0x060035A5 RID: 13733
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_RoomOptions_Create();

		// Token: 0x060035A6 RID: 13734
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_RoomOptions_Destroy(IntPtr handle);

		// Token: 0x060035A7 RID: 13735 RVA: 0x0010A83C File Offset: 0x00108C3C
		public static void ovr_RoomOptions_SetDataStoreString(IntPtr handle, string key, string value)
		{
			IntPtr intPtr = CAPI.StringToNative(key);
			IntPtr intPtr2 = CAPI.StringToNative(value);
			CAPI.ovr_RoomOptions_SetDataStoreString_Native(handle, intPtr, intPtr2);
			Marshal.FreeCoTaskMem(intPtr);
			Marshal.FreeCoTaskMem(intPtr2);
		}

		// Token: 0x060035A8 RID: 13736
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ovr_RoomOptions_SetDataStoreString")]
		private static extern void ovr_RoomOptions_SetDataStoreString_Native(IntPtr handle, IntPtr key, IntPtr value);

		// Token: 0x060035A9 RID: 13737
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_RoomOptions_ClearDataStore(IntPtr handle);

		// Token: 0x060035AA RID: 13738
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_RoomOptions_SetExcludeRecentlyMet(IntPtr handle, bool value);

		// Token: 0x060035AB RID: 13739
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_RoomOptions_SetMaxUserResults(IntPtr handle, uint value);

		// Token: 0x060035AC RID: 13740
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_RoomOptions_SetOrdering(IntPtr handle, UserOrdering value);

		// Token: 0x060035AD RID: 13741
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_RoomOptions_SetRecentlyMetTimeWindow(IntPtr handle, TimeWindow value);

		// Token: 0x060035AE RID: 13742
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_RoomOptions_SetRoomId(IntPtr handle, ulong value);

		// Token: 0x060035AF RID: 13743
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_RoomOptions_SetTurnOffUpdates(IntPtr handle, bool value);

		// Token: 0x060035B0 RID: 13744
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ovr_UserOptions_Create();

		// Token: 0x060035B1 RID: 13745
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_UserOptions_Destroy(IntPtr handle);

		// Token: 0x060035B2 RID: 13746
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_UserOptions_SetMaxUsers(IntPtr handle, uint value);

		// Token: 0x060035B3 RID: 13747
		[DllImport("LibOVRPlatform64_1", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ovr_UserOptions_SetTimeWindow(IntPtr handle, TimeWindow value);

		// Token: 0x060035B4 RID: 13748 RVA: 0x0010A86B File Offset: 0x00108C6B
		// Note: this type is marked as 'beforefieldinit'.
		static CAPI()
		{
		}

		// Token: 0x04002711 RID: 10001
		public const string DLL_NAME = "LibOVRPlatform64_1";

		// Token: 0x04002712 RID: 10002
		private static UTF8Encoding nativeStringEncoding = new UTF8Encoding(false);

		// Token: 0x04002713 RID: 10003
		public const int VoipFilterBufferSize = 480;

		// Token: 0x020007E8 RID: 2024
		public struct ovrKeyValuePair
		{
			// Token: 0x060035B5 RID: 13749 RVA: 0x0010A878 File Offset: 0x00108C78
			public ovrKeyValuePair(string key, string value)
			{
				this.key_ = key;
				this.valueType_ = KeyValuePairType.String;
				this.stringValue_ = value;
				this.intValue_ = 0;
				this.doubleValue_ = 0.0;
			}

			// Token: 0x060035B6 RID: 13750 RVA: 0x0010A8A5 File Offset: 0x00108CA5
			public ovrKeyValuePair(string key, int value)
			{
				this.key_ = key;
				this.valueType_ = KeyValuePairType.Int;
				this.intValue_ = value;
				this.stringValue_ = null;
				this.doubleValue_ = 0.0;
			}

			// Token: 0x060035B7 RID: 13751 RVA: 0x0010A8D2 File Offset: 0x00108CD2
			public ovrKeyValuePair(string key, double value)
			{
				this.key_ = key;
				this.valueType_ = KeyValuePairType.Double;
				this.doubleValue_ = value;
				this.stringValue_ = null;
				this.intValue_ = 0;
			}

			// Token: 0x04002714 RID: 10004
			public string key_;

			// Token: 0x04002715 RID: 10005
			private KeyValuePairType valueType_;

			// Token: 0x04002716 RID: 10006
			public string stringValue_;

			// Token: 0x04002717 RID: 10007
			public int intValue_;

			// Token: 0x04002718 RID: 10008
			public double doubleValue_;
		}

		// Token: 0x020007E9 RID: 2025
		public struct ovrMatchmakingCriterion
		{
			// Token: 0x060035B8 RID: 13752 RVA: 0x0010A8F7 File Offset: 0x00108CF7
			public ovrMatchmakingCriterion(string key, MatchmakingCriterionImportance importance)
			{
				this.key_ = key;
				this.importance_ = importance;
				this.parameterArray = IntPtr.Zero;
				this.parameterArrayCount = 0U;
			}

			// Token: 0x04002719 RID: 10009
			public string key_;

			// Token: 0x0400271A RID: 10010
			public MatchmakingCriterionImportance importance_;

			// Token: 0x0400271B RID: 10011
			public IntPtr parameterArray;

			// Token: 0x0400271C RID: 10012
			public uint parameterArrayCount;
		}

		// Token: 0x020007EA RID: 2026
		public struct ovrMatchmakingCustomQueryData
		{
			// Token: 0x0400271D RID: 10013
			public IntPtr dataArray;

			// Token: 0x0400271E RID: 10014
			public uint dataArrayCount;

			// Token: 0x0400271F RID: 10015
			public IntPtr criterionArray;

			// Token: 0x04002720 RID: 10016
			public uint criterionArrayCount;
		}

		// Token: 0x020007EB RID: 2027
		public struct OculusInitParams
		{
			// Token: 0x04002721 RID: 10017
			public int sType;

			// Token: 0x04002722 RID: 10018
			public string email;

			// Token: 0x04002723 RID: 10019
			public string password;

			// Token: 0x04002724 RID: 10020
			public ulong appId;

			// Token: 0x04002725 RID: 10021
			public string uriPrefixOverride;
		}

		// Token: 0x020007EC RID: 2028
		// (Invoke) Token: 0x060035BA RID: 13754
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void FilterCallback([MarshalAs(UnmanagedType.LPArray, SizeConst = 480)] [In] [Out] short[] pcmData, UIntPtr pcmDataLength, int frequency, int numChannels);
	}
}
