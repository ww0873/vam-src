using System;
using UnityEngine;
using UnityEngine.UI;

namespace Leap.Unity
{
	// Token: 0x02000725 RID: 1829
	public class DisconnectionNotice : MonoBehaviour
	{
		// Token: 0x06002C9E RID: 11422 RVA: 0x000EF766 File Offset: 0x000EDB66
		public DisconnectionNotice()
		{
		}

		// Token: 0x06002C9F RID: 11423 RVA: 0x000EF797 File Offset: 0x000EDB97
		private void Start()
		{
			this.leap_controller_ = new Controller();
			this.SetAlpha(0f);
		}

		// Token: 0x06002CA0 RID: 11424 RVA: 0x000EF7AF File Offset: 0x000EDBAF
		private void SetAlpha(float alpha)
		{
			base.GetComponent<Image>().color = Color.Lerp(Color.clear, this.onColor, alpha);
		}

		// Token: 0x06002CA1 RID: 11425 RVA: 0x000EF7CD File Offset: 0x000EDBCD
		private bool IsConnected()
		{
			return this.leap_controller_.IsConnected;
		}

		// Token: 0x06002CA2 RID: 11426 RVA: 0x000EF7DC File Offset: 0x000EDBDC
		private void Update()
		{
			if (this.IsConnected())
			{
				this.frames_disconnected_ = 0;
			}
			else
			{
				this.frames_disconnected_++;
			}
			if (this.frames_disconnected_ < this.waitFrames)
			{
				this.fadedIn -= Time.deltaTime / this.fadeOutTime;
			}
			else
			{
				this.fadedIn += Time.deltaTime / this.fadeInTime;
			}
			this.fadedIn = Mathf.Clamp(this.fadedIn, 0f, 1f);
			this.SetAlpha(this.fade.Evaluate(this.fadedIn));
		}

		// Token: 0x04002394 RID: 9108
		public float fadeInTime = 1f;

		// Token: 0x04002395 RID: 9109
		public float fadeOutTime = 1f;

		// Token: 0x04002396 RID: 9110
		public AnimationCurve fade;

		// Token: 0x04002397 RID: 9111
		public int waitFrames = 10;

		// Token: 0x04002398 RID: 9112
		public Color onColor = Color.white;

		// Token: 0x04002399 RID: 9113
		private Controller leap_controller_;

		// Token: 0x0400239A RID: 9114
		private float fadedIn;

		// Token: 0x0400239B RID: 9115
		private int frames_disconnected_;
	}
}
