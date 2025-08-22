using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x020001D1 RID: 465
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentTreePrototype : PersistentData
	{
		// Token: 0x06000971 RID: 2417 RVA: 0x0003A774 File Offset: 0x00038B74
		public PersistentTreePrototype()
		{
		}

		// Token: 0x06000972 RID: 2418 RVA: 0x0003A77C File Offset: 0x00038B7C
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			TreePrototype treePrototype = (TreePrototype)obj;
			treePrototype.prefab = (GameObject)objects.Get(this.prefab);
			treePrototype.bendFactor = this.bendFactor;
			return treePrototype;
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x0003A7C8 File Offset: 0x00038BC8
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			TreePrototype treePrototype = (TreePrototype)obj;
			this.prefab = treePrototype.prefab.GetMappedInstanceID();
			this.bendFactor = treePrototype.bendFactor;
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x0003A807 File Offset: 0x00038C07
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.prefab, dependencies, objects, allowNulls);
		}

		// Token: 0x06000975 RID: 2421 RVA: 0x0003A824 File Offset: 0x00038C24
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			TreePrototype treePrototype = (TreePrototype)obj;
			base.AddDependency(treePrototype.prefab, dependencies);
		}

		// Token: 0x04000AA1 RID: 2721
		public long prefab;

		// Token: 0x04000AA2 RID: 2722
		public float bendFactor;
	}
}
