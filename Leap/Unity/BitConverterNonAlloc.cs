using System;
using System.Runtime.InteropServices;

namespace Leap.Unity
{
	// Token: 0x02000721 RID: 1825
	public static class BitConverterNonAlloc
	{
		// Token: 0x06002C76 RID: 11382 RVA: 0x000EE4BC File Offset: 0x000EC8BC
		public static ushort ToUInt16(byte[] bytes, int offset = 0)
		{
			BitConverterNonAlloc._c.Byte0 = bytes[offset++];
			BitConverterNonAlloc._c.Byte1 = bytes[offset++];
			return BitConverterNonAlloc._c.UInt16;
		}

		// Token: 0x06002C77 RID: 11383 RVA: 0x000EE4EC File Offset: 0x000EC8EC
		public static short ToInt16(byte[] bytes, int offset = 0)
		{
			BitConverterNonAlloc._c.Byte0 = bytes[offset++];
			BitConverterNonAlloc._c.Byte1 = bytes[offset++];
			return BitConverterNonAlloc._c.Int16;
		}

		// Token: 0x06002C78 RID: 11384 RVA: 0x000EE51C File Offset: 0x000EC91C
		public static uint ToUInt32(byte[] bytes, int offset = 0)
		{
			BitConverterNonAlloc._c.Byte0 = bytes[offset++];
			BitConverterNonAlloc._c.Byte1 = bytes[offset++];
			BitConverterNonAlloc._c.Byte2 = bytes[offset++];
			BitConverterNonAlloc._c.Byte3 = bytes[offset++];
			return BitConverterNonAlloc._c.UInt32;
		}

		// Token: 0x06002C79 RID: 11385 RVA: 0x000EE57C File Offset: 0x000EC97C
		public static int ToInt32(byte[] bytes, int offset = 0)
		{
			BitConverterNonAlloc._c.Byte0 = bytes[offset++];
			BitConverterNonAlloc._c.Byte1 = bytes[offset++];
			BitConverterNonAlloc._c.Byte2 = bytes[offset++];
			BitConverterNonAlloc._c.Byte3 = bytes[offset++];
			return BitConverterNonAlloc._c.Int32;
		}

		// Token: 0x06002C7A RID: 11386 RVA: 0x000EE5DC File Offset: 0x000EC9DC
		public static ulong ToUInt64(byte[] bytes, int offset = 0)
		{
			BitConverterNonAlloc._c.Byte0 = bytes[offset++];
			BitConverterNonAlloc._c.Byte1 = bytes[offset++];
			BitConverterNonAlloc._c.Byte2 = bytes[offset++];
			BitConverterNonAlloc._c.Byte3 = bytes[offset++];
			BitConverterNonAlloc._c.Byte4 = bytes[offset++];
			BitConverterNonAlloc._c.Byte5 = bytes[offset++];
			BitConverterNonAlloc._c.Byte6 = bytes[offset++];
			BitConverterNonAlloc._c.Byte7 = bytes[offset++];
			return BitConverterNonAlloc._c.UInt64;
		}

		// Token: 0x06002C7B RID: 11387 RVA: 0x000EE684 File Offset: 0x000ECA84
		public static long ToInt64(byte[] bytes, int offset = 0)
		{
			BitConverterNonAlloc._c.Byte0 = bytes[offset++];
			BitConverterNonAlloc._c.Byte1 = bytes[offset++];
			BitConverterNonAlloc._c.Byte2 = bytes[offset++];
			BitConverterNonAlloc._c.Byte3 = bytes[offset++];
			BitConverterNonAlloc._c.Byte4 = bytes[offset++];
			BitConverterNonAlloc._c.Byte5 = bytes[offset++];
			BitConverterNonAlloc._c.Byte6 = bytes[offset++];
			BitConverterNonAlloc._c.Byte7 = bytes[offset++];
			return BitConverterNonAlloc._c.Int64;
		}

		// Token: 0x06002C7C RID: 11388 RVA: 0x000EE72C File Offset: 0x000ECB2C
		public static float ToSingle(byte[] bytes, int offset = 0)
		{
			BitConverterNonAlloc._c.Byte0 = bytes[offset++];
			BitConverterNonAlloc._c.Byte1 = bytes[offset++];
			BitConverterNonAlloc._c.Byte2 = bytes[offset++];
			BitConverterNonAlloc._c.Byte3 = bytes[offset++];
			return BitConverterNonAlloc._c.Single;
		}

