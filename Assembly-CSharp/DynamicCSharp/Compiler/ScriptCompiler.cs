using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace DynamicCSharp.Compiler
{
	// Token: 0x020002D6 RID: 726
	public sealed class ScriptCompiler
	{
		// Token: 0x060010D6 RID: 4310 RVA: 0x0005E648 File Offset: 0x0005CA48
		public ScriptCompiler()
		{
			Type compilerType = ScriptCompiler.CompilerType;
			if (compilerType == null)
			{
				throw new ApplicationException("Failed to load the compiler service. Make sure you have installed the compiler package for runtime script compilation. See documentation for help");
			}
			this.compiler = (ICompiler)Activator.CreateInstance(compilerType);
			if (this.compiler != null)
			{
				this.compiler.OutputDirectory = DynamicCSharp.Settings.compilerWorkingDirectory;
				this.compiler.GenerateSymbols = DynamicCSharp.Settings.debugMode;
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x060010D7 RID: 4311 RVA: 0x0005E6D0 File Offset: 0x0005CAD0
		public static Type CompilerType
		{
			get
			{
				return typeof(ScriptCompiler).Assembly.GetType("DynamicCSharp.Compiler.McsMarshal");
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x060010D8 RID: 4312 RVA: 0x0005E6EB File Offset: 0x0005CAEB
		public string[] Warnings
		{
			get
			{
				return this.warnings;
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x060010D9 RID: 4313 RVA: 0x0005E6F3 File Offset: 0x0005CAF3
		public bool HasWarnings
		{
			get
			{
				return this.warnings.Length > 0;
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x060010DA RID: 4314 RVA: 0x0005E700 File Offset: 0x0005CB00
		public string[] Errors
		{
			get
			{
				return this.errors;
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x060010DB RID: 4315 RVA: 0x0005E708 File Offset: 0x0005CB08
		public bool HasErrors
		{
			get
			{
				return this.errors.Length > 0;
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x060010DC RID: 4316 RVA: 0x0005E715 File Offset: 0x0005CB15
		public byte[] AssemblyData
		{
			get
			{
				return this.assemblyData;
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x060010DD RID: 4317 RVA: 0x0005E71D File Offset: 0x0005CB1D
		public byte[] SymbolsData
		{
			get
			{
				return this.symbolsData;
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x060010DE RID: 4318 RVA: 0x0005E725 File Offset: 0x0005CB25
		public bool IsCompiling
		{
			get
			{
				return this.isCompiling;
			}
		}

		// Token: 0x060010DF RID: 4319 RVA: 0x0005E730 File Offset: 0x0005CB30
		public void PrintWarnings()
		{
			foreach (string message in this.warnings)
			{
				Debug.LogWarning(message);
			}
		}

		// Token: 0x060010E0 RID: 4320 RVA: 0x0005E764 File Offset: 0x0005CB64
		public void PrintErrors()
		{
			foreach (string message in this.errors)
			{
				Debug.LogError(message);
			}
		}

		// Token: 0x060010E1 RID: 4321 RVA: 0x0005E798 File Offset: 0x0005CB98
		public void AddConditionalSymbol(string symbol)
		{
			object obj = ScriptCompiler.compilerLock;
			lock (obj)
			{
				this.compiler.AddConditionalSymbol(symbol);
			}
		}

		// Token: 0x060010E2 RID: 4322 RVA: 0x0005E7DC File Offset: 0x0005CBDC
		public void SetSuggestedAssemblyNamePrefix(string assemblyNamePrefix)
		{
			object obj = ScriptCompiler.compilerLock;
			lock (obj)
			{
				this.compiler.SetSuggestedAssemblyNamePrefix(assemblyNamePrefix);
			}
		}

		// Token: 0x060010E3 RID: 4323 RVA: 0x0005E820 File Offset: 0x0005CC20
		public bool CompileFiles(string[] sourceFiles, params string[] extraReferences)
		{
			this.isCompiling = true;
			this.ResetCompiler();
			ScriptCompilerError[] array = null;
			object obj = ScriptCompiler.compilerLock;
			lock (obj)
			{
				this.compiler.AddReferences(extraReferences);
				array = this.compiler.CompileFiles(sourceFiles);
				this.assemblyData = this.compiler.AssemblyData;
				this.symbolsData = this.compiler.SymbolsData;
			}
			bool flag = true;
			foreach (ScriptCompilerError scriptCompilerError in array)
			{
				if (scriptCompilerError.isWarning)
				{
					this.AddWarning(scriptCompilerError.errorCode, scriptCompilerError.errorText, scriptCompilerError.fileName, scriptCompilerError.line, scriptCompilerError.column);
				}
				else
				{
					flag = false;
					this.AddError(scriptCompilerError.errorCode, scriptCompilerError.errorText, scriptCompilerError.fileName, scriptCompilerError.line, scriptCompilerError.column);
				}
			}
			if (!flag)
			{
				this.assemblyData = null;
				this.symbolsData = null;
			}
			this.isCompiling = false;
			return flag;
		}

		// Token: 0x060010E4 RID: 4324 RVA: 0x0005E954 File Offset: 0x0005CD54
		public bool CompileSources(string[] sourceContent, params string[] extraReferences)
		{
			this.isCompiling = true;
			this.ResetCompiler();
			ScriptCompilerError[] array = null;
			object obj = ScriptCompiler.compilerLock;
			lock (obj)
			{
				this.compiler.AddReferences(extraReferences);
				array = this.compiler.CompileSource(sourceContent);
				this.assemblyData = this.compiler.AssemblyData;
				this.symbolsData = this.compiler.SymbolsData;
			}
			bool flag = true;
			foreach (ScriptCompilerError scriptCompilerError in array)
			{
				if (scriptCompilerError.isWarning)
				{
					this.AddWarning(scriptCompilerError.errorCode, scriptCompilerError.errorText, scriptCompilerError.fileName, scriptCompilerError.line, scriptCompilerError.column);
				}
				else
				{
					flag = false;
					this.AddError(scriptCompilerError.errorCode, scriptCompilerError.errorText, scriptCompilerError.fileName, scriptCompilerError.line, scriptCompilerError.column);
				}
			}
			if (!flag)
			{
				this.assemblyData = null;
				this.symbolsData = null;
			}
			this.isCompiling = false;
			return flag;
		}

		// Token: 0x060010E5 RID: 4325 RVA: 0x0005EA88 File Offset: 0x0005CE88
		public AsyncCompileOperation CompileFilesAsync(string[] sourceFiles, params string[] extraReferences)
		{
			ScriptCompiler.<CompileFilesAsync>c__AnonStorey0 <CompileFilesAsync>c__AnonStorey = new ScriptCompiler.<CompileFilesAsync>c__AnonStorey0();
			<CompileFilesAsync>c__AnonStorey.sourceFiles = sourceFiles;
			<CompileFilesAsync>c__AnonStorey.extraReferences = extraReferences;
			<CompileFilesAsync>c__AnonStorey.$this = this;
			return new AsyncCompileOperation(this, new Func<bool>(<CompileFilesAsync>c__AnonStorey.<>m__0));
		}

		// Token: 0x060010E6 RID: 4326 RVA: 0x0005EAC4 File Offset: 0x0005CEC4
		public AsyncCompileOperation CompileSourcesAsync(string[] sourceContent, params string[] extraReferences)
		{
			ScriptCompiler.<CompileSourcesAsync>c__AnonStorey1 <CompileSourcesAsync>c__AnonStorey = new ScriptCompiler.<CompileSourcesAsync>c__AnonStorey1();
			<CompileSourcesAsync>c__AnonStorey.sourceContent = sourceContent;
			<CompileSourcesAsync>c__AnonStorey.extraReferences = extraReferences;
			<CompileSourcesAsync>c__AnonStorey.$this = this;
			return new AsyncCompileOperation(this, new Func<bool>(<CompileSourcesAsync>c__AnonStorey.<>m__0));
		}

		// Token: 0x060010E7 RID: 4327 RVA: 0x0005EB00 File Offset: 0x0005CF00
		private void AddWarning(string code, string message, string file, int line, int column)
		{
			string text = string.Format("[CS{0}]: {1} in {2} at [{3}, {4}]", new object[]
			{
				code,
				message,
				file,
				line,
				column
			});
			if (line == -1 || column == -1)
			{
				text = string.Format("[CS{0}]: {1}", code, message);
			}
			Array.Resize<string>(ref this.warnings, this.warnings.Length + 1);
			this.warnings[this.warnings.Length - 1] = text;
		}

		// Token: 0x060010E8 RID: 4328 RVA: 0x0005EB84 File Offset: 0x0005CF84
		private void AddError(string code, string message, string file, int line, int column)
		{
			string text = string.Format("[CS{0}]: {1} in {2} at [{3}, {4}]", new object[]
			{
				code,
				message,
				file,
				line,
				column
			});
			if (line == -1 || column == -1)
			{
				text = string.Format("[CS{0}]: {1}", code, message);
			}
			Array.Resize<string>(ref this.errors, this.errors.Length + 1);
			this.errors[this.errors.Length - 1] = text;
		}

		// Token: 0x060010E9 RID: 4329 RVA: 0x0005EC06 File Offset: 0x0005D006
		private void ResetCompiler()
		{
			this.errors = new string[0];
			this.warnings = new string[0];
			this.assemblyData = null;
			this.symbolsData = null;
		}

		// Token: 0x060010EA RID: 4330 RVA: 0x0005EC2E File Offset: 0x0005D02E
		// Note: this type is marked as 'beforefieldinit'.
		static ScriptCompiler()
		{
		}

		// Token: 0x04000EE5 RID: 3813
		private const string compilerModule = "DynamicCSharp.Compiler.McsMarshal";

		// Token: 0x04000EE6 RID: 3814
		private static readonly object compilerLock = new object();

		// Token: 0x04000EE7 RID: 3815
		private ICompiler compiler;

		// Token: 0x04000EE8 RID: 3816
		private string[] warnings = new string[0];

		// Token: 0x04000EE9 RID: 3817
		private string[] errors = new string[0];

		// Token: 0x04000EEA RID: 3818
		private byte[] assemblyData;

		// Token: 0x04000EEB RID: 3819
		private byte[] symbolsData;

		// Token: 0x04000EEC RID: 3820
		private volatile bool isCompiling;

		// Token: 0x02000EEA RID: 3818
		[CompilerGenerated]
		private sealed class <CompileFilesAsync>c__AnonStorey0
		{
			// Token: 0x06007237 RID: 29239 RVA: 0x0005EC3A File Offset: 0x0005D03A
			public <CompileFilesAsync>c__AnonStorey0()
			{
			}

			// Token: 0x06007238 RID: 29240 RVA: 0x0005EC42 File Offset: 0x0005D042
			internal bool <>m__0()
			{
				return this.$this.CompileFiles(this.sourceFiles, this.extraReferences);
			}

			// Token: 0x0400660F RID: 26127
			internal string[] sourceFiles;

			// Token: 0x04006610 RID: 26128
			internal string[] extraReferences;

			// Token: 0x04006611 RID: 26129
			internal ScriptCompiler $this;
		}

		// Token: 0x02000EEB RID: 3819
		[CompilerGenerated]
		private sealed class <CompileSourcesAsync>c__AnonStorey1
		{
			// Token: 0x06007239 RID: 29241 RVA: 0x0005EC5B File Offset: 0x0005D05B
			public <CompileSourcesAsync>c__AnonStorey1()
			{
			}

			// Token: 0x0600723A RID: 29242 RVA: 0x0005EC63 File Offset: 0x0005D063
			internal bool <>m__0()
			{
				return this.$this.CompileSources(this.sourceContent, this.extraReferences);
			}

			// Token: 0x04006612 RID: 26130
			internal string[] sourceContent;

			// Token: 0x04006613 RID: 26131
			internal string[] extraReferences;

			// Token: 0x04006614 RID: 26132
			internal ScriptCompiler $this;
		}
	}
}
