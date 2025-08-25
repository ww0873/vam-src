using System;
using System.Collections.Generic;
using UnityEngine;

namespace Leap.Unity.Attributes
{
	// Token: 0x02000683 RID: 1667
	public class ImplementsTypeNameDropdownAttribute : CombinablePropertyAttribute, IFullPropertyDrawer
	{
		// Token: 0x0600285D RID: 10333 RVA: 0x000DEAA3 File Offset: 0x000DCEA3
		public ImplementsTypeNameDropdownAttribute(Type type)
		{
			this._baseType = type;
		}

		// Token: 0x040021AC RID: 8620
		protected Type _baseType;

		// Token: 0x040021AD RID: 8621
		protected List<Type> _implementingTypes = new List<Type>();

		// Token: 0x040021AE RID: 8622
		protected GUIContent[] _typeOptions;
	}
}
