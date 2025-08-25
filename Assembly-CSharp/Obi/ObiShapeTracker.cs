using System;
using UnityEngine;

namespace Obi
{
	// Token: 0x020003A0 RID: 928
	public abstract class ObiShapeTracker
	{
		// Token: 0x06001774 RID: 6004 RVA: 0x0008570C File Offset: 0x00083B0C
		protected ObiShapeTracker()
		{
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06001775 RID: 6005 RVA: 0x00085739 File Offset: 0x00083B39
		public IntPtr OniShape
		{
			get
			{
				return this.oniShape;
			}
		}

		// Token: 0x06001776 RID: 6006 RVA: 0x00085741 File Offset: 0x00083B41
		public virtual void Destroy()
		{
			Oni.DestroyShape(this.oniShape);
			this.oniShape = IntPtr.Zero;
		}

		// Token: 0x06001777 RID: 6007
		public abstract void UpdateIfNeeded();

		// Token: 0x04001352 RID: 4946
		protected Component collider;

		// Token: 0x04001353 RID: 4947
		protected Oni.Shape adaptor = default(Oni.Shape);

		// Token: 0x04001354 RID: 4948
		protected IntPtr oniShape = IntPtr.Zero;
	}
}
