using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Battlehub.RTCommon;
using Battlehub.Utils;
using UnityEngine;

namespace Battlehub.RTHandles
{
	// Token: 0x02000106 RID: 262
	public class RuntimeSelectionComponent : RuntimeEditorWindow
	{
		// Token: 0x06000637 RID: 1591 RVA: 0x000272F4 File Offset: 0x000256F4
		public RuntimeSelectionComponent()
		{
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000638 RID: 1592 RVA: 0x00027346 File Offset: 0x00025746
		public KeyCode ModifierKey
		{
			get
			{
				return this.RuntimeModifierKey;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000639 RID: 1593 RVA: 0x0002734E File Offset: 0x0002574E
		protected virtual LayerMask LayerMask
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x0600063A RID: 1594 RVA: 0x00027356 File Offset: 0x00025756
		protected virtual bool IPointerOverEditorArea
		{
			get
			{
				return !RuntimeTools.IsPointerOverGameObject();
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x0600063B RID: 1595 RVA: 0x00027360 File Offset: 0x00025760
		protected PositionHandle PositionHandle
		{
			get
			{
				return this.m_positionHandle;
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x0600063C RID: 1596 RVA: 0x00027368 File Offset: 0x00025768
		protected RotationHandle RotationHandle
		{
			get
			{
				return this.m_rotationHandle;
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x0600063D RID: 1597 RVA: 0x00027370 File Offset: 0x00025770
		protected ScaleHandle ScaleHandle
		{
			get
			{
				return this.m_scaleHandle;
			}
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x00027378 File Offset: 0x00025778
		private void Start()
		{
			if (BoxSelection.Current == null)
			{
				GameObject gameObject = new GameObject();
				gameObject.name = "BoxSelection";
				gameObject.transform.SetParent(base.transform, false);
				gameObject.AddComponent<BoxSelection>();
			}
			this.StartOverride();
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x000273C5 File Offset: 0x000257C5
		private void OnEnable()
		{
			this.OnEnableOverride();
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x000273CD File Offset: 0x000257CD
		private void OnDisable()
		{
			this.OnDisableOverride();
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x000273D8 File Offset: 0x000257D8
		private void LateUpdate()
		{
			if (Input.GetMouseButtonDown(0))
			{
				if (RuntimeTools.ActiveTool != null && RuntimeTools.ActiveTool != BoxSelection.Current)
				{
					return;
				}
				if (!this.IPointerOverEditorArea)
				{
					return;
				}
				if (RuntimeTools.IsViewing)
				{
					return;
				}
				bool key = InputController.GetKey(this.RangeSelectKey);
				bool flag = InputController.GetKey(this.MultiselectKey) || InputController.GetKey(this.MultiselectKey2) || key;
				Ray ray = this.SceneCamera.ScreenPointToRay(Input.mousePosition);
				RaycastHit raycastHit;
				if (Physics.Raycast(ray, out raycastHit, 3.4028235E+38f))
				{
					GameObject gameObject = raycastHit.collider.gameObject;
					bool flag2 = this.CanSelect(gameObject);
					if (flag2)
					{
						if (flag)
						{
							List<UnityEngine.Object> list;
							if (RuntimeSelection.objects != null)
							{
								list = RuntimeSelection.objects.ToList<UnityEngine.Object>();
							}
							else
							{
								list = new List<UnityEngine.Object>();
							}
							if (list.Contains(gameObject))
							{
								list.Remove(gameObject);
								if (key)
								{
									list.Insert(0, gameObject);
								}
							}
							else
							{
								list.Insert(0, gameObject);
							}
							RuntimeSelection.Select(gameObject, list.ToArray());
						}
						else
						{
							RuntimeSelection.activeObject = gameObject;
						}
					}
					else if (!flag)
					{
						RuntimeSelection.activeObject = null;
					}
				}
				else if (!flag)
				{
					RuntimeSelection.activeObject = null;
				}
			}
			if (RuntimeEditorApplication.IsActiveWindow(this) && InputController.GetKeyDown(this.SelectAllKey) && InputController.GetKey(this.ModifierKey))
			{
				IEnumerable<GameObject> source = (!RuntimeEditorApplication.IsPlaying) ? ExposeToEditor.FindAll(ExposeToEditorObjectType.EditorMode, true) : ExposeToEditor.FindAll(ExposeToEditorObjectType.PlayMode, true);
				RuntimeSelection.objects = source.ToArray<GameObject>();
			}
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x00027590 File Offset: 0x00025990
		private void OnApplicationQuit()
		{
			BoxSelection.Filtering -= this.OnBoxSelectionFiltering;
			this.OnApplicationQuitOverride();
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x000275AC File Offset: 0x000259AC
		protected override void AwakeOverride()
		{
			base.AwakeOverride();
			if (this.SceneCamera == null)
			{
				this.SceneCamera = Camera.main;
			}
			if (this.HandlesParent == null)
			{
				this.HandlesParent = base.transform;
			}
			GameObject gameObject = new GameObject();
			gameObject.name = "PositionHandle";
			this.m_positionHandle = gameObject.AddComponent<PositionHandle>();
			this.m_positionHandle.Model = this.m_positonHandleModel;
			this.m_positionHandle.SceneCamera = this.SceneCamera;
			gameObject.SetActive(false);
			gameObject.transform.SetParent(this.HandlesParent);
			GameObject gameObject2 = new GameObject();
			gameObject2.name = "RotationHandle";
			this.m_rotationHandle = gameObject2.AddComponent<RotationHandle>();
			this.m_rotationHandle.Model = this.m_rotationHandleModel;
			this.m_rotationHandle.SceneCamera = this.SceneCamera;
			gameObject2.SetActive(false);
			gameObject2.transform.SetParent(this.HandlesParent);
			GameObject gameObject3 = new GameObject();
			gameObject3.name = "ScaleHandle";
			this.m_scaleHandle = gameObject3.AddComponent<ScaleHandle>();
			this.m_scaleHandle.Model = this.m_scaleHandleModel;
			this.m_scaleHandle.SceneCamera = this.SceneCamera;
			gameObject3.SetActive(false);
			gameObject3.transform.SetParent(this.HandlesParent);
			BoxSelection.Filtering += this.OnBoxSelectionFiltering;
			RuntimeSelection.SelectionChanged += this.OnRuntimeSelectionChanged;
			RuntimeTools.ToolChanged += this.OnRuntimeToolChanged;
			if (InputController.Instance == null)
			{
				base.gameObject.AddComponent<InputController>();
			}
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x0002774C File Offset: 0x00025B4C
		protected virtual void StartOverride()
		{
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x0002774E File Offset: 0x00025B4E
		protected virtual void OnEnableOverride()
		{
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x00027750 File Offset: 0x00025B50
		protected virtual void OnDisableOverride()
		{
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x00027752 File Offset: 0x00025B52
		private void OnApplicationQuitOverride()
		{
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x00027754 File Offset: 0x00025B54
		protected override void OnDestroyOverride()
		{
			base.OnDestroyOverride();
			BoxSelection.Filtering -= this.OnBoxSelectionFiltering;
			RuntimeTools.Current = RuntimeTool.None;
			RuntimeSelection.SelectionChanged -= this.OnRuntimeSelectionChanged;
			RuntimeTools.ToolChanged -= this.OnRuntimeToolChanged;
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x000277A0 File Offset: 0x00025BA0
		private void OnRuntimeToolChanged()
		{
			this.SetCursor();
			if (RuntimeSelection.activeTransform == null)
			{
				return;
			}
			if (this.m_positionHandle != null)
			{
				this.m_positionHandle.gameObject.SetActive(false);
				if (RuntimeTools.Current == RuntimeTool.Move)
				{
					this.m_positionHandle.transform.position = RuntimeSelection.activeTransform.position;
					this.m_positionHandle.Targets = this.GetTargets();
					this.m_positionHandle.gameObject.SetActive(this.m_positionHandle.Targets.Length > 0);
				}
			}
			if (this.m_rotationHandle != null)
			{
				this.m_rotationHandle.gameObject.SetActive(false);
				if (RuntimeTools.Current == RuntimeTool.Rotate)
				{
					this.m_rotationHandle.transform.position = RuntimeSelection.activeTransform.position;
					this.m_rotationHandle.Targets = this.GetTargets();
					this.m_rotationHandle.gameObject.SetActive(this.m_rotationHandle.Targets.Length > 0);
				}
			}
			if (this.m_scaleHandle != null)
			{
				this.m_scaleHandle.gameObject.SetActive(false);
				if (RuntimeTools.Current == RuntimeTool.Scale)
				{
					this.m_scaleHandle.transform.position = RuntimeSelection.activeTransform.position;
					this.m_scaleHandle.Targets = this.GetTargets();
					this.m_scaleHandle.gameObject.SetActive(this.m_scaleHandle.Targets.Length > 0);
				}
			}
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x0002792C File Offset: 0x00025D2C
		private void OnBoxSelectionFiltering(object sender, FilteringArgs e)
		{
			if (e.Object == null)
			{
				e.Cancel = true;
			}
			ExposeToEditor component = e.Object.GetComponent<ExposeToEditor>();
			if (!component || !component.CanSelect)
			{
				e.Cancel = true;
			}
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x0002797C File Offset: 0x00025D7C
		private void OnRuntimeSelectionChanged(UnityEngine.Object[] unselected)
		{
			if (unselected != null)
			{
				for (int i = 0; i < unselected.Length; i++)
				{
					GameObject gameObject = unselected[i] as GameObject;
					if (gameObject != null)
					{
						SelectionGizmo component = gameObject.GetComponent<SelectionGizmo>();
						if (component != null)
						{
							UnityEngine.Object.DestroyImmediate(component);
						}
						ExposeToEditor component2 = gameObject.GetComponent<ExposeToEditor>();
						if (component2 && component2.Unselected != null)
						{
							component2.Unselected.Invoke(component2);
						}
					}
				}
			}
			GameObject[] gameObjects = RuntimeSelection.gameObjects;
			if (gameObjects != null)
			{
				foreach (GameObject gameObject2 in gameObjects)
				{
					ExposeToEditor component3 = gameObject2.GetComponent<ExposeToEditor>();
					if (component3 && component3.CanSelect && !gameObject2.IsPrefab() && !gameObject2.isStatic)
					{
						SelectionGizmo selectionGizmo = gameObject2.GetComponent<SelectionGizmo>();
						if (selectionGizmo == null)
						{
							selectionGizmo = gameObject2.AddComponent<SelectionGizmo>();
						}
						selectionGizmo.SceneCamera = this.SceneCamera;
						if (component3.Selected != null)
						{
							component3.Selected.Invoke(component3);
						}
					}
				}
			}
			if (RuntimeSelection.activeGameObject == null || RuntimeSelection.activeGameObject.IsPrefab())
			{
				if (this.m_positionHandle != null)
				{
					this.m_positionHandle.gameObject.SetActive(false);
				}
				if (this.m_rotationHandle != null)
				{
					this.m_rotationHandle.gameObject.SetActive(false);
				}
				if (this.m_scaleHandle != null)
				{
					this.m_scaleHandle.gameObject.SetActive(false);
				}
			}
			else
			{
				this.OnRuntimeToolChanged();
			}
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x00027B37 File Offset: 0x00025F37
		protected virtual void SetCursor()
		{
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x00027B39 File Offset: 0x00025F39
		protected virtual bool CanSelect(GameObject go)
		{
			return go.GetComponent<ExposeToEditor>();
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x00027B48 File Offset: 0x00025F48
		protected virtual Transform[] GetTargets()
		{
			IEnumerable<GameObject> gameObjects = RuntimeSelection.gameObjects;
			if (RuntimeSelectionComponent.<>f__am$cache0 == null)
			{
				RuntimeSelectionComponent.<>f__am$cache0 = new Func<GameObject, Transform>(RuntimeSelectionComponent.<GetTargets>m__0);
			}
			IEnumerable<Transform> source = gameObjects.Select(RuntimeSelectionComponent.<>f__am$cache0);
			if (RuntimeSelectionComponent.<>f__am$cache1 == null)
			{
				RuntimeSelectionComponent.<>f__am$cache1 = new Func<Transform, bool>(RuntimeSelectionComponent.<GetTargets>m__1);
			}
			return source.OrderByDescending(RuntimeSelectionComponent.<>f__am$cache1).ToArray<Transform>();
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x00027BA4 File Offset: 0x00025FA4
		public virtual void SetSceneCamera(Camera camera)
		{
			this.SceneCamera = camera;
			if (this.m_positionHandle != null)
			{
				this.m_positionHandle.SceneCamera = camera;
			}
			if (this.m_rotationHandle != null)
			{
				this.m_rotationHandle.SceneCamera = camera;
			}
			if (this.m_scaleHandle != null)
			{
				this.m_scaleHandle.SceneCamera = camera;
			}
			GameObject[] gameObjects = RuntimeSelection.gameObjects;
			if (gameObjects != null)
			{
				foreach (GameObject gameObject in gameObjects)
				{
					ExposeToEditor component = gameObject.GetComponent<ExposeToEditor>();
					if (component && component.CanSelect && !gameObject.IsPrefab() && !gameObject.isStatic)
					{
						SelectionGizmo selectionGizmo = gameObject.GetComponent<SelectionGizmo>();
						if (selectionGizmo != null)
						{
							UnityEngine.Object.Destroy(selectionGizmo);
							selectionGizmo = gameObject.AddComponent<SelectionGizmo>();
						}
						if (selectionGizmo != null)
						{
							selectionGizmo.SceneCamera = this.SceneCamera;
						}
					}
				}
			}
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x00027CA4 File Offset: 0x000260A4
		[CompilerGenerated]
		private static Transform <GetTargets>m__0(GameObject g)
		{
			return g.transform;
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x00027CAC File Offset: 0x000260AC
		[CompilerGenerated]
		private static bool <GetTargets>m__1(Transform g)
		{
			return RuntimeSelection.activeTransform == g;
		}

		// Token: 0x040005F3 RID: 1523
		[SerializeField]
		private PositionHandleModel m_positonHandleModel;

		// Token: 0x040005F4 RID: 1524
		[SerializeField]
		private RotationHandleModel m_rotationHandleModel;

		// Token: 0x040005F5 RID: 1525
		[SerializeField]
		private ScaleHandleModel m_scaleHandleModel;

		// Token: 0x040005F6 RID: 1526
		public KeyCode RuntimeModifierKey = KeyCode.LeftControl;

		// Token: 0x040005F7 RID: 1527
		public KeyCode EditorModifierKey = KeyCode.LeftShift;

		// Token: 0x040005F8 RID: 1528
		public KeyCode SelectAllKey = KeyCode.A;

		// Token: 0x040005F9 RID: 1529
		public KeyCode MultiselectKey = KeyCode.LeftControl;

		// Token: 0x040005FA RID: 1530
		public KeyCode MultiselectKey2 = KeyCode.RightControl;

		// Token: 0x040005FB RID: 1531
		public KeyCode RangeSelectKey = KeyCode.LeftShift;

		// Token: 0x040005FC RID: 1532
		public Camera SceneCamera;

		// Token: 0x040005FD RID: 1533
		private PositionHandle m_positionHandle;

		// Token: 0x040005FE RID: 1534
		private RotationHandle m_rotationHandle;

		// Token: 0x040005FF RID: 1535
		private ScaleHandle m_scaleHandle;

		// Token: 0x04000600 RID: 1536
		public Transform HandlesParent;

		// Token: 0x04000601 RID: 1537
		[CompilerGenerated]
		private static Func<GameObject, Transform> <>f__am$cache0;

		// Token: 0x04000602 RID: 1538
		[CompilerGenerated]
		private static Func<Transform, bool> <>f__am$cache1;
	}
}
