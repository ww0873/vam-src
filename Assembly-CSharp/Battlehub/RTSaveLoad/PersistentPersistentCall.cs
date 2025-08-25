using System;
using System.Collections.Generic;
using System.Reflection;
using ProtoBuf;
using UnityEngine;
using UnityEngine.Events;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x02000211 RID: 529
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentPersistentCall
	{
		// Token: 0x06000AA0 RID: 2720 RVA: 0x00041B7A File Offset: 0x0003FF7A
		public PersistentPersistentCall()
		{
		}

		// Token: 0x06000AA1 RID: 2721 RVA: 0x00041B84 File Offset: 0x0003FF84
		private static void Initialize(Type type)
		{
			if (PersistentPersistentCall.m_isFieldInfoInitialized)
			{
				return;
			}
			PersistentPersistentCall.m_argumentsFieldInfo = type.GetField("m_Arguments", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (PersistentPersistentCall.m_argumentsFieldInfo == null)
			{
				throw new NotSupportedException("m_Arguments FieldInfo not found.");
			}
			PersistentPersistentCall.m_callStatFieldInfo = type.GetField("m_CallState", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (PersistentPersistentCall.m_callStatFieldInfo == null)
			{
				throw new NotSupportedException("m_CallState FieldInfo not found.");
			}
			PersistentPersistentCall.m_methodNameFieldInfo = type.GetField("m_MethodName", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (PersistentPersistentCall.m_methodNameFieldInfo == null)
			{
				throw new NotSupportedException("m_MethodName FieldInfo not found.");
			}
			PersistentPersistentCall.m_modeFieldInfo = type.GetField("m_Mode", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (PersistentPersistentCall.m_modeFieldInfo == null)
			{
				throw new NotSupportedException("m_Mode FieldInfo not found.");
			}
			PersistentPersistentCall.m_targetFieldInfo = type.GetField("m_Target", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (PersistentPersistentCall.m_targetFieldInfo == null)
			{
				throw new NotSupportedException("m_Target FieldInfo not found.");
			}
			PersistentPersistentCall.m_isFieldInfoInitialized = true;
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x00041C68 File Offset: 0x00040068
		public void ReadFrom(object obj)
		{
			if (obj == null)
			{
				this.m_Arguments = null;
				this.m_CallState = UnityEventCallState.Off;
				this.m_MethodName = null;
				this.m_Mode = PersistentListenerMode.EventDefined;
				this.m_Target = 0L;
				return;
			}
			PersistentPersistentCall.Initialize(obj.GetType());
			this.m_Arguments = new PersistentArgumentCache();
			this.m_Arguments.ReadFrom(PersistentPersistentCall.m_argumentsFieldInfo.GetValue(obj));
			this.m_CallState = (UnityEventCallState)PersistentPersistentCall.m_callStatFieldInfo.GetValue(obj);
			this.m_MethodName = (string)PersistentPersistentCall.m_methodNameFieldInfo.GetValue(obj);
			this.m_Mode = (PersistentListenerMode)PersistentPersistentCall.m_modeFieldInfo.GetValue(obj);
			UnityEngine.Object obj2 = (UnityEngine.Object)PersistentPersistentCall.m_targetFieldInfo.GetValue(obj);
			this.m_Target = obj2.GetMappedInstanceID();
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x00041D2C File Offset: 0x0004012C
		public void GetDependencies(object obj, Dictionary<long, UnityEngine.Object> dependencies)
		{
			if (obj == null)
			{
				return;
			}
			PersistentPersistentCall.Initialize(obj.GetType());
			PersistentArgumentCache persistentArgumentCache = new PersistentArgumentCache();
			persistentArgumentCache.GetDependencies(PersistentPersistentCall.m_argumentsFieldInfo.GetValue(obj), dependencies);
			UnityEngine.Object obj2 = (UnityEngine.Object)PersistentPersistentCall.m_targetFieldInfo.GetValue(obj);
			this.AddDependency(obj2, dependencies);
		}

		// Token: 0x06000AA4 RID: 2724 RVA: 0x00041D7C File Offset: 0x0004017C
		protected void AddDependency(UnityEngine.Object obj, Dictionary<long, UnityEngine.Object> dependencies)
		{
			if (obj == null)
			{
				return;
			}
			long mappedInstanceID = obj.GetMappedInstanceID();
			if (!dependencies.ContainsKey(mappedInstanceID))
			{
				dependencies.Add(mappedInstanceID, obj);
			}
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x00041DB1 File Offset: 0x000401B1
		public void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			if (this.m_Arguments != null)
			{
				this.m_Arguments.FindDependencies<T>(dependencies, objects, allowNulls);
			}
			this.AddDependency<T>(this.m_Target, dependencies, objects, allowNulls);
		}

		// Token: 0x06000AA6 RID: 2726 RVA: 0x00041DDC File Offset: 0x000401DC
		protected void AddDependency<T>(long id, Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			T t = objects.Get(id);
			if ((t != null || allowNulls) && !dependencies.ContainsKey(id))
			{
				dependencies.Add(id, t);
			}
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x00041E18 File Offset: 0x00040218
		public void WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			if (obj == null)
			{
				return;
			}
			PersistentPersistentCall.Initialize(obj.GetType());
			this.TypeName = obj.GetType().AssemblyQualifiedName;
			if (this.m_Arguments != null)
			{
				object obj2 = Activator.CreateInstance(PersistentPersistentCall.m_argumentsFieldInfo.FieldType);
				this.m_Arguments.WriteTo(obj2, objects);
				PersistentPersistentCall.m_argumentsFieldInfo.SetValue(obj, obj2);
			}
			PersistentPersistentCall.m_callStatFieldInfo.SetValue(obj, this.m_CallState);
			PersistentPersistentCall.m_methodNameFieldInfo.SetValue(obj, this.m_MethodName);
			PersistentPersistentCall.m_modeFieldInfo.SetValue(obj, this.m_Mode);
			PersistentPersistentCall.m_targetFieldInfo.SetValue(obj, objects.Get(this.m_Target));
		}

		// Token: 0x04000BE6 RID: 3046
		public PersistentArgumentCache m_Arguments;

		// Token: 0x04000BE7 RID: 3047
		public UnityEventCallState m_CallState;

		// Token: 0x04000BE8 RID: 3048
		public string m_MethodName;

		// Token: 0x04000BE9 RID: 3049
		public PersistentListenerMode m_Mode;

		// Token: 0x04000BEA RID: 3050
		public long m_Target;

		// Token: 0x04000BEB RID: 3051
		public string TypeName;

		// Token: 0x04000BEC RID: 3052
		private static bool m_isFieldInfoInitialized;

		// Token: 0x04000BED RID: 3053
		private static FieldInfo m_argumentsFieldInfo;

		// Token: 0x04000BEE RID: 3054
		private static FieldInfo m_callStatFieldInfo;

		// Token: 0x04000BEF RID: 3055
		private static FieldInfo m_methodNameFieldInfo;

		// Token: 0x04000BF0 RID: 3056
		private static FieldInfo m_modeFieldInfo;

		// Token: 0x04000BF1 RID: 3057
		private static FieldInfo m_targetFieldInfo;
	}
}
