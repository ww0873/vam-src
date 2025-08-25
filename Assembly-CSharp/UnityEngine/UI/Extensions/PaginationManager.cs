using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200054B RID: 1355
	[AddComponentMenu("UI/Extensions/Pagination Manager")]
	public class PaginationManager : ToggleGroup
	{
		// Token: 0x06002286 RID: 8838 RVA: 0x000C5244 File Offset: 0x000C3644
		protected PaginationManager()
		{
		}

		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x06002287 RID: 8839 RVA: 0x000C524C File Offset: 0x000C364C
		public int CurrentPage
		{
			get
			{
				return this.scrollSnap.CurrentPage;
			}
		}

		// Token: 0x06002288 RID: 8840 RVA: 0x000C525C File Offset: 0x000C365C
		protected override void Start()
		{
			base.Start();
			if (this.scrollSnap == null)
			{
				Debug.LogError("A ScrollSnap script must be attached");
				return;
			}
			if (this.scrollSnap.Pagination)
			{
				this.scrollSnap.Pagination = null;
			}
			this.scrollSnap.OnSelectionPageChangedEvent.AddListener(new UnityAction<int>(this.SetToggleGraphics));
			this.scrollSnap.OnSelectionChangeEndEvent.AddListener(new UnityAction<int>(this.OnPageChangeEnd));
			this.m_PaginationChildren = base.GetComponentsInChildren<Toggle>().ToList<Toggle>();
			for (int i = 0; i < this.m_PaginationChildren.Count; i++)
			{
				this.m_PaginationChildren[i].onValueChanged.AddListener(new UnityAction<bool>(this.ToggleClick));
				this.m_PaginationChildren[i].group = this;
				this.m_PaginationChildren[i].isOn = false;
			}
			this.SetToggleGraphics(this.CurrentPage);
			if (this.m_PaginationChildren.Count != this.scrollSnap._scroll_rect.content.childCount)
			{
				Debug.LogWarning("Uneven pagination icon to page count");
			}
		}

		// Token: 0x06002289 RID: 8841 RVA: 0x000C5392 File Offset: 0x000C3792
		public void GoToScreen(int pageNo)
		{
			this.scrollSnap.GoToScreen(pageNo);
		}

		// Token: 0x0600228A RID: 8842 RVA: 0x000C53A0 File Offset: 0x000C37A0
		private void ToggleClick(Toggle target)
		{
			if (!target.isOn)
			{
				this.isAClick = true;
				this.GoToScreen(this.m_PaginationChildren.IndexOf(target));
			}
		}

		// Token: 0x0600228B RID: 8843 RVA: 0x000C53C8 File Offset: 0x000C37C8
		private void ToggleClick(bool toggle)
		{
			if (toggle)
			{
				for (int i = 0; i < this.m_PaginationChildren.Count; i++)
				{
					if (this.m_PaginationChildren[i].isOn)
					{
						this.GoToScreen(i);
						break;
					}
				}
			}
		}

		// Token: 0x0600228C RID: 8844 RVA: 0x000C5419 File Offset: 0x000C3819
		private void ToggleClick(int target)
		{
			this.isAClick = true;
			this.GoToScreen(target);
		}

		// Token: 0x0600228D RID: 8845 RVA: 0x000C5429 File Offset: 0x000C3829
		private void SetToggleGraphics(int pageNo)
		{
			if (!this.isAClick)
			{
				this.m_PaginationChildren[pageNo].isOn = true;
			}
		}

		// Token: 0x0600228E RID: 8846 RVA: 0x000C5448 File Offset: 0x000C3848
		private void OnPageChangeEnd(int pageNo)
		{
			this.isAClick = false;
		}

		// Token: 0x04001C97 RID: 7319
		private List<Toggle> m_PaginationChildren;

		// Token: 0x04001C98 RID: 7320
		[SerializeField]
		private ScrollSnapBase scrollSnap;

		// Token: 0x04001C99 RID: 7321
		private bool isAClick;
	}
}
