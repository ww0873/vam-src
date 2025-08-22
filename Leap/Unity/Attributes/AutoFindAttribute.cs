using System;

namespace Leap.Unity.Attributes
{
	// Token: 0x0200066E RID: 1646
	[Obsolete]
	public class AutoFindAttribute : Attribute
	{
		// Token: 0x06002848 RID: 10312 RVA: 0x000DE840 File Offset: 0x000DCC40
		public AutoFindAttribute(AutoFindLocations searchLocations = AutoFindLocations.All)
		{
			this.searchLocations = searchLocations;
		}

		// Token: 0x040021A2 RID: 8610
		public readonly AutoFindLocations searchLocations;
	}
}
