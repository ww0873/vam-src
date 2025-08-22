using System;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x02000494 RID: 1172
	public class Example03ScrollViewCell : FancyScrollViewCell<Example03CellDto, Example03ScrollViewContext>
	{
		// Token: 0x06001DB3 RID: 7603 RVA: 0x000AAD5E File Offset: 0x000A915E
		public Example03ScrollViewCell()
		{
		}

		// Token: 0x06001DB4 RID: 7604 RVA: 0x000AAD78 File Offset: 0x000A9178
		private void Start()
		{
			RectTransform rectTransform = base.transform as RectTransform;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchoredPosition3D = Vector3.zero;
			this.UpdatePosition(0f);
			this.button.onClick.AddListener(new UnityAction(this.OnPressedCell));
		}

		// Token: 0x06001DB5 RID: 7605 RVA: 0x000AADD9 File Offset: 0x000A91D9
		public override void SetContext(Example03ScrollViewContext context)
		{
			this.context = context;
		}

		// Token: 0x06001DB6 RID: 7606 RVA: 0x000AADE4 File Offset: 0x000A91E4
		public override void UpdateContent(Example03CellDto itemData)
		{
			this.message.text = itemData.Message;
			if (this.context != null)
			{
				bool flag = this.context.SelectedIndex == base.DataIndex;
				this.image.color = ((!flag) ? new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 77) : new Color32(0, byte.MaxValue, byte.MaxValue, 100));
			}
		}

		// Token: 0x06001DB7 RID: 7607 RVA: 0x000AAE64 File Offset: 0x000A9264
		public override void UpdatePosition(float position)
		{
			this.animator.Play(this.scrollTriggerHash, -1, position);
			this.animator.speed = 0f;
		}

		// Token: 0x06001DB8 RID: 7608 RVA: 0x000AAE89 File Offset: 0x000A9289
		public void OnPressedCell()
		{
			if (this.context != null)
			{
				this.context.OnPressedCell(this);
			}
		}

		// Token: 0x0400191D RID: 6429
		[SerializeField]
		private Animator animator;

		// Token: 0x0400191E RID: 6430
		[SerializeField]
		private Text message;

		// Token: 0x0400191F RID: 6431
		[SerializeField]
		private Image image;

		// Token: 0x04001920 RID: 6432
		[SerializeField]
		private Button button;

		// Token: 0x04001921 RID: 6433
		private readonly int scrollTriggerHash = Animator.StringToHash("scroll");

		// Token: 0x04001922 RID: 6434
		private Example03ScrollViewContext context;
	}
}
