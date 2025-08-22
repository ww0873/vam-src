using System;
using System.Collections.Generic;
using Mono.Cecil;
using UnityEngine;

namespace DynamicCSharp.Security
{
	// Token: 0x020002E4 RID: 740
	[Serializable]
	public sealed class NamespaceRestriction : Restriction
	{
		// Token: 0x0600117B RID: 4475 RVA: 0x00061268 File Offset: 0x0005F668
		public NamespaceRestriction(string restrictedName)
		{
			this.namespaceName = restrictedName;
		}

		// Token: 0x0600117C RID: 4476 RVA: 0x00061284 File Offset: 0x0005F684
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

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x0600117D RID: 4477 RVA: 0x000612EC File Offset: 0x0005F6EC
		public string RestrictedNamespace
		{
			get
			{
				return this.namespaceName;
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x0600117E RID: 4478 RVA: 0x000612F4 File Offset: 0x0005F6F4
		public override string Message
		{
			get
			{
				return string.Format("The namespace '{0}' is prohibited and cannot be referenced", this.namespaceName);
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x0600117F RID: 4479 RVA: 0x00061306 File Offset: 0x0005F706
		public override RestrictionMode Mode
		{
			get
			{
				return DynamicCSharp.Settings.namespaceRestrictionMode;
			}
		}

		// Token: 0x06001180 RID: 4480 RVA: 0x00061314 File Offset: 0x0005F714
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

		// Token: 0x04000F30 RID: 3888
		[SerializeField]
		private string namespaceName = string.Empty;

		// Token: 0x04000F31 RID: 3889
		[SerializeField]
		private string[] typeExceptions;

		// Token: 0x04000F32 RID: 3890
		private HashSet<string> typeExceptionsHash;
	}
}
