using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GPUTools.Cloth.Scripts.Geometry;
using GPUTools.Cloth.Scripts.Geometry.Data;
using GPUTools.Cloth.Scripts.Geometry.DebugDraw;
using GPUTools.Cloth.Scripts.Geometry.Tools;
using GPUTools.Cloth.Scripts.Runtime;
using GPUTools.Cloth.Scripts.Runtime.Data;
using GPUTools.Painter.Scripts;
using GPUTools.Skinner.Scripts.Providers;
using UnityEngine;

namespace GPUTools.Cloth.Scripts
{
	// Token: 0x02000987 RID: 2439
	public class ClothSettings : GPUCollidersConsumer
	{
		// Token: 0x06003CD7 RID: 15575 RVA: 0x00126C18 File Offset: 0x00125018
		public ClothSettings()
		{
		}

		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x06003CD8 RID: 15576 RVA: 0x00126D44 File Offset: 0x00125144
		public Material Material
		{
			get
			{
				return base.GetComponent<Renderer>().material;
			}
		}

		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x06003CD9 RID: 15577 RVA: 0x00126D51 File Offset: 0x00125151
		public Material SharedMaterial
		{
			get
			{
				return base.GetComponent<Renderer>().sharedMaterial;
			}
		}

		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x06003CDA RID: 15578 RVA: 0x00126D5E File Offset: 0x0012515E
		public Material[] Materials
		{
			get
			{
				return base.GetComponent<Renderer>().materials;
			}
		}

		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x06003CDB RID: 15579 RVA: 0x00126D6B File Offset: 0x0012516B
		public Material[] SharedMaterials
		{
			get
			{
				return base.GetComponent<Renderer>().sharedMaterials;
			}
		}

		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x06003CDD RID: 15581 RVA: 0x00126D81 File Offset: 0x00125181
		// (set) Token: 0x06003CDC RID: 15580 RVA: 0x00126D78 File Offset: 0x00125178
		public RuntimeData Runtime
		{
			[CompilerGenerated]
			get
			{
				return this.<Runtime>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Runtime>k__BackingField = value;
			}
		}

		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x06003CDF RID: 15583 RVA: 0x00126D92 File Offset: 0x00125192
		// (set) Token: 0x06003CDE RID: 15582 RVA: 0x00126D89 File Offset: 0x00125189
		public BuildRuntimeCloth builder
		{
			[CompilerGenerated]
			get
			{
				return this.<builder>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<builder>k__BackingField = value;
			}
		}

		// Token: 0x06003CE0 RID: 15584 RVA: 0x00126D9A File Offset: 0x0012519A
		public void ProcessGeometry()
		{
			this.ProcessGeometryMainThread();
			this.ProcessGeometryThreaded();
		}

		// Token: 0x06003CE1 RID: 15585 RVA: 0x00126DA8 File Offset: 0x001251A8
		public void ProcessGeometryMainThread()
		{
			this.GeometryData = new ClothGeometryData();
			this.geometryImporter = new ClothGeometryImporter(this);
			this.geometryImporter.Cache();
		}

		// Token: 0x06003CE2 RID: 15586 RVA: 0x00126DCC File Offset: 0x001251CC
		public void CancelProcessGeometryThreaded()
		{
			if (this.geometryImporter != null)
			{
				this.geometryImporter.CancelCacheThreaded();
			}
		}

		// Token: 0x06003CE3 RID: 15587 RVA: 0x00126DE4 File Offset: 0x001251E4
		public void ProcessGeometryThreaded()
		{
			if (this.geometryImporter != null)
			{
				this.geometryImporter.CacheThreaded();
			}
			this.GeometryData.LogStatistics();
		}

		// Token: 0x06003CE4 RID: 15588 RVA: 0x00126E08 File Offset: 0x00125208
		private void Init()
		{
			if (!this._wasInit && this.MeshProvider.Validate(false) && this.GeometryData != null && this.GeometryData.IsProcessed)
			{
				this.MeshProvider.Dispatch();
				if (this.Runtime == null)
				{
					this.Runtime = new RuntimeData();
				}
				if (this.builder == null)
				{
					this.builder = new BuildRuntimeCloth(this);
				}
				this.builder.Build();
				this._wasInit = true;
			}
		}

