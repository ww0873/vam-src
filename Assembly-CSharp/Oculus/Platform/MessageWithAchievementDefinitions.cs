using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x02000802 RID: 2050
	public class MessageWithAchievementDefinitions : Message<AchievementDefinitionList>
	{
		// Token: 0x0600361E RID: 13854 RVA: 0x0010B391 File Offset: 0x00109791
		public MessageWithAchievementDefinitions(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x0600361F RID: 13855 RVA: 0x0010B39A File Offset: 0x0010979A
		public override AchievementDefinitionList GetAchievementDefinitions()
		{
			return base.Data;
		}

		// Token: 0x06003620 RID: 13856 RVA: 0x0010B3A4 File Offset: 0x001097A4
		protected override AchievementDefinitionList GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr a = CAPI.ovr_Message_GetAchievementDefinitionArray(obj);
			return new AchievementDefinitionList(a);
		}
	}
}
