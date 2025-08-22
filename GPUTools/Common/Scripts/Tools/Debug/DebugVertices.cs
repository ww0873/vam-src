using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Common.Scripts.Tools.Debug
{
	// Token: 0x020009C4 RID: 2500
	public class DebugVertices : MonoBehaviour
	{
		// Token: 0x06003F2A RID: 16170 RVA: 0x0012EA10 File Offset: 0x0012CE10
		public DebugVertices()
		{
		}

		// Token: 0x06003F2B RID: 16171 RVA: 0x0012EA18 File Offset: 0x0012CE18
		public static DebugVertices Draw(List<Vector3> vertices, float radius)
		{
			GameObject gameObject = new GameObject("DebugVertices");
			DebugVertices debugVertices = gameObject.AddComponent<DebugVertices>();
			debugVertices.Vertices = vertices;
			debugVertices.Radius = radius;
			return debugVertices;
		}

		// Token: 0x06003F2C RID: 16172 RVA: 0x0012EA48 File Offset: 0x0012CE48
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.magenta;
			foreach (Vector3 center in this.Vertices)
			{
				Gizmos.DrawWireSphere(center, this.Radius);
			}
		}

		// Token: 0x04002FF3 RID: 12275
		public List<Vector3> Vertices;

		// Token: 0x04002FF4 RID: 12276
		public float Radius;
	}
}