		// Token: 0x06002C7D RID: 11389 RVA: 0x000EE78C File Offset: 0x000ECB8C
		public static double ToDouble(byte[] bytes, int offset = 0)
		{
			BitConverterNonAlloc._c.Byte0 = bytes[offset++];
			BitConverterNonAlloc._c.Byte1 = bytes[offset++];
			BitConverterNonAlloc._c.Byte2 = bytes[offset++];
			BitConverterNonAlloc._c.Byte3 = bytes[offset++];
			BitConverterNonAlloc._c.Byte4 = bytes[offset++];
			BitConverterNonAlloc._c.Byte5 = bytes[offset++];
			BitConverterNonAlloc._c.Byte6 = bytes[offset++];
			BitConverterNonAlloc._c.Byte7 = bytes[offset++];
			return BitConverterNonAlloc._c.Double;
		}

		// Token: 0x06002C7E RID: 11390 RVA: 0x000EE834 File Offset: 0x000ECC34
		public static ushort ToUInt16(byte[] bytes, ref int offset)
		{
			BitConverterNonAlloc._c.Byte0 = bytes[offset++];
			BitConverterNonAlloc._c.Byte1 = bytes[offset++];
			return BitConverterNonAlloc._c.UInt16;
		}

		// Token: 0x06002C7F RID: 11391 RVA: 0x000EE878 File Offset: 0x000ECC78
		public static short ToInt16(byte[] bytes, ref int offset)
		{
			BitConverterNonAlloc._c.Byte0 = bytes[offset++];
			BitConverterNonAlloc._c.Byte1 = bytes[offset++];
			return BitConverterNonAlloc._c.Int16;
		}

		// Token: 0x06002C80 RID: 11392 RVA: 0x000EE8BC File Offset: 0x000ECCBC
		public static uint ToUInt32(byte[] bytes, ref int offset)
		{
			BitConverterNonAlloc._c.Byte0 = bytes[offset++];
			BitConverterNonAlloc._c.Byte1 = bytes[offset++];
			BitConverterNonAlloc._c.Byte2 = bytes[offset++];
			BitConverterNonAlloc._c.Byte3 = bytes[offset++];
			return BitConverterNonAlloc._c.UInt32;
		}

		// Token: 0x06002C81 RID: 11393 RVA: 0x000EE928 File Offset: 0x000ECD28
		public static int ToInt32(byte[] bytes, ref int offset)
		{
			BitConverterNonAlloc._c.Byte0 = bytes[offset++];
			BitConverterNonAlloc._c.Byte1 = bytes[offset++];
			BitConverterNonAlloc._c.Byte2 = bytes[offset++];
			BitConverterNonAlloc._c.Byte3 = bytes[offset++];
			return BitConverterNonAlloc._c.Int32;
		}

		// Token: 0x06002C82 RID: 11394 RVA: 0x000EE994 File Offset: 0x000ECD94
		public static ulong ToUInt64(byte[] bytes, ref int offset)
		{
			BitConverterNonAlloc._c.Byte0 = bytes[offset++];
			BitConverterNonAlloc._c.Byte1 = bytes[offset++];
			BitConverterNonAlloc._c.Byte2 = bytes[offset++];
			BitConverterNonAlloc._c.Byte3 = bytes[offset++];
			BitConverterNonAlloc._c.Byte4 = bytes[offset++];
			BitConverterNonAlloc._c.Byte5 = bytes[offset++];
			BitConverterNonAlloc._c.Byte6 = bytes[offset++];
			BitConverterNonAlloc._c.Byte7 = bytes[offset++];
			return BitConverterNonAlloc._c.UInt64;
		}

		// Token: 0x06002C83 RID: 11395 RVA: 0x000EEA54 File Offset: 0x000ECE54
		public static long ToInt64(byte[] bytes, ref int offset)
		{
			BitConverterNonAlloc._c.Byte0 = bytes[offset++];
			BitConverterNonAlloc._c.Byte1 = bytes[offset++];
			BitConverterNonAlloc._c.Byte2 = bytes[offset++];
			BitConverterNonAlloc._c.Byte3 = bytes[offset++];
			BitConverterNonAlloc._c.Byte4 = bytes[offset++];
			BitConverterNonAlloc._c.Byte5 = bytes[offset++];
			BitConverterNonAlloc._c.Byte6 = bytes[offset++];
			BitConverterNonAlloc._c.Byte7 = bytes[offset++];
			return BitConverterNonAlloc._c.Int64;
		}

