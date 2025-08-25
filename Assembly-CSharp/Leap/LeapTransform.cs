using System;

namespace Leap
{
	// Token: 0x0200061E RID: 1566
	public struct LeapTransform
	{
		// Token: 0x06002669 RID: 9833 RVA: 0x000D7EC9 File Offset: 0x000D62C9
		public LeapTransform(Vector translation, LeapQuaternion rotation)
		{
			this = new LeapTransform(translation, rotation, Vector.Ones);
		}

		// Token: 0x0600266A RID: 9834 RVA: 0x000D7ED8 File Offset: 0x000D62D8
		public LeapTransform(Vector translation, LeapQuaternion rotation, Vector scale)
		{
			this = default(LeapTransform);
			this._scale = scale;
			this.translation = translation;
			this.rotation = rotation;
		}

		// Token: 0x0600266B RID: 9835 RVA: 0x000D7EF8 File Offset: 0x000D62F8
		public Vector TransformPoint(Vector point)
		{
			return this._xBasisScaled * point.x + this._yBasisScaled * point.y + this._zBasisScaled * point.z + this.translation;
		}

		// Token: 0x0600266C RID: 9836 RVA: 0x000D7F50 File Offset: 0x000D6350
		public Vector TransformDirection(Vector direction)
		{
			return this._xBasis * direction.x + this._yBasis * direction.y + this._zBasis * direction.z;
		}

		// Token: 0x0600266D RID: 9837 RVA: 0x000D7FA0 File Offset: 0x000D63A0
		public Vector TransformVelocity(Vector velocity)
		{
			return this._xBasisScaled * velocity.x + this._yBasisScaled * velocity.y + this._zBasisScaled * velocity.z;
		}

		// Token: 0x0600266E RID: 9838 RVA: 0x000D7FF0 File Offset: 0x000D63F0
		public LeapQuaternion TransformQuaternion(LeapQuaternion rhs)
		{
			if (this._quaternionDirty)
			{
				throw new InvalidOperationException("Calling TransformQuaternion after Basis vectors have been modified.");
			}
			if (this._flip)
			{
				rhs.x *= this._flipAxes.x;
				rhs.y *= this._flipAxes.y;
				rhs.z *= this._flipAxes.z;
			}
			return this._quaternion.Multiply(rhs);
		}

		// Token: 0x0600266F RID: 9839 RVA: 0x000D8078 File Offset: 0x000D6478
		public void MirrorX()
		{
			this._xBasis = -this._xBasis;
			this._xBasisScaled = -this._xBasisScaled;
			this._flip = true;
			this._flipAxes.y = -this._flipAxes.y;
			this._flipAxes.z = -this._flipAxes.z;
		}

