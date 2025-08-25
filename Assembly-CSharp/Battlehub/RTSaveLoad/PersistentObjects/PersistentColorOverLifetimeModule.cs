using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x0200010F RID: 271
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentColorOverLifetimeModule : PersistentData
	{
		// Token: 0x06000693 RID: 1683 RVA: 0x0002D32C File Offset: 0x0002B72C
		public PersistentColorOverLifetimeModule()
		{
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x0002D334 File Offset: 0x0002B734
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			ParticleSystem.ColorOverLifetimeModule colorOverLifetimeModule = (ParticleSystem.ColorOverLifetimeModule)obj;
			colorOverLifetimeModule.enabled = this.enabled;
			colorOverLifetimeModule.color = base.Write<ParticleSystem.MinMaxGradient>(colorOverLifetimeModule.color, this.color, objects);
			return colorOverLifetimeModule;
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x0002D388 File Offset: 0x0002B788
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystem.ColorOverLifetimeModule colorOverLifetimeModule = (ParticleSystem.ColorOverLifetimeModule)obj;
			this.enabled = colorOverLifetimeModule.enabled;
			this.color = base.Read<PersistentMinMaxGradient>(this.color, colorOverLifetimeModule.color);
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x0002D3D5 File Offset: 0x0002B7D5
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxGradient>(this.color, dependencies, objects, allowNulls);
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x0002D3F0 File Offset: 0x0002B7F0
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystem.ColorOverLifetimeModule colorOverLifetimeModule = (ParticleSystem.ColorOverLifetimeModule)obj;
			base.GetDependencies<PersistentMinMaxGradient>(this.color, colorOverLifetimeModule.color, dependencies);
		}

		// Token: 0x0400065E RID: 1630
		public bool enabled;

		// Token: 0x0400065F RID: 1631
		public PersistentMinMaxGradient color;
	}
}
