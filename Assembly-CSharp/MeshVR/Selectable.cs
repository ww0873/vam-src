using System;
using UnityEngine;

namespace MeshVR
{
	// Token: 0x02000C49 RID: 3145
	public class Selectable : MonoBehaviour
	{
		// Token: 0x06005B85 RID: 23429 RVA: 0x0021A113 File Offset: 0x00218513
		public Selectable()
		{
		}

		// Token: 0x17000D6F RID: 3439
		// (get) Token: 0x06005B86 RID: 23430 RVA: 0x0021A11B File Offset: 0x0021851B
		// (set) Token: 0x06005B87 RID: 23431 RVA: 0x0021A124 File Offset: 0x00218524
		public bool isSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (this._isSelected != value)
				{
					this._isSelected = value;
					Renderer componentInChildren = base.GetComponentInChildren<Renderer>();
					if (componentInChildren != null)
					{
						componentInChildren.material.color = ((!value) ? Color.red : Color.white);
					}
					if (this.selectionChanged != null)
					{
						this.selectionChanged(this.id, this._isSelected);
					}
				}
			}
		}

		// Token: 0x17000D70 RID: 3440
		// (get) Token: 0x06005B88 RID: 23432 RVA: 0x0021A199 File Offset: 0x00218599
		// (set) Token: 0x06005B89 RID: 23433 RVA: 0x0021A1A4 File Offset: 0x002185A4
		public bool isHidden
		{
			get
			{
				return this._isHidden;
			}
			set
			{
				if (this._isHidden != value)
				{
					this._isHidden = value;
					if (this.render != null)
					{
						this.render.enabled = !this._isHidden;
					}
					if (this.collide != null)
					{
						this.collide.enabled = !this._isHidden;
					}
				}
			}
		}

		// Token: 0x06005B8A RID: 23434 RVA: 0x0021A20E File Offset: 0x0021860E
		private void Awake()
		{
			this.render = base.GetComponent<Renderer>();
			this.collide = base.GetComponent<Collider>();
		}

		// Token: 0x06005B8B RID: 23435 RVA: 0x0021A228 File Offset: 0x00218628
		private void OnEnable()
		{
			Selector.selectables.Add(this);
		}

		// Token: 0x06005B8C RID: 23436 RVA: 0x0021A236 File Offset: 0x00218636
		private void OnDisable()
		{
			Selector.selectables.Remove(this);
		}

		// Token: 0x06005B8D RID: 23437 RVA: 0x0021A244 File Offset: 0x00218644
		private void OnDestroy()
		{
			this.selectionChanged = null;
		}

		// Token: 0x06005B8E RID: 23438 RVA: 0x0021A250 File Offset: 0x00218650
		private void OnTriggerEnter(Collider other)
		{
			SelectableSelect component = other.GetComponent<SelectableSelect>();
			if (component != null && component.enabled)
			{
				this.isSelected = true;
			}
			else
			{
				SelectableUnselect component2 = other.GetComponent<SelectableUnselect>();
				if (component2 != null && component2.enabled)
				{
					this.isSelected = false;
				}
			}
		}

		// Token: 0x04004B7B RID: 19323
		public Selectable.SelectionChanged selectionChanged;

		// Token: 0x04004B7C RID: 19324
		public int id;

		// Token: 0x04004B7D RID: 19325
		private bool _isSelected;

		// Token: 0x04004B7E RID: 19326
		private bool _isHidden;

		// Token: 0x04004B7F RID: 19327
		private Renderer render;

		// Token: 0x04004B80 RID: 19328
		private Collider collide;

		// Token: 0x02000C4A RID: 3146
		// (Invoke) Token: 0x06005B90 RID: 23440
		public delegate void SelectionChanged(int uid, bool b);
	}
}
