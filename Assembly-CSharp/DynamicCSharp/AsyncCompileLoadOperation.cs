using System;
using DynamicCSharp.Compiler;

namespace DynamicCSharp
{
	// Token: 0x020002CD RID: 717
	public class AsyncCompileLoadOperation : AsyncCompileOperation
	{
		// Token: 0x06001093 RID: 4243 RVA: 0x0005D568 File Offset: 0x0005B968
		internal AsyncCompileLoadOperation(ScriptDomain domain, Func<bool> asyncCallback) : base(domain.CompilerService, asyncCallback)
		{
			this.domain = domain;
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06001094 RID: 4244 RVA: 0x0005D57E File Offset: 0x0005B97E
		public ScriptType MainType
		{
			get
			{
				return this.loadedAssembly.MainType;
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06001095 RID: 4245 RVA: 0x0005D58B File Offset: 0x0005B98B
		public ScriptAssembly LoadedAssembly
		{
			get
			{
				return this.loadedAssembly;
			}
		}

		// Token: 0x06001096 RID: 4246 RVA: 0x0005D593 File Offset: 0x0005B993
		protected override void DoSyncFinalize()
		{
			base.DoSyncFinalize();
			if (base.IsSuccessful)
			{
				this.loadedAssembly = this.domain.LoadAssembly(this.assemblyData, null);
			}
		}

		// Token: 0x04000EC2 RID: 3778
		private ScriptDomain domain;

		// Token: 0x04000EC3 RID: 3779
		private ScriptAssembly loadedAssembly;
	}
}
