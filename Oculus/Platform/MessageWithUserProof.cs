using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x0200082F RID: 2095
	public class MessageWithUserProof : Message<UserProof>
	{
		// Token: 0x060036A5 RID: 13989 RVA: 0x0010BC99 File Offset: 0x0010A099
		public MessageWithUserProof(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x060036A6 RID: 13990 RVA: 0x0010BCA2 File Offset: 0x0010A0A2
		public override UserProof GetUserProof()
		{
			return base.Data;
		}

		// Token: 0x060036A7 RID: 13991 RVA: 0x0010BCAC File Offset: 0x0010A0AC
		protected override UserProof GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr o = CAPI.ovr_Message_GetUserProof(obj);
			return new UserProof(o);
		}
	}
}