		// Token: 0x06003CE5 RID: 15589 RVA: 0x00126E98 File Offset: 0x00125298
		public void Reset()
		{
			if (this.builder != null)
			{
				if (this.builder.particles != null)
				{
					this.builder.particles.UpdateSettings();
				}
				if (this.builder.distanceJoints != null)
				{
					this.builder.distanceJoints.UpdateSettings();
				}
				if (this.builder.pointJoints != null)
				{
					this.builder.pointJoints.UpdateSettings();
				}
			}
		}

		// Token: 0x06003CE6 RID: 15590 RVA: 0x00126F10 File Offset: 0x00125310
		private void Start()
		{
			this.Init();
		}

		// Token: 0x06003CE7 RID: 15591 RVA: 0x00126F18 File Offset: 0x00125318
		private void FixedUpdate()
		{
			if (this._wasInit)
			{
				this.builder.FixedDispatch();
			}
		}

		// Token: 0x06003CE8 RID: 15592 RVA: 0x00126F30 File Offset: 0x00125330
		private void LateUpdate()
		{
			this.Init();
			if (this._wasInit)
			{
				this.builder.DispatchCopyToOld();
				this.MeshProvider.Dispatch();
				this.builder.Dispatch();
				if (this.MeshProvider.Type == ScalpMeshType.PreCalc && this.MeshProvider.PreCalcProvider != null)
				{
					this.MeshProvider.PreCalcProvider.PostProcessDispatch(this.Runtime.ClothOnlyVertices.ComputeBuffer);
				}
			}
		}

		// Token: 0x06003CE9 RID: 15593 RVA: 0x00126FB6 File Offset: 0x001253B6
		private void OnDestroy()
		{
			this.MeshProvider.Dispose();
			if (this.builder != null)
			{
				this.builder.Dispose();
			}
		}

		// Token: 0x06003CEA RID: 15594 RVA: 0x00126FD9 File Offset: 0x001253D9
		protected override void OnDisable()
		{
			base.OnDisable();
			this.MeshProvider.Stop();
		}

		// Token: 0x06003CEB RID: 15595 RVA: 0x00126FEC File Offset: 0x001253EC
		public void UpdateSettings()
		{
			if (Application.isPlaying && this.builder != null)
			{
				this.builder.UpdateSettings();
			}
		}

		// Token: 0x06003CEC RID: 15596 RVA: 0x0012700E File Offset: 0x0012540E
		public void OnDrawGizmos()
		{
			if (this.GeometryDebugDraw)
			{
				ClothDebugDraw.Draw(this);
			}
			ClothDebugDraw.DrawAlways(this);
		}

		// Token: 0x04002EB6 RID: 11958
		[SerializeField]
		public bool GeometryDebugDraw;

		// Token: 0x04002EB7 RID: 11959
		[SerializeField]
		public bool GeometryDebugDrawDistanceJoints = true;

		// Token: 0x04002EB8 RID: 11960
		[SerializeField]
		public bool GeometryDebugDrawStiffnessJoints = true;

		// Token: 0x04002EB9 RID: 11961
		[SerializeField]
		public ClothEditorType EditorType;

		// Token: 0x04002EBA RID: 11962
		[SerializeField]
		public ClothEditorType EditorStrengthType;

		// Token: 0x04002EBB RID: 11963
		[SerializeField]
		public Texture2D EditorTexture;

		// Token: 0x04002EBC RID: 11964
		[SerializeField]
		public Texture2D EditorStrengthTexture;

		// Token: 0x04002EBD RID: 11965
		[SerializeField]
		public PainterSettings EditorPainter;

		// Token: 0x04002EBE RID: 11966
		[SerializeField]
		public PainterSettings EditorStrengthPainter;

		// Token: 0x04002EBF RID: 11967
		[SerializeField]
		public ColorChannel SimulateVsKinematicChannel;

		// Token: 0x04002EC0 RID: 11968
		[SerializeField]
		public ColorChannel StrengthChannel;

