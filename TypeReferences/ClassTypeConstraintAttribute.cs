using System;
using UnityEngine;

namespace TypeReferences
{
	// Token: 0x020002AF RID: 687
	public abstract class ClassTypeConstraintAttribute : PropertyAttribute
	{
		// Token: 0x0600101D RID: 4125 RVA: 0x0005B6BC File Offset: 0x00059ABC
		protected ClassTypeConstraintAttribute()
		{
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x0600101E RID: 4126 RVA: 0x0005B6CB File Offset: 0x00059ACB
		// (set) Token: 0x0600101F RID: 4127 RVA: 0x0005B6D3 File Offset: 0x00059AD3
		public ClassGrouping Grouping
		{
			get
			{
				return this._grouping;
			}
			set
			{
				this._grouping = value;
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06001020 RID: 4128 RVA: 0x0005B6DC File Offset: 0x00059ADC
		// (set) Token: 0x06001021 RID: 4129 RVA: 0x0005B6E4 File Offset: 0x00059AE4
		public bool AllowAbstract
		{
			get
			{
				return this._allowAbstract;
			}
			set
			{
				this._allowAbstract = value;
			}
		}

		// Token: 0x06001022 RID: 4130 RVA: 0x0005B6ED File Offset: 0x00059AED
		public virtual bool IsConstraintSatisfied(Type type)
		{
			return this.AllowAbstract || !type.IsAbstract;
		}

		// Token: 0x04000E64 RID: 3684
		private ClassGrouping _grouping = ClassGrouping.ByNamespaceFlat;

		// Token: 0x04000E65 RID: 3685
		private bool _allowAbstract;
	}
}
