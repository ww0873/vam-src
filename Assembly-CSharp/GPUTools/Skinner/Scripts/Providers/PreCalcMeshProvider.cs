using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Skinner.Scripts.Abstract;
using UnityEngine;

namespace GPUTools.Skinner.Scripts.Providers
{
	// Token: 0x02000A90 RID: 2704
	[Serializable]
	public class PreCalcMeshProvider : MonoBehaviour, IMeshProvider
	{
		// Token: 0x0600461B RID: 17947 RVA: 0x00140732 File Offset: 0x0013EB32
		public PreCalcMeshProvider()
		{
		}

		// Token: 0x0600461C RID: 17948 RVA: 0x00140741 File Offset: 0x0013EB41
		public virtual bool Validate(bool log)
		{
			if (log)
			{
			}
			return this.Mesh != null;
		}

		// Token: 0x1700098C RID: 2444
		// (get) Token: 0x0600461D RID: 17949 RVA: 0x00140755 File Offset: 0x0013EB55
		public virtual Matrix4x4 ToWorldMatrix
		{
			get
			{
				return base.transform.localToWorldMatrix;
			}
		}

		// Token: 0x1700098D RID: 2445
		// (get) Token: 0x0600461E RID: 17950 RVA: 0x00140762 File Offset: 0x0013EB62
		public virtual GpuBuffer<Matrix4x4> ToWorldMatricesBuffer
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700098E RID: 2446
		// (get) Token: 0x0600461F RID: 17951 RVA: 0x00140765 File Offset: 0x0013EB65
		// (set) Token: 0x06004620 RID: 17952 RVA: 0x0014076D File Offset: 0x0013EB6D
		public virtual GpuBuffer<Vector3> PreCalculatedVerticesBuffer
		{
			[CompilerGenerated]
			get
			{
				return this.<PreCalculatedVerticesBuffer>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<PreCalculatedVerticesBuffer>k__BackingField = value;
			}
		}

		// Token: 0x1700098F RID: 2447
		// (get) Token: 0x06004621 RID: 17953 RVA: 0x00140776 File Offset: 0x0013EB76
		// (set) Token: 0x06004622 RID: 17954 RVA: 0x0014077E File Offset: 0x0013EB7E
		public virtual GpuBuffer<Vector3> NormalsBuffer
		{
			[CompilerGenerated]
			get
			{
				return this.<NormalsBuffer>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<NormalsBuffer>k__BackingField = value;
			}
		}

		// Token: 0x17000990 RID: 2448
		// (get) Token: 0x06004623 RID: 17955 RVA: 0x00140787 File Offset: 0x0013EB87
		public virtual Mesh Mesh
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000991 RID: 2449
		// (get) Token: 0x06004624 RID: 17956 RVA: 0x0014078A File Offset: 0x0013EB8A
		public virtual Mesh BaseMesh
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000992 RID: 2450
		// (get) Token: 0x06004625 RID: 17957 RVA: 0x0014078D File Offset: 0x0013EB8D
		public virtual Mesh MeshForImport
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000993 RID: 2451
		// (get) Token: 0x06004626 RID: 17958 RVA: 0x00140790 File Offset: 0x0013EB90
		public virtual Color[] VertexSimColors
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06004627 RID: 17959 RVA: 0x00140793 File Offset: 0x0013EB93
		public virtual void Stop()
		{
		}

		// Token: 0x06004628 RID: 17960 RVA: 0x00140795 File Offset: 0x0013EB95
		public virtual void Dispatch()
		{
		}

		// Token: 0x06004629 RID: 17961 RVA: 0x00140797 File Offset: 0x0013EB97
		public virtual void PostProcessDispatch(ComputeBuffer finalVerts)
		{
		}

		// Token: 0x0600462A RID: 17962 RVA: 0x00140799 File Offset: 0x0013EB99
		public virtual void Dispose()
		{
		}

		// Token: 0x040033B5 RID: 13237
		public bool provideToWorldMatrices;

		// Token: 0x040033B6 RID: 13238
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Vector3> <PreCalculatedVerticesBuffer>k__BackingField;

		// Token: 0x040033B7 RID: 13239
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Vector3> <NormalsBuffer>k__BackingField;

		// Token: 0x040033B8 RID: 13240
		public bool useBaseMesh;

		// Token: 0x040033B9 RID: 13241
		public int[] materialsToUse;

		// Token: 0x040033BA RID: 13242
		public bool drawInPostProcess = true;
	}
}
