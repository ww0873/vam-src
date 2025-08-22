using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Leap.Unity
{
	// Token: 0x02000740 RID: 1856
	public class ProgressBar
	{
		// Token: 0x06002D50 RID: 11600 RVA: 0x000F153C File Offset: 0x000EF93C
		public ProgressBar(IProgressView view)
		{
			this._view = view;
		}

		// Token: 0x06002D51 RID: 11601 RVA: 0x000F1590 File Offset: 0x000EF990
		public void Begin(int sections, string title, string info, Action action)
		{
			if (!this.stopwatch.IsRunning)
			{
				this.stopwatch.Reset();
				this.stopwatch.Start();
			}
			this.chunks.Add(sections);
			this.progress.Add(0);
			this.titleStrings.Add(title);
			this.infoStrings.Add(info);
			try
			{
				this._forceUpdate = true;
				action();
			}
			finally
			{
				int num = this.chunks.Count - 1;
				this.chunks.RemoveAt(num);
				this.progress.RemoveAt(num);
				this.titleStrings.RemoveAt(num);
				this.infoStrings.RemoveAt(num);
				num--;
				if (num >= 0)
				{
					List<int> list;
					int index;
					(list = this.progress)[index = num] = list[index] + 1;
				}
				if (this.chunks.Count == 0)
				{
					this._view.Clear();
					this.stopwatch.Stop();
				}
			}
		}

		// Token: 0x06002D52 RID: 11602 RVA: 0x000F16A0 File Offset: 0x000EFAA0
		public void Step(string infoString = "")
		{
			List<int> list;
			int index;
			(list = this.progress)[index = this.progress.Count - 1] = list[index] + 1;
			if (this.stopwatch.ElapsedMilliseconds > 17L || this._forceUpdate)
			{
				this.displayBar(infoString);
				this.stopwatch.Reset();
				this.stopwatch.Start();
			}
		}

		// Token: 0x06002D53 RID: 11603 RVA: 0x000F1710 File Offset: 0x000EFB10
		private void displayBar(string info = "")
		{
			this._forceUpdate = false;
			float num = 0f;
			float num2 = 1f;
			string text = string.Empty;
			string text2 = string.Empty;
			for (int i = 0; i < this.chunks.Count; i++)
			{
				float num3 = (float)this.chunks[i];
				float num4 = (float)this.progress[i];
				num += num2 * (num4 / num3);
				num2 /= num3;
				text += this.titleStrings[i];
				text2 += this.infoStrings[i];
			}
			text2 += info;
			this._view.DisplayProgress(text, text2, num);
		}

		// Token: 0x040023CC RID: 9164
		private List<int> chunks = new List<int>();

		// Token: 0x040023CD RID: 9165
		private List<int> progress = new List<int>();

		// Token: 0x040023CE RID: 9166
		private List<string> titleStrings = new List<string>();

		// Token: 0x040023CF RID: 9167
		private List<string> infoStrings = new List<string>();

		// Token: 0x040023D0 RID: 9168
		private Stopwatch stopwatch = new Stopwatch();

		// Token: 0x040023D1 RID: 9169
		private bool _forceUpdate;

		// Token: 0x040023D2 RID: 9170
		private IProgressView _view;
	}
}
