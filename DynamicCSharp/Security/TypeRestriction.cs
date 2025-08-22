using System;
using System.Collections.Generic;
using Mono.Cecil;
using UnityEngine;

namespace DynamicCSharp.Security
{
	// Token: 0x020002E8 RID: 744
	[Serializable]
	public sealed class TypeRestriction : Restriction
	{
		// Token: 0x06001190 RID: 4496 RVA: 0x000615D0 File Offset: 0x0005F9D0
		public TypeRestriction(string restrictedName)
		{
			this.typeName = restrictedName;
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06001191 RID: 4497 RVA: 0x000615EA File Offset: 0x0005F9EA
		public string RestrictedType
		{
			get
			{
				return this.typeName;
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06001192 RID: 4498 RVA: 0x000615F2 File Offset: 0x0005F9F2
		public override string Message
		{
			get
			{
				return string.Format("The type '{0}' is prohibited and cannot be referenced", this.typeName);
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06001193 RID: 4499 RVA: 0x00061604 File Offset: 0x0005FA04
		public override RestrictionMode Mode
		{
			get
			{
				return RestrictionMode.Exclusive;
			}
		}

		// Token: 0x06001194 RID: 4500 RVA: 0x00061608 File Offset: 0x0005FA08
		public override bool Verify(ModuleDefinition module)
		{
			if (string.IsNullOrEmpty(this.typeName))
			{
				return true;
			}
			IEnumerable<TypeReference> typeReferences = module.GetTypeReferences();
			foreach (TypeReference typeReference in typeReferences)
			{
				string fullName = typeReference.FullName;
				if (string.Compare(this.typeName, fullName) == 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04000F37 RID: 3895
		[SerializeField]
		private string typeName = string.Empty;
	}
}
