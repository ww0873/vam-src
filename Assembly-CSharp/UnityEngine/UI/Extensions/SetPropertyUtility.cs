using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000555 RID: 1365
	internal static class SetPropertyUtility
	{
		// Token: 0x060022C1 RID: 8897 RVA: 0x000C62E0 File Offset: 0x000C46E0
		public static bool SetColor(ref Color currentValue, Color newValue)
		{
			if (currentValue.r == newValue.r && currentValue.g == newValue.g && currentValue.b == newValue.b && currentValue.a == newValue.a)
			{
				return false;
			}
			currentValue = newValue;
			return true;
		}

		// Token: 0x060022C2 RID: 8898 RVA: 0x000C633F File Offset: 0x000C473F
		public static bool SetStruct<T>(ref T currentValue, T newValue) where T : struct
		{
			if (currentValue.Equals(newValue))
			{
				return false;
			}
			currentValue = newValue;
			return true;
		}

		// Token: 0x060022C3 RID: 8899 RVA: 0x000C6364 File Offset: 0x000C4764
		public static bool SetClass<T>(ref T currentValue, T newValue) where T : class
		{
			if ((currentValue == null && newValue == null) || (currentValue != null && currentValue.Equals(newValue)))
			{
				return false;
			}
			currentValue = newValue;
			return true;
		}
	}
}
