using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x0200082A RID: 2090
	public class MessageWithSystemPermission : Message<SystemPermission>
	{
		// Token: 0x06003696 RID: 13974 RVA: 0x0010BB96 File Offset: 0x00109F96
		public MessageWithSystemPermission(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x06003697 RID: 13975 RVA: 0x0010BB9F File Offset: 0x00109F9F
		public override SystemPermission GetSystemPermission()
		{
			return base.Data;
		}

		// Token: 0x06003698 RID: 13976 RVA: 0x0010BBA8 File Offset: 0x00109FA8
		protected override SystemPermission GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr o = CAPI.ovr_Message_GetSystemPermission(obj);
			return new SystemPermission(o);
		}
	}
}
