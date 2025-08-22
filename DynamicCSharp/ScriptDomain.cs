using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using DynamicCSharp.Compiler;
using DynamicCSharp.Security;
using UnityEngine;

namespace DynamicCSharp
{
	// Token: 0x020002DB RID: 731
	public class ScriptDomain
	{
		// Token: 0x0600110D RID: 4365 RVA: 0x0005F4C1 File Offset: 0x0005D8C1
		private ScriptDomain(string name)
		{
			ScriptDomain.domain = this;
			this.sandbox = AppDomain.CurrentDomain;
			this.checker = new AssemblyChecker();
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x0600110E RID: 4366 RVA: 0x0005F4E5 File Offset: 0x0005D8E5
		internal static ScriptDomain Active
		{
			get
			{
				return ScriptDomain.domain;
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x0600110F RID: 4367 RVA: 0x0005F4EC File Offset: 0x0005D8EC
		public ScriptCompiler CompilerService
		{
			get
			{
				return this.compilerService;
			}
		}

		// Token: 0x06001110 RID: 4368 RVA: 0x0005F4F4 File Offset: 0x0005D8F4
		public ScriptAssembly LoadAssemblyFromResources(string resourcePath)
		{
			TextAsset textAsset = Resources.Load<TextAsset>(resourcePath);
			if (textAsset == null)
			{
				throw new DllNotFoundException(string.Format("Failed to load dll from resources path '{0}'", resourcePath));
			}
			return this.LoadAssembly(textAsset.bytes, null);
		}

		// Token: 0x06001111 RID: 4369 RVA: 0x0005F534 File Offset: 0x0005D934
		public ScriptAssembly LoadAssembly(string fullPath)
		{
			this.CheckDisposed();
			if (!File.Exists(fullPath))
			{
				throw new DllNotFoundException(string.Format("Failed to load dll at '{0}'", fullPath));
			}
			byte[] data = File.ReadAllBytes(fullPath);
			byte[] symbols = null;
			if (DynamicCSharp.Settings.debugMode && File.Exists(fullPath = ".mdb"))
			{
				symbols = File.ReadAllBytes(fullPath + ".mdb");
			}
			return this.LoadAssembly(data, symbols);
		}

		// Token: 0x06001112 RID: 4370 RVA: 0x0005F5A8 File Offset: 0x0005D9A8
		public ScriptAssembly LoadAssembly(AssemblyName name)
		{
			this.CheckDisposed();
			Assembly rawAssembly = this.sandbox.Load(name);
			return new ScriptAssembly(this, rawAssembly);
		}

		// Token: 0x06001113 RID: 4371 RVA: 0x0005F5D0 File Offset: 0x0005D9D0
		public ScriptAssembly LoadAssembly(byte[] data, byte[] symbols = null)
		{
			this.CheckDisposed();
			if (DynamicCSharp.Settings.securityCheckCode)
			{
				this.SecurityCheckAssembly(data, true);
			}
			Assembly assembly;
			if (symbols == null || !DynamicCSharp.Settings.debugMode)
			{
				assembly = this.sandbox.Load(data);
			}
			else
			{
				assembly = this.sandbox.Load(data, symbols);
			}
			if (assembly == null)
			{
				return null;
			}
			return new ScriptAssembly(this, assembly);
		}

		// Token: 0x06001114 RID: 4372 RVA: 0x0005F644 File Offset: 0x0005DA44
		public bool TryLoadAssembly(string fullPath, out ScriptAssembly result)
		{
			bool result2;
			try
			{
				result = this.LoadAssembly(fullPath);
				result2 = true;
			}
			catch (Exception)
			{
				result = null;
				result2 = false;
			}
			return result2;
		}

		// Token: 0x06001115 RID: 4373 RVA: 0x0005F680 File Offset: 0x0005DA80
		public bool TryLoadAssembly(AssemblyName name, out ScriptAssembly result)
		{
			bool result2;
			try
			{
				result = this.LoadAssembly(name);
				result2 = true;
			}
			catch (Exception)
			{
				result = null;
				result2 = false;
			}
			return result2;
		}

		// Token: 0x06001116 RID: 4374 RVA: 0x0005F6BC File Offset: 0x0005DABC
		public bool TryLoadAssembly(byte[] data, out ScriptAssembly result)
		{
			bool result2;
			try
			{
				result = this.LoadAssembly(data, null);
				result2 = true;
			}
			catch (Exception)
			{
				result = null;
				result2 = false;
			}
			return result2;
		}

		// Token: 0x06001117 RID: 4375 RVA: 0x0005F6F8 File Offset: 0x0005DAF8
		public ScriptType CompileAndLoadScriptFile(string file)
		{
			this.CheckCompiler();
			string[] sourceFiles = new string[]
			{
				file
			};
			if (!this.compilerService.CompileFiles(sourceFiles, DynamicCSharp.Settings.assemblyReferences))
			{
				this.compilerService.PrintErrors();
				return null;
			}
			if (this.compilerService.HasWarnings)
			{
				this.compilerService.PrintWarnings();
			}
			ScriptAssembly scriptAssembly = this.LoadAssembly(this.compilerService.AssemblyData, this.compilerService.SymbolsData);
			if (scriptAssembly == null)
			{
				return null;
			}
			return scriptAssembly.MainType;
		}

		// Token: 0x06001118 RID: 4376 RVA: 0x0005F784 File Offset: 0x0005DB84
		public ScriptAssembly CompileAndLoadScriptFiles(params string[] files)
		{
			this.CheckCompiler();
			if (!this.compilerService.CompileFiles(files, DynamicCSharp.Settings.assemblyReferences))
			{
				this.compilerService.PrintErrors();
				return null;
			}
			if (this.compilerService.HasWarnings)
			{
				this.compilerService.PrintWarnings();
			}
			return this.LoadAssembly(this.compilerService.AssemblyData, this.compilerService.SymbolsData);
		}

		// Token: 0x06001119 RID: 4377 RVA: 0x0005F7F8 File Offset: 0x0005DBF8
		public ScriptType CompileAndLoadScriptSource(string source)
		{
			this.CheckCompiler();
			string[] sourceContent = new string[]
			{
				source
			};
			if (!this.compilerService.CompileSources(sourceContent, DynamicCSharp.Settings.assemblyReferences))
			{
				this.compilerService.PrintErrors();
				return null;
			}
			if (this.compilerService.HasWarnings)
			{
				this.compilerService.PrintWarnings();
			}
			ScriptAssembly scriptAssembly = this.LoadAssembly(this.compilerService.AssemblyData, this.compilerService.SymbolsData);
			if (scriptAssembly == null)
			{
				return null;
			}
			return scriptAssembly.MainType;
		}

		// Token: 0x0600111A RID: 4378 RVA: 0x0005F884 File Offset: 0x0005DC84
		public ScriptAssembly CompileAndLoadScriptSources(params string[] sources)
		{
			this.CheckCompiler();
			if (!this.compilerService.CompileSources(sources, DynamicCSharp.Settings.assemblyReferences))
			{
				this.compilerService.PrintErrors();
				return null;
			}
			if (this.compilerService.HasWarnings)
			{
				this.compilerService.PrintWarnings();
			}
			return this.LoadAssembly(this.compilerService.AssemblyData, this.compilerService.SymbolsData);
		}

		// Token: 0x0600111B RID: 4379 RVA: 0x0005F8F8 File Offset: 0x0005DCF8
		public AsyncCompileLoadOperation CompileAndLoadScriptFilesAsync(params string[] files)
		{
			ScriptDomain.<CompileAndLoadScriptFilesAsync>c__AnonStorey0 <CompileAndLoadScriptFilesAsync>c__AnonStorey = new ScriptDomain.<CompileAndLoadScriptFilesAsync>c__AnonStorey0();
			<CompileAndLoadScriptFilesAsync>c__AnonStorey.files = files;
			<CompileAndLoadScriptFilesAsync>c__AnonStorey.$this = this;
			this.CheckCompiler();
			<CompileAndLoadScriptFilesAsync>c__AnonStorey.references = DynamicCSharp.Settings.assemblyReferences;
			return new AsyncCompileLoadOperation(this, new Func<bool>(<CompileAndLoadScriptFilesAsync>c__AnonStorey.<>m__0));
		}

		// Token: 0x0600111C RID: 4380 RVA: 0x0005F944 File Offset: 0x0005DD44
		public AsyncCompileLoadOperation CompileAndLoadScriptSourcesAsync(params string[] sources)
		{
			ScriptDomain.<CompileAndLoadScriptSourcesAsync>c__AnonStorey1 <CompileAndLoadScriptSourcesAsync>c__AnonStorey = new ScriptDomain.<CompileAndLoadScriptSourcesAsync>c__AnonStorey1();
			<CompileAndLoadScriptSourcesAsync>c__AnonStorey.sources = sources;
			<CompileAndLoadScriptSourcesAsync>c__AnonStorey.$this = this;
			this.CheckCompiler();
			<CompileAndLoadScriptSourcesAsync>c__AnonStorey.references = DynamicCSharp.Settings.assemblyReferences;
			return new AsyncCompileLoadOperation(this, new Func<bool>(<CompileAndLoadScriptSourcesAsync>c__AnonStorey.<>m__0));
		}

		// Token: 0x0600111D RID: 4381 RVA: 0x0005F990 File Offset: 0x0005DD90
		public bool SecurityCheckAssembly(string fullpath, bool throwOnError = false)
		{
			try
			{
				byte[] assemblyData = new byte[0];
				using (BinaryReader binaryReader = new BinaryReader(File.Open(fullpath, FileMode.Open)))
				{
					assemblyData = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);
				}
				this.checker.SecurityCheckAssembly(assemblyData);
				if (this.checker.HasErrors)
				{
					throw new SecurityException(this.checker.Errors[0].ToString());
				}
			}
			catch (Exception)
			{
				if (throwOnError)
				{
					throw;
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600111E RID: 4382 RVA: 0x0005FA48 File Offset: 0x0005DE48
		public bool SecurityCheckAssembly(AssemblyName name, bool throwOnError = false)
		{
			try
			{
				byte[] assemblyData = new byte[0];
				using (BinaryReader binaryReader = new BinaryReader(File.Open(name.CodeBase, FileMode.Open)))
				{
					assemblyData = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);
				}
				this.checker.SecurityCheckAssembly(assemblyData);
				if (this.checker.HasErrors)
				{
					throw new SecurityException(this.checker.Errors[0].ToString());
				}
			}
			catch (Exception)
			{
				if (throwOnError)
				{
					throw;
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600111F RID: 4383 RVA: 0x0005FB08 File Offset: 0x0005DF08
		public bool SecurityCheckAssembly(byte[] assemblyData, bool throwOnError = false)
		{
			try
			{
				if (assemblyData == null)
				{
					throw new ArgumentNullException("assemblyData");
				}
				this.checker.SecurityCheckAssembly(assemblyData);
				if (this.checker.HasErrors)
				{
					throw new SecurityException(this.checker.Errors[0].ToString());
				}
			}
			catch (Exception)
			{
				if (throwOnError)
				{
					throw;
				}
				return false;
			}
			return true;
		}

		// Token: 0x06001120 RID: 4384 RVA: 0x0005FB8C File Offset: 0x0005DF8C
		internal void CreateCompilerService()
		{
			this.compilerService = new ScriptCompiler();
		}

		// Token: 0x06001121 RID: 4385 RVA: 0x0005FB99 File Offset: 0x0005DF99
		private void CheckDisposed()
		{
			if (this.sandbox == null)
			{
				throw new ObjectDisposedException("The 'ScriptDomain' has already been disposed");
			}
		}

		// Token: 0x06001122 RID: 4386 RVA: 0x0005FBB1 File Offset: 0x0005DFB1
		private void CheckCompiler()
		{
			if (this.compilerService == null)
			{
				throw new Exception("The compiler service has not been initialized");
			}
		}

		// Token: 0x06001123 RID: 4387 RVA: 0x0005FBCC File Offset: 0x0005DFCC
		public static ScriptDomain CreateDomain(string domainName, bool initCompiler)
		{
			ScriptDomain scriptDomain = new ScriptDomain("DynamicCSharp");
			if (initCompiler)
			{
				scriptDomain.CreateCompilerService();
			}
			return scriptDomain;
		}

		// Token: 0x06001124 RID: 4388 RVA: 0x0005FBF1 File Offset: 0x0005DFF1
		// Note: this type is marked as 'beforefieldinit'.
		static ScriptDomain()
		{
		}

		// Token: 0x04000F04 RID: 3844
		private static ScriptDomain domain;

		// Token: 0x04000F05 RID: 3845
		private AppDomain sandbox;

		// Token: 0x04000F06 RID: 3846
		private AssemblyChecker checker;

		// Token: 0x04000F07 RID: 3847
		private ScriptCompiler compilerService;

		// Token: 0x02000EED RID: 3821
		[CompilerGenerated]
		private sealed class <CompileAndLoadScriptFilesAsync>c__AnonStorey0
		{
			// Token: 0x06007243 RID: 29251 RVA: 0x0005FBF3 File Offset: 0x0005DFF3
			public <CompileAndLoadScriptFilesAsync>c__AnonStorey0()
			{
			}

			// Token: 0x06007244 RID: 29252 RVA: 0x0005FBFC File Offset: 0x0005DFFC
			internal bool <>m__0()
			{
				bool result = this.$this.compilerService.CompileFiles(this.files, this.references);
				if (this.$this.compilerService.HasErrors)
				{
					this.$this.compilerService.PrintErrors();
				}
				if (this.$this.compilerService.HasWarnings)
				{
					this.$this.compilerService.PrintWarnings();
				}
				return result;
			}

			// Token: 0x04006624 RID: 26148
			internal string[] files;

			// Token: 0x04006625 RID: 26149
			internal string[] references;

			// Token: 0x04006626 RID: 26150
			internal ScriptDomain $this;
		}

		// Token: 0x02000EEE RID: 3822
		[CompilerGenerated]
		private sealed class <CompileAndLoadScriptSourcesAsync>c__AnonStorey1
		{
			// Token: 0x06007245 RID: 29253 RVA: 0x0005FC71 File Offset: 0x0005E071
			public <CompileAndLoadScriptSourcesAsync>c__AnonStorey1()
			{
			}

			// Token: 0x06007246 RID: 29254 RVA: 0x0005FC7C File Offset: 0x0005E07C
			internal bool <>m__0()
			{
				bool result = this.$this.compilerService.CompileSources(this.sources, this.references);
				if (this.$this.compilerService.HasErrors)
				{
					this.$this.compilerService.PrintErrors();
				}
				if (this.$this.compilerService.HasWarnings)
				{
					this.$this.compilerService.PrintWarnings();
				}
				return result;
			}

			// Token: 0x04006627 RID: 26151
			internal string[] sources;

			// Token: 0x04006628 RID: 26152
			internal string[] references;

			// Token: 0x04006629 RID: 26153
			internal ScriptDomain $this;
		}
	}
}
