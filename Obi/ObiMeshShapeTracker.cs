using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Obi
{
	// Token: 0x0200039E RID: 926
	public class ObiMeshShapeTracker : ObiShapeTracker
	{
		// Token: 0x06001768 RID: 5992 RVA: 0x00085E49 File Offset: 0x00084249
		public ObiMeshShapeTracker(MeshCollider collider)
		{
			this.collider = collider;
			this.adaptor.is2D = false;
			this.oniShape = Oni.CreateShape(Oni.ShapeType.TriangleMesh);
			this.UpdateMeshData();
		}

		// Token: 0x06001769 RID: 5993 RVA: 0x00085E78 File Offset: 0x00084278
		public void UpdateMeshData()
		{
			MeshCollider meshCollider = this.collider as MeshCollider;
			if (meshCollider != null)
			{
				Mesh sharedMesh = meshCollider.sharedMesh;
				if (this.handles != null)
				{
					this.handles.Unref();
				}
				ObiMeshShapeTracker.MeshDataHandles meshDataHandles;
				if (!ObiMeshShapeTracker.meshDataCache.TryGetValue(sharedMesh, out meshDataHandles))
				{
					this.handles = new ObiMeshShapeTracker.MeshDataHandles();
					ObiMeshShapeTracker.meshDataCache[sharedMesh] = this.handles;
				}
				else
				{
					meshDataHandles.Ref();
					this.handles = meshDataHandles;
				}
				this.handles.FromMesh(meshCollider.sharedMesh);
				this.meshDataHasChanged = true;
			}
		}

		// Token: 0x0600176A RID: 5994 RVA: 0x00085F14 File Offset: 0x00084314
		public override void UpdateIfNeeded()
		{
			MeshCollider meshCollider = this.collider as MeshCollider;
			if (meshCollider != null)
			{
				Mesh sharedMesh = meshCollider.sharedMesh;
				if (sharedMesh != null && this.meshDataHasChanged)
				{
					this.meshDataHasChanged = false;
					this.adaptor.Set(this.handles.VerticesAddress, this.handles.IndicesAddress, sharedMesh.vertexCount, sharedMesh.triangles.Length);
					Oni.UpdateShape(this.oniShape, ref this.adaptor);
				}
			}
		}

		// Token: 0x0600176B RID: 5995 RVA: 0x00085FA0 File Offset: 0x000843A0
		public override void Destroy()
		{
			base.Destroy();
			MeshCollider meshCollider = this.collider as MeshCollider;
			if (meshCollider != null && this.handles != null)
			{
				this.handles.Unref();
				if (this.handles.RefCount <= 0)
				{
					ObiMeshShapeTracker.meshDataCache.Remove(meshCollider.sharedMesh);
				}
			}
		}

		// Token: 0x0600176C RID: 5996 RVA: 0x00086003 File Offset: 0x00084403
		// Note: this type is marked as 'beforefieldinit'.
		static ObiMeshShapeTracker()
		{
		}

		// Token: 0x0400134C RID: 4940
		private static Dictionary<Mesh, ObiMeshShapeTracker.MeshDataHandles> meshDataCache = new Dictionary<Mesh, ObiMeshShapeTracker.MeshDataHandles>();

		// Token: 0x0400134D RID: 4941
		private bool meshDataHasChanged;

		// Token: 0x0400134E RID: 4942
		private ObiMeshShapeTracker.MeshDataHandles handles;

		// Token: 0x0200039F RID: 927
		private class MeshDataHandles
		{
			// Token: 0x0600176D RID: 5997 RVA: 0x0008600F File Offset: 0x0008440F
			public MeshDataHandles()
			{
			}

			// Token: 0x170002B6 RID: 694
			// (get) Token: 0x0600176E RID: 5998 RVA: 0x0008601E File Offset: 0x0008441E
			public int RefCount
			{
				get
				{
					return this.refCount;
				}
			}

			// Token: 0x170002B7 RID: 695
			// (get) Token: 0x0600176F RID: 5999 RVA: 0x00086026 File Offset: 0x00084426
			public IntPtr VerticesAddress
			{
				get
				{
					return this.verticesHandle.AddrOfPinnedObject();
				}
			}

			// Token: 0x170002B8 RID: 696
			// (get) Token: 0x06001770 RID: 6000 RVA: 0x00086033 File Offset: 0x00084433
			public IntPtr IndicesAddress
			{
				get
				{
					return this.indicesHandle.AddrOfPinnedObject();
				}
			}

			// Token: 0x06001771 RID: 6001 RVA: 0x00086040 File Offset: 0x00084440
			public void FromMesh(Mesh mesh)
			{
				Oni.UnpinMemory(this.verticesHandle);
				Oni.UnpinMemory(this.indicesHandle);
				this.verticesHandle = Oni.PinMemory(mesh.vertices);
				this.indicesHandle = Oni.PinMemory(mesh.triangles);
			}

			// Token: 0x06001772 RID: 6002 RVA: 0x0008607A File Offset: 0x0008447A
			public void Ref()
			{
				this.refCount++;
			}

			// Token: 0x06001773 RID: 6003 RVA: 0x0008608A File Offset: 0x0008448A
			public void Unref()
			{
				this.refCount--;
				if (this.refCount <= 0)
				{
					this.refCount = 0;
					Oni.UnpinMemory(this.verticesHandle);
					Oni.UnpinMemory(this.indicesHandle);
				}
			}

			// Token: 0x0400134F RID: 4943
			private int refCount = 1;

			// Token: 0x04001350 RID: 4944
			private GCHandle verticesHandle;

			// Token: 0x04001351 RID: 4945
			private GCHandle indicesHandle;
		}
	}
}
