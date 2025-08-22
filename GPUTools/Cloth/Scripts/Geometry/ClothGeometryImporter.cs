using System;
using GPUTools.Cloth.Scripts.Geometry.Passes;

namespace GPUTools.Cloth.Scripts.Geometry
{
	// Token: 0x02000988 RID: 2440
	public class ClothGeometryImporter
	{
		// Token: 0x06003CED RID: 15597 RVA: 0x00127028 File Offset: 0x00125428
		public ClothGeometryImporter(ClothSettings settings)
		{
			this.settings = settings;
			this.commonJobsPass = new CommonJobsPass(settings);
			this.physicsVerticesPass = new PhysicsVerticesPass(settings);
			this.neighborsPass = new NeighborsPass(settings);
			this.jointsPass = new JointsPass(settings);
			this.stiffnessJointsPass = new StiffnessJointsPass(settings);
			this.nearbyJointsPass = new NearbyJointsPass(settings);
		}

		// Token: 0x06003CEE RID: 15598 RVA: 0x0012708A File Offset: 0x0012548A
		public void Cache()
		{
			this.commonJobsPass.Cache();
			this.physicsVerticesPass.Cache();
			this.neighborsPass.Cache();
		}

		// Token: 0x06003CEF RID: 15599 RVA: 0x001270AD File Offset: 0x001254AD
		public void CancelCacheThreaded()
		{
			this.cancelCacheThreaded = true;
			this.jointsPass.CancelCache();
			this.stiffnessJointsPass.CancelCache();
			this.nearbyJointsPass.CancelCache();
		}

		// Token: 0x06003CF0 RID: 15600 RVA: 0x001270D7 File Offset: 0x001254D7
		public void PrepCacheThreaded()
		{
			this.cancelCacheThreaded = false;
			this.jointsPass.PrepCache();
			this.stiffnessJointsPass.PrepCache();
			this.nearbyJointsPass.PrepCache();
		}

		// Token: 0x06003CF1 RID: 15601 RVA: 0x00127104 File Offset: 0x00125504
		public void CacheThreaded()
		{
			if (!this.cancelCacheThreaded)
			{
				this.jointsPass.Cache();
			}
			if (!this.cancelCacheThreaded)
			{
				this.stiffnessJointsPass.Cache();
			}
			if (!this.cancelCacheThreaded)
			{
				this.nearbyJointsPass.Cache();
			}
			if (!this.cancelCacheThreaded && this.settings.GeometryData.Particles != null)
			{
				this.settings.GeometryData.IsProcessed = true;
			}
		}

		// Token: 0x04002EE4 RID: 12004
		private readonly ClothSettings settings;

		// Token: 0x04002EE5 RID: 12005
		protected CommonJobsPass commonJobsPass;

		// Token: 0x04002EE6 RID: 12006
		protected PhysicsVerticesPass physicsVerticesPass;

		// Token: 0x04002EE7 RID: 12007
		protected NeighborsPass neighborsPass;

		// Token: 0x04002EE8 RID: 12008
		protected JointsPass jointsPass;

		// Token: 0x04002EE9 RID: 12009
		protected StiffnessJointsPass stiffnessJointsPass;

		// Token: 0x04002EEA RID: 12010
		protected NearbyJointsPass nearbyJointsPass;

		// Token: 0x04002EEB RID: 12011
		protected bool cancelCacheThreaded;
	}
}
