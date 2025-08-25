using System;
using GPUTools.Cloth.Scripts.Runtime.Data;
using UnityEngine;

namespace GPUTools.Cloth.Scripts.Runtime.Physics
{
	// Token: 0x020009A7 RID: 2471
	public class ClothPhysics : MonoBehaviour
	{
		// Token: 0x06003E0A RID: 15882 RVA: 0x0012B6A1 File Offset: 0x00129AA1
		public ClothPhysics()
		{
		}

		// Token: 0x06003E0B RID: 15883 RVA: 0x0012B6A9 File Offset: 0x00129AA9
		public void Initialize(ClothDataFacade data)
		{
			this.world = new ClothPhysicsWorld(data);
		}

		// Token: 0x06003E0C RID: 15884 RVA: 0x0012B6B7 File Offset: 0x00129AB7
		public void ResetPhysics()
		{
			this.world.Reset();
		}

		// Token: 0x06003E0D RID: 15885 RVA: 0x0012B6C4 File Offset: 0x00129AC4
		public void PartialResetPhysics()
		{
			this.world.PartialReset();
		}

		// Token: 0x06003E0E RID: 15886 RVA: 0x0012B6D1 File Offset: 0x00129AD1
		public void FixedDispatch()
		{
			this.world.FixedDispatch();
		}

		// Token: 0x06003E0F RID: 15887 RVA: 0x0012B6DE File Offset: 0x00129ADE
		public void DispatchCopyToOld()
		{
			this.world.DispatchCopyToOld();
		}

		// Token: 0x06003E10 RID: 15888 RVA: 0x0012B6EB File Offset: 0x00129AEB
		public void Dispatch()
		{
			this.world.Dispatch();
		}

		// Token: 0x06003E11 RID: 15889 RVA: 0x0012B6F8 File Offset: 0x00129AF8
		private void OnDestroy()
		{
			this.world.Dispose();
		}

		// Token: 0x06003E12 RID: 15890 RVA: 0x0012B705 File Offset: 0x00129B05
		private void OnDrawGizmos()
		{
			this.world.DebugDraw();
		}

		// Token: 0x04002F64 RID: 12132
		private ClothPhysicsWorld world;
	}
}
