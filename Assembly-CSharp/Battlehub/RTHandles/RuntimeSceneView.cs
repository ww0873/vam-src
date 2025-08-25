using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Battlehub.RTCommon;
using Battlehub.Utils;
using UnityEngine;

namespace Battlehub.RTHandles
{
	// Token: 0x02000103 RID: 259
	public class RuntimeSceneView : RuntimeSelectionComponent
	{
		// Token: 0x06000619 RID: 1561 RVA: 0x00027CBC File Offset: 0x000260BC
		public RuntimeSceneView()
		{
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x0600061A RID: 1562 RVA: 0x00027D21 File Offset: 0x00026121
		protected override bool IPointerOverEditorArea
		{
			get
			{
				return RuntimeEditorApplication.IsPointerOverWindow(this) || !RuntimeEditorApplication.IsOpened;
			}
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x00027D3C File Offset: 0x0002613C
		protected override void AwakeOverride()
		{
			base.AwakeOverride();
			if (Run.Instance == null)
			{
				new GameObject
				{
					name = "Run"
				}.AddComponent<Run>();
			}
			if (this.Pivot == null)
			{
				GameObject gameObject = new GameObject();
				gameObject.transform.SetParent(base.transform, false);
				gameObject.name = "Pivot";
				this.Pivot = gameObject.transform;
			}
			if (this.SecondaryPivot == null)
			{
				GameObject gameObject2 = new GameObject();
				gameObject2.transform.SetParent(base.transform, false);
				gameObject2.name = "SecondaryPivot";
				this.SecondaryPivot = gameObject2.transform;
			}
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x00027DF7 File Offset: 0x000261F7
		protected override void OnEnableOverride()
		{
			if (this.SceneCamera == null)
			{
				return;
			}
			base.OnEnableOverride();
			if (this.SceneCamera != null)
			{
				this.SetSceneCamera(this.SceneCamera);
			}
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x00027E2E File Offset: 0x0002622E
		protected override void UpdateOverride()
		{
			base.UpdateOverride();
			if (RuntimeTools.ActiveTool != null)
			{
				return;
			}
			this.HandleInput();
			if (RuntimeEditorApplication.IsPointerOverWindow(this))
			{
				this.SetCursor();
			}
			else
			{
				CursorHelper.ResetCursor(this);
			}
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x00027E6C File Offset: 0x0002626C
		protected override void SetCursor()
		{
			if (!this.IPointerOverEditorArea)
			{
				CursorHelper.ResetCursor(this);
				return;
			}
			if (this.m_pan)
			{
				if (this.m_rotate && RuntimeTools.Current == RuntimeTool.View)
				{
					CursorHelper.SetCursor(this, this.ViewTexture, (!(this.ViewTexture != null)) ? Vector2.zero : new Vector2((float)(this.ViewTexture.width / 2), (float)(this.ViewTexture.height / 2)), CursorMode.Auto);
				}
				else
				{
					CursorHelper.SetCursor(this, this.MoveTexture, (!(this.MoveTexture != null)) ? Vector2.zero : new Vector2((float)(this.MoveTexture.width / 2), (float)(this.MoveTexture.height / 2)), CursorMode.Auto);
				}
			}
			else if (this.m_rotate)
			{
				CursorHelper.SetCursor(this, this.ViewTexture, (!(this.ViewTexture != null)) ? Vector2.zero : new Vector2((float)(this.ViewTexture.width / 2), (float)(this.ViewTexture.height / 2)), CursorMode.Auto);
			}
			else if (RuntimeTools.Current == RuntimeTool.View)
			{
				CursorHelper.SetCursor(this, this.MoveTexture, (!(this.MoveTexture != null)) ? Vector2.zero : new Vector2((float)(this.MoveTexture.width / 2), (float)(this.MoveTexture.height / 2)), CursorMode.Auto);
			}
			else if (!InputController.GetKey(this.RotateKey) && !InputController.GetKey(this.RotateKey2) && !InputController.GetKey(this.RotateKey3))
			{
				CursorHelper.ResetCursor(this);
			}
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x00028035 File Offset: 0x00026435
		public void LockInput()
		{
			this.m_lockInput = true;
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x00028040 File Offset: 0x00026440
		public void UnlockInput()
		{
			this.m_lockInput = false;
			if (this.m_mouseOrbit != null)
			{
				this.Pivot.position = this.SceneCamera.transform.position + this.SceneCamera.transform.forward * this.m_mouseOrbit.Distance;
				this.SecondaryPivot.position = this.Pivot.position;
				this.m_mouseOrbit.Target = this.Pivot;
				this.m_mouseOrbit.SyncAngles();
			}
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x000280D8 File Offset: 0x000264D8
		public void OnProjectionChanged()
		{
			float num = this.SceneCamera.fieldOfView * 0.017453292f;
			float magnitude = (this.SceneCamera.transform.position - this.Pivot.position).magnitude;
			float orthographicSize = magnitude * Mathf.Sin(num / 2f);
			this.SceneCamera.orthographicSize = orthographicSize;
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x0002813C File Offset: 0x0002653C
		public void SnapToGrid()
		{
			GameObject[] gameObjects = RuntimeSelection.gameObjects;
			if (gameObjects == null || gameObjects.Length == 0)
			{
				return;
			}
			Transform transform = gameObjects[0].transform;
			Vector3 position = transform.position;
			if ((double)this.GridSize < 0.01)
			{
				this.GridSize = 0.01f;
			}
			position.x = Mathf.Round(position.x / this.GridSize) * this.GridSize;
			position.y = Mathf.Round(position.y / this.GridSize) * this.GridSize;
			position.z = Mathf.Round(position.z / this.GridSize) * this.GridSize;
			Vector3 b = position - transform.position;
			for (int i = 0; i < gameObjects.Length; i++)
			{
				gameObjects[i].transform.position += b;
			}
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x00028230 File Offset: 0x00026630
		public void Focus()
		{
			if (RuntimeSelection.activeTransform == null)
			{
				return;
			}
			this.m_autoFocusTransform = RuntimeSelection.activeTransform;
			if (RuntimeSelection.activeTransform.gameObject.hideFlags != HideFlags.None)
			{
				return;
			}
			Bounds bounds = this.CalculateBounds(RuntimeSelection.activeTransform);
			float num = this.SceneCamera.fieldOfView * 0.017453292f;
			float num2 = Mathf.Max(new float[]
			{
				bounds.extents.y,
				bounds.extents.x,
				bounds.extents.z
			}) * 2f;
			float num3 = Mathf.Abs(num2 / Mathf.Sin(num / 2f));
			this.Pivot.position = bounds.center;
			this.SecondaryPivot.position = RuntimeSelection.activeTransform.position;
			Vector3 position = this.SceneCamera.transform.position;
			Vector3 to = this.Pivot.position - num3 * this.SceneCamera.transform.forward;
			float duration = 0.1f;
			if (RuntimeSceneView.<>f__mg$cache0 == null)
			{
				RuntimeSceneView.<>f__mg$cache0 = new Func<float, float>(AnimationInfo<object, Vector3>.EaseOutCubic);
			}
			this.m_focusAnimation = new Vector3AnimationInfo(position, to, duration, RuntimeSceneView.<>f__mg$cache0, new AnimationCallback<object, Vector3>(this.<Focus>m__0), null);
			Run.Instance.Animation(this.m_focusAnimation);
			Run instance = Run.Instance;
			float distance = this.m_mouseOrbit.Distance;
			float to2 = num3;
			float duration2 = 0.1f;
			if (RuntimeSceneView.<>f__mg$cache1 == null)
			{
				RuntimeSceneView.<>f__mg$cache1 = new Func<float, float>(AnimationInfo<object, Vector3>.EaseOutCubic);
			}
			instance.Animation(new FloatAnimationInfo(distance, to2, duration2, RuntimeSceneView.<>f__mg$cache1, new AnimationCallback<object, float>(this.<Focus>m__1), null));
			Run instance2 = Run.Instance;
			float orthographicSize = this.SceneCamera.orthographicSize;
			float to3 = num2;
			float duration3 = 0.1f;
			if (RuntimeSceneView.<>f__mg$cache2 == null)
			{
				RuntimeSceneView.<>f__mg$cache2 = new Func<float, float>(AnimationInfo<object, Vector3>.EaseOutCubic);
			}
			instance2.Animation(new FloatAnimationInfo(orthographicSize, to3, duration3, RuntimeSceneView.<>f__mg$cache2, new AnimationCallback<object, float>(this.<Focus>m__2), null));
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x00028428 File Offset: 0x00026828
		private Bounds CalculateBounds(Transform t)
		{
			Renderer componentInChildren = t.GetComponentInChildren<Renderer>();
			if (componentInChildren)
			{
				Bounds bounds = componentInChildren.bounds;
				if (bounds.size == Vector3.zero && bounds.center != componentInChildren.transform.position)
				{
					bounds = RuntimeSceneView.TransformBounds(componentInChildren.transform.localToWorldMatrix, bounds);
				}
				this.CalculateBounds(t, ref bounds);
				if (bounds.extents == Vector3.zero)
				{
					bounds.extents = new Vector3(0.5f, 0.5f, 0.5f);
				}
				return bounds;
			}
			return new Bounds(t.position, new Vector3(0.5f, 0.5f, 0.5f));
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x000284F0 File Offset: 0x000268F0
		private void CalculateBounds(Transform t, ref Bounds totalBounds)
		{
			IEnumerator enumerator = t.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					Renderer component = transform.GetComponent<Renderer>();
					if (component)
					{
						Bounds bounds = component.bounds;
						if (bounds.size == Vector3.zero && bounds.center != component.transform.position)
						{
							bounds = RuntimeSceneView.TransformBounds(component.transform.localToWorldMatrix, bounds);
						}
						totalBounds.Encapsulate(bounds.min);
						totalBounds.Encapsulate(bounds.max);
					}
					this.CalculateBounds(transform, ref totalBounds);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x000285CC File Offset: 0x000269CC
		public static Bounds TransformBounds(Matrix4x4 matrix, Bounds bounds)
		{
			Vector3 center = matrix.MultiplyPoint(bounds.center);
			Vector3 extents = bounds.extents;
			Vector3 vector = matrix.MultiplyVector(new Vector3(extents.x, 0f, 0f));
			Vector3 vector2 = matrix.MultiplyVector(new Vector3(0f, extents.y, 0f));
			Vector3 vector3 = matrix.MultiplyVector(new Vector3(0f, 0f, extents.z));
			extents.x = Mathf.Abs(vector.x) + Mathf.Abs(vector2.x) + Mathf.Abs(vector3.x);
			extents.y = Mathf.Abs(vector.y) + Mathf.Abs(vector2.y) + Mathf.Abs(vector3.y);
			extents.z = Mathf.Abs(vector.z) + Mathf.Abs(vector2.z) + Mathf.Abs(vector3.z);
			return new Bounds
			{
				center = center,
				extents = extents
			};
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x000286EC File Offset: 0x00026AEC
		private void Pan()
		{
			Vector3 a;
			Vector3 b;
			if (this.GetPointOnDragPlane(Input.mousePosition, out a) && this.GetPointOnDragPlane(this.m_lastMousePosition, out b))
			{
				Vector3 b2 = a - b;
				this.m_lastMousePosition = Input.mousePosition;
				this.SceneCamera.transform.position -= b2;
				this.Pivot.position -= b2;
				this.SecondaryPivot.position -= b2;
			}
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x0002877C File Offset: 0x00026B7C
		private bool GetPointOnDragPlane(Vector3 mouse, out Vector3 point)
		{
			Ray ray = this.SceneCamera.ScreenPointToRay(mouse);
			float distance;
			if (this.m_dragPlane.Raycast(ray, out distance))
			{
				point = ray.GetPoint(distance);
				return true;
			}
			point = Vector3.zero;
			return false;
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x000287C8 File Offset: 0x00026BC8
		protected override bool CanSelect(GameObject go)
		{
			ExposeToEditor component = go.GetComponent<ExposeToEditor>();
			return component != null && component.CanSelect;
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x000287F4 File Offset: 0x00026BF4
		private void HandleInput()
		{
			if (RuntimeTools.AutoFocus)
			{
				if (!(RuntimeTools.ActiveTool != null))
				{
					if (!(this.m_autoFocusTransform == null))
					{
						if (!(this.m_autoFocusTransform.position == this.SecondaryPivot.position))
						{
							if (this.m_focusAnimation == null || !this.m_focusAnimation.InProgress)
							{
								Vector3 b = this.m_autoFocusTransform.position - this.SecondaryPivot.position;
								this.SceneCamera.transform.position += b;
								this.Pivot.transform.position += b;
								this.SecondaryPivot.transform.position += b;
							}
						}
					}
				}
			}
			if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2))
			{
				this.m_handleInput = false;
				this.m_mouseOrbit.enabled = false;
				this.m_rotate = false;
				this.SetCursor();
				return;
			}
			bool flag = RuntimeEditorApplication.IsActiveWindow(RuntimeWindowType.GameView);
			if (!flag)
			{
				float axis = Input.GetAxis("Mouse ScrollWheel");
				if (axis != 0f && !RuntimeTools.IsPointerOverGameObject())
				{
					this.m_mouseOrbit.Zoom();
				}
			}
			if (this.m_lockInput)
			{
				return;
			}
			if (!flag)
			{
				if (InputController.GetKeyDown(this.SnapToGridKey) && InputController.GetKey(base.ModifierKey))
				{
					this.SnapToGrid();
				}
				if (InputController.GetKeyDown(this.FocusKey))
				{
					this.Focus();
				}
				bool flag2 = InputController.GetKey(this.RotateKey) || InputController.GetKey(this.RotateKey2) || InputController.GetKey(this.RotateKey3);
				bool flag3 = Input.GetMouseButton(2) || Input.GetMouseButton(1) || (Input.GetMouseButton(0) && RuntimeTools.Current == RuntimeTool.View);
				if (flag3 != this.m_pan)
				{
					this.m_pan = flag3;
					if (this.m_pan)
					{
						if (RuntimeTools.Current != RuntimeTool.View)
						{
							this.m_rotate = false;
						}
						this.m_dragPlane = new Plane(-this.SceneCamera.transform.forward, this.Pivot.position);
					}
					this.SetCursor();
				}
				else if (flag2 != this.m_rotate)
				{
					this.m_rotate = flag2;
					this.SetCursor();
				}
			}
			RuntimeTools.IsViewing = (this.m_rotate || this.m_pan);
			bool flag4 = RuntimeTools.IsViewing || flag;
			if (!this.IPointerOverEditorArea)
			{
				return;
			}
			bool mouseButtonDown = Input.GetMouseButtonDown(0);
			bool mouseButtonDown2 = Input.GetMouseButtonDown(1);
			bool mouseButtonDown3 = Input.GetMouseButtonDown(2);
			if (mouseButtonDown || mouseButtonDown2 || mouseButtonDown3)
			{
				this.m_handleInput = !base.PositionHandle.IsDragging;
				this.m_lastMousePosition = Input.mousePosition;
				if (this.m_rotate)
				{
					this.m_mouseOrbit.enabled = true;
				}
			}
			if (this.m_handleInput && flag4 && this.m_pan && (!this.m_rotate || RuntimeTools.Current != RuntimeTool.View))
			{
				this.Pan();
			}
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x00028B68 File Offset: 0x00026F68
		public override void SetSceneCamera(Camera camera)
		{
			base.SetSceneCamera(camera);
			this.SceneCamera.fieldOfView = 60f;
			this.OnProjectionChanged();
			this.m_mouseOrbit = this.SceneCamera.gameObject.GetComponent<MouseOrbit>();
			if (this.m_mouseOrbit == null)
			{
				this.m_mouseOrbit = this.SceneCamera.gameObject.AddComponent<MouseOrbit>();
			}
			this.UnlockInput();
			this.m_mouseOrbit.enabled = false;
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x00028BE1 File Offset: 0x00026FE1
		[CompilerGenerated]
		private void <Focus>m__0(object target, Vector3 value, float t, bool completed)
		{
			if (this.SceneCamera)
			{
				this.SceneCamera.transform.position = value;
			}
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x00028C04 File Offset: 0x00027004
		[CompilerGenerated]
		private void <Focus>m__1(object target, float value, float t, bool completed)
		{
			if (this.m_mouseOrbit)
			{
				this.m_mouseOrbit.Distance = value;
			}
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x00028C22 File Offset: 0x00027022
		[CompilerGenerated]
		private void <Focus>m__2(object target, float value, float t, bool completed)
		{
			if (this.SceneCamera)
			{
				this.SceneCamera.orthographicSize = value;
			}
		}

		// Token: 0x040005DA RID: 1498
		public KeyCode FocusKey = KeyCode.F;

		// Token: 0x040005DB RID: 1499
		public KeyCode SnapToGridKey = KeyCode.S;

		// Token: 0x040005DC RID: 1500
		public KeyCode RotateKey = KeyCode.LeftAlt;

		// Token: 0x040005DD RID: 1501
		public KeyCode RotateKey2 = KeyCode.RightAlt;

		// Token: 0x040005DE RID: 1502
		public KeyCode RotateKey3 = KeyCode.AltGr;

		// Token: 0x040005DF RID: 1503
		public Texture2D ViewTexture;

		// Token: 0x040005E0 RID: 1504
		public Texture2D MoveTexture;

		// Token: 0x040005E1 RID: 1505
		public Transform Pivot;

		// Token: 0x040005E2 RID: 1506
		public Transform SecondaryPivot;

		// Token: 0x040005E3 RID: 1507
		private bool m_pan;

		// Token: 0x040005E4 RID: 1508
		private Plane m_dragPlane;

		// Token: 0x040005E5 RID: 1509
		private bool m_rotate;

		// Token: 0x040005E6 RID: 1510
		private bool m_handleInput;

		// Token: 0x040005E7 RID: 1511
		private bool m_lockInput;

		// Token: 0x040005E8 RID: 1512
		private Vector3 m_lastMousePosition;

		// Token: 0x040005E9 RID: 1513
		private MouseOrbit m_mouseOrbit;

		// Token: 0x040005EA RID: 1514
		public float ZoomSensitivity = 8f;

		// Token: 0x040005EB RID: 1515
		public float PanSensitivity = 100f;

		// Token: 0x040005EC RID: 1516
		private IAnimationInfo m_focusAnimation;

		// Token: 0x040005ED RID: 1517
		private Transform m_autoFocusTransform;

		// Token: 0x040005EE RID: 1518
		public float GridSize = 1f;

		// Token: 0x040005EF RID: 1519
		[CompilerGenerated]
		private static Func<float, float> <>f__mg$cache0;

		// Token: 0x040005F0 RID: 1520
		[CompilerGenerated]
		private static Func<float, float> <>f__mg$cache1;

		// Token: 0x040005F1 RID: 1521
		[CompilerGenerated]
		private static Func<float, float> <>f__mg$cache2;
	}
}
