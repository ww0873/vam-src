using System;

namespace Leap.Unity.Attributes
{
	// Token: 0x02000682 RID: 1666
	public class ImplementsInterfaceAttribute : CombinablePropertyAttribute, IPropertyConstrainer, IFullPropertyDrawer, ISupportDragAndDrop
	{
		// Token: 0x0600285C RID: 10332 RVA: 0x000DEA73 File Offset: 0x000DCE73
		public ImplementsInterfaceAttribute(Type type)
		{
			if (!type.IsInterface)
			{
				throw new Exception(type.Name + " is not an interface.");
			}
			this.type = type;
		}

		// Token: 0x040021AB RID: 8619
		private Type type;
	}
}
