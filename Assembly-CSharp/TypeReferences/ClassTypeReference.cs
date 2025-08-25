using System;
using UnityEngine;

namespace TypeReferences
{
	// Token: 0x020002B2 RID: 690
	[Serializable]
	public sealed class ClassTypeReference : ISerializationCallbackReceiver
	{
		// Token: 0x0600102D RID: 4141 RVA: 0x0005B7CB File Offset: 0x00059BCB
		public ClassTypeReference()
		{
		}

		// Token: 0x0600102E RID: 4142 RVA: 0x0005B7D3 File Offset: 0x00059BD3
		public ClassTypeReference(string assemblyQualifiedClassName)
		{
			this.Type = (string.IsNullOrEmpty(assemblyQualifiedClassName) ? null : Type.GetType(assemblyQualifiedClassName));
		}

		// Token: 0x0600102F RID: 4143 RVA: 0x0005B7F8 File Offset: 0x00059BF8
		public ClassTypeReference(Type type)
		{
			this.Type = type;
		}

		// Token: 0x06001030 RID: 4144 RVA: 0x0005B807 File Offset: 0x00059C07
		public static string GetClassRef(Type type)
		{
			return (type == null) ? string.Empty : (type.FullName + ", " + type.Assembly.GetName().Name);
		}

		// Token: 0x06001031 RID: 4145 RVA: 0x0005B83C File Offset: 0x00059C3C
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			if (!string.IsNullOrEmpty(this._classRef))
			{
				this._type = Type.GetType(this._classRef);
				if (this._type == null)
				{
					Debug.LogWarning(string.Format("'{0}' was referenced but class type was not found.", this._classRef));
				}
			}
			else
			{
				this._type = null;
			}
		}

		// Token: 0x06001032 RID: 4146 RVA: 0x0005B896 File Offset: 0x00059C96
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06001033 RID: 4147 RVA: 0x0005B898 File Offset: 0x00059C98
		// (set) Token: 0x06001034 RID: 4148 RVA: 0x0005B8A0 File Offset: 0x00059CA0
		public Type Type
		{
			get
			{
				return this._type;
			}
			set
			{
				if (value != null && !value.IsClass)
				{
					throw new ArgumentException(string.Format("'{0}' is not a class type.", value.FullName), "value");
				}
				this._type = value;
				this._classRef = ClassTypeReference.GetClassRef(value);
			}
		}

		// Token: 0x06001035 RID: 4149 RVA: 0x0005B8EC File Offset: 0x00059CEC
		public static implicit operator string(ClassTypeReference typeReference)
		{
			return typeReference._classRef;
		}

		// Token: 0x06001036 RID: 4150 RVA: 0x0005B8F4 File Offset: 0x00059CF4
		public static implicit operator Type(ClassTypeReference typeReference)
		{
			return typeReference.Type;
		}

		// Token: 0x06001037 RID: 4151 RVA: 0x0005B8FC File Offset: 0x00059CFC
		public static implicit operator ClassTypeReference(Type type)
		{
			return new ClassTypeReference(type);
		}

		// Token: 0x06001038 RID: 4152 RVA: 0x0005B904 File Offset: 0x00059D04
		public override string ToString()
		{
			return (this.Type == null) ? "(None)" : this.Type.FullName;
		}

		// Token: 0x04000E68 RID: 3688
		[SerializeField]
		private string _classRef;

		// Token: 0x04000E69 RID: 3689
		private Type _type;
	}
}
