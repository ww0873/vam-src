using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace LeapInternal
{
	// Token: 0x02000628 RID: 1576
	public static class StructMarshal<T> where T : struct
	{
		// Token: 0x060026AD RID: 9901 RVA: 0x000D90F4 File Offset: 0x000D74F4
		static StructMarshal()
		{
		}

		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x060026AE RID: 9902 RVA: 0x000D910A File Offset: 0x000D750A
		public static int Size
		{
			get
			{
				return StructMarshal<T>._sizeofT;
			}
		}

		// Token: 0x060026AF RID: 9903 RVA: 0x000D9114 File Offset: 0x000D7514
		public static void PtrToStruct(IntPtr ptr, out T t)
		{
			if (StructMarshal<T>._container == null)
			{
				StructMarshal<T>._container = new StructMarshal<T>.StructContainer();
			}
			try
			{
				Marshal.PtrToStructure(ptr, StructMarshal<T>._container);
				t = StructMarshal<T>._container.value;
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				t = default(T);
			}
		}

		// Token: 0x060026B0 RID: 9904 RVA: 0x000D9180 File Offset: 0x000D7580
		public static void ArrayElementToStruct(IntPtr ptr, int arrayIndex, out T t)
		{
			StructMarshal<T>.PtrToStruct(new IntPtr(ptr.ToInt64() + (long)(StructMarshal<T>._sizeofT * arrayIndex)), out t);
		}

		// Token: 0x040020D5 RID: 8405
		[ThreadStatic]
		private static StructMarshal<T>.StructContainer _container;

		// Token: 0x040020D6 RID: 8406
		private static int _sizeofT = Marshal.SizeOf(typeof(T));

		// Token: 0x02000629 RID: 1577
		[StructLayout(LayoutKind.Sequential)]
		private class StructContainer
		{
			// Token: 0x060026B1 RID: 9905 RVA: 0x000D919D File Offset: 0x000D759D
			public StructContainer()
			{
			}

			// Token: 0x040020D7 RID: 8407
			public T value;
		}
	}
}
