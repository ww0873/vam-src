using System;
using Battlehub.RTCommon;
using UnityEngine;

namespace Battlehub.RTHandles
{
	// Token: 0x0200010B RID: 267
	public class SelectionGizmo : MonoBehaviour, IGL
	{
		// Token: 0x0600067F RID: 1663 RVA: 0x0002AD78 File Offset: 0x00029178
		public SelectionGizmo()
		{
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x0002AD87 File Offset: 0x00029187
		private void Awake()
		{
			if (this.SceneCamera == null)
			{
				this.SceneCamera = Camera.main;
			}
			this.m_exposeToEditor = base.GetComponent<ExposeToEditor>();
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x0002ADB4 File Offset: 0x000291B4
		private void Start()
		{
			if (GLRenderer.Instance == null)
			{
				new GameObject
				{
					name = "GLRenderer"
				}.AddComponent<GLRenderer>();
			}
			if (this.SceneCamera != null && !this.SceneCamera.GetComponent<GLCamera>())
			{
				this.SceneCamera.gameObject.AddComponent<GLCamera>();
			}
			if (this.m_exposeToEditor != null)
			{
				GLRenderer.Instance.Add(this);
			}
			if (!RuntimeSelection.IsSelected(base.gameObject))
			{
				UnityEngine.Object.Destroy(this);
			}
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x0002AE52 File Offset: 0x00029252
		private void OnEnable()
		{
			if (this.m_exposeToEditor != null && GLRenderer.Instance != null)
			{
				GLRenderer.Instance.Add(this);
			}
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x0002AE80 File Offset: 0x00029280
		private void OnDisable()
		{
			if (GLRenderer.Instance != null)
			{
				GLRenderer.Instance.Remove(this);
			}
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x0002AEA0 File Offset: 0x000292A0
		public void Draw(int cullingMask)
		{
			if (RuntimeTools.ShowSelectionGizmos)
			{
				RTLayer rtlayer = RTLayer.SceneView;
				if ((cullingMask & (int)rtlayer) == 0)
				{
					return;
				}
				Bounds bounds = this.m_exposeToEditor.Bounds;
				Transform transform = this.m_exposeToEditor.BoundsObject.transform;
				RuntimeHandles.DrawBounds(ref bounds, transform.position, transform.rotation, transform.lossyScale);
				if (RuntimeTools.DrawSelectionGizmoRay)
				{
					RuntimeHandles.DrawBoundRay(ref bounds, transform.TransformPoint(bounds.center), Quaternion.identity, transform.lossyScale);
				}
			}
		}

		// Token: 0x04000650 RID: 1616
		public bool DrawRay = true;

		// Token: 0x04000651 RID: 1617
		public Camera SceneCamera;

		// Token: 0x04000652 RID: 1618
		private ExposeToEditor m_exposeToEditor;
	}
}
