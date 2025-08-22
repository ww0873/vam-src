using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MeshVR
{
	// Token: 0x02000C4D RID: 3149
	public class Selector : MonoBehaviour
	{
		// Token: 0x06005B97 RID: 23447 RVA: 0x0021A2C0 File Offset: 0x002186C0
		public Selector()
		{
		}

		// Token: 0x06005B98 RID: 23448 RVA: 0x0021A2DA File Offset: 0x002186DA
		public static void Activate()
		{
			if (Selector.singleton != null)
			{
				Selector.singleton.enabled = true;
			}
		}

		// Token: 0x06005B99 RID: 23449 RVA: 0x0021A2F7 File Offset: 0x002186F7
		public static void Deactivate()
		{
			if (Selector.singleton != null)
			{
				Selector.singleton.enabled = false;
			}
		}

		// Token: 0x17000D71 RID: 3441
		// (get) Token: 0x06005B9A RID: 23450 RVA: 0x0021A314 File Offset: 0x00218714
		// (set) Token: 0x06005B9B RID: 23451 RVA: 0x0021A31B File Offset: 0x0021871B
		public static bool hideBackfaces
		{
			get
			{
				return Selector._hideBackfaces;
			}
			set
			{
				if (Selector._hideBackfaces != value)
				{
					Selector._hideBackfaces = value;
				}
			}
		}

		// Token: 0x06005B9C RID: 23452 RVA: 0x0021A32E File Offset: 0x0021872E
		public static void RemoveAll()
		{
			Selector.selectables.Clear();
		}

		// Token: 0x06005B9D RID: 23453 RVA: 0x0021A33C File Offset: 0x0021873C
		private void Awake()
		{
			Selector.singleton = this;
			Selector.Deactivate();
			if (this.selectionBox != null)
			{
				this.rt = this.selectionBox.GetComponent<RectTransform>();
				this.rt.pivot = Vector2.one * 0.5f;
				this.rt.anchorMin = Vector2.zero;
				this.rt.anchorMax = Vector2.zero;
				this.selectionBox.gameObject.SetActive(false);
			}
		}

		// Token: 0x06005B9E RID: 23454 RVA: 0x0021A3C4 File Offset: 0x002187C4
		private void Update()
		{
			if (SuperController.singleton != null && SuperController.singleton.GetMouseSelect())
			{
				Ray ray = this.mouseCamera.ScreenPointToRay(Input.mousePosition);
				RaycastHit[] array = Physics.RaycastAll(ray, 50f, this.selectColliderMask);
				if (array != null && array.Length > 0)
				{
					foreach (RaycastHit raycastHit in array)
					{
						Selectable componentInParent = raycastHit.transform.GetComponentInParent<Selectable>();
						if (componentInParent != null)
						{
							this.UpdateSelection(componentInParent, !componentInParent.isSelected);
							return;
						}
					}
				}
				if (this.selectionBox == null)
				{
					return;
				}
				this.startScreenPos = Input.mousePosition;
				this.isSelecting = true;
			}
			if (this.selectionBox == null)
			{
				return;
			}
			if (SuperController.singleton != null && SuperController.singleton.GetMouseRelease())
			{
				this.isSelecting = false;
			}
			this.selectionBox.gameObject.SetActive(this.isSelecting);
			if (Selector._hideBackfaces)
			{
				Camera lookCamera;
				if (SuperController.singleton != null && SuperController.singleton.lookCamera != null)
				{
					lookCamera = SuperController.singleton.lookCamera;
				}
				else
				{
					lookCamera = Selector.singleton.mouseCamera;
				}
				if (lookCamera != null)
				{
					Vector3 forward = lookCamera.transform.forward;
					foreach (Selectable selectable in Selector.selectables)
					{
						if (Vector3.Dot(forward, selectable.transform.forward) > 0f)
						{
							selectable.isHidden = true;
						}
						else
						{
							selectable.isHidden = false;
						}
					}
				}
			}
			else
			{
				foreach (Selectable selectable2 in Selector.selectables)
				{
					selectable2.isHidden = false;
				}
			}
			if (this.isSelecting)
			{
				Bounds bounds = default(Bounds);
				if (this.selectMode == Selector.SelectMode.SelectBox)
				{
					bounds.center = Vector3.Lerp(this.startScreenPos, Input.mousePosition, 0.5f);
					bounds.size = new Vector3(Mathf.Abs(this.startScreenPos.x - Input.mousePosition.x), Mathf.Abs(this.startScreenPos.y - Input.mousePosition.y), 0f);
				}
				else
				{
					bounds.center = Input.mousePosition;
					Vector3 size;
					size.x = 60f;
					size.y = 60f;
					size.z = 0f;
					bounds.size = size;
				}
				if (this.canvas.renderMode == RenderMode.ScreenSpaceOverlay || this.canvas.renderMode == RenderMode.ScreenSpaceCamera)
				{
					this.rt.anchoredPosition = bounds.center;
					this.rt.sizeDelta = bounds.size;
				}
				else
				{
					Debug.LogError("Selector canvas is not in ScreenSpaceOverlay or ScreenSpcaeCamera mode");
				}
				bool key = Input.GetKey(this.removeKey);
				if (key)
				{
					this.selectionBox.color = Color.red;
				}
				else
				{
					this.selectionBox.color = Color.white;
				}
				foreach (Selectable selectable3 in Selector.selectables)
				{
					if (!selectable3.isHidden)
					{
						Vector3 point = this.mouseCamera.WorldToScreenPoint(selectable3.transform.position);
						point.z = 0f;
						if (key)
						{
							if (bounds.Contains(point))
							{
								this.UpdateSelection(selectable3, false);
							}
						}
						else if (bounds.Contains(point))
						{
							this.UpdateSelection(selectable3, true);
						}
					}
				}
			}
		}

		// Token: 0x06005B9F RID: 23455 RVA: 0x0021A83C File Offset: 0x00218C3C
		private void UpdateSelection(Selectable s, bool value)
		{
			s.isSelected = value;
		}

		// Token: 0x06005BA0 RID: 23456 RVA: 0x0021A845 File Offset: 0x00218C45
		// Note: this type is marked as 'beforefieldinit'.
		static Selector()
		{
		}

		// Token: 0x04004B81 RID: 19329
		public static Selector singleton;

		// Token: 0x04004B82 RID: 19330
		protected static bool _hideBackfaces = false;

		// Token: 0x04004B83 RID: 19331
		public static HashSet<Selectable> selectables = new HashSet<Selectable>();

		// Token: 0x04004B84 RID: 19332
		public Selector.SelectMode selectMode = Selector.SelectMode.PaintBox;

		// Token: 0x04004B85 RID: 19333
		public Camera mouseCamera;

		// Token: 0x04004B86 RID: 19334
		public Canvas canvas;

		// Token: 0x04004B87 RID: 19335
		public Image selectionBox;

		// Token: 0x04004B88 RID: 19336
		public LayerMask selectColliderMask;

		// Token: 0x04004B89 RID: 19337
		public KeyCode removeKey = KeyCode.LeftControl;

		// Token: 0x04004B8A RID: 19338
		private Vector3 startScreenPos;

		// Token: 0x04004B8B RID: 19339
		private BoxCollider worldCollider;

		// Token: 0x04004B8C RID: 19340
		private RectTransform rt;

		// Token: 0x04004B8D RID: 19341
		private bool isSelecting;

		// Token: 0x02000C4E RID: 3150
		public enum SelectMode
		{
			// Token: 0x04004B8F RID: 19343
			SelectBox,
			// Token: 0x04004B90 RID: 19344
			PaintBox
		}
	}
}
