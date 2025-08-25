using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using SimpleJSON;
using UnityEngine;

// Token: 0x02000B02 RID: 2818
public class DAZSkin : MonoBehaviour
{
	// Token: 0x06004C7F RID: 19583 RVA: 0x00187A6E File Offset: 0x00185E6E
	public DAZSkin()
	{
	}

	// Token: 0x17000AD5 RID: 2773
	// (get) Token: 0x06004C80 RID: 19584 RVA: 0x00187A93 File Offset: 0x00185E93
	// (set) Token: 0x06004C81 RID: 19585 RVA: 0x00187A9B File Offset: 0x00185E9B
	public bool useGeneralWeights
	{
		get
		{
			return this._useGeneralWeights;
		}
		set
		{
			this._useGeneralWeights = value;
		}
	}

	// Token: 0x17000AD6 RID: 2774
	// (get) Token: 0x06004C82 RID: 19586 RVA: 0x00187AA4 File Offset: 0x00185EA4
	// (set) Token: 0x06004C83 RID: 19587 RVA: 0x00187AAC File Offset: 0x00185EAC
	public bool useFastNormals
	{
		get
		{
			return this._useFastNormals;
		}
		set
		{
			this._useFastNormals = value;
		}
	}

	// Token: 0x17000AD7 RID: 2775
	// (get) Token: 0x06004C84 RID: 19588 RVA: 0x00187AB5 File Offset: 0x00185EB5
	// (set) Token: 0x06004C85 RID: 19589 RVA: 0x00187ABD File Offset: 0x00185EBD
	public bool renormalize
	{
		get
		{
			return this._renormalize;
		}
		set
		{
			this._renormalize = value;
		}
	}

	// Token: 0x17000AD8 RID: 2776
	// (get) Token: 0x06004C86 RID: 19590 RVA: 0x00187AC6 File Offset: 0x00185EC6
	// (set) Token: 0x06004C87 RID: 19591 RVA: 0x00187ACE File Offset: 0x00185ECE
	public bool useSmoothing
	{
		get
		{
			return this._useSmoothing;
		}
		set
		{
			this._useSmoothing = value;
		}
	}

	// Token: 0x06004C88 RID: 19592 RVA: 0x00187AD7 File Offset: 0x00185ED7
	public void SetSmoothingFromUIToggle()
	{
	}

	// Token: 0x17000AD9 RID: 2777
	// (get) Token: 0x06004C89 RID: 19593 RVA: 0x00187AD9 File Offset: 0x00185ED9
	public int numBones
	{
		get
		{
			return this._numBones;
		}
	}

	// Token: 0x06004C8A RID: 19594 RVA: 0x00187AE1 File Offset: 0x00185EE1
	public void ImportStart()
	{
		this.importNodes = new List<DAZNode>();
	}

