using System;
using GPUTools.Cloth.Scripts.Runtime.Data;
using GPUTools.Hair.Scripts.Utils;
using UnityEngine;

namespace GPUTools.Cloth.Scripts.Runtime.Render
{
	// Token: 0x020009A9 RID: 2473
	public class ClothRender : MonoBehaviour
	{
		// Token: 0x06003E65 RID: 15973 RVA: 0x0012C996 File Offset: 0x0012AD96
		public ClothRender()
		{
		}

		// Token: 0x06003E66 RID: 15974 RVA: 0x0012C99E File Offset: 0x0012AD9E
		public void Initialize(ClothDataFacade data)
		{
			this.data = data;
			this.Update();
		}

		// Token: 0x06003E67 RID: 15975 RVA: 0x0012C9AD File Offset: 0x0012ADAD
		private void UpdateBounds()
		{
			this.data.MeshProvider.Mesh.bounds = base.transform.InverseTransformBounds(this.data.Bounds);
		}

		// Token: 0x06003E68 RID: 15976 RVA: 0x0012C9DC File Offset: 0x0012ADDC
		private void Update()
		{
			for (int i = 0; i < this.data.Materials.Length; i++)
			{
				Material material = this.data.Materials[i];
				material.EnableKeyword("VERTEX_FROM_BUFFER");
				material.SetBuffer("_ClothVertices", this.data.ClothVertices.ComputeBuffer);
			}
			if (this.data.CustomBounds)
			{
				this.UpdateBounds();
			}
		}

		// Token: 0x04002FA3 RID: 12195
		private ClothDataFacade data;
	}
}
