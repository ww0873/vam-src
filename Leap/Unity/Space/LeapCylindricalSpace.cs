using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Leap.Unity.Space
{
	// Token: 0x02000710 RID: 1808
	public class LeapCylindricalSpace : LeapRadialSpace
	{
		// Token: 0x06002C0B RID: 11275 RVA: 0x000ED01E File Offset: 0x000EB41E
		public LeapCylindricalSpace()
		{
		}

		// Token: 0x06002C0C RID: 11276 RVA: 0x000ED028 File Offset: 0x000EB428
		protected override ITransformer CosntructBaseTransformer()
		{
			return new LeapCylindricalSpace.Transformer
			{
				space = this,
				anchor = this,
				angleOffset = 0f,
				heightOffset = 0f,
				radiusOffset = base.radius,
				radiansPerMeter = 1f / base.radius
			};
		}

		// Token: 0x06002C0D RID: 11277 RVA: 0x000ED080 File Offset: 0x000EB480
		protected override ITransformer ConstructTransformer(LeapSpaceAnchor anchor)
		{
			return new LeapCylindricalSpace.Transformer
			{
				space = this,
				anchor = anchor
			};
		}

		// Token: 0x06002C0E RID: 11278 RVA: 0x000ED0A4 File Offset: 0x000EB4A4
		protected override void UpdateRadialTransformer(ITransformer transformer, ITransformer parent, Vector3 rectSpaceDelta)
		{
			LeapCylindricalSpace.Transformer transformer2 = transformer as LeapCylindricalSpace.Transformer;
			LeapCylindricalSpace.Transformer transformer3 = parent as LeapCylindricalSpace.Transformer;
			transformer2.angleOffset = transformer3.angleOffset + rectSpaceDelta.x / transformer3.radiusOffset;
			transformer2.heightOffset = transformer3.heightOffset + rectSpaceDelta.y;
			transformer2.radiusOffset = transformer3.radiusOffset + rectSpaceDelta.z;
			transformer2.radiansPerMeter = 1f / transformer2.radiusOffset;
		}

		// Token: 0x02000711 RID: 1809
		public class Transformer : IRadialTransformer, ITransformer
		{
			// Token: 0x06002C0F RID: 11279 RVA: 0x000ED114 File Offset: 0x000EB514
			public Transformer()
			{
			}

			// Token: 0x17000582 RID: 1410
			// (get) Token: 0x06002C10 RID: 11280 RVA: 0x000ED11C File Offset: 0x000EB51C
			// (set) Token: 0x06002C11 RID: 11281 RVA: 0x000ED124 File Offset: 0x000EB524
			public LeapCylindricalSpace space
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

			// Token: 0x17000583 RID: 1411
			// (get) Token: 0x06002C12 RID: 11282 RVA: 0x000ED12D File Offset: 0x000EB52D
			// (set) Token: 0x06002C13 RID: 11283 RVA: 0x000ED135 File Offset: 0x000EB535
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

			// Token: 0x06002C14 RID: 11284 RVA: 0x000ED140 File Offset: 0x000EB540
			public Vector3 TransformPoint(Vector3 localRectPos)
			{
				Vector3 b = this.space.transform.InverseTransformPoint(this.anchor.transform.position);
				Vector3 vector = localRectPos - b;
				float f = this.angleOffset + vector.x / this.radiusOffset;
				float y = this.heightOffset + vector.y;
				float num = this.radiusOffset + vector.z;
				Vector3 result;
				result.x = Mathf.Sin(f) * num;
				result.y = y;
				result.z = Mathf.Cos(f) * num - this.space.radius;
				return result;
			}

			// Token: 0x06002C15 RID: 11285 RVA: 0x000ED1E4 File Offset: 0x000EB5E4
			public Vector3 InverseTransformPoint(Vector3 localWarpedPos)
			{
				localWarpedPos.z += this.space.radius;
				float num = Mathf.Atan2(localWarpedPos.x, localWarpedPos.z);
				float y = localWarpedPos.y;
				Vector2 vector = new Vector2(localWarpedPos.x, localWarpedPos.z);
				float magnitude = vector.magnitude;
				Vector3 b;
				b.x = (num - this.angleOffset) * this.radiusOffset;
				b.y = y - this.heightOffset;
				b.z = magnitude - this.radiusOffset;
				Vector3 a = this.space.transform.InverseTransformPoint(this.anchor.transform.position);
				return a + b;
			}

			// Token: 0x06002C16 RID: 11286 RVA: 0x000ED2A8 File Offset: 0x000EB6A8
			public Quaternion TransformRotation(Vector3 localRectPos, Quaternion localRectRot)
			{
				Vector3 b = this.space.transform.InverseTransformPoint(this.anchor.transform.position);
				Vector3 vector = localRectPos - b;
				float num = this.angleOffset + vector.x / this.radiusOffset;
				Quaternion lhs = Quaternion.Euler(0f, num * 57.29578f, 0f);
				return lhs * localRectRot;
			}

			// Token: 0x06002C17 RID: 11287 RVA: 0x000ED314 File Offset: 0x000EB714
			public Quaternion InverseTransformRotation(Vector3 localWarpedPos, Quaternion localWarpedRot)
			{
				localWarpedPos.z += this.space.radius;
				float num = Mathf.Atan2(localWarpedPos.x, localWarpedPos.z);
				return Quaternion.Euler(0f, -num * 57.29578f, 0f) * localWarpedRot;
			}

			// Token: 0x06002C18 RID: 11288 RVA: 0x000ED36C File Offset: 0x000EB76C
			public Vector3 TransformDirection(Vector3 localRectPos, Vector3 localRectDirection)
			{
				Vector3 b = this.space.transform.InverseTransformPoint(this.anchor.transform.position);
				Vector3 vector = localRectPos - b;
				float num = this.angleOffset + vector.x / this.radiusOffset;
				Quaternion rotation = Quaternion.Euler(0f, num * 57.29578f, 0f);
				return rotation * localRectDirection;
			}

			// Token: 0x06002C19 RID: 11289 RVA: 0x000ED3D8 File Offset: 0x000EB7D8
			public Vector3 InverseTransformDirection(Vector3 localWarpedPos, Vector3 localWarpedDirection)
			{
				localWarpedPos.z += this.space.radius;
				float num = Mathf.Atan2(localWarpedPos.x, localWarpedPos.z);
				return Quaternion.Euler(0f, -num * 57.29578f, 0f) * localWarpedDirection;
			}

			// Token: 0x06002C1A RID: 11290 RVA: 0x000ED430 File Offset: 0x000EB830
			public Matrix4x4 GetTransformationMatrix(Vector3 localRectPos)
			{
				Vector3 b = this.space.transform.InverseTransformPoint(this.anchor.transform.position);
				Vector3 vector = localRectPos - b;
				float num = this.angleOffset + vector.x / this.radiusOffset;
				float y = this.heightOffset + vector.y;
				float num2 = this.radiusOffset + vector.z;
				Vector3 pos;
				pos.x = Mathf.Sin(num) * num2;
				pos.y = y;
				pos.z = Mathf.Cos(num) * num2 - this.space.radius;
				Quaternion q = Quaternion.Euler(0f, num * 57.29578f, 0f);
				return Matrix4x4.TRS(pos, q, Vector3.one);
			}

			// Token: 0x06002C1B RID: 11291 RVA: 0x000ED4F8 File Offset: 0x000EB8F8
			public Vector4 GetVectorRepresentation(Transform element)
			{
				Vector3 a = this.space.transform.InverseTransformPoint(element.position);
				Vector3 b = this.space.transform.InverseTransformPoint(this.anchor.transform.position);
				Vector3 vector = a - b;
				Vector4 result;
				result.x = this.angleOffset + vector.x / this.radiusOffset;
				result.y = this.heightOffset + vector.y;
				result.z = this.radiusOffset + vector.z;
				result.w = 1f / this.radiusOffset;
				return result;
			}

			// Token: 0x04002360 RID: 9056
			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private LeapCylindricalSpace <space>k__BackingField;

			// Token: 0x04002361 RID: 9057
			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private LeapSpaceAnchor <anchor>k__BackingField;

			// Token: 0x04002362 RID: 9058
			public float angleOffset;

			// Token: 0x04002363 RID: 9059
			public float heightOffset;

			// Token: 0x04002364 RID: 9060
			public float radiusOffset;

			// Token: 0x04002365 RID: 9061
			public float radiansPerMeter;
		}
	}
}
