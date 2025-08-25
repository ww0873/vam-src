using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Leap.Unity.Space
{
	// Token: 0x02000714 RID: 1812
	public class LeapSphericalSpace : LeapRadialSpace
	{
		// Token: 0x06002C23 RID: 11299 RVA: 0x000ED59F File Offset: 0x000EB99F
		public LeapSphericalSpace()
		{
		}

		// Token: 0x06002C24 RID: 11300 RVA: 0x000ED5A8 File Offset: 0x000EB9A8
		protected override ITransformer CosntructBaseTransformer()
		{
			return new LeapSphericalSpace.Transformer
			{
				space = this,
				anchor = this,
				angleXOffset = 0f,
				angleYOffset = 0f,
				radiusOffset = base.radius,
				radiansPerMeter = 1f / base.radius
			};
		}

		// Token: 0x06002C25 RID: 11301 RVA: 0x000ED600 File Offset: 0x000EBA00
		protected override ITransformer ConstructTransformer(LeapSpaceAnchor anchor)
		{
			return new LeapSphericalSpace.Transformer
			{
				space = this,
				anchor = anchor
			};
		}

		// Token: 0x06002C26 RID: 11302 RVA: 0x000ED624 File Offset: 0x000EBA24
		protected override void UpdateRadialTransformer(ITransformer transformer, ITransformer parent, Vector3 rectSpaceDelta)
		{
			LeapSphericalSpace.Transformer transformer2 = transformer as LeapSphericalSpace.Transformer;
			LeapSphericalSpace.Transformer transformer3 = parent as LeapSphericalSpace.Transformer;
			transformer2.angleXOffset = transformer3.angleXOffset + rectSpaceDelta.x / transformer3.radiusOffset;
			transformer2.angleYOffset = transformer3.angleYOffset + rectSpaceDelta.y / transformer3.radiusOffset;
			transformer2.radiusOffset = transformer3.radiusOffset + rectSpaceDelta.z;
			transformer2.radiansPerMeter = 1f / transformer2.radiusOffset;
		}

		// Token: 0x02000715 RID: 1813
		public class Transformer : IRadialTransformer, ITransformer
		{
			// Token: 0x06002C27 RID: 11303 RVA: 0x000ED69B File Offset: 0x000EBA9B
			public Transformer()
			{
			}

			// Token: 0x17000585 RID: 1413
			// (get) Token: 0x06002C28 RID: 11304 RVA: 0x000ED6A3 File Offset: 0x000EBAA3
			// (set) Token: 0x06002C29 RID: 11305 RVA: 0x000ED6AB File Offset: 0x000EBAAB
			public LeapSphericalSpace space
			{
				[CompilerGenerated]
				get
				{
					return this.<space>k__BackingField;
				}
				[CompilerGenerated]
				set
				{
					this.<space>k__BackingField = value;
				}
			}

			// Token: 0x17000586 RID: 1414
			// (get) Token: 0x06002C2A RID: 11306 RVA: 0x000ED6B4 File Offset: 0x000EBAB4
			// (set) Token: 0x06002C2B RID: 11307 RVA: 0x000ED6BC File Offset: 0x000EBABC
			public LeapSpaceAnchor anchor
			{
				[CompilerGenerated]
				get
				{
					return this.<anchor>k__BackingField;
				}
				[CompilerGenerated]
				set
				{
					this.<anchor>k__BackingField = value;
				}
			}

			// Token: 0x06002C2C RID: 11308 RVA: 0x000ED6C8 File Offset: 0x000EBAC8
			public Vector3 TransformPoint(Vector3 localRectPos)
			{
				Vector3 b = this.space.transform.InverseTransformPoint(this.anchor.transform.position);
				Vector3 vector = localRectPos - b;
				float f = this.angleXOffset + vector.x / this.radiusOffset;
				float f2 = this.angleYOffset + vector.y / this.radiusOffset;
				float num = this.radiusOffset + vector.z;
				Vector3 vector2;
				vector2.x = 0f;
				vector2.y = Mathf.Sin(f2) * num;
				vector2.z = Mathf.Cos(f2) * num;
				Vector3 result;
				result.x = Mathf.Sin(f) * vector2.z;
				result.y = vector2.y;
				result.z = Mathf.Cos(f) * vector2.z - this.space.radius;
				return result;
			}

			// Token: 0x06002C2D RID: 11309 RVA: 0x000ED7AC File Offset: 0x000EBBAC
			public Vector3 InverseTransformPoint(Vector3 localWarpedPos)
			{
				localWarpedPos.z += this.space.radius;
				Vector3 vector;
				vector.x = 0f;
				vector.y = localWarpedPos.y;
				Vector2 vector2 = new Vector2(localWarpedPos.x, localWarpedPos.z);
				vector.z = vector2.magnitude;
				float num = Mathf.Atan2(localWarpedPos.x, localWarpedPos.z);
				float num2 = Mathf.Atan2(vector.y, vector.z);
				Vector2 vector3 = new Vector2(vector.z, vector.y);
				float magnitude = vector3.magnitude;
				Vector3 b;
				b.x = (num - this.angleXOffset) * this.radiusOffset;
				b.y = (num2 - this.angleYOffset) * this.radiusOffset;
				b.z = magnitude - this.radiusOffset;
				Vector3 a = this.space.transform.InverseTransformPoint(this.anchor.transform.position);
				return a + b;
			}

			// Token: 0x06002C2E RID: 11310 RVA: 0x000ED8C0 File Offset: 0x000EBCC0
			public Quaternion TransformRotation(Vector3 localRectPos, Quaternion localRectRot)
			{
				Vector3 b = this.space.transform.InverseTransformPoint(this.anchor.transform.position);
				Vector3 vector = localRectPos - b;
				float num = this.angleXOffset + vector.x / this.radiusOffset;
				float num2 = this.angleYOffset + vector.y / this.radiusOffset;
				Quaternion lhs = Quaternion.Euler(-num2 * 57.29578f, num * 57.29578f, 0f);
				return lhs * localRectRot;
			}

			// Token: 0x06002C2F RID: 11311 RVA: 0x000ED948 File Offset: 0x000EBD48
			public Quaternion InverseTransformRotation(Vector3 localWarpedPos, Quaternion localWarpedRot)
			{
				localWarpedPos.z += this.space.radius;
				Vector3 vector;
				vector.x = 0f;
				vector.y = localWarpedPos.y;
				Vector2 vector2 = new Vector2(localWarpedPos.x, localWarpedPos.z);
				vector.z = vector2.magnitude;
				float num = Mathf.Atan2(localWarpedPos.x, localWarpedPos.z);
				float num2 = Mathf.Atan2(vector.y, vector.z);
				Quaternion rotation = Quaternion.Euler(-num2 * 57.29578f, num * 57.29578f, 0f);
				Quaternion lhs = Quaternion.Inverse(rotation);
				return lhs * localWarpedRot;
			}

			// Token: 0x06002C30 RID: 11312 RVA: 0x000EDA00 File Offset: 0x000EBE00
			public Vector3 TransformDirection(Vector3 localRectPos, Vector3 localRectDirection)
			{
				Vector3 b = this.space.transform.InverseTransformPoint(this.anchor.transform.position);
				Vector3 vector = localRectPos - b;
				float num = this.angleXOffset + vector.x / this.radiusOffset;
				float num2 = this.angleYOffset + vector.y / this.radiusOffset;
				Quaternion rotation = Quaternion.Euler(-num2 * 57.29578f, num * 57.29578f, 0f);
				return rotation * localRectDirection;
			}

			// Token: 0x06002C31 RID: 11313 RVA: 0x000EDA88 File Offset: 0x000EBE88
			public Vector3 InverseTransformDirection(Vector3 localWarpedPos, Vector3 localWarpedDirection)
			{
				localWarpedPos.z += this.space.radius;
				Vector3 vector;
				vector.x = 0f;
				vector.y = localWarpedPos.y;
				Vector2 vector2 = new Vector2(localWarpedPos.x, localWarpedPos.z);
				vector.z = vector2.magnitude;
				float num = Mathf.Atan2(localWarpedPos.x, localWarpedPos.z);
				float num2 = Mathf.Atan2(vector.y, vector.z);
				Quaternion rotation = Quaternion.Euler(-num2 * 57.29578f, num * 57.29578f, 0f);
				Quaternion rotation2 = Quaternion.Inverse(rotation);
				return rotation2 * localWarpedDirection;
			}

			// Token: 0x06002C32 RID: 11314 RVA: 0x000EDB40 File Offset: 0x000EBF40
			public Matrix4x4 GetTransformationMatrix(Vector3 localRectPos)
			{
				Vector3 b = this.space.transform.InverseTransformPoint(this.anchor.transform.position);
				Vector3 vector = localRectPos - b;
				float num = this.angleXOffset + vector.x / this.radiusOffset;
				float num2 = this.angleYOffset + vector.y / this.radiusOffset;
				float num3 = this.radiusOffset + vector.z;
				Vector3 vector2;
				vector2.x = 0f;
				vector2.y = Mathf.Sin(num2) * num3;
				vector2.z = Mathf.Cos(num2) * num3;
				Vector3 pos;
				pos.x = Mathf.Sin(num) * vector2.z;
				pos.y = vector2.y;
				pos.z = Mathf.Cos(num) * vector2.z - this.space.radius;
				Quaternion q = Quaternion.Euler(-num2 * 57.29578f, num * 57.29578f, 0f);
				return Matrix4x4.TRS(pos, q, Vector3.one);
			}

			// Token: 0x06002C33 RID: 11315 RVA: 0x000EDC4C File Offset: 0x000EC04C
			public Vector4 GetVectorRepresentation(Transform element)
			{
				Vector3 a = this.space.transform.InverseTransformPoint(element.position);
				Vector3 b = this.space.transform.InverseTransformPoint(this.anchor.transform.position);
				Vector3 vector = a - b;
				Vector4 result;
				result.x = this.angleXOffset + vector.x / this.radiusOffset;
				result.y = this.angleYOffset + vector.y / this.radiusOffset;
				result.z = this.radiusOffset + vector.z;
				result.w = 1f / this.radiusOffset;
				return result;
			}

			// Token: 0x04002367 RID: 9063
			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private LeapSphericalSpace <space>k__BackingField;

			// Token: 0x04002368 RID: 9064
			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private LeapSpaceAnchor <anchor>k__BackingField;

			// Token: 0x04002369 RID: 9065
			public float angleXOffset;

			// Token: 0x0400236A RID: 9066
			public float angleYOffset;

			// Token: 0x0400236B RID: 9067
			public float radiusOffset;

			// Token: 0x0400236C RID: 9068
			public float radiansPerMeter;
		}
	}
}
