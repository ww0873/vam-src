using System;
using System.Collections.Generic;
using System.IO;
using Mono.Cecil;

namespace DynamicCSharp.Security
{
	// Token: 0x020002E3 RID: 739
	internal sealed class AssemblyChecker
	{
		// Token: 0x06001173 RID: 4467 RVA: 0x00060FF1 File Offset: 0x0005F3F1
		public AssemblyChecker()
		{
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06001174 RID: 4468 RVA: 0x00061005 File Offset: 0x0005F405
		public AssemblySecurityError[] Errors
		{
			get
			{
				return this.errors;
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06001175 RID: 4469 RVA: 0x0006100D File Offset: 0x0005F40D
		public bool HasErrors
		{
			get
			{
				return this.errors.Length > 0;
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06001176 RID: 4470 RVA: 0x0006101A File Offset: 0x0005F41A
		public int ErrorCount
		{
			get
			{
				return this.errors.Length;
			}
		}

		// Token: 0x06001177 RID: 4471 RVA: 0x00061024 File Offset: 0x0005F424
		public void SecurityCheckAssembly(byte[] assemblyData)
		{
			this.ClearErrors();
			AssemblyDefinition assemblyDefinition = null;
			using (MemoryStream memoryStream = new MemoryStream(assemblyData))
			{
				assemblyDefinition = AssemblyDefinition.ReadAssembly(memoryStream);
			}
			foreach (ModuleDefinition module in assemblyDefinition.Modules)
			{
				this.SecurityCheckModule(assemblyDefinition.Name, module);
			}
		}

		// Token: 0x06001178 RID: 4472 RVA: 0x000610BC File Offset: 0x0005F4BC
		private void SecurityCheckModule(AssemblyNameDefinition assemblyName, ModuleDefinition module)
		{
			foreach (ModuleReference moduleReference in module.ModuleReferences)
			{
				this.CreateError(assemblyName.Name, module.Name, "Unmanaged dll references not allowed: " + moduleReference.Name, "ModuleReference");
			}
			IEnumerable<Restriction> restrictions = DynamicCSharp.Settings.Restrictions;
			foreach (Restriction restriction in restrictions)
			{
				if (restriction.Mode == RestrictionMode.Exclusive)
				{
					if (!restriction.Verify(module))
					{
						this.CreateError(assemblyName.Name, module.Name, restriction.Message, restriction.GetType().Name);
					}
				}
				else
				{
					this.CreateError(assemblyName.Name, module.Name, "Inclusive security checking is not implemented", restriction.GetType().Name);
				}
			}
		}

		// Token: 0x06001179 RID: 4473 RVA: 0x000611E8 File Offset: 0x0005F5E8
		private void ClearErrors()
		{
			this.errors = new AssemblySecurityError[0];
		}

		// Token: 0x0600117A RID: 4474 RVA: 0x000611F8 File Offset: 0x0005F5F8
		private void CreateError(string assemblyName, string moduleName, string message, string type)
		{
			AssemblySecurityError assemblySecurityError = new AssemblySecurityError
			{
				assemblyName = assemblyName,
				moduleName = moduleName,
				securityMessage = message,
				securityType = type
			};
			Array.Resize<AssemblySecurityError>(ref this.errors, this.errors.Length + 1);
			this.errors[this.errors.Length - 1] = assemblySecurityError;
		}

		// Token: 0x04000F2F RID: 3887
		private AssemblySecurityError[] errors = new AssemblySecurityError[0];
	}
}
