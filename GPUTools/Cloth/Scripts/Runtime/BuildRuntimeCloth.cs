using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GPUTools.Cloth.Scripts.Geometry.Passes;
using GPUTools.Cloth.Scripts.Runtime.Commands;
using GPUTools.Cloth.Scripts.Runtime.Data;
using GPUTools.Cloth.Scripts.Runtime.Physics;
using GPUTools.Cloth.Scripts.Runtime.Render;
using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Skinner.Scripts.Providers;
using UnityEngine;

namespace GPUTools.Cloth.Scripts.Runtime
{
	// Token: 0x02000995 RID: 2453
	public class BuildRuntimeCloth : BuildChainCommand
	{
		// Token: 0x06003D40 RID: 15680 RVA: 0x00129924 File Offset: 0x00127D24
		public BuildRuntimeCloth(ClothSettings settings)
		{
			this.settings = settings;
			this.physicsBlend = new BuildPhysicsBlend(settings);
			base.Add(this.physicsBlend);
			base.Add(new BuildPhysicsStrength(settings));
			this.particles = new BuildParticles(settings);
			base.Add(this.particles);
			base.Add(new BuildPlanes(settings));
			this.spheres = new BuildSpheres(settings);
			base.Add(this.spheres);
			this.grabSpheres = new BuildGrabSpheres(settings);
			base.Add(this.grabSpheres);
			this.lineSpheres = new BuildLineSpheres(settings);
			base.Add(this.lineSpheres);
			this.distanceJoints = new BuildDistanceJoints(settings);
			base.Add(this.distanceJoints);
			this.stiffnessJoints = new BuildStiffnessJoints(settings);
			base.Add(this.stiffnessJoints);
			this.nearbyJoints = new BuildNearbyJoints(settings);
			base.Add(this.nearbyJoints);
			this.pointJoints = new BuildPointJoints(settings);
			base.Add(this.pointJoints);
			base.Add(new BuildVertexData(settings));
			base.Add(new BuildClothAccessories(settings));
		}

		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x06003D41 RID: 15681 RVA: 0x00129A46 File Offset: 0x00127E46
		// (set) Token: 0x06003D42 RID: 15682 RVA: 0x00129A4E File Offset: 0x00127E4E
		public ClothPhysics physics
		{
			[CompilerGenerated]
			get
			{
				return this.<physics>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<physics>k__BackingField = value;
			}
		}

		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x06003D43 RID: 15683 RVA: 0x00129A57 File Offset: 0x00127E57
		// (set) Token: 0x06003D44 RID: 15684 RVA: 0x00129A5F File Offset: 0x00127E5F
		public BuildParticles particles
		{
			[CompilerGenerated]
			get
			{
				return this.<particles>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<particles>k__BackingField = value;
			}
		}

		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x06003D45 RID: 15685 RVA: 0x00129A68 File Offset: 0x00127E68
		// (set) Token: 0x06003D46 RID: 15686 RVA: 0x00129A70 File Offset: 0x00127E70
		public BuildSpheres spheres
		{
			[CompilerGenerated]
			get
			{
				return this.<spheres>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<spheres>k__BackingField = value;
			}
		}

		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x06003D47 RID: 15687 RVA: 0x00129A79 File Offset: 0x00127E79
		// (set) Token: 0x06003D48 RID: 15688 RVA: 0x00129A81 File Offset: 0x00127E81
		public BuildLineSpheres lineSpheres
		{
			[CompilerGenerated]
			get
			{
				return this.<lineSpheres>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<lineSpheres>k__BackingField = value;
			}
		}

		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x06003D49 RID: 15689 RVA: 0x00129A8A File Offset: 0x00127E8A
		// (set) Token: 0x06003D4A RID: 15690 RVA: 0x00129A92 File Offset: 0x00127E92
		public BuildPointJoints pointJoints
		{
			[CompilerGenerated]
			get
			{
				return this.<pointJoints>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<pointJoints>k__BackingField = value;
			}
		}

		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x06003D4B RID: 15691 RVA: 0x00129A9B File Offset: 0x00127E9B
		// (set) Token: 0x06003D4C RID: 15692 RVA: 0x00129AA3 File Offset: 0x00127EA3
		public BuildDistanceJoints distanceJoints
		{
			[CompilerGenerated]
			get
			{
				return this.<distanceJoints>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<distanceJoints>k__BackingField = value;
			}
		}

		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x06003D4D RID: 15693 RVA: 0x00129AAC File Offset: 0x00127EAC
		// (set) Token: 0x06003D4E RID: 15694 RVA: 0x00129AB4 File Offset: 0x00127EB4
		public BuildStiffnessJoints stiffnessJoints
		{
			[CompilerGenerated]
			get
			{
				return this.<stiffnessJoints>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<stiffnessJoints>k__BackingField = value;
			}
		}

		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x06003D4F RID: 15695 RVA: 0x00129ABD File Offset: 0x00127EBD
		// (set) Token: 0x06003D50 RID: 15696 RVA: 0x00129AC5 File Offset: 0x00127EC5
		public BuildNearbyJoints nearbyJoints
		{
			[CompilerGenerated]
			get
			{
				return this.<nearbyJoints>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<nearbyJoints>k__BackingField = value;
			}
		}

		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x06003D51 RID: 15697 RVA: 0x00129ACE File Offset: 0x00127ECE
		// (set) Token: 0x06003D52 RID: 15698 RVA: 0x00129AD6 File Offset: 0x00127ED6
		public BuildPhysicsBlend physicsBlend
		{
			[CompilerGenerated]
			get
			{
				return this.<physicsBlend>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<physicsBlend>k__BackingField = value;
			}
		}

		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x06003D53 RID: 15699 RVA: 0x00129ADF File Offset: 0x00127EDF
		// (set) Token: 0x06003D54 RID: 15700 RVA: 0x00129AE7 File Offset: 0x00127EE7
		public BuildGrabSpheres grabSpheres
		{
			[CompilerGenerated]
			get
			{
				return this.<grabSpheres>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<grabSpheres>k__BackingField = value;
			}
		}

