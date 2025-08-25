using System;
using UnityEngine;
using UnityEngine.UI;

namespace MeshVR
{
	// Token: 0x02000DBE RID: 3518
	public class FPS : MonoBehaviour
	{
		// Token: 0x06006D02 RID: 27906 RVA: 0x00291593 File Offset: 0x0028F993
		public FPS()
		{
		}

		// Token: 0x06006D03 RID: 27907 RVA: 0x002915A8 File Offset: 0x0028F9A8
		private void UpdateFPS()
		{
			float elapsedMilliseconds = GlobalStopwatch.GetElapsedMilliseconds();
			float num = elapsedMilliseconds - this.lastUpdateTime;
			this.TimeLeft -= num;
			this.Accum += 1000f / num;
			this.lastUpdateTime = elapsedMilliseconds;
			this.Frames++;
			if ((double)this.TimeLeft <= 0.0 && this.Frames != 0)
			{
				float num2 = this.Accum / (float)this.Frames;
				this.fps = string.Format("FPS: {0:F2}", num2);
				if (this.text)
				{
					this.text.text = this.fps;
				}
				this.TimeLeft += this.UpdateIntervalInMilliseconds;
				this.Accum = 0f;
				this.Frames = 0;
			}
		}

		// Token: 0x06006D04 RID: 27908 RVA: 0x00291686 File Offset: 0x0028FA86
		private void Update()
		{
			this.UpdateFPS();
		}

		// Token: 0x04005E80 RID: 24192
		public string fps;

		// Token: 0x04005E81 RID: 24193
		public float UpdateIntervalInMilliseconds = 500f;

		// Token: 0x04005E82 RID: 24194
		public Text text;

		// Token: 0x04005E83 RID: 24195
		private float TimeLeft;

		// Token: 0x04005E84 RID: 24196
		private float Accum;

		// Token: 0x04005E85 RID: 24197
		private int Frames;

		// Token: 0x04005E86 RID: 24198
		private float lastUpdateTime;
	}
}
