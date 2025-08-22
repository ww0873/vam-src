using System;
using Battlehub.Cubeman;
using UnityEngine;

namespace Battlehub.RTHandles
{
	// Token: 0x020000F0 RID: 240
	public class GameFerry : MonoBehaviour
	{
		// Token: 0x06000549 RID: 1353 RVA: 0x0001D8DE File Offset: 0x0001BCDE
		public GameFerry()
		{
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x0001D8E6 File Offset: 0x0001BCE6
		private void Start()
		{
			this.m_joint = base.gameObject.AddComponent<FixedJoint>();
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x0001D8FC File Offset: 0x0001BCFC
		private void OnTriggerEnter(Collider c)
		{
			if (!c.GetComponent<CubemanCharacter>())
			{
				return;
			}
			Rigidbody component = c.GetComponent<Rigidbody>();
			this.m_rig = component;
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x0001D928 File Offset: 0x0001BD28
		private void OnTriggerExit(Collider c)
		{
			if (!c.GetComponent<CubemanCharacter>())
			{
				return;
			}
			this.m_rig = null;
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x0001D944 File Offset: 0x0001BD44
		public void Lock()
		{
			if (!this.m_joint)
			{
				this.m_joint = base.gameObject.AddComponent<FixedJoint>();
			}
			this.m_joint.connectedBody = this.m_rig;
			this.m_joint.breakForce = float.PositiveInfinity;
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x0001D993 File Offset: 0x0001BD93
		public void Unlock()
		{
			if (this.m_joint)
			{
				this.m_joint.breakForce = 0.0001f;
			}
		}

		// Token: 0x040004E1 RID: 1249
		private FixedJoint m_joint;

		// Token: 0x040004E2 RID: 1250
		private Rigidbody m_rig;
	}
}
