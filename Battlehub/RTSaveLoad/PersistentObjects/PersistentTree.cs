using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x020001D0 RID: 464
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentTree : PersistentComponent
	{
		// Token: 0x0600096C RID: 2412 RVA: 0x0003A6A9 File Offset: 0x00038AA9
		public PersistentTree()
		{
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x0003A6B4 File Offset: 0x00038AB4
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			Tree tree = (Tree)obj;
			tree.data = (ScriptableObject)objects.Get(this.data);
			return tree;
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x0003A6F4 File Offset: 0x00038AF4
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			Tree tree = (Tree)obj;
			this.data = tree.data.GetMappedInstanceID();
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x0003A727 File Offset: 0x00038B27
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.data, dependencies, objects, allowNulls);
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x0003A744 File Offset: 0x00038B44
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			Tree tree = (Tree)obj;
			base.AddDependency(tree.data, dependencies);
		}

		// Token: 0x04000AA0 RID: 2720
		public long data;
	}
}
