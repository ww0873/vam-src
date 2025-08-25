using System;
using UnityEngine;

namespace GPUTools.Common.Scripts.Tools.Debug
{
	// Token: 0x020009C7 RID: 2503
	public class MeshDebuger : MonoBehaviour
	{
		// Token: 0x06003F34 RID: 16180 RVA: 0x0012EB83 File Offset: 0x0012CF83
		public MeshDebuger()
		{
		}

		// Token: 0x06003F35 RID: 16181 RVA: 0x0012EB8C File Offset: 0x0012CF8C
		private void Start()
		{
			Vector3[] vertices = this.filter.mesh.vertices;
			Debug.Log("VerticesNum" + vertices.Length);
		}

		// Token: 0x06003F36 RID: 16182 RVA: 0x0012EBC1 File Offset: 0x0012CFC1
		private void OnDrawGizmos()
		{
		}

		// Token: 0x04002FF6 RID: 12278
		[SerializeField]
		private MeshFilter filter;
	}
}
