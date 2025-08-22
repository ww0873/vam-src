using System;

namespace Oculus.Platform
{
	// Token: 0x020007F9 RID: 2041
	public class MatchmakingOptions
	{
		// Token: 0x060035C4 RID: 13764 RVA: 0x0010A919 File Offset: 0x00108D19
		public MatchmakingOptions()
		{
			this.Handle = CAPI.ovr_MatchmakingOptions_Create();
		}

		// Token: 0x060035C5 RID: 13765 RVA: 0x0010A92C File Offset: 0x00108D2C
		public void SetCreateRoomDataStore(string key, string value)
		{
			CAPI.ovr_MatchmakingOptions_SetCreateRoomDataStoreString(this.Handle, key, value);
		}

		// Token: 0x060035C6 RID: 13766 RVA: 0x0010A93B File Offset: 0x00108D3B
		public void ClearCreateRoomDataStore()
		{
			CAPI.ovr_MatchmakingOptions_ClearCreateRoomDataStore(this.Handle);
		}

		// Token: 0x060035C7 RID: 13767 RVA: 0x0010A948 File Offset: 0x00108D48
		public void SetCreateRoomJoinPolicy(RoomJoinPolicy value)
		{
			CAPI.ovr_MatchmakingOptions_SetCreateRoomJoinPolicy(this.Handle, value);
		}

		// Token: 0x060035C8 RID: 13768 RVA: 0x0010A956 File Offset: 0x00108D56
		public void SetCreateRoomMaxUsers(uint value)
		{
			CAPI.ovr_MatchmakingOptions_SetCreateRoomMaxUsers(this.Handle, value);
		}

		// Token: 0x060035C9 RID: 13769 RVA: 0x0010A964 File Offset: 0x00108D64
		public void AddEnqueueAdditionalUser(ulong userID)
		{
			CAPI.ovr_MatchmakingOptions_AddEnqueueAdditionalUser(this.Handle, userID);
		}

		// Token: 0x060035CA RID: 13770 RVA: 0x0010A972 File Offset: 0x00108D72
		public void ClearEnqueueAdditionalUsers()
		{
			CAPI.ovr_MatchmakingOptions_ClearEnqueueAdditionalUsers(this.Handle);
		}

		// Token: 0x060035CB RID: 13771 RVA: 0x0010A97F File Offset: 0x00108D7F
		public void SetEnqueueDataSettings(string key, int value)
		{
			CAPI.ovr_MatchmakingOptions_SetEnqueueDataSettingsInt(this.Handle, key, value);
		}

		// Token: 0x060035CC RID: 13772 RVA: 0x0010A98E File Offset: 0x00108D8E
		public void SetEnqueueDataSettings(string key, double value)
		{
			CAPI.ovr_MatchmakingOptions_SetEnqueueDataSettingsDouble(this.Handle, key, value);
		}

		// Token: 0x060035CD RID: 13773 RVA: 0x0010A99D File Offset: 0x00108D9D
		public void SetEnqueueDataSettings(string key, string value)
		{
			CAPI.ovr_MatchmakingOptions_SetEnqueueDataSettingsString(this.Handle, key, value);
		}

		// Token: 0x060035CE RID: 13774 RVA: 0x0010A9AC File Offset: 0x00108DAC
		public void ClearEnqueueDataSettings()
		{
			CAPI.ovr_MatchmakingOptions_ClearEnqueueDataSettings(this.Handle);
		}

		// Token: 0x060035CF RID: 13775 RVA: 0x0010A9B9 File Offset: 0x00108DB9
		public void SetEnqueueIsDebug(bool value)
		{
			CAPI.ovr_MatchmakingOptions_SetEnqueueIsDebug(this.Handle, value);
		}

		// Token: 0x060035D0 RID: 13776 RVA: 0x0010A9C7 File Offset: 0x00108DC7
		public void SetEnqueueQueryKey(string value)
		{
			CAPI.ovr_MatchmakingOptions_SetEnqueueQueryKey(this.Handle, value);
		}

		// Token: 0x060035D1 RID: 13777 RVA: 0x0010A9D5 File Offset: 0x00108DD5
		public static explicit operator IntPtr(MatchmakingOptions options)
		{
			return (options == null) ? IntPtr.Zero : options.Handle;
		}

		// Token: 0x060035D2 RID: 13778 RVA: 0x0010A9F0 File Offset: 0x00108DF0
		~MatchmakingOptions()
		{
			CAPI.ovr_MatchmakingOptions_Destroy(this.Handle);
		}

		// Token: 0x0400275D RID: 10077
		private IntPtr Handle;
	}
}
