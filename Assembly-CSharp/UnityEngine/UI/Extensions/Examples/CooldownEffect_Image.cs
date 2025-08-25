using System;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x02000486 RID: 1158
	[RequireComponent(typeof(Image))]
	public class CooldownEffect_Image : MonoBehaviour
	{
		// Token: 0x06001D87 RID: 7559 RVA: 0x000AA2F3 File Offset: 0x000A86F3
		public CooldownEffect_Image()
		{
		}

		// Token: 0x06001D88 RID: 7560 RVA: 0x000AA2FB File Offset: 0x000A86FB
		private void Start()
		{
			if (this.cooldown == null)
			{
				Debug.LogError("Missing Cooldown Button assignment");
			}
			this.target = base.GetComponent<Image>();
		}

		// Token: 0x06001D89 RID: 7561 RVA: 0x000AA324 File Offset: 0x000A8724
		private void Update()
		{
			this.target.fillAmount = Mathf.Lerp(0f, 1f, this.cooldown.CooldownTimeRemaining / this.cooldown.CooldownTimeout);
			if (this.displayText)
			{
				this.displayText.text = string.Format("{0}%", this.cooldown.CooldownPercentComplete);
			}
		}

		// Token: 0x06001D8A RID: 7562 RVA: 0x000AA397 File Offset: 0x000A8797
		private void OnDisable()
		{
			if (this.displayText)
			{
				this.displayText.text = this.originalText;
			}
		}

		// Token: 0x06001D8B RID: 7563 RVA: 0x000AA3BA File Offset: 0x000A87BA
		private void OnEnable()
		{
			if (this.displayText)
			{
				this.originalText = this.displayText.text;
			}
		}

		// Token: 0x04001900 RID: 6400
		public CooldownButton cooldown;

		// Token: 0x04001901 RID: 6401
		public Text displayText;

		// Token: 0x04001902 RID: 6402
		private Image target;

		// Token: 0x04001903 RID: 6403
		private string originalText;
	}
}