		// Token: 0x06002C84 RID: 11396 RVA: 0x000EEB14 File Offset: 0x000ECF14
		public static float ToSingle(byte[] bytes, ref int offset)
		{
			BitConverterNonAlloc._c.Byte0 = bytes[offset++];
			BitConverterNonAlloc._c.Byte1 = bytes[offset++];
			BitConverterNonAlloc._c.Byte2 = bytes[offset++];
			BitConverterNonAlloc._c.Byte3 = bytes[offset++];
			return BitConverterNonAlloc._c.Single;
		}

		// Token: 0x06002C85 RID: 11397 RVA: 0x000EEB80 File Offset: 0x000ECF80
		public static double ToDouble(byte[] bytes, ref int offset)
		{
			BitConverterNonAlloc._c.Byte0 = bytes[offset++];
			BitConverterNonAlloc._c.Byte1 = bytes[offset++];
			BitConverterNonAlloc._c.Byte2 = bytes[offset++];
			BitConverterNonAlloc._c.Byte3 = bytes[offset++];
			BitConverterNonAlloc._c.Byte4 = bytes[offset++];
			BitConverterNonAlloc._c.Byte5 = bytes[offset++];
			BitConverterNonAlloc._c.Byte6 = bytes[offset++];
			BitConverterNonAlloc._c.Byte7 = bytes[offset++];
			return BitConverterNonAlloc._c.Double;
		}

		// Token: 0x06002C86 RID: 11398 RVA: 0x000EEC3F File Offset: 0x000ED03F
		public static void GetBytes(ushort value, byte[] bytes, int offset = 0)
		{
			BitConverterNonAlloc._c.UInt16 = value;
			bytes[offset++] = BitConverterNonAlloc._c.Byte0;
			bytes[offset++] = BitConverterNonAlloc._c.Byte1;
		}

		// Token: 0x06002C87 RID: 11399 RVA: 0x000EEC70 File Offset: 0x000ED070
		public static void GetBytes(short value, byte[] bytes, int offset = 0)
		{
			BitConverterNonAlloc._c.Int16 = value;
			bytes[offset++] = BitConverterNonAlloc._c.Byte0;
			bytes[offset++] = BitConverterNonAlloc._c.Byte1;
		}

		// Token: 0x06002C88 RID: 11400 RVA: 0x000EECA4 File Offset: 0x000ED0A4
		public static void GetBytes(uint value, byte[] bytes, int offset = 0)
		{
			BitConverterNonAlloc._c.UInt32 = value;
			bytes[offset++] = BitConverterNonAlloc._c.Byte0;
			bytes[offset++] = BitConverterNonAlloc._c.Byte1;
			bytes[offset++] = BitConverterNonAlloc._c.Byte2;
			bytes[offset++] = BitConverterNonAlloc._c.Byte3;
		}

		// Token: 0x06002C89 RID: 11401 RVA: 0x000EED04 File Offset: 0x000ED104
		public static void GetBytes(int value, byte[] bytes, int offset = 0)
		{
			BitConverterNonAlloc._c.Int32 = value;
			bytes[offset++] = BitConverterNonAlloc._c.Byte0;
			bytes[offset++] = BitConverterNonAlloc._c.Byte1;
			bytes[offset++] = BitConverterNonAlloc._c.Byte2;
			bytes[offset++] = BitConverterNonAlloc._c.Byte3;
		}

		// Token: 0x06002C8A RID: 11402 RVA: 0x000EED64 File Offset: 0x000ED164
		public static void GetBytes(ulong value, byte[] bytes, int offset = 0)
		{
			BitConverterNonAlloc._c.UInt64 = value;
			bytes[offset++] = BitConverterNonAlloc._c.Byte0;
			bytes[offset++] = BitConverterNonAlloc._c.Byte1;
			bytes[offset++] = BitConverterNonAlloc._c.Byte2;
			bytes[offset++] = BitConverterNonAlloc._c.Byte3;
			bytes[offset++] = BitConverterNonAlloc._c.Byte4;
			bytes[offset++] = BitConverterNonAlloc._c.Byte5;
			bytes[offset++] = BitConverterNonAlloc._c.Byte6;
			bytes[offset++] = BitConverterNonAlloc._c.Byte7;
		}

