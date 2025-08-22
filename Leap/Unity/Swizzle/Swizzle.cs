using System;
using UnityEngine;

namespace Leap.Unity.Swizzle
{
	// Token: 0x0200074A RID: 1866
	public static class Swizzle
	{
		// Token: 0x06002D9C RID: 11676 RVA: 0x000F3473 File Offset: 0x000F1873
		public static Vector2 xx(this Vector2 vector)
		{
			return new Vector2(vector.x, vector.x);
		}

		// Token: 0x06002D9D RID: 11677 RVA: 0x000F3488 File Offset: 0x000F1888
		public static Vector2 xy(this Vector2 vector)
		{
			return new Vector2(vector.x, vector.y);
		}

		// Token: 0x06002D9E RID: 11678 RVA: 0x000F349D File Offset: 0x000F189D
		public static Vector2 yx(this Vector2 vector)
		{
			return new Vector2(vector.y, vector.x);
		}

		// Token: 0x06002D9F RID: 11679 RVA: 0x000F34B2 File Offset: 0x000F18B2
		public static Vector2 yy(this Vector2 vector)
		{
			return new Vector2(vector.y, vector.y);
		}

		// Token: 0x06002DA0 RID: 11680 RVA: 0x000F34C7 File Offset: 0x000F18C7
		public static Vector3 xxx(this Vector2 vector)
		{
			return new Vector3(vector.x, vector.x, vector.x);
		}

		// Token: 0x06002DA1 RID: 11681 RVA: 0x000F34E3 File Offset: 0x000F18E3
		public static Vector3 xxy(this Vector2 vector)
		{
			return new Vector3(vector.x, vector.x, vector.y);
		}

		// Token: 0x06002DA2 RID: 11682 RVA: 0x000F34FF File Offset: 0x000F18FF
		public static Vector3 xyx(this Vector2 vector)
		{
			return new Vector3(vector.x, vector.y, vector.x);
		}

		// Token: 0x06002DA3 RID: 11683 RVA: 0x000F351B File Offset: 0x000F191B
		public static Vector3 xyy(this Vector2 vector)
		{
			return new Vector3(vector.x, vector.y, vector.y);
		}

		// Token: 0x06002DA4 RID: 11684 RVA: 0x000F3537 File Offset: 0x000F1937
		public static Vector3 yxx(this Vector2 vector)
		{
			return new Vector3(vector.y, vector.x, vector.x);
		}

		// Token: 0x06002DA5 RID: 11685 RVA: 0x000F3553 File Offset: 0x000F1953
		public static Vector3 yxy(this Vector2 vector)
		{
			return new Vector3(vector.y, vector.x, vector.y);
		}

		// Token: 0x06002DA6 RID: 11686 RVA: 0x000F356F File Offset: 0x000F196F
		public static Vector3 yyx(this Vector2 vector)
		{
			return new Vector3(vector.y, vector.y, vector.x);
		}

		// Token: 0x06002DA7 RID: 11687 RVA: 0x000F358B File Offset: 0x000F198B
		public static Vector3 yyy(this Vector2 vector)
		{
			return new Vector3(vector.y, vector.y, vector.y);
		}

		// Token: 0x06002DA8 RID: 11688 RVA: 0x000F35A7 File Offset: 0x000F19A7
		public static Vector4 xxxx(this Vector2 vector)
		{
			return new Vector4(vector.x, vector.x, vector.x, vector.x);
		}

		// Token: 0x06002DA9 RID: 11689 RVA: 0x000F35CA File Offset: 0x000F19CA
		public static Vector4 xxxy(this Vector2 vector)
		{
			return new Vector4(vector.x, vector.x, vector.x, vector.y);
		}

		// Token: 0x06002DAA RID: 11690 RVA: 0x000F35ED File Offset: 0x000F19ED
		public static Vector4 xxyx(this Vector2 vector)
		{
			return new Vector4(vector.x, vector.x, vector.y, vector.x);
		}

		// Token: 0x06002DAB RID: 11691 RVA: 0x000F3610 File Offset: 0x000F1A10
		public static Vector4 xxyy(this Vector2 vector)
		{
			return new Vector4(vector.x, vector.x, vector.y, vector.y);
		}

		// Token: 0x06002DAC RID: 11692 RVA: 0x000F3633 File Offset: 0x000F1A33
		public static Vector4 xyxx(this Vector2 vector)
		{
			return new Vector4(vector.x, vector.y, vector.x, vector.x);
		}

		// Token: 0x06002DAD RID: 11693 RVA: 0x000F3656 File Offset: 0x000F1A56
		public static Vector4 xyxy(this Vector2 vector)
		{
			return new Vector4(vector.x, vector.y, vector.x, vector.y);
		}

		// Token: 0x06002DAE RID: 11694 RVA: 0x000F3679 File Offset: 0x000F1A79
		public static Vector4 xyyx(this Vector2 vector)
		{
			return new Vector4(vector.x, vector.y, vector.y, vector.x);
		}

		// Token: 0x06002DAF RID: 11695 RVA: 0x000F369C File Offset: 0x000F1A9C
		public static Vector4 xyyy(this Vector2 vector)
		{
			return new Vector4(vector.x, vector.y, vector.y, vector.y);
		}

		// Token: 0x06002DB0 RID: 11696 RVA: 0x000F36BF File Offset: 0x000F1ABF
		public static Vector4 yxxx(this Vector2 vector)
		{
			return new Vector4(vector.y, vector.x, vector.x, vector.x);
		}

		// Token: 0x06002DB1 RID: 11697 RVA: 0x000F36E2 File Offset: 0x000F1AE2
		public static Vector4 yxxy(this Vector2 vector)
		{
			return new Vector4(vector.y, vector.x, vector.x, vector.y);
		}

		// Token: 0x06002DB2 RID: 11698 RVA: 0x000F3705 File Offset: 0x000F1B05
		public static Vector4 yxyx(this Vector2 vector)
		{
			return new Vector4(vector.y, vector.x, vector.y, vector.x);
		}

		// Token: 0x06002DB3 RID: 11699 RVA: 0x000F3728 File Offset: 0x000F1B28
		public static Vector4 yxyy(this Vector2 vector)
		{
			return new Vector4(vector.y, vector.x, vector.y, vector.y);
		}

		// Token: 0x06002DB4 RID: 11700 RVA: 0x000F374B File Offset: 0x000F1B4B
		public static Vector4 yyxx(this Vector2 vector)
		{
			return new Vector4(vector.y, vector.y, vector.x, vector.x);
		}

		// Token: 0x06002DB5 RID: 11701 RVA: 0x000F376E File Offset: 0x000F1B6E
		public static Vector4 yyxy(this Vector2 vector)
		{
			return new Vector4(vector.y, vector.y, vector.x, vector.y);
		}

		// Token: 0x06002DB6 RID: 11702 RVA: 0x000F3791 File Offset: 0x000F1B91
		public static Vector4 yyyx(this Vector2 vector)
		{
			return new Vector4(vector.y, vector.y, vector.y, vector.x);
		}

		// Token: 0x06002DB7 RID: 11703 RVA: 0x000F37B4 File Offset: 0x000F1BB4
		public static Vector4 yyyy(this Vector2 vector)
		{
			return new Vector4(vector.y, vector.y, vector.y, vector.y);
		}

		// Token: 0x06002DB8 RID: 11704 RVA: 0x000F37D7 File Offset: 0x000F1BD7
		public static Vector2 xx(this Vector3 vector)
		{
			return new Vector2(vector.x, vector.x);
		}

		// Token: 0x06002DB9 RID: 11705 RVA: 0x000F37EC File Offset: 0x000F1BEC
		public static Vector2 xy(this Vector3 vector)
		{
			return new Vector2(vector.x, vector.y);
		}

		// Token: 0x06002DBA RID: 11706 RVA: 0x000F3801 File Offset: 0x000F1C01
		public static Vector2 xz(this Vector3 vector)
		{
			return new Vector2(vector.x, vector.z);
		}

		// Token: 0x06002DBB RID: 11707 RVA: 0x000F3816 File Offset: 0x000F1C16
		public static Vector2 yx(this Vector3 vector)
		{
			return new Vector2(vector.y, vector.x);
		}

		// Token: 0x06002DBC RID: 11708 RVA: 0x000F382B File Offset: 0x000F1C2B
		public static Vector2 yy(this Vector3 vector)
		{
			return new Vector2(vector.y, vector.y);
		}

		// Token: 0x06002DBD RID: 11709 RVA: 0x000F3840 File Offset: 0x000F1C40
		public static Vector2 yz(this Vector3 vector)
		{
			return new Vector2(vector.y, vector.z);
		}

		// Token: 0x06002DBE RID: 11710 RVA: 0x000F3855 File Offset: 0x000F1C55
		public static Vector2 zx(this Vector3 vector)
		{
			return new Vector2(vector.z, vector.x);
		}

		// Token: 0x06002DBF RID: 11711 RVA: 0x000F386A File Offset: 0x000F1C6A
		public static Vector2 zy(this Vector3 vector)
		{
			return new Vector2(vector.z, vector.y);
		}

		// Token: 0x06002DC0 RID: 11712 RVA: 0x000F387F File Offset: 0x000F1C7F
		public static Vector2 zz(this Vector3 vector)
		{
			return new Vector2(vector.z, vector.z);
		}

		// Token: 0x06002DC1 RID: 11713 RVA: 0x000F3894 File Offset: 0x000F1C94
		public static Vector3 xxx(this Vector3 vector)
		{
			return new Vector3(vector.x, vector.x, vector.x);
		}

		// Token: 0x06002DC2 RID: 11714 RVA: 0x000F38B0 File Offset: 0x000F1CB0
		public static Vector3 xxy(this Vector3 vector)
		{
			return new Vector3(vector.x, vector.x, vector.y);
		}

		// Token: 0x06002DC3 RID: 11715 RVA: 0x000F38CC File Offset: 0x000F1CCC
		public static Vector3 xxz(this Vector3 vector)
		{
			return new Vector3(vector.x, vector.x, vector.z);
		}

		// Token: 0x06002DC4 RID: 11716 RVA: 0x000F38E8 File Offset: 0x000F1CE8
		public static Vector3 xyx(this Vector3 vector)
		{
			return new Vector3(vector.x, vector.y, vector.x);
		}

		// Token: 0x06002DC5 RID: 11717 RVA: 0x000F3904 File Offset: 0x000F1D04
		public static Vector3 xyy(this Vector3 vector)
		{
			return new Vector3(vector.x, vector.y, vector.y);
		}

		// Token: 0x06002DC6 RID: 11718 RVA: 0x000F3920 File Offset: 0x000F1D20
		public static Vector3 xyz(this Vector3 vector)
		{
			return new Vector3(vector.x, vector.y, vector.z);
		}

		// Token: 0x06002DC7 RID: 11719 RVA: 0x000F393C File Offset: 0x000F1D3C
		public static Vector3 xzx(this Vector3 vector)
		{
			return new Vector3(vector.x, vector.z, vector.x);
		}

		// Token: 0x06002DC8 RID: 11720 RVA: 0x000F3958 File Offset: 0x000F1D58
		public static Vector3 xzy(this Vector3 vector)
		{
			return new Vector3(vector.x, vector.z, vector.y);
		}

		// Token: 0x06002DC9 RID: 11721 RVA: 0x000F3974 File Offset: 0x000F1D74
		public static Vector3 xzz(this Vector3 vector)
		{
			return new Vector3(vector.x, vector.z, vector.z);
		}

		// Token: 0x06002DCA RID: 11722 RVA: 0x000F3990 File Offset: 0x000F1D90
		public static Vector3 yxx(this Vector3 vector)
		{
			return new Vector3(vector.y, vector.x, vector.x);
		}

		// Token: 0x06002DCB RID: 11723 RVA: 0x000F39AC File Offset: 0x000F1DAC
		public static Vector3 yxy(this Vector3 vector)
		{
			return new Vector3(vector.y, vector.x, vector.y);
		}

		// Token: 0x06002DCC RID: 11724 RVA: 0x000F39C8 File Offset: 0x000F1DC8
		public static Vector3 yxz(this Vector3 vector)
		{
			return new Vector3(vector.y, vector.x, vector.z);
		}

		// Token: 0x06002DCD RID: 11725 RVA: 0x000F39E4 File Offset: 0x000F1DE4
		public static Vector3 yyx(this Vector3 vector)
		{
			return new Vector3(vector.y, vector.y, vector.x);
		}

		// Token: 0x06002DCE RID: 11726 RVA: 0x000F3A00 File Offset: 0x000F1E00
		public static Vector3 yyy(this Vector3 vector)
		{
			return new Vector3(vector.y, vector.y, vector.y);
		}

		// Token: 0x06002DCF RID: 11727 RVA: 0x000F3A1C File Offset: 0x000F1E1C
		public static Vector3 yyz(this Vector3 vector)
		{
			return new Vector3(vector.y, vector.y, vector.z);
		}

		// Token: 0x06002DD0 RID: 11728 RVA: 0x000F3A38 File Offset: 0x000F1E38
		public static Vector3 yzx(this Vector3 vector)
		{
			return new Vector3(vector.y, vector.z, vector.x);
		}

