using System;

namespace Leap.Unity.Attributes
{
	// Token: 0x0200067C RID: 1660
	public abstract class DisableIfBase : CombinablePropertyAttribute, IPropertyDisabler
	{
		// Token: 0x06002852 RID: 10322 RVA: 0x000DE8FC File Offset: 0x000DCCFC
		public DisableIfBase(object isEqualTo, object isNotEqualTo, bool isAndOperation, params string[] propertyNames)
		{
			this.propertyNames = propertyNames;
			this.isAndOperation = isAndOperation;
			if (isEqualTo != null == (isNotEqualTo != null))
			{
				throw new ArgumentException("Must specify exactly one of 'equalTo' or 'notEqualTo'.");
			}
			if (isEqualTo != null)
			{
				this.testValue = isEqualTo;
				this.disableResult = true;
			}
			else if (isNotEqualTo != null)
			{
				this.testValue = isNotEqualTo;
				this.disableResult = false;
			}
			if (!(this.testValue is bool) && !(this.testValue is Enum))
			{
				throw new ArgumentException("Only values of bool or Enum are allowed in comparisons using DisableIf.");
			}
		}

		// Token: 0x040021A7 RID: 8615
		public readonly string[] propertyNames;

		// Token: 0x040021A8 RID: 8616
		public readonly object testValue;

		// Token: 0x040021A9 RID: 8617
		public readonly bool disableResult;

		// Token: 0x040021AA RID: 8618
		public readonly bool isAndOperation;
	}
}
