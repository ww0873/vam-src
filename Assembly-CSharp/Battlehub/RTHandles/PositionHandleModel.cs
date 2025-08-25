using System;
using Battlehub.RTCommon;
using UnityEngine;

namespace Battlehub.RTHandles
{
	// Token: 0x020000FD RID: 253
	public class PositionHandleModel : BaseHandleModel
	{
		// Token: 0x060005DB RID: 1499 RVA: 0x00021F58 File Offset: 0x00020358
		public PositionHandleModel()
		{
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060005DC RID: 1500 RVA: 0x00021FFD File Offset: 0x000203FD
		// (set) Token: 0x060005DD RID: 1501 RVA: 0x00022005 File Offset: 0x00020405
		public bool IsVertexSnapping
		{
			get
			{
				return this.m_isVertexSnapping;
			}
			set
			{
				if (this.m_isVertexSnapping == value)
				{
					return;
				}
				this.m_isVertexSnapping = value;
				this.OnVertexSnappingModeChaged();
				this.SetColors();
			}
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x00022028 File Offset: 0x00020428
		protected override void Awake()
		{
			base.Awake();
			if (this.m_camera == null)
			{
				this.OnActiveSceneCameraChanged();
			}
			RuntimeEditorApplication.ActiveSceneCameraChanged += this.OnActiveSceneCameraChanged;
			this.m_defaultArmaturesScale = new Vector3[this.m_armatures.Length];
			this.m_defaultB3XScale = new Vector3[this.m_armatures.Length];
			this.m_defaultB3YScale = new Vector3[this.m_armatures.Length];
			this.m_defaultB3ZScale = new Vector3[this.m_armatures.Length];
			this.m_b1x = new Transform[this.m_armatures.Length];
			this.m_b1y = new Transform[this.m_armatures.Length];
			this.m_b1z = new Transform[this.m_armatures.Length];
			this.m_b2x = new Transform[this.m_armatures.Length];
			this.m_b2y = new Transform[this.m_armatures.Length];
			this.m_b2z = new Transform[this.m_armatures.Length];
			this.m_b3x = new Transform[this.m_armatures.Length];
			this.m_b3y = new Transform[this.m_armatures.Length];
			this.m_b3z = new Transform[this.m_armatures.Length];
			this.m_b0 = new Transform[this.m_armatures.Length];
			this.m_bSx = new Transform[this.m_armatures.Length];
			this.m_bSy = new Transform[this.m_armatures.Length];
			this.m_bSz = new Transform[this.m_armatures.Length];
			for (int i = 0; i < this.m_armatures.Length; i++)
			{
				this.m_b1x[i] = this.m_armatures[i].GetChild(0);
				this.m_b1y[i] = this.m_armatures[i].GetChild(1);
				this.m_b1z[i] = this.m_armatures[i].GetChild(2);
				this.m_b2x[i] = this.m_armatures[i].GetChild(3);
				this.m_b2y[i] = this.m_armatures[i].GetChild(4);
				this.m_b2z[i] = this.m_armatures[i].GetChild(5);
				this.m_b3x[i] = this.m_armatures[i].GetChild(6);
				this.m_b3y[i] = this.m_armatures[i].GetChild(7);
				this.m_b3z[i] = this.m_armatures[i].GetChild(8);
				this.m_b0[i] = this.m_armatures[i].GetChild(9);
				this.m_bSx[i] = this.m_armatures[i].GetChild(10);
				this.m_bSy[i] = this.m_armatures[i].GetChild(11);
				this.m_bSz[i] = this.m_armatures[i].GetChild(12);
				this.m_defaultArmaturesScale[i] = this.m_armatures[i].localScale;
				this.m_defaultB3XScale[i] = base.transform.TransformVector(this.m_b3x[i].localScale);
				this.m_defaultB3YScale[i] = base.transform.TransformVector(this.m_b3y[i].localScale);
				this.m_defaultB3ZScale[i] = base.transform.TransformVector(this.m_b3z[i].localScale);
			}
			this.m_b1ss = this.m_ssQuadArmature.GetChild(1);
			this.m_b2ss = this.m_ssQuadArmature.GetChild(2);
			this.m_b3ss = this.m_ssQuadArmature.GetChild(3);
			this.m_b4ss = this.m_ssQuadArmature.GetChild(4);
			this.m_materials = this.m_models[0].GetComponent<Renderer>().materials;
			this.m_ssQuadMaterial = this.m_screenSpaceQuad.GetComponent<Renderer>().sharedMaterial;
			this.SetDefaultColors();
			for (int j = 0; j < this.m_models.Length; j++)
			{
				Renderer component = this.m_models[j].GetComponent<Renderer>();
				component.sharedMaterials = this.m_materials;
			}
			this.OnVertexSnappingModeChaged();
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x0002242F File Offset: 0x0002082F
		protected override void Start()
		{
			base.Start();
			this.UpdateTransforms();
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x0002243D File Offset: 0x0002083D
		protected override void OnDestroy()
		{
			base.OnDestroy();
			RuntimeEditorApplication.ActiveSceneCameraChanged -= this.OnActiveSceneCameraChanged;
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x00022458 File Offset: 0x00020858
		private void OnActiveSceneCameraChanged()
		{
			if (RuntimeEditorApplication.ActiveSceneCamera != null)
			{
				this.m_camera = RuntimeEditorApplication.ActiveSceneCamera.transform;
			}
			else if (Camera.main != null)
			{
				this.m_camera = Camera.main.transform;
			}
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x000224AA File Offset: 0x000208AA
		public override void SetLock(LockObject lockObj)
		{
			base.SetLock(this.m_lockObj);
			this.OnVertexSnappingModeChaged();
			this.SetColors();
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x000224C4 File Offset: 0x000208C4
		public override void Select(RuntimeHandleAxis axis)
		{
			base.Select(axis);
			this.OnVertexSnappingModeChaged();
			this.SetColors();
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x000224DC File Offset: 0x000208DC
		private void OnVertexSnappingModeChaged()
		{
			this.m_normalModeArrows.SetActive(!this.m_isVertexSnapping && !this.m_lockObj.IsPositionLocked);
			this.m_vertexSnappingModeArrows.SetActive(this.m_isVertexSnapping && !this.m_lockObj.IsPositionLocked);
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x00022538 File Offset: 0x00020938
		private void SetDefaultColors()
		{
			if (this.m_lockObj.PositionX)
			{
				this.m_materials[this.m_xMatIndex].color = this.m_disabledColor;
				this.m_materials[this.m_xArrowMatIndex].color = this.m_disabledColor;
			}
			else
			{
				this.m_materials[this.m_xMatIndex].color = this.m_xColor;
				this.m_materials[this.m_xArrowMatIndex].color = this.m_xColor;
			}
			if (this.m_lockObj.PositionY)
			{
				this.m_materials[this.m_yMatIndex].color = this.m_disabledColor;
				this.m_materials[this.m_yArrowMatIndex].color = this.m_disabledColor;
			}
			else
			{
				this.m_materials[this.m_yMatIndex].color = this.m_yColor;
				this.m_materials[this.m_yArrowMatIndex].color = this.m_yColor;
			}
			if (this.m_lockObj.PositionZ)
			{
				this.m_materials[this.m_zMatIndex].color = this.m_disabledColor;
			}
			else
			{
				this.m_materials[this.m_zMatIndex].color = this.m_zColor;
				this.m_materials[this.m_zArrowMatIndex].color = this.m_zColor;
			}
			this.m_materials[this.m_xQMatIndex].color = ((!this.m_lockObj.PositionY && !this.m_lockObj.PositionZ) ? this.m_xColor : this.m_disabledColor);
			this.m_materials[this.m_yQMatIndex].color = ((!this.m_lockObj.PositionX && !this.m_lockObj.PositionZ) ? this.m_yColor : this.m_disabledColor);
			this.m_materials[this.m_zQMatIndex].color = ((!this.m_lockObj.PositionX && !this.m_lockObj.PositionY) ? this.m_zColor : this.m_disabledColor);
			Color xColor = this.m_xColor;
			xColor.a = this.m_quadTransparency;
			this.m_materials[this.m_xQuadMatIndex].color = xColor;
			Color yColor = this.m_yColor;
			yColor.a = this.m_quadTransparency;
			this.m_materials[this.m_yQuadMatIndex].color = yColor;
			Color zColor = this.m_zColor;
			zColor.a = this.m_quadTransparency;
			this.m_materials[this.m_zQuadMatIndex].color = zColor;
			this.m_ssQuadMaterial.color = this.m_altColor;
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x000227DC File Offset: 0x00020BDC
		private void SetColors()
		{
			this.SetDefaultColors();
			RuntimeHandleAxis selectedAxis = this.m_selectedAxis;
			switch (selectedAxis)
			{
			case RuntimeHandleAxis.X:
				if (!this.m_lockObj.PositionX)
				{
					this.m_materials[this.m_xArrowMatIndex].color = this.m_selectionColor;
					this.m_materials[this.m_xMatIndex].color = this.m_selectionColor;
				}
				break;
			case RuntimeHandleAxis.Y:
				if (!this.m_lockObj.PositionY)
				{
					this.m_materials[this.m_yArrowMatIndex].color = this.m_selectionColor;
					this.m_materials[this.m_yMatIndex].color = this.m_selectionColor;
				}
				break;
			case RuntimeHandleAxis.XY:
				if (!this.m_lockObj.PositionX && !this.m_lockObj.PositionY)
				{
					this.m_materials[this.m_xArrowMatIndex].color = this.m_selectionColor;
					this.m_materials[this.m_yArrowMatIndex].color = this.m_selectionColor;
					this.m_materials[this.m_xMatIndex].color = this.m_selectionColor;
					this.m_materials[this.m_yMatIndex].color = this.m_selectionColor;
					this.m_materials[this.m_zQMatIndex].color = this.m_selectionColor;
					this.m_materials[this.m_zQuadMatIndex].color = this.m_selectionColor;
				}
				break;
			case RuntimeHandleAxis.Z:
				if (!this.m_lockObj.PositionZ)
				{
					this.m_materials[this.m_zArrowMatIndex].color = this.m_selectionColor;
					this.m_materials[this.m_zMatIndex].color = this.m_selectionColor;
				}
				break;
			case RuntimeHandleAxis.XZ:
				if (!this.m_lockObj.PositionX && !this.m_lockObj.PositionZ)
				{
					this.m_materials[this.m_xArrowMatIndex].color = this.m_selectionColor;
					this.m_materials[this.m_zArrowMatIndex].color = this.m_selectionColor;
					this.m_materials[this.m_xMatIndex].color = this.m_selectionColor;
					this.m_materials[this.m_zMatIndex].color = this.m_selectionColor;
					this.m_materials[this.m_yQMatIndex].color = this.m_selectionColor;
					this.m_materials[this.m_yQuadMatIndex].color = this.m_selectionColor;
				}
				break;
			case RuntimeHandleAxis.YZ:
				if (!this.m_lockObj.PositionY && !this.m_lockObj.PositionZ)
				{
					this.m_materials[this.m_yArrowMatIndex].color = this.m_selectionColor;
					this.m_materials[this.m_zArrowMatIndex].color = this.m_selectionColor;
					this.m_materials[this.m_yMatIndex].color = this.m_selectionColor;
					this.m_materials[this.m_zMatIndex].color = this.m_selectionColor;
					this.m_materials[this.m_xQMatIndex].color = this.m_selectionColor;
					this.m_materials[this.m_xQuadMatIndex].color = this.m_selectionColor;
				}
				break;
			default:
				if (selectedAxis == RuntimeHandleAxis.Snap)
				{
					this.m_ssQuadMaterial.color = this.m_selectionColor;
				}
				break;
			case RuntimeHandleAxis.Screen:
				break;
			}
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x00022B34 File Offset: 0x00020F34
		private void UpdateTransforms()
		{
			this.m_quadLength = Mathf.Abs(this.m_quadLength);
			this.m_radius = Mathf.Max(0.01f, this.m_radius);
			Vector3 vector = base.transform.rotation * Vector3.right * base.transform.localScale.x;
			Vector3 vector2 = base.transform.rotation * Vector3.up * base.transform.localScale.y;
			Vector3 vector3 = base.transform.rotation * Vector3.forward * base.transform.localScale.z;
			Vector3 position = base.transform.position;
			float num = this.m_radius / 0.05f;
			float d = this.m_arrowLength / 0.2f / num;
			float d2 = this.m_arrowRadius / 0.1f / num;
			for (int i = 0; i < this.m_models.Length; i++)
			{
				this.m_armatures[i].localScale = this.m_defaultArmaturesScale[i] * num;
				this.m_ssQuadArmature.localScale = Vector3.one * num;
				this.m_b3x[i].position = position + vector * this.m_length;
				this.m_b3y[i].position = position + vector2 * this.m_length;
				this.m_b3z[i].position = position + vector3 * this.m_length;
				this.m_b2x[i].position = position + vector * (this.m_length - this.m_arrowLength);
				this.m_b2y[i].position = position + vector2 * (this.m_length - this.m_arrowLength);
				this.m_b2z[i].position = position + vector3 * (this.m_length - this.m_arrowLength);
				this.m_b3x[i].localScale = Vector3.right * d + new Vector3(0f, 1f, 1f) * d2;
				this.m_b3y[i].localScale = Vector3.forward * d + new Vector3(1f, 1f, 0f) * d2;
				this.m_b3z[i].localScale = Vector3.up * d + new Vector3(1f, 0f, 1f) * d2;
				this.m_b1x[i].position = position + Mathf.Sign(Vector3.Dot(vector, this.m_b1x[i].position - position)) * vector * this.m_quadLength;
				this.m_b1y[i].position = position + Mathf.Sign(Vector3.Dot(vector2, this.m_b1y[i].position - position)) * vector2 * this.m_quadLength;
				this.m_b1z[i].position = position + Mathf.Sign(Vector3.Dot(vector3, this.m_b1z[i].position - position)) * vector3 * this.m_quadLength;
				this.m_bSx[i].position = position + (this.m_b1y[i].position - position) + (this.m_b1z[i].position - position);
				this.m_bSy[i].position = position + (this.m_b1x[i].position - position) + (this.m_b1z[i].position - position);
				this.m_bSz[i].position = position + (this.m_b1x[i].position - position) + (this.m_b1y[i].position - position);
			}
			this.m_b1ss.position = position + base.transform.rotation * new Vector3(1f, 1f, 0f) * this.m_quadLength;
			this.m_b2ss.position = position + base.transform.rotation * new Vector3(-1f, -1f, 0f) * this.m_quadLength;
			this.m_b3ss.position = position + base.transform.rotation * new Vector3(-1f, 1f, 0f) * this.m_quadLength;
			this.m_b4ss.position = position + base.transform.rotation * new Vector3(1f, -1f, 0f) * this.m_quadLength;
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x000230BC File Offset: 0x000214BC
		public void SetCameraPosition(Vector3 pos)
		{
			Vector3 vector = (pos - base.transform.position).normalized;
			vector = base.transform.InverseTransformDirection(vector);
			float[] array = new float[8];
			int num = 0;
			Vector3 vector2 = new Vector3(1f, 1f, 1f);
			array[num] = Vector3.Dot(vector2.normalized, vector);
			int num2 = 1;
			Vector3 vector3 = new Vector3(-1f, 1f, 1f);
			array[num2] = Vector3.Dot(vector3.normalized, vector);
			int num3 = 2;
			Vector3 vector4 = new Vector3(-1f, -1f, 1f);
			array[num3] = Vector3.Dot(vector4.normalized, vector);
			int num4 = 3;
			Vector3 vector5 = new Vector3(1f, -1f, 1f);
			array[num4] = Vector3.Dot(vector5.normalized, vector);
			int num5 = 4;
			Vector3 vector6 = new Vector3(1f, 1f, -1f);
			array[num5] = Vector3.Dot(vector6.normalized, vector);
			int num6 = 5;
			Vector3 vector7 = new Vector3(-1f, 1f, -1f);
			array[num6] = Vector3.Dot(vector7.normalized, vector);
			int num7 = 6;
			Vector3 vector8 = new Vector3(-1f, -1f, -1f);
			array[num7] = Vector3.Dot(vector8.normalized, vector);
			int num8 = 7;
			Vector3 vector9 = new Vector3(1f, -1f, -1f);
			array[num8] = Vector3.Dot(vector9.normalized, vector);
			float[] array2 = array;
			float num9 = float.MinValue;
			int num10 = -1;
			for (int i = 0; i < array2.Length; i++)
			{
				if (array2[i] > num9)
				{
					num9 = array2[i];
					num10 = i;
				}
			}
			for (int j = 0; j < this.m_models.Length - 1; j++)
			{
				if (j != num10)
				{
					this.m_models[j].SetActive(false);
				}
			}
			if (num10 >= 0)
			{
				this.m_models[num10].SetActive(true);
			}
		}

		// Token: 0x04000540 RID: 1344
		[SerializeField]
		private Transform m_camera;

		// Token: 0x04000541 RID: 1345
		[SerializeField]
		private GameObject[] m_models;

		// Token: 0x04000542 RID: 1346
		[SerializeField]
		private GameObject m_screenSpaceQuad;

		// Token: 0x04000543 RID: 1347
		[SerializeField]
		private GameObject m_normalModeArrows;

		// Token: 0x04000544 RID: 1348
		[SerializeField]
		private GameObject m_vertexSnappingModeArrows;

		// Token: 0x04000545 RID: 1349
		[SerializeField]
		private Transform[] m_armatures;

		// Token: 0x04000546 RID: 1350
		[SerializeField]
		private Transform m_ssQuadArmature;

		// Token: 0x04000547 RID: 1351
		[SerializeField]
		private int m_xMatIndex;

		// Token: 0x04000548 RID: 1352
		[SerializeField]
		private int m_yMatIndex = 1;

		// Token: 0x04000549 RID: 1353
		[SerializeField]
		private int m_zMatIndex = 2;

		// Token: 0x0400054A RID: 1354
		[SerializeField]
		private int m_xArrowMatIndex = 3;

		// Token: 0x0400054B RID: 1355
		[SerializeField]
		private int m_yArrowMatIndex = 4;

		// Token: 0x0400054C RID: 1356
		[SerializeField]
		private int m_zArrowMatIndex = 5;

		// Token: 0x0400054D RID: 1357
		[SerializeField]
		private int m_xQMatIndex = 6;

		// Token: 0x0400054E RID: 1358
		[SerializeField]
		private int m_yQMatIndex = 7;

		// Token: 0x0400054F RID: 1359
		[SerializeField]
		private int m_zQMatIndex = 8;

		// Token: 0x04000550 RID: 1360
		[SerializeField]
		private int m_xQuadMatIndex = 9;

		// Token: 0x04000551 RID: 1361
		[SerializeField]
		private int m_yQuadMatIndex = 10;

		// Token: 0x04000552 RID: 1362
		[SerializeField]
		private int m_zQuadMatIndex = 11;

		// Token: 0x04000553 RID: 1363
		[SerializeField]
		private float m_quadTransparency = 0.5f;

		// Token: 0x04000554 RID: 1364
		[SerializeField]
		private float m_radius = 0.05f;

		// Token: 0x04000555 RID: 1365
		[SerializeField]
		private float m_length = 1f;

		// Token: 0x04000556 RID: 1366
		[SerializeField]
		private float m_arrowRadius = 0.1f;

		// Token: 0x04000557 RID: 1367
		[SerializeField]
		private float m_arrowLength = 0.2f;

		// Token: 0x04000558 RID: 1368
		[SerializeField]
		private float m_quadLength = 0.2f;

		// Token: 0x04000559 RID: 1369
		[SerializeField]
		private bool m_isVertexSnapping;

		// Token: 0x0400055A RID: 1370
		private Material[] m_materials;

		// Token: 0x0400055B RID: 1371
		private Material m_ssQuadMaterial;

		// Token: 0x0400055C RID: 1372
		private Transform[] m_b0;

		// Token: 0x0400055D RID: 1373
		private Transform[] m_b1x;

		// Token: 0x0400055E RID: 1374
		private Transform[] m_b2x;

		// Token: 0x0400055F RID: 1375
		private Transform[] m_b3x;

		// Token: 0x04000560 RID: 1376
		private Transform[] m_bSx;

		// Token: 0x04000561 RID: 1377
		private Transform[] m_b1y;

		// Token: 0x04000562 RID: 1378
		private Transform[] m_b2y;

		// Token: 0x04000563 RID: 1379
		private Transform[] m_b3y;

		// Token: 0x04000564 RID: 1380
		private Transform[] m_bSy;

		// Token: 0x04000565 RID: 1381
		private Transform[] m_b1z;

		// Token: 0x04000566 RID: 1382
		private Transform[] m_b2z;

		// Token: 0x04000567 RID: 1383
		private Transform[] m_b3z;

		// Token: 0x04000568 RID: 1384
		private Transform[] m_bSz;

		// Token: 0x04000569 RID: 1385
		private Transform m_b1ss;

		// Token: 0x0400056A RID: 1386
		private Transform m_b2ss;

		// Token: 0x0400056B RID: 1387
		private Transform m_b3ss;

		// Token: 0x0400056C RID: 1388
		private Transform m_b4ss;

		// Token: 0x0400056D RID: 1389
		private Vector3[] m_defaultArmaturesScale;

		// Token: 0x0400056E RID: 1390
		private Vector3[] m_defaultB3XScale;

		// Token: 0x0400056F RID: 1391
		private Vector3[] m_defaultB3YScale;

		// Token: 0x04000570 RID: 1392
		private Vector3[] m_defaultB3ZScale;

		// Token: 0x04000571 RID: 1393
		private const float DefaultRadius = 0.05f;

		// Token: 0x04000572 RID: 1394
		private const float DefaultLength = 1f;

		// Token: 0x04000573 RID: 1395
		private const float DefaultArrowRadius = 0.1f;

		// Token: 0x04000574 RID: 1396
		private const float DefaultArrowLength = 0.2f;

		// Token: 0x04000575 RID: 1397
		private const float DefaultQuadLength = 0.2f;
	}
}
