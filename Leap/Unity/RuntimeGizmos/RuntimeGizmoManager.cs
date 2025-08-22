using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Leap.Unity.RuntimeGizmos
{
	// Token: 0x02000743 RID: 1859
	[ExecuteInEditMode]
	public class RuntimeGizmoManager : MonoBehaviour
	{
		// Token: 0x06002D58 RID: 11608 RVA: 0x000F183F File Offset: 0x000EFC3F
		public RuntimeGizmoManager()
		{
		}

		// Token: 0x140000B9 RID: 185
		// (add) Token: 0x06002D59 RID: 11609 RVA: 0x000F186C File Offset: 0x000EFC6C
		// (remove) Token: 0x06002D5A RID: 11610 RVA: 0x000F18A0 File Offset: 0x000EFCA0
		public static event Action<RuntimeGizmoDrawer> OnPostRenderGizmos
		{
			add
			{
				Action<RuntimeGizmoDrawer> action = RuntimeGizmoManager.OnPostRenderGizmos;
				Action<RuntimeGizmoDrawer> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<RuntimeGizmoDrawer>>(ref RuntimeGizmoManager.OnPostRenderGizmos, (Action<RuntimeGizmoDrawer>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<RuntimeGizmoDrawer> action = RuntimeGizmoManager.OnPostRenderGizmos;
				Action<RuntimeGizmoDrawer> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<RuntimeGizmoDrawer>>(ref RuntimeGizmoManager.OnPostRenderGizmos, (Action<RuntimeGizmoDrawer>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		// Token: 0x06002D5B RID: 11611 RVA: 0x000F18D4 File Offset: 0x000EFCD4
		public static bool TryGetGizmoDrawer(out RuntimeGizmoDrawer drawer)
		{
			drawer = RuntimeGizmoManager._backDrawer;
			if (drawer != null)
			{
				drawer.ResetMatrixAndColorState();
				return true;
			}
			return false;
		}

		// Token: 0x06002D5C RID: 11612 RVA: 0x000F18EE File Offset: 0x000EFCEE
		public static bool TryGetGizmoDrawer(GameObject attatchedGameObject, out RuntimeGizmoDrawer drawer)
		{
			drawer = RuntimeGizmoManager._backDrawer;
			if (drawer != null && !RuntimeGizmoManager.areGizmosDisabled(attatchedGameObject.transform))
			{
				drawer.ResetMatrixAndColorState();
				return true;
			}
			return false;
		}

		// Token: 0x06002D5D RID: 11613 RVA: 0x000F1918 File Offset: 0x000EFD18
		protected virtual void OnValidate()
		{
			if (this._gizmoShader == null)
			{
				this._gizmoShader = Shader.Find("Hidden/Runtime Gizmos");
			}
			if (new Material(this._gizmoShader)
			{
				hideFlags = HideFlags.HideAndDontSave
			}.passCount != 4)
			{
				UnityEngine.Debug.LogError("Shader " + this._gizmoShader + " does not have 4 passes and cannot be used as a gizmo shader.");
				this._gizmoShader = Shader.Find("Hidden/Runtime Gizmos");
			}
			if (RuntimeGizmoManager._frontDrawer != null && RuntimeGizmoManager._backDrawer != null)
			{
				this.assignDrawerParams();
			}
		}

		// Token: 0x06002D5E RID: 11614 RVA: 0x000F19AA File Offset: 0x000EFDAA
		protected virtual void Reset()
		{
			this._gizmoShader = Shader.Find("Hidden/Runtime Gizmos");
		}

		// Token: 0x06002D5F RID: 11615 RVA: 0x000F19BC File Offset: 0x000EFDBC
		protected virtual void OnEnable()
		{
			if (!this._enabledForBuild)
			{
				base.enabled = false;
				return;
			}
			RuntimeGizmoManager._frontDrawer = new RuntimeGizmoDrawer();
			RuntimeGizmoManager._backDrawer = new RuntimeGizmoDrawer();
			RuntimeGizmoManager._frontDrawer.BeginGuard();
			if (this._gizmoShader == null)
			{
				this._gizmoShader = Shader.Find("Hidden/Runtime Gizmos");
			}
			this.generateMeshes();
			this.assignDrawerParams();
			Camera.onPostRender = (Camera.CameraCallback)Delegate.Remove(Camera.onPostRender, new Camera.CameraCallback(this.onPostRender));
			Camera.onPostRender = (Camera.CameraCallback)Delegate.Combine(Camera.onPostRender, new Camera.CameraCallback(this.onPostRender));
		}

		// Token: 0x06002D60 RID: 11616 RVA: 0x000F1A67 File Offset: 0x000EFE67
		protected virtual void OnDisable()
		{
			RuntimeGizmoManager._frontDrawer = null;
			RuntimeGizmoManager._backDrawer = null;
			Camera.onPostRender = (Camera.CameraCallback)Delegate.Remove(Camera.onPostRender, new Camera.CameraCallback(this.onPostRender));
		}

		// Token: 0x06002D61 RID: 11617 RVA: 0x000F1A98 File Offset: 0x000EFE98
		protected virtual void Update()
		{
			SceneManager.GetActiveScene().GetRootGameObjects(this._objList);
			for (int i = 0; i < this._objList.Count; i++)
			{
				GameObject gameObject = this._objList[i];
				gameObject.GetComponentsInChildren<IRuntimeGizmoComponent>(false, this._gizmoList);
				for (int j = 0; j < this._gizmoList.Count; j++)
				{
					if (!RuntimeGizmoManager.areGizmosDisabled((this._gizmoList[j] as Component).transform))
					{
						RuntimeGizmoManager._backDrawer.ResetMatrixAndColorState();
						try
						{
							this._gizmoList[j].OnDrawRuntimeGizmos(RuntimeGizmoManager._backDrawer);
						}
						catch (Exception exception)
						{
							UnityEngine.Debug.LogException(exception);
						}
					}
				}
			}
			this._readyForSwap = true;
		}

		// Token: 0x06002D62 RID: 11618 RVA: 0x000F1B7C File Offset: 0x000EFF7C
		protected void onPostRender(Camera camera)
		{
			if (this._readyForSwap)
			{
				if (RuntimeGizmoManager.OnPostRenderGizmos != null)
				{
					RuntimeGizmoManager._backDrawer.ResetMatrixAndColorState();
					RuntimeGizmoManager.OnPostRenderGizmos(RuntimeGizmoManager._backDrawer);
				}
				RuntimeGizmoDrawer backDrawer = RuntimeGizmoManager._backDrawer;
				RuntimeGizmoManager._backDrawer = RuntimeGizmoManager._frontDrawer;
				RuntimeGizmoManager._frontDrawer = backDrawer;
				RuntimeGizmoManager._frontDrawer.BeginGuard();
				RuntimeGizmoManager._backDrawer.EndGuard();
				this._readyForSwap = false;
				RuntimeGizmoManager._backDrawer.ClearAllGizmos();
			}
			RuntimeGizmoManager._frontDrawer.DrawAllGizmosToScreen();
		}

		// Token: 0x06002D63 RID: 11619 RVA: 0x000F1BFC File Offset: 0x000EFFFC
		protected static bool areGizmosDisabled(Transform transform)
		{
			bool result = false;
			do
			{
				RuntimeGizmoToggle componentInParent = transform.GetComponentInParent<RuntimeGizmoToggle>();
				if (componentInParent == null)
				{
					break;
				}
				if (!componentInParent.enabled)
				{
					goto Block_2;
				}
				transform = transform.parent;
			}
			while (transform != null);
			return result;
			Block_2:
			result = true;
			return result;
		}

		// Token: 0x06002D64 RID: 11620 RVA: 0x000F1C4C File Offset: 0x000F004C
		private void assignDrawerParams()
		{
			if (this._gizmoShader != null)
			{
				RuntimeGizmoManager._frontDrawer.gizmoShader = this._gizmoShader;
				RuntimeGizmoManager._backDrawer.gizmoShader = this._gizmoShader;
			}
			RuntimeGizmoManager._frontDrawer.sphereMesh = this._sphereMesh;
			RuntimeGizmoManager._frontDrawer.cubeMesh = this._cubeMesh;
			RuntimeGizmoManager._frontDrawer.wireSphereMesh = this._wireSphereMesh;
			RuntimeGizmoManager._frontDrawer.wireCubeMesh = this._wireCubeMesh;
			RuntimeGizmoManager._backDrawer.sphereMesh = this._sphereMesh;
			RuntimeGizmoManager._backDrawer.cubeMesh = this._cubeMesh;
			RuntimeGizmoManager._backDrawer.wireSphereMesh = this._wireSphereMesh;
			RuntimeGizmoManager._backDrawer.wireCubeMesh = this._wireCubeMesh;
		}

		// Token: 0x06002D65 RID: 11621 RVA: 0x000F1D0C File Offset: 0x000F010C
		private void generateMeshes()
		{
			this._cubeMesh = new Mesh();
			this._cubeMesh.name = "RuntimeGizmoCube";
			this._cubeMesh.hideFlags = HideFlags.HideAndDontSave;
			List<Vector3> list = new List<Vector3>();
			List<int> list2 = new List<int>();
			Vector3[] array = new Vector3[]
			{
				Vector3.forward,
				Vector3.right,
				Vector3.up
			};
			for (int i = 0; i < 3; i++)
			{
				this.addQuad(list, list2, array[i % 3], -array[(i + 1) % 3], array[(i + 2) % 3]);
				this.addQuad(list, list2, -array[i % 3], array[(i + 1) % 3], array[(i + 2) % 3]);
			}
			this._cubeMesh.SetVertices(list);
			this._cubeMesh.SetIndices(list2.ToArray(), MeshTopology.Quads, 0);
			this._cubeMesh.RecalculateNormals();
			this._cubeMesh.RecalculateBounds();
			this._cubeMesh.UploadMeshData(true);
			this._wireCubeMesh = new Mesh();
			this._wireCubeMesh.name = "RuntimeWireCubeMesh";
			this._wireCubeMesh.hideFlags = HideFlags.HideAndDontSave;
			list.Clear();
			list2.Clear();
			for (int j = 1; j >= -1; j -= 2)
			{
				for (int k = 1; k >= -1; k -= 2)
				{
					for (int l = 1; l >= -1; l -= 2)
					{
						list.Add(0.5f * new Vector3((float)j, (float)k, (float)l));
					}
				}
			}
			this.addCorner(list2, 0, 1, 2, 4);
			this.addCorner(list2, 3, 1, 2, 7);
			this.addCorner(list2, 5, 1, 4, 7);
			this.addCorner(list2, 6, 2, 4, 7);
			this._wireCubeMesh.SetVertices(list);
			this._wireCubeMesh.SetIndices(list2.ToArray(), MeshTopology.Lines, 0);
			this._wireCubeMesh.RecalculateBounds();
			this._wireCubeMesh.UploadMeshData(true);
			this._wireSphereMesh = new Mesh();
			this._wireSphereMesh.name = "RuntimeWireSphereMesh";
			this._wireSphereMesh.hideFlags = HideFlags.HideAndDontSave;
			list.Clear();
			list2.Clear();
			int num = 96;
			for (int m = 0; m < 32; m++)
			{
				float f = 6.2831855f * (float)m / 32f;
				float num2 = 0.5f * Mathf.Cos(f);
				float num3 = 0.5f * Mathf.Sin(f);
				for (int n = 0; n < 3; n++)
				{
					list2.Add((m * 3 + n) % num);
					list2.Add((m * 3 + n + 3) % num);
				}
				list.Add(new Vector3(num2, num3, 0f));
				list.Add(new Vector3(0f, num2, num3));
				list.Add(new Vector3(num2, 0f, num3));
			}
			this._wireSphereMesh.SetVertices(list);
			this._wireSphereMesh.SetIndices(list2.ToArray(), MeshTopology.Lines, 0);
			this._wireSphereMesh.RecalculateBounds();
			this._wireSphereMesh.UploadMeshData(true);
		}

		// Token: 0x06002D66 RID: 11622 RVA: 0x000F2070 File Offset: 0x000F0470
		private void addQuad(List<Vector3> verts, List<int> indexes, Vector3 normal, Vector3 axis1, Vector3 axis2)
		{
			indexes.Add(verts.Count);
			indexes.Add(verts.Count + 1);
			indexes.Add(verts.Count + 2);
			indexes.Add(verts.Count + 3);
			verts.Add(0.5f * (normal + axis1 + axis2));
			verts.Add(0.5f * (normal + axis1 - axis2));
			verts.Add(0.5f * (normal - axis1 - axis2));
			verts.Add(0.5f * (normal - axis1 + axis2));
		}

		// Token: 0x06002D67 RID: 11623 RVA: 0x000F212F File Offset: 0x000F052F
		private void addCorner(List<int> indexes, int a, int b, int c, int d)
		{
			indexes.Add(a);
			indexes.Add(b);
			indexes.Add(a);
			indexes.Add(c);
			indexes.Add(a);
			indexes.Add(d);
		}

		// Token: 0x06002D68 RID: 11624 RVA: 0x000F215D File Offset: 0x000F055D
		// Note: this type is marked as 'beforefieldinit'.
		static RuntimeGizmoManager()
		{
		}

		// Token: 0x040023D7 RID: 9175
		public const string DEFAULT_SHADER_NAME = "Hidden/Runtime Gizmos";

		// Token: 0x040023D8 RID: 9176
		public const int CIRCLE_RESOLUTION = 32;

		// Token: 0x040023D9 RID: 9177
		[Tooltip("Should the gizmos be visible in the game view.")]
		[SerializeField]
		protected bool _displayInGameView = true;

		// Token: 0x040023DA RID: 9178
		[Tooltip("Should the gizmos be visible in a build.")]
		[SerializeField]
		protected bool _enabledForBuild = true;

		// Token: 0x040023DB RID: 9179
		[Tooltip("The mesh to use for the filled sphere gizmo.")]
		[SerializeField]
		protected Mesh _sphereMesh;

		// Token: 0x040023DC RID: 9180
		[Tooltip("The shader to use for rendering gizmos.")]
		[SerializeField]
		protected Shader _gizmoShader;

		// Token: 0x040023DD RID: 9181
		protected Mesh _cubeMesh;

		// Token: 0x040023DE RID: 9182
		protected Mesh _wireCubeMesh;

		// Token: 0x040023DF RID: 9183
		protected Mesh _wireSphereMesh;

		// Token: 0x040023E0 RID: 9184
		protected static RuntimeGizmoDrawer _backDrawer;

		// Token: 0x040023E1 RID: 9185
		protected static RuntimeGizmoDrawer _frontDrawer;

		// Token: 0x040023E2 RID: 9186
		private bool _readyForSwap;

		// Token: 0x040023E3 RID: 9187
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Action<RuntimeGizmoDrawer> OnPostRenderGizmos;

		// Token: 0x040023E4 RID: 9188
		private List<GameObject> _objList = new List<GameObject>();

		// Token: 0x040023E5 RID: 9189
		private List<IRuntimeGizmoComponent> _gizmoList = new List<IRuntimeGizmoComponent>();
	}
}
