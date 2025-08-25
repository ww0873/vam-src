using System;
using Battlehub.RTCommon;
using UnityEngine;

namespace Battlehub.RTGizmos
{
	// Token: 0x020000DD RID: 221
	public abstract class BaseGizmo : MonoBehaviour, IGL
	{
		// Token: 0x06000454 RID: 1108 RVA: 0x00017FA8 File Offset: 0x000163A8
		protected BaseGizmo()
		{
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000455 RID: 1109 RVA: 0x00018040 File Offset: 0x00016440
		protected int DragIndex
		{
			get
			{
				return this.m_dragIndex;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000456 RID: 1110 RVA: 0x00018048 File Offset: 0x00016448
		protected bool IsDragging
		{
			get
			{
				return this.m_isDragging;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000457 RID: 1111
		protected abstract Matrix4x4 HandlesTransform { get; }

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000458 RID: 1112 RVA: 0x00018050 File Offset: 0x00016450
		protected virtual Vector3[] HandlesPositions
		{
			get
			{
				return this.m_handlesPositions;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000459 RID: 1113 RVA: 0x00018058 File Offset: 0x00016458
		protected virtual Vector3[] HandlesNormals
		{
			get
			{
				return this.m_handlesNormals;
			}
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x00018060 File Offset: 0x00016460
		private void Awake()
		{
			this.AwakeOverride();
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x00018068 File Offset: 0x00016468
		private void Start()
		{
			if (this.SceneCamera == null)
			{
				this.SceneCamera = RuntimeEditorApplication.ActiveSceneCamera;
			}
			if (this.SceneCamera == null)
			{
				this.SceneCamera = Camera.main;
			}
			if (this.Target == null)
			{
				this.Target = base.transform;
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
			if (GLRenderer.Instance != null)
			{
				GLRenderer.Instance.Add(this);
			}
			this.StartOverride();
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x00018177 File Offset: 0x00016577
		private void OnEnable()
		{
			if (GLRenderer.Instance != null)
			{
				GLRenderer.Instance.Add(this);
			}
			this.OnEnableOverride();
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x0001819A File Offset: 0x0001659A
		private void OnDisable()
		{
			if (GLRenderer.Instance != null)
			{
				GLRenderer.Instance.Remove(this);
			}
			this.OnDisableOverride();
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x000181BD File Offset: 0x000165BD
		private void OnDestroy()
		{
			if (GLRenderer.Instance != null)
			{
				GLRenderer.Instance.Remove(this);
			}
			if (RuntimeTools.ActiveTool == this)
			{
				RuntimeTools.ActiveTool = null;
			}
			this.OnDestroyOverride();
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x000181F8 File Offset: 0x000165F8
		private void Update()
		{
			Vector3 vector;
			if (Input.GetMouseButtonDown(0))
			{
				if (RuntimeTools.IsPointerOverGameObject())
				{
					return;
				}
				if (this.SceneCamera == null)
				{
					Debug.LogError("Camera is null");
					return;
				}
				if (RuntimeTools.IsViewing)
				{
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
				Vector2 pointer = Input.mousePosition;
				this.m_dragIndex = this.Hit(pointer, this.HandlesPositions, this.HandlesNormals);
				if (this.m_dragIndex >= 0 && this.OnBeginDrag(this.m_dragIndex))
				{
					this.m_handlesTransform = this.HandlesTransform;
					this.m_handlesInverseTransform = Matrix4x4.TRS(this.Target.position, this.Target.rotation, this.Target.localScale).inverse;
					this.m_dragPlane = this.GetDragPlane();
					this.m_isDragging = this.GetPointOnDragPlane(Input.mousePosition, out this.m_prevPoint);
					this.m_normal = this.HandlesNormals[this.m_dragIndex].normalized;
					if (this.m_isDragging)
					{
						RuntimeTools.ActiveTool = this;
					}
					if (this.EnableUndo)
					{
						bool isRecording = RuntimeUndo.IsRecording;
						if (!isRecording)
						{
							RuntimeUndo.BeginRecord();
						}
						this.RecordOverride();
						if (!isRecording)
						{
							RuntimeUndo.EndRecord();
						}
					}
				}
			}
			else if (Input.GetMouseButtonUp(0))
			{
				if (this.m_isDragging)
				{
					this.OnDrop();
					bool isRecording2 = RuntimeUndo.IsRecording;
					if (!isRecording2)
					{
						RuntimeUndo.BeginRecord();
					}
					this.RecordOverride();
					if (!isRecording2)
					{
						RuntimeUndo.EndRecord();
					}
					this.m_isDragging = false;
					RuntimeTools.ActiveTool = null;
				}
			}
			else if (this.m_isDragging && this.GetPointOnDragPlane(Input.mousePosition, out vector))
			{
				Vector3 vector2 = this.m_handlesInverseTransform.MultiplyVector(vector - this.m_prevPoint);
				vector2 = Vector3.Project(vector2, this.m_normal);
				if (InputController.GetKey(this.UnitSnapKey) || RuntimeTools.UnitSnapping)
				{
					Vector3 zero = Vector3.zero;
					if (Mathf.Abs(vector2.x * 1.5f) >= this.GridSize)
					{
						zero.x = this.GridSize * Mathf.Sign(vector2.x);
					}
					if (Mathf.Abs(vector2.y * 1.5f) >= this.GridSize)
					{
						zero.y = this.GridSize * Mathf.Sign(vector2.y);
					}
					if (Mathf.Abs(vector2.z * 1.5f) >= this.GridSize)
					{
						zero.z = this.GridSize * Mathf.Sign(vector2.z);
					}
					if (zero != Vector3.zero && this.OnDrag(this.m_dragIndex, zero))
					{
						this.m_prevPoint = vector;
					}
				}
				else if (this.OnDrag(this.m_dragIndex, vector2))
				{
					this.m_prevPoint = vector;
				}
			}
			this.UpdateOverride();
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x00018520 File Offset: 0x00016920
		protected virtual void AwakeOverride()
		{
			this.m_handlesPositions = RuntimeGizmos.GetHandlesPositions();
			this.m_handlesNormals = RuntimeGizmos.GetHandlesNormals();
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x00018538 File Offset: 0x00016938
		protected virtual void StartOverride()
		{
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x0001853A File Offset: 0x0001693A
		protected virtual void OnEnableOverride()
		{
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x0001853C File Offset: 0x0001693C
		protected virtual void OnDisableOverride()
		{
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x0001853E File Offset: 0x0001693E
		protected virtual void OnDestroyOverride()
		{
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x00018540 File Offset: 0x00016940
		protected virtual void UpdateOverride()
		{
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x00018542 File Offset: 0x00016942
		protected virtual void RecordOverride()
		{
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x00018544 File Offset: 0x00016944
		protected virtual bool OnBeginDrag(int index)
		{
			return true;
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x00018547 File Offset: 0x00016947
		protected virtual bool OnDrag(int index, Vector3 offset)
		{
			return true;
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x0001854A File Offset: 0x0001694A
		protected virtual void OnDrop()
		{
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x0001854C File Offset: 0x0001694C
		void IGL.Draw(int cullingMask)
		{
			RTLayer rtlayer = RTLayer.SceneView;
			if ((cullingMask & (int)rtlayer) == 0)
			{
				return;
			}
			if (this.Target == null)
			{
				return;
			}
			this.DrawOverride();
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x0001857C File Offset: 0x0001697C
		protected virtual void DrawOverride()
		{
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x0001857E File Offset: 0x0001697E
		protected virtual bool HitOverride(int index, Vector3 vertex, Vector3 normal)
		{
			return true;
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x00018584 File Offset: 0x00016984
		private int Hit(Vector2 pointer, Vector3[] vertices, Vector3[] normals)
		{
			float num = float.MaxValue;
			int result = -1;
			for (int i = 0; i < vertices.Length; i++)
			{
				Vector3 vector = normals[i];
				vector = this.HandlesTransform.MultiplyVector(vector);
				Vector3 vertex = vertices[i];
				Vector3 vector2 = this.HandlesTransform.MultiplyPoint(vertices[i]);
				if (Mathf.Abs(Vector3.Dot((this.SceneCamera.transform.position - vector2).normalized, vector.normalized)) <= 0.999f)
				{
					if (this.HitOverride(i, vertex, vector))
					{
						Vector2 a = this.SceneCamera.WorldToScreenPoint(vector2);
						float magnitude = (a - pointer).magnitude;
						if (magnitude < num && magnitude <= this.SelectionMargin)
						{
							num = magnitude;
							result = i;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x0001868C File Offset: 0x00016A8C
		protected Plane GetDragPlane()
		{
			Vector3 normalized = (this.SceneCamera.transform.position - this.HandlesTransform.MultiplyPoint(this.HandlesPositions[this.m_dragIndex])).normalized;
			Vector3 inPoint = this.m_handlesTransform.MultiplyPoint(Vector3.zero);
			Plane result = new Plane(normalized, inPoint);
			return result;
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x000186F7 File Offset: 0x00016AF7
		protected bool GetPointOnDragPlane(Vector3 screenPos, out Vector3 point)
		{
			return this.GetPointOnDragPlane(this.m_dragPlane, screenPos, out point);
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x00018708 File Offset: 0x00016B08
		protected bool GetPointOnDragPlane(Plane dragPlane, Vector3 screenPos, out Vector3 point)
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

		// Token: 0x04000460 RID: 1120
		public float GridSize = 1f;

		// Token: 0x04000461 RID: 1121
		public Color LineColor = new Color(0f, 1f, 0f, 0.75f);

		// Token: 0x04000462 RID: 1122
		public Color HandlesColor = new Color(0f, 1f, 0f, 0.75f);

		// Token: 0x04000463 RID: 1123
		public Color SelectionColor = new Color(1f, 1f, 0f, 1f);

		// Token: 0x04000464 RID: 1124
		public bool EnableUndo = true;

		// Token: 0x04000465 RID: 1125
		public KeyCode UnitSnapKey = KeyCode.LeftControl;

		// Token: 0x04000466 RID: 1126
		public Camera SceneCamera;

		// Token: 0x04000467 RID: 1127
		public float SelectionMargin = 10f;

		// Token: 0x04000468 RID: 1128
		public Transform Target;

		// Token: 0x04000469 RID: 1129
		private bool m_isDragging;

		// Token: 0x0400046A RID: 1130
		private int m_dragIndex;

		// Token: 0x0400046B RID: 1131
		private Plane m_dragPlane;

		// Token: 0x0400046C RID: 1132
		private Vector3 m_prevPoint;

		// Token: 0x0400046D RID: 1133
		private Vector3 m_normal;

		// Token: 0x0400046E RID: 1134
		private Vector3[] m_handlesNormals;

		// Token: 0x0400046F RID: 1135
		private Vector3[] m_handlesPositions;

		// Token: 0x04000470 RID: 1136
		private Matrix4x4 m_handlesTransform;

		// Token: 0x04000471 RID: 1137
		private Matrix4x4 m_handlesInverseTransform;
	}
}