		// Token: 0x06002C8B RID: 11403 RVA: 0x000EEE0C File Offset: 0x000ED20C
		public static void GetBytes(long value, byte[] bytes, int offset = 0)
		{
			BitConverterNonAlloc._c.Int64 = value;
			bytes[offset++] = BitConverterNonAlloc._c.Byte0;
			bytes[offset++] = BitConverterNonAlloc._c.Byte1;
			bytes[offset++] = BitConverterNonAlloc._c.Byte2;
			bytes[offset++] = BitConverterNonAlloc._c.Byte3;
			bytes[offset++] = BitConverterNonAlloc._c.Byte4;
			bytes[offset++] = BitConverterNonAlloc._c.Byte5;
			bytes[offset++] = BitConverterNonAlloc._c.Byte6;
			bytes[offset++] = BitConverterNonAlloc._c.Byte7;
		}

		// Token: 0x06002C8C RID: 11404 RVA: 0x000EEEB4 File Offset: 0x000ED2B4
		public static void GetBytes(float value, byte[] bytes, int offset = 0)
		{
			BitConverterNonAlloc._c.Single = value;
			bytes[offset++] = BitConverterNonAlloc._c.Byte0;
			bytes[offset++] = BitConverterNonAlloc._c.Byte1;
			bytes[offset++] = BitConverterNonAlloc._c.Byte2;
			bytes[offset++] = BitConverterNonAlloc._c.Byte3;
		}

		// Token: 0x06002C8D RID: 11405 RVA: 0x000EEF14 File Offset: 0x000ED314
		public static void GetBytes(double value, byte[] bytes, int offset = 0)
		{
			BitConverterNonAlloc._c.Double = value;
			bytes[offset++] = BitConverterNonAlloc._c.Byte0;
			bytes[offset++] = BitConverterNonAlloc._c.Byte1;
			bytes[offset++] = BitConverterNonAlloc._c.Byte2;
			bytes[offset++] = BitConverterNonAlloc._c.Byte3;
			bytes[offset++] = BitConverterNonAlloc._c.Byte4;
			bytes[offset++] = BitConverterNonAlloc._c.Byte5;
			bytes[offset++] = BitConverterNonAlloc._c.Byte6;
			bytes[offset++] = BitConverterNonAlloc._c.Byte7;
		}

		// Token: 0x06002C8E RID: 11406 RVA: 0x000EEFBC File Offset: 0x000ED3BC
		public static void GetBytes(ushort value, byte[] bytes, ref int offset)
		{
			BitConverterNonAlloc._c.UInt16 = value;
			bytes[offset++] = BitConverterNonAlloc._c.Byte0;
			bytes[offset++] = BitConverterNonAlloc._c.Byte1;
		}

		// Token: 0x06002C8F RID: 11407 RVA: 0x000EF000 File Offset: 0x000ED400
		public static void GetBytes(short value, byte[] bytes, ref int offset)
		{
			BitConverterNonAlloc._c.Int16 = value;
			bytes[offset++] = BitConverterNonAlloc._c.Byte0;
			bytes[offset++] = BitConverterNonAlloc._c.Byte1;
		}

		// Token: 0x06002C90 RID: 11408 RVA: 0x000EF044 File Offset: 0x000ED444
		public static void GetBytes(uint value, byte[] bytes, ref int offset)
		{
			BitConverterNonAlloc._c.UInt32 = value;
			bytes[offset++] = BitConverterNonAlloc._c.Byte0;
			bytes[offset++] = BitConverterNonAlloc._c.Byte1;
			bytes[offset++] = BitConverterNonAlloc._c.Byte2;
			bytes[offset++] = BitConverterNonAlloc._c.Byte3;
		}

		// Token: 0x06002C91 RID: 11409 RVA: 0x000EF0B0 File Offset: 0x000ED4B0
		public static void GetBytes(int value, byte[] bytes, ref int offset)
		{
			BitConverterNonAlloc._c.Int32 = value;
			bytes[offset++] = BitConverterNonAlloc._c.Byte0;
			bytes[offset++] = BitConverterNonAlloc._c.Byte1;
			bytes[offset++] = BitConverterNonAlloc._c.Byte2;
			bytes[offset++] = BitConverterNonAlloc._c.Byte3;
		}

		// Token: 0x06002C92 RID: 11410 RVA: 0x000EF11C File Offset: 0x000ED51C
		public static void GetBytes(ulong value, byte[] bytes, ref int offset)
		{
			BitConverterNonAlloc._c.UInt64 = value;
			bytes[offset++] = BitConverterNonAlloc._c.Byte0;
			bytes[offset++] = BitConverterNonAlloc._c.Byte1;
			bytes[offset++] = BitConverterNonAlloc._c.Byte2;
			bytes[offset++] = BitConverterNonAlloc._c.Byte3;
			bytes[offset++] = BitConverterNonAlloc._c.Byte4;
			bytes[offset++] = BitConverterNonAlloc._c.Byte5;
			bytes[offset++] = BitConverterNonAlloc._c.Byte6;
			bytes[offset++] = BitConverterNonAlloc._c.Byte7;
		}

