using System;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

// Token: 0x02000C87 RID: 3207
[ExecuteInEditMode]
public class CubicBezierCurve : JSONStorable
{
	// Token: 0x060060AA RID: 24746 RVA: 0x001D0A0B File Offset: 0x001CEE0B
	public CubicBezierCurve()
	{
	}

	// Token: 0x060060AB RID: 24747 RVA: 0x001D0A36 File Offset: 0x001CEE36
	public override string[] GetCustomParamNames()
	{
		return this.customParamNames;
	}

	// Token: 0x060060AC RID: 24748 RVA: 0x001D0A3E File Offset: 0x001CEE3E
	protected void RegisterAllocatedObject(UnityEngine.Object o)
	{
		if (Application.isPlaying)
		{
			if (this.allocatedObjects == null)
			{
				this.allocatedObjects = new List<UnityEngine.Object>();
			}
			this.allocatedObjects.Add(o);
		}
	}

	// Token: 0x060060AD RID: 24749 RVA: 0x001D0A6C File Offset: 0x001CEE6C
	protected void DestroyAllocatedObjects()
	{
		if (Application.isPlaying && this.allocatedObjects != null)
		{
			foreach (UnityEngine.Object obj in this.allocatedObjects)
			{
				UnityEngine.Object.Destroy(obj);
			}
		}
	}

	// Token: 0x060060AE RID: 24750 RVA: 0x001D0ADC File Offset: 0x001CEEDC
	public override JSONClass GetJSON(bool includePhysical = true, bool includeAppearance = true, bool forceStore = false)
	{
		JSONClass json = base.GetJSON(includePhysical, includeAppearance, forceStore);
		if ((includePhysical || forceStore) && (this.curveType != CubicBezierCurve.CurveType.Auto || forceStore))
		{
			this.needsStore = true;
			json["curveType"] = this.curveType.ToString();
		}
		return json;
	}

