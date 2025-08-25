using System;
using GPUTools.Skinner.Scripts.Kernels;
using UnityEngine;

namespace GPUTools.HairDemo.Scripts.BlendShapesDemo
{
	// Token: 0x020009E6 RID: 2534
	public class TestBlendShapes : MonoBehaviour
	{
		// Token: 0x06003FCE RID: 16334 RVA: 0x001304BD File Offset: 0x0012E8BD
		public TestBlendShapes()
		{
		}

		// Token: 0x06003FCF RID: 16335 RVA: 0x001304C5 File Offset: 0x0012E8C5
		private void Start()
		{
			this.skinner = new GPUSkinnerPro(this.skin);
			this.skinner.Dispatch();
			this.vertices = this.skin.sharedMesh.vertices;
		}

		// Token: 0x06003FD0 RID: 16336 RVA: 0x001304F9 File Offset: 0x0012E8F9
		private void Update()
		{
			this.skinner.Dispatch();
		}

		// Token: 0x06003FD1 RID: 16337 RVA: 0x00130506 File Offset: 0x0012E906
		private void OnDestroy()
		{
			this.skinner.Dispose();
		}

		// Token: 0x06003FD2 RID: 16338 RVA: 0x00130514 File Offset: 0x0012E914
		private void OnDrawGizmos()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			this.skinner.TransformMatricesBuffer.PullData();
			Matrix4x4[] data = this.skinner.TransformMatricesBuffer.Data;
			for (int i = 0; i < this.vertices.Length; i++)
			{
				Vector3 point = this.vertices[i];
				Vector3 center = data[i].MultiplyPoint3x4(point);
				Gizmos.color = Color.red;
				Gizmos.DrawWireSphere(center, 0.002f);
			}
		}

		// Token: 0x04003036 RID: 12342
		[SerializeField]
		private SkinnedMeshRenderer skin;

		// Token: 0x04003037 RID: 12343
		private GPUSkinnerPro skinner;

		// Token: 0x04003038 RID: 12344
		private Vector3[] vertices;
	}
}
