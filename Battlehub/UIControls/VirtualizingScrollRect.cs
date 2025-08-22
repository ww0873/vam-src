using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Battlehub.UIControls
{
	// Token: 0x02000296 RID: 662
	public class VirtualizingScrollRect : ScrollRect
	{
		// Token: 0x06000F60 RID: 3936 RVA: 0x00058351 File Offset: 0x00056751
		public VirtualizingScrollRect()
		{
		}

		// Token: 0x14000073 RID: 115
		// (add) Token: 0x06000F61 RID: 3937 RVA: 0x0005836C File Offset: 0x0005676C
		// (remove) Token: 0x06000F62 RID: 3938 RVA: 0x000583A4 File Offset: 0x000567A4
		public event DataBindAction ItemDataBinding
		{
			add
			{
				DataBindAction dataBindAction = this.ItemDataBinding;
				DataBindAction dataBindAction2;
				do
				{
					dataBindAction2 = dataBindAction;
					dataBindAction = Interlocked.CompareExchange<DataBindAction>(ref this.ItemDataBinding, (DataBindAction)Delegate.Combine(dataBindAction2, value), dataBindAction);
				}
				while (dataBindAction != dataBindAction2);
			}
			remove
			{
				DataBindAction dataBindAction = this.ItemDataBinding;
				DataBindAction dataBindAction2;
				do
				{
					dataBindAction2 = dataBindAction;
					dataBindAction = Interlocked.CompareExchange<DataBindAction>(ref this.ItemDataBinding, (DataBindAction)Delegate.Remove(dataBindAction2, value), dataBindAction);
				}
				while (dataBindAction != dataBindAction2);
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000F63 RID: 3939 RVA: 0x000583DA File Offset: 0x000567DA
		// (set) Token: 0x06000F64 RID: 3940 RVA: 0x000583E2 File Offset: 0x000567E2
		public IList Items
		{
			get
			{
				return this.m_items;
			}
			set
			{
				if (this.m_items != value)
				{
					this.m_items = value;
					this.DataBind(this.Index);
					this.UpdateContentSize();
				}
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06000F65 RID: 3941 RVA: 0x00058409 File Offset: 0x00056809
		public int ItemsCount
		{
			get
			{
				if (this.Items == null)
				{
					return 0;
				}
				return this.Items.Count;
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000F66 RID: 3942 RVA: 0x00058423 File Offset: 0x00056823
		// (set) Token: 0x06000F67 RID: 3943 RVA: 0x0005842B File Offset: 0x0005682B
		private float NormalizedIndex
		{
			get
			{
				return this.m_normalizedIndex;
			}
			set
			{
				if (value == this.m_normalizedIndex)
				{
					return;
				}
				this.OnNormalizedIndexChanged(value);
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000F68 RID: 3944 RVA: 0x00058444 File Offset: 0x00056844
		// (set) Token: 0x06000F69 RID: 3945 RVA: 0x00058480 File Offset: 0x00056880
		private int Index
		{
			get
			{
				if (this.ItemsCount == 0)
				{
					return 0;
				}
				return Mathf.RoundToInt(this.NormalizedIndex * (float)Mathf.Max(this.ItemsCount - this.VisibleItemsCount, 0));
			}
			set
			{
				if (value < 0 || value >= this.ItemsCount)
				{
					return;
				}
				this.NormalizedIndex = this.EvalNormalizedIndex(value);
			}
		}

		// Token: 0x06000F6A RID: 3946 RVA: 0x000584A4 File Offset: 0x000568A4
		private float EvalNormalizedIndex(int index)
		{
			int num = this.ItemsCount - this.VisibleItemsCount;
			if (num <= 0)
			{
				return 0f;
			}
			return (float)index / (float)num;
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000F6B RID: 3947 RVA: 0x000584D3 File Offset: 0x000568D3
		private int VisibleItemsCount
		{
			get
			{
				return Mathf.Min(this.ItemsCount, this.PossibleItemsCount);
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000F6C RID: 3948 RVA: 0x000584E6 File Offset: 0x000568E6
		private int PossibleItemsCount
		{
			get
			{
				if (this.ContainerSize < 1E-05f)
				{
					UnityEngine.Debug.LogWarning("ContainerSize is too small");
					return 0;
				}
				return Mathf.FloorToInt(this.Size / this.ContainerSize);
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000F6D RID: 3949 RVA: 0x00058518 File Offset: 0x00056918
		private float ContainerSize
		{
			get
			{
				if (this.m_mode == VirtualizingMode.Horizontal)
				{
					return Mathf.Max(0f, this.ContainerPrefab.rect.width);
				}
				if (this.m_mode == VirtualizingMode.Vertical)
				{
					return Mathf.Max(0f, this.ContainerPrefab.rect.height);
				}
				throw new InvalidOperationException("Unable to eval container size in non-virtualizing mode");
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000F6E RID: 3950 RVA: 0x00058584 File Offset: 0x00056984
		private float Size
		{
			get
			{
				if (this.m_mode == VirtualizingMode.Horizontal)
				{
					return Mathf.Max(0f, this.m_virtualContent.rect.width);
				}
				return Mathf.Max(0f, this.m_virtualContent.rect.height);
			}
		}

		// Token: 0x06000F6F RID: 3951 RVA: 0x000585D8 File Offset: 0x000569D8
		protected override void Awake()
		{
			base.Awake();
			if (this.m_virtualContent == null)
			{
				return;
			}
			this.m_virtualContentTransformChangeListener = this.m_virtualContent.GetComponent<RectTransformChangeListener>();
			this.m_virtualContentTransformChangeListener.RectTransformChanged += this.OnVirtualContentTransformChaged;
			this.UpdateVirtualContentPosition();
			if (this.m_mode == VirtualizingMode.Horizontal)
			{
				VerticalLayoutGroup component = this.m_virtualContent.GetComponent<VerticalLayoutGroup>();
				if (component != null)
				{
					UnityEngine.Object.DestroyImmediate(component);
				}
				HorizontalLayoutGroup horizontalLayoutGroup = this.m_virtualContent.GetComponent<HorizontalLayoutGroup>();
				if (horizontalLayoutGroup == null)
				{
					horizontalLayoutGroup = this.m_virtualContent.gameObject.AddComponent<HorizontalLayoutGroup>();
				}
				horizontalLayoutGroup.childControlHeight = true;
				horizontalLayoutGroup.childControlWidth = false;
				horizontalLayoutGroup.childForceExpandWidth = false;
			}
			else
			{
				HorizontalLayoutGroup component2 = this.m_virtualContent.GetComponent<HorizontalLayoutGroup>();
				if (component2 != null)
				{
					UnityEngine.Object.DestroyImmediate(component2);
				}
				VerticalLayoutGroup verticalLayoutGroup = this.m_virtualContent.GetComponent<VerticalLayoutGroup>();
				if (verticalLayoutGroup == null)
				{
					verticalLayoutGroup = this.m_virtualContent.gameObject.AddComponent<VerticalLayoutGroup>();
				}
				verticalLayoutGroup.childControlWidth = true;
				verticalLayoutGroup.childControlHeight = false;
				verticalLayoutGroup.childForceExpandHeight = false;
			}
			base.scrollSensitivity = this.ContainerSize;
		}

		// Token: 0x06000F70 RID: 3952 RVA: 0x000586FF File Offset: 0x00056AFF
		protected override void Start()
		{
			base.Start();
		}

		// Token: 0x06000F71 RID: 3953 RVA: 0x00058707 File Offset: 0x00056B07
		protected override void OnDestroy()
		{
			base.OnDestroy();
			if (this.m_virtualContentTransformChangeListener != null)
			{
				this.m_virtualContentTransformChangeListener.RectTransformChanged -= this.OnVirtualContentTransformChaged;
			}
		}

		// Token: 0x06000F72 RID: 3954 RVA: 0x00058738 File Offset: 0x00056B38
		private void OnVirtualContentTransformChaged()
		{
			if (this.m_mode == VirtualizingMode.Horizontal)
			{
				base.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this.m_virtualContent.rect.height);
			}
			else if (this.m_mode == VirtualizingMode.Vertical)
			{
				base.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.m_virtualContent.rect.width);
			}
		}

		// Token: 0x06000F73 RID: 3955 RVA: 0x000587A0 File Offset: 0x00056BA0
		protected override void SetNormalizedPosition(float value, int axis)
		{
			base.SetNormalizedPosition(value, axis);
			this.UpdateVirtualContentPosition();
			if (this.m_mode == VirtualizingMode.Vertical && axis == 1)
			{
				this.NormalizedIndex = 1f - value;
			}
			else if (this.m_mode == VirtualizingMode.Horizontal && axis == 0)
			{
				this.NormalizedIndex = value;
			}
		}

		// Token: 0x06000F74 RID: 3956 RVA: 0x000587F8 File Offset: 0x00056BF8
		protected override void SetContentAnchoredPosition(Vector2 position)
		{
			base.SetContentAnchoredPosition(position);
			this.UpdateVirtualContentPosition();
			if (this.m_mode == VirtualizingMode.Vertical)
			{
				this.NormalizedIndex = 1f - base.verticalNormalizedPosition;
			}
			else if (this.m_mode == VirtualizingMode.Horizontal)
			{
				this.NormalizedIndex = base.horizontalNormalizedPosition;
			}
		}

		// Token: 0x06000F75 RID: 3957 RVA: 0x0005884C File Offset: 0x00056C4C
		protected override void OnRectTransformDimensionsChange()
		{
			base.OnRectTransformDimensionsChange();
			base.StartCoroutine(this.CoRectTransformDimensionsChange());
		}

		// Token: 0x06000F76 RID: 3958 RVA: 0x00058864 File Offset: 0x00056C64
		private IEnumerator CoRectTransformDimensionsChange()
		{
			yield return new WaitForEndOfFrame();
			if (this.VisibleItemsCount != this.m_containers.Count)
			{
				this.DataBind(this.Index);
			}
			this.OnVirtualContentTransformChaged();
			yield break;
		}

		// Token: 0x06000F77 RID: 3959 RVA: 0x00058880 File Offset: 0x00056C80
		private void UpdateVirtualContentPosition()
		{
			if (this.m_virtualContent != null)
			{
				if (this.m_mode == VirtualizingMode.Horizontal)
				{
					this.m_virtualContent.anchoredPosition = new Vector2(0f, base.content.anchoredPosition.y);
				}
				else if (this.m_mode == VirtualizingMode.Vertical)
				{
					this.m_virtualContent.anchoredPosition = new Vector2(base.content.anchoredPosition.x, 0f);
				}
			}
		}

		// Token: 0x06000F78 RID: 3960 RVA: 0x0005890C File Offset: 0x00056D0C
		private void UpdateContentSize()
		{
			if (this.m_mode == VirtualizingMode.Horizontal)
			{
				base.content.sizeDelta = new Vector2((float)this.ItemsCount * this.ContainerSize, base.content.sizeDelta.y);
			}
			else if (this.m_mode == VirtualizingMode.Vertical)
			{
				base.content.sizeDelta = new Vector2(base.content.sizeDelta.x, (float)this.ItemsCount * this.ContainerSize);
			}
		}

		// Token: 0x06000F79 RID: 3961 RVA: 0x00058998 File Offset: 0x00056D98
		private void OnNormalizedIndexChanged(float newValue)
		{
			newValue = Mathf.Clamp01(newValue);
			int num = this.Index;
			float normalizedIndex = this.m_normalizedIndex;
			this.m_normalizedIndex = newValue;
			int index = this.Index;
			if (index < 0 || index >= this.ItemsCount)
			{
				this.m_normalizedIndex = normalizedIndex;
				return;
			}
			if (num != index)
			{
				int num2 = index - num;
				bool flag = num2 > 0;
				num2 = Mathf.Abs(num2);
				if (num2 > this.VisibleItemsCount)
				{
					this.DataBind(index);
				}
				else if (flag)
				{
					for (int i = 0; i < num2; i++)
					{
						LinkedListNode<RectTransform> first = this.m_containers.First;
						this.m_containers.RemoveFirst();
						int siblingIndex = this.m_containers.Last.Value.transform.GetSiblingIndex();
						this.m_containers.AddLast(first);
						RectTransform value = first.Value;
						value.SetSiblingIndex(siblingIndex + 1);
						if (this.ItemDataBinding != null && this.Items != null)
						{
							object item = this.Items[num + this.VisibleItemsCount];
							this.ItemDataBinding(value, item);
						}
						num++;
					}
				}
				else
				{
					for (int j = 0; j < num2; j++)
					{
						LinkedListNode<RectTransform> last = this.m_containers.Last;
						this.m_containers.RemoveLast();
						int siblingIndex2 = this.m_containers.First.Value.transform.GetSiblingIndex();
						this.m_containers.AddFirst(last);
						RectTransform value2 = last.Value;
						value2.SetSiblingIndex(siblingIndex2);
						num--;
						if (this.ItemDataBinding != null && this.Items != null)
						{
							object item2 = this.Items[num];
							this.ItemDataBinding(value2, item2);
						}
					}
				}
			}
		}

		// Token: 0x06000F7A RID: 3962 RVA: 0x00058B6C File Offset: 0x00056F6C
		private void DataBind(int firstItemIndex)
		{
			int num = this.VisibleItemsCount - this.m_containers.Count;
			if (num < 0)
			{
				for (int i = 0; i < -num; i++)
				{
					UnityEngine.Object.Destroy(this.m_containers.Last.Value.gameObject);
					this.m_containers.RemoveLast();
				}
			}
			else
			{
				for (int j = 0; j < num; j++)
				{
					RectTransform value = UnityEngine.Object.Instantiate<RectTransform>(this.ContainerPrefab, this.m_virtualContent);
					this.m_containers.AddLast(value);
				}
			}
			if (this.ItemDataBinding != null && this.Items != null)
			{
				int num2 = 0;
				foreach (RectTransform container in this.m_containers)
				{
					this.ItemDataBinding(container, this.Items[firstItemIndex + num2]);
					num2++;
				}
			}
		}

		// Token: 0x06000F7B RID: 3963 RVA: 0x00058C88 File Offset: 0x00057088
		public bool IsParentOf(Transform child)
		{
			return !(this.m_virtualContent == null) && child.IsChildOf(this.m_virtualContent);
		}

		// Token: 0x06000F7C RID: 3964 RVA: 0x00058CAC File Offset: 0x000570AC
		public void InsertItem(int index, object item, bool raiseItemDataBindingEvent = true)
		{
			int index2 = this.Index;
			int num = index2 + this.VisibleItemsCount - 1;
			this.m_items.Insert(index, item);
			this.UpdateContentSize();
			this.UpdateScrollbar(index2);
			if (this.PossibleItemsCount >= this.m_items.Count && this.m_containers.Count < this.VisibleItemsCount)
			{
				RectTransform value = UnityEngine.Object.Instantiate<RectTransform>(this.ContainerPrefab, this.m_virtualContent);
				this.m_containers.AddLast(value);
				num++;
			}
			if (index2 <= index && index <= num)
			{
				RectTransform value2 = this.m_containers.Last.Value;
				this.m_containers.RemoveLast();
				if (index == index2)
				{
					this.m_containers.AddFirst(value2);
					value2.SetSiblingIndex(0);
				}
				else
				{
					RectTransform value3 = this.m_containers.ElementAt(index - index2 - 1);
					LinkedListNode<RectTransform> node = this.m_containers.Find(value3);
					this.m_containers.AddAfter(node, value2);
					value2.SetSiblingIndex(index - index2);
				}
				if (raiseItemDataBindingEvent && this.ItemDataBinding != null)
				{
					this.ItemDataBinding(value2, item);
				}
			}
		}

		// Token: 0x06000F7D RID: 3965 RVA: 0x00058DD8 File Offset: 0x000571D8
		public void RemoveItems(int[] indices, bool raiseItemDataBindingEvent = true)
		{
			int num = this.Index;
			IEnumerable<int> source = indices;
			if (VirtualizingScrollRect.<>f__am$cache0 == null)
			{
				VirtualizingScrollRect.<>f__am$cache0 = new Func<int, int>(VirtualizingScrollRect.<RemoveItems>m__0);
			}
			indices = source.OrderBy(VirtualizingScrollRect.<>f__am$cache0).ToArray<int>();
			for (int i = indices.Length - 1; i >= 0; i--)
			{
				int num2 = indices[i];
				if (num2 >= 0 && num2 < this.m_items.Count)
				{
					this.m_items.RemoveAt(num2);
				}
			}
			if (num + this.VisibleItemsCount >= this.ItemsCount)
			{
				num = Mathf.Max(0, this.ItemsCount - this.VisibleItemsCount);
			}
			this.UpdateContentSize();
			this.UpdateScrollbar(num);
			this.DataBind(num);
			this.OnVirtualContentTransformChaged();
		}

		// Token: 0x06000F7E RID: 3966 RVA: 0x00058E9C File Offset: 0x0005729C
		public void SetNextSibling(object sibling, object nextSibling)
		{
			if (sibling == nextSibling)
			{
				return;
			}
			int num = this.m_items.IndexOf(sibling);
			int num2 = this.m_items.IndexOf(nextSibling);
			int index = this.Index;
			int num3 = index + this.VisibleItemsCount - 1;
			bool flag = index <= num2 && num2 <= num3;
			int num4 = num;
			if (num2 > num)
			{
				num4++;
			}
			int num5 = num2 - index;
			int num6 = num4 - index;
			bool flag2 = index <= num4 && ((num5 < 0) ? (num4 < num3) : (num4 <= num3));
			this.m_items.RemoveAt(num2);
			this.m_items.Insert(num4, nextSibling);
			if (flag2)
			{
				if (flag)
				{
					RectTransform rectTransform = this.m_containers.ElementAt(num5);
					this.m_containers.Remove(rectTransform);
					if (num6 == 0)
					{
						this.m_containers.AddFirst(rectTransform);
						rectTransform.SetSiblingIndex(0);
					}
					else
					{
						RectTransform value = this.m_containers.ElementAt(num6 - 1);
						LinkedListNode<RectTransform> node = this.m_containers.Find(value);
						this.m_containers.AddAfter(node, rectTransform);
					}
					rectTransform.SetSiblingIndex(num6);
					if (this.ItemDataBinding != null)
					{
						this.ItemDataBinding(rectTransform, nextSibling);
					}
				}
				else
				{
					RectTransform value2 = this.m_containers.Last.Value;
					this.m_containers.RemoveLast();
					if (num6 == 0)
					{
						this.m_containers.AddFirst(value2);
					}
					else
					{
						RectTransform value3 = (num5 >= 0) ? this.m_containers.ElementAt(num6 - 1) : this.m_containers.ElementAt(num6);
						LinkedListNode<RectTransform> node2 = this.m_containers.Find(value3);
						this.m_containers.AddAfter(node2, value2);
					}
					if (num5 < 0)
					{
						this.UpdateScrollbar(index - 1);
						value2.SetSiblingIndex(num6 + 1);
					}
					else
					{
						value2.SetSiblingIndex(num6);
					}
					if (this.ItemDataBinding != null)
					{
						this.ItemDataBinding(value2, nextSibling);
					}
				}
			}
			else if (flag)
			{
				if (num4 < index)
				{
					RectTransform rectTransform2 = this.m_containers.ElementAt(num5);
					this.m_containers.Remove(rectTransform2);
					this.m_containers.AddFirst(rectTransform2);
					rectTransform2.SetSiblingIndex(0);
					if (this.ItemDataBinding != null)
					{
						this.ItemDataBinding(rectTransform2, this.m_items[index]);
					}
				}
				else if (num4 > num3)
				{
					RectTransform rectTransform3 = this.m_containers.ElementAt(num5);
					this.m_containers.Remove(rectTransform3);
					this.m_containers.AddLast(rectTransform3);
					rectTransform3.SetSiblingIndex(this.m_containers.Count - 1);
					if (this.ItemDataBinding != null)
					{
						this.ItemDataBinding(rectTransform3, this.m_items[num3]);
					}
				}
			}
			else if (num5 < 0)
			{
				this.UpdateScrollbar(index - 1);
			}
		}

		// Token: 0x06000F7F RID: 3967 RVA: 0x000591A8 File Offset: 0x000575A8
		public void SetPrevSibling(object sibling, object prevSibling)
		{
			int num = this.m_items.IndexOf(sibling);
			num--;
			if (num >= 0)
			{
				sibling = this.m_items[num];
				this.SetNextSibling(sibling, prevSibling);
			}
			else
			{
				int index = this.m_items.IndexOf(prevSibling);
				this.m_items.RemoveAt(index);
				this.m_items.Insert(0, prevSibling);
				RectTransform value = this.m_containers.Last.Value;
				this.m_containers.RemoveLast();
				this.m_containers.AddFirst(value);
				value.SetSiblingIndex(0);
				if (this.ItemDataBinding != null)
				{
					this.ItemDataBinding(value, prevSibling);
				}
			}
		}

		// Token: 0x06000F80 RID: 3968 RVA: 0x00059258 File Offset: 0x00057658
		public RectTransform GetContainer(object obj)
		{
			if (this.m_items == null)
			{
				return null;
			}
			int num = this.m_items.IndexOf(obj);
			if (num < 0)
			{
				return null;
			}
			int index = this.Index;
			int num2 = index + this.VisibleItemsCount - 1;
			if (index <= num && num <= num2)
			{
				return this.m_containers.ElementAt(num - index);
			}
			return null;
		}

		// Token: 0x06000F81 RID: 3969 RVA: 0x000592B8 File Offset: 0x000576B8
		public RectTransform FirstContainer()
		{
			if (this.m_containers.Count == 0)
			{
				return null;
			}
			return this.m_containers.First.Value;
		}

		// Token: 0x06000F82 RID: 3970 RVA: 0x000592DC File Offset: 0x000576DC
		public void ForEachContainer(Action<RectTransform> action)
		{
			if (action == null)
			{
				return;
			}
			foreach (RectTransform obj in this.m_containers)
			{
				action(obj);
			}
		}

		// Token: 0x06000F83 RID: 3971 RVA: 0x00059340 File Offset: 0x00057740
		public RectTransform LastContainer()
		{
			if (this.m_containers.Count == 0)
			{
				return null;
			}
			return this.m_containers.Last.Value;
		}

		// Token: 0x06000F84 RID: 3972 RVA: 0x00059364 File Offset: 0x00057764
		private void UpdateScrollbar(int index)
		{
			this.m_normalizedIndex = this.EvalNormalizedIndex(index);
			if (this.m_mode == VirtualizingMode.Vertical)
			{
				base.verticalNormalizedPosition = 1f - this.m_normalizedIndex;
			}
			else if (this.m_mode == VirtualizingMode.Horizontal)
			{
				base.horizontalNormalizedPosition = this.m_normalizedIndex;
			}
		}

		// Token: 0x06000F85 RID: 3973 RVA: 0x000593B8 File Offset: 0x000577B8
		[CompilerGenerated]
		private static int <RemoveItems>m__0(int i)
		{
			return i;
		}

		// Token: 0x04000E30 RID: 3632
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private DataBindAction ItemDataBinding;

		// Token: 0x04000E31 RID: 3633
		public RectTransform ContainerPrefab;

		// Token: 0x04000E32 RID: 3634
		[SerializeField]
		private RectTransform m_virtualContent;

		// Token: 0x04000E33 RID: 3635
		private RectTransformChangeListener m_virtualContentTransformChangeListener;

		// Token: 0x04000E34 RID: 3636
		[SerializeField]
		private VirtualizingMode m_mode = VirtualizingMode.Vertical;

		// Token: 0x04000E35 RID: 3637
		private LinkedList<RectTransform> m_containers = new LinkedList<RectTransform>();

		// Token: 0x04000E36 RID: 3638
		private IList m_items;

		// Token: 0x04000E37 RID: 3639
		private float m_normalizedIndex;

		// Token: 0x04000E38 RID: 3640
		[CompilerGenerated]
		private static Func<int, int> <>f__am$cache0;

		// Token: 0x02000EE3 RID: 3811
		[CompilerGenerated]
		private sealed class <CoRectTransformDimensionsChange>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x06007211 RID: 29201 RVA: 0x000593BB File Offset: 0x000577BB
			[DebuggerHidden]
			public <CoRectTransformDimensionsChange>c__Iterator0()
			{
			}

			// Token: 0x06007212 RID: 29202 RVA: 0x000593C4 File Offset: 0x000577C4
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					this.$current = new WaitForEndOfFrame();
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1U:
					if (base.VisibleItemsCount != this.m_containers.Count)
					{
						base.DataBind(base.Index);
					}
					base.OnVirtualContentTransformChaged();
					this.$PC = -1;
					break;
				}
				return false;
			}

			// Token: 0x170010AB RID: 4267
			// (get) Token: 0x06007213 RID: 29203 RVA: 0x0005945C File Offset: 0x0005785C
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170010AC RID: 4268
			// (get) Token: 0x06007214 RID: 29204 RVA: 0x00059464 File Offset: 0x00057864
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x06007215 RID: 29205 RVA: 0x0005946C File Offset: 0x0005786C
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x06007216 RID: 29206 RVA: 0x0005947C File Offset: 0x0005787C
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x040065E9 RID: 26089
			internal VirtualizingScrollRect $this;

			// Token: 0x040065EA RID: 26090
			internal object $current;

			// Token: 0x040065EB RID: 26091
			internal bool $disposing;

			// Token: 0x040065EC RID: 26092
			internal int $PC;
		}
	}
}
