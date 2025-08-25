using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace TypeReferences
{
	// Token: 0x020002B1 RID: 689
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public sealed class ClassImplementsAttribute : ClassTypeConstraintAttribute
	{
		// Token: 0x06001028 RID: 4136 RVA: 0x0005B75C File Offset: 0x00059B5C
		public ClassImplementsAttribute()
		{
		}

		// Token: 0x06001029 RID: 4137 RVA: 0x0005B764 File Offset: 0x00059B64
		public ClassImplementsAttribute(Type interfaceType)
		{
			this.InterfaceType = interfaceType;
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x0600102A RID: 4138 RVA: 0x0005B773 File Offset: 0x00059B73
		// (set) Token: 0x0600102B RID: 4139 RVA: 0x0005B77B File Offset: 0x00059B7B
		public Type InterfaceType
		{
			[CompilerGenerated]
			get
			{
				return this.<InterfaceType>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<InterfaceType>k__BackingField = value;
			}
		}

		// Token: 0x0600102C RID: 4140 RVA: 0x0005B784 File Offset: 0x00059B84
		public override bool IsConstraintSatisfied(Type type)
		{
			if (base.IsConstraintSatisfied(type))
			{
				foreach (Type type2 in type.GetInterfaces())
				{
					if (type2 == this.InterfaceType)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04000E67 RID: 3687
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Type <InterfaceType>k__BackingField;
	}
}
