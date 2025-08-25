using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AF7 RID: 2807
[ExecuteInEditMode]
public class DAZMorphSplit : MonoBehaviour
{
	// Token: 0x06004B91 RID: 19345 RVA: 0x001A5138 File Offset: 0x001A3538
	public DAZMorphSplit()
	{
	}

	// Token: 0x06004B92 RID: 19346 RVA: 0x001A5159 File Offset: 0x001A3559
	public void ClickVertex(int vid)
	{
		if (this.IsFilterVertex(vid))
		{
			this.filterVerticesDict.Remove(vid);
		}
		else
		{
			this.filterVerticesDict.Add(vid, true);
		}
	}

	// Token: 0x06004B93 RID: 19347 RVA: 0x001A5186 File Offset: 0x001A3586
	public void UpclickVertex(int vid)
	{
	}

	// Token: 0x06004B94 RID: 19348 RVA: 0x001A5188 File Offset: 0x001A3588
	public void OnVertex(int vid)
	{
		if (!this.IsFilterVertex(vid) && this.IsInputMorphVertex(vid))
		{
			this.filterVerticesDict.Add(vid, true);
		}
	}

	// Token: 0x06004B95 RID: 19349 RVA: 0x001A51AF File Offset: 0x001A35AF
	public void OffVertex(int vid)
	{
		if (this.IsFilterVertex(vid))
		{
			this.filterVerticesDict.Remove(vid);
		}
	}

	// Token: 0x06004B96 RID: 19350 RVA: 0x001A51CC File Offset: 0x001A35CC
	public int GetBaseVertex(int vid)
	{
		int num;
		if (this._uvVertToBaseVertDict.TryGetValue(vid, out num))
		{
			vid = num;
		}
		return vid;
	}

	// Token: 0x06004B97 RID: 19351 RVA: 0x001A51F0 File Offset: 0x001A35F0
	public bool IsBaseVertex(int vid)
	{
		return this._uvVertToBaseVertDict == null || !this._uvVertToBaseVertDict.ContainsKey(vid);
	}

	// Token: 0x17000ACB RID: 2763
	// (get) Token: 0x06004B98 RID: 19352 RVA: 0x001A520E File Offset: 0x001A360E
	public DAZMorph inputMorph
	{
		get
		{
			return this._inputMorph;
		}
	}

	// Token: 0x06004B99 RID: 19353 RVA: 0x001A5218 File Offset: 0x001A3618
	protected void CheckInputMorph()
	{
		if (this.bankToModify != null)
		{
			if (this._inputMorph == null || this._inputMorph.morphName != this.inputMorphName)
			{
				this._inputMorph = this.bankToModify.GetMorph(this.inputMorphName);
				this._inputMorphVerticesDict = new Dictionary<int, bool>();
				if (this._inputMorph != null)
				{
					for (int i = 0; i < this._inputMorph.deltas.Length; i++)
					{
						int vertex = this._inputMorph.deltas[i].vertex;
						this._inputMorphVerticesDict.Add(vertex, true);
					}
				}
			}
		}
		else
		{
			this._inputMorph = null;
			this._inputMorphVerticesDict = null;
		}
	}

	// Token: 0x06004B9A RID: 19354 RVA: 0x001A52DA File Offset: 0x001A36DA
	public bool IsValidInputMorph()
	{
		this.CheckInputMorph();
		return this._inputMorph != null;
	}

	// Token: 0x17000ACC RID: 2764
	// (get) Token: 0x06004B9B RID: 19355 RVA: 0x001A52F0 File Offset: 0x001A36F0
	public DAZMorph outputMorph
	{
		get
		{
			return this._outputMorph;
		}
	}

	// Token: 0x06004B9C RID: 19356 RVA: 0x001A52F8 File Offset: 0x001A36F8
	public void InvalidateOutputMorph()
	{
		this._outputMorph = null;
	}

	// Token: 0x06004B9D RID: 19357 RVA: 0x001A5304 File Offset: 0x001A3704
	protected void CheckOutputMorph()
	{
		if (this.bankToModify != null)
		{
			if (this._outputMorph == null || this._outputMorph.morphName != this.outputMorphName)
			{
				this._outputMorph = this.bankToModify.GetMorph(this.outputMorphName);
			}
		}
		else
		{
			this._outputMorph = null;
		}
	}

