using System;
using UnityEngine;

namespace Battlehub.RTCommon
{
	// Token: 0x020000B7 RID: 183
	public class HierarchyItem : MonoBehaviour
	{
		// Token: 0x060002F7 RID: 759 RVA: 0x00013C94 File Offset: 0x00012094
		public HierarchyItem()
		{
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x00013C9C File Offset: 0x0001209C
		private void Awake()
		{
			this.m_exposeToEditor = base.GetComponent<ExposeToEditor>();
			if (base.transform.parent != null)
			{
				this.m_parentExp = this.CreateChainToParent(base.transform.parent);
				this.m_parentTransform = base.transform.parent;
			}
			this.m_isAwaked = true;
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x00013CFC File Offset: 0x000120FC
		private ExposeToEditor CreateChainToParent(Transform parent)
		{
			ExposeToEditor exposeToEditor = null;
			if (parent != null)
			{
				exposeToEditor = parent.GetComponentInParent<ExposeToEditor>();
			}
			if (exposeToEditor == null)
			{
				return null;
			}
			while (parent != null && parent.gameObject != exposeToEditor.gameObject)
			{
				if (!parent.GetComponent<ExposeToEditor>() && !parent.GetComponent<HierarchyItem>())
				{
					parent.gameObject.AddComponent<HierarchyItem>();
				}
				parent = parent.parent;
			}
			return exposeToEditor;
		}

		// Token: 0x060002FA RID: 762 RVA: 0x00013D88 File Offset: 0x00012188
		private void TryDestroyChainToParent(Transform parent, ExposeToEditor parentExp)
		{
			if (parentExp == null)
			{
				return;
			}
			while (parent != null && parent.gameObject != parentExp.gameObject)
			{
				if (!parent.GetComponent<ExposeToEditor>())
				{
					HierarchyItem component = parent.GetComponent<HierarchyItem>();
					if (component && !this.HasExposeToEditorChildren(parent))
					{
						UnityEngine.Object.Destroy(component);
					}
				}
				parent = parent.parent;
			}
		}

		// Token: 0x060002FB RID: 763 RVA: 0x00013E08 File Offset: 0x00012208
		private bool HasExposeToEditorChildren(Transform parentTransform)
		{
			int childCount = parentTransform.childCount;
			if (childCount == 0)
			{
				return false;
			}
			for (int i = 0; i < childCount; i++)
			{
				Transform child = parentTransform.GetChild(i);
				ExposeToEditor component = child.GetComponent<ExposeToEditor>();
				if (component != null)
				{
					return true;
				}
				HierarchyItem component2 = child.GetComponent<HierarchyItem>();
				if (component2 != null && this.HasExposeToEditorChildren(child))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060002FC RID: 764 RVA: 0x00013E78 File Offset: 0x00012278
		private void UpdateChildren(Transform parentTransform, ExposeToEditor parentExp)
		{
			int childCount = parentTransform.childCount;
			if (childCount == 0)
			{
				return;
			}
			for (int i = 0; i < childCount; i++)
			{
				Transform child = parentTransform.GetChild(i);
				ExposeToEditor component = child.GetComponent<ExposeToEditor>();
				HierarchyItem component2 = child.GetComponent<HierarchyItem>();
				if (component != null)
				{
					component.Parent = parentExp;
					component2.m_parentExp = parentExp;
				}
				else if (component2 != null)
				{
					this.UpdateChildren(child, parentExp);
				}
			}
		}

		// Token: 0x060002FD RID: 765 RVA: 0x00013EF4 File Offset: 0x000122F4
		private void OnTransformParentChanged()
		{
			if (!this.m_isAwaked)
			{
				return;
			}
			if (base.transform.parent != this.m_parentTransform)
			{
				if (this.m_parentTransform != null && this.m_parentExp != null)
				{
					this.TryDestroyChainToParent(this.m_parentTransform, this.m_parentExp);
				}
				ExposeToEditor exposeToEditor = this.CreateChainToParent(base.transform.parent);
				if (exposeToEditor != this.m_parentExp)
				{
					if (this.m_exposeToEditor == null)
					{
						this.UpdateChildren(base.transform, exposeToEditor);
					}
					else
					{
						this.m_exposeToEditor.Parent = exposeToEditor;
					}
					this.m_parentExp = exposeToEditor;
				}
				this.m_parentTransform = base.transform.parent;
			}
		}

		// Token: 0x040003A8 RID: 936
		private ExposeToEditor m_parentExp;

		// Token: 0x040003A9 RID: 937
		private ExposeToEditor m_exposeToEditor;

		// Token: 0x040003AA RID: 938
		private Transform m_parentTransform;

		// Token: 0x040003AB RID: 939
		private bool m_isAwaked;
	}
}
