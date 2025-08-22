using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.Create
{
	// Token: 0x020009EE RID: 2542
	[Serializable]
	public class CreatorGeometry
	{
		// Token: 0x06004005 RID: 16389 RVA: 0x00130D43 File Offset: 0x0012F143
		public CreatorGeometry()
		{
		}

		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x06004006 RID: 16390 RVA: 0x00130D56 File Offset: 0x0012F156
		public GeometryGroupData Selected
		{
			get
			{
				return (this.SelectedIndex < 0 || this.SelectedIndex >= this.List.Count) ? null : this.List[this.SelectedIndex];
			}
		}

		// Token: 0x06004007 RID: 16391 RVA: 0x00130D91 File Offset: 0x0012F191
		public bool Validate(bool log)
		{
			if (this.List.Count == 0)
			{
				if (log)
				{
					Debug.LogError("No geometry was created");
				}
				return false;
			}
			return true;
		}

		// Token: 0x0400304F RID: 12367
		public List<GeometryGroupData> List = new List<GeometryGroupData>();

		// Token: 0x04003050 RID: 12368
		public int SelectedIndex;
	}
}
