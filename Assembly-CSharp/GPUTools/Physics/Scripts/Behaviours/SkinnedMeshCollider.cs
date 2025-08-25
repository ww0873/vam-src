using System;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Behaviours
{
	// Token: 0x02000A47 RID: 2631
	public class SkinnedMeshCollider : MonoBehaviour
	{
		// Token: 0x060043BB RID: 17339 RVA: 0x0013D2E9 File Offset: 0x0013B6E9
		public SkinnedMeshCollider()
		{
		}

		// Token: 0x060043BC RID: 17340 RVA: 0x0013D2F4 File Offset: 0x0013B6F4
		private void OnDrawGizmos()
		{
			if (this.Vertices == null || !this.debugDraw)
			{
				return;
			}
			Gizmos.color = Color.red;
			foreach (Vector3 position in this.Vertices)
			{
				Gizmos.DrawWireSphere(base.transform.TransformPoint(position), 0.01f);
			}
		}

		// Token: 0x170008AD RID: 2221
		// (get) Token: 0x060043BD RID: 17341 RVA: 0x0013D360 File Offset: 0x0013B760
		public Vector3[] Vertices
		{
			get
			{
				Vector3[] array = new Vector3[this.filter.sharedMesh.vertexCount];
				Vector3[] vertices = this.filter.sharedMesh.vertices;
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = base.transform.TransformPoint(vertices[i]);
				}
				return array;
			}
		}

		// Token: 0x04003285 RID: 12933
		[SerializeField]
		private bool debugDraw;

		// Token: 0x04003286 RID: 12934
		[SerializeField]
		private MeshFilter filter;
	}
}
