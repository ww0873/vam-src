using System;
using System.Collections.Generic;

namespace PrefabEvolution
{
	// Token: 0x02000403 RID: 1027
	[Serializable]
	public class ExposedPropertyGroup : BaseExposedData
	{
		// Token: 0x06001A26 RID: 6694 RVA: 0x00092A51 File Offset: 0x00090E51
		public ExposedPropertyGroup()
		{
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06001A27 RID: 6695 RVA: 0x00092A60 File Offset: 0x00090E60
		// (set) Token: 0x06001A28 RID: 6696 RVA: 0x00092A94 File Offset: 0x00090E94
		public bool Expanded
		{
			get
			{
				if (!this.expandedLoaded)
				{
					ExposedPropertyGroup.expandedDict.TryGetValue(base.Id, out this.expanded);
					this.expandedLoaded = true;
				}
				return this.expanded;
			}
			set
			{
				if (value == this.expanded)
				{
					return;
				}
				if (!ExposedPropertyGroup.expandedDict.ContainsKey(base.Id))
				{
					ExposedPropertyGroup.expandedDict.Add(base.Id, true);
				}
				ExposedPropertyGroup.expandedDict[base.Id] = value;
				this.expanded = value;
			}
		}

		// Token: 0x06001A29 RID: 6697 RVA: 0x00092AEC File Offset: 0x00090EEC
		// Note: this type is marked as 'beforefieldinit'.
		static ExposedPropertyGroup()
		{
		}

		// Token: 0x04001529 RID: 5417
		public static Dictionary<int, bool> expandedDict = new Dictionary<int, bool>();

		// Token: 0x0400152A RID: 5418
		private bool expandedLoaded;

		// Token: 0x0400152B RID: 5419
		private bool expanded = true;
	}
}
