using System;
using System.Collections.Generic;
using UnityEngine;

namespace Leap.Unity.Space
{
	// Token: 0x0200071A RID: 1818
	public abstract class LeapSpace : LeapSpaceAnchor
	{
		// Token: 0x06002C48 RID: 11336 RVA: 0x000ECDE7 File Offset: 0x000EB1E7
		protected LeapSpace()
		{
		}

		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x06002C49 RID: 11337 RVA: 0x000ECDFA File Offset: 0x000EB1FA
		public static List<LeapSpace> allEnabled
		{
			get
			{
				return LeapSpace._enabledSpaces;
			}
		}

		// Token: 0x06002C4A RID: 11338 RVA: 0x000ECE01 File Offset: 0x000EB201
		protected override void OnEnable()
		{
			base.OnEnable();
			LeapSpace._enabledSpaces.Add(this);
			this.RebuildHierarchy();
			this.RecalculateTransformers();
		}

		// Token: 0x06002C4B RID: 11339 RVA: 0x000ECE20 File Offset: 0x000EB220
		protected override void OnDisable()
		{
			base.OnDisable();
			LeapSpace._enabledSpaces.Remove(this);
			for (int i = 0; i < this._anchors.Count; i++)
			{
				this._anchors[i].space = null;
				this._anchors[i].transformer = null;
			}
		}

		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x06002C4C RID: 11340 RVA: 0x000ECE7F File Offset: 0x000EB27F
		public List<LeapSpaceAnchor> anchors
		{
			get
			{
				return this._anchors;
			}
		}

		// Token: 0x06002C4D RID: 11341 RVA: 0x000ECE87 File Offset: 0x000EB287
		public void RebuildHierarchy()
		{
			this._anchors.Clear();
			this.rebuildHierarchyRecursively(base.transform);
		}

		// Token: 0x06002C4E RID: 11342 RVA: 0x000ECEA0 File Offset: 0x000EB2A0
		public void RecalculateTransformers()
		{
			this.transformer = this.CosntructBaseTransformer();
			for (int i = 1; i < this._anchors.Count; i++)
			{
				LeapSpaceAnchor leapSpaceAnchor = this._anchors[i];
				LeapSpaceAnchor parent = leapSpaceAnchor.parent;
				this.UpdateTransformer(leapSpaceAnchor.transformer, parent.transformer);
			}
		}

		// Token: 0x06002C4F RID: 11343
		public abstract Hash GetSettingHash();

		// Token: 0x06002C50 RID: 11344
		protected abstract ITransformer CosntructBaseTransformer();

		// Token: 0x06002C51 RID: 11345
		protected abstract ITransformer ConstructTransformer(LeapSpaceAnchor anchor);

		// Token: 0x06002C52 RID: 11346
		protected abstract void UpdateTransformer(ITransformer transformer, ITransformer parent);

		// Token: 0x06002C53 RID: 11347 RVA: 0x000ECEFC File Offset: 0x000EB2FC
		private void rebuildHierarchyRecursively(Transform root)
		{
			LeapSpaceAnchor component = root.GetComponent<LeapSpaceAnchor>();
			if (component != null && component.enabled)
			{
				component.space = this;
				component.RecalculateParentAnchor();
				component.transformer = this.ConstructTransformer(component);
				this._anchors.Add(component);
			}
			int childCount = root.childCount;
			for (int i = 0; i < childCount; i++)
			{
				this.rebuildHierarchyRecursively(root.GetChild(i));
			}
		}

		// Token: 0x06002C54 RID: 11348 RVA: 0x000ECF73 File Offset: 0x000EB373
		// Note: this type is marked as 'beforefieldinit'.
		static LeapSpace()
		{
		}

		// Token: 0x0400236E RID: 9070
		private static List<LeapSpace> _enabledSpaces = new List<LeapSpace>();

		// Token: 0x0400236F RID: 9071
		private List<LeapSpaceAnchor> _anchors = new List<LeapSpaceAnchor>();
	}
}