		// Token: 0x06002DD1 RID: 11729 RVA: 0x000F3A54 File Offset: 0x000F1E54
		public static Vector3 yzy(this Vector3 vector)
		{
			return new Vector3(vector.y, vector.z, vector.y);
		}

		// Token: 0x06002DD2 RID: 11730 RVA: 0x000F3A70 File Offset: 0x000F1E70
		public static Vector3 yzz(this Vector3 vector)
		{
			return new Vector3(vector.y, vector.z, vector.z);
		}

		// Token: 0x06002DD3 RID: 11731 RVA: 0x000F3A8C File Offset: 0x000F1E8C
		public static Vector3 zxx(this Vector3 vector)
		{
			return new Vector3(vector.z, vector.x, vector.x);
		}

		// Token: 0x06002DD4 RID: 11732 RVA: 0x000F3AA8 File Offset: 0x000F1EA8
		public static Vector3 zxy(this Vector3 vector)
		{
			return new Vector3(vector.z, vector.x, vector.y);
		}

		// Token: 0x06002DD5 RID: 11733 RVA: 0x000F3AC4 File Offset: 0x000F1EC4
		public static Vector3 zxz(this Vector3 vector)
		{
			return new Vector3(vector.z, vector.x, vector.z);
		}

		// Token: 0x06002DD6 RID: 11734 RVA: 0x000F3AE0 File Offset: 0x000F1EE0
		public static Vector3 zyx(this Vector3 vector)
		{
			return new Vector3(vector.z, vector.y, vector.x);
		}

		// Token: 0x06002DD7 RID: 11735 RVA: 0x000F3AFC File Offset: 0x000F1EFC
		public static Vector3 zyy(this Vector3 vector)
		{
			return new Vector3(vector.z, vector.y, vector.y);
		}

		// Token: 0x06002DD8 RID: 11736 RVA: 0x000F3B18 File Offset: 0x000F1F18
		public static Vector3 zyz(this Vector3 vector)
		{
			return new Vector3(vector.z, vector.y, vector.z);
		}

		// Token: 0x06002DD9 RID: 11737 RVA: 0x000F3B34 File Offset: 0x000F1F34
		public static Vector3 zzx(this Vector3 vector)
		{
			return new Vector3(vector.z, vector.z, vector.x);
		}

		// Token: 0x06002DDA RID: 11738 RVA: 0x000F3B50 File Offset: 0x000F1F50
		public static Vector3 zzy(this Vector3 vector)
		{
			return new Vector3(vector.z, vector.z, vector.y);
		}

		// Token: 0x06002DDB RID: 11739 RVA: 0x000F3B6C File Offset: 0x000F1F6C
		public static Vector3 zzz(this Vector3 vector)
		{
			return new Vector3(vector.z, vector.z, vector.z);
		}

		// Token: 0x06002DDC RID: 11740 RVA: 0x000F3B88 File Offset: 0x000F1F88
		public static Vector4 xxxx(this Vector3 vector)
		{
			return new Vector4(vector.x, vector.x, vector.x, vector.x);
		}

		// Token: 0x06002DDD RID: 11741 RVA: 0x000F3BAB File Offset: 0x000F1FAB
		public static Vector4 xxxy(this Vector3 vector)
		{
			return new Vector4(vector.x, vector.x, vector.x, vector.y);
		}

		// Token: 0x06002DDE RID: 11742 RVA: 0x000F3BCE File Offset: 0x000F1FCE
		public static Vector4 xxxz(this Vector3 vector)
		{
			return new Vector4(vector.x, vector.x, vector.x, vector.z);
		}

		// Token: 0x06002DDF RID: 11743 RVA: 0x000F3BF1 File Offset: 0x000F1FF1
		public static Vector4 xxyx(this Vector3 vector)
		{
			return new Vector4(vector.x, vector.x, vector.y, vector.x);
		}

		// Token: 0x06002DE0 RID: 11744 RVA: 0x000F3C14 File Offset: 0x000F2014
		public static Vector4 xxyy(this Vector3 vector)
		{
			return new Vector4(vector.x, vector.x, vector.y, vector.y);
		}

		// Token: 0x06002DE1 RID: 11745 RVA: 0x000F3C37 File Offset: 0x000F2037
		public static Vector4 xxyz(this Vector3 vector)
		{
			return new Vector4(vector.x, vector.x, vector.y, vector.z);
		}

		// Token: 0x06002DE2 RID: 11746 RVA: 0x000F3C5A File Offset: 0x000F205A
		public static Vector4 xxzx(this Vector3 vector)
		{
			return new Vector4(vector.x, vector.x, vector.z, vector.x);
		}

		// Token: 0x06002DE3 RID: 11747 RVA: 0x000F3C7D File Offset: 0x000F207D
		public static Vector4 xxzy(this Vector3 vector)
		{
			return new Vector4(vector.x, vector.x, vector.z, vector.y);
		}

		// Token: 0x06002DE4 RID: 11748 RVA: 0x000F3CA0 File Offset: 0x000F20A0
		public static Vector4 xxzz(this Vector3 vector)
		{
			return new Vector4(vector.x, vector.x, vector.z, vector.z);
		}

		// Token: 0x06002DE5 RID: 11749 RVA: 0x000F3CC3 File Offset: 0x000F20C3
		public static Vector4 xyxx(this Vector3 vector)
		{
			return new Vector4(vector.x, vector.y, vector.x, vector.x);
		}

		// Token: 0x06002DE6 RID: 11750 RVA: 0x000F3CE6 File Offset: 0x000F20E6
		public static Vector4 xyxy(this Vector3 vector)
		{
			return new Vector4(vector.x, vector.y, vector.x, vector.y);
		}

		// Token: 0x06002DE7 RID: 11751 RVA: 0x000F3D09 File Offset: 0x000F2109
		public static Vector4 xyxz(this Vector3 vector)
		{
			return new Vector4(vector.x, vector.y, vector.x, vector.z);
		}

		// Token: 0x06002DE8 RID: 11752 RVA: 0x000F3D2C File Offset: 0x000F212C
		public static Vector4 xyyx(this Vector3 vector)
		{
			return new Vector4(vector.x, vector.y, vector.y, vector.x);
		}

		// Token: 0x06002DE9 RID: 11753 RVA: 0x000F3D4F File Offset: 0x000F214F
		public static Vector4 xyyy(this Vector3 vector)
		{
			return new Vector4(vector.x, vector.y, vector.y, vector.y);
		}

		// Token: 0x06002DEA RID: 11754 RVA: 0x000F3D72 File Offset: 0x000F2172
		public static Vector4 xyyz(this Vector3 vector)
		{
			return new Vector4(vector.x, vector.y, vector.y, vector.z);
		}

		// Token: 0x06002DEB RID: 11755 RVA: 0x000F3D95 File Offset: 0x000F2195
		public static Vector4 xyzx(this Vector3 vector)
		{
			return new Vector4(vector.x, vector.y, vector.z, vector.x);
		}

		// Token: 0x06002DEC RID: 11756 RVA: 0x000F3DB8 File Offset: 0x000F21B8
		public static Vector4 xyzy(this Vector3 vector)
		{
			return new Vector4(vector.x, vector.y, vector.z, vector.y);
		}

		// Token: 0x06002DED RID: 11757 RVA: 0x000F3DDB File Offset: 0x000F21DB
		public static Vector4 xyzz(this Vector3 vector)
		{
			return new Vector4(vector.x, vector.y, vector.z, vector.z);
		}

		// Token: 0x06002DEE RID: 11758 RVA: 0x000F3DFE File Offset: 0x000F21FE
		public static Vector4 xzxx(this Vector3 vector)
		{
			return new Vector4(vector.x, vector.z, vector.x, vector.x);
		}

		// Token: 0x06002DEF RID: 11759 RVA: 0x000F3E21 File Offset: 0x000F2221
		public static Vector4 xzxy(this Vector3 vector)
		{
			return new Vector4(vector.x, vector.z, vector.x, vector.y);
		}

		// Token: 0x06002DF0 RID: 11760 RVA: 0x000F3E44 File Offset: 0x000F2244
		public static Vector4 xzxz(this Vector3 vector)
		{
			return new Vector4(vector.x, vector.z, vector.x, vector.z);
		}

		// Token: 0x06002DF1 RID: 11761 RVA: 0x000F3E67 File Offset: 0x000F2267
		public static Vector4 xzyx(this Vector3 vector)
		{
			return new Vector4(vector.x, vector.z, vector.y, vector.x);
		}

		// Token: 0x06002DF2 RID: 11762 RVA: 0x000F3E8A File Offset: 0x000F228A
		public static Vector4 xzyy(this Vector3 vector)
		{
			return new Vector4(vector.x, vector.z, vector.y, vector.y);
		}

		// Token: 0x06002DF3 RID: 11763 RVA: 0x000F3EAD File Offset: 0x000F22AD
		public static Vector4 xzyz(this Vector3 vector)
		{
			return new Vector4(vector.x, vector.z, vector.y, vector.z);
		}

		// Token: 0x06002DF4 RID: 11764 RVA: 0x000F3ED0 File Offset: 0x000F22D0
		public static Vector4 xzzx(this Vector3 vector)
		{
			return new Vector4(vector.x, vector.z, vector.z, vector.x);
		}

		// Token: 0x06002DF5 RID: 11765 RVA: 0x000F3EF3 File Offset: 0x000F22F3
		public static Vector4 xzzy(this Vector3 vector)
		{
			return new Vector4(vector.x, vector.z, vector.z, vector.y);
		}

		// Token: 0x06002DF6 RID: 11766 RVA: 0x000F3F16 File Offset: 0x000F2316
		public static Vector4 xzzz(this Vector3 vector)
		{
			return new Vector4(vector.x, vector.z, vector.z, vector.z);
		}

		// Token: 0x06002DF7 RID: 11767 RVA: 0x000F3F39 File Offset: 0x000F2339
		public static Vector4 yxxx(this Vector3 vector)
		{
			return new Vector4(vector.y, vector.x, vector.x, vector.x);
		}

		// Token: 0x06002DF8 RID: 11768 RVA: 0x000F3F5C File Offset: 0x000F235C
		public static Vector4 yxxy(this Vector3 vector)
		{
			return new Vector4(vector.y, vector.x, vector.x, vector.y);
		}

		// Token: 0x06002DF9 RID: 11769 RVA: 0x000F3F7F File Offset: 0x000F237F
		public static Vector4 yxxz(this Vector3 vector)
		{
			return new Vector4(vector.y, vector.x, vector.x, vector.z);
		}

		// Token: 0x06002DFA RID: 11770 RVA: 0x000F3FA2 File Offset: 0x000F23A2
		public static Vector4 yxyx(this Vector3 vector)
		{
			return new Vector4(vector.y, vector.x, vector.y, vector.x);
		}

		// Token: 0x06002DFB RID: 11771 RVA: 0x000F3FC5 File Offset: 0x000F23C5
		public static Vector4 yxyy(this Vector3 vector)
		{
			return new Vector4(vector.y, vector.x, vector.y, vector.y);
		}

		// Token: 0x06002DFC RID: 11772 RVA: 0x000F3FE8 File Offset: 0x000F23E8
		public static Vector4 yxyz(this Vector3 vector)
		{
			return new Vector4(vector.y, vector.x, vector.y, vector.z);
		}

		// Token: 0x06002DFD RID: 11773 RVA: 0x000F400B File Offset: 0x000F240B
		public static Vector4 yxzx(this Vector3 vector)
		{
			return new Vector4(vector.y, vector.x, vector.z, vector.x);
		}

		// Token: 0x06002DFE RID: 11774 RVA: 0x000F402E File Offset: 0x000F242E
		public static Vector4 yxzy(this Vector3 vector)
		{
			return new Vector4(vector.y, vector.x, vector.z, vector.y);
		}

		// Token: 0x06002DFF RID: 11775 RVA: 0x000F4051 File Offset: 0x000F2451
		public static Vector4 yxzz(this Vector3 vector)
		{
			return new Vector4(vector.y, vector.x, vector.z, vector.z);
		}

		// Token: 0x06002E00 RID: 11776 RVA: 0x000F4074 File Offset: 0x000F2474
		public static Vector4 yyxx(this Vector3 vector)
		{
			return new Vector4(vector.y, vector.y, vector.x, vector.x);
		}

		// Token: 0x06002E01 RID: 11777 RVA: 0x000F4097 File Offset: 0x000F2497
		public static Vector4 yyxy(this Vector3 vector)
		{
			return new Vector4(vector.y, vector.y, vector.x, vector.y);
		}

		// Token: 0x06002E02 RID: 11778 RVA: 0x000F40BA File Offset: 0x000F24BA
		public static Vector4 yyxz(this Vector3 vector)
		{
			return new Vector4(vector.y, vector.y, vector.x, vector.z);
		}

		// Token: 0x06002E03 RID: 11779 RVA: 0x000F40DD File Offset: 0x000F24DD
		public static Vector4 yyyx(this Vector3 vector)
		{
			return new Vector4(vector.y, vector.y, vector.y, vector.x);
		}

