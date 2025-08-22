using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Leap.Unity.Query;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x020006DA RID: 1754
	public static class LeapProjectChecks
	{
		// Token: 0x06002A2B RID: 10795 RVA: 0x000E3C60 File Offset: 0x000E2060
		private static void ensureChecksLoaded()
		{
			if (LeapProjectChecks._projectChecks != null)
			{
				return;
			}
			LeapProjectChecks._projectChecks = new List<LeapProjectChecks.ProjectCheck>();
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			Query<Assembly> query = assemblies.Query<Assembly>();
			if (LeapProjectChecks.<>f__am$cache0 == null)
			{
				LeapProjectChecks.<>f__am$cache0 = new Func<Assembly, ICollection<Type>>(LeapProjectChecks.<ensureChecksLoaded>m__0);
			}
			foreach (Type type in query.SelectMany(LeapProjectChecks.<>f__am$cache0))
			{
				MethodInfo[] methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				for (int i = 0; i < methods.Length; i++)
				{
					LeapProjectChecks.<ensureChecksLoaded>c__AnonStorey0 <ensureChecksLoaded>c__AnonStorey = new LeapProjectChecks.<ensureChecksLoaded>c__AnonStorey0();
					<ensureChecksLoaded>c__AnonStorey.method = methods[i];
					object[] customAttributes = <ensureChecksLoaded>c__AnonStorey.method.GetCustomAttributes(typeof(LeapProjectCheckAttribute), true);
					if (customAttributes.Length != 0)
					{
						LeapProjectCheckAttribute attribute = customAttributes[0] as LeapProjectCheckAttribute;
						LeapProjectChecks._projectChecks.Add(new LeapProjectChecks.ProjectCheck
						{
							checkFunc = new Func<bool>(<ensureChecksLoaded>c__AnonStorey.<>m__0),
							attribute = attribute
						});
					}
				}
			}
			List<LeapProjectChecks.ProjectCheck> projectChecks = LeapProjectChecks._projectChecks;
			if (LeapProjectChecks.<>f__am$cache1 == null)
			{
				LeapProjectChecks.<>f__am$cache1 = new Comparison<LeapProjectChecks.ProjectCheck>(LeapProjectChecks.<ensureChecksLoaded>m__1);
			}
			projectChecks.Sort(LeapProjectChecks.<>f__am$cache1);
		}

		// Token: 0x06002A2C RID: 10796 RVA: 0x000E3DBC File Offset: 0x000E21BC
		public static void DrawProjectChecksGUI()
		{
		}

		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x06002A2D RID: 10797 RVA: 0x000E3DBE File Offset: 0x000E21BE
		private static HashSet<string> _ignoredKeys
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06002A2E RID: 10798 RVA: 0x000E3DC1 File Offset: 0x000E21C1
		public static bool CheckIgnoredKey(string editorPrefKey)
		{
			return false;
		}

		// Token: 0x06002A2F RID: 10799 RVA: 0x000E3DC4 File Offset: 0x000E21C4
		public static void SetIgnoredKey(string editorPrefKey, bool ignore)
		{
		}

		// Token: 0x06002A30 RID: 10800 RVA: 0x000E3DC6 File Offset: 0x000E21C6
		public static void ClearAllIgnoredKeys()
		{
		}

		// Token: 0x06002A31 RID: 10801 RVA: 0x000E3DC8 File Offset: 0x000E21C8
		private static HashSet<string> splitBySemicolonToSet(string ignoredKeys_semicolonDelimited)
		{
			HashSet<string> hashSet = new HashSet<string>();
			foreach (string item in ignoredKeys_semicolonDelimited.Split(new char[]
			{
				';'
			}, StringSplitOptions.RemoveEmptyEntries))
			{
				hashSet.Add(item);
			}
			return hashSet;
		}

		// Token: 0x06002A32 RID: 10802 RVA: 0x000E3E16 File Offset: 0x000E2216
		private static string joinBySemicolon(HashSet<string> keys)
		{
			return string.Join(";", keys.Query<string>().ToArray<string>());
		}

		// Token: 0x06002A33 RID: 10803 RVA: 0x000E3E2D File Offset: 0x000E222D
		private static void uploadignoredKeyChangesToEditorPrefs()
		{
		}

		// Token: 0x06002A34 RID: 10804 RVA: 0x000E3E2F File Offset: 0x000E222F
		// Note: this type is marked as 'beforefieldinit'.
		static LeapProjectChecks()
		{
		}

		// Token: 0x06002A35 RID: 10805 RVA: 0x000E3E31 File Offset: 0x000E2231
		[CompilerGenerated]
		private static ICollection<Type> <ensureChecksLoaded>m__0(Assembly a)
		{
			return a.GetTypes();
		}

		// Token: 0x06002A36 RID: 10806 RVA: 0x000E3E39 File Offset: 0x000E2239
		[CompilerGenerated]
		private static int <ensureChecksLoaded>m__1(LeapProjectChecks.ProjectCheck a, LeapProjectChecks.ProjectCheck b)
		{
			return a.attribute.order.CompareTo(b.attribute.order);
		}

		// Token: 0x04002269 RID: 8809
		private static List<LeapProjectChecks.ProjectCheck> _projectChecks;

		// Token: 0x0400226A RID: 8810
		private const string IGNORED_KEYS_PREF = "LeapUnityWindow_IgnoredKeys";

		// Token: 0x0400226B RID: 8811
		[CompilerGenerated]
		private static Func<Assembly, ICollection<Type>> <>f__am$cache0;

		// Token: 0x0400226C RID: 8812
		[CompilerGenerated]
		private static Comparison<LeapProjectChecks.ProjectCheck> <>f__am$cache1;

		// Token: 0x020006DB RID: 1755
		private struct ProjectCheck
		{
			// Token: 0x0400226D RID: 8813
			public Func<bool> checkFunc;

			// Token: 0x0400226E RID: 8814
			public LeapProjectCheckAttribute attribute;
		}

		// Token: 0x02000F9F RID: 3999
		[CompilerGenerated]
		private sealed class <ensureChecksLoaded>c__AnonStorey0
		{
			// Token: 0x06007489 RID: 29833 RVA: 0x000E3E58 File Offset: 0x000E2258
			public <ensureChecksLoaded>c__AnonStorey0()
			{
			}

			// Token: 0x0600748A RID: 29834 RVA: 0x000E3E60 File Offset: 0x000E2260
			internal bool <>m__0()
			{
				if (!this.method.IsStatic)
				{
					Debug.LogError("Invalid project check definition; project checks must be static methods.");
					return true;
				}
				return this.method.ReturnType != typeof(bool) || (bool)this.method.Invoke(null, null);
			}

			// Token: 0x040068A2 RID: 26786
			internal MethodInfo method;
		}
	}
}
