using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Battlehub.RTCommon;
using Battlehub.Utils;
using UnityEngine;

namespace Battlehub.RTHandles
{
	// Token: 0x020000F6 RID: 246
	public abstract class BaseHandle : MonoBehaviour, IGL
	{
		// Token: 0x06000562 RID: 1378 RVA: 0x0001DFB5 File Offset: 0x0001C3B5
		protected BaseHandle()
		{
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000563 RID: 1379 RVA: 0x0001DFE1 File Offset: 0x0001C3E1
		// (set) Token: 0x06000564 RID: 1380 RVA: 0x0001DFE9 File Offset: 0x0001C3E9
		private protected float EffectiveGridUnitSize
		{
			[CompilerGenerated]
			protected get
			{
				return this.<EffectiveGridUnitSize>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<EffectiveGridUnitSize>k__BackingField = value;
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000565 RID: 1381 RVA: 0x0001DFF2 File Offset: 0x0001C3F2
		// (set) Token: 0x06000566 RID: 1382 RVA: 0x0001DFF9 File Offset: 0x0001C3F9
		protected LockObject LockObject
		{
			get
			{
				return RuntimeTools.LockAxes;
			}
			set
			{
				RuntimeTools.LockAxes = value;
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000567 RID: 1383 RVA: 0x0001E001 File Offset: 0x0001C401
		// (set) Token: 0x06000568 RID: 1384 RVA: 0x0001E00E File Offset: 0x0001C40E
		protected virtual Vector3 HandlePosition
		{
			get
			{
				return base.transform.position;
			}
			set
			{
				base.transform.position = value;
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000569 RID: 1385 RVA: 0x0001E01C File Offset: 0x0001C41C
		protected Transform[] ActiveTargets
		{
			get
			{
				return this.m_activeTargets;
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x0600056A RID: 1386 RVA: 0x0001E024 File Offset: 0x0001C424
		protected Transform[] RealTargets
		{
			get
			{
				if (this.m_realTargets == null)
				{
					return this.Targets;
				}
				return this.m_realTargets;
			}
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x0001E040 File Offset: 0x0001C440
		private void GetActiveRealTargets()
		{
			if (this.m_realTargets == null)
			{
				this.m_activeRealTargets = null;
				return;
			}
			IEnumerable<Transform> realTargets = this.m_realTargets;
			if (BaseHandle.<>f__am$cache0 == null)
			{
				BaseHandle.<>f__am$cache0 = new Func<Transform, bool>(BaseHandle.<GetActiveRealTargets>m__0);
			}
			this.m_realTargets = realTargets.Where(BaseHandle.<>f__am$cache0).ToArray<Transform>();
			HashSet<Transform> hashSet = new HashSet<Transform>();
			for (int i = 0; i < this.m_realTargets.Length; i++)
			{
				if (this.m_realTargets[i] != null && !hashSet.Contains(this.m_realTargets[i]))
				{
					hashSet.Add(this.m_realTargets[i]);
				}
			}
			this.m_realTargets = hashSet.ToArray<Transform>();
			if (this.m_realTargets.Length == 0)
			{
				this.m_activeRealTargets = new Transform[0];
				return;
			}
			if (this.m_realTargets.Length == 1)
			{
				this.m_activeRealTargets = new Transform[]
				{
					this.m_realTargets[0]
				};
			}
			for (int j = 0; j < this.m_realTargets.Length; j++)
			{
				Transform transform = this.m_realTargets[j];
				Transform parent = transform.parent;
				while (parent != null)
				{
					if (hashSet.Contains(parent))
					{
						hashSet.Remove(transform);
						break;
					}
					parent = parent.parent;
				}
			}
			this.m_activeRealTargets = hashSet.ToArray<Transform>();
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x0600056C RID: 1388 RVA: 0x0001E19D File Offset: 0x0001C59D
		// (set) Token: 0x0600056D RID: 1389 RVA: 0x0001E1A8 File Offset: 0x0001C5A8
		public Transform[] Targets
		{
			get
			{
				return this.Targets_Internal;
			}
			set
			{
				this.DestroyCommonCenter();
				this.m_realTargets = value;
				this.GetActiveRealTargets();
				this.Targets_Internal = value;
				if (this.Targets_Internal == null || this.Targets_Internal.Length == 0)
				{
					return;
				}
				if (RuntimeTools.PivotMode == RuntimePivotMode.Center && this.ActiveTargets.Length > 1)
				{
					Vector3 vector = this.Targets_Internal[0].position;
					for (int i = 1; i < this.Targets_Internal.Length; i++)
					{
						vector += this.Targets_Internal[i].position;
					}
					vector /= (float)this.Targets_Internal.Length;
					this.m_commonCenter = new Transform[1];
					this.m_commonCenter[0] = new GameObject
					{
						name = "CommonCenter"
					}.transform;
					this.m_commonCenter[0].SetParent(base.transform.parent, true);
					this.m_commonCenter[0].position = vector;
					this.m_commonCenterTarget = new Transform[this.m_realTargets.Length];
					for (int j = 0; j < this.m_commonCenterTarget.Length; j++)
					{
						GameObject gameObject = new GameObject
						{
							name = "ActiveTarget " + this.m_realTargets[j].name
						};
						gameObject.transform.SetParent(this.m_commonCenter[0]);
						gameObject.transform.position = this.m_realTargets[j].position;
						gameObject.transform.rotation = this.m_realTargets[j].rotation;
						gameObject.transform.localScale = this.m_realTargets[j].localScale;
						this.m_commonCenterTarget[j] = gameObject.transform;
					}
					LockObject lockObject = this.LockObject;
					this.Targets_Internal = this.m_commonCenter;
					this.LockObject = lockObject;
				}
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x0600056E RID: 1390 RVA: 0x0001E379 File Offset: 0x0001C779
		// (set) Token: 0x0600056F RID: 1391 RVA: 0x0001E384 File Offset: 0x0001C784
		private Transform[] Targets_Internal
		{
			get
			{
				return this.m_targets;
			}
			set
			{
				this.m_targets = value;
				if (this.m_targets == null)
				{
					this.LockObject = LockAxes.Eval(null);
					this.m_activeTargets = null;
					return;
				}
				IEnumerable<Transform> targets = this.m_targets;
				if (BaseHandle.<>f__am$cache1 == null)
				{
					BaseHandle.<>f__am$cache1 = new Func<Transform, bool>(BaseHandle.<set_Targets_Internal>m__1);
				}
				this.m_targets = targets.Where(BaseHandle.<>f__am$cache1).ToArray<Transform>();
				HashSet<Transform> hashSet = new HashSet<Transform>();
				for (int i = 0; i < this.m_targets.Length; i++)
				{
					if (this.m_targets[i] != null && !hashSet.Contains(this.m_targets[i]))
					{
						hashSet.Add(this.m_targets[i]);
					}
				}
				this.m_targets = hashSet.ToArray<Transform>();
				if (this.m_targets.Length == 0)
				{
					this.LockObject = LockAxes.Eval(new LockAxes[0]);
					this.m_activeTargets = new Transform[0];
					return;
				}
				if (this.m_targets.Length == 1)
				{
					this.m_activeTargets = new Transform[]
					{
						this.m_targets[0]
					};
				}
				for (int j = 0; j < this.m_targets.Length; j++)
				{
					Transform transform = this.m_targets[j];
					Transform parent = transform.parent;
					while (parent != null)
					{
						if (hashSet.Contains(parent))
						{
							hashSet.Remove(transform);
							break;
						}
						parent = parent.parent;
					}
				}
				this.m_activeTargets = hashSet.ToArray<Transform>();
				IEnumerable<Transform> activeTargets = this.m_activeTargets;
				if (BaseHandle.<>f__am$cache2 == null)
				{
					BaseHandle.<>f__am$cache2 = new Func<Transform, bool>(BaseHandle.<set_Targets_Internal>m__2);
				}
				IEnumerable<Transform> source = activeTargets.Where(BaseHandle.<>f__am$cache2);
				if (BaseHandle.<>f__am$cache3 == null)
				{
					BaseHandle.<>f__am$cache3 = new Func<Transform, LockAxes>(BaseHandle.<set_Targets_Internal>m__3);
				}
				this.LockObject = LockAxes.Eval(source.Select(BaseHandle.<>f__am$cache3).ToArray<LockAxes>());
				IEnumerable<Transform> activeTargets2 = this.m_activeTargets;
				if (BaseHandle.<>f__am$cache4 == null)
				{
					BaseHandle.<>f__am$cache4 = new Func<Transform, bool>(BaseHandle.<set_Targets_Internal>m__4);
				}
				if (activeTargets2.Any(BaseHandle.<>f__am$cache4))
				{
					this.LockObject = new LockObject();
					this.LockObject.PositionX = (this.LockObject.PositionY = (this.LockObject.PositionZ = true));
					this.LockObject.RotationX = (this.LockObject.RotationY = (this.LockObject.RotationZ = true));
					this.LockObject.ScaleX = (this.LockObject.ScaleY = (this.LockObject.ScaleZ = true));
					this.LockObject.RotationScreen = true;
				}
				if (this.m_activeTargets != null && this.m_activeTargets.Length > 0)
				{
					base.transform.position = this.m_activeTargets[0].position;
				}
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000570 RID: 1392 RVA: 0x0001E658 File Offset: 0x0001CA58
		public Transform Target
		{
			get
			{
				if (this.Targets == null || this.Targets.Length == 0)
				{
					return null;
				}
				return this.Targets[0];
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000571 RID: 1393 RVA: 0x0001E67C File Offset: 0x0001CA7C
		public bool IsDragging
		{
			get
			{
				return this.m_isDragging;
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000572 RID: 1394
		protected abstract RuntimeTool Tool { get; }

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000573 RID: 1395 RVA: 0x0001E684 File Offset: 0x0001CA84
		protected Quaternion Rotation
		{
			get
			{
				if (this.Targets == null || this.Targets.Length <= 0 || this.Target == null)
				{
					return Quaternion.identity;
				}
				return (RuntimeTools.PivotRotation != RuntimePivotRotation.Local) ? Quaternion.identity : this.Target.rotation;
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000574 RID: 1396 RVA: 0x0001E6E0 File Offset: 0x0001CAE0
		// (set) Token: 0x06000575 RID: 1397 RVA: 0x0001E6E8 File Offset: 0x0001CAE8
		protected virtual RuntimeHandleAxis SelectedAxis
		{
			get
			{
				return this.m_selectedAxis;
			}
			set
			{
				this.m_selectedAxis = value;
				if (this.Model != null)
				{
					this.Model.Select(this.SelectedAxis);
				}
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000576 RID: 1398 RVA: 0x0001E713 File Offset: 0x0001CB13
		// (set) Token: 0x06000577 RID: 1399 RVA: 0x0001E71B File Offset: 0x0001CB1B
		protected Plane DragPlane
		{
			get
			{
				return this.m_dragPlane;
			}
			set
			{
				this.m_dragPlane = value;
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000578 RID: 1400
		protected abstract float CurrentGridUnitSize { get; }

		// Token: 0x06000579 RID: 1401 RVA: 0x0001E724 File Offset: 0x0001CB24
		private void Awake()
		{
			if (this.m_targets != null && this.m_targets.Length > 0)
			{
				this.Targets = this.m_targets;
			}
			RuntimeTools.PivotModeChanged += this.OnPivotModeChanged;
			RuntimeTools.ToolChanged += this.OnRuntimeToolChanged;
			RuntimeTools.LockAxesChanged += this.OnLockAxesChanged;
			this.AwakeOverride();
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x0001E794 File Offset: 0x0001CB94
		private void Start()
		{
			if (this.SceneCamera == null)
			{
				this.SceneCamera = Camera.main;
			}
			if (this.EnableUndo && !RuntimeUndoComponent.IsInitialized)
			{
				new GameObject
				{
					name = "RuntimeUndo"
				}.AddComponent<RuntimeUndoComponent>();
			}
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
			if (this.Targets == null || this.Targets.Length == 0)
			{
				this.Targets = new Transform[]
				{
					base.transform
				};
			}
			if (GLRenderer.Instance != null)
			{
				GLRenderer.Instance.Add(this);
			}
			if (this.Targets[0].position != base.transform.position)
			{
				base.transform.position = this.Targets[0].position;
			}
			if (this.Model != null)
			{
				BaseHandleModel baseHandleModel = UnityEngine.Object.Instantiate<BaseHandleModel>(this.Model, base.transform.parent);
				baseHandleModel.name = this.Model.name;
				this.Model = baseHandleModel;
				this.Model.SetLock(this.LockObject);
			}
			this.StartOverride();
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x0001E924 File Offset: 0x0001CD24
		private void OnEnable()
		{
			this.OnEnableOverride();
			if (this.Model != null)
			{
				this.Model.gameObject.SetActive(true);
				this.SyncModelTransform();
			}
			else if (GLRenderer.Instance != null)
			{
				GLRenderer.Instance.Add(this);
			}
			RuntimeUndo.UndoCompleted += this.OnUndoCompleted;
			RuntimeUndo.RedoCompleted += this.OnRedoCompleted;
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x0001E9A4 File Offset: 0x0001CDA4
		private void OnDisable()
		{
			if (GLRenderer.Instance != null)
			{
				GLRenderer.Instance.Remove(this);
			}
			this.DestroyCommonCenter();
			if (this.Model != null)
			{
				this.Model.gameObject.SetActive(false);
			}
			this.OnDisableOverride();
			RuntimeUndo.UndoCompleted -= this.OnUndoCompleted;
			RuntimeUndo.RedoCompleted -= this.OnRedoCompleted;
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x0001EA1C File Offset: 0x0001CE1C
		private void OnDestroy()
		{
			RuntimeTools.ToolChanged -= this.OnRuntimeToolChanged;
			RuntimeTools.PivotModeChanged -= this.OnPivotModeChanged;
			RuntimeTools.LockAxesChanged -= this.OnLockAxesChanged;
			if (GLRenderer.Instance != null)
			{
				GLRenderer.Instance.Remove(this);
			}
			if (RuntimeTools.ActiveTool == this)
			{
				RuntimeTools.ActiveTool = null;
			}
			this.DestroyCommonCenter();
			if (this.Model != null && this.Model.gameObject != null && !this.Model.gameObject.IsPrefab())
			{
				UnityEngine.Object.Destroy(this.Model);
			}
			this.OnDestroyOverride();
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x0001EAE4 File Offset: 0x0001CEE4
		private void DestroyCommonCenter()
		{
			if (this.m_commonCenter != null)
			{
				for (int i = 0; i < this.m_commonCenter.Length; i++)
				{
					UnityEngine.Object.Destroy(this.m_commonCenter[i].gameObject);
				}
			}
			if (this.m_commonCenterTarget != null)
			{
				for (int j = 0; j < this.m_commonCenterTarget.Length; j++)
				{
					UnityEngine.Object.Destroy(this.m_commonCenterTarget[j].gameObject);
				}
			}
			this.m_commonCenter = null;
			this.m_commonCenterTarget = null;
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x0001EB6C File Offset: 0x0001CF6C
		private void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				if ((RuntimeTools.Current != this.Tool && RuntimeTools.Current != RuntimeTool.None) || RuntimeTools.IsViewing)
				{
					return;
				}
				if (RuntimeTools.IsPointerOverGameObject())
				{
					return;
				}
				if (this.SceneCamera == null)
				{
					UnityEngine.Debug.LogError("Camera is null");
					return;
				}
				if (RuntimeTools.ActiveTool != null)
				{
					return;
				}
				if (RuntimeEditorApplication.ActiveSceneCamera != null && !RuntimeEditorApplication.IsPointerOverWindow(RuntimeWindowType.SceneView))
				{
					return;
				}
				this.m_isDragging = this.OnBeginDrag();
				if (this.m_isDragging)
				{
					RuntimeTools.ActiveTool = this;
					this.RecordTransform();
				}
				else
				{
					RuntimeTools.ActiveTool = null;
				}
			}
			else if (Input.GetMouseButtonUp(0))
			{
				this.TryCancelDrag();
			}
			else if (this.m_isDragging)
			{
				if (InputController.GetKey(this.UnitSnapKey) || RuntimeTools.UnitSnapping)
				{
					this.EffectiveGridUnitSize = this.CurrentGridUnitSize;
				}
				else
				{
					this.EffectiveGridUnitSize = 0f;
				}
				this.OnDrag();
			}
			this.UpdateOverride();
			if (this.Model != null)
			{
				this.SyncModelTransform();
			}
			if (this.m_isDragging && RuntimeTools.PivotMode == RuntimePivotMode.Center && this.m_commonCenterTarget != null && this.m_realTargets != null && this.m_realTargets.Length > 1)
			{
				for (int i = 0; i < this.m_commonCenterTarget.Length; i++)
				{
					Transform transform = this.m_commonCenterTarget[i];
					Transform transform2 = this.m_realTargets[i];
					transform2.transform.position = transform.position;
					transform2.transform.rotation = transform.rotation;
					transform2.transform.localScale = transform.localScale;
				}
			}
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x0001ED44 File Offset: 0x0001D144
		private void SyncModelTransform()
		{
			Vector3 handlePosition = this.HandlePosition;
			this.Model.transform.position = handlePosition;
			this.Model.transform.rotation = this.Rotation;
			float screenScale = RuntimeHandles.GetScreenScale(handlePosition, this.SceneCamera);
			this.Model.transform.localScale = Vector3.one * screenScale;
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x0001EDA7 File Offset: 0x0001D1A7
		private void TryCancelDrag()
		{
			if (this.m_isDragging)
			{
				this.OnDrop();
				this.RecordTransform();
				this.m_isDragging = false;
				RuntimeTools.ActiveTool = null;
			}
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x0001EDCD File Offset: 0x0001D1CD
		protected virtual void AwakeOverride()
		{
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x0001EDCF File Offset: 0x0001D1CF
		protected virtual void StartOverride()
		{
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x0001EDD1 File Offset: 0x0001D1D1
		protected virtual void OnEnableOverride()
		{
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x0001EDD3 File Offset: 0x0001D1D3
		protected virtual void OnDisableOverride()
		{
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x0001EDD5 File Offset: 0x0001D1D5
		protected virtual void OnDestroyOverride()
		{
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x0001EDD8 File Offset: 0x0001D1D8
		protected virtual void UpdateOverride()
		{
			if (this.Targets != null && this.Targets.Length > 0 && this.Targets[0] != null && this.Targets[0].position != base.transform.position)
			{
				if (this.IsDragging)
				{
					Vector3 b = base.transform.position - this.Targets[0].position;
					for (int i = 0; i < this.ActiveTargets.Length; i++)
					{
						if (this.ActiveTargets[i] != null)
						{
							this.ActiveTargets[i].position += b;
						}
					}
				}
				else
				{
					base.transform.position = this.Targets[0].position;
					base.transform.rotation = this.Targets[0].rotation;
				}
			}
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x0001EED6 File Offset: 0x0001D2D6
		protected virtual bool OnBeginDrag()
		{
			return false;
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x0001EED9 File Offset: 0x0001D2D9
		protected virtual void OnDrag()
		{
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x0001EEDB File Offset: 0x0001D2DB
		protected virtual void OnDrop()
		{
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x0001EEDD File Offset: 0x0001D2DD
		protected virtual void OnRuntimeToolChanged()
		{
			this.TryCancelDrag();
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x0001EEE8 File Offset: 0x0001D2E8
		protected virtual void OnPivotModeChanged()
		{
			if (this.RealTargets != null)
			{
				this.Targets = this.RealTargets;
			}
			if (RuntimeTools.PivotMode != RuntimePivotMode.Center)
			{
				this.m_realTargets = null;
			}
			if (this.Target != null)
			{
				base.transform.position = this.Target.position;
			}
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x0001EF44 File Offset: 0x0001D344
		private void OnLockAxesChanged()
		{
			if (this.Model != null && !this.Model.gameObject.IsPrefab())
			{
				this.Model.SetLock(this.LockObject);
			}
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x0001EF80 File Offset: 0x0001D380
		protected virtual void RecordTransform()
		{
			if (this.EnableUndo)
			{
				RuntimeUndo.BeginRecord();
				for (int i = 0; i < this.m_activeRealTargets.Length; i++)
				{
					RuntimeUndo.RecordTransform(this.m_activeRealTargets[i], null, -1);
				}
				RuntimeUndo.EndRecord();
			}
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x0001EFCA File Offset: 0x0001D3CA
		private void OnRedoCompleted()
		{
			if (RuntimeTools.PivotMode == RuntimePivotMode.Center && this.m_realTargets != null)
			{
				this.Targets = this.m_realTargets;
			}
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x0001EFED File Offset: 0x0001D3ED
		private void OnUndoCompleted()
		{
			if (RuntimeTools.PivotMode == RuntimePivotMode.Center && this.m_realTargets != null)
			{
				this.Targets = this.m_realTargets;
			}
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x0001F010 File Offset: 0x0001D410
		protected virtual bool HitCenter()
		{
			Vector2 b = this.SceneCamera.WorldToScreenPoint(base.transform.position);
			Vector2 a = Input.mousePosition;
			return (a - b).magnitude <= this.SelectionMargin;
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x0001F060 File Offset: 0x0001D460
		protected virtual bool HitAxis(Vector3 axis, Matrix4x4 matrix, out float distanceToAxis)
		{
			axis = matrix.MultiplyVector(axis);
			Vector2 vector = this.SceneCamera.WorldToScreenPoint(base.transform.position);
			Vector2 a = this.SceneCamera.WorldToScreenPoint(axis + base.transform.position);
			Vector3 vector2 = a - vector;
			float magnitude = vector2.magnitude;
			vector2.Normalize();
			if (vector2 != Vector3.zero)
			{
				return this.HitScreenAxis(out distanceToAxis, vector, vector2, magnitude);
			}
			Vector2 b = Input.mousePosition;
			distanceToAxis = (vector - b).magnitude;
			bool flag = distanceToAxis <= this.SelectionMargin;
			if (!flag)
			{
				distanceToAxis = float.PositiveInfinity;
			}
			else
			{
				distanceToAxis = 0f;
			}
			return flag;
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x0001F138 File Offset: 0x0001D538
		protected virtual bool HitScreenAxis(out float distanceToAxis, Vector2 screenVectorBegin, Vector3 screenVector, float screenVectorMag)
		{
			Vector2 normalized = BaseHandle.PerpendicularClockwise(screenVector).normalized;
			Vector2 a = Input.mousePosition;
			Vector2 vector = a - screenVectorBegin;
			distanceToAxis = Mathf.Abs(Vector2.Dot(normalized, vector));
			Vector2 rhs = vector - normalized * distanceToAxis;
			float num = Vector2.Dot(screenVector, rhs);
			bool flag = num <= screenVectorMag + this.SelectionMargin && num >= -this.SelectionMargin && distanceToAxis <= this.SelectionMargin;
			if (!flag)
			{
				distanceToAxis = float.PositiveInfinity;
			}
			else if (screenVectorMag < this.SelectionMargin)
			{
				distanceToAxis = 0f;
			}
			return flag;
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x0001F1F4 File Offset: 0x0001D5F4
		protected virtual Plane GetDragPlane(Matrix4x4 matrix, Vector3 axis)
		{
			Plane result = new Plane(matrix.MultiplyVector(axis).normalized, matrix.MultiplyPoint(Vector3.zero));
			return result;
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x0001F228 File Offset: 0x0001D628
		protected virtual Plane GetDragPlane()
		{
			Plane result = new Plane(this.SceneCamera.cameraToWorldMatrix.MultiplyVector(Vector3.forward).normalized, base.transform.position);
			return result;
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x0001F268 File Offset: 0x0001D668
		protected virtual bool GetPointOnDragPlane(Vector3 screenPos, out Vector3 point)
		{
			return this.GetPointOnDragPlane(this.m_dragPlane, screenPos, out point);
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x0001F278 File Offset: 0x0001D678
		protected virtual bool GetPointOnDragPlane(Plane dragPlane, Vector3 screenPos, out Vector3 point)
		{
			Ray ray = this.SceneCamera.ScreenPointToRay(screenPos);
			float distance;
			if (dragPlane.Raycast(ray, out distance))
			{
				point = ray.GetPoint(distance);
				return true;
			}
			point = Vector3.zero;
			return false;
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x0001F2BD File Offset: 0x0001D6BD
		private static Vector2 PerpendicularClockwise(Vector2 vector2)
		{
			return new Vector2(-vector2.y, vector2.x);
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x0001F2D4 File Offset: 0x0001D6D4
		void IGL.Draw(int cullingMask)
		{
			RTLayer rtlayer = RTLayer.SceneView;
			if ((cullingMask & (int)rtlayer) == 0)
			{
				return;
			}
			if (this.Model == null)
			{
				this.DrawOverride();
			}
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x0001F303 File Offset: 0x0001D703
		protected virtual void DrawOverride()
		{
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x0001F305 File Offset: 0x0001D705
		[CompilerGenerated]
		private static bool <GetActiveRealTargets>m__0(Transform t)
		{
			return t != null && t.hideFlags == HideFlags.None;
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x0001F31F File Offset: 0x0001D71F
		[CompilerGenerated]
		private static bool <set_Targets_Internal>m__1(Transform t)
		{
			return t != null && t.hideFlags == HideFlags.None;
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x0001F339 File Offset: 0x0001D739
		[CompilerGenerated]
		private static bool <set_Targets_Internal>m__2(Transform t)
		{
			return t.GetComponent<LockAxes>() != null;
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x0001F347 File Offset: 0x0001D747
		[CompilerGenerated]
		private static LockAxes <set_Targets_Internal>m__3(Transform t)
		{
			return t.GetComponent<LockAxes>();
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x0001F34F File Offset: 0x0001D74F
		[CompilerGenerated]
		private static bool <set_Targets_Internal>m__4(Transform target)
		{
			return target.gameObject.isStatic;
		}

		// Token: 0x040004F4 RID: 1268
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private float <EffectiveGridUnitSize>k__BackingField;

		// Token: 0x040004F5 RID: 1269
		public bool EnableUndo = true;

		// Token: 0x040004F6 RID: 1270
		public bool HightlightOnHover = true;

		// Token: 0x040004F7 RID: 1271
		public KeyCode UnitSnapKey = KeyCode.LeftControl;

		// Token: 0x040004F8 RID: 1272
		public Camera SceneCamera;

		// Token: 0x040004F9 RID: 1273
		public BaseHandleModel Model;

		// Token: 0x040004FA RID: 1274
		public float SelectionMargin = 10f;

		// Token: 0x040004FB RID: 1275
		private Transform[] m_activeTargets;

		// Token: 0x040004FC RID: 1276
		private Transform[] m_activeRealTargets;

		// Token: 0x040004FD RID: 1277
		private Transform[] m_realTargets;

		// Token: 0x040004FE RID: 1278
		private Transform[] m_commonCenter;

		// Token: 0x040004FF RID: 1279
		private Transform[] m_commonCenterTarget;

		// Token: 0x04000500 RID: 1280
		[SerializeField]
		private Transform[] m_targets;

		// Token: 0x04000501 RID: 1281
		private RuntimeHandleAxis m_selectedAxis;

		// Token: 0x04000502 RID: 1282
		private bool m_isDragging;

		// Token: 0x04000503 RID: 1283
		private Plane m_dragPlane;

		// Token: 0x04000504 RID: 1284
		[CompilerGenerated]
		private static Func<Transform, bool> <>f__am$cache0;

		// Token: 0x04000505 RID: 1285
		[CompilerGenerated]
		private static Func<Transform, bool> <>f__am$cache1;

		// Token: 0x04000506 RID: 1286
		[CompilerGenerated]
		private static Func<Transform, bool> <>f__am$cache2;

		// Token: 0x04000507 RID: 1287
		[CompilerGenerated]
		private static Func<Transform, LockAxes> <>f__am$cache3;

		// Token: 0x04000508 RID: 1288
		[CompilerGenerated]
		private static Func<Transform, bool> <>f__am$cache4;
	}
}
