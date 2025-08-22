using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using ProtoBuf;
using UnityEngine;
using UnityEngine.Events;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x02000212 RID: 530
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentUnityEventBase
	{
		// Token: 0x06000AA8 RID: 2728 RVA: 0x00041ED0 File Offset: 0x000402D0
		static PersistentUnityEventBase()
		{
			if (PersistentUnityEventBase.m_persistentCallGroupInfo == null)
			{
				throw new NotSupportedException("m_PersistentCalls FieldInfo not found.");
			}
			Type fieldType = PersistentUnityEventBase.m_persistentCallGroupInfo.FieldType;
			PersistentUnityEventBase.m_callsInfo = fieldType.GetField("m_Calls", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (PersistentUnityEventBase.m_callsInfo == null)
			{
				throw new NotSupportedException("m_Calls FieldInfo not found. ");
			}
			Type fieldType2 = PersistentUnityEventBase.m_callsInfo.FieldType;
			if (!fieldType2.IsGenericType() || fieldType2.GetGenericTypeDefinition() != typeof(List<>))
			{
				throw new NotSupportedException("m_callsInfo.FieldType is not a generic List<>");
			}
			PersistentUnityEventBase.m_callType = fieldType2.GetGenericArguments()[0];
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x00041F82 File Offset: 0x00040382
		public PersistentUnityEventBase()
		{
		}

		// Token: 0x06000AAA RID: 2730 RVA: 0x00041F8C File Offset: 0x0004038C
		public void ReadFrom(UnityEventBase obj)
		{
			if (obj == null)
			{
				return;
			}
			object value = PersistentUnityEventBase.m_persistentCallGroupInfo.GetValue(obj);
			if (value == null)
			{
				return;
			}
			object value2 = PersistentUnityEventBase.m_callsInfo.GetValue(value);
			if (value2 == null)
			{
				return;
			}
			IList list = (IList)value2;
			this.m_calls = new PersistentPersistentCall[list.Count];
			for (int i = 0; i < list.Count; i++)
			{
				object obj2 = list[i];
				PersistentPersistentCall persistentPersistentCall = new PersistentPersistentCall();
				persistentPersistentCall.ReadFrom(obj2);
				this.m_calls[i] = persistentPersistentCall;
			}
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x00042018 File Offset: 0x00040418
		public void GetDependencies(UnityEventBase obj, Dictionary<long, UnityEngine.Object> dependencies)
		{
			if (obj == null)
			{
				return;
			}
			object value = PersistentUnityEventBase.m_persistentCallGroupInfo.GetValue(obj);
			if (value == null)
			{
				return;
			}
			object value2 = PersistentUnityEventBase.m_callsInfo.GetValue(value);
			if (value2 == null)
			{
				return;
			}
			IList list = (IList)value2;
			for (int i = 0; i < list.Count; i++)
			{
				object obj2 = list[i];
				PersistentPersistentCall persistentPersistentCall = new PersistentPersistentCall();
				persistentPersistentCall.GetDependencies(obj2, dependencies);
			}
		}

		// Token: 0x06000AAC RID: 2732 RVA: 0x0004208C File Offset: 0x0004048C
		public void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			if (this.m_calls == null)
			{
				return;
			}
			for (int i = 0; i < this.m_calls.Length; i++)
			{
				PersistentPersistentCall persistentPersistentCall = this.m_calls[i];
				if (persistentPersistentCall != null)
				{
					persistentPersistentCall.FindDependencies<T>(dependencies, objects, allowNulls);
				}
			}
		}

		// Token: 0x06000AAD RID: 2733 RVA: 0x000420D8 File Offset: 0x000404D8
		public void WriteTo(UnityEventBase obj, Dictionary<long, UnityEngine.Object> objects)
		{
			if (obj == null)
			{
				return;
			}
			if (this.m_calls == null)
			{
				return;
			}
			object obj2 = Activator.CreateInstance(PersistentUnityEventBase.m_persistentCallGroupInfo.FieldType);
			object obj3 = Activator.CreateInstance(PersistentUnityEventBase.m_callsInfo.FieldType);
			IList list = (IList)obj3;
			for (int i = 0; i < this.m_calls.Length; i++)
			{
				PersistentPersistentCall persistentPersistentCall = this.m_calls[i];
				if (persistentPersistentCall != null)
				{
					object obj4 = Activator.CreateInstance(PersistentUnityEventBase.m_callType);
					persistentPersistentCall.WriteTo(obj4, objects);
					list.Add(obj4);
				}
				else
				{
					list.Add(null);
				}
			}
			PersistentUnityEventBase.m_callsInfo.SetValue(obj2, obj3);
			PersistentUnityEventBase.m_persistentCallGroupInfo.SetValue(obj, obj2);
		}

		// Token: 0x04000BF2 RID: 3058
		private static FieldInfo m_persistentCallGroupInfo = typeof(UnityEventBase).GetField("m_PersistentCalls", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04000BF3 RID: 3059
		private static FieldInfo m_callsInfo;

		// Token: 0x04000BF4 RID: 3060
		private static Type m_callType;

		// Token: 0x04000BF5 RID: 3061
		public PersistentPersistentCall[] m_calls;
	}
}
