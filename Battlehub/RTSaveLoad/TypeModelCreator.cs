using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using ProtoBuf;
using ProtoBuf.Meta;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x02000235 RID: 565
	public class TypeModelCreator
	{
		// Token: 0x06000BCB RID: 3019 RVA: 0x0004A416 File Offset: 0x00048816
		public TypeModelCreator()
		{
		}

		// Token: 0x06000BCC RID: 3020 RVA: 0x0004A420 File Offset: 0x00048820
		public RuntimeTypeModel Create()
		{
			RuntimeTypeModel runtimeTypeModel = TypeModel.Create();
			this.RegisterTypes(runtimeTypeModel);
			return runtimeTypeModel;
		}

		// Token: 0x06000BCD RID: 3021 RVA: 0x0004A43C File Offset: 0x0004883C
		protected void RegisterTypes(RuntimeTypeModel model)
		{
			IEnumerable<Type> allFromCurrentAssembly = Reflection.GetAllFromCurrentAssembly();
			if (TypeModelCreator.<>f__am$cache0 == null)
			{
				TypeModelCreator.<>f__am$cache0 = new Func<Type, bool>(TypeModelCreator.<RegisterTypes>m__0);
			}
			Type[] array = allFromCurrentAssembly.Where(TypeModelCreator.<>f__am$cache0).ToArray<Type>();
			foreach (Type type in array)
			{
				if (!type.IsGenericType())
				{
					model.Add(type, true);
					model.Add(typeof(PrimitiveContract<>).MakeGenericType(new Type[]
					{
						type.MakeArrayType()
					}), true);
					model.Add(typeof(List<>).MakeGenericType(new Type[]
					{
						type
					}), true);
				}
			}
			Type[] array3 = new Type[]
			{
				typeof(bool),
				typeof(char),
				typeof(byte),
				typeof(short),
				typeof(int),
				typeof(long),
				typeof(ushort),
				typeof(uint),
				typeof(ulong),
				typeof(string),
				typeof(float),
				typeof(double),
				typeof(decimal)
			};
			foreach (Type type2 in array3)
			{
				if (!type2.IsGenericType())
				{
					model.Add(typeof(List<>).MakeGenericType(new Type[]
					{
						type2
					}), true);
				}
			}
			Dictionary<Type, ISerializationSurrogate> surrogates = SerializationSurrogates.GetSurrogates();
			foreach (KeyValuePair<Type, ISerializationSurrogate> keyValuePair in surrogates)
			{
				model.Add(keyValuePair.Value.GetType(), true);
				model.Add(keyValuePair.Key, false).SetSurrogate(keyValuePair.Value.GetType());
				model.Add(typeof(PrimitiveContract<>).MakeGenericType(new Type[]
				{
					keyValuePair.Key.MakeArrayType()
				}), true);
				model.Add(typeof(List<>).MakeGenericType(new Type[]
				{
					keyValuePair.Key
				}), true);
			}
		}

		// Token: 0x06000BCE RID: 3022 RVA: 0x0004A6D0 File Offset: 0x00048AD0
		[CompilerGenerated]
		private static bool <RegisterTypes>m__0(Type type)
		{
			return type.IsDefined(typeof(ProtoContractAttribute), false);
		}

		// Token: 0x04000CA9 RID: 3241
		[CompilerGenerated]
		private static Func<Type, bool> <>f__am$cache0;
	}
}
