using System;
using System.Collections.Generic;
using UnityEngine;

namespace PrefabEvolution
{
	// Token: 0x0200040C RID: 1036
	[Serializable]
	public class PEModifications
	{
		// Token: 0x06001A4B RID: 6731 RVA: 0x0009339D File Offset: 0x0009179D
		public PEModifications()
		{
		}

		// Token: 0x0400153A RID: 5434
		public List<PEModifications.PropertyData> Modificated = Utils.Create<List<PEModifications.PropertyData>>();

		// Token: 0x0400153B RID: 5435
		public List<PEModifications.HierarchyData> NonPrefabObjects = Utils.Create<List<PEModifications.HierarchyData>>();

		// Token: 0x0400153C RID: 5436
		public List<PEModifications.ComponentsData> NonPrefabComponents = Utils.Create<List<PEModifications.ComponentsData>>();

		// Token: 0x0400153D RID: 5437
		public List<int> RemovedObjects = Utils.Create<List<int>>();

		// Token: 0x0400153E RID: 5438
		public List<PEModifications.HierarchyData> TransformParentChanges = Utils.Create<List<PEModifications.HierarchyData>>();

		// Token: 0x0200040D RID: 1037
		[Serializable]
		public class PropertyData
		{
			// Token: 0x06001A4C RID: 6732 RVA: 0x000933DC File Offset: 0x000917DC
			public PropertyData()
			{
			}

			// Token: 0x0400153F RID: 5439
			public UnityEngine.Object Object;

			// Token: 0x04001540 RID: 5440
			public int ObjeckLink;

			// Token: 0x04001541 RID: 5441
			public string PropertyPath;

			// Token: 0x04001542 RID: 5442
			public PEModifications.PropertyData.PropertyMode Mode;

			// Token: 0x04001543 RID: 5443
			public object UserData;

			// Token: 0x0200040E RID: 1038
			public enum PropertyMode
			{
				// Token: 0x04001545 RID: 5445
				Default,
				// Token: 0x04001546 RID: 5446
				Keep,
				// Token: 0x04001547 RID: 5447
				Ignore
			}
		}

		// Token: 0x0200040F RID: 1039
		[Serializable]
		public class HierarchyData
		{
			// Token: 0x06001A4D RID: 6733 RVA: 0x000933E4 File Offset: 0x000917E4
			public HierarchyData()
			{
			}

			// Token: 0x04001548 RID: 5448
			public Transform child;

			// Token: 0x04001549 RID: 5449
			public Transform parent;
		}

		// Token: 0x02000410 RID: 1040
		[Serializable]
		public class ComponentsData
		{
			// Token: 0x06001A4E RID: 6734 RVA: 0x000933EC File Offset: 0x000917EC
			public ComponentsData()
			{
			}

			// Token: 0x0400154A RID: 5450
			public Component child;

			// Token: 0x0400154B RID: 5451
			public GameObject parent;
		}
	}
}
