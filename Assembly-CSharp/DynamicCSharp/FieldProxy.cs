using System;
using System.Reflection;

namespace DynamicCSharp
{
	// Token: 0x020002DE RID: 734
	public sealed class FieldProxy : IMemberProxy
	{
		// Token: 0x0600113D RID: 4413 RVA: 0x0006060E File Offset: 0x0005EA0E
		internal FieldProxy(ScriptProxy owner)
		{
			this.owner = owner;
		}

		// Token: 0x170001D9 RID: 473
		public object this[string name]
		{
			get
			{
				FieldInfo fieldInfo = this.owner.ScriptType.FindCachedField(name);
				if (fieldInfo == null)
				{
					throw new TargetException(string.Format("Type '{0}' does not define a field called '{1}'", this.owner.ScriptType, name));
				}
				return fieldInfo.GetValue(this.owner.Instance);
			}
			set
			{
				FieldInfo fieldInfo = this.owner.ScriptType.FindCachedField(name);
				if (fieldInfo == null)
				{
					throw new TargetException(string.Format("Type '{0}' does not define a field called '{1}'", this.owner.ScriptType, name));
				}
				fieldInfo.SetValue(this.owner.Instance, value);
			}
		}

		// Token: 0x04000F1F RID: 3871
		private ScriptProxy owner;
	}
}