		// Token: 0x06002E04 RID: 11780 RVA: 0x000F4100 File Offset: 0x000F2500
		public static Vector4 yyyy(this Vector3 vector)
		{
			return new Vector4(vector.y, vector.y, vector.y, vector.y);
		}

		// Token: 0x06002E05 RID: 11781 RVA: 0x000F4123 File Offset: 0x000F2523
		public static Vector4 yyyz(this Vector3 vector)
		{
			return new Vector4(vector.y, vector.y, vector.y, vector.z);
		}

		// Token: 0x06002E06 RID: 11782 RVA: 0x000F4146 File Offset: 0x000F2546
		public static Vector4 yyzx(this Vector3 vector)
		{
			return new Vector4(vector.y, vector.y, vector.z, vector.x);
		}

		// Token: 0x06002E07 RID: 11783 RVA: 0x000F4169 File Offset: 0x000F2569
		public static Vector4 yyzy(this Vector3 vector)
		{
			return new Vector4(vector.y, vector.y, vector.z, vector.y);
		}

		// Token: 0x06002E08 RID: 11784 RVA: 0x000F418C File Offset: 0x000F258C
		public static Vector4 yyzz(this Vector3 vector)
		{
			return new Vector4(vector.y, vector.y, vector.z, vector.z);
		}

		// Token: 0x06002E09 RID: 11785 RVA: 0x000F41AF File Offset: 0x000F25AF
		public static Vector4 yzxx(this Vector3 vector)
		{
			return new Vector4(vector.y, vector.z, vector.x, vector.x);
		}

		// Token: 0x06002E0A RID: 11786 RVA: 0x000F41D2 File Offset: 0x000F25D2
		public static Vector4 yzxy(this Vector3 vector)
		{
			return new Vector4(vector.y, vector.z, vector.x, vector.y);
		}

		// Token: 0x06002E0B RID: 11787 RVA: 0x000F41F5 File Offset: 0x000F25F5
		public static Vector4 yzxz(this Vector3 vector)
		{
			return new Vector4(vector.y, vector.z, vector.x, vector.z);
		}

		// Token: 0x06002E0C RID: 11788 RVA: 0x000F4218 File Offset: 0x000F2618
		public static Vector4 yzyx(this Vector3 vector)
		{
			return new Vector4(vector.y, vector.z, vector.y, vector.x);
		}

		// Token: 0x06002E0D RID: 11789 RVA: 0x000F423B File Offset: 0x000F263B
		public static Vector4 yzyy(this Vector3 vector)
		{
			return new Vector4(vector.y, vector.z, vector.y, vector.y);
		}

		// Token: 0x06002E0E RID: 11790 RVA: 0x000F425E File Offset: 0x000F265E
		public static Vector4 yzyz(this Vector3 vector)
		{
			return new Vector4(vector.y, vector.z, vector.y, vector.z);
		}

		// Token: 0x06002E0F RID: 11791 RVA: 0x000F4281 File Offset: 0x000F2681
		public static Vector4 yzzx(this Vector3 vector)
		{
			return new Vector4(vector.y, vector.z, vector.z, vector.x);
		}

		// Token: 0x06002E10 RID: 11792 RVA: 0x000F42A4 File Offset: 0x000F26A4
		public static Vector4 yzzy(this Vector3 vector)
		{
			return new Vector4(vector.y, vector.z, vector.z, vector.y);
		}

		// Token: 0x06002E11 RID: 11793 RVA: 0x000F42C7 File Offset: 0x000F26C7
		public static Vector4 yzzz(this Vector3 vector)
		{
			return new Vector4(vector.y, vector.z, vector.z, vector.z);
		}

		// Token: 0x06002E12 RID: 11794 RVA: 0x000F42EA File Offset: 0x000F26EA
		public static Vector4 zxxx(this Vector3 vector)
		{
			return new Vector4(vector.z, vector.x, vector.x, vector.x);
		}

		// Token: 0x06002E13 RID: 11795 RVA: 0x000F430D File Offset: 0x000F270D
		public static Vector4 zxxy(this Vector3 vector)
		{
			return new Vector4(vector.z, vector.x, vector.x, vector.y);
		}

		// Token: 0x06002E14 RID: 11796 RVA: 0x000F4330 File Offset: 0x000F2730
		public static Vector4 zxxz(this Vector3 vector)
		{
			return new Vector4(vector.z, vector.x, vector.x, vector.z);
		}

		// Token: 0x06002E15 RID: 11797 RVA: 0x000F4353 File Offset: 0x000F2753
		public static Vector4 zxyx(this Vector3 vector)
		{
			return new Vector4(vector.z, vector.x, vector.y, vector.x);
		}

		// Token: 0x06002E16 RID: 11798 RVA: 0x000F4376 File Offset: 0x000F2776
		public static Vector4 zxyy(this Vector3 vector)
		{
			return new Vector4(vector.z, vector.x, vector.y, vector.y);
		}

		// Token: 0x06002E17 RID: 11799 RVA: 0x000F4399 File Offset: 0x000F2799
		public static Vector4 zxyz(this Vector3 vector)
		{
			return new Vector4(vector.z, vector.x, vector.y, vector.z);
		}

		// Token: 0x06002E18 RID: 11800 RVA: 0x000F43BC File Offset: 0x000F27BC
		public static Vector4 zxzx(this Vector3 vector)
		{
			return new Vector4(vector.z, vector.x, vector.z, vector.x);
		}

		// Token: 0x06002E19 RID: 11801 RVA: 0x000F43DF File Offset: 0x000F27DF
		public static Vector4 zxzy(this Vector3 vector)
		{
			return new Vector4(vector.z, vector.x, vector.z, vector.y);
		}

		// Token: 0x06002E1A RID: 11802 RVA: 0x000F4402 File Offset: 0x000F2802
		public static Vector4 zxzz(this Vector3 vector)
		{
			return new Vector4(vector.z, vector.x, vector.z, vector.z);
		}

		// Token: 0x06002E1B RID: 11803 RVA: 0x000F4425 File Offset: 0x000F2825
		public static Vector4 zyxx(this Vector3 vector)
		{
			return new Vector4(vector.z, vector.y, vector.x, vector.x);
		}

		// Token: 0x06002E1C RID: 11804 RVA: 0x000F4448 File Offset: 0x000F2848
		public static Vector4 zyxy(this Vector3 vector)
		{
			return new Vector4(vector.z, vector.y, vector.x, vector.y);
		}

		// Token: 0x06002E1D RID: 11805 RVA: 0x000F446B File Offset: 0x000F286B
		public static Vector4 zyxz(this Vector3 vector)
		{
			return new Vector4(vector.z, vector.y, vector.x, vector.z);
		}

		// Token: 0x06002E1E RID: 11806 RVA: 0x000F448E File Offset: 0x000F288E
		public static Vector4 zyyx(this Vector3 vector)
		{
			return new Vector4(vector.z, vector.y, vector.y, vector.x);
		}

		// Token: 0x06002E1F RID: 11807 RVA: 0x000F44B1 File Offset: 0x000F28B1
		public static Vector4 zyyy(this Vector3 vector)
		{
			return new Vector4(vector.z, vector.y, vector.y, vector.y);
		}

		// Token: 0x06002E20 RID: 11808 RVA: 0x000F44D4 File Offset: 0x000F28D4
		public static Vector4 zyyz(this Vector3 vector)
		{
			return new Vector4(vector.z, vector.y, vector.y, vector.z);
		}

		// Token: 0x06002E21 RID: 11809 RVA: 0x000F44F7 File Offset: 0x000F28F7
		public static Vector4 zyzx(this Vector3 vector)
		{
			return new Vector4(vector.z, vector.y, vector.z, vector.x);
		}

		// Token: 0x06002E22 RID: 11810 RVA: 0x000F451A File Offset: 0x000F291A
		public static Vector4 zyzy(this Vector3 vector)
		{
			return new Vector4(vector.z, vector.y, vector.z, vector.y);
		}

		// Token: 0x06002E23 RID: 11811 RVA: 0x000F453D File Offset: 0x000F293D
		public static Vector4 zyzz(this Vector3 vector)
		{
			return new Vector4(vector.z, vector.y, vector.z, vector.z);
		}

		// Token: 0x06002E24 RID: 11812 RVA: 0x000F4560 File Offset: 0x000F2960
		public static Vector4 zzxx(this Vector3 vector)
		{
			return new Vector4(vector.z, vector.z, vector.x, vector.x);
		}

		// Token: 0x06002E25 RID: 11813 RVA: 0x000F4583 File Offset: 0x000F2983
		public static Vector4 zzxy(this Vector3 vector)
		{
			return new Vector4(vector.z, vector.z, vector.x, vector.y);
		}

		// Token: 0x06002E26 RID: 11814 RVA: 0x000F45A6 File Offset: 0x000F29A6
		public static Vector4 zzxz(this Vector3 vector)
		{
			return new Vector4(vector.z, vector.z, vector.x, vector.z);
		}

		// Token: 0x06002E27 RID: 11815 RVA: 0x000F45C9 File Offset: 0x000F29C9
		public static Vector4 zzyx(this Vector3 vector)
		{
			return new Vector4(vector.z, vector.z, vector.y, vector.x);
		}

		// Token: 0x06002E28 RID: 11816 RVA: 0x000F45EC File Offset: 0x000F29EC
		public static Vector4 zzyy(this Vector3 vector)
		{
			return new Vector4(vector.z, vector.z, vector.y, vector.y);
		}

		// Token: 0x06002E29 RID: 11817 RVA: 0x000F460F File Offset: 0x000F2A0F
		public static Vector4 zzyz(this Vector3 vector)
		{
			return new Vector4(vector.z, vector.z, vector.y, vector.z);
		}

		// Token: 0x06002E2A RID: 11818 RVA: 0x000F4632 File Offset: 0x000F2A32
		public static Vector4 zzzx(this Vector3 vector)
		{
			return new Vector4(vector.z, vector.z, vector.z, vector.x);
		}

		// Token: 0x06002E2B RID: 11819 RVA: 0x000F4655 File Offset: 0x000F2A55
		public static Vector4 zzzy(this Vector3 vector)
		{
			return new Vector4(vector.z, vector.z, vector.z, vector.y);
		}

		// Token: 0x06002E2C RID: 11820 RVA: 0x000F4678 File Offset: 0x000F2A78
		public static Vector4 zzzz(this Vector3 vector)
		{
			return new Vector4(vector.z, vector.z, vector.z, vector.z);
		}

		// Token: 0x06002E2D RID: 11821 RVA: 0x000F469B File Offset: 0x000F2A9B
		public static Vector2 xx(this Vector4 vector)
		{
			return new Vector2(vector.x, vector.x);
		}

		// Token: 0x06002E2E RID: 11822 RVA: 0x000F46B0 File Offset: 0x000F2AB0
		public static Vector2 xy(this Vector4 vector)
		{
			return new Vector2(vector.x, vector.y);
		}

		// Token: 0x06002E2F RID: 11823 RVA: 0x000F46C5 File Offset: 0x000F2AC5
		public static Vector2 xz(this Vector4 vector)
		{
			return new Vector2(vector.x, vector.z);
		}

		// Token: 0x06002E30 RID: 11824 RVA: 0x000F46DA File Offset: 0x000F2ADA
		public static Vector2 xw(this Vector4 vector)
		{
			return new Vector2(vector.x, vector.w);
		}

		// Token: 0x06002E31 RID: 11825 RVA: 0x000F46EF File Offset: 0x000F2AEF
		public static Vector2 yx(this Vector4 vector)
		{
			return new Vector2(vector.y, vector.x);
		}

		// Token: 0x06002E32 RID: 11826 RVA: 0x000F4704 File Offset: 0x000F2B04
		public static Vector2 yy(this Vector4 vector)
		{
			return new Vector2(vector.y, vector.y);
		}

		// Token: 0x06002E33 RID: 11827 RVA: 0x000F4719 File Offset: 0x000F2B19
		public static Vector2 yz(this Vector4 vector)
		{
			return new Vector2(vector.y, vector.z);
		}

		// Token: 0x06002E34 RID: 11828 RVA: 0x000F472E File Offset: 0x000F2B2E
		public static Vector2 yw(this Vector4 vector)
		{
			return new Vector2(vector.y, vector.w);
		}

		// Token: 0x06002E35 RID: 11829 RVA: 0x000F4743 File Offset: 0x000F2B43
		public static Vector2 zx(this Vector4 vector)
		{
			return new Vector2(vector.z, vector.x);
		}

		// Token: 0x06002E36 RID: 11830 RVA: 0x000F4758 File Offset: 0x000F2B58
		public static Vector2 zy(this Vector4 vector)
		{
			return new Vector2(vector.z, vector.y);
		}

		// Token: 0x06002E37 RID: 11831 RVA: 0x000F476D File Offset: 0x000F2B6D
		public static Vector2 zz(this Vector4 vector)
		{
			return new Vector2(vector.z, vector.z);
		}

		// Token: 0x06002E38 RID: 11832 RVA: 0x000F4782 File Offset: 0x000F2B82
		public static Vector2 zw(this Vector4 vector)
		{
			return new Vector2(vector.z, vector.w);
		}

