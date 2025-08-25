using System;
using UnityEngine;

namespace MeshVR
{
	// Token: 0x02000C14 RID: 3092
	public class GpuProximityGrab : MonoBehaviour
	{
		// Token: 0x060059DD RID: 23005 RVA: 0x00210E90 File Offset: 0x0020F290
		public GpuProximityGrab()
		{
		}

		// Token: 0x060059DE RID: 23006 RVA: 0x00210EA3 File Offset: 0x0020F2A3
		private void Start()
		{
			this.grabSphere = base.GetComponent<GpuGrabSphere>();
		}

		// Token: 0x060059DF RID: 23007 RVA: 0x00210EB4 File Offset: 0x0020F2B4
		private void Update()
		{
			if (this.measureTo != null)
			{
				this.currentSquareDistance = (this.measureTo.position - base.transform.position).sqrMagnitude;
				if (this.currentSquareDistance < this.squareDistance)
				{
					this.grabSphere.enabled = true;
				}
				else
				{
					this.grabSphere.enabled = false;
				}
			}
		}

		// Token: 0x04004A21 RID: 18977
		public Transform measureTo;

		// Token: 0x04004A22 RID: 18978
		public float squareDistance = 0.0001f;

		// Token: 0x04004A23 RID: 18979
		public float currentSquareDistance;

		// Token: 0x04004A24 RID: 18980
		protected GpuGrabSphere grabSphere;
	}
}
