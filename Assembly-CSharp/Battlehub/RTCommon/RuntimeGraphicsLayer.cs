using System;
using UnityEngine;

namespace Battlehub.RTCommon
{
	// Token: 0x020000C3 RID: 195
	public class RuntimeGraphicsLayer : MonoBehaviour
	{
		// Token: 0x06000377 RID: 887 RVA: 0x000158B7 File Offset: 0x00013CB7
		public RuntimeGraphicsLayer()
		{
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000378 RID: 888 RVA: 0x000158C7 File Offset: 0x00013CC7
		public int GraphicsLayer
		{
			get
			{
				return this.m_graphicsLayer;
			}
		}

		// Token: 0x06000379 RID: 889 RVA: 0x000158D0 File Offset: 0x00013CD0
		private void UpdateCameraCullingMask()
		{
			this.m_sceneCamera.cullingMask &= ~(1 << this.m_graphicsLayer);
			this.m_graphicsLayerCamera.cullingMask = 1 << this.m_graphicsLayer;
			if (RuntimeEditorApplication.GameCameras != null)
			{
				for (int i = 0; i < RuntimeEditorApplication.GameCameras.Length; i++)
				{
					RuntimeEditorApplication.GameCameras[i].cullingMask &= ~(1 << this.m_graphicsLayer);
				}
			}
		}

		// Token: 0x0600037A RID: 890 RVA: 0x00015952 File Offset: 0x00013D52
		private void Awake()
		{
			RuntimeEditorApplication.ActiveSceneCameraChanged += this.OnActiveSceneCameraChanged;
			this.PrepareGraphicsLayerCamera();
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0001596B File Offset: 0x00013D6B
		private void OnDestroy()
		{
			RuntimeEditorApplication.ActiveSceneCameraChanged -= this.OnActiveSceneCameraChanged;
		}

		// Token: 0x0600037C RID: 892 RVA: 0x0001597E File Offset: 0x00013D7E
		private void OnActiveSceneCameraChanged()
		{
			this.PrepareGraphicsLayerCamera();
		}

		// Token: 0x0600037D RID: 893 RVA: 0x00015988 File Offset: 0x00013D88
		private void PrepareGraphicsLayerCamera()
		{
			this.m_sceneCamera = RuntimeEditorApplication.ActiveSceneCamera;
			if (this.m_sceneCamera == null)
			{
				this.m_sceneCamera = Camera.main;
			}
			if (this.m_sceneCamera != null)
			{
				UnityEngine.Object.Destroy(this.m_graphicsLayerCamera);
				this.m_graphicsLayerCamera = UnityEngine.Object.Instantiate<Camera>(this.m_sceneCamera, this.m_sceneCamera.transform);
				for (int i = this.m_graphicsLayerCamera.transform.childCount - 1; i >= 0; i--)
				{
					UnityEngine.Object.Destroy(this.m_graphicsLayerCamera.transform.GetChild(i).gameObject);
				}
				foreach (Component component in this.m_graphicsLayerCamera.GetComponents<Component>())
				{
					if (!(component is Transform))
					{
						if (!(component is Camera))
						{
							UnityEngine.Object.Destroy(component);
						}
					}
				}
				this.m_graphicsLayerCamera.clearFlags = CameraClearFlags.Depth;
				this.m_graphicsLayerCamera.transform.localPosition = Vector3.zero;
				this.m_graphicsLayerCamera.transform.localRotation = Quaternion.identity;
				this.m_graphicsLayerCamera.transform.localScale = Vector3.one;
				this.m_graphicsLayerCamera.name = "GraphicsLayerCamera";
				this.UpdateCameraCullingMask();
			}
		}

		// Token: 0x0600037E RID: 894 RVA: 0x00015AE0 File Offset: 0x00013EE0
		private void Update()
		{
			if (this.m_graphicsLayerCamera.fieldOfView != this.m_sceneCamera.fieldOfView)
			{
				this.m_graphicsLayerCamera.fieldOfView = this.m_sceneCamera.fieldOfView;
			}
			if (this.m_graphicsLayerCamera.orthographic != this.m_sceneCamera.orthographic)
			{
				this.m_graphicsLayerCamera.orthographic = this.m_sceneCamera.orthographic;
			}
			if (this.m_graphicsLayerCamera.orthographicSize != this.m_sceneCamera.orthographicSize)
			{
				this.m_graphicsLayerCamera.orthographicSize = this.m_sceneCamera.orthographicSize;
			}
			if (this.m_graphicsLayerCamera.rect != this.m_sceneCamera.rect)
			{
				this.m_graphicsLayerCamera.rect = this.m_sceneCamera.rect;
			}
			if (this.m_graphicsLayerCamera.enabled != this.m_sceneCamera.enabled)
			{
				this.m_graphicsLayerCamera.enabled = this.m_sceneCamera.enabled;
			}
		}

		// Token: 0x040003F9 RID: 1017
		private Camera m_sceneCamera;

		// Token: 0x040003FA RID: 1018
		private Camera m_graphicsLayerCamera;

		// Token: 0x040003FB RID: 1019
		[SerializeField]
		private int m_graphicsLayer = 24;
	}
}