	// Token: 0x06004C8B RID: 19595 RVA: 0x00187AF0 File Offset: 0x00185EF0
	public void ImportNode(JSONNode jn)
	{
		DAZNode daznode = new DAZNode();
		daznode.name = jn["id"];
		string text = jn["rotation_order"];
		Quaternion2Angles.RotationOrder rotationOrder;
		if (text != null)
		{
			if (text == "XYZ")
			{
				rotationOrder = Quaternion2Angles.RotationOrder.ZYX;
				goto IL_D9;
			}
			if (text == "XZY")
			{
				rotationOrder = Quaternion2Angles.RotationOrder.YZX;
				goto IL_D9;
			}
			if (text == "YXZ")
			{
				rotationOrder = Quaternion2Angles.RotationOrder.ZXY;
				goto IL_D9;
			}
			if (text == "YZX")
			{
				rotationOrder = Quaternion2Angles.RotationOrder.XZY;
				goto IL_D9;
			}
			if (text == "ZXY")
			{
				rotationOrder = Quaternion2Angles.RotationOrder.YXZ;
				goto IL_D9;
			}
			if (text == "ZYX")
			{
				rotationOrder = Quaternion2Angles.RotationOrder.XYZ;
				goto IL_D9;
			}
		}
		UnityEngine.Debug.LogError("Bad rotation order in json: " + text);
		rotationOrder = Quaternion2Angles.RotationOrder.XYZ;
		IL_D9:
		daznode.rotationOrder = rotationOrder;
		bool flag = false;
		if (this.importNodes == null)
		{
			this.importNodes = new List<DAZNode>();
		}
		for (int i = 0; i < this.importNodes.Count; i++)
		{
			if (this.importNodes[i].name == daznode.name)
			{
				this.importNodes[i] = daznode;
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			this.importNodes.Add(daznode);
		}
	}

	// Token: 0x06004C8C RID: 19596 RVA: 0x00187C60 File Offset: 0x00186060
	protected DAZNode FindNode(string name)
	{
		for (int i = 0; i < this.importNodes.Count; i++)
		{
			if (this.importNodes[i].name == name)
			{
				return this.importNodes[i];
			}
		}
		UnityEngine.Debug.LogWarning("Could not find node " + name);
		return null;
	}

	// Token: 0x06004C8D RID: 19597 RVA: 0x00187CC3 File Offset: 0x001860C3
	protected Dictionary<int, DAZMeshVertexWeights> WalkBonesAndAccumulateWeights(Transform bone)
	{
		this.vertexDoneAccumulating = new Dictionary<int, bool>();
		return this.WalkBonesAndAccumulateWeightsRecursive(bone);
	}

	// Token: 0x06004C8E RID: 19598 RVA: 0x00187CD8 File Offset: 0x001860D8
	protected Dictionary<int, DAZMeshVertexWeights> WalkBonesAndAccumulateWeightsRecursive(Transform bone)
	{
		Dictionary<int, DAZMeshVertexWeights> dictionary;
		if (this.boneWeightsMap.TryGetValue(bone.name, out dictionary))
		{
			IEnumerator enumerator = bone.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform bone2 = (Transform)obj;
					Dictionary<int, DAZMeshVertexWeights> dictionary2 = this.WalkBonesAndAccumulateWeightsRecursive(bone2);
					if (dictionary2 != null)
					{
						foreach (int key in dictionary2.Keys)
						{
							DAZMeshVertexWeights dazmeshVertexWeights;
							if (!this.vertexDoneAccumulating.ContainsKey(key) && dictionary2.TryGetValue(key, out dazmeshVertexWeights))
							{
								DAZMeshVertexWeights dazmeshVertexWeights2;
								if (dictionary.TryGetValue(key, out dazmeshVertexWeights2))
								{
									dazmeshVertexWeights2.xweight += dazmeshVertexWeights.xweight;
									dazmeshVertexWeights2.yweight += dazmeshVertexWeights.yweight;
									dazmeshVertexWeights2.zweight += dazmeshVertexWeights.zweight;
									if (dazmeshVertexWeights2.xweight > 0.99999f && dazmeshVertexWeights2.yweight > 0.99999f && dazmeshVertexWeights2.zweight > 0.99999f)
									{
										dazmeshVertexWeights2.xweight = 1f;
										dazmeshVertexWeights2.yweight = 1f;
										dazmeshVertexWeights2.zweight = 1f;
										this.vertexDoneAccumulating.Add(key, true);
									}
									dictionary.Remove(key);
									dictionary.Add(key, dazmeshVertexWeights2);
								}
								else if (dazmeshVertexWeights.xweight > 0.99999f && dazmeshVertexWeights.yweight > 0.99999f && dazmeshVertexWeights.zweight > 0.99999f)
								{
									dazmeshVertexWeights.xweight = 1f;
									dazmeshVertexWeights.yweight = 1f;
									dazmeshVertexWeights.zweight = 1f;
									this.vertexDoneAccumulating.Add(key, true);
								}
								else
								{
									DAZMeshVertexWeights dazmeshVertexWeights3 = new DAZMeshVertexWeights();
									dazmeshVertexWeights3.vertex = dazmeshVertexWeights.vertex;
									dazmeshVertexWeights3.xweight = dazmeshVertexWeights.xweight;
									dazmeshVertexWeights3.yweight = dazmeshVertexWeights.yweight;
									dazmeshVertexWeights3.zweight = dazmeshVertexWeights.zweight;
									dictionary.Add(key, dazmeshVertexWeights3);
								}
							}
						}
					}
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
			return dictionary;
		}
		return null;
	}

	// Token: 0x06004C8F RID: 19599 RVA: 0x00187F5C File Offset: 0x0018635C
	protected void CreateBoneWeightsArray()
	{
		foreach (string key in this.boneWeightsMap.Keys)
		{
			Dictionary<int, DAZMeshVertexWeights> dictionary;
			int num;
			if (this.boneWeightsMap.TryGetValue(key, out dictionary) && this.boneNameToIndexMap.TryGetValue(key, out num))
			{
				int num2 = 0;
				DAZNode daznode = this.nodes[num];
				daznode.weights = new DAZMeshVertexWeights[dictionary.Count];
				foreach (int key2 in dictionary.Keys)
				{
					DAZMeshVertexWeights dazmeshVertexWeights;
					if (dictionary.TryGetValue(key2, out dazmeshVertexWeights))
					{
						daznode.weights[num2] = dazmeshVertexWeights;
						num2++;
					}
				}
			}
		}
	}

	// Token: 0x06004C90 RID: 19600 RVA: 0x00188064 File Offset: 0x00186464
	public void Import(JSONNode jn)
	{
		if (this.rootBone == null)
		{
			UnityEngine.Debug.LogError("Root bone not set. Can't import skin");
			return;
		}
		JSONNode jsonnode = jn["skin"]["joints"];
		this._numBones = jsonnode.Count;
		DAZMesh[] components = base.GetComponents<DAZMesh>();
		this.dazMesh = null;
		foreach (DAZMesh dazmesh in components)
		{
			if (dazmesh.geometryId == this.geometryId)
			{
				this.dazMesh = dazmesh;
				break;
			}
		}
		if (this.dazMesh == null)
		{
			UnityEngine.Debug.LogError("Could not find DAZMesh component with geometryID " + this.geometryId);
			return;
		}
		Dictionary<int, List<int>> baseVertToUVVertFullMap = this.dazMesh.baseVertToUVVertFullMap;
		this.boneNameToIndexMap = new Dictionary<string, int>();
		this.boneWeightsMap = new Dictionary<string, Dictionary<int, DAZMeshVertexWeights>>();
		this.nodes = new DAZNode[this._numBones];
		int num = 0;
		IEnumerator enumerator = jsonnode.AsArray.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				JSONNode jsonnode2 = (JSONNode)obj;
				string text = jsonnode2["id"];
				DAZNode daznode = this.FindNode(text);
				this.nodes[num] = daznode;
				this.boneNameToIndexMap.Add(text, num);
				Dictionary<int, DAZMeshVertexWeights> dictionary = new Dictionary<int, DAZMeshVertexWeights>();
				IEnumerator enumerator2 = jsonnode2["node_weights"]["values"].AsArray.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						object obj2 = enumerator2.Current;
						JSONNode jsonnode3 = (JSONNode)obj2;
						int asInt = jsonnode3[0].AsInt;
						float asFloat = jsonnode3[1].AsFloat;
						List<int> list;
						if (baseVertToUVVertFullMap.TryGetValue(asInt, out list))
						{
							foreach (int num2 in list)
							{
								DAZMeshVertexWeights dazmeshVertexWeights;
								if (dictionary.TryGetValue(num2, out dazmeshVertexWeights))
								{
									dazmeshVertexWeights.weight = asFloat;
									dictionary.Remove(num2);
									dictionary.Add(num2, dazmeshVertexWeights);
								}
								else
								{
									dazmeshVertexWeights = new DAZMeshVertexWeights();
									dazmeshVertexWeights.vertex = num2;
									dazmeshVertexWeights.weight = asFloat;
									dictionary.Add(num2, dazmeshVertexWeights);
								}
							}
						}
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator2 as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
				IEnumerator enumerator4 = jsonnode2["local_weights"]["x"]["values"].AsArray.GetEnumerator();
				try
				{
					while (enumerator4.MoveNext())
					{
						object obj3 = enumerator4.Current;
						JSONNode jsonnode4 = (JSONNode)obj3;
						int asInt2 = jsonnode4[0].AsInt;
						float asFloat2 = jsonnode4[1].AsFloat;
						List<int> list2;
						if (baseVertToUVVertFullMap.TryGetValue(asInt2, out list2))
						{
							foreach (int num3 in list2)
							{
								DAZMeshVertexWeights dazmeshVertexWeights2;
								if (dictionary.TryGetValue(num3, out dazmeshVertexWeights2))
								{
									dazmeshVertexWeights2.xweight = asFloat2;
									dictionary.Remove(num3);
									dictionary.Add(num3, dazmeshVertexWeights2);
								}
								else
								{
									dazmeshVertexWeights2 = new DAZMeshVertexWeights();
									dazmeshVertexWeights2.vertex = num3;
									dazmeshVertexWeights2.xweight = asFloat2;
									dictionary.Add(num3, dazmeshVertexWeights2);
								}
							}
						}
					}
				}
				finally
				{
					IDisposable disposable2;
					if ((disposable2 = (enumerator4 as IDisposable)) != null)
					{
						disposable2.Dispose();
					}
				}
				IEnumerator enumerator6 = jsonnode2["local_weights"]["y"]["values"].AsArray.GetEnumerator();
				try
				{
					while (enumerator6.MoveNext())
					{
						object obj4 = enumerator6.Current;
						JSONNode jsonnode5 = (JSONNode)obj4;
						int asInt3 = jsonnode5[0].AsInt;
						float asFloat3 = jsonnode5[1].AsFloat;
						List<int> list3;
						if (baseVertToUVVertFullMap.TryGetValue(asInt3, out list3))
						{
							foreach (int num4 in list3)
							{
								DAZMeshVertexWeights dazmeshVertexWeights3;
								if (dictionary.TryGetValue(num4, out dazmeshVertexWeights3))
								{
									dazmeshVertexWeights3.yweight = asFloat3;
									dictionary.Remove(num4);
									dictionary.Add(num4, dazmeshVertexWeights3);
								}
								else
								{
									dazmeshVertexWeights3 = new DAZMeshVertexWeights();
									dazmeshVertexWeights3.vertex = num4;
									dazmeshVertexWeights3.yweight = asFloat3;
									dictionary.Add(num4, dazmeshVertexWeights3);
								}
							}
						}
					}
				}
				finally
				{
					IDisposable disposable3;
					if ((disposable3 = (enumerator6 as IDisposable)) != null)
					{
						disposable3.Dispose();
					}
				}
				IEnumerator enumerator8 = jsonnode2["local_weights"]["z"]["values"].AsArray.GetEnumerator();
				try
				{
					while (enumerator8.MoveNext())
					{
						object obj5 = enumerator8.Current;
						JSONNode jsonnode6 = (JSONNode)obj5;
						int asInt4 = jsonnode6[0].AsInt;
						float asFloat4 = jsonnode6[1].AsFloat;
						List<int> list4;
						if (baseVertToUVVertFullMap.TryGetValue(asInt4, out list4))
						{
							foreach (int num5 in list4)
							{
								DAZMeshVertexWeights dazmeshVertexWeights4;
								if (dictionary.TryGetValue(num5, out dazmeshVertexWeights4))
								{
									dazmeshVertexWeights4.zweight = asFloat4;
									dictionary.Remove(num5);
									dictionary.Add(num5, dazmeshVertexWeights4);
								}
								else
								{
									dazmeshVertexWeights4 = new DAZMeshVertexWeights();
									dazmeshVertexWeights4.vertex = num5;
									dazmeshVertexWeights4.zweight = asFloat4;
									dictionary.Add(num5, dazmeshVertexWeights4);
								}
							}
						}
					}
				}
				finally
				{
					IDisposable disposable4;
					if ((disposable4 = (enumerator8 as IDisposable)) != null)
					{
						disposable4.Dispose();
					}
				}
				DAZBulgeFactors dazbulgeFactors = new DAZBulgeFactors();
				dazbulgeFactors.name = text;
				IEnumerator enumerator10 = jsonnode2["bulge_weights"]["x"]["bulges"].AsArray.GetEnumerator();
				try
				{
					while (enumerator10.MoveNext())
					{
						object obj6 = enumerator10.Current;
						JSONNode jsonnode7 = (JSONNode)obj6;
						string text2 = jsonnode7["id"];
						if (text2 != null)
						{
							if (!(text2 == "positive-left"))
							{
								if (!(text2 == "positive-right"))
								{
									if (!(text2 == "negative-left"))
									{
										if (text2 == "negative-right")
										{
											dazbulgeFactors.xnegright = -jsonnode7["value"].AsFloat;
										}
									}
									else
									{
										dazbulgeFactors.xnegleft = -jsonnode7["value"].AsFloat;
									}
								}
								else
								{
									dazbulgeFactors.xposright = jsonnode7["value"].AsFloat;
								}
							}
							else
							{
								dazbulgeFactors.xposleft = jsonnode7["value"].AsFloat;
							}
						}
					}
				}
				finally
				{
					IDisposable disposable5;
					if ((disposable5 = (enumerator10 as IDisposable)) != null)
					{
						disposable5.Dispose();
					}
				}
				IEnumerator enumerator11 = jsonnode2["bulge_weights"]["x"]["left_map"]["values"].AsArray.GetEnumerator();
				try
				{
					while (enumerator11.MoveNext())
					{
						object obj7 = enumerator11.Current;
						JSONNode jsonnode8 = (JSONNode)obj7;
						int asInt5 = jsonnode8[0].AsInt;
						float asFloat5 = jsonnode8[1].AsFloat;
						List<int> list5;
						if (baseVertToUVVertFullMap.TryGetValue(asInt5, out list5))
						{
							foreach (int num6 in list5)
							{
								DAZMeshVertexWeights dazmeshVertexWeights5;
								if (dictionary.TryGetValue(num6, out dazmeshVertexWeights5))
								{
									dazmeshVertexWeights5.xleftbulge = asFloat5;
									dictionary.Remove(num6);
									dictionary.Add(num6, dazmeshVertexWeights5);
								}
								else
								{
									dazmeshVertexWeights5 = new DAZMeshVertexWeights();
									dazmeshVertexWeights5.vertex = num6;
									dazmeshVertexWeights5.xleftbulge = asFloat5;
									dictionary.Add(num6, dazmeshVertexWeights5);
								}
							}
						}
					}
				}
				finally
				{
					IDisposable disposable6;
					if ((disposable6 = (enumerator11 as IDisposable)) != null)
					{
						disposable6.Dispose();
					}
				}
				IEnumerator enumerator13 = jsonnode2["bulge_weights"]["x"]["right_map"]["values"].AsArray.GetEnumerator();
				try
				{
					while (enumerator13.MoveNext())
					{
						object obj8 = enumerator13.Current;
						JSONNode jsonnode9 = (JSONNode)obj8;
						int asInt6 = jsonnode9[0].AsInt;
						float asFloat6 = jsonnode9[1].AsFloat;
						List<int> list6;
						if (baseVertToUVVertFullMap.TryGetValue(asInt6, out list6))
						{
							foreach (int num7 in list6)
							{
								DAZMeshVertexWeights dazmeshVertexWeights6;
								if (dictionary.TryGetValue(num7, out dazmeshVertexWeights6))
								{
									dazmeshVertexWeights6.xrightbulge = asFloat6;
									dictionary.Remove(num7);
									dictionary.Add(num7, dazmeshVertexWeights6);
								}
								else
								{
									dazmeshVertexWeights6 = new DAZMeshVertexWeights();
									dazmeshVertexWeights6.vertex = num7;
									dazmeshVertexWeights6.xrightbulge = asFloat6;
									dictionary.Add(num7, dazmeshVertexWeights6);
								}
							}
						}
					}
				}
				finally
				{
					IDisposable disposable7;
					if ((disposable7 = (enumerator13 as IDisposable)) != null)
					{
						disposable7.Dispose();
					}
				}
				IEnumerator enumerator15 = jsonnode2["bulge_weights"]["y"]["bulges"].AsArray.GetEnumerator();
				try
				{
					while (enumerator15.MoveNext())
					{
						object obj9 = enumerator15.Current;
						JSONNode jsonnode10 = (JSONNode)obj9;
						string text3 = jsonnode10["id"];
						if (text3 != null)
						{
							if (!(text3 == "positive-left"))
							{
								if (!(text3 == "positive-right"))
								{
									if (!(text3 == "negative-left"))
									{
										if (text3 == "negative-right")
										{
											dazbulgeFactors.yposright = jsonnode10["value"].AsFloat;
										}
									}
									else
									{
										dazbulgeFactors.yposleft = jsonnode10["value"].AsFloat;
									}
								}
								else
								{
									dazbulgeFactors.ynegright = -jsonnode10["value"].AsFloat;
								}
							}
							else
							{
								dazbulgeFactors.ynegleft = -jsonnode10["value"].AsFloat;
							}
						}
					}
				}
				finally
				{
					IDisposable disposable8;
					if ((disposable8 = (enumerator15 as IDisposable)) != null)
					{
						disposable8.Dispose();
					}
				}
				IEnumerator enumerator16 = jsonnode2["bulge_weights"]["y"]["left_map"]["values"].AsArray.GetEnumerator();
				try
				{
					while (enumerator16.MoveNext())
					{
						object obj10 = enumerator16.Current;
						JSONNode jsonnode11 = (JSONNode)obj10;
						int asInt7 = jsonnode11[0].AsInt;
						float asFloat7 = jsonnode11[1].AsFloat;
						List<int> list7;
						if (baseVertToUVVertFullMap.TryGetValue(asInt7, out list7))
						{
							foreach (int num8 in list7)
							{
								DAZMeshVertexWeights dazmeshVertexWeights7;
								if (dictionary.TryGetValue(num8, out dazmeshVertexWeights7))
								{
									dazmeshVertexWeights7.yleftbulge = asFloat7;
									dictionary.Remove(num8);
									dictionary.Add(num8, dazmeshVertexWeights7);
								}
								else
								{
									dazmeshVertexWeights7 = new DAZMeshVertexWeights();
									dazmeshVertexWeights7.vertex = num8;
									dazmeshVertexWeights7.yleftbulge = asFloat7;
									dictionary.Add(num8, dazmeshVertexWeights7);
								}
							}
						}
					}
				}
				finally
				{
					IDisposable disposable9;
					if ((disposable9 = (enumerator16 as IDisposable)) != null)
					{
						disposable9.Dispose();
					}
				}
				IEnumerator enumerator18 = jsonnode2["bulge_weights"]["y"]["right_map"]["values"].AsArray.GetEnumerator();
				try
				{
					while (enumerator18.MoveNext())
					{
						object obj11 = enumerator18.Current;
						JSONNode jsonnode12 = (JSONNode)obj11;
						int asInt8 = jsonnode12[0].AsInt;
						float asFloat8 = jsonnode12[1].AsFloat;
						List<int> list8;
						if (baseVertToUVVertFullMap.TryGetValue(asInt8, out list8))
						{
							foreach (int num9 in list8)
							{
								DAZMeshVertexWeights dazmeshVertexWeights8;
								if (dictionary.TryGetValue(num9, out dazmeshVertexWeights8))
								{
									dazmeshVertexWeights8.yrightbulge = asFloat8;
									dictionary.Remove(num9);
									dictionary.Add(num9, dazmeshVertexWeights8);
								}
								else
								{
									dazmeshVertexWeights8 = new DAZMeshVertexWeights();
									dazmeshVertexWeights8.vertex = num9;
									dazmeshVertexWeights8.yrightbulge = asFloat8;
									dictionary.Add(num9, dazmeshVertexWeights8);
								}
							}
						}
					}
				}
				finally
				{
					IDisposable disposable10;
					if ((disposable10 = (enumerator18 as IDisposable)) != null)
					{
						disposable10.Dispose();
					}
				}
				IEnumerator enumerator20 = jsonnode2["bulge_weights"]["z"]["bulges"].AsArray.GetEnumerator();
				try
				{
					while (enumerator20.MoveNext())
					{
						object obj12 = enumerator20.Current;
						JSONNode jsonnode13 = (JSONNode)obj12;
						string text4 = jsonnode13["id"];
						if (text4 != null)
						{
							if (!(text4 == "positive-left"))
							{
								if (!(text4 == "positive-right"))
								{
									if (!(text4 == "negative-left"))
									{
										if (text4 == "negative-right")
										{
											dazbulgeFactors.zposright = jsonnode13["value"].AsFloat;
										}
									}
									else
									{
										dazbulgeFactors.zposleft = jsonnode13["value"].AsFloat;
									}
								}
								else
								{
									dazbulgeFactors.znegright = -jsonnode13["value"].AsFloat;
								}
							}
							else
							{
								dazbulgeFactors.znegleft = -jsonnode13["value"].AsFloat;
							}
						}
					}
				}
				finally
				{
					IDisposable disposable11;
					if ((disposable11 = (enumerator20 as IDisposable)) != null)
					{
						disposable11.Dispose();
					}
				}
				IEnumerator enumerator21 = jsonnode2["bulge_weights"]["z"]["left_map"]["values"].AsArray.GetEnumerator();
				try
				{
					while (enumerator21.MoveNext())
					{
						object obj13 = enumerator21.Current;
						JSONNode jsonnode14 = (JSONNode)obj13;
						int asInt9 = jsonnode14[0].AsInt;
						float asFloat9 = jsonnode14[1].AsFloat;
						List<int> list9;
						if (baseVertToUVVertFullMap.TryGetValue(asInt9, out list9))
						{
							foreach (int num10 in list9)
							{
								DAZMeshVertexWeights dazmeshVertexWeights9;
								if (dictionary.TryGetValue(num10, out dazmeshVertexWeights9))
								{
									dazmeshVertexWeights9.zleftbulge = asFloat9;
									dictionary.Remove(num10);
									dictionary.Add(num10, dazmeshVertexWeights9);
								}
								else
								{
									dazmeshVertexWeights9 = new DAZMeshVertexWeights();
									dazmeshVertexWeights9.vertex = num10;
									dazmeshVertexWeights9.zleftbulge = asFloat9;
									dictionary.Add(num10, dazmeshVertexWeights9);
								}
							}
						}
					}
				}
				finally
				{
					IDisposable disposable12;
					if ((disposable12 = (enumerator21 as IDisposable)) != null)
					{
						disposable12.Dispose();
					}
				}
				IEnumerator enumerator23 = jsonnode2["bulge_weights"]["z"]["right_map"]["values"].AsArray.GetEnumerator();
				try
				{
					while (enumerator23.MoveNext())
					{
						object obj14 = enumerator23.Current;
						JSONNode jsonnode15 = (JSONNode)obj14;
						int asInt10 = jsonnode15[0].AsInt;
						float asFloat10 = jsonnode15[1].AsFloat;
						List<int> list10;
						if (baseVertToUVVertFullMap.TryGetValue(asInt10, out list10))
						{
							foreach (int num11 in list10)
							{
								DAZMeshVertexWeights dazmeshVertexWeights10;
								if (dictionary.TryGetValue(num11, out dazmeshVertexWeights10))
								{
									dazmeshVertexWeights10.zrightbulge = asFloat10;
									dictionary.Remove(num11);
									dictionary.Add(num11, dazmeshVertexWeights10);
								}
								else
								{
									dazmeshVertexWeights10 = new DAZMeshVertexWeights();
									dazmeshVertexWeights10.vertex = num11;
									dazmeshVertexWeights10.zrightbulge = asFloat10;
									dictionary.Add(num11, dazmeshVertexWeights10);
								}
							}
						}
					}
				}
				finally
				{
					IDisposable disposable13;
					if ((disposable13 = (enumerator23 as IDisposable)) != null)
					{
						disposable13.Dispose();
					}
				}
				this.nodes[num].bulgeFactors = dazbulgeFactors;
				this.boneWeightsMap.Add(text, dictionary);
				num++;
			}
		}
		finally
		{
			IDisposable disposable14;
			if ((disposable14 = (enumerator as IDisposable)) != null)
			{
				disposable14.Dispose();
			}
		}
		this.WalkBonesAndAccumulateWeights(this.rootBone);
		this.CreateBoneWeightsArray();
	}

	// Token: 0x06004C91 RID: 19601 RVA: 0x001893C4 File Offset: 0x001877C4
	protected void InitBones()
	{
		this.boneTransforms = new Transform[this.numBones];
		this.dazBones = new DAZBone[this.numBones];
		this.boneMatrices = new Matrix4x4[this.numBones];
		this.boneRotationAngles = new Vector3[this.numBones];
		this.startingMatrices = new Matrix4x4[this.numBones];
		this.startingMatricesInverted = new Matrix4x4[this.numBones];
		Dictionary<string, Transform> dictionary = new Dictionary<string, Transform>();
		Dictionary<string, Matrix4x4> dictionary2 = new Dictionary<string, Matrix4x4>();
		if (this.rootBone)
		{
			foreach (Transform transform in this.rootBone.GetComponentsInChildren<Transform>())
			{
				if (!dictionary.ContainsKey(transform.name))
				{
					dictionary.Add(transform.name, transform);
					dictionary2.Add(transform.name, transform.worldToLocalMatrix * base.transform.localToWorldMatrix);
				}
			}
		}
		for (int j = 0; j < this.numBones; j++)
		{
			string name = this.nodes[j].name;
			Transform transform2;
			if (dictionary.TryGetValue(name, out transform2))
			{
				this.boneTransforms[j] = transform2;
				DAZBone component = transform2.GetComponent<DAZBone>();
				this.dazBones[j] = component;
			}
			else
			{
				UnityEngine.Debug.LogError("Could not find transform for bone " + name);
			}
			Matrix4x4 matrix4x;
			if (dictionary2.TryGetValue(name, out matrix4x))
			{
				this.startingMatrices[j] = matrix4x;
				this.startingMatricesInverted[j] = Matrix4x4.Inverse(matrix4x);
			}
			else
			{
				UnityEngine.Debug.LogError("Could not find transform for bone " + name);
			}
		}
	}

	// Token: 0x06004C92 RID: 19602 RVA: 0x0018957D File Offset: 0x0018797D
	protected void InitSmoothing()
	{
		if (this.meshSmooth == null)
		{
			this.meshSmooth = new MeshSmooth(this.dazMesh.baseVertices, this.dazMesh.basePolyList);
		}
	}

	// Token: 0x06004C93 RID: 19603 RVA: 0x001895AC File Offset: 0x001879AC
	protected void InitMesh()
	{
		if (this.dazMesh != null)
		{
			this.mesh = UnityEngine.Object.Instantiate<Mesh>(this.dazMesh.morphedUVMappedMesh);
			this.startVerts = this.dazMesh.morphedUVVertices;
			this.strongestBone = new int[this.startVerts.Length];
			this.strongestBoneWeight = new float[this.startVerts.Length];
			for (int i = 0; i < this.numBones; i++)
			{
				foreach (DAZMeshVertexWeights dazmeshVertexWeights in this.nodes[i].weights)
				{
					if (dazmeshVertexWeights.weight > this.strongestBoneWeight[dazmeshVertexWeights.vertex])
					{
						this.strongestBoneWeight[dazmeshVertexWeights.vertex] = dazmeshVertexWeights.weight;
						this.strongestBone[dazmeshVertexWeights.vertex] = i;
					}
				}
			}
			this.smoothedVerts = (Vector3[])this.startVerts.Clone();
			this.drawVerts = new Vector3[this.startVerts.Length];
			this.startNormals = this.dazMesh.morphedUVNormals;
			this.drawNormals = new Vector3[this.startNormals.Length];
			this.startTangents = this.dazMesh.morphedUVTangents;
			this.drawTangents = new Vector4[this.startTangents.Length];
			Vector3 size = new Vector3(10000f, 10000f, 10000f);
			Bounds bounds = new Bounds(base.transform.position, size);
			this.mesh.bounds = bounds;
		}
		else
		{
			UnityEngine.Debug.LogError("Could not find mesh matching geometryId: " + this.geometryId);
		}
	}

	// Token: 0x17000ADA RID: 2778
	// (get) Token: 0x06004C94 RID: 19604 RVA: 0x0018974F File Offset: 0x00187B4F
	// (set) Token: 0x06004C95 RID: 19605 RVA: 0x00189758 File Offset: 0x00187B58
	public int numSubThreads
	{
		get
		{
			return this._numSubThreads;
		}
		set
		{
			if (this._numSubThreads != value)
			{
				bool threadsRunning = this._threadsRunning;
				if (threadsRunning)
				{
					this.StopThreads();
				}
				this._numSubThreads = value;
				this.InitSkinTimes();
				if (threadsRunning)
				{
					this.StartThreads();
				}
			}
		}
	}

	// Token: 0x06004C96 RID: 19606 RVA: 0x0018979D File Offset: 0x00187B9D
	public void SetNumSubThreads(int num)
	{
		this.numSubThreads = num;
	}

	// Token: 0x06004C97 RID: 19607 RVA: 0x001897A8 File Offset: 0x00187BA8
	public void InitSkinTimes()
	{
		this.threadSkinTime = new float[this._numSubThreads];
		this.threadSkinStartTime = new float[this._numSubThreads];
		this.threadSkinStopTime = new float[this._numSubThreads];
		this.threadSmoothTime = new float[this._numSubThreads];
		this.threadSmoothStartTime = new float[this._numSubThreads];
		this.threadSmoothStopTime = new float[this._numSubThreads];
		this.threadSmoothCorrectionTime = new float[this._numSubThreads];
		this.threadSmoothCorrectionStartTime = new float[this._numSubThreads];
		this.threadSmoothCorrectionStopTime = new float[this._numSubThreads];
		this.threadRenormalizeTime = new float[this._numSubThreads];
		this.threadRenormalizeStartTime = new float[this._numSubThreads];
		this.threadRenormalizeStopTime = new float[this._numSubThreads];
	}

	// Token: 0x17000ADB RID: 2779
	// (get) Token: 0x06004C98 RID: 19608 RVA: 0x00189881 File Offset: 0x00187C81
	public bool threadsRunning
	{
		get
		{
			return this._threadsRunning;
		}
	}

	// Token: 0x06004C99 RID: 19609 RVA: 0x0018988C File Offset: 0x00187C8C
	protected void StopThreads()
	{
		this._threadsRunning = false;
		if (this.tasks != null)
		{
			for (int i = 0; i < this.tasks.Length; i++)
			{
				this.tasks[i].resetEvent.Set();
				while (this.tasks[i].thread.IsAlive)
				{
				}
			}
		}
		this.tasks = null;
	}

	// Token: 0x06004C9A RID: 19610 RVA: 0x001898FC File Offset: 0x00187CFC
	protected void StartThreads()
	{
		this._threadsRunning = true;
		if (this._numSubThreads > 0)
		{
			this.tasks = new DAZSkinTaskInfo[this._numSubThreads];
			for (int i = 0; i < this._numSubThreads; i++)
			{
				DAZSkinTaskInfo dazskinTaskInfo = new DAZSkinTaskInfo();
				dazskinTaskInfo.threadIndex = i;
				dazskinTaskInfo.name = "Task" + i;
				dazskinTaskInfo.resetEvent = new AutoResetEvent(false);
				dazskinTaskInfo.thread = new Thread(new ParameterizedThreadStart(this.MTTask));
				dazskinTaskInfo.thread.Start(dazskinTaskInfo);
				this.tasks[i] = dazskinTaskInfo;
			}
		}
	}

	// Token: 0x06004C9B RID: 19611 RVA: 0x0018999F File Offset: 0x00187D9F
	protected void OnApplicationQuit()
	{
		if (Application.isPlaying)
		{
			this.StopThreads();
		}
	}

	// Token: 0x06004C9C RID: 19612 RVA: 0x001899B4 File Offset: 0x00187DB4
	protected void MTTask(object info)
	{
		DAZSkinTaskInfo dazskinTaskInfo = (DAZSkinTaskInfo)info;
		while (this._threadsRunning)
		{
			dazskinTaskInfo.resetEvent.WaitOne(-1, false);
			float num = 1000f / (float)Stopwatch.Frequency;
			if (dazskinTaskInfo.taskType == DAZSkinTaskType.Skin)
			{
				this.threadSkinStartTime[dazskinTaskInfo.threadIndex] = (float)this.stopwatch.ElapsedTicks * num;
				this.SkinMeshPart(dazskinTaskInfo.index1, dazskinTaskInfo.index2);
				this.threadSkinStopTime[dazskinTaskInfo.threadIndex] = (float)this.stopwatch.ElapsedTicks * num;
				this.threadSkinTime[dazskinTaskInfo.threadIndex] = this.threadSkinStopTime[dazskinTaskInfo.threadIndex] - this.threadSkinStartTime[dazskinTaskInfo.threadIndex];
			}
			else if (dazskinTaskInfo.taskType == DAZSkinTaskType.Smooth)
			{
				this.threadSmoothStartTime[dazskinTaskInfo.threadIndex] = (float)this.stopwatch.ElapsedTicks * num;
				this.meshSmooth.LaplacianSmooth(this.drawVerts, this.smoothedVerts, dazskinTaskInfo.index1, dazskinTaskInfo.index2);
				this.threadSmoothStopTime[dazskinTaskInfo.threadIndex] = (float)this.stopwatch.ElapsedTicks * num;
				this.threadSmoothTime[dazskinTaskInfo.threadIndex] = this.threadSmoothStopTime[dazskinTaskInfo.threadIndex] - this.threadSmoothStartTime[dazskinTaskInfo.threadIndex];
			}
			else if (dazskinTaskInfo.taskType == DAZSkinTaskType.SmoothCorrection)
			{
				this.threadSmoothCorrectionStartTime[dazskinTaskInfo.threadIndex] = (float)this.stopwatch.ElapsedTicks * num;
				this.meshSmooth.HCCorrection(this.drawVerts, this.smoothedVerts, this.smoothCorrectionBeta, dazskinTaskInfo.index1, dazskinTaskInfo.index2);
				this.threadSmoothCorrectionStopTime[dazskinTaskInfo.threadIndex] = (float)this.stopwatch.ElapsedTicks * num;
				this.threadSmoothCorrectionTime[dazskinTaskInfo.threadIndex] = this.threadSmoothCorrectionStopTime[dazskinTaskInfo.threadIndex] - this.threadSmoothCorrectionStartTime[dazskinTaskInfo.threadIndex];
			}
			else if (dazskinTaskInfo.taskType == DAZSkinTaskType.Renormalize)
			{
				this.threadRenormalizeStartTime[dazskinTaskInfo.threadIndex] = (float)this.stopwatch.ElapsedTicks * num;
				this.RenormalizePart(dazskinTaskInfo.index1, dazskinTaskInfo.index2);
				this.threadRenormalizeStopTime[dazskinTaskInfo.threadIndex] = (float)this.stopwatch.ElapsedTicks * num;
				this.threadRenormalizeTime[dazskinTaskInfo.threadIndex] = this.threadRenormalizeStopTime[dazskinTaskInfo.threadIndex] - this.threadRenormalizeStartTime[dazskinTaskInfo.threadIndex];
			}
			dazskinTaskInfo.working = false;
		}
	}

	// Token: 0x06004C9D RID: 19613 RVA: 0x00189C24 File Offset: 0x00188024
	protected void SkinMesh()
	{
		if (this.mesh != null)
		{
			if (this._useGeneralWeights)
			{
				for (int i = 0; i < this.drawVerts.Length; i++)
				{
					this.drawVerts[i].x = 0f;
					this.drawVerts[i].y = 0f;
					this.drawVerts[i].z = 0f;
					this.drawNormals[i].x = 0f;
					this.drawNormals[i].y = 0f;
					this.drawNormals[i].z = 0f;
					this.drawTangents[i].x = 0f;
					this.drawTangents[i].y = 0f;
					this.drawTangents[i].z = 0f;
					this.drawTangents[i].w = this.startTangents[i].w;
				}
			}
			else
			{
				for (int j = 0; j < this.drawVerts.Length; j++)
				{
					this.drawVerts[j] = this.startVerts[j];
					this.drawNormals[j] = this.startNormals[j];
					this.drawTangents[j] = this.startTangents[j];
				}
			}
			for (int k = 0; k < this.numBones; k++)
			{
				Transform transform = this.boneTransforms[k];
				this.boneMatrices[k] = transform.localToWorldMatrix * this.startingMatrices[k];
				if (!this._useGeneralWeights)
				{
					DAZBone dazbone = this.dazBones[k];
					if (dazbone == null)
					{
						this.boneRotationAngles[k] = Quaternion2Angles.GetAngles(transform.localRotation, this.nodes[k].rotationOrder);
					}
					else
					{
						this.boneRotationAngles[k] = dazbone.GetAngles();
					}
				}
			}
			if (this.tasks == null)
			{
				this.StartThreads();
			}
			int num = this.drawVerts.Length;
			float num2 = 1000f / (float)Stopwatch.Frequency;
			if (this._numSubThreads > 0)
			{
				int num3 = num / this._numSubThreads;
				for (int l = 0; l < this._numSubThreads; l++)
				{
					this.tasks[l].taskType = DAZSkinTaskType.Skin;
					this.tasks[l].index1 = l * num3;
					if (l == this._numSubThreads - 1)
					{
						this.tasks[l].index2 = num - 1;
					}
					else
					{
						this.tasks[l].index2 = (l + 1) * num3 - 1;
					}
					this.tasks[l].working = true;
					this.tasks[l].resetEvent.Set();
				}
				bool flag;
				do
				{
					flag = false;
					for (int m = 0; m < this._numSubThreads; m++)
					{
						if (this.tasks[m].working)
						{
							flag = true;
						}
					}
					if (flag)
					{
						Thread.Sleep(0);
					}
				}
				while (flag);
			}
			else
			{
				this.mainThreadSkinStartTime = (float)this.stopwatch.ElapsedTicks * num2;
				this.SkinMeshPart(0, num - 1);
				this.mainThreadSkinStopTime = (float)this.stopwatch.ElapsedTicks * num2;
				this.mainThreadSkinTime = this.mainThreadSkinStopTime - this.mainThreadSkinStartTime;
			}
			if (this.useSmoothing)
			{
				this.InitSmoothing();
				if (this._numSubThreads > 0)
				{
					int num4 = this.dazMesh.baseVertices.Length;
					int num5 = num4 / this._numSubThreads;
					for (int n = 0; n < this._numSubThreads; n++)
					{
						this.tasks[n].taskType = DAZSkinTaskType.Smooth;
						this.tasks[n].index1 = n * num5;
						if (n == this._numSubThreads - 1)
						{
							this.tasks[n].index2 = num4 - 1;
						}
						else
						{
							this.tasks[n].index2 = (n + 1) * num5 - 1;
						}
						this.tasks[n].working = true;
						this.tasks[n].resetEvent.Set();
					}
					bool flag2;
					do
					{
						flag2 = false;
						for (int num6 = 0; num6 < this._numSubThreads; num6++)
						{
							if (this.tasks[num6].working)
							{
								flag2 = true;
							}
						}
						if (flag2)
						{
							Thread.Sleep(0);
						}
					}
					while (flag2);
					for (int num7 = 0; num7 < this._numSubThreads; num7++)
					{
						this.tasks[num7].taskType = DAZSkinTaskType.SmoothCorrection;
						this.tasks[num7].index1 = num7 * num5;
						if (num7 == this._numSubThreads - 1)
						{
							this.tasks[num7].index2 = num4 - 1;
						}
						else
						{
							this.tasks[num7].index2 = (num7 + 1) * num5 - 1;
						}
						this.tasks[num7].working = true;
						this.tasks[num7].resetEvent.Set();
					}
					do
					{
						flag2 = false;
						for (int num8 = 0; num8 < this._numSubThreads; num8++)
						{
							if (this.tasks[num8].working)
							{
								flag2 = true;
							}
						}
						if (flag2)
						{
							Thread.Sleep(0);
						}
					}
					while (flag2);
					foreach (DAZVertexMap dazvertexMap in this.dazMesh.baseVerticesToUVVertices)
					{
						this.smoothedVerts[dazvertexMap.tovert] = this.smoothedVerts[dazvertexMap.fromvert];
					}
					this.mesh.vertices = this.smoothedVerts;
				}
				else
				{
					this.mainThreadSmoothStartTime = (float)this.stopwatch.ElapsedTicks * num2;
					this.meshSmooth.LaplacianSmooth(this.drawVerts, this.smoothedVerts, 0, 100000000);
					this.mainThreadSmoothStopTime = (float)this.stopwatch.ElapsedTicks * num2;
					this.mainThreadSmoothTime = this.mainThreadSmoothStopTime - this.mainThreadSmoothStartTime;
					this.mainThreadSmoothCorrectionStartTime = (float)this.stopwatch.ElapsedTicks * num2;
					this.meshSmooth.HCCorrection(this.drawVerts, this.smoothedVerts, 0.5f, 0, 1000000000);
					this.mainThreadSmoothCorrectionStopTime = (float)this.stopwatch.ElapsedTicks * num2;
					this.mainThreadSmoothCorrectionTime = this.mainThreadSmoothCorrectionStopTime - this.mainThreadSmoothCorrectionStartTime;
					foreach (DAZVertexMap dazvertexMap2 in this.dazMesh.baseVerticesToUVVertices)
					{
						this.smoothedVerts[dazvertexMap2.tovert] = this.smoothedVerts[dazvertexMap2.fromvert];
					}
					this.mesh.vertices = this.smoothedVerts;
				}
			}
			else
			{
				foreach (DAZVertexMap dazvertexMap3 in this.dazMesh.baseVerticesToUVVertices)
				{
					this.drawVerts[dazvertexMap3.tovert] = this.drawVerts[dazvertexMap3.fromvert];
				}
				this.mesh.vertices = this.drawVerts;
			}
			if (this._useGeneralWeights && !this._useFastNormals && this._renormalize)
			{
				if (this._numSubThreads > 0)
				{
					int num12 = num / this._numSubThreads;
					for (int num13 = 0; num13 < this._numSubThreads; num13++)
					{
						this.tasks[num13].taskType = DAZSkinTaskType.Renormalize;
						this.tasks[num13].index1 = num13 * num12;
						if (num13 == this._numSubThreads - 1)
						{
							this.tasks[num13].index2 = num - 1;
						}
						else
						{
							this.tasks[num13].index2 = (num13 + 1) * num12 - 1;
						}
						this.tasks[num13].working = true;
						this.tasks[num13].resetEvent.Set();
					}
					bool flag3;
					do
					{
						flag3 = false;
						for (int num14 = 0; num14 < this._numSubThreads; num14++)
						{
							if (this.tasks[num14].working)
							{
								flag3 = true;
							}
						}
						if (flag3)
						{
							Thread.Sleep(0);
						}
					}
					while (flag3);
				}
				else
				{
					this.mainThreadRenormalizeStartTime = (float)this.stopwatch.ElapsedTicks * num2;
					this.Renormalize();
					this.mainThreadRenormalizeStopTime = (float)this.stopwatch.ElapsedTicks * num2;
					this.mainThreadRenormalizeTime = this.mainThreadRenormalizeStopTime - this.mainThreadRenormalizeStartTime;
				}
			}
			this.mesh.normals = this.drawNormals;
			this.mesh.tangents = this.drawTangents;
		}
	}

	// Token: 0x06004C9E RID: 19614 RVA: 0x0018A598 File Offset: 0x00188998
	protected void RenormalizePart(int startIndex, int stopIndex)
	{
		for (int i = startIndex; i <= stopIndex; i++)
		{
			float num = 1f / Mathf.Sqrt(this.drawNormals[i].x * this.drawNormals[i].x + this.drawNormals[i].y * this.drawNormals[i].y + this.drawNormals[i].z * this.drawNormals[i].z);
			Vector3[] array = this.drawNormals;
			int num2 = i;
			array[num2].x = array[num2].x * num;
			Vector3[] array2 = this.drawNormals;
			int num3 = i;
			array2[num3].y = array2[num3].y * num;
			Vector3[] array3 = this.drawNormals;
			int num4 = i;
			array3[num4].z = array3[num4].z * num;
			num = 1f / Mathf.Sqrt(this.drawTangents[i].x * this.drawTangents[i].x + this.drawTangents[i].y * this.drawTangents[i].y + this.drawTangents[i].z * this.drawTangents[i].z);
			Vector4[] array4 = this.drawTangents;
			int num5 = i;
			array4[num5].x = array4[num5].x * num;
			Vector4[] array5 = this.drawTangents;
			int num6 = i;
			array5[num6].y = array5[num6].y * num;
			Vector4[] array6 = this.drawTangents;
			int num7 = i;
			array6[num7].z = array6[num7].z * num;
		}
	}

	// Token: 0x06004C9F RID: 19615 RVA: 0x0018A73B File Offset: 0x00188B3B
	protected void Renormalize()
	{
		this.RenormalizePart(0, this.drawNormals.Length - 1);
	}

	// Token: 0x06004CA0 RID: 19616 RVA: 0x0018A750 File Offset: 0x00188B50
	protected void SkinMeshPart(int startIndex, int stopIndex)
	{
		for (int i = 0; i < this.numBones; i++)
		{
			DAZMeshVertexWeights[] weights = this.nodes[i].weights;
			float m = this.boneMatrices[i].m00;
			float m2 = this.boneMatrices[i].m01;
			float m3 = this.boneMatrices[i].m02;
			float m4 = this.boneMatrices[i].m03;
			float m5 = this.boneMatrices[i].m10;
			float m6 = this.boneMatrices[i].m11;
			float m7 = this.boneMatrices[i].m12;
			float m8 = this.boneMatrices[i].m13;
			float m9 = this.boneMatrices[i].m20;
			float m10 = this.boneMatrices[i].m21;
			float m11 = this.boneMatrices[i].m22;
			float m12 = this.boneMatrices[i].m23;
			if (this._useGeneralWeights)
			{
				foreach (DAZMeshVertexWeights dazmeshVertexWeights in weights)
				{
					if (dazmeshVertexWeights.vertex >= startIndex && dazmeshVertexWeights.vertex <= stopIndex)
					{
						Vector3 vector = this.startVerts[dazmeshVertexWeights.vertex];
						Vector3[] array = this.drawVerts;
						int vertex = dazmeshVertexWeights.vertex;
						array[vertex].x = array[vertex].x + (vector.x * m + vector.y * m2 + vector.z * m3 + m4) * dazmeshVertexWeights.weight;
						Vector3[] array2 = this.drawVerts;
						int vertex2 = dazmeshVertexWeights.vertex;
						array2[vertex2].y = array2[vertex2].y + (vector.x * m5 + vector.y * m6 + vector.z * m7 + m8) * dazmeshVertexWeights.weight;
						Vector3[] array3 = this.drawVerts;
						int vertex3 = dazmeshVertexWeights.vertex;
						array3[vertex3].z = array3[vertex3].z + (vector.x * m9 + vector.y * m10 + vector.z * m11 + m12) * dazmeshVertexWeights.weight;
						if (this._useFastNormals)
						{
							if (this.strongestBone[dazmeshVertexWeights.vertex] == i)
							{
								Vector3 vector2 = this.startNormals[dazmeshVertexWeights.vertex];
								this.drawNormals[dazmeshVertexWeights.vertex].x = vector2.x * m + vector2.y * m2 + vector2.z * m3;
								this.drawNormals[dazmeshVertexWeights.vertex].y = vector2.x * m5 + vector2.y * m6 + vector2.z * m7;
								this.drawNormals[dazmeshVertexWeights.vertex].z = vector2.x * m9 + vector2.y * m10 + vector2.z * m11;
								Vector4 vector3 = this.startTangents[dazmeshVertexWeights.vertex];
								this.drawTangents[dazmeshVertexWeights.vertex].x = vector3.x * m + vector3.y * m2 + vector3.z * m3;
								this.drawTangents[dazmeshVertexWeights.vertex].y = vector3.x * m5 + vector3.y * m6 + vector3.z * m7;
								this.drawTangents[dazmeshVertexWeights.vertex].z = vector3.x * m9 + vector3.y * m10 + vector3.z * m11;
							}
						}
						else
						{
							Vector3 vector4 = this.startNormals[dazmeshVertexWeights.vertex];
							Vector3[] array4 = this.drawNormals;
							int vertex4 = dazmeshVertexWeights.vertex;
							array4[vertex4].x = array4[vertex4].x + (vector4.x * m + vector4.y * m2 + vector4.z * m3) * dazmeshVertexWeights.weight;
							Vector3[] array5 = this.drawNormals;
							int vertex5 = dazmeshVertexWeights.vertex;
							array5[vertex5].y = array5[vertex5].y + (vector4.x * m5 + vector4.y * m6 + vector4.z * m7) * dazmeshVertexWeights.weight;
							Vector3[] array6 = this.drawNormals;
							int vertex6 = dazmeshVertexWeights.vertex;
							array6[vertex6].z = array6[vertex6].z + (vector4.x * m9 + vector4.y * m10 + vector4.z * m11) * dazmeshVertexWeights.weight;
							Vector4 vector5 = this.startTangents[dazmeshVertexWeights.vertex];
							Vector4[] array7 = this.drawTangents;
							int vertex7 = dazmeshVertexWeights.vertex;
							array7[vertex7].x = array7[vertex7].x + (vector5.x * m + vector5.y * m2 + vector5.z * m3) * dazmeshVertexWeights.weight;
							Vector4[] array8 = this.drawTangents;
							int vertex8 = dazmeshVertexWeights.vertex;
							array8[vertex8].y = array8[vertex8].y + (vector5.x * m5 + vector5.y * m6 + vector5.z * m7) * dazmeshVertexWeights.weight;
							Vector4[] array9 = this.drawTangents;
							int vertex9 = dazmeshVertexWeights.vertex;
							array9[vertex9].z = array9[vertex9].z + (vector5.x * m9 + vector5.y * m10 + vector5.z * m11) * dazmeshVertexWeights.weight;
						}
					}
				}
			}
			else
			{
				DAZBulgeFactors bulgeFactors = this.nodes[i].bulgeFactors;
				Quaternion2Angles.RotationOrder rotationOrder = this.nodes[i].rotationOrder;
				Matrix4x4 matrix4x = this.startingMatrices[i];
				float m13 = matrix4x.m00;
				float m14 = matrix4x.m01;
				float m15 = matrix4x.m02;
				float m16 = matrix4x.m03;
				float m17 = matrix4x.m10;
				float m18 = matrix4x.m11;
				float m19 = matrix4x.m12;
				float m20 = matrix4x.m13;
				float m21 = matrix4x.m20;
				float m22 = matrix4x.m21;
				float m23 = matrix4x.m22;
				float m24 = matrix4x.m23;
				Matrix4x4 matrix4x2 = this.startingMatricesInverted[i];
				float m25 = matrix4x2.m00;
				float m26 = matrix4x2.m01;
				float m27 = matrix4x2.m02;
				float m28 = matrix4x2.m03;
				float m29 = matrix4x2.m10;
				float m30 = matrix4x2.m11;
				float m31 = matrix4x2.m12;
				float m32 = matrix4x2.m13;
				float m33 = matrix4x2.m20;
				float m34 = matrix4x2.m21;
				float m35 = matrix4x2.m22;
				float m36 = matrix4x2.m23;
				Vector3 vector6 = this.boneRotationAngles[i];
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				bool flag4 = false;
				bool flag5 = false;
				bool flag6 = false;
				bool flag7 = false;
				bool flag8 = false;
				bool flag9 = false;
				bool flag10 = false;
				bool flag11 = false;
				bool flag12 = false;
				bool flag13 = false;
				bool flag14 = false;
				bool flag15 = false;
				float num = 0f;
				float num2 = 0f;
				float num3 = 0f;
				float num4 = 0f;
				float num5 = 0f;
				float num6 = 0f;
				float num7 = 0f;
				float num8 = 0f;
				float num9 = 0f;
				float num10 = 0f;
				float num11 = 0f;
				float num12 = 0f;
				if (vector6.x > 0.01f)
				{
					if (bulgeFactors.xposleft != 0f)
					{
						flag = true;
						flag2 = true;
						num = bulgeFactors.xposleft * vector6.x * this.bulgeScale;
					}
					if (bulgeFactors.xposright != 0f)
					{
						flag = true;
						flag4 = true;
						num3 = bulgeFactors.xposright * vector6.x * this.bulgeScale;
					}
				}
				else if (vector6.x < -0.01f)
				{
					if (bulgeFactors.xnegleft != 0f)
					{
						flag = true;
						flag3 = true;
						num2 = bulgeFactors.xnegleft * vector6.x * this.bulgeScale;
					}
					if (bulgeFactors.xnegright != 0f)
					{
						flag = true;
						flag5 = true;
						num4 = bulgeFactors.xnegright * vector6.x * this.bulgeScale;
					}
				}
				if (vector6.y > 0.01f)
				{
					if (bulgeFactors.yposleft != 0f)
					{
						flag6 = true;
						flag7 = true;
						num5 = bulgeFactors.yposleft * vector6.y * this.bulgeScale;
					}
					if (bulgeFactors.yposright != 0f)
					{
						flag6 = true;
						flag9 = true;
						num7 = bulgeFactors.yposright * vector6.y * this.bulgeScale;
					}
				}
				else if (vector6.y < -0.01f)
				{
					if (bulgeFactors.ynegleft != 0f)
					{
						flag6 = true;
						flag8 = true;
						num6 = bulgeFactors.ynegleft * vector6.y * this.bulgeScale;
					}
					if (bulgeFactors.ynegright != 0f)
					{
						flag6 = true;
						flag10 = true;
						num8 = bulgeFactors.ynegright * vector6.y * this.bulgeScale;
					}
				}
				if (vector6.z > 0.01f)
				{
					if (bulgeFactors.zposleft != 0f)
					{
						flag11 = true;
						flag12 = true;
						num9 = bulgeFactors.zposleft * vector6.z * this.bulgeScale;
					}
					if (bulgeFactors.zposright != 0f)
					{
						flag11 = true;
						flag14 = true;
						num11 = bulgeFactors.zposright * vector6.z * this.bulgeScale;
					}
				}
				else if (vector6.z < -0.01f)
				{
					if (bulgeFactors.znegleft != 0f)
					{
						flag11 = true;
						flag13 = true;
						num10 = bulgeFactors.znegleft * vector6.z * this.bulgeScale;
					}
					if (bulgeFactors.znegright != 0f)
					{
						flag11 = true;
						flag15 = true;
						num12 = bulgeFactors.znegright * vector6.z * this.bulgeScale;
					}
				}
				if (rotationOrder == Quaternion2Angles.RotationOrder.XYZ)
				{
					foreach (DAZMeshVertexWeights dazmeshVertexWeights2 in weights)
					{
						if (dazmeshVertexWeights2.vertex >= startIndex && dazmeshVertexWeights2.vertex <= stopIndex)
						{
							if (dazmeshVertexWeights2.xweight > 0.99999f && dazmeshVertexWeights2.yweight > 0.99999f && dazmeshVertexWeights2.zweight > 0.99999f)
							{
								Vector3 vector7 = this.drawVerts[dazmeshVertexWeights2.vertex];
								this.drawVerts[dazmeshVertexWeights2.vertex].x = vector7.x * m + vector7.y * m2 + vector7.z * m3 + m4;
								this.drawVerts[dazmeshVertexWeights2.vertex].y = vector7.x * m5 + vector7.y * m6 + vector7.z * m7 + m8;
								this.drawVerts[dazmeshVertexWeights2.vertex].z = vector7.x * m9 + vector7.y * m10 + vector7.z * m11 + m12;
								Vector3 vector8 = this.drawNormals[dazmeshVertexWeights2.vertex];
								this.drawNormals[dazmeshVertexWeights2.vertex].x = vector8.x * m + vector8.y * m2 + vector8.z * m3;
								this.drawNormals[dazmeshVertexWeights2.vertex].y = vector8.x * m5 + vector8.y * m6 + vector8.z * m7;
								this.drawNormals[dazmeshVertexWeights2.vertex].z = vector8.x * m9 + vector8.y * m10 + vector8.z * m11;
								Vector4 vector9 = this.drawTangents[dazmeshVertexWeights2.vertex];
								this.drawTangents[dazmeshVertexWeights2.vertex].x = vector9.x * m + vector9.y * m2 + vector9.z * m3;
								this.drawTangents[dazmeshVertexWeights2.vertex].y = vector9.x * m5 + vector9.y * m6 + vector9.z * m7;
								this.drawTangents[dazmeshVertexWeights2.vertex].z = vector9.x * m9 + vector9.y * m10 + vector9.z * m11;
							}
							else
							{
								Vector3 vector11;
								if (this.useOrientation)
								{
									Vector3 vector10 = this.drawVerts[dazmeshVertexWeights2.vertex];
									vector11.x = vector10.x * m13 + vector10.y * m14 + vector10.z * m15 + m16;
									vector11.y = vector10.x * m17 + vector10.y * m18 + vector10.z * m19 + m20;
									vector11.z = vector10.x * m21 + vector10.y * m22 + vector10.z * m23 + m24;
								}
								else
								{
									vector11 = this.drawVerts[dazmeshVertexWeights2.vertex];
									vector11.x += m16;
									vector11.y += m20;
									vector11.z += m24;
								}
								Vector3 vector12 = this.drawNormals[dazmeshVertexWeights2.vertex];
								Vector4 vector13 = this.drawTangents[dazmeshVertexWeights2.vertex];
								if (dazmeshVertexWeights2.zweight > 0f)
								{
									float num13 = vector6.z * dazmeshVertexWeights2.zweight;
									float num14 = (float)Math.Sin((double)num13);
									float num15 = (float)Math.Cos((double)num13);
									float x = vector11.x * num15 - vector11.y * num14;
									vector11.y = vector11.x * num14 + vector11.y * num15;
									vector11.x = x;
									float x2 = vector12.x * num15 - vector12.y * num14;
									vector12.y = vector12.x * num14 + vector12.y * num15;
									vector12.x = x2;
									float x3 = vector13.x * num15 - vector13.y * num14;
									vector13.y = vector13.x * num14 + vector13.y * num15;
									vector13.x = x3;
								}
								if (flag11)
								{
									if (flag12 && dazmeshVertexWeights2.zleftbulge > 0f)
									{
										float num16 = 1f + num9 * dazmeshVertexWeights2.zleftbulge;
										vector11.x *= num16;
										vector11.y *= num16;
									}
									if (flag14 && dazmeshVertexWeights2.zrightbulge > 0f)
									{
										float num17 = 1f + num11 * dazmeshVertexWeights2.zrightbulge;
										vector11.x *= num17;
										vector11.y *= num17;
									}
									if (flag13 && dazmeshVertexWeights2.zleftbulge > 0f)
									{
										float num18 = 1f + num10 * dazmeshVertexWeights2.zleftbulge;
										vector11.x *= num18;
										vector11.y *= num18;
									}
									if (flag15 && dazmeshVertexWeights2.zrightbulge > 0f)
									{
										float num19 = 1f + num12 * dazmeshVertexWeights2.zrightbulge;
										vector11.x *= num19;
										vector11.y *= num19;
									}
								}
								if (dazmeshVertexWeights2.yweight > 0f)
								{
									float num20 = vector6.y * dazmeshVertexWeights2.yweight;
									float num21 = (float)Math.Sin((double)num20);
									float num22 = (float)Math.Cos((double)num20);
									float x4 = vector11.x * num22 + vector11.z * num21;
									vector11.z = vector11.z * num22 - vector11.x * num21;
									vector11.x = x4;
									float x5 = vector12.x * num22 + vector12.z * num21;
									vector12.z = vector12.z * num22 - vector12.x * num21;
									vector12.x = x5;
									float x6 = vector13.x * num22 + vector13.z * num21;
									vector13.z = vector13.z * num22 - vector13.x * num21;
									vector13.x = x6;
								}
								if (flag6)
								{
									if (flag7 && dazmeshVertexWeights2.yleftbulge > 0f)
									{
										float num23 = 1f + num5 * dazmeshVertexWeights2.yleftbulge;
										vector11.x *= num23;
										vector11.z *= num23;
									}
									if (flag9 && dazmeshVertexWeights2.yrightbulge > 0f)
									{
										float num24 = 1f + num7 * dazmeshVertexWeights2.yrightbulge;
										vector11.x *= num24;
										vector11.z *= num24;
									}
									if (flag8 && dazmeshVertexWeights2.yleftbulge > 0f)
									{
										float num25 = 1f + num6 * dazmeshVertexWeights2.yleftbulge;
										vector11.x *= num25;
										vector11.z *= num25;
									}
									if (flag10 && dazmeshVertexWeights2.yrightbulge > 0f)
									{
										float num26 = 1f + num8 * dazmeshVertexWeights2.yrightbulge;
										vector11.x *= num26;
										vector11.z *= num26;
									}
								}
								if (dazmeshVertexWeights2.xweight > 0f)
								{
									float num27 = vector6.x * dazmeshVertexWeights2.xweight;
									float num28 = (float)Math.Sin((double)num27);
									float num29 = (float)Math.Cos((double)num27);
									float y = vector11.y * num29 - vector11.z * num28;
									vector11.z = vector11.y * num28 + vector11.z * num29;
									vector11.y = y;
									float y2 = vector12.y * num29 - vector12.z * num28;
									vector12.z = vector12.y * num28 + vector12.z * num29;
									vector12.y = y2;
									float y3 = vector13.y * num29 - vector13.z * num28;
									vector13.z = vector13.y * num28 + vector13.z * num29;
									vector13.y = y3;
								}
								if (flag)
								{
									if (flag2 && dazmeshVertexWeights2.xleftbulge > 0f)
									{
										float num30 = 1f + num * dazmeshVertexWeights2.xleftbulge;
										vector11.y *= num30;
										vector11.z *= num30;
									}
									if (flag4 && dazmeshVertexWeights2.xrightbulge > 0f)
									{
										float num31 = 1f + num3 * dazmeshVertexWeights2.xrightbulge;
										vector11.y *= num31;
										vector11.z *= num31;
									}
									if (flag3 && dazmeshVertexWeights2.xleftbulge > 0f)
									{
										float num32 = 1f + num2 * dazmeshVertexWeights2.xleftbulge;
										vector11.y *= num32;
										vector11.z *= num32;
									}
									if (flag5 && dazmeshVertexWeights2.xrightbulge > 0f)
									{
										float num33 = 1f + num4 * dazmeshVertexWeights2.xrightbulge;
										vector11.y *= num33;
										vector11.z *= num33;
									}
								}
								if (this.useOrientation)
								{
									this.drawVerts[dazmeshVertexWeights2.vertex].x = vector11.x * m25 + vector11.y * m26 + vector11.z * m27 + m28;
									this.drawVerts[dazmeshVertexWeights2.vertex].y = vector11.x * m29 + vector11.y * m30 + vector11.z * m31 + m32;
									this.drawVerts[dazmeshVertexWeights2.vertex].z = vector11.x * m33 + vector11.y * m34 + vector11.z * m35 + m36;
								}
								else
								{
									this.drawVerts[dazmeshVertexWeights2.vertex].x = vector11.x - m16;
									this.drawVerts[dazmeshVertexWeights2.vertex].y = vector11.y - m20;
									this.drawVerts[dazmeshVertexWeights2.vertex].z = vector11.z - m24;
								}
								this.drawNormals[dazmeshVertexWeights2.vertex] = vector12;
								this.drawTangents[dazmeshVertexWeights2.vertex] = vector13;
							}
						}
					}
				}
				else if (rotationOrder == Quaternion2Angles.RotationOrder.XZY)
				{
					foreach (DAZMeshVertexWeights dazmeshVertexWeights3 in weights)
					{
						if (dazmeshVertexWeights3.vertex >= startIndex && dazmeshVertexWeights3.vertex <= stopIndex)
						{
							if (dazmeshVertexWeights3.xweight > 0.99999f && dazmeshVertexWeights3.yweight > 0.99999f && dazmeshVertexWeights3.zweight > 0.99999f)
							{
								Vector3 vector14 = this.drawVerts[dazmeshVertexWeights3.vertex];
								this.drawVerts[dazmeshVertexWeights3.vertex].x = vector14.x * m + vector14.y * m2 + vector14.z * m3 + m4;
								this.drawVerts[dazmeshVertexWeights3.vertex].y = vector14.x * m5 + vector14.y * m6 + vector14.z * m7 + m8;
								this.drawVerts[dazmeshVertexWeights3.vertex].z = vector14.x * m9 + vector14.y * m10 + vector14.z * m11 + m12;
								Vector3 vector15 = this.drawNormals[dazmeshVertexWeights3.vertex];
								this.drawNormals[dazmeshVertexWeights3.vertex].x = vector15.x * m + vector15.y * m2 + vector15.z * m3;
								this.drawNormals[dazmeshVertexWeights3.vertex].y = vector15.x * m5 + vector15.y * m6 + vector15.z * m7;
								this.drawNormals[dazmeshVertexWeights3.vertex].z = vector15.x * m9 + vector15.y * m10 + vector15.z * m11;
								Vector4 vector16 = this.drawTangents[dazmeshVertexWeights3.vertex];
								this.drawTangents[dazmeshVertexWeights3.vertex].x = vector16.x * m + vector16.y * m2 + vector16.z * m3;
								this.drawTangents[dazmeshVertexWeights3.vertex].y = vector16.x * m5 + vector16.y * m6 + vector16.z * m7;
								this.drawTangents[dazmeshVertexWeights3.vertex].z = vector16.x * m9 + vector16.y * m10 + vector16.z * m11;
							}
							else
							{
								Vector3 vector18;
								if (this.useOrientation)
								{
									Vector3 vector17 = this.drawVerts[dazmeshVertexWeights3.vertex];
									vector18.x = vector17.x * m13 + vector17.y * m14 + vector17.z * m15 + m16;
									vector18.y = vector17.x * m17 + vector17.y * m18 + vector17.z * m19 + m20;
									vector18.z = vector17.x * m21 + vector17.y * m22 + vector17.z * m23 + m24;
								}
								else
								{
									vector18 = this.drawVerts[dazmeshVertexWeights3.vertex];
									vector18.x += m16;
									vector18.y += m20;
									vector18.z += m24;
								}
								Vector3 vector19 = this.drawNormals[dazmeshVertexWeights3.vertex];
								Vector4 vector20 = this.drawTangents[dazmeshVertexWeights3.vertex];
								if (dazmeshVertexWeights3.yweight > 0f)
								{
									float num34 = vector6.y * dazmeshVertexWeights3.yweight;
									float num35 = (float)Math.Sin((double)num34);
									float num36 = (float)Math.Cos((double)num34);
									float x7 = vector18.x * num36 + vector18.z * num35;
									vector18.z = vector18.z * num36 - vector18.x * num35;
									vector18.x = x7;
									float x8 = vector19.x * num36 + vector19.z * num35;
									vector19.z = vector19.z * num36 - vector19.x * num35;
									vector19.x = x8;
									float x9 = vector20.x * num36 + vector20.z * num35;
									vector20.z = vector20.z * num36 - vector20.x * num35;
									vector20.x = x9;
								}
								if (flag6)
								{
									if (flag7 && dazmeshVertexWeights3.yleftbulge > 0f)
									{
										float num37 = 1f + num5 * dazmeshVertexWeights3.yleftbulge;
										vector18.x *= num37;
										vector18.z *= num37;
									}
									if (flag9 && dazmeshVertexWeights3.yrightbulge > 0f)
									{
										float num38 = 1f + num7 * dazmeshVertexWeights3.yrightbulge;
										vector18.x *= num38;
										vector18.z *= num38;
									}
									if (flag8 && dazmeshVertexWeights3.yleftbulge > 0f)
									{
										float num39 = 1f + num6 * dazmeshVertexWeights3.yleftbulge;
										vector18.x *= num39;
										vector18.z *= num39;
									}
									if (flag10 && dazmeshVertexWeights3.yrightbulge > 0f)
									{
										float num40 = 1f + num8 * dazmeshVertexWeights3.yrightbulge;
										vector18.x *= num40;
										vector18.z *= num40;
									}
								}
								if (dazmeshVertexWeights3.zweight > 0f)
								{
									float num41 = vector6.z * dazmeshVertexWeights3.zweight;
									float num42 = (float)Math.Sin((double)num41);
									float num43 = (float)Math.Cos((double)num41);
									float x10 = vector18.x * num43 - vector18.y * num42;
									vector18.y = vector18.x * num42 + vector18.y * num43;
									vector18.x = x10;
									float x11 = vector19.x * num43 - vector19.y * num42;
									vector19.y = vector19.x * num42 + vector19.y * num43;
									vector19.x = x11;
									float x12 = vector20.x * num43 - vector20.y * num42;
									vector20.y = vector20.x * num42 + vector20.y * num43;
									vector20.x = x12;
								}
								if (flag11)
								{
									if (flag12 && dazmeshVertexWeights3.zleftbulge > 0f)
									{
										float num44 = 1f + num9 * dazmeshVertexWeights3.zleftbulge;
										vector18.x *= num44;
										vector18.y *= num44;
									}
									if (flag14 && dazmeshVertexWeights3.zrightbulge > 0f)
									{
										float num45 = 1f + num11 * dazmeshVertexWeights3.zrightbulge;
										vector18.x *= num45;
										vector18.y *= num45;
									}
									if (flag13 && dazmeshVertexWeights3.zleftbulge > 0f)
									{
										float num46 = 1f + num10 * dazmeshVertexWeights3.zleftbulge;
										vector18.x *= num46;
										vector18.y *= num46;
									}
									if (flag15 && dazmeshVertexWeights3.zrightbulge > 0f)
									{
										float num47 = 1f + num12 * dazmeshVertexWeights3.zrightbulge;
										vector18.x *= num47;
										vector18.y *= num47;
									}
								}
								if (dazmeshVertexWeights3.xweight > 0f)
								{
									float num48 = vector6.x * dazmeshVertexWeights3.xweight;
									float num49 = (float)Math.Sin((double)num48);
									float num50 = (float)Math.Cos((double)num48);
									float y4 = vector18.y * num50 - vector18.z * num49;
									vector18.z = vector18.y * num49 + vector18.z * num50;
									vector18.y = y4;
									float y5 = vector19.y * num50 - vector19.z * num49;
									vector19.z = vector19.y * num49 + vector19.z * num50;
									vector19.y = y5;
									float y6 = vector20.y * num50 - vector20.z * num49;
									vector20.z = vector20.y * num49 + vector20.z * num50;
									vector20.y = y6;
								}
								if (flag)
								{
									if (flag2 && dazmeshVertexWeights3.xleftbulge > 0f)
									{
										float num51 = 1f + num * dazmeshVertexWeights3.xleftbulge;
										vector18.y *= num51;
										vector18.z *= num51;
									}
									if (flag4 && dazmeshVertexWeights3.xrightbulge > 0f)
									{
										float num52 = 1f + num3 * dazmeshVertexWeights3.xrightbulge;
										vector18.y *= num52;
										vector18.z *= num52;
									}
									if (flag3 && dazmeshVertexWeights3.xleftbulge > 0f)
									{
										float num53 = 1f + num2 * dazmeshVertexWeights3.xleftbulge;
										vector18.y *= num53;
										vector18.z *= num53;
									}
									if (flag5 && dazmeshVertexWeights3.xrightbulge > 0f)
									{
										float num54 = 1f + num4 * dazmeshVertexWeights3.xrightbulge;
										vector18.y *= num54;
										vector18.z *= num54;
									}
								}
								if (this.useOrientation)
								{
									this.drawVerts[dazmeshVertexWeights3.vertex].x = vector18.x * m25 + vector18.y * m26 + vector18.z * m27 + m28;
									this.drawVerts[dazmeshVertexWeights3.vertex].y = vector18.x * m29 + vector18.y * m30 + vector18.z * m31 + m32;
									this.drawVerts[dazmeshVertexWeights3.vertex].z = vector18.x * m33 + vector18.y * m34 + vector18.z * m35 + m36;
								}
								else
								{
									this.drawVerts[dazmeshVertexWeights3.vertex].x = vector18.x - m16;
									this.drawVerts[dazmeshVertexWeights3.vertex].y = vector18.y - m20;
									this.drawVerts[dazmeshVertexWeights3.vertex].z = vector18.z - m24;
								}
								this.drawNormals[dazmeshVertexWeights3.vertex] = vector19;
								this.drawTangents[dazmeshVertexWeights3.vertex] = vector20;
							}
						}
					}
				}
				else if (rotationOrder == Quaternion2Angles.RotationOrder.YXZ)
				{
					foreach (DAZMeshVertexWeights dazmeshVertexWeights4 in weights)
					{
						if (dazmeshVertexWeights4.vertex >= startIndex && dazmeshVertexWeights4.vertex <= stopIndex)
						{
							if (dazmeshVertexWeights4.xweight > 0.99999f && dazmeshVertexWeights4.yweight > 0.99999f && dazmeshVertexWeights4.zweight > 0.99999f)
							{
								Vector3 vector21 = this.drawVerts[dazmeshVertexWeights4.vertex];
								this.drawVerts[dazmeshVertexWeights4.vertex].x = vector21.x * m + vector21.y * m2 + vector21.z * m3 + m4;
								this.drawVerts[dazmeshVertexWeights4.vertex].y = vector21.x * m5 + vector21.y * m6 + vector21.z * m7 + m8;
								this.drawVerts[dazmeshVertexWeights4.vertex].z = vector21.x * m9 + vector21.y * m10 + vector21.z * m11 + m12;
								Vector3 vector22 = this.drawNormals[dazmeshVertexWeights4.vertex];
								this.drawNormals[dazmeshVertexWeights4.vertex].x = vector22.x * m + vector22.y * m2 + vector22.z * m3;
								this.drawNormals[dazmeshVertexWeights4.vertex].y = vector22.x * m5 + vector22.y * m6 + vector22.z * m7;
								this.drawNormals[dazmeshVertexWeights4.vertex].z = vector22.x * m9 + vector22.y * m10 + vector22.z * m11;
								Vector4 vector23 = this.drawTangents[dazmeshVertexWeights4.vertex];
								this.drawTangents[dazmeshVertexWeights4.vertex].x = vector23.x * m + vector23.y * m2 + vector23.z * m3;
								this.drawTangents[dazmeshVertexWeights4.vertex].y = vector23.x * m5 + vector23.y * m6 + vector23.z * m7;
								this.drawTangents[dazmeshVertexWeights4.vertex].z = vector23.x * m9 + vector23.y * m10 + vector23.z * m11;
							}
							else
							{
								Vector3 vector25;
								if (this.useOrientation)
								{
									Vector3 vector24 = this.drawVerts[dazmeshVertexWeights4.vertex];
									vector25.x = vector24.x * m13 + vector24.y * m14 + vector24.z * m15 + m16;
									vector25.y = vector24.x * m17 + vector24.y * m18 + vector24.z * m19 + m20;
									vector25.z = vector24.x * m21 + vector24.y * m22 + vector24.z * m23 + m24;
								}
								else
								{
									vector25 = this.drawVerts[dazmeshVertexWeights4.vertex];
									vector25.x += m16;
									vector25.y += m20;
									vector25.z += m24;
								}
								Vector3 vector26 = this.drawNormals[dazmeshVertexWeights4.vertex];
								Vector4 vector27 = this.drawTangents[dazmeshVertexWeights4.vertex];
								if (dazmeshVertexWeights4.zweight > 0f)
								{
									float num55 = vector6.z * dazmeshVertexWeights4.zweight;
									float num56 = (float)Math.Sin((double)num55);
									float num57 = (float)Math.Cos((double)num55);
									float x13 = vector25.x * num57 - vector25.y * num56;
									vector25.y = vector25.x * num56 + vector25.y * num57;
									vector25.x = x13;
									float x14 = vector26.x * num57 - vector26.y * num56;
									vector26.y = vector26.x * num56 + vector26.y * num57;
									vector26.x = x14;
									float x15 = vector27.x * num57 - vector27.y * num56;
									vector27.y = vector27.x * num56 + vector27.y * num57;
									vector27.x = x15;
								}
								if (flag11)
								{
									if (flag12 && dazmeshVertexWeights4.zleftbulge > 0f)
									{
										float num58 = 1f + num9 * dazmeshVertexWeights4.zleftbulge;
										vector25.x *= num58;
										vector25.y *= num58;
									}
									if (flag14 && dazmeshVertexWeights4.zrightbulge > 0f)
									{
										float num59 = 1f + num11 * dazmeshVertexWeights4.zrightbulge;
										vector25.x *= num59;
										vector25.y *= num59;
									}
									if (flag13 && dazmeshVertexWeights4.zleftbulge > 0f)
									{
										float num60 = 1f + num10 * dazmeshVertexWeights4.zleftbulge;
										vector25.x *= num60;
										vector25.y *= num60;
									}
									if (flag15 && dazmeshVertexWeights4.zrightbulge > 0f)
									{
										float num61 = 1f + num12 * dazmeshVertexWeights4.zrightbulge;
										vector25.x *= num61;
										vector25.y *= num61;
									}
								}
								if (dazmeshVertexWeights4.xweight > 0f)
								{
									float num62 = vector6.x * dazmeshVertexWeights4.xweight;
									float num63 = (float)Math.Sin((double)num62);
									float num64 = (float)Math.Cos((double)num62);
									float y7 = vector25.y * num64 - vector25.z * num63;
									vector25.z = vector25.y * num63 + vector25.z * num64;
									vector25.y = y7;
									float y8 = vector26.y * num64 - vector26.z * num63;
									vector26.z = vector26.y * num63 + vector26.z * num64;
									vector26.y = y8;
									float y9 = vector27.y * num64 - vector27.z * num63;
									vector27.z = vector27.y * num63 + vector27.z * num64;
									vector27.y = y9;
								}
								if (flag)
								{
									if (flag2 && dazmeshVertexWeights4.xleftbulge > 0f)
									{
										float num65 = 1f + num * dazmeshVertexWeights4.xleftbulge;
										vector25.y *= num65;
										vector25.z *= num65;
									}
									if (flag4 && dazmeshVertexWeights4.xrightbulge > 0f)
									{
										float num66 = 1f + num3 * dazmeshVertexWeights4.xrightbulge;
										vector25.y *= num66;
										vector25.z *= num66;
									}
									if (flag3 && dazmeshVertexWeights4.xleftbulge > 0f)
									{
										float num67 = 1f + num2 * dazmeshVertexWeights4.xleftbulge;
										vector25.y *= num67;
										vector25.z *= num67;
									}
									if (flag5 && dazmeshVertexWeights4.xrightbulge > 0f)
									{
										float num68 = 1f + num4 * dazmeshVertexWeights4.xrightbulge;
										vector25.y *= num68;
										vector25.z *= num68;
									}
								}
								if (dazmeshVertexWeights4.yweight > 0f)
								{
									float num69 = vector6.y * dazmeshVertexWeights4.yweight;
									float num70 = (float)Math.Sin((double)num69);
									float num71 = (float)Math.Cos((double)num69);
									float x16 = vector25.x * num71 + vector25.z * num70;
									vector25.z = vector25.z * num71 - vector25.x * num70;
									vector25.x = x16;
									float x17 = vector26.x * num71 + vector26.z * num70;
									vector26.z = vector26.z * num71 - vector26.x * num70;
									vector26.x = x17;
									float x18 = vector27.x * num71 + vector27.z * num70;
									vector27.z = vector27.z * num71 - vector27.x * num70;
									vector27.x = x18;
								}
								if (flag6)
								{
									if (flag7 && dazmeshVertexWeights4.yleftbulge > 0f)
									{
										float num72 = 1f + num5 * dazmeshVertexWeights4.yleftbulge;
										vector25.x *= num72;
										vector25.z *= num72;
									}
									if (flag9 && dazmeshVertexWeights4.yrightbulge > 0f)
									{
										float num73 = 1f + num7 * dazmeshVertexWeights4.yrightbulge;
										vector25.x *= num73;
										vector25.z *= num73;
									}
									if (flag8 && dazmeshVertexWeights4.yleftbulge > 0f)
									{
										float num74 = 1f + num6 * dazmeshVertexWeights4.yleftbulge;
										vector25.x *= num74;
										vector25.z *= num74;
									}
									if (flag10 && dazmeshVertexWeights4.yrightbulge > 0f)
									{
										float num75 = 1f + num8 * dazmeshVertexWeights4.yrightbulge;
										vector25.x *= num75;
										vector25.z *= num75;
									}
								}
								if (this.useOrientation)
								{
									this.drawVerts[dazmeshVertexWeights4.vertex].x = vector25.x * m25 + vector25.y * m26 + vector25.z * m27 + m28;
									this.drawVerts[dazmeshVertexWeights4.vertex].y = vector25.x * m29 + vector25.y * m30 + vector25.z * m31 + m32;
									this.drawVerts[dazmeshVertexWeights4.vertex].z = vector25.x * m33 + vector25.y * m34 + vector25.z * m35 + m36;
								}
								else
								{
									this.drawVerts[dazmeshVertexWeights4.vertex].x = vector25.x - m16;
									this.drawVerts[dazmeshVertexWeights4.vertex].y = vector25.y - m20;
									this.drawVerts[dazmeshVertexWeights4.vertex].z = vector25.z - m24;
								}
								this.drawNormals[dazmeshVertexWeights4.vertex] = vector26;
								this.drawTangents[dazmeshVertexWeights4.vertex] = vector27;
							}
						}
					}
				}
				else if (rotationOrder == Quaternion2Angles.RotationOrder.YZX)
				{
					foreach (DAZMeshVertexWeights dazmeshVertexWeights5 in weights)
					{
						if (dazmeshVertexWeights5.vertex >= startIndex && dazmeshVertexWeights5.vertex <= stopIndex)
						{
							if (dazmeshVertexWeights5.xweight > 0.99999f && dazmeshVertexWeights5.yweight > 0.99999f && dazmeshVertexWeights5.zweight > 0.99999f)
							{
								Vector3 vector28 = this.drawVerts[dazmeshVertexWeights5.vertex];
								this.drawVerts[dazmeshVertexWeights5.vertex].x = vector28.x * m + vector28.y * m2 + vector28.z * m3 + m4;
								this.drawVerts[dazmeshVertexWeights5.vertex].y = vector28.x * m5 + vector28.y * m6 + vector28.z * m7 + m8;
								this.drawVerts[dazmeshVertexWeights5.vertex].z = vector28.x * m9 + vector28.y * m10 + vector28.z * m11 + m12;
								Vector3 vector29 = this.drawNormals[dazmeshVertexWeights5.vertex];
								this.drawNormals[dazmeshVertexWeights5.vertex].x = vector29.x * m + vector29.y * m2 + vector29.z * m3;
								this.drawNormals[dazmeshVertexWeights5.vertex].y = vector29.x * m5 + vector29.y * m6 + vector29.z * m7;
								this.drawNormals[dazmeshVertexWeights5.vertex].z = vector29.x * m9 + vector29.y * m10 + vector29.z * m11;
								Vector4 vector30 = this.drawTangents[dazmeshVertexWeights5.vertex];
								this.drawTangents[dazmeshVertexWeights5.vertex].x = vector30.x * m + vector30.y * m2 + vector30.z * m3;
								this.drawTangents[dazmeshVertexWeights5.vertex].y = vector30.x * m5 + vector30.y * m6 + vector30.z * m7;
								this.drawTangents[dazmeshVertexWeights5.vertex].z = vector30.x * m9 + vector30.y * m10 + vector30.z * m11;
							}
							else
							{
								Vector3 vector32;
								if (this.useOrientation)
								{
									Vector3 vector31 = this.drawVerts[dazmeshVertexWeights5.vertex];
									vector32.x = vector31.x * m13 + vector31.y * m14 + vector31.z * m15 + m16;
									vector32.y = vector31.x * m17 + vector31.y * m18 + vector31.z * m19 + m20;
									vector32.z = vector31.x * m21 + vector31.y * m22 + vector31.z * m23 + m24;
								}
								else
								{
									vector32 = this.drawVerts[dazmeshVertexWeights5.vertex];
									vector32.x += m16;
									vector32.y += m20;
									vector32.z += m24;
								}
								Vector3 vector33 = this.drawNormals[dazmeshVertexWeights5.vertex];
								Vector4 vector34 = this.drawTangents[dazmeshVertexWeights5.vertex];
								if (dazmeshVertexWeights5.xweight > 0f)
								{
									float num77 = vector6.x * dazmeshVertexWeights5.xweight;
									float num78 = (float)Math.Sin((double)num77);
									float num79 = (float)Math.Cos((double)num77);
									float y10 = vector32.y * num79 - vector32.z * num78;
									vector32.z = vector32.y * num78 + vector32.z * num79;
									vector32.y = y10;
									float y11 = vector33.y * num79 - vector33.z * num78;
									vector33.z = vector33.y * num78 + vector33.z * num79;
									vector33.y = y11;
									float y12 = vector34.y * num79 - vector34.z * num78;
									vector34.z = vector34.y * num78 + vector34.z * num79;
									vector34.y = y12;
								}
								if (flag)
								{
									if (flag2 && dazmeshVertexWeights5.xleftbulge > 0f)
									{
										float num80 = 1f + num * dazmeshVertexWeights5.xleftbulge;
										vector32.y *= num80;
										vector32.z *= num80;
									}
									if (flag4 && dazmeshVertexWeights5.xrightbulge > 0f)
									{
										float num81 = 1f + num3 * dazmeshVertexWeights5.xrightbulge;
										vector32.y *= num81;
										vector32.z *= num81;
									}
									if (flag3 && dazmeshVertexWeights5.xleftbulge > 0f)
									{
										float num82 = 1f + num2 * dazmeshVertexWeights5.xleftbulge;
										vector32.y *= num82;
										vector32.z *= num82;
									}
									if (flag5 && dazmeshVertexWeights5.xrightbulge > 0f)
									{
										float num83 = 1f + num4 * dazmeshVertexWeights5.xrightbulge;
										vector32.y *= num83;
										vector32.z *= num83;
									}
								}
								if (dazmeshVertexWeights5.zweight > 0f)
								{
									float num84 = vector6.z * dazmeshVertexWeights5.zweight;
									float num85 = (float)Math.Sin((double)num84);
									float num86 = (float)Math.Cos((double)num84);
									float x19 = vector32.x * num86 - vector32.y * num85;
									vector32.y = vector32.x * num85 + vector32.y * num86;
									vector32.x = x19;
									float x20 = vector33.x * num86 - vector33.y * num85;
									vector33.y = vector33.x * num85 + vector33.y * num86;
									vector33.x = x20;
									float x21 = vector34.x * num86 - vector34.y * num85;
									vector34.y = vector34.x * num85 + vector34.y * num86;
									vector34.x = x21;
								}
								if (flag11)
								{
									if (flag12 && dazmeshVertexWeights5.zleftbulge > 0f)
									{
										float num87 = 1f + num9 * dazmeshVertexWeights5.zleftbulge;
										vector32.x *= num87;
										vector32.y *= num87;
									}
									if (flag14 && dazmeshVertexWeights5.zrightbulge > 0f)
									{
										float num88 = 1f + num11 * dazmeshVertexWeights5.zrightbulge;
										vector32.x *= num88;
										vector32.y *= num88;
									}
									if (flag13 && dazmeshVertexWeights5.zleftbulge > 0f)
									{
										float num89 = 1f + num10 * dazmeshVertexWeights5.zleftbulge;
										vector32.x *= num89;
										vector32.y *= num89;
									}
									if (flag15 && dazmeshVertexWeights5.zrightbulge > 0f)
									{
										float num90 = 1f + num12 * dazmeshVertexWeights5.zrightbulge;
										vector32.x *= num90;
										vector32.y *= num90;
									}
								}
								if (dazmeshVertexWeights5.yweight > 0f)
								{
									float num91 = vector6.y * dazmeshVertexWeights5.yweight;
									float num92 = (float)Math.Sin((double)num91);
									float num93 = (float)Math.Cos((double)num91);
									float x22 = vector32.x * num93 + vector32.z * num92;
									vector32.z = vector32.z * num93 - vector32.x * num92;
									vector32.x = x22;
									float x23 = vector33.x * num93 + vector33.z * num92;
									vector33.z = vector33.z * num93 - vector33.x * num92;
									vector33.x = x23;
									float x24 = vector34.x * num93 + vector34.z * num92;
									vector34.z = vector34.z * num93 - vector34.x * num92;
									vector34.x = x24;
								}
								if (flag6)
								{
									if (flag7 && dazmeshVertexWeights5.yleftbulge > 0f)
									{
										float num94 = 1f + num5 * dazmeshVertexWeights5.yleftbulge;
										vector32.x *= num94;
										vector32.z *= num94;
									}
									if (flag9 && dazmeshVertexWeights5.yrightbulge > 0f)
									{
										float num95 = 1f + num7 * dazmeshVertexWeights5.yrightbulge;
										vector32.x *= num95;
										vector32.z *= num95;
									}
									if (flag8 && dazmeshVertexWeights5.yleftbulge > 0f)
									{
										float num96 = 1f + num6 * dazmeshVertexWeights5.yleftbulge;
										vector32.x *= num96;
										vector32.z *= num96;
									}
									if (flag10 && dazmeshVertexWeights5.yrightbulge > 0f)
									{
										float num97 = 1f + num8 * dazmeshVertexWeights5.yrightbulge;
										vector32.x *= num97;
										vector32.z *= num97;
									}
								}
								if (this.useOrientation)
								{
									this.drawVerts[dazmeshVertexWeights5.vertex].x = vector32.x * m25 + vector32.y * m26 + vector32.z * m27 + m28;
									this.drawVerts[dazmeshVertexWeights5.vertex].y = vector32.x * m29 + vector32.y * m30 + vector32.z * m31 + m32;
									this.drawVerts[dazmeshVertexWeights5.vertex].z = vector32.x * m33 + vector32.y * m34 + vector32.z * m35 + m36;
								}
								else
								{
									this.drawVerts[dazmeshVertexWeights5.vertex].x = vector32.x - m16;
									this.drawVerts[dazmeshVertexWeights5.vertex].y = vector32.y - m20;
									this.drawVerts[dazmeshVertexWeights5.vertex].z = vector32.z - m24;
								}
								this.drawNormals[dazmeshVertexWeights5.vertex] = vector33;
								this.drawTangents[dazmeshVertexWeights5.vertex] = vector34;
							}
						}
					}
				}
				else if (rotationOrder == Quaternion2Angles.RotationOrder.ZXY)
				{
					foreach (DAZMeshVertexWeights dazmeshVertexWeights6 in weights)
					{
						if (dazmeshVertexWeights6.vertex >= startIndex && dazmeshVertexWeights6.vertex <= stopIndex)
						{
							if (dazmeshVertexWeights6.xweight > 0.99999f && dazmeshVertexWeights6.yweight > 0.99999f && dazmeshVertexWeights6.zweight > 0.99999f)
							{
								Vector3 vector35 = this.drawVerts[dazmeshVertexWeights6.vertex];
								this.drawVerts[dazmeshVertexWeights6.vertex].x = vector35.x * m + vector35.y * m2 + vector35.z * m3 + m4;
								this.drawVerts[dazmeshVertexWeights6.vertex].y = vector35.x * m5 + vector35.y * m6 + vector35.z * m7 + m8;
								this.drawVerts[dazmeshVertexWeights6.vertex].z = vector35.x * m9 + vector35.y * m10 + vector35.z * m11 + m12;
								Vector3 vector36 = this.drawNormals[dazmeshVertexWeights6.vertex];
								this.drawNormals[dazmeshVertexWeights6.vertex].x = vector36.x * m + vector36.y * m2 + vector36.z * m3;
								this.drawNormals[dazmeshVertexWeights6.vertex].y = vector36.x * m5 + vector36.y * m6 + vector36.z * m7;
								this.drawNormals[dazmeshVertexWeights6.vertex].z = vector36.x * m9 + vector36.y * m10 + vector36.z * m11;
								Vector4 vector37 = this.drawTangents[dazmeshVertexWeights6.vertex];
								this.drawTangents[dazmeshVertexWeights6.vertex].x = vector37.x * m + vector37.y * m2 + vector37.z * m3;
								this.drawTangents[dazmeshVertexWeights6.vertex].y = vector37.x * m5 + vector37.y * m6 + vector37.z * m7;
								this.drawTangents[dazmeshVertexWeights6.vertex].z = vector37.x * m9 + vector37.y * m10 + vector37.z * m11;
							}
							else
							{
								Vector3 vector39;
								if (this.useOrientation)
								{
									Vector3 vector38 = this.drawVerts[dazmeshVertexWeights6.vertex];
									vector39.x = vector38.x * m13 + vector38.y * m14 + vector38.z * m15 + m16;
									vector39.y = vector38.x * m17 + vector38.y * m18 + vector38.z * m19 + m20;
									vector39.z = vector38.x * m21 + vector38.y * m22 + vector38.z * m23 + m24;
								}
								else
								{
									vector39 = this.drawVerts[dazmeshVertexWeights6.vertex];
									vector39.x += m16;
									vector39.y += m20;
									vector39.z += m24;
								}
								Vector3 vector40 = this.drawNormals[dazmeshVertexWeights6.vertex];
								Vector4 vector41 = this.drawTangents[dazmeshVertexWeights6.vertex];
								if (dazmeshVertexWeights6.yweight > 0f)
								{
									float num99 = vector6.y * dazmeshVertexWeights6.yweight;
									float num100 = (float)Math.Sin((double)num99);
									float num101 = (float)Math.Cos((double)num99);
									float x25 = vector39.x * num101 + vector39.z * num100;
									vector39.z = vector39.z * num101 - vector39.x * num100;
									vector39.x = x25;
									float x26 = vector40.x * num101 + vector40.z * num100;
									vector40.z = vector40.z * num101 - vector40.x * num100;
									vector40.x = x26;
									float x27 = vector41.x * num101 + vector41.z * num100;
									vector41.z = vector41.z * num101 - vector41.x * num100;
									vector41.x = x27;
								}
								if (flag6)
								{
									if (flag7 && dazmeshVertexWeights6.yleftbulge > 0f)
									{
										float num102 = 1f + num5 * dazmeshVertexWeights6.yleftbulge;
										vector39.x *= num102;
										vector39.z *= num102;
									}
									if (flag9 && dazmeshVertexWeights6.yrightbulge > 0f)
									{
										float num103 = 1f + num7 * dazmeshVertexWeights6.yrightbulge;
										vector39.x *= num103;
										vector39.z *= num103;
									}
									if (flag8 && dazmeshVertexWeights6.yleftbulge > 0f)
									{
										float num104 = 1f + num6 * dazmeshVertexWeights6.yleftbulge;
										vector39.x *= num104;
										vector39.z *= num104;
									}
									if (flag10 && dazmeshVertexWeights6.yrightbulge > 0f)
									{
										float num105 = 1f + num8 * dazmeshVertexWeights6.yrightbulge;
										vector39.x *= num105;
										vector39.z *= num105;
									}
								}
								if (dazmeshVertexWeights6.xweight > 0f)
								{
									float num106 = vector6.x * dazmeshVertexWeights6.xweight;
									float num107 = (float)Math.Sin((double)num106);
									float num108 = (float)Math.Cos((double)num106);
									float y13 = vector39.y * num108 - vector39.z * num107;
									vector39.z = vector39.y * num107 + vector39.z * num108;
									vector39.y = y13;
									float y14 = vector40.y * num108 - vector40.z * num107;
									vector40.z = vector40.y * num107 + vector40.z * num108;
									vector40.y = y14;
									float y15 = vector41.y * num108 - vector41.z * num107;
									vector41.z = vector41.y * num107 + vector41.z * num108;
									vector41.y = y15;
								}
								if (flag)
								{
									if (flag2 && dazmeshVertexWeights6.xleftbulge > 0f)
									{
										float num109 = 1f + num * dazmeshVertexWeights6.xleftbulge;
										vector39.y *= num109;
										vector39.z *= num109;
									}
									if (flag4 && dazmeshVertexWeights6.xrightbulge > 0f)
									{
										float num110 = 1f + num3 * dazmeshVertexWeights6.xrightbulge;
										vector39.y *= num110;
										vector39.z *= num110;
									}
									if (flag3 && dazmeshVertexWeights6.xleftbulge > 0f)
									{
										float num111 = 1f + num2 * dazmeshVertexWeights6.xleftbulge;
										vector39.y *= num111;
										vector39.z *= num111;
									}
									if (flag5 && dazmeshVertexWeights6.xrightbulge > 0f)
									{
										float num112 = 1f + num4 * dazmeshVertexWeights6.xrightbulge;
										vector39.y *= num112;
										vector39.z *= num112;
									}
								}
								if (dazmeshVertexWeights6.zweight > 0f)
								{
									float num113 = vector6.z * dazmeshVertexWeights6.zweight;
									float num114 = (float)Math.Sin((double)num113);
									float num115 = (float)Math.Cos((double)num113);
									float x28 = vector39.x * num115 - vector39.y * num114;
									vector39.y = vector39.x * num114 + vector39.y * num115;
									vector39.x = x28;
									float x29 = vector40.x * num115 - vector40.y * num114;
									vector40.y = vector40.x * num114 + vector40.y * num115;
									vector40.x = x29;
									float x30 = vector41.x * num115 - vector41.y * num114;
									vector41.y = vector41.x * num114 + vector41.y * num115;
									vector41.x = x30;
								}
								if (flag11)
								{
									if (flag12 && dazmeshVertexWeights6.zleftbulge > 0f)
									{
										float num116 = 1f + num9 * dazmeshVertexWeights6.zleftbulge;
										vector39.x *= num116;
										vector39.y *= num116;
									}
									if (flag14 && dazmeshVertexWeights6.zrightbulge > 0f)
									{
										float num117 = 1f + num11 * dazmeshVertexWeights6.zrightbulge;
										vector39.x *= num117;
										vector39.y *= num117;
									}
									if (flag13 && dazmeshVertexWeights6.zleftbulge > 0f)
									{
										float num118 = 1f + num10 * dazmeshVertexWeights6.zleftbulge;
										vector39.x *= num118;
										vector39.y *= num118;
									}
									if (flag15 && dazmeshVertexWeights6.zrightbulge > 0f)
									{
										float num119 = 1f + num12 * dazmeshVertexWeights6.zrightbulge;
										vector39.x *= num119;
										vector39.y *= num119;
									}
								}
								if (this.useOrientation)
								{
									this.drawVerts[dazmeshVertexWeights6.vertex].x = vector39.x * m25 + vector39.y * m26 + vector39.z * m27 + m28;
									this.drawVerts[dazmeshVertexWeights6.vertex].y = vector39.x * m29 + vector39.y * m30 + vector39.z * m31 + m32;
									this.drawVerts[dazmeshVertexWeights6.vertex].z = vector39.x * m33 + vector39.y * m34 + vector39.z * m35 + m36;
								}
								else
								{
									this.drawVerts[dazmeshVertexWeights6.vertex].x = vector39.x - m16;
									this.drawVerts[dazmeshVertexWeights6.vertex].y = vector39.y - m20;
									this.drawVerts[dazmeshVertexWeights6.vertex].z = vector39.z - m24;
								}
								this.drawNormals[dazmeshVertexWeights6.vertex] = vector40;
								this.drawTangents[dazmeshVertexWeights6.vertex] = vector41;
							}
						}
					}
				}
				else if (rotationOrder == Quaternion2Angles.RotationOrder.ZYX)
				{
					foreach (DAZMeshVertexWeights dazmeshVertexWeights7 in weights)
					{
						if (dazmeshVertexWeights7.vertex >= startIndex && dazmeshVertexWeights7.vertex <= stopIndex)
						{
							if (dazmeshVertexWeights7.xweight > 0.99999f && dazmeshVertexWeights7.yweight > 0.99999f && dazmeshVertexWeights7.zweight > 0.99999f)
							{
								Vector3 vector42 = this.drawVerts[dazmeshVertexWeights7.vertex];
								this.drawVerts[dazmeshVertexWeights7.vertex].x = vector42.x * m + vector42.y * m2 + vector42.z * m3 + m4;
								this.drawVerts[dazmeshVertexWeights7.vertex].y = vector42.x * m5 + vector42.y * m6 + vector42.z * m7 + m8;
								this.drawVerts[dazmeshVertexWeights7.vertex].z = vector42.x * m9 + vector42.y * m10 + vector42.z * m11 + m12;
								Vector3 vector43 = this.drawNormals[dazmeshVertexWeights7.vertex];
								this.drawNormals[dazmeshVertexWeights7.vertex].x = vector43.x * m + vector43.y * m2 + vector43.z * m3;
								this.drawNormals[dazmeshVertexWeights7.vertex].y = vector43.x * m5 + vector43.y * m6 + vector43.z * m7;
								this.drawNormals[dazmeshVertexWeights7.vertex].z = vector43.x * m9 + vector43.y * m10 + vector43.z * m11;
								Vector4 vector44 = this.drawTangents[dazmeshVertexWeights7.vertex];
								this.drawTangents[dazmeshVertexWeights7.vertex].x = vector44.x * m + vector44.y * m2 + vector44.z * m3;
								this.drawTangents[dazmeshVertexWeights7.vertex].y = vector44.x * m5 + vector44.y * m6 + vector44.z * m7;
								this.drawTangents[dazmeshVertexWeights7.vertex].z = vector44.x * m9 + vector44.y * m10 + vector44.z * m11;
							}
							else
							{
								Vector3 vector46;
								if (this.useOrientation)
								{
									Vector3 vector45 = this.drawVerts[dazmeshVertexWeights7.vertex];
									vector46.x = vector45.x * m13 + vector45.y * m14 + vector45.z * m15 + m16;
									vector46.y = vector45.x * m17 + vector45.y * m18 + vector45.z * m19 + m20;
									vector46.z = vector45.x * m21 + vector45.y * m22 + vector45.z * m23 + m24;
								}
								else
								{
									vector46 = this.drawVerts[dazmeshVertexWeights7.vertex];
									vector46.x += m16;
									vector46.y += m20;
									vector46.z += m24;
								}
								Vector3 vector47 = this.drawNormals[dazmeshVertexWeights7.vertex];
								Vector4 vector48 = this.drawTangents[dazmeshVertexWeights7.vertex];
								if (dazmeshVertexWeights7.xweight > 0f)
								{
									float num121 = vector6.x * dazmeshVertexWeights7.xweight;
									float num122 = (float)Math.Sin((double)num121);
									float num123 = (float)Math.Cos((double)num121);
									float y16 = vector46.y * num123 - vector46.z * num122;
									vector46.z = vector46.y * num122 + vector46.z * num123;
									vector46.y = y16;
									float y17 = vector47.y * num123 - vector47.z * num122;
									vector47.z = vector47.y * num122 + vector47.z * num123;
									vector47.y = y17;
									float y18 = vector48.y * num123 - vector48.z * num122;
									vector48.z = vector48.y * num122 + vector48.z * num123;
									vector48.y = y18;
								}
								if (flag)
								{
									if (flag2 && dazmeshVertexWeights7.xleftbulge > 0f)
									{
										float num124 = 1f + num * dazmeshVertexWeights7.xleftbulge;
										vector46.y *= num124;
										vector46.z *= num124;
									}
									if (flag4 && dazmeshVertexWeights7.xrightbulge > 0f)
									{
										float num125 = 1f + num3 * dazmeshVertexWeights7.xrightbulge;
										vector46.y *= num125;
										vector46.z *= num125;
									}
									if (flag3 && dazmeshVertexWeights7.xleftbulge > 0f)
									{
										float num126 = 1f + num2 * dazmeshVertexWeights7.xleftbulge;
										vector46.y *= num126;
										vector46.z *= num126;
									}
									if (flag5 && dazmeshVertexWeights7.xrightbulge > 0f)
									{
										float num127 = 1f + num4 * dazmeshVertexWeights7.xrightbulge;
										vector46.y *= num127;
										vector46.z *= num127;
									}
								}
								if (dazmeshVertexWeights7.yweight > 0f)
								{
									float num128 = vector6.y * dazmeshVertexWeights7.yweight;
									float num129 = (float)Math.Sin((double)num128);
									float num130 = (float)Math.Cos((double)num128);
									float x31 = vector46.x * num130 + vector46.z * num129;
									vector46.z = vector46.z * num130 - vector46.x * num129;
									vector46.x = x31;
									float x32 = vector47.x * num130 + vector47.z * num129;
									vector47.z = vector47.z * num130 - vector47.x * num129;
									vector47.x = x32;
									float x33 = vector48.x * num130 + vector48.z * num129;
									vector48.z = vector48.z * num130 - vector48.x * num129;
									vector48.x = x33;
								}
								if (flag6)
								{
									if (flag7 && dazmeshVertexWeights7.yleftbulge > 0f)
									{
										float num131 = 1f + num5 * dazmeshVertexWeights7.yleftbulge;
										vector46.x *= num131;
										vector46.z *= num131;
									}
									if (flag9 && dazmeshVertexWeights7.yrightbulge > 0f)
									{
										float num132 = 1f + num7 * dazmeshVertexWeights7.yrightbulge;
										vector46.x *= num132;
										vector46.z *= num132;
									}
									if (flag8 && dazmeshVertexWeights7.yleftbulge > 0f)
									{
										float num133 = 1f + num6 * dazmeshVertexWeights7.yleftbulge;
										vector46.x *= num133;
										vector46.z *= num133;
									}
									if (flag10 && dazmeshVertexWeights7.yrightbulge > 0f)
									{
										float num134 = 1f + num8 * dazmeshVertexWeights7.yrightbulge;
										vector46.x *= num134;
										vector46.z *= num134;
									}
								}
								if (dazmeshVertexWeights7.zweight > 0f)
								{
									float num135 = vector6.z * dazmeshVertexWeights7.zweight;
									float num136 = (float)Math.Sin((double)num135);
									float num137 = (float)Math.Cos((double)num135);
									float x34 = vector46.x * num137 - vector46.y * num136;
									vector46.y = vector46.x * num136 + vector46.y * num137;
									vector46.x = x34;
									float x35 = vector47.x * num137 - vector47.y * num136;
									vector47.y = vector47.x * num136 + vector47.y * num137;
									vector47.x = x35;
									float x36 = vector48.x * num137 - vector48.y * num136;
									vector48.y = vector48.x * num136 + vector48.y * num137;
									vector48.x = x36;
								}
								if (flag11)
								{
									if (flag12 && dazmeshVertexWeights7.zleftbulge > 0f)
									{
										float num138 = 1f + num9 * dazmeshVertexWeights7.zleftbulge;
										vector46.x *= num138;
										vector46.y *= num138;
									}
									if (flag14 && dazmeshVertexWeights7.zrightbulge > 0f)
									{
										float num139 = 1f + num11 * dazmeshVertexWeights7.zrightbulge;
										vector46.x *= num139;
										vector46.y *= num139;
									}
									if (flag13 && dazmeshVertexWeights7.zleftbulge > 0f)
									{
										float num140 = 1f + num10 * dazmeshVertexWeights7.zleftbulge;
										vector46.x *= num140;
										vector46.y *= num140;
									}
									if (flag15 && dazmeshVertexWeights7.zrightbulge > 0f)
									{
										float num141 = 1f + num12 * dazmeshVertexWeights7.zrightbulge;
										vector46.x *= num141;
										vector46.y *= num141;
									}
								}
								if (this.useOrientation)
								{
									this.drawVerts[dazmeshVertexWeights7.vertex].x = vector46.x * m25 + vector46.y * m26 + vector46.z * m27 + m28;
									this.drawVerts[dazmeshVertexWeights7.vertex].y = vector46.x * m29 + vector46.y * m30 + vector46.z * m31 + m32;
									this.drawVerts[dazmeshVertexWeights7.vertex].z = vector46.x * m33 + vector46.y * m34 + vector46.z * m35 + m36;
								}
								else
								{
									this.drawVerts[dazmeshVertexWeights7.vertex].x = vector46.x - m16;
									this.drawVerts[dazmeshVertexWeights7.vertex].y = vector46.y - m20;
									this.drawVerts[dazmeshVertexWeights7.vertex].z = vector46.z - m24;
								}
								this.drawNormals[dazmeshVertexWeights7.vertex] = vector47;
								this.drawTangents[dazmeshVertexWeights7.vertex] = vector48;
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06004CA1 RID: 19617 RVA: 0x0018F934 File Offset: 0x0018DD34
	protected void DrawMesh()
	{
		if (this.draw && this.mesh != null)
		{
			Matrix4x4 identity = Matrix4x4.identity;
			identity.m03 += this.drawOffset.x;
			identity.m13 += this.drawOffset.y;
			identity.m23 += this.drawOffset.z;
			for (int i = 0; i < this.mesh.subMeshCount; i++)
			{
				if (this.dazMesh.useSimpleMaterial && this.dazMesh.simpleMaterial)
				{
					Graphics.DrawMesh(this.mesh, identity, this.dazMesh.simpleMaterial, 0, null, i, null, this.dazMesh.castShadows, this.dazMesh.receiveShadows);
				}
				else
				{
					Graphics.DrawMesh(this.mesh, identity, this.dazMesh.materials[i], 0, null, i, null, this.dazMesh.castShadows, this.dazMesh.receiveShadows);
				}
			}
		}
	}

	// Token: 0x06004CA2 RID: 19618 RVA: 0x0018FA58 File Offset: 0x0018DE58
	private void Start()
	{
		this.stopwatch = new Stopwatch();
		this.stopwatch.Start();
		this.InitSkinTimes();
		if (this.useThreadControlNumThreads && ThreadControl.singleton != null)
		{
			ThreadControl singleton = ThreadControl.singleton;
			singleton.onNumSubThreadsChangedHandlers = (ThreadControl.OnNumSubThreadsChanged)Delegate.Combine(singleton.onNumSubThreadsChangedHandlers, new ThreadControl.OnNumSubThreadsChanged(this.SetNumSubThreads));
			this.numSubThreads = ThreadControl.singleton.numSubThreads;
		}
		this.InitBones();
		this.InitMesh();
	}

	// Token: 0x06004CA3 RID: 19619 RVA: 0x0018FADE File Offset: 0x0018DEDE
	private void OnDestroy()
	{
		if (this.stopwatch != null)
		{
			this.stopwatch.Stop();
		}
	}

	// Token: 0x06004CA4 RID: 19620 RVA: 0x0018FAF6 File Offset: 0x0018DEF6
	private void LateUpdate()
	{
		if (this.draw)
		{
			this.SkinMesh();
			this.DrawMesh();
		}
	}

	// Token: 0x04003B57 RID: 15191
	public bool draw;

	// Token: 0x04003B58 RID: 15192
	public bool useOrientation = true;

	// Token: 0x04003B59 RID: 15193
	[SerializeField]
	protected bool _useGeneralWeights;

	// Token: 0x04003B5A RID: 15194
	[SerializeField]
	protected bool _useFastNormals;

	// Token: 0x04003B5B RID: 15195
	[SerializeField]
	protected bool _renormalize;

	// Token: 0x04003B5C RID: 15196
	public string geometryId;

	// Token: 0x04003B5D RID: 15197
	public Transform rootBone;

	// Token: 0x04003B5E RID: 15198
	public Vector3 drawOffset;

	// Token: 0x04003B5F RID: 15199
	[SerializeField]
	private bool _useSmoothing;

	// Token: 0x04003B60 RID: 15200
	[SerializeField]
	protected int _numBones;

	// Token: 0x04003B61 RID: 15201
	public DAZNode[] nodes;

	// Token: 0x04003B62 RID: 15202
	protected List<DAZNode> importNodes;

	// Token: 0x04003B63 RID: 15203
	protected Dictionary<string, int> boneNameToIndexMap;

	// Token: 0x04003B64 RID: 15204
	protected Dictionary<string, Dictionary<int, DAZMeshVertexWeights>> boneWeightsMap;

	// Token: 0x04003B65 RID: 15205
	protected Dictionary<int, bool> vertexDoneAccumulating;

	// Token: 0x04003B66 RID: 15206
	protected Transform[] boneTransforms;

	// Token: 0x04003B67 RID: 15207
	protected DAZBone[] dazBones;

	// Token: 0x04003B68 RID: 15208
	protected Matrix4x4[] boneMatrices;

	// Token: 0x04003B69 RID: 15209
	protected Vector3[] boneRotationAngles;

	// Token: 0x04003B6A RID: 15210
	protected Matrix4x4[] startingMatrices;

	// Token: 0x04003B6B RID: 15211
	protected Matrix4x4[] startingMatricesInverted;

	// Token: 0x04003B6C RID: 15212
	protected Mesh mesh;

	// Token: 0x04003B6D RID: 15213
	public DAZMesh dazMesh;

	// Token: 0x04003B6E RID: 15214
	protected Vector3[] startVerts;

	// Token: 0x04003B6F RID: 15215
	protected Vector3[] startNormals;

	// Token: 0x04003B70 RID: 15216
	protected Vector4[] startTangents;

	// Token: 0x04003B71 RID: 15217
	protected Vector3[] smoothedVerts;

	// Token: 0x04003B72 RID: 15218
	public Vector3[] drawVerts;

	// Token: 0x04003B73 RID: 15219
	public Vector3[] drawNormals;

	// Token: 0x04003B74 RID: 15220
	public Vector4[] drawTangents;

	// Token: 0x04003B75 RID: 15221
	protected int[] strongestBone;

	// Token: 0x04003B76 RID: 15222
	protected float[] strongestBoneWeight;

	// Token: 0x04003B77 RID: 15223
	protected MeshSmooth meshSmooth;

	// Token: 0x04003B78 RID: 15224
	public bool useThreadControlNumThreads;

	// Token: 0x04003B79 RID: 15225
	[SerializeField]
	protected int _numSubThreads;

	// Token: 0x04003B7A RID: 15226
	public float mainThreadSkinStartTime;

	// Token: 0x04003B7B RID: 15227
	public float mainThreadSkinStopTime;

	// Token: 0x04003B7C RID: 15228
	public float mainThreadSkinTime;

	// Token: 0x04003B7D RID: 15229
	public float[] threadSkinStartTime;

	// Token: 0x04003B7E RID: 15230
	public float[] threadSkinStopTime;

	// Token: 0x04003B7F RID: 15231
	public float[] threadSkinTime;

	// Token: 0x04003B80 RID: 15232
	public float mainThreadSmoothStartTime;

	// Token: 0x04003B81 RID: 15233
	public float mainThreadSmoothStopTime;

	// Token: 0x04003B82 RID: 15234
	public float mainThreadSmoothTime;

	// Token: 0x04003B83 RID: 15235
	public float[] threadSmoothStartTime;

	// Token: 0x04003B84 RID: 15236
	public float[] threadSmoothStopTime;

	// Token: 0x04003B85 RID: 15237
	public float[] threadSmoothTime;

	// Token: 0x04003B86 RID: 15238
	public float mainThreadSmoothCorrectionStartTime;

	// Token: 0x04003B87 RID: 15239
	public float mainThreadSmoothCorrectionStopTime;

	// Token: 0x04003B88 RID: 15240
	public float mainThreadSmoothCorrectionTime;

	// Token: 0x04003B89 RID: 15241
	public float[] threadSmoothCorrectionStartTime;

	// Token: 0x04003B8A RID: 15242
	public float[] threadSmoothCorrectionStopTime;

	// Token: 0x04003B8B RID: 15243
	public float[] threadSmoothCorrectionTime;

	// Token: 0x04003B8C RID: 15244
	public float mainThreadRenormalizeStartTime;

	// Token: 0x04003B8D RID: 15245
	public float mainThreadRenormalizeTime;

	// Token: 0x04003B8E RID: 15246
	public float mainThreadRenormalizeStopTime;

	// Token: 0x04003B8F RID: 15247
	public float[] threadRenormalizeStartTime;

	// Token: 0x04003B90 RID: 15248
	public float[] threadRenormalizeTime;

	// Token: 0x04003B91 RID: 15249
	public float[] threadRenormalizeStopTime;

	// Token: 0x04003B92 RID: 15250
	protected float bulgeScale = 0.007f;

	// Token: 0x04003B93 RID: 15251
	protected float smoothCorrectionBeta = 0.5f;

	// Token: 0x04003B94 RID: 15252
	protected DAZSkinTaskInfo[] tasks;

	// Token: 0x04003B95 RID: 15253
	protected bool _threadsRunning;

	// Token: 0x04003B96 RID: 15254
	protected Stopwatch stopwatch;
}
