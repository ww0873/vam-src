using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace MeshVR
{
	// Token: 0x02000C89 RID: 3209
	public class CubicBezierPointCompact
	{
		// Token: 0x060060CE RID: 24782 RVA: 0x00249432 File Offset: 0x00247832
		public CubicBezierPointCompact()
		{
		}

		// Token: 0x17000E46 RID: 3654
		// (get) Token: 0x060060CF RID: 24783 RVA: 0x0024943A File Offset: 0x0024783A
		// (set) Token: 0x060060D0 RID: 24784 RVA: 0x00249442 File Offset: 0x00247842
		public Vector3 position
		{
			get
			{
				return this._position;
			}
			set
			{
				this.position = value;
				this.dirty = true;
			}
		}

		// Token: 0x17000E47 RID: 3655
		// (get) Token: 0x060060D1 RID: 24785 RVA: 0x00249452 File Offset: 0x00247852
		// (set) Token: 0x060060D2 RID: 24786 RVA: 0x0024946A File Offset: 0x0024786A
		public Vector3 worldPosition
		{
			get
			{
				return this.parent.transform.TransformPoint(this.position);
			}
			set
			{
				this.position = this.parent.transform.InverseTransformPoint(value);
			}
		}

		// Token: 0x17000E48 RID: 3656
		// (get) Token: 0x060060D3 RID: 24787 RVA: 0x00249483 File Offset: 0x00247883
		// (set) Token: 0x060060D4 RID: 24788 RVA: 0x002494A0 File Offset: 0x002478A0
		public Quaternion worldRotation
		{
			get
			{
				return this.parent.transform.rotation * this.rotation;
			}
			set
			{
				this.rotation = Quaternion.Inverse(this.parent.transform.rotation) * value;
			}
		}

		// Token: 0x17000E49 RID: 3657
		// (get) Token: 0x060060D5 RID: 24789 RVA: 0x002494C3 File Offset: 0x002478C3
		// (set) Token: 0x060060D6 RID: 24790 RVA: 0x002494CB File Offset: 0x002478CB
		public bool dirty
		{
			[CompilerGenerated]
			get
			{
				return this.<dirty>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<dirty>k__BackingField = value;
			}
		}

		// Token: 0x04005066 RID: 20582
		public CubicBezierCurveCompact parent;

		// Token: 0x04005067 RID: 20583
		protected Vector3 _position;

		// Token: 0x04005068 RID: 20584
		public Quaternion rotation;

		// Token: 0x04005069 RID: 20585
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool <dirty>k__BackingField;

		// Token: 0x0400506A RID: 20586
		public Vector3 controlPointIn;

		// Token: 0x0400506B RID: 20587
		public Vector3 controlPointOut;
	}
}
