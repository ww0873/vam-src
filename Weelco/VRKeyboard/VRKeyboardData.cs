using System;
using UnityEngine;

namespace Weelco.VRKeyboard
{
	// Token: 0x02000599 RID: 1433
	public abstract class VRKeyboardData : MonoBehaviour
	{
		// Token: 0x06002407 RID: 9223 RVA: 0x000D02D2 File Offset: 0x000CE6D2
		protected VRKeyboardData()
		{
		}

		// Token: 0x06002408 RID: 9224 RVA: 0x000D02DC File Offset: 0x000CE6DC
		// Note: this type is marked as 'beforefieldinit'.
		static VRKeyboardData()
		{
		}

		// Token: 0x04001E52 RID: 7762
		public const string ABC = "abc";

		// Token: 0x04001E53 RID: 7763
		public const string SYM = "sym";

		// Token: 0x04001E54 RID: 7764
		public const string BACK = "BACK";

		// Token: 0x04001E55 RID: 7765
		public const string ENTER = "ENTER";

		// Token: 0x04001E56 RID: 7766
		public const string UP = "UP";

		// Token: 0x04001E57 RID: 7767
		public const string LOW = "LOW";

		// Token: 0x04001E58 RID: 7768
		public static readonly string[] allLettersLowercase = new string[]
		{
			"1",
			"2",
			"3",
			"4",
			"5",
			"6",
			"7",
			"8",
			"9",
			"0",
			"q",
			"w",
			"e",
			"r",
			"t",
			"y",
			"u",
			"i",
			"o",
			"p",
			"sym",
			"a",
			"s",
			"d",
			"f",
			"g",
			"h",
			"j",
			"k",
			"l",
			"UP",
			"z",
			"x",
			"c",
			"v",
			"b",
			"n",
			"m",
			"BACK",
			".com",
			"@",
			" ",
			".",
			"ENTER"
		};

		// Token: 0x04001E59 RID: 7769
		public static readonly string[] allLettersUppercase = new string[]
		{
			"1",
			"2",
			"3",
			"4",
			"5",
			"6",
			"7",
			"8",
			"9",
			"0",
			"Q",
			"W",
			"E",
			"R",
			"T",
			"Y",
			"U",
			"I",
			"O",
			"P",
			"sym",
			"A",
			"S",
			"D",
			"F",
			"G",
			"H",
			"J",
			"K",
			"L",
			"LOW",
			"Z",
			"X",
			"C",
			"V",
			"B",
			"N",
			"M",
			"BACK",
			".com",
			"@",
			" ",
			".",
			"ENTER"
		};

		// Token: 0x04001E5A RID: 7770
		public static readonly string[] allSpecials = new string[]
		{
			"1",
			"2",
			"3",
			"4",
			"5",
			"6",
			"7",
			"8",
			"9",
			"0",
			"!",
			"~",
			"#",
			"$",
			"%",
			"^",
			"&",
			"*",
			"(",
			")",
			"abc",
			"-",
			"_",
			"+",
			"=",
			"\\",
			";",
			":",
			"'",
			"\"",
			"№",
			"{",
			"}",
			"<",
			">",
			",",
			"/",
			"?",
			"BACK",
			".com",
			"@",
			" ",
			".",
			"ENTER"
		};
	}
}
