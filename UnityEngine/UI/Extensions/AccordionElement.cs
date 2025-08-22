using System;
using UnityEngine.Events;
using UnityEngine.UI.Extensions.Tweens;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020004A5 RID: 1189
	[RequireComponent(typeof(RectTransform), typeof(LayoutElement))]
	[AddComponentMenu("UI/Extensions/Accordion/Accordion Element")]
	public class AccordionElement : Toggle
	{
		// Token: 0x06001DFE RID: 7678 RVA: 0x000ABDF2 File Offset: 0x000AA1F2
		protected AccordionElement()
		{
			if (this.m_FloatTweenRunner == null)
			{
				this.m_FloatTweenRunner = new TweenRunner<FloatTween>();
			}
			this.m_FloatTweenRunner.Init(this);
		}

		// Token: 0x06001DFF RID: 7679 RVA: 0x000ABE28 File Offset: 0x000AA228
		protected override void Awake()
		{
			base.Awake();
			base.transition = Selectable.Transition.None;
			this.toggleTransition = Toggle.ToggleTransition.None;
			this.m_Accordion = base.gameObject.GetComponentInParent<Accordion>();
			this.m_RectTransform = (base.transform as RectTransform);
			this.m_LayoutElement = base.gameObject.GetComponent<LayoutElement>();
			this.onValueChanged.AddListener(new UnityAction<bool>(this.OnValueChanged));
		}

		// Token: 0x06001E00 RID: 7680 RVA: 0x000ABE94 File Offset: 0x000AA294
		public void OnValueChanged(bool state)
		{
			if (this.m_LayoutElement == null)
			{
				return;
			}
			Accordion.Transition transition = (!(this.m_Accordion != null)) ? Accordion.Transition.Instant : this.m_Accordion.transition;
			if (transition == Accordion.Transition.Instant)
			{
				if (state)
				{
					this.m_LayoutElement.preferredHeight = -1f;
				}
				else
				{
					this.m_LayoutElement.preferredHeight = this.m_MinHeight;
				}
			}
			else if (transition == Accordion.Transition.Tween)
			{
				if (state)
				{
					this.StartTween(this.m_MinHeight, this.GetExpandedHeight());
				}
				else
				{
					this.StartTween(this.m_RectTransform.rect.height, this.m_MinHeight);
				}
			}
		}

		// Token: 0x06001E01 RID: 7681 RVA: 0x000ABF50 File Offset: 0x000AA350
		protected float GetExpandedHeight()
		{
			if (this.m_LayoutElement == null)
			{
				return this.m_MinHeight;
			}
			float preferredHeight = this.m_LayoutElement.preferredHeight;
			this.m_LayoutElement.preferredHeight = -1f;
			float preferredHeight2 = LayoutUtility.GetPreferredHeight(this.m_RectTransform);
			this.m_LayoutElement.preferredHeight = preferredHeight;
			return preferredHeight2;
		}

		// Token: 0x06001E02 RID: 7682 RVA: 0x000ABFAC File Offset: 0x000AA3AC
		protected void StartTween(float startFloat, float targetFloat)
		{
			float duration = (!(this.m_Accordion != null)) ? 0.3f : this.m_Accordion.transitionDuration;
			FloatTween info = new FloatTween
			{
				duration = duration,
				startFloat = startFloat,
				targetFloat = targetFloat
			};
			info.AddOnChangedCallback(new UnityAction<float>(this.SetHeight));
			info.ignoreTimeScale = true;
			this.m_FloatTweenRunner.StartTween(info);
		}

		// Token: 0x06001E03 RID: 7683 RVA: 0x000AC029 File Offset: 0x000AA429
		protected void SetHeight(float height)
		{
			if (this.m_LayoutElement == null)
			{
				return;
			}
			this.m_LayoutElement.preferredHeight = height;
		}

		// Token: 0x0400196B RID: 6507
		[SerializeField]
		private float m_MinHeight = 18f;

		// Token: 0x0400196C RID: 6508
		private Accordion m_Accordion;

		// Token: 0x0400196D RID: 6509
		private RectTransform m_RectTransform;

		// Token: 0x0400196E RID: 6510
		private LayoutElement m_LayoutElement;

		// Token: 0x0400196F RID: 6511
		[NonSerialized]
		private readonly TweenRunner<FloatTween> m_FloatTweenRunner;
	}
}