		// Token: 0x06003D55 RID: 15701 RVA: 0x00129AF0 File Offset: 0x00127EF0
		protected override void OnBuild()
		{
			this.obj = this.settings.gameObject;
			this.obj.layer = this.settings.gameObject.layer;
			this.obj.transform.SetParent(this.settings.transform.parent, false);
			ClothDataFacade data = new ClothDataFacade(this.settings);
			this.physics = this.obj.AddComponent<ClothPhysics>();
			this.physics.Initialize(data);
			if (this.settings.MeshProvider.Type != ScalpMeshType.PreCalc)
			{
				this.render = this.obj.AddComponent<ClothRender>();
				this.render.Initialize(data);
			}
		}

		// Token: 0x06003D56 RID: 15702 RVA: 0x00129BA6 File Offset: 0x00127FA6
		protected override void OnUpdateSettings()
		{
			this.physics.ResetPhysics();
		}

		// Token: 0x06003D57 RID: 15703 RVA: 0x00129BB3 File Offset: 0x00127FB3
		public void DispatchCopyToOld()
		{
			this.physics.DispatchCopyToOld();
		}

		// Token: 0x06003D58 RID: 15704 RVA: 0x00129BC0 File Offset: 0x00127FC0
		protected override void OnDispatch()
		{
			this.physics.Dispatch();
		}

		// Token: 0x06003D59 RID: 15705 RVA: 0x00129BCD File Offset: 0x00127FCD
		protected override void OnFixedDispatch()
		{
			this.physics.FixedDispatch();
		}

		// Token: 0x04002F1A RID: 12058
		private readonly ClothSettings settings;

		// Token: 0x04002F1B RID: 12059
		private GameObject obj;

		// Token: 0x04002F1C RID: 12060
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private ClothPhysics <physics>k__BackingField;

		// Token: 0x04002F1D RID: 12061
		private ClothRender render;

		// Token: 0x04002F1E RID: 12062
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private BuildParticles <particles>k__BackingField;

		// Token: 0x04002F1F RID: 12063
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private BuildSpheres <spheres>k__BackingField;

		// Token: 0x04002F20 RID: 12064
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private BuildLineSpheres <lineSpheres>k__BackingField;

		// Token: 0x04002F21 RID: 12065
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private BuildPointJoints <pointJoints>k__BackingField;

		// Token: 0x04002F22 RID: 12066
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private BuildDistanceJoints <distanceJoints>k__BackingField;

		// Token: 0x04002F23 RID: 12067
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private BuildStiffnessJoints <stiffnessJoints>k__BackingField;

		// Token: 0x04002F24 RID: 12068
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private BuildNearbyJoints <nearbyJoints>k__BackingField;

		// Token: 0x04002F25 RID: 12069
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private BuildPhysicsBlend <physicsBlend>k__BackingField;

		// Token: 0x04002F26 RID: 12070
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private BuildGrabSpheres <grabSpheres>k__BackingField;
	}
}
