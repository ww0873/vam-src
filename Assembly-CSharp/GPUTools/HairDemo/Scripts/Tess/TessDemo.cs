using System;
using GPUTools.Common.Scripts.Tools;
using UnityEngine;

namespace GPUTools.HairDemo.Scripts.Tess
{
	// Token: 0x020009E9 RID: 2537
	public class TessDemo : MonoBehaviour
	{
		// Token: 0x06003FE1 RID: 16353 RVA: 0x001307DD File Offset: 0x0012EBDD
		public TessDemo()
		{
		}

		// Token: 0x06003FE2 RID: 16354 RVA: 0x001307FA File Offset: 0x0012EBFA
		private void Start()
		{
			this.Gen();
			this.oldCount = this.count;
		}

		// Token: 0x06003FE3 RID: 16355 RVA: 0x0013080E File Offset: 0x0012EC0E
		private void Update()
		{
			if (this.count != this.oldCount)
			{
				this.Gen();
			}
			this.oldCount = this.count;
		}

		// Token: 0x06003FE4 RID: 16356 RVA: 0x00130834 File Offset: 0x0012EC34
		private void Gen()
		{
			this.barycentric.Reset();
			int steps = 1;
			float num = 0.2f;
			if (this.count >= 15)
			{
				num = 0.1f;
				steps = 2;
			}
			if (this.count >= 45)
			{
				num = 0.05f;
				steps = 3;
			}
			float num2 = 1f - num;
			float num3 = (1f - num2) * 0.5f;
			this.Split(new Vector3(num2, num3, num3), new Vector3(num3, num2, num3), new Vector3(num3, num3, num2), steps);
			while (this.barycentric.Count < this.count)
			{
				Vector3 randomK = this.GetRandomK();
				if (!this.barycentric.Contains(randomK))
				{
					this.barycentric.Add(this.GetRandomK());
				}
			}
			Debug.Log(this.barycentric.Count);
		}

		// Token: 0x06003FE5 RID: 16357 RVA: 0x00130910 File Offset: 0x0012ED10
		private void Split(Vector3 b1, Vector3 b2, Vector3 b3, int steps)
		{
			steps--;
			this.TryAdd(b1);
			this.TryAdd(b2);
			this.TryAdd(b3);
			Vector3 vector = (b1 + b2) * 0.5f;
			Vector3 vector2 = (b2 + b3) * 0.5f;
			Vector3 b4 = (b3 + b1) * 0.5f;
			if (steps < 0)
			{
				return;
			}
			this.Split(b1, vector, b4, steps);
			this.Split(b2, vector, vector2, steps);
			this.Split(b3, vector2, b4, steps);
			this.Split(vector, vector2, b4, steps);
		}

		// Token: 0x06003FE6 RID: 16358 RVA: 0x001309A3 File Offset: 0x0012EDA3
		private void TryAdd(Vector3 v)
		{
			if (!this.barycentric.Contains(v))
			{
				this.barycentric.Add(v);
			}
		}

		// Token: 0x06003FE7 RID: 16359 RVA: 0x001309C4 File Offset: 0x0012EDC4
		private Vector3 GetRandomK()
		{
			float num = UnityEngine.Random.Range(0f, 1f);
			float num2 = UnityEngine.Random.Range(0f, 1f);
			if (num + num2 > 1f)
			{
				num = 1f - num;
				num2 = 1f - num2;
			}
			float z = 1f - (num + num2);
			return new Vector3(num, num2, z);
		}

		// Token: 0x06003FE8 RID: 16360 RVA: 0x00130A20 File Offset: 0x0012EE20
		private void OnDrawGizmos()
		{
			Gizmos.DrawLine(this.a, this.b);
			Gizmos.DrawLine(this.b, this.c);
			Gizmos.DrawLine(this.c, this.a);
			Gizmos.color = new Color(1f, 0f, 0f, 1f);
			for (int i = 0; i < this.barycentric.Count; i++)
			{
				Vector3 vector = this.barycentric[i];
				Vector3 center = this.a * vector.x + this.b * vector.y + this.c * vector.z;
				Gizmos.DrawSphere(center, 0.01f);
			}
		}

		// Token: 0x04003043 RID: 12355
		[SerializeField]
		[Range(6f, 64f)]
		private int count = 64;

		// Token: 0x04003044 RID: 12356
		private int oldCount;

		// Token: 0x04003045 RID: 12357
		[SerializeField]
		private Vector3 a;

		// Token: 0x04003046 RID: 12358
		[SerializeField]
		private Vector3 b;

		// Token: 0x04003047 RID: 12359
		[SerializeField]
		private Vector3 c;

		// Token: 0x04003048 RID: 12360
		private FixedList<Vector3> barycentric = new FixedList<Vector3>(64);
	}
}
