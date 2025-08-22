using System;
using System.Collections.Generic;

namespace GPUTools.Common.Scripts.Tools.Commands
{
	// Token: 0x020009BF RID: 2495
	public class BuildChainCommand : IBuildCommand
	{
		// Token: 0x06003F0F RID: 16143 RVA: 0x00127E66 File Offset: 0x00126266
		public BuildChainCommand()
		{
		}

		// Token: 0x06003F10 RID: 16144 RVA: 0x00127E79 File Offset: 0x00126279
		public void Add(IBuildCommand command)
		{
			this.commands.Add(command);
		}

		// Token: 0x06003F11 RID: 16145 RVA: 0x00127E88 File Offset: 0x00126288
		public void Build()
		{
			for (int i = 0; i < this.commands.Count; i++)
			{
				this.commands[i].Build();
			}
			this.OnBuild();
		}

		// Token: 0x06003F12 RID: 16146 RVA: 0x00127EC8 File Offset: 0x001262C8
		public void UpdateSettings()
		{
			for (int i = 0; i < this.commands.Count; i++)
			{
				this.commands[i].UpdateSettings();
			}
			this.OnUpdateSettings();
		}

		// Token: 0x06003F13 RID: 16147 RVA: 0x00127F08 File Offset: 0x00126308
		public virtual void Dispatch()
		{
			for (int i = 0; i < this.commands.Count; i++)
			{
				this.commands[i].Dispatch();
			}
			this.OnDispatch();
		}

		// Token: 0x06003F14 RID: 16148 RVA: 0x00127F48 File Offset: 0x00126348
		public virtual void FixedDispatch()
		{
			for (int i = 0; i < this.commands.Count; i++)
			{
				this.commands[i].FixedDispatch();
			}
			this.OnFixedDispatch();
		}

		// Token: 0x06003F15 RID: 16149 RVA: 0x00127F88 File Offset: 0x00126388
		public void Dispose()
		{
			for (int i = 0; i < this.commands.Count; i++)
			{
				this.commands[i].Dispose();
			}
			this.OnDispose();
		}

		// Token: 0x06003F16 RID: 16150 RVA: 0x00127FC8 File Offset: 0x001263C8
		protected virtual void OnBuild()
		{
		}

		// Token: 0x06003F17 RID: 16151 RVA: 0x00127FCA File Offset: 0x001263CA
		protected virtual void OnUpdateSettings()
		{
		}

		// Token: 0x06003F18 RID: 16152 RVA: 0x00127FCC File Offset: 0x001263CC
		protected virtual void OnDispatch()
		{
		}

		// Token: 0x06003F19 RID: 16153 RVA: 0x00127FCE File Offset: 0x001263CE
		protected virtual void OnFixedDispatch()
		{
		}

		// Token: 0x06003F1A RID: 16154 RVA: 0x00127FD0 File Offset: 0x001263D0
		protected virtual void OnDispose()
		{
		}

		// Token: 0x04002FF0 RID: 12272
		private readonly List<IBuildCommand> commands = new List<IBuildCommand>();
	}
}
