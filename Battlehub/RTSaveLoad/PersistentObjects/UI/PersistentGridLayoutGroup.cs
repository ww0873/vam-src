using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;
using UnityEngine.UI;

namespace Battlehub.RTSaveLoad.PersistentObjects.UI
{
	// Token: 0x02000170 RID: 368
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentGridLayoutGroup : PersistentLayoutGroup
	{
		// Token: 0x0600081E RID: 2078 RVA: 0x00034C29 File Offset: 0x00033029
		public PersistentGridLayoutGroup()
		{
		}

		// Token: 0x0600081F RID: 2079 RVA: 0x00034C34 File Offset: 0x00033034
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			GridLayoutGroup gridLayoutGroup = (GridLayoutGroup)obj;
			gridLayoutGroup.startCorner = (GridLayoutGroup.Corner)this.startCorner;
			gridLayoutGroup.startAxis = (GridLayoutGroup.Axis)this.startAxis;
			gridLayoutGroup.cellSize = this.cellSize;
			gridLayoutGroup.spacing = this.spacing;
			gridLayoutGroup.constraint = (GridLayoutGroup.Constraint)this.constraint;
			gridLayoutGroup.constraintCount = this.constraintCount;
			return gridLayoutGroup;
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x00034CA4 File Offset: 0x000330A4
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			GridLayoutGroup gridLayoutGroup = (GridLayoutGroup)obj;
			this.startCorner = (uint)gridLayoutGroup.startCorner;
			this.startAxis = (uint)gridLayoutGroup.startAxis;
			this.cellSize = gridLayoutGroup.cellSize;
			this.spacing = gridLayoutGroup.spacing;
			this.constraint = (uint)gridLayoutGroup.constraint;
			this.constraintCount = gridLayoutGroup.constraintCount;
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x00034D0E File Offset: 0x0003310E
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x040008A5 RID: 2213
		public uint startCorner;

		// Token: 0x040008A6 RID: 2214
		public uint startAxis;

		// Token: 0x040008A7 RID: 2215
		public Vector2 cellSize;

		// Token: 0x040008A8 RID: 2216
		public Vector2 spacing;

		// Token: 0x040008A9 RID: 2217
		public uint constraint;

		// Token: 0x040008AA RID: 2218
		public int constraintCount;
	}
}
