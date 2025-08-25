using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace PrefabEvolution
{
	// Token: 0x02000401 RID: 1025
	public class BaseExposedData : ISerializationCallbackReceiver
	{
		// Token: 0x06001A16 RID: 6678 RVA: 0x0009285C File Offset: 0x00090C5C
		public BaseExposedData()
		{
		}

		// Token: 0x06001A17 RID: 6679 RVA: 0x00092888 File Offset: 0x00090C88
		public virtual void OnBeforeSerialize()
		{
		}

		// Token: 0x06001A18 RID: 6680 RVA: 0x0009288A File Offset: 0x00090C8A
		public virtual void OnAfterDeserialize()
		{
		}

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06001A19 RID: 6681 RVA: 0x0009288C File Offset: 0x00090C8C
		public int SiblingIndex
		{
			get
			{
				return this.Brothers.ToList<BaseExposedData>().IndexOf(this);
			}
		}

		// Token: 0x06001A1A RID: 6682 RVA: 0x000928A0 File Offset: 0x00090CA0
		public float GetOrder(bool next)
		{
			int index = (!next) ? (this.SiblingIndex - 1) : (this.SiblingIndex + 1);
			BaseExposedData baseExposedData = this.Brothers.ElementAtOrDefault(index);
			return (baseExposedData != null) ? ((this.Order + baseExposedData.Order) * 0.5f) : (this.Order + (float)((!next) ? -1 : 1));
		}

		// Token: 0x06001A1B RID: 6683 RVA: 0x0009290C File Offset: 0x00090D0C
		public virtual BaseExposedData Clone()
		{
			BaseExposedData baseExposedData = Activator.CreateInstance(base.GetType()) as BaseExposedData;
			baseExposedData.ParentId = this.ParentId;
			baseExposedData.Label = this.Label;
			baseExposedData.guid = this.guid;
			baseExposedData.Order = this.Order;
			return baseExposedData;
		}

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06001A1C RID: 6684 RVA: 0x0009295B File Offset: 0x00090D5B
		public int Id
		{
			get
			{
				return this.guid;
			}
		}

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06001A1D RID: 6685 RVA: 0x00092963 File Offset: 0x00090D63
		// (set) Token: 0x06001A1E RID: 6686 RVA: 0x00092976 File Offset: 0x00090D76
		public BaseExposedData Parent
		{
			get
			{
				return this.Container[this.ParentId];
			}
			set
			{
				this.ParentId = value.Id;
			}
		}

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06001A1F RID: 6687 RVA: 0x00092984 File Offset: 0x00090D84
		public IEnumerable<BaseExposedData> Children
		{
			get
			{
				return this.Container.OrderedItems.Where(new Func<BaseExposedData, bool>(this.<get_Children>m__0));
			}
		}

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06001A20 RID: 6688 RVA: 0x000929A4 File Offset: 0x00090DA4
		public IEnumerable<BaseExposedData> Brothers
		{
			get
			{
				BaseExposedData.<>c__AnonStorey0 <>c__AnonStorey = new BaseExposedData.<>c__AnonStorey0();
				<>c__AnonStorey.parent = this.Parent;
				return this.Container.OrderedItems.Where(new Func<BaseExposedData, bool>(<>c__AnonStorey.<>m__0));
			}
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06001A21 RID: 6689 RVA: 0x000929DF File Offset: 0x00090DDF
		public bool Inherited
		{
			get
			{
				return this.Container.GetInherited(this.Id);
			}
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06001A22 RID: 6690 RVA: 0x000929F2 File Offset: 0x00090DF2
		// (set) Token: 0x06001A23 RID: 6691 RVA: 0x00092A05 File Offset: 0x00090E05
		public bool Hidden
		{
			get
			{
				return this.Container.GetHidden(this.Id);
			}
			set
			{
				this.Container.SetHide(this, value);
			}
		}

		// Token: 0x06001A24 RID: 6692 RVA: 0x00092A14 File Offset: 0x00090E14
		[CompilerGenerated]
		private bool <get_Children>m__0(BaseExposedData item)
		{
			return item.ParentId == this.Id;
		}

		// Token: 0x04001524 RID: 5412
		[NonSerialized]
		public PEExposedProperties Container;

		// Token: 0x04001525 RID: 5413
		[SerializeField]
		private int guid = Guid.NewGuid().GetHashCode();

		// Token: 0x04001526 RID: 5414
		public string Label;

		// Token: 0x04001527 RID: 5415
		public int ParentId;

		// Token: 0x04001528 RID: 5416
		public float Order;

		// Token: 0x02000402 RID: 1026
		public struct Comparer : IComparer<BaseExposedData>
		{
			// Token: 0x06001A25 RID: 6693 RVA: 0x00092A24 File Offset: 0x00090E24
			public int Compare(BaseExposedData x, BaseExposedData y)
			{
				return (int)Mathf.Sign(x.Order - y.Order);
			}
		}

		// Token: 0x02000F57 RID: 3927
		[CompilerGenerated]
		private sealed class <>c__AnonStorey0
		{
			// Token: 0x0600738E RID: 29582 RVA: 0x00092A39 File Offset: 0x00090E39
			public <>c__AnonStorey0()
			{
			}

			// Token: 0x0600738F RID: 29583 RVA: 0x00092A41 File Offset: 0x00090E41
			internal bool <>m__0(BaseExposedData i)
			{
				return i.Parent == this.parent;
			}

			// Token: 0x04006776 RID: 26486
			internal BaseExposedData parent;
		}
	}
}
