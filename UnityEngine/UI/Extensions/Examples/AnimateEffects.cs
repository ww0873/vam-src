using System;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x0200049E RID: 1182
	public class AnimateEffects : MonoBehaviour
	{
		// Token: 0x06001DD6 RID: 7638 RVA: 0x000AB2D8 File Offset: 0x000A96D8
		public AnimateEffects()
		{
		}

		// Token: 0x06001DD7 RID: 7639 RVA: 0x000AB37E File Offset: 0x000A977E
		private void Start()
		{
			this.cylinderTextRT = this.cylinderText.GetComponent<Transform>();
		}

		// Token: 0x06001DD8 RID: 7640 RVA: 0x000AB394 File Offset: 0x000A9794
		private void Update()
		{
			this.letterSpacing.spacing += this.letterSpacingModifier;
			if (this.letterSpacing.spacing > this.letterSpacingMax || this.letterSpacing.spacing < this.letterSpacingMin)
			{
				this.letterSpacingModifier = -this.letterSpacingModifier;
			}
			this.curvedText.CurveMultiplier += this.curvedTextModifier;
			if (this.curvedText.CurveMultiplier > this.curvedTextMax || this.curvedText.CurveMultiplier < this.curvedTextMin)
			{
				this.curvedTextModifier = -this.curvedTextModifier;
			}
			this.gradient2.Offset += this.gradient2Modifier;
			if (this.gradient2.Offset > this.gradient2Max || this.gradient2.Offset < this.gradient2Min)
			{
				this.gradient2Modifier = -this.gradient2Modifier;
			}
			this.cylinderTextRT.Rotate(this.cylinderRotation);
			this.SAUIM.CutOff += this.SAUIMModifier;
			if (this.SAUIM.CutOff > this.SAUIMMax || this.SAUIM.CutOff < this.SAUIMMin)
			{
				this.SAUIMModifier = -this.SAUIMModifier;
			}
		}

		// Token: 0x04001931 RID: 6449
		public LetterSpacing letterSpacing;

		// Token: 0x04001932 RID: 6450
		private float letterSpacingMax = 10f;

		// Token: 0x04001933 RID: 6451
		private float letterSpacingMin = -10f;

		// Token: 0x04001934 RID: 6452
		private float letterSpacingModifier = 0.1f;

		// Token: 0x04001935 RID: 6453
		public CurvedText curvedText;

		// Token: 0x04001936 RID: 6454
		private float curvedTextMax = 0.05f;

		// Token: 0x04001937 RID: 6455
		private float curvedTextMin = -0.05f;

		// Token: 0x04001938 RID: 6456
		private float curvedTextModifier = 0.001f;

		// Token: 0x04001939 RID: 6457
		public Gradient2 gradient2;

		// Token: 0x0400193A RID: 6458
		private float gradient2Max = 1f;

		// Token: 0x0400193B RID: 6459
		private float gradient2Min = -1f;

		// Token: 0x0400193C RID: 6460
		private float gradient2Modifier = 0.01f;

		// Token: 0x0400193D RID: 6461
		public CylinderText cylinderText;

		// Token: 0x0400193E RID: 6462
		private Transform cylinderTextRT;

		// Token: 0x0400193F RID: 6463
		private Vector3 cylinderRotation = new Vector3(0f, 1f, 0f);

		// Token: 0x04001940 RID: 6464
		public SoftMaskScript SAUIM;

		// Token: 0x04001941 RID: 6465
		private float SAUIMMax = 1f;

		// Token: 0x04001942 RID: 6466
		private float SAUIMMin;

		// Token: 0x04001943 RID: 6467
		private float SAUIMModifier = 0.01f;
	}
}
