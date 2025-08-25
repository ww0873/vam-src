using System;

namespace DynamicCSharp.Security
{
	// Token: 0x020002E2 RID: 738
	public struct AssemblySecurityError
	{
		// Token: 0x06001172 RID: 4466 RVA: 0x00060FBB File Offset: 0x0005F3BB
		public override string ToString()
		{
			return string.Format("Security Check Failed ({0}) : [{1}, {2}] : {3}", new object[]
			{
				this.securityType,
				this.assemblyName,
				this.moduleName,
				this.securityMessage
			});
		}

		// Token: 0x04000F2B RID: 3883
		public string assemblyName;

		// Token: 0x04000F2C RID: 3884
		public string moduleName;

		// Token: 0x04000F2D RID: 3885
		public string securityMessage;

		// Token: 0x04000F2E RID: 3886
		public string securityType;
	}
}
