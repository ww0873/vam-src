using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using DynamicCSharp.Security;
using UnityEngine;

namespace DynamicCSharp
{
	// Token: 0x020002D8 RID: 728
	public sealed class DynamicCSharp : ScriptableObject
	{
		// Token: 0x060010EB RID: 4331 RVA: 0x0005EC7C File Offset: 0x0005D07C
		public DynamicCSharp()
		{
			int num = this.assemblyReferences.Length;
			Array.Resize<string>(ref this.assemblyReferences, num + DynamicCSharp.unityAssemblyReferences.Length);
			for (int i = num; i < this.assemblyReferences.Length; i++)
			{
				this.assemblyReferences[i] = DynamicCSharp.unityAssemblyReferences[i - num];
			}
		}

		// Token: 0x060010EC RID: 4332 RVA: 0x0005ED80 File Offset: 0x0005D180
		public void AddRuntimeNamespaceRestriction(string namespaceName)
		{
			if (this.runtimeNamespaceRestrictions == null)
			{
				this.runtimeNamespaceRestrictions = new Dictionary<string, RuntimeNamespaceRestriction>();
			}
			if (!this.runtimeNamespaceRestrictions.ContainsKey(namespaceName))
			{
				RuntimeNamespaceRestriction value = new RuntimeNamespaceRestriction(namespaceName);
				this.runtimeNamespaceRestrictions.Add(namespaceName, value);
			}
		}

