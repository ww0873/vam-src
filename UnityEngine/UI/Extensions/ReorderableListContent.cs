using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020004D3 RID: 1235
	public class ReorderableListContent : MonoBehaviour
	{
		// Token: 0x06001F29 RID: 7977 RVA: 0x000B0DB3 File Offset: 0x000AF1B3
		public ReorderableListContent()
		{
		}

		// Token: 0x06001F2A RID: 7978 RVA: 0x000B0DBB File Offset: 0x000AF1BB
		private void OnEnable()
		{
			if (this._rect)
			{
				base.StartCoroutine(this.RefreshChildren());
			}
		}

		// Token: 0x06001F2B RID: 7979 RVA: 0x000B0DDA File Offset: 0x000AF1DA
		public void OnTransformChildrenChanged()
		{
			if (base.isActiveAndEnabled)
			{
				base.StartCoroutine(this.RefreshChildren());
			}
		}

		// Token: 0x06001F2C RID: 7980 RVA: 0x000B0DF4 File Offset: 0x000AF1F4
		public void Init(ReorderableList extList)
		{
			this._extList = extList;
			this._rect = base.GetComponent<RectTransform>();
			this._cachedChildren = new List<Transform>();
			this._cachedListElement = new List<ReorderableListElement>();
			base.StartCoroutine(this.RefreshChildren());
		}

		// Token: 0x06001F2D RID: 7981 RVA: 0x000B0E2C File Offset: 0x000AF22C
		private IEnumerator RefreshChildren()
		{
			for (int i = 0; i < this._rect.childCount; i++)
			{
				if (!this._cachedChildren.Contains(this._rect.GetChild(i)))
				{
					this._ele = (this._rect.GetChild(i).gameObject.GetComponent<ReorderableListElement>() ?? this._rect.GetChild(i).gameObject.AddComponent<ReorderableListElement>());
					this._ele.Init(this._extList);
					this._cachedChildren.Add(this._rect.GetChild(i));
					this._cachedListElement.Add(this._ele);
				}
			}
			yield return 0;
			for (int j = this._cachedChildren.Count - 1; j >= 0; j--)
			{
				if (this._cachedChildren[j] == null)
				{
					this._cachedChildren.RemoveAt(j);
					this._cachedListElement.RemoveAt(j);
				}
			}
			yield break;
		}

		// Token: 0x04001A52 RID: 6738
		private List<Transform> _cachedChildren;

		// Token: 0x04001A53 RID: 6739
		private List<ReorderableListElement> _cachedListElement;

		// Token: 0x04001A54 RID: 6740
		private ReorderableListElement _ele;

		// Token: 0x04001A55 RID: 6741
		private ReorderableList _extList;

		// Token: 0x04001A56 RID: 6742
		private RectTransform _rect;

		// Token: 0x02000F73 RID: 3955
		[CompilerGenerated]
		private sealed class <RefreshChildren>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x060073E3 RID: 29667 RVA: 0x000B0E47 File Offset: 0x000AF247
			[DebuggerHidden]
			public <RefreshChildren>c__Iterator0()
			{
			}

			// Token: 0x060073E4 RID: 29668 RVA: 0x000B0E50 File Offset: 0x000AF250
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					for (int i = 0; i < this._rect.childCount; i++)
					{
						if (!this._cachedChildren.Contains(this._rect.GetChild(i)))
						{
							this._ele = (this._rect.GetChild(i).gameObject.GetComponent<ReorderableListElement>() ?? this._rect.GetChild(i).gameObject.AddComponent<ReorderableListElement>());
							this._ele.Init(this._extList);
							this._cachedChildren.Add(this._rect.GetChild(i));
							this._cachedListElement.Add(this._ele);
						}
					}
					this.$current = 0;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1U:
					for (int j = this._cachedChildren.Count - 1; j >= 0; j--)
					{
						if (this._cachedChildren[j] == null)
						{
							this._cachedChildren.RemoveAt(j);
							this._cachedListElement.RemoveAt(j);
						}
					}
					this.$PC = -1;
					break;
				}
				return false;
			}

			// Token: 0x17001103 RID: 4355
			// (get) Token: 0x060073E5 RID: 29669 RVA: 0x000B0FF4 File Offset: 0x000AF3F4
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x17001104 RID: 4356
			// (get) Token: 0x060073E6 RID: 29670 RVA: 0x000B0FFC File Offset: 0x000AF3FC
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x060073E7 RID: 29671 RVA: 0x000B1004 File Offset: 0x000AF404
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x060073E8 RID: 29672 RVA: 0x000B1014 File Offset: 0x000AF414
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x040067FF RID: 26623
			internal ReorderableListContent $this;

			// Token: 0x04006800 RID: 26624
			internal object $current;

			// Token: 0x04006801 RID: 26625
			internal bool $disposing;

			// Token: 0x04006802 RID: 26626
			internal int $PC;
		}
	}
}