		// Token: 0x06002E39 RID: 11833 RVA: 0x000F4797 File Offset: 0x000F2B97
		public static Vector2 wx(this Vector4 vector)
		{
			return new Vector2(vector.w, vector.x);
		}

		// Token: 0x06002E3A RID: 11834 RVA: 0x000F47AC File Offset: 0x000F2BAC
		public static Vector2 wy(this Vector4 vector)
		{
			return new Vector2(vector.w, vector.y);
		}

		// Token: 0x06002E3B RID: 11835 RVA: 0x000F47C1 File Offset: 0x000F2BC1
		public static Vector2 wz(this Vector4 vector)
		{
			return new Vector2(vector.w, vector.z);
		}

		// Token: 0x06002E3C RID: 11836 RVA: 0x000F47D6 File Offset: 0x000F2BD6
		public static Vector2 ww(this Vector4 vector)
		{
			return new Vector2(vector.w, vector.w);
		}

		// Token: 0x06002E3D RID: 11837 RVA: 0x000F47EB File Offset: 0x000F2BEB
		public static Vector3 xxx(this Vector4 vector)
		{
			return new Vector3(vector.x, vector.x, vector.x);
		}

		// Token: 0x06002E3E RID: 11838 RVA: 0x000F4807 File Offset: 0x000F2C07
		public static Vector3 xxy(this Vector4 vector)
		{
			return new Vector3(vector.x, vector.x, vector.y);
		}

		// Token: 0x06002E3F RID: 11839 RVA: 0x000F4823 File Offset: 0x000F2C23
		public static Vector3 xxz(this Vector4 vector)
		{
			return new Vector3(vector.x, vector.x, vector.z);
		}

		// Token: 0x06002E40 RID: 11840 RVA: 0x000F483F File Offset: 0x000F2C3F
		public static Vector3 xxw(this Vector4 vector)
		{
			return new Vector3(vector.x, vector.x, vector.w);
		}

		// Token: 0x06002E41 RID: 11841 RVA: 0x000F485B File Offset: 0x000F2C5B
		public static Vector3 xyx(this Vector4 vector)
		{
			return new Vector3(vector.x, vector.y, vector.x);
		}

		// Token: 0x06002E42 RID: 11842 RVA: 0x000F4877 File Offset: 0x000F2C77
		public static Vector3 xyy(this Vector4 vector)
		{
			return new Vector3(vector.x, vector.y, vector.y);
		}

		// Token: 0x06002E43 RID: 11843 RVA: 0x000F4893 File Offset: 0x000F2C93
		public static Vector3 xyz(this Vector4 vector)
		{
			return new Vector3(vector.x, vector.y, vector.z);
		}

		// Token: 0x06002E44 RID: 11844 RVA: 0x000F48AF File Offset: 0x000F2CAF
		public static Vector3 xyw(this Vector4 vector)
		{
			return new Vector3(vector.x, vector.y, vector.w);
		}

		// Token: 0x06002E45 RID: 11845 RVA: 0x000F48CB File Offset: 0x000F2CCB
		public static Vector3 xzx(this Vector4 vector)
		{
			return new Vector3(vector.x, vector.z, vector.x);
		}

		// Token: 0x06002E46 RID: 11846 RVA: 0x000F48E7 File Offset: 0x000F2CE7
		public static Vector3 xzy(this Vector4 vector)
		{
			return new Vector3(vector.x, vector.z, vector.y);
		}

		// Token: 0x06002E47 RID: 11847 RVA: 0x000F4903 File Offset: 0x000F2D03
		public static Vector3 xzz(this Vector4 vector)
		{
			return new Vector3(vector.x, vector.z, vector.z);
		}

		// Token: 0x06002E48 RID: 11848 RVA: 0x000F491F File Offset: 0x000F2D1F
		public static Vector3 xzw(this Vector4 vector)
		{
			return new Vector3(vector.x, vector.z, vector.w);
		}

		// Token: 0x06002E49 RID: 11849 RVA: 0x000F493B File Offset: 0x000F2D3B
		public static Vector3 xwx(this Vector4 vector)
		{
			return new Vector3(vector.x, vector.w, vector.x);
		}

		// Token: 0x06002E4A RID: 11850 RVA: 0x000F4957 File Offset: 0x000F2D57
		public static Vector3 xwy(this Vector4 vector)
		{
			return new Vector3(vector.x, vector.w, vector.y);
		}

		// Token: 0x06002E4B RID: 11851 RVA: 0x000F4973 File Offset: 0x000F2D73
		public static Vector3 xwz(this Vector4 vector)
		{
			return new Vector3(vector.x, vector.w, vector.z);
		}

		// Token: 0x06002E4C RID: 11852 RVA: 0x000F498F File Offset: 0x000F2D8F
		public static Vector3 xww(this Vector4 vector)
		{
			return new Vector3(vector.x, vector.w, vector.w);
		}

		// Token: 0x06002E4D RID: 11853 RVA: 0x000F49AB File Offset: 0x000F2DAB
		public static Vector3 yxx(this Vector4 vector)
		{
			return new Vector3(vector.y, vector.x, vector.x);
		}

		// Token: 0x06002E4E RID: 11854 RVA: 0x000F49C7 File Offset: 0x000F2DC7
		public static Vector3 yxy(this Vector4 vector)
		{
			return new Vector3(vector.y, vector.x, vector.y);
		}

		// Token: 0x06002E4F RID: 11855 RVA: 0x000F49E3 File Offset: 0x000F2DE3
		public static Vector3 yxz(this Vector4 vector)
		{
			return new Vector3(vector.y, vector.x, vector.z);
		}

		// Token: 0x06002E50 RID: 11856 RVA: 0x000F49FF File Offset: 0x000F2DFF
		public static Vector3 yxw(this Vector4 vector)
		{
			return new Vector3(vector.y, vector.x, vector.w);
		}

		// Token: 0x06002E51 RID: 11857 RVA: 0x000F4A1B File Offset: 0x000F2E1B
		public static Vector3 yyx(this Vector4 vector)
		{
			return new Vector3(vector.y, vector.y, vector.x);
		}

		// Token: 0x06002E52 RID: 11858 RVA: 0x000F4A37 File Offset: 0x000F2E37
		public static Vector3 yyy(this Vector4 vector)
		{
			return new Vector3(vector.y, vector.y, vector.y);
		}

		// Token: 0x06002E53 RID: 11859 RVA: 0x000F4A53 File Offset: 0x000F2E53
		public static Vector3 yyz(this Vector4 vector)
		{
			return new Vector3(vector.y, vector.y, vector.z);
		}

		// Token: 0x06002E54 RID: 11860 RVA: 0x000F4A6F File Offset: 0x000F2E6F
		public static Vector3 yyw(this Vector4 vector)
		{
			return new Vector3(vector.y, vector.y, vector.w);
		}

		// Token: 0x06002E55 RID: 11861 RVA: 0x000F4A8B File Offset: 0x000F2E8B
		public static Vector3 yzx(this Vector4 vector)
		{
			return new Vector3(vector.y, vector.z, vector.x);
		}

		// Token: 0x06002E56 RID: 11862 RVA: 0x000F4AA7 File Offset: 0x000F2EA7
		public static Vector3 yzy(this Vector4 vector)
		{
			return new Vector3(vector.y, vector.z, vector.y);
		}

		// Token: 0x06002E57 RID: 11863 RVA: 0x000F4AC3 File Offset: 0x000F2EC3
		public static Vector3 yzz(this Vector4 vector)
		{
			return new Vector3(vector.y, vector.z, vector.z);
		}

		// Token: 0x06002E58 RID: 11864 RVA: 0x000F4ADF File Offset: 0x000F2EDF
		public static Vector3 yzw(this Vector4 vector)
		{
			return new Vector3(vector.y, vector.z, vector.w);
		}

		// Token: 0x06002E59 RID: 11865 RVA: 0x000F4AFB File Offset: 0x000F2EFB
		public static Vector3 ywx(this Vector4 vector)
		{
			return new Vector3(vector.y, vector.w, vector.x);
		}

		// Token: 0x06002E5A RID: 11866 RVA: 0x000F4B17 File Offset: 0x000F2F17
		public static Vector3 ywy(this Vector4 vector)
		{
			return new Vector3(vector.y, vector.w, vector.y);
		}

		// Token: 0x06002E5B RID: 11867 RVA: 0x000F4B33 File Offset: 0x000F2F33
		public static Vector3 ywz(this Vector4 vector)
		{
			return new Vector3(vector.y, vector.w, vector.z);
		}

		// Token: 0x06002E5C RID: 11868 RVA: 0x000F4B4F File Offset: 0x000F2F4F
		public static Vector3 yww(this Vector4 vector)
		{
			return new Vector3(vector.y, vector.w, vector.w);
		}

		// Token: 0x06002E5D RID: 11869 RVA: 0x000F4B6B File Offset: 0x000F2F6B
		public static Vector3 zxx(this Vector4 vector)
		{
			return new Vector3(vector.z, vector.x, vector.x);
		}

		// Token: 0x06002E5E RID: 11870 RVA: 0x000F4B87 File Offset: 0x000F2F87
		public static Vector3 zxy(this Vector4 vector)
		{
			return new Vector3(vector.z, vector.x, vector.y);
		}

		// Token: 0x06002E5F RID: 11871 RVA: 0x000F4BA3 File Offset: 0x000F2FA3
		public static Vector3 zxz(this Vector4 vector)
		{
			return new Vector3(vector.z, vector.x, vector.z);
		}

		// Token: 0x06002E60 RID: 11872 RVA: 0x000F4BBF File Offset: 0x000F2FBF
		public static Vector3 zxw(this Vector4 vector)
		{
			return new Vector3(vector.z, vector.x, vector.w);
		}

		// Token: 0x06002E61 RID: 11873 RVA: 0x000F4BDB File Offset: 0x000F2FDB
		public static Vector3 zyx(this Vector4 vector)
		{
			return new Vector3(vector.z, vector.y, vector.x);
		}

		// Token: 0x06002E62 RID: 11874 RVA: 0x000F4BF7 File Offset: 0x000F2FF7
		public static Vector3 zyy(this Vector4 vector)
		{
			return new Vector3(vector.z, vector.y, vector.y);
		}

		// Token: 0x06002E63 RID: 11875 RVA: 0x000F4C13 File Offset: 0x000F3013
		public static Vector3 zyz(this Vector4 vector)
		{
			return new Vector3(vector.z, vector.y, vector.z);
		}

		// Token: 0x06002E64 RID: 11876 RVA: 0x000F4C2F File Offset: 0x000F302F
		public static Vector3 zyw(this Vector4 vector)
		{
			return new Vector3(vector.z, vector.y, vector.w);
		}

		// Token: 0x06002E65 RID: 11877 RVA: 0x000F4C4B File Offset: 0x000F304B
		public static Vector3 zzx(this Vector4 vector)
		{
			return new Vector3(vector.z, vector.z, vector.x);
		}

		// Token: 0x06002E66 RID: 11878 RVA: 0x000F4C67 File Offset: 0x000F3067
		public static Vector3 zzy(this Vector4 vector)
		{
			return new Vector3(vector.z, vector.z, vector.y);
		}

		// Token: 0x06002E67 RID: 11879 RVA: 0x000F4C83 File Offset: 0x000F3083
		public static Vector3 zzz(this Vector4 vector)
		{
			return new Vector3(vector.z, vector.z, vector.z);
		}

		// Token: 0x06002E68 RID: 11880 RVA: 0x000F4C9F File Offset: 0x000F309F
		public static Vector3 zzw(this Vector4 vector)
		{
			return new Vector3(vector.z, vector.z, vector.w);
		}

		// Token: 0x06002E69 RID: 11881 RVA: 0x000F4CBB File Offset: 0x000F30BB
		public static Vector3 zwx(this Vector4 vector)
		{
			return new Vector3(vector.z, vector.w, vector.x);
		}

		// Token: 0x06002E6A RID: 11882 RVA: 0x000F4CD7 File Offset: 0x000F30D7
		public static Vector3 zwy(this Vector4 vector)
		{
			return new Vector3(vector.z, vector.w, vector.y);
		}

		// Token: 0x06002E6B RID: 11883 RVA: 0x000F4CF3 File Offset: 0x000F30F3
		public static Vector3 zwz(this Vector4 vector)
		{
			return new Vector3(vector.z, vector.w, vector.z);
		}

		// Token: 0x06002E6C RID: 11884 RVA: 0x000F4D0F File Offset: 0x000F310F
		public static Vector3 zww(this Vector4 vector)
		{
			return new Vector3(vector.z, vector.w, vector.w);
		}

		// Token: 0x06002E6D RID: 11885 RVA: 0x000F4D2B File Offset: 0x000F312B
		public static Vector3 wxx(this Vector4 vector)
		{
			return new Vector3(vector.w, vector.x, vector.x);
		}

		// Token: 0x06002E6E RID: 11886 RVA: 0x000F4D47 File Offset: 0x000F3147
		public static Vector3 wxy(this Vector4 vector)
		{
			return new Vector3(vector.w, vector.x, vector.y);
		}

