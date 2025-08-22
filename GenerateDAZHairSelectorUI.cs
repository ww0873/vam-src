using System;

// Token: 0x02000B26 RID: 2854
public class GenerateDAZHairSelectorUI : GenerateDAZDynamicSelectorUI
{
	// Token: 0x06004DF4 RID: 19956 RVA: 0x001B704C File Offset: 0x001B544C
	public GenerateDAZHairSelectorUI()
	{
	}

	// Token: 0x06004DF5 RID: 19957 RVA: 0x001B7054 File Offset: 0x001B5454
	protected override DAZDynamicItem[] GetDynamicItems()
	{
		return this.characterSelector.hairItems;
	}
}
