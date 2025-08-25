using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000192 RID: 402
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentNetworkView : PersistentBehaviour
	{
		// Token: 0x0600089D RID: 2205 RVA: 0x0003716D File Offset: 0x0003556D
		public PersistentNetworkView()
		{
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x00037175 File Offset: 0x00035575
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			return base.WriteTo(obj, objects);
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x0003717F File Offset: 0x0003557F
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x00037188 File Offset: 0x00035588
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.observed, dependencies, objects, allowNulls);
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x000371A2 File Offset: 0x000355A2
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
		}

		// Token: 0x0400096C RID: 2412
		public long observed;

		// Token: 0x0400096D RID: 2413
		public uint stateSynchronization;

		// Token: 0x0400096E RID: 2414
		public int group;
	}
}
