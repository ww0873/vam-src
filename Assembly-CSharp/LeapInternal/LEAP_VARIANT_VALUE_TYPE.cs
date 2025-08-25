using System;
using System.Runtime.InteropServices;

namespace LeapInternal
{
	// Token: 0x02000614 RID: 1556
	[StructLayout(LayoutKind.Explicit, Pack = 1)]
	public struct LEAP_VARIANT_VALUE_TYPE
	{
		// Token: 0x04002097 RID: 8343
		[FieldOffset(0)]
		public eLeapValueType type;

		// Token: 0x04002098 RID: 8344
		[FieldOffset(4)]
		public int boolValue;

		// Token: 0x04002099 RID: 8345
		[FieldOffset(4)]
		public int intValue;

		// Token: 0x0400209A RID: 8346
		[FieldOffset(4)]
		public float floatValue;
	}
}
