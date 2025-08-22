using System;
using GPUTools.Cloth.Scripts;
using UnityEngine;

namespace MeshVR
{
	// Token: 0x02000AC3 RID: 2755
	public class DAZClothSettingsSimTextureReloader : MonoBehaviour
	{
		// Token: 0x0600492A RID: 18730 RVA: 0x00173861 File Offset: 0x00171C61
		public DAZClothSettingsSimTextureReloader()
		{
		}

		// Token: 0x0600492B RID: 18731 RVA: 0x0017386C File Offset: 0x00171C6C
		protected void IndividualSimTextureUpdated()
		{
			ClothSettings[] components = base.GetComponents<ClothSettings>();
			ClothSettings clothSettings = null;
			foreach (ClothSettings clothSettings2 in components)
			{
				if (clothSettings2 != null && clothSettings2.enabled)
				{
					clothSettings = clothSettings2;
				}
			}
			if (clothSettings != null)
			{
				if (this.originalParticlesBlend == null && clothSettings.GeometryData != null)
				{
					this.originalEditorType = clothSettings.EditorType;
					this.originalParticlesBlend = (float[])clothSettings.GeometryData.ParticlesBlend.Clone();
				}
				bool flag = false;
				foreach (DAZSkinWrapMaterialOptions dazskinWrapMaterialOptions in this.dazSkinWrapMaterialOptions)
				{
					if (dazskinWrapMaterialOptions.hasCustomSimTexture)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					clothSettings.EditorType = ClothEditorType.Provider;
					if (clothSettings.GeometryData != null)
					{
						clothSettings.GeometryData.ResetParticlesBlend();
					}
				}
				else
				{
					clothSettings.EditorType = this.originalEditorType;
					if (clothSettings.GeometryData != null)
					{
						for (int k = 0; k < clothSettings.GeometryData.ParticlesBlend.Length; k++)
						{
							clothSettings.GeometryData.ParticlesBlend[k] = this.originalParticlesBlend[k];
						}
					}
				}
				if (clothSettings.builder != null)
				{
					if (clothSettings.builder.physicsBlend != null)
					{
						clothSettings.builder.physicsBlend.UpdateSettings();
					}
					clothSettings.Reset();
				}
				else
				{
					Debug.LogError("Builder is null");
				}
			}
			else
			{
				Debug.LogError("Cloth settings is null");
			}
		}

		// Token: 0x0600492C RID: 18732 RVA: 0x00173A0C File Offset: 0x00171E0C
		public void SyncSkinWrapMaterialOptions()
		{
			if (this.dazSkinWrapMaterialOptions != null)
			{
				foreach (DAZSkinWrapMaterialOptions dazskinWrapMaterialOptions in this.dazSkinWrapMaterialOptions)
				{
					DAZSkinWrapMaterialOptions dazskinWrapMaterialOptions2 = dazskinWrapMaterialOptions;
					dazskinWrapMaterialOptions2.simTextureLoadedHandlers = (DAZSkinWrapMaterialOptions.SimTextureLoaded)Delegate.Remove(dazskinWrapMaterialOptions2.simTextureLoadedHandlers, new DAZSkinWrapMaterialOptions.SimTextureLoaded(this.IndividualSimTextureUpdated));
				}
			}
			this.dazSkinWrapMaterialOptions = base.GetComponents<DAZSkinWrapMaterialOptions>();
			foreach (DAZSkinWrapMaterialOptions dazskinWrapMaterialOptions3 in this.dazSkinWrapMaterialOptions)
			{
				DAZSkinWrapMaterialOptions dazskinWrapMaterialOptions4 = dazskinWrapMaterialOptions3;
				dazskinWrapMaterialOptions4.simTextureLoadedHandlers = (DAZSkinWrapMaterialOptions.SimTextureLoaded)Delegate.Combine(dazskinWrapMaterialOptions4.simTextureLoadedHandlers, new DAZSkinWrapMaterialOptions.SimTextureLoaded(this.IndividualSimTextureUpdated));
			}
		}

		// Token: 0x040037A8 RID: 14248
		protected ClothSettings clothSettings;

		// Token: 0x040037A9 RID: 14249
		protected ClothEditorType originalEditorType;

		// Token: 0x040037AA RID: 14250
		protected float[] originalParticlesBlend;

		// Token: 0x040037AB RID: 14251
		protected DAZSkinWrapMaterialOptions[] dazSkinWrapMaterialOptions;
	}
}
