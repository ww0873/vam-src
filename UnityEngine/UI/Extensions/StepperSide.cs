using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020004E2 RID: 1250
	[RequireComponent(typeof(Selectable))]
	public class StepperSide : UIBehaviour, IPointerClickHandler, ISubmitHandler, IEventSystemHandler
	{
		// Token: 0x06001FA7 RID: 8103 RVA: 0x000B3914 File Offset: 0x000B1D14
		protected StepperSide()
		{
		}

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x06001FA8 RID: 8104 RVA: 0x000B391C File Offset: 0x000B1D1C
		private Selectable button
		{
			get
			{
				return base.GetComponent<Selectable>();
			}
		}

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x06001FA9 RID: 8105 RVA: 0x000B3924 File Offset: 0x000B1D24
		private Stepper stepper
		{
			get
			{
				return base.GetComponentInParent<Stepper>();
			}
		}

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x06001FAA RID: 8106 RVA: 0x000B392C File Offset: 0x000B1D2C
		private bool leftmost
		{
			get
			{
				return this.button == this.stepper.sides[0];
			}
		}

		// Token: 0x06001FAB RID: 8107 RVA: 0x000B3946 File Offset: 0x000B1D46
		public virtual void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			this.Press();
		}

		// Token: 0x06001FAC RID: 8108 RVA: 0x000B395A File Offset: 0x000B1D5A
		public virtual void OnSubmit(BaseEventData eventData)
		{
			this.Press();
		}

		// Token: 0x06001FAD RID: 8109 RVA: 0x000B3964 File Offset: 0x000B1D64
		private void Press()
		{
			if (!this.button.IsActive() || !this.button.IsInteractable())
			{
				return;
			}
			if (this.leftmost)
			{
				this.stepper.StepDown();
			}
			else
			{
				this.stepper.StepUp();
			}
		}
	}
}
