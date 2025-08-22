using System;
using GPUTools.Cloth.Scripts;

// Token: 0x02000D65 RID: 3429
public class GPUToolsClothColliderReceiver : GPUToolsColliderReceiver
{
	// Token: 0x06006978 RID: 27000 RVA: 0x00277ED7 File Offset: 0x002762D7
	public GPUToolsClothColliderReceiver()
	{
	}

	// Token: 0x06006979 RID: 27001 RVA: 0x00277EE0 File Offset: 0x002762E0
	public void SyncClothSettings()
	{
		ClothSettings[] componentsInChildren = base.GetComponentsInChildren<ClothSettings>(true);
		foreach (ClothSettings clothSettings in componentsInChildren)
		{
			clothSettings.ColliderProviders = this._providerGameObjects;
			if (clothSettings.builder != null)
			{
				if (clothSettings.builder.spheres != null)
				{
					clothSettings.builder.spheres.UpdateSettings();
				}
				if (clothSettings.builder.lineSpheres != null)
				{
					clothSettings.builder.lineSpheres.UpdateSettings();
				}
			}
		}
	}

	// Token: 0x0600697A RID: 27002 RVA: 0x00277F66 File Offset: 0x00276366
	public override void SyncProviders()
	{
	}
}
