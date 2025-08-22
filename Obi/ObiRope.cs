using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Obi
{
	// Token: 0x02000393 RID: 915
	[ExecuteInEditMode]
	[AddComponentMenu("Physics/Obi/Obi Rope")]
	[RequireComponent(typeof(MeshRenderer))]
	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(ObiDistanceConstraints))]
	[RequireComponent(typeof(ObiBendingConstraints))]
	[RequireComponent(typeof(ObiTetherConstraints))]
	[RequireComponent(typeof(ObiPinConstraints))]
	[DisallowMultipleComponent]
	public class ObiRope : ObiActor
	{
		// Token: 0x060016F7 RID: 5879 RVA: 0x00081D28 File Offset: 0x00080128
		public ObiRope()
		{
		}

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x060016F8 RID: 5880 RVA: 0x00081DEC File Offset: 0x000801EC
		public ObiDistanceConstraints DistanceConstraints
		{
			get
			{
				return this.constraints[Oni.ConstraintType.Distance] as ObiDistanceConstraints;
			}
		}

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x060016F9 RID: 5881 RVA: 0x00081DFF File Offset: 0x000801FF
		public ObiBendingConstraints BendingConstraints
		{
			get
			{
				return this.constraints[Oni.ConstraintType.Bending] as ObiBendingConstraints;
			}
		}

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x060016FA RID: 5882 RVA: 0x00081E12 File Offset: 0x00080212
		public ObiTetherConstraints TetherConstraints
		{
			get
			{
				return this.constraints[Oni.ConstraintType.Tether] as ObiTetherConstraints;
			}
		}

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x060016FB RID: 5883 RVA: 0x00081E25 File Offset: 0x00080225
		public ObiPinConstraints PinConstraints
		{
			get
			{
				return this.constraints[Oni.ConstraintType.Pin] as ObiPinConstraints;
			}
		}

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x060016FD RID: 5885 RVA: 0x00081E64 File Offset: 0x00080264
		// (set) Token: 0x060016FC RID: 5884 RVA: 0x00081E38 File Offset: 0x00080238
		public ObiRope.RenderingMode RenderMode
		{
			get
			{
				return this.renderMode;
			}
			set
			{
				if (value != this.renderMode)
				{
					this.renderMode = value;
					this.ClearChainLinkInstances();
					UnityEngine.Object.DestroyImmediate(this.ropeMesh);
					this.GenerateVisualRepresentation();
				}
			}
		}

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x060016FF RID: 5887 RVA: 0x00081E8C File Offset: 0x0008028C
		// (set) Token: 0x060016FE RID: 5886 RVA: 0x00081E6C File Offset: 0x0008026C
		public ObiRopeSection Section
		{
			get
			{
				return this.section;
			}
			set
			{
				if (value != this.section)
				{
					this.section = value;
					this.GenerateProceduralRopeMesh();
				}
			}
		}

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06001701 RID: 5889 RVA: 0x00081EB9 File Offset: 0x000802B9
		// (set) Token: 0x06001700 RID: 5888 RVA: 0x00081E94 File Offset: 0x00080294
		public float SectionThicknessScale
		{
			get
			{
				return this.sectionThicknessScale;
			}
			set
			{
				if (value != this.sectionThicknessScale)
				{
					this.sectionThicknessScale = Mathf.Max(0f, value);
					this.UpdateProceduralRopeMesh();
				}
			}
		}

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x06001703 RID: 5891 RVA: 0x00081EDC File Offset: 0x000802DC
		// (set) Token: 0x06001702 RID: 5890 RVA: 0x00081EC1 File Offset: 0x000802C1
		public bool ThicknessFromParticles
		{
			get
			{
				return this.thicknessFromParticles;
			}
			set
			{
				if (value != this.thicknessFromParticles)
				{
					this.thicknessFromParticles = value;
					this.UpdateVisualRepresentation();
				}
			}
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06001705 RID: 5893 RVA: 0x00081EFF File Offset: 0x000802FF
		// (set) Token: 0x06001704 RID: 5892 RVA: 0x00081EE4 File Offset: 0x000802E4
		public float SectionTwist
		{
			get
			{
				return this.sectionTwist;
			}
			set
			{
				if (value != this.sectionTwist)
				{
					this.sectionTwist = value;
					this.UpdateVisualRepresentation();
				}
			}
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06001707 RID: 5895 RVA: 0x00081F22 File Offset: 0x00080322
		// (set) Token: 0x06001706 RID: 5894 RVA: 0x00081F07 File Offset: 0x00080307
		public uint Smoothing
		{
			get
			{
				return this.smoothing;
			}
			set
			{
				if (value != this.smoothing)
				{
					this.smoothing = value;
					this.UpdateProceduralRopeMesh();
				}
			}
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06001709 RID: 5897 RVA: 0x00081F4A File Offset: 0x0008034A
		// (set) Token: 0x06001708 RID: 5896 RVA: 0x00081F2A File Offset: 0x0008032A
		public Vector3 LinkScale
		{
			get
			{
				return this.linkScale;
			}
			set
			{
				if (value != this.linkScale)
				{
					this.linkScale = value;
					this.UpdateProceduralChainLinks();
				}
			}
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x0600170B RID: 5899 RVA: 0x00081F72 File Offset: 0x00080372
		// (set) Token: 0x0600170A RID: 5898 RVA: 0x00081F52 File Offset: 0x00080352
		public Vector2 UVScale
		{
			get
			{
				return this.uvScale;
			}
			set
			{
				if (value != this.uvScale)
				{
					this.uvScale = value;
					this.UpdateProceduralRopeMesh();
				}
			}
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x0600170D RID: 5901 RVA: 0x00081F95 File Offset: 0x00080395
		// (set) Token: 0x0600170C RID: 5900 RVA: 0x00081F7A File Offset: 0x0008037A
		public float UVAnchor
		{
			get
			{
				return this.uvAnchor;
			}
			set
			{
				if (value != this.uvAnchor)
				{
					this.uvAnchor = value;
					this.UpdateProceduralRopeMesh();
				}
			}
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x0600170F RID: 5903 RVA: 0x00081FB8 File Offset: 0x000803B8
		// (set) Token: 0x0600170E RID: 5902 RVA: 0x00081F9D File Offset: 0x0008039D
		public bool NormalizeV
		{
			get
			{
				return this.normalizeV;
			}
			set
			{
				if (value != this.normalizeV)
				{
					this.normalizeV = value;
					this.UpdateProceduralRopeMesh();
				}
			}
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06001711 RID: 5905 RVA: 0x00081FDB File Offset: 0x000803DB
		// (set) Token: 0x06001710 RID: 5904 RVA: 0x00081FC0 File Offset: 0x000803C0
		public bool RandomizeLinks
		{
			get
			{
				return this.randomizeLinks;
			}
			set
			{
				if (value != this.randomizeLinks)
				{
					this.randomizeLinks = value;
					this.GenerateProceduralChainLinks();
				}
			}
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06001712 RID: 5906 RVA: 0x00081FE3 File Offset: 0x000803E3
		public float InterparticleDistance
		{
			get
			{
				return this.interParticleDistance * this.DistanceConstraints.stretchingScale;
			}
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06001713 RID: 5907 RVA: 0x00081FF7 File Offset: 0x000803F7
		public int TotalParticles
		{
			get
			{
				return this.totalParticles;
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06001714 RID: 5908 RVA: 0x00081FFF File Offset: 0x000803FF
		// (set) Token: 0x06001715 RID: 5909 RVA: 0x00082007 File Offset: 0x00080407
		public int UsedParticles
		{
			get
			{
				return this.usedParticles;
			}
			set
			{
				this.usedParticles = value;
				this.pooledParticles = this.totalParticles - this.usedParticles;
			}
		}

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06001716 RID: 5910 RVA: 0x00082023 File Offset: 0x00080423
		// (set) Token: 0x06001717 RID: 5911 RVA: 0x0008202B File Offset: 0x0008042B
		public float RestLength
		{
			get
			{
				return this.restLength;
			}
			set
			{
				this.restLength = value;
			}
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06001718 RID: 5912 RVA: 0x00082034 File Offset: 0x00080434
		public bool Closed
		{
			get
			{
				return this.closed;
			}
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06001719 RID: 5913 RVA: 0x0008203C File Offset: 0x0008043C
		public int PooledParticles
		{
			get
			{
				return this.pooledParticles;
			}
		}

		// Token: 0x0600171A RID: 5914 RVA: 0x00082044 File Offset: 0x00080444
		public override void Awake()
		{
			base.Awake();
			this.linkInstances = new List<GameObject>();
			this.meshFilter = base.GetComponent<MeshFilter>();
		}

		// Token: 0x0600171B RID: 5915 RVA: 0x00082064 File Offset: 0x00080464
		public void OnValidate()
		{
			this.thickness = Mathf.Max(0.0001f, this.thickness);
			this.uvAnchor = Mathf.Clamp01(this.uvAnchor);
			this.tearResistanceMultiplier = Mathf.Max(0.1f, this.tearResistanceMultiplier);
			this.resolution = Mathf.Max(0.0001f, this.resolution);
		}

		// Token: 0x0600171C RID: 5916 RVA: 0x000820C4 File Offset: 0x000804C4
		public override void OnEnable()
		{
			base.OnEnable();
			Camera.onPreCull = (Camera.CameraCallback)Delegate.Combine(Camera.onPreCull, new Camera.CameraCallback(this.RopePreCull));
			this.GenerateVisualRepresentation();
		}

		// Token: 0x0600171D RID: 5917 RVA: 0x000820F2 File Offset: 0x000804F2
		public override void OnDisable()
		{
			base.OnDisable();
			Camera.onPreCull = (Camera.CameraCallback)Delegate.Remove(Camera.onPreCull, new Camera.CameraCallback(this.RopePreCull));
		}

		// Token: 0x0600171E RID: 5918 RVA: 0x0008211A File Offset: 0x0008051A
		public void RopePreCull(Camera cam)
		{
			if (this.renderMode == ObiRope.RenderingMode.Line)
			{
				this.UpdateLineMesh(cam);
			}
		}

		// Token: 0x0600171F RID: 5919 RVA: 0x00082130 File Offset: 0x00080530
		public override void OnSolverStepEnd()
		{
			base.OnSolverStepEnd();
			if (base.isActiveAndEnabled)
			{
				this.ApplyTearing();
				if (this.PinConstraints.GetBatches().Count > 0)
				{
					((ObiPinConstraintBatch)this.PinConstraints.GetBatches()[0]).BreakConstraints();
				}
			}
		}

		// Token: 0x06001720 RID: 5920 RVA: 0x00082185 File Offset: 0x00080585
		public override void OnSolverFrameEnd()
		{
			base.OnSolverFrameEnd();
			this.UpdateVisualRepresentation();
		}

		// Token: 0x06001721 RID: 5921 RVA: 0x00082193 File Offset: 0x00080593
		public override void OnDestroy()
		{
			base.OnDestroy();
			UnityEngine.Object.DestroyImmediate(this.ropeMesh);
			this.ClearChainLinkInstances();
			this.ClearPrefabInstances();
		}

		// Token: 0x06001722 RID: 5922 RVA: 0x000821B2 File Offset: 0x000805B2
		public override bool AddToSolver(object info)
		{
			if (base.Initialized && base.AddToSolver(info))
			{
				this.solver.RequireRenderablePositions();
				return true;
			}
			return false;
		}

		// Token: 0x06001723 RID: 5923 RVA: 0x000821D9 File Offset: 0x000805D9
		public override bool RemoveFromSolver(object info)
		{
			if (this.solver != null)
			{
				this.solver.RelinquishRenderablePositions();
			}
			return base.RemoveFromSolver(info);
		}

		// Token: 0x06001724 RID: 5924 RVA: 0x00082200 File Offset: 0x00080600
		public IEnumerator GeneratePhysicRepresentationForMesh()
		{
			this.initialized = false;
			this.initializing = true;
			this.interParticleDistance = -1f;
			this.RemoveFromSolver(null);
			if (this.ropePath == null)
			{
				UnityEngine.Debug.LogError("Cannot initialize rope. There's no ropePath present. Please provide a spline to define the shape of the rope");
				yield break;
			}
			this.ropePath.RecalculateSplineLenght(1E-05f, 7);
			this.closed = this.ropePath.Closed;
			this.restLength = this.ropePath.Length;
			this.usedParticles = Mathf.CeilToInt(this.restLength / this.thickness * this.resolution) + ((!this.closed) ? 1 : 0);
			this.totalParticles = this.usedParticles + this.pooledParticles;
			this.active = new bool[this.totalParticles];
			this.positions = new Vector3[this.totalParticles];
			this.velocities = new Vector3[this.totalParticles];
			this.invMasses = new float[this.totalParticles];
			this.solidRadii = new float[this.totalParticles];
			this.phases = new int[this.totalParticles];
			this.restPositions = new Vector4[this.totalParticles];
			this.tearResistance = new float[this.totalParticles];
			int numSegments = this.usedParticles - ((!this.closed) ? 1 : 0);
			if (numSegments > 0)
			{
				this.interParticleDistance = this.restLength / (float)numSegments;
			}
			else
			{
				this.interParticleDistance = 0f;
			}
			float radius = this.interParticleDistance * this.resolution;
			for (int i = 0; i < this.usedParticles; i++)
			{
				this.active[i] = true;
				this.invMasses[i] = 10f;
				float mu = this.ropePath.GetMuAtLenght(this.interParticleDistance * (float)i);
				this.positions[i] = base.transform.InverseTransformPoint(this.ropePath.transform.TransformPoint(this.ropePath.GetPositionAt(mu)));
				this.solidRadii[i] = radius;
				this.phases[i] = Oni.MakePhase(1, (!this.selfCollisions) ? ((Oni.ParticlePhase)0) : Oni.ParticlePhase.SelfCollide);
				this.tearResistance[i] = 1f;
				if (i % 100 == 0)
				{
					yield return new CoroutineJob.ProgressInfo("ObiRope: generating particles...", (float)i / (float)this.usedParticles);
				}
			}
			for (int j = this.usedParticles; j < this.totalParticles; j++)
			{
				this.active[j] = false;
				this.invMasses[j] = 10f;
				this.solidRadii[j] = radius;
				this.phases[j] = Oni.MakePhase(1, (!this.selfCollisions) ? ((Oni.ParticlePhase)0) : Oni.ParticlePhase.SelfCollide);
				this.tearResistance[j] = 1f;
				if (j % 100 == 0)
				{
					yield return new CoroutineJob.ProgressInfo("ObiRope: generating particles...", (float)j / (float)this.usedParticles);
				}
			}
			this.DistanceConstraints.Clear();
			ObiDistanceConstraintBatch distanceBatch = new ObiDistanceConstraintBatch(false, false, 0.0001f, 200f);
			this.DistanceConstraints.AddBatch(distanceBatch);
			for (int k = 0; k < numSegments; k++)
			{
				distanceBatch.AddConstraint(k, (k + 1) % ((!this.ropePath.Closed) ? (this.usedParticles + 1) : this.usedParticles), this.interParticleDistance, 1f, 1f);
				if (k % 500 == 0)
				{
					yield return new CoroutineJob.ProgressInfo("ObiRope: generating structural constraints...", (float)k / (float)numSegments);
				}
			}
			this.BendingConstraints.Clear();
			ObiBendConstraintBatch bendingBatch = new ObiBendConstraintBatch(false, false, 0.0001f, 200f);
			this.BendingConstraints.AddBatch(bendingBatch);
			for (int l = 0; l < this.usedParticles - ((!this.closed) ? 2 : 0); l++)
			{
				bendingBatch.AddConstraint(l, (l + 2) % this.usedParticles, (l + 1) % this.usedParticles, 0f, 0f, 1f);
				if (l % 500 == 0)
				{
					yield return new CoroutineJob.ProgressInfo("ObiRope: adding bend constraints...", (float)l / (float)this.usedParticles);
				}
			}
			this.TetherConstraints.Clear();
			this.PinConstraints.Clear();
			ObiPinConstraintBatch pinBatch = new ObiPinConstraintBatch(false, false, 0f, 200f);
			this.PinConstraints.AddBatch(pinBatch);
			this.initializing = false;
			this.initialized = true;
			this.RegenerateRestPositions();
			this.GenerateVisualRepresentation();
			yield break;
		}

		// Token: 0x06001725 RID: 5925 RVA: 0x0008221C File Offset: 0x0008061C
		public void RegenerateRestPositions()
		{
			ObiDistanceConstraintBatch obiDistanceConstraintBatch = this.DistanceConstraints.GetBatches()[0] as ObiDistanceConstraintBatch;
			int num = -1;
			int num2 = -1;
			float num3 = 0f;
			for (int i = 0; i < obiDistanceConstraintBatch.ConstraintCount; i++)
			{
				if (i == 0)
				{
					num = (num2 = obiDistanceConstraintBatch.springIndices[i * 2]);
					this.restPositions[num] = Vector4.zero;
				}
				num3 += Mathf.Min(new float[]
				{
					this.interParticleDistance,
					this.solidRadii[num],
					this.solidRadii[num2]
				});
				num = obiDistanceConstraintBatch.springIndices[i * 2 + 1];
				this.restPositions[num] = Vector3.right * num3;
				this.restPositions[num][3] = 0f;
			}
			this.PushDataToSolver(ParticleData.REST_POSITIONS);
		}

		// Token: 0x06001726 RID: 5926 RVA: 0x00082318 File Offset: 0x00080718
		public void RecalculateLenght()
		{
			ObiDistanceConstraintBatch obiDistanceConstraintBatch = this.DistanceConstraints.GetBatches()[0] as ObiDistanceConstraintBatch;
			this.restLength = 0f;
			for (int i = 0; i < obiDistanceConstraintBatch.ConstraintCount; i++)
			{
				this.restLength += obiDistanceConstraintBatch.restLengths[i];
			}
		}

		// Token: 0x06001727 RID: 5927 RVA: 0x00082378 File Offset: 0x00080778
		public float CalculateLength()
		{
			ObiDistanceConstraintBatch obiDistanceConstraintBatch = this.DistanceConstraints.GetBatches()[0] as ObiDistanceConstraintBatch;
			float num = 0f;
			for (int i = 0; i < obiDistanceConstraintBatch.ConstraintCount; i++)
			{
				Vector3 particlePosition = base.GetParticlePosition(obiDistanceConstraintBatch.springIndices[i * 2]);
				Vector3 particlePosition2 = base.GetParticlePosition(obiDistanceConstraintBatch.springIndices[i * 2 + 1]);
				num += Vector3.Distance(particlePosition, particlePosition2);
			}
			return num;
		}

		// Token: 0x06001728 RID: 5928 RVA: 0x000823F6 File Offset: 0x000807F6
		public void GenerateVisualRepresentation()
		{
			this.GeneratePrefabInstances();
			if (this.renderMode != ObiRope.RenderingMode.Chain)
			{
				this.GenerateProceduralRopeMesh();
			}
			else
			{
				this.GenerateProceduralChainLinks();
			}
		}

		// Token: 0x06001729 RID: 5929 RVA: 0x0008241B File Offset: 0x0008081B
		public void UpdateVisualRepresentation()
		{
			if (this.renderMode != ObiRope.RenderingMode.Chain)
			{
				this.UpdateProceduralRopeMesh();
			}
			else
			{
				this.UpdateProceduralChainLinks();
			}
		}

		// Token: 0x0600172A RID: 5930 RVA: 0x0008243C File Offset: 0x0008083C
		private void GenerateProceduralRopeMesh()
		{
			if (!this.initialized)
			{
				return;
			}
			UnityEngine.Object.DestroyImmediate(this.ropeMesh);
			this.ropeMesh = new Mesh();
			this.ropeMesh.MarkDynamic();
			this.meshFilter.mesh = this.ropeMesh;
			this.UpdateProceduralRopeMesh();
		}

		// Token: 0x0600172B RID: 5931 RVA: 0x00082490 File Offset: 0x00080890
		private void GeneratePrefabInstances()
		{
			this.ClearPrefabInstances();
			if (this.tearPrefab != null)
			{
				this.tearPrefabPool = new GameObject[this.pooledParticles * 2];
				for (int i = 0; i < this.tearPrefabPool.Length; i++)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.tearPrefab);
					gameObject.hideFlags = HideFlags.HideAndDontSave;
					gameObject.SetActive(false);
					this.tearPrefabPool[i] = gameObject;
				}
			}
			if (this.startPrefabInstance == null && this.startPrefab != null)
			{
				this.startPrefabInstance = UnityEngine.Object.Instantiate<GameObject>(this.startPrefab);
				this.startPrefabInstance.hideFlags = HideFlags.HideAndDontSave;
			}
			if (this.endPrefabInstance == null && this.endPrefab != null)
			{
				this.endPrefabInstance = UnityEngine.Object.Instantiate<GameObject>(this.endPrefab);
				this.endPrefabInstance.hideFlags = HideFlags.HideAndDontSave;
			}
		}

		// Token: 0x0600172C RID: 5932 RVA: 0x00082584 File Offset: 0x00080984
		private void ClearPrefabInstances()
		{
			UnityEngine.Object.DestroyImmediate(this.startPrefabInstance);
			UnityEngine.Object.DestroyImmediate(this.endPrefabInstance);
			if (this.tearPrefabPool != null)
			{
				for (int i = 0; i < this.tearPrefabPool.Length; i++)
				{
					if (this.tearPrefabPool[i] != null)
					{
						UnityEngine.Object.DestroyImmediate(this.tearPrefabPool[i]);
						this.tearPrefabPool[i] = null;
					}
				}
			}
		}

		// Token: 0x0600172D RID: 5933 RVA: 0x000825F4 File Offset: 0x000809F4
		public void GenerateProceduralChainLinks()
		{
			this.ClearChainLinkInstances();
			if (!this.initialized)
			{
				return;
			}
			if (this.chainLinks.Count > 0)
			{
				for (int i = 0; i < this.totalParticles; i++)
				{
					int index = (!this.randomizeLinks) ? (i % this.chainLinks.Count) : UnityEngine.Random.Range(0, this.chainLinks.Count);
					GameObject gameObject = null;
					if (this.chainLinks[index] != null)
					{
						gameObject = UnityEngine.Object.Instantiate<GameObject>(this.chainLinks[index]);
						gameObject.hideFlags = HideFlags.HideAndDontSave;
						gameObject.SetActive(false);
					}
					this.linkInstances.Add(gameObject);
				}
			}
			this.UpdateProceduralChainLinks();
		}

		// Token: 0x0600172E RID: 5934 RVA: 0x000826B8 File Offset: 0x00080AB8
		private void ClearChainLinkInstances()
		{
			for (int i = 0; i < this.linkInstances.Count; i++)
			{
				if (this.linkInstances[i] != null)
				{
					UnityEngine.Object.DestroyImmediate(this.linkInstances[i]);
				}
			}
			this.linkInstances.Clear();
		}

		// Token: 0x0600172F RID: 5935 RVA: 0x00082714 File Offset: 0x00080B14
		private Vector4[] ChaikinSmoothing(Vector4[] input, uint k)
		{
			if (k == 0U || input.Length < 3)
			{
				return input;
			}
			int num = (int)Mathf.Pow(2f, k);
			int num2 = input.Length - 1;
			float num3 = Mathf.Pow(2f, (float)(-(float)((ulong)(k + 1U))));
			float num4 = Mathf.Pow(2f, (float)(-(float)((ulong)k)));
			float num5 = Mathf.Pow(2f, (float)(18446744073709551614UL * (ulong)k));
			float num6 = Mathf.Pow(2f, (float)(18446744073709551614UL * (ulong)k - 1UL));
			Vector4[] array = new Vector4[(num2 - 1) * num + 2];
			float[] array2 = new float[num];
			float[] array3 = new float[num];
			float[] array4 = new float[num];
			for (int i = 1; i <= num; i++)
			{
				array2[i - 1] = 0.5f - num3 - (float)(i - 1) * (num4 - (float)i * num6);
				array3[i - 1] = 0.5f + num3 + (float)(i - 1) * (num4 - (float)i * num5);
				array4[i - 1] = (float)((i - 1) * i) * num6;
			}
			array[0] = (0.5f + num3) * input[0] + (0.5f - num3) * input[1];
			array[num * num2 - num + 1] = (0.5f - num3) * input[num2 - 1] + (0.5f + num3) * input[num2];
			for (int j = 1; j < num2; j++)
			{
				for (int l = 1; l <= num; l++)
				{
					array[(j - 1) * num + l] = array2[l - 1] * input[j - 1] + array3[l - 1] * input[j] + array4[l - 1] * input[j + 1];
				}
			}
			return array;
		}

		// Token: 0x06001730 RID: 5936 RVA: 0x00082940 File Offset: 0x00080D40
		private float CalculateCurveLength(Vector4[] curve)
		{
			float num = 0f;
			for (int i = 1; i < curve.Length; i++)
			{
				num += Vector3.Distance(curve[i], curve[i - 1]);
			}
			return num;
		}

		// Token: 0x06001731 RID: 5937 RVA: 0x00082994 File Offset: 0x00080D94
		public int GetConstraintIndexAtNormalizedCoordinate(float coord)
		{
			ObiDistanceConstraintBatch obiDistanceConstraintBatch = this.DistanceConstraints.GetBatches()[0] as ObiDistanceConstraintBatch;
			float f = coord * (float)obiDistanceConstraintBatch.ConstraintCount;
			return Mathf.Clamp(Mathf.FloorToInt(f), 0, obiDistanceConstraintBatch.ConstraintCount - 1);
		}

		// Token: 0x06001732 RID: 5938 RVA: 0x000829D8 File Offset: 0x00080DD8
		private List<int> CountContinuousSections()
		{
			List<int> list = new List<int>(this.usedParticles);
			ObiDistanceConstraintBatch obiDistanceConstraintBatch = this.DistanceConstraints.GetBatches()[0] as ObiDistanceConstraintBatch;
			int num = 0;
			int num2 = -1;
			for (int i = 0; i < obiDistanceConstraintBatch.ConstraintCount; i++)
			{
				int num3 = obiDistanceConstraintBatch.springIndices[i * 2];
				int num4 = obiDistanceConstraintBatch.springIndices[i * 2 + 1];
				if (num3 != num2 && num > 0)
				{
					list.Add(num);
					num = 0;
				}
				num2 = num4;
				num++;
			}
			if (num > 0)
			{
				list.Add(num);
			}
			return list;
		}

		// Token: 0x06001733 RID: 5939 RVA: 0x00082A7C File Offset: 0x00080E7C
		private void SmoothCurvesFromParticles()
		{
			this.curveSections = 0f;
			this.curveLength = 0f;
			this.curves.Clear();
			List<int> list = this.CountContinuousSections();
			ObiDistanceConstraintBatch obiDistanceConstraintBatch = this.DistanceConstraints.GetBatches()[0] as ObiDistanceConstraintBatch;
			Matrix4x4 worldToLocalMatrix = base.transform.worldToLocalMatrix;
			int num = 0;
			foreach (int num2 in list)
			{
				Vector4[] array = new Vector4[num2 + 1];
				for (int i = 0; i < num2; i++)
				{
					int num3 = obiDistanceConstraintBatch.springIndices[(num + i) * 2];
					array[i] = worldToLocalMatrix.MultiplyPoint3x4(base.GetParticlePosition(num3));
					array[i].w = this.solidRadii[num3];
					if (i == num2 - 1)
					{
						num3 = obiDistanceConstraintBatch.springIndices[(num + i) * 2 + 1];
						array[i + 1] = worldToLocalMatrix.MultiplyPoint3x4(base.GetParticlePosition(num3));
						array[i + 1].w = this.solidRadii[num3];
					}
				}
				num += num2;
				Vector4[] array2 = this.ChaikinSmoothing(array, this.smoothing);
				array2[0] = array[0];
				array2[array2.Length - 1] = array[array.Length - 1];
				this.curves.Add(array2);
				this.curveSections += (float)(array2.Length - 1);
				this.curveLength += this.CalculateCurveLength(array2);
			}
		}

		// Token: 0x06001734 RID: 5940 RVA: 0x00082C7C File Offset: 0x0008107C
		private void PlaceObjectAtCurveFrame(ObiRope.CurveFrame frame, GameObject obj, Space space, bool reverseLookDirection)
		{
			if (space == Space.Self)
			{
				Matrix4x4 localToWorldMatrix = base.transform.localToWorldMatrix;
				obj.transform.position = localToWorldMatrix.MultiplyPoint3x4(frame.position);
				if (frame.tangent != Vector3.zero)
				{
					obj.transform.rotation = Quaternion.LookRotation(localToWorldMatrix.MultiplyVector((!reverseLookDirection) ? (-frame.tangent) : frame.tangent), localToWorldMatrix.MultiplyVector(frame.normal));
				}
			}
			else
			{
				obj.transform.position = frame.position;
				if (frame.tangent != Vector3.zero)
				{
					obj.transform.rotation = Quaternion.LookRotation((!reverseLookDirection) ? (-frame.tangent) : frame.tangent, frame.normal);
				}
			}
		}

		// Token: 0x06001735 RID: 5941 RVA: 0x00082D68 File Offset: 0x00081168
		public void UpdateProceduralRopeMesh()
		{
			if (!base.enabled || this.ropeMesh == null || this.section == null)
			{
				return;
			}
			this.SmoothCurvesFromParticles();
			if (this.renderMode == ObiRope.RenderingMode.ProceduralRope)
			{
				this.UpdateRopeMesh();
			}
		}

		// Token: 0x06001736 RID: 5942 RVA: 0x00082DBC File Offset: 0x000811BC
		private void ClearMeshData()
		{
			this.ropeMesh.Clear();
			this.vertices.Clear();
			this.normals.Clear();
			this.tangents.Clear();
			this.uvs.Clear();
			this.tris.Clear();
		}

		// Token: 0x06001737 RID: 5943 RVA: 0x00082E0C File Offset: 0x0008120C
		private void CommitMeshData()
		{
			this.ropeMesh.SetVertices(this.vertices);
			this.ropeMesh.SetNormals(this.normals);
			this.ropeMesh.SetTangents(this.tangents);
			this.ropeMesh.SetUVs(0, this.uvs);
			this.ropeMesh.SetTriangles(this.tris, 0, true);
		}

		// Token: 0x06001738 RID: 5944 RVA: 0x00082E74 File Offset: 0x00081274
		private void UpdateRopeMesh()
		{
			this.ClearMeshData();
			float num = this.curveLength / this.restLength;
			int segments = this.section.Segments;
			int num2 = segments + 1;
			float num3 = -this.uvScale.y * this.restLength * this.uvAnchor;
			int num4 = 0;
			int num5 = 0;
			ObiRope.CurveFrame curveFrame = new ObiRope.CurveFrame(-this.sectionTwist * this.curveSections * this.uvAnchor);
			Vector3 vector = Vector3.forward;
			Vector4 item = Vector4.zero;
			Vector2 zero = Vector2.zero;
			for (int i = 0; i < this.curves.Count; i++)
			{
				Vector4[] array = this.curves[i];
				curveFrame.Reset();
				for (int j = 0; j < array.Length; j++)
				{
					int num6 = Mathf.Min(j + 1, array.Length - 1);
					int num7 = Mathf.Max(j - 1, 0);
					Vector3 a;
					if (this.closed && i == this.curves.Count - 1 && j == array.Length - 1)
					{
						a = vector;
					}
					else
					{
						a = array[num6] - array[j];
					}
					Vector3 b = array[j] - array[num7];
					Vector3 vector2 = a + b;
					curveFrame.Transport(array[j], vector2, this.sectionTwist);
					if (this.tearPrefabPool != null)
					{
						if (num5 < this.tearPrefabPool.Length && i > 0 && j == 0)
						{
							if (!this.tearPrefabPool[num5].activeSelf)
							{
								this.tearPrefabPool[num5].SetActive(true);
							}
							this.PlaceObjectAtCurveFrame(curveFrame, this.tearPrefabPool[num5], Space.Self, false);
							num5++;
						}
						if (num5 < this.tearPrefabPool.Length && i < this.curves.Count - 1 && j == array.Length - 1)
						{
							if (!this.tearPrefabPool[num5].activeSelf)
							{
								this.tearPrefabPool[num5].SetActive(true);
							}
							this.PlaceObjectAtCurveFrame(curveFrame, this.tearPrefabPool[num5], Space.Self, true);
							num5++;
						}
					}
					if (i == 0 && j == 0)
					{
						vector = vector2;
						if (this.startPrefabInstance != null && !this.closed)
						{
							this.PlaceObjectAtCurveFrame(curveFrame, this.startPrefabInstance, Space.Self, false);
						}
					}
					else if (i == this.curves.Count - 1 && j == array.Length - 1 && this.endPrefabInstance != null && !this.closed)
					{
						this.PlaceObjectAtCurveFrame(curveFrame, this.endPrefabInstance, Space.Self, true);
					}
					num3 += this.uvScale.y * (Vector3.Distance(array[j], array[num7]) / ((!this.normalizeV) ? num : this.curveLength));
					float d = ((!this.thicknessFromParticles) ? this.thickness : array[j].w) * this.sectionThicknessScale;
					for (int k = 0; k <= segments; k++)
					{
						this.vertices.Add(curveFrame.position + (this.section.vertices[k].x * curveFrame.normal + this.section.vertices[k].y * curveFrame.binormal) * d);
						this.normals.Add(this.vertices[this.vertices.Count - 1] - curveFrame.position);
						item = -Vector3.Cross(this.normals[this.normals.Count - 1], curveFrame.tangent);
						item.w = 1f;
						this.tangents.Add(item);
						zero.Set((float)k / (float)segments * this.uvScale.x, num3);
						this.uvs.Add(zero);
						if (k < segments && j < array.Length - 1)
						{
							this.tris.Add(num4 * num2 + k);
							this.tris.Add(num4 * num2 + (k + 1));
							this.tris.Add((num4 + 1) * num2 + k);
							this.tris.Add(num4 * num2 + (k + 1));
							this.tris.Add((num4 + 1) * num2 + (k + 1));
							this.tris.Add((num4 + 1) * num2 + k);
						}
					}
					num4++;
				}
			}
			this.CommitMeshData();
		}

		// Token: 0x06001739 RID: 5945 RVA: 0x000833B4 File Offset: 0x000817B4
		private void UpdateLineMesh(Camera camera)
		{
			this.ClearMeshData();
			float num = this.curveLength / this.restLength;
			float num2 = -this.uvScale.y * this.restLength * this.uvAnchor;
			int num3 = 0;
			int num4 = 0;
			Vector3 b = base.transform.InverseTransformPoint(camera.transform.position);
			ObiRope.CurveFrame curveFrame = new ObiRope.CurveFrame(-this.sectionTwist * this.curveSections * this.uvAnchor);
			Vector3 vector = Vector3.forward;
			Vector4 item = Vector4.zero;
			Vector2 zero = Vector2.zero;
			for (int i = 0; i < this.curves.Count; i++)
			{
				Vector4[] array = this.curves[i];
				curveFrame.Reset();
				for (int j = 0; j < array.Length; j++)
				{
					int num5 = Mathf.Min(j + 1, array.Length - 1);
					int num6 = Mathf.Max(j - 1, 0);
					Vector3 a;
					if (this.closed && i == this.curves.Count - 1 && j == array.Length - 1)
					{
						a = vector;
					}
					else
					{
						a = array[num5] - array[j];
					}
					Vector3 b2 = array[j] - array[num6];
					Vector3 vector2 = a + b2;
					curveFrame.Transport(array[j], vector2, this.sectionTwist);
					if (this.tearPrefabPool != null)
					{
						if (num4 < this.tearPrefabPool.Length && i > 0 && j == 0)
						{
							if (!this.tearPrefabPool[num4].activeSelf)
							{
								this.tearPrefabPool[num4].SetActive(true);
							}
							this.PlaceObjectAtCurveFrame(curveFrame, this.tearPrefabPool[num4], Space.Self, false);
							num4++;
						}
						if (num4 < this.tearPrefabPool.Length && i < this.curves.Count - 1 && j == array.Length - 1)
						{
							if (!this.tearPrefabPool[num4].activeSelf)
							{
								this.tearPrefabPool[num4].SetActive(true);
							}
							this.PlaceObjectAtCurveFrame(curveFrame, this.tearPrefabPool[num4], Space.Self, true);
							num4++;
						}
					}
					if (i == 0 && j == 0)
					{
						vector = vector2;
						if (this.startPrefabInstance != null && !this.closed)
						{
							this.PlaceObjectAtCurveFrame(curveFrame, this.startPrefabInstance, Space.Self, false);
						}
					}
					else if (i == this.curves.Count - 1 && j == array.Length - 1 && this.endPrefabInstance != null && !this.closed)
					{
						this.PlaceObjectAtCurveFrame(curveFrame, this.endPrefabInstance, Space.Self, true);
					}
					num2 += this.uvScale.y * (Vector3.Distance(array[j], array[num6]) / ((!this.normalizeV) ? num : this.curveLength));
					float d = ((!this.thicknessFromParticles) ? this.thickness : array[j].w) * this.sectionThicknessScale;
					Vector3 vector3 = curveFrame.position - b;
					vector3.Normalize();
					Vector3 a2 = Vector3.Cross(curveFrame.tangent, vector3);
					a2.Normalize();
					this.vertices.Add(curveFrame.position + a2 * d);
					this.vertices.Add(curveFrame.position - a2 * d);
					this.normals.Add(-vector3);
					this.normals.Add(-vector3);
					item = -a2;
					item.w = 1f;
					this.tangents.Add(item);
					this.tangents.Add(item);
					zero.Set(0f, num2);
					this.uvs.Add(zero);
					zero.Set(1f, num2);
					this.uvs.Add(zero);
					if (j < array.Length - 1)
					{
						this.tris.Add(num3 * 2);
						this.tris.Add(num3 * 2 + 1);
						this.tris.Add((num3 + 1) * 2);
						this.tris.Add(num3 * 2 + 1);
						this.tris.Add((num3 + 1) * 2 + 1);
						this.tris.Add((num3 + 1) * 2);
					}
					num3++;
				}
			}
			this.CommitMeshData();
		}

		// Token: 0x0600173A RID: 5946 RVA: 0x000838A0 File Offset: 0x00081CA0
		public void UpdateProceduralChainLinks()
		{
			if (this.linkInstances.Count == 0)
			{
				return;
			}
			ObiDistanceConstraintBatch obiDistanceConstraintBatch = this.DistanceConstraints.GetBatches()[0] as ObiDistanceConstraintBatch;
			ObiRope.CurveFrame curveFrame = new ObiRope.CurveFrame(-this.sectionTwist * (float)obiDistanceConstraintBatch.ConstraintCount * this.uvAnchor);
			int num = -1;
			int num2 = 0;
			for (int i = 0; i < obiDistanceConstraintBatch.ConstraintCount; i++)
			{
				int num3 = obiDistanceConstraintBatch.springIndices[i * 2];
				int num4 = obiDistanceConstraintBatch.springIndices[i * 2 + 1];
				Vector3 particlePosition = base.GetParticlePosition(num3);
				Vector3 particlePosition2 = base.GetParticlePosition(num4);
				Vector3 a = particlePosition2 - particlePosition;
				Vector3 normalized = a.normalized;
				if (i > 0 && num3 != num)
				{
					if (this.tearPrefabPool != null && num2 < this.tearPrefabPool.Length)
					{
						if (!this.tearPrefabPool[num2].activeSelf)
						{
							this.tearPrefabPool[num2].SetActive(true);
						}
						this.PlaceObjectAtCurveFrame(curveFrame, this.tearPrefabPool[num2], Space.World, true);
						num2++;
					}
					curveFrame.Reset();
				}
				curveFrame.Transport(particlePosition2, normalized, this.sectionTwist);
				if (i > 0 && num3 != num && this.tearPrefabPool != null && num2 < this.tearPrefabPool.Length)
				{
					if (!this.tearPrefabPool[num2].activeSelf)
					{
						this.tearPrefabPool[num2].SetActive(true);
					}
					curveFrame.position = particlePosition;
					this.PlaceObjectAtCurveFrame(curveFrame, this.tearPrefabPool[num2], Space.World, false);
					num2++;
				}
				if (!this.closed)
				{
					if (i == 0 && this.startPrefabInstance != null)
					{
						this.PlaceObjectAtCurveFrame(curveFrame, this.startPrefabInstance, Space.World, false);
					}
					else if (i == obiDistanceConstraintBatch.ConstraintCount - 1 && this.endPrefabInstance != null)
					{
						curveFrame.position = particlePosition2;
						this.PlaceObjectAtCurveFrame(curveFrame, this.endPrefabInstance, Space.World, true);
					}
				}
				if (this.linkInstances[i] != null)
				{
					this.linkInstances[i].SetActive(true);
					Transform transform = this.linkInstances[i].transform;
					transform.position = particlePosition + a * 0.5f;
					transform.localScale = ((!this.thicknessFromParticles) ? this.linkScale : (this.solidRadii[num3] / this.thickness * this.linkScale));
					transform.rotation = Quaternion.LookRotation(normalized, curveFrame.normal);
				}
				num = num4;
			}
			for (int j = obiDistanceConstraintBatch.ConstraintCount; j < this.linkInstances.Count; j++)
			{
				if (this.linkInstances[j] != null)
				{
					this.linkInstances[j].SetActive(false);
				}
			}
		}

		// Token: 0x0600173B RID: 5947 RVA: 0x00083B9C File Offset: 0x00081F9C
		public override void ResetActor()
		{
			this.PushDataToSolver(ParticleData.POSITIONS | ParticleData.VELOCITIES);
			if (this.particleIndices != null)
			{
				for (int i = 0; i < this.particleIndices.Length; i++)
				{
					this.solver.renderablePositions[this.particleIndices[i]] = this.positions[i];
				}
			}
			this.UpdateVisualRepresentation();
		}

		// Token: 0x0600173C RID: 5948 RVA: 0x00083C10 File Offset: 0x00082010
		private void ApplyTearing()
		{
			if (!this.tearable)
			{
				return;
			}
			ObiDistanceConstraintBatch obiDistanceConstraintBatch = this.DistanceConstraints.GetBatches()[0] as ObiDistanceConstraintBatch;
			float[] array = new float[obiDistanceConstraintBatch.ConstraintCount];
			Oni.GetBatchConstraintForces(obiDistanceConstraintBatch.OniBatch, array, obiDistanceConstraintBatch.ConstraintCount, 0);
			List<int> list = new List<int>();
			for (int i = 0; i < array.Length; i++)
			{
				float num = this.tearResistance[obiDistanceConstraintBatch.springIndices[i * 2]];
				float num2 = this.tearResistance[obiDistanceConstraintBatch.springIndices[i * 2 + 1]];
				float num3 = (num + num2) * 0.5f * this.tearResistanceMultiplier;
				if (-array[i] * 1000f > num3)
				{
					list.Add(i);
				}
			}
			if (list.Count > 0)
			{
				this.DistanceConstraints.RemoveFromSolver(null);
				this.BendingConstraints.RemoveFromSolver(null);
				for (int j = 0; j < list.Count; j++)
				{
					this.Tear(list[j]);
				}
				this.BendingConstraints.AddToSolver(this);
				this.DistanceConstraints.AddToSolver(this);
				this.BendingConstraints.SetActiveConstraints();
				this.solver.UpdateActiveParticles();
			}
		}

		// Token: 0x0600173D RID: 5949 RVA: 0x00083D58 File Offset: 0x00082158
		public bool DoesBendConstraintSpanDistanceConstraint(ObiDistanceConstraintBatch dbatch, ObiBendConstraintBatch bbatch, int d, int b)
		{
			return (bbatch.bendingIndices[b * 3 + 2] == dbatch.springIndices[d * 2] && bbatch.bendingIndices[b * 3 + 1] == dbatch.springIndices[d * 2 + 1]) || (bbatch.bendingIndices[b * 3 + 1] == dbatch.springIndices[d * 2] && bbatch.bendingIndices[b * 3 + 2] == dbatch.springIndices[d * 2 + 1]) || (bbatch.bendingIndices[b * 3 + 2] == dbatch.springIndices[d * 2] && bbatch.bendingIndices[b * 3] == dbatch.springIndices[d * 2 + 1]) || (bbatch.bendingIndices[b * 3] == dbatch.springIndices[d * 2] && bbatch.bendingIndices[b * 3 + 2] == dbatch.springIndices[d * 2 + 1]);
		}

		// Token: 0x0600173E RID: 5950 RVA: 0x00083E8C File Offset: 0x0008228C
		public void Tear(int constraintIndex)
		{
			if (this.usedParticles >= this.totalParticles)
			{
				return;
			}
			ObiDistanceConstraintBatch obiDistanceConstraintBatch = (ObiDistanceConstraintBatch)this.DistanceConstraints.GetBatches()[0];
			ObiBendConstraintBatch obiBendConstraintBatch = (ObiBendConstraintBatch)this.BendingConstraints.GetBatches()[0];
			int num = obiDistanceConstraintBatch.springIndices[constraintIndex * 2];
			int num2 = obiDistanceConstraintBatch.springIndices[constraintIndex * 2 + 1];
			bool flag = (constraintIndex < obiDistanceConstraintBatch.ConstraintCount - 1 && obiDistanceConstraintBatch.springIndices[(constraintIndex + 1) * 2] == num) || (constraintIndex > 0 && obiDistanceConstraintBatch.springIndices[(constraintIndex - 1) * 2 + 1] == num);
			bool flag2 = (constraintIndex < obiDistanceConstraintBatch.ConstraintCount - 1 && obiDistanceConstraintBatch.springIndices[(constraintIndex + 1) * 2] == num2) || (constraintIndex > 0 && obiDistanceConstraintBatch.springIndices[(constraintIndex - 1) * 2 + 1] == num2);
			if ((this.invMasses[num] > this.invMasses[num2] || this.invMasses[num] == 0f) && flag2)
			{
				num = num2;
			}
			if (this.invMasses[num] == 0f || !flag)
			{
				return;
			}
			this.invMasses[num] *= 2f;
			this.positions[this.usedParticles] = this.positions[num];
			this.velocities[this.usedParticles] = this.velocities[num];
			this.active[this.usedParticles] = this.active[num];
			this.invMasses[this.usedParticles] = this.invMasses[num];
			this.solidRadii[this.usedParticles] = this.solidRadii[num];
			this.phases[this.usedParticles] = this.phases[num];
			this.tearResistance[this.usedParticles] = this.tearResistance[num];
			this.restPositions[this.usedParticles] = this.positions[num];
			this.restPositions[this.usedParticles][3] = 0f;
			Vector4[] velocities = new Vector4[]
			{
				Vector4.zero
			};
			Oni.GetParticleVelocities(this.solver.OniSolver, velocities, 1, this.particleIndices[num]);
			Oni.SetParticleVelocities(this.solver.OniSolver, velocities, 1, this.particleIndices[this.usedParticles]);
			Vector4[] positions = new Vector4[]
			{
				Vector4.zero
			};
			Oni.GetParticlePositions(this.solver.OniSolver, positions, 1, this.particleIndices[num]);
			Oni.SetParticlePositions(this.solver.OniSolver, positions, 1, this.particleIndices[this.usedParticles]);
			Oni.SetParticleInverseMasses(this.solver.OniSolver, new float[]
			{
				this.invMasses[num]
			}, 1, this.particleIndices[this.usedParticles]);
			Oni.SetParticleSolidRadii(this.solver.OniSolver, new float[]
			{
				this.solidRadii[num]
			}, 1, this.particleIndices[this.usedParticles]);
			Oni.SetParticlePhases(this.solver.OniSolver, new int[]
			{
				this.phases[num]
			}, 1, this.particleIndices[this.usedParticles]);
			for (int i = 0; i < obiBendConstraintBatch.ConstraintCount; i++)
			{
				if (obiBendConstraintBatch.bendingIndices[i * 3 + 2] == num)
				{
					obiBendConstraintBatch.DeactivateConstraint(i);
				}
				else if (!this.DoesBendConstraintSpanDistanceConstraint(obiDistanceConstraintBatch, obiBendConstraintBatch, constraintIndex, i))
				{
					if (obiBendConstraintBatch.bendingIndices[i * 3] == num)
					{
						obiBendConstraintBatch.bendingIndices[i * 3] = this.usedParticles;
					}
					else if (obiBendConstraintBatch.bendingIndices[i * 3 + 1] == num)
					{
						obiBendConstraintBatch.bendingIndices[i * 3 + 1] = this.usedParticles;
					}
				}
			}
			if (constraintIndex < obiDistanceConstraintBatch.ConstraintCount - 1)
			{
				if (obiDistanceConstraintBatch.springIndices[(constraintIndex + 1) * 2] == num)
				{
					obiDistanceConstraintBatch.springIndices[(constraintIndex + 1) * 2] = this.usedParticles;
				}
				if (obiDistanceConstraintBatch.springIndices[(constraintIndex + 1) * 2 + 1] == num)
				{
					obiDistanceConstraintBatch.springIndices[(constraintIndex + 1) * 2 + 1] = this.usedParticles;
				}
			}
			if (constraintIndex > 0)
			{
				if (obiDistanceConstraintBatch.springIndices[(constraintIndex - 1) * 2] == num)
				{
					obiDistanceConstraintBatch.springIndices[(constraintIndex - 1) * 2] = this.usedParticles;
				}
				if (obiDistanceConstraintBatch.springIndices[(constraintIndex - 1) * 2 + 1] == num)
				{
					obiDistanceConstraintBatch.springIndices[(constraintIndex - 1) * 2 + 1] = this.usedParticles;
				}
			}
			this.usedParticles++;
			this.pooledParticles--;
		}

		// Token: 0x0600173F RID: 5951 RVA: 0x000843BE File Offset: 0x000827BE
		public override bool GenerateTethers(ObiActor.TetherType type)
		{
			if (!base.Initialized)
			{
				return false;
			}
			this.TetherConstraints.Clear();
			if (type == ObiActor.TetherType.Hierarchical)
			{
				this.GenerateHierarchicalTethers(5);
			}
			else
			{
				this.GenerateFixedTethers(2);
			}
			return true;
		}

		// Token: 0x06001740 RID: 5952 RVA: 0x000843F4 File Offset: 0x000827F4
		private void GenerateFixedTethers(int maxTethers)
		{
			ObiTetherConstraintBatch obiTetherConstraintBatch = new ObiTetherConstraintBatch(true, false, 0.0001f, 200f);
			this.TetherConstraints.AddBatch(obiTetherConstraintBatch);
			List<HashSet<int>> list = new List<HashSet<int>>();
			for (int i = 0; i < this.usedParticles; i++)
			{
				if (this.invMasses[i] <= 0f && this.active[i])
				{
					int num = -1;
					List<int> list2 = new List<int>();
					int num2 = Mathf.Max(i - 1, 0);
					int num3 = Mathf.Min(i + 1, this.usedParticles - 1);
					for (int j = 0; j < list.Count; j++)
					{
						if ((this.active[num2] && list[j].Contains(num2)) || (this.active[num3] && list[j].Contains(num3)))
						{
							if (num < 0)
							{
								num = j;
								list[j].Add(i);
							}
							else if (num != j && !list2.Contains(j))
							{
								list2.Add(j);
							}
						}
					}
					foreach (int index in list2)
					{
						list[num].UnionWith(list[index]);
					}
					list2.Sort();
					list2.Reverse();
					foreach (int index2 in list2)
					{
						list.RemoveAt(index2);
					}
					if (num < 0)
					{
						list.Add(new HashSet<int>
						{
							i
						});
					}
				}
			}
			for (int k = 0; k < this.usedParticles; k++)
			{
				if (this.invMasses[k] != 0f)
				{
					List<KeyValuePair<float, int>> list3 = new List<KeyValuePair<float, int>>(list.Count);
					foreach (HashSet<int> hashSet in list)
					{
						int num4 = -1;
						float num5 = float.PositiveInfinity;
						foreach (int num6 in hashSet)
						{
							int num7 = Mathf.Min(k, num6);
							int num8 = Mathf.Max(k, num6);
							float num9 = 0f;
							for (int l = num7; l < num8; l++)
							{
								num9 += Vector3.Distance(this.positions[l], this.positions[l + 1]);
							}
							if (num9 < num5)
							{
								num5 = num9;
								num4 = num6;
							}
						}
						if (num4 >= 0)
						{
							list3.Add(new KeyValuePair<float, int>(num5, num4));
						}
					}
					List<KeyValuePair<float, int>> list4 = list3;
					if (ObiRope.<>f__am$cache0 == null)
					{
						ObiRope.<>f__am$cache0 = new Comparison<KeyValuePair<float, int>>(ObiRope.<GenerateFixedTethers>m__0);
					}
					list4.Sort(ObiRope.<>f__am$cache0);
					for (int m = 0; m < Mathf.Min(maxTethers, list3.Count); m++)
					{
						obiTetherConstraintBatch.AddConstraint(k, list3[m].Value, list3[m].Key, 1f, 1f);
					}
				}
			}
			obiTetherConstraintBatch.Cook();
		}

		// Token: 0x06001741 RID: 5953 RVA: 0x000847D0 File Offset: 0x00082BD0
		private void GenerateHierarchicalTethers(int maxLevels)
		{
			ObiTetherConstraintBatch obiTetherConstraintBatch = new ObiTetherConstraintBatch(true, false, 0.0001f, 200f);
			this.TetherConstraints.AddBatch(obiTetherConstraintBatch);
			for (int i = 1; i <= maxLevels; i++)
			{
				int num = i * 2;
				for (int j = 0; j < this.usedParticles - num; j++)
				{
					int num2 = j + num;
					obiTetherConstraintBatch.AddConstraint(j, num2 % this.usedParticles, this.interParticleDistance * (float)num, 1f, 1f);
				}
			}
			obiTetherConstraintBatch.Cook();
		}

		// Token: 0x06001742 RID: 5954 RVA: 0x00084858 File Offset: 0x00082C58
		[CompilerGenerated]
		private static int <GenerateFixedTethers>m__0(KeyValuePair<float, int> x, KeyValuePair<float, int> y)
		{
			return x.Key.CompareTo(y.Key);
		}

		// Token: 0x040012FF RID: 4863
		public const float DEFAULT_PARTICLE_MASS = 0.1f;

		// Token: 0x04001300 RID: 4864
		public const float MAX_YOUNG_MODULUS = 200f;

		// Token: 0x04001301 RID: 4865
		public const float MIN_YOUNG_MODULUS = 0.0001f;

		// Token: 0x04001302 RID: 4866
		[Tooltip("Amount of additional particles in this rope's pool that can be used to extend its lenght, or to tear it.")]
		public int pooledParticles = 10;

		// Token: 0x04001303 RID: 4867
		[Tooltip("Path used to generate the rope.")]
		public ObiCurve ropePath;

		// Token: 0x04001304 RID: 4868
		[HideInInspector]
		[SerializeField]
		private ObiRopeSection section;

		// Token: 0x04001305 RID: 4869
		[HideInInspector]
		[SerializeField]
		private float sectionTwist;

		// Token: 0x04001306 RID: 4870
		[HideInInspector]
		[SerializeField]
		private float sectionThicknessScale = 0.8f;

		// Token: 0x04001307 RID: 4871
		[HideInInspector]
		[SerializeField]
		private bool thicknessFromParticles = true;

		// Token: 0x04001308 RID: 4872
		[HideInInspector]
		[SerializeField]
		private Vector2 uvScale = Vector3.one;

		// Token: 0x04001309 RID: 4873
		[HideInInspector]
		[SerializeField]
		private float uvAnchor;

		// Token: 0x0400130A RID: 4874
		[HideInInspector]
		[SerializeField]
		private bool normalizeV = true;

		// Token: 0x0400130B RID: 4875
		[Tooltip("Modulates the amount of particles per lenght unit. 1 means as many particles as needed for the given length/thickness will be used, whichcan be a lot in very thin and long ropes. Setting values between 0 and 1 allows you to override the amount of particles used.")]
		[Range(0f, 1f)]
		public float resolution = 0.5f;

		// Token: 0x0400130C RID: 4876
		[HideInInspector]
		[SerializeField]
		private uint smoothing = 1U;

		// Token: 0x0400130D RID: 4877
		public bool tearable;

		// Token: 0x0400130E RID: 4878
		[Tooltip("Maximum strain betweeen particles before the spring constraint holding them together would break.")]
		[Delayed]
		public float tearResistanceMultiplier = 1000f;

		// Token: 0x0400130F RID: 4879
		[HideInInspector]
		public float[] tearResistance;

		// Token: 0x04001310 RID: 4880
		[HideInInspector]
		[SerializeField]
		private ObiRope.RenderingMode renderMode;

		// Token: 0x04001311 RID: 4881
		public List<GameObject> chainLinks = new List<GameObject>();

		// Token: 0x04001312 RID: 4882
		[HideInInspector]
		[SerializeField]
		private Vector3 linkScale = Vector3.one;

		// Token: 0x04001313 RID: 4883
		[HideInInspector]
		[SerializeField]
		private bool randomizeLinks;

		// Token: 0x04001314 RID: 4884
		[HideInInspector]
		public Mesh ropeMesh;

		// Token: 0x04001315 RID: 4885
		[HideInInspector]
		[SerializeField]
		private List<GameObject> linkInstances;

		// Token: 0x04001316 RID: 4886
		public GameObject startPrefab;

		// Token: 0x04001317 RID: 4887
		public GameObject endPrefab;

		// Token: 0x04001318 RID: 4888
		public GameObject tearPrefab;

		// Token: 0x04001319 RID: 4889
		[Tooltip("Thickness of the rope, it is equivalent to particle radius.")]
		public float thickness = 0.05f;

		// Token: 0x0400131A RID: 4890
		private GameObject[] tearPrefabPool;

		// Token: 0x0400131B RID: 4891
		[HideInInspector]
		[SerializeField]
		private bool closed;

		// Token: 0x0400131C RID: 4892
		[HideInInspector]
		[SerializeField]
		private float interParticleDistance;

		// Token: 0x0400131D RID: 4893
		[HideInInspector]
		[SerializeField]
		private float restLength;

		// Token: 0x0400131E RID: 4894
		[HideInInspector]
		[SerializeField]
		private int usedParticles;

		// Token: 0x0400131F RID: 4895
		[HideInInspector]
		[SerializeField]
		private int totalParticles;

		// Token: 0x04001320 RID: 4896
		private MeshFilter meshFilter;

		// Token: 0x04001321 RID: 4897
		private GameObject startPrefabInstance;

		// Token: 0x04001322 RID: 4898
		private GameObject endPrefabInstance;

		// Token: 0x04001323 RID: 4899
		private float curveLength;

		// Token: 0x04001324 RID: 4900
		private float curveSections;

		// Token: 0x04001325 RID: 4901
		private List<Vector4[]> curves = new List<Vector4[]>();

		// Token: 0x04001326 RID: 4902
		private List<Vector3> vertices = new List<Vector3>();

		// Token: 0x04001327 RID: 4903
		private List<Vector3> normals = new List<Vector3>();

		// Token: 0x04001328 RID: 4904
		private List<Vector4> tangents = new List<Vector4>();

		// Token: 0x04001329 RID: 4905
		private List<Vector2> uvs = new List<Vector2>();

		// Token: 0x0400132A RID: 4906
		private List<int> tris = new List<int>();

		// Token: 0x0400132B RID: 4907
		[CompilerGenerated]
		private static Comparison<KeyValuePair<float, int>> <>f__am$cache0;

		// Token: 0x02000394 RID: 916
		public enum RenderingMode
		{
			// Token: 0x0400132D RID: 4909
			ProceduralRope,
			// Token: 0x0400132E RID: 4910
			Chain,
			// Token: 0x0400132F RID: 4911
			Line
		}

		// Token: 0x02000395 RID: 917
		public class CurveFrame
		{
			// Token: 0x06001743 RID: 5955 RVA: 0x0008487C File Offset: 0x00082C7C
			public CurveFrame(float twist)
			{
				Quaternion rotation = Quaternion.AngleAxis(twist, this.tangent);
				this.normal = rotation * this.normal;
				this.binormal = rotation * this.binormal;
			}

			// Token: 0x06001744 RID: 5956 RVA: 0x000848EC File Offset: 0x00082CEC
			public void Reset()
			{
				this.position = Vector3.zero;
				this.tangent = Vector3.forward;
				this.normal = Vector3.up;
				this.binormal = Vector3.left;
			}

			// Token: 0x06001745 RID: 5957 RVA: 0x0008491C File Offset: 0x00082D1C
			public void Transport(Vector3 newPosition, Vector3 newTangent, float twist)
			{
				Quaternion rhs = Quaternion.FromToRotation(this.tangent, newTangent);
				Quaternion lhs = Quaternion.AngleAxis(twist, newTangent);
				Quaternion rotation = lhs * rhs;
				this.normal = rotation * this.normal;
				this.binormal = rotation * this.binormal;
				this.tangent = newTangent;
				this.position = newPosition;
			}

			// Token: 0x04001330 RID: 4912
			public Vector3 position = Vector3.zero;

			// Token: 0x04001331 RID: 4913
			public Vector3 tangent = Vector3.forward;

			// Token: 0x04001332 RID: 4914
			public Vector3 normal = Vector3.up;

			// Token: 0x04001333 RID: 4915
			public Vector3 binormal = Vector3.left;
		}

		// Token: 0x02000F44 RID: 3908
		[CompilerGenerated]
		private sealed class <GeneratePhysicRepresentationForMesh>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x06007358 RID: 29528 RVA: 0x00084978 File Offset: 0x00082D78
			[DebuggerHidden]
			public <GeneratePhysicRepresentationForMesh>c__Iterator0()
			{
			}

			// Token: 0x06007359 RID: 29529 RVA: 0x00084980 File Offset: 0x00082D80
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					this.initialized = false;
					this.initializing = true;
					this.interParticleDistance = -1f;
					this.RemoveFromSolver(null);
					if (this.ropePath == null)
					{
						UnityEngine.Debug.LogError("Cannot initialize rope. There's no ropePath present. Please provide a spline to define the shape of the rope");
						return false;
					}
					this.ropePath.RecalculateSplineLenght(1E-05f, 7);
					this.closed = this.ropePath.Closed;
					this.restLength = this.ropePath.Length;
					this.usedParticles = Mathf.CeilToInt(this.restLength / this.thickness * this.resolution) + ((!this.closed) ? 1 : 0);
					this.totalParticles = this.usedParticles + this.pooledParticles;
					this.active = new bool[this.totalParticles];
					this.positions = new Vector3[this.totalParticles];
					this.velocities = new Vector3[this.totalParticles];
					this.invMasses = new float[this.totalParticles];
					this.solidRadii = new float[this.totalParticles];
					this.phases = new int[this.totalParticles];
					this.restPositions = new Vector4[this.totalParticles];
					this.tearResistance = new float[this.totalParticles];
					numSegments = this.usedParticles - ((!this.closed) ? 1 : 0);
					if (numSegments > 0)
					{
						this.interParticleDistance = this.restLength / (float)numSegments;
					}
					else
					{
						this.interParticleDistance = 0f;
					}
					radius = this.interParticleDistance * this.resolution;
					i = 0;
					break;
				case 1U:
					IL_3FB:
					i++;
					break;
				case 2U:
					IL_507:
					j++;
					goto IL_515;
				case 3U:
					IL_619:
					k++;
					goto IL_627;
				case 4U:
					IL_712:
					l++;
					goto IL_720;
				default:
					return false;
				}
				if (i >= this.usedParticles)
				{
					j = this.usedParticles;
				}
				else
				{
					this.active[i] = true;
					this.invMasses[i] = 10f;
					mu = this.ropePath.GetMuAtLenght(this.interParticleDistance * (float)i);
					this.positions[i] = base.transform.InverseTransformPoint(this.ropePath.transform.TransformPoint(this.ropePath.GetPositionAt(mu)));
					this.solidRadii[i] = radius;
					this.phases[i] = Oni.MakePhase(1, (!this.selfCollisions) ? ((Oni.ParticlePhase)0) : Oni.ParticlePhase.SelfCollide);
					this.tearResistance[i] = 1f;
					if (i % 100 == 0)
					{
						this.$current = new CoroutineJob.ProgressInfo("ObiRope: generating particles...", (float)i / (float)this.usedParticles);
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					goto IL_3FB;
				}
				IL_515:
				if (j >= this.totalParticles)
				{
					base.DistanceConstraints.Clear();
					distanceBatch = new ObiDistanceConstraintBatch(false, false, 0.0001f, 200f);
					base.DistanceConstraints.AddBatch(distanceBatch);
					k = 0;
				}
				else
				{
					this.active[j] = false;
					this.invMasses[j] = 10f;
					this.solidRadii[j] = radius;
					this.phases[j] = Oni.MakePhase(1, (!this.selfCollisions) ? ((Oni.ParticlePhase)0) : Oni.ParticlePhase.SelfCollide);
					this.tearResistance[j] = 1f;
					if (j % 100 == 0)
					{
						this.$current = new CoroutineJob.ProgressInfo("ObiRope: generating particles...", (float)j / (float)this.usedParticles);
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						return true;
					}
					goto IL_507;
				}
				IL_627:
				if (k >= numSegments)
				{
					base.BendingConstraints.Clear();
					bendingBatch = new ObiBendConstraintBatch(false, false, 0.0001f, 200f);
					base.BendingConstraints.AddBatch(bendingBatch);
					l = 0;
				}
				else
				{
					distanceBatch.AddConstraint(k, (k + 1) % ((!this.ropePath.Closed) ? (this.usedParticles + 1) : this.usedParticles), this.interParticleDistance, 1f, 1f);
					if (k % 500 == 0)
					{
						this.$current = new CoroutineJob.ProgressInfo("ObiRope: generating structural constraints...", (float)k / (float)numSegments);
						if (!this.$disposing)
						{
							this.$PC = 3;
						}
						return true;
					}
					goto IL_619;
				}
				IL_720:
				if (l >= this.usedParticles - ((!this.closed) ? 2 : 0))
				{
					base.TetherConstraints.Clear();
					base.PinConstraints.Clear();
					pinBatch = new ObiPinConstraintBatch(false, false, 0f, 200f);
					base.PinConstraints.AddBatch(pinBatch);
					this.initializing = false;
					this.initialized = true;
					base.RegenerateRestPositions();
					base.GenerateVisualRepresentation();
					this.$PC = -1;
				}
				else
				{
					bendingBatch.AddConstraint(l, (l + 2) % this.usedParticles, (l + 1) % this.usedParticles, 0f, 0f, 1f);
					if (l % 500 == 0)
					{
						this.$current = new CoroutineJob.ProgressInfo("ObiRope: adding bend constraints...", (float)l / (float)this.usedParticles);
						if (!this.$disposing)
						{
							this.$PC = 4;
						}
						return true;
					}
					goto IL_712;
				}
				return false;
			}

			// Token: 0x170010E3 RID: 4323
			// (get) Token: 0x0600735A RID: 29530 RVA: 0x00085160 File Offset: 0x00083560
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170010E4 RID: 4324
			// (get) Token: 0x0600735B RID: 29531 RVA: 0x00085168 File Offset: 0x00083568
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x0600735C RID: 29532 RVA: 0x00085170 File Offset: 0x00083570
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x0600735D RID: 29533 RVA: 0x00085180 File Offset: 0x00083580
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x04006747 RID: 26439
			internal int <numSegments>__0;

			// Token: 0x04006748 RID: 26440
			internal float <radius>__0;

			// Token: 0x04006749 RID: 26441
			internal int <i>__1;

			// Token: 0x0400674A RID: 26442
			internal float <mu>__2;

			// Token: 0x0400674B RID: 26443
			internal int <i>__3;

			// Token: 0x0400674C RID: 26444
			internal ObiDistanceConstraintBatch <distanceBatch>__0;

			// Token: 0x0400674D RID: 26445
			internal int <i>__4;

			// Token: 0x0400674E RID: 26446
			internal ObiBendConstraintBatch <bendingBatch>__0;

			// Token: 0x0400674F RID: 26447
			internal int <i>__5;

			// Token: 0x04006750 RID: 26448
			internal ObiPinConstraintBatch <pinBatch>__0;

			// Token: 0x04006751 RID: 26449
			internal ObiRope $this;

			// Token: 0x04006752 RID: 26450
			internal object $current;

			// Token: 0x04006753 RID: 26451
			internal bool $disposing;

			// Token: 0x04006754 RID: 26452
			internal int $PC;
		}
	}
}
