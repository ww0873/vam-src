using System;
using System.Collections.Generic;
using System.Reflection;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x02000210 RID: 528
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentArgumentCache
	{
		// Token: 0x06000A98 RID: 2712 RVA: 0x00041836 File Offset: 0x0003FC36
		public PersistentArgumentCache()
		{
		}

		// Token: 0x06000A99 RID: 2713 RVA: 0x00041840 File Offset: 0x0003FC40
		private static void Initialize(Type type)
		{
			if (PersistentArgumentCache.m_isFieldInfoInitialized)
			{
				return;
			}
			PersistentArgumentCache.m_boolArgumentFieldInfo = type.GetField("m_BoolArgument", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (PersistentArgumentCache.m_boolArgumentFieldInfo == null)
			{
				throw new NotSupportedException("m_BoolArgument FieldInfo not found.");
			}
			PersistentArgumentCache.m_floatArgumentFieldInfo = type.GetField("m_FloatArgument", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (PersistentArgumentCache.m_floatArgumentFieldInfo == null)
			{
				throw new NotSupportedException("m_FloatArgument FieldInfo not found.");
			}
			PersistentArgumentCache.m_intArgumentFieldInfo = type.GetField("m_IntArgument", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (PersistentArgumentCache.m_intArgumentFieldInfo == null)
			{
				throw new NotSupportedException("m_IntArgument FieldInfo not found.");
			}
			PersistentArgumentCache.m_stringArgumentFieldInfo = type.GetField("m_StringArgument", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (PersistentArgumentCache.m_stringArgumentFieldInfo == null)
			{
				throw new NotSupportedException("m_StringArgument FieldInfo not found.");
			}
			PersistentArgumentCache.m_objectArgumentFieldInfo = type.GetField("m_ObjectArgument", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (PersistentArgumentCache.m_objectArgumentFieldInfo == null)
			{
				throw new NotSupportedException("m_ObjectArgument FieldInfo not found.");
			}
			PersistentArgumentCache.m_objectArgumentAssemblyTypeNameFieldInfo = type.GetField("m_ObjectArgumentAssemblyTypeName", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (PersistentArgumentCache.m_objectArgumentAssemblyTypeNameFieldInfo == null)
			{
				throw new NotSupportedException("m_ObjectArgumentAssemblyTypeName FieldInfo not found.");
			}
			PersistentArgumentCache.m_isFieldInfoInitialized = true;
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x00041948 File Offset: 0x0003FD48
		public void ReadFrom(object obj)
		{
			if (obj == null)
			{
				this.m_BoolArgument = false;
				this.m_FloatArgument = 0f;
				this.m_IntArgument = 0;
				this.m_StringArgument = null;
				this.m_ObjectArgument = 0L;
				this.m_ObjectArgumentAssemblyTypeName = null;
				return;
			}
			PersistentArgumentCache.Initialize(obj.GetType());
			this.m_BoolArgument = (bool)PersistentArgumentCache.m_boolArgumentFieldInfo.GetValue(obj);
			this.m_FloatArgument = (float)PersistentArgumentCache.m_floatArgumentFieldInfo.GetValue(obj);
			this.m_IntArgument = (int)PersistentArgumentCache.m_intArgumentFieldInfo.GetValue(obj);
			this.m_StringArgument = (string)PersistentArgumentCache.m_stringArgumentFieldInfo.GetValue(obj);
			UnityEngine.Object obj2 = (UnityEngine.Object)PersistentArgumentCache.m_objectArgumentFieldInfo.GetValue(obj);
			this.m_ObjectArgument = obj2.GetMappedInstanceID();
			this.m_ObjectArgumentAssemblyTypeName = (string)PersistentArgumentCache.m_objectArgumentAssemblyTypeNameFieldInfo.GetValue(obj);
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x00041A24 File Offset: 0x0003FE24
		public void GetDependencies(object obj, Dictionary<long, UnityEngine.Object> dependencies)
		{
			if (obj == null)
			{
				return;
			}
			PersistentArgumentCache.Initialize(obj.GetType());
			UnityEngine.Object obj2 = (UnityEngine.Object)PersistentArgumentCache.m_objectArgumentFieldInfo.GetValue(obj);
			this.AddDependency(obj2, dependencies);
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x00041A5C File Offset: 0x0003FE5C
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

		// Token: 0x06000A9D RID: 2717 RVA: 0x00041A91 File Offset: 0x0003FE91
		public void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			this.AddDependency<T>(this.m_ObjectArgument, dependencies, objects, allowNulls);
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x00041AA4 File Offset: 0x0003FEA4
		protected void AddDependency<T>(long id, Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			T t = objects.Get(id);
			if ((t != null || allowNulls) && !dependencies.ContainsKey(id))
			{
				dependencies.Add(id, t);
			}
		}

		// Token: 0x06000A9F RID: 2719 RVA: 0x00041AE0 File Offset: 0x0003FEE0
		public void WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			if (obj == null)
			{
				return;
			}
			PersistentArgumentCache.Initialize(obj.GetType());
			PersistentArgumentCache.m_boolArgumentFieldInfo.SetValue(obj, this.m_BoolArgument);
			PersistentArgumentCache.m_floatArgumentFieldInfo.SetValue(obj, this.m_FloatArgument);
			PersistentArgumentCache.m_intArgumentFieldInfo.SetValue(obj, this.m_IntArgument);
			PersistentArgumentCache.m_stringArgumentFieldInfo.SetValue(obj, this.m_StringArgument);
			PersistentArgumentCache.m_objectArgumentFieldInfo.SetValue(obj, objects.Get(this.m_ObjectArgument));
			PersistentArgumentCache.m_objectArgumentAssemblyTypeNameFieldInfo.SetValue(obj, this.m_ObjectArgumentAssemblyTypeName);
		}

		// Token: 0x04000BD9 RID: 3033
		public bool m_BoolArgument;

		// Token: 0x04000BDA RID: 3034
		public float m_FloatArgument;

		// Token: 0x04000BDB RID: 3035
		public int m_IntArgument;

		// Token: 0x04000BDC RID: 3036
		public string m_StringArgument;

		// Token: 0x04000BDD RID: 3037
		public long m_ObjectArgument;

		// Token: 0x04000BDE RID: 3038
		public string m_ObjectArgumentAssemblyTypeName;

		// Token: 0x04000BDF RID: 3039
		private static bool m_isFieldInfoInitialized;

		// Token: 0x04000BE0 RID: 3040
		private static FieldInfo m_boolArgumentFieldInfo;

		// Token: 0x04000BE1 RID: 3041
		private static FieldInfo m_floatArgumentFieldInfo;

		// Token: 0x04000BE2 RID: 3042
		private static FieldInfo m_intArgumentFieldInfo;

		// Token: 0x04000BE3 RID: 3043
		private static FieldInfo m_stringArgumentFieldInfo;

		// Token: 0x04000BE4 RID: 3044
		private static FieldInfo m_objectArgumentFieldInfo;

		// Token: 0x04000BE5 RID: 3045
		private static FieldInfo m_objectArgumentAssemblyTypeNameFieldInfo;
	}
}
