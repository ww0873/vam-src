using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Battlehub
{
	// Token: 0x020000BD RID: 189
	public static class Reflection
	{
		// Token: 0x0600031E RID: 798 RVA: 0x00014645 File Offset: 0x00012A45
		public static object GetDefault(Type type)
		{
			if (type == typeof(string))
			{
				return string.Empty;
			}
			if (type.IsValueType())
			{
				return Activator.CreateInstance(type);
			}
			return null;
		}

		// Token: 0x0600031F RID: 799 RVA: 0x00014670 File Offset: 0x00012A70
		public static bool IsScript(this Type type)
		{
			return type.IsSubclassOf(typeof(MonoBehaviour));
		}

		// Token: 0x06000320 RID: 800 RVA: 0x00014682 File Offset: 0x00012A82
		public static PropertyInfo[] GetSerializableProperties(this Type type)
		{
			IEnumerable<PropertyInfo> properties = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (Reflection.<>f__mg$cache0 == null)
			{
				Reflection.<>f__mg$cache0 = new Func<PropertyInfo, bool>(Reflection.IsSerializable);
			}
			return properties.Where(Reflection.<>f__mg$cache0).ToArray<PropertyInfo>();
		}

		// Token: 0x06000321 RID: 801 RVA: 0x000146B3 File Offset: 0x00012AB3
		public static FieldInfo[] GetSerializableFields(this Type type)
		{
			IEnumerable<FieldInfo> fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (Reflection.<>f__mg$cache1 == null)
			{
				Reflection.<>f__mg$cache1 = new Func<FieldInfo, bool>(Reflection.IsSerializable);
			}
			return fields.Where(Reflection.<>f__mg$cache1).ToArray<FieldInfo>();
		}

		// Token: 0x06000322 RID: 802 RVA: 0x000146E4 File Offset: 0x00012AE4
		private static bool IsSerializable(this FieldInfo field)
		{
			return (field.IsPublic || field.IsDefined(typeof(SerializeField), false)) && !field.IsDefined(typeof(SerializeIgnore), false);
		}

		// Token: 0x06000323 RID: 803 RVA: 0x00014720 File Offset: 0x00012B20
		private static bool IsSerializable(this PropertyInfo property)
		{
			return ((property.CanWrite && property.GetSetMethod(true).IsPublic && property.CanRead && property.GetGetMethod(true).IsPublic) || property.IsDefined(typeof(SerializeField), false)) && !property.IsDefined(typeof(SerializeIgnore), false);
		}

		// Token: 0x06000324 RID: 804 RVA: 0x00014794 File Offset: 0x00012B94
		public static Type[] GetAllFromCurrentAssembly()
		{
			Type[] types = typeof(Reflection).Assembly.GetTypes();
			return types.ToArray<Type>();
		}

		// Token: 0x06000325 RID: 805 RVA: 0x000147BC File Offset: 0x00012BBC
		public static Type[] GetAssignableFromTypes(Type type)
		{
			Reflection.<GetAssignableFromTypes>c__AnonStorey0 <GetAssignableFromTypes>c__AnonStorey = new Reflection.<GetAssignableFromTypes>c__AnonStorey0();
			<GetAssignableFromTypes>c__AnonStorey.type = type;
			IEnumerable<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies();
			if (Reflection.<>f__am$cache0 == null)
			{
				Reflection.<>f__am$cache0 = new Func<Assembly, IEnumerable<Type>>(Reflection.<GetAssignableFromTypes>m__0);
			}
			IEnumerable<Type> source = assemblies.SelectMany(Reflection.<>f__am$cache0).Where(new Func<Type, bool>(<GetAssignableFromTypes>c__AnonStorey.<>m__0));
			return source.ToArray<Type>();
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0001481A File Offset: 0x00012C1A
		public static Type BaseType(this Type type)
		{
			return type.BaseType;
		}

		// Token: 0x06000327 RID: 807 RVA: 0x00014822 File Offset: 0x00012C22
		public static bool IsValueType(this Type type)
		{
			return type.IsValueType;
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0001482A File Offset: 0x00012C2A
		public static bool IsPrimitive(this Type type)
		{
			return type.IsPrimitive;
		}

		// Token: 0x06000329 RID: 809 RVA: 0x00014832 File Offset: 0x00012C32
		public static bool IsArray(this Type type)
		{
			return type.IsArray;
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0001483A File Offset: 0x00012C3A
		public static bool IsGenericType(this Type type)
		{
			return type.IsGenericType;
		}

		// Token: 0x0600032B RID: 811 RVA: 0x00014842 File Offset: 0x00012C42
		public static bool IsEnum(this Type type)
		{
			return type.IsEnum;
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0001484A File Offset: 0x00012C4A
		public static bool IsClass(this Type type)
		{
			return type.IsClass;
		}

		// Token: 0x0600032D RID: 813 RVA: 0x00014852 File Offset: 0x00012C52
		[CompilerGenerated]
		private static IEnumerable<Type> <GetAssignableFromTypes>m__0(Assembly s)
		{
			return s.GetTypes();
		}

		// Token: 0x040003D9 RID: 985
		[CompilerGenerated]
		private static Func<PropertyInfo, bool> <>f__mg$cache0;

		// Token: 0x040003DA RID: 986
		[CompilerGenerated]
		private static Func<FieldInfo, bool> <>f__mg$cache1;

		// Token: 0x040003DB RID: 987
		[CompilerGenerated]
		private static Func<Assembly, IEnumerable<Type>> <>f__am$cache0;

		// Token: 0x02000EA2 RID: 3746
		[CompilerGenerated]
		private sealed class <GetAssignableFromTypes>c__AnonStorey0
		{
			// Token: 0x0600715F RID: 29023 RVA: 0x0001485A File Offset: 0x00012C5A
			public <GetAssignableFromTypes>c__AnonStorey0()
			{
			}

			// Token: 0x06007160 RID: 29024 RVA: 0x00014862 File Offset: 0x00012C62
			internal bool <>m__0(Type p)
			{
				return this.type.IsAssignableFrom(p) && p.IsClass;
			}

			// Token: 0x04006535 RID: 25909
			internal Type type;
		}
	}
}
