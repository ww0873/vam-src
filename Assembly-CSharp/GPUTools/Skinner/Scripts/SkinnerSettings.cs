using System;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Skinner.Scripts.Commands;
using GPUTools.Skinner.Scripts.Providers;
using UnityEngine;

namespace GPUTools.Skinner.Scripts
{
	// Token: 0x02000A94 RID: 2708
	public class SkinnerSettings : MonoBehaviour
	{
		// Token: 0x06004651 RID: 18001 RVA: 0x00140B5D File Offset: 0x0013EF5D
		public SkinnerSettings()
		{
		}

		// Token: 0x06004652 RID: 18002 RVA: 0x00140B70 File Offset: 0x0013EF70
		public void Initialize(int[] indices = null)
		{
			this.command = new SkinnerCommand(this.MeshProvider, indices);
			this.command.Build();
		}

		// Token: 0x06004653 RID: 18003 RVA: 0x00140B8F File Offset: 0x0013EF8F
		private void OnDestroy()
		{
			this.MeshProvider.Dispose();
			if (this.command != null)
			{
				this.command.Dispose();
			}
		}

		// Token: 0x06004654 RID: 18004 RVA: 0x00140BB2 File Offset: 0x0013EFB2
		public void Dispatch()
		{
			if (!this.MeshProvider.Validate(false) || this.command == null)
			{
				return;
			}
			this.MeshProvider.Dispatch();
			this.command.Dispatch();
		}

		// Token: 0x170009A9 RID: 2473
		// (get) Token: 0x06004655 RID: 18005 RVA: 0x00140BE7 File Offset: 0x0013EFE7
		public GpuBuffer<Matrix4x4> SelectedToWorldMatricesBuffer
		{
			get
			{
				return this.command.SelectedMatrices;
			}
		}

		// Token: 0x170009AA RID: 2474
		// (get) Token: 0x06004656 RID: 18006 RVA: 0x00140BF4 File Offset: 0x0013EFF4
		public GpuBuffer<Matrix4x4> ToWorldMatricesBuffer
		{
			get
			{
				return this.MeshProvider.ToWorldMatricesBuffer;
			}
		}

		// Token: 0x170009AB RID: 2475
		// (get) Token: 0x06004657 RID: 18007 RVA: 0x00140C01 File Offset: 0x0013F001
		public GpuBuffer<Vector3> SelectedWorldVerticesBuffer
		{
			get
			{
				return this.command.SelectedPoints;
			}
		}

		// Token: 0x170009AC RID: 2476
		// (get) Token: 0x06004658 RID: 18008 RVA: 0x00140C0E File Offset: 0x0013F00E
		public GpuBuffer<Vector3> WorldVerticesBuffer
		{
			get
			{
				return this.command.Points;
			}
		}

		// Token: 0x06004659 RID: 18009 RVA: 0x00140C1C File Offset: 0x0013F01C
		private void OnDrawGizmos()
		{
			if (!this.DebugDraw || !Application.isPlaying || !this.MeshProvider.Validate(false))
			{
				return;
			}
			int[] triangles = this.MeshProvider.Mesh.triangles;
			Vector3[] vertices = this.MeshProvider.Mesh.vertices;
			this.MeshProvider.Dispatch();
			this.MeshProvider.ToWorldMatricesBuffer.PullData();
			Matrix4x4[] data = this.MeshProvider.ToWorldMatricesBuffer.Data;
			Gizmos.color = Color.magenta;
			for (int i = 0; i < triangles.Length; i += 3)
			{
				int num = triangles[i];
				int num2 = triangles[i + 1];
				int num3 = triangles[i + 2];
				Vector3 vector = data[num].MultiplyPoint3x4(vertices[num]);
				Vector3 vector2 = data[num2].MultiplyPoint3x4(vertices[num2]);
				Vector3 vector3 = data[num3].MultiplyPoint3x4(vertices[num3]);
				Gizmos.DrawLine(vector, vector2);
				Gizmos.DrawLine(vector2, vector3);
				Gizmos.DrawLine(vector3, vector);
			}
		}

		// Token: 0x040033C1 RID: 13249
		[SerializeField]
		public bool DebugDraw;

		// Token: 0x040033C2 RID: 13250
		[SerializeField]
		public SkinnedMeshProvider MeshProvider = new SkinnedMeshProvider();

		// Token: 0x040033C3 RID: 13251
		private SkinnerCommand command;
	}
}
