using System;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Physics.Scripts.Types.Dynamic;
using UnityEngine;

namespace GPUTools.ClothDemo.Scripts
{
	// Token: 0x020009E1 RID: 2529
	public class TestKernels : MonoBehaviour
	{
		// Token: 0x06003FB6 RID: 16310 RVA: 0x001301BE File Offset: 0x0012E5BE
		public TestKernels()
		{
		}

		// Token: 0x06003FB7 RID: 16311 RVA: 0x001301C8 File Offset: 0x0012E5C8
		private void Start()
		{
			GPParticle[] array = new GPParticle[6849];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new GPParticle(Vector3.left * 0.1f * (float)i, 0.1f);
			}
			this.buffer = new GpuBuffer<GPParticle>(array, GPParticle.Size());
			this.primitive = new TestPrimitive();
			this.primitive.Particles = this.buffer;
			this.primitive.Dt = new GpuValue<float>(0.02f);
			this.primitive.InvDrag = new GpuValue<float>(1f);
			this.primitive.Gravity = new GpuValue<Vector3>(Vector3.down * 0.001f);
			this.primitive.Wind = new GpuValue<Vector3>(Vector3.zero);
			this.primitive.Start();
		}

		// Token: 0x06003FB8 RID: 16312 RVA: 0x001302B6 File Offset: 0x0012E6B6
		private void Update()
		{
			this.primitive.Dt.Value = Time.deltaTime;
			this.primitive.Dispatch();
			this.buffer.PullData();
		}

		// Token: 0x06003FB9 RID: 16313 RVA: 0x001302E4 File Offset: 0x0012E6E4
		private void OnDrawGizmos()
		{
			if (this.buffer != null)
			{
				for (int i = 0; i < this.buffer.Data.Length; i++)
				{
					Gizmos.DrawWireSphere(this.buffer.Data[i].Position, 0.1f);
				}
			}
		}

		// Token: 0x0400302B RID: 12331
		private TestPrimitive primitive;

		// Token: 0x0400302C RID: 12332
		private GpuBuffer<GPParticle> buffer;
	}
}
