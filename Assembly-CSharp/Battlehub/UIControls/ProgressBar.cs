using System;
using UnityEngine;
using UnityEngine.UI;

namespace Battlehub.UIControls
{
	// Token: 0x02000281 RID: 641
	public class ProgressBar : MonoBehaviour
	{
		// Token: 0x06000E43 RID: 3651 RVA: 0x00053640 File Offset: 0x00051A40
		public ProgressBar()
		{
		}

		// Token: 0x06000E44 RID: 3652 RVA: 0x0005365C File Offset: 0x00051A5C
		private void Start()
		{
			this.m_alpha = new float[this.Images.Length];
			for (int i = 0; i < this.Images.Length; i++)
			{
				this.m_alpha[i] = (float)i / (float)this.Images.Length;
				Color color = this.Images[i].color;
				color.a = this.m_alpha[i];
				this.Images[i].color = color;
				Color effectColor = this.Outlines[i].effectColor;
				effectColor.a = this.m_alpha[i];
				this.Outlines[i].effectColor = effectColor;
			}
		}

		// Token: 0x06000E45 RID: 3653 RVA: 0x00053700 File Offset: 0x00051B00
		private void FixedUpdate()
		{
			if (!this.IsInProgress)
			{
				return;
			}
			for (int i = 0; i < this.Images.Length; i++)
			{
				this.Images[i].color = this.UpdateAlpha(this.Images[i].color, i);
				this.Outlines[i].effectColor = this.UpdateAlpha(this.Outlines[i].effectColor, i);
			}
		}

		// Token: 0x06000E46 RID: 3654 RVA: 0x00053774 File Offset: 0x00051B74
		private Color UpdateAlpha(Color color, int index)
		{
			this.m_alpha[index] -= Time.deltaTime * this.Speed;
			if (this.m_alpha[index] < 0f)
			{
				this.m_alpha[index] = 1f;
			}
			color.a = Mathf.Clamp01(this.m_alpha[index]);
			return color;
		}

		// Token: 0x04000DB7 RID: 3511
		[SerializeField]
		private Image[] Images;

		// Token: 0x04000DB8 RID: 3512
		[SerializeField]
		private Outline[] Outlines;

		// Token: 0x04000DB9 RID: 3513
		private float[] m_alpha;

		// Token: 0x04000DBA RID: 3514
		[SerializeField]
		private float Speed = 1f;

		// Token: 0x04000DBB RID: 3515
		public bool IsInProgress = true;
	}
}
