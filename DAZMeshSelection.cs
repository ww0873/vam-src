using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AEC RID: 2796
public class DAZMeshSelection : MonoBehaviour
{
	// Token: 0x06004AD2 RID: 19154 RVA: 0x0019E027 File Offset: 0x0019C427
	public DAZMeshSelection()
	{
	}

	// Token: 0x17000A9F RID: 2719
	// (get) Token: 0x06004AD3 RID: 19155 RVA: 0x0019E041 File Offset: 0x0019C441
	// (set) Token: 0x06004AD4 RID: 19156 RVA: 0x0019E049 File Offset: 0x0019C449
	public int subMeshSelection
	{
		get
		{
			return this._subMeshSelection;
		}
		set
		{
			if (value != this._subMeshSelection)
			{
				this._subMeshSelection = value;
			}
		}
	}

	// Token: 0x17000AA0 RID: 2720
	// (get) Token: 0x06004AD5 RID: 19157 RVA: 0x0019E05E File Offset: 0x0019C45E
	public List<int> selectedVertices
	{
		get
		{
			return this._selectedVertices;
		}
	}

	// Token: 0x17000AA1 RID: 2721
	// (get) Token: 0x06004AD6 RID: 19158 RVA: 0x0019E066 File Offset: 0x0019C466
	public bool changed
	{
		get
		{
			return this._changed;
		}
	}

	// Token: 0x06004AD7 RID: 19159 RVA: 0x0019E06E File Offset: 0x0019C46E
	public void clearChanged()
	{
		this._changed = false;
	}

	// Token: 0x06004AD8 RID: 19160 RVA: 0x0019E077 File Offset: 0x0019C477
	protected void InitList(bool force = false)
	{
		if (this._selectedVertices == null || force)
		{
			this._selectedVertices = new List<int>();
		}
	}

	// Token: 0x06004AD9 RID: 19161 RVA: 0x0019E098 File Offset: 0x0019C498
	protected void InitDict(bool force = false)
	{
		this.InitList(force);
		if (this.selectedVerticesDict == null || force)
		{
			this.selectedVerticesDict = new Dictionary<int, bool>();
			foreach (int key in this.selectedVertices)
			{
				this.selectedVerticesDict.Add(key, true);
			}
		}
	}

	// Token: 0x06004ADA RID: 19162 RVA: 0x0019E120 File Offset: 0x0019C520
	public bool IsVertexSelected(int vid)
	{
		this.InitDict(false);
		return this.selectedVerticesDict.ContainsKey(vid);
	}

	// Token: 0x06004ADB RID: 19163 RVA: 0x0019E138 File Offset: 0x0019C538
	public void SelectVertex(int vid)
	{
		this.InitDict(false);
		if (vid >= 0 && vid <= this.mesh.numBaseVertices && !this.selectedVerticesDict.ContainsKey(vid))
		{
			this._changed = true;
			this._selectedVertices.Add(vid);
			this.selectedVerticesDict.Add(vid, true);
		}
	}

	// Token: 0x06004ADC RID: 19164 RVA: 0x0019E195 File Offset: 0x0019C595
	public void DeselectVertex(int vid)
	{
		this.InitDict(false);
		if (this.selectedVerticesDict.ContainsKey(vid))
		{
			this._changed = true;
			this._selectedVertices.Remove(vid);
			this.selectedVerticesDict.Remove(vid);
		}
	}

	// Token: 0x06004ADD RID: 19165 RVA: 0x0019E1D0 File Offset: 0x0019C5D0
	public void ToggleVertexSelection(int vid)
	{
		this.InitDict(false);
		this._changed = true;
		if (this.selectedVerticesDict.ContainsKey(vid))
		{
			this._selectedVertices.Remove(vid);
			this.selectedVerticesDict.Remove(vid);
		}
		else
		{
			this._selectedVertices.Add(vid);
			this.selectedVerticesDict.Add(vid, true);
		}
	}

	// Token: 0x06004ADE RID: 19166 RVA: 0x0019E234 File Offset: 0x0019C634
	public void ClearSelection()
	{
		this._changed = true;
		this.InitDict(true);
	}

	// Token: 0x06004ADF RID: 19167 RVA: 0x0019E244 File Offset: 0x0019C644
	public void Start()
	{
		this.InitList(false);
	}

	// Token: 0x040039A6 RID: 14758
	public Transform meshTransform;

	// Token: 0x040039A7 RID: 14759
	public DAZMesh mesh;

	// Token: 0x040039A8 RID: 14760
	public string selectionName;

	// Token: 0x040039A9 RID: 14761
	public bool useUVVertices;

	// Token: 0x040039AA RID: 14762
	[SerializeField]
	protected int _subMeshSelection = -1;

	// Token: 0x040039AB RID: 14763
	[SerializeField]
	protected List<int> _selectedVertices;

	// Token: 0x040039AC RID: 14764
	protected Dictionary<int, bool> selectedVerticesDict;

	// Token: 0x040039AD RID: 14765
	public bool showSelection;

	// Token: 0x040039AE RID: 14766
	public bool showBackfaces;

	// Token: 0x040039AF RID: 14767
	public float handleSize = 0.002f;

	// Token: 0x040039B0 RID: 14768
	[SerializeField]
	protected bool _changed;
}
