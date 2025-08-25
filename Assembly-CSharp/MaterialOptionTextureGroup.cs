using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000D1E RID: 3358
[Serializable]
public class MaterialOptionTextureGroup
{
	// Token: 0x060066AC RID: 26284 RVA: 0x0026D20C File Offset: 0x0026B60C
	public MaterialOptionTextureGroup()
	{
	}

	// Token: 0x060066AD RID: 26285 RVA: 0x0026D274 File Offset: 0x0026B674
	public MaterialOptionTextureSet GetSetByName(string setName)
	{
		MaterialOptionTextureSet result = null;
		if (this.setNameToSet == null)
		{
			this.setNameToSet = new Dictionary<string, MaterialOptionTextureSet>();
			for (int i = 0; i < this.sets.Length; i++)
			{
				if (this.setNameToSet.ContainsKey(this.sets[i].name))
				{
					Debug.LogError("Duplicate material options texture set " + this.sets[i].name + " found. Must be unique by name");
				}
				else
				{
					this.setNameToSet.Add(this.sets[i].name, this.sets[i]);
				}
			}
		}
		this.setNameToSet.TryGetValue(setName, out result);
		return result;
	}

	// Token: 0x0400564C RID: 22092
	[NonSerialized]
	public MaterialOptions materialOptions;

	// Token: 0x0400564D RID: 22093
	public string name = "Texture Group";

	// Token: 0x0400564E RID: 22094
	public string textureName = "_MainTex";

	// Token: 0x0400564F RID: 22095
	public string secondaryTextureName = "_SpecTex";

	// Token: 0x04005650 RID: 22096
	public string thirdTextureName = "_GlossTex";

	// Token: 0x04005651 RID: 22097
	public string fourthTextureName = "_AlphaTex";

	// Token: 0x04005652 RID: 22098
	public string fifthTextureName = "_BumpMap";

	// Token: 0x04005653 RID: 22099
	public string sixthTextureName = "_DecalTex";

	// Token: 0x04005654 RID: 22100
	public bool mapTexturesToTextureNames = true;

	// Token: 0x04005655 RID: 22101
	public Texture2D autoCreateDefaultTexture;

	// Token: 0x04005656 RID: 22102
	public string autoCreateDefaultSetName;

	// Token: 0x04005657 RID: 22103
	public string autoCreateTextureFilePrefix;

	// Token: 0x04005658 RID: 22104
	public string autoCreateSetNamePrefix;

	// Token: 0x04005659 RID: 22105
	public int[] materialSlots;

	// Token: 0x0400565A RID: 22106
	public int[] materialSlots2;

	// Token: 0x0400565B RID: 22107
	public MaterialOptionTextureSet[] sets;

	// Token: 0x0400565C RID: 22108
	protected Dictionary<string, MaterialOptionTextureSet> setNameToSet;
}
