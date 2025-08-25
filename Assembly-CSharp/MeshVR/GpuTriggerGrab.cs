using System;
using UnityEngine;

namespace MeshVR
{
	// Token: 0x02000C15 RID: 3093
	public class GpuTriggerGrab : MonoBehaviour
	{
		// Token: 0x060059E0 RID: 23008 RVA: 0x00210F29 File Offset: 0x0020F329
		public GpuTriggerGrab()
		{
		}

		// Token: 0x060059E1 RID: 23009 RVA: 0x00210F31 File Offset: 0x0020F331
		private void Start()
		{
			this.grabSphere = base.GetComponent<GpuGrabSphere>();
		}

		// Token: 0x060059E2 RID: 23010 RVA: 0x00210F40 File Offset: 0x0020F340
		private void Update()
		{
			if (SuperController.singleton != null)
			{
				if (this.side == GpuTriggerGrab.Side.Left)
				{
					if (SuperController.singleton.GetLeftGrab())
					{
						this.grabSphere.enabled = true;
					}
					if (SuperController.singleton.GetLeftGrabRelease())
					{
						this.grabSphere.enabled = false;
					}
				}
				else
				{
					if (SuperController.singleton.GetRightGrab())
					{
						this.grabSphere.enabled = true;
					}
					if (SuperController.singleton.GetRightGrabRelease())
					{
						this.grabSphere.enabled = false;
					}
				}
			}
		}

		// Token: 0x04004A25 RID: 18981
		public GpuTriggerGrab.Side side;

		// Token: 0x04004A26 RID: 18982
		protected GpuGrabSphere grabSphere;

		// Token: 0x02000C16 RID: 3094
		public enum Side
		{
			// Token: 0x04004A28 RID: 18984
			Left,
			// Token: 0x04004A29 RID: 18985
			Right
		}
	}
}