		// Token: 0x04002EC1 RID: 11969
		[SerializeField]
		public bool PhysicsDebugDraw;

		// Token: 0x04002EC2 RID: 11970
		[SerializeField]
		public bool IntegrateEnabled = true;

		// Token: 0x04002EC3 RID: 11971
		[SerializeField]
		public Vector3 Gravity = new Vector3(0f, -1f, 0f);

		// Token: 0x04002EC4 RID: 11972
		[SerializeField]
		public float Drag = 0.06f;

		// Token: 0x04002EC5 RID: 11973
		[SerializeField]
		public float Stretchability;

		// Token: 0x04002EC6 RID: 11974
		[SerializeField]
		public float Stiffness = 0.5f;

		// Token: 0x04002EC7 RID: 11975
		[SerializeField]
		public float DistanceScale = 1f;

		// Token: 0x04002EC8 RID: 11976
		[SerializeField]
		public float WorldScale = 1f;

		// Token: 0x04002EC9 RID: 11977
		[SerializeField]
		public float CompressionResistance = 0.5f;

		// Token: 0x04002ECA RID: 11978
		[SerializeField]
		public float Weight = 1f;

		// Token: 0x04002ECB RID: 11979
		[SerializeField]
		public float Friction = 0.5f;

		// Token: 0x04002ECC RID: 11980
		[SerializeField]
		public float StaticMultiplier = 2f;

		// Token: 0x04002ECD RID: 11981
		[SerializeField]
		public bool CreateNearbyJoints;

		// Token: 0x04002ECE RID: 11982
		[SerializeField]
		public float NearbyJointsMaxDistance = 0.001f;

		// Token: 0x04002ECF RID: 11983
		[SerializeField]
		public bool CollisionEnabled = true;

		// Token: 0x04002ED0 RID: 11984
		[SerializeField]
		public float CollisionPower = 0.5f;

		// Token: 0x04002ED1 RID: 11985
		[SerializeField]
		public float GravityMultiplier = 1f;

		// Token: 0x04002ED2 RID: 11986
		[SerializeField]
		public float ParticleRadius = 0.01f;

		// Token: 0x04002ED3 RID: 11987
		[SerializeField]
		public bool BreakEnabled;

		// Token: 0x04002ED4 RID: 11988
		[SerializeField]
		public float BreakThreshold = 0.005f;

		// Token: 0x04002ED5 RID: 11989
		[SerializeField]
		public float JointStrength = 1f;

		// Token: 0x04002ED6 RID: 11990
		[SerializeField]
		public int Iterations = 3;

		// Token: 0x04002ED7 RID: 11991
		[SerializeField]
		public int InnerIterations = 2;

		// Token: 0x04002ED8 RID: 11992
		[SerializeField]
		public float WindMultiplier;

		// Token: 0x04002ED9 RID: 11993
		[SerializeField]
		public List<GameObject> ColliderProviders = new List<GameObject>();

		// Token: 0x04002EDA RID: 11994
		[SerializeField]
		public List<GameObject> AccessoriesProviders = new List<GameObject>();

		// Token: 0x04002EDB RID: 11995
		[SerializeField]
		public MeshProvider MeshProvider = new MeshProvider();

		// Token: 0x04002EDC RID: 11996
		[SerializeField]
		public ClothSphereBrash Brush = new ClothSphereBrash();

		// Token: 0x04002EDD RID: 11997
		public bool CustomBounds;

		// Token: 0x04002EDE RID: 11998
		public Bounds Bounds = default(Bounds);

		// Token: 0x04002EDF RID: 11999
		[SerializeField]
		public ClothGeometryData GeometryData;

		// Token: 0x04002EE0 RID: 12000
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private RuntimeData <Runtime>k__BackingField;

		// Token: 0x04002EE1 RID: 12001
		private ClothGeometryImporter geometryImporter;

		// Token: 0x04002EE2 RID: 12002
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private BuildRuntimeCloth <builder>k__BackingField;

		// Token: 0x04002EE3 RID: 12003
		private bool _wasInit;
	}
}
