using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace TypeReferences
{
	// Token: 0x020002B0 RID: 688
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public sealed class ClassExtendsAttribute : ClassTypeConstraintAttribute
	{
		// Token: 0x06001023 RID: 4131 RVA: 0x0005B706 File Offset: 0x00059B06
		public ClassExtendsAttribute()
		{
		}

		// Token: 0x06001024 RID: 4132 RVA: 0x0005B70E File Offset: 0x00059B0E
		public ClassExtendsAttribute(Type baseType)
		{
			this.BaseType = baseType;
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06001025 RID: 4133 RVA: 0x0005B71D File Offset: 0x00059B1D
		// (set) Token: 0x06001026 RID: 4134 RVA: 0x0005B725 File Offset: 0x00059B25
		public Type BaseType
		{
			[CompilerGenerated]
			get
			{
				return this.<BaseType>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<BaseType>k__BackingField = value;
			}
		}

		// Token: 0x06001027 RID: 4135 RVA: 0x0005B72E File Offset: 0x00059B2E
		public override bool IsConstraintSatisfied(Type type)
		{
			return base.IsConstraintSatisfied(type) && this.BaseType.IsAssignableFrom(type) && type != this.BaseType;
		}

		// Token: 0x04000E66 RID: 3686
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Type <BaseType>k__BackingField;
	}
}