		// Token: 0x060010ED RID: 4333 RVA: 0x0005EDC8 File Offset: 0x0005D1C8
		public void RemoveRuntimeNamespaceRestriction(string namespaceName)
		{
			if (this.runtimeNamespaceRestrictions == null)
			{
				this.runtimeNamespaceRestrictions = new Dictionary<string, RuntimeNamespaceRestriction>();
			}
			if (this.runtimeNamespaceRestrictions.ContainsKey(namespaceName))
			{
				this.runtimeNamespaceRestrictions.Remove(namespaceName);
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x060010EE RID: 4334 RVA: 0x0005EDFE File Offset: 0x0005D1FE
		public static DynamicCSharp Settings
		{
			get
			{
				if (DynamicCSharp.instance == null)
				{
					DynamicCSharp.instance = DynamicCSharp.LoadSettings();
				}
				return DynamicCSharp.instance;
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x060010EF RID: 4335 RVA: 0x0005EE1F File Offset: 0x0005D21F
		public static bool IsPlatformSupported
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x060010F0 RID: 4336 RVA: 0x0005EE24 File Offset: 0x0005D224
		public IEnumerable<Restriction> Restrictions
		{
			get
			{
				foreach (NamespaceRestriction r in this.namespaceRestrictions)
				{
					yield return r;
				}
				if (this.runtimeNamespaceRestrictions != null)
				{
					foreach (Restriction r2 in this.runtimeNamespaceRestrictions.Values)
					{
						yield return r2;
					}
				}
				foreach (ReferenceRestriction r3 in this.referenceRestrictions)
				{
					yield return r3;
				}
				foreach (TypeRestriction r4 in this.typeRestrictions)
				{
					yield return r4;
				}
				yield break;
			}
		}

		// Token: 0x060010F1 RID: 4337 RVA: 0x0005EE48 File Offset: 0x0005D248
		internal BindingFlags GetTypeBindings()
		{
			BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;
			if (this.discoverNonPublicTypes)
			{
				bindingFlags |= BindingFlags.NonPublic;
			}
			return bindingFlags;
		}

		// Token: 0x060010F2 RID: 4338 RVA: 0x0005EE6C File Offset: 0x0005D26C
		internal BindingFlags GetMemberBindings()
		{
			BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;
			if (this.discoverNonPublicMembers)
			{
				bindingFlags |= BindingFlags.NonPublic;
			}
			return bindingFlags;
		}

		// Token: 0x060010F3 RID: 4339 RVA: 0x0005EE90 File Offset: 0x0005D290
		private static DynamicCSharp LoadSettings()
		{
			UnityEngine.Object @object = Resources.Load("DynamicCSharp_Settings");
			if (@object != null)
			{
				return @object as DynamicCSharp;
			}
			UnityEngine.Debug.LogWarning("DynamicCSharp: Failed to load settings - Default values will be used");
			return ScriptableObject.CreateInstance<DynamicCSharp>();
		}

		// Token: 0x060010F4 RID: 4340 RVA: 0x0005EECA File Offset: 0x0005D2CA
		public static void SaveAsset(DynamicCSharp save)
		{
		}

		// Token: 0x060010F5 RID: 4341 RVA: 0x0005EECC File Offset: 0x0005D2CC
		public static void LoadAsset()
		{
		}

		// Token: 0x060010F6 RID: 4342 RVA: 0x0005EED0 File Offset: 0x0005D2D0
		// Note: this type is marked as 'beforefieldinit'.
		static DynamicCSharp()
		{
		}

		// Token: 0x04000EF0 RID: 3824
		private const string editorSettingsDirectory = "/Resources";

		// Token: 0x04000EF1 RID: 3825
		private const string settingsLocation = "DynamicCSharp_Settings";

		// Token: 0x04000EF2 RID: 3826
		private const BindingFlags defaultFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;

		// Token: 0x04000EF3 RID: 3827
		private static DynamicCSharp instance = null;

		// Token: 0x04000EF4 RID: 3828
		public bool caseSensitiveNames;

		// Token: 0x04000EF5 RID: 3829
		public bool discoverNonPublicTypes = true;

		// Token: 0x04000EF6 RID: 3830
		public bool discoverNonPublicMembers = true;

		// Token: 0x04000EF7 RID: 3831
		public bool securityCheckCode = true;

		// Token: 0x04000EF8 RID: 3832
		public readonly bool debugMode;

		// Token: 0x04000EF9 RID: 3833
		[HideInInspector]
		public string compilerWorkingDirectory = string.Empty;

		// Token: 0x04000EFA RID: 3834
		public string[] assemblyReferences = new string[]
		{
			"Assembly-CSharp.dll"
		};

		// Token: 0x04000EFB RID: 3835
		public static readonly string[] unityAssemblyReferences = new string[]
		{
			"UnityEngine.AudioModule.dll",
			"UnityEngine.CoreModule.dll",
			"UnityEngine.JSONSerializeModule.dll",
			"UnityEngine.ParticleSystemModule.dll",
			"UnityEngine.PhysicsModule.dll",
			"UnityEngine.UIModule.dll"
		};

		// Token: 0x04000EFC RID: 3836
		public RestrictionMode namespaceRestrictionMode = RestrictionMode.Exclusive;

		// Token: 0x04000EFD RID: 3837
		public RestrictionMode assemblyRestrictionMode = RestrictionMode.Exclusive;

		// Token: 0x04000EFE RID: 3838
		public NamespaceRestriction[] namespaceRestrictions = new NamespaceRestriction[]
		{
			new NamespaceRestriction("System.IO"),
			new NamespaceRestriction("System.Reflection")
		};

		// Token: 0x04000EFF RID: 3839
		private Dictionary<string, RuntimeNamespaceRestriction> runtimeNamespaceRestrictions;

		// Token: 0x04000F00 RID: 3840
		public ReferenceRestriction[] referenceRestrictions = new ReferenceRestriction[]
		{
			new ReferenceRestriction("UnityEditor.dll"),
			new ReferenceRestriction("Mono.Cecil.dll")
		};

		// Token: 0x04000F01 RID: 3841
		public TypeRestriction[] typeRestrictions = new TypeRestriction[]
		{
			new TypeRestriction("System.AppDomain")
		};

		// Token: 0x02000EEC RID: 3820
		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<Restriction>, IEnumerator, IDisposable, IEnumerator<Restriction>
		{
			// Token: 0x0600723B RID: 29243 RVA: 0x0005EF1E File Offset: 0x0005D31E
			[DebuggerHidden]
			public <>c__Iterator0()
			{
			}

			// Token: 0x0600723C RID: 29244 RVA: 0x0005EF28 File Offset: 0x0005D328
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0U:
					array = this.namespaceRestrictions;
					i = 0;
					break;
				case 1U:
					i++;
					break;
				case 2U:
					Block_5:
					try
					{
						switch (num)
						{
						}
						if (enumerator.MoveNext())
						{
							r2 = enumerator.Current;
							this.$current = r2;
							if (!this.$disposing)
							{
								this.$PC = 2;
							}
							flag = true;
							return true;
						}
					}
					finally
					{
						if (!flag)
						{
							((IDisposable)enumerator).Dispose();
						}
					}
					goto IL_13D;
				case 3U:
					j++;
					goto IL_19B;
				case 4U:
					k++;
					goto IL_20C;
				default:
					return false;
				}
				if (i < array.Length)
				{
					r = array[i];
					this.$current = r;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				if (this.runtimeNamespaceRestrictions != null)
				{
					enumerator = this.runtimeNamespaceRestrictions.Values.GetEnumerator();
					num = 4294967293U;
					goto Block_5;
				}
				IL_13D:
				array2 = this.referenceRestrictions;
				j = 0;
				IL_19B:
				if (j < array2.Length)
				{
					r3 = array2[j];
					this.$current = r3;
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				array3 = this.typeRestrictions;
				k = 0;
				IL_20C:
				if (k < array3.Length)
				{
					r4 = array3[k];
					this.$current = r4;
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				}
				this.$PC = -1;
				return false;
			}

			// Token: 0x170010B7 RID: 4279
			// (get) Token: 0x0600723D RID: 29245 RVA: 0x0005F170 File Offset: 0x0005D570
			Restriction IEnumerator<Restriction>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170010B8 RID: 4280
			// (get) Token: 0x0600723E RID: 29246 RVA: 0x0005F178 File Offset: 0x0005D578
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x0600723F RID: 29247 RVA: 0x0005F180 File Offset: 0x0005D580
			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 2U:
					try
					{
					}
					finally
					{
						((IDisposable)enumerator).Dispose();
					}
					break;
				}
			}

			// Token: 0x06007240 RID: 29248 RVA: 0x0005F1E8 File Offset: 0x0005D5E8
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x06007241 RID: 29249 RVA: 0x0005F1EF File Offset: 0x0005D5EF
			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<DynamicCSharp.Security.Restriction>.GetEnumerator();
			}

			// Token: 0x06007242 RID: 29250 RVA: 0x0005F1F8 File Offset: 0x0005D5F8
			[DebuggerHidden]
			IEnumerator<Restriction> IEnumerable<Restriction>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				DynamicCSharp.<>c__Iterator0 <>c__Iterator = new DynamicCSharp.<>c__Iterator0();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}

			// Token: 0x04006615 RID: 26133
			internal NamespaceRestriction[] $locvar0;

			// Token: 0x04006616 RID: 26134
			internal int $locvar1;

			// Token: 0x04006617 RID: 26135
			internal Restriction <r>__1;

			// Token: 0x04006618 RID: 26136
			internal Dictionary<string, RuntimeNamespaceRestriction>.ValueCollection.Enumerator $locvar2;

			// Token: 0x04006619 RID: 26137
			internal Restriction <r>__2;

			// Token: 0x0400661A RID: 26138
			internal ReferenceRestriction[] $locvar3;

			// Token: 0x0400661B RID: 26139
			internal int $locvar4;

			// Token: 0x0400661C RID: 26140
			internal Restriction <r>__3;

			// Token: 0x0400661D RID: 26141
			internal TypeRestriction[] $locvar5;

			// Token: 0x0400661E RID: 26142
			internal int $locvar6;

			// Token: 0x0400661F RID: 26143
			internal Restriction <r>__4;

			// Token: 0x04006620 RID: 26144
			internal DynamicCSharp $this;

			// Token: 0x04006621 RID: 26145
			internal Restriction $current;

			// Token: 0x04006622 RID: 26146
			internal bool $disposing;

			// Token: 0x04006623 RID: 26147
			internal int $PC;
		}
	}
}
