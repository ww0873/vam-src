using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x02000819 RID: 2073
	public class MessageWithOrgScopedID : Message<OrgScopedID>
	{
		// Token: 0x06003663 RID: 13923 RVA: 0x0010B83D File Offset: 0x00109C3D
		public MessageWithOrgScopedID(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x06003664 RID: 13924 RVA: 0x0010B846 File Offset: 0x00109C46
		public override OrgScopedID GetOrgScopedID()
		{
			return base.Data;
		}

		// Token: 0x06003665 RID: 13925 RVA: 0x0010B850 File Offset: 0x00109C50
		protected override OrgScopedID GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr o = CAPI.ovr_Message_GetOrgScopedID(obj);
			return new OrgScopedID(o);
		}
	}
}
