using System;
using System.Reflection;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x020006C6 RID: 1734
	[Serializable]
	public struct SerializableType : ISerializationCallbackReceiver
	{
		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x060029CA RID: 10698 RVA: 0x000E23FA File Offset: 0x000E07FA
		private static Assembly[] _assemblies
		{
			get
			{
				if (SerializableType._cachedAssemblies == null)
				{
					SerializableType._cachedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
				}
				return SerializableType._cachedAssemblies;
			}
		}

		// Token: 0x060029CB RID: 10699 RVA: 0x000E241C File Offset: 0x000E081C
		public void OnAfterDeserialize()
		{
			if (!string.IsNullOrEmpty(this._fullName))
			{
				foreach (Assembly assembly in SerializableType._assemblies)
				{
					this._type = assembly.GetType(this._fullName, false);
					if (this._type != null)
					{
						break;
					}
				}
			}
			else
			{
				this._type = null;
			}
		}

		// Token: 0x060029CC RID: 10700 RVA: 0x000E2486 File Offset: 0x000E0886
		public void OnBeforeSerialize()
		{
			if (this._type != null)
			{
				this._fullName = this._type.FullName;
			}
		}

		// Token: 0x060029CD RID: 10701 RVA: 0x000E24A4 File Offset: 0x000E08A4
		public static implicit operator Type(SerializableType serializableType)
		{
			return serializableType._type;
		}

		// Token: 0x060029CE RID: 10702 RVA: 0x000E24B0 File Offset: 0x000E08B0
		public static implicit operator SerializableType(Type type)
		{
			return new SerializableType
			{
				_type = type
			};
		}

		// Token: 0x060029CF RID: 10703 RVA: 0x000E24CE File Offset: 0x000E08CE
		// Note: this type is marked as 'beforefieldinit'.
		static SerializableType()
		{
		}

		// Token: 0x04002202 RID: 8706
		[SerializeField]
		[HideInInspector]
		private Type _type;

		// Token: 0x04002203 RID: 8707
		[SerializeField]
		[HideInInspector]
		private string _fullName;

		// Token: 0x04002204 RID: 8708
		private static Assembly[] _cachedAssemblies;
	}
}
