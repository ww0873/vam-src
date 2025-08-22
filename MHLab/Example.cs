using System;
using UnityEngine;

namespace MHLab
{
	// Token: 0x0200033E RID: 830
	public class Example : MonoBehaviour
	{
		// Token: 0x06001423 RID: 5155 RVA: 0x000746DA File Offset: 0x00072ADA
		public Example()
		{
		}

		// Token: 0x06001424 RID: 5156 RVA: 0x000746E2 File Offset: 0x00072AE2
		private void Start()
		{
			this._screenMiddleX = (float)(Screen.width / 2);
			this._screenMiddleY = (float)(Screen.height / 2);
		}

		// Token: 0x06001425 RID: 5157 RVA: 0x00074700 File Offset: 0x00072B00
		private void OnGUI()
		{
			if (GUI.Button(new Rect(10f, 10f, 50f, 22f), "it-IT"))
			{
				Singleton<Localizatron>.Instance.SetLanguage("it-IT");
			}
			if (GUI.Button(new Rect(70f, 10f, 50f, 22f), "en-EN"))
			{
				Singleton<Localizatron>.Instance.SetLanguage("en-EN");
			}
			GUI.Label(new Rect(this._screenMiddleX - 50f, this._screenMiddleY - 11f, 100f, 22f), Singleton<Localizatron>.Instance.Translate("Hello World"));
		}

		// Token: 0x04001170 RID: 4464
		private float _screenMiddleX;

		// Token: 0x04001171 RID: 4465
		private float _screenMiddleY;
	}
}
