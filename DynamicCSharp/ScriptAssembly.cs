using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace DynamicCSharp
{
	// Token: 0x020002DA RID: 730
	public sealed class ScriptAssembly
	{
		// Token: 0x060010F9 RID: 4345 RVA: 0x0005F246 File Offset: 0x0005D646
		internal ScriptAssembly(ScriptDomain domain, Assembly rawAssembly)
		{
			this.domain = domain;
			this.rawAssembly = rawAssembly;
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x060010FA RID: 4346 RVA: 0x0005F25C File Offset: 0x0005D65C
		public ScriptDomain Domain
		{
			get
			{
				return this.domain;
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x060010FB RID: 4347 RVA: 0x0005F264 File Offset: 0x0005D664
		public ScriptType MainType
		{
			get
			{
				Type[] types = this.rawAssembly.GetTypes();
				if (types.Length == 0)
				{
					throw new InvalidProgramException("The assembly does not contain a 'MainType'");
				}
				return new ScriptType(this, types[0]);
			}
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x060010FC RID: 4348 RVA: 0x0005F299 File Offset: 0x0005D699
		public Assembly RawAssembly
		{
			get
			{
				return this.rawAssembly;
			}
		}

		// Token: 0x060010FD RID: 4349 RVA: 0x0005F2A1 File Offset: 0x0005D6A1
		public bool HasType(string name)
		{
			return this.FindType(name) != null;
		}

		// Token: 0x060010FE RID: 4350 RVA: 0x0005F2B0 File Offset: 0x0005D6B0
		public bool HasSubtypeOf(Type baseType)
		{
			return this.FindSubtypeOf(baseType) != null;
		}

		// Token: 0x060010FF RID: 4351 RVA: 0x0005F2BF File Offset: 0x0005D6BF
		public bool HasSubtypeOf(Type baseType, string name)
		{
			return this.FindSubtypeOf(baseType, name) != null;
		}

		// Token: 0x06001100 RID: 4352 RVA: 0x0005F2CF File Offset: 0x0005D6CF
		public bool HasSubtypeOf<T>()
		{
			return this.FindSubtypeOf<T>() != null;
		}

		// Token: 0x06001101 RID: 4353 RVA: 0x0005F2DD File Offset: 0x0005D6DD
		public bool HasSubtypeOf<T>(string name)
		{
			return this.FindSubtypeOf<T>(name) != null;
		}

		// Token: 0x06001102 RID: 4354 RVA: 0x0005F2EC File Offset: 0x0005D6EC
		public ScriptType FindType(string name)
		{
			Type type = this.rawAssembly.GetType(name, false, DynamicCSharp.Settings.caseSensitiveNames);
			if (type == null)
			{
				return null;
			}
			return new ScriptType(this, type);
		}

		// Token: 0x06001103 RID: 4355 RVA: 0x0005F320 File Offset: 0x0005D720
		public ScriptType FindSubtypeOf(Type baseType)
		{
			foreach (ScriptType scriptType in this.FindAllTypes())
			{
				if (scriptType.IsSubtypeOf(baseType))
				{
					return scriptType;
				}
			}
			return null;
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x0005F35C File Offset: 0x0005D75C
		public ScriptType FindSubtypeOf(Type baseType, string name)
		{
			ScriptType scriptType = this.FindType(name);
			if (scriptType == null)
			{
				return null;
			}
			if (scriptType.IsSubtypeOf(baseType))
			{
				return scriptType;
			}
			return null;
		}

		// Token: 0x06001105 RID: 4357 RVA: 0x0005F388 File Offset: 0x0005D788
		public ScriptType FindSubtypeOf<T>()
		{
			return this.FindSubtypeOf(typeof(T));
		}

		// Token: 0x06001106 RID: 4358 RVA: 0x0005F39A File Offset: 0x0005D79A
		public ScriptType FindSubtypeOf<T>(string name)
		{
			return this.FindSubtypeOf(typeof(T), name);
		}

		// Token: 0x06001107 RID: 4359 RVA: 0x0005F3B0 File Offset: 0x0005D7B0
		public ScriptType[] FindAllSubtypesOf(Type baseType)
		{
			List<ScriptType> list = new List<ScriptType>();
			foreach (Type type in this.rawAssembly.GetTypes())
			{
				if (DynamicCSharp.Settings.discoverNonPublicTypes || type.IsPublic)
				{
					ScriptType scriptType = new ScriptType(this, type);
					if (scriptType.IsSubtypeOf(baseType))
					{
						list.Add(scriptType);
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x06001108 RID: 4360 RVA: 0x0005F42A File Offset: 0x0005D82A
		public ScriptType[] FindAllSubtypesOf<T>()
		{
			return this.FindAllSubtypesOf(typeof(T));
		}

		// Token: 0x06001109 RID: 4361 RVA: 0x0005F43C File Offset: 0x0005D83C
		public ScriptType[] FindAllTypes()
		{
			List<ScriptType> list = new List<ScriptType>();
			foreach (Type type in this.rawAssembly.GetTypes())
			{
				if (DynamicCSharp.Settings.discoverNonPublicTypes || type.IsPublic)
				{
					ScriptType item = new ScriptType(this, type);
					list.Add(item);
				}
			}
			return list.ToArray();
		}

		// Token: 0x0600110A RID: 4362 RVA: 0x0005F4A9 File Offset: 0x0005D8A9
		public ScriptType[] FindAllUnityTypes()
		{
			return this.FindAllSubtypesOf<UnityEngine.Object>();
		}

		// Token: 0x0600110B RID: 4363 RVA: 0x0005F4B1 File Offset: 0x0005D8B1
		public ScriptType[] FindAllMonoBehaviourTypes()
		{
			return this.FindAllSubtypesOf<MonoBehaviour>();
		}

		// Token: 0x0600110C RID: 4364 RVA: 0x0005F4B9 File Offset: 0x0005D8B9
		public ScriptType[] FindAllScriptableObjectTypes()
		{
			return this.FindAllSubtypesOf<ScriptableObject>();
		}

		// Token: 0x04000F02 RID: 3842
		private ScriptDomain domain;

		// Token: 0x04000F03 RID: 3843
		private Assembly rawAssembly;
	}
}
