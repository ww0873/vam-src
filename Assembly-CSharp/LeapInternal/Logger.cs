using System;
using System.Reflection;
using UnityEngine;

namespace LeapInternal
{
	// Token: 0x0200061F RID: 1567
	public static class Logger
	{
		// Token: 0x0600267E RID: 9854 RVA: 0x000D8455 File Offset: 0x000D6855
		public static void Log(object message)
		{
			Debug.Log(message);
		}

		// Token: 0x0600267F RID: 9855 RVA: 0x000D8460 File Offset: 0x000D6860
		public static void LogStruct(object thisObject, string title = "")
		{
			try
			{
				if (!thisObject.GetType().IsValueType)
				{
					Logger.Log(title + " ---- Trying to log non-struct with struct logger");
				}
				else
				{
					Logger.Log(title + " ---- " + thisObject.GetType().ToString());
					FieldInfo[] fields = thisObject.GetType().GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
					foreach (FieldInfo fieldInfo in fields)
					{
						object value = fieldInfo.GetValue(thisObject);
						string str = (value != null) ? value.ToString() : "null";
						Logger.Log(" -------- Name: " + fieldInfo.Name + ", Value = " + str);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex.Message);
			}
		}
	}
}
