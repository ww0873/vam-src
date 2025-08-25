using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using Mono.CSharp;

namespace DynamicCSharp.Compiler
{
	// Token: 0x020002D0 RID: 720
	internal sealed class McsCompiler : ICodeCompiler
	{
		// Token: 0x060010A1 RID: 4257 RVA: 0x0005D5BE File Offset: 0x0005B9BE
		public McsCompiler()
		{
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x060010A2 RID: 4258 RVA: 0x0005D5C6 File Offset: 0x0005B9C6
		// (set) Token: 0x060010A3 RID: 4259 RVA: 0x0005D5CD File Offset: 0x0005B9CD
		internal static string OutputDirectory
		{
			get
			{
				return McsCompiler.outputDirectory;
			}
			set
			{
				if (value != string.Empty && !Directory.Exists(value))
				{
					throw new IOException("The specified directory path does not exist. Make sure the specified directory path exists before setting this property");
				}
				McsCompiler.outputDirectory = value;
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x060010A4 RID: 4260 RVA: 0x0005D5FB File Offset: 0x0005B9FB
		// (set) Token: 0x060010A5 RID: 4261 RVA: 0x0005D602 File Offset: 0x0005BA02
		internal static bool GenerateSymbols
		{
			get
			{
				return McsCompiler.generateSymbols;
			}
			set
			{
				McsCompiler.generateSymbols = value;
			}
		}

		// Token: 0x060010A6 RID: 4262 RVA: 0x0005D60A File Offset: 0x0005BA0A
		public CompilerResults CompileAssemblyFromDom(CompilerParameters options, CodeCompileUnit compilationUnit)
		{
			return this.CompileAssemblyFromDomBatch(options, new CodeCompileUnit[]
			{
				compilationUnit
			});
		}

		// Token: 0x060010A7 RID: 4263 RVA: 0x0005D620 File Offset: 0x0005BA20
		public CompilerResults CompileAssemblyFromDomBatch(CompilerParameters options, CodeCompileUnit[] compilationUnits)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			CompilerResults result;
			try
			{
				result = this.CompileFromDomBatch(options, compilationUnits);
			}
			finally
			{
				options.TempFiles.Delete();
			}
			return result;
		}

		// Token: 0x060010A8 RID: 4264 RVA: 0x0005D66C File Offset: 0x0005BA6C
		public CompilerResults CompileAssemblyFromFile(CompilerParameters options, string fileName)
		{
			return this.CompileAssemblyFromFileBatch(options, new string[]
			{
				fileName
			});
		}

		// Token: 0x060010A9 RID: 4265 RVA: 0x0005D680 File Offset: 0x0005BA80
		public CompilerResults CompileAssemblyFromFileBatch(CompilerParameters options, string[] fileNames)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			CompilerSettings settings = this.GetSettings(options);
			foreach (string text in fileNames)
			{
				string fullPath = Path.GetFullPath(text);
				SourceFile item = new SourceFile(text, fullPath, settings.SourceFiles.Count + 1, null);
				settings.SourceFiles.Add(item);
			}
			return this.CompileFromSettings(settings, options.GenerateInMemory);
		}

		// Token: 0x060010AA RID: 4266 RVA: 0x0005D6FA File Offset: 0x0005BAFA
		public CompilerResults CompileAssemblyFromSource(CompilerParameters options, string source)
		{
			return this.CompileAssemblyFromSourceBatch(options, new string[]
			{
				source
			});
		}

		// Token: 0x060010AB RID: 4267 RVA: 0x0005D710 File Offset: 0x0005BB10
		public CompilerResults CompileAssemblyFromSourceBatch(CompilerParameters options, string[] sources)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			CompilerSettings settings = this.GetSettings(options);
			for (int i = 0; i < sources.Length; i++)
			{
				McsCompiler.<CompileAssemblyFromSourceBatch>c__AnonStorey0 <CompileAssemblyFromSourceBatch>c__AnonStorey = new McsCompiler.<CompileAssemblyFromSourceBatch>c__AnonStorey0();
				<CompileAssemblyFromSourceBatch>c__AnonStorey.source = sources[i];
				Func<Stream> streamIfDynamicFile = new Func<Stream>(<CompileAssemblyFromSourceBatch>c__AnonStorey.<>m__0);
				SourceFile item = new SourceFile(string.Empty, string.Empty, settings.SourceFiles.Count + 1, streamIfDynamicFile);
				settings.SourceFiles.Add(item);
			}
			return this.CompileFromSettings(settings, options.GenerateInMemory);
		}

		// Token: 0x060010AC RID: 4268 RVA: 0x0005D79F File Offset: 0x0005BB9F
		private CompilerResults CompileFromDomBatch(CompilerParameters options, CodeCompileUnit[] compilationUnits)
		{
			throw new NotImplementedException("Use compile from source or file!");
		}

