using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

namespace DynamicCSharp
{
	// Token: 0x020002E0 RID: 736
	public class ScriptProxy : IDisposable
	{
		// Token: 0x06001143 RID: 4419 RVA: 0x000607B8 File Offset: 0x0005EBB8
		internal ScriptProxy(ScriptType scriptType, object instance)
		{
			this.scriptType = scriptType;
			this.instance = instance;
			this.fields = new FieldProxy(this);
			this.properies = new PropertyProxy(this);
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06001144 RID: 4420 RVA: 0x000607ED File Offset: 0x0005EBED
		public ScriptType ScriptType
		{
			get
			{
				this.CheckDisposed();
				return this.scriptType;
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06001145 RID: 4421 RVA: 0x000607FB File Offset: 0x0005EBFB
		public IMemberProxy Fields
		{
			get
			{
				this.CheckDisposed();
				return this.fields;
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06001146 RID: 4422 RVA: 0x00060809 File Offset: 0x0005EC09
		public IMemberProxy Properties
		{
			get
			{
				this.CheckDisposed();
				return this.properies;
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06001147 RID: 4423 RVA: 0x00060817 File Offset: 0x0005EC17
		public object Instance
		{
			get
			{
				this.CheckDisposed();
				return this.instance;
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06001148 RID: 4424 RVA: 0x00060825 File Offset: 0x0005EC25
		public UnityEngine.Object UnityInstance
		{
			get
			{
				this.CheckDisposed();
				return this.instance as UnityEngine.Object;
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06001149 RID: 4425 RVA: 0x00060838 File Offset: 0x0005EC38
		public MonoBehaviour BehaviourInstance
		{
			get
			{
				this.CheckDisposed();
				return this.instance as MonoBehaviour;
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x0600114A RID: 4426 RVA: 0x0006084B File Offset: 0x0005EC4B
		public ScriptableObject ScriptableInstance
		{
			get
			{
				this.CheckDisposed();
				return this.instance as ScriptableObject;
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x0600114B RID: 4427 RVA: 0x0006085E File Offset: 0x0005EC5E
		public bool IsUnityObject
		{
			get
			{
				this.CheckDisposed();
				return this.scriptType.IsUnityObject;
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x0600114C RID: 4428 RVA: 0x00060871 File Offset: 0x0005EC71
		public bool IsMonoBehaviour
		{
			get
			{
				this.CheckDisposed();
				return this.scriptType.IsMonoBehaviour;
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x0600114D RID: 4429 RVA: 0x00060884 File Offset: 0x0005EC84
		public bool IsScriptableObject
		{
			get
			{
				this.CheckDisposed();
				return this.scriptType.IsScriptableObject;
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x0600114E RID: 4430 RVA: 0x00060897 File Offset: 0x0005EC97
		public bool IsDisposed
		{
			get
			{
				return this.instance == null;
			}
		}

		// Token: 0x0600114F RID: 4431 RVA: 0x000608A4 File Offset: 0x0005ECA4
		public object Call(string methodName)
		{
			this.CheckDisposed();
			MethodInfo methodInfo = this.scriptType.FindCachedMethod(methodName);
			if (methodInfo == null)
			{
				throw new TargetException(string.Format("Type '{0}' does not define a method called '{1}'", this.ScriptType, methodName));
			}
			object obj = methodInfo.Invoke(this.instance, null);
			if (obj is IEnumerator && this.supportCoroutines)
			{
				IEnumerator routine = obj as IEnumerator;
				if (this.IsMonoBehaviour)
				{
					MonoBehaviour instanceAs = this.GetInstanceAs<MonoBehaviour>(false);
					instanceAs.StartCoroutine(routine);
				}
			}
			return obj;
		}

		// Token: 0x06001150 RID: 4432 RVA: 0x00060928 File Offset: 0x0005ED28
		public object Call(string methodName, params object[] arguments)
		{
			this.CheckDisposed();
			MethodInfo methodInfo = this.scriptType.FindCachedMethod(methodName);
			if (methodInfo == null)
			{
				throw new TargetException(string.Format("Type '{0}' does not define a method called '{1}'", this.ScriptType, methodName));
			}
			object obj = methodInfo.Invoke(this.instance, arguments);
			if (obj is IEnumerator && this.supportCoroutines)
			{
				IEnumerator routine = obj as IEnumerator;
				if (this.IsMonoBehaviour)
				{
					MonoBehaviour instanceAs = this.GetInstanceAs<MonoBehaviour>(false);
					instanceAs.StartCoroutine(routine);
				}
			}
			return obj;
		}

		// Token: 0x06001151 RID: 4433 RVA: 0x000609AC File Offset: 0x0005EDAC
		public object SafeCall(string method)
		{
			object result;
			try
			{
				this.CheckDisposed();
				result = this.Call(method);
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06001152 RID: 4434 RVA: 0x000609E8 File Offset: 0x0005EDE8
		public object SafeCall(string method, params object[] arguments)
		{
			object result;
			try
			{
				this.CheckDisposed();
				result = this.Call(method);
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06001153 RID: 4435 RVA: 0x00060A24 File Offset: 0x0005EE24
		public Type GetInstanceType()
		{
			this.CheckDisposed();
			return this.instance.GetType();
		}

		// Token: 0x06001154 RID: 4436 RVA: 0x00060A38 File Offset: 0x0005EE38
		public T GetInstanceAs<T>(bool throwOnError)
		{
			if (throwOnError)
			{
				return (T)((object)this.instance);
			}
			T result;
			try
			{
				T t = (T)((object)this.instance);
				result = t;
			}
			catch
			{
				result = default(T);
			}
			return result;
		}

		// Token: 0x06001155 RID: 4437 RVA: 0x00060A8C File Offset: 0x0005EE8C
		public virtual void Dispose()
		{
			this.CheckDisposed();
			if (this.IsUnityObject)
			{
				UnityEngine.Object.Destroy(this.UnityInstance);
			}
			this.scriptType = null;
			this.instance = null;
		}

		// Token: 0x06001156 RID: 4438 RVA: 0x00060AB8 File Offset: 0x0005EEB8
		public void MakePersistent()
		{
			if (this.IsUnityObject)
			{
				UnityEngine.Object.DontDestroyOnLoad(this.UnityInstance);
			}
		}

		// Token: 0x06001157 RID: 4439 RVA: 0x00060AD0 File Offset: 0x0005EED0
		private void CheckDisposed()
		{
			if (this.instance == null)
			{
				throw new ObjectDisposedException("The script has already been disposed. Unity types can be disposed automatically when the wrapped type is destroyed");
			}
		}

		// Token: 0x04000F21 RID: 3873
		private ScriptType scriptType;

		// Token: 0x04000F22 RID: 3874
		private IMemberProxy fields;

		// Token: 0x04000F23 RID: 3875
		private IMemberProxy properies;

		// Token: 0x04000F24 RID: 3876
		private object instance;

		// Token: 0x04000F25 RID: 3877
		public bool supportCoroutines = true;
	}
}