	// Token: 0x06004B9E RID: 19358 RVA: 0x001A536B File Offset: 0x001A376B
	public bool IsExistingOutputMorph()
	{
		this.CheckOutputMorph();
		return this._outputMorph != null;
	}

	// Token: 0x06004B9F RID: 19359 RVA: 0x001A5381 File Offset: 0x001A3781
	public bool IsInputMorphVertex(int vid)
	{
		this.CheckInputMorph();
		return this._inputMorphVerticesDict != null && this._inputMorphVerticesDict.ContainsKey(vid);
	}

	// Token: 0x06004BA0 RID: 19360 RVA: 0x001A53A8 File Offset: 0x001A37A8
	public bool IsFilterVertex(int vid)
	{
		return this.filterVerticesDict != null && this.filterVerticesDict.ContainsKey(vid);
	}

	// Token: 0x06004BA1 RID: 19361 RVA: 0x001A53CC File Offset: 0x001A37CC
	protected void InitCaches(bool force = false)
	{
		if (this.filterVerticesDict == null)
		{
			this.filterVerticesDict = new Dictionary<int, bool>();
		}
		if (this.bankToModify != null && this.bankToModify.connectedMesh != null)
		{
			this._uvVertToBaseVertDict = this.bankToModify.connectedMesh.uvVertToBaseVert;
		}
		else
		{
			this._uvVertToBaseVertDict = new Dictionary<int, int>();
		}
	}

	// Token: 0x06004BA2 RID: 19362 RVA: 0x001A543C File Offset: 0x001A383C
	public void FilterMorph()
	{
		if (this.bankToModify != null && this.filterVerticesDict != null)
		{
			DAZMesh connectedMesh = this.bankToModify.connectedMesh;
			this.CheckInputMorph();
			if (this._inputMorph != null)
			{
				this.CheckOutputMorph();
				if (this._outputMorph == null)
				{
					this._outputMorph = new DAZMorph(this._inputMorph);
					List<DAZMorphVertex> list = new List<DAZMorphVertex>();
					foreach (DAZMorphVertex dazmorphVertex in this._inputMorph.deltas)
					{
						if (this.filterVerticesDict.ContainsKey(dazmorphVertex.vertex))
						{
							list.Add(new DAZMorphVertex
							{
								delta = dazmorphVertex.delta,
								vertex = dazmorphVertex.vertex
							});
						}
					}
					this._outputMorph.morphName = this.outputMorphName;
					this._outputMorph.displayName = this.outputMorphName;
					this._outputMorph.deltas = list.ToArray();
					this._outputMorph.numDeltas = this._outputMorph.deltas.Length;
					this.bankToModify.AddMorphUsingSubBanks(this._outputMorph);
				}
				else
				{
					Debug.Log("Output morph with name " + this.outputMorphName + " already exists");
				}
			}
		}
	}

	// Token: 0x06004BA3 RID: 19363 RVA: 0x001A558E File Offset: 0x001A398E
	private void OnEnable()
	{
		if (Application.isEditor)
		{
			this.InitCaches(true);
		}
	}

	// Token: 0x04003A60 RID: 14944
	public DAZMorphBank bankToModify;

	// Token: 0x04003A61 RID: 14945
	public string inputMorphName;

	// Token: 0x04003A62 RID: 14946
	public string outputMorphName;

	// Token: 0x04003A63 RID: 14947
	public Dictionary<int, bool> filterVerticesDict;

	// Token: 0x04003A64 RID: 14948
	public float handleSize = 0.0001f;

	// Token: 0x04003A65 RID: 14949
	public bool showHandles;

	// Token: 0x04003A66 RID: 14950
	public bool showBackfaceHandles;

	// Token: 0x04003A67 RID: 14951
	public int subMeshSelection = -1;

	// Token: 0x04003A68 RID: 14952
	public int subMeshSelection2 = -1;

	// Token: 0x04003A69 RID: 14953
	protected DAZMorph _inputMorph;

	// Token: 0x04003A6A RID: 14954
	protected Dictionary<int, bool> _inputMorphVerticesDict;

	// Token: 0x04003A6B RID: 14955
	protected DAZMorph _outputMorph;

	// Token: 0x04003A6C RID: 14956
	protected Dictionary<int, int> _uvVertToBaseVertDict;
}
