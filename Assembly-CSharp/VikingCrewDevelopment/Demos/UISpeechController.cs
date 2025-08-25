using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using VikingCrewTools.UI;

namespace VikingCrewDevelopment.Demos
{
	// Token: 0x0200056B RID: 1387
	public class UISpeechController : MonoBehaviour
	{
		// Token: 0x0600232C RID: 9004 RVA: 0x000C8757 File Offset: 0x000C6B57
		public UISpeechController()
		{
		}

		// Token: 0x0600232D RID: 9005 RVA: 0x000C875F File Offset: 0x000C6B5F
		private void Start()
		{
		}

		// Token: 0x0600232E RID: 9006 RVA: 0x000C8761 File Offset: 0x000C6B61
		private void Update()
		{
		}

		// Token: 0x0600232F RID: 9007 RVA: 0x000C8763 File Offset: 0x000C6B63
		public void OnTalk()
		{
			this.talkBehaviour.SaySomething(this.txtMessage.text, (SpeechBubbleManager.SpeechbubbleType)this.toggles.ActiveToggles().First<Toggle>().transform.GetSiblingIndex());
		}

		// Token: 0x04001D23 RID: 7459
		public InputField txtMessage;

		// Token: 0x04001D24 RID: 7460
		public SayRandomThingsBehaviour talkBehaviour;

		// Token: 0x04001D25 RID: 7461
		public ToggleGroup toggles;
	}
}
