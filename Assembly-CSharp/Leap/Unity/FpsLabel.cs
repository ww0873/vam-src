using System;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x0200072E RID: 1838
	public class FpsLabel : MonoBehaviour
	{
		// Token: 0x06002CB0 RID: 11440 RVA: 0x000EFA44 File Offset: 0x000EDE44
		public FpsLabel()
		{
		}

		// Token: 0x06002CB1 RID: 11441 RVA: 0x000EFA58 File Offset: 0x000EDE58
		private void OnEnable()
		{
			if (this._provider == null)
			{
				this._provider = Hands.Provider;
			}
			if (this._frameRateText == null)
			{
				this._frameRateText = base.GetComponentInChildren<TextMesh>();
				if (this._frameRateText == null)
				{
					Debug.LogError("Could not enable FpsLabel because no TextMesh was specified!");
					base.enabled = false;
				}
			}
			this._smoothedRenderFps.delay = 0.3f;
			this._smoothedRenderFps.reset = true;
		}

		// Token: 0x06002CB2 RID: 11442 RVA: 0x000EFADC File Offset: 0x000EDEDC
		private void Update()
		{
			this._frameRateText.text = string.Empty;
			if (this._provider != null)
			{
				Frame currentFrame = this._provider.CurrentFrame;
				if (currentFrame != null)
				{
					TextMesh frameRateText = this._frameRateText;
					frameRateText.text = frameRateText.text + "Data FPS:" + this._provider.CurrentFrame.CurrentFramesPerSecond.ToString("f2");
					TextMesh frameRateText2 = this._frameRateText;
					frameRateText2.text += Environment.NewLine;
				}
			}
			if (Time.smoothDeltaTime > Mathf.Epsilon)
			{
				this._smoothedRenderFps.Update(1f / Time.smoothDeltaTime, Time.deltaTime);
			}
			TextMesh frameRateText3 = this._frameRateText;
			frameRateText3.text = frameRateText3.text + "Render FPS:" + Mathf.RoundToInt(this._smoothedRenderFps.value).ToString("f2");
		}

		// Token: 0x040023A6 RID: 9126
		[SerializeField]
		private LeapProvider _provider;

		// Token: 0x040023A7 RID: 9127
		[SerializeField]
		private TextMesh _frameRateText;

		// Token: 0x040023A8 RID: 9128
		private SmoothedFloat _smoothedRenderFps = new SmoothedFloat();
	}
}
