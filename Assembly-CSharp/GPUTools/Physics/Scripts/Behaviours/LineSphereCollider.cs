using System;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Behaviours
{
	// Token: 0x02000A45 RID: 2629
	public class LineSphereCollider : MonoBehaviour
	{
		// Token: 0x060043AC RID: 17324 RVA: 0x0013C3F8 File Offset: 0x0013A7F8
		public LineSphereCollider()
		{
		}

		// Token: 0x170008A8 RID: 2216
		// (get) Token: 0x060043AE RID: 17326 RVA: 0x0013C45A File Offset: 0x0013A85A
		// (set) Token: 0x060043AD RID: 17325 RVA: 0x0013C446 File Offset: 0x0013A846
		public Vector3 WorldA
		{
			get
			{
				return base.transform.TransformPoint(this.A);
			}
			set
			{
				this.A = base.transform.InverseTransformPoint(value);
			}
		}

		// Token: 0x170008A9 RID: 2217
		// (get) Token: 0x060043B0 RID: 17328 RVA: 0x0013C481 File Offset: 0x0013A881
		// (set) Token: 0x060043AF RID: 17327 RVA: 0x0013C46D File Offset: 0x0013A86D
		public Vector3 WorldB
		{
			get
			{
				return base.transform.TransformPoint(this.B);
			}
			set
			{
				this.B = base.transform.InverseTransformPoint(value);
			}
		}

		// Token: 0x170008AA RID: 2218
		// (get) Token: 0x060043B2 RID: 17330 RVA: 0x0013C4A4 File Offset: 0x0013A8A4
		// (set) Token: 0x060043B1 RID: 17329 RVA: 0x0013C494 File Offset: 0x0013A894
		public float WorldRadiusA
		{
			get
			{
				return this.RadiusA * base.transform.lossyScale.x;
			}
			set
			{
				this.RadiusA = value / this.Scale;
			}
		}

		// Token: 0x170008AB RID: 2219
		// (get) Token: 0x060043B4 RID: 17332 RVA: 0x0013C4DC File Offset: 0x0013A8DC
		// (set) Token: 0x060043B3 RID: 17331 RVA: 0x0013C4CB File Offset: 0x0013A8CB
		public float WorldRadiusB
		{
			get
			{
				return this.RadiusB * base.transform.lossyScale.x;
			}
			set
			{
				this.RadiusB = value / this.Scale;
			}
		}

		// Token: 0x170008AC RID: 2220
		// (get) Token: 0x060043B5 RID: 17333 RVA: 0x0013C504 File Offset: 0x0013A904
		private float Scale
		{
			get
			{
				return Mathf.Max(Mathf.Max(base.transform.lossyScale.x, base.transform.lossyScale.y), base.transform.lossyScale.z);
			}
		}

		// Token: 0x060043B6 RID: 17334 RVA: 0x0013C554 File Offset: 0x0013A954
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(this.WorldA, this.WorldRadiusA);
			Gizmos.DrawWireSphere(this.WorldB, this.WorldRadiusB);
			if (this.WorldA != this.WorldB)
			{
				Vector3 lhs = Vector3.Normalize(this.WorldA - this.WorldB);
				Vector3 normalized = Vector3.Cross(lhs, new Vector3(lhs.z, lhs.y, -lhs.x)).normalized;
				float f = 0.31415927f;
				float num = Mathf.Cos(f);
				float w = Mathf.Sin(f);
				Quaternion quaternion = new Quaternion(num * lhs.x, num * lhs.y, num * lhs.z, w);
				if (quaternion == Quaternion.identity)
				{
					return;
				}
				Quaternion quaternion2 = Quaternion.identity;
				for (int i = 0; i < 5; i++)
				{
					quaternion2 *= quaternion;
					Matrix4x4 matrix4x = Matrix4x4.TRS(this.WorldA, quaternion2, Vector3.one * this.WorldRadiusA);
					Matrix4x4 matrix4x2 = Matrix4x4.TRS(this.WorldB, quaternion2, Vector3.one * this.WorldRadiusB);
					Vector3 from = matrix4x.MultiplyPoint3x4(normalized);
					Vector3 to = matrix4x2.MultiplyPoint3x4(normalized);
					Gizmos.DrawLine(from, to);
				}
			}
		}

		// Token: 0x04003281 RID: 12929
		[SerializeField]
		public Vector3 A = Vector3.zero;

		// Token: 0x04003282 RID: 12930
		[SerializeField]
		public Vector3 B = new Vector3(0f, -0.2f, 0f);

		// Token: 0x04003283 RID: 12931
		[SerializeField]
		public float RadiusA = 0.1f;

		// Token: 0x04003284 RID: 12932
		[SerializeField]
		public float RadiusB = 0.1f;
	}
}
