using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Emit;
using System.Security.Cryptography;
using Mono.CSharp;

namespace DynamicCSharp.Compiler
{
	// Token: 0x020002D1 RID: 721
	internal sealed class McsDriver
	{
		// Token: 0x060010B1 RID: 4273 RVA: 0x0005DB22 File Offset: 0x0005BF22
		public McsDriver(CompilerContext context)
		{
			this.context = context;
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x060010B2 RID: 4274 RVA: 0x0005DB31 File Offset: 0x0005BF31
		public Report Report
		{
			get
			{
				return this.context.Report;
			}
		}

		// Token: 0x060010B3 RID: 4275 RVA: 0x0005DB40 File Offset: 0x0005BF40
		public void TokenizeFile(SourceFile source, ModuleContainer module, ParserSession session)
		{
			Stream stream = null;
			try
			{
				stream = source.GetDataStream();
			}
			catch
			{
				this.Report.Error(2001, "Failed to open file '{0}' for reading", source.Name);
				return;
			}
			using (stream)
			{
				using (SeekableStreamReader seekableStreamReader = new SeekableStreamReader(stream, this.context.Settings.Encoding, null))
				{
					CompilationSourceFile file = new CompilationSourceFile(module, source);
					Tokenizer tokenizer = new Tokenizer(seekableStreamReader, file, session, this.context.Report);
					int num = 0;
					int num2 = 0;
					int num3;
					while ((num3 = tokenizer.token()) != 257)
					{
						num++;
						if (num3 == 259)
						{
							num2++;
						}
					}
				}
			}
		}

		// Token: 0x060010B4 RID: 4276 RVA: 0x0005DC3C File Offset: 0x0005C03C
		public void Parse(ModuleContainer module)
		{
			bool tokenizeOnly = module.Compiler.Settings.TokenizeOnly;
			List<SourceFile> sourceFiles = module.Compiler.SourceFiles;
			Location.Initialize(sourceFiles);
			ParserSession session = new ParserSession
			{
				UseJayGlobalArrays = true,
				LocatedTokens = new LocatedToken[15000]
			};
			for (int i = 0; i < sourceFiles.Count; i++)
			{
				if (tokenizeOnly)
				{
					this.TokenizeFile(sourceFiles[i], module, session);
				}
				else
				{
					this.Parse(sourceFiles[i], module, session, this.Report);
				}
			}
		}

		// Token: 0x060010B5 RID: 4277 RVA: 0x0005DCD8 File Offset: 0x0005C0D8
		public void Parse(SourceFile source, ModuleContainer module, ParserSession session, Report report)
		{
			Stream stream = null;
			try
			{
				stream = source.GetDataStream();
			}
			catch
			{
				this.Report.Error(2001, "Failed to open file '{0}' for reading", source.Name);
				return;
			}
			using (stream)
			{
				if (stream.ReadByte() == 77 && stream.ReadByte() == 90)
				{
					report.Error(2015, "Failed to open file '{0}' for reading because it is a binary file. A text file was expected", source.Name);
					stream.Close();
				}
				else
				{
					stream.Position = 0L;
					using (SeekableStreamReader seekableStreamReader = new SeekableStreamReader(stream, this.context.Settings.Encoding, session.StreamReaderBuffer))
					{
						McsDriver.Parse(seekableStreamReader, source, module, session, report);
						if (this.context.Settings.GenerateDebugInfo && report.Errors == 0 && !source.HasChecksum)
						{
							stream.Position = 0L;
							MD5 checksumAlgorithm = session.GetChecksumAlgorithm();
							source.SetChecksum(checksumAlgorithm.ComputeHash(stream));
						}
					}
				}
			}
		}

		// Token: 0x060010B6 RID: 4278 RVA: 0x0005DE18 File Offset: 0x0005C218
		public bool Compile(out AssemblyBuilder assembly, AppDomain domain, bool generateInMemory)
		{
			CompilerSettings settings = this.context.Settings;
			assembly = null;
			if (settings.FirstSourceFile == null && (settings.Target == Target.Exe || settings.Target == Target.WinExe || settings.Target == Target.Module || settings.Resources == null))
			{
				this.Report.Error(2008, "No source files specified");
				return false;
			}
			if (settings.Platform == Platform.AnyCPU32Preferred && (settings.Target == Target.Library || settings.Target == Target.Module))
			{
				this.Report.Error(4023, "The preferred platform '{0}' is only valid on executable outputs", Platform.AnyCPU32Preferred.ToString());
				return false;
			}
			TimeReporter timeReporter = new TimeReporter(settings.Timestamps);
			this.context.TimeReporter = timeReporter;
			timeReporter.StartTotal();
			ModuleContainer moduleContainer = new ModuleContainer(this.context);
			RootContext.ToplevelTypes = moduleContainer;
			timeReporter.Start(TimeReporter.TimerType.ParseTotal);
			this.Parse(moduleContainer);
			timeReporter.Stop(TimeReporter.TimerType.ParseTotal);
			if (this.Report.Errors > 0)
			{
				return false;
			}
			if (settings.TokenizeOnly || settings.ParseOnly)
			{
				timeReporter.StopTotal();
				timeReporter.ShowStats();
				return true;
			}
			string outputFile = settings.OutputFile;
			string fileName = Path.GetFileName(outputFile);
			AssemblyDefinitionDynamic assemblyDefinitionDynamic = new AssemblyDefinitionDynamic(moduleContainer, fileName, outputFile);
			moduleContainer.SetDeclaringAssembly(assemblyDefinitionDynamic);
			ReflectionImporter importer = new ReflectionImporter(moduleContainer, this.context.BuiltinTypes);
			assemblyDefinitionDynamic.Importer = importer;
			DynamicLoader dynamicLoader = new DynamicLoader(importer, this.context);
			dynamicLoader.LoadReferences(moduleContainer);
			if (!this.context.BuiltinTypes.CheckDefinitions(moduleContainer))
			{
				return false;
			}
			if (!assemblyDefinitionDynamic.Create(domain, AssemblyBuilderAccess.RunAndSave))
			{
				return false;
			}
			moduleContainer.CreateContainer();
			dynamicLoader.LoadModules(assemblyDefinitionDynamic, moduleContainer.GlobalRootNamespace);
			moduleContainer.InitializePredefinedTypes();
			if (settings.GetResourceStrings != null)
			{
				moduleContainer.LoadGetResourceStrings(settings.GetResourceStrings);
			}
			timeReporter.Start(TimeReporter.TimerType.ModuleDefinitionTotal);
			try
			{
				moduleContainer.Define();
			}
			catch
			{
				return false;
			}
			timeReporter.Stop(TimeReporter.TimerType.ModuleDefinitionTotal);
			if (this.Report.Errors > 0)
			{
				return false;
			}
			if (settings.DocumentationFile != null)
			{
				DocumentationBuilder documentationBuilder = new DocumentationBuilder(moduleContainer);
				documentationBuilder.OutputDocComment(outputFile, settings.DocumentationFile);
			}
			assemblyDefinitionDynamic.Resolve();
			if (this.Report.Errors > 0)
			{
				return false;
			}
			timeReporter.Start(TimeReporter.TimerType.EmitTotal);
			assemblyDefinitionDynamic.Emit();
			timeReporter.Stop(TimeReporter.TimerType.EmitTotal);
			if (this.Report.Errors > 0)
			{
				return false;
			}
			timeReporter.Start(TimeReporter.TimerType.CloseTypes);
			moduleContainer.CloseContainer();
			timeReporter.Stop(TimeReporter.TimerType.CloseTypes);
			timeReporter.Start(TimeReporter.TimerType.Resouces);
			if (!settings.WriteMetadataOnly)
			{
				assemblyDefinitionDynamic.EmbedResources();
			}
			timeReporter.Stop(TimeReporter.TimerType.Resouces);
			if (this.Report.Errors > 0)
			{
				return false;
			}
			if (!generateInMemory)
			{
				assemblyDefinitionDynamic.Save();
			}
			assembly = assemblyDefinitionDynamic.Builder;
			timeReporter.StopTotal();
			timeReporter.ShowStats();
			return this.Report.Errors == 0;
		}

		// Token: 0x060010B7 RID: 4279 RVA: 0x0005E124 File Offset: 0x0005C524
		public static void Parse(SeekableStreamReader reader, SourceFile source, ModuleContainer module, ParserSession session, Report report)
		{
			CompilationSourceFile compilationSourceFile = new CompilationSourceFile(module, source);
			module.AddTypeContainer(compilationSourceFile);
			CSharpParser csharpParser = new CSharpParser(reader, compilationSourceFile, report, session);
			csharpParser.parse();
		}

		// Token: 0x04000ED5 RID: 3797
		private readonly CompilerContext context;
	}
}
