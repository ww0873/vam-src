using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace GPUTools.Common.Scripts.Tools.Debug
{
	// Token: 0x020009C9 RID: 2505
	public class Validator
	{
		// Token: 0x06003F39 RID: 16185 RVA: 0x0012EBD8 File Offset: 0x0012CFD8
		public Validator()
		{
		}

		// Token: 0x06003F3A RID: 16186 RVA: 0x0012EBE0 File Offset: 0x0012CFE0
		public static bool TestList<T>(List<T> list)
		{
			return list.Count != 0 && !list.Any(new Func<T, bool>(Validator.<TestList`1>m__0<T>));
		}

		// Token: 0x06003F3B RID: 16187 RVA: 0x0012EC09 File Offset: 0x0012D009
		[CompilerGenerated]
		private static bool <TestList<T>(T item)
		{
			return item == null;
		}
	}
}
