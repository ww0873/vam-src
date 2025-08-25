using System;
using System.CodeDom.Compiler;
using Mono.CSharp;

namespace DynamicCSharp.Compiler
{
	// Token: 0x020002D3 RID: 723
	internal sealed class McsReporter : ReportPrinter
	{
		// Token: 0x060010C6 RID: 4294 RVA: 0x0005E4AC File Offset: 0x0005C8AC
		public McsReporter(CompilerResults results)
		{
			this.results = results;
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x060010C7 RID: 4295 RVA: 0x0005E4BB File Offset: 0x0005C8BB
		public int WarningCount
		{
			get
			{
				return this.warningCount;
			}
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x060010C8 RID: 4296 RVA: 0x0005E4C3 File Offset: 0x0005C8C3
		public int ErrorCount
		{
			get
			{
				return this.errorCount;
			}
		}

		// Token: 0x060010C9 RID: 4297 RVA: 0x0005E4CC File Offset: 0x0005C8CC
		public override void Print(AbstractMessage msg, bool showFullPath)
		{
			if (msg.IsWarning)
			{
				this.warningCount++;
			}
			else
			{
				this.errorCount++;
			}
			string fileName = "<Unknown>";
			if (msg.Location.SourceFile != null)
			{
				if (showFullPath)
				{
					if (!string.IsNullOrEmpty(msg.Location.SourceFile.FullPathName))
					{
						fileName = msg.Location.SourceFile.FullPathName;
					}
				}
				else if (!string.IsNullOrEmpty(msg.Location.SourceFile.Name))
				{
					fileName = msg.Location.SourceFile.Name;
				}
			}
			this.results.Errors.Add(new CompilerError
			{
				IsWarning = msg.IsWarning,
				Column = ((!msg.Location.IsNull) ? msg.Location.Column : -1),
				Line = ((!msg.Location.IsNull) ? msg.Location.Row : -1),
				ErrorNumber = msg.Code.ToString(),
				ErrorText = msg.Text,
				FileName = fileName
			});
		}

		// Token: 0x04000EDC RID: 3804
		private readonly CompilerResults results;

		// Token: 0x04000EDD RID: 3805
		private int warningCount;

		// Token: 0x04000EDE RID: 3806
		private int errorCount;
	}
}
