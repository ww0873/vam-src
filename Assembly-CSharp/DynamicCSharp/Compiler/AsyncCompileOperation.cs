using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace DynamicCSharp.Compiler
{
	// Token: 0x020002CE RID: 718
	public class AsyncCompileOperation : CustomYieldInstruction
	{
		// Token: 0x06001097 RID: 4247 RVA: 0x0005D3DD File Offset: 0x0005B7DD
		internal AsyncCompileOperation(ScriptCompiler handler, Func<bool> asyncCallback)
		{
			this.handler = handler;
			this.asyncCallback = asyncCallback;
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06001098 RID: 4248 RVA: 0x0005D3F3 File Offset: 0x0005B7F3
		public byte[] AssemblyData
		{
			get
			{
				return this.assemblyData;
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06001099 RID: 4249 RVA: 0x0005D3FB File Offset: 0x0005B7FB
		public string[] CompilerErrors
		{
			get
			{
				return this.errors;
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x0600109A RID: 4250 RVA: 0x0005D403 File Offset: 0x0005B803
		public string[] CompilerWarnings
		{
			get
			{
				return this.warnings;
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x0600109B RID: 4251 RVA: 0x0005D40B File Offset: 0x0005B80B
		public bool IsSuccessful
		{
			get
			{
				return this.isSuccessful;
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x0600109C RID: 4252 RVA: 0x0005D413 File Offset: 0x0005B813
		public bool IsDone
		{
			get
			{
				return this.isDone;
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x0600109D RID: 4253 RVA: 0x0005D41C File Offset: 0x0005B81C
		public override bool keepWaiting
		{
			get
			{
				if (!this.hasStarted)
				{
					this.isSuccessful = true;
					ThreadPool.QueueUserWorkItem(new WaitCallback(this.<get_keepWaiting>m__0));
					this.hasStarted = true;
				}
				if (!this.threadExit)
				{
					return true;
				}
				try
				{
					this.DoSyncFinalize();
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
					this.isSuccessful = false;
				}
				if (this.handler.HasErrors)
				{
					this.isSuccessful = false;
				}
				this.isDone = true;
				return false;
			}
		}

		// Token: 0x0600109E RID: 4254 RVA: 0x0005D4B0 File Offset: 0x0005B8B0
		protected virtual void DoSyncFinalize()
		{
			this.assemblyData = this.handler.AssemblyData;
			this.errors = this.handler.Errors;
			this.warnings = this.handler.Warnings;
		}

		// Token: 0x0600109F RID: 4255 RVA: 0x0005D4E5 File Offset: 0x0005B8E5
		public new void Reset()
		{
			this.threadExit = false;
			this.isSuccessful = false;
			this.isDone = false;
			this.hasStarted = false;
		}

		// Token: 0x060010A0 RID: 4256 RVA: 0x0005D508 File Offset: 0x0005B908
		[CompilerGenerated]
		private void <get_keepWaiting>m__0(object state)
		{
			try
			{
				if (this.asyncCallback != null && !this.asyncCallback())
				{
					this.isSuccessful = false;
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				this.isSuccessful = false;
			}
			this.threadExit = true;
		}

		// Token: 0x04000EC4 RID: 3780
		private ScriptCompiler handler;

		// Token: 0x04000EC5 RID: 3781
		private Func<bool> asyncCallback;

		// Token: 0x04000EC6 RID: 3782
		private volatile bool threadExit;

		// Token: 0x04000EC7 RID: 3783
		private bool hasStarted;

		// Token: 0x04000EC8 RID: 3784
		protected byte[] assemblyData;

		// Token: 0x04000EC9 RID: 3785
		protected string[] errors;

		// Token: 0x04000ECA RID: 3786
		protected string[] warnings;

		// Token: 0x04000ECB RID: 3787
		protected bool isSuccessful;

		// Token: 0x04000ECC RID: 3788
		protected bool isDone;
	}
}