		// Token: 0x06002C93 RID: 11411 RVA: 0x000EF1DC File Offset: 0x000ED5DC
		public static void GetBytes(long value, byte[] bytes, ref int offset)
		{
			BitConverterNonAlloc._c.Int64 = value;
			bytes[offset++] = BitConverterNonAlloc._c.Byte0;
			bytes[offset++] = BitConverterNonAlloc._c.Byte1;
			bytes[offset++] = BitConverterNonAlloc._c.Byte2;
			bytes[offset++] = BitConverterNonAlloc._c.Byte3;
			bytes[offset++] = BitConverterNonAlloc._c.Byte4;
			bytes[offset++] = BitConverterNonAlloc._c.Byte5;
			bytes[offset++] = BitConverterNonAlloc._c.Byte6;
			bytes[offset++] = BitConverterNonAlloc._c.Byte7;
		}

		// Token: 0x06002C94 RID: 11412 RVA: 0x000EF29C File Offset: 0x000ED69C
		public static void GetBytes(float value, byte[] bytes, ref int offset)
		{
			BitConverterNonAlloc._c.Single = value;
			bytes[offset++] = BitConverterNonAlloc._c.Byte0;
			bytes[offset++] = BitConverterNonAlloc._c.Byte1;
			bytes[offset++] = BitConverterNonAlloc._c.Byte2;
			bytes[offset++] = BitConverterNonAlloc._c.Byte3;
		}

		// Token: 0x06002C95 RID: 11413 RVA: 0x000EF308 File Offset: 0x000ED708
		public static void GetBytes(double value, byte[] bytes, ref int offset)
		{
			BitConverterNonAlloc._c.Double = value;
			bytes[offset++] = BitConverterNonAlloc._c.Byte0;
			bytes[offset++] = BitConverterNonAlloc._c.Byte1;
			bytes[offset++] = BitConverterNonAlloc._c.Byte2;
			bytes[offset++] = BitConverterNonAlloc._c.Byte3;
			bytes[offset++] = BitConverterNonAlloc._c.Byte4;
			bytes[offset++] = BitConverterNonAlloc._c.Byte5;
			bytes[offset++] = BitConverterNonAlloc._c.Byte6;
			bytes[offset++] = BitConverterNonAlloc._c.Byte7;
		}

		// Token: 0x04002377 RID: 9079
		[ThreadStatic]
		private static BitConverterNonAlloc.ConversionStruct _c;

		// Token: 0x02000722 RID: 1826
		[StructLayout(LayoutKind.Explicit)]
		private struct ConversionStruct
		{
			// Token: 0x04002378 RID: 9080
			[FieldOffset(0)]
			public byte Byte0;

			// Token: 0x04002379 RID: 9081
			[FieldOffset(1)]
			public byte Byte1;

			// Token: 0x0400237A RID: 9082
			[FieldOffset(2)]
			public byte Byte2;

			// Token: 0x0400237B RID: 9083
			[FieldOffset(3)]
			public byte Byte3;

			// Token: 0x0400237C RID: 9084
			[FieldOffset(4)]
			public byte Byte4;

			// Token: 0x0400237D RID: 9085
			[FieldOffset(5)]
			public byte Byte5;

			// Token: 0x0400237E RID: 9086
			[FieldOffset(6)]
			public byte Byte6;

			// Token: 0x0400237F RID: 9087
			[FieldOffset(7)]
			public byte Byte7;

			// Token: 0x04002380 RID: 9088
			[FieldOffset(0)]
			public ushort UInt16;

			// Token: 0x04002381 RID: 9089
			[FieldOffset(0)]
			public short Int16;

			// Token: 0x04002382 RID: 9090
			[FieldOffset(0)]
			public uint UInt32;

			// Token: 0x04002383 RID: 9091
			[FieldOffset(0)]
			public int Int32;

			// Token: 0x04002384 RID: 9092
			[FieldOffset(0)]
			public ulong UInt64;

			// Token: 0x04002385 RID: 9093
			[FieldOffset(0)]
			public long Int64;

			// Token: 0x04002386 RID: 9094
			[FieldOffset(0)]
			public float Single;

			// Token: 0x04002387 RID: 9095
			[FieldOffset(0)]
			public double Double;
		}
	}
}
