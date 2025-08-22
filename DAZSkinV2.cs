using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Skinner.Scripts.Providers;
using MeshVR;
using MVR;
using SimpleJSON;
using UnityEngine;

// Token: 0x02000B09 RID: 2825
[ExecuteInEditMode]
public class DAZSkinV2 : PreCalcMeshProvider, RenderSuspend
{
	// Token: 0x06004CB5 RID: 19637 RVA: 0x00190048 File Offset: 0x0018E448
	public DAZSkinV2()
	{
	}

	// Token: 0x06004CB6 RID: 19638 RVA: 0x001900F7 File Offset: 0x0018E4F7
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

	// Token: 0x06004CB7 RID: 19639 RVA: 0x00190128 File Offset: 0x0018E528
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

	// Token: 0x17000ADF RID: 2783
	// (get) Token: 0x06004CB8 RID: 19640 RVA: 0x00190198 File Offset: 0x0018E598
	public override Mesh Mesh
	{
		get
		{
			this.InitMesh();
			return this.mesh;
		}
	}

	// Token: 0x17000AE0 RID: 2784
	// (get) Token: 0x06004CB9 RID: 19641 RVA: 0x001901A6 File Offset: 0x0018E5A6
	public override Mesh BaseMesh
	{
		get
		{
			this.InitMesh();
			if (this.dazMesh != null)
			{
				return this.dazMesh.baseMesh;
			}
			return null;
		}
	}

	// Token: 0x17000AE1 RID: 2785
	// (get) Token: 0x06004CBA RID: 19642 RVA: 0x001901CC File Offset: 0x0018E5CC
	public override Mesh MeshForImport
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000AE2 RID: 2786
	// (get) Token: 0x06004CBB RID: 19643 RVA: 0x001901CF File Offset: 0x0018E5CF
	public override GpuBuffer<Matrix4x4> ToWorldMatricesBuffer
	{
		get
		{
			this.InitMesh();
			this.SkinMeshGPUInit();
			return this._ToWorldMatricesBuffer;
		}
	}

	// Token: 0x17000AE3 RID: 2787
	// (get) Token: 0x06004CBC RID: 19644 RVA: 0x001901E3 File Offset: 0x0018E5E3
	// (set) Token: 0x06004CBD RID: 19645 RVA: 0x001901F7 File Offset: 0x0018E5F7
	public override GpuBuffer<Vector3> PreCalculatedVerticesBuffer
	{
		get
		{
			this.InitMesh();
			this.SkinMeshGPUInit();
			return this._PreCalculatedVerticesBuffer;
		}
		protected set
		{
			this._PreCalculatedVerticesBuffer = value;
		}
	}

	// Token: 0x17000AE4 RID: 2788
	// (get) Token: 0x06004CBE RID: 19646 RVA: 0x00190200 File Offset: 0x0018E600
	// (set) Token: 0x06004CBF RID: 19647 RVA: 0x00190214 File Offset: 0x0018E614
	public override GpuBuffer<Vector3> NormalsBuffer
	{
		get
		{
			this.InitMesh();
			this.SkinMeshGPUInit();
			return this._NormalsBuffer;
		}
		protected set
		{
			this._NormalsBuffer = value;
		}
	}

	// Token: 0x06004CC0 RID: 19648 RVA: 0x00190220 File Offset: 0x0018E620
	public override void Dispatch()
	{
		if (this.needsDispatch && this.smoothedVertsBuffer != null)
		{
			this.needsDispatch = false;
			this.GPUSkinner.SetBuffer(this._copyKernel, "inVerts", this.smoothedVertsBuffer);
			this.GPUSkinner.SetBuffer(this._copyKernel, "outVerts", this.preCalcVertsBuffer);
			this.GPUSkinner.Dispatch(this._copyKernel, this.numVertThreadGroups, 1, 1);
			if (this.provideToWorldMatrices)
			{
				this.GPUSkinner.SetBuffer(this._calcChangeMatricesKernel, "originalPositions", this._originalVerticesBuffer);
				this.GPUSkinner.SetBuffer(this._calcChangeMatricesKernel, "originalNormals", this._originalNormalsBuffer);
				this.GPUSkinner.SetBuffer(this._calcChangeMatricesKernel, "originalTangents", this._originalTangentsBuffer);
				this.GPUSkinner.SetBuffer(this._calcChangeMatricesKernel, "currentPositions", this.preCalcVertsBuffer);
				this.GPUSkinner.SetBuffer(this._calcChangeMatricesKernel, "currentNormals", this._normalsBuffer);
				this.GPUSkinner.SetBuffer(this._calcChangeMatricesKernel, "currentTangents", this._tangentsBuffer);
				this.GPUSkinner.SetBuffer(this._calcChangeMatricesKernel, "vertexChangeMatrices", this._matricesBuffer);
				this.GPUSkinner.Dispatch(this._calcChangeMatricesKernel, this.numVertThreadGroups, 1, 1);
			}
		}
	}

	// Token: 0x06004CC1 RID: 19649 RVA: 0x00190383 File Offset: 0x0018E783
	public override void Dispose()
	{
		this._updateDrawDisabled = false;
	}

	// Token: 0x06004CC2 RID: 19650 RVA: 0x0019038C File Offset: 0x0018E78C
	public override void Stop()
	{
		this._updateDrawDisabled = false;
	}

	// Token: 0x06004CC3 RID: 19651 RVA: 0x00190395 File Offset: 0x0018E795
	public override void PostProcessDispatch(ComputeBuffer finalVerts)
	{
	}

	// Token: 0x17000AE5 RID: 2789
	// (get) Token: 0x06004CC4 RID: 19652 RVA: 0x00190397 File Offset: 0x0018E797
	public override Color[] VertexSimColors
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000AE6 RID: 2790
	// (get) Token: 0x06004CC5 RID: 19653 RVA: 0x0019039A File Offset: 0x0018E79A
	// (set) Token: 0x06004CC6 RID: 19654 RVA: 0x001903A2 File Offset: 0x0018E7A2
	public bool renderSuspend
	{
		get
		{
			return this._renderSuspend;
		}
		set
		{
			this._renderSuspend = value;
		}
	}

	// Token: 0x17000AE7 RID: 2791
	// (get) Token: 0x06004CC7 RID: 19655 RVA: 0x001903AB File Offset: 0x0018E7AB
	// (set) Token: 0x06004CC8 RID: 19656 RVA: 0x001903B3 File Offset: 0x0018E7B3
	public DAZSkinV2.PhysicsType physicsType
	{
		get
		{
			return this._physicsType;
		}
		set
		{
			if (value != this._physicsType)
			{
				this._physicsType = value;
				this.InitPhysicsObjects();
			}
		}
	}

	// Token: 0x17000AE8 RID: 2792
	// (get) Token: 0x06004CC9 RID: 19657 RVA: 0x001903CE File Offset: 0x0018E7CE
	public bool hasGeneralWeights
	{
		get
		{
			return this._hasGeneralWeights;
		}
	}

	// Token: 0x17000AE9 RID: 2793
	// (get) Token: 0x06004CCA RID: 19658 RVA: 0x001903D6 File Offset: 0x0018E7D6
	// (set) Token: 0x06004CCB RID: 19659 RVA: 0x001903DE File Offset: 0x0018E7DE
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

	// Token: 0x17000AEA RID: 2794
	// (get) Token: 0x06004CCC RID: 19660 RVA: 0x001903E7 File Offset: 0x0018E7E7
	// (set) Token: 0x06004CCD RID: 19661 RVA: 0x001903EF File Offset: 0x0018E7EF
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

	// Token: 0x17000AEB RID: 2795
	// (get) Token: 0x06004CCE RID: 19662 RVA: 0x001903F8 File Offset: 0x0018E7F8
	// (set) Token: 0x06004CCF RID: 19663 RVA: 0x00190400 File Offset: 0x0018E800
	public bool recalculateTangents
	{
		get
		{
			return this._recalculateTangents;
		}
		set
		{
			this._recalculateTangents = value;
		}
	}

	// Token: 0x17000AEC RID: 2796
	// (get) Token: 0x06004CD0 RID: 19664 RVA: 0x00190409 File Offset: 0x0018E809
	// (set) Token: 0x06004CD1 RID: 19665 RVA: 0x00190411 File Offset: 0x0018E811
	public bool recalculateNormals
	{
		get
		{
			return this._recalculateNormals;
		}
		set
		{
			this._recalculateNormals = value;
		}
	}

	// Token: 0x17000AED RID: 2797
	// (get) Token: 0x06004CD2 RID: 19666 RVA: 0x0019041A File Offset: 0x0018E81A
	public int numBones
	{
		get
		{
			return this._numBones;
		}
	}

	// Token: 0x06004CD3 RID: 19667 RVA: 0x00190422 File Offset: 0x0018E822
	public void ImportStart()
	{
		this.importNodes = new List<DAZSkinV2Node>();
	}

