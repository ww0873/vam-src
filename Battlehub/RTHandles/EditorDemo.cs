using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Battlehub.RTCommon;
using Battlehub.RTSaveLoad;
using Battlehub.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Battlehub.RTHandles
{
	// Token: 0x020000ED RID: 237
	[DisallowMultipleComponent]
	public class EditorDemo : MonoBehaviour
	{
		// Token: 0x060004FF RID: 1279 RVA: 0x0001BBFC File Offset: 0x00019FFC
		public EditorDemo()
		{
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000500 RID: 1280 RVA: 0x0001BCBC File Offset: 0x0001A0BC
		public KeyCode ModifierKey
		{
			get
			{
				return this.RuntimeModifierKey;
			}
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x0001BCC4 File Offset: 0x0001A0C4
		private void OnAwaked(ExposeToEditor obj)
		{
			if (this.IsInPlayMode)
			{
				if (obj.ObjectType == ExposeToEditorObjectType.Undefined)
				{
					obj.ObjectType = ExposeToEditorObjectType.PlayMode;
				}
			}
			else if (obj.ObjectType == ExposeToEditorObjectType.Undefined)
			{
				obj.ObjectType = ExposeToEditorObjectType.EditorMode;
			}
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x0001BCFA File Offset: 0x0001A0FA
		private void OnDestroyed(ExposeToEditor obj)
		{
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000503 RID: 1283 RVA: 0x0001BCFC File Offset: 0x0001A0FC
		private bool IsInPlayMode
		{
			get
			{
				return this.m_game != null;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000504 RID: 1284 RVA: 0x0001BD0A File Offset: 0x0001A10A
		public Vector3 Pivot
		{
			get
			{
				return this.m_pivot;
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000505 RID: 1285 RVA: 0x0001BD12 File Offset: 0x0001A112
		// (set) Token: 0x06000506 RID: 1286 RVA: 0x0001BD19 File Offset: 0x0001A119
		public bool AutoFocus
		{
			get
			{
				return RuntimeTools.AutoFocus;
			}
			set
			{
				RuntimeTools.AutoFocus = value;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000507 RID: 1287 RVA: 0x0001BD21 File Offset: 0x0001A121
		// (set) Token: 0x06000508 RID: 1288 RVA: 0x0001BD28 File Offset: 0x0001A128
		public bool AutoUnitSnapping
		{
			get
			{
				return RuntimeTools.UnitSnapping;
			}
			set
			{
				RuntimeTools.UnitSnapping = value;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000509 RID: 1289 RVA: 0x0001BD30 File Offset: 0x0001A130
		// (set) Token: 0x0600050A RID: 1290 RVA: 0x0001BD37 File Offset: 0x0001A137
		public bool BoundingBoxSnapping
		{
			get
			{
				return RuntimeTools.IsSnapping;
			}
			set
			{
				RuntimeTools.IsSnapping = value;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x0600050B RID: 1291 RVA: 0x0001BD3F File Offset: 0x0001A13F
		// (set) Token: 0x0600050C RID: 1292 RVA: 0x0001BD47 File Offset: 0x0001A147
		public bool EnableCharacters
		{
			get
			{
				return this.m_enableCharacters;
			}
			set
			{
				if (this.m_enableCharacters == value)
				{
					return;
				}
				this.m_enableCharacters = value;
				if (EditorDemo.<>f__am$cache0 == null)
				{
					EditorDemo.<>f__am$cache0 = new Action<GameObject>(EditorDemo.<set_EnableCharacters>m__0);
				}
				EditorDemo.ForEachSelectedObject(EditorDemo.<>f__am$cache0);
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x0600050D RID: 1293 RVA: 0x0001BD7F File Offset: 0x0001A17F
		// (set) Token: 0x0600050E RID: 1294 RVA: 0x0001BD86 File Offset: 0x0001A186
		public bool ShowSelectionGizmos
		{
			get
			{
				return RuntimeTools.ShowSelectionGizmos;
			}
			set
			{
				RuntimeTools.ShowSelectionGizmos = value;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x0600050F RID: 1295 RVA: 0x0001BD8E File Offset: 0x0001A18E
		// (set) Token: 0x06000510 RID: 1296 RVA: 0x0001BD98 File Offset: 0x0001A198
		public bool IsGlobalPivotRotation
		{
			get
			{
				return RuntimeTools.PivotRotation == RuntimePivotRotation.Global;
			}
			set
			{
				if (value)
				{
					RuntimeTools.PivotRotation = RuntimePivotRotation.Global;
				}
				else
				{
					RuntimeTools.PivotRotation = RuntimePivotRotation.Local;
				}
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000511 RID: 1297 RVA: 0x0001BDB1 File Offset: 0x0001A1B1
		// (set) Token: 0x06000512 RID: 1298 RVA: 0x0001BDB8 File Offset: 0x0001A1B8
		public static EditorDemo Instance
		{
			[CompilerGenerated]
			get
			{
				return EditorDemo.<Instance>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				EditorDemo.<Instance>k__BackingField = value;
			}
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x0001BDC0 File Offset: 0x0001A1C0
		private void Awake()
		{
			EditorDemo.Instance = this;
			IEnumerable<GameObject> source = ExposeToEditor.FindAll(ExposeToEditorObjectType.Undefined, false);
			if (EditorDemo.<>f__am$cache1 == null)
			{
				EditorDemo.<>f__am$cache1 = new Func<GameObject, ExposeToEditor>(EditorDemo.<Awake>m__1);
			}
			ExposeToEditor[] array = source.Select(EditorDemo.<>f__am$cache1).ToArray<ExposeToEditor>();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].ObjectType = ExposeToEditorObjectType.EditorMode;
			}
			RuntimeTools.SnappingMode = SnappingMode.BoundingBox;
			RuntimeEditorApplication.IsOpened = !this.IsInPlayMode;
			RuntimeEditorApplication.SceneCameras = new Camera[]
			{
				this.EditorCamera
			};
			RuntimeEditorApplication.PlaymodeStateChanged += this.OnPlaymodeStateChanged;
			RuntimeEditorApplication.IsOpenedChanged += this.OnIsOpenedChanged;
			RuntimeSelection.SelectionChanged += this.OnRuntimeSelectionChanged;
			RuntimeTools.ToolChanged += this.OnRuntimeToolChanged;
			RuntimeTools.PivotRotationChanged += this.OnPivotRotationChanged;
			RuntimeUndo.UndoCompleted += this.OnUndoCompleted;
			RuntimeUndo.RedoCompleted += this.OnRedoCompleted;
			RuntimeUndo.StateChanged += this.OnUndoRedoStateChanged;
			this.TransformPanel.SetActive(RuntimeSelection.activeTransform != null);
			if (this.Prefabs != null && this.PrefabsPanel != null && this.PrefabPresenter != null)
			{
				IEnumerable<GameObject> prefabs = this.Prefabs;
				if (EditorDemo.<>f__am$cache2 == null)
				{
					EditorDemo.<>f__am$cache2 = new Func<GameObject, bool>(EditorDemo.<Awake>m__2);
				}
				this.Prefabs = prefabs.Where(EditorDemo.<>f__am$cache2).ToArray<GameObject>();
				for (int j = 0; j < this.Prefabs.Length; j++)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.PrefabPresenter);
					gameObject.transform.SetParent(this.PrefabsPanel.transform);
					gameObject.transform.position = Vector3.zero;
					gameObject.transform.localScale = Vector3.one;
					InstantiatePrefab componentInChildren = gameObject.GetComponentInChildren<InstantiatePrefab>();
					if (componentInChildren != null)
					{
						componentInChildren.Prefab = this.Prefabs[j];
					}
					TakeSnapshot componentInChildren2 = gameObject.GetComponentInChildren<TakeSnapshot>();
					if (componentInChildren2 != null)
					{
						componentInChildren2.TargetPrefab = this.Prefabs[j];
					}
				}
			}
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x0001BFE8 File Offset: 0x0001A3E8
		private void Start()
		{
			Vector3 a = new Vector3(1f, 1f, 1f);
			if (!(this.SelectionController is RuntimeSceneView))
			{
				this.EditorCamera.transform.position = this.m_pivot + a * this.EditorCamDistance;
				this.EditorCamera.transform.LookAt(this.m_pivot);
				RuntimeTools.DrawSelectionGizmoRay = true;
			}
			this.UpdateUIState(this.IsInPlayMode);
			this.AutoFocus = this.TogAutoFocus.isOn;
			this.AutoUnitSnapping = this.TogUnitSnap.isOn;
			this.BoundingBoxSnapping = this.TogBoundingBoxSnap.isOn;
			this.ShowSelectionGizmos = this.TogShowGizmos.isOn;
			this.EnableCharacters = this.TogEnableCharacters.isOn;
			ExposeToEditor.Awaked += this.OnAwaked;
			ExposeToEditor.Destroyed += this.OnDestroyed;
			this.m_sceneManager = Dependencies.SceneManager;
			if (this.m_sceneManager != null)
			{
				this.m_sceneManager.ActiveScene.Name = this.SaveFileName;
				this.m_sceneManager.Exists(this.m_sceneManager.ActiveScene, new ProjectManagerCallback<bool>(this.<Start>m__3));
			}
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x0001C134 File Offset: 0x0001A534
		private void OnDestroy()
		{
			if (EditorDemo.Instance == this)
			{
				EditorDemo.Instance = null;
				RuntimeEditorApplication.Reset();
			}
			RuntimeEditorApplication.PlaymodeStateChanged -= this.OnPlaymodeStateChanged;
			RuntimeEditorApplication.IsOpenedChanged -= this.OnIsOpenedChanged;
			RuntimeSelection.SelectionChanged -= this.OnRuntimeSelectionChanged;
			RuntimeTools.ToolChanged -= this.OnRuntimeToolChanged;
			RuntimeTools.PivotRotationChanged -= this.OnPivotRotationChanged;
			RuntimeUndo.RedoCompleted -= this.OnUndoCompleted;
			RuntimeUndo.RedoCompleted -= this.OnRedoCompleted;
			RuntimeUndo.StateChanged -= this.OnUndoRedoStateChanged;
			ExposeToEditor.Awaked -= this.OnAwaked;
			ExposeToEditor.Destroyed -= this.OnDestroyed;
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x0001C208 File Offset: 0x0001A608
		private void Update()
		{
			if (InputController.GetKeyDown(this.EnterPlayModeKey) && InputController.GetKey(this.ModifierKey))
			{
				this.TogglePlayMode();
			}
			if (this.IsInPlayMode)
			{
				return;
			}
			if (this.BoundingBoxSnapping != this.TogBoundingBoxSnap.isOn)
			{
				this.TogBoundingBoxSnap.isOn = this.BoundingBoxSnapping;
			}
			if (InputController.GetKeyDown(this.DuplicateKey))
			{
				if (InputController.GetKey(this.ModifierKey))
				{
					this.Duplicate();
				}
			}
			else if (InputController.GetKeyDown(this.SnapToGridKey))
			{
				if (InputController.GetKey(this.ModifierKey))
				{
					this.SnapToGrid();
				}
			}
			else if (InputController.GetKeyDown(this.DeleteKey))
			{
				this.Delete();
			}
			if (!(this.SelectionController is RuntimeSceneView))
			{
				float axis = Input.GetAxis("Mouse ScrollWheel");
				if (axis != 0f)
				{
					this.EditorCamera.orthographicSize -= axis * this.EditorCamera.orthographicSize;
					if (this.EditorCamera.orthographicSize < 0.2f)
					{
						this.EditorCamera.orthographicSize = 0.2f;
					}
					else if (this.EditorCamera.orthographicSize > 15f)
					{
						this.EditorCamera.orthographicSize = 15f;
					}
				}
				if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
				{
					this.m_dragPlane = new Plane(Vector3.up, this.m_pivot);
					this.m_pan = this.GetPointOnDragPlane(Input.mousePosition, out this.m_prevMouse);
					this.m_prevMouse = Input.mousePosition;
					CursorHelper.SetCursor(this, this.PanTexture, Vector2.zero, CursorMode.Auto);
				}
				else if (Input.GetMouseButton(1) || Input.GetMouseButton(2))
				{
					Vector3 a;
					Vector3 b;
					if (this.m_pan && this.GetPointOnDragPlane(Input.mousePosition, out a) && this.GetPointOnDragPlane(this.m_prevMouse, out b))
					{
						Vector3 b2 = a - b;
						this.m_prevMouse = Input.mousePosition;
						this.m_panOffset -= b2;
						this.EditorCamera.transform.position -= b2;
					}
				}
				else if (Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2))
				{
					this.m_pan = false;
					CursorHelper.ResetCursor(this);
				}
				if (InputController.GetKey(this.UpKey))
				{
					Vector3 position = this.EditorCamera.transform.position;
					position.y += this.PanSpeed * Time.deltaTime;
					this.m_panOffset.y = this.m_panOffset.y + this.PanSpeed * Time.deltaTime;
					this.EditorCamera.transform.position = position;
				}
				if (InputController.GetKey(this.DownKey))
				{
					Vector3 position2 = this.EditorCamera.transform.position;
					position2.y -= this.PanSpeed * Time.deltaTime;
					this.m_panOffset.y = this.m_panOffset.y - this.PanSpeed * Time.deltaTime;
					this.EditorCamera.transform.position = position2;
				}
				if (InputController.GetKey(this.LeftKey))
				{
					this.MoveMinZ();
					this.MovePlusX();
				}
				if (InputController.GetKey(this.RightKey))
				{
					this.MovePlusZ();
					this.MoveMinX();
				}
				if (InputController.GetKey(this.FwdKey))
				{
					this.MoveMinX();
					this.MoveMinZ();
				}
				if (InputController.GetKey(this.BwdKey))
				{
					this.MovePlusX();
					this.MovePlusZ();
				}
				if (InputController.GetKeyDown(this.FocusKey))
				{
					this.Focus();
				}
				else if (this.AutoFocus)
				{
					if (!(RuntimeTools.ActiveTool != null))
					{
						if (!(this.m_autoFocusTranform == null))
						{
							if (!(this.m_autoFocusTranform.position == this.m_pivot))
							{
								if (this.m_focusAnimations[0] != null && !this.m_focusAnimations[0].InProgress)
								{
									Vector3 b3 = this.m_autoFocusTranform.position - this.m_pivot - this.m_panOffset;
									this.EditorCamera.transform.position += b3;
									this.m_pivot += b3;
									this.m_panOffset = Vector3.zero;
								}
							}
						}
					}
				}
			}
			if (RuntimeSelection.activeTransform != null)
			{
				Vector3 gridOffset = this.Grid.GridOffset;
				gridOffset.y = RuntimeSelection.activeTransform.position.y;
				this.Grid.GridOffset = gridOffset;
			}
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x0001C70C File Offset: 0x0001AB0C
		private bool GetPointOnDragPlane(Vector3 mouse, out Vector3 point)
		{
			Ray ray = this.EditorCamera.ScreenPointToRay(mouse);
			float distance;
			if (this.m_dragPlane.Raycast(ray, out distance))
			{
				point = ray.GetPoint(distance);
				return true;
			}
			point = Vector3.zero;
			return false;
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x0001C758 File Offset: 0x0001AB58
		public void MovePlusX()
		{
			Vector3 position = this.EditorCamera.transform.position;
			position.x += this.PanSpeed * Time.deltaTime;
			this.m_panOffset.x = this.m_panOffset.x + this.PanSpeed * Time.deltaTime;
			this.EditorCamera.transform.position = position;
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x0001C7C0 File Offset: 0x0001ABC0
		public void MoveMinX()
		{
			Vector3 position = this.EditorCamera.transform.position;
			position.x -= this.PanSpeed * Time.deltaTime;
			this.m_panOffset.x = this.m_panOffset.x - this.PanSpeed * Time.deltaTime;
			this.EditorCamera.transform.position = position;
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x0001C828 File Offset: 0x0001AC28
		public void MovePlusZ()
		{
			Vector3 position = this.EditorCamera.transform.position;
			position.z += this.PanSpeed * Time.deltaTime;
			this.m_panOffset.z = this.m_panOffset.z + this.PanSpeed * Time.deltaTime;
			this.EditorCamera.transform.position = position;
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x0001C890 File Offset: 0x0001AC90
		public void MoveMinZ()
		{
			Vector3 position = this.EditorCamera.transform.position;
			position.z -= this.PanSpeed * Time.deltaTime;
			this.m_panOffset.z = this.m_panOffset.z - this.PanSpeed * Time.deltaTime;
			this.EditorCamera.transform.position = position;
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x0001C8F8 File Offset: 0x0001ACF8
		public void Duplicate()
		{
			GameObject[] gameObjects = RuntimeSelection.gameObjects;
			if (gameObjects == null)
			{
				return;
			}
			RuntimeUndo.BeginRecord();
			for (int i = 0; i < gameObjects.Length; i++)
			{
				GameObject gameObject = gameObjects[i];
				if (gameObject != null)
				{
					Transform parent = gameObject.transform.parent;
					GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject, gameObject.transform.position, gameObject.transform.rotation);
					gameObject2.transform.SetParent(parent, true);
					gameObjects[i] = gameObject2;
					RuntimeUndo.BeginRegisterCreateObject(gameObject2);
				}
			}
			RuntimeUndo.RecordSelection();
			RuntimeUndo.EndRecord();
			bool enabled = RuntimeUndo.Enabled;
			RuntimeUndo.Enabled = false;
			RuntimeSelection.objects = gameObjects;
			RuntimeUndo.Enabled = enabled;
			RuntimeUndo.BeginRecord();
			foreach (GameObject gameObject3 in gameObjects)
			{
				if (gameObject3 != null)
				{
					RuntimeUndo.RegisterCreatedObject(gameObject3);
				}
			}
			RuntimeUndo.RecordSelection();
			RuntimeUndo.EndRecord();
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x0001C9E4 File Offset: 0x0001ADE4
		public void Delete()
		{
			GameObject[] gameObjects = RuntimeSelection.gameObjects;
			if (gameObjects == null)
			{
				return;
			}
			RuntimeUndo.BeginRecord();
			foreach (GameObject gameObject in gameObjects)
			{
				if (gameObject != null)
				{
					RuntimeUndo.BeginDestroyObject(gameObject);
				}
			}
			RuntimeUndo.RecordSelection();
			RuntimeUndo.EndRecord();
			bool enabled = RuntimeUndo.Enabled;
			RuntimeUndo.Enabled = false;
			RuntimeSelection.objects = null;
			RuntimeUndo.Enabled = enabled;
			RuntimeUndo.BeginRecord();
			foreach (GameObject gameObject2 in gameObjects)
			{
				if (gameObject2 != null)
				{
					RuntimeUndo.DestroyObject(gameObject2);
				}
			}
			RuntimeUndo.RecordSelection();
			RuntimeUndo.EndRecord();
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x0001CA90 File Offset: 0x0001AE90
		public void TogglePlayMode()
		{
			RuntimeEditorApplication.IsPlaying = !RuntimeEditorApplication.IsPlaying;
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x0001CA9F File Offset: 0x0001AE9F
		private void OnIsOpenedChanged()
		{
			RuntimeEditorApplication.IsPlaying = !RuntimeEditorApplication.IsOpened;
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x0001CAB0 File Offset: 0x0001AEB0
		private void OnPlaymodeStateChanged()
		{
			this.UpdateUIState(this.m_game == null);
			RuntimeEditorApplication.IsOpened = !RuntimeEditorApplication.IsPlaying;
			if (this.m_game == null)
			{
				this.m_game = UnityEngine.Object.Instantiate<GameObject>(this.GamePrefab);
			}
			else
			{
				UnityEngine.Object.DestroyImmediate(this.m_game);
				this.m_game = null;
			}
			RuntimeEditorApplication.IsOpened = !this.IsInPlayMode;
			if (BoxSelection.Current != null)
			{
				BoxSelection.Current.gameObject.SetActive(!this.IsInPlayMode);
			}
			if (this.IsInPlayMode)
			{
				RuntimeSelection.objects = null;
				RuntimeUndo.Purge();
				this.m_playerCameraPostion = this.PlayerCamera.transform.position;
				this.m_playerCameraRotation = this.PlayerCamera.transform.rotation;
			}
			else
			{
				this.PlayerCamera.transform.position = this.m_playerCameraPostion;
				this.PlayerCamera.transform.rotation = this.m_playerCameraRotation;
			}
			this.SaveButton.interactable = false;
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x0001CBCC File Offset: 0x0001AFCC
		public void Focus()
		{
			RuntimeSceneView runtimeSceneView = this.SelectionController as RuntimeSceneView;
			if (runtimeSceneView != null)
			{
				runtimeSceneView.Focus();
				return;
			}
			if (RuntimeSelection.activeTransform == null)
			{
				return;
			}
			this.m_autoFocusTranform = RuntimeSelection.activeTransform;
			Vector3 b = RuntimeSelection.activeTransform.position - this.m_pivot - this.m_panOffset;
			Run.Instance.Remove(this.m_focusAnimations[0]);
			Run.Instance.Remove(this.m_focusAnimations[1]);
			Run.Instance.Remove(this.m_focusAnimations[2]);
			IAnimationInfo[] focusAnimations = this.m_focusAnimations;
			int num = 0;
			Vector3 position = this.EditorCamera.transform.position;
			Vector3 to = this.EditorCamera.transform.position + b;
			float duration = 0.1f;
			if (EditorDemo.<>f__mg$cache0 == null)
			{
				EditorDemo.<>f__mg$cache0 = new Func<float, float>(AnimationInfo<object, Vector3>.EaseOutCubic);
			}
			focusAnimations[num] = new Vector3AnimationInfo(position, to, duration, EditorDemo.<>f__mg$cache0, new AnimationCallback<object, Vector3>(this.<Focus>m__4), null);
			IAnimationInfo[] focusAnimations2 = this.m_focusAnimations;
			int num2 = 1;
			Vector3 pivot = this.m_pivot;
			Vector3 position2 = RuntimeSelection.activeTransform.position;
			float duration2 = 0.1f;
			if (EditorDemo.<>f__mg$cache1 == null)
			{
				EditorDemo.<>f__mg$cache1 = new Func<float, float>(AnimationInfo<object, Vector3>.EaseOutCubic);
			}
			focusAnimations2[num2] = new Vector3AnimationInfo(pivot, position2, duration2, EditorDemo.<>f__mg$cache1, new AnimationCallback<object, Vector3>(this.<Focus>m__5), null);
			IAnimationInfo[] focusAnimations3 = this.m_focusAnimations;
			int num3 = 2;
			Vector3 panOffset = this.m_panOffset;
			Vector3 zero = Vector3.zero;
			float duration3 = 0.1f;
			if (EditorDemo.<>f__mg$cache2 == null)
			{
				EditorDemo.<>f__mg$cache2 = new Func<float, float>(AnimationInfo<object, Vector3>.EaseOutCubic);
			}
			focusAnimations3[num3] = new Vector3AnimationInfo(panOffset, zero, duration3, EditorDemo.<>f__mg$cache2, new AnimationCallback<object, Vector3>(this.<Focus>m__6), null);
			Run.Instance.Animation(this.m_focusAnimations[0]);
			Run.Instance.Animation(this.m_focusAnimations[1]);
			Run.Instance.Animation(this.m_focusAnimations[2]);
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x0001CD96 File Offset: 0x0001B196
		private void OnRuntimeSelectionChanged(UnityEngine.Object[] unselectedObjects)
		{
			this.TransformPanel.SetActive(RuntimeSelection.activeTransform != null);
			this.TogPivotRotation.isOn = this.IsGlobalPivotRotation;
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x0001CDBF File Offset: 0x0001B1BF
		private void OnPivotRotationChanged()
		{
			this.TogPivotRotation.isOn = this.IsGlobalPivotRotation;
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x0001CDD4 File Offset: 0x0001B1D4
		private void OnRuntimeToolChanged()
		{
			if (RuntimeTools.Current == RuntimeTool.None || RuntimeTools.Current == RuntimeTool.View)
			{
				this.TxtCurrentControl.text = "none";
				this.ResetButton.gameObject.SetActive(false);
			}
			else if (RuntimeTools.Current == RuntimeTool.Move)
			{
				this.TxtCurrentControl.text = "move";
				this.ResetButton.gameObject.SetActive(true);
			}
			else if (RuntimeTools.Current == RuntimeTool.Rotate)
			{
				this.TxtCurrentControl.text = "rotate";
				this.ResetButton.gameObject.SetActive(true);
			}
			else if (RuntimeTools.Current == RuntimeTool.Scale)
			{
				this.TxtCurrentControl.text = "scale";
				this.ResetButton.gameObject.SetActive(true);
			}
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x0001CEAC File Offset: 0x0001B2AC
		public void SwitchControl()
		{
			if (RuntimeTools.Current == RuntimeTool.None || RuntimeTools.Current == RuntimeTool.View)
			{
				RuntimeTools.Current = RuntimeTool.Move;
				this.TxtCurrentControl.text = "move";
			}
			else if (RuntimeTools.Current == RuntimeTool.Move)
			{
				RuntimeTools.Current = RuntimeTool.Rotate;
				this.TxtCurrentControl.text = "rotate";
			}
			else if (RuntimeTools.Current == RuntimeTool.Rotate)
			{
				RuntimeTools.Current = RuntimeTool.Scale;
				this.TxtCurrentControl.text = "scale";
			}
			else if (RuntimeTools.Current == RuntimeTool.Scale)
			{
				RuntimeTools.Current = RuntimeTool.View;
				this.TxtCurrentControl.text = "none";
			}
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x0001CF58 File Offset: 0x0001B358
		public void ResetPosition()
		{
			if (RuntimeTools.Current == RuntimeTool.Move)
			{
				if (EditorDemo.<>f__am$cache3 == null)
				{
					EditorDemo.<>f__am$cache3 = new Action<GameObject>(EditorDemo.<ResetPosition>m__7);
				}
				EditorDemo.ForEachSelectedObject(EditorDemo.<>f__am$cache3);
			}
			else if (RuntimeTools.Current == RuntimeTool.Rotate)
			{
				if (EditorDemo.<>f__am$cache4 == null)
				{
					EditorDemo.<>f__am$cache4 = new Action<GameObject>(EditorDemo.<ResetPosition>m__8);
				}
				EditorDemo.ForEachSelectedObject(EditorDemo.<>f__am$cache4);
			}
			else if (RuntimeTools.Current == RuntimeTool.Scale)
			{
				if (EditorDemo.<>f__am$cache5 == null)
				{
					EditorDemo.<>f__am$cache5 = new Action<GameObject>(EditorDemo.<ResetPosition>m__9);
				}
				EditorDemo.ForEachSelectedObject(EditorDemo.<>f__am$cache5);
			}
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x0001CFF8 File Offset: 0x0001B3F8
		public void SnapToGrid()
		{
			GameObject[] gameObjects = RuntimeSelection.gameObjects;
			if (gameObjects == null || gameObjects.Length == 0)
			{
				return;
			}
			Transform transform = gameObjects[0].transform;
			Vector3 position = transform.position;
			position.x = Mathf.Round(position.x);
			position.y = Mathf.Round(position.y);
			position.z = Mathf.Round(position.z);
			Vector3 b = position - transform.position;
			for (int i = 0; i < gameObjects.Length; i++)
			{
				gameObjects[i].transform.position += b;
			}
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x0001D0A4 File Offset: 0x0001B4A4
		private static void ForEachSelectedObject(Action<GameObject> execute)
		{
			GameObject[] gameObjects = RuntimeSelection.gameObjects;
			if (gameObjects == null)
			{
				return;
			}
			foreach (GameObject gameObject in gameObjects)
			{
				if (gameObject != null)
				{
					execute(gameObject);
				}
			}
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x0001D0EC File Offset: 0x0001B4EC
		public void Save()
		{
			if (!this.ConfirmationSave.activeSelf)
			{
				this.ConfirmationSave.SetActive(true);
				return;
			}
			RuntimeUndo.Purge();
			this.ConfirmationSave.SetActive(false);
			if (this.m_sceneManager != null)
			{
				this.m_sceneManager.ActiveScene.Name = this.SaveFileName;
				this.m_sceneManager.SaveScene(this.m_sceneManager.ActiveScene, new ProjectManagerCallback(this.<Save>m__A));
			}
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x0001D16C File Offset: 0x0001B56C
		public void Load()
		{
			if (!this.ConfirmationLoad.activeSelf)
			{
				this.ConfirmationLoad.SetActive(true);
				return;
			}
			RuntimeUndo.Purge();
			IEnumerable<GameObject> source = ExposeToEditor.FindAll(ExposeToEditorObjectType.EditorMode, true);
			if (EditorDemo.<>f__am$cache6 == null)
			{
				EditorDemo.<>f__am$cache6 = new Func<GameObject, ExposeToEditor>(EditorDemo.<Load>m__B);
			}
			foreach (ExposeToEditor exposeToEditor in source.Select(EditorDemo.<>f__am$cache6).ToArray<ExposeToEditor>())
			{
				if (exposeToEditor != null)
				{
					UnityEngine.Object.DestroyImmediate(exposeToEditor.gameObject);
				}
			}
			this.ConfirmationLoad.SetActive(false);
			if (this.m_sceneManager != null)
			{
				this.m_sceneManager.ActiveScene.Name = this.SaveFileName;
				this.m_sceneManager.LoadScene(this.m_sceneManager.ActiveScene, new ProjectManagerCallback(this.<Load>m__C));
			}
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x0001D248 File Offset: 0x0001B648
		public void Undo()
		{
			RuntimeUndo.Undo();
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x0001D24F File Offset: 0x0001B64F
		public void Redo()
		{
			RuntimeUndo.Redo();
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x0001D258 File Offset: 0x0001B658
		private void OnUndoCompleted()
		{
			this.UndoButton.interactable = RuntimeUndo.CanUndo;
			this.RedoButton.interactable = RuntimeUndo.CanRedo;
			this.SaveButton.interactable = (this.m_sceneManager != null);
			this.LoadButton.interactable = (this.m_sceneManager != null && this.m_saveFileExists);
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x0001D2BC File Offset: 0x0001B6BC
		private void OnRedoCompleted()
		{
			this.UndoButton.interactable = RuntimeUndo.CanUndo;
			this.RedoButton.interactable = RuntimeUndo.CanRedo;
			this.SaveButton.interactable = (this.m_sceneManager != null);
			this.LoadButton.interactable = (this.m_sceneManager != null && this.m_saveFileExists);
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x0001D320 File Offset: 0x0001B720
		private void OnUndoRedoStateChanged()
		{
			this.UndoButton.interactable = RuntimeUndo.CanUndo;
			this.RedoButton.interactable = RuntimeUndo.CanRedo;
			this.SaveButton.interactable = (this.m_sceneManager != null);
			this.LoadButton.interactable = (this.m_sceneManager != null && this.m_saveFileExists);
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x0001D384 File Offset: 0x0001B784
		private void UpdateUIState(bool isInPlayMode)
		{
			if (this.ProjectionButton != null)
			{
				this.ProjectionButton.gameObject.SetActive(!isInPlayMode);
			}
			this.EditorCamera.gameObject.SetActive(!isInPlayMode);
			this.PlayerCamera.gameObject.SetActive(isInPlayMode);
			this.SelectionController.gameObject.SetActive(!isInPlayMode);
			this.PlayButton.gameObject.SetActive(!isInPlayMode);
			this.HintButton.gameObject.SetActive(!isInPlayMode);
			this.SaveButton.gameObject.SetActive(!isInPlayMode);
			this.LoadButton.gameObject.SetActive(!isInPlayMode);
			this.StopButton.gameObject.SetActive(isInPlayMode);
			this.UndoButton.gameObject.SetActive(!isInPlayMode);
			this.RedoButton.gameObject.SetActive(!isInPlayMode);
			this.UI.gameObject.SetActive(!isInPlayMode);
			this.Grid.gameObject.SetActive(this.TogGrid.isOn && !isInPlayMode);
			this.LoadButton.interactable = (this.m_sceneManager != null && this.m_saveFileExists);
			if (isInPlayMode)
			{
				RuntimeEditorApplication.ActivateWindow(RuntimeWindowType.GameView);
			}
			else
			{
				RuntimeEditorApplication.ActivateWindow(RuntimeWindowType.SceneView);
			}
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x0001D4EC File Offset: 0x0001B8EC
		[CompilerGenerated]
		private static void <set_EnableCharacters>m__0(GameObject go)
		{
			ExposeToEditor component = go.GetComponent<ExposeToEditor>();
			if (component)
			{
				component.Unselected.Invoke(component);
				component.Selected.Invoke(component);
			}
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x0001D523 File Offset: 0x0001B923
		[CompilerGenerated]
		private static ExposeToEditor <Awake>m__1(GameObject go)
		{
			return go.GetComponent<ExposeToEditor>();
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x0001D52B File Offset: 0x0001B92B
		[CompilerGenerated]
		private static bool <Awake>m__2(GameObject p)
		{
			return p != null;
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x0001D534 File Offset: 0x0001B934
		[CompilerGenerated]
		private void <Start>m__3(bool exists)
		{
			this.m_saveFileExists = exists;
			this.LoadButton.interactable = exists;
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x0001D549 File Offset: 0x0001B949
		[CompilerGenerated]
		private void <Focus>m__4(object target, Vector3 value, float t, bool completed)
		{
			this.EditorCamera.transform.position = value;
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x0001D55C File Offset: 0x0001B95C
		[CompilerGenerated]
		private void <Focus>m__5(object target, Vector3 value, float t, bool completed)
		{
			this.m_pivot = value;
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x0001D565 File Offset: 0x0001B965
		[CompilerGenerated]
		private void <Focus>m__6(object target, Vector3 value, float t, bool completed)
		{
			this.m_panOffset = value;
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x0001D56E File Offset: 0x0001B96E
		[CompilerGenerated]
		private static void <ResetPosition>m__7(GameObject go)
		{
			go.transform.position = Vector3.zero;
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x0001D580 File Offset: 0x0001B980
		[CompilerGenerated]
		private static void <ResetPosition>m__8(GameObject go)
		{
			go.transform.rotation = Quaternion.identity;
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x0001D592 File Offset: 0x0001B992
		[CompilerGenerated]
		private static void <ResetPosition>m__9(GameObject go)
		{
			go.transform.localScale = Vector3.one;
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x0001D5A4 File Offset: 0x0001B9A4
		[CompilerGenerated]
		private void <Save>m__A()
		{
			this.SaveButton.interactable = false;
			this.m_saveFileExists = true;
			this.LoadButton.interactable = true;
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x0001D5C5 File Offset: 0x0001B9C5
		[CompilerGenerated]
		private static ExposeToEditor <Load>m__B(GameObject go)
		{
			return go.GetComponent<ExposeToEditor>();
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x0001D5CD File Offset: 0x0001B9CD
		[CompilerGenerated]
		private void <Load>m__C()
		{
			this.SaveButton.interactable = false;
		}

		// Token: 0x04000494 RID: 1172
		[SerializeField]
		private string SaveFileName = "RTHandlesEditorDemo";

		// Token: 0x04000495 RID: 1173
		private bool m_saveFileExists;

		// Token: 0x04000496 RID: 1174
		public GameObject[] Prefabs;

		// Token: 0x04000497 RID: 1175
		public GameObject PrefabsPanel;

		// Token: 0x04000498 RID: 1176
		public GameObject PrefabPresenter;

		// Token: 0x04000499 RID: 1177
		public GameObject GamePrefab;

		// Token: 0x0400049A RID: 1178
		public RuntimeSelectionComponent SelectionController;

		// Token: 0x0400049B RID: 1179
		public KeyCode EditorModifierKey = KeyCode.LeftShift;

		// Token: 0x0400049C RID: 1180
		public KeyCode RuntimeModifierKey = KeyCode.LeftControl;

		// Token: 0x0400049D RID: 1181
		public KeyCode SnapToGridKey = KeyCode.S;

		// Token: 0x0400049E RID: 1182
		public KeyCode DuplicateKey = KeyCode.D;

		// Token: 0x0400049F RID: 1183
		public KeyCode DeleteKey = KeyCode.Delete;

		// Token: 0x040004A0 RID: 1184
		public KeyCode EnterPlayModeKey = KeyCode.P;

		// Token: 0x040004A1 RID: 1185
		public KeyCode FocusKey = KeyCode.F;

		// Token: 0x040004A2 RID: 1186
		public KeyCode LeftKey = KeyCode.LeftArrow;

		// Token: 0x040004A3 RID: 1187
		public KeyCode RightKey = KeyCode.RightArrow;

		// Token: 0x040004A4 RID: 1188
		public KeyCode FwdKey = KeyCode.UpArrow;

		// Token: 0x040004A5 RID: 1189
		public KeyCode BwdKey = KeyCode.DownArrow;

		// Token: 0x040004A6 RID: 1190
		public KeyCode UpKey = KeyCode.PageUp;

		// Token: 0x040004A7 RID: 1191
		public KeyCode DownKey = KeyCode.PageDown;

		// Token: 0x040004A8 RID: 1192
		public float PanSpeed = 10f;

		// Token: 0x040004A9 RID: 1193
		public Texture2D PanTexture;

		// Token: 0x040004AA RID: 1194
		private Vector3 m_panOffset;

		// Token: 0x040004AB RID: 1195
		private bool m_pan;

		// Token: 0x040004AC RID: 1196
		private Plane m_dragPlane;

		// Token: 0x040004AD RID: 1197
		private Vector3 m_prevMouse;

		// Token: 0x040004AE RID: 1198
		private Vector3 m_playerCameraPostion;

		// Token: 0x040004AF RID: 1199
		private Quaternion m_playerCameraRotation;

		// Token: 0x040004B0 RID: 1200
		public Camera PlayerCamera;

		// Token: 0x040004B1 RID: 1201
		public Camera EditorCamera;

		// Token: 0x040004B2 RID: 1202
		public Grid Grid;

		// Token: 0x040004B3 RID: 1203
		public Button ProjectionButton;

		// Token: 0x040004B4 RID: 1204
		public Button PlayButton;

		// Token: 0x040004B5 RID: 1205
		public Button HintButton;

		// Token: 0x040004B6 RID: 1206
		public Button StopButton;

		// Token: 0x040004B7 RID: 1207
		public Button SaveButton;

		// Token: 0x040004B8 RID: 1208
		public Button LoadButton;

		// Token: 0x040004B9 RID: 1209
		public Button UndoButton;

		// Token: 0x040004BA RID: 1210
		public Button RedoButton;

		// Token: 0x040004BB RID: 1211
		public Button ResetButton;

		// Token: 0x040004BC RID: 1212
		public GameObject UI;

		// Token: 0x040004BD RID: 1213
		public GameObject TransformPanel;

		// Token: 0x040004BE RID: 1214
		public GameObject BottomPanel;

		// Token: 0x040004BF RID: 1215
		public Toggle TogAutoFocus;

		// Token: 0x040004C0 RID: 1216
		public Toggle TogUnitSnap;

		// Token: 0x040004C1 RID: 1217
		public Toggle TogBoundingBoxSnap;

		// Token: 0x040004C2 RID: 1218
		public Toggle TogEnableCharacters;

		// Token: 0x040004C3 RID: 1219
		public Toggle TogGrid;

		// Token: 0x040004C4 RID: 1220
		public Toggle TogShowGizmos;

		// Token: 0x040004C5 RID: 1221
		public Toggle TogPivotRotation;

		// Token: 0x040004C6 RID: 1222
		public Text TxtCurrentControl;

		// Token: 0x040004C7 RID: 1223
		public GameObject ConfirmationSave;

		// Token: 0x040004C8 RID: 1224
		public GameObject ConfirmationLoad;

		// Token: 0x040004C9 RID: 1225
		private GameObject m_game;

		// Token: 0x040004CA RID: 1226
		private ISceneManager m_sceneManager;

		// Token: 0x040004CB RID: 1227
		private Vector3 m_pivot;

		// Token: 0x040004CC RID: 1228
		public float EditorCamDistance = 10f;

		// Token: 0x040004CD RID: 1229
		private IAnimationInfo[] m_focusAnimations = new IAnimationInfo[3];

		// Token: 0x040004CE RID: 1230
		private Transform m_autoFocusTranform;

		// Token: 0x040004CF RID: 1231
		private bool m_enableCharacters;

		// Token: 0x040004D0 RID: 1232
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static EditorDemo <Instance>k__BackingField;

		// Token: 0x040004D1 RID: 1233
		[CompilerGenerated]
		private static Action<GameObject> <>f__am$cache0;

		// Token: 0x040004D2 RID: 1234
		[CompilerGenerated]
		private static Func<GameObject, ExposeToEditor> <>f__am$cache1;

		// Token: 0x040004D3 RID: 1235
		[CompilerGenerated]
		private static Func<GameObject, bool> <>f__am$cache2;

		// Token: 0x040004D4 RID: 1236
		[CompilerGenerated]
		private static Func<float, float> <>f__mg$cache0;

		// Token: 0x040004D5 RID: 1237
		[CompilerGenerated]
		private static Func<float, float> <>f__mg$cache1;

		// Token: 0x040004D6 RID: 1238
		[CompilerGenerated]
		private static Func<float, float> <>f__mg$cache2;

		// Token: 0x040004D7 RID: 1239
		[CompilerGenerated]
		private static Action<GameObject> <>f__am$cache3;

		// Token: 0x040004D8 RID: 1240
		[CompilerGenerated]
		private static Action<GameObject> <>f__am$cache4;

		// Token: 0x040004D9 RID: 1241
		[CompilerGenerated]
		private static Action<GameObject> <>f__am$cache5;

		// Token: 0x040004DA RID: 1242
		[CompilerGenerated]
		private static Func<GameObject, ExposeToEditor> <>f__am$cache6;
	}
}
