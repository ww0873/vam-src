using System;
using UnityEngine;

// Token: 0x02000ADF RID: 2783
public class DAZMergedSkin : DAZSkin
{
	// Token: 0x06004A15 RID: 18965 RVA: 0x0018FB0F File Offset: 0x0018DF0F
	public DAZMergedSkin()
	{
	}

	// Token: 0x06004A16 RID: 18966 RVA: 0x0018FB18 File Offset: 0x0018DF18
	public void Merge()
	{
		DAZMergedMesh component = base.GetComponent<DAZMergedMesh>();
		this.dazMesh = component;
		if (this.dazMesh == null)
		{
			Debug.LogError("Can't merge because no DAZMergedMesh component found");
			return;
		}
		string geometryId = component.targetMesh.geometryId;
		string geometryId2 = component.graftMesh.geometryId;
		this.geometryId = geometryId + ":" + geometryId2;
		string text = null;
		bool has2ndGraft = component.has2ndGraft;
		if (has2ndGraft)
		{
			text = component.graft2Mesh.geometryId;
			this.geometryId = this.geometryId + ":" + text;
		}
		DAZSkin[] components = base.GetComponents<DAZSkin>();
		if (components == null)
		{
			Debug.LogError("Can't merge because no DAZSkin components found");
			return;
		}
		DAZSkin dazskin = null;
		DAZSkin dazskin2 = null;
		DAZSkin dazskin3 = null;
		foreach (DAZSkin dazskin4 in components)
		{
			if (dazskin4.geometryId == geometryId)
			{
				dazskin = dazskin4;
			}
			else if (dazskin4.geometryId == geometryId2)
			{
				dazskin2 = dazskin4;
			}
			else if (has2ndGraft && dazskin4.geometryId == text)
			{
				dazskin3 = dazskin4;
			}
		}
		if (dazskin == null || dazskin2 == null)
		{
			Debug.LogError("Could not find both target and graft skin to merge");
			return;
		}
		int numBones = dazskin.numBones;
		int numBones2 = dazskin2.numBones;
		this._numBones = dazskin.numBones + numBones2;
		if (has2ndGraft)
		{
			this._numBones += dazskin3.numBones;
		}
		this.nodes = new DAZNode[this._numBones];
		for (int j = 0; j < dazskin.numBones; j++)
		{
			this.nodes[j] = dazskin.nodes[j];
		}
		int startGraftVertIndex = component.startGraftVertIndex;
		for (int k = 0; k < dazskin2.numBones; k++)
		{
			DAZNode daznode = dazskin2.nodes[k];
			int num = numBones + k;
			this.nodes[num] = new DAZNode();
			this.nodes[num].name = daznode.name;
			this.nodes[num].rotationOrder = daznode.rotationOrder;
			this.nodes[num].bulgeFactors = daznode.bulgeFactors;
			DAZMeshVertexWeights[] weights = daznode.weights;
			DAZMeshVertexWeights[] array2 = new DAZMeshVertexWeights[weights.Length];
			this.nodes[num].weights = array2;
			for (int l = 0; l < array2.Length; l++)
			{
				array2[l] = new DAZMeshVertexWeights();
				array2[l].vertex = weights[l].vertex + startGraftVertIndex;
				array2[l].weight = weights[l].weight;
				array2[l].xweight = weights[l].xweight;
				array2[l].yweight = weights[l].yweight;
				array2[l].zweight = weights[l].zweight;
				array2[l].xleftbulge = weights[l].xleftbulge;
				array2[l].xrightbulge = weights[l].xrightbulge;
				array2[l].yleftbulge = weights[l].yleftbulge;
				array2[l].yrightbulge = weights[l].yrightbulge;
				array2[l].zleftbulge = weights[l].zleftbulge;
				array2[l].zrightbulge = weights[l].zrightbulge;
			}
		}
		if (has2ndGraft)
		{
			int startGraft2VertIndex = component.startGraft2VertIndex;
			for (int m = 0; m < dazskin3.numBones; m++)
			{
				DAZNode daznode2 = dazskin3.nodes[m];
				int num2 = numBones + numBones2 + m;
				this.nodes[num2] = new DAZNode();
				this.nodes[num2].name = daznode2.name;
				this.nodes[num2].rotationOrder = daznode2.rotationOrder;
				this.nodes[num2].bulgeFactors = daznode2.bulgeFactors;
				DAZMeshVertexWeights[] weights2 = daznode2.weights;
				DAZMeshVertexWeights[] array3 = new DAZMeshVertexWeights[weights2.Length];
				this.nodes[num2].weights = array3;
				for (int n = 0; n < array3.Length; n++)
				{
					array3[n] = new DAZMeshVertexWeights();
					array3[n].vertex = weights2[n].vertex + startGraft2VertIndex;
					array3[n].weight = weights2[n].weight;
					array3[n].xweight = weights2[n].xweight;
					array3[n].yweight = weights2[n].yweight;
					array3[n].zweight = weights2[n].zweight;
					array3[n].xleftbulge = weights2[n].xleftbulge;
					array3[n].xrightbulge = weights2[n].xrightbulge;
					array3[n].yleftbulge = weights2[n].yleftbulge;
					array3[n].yrightbulge = weights2[n].yrightbulge;
					array3[n].zleftbulge = weights2[n].zleftbulge;
					array3[n].zrightbulge = weights2[n].zrightbulge;
				}
			}
		}
	}
}
