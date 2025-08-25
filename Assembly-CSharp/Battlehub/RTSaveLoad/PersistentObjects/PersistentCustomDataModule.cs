using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000110 RID: 272
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentCustomDataModule : PersistentData
	{
		// Token: 0x06000698 RID: 1688 RVA: 0x0002D42C File Offset: 0x0002B82C
		public PersistentCustomDataModule()
		{
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x0002D434 File Offset: 0x0002B834
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			ParticleSystem.CustomDataModule customDataModule = (ParticleSystem.CustomDataModule)obj;
			customDataModule.enabled = this.enabled;
			return customDataModule;
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x0002D470 File Offset: 0x0002B870
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			this.enabled = ((ParticleSystem.CustomDataModule)obj).enabled;
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x0002D49F File Offset: 0x0002B89F
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x04000660 RID: 1632
		public bool enabled;
	}
}
