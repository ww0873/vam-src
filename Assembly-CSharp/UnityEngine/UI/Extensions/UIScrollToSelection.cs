using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200055F RID: 1375
	[RequireComponent(typeof(ScrollRect))]
	[AddComponentMenu("UI/Extensions/UIScrollToSelection")]
	public class UIScrollToSelection : MonoBehaviour
	{
		// Token: 0x060022E7 RID: 8935 RVA: 0x000C7766 File Offset: 0x000C5B66
		public UIScrollToSelection()
		{
		}

		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x060022E8 RID: 8936 RVA: 0x000C7784 File Offset: 0x000C5B84
		protected RectTransform LayoutListGroup
		{
			get
			{
				return (!(this.TargetScrollRect != null)) ? null : this.TargetScrollRect.content;
			}
		}

		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x060022E9 RID: 8937 RVA: 0x000C77A8 File Offset: 0x000C5BA8
		protected UIScrollToSelection.ScrollType ScrollDirection
		{
			get
			{
				return this.scrollDirection;
			}
		}

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x060022EA RID: 8938 RVA: 0x000C77B0 File Offset: 0x000C5BB0
		protected float ScrollSpeed
		{
			get
			{
				return this.scrollSpeed;
			}
		}

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x060022EB RID: 8939 RVA: 0x000C77B8 File Offset: 0x000C5BB8
		protected bool CancelScrollOnInput
		{
			get
			{
				return this.cancelScrollOnInput;
			}
		}

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x060022EC RID: 8940 RVA: 0x000C77C0 File Offset: 0x000C5BC0
		protected List<KeyCode> CancelScrollKeycodes
		{
			get
			{
				return this.cancelScrollKeycodes;
			}
		}

		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x060022ED RID: 8941 RVA: 0x000C77C8 File Offset: 0x000C5BC8
		// (set) Token: 0x060022EE RID: 8942 RVA: 0x000C77D0 File Offset: 0x000C5BD0
		protected RectTransform ScrollWindow
		{
			[CompilerGenerated]
			get
			{
				return this.<ScrollWindow>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ScrollWindow>k__BackingField = value;
			}
		}

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x060022EF RID: 8943 RVA: 0x000C77D9 File Offset: 0x000C5BD9
		// (set) Token: 0x060022F0 RID: 8944 RVA: 0x000C77E1 File Offset: 0x000C5BE1
		protected ScrollRect TargetScrollRect
		{
			[CompilerGenerated]
			get
			{
				return this.<TargetScrollRect>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<TargetScrollRect>k__BackingField = value;
			}
		}

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x060022F1 RID: 8945 RVA: 0x000C77EA File Offset: 0x000C5BEA
		protected EventSystem CurrentEventSystem
		{
			get
			{
				return EventSystem.current;
			}
		}

		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x060022F2 RID: 8946 RVA: 0x000C77F1 File Offset: 0x000C5BF1
		// (set) Token: 0x060022F3 RID: 8947 RVA: 0x000C77F9 File Offset: 0x000C5BF9
		protected GameObject LastCheckedGameObject
		{
			[CompilerGenerated]
			get
			{
				return this.<LastCheckedGameObject>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<LastCheckedGameObject>k__BackingField = value;
			}
		}

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x060022F4 RID: 8948 RVA: 0x000C7802 File Offset: 0x000C5C02
		protected GameObject CurrentSelectedGameObject
		{
			get
			{
				return EventSystem.current.currentSelectedGameObject;
			}
		}

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x060022F5 RID: 8949 RVA: 0x000C780E File Offset: 0x000C5C0E
		// (set) Token: 0x060022F6 RID: 8950 RVA: 0x000C7816 File Offset: 0x000C5C16
		protected RectTransform CurrentTargetRectTransform
		{
			[CompilerGenerated]
			get
			{
				return this.<CurrentTargetRectTransform>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<CurrentTargetRectTransform>k__BackingField = value;
			}
		}

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x060022F7 RID: 8951 RVA: 0x000C781F File Offset: 0x000C5C1F
		// (set) Token: 0x060022F8 RID: 8952 RVA: 0x000C7827 File Offset: 0x000C5C27
		protected bool IsManualScrollingAvailable
		{
			[CompilerGenerated]
			get
			{
				return this.<IsManualScrollingAvailable>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<IsManualScrollingAvailable>k__BackingField = value;
			}
		}

		// Token: 0x060022F9 RID: 8953 RVA: 0x000C7830 File Offset: 0x000C5C30
		protected virtual void Awake()
		{
			this.TargetScrollRect = base.GetComponent<ScrollRect>();
			this.ScrollWindow = this.TargetScrollRect.GetComponent<RectTransform>();
		}

		// Token: 0x060022FA RID: 8954 RVA: 0x000C784F File Offset: 0x000C5C4F
		protected virtual void Start()
		{
		}

		// Token: 0x060022FB RID: 8955 RVA: 0x000C7851 File Offset: 0x000C5C51
		protected virtual void Update()
		{
			this.UpdateReferences();
			this.CheckIfScrollingShouldBeLocked();
			this.ScrollRectToLevelSelection();
		}

		// Token: 0x060022FC RID: 8956 RVA: 0x000C7868 File Offset: 0x000C5C68
		private void UpdateReferences()
		{
			if (this.CurrentSelectedGameObject != this.LastCheckedGameObject)
			{
				this.CurrentTargetRectTransform = ((!(this.CurrentSelectedGameObject != null)) ? null : this.CurrentSelectedGameObject.GetComponent<RectTransform>());
				if (this.CurrentSelectedGameObject != null && this.CurrentSelectedGameObject.transform.parent == this.LayoutListGroup.transform)
				{
					this.IsManualScrollingAvailable = false;
				}
			}
			this.LastCheckedGameObject = this.CurrentSelectedGameObject;
		}

		// Token: 0x060022FD RID: 8957 RVA: 0x000C78FC File Offset: 0x000C5CFC
		private void CheckIfScrollingShouldBeLocked()
		{
			if (!this.CancelScrollOnInput || this.IsManualScrollingAvailable)
			{
				return;
			}
			for (int i = 0; i < this.CancelScrollKeycodes.Count; i++)
			{
				if (Input.GetKeyDown(this.CancelScrollKeycodes[i]))
				{
					this.IsManualScrollingAvailable = true;
					break;
				}
			}
		}

		// Token: 0x060022FE RID: 8958 RVA: 0x000C7960 File Offset: 0x000C5D60
		private void ScrollRectToLevelSelection()
		{
			if (this.TargetScrollRect == null || this.LayoutListGroup == null || this.ScrollWindow == null || this.IsManualScrollingAvailable)
			{
				return;
			}
			RectTransform currentTargetRectTransform = this.CurrentTargetRectTransform;
			if (currentTargetRectTransform == null || currentTargetRectTransform.transform.parent != this.LayoutListGroup.transform)
			{
				return;
			}
			UIScrollToSelection.ScrollType scrollType = this.ScrollDirection;
			if (scrollType != UIScrollToSelection.ScrollType.VERTICAL)
			{
				if (scrollType != UIScrollToSelection.ScrollType.HORIZONTAL)
				{
					if (scrollType == UIScrollToSelection.ScrollType.BOTH)
					{
						this.UpdateVerticalScrollPosition(currentTargetRectTransform);
						this.UpdateHorizontalScrollPosition(currentTargetRectTransform);
					}
				}
				else
				{
					this.UpdateHorizontalScrollPosition(currentTargetRectTransform);
				}
			}
			else
			{
				this.UpdateVerticalScrollPosition(currentTargetRectTransform);
			}
		}

		// Token: 0x060022FF RID: 8959 RVA: 0x000C7A30 File Offset: 0x000C5E30
		private void UpdateVerticalScrollPosition(RectTransform selection)
		{
			float position = -selection.anchoredPosition.y - selection.rect.height * (1f - selection.pivot.y);
			float height = selection.rect.height;
			float height2 = this.ScrollWindow.rect.height;
			float y = this.LayoutListGroup.anchoredPosition.y;
			float scrollOffset = this.GetScrollOffset(position, y, height, height2);
			this.TargetScrollRect.verticalNormalizedPosition += scrollOffset / this.LayoutListGroup.rect.height * Time.unscaledDeltaTime * this.scrollSpeed;
		}

		// Token: 0x06002300 RID: 8960 RVA: 0x000C7AF4 File Offset: 0x000C5EF4
		private void UpdateHorizontalScrollPosition(RectTransform selection)
		{
			float position = -selection.anchoredPosition.x - selection.rect.width * (1f - selection.pivot.x);
			float width = selection.rect.width;
			float width2 = this.ScrollWindow.rect.width;
			float listAnchorPosition = -this.LayoutListGroup.anchoredPosition.x;
			float num = -this.GetScrollOffset(position, listAnchorPosition, width, width2);
			this.TargetScrollRect.horizontalNormalizedPosition += num / this.LayoutListGroup.rect.width * Time.unscaledDeltaTime * this.scrollSpeed;
		}

		// Token: 0x06002301 RID: 8961 RVA: 0x000C7BB9 File Offset: 0x000C5FB9
		private float GetScrollOffset(float position, float listAnchorPosition, float targetLength, float maskLength)
		{
			if (position < listAnchorPosition + targetLength / 2f)
			{
				return listAnchorPosition + maskLength - (position - targetLength);
			}
			if (position + targetLength > listAnchorPosition + maskLength)
			{
				return listAnchorPosition + maskLength - (position + targetLength);
			}
			return 0f;
		}

		// Token: 0x04001CE8 RID: 7400
		[Header("[ Settings ]")]
		[SerializeField]
		private UIScrollToSelection.ScrollType scrollDirection;

		// Token: 0x04001CE9 RID: 7401
		[SerializeField]
		private float scrollSpeed = 10f;

		// Token: 0x04001CEA RID: 7402
		[Header("[ Input ]")]
		[SerializeField]
		private bool cancelScrollOnInput;

		// Token: 0x04001CEB RID: 7403
		[SerializeField]
		private List<KeyCode> cancelScrollKeycodes = new List<KeyCode>();

		// Token: 0x04001CEC RID: 7404
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private RectTransform <ScrollWindow>k__BackingField;

		// Token: 0x04001CED RID: 7405
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private ScrollRect <TargetScrollRect>k__BackingField;

		// Token: 0x04001CEE RID: 7406
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GameObject <LastCheckedGameObject>k__BackingField;

		// Token: 0x04001CEF RID: 7407
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private RectTransform <CurrentTargetRectTransform>k__BackingField;

		// Token: 0x04001CF0 RID: 7408
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool <IsManualScrollingAvailable>k__BackingField;

		// Token: 0x02000560 RID: 1376
		public enum ScrollType
		{
			// Token: 0x04001CF2 RID: 7410
			VERTICAL,
			// Token: 0x04001CF3 RID: 7411
			HORIZONTAL,
			// Token: 0x04001CF4 RID: 7412
			BOTH
		}
	}
}