	// Token: 0x060060AF RID: 24751 RVA: 0x001D0B3C File Offset: 0x001CEF3C
	public override void RestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, JSONArray presetAtoms = null, bool setMissingToDefault = true)
	{
		base.RestoreFromJSON(jc, restorePhysical, restoreAppearance, presetAtoms, setMissingToDefault);
		if (!base.physicalLocked && restorePhysical && !base.IsCustomPhysicalParamLocked("curveType"))
		{
			if (jc["curveType"] != null)
			{
				this.SetCurveType(jc["curveType"]);
			}
			else if (setMissingToDefault)
			{
				this.SetCurveType("Auto");
			}
		}
	}

	// Token: 0x17000E41 RID: 3649
	// (get) Token: 0x060060B0 RID: 24752 RVA: 0x001D0BBA File Offset: 0x001CEFBA
	// (set) Token: 0x060060B1 RID: 24753 RVA: 0x001D0BC2 File Offset: 0x001CEFC2
	public bool draw
	{
		get
		{
			return this._draw;
		}
		set
		{
			if (this._draw != value)
			{
				this._draw = value;
			}
		}
	}

	// Token: 0x060060B2 RID: 24754 RVA: 0x001D0BD8 File Offset: 0x001CEFD8
	public void SetDrawColor(Color c)
	{
		if (this._drawColor.r != c.r || this._drawColor.g != c.g || this._drawColor.b != c.b)
		{
			this._drawColor.r = c.r;
			this._drawColor.g = c.g;
			this._drawColor.b = c.b;
			this.materialLocal.color = this._drawColor;
		}
	}

	// Token: 0x17000E42 RID: 3650
	// (get) Token: 0x060060B3 RID: 24755 RVA: 0x001D0C71 File Offset: 0x001CF071
	// (set) Token: 0x060060B4 RID: 24756 RVA: 0x001D0C79 File Offset: 0x001CF079
	public CubicBezierPoint[] points
	{
		get
		{
			return this._points;
		}
		set
		{
			this._points = value;
			this.ResyncControlPoints();
			this.RegenerateMesh();
		}
	}

	// Token: 0x17000E43 RID: 3651
	// (get) Token: 0x060060B5 RID: 24757 RVA: 0x001D0C8E File Offset: 0x001CF08E
	// (set) Token: 0x060060B6 RID: 24758 RVA: 0x001D0C96 File Offset: 0x001CF096
	public bool loop
	{
		get
		{
			return this._loop;
		}
		set
		{
			this._loop = value;
		}
	}

	// Token: 0x060060B7 RID: 24759 RVA: 0x001D0C9F File Offset: 0x001CF09F
	protected virtual void SyncLoop(bool val)
	{
		this._loop = val;
		this.RegenerateMesh();
	}

	// Token: 0x060060B8 RID: 24760 RVA: 0x001D0CAE File Offset: 0x001CF0AE
	protected void SyncCurveType()
	{
		this.ResyncControlPoints();
	}

	// Token: 0x060060B9 RID: 24761 RVA: 0x001D0CB8 File Offset: 0x001CF0B8
	protected void SetCurveType(string type)
	{
		try
		{
			CubicBezierCurve.CurveType curveType = (CubicBezierCurve.CurveType)Enum.Parse(typeof(CubicBezierCurve.CurveType), type);
			this._curveType = curveType;
			this.SyncCurveType();
		}
		catch (ArgumentException)
		{
			Debug.LogError("Attempted to set curve type to " + type + " which is not a valid curve type");
		}
	}

	// Token: 0x17000E44 RID: 3652
	// (get) Token: 0x060060BA RID: 24762 RVA: 0x001D0D18 File Offset: 0x001CF118
	// (set) Token: 0x060060BB RID: 24763 RVA: 0x001D0D20 File Offset: 0x001CF120
	public CubicBezierCurve.CurveType curveType
	{
		get
		{
			return this._curveType;
		}
		set
		{
			if (this.curveTypeJSON != null)
			{
				this.curveTypeJSON.val = value.ToString();
			}
			else if (this._curveType != value)
			{
				this._curveType = value;
				this.SyncCurveType();
			}
		}
	}

	// Token: 0x17000E45 RID: 3653
	// (get) Token: 0x060060BC RID: 24764 RVA: 0x001D0D6E File Offset: 0x001CF16E
	// (set) Token: 0x060060BD RID: 24765 RVA: 0x001D0D76 File Offset: 0x001CF176
	public int curveSmooth
	{
		get
		{
			return this._curveSmooth;
		}
		set
		{
			if (this._curveSmooth != value)
			{
				this._curveSmooth = value;
				this.RegenerateMesh();
			}
		}
	}

	// Token: 0x060060BE RID: 24766 RVA: 0x001D0D94 File Offset: 0x001CF194
	protected void AutoComputeControlPoints()
	{
		if (this._points != null && this._points.Length != 0)
		{
			int num = this._points.Length;
			if (num == 1)
			{
				this._points[0].controlPointIn.transform.position = this._points[0].point.position;
				this._points[0].controlPointOut.transform.position = this._points[0].point.position;
			}
			else if (num == 2 && !this._loop)
			{
				this._points[0].controlPointIn.transform.position = this._points[0].point.position;
				this._points[0].controlPointOut.transform.position = this._points[0].point.position;
				this._points[1].controlPointIn.transform.position = this._points[1].point.position;
				this._points[1].controlPointOut.transform.position = this._points[1].point.position;
			}
			else
			{
				int num2;
				if (this._loop)
				{
					num2 = num + 1;
				}
				else
				{
					num2 = num - 1;
				}
				if (this.K == null || this.K.Length < num2 + 1)
				{
					this.K = new Vector3[num2 + 1];
				}
				if (this._loop)
				{
					this.K[0] = this._points[num - 1].point.position;
					for (int i = 1; i < num2; i++)
					{
						this.K[i] = this._points[i - 1].point.position;
					}
					this.K[num2] = this._points[0].point.position;
				}
				else
				{
					for (int j = 0; j < num; j++)
					{
						this.K[j] = this._points[j].point.position;
					}
				}
				if (this.a == null || this.a.Length < num2)
				{
					this.a = new float[num2];
				}
				if (this.b == null || this.b.Length < num2)
				{
					this.b = new float[num2];
				}
				if (this.c == null || this.c.Length < num2)
				{
					this.c = new float[num2];
				}
				if (this.r == null || this.r.Length < num2)
				{
					this.r = new Vector3[num2];
				}
				this.a[0] = 0f;
				this.b[0] = 2f;
				this.c[0] = 1f;
				this.r[0] = this.K[0] + 2f * this.K[1];
				for (int k = 1; k < num2 - 1; k++)
				{
					this.a[k] = 1f;
					this.b[k] = 4f;
					this.c[k] = 1f;
					this.r[k] = 4f * this.K[k] + 2f * this.K[k + 1];
				}
				this.a[num2 - 1] = 2f;
				this.b[num2 - 1] = 7f;
				this.c[num2 - 1] = 0f;
				this.r[num2 - 1] = 8f * this.K[num2 - 1] + this.K[num2];
				for (int l = 1; l < num2; l++)
				{
					float num3 = this.a[l] / this.b[l - 1];
					this.b[l] = this.b[l] - num3 * this.c[l - 1];
					this.r[l] = this.r[l] - num3 * this.r[l - 1];
				}
				if (this._loop)
				{
					Vector3 vector = this.r[num2 - 1] / this.b[num2 - 1];
					this._points[num2 - 2].controlPointOut.transform.position = (this.r[num2 - 1] - this.c[num2 - 1] * vector) / this.b[num2 - 1];
					for (int m = num2 - 3; m >= 0; m--)
					{
						this._points[m].controlPointOut.transform.position = (this.r[m + 1] - this.c[m + 1] * this._points[m + 1].controlPointOut.transform.position) / this.b[m + 1];
					}
				}
				else
				{
					this._points[num2].controlPointOut.transform.position = this._points[num2].point.position;
					this._points[num2 - 1].controlPointOut.transform.position = this.r[num2 - 1] / this.b[num2 - 1];
					for (int n = num2 - 2; n >= 0; n--)
					{
						this._points[n].controlPointOut.transform.position = (this.r[n] - this.c[n] * this._points[n + 1].controlPointOut.transform.position) / this.b[n];
					}
				}
				if (this._loop)
				{
					for (int num4 = 0; num4 < num2 - 1; num4++)
					{
						this._points[num4].controlPointIn.transform.position = 2f * this.K[num4 + 1] - this._points[num4].controlPointOut.transform.position;
					}
				}
				else
				{
					this._points[0].controlPointIn.transform.position = this._points[0].point.position;
					for (int num5 = 1; num5 < num2; num5++)
					{
						this._points[num5].controlPointIn.transform.position = 2f * this.K[num5] - this._points[num5].controlPointOut.transform.position;
					}
					this._points[num2].controlPointIn.transform.position = 0.5f * (this.K[num2] + this._points[num2 - 1].controlPointOut.transform.position);
				}
			}
		}
	}

	// Token: 0x060060BF RID: 24767 RVA: 0x001D15BC File Offset: 0x001CF9BC
	protected void SlaveOutputControlPoints()
	{
		for (int i = 0; i < this._points.Length; i++)
		{
			if (this._points[i] != null && this._points[i].controlPointIn != null && this._points[i].controlPointOut != null)
			{
				Vector3 vector = this._points[i].point.transform.position - this._points[i].controlPointIn.transform.position;
				this._points[i].controlPointOut.transform.position = this._points[i].point.transform.position + vector;
			}
		}
	}

	// Token: 0x060060C0 RID: 24768 RVA: 0x001D1690 File Offset: 0x001CFA90
	public void RegenerateMesh()
	{
		if (this._points != null && this._points.Length > 1)
		{
			int num = this._points.Length - 1;
			if (this._loop)
			{
				num++;
			}
			this.mesh = new Mesh();
			this.RegisterAllocatedObject(this.mesh);
			this.indices = new int[num * this._curveSmooth];
			this.vertices = new Vector3[num * this._curveSmooth];
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < this._curveSmooth; j++)
				{
					this.indices[num2] = num2;
					num2++;
				}
			}
			this.mesh.vertices = this.vertices;
			this.mesh.SetIndices(this.indices, MeshTopology.LineStrip, 0);
		}
		else
		{
			this.mesh = new Mesh();
			this.RegisterAllocatedObject(this.mesh);
		}
	}

	// Token: 0x060060C1 RID: 24769 RVA: 0x001D1788 File Offset: 0x001CFB88
	protected void UpdateMesh()
	{
		if (this.mesh != null && this._points.Length > 1)
		{
			int num = this._points.Length - 1;
			if (this._loop)
			{
				num++;
			}
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				Vector3 position = this._points[i].point.position;
				Vector3 position2 = this._points[i].controlPointOut.transform.position;
				Vector3 position3;
				Vector3 position4;
				if (this._loop && i == this._points.Length - 1)
				{
					position3 = this._points[0].controlPointIn.transform.position;
					position4 = this._points[0].point.position;
				}
				else
				{
					position3 = this._points[i + 1].controlPointIn.transform.position;
					position4 = this._points[i + 1].point.position;
				}
				for (int j = 0; j < this._curveSmooth; j++)
				{
					float num3 = (float)j * 1f / (float)(this._curveSmooth - 1);
					float num4 = 1f - num3;
					float num5 = num4 * num4;
					float d = num5 * num4;
					float num6 = num3 * num3;
					float d2 = num6 * num3;
					Vector3 vector = position * d + 3f * position2 * num5 * num3 + 3f * position3 * num4 * num6 + position4 * d2;
					this.vertices[num2] = vector;
					num2++;
				}
			}
			this.mesh.vertices = this.vertices;
			this.mesh.RecalculateBounds();
		}
	}

	// Token: 0x060060C2 RID: 24770 RVA: 0x001D196C File Offset: 0x001CFD6C
	public void ResyncControlPoints()
	{
		CubicBezierCurve.CurveType curveType = this._curveType;
		if (curveType != CubicBezierCurve.CurveType.Auto)
		{
			if (curveType != CubicBezierCurve.CurveType.Smooth)
			{
				if (curveType == CubicBezierCurve.CurveType.Disjoint)
				{
					this.SetCurveDisjoint();
				}
			}
			else
			{
				this.SetCurveSmooth();
			}
		}
		else
		{
			this.SetCurveAuto();
		}
	}

	// Token: 0x060060C3 RID: 24771 RVA: 0x001D19BC File Offset: 0x001CFDBC
	protected void SetCurveAuto()
	{
		for (int i = 0; i < this._points.Length; i++)
		{
			if (this._points[i] != null)
			{
				if (this._points[i].controlPointIn != null)
				{
					this._points[i].controlPointIn.gameObject.SetActive(false);
				}
				if (this._points[i].controlPointOut != null)
				{
					this._points[i].controlPointOut.gameObject.SetActive(false);
				}
			}
		}
	}

	// Token: 0x060060C4 RID: 24772 RVA: 0x001D1A58 File Offset: 0x001CFE58
	protected void SetCurveSmooth()
	{
		for (int i = 0; i < this._points.Length; i++)
		{
			if (this._points[i] != null)
			{
				if (this._points[i].controlPointIn != null)
				{
					this._points[i].controlPointIn.gameObject.SetActive(true);
				}
				if (this._points[i].controlPointOut != null)
				{
					this._points[i].controlPointOut.gameObject.SetActive(false);
				}
			}
		}
	}

	// Token: 0x060060C5 RID: 24773 RVA: 0x001D1AF4 File Offset: 0x001CFEF4
	protected void SetCurveDisjoint()
	{
		for (int i = 0; i < this._points.Length; i++)
		{
			if (this._points[i] != null)
			{
				if (this._points[i].controlPointIn != null)
				{
					this._points[i].controlPointIn.gameObject.SetActive(true);
				}
				if (this._points[i].controlPointOut != null)
				{
					this._points[i].controlPointOut.gameObject.SetActive(true);
				}
			}
		}
	}

	// Token: 0x060060C6 RID: 24774 RVA: 0x001D1B90 File Offset: 0x001CFF90
	protected void SetControlPoints()
	{
		CubicBezierCurve.CurveType curveType = this._curveType;
		if (curveType != CubicBezierCurve.CurveType.Auto)
		{
			if (curveType == CubicBezierCurve.CurveType.Smooth)
			{
				this.SlaveOutputControlPoints();
			}
		}
		else
		{
			this.AutoComputeControlPoints();
		}
	}

	// Token: 0x060060C7 RID: 24775 RVA: 0x001D1BD4 File Offset: 0x001CFFD4
	protected void DrawMesh()
	{
		if (this.mesh != null && this.material != null && this._draw)
		{
			this.UpdateMesh();
			Matrix4x4 identity = Matrix4x4.identity;
			Graphics.DrawMesh(this.mesh, identity, this.materialLocal, base.gameObject.layer, null, 0, null, false, false);
		}
	}

	// Token: 0x060060C8 RID: 24776 RVA: 0x001D1C3C File Offset: 0x001D003C
	public Vector3 GetPositionFromPoint(int fromPoint, float t)
	{
		Vector3 position = this._points[fromPoint].point.position;
		Vector3 position2 = this._points[fromPoint].controlPointOut.transform.position;
		Vector3 result;
		if (this._points.Length == 1)
		{
			result = position;
		}
		else
		{
			Vector3 position3;
			Vector3 position4;
			if (fromPoint == this._points.Length - 1)
			{
				if (!this._loop)
				{
					return position;
				}
				position3 = this._points[0].controlPointIn.transform.position;
				position4 = this._points[0].point.position;
			}
			else
			{
				position3 = this._points[fromPoint + 1].controlPointIn.transform.position;
				position4 = this._points[fromPoint + 1].point.position;
			}
			float num = 1f - t;
			float num2 = num * num;
			float d = num2 * num;
			float num3 = t * t;
			float d2 = num3 * t;
			result = position * d + 3f * position2 * num2 * t + 3f * position3 * num * num3 + position4 * d2;
		}
		return result;
	}

	// Token: 0x060060C9 RID: 24777 RVA: 0x001D1D84 File Offset: 0x001D0184
	public Quaternion GetRotationFromPoint(int fromPoint, float t)
	{
		Quaternion rotation = this._points[fromPoint].point.rotation;
		Quaternion result;
		if (this._points.Length == 1)
		{
			result = rotation;
		}
		else
		{
			Quaternion rotation2;
			if (fromPoint == this._points.Length - 1)
			{
				if (!this._loop)
				{
					return rotation;
				}
				rotation2 = this._points[0].point.rotation;
			}
			else
			{
				rotation2 = this._points[fromPoint + 1].point.rotation;
			}
			result = Quaternion.Lerp(rotation, rotation2, t);
		}
		return result;
	}

	// Token: 0x060060CA RID: 24778 RVA: 0x001D1E14 File Offset: 0x001D0214
	protected virtual void Init()
	{
		this.loopJSON = new JSONStorableBool("loop", this._loop, new JSONStorableBool.SetBoolCallback(this.SyncLoop));
		this.loopJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterBool(this.loopJSON);
		string[] names = Enum.GetNames(typeof(CubicBezierCurve.CurveType));
		List<string> choicesList = new List<string>(names);
		this.curveTypeJSON = new JSONStorableStringChooser("curveType", choicesList, this._curveType.ToString(), "Curve Type", new JSONStorableStringChooser.SetStringCallback(this.SetCurveType));
		this.curveTypeJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterStringChooser(this.curveTypeJSON);
		if (this.material != null)
		{
			this.materialLocal = UnityEngine.Object.Instantiate<Material>(this.material);
			this.RegisterAllocatedObject(this.materialLocal);
			this._drawColor = this.materialLocal.color;
		}
	}

	// Token: 0x060060CB RID: 24779 RVA: 0x001D1EFD File Offset: 0x001D02FD
	protected override void Awake()
	{
		if (!this.awakecalled)
		{
			base.Awake();
			if (Application.isPlaying)
			{
				this.Init();
				this.InitUI();
				this.InitUIAlt();
			}
		}
	}

	// Token: 0x060060CC RID: 24780 RVA: 0x001D1F2C File Offset: 0x001D032C
	protected void Update()
	{
		if (this.mesh == null)
		{
			this.RegenerateMesh();
		}
		this.SetControlPoints();
		this.DrawMesh();
	}

	// Token: 0x060060CD RID: 24781 RVA: 0x001D1F51 File Offset: 0x001D0351
	protected virtual void OnDestroy()
	{
		this.DestroyAllocatedObjects();
	}

	// Token: 0x0400504E RID: 20558
	protected string[] customParamNames = new string[]
	{
		"curveType"
	};

	// Token: 0x0400504F RID: 20559
	protected List<UnityEngine.Object> allocatedObjects;

	// Token: 0x04005050 RID: 20560
	[SerializeField]
	protected bool _draw;

	// Token: 0x04005051 RID: 20561
	protected Color _drawColor;

	// Token: 0x04005052 RID: 20562
	[SerializeField]
	protected CubicBezierPoint[] _points;

	// Token: 0x04005053 RID: 20563
	[SerializeField]
	protected bool _loop = true;

	// Token: 0x04005054 RID: 20564
	protected JSONStorableBool loopJSON;

	// Token: 0x04005055 RID: 20565
	protected JSONStorableStringChooser curveTypeJSON;

	// Token: 0x04005056 RID: 20566
	[SerializeField]
	protected CubicBezierCurve.CurveType _curveType;

	// Token: 0x04005057 RID: 20567
	[SerializeField]
	protected int _curveSmooth = 10;

	// Token: 0x04005058 RID: 20568
	public Material material;

	// Token: 0x04005059 RID: 20569
	protected Material materialLocal;

	// Token: 0x0400505A RID: 20570
	protected int[] indices;

	// Token: 0x0400505B RID: 20571
	protected Vector3[] vertices;

	// Token: 0x0400505C RID: 20572
	protected Mesh mesh;

	// Token: 0x0400505D RID: 20573
	protected Vector3[] K;

	// Token: 0x0400505E RID: 20574
	protected Vector3[] r;

	// Token: 0x0400505F RID: 20575
	protected float[] a;

	// Token: 0x04005060 RID: 20576
	protected float[] b;

	// Token: 0x04005061 RID: 20577
	protected float[] c;

	// Token: 0x02000C88 RID: 3208
	public enum CurveType
	{
		// Token: 0x04005063 RID: 20579
		Auto,
		// Token: 0x04005064 RID: 20580
		Smooth,
		// Token: 0x04005065 RID: 20581
		Disjoint
	}
}
