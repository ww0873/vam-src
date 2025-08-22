using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

// Token: 0x02000720 RID: 1824
public static class AssertHelper
{
	// Token: 0x06002C6F RID: 11375 RVA: 0x000EE3D5 File Offset: 0x000EC7D5
	[Conditional("UNITY_EDITOR")]
	public static void AssertRuntimeOnly(string message = null)
	{
		message = (message ?? "Assert failed because game was not in Play Mode.");
	}

	// Token: 0x06002C70 RID: 11376 RVA: 0x000EE3E6 File Offset: 0x000EC7E6
	[Conditional("UNITY_EDITOR")]
	public static void AssertEditorOnly(string message = null)
	{
		message = (message ?? "Assert failed because game was in Play Mode.");
	}

	// Token: 0x06002C71 RID: 11377 RVA: 0x000EE3F7 File Offset: 0x000EC7F7
	[Conditional("UNITY_ASSERTIONS")]
	public static void Implies(bool condition, bool result, string message = "")
	{
		if (condition)
		{
		}
	}

	// Token: 0x06002C72 RID: 11378 RVA: 0x000EE3FF File Offset: 0x000EC7FF
	[Conditional("UNITY_ASSERTIONS")]
	public static void Implies(bool condition, Func<bool> result, string message = "")
	{
		if (condition)
		{
		}
	}

	// Token: 0x06002C73 RID: 11379 RVA: 0x000EE407 File Offset: 0x000EC807
	[Conditional("UNITY_ASSERTIONS")]
	public static void Implies(string conditionName, bool condition, string resultName, bool result)
	{
	}

	// Token: 0x06002C74 RID: 11380 RVA: 0x000EE409 File Offset: 0x000EC809
	[Conditional("UNITY_ASSERTIONS")]
	public static void Implies(string conditionName, bool condition, string resultName, Func<bool> result)
	{
		if (condition)
		{
		}
	}

	// Token: 0x06002C75 RID: 11381 RVA: 0x000EE414 File Offset: 0x000EC814
	[Conditional("UNITY_ASSERTIONS")]
	public static void Contains<T>(T value, IEnumerable<T> collection, string message = "")
	{
		if (!collection.Contains(value))
		{
			string str = "The value " + value + " was not found in the collection [";
			bool flag = true;
			foreach (T t in collection)
			{
				if (!flag)
				{
					str += ", ";
					flag = false;
				}
				str += t.ToString();
			}
			str = str + "]\n" + message;
		}
	}
}
