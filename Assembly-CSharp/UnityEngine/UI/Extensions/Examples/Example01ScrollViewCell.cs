using System;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x0200048B RID: 1163
	public class Example01ScrollViewCell : FancyScrollViewCell<Example01CellDto>
	{
		// Token: 0x06001D96 RID: 7574 RVA: 0x000AA8B0 File Offset: 0x000A8CB0
		public Example01ScrollViewCell()
		{
		}

		// Token: 0x06001D97 RID: 7575 RVA: 0x000AA8C8 File Offset: 0x000A8CC8
		private void Start()
		{
			RectTransform rectTransform = base.transform as RectTransform;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchoredPosition3D = Vector3.zero;
			this.UpdatePosition(0f);
		}

		// Token: 0x06001D98 RID: 7576 RVA: 0x000AA90D File Offset: 0x000A8D0D
		public override void UpdateContent(Example01CellDto itemData)
		{
			this.message.text = itemData.Message;
		}

		// Token: 0x06001D99 RID: 7577 RVA: 0x000AA920 File Offset: 0x000A8D20
		public override void UpdatePosition(float position)
		{
			this.animator.Play(this.scrollTriggerHash, -1, position);
			this.animator.speed = 0f;
		}

		// Token: 0x0400190A RID: 6410
		[SerializeField]
		private Animator animator;

		// Token: 0x0400190B RID: 6411
		[SerializeField]
		private Text message;

		// Token: 0x0400190C RID: 6412
		private readonly int scrollTriggerHash = Animator.StringToHash("scroll");
	}
}
