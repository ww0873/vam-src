using System;
using GPUTools.Common.Scripts.PL.Tools;
using UnityEngine;

namespace GPUTools.Skinner.Scripts.Abstract
{
	// Token: 0x02000A88 RID: 2696
	public interface IMeshProvider
	{
		// Token: 0x1700096F RID: 2415
		// (get) Token: 0x060045D3 RID: 17875
		Matrix4x4 ToWorldMatrix { get; }

		// Token: 0x17000970 RID: 2416
		// (get) Token: 0x060045D4 RID: 17876
		GpuBuffer<Matrix4x4> ToWorldMatricesBuffer { get; }

		// Token: 0x17000971 RID: 2417
		// (get) Token: 0x060045D5 RID: 17877
		GpuBuffer<Vector3> PreCalculatedVerticesBuffer { get; }

		// Token: 0x17000972 RID: 2418
		// (get) Token: 0x060045D6 RID: 17878
		GpuBuffer<Vector3> NormalsBuffer { get; }

		// Token: 0x17000973 RID: 2419
		// (get) Token: 0x060045D7 RID: 17879
		Mesh Mesh { get; }

		// Token: 0x17000974 RID: 2420
		// (get) Token: 0x060045D8 RID: 17880
		Mesh MeshForImport { get; }

		// Token: 0x060045D9 RID: 17881
		bool Validate(bool log);

		// Token: 0x060045DA RID: 17882
		void Dispatch();

		// Token: 0x060045DB RID: 17883
		void Dispose();
	}
}
