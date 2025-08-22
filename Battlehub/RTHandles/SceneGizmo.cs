using System;
using System.Runtime.CompilerServices;
using Battlehub.RTCommon;
using Battlehub.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Battlehub.RTHandles
{
	// Token: 0x0200010A RID: 266
	[RequireComponent(typeof(Camera))]
	public class SceneGizmo : MonoBehaviour
	{
		// Token: 0x0600066D RID: 1645 RVA: 0x00029C1C File Offset: 0x0002801C
		public SceneGizmo()
		{
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x0600066E RID: 1646 RVA: 0x00029C70 File Offset: 0x00028070
		// (set) Token: 0x0600066F RID: 1647 RVA: 0x00029C80 File Offset: 0x00028080
		private bool IsOrthographic
		{
			get
			{
				return this.m_camera.orthographic;
			}
			set
			{
				this.m_camera.orthographic = value;
				this.SceneCamera.orthographic = value;
				if (this.BtnProjection != null)
				{
					Text componentInChildren = this.BtnProjection.GetComponentInChildren<Text>();
					if (componentInChildren != null)
					{
						if (value)
						{
							componentInChildren.text = "Iso";
						}
						else
						{
							componentInChildren.text = "Persp";
						}
					}
				}
				if (this.ProjectionChanged != null)
				{
					this.ProjectionChanged.Invoke();
					this.InitColliders();
				}
			}
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x00029D0C File Offset: 0x0002810C
		private void Start()
		{
			if (this.SceneCamera == null)
			{
				this.SceneCamera = Camera.main;
			}
			if (this.Pivot == null)
			{
				this.Pivot = base.transform;
			}
			this.m_collidersGO = new GameObject();
			this.m_collidersGO.transform.SetParent(base.transform, false);
			this.m_collidersGO.transform.position = this.GetGizmoPosition();
			this.m_collidersGO.transform.rotation = Quaternion.identity;
			this.m_collidersGO.name = "Colliders";
			this.m_colliderProj = this.m_collidersGO.AddComponent<BoxCollider>();
			this.m_colliderUp = this.m_collidersGO.AddComponent<BoxCollider>();
			this.m_colliderDown = this.m_collidersGO.AddComponent<BoxCollider>();
			this.m_colliderLeft = this.m_collidersGO.AddComponent<BoxCollider>();
			this.m_colliderRight = this.m_collidersGO.AddComponent<BoxCollider>();
			this.m_colliderForward = this.m_collidersGO.AddComponent<BoxCollider>();
			this.m_colliderBackward = this.m_collidersGO.AddComponent<BoxCollider>();
			this.m_colliders = new BoxCollider[]
			{
				this.m_colliderProj,
				this.m_colliderUp,
				this.m_colliderDown,
				this.m_colliderRight,
				this.m_colliderLeft,
				this.m_colliderForward,
				this.m_colliderBackward
			};
			this.DisableColliders();
			this.m_camera = base.GetComponent<Camera>();
			this.m_camera.clearFlags = CameraClearFlags.Depth;
			this.m_camera.renderingPath = RenderingPath.Forward;
			this.m_camera.allowMSAA = false;
			this.m_camera.allowHDR = false;
			this.m_camera.cullingMask = 0;
			this.SceneCamera.orthographic = this.m_camera.orthographic;
			this.m_screenHeight = (float)Screen.height;
			this.m_screenWidth = (float)Screen.width;
			this.UpdateLayout();
			this.InitColliders();
			this.UpdateAlpha(ref this.m_xAlpha, Vector3.right, 1f);
			this.UpdateAlpha(ref this.m_yAlpha, Vector3.up, 1f);
			this.UpdateAlpha(ref this.m_zAlpha, Vector3.forward, 1f);
			if (Run.Instance == null)
			{
				new GameObject
				{
					name = "Run"
				}.AddComponent<Run>();
			}
			if (this.BtnProjection != null)
			{
				this.BtnProjection.onClick.AddListener(new UnityAction(this.OnBtnModeClick));
			}
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x00029F94 File Offset: 0x00028394
		private void OnDestroy()
		{
			if (this.BtnProjection != null)
			{
				this.BtnProjection.onClick.RemoveListener(new UnityAction(this.OnBtnModeClick));
			}
			if (RuntimeTools.ActiveTool == this)
			{
				RuntimeTools.ActiveTool = null;
			}
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x00029FE4 File Offset: 0x000283E4
		private void OnBtnModeClick()
		{
			this.IsOrthographic = !this.SceneCamera.orthographic;
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x00029FFA File Offset: 0x000283FA
		public void SetSceneCamera(Camera camera)
		{
			this.SceneCamera = camera;
			this.UpdateLayout();
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x0002A009 File Offset: 0x00028409
		private void OnPostRender()
		{
			RuntimeHandles.DoSceneGizmo(this.GetGizmoPosition(), Quaternion.identity, this.m_selectedAxis, this.Size.y / 96f, this.m_xAlpha, this.m_yAlpha, this.m_zAlpha);
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x0002A044 File Offset: 0x00028444
		private void OnGUI()
		{
			if (this.BtnProjection != null)
			{
				return;
			}
			if (this.SceneCamera.orthographic)
			{
				if (GUI.Button(this.m_buttonRect, "Iso", this.m_buttonStyle))
				{
					this.IsOrthographic = false;
				}
			}
			else if (GUI.Button(this.m_buttonRect, "Persp", this.m_buttonStyle))
			{
				this.IsOrthographic = true;
			}
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x0002A0BC File Offset: 0x000284BC
		private void Update()
		{
			this.Sync();
			float delta = Time.deltaTime / this.m_animationDuration;
			bool flag = this.UpdateAlpha(ref this.m_xAlpha, Vector3.right, delta);
			flag |= this.UpdateAlpha(ref this.m_yAlpha, Vector3.up, delta);
			flag |= this.UpdateAlpha(ref this.m_zAlpha, Vector3.forward, delta);
			if (RuntimeTools.IsPointerOverGameObject())
			{
				if (RuntimeTools.ActiveTool == this)
				{
					RuntimeTools.ActiveTool = null;
				}
				this.m_selectedAxis = Vector3.zero;
				return;
			}
			if (RuntimeTools.IsViewing)
			{
				this.m_selectedAxis = Vector3.zero;
				return;
			}
			if (RuntimeTools.ActiveTool != null && RuntimeTools.ActiveTool != this)
			{
				this.m_selectedAxis = Vector3.zero;
				return;
			}
			Vector2 v = Input.mousePosition;
			v.y = (float)Screen.height - v.y;
			bool flag2 = this.m_buttonRect.Contains(v, true);
			if (flag2)
			{
				RuntimeTools.ActiveTool = this;
			}
			else
			{
				RuntimeTools.ActiveTool = null;
			}
			if (this.m_camera.pixelRect.Contains(Input.mousePosition))
			{
				if (!this.m_mouseOver || flag)
				{
					this.EnableColliders();
				}
				Collider x = this.HitTest();
				if (x == null || (this.m_rotateAnimation != null && this.m_rotateAnimation.InProgress))
				{
					this.m_selectedAxis = Vector3.zero;
				}
				else if (x == this.m_colliderProj)
				{
					this.m_selectedAxis = Vector3.one;
				}
				else if (x == this.m_colliderUp)
				{
					this.m_selectedAxis = Vector3.up;
				}
				else if (x == this.m_colliderDown)
				{
					this.m_selectedAxis = Vector3.down;
				}
				else if (x == this.m_colliderForward)
				{
					this.m_selectedAxis = Vector3.forward;
				}
				else if (x == this.m_colliderBackward)
				{
					this.m_selectedAxis = Vector3.back;
				}
				else if (x == this.m_colliderRight)
				{
					this.m_selectedAxis = Vector3.right;
				}
				else if (x == this.m_colliderLeft)
				{
					this.m_selectedAxis = Vector3.left;
				}
				if (this.m_selectedAxis != Vector3.zero || flag2)
				{
					RuntimeTools.ActiveTool = this;
				}
				else
				{
					RuntimeTools.ActiveTool = null;
				}
				if (Input.GetMouseButtonUp(0) && this.m_selectedAxis != Vector3.zero)
				{
					if (this.m_selectedAxis == Vector3.one)
					{
						this.IsOrthographic = !this.IsOrthographic;
					}
					else
					{
						SceneGizmo.<Update>c__AnonStorey0 <Update>c__AnonStorey = new SceneGizmo.<Update>c__AnonStorey0();
						<Update>c__AnonStorey.$this = this;
						if ((this.m_rotateAnimation == null || !this.m_rotateAnimation.InProgress) && this.OrientationChanging != null)
						{
							this.OrientationChanging.Invoke();
						}
						if (this.m_rotateAnimation != null)
						{
							this.m_rotateAnimation.Abort();
						}
						<Update>c__AnonStorey.pivot = this.Pivot.transform.position;
						<Update>c__AnonStorey.radiusVector = Vector3.back * (this.SceneCamera.transform.position - <Update>c__AnonStorey.pivot).magnitude;
						Quaternion quaternion = Quaternion.LookRotation(-this.m_selectedAxis, Vector3.up);
						Quaternion rotation = this.SceneCamera.transform.rotation;
						Quaternion to = quaternion;
						float duration = 0.4f;
						if (SceneGizmo.<>f__mg$cache0 == null)
						{
							SceneGizmo.<>f__mg$cache0 = new Func<float, float>(AnimationInfo<object, Quaternion>.EaseOutCubic);
						}
						this.m_rotateAnimation = new QuaternionAnimationInfo(rotation, to, duration, SceneGizmo.<>f__mg$cache0, new AnimationCallback<object, Quaternion>(<Update>c__AnonStorey.<>m__0), null);
						Run.Instance.Animation(this.m_rotateAnimation);
					}
				}
				this.m_mouseOver = true;
			}
			else
			{
				if (this.m_mouseOver)
				{
					this.DisableColliders();
					RuntimeTools.ActiveTool = null;
				}
				this.m_mouseOver = false;
			}
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x0002A4E8 File Offset: 0x000288E8
		private void Sync()
		{
			if (this.m_position != base.transform.position || this.m_rotation != base.transform.rotation)
			{
				this.InitColliders();
				this.m_position = base.transform.position;
				this.m_rotation = base.transform.rotation;
			}
			if (this.m_screenHeight != (float)Screen.height || this.m_screenWidth != (float)Screen.width)
			{
				this.m_screenHeight = (float)Screen.height;
				this.m_screenWidth = (float)Screen.width;
				this.UpdateLayout();
			}
			if (this.m_aspect != this.m_camera.aspect)
			{
				this.m_camera.pixelRect = new Rect(this.SceneCamera.pixelRect.min.x + (float)this.SceneCamera.pixelWidth - this.Size.x, this.SceneCamera.pixelRect.min.y + (float)this.SceneCamera.pixelHeight - this.Size.y, this.Size.x, this.Size.y);
				this.m_aspect = this.m_camera.aspect;
			}
			this.m_camera.transform.rotation = this.SceneCamera.transform.rotation;
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x0002A668 File Offset: 0x00028A68
		private void EnableColliders()
		{
			this.m_colliderProj.enabled = true;
			if (this.m_zAlpha == 1f)
			{
				this.m_colliderForward.enabled = true;
				this.m_colliderBackward.enabled = true;
			}
			if (this.m_yAlpha == 1f)
			{
				this.m_colliderUp.enabled = true;
				this.m_colliderDown.enabled = true;
			}
			if (this.m_xAlpha == 1f)
			{
				this.m_colliderRight.enabled = true;
				this.m_colliderLeft.enabled = true;
			}
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x0002A6FC File Offset: 0x00028AFC
		private void DisableColliders()
		{
			for (int i = 0; i < this.m_colliders.Length; i++)
			{
				this.m_colliders[i].enabled = false;
			}
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x0002A730 File Offset: 0x00028B30
		private Collider HitTest()
		{
			Ray ray = this.m_camera.ScreenPointToRay(Input.mousePosition);
			float num = float.MaxValue;
			Collider result = null;
			for (int i = 0; i < this.m_colliders.Length; i++)
			{
				Collider collider = this.m_colliders[i];
				RaycastHit raycastHit;
				if (collider.Raycast(ray, out raycastHit, this.m_gizmoPosition.magnitude * 5f) && raycastHit.distance < num)
				{
					num = raycastHit.distance;
					result = raycastHit.collider;
				}
			}
			return result;
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x0002A7B7 File Offset: 0x00028BB7
		private Vector3 GetGizmoPosition()
		{
			return base.transform.TransformPoint(Vector3.forward * 5f);
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x0002A7D4 File Offset: 0x00028BD4
		private void InitColliders()
		{
			this.m_gizmoPosition = this.GetGizmoPosition();
			float d = RuntimeHandles.GetScreenScale(this.m_gizmoPosition, this.m_camera) * this.Size.y / 96f;
			this.m_collidersGO.transform.rotation = Quaternion.identity;
			this.m_collidersGO.transform.position = this.GetGizmoPosition();
			this.m_colliderProj.size = new Vector3(0.15f, 0.15f, 0.15f) * d;
			this.m_colliderUp.size = new Vector3(0.15f, 0.3f, 0.15f) * d;
			this.m_colliderUp.center = new Vector3(0f, 0.22500001f, 0f) * d;
			this.m_colliderDown.size = new Vector3(0.15f, 0.3f, 0.15f) * d;
			this.m_colliderDown.center = new Vector3(0f, -0.22500001f, 0f) * d;
			this.m_colliderForward.size = new Vector3(0.15f, 0.15f, 0.3f) * d;
			this.m_colliderForward.center = new Vector3(0f, 0f, 0.22500001f) * d;
			this.m_colliderBackward.size = new Vector3(0.15f, 0.15f, 0.3f) * d;
			this.m_colliderBackward.center = new Vector3(0f, 0f, -0.22500001f) * d;
			this.m_colliderRight.size = new Vector3(0.3f, 0.15f, 0.15f) * d;
			this.m_colliderRight.center = new Vector3(0.22500001f, 0f, 0f) * d;
			this.m_colliderLeft.size = new Vector3(0.3f, 0.15f, 0.15f) * d;
			this.m_colliderLeft.center = new Vector3(-0.22500001f, 0f, 0f) * d;
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x0002AA20 File Offset: 0x00028E20
		private bool UpdateAlpha(ref float alpha, Vector3 axis, float delta)
		{
			bool flag = (double)Math.Abs(Vector3.Dot(this.SceneCamera.transform.forward, axis)) > 0.9;
			if (flag)
			{
				if (alpha > 0f)
				{
					alpha -= delta;
					if (alpha < 0f)
					{
						alpha = 0f;
					}
					return true;
				}
			}
			else if (alpha < 1f)
			{
				alpha += delta;
				if (alpha > 1f)
				{
					alpha = 1f;
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x0002AAB0 File Offset: 0x00028EB0
		public void UpdateLayout()
		{
			if (this.m_camera == null)
			{
				return;
			}
			this.m_aspect = this.m_camera.aspect;
			if (this.SceneCamera != null)
			{
				bool flag = false;
				this.m_camera.pixelRect = new Rect(this.SceneCamera.pixelRect.min.x + (float)this.SceneCamera.pixelWidth - this.Size.x, this.SceneCamera.pixelRect.min.y + (float)this.SceneCamera.pixelHeight - this.Size.y, this.Size.x, this.Size.y);
				if (this.m_camera.pixelRect.height == 0f || this.m_camera.pixelRect.width == 0f)
				{
					base.enabled = false;
					return;
				}
				if (!base.enabled)
				{
					flag = true;
				}
				base.enabled = true;
				this.m_camera.depth = this.SceneCamera.depth + 1f;
				this.m_aspect = this.m_camera.aspect;
				this.m_buttonRect = new Rect(this.SceneCamera.pixelRect.min.x + (float)this.SceneCamera.pixelWidth - this.Size.x / 2f - 20f, (float)Screen.height - this.SceneCamera.pixelRect.yMax + this.Size.y - 5f, 40f, 30f);
				this.m_buttonStyle = new GUIStyle();
				this.m_buttonStyle.alignment = TextAnchor.MiddleCenter;
				this.m_buttonStyle.normal.textColor = new Color(0.8f, 0.8f, 0.8f, 0.8f);
				this.m_buttonStyle.fontSize = 12;
				if (flag)
				{
					this.InitColliders();
				}
			}
		}

		// Token: 0x0400062F RID: 1583
		public Camera SceneCamera;

		// Token: 0x04000630 RID: 1584
		public Button BtnProjection;

		// Token: 0x04000631 RID: 1585
		public Transform Pivot;

		// Token: 0x04000632 RID: 1586
		public Vector2 Size = new Vector2(96f, 96f);

		// Token: 0x04000633 RID: 1587
		public UnityEvent OrientationChanging;

		// Token: 0x04000634 RID: 1588
		public UnityEvent OrientationChanged;

		// Token: 0x04000635 RID: 1589
		public UnityEvent ProjectionChanged;

		// Token: 0x04000636 RID: 1590
		private float m_aspect;

		// Token: 0x04000637 RID: 1591
		private Camera m_camera;

		// Token: 0x04000638 RID: 1592
		private float m_xAlpha = 1f;

		// Token: 0x04000639 RID: 1593
		private float m_yAlpha = 1f;

		// Token: 0x0400063A RID: 1594
		private float m_zAlpha = 1f;

		// Token: 0x0400063B RID: 1595
		private float m_animationDuration = 0.2f;

		// Token: 0x0400063C RID: 1596
		private GUIStyle m_buttonStyle;

		// Token: 0x0400063D RID: 1597
		private Rect m_buttonRect;

		// Token: 0x0400063E RID: 1598
		private bool m_mouseOver;

		// Token: 0x0400063F RID: 1599
		private Vector3 m_selectedAxis;

		// Token: 0x04000640 RID: 1600
		private GameObject m_collidersGO;

		// Token: 0x04000641 RID: 1601
		private BoxCollider m_colliderProj;

		// Token: 0x04000642 RID: 1602
		private BoxCollider m_colliderUp;

		// Token: 0x04000643 RID: 1603
		private BoxCollider m_colliderDown;

		// Token: 0x04000644 RID: 1604
		private BoxCollider m_colliderForward;

		// Token: 0x04000645 RID: 1605
		private BoxCollider m_colliderBackward;

		// Token: 0x04000646 RID: 1606
		private BoxCollider m_colliderLeft;

		// Token: 0x04000647 RID: 1607
		private BoxCollider m_colliderRight;

		// Token: 0x04000648 RID: 1608
		private Collider[] m_colliders;

		// Token: 0x04000649 RID: 1609
		private Vector3 m_position;

		// Token: 0x0400064A RID: 1610
		private Quaternion m_rotation;

		// Token: 0x0400064B RID: 1611
		private Vector3 m_gizmoPosition;

		// Token: 0x0400064C RID: 1612
		private IAnimationInfo m_rotateAnimation;

		// Token: 0x0400064D RID: 1613
		private float m_screenHeight;

		// Token: 0x0400064E RID: 1614
		private float m_screenWidth;

		// Token: 0x0400064F RID: 1615
		[CompilerGenerated]
		private static Func<float, float> <>f__mg$cache0;

		// Token: 0x02000EA9 RID: 3753
		[CompilerGenerated]
		private sealed class <Update>c__AnonStorey0
		{
			// Token: 0x06007173 RID: 29043 RVA: 0x0002ACE4 File Offset: 0x000290E4
			public <Update>c__AnonStorey0()
			{
			}

			// Token: 0x06007174 RID: 29044 RVA: 0x0002ACEC File Offset: 0x000290EC
			internal void <>m__0(object target, Quaternion value, float t, bool completed)
			{
				this.$this.SceneCamera.transform.position = this.pivot + value * this.radiusVector;
				this.$this.SceneCamera.transform.rotation = value;
				if (completed)
				{
					this.$this.DisableColliders();
					this.$this.EnableColliders();
					if (this.$this.OrientationChanged != null)
					{
						this.$this.OrientationChanged.Invoke();
					}
				}
			}

			// Token: 0x04006543 RID: 25923
			internal Vector3 pivot;

			// Token: 0x04006544 RID: 25924
			internal Vector3 radiusVector;

			// Token: 0x04006545 RID: 25925
			internal SceneGizmo $this;
		}
	}
}
