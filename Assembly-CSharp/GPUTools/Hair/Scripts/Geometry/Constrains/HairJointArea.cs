using System;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.Constrains
{
	// Token: 0x020009ED RID: 2541
	public class HairJointArea : MonoBehaviour
	{
		// Token: 0x06004002 RID: 16386 RVA: 0x00130D11 File Offset: 0x0012F111
		public HairJointArea()
		{
		}

		// Token: 0x06004003 RID: 16387 RVA: 0x00130D19 File Offset: 0x0012F119
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.magenta;
			Gizmos.DrawWireSphere(base.transform.position, this.radius);
		}

		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x06004004 RID: 16388 RVA: 0x00130D3B File Offset: 0x0012F13B
		public float Radius
		{
			get
			{
				return this.radius;
			}
		}

		// Token: 0x0400304E RID: 12366
		[SerializeField]
		private float radius;
	}
}
