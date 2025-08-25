using System;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x02000737 RID: 1847
	public static class LeapColor
	{
		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x06002D00 RID: 11520 RVA: 0x000F0BF6 File Offset: 0x000EEFF6
		public static Color black
		{
			get
			{
				return Color.black;
			}
		}

		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x06002D01 RID: 11521 RVA: 0x000F0BFD File Offset: 0x000EEFFD
		public static Color gray
		{
			get
			{
				return new Color(0.5f, 0.5f, 0.5f);
			}
		}

		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x06002D02 RID: 11522 RVA: 0x000F0C13 File Offset: 0x000EF013
		public static Color white
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x06002D03 RID: 11523 RVA: 0x000F0C1A File Offset: 0x000EF01A
		public static Color pink
		{
			get
			{
				return new Color(1f, 0.7529412f, 0.79607844f);
			}
		}

		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x06002D04 RID: 11524 RVA: 0x000F0C30 File Offset: 0x000EF030
		public static Color magenta
		{
			get
			{
				return Color.magenta;
			}
		}

		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x06002D05 RID: 11525 RVA: 0x000F0C37 File Offset: 0x000EF037
		public static Color fuschia
		{
			get
			{
				return LeapColor.lerp(Color.magenta, Color.blue, 0.1f);
			}
		}

		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x06002D06 RID: 11526 RVA: 0x000F0C4D File Offset: 0x000EF04D
		public static Color red
		{
			get
			{
				return Color.red;
			}
		}

		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x06002D07 RID: 11527 RVA: 0x000F0C54 File Offset: 0x000EF054
		public static Color brown
		{
			get
			{
				return new Color(0.5882353f, 0.29411766f, 0f);
			}
		}

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x06002D08 RID: 11528 RVA: 0x000F0C6A File Offset: 0x000EF06A
		public static Color beige
		{
			get
			{
				return new Color(0.9607843f, 0.9607843f, 0.8627451f);
			}
		}

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x06002D09 RID: 11529 RVA: 0x000F0C80 File Offset: 0x000EF080
		public static Color coral
		{
			get
			{
				return new Color(1f, 0.49803922f, 0.3137255f);
			}
		}

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x06002D0A RID: 11530 RVA: 0x000F0C96 File Offset: 0x000EF096
		public static Color orange
		{
			get
			{
				return LeapColor.lerp(LeapColor.red, LeapColor.yellow, 0.5f);
			}
		}

		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x06002D0B RID: 11531 RVA: 0x000F0CAC File Offset: 0x000EF0AC
		public static Color khaki
		{
			get
			{
				return new Color(0.7647059f, 0.6901961f, 0.5686275f);
			}
		}

		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x06002D0C RID: 11532 RVA: 0x000F0CC2 File Offset: 0x000EF0C2
		public static Color amber
		{
			get
			{
				return new Color(1f, 0.7490196f, 0f);
			}
		}

		// Token: 0x170005A9 RID: 1449
		// (get) Token: 0x06002D0D RID: 11533 RVA: 0x000F0CD8 File Offset: 0x000EF0D8
		public static Color yellow
		{
			get
			{
				return Color.yellow;
			}
		}

		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x06002D0E RID: 11534 RVA: 0x000F0CDF File Offset: 0x000EF0DF
		public static Color gold
		{
			get
			{
				return new Color(0.83137256f, 0.6862745f, 0.21568628f);
			}
		}

		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x06002D0F RID: 11535 RVA: 0x000F0CF5 File Offset: 0x000EF0F5
		public static Color green
		{
			get
			{
				return Color.green;
			}
		}

		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x06002D10 RID: 11536 RVA: 0x000F0CFC File Offset: 0x000EF0FC
		public static Color forest
		{
			get
			{
				return new Color(0.13333334f, 0.54509807f, 0.13333334f);
			}
		}

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x06002D11 RID: 11537 RVA: 0x000F0D12 File Offset: 0x000EF112
		public static Color lime
		{
			get
			{
				return new Color(0.61960787f, 0.99215686f, 0.21960784f);
			}
		}

		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x06002D12 RID: 11538 RVA: 0x000F0D28 File Offset: 0x000EF128
		public static Color mint
		{
			get
			{
				return new Color(0.59607846f, 0.9843137f, 0.59607846f);
			}
		}

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x06002D13 RID: 11539 RVA: 0x000F0D3E File Offset: 0x000EF13E
		public static Color olive
		{
			get
			{
				return new Color(0.5019608f, 0.5019608f, 0f);
			}
		}

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x06002D14 RID: 11540 RVA: 0x000F0D54 File Offset: 0x000EF154
		public static Color jade
		{
			get
			{
				return new Color(0f, 0.65882355f, 0.41960785f);
			}
		}

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x06002D15 RID: 11541 RVA: 0x000F0D6A File Offset: 0x000EF16A
		public static Color teal
		{
			get
			{
				return new Color(0f, 0.5019608f, 0.5019608f);
			}
		}

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x06002D16 RID: 11542 RVA: 0x000F0D80 File Offset: 0x000EF180
		public static Color veridian
		{
			get
			{
				return new Color(0.2509804f, 0.50980395f, 0.42745098f);
			}
		}

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x06002D17 RID: 11543 RVA: 0x000F0D96 File Offset: 0x000EF196
		public static Color turquoise
		{
			get
			{
				return new Color(0.2509804f, 0.8784314f, 0.8156863f);
			}
		}

		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x06002D18 RID: 11544 RVA: 0x000F0DAC File Offset: 0x000EF1AC
		public static Color cyan
		{
			get
			{
				return Color.cyan;
			}
		}

		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x06002D19 RID: 11545 RVA: 0x000F0DB3 File Offset: 0x000EF1B3
		public static Color cerulean
		{
			get
			{
				return new Color(0f, 0.48235294f, 0.654902f);
			}
		}

		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x06002D1A RID: 11546 RVA: 0x000F0DC9 File Offset: 0x000EF1C9
		public static Color aqua
		{
			get
			{
				return new Color(0.56078434f, 0.8784314f, 0.96862745f);
			}
		}

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x06002D1B RID: 11547 RVA: 0x000F0DDF File Offset: 0x000EF1DF
		public static Color electricBlue
		{
			get
			{
				return new Color(0.49019608f, 0.9764706f, 1f);
			}
		}

		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x06002D1C RID: 11548 RVA: 0x000F0DF5 File Offset: 0x000EF1F5
		public static Color blue
		{
			get
			{
				return Color.blue;
			}
		}

		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x06002D1D RID: 11549 RVA: 0x000F0DFC File Offset: 0x000EF1FC
		public static Color navy
		{
			get
			{
				return new Color(0f, 0f, 0.5019608f);
			}
		}

		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x06002D1E RID: 11550 RVA: 0x000F0E12 File Offset: 0x000EF212
		public static Color periwinkle
		{
			get
			{
				return new Color(0.8f, 0.8f, 1f);
			}
		}

		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x06002D1F RID: 11551 RVA: 0x000F0E28 File Offset: 0x000EF228
		public static Color purple
		{
			get
			{
				return LeapColor.lerp(LeapColor.magenta, LeapColor.blue, 0.3f);
			}
		}

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x06002D20 RID: 11552 RVA: 0x000F0E3E File Offset: 0x000EF23E
		public static Color violet
		{
			get
			{
				return new Color(0.49803922f, 0f, 1f);
			}
		}

		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x06002D21 RID: 11553 RVA: 0x000F0E54 File Offset: 0x000EF254
		public static Color lavender
		{
			get
			{
				return new Color(0.70980394f, 0.49411765f, 0.8627451f);
			}
		}

		// Token: 0x06002D22 RID: 11554 RVA: 0x000F0E6A File Offset: 0x000EF26A
		private static Color lerp(Color a, Color b, float amount)
		{
			return Color.Lerp(a, b, amount);
		}
	}
}
