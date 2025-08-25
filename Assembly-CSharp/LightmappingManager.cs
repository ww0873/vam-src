using System;
using UnityEngine;

// Token: 0x020002F8 RID: 760
[ExecuteInEditMode]
public class LightmappingManager : MonoBehaviour
{
	// Token: 0x060011E5 RID: 4581 RVA: 0x00062E75 File Offset: 0x00061275
	public LightmappingManager()
	{
	}

	// Token: 0x060011E6 RID: 4582 RVA: 0x00062E7D File Offset: 0x0006127D
	private void Awake()
	{
		this.SetLightMapData();
		this.SetLightMapTextures();
	}

	// Token: 0x060011E7 RID: 4583 RVA: 0x00062E8C File Offset: 0x0006128C
	public void SetLightMapTextures()
	{
		if (this.lightMapTexturesFar == null || this.lightMapTexturesFar.Length <= 0)
		{
			return;
		}
		LightmapData[] array = new LightmapData[this.lightMapTexturesFar.Length];
		for (int i = 0; i < this.lightMapTexturesFar.Length; i++)
		{
			array[i] = new LightmapData();
			array[i].lightmapColor = this.lightMapTexturesFar[i];
		}
		LightmapSettings.lightmaps = array;
	}

	// Token: 0x060011E8 RID: 4584 RVA: 0x00062EF8 File Offset: 0x000612F8
	public void SetLightMapData()
	{
		if (this.sceneRenderers.Length <= 0)
		{
			return;
		}
		for (int i = 0; i < this.sceneRenderers.Length; i++)
		{
			if (this.sceneRenderers[i])
			{
				this.sceneRenderers[i].lightmapIndex = this.lighmapDataContainer.lightmapIndexes[i];
				this.sceneRenderers[i].lightmapScaleOffset = this.lighmapDataContainer.lightmapOffsetScales[i];
			}
		}
	}

	// Token: 0x04000F61 RID: 3937
	[SerializeField]
	public Renderer[] sceneRenderers;

	// Token: 0x04000F62 RID: 3938
	public LightMapDataContainerObject lighmapDataContainer;

	// Token: 0x04000F63 RID: 3939
	[SerializeField]
	public Texture2D[] lightMapTexturesFar;
}
