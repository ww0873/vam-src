using System;
using UnityEngine;

// Token: 0x02000AE0 RID: 2784
public class DAZMergedSkinV2 : DAZSkinV2
{
	// Token: 0x06004A17 RID: 18967 RVA: 0x0019BB42 File Offset: 0x00199F42
	public DAZMergedSkinV2()
	{
	}

	// Token: 0x06004A18 RID: 18968 RVA: 0x0019BB4C File Offset: 0x00199F4C
	public void Merge()
	{
		DAZMergedMesh component = base.GetComponent<DAZMergedMesh>();
		this.dazMesh = component;
		if (this.dazMesh == null)
		{
			Debug.LogError("Could not merge because no DAZMergedMesh component found");
			return;
		}
		string sceneGeometryId = component.targetMesh.sceneGeometryId;
		string sceneGeometryId2 = component.graftMesh.sceneGeometryId;
		this.sceneGeometryId = sceneGeometryId + ":" + sceneGeometryId2;
		string text = null;
		bool has2ndGraft = component.has2ndGraft;
		if (has2ndGraft)
		{
			text = component.graft2Mesh.sceneGeometryId;
			this.sceneGeometryId = this.sceneGeometryId + ":" + text;
		}
		DAZSkinV2[] components = base.GetComponents<DAZSkinV2>();
		if (components == null)
		{
			Debug.LogError("Can't merge because no DAZSkin components found");
			return;
		}
		DAZSkinV2 dazskinV = null;
		DAZSkinV2 dazskinV2 = null;
		DAZSkinV2 dazskinV3 = null;
		this._hasGeneralWeights = true;
		foreach (DAZSkinV2 dazskinV4 in components)
		{
			if (dazskinV4.sceneGeometryId == sceneGeometryId)
			{
				dazskinV = dazskinV4;
				dazskinV.skin = false;
				dazskinV.draw = false;
				if (!dazskinV.hasGeneralWeights)
				{
					this._hasGeneralWeights = false;
				}
			}
			else if (dazskinV4.sceneGeometryId == sceneGeometryId2)
			{
				dazskinV2 = dazskinV4;
				dazskinV2.skin = false;
				dazskinV2.draw = false;
				if (!dazskinV2.hasGeneralWeights)
				{
					this._hasGeneralWeights = false;
				}
			}
			else if (has2ndGraft && dazskinV4.sceneGeometryId == text)
			{
				dazskinV3 = dazskinV4;
				dazskinV3.skin = false;
				dazskinV3.draw = false;
				if (!dazskinV3.hasGeneralWeights)
				{
					this._hasGeneralWeights = false;
				}
			}
		}
		if (dazskinV == null || dazskinV2 == null)
		{
			Debug.LogError("Could not find both target and graft skin to merge");
			return;
		}
		int numBones = dazskinV.numBones;
		int numBones2 = dazskinV2.numBones;
		this._numBones = dazskinV.numBones + numBones2;
		if (has2ndGraft)
		{
			this._numBones += dazskinV3.numBones;
		}
		this.nodes = new DAZSkinV2Node[this._numBones];
		int num = component.numGraftBaseVertices + component.numGraft2BaseVertices;
		for (int j = 0; j < dazskinV.numBones; j++)
		{
			DAZSkinV2Node dazskinV2Node = dazskinV.nodes[j];
			this.nodes[j] = new DAZSkinV2Node();
			this.nodes[j].name = dazskinV2Node.name;
			this.nodes[j].rotationOrder = dazskinV2Node.rotationOrder;
			this.nodes[j].bulgeFactors = dazskinV2Node.bulgeFactors;
			DAZSkinV2VertexWeights[] weights = dazskinV2Node.weights;
			DAZSkinV2VertexWeights[] array2 = new DAZSkinV2VertexWeights[weights.Length];
			this.nodes[j].weights = array2;
			for (int k = 0; k < array2.Length; k++)
			{
				array2[k] = new DAZSkinV2VertexWeights();
				if (weights[k].vertex >= component.numTargetBaseVertices)
				{
					array2[k].vertex = weights[k].vertex + num;
				}
				else
				{
					array2[k].vertex = weights[k].vertex;
				}
				array2[k].xweight = weights[k].xweight;
				array2[k].yweight = weights[k].yweight;
				array2[k].zweight = weights[k].zweight;
				array2[k].xleftbulge = weights[k].xleftbulge;
				array2[k].xrightbulge = weights[k].xrightbulge;
				array2[k].yleftbulge = weights[k].yleftbulge;
				array2[k].yrightbulge = weights[k].yrightbulge;
				array2[k].zleftbulge = weights[k].zleftbulge;
				array2[k].zrightbulge = weights[k].zrightbulge;
			}
			int[] fullyWeightedVertices = dazskinV2Node.fullyWeightedVertices;
			int[] array3 = new int[fullyWeightedVertices.Length];
			this.nodes[j].fullyWeightedVertices = array3;
			for (int l = 0; l < array3.Length; l++)
			{
				if (fullyWeightedVertices[l] >= component.numTargetBaseVertices)
				{
					array3[l] = fullyWeightedVertices[l] + num;
				}
				else
				{
					array3[l] = fullyWeightedVertices[l];
				}
			}
			DAZSkinV2GeneralVertexWeights[] generalWeights = dazskinV2Node.generalWeights;
			DAZSkinV2GeneralVertexWeights[] array4 = new DAZSkinV2GeneralVertexWeights[generalWeights.Length];
			this.nodes[j].generalWeights = array4;
			for (int m = 0; m < array4.Length; m++)
			{
				array4[m] = new DAZSkinV2GeneralVertexWeights();
				if (generalWeights[m].vertex >= component.numTargetBaseVertices)
				{
					array4[m].vertex = generalWeights[m].vertex + num;
				}
				else
				{
					array4[m].vertex = generalWeights[m].vertex;
				}
				array4[m].weight = generalWeights[m].weight;
			}
		}
		int startGraftVertIndex = component.startGraftVertIndex;
		int num2 = component.numTargetUVVertices + component.numGraft2BaseVertices;
		for (int n = 0; n < dazskinV2.numBones; n++)
		{
			DAZSkinV2Node dazskinV2Node2 = dazskinV2.nodes[n];
			int num3 = numBones + n;
			this.nodes[num3] = new DAZSkinV2Node();
			this.nodes[num3].name = dazskinV2Node2.name;
			this.nodes[num3].rotationOrder = dazskinV2Node2.rotationOrder;
			this.nodes[num3].bulgeFactors = dazskinV2Node2.bulgeFactors;
			DAZSkinV2VertexWeights[] weights2 = dazskinV2Node2.weights;
			DAZSkinV2VertexWeights[] array5 = new DAZSkinV2VertexWeights[weights2.Length];
			this.nodes[num3].weights = array5;
			for (int num4 = 0; num4 < array5.Length; num4++)
			{
				array5[num4] = new DAZSkinV2VertexWeights();
				if (weights2[num4].vertex >= component.numGraftBaseVertices)
				{
					array5[num4].vertex = weights2[num4].vertex + num2;
				}
				else
				{
					array5[num4].vertex = weights2[num4].vertex + startGraftVertIndex;
				}
				array5[num4].xweight = weights2[num4].xweight;
				array5[num4].yweight = weights2[num4].yweight;
				array5[num4].zweight = weights2[num4].zweight;
				array5[num4].xleftbulge = weights2[num4].xleftbulge;
				array5[num4].xrightbulge = weights2[num4].xrightbulge;
				array5[num4].yleftbulge = weights2[num4].yleftbulge;
				array5[num4].yrightbulge = weights2[num4].yrightbulge;
				array5[num4].zleftbulge = weights2[num4].zleftbulge;
				array5[num4].zrightbulge = weights2[num4].zrightbulge;
			}
			int[] fullyWeightedVertices2 = dazskinV2Node2.fullyWeightedVertices;
			int[] array6 = new int[fullyWeightedVertices2.Length];
			this.nodes[num3].fullyWeightedVertices = array6;
			for (int num5 = 0; num5 < array6.Length; num5++)
			{
				if (fullyWeightedVertices2[num5] >= component.numGraftBaseVertices)
				{
					array6[num5] = fullyWeightedVertices2[num5] + num2;
				}
				else
				{
					array6[num5] = fullyWeightedVertices2[num5] + startGraftVertIndex;
				}
			}
			DAZSkinV2GeneralVertexWeights[] generalWeights2 = dazskinV2Node2.generalWeights;
			DAZSkinV2GeneralVertexWeights[] array7 = new DAZSkinV2GeneralVertexWeights[generalWeights2.Length];
			this.nodes[num3].generalWeights = array7;
			for (int num6 = 0; num6 < array7.Length; num6++)
			{
				array7[num6] = new DAZSkinV2GeneralVertexWeights();
				if (generalWeights2[num6].vertex >= component.numGraftBaseVertices)
				{
					array7[num6].vertex = generalWeights2[num6].vertex + num2;
				}
				else
				{
					array7[num6].vertex = generalWeights2[num6].vertex + startGraftVertIndex;
				}
				array7[num6].weight = generalWeights2[num6].weight;
			}
		}
		if (has2ndGraft)
		{
			int startGraft2VertIndex = component.startGraft2VertIndex;
			int num7 = component.numTargetUVVertices + component.numGraftUVVertices;
			for (int num8 = 0; num8 < dazskinV3.numBones; num8++)
			{
				DAZSkinV2Node dazskinV2Node3 = dazskinV3.nodes[num8];
				int num9 = numBones + numBones2 + num8;
				this.nodes[num9] = new DAZSkinV2Node();
				this.nodes[num9].name = dazskinV2Node3.name;
				this.nodes[num9].rotationOrder = dazskinV2Node3.rotationOrder;
				this.nodes[num9].bulgeFactors = dazskinV2Node3.bulgeFactors;
				DAZSkinV2VertexWeights[] weights3 = dazskinV2Node3.weights;
				DAZSkinV2VertexWeights[] array8 = new DAZSkinV2VertexWeights[weights3.Length];
				this.nodes[num9].weights = array8;
				for (int num10 = 0; num10 < array8.Length; num10++)
				{
					array8[num10] = new DAZSkinV2VertexWeights();
					if (weights3[num10].vertex >= component.numGraft2BaseVertices)
					{
						array8[num10].vertex = weights3[num10].vertex + num7;
					}
					else
					{
						array8[num10].vertex = weights3[num10].vertex + startGraft2VertIndex;
					}
					array8[num10].xweight = weights3[num10].xweight;
					array8[num10].yweight = weights3[num10].yweight;
					array8[num10].zweight = weights3[num10].zweight;
					array8[num10].xleftbulge = weights3[num10].xleftbulge;
					array8[num10].xrightbulge = weights3[num10].xrightbulge;
					array8[num10].yleftbulge = weights3[num10].yleftbulge;
					array8[num10].yrightbulge = weights3[num10].yrightbulge;
					array8[num10].zleftbulge = weights3[num10].zleftbulge;
					array8[num10].zrightbulge = weights3[num10].zrightbulge;
				}
				int[] fullyWeightedVertices3 = dazskinV2Node3.fullyWeightedVertices;
				int[] array9 = new int[fullyWeightedVertices3.Length];
				this.nodes[num9].fullyWeightedVertices = array9;
				for (int num11 = 0; num11 < array9.Length; num11++)
				{
					if (fullyWeightedVertices3[num11] >= component.numGraft2BaseVertices)
					{
						array9[num11] = fullyWeightedVertices3[num11] + num7;
					}
					else
					{
						array9[num11] = fullyWeightedVertices3[num11] + startGraft2VertIndex;
					}
				}
				DAZSkinV2GeneralVertexWeights[] generalWeights3 = dazskinV2Node3.generalWeights;
				DAZSkinV2GeneralVertexWeights[] array10 = new DAZSkinV2GeneralVertexWeights[generalWeights3.Length];
				this.nodes[num9].generalWeights = array10;
				for (int num12 = 0; num12 < array10.Length; num12++)
				{
					array10[num12] = new DAZSkinV2GeneralVertexWeights();
					if (generalWeights3[num12].vertex >= component.numGraft2BaseVertices)
					{
						array10[num12].vertex = generalWeights3[num12].vertex + num7;
					}
					else
					{
						array10[num12].vertex = generalWeights3[num12].vertex + startGraft2VertIndex;
					}
					array10[num12].weight = generalWeights3[num12].weight;
				}
			}
		}
	}
}
