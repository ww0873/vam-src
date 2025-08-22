using System;
using Battlehub.RTSaveLoad;
using UnityEngine;

namespace Battlehub.RTHandles
{
	// Token: 0x020000FB RID: 251
	[RequireComponent(typeof(PersistentIgnore))]
	[RequireComponent(typeof(Camera))]
	public class Grid : MonoBehaviour
	{
		// Token: 0x060005BF RID: 1471 RVA: 0x00020103 File Offset: 0x0001E503
		public Grid()
		{
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x00020114 File Offset: 0x0001E514
		private void Start()
		{
			this.m_camera = base.GetComponent<Camera>();
			if (this.SceneCamera == null)
			{
				this.SceneCamera = Camera.main;
			}
			if (this.SceneCamera == null)
			{
				Debug.LogError("SceneCamera is null");
				base.enabled = false;
				return;
			}
			this.m_camera.clearFlags = CameraClearFlags.Nothing;
			this.m_camera.renderingPath = RenderingPath.Forward;
			this.m_camera.cullingMask = 0;
			this.SetupCamera();
			if ((double)this.m_camera.depth != 0.01)
			{
				this.m_camera.depth = 0.01f;
			}
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x000201C0 File Offset: 0x0001E5C0
		private void OnPreRender()
		{
			this.m_camera.farClipPlane = RuntimeHandles.GetGridFarPlane();
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x000201D4 File Offset: 0x0001E5D4
		private void OnPostRender()
		{
			if (this.AutoCamOffset)
			{
				RuntimeHandles.DrawGrid(this.GridOffset, Camera.current.transform.position.y);
			}
			else
			{
				RuntimeHandles.DrawGrid(this.GridOffset, this.CamOffset);
			}
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x00020224 File Offset: 0x0001E624
		private void Update()
		{
			this.SetupCamera();
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x0002022C File Offset: 0x0001E62C
		private void SetupCamera()
		{
			this.m_camera.transform.position = this.SceneCamera.transform.position;
			this.m_camera.transform.rotation = this.SceneCamera.transform.rotation;
			this.m_camera.transform.localScale = this.SceneCamera.transform.localScale;
			if (this.m_camera.fieldOfView != this.SceneCamera.fieldOfView)
			{
				this.m_camera.fieldOfView = this.SceneCamera.fieldOfView;
			}
			if (this.m_camera.orthographic != this.SceneCamera.orthographic)
			{
				this.m_camera.orthographic = this.SceneCamera.orthographic;
			}
			if (this.m_camera.orthographicSize != this.SceneCamera.orthographicSize)
			{
				this.m_camera.orthographicSize = this.SceneCamera.orthographicSize;
			}
			if (this.m_camera.rect != this.SceneCamera.rect)
			{
				this.m_camera.rect = this.SceneCamera.rect;
			}
		}

		// Token: 0x04000529 RID: 1321
		private Camera m_camera;

		// Token: 0x0400052A RID: 1322
		public Camera SceneCamera;

		// Token: 0x0400052B RID: 1323
		public float CamOffset;

		// Token: 0x0400052C RID: 1324
		public bool AutoCamOffset = true;

		// Token: 0x0400052D RID: 1325
		public Vector3 GridOffset;
	}
}
