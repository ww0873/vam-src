using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Battlehub.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Battlehub.RTCommon
{
	// Token: 0x020000AF RID: 175
	[DisallowMultipleComponent]
	public class ExposeToEditor : MonoBehaviour
	{
		// Token: 0x06000295 RID: 661 RVA: 0x0001241C File Offset: 0x0001081C
		public ExposeToEditor()
		{
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000296 RID: 662 RVA: 0x00012444 File Offset: 0x00010844
		// (remove) Token: 0x06000297 RID: 663 RVA: 0x00012478 File Offset: 0x00010878
		public static event ExposeToEditorEvent Awaked
		{
			add
			{
				ExposeToEditorEvent exposeToEditorEvent = ExposeToEditor.Awaked;
				ExposeToEditorEvent exposeToEditorEvent2;
				do
				{
					exposeToEditorEvent2 = exposeToEditorEvent;
					exposeToEditorEvent = Interlocked.CompareExchange<ExposeToEditorEvent>(ref ExposeToEditor.Awaked, (ExposeToEditorEvent)Delegate.Combine(exposeToEditorEvent2, value), exposeToEditorEvent);
				}
				while (exposeToEditorEvent != exposeToEditorEvent2);
			}
			remove
			{
				ExposeToEditorEvent exposeToEditorEvent = ExposeToEditor.Awaked;
				ExposeToEditorEvent exposeToEditorEvent2;
				do
				{
					exposeToEditorEvent2 = exposeToEditorEvent;
					exposeToEditorEvent = Interlocked.CompareExchange<ExposeToEditorEvent>(ref ExposeToEditor.Awaked, (ExposeToEditorEvent)Delegate.Remove(exposeToEditorEvent2, value), exposeToEditorEvent);
				}
				while (exposeToEditorEvent != exposeToEditorEvent2);
			}
		}

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000298 RID: 664 RVA: 0x000124AC File Offset: 0x000108AC
		// (remove) Token: 0x06000299 RID: 665 RVA: 0x000124E0 File Offset: 0x000108E0
		public static event ExposeToEditorEvent Destroying
		{
			add
			{
				ExposeToEditorEvent exposeToEditorEvent = ExposeToEditor.Destroying;
				ExposeToEditorEvent exposeToEditorEvent2;
				do
				{
					exposeToEditorEvent2 = exposeToEditorEvent;
					exposeToEditorEvent = Interlocked.CompareExchange<ExposeToEditorEvent>(ref ExposeToEditor.Destroying, (ExposeToEditorEvent)Delegate.Combine(exposeToEditorEvent2, value), exposeToEditorEvent);
				}
				while (exposeToEditorEvent != exposeToEditorEvent2);
			}
			remove
			{
				ExposeToEditorEvent exposeToEditorEvent = ExposeToEditor.Destroying;
				ExposeToEditorEvent exposeToEditorEvent2;
				do
				{
					exposeToEditorEvent2 = exposeToEditorEvent;
					exposeToEditorEvent = Interlocked.CompareExchange<ExposeToEditorEvent>(ref ExposeToEditor.Destroying, (ExposeToEditorEvent)Delegate.Remove(exposeToEditorEvent2, value), exposeToEditorEvent);
				}
				while (exposeToEditorEvent != exposeToEditorEvent2);
			}
		}

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x0600029A RID: 666 RVA: 0x00012514 File Offset: 0x00010914
		// (remove) Token: 0x0600029B RID: 667 RVA: 0x00012548 File Offset: 0x00010948
		public static event ExposeToEditorEvent Destroyed
		{
			add
			{
				ExposeToEditorEvent exposeToEditorEvent = ExposeToEditor.Destroyed;
				ExposeToEditorEvent exposeToEditorEvent2;
				do
				{
					exposeToEditorEvent2 = exposeToEditorEvent;
					exposeToEditorEvent = Interlocked.CompareExchange<ExposeToEditorEvent>(ref ExposeToEditor.Destroyed, (ExposeToEditorEvent)Delegate.Combine(exposeToEditorEvent2, value), exposeToEditorEvent);
				}
				while (exposeToEditorEvent != exposeToEditorEvent2);
			}
			remove
			{
				ExposeToEditorEvent exposeToEditorEvent = ExposeToEditor.Destroyed;
				ExposeToEditorEvent exposeToEditorEvent2;
				do
				{
					exposeToEditorEvent2 = exposeToEditorEvent;
					exposeToEditorEvent = Interlocked.CompareExchange<ExposeToEditorEvent>(ref ExposeToEditor.Destroyed, (ExposeToEditorEvent)Delegate.Remove(exposeToEditorEvent2, value), exposeToEditorEvent);
				}
				while (exposeToEditorEvent != exposeToEditorEvent2);
			}
		}

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x0600029C RID: 668 RVA: 0x0001257C File Offset: 0x0001097C
		// (remove) Token: 0x0600029D RID: 669 RVA: 0x000125B0 File Offset: 0x000109B0
		public static event ExposeToEditorEvent MarkAsDestroyedChanged
		{
			add
			{
				ExposeToEditorEvent exposeToEditorEvent = ExposeToEditor.MarkAsDestroyedChanged;
				ExposeToEditorEvent exposeToEditorEvent2;
				do
				{
					exposeToEditorEvent2 = exposeToEditorEvent;
					exposeToEditorEvent = Interlocked.CompareExchange<ExposeToEditorEvent>(ref ExposeToEditor.MarkAsDestroyedChanged, (ExposeToEditorEvent)Delegate.Combine(exposeToEditorEvent2, value), exposeToEditorEvent);
				}
				while (exposeToEditorEvent != exposeToEditorEvent2);
			}
			remove
			{
				ExposeToEditorEvent exposeToEditorEvent = ExposeToEditor.MarkAsDestroyedChanged;
				ExposeToEditorEvent exposeToEditorEvent2;
				do
				{
					exposeToEditorEvent2 = exposeToEditorEvent;
					exposeToEditorEvent = Interlocked.CompareExchange<ExposeToEditorEvent>(ref ExposeToEditor.MarkAsDestroyedChanged, (ExposeToEditorEvent)Delegate.Remove(exposeToEditorEvent2, value), exposeToEditorEvent);
				}
				while (exposeToEditorEvent != exposeToEditorEvent2);
			}
		}

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x0600029E RID: 670 RVA: 0x000125E4 File Offset: 0x000109E4
		// (remove) Token: 0x0600029F RID: 671 RVA: 0x00012618 File Offset: 0x00010A18
		public static event ExposeToEditorEvent NameChanged
		{
			add
			{
				ExposeToEditorEvent exposeToEditorEvent = ExposeToEditor.NameChanged;
				ExposeToEditorEvent exposeToEditorEvent2;
				do
				{
					exposeToEditorEvent2 = exposeToEditorEvent;
					exposeToEditorEvent = Interlocked.CompareExchange<ExposeToEditorEvent>(ref ExposeToEditor.NameChanged, (ExposeToEditorEvent)Delegate.Combine(exposeToEditorEvent2, value), exposeToEditorEvent);
				}
				while (exposeToEditorEvent != exposeToEditorEvent2);
			}
			remove
			{
				ExposeToEditorEvent exposeToEditorEvent = ExposeToEditor.NameChanged;
				ExposeToEditorEvent exposeToEditorEvent2;
				do
				{
					exposeToEditorEvent2 = exposeToEditorEvent;
					exposeToEditorEvent = Interlocked.CompareExchange<ExposeToEditorEvent>(ref ExposeToEditor.NameChanged, (ExposeToEditorEvent)Delegate.Remove(exposeToEditorEvent2, value), exposeToEditorEvent);
				}
				while (exposeToEditorEvent != exposeToEditorEvent2);
			}
		}

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x060002A0 RID: 672 RVA: 0x0001264C File Offset: 0x00010A4C
		// (remove) Token: 0x060002A1 RID: 673 RVA: 0x00012680 File Offset: 0x00010A80
		public static event ExposeToEditorEvent TransformChanged
		{
			add
			{
				ExposeToEditorEvent exposeToEditorEvent = ExposeToEditor.TransformChanged;
				ExposeToEditorEvent exposeToEditorEvent2;
				do
				{
					exposeToEditorEvent2 = exposeToEditorEvent;
					exposeToEditorEvent = Interlocked.CompareExchange<ExposeToEditorEvent>(ref ExposeToEditor.TransformChanged, (ExposeToEditorEvent)Delegate.Combine(exposeToEditorEvent2, value), exposeToEditorEvent);
				}
				while (exposeToEditorEvent != exposeToEditorEvent2);
			}
			remove
			{
				ExposeToEditorEvent exposeToEditorEvent = ExposeToEditor.TransformChanged;
				ExposeToEditorEvent exposeToEditorEvent2;
				do
				{
					exposeToEditorEvent2 = exposeToEditorEvent;
					exposeToEditorEvent = Interlocked.CompareExchange<ExposeToEditorEvent>(ref ExposeToEditor.TransformChanged, (ExposeToEditorEvent)Delegate.Remove(exposeToEditorEvent2, value), exposeToEditorEvent);
				}
				while (exposeToEditorEvent != exposeToEditorEvent2);
			}
		}

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x060002A2 RID: 674 RVA: 0x000126B4 File Offset: 0x00010AB4
		// (remove) Token: 0x060002A3 RID: 675 RVA: 0x000126E8 File Offset: 0x00010AE8
		public static event ExposeToEditorEvent Started
		{
			add
			{
				ExposeToEditorEvent exposeToEditorEvent = ExposeToEditor.Started;
				ExposeToEditorEvent exposeToEditorEvent2;
				do
				{
					exposeToEditorEvent2 = exposeToEditorEvent;
					exposeToEditorEvent = Interlocked.CompareExchange<ExposeToEditorEvent>(ref ExposeToEditor.Started, (ExposeToEditorEvent)Delegate.Combine(exposeToEditorEvent2, value), exposeToEditorEvent);
				}
				while (exposeToEditorEvent != exposeToEditorEvent2);
			}
			remove
			{
				ExposeToEditorEvent exposeToEditorEvent = ExposeToEditor.Started;
				ExposeToEditorEvent exposeToEditorEvent2;
				do
				{
					exposeToEditorEvent2 = exposeToEditorEvent;
					exposeToEditorEvent = Interlocked.CompareExchange<ExposeToEditorEvent>(ref ExposeToEditor.Started, (ExposeToEditorEvent)Delegate.Remove(exposeToEditorEvent2, value), exposeToEditorEvent);
				}
				while (exposeToEditorEvent != exposeToEditorEvent2);
			}
		}

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x060002A4 RID: 676 RVA: 0x0001271C File Offset: 0x00010B1C
		// (remove) Token: 0x060002A5 RID: 677 RVA: 0x00012750 File Offset: 0x00010B50
		public static event ExposeToEditorEvent Enabled
		{
			add
			{
				ExposeToEditorEvent exposeToEditorEvent = ExposeToEditor.Enabled;
				ExposeToEditorEvent exposeToEditorEvent2;
				do
				{
					exposeToEditorEvent2 = exposeToEditorEvent;
					exposeToEditorEvent = Interlocked.CompareExchange<ExposeToEditorEvent>(ref ExposeToEditor.Enabled, (ExposeToEditorEvent)Delegate.Combine(exposeToEditorEvent2, value), exposeToEditorEvent);
				}
				while (exposeToEditorEvent != exposeToEditorEvent2);
			}
			remove
			{
				ExposeToEditorEvent exposeToEditorEvent = ExposeToEditor.Enabled;
				ExposeToEditorEvent exposeToEditorEvent2;
				do
				{
					exposeToEditorEvent2 = exposeToEditorEvent;
					exposeToEditorEvent = Interlocked.CompareExchange<ExposeToEditorEvent>(ref ExposeToEditor.Enabled, (ExposeToEditorEvent)Delegate.Remove(exposeToEditorEvent2, value), exposeToEditorEvent);
				}
				while (exposeToEditorEvent != exposeToEditorEvent2);
			}
		}

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x060002A6 RID: 678 RVA: 0x00012784 File Offset: 0x00010B84
		// (remove) Token: 0x060002A7 RID: 679 RVA: 0x000127B8 File Offset: 0x00010BB8
		public static event ExposeToEditorEvent Disabled
		{
			add
			{
				ExposeToEditorEvent exposeToEditorEvent = ExposeToEditor.Disabled;
				ExposeToEditorEvent exposeToEditorEvent2;
				do
				{
					exposeToEditorEvent2 = exposeToEditorEvent;
					exposeToEditorEvent = Interlocked.CompareExchange<ExposeToEditorEvent>(ref ExposeToEditor.Disabled, (ExposeToEditorEvent)Delegate.Combine(exposeToEditorEvent2, value), exposeToEditorEvent);
				}
				while (exposeToEditorEvent != exposeToEditorEvent2);
			}
			remove
			{
				ExposeToEditorEvent exposeToEditorEvent = ExposeToEditor.Disabled;
				ExposeToEditorEvent exposeToEditorEvent2;
				do
				{
					exposeToEditorEvent2 = exposeToEditorEvent;
					exposeToEditorEvent = Interlocked.CompareExchange<ExposeToEditorEvent>(ref ExposeToEditor.Disabled, (ExposeToEditorEvent)Delegate.Remove(exposeToEditorEvent2, value), exposeToEditorEvent);
				}
				while (exposeToEditorEvent != exposeToEditorEvent2);
			}
		}

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x060002A8 RID: 680 RVA: 0x000127EC File Offset: 0x00010BEC
		// (remove) Token: 0x060002A9 RID: 681 RVA: 0x00012820 File Offset: 0x00010C20
		public static event ExposeToEditorChangeEvent<ExposeToEditor> ParentChanged
		{
			add
			{
				ExposeToEditorChangeEvent<ExposeToEditor> exposeToEditorChangeEvent = ExposeToEditor.ParentChanged;
				ExposeToEditorChangeEvent<ExposeToEditor> exposeToEditorChangeEvent2;
				do
				{
					exposeToEditorChangeEvent2 = exposeToEditorChangeEvent;
					exposeToEditorChangeEvent = Interlocked.CompareExchange<ExposeToEditorChangeEvent<ExposeToEditor>>(ref ExposeToEditor.ParentChanged, (ExposeToEditorChangeEvent<ExposeToEditor>)Delegate.Combine(exposeToEditorChangeEvent2, value), exposeToEditorChangeEvent);
				}
				while (exposeToEditorChangeEvent != exposeToEditorChangeEvent2);
			}
			remove
			{
				ExposeToEditorChangeEvent<ExposeToEditor> exposeToEditorChangeEvent = ExposeToEditor.ParentChanged;
				ExposeToEditorChangeEvent<ExposeToEditor> exposeToEditorChangeEvent2;
				do
				{
					exposeToEditorChangeEvent2 = exposeToEditorChangeEvent;
					exposeToEditorChangeEvent = Interlocked.CompareExchange<ExposeToEditorChangeEvent<ExposeToEditor>>(ref ExposeToEditor.ParentChanged, (ExposeToEditorChangeEvent<ExposeToEditor>)Delegate.Remove(exposeToEditorChangeEvent2, value), exposeToEditorChangeEvent);
				}
				while (exposeToEditorChangeEvent != exposeToEditorChangeEvent2);
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060002AA RID: 682 RVA: 0x00012854 File Offset: 0x00010C54
		public Collider[] Colliders
		{
			get
			{
				return this.m_colliders;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060002AB RID: 683 RVA: 0x0001285C File Offset: 0x00010C5C
		// (set) Token: 0x060002AC RID: 684 RVA: 0x00012864 File Offset: 0x00010C64
		public ExposeToEditorObjectType ObjectType
		{
			get
			{
				return this.m_objectType;
			}
			set
			{
				if (this.m_objectType == ExposeToEditorObjectType.Undefined || this.m_objectType != value)
				{
				}
				this.m_objectType = value;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060002AD RID: 685 RVA: 0x00012884 File Offset: 0x00010C84
		// (set) Token: 0x060002AE RID: 686 RVA: 0x0001288C File Offset: 0x00010C8C
		public bool MarkAsDestroyed
		{
			get
			{
				return this.m_markAsDestroyed;
			}
			set
			{
				if (this.m_markAsDestroyed != value)
				{
					this.m_markAsDestroyed = value;
					base.gameObject.SetActive(!this.m_markAsDestroyed);
					if (ExposeToEditor.MarkAsDestroyedChanged != null)
					{
						ExposeToEditor.MarkAsDestroyedChanged(this);
					}
				}
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060002AF RID: 687 RVA: 0x000128CC File Offset: 0x00010CCC
		public Bounds Bounds
		{
			get
			{
				if (this.m_effectiveBoundsType == BoundsType.Any)
				{
					if (this.m_filter != null && this.m_filter.sharedMesh != null)
					{
						return this.m_filter.sharedMesh.bounds;
					}
					if (this.m_skinned != null && this.m_skinned.sharedMesh != null)
					{
						return this.m_skinned.sharedMesh.bounds;
					}
					if (this.m_spriteRenderer != null)
					{
						return this.m_spriteRenderer.sprite.bounds;
					}
					return this.CustomBounds;
				}
				else
				{
					if (this.m_effectiveBoundsType != BoundsType.Mesh)
					{
						if (this.m_effectiveBoundsType == BoundsType.SkinnedMesh)
						{
							if (this.m_skinned != null && this.m_skinned.sharedMesh != null)
							{
								return this.m_skinned.sharedMesh.bounds;
							}
						}
						else if (this.m_effectiveBoundsType == BoundsType.Sprite)
						{
							if (this.m_spriteRenderer != null)
							{
								return this.m_spriteRenderer.sprite.bounds;
							}
						}
						else if (this.m_effectiveBoundsType == BoundsType.Custom)
						{
							return this.CustomBounds;
						}
						return ExposeToEditor.m_none;
					}
					if (this.m_filter != null && this.m_filter.sharedMesh != null)
					{
						return this.m_filter.sharedMesh.bounds;
					}
					return ExposeToEditor.m_none;
				}
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060002B0 RID: 688 RVA: 0x00012A5B File Offset: 0x00010E5B
		public int ChildCount
		{
			get
			{
				return this.m_children.Count;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060002B1 RID: 689 RVA: 0x00012A68 File Offset: 0x00010E68
		public int MarkedAsDestroyedChildCount
		{
			get
			{
				IEnumerable<ExposeToEditor> children = this.m_children;
				if (ExposeToEditor.<>f__am$cache0 == null)
				{
					ExposeToEditor.<>f__am$cache0 = new Func<ExposeToEditor, bool>(ExposeToEditor.<get_MarkedAsDestroyedChildCount>m__0);
				}
				return children.Where(ExposeToEditor.<>f__am$cache0).Count<ExposeToEditor>();
			}
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x00012A97 File Offset: 0x00010E97
		public ExposeToEditor[] GetChildren()
		{
			IEnumerable<ExposeToEditor> children = this.m_children;
			if (ExposeToEditor.<>f__am$cache1 == null)
			{
				ExposeToEditor.<>f__am$cache1 = new Func<ExposeToEditor, int>(ExposeToEditor.<GetChildren>m__1);
			}
			return children.OrderBy(ExposeToEditor.<>f__am$cache1).ToArray<ExposeToEditor>();
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x00012AC8 File Offset: 0x00010EC8
		public ExposeToEditor NextSibling()
		{
			if (!(this.Parent != null))
			{
				IEnumerable<GameObject> enumerable;
				if (RuntimeEditorApplication.IsPlaying)
				{
					enumerable = ExposeToEditor.FindAll(ExposeToEditorObjectType.PlayMode, true);
				}
				else
				{
					IEnumerable<GameObject> source = ExposeToEditor.FindAll(ExposeToEditorObjectType.EditorMode, true);
					if (ExposeToEditor.<>f__am$cache2 == null)
					{
						ExposeToEditor.<>f__am$cache2 = new Func<GameObject, int>(ExposeToEditor.<NextSibling>m__2);
					}
					enumerable = source.OrderBy(ExposeToEditor.<>f__am$cache2);
				}
				IEnumerable<GameObject> enumerable2 = enumerable;
				IEnumerator<GameObject> enumerator = enumerable2.GetEnumerator();
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == base.gameObject)
					{
						enumerator.MoveNext();
						return enumerator.Current.GetComponent<ExposeToEditor>();
					}
				}
				return null;
			}
			int num = this.Parent.m_children.IndexOf(this);
			if (num < this.Parent.m_children.Count - 1)
			{
				return this.Parent.m_children[num - 1];
			}
			return null;
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060002B4 RID: 692 RVA: 0x00012BA7 File Offset: 0x00010FA7
		// (set) Token: 0x060002B5 RID: 693 RVA: 0x00012BB0 File Offset: 0x00010FB0
		public ExposeToEditor Parent
		{
			get
			{
				return this.m_parent;
			}
			set
			{
				if (this.m_parent != value)
				{
					ExposeToEditor oldValue = this.ChangeParent(value);
					if (ExposeToEditor.ParentChanged != null)
					{
						ExposeToEditor.ParentChanged(this, oldValue, this.m_parent);
					}
				}
			}
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x00012BF4 File Offset: 0x00010FF4
		private ExposeToEditor ChangeParent(ExposeToEditor value)
		{
			ExposeToEditor parent = this.m_parent;
			this.m_parent = value;
			if (parent != null)
			{
				parent.m_children.Remove(this);
			}
			if (this.m_parent != null)
			{
				this.m_parent.m_children.Add(this);
			}
			return parent;
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x00012C4C File Offset: 0x0001104C
		private void Awake()
		{
			RuntimeEditorApplication.IsOpenedChanged += this.OnEditorIsOpenedChanged;
			this.m_objectType = ExposeToEditorObjectType.Undefined;
			this.Init();
			this.m_hierarchyItem = base.gameObject.GetComponent<HierarchyItem>();
			if (this.m_hierarchyItem == null)
			{
				this.m_hierarchyItem = base.gameObject.AddComponent<HierarchyItem>();
			}
			if (base.hideFlags != HideFlags.HideAndDontSave && ExposeToEditor.Awaked != null)
			{
				ExposeToEditor.Awaked(this);
			}
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x00012CCC File Offset: 0x000110CC
		public void Init()
		{
			if (this.m_initialized)
			{
				return;
			}
			this.FindChildren(base.transform);
			this.m_initialized = true;
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x00012CF0 File Offset: 0x000110F0
		private void FindChildren(Transform parent)
		{
			IEnumerator enumerator = parent.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					ExposeToEditor component = transform.GetComponent<ExposeToEditor>();
					if (component == null)
					{
						this.FindChildren(transform);
					}
					else
					{
						component.m_parent = this;
						this.m_children.Add(component);
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}

		// Token: 0x060002BA RID: 698 RVA: 0x00012D7C File Offset: 0x0001117C
		private void OnEditorIsOpenedChanged()
		{
			if (RuntimeEditorApplication.IsOpened)
			{
				this.TryToAddColliders();
			}
			else
			{
				this.TryToDestroyColliders();
			}
		}

		// Token: 0x060002BB RID: 699 RVA: 0x00012D9C File Offset: 0x0001119C
		private void Start()
		{
			if (this.BoundsObject == null)
			{
				this.BoundsObject = base.gameObject;
			}
			this.m_effectiveBoundsType = this.BoundsType;
			this.m_filter = this.BoundsObject.GetComponent<MeshFilter>();
			this.m_skinned = this.BoundsObject.GetComponent<SkinnedMeshRenderer>();
			if (this.m_filter == null && this.m_skinned == null)
			{
				this.m_spriteRenderer = this.BoundsObject.GetComponent<SpriteRenderer>();
			}
			if (RuntimeEditorApplication.IsOpened)
			{
				this.TryToAddColliders();
			}
			else
			{
				this.TryToDestroyColliders();
				this.m_colliders = null;
			}
			if (base.hideFlags != HideFlags.HideAndDontSave && ExposeToEditor.Started != null)
			{
				ExposeToEditor.Started(this);
			}
		}

		// Token: 0x060002BC RID: 700 RVA: 0x00012E6B File Offset: 0x0001126B
		private void OnEnable()
		{
			if (base.hideFlags != HideFlags.HideAndDontSave && ExposeToEditor.Enabled != null)
			{
				ExposeToEditor.Enabled(this);
			}
		}

		// Token: 0x060002BD RID: 701 RVA: 0x00012E8F File Offset: 0x0001128F
		private void OnDisable()
		{
			if (base.hideFlags != HideFlags.HideAndDontSave && ExposeToEditor.Disabled != null)
			{
				ExposeToEditor.Disabled(this);
			}
		}

		// Token: 0x060002BE RID: 702 RVA: 0x00012EB4 File Offset: 0x000112B4
		private void OnDestroy()
		{
			RuntimeEditorApplication.IsOpenedChanged -= this.OnEditorIsOpenedChanged;
			if (!this.m_applicationQuit)
			{
				if (base.hideFlags != HideFlags.HideAndDontSave && ExposeToEditor.Destroying != null)
				{
					ExposeToEditor.Destroying(this);
				}
				if (this.m_parent != null)
				{
					this.ChangeParent(null);
				}
				this.TryToDestroyColliders();
				if (this.m_hierarchyItem != null)
				{
					UnityEngine.Object.Destroy(this.m_hierarchyItem);
				}
				if (base.hideFlags != HideFlags.HideAndDontSave && ExposeToEditor.Destroyed != null)
				{
					ExposeToEditor.Destroyed(this);
				}
			}
		}

		// Token: 0x060002BF RID: 703 RVA: 0x00012F5C File Offset: 0x0001135C
		private void OnApplicationQuit()
		{
			this.m_applicationQuit = true;
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x00012F68 File Offset: 0x00011368
		private void Update()
		{
			if (ExposeToEditor.TransformChanged != null && base.transform.hasChanged)
			{
				base.transform.hasChanged = false;
				if (base.hideFlags != HideFlags.HideAndDontSave && ExposeToEditor.TransformChanged != null)
				{
					ExposeToEditor.TransformChanged(this);
				}
			}
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x00012FC0 File Offset: 0x000113C0
		private void TryToAddColliders()
		{
			if (this == null)
			{
				return;
			}
			if (this.m_colliders == null || this.m_colliders.Length == 0)
			{
				List<Collider> list = new List<Collider>();
				Rigidbody component = base.gameObject.GetComponent<Rigidbody>();
				bool flag = component != null;
				if (this.m_effectiveBoundsType == BoundsType.Any)
				{
					if (this.m_filter != null)
					{
						if (this.AddColliders && !flag)
						{
							MeshCollider meshCollider = base.gameObject.AddComponent<MeshCollider>();
							meshCollider.convex = flag;
							meshCollider.sharedMesh = this.m_filter.sharedMesh;
							list.Add(meshCollider);
						}
					}
					else if (this.m_skinned != null)
					{
						if (this.AddColliders && !flag)
						{
							MeshCollider meshCollider2 = base.gameObject.AddComponent<MeshCollider>();
							meshCollider2.convex = flag;
							meshCollider2.sharedMesh = this.m_skinned.sharedMesh;
							list.Add(meshCollider2);
						}
					}
					else if (this.m_spriteRenderer != null && this.AddColliders && !flag)
					{
						BoxCollider boxCollider = base.gameObject.AddComponent<BoxCollider>();
						boxCollider.size = this.m_spriteRenderer.sprite.bounds.size;
						list.Add(boxCollider);
					}
				}
				else if (this.m_effectiveBoundsType == BoundsType.Mesh)
				{
					if (this.m_filter != null && this.AddColliders && !flag)
					{
						MeshCollider meshCollider3 = base.gameObject.AddComponent<MeshCollider>();
						meshCollider3.convex = flag;
						meshCollider3.sharedMesh = this.m_filter.sharedMesh;
						list.Add(meshCollider3);
					}
				}
				else if (this.m_effectiveBoundsType == BoundsType.SkinnedMesh)
				{
					if (this.m_skinned != null && this.AddColliders && !flag)
					{
						MeshCollider meshCollider4 = base.gameObject.AddComponent<MeshCollider>();
						meshCollider4.convex = flag;
						meshCollider4.sharedMesh = this.m_skinned.sharedMesh;
						list.Add(meshCollider4);
					}
				}
				else if (this.m_effectiveBoundsType == BoundsType.Sprite)
				{
					if (this.m_spriteRenderer != null && this.AddColliders && !flag)
					{
						BoxCollider boxCollider2 = base.gameObject.AddComponent<BoxCollider>();
						boxCollider2.size = this.m_spriteRenderer.sprite.bounds.size;
						list.Add(boxCollider2);
					}
				}
				else if (this.m_effectiveBoundsType == BoundsType.Custom && this.AddColliders && !flag)
				{
					Mesh sharedMesh = RuntimeGraphics.CreateCubeMesh(Color.black, this.CustomBounds.center, this.CustomBounds.extents.x * 2f, this.CustomBounds.extents.y * 2f, this.CustomBounds.extents.z * 2f, 1f);
					MeshCollider meshCollider5 = base.gameObject.AddComponent<MeshCollider>();
					meshCollider5.convex = flag;
					meshCollider5.sharedMesh = sharedMesh;
					list.Add(meshCollider5);
				}
				this.m_colliders = list.ToArray();
			}
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x00013300 File Offset: 0x00011700
		private void TryToDestroyColliders()
		{
			if (this.m_colliders != null)
			{
				for (int i = 0; i < this.m_colliders.Length; i++)
				{
					Collider collider = this.m_colliders[i];
					if (collider != null)
					{
						UnityEngine.Object.Destroy(collider);
					}
				}
				this.m_colliders = null;
			}
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x00013353 File Offset: 0x00011753
		public void SetName(string name)
		{
			base.gameObject.name = name;
			if (base.hideFlags != HideFlags.HideAndDontSave && ExposeToEditor.NameChanged != null)
			{
				ExposeToEditor.NameChanged(this);
			}
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x00013384 File Offset: 0x00011784
		private static bool IsExposedToEditor(GameObject go, ExposeToEditorObjectType type, bool roots)
		{
			ExposeToEditor component = go.GetComponent<ExposeToEditor>();
			return component != null && (!roots || component.transform.parent == null || component.transform.parent.GetComponentsInParent<ExposeToEditor>(true).Length == 0) && !component.MarkAsDestroyed && component.ObjectType == type && component.hideFlags != HideFlags.HideAndDontSave;
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x00013400 File Offset: 0x00011800
		public static IEnumerable<GameObject> FindAll(ExposeToEditorObjectType type, bool roots = true)
		{
			ExposeToEditor.<FindAll>c__AnonStorey0 <FindAll>c__AnonStorey = new ExposeToEditor.<FindAll>c__AnonStorey0();
			<FindAll>c__AnonStorey.type = type;
			<FindAll>c__AnonStorey.roots = roots;
			if (SceneManager.GetActiveScene().isLoaded)
			{
				return ExposeToEditor.FindAllUsingSceneManagement(<FindAll>c__AnonStorey.type, <FindAll>c__AnonStorey.roots);
			}
			List<GameObject> list = new List<GameObject>();
			foreach (GameObject gameObject in Resources.FindObjectsOfTypeAll<GameObject>())
			{
				if (!(gameObject == null))
				{
					if (!gameObject.IsPrefab())
					{
						list.Add(gameObject);
					}
				}
			}
			return list.Where(new Func<GameObject, bool>(<FindAll>c__AnonStorey.<>m__0));
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x000134A8 File Offset: 0x000118A8
		public static IEnumerable<GameObject> FindAllUsingSceneManagement(ExposeToEditorObjectType type, bool roots = true)
		{
			List<GameObject> list = new List<GameObject>();
			GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
			for (int i = 0; i < rootGameObjects.Length; i++)
			{
				foreach (ExposeToEditor exposeToEditor in rootGameObjects[i].GetComponentsInChildren<ExposeToEditor>(true))
				{
					if (ExposeToEditor.IsExposedToEditor(exposeToEditor.gameObject, type, roots) && !exposeToEditor.gameObject.IsPrefab())
					{
						list.Add(exposeToEditor.gameObject);
					}
				}
			}
			return list;
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0001353C File Offset: 0x0001193C
		// Note: this type is marked as 'beforefieldinit'.
		static ExposeToEditor()
		{
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x00013557 File Offset: 0x00011957
		[CompilerGenerated]
		private static bool <get_MarkedAsDestroyedChildCount>m__0(ExposeToEditor e)
		{
			return e.MarkAsDestroyed;
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0001355F File Offset: 0x0001195F
		[CompilerGenerated]
		private static int <GetChildren>m__1(ExposeToEditor c)
		{
			return c.transform.GetSiblingIndex();
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0001356C File Offset: 0x0001196C
		[CompilerGenerated]
		private static int <NextSibling>m__2(GameObject g)
		{
			return g.transform.GetSiblingIndex();
		}

		// Token: 0x04000371 RID: 881
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static ExposeToEditorEvent Awaked;

		// Token: 0x04000372 RID: 882
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static ExposeToEditorEvent Destroying;

		// Token: 0x04000373 RID: 883
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static ExposeToEditorEvent Destroyed;

		// Token: 0x04000374 RID: 884
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static ExposeToEditorEvent MarkAsDestroyedChanged;

		// Token: 0x04000375 RID: 885
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static ExposeToEditorEvent NameChanged;

		// Token: 0x04000376 RID: 886
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static ExposeToEditorEvent TransformChanged;

		// Token: 0x04000377 RID: 887
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static ExposeToEditorEvent Started;

		// Token: 0x04000378 RID: 888
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static ExposeToEditorEvent Enabled;

		// Token: 0x04000379 RID: 889
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static ExposeToEditorEvent Disabled;

		// Token: 0x0400037A RID: 890
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static ExposeToEditorChangeEvent<ExposeToEditor> ParentChanged;

		// Token: 0x0400037B RID: 891
		private bool m_applicationQuit;

		// Token: 0x0400037C RID: 892
		[SerializeField]
		[HideInInspector]
		private Collider[] m_colliders;

		// Token: 0x0400037D RID: 893
		private SpriteRenderer m_spriteRenderer;

		// Token: 0x0400037E RID: 894
		private MeshFilter m_filter;

		// Token: 0x0400037F RID: 895
		private SkinnedMeshRenderer m_skinned;

		// Token: 0x04000380 RID: 896
		private static readonly Bounds m_none = default(Bounds);

		// Token: 0x04000381 RID: 897
		public ExposeToEditorUnityEvent Selected;

		// Token: 0x04000382 RID: 898
		public ExposeToEditorUnityEvent Unselected;

		// Token: 0x04000383 RID: 899
		public GameObject BoundsObject;

		// Token: 0x04000384 RID: 900
		public BoundsType BoundsType;

		// Token: 0x04000385 RID: 901
		public Bounds CustomBounds;

		// Token: 0x04000386 RID: 902
		[HideInInspector]
		public bool CanSelect = true;

		// Token: 0x04000387 RID: 903
		[HideInInspector]
		public bool CanSnap = true;

		// Token: 0x04000388 RID: 904
		public bool AddColliders = true;

		// Token: 0x04000389 RID: 905
		[SerializeField]
		[HideInInspector]
		private ExposeToEditorObjectType m_objectType;

		// Token: 0x0400038A RID: 906
		private bool m_markAsDestroyed;

		// Token: 0x0400038B RID: 907
		private BoundsType m_effectiveBoundsType;

		// Token: 0x0400038C RID: 908
		private HierarchyItem m_hierarchyItem;

		// Token: 0x0400038D RID: 909
		private List<ExposeToEditor> m_children = new List<ExposeToEditor>();

		// Token: 0x0400038E RID: 910
		private ExposeToEditor m_parent;

		// Token: 0x0400038F RID: 911
		private bool m_initialized;

		// Token: 0x04000390 RID: 912
		[CompilerGenerated]
		private static Func<ExposeToEditor, bool> <>f__am$cache0;

		// Token: 0x04000391 RID: 913
		[CompilerGenerated]
		private static Func<ExposeToEditor, int> <>f__am$cache1;

		// Token: 0x04000392 RID: 914
		[CompilerGenerated]
		private static Func<GameObject, int> <>f__am$cache2;

		// Token: 0x02000EA1 RID: 3745
		[CompilerGenerated]
		private sealed class <FindAll>c__AnonStorey0
		{
			// Token: 0x0600715D RID: 29021 RVA: 0x00013579 File Offset: 0x00011979
			public <FindAll>c__AnonStorey0()
			{
			}

			// Token: 0x0600715E RID: 29022 RVA: 0x00013581 File Offset: 0x00011981
			internal bool <>m__0(GameObject f)
			{
				return ExposeToEditor.IsExposedToEditor(f, this.type, this.roots);
			}

			// Token: 0x04006533 RID: 25907
			internal ExposeToEditorObjectType type;

			// Token: 0x04006534 RID: 25908
			internal bool roots;
		}
	}
}