	// Token: 0x06004CD4 RID: 19668 RVA: 0x00190430 File Offset: 0x0018E830
	public void ImportNode(JSONNode jn, string url)
	{
		DAZSkinV2Node dazskinV2Node = new DAZSkinV2Node();
		dazskinV2Node.url = url;
		dazskinV2Node.id = jn["id"];
		dazskinV2Node.name = jn["name"];
		string text = jn["rotation_order"];
		Quaternion2Angles.RotationOrder rotationOrder;
		if (text != null)
		{
			if (text == "XYZ")
			{
				rotationOrder = Quaternion2Angles.RotationOrder.ZYX;
				goto IL_F6;
			}
			if (text == "XZY")
			{
				rotationOrder = Quaternion2Angles.RotationOrder.YZX;
				goto IL_F6;
			}
			if (text == "YXZ")
			{
				rotationOrder = Quaternion2Angles.RotationOrder.ZXY;
				goto IL_F6;
			}
			if (text == "YZX")
			{
				rotationOrder = Quaternion2Angles.RotationOrder.XZY;
				goto IL_F6;
			}
			if (text == "ZXY")
			{
				rotationOrder = Quaternion2Angles.RotationOrder.YXZ;
				goto IL_F6;
			}
			if (text == "ZYX")
			{
				rotationOrder = Quaternion2Angles.RotationOrder.XYZ;
				goto IL_F6;
			}
		}
		UnityEngine.Debug.LogError("Bad rotation order in json: " + text);
		rotationOrder = Quaternion2Angles.RotationOrder.XYZ;
		IL_F6:
		dazskinV2Node.rotationOrder = rotationOrder;
		bool flag = false;
		if (this.importNodes == null)
		{
			this.importNodes = new List<DAZSkinV2Node>();
		}
		for (int i = 0; i < this.importNodes.Count; i++)
		{
			if (this.importNodes[i].url == dazskinV2Node.url)
			{
				this.importNodes[i] = dazskinV2Node;
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			this.importNodes.Add(dazskinV2Node);
		}
	}

	// Token: 0x06004CD5 RID: 19669 RVA: 0x001905BC File Offset: 0x0018E9BC
	protected DAZSkinV2Node FindNodeByUrl(string url)
	{
		string text = url;
		if (Regex.IsMatch(url, "^#"))
		{
			text = DAZImport.DAZurlToPathKey(this.skinUrl) + url;
		}
		for (int i = 0; i < this.importNodes.Count; i++)
		{
			if (this.importNodes[i].url == text)
			{
				return this.importNodes[i];
			}
		}
		UnityEngine.Debug.LogError("Could not find node by url " + text);
		return null;
	}

	// Token: 0x06004CD6 RID: 19670 RVA: 0x00190644 File Offset: 0x0018EA44
	protected DAZSkinV2Node FindNodeById(string id)
	{
		for (int i = 0; i < this.importNodes.Count; i++)
		{
			if (this.importNodes[i].id == id)
			{
				return this.importNodes[i];
			}
		}
		UnityEngine.Debug.LogError("Could not find node by id " + id);
		return null;
	}

	// Token: 0x06004CD7 RID: 19671 RVA: 0x001906A8 File Offset: 0x0018EAA8
	protected DAZSkinV2Node FindNodeByName(string name)
	{
		for (int i = 0; i < this.importNodes.Count; i++)
		{
			if (this.importNodes[i].name == name)
			{
				return this.importNodes[i];
			}
		}
		UnityEngine.Debug.LogError("Could not find node by name " + name);
		return null;
	}

	// Token: 0x06004CD8 RID: 19672 RVA: 0x0019070B File Offset: 0x0018EB0B
	protected Dictionary<int, DAZSkinV2VertexWeights> WalkBonesAndAccumulateWeights(Transform bone)
	{
		this.accumlationStarted = false;
		this.vertexDoneAccumulating = new Dictionary<int, bool>();
		return this.WalkBonesAndAccumulateWeightsRecursive(bone);
	}

	// Token: 0x06004CD9 RID: 19673 RVA: 0x00190728 File Offset: 0x0018EB28
	protected Dictionary<int, DAZSkinV2VertexWeights> WalkBonesAndAccumulateWeightsRecursive(Transform bone)
	{
		Dictionary<int, DAZSkinV2VertexWeights> dictionary;
		if (this.boneWeightsMap.TryGetValue(bone.name, out dictionary))
		{
			this.accumlationStarted = true;
			IEnumerator enumerator = bone.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform bone2 = (Transform)obj;
					Dictionary<int, DAZSkinV2VertexWeights> dictionary2 = this.WalkBonesAndAccumulateWeightsRecursive(bone2);
					if (dictionary2 != null)
					{
						foreach (int key in dictionary2.Keys)
						{
							DAZSkinV2VertexWeights dazskinV2VertexWeights;
							if (!this.vertexDoneAccumulating.ContainsKey(key) && dictionary2.TryGetValue(key, out dazskinV2VertexWeights))
							{
								DAZSkinV2VertexWeights dazskinV2VertexWeights2;
								if (dictionary.TryGetValue(key, out dazskinV2VertexWeights2))
								{
									dazskinV2VertexWeights2.xweight += dazskinV2VertexWeights.xweight;
									dazskinV2VertexWeights2.yweight += dazskinV2VertexWeights.yweight;
									dazskinV2VertexWeights2.zweight += dazskinV2VertexWeights.zweight;
									if (dazskinV2VertexWeights2.xweight > 0.99999f && dazskinV2VertexWeights2.yweight > 0.99999f && dazskinV2VertexWeights2.zweight > 0.99999f)
									{
										dazskinV2VertexWeights2.xweight = 1f;
										dazskinV2VertexWeights2.yweight = 1f;
										dazskinV2VertexWeights2.zweight = 1f;
										this.vertexDoneAccumulating.Add(key, true);
									}
									dictionary.Remove(key);
									dictionary.Add(key, dazskinV2VertexWeights2);
								}
								else if (dazskinV2VertexWeights.xweight > 0.99999f && dazskinV2VertexWeights.yweight > 0.99999f && dazskinV2VertexWeights.zweight > 0.99999f)
								{
									dazskinV2VertexWeights.xweight = 1f;
									dazskinV2VertexWeights.yweight = 1f;
									dazskinV2VertexWeights.zweight = 1f;
									this.vertexDoneAccumulating.Add(key, true);
								}
								else
								{
									DAZSkinV2VertexWeights dazskinV2VertexWeights3 = new DAZSkinV2VertexWeights();
									dazskinV2VertexWeights3.vertex = dazskinV2VertexWeights.vertex;
									dazskinV2VertexWeights3.xweight = dazskinV2VertexWeights.xweight;
									dazskinV2VertexWeights3.yweight = dazskinV2VertexWeights.yweight;
									dazskinV2VertexWeights3.zweight = dazskinV2VertexWeights.zweight;
									dictionary.Add(key, dazskinV2VertexWeights3);
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
		if (!this.accumlationStarted)
		{
			IEnumerator enumerator3 = bone.GetEnumerator();
			try
			{
				while (enumerator3.MoveNext())
				{
					object obj2 = enumerator3.Current;
					Transform bone3 = (Transform)obj2;
					Dictionary<int, DAZSkinV2VertexWeights> dictionary3 = this.WalkBonesAndAccumulateWeightsRecursive(bone3);
					if (dictionary3 != null)
					{
						return dictionary3;
					}
				}
			}
			finally
			{
				IDisposable disposable2;
				if ((disposable2 = (enumerator3 as IDisposable)) != null)
				{
					disposable2.Dispose();
				}
			}
			return null;
		}
		return null;
	}

	// Token: 0x06004CDA RID: 19674 RVA: 0x00190A38 File Offset: 0x0018EE38
	protected void CreateBoneWeightsArray()
	{
		foreach (string key in this.boneWeightsMap.Keys)
		{
			Dictionary<int, DAZSkinV2VertexWeights> dictionary;
			int num;
			if (this.boneWeightsMap.TryGetValue(key, out dictionary) && this.boneNameToIndexMap.TryGetValue(key, out num))
			{
				DAZSkinV2Node dazskinV2Node = this.nodes[num];
				List<DAZSkinV2VertexWeights> list = new List<DAZSkinV2VertexWeights>();
				List<int> list2 = new List<int>();
				foreach (int key2 in dictionary.Keys)
				{
					DAZSkinV2VertexWeights dazskinV2VertexWeights;
					if (dictionary.TryGetValue(key2, out dazskinV2VertexWeights))
					{
						if (dazskinV2VertexWeights.xweight == 1f && dazskinV2VertexWeights.yweight == 1f && dazskinV2VertexWeights.zweight == 1f)
						{
							list2.Add(dazskinV2VertexWeights.vertex);
						}
						else
						{
							list.Add(dazskinV2VertexWeights);
						}
					}
				}
				dazskinV2Node.weights = list.ToArray();
				dazskinV2Node.fullyWeightedVertices = list2.ToArray();
			}
		}
		foreach (string key3 in this.boneGeneralWeightsMap.Keys)
		{
			Dictionary<int, DAZSkinV2GeneralVertexWeights> dictionary2;
			if (this.boneGeneralWeightsMap.TryGetValue(key3, out dictionary2))
			{
				int num2 = 0;
				int num3;
				if (this.boneNameToIndexMap.TryGetValue(key3, out num3))
				{
					DAZSkinV2Node dazskinV2Node2 = this.nodes[num3];
					dazskinV2Node2.generalWeights = new DAZSkinV2GeneralVertexWeights[dictionary2.Count];
					foreach (int key4 in dictionary2.Keys)
					{
						DAZSkinV2GeneralVertexWeights dazskinV2GeneralVertexWeights;
						if (dictionary2.TryGetValue(key4, out dazskinV2GeneralVertexWeights))
						{
							dazskinV2Node2.generalWeights[num2] = dazskinV2GeneralVertexWeights;
							num2++;
						}
					}
				}
			}
		}
	}

	// Token: 0x06004CDB RID: 19675 RVA: 0x00190CC0 File Offset: 0x0018F0C0
	public void CheckGeneralWeights()
	{
		int numBaseVertices = this.dazMesh.numBaseVertices;
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"Num base verts:",
			numBaseVertices,
			" Num UV verts:",
			this.dazMesh.numUVVertices
		}));
		float[] array = new float[numBaseVertices];
		for (int i = 0; i < numBaseVertices; i++)
		{
			array[i] = 0f;
		}
		foreach (DAZSkinV2Node dazskinV2Node in this.nodes)
		{
			foreach (DAZSkinV2GeneralVertexWeights dazskinV2GeneralVertexWeights in dazskinV2Node.generalWeights)
			{
				if (dazskinV2GeneralVertexWeights.vertex < numBaseVertices)
				{
					array[dazskinV2GeneralVertexWeights.vertex] += dazskinV2GeneralVertexWeights.weight;
				}
				else
				{
					UnityEngine.Debug.LogError("Vertex " + dazskinV2GeneralVertexWeights.vertex + " in generalWeights is out of range");
				}
			}
		}
		for (int l = 0; l < numBaseVertices; l++)
		{
			if (array[l] < 0.999f)
			{
				UnityEngine.Debug.LogError("Vertex " + l + " weights don't add up to 1");
			}
		}
	}

	// Token: 0x06004CDC RID: 19676 RVA: 0x00190E10 File Offset: 0x0018F210
	public void ReassignDAZMesh()
	{
		DAZMesh[] components = base.GetComponents<DAZMesh>();
		this.dazMesh = null;
		foreach (DAZMesh dazmesh in components)
		{
			if (dazmesh.sceneGeometryId == this.sceneGeometryId)
			{
				this.dazMesh = dazmesh;
				break;
			}
		}
		if (this.dazMesh == null)
		{
			UnityEngine.Debug.LogError("Could not find DAZMesh component with geometryID " + this.sceneGeometryId);
			return;
		}
	}

	// Token: 0x06004CDD RID: 19677 RVA: 0x00190E90 File Offset: 0x0018F290
	public void Import(JSONNode jn)
	{
		if (this.root == null)
		{
			UnityEngine.Debug.LogError("Root bone not set. Can't import skin");
			return;
		}
		JSONNode jsonnode = jn["skin"]["joints"];
		this._numBones = jsonnode.Count;
		this.ReassignDAZMesh();
		Dictionary<int, List<int>> baseVertToUVVertFullMap = this.dazMesh.baseVertToUVVertFullMap;
		this.InitPhysicsObjects();
		this.boneNameToIndexMap = new Dictionary<string, int>();
		this.boneWeightsMap = new Dictionary<string, Dictionary<int, DAZSkinV2VertexWeights>>();
		this.boneGeneralWeightsMap = new Dictionary<string, Dictionary<int, DAZSkinV2GeneralVertexWeights>>();
		this.nodes = new DAZSkinV2Node[this._numBones];
		int num = 0;
		IEnumerator enumerator = jsonnode.AsArray.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				JSONNode jsonnode2 = (JSONNode)obj;
				DAZSkinV2Node dazskinV2Node = this.FindNodeByUrl(DAZImport.DAZurlFix(jsonnode2["node"]));
				string name = dazskinV2Node.name;
				this.nodes[num] = dazskinV2Node;
				this.boneNameToIndexMap.Add(dazskinV2Node.name, num);
				Dictionary<int, DAZSkinV2VertexWeights> dictionary = new Dictionary<int, DAZSkinV2VertexWeights>();
				Dictionary<int, DAZSkinV2GeneralVertexWeights> dictionary2 = new Dictionary<int, DAZSkinV2GeneralVertexWeights>();
				IEnumerator enumerator2 = jsonnode2["node_weights"]["values"].AsArray.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						object obj2 = enumerator2.Current;
						JSONNode jsonnode3 = (JSONNode)obj2;
						this._hasGeneralWeights = true;
						int asInt = jsonnode3[0].AsInt;
						float asFloat = jsonnode3[1].AsFloat;
						List<int> list;
						if (baseVertToUVVertFullMap.TryGetValue(asInt, out list))
						{
							foreach (int num2 in list)
							{
								DAZSkinV2GeneralVertexWeights dazskinV2GeneralVertexWeights;
								if (dictionary2.TryGetValue(num2, out dazskinV2GeneralVertexWeights))
								{
									dazskinV2GeneralVertexWeights.weight = asFloat;
									dictionary2.Remove(num2);
									dictionary2.Add(num2, dazskinV2GeneralVertexWeights);
								}
								else
								{
									dazskinV2GeneralVertexWeights = new DAZSkinV2GeneralVertexWeights();
									dazskinV2GeneralVertexWeights.vertex = num2;
									dazskinV2GeneralVertexWeights.weight = asFloat;
									dictionary2.Add(num2, dazskinV2GeneralVertexWeights);
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
								DAZSkinV2VertexWeights dazskinV2VertexWeights;
								if (dictionary.TryGetValue(num3, out dazskinV2VertexWeights))
								{
									dazskinV2VertexWeights.xweight = asFloat2;
									dictionary.Remove(num3);
									dictionary.Add(num3, dazskinV2VertexWeights);
								}
								else
								{
									dazskinV2VertexWeights = new DAZSkinV2VertexWeights();
									dazskinV2VertexWeights.vertex = num3;
									dazskinV2VertexWeights.xweight = asFloat2;
									dictionary.Add(num3, dazskinV2VertexWeights);
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
								DAZSkinV2VertexWeights dazskinV2VertexWeights2;
								if (dictionary.TryGetValue(num4, out dazskinV2VertexWeights2))
								{
									dazskinV2VertexWeights2.yweight = asFloat3;
									dictionary.Remove(num4);
									dictionary.Add(num4, dazskinV2VertexWeights2);
								}
								else
								{
									dazskinV2VertexWeights2 = new DAZSkinV2VertexWeights();
									dazskinV2VertexWeights2.vertex = num4;
									dazskinV2VertexWeights2.yweight = asFloat3;
									dictionary.Add(num4, dazskinV2VertexWeights2);
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
								DAZSkinV2VertexWeights dazskinV2VertexWeights3;
								if (dictionary.TryGetValue(num5, out dazskinV2VertexWeights3))
								{
									dazskinV2VertexWeights3.zweight = asFloat4;
									dictionary.Remove(num5);
									dictionary.Add(num5, dazskinV2VertexWeights3);
								}
								else
								{
									dazskinV2VertexWeights3 = new DAZSkinV2VertexWeights();
									dazskinV2VertexWeights3.vertex = num5;
									dazskinV2VertexWeights3.zweight = asFloat4;
									dictionary.Add(num5, dazskinV2VertexWeights3);
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
				DAZSkinV2BulgeFactors dazskinV2BulgeFactors = new DAZSkinV2BulgeFactors();
				dazskinV2BulgeFactors.name = name;
				IEnumerator enumerator10 = jsonnode2["bulge_weights"]["x"]["bulges"].AsArray.GetEnumerator();
				try
				{
					while (enumerator10.MoveNext())
					{
						object obj6 = enumerator10.Current;
						JSONNode jsonnode7 = (JSONNode)obj6;
						string text = jsonnode7["id"];
						if (text != null)
						{
							if (!(text == "positive-left"))
							{
								if (!(text == "positive-right"))
								{
									if (!(text == "negative-left"))
									{
										if (text == "negative-right")
										{
											dazskinV2BulgeFactors.xnegright = -jsonnode7["value"].AsFloat;
										}
									}
									else
									{
										dazskinV2BulgeFactors.xnegleft = -jsonnode7["value"].AsFloat;
									}
								}
								else
								{
									dazskinV2BulgeFactors.xposright = jsonnode7["value"].AsFloat;
								}
							}
							else
							{
								dazskinV2BulgeFactors.xposleft = jsonnode7["value"].AsFloat;
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
								DAZSkinV2VertexWeights dazskinV2VertexWeights4;
								if (dictionary.TryGetValue(num6, out dazskinV2VertexWeights4))
								{
									dazskinV2VertexWeights4.xleftbulge = asFloat5;
									dictionary.Remove(num6);
									dictionary.Add(num6, dazskinV2VertexWeights4);
								}
								else
								{
									dazskinV2VertexWeights4 = new DAZSkinV2VertexWeights();
									dazskinV2VertexWeights4.vertex = num6;
									dazskinV2VertexWeights4.xleftbulge = asFloat5;
									dictionary.Add(num6, dazskinV2VertexWeights4);
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
								DAZSkinV2VertexWeights dazskinV2VertexWeights5;
								if (dictionary.TryGetValue(num7, out dazskinV2VertexWeights5))
								{
									dazskinV2VertexWeights5.xrightbulge = asFloat6;
									dictionary.Remove(num7);
									dictionary.Add(num7, dazskinV2VertexWeights5);
								}
								else
								{
									dazskinV2VertexWeights5 = new DAZSkinV2VertexWeights();
									dazskinV2VertexWeights5.vertex = num7;
									dazskinV2VertexWeights5.xrightbulge = asFloat6;
									dictionary.Add(num7, dazskinV2VertexWeights5);
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
						string text2 = jsonnode10["id"];
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
											dazskinV2BulgeFactors.yposright = jsonnode10["value"].AsFloat;
										}
									}
									else
									{
										dazskinV2BulgeFactors.yposleft = jsonnode10["value"].AsFloat;
									}
								}
								else
								{
									dazskinV2BulgeFactors.ynegright = -jsonnode10["value"].AsFloat;
								}
							}
							else
							{
								dazskinV2BulgeFactors.ynegleft = -jsonnode10["value"].AsFloat;
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
								DAZSkinV2VertexWeights dazskinV2VertexWeights6;
								if (dictionary.TryGetValue(num8, out dazskinV2VertexWeights6))
								{
									dazskinV2VertexWeights6.yleftbulge = asFloat7;
									dictionary.Remove(num8);
									dictionary.Add(num8, dazskinV2VertexWeights6);
								}
								else
								{
									dazskinV2VertexWeights6 = new DAZSkinV2VertexWeights();
									dazskinV2VertexWeights6.vertex = num8;
									dazskinV2VertexWeights6.yleftbulge = asFloat7;
									dictionary.Add(num8, dazskinV2VertexWeights6);
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
								DAZSkinV2VertexWeights dazskinV2VertexWeights7;
								if (dictionary.TryGetValue(num9, out dazskinV2VertexWeights7))
								{
									dazskinV2VertexWeights7.yrightbulge = asFloat8;
									dictionary.Remove(num9);
									dictionary.Add(num9, dazskinV2VertexWeights7);
								}
								else
								{
									dazskinV2VertexWeights7 = new DAZSkinV2VertexWeights();
									dazskinV2VertexWeights7.vertex = num9;
									dazskinV2VertexWeights7.yrightbulge = asFloat8;
									dictionary.Add(num9, dazskinV2VertexWeights7);
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
						string text3 = jsonnode13["id"];
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
											dazskinV2BulgeFactors.zposright = jsonnode13["value"].AsFloat;
										}
									}
									else
									{
										dazskinV2BulgeFactors.zposleft = jsonnode13["value"].AsFloat;
									}
								}
								else
								{
									dazskinV2BulgeFactors.znegright = -jsonnode13["value"].AsFloat;
								}
							}
							else
							{
								dazskinV2BulgeFactors.znegleft = -jsonnode13["value"].AsFloat;
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
								DAZSkinV2VertexWeights dazskinV2VertexWeights8;
								if (dictionary.TryGetValue(num10, out dazskinV2VertexWeights8))
								{
									dazskinV2VertexWeights8.zleftbulge = asFloat9;
									dictionary.Remove(num10);
									dictionary.Add(num10, dazskinV2VertexWeights8);
								}
								else
								{
									dazskinV2VertexWeights8 = new DAZSkinV2VertexWeights();
									dazskinV2VertexWeights8.vertex = num10;
									dazskinV2VertexWeights8.zleftbulge = asFloat9;
									dictionary.Add(num10, dazskinV2VertexWeights8);
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
								DAZSkinV2VertexWeights dazskinV2VertexWeights9;
								if (dictionary.TryGetValue(num11, out dazskinV2VertexWeights9))
								{
									dazskinV2VertexWeights9.zrightbulge = asFloat10;
									dictionary.Remove(num11);
									dictionary.Add(num11, dazskinV2VertexWeights9);
								}
								else
								{
									dazskinV2VertexWeights9 = new DAZSkinV2VertexWeights();
									dazskinV2VertexWeights9.vertex = num11;
									dazskinV2VertexWeights9.zrightbulge = asFloat10;
									dictionary.Add(num11, dazskinV2VertexWeights9);
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
				this.nodes[num].bulgeFactors = dazskinV2BulgeFactors;
				this.boneWeightsMap.Add(name, dictionary);
				this.boneGeneralWeightsMap.Add(name, dictionary2);
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
		this.WalkBonesAndAccumulateWeights(this.root.transform);
		this.CreateBoneWeightsArray();
	}

	// Token: 0x06004CDE RID: 19678 RVA: 0x001921A4 File Offset: 0x001905A4
	protected void CalculateStrongestWeights()
	{
		this.numUVVerts = this.dazMesh.numUVVertices;
		this.strongestDAZBone = new DAZBone[this.numUVVerts];
		this.strongestBoneWeight = new float[this.numUVVerts];
		for (int i = 0; i < this.numBones; i++)
		{
			foreach (DAZSkinV2VertexWeights dazskinV2VertexWeights in this.nodes[i].weights)
			{
				float num = (dazskinV2VertexWeights.xweight + dazskinV2VertexWeights.yweight + dazskinV2VertexWeights.zweight) * 0.33f;
				if (num > this.strongestBoneWeight[dazskinV2VertexWeights.vertex])
				{
					this.strongestBoneWeight[dazskinV2VertexWeights.vertex] = num;
					this.strongestDAZBone[dazskinV2VertexWeights.vertex] = this.dazBones[i];
				}
			}
		}
	}

	// Token: 0x06004CDF RID: 19679 RVA: 0x00192278 File Offset: 0x00190678
	protected void InitBones()
	{
		this.dazBones = new DAZBone[this.numBones];
		this.boneRotationAngles = new Vector3[this.numBones];
		this.boneChangeFromOriginalMatrices = new Matrix4x4[this.numBones];
		this.boneWorldToLocalMatrices = new Matrix4x4[this.numBones];
		this.boneLocalToWorldMatrices = new Matrix4x4[this.numBones];
		if (this.root != null)
		{
			for (int i = 0; i < this.numBones; i++)
			{
				string name = this.nodes[i].name;
				DAZBone dazbone = this.root.GetDAZBone(name);
				if (dazbone != null)
				{
					this.dazBones[i] = dazbone;
				}
				else
				{
					UnityEngine.Debug.LogError("Could not find DazBone for bone " + name + " for skin " + this.sceneGeometryId);
				}
			}
		}
		else
		{
			UnityEngine.Debug.LogError("Could not init bones since root DazBones is not set for skin " + this.sceneGeometryId);
		}
		this.CalculateStrongestWeights();
	}

	// Token: 0x06004CE0 RID: 19680 RVA: 0x00192372 File Offset: 0x00190772
	public Mesh GetMesh()
	{
		return this.mesh;
	}

	// Token: 0x17000AEE RID: 2798
	// (get) Token: 0x06004CE1 RID: 19681 RVA: 0x0019237A File Offset: 0x0019077A
	public int numMaterials
	{
		get
		{
			return this._numMaterials;
		}
	}

	// Token: 0x17000AEF RID: 2799
	// (get) Token: 0x06004CE2 RID: 19682 RVA: 0x00192382 File Offset: 0x00190782
	public string[] materialNames
	{
		get
		{
			return this._materialNames;
		}
	}

	// Token: 0x06004CE3 RID: 19683 RVA: 0x0019238C File Offset: 0x0019078C
	public void CopyMaterials()
	{
		if (this.dazMesh != null)
		{
			this._numMaterials = this.dazMesh.materials.Length;
			this.GPUsimpleMaterial = this.dazMesh.simpleMaterial;
			this.GPUmaterials = new Material[this._numMaterials];
			this.materialsEnabled = new bool[this._numMaterials];
			this.materialsShadowCastEnabled = new bool[this._numMaterials];
			this._materialNames = new string[this._numMaterials];
			for (int i = 0; i < this._numMaterials; i++)
			{
				this.GPUmaterials[i] = this.dazMesh.materials[i];
				this.materialsEnabled[i] = this.dazMesh.materialsEnabled[i];
				this.materialsShadowCastEnabled[i] = this.dazMesh.materialsShadowCastEnabled[i];
				this._materialNames[i] = this.dazMesh.materialNames[i];
			}
		}
	}

	// Token: 0x06004CE4 RID: 19684 RVA: 0x0019247D File Offset: 0x0019087D
	public void SetWorkingVerts(Vector3[] verts)
	{
		this.workingVerts = verts;
	}

	// Token: 0x06004CE5 RID: 19685 RVA: 0x00192488 File Offset: 0x00190888
	protected void InitSmoothing()
	{
		if (this.meshSmooth == null)
		{
			this.meshSmooth = new MeshSmooth(this.dazMesh.baseVertices, this.dazMesh.basePolyList);
		}
		if (this.meshSmoothGPU == null)
		{
			this.meshSmoothGPU = new MeshSmoothGPU(this.GPUMeshCompute, this.dazMesh.baseVertices, this.dazMesh.basePolyList);
		}
	}

	// Token: 0x06004CE6 RID: 19686 RVA: 0x001924F4 File Offset: 0x001908F4
	protected void InitRecalcNormalsTangents()
	{
		if (this._recalculateNormals && this.recalcNormals == null)
		{
			this.recalcNormals = new RecalculateNormals(this.dazMesh.baseTriangles, this.workingVerts2, this.workingNormals, this.workingSurfaceNormals, true);
		}
		if (this.postSkinRecalcNormals == null)
		{
			this.postSkinRecalcNormals = new RecalculateNormals(this.dazMesh.baseTriangles, this.rawSkinnedWorkingVerts, this.postSkinWorkingNormals, this.postSkinWorkingSurfaceNormals, false);
		}
		if (this.skinMethod != DAZSkinV2.SkinMethod.CPU)
		{
			if (this.originalRecalcNormalsGPU == null)
			{
				this.originalRecalcNormalsGPU = new RecalculateNormalsGPU(this.GPUMeshCompute, this.dazMesh.baseTriangles, this.numUVVerts, this.dazMesh.baseVerticesToUVVertices);
				this._originalNormalsBuffer = this.originalRecalcNormalsGPU.normalsBuffer;
				this._originalSurfaceNormalsBuffer = this.originalRecalcNormalsGPU.surfaceNormalsBuffer;
				this.originalRecalcNormalsGPU.RecalculateNormals(this._originalVerticesBuffer);
			}
			if (this.recalcNormalsGPU == null)
			{
				this.recalcNormalsGPU = new RecalculateNormalsGPU(this.GPUMeshCompute, this.dazMesh.baseTriangles, this.numUVVerts, this.dazMesh.baseVerticesToUVVertices);
				this._normalsBuffer = this.recalcNormalsGPU.normalsBuffer;
				this._surfaceNormalsBuffer = this.recalcNormalsGPU.surfaceNormalsBuffer;
				this._normalsBuffer.SetData(this.startNormals);
			}
			if (this.recalcNormalsGPURaw == null)
			{
				this.recalcNormalsGPURaw = new RecalculateNormalsGPU(this.GPUMeshCompute, this.dazMesh.baseTriangles, this.numUVVerts, this.dazMesh.baseVerticesToUVVertices);
				this._surfaceNormalsRawBuffer = this.recalcNormalsGPURaw.surfaceNormalsBuffer;
			}
		}
		if (this._recalculateTangents && this.recalcTangents == null)
		{
			this.recalcTangents = new RecalculateTangents(this.dazMesh.UVTriangles, this.workingVerts2, this.workingNormals, this.dazMesh.UV, this.workingTangents, true);
		}
		if (this.skinMethod != DAZSkinV2.SkinMethod.CPU)
		{
			if (this.originalRecalcTangentsGPU == null)
			{
				this.originalRecalcTangentsGPU = new RecalculateTangentsGPU(this.GPUMeshCompute, this.dazMesh.UVTriangles, this.dazMesh.UV, this.numUVVerts);
				this._originalTangentsBuffer = this.originalRecalcTangentsGPU.tangentsBuffer;
				this.originalRecalcTangentsGPU.RecalculateTangents(this._originalVerticesBuffer, this._originalNormalsBuffer);
			}
			if (this.recalcTangentsGPU == null)
			{
				this.recalcTangentsGPU = new RecalculateTangentsGPU(this.GPUMeshCompute, this.dazMesh.UVTriangles, this.dazMesh.UV, this.numUVVerts);
				this._tangentsBuffer = this.recalcTangentsGPU.tangentsBuffer;
				this._tangentsBuffer.SetData(this.startTangents);
			}
		}
	}

	// Token: 0x06004CE7 RID: 19687 RVA: 0x001927B0 File Offset: 0x00190BB0
	protected void InitMesh()
	{
		if (this.dazMesh != null && !this.meshWasInit)
		{
			this.dazMesh.Init();
			this.meshWasInit = true;
			this.mesh = UnityEngine.Object.Instantiate<Mesh>(this.dazMesh.morphedUVMappedMesh);
			this.RegisterAllocatedObject(this.mesh);
			this.startVerts = this.dazMesh.morphedUVVertices;
			this.startVertsCopy = (Vector3[])this.startVerts.Clone();
			this.numUVVerts = this.dazMesh.numUVVertices;
			this.numBaseVerts = this.dazMesh.numBaseVertices;
			if (this.root != null)
			{
				if (this.dazBones == null)
				{
					this.InitBones();
				}
				for (int i = 0; i < this.numBones; i++)
				{
					foreach (DAZSkinV2VertexWeights dazskinV2VertexWeights in this.nodes[i].weights)
					{
						float num = 1f / Vector3.Distance(this.startVerts[dazskinV2VertexWeights.vertex], this.dazBones[i].worldPosition);
						dazskinV2VertexWeights.xleftbulge *= num;
						dazskinV2VertexWeights.xrightbulge *= num;
						dazskinV2VertexWeights.yleftbulge *= num;
						dazskinV2VertexWeights.yrightbulge *= num;
						dazskinV2VertexWeights.zleftbulge *= num;
						dazskinV2VertexWeights.zrightbulge *= num;
					}
				}
			}
			this.drawVerts = (Vector3[])this.startVerts.Clone();
			this.postSkinVerts = new bool[this.numUVVerts];
			this.postSkinVertsReady = new bool[this.numUVVerts];
			this.postSkinNeededVerts = new bool[this.numUVVerts];
			this.postSkinNeededVertsList = new int[0];
			this.postSkinNormalVerts = new bool[this.numUVVerts];
			this.postSkinMorphs = new Vector3[this.numUVVerts];
			this.workingVerts1 = (Vector3[])this.startVerts.Clone();
			this.workingVerts = this.workingVerts1;
			this.workingVerts2 = (Vector3[])this.startVerts.Clone();
			this.rawSkinnedWorkingVerts = (Vector3[])this.startVerts.Clone();
			this.rawSkinnedVerts = (Vector3[])this.startVerts.Clone();
			this.smoothedVerts = (Vector3[])this.startVerts.Clone();
			this.unsmoothedVerts = (Vector3[])this.startVerts.Clone();
			this.baseVertsToUVVertsCopy = (DAZVertexMap[])this.dazMesh.baseVerticesToUVVertices.Clone();
			this.isBaseVert = new bool[this.numUVVerts];
			for (int k = 0; k < this.numUVVerts; k++)
			{
				this.isBaseVert[k] = true;
			}
			this.numUVOnlyVerts = 0;
			for (int l = 0; l < this.baseVertsToUVVertsCopy.Length; l++)
			{
				DAZVertexMap dazvertexMap = this.baseVertsToUVVertsCopy[l];
				int tovert = dazvertexMap.tovert;
				this.isBaseVert[tovert] = false;
			}
			this.startNormals = this.dazMesh.morphedUVNormals;
			this.drawNormals = (Vector3[])this.startNormals.Clone();
			this.workingNormals = (Vector3[])this.startNormals.Clone();
			this.postSkinWorkingNormals = (Vector3[])this.startNormals.Clone();
			this.postSkinNormals = (Vector3[])this.startNormals.Clone();
			this.workingSurfaceNormals = new Vector3[this.dazMesh.baseTriangles.Length / 3];
			this.postSkinWorkingSurfaceNormals = new Vector3[this.dazMesh.baseTriangles.Length / 3];
			this.drawSurfaceNormals = new Vector3[this.dazMesh.baseTriangles.Length / 3];
			this.startTangents = this.dazMesh.morphedUVTangents;
			this.drawTangents = (Vector4[])this.startTangents.Clone();
			this.workingTangents = (Vector4[])this.startTangents.Clone();
			Vector3 size = new Vector3(10000f, 10000f, 10000f);
			Bounds bounds = new Bounds(base.transform.position, size);
			this.mesh.bounds = bounds;
		}
	}

	// Token: 0x17000AF0 RID: 2800
	// (get) Token: 0x06004CE8 RID: 19688 RVA: 0x00192C04 File Offset: 0x00191004
	// (set) Token: 0x06004CE9 RID: 19689 RVA: 0x00192C0C File Offset: 0x0019100C
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
				bool subThreadsRunning = this._subThreadsRunning;
				if (subThreadsRunning)
				{
					this.StopSubThreads();
				}
				this._numSubThreads = value;
				this.InitSkinTimes();
				if (subThreadsRunning)
				{
					this.StartSubThreads();
				}
			}
		}
	}

	// Token: 0x06004CEA RID: 19690 RVA: 0x00192C51 File Offset: 0x00191051
	public void SetNumSubThreads(int num)
	{
		this.numSubThreads = num;
	}

	// Token: 0x06004CEB RID: 19691 RVA: 0x00192C5C File Offset: 0x0019105C
	public void InitSkinTimes()
	{
		this.threadSkinTime = new float[this._numSubThreads];
		this.threadSkinVertsCount = new int[this._numSubThreads];
		this.threadSkinStartTime = new float[this._numSubThreads];
		this.threadSkinStopTime = new float[this._numSubThreads];
		this.threadSmoothTime = new float[this._numSubThreads];
		this.threadSmoothStartTime = new float[this._numSubThreads];
		this.threadSmoothStopTime = new float[this._numSubThreads];
		this.threadSmoothCorrectionTime = new float[this._numSubThreads];
		this.threadSmoothCorrectionStartTime = new float[this._numSubThreads];
		this.threadSmoothCorrectionStopTime = new float[this._numSubThreads];
	}

	// Token: 0x17000AF1 RID: 2801
	// (get) Token: 0x06004CEC RID: 19692 RVA: 0x00192D13 File Offset: 0x00191113
	public bool threadsRunning
	{
		get
		{
			return this._threadsRunning;
		}
	}

	// Token: 0x06004CED RID: 19693 RVA: 0x00192D1C File Offset: 0x0019111C
	protected void StopSubThreads()
	{
		this._subThreadsRunning = false;
		if (this.tasks != null)
		{
			for (int i = 0; i < this.tasks.Length; i++)
			{
				this.tasks[i].kill = true;
				this.tasks[i].resetEvent.Set();
				while (this.tasks[i].thread.IsAlive)
				{
				}
			}
		}
		this.tasks = null;
	}

	// Token: 0x06004CEE RID: 19694 RVA: 0x00192D9C File Offset: 0x0019119C
	protected void StopThreads()
	{
		this._threadsRunning = false;
		if (this.normalTangentTask != null)
		{
			this.normalTangentTask.kill = true;
			this.normalTangentTask.resetEvent.Set();
			while (this.normalTangentTask.thread.IsAlive)
			{
			}
			this.normalTangentTask = null;
		}
		if (this.mainSkinTask != null)
		{
			this.mainSkinTask.kill = true;
			this.mainSkinTask.resetEvent.Set();
			while (this.mainSkinTask.thread.IsAlive)
			{
			}
			this.mainSkinTask = null;
		}
		if (this.postSkinMorphTask != null)
		{
			this.postSkinMorphTask.kill = true;
			this.postSkinMorphTask.resetEvent.Set();
			while (this.postSkinMorphTask.thread.IsAlive)
			{
			}
			this.postSkinMorphTask = null;
		}
	}

	// Token: 0x06004CEF RID: 19695 RVA: 0x00192E94 File Offset: 0x00191294
	public void StartSubThreads()
	{
		if (!this._subThreadsRunning)
		{
			this._subThreadsRunning = true;
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
					dazskinTaskInfo.thread.Priority = System.Threading.ThreadPriority.AboveNormal;
					dazskinTaskInfo.thread.Start(dazskinTaskInfo);
					this.tasks[i] = dazskinTaskInfo;
				}
			}
		}
	}

	// Token: 0x06004CF0 RID: 19696 RVA: 0x00192F50 File Offset: 0x00191350
	protected void StartThreads()
	{
		if (!this._threadsRunning)
		{
			this._threadsRunning = true;
			this.StartSubThreads();
			this.normalTangentTask = new DAZSkinTaskInfo();
			this.normalTangentTask.threadIndex = 0;
			this.normalTangentTask.name = "NormalTangentTask";
			this.normalTangentTask.resetEvent = new AutoResetEvent(false);
			this.normalTangentTask.thread = new Thread(new ParameterizedThreadStart(this.MTTask));
			this.normalTangentTask.thread.Priority = System.Threading.ThreadPriority.BelowNormal;
			this.normalTangentTask.taskType = DAZSkinTaskType.NormalTangentRecalc;
			this.normalTangentTask.thread.Start(this.normalTangentTask);
			this.mainSkinTask = new DAZSkinTaskInfo();
			this.mainSkinTask.threadIndex = 0;
			this.mainSkinTask.name = "MainSkinTask";
			this.mainSkinTask.resetEvent = new AutoResetEvent(false);
			this.mainSkinTask.thread = new Thread(new ParameterizedThreadStart(this.MTTask));
			this.mainSkinTask.thread.Priority = System.Threading.ThreadPriority.Normal;
			this.mainSkinTask.taskType = DAZSkinTaskType.MainSkin;
			this.mainSkinTask.thread.Start(this.mainSkinTask);
			this.postSkinMorphTask = new DAZSkinTaskInfo();
			this.postSkinMorphTask.threadIndex = 0;
			this.postSkinMorphTask.name = "PostSkinMorphTask";
			this.postSkinMorphTask.resetEvent = new AutoResetEvent(false);
			this.postSkinMorphTask.thread = new Thread(new ParameterizedThreadStart(this.MTTask));
			this.postSkinMorphTask.thread.Priority = System.Threading.ThreadPriority.Normal;
			this.postSkinMorphTask.taskType = DAZSkinTaskType.PostSkinMorph;
			this.postSkinMorphTask.thread.Start(this.postSkinMorphTask);
		}
	}

	// Token: 0x06004CF1 RID: 19697 RVA: 0x0019310C File Offset: 0x0019150C
	protected void MTTask(object info)
	{
		DAZSkinTaskInfo dazskinTaskInfo = (DAZSkinTaskInfo)info;
		while (this._threadsRunning || this._subThreadsRunning)
		{
			dazskinTaskInfo.resetEvent.WaitOne(-1, true);
			if (dazskinTaskInfo.kill)
			{
				break;
			}
			if (dazskinTaskInfo.taskType == DAZSkinTaskType.Skin)
			{
				this.threadSkinStartTime[dazskinTaskInfo.threadIndex] = (float)this.stopwatch.ElapsedTicks * this.f;
				this.SkinMeshPart(dazskinTaskInfo.index1, dazskinTaskInfo.index2, this.isBaseVert, null);
				this.threadSkinStopTime[dazskinTaskInfo.threadIndex] = (float)this.stopwatch.ElapsedTicks * this.f;
				this.threadSkinTime[dazskinTaskInfo.threadIndex] = this.threadSkinStopTime[dazskinTaskInfo.threadIndex] - this.threadSkinStartTime[dazskinTaskInfo.threadIndex];
			}
			else if (dazskinTaskInfo.taskType == DAZSkinTaskType.PostSkin)
			{
				this.threadSkinStartTime[dazskinTaskInfo.threadIndex] = (float)this.stopwatch.ElapsedTicks * this.f;
				this.SkinMeshPart(dazskinTaskInfo.index1, dazskinTaskInfo.index2, this.postSkinNeededVerts, this.postSkinBones);
				this.threadSkinStopTime[dazskinTaskInfo.threadIndex] = (float)this.stopwatch.ElapsedTicks * this.f;
				this.threadSkinTime[dazskinTaskInfo.threadIndex] = this.threadSkinStopTime[dazskinTaskInfo.threadIndex] - this.threadSkinStartTime[dazskinTaskInfo.threadIndex];
			}
			else if (dazskinTaskInfo.taskType == DAZSkinTaskType.Smooth)
			{
				this.threadSmoothStartTime[dazskinTaskInfo.threadIndex] = (float)this.stopwatch.ElapsedTicks * this.f;
				this.meshSmooth.LaplacianSmooth(this.workingVerts, this.smoothedVerts, dazskinTaskInfo.index1, dazskinTaskInfo.index2);
				this.threadSmoothStopTime[dazskinTaskInfo.threadIndex] = (float)this.stopwatch.ElapsedTicks * this.f;
				this.threadSmoothTime[dazskinTaskInfo.threadIndex] = this.threadSmoothStopTime[dazskinTaskInfo.threadIndex] - this.threadSmoothStartTime[dazskinTaskInfo.threadIndex];
			}
			else if (dazskinTaskInfo.taskType == DAZSkinTaskType.SmoothCorrection)
			{
				this.threadSmoothCorrectionStartTime[dazskinTaskInfo.threadIndex] = (float)this.stopwatch.ElapsedTicks * this.f;
				this.meshSmooth.HCCorrection(this.workingVerts, this.smoothedVerts, this.laplacianSmoothBeta, dazskinTaskInfo.index1, dazskinTaskInfo.index2);
				this.threadSmoothCorrectionStopTime[dazskinTaskInfo.threadIndex] = (float)this.stopwatch.ElapsedTicks * this.f;
				this.threadSmoothCorrectionTime[dazskinTaskInfo.threadIndex] = this.threadSmoothCorrectionStopTime[dazskinTaskInfo.threadIndex] - this.threadSmoothCorrectionStartTime[dazskinTaskInfo.threadIndex];
			}
			else if (dazskinTaskInfo.taskType == DAZSkinTaskType.NormalTangentRecalc)
			{
				this.threadRecalcNormalTangentStartTime = (float)this.stopwatch.ElapsedTicks * this.f;
				Thread.Sleep(0);
				this.NormalTangentRecalc();
				this.threadRecalcNormalTangentStopTime = (float)this.stopwatch.ElapsedTicks * this.f;
				this.threadRecalcNormalTangentTime = this.threadRecalcNormalTangentStopTime - this.threadRecalcNormalTangentStartTime;
			}
			else if (dazskinTaskInfo.taskType == DAZSkinTaskType.MainSkin)
			{
				Thread.Sleep(0);
				this.SkinMeshThreaded(false);
			}
			else if (dazskinTaskInfo.taskType == DAZSkinTaskType.PostSkinMorph)
			{
				Thread.Sleep(0);
				this.SkinMeshGPUPostSkinVertsThreaded();
			}
			dazskinTaskInfo.working = false;
		}
	}

	// Token: 0x06004CF2 RID: 19698 RVA: 0x00193464 File Offset: 0x00191864
	protected void NormalTangentRecalc()
	{
		if (this._recalculateNormals)
		{
			this.recalcNormals.recalculateNormals(null);
			foreach (DAZVertexMap dazvertexMap in this.baseVertsToUVVertsCopy)
			{
				this.workingNormals[dazvertexMap.tovert] = this.workingNormals[dazvertexMap.fromvert];
			}
		}
		if (this._recalculateTangents)
		{
			this.recalcTangents.recalculateTangents(null);
		}
	}

	// Token: 0x17000AF2 RID: 2802
	// (get) Token: 0x06004CF3 RID: 19699 RVA: 0x001934EA File Offset: 0x001918EA
	public ComputeBuffer normalsBuffer
	{
		get
		{
			return this._normalsBuffer;
		}
	}

	// Token: 0x17000AF3 RID: 2803
	// (get) Token: 0x06004CF4 RID: 19700 RVA: 0x001934F2 File Offset: 0x001918F2
	public ComputeBuffer tangentsBuffer
	{
		get
		{
			return this._tangentsBuffer;
		}
	}

	// Token: 0x17000AF4 RID: 2804
	// (get) Token: 0x06004CF5 RID: 19701 RVA: 0x001934FA File Offset: 0x001918FA
	public ComputeBuffer surfaceNormalsBuffer
	{
		get
		{
			return this._surfaceNormalsBuffer;
		}
	}

	// Token: 0x17000AF5 RID: 2805
	// (get) Token: 0x06004CF6 RID: 19702 RVA: 0x00193502 File Offset: 0x00191902
	public ComputeBuffer surfaceNormalsRawBuffer
	{
		get
		{
			return this._surfaceNormalsRawBuffer;
		}
	}

	// Token: 0x06004CF7 RID: 19703 RVA: 0x0019350C File Offset: 0x0019190C
	protected void SkinMeshGPUMaterialInit()
	{
		if (this.GPUAutoSwapShader)
		{
			for (int i = 0; i < this.GPUmaterials.Length; i++)
			{
				if (this.GPUmaterials[i] != null)
				{
					Shader shader = this.GPUmaterials[i].shader;
					Shader shader2;
					if (this.GPUAutoSwapCopyNum > 0)
					{
						shader2 = Shader.Find(shader.name + "ComputeBuffCopy" + this.GPUAutoSwapCopyNum);
						if (shader2 == null)
						{
							shader2 = Shader.Find(shader.name + "ComputeBuff");
						}
					}
					else
					{
						shader2 = Shader.Find(shader.name + "ComputeBuff");
					}
					Material material = new Material(this.GPUmaterials[i]);
					this.RegisterAllocatedObject(material);
					if (shader2 != null)
					{
						material.shader = shader2;
					}
					this.GPUmaterials[i] = material;
				}
			}
			if (this.GPUsimpleMaterial != null)
			{
				Shader shader3 = this.GPUsimpleMaterial.shader;
				Shader shader4 = Shader.Find(shader3.name + "ComputeBuff");
				Material material2 = new Material(this.GPUsimpleMaterial);
				this.RegisterAllocatedObject(material2);
				if (shader4 != null)
				{
					material2.shader = shader4;
				}
				this.GPUsimpleMaterial = material2;
			}
		}
	}

	// Token: 0x06004CF8 RID: 19704 RVA: 0x00193664 File Offset: 0x00191A64
	protected void SkinMeshGPUInit()
	{
		if (this.startVertsBuffer == null)
		{
			this._zeroKernel = this.GPUSkinner.FindKernel("ZeroVerts");
			this._skinGeneralKernel = this.GPUSkinner.FindKernel("GeneralSkin");
			this._initKernel = this.GPUSkinner.FindKernel("InitVerts");
			this._copyKernel = this.GPUSkinner.FindKernel("CopyVerts");
			this._skinXYZKernel = this.GPUSkinner.FindKernel("SkinXYZ");
			this._skinXZYKernel = this.GPUSkinner.FindKernel("SkinXZY");
			this._skinYXZKernel = this.GPUSkinner.FindKernel("SkinYXZ");
			this._skinYZXKernel = this.GPUSkinner.FindKernel("SkinYZX");
			this._skinZXYKernel = this.GPUSkinner.FindKernel("SkinZXY");
			this._skinZYXKernel = this.GPUSkinner.FindKernel("SkinZYX");
			this._skinFinishKernel = this.GPUSkinner.FindKernel("SkinFinish");
			this._postSkinMorphKernel = this.GPUSkinner.FindKernel("PostSkinMorph");
			this._copyTangentsKernel = this.GPUMeshCompute.FindKernel("CopyTangents");
			this._calcChangeMatricesKernel = this.GPUSkinner.FindKernel("SkinWrapCalcChangeMatrices");
			this.boneBuffer = new ComputeBuffer[this.numBones];
			this.numVertThreadGroups = this.numUVVerts / 256;
			int num = this.numUVVerts % 256;
			if (num != 0)
			{
				this.numVertThreadGroups++;
			}
			int num2 = this.numVertThreadGroups * 256;
			this._nullVertexIndex = num2;
			this.startVertsBuffer = new ComputeBuffer(num2 + 1, 12);
			this.rawVertsBuffer = new ComputeBuffer(num2 + 1, 12);
			this.postSkinMorphsBuffer = new ComputeBuffer(num2 + 1, 12);
			this._verticesBuffer1 = new ComputeBuffer(num2 + 1, 12);
			this._verticesBuffer2 = new ComputeBuffer(num2 + 1, 12);
			this.preCalcVertsBuffer = new ComputeBuffer(num2 + 1, 12);
			this.delayedVertsBuffer = new ComputeBuffer(num2 + 1, 12);
			this.delayedNormalsBuffer = new ComputeBuffer(num2 + 1, 12);
			this.delayedTangentsBuffer = new ComputeBuffer(num2 + 1, 16);
			this._originalVerticesBuffer = new ComputeBuffer(num2 + 1, 12);
			this._originalVerticesBuffer.SetData(this.dazMesh.morphedUVVertices);
			this.weightsBuffer = new ComputeBuffer[this.numBones];
			this.fullWeightsBuffer = new ComputeBuffer[this.numBones];
			this.generalWeightsBuffer = new ComputeBuffer[this.numBones];
			this.numSkinThreadGroups = new int[this.numBones];
			this.numSkinFinishThreadGroups = new int[this.numBones];
			this.numGeneralSkinThreadGroups = new int[this.numBones];
			for (int i = 0; i < this.numBones; i++)
			{
				this.boneBuffer[i] = new ComputeBuffer(1, 252);
				int num3 = this.nodes[i].weights.Length;
				this.numSkinThreadGroups[i] = num3 / 256;
				num = num3 % 256;
				if (num != 0)
				{
					this.numSkinThreadGroups[i]++;
				}
				int num4 = this.numSkinThreadGroups[i] * 256;
				if (num4 > 0)
				{
					this.weightsBuffer[i] = new ComputeBuffer(num4, 40);
					DAZSkinV2.BoneWeights[] array = new DAZSkinV2.BoneWeights[num4];
					for (int j = 0; j < num4; j++)
					{
						if (j < num3)
						{
							array[j].vertex = this.nodes[i].weights[j].vertex;
							array[j].xweight = this.nodes[i].weights[j].xweight;
							array[j].yweight = this.nodes[i].weights[j].yweight;
							array[j].zweight = this.nodes[i].weights[j].zweight;
							array[j].xleftbulge = this.nodes[i].weights[j].xleftbulge;
							array[j].xrightbulge = this.nodes[i].weights[j].xrightbulge;
							array[j].yleftbulge = this.nodes[i].weights[j].yleftbulge;
							array[j].yrightbulge = this.nodes[i].weights[j].yrightbulge;
							array[j].zleftbulge = this.nodes[i].weights[j].zleftbulge;
							array[j].zrightbulge = this.nodes[i].weights[j].zrightbulge;
						}
						else
						{
							array[j].vertex = this._nullVertexIndex;
							array[j].xweight = 0f;
							array[j].yweight = 0f;
							array[j].zweight = 0f;
							array[j].xleftbulge = 0f;
							array[j].xrightbulge = 0f;
							array[j].yleftbulge = 0f;
							array[j].yrightbulge = 0f;
							array[j].zleftbulge = 0f;
							array[j].zrightbulge = 0f;
						}
					}
					this.weightsBuffer[i].SetData(array);
				}
				int num5 = this.nodes[i].fullyWeightedVertices.Length;
				this.numSkinFinishThreadGroups[i] = num5 / 256;
				num = num5 % 256;
				if (num != 0)
				{
					this.numSkinFinishThreadGroups[i]++;
				}
				int num6 = this.numSkinFinishThreadGroups[i] * 256;
				if (num6 > 0)
				{
					this.fullWeightsBuffer[i] = new ComputeBuffer(num6, 4);
					DAZSkinV2.BoneFullWeights[] array2 = new DAZSkinV2.BoneFullWeights[num6];
					for (int k = 0; k < num6; k++)
					{
						if (k < num5)
						{
							array2[k].vertex = this.nodes[i].fullyWeightedVertices[k];
						}
						else
						{
							array2[k].vertex = this._nullVertexIndex;
						}
					}
					this.fullWeightsBuffer[i].SetData(array2);
				}
				int num7 = this.nodes[i].generalWeights.Length;
				this.numGeneralSkinThreadGroups[i] = num7 / 256;
				num = num7 % 256;
				if (num != 0)
				{
					this.numGeneralSkinThreadGroups[i]++;
				}
				int num8 = this.numGeneralSkinThreadGroups[i] * 256;
				if (num8 > 0)
				{
					this.generalWeightsBuffer[i] = new ComputeBuffer(num8, 8);
					DAZSkinV2.BoneGeneralWeights[] array3 = new DAZSkinV2.BoneGeneralWeights[num8];
					for (int l = 0; l < num8; l++)
					{
						if (l < num7)
						{
							array3[l].vertex = this.nodes[i].generalWeights[l].vertex;
							array3[l].weight = this.nodes[i].generalWeights[l].weight;
						}
						else
						{
							array3[l].vertex = this._nullVertexIndex;
							array3[l].weight = 0f;
						}
					}
					this.generalWeightsBuffer[i].SetData(array3);
				}
			}
			this.mapVerticesGPU = new MapVerticesGPU(this.GPUMeshCompute, this.dazMesh.baseVerticesToUVVertices);
			this.InitRecalcNormalsTangents();
			this.PreCalculatedVerticesBuffer = new GpuBuffer<Vector3>(this.preCalcVertsBuffer);
			this._matricesBuffer = new ComputeBuffer(num2, 64);
			this._ToWorldMatricesBuffer = new GpuBuffer<Matrix4x4>(this._matricesBuffer);
			this.NormalsBuffer = new GpuBuffer<Vector3>(this._normalsBuffer);
		}
	}

	// Token: 0x06004CF9 RID: 19705 RVA: 0x00193E5C File Offset: 0x0019225C
	protected void SkinMeshGPUCleanup()
	{
		if (this.mapVerticesGPU != null)
		{
			this.mapVerticesGPU.Release();
			this.mapVerticesGPU = null;
		}
		if (this.meshSmoothGPU != null)
		{
			this.meshSmoothGPU.Release();
			this.meshSmoothGPU = null;
		}
		if (this.originalRecalcNormalsGPU != null)
		{
			this.originalRecalcNormalsGPU.Release();
			this.originalRecalcNormalsGPU = null;
		}
		if (this.recalcNormalsGPU != null)
		{
			this.recalcNormalsGPU.Release();
			this.recalcNormalsGPU = null;
		}
		if (this.recalcNormalsGPURaw != null)
		{
			this.recalcNormalsGPURaw.Release();
			this.recalcNormalsGPURaw = null;
		}
		if (this.originalRecalcTangentsGPU != null)
		{
			this.originalRecalcTangentsGPU.Release();
			this.originalRecalcTangentsGPU = null;
		}
		if (this.recalcTangentsGPU != null)
		{
			this.recalcTangentsGPU.Release();
			this.recalcTangentsGPU = null;
		}
		if (this.boneBuffer != null)
		{
			for (int i = 0; i < this.numBones; i++)
			{
				this.boneBuffer[i].Release();
				this.boneBuffer[i] = null;
			}
			this.boneBuffer = null;
		}
		if (this.startVertsBuffer != null)
		{
			this.startVertsBuffer.Release();
			this.startVertsBuffer = null;
		}
		if (this.rawVertsBuffer != null)
		{
			this.rawVertsBuffer.Release();
			this.rawVertsBuffer = null;
		}
		if (this.postSkinMorphsBuffer != null)
		{
			this.postSkinMorphsBuffer.Release();
			this.postSkinMorphsBuffer = null;
		}
		if (this._verticesBuffer1 != null)
		{
			this._verticesBuffer1.Release();
			this._verticesBuffer1 = null;
		}
		if (this._verticesBuffer2 != null)
		{
			this._verticesBuffer2.Release();
			this._verticesBuffer2 = null;
		}
		if (this.preCalcVertsBuffer != null)
		{
			this.preCalcVertsBuffer.Release();
			this.preCalcVertsBuffer = null;
		}
		if (this.delayedVertsBuffer != null)
		{
			this.delayedVertsBuffer.Release();
			this.delayedVertsBuffer = null;
		}
		if (this.delayedNormalsBuffer != null)
		{
			this.delayedNormalsBuffer.Release();
			this.delayedNormalsBuffer = null;
		}
		if (this.delayedTangentsBuffer != null)
		{
			this.delayedTangentsBuffer.Release();
			this.delayedTangentsBuffer = null;
		}
		if (this._originalVerticesBuffer != null)
		{
			this._originalVerticesBuffer.Release();
			this._originalVerticesBuffer = null;
		}
		if (this._matricesBuffer != null)
		{
			this._matricesBuffer.Release();
			this._matricesBuffer = null;
		}
		if (this.weightsBuffer != null)
		{
			for (int j = 0; j < this.numBones; j++)
			{
				if (this.weightsBuffer[j] != null)
				{
					this.weightsBuffer[j].Release();
					this.weightsBuffer[j] = null;
				}
			}
			this.weightsBuffer = null;
		}
		if (this.fullWeightsBuffer != null)
		{
			for (int k = 0; k < this.numBones; k++)
			{
				if (this.fullWeightsBuffer[k] != null)
				{
					this.fullWeightsBuffer[k].Release();
					this.fullWeightsBuffer[k] = null;
				}
			}
			this.fullWeightsBuffer = null;
		}
		if (this.generalWeightsBuffer != null)
		{
			for (int l = 0; l < this.numBones; l++)
			{
				if (this.generalWeightsBuffer[l] != null)
				{
					this.generalWeightsBuffer[l].Release();
					this.generalWeightsBuffer[l] = null;
				}
			}
			this.generalWeightsBuffer = null;
		}
	}

	// Token: 0x17000AF6 RID: 2806
	// (get) Token: 0x06004CFA RID: 19706 RVA: 0x00194196 File Offset: 0x00192596
	public Vector3[] postSkinNormalsThreaded
	{
		get
		{
			return this.postSkinWorkingNormals;
		}
	}

	// Token: 0x06004CFB RID: 19707 RVA: 0x001941A0 File Offset: 0x001925A0
	protected void RecalculatePostSkinNeededVertsAndTriangles()
	{
		for (int i = 0; i < this.numUVVerts; i++)
		{
			this.postSkinNeededVerts[i] = this.postSkinVerts[i];
		}
		this.needsPostSkinNormals = false;
		List<int> list = new List<int>();
		List<int> list2 = new List<int>();
		int[] baseTriangles = this.dazMesh.baseTriangles;
		for (int j = 0; j < baseTriangles.Length; j += 3)
		{
			int num = baseTriangles[j];
			int num2 = baseTriangles[j + 1];
			int num3 = baseTriangles[j + 2];
			if (this.postSkinNormalVerts[num] || this.postSkinNormalVerts[num2] || this.postSkinNormalVerts[num3])
			{
				this.needsPostSkinNormals = true;
				list.Add(j);
				list2.Add(j / 3);
				this.postSkinNeededVerts[num] = true;
				this.postSkinNeededVerts[num2] = true;
				this.postSkinNeededVerts[num3] = true;
			}
		}
		List<int> list3 = new List<int>();
		for (int k = 0; k < this.numUVVerts; k++)
		{
			if (this.postSkinNeededVerts[k])
			{
				list3.Add(k);
			}
		}
		this.postSkinNeededTriangles = list.ToArray();
		this.postSkinNeededTriangleIndices = list2.ToArray();
		this.postSkinNeededVertsList = list3.ToArray();
	}

	// Token: 0x06004CFC RID: 19708 RVA: 0x001942E0 File Offset: 0x001926E0
	protected bool[] DetermineUsedBonesForVerts(bool[] usedVerts)
	{
		bool[] array = new bool[this._numBones];
		for (int i = 0; i < this._numBones; i++)
		{
			if (this._useGeneralWeights)
			{
				DAZSkinV2GeneralVertexWeights[] generalWeights = this.nodes[i].generalWeights;
				for (int j = 0; j < generalWeights.Length; j++)
				{
					if (usedVerts[generalWeights[j].vertex])
					{
						array[i] = true;
						break;
					}
				}
			}
			else
			{
				DAZSkinV2VertexWeights[] weights = this.nodes[i].weights;
				bool flag = false;
				for (int k = 0; k < weights.Length; k++)
				{
					if (usedVerts[weights[k].vertex])
					{
						array[i] = true;
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					int[] fullyWeightedVertices = this.nodes[i].fullyWeightedVertices;
					for (int l = 0; l < fullyWeightedVertices.Length; l++)
					{
						if (usedVerts[fullyWeightedVertices[l]])
						{
							array[i] = true;
							break;
						}
					}
				}
			}
		}
		return array;
	}

	// Token: 0x06004CFD RID: 19709 RVA: 0x001943E8 File Offset: 0x001927E8
	public void SkinMeshGPUPostSkinVertsStart()
	{
		if (this._useGeneralWeights)
		{
			for (int i = 0; i < this.postSkinNeededVertsList.Length; i++)
			{
				int num = this.postSkinNeededVertsList[i];
				this.startVertsCopy[num] = this.startVerts[num];
				this.workingVerts[num].x = 0f;
				this.workingVerts[num].y = 0f;
				this.workingVerts[num].z = 0f;
			}
		}
		else
		{
			for (int j = 0; j < this.postSkinNeededVertsList.Length; j++)
			{
				int num2 = this.postSkinNeededVertsList[j];
				this.workingVerts[num2] = this.startVerts[num2];
			}
		}
		this.postSkinVertsChangedThreaded = this.postSkinVertsChanged;
		this.postSkinVertsChanged = false;
	}

	// Token: 0x06004CFE RID: 19710 RVA: 0x001944E4 File Offset: 0x001928E4
	public void SkinMeshGPUPostSkinVertsThreaded()
	{
		if (this.postSkinVertsChangedThreaded)
		{
			this.RecalculatePostSkinNeededVertsAndTriangles();
			this.postSkinBones = this.DetermineUsedBonesForVerts(this.postSkinNeededVerts);
			this.postSkinVertsChangedThreaded = false;
		}
		this.SkinMeshPart(0, this.numBaseVerts - 1, this.postSkinNeededVerts, this.postSkinBones);
		for (int i = 0; i < this.postSkinNeededVertsList.Length; i++)
		{
			int num = this.postSkinNeededVertsList[i];
			this.rawSkinnedWorkingVerts[num] = this.workingVerts[num];
		}
		if (this.needsPostSkinNormals)
		{
			this.postSkinRecalcNormals.recalculateNormals(this.postSkinNeededTriangles, this.postSkinNeededVertsList);
		}
	}

	// Token: 0x06004CFF RID: 19711 RVA: 0x0019459C File Offset: 0x0019299C
	public Vector3[] RecalcAndGetAllNormals(Vector3[] vertsToUse)
	{
		this.postSkinRecalcNormals.recalculateNormals(vertsToUse);
		foreach (DAZVertexMap dazvertexMap in this.baseVertsToUVVertsCopy)
		{
			this.postSkinWorkingNormals[dazvertexMap.tovert] = this.postSkinWorkingNormals[dazvertexMap.fromvert];
		}
		return (Vector3[])this.postSkinWorkingNormals.Clone();
	}

	// Token: 0x06004D00 RID: 19712 RVA: 0x00194614 File Offset: 0x00192A14
	public void SmoothAllVertices(Vector3[] inVerts, Vector3[] outVerts)
	{
		this.meshSmooth.LaplacianSmooth(inVerts, outVerts, 0, 100000000);
		this.meshSmooth.HCCorrection(inVerts, outVerts, this.laplacianSmoothBeta, 0, 1000000000);
		foreach (DAZVertexMap dazvertexMap in this.baseVertsToUVVertsCopy)
		{
			outVerts[dazvertexMap.tovert] = outVerts[dazvertexMap.fromvert];
		}
	}

	// Token: 0x06004D01 RID: 19713 RVA: 0x00194690 File Offset: 0x00192A90
	public void SkinMeshGPUPostSkinVertsFinish()
	{
		for (int i = 0; i < this.postSkinNeededVertsList.Length; i++)
		{
			int num = this.postSkinNeededVertsList[i];
			this.postSkinVertsReady[num] = true;
			this.rawSkinnedVerts[num] = this.rawSkinnedWorkingVerts[num];
			this.postSkinNormals[num] = this.postSkinWorkingNormals[num];
		}
	}

	// Token: 0x06004D02 RID: 19714 RVA: 0x0019470C File Offset: 0x00192B0C
	public void SkinMeshGPUPostSkinVerts()
	{
		if (this.useAsynchronousThreadedSkinning)
		{
			if (!this.postSkinMorphTask.working)
			{
				this.SkinMeshGPUPostSkinVertsFinish();
				this.SkinMeshGPUPostSkinVertsStart();
				this.postSkinMorphTask.working = true;
				this.postSkinMorphTask.resetEvent.Set();
			}
		}
		else
		{
			this.SkinMeshGPUPostSkinVertsStart();
			this.SkinMeshGPUPostSkinVertsThreaded();
			this.SkinMeshGPUPostSkinVertsFinish();
		}
	}

	// Token: 0x06004D03 RID: 19715 RVA: 0x00194778 File Offset: 0x00192B78
	protected void SkinMeshGPUFrame()
	{
		this.startVertsBuffer.SetData(this.startVerts);
		if (this._useGeneralWeights)
		{
			this.GPUSkinner.SetBuffer(this._zeroKernel, "outVerts", this.rawVertsBuffer);
			this.GPUSkinner.Dispatch(this._zeroKernel, this.numVertThreadGroups, 1, 1);
		}
		else
		{
			this.GPUSkinner.SetBuffer(this._initKernel, "inVerts", this.startVertsBuffer);
			this.GPUSkinner.SetBuffer(this._initKernel, "outVerts", this.rawVertsBuffer);
			this.GPUSkinner.Dispatch(this._initKernel, this.numVertThreadGroups, 1, 1);
		}
		DAZSkinV2.Bone[] array = new DAZSkinV2.Bone[]
		{
			default(DAZSkinV2.Bone)
		};
		for (int i = 0; i < this.numBones; i++)
		{
			DAZBone dazbone = this.dazBones[i];
			array[0].changeFromOriginal = dazbone.changeFromOriginalMatrix;
			this.boneChangeFromOriginalMatrices[i] = dazbone.changeFromOriginalMatrix;
			if (!this._useGeneralWeights)
			{
				DAZSkinV2BulgeFactors bulgeFactors = this.nodes[i].bulgeFactors;
				Vector3 angles = dazbone.GetAngles();
				array[0].rotationAngles = angles;
				array[0].worldToLocal = dazbone.morphedWorldToLocalMatrix;
				array[0].localToWorld = dazbone.morphedLocalToWorldMatrix;
				this.boneRotationAngles[i] = angles;
				this.boneWorldToLocalMatrices[i] = dazbone.morphedWorldToLocalMatrix;
				this.boneLocalToWorldMatrices[i] = dazbone.morphedLocalToWorldMatrix;
				if (angles.x >= 0f)
				{
					array[0].xposleftbulge = bulgeFactors.xposleft * angles.x * this.bulgeScale;
					array[0].xposrightbulge = bulgeFactors.xposright * angles.x * this.bulgeScale;
					array[0].xnegleftbulge = 0f;
					array[0].xnegrightbulge = 0f;
				}
				else
				{
					array[0].xposleftbulge = 0f;
					array[0].xposrightbulge = 0f;
					array[0].xnegleftbulge = bulgeFactors.xnegleft * angles.x * this.bulgeScale;
					array[0].xnegrightbulge = bulgeFactors.xnegright * angles.x * this.bulgeScale;
				}
				if (angles.y >= 0f)
				{
					array[0].yposleftbulge = bulgeFactors.yposleft * angles.y * this.bulgeScale;
					array[0].yposrightbulge = bulgeFactors.yposright * angles.y * this.bulgeScale;
					array[0].ynegleftbulge = 0f;
					array[0].ynegrightbulge = 0f;
				}
				else
				{
					array[0].yposleftbulge = 0f;
					array[0].yposrightbulge = 0f;
					array[0].ynegleftbulge = bulgeFactors.ynegleft * angles.y * this.bulgeScale;
					array[0].ynegrightbulge = bulgeFactors.ynegright * angles.y * this.bulgeScale;
				}
				if (angles.z >= 0f)
				{
					array[0].zposleftbulge = bulgeFactors.zposleft * angles.z * this.bulgeScale;
					array[0].zposrightbulge = bulgeFactors.zposright * angles.z * this.bulgeScale;
					array[0].znegleftbulge = 0f;
					array[0].znegrightbulge = 0f;
				}
				else
				{
					array[0].zposleftbulge = 0f;
					array[0].zposrightbulge = 0f;
					array[0].znegleftbulge = bulgeFactors.znegleft * angles.z * this.bulgeScale;
					array[0].znegrightbulge = bulgeFactors.znegright * angles.z * this.bulgeScale;
				}
			}
			this.boneBuffer[i].SetData(array);
			if (this._useGeneralWeights)
			{
				if (this.numGeneralSkinThreadGroups[i] > 0)
				{
					this.GPUSkinner.SetBuffer(this._skinGeneralKernel, "bone", this.boneBuffer[i]);
					this.GPUSkinner.SetBuffer(this._skinGeneralKernel, "generalWeights", this.generalWeightsBuffer[i]);
					this.GPUSkinner.SetBuffer(this._skinGeneralKernel, "inVerts", this.startVertsBuffer);
					this.GPUSkinner.SetBuffer(this._skinGeneralKernel, "outVerts", this.rawVertsBuffer);
					this.GPUSkinner.Dispatch(this._skinGeneralKernel, this.numGeneralSkinThreadGroups[i], 1, 1);
				}
			}
			else
			{
				if (this.numSkinThreadGroups[i] > 0)
				{
					switch (this.nodes[i].rotationOrder)
					{
					case Quaternion2Angles.RotationOrder.XYZ:
						this.GPUSkinner.SetBuffer(this._skinXYZKernel, "bone", this.boneBuffer[i]);
						this.GPUSkinner.SetBuffer(this._skinXYZKernel, "weights", this.weightsBuffer[i]);
						this.GPUSkinner.SetBuffer(this._skinXYZKernel, "outVerts", this.rawVertsBuffer);
						this.GPUSkinner.Dispatch(this._skinXYZKernel, this.numSkinThreadGroups[i], 1, 1);
						break;
					case Quaternion2Angles.RotationOrder.XZY:
						this.GPUSkinner.SetBuffer(this._skinXZYKernel, "bone", this.boneBuffer[i]);
						this.GPUSkinner.SetBuffer(this._skinXZYKernel, "weights", this.weightsBuffer[i]);
						this.GPUSkinner.SetBuffer(this._skinXZYKernel, "outVerts", this.rawVertsBuffer);
						this.GPUSkinner.Dispatch(this._skinXZYKernel, this.numSkinThreadGroups[i], 1, 1);
						break;
					case Quaternion2Angles.RotationOrder.YXZ:
						this.GPUSkinner.SetBuffer(this._skinYXZKernel, "bone", this.boneBuffer[i]);
						this.GPUSkinner.SetBuffer(this._skinYXZKernel, "weights", this.weightsBuffer[i]);
						this.GPUSkinner.SetBuffer(this._skinYXZKernel, "outVerts", this.rawVertsBuffer);
						this.GPUSkinner.Dispatch(this._skinYXZKernel, this.numSkinThreadGroups[i], 1, 1);
						break;
					case Quaternion2Angles.RotationOrder.YZX:
						this.GPUSkinner.SetBuffer(this._skinYZXKernel, "bone", this.boneBuffer[i]);
						this.GPUSkinner.SetBuffer(this._skinYZXKernel, "weights", this.weightsBuffer[i]);
						this.GPUSkinner.SetBuffer(this._skinYZXKernel, "outVerts", this.rawVertsBuffer);
						this.GPUSkinner.Dispatch(this._skinYZXKernel, this.numSkinThreadGroups[i], 1, 1);
						break;
					case Quaternion2Angles.RotationOrder.ZXY:
						this.GPUSkinner.SetBuffer(this._skinZXYKernel, "bone", this.boneBuffer[i]);
						this.GPUSkinner.SetBuffer(this._skinZXYKernel, "weights", this.weightsBuffer[i]);
						this.GPUSkinner.SetBuffer(this._skinZXYKernel, "outVerts", this.rawVertsBuffer);
						this.GPUSkinner.Dispatch(this._skinZXYKernel, this.numSkinThreadGroups[i], 1, 1);
						break;
					case Quaternion2Angles.RotationOrder.ZYX:
						this.GPUSkinner.SetBuffer(this._skinZYXKernel, "bone", this.boneBuffer[i]);
						this.GPUSkinner.SetBuffer(this._skinZYXKernel, "weights", this.weightsBuffer[i]);
						this.GPUSkinner.SetBuffer(this._skinZYXKernel, "outVerts", this.rawVertsBuffer);
						this.GPUSkinner.Dispatch(this._skinZYXKernel, this.numSkinThreadGroups[i], 1, 1);
						break;
					}
				}
				if (this.numSkinFinishThreadGroups[i] > 0)
				{
					this.GPUSkinner.SetBuffer(this._skinFinishKernel, "bone", this.boneBuffer[i]);
					this.GPUSkinner.SetBuffer(this._skinFinishKernel, "fullWeights", this.fullWeightsBuffer[i]);
					this.GPUSkinner.SetBuffer(this._skinFinishKernel, "outVerts", this.rawVertsBuffer);
					this.GPUSkinner.Dispatch(this._skinFinishKernel, this.numSkinFinishThreadGroups[i], 1, 1);
				}
			}
		}
	}

	// Token: 0x06004D04 RID: 19716 RVA: 0x0019501C File Offset: 0x0019341C
	protected void SkinMeshGPU()
	{
		this.lastFrameSkinStartTime = this.skinStartTime;
		this.skinStartTime = (float)this.stopwatch.ElapsedTicks * this.f;
		if (this.mesh != null && this.root != null && this.GPUSkinner != null)
		{
			this.StartThreads();
			this.SkinMeshGPUInit();
			this.SkinMeshGPUFrame();
			this.InitRecalcNormalsTangents();
			if (this.allowPostSkinMorph)
			{
				this.SkinMeshGPUPostSkinVerts();
				this.postSkinMorphsBuffer.SetData(this.postSkinMorphs);
				this.GPUSkinner.SetBuffer(this._postSkinMorphKernel, "postSkinMorphs", this.postSkinMorphsBuffer);
				this.GPUSkinner.SetBuffer(this._postSkinMorphKernel, "outVerts", this.rawVertsBuffer);
				this.GPUSkinner.Dispatch(this._postSkinMorphKernel, this.numVertThreadGroups, 1, 1);
			}
			if (this._useSmoothing)
			{
				this.InitSmoothing();
				int num = 0;
				this.smoothedVertsBuffer = this.rawVertsBuffer;
				if (UserPreferences.singleton != null)
				{
					this.smoothOuterLoops = UserPreferences.singleton.smoothPasses;
				}
				for (int i = 0; i < this.smoothOuterLoops; i++)
				{
					for (int j = 0; j < this.laplacianSmoothPasses; j++)
					{
						if (num == 0)
						{
							this.meshSmoothGPU.LaplacianSmoothGPU(this.rawVertsBuffer, this._verticesBuffer2);
							this.meshSmoothGPU.HCCorrectionGPU(this.rawVertsBuffer, this._verticesBuffer2, this.laplacianSmoothBeta);
							this.smoothedVertsBuffer = this._verticesBuffer2;
						}
						else if (num % 2 == 0)
						{
							this.meshSmoothGPU.LaplacianSmoothGPU(this._verticesBuffer1, this._verticesBuffer2);
							this.meshSmoothGPU.HCCorrectionGPU(this._verticesBuffer1, this._verticesBuffer2, this.laplacianSmoothBeta);
							this.smoothedVertsBuffer = this._verticesBuffer2;
						}
						else
						{
							this.meshSmoothGPU.LaplacianSmoothGPU(this._verticesBuffer2, this._verticesBuffer1);
							this.meshSmoothGPU.HCCorrectionGPU(this._verticesBuffer2, this._verticesBuffer1, this.laplacianSmoothBeta);
							this.smoothedVertsBuffer = this._verticesBuffer1;
						}
						num++;
					}
					for (int k = 0; k < this.springSmoothPasses; k++)
					{
						if (num == 0)
						{
							this.meshSmoothGPU.SpringSmoothGPU(this.rawVertsBuffer, this._verticesBuffer2, this.springSmoothFactor, 1f);
							this.smoothedVertsBuffer = this._verticesBuffer2;
						}
						else if (num % 2 == 0)
						{
							this.meshSmoothGPU.SpringSmoothGPU(this._verticesBuffer1, this._verticesBuffer2, this.springSmoothFactor, 1f);
							this.smoothedVertsBuffer = this._verticesBuffer2;
						}
						else
						{
							this.meshSmoothGPU.SpringSmoothGPU(this._verticesBuffer2, this._verticesBuffer1, this.springSmoothFactor, 1f);
							this.smoothedVertsBuffer = this._verticesBuffer1;
						}
						num++;
					}
				}
				this.mapVerticesGPU.Map(this.smoothedVertsBuffer);
			}
			else
			{
				this.mapVerticesGPU.Map(this.rawVertsBuffer);
			}
			if (this._recalculateNormals)
			{
				if (this._useSmoothing && this.useSmoothVertsForNormalTangentRecalc)
				{
					this.recalcNormalsGPU.RecalculateNormals(this.smoothedVertsBuffer);
					this.recalcNormalsGPURaw.RecalculateSurfaceNormals(this.rawVertsBuffer);
				}
				else
				{
					this.recalcNormalsGPU.RecalculateNormals(this.rawVertsBuffer);
				}
			}
			if (this._recalculateTangents)
			{
				if (this._useSmoothing && this.useSmoothVertsForNormalTangentRecalc)
				{
					this.recalcTangentsGPU.RecalculateTangents(this.smoothedVertsBuffer, this._normalsBuffer);
				}
				else
				{
					this.recalcTangentsGPU.RecalculateTangents(this.rawVertsBuffer, this._normalsBuffer);
				}
			}
		}
		this.skinStopTime = (float)this.stopwatch.ElapsedTicks * this.f;
		this.skinTime = this.skinStopTime - this.skinStartTime;
	}

	// Token: 0x06004D05 RID: 19717 RVA: 0x00195412 File Offset: 0x00193812
	public void SkinMeshCPUandGPUStartFrameFast()
	{
		this.StartSubThreads();
		this.postSkinVertsChangedThreaded = this.postSkinVertsChanged;
		this.postSkinVertsChanged = false;
	}

	// Token: 0x06004D06 RID: 19718 RVA: 0x00195430 File Offset: 0x00193830
	public void SkinMeshThreadedFast(Vector3[] verts, bool forceSynchronous = false)
	{
		this.threadMainSkinStartTime = (float)this.stopwatch.ElapsedTicks * this.f;
		int num = this.numBaseVerts;
		if (this._useGeneralWeights)
		{
			for (int i = 0; i < num; i++)
			{
				this.workingVerts[i].x = 0f;
				this.workingVerts[i].y = 0f;
				this.workingVerts[i].z = 0f;
			}
			this.startVertsCopy = verts;
		}
		else
		{
			this.workingVerts = verts;
		}
		if (this.useMultithreadedSkinning && !forceSynchronous && this._numSubThreads > 0)
		{
			int num2 = (this.numUVVerts - this.numUVOnlyVerts) / this._numSubThreads;
			for (int j = 0; j < this._numSubThreads; j++)
			{
				this.tasks[j].taskType = DAZSkinTaskType.Skin;
				this.tasks[j].index1 = j * num2;
				if (j == this._numSubThreads - 1)
				{
					this.tasks[j].index2 = num - 1;
				}
				else
				{
					this.tasks[j].index2 = (j + 1) * num2 - 1;
				}
				this.threadSkinVertsCount[j] = this.tasks[j].index2 - this.tasks[j].index1 + 1;
				this.tasks[j].working = true;
				this.tasks[j].resetEvent.Set();
			}
			bool flag;
			do
			{
				flag = false;
				for (int k = 0; k < this._numSubThreads; k++)
				{
					if (this.tasks[k].working)
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
			this.mainThreadSkinStartTime = (float)this.stopwatch.ElapsedTicks * this.f;
			this.SkinMeshPart(0, num - 1, this.isBaseVert, null);
			this.mainThreadSkinStopTime = (float)this.stopwatch.ElapsedTicks * this.f;
			this.mainThreadSkinTime = this.mainThreadSkinStopTime - this.mainThreadSkinStartTime;
		}
		foreach (DAZVertexMap dazvertexMap in this.dazMesh.baseVerticesToUVVertices)
		{
			this.workingVerts[dazvertexMap.tovert] = this.workingVerts[dazvertexMap.fromvert];
		}
		this.threadMainSkinStopTime = (float)this.stopwatch.ElapsedTicks * this.f;
		this.threadMainSkinTime = this.threadMainSkinStopTime - this.threadMainSkinStartTime;
	}

	// Token: 0x06004D07 RID: 19719 RVA: 0x001956E4 File Offset: 0x00193AE4
	public void SkinMeshPostVertsThreadedFast(Vector3[] verts)
	{
		this.threadMainSkinStartTime = (float)this.stopwatch.ElapsedTicks * this.f;
		int num = this.numBaseVerts;
		if (this._useGeneralWeights)
		{
			for (int i = 0; i < num; i++)
			{
				this.workingVerts[i].x = 0f;
				this.workingVerts[i].y = 0f;
				this.workingVerts[i].z = 0f;
			}
			this.startVertsCopy = verts;
		}
		else
		{
			this.workingVerts = verts;
		}
		if (this.postSkinVertsChangedThreaded)
		{
			this.RecalculatePostSkinNeededVertsAndTriangles();
			this.postSkinBones = this.DetermineUsedBonesForVerts(this.postSkinNeededVerts);
			this.postSkinVertsChangedThreaded = false;
		}
		if (this.useMultithreadedSkinning && this._numSubThreads > 0)
		{
			int num2 = (this.numUVVerts - this.numUVOnlyVerts) / this._numSubThreads;
			for (int j = 0; j < this._numSubThreads; j++)
			{
				this.tasks[j].taskType = DAZSkinTaskType.PostSkin;
				this.tasks[j].index1 = j * num2;
				if (j == this._numSubThreads - 1)
				{
					this.tasks[j].index2 = num - 1;
				}
				else
				{
					this.tasks[j].index2 = (j + 1) * num2 - 1;
				}
				this.threadSkinVertsCount[j] = this.tasks[j].index2 - this.tasks[j].index1 + 1;
				this.tasks[j].working = true;
				this.tasks[j].resetEvent.Set();
			}
			bool flag;
			do
			{
				flag = false;
				for (int k = 0; k < this._numSubThreads; k++)
				{
					if (this.tasks[k].working)
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
			this.SkinMeshPart(0, this.numBaseVerts - 1, this.postSkinNeededVerts, this.postSkinBones);
		}
		for (int l = 0; l < this.postSkinNeededVertsList.Length; l++)
		{
			int num3 = this.postSkinNeededVertsList[l];
			this.rawSkinnedWorkingVerts[num3] = this.workingVerts[num3];
		}
		if (this.needsPostSkinNormals)
		{
			this.threadRecalcNormalTangentStartTime = (float)this.stopwatch.ElapsedTicks * this.f;
			this.postSkinRecalcNormals.recalculateNormals(this.postSkinNeededTriangles, this.postSkinNeededVertsList);
			this.threadRecalcNormalTangentStopTime = (float)this.stopwatch.ElapsedTicks * this.f;
			this.threadRecalcNormalTangentTime = this.threadRecalcNormalTangentStopTime - this.threadRecalcNormalTangentStartTime;
		}
	}

	// Token: 0x06004D08 RID: 19720 RVA: 0x001959AC File Offset: 0x00193DAC
	public void SkinMeshCPUandGPUApplyPostSkinMorphsFast()
	{
		if (this.allowPostSkinMorph)
		{
			for (int i = 0; i < this.postSkinNeededVertsList.Length; i++)
			{
				int num = this.postSkinNeededVertsList[i];
				this.workingVerts[num] += this.postSkinMorphs[num];
			}
		}
	}

	// Token: 0x06004D09 RID: 19721 RVA: 0x00195A14 File Offset: 0x00193E14
	public void SkinMeshCPUandGPUFinishFrameFast()
	{
		this.SkinMeshGPUDelay();
		if (this.debugVertex != -1)
		{
			MyDebug.DrawWireCube(this.workingVerts[this.debugVertex], 0.002f, Color.white);
		}
		if (this.allowPostSkinMorph)
		{
			for (int i = 0; i < this.postSkinNeededVertsList.Length; i++)
			{
				int num = this.postSkinNeededVertsList[i];
				this.postSkinVertsReady[num] = true;
				this.rawSkinnedVerts[num] = this.rawSkinnedWorkingVerts[num];
				this.postSkinNormals[num] = this.postSkinWorkingNormals[num];
			}
		}
		this.rawVertsBuffer.SetData(this.workingVerts);
		if (this._useSmoothing)
		{
			this.InitSmoothing();
			int num2 = 0;
			this.smoothedVertsBuffer = this.rawVertsBuffer;
			if (UserPreferences.singleton != null)
			{
				this.smoothOuterLoops = UserPreferences.singleton.smoothPasses;
			}
			for (int j = 0; j < this.smoothOuterLoops; j++)
			{
				for (int k = 0; k < this.laplacianSmoothPasses; k++)
				{
					if (num2 == 0)
					{
						this.meshSmoothGPU.LaplacianSmoothGPU(this.rawVertsBuffer, this._verticesBuffer2);
						this.meshSmoothGPU.HCCorrectionGPU(this.rawVertsBuffer, this._verticesBuffer2, this.laplacianSmoothBeta);
						this.smoothedVertsBuffer = this._verticesBuffer2;
					}
					else if (num2 % 2 == 0)
					{
						this.meshSmoothGPU.LaplacianSmoothGPU(this._verticesBuffer1, this._verticesBuffer2);
						this.meshSmoothGPU.HCCorrectionGPU(this._verticesBuffer1, this._verticesBuffer2, this.laplacianSmoothBeta);
						this.smoothedVertsBuffer = this._verticesBuffer2;
					}
					else
					{
						this.meshSmoothGPU.LaplacianSmoothGPU(this._verticesBuffer2, this._verticesBuffer1);
						this.meshSmoothGPU.HCCorrectionGPU(this._verticesBuffer2, this._verticesBuffer1, this.laplacianSmoothBeta);
						this.smoothedVertsBuffer = this._verticesBuffer1;
					}
					num2++;
				}
				for (int l = 0; l < this.springSmoothPasses; l++)
				{
					if (num2 == 0)
					{
						this.meshSmoothGPU.SpringSmoothGPU(this.rawVertsBuffer, this._verticesBuffer2, this.springSmoothFactor, 1f);
						this.smoothedVertsBuffer = this._verticesBuffer2;
					}
					else if (num2 % 2 == 0)
					{
						this.meshSmoothGPU.SpringSmoothGPU(this._verticesBuffer1, this._verticesBuffer2, this.springSmoothFactor, 1f);
						this.smoothedVertsBuffer = this._verticesBuffer2;
					}
					else
					{
						this.meshSmoothGPU.SpringSmoothGPU(this._verticesBuffer2, this._verticesBuffer1, this.springSmoothFactor, 1f);
						this.smoothedVertsBuffer = this._verticesBuffer1;
					}
					num2++;
				}
			}
			this.mapVerticesGPU.Map(this.smoothedVertsBuffer);
		}
		else
		{
			this.mapVerticesGPU.Map(this.rawVertsBuffer);
		}
		this.InitRecalcNormalsTangents();
		if (this._recalculateNormals)
		{
			if (this._useSmoothing && this.useSmoothVertsForNormalTangentRecalc)
			{
				this.recalcNormalsGPU.RecalculateNormals(this.smoothedVertsBuffer);
				this.recalcNormalsGPURaw.RecalculateSurfaceNormals(this.rawVertsBuffer);
			}
			else
			{
				this.recalcNormalsGPU.RecalculateNormals(this.rawVertsBuffer);
			}
		}
		if (this._recalculateTangents)
		{
			if (this._useSmoothing && this.useSmoothVertsForNormalTangentRecalc)
			{
				this.recalcTangentsGPU.RecalculateTangents(this.smoothedVertsBuffer, this._normalsBuffer);
			}
			else
			{
				this.recalcTangentsGPU.RecalculateTangents(this.rawVertsBuffer, this._normalsBuffer);
			}
		}
	}

	// Token: 0x06004D0A RID: 19722 RVA: 0x00195DCC File Offset: 0x001941CC
	protected void SkinMeshCPUandGPUFinishFrame()
	{
		if (this.useAsynchronousThreadedSkinning && this.dazMesh is DAZMergedMesh)
		{
			DAZMergedMesh dazmergedMesh = this.dazMesh as DAZMergedMesh;
			dazmergedMesh.UpdateVerticesPost(false);
		}
		if (this.allowPostSkinMorph)
		{
			for (int i = 0; i < this.postSkinNeededVertsList.Length; i++)
			{
				int num = this.postSkinNeededVertsList[i];
				this.workingVerts[num] += this.postSkinMorphs[num];
			}
		}
		if (this.debugVertex != -1)
		{
			MyDebug.DrawWireCube(this.workingVerts[this.debugVertex], 0.002f, Color.white);
		}
		this.rawVertsBuffer.SetData(this.workingVerts);
		if (this._useSmoothing)
		{
			this.InitSmoothing();
			int num2 = 0;
			this.smoothedVertsBuffer = this.rawVertsBuffer;
			if (UserPreferences.singleton != null)
			{
				this.smoothOuterLoops = UserPreferences.singleton.smoothPasses;
			}
			for (int j = 0; j < this.smoothOuterLoops; j++)
			{
				for (int k = 0; k < this.laplacianSmoothPasses; k++)
				{
					if (num2 == 0)
					{
						this.meshSmoothGPU.LaplacianSmoothGPU(this.rawVertsBuffer, this._verticesBuffer2);
						this.meshSmoothGPU.HCCorrectionGPU(this.rawVertsBuffer, this._verticesBuffer2, this.laplacianSmoothBeta);
						this.smoothedVertsBuffer = this._verticesBuffer2;
					}
					else if (num2 % 2 == 0)
					{
						this.meshSmoothGPU.LaplacianSmoothGPU(this._verticesBuffer1, this._verticesBuffer2);
						this.meshSmoothGPU.HCCorrectionGPU(this._verticesBuffer1, this._verticesBuffer2, this.laplacianSmoothBeta);
						this.smoothedVertsBuffer = this._verticesBuffer2;
					}
					else
					{
						this.meshSmoothGPU.LaplacianSmoothGPU(this._verticesBuffer2, this._verticesBuffer1);
						this.meshSmoothGPU.HCCorrectionGPU(this._verticesBuffer2, this._verticesBuffer1, this.laplacianSmoothBeta);
						this.smoothedVertsBuffer = this._verticesBuffer1;
					}
					num2++;
				}
				for (int l = 0; l < this.springSmoothPasses; l++)
				{
					if (num2 == 0)
					{
						this.meshSmoothGPU.SpringSmoothGPU(this.rawVertsBuffer, this._verticesBuffer2, this.springSmoothFactor, 1f);
						this.smoothedVertsBuffer = this._verticesBuffer2;
					}
					else if (num2 % 2 == 0)
					{
						this.meshSmoothGPU.SpringSmoothGPU(this._verticesBuffer1, this._verticesBuffer2, this.springSmoothFactor, 1f);
						this.smoothedVertsBuffer = this._verticesBuffer2;
					}
					else
					{
						this.meshSmoothGPU.SpringSmoothGPU(this._verticesBuffer2, this._verticesBuffer1, this.springSmoothFactor, 1f);
						this.smoothedVertsBuffer = this._verticesBuffer1;
					}
					num2++;
				}
			}
			this.mapVerticesGPU.Map(this.smoothedVertsBuffer);
		}
		else
		{
			this.mapVerticesGPU.Map(this.rawVertsBuffer);
		}
		this.InitRecalcNormalsTangents();
		if (this._recalculateNormals)
		{
			if (this._useSmoothing && this.useSmoothVertsForNormalTangentRecalc)
			{
				this.recalcNormalsGPU.RecalculateNormals(this.smoothedVertsBuffer);
				this.recalcNormalsGPURaw.RecalculateSurfaceNormals(this.rawVertsBuffer);
			}
			else
			{
				this.recalcNormalsGPU.RecalculateNormals(this.rawVertsBuffer);
			}
		}
		if (this._recalculateTangents)
		{
			if (this._useSmoothing && this.useSmoothVertsForNormalTangentRecalc)
			{
				this.recalcTangentsGPU.RecalculateTangents(this.smoothedVertsBuffer, this._normalsBuffer);
			}
			else
			{
				this.recalcTangentsGPU.RecalculateTangents(this.rawVertsBuffer, this._normalsBuffer);
			}
		}
	}

	// Token: 0x06004D0B RID: 19723 RVA: 0x00196190 File Offset: 0x00194590
	protected void SkinMeshCPUandGPUEarlyFinish()
	{
		if (this.useAsynchronousThreadedSkinning)
		{
			this.totalFrameCount++;
			while (this.mainSkinTask.working)
			{
				Thread.Sleep(0);
			}
			if (this.allowPostSkinMorph)
			{
				for (int i = 0; i < this.postSkinNeededVertsList.Length; i++)
				{
					int num = this.postSkinNeededVertsList[i];
					this.postSkinVertsReady[num] = true;
					this.rawSkinnedVerts[num] = this.rawSkinnedWorkingVerts[num];
					this.postSkinNormals[num] = this.postSkinWorkingNormals[num];
				}
			}
		}
	}

	// Token: 0x06004D0C RID: 19724 RVA: 0x00196250 File Offset: 0x00194650
	protected void SkinMeshCPUandGPU(bool forceSynchronous = false)
	{
		this.lastFrameSkinStartTime = this.skinStartTime;
		this.skinStartTime = (float)this.stopwatch.ElapsedTicks * this.f;
		if (this.mesh != null && this.root != null && this.GPUSkinner != null)
		{
			if (!forceSynchronous)
			{
				this.StartThreads();
			}
			this.SkinMeshGPUInit();
			if (this.useAsynchronousThreadedSkinning && !forceSynchronous)
			{
				this.totalFrameCount++;
				bool flag = true;
				if (flag)
				{
					while (this.mainSkinTask.working)
					{
						Thread.Sleep(0);
					}
				}
				if (!this.mainSkinTask.working)
				{
					this.SkinMeshCPUandGPUFinishFrame();
					this.RecalcBones();
					this.SkinMeshStartFrame(false);
					this.mainSkinTask.working = true;
					this.debugStartTime = (float)this.stopwatch.ElapsedTicks * this.f;
					this.mainSkinTask.resetEvent.Set();
					this.debugStopTime = (float)this.stopwatch.ElapsedTicks * this.f;
					this.debugTime = this.debugStopTime - this.debugStartTime;
				}
				else if (OVRManager.isHmdPresent)
				{
					this.missedFrameCount++;
					float num = (float)this.stopwatch.ElapsedTicks * this.f;
					float num2 = num - this.threadMainSkinStartTime;
					UnityEngine.Debug.LogWarning(string.Concat(new object[]
					{
						"Skinning did not complete in 1 frame for ",
						this.sceneGeometryId,
						". Last thread time ",
						this.threadMainSkinTime,
						" Current thread time ",
						num2,
						" missed ",
						this.missedFrameCount,
						" out of total ",
						this.totalFrameCount
					}));
					UnityEngine.Debug.LogWarning(string.Concat(new object[]
					{
						"Current time ",
						num,
						". Thread start time ",
						this.threadMainSkinStartTime,
						". Last thread stop time ",
						this.threadMainSkinStopTime,
						". Last frame skin start time ",
						this.lastFrameSkinStartTime
					}));
					DebugHUD.Msg(string.Concat(new object[]
					{
						"Skin miss ",
						this.missedFrameCount,
						" out of total ",
						this.totalFrameCount
					}));
					DebugHUD.Alert2();
				}
			}
			else
			{
				this.RecalcBones();
				this.SkinMeshStartFrame(forceSynchronous);
				this.SkinMeshThreaded(forceSynchronous);
				this.SkinMeshCPUandGPUFinishFrame();
			}
		}
		this.skinStopTime = (float)this.stopwatch.ElapsedTicks * this.f;
		this.skinTime = this.skinStopTime - this.skinStartTime;
	}

	// Token: 0x06004D0D RID: 19725 RVA: 0x00196534 File Offset: 0x00194934
	protected void SkinMeshStartFrame(bool forceSynchronous = false)
	{
		if (this.useAsynchronousThreadedSkinning && !forceSynchronous && this.skinMethod != DAZSkinV2.SkinMethod.GPU && this.dazMesh is DAZMergedMesh)
		{
			this.dazMesh.enabled = false;
			DAZMergedMesh dazmergedMesh = this.dazMesh as DAZMergedMesh;
			this.startVerts = dazmergedMesh.threadedMorphedUVVertices;
			this.workingVerts = this.startVerts;
			dazmergedMesh.UpdateVerticesPre(true);
			if (this._useGeneralWeights)
			{
				for (int i = 0; i < this.numBaseVerts; i++)
				{
					this.startVertsCopy[i] = this.startVerts[i];
				}
			}
		}
		else
		{
			this.dazMesh.enabled = true;
			this.startVerts = this.dazMesh.morphedUVVertices;
			this.workingVerts = this.workingVerts1;
			if (this._useGeneralWeights)
			{
				for (int j = 0; j < this.numBaseVerts; j++)
				{
					this.startVertsCopy[j] = this.startVerts[j];
				}
			}
			else
			{
				for (int k = 0; k < this.numBaseVerts; k++)
				{
					this.workingVerts[k] = this.startVerts[k];
				}
			}
		}
		this.postSkinVertsChangedThreaded = this.postSkinVertsChanged;
		this.postSkinVertsChanged = false;
	}

	// Token: 0x06004D0E RID: 19726 RVA: 0x001966AC File Offset: 0x00194AAC
	protected void SkinMeshFinishFrame()
	{
		if (this.useAsynchronousThreadedSkinning && this.skinMethod != DAZSkinV2.SkinMethod.GPU && this.dazMesh is DAZMergedMesh)
		{
			DAZMergedMesh dazmergedMesh = this.dazMesh as DAZMergedMesh;
			dazmergedMesh.UpdateVerticesPost(false);
		}
		if (this.allowPostSkinMorph)
		{
			for (int i = 0; i < this.postSkinNeededVertsList.Length; i++)
			{
				int num = this.postSkinNeededVertsList[i];
				this.postSkinVertsReady[num] = true;
				this.rawSkinnedVerts[num] = this.rawSkinnedWorkingVerts[num];
				this.postSkinNormals[num] = this.postSkinWorkingNormals[num];
			}
		}
		if (this._useSmoothing)
		{
			if (this.useSmoothVertsForNormalTangentRecalc)
			{
				for (int j = 0; j < this.numUVVerts; j++)
				{
					this.drawVerts[j] = this.smoothedVerts[j];
				}
			}
			else
			{
				for (int k = 0; k < this.numUVVerts; k++)
				{
					this.drawVerts[k] = this.smoothedVerts[k];
					this.unsmoothedVerts[k] = this.workingVerts[k];
				}
			}
		}
		else
		{
			for (int l = 0; l < this.numUVVerts; l++)
			{
				this.drawVerts[l] = this.workingVerts[l];
			}
		}
		this.mesh.vertices = this.drawVerts;
	}

	// Token: 0x06004D0F RID: 19727 RVA: 0x0019687C File Offset: 0x00194C7C
	public void RecalcBones()
	{
		for (int i = 0; i < this.numBones; i++)
		{
			DAZBone dazbone = this.dazBones[i];
			if (NaNUtils.IsMatrixValid(dazbone.morphedLocalToWorldMatrix) && NaNUtils.IsMatrixValid(dazbone.morphedWorldToLocalMatrix) && NaNUtils.IsMatrixValid(dazbone.changeFromOriginalMatrix))
			{
				this.boneWorldToLocalMatrices[i] = dazbone.morphedWorldToLocalMatrix;
				this.boneLocalToWorldMatrices[i] = dazbone.morphedLocalToWorldMatrix;
				this.boneChangeFromOriginalMatrices[i] = dazbone.changeFromOriginalMatrix;
				if (!this._useGeneralWeights)
				{
					this.boneRotationAngles[i] = dazbone.GetAngles();
				}
			}
			else
			{
				UnityEngine.Debug.LogError("Detected matrix corruption for bone " + dazbone.name);
			}
		}
	}

	// Token: 0x06004D10 RID: 19728 RVA: 0x0019695C File Offset: 0x00194D5C
	public void SkinMesh(bool forceSynchronous = false)
	{
		this.lastFrameSkinStartTime = this.skinStartTime;
		this.skinStartTime = (float)this.stopwatch.ElapsedTicks * this.f;
		if (this.mesh != null && this.root != null)
		{
			if (!forceSynchronous)
			{
				this.StartThreads();
			}
			this.InitRecalcNormalsTangents();
			if (this._useSmoothing)
			{
				this.InitSmoothing();
			}
			if (!forceSynchronous && !this.normalTangentTask.working && this.useAsynchronousNormalTangentRecalc)
			{
				if (this._recalculateNormals)
				{
					for (int i = 0; i < this.numUVVerts; i++)
					{
						this.drawNormals[i] = this.workingNormals[i];
					}
					for (int j = 0; j < this.workingSurfaceNormals.Length; j++)
					{
						this.drawSurfaceNormals[j] = this.workingSurfaceNormals[j];
					}
					this.mesh.normals = this.drawNormals;
				}
				if (this._recalculateTangents)
				{
					for (int k = 0; k < this.numUVVerts; k++)
					{
						this.drawTangents[k] = this.workingTangents[k];
					}
					this.mesh.tangents = this.drawTangents;
				}
				if (this._recalculateNormals || this._recalculateTangents)
				{
					if (this.useSmoothVertsForNormalTangentRecalc || !this._useSmoothing)
					{
						for (int l = 0; l < this.numUVVerts; l++)
						{
							this.workingVerts2[l] = this.drawVerts[l];
						}
					}
					else
					{
						for (int m = 0; m < this.numUVVerts; m++)
						{
							this.workingVerts2[m] = this.unsmoothedVerts[m];
						}
					}
					this.normalTangentTask.working = true;
					this.normalTangentTask.resetEvent.Set();
				}
			}
			if (this.useAsynchronousThreadedSkinning && !forceSynchronous)
			{
				if (!this.mainSkinTask.working)
				{
					this.SkinMeshFinishFrame();
					this.RecalcBones();
					this.SkinMeshStartFrame(false);
					this.mainSkinTask.working = true;
					this.debugStartTime = (float)this.stopwatch.ElapsedTicks * this.f;
					this.mainSkinTask.resetEvent.Set();
					this.debugStopTime = (float)this.stopwatch.ElapsedTicks * this.f;
					this.debugTime = this.debugStopTime - this.debugStartTime;
				}
			}
			else
			{
				this.RecalcBones();
				this.SkinMeshStartFrame(forceSynchronous);
				this.SkinMeshThreaded(forceSynchronous);
				this.SkinMeshFinishFrame();
			}
		}
		this.skinStopTime = (float)this.stopwatch.ElapsedTicks * this.f;
		this.skinTime = this.skinStopTime - this.skinStartTime;
	}

	// Token: 0x06004D11 RID: 19729 RVA: 0x00196C88 File Offset: 0x00195088
	protected void SkinMeshThreaded(bool forceSynchronous = false)
	{
		this.threadMainSkinStartTime = (float)this.stopwatch.ElapsedTicks * this.f;
		if (this.useAsynchronousThreadedSkinning && this.dazMesh is DAZMergedMesh)
		{
			DAZMergedMesh dazmergedMesh = this.dazMesh as DAZMergedMesh;
			dazmergedMesh.UpdateVerticesThreaded(false);
		}
		int num = this.numBaseVerts;
		if (this._useGeneralWeights)
		{
			for (int i = 0; i < num; i++)
			{
				this.workingVerts[i].x = 0f;
				this.workingVerts[i].y = 0f;
				this.workingVerts[i].z = 0f;
			}
		}
		if (this.useMultithreadedSkinning && !forceSynchronous && this._numSubThreads > 0)
		{
			int num2 = (this.numUVVerts - this.numUVOnlyVerts) / this._numSubThreads;
			for (int j = 0; j < this._numSubThreads; j++)
			{
				this.tasks[j].taskType = DAZSkinTaskType.Skin;
				this.tasks[j].index1 = j * num2;
				if (j == this._numSubThreads - 1)
				{
					this.tasks[j].index2 = num - 1;
				}
				else
				{
					this.tasks[j].index2 = (j + 1) * num2 - 1;
				}
				this.threadSkinVertsCount[j] = this.tasks[j].index2 - this.tasks[j].index1 + 1;
				this.tasks[j].working = true;
				this.tasks[j].resetEvent.Set();
			}
			bool flag;
			do
			{
				flag = false;
				for (int k = 0; k < this._numSubThreads; k++)
				{
					if (this.tasks[k].working)
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
			this.mainThreadSkinStartTime = (float)this.stopwatch.ElapsedTicks * this.f;
			this.SkinMeshPart(0, num - 1, this.isBaseVert, null);
			this.mainThreadSkinStopTime = (float)this.stopwatch.ElapsedTicks * this.f;
			this.mainThreadSkinTime = this.mainThreadSkinStopTime - this.mainThreadSkinStartTime;
		}
		if (this.postSkinVertsChangedThreaded)
		{
			this.RecalculatePostSkinNeededVertsAndTriangles();
			this.postSkinBones = this.DetermineUsedBonesForVerts(this.postSkinNeededVerts);
			this.postSkinVertsChangedThreaded = false;
		}
		if (this.allowPostSkinMorph)
		{
			if (this.skinMethod == DAZSkinV2.SkinMethod.CPU)
			{
				for (int l = 0; l < this.postSkinNeededVertsList.Length; l++)
				{
					int num3 = this.postSkinNeededVertsList[l];
					this.rawSkinnedWorkingVerts[num3] = this.workingVerts[num3];
					this.workingVerts[num3] += this.postSkinMorphs[num3];
				}
			}
			else
			{
				for (int m = 0; m < this.postSkinNeededVertsList.Length; m++)
				{
					int num4 = this.postSkinNeededVertsList[m];
					this.rawSkinnedWorkingVerts[num4] = this.workingVerts[num4];
				}
			}
		}
		if (this.needsPostSkinNormals)
		{
			this.threadRecalcNormalTangentStartTime = (float)this.stopwatch.ElapsedTicks * this.f;
			this.postSkinRecalcNormals.recalculateNormals(this.postSkinNeededTriangles, this.postSkinNeededVertsList);
			this.threadRecalcNormalTangentStopTime = (float)this.stopwatch.ElapsedTicks * this.f;
			this.threadRecalcNormalTangentTime = this.threadRecalcNormalTangentStopTime - this.threadRecalcNormalTangentStartTime;
		}
		if (this._useSmoothing && this.skinMethod == DAZSkinV2.SkinMethod.CPU)
		{
			this.InitSmoothing();
			if (this.useMultithreadedSmoothing && !forceSynchronous && this._numSubThreads > 0)
			{
				int num5 = this.numBaseVerts / this._numSubThreads;
				for (int n = 0; n < this._numSubThreads; n++)
				{
					this.tasks[n].taskType = DAZSkinTaskType.Smooth;
					this.tasks[n].index1 = n * num5;
					if (n == this._numSubThreads - 1)
					{
						this.tasks[n].index2 = this.numBaseVerts - 1;
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
						this.tasks[num7].index2 = this.numBaseVerts - 1;
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
			}
			else
			{
				if (this.useAsynchronousThreadedSkinning && !forceSynchronous)
				{
					Thread.Sleep(0);
				}
				this.mainThreadSmoothStartTime = (float)this.stopwatch.ElapsedTicks * this.f;
				this.meshSmooth.LaplacianSmooth(this.workingVerts, this.smoothedVerts, 0, 100000000);
				this.mainThreadSmoothStopTime = (float)this.stopwatch.ElapsedTicks * this.f;
				this.mainThreadSmoothTime = this.mainThreadSmoothStopTime - this.mainThreadSmoothStartTime;
				this.mainThreadSmoothCorrectionStartTime = (float)this.stopwatch.ElapsedTicks * this.f;
				this.meshSmooth.HCCorrection(this.workingVerts, this.smoothedVerts, this.laplacianSmoothBeta, 0, 1000000000);
				this.mainThreadSmoothCorrectionStopTime = (float)this.stopwatch.ElapsedTicks * this.f;
				this.mainThreadSmoothCorrectionTime = this.mainThreadSmoothCorrectionStopTime - this.mainThreadSmoothCorrectionStartTime;
				foreach (DAZVertexMap dazvertexMap2 in this.dazMesh.baseVerticesToUVVertices)
				{
					this.smoothedVerts[dazvertexMap2.tovert] = this.smoothedVerts[dazvertexMap2.fromvert];
				}
			}
		}
		else
		{
			foreach (DAZVertexMap dazvertexMap3 in this.dazMesh.baseVerticesToUVVertices)
			{
				this.workingVerts[dazvertexMap3.tovert] = this.workingVerts[dazvertexMap3.fromvert];
			}
		}
		this.threadMainSkinStopTime = (float)this.stopwatch.ElapsedTicks * this.f;
		this.threadMainSkinTime = this.threadMainSkinStopTime - this.threadMainSkinStartTime;
	}

	// Token: 0x06004D12 RID: 19730 RVA: 0x0019749C File Offset: 0x0019589C
	protected void SkinMeshPart(int startIndex, int stopIndex, bool[] onlyVerts = null, bool[] onlyBones = null)
	{
		for (int i = 0; i < this._numBones; i++)
		{
			if (onlyBones == null || onlyBones[i])
			{
				DAZSkinV2VertexWeights[] weights = this.nodes[i].weights;
				int[] fullyWeightedVertices = this.nodes[i].fullyWeightedVertices;
				float m = this.boneChangeFromOriginalMatrices[i].m00;
				float m2 = this.boneChangeFromOriginalMatrices[i].m01;
				float m3 = this.boneChangeFromOriginalMatrices[i].m02;
				float m4 = this.boneChangeFromOriginalMatrices[i].m03;
				float m5 = this.boneChangeFromOriginalMatrices[i].m10;
				float m6 = this.boneChangeFromOriginalMatrices[i].m11;
				float m7 = this.boneChangeFromOriginalMatrices[i].m12;
				float m8 = this.boneChangeFromOriginalMatrices[i].m13;
				float m9 = this.boneChangeFromOriginalMatrices[i].m20;
				float m10 = this.boneChangeFromOriginalMatrices[i].m21;
				float m11 = this.boneChangeFromOriginalMatrices[i].m22;
				float m12 = this.boneChangeFromOriginalMatrices[i].m23;
				if (this._useGeneralWeights)
				{
					foreach (DAZSkinV2GeneralVertexWeights dazskinV2GeneralVertexWeights in this.nodes[i].generalWeights)
					{
						if (dazskinV2GeneralVertexWeights.vertex >= startIndex && dazskinV2GeneralVertexWeights.vertex <= stopIndex)
						{
							Vector3 vector = this.startVertsCopy[dazskinV2GeneralVertexWeights.vertex];
							Vector3[] array = this.workingVerts;
							int vertex = dazskinV2GeneralVertexWeights.vertex;
							array[vertex].x = array[vertex].x + (vector.x * m + vector.y * m2 + vector.z * m3 + m4) * dazskinV2GeneralVertexWeights.weight;
							Vector3[] array2 = this.workingVerts;
							int vertex2 = dazskinV2GeneralVertexWeights.vertex;
							array2[vertex2].y = array2[vertex2].y + (vector.x * m5 + vector.y * m6 + vector.z * m7 + m8) * dazskinV2GeneralVertexWeights.weight;
							Vector3[] array3 = this.workingVerts;
							int vertex3 = dazskinV2GeneralVertexWeights.vertex;
							array3[vertex3].z = array3[vertex3].z + (vector.x * m9 + vector.y * m10 + vector.z * m11 + m12) * dazskinV2GeneralVertexWeights.weight;
						}
					}
				}
				else
				{
					DAZSkinV2BulgeFactors bulgeFactors = this.nodes[i].bulgeFactors;
					Quaternion2Angles.RotationOrder rotationOrder = this.nodes[i].rotationOrder;
					Matrix4x4 matrix4x = this.boneWorldToLocalMatrices[i];
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
					Matrix4x4 matrix4x2 = this.boneLocalToWorldMatrices[i];
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
					Vector3 vector2 = this.boneRotationAngles[i];
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
					if (vector2.x > 0.01f)
					{
						if (bulgeFactors.xposleft != 0f)
						{
							flag = true;
							flag2 = true;
							num = bulgeFactors.xposleft * vector2.x * this.bulgeScale;
						}
						if (bulgeFactors.xposright != 0f)
						{
							flag = true;
							flag4 = true;
							num3 = bulgeFactors.xposright * vector2.x * this.bulgeScale;
						}
					}
					else if (vector2.x < -0.01f)
					{
						if (bulgeFactors.xnegleft != 0f)
						{
							flag = true;
							flag3 = true;
							num2 = bulgeFactors.xnegleft * vector2.x * this.bulgeScale;
						}
						if (bulgeFactors.xnegright != 0f)
						{
							flag = true;
							flag5 = true;
							num4 = bulgeFactors.xnegright * vector2.x * this.bulgeScale;
						}
					}
					if (vector2.y > 0.01f)
					{
						if (bulgeFactors.yposleft != 0f)
						{
							flag6 = true;
							flag7 = true;
							num5 = bulgeFactors.yposleft * vector2.y * this.bulgeScale;
						}
						if (bulgeFactors.yposright != 0f)
						{
							flag6 = true;
							flag9 = true;
							num7 = bulgeFactors.yposright * vector2.y * this.bulgeScale;
						}
					}
					else if (vector2.y < -0.01f)
					{
						if (bulgeFactors.ynegleft != 0f)
						{
							flag6 = true;
							flag8 = true;
							num6 = bulgeFactors.ynegleft * vector2.y * this.bulgeScale;
						}
						if (bulgeFactors.ynegright != 0f)
						{
							flag6 = true;
							flag10 = true;
							num8 = bulgeFactors.ynegright * vector2.y * this.bulgeScale;
						}
					}
					if (vector2.z > 0.01f)
					{
						if (bulgeFactors.zposleft != 0f)
						{
							flag11 = true;
							flag12 = true;
							num9 = bulgeFactors.zposleft * vector2.z * this.bulgeScale;
						}
						if (bulgeFactors.zposright != 0f)
						{
							flag11 = true;
							flag14 = true;
							num11 = bulgeFactors.zposright * vector2.z * this.bulgeScale;
						}
					}
					else if (vector2.z < -0.01f)
					{
						if (bulgeFactors.znegleft != 0f)
						{
							flag11 = true;
							flag13 = true;
							num10 = bulgeFactors.znegleft * vector2.z * this.bulgeScale;
						}
						if (bulgeFactors.znegright != 0f)
						{
							flag11 = true;
							flag15 = true;
							num12 = bulgeFactors.znegright * vector2.z * this.bulgeScale;
						}
					}
					if (rotationOrder == Quaternion2Angles.RotationOrder.XYZ)
					{
						foreach (DAZSkinV2VertexWeights dazskinV2VertexWeights in weights)
						{
							if (dazskinV2VertexWeights.vertex >= startIndex && dazskinV2VertexWeights.vertex <= stopIndex && (onlyVerts == null || onlyVerts[dazskinV2VertexWeights.vertex]))
							{
								if (dazskinV2VertexWeights.xweight > 0.99999f && dazskinV2VertexWeights.yweight > 0.99999f && dazskinV2VertexWeights.zweight > 0.99999f)
								{
									Vector3 vector3 = this.workingVerts[dazskinV2VertexWeights.vertex];
									this.workingVerts[dazskinV2VertexWeights.vertex].x = vector3.x * m + vector3.y * m2 + vector3.z * m3 + m4;
									this.workingVerts[dazskinV2VertexWeights.vertex].y = vector3.x * m5 + vector3.y * m6 + vector3.z * m7 + m8;
									this.workingVerts[dazskinV2VertexWeights.vertex].z = vector3.x * m9 + vector3.y * m10 + vector3.z * m11 + m12;
								}
								else
								{
									Vector3 vector4 = this.workingVerts[dazskinV2VertexWeights.vertex];
									Vector3 vector5;
									vector5.x = vector4.x * m13 + vector4.y * m14 + vector4.z * m15 + m16;
									vector5.y = vector4.x * m17 + vector4.y * m18 + vector4.z * m19 + m20;
									vector5.z = vector4.x * m21 + vector4.y * m22 + vector4.z * m23 + m24;
									if (dazskinV2VertexWeights.zweight > 0f)
									{
										float num13 = vector2.z * dazskinV2VertexWeights.zweight;
										float num14 = (float)Math.Sin((double)num13);
										float num15 = (float)Math.Cos((double)num13);
										float x = vector5.x * num15 - vector5.y * num14;
										vector5.y = vector5.x * num14 + vector5.y * num15;
										vector5.x = x;
									}
									if (flag11)
									{
										if (flag12 && dazskinV2VertexWeights.zleftbulge > 0f)
										{
											float num16 = num9 * dazskinV2VertexWeights.zleftbulge;
											vector5.x += vector5.x * num16;
											vector5.y += vector5.y * num16;
										}
										if (flag14 && dazskinV2VertexWeights.zrightbulge > 0f)
										{
											float num17 = num11 * dazskinV2VertexWeights.zrightbulge;
											vector5.x += vector5.x * num17;
											vector5.y += vector5.y * num17;
										}
										if (flag13 && dazskinV2VertexWeights.zleftbulge > 0f)
										{
											float num18 = num10 * dazskinV2VertexWeights.zleftbulge;
											vector5.x += vector5.x * num18;
											vector5.y += vector5.y * num18;
										}
										if (flag15 && dazskinV2VertexWeights.zrightbulge > 0f)
										{
											float num19 = num12 * dazskinV2VertexWeights.zrightbulge;
											vector5.x += vector5.x * num19;
											vector5.y += vector5.y * num19;
										}
									}
									if (dazskinV2VertexWeights.yweight > 0f)
									{
										float num20 = vector2.y * dazskinV2VertexWeights.yweight;
										float num21 = (float)Math.Sin((double)num20);
										float num22 = (float)Math.Cos((double)num20);
										float x2 = vector5.x * num22 + vector5.z * num21;
										vector5.z = vector5.z * num22 - vector5.x * num21;
										vector5.x = x2;
									}
									if (flag6)
									{
										if (flag7 && dazskinV2VertexWeights.yleftbulge > 0f)
										{
											float num23 = num5 * dazskinV2VertexWeights.yleftbulge;
											vector5.x += vector5.x * num23;
											vector5.z += vector5.z * num23;
										}
										if (flag9 && dazskinV2VertexWeights.yrightbulge > 0f)
										{
											float num24 = num7 * dazskinV2VertexWeights.yrightbulge;
											vector5.x += vector5.x * num24;
											vector5.z += vector5.z * num24;
										}
										if (flag8 && dazskinV2VertexWeights.yleftbulge > 0f)
										{
											float num25 = num6 * dazskinV2VertexWeights.yleftbulge;
											vector5.x += vector5.x * num25;
											vector5.z += vector5.z * num25;
										}
										if (flag10 && dazskinV2VertexWeights.yrightbulge > 0f)
										{
											float num26 = num8 * dazskinV2VertexWeights.yrightbulge;
											vector5.x += vector5.x * num26;
											vector5.z += vector5.z * num26;
										}
									}
									if (dazskinV2VertexWeights.xweight > 0f)
									{
										float num27 = vector2.x * dazskinV2VertexWeights.xweight;
										float num28 = (float)Math.Sin((double)num27);
										float num29 = (float)Math.Cos((double)num27);
										float y = vector5.y * num29 - vector5.z * num28;
										vector5.z = vector5.y * num28 + vector5.z * num29;
										vector5.y = y;
									}
									if (flag)
									{
										if (flag2 && dazskinV2VertexWeights.xleftbulge > 0f)
										{
											float num30 = num * dazskinV2VertexWeights.xleftbulge;
											vector5.y += vector5.y * num30;
											vector5.z += vector5.z * num30;
										}
										if (flag4 && dazskinV2VertexWeights.xrightbulge > 0f)
										{
											float num31 = num3 * dazskinV2VertexWeights.xrightbulge;
											vector5.y += vector5.y * num31;
											vector5.z += vector5.z * num31;
										}
										if (flag3 && dazskinV2VertexWeights.xleftbulge > 0f)
										{
											float num32 = num2 * dazskinV2VertexWeights.xleftbulge;
											vector5.y += vector5.y * num32;
											vector5.z += vector5.z * num32;
										}
										if (flag5 && dazskinV2VertexWeights.xrightbulge > 0f)
										{
											float num33 = num4 * dazskinV2VertexWeights.xrightbulge;
											vector5.y += vector5.y * num33;
											vector5.z += vector5.z * num33;
										}
									}
									this.workingVerts[dazskinV2VertexWeights.vertex].x = vector5.x * m25 + vector5.y * m26 + vector5.z * m27 + m28;
									this.workingVerts[dazskinV2VertexWeights.vertex].y = vector5.x * m29 + vector5.y * m30 + vector5.z * m31 + m32;
									this.workingVerts[dazskinV2VertexWeights.vertex].z = vector5.x * m33 + vector5.y * m34 + vector5.z * m35 + m36;
								}
							}
						}
					}
					else if (rotationOrder == Quaternion2Angles.RotationOrder.XZY)
					{
						foreach (DAZSkinV2VertexWeights dazskinV2VertexWeights2 in weights)
						{
							if (dazskinV2VertexWeights2.vertex >= startIndex && dazskinV2VertexWeights2.vertex <= stopIndex && (onlyVerts == null || onlyVerts[dazskinV2VertexWeights2.vertex]))
							{
								if (dazskinV2VertexWeights2.xweight > 0.99999f && dazskinV2VertexWeights2.yweight > 0.99999f && dazskinV2VertexWeights2.zweight > 0.99999f)
								{
									Vector3 vector6 = this.workingVerts[dazskinV2VertexWeights2.vertex];
									this.workingVerts[dazskinV2VertexWeights2.vertex].x = vector6.x * m + vector6.y * m2 + vector6.z * m3 + m4;
									this.workingVerts[dazskinV2VertexWeights2.vertex].y = vector6.x * m5 + vector6.y * m6 + vector6.z * m7 + m8;
									this.workingVerts[dazskinV2VertexWeights2.vertex].z = vector6.x * m9 + vector6.y * m10 + vector6.z * m11 + m12;
								}
								else
								{
									Vector3 vector7 = this.workingVerts[dazskinV2VertexWeights2.vertex];
									Vector3 vector8;
									vector8.x = vector7.x * m13 + vector7.y * m14 + vector7.z * m15 + m16;
									vector8.y = vector7.x * m17 + vector7.y * m18 + vector7.z * m19 + m20;
									vector8.z = vector7.x * m21 + vector7.y * m22 + vector7.z * m23 + m24;
									if (dazskinV2VertexWeights2.yweight > 0f)
									{
										float num34 = vector2.y * dazskinV2VertexWeights2.yweight;
										float num35 = (float)Math.Sin((double)num34);
										float num36 = (float)Math.Cos((double)num34);
										float x3 = vector8.x * num36 + vector8.z * num35;
										vector8.z = vector8.z * num36 - vector8.x * num35;
										vector8.x = x3;
									}
									if (flag6)
									{
										if (flag7 && dazskinV2VertexWeights2.yleftbulge > 0f)
										{
											float num37 = num5 * dazskinV2VertexWeights2.yleftbulge;
											vector8.x += vector8.x * num37;
											vector8.z += vector8.z * num37;
										}
										if (flag9 && dazskinV2VertexWeights2.yrightbulge > 0f)
										{
											float num38 = num7 * dazskinV2VertexWeights2.yrightbulge;
											vector8.x += vector8.x * num38;
											vector8.z += vector8.z * num38;
										}
										if (flag8 && dazskinV2VertexWeights2.yleftbulge > 0f)
										{
											float num39 = num6 * dazskinV2VertexWeights2.yleftbulge;
											vector8.x += vector8.x * num39;
											vector8.z += vector8.z * num39;
										}
										if (flag10 && dazskinV2VertexWeights2.yrightbulge > 0f)
										{
											float num40 = num8 * dazskinV2VertexWeights2.yrightbulge;
											vector8.x += vector8.x * num40;
											vector8.z += vector8.z * num40;
										}
									}
									if (dazskinV2VertexWeights2.zweight > 0f)
									{
										float num41 = vector2.z * dazskinV2VertexWeights2.zweight;
										float num42 = (float)Math.Sin((double)num41);
										float num43 = (float)Math.Cos((double)num41);
										float x4 = vector8.x * num43 - vector8.y * num42;
										vector8.y = vector8.x * num42 + vector8.y * num43;
										vector8.x = x4;
									}
									if (flag11)
									{
										if (flag12 && dazskinV2VertexWeights2.zleftbulge > 0f)
										{
											float num44 = num9 * dazskinV2VertexWeights2.zleftbulge;
											vector8.x += vector8.x * num44;
											vector8.y += vector8.y * num44;
										}
										if (flag14 && dazskinV2VertexWeights2.zrightbulge > 0f)
										{
											float num45 = num11 * dazskinV2VertexWeights2.zrightbulge;
											vector8.x += vector8.x * num45;
											vector8.y += vector8.y * num45;
										}
										if (flag13 && dazskinV2VertexWeights2.zleftbulge > 0f)
										{
											float num46 = num10 * dazskinV2VertexWeights2.zleftbulge;
											vector8.x += vector8.x * num46;
											vector8.y += vector8.y * num46;
										}
										if (flag15 && dazskinV2VertexWeights2.zrightbulge > 0f)
										{
											float num47 = num12 * dazskinV2VertexWeights2.zrightbulge;
											vector8.x += vector8.x * num47;
											vector8.y += vector8.y * num47;
										}
									}
									if (dazskinV2VertexWeights2.xweight > 0f)
									{
										float num48 = vector2.x * dazskinV2VertexWeights2.xweight;
										float num49 = (float)Math.Sin((double)num48);
										float num50 = (float)Math.Cos((double)num48);
										float y2 = vector8.y * num50 - vector8.z * num49;
										vector8.z = vector8.y * num49 + vector8.z * num50;
										vector8.y = y2;
									}
									if (flag)
									{
										if (flag2 && dazskinV2VertexWeights2.xleftbulge > 0f)
										{
											float num51 = num * dazskinV2VertexWeights2.xleftbulge;
											vector8.y += vector8.y * num51;
											vector8.z += vector8.z * num51;
										}
										if (flag4 && dazskinV2VertexWeights2.xrightbulge > 0f)
										{
											float num52 = num3 * dazskinV2VertexWeights2.xrightbulge;
											vector8.y += vector8.y * num52;
											vector8.z += vector8.z * num52;
										}
										if (flag3 && dazskinV2VertexWeights2.xleftbulge > 0f)
										{
											float num53 = num2 * dazskinV2VertexWeights2.xleftbulge;
											vector8.y += vector8.y * num53;
											vector8.z += vector8.z * num53;
										}
										if (flag5 && dazskinV2VertexWeights2.xrightbulge > 0f)
										{
											float num54 = num4 * dazskinV2VertexWeights2.xrightbulge;
											vector8.y += vector8.y * num54;
											vector8.z += vector8.z * num54;
										}
									}
									this.workingVerts[dazskinV2VertexWeights2.vertex].x = vector8.x * m25 + vector8.y * m26 + vector8.z * m27 + m28;
									this.workingVerts[dazskinV2VertexWeights2.vertex].y = vector8.x * m29 + vector8.y * m30 + vector8.z * m31 + m32;
									this.workingVerts[dazskinV2VertexWeights2.vertex].z = vector8.x * m33 + vector8.y * m34 + vector8.z * m35 + m36;
								}
							}
						}
					}
					else if (rotationOrder == Quaternion2Angles.RotationOrder.YXZ)
					{
						foreach (DAZSkinV2VertexWeights dazskinV2VertexWeights3 in weights)
						{
							if (dazskinV2VertexWeights3.vertex >= startIndex && dazskinV2VertexWeights3.vertex <= stopIndex && (onlyVerts == null || onlyVerts[dazskinV2VertexWeights3.vertex]))
							{
								if (dazskinV2VertexWeights3.xweight > 0.99999f && dazskinV2VertexWeights3.yweight > 0.99999f && dazskinV2VertexWeights3.zweight > 0.99999f)
								{
									Vector3 vector9 = this.workingVerts[dazskinV2VertexWeights3.vertex];
									this.workingVerts[dazskinV2VertexWeights3.vertex].x = vector9.x * m + vector9.y * m2 + vector9.z * m3 + m4;
									this.workingVerts[dazskinV2VertexWeights3.vertex].y = vector9.x * m5 + vector9.y * m6 + vector9.z * m7 + m8;
									this.workingVerts[dazskinV2VertexWeights3.vertex].z = vector9.x * m9 + vector9.y * m10 + vector9.z * m11 + m12;
								}
								else
								{
									Vector3 vector10 = this.workingVerts[dazskinV2VertexWeights3.vertex];
									Vector3 vector11;
									vector11.x = vector10.x * m13 + vector10.y * m14 + vector10.z * m15 + m16;
									vector11.y = vector10.x * m17 + vector10.y * m18 + vector10.z * m19 + m20;
									vector11.z = vector10.x * m21 + vector10.y * m22 + vector10.z * m23 + m24;
									if (dazskinV2VertexWeights3.zweight > 0f)
									{
										float num55 = vector2.z * dazskinV2VertexWeights3.zweight;
										float num56 = (float)Math.Sin((double)num55);
										float num57 = (float)Math.Cos((double)num55);
										float x5 = vector11.x * num57 - vector11.y * num56;
										vector11.y = vector11.x * num56 + vector11.y * num57;
										vector11.x = x5;
									}
									if (flag11)
									{
										if (flag12 && dazskinV2VertexWeights3.zleftbulge > 0f)
										{
											float num58 = num9 * dazskinV2VertexWeights3.zleftbulge;
											vector11.x += vector11.x * num58;
											vector11.y += vector11.y * num58;
										}
										if (flag14 && dazskinV2VertexWeights3.zrightbulge > 0f)
										{
											float num59 = num11 * dazskinV2VertexWeights3.zrightbulge;
											vector11.x += vector11.x * num59;
											vector11.y += vector11.y * num59;
										}
										if (flag13 && dazskinV2VertexWeights3.zleftbulge > 0f)
										{
											float num60 = num10 * dazskinV2VertexWeights3.zleftbulge;
											vector11.x += vector11.x * num60;
											vector11.y += vector11.y * num60;
										}
										if (flag15 && dazskinV2VertexWeights3.zrightbulge > 0f)
										{
											float num61 = num12 * dazskinV2VertexWeights3.zrightbulge;
											vector11.x += vector11.x * num61;
											vector11.y += vector11.y * num61;
										}
									}
									if (dazskinV2VertexWeights3.xweight > 0f)
									{
										float num62 = vector2.x * dazskinV2VertexWeights3.xweight;
										float num63 = (float)Math.Sin((double)num62);
										float num64 = (float)Math.Cos((double)num62);
										float y3 = vector11.y * num64 - vector11.z * num63;
										vector11.z = vector11.y * num63 + vector11.z * num64;
										vector11.y = y3;
									}
									if (flag)
									{
										if (flag2 && dazskinV2VertexWeights3.xleftbulge > 0f)
										{
											float num65 = num * dazskinV2VertexWeights3.xleftbulge;
											vector11.y += vector11.y * num65;
											vector11.z += vector11.z * num65;
										}
										if (flag4 && dazskinV2VertexWeights3.xrightbulge > 0f)
										{
											float num66 = num3 * dazskinV2VertexWeights3.xrightbulge;
											vector11.y += vector11.y * num66;
											vector11.z += vector11.z * num66;
										}
										if (flag3 && dazskinV2VertexWeights3.xleftbulge > 0f)
										{
											float num67 = num2 * dazskinV2VertexWeights3.xleftbulge;
											vector11.y += vector11.y * num67;
											vector11.z += vector11.z * num67;
										}
										if (flag5 && dazskinV2VertexWeights3.xrightbulge > 0f)
										{
											float num68 = num4 * dazskinV2VertexWeights3.xrightbulge;
											vector11.y += vector11.y * num68;
											vector11.z += vector11.z * num68;
										}
									}
									if (dazskinV2VertexWeights3.yweight > 0f)
									{
										float num69 = vector2.y * dazskinV2VertexWeights3.yweight;
										float num70 = (float)Math.Sin((double)num69);
										float num71 = (float)Math.Cos((double)num69);
										float x6 = vector11.x * num71 + vector11.z * num70;
										vector11.z = vector11.z * num71 - vector11.x * num70;
										vector11.x = x6;
									}
									if (flag6)
									{
										if (flag7 && dazskinV2VertexWeights3.yleftbulge > 0f)
										{
											float num72 = num5 * dazskinV2VertexWeights3.yleftbulge;
											vector11.x += vector11.x * num72;
											vector11.z += vector11.z * num72;
										}
										if (flag9 && dazskinV2VertexWeights3.yrightbulge > 0f)
										{
											float num73 = num7 * dazskinV2VertexWeights3.yrightbulge;
											vector11.x += vector11.x * num73;
											vector11.z += vector11.z * num73;
										}
										if (flag8 && dazskinV2VertexWeights3.yleftbulge > 0f)
										{
											float num74 = num6 * dazskinV2VertexWeights3.yleftbulge;
											vector11.x += vector11.x * num74;
											vector11.z += vector11.z * num74;
										}
										if (flag10 && dazskinV2VertexWeights3.yrightbulge > 0f)
										{
											float num75 = num8 * dazskinV2VertexWeights3.yrightbulge;
											vector11.x += vector11.x * num75;
											vector11.z += vector11.z * num75;
										}
									}
									this.workingVerts[dazskinV2VertexWeights3.vertex].x = vector11.x * m25 + vector11.y * m26 + vector11.z * m27 + m28;
									this.workingVerts[dazskinV2VertexWeights3.vertex].y = vector11.x * m29 + vector11.y * m30 + vector11.z * m31 + m32;
									this.workingVerts[dazskinV2VertexWeights3.vertex].z = vector11.x * m33 + vector11.y * m34 + vector11.z * m35 + m36;
								}
							}
						}
					}
					else if (rotationOrder == Quaternion2Angles.RotationOrder.YZX)
					{
						foreach (DAZSkinV2VertexWeights dazskinV2VertexWeights4 in weights)
						{
							if (dazskinV2VertexWeights4.vertex >= startIndex && dazskinV2VertexWeights4.vertex <= stopIndex && (onlyVerts == null || onlyVerts[dazskinV2VertexWeights4.vertex]))
							{
								if (dazskinV2VertexWeights4.xweight > 0.99999f && dazskinV2VertexWeights4.yweight > 0.99999f && dazskinV2VertexWeights4.zweight > 0.99999f)
								{
									Vector3 vector12 = this.workingVerts[dazskinV2VertexWeights4.vertex];
									this.workingVerts[dazskinV2VertexWeights4.vertex].x = vector12.x * m + vector12.y * m2 + vector12.z * m3 + m4;
									this.workingVerts[dazskinV2VertexWeights4.vertex].y = vector12.x * m5 + vector12.y * m6 + vector12.z * m7 + m8;
									this.workingVerts[dazskinV2VertexWeights4.vertex].z = vector12.x * m9 + vector12.y * m10 + vector12.z * m11 + m12;
								}
								else
								{
									Vector3 vector13 = this.workingVerts[dazskinV2VertexWeights4.vertex];
									Vector3 vector14;
									vector14.x = vector13.x * m13 + vector13.y * m14 + vector13.z * m15 + m16;
									vector14.y = vector13.x * m17 + vector13.y * m18 + vector13.z * m19 + m20;
									vector14.z = vector13.x * m21 + vector13.y * m22 + vector13.z * m23 + m24;
									if (dazskinV2VertexWeights4.xweight > 0f)
									{
										float num77 = vector2.x * dazskinV2VertexWeights4.xweight;
										float num78 = (float)Math.Sin((double)num77);
										float num79 = (float)Math.Cos((double)num77);
										float y4 = vector14.y * num79 - vector14.z * num78;
										vector14.z = vector14.y * num78 + vector14.z * num79;
										vector14.y = y4;
									}
									if (flag)
									{
										if (flag2 && dazskinV2VertexWeights4.xleftbulge > 0f)
										{
											float num80 = num * dazskinV2VertexWeights4.xleftbulge;
											vector14.y += vector14.y * num80;
											vector14.z += vector14.z * num80;
										}
										if (flag4 && dazskinV2VertexWeights4.xrightbulge > 0f)
										{
											float num81 = num3 * dazskinV2VertexWeights4.xrightbulge;
											vector14.y += vector14.y * num81;
											vector14.z += vector14.z * num81;
										}
										if (flag3 && dazskinV2VertexWeights4.xleftbulge > 0f)
										{
											float num82 = num2 * dazskinV2VertexWeights4.xleftbulge;
											vector14.y += vector14.y * num82;
											vector14.z += vector14.z * num82;
										}
										if (flag5 && dazskinV2VertexWeights4.xrightbulge > 0f)
										{
											float num83 = num4 * dazskinV2VertexWeights4.xrightbulge;
											vector14.y += vector14.y * num83;
											vector14.z += vector14.z * num83;
										}
									}
									if (dazskinV2VertexWeights4.zweight > 0f)
									{
										float num84 = vector2.z * dazskinV2VertexWeights4.zweight;
										float num85 = (float)Math.Sin((double)num84);
										float num86 = (float)Math.Cos((double)num84);
										float x7 = vector14.x * num86 - vector14.y * num85;
										vector14.y = vector14.x * num85 + vector14.y * num86;
										vector14.x = x7;
									}
									if (flag11)
									{
										if (flag12 && dazskinV2VertexWeights4.zleftbulge > 0f)
										{
											float num87 = num9 * dazskinV2VertexWeights4.zleftbulge;
											vector14.x += vector14.x * num87;
											vector14.y += vector14.y * num87;
										}
										if (flag14 && dazskinV2VertexWeights4.zrightbulge > 0f)
										{
											float num88 = num11 * dazskinV2VertexWeights4.zrightbulge;
											vector14.x += vector14.x * num88;
											vector14.y += vector14.y * num88;
										}
										if (flag13 && dazskinV2VertexWeights4.zleftbulge > 0f)
										{
											float num89 = num10 * dazskinV2VertexWeights4.zleftbulge;
											vector14.x += vector14.x * num89;
											vector14.y += vector14.y * num89;
										}
										if (flag15 && dazskinV2VertexWeights4.zrightbulge > 0f)
										{
											float num90 = num12 * dazskinV2VertexWeights4.zrightbulge;
											vector14.x += vector14.x * num90;
											vector14.y += vector14.y * num90;
										}
									}
									if (dazskinV2VertexWeights4.yweight > 0f)
									{
										float num91 = vector2.y * dazskinV2VertexWeights4.yweight;
										float num92 = (float)Math.Sin((double)num91);
										float num93 = (float)Math.Cos((double)num91);
										float x8 = vector14.x * num93 + vector14.z * num92;
										vector14.z = vector14.z * num93 - vector14.x * num92;
										vector14.x = x8;
									}
									if (flag6)
									{
										if (flag7 && dazskinV2VertexWeights4.yleftbulge > 0f)
										{
											float num94 = num5 * dazskinV2VertexWeights4.yleftbulge;
											vector14.x += vector14.x * num94;
											vector14.z += vector14.z * num94;
										}
										if (flag9 && dazskinV2VertexWeights4.yrightbulge > 0f)
										{
											float num95 = num7 * dazskinV2VertexWeights4.yrightbulge;
											vector14.x += vector14.x * num95;
											vector14.z += vector14.z * num95;
										}
										if (flag8 && dazskinV2VertexWeights4.yleftbulge > 0f)
										{
											float num96 = num6 * dazskinV2VertexWeights4.yleftbulge;
											vector14.x += vector14.x * num96;
											vector14.z += vector14.z * num96;
										}
										if (flag10 && dazskinV2VertexWeights4.yrightbulge > 0f)
										{
											float num97 = num8 * dazskinV2VertexWeights4.yrightbulge;
											vector14.x += vector14.x * num97;
											vector14.z += vector14.z * num97;
										}
									}
									this.workingVerts[dazskinV2VertexWeights4.vertex].x = vector14.x * m25 + vector14.y * m26 + vector14.z * m27 + m28;
									this.workingVerts[dazskinV2VertexWeights4.vertex].y = vector14.x * m29 + vector14.y * m30 + vector14.z * m31 + m32;
									this.workingVerts[dazskinV2VertexWeights4.vertex].z = vector14.x * m33 + vector14.y * m34 + vector14.z * m35 + m36;
								}
							}
						}
					}
					else if (rotationOrder == Quaternion2Angles.RotationOrder.ZXY)
					{
						foreach (DAZSkinV2VertexWeights dazskinV2VertexWeights5 in weights)
						{
							if (dazskinV2VertexWeights5.vertex >= startIndex && dazskinV2VertexWeights5.vertex <= stopIndex && (onlyVerts == null || onlyVerts[dazskinV2VertexWeights5.vertex]))
							{
								if (dazskinV2VertexWeights5.xweight > 0.99999f && dazskinV2VertexWeights5.yweight > 0.99999f && dazskinV2VertexWeights5.zweight > 0.99999f)
								{
									Vector3 vector15 = this.workingVerts[dazskinV2VertexWeights5.vertex];
									this.workingVerts[dazskinV2VertexWeights5.vertex].x = vector15.x * m + vector15.y * m2 + vector15.z * m3 + m4;
									this.workingVerts[dazskinV2VertexWeights5.vertex].y = vector15.x * m5 + vector15.y * m6 + vector15.z * m7 + m8;
									this.workingVerts[dazskinV2VertexWeights5.vertex].z = vector15.x * m9 + vector15.y * m10 + vector15.z * m11 + m12;
								}
								else
								{
									Vector3 vector16 = this.workingVerts[dazskinV2VertexWeights5.vertex];
									Vector3 vector17;
									vector17.x = vector16.x * m13 + vector16.y * m14 + vector16.z * m15 + m16;
									vector17.y = vector16.x * m17 + vector16.y * m18 + vector16.z * m19 + m20;
									vector17.z = vector16.x * m21 + vector16.y * m22 + vector16.z * m23 + m24;
									if (dazskinV2VertexWeights5.yweight > 0f)
									{
										float num99 = vector2.y * dazskinV2VertexWeights5.yweight;
										float num100 = (float)Math.Sin((double)num99);
										float num101 = (float)Math.Cos((double)num99);
										float x9 = vector17.x * num101 + vector17.z * num100;
										vector17.z = vector17.z * num101 - vector17.x * num100;
										vector17.x = x9;
									}
									if (flag6)
									{
										if (flag7 && dazskinV2VertexWeights5.yleftbulge > 0f)
										{
											float num102 = num5 * dazskinV2VertexWeights5.yleftbulge;
											vector17.x += vector17.x * num102;
											vector17.z += vector17.z * num102;
										}
										if (flag9 && dazskinV2VertexWeights5.yrightbulge > 0f)
										{
											float num103 = num7 * dazskinV2VertexWeights5.yrightbulge;
											vector17.x += vector17.x * num103;
											vector17.z += vector17.z * num103;
										}
										if (flag8 && dazskinV2VertexWeights5.yleftbulge > 0f)
										{
											float num104 = num6 * dazskinV2VertexWeights5.yleftbulge;
											vector17.x += vector17.x * num104;
											vector17.z += vector17.z * num104;
										}
										if (flag10 && dazskinV2VertexWeights5.yrightbulge > 0f)
										{
											float num105 = num8 * dazskinV2VertexWeights5.yrightbulge;
											vector17.x += vector17.x * num105;
											vector17.z += vector17.z * num105;
										}
									}
									if (dazskinV2VertexWeights5.xweight > 0f)
									{
										float num106 = vector2.x * dazskinV2VertexWeights5.xweight;
										float num107 = (float)Math.Sin((double)num106);
										float num108 = (float)Math.Cos((double)num106);
										float y5 = vector17.y * num108 - vector17.z * num107;
										vector17.z = vector17.y * num107 + vector17.z * num108;
										vector17.y = y5;
									}
									if (flag)
									{
										if (flag2 && dazskinV2VertexWeights5.xleftbulge > 0f)
										{
											float num109 = num * dazskinV2VertexWeights5.xleftbulge;
											vector17.y += vector17.y * num109;
											vector17.z += vector17.z * num109;
										}
										if (flag4 && dazskinV2VertexWeights5.xrightbulge > 0f)
										{
											float num110 = num3 * dazskinV2VertexWeights5.xrightbulge;
											vector17.y += vector17.y * num110;
											vector17.z += vector17.z * num110;
										}
										if (flag3 && dazskinV2VertexWeights5.xleftbulge > 0f)
										{
											float num111 = num2 * dazskinV2VertexWeights5.xleftbulge;
											vector17.y += vector17.y * num111;
											vector17.z += vector17.z * num111;
										}
										if (flag5 && dazskinV2VertexWeights5.xrightbulge > 0f)
										{
											float num112 = num4 * dazskinV2VertexWeights5.xrightbulge;
											vector17.y += vector17.y * num112;
											vector17.z += vector17.z * num112;
										}
									}
									if (dazskinV2VertexWeights5.zweight > 0f)
									{
										float num113 = vector2.z * dazskinV2VertexWeights5.zweight;
										float num114 = (float)Math.Sin((double)num113);
										float num115 = (float)Math.Cos((double)num113);
										float x10 = vector17.x * num115 - vector17.y * num114;
										vector17.y = vector17.x * num114 + vector17.y * num115;
										vector17.x = x10;
									}
									if (flag11)
									{
										if (flag12 && dazskinV2VertexWeights5.zleftbulge > 0f)
										{
											float num116 = num9 * dazskinV2VertexWeights5.zleftbulge;
											vector17.x += vector17.x * num116;
											vector17.y += vector17.y * num116;
										}
										if (flag14 && dazskinV2VertexWeights5.zrightbulge > 0f)
										{
											float num117 = num11 * dazskinV2VertexWeights5.zrightbulge;
											vector17.x += vector17.x * num117;
											vector17.y += vector17.y * num117;
										}
										if (flag13 && dazskinV2VertexWeights5.zleftbulge > 0f)
										{
											float num118 = num10 * dazskinV2VertexWeights5.zleftbulge;
											vector17.x += vector17.x * num118;
											vector17.y += vector17.y * num118;
										}
										if (flag15 && dazskinV2VertexWeights5.zrightbulge > 0f)
										{
											float num119 = num12 * dazskinV2VertexWeights5.zrightbulge;
											vector17.x += vector17.x * num119;
											vector17.y += vector17.y * num119;
										}
									}
									this.workingVerts[dazskinV2VertexWeights5.vertex].x = vector17.x * m25 + vector17.y * m26 + vector17.z * m27 + m28;
									this.workingVerts[dazskinV2VertexWeights5.vertex].y = vector17.x * m29 + vector17.y * m30 + vector17.z * m31 + m32;
									this.workingVerts[dazskinV2VertexWeights5.vertex].z = vector17.x * m33 + vector17.y * m34 + vector17.z * m35 + m36;
								}
							}
						}
					}
					else if (rotationOrder == Quaternion2Angles.RotationOrder.ZYX)
					{
						foreach (DAZSkinV2VertexWeights dazskinV2VertexWeights6 in weights)
						{
							if (dazskinV2VertexWeights6.vertex >= startIndex && dazskinV2VertexWeights6.vertex <= stopIndex && (onlyVerts == null || onlyVerts[dazskinV2VertexWeights6.vertex]))
							{
								if (dazskinV2VertexWeights6.xweight > 0.99999f && dazskinV2VertexWeights6.yweight > 0.99999f && dazskinV2VertexWeights6.zweight > 0.99999f)
								{
									Vector3 vector18 = this.workingVerts[dazskinV2VertexWeights6.vertex];
									this.workingVerts[dazskinV2VertexWeights6.vertex].x = vector18.x * m + vector18.y * m2 + vector18.z * m3 + m4;
									this.workingVerts[dazskinV2VertexWeights6.vertex].y = vector18.x * m5 + vector18.y * m6 + vector18.z * m7 + m8;
									this.workingVerts[dazskinV2VertexWeights6.vertex].z = vector18.x * m9 + vector18.y * m10 + vector18.z * m11 + m12;
								}
								else
								{
									Vector3 vector19 = this.workingVerts[dazskinV2VertexWeights6.vertex];
									Vector3 vector20;
									vector20.x = vector19.x * m13 + vector19.y * m14 + vector19.z * m15 + m16;
									vector20.y = vector19.x * m17 + vector19.y * m18 + vector19.z * m19 + m20;
									vector20.z = vector19.x * m21 + vector19.y * m22 + vector19.z * m23 + m24;
									if (dazskinV2VertexWeights6.xweight > 0f)
									{
										float num121 = vector2.x * dazskinV2VertexWeights6.xweight;
										float num122 = (float)Math.Sin((double)num121);
										float num123 = (float)Math.Cos((double)num121);
										float y6 = vector20.y * num123 - vector20.z * num122;
										vector20.z = vector20.y * num122 + vector20.z * num123;
										vector20.y = y6;
									}
									if (flag)
									{
										if (flag2 && dazskinV2VertexWeights6.xleftbulge > 0f)
										{
											float num124 = num * dazskinV2VertexWeights6.xleftbulge;
											vector20.y += vector20.y * num124;
											vector20.z += vector20.z * num124;
										}
										if (flag4 && dazskinV2VertexWeights6.xrightbulge > 0f)
										{
											float num125 = num3 * dazskinV2VertexWeights6.xrightbulge;
											vector20.y += vector20.y * num125;
											vector20.z += vector20.z * num125;
										}
										if (flag3 && dazskinV2VertexWeights6.xleftbulge > 0f)
										{
											float num126 = num2 * dazskinV2VertexWeights6.xleftbulge;
											vector20.y += vector20.y * num126;
											vector20.z += vector20.z * num126;
										}
										if (flag5 && dazskinV2VertexWeights6.xrightbulge > 0f)
										{
											float num127 = num4 * dazskinV2VertexWeights6.xrightbulge;
											vector20.y += vector20.y * num127;
											vector20.z += vector20.z * num127;
										}
									}
									if (dazskinV2VertexWeights6.yweight > 0f)
									{
										float num128 = vector2.y * dazskinV2VertexWeights6.yweight;
										float num129 = (float)Math.Sin((double)num128);
										float num130 = (float)Math.Cos((double)num128);
										float x11 = vector20.x * num130 + vector20.z * num129;
										vector20.z = vector20.z * num130 - vector20.x * num129;
										vector20.x = x11;
									}
									if (flag6)
									{
										if (flag7 && dazskinV2VertexWeights6.yleftbulge > 0f)
										{
											float num131 = num5 * dazskinV2VertexWeights6.yleftbulge;
											vector20.x += vector20.x * num131;
											vector20.z += vector20.z * num131;
										}
										if (flag9 && dazskinV2VertexWeights6.yrightbulge > 0f)
										{
											float num132 = num7 * dazskinV2VertexWeights6.yrightbulge;
											vector20.x += vector20.x * num132;
											vector20.z += vector20.z * num132;
										}
										if (flag8 && dazskinV2VertexWeights6.yleftbulge > 0f)
										{
											float num133 = num6 * dazskinV2VertexWeights6.yleftbulge;
											vector20.x += vector20.x * num133;
											vector20.z += vector20.z * num133;
										}
										if (flag10 && dazskinV2VertexWeights6.yrightbulge > 0f)
										{
											float num134 = num8 * dazskinV2VertexWeights6.yrightbulge;
											vector20.x += vector20.x * num134;
											vector20.z += vector20.z * num134;
										}
									}
									if (dazskinV2VertexWeights6.zweight > 0f)
									{
										float num135 = vector2.z * dazskinV2VertexWeights6.zweight;
										float num136 = (float)Math.Sin((double)num135);
										float num137 = (float)Math.Cos((double)num135);
										float x12 = vector20.x * num137 - vector20.y * num136;
										vector20.y = vector20.x * num136 + vector20.y * num137;
										vector20.x = x12;
									}
									if (flag11)
									{
										if (flag12 && dazskinV2VertexWeights6.zleftbulge > 0f)
										{
											float num138 = num9 * dazskinV2VertexWeights6.zleftbulge;
											vector20.x += vector20.x * num138;
											vector20.y += vector20.y * num138;
										}
										if (flag14 && dazskinV2VertexWeights6.zrightbulge > 0f)
										{
											float num139 = num11 * dazskinV2VertexWeights6.zrightbulge;
											vector20.x += vector20.x * num139;
											vector20.y += vector20.y * num139;
										}
										if (flag13 && dazskinV2VertexWeights6.zleftbulge > 0f)
										{
											float num140 = num10 * dazskinV2VertexWeights6.zleftbulge;
											vector20.x += vector20.x * num140;
											vector20.y += vector20.y * num140;
										}
										if (flag15 && dazskinV2VertexWeights6.zrightbulge > 0f)
										{
											float num141 = num12 * dazskinV2VertexWeights6.zrightbulge;
											vector20.x += vector20.x * num141;
											vector20.y += vector20.y * num141;
										}
									}
									this.workingVerts[dazskinV2VertexWeights6.vertex].x = vector20.x * m25 + vector20.y * m26 + vector20.z * m27 + m28;
									this.workingVerts[dazskinV2VertexWeights6.vertex].y = vector20.x * m29 + vector20.y * m30 + vector20.z * m31 + m32;
									this.workingVerts[dazskinV2VertexWeights6.vertex].z = vector20.x * m33 + vector20.y * m34 + vector20.z * m35 + m36;
								}
							}
						}
					}
					foreach (int num143 in fullyWeightedVertices)
					{
						if (num143 >= startIndex && num143 <= stopIndex && (onlyVerts == null || onlyVerts[num143]))
						{
							Vector3 vector21 = this.workingVerts[num143];
							this.workingVerts[num143].x = vector21.x * m + vector21.y * m2 + vector21.z * m3 + m4;
							this.workingVerts[num143].y = vector21.x * m5 + vector21.y * m6 + vector21.z * m7 + m8;
							this.workingVerts[num143].z = vector21.x * m9 + vector21.y * m10 + vector21.z * m11 + m12;
						}
					}
				}
			}
		}
	}

	// Token: 0x06004D13 RID: 19731 RVA: 0x0019AC50 File Offset: 0x00199050
	protected void DrawMesh()
	{
		if (!this._renderSuspend)
		{
			if (!this.alreadyCheckedForMeshComponents)
			{
				this.meshFilter = base.GetComponent<MeshFilter>();
				this.meshRenderer = base.GetComponent<MeshRenderer>();
				this.alreadyCheckedForMeshComponents = Application.isPlaying;
			}
			if (this.meshFilter != null && this.meshRenderer != null)
			{
				this.DrawMeshNative();
			}
			else if (this.mesh != null)
			{
				Matrix4x4 identity = Matrix4x4.identity;
				identity.m03 += this.drawOffset.x;
				identity.m13 += this.drawOffset.y;
				identity.m23 += this.drawOffset.z;
				for (int i = 0; i < this.mesh.subMeshCount; i++)
				{
					if (this.dazMesh.materialsEnabled[i])
					{
						Material material = null;
						if (this.dazMesh.materials != null)
						{
							material = this.dazMesh.materials[i];
						}
						if (material == null || this.dazMesh.useSimpleMaterial)
						{
							material = this.dazMesh.simpleMaterial;
						}
						if (material != null)
						{
							Graphics.DrawMesh(this.mesh, identity, material, base.gameObject.layer, null, i, null, this.materialsShadowCastEnabled[i], this.dazMesh.receiveShadows);
						}
					}
				}
			}
		}
	}

	// Token: 0x06004D14 RID: 19732 RVA: 0x0019ADD8 File Offset: 0x001991D8
	protected void DrawMeshNative()
	{
		if (!this.alreadyCheckedForMeshComponents)
		{
			this.meshFilter = base.GetComponent<MeshFilter>();
			this.meshRenderer = base.GetComponent<MeshRenderer>();
			this.alreadyCheckedForMeshComponents = Application.isPlaying;
		}
		if (this.meshFilter != null && this.meshRenderer != null)
		{
			this.meshRenderer.enabled = true;
			if (this.meshFilter.sharedMesh != this.mesh)
			{
				this.meshFilter.sharedMesh = this.mesh;
			}
			for (int i = 0; i < this.mesh.subMeshCount; i++)
			{
				if (this.dazMesh.materialsEnabled[i])
				{
					if (this.dazMesh.useSimpleMaterial)
					{
						this.meshRenderer.materials[i] = this.dazMesh.simpleMaterial;
					}
					else
					{
						this.meshRenderer.materials[i] = this.dazMesh.materials[i];
					}
				}
				else
				{
					this.meshRenderer.materials[i] = null;
				}
			}
		}
	}

	// Token: 0x06004D15 RID: 19733 RVA: 0x0019AEF8 File Offset: 0x001992F8
	protected void SkinMeshGPUDelay()
	{
		if (this.delayDisplayOneFrame && this.GPUSkinner != null && this.delayedVertsBuffer != null)
		{
			if (this._useSmoothing)
			{
				this.GPUSkinner.SetBuffer(this._copyKernel, "inVerts", this.smoothedVertsBuffer);
			}
			else
			{
				this.GPUSkinner.SetBuffer(this._copyKernel, "inVerts", this.rawVertsBuffer);
			}
			this.GPUSkinner.SetBuffer(this._copyKernel, "outVerts", this.delayedVertsBuffer);
			this.GPUSkinner.Dispatch(this._copyKernel, this.numVertThreadGroups, 1, 1);
			if (this._normalsBuffer != null)
			{
				this.GPUSkinner.SetBuffer(this._copyKernel, "inVerts", this._normalsBuffer);
				this.GPUSkinner.SetBuffer(this._copyKernel, "outVerts", this.delayedNormalsBuffer);
				this.GPUSkinner.Dispatch(this._copyKernel, this.numVertThreadGroups, 1, 1);
			}
			if (this._tangentsBuffer != null)
			{
				this.GPUMeshCompute.SetBuffer(this._copyTangentsKernel, "inTangents", this._tangentsBuffer);
				this.GPUMeshCompute.SetBuffer(this._copyTangentsKernel, "outTangents", this.delayedTangentsBuffer);
				this.GPUMeshCompute.Dispatch(this._copyTangentsKernel, this.numVertThreadGroups, 1, 1);
			}
		}
	}

	// Token: 0x06004D16 RID: 19734 RVA: 0x0019B061 File Offset: 0x00199461
	public void FlushBuffers()
	{
		this.materialVertsBuffers = new Dictionary<int, ComputeBuffer>();
		this.materialNormalsBuffers = new Dictionary<int, ComputeBuffer>();
		this.materialTangentsBuffers = new Dictionary<int, ComputeBuffer>();
	}

	// Token: 0x06004D17 RID: 19735 RVA: 0x0019B084 File Offset: 0x00199484
	private void OnApplicationFocus(bool focus)
	{
		this.FlushBuffers();
	}

	// Token: 0x06004D18 RID: 19736 RVA: 0x0019B08C File Offset: 0x0019948C
	public void DrawMeshGPU()
	{
		this.needsDispatch = true;
		if (!this._renderSuspend)
		{
			if (!this.alreadyCheckedForMeshComponents)
			{
				this.meshFilter = base.GetComponent<MeshFilter>();
				this.meshRenderer = base.GetComponent<MeshRenderer>();
				this.alreadyCheckedForMeshComponents = Application.isPlaying;
			}
			if (this.meshRenderer != null)
			{
				this.meshRenderer.enabled = false;
			}
			if (this.materialVertsBuffers == null)
			{
				this.materialVertsBuffers = new Dictionary<int, ComputeBuffer>();
			}
			if (this.materialNormalsBuffers == null)
			{
				this.materialNormalsBuffers = new Dictionary<int, ComputeBuffer>();
			}
			if (this.materialTangentsBuffers == null)
			{
				this.materialTangentsBuffers = new Dictionary<int, ComputeBuffer>();
			}
			if (DAZSkinV2.staticDraw)
			{
				Matrix4x4 identity = Matrix4x4.identity;
				this.dazMesh.DrawMorphedUVMappedMesh(identity);
			}
			else if (this.mesh != null)
			{
				Matrix4x4 identity2 = Matrix4x4.identity;
				identity2.m03 += this.drawOffset.x;
				identity2.m13 += this.drawOffset.y;
				identity2.m23 += this.drawOffset.z;
				for (int i = 0; i < this.mesh.subMeshCount; i++)
				{
					if (this.materialsEnabled[i])
					{
						if (this.GPUuseSimpleMaterial && this.GPUsimpleMaterial)
						{
							if (this.delayDisplayOneFrame)
							{
								this.GPUsimpleMaterial.SetBuffer("verts", this.delayedVertsBuffer);
								if (this._normalsBuffer != null)
								{
									this.GPUsimpleMaterial.SetBuffer("normals", this.delayedNormalsBuffer);
								}
								if (this._tangentsBuffer != null)
								{
									this.GPUsimpleMaterial.SetBuffer("tangents", this.delayedTangentsBuffer);
								}
							}
							else
							{
								if (this._useSmoothing)
								{
									this.GPUsimpleMaterial.SetBuffer("verts", this.smoothedVertsBuffer);
								}
								else
								{
									this.GPUsimpleMaterial.SetBuffer("verts", this.rawVertsBuffer);
								}
								if (this._normalsBuffer != null)
								{
									this.GPUsimpleMaterial.SetBuffer("normals", this._normalsBuffer);
								}
								if (this._tangentsBuffer != null)
								{
									this.GPUsimpleMaterial.SetBuffer("tangents", this._tangentsBuffer);
								}
							}
							Graphics.DrawMesh(this.mesh, identity2, this.GPUsimpleMaterial, base.gameObject.layer, null, i, null, this.materialsShadowCastEnabled[i], this.dazMesh.receiveShadows);
						}
						else if (this.GPUmaterials[i] != null)
						{
							if (this.delayDisplayOneFrame)
							{
								this.GPUmaterials[i].SetBuffer("verts", this.delayedVertsBuffer);
								if (this._normalsBuffer != null)
								{
									this.GPUmaterials[i].SetBuffer("normals", this.delayedNormalsBuffer);
								}
								if (this._tangentsBuffer != null)
								{
									this.GPUmaterials[i].SetBuffer("tangents", this.delayedTangentsBuffer);
								}
							}
							else
							{
								ComputeBuffer computeBuffer;
								this.materialVertsBuffers.TryGetValue(i, out computeBuffer);
								if (this._useSmoothing)
								{
									if (computeBuffer != this.smoothedVertsBuffer)
									{
										this.GPUmaterials[i].SetBuffer("verts", this.smoothedVertsBuffer);
										this.materialVertsBuffers.Remove(i);
										this.materialVertsBuffers.Add(i, this.smoothedVertsBuffer);
									}
								}
								else if (computeBuffer != this.rawVertsBuffer)
								{
									this.GPUmaterials[i].SetBuffer("verts", this.rawVertsBuffer);
									this.materialVertsBuffers.Remove(i);
									this.materialVertsBuffers.Add(i, this.rawVertsBuffer);
								}
								if (this._normalsBuffer != null)
								{
									this.materialNormalsBuffers.TryGetValue(i, out computeBuffer);
									if (computeBuffer != this._normalsBuffer)
									{
										this.GPUmaterials[i].SetBuffer("normals", this._normalsBuffer);
										this.materialNormalsBuffers.Remove(i);
										this.materialNormalsBuffers.Add(i, this._normalsBuffer);
									}
								}
								if (this._tangentsBuffer != null)
								{
									this.materialTangentsBuffers.TryGetValue(i, out computeBuffer);
									if (computeBuffer != this._tangentsBuffer)
									{
										this.GPUmaterials[i].SetBuffer("tangents", this._tangentsBuffer);
										this.materialTangentsBuffers.Remove(i);
										this.materialTangentsBuffers.Add(i, this._tangentsBuffer);
									}
								}
							}
							Graphics.DrawMesh(this.mesh, identity2, this.GPUmaterials[i], base.gameObject.layer, null, i, null, this.materialsShadowCastEnabled[i], this.dazMesh.receiveShadows);
						}
					}
				}
			}
		}
	}

	// Token: 0x06004D19 RID: 19737 RVA: 0x0019B534 File Offset: 0x00199934
	private void InitCollider(bool convex = false)
	{
		if (this.root != null)
		{
			this.meshCollider = this.root.gameObject.GetComponent<MeshCollider>();
			if (this.meshCollider == null)
			{
				this.meshCollider = this.root.gameObject.AddComponent<MeshCollider>();
			}
			if (this.dazMesh != null)
			{
				this.meshCollider.sharedMesh = this.dazMesh.morphedUVMappedMesh;
			}
			this.meshCollider.convex = convex;
		}
	}

	// Token: 0x06004D1A RID: 19738 RVA: 0x0019B5C4 File Offset: 0x001999C4
	private void InitRigidbody(bool kinematic = false)
	{
		if (this.root != null)
		{
			this.RB = this.root.gameObject.GetComponent<Rigidbody>();
			if (this.RB == null)
			{
				this.RB = this.root.gameObject.AddComponent<Rigidbody>();
			}
			this.RB.mass = this.mass;
			this.RB.constraints = RigidbodyConstraints.None;
			this.RB.isKinematic = kinematic;
		}
	}

	// Token: 0x06004D1B RID: 19739 RVA: 0x0019B648 File Offset: 0x00199A48
	public void InitPhysicsObjects()
	{
		switch (this._physicsType)
		{
		case DAZSkinV2.PhysicsType.None:
		{
			Joint component = this.root.gameObject.GetComponent<Joint>();
			if (component != null)
			{
				UnityEngine.Object.DestroyImmediate(component);
			}
			this.RB = this.root.gameObject.GetComponent<Rigidbody>();
			if (this.RB != null)
			{
				UnityEngine.Object.DestroyImmediate(this.RB);
			}
			this.meshCollider = this.root.gameObject.GetComponent<MeshCollider>();
			if (this.meshCollider != null)
			{
				UnityEngine.Object.DestroyImmediate(this.meshCollider);
			}
			break;
		}
		case DAZSkinV2.PhysicsType.Static:
		{
			this.InitCollider(false);
			Joint component = this.root.gameObject.GetComponent<Joint>();
			if (component != null)
			{
				UnityEngine.Object.DestroyImmediate(component);
			}
			this.RB = this.root.gameObject.GetComponent<Rigidbody>();
			if (this.RB != null)
			{
				UnityEngine.Object.DestroyImmediate(this.RB);
			}
			break;
		}
		case DAZSkinV2.PhysicsType.Rigidbody:
			this.InitCollider(true);
			this.InitRigidbody(false);
			break;
		case DAZSkinV2.PhysicsType.KinematicRigidbody:
			this.InitCollider(false);
			this.InitRigidbody(true);
			break;
		}
	}

	// Token: 0x06004D1C RID: 19740 RVA: 0x0019B793 File Offset: 0x00199B93
	private void Awake()
	{
		if (Application.isPlaying)
		{
			this.SkinMeshGPUMaterialInit();
		}
	}

	// Token: 0x17000AF7 RID: 2807
	// (get) Token: 0x06004D1D RID: 19741 RVA: 0x0019B7A5 File Offset: 0x00199BA5
	public bool wasInit
	{
		get
		{
			return this._wasInit;
		}
	}

	// Token: 0x06004D1E RID: 19742 RVA: 0x0019B7B0 File Offset: 0x00199BB0
	public void Init()
	{
		if (!this._wasInit && Application.isPlaying && this.root != null)
		{
			this._wasInit = true;
			this.stopwatch = new Stopwatch();
			this.f = 1000f / (float)Stopwatch.Frequency;
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
			this.InitPhysicsObjects();
			if (this.skinMethod == DAZSkinV2.SkinMethod.CPU)
			{
				this.SkinMesh(true);
			}
			else
			{
				this.SkinMeshCPUandGPU(true);
			}
		}
	}

	// Token: 0x06004D1F RID: 19743 RVA: 0x0019B8A0 File Offset: 0x00199CA0
	public void ResetPostSkinMorphs()
	{
		if (Application.isPlaying)
		{
			this.postSkinNeededVertsList = new int[0];
			this.postSkinNeededTriangles = new int[0];
			for (int i = 0; i < this.postSkinMorphs.Length; i++)
			{
				this.postSkinVerts[i] = false;
				this.postSkinVertsReady[i] = false;
				this.postSkinNeededVerts[i] = false;
				this.postSkinNormalVerts[i] = false;
				this.postSkinMorphs[i].x = 0f;
				this.postSkinMorphs[i].y = 0f;
				this.postSkinMorphs[i].z = 0f;
			}
		}
	}

	// Token: 0x06004D20 RID: 19744 RVA: 0x0019B94E File Offset: 0x00199D4E
	private void Start()
	{
		this.Init();
	}

	// Token: 0x06004D21 RID: 19745 RVA: 0x0019B956 File Offset: 0x00199D56
	private void OnEnable()
	{
		this.FlushBuffers();
	}

	// Token: 0x06004D22 RID: 19746 RVA: 0x0019B95E File Offset: 0x00199D5E
	private void OnDestroy()
	{
		this.DestroyAllocatedObjects();
		this.SkinMeshGPUCleanup();
		if (this.stopwatch != null)
		{
			this.stopwatch.Stop();
		}
		if (Application.isPlaying)
		{
			this.StopSubThreads();
			this.StopThreads();
		}
	}

	// Token: 0x06004D23 RID: 19747 RVA: 0x0019B998 File Offset: 0x00199D98
	protected void OnApplicationQuit()
	{
		if (Application.isPlaying)
		{
			this.StopSubThreads();
			this.StopThreads();
		}
	}

	// Token: 0x06004D24 RID: 19748 RVA: 0x0019B9B0 File Offset: 0x00199DB0
	private void Update()
	{
		this.Init();
		if (this.skin && Application.isPlaying && this._wasInit && this.useEarlyFinish && this.skinMethod == DAZSkinV2.SkinMethod.CPUAndGPU)
		{
			this.StartThreads();
			this.SkinMeshCPUandGPUEarlyFinish();
		}
	}

	// Token: 0x06004D25 RID: 19749 RVA: 0x0019BA08 File Offset: 0x00199E08
	private void LateUpdate()
	{
		if (this.skin && Application.isPlaying && this._wasInit)
		{
			this.updateStartTime = (float)this.stopwatch.ElapsedTicks * this.f;
			if (this.skinMethod == DAZSkinV2.SkinMethod.CPU)
			{
				this.SkinMesh(false);
			}
			else
			{
				this.SkinMeshGPUDelay();
				if (this.skinMethod == DAZSkinV2.SkinMethod.CPUAndGPU)
				{
					this.StartThreads();
					this.SkinMeshCPUandGPUEarlyFinish();
					this.SkinMeshCPUandGPU(false);
				}
				else
				{
					this.SkinMeshGPU();
				}
			}
			if (this.draw)
			{
				if (this.skinMethod == DAZSkinV2.SkinMethod.CPU)
				{
					this.DrawMesh();
				}
				else
				{
					this.DrawMeshGPU();
				}
			}
			this.updateStopTime = (float)this.stopwatch.ElapsedTicks * this.f;
			this.updateTime = this.updateStopTime - this.updateStartTime;
		}
		else if (this.dazMesh != null && this.root != null && this.nodes != null && this.draw)
		{
			this.dazMesh.DrawMorphedUVMappedMesh(this.root.transform.localToWorldMatrix);
		}
	}

	// Token: 0x06004D26 RID: 19750 RVA: 0x0019BB40 File Offset: 0x00199F40
	// Note: this type is marked as 'beforefieldinit'.
	static DAZSkinV2()
	{
	}

	// Token: 0x04003BBC RID: 15292
	public static bool staticDraw;

	// Token: 0x04003BBD RID: 15293
	protected List<UnityEngine.Object> allocatedObjects;

	// Token: 0x04003BBE RID: 15294
	private GpuBuffer<Matrix4x4> _ToWorldMatricesBuffer;

	// Token: 0x04003BBF RID: 15295
	private GpuBuffer<Vector3> _PreCalculatedVerticesBuffer;

	// Token: 0x04003BC0 RID: 15296
	private GpuBuffer<Vector3> _NormalsBuffer;

	// Token: 0x04003BC1 RID: 15297
	protected bool needsDispatch;

	// Token: 0x04003BC2 RID: 15298
	protected bool _updateDrawDisabled;

	// Token: 0x04003BC3 RID: 15299
	public bool skin;

	// Token: 0x04003BC4 RID: 15300
	public bool draw = true;

	// Token: 0x04003BC5 RID: 15301
	protected bool _renderSuspend;

	// Token: 0x04003BC6 RID: 15302
	[SerializeField]
	protected DAZSkinV2.PhysicsType _physicsType;

	// Token: 0x04003BC7 RID: 15303
	public float mass = 1000f;

	// Token: 0x04003BC8 RID: 15304
	protected MeshCollider meshCollider;

	// Token: 0x04003BC9 RID: 15305
	protected Rigidbody RB;

	// Token: 0x04003BCA RID: 15306
	public DAZSkinV2.SkinMethod skinMethod = DAZSkinV2.SkinMethod.CPUAndGPU;

	// Token: 0x04003BCB RID: 15307
	public bool delayDisplayOneFrame;

	// Token: 0x04003BCC RID: 15308
	[SerializeField]
	protected bool _hasGeneralWeights;

	// Token: 0x04003BCD RID: 15309
	[SerializeField]
	protected bool _useGeneralWeights;

	// Token: 0x04003BCE RID: 15310
	public string skinUrl;

	// Token: 0x04003BCF RID: 15311
	public string skinId;

	// Token: 0x04003BD0 RID: 15312
	public string sceneGeometryId;

	// Token: 0x04003BD1 RID: 15313
	public DAZBones root;

	// Token: 0x04003BD2 RID: 15314
	public Vector3 drawOffset;

	// Token: 0x04003BD3 RID: 15315
	[SerializeField]
	private bool _useSmoothing;

	// Token: 0x04003BD4 RID: 15316
	[SerializeField]
	private bool _recalculateTangents = true;

	// Token: 0x04003BD5 RID: 15317
	[SerializeField]
	private bool _recalculateNormals = true;

	// Token: 0x04003BD6 RID: 15318
	[SerializeField]
	protected int _numBones;

	// Token: 0x04003BD7 RID: 15319
	public DAZSkinV2Node[] nodes;

	// Token: 0x04003BD8 RID: 15320
	protected List<DAZSkinV2Node> importNodes;

	// Token: 0x04003BD9 RID: 15321
	protected Dictionary<string, int> boneNameToIndexMap;

	// Token: 0x04003BDA RID: 15322
	protected Dictionary<string, Dictionary<int, DAZSkinV2VertexWeights>> boneWeightsMap;

	// Token: 0x04003BDB RID: 15323
	protected Dictionary<string, Dictionary<int, DAZSkinV2GeneralVertexWeights>> boneGeneralWeightsMap;

	// Token: 0x04003BDC RID: 15324
	protected Dictionary<int, bool> vertexDoneAccumulating;

	// Token: 0x04003BDD RID: 15325
	protected bool accumlationStarted;

	// Token: 0x04003BDE RID: 15326
	protected DAZBone[] dazBones;

	// Token: 0x04003BDF RID: 15327
	protected Vector3[] boneRotationAngles;

	// Token: 0x04003BE0 RID: 15328
	protected Matrix4x4[] boneChangeFromOriginalMatrices;

	// Token: 0x04003BE1 RID: 15329
	protected Matrix4x4[] boneWorldToLocalMatrices;

	// Token: 0x04003BE2 RID: 15330
	protected Matrix4x4[] boneLocalToWorldMatrices;

	// Token: 0x04003BE3 RID: 15331
	[NonSerialized]
	public DAZBone[] strongestDAZBone;

	// Token: 0x04003BE4 RID: 15332
	protected float[] strongestBoneWeight;

	// Token: 0x04003BE5 RID: 15333
	protected Mesh mesh;

	// Token: 0x04003BE6 RID: 15334
	public DAZMesh dazMesh;

	// Token: 0x04003BE7 RID: 15335
	public bool showNodes;

	// Token: 0x04003BE8 RID: 15336
	public bool showMaterials = true;

	// Token: 0x04003BE9 RID: 15337
	public bool GPUuseSimpleMaterial;

	// Token: 0x04003BEA RID: 15338
	public Material GPUsimpleMaterial;

	// Token: 0x04003BEB RID: 15339
	public Material[] GPUmaterials;

	// Token: 0x04003BEC RID: 15340
	public bool GPUAutoSwapShader = true;

	// Token: 0x04003BED RID: 15341
	public int GPUAutoSwapCopyNum;

	// Token: 0x04003BEE RID: 15342
	public bool[] materialsEnabled;

	// Token: 0x04003BEF RID: 15343
	public bool[] materialsShadowCastEnabled;

	// Token: 0x04003BF0 RID: 15344
	[SerializeField]
	protected int _numMaterials;

	// Token: 0x04003BF1 RID: 15345
	[SerializeField]
	protected string[] _materialNames;

	// Token: 0x04003BF2 RID: 15346
	protected Vector3[] startVerts;

	// Token: 0x04003BF3 RID: 15347
	protected Vector3[] startVertsCopy;

	// Token: 0x04003BF4 RID: 15348
	protected Vector3[] startNormals;

	// Token: 0x04003BF5 RID: 15349
	protected Vector4[] startTangents;

	// Token: 0x04003BF6 RID: 15350
	[NonSerialized]
	public Vector3[] drawVerts;

	// Token: 0x04003BF7 RID: 15351
	public bool allowPostSkinMorph = true;

	// Token: 0x04003BF8 RID: 15352
	protected Vector3[] workingVerts;

	// Token: 0x04003BF9 RID: 15353
	protected Vector3[] workingVerts1;

	// Token: 0x04003BFA RID: 15354
	protected Vector3[] workingVerts2;

	// Token: 0x04003BFB RID: 15355
	[NonSerialized]
	public Vector3[] rawSkinnedWorkingVerts;

	// Token: 0x04003BFC RID: 15356
	[NonSerialized]
	public Vector3[] rawSkinnedVerts;

	// Token: 0x04003BFD RID: 15357
	protected Vector3[] smoothedVerts;

	// Token: 0x04003BFE RID: 15358
	protected Vector3[] unsmoothedVerts;

	// Token: 0x04003BFF RID: 15359
	protected bool[] isBaseVert;

	// Token: 0x04003C00 RID: 15360
	protected int[] baseVertIndices;

	// Token: 0x04003C01 RID: 15361
	protected int numBaseVerts;

	// Token: 0x04003C02 RID: 15362
	protected int numUVVerts;

	// Token: 0x04003C03 RID: 15363
	protected int numUVOnlyVerts;

	// Token: 0x04003C04 RID: 15364
	protected DAZVertexMap[] baseVertsToUVVertsCopy;

	// Token: 0x04003C05 RID: 15365
	protected Vector3[] workingNormals;

	// Token: 0x04003C06 RID: 15366
	protected Vector3[] workingSurfaceNormals;

	// Token: 0x04003C07 RID: 15367
	[NonSerialized]
	public Vector3[] drawNormals;

	// Token: 0x04003C08 RID: 15368
	[NonSerialized]
	public Vector3[] drawSurfaceNormals;

	// Token: 0x04003C09 RID: 15369
	protected Vector4[] workingTangents;

	// Token: 0x04003C0A RID: 15370
	[NonSerialized]
	public Vector4[] drawTangents;

	// Token: 0x04003C0B RID: 15371
	protected MeshSmooth meshSmooth;

	// Token: 0x04003C0C RID: 15372
	protected MeshSmoothGPU meshSmoothGPU;

	// Token: 0x04003C0D RID: 15373
	protected RecalculateNormals recalcNormals;

	// Token: 0x04003C0E RID: 15374
	protected RecalculateNormals postSkinRecalcNormals;

	// Token: 0x04003C0F RID: 15375
	protected RecalculateNormalsGPU originalRecalcNormalsGPU;

	// Token: 0x04003C10 RID: 15376
	protected RecalculateNormalsGPU recalcNormalsGPU;

	// Token: 0x04003C11 RID: 15377
	protected RecalculateNormalsGPU recalcNormalsGPURaw;

	// Token: 0x04003C12 RID: 15378
	protected RecalculateTangents recalcTangents;

	// Token: 0x04003C13 RID: 15379
	protected RecalculateTangentsGPU originalRecalcTangentsGPU;

	// Token: 0x04003C14 RID: 15380
	protected RecalculateTangentsGPU recalcTangentsGPU;

	// Token: 0x04003C15 RID: 15381
	protected bool meshWasInit;

	// Token: 0x04003C16 RID: 15382
	public bool useThreadControlNumThreads = true;

	// Token: 0x04003C17 RID: 15383
	[SerializeField]
	protected int _numSubThreads = 2;

	// Token: 0x04003C18 RID: 15384
	public bool useMultithreadedSkinning = true;

	// Token: 0x04003C19 RID: 15385
	public bool useMultithreadedSmoothing;

	// Token: 0x04003C1A RID: 15386
	public bool useAsynchronousThreadedSkinning = true;

	// Token: 0x04003C1B RID: 15387
	public bool useAsynchronousNormalTangentRecalc = true;

	// Token: 0x04003C1C RID: 15388
	public bool useSmoothVertsForNormalTangentRecalc;

	// Token: 0x04003C1D RID: 15389
	protected float f;

	// Token: 0x04003C1E RID: 15390
	public float debugStartTime;

	// Token: 0x04003C1F RID: 15391
	public float debugTime;

	// Token: 0x04003C20 RID: 15392
	public float debugStopTime;

	// Token: 0x04003C21 RID: 15393
	public float updateStartTime;

	// Token: 0x04003C22 RID: 15394
	public float updateStopTime;

	// Token: 0x04003C23 RID: 15395
	public float updateTime;

	// Token: 0x04003C24 RID: 15396
	public float lastFrameSkinStartTime;

	// Token: 0x04003C25 RID: 15397
	public float skinStartTime;

	// Token: 0x04003C26 RID: 15398
	public float skinStopTime;

	// Token: 0x04003C27 RID: 15399
	public float skinTime;

	// Token: 0x04003C28 RID: 15400
	public float mainThreadSkinStartTime;

	// Token: 0x04003C29 RID: 15401
	public float mainThreadSkinStopTime;

	// Token: 0x04003C2A RID: 15402
	public float mainThreadSkinTime;

	// Token: 0x04003C2B RID: 15403
	public float[] threadSkinStartTime;

	// Token: 0x04003C2C RID: 15404
	public float[] threadSkinStopTime;

	// Token: 0x04003C2D RID: 15405
	public float[] threadSkinTime;

	// Token: 0x04003C2E RID: 15406
	public int[] threadSkinVertsCount;

	// Token: 0x04003C2F RID: 15407
	public float mainThreadSmoothStartTime;

	// Token: 0x04003C30 RID: 15408
	public float mainThreadSmoothStopTime;

	// Token: 0x04003C31 RID: 15409
	public float mainThreadSmoothTime;

	// Token: 0x04003C32 RID: 15410
	public float[] threadSmoothStartTime;

	// Token: 0x04003C33 RID: 15411
	public float[] threadSmoothStopTime;

	// Token: 0x04003C34 RID: 15412
	public float[] threadSmoothTime;

	// Token: 0x04003C35 RID: 15413
	public float mainThreadSmoothCorrectionStartTime;

	// Token: 0x04003C36 RID: 15414
	public float mainThreadSmoothCorrectionStopTime;

	// Token: 0x04003C37 RID: 15415
	public float mainThreadSmoothCorrectionTime;

	// Token: 0x04003C38 RID: 15416
	public float[] threadSmoothCorrectionStartTime;

	// Token: 0x04003C39 RID: 15417
	public float[] threadSmoothCorrectionStopTime;

	// Token: 0x04003C3A RID: 15418
	public float[] threadSmoothCorrectionTime;

	// Token: 0x04003C3B RID: 15419
	public float threadRecalcNormalTangentStartTime;

	// Token: 0x04003C3C RID: 15420
	public float threadRecalcNormalTangentTime;

	// Token: 0x04003C3D RID: 15421
	public float threadRecalcNormalTangentStopTime;

	// Token: 0x04003C3E RID: 15422
	public float threadMainSkinStartTime;

	// Token: 0x04003C3F RID: 15423
	public float threadMainSkinTime;

	// Token: 0x04003C40 RID: 15424
	public float threadMainSkinStopTime;

	// Token: 0x04003C41 RID: 15425
	public float bulgeScale = 0.0015f;

	// Token: 0x04003C42 RID: 15426
	public int smoothOuterLoops = 1;

	// Token: 0x04003C43 RID: 15427
	public int laplacianSmoothPasses = 2;

	// Token: 0x04003C44 RID: 15428
	public int springSmoothPasses;

	// Token: 0x04003C45 RID: 15429
	public float laplacianSmoothBeta = 0.5f;

	// Token: 0x04003C46 RID: 15430
	public float springSmoothFactor = 0.2f;

	// Token: 0x04003C47 RID: 15431
	protected DAZSkinTaskInfo[] tasks;

	// Token: 0x04003C48 RID: 15432
	protected DAZSkinTaskInfo normalTangentTask;

	// Token: 0x04003C49 RID: 15433
	protected DAZSkinTaskInfo mainSkinTask;

	// Token: 0x04003C4A RID: 15434
	protected DAZSkinTaskInfo postSkinMorphTask;

	// Token: 0x04003C4B RID: 15435
	protected bool _threadsRunning;

	// Token: 0x04003C4C RID: 15436
	protected bool _subThreadsRunning;

	// Token: 0x04003C4D RID: 15437
	protected const int skinGroupSize = 256;

	// Token: 0x04003C4E RID: 15438
	protected const int vertGroupSize = 256;

	// Token: 0x04003C4F RID: 15439
	protected const int baseVertToUVVertGroupSize = 256;

	// Token: 0x04003C50 RID: 15440
	protected int[] numGeneralSkinThreadGroups;

	// Token: 0x04003C51 RID: 15441
	protected int[] numSkinThreadGroups;

	// Token: 0x04003C52 RID: 15442
	protected int[] numSkinFinishThreadGroups;

	// Token: 0x04003C53 RID: 15443
	protected int numVertThreadGroups;

	// Token: 0x04003C54 RID: 15444
	public ComputeShader GPUSkinner;

	// Token: 0x04003C55 RID: 15445
	public ComputeShader GPUMeshCompute;

	// Token: 0x04003C56 RID: 15446
	protected ComputeBuffer[] boneBuffer;

	// Token: 0x04003C57 RID: 15447
	protected ComputeBuffer[] generalWeightsBuffer;

	// Token: 0x04003C58 RID: 15448
	protected ComputeBuffer[] weightsBuffer;

	// Token: 0x04003C59 RID: 15449
	protected ComputeBuffer[] fullWeightsBuffer;

	// Token: 0x04003C5A RID: 15450
	protected ComputeBuffer startVertsBuffer;

	// Token: 0x04003C5B RID: 15451
	public ComputeBuffer rawVertsBuffer;

	// Token: 0x04003C5C RID: 15452
	protected ComputeBuffer postSkinMorphsBuffer;

	// Token: 0x04003C5D RID: 15453
	public ComputeBuffer _verticesBuffer1;

	// Token: 0x04003C5E RID: 15454
	public ComputeBuffer _verticesBuffer2;

	// Token: 0x04003C5F RID: 15455
	protected ComputeBuffer _originalVerticesBuffer;

	// Token: 0x04003C60 RID: 15456
	protected ComputeBuffer _matricesBuffer;

	// Token: 0x04003C61 RID: 15457
	public ComputeBuffer smoothedVertsBuffer;

	// Token: 0x04003C62 RID: 15458
	protected ComputeBuffer preCalcVertsBuffer;

	// Token: 0x04003C63 RID: 15459
	public ComputeBuffer delayedVertsBuffer;

	// Token: 0x04003C64 RID: 15460
	public ComputeBuffer delayedNormalsBuffer;

	// Token: 0x04003C65 RID: 15461
	public ComputeBuffer delayedTangentsBuffer;

	// Token: 0x04003C66 RID: 15462
	protected ComputeBuffer _originalNormalsBuffer;

	// Token: 0x04003C67 RID: 15463
	protected ComputeBuffer _normalsBuffer;

	// Token: 0x04003C68 RID: 15464
	protected ComputeBuffer _originalTangentsBuffer;

	// Token: 0x04003C69 RID: 15465
	protected ComputeBuffer _tangentsBuffer;

	// Token: 0x04003C6A RID: 15466
	protected ComputeBuffer _originalSurfaceNormalsBuffer;

	// Token: 0x04003C6B RID: 15467
	protected ComputeBuffer _surfaceNormalsBuffer;

	// Token: 0x04003C6C RID: 15468
	protected ComputeBuffer _surfaceNormalsRawBuffer;

	// Token: 0x04003C6D RID: 15469
	protected int _zeroKernel;

	// Token: 0x04003C6E RID: 15470
	protected int _skinGeneralKernel;

	// Token: 0x04003C6F RID: 15471
	protected int _initKernel;

	// Token: 0x04003C70 RID: 15472
	protected int _copyKernel;

	// Token: 0x04003C71 RID: 15473
	protected int _copyTangentsKernel;

	// Token: 0x04003C72 RID: 15474
	protected int _skinXYZKernel;

	// Token: 0x04003C73 RID: 15475
	protected int _skinXZYKernel;

	// Token: 0x04003C74 RID: 15476
	protected int _skinYXZKernel;

	// Token: 0x04003C75 RID: 15477
	protected int _skinYZXKernel;

	// Token: 0x04003C76 RID: 15478
	protected int _skinZXYKernel;

	// Token: 0x04003C77 RID: 15479
	protected int _skinZYXKernel;

	// Token: 0x04003C78 RID: 15480
	protected int _skinFinishKernel;

	// Token: 0x04003C79 RID: 15481
	protected int _postSkinMorphKernel;

	// Token: 0x04003C7A RID: 15482
	protected int _nullVertexIndex;

	// Token: 0x04003C7B RID: 15483
	protected int _calcChangeMatricesKernel;

	// Token: 0x04003C7C RID: 15484
	protected MapVerticesGPU mapVerticesGPU;

	// Token: 0x04003C7D RID: 15485
	public float bloat;

	// Token: 0x04003C7E RID: 15486
	[NonSerialized]
	public bool[] postSkinVerts;

	// Token: 0x04003C7F RID: 15487
	[NonSerialized]
	public bool[] postSkinVertsReady;

	// Token: 0x04003C80 RID: 15488
	[NonSerialized]
	public bool[] postSkinNormalVerts;

	// Token: 0x04003C81 RID: 15489
	protected bool needsPostSkinNormals;

	// Token: 0x04003C82 RID: 15490
	protected bool[] postSkinNeededVerts;

	// Token: 0x04003C83 RID: 15491
	protected int[] postSkinNeededTriangles;

	// Token: 0x04003C84 RID: 15492
	protected int[] postSkinNeededTriangleIndices;

	// Token: 0x04003C85 RID: 15493
	protected int[] postSkinNeededVertsList;

	// Token: 0x04003C86 RID: 15494
	protected bool[] postSkinBones;

	// Token: 0x04003C87 RID: 15495
	[NonSerialized]
	public Vector3[] postSkinMorphs;

	// Token: 0x04003C88 RID: 15496
	protected Vector3[] postSkinMorphsThreaded;

	// Token: 0x04003C89 RID: 15497
	protected Vector3[] postSkinWorkingNormals;

	// Token: 0x04003C8A RID: 15498
	protected Vector3[] postSkinWorkingSurfaceNormals;

	// Token: 0x04003C8B RID: 15499
	[NonSerialized]
	public Vector3[] postSkinNormals;

	// Token: 0x04003C8C RID: 15500
	public bool postSkinVertsChanged = true;

	// Token: 0x04003C8D RID: 15501
	public bool postSkinVertsChangedThreaded;

	// Token: 0x04003C8E RID: 15502
	public bool useEarlyFinish;

	// Token: 0x04003C8F RID: 15503
	protected int totalFrameCount;

	// Token: 0x04003C90 RID: 15504
	protected int missedFrameCount;

	// Token: 0x04003C91 RID: 15505
	public int debugVertex = -1;

	// Token: 0x04003C92 RID: 15506
	protected MeshFilter meshFilter;

	// Token: 0x04003C93 RID: 15507
	protected MeshRenderer meshRenderer;

	// Token: 0x04003C94 RID: 15508
	protected bool alreadyCheckedForMeshComponents;

	// Token: 0x04003C95 RID: 15509
	protected Dictionary<int, ComputeBuffer> materialVertsBuffers;

	// Token: 0x04003C96 RID: 15510
	protected Dictionary<int, ComputeBuffer> materialNormalsBuffers;

	// Token: 0x04003C97 RID: 15511
	protected Dictionary<int, ComputeBuffer> materialTangentsBuffers;

	// Token: 0x04003C98 RID: 15512
	protected Stopwatch stopwatch;

	// Token: 0x04003C99 RID: 15513
	protected bool _wasInit;

	// Token: 0x02000B0A RID: 2826
	public enum PhysicsType
	{
		// Token: 0x04003C9B RID: 15515
		None,
		// Token: 0x04003C9C RID: 15516
		Static,
		// Token: 0x04003C9D RID: 15517
		Rigidbody,
		// Token: 0x04003C9E RID: 15518
		KinematicRigidbody,
		// Token: 0x04003C9F RID: 15519
		Ignore
	}

	// Token: 0x02000B0B RID: 2827
	public enum SkinMethod
	{
		// Token: 0x04003CA1 RID: 15521
		CPU,
		// Token: 0x04003CA2 RID: 15522
		GPU,
		// Token: 0x04003CA3 RID: 15523
		CPUAndGPU
	}

	// Token: 0x02000B0C RID: 2828
	protected struct Bone
	{
		// Token: 0x04003CA4 RID: 15524
		public Vector3 rotationAngles;

		// Token: 0x04003CA5 RID: 15525
		public Matrix4x4 worldToLocal;

		// Token: 0x04003CA6 RID: 15526
		public Matrix4x4 localToWorld;

		// Token: 0x04003CA7 RID: 15527
		public Matrix4x4 changeFromOriginal;

		// Token: 0x04003CA8 RID: 15528
		public float xposleftbulge;

		// Token: 0x04003CA9 RID: 15529
		public float xnegleftbulge;

		// Token: 0x04003CAA RID: 15530
		public float xposrightbulge;

		// Token: 0x04003CAB RID: 15531
		public float xnegrightbulge;

		// Token: 0x04003CAC RID: 15532
		public float yposleftbulge;

		// Token: 0x04003CAD RID: 15533
		public float ynegleftbulge;

		// Token: 0x04003CAE RID: 15534
		public float yposrightbulge;

		// Token: 0x04003CAF RID: 15535
		public float ynegrightbulge;

		// Token: 0x04003CB0 RID: 15536
		public float zposleftbulge;

		// Token: 0x04003CB1 RID: 15537
		public float znegleftbulge;

		// Token: 0x04003CB2 RID: 15538
		public float zposrightbulge;

		// Token: 0x04003CB3 RID: 15539
		public float znegrightbulge;
	}

	// Token: 0x02000B0D RID: 2829
	protected struct BoneGeneralWeights
	{
		// Token: 0x04003CB4 RID: 15540
		public int vertex;

		// Token: 0x04003CB5 RID: 15541
		public float weight;
	}

	// Token: 0x02000B0E RID: 2830
	protected struct BoneWeights
	{
		// Token: 0x04003CB6 RID: 15542
		public int vertex;

		// Token: 0x04003CB7 RID: 15543
		public float xweight;

		// Token: 0x04003CB8 RID: 15544
		public float yweight;

		// Token: 0x04003CB9 RID: 15545
		public float zweight;

		// Token: 0x04003CBA RID: 15546
		public float xleftbulge;

		// Token: 0x04003CBB RID: 15547
		public float xrightbulge;

		// Token: 0x04003CBC RID: 15548
		public float yleftbulge;

		// Token: 0x04003CBD RID: 15549
		public float yrightbulge;

		// Token: 0x04003CBE RID: 15550
		public float zleftbulge;

		// Token: 0x04003CBF RID: 15551
		public float zrightbulge;
	}

	// Token: 0x02000B0F RID: 2831
	protected struct BoneFullWeights
	{
		// Token: 0x04003CC0 RID: 15552
		public int vertex;
	}

	// Token: 0x02000B10 RID: 2832
	protected struct BaseVertToUVVert
	{
		// Token: 0x04003CC1 RID: 15553
		public int baseVertex;

		// Token: 0x04003CC2 RID: 15554
		public int UVVertex;
	}
}
