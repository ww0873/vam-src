using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace DynamicCSharp.Compiler
{
	// Token: 0x020002D2 RID: 722
	internal sealed class McsMarshal : ICompiler
	{
		// Token: 0x060010B8 RID: 4280 RVA: 0x0005E154 File Offset: 0x0005C554
		public McsMarshal()
		{
			this.compiler = new McsCompiler();
			this.parameters = new CompilerParameters();
			this.parameters.GenerateExecutable = false;
			this.parameters.GenerateInMemory = false;
			this.parameters.IncludeDebugInformation = this.generateSymbols;
			this.parameters.TempFiles = new TempFileCollection(Environment.GetEnvironmentVariable("TEMP"), true);
			this.parameters.TempFiles.KeepFiles = true;
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x060010B9 RID: 4281 RVA: 0x0005E1E4 File Offset: 0x0005C5E4
		// (set) Token: 0x060010BA RID: 4282 RVA: 0x0005E1EC File Offset: 0x0005C5EC
		public string OutputDirectory
		{
			get
			{
				return this.outputDirectory;
			}
			set
			{
				McsCompiler.OutputDirectory = value;
				this.outputDirectory = value;
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x060010BB RID: 4283 RVA: 0x0005E1FB File Offset: 0x0005C5FB
		// (set) Token: 0x060010BC RID: 4284 RVA: 0x0005E203 File Offset: 0x0005C603
		public bool GenerateSymbols
		{
			get
			{
				return this.generateSymbols;
			}
			set
			{
				this.generateSymbols = value;
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x060010BD RID: 4285 RVA: 0x0005E20C File Offset: 0x0005C60C
		public byte[] AssemblyData
		{
			get
			{
				return this.assemblyData;
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x060010BE RID: 4286 RVA: 0x0005E214 File Offset: 0x0005C614
		public byte[] SymbolsData
		{
			get
			{
				return this.symbolsData;
			}
		}

		// Token: 0x060010BF RID: 4287 RVA: 0x0005E21C File Offset: 0x0005C61C
		public void AddReference(string reference)
		{
			this.parameters.ReferencedAssemblies.Add(reference);
		}

		// Token: 0x060010C0 RID: 4288 RVA: 0x0005E230 File Offset: 0x0005C630
		public void AddReferences(IEnumerable<string> references)
		{
			foreach (string reference in references)
			{
				this.AddReference(reference);
			}
		}

		// Token: 0x060010C1 RID: 4289 RVA: 0x0005E284 File Offset: 0x0005C684
		public void AddConditionalSymbol(string symbol)
		{
			string text = this.parameters.CompilerOptions;
			if (text == null)
			{
				text = "/define:" + symbol;
			}
			else
			{
				text = text + " /define:" + symbol;
			}
			this.parameters.CompilerOptions = text;
		}

		// Token: 0x060010C2 RID: 4290 RVA: 0x0005E2CD File Offset: 0x0005C6CD
		public void SetSuggestedAssemblyNamePrefix(string suggestedNamePrefix)
		{
			this.parameters.OutputAssembly = suggestedNamePrefix;
		}

		// Token: 0x060010C3 RID: 4291 RVA: 0x0005E2DB File Offset: 0x0005C6DB
		public ScriptCompilerError[] CompileFiles(string[] files)
		{
			return this.CompileShared(new Func<CompilerParameters, string[], CompilerResults>(this.compiler.CompileAssemblyFromFileBatch), this.parameters, files);
		}

		// Token: 0x060010C4 RID: 4292 RVA: 0x0005E2FC File Offset: 0x0005C6FC
		public ScriptCompilerError[] CompileSource(string[] source)
		{
			return this.CompileShared(new Func<CompilerParameters, string[], CompilerResults>(this.compiler.CompileAssemblyFromSourceBatch), this.parameters, source);
		}

		// Token: 0x060010C5 RID: 4293 RVA: 0x0005E320 File Offset: 0x0005C720
		private ScriptCompilerError[] CompileShared(Func<CompilerParameters, string[], CompilerResults> compileMethod, CompilerParameters parameters, string[] sourceOrFiles)
		{
			McsCompiler.OutputDirectory = this.outputDirectory;
			McsCompiler.GenerateSymbols = this.generateSymbols;
			this.assemblyData = null;
			this.symbolsData = null;
			CompilerResults compilerResults = compileMethod(parameters, sourceOrFiles);
			ScriptCompilerError[] array = new ScriptCompilerError[compilerResults.Errors.Count];
			parameters.ReferencedAssemblies.Clear();
			int num = 0;
			IEnumerator enumerator = compilerResults.Errors.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					CompilerError compilerError = (CompilerError)obj;
					array[num] = new ScriptCompilerError
					{
						errorCode = compilerError.ErrorNumber,
						errorText = compilerError.ErrorText,
						fileName = compilerError.FileName,
						line = compilerError.Line,
						column = compilerError.Column,
						isWarning = compilerError.IsWarning
					};
					num++;
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			if (compilerResults.CompiledAssembly != null)
			{
				string text = compilerResults.CompiledAssembly.GetName().Name + ".dll";
				this.assemblyData = File.ReadAllBytes(text);
				File.Delete(text);
				if (this.generateSymbols)
				{
					string path = text + ".mdb";
					if (File.Exists(path))
					{
						this.symbolsData = File.ReadAllBytes(path);
						File.Delete(path);
					}
				}
			}
			return array;
		}

		// Token: 0x04000ED6 RID: 3798
		private ICodeCompiler compiler;

		// Token: 0x04000ED7 RID: 3799
		private CompilerParameters parameters;

		// Token: 0x04000ED8 RID: 3800
		private string outputDirectory = string.Empty;

		// Token: 0x04000ED9 RID: 3801
		private bool generateSymbols = true;

		// Token: 0x04000EDA RID: 3802
		private byte[] assemblyData;

		// Token: 0x04000EDB RID: 3803
		private byte[] symbolsData;
	}
}
