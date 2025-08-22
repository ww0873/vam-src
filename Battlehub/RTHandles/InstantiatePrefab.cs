using System;
using Battlehub.RTCommon;
using Battlehub.RTSaveLoad;
using UnityEngine;

namespace Battlehub.RTHandles
{
	// Token: 0x020000F4 RID: 244
	public class InstantiatePrefab : MonoBehaviour
	{
		// Token: 0x06000558 RID: 1368 RVA: 0x0001DBBC File Offset: 0x0001BFBC
		public InstantiatePrefab()
		{
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x0001DBC4 File Offset: 0x0001BFC4
		private bool GetPointOnDragPlane(out Vector3 point)
		{
			Ray ray = this.m_editor.EditorCamera.ScreenPointToRay(Input.mousePosition);
			float distance;
			if (this.m_dragPlane.Raycast(ray, out distance))
			{
				point = ray.GetPoint(distance);
				return true;
			}
			point = Vector3.zero;
			return false;
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x0001DC18 File Offset: 0x0001C018
		public void Spawn()
		{
			this.m_editor = EditorDemo.Instance;
			if (this.m_editor == null)
			{
				Debug.LogError("Editor.Instance is null");
				return;
			}
			this.m_dragPlane = new Plane(Vector3.up, this.m_editor.Pivot);
			Vector3 position;
			if (this.GetPointOnDragPlane(out position))
			{
				this.m_instance = this.Prefab.InstantiatePrefab(position, Quaternion.identity);
				base.enabled = true;
				this.m_spawn = true;
			}
			else
			{
				this.m_instance = this.Prefab.InstantiatePrefab(this.m_editor.Pivot, Quaternion.identity);
			}
			ExposeToEditor exposeToEditor = this.m_instance.GetComponent<ExposeToEditor>();
			if (!exposeToEditor)
			{
				exposeToEditor = this.m_instance.AddComponent<ExposeToEditor>();
			}
			exposeToEditor.SetName(this.Prefab.name);
			this.m_instance.SetActive(true);
			RuntimeUndo.BeginRecord();
			RuntimeUndo.RecordSelection();
			RuntimeUndo.BeginRegisterCreateObject(this.m_instance);
			RuntimeUndo.EndRecord();
			bool enabled = RuntimeUndo.Enabled;
			RuntimeUndo.Enabled = false;
			RuntimeSelection.activeGameObject = this.m_instance;
			RuntimeUndo.Enabled = enabled;
			RuntimeUndo.BeginRecord();
			RuntimeUndo.RegisterCreatedObject(this.m_instance);
			RuntimeUndo.RecordSelection();
			RuntimeUndo.EndRecord();
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x0001DD50 File Offset: 0x0001C150
		private void Update()
		{
			if (!this.m_spawn)
			{
				return;
			}
			Vector3 position;
			if (this.GetPointOnDragPlane(out position))
			{
				if (this.m_editor.AutoUnitSnapping)
				{
					position.x = Mathf.Round(position.x);
					position.y = Mathf.Round(position.y);
					position.z = Mathf.Round(position.z);
				}
				this.m_instance.transform.position = position;
			}
			if (Input.GetMouseButtonDown(0))
			{
				base.enabled = false;
				this.m_spawn = false;
				this.m_instance = null;
			}
		}

		// Token: 0x040004EA RID: 1258
		public GameObject Prefab;

		// Token: 0x040004EB RID: 1259
		private EditorDemo m_editor;

		// Token: 0x040004EC RID: 1260
		private GameObject m_instance;

		// Token: 0x040004ED RID: 1261
		private Plane m_dragPlane;

		// Token: 0x040004EE RID: 1262
		private bool m_spawn;
	}
}
