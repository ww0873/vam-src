using System;
using GPUTools.Hair.Scripts;

// Token: 0x02000D68 RID: 3432
public class GPUToolsHairColliderReceiver : GPUToolsColliderReceiver
{
	// Token: 0x06006981 RID: 27009 RVA: 0x00277F70 File Offset: 0x00276370
	public GPUToolsHairColliderReceiver()
	{
	}

	// Token: 0x06006982 RID: 27010 RVA: 0x00277F78 File Offset: 0x00276378
	public void SyncHairSettings()
	{
		HairSettings[] componentsInChildren = base.GetComponentsInChildren<HairSettings>(true);
		foreach (HairSettings hairSettings in componentsInChildren)
		{
			hairSettings.PhysicsSettings.ColliderProviders = this._providerGameObjects;
			if (hairSettings.HairBuidCommand != null)
			{
				if (hairSettings.HairBuidCommand.spheres != null)
				{
					hairSettings.HairBuidCommand.spheres.UpdateSettings();
				}
				if (hairSettings.HairBuidCommand.lineSpheres != null)
				{
					hairSettings.HairBuidCommand.lineSpheres.UpdateSettings();
				}
			}
		}
	}

	// Token: 0x06006983 RID: 27011 RVA: 0x00278003 File Offset: 0x00276403
	public override void SyncProviders()
	{
	}
}
