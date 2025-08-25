using System;
using System.Collections.Generic;
using Mono.Cecil;
using UnityEngine;

namespace DynamicCSharp.Security
{
	// Token: 0x020002E5 RID: 741
	[Serializable]
	public sealed class RuntimeNamespaceRestriction : Restriction
	{
		// Token: 0x06001181 RID: 4481 RVA: 0x000613B4 File Offset: 0x0005F7B4
		public RuntimeNamespaceRestriction(string restrictedName)
		{
			this.namespaceName = restrictedName;
		}

		// Token: 0x06001182 RID: 4482 RVA: 0x000613D0 File Offset: 0x0005F7D0
		private bool IsTypeException(string type)
		{
			if (this.typeExceptions != null)
			{
				if (this.typeExceptionsHash == null)
				{
					this.typeExceptionsHash = new HashSet<string>();
					foreach (string item in this.typeExceptions)
					{
						this.typeExceptionsHash.Add(item);
					}
				}
				return this.typeExceptionsHash.Contains(type);
			}
			return false;
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06001183 RID: 4483 RVA: 0x00061438 File Offset: 0x0005F838
		public string RestrictedNamespace
		{
			get
			{
				return this.namespaceName;
			}
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06001184 RID: 4484 RVA: 0x00061440 File Offset: 0x0005F840
		public override string Message
		{
			get
			{
				return string.Format("The namespace '{0}' is prohibited and cannot be referenced", this.namespaceName);
			}
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06001185 RID: 4485 RVA: 0x00061452 File Offset: 0x0005F852
		public override RestrictionMode Mode
		{
			get
			{
				return DynamicCSharp.Settings.namespaceRestrictionMode;
			}
		}

		// Token: 0x06001186 RID: 4486 RVA: 0x00061460 File Offset: 0x0005F860
		public override bool Verify(ModuleDefinition module)
		{
			if (string.IsNullOrEmpty(this.namespaceName))
			{
				return true;
			}
			IEnumerable<TypeReference> typeReferences = module.GetTypeReferences();
			foreach (TypeReference typeReference in typeReferences)
			{
				if (!this.IsTypeException(typeReference.FullName))
				{
					string @namespace = typeReference.Namespace;
					if (string.Compare(this.namespaceName, @namespace) == 0)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x04000F33 RID: 3891
		[SerializeField]
		private string namespaceName = string.Empty;

		// Token: 0x04000F34 RID: 3892
		[SerializeField]
		private string[] typeExceptions;

		// Token: 0x04000F35 RID: 3893
		private HashSet<string> typeExceptionsHash;
	}
}