		// Token: 0x06002670 RID: 9840 RVA: 0x000D80DC File Offset: 0x000D64DC
		public void MirrorZ()
		{
			this._zBasis = -this._zBasis;
			this._zBasisScaled = -this._zBasisScaled;
			this._flip = true;
			this._flipAxes.x = -this._flipAxes.x;
			this._flipAxes.y = -this._flipAxes.y;
		}

		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x06002671 RID: 9841 RVA: 0x000D8140 File Offset: 0x000D6540
		// (set) Token: 0x06002672 RID: 9842 RVA: 0x000D8148 File Offset: 0x000D6548
		public Vector xBasis
		{
			get
			{
				return this._xBasis;
			}
			set
			{
				this._xBasis = value;
				this._xBasisScaled = value * this.scale.x;
				this._quaternionDirty = true;
			}
		}

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x06002673 RID: 9843 RVA: 0x000D817D File Offset: 0x000D657D
		// (set) Token: 0x06002674 RID: 9844 RVA: 0x000D8188 File Offset: 0x000D6588
		public Vector yBasis
		{
			get
			{
				return this._yBasis;
			}
			set
			{
				this._yBasis = value;
				this._yBasisScaled = value * this.scale.y;
				this._quaternionDirty = true;
			}
		}

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x06002675 RID: 9845 RVA: 0x000D81BD File Offset: 0x000D65BD
		// (set) Token: 0x06002676 RID: 9846 RVA: 0x000D81C8 File Offset: 0x000D65C8
		public Vector zBasis
		{
			get
			{
				return this._zBasis;
			}
			set
			{
				this._zBasis = value;
				this._zBasisScaled = value * this.scale.z;
				this._quaternionDirty = true;
			}
		}

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x06002677 RID: 9847 RVA: 0x000D81FD File Offset: 0x000D65FD
		// (set) Token: 0x06002678 RID: 9848 RVA: 0x000D8205 File Offset: 0x000D6605
		public Vector translation
		{
			get
			{
				return this._translation;
			}
			set
			{
				this._translation = value;
			}
		}

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x06002679 RID: 9849 RVA: 0x000D820E File Offset: 0x000D660E
		// (set) Token: 0x0600267A RID: 9850 RVA: 0x000D8218 File Offset: 0x000D6618
		public Vector scale
		{
			get
			{
				return this._scale;
			}
			set
			{
				this._scale = value;
				this._xBasisScaled = this._xBasis * this.scale.x;
				this._yBasisScaled = this._yBasis * this.scale.y;
				this._zBasisScaled = this._zBasis * this.scale.z;
			}
		}

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x0600267B RID: 9851 RVA: 0x000D8289 File Offset: 0x000D6689
		// (set) Token: 0x0600267C RID: 9852 RVA: 0x000D82A8 File Offset: 0x000D66A8
		public LeapQuaternion rotation
		{
			get
			{
				if (this._quaternionDirty)
				{
					throw new InvalidOperationException("Requesting rotation after Basis vectors have been modified.");
				}
				return this._quaternion;
			}
			set
			{
				this._quaternion = value;
				float magnitudeSquared = value.MagnitudeSquared;
				float num = 2f / magnitudeSquared;
				float num2 = value.x * num;
				float num3 = value.y * num;
				float num4 = value.z * num;
				float num5 = value.w * num2;
				float num6 = value.w * num3;
				float num7 = value.w * num4;
				float num8 = value.x * num2;
				float num9 = value.x * num3;
				float num10 = value.x * num4;
				float num11 = value.y * num3;
				float num12 = value.y * num4;
				float num13 = value.z * num4;
				this._xBasis = new Vector(1f - (num11 + num13), num9 + num7, num10 - num6);
				this._yBasis = new Vector(num9 - num7, 1f - (num8 + num13), num12 + num5);
				this._zBasis = new Vector(num10 + num6, num12 - num5, 1f - (num8 + num11));
				this._xBasisScaled = this._xBasis * this.scale.x;
				this._yBasisScaled = this._yBasis * this.scale.y;
				this._zBasisScaled = this._zBasis * this.scale.z;
				this._quaternionDirty = false;
				this._flip = false;
				this._flipAxes = new Vector(1f, 1f, 1f);
			}
		}

		// Token: 0x0600267D RID: 9853 RVA: 0x000D843A File Offset: 0x000D683A
		// Note: this type is marked as 'beforefieldinit'.
		static LeapTransform()
		{
		}

		// Token: 0x040020B2 RID: 8370
		public static readonly LeapTransform Identity = new LeapTransform(Vector.Zero, LeapQuaternion.Identity, Vector.Ones);

		// Token: 0x040020B3 RID: 8371
		private Vector _translation;

		// Token: 0x040020B4 RID: 8372
		private Vector _scale;

		// Token: 0x040020B5 RID: 8373
		private LeapQuaternion _quaternion;

		// Token: 0x040020B6 RID: 8374
		private bool _quaternionDirty;

		// Token: 0x040020B7 RID: 8375
		private bool _flip;

		// Token: 0x040020B8 RID: 8376
		private Vector _flipAxes;

		// Token: 0x040020B9 RID: 8377
		private Vector _xBasis;

		// Token: 0x040020BA RID: 8378
		private Vector _yBasis;

		// Token: 0x040020BB RID: 8379
		private Vector _zBasis;

		// Token: 0x040020BC RID: 8380
		private Vector _xBasisScaled;

		// Token: 0x040020BD RID: 8381
		private Vector _yBasisScaled;

		// Token: 0x040020BE RID: 8382
		private Vector _zBasisScaled;
	}
}
