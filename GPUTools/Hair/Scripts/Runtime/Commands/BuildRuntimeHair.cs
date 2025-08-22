using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Hair.Scripts.Runtime.Commands.Physics;
using GPUTools.Hair.Scripts.Runtime.Commands.Render;
using GPUTools.Hair.Scripts.Runtime.Data;
using GPUTools.Hair.Scripts.Runtime.Physics;
using GPUTools.Hair.Scripts.Runtime.Render;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Runtime.Commands
{
	// Token: 0x02000A0A RID: 2570
	public class BuildRuntimeHair : BuildChainCommand
	{
		// Token: 0x06004118 RID: 16664 RVA: 0x00135800 File Offset: 0x00133C00
		public BuildRuntimeHair(HairSettings settings)
		{
			this.settings = settings;
			this.particles = new BuildParticles(settings);
			base.Add(this.particles);
			this.planes = new BuildPlanes(settings);
			base.Add(this.planes);
			this.spheres = new BuildSpheres(settings);
			base.Add(this.spheres);
			this.lineSpheres = new BuildLineSpheres(settings);
			base.Add(this.lineSpheres);
			this.editLineSpheres = new BuildEditLineSpheres(settings);
			base.Add(this.editLineSpheres);
			this.distanceJoints = new BuildDistanceJoints(settings);
			base.Add(this.distanceJoints);
			this.compressionJoints = new BuildCompressionJoints(settings);
			base.Add(this.compressionJoints);
			this.nearbyDistanceJoints = new BuildNearbyDistanceJoints(settings);
			base.Add(this.nearbyDistanceJoints);
			this.pointJoints = new BuildPointJoints(settings);
			base.Add(this.pointJoints);
			base.Add(new BuildAccessories(settings));
			this.barycentrics = new BuildBarycentrics(settings);
			base.Add(this.barycentrics);
			this.particlesData = new BuildParticlesData(settings);
			base.Add(this.particlesData);
			this.tesselation = new BuildTesselation(settings);
			base.Add(this.tesselation);
		}

		// Token: 0x17000799 RID: 1945
		// (get) Token: 0x06004119 RID: 16665 RVA: 0x00135946 File Offset: 0x00133D46
		// (set) Token: 0x0600411A RID: 16666 RVA: 0x0013594E File Offset: 0x00133D4E
		public GPHairPhysics physics
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

		// Token: 0x1700079A RID: 1946
		// (get) Token: 0x0600411B RID: 16667 RVA: 0x00135957 File Offset: 0x00133D57
		// (set) Token: 0x0600411C RID: 16668 RVA: 0x0013595F File Offset: 0x00133D5F
		public HairRender render
		{
			[CompilerGenerated]
			get
			{
				return this.<render>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<render>k__BackingField = value;
			}
		}

		// Token: 0x1700079B RID: 1947
		// (get) Token: 0x0600411D RID: 16669 RVA: 0x00135968 File Offset: 0x00133D68
		// (set) Token: 0x0600411E RID: 16670 RVA: 0x00135970 File Offset: 0x00133D70
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

		// Token: 0x1700079C RID: 1948
		// (get) Token: 0x0600411F RID: 16671 RVA: 0x00135979 File Offset: 0x00133D79
		// (set) Token: 0x06004120 RID: 16672 RVA: 0x00135981 File Offset: 0x00133D81
		public BuildPlanes planes
		{
			[CompilerGenerated]
			get
			{
				return this.<planes>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<planes>k__BackingField = value;
			}
		}

		// Token: 0x1700079D RID: 1949
		// (get) Token: 0x06004121 RID: 16673 RVA: 0x0013598A File Offset: 0x00133D8A
		// (set) Token: 0x06004122 RID: 16674 RVA: 0x00135992 File Offset: 0x00133D92
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

		// Token: 0x1700079E RID: 1950
		// (get) Token: 0x06004123 RID: 16675 RVA: 0x0013599B File Offset: 0x00133D9B
		// (set) Token: 0x06004124 RID: 16676 RVA: 0x001359A3 File Offset: 0x00133DA3
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

		// Token: 0x1700079F RID: 1951
		// (get) Token: 0x06004125 RID: 16677 RVA: 0x001359AC File Offset: 0x00133DAC
		// (set) Token: 0x06004126 RID: 16678 RVA: 0x001359B4 File Offset: 0x00133DB4
		public BuildEditLineSpheres editLineSpheres
		{
			[CompilerGenerated]
			get
			{
				return this.<editLineSpheres>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<editLineSpheres>k__BackingField = value;
			}
		}

		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x06004127 RID: 16679 RVA: 0x001359BD File Offset: 0x00133DBD
		// (set) Token: 0x06004128 RID: 16680 RVA: 0x001359C5 File Offset: 0x00133DC5
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

		// Token: 0x170007A1 RID: 1953
		// (get) Token: 0x06004129 RID: 16681 RVA: 0x001359CE File Offset: 0x00133DCE
		// (set) Token: 0x0600412A RID: 16682 RVA: 0x001359D6 File Offset: 0x00133DD6
		public BuildParticlesData particlesData
		{
			[CompilerGenerated]
			get
			{
				return this.<particlesData>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<particlesData>k__BackingField = value;
			}
		}

		// Token: 0x170007A2 RID: 1954
		// (get) Token: 0x0600412B RID: 16683 RVA: 0x001359DF File Offset: 0x00133DDF
		// (set) Token: 0x0600412C RID: 16684 RVA: 0x001359E7 File Offset: 0x00133DE7
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

		// Token: 0x170007A3 RID: 1955
		// (get) Token: 0x0600412D RID: 16685 RVA: 0x001359F0 File Offset: 0x00133DF0
		// (set) Token: 0x0600412E RID: 16686 RVA: 0x001359F8 File Offset: 0x00133DF8
		public BuildCompressionJoints compressionJoints
		{
			[CompilerGenerated]
			get
			{
				return this.<compressionJoints>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<compressionJoints>k__BackingField = value;
			}
		}

		// Token: 0x170007A4 RID: 1956
		// (get) Token: 0x0600412F RID: 16687 RVA: 0x00135A01 File Offset: 0x00133E01
		// (set) Token: 0x06004130 RID: 16688 RVA: 0x00135A09 File Offset: 0x00133E09
		public BuildNearbyDistanceJoints nearbyDistanceJoints
		{
			[CompilerGenerated]
			get
			{
				return this.<nearbyDistanceJoints>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<nearbyDistanceJoints>k__BackingField = value;
			}
		}

		// Token: 0x170007A5 RID: 1957
		// (get) Token: 0x06004131 RID: 16689 RVA: 0x00135A12 File Offset: 0x00133E12
		// (set) Token: 0x06004132 RID: 16690 RVA: 0x00135A1A File Offset: 0x00133E1A
		public BuildTesselation tesselation
		{
			[CompilerGenerated]
			get
			{
				return this.<tesselation>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<tesselation>k__BackingField = value;
			}
		}

		// Token: 0x170007A6 RID: 1958
		// (get) Token: 0x06004133 RID: 16691 RVA: 0x00135A23 File Offset: 0x00133E23
		// (set) Token: 0x06004134 RID: 16692 RVA: 0x00135A2B File Offset: 0x00133E2B
		public BuildBarycentrics barycentrics
		{
			[CompilerGenerated]
			get
			{
				return this.<barycentrics>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<barycentrics>k__BackingField = value;
			}
		}

		// Token: 0x06004135 RID: 16693 RVA: 0x00135A34 File Offset: 0x00133E34
		public void RebuildHair()
		{
			this.particles.Build();
			this.distanceJoints.Build();
			this.compressionJoints.Build();
			this.nearbyDistanceJoints.Build();
			this.pointJoints.Build();
			this.barycentrics.Build();
			this.particlesData.Build();
			this.tesselation.Build();
			this.render.InitializeMesh();
			this.physics.RebindData();
			this.physics.ResetPhysics();
		}

		// Token: 0x06004136 RID: 16694 RVA: 0x00135ABC File Offset: 0x00133EBC
		protected override void OnBuild()
		{
			this.obj = new GameObject("Render");
			this.obj.layer = this.settings.gameObject.layer;
			this.obj.transform.SetParent(this.settings.transform, false);
			HairDataFacade data = new HairDataFacade(this.settings);
			this.physics = this.obj.AddComponent<GPHairPhysics>();
			this.physics.Initialize(data);
			this.render = this.obj.AddComponent<HairRender>();
			this.render.Initialize(data);
		}

		// Token: 0x06004137 RID: 16695 RVA: 0x00135B56 File Offset: 0x00133F56
		protected override void OnDispatch()
		{
			this.physics.Dispatch();
			this.render.Dispatch();
		}

		// Token: 0x06004138 RID: 16696 RVA: 0x00135B6E File Offset: 0x00133F6E
		protected override void OnFixedDispatch()
		{
			this.physics.FixedDispatch();
		}

		// Token: 0x06004139 RID: 16697 RVA: 0x00135B7B File Offset: 0x00133F7B
		protected override void OnDispose()
		{
			UnityEngine.Object.Destroy(this.obj);
		}

		// Token: 0x170007A7 RID: 1959
		// (get) Token: 0x0600413A RID: 16698 RVA: 0x00135B88 File Offset: 0x00133F88
		public bool IsVisible
		{
			get
			{
				return this.render.IsVisible;
			}
		}

		// Token: 0x040030EB RID: 12523
		private readonly HairSettings settings;

		// Token: 0x040030EC RID: 12524
		private GameObject obj;

		// Token: 0x040030ED RID: 12525
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GPHairPhysics <physics>k__BackingField;

		// Token: 0x040030EE RID: 12526
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private HairRender <render>k__BackingField;

		// Token: 0x040030EF RID: 12527
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private BuildParticles <particles>k__BackingField;

		// Token: 0x040030F0 RID: 12528
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private BuildPlanes <planes>k__BackingField;

		// Token: 0x040030F1 RID: 12529
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private BuildSpheres <spheres>k__BackingField;

		// Token: 0x040030F2 RID: 12530
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private BuildLineSpheres <lineSpheres>k__BackingField;

		// Token: 0x040030F3 RID: 12531
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private BuildEditLineSpheres <editLineSpheres>k__BackingField;

		// Token: 0x040030F4 RID: 12532
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private BuildPointJoints <pointJoints>k__BackingField;

		// Token: 0x040030F5 RID: 12533
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private BuildParticlesData <particlesData>k__BackingField;

		// Token: 0x040030F6 RID: 12534
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private BuildDistanceJoints <distanceJoints>k__BackingField;

		// Token: 0x040030F7 RID: 12535
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private BuildCompressionJoints <compressionJoints>k__BackingField;

		// Token: 0x040030F8 RID: 12536
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private BuildNearbyDistanceJoints <nearbyDistanceJoints>k__BackingField;

		// Token: 0x040030F9 RID: 12537
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private BuildTesselation <tesselation>k__BackingField;

		// Token: 0x040030FA RID: 12538
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private BuildBarycentrics <barycentrics>k__BackingField;
	}
}