		// Token: 0x06002E6F RID: 11887 RVA: 0x000F4D63 File Offset: 0x000F3163
		public static Vector3 wxz(this Vector4 vector)
		{
			return new Vector3(vector.w, vector.x, vector.z);
		}

		// Token: 0x06002E70 RID: 11888 RVA: 0x000F4D7F File Offset: 0x000F317F
		public static Vector3 wxw(this Vector4 vector)
		{
			return new Vector3(vector.w, vector.x, vector.w);
		}

		// Token: 0x06002E71 RID: 11889 RVA: 0x000F4D9B File Offset: 0x000F319B
		public static Vector3 wyx(this Vector4 vector)
		{
			return new Vector3(vector.w, vector.y, vector.x);
		}

		// Token: 0x06002E72 RID: 11890 RVA: 0x000F4DB7 File Offset: 0x000F31B7
		public static Vector3 wyy(this Vector4 vector)
		{
			return new Vector3(vector.w, vector.y, vector.y);
		}

		// Token: 0x06002E73 RID: 11891 RVA: 0x000F4DD3 File Offset: 0x000F31D3
		public static Vector3 wyz(this Vector4 vector)
		{
			return new Vector3(vector.w, vector.y, vector.z);
		}

		// Token: 0x06002E74 RID: 11892 RVA: 0x000F4DEF File Offset: 0x000F31EF
		public static Vector3 wyw(this Vector4 vector)
		{
			return new Vector3(vector.w, vector.y, vector.w);
		}

		// Token: 0x06002E75 RID: 11893 RVA: 0x000F4E0B File Offset: 0x000F320B
		public static Vector3 wzx(this Vector4 vector)
		{
			return new Vector3(vector.w, vector.z, vector.x);
		}

		// Token: 0x06002E76 RID: 11894 RVA: 0x000F4E27 File Offset: 0x000F3227
		public static Vector3 wzy(this Vector4 vector)
		{
			return new Vector3(vector.w, vector.z, vector.y);
		}

		// Token: 0x06002E77 RID: 11895 RVA: 0x000F4E43 File Offset: 0x000F3243
		public static Vector3 wzz(this Vector4 vector)
		{
			return new Vector3(vector.w, vector.z, vector.z);
		}

		// Token: 0x06002E78 RID: 11896 RVA: 0x000F4E5F File Offset: 0x000F325F
		public static Vector3 wzw(this Vector4 vector)
		{
			return new Vector3(vector.w, vector.z, vector.w);
		}

		// Token: 0x06002E79 RID: 11897 RVA: 0x000F4E7B File Offset: 0x000F327B
		public static Vector3 wwx(this Vector4 vector)
		{
			return new Vector3(vector.w, vector.w, vector.x);
		}

		// Token: 0x06002E7A RID: 11898 RVA: 0x000F4E97 File Offset: 0x000F3297
		public static Vector3 wwy(this Vector4 vector)
		{
			return new Vector3(vector.w, vector.w, vector.y);
		}

		// Token: 0x06002E7B RID: 11899 RVA: 0x000F4EB3 File Offset: 0x000F32B3
		public static Vector3 wwz(this Vector4 vector)
		{
			return new Vector3(vector.w, vector.w, vector.z);
		}

		// Token: 0x06002E7C RID: 11900 RVA: 0x000F4ECF File Offset: 0x000F32CF
		public static Vector3 www(this Vector4 vector)
		{
			return new Vector3(vector.w, vector.w, vector.w);
		}

		// Token: 0x06002E7D RID: 11901 RVA: 0x000F4EEB File Offset: 0x000F32EB
		public static Vector4 xxxx(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.x, vector.x, vector.x);
		}

