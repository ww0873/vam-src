using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Battlehub.RTCommon
{
	// Token: 0x020000C1 RID: 193
	public class RuntimeEditorWindow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
	{
		// Token: 0x0600035E RID: 862 RVA: 0x00014E81 File Offset: 0x00013281
		public RuntimeEditorWindow()
		{
		}

		// Token: 0x0600035F RID: 863 RVA: 0x00014E89 File Offset: 0x00013289
		private void Awake()
		{
			RuntimeEditorApplication.AddWindow(this);
			this.AwakeOverride();
		}

		// Token: 0x06000360 RID: 864 RVA: 0x00014E97 File Offset: 0x00013297
		private void OnDestroy()
		{
			RuntimeEditorApplication.ActivateWindow(null);
			RuntimeEditorApplication.PointerExit(this);
			RuntimeEditorApplication.RemoveWindow(this);
			this.OnDestroyOverride();
		}

		// Token: 0x06000361 RID: 865 RVA: 0x00014EB4 File Offset: 0x000132B4
		private void Update()
		{
			if (this.WindowType == RuntimeWindowType.GameView)
			{
				if (RuntimeEditorApplication.GameCameras == null || RuntimeEditorApplication.GameCameras.Length == 0)
				{
					return;
				}
				Rect pixelRect = RuntimeEditorApplication.GameCameras[0].pixelRect;
				this.UpdateState(pixelRect, true);
			}
			else if (this.WindowType == RuntimeWindowType.SceneView)
			{
				if (RuntimeEditorApplication.ActiveSceneCamera == null)
				{
					if (!(Camera.main != null))
					{
						return;
					}
					RuntimeEditorApplication.SceneCameras = new Camera[]
					{
						Camera.main
					};
				}
				Rect pixelRect2 = RuntimeEditorApplication.ActiveSceneCamera.pixelRect;
				this.UpdateState(pixelRect2, false);
			}
			else if (this.WindowType == RuntimeWindowType.None)
			{
				if (Camera.main == null)
				{
					return;
				}
				Rect pixelRect3 = Camera.main.pixelRect;
				this.UpdateState(pixelRect3, false);
			}
			else
			{
				if (this.WindowType == RuntimeWindowType.Other)
				{
					return;
				}
				if (this.m_isPointerOver && (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)))
				{
					RuntimeEditorApplication.ActivateWindow(this);
				}
			}
			this.UpdateOverride();
		}

		// Token: 0x06000362 RID: 866 RVA: 0x00014FF5 File Offset: 0x000133F5
		void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
		{
			if (this.WindowType == RuntimeWindowType.SceneView || this.WindowType == RuntimeWindowType.GameView)
			{
				return;
			}
			RuntimeEditorApplication.ActivateWindow(this);
			this.OnPointerDownOverride(eventData);
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0001501D File Offset: 0x0001341D
		void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
		{
			if (this.WindowType == RuntimeWindowType.SceneView || this.WindowType == RuntimeWindowType.GameView)
			{
				return;
			}
			this.m_isPointerOver = true;
			RuntimeEditorApplication.PointerEnter(this);
			this.OnPointerEnterOverride(eventData);
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0001504C File Offset: 0x0001344C
		void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
		{
			if (this.WindowType == RuntimeWindowType.SceneView || this.WindowType == RuntimeWindowType.GameView)
			{
				return;
			}
			this.m_isPointerOver = false;
			RuntimeEditorApplication.PointerExit(this);
			this.OnPointerExitOverride(eventData);
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0001507B File Offset: 0x0001347B
		void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
		{
			this.OnPointerUpOverride(eventData);
		}

		// Token: 0x06000366 RID: 870 RVA: 0x00015084 File Offset: 0x00013484
		protected virtual void AwakeOverride()
		{
		}

		// Token: 0x06000367 RID: 871 RVA: 0x00015086 File Offset: 0x00013486
		protected virtual void OnDestroyOverride()
		{
		}

		// Token: 0x06000368 RID: 872 RVA: 0x00015088 File Offset: 0x00013488
		protected virtual void UpdateOverride()
		{
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0001508A File Offset: 0x0001348A
		protected virtual void OnPointerDownOverride(PointerEventData eventData)
		{
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0001508C File Offset: 0x0001348C
		protected virtual void OnPointerUpOverride(PointerEventData eventData)
		{
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0001508E File Offset: 0x0001348E
		protected virtual void OnPointerEnterOverride(PointerEventData eventData)
		{
		}

		// Token: 0x0600036C RID: 876 RVA: 0x00015090 File Offset: 0x00013490
		protected virtual void OnPointerExitOverride(PointerEventData eventData)
		{
		}

		// Token: 0x0600036D RID: 877 RVA: 0x00015094 File Offset: 0x00013494
		private void UpdateState(Rect cameraRect, bool isGameView)
		{
			bool flag = cameraRect.Contains(Input.mousePosition) && !RuntimeTools.IsPointerOverGameObject();
			if (RuntimeEditorApplication.IsPointerOverWindow(this))
			{
				if (!flag)
				{
					RuntimeEditorApplication.PointerExit(this);
				}
			}
			else if (flag)
			{
				RuntimeEditorApplication.PointerEnter(this);
			}
			if (flag && (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)) && (!isGameView || (isGameView && RuntimeEditorApplication.IsPlaying)))
			{
				RuntimeEditorApplication.ActivateWindow(this);
			}
		}

		// Token: 0x040003F5 RID: 1013
		public RuntimeWindowType WindowType;

		// Token: 0x040003F6 RID: 1014
		private bool m_isPointerOver;
	}
}
