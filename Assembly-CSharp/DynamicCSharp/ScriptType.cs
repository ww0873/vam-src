using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using UnityEngine;

namespace DynamicCSharp
{
	// Token: 0x020002E1 RID: 737
	public sealed class ScriptType
	{
		// Token: 0x06001158 RID: 4440 RVA: 0x00060AE8 File Offset: 0x0005EEE8
		public ScriptType(Type type)
		{
			this.assembly = null;
			this.rawType = type;
		}

		// Token: 0x06001159 RID: 4441 RVA: 0x00060B1F File Offset: 0x0005EF1F
		internal ScriptType(ScriptAssembly assembly, Type type)
		{
			this.assembly = assembly;
			this.rawType = type;
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x0600115A RID: 4442 RVA: 0x00060B56 File Offset: 0x0005EF56
		public Type RawType
		{
			get
			{
				return this.rawType;
			}
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x0600115B RID: 4443 RVA: 0x00060B5E File Offset: 0x0005EF5E
		public ScriptAssembly Assembly
		{
			get
			{
				return this.assembly;
			}
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x0600115C RID: 4444 RVA: 0x00060B66 File Offset: 0x0005EF66
		public bool IsUnityObject
		{
			get
			{
				return this.IsSubtypeOf<UnityEngine.Object>();
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x0600115D RID: 4445 RVA: 0x00060B6E File Offset: 0x0005EF6E
		public bool IsMonoBehaviour
		{
			get
			{
				return this.IsSubtypeOf<MonoBehaviour>();
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x0600115E RID: 4446 RVA: 0x00060B76 File Offset: 0x0005EF76
		public bool IsScriptableObject
		{
			get
			{
				return this.IsSubtypeOf<ScriptableObject>();
			}
		}

		// Token: 0x0600115F RID: 4447 RVA: 0x00060B7E File Offset: 0x0005EF7E
		public ScriptProxy CreateInstance(GameObject parent = null)
		{
			if (this.IsMonoBehaviour)
			{
				return this.CreateBehaviourInstance(parent);
			}
			if (this.IsScriptableObject)
			{
				return this.CreateScriptableInstance();
			}
			return this.CreateCSharpInstance(new object[0]);
		}

		// Token: 0x06001160 RID: 4448 RVA: 0x00060BB1 File Offset: 0x0005EFB1
		public ScriptProxy CreateInstance(GameObject parent = null, params object[] parameters)
		{
			if (this.IsMonoBehaviour)
			{
				return this.CreateBehaviourInstance(parent);
			}
			if (this.IsScriptableObject)
			{
				return this.CreateScriptableInstance();
			}
			return this.CreateCSharpInstance(parameters);
		}

		// Token: 0x06001161 RID: 4449 RVA: 0x00060BE0 File Offset: 0x0005EFE0
		public object CreateRawInstance(GameObject parent = null)
		{
			ScriptProxy scriptProxy = this.CreateInstance(parent);
			if (scriptProxy == null)
			{
				return null;
			}
			return scriptProxy.Instance;
		}

		// Token: 0x06001162 RID: 4450 RVA: 0x00060C04 File Offset: 0x0005F004
		public object CreateRawInstance(GameObject parent = null, params object[] parameters)
		{
			ScriptProxy scriptProxy = this.CreateInstance(parent, parameters);
			if (scriptProxy == null)
			{
				return null;
			}
			return scriptProxy.Instance;
		}

		// Token: 0x06001163 RID: 4451 RVA: 0x00060C28 File Offset: 0x0005F028
		public T CreateRawInstance<T>(GameObject parent = null) where T : class
		{
			ScriptProxy scriptProxy = this.CreateInstance(parent);
			if (scriptProxy == null)
			{
				return (T)((object)null);
			}
			return scriptProxy.GetInstanceAs<T>(false);
		}

		// Token: 0x06001164 RID: 4452 RVA: 0x00060C54 File Offset: 0x0005F054
		public T CreateRawInstance<T>(GameObject parent = null, params object[] parameters) where T : class
		{
			ScriptProxy scriptProxy = this.CreateInstance(parent);
			if (scriptProxy == null)
			{
				return (T)((object)null);
			}
			return scriptProxy.GetInstanceAs<T>(false);
		}

		// Token: 0x06001165 RID: 4453 RVA: 0x00060C80 File Offset: 0x0005F080
		private ScriptProxy CreateBehaviourInstance(GameObject parent)
		{
			if (parent == null)
			{
				throw new ArgumentNullException("parent");
			}
			MonoBehaviour monoBehaviour = parent.AddComponent(this.rawType) as MonoBehaviour;
			if (monoBehaviour != null)
			{
				return new ScriptProxy(this, monoBehaviour);
			}
			return null;
		}

		// Token: 0x06001166 RID: 4454 RVA: 0x00060CCC File Offset: 0x0005F0CC
		private ScriptProxy CreateScriptableInstance()
		{
			ScriptableObject scriptableObject = ScriptableObject.CreateInstance(this.rawType);
			if (scriptableObject != null)
			{
				return new ScriptProxy(this, scriptableObject);
			}
			return null;
		}

		// Token: 0x06001167 RID: 4455 RVA: 0x00060CFC File Offset: 0x0005F0FC
		private ScriptProxy CreateCSharpInstance(params object[] args)
		{
			object obj = null;
			try
			{
				obj = Activator.CreateInstance(this.rawType, DynamicCSharp.Settings.GetMemberBindings(), null, args, null);
			}
			catch (MissingMethodException)
			{
				if (args.Length > 0)
				{
					return null;
				}
				obj = FormatterServices.GetUninitializedObject(this.rawType);
			}
			if (obj != null)
			{
				return new ScriptProxy(this, obj);
			}
			return null;
		}

		// Token: 0x06001168 RID: 4456 RVA: 0x00060D6C File Offset: 0x0005F16C
		public bool IsSubtypeOf(Type baseClass)
		{
			return baseClass.IsAssignableFrom(this.rawType);
		}

		// Token: 0x06001169 RID: 4457 RVA: 0x00060D7A File Offset: 0x0005F17A
		public bool IsSubtypeOf<T>()
		{
			return this.IsSubtypeOf(typeof(T));
		}

		// Token: 0x0600116A RID: 4458 RVA: 0x00060D8C File Offset: 0x0005F18C
		public FieldInfo FindCachedField(string name)
		{
			if (this.fieldCache.ContainsKey(name))
			{
				return this.fieldCache[name];
			}
			FieldInfo field = this.rawType.GetField(name, DynamicCSharp.Settings.GetMemberBindings());
			if (field == null)
			{
				return null;
			}
			this.fieldCache.Add(name, field);
			return field;
		}

		// Token: 0x0600116B RID: 4459 RVA: 0x00060DE4 File Offset: 0x0005F1E4
		public PropertyInfo FindCachedProperty(string name)
		{
			if (this.propertyCache.ContainsKey(name))
			{
				return this.propertyCache[name];
			}
			PropertyInfo property = this.rawType.GetProperty(name, DynamicCSharp.Settings.GetMemberBindings());
			if (property == null)
			{
				return null;
			}
			this.propertyCache.Add(name, property);
			return property;
		}

		// Token: 0x0600116C RID: 4460 RVA: 0x00060E3C File Offset: 0x0005F23C
		public MethodInfo FindCachedMethod(string name)
		{
			if (this.methodCache.ContainsKey(name))
			{
				return this.methodCache[name];
			}
			MethodInfo method = this.rawType.GetMethod(name, DynamicCSharp.Settings.GetMemberBindings());
			if (method == null)
			{
				return null;
			}
			this.methodCache.Add(name, method);
			return method;
		}

		// Token: 0x0600116D RID: 4461 RVA: 0x00060E94 File Offset: 0x0005F294
		public object CallStatic(string methodName)
		{
			MethodInfo methodInfo = this.FindCachedMethod(methodName);
			if (methodInfo == null)
			{
				throw new TargetException(string.Format("Type '{0}' does not define a static method called '{1}'", this, methodName));
			}
			if (!methodInfo.IsStatic)
			{
				throw new TargetException(string.Format("The target method '{0}' is not marked as static and must be called on an object", methodName));
			}
			return methodInfo.Invoke(null, null);
		}

		// Token: 0x0600116E RID: 4462 RVA: 0x00060EE8 File Offset: 0x0005F2E8
		public object CallStatic(string methodName, params object[] arguments)
		{
			MethodInfo methodInfo = this.FindCachedMethod(methodName);
			if (methodInfo == null)
			{
				throw new TargetException(string.Format("Type '{0}' does not define a static method called '{1}'", this, methodName));
			}
			if (!methodInfo.IsStatic)
			{
				throw new TargetException(string.Format("The target method '{0}' is not marked as static and must be called on an object", methodName));
			}
			return methodInfo.Invoke(null, arguments);
		}

		// Token: 0x0600116F RID: 4463 RVA: 0x00060F3C File Offset: 0x0005F33C
		public object SafeCallStatic(string method)
		{
			object result;
			try
			{
				result = this.CallStatic(method);
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06001170 RID: 4464 RVA: 0x00060F70 File Offset: 0x0005F370
		public object SafeCallStatic(string method, params object[] arguments)
		{
			object result;
			try
			{
				result = this.CallStatic(method, arguments);
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06001171 RID: 4465 RVA: 0x00060FA4 File Offset: 0x0005F3A4
		public override string ToString()
		{
			return string.Format("ScriptType({0})", this.rawType.Name);
		}

		// Token: 0x04000F26 RID: 3878
		private Dictionary<string, FieldInfo> fieldCache = new Dictionary<string, FieldInfo>();

		// Token: 0x04000F27 RID: 3879
		private Dictionary<string, PropertyInfo> propertyCache = new Dictionary<string, PropertyInfo>();

		// Token: 0x04000F28 RID: 3880
		private Dictionary<string, MethodInfo> methodCache = new Dictionary<string, MethodInfo>();

		// Token: 0x04000F29 RID: 3881
		private Type rawType;

		// Token: 0x04000F2A RID: 3882
		private ScriptAssembly assembly;
	}
}
