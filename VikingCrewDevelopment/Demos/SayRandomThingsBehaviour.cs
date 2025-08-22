using System;
using UnityEngine;
using VikingCrewTools.UI;

namespace VikingCrewDevelopment.Demos
{
	// Token: 0x02000568 RID: 1384
	public class SayRandomThingsBehaviour : MonoBehaviour
	{
		// Token: 0x06002322 RID: 8994 RVA: 0x000C843F File Offset: 0x000C683F
		public SayRandomThingsBehaviour()
		{
		}

		// Token: 0x06002323 RID: 8995 RVA: 0x000C846D File Offset: 0x000C686D
		private void Start()
		{
			this.timeToNextSpeak = this.timeBetweenSpeak;
		}

		// Token: 0x06002324 RID: 8996 RVA: 0x000C847C File Offset: 0x000C687C
		private void Update()
		{
			this.timeToNextSpeak -= Time.deltaTime;
			if (this.doTalkOnYourOwn && this.timeToNextSpeak <= 0f && this.thingsToSay.Length > 0)
			{
				this.SaySomething();
			}
		}

		// Token: 0x06002325 RID: 8997 RVA: 0x000C84CC File Offset: 0x000C68CC
		public void SaySomething()
		{
			string message = this.thingsToSay[UnityEngine.Random.Range(0, this.thingsToSay.Length)];
			this.SaySomething(message);
		}

		// Token: 0x06002326 RID: 8998 RVA: 0x000C84F6 File Offset: 0x000C68F6
		public void SaySomething(string message)
		{
			this.SaySomething(message, SpeechBubbleManager.Instance.GetRandomSpeechbubbleType());
		}

		// Token: 0x06002327 RID: 8999 RVA: 0x000C850C File Offset: 0x000C690C
		public void SaySomething(string message, SpeechBubbleManager.SpeechbubbleType speechbubbleType)
		{
			if (this.mouth == null)
			{
				SpeechBubbleManager.Instance.AddSpeechBubble(base.transform, message, speechbubbleType, 0f, default(Color), default(Vector3));
			}
			else
			{
				SpeechBubbleManager.Instance.AddSpeechBubble(this.mouth, message, speechbubbleType, 0f, default(Color), default(Vector3));
			}
			this.timeToNextSpeak = this.timeBetweenSpeak;
		}

		// Token: 0x04001D17 RID: 7447
		[Multiline]
		public string[] thingsToSay = new string[]
		{
			"Hello world"
		};

		// Token: 0x04001D18 RID: 7448
		[Header("Leave as null if you just want center of character to emit speechbubbles")]
		public Transform mouth;

		// Token: 0x04001D19 RID: 7449
		public float timeBetweenSpeak = 5f;

		// Token: 0x04001D1A RID: 7450
		public bool doTalkOnYourOwn = true;

		// Token: 0x04001D1B RID: 7451
		private float timeToNextSpeak;
	}
}
