using System;
using UnityEngine;

namespace Obi
{
	// Token: 0x020003F7 RID: 1015
	public class ObiProfiler : MonoBehaviour
	{
		// Token: 0x060019D9 RID: 6617 RVA: 0x0008F841 File Offset: 0x0008DC41
		public ObiProfiler()
		{
		}

		// Token: 0x060019DA RID: 6618 RVA: 0x0008F86D File Offset: 0x0008DC6D
		public void OnEnable()
		{
			Oni.EnableProfiler(true);
			this.numThreads = Oni.GetMaxSystemConcurrency();
		}

		// Token: 0x060019DB RID: 6619 RVA: 0x0008F880 File Offset: 0x0008DC80
		public void OnDisable()
		{
			Oni.EnableProfiler(false);
		}

		// Token: 0x060019DC RID: 6620 RVA: 0x0008F888 File Offset: 0x0008DC88
		public void OnGUI()
		{
			GUI.skin = this.skin;
			int num = 20;
			int num2 = 20;
			GUI.BeginGroup(new Rect(0f, 0f, (float)Screen.width, (float)num), string.Empty, "Box");
			GUI.Label(new Rect(5f, 0f, 50f, (float)num), "Zoom:");
			this.zoom = GUI.HorizontalSlider(new Rect(50f, 5f, 100f, (float)num), this.zoom, 0.005f, 1f);
			GUI.Label(new Rect((float)(Screen.width - 100), 0f, 100f, (float)num), (ObiProfiler.frameDuration / 1000.0).ToString("0.###") + " ms/step");
			GUI.EndGroup();
			this.scrollPosition = GUI.BeginScrollView(new Rect(0f, (float)num, (float)Screen.width, (float)(Mathf.Min(this.maxVisibleThreads, this.numThreads) * num2 + 10)), this.scrollPosition, new Rect(0f, 0f, (float)Screen.width / this.zoom, (float)(this.numThreads * num2)));
			foreach (Oni.ProfileInfo profileInfo in ObiProfiler.info)
			{
				GUI.color = Color.green;
				int num3 = (int)(profileInfo.start / ObiProfiler.frameDuration * (double)(Screen.width - 10) / (double)this.zoom);
				int num4 = (int)(profileInfo.end / ObiProfiler.frameDuration * (double)(Screen.width - 10) / (double)this.zoom);
				string text;
				if (this.showPercentages)
				{
					double num5 = (profileInfo.end - profileInfo.start) / ObiProfiler.frameDuration * 100.0;
					text = profileInfo.name + " (" + num5.ToString("0.#") + "%)";
				}
				else
				{
					double num6 = (profileInfo.end - profileInfo.start) / 1000.0;
					text = profileInfo.name + " (" + num6.ToString("0.##") + "ms)";
				}
				GUI.Box(new Rect((float)num3, (float)(profileInfo.threadID * num2), (float)(num4 - num3), (float)num2), text, "thread");
			}
			GUI.EndScrollView();
		}

		// Token: 0x04001503 RID: 5379
		public GUISkin skin;

		// Token: 0x04001504 RID: 5380
		public bool showPercentages;

		// Token: 0x04001505 RID: 5381
		public int maxVisibleThreads = 4;

		// Token: 0x04001506 RID: 5382
		public static Oni.ProfileInfo[] info;

		// Token: 0x04001507 RID: 5383
		public static double frameDuration;

		// Token: 0x04001508 RID: 5384
		private float zoom = 1f;

		// Token: 0x04001509 RID: 5385
		private Vector2 scrollPosition = Vector2.zero;

		// Token: 0x0400150A RID: 5386
		private int numThreads = 1;
	}
}
