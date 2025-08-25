using System;
using System.Reflection;

namespace DynamicCSharp
{
	// Token: 0x020002DF RID: 735
	public sealed class PropertyProxy : IMemberProxy
	{
		// Token: 0x06001140 RID: 4416 RVA: 0x000606C7 File Offset: 0x0005EAC7
		internal PropertyProxy(ScriptProxy owner)
		{
			this.owner = owner;
		}

		// Token: 0x170001DA RID: 474
		public object this[string name]
		{
			get
			{
				PropertyInfo propertyInfo = this.owner.ScriptType.FindCachedProperty(name);
				if (propertyInfo == null)
				{
					throw new TargetException(string.Format("Type '{0}' does not define a property called '{1}'", this.owner.ScriptType, name));
				}
				if (!propertyInfo.CanRead)
				{
					throw new TargetException(string.Format("The property '{0}' was found but it does not define a get accessor", name));
				}
				return propertyInfo.GetValue(this.owner.Instance, null);
			}
			set
			{
				PropertyInfo propertyInfo = this.owner.ScriptType.FindCachedProperty(name);
				if (propertyInfo == null)
				{
					throw new TargetException(string.Format("Type '{0}' does not define a property called '{1}'", this.owner.ScriptType, name));
				}
				if (!propertyInfo.CanWrite)
				{
					throw new TargetException(string.Format("The property '{0}' was found but it does not define a set accessor", name));
				}
				propertyInfo.SetValue(this.owner.Instance, value, null);
			}
		}

		// Token: 0x04000F20 RID: 3872
		private ScriptProxy owner;
	}
}
