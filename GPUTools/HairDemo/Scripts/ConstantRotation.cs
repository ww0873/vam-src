using System;
using UnityEngine;

namespace GPUTools.HairDemo.Scripts
{
	// Token: 0x020009E7 RID: 2535
	public class ConstantRotation : MonoBehaviour
	{
		// Token: 0x06003FD3 RID: 16339 RVA: 0x0013059B File Offset: 0x0012E99B
		public ConstantRotation()
		{
		}

		// Token: 0x06003FD4 RID: 16340 RVA: 0x001305A3 File Offset: 0x0012E9A3
		private void Update()
		{
			base.transform.Rotate(this.axis, this.Speed * Time.deltaTime);
		}

		// Token: 0x04003039 RID: 12345
		[SerializeField]
		private Vector3 axis;

		// Token: 0x0400303A RID: 12346
		[SerializeField]
		public float Speed;
	}
}
