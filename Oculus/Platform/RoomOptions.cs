using System;

namespace Oculus.Platform
{
	// Token: 0x0200089E RID: 2206
	public class RoomOptions
	{
		// Token: 0x060037C4 RID: 14276 RVA: 0x0010EA5C File Offset: 0x0010CE5C
		public RoomOptions()
		{
			this.Handle = CAPI.ovr_RoomOptions_Create();
		}

		// Token: 0x060037C5 RID: 14277 RVA: 0x0010EA6F File Offset: 0x0010CE6F
		public void SetDataStore(string key, string value)
		{
			CAPI.ovr_RoomOptions_SetDataStoreString(this.Handle, key, value);
		}

		// Token: 0x060037C6 RID: 14278 RVA: 0x0010EA7E File Offset: 0x0010CE7E
		public void ClearDataStore()
		{
			CAPI.ovr_RoomOptions_ClearDataStore(this.Handle);
		}

		// Token: 0x060037C7 RID: 14279 RVA: 0x0010EA8B File Offset: 0x0010CE8B
		public void SetExcludeRecentlyMet(bool value)
		{
			CAPI.ovr_RoomOptions_SetExcludeRecentlyMet(this.Handle, value);
		}

		// Token: 0x060037C8 RID: 14280 RVA: 0x0010EA99 File Offset: 0x0010CE99
		public void SetMaxUserResults(uint value)
		{
			CAPI.ovr_RoomOptions_SetMaxUserResults(this.Handle, value);
		}

		// Token: 0x060037C9 RID: 14281 RVA: 0x0010EAA7 File Offset: 0x0010CEA7
		public void SetOrdering(UserOrdering value)
		{
			CAPI.ovr_RoomOptions_SetOrdering(this.Handle, value);
		}

		// Token: 0x060037CA RID: 14282 RVA: 0x0010EAB5 File Offset: 0x0010CEB5
		public void SetRecentlyMetTimeWindow(TimeWindow value)
		{
			CAPI.ovr_RoomOptions_SetRecentlyMetTimeWindow(this.Handle, value);
		}

		// Token: 0x060037CB RID: 14283 RVA: 0x0010EAC3 File Offset: 0x0010CEC3
		public void SetRoomId(ulong value)
		{
			CAPI.ovr_RoomOptions_SetRoomId(this.Handle, value);
		}

		// Token: 0x060037CC RID: 14284 RVA: 0x0010EAD1 File Offset: 0x0010CED1
		public void SetTurnOffUpdates(bool value)
		{
			CAPI.ovr_RoomOptions_SetTurnOffUpdates(this.Handle, value);
		}

		// Token: 0x060037CD RID: 14285 RVA: 0x0010EADF File Offset: 0x0010CEDF
		public static explicit operator IntPtr(RoomOptions options)
		{
			return (options == null) ? IntPtr.Zero : options.Handle;
		}

		// Token: 0x060037CE RID: 14286 RVA: 0x0010EAF8 File Offset: 0x0010CEF8
		~RoomOptions()
		{
			CAPI.ovr_RoomOptions_Destroy(this.Handle);
		}

		// Token: 0x040028EA RID: 10474
		private IntPtr Handle;
	}
}
