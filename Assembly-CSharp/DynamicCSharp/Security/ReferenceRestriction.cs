using System;
using System.Collections.Generic;
using Mono.Cecil;
using UnityEngine;

namespace DynamicCSharp.Security
{
	// Token: 0x020002E6 RID: 742
	[Serializable]
	public sealed class ReferenceRestriction : Restriction
	{
		// Token: 0x06001187 RID: 4487 RVA: 0x00061500 File Offset: 0x0005F900
		public ReferenceRestriction(string referenceName)
		{
			this.referenceName = referenceName;
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06001188 RID: 4488 RVA: 0x0006151A File Offset: 0x0005F91A
		public string RestrictedName
		{
			get
			{
				return this.referenceName;
			}
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06001189 RID: 4489 RVA: 0x00061522 File Offset: 0x0005F922
		public override string Message
		{
			get
			{
				return string.Format("The references assembly '{0}' is prohibited and cannot be referenced", this.referenceName);
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x0600118A RID: 4490 RVA: 0x00061534 File Offset: 0x0005F934
		public override RestrictionMode Mode
		{
			get
			{
				return DynamicCSharp.Settings.assemblyRestrictionMode;
			}
		}

		// Token: 0x0600118B RID: 4491 RVA: 0x00061540 File Offset: 0x0005F940
		public override bool Verify(ModuleDefinition module)
		{
			if (string.IsNullOrEmpty(this.referenceName))
			{
				return true;
			}
			IEnumerable<AssemblyNameReference> assemblyReferences = module.AssemblyReferences;
			foreach (AssemblyNameReference assemblyNameReference in assemblyReferences)
			{
				if (string.Compare(this.referenceName, assemblyNameReference.Name + ".dll") == 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04000F36 RID: 3894
		[SerializeField]
		private string referenceName = string.Empty;
	}
}