		// Token: 0x06002E7E RID: 11902 RVA: 0x000F4F0E File Offset: 0x000F330E
		public static Vector4 xxxy(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.x, vector.x, vector.y);
		}

		// Token: 0x06002E7F RID: 11903 RVA: 0x000F4F31 File Offset: 0x000F3331
		public static Vector4 xxxz(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.x, vector.x, vector.z);
		}

		// Token: 0x06002E80 RID: 11904 RVA: 0x000F4F54 File Offset: 0x000F3354
		public static Vector4 xxxw(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.x, vector.x, vector.w);
		}

		// Token: 0x06002E81 RID: 11905 RVA: 0x000F4F77 File Offset: 0x000F3377
		public static Vector4 xxyx(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.x, vector.y, vector.x);
		}

		// Token: 0x06002E82 RID: 11906 RVA: 0x000F4F9A File Offset: 0x000F339A
		public static Vector4 xxyy(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.x, vector.y, vector.y);
		}

		// Token: 0x06002E83 RID: 11907 RVA: 0x000F4FBD File Offset: 0x000F33BD
		public static Vector4 xxyz(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.x, vector.y, vector.z);
		}

		// Token: 0x06002E84 RID: 11908 RVA: 0x000F4FE0 File Offset: 0x000F33E0
		public static Vector4 xxyw(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.x, vector.y, vector.w);
		}

		// Token: 0x06002E85 RID: 11909 RVA: 0x000F5003 File Offset: 0x000F3403
		public static Vector4 xxzx(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.x, vector.z, vector.x);
		}

		// Token: 0x06002E86 RID: 11910 RVA: 0x000F5026 File Offset: 0x000F3426
		public static Vector4 xxzy(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.x, vector.z, vector.y);
		}

		// Token: 0x06002E87 RID: 11911 RVA: 0x000F5049 File Offset: 0x000F3449
		public static Vector4 xxzz(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.x, vector.z, vector.z);
		}

		// Token: 0x06002E88 RID: 11912 RVA: 0x000F506C File Offset: 0x000F346C
		public static Vector4 xxzw(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.x, vector.z, vector.w);
		}

		// Token: 0x06002E89 RID: 11913 RVA: 0x000F508F File Offset: 0x000F348F
		public static Vector4 xxwx(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.x, vector.w, vector.x);
		}

		// Token: 0x06002E8A RID: 11914 RVA: 0x000F50B2 File Offset: 0x000F34B2
		public static Vector4 xxwy(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.x, vector.w, vector.y);
		}

		// Token: 0x06002E8B RID: 11915 RVA: 0x000F50D5 File Offset: 0x000F34D5
		public static Vector4 xxwz(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.x, vector.w, vector.z);
		}

		// Token: 0x06002E8C RID: 11916 RVA: 0x000F50F8 File Offset: 0x000F34F8
		public static Vector4 xxww(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.x, vector.w, vector.w);
		}

		// Token: 0x06002E8D RID: 11917 RVA: 0x000F511B File Offset: 0x000F351B
		public static Vector4 xyxx(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.y, vector.x, vector.x);
		}

		// Token: 0x06002E8E RID: 11918 RVA: 0x000F513E File Offset: 0x000F353E
		public static Vector4 xyxy(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.y, vector.x, vector.y);
		}

		// Token: 0x06002E8F RID: 11919 RVA: 0x000F5161 File Offset: 0x000F3561
		public static Vector4 xyxz(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.y, vector.x, vector.z);
		}

		// Token: 0x06002E90 RID: 11920 RVA: 0x000F5184 File Offset: 0x000F3584
		public static Vector4 xyxw(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.y, vector.x, vector.w);
		}

		// Token: 0x06002E91 RID: 11921 RVA: 0x000F51A7 File Offset: 0x000F35A7
		public static Vector4 xyyx(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.y, vector.y, vector.x);
		}

		// Token: 0x06002E92 RID: 11922 RVA: 0x000F51CA File Offset: 0x000F35CA
		public static Vector4 xyyy(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.y, vector.y, vector.y);
		}

		// Token: 0x06002E93 RID: 11923 RVA: 0x000F51ED File Offset: 0x000F35ED
		public static Vector4 xyyz(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.y, vector.y, vector.z);
		}

		// Token: 0x06002E94 RID: 11924 RVA: 0x000F5210 File Offset: 0x000F3610
		public static Vector4 xyyw(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.y, vector.y, vector.w);
		}

		// Token: 0x06002E95 RID: 11925 RVA: 0x000F5233 File Offset: 0x000F3633
		public static Vector4 xyzx(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.y, vector.z, vector.x);
		}

		// Token: 0x06002E96 RID: 11926 RVA: 0x000F5256 File Offset: 0x000F3656
		public static Vector4 xyzy(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.y, vector.z, vector.y);
		}

		// Token: 0x06002E97 RID: 11927 RVA: 0x000F5279 File Offset: 0x000F3679
		public static Vector4 xyzz(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.y, vector.z, vector.z);
		}

		// Token: 0x06002E98 RID: 11928 RVA: 0x000F529C File Offset: 0x000F369C
		public static Vector4 xyzw(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.y, vector.z, vector.w);
		}

		// Token: 0x06002E99 RID: 11929 RVA: 0x000F52BF File Offset: 0x000F36BF
		public static Vector4 xywx(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.y, vector.w, vector.x);
		}

		// Token: 0x06002E9A RID: 11930 RVA: 0x000F52E2 File Offset: 0x000F36E2
		public static Vector4 xywy(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.y, vector.w, vector.y);
		}

		// Token: 0x06002E9B RID: 11931 RVA: 0x000F5305 File Offset: 0x000F3705
		public static Vector4 xywz(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.y, vector.w, vector.z);
		}

		// Token: 0x06002E9C RID: 11932 RVA: 0x000F5328 File Offset: 0x000F3728
		public static Vector4 xyww(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.y, vector.w, vector.w);
		}

		// Token: 0x06002E9D RID: 11933 RVA: 0x000F534B File Offset: 0x000F374B
		public static Vector4 xzxx(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.z, vector.x, vector.x);
		}

		// Token: 0x06002E9E RID: 11934 RVA: 0x000F536E File Offset: 0x000F376E
		public static Vector4 xzxy(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.z, vector.x, vector.y);
		}

		// Token: 0x06002E9F RID: 11935 RVA: 0x000F5391 File Offset: 0x000F3791
		public static Vector4 xzxz(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.z, vector.x, vector.z);
		}

		// Token: 0x06002EA0 RID: 11936 RVA: 0x000F53B4 File Offset: 0x000F37B4
		public static Vector4 xzxw(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.z, vector.x, vector.w);
		}

		// Token: 0x06002EA1 RID: 11937 RVA: 0x000F53D7 File Offset: 0x000F37D7
		public static Vector4 xzyx(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.z, vector.y, vector.x);
		}

		// Token: 0x06002EA2 RID: 11938 RVA: 0x000F53FA File Offset: 0x000F37FA
		public static Vector4 xzyy(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.z, vector.y, vector.y);
		}

		// Token: 0x06002EA3 RID: 11939 RVA: 0x000F541D File Offset: 0x000F381D
		public static Vector4 xzyz(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.z, vector.y, vector.z);
		}

		// Token: 0x06002EA4 RID: 11940 RVA: 0x000F5440 File Offset: 0x000F3840
		public static Vector4 xzyw(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.z, vector.y, vector.w);
		}

		// Token: 0x06002EA5 RID: 11941 RVA: 0x000F5463 File Offset: 0x000F3863
		public static Vector4 xzzx(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.z, vector.z, vector.x);
		}

		// Token: 0x06002EA6 RID: 11942 RVA: 0x000F5486 File Offset: 0x000F3886
		public static Vector4 xzzy(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.z, vector.z, vector.y);
		}

		// Token: 0x06002EA7 RID: 11943 RVA: 0x000F54A9 File Offset: 0x000F38A9
		public static Vector4 xzzz(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.z, vector.z, vector.z);
		}

		// Token: 0x06002EA8 RID: 11944 RVA: 0x000F54CC File Offset: 0x000F38CC
		public static Vector4 xzzw(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.z, vector.z, vector.w);
		}

		// Token: 0x06002EA9 RID: 11945 RVA: 0x000F54EF File Offset: 0x000F38EF
		public static Vector4 xzwx(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.z, vector.w, vector.x);
		}

		// Token: 0x06002EAA RID: 11946 RVA: 0x000F5512 File Offset: 0x000F3912
		public static Vector4 xzwy(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.z, vector.w, vector.y);
		}

		// Token: 0x06002EAB RID: 11947 RVA: 0x000F5535 File Offset: 0x000F3935
		public static Vector4 xzwz(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.z, vector.w, vector.z);
		}

		// Token: 0x06002EAC RID: 11948 RVA: 0x000F5558 File Offset: 0x000F3958
		public static Vector4 xzww(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.z, vector.w, vector.w);
		}

		// Token: 0x06002EAD RID: 11949 RVA: 0x000F557B File Offset: 0x000F397B
		public static Vector4 xwxx(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.w, vector.x, vector.x);
		}

		// Token: 0x06002EAE RID: 11950 RVA: 0x000F559E File Offset: 0x000F399E
		public static Vector4 xwxy(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.w, vector.x, vector.y);
		}

		// Token: 0x06002EAF RID: 11951 RVA: 0x000F55C1 File Offset: 0x000F39C1
		public static Vector4 xwxz(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.w, vector.x, vector.z);
		}

		// Token: 0x06002EB0 RID: 11952 RVA: 0x000F55E4 File Offset: 0x000F39E4
		public static Vector4 xwxw(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.w, vector.x, vector.w);
		}

		// Token: 0x06002EB1 RID: 11953 RVA: 0x000F5607 File Offset: 0x000F3A07
		public static Vector4 xwyx(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.w, vector.y, vector.x);
		}

		// Token: 0x06002EB2 RID: 11954 RVA: 0x000F562A File Offset: 0x000F3A2A
		public static Vector4 xwyy(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.w, vector.y, vector.y);
		}

		// Token: 0x06002EB3 RID: 11955 RVA: 0x000F564D File Offset: 0x000F3A4D
		public static Vector4 xwyz(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.w, vector.y, vector.z);
		}

		// Token: 0x06002EB4 RID: 11956 RVA: 0x000F5670 File Offset: 0x000F3A70
		public static Vector4 xwyw(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.w, vector.y, vector.w);
		}

		// Token: 0x06002EB5 RID: 11957 RVA: 0x000F5693 File Offset: 0x000F3A93
		public static Vector4 xwzx(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.w, vector.z, vector.x);
		}

		// Token: 0x06002EB6 RID: 11958 RVA: 0x000F56B6 File Offset: 0x000F3AB6
		public static Vector4 xwzy(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.w, vector.z, vector.y);
		}

		// Token: 0x06002EB7 RID: 11959 RVA: 0x000F56D9 File Offset: 0x000F3AD9
		public static Vector4 xwzz(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.w, vector.z, vector.z);
		}

		// Token: 0x06002EB8 RID: 11960 RVA: 0x000F56FC File Offset: 0x000F3AFC
		public static Vector4 xwzw(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.w, vector.z, vector.w);
		}

		// Token: 0x06002EB9 RID: 11961 RVA: 0x000F571F File Offset: 0x000F3B1F
		public static Vector4 xwwx(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.w, vector.w, vector.x);
		}

		// Token: 0x06002EBA RID: 11962 RVA: 0x000F5742 File Offset: 0x000F3B42
		public static Vector4 xwwy(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.w, vector.w, vector.y);
		}

		// Token: 0x06002EBB RID: 11963 RVA: 0x000F5765 File Offset: 0x000F3B65
		public static Vector4 xwwz(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.w, vector.w, vector.z);
		}

		// Token: 0x06002EBC RID: 11964 RVA: 0x000F5788 File Offset: 0x000F3B88
		public static Vector4 xwww(this Vector4 vector)
		{
			return new Vector4(vector.x, vector.w, vector.w, vector.w);
		}

		// Token: 0x06002EBD RID: 11965 RVA: 0x000F57AB File Offset: 0x000F3BAB
		public static Vector4 yxxx(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.x, vector.x, vector.x);
		}

		// Token: 0x06002EBE RID: 11966 RVA: 0x000F57CE File Offset: 0x000F3BCE
		public static Vector4 yxxy(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.x, vector.x, vector.y);
		}

		// Token: 0x06002EBF RID: 11967 RVA: 0x000F57F1 File Offset: 0x000F3BF1
		public static Vector4 yxxz(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.x, vector.x, vector.z);
		}

		// Token: 0x06002EC0 RID: 11968 RVA: 0x000F5814 File Offset: 0x000F3C14
		public static Vector4 yxxw(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.x, vector.x, vector.w);
		}

		// Token: 0x06002EC1 RID: 11969 RVA: 0x000F5837 File Offset: 0x000F3C37
		public static Vector4 yxyx(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.x, vector.y, vector.x);
		}

		// Token: 0x06002EC2 RID: 11970 RVA: 0x000F585A File Offset: 0x000F3C5A
		public static Vector4 yxyy(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.x, vector.y, vector.y);
		}

		// Token: 0x06002EC3 RID: 11971 RVA: 0x000F587D File Offset: 0x000F3C7D
		public static Vector4 yxyz(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.x, vector.y, vector.z);
		}

		// Token: 0x06002EC4 RID: 11972 RVA: 0x000F58A0 File Offset: 0x000F3CA0
		public static Vector4 yxyw(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.x, vector.y, vector.w);
		}

		// Token: 0x06002EC5 RID: 11973 RVA: 0x000F58C3 File Offset: 0x000F3CC3
		public static Vector4 yxzx(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.x, vector.z, vector.x);
		}

		// Token: 0x06002EC6 RID: 11974 RVA: 0x000F58E6 File Offset: 0x000F3CE6
		public static Vector4 yxzy(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.x, vector.z, vector.y);
		}

		// Token: 0x06002EC7 RID: 11975 RVA: 0x000F5909 File Offset: 0x000F3D09
		public static Vector4 yxzz(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.x, vector.z, vector.z);
		}

		// Token: 0x06002EC8 RID: 11976 RVA: 0x000F592C File Offset: 0x000F3D2C
		public static Vector4 yxzw(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.x, vector.z, vector.w);
		}

		// Token: 0x06002EC9 RID: 11977 RVA: 0x000F594F File Offset: 0x000F3D4F
		public static Vector4 yxwx(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.x, vector.w, vector.x);
		}

		// Token: 0x06002ECA RID: 11978 RVA: 0x000F5972 File Offset: 0x000F3D72
		public static Vector4 yxwy(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.x, vector.w, vector.y);
		}

		// Token: 0x06002ECB RID: 11979 RVA: 0x000F5995 File Offset: 0x000F3D95
		public static Vector4 yxwz(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.x, vector.w, vector.z);
		}

		// Token: 0x06002ECC RID: 11980 RVA: 0x000F59B8 File Offset: 0x000F3DB8
		public static Vector4 yxww(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.x, vector.w, vector.w);
		}

		// Token: 0x06002ECD RID: 11981 RVA: 0x000F59DB File Offset: 0x000F3DDB
		public static Vector4 yyxx(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.y, vector.x, vector.x);
		}

		// Token: 0x06002ECE RID: 11982 RVA: 0x000F59FE File Offset: 0x000F3DFE
		public static Vector4 yyxy(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.y, vector.x, vector.y);
		}

		// Token: 0x06002ECF RID: 11983 RVA: 0x000F5A21 File Offset: 0x000F3E21
		public static Vector4 yyxz(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.y, vector.x, vector.z);
		}

		// Token: 0x06002ED0 RID: 11984 RVA: 0x000F5A44 File Offset: 0x000F3E44
		public static Vector4 yyxw(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.y, vector.x, vector.w);
		}

		// Token: 0x06002ED1 RID: 11985 RVA: 0x000F5A67 File Offset: 0x000F3E67
		public static Vector4 yyyx(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.y, vector.y, vector.x);
		}

		// Token: 0x06002ED2 RID: 11986 RVA: 0x000F5A8A File Offset: 0x000F3E8A
		public static Vector4 yyyy(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.y, vector.y, vector.y);
		}

		// Token: 0x06002ED3 RID: 11987 RVA: 0x000F5AAD File Offset: 0x000F3EAD
		public static Vector4 yyyz(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.y, vector.y, vector.z);
		}

		// Token: 0x06002ED4 RID: 11988 RVA: 0x000F5AD0 File Offset: 0x000F3ED0
		public static Vector4 yyyw(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.y, vector.y, vector.w);
		}

		// Token: 0x06002ED5 RID: 11989 RVA: 0x000F5AF3 File Offset: 0x000F3EF3
		public static Vector4 yyzx(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.y, vector.z, vector.x);
		}

		// Token: 0x06002ED6 RID: 11990 RVA: 0x000F5B16 File Offset: 0x000F3F16
		public static Vector4 yyzy(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.y, vector.z, vector.y);
		}

		// Token: 0x06002ED7 RID: 11991 RVA: 0x000F5B39 File Offset: 0x000F3F39
		public static Vector4 yyzz(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.y, vector.z, vector.z);
		}

		// Token: 0x06002ED8 RID: 11992 RVA: 0x000F5B5C File Offset: 0x000F3F5C
		public static Vector4 yyzw(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.y, vector.z, vector.w);
		}

		// Token: 0x06002ED9 RID: 11993 RVA: 0x000F5B7F File Offset: 0x000F3F7F
		public static Vector4 yywx(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.y, vector.w, vector.x);
		}

		// Token: 0x06002EDA RID: 11994 RVA: 0x000F5BA2 File Offset: 0x000F3FA2
		public static Vector4 yywy(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.y, vector.w, vector.y);
		}

		// Token: 0x06002EDB RID: 11995 RVA: 0x000F5BC5 File Offset: 0x000F3FC5
		public static Vector4 yywz(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.y, vector.w, vector.z);
		}

		// Token: 0x06002EDC RID: 11996 RVA: 0x000F5BE8 File Offset: 0x000F3FE8
		public static Vector4 yyww(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.y, vector.w, vector.w);
		}

		// Token: 0x06002EDD RID: 11997 RVA: 0x000F5C0B File Offset: 0x000F400B
		public static Vector4 yzxx(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.z, vector.x, vector.x);
		}

		// Token: 0x06002EDE RID: 11998 RVA: 0x000F5C2E File Offset: 0x000F402E
		public static Vector4 yzxy(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.z, vector.x, vector.y);
		}

		// Token: 0x06002EDF RID: 11999 RVA: 0x000F5C51 File Offset: 0x000F4051
		public static Vector4 yzxz(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.z, vector.x, vector.z);
		}

		// Token: 0x06002EE0 RID: 12000 RVA: 0x000F5C74 File Offset: 0x000F4074
		public static Vector4 yzxw(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.z, vector.x, vector.w);
		}

		// Token: 0x06002EE1 RID: 12001 RVA: 0x000F5C97 File Offset: 0x000F4097
		public static Vector4 yzyx(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.z, vector.y, vector.x);
		}

		// Token: 0x06002EE2 RID: 12002 RVA: 0x000F5CBA File Offset: 0x000F40BA
		public static Vector4 yzyy(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.z, vector.y, vector.y);
		}

		// Token: 0x06002EE3 RID: 12003 RVA: 0x000F5CDD File Offset: 0x000F40DD
		public static Vector4 yzyz(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.z, vector.y, vector.z);
		}

		// Token: 0x06002EE4 RID: 12004 RVA: 0x000F5D00 File Offset: 0x000F4100
		public static Vector4 yzyw(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.z, vector.y, vector.w);
		}

		// Token: 0x06002EE5 RID: 12005 RVA: 0x000F5D23 File Offset: 0x000F4123
		public static Vector4 yzzx(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.z, vector.z, vector.x);
		}

		// Token: 0x06002EE6 RID: 12006 RVA: 0x000F5D46 File Offset: 0x000F4146
		public static Vector4 yzzy(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.z, vector.z, vector.y);
		}

		// Token: 0x06002EE7 RID: 12007 RVA: 0x000F5D69 File Offset: 0x000F4169
		public static Vector4 yzzz(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.z, vector.z, vector.z);
		}

		// Token: 0x06002EE8 RID: 12008 RVA: 0x000F5D8C File Offset: 0x000F418C
		public static Vector4 yzzw(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.z, vector.z, vector.w);
		}

		// Token: 0x06002EE9 RID: 12009 RVA: 0x000F5DAF File Offset: 0x000F41AF
		public static Vector4 yzwx(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.z, vector.w, vector.x);
		}

		// Token: 0x06002EEA RID: 12010 RVA: 0x000F5DD2 File Offset: 0x000F41D2
		public static Vector4 yzwy(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.z, vector.w, vector.y);
		}

		// Token: 0x06002EEB RID: 12011 RVA: 0x000F5DF5 File Offset: 0x000F41F5
		public static Vector4 yzwz(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.z, vector.w, vector.z);
		}

		// Token: 0x06002EEC RID: 12012 RVA: 0x000F5E18 File Offset: 0x000F4218
		public static Vector4 yzww(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.z, vector.w, vector.w);
		}

		// Token: 0x06002EED RID: 12013 RVA: 0x000F5E3B File Offset: 0x000F423B
		public static Vector4 ywxx(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.w, vector.x, vector.x);
		}

		// Token: 0x06002EEE RID: 12014 RVA: 0x000F5E5E File Offset: 0x000F425E
		public static Vector4 ywxy(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.w, vector.x, vector.y);
		}

		// Token: 0x06002EEF RID: 12015 RVA: 0x000F5E81 File Offset: 0x000F4281
		public static Vector4 ywxz(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.w, vector.x, vector.z);
		}

		// Token: 0x06002EF0 RID: 12016 RVA: 0x000F5EA4 File Offset: 0x000F42A4
		public static Vector4 ywxw(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.w, vector.x, vector.w);
		}

		// Token: 0x06002EF1 RID: 12017 RVA: 0x000F5EC7 File Offset: 0x000F42C7
		public static Vector4 ywyx(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.w, vector.y, vector.x);
		}

		// Token: 0x06002EF2 RID: 12018 RVA: 0x000F5EEA File Offset: 0x000F42EA
		public static Vector4 ywyy(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.w, vector.y, vector.y);
		}

		// Token: 0x06002EF3 RID: 12019 RVA: 0x000F5F0D File Offset: 0x000F430D
		public static Vector4 ywyz(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.w, vector.y, vector.z);
		}

		// Token: 0x06002EF4 RID: 12020 RVA: 0x000F5F30 File Offset: 0x000F4330
		public static Vector4 ywyw(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.w, vector.y, vector.w);
		}

		// Token: 0x06002EF5 RID: 12021 RVA: 0x000F5F53 File Offset: 0x000F4353
		public static Vector4 ywzx(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.w, vector.z, vector.x);
		}

		// Token: 0x06002EF6 RID: 12022 RVA: 0x000F5F76 File Offset: 0x000F4376
		public static Vector4 ywzy(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.w, vector.z, vector.y);
		}

		// Token: 0x06002EF7 RID: 12023 RVA: 0x000F5F99 File Offset: 0x000F4399
		public static Vector4 ywzz(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.w, vector.z, vector.z);
		}

		// Token: 0x06002EF8 RID: 12024 RVA: 0x000F5FBC File Offset: 0x000F43BC
		public static Vector4 ywzw(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.w, vector.z, vector.w);
		}

		// Token: 0x06002EF9 RID: 12025 RVA: 0x000F5FDF File Offset: 0x000F43DF
		public static Vector4 ywwx(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.w, vector.w, vector.x);
		}

		// Token: 0x06002EFA RID: 12026 RVA: 0x000F6002 File Offset: 0x000F4402
		public static Vector4 ywwy(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.w, vector.w, vector.y);
		}

		// Token: 0x06002EFB RID: 12027 RVA: 0x000F6025 File Offset: 0x000F4425
		public static Vector4 ywwz(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.w, vector.w, vector.z);
		}

		// Token: 0x06002EFC RID: 12028 RVA: 0x000F6048 File Offset: 0x000F4448
		public static Vector4 ywww(this Vector4 vector)
		{
			return new Vector4(vector.y, vector.w, vector.w, vector.w);
		}

		// Token: 0x06002EFD RID: 12029 RVA: 0x000F606B File Offset: 0x000F446B
		public static Vector4 zxxx(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.x, vector.x, vector.x);
		}

		// Token: 0x06002EFE RID: 12030 RVA: 0x000F608E File Offset: 0x000F448E
		public static Vector4 zxxy(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.x, vector.x, vector.y);
		}

		// Token: 0x06002EFF RID: 12031 RVA: 0x000F60B1 File Offset: 0x000F44B1
		public static Vector4 zxxz(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.x, vector.x, vector.z);
		}

		// Token: 0x06002F00 RID: 12032 RVA: 0x000F60D4 File Offset: 0x000F44D4
		public static Vector4 zxxw(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.x, vector.x, vector.w);
		}

		// Token: 0x06002F01 RID: 12033 RVA: 0x000F60F7 File Offset: 0x000F44F7
		public static Vector4 zxyx(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.x, vector.y, vector.x);
		}

		// Token: 0x06002F02 RID: 12034 RVA: 0x000F611A File Offset: 0x000F451A
		public static Vector4 zxyy(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.x, vector.y, vector.y);
		}

		// Token: 0x06002F03 RID: 12035 RVA: 0x000F613D File Offset: 0x000F453D
		public static Vector4 zxyz(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.x, vector.y, vector.z);
		}

		// Token: 0x06002F04 RID: 12036 RVA: 0x000F6160 File Offset: 0x000F4560
		public static Vector4 zxyw(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.x, vector.y, vector.w);
		}

		// Token: 0x06002F05 RID: 12037 RVA: 0x000F6183 File Offset: 0x000F4583
		public static Vector4 zxzx(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.x, vector.z, vector.x);
		}

		// Token: 0x06002F06 RID: 12038 RVA: 0x000F61A6 File Offset: 0x000F45A6
		public static Vector4 zxzy(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.x, vector.z, vector.y);
		}

		// Token: 0x06002F07 RID: 12039 RVA: 0x000F61C9 File Offset: 0x000F45C9
		public static Vector4 zxzz(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.x, vector.z, vector.z);
		}

		// Token: 0x06002F08 RID: 12040 RVA: 0x000F61EC File Offset: 0x000F45EC
		public static Vector4 zxzw(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.x, vector.z, vector.w);
		}

		// Token: 0x06002F09 RID: 12041 RVA: 0x000F620F File Offset: 0x000F460F
		public static Vector4 zxwx(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.x, vector.w, vector.x);
		}

		// Token: 0x06002F0A RID: 12042 RVA: 0x000F6232 File Offset: 0x000F4632
		public static Vector4 zxwy(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.x, vector.w, vector.y);
		}

		// Token: 0x06002F0B RID: 12043 RVA: 0x000F6255 File Offset: 0x000F4655
		public static Vector4 zxwz(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.x, vector.w, vector.z);
		}

		// Token: 0x06002F0C RID: 12044 RVA: 0x000F6278 File Offset: 0x000F4678
		public static Vector4 zxww(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.x, vector.w, vector.w);
		}

		// Token: 0x06002F0D RID: 12045 RVA: 0x000F629B File Offset: 0x000F469B
		public static Vector4 zyxx(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.y, vector.x, vector.x);
		}

		// Token: 0x06002F0E RID: 12046 RVA: 0x000F62BE File Offset: 0x000F46BE
		public static Vector4 zyxy(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.y, vector.x, vector.y);
		}

		// Token: 0x06002F0F RID: 12047 RVA: 0x000F62E1 File Offset: 0x000F46E1
		public static Vector4 zyxz(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.y, vector.x, vector.z);
		}

		// Token: 0x06002F10 RID: 12048 RVA: 0x000F6304 File Offset: 0x000F4704
		public static Vector4 zyxw(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.y, vector.x, vector.w);
		}

		// Token: 0x06002F11 RID: 12049 RVA: 0x000F6327 File Offset: 0x000F4727
		public static Vector4 zyyx(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.y, vector.y, vector.x);
		}

		// Token: 0x06002F12 RID: 12050 RVA: 0x000F634A File Offset: 0x000F474A
		public static Vector4 zyyy(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.y, vector.y, vector.y);
		}

		// Token: 0x06002F13 RID: 12051 RVA: 0x000F636D File Offset: 0x000F476D
		public static Vector4 zyyz(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.y, vector.y, vector.z);
		}

		// Token: 0x06002F14 RID: 12052 RVA: 0x000F6390 File Offset: 0x000F4790
		public static Vector4 zyyw(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.y, vector.y, vector.w);
		}

		// Token: 0x06002F15 RID: 12053 RVA: 0x000F63B3 File Offset: 0x000F47B3
		public static Vector4 zyzx(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.y, vector.z, vector.x);
		}

		// Token: 0x06002F16 RID: 12054 RVA: 0x000F63D6 File Offset: 0x000F47D6
		public static Vector4 zyzy(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.y, vector.z, vector.y);
		}

		// Token: 0x06002F17 RID: 12055 RVA: 0x000F63F9 File Offset: 0x000F47F9
		public static Vector4 zyzz(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.y, vector.z, vector.z);
		}

		// Token: 0x06002F18 RID: 12056 RVA: 0x000F641C File Offset: 0x000F481C
		public static Vector4 zyzw(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.y, vector.z, vector.w);
		}

		// Token: 0x06002F19 RID: 12057 RVA: 0x000F643F File Offset: 0x000F483F
		public static Vector4 zywx(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.y, vector.w, vector.x);
		}

		// Token: 0x06002F1A RID: 12058 RVA: 0x000F6462 File Offset: 0x000F4862
		public static Vector4 zywy(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.y, vector.w, vector.y);
		}

		// Token: 0x06002F1B RID: 12059 RVA: 0x000F6485 File Offset: 0x000F4885
		public static Vector4 zywz(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.y, vector.w, vector.z);
		}

		// Token: 0x06002F1C RID: 12060 RVA: 0x000F64A8 File Offset: 0x000F48A8
		public static Vector4 zyww(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.y, vector.w, vector.w);
		}

		// Token: 0x06002F1D RID: 12061 RVA: 0x000F64CB File Offset: 0x000F48CB
		public static Vector4 zzxx(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.z, vector.x, vector.x);
		}

		// Token: 0x06002F1E RID: 12062 RVA: 0x000F64EE File Offset: 0x000F48EE
		public static Vector4 zzxy(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.z, vector.x, vector.y);
		}

		// Token: 0x06002F1F RID: 12063 RVA: 0x000F6511 File Offset: 0x000F4911
		public static Vector4 zzxz(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.z, vector.x, vector.z);
		}

		// Token: 0x06002F20 RID: 12064 RVA: 0x000F6534 File Offset: 0x000F4934
		public static Vector4 zzxw(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.z, vector.x, vector.w);
		}

		// Token: 0x06002F21 RID: 12065 RVA: 0x000F6557 File Offset: 0x000F4957
		public static Vector4 zzyx(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.z, vector.y, vector.x);
		}

		// Token: 0x06002F22 RID: 12066 RVA: 0x000F657A File Offset: 0x000F497A
		public static Vector4 zzyy(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.z, vector.y, vector.y);
		}

		// Token: 0x06002F23 RID: 12067 RVA: 0x000F659D File Offset: 0x000F499D
		public static Vector4 zzyz(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.z, vector.y, vector.z);
		}

		// Token: 0x06002F24 RID: 12068 RVA: 0x000F65C0 File Offset: 0x000F49C0
		public static Vector4 zzyw(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.z, vector.y, vector.w);
		}

		// Token: 0x06002F25 RID: 12069 RVA: 0x000F65E3 File Offset: 0x000F49E3
		public static Vector4 zzzx(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.z, vector.z, vector.x);
		}

		// Token: 0x06002F26 RID: 12070 RVA: 0x000F6606 File Offset: 0x000F4A06
		public static Vector4 zzzy(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.z, vector.z, vector.y);
		}

		// Token: 0x06002F27 RID: 12071 RVA: 0x000F6629 File Offset: 0x000F4A29
		public static Vector4 zzzz(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.z, vector.z, vector.z);
		}

		// Token: 0x06002F28 RID: 12072 RVA: 0x000F664C File Offset: 0x000F4A4C
		public static Vector4 zzzw(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.z, vector.z, vector.w);
		}

		// Token: 0x06002F29 RID: 12073 RVA: 0x000F666F File Offset: 0x000F4A6F
		public static Vector4 zzwx(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.z, vector.w, vector.x);
		}

		// Token: 0x06002F2A RID: 12074 RVA: 0x000F6692 File Offset: 0x000F4A92
		public static Vector4 zzwy(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.z, vector.w, vector.y);
		}

		// Token: 0x06002F2B RID: 12075 RVA: 0x000F66B5 File Offset: 0x000F4AB5
		public static Vector4 zzwz(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.z, vector.w, vector.z);
		}

		// Token: 0x06002F2C RID: 12076 RVA: 0x000F66D8 File Offset: 0x000F4AD8
		public static Vector4 zzww(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.z, vector.w, vector.w);
		}

		// Token: 0x06002F2D RID: 12077 RVA: 0x000F66FB File Offset: 0x000F4AFB
		public static Vector4 zwxx(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.w, vector.x, vector.x);
		}

		// Token: 0x06002F2E RID: 12078 RVA: 0x000F671E File Offset: 0x000F4B1E
		public static Vector4 zwxy(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.w, vector.x, vector.y);
		}

		// Token: 0x06002F2F RID: 12079 RVA: 0x000F6741 File Offset: 0x000F4B41
		public static Vector4 zwxz(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.w, vector.x, vector.z);
		}

		// Token: 0x06002F30 RID: 12080 RVA: 0x000F6764 File Offset: 0x000F4B64
		public static Vector4 zwxw(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.w, vector.x, vector.w);
		}

		// Token: 0x06002F31 RID: 12081 RVA: 0x000F6787 File Offset: 0x000F4B87
		public static Vector4 zwyx(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.w, vector.y, vector.x);
		}

		// Token: 0x06002F32 RID: 12082 RVA: 0x000F67AA File Offset: 0x000F4BAA
		public static Vector4 zwyy(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.w, vector.y, vector.y);
		}

		// Token: 0x06002F33 RID: 12083 RVA: 0x000F67CD File Offset: 0x000F4BCD
		public static Vector4 zwyz(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.w, vector.y, vector.z);
		}

		// Token: 0x06002F34 RID: 12084 RVA: 0x000F67F0 File Offset: 0x000F4BF0
		public static Vector4 zwyw(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.w, vector.y, vector.w);
		}

		// Token: 0x06002F35 RID: 12085 RVA: 0x000F6813 File Offset: 0x000F4C13
		public static Vector4 zwzx(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.w, vector.z, vector.x);
		}

		// Token: 0x06002F36 RID: 12086 RVA: 0x000F6836 File Offset: 0x000F4C36
		public static Vector4 zwzy(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.w, vector.z, vector.y);
		}

		// Token: 0x06002F37 RID: 12087 RVA: 0x000F6859 File Offset: 0x000F4C59
		public static Vector4 zwzz(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.w, vector.z, vector.z);
		}

		// Token: 0x06002F38 RID: 12088 RVA: 0x000F687C File Offset: 0x000F4C7C
		public static Vector4 zwzw(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.w, vector.z, vector.w);
		}

		// Token: 0x06002F39 RID: 12089 RVA: 0x000F689F File Offset: 0x000F4C9F
		public static Vector4 zwwx(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.w, vector.w, vector.x);
		}

		// Token: 0x06002F3A RID: 12090 RVA: 0x000F68C2 File Offset: 0x000F4CC2
		public static Vector4 zwwy(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.w, vector.w, vector.y);
		}

		// Token: 0x06002F3B RID: 12091 RVA: 0x000F68E5 File Offset: 0x000F4CE5
		public static Vector4 zwwz(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.w, vector.w, vector.z);
		}

		// Token: 0x06002F3C RID: 12092 RVA: 0x000F6908 File Offset: 0x000F4D08
		public static Vector4 zwww(this Vector4 vector)
		{
			return new Vector4(vector.z, vector.w, vector.w, vector.w);
		}

		// Token: 0x06002F3D RID: 12093 RVA: 0x000F692B File Offset: 0x000F4D2B
		public static Vector4 wxxx(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.x, vector.x, vector.x);
		}

		// Token: 0x06002F3E RID: 12094 RVA: 0x000F694E File Offset: 0x000F4D4E
		public static Vector4 wxxy(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.x, vector.x, vector.y);
		}

		// Token: 0x06002F3F RID: 12095 RVA: 0x000F6971 File Offset: 0x000F4D71
		public static Vector4 wxxz(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.x, vector.x, vector.z);
		}

		// Token: 0x06002F40 RID: 12096 RVA: 0x000F6994 File Offset: 0x000F4D94
		public static Vector4 wxxw(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.x, vector.x, vector.w);
		}

		// Token: 0x06002F41 RID: 12097 RVA: 0x000F69B7 File Offset: 0x000F4DB7
		public static Vector4 wxyx(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.x, vector.y, vector.x);
		}

		// Token: 0x06002F42 RID: 12098 RVA: 0x000F69DA File Offset: 0x000F4DDA
		public static Vector4 wxyy(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.x, vector.y, vector.y);
		}

		// Token: 0x06002F43 RID: 12099 RVA: 0x000F69FD File Offset: 0x000F4DFD
		public static Vector4 wxyz(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.x, vector.y, vector.z);
		}

		// Token: 0x06002F44 RID: 12100 RVA: 0x000F6A20 File Offset: 0x000F4E20
		public static Vector4 wxyw(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.x, vector.y, vector.w);
		}

		// Token: 0x06002F45 RID: 12101 RVA: 0x000F6A43 File Offset: 0x000F4E43
		public static Vector4 wxzx(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.x, vector.z, vector.x);
		}

		// Token: 0x06002F46 RID: 12102 RVA: 0x000F6A66 File Offset: 0x000F4E66
		public static Vector4 wxzy(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.x, vector.z, vector.y);
		}

		// Token: 0x06002F47 RID: 12103 RVA: 0x000F6A89 File Offset: 0x000F4E89
		public static Vector4 wxzz(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.x, vector.z, vector.z);
		}

		// Token: 0x06002F48 RID: 12104 RVA: 0x000F6AAC File Offset: 0x000F4EAC
		public static Vector4 wxzw(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.x, vector.z, vector.w);
		}

		// Token: 0x06002F49 RID: 12105 RVA: 0x000F6ACF File Offset: 0x000F4ECF
		public static Vector4 wxwx(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.x, vector.w, vector.x);
		}

		// Token: 0x06002F4A RID: 12106 RVA: 0x000F6AF2 File Offset: 0x000F4EF2
		public static Vector4 wxwy(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.x, vector.w, vector.y);
		}

		// Token: 0x06002F4B RID: 12107 RVA: 0x000F6B15 File Offset: 0x000F4F15
		public static Vector4 wxwz(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.x, vector.w, vector.z);
		}

		// Token: 0x06002F4C RID: 12108 RVA: 0x000F6B38 File Offset: 0x000F4F38
		public static Vector4 wxww(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.x, vector.w, vector.w);
		}

		// Token: 0x06002F4D RID: 12109 RVA: 0x000F6B5B File Offset: 0x000F4F5B
		public static Vector4 wyxx(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.y, vector.x, vector.x);
		}

		// Token: 0x06002F4E RID: 12110 RVA: 0x000F6B7E File Offset: 0x000F4F7E
		public static Vector4 wyxy(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.y, vector.x, vector.y);
		}

		// Token: 0x06002F4F RID: 12111 RVA: 0x000F6BA1 File Offset: 0x000F4FA1
		public static Vector4 wyxz(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.y, vector.x, vector.z);
		}

		// Token: 0x06002F50 RID: 12112 RVA: 0x000F6BC4 File Offset: 0x000F4FC4
		public static Vector4 wyxw(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.y, vector.x, vector.w);
		}

		// Token: 0x06002F51 RID: 12113 RVA: 0x000F6BE7 File Offset: 0x000F4FE7
		public static Vector4 wyyx(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.y, vector.y, vector.x);
		}

		// Token: 0x06002F52 RID: 12114 RVA: 0x000F6C0A File Offset: 0x000F500A
		public static Vector4 wyyy(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.y, vector.y, vector.y);
		}

		// Token: 0x06002F53 RID: 12115 RVA: 0x000F6C2D File Offset: 0x000F502D
		public static Vector4 wyyz(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.y, vector.y, vector.z);
		}

		// Token: 0x06002F54 RID: 12116 RVA: 0x000F6C50 File Offset: 0x000F5050
		public static Vector4 wyyw(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.y, vector.y, vector.w);
		}

		// Token: 0x06002F55 RID: 12117 RVA: 0x000F6C73 File Offset: 0x000F5073
		public static Vector4 wyzx(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.y, vector.z, vector.x);
		}

		// Token: 0x06002F56 RID: 12118 RVA: 0x000F6C96 File Offset: 0x000F5096
		public static Vector4 wyzy(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.y, vector.z, vector.y);
		}

		// Token: 0x06002F57 RID: 12119 RVA: 0x000F6CB9 File Offset: 0x000F50B9
		public static Vector4 wyzz(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.y, vector.z, vector.z);
		}

		// Token: 0x06002F58 RID: 12120 RVA: 0x000F6CDC File Offset: 0x000F50DC
		public static Vector4 wyzw(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.y, vector.z, vector.w);
		}

		// Token: 0x06002F59 RID: 12121 RVA: 0x000F6CFF File Offset: 0x000F50FF
		public static Vector4 wywx(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.y, vector.w, vector.x);
		}

		// Token: 0x06002F5A RID: 12122 RVA: 0x000F6D22 File Offset: 0x000F5122
		public static Vector4 wywy(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.y, vector.w, vector.y);
		}

		// Token: 0x06002F5B RID: 12123 RVA: 0x000F6D45 File Offset: 0x000F5145
		public static Vector4 wywz(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.y, vector.w, vector.z);
		}

		// Token: 0x06002F5C RID: 12124 RVA: 0x000F6D68 File Offset: 0x000F5168
		public static Vector4 wyww(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.y, vector.w, vector.w);
		}

		// Token: 0x06002F5D RID: 12125 RVA: 0x000F6D8B File Offset: 0x000F518B
		public static Vector4 wzxx(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.z, vector.x, vector.x);
		}

		// Token: 0x06002F5E RID: 12126 RVA: 0x000F6DAE File Offset: 0x000F51AE
		public static Vector4 wzxy(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.z, vector.x, vector.y);
		}

		// Token: 0x06002F5F RID: 12127 RVA: 0x000F6DD1 File Offset: 0x000F51D1
		public static Vector4 wzxz(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.z, vector.x, vector.z);
		}

		// Token: 0x06002F60 RID: 12128 RVA: 0x000F6DF4 File Offset: 0x000F51F4
		public static Vector4 wzxw(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.z, vector.x, vector.w);
		}

		// Token: 0x06002F61 RID: 12129 RVA: 0x000F6E17 File Offset: 0x000F5217
		public static Vector4 wzyx(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.z, vector.y, vector.x);
		}

		// Token: 0x06002F62 RID: 12130 RVA: 0x000F6E3A File Offset: 0x000F523A
		public static Vector4 wzyy(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.z, vector.y, vector.y);
		}

		// Token: 0x06002F63 RID: 12131 RVA: 0x000F6E5D File Offset: 0x000F525D
		public static Vector4 wzyz(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.z, vector.y, vector.z);
		}

		// Token: 0x06002F64 RID: 12132 RVA: 0x000F6E80 File Offset: 0x000F5280
		public static Vector4 wzyw(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.z, vector.y, vector.w);
		}

		// Token: 0x06002F65 RID: 12133 RVA: 0x000F6EA3 File Offset: 0x000F52A3
		public static Vector4 wzzx(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.z, vector.z, vector.x);
		}

		// Token: 0x06002F66 RID: 12134 RVA: 0x000F6EC6 File Offset: 0x000F52C6
		public static Vector4 wzzy(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.z, vector.z, vector.y);
		}

		// Token: 0x06002F67 RID: 12135 RVA: 0x000F6EE9 File Offset: 0x000F52E9
		public static Vector4 wzzz(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.z, vector.z, vector.z);
		}

		// Token: 0x06002F68 RID: 12136 RVA: 0x000F6F0C File Offset: 0x000F530C
		public static Vector4 wzzw(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.z, vector.z, vector.w);
		}

		// Token: 0x06002F69 RID: 12137 RVA: 0x000F6F2F File Offset: 0x000F532F
		public static Vector4 wzwx(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.z, vector.w, vector.x);
		}

		// Token: 0x06002F6A RID: 12138 RVA: 0x000F6F52 File Offset: 0x000F5352
		public static Vector4 wzwy(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.z, vector.w, vector.y);
		}

		// Token: 0x06002F6B RID: 12139 RVA: 0x000F6F75 File Offset: 0x000F5375
		public static Vector4 wzwz(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.z, vector.w, vector.z);
		}

		// Token: 0x06002F6C RID: 12140 RVA: 0x000F6F98 File Offset: 0x000F5398
		public static Vector4 wzww(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.z, vector.w, vector.w);
		}

		// Token: 0x06002F6D RID: 12141 RVA: 0x000F6FBB File Offset: 0x000F53BB
		public static Vector4 wwxx(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.w, vector.x, vector.x);
		}

		// Token: 0x06002F6E RID: 12142 RVA: 0x000F6FDE File Offset: 0x000F53DE
		public static Vector4 wwxy(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.w, vector.x, vector.y);
		}

		// Token: 0x06002F6F RID: 12143 RVA: 0x000F7001 File Offset: 0x000F5401
		public static Vector4 wwxz(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.w, vector.x, vector.z);
		}

		// Token: 0x06002F70 RID: 12144 RVA: 0x000F7024 File Offset: 0x000F5424
		public static Vector4 wwxw(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.w, vector.x, vector.w);
		}

		// Token: 0x06002F71 RID: 12145 RVA: 0x000F7047 File Offset: 0x000F5447
		public static Vector4 wwyx(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.w, vector.y, vector.x);
		}

		// Token: 0x06002F72 RID: 12146 RVA: 0x000F706A File Offset: 0x000F546A
		public static Vector4 wwyy(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.w, vector.y, vector.y);
		}

		// Token: 0x06002F73 RID: 12147 RVA: 0x000F708D File Offset: 0x000F548D
		public static Vector4 wwyz(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.w, vector.y, vector.z);
		}

		// Token: 0x06002F74 RID: 12148 RVA: 0x000F70B0 File Offset: 0x000F54B0
		public static Vector4 wwyw(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.w, vector.y, vector.w);
		}

		// Token: 0x06002F75 RID: 12149 RVA: 0x000F70D3 File Offset: 0x000F54D3
		public static Vector4 wwzx(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.w, vector.z, vector.x);
		}

		// Token: 0x06002F76 RID: 12150 RVA: 0x000F70F6 File Offset: 0x000F54F6
		public static Vector4 wwzy(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.w, vector.z, vector.y);
		}

		// Token: 0x06002F77 RID: 12151 RVA: 0x000F7119 File Offset: 0x000F5519
		public static Vector4 wwzz(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.w, vector.z, vector.z);
		}

		// Token: 0x06002F78 RID: 12152 RVA: 0x000F713C File Offset: 0x000F553C
		public static Vector4 wwzw(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.w, vector.z, vector.w);
		}

		// Token: 0x06002F79 RID: 12153 RVA: 0x000F715F File Offset: 0x000F555F
		public static Vector4 wwwx(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.w, vector.w, vector.x);
		}

		// Token: 0x06002F7A RID: 12154 RVA: 0x000F7182 File Offset: 0x000F5582
		public static Vector4 wwwy(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.w, vector.w, vector.y);
		}

		// Token: 0x06002F7B RID: 12155 RVA: 0x000F71A5 File Offset: 0x000F55A5
		public static Vector4 wwwz(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.w, vector.w, vector.z);
		}

		// Token: 0x06002F7C RID: 12156 RVA: 0x000F71C8 File Offset: 0x000F55C8
		public static Vector4 wwww(this Vector4 vector)
		{
			return new Vector4(vector.w, vector.w, vector.w, vector.w);
		}
	}
}