		// Token: 0x060010AD RID: 4269 RVA: 0x0005D7AC File Offset: 0x0005BBAC
		private CompilerResults CompileFromSettings(CompilerSettings settings, bool generateInMemory)
		{
			CompilerResults compilerResults = new CompilerResults(new TempFileCollection(Path.GetTempPath()));
			McsDriver mcsDriver = new McsDriver(new CompilerContext(settings, new McsReporter(compilerResults)));
			AssemblyBuilder compiledAssembly = null;
			try
			{
				mcsDriver.Compile(out compiledAssembly, AppDomain.CurrentDomain, generateInMemory);
			}
			catch (Exception ex)
			{
				compilerResults.Errors.Add(new CompilerError
				{
					IsWarning = false,
					ErrorText = ex.ToString()
				});
			}
			compilerResults.CompiledAssembly = compiledAssembly;
			return compilerResults;
		}

		// Token: 0x060010AE RID: 4270 RVA: 0x0005D83C File Offset: 0x0005BC3C
		private void SetTargetEnumField(FieldInfo field, object instance, MCSTarget target)
		{
			try
			{
				field.SetValue(instance, (int)target);
			}
			catch
			{
			}
		}

		// Token: 0x060010AF RID: 4271 RVA: 0x0005D874 File Offset: 0x0005BC74
		private CompilerSettings GetSettings(CompilerParameters parameters)
		{
			CompilerSettings compilerSettings = new CompilerSettings();
			StringEnumerator enumerator = parameters.ReferencedAssemblies.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					string item = enumerator.Current;
					compilerSettings.AssemblyReferences.Add(item);
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
			compilerSettings.Encoding = Encoding.UTF8;
			compilerSettings.GenerateDebugInfo = parameters.IncludeDebugInformation;
			compilerSettings.MainClass = parameters.MainClass;
			compilerSettings.Platform = Platform.AnyCPU;
			compilerSettings.StdLibRuntimeVersion = RuntimeVersion.v4;
			FieldInfo field = typeof(CompilerSettings).GetField("Target");
			if (parameters.GenerateExecutable)
			{
				this.SetTargetEnumField(field, compilerSettings, MCSTarget.Exe);
				compilerSettings.TargetExt = ".exe";
			}
			else
			{
				this.SetTargetEnumField(field, compilerSettings, MCSTarget.Library);
				compilerSettings.TargetExt = ".dll";
			}
			if (parameters.GenerateInMemory)
			{
				this.SetTargetEnumField(field, compilerSettings, MCSTarget.Library);
			}
			if (parameters.OutputAssembly != null && !parameters.OutputAssembly.StartsWith("DynamicAssembly_"))
			{
				parameters.OutputAssembly = (compilerSettings.OutputFile = Path.Combine(McsCompiler.outputDirectory, string.Concat(new object[]
				{
					parameters.OutputAssembly,
					"_",
					McsCompiler.assemblyCounter,
					compilerSettings.TargetExt
				})));
			}
			else
			{
				parameters.OutputAssembly = (compilerSettings.OutputFile = Path.Combine(McsCompiler.outputDirectory, "DynamicAssembly_" + McsCompiler.assemblyCounter + compilerSettings.TargetExt));
			}
			McsCompiler.assemblyCounter += 1L;
			compilerSettings.OutputFile = parameters.OutputAssembly;
			compilerSettings.GenerateDebugInfo = McsCompiler.generateSymbols;
			compilerSettings.Version = LanguageVersion.V_6;
			compilerSettings.WarningLevel = parameters.WarningLevel;
			compilerSettings.WarningsAreErrors = parameters.TreatWarningsAsErrors;
			compilerSettings.Optimize = false;
			if (parameters.CompilerOptions != null)
			{
				string[] array = parameters.CompilerOptions.Split(new char[]
				{
					' '
				});
				foreach (string text in array)
				{
					if (text.StartsWith("/define:"))
					{
						compilerSettings.AddConditionalSymbol(text.Remove(0, 8));
					}
				}
			}
			return compilerSettings;
		}

		// Token: 0x060010B0 RID: 4272 RVA: 0x0005DAD0 File Offset: 0x0005BED0
		// Note: this type is marked as 'beforefieldinit'.
		static McsCompiler()
		{
		}

		// Token: 0x04000ED2 RID: 3794
		private static string outputDirectory = string.Empty;

		// Token: 0x04000ED3 RID: 3795
		private static bool generateSymbols;

		// Token: 0x04000ED4 RID: 3796
		private static long assemblyCounter;

		// Token: 0x02000EE9 RID: 3817
		[CompilerGenerated]
		private sealed class <CompileAssemblyFromSourceBatch>c__AnonStorey0
		{
			// Token: 0x06007235 RID: 29237 RVA: 0x0005DADC File Offset: 0x0005BEDC
			public <CompileAssemblyFromSourceBatch>c__AnonStorey0()
			{
			}

			// Token: 0x06007236 RID: 29238 RVA: 0x0005DAE4 File Offset: 0x0005BEE4
			internal Stream <>m__0()
			{
				string s = (!string.IsNullOrEmpty(this.source)) ? this.source : string.Empty;
				return new MemoryStream(Encoding.UTF8.GetBytes(s));
			}

			// Token: 0x0400660E RID: 26126
			internal string source;
		}
	}
}
