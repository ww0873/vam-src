using System;
using GPUTools.Common.Scripts.Tools.Commands;

namespace GPUTools.Hair.Scripts.Runtime.Commands.Physics
{
	// Token: 0x02000A0E RID: 2574
	public class BuildEditLineSpheres : BuildChainCommand
	{
		// Token: 0x0600414B RID: 16715 RVA: 0x0013643B File Offset: 0x0013483B
		public BuildEditLineSpheres(HairSettings settings)
		{
			this.settings = settings;
		}

		// Token: 0x0600414C RID: 16716 RVA: 0x0013644C File Offset: 0x0013484C
		protected override void OnBuild()
		{
			this.settings.RuntimeData.CutLineSpheres = GPUCollidersManager.cutLineSpheresBuffer;
			this.settings.RuntimeData.GrowLineSpheres = GPUCollidersManager.growLineSpheresBuffer;
			this.settings.RuntimeData.HoldLineSpheres = GPUCollidersManager.holdLineSpheresBuffer;
			this.settings.RuntimeData.GrabLineSpheres = GPUCollidersManager.grabLineSpheresBuffer;
			this.settings.RuntimeData.PushLineSpheres = GPUCollidersManager.pushLineSpheresBuffer;
			this.settings.RuntimeData.PullLineSpheres = GPUCollidersManager.pullLineSpheresBuffer;
			this.settings.RuntimeData.BrushLineSpheres = GPUCollidersManager.brushLineSpheresBuffer;
			this.settings.RuntimeData.RigidityIncreaseLineSpheres = GPUCollidersManager.rigidityIncreaseLineSpheresBuffer;
			this.settings.RuntimeData.RigidityDecreaseLineSpheres = GPUCollidersManager.rigidityDecreaseLineSpheresBuffer;
			this.settings.RuntimeData.RigiditySetLineSpheres = GPUCollidersManager.rigiditySetLineSpheresBuffer;
		}

		// Token: 0x0600414D RID: 16717 RVA: 0x0013652C File Offset: 0x0013492C
		protected override void OnFixedDispatch()
		{
			this.settings.RuntimeData.CutLineSpheres = GPUCollidersManager.cutLineSpheresBuffer;
			this.settings.RuntimeData.GrowLineSpheres = GPUCollidersManager.growLineSpheresBuffer;
			this.settings.RuntimeData.HoldLineSpheres = GPUCollidersManager.holdLineSpheresBuffer;
			this.settings.RuntimeData.GrabLineSpheres = GPUCollidersManager.grabLineSpheresBuffer;
			this.settings.RuntimeData.PushLineSpheres = GPUCollidersManager.pushLineSpheresBuffer;
			this.settings.RuntimeData.PullLineSpheres = GPUCollidersManager.pullLineSpheresBuffer;
			this.settings.RuntimeData.BrushLineSpheres = GPUCollidersManager.brushLineSpheresBuffer;
			this.settings.RuntimeData.RigidityIncreaseLineSpheres = GPUCollidersManager.rigidityIncreaseLineSpheresBuffer;
			this.settings.RuntimeData.RigidityDecreaseLineSpheres = GPUCollidersManager.rigidityDecreaseLineSpheresBuffer;
			this.settings.RuntimeData.RigiditySetLineSpheres = GPUCollidersManager.rigiditySetLineSpheresBuffer;
		}

		// Token: 0x04003104 RID: 12548
		private readonly HairSettings settings;
	}
}
