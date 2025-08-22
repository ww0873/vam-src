using System;
using System.Runtime.CompilerServices;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000550 RID: 1360
	[AddComponentMenu("UI/Extensions/ScrollRectEx")]
	public class ScrollRectEx : ScrollRect
	{
		// Token: 0x0600229C RID: 8860 RVA: 0x000C56DE File Offset: 0x000C3ADE
		public ScrollRectEx()
		{
		}

		// Token: 0x0600229D RID: 8861 RVA: 0x000C56E8 File Offset: 0x000C3AE8
		private void DoForParents<T>(Action<T> action) where T : IEventSystemHandler
		{
			Transform parent = base.transform.parent;
			while (parent != null)
			{
				foreach (Component component in parent.GetComponents<Component>())
				{
					if (component is T)
					{
						action((T)((object)((IEventSystemHandler)component)));
					}
				}
				parent = parent.parent;
			}
		}

		// Token: 0x0600229E RID: 8862 RVA: 0x000C5754 File Offset: 0x000C3B54
		public override void OnInitializePotentialDrag(PointerEventData eventData)
		{
			ScrollRectEx.<OnInitializePotentialDrag>c__AnonStorey0 <OnInitializePotentialDrag>c__AnonStorey = new ScrollRectEx.<OnInitializePotentialDrag>c__AnonStorey0();
			<OnInitializePotentialDrag>c__AnonStorey.eventData = eventData;
			this.DoForParents<IInitializePotentialDragHandler>(new Action<IInitializePotentialDragHandler>(<OnInitializePotentialDrag>c__AnonStorey.<>m__0));
			base.OnInitializePotentialDrag(<OnInitializePotentialDrag>c__AnonStorey.eventData);
		}

		// Token: 0x0600229F RID: 8863 RVA: 0x000C578C File Offset: 0x000C3B8C
		public override void OnDrag(PointerEventData eventData)
		{
			ScrollRectEx.<OnDrag>c__AnonStorey1 <OnDrag>c__AnonStorey = new ScrollRectEx.<OnDrag>c__AnonStorey1();
			<OnDrag>c__AnonStorey.eventData = eventData;
			if (this.routeToParent)
			{
				this.DoForParents<IDragHandler>(new Action<IDragHandler>(<OnDrag>c__AnonStorey.<>m__0));
			}
			else
			{
				base.OnDrag(<OnDrag>c__AnonStorey.eventData);
			}
		}

		// Token: 0x060022A0 RID: 8864 RVA: 0x000C57D4 File Offset: 0x000C3BD4
		public override void OnBeginDrag(PointerEventData eventData)
		{
			ScrollRectEx.<OnBeginDrag>c__AnonStorey2 <OnBeginDrag>c__AnonStorey = new ScrollRectEx.<OnBeginDrag>c__AnonStorey2();
			<OnBeginDrag>c__AnonStorey.eventData = eventData;
			if (!base.horizontal && Math.Abs(<OnBeginDrag>c__AnonStorey.eventData.delta.x) > Math.Abs(<OnBeginDrag>c__AnonStorey.eventData.delta.y))
			{
				this.routeToParent = true;
			}
			else if (!base.vertical && Math.Abs(<OnBeginDrag>c__AnonStorey.eventData.delta.x) < Math.Abs(<OnBeginDrag>c__AnonStorey.eventData.delta.y))
			{
				this.routeToParent = true;
			}
			else
			{
				this.routeToParent = false;
			}
			if (this.routeToParent)
			{
				this.DoForParents<IBeginDragHandler>(new Action<IBeginDragHandler>(<OnBeginDrag>c__AnonStorey.<>m__0));
			}
			else
			{
				base.OnBeginDrag(<OnBeginDrag>c__AnonStorey.eventData);
			}
		}

		// Token: 0x060022A1 RID: 8865 RVA: 0x000C58BC File Offset: 0x000C3CBC
		public override void OnEndDrag(PointerEventData eventData)
		{
			ScrollRectEx.<OnEndDrag>c__AnonStorey3 <OnEndDrag>c__AnonStorey = new ScrollRectEx.<OnEndDrag>c__AnonStorey3();
			<OnEndDrag>c__AnonStorey.eventData = eventData;
			if (this.routeToParent)
			{
				this.DoForParents<IEndDragHandler>(new Action<IEndDragHandler>(<OnEndDrag>c__AnonStorey.<>m__0));
			}
			else
			{
				base.OnEndDrag(<OnEndDrag>c__AnonStorey.eventData);
			}
			this.routeToParent = false;
		}

		// Token: 0x060022A2 RID: 8866 RVA: 0x000C590C File Offset: 0x000C3D0C
		public override void OnScroll(PointerEventData eventData)
		{
			ScrollRectEx.<OnScroll>c__AnonStorey4 <OnScroll>c__AnonStorey = new ScrollRectEx.<OnScroll>c__AnonStorey4();
			<OnScroll>c__AnonStorey.eventData = eventData;
			if (!base.horizontal && Math.Abs(<OnScroll>c__AnonStorey.eventData.scrollDelta.x) > Math.Abs(<OnScroll>c__AnonStorey.eventData.scrollDelta.y))
			{
				this.routeToParent = true;
			}
			else if (!base.vertical && Math.Abs(<OnScroll>c__AnonStorey.eventData.scrollDelta.x) < Math.Abs(<OnScroll>c__AnonStorey.eventData.scrollDelta.y))
			{
				this.routeToParent = true;
			}
			else
			{
				this.routeToParent = false;
			}
			if (this.routeToParent)
			{
				this.DoForParents<IScrollHandler>(new Action<IScrollHandler>(<OnScroll>c__AnonStorey.<>m__0));
			}
			else
			{
				base.OnScroll(<OnScroll>c__AnonStorey.eventData);
			}
		}

		// Token: 0x04001CA3 RID: 7331
		private bool routeToParent;

		// Token: 0x02000F75 RID: 3957
		[CompilerGenerated]
		private sealed class <OnInitializePotentialDrag>c__AnonStorey0
		{
			// Token: 0x060073EB RID: 29675 RVA: 0x000C59F4 File Offset: 0x000C3DF4
			public <OnInitializePotentialDrag>c__AnonStorey0()
			{
			}

			// Token: 0x060073EC RID: 29676 RVA: 0x000C59FC File Offset: 0x000C3DFC
			internal void <>m__0(IInitializePotentialDragHandler parent)
			{
				parent.OnInitializePotentialDrag(this.eventData);
			}

			// Token: 0x04006805 RID: 26629
			internal PointerEventData eventData;
		}

		// Token: 0x02000F76 RID: 3958
		[CompilerGenerated]
		private sealed class <OnDrag>c__AnonStorey1
		{
			// Token: 0x060073ED RID: 29677 RVA: 0x000C5A0A File Offset: 0x000C3E0A
			public <OnDrag>c__AnonStorey1()
			{
			}

			// Token: 0x060073EE RID: 29678 RVA: 0x000C5A12 File Offset: 0x000C3E12
			internal void <>m__0(IDragHandler parent)
			{
				parent.OnDrag(this.eventData);
			}

			// Token: 0x04006806 RID: 26630
			internal PointerEventData eventData;
		}

		// Token: 0x02000F77 RID: 3959
		[CompilerGenerated]
		private sealed class <OnBeginDrag>c__AnonStorey2
		{
			// Token: 0x060073EF RID: 29679 RVA: 0x000C5A20 File Offset: 0x000C3E20
			public <OnBeginDrag>c__AnonStorey2()
			{
			}

			// Token: 0x060073F0 RID: 29680 RVA: 0x000C5A28 File Offset: 0x000C3E28
			internal void <>m__0(IBeginDragHandler parent)
			{
				parent.OnBeginDrag(this.eventData);
			}

			// Token: 0x04006807 RID: 26631
			internal PointerEventData eventData;
		}

		// Token: 0x02000F78 RID: 3960
		[CompilerGenerated]
		private sealed class <OnEndDrag>c__AnonStorey3
		{
			// Token: 0x060073F1 RID: 29681 RVA: 0x000C5A36 File Offset: 0x000C3E36
			public <OnEndDrag>c__AnonStorey3()
			{
			}

			// Token: 0x060073F2 RID: 29682 RVA: 0x000C5A3E File Offset: 0x000C3E3E
			internal void <>m__0(IEndDragHandler parent)
			{
				parent.OnEndDrag(this.eventData);
			}

			// Token: 0x04006808 RID: 26632
			internal PointerEventData eventData;
		}

		// Token: 0x02000F79 RID: 3961
		[CompilerGenerated]
		private sealed class <OnScroll>c__AnonStorey4
		{
			// Token: 0x060073F3 RID: 29683 RVA: 0x000C5A4C File Offset: 0x000C3E4C
			public <OnScroll>c__AnonStorey4()
			{
			}

			// Token: 0x060073F4 RID: 29684 RVA: 0x000C5A54 File Offset: 0x000C3E54
			internal void <>m__0(IScrollHandler parent)
			{
				parent.OnScroll(this.eventData);
			}

			// Token: 0x04006809 RID: 26633
			internal PointerEventData eventData;
		}
	}
}
