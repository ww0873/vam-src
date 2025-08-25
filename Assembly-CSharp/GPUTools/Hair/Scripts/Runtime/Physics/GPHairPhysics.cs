using System;
using GPUTools.Hair.Scripts.Runtime.Data;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Runtime.Physics
{
	// Token: 0x02000A1E RID: 2590
	public class GPHairPhysics : MonoBehaviour
	{
		// Token: 0x06004288 RID: 17032 RVA: 0x00138D37 File Offset: 0x00137137
		public GPHairPhysics()
		{
		}

		// Token: 0x06004289 RID: 17033 RVA: 0x00138D3F File Offset: 0x0013713F
		public void Initialize(HairDataFacade data)
		{
			this.world = new HairPhysicsWorld(data);
		}

		// Token: 0x0600428A RID: 17034 RVA: 0x00138D4D File Offset: 0x0013714D
		public void FixedDispatch()
		{
			this.world.FixedDispatch();
		}

		// Token: 0x0600428B RID: 17035 RVA: 0x00138D5A File Offset: 0x0013715A
		public void Dispatch()
		{
			this.world.Dispatch();
		}

		// Token: 0x0600428C RID: 17036 RVA: 0x00138D67 File Offset: 0x00137167
		public void RebindData()
		{
			this.world.RebindData();
		}

		// Token: 0x0600428D RID: 17037 RVA: 0x00138D74 File Offset: 0x00137174
		public void ResetPhysics()
		{
			this.world.Reset();
		}

		// Token: 0x0600428E RID: 17038 RVA: 0x00138D81 File Offset: 0x00137181
		public void PartialResetPhysics()
		{
			this.world.PartialReset();
		}

		// Token: 0x0600428F RID: 17039 RVA: 0x00138D8E File Offset: 0x0013718E
		private void OnDestroy()
		{
			this.world.Dispose();
		}

		// Token: 0x06004290 RID: 17040 RVA: 0x00138D9B File Offset: 0x0013719B
		private void OnDrawGizmos()
		{
			this.world.DebugDraw();
		}

		// Token: 0x04003168 RID: 12648
		private HairPhysicsWorld world;
	}
}
