using System;
using System.Collections.Generic;

namespace DynamicCSharp.Compiler
{
	// Token: 0x020002D5 RID: 725
	internal interface ICompiler
	{
		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x060010CA RID: 4298
		// (set) Token: 0x060010CB RID: 4299
		string OutputDirectory { get; set; }

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x060010CC RID: 4300
		// (set) Token: 0x060010CD RID: 4301
		bool GenerateSymbols { get; set; }

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x060010CE RID: 4302
		byte[] AssemblyData { get; }

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x060010CF RID: 4303
		byte[] SymbolsData { get; }

		// Token: 0x060010D0 RID: 4304
		void AddReference(string reference);

		// Token: 0x060010D1 RID: 4305
		void AddReferences(IEnumerable<string> references);

		// Token: 0x060010D2 RID: 4306
		void AddConditionalSymbol(string symbol);

		// Token: 0x060010D3 RID: 4307
		void SetSuggestedAssemblyNamePrefix(string assemblyNamePrefix);

		// Token: 0x060010D4 RID: 4308
		ScriptCompilerError[] CompileFiles(string[] source);

		// Token: 0x060010D5 RID: 4309
		ScriptCompilerError[] CompileSource(string[] source);
	}
}
