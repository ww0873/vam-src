using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020004DB RID: 1243
	[RequireComponent(typeof(Selectable))]
	public class Segment : UIBehaviour, IPointerClickHandler, ISubmitHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, ISelectHandler, IDeselectHandler, IEventSystemHandler
	{
		// Token: 0x06001F56 RID: 8022 RVA: 0x000B255B File Offset: 0x000B095B
		protected Segment()
		{
		}

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x06001F57 RID: 8023 RVA: 0x000B2563 File Offset: 0x000B0963
		internal bool leftmost
		{
			get
			{
				return this.index == 0;
			}
		}

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x06001F58 RID: 8024 RVA: 0x000B256E File Offset: 0x000B096E
		internal bool rightmost
		{
			get
			{
				return this.index == this.segmentControl.segments.Length - 1;
			}
		}

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x06001F59 RID: 8025 RVA: 0x000B2587 File Offset: 0x000B0987
		// (set) Token: 0x06001F5A RID: 8026 RVA: 0x000B259F File Offset: 0x000B099F
		public bool selected
		{
			get
			{
				return this.segmentControl.selectedSegment == this.button;
			}
			set
			{
				this.SetSelected(value);
			}
		}

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x06001F5B RID: 8027 RVA: 0x000B25A8 File Offset: 0x000B09A8
		internal SegmentedControl segmentControl
		{
			get
			{
				return base.GetComponentInParent<SegmentedControl>();
			}
		}

		// Token: 0x17000393 RID: 915
		// (get) Token: 0x06001F5C RID: 8028 RVA: 0x000B25B0 File Offset: 0x000B09B0
		internal Selectable button
		{
			get
			{
				return base.GetComponent<Selectable>();
			}
		}

		// Token: 0x06001F5D RID: 8029 RVA: 0x000B25B8 File Offset: 0x000B09B8
		public virtual void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			this.selected = true;
		}

		// Token: 0x06001F5E RID: 8030 RVA: 0x000B25CD File Offset: 0x000B09CD
		public virtual void OnPointerEnter(PointerEventData eventData)
		{
			this.MaintainSelection();
		}

		// Token: 0x06001F5F RID: 8031 RVA: 0x000B25D5 File Offset: 0x000B09D5
		public virtual void OnPointerExit(PointerEventData eventData)
		{
			this.MaintainSelection();
		}

		// Token: 0x06001F60 RID: 8032 RVA: 0x000B25DD File Offset: 0x000B09DD
		public virtual void OnPointerDown(PointerEventData eventData)
		{
			this.MaintainSelection();
		}

		// Token: 0x06001F61 RID: 8033 RVA: 0x000B25E5 File Offset: 0x000B09E5
		public virtual void OnPointerUp(PointerEventData eventData)
		{
			this.MaintainSelection();
		}

		// Token: 0x06001F62 RID: 8034 RVA: 0x000B25ED File Offset: 0x000B09ED
		public virtual void OnSelect(BaseEventData eventData)
		{
			this.MaintainSelection();
		}

		// Token: 0x06001F63 RID: 8035 RVA: 0x000B25F5 File Offset: 0x000B09F5
		public virtual void OnDeselect(BaseEventData eventData)
		{
			this.MaintainSelection();
		}

		// Token: 0x06001F64 RID: 8036 RVA: 0x000B25FD File Offset: 0x000B09FD
		public virtual void OnSubmit(BaseEventData eventData)
		{
			this.selected = true;
		}

		// Token: 0x06001F65 RID: 8037 RVA: 0x000B2608 File Offset: 0x000B0A08
		private void SetSelected(bool value)
		{
			if (value && this.button.IsActive() && this.button.IsInteractable())
			{
				if (this.segmentControl.selectedSegment == this.button)
				{
					if (this.segmentControl.allowSwitchingOff)
					{
						this.Deselect();
					}
					else
					{
						this.MaintainSelection();
					}
				}
				else
				{
					if (this.segmentControl.selectedSegment)
					{
						Segment component = this.segmentControl.selectedSegment.GetComponent<Segment>();
						this.segmentControl.selectedSegment = null;
						component.TransitionButton();
					}
					this.segmentControl.selectedSegment = this.button;
					this.StoreTextColor();
					this.TransitionButton();
					this.segmentControl.onValueChanged.Invoke(this.index);
				}
			}
			else if (this.segmentControl.selectedSegment == this.button)
			{
				this.Deselect();
			}
		}

		// Token: 0x06001F66 RID: 8038 RVA: 0x000B270D File Offset: 0x000B0B0D
		private void Deselect()
		{
			this.segmentControl.selectedSegment = null;
			this.TransitionButton();
			this.segmentControl.onValueChanged.Invoke(-1);
		}

		// Token: 0x06001F67 RID: 8039 RVA: 0x000B2732 File Offset: 0x000B0B32
		private void MaintainSelection()
		{
			if (this.button != this.segmentControl.selectedSegment)
			{
				return;
			}
			this.TransitionButton(true);
		}

		// Token: 0x06001F68 RID: 8040 RVA: 0x000B2757 File Offset: 0x000B0B57
		internal void TransitionButton()
		{
			this.TransitionButton(false);
		}

		// Token: 0x06001F69 RID: 8041 RVA: 0x000B2760 File Offset: 0x000B0B60
		internal void TransitionButton(bool instant)
		{
			Color a = (!this.selected) ? this.button.colors.normalColor : this.segmentControl.selectedColor;
			Color a2 = (!this.selected) ? this.textColor : this.button.colors.normalColor;
			Sprite newSprite = (!this.selected) ? null : this.button.spriteState.pressedSprite;
			string triggername = (!this.selected) ? this.button.animationTriggers.normalTrigger : this.button.animationTriggers.pressedTrigger;
			Selectable.Transition transition = this.button.transition;
			if (transition != Selectable.Transition.ColorTint)
			{
				if (transition != Selectable.Transition.SpriteSwap)
				{
					if (transition == Selectable.Transition.Animation)
					{
						this.TriggerAnimation(triggername);
					}
				}
				else
				{
					this.DoSpriteSwap(newSprite);
				}
			}
			else
			{
				this.StartColorTween(a * this.button.colors.colorMultiplier, instant);
				this.ChangeTextColor(a2 * this.button.colors.colorMultiplier);
			}
		}

		// Token: 0x06001F6A RID: 8042 RVA: 0x000B28AC File Offset: 0x000B0CAC
		private void StartColorTween(Color targetColor, bool instant)
		{
			if (this.button.targetGraphic == null)
			{
				return;
			}
			this.button.targetGraphic.CrossFadeColor(targetColor, (!instant) ? this.button.colors.fadeDuration : 0f, true, true);
		}

		// Token: 0x06001F6B RID: 8043 RVA: 0x000B2908 File Offset: 0x000B0D08
		internal void StoreTextColor()
		{
			Text componentInChildren = base.GetComponentInChildren<Text>();
			if (!componentInChildren)
			{
				return;
			}
			this.textColor = componentInChildren.color;
		}

		// Token: 0x06001F6C RID: 8044 RVA: 0x000B2934 File Offset: 0x000B0D34
		private void ChangeTextColor(Color targetColor)
		{
			Text componentInChildren = base.GetComponentInChildren<Text>();
			if (!componentInChildren)
			{
				return;
			}
			componentInChildren.color = targetColor;
		}

		// Token: 0x06001F6D RID: 8045 RVA: 0x000B295B File Offset: 0x000B0D5B
		private void DoSpriteSwap(Sprite newSprite)
		{
			if (this.button.image == null)
			{
				return;
			}
			this.button.image.overrideSprite = newSprite;
		}

		// Token: 0x06001F6E RID: 8046 RVA: 0x000B2988 File Offset: 0x000B0D88
		private void TriggerAnimation(string triggername)
		{
			if (this.button.animator == null || !this.button.animator.isActiveAndEnabled || !this.button.animator.hasBoundPlayables || string.IsNullOrEmpty(triggername))
			{
				return;
			}
			this.button.animator.ResetTrigger(this.button.animationTriggers.normalTrigger);
			this.button.animator.ResetTrigger(this.button.animationTriggers.pressedTrigger);
			this.button.animator.ResetTrigger(this.button.animationTriggers.highlightedTrigger);
			this.button.animator.ResetTrigger(this.button.animationTriggers.disabledTrigger);
			this.button.animator.SetTrigger(triggername);
		}

		// Token: 0x04001A81 RID: 6785
		internal int index;

		// Token: 0x04001A82 RID: 6786
		[SerializeField]
		private Color textColor;
	}
}
