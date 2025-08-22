using System;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x0200048F RID: 1167
	public class Example02ScrollViewCell : FancyScrollViewCell<Example02CellDto, Example02ScrollViewContext>
	{
		// Token: 0x06001DA3 RID: 7587 RVA: 0x000AAAAA File Offset: 0x000A8EAA
		public Example02ScrollViewCell()
		{
		}

		// Token: 0x06001DA4 RID: 7588 RVA: 0x000AAAC4 File Offset: 0x000A8EC4
		private void Start()
		{
			RectTransform rectTransform = base.transform as RectTransform;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchoredPosition3D = Vector3.zero;
			this.UpdatePosition(0f);
			this.button.onClick.AddListener(new UnityAction(this.OnPressedCell));
		}

		// Token: 0x06001DA5 RID: 7589 RVA: 0x000AAB25 File Offset: 0x000A8F25
		public override void SetContext(Example02ScrollViewContext context)
		{
			this.context = context;
		}

		// Token: 0x06001DA6 RID: 7590 RVA: 0x000AAB30 File Offset: 0x000A8F30
		public override void UpdateContent(Example02CellDto itemData)
		{
			this.message.text = itemData.Message;
			if (this.context != null)
			{
				bool flag = this.context.SelectedIndex == base.DataIndex;
				this.image.color = ((!flag) ? new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 77) : new Color32(0, byte.MaxValue, byte.MaxValue, 100));
			}
		}

		// Token: 0x06001DA7 RID: 7591 RVA: 0x000AABB0 File Offset: 0x000A8FB0
		public override void UpdatePosition(float position)
		{
			this.animator.Play(this.scrollTriggerHash, -1, position);
			this.animator.speed = 0f;
		}

		// Token: 0x06001DA8 RID: 7592 RVA: 0x000AABD5 File Offset: 0x000A8FD5
		public void OnPressedCell()
		{
			if (this.context != null)
			{
				this.context.OnPressedCell(this);
			}
		}

		// Token: 0x04001911 RID: 6417
		[SerializeField]
		private Animator animator;

		// Token: 0x04001912 RID: 6418
		[SerializeField]
		private Text message;

		// Token: 0x04001913 RID: 6419
		[SerializeField]
		private Image image;

		// Token: 0x04001914 RID: 6420
		[SerializeField]
		private Button button;

		// Token: 0x04001915 RID: 6421
		private readonly int scrollTriggerHash = Animator.StringToHash("scroll");

		// Token: 0x04001916 RID: 6422
		private Example02ScrollViewContext context;
	}
}
