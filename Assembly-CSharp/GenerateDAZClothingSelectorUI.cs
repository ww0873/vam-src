using System;

// Token: 0x02000B22 RID: 2850
public class GenerateDAZClothingSelectorUI : GenerateDAZDynamicSelectorUI
{
	// Token: 0x06004DBB RID: 19899 RVA: 0x001B7037 File Offset: 0x001B5437
	public GenerateDAZClothingSelectorUI()
	{
	}

	// Token: 0x06004DBC RID: 19900 RVA: 0x001B703F File Offset: 0x001B543F
	protected override DAZDynamicItem[] GetDynamicItems()
	{
		return this.characterSelector.clothingItems;
	}
}
