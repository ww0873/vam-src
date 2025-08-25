using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace DynamicCSharp
{
	// Token: 0x020002DC RID: 732
	public sealed class ScriptEvaluator
	{
		// Token: 0x06001125 RID: 4389 RVA: 0x0005FCF4 File Offset: 0x0005E0F4
		public ScriptEvaluator(ScriptDomain domain = null)
		{
			if (domain == null)
			{
				domain = ScriptDomain.Active;
			}
			if (domain == null)
			{
				throw new ArgumentNullException("The specified domain was null and there are no active domains");
			}
			if (domain.CompilerService == null)
			{
				throw new ArgumentException("The specified domain does not have a compiler service registered. The compiler service is required by a ScriptEvaluator");
			}
			this.domain = domain;
		}

		// Token: 0x06001126 RID: 4390 RVA: 0x0005FD63 File Offset: 0x0005E163
		public Variable BindVar(string name, object value = null)
		{
			return this.BindVar<object>(name, value);
		}

		// Token: 0x06001127 RID: 4391 RVA: 0x0005FD70 File Offset: 0x0005E170
		public Variable<T> BindVar<T>(string name, T value = default(T))
		{
			foreach (Variable variable in this.bindingVariables)
			{
				if (variable.Name == name)
				{
					variable.Update(value);
					return variable as Variable<T>;
				}
			}
			Variable<T> variable2 = new Variable<T>(name, value);
			this.bindingVariables.Add(variable2);
			if (variable2.Value != null)
			{
				T value2 = variable2.Value;
				Type type = value2.GetType();
				this.AddUsing(type.Namespace);
			}
			return variable2;
		}

		// Token: 0x06001128 RID: 4392 RVA: 0x0005FE3C File Offset: 0x0005E23C
		public void BindDelegate(string name, Action action)
		{
			if (this.bindingDelegates.ContainsKey(name))
			{
				this.bindingDelegates[name] = action;
				return;
			}
			this.bindingDelegates.Add(name, action);
		}

		// Token: 0x06001129 RID: 4393 RVA: 0x0005FE6A File Offset: 0x0005E26A
		public void BindDelegate<T>(string name, Action<T> action)
		{
			if (this.bindingDelegates.ContainsKey(name))
			{
				this.bindingDelegates[name] = action;
				return;
			}
			this.bindingDelegates.Add(name, action);
		}

		// Token: 0x0600112A RID: 4394 RVA: 0x0005FE98 File Offset: 0x0005E298
		public void BindDelegate<R>(string name, Func<R> func)
		{
			if (this.bindingDelegates.ContainsKey(name))
			{
				this.bindingDelegates[name] = func;
				return;
			}
			this.bindingDelegates.Add(name, func);
		}

		// Token: 0x0600112B RID: 4395 RVA: 0x0005FEC6 File Offset: 0x0005E2C6
		public void BindDelegate<R, T>(string name, Func<T, R> func)
		{
			if (this.bindingDelegates.ContainsKey(name))
			{
				this.bindingDelegates[name] = func;
				return;
			}
			this.bindingDelegates.Add(name, func);
		}

		// Token: 0x0600112C RID: 4396 RVA: 0x0005FEF4 File Offset: 0x0005E2F4
		public void ClearVarBindings()
		{
			this.bindingVariables.Clear();
		}

		// Token: 0x0600112D RID: 4397 RVA: 0x0005FF01 File Offset: 0x0005E301
		public void ClearDelegateBindings()
		{
			this.bindingDelegates.Clear();
		}

		// Token: 0x0600112E RID: 4398 RVA: 0x0005FF0E File Offset: 0x0005E30E
		public void AddUsing(string namespaceName)
		{
			if (!this.usingStatements.Contains(namespaceName))
			{
				this.usingStatements.Add(namespaceName);
			}
		}

		// Token: 0x0600112F RID: 4399 RVA: 0x0005FF2D File Offset: 0x0005E32D
		public Variable Eval(string sourceCode)
		{
			return this.Eval<object>(sourceCode);
		}

		// Token: 0x06001130 RID: 4400 RVA: 0x0005FF38 File Offset: 0x0005E338
		public Variable<T> Eval<T>(string sourceCode)
		{
			bool flag = ScriptEvaluator.evalCache.ContainsKey(sourceCode);
			ScriptProxy scriptProxy;
			if (flag)
			{
				scriptProxy = ScriptEvaluator.evalCache[sourceCode];
			}
			else
			{
				string source = this.BuildSourceAroundTemplate(sourceCode);
				ScriptType scriptType = this.domain.CompileAndLoadScriptSource(source);
				if (scriptType == null)
				{
					return null;
				}
				scriptProxy = scriptType.CreateInstance(null);
				if (scriptProxy == null)
				{
					return null;
				}
			}
			if (!flag)
			{
				ScriptEvaluator.evalCache.Add(sourceCode, scriptProxy);
			}
			this.BindProxyDelegates(scriptProxy);
			this.BindProxyVars(scriptProxy);
			object obj = new object();
			scriptProxy.Fields["_returnVal"] = obj;
			object obj2 = scriptProxy.SafeCall("_EvalEntry");
			this.UnbindProxyVars(scriptProxy);
			if (obj == obj2)
			{
				return new Variable<T>("_returnVal", default(T));
			}
			T data = default(T);
			try
			{
				data = (T)((object)obj2);
			}
			catch (InvalidCastException)
			{
			}
			return new Variable<T>("_returnVal", data);
		}

		// Token: 0x06001131 RID: 4401 RVA: 0x00060040 File Offset: 0x0005E440
		private void BindProxyVars(ScriptProxy proxy)
		{
			foreach (Variable variable in this.bindingVariables)
			{
				proxy.Fields[variable.Name] = variable.Value;
			}
		}

		// Token: 0x06001132 RID: 4402 RVA: 0x000600AC File Offset: 0x0005E4AC
		private void UnbindProxyVars(ScriptProxy proxy)
		{
			foreach (Variable variable in this.bindingVariables)
			{
				variable.Update(proxy.Fields[variable.Name]);
			}
		}

		// Token: 0x06001133 RID: 4403 RVA: 0x00060118 File Offset: 0x0005E518
		private void BindProxyDelegates(ScriptProxy proxy)
		{
			List<string> list = new List<string>(this.bindingDelegates.Keys);
			foreach (string text in list)
			{
				Delegate value = this.bindingDelegates[text];
				proxy.Fields[text] = value;
			}
		}

		// Token: 0x06001134 RID: 4404 RVA: 0x00060194 File Offset: 0x0005E594
		private string BuildSourceAroundTemplate(string source)
		{
			string text = this.GetTemplateSource();
			text = text.Replace("[TAG_USINGSTATEMENTS]", this.GetUsingStatementsSource());
			text = text.Replace("[TAG_DELEGATESTATEMENTS]", this.GetDelegateStatementsSource());
			text = text.Replace("[TAG_FIELDSTATEMENTS]", this.GetFieldStatementsSource());
			text = text.Replace("[TAG_CLASSNAME]", "_EvalClass" + Guid.NewGuid().ToString("N"));
			text = text.Replace("[TAG_METHODNAME]", "_EvalEntry");
			return text.Replace("[TAG_METHODBODY]", source);
		}

		// Token: 0x06001135 RID: 4405 RVA: 0x00060225 File Offset: 0x0005E625
		private string GetTemplateSource()
		{
			if (this.templateSource == null)
			{
				this.templateSource = Resources.Load<TextAsset>("DynamicCSharp_EvalTemplate");
			}
			return this.templateSource.text;
		}

		// Token: 0x06001136 RID: 4406 RVA: 0x00060254 File Offset: 0x0005E654
		private string GetUsingStatementsSource()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string value in this.usingStatements)
			{
				stringBuilder.Append("using");
				stringBuilder.Append(" ");
				stringBuilder.Append(value);
				stringBuilder.Append(";");
				stringBuilder.AppendLine();
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06001137 RID: 4407 RVA: 0x000602E8 File Offset: 0x0005E6E8
		private string GetDelegateStatementsSource()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<string, Delegate> keyValuePair in this.bindingDelegates)
			{
				Delegate value = keyValuePair.Value;
				MethodInfo method = value.Method;
				ParameterInfo[] parameters = method.GetParameters();
				Type returnType = method.ReturnType;
				Type[] array = new Type[parameters.Length];
				for (int i = 0; i < parameters.Length; i++)
				{
					array[i] = parameters[i].ParameterType;
				}
				if (returnType == typeof(void))
				{
					stringBuilder.Append(typeof(Action).FullName);
					if (array.Length > 0)
					{
						stringBuilder.Append("<");
						for (int j = 0; j < array.Length; j++)
						{
							string fullName = array[j].FullName;
							stringBuilder.Append(fullName);
							if (j < array.Length - 1)
							{
								stringBuilder.Append(",");
							}
						}
						stringBuilder.Append(">");
					}
					stringBuilder.Append(" ");
					stringBuilder.Append(keyValuePair.Key);
					stringBuilder.Append(";");
					stringBuilder.AppendLine();
				}
				else
				{
					stringBuilder.Append(typeof(Func<>).FullName.Replace("`1", string.Empty));
					stringBuilder.Append("<");
					if (array.Length > 0)
					{
						for (int k = 0; k < array.Length; k++)
						{
							string fullName2 = array[k].FullName;
							stringBuilder.Append(fullName2);
							stringBuilder.Append(",");
						}
					}
					string fullName3 = returnType.FullName;
					stringBuilder.Append(fullName3);
					stringBuilder.Append(">");
					stringBuilder.Append(" ");
					stringBuilder.Append(keyValuePair.Key);
					stringBuilder.Append(";");
					stringBuilder.AppendLine();
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06001138 RID: 4408 RVA: 0x0006052C File Offset: 0x0005E92C
		private string GetFieldStatementsSource()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Variable variable in this.bindingVariables)
			{
				object value = variable.Value;
				Type type = (value != null) ? value.GetType() : typeof(object);
				string fullName = type.FullName;
				stringBuilder.Append(fullName);
				stringBuilder.Append(" ");
				stringBuilder.Append(variable.Name);
				stringBuilder.Append(";");
				stringBuilder.AppendLine();
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06001139 RID: 4409 RVA: 0x000605F0 File Offset: 0x0005E9F0
		public static void ClearCache()
		{
			ScriptEvaluator.evalCache.Clear();
		}

		// Token: 0x0600113A RID: 4410 RVA: 0x000605FC File Offset: 0x0005E9FC
		// Note: this type is marked as 'beforefieldinit'.
		static ScriptEvaluator()
		{
		}

		// Token: 0x04000F08 RID: 3848
		private static Dictionary<string, ScriptProxy> evalCache = new Dictionary<string, ScriptProxy>();

		// Token: 0x04000F09 RID: 3849
		private ScriptDomain domain;

		// Token: 0x04000F0A RID: 3850
		private TextAsset templateSource;

		// Token: 0x04000F0B RID: 3851
		private Dictionary<string, Delegate> bindingDelegates = new Dictionary<string, Delegate>();

		// Token: 0x04000F0C RID: 3852
		private List<Variable> bindingVariables = new List<Variable>();

		// Token: 0x04000F0D RID: 3853
		private List<string> usingStatements = new List<string>();

		// Token: 0x04000F0E RID: 3854
		private const string templateResource = "DynamicCSharp_EvalTemplate";

		// Token: 0x04000F0F RID: 3855
		private const string entryClass = "_EvalClass";

		// Token: 0x04000F10 RID: 3856
		private const string entryMethod = "_EvalEntry";

		// Token: 0x04000F11 RID: 3857
		private const string returnObject = "_returnVal";

		// Token: 0x04000F12 RID: 3858
		private const string tagUsingStatements = "[TAG_USINGSTATEMENTS]";

		// Token: 0x04000F13 RID: 3859
		private const string tagClassName = "[TAG_CLASSNAME]";

		// Token: 0x04000F14 RID: 3860
		private const string tagFieldStatements = "[TAG_FIELDSTATEMENTS]";

		// Token: 0x04000F15 RID: 3861
		private const string tagDelegateStatements = "[TAG_DELEGATESTATEMENTS]";

		// Token: 0x04000F16 RID: 3862
		private const string tagMethodName = "[TAG_METHODNAME]";

		// Token: 0x04000F17 RID: 3863
		private const string tagMethodBody = "[TAG_METHODBODY]";

		// Token: 0x04000F18 RID: 3864
		private const string tagUsing = "using";

		// Token: 0x04000F19 RID: 3865
		private const string tagSpace = " ";

		// Token: 0x04000F1A RID: 3866
		private const string tagSemiColon = ";";

		// Token: 0x04000F1B RID: 3867
		private const string tagComma = ",";

		// Token: 0x04000F1C RID: 3868
		private const string tagArrowL = "<";

		// Token: 0x04000F1D RID: 3869
		private const string tagArrowR = ">";

		// Token: 0x04000F1E RID: 3870
		public static bool outputGeneratedSourceIfDebug = true;
	}
}
