using System;
using UnityEngine;

// Token: 0x02000C8C RID: 3212
public class DebugSMR : MonoBehaviour
{
	// Token: 0x060060F1 RID: 24817 RVA: 0x0024A3B2 File Offset: 0x002487B2
	public DebugSMR()
	{
	}

	// Token: 0x060060F2 RID: 24818 RVA: 0x0024A3BC File Offset: 0x002487BC
	private void report()
	{
		this.reportedIndex = this.vertexIndex;
		BoneWeight boneWeight = this.smr.sharedMesh.boneWeights[this.vertexIndex];
		Debug.Log(string.Concat(new object[]
		{
			"Boneweight: bi0:",
			boneWeight.boneIndex0,
			" w:",
			boneWeight.weight0,
			" bi1:",
			boneWeight.boneIndex1,
			" w:",
			boneWeight.weight1,
			" bi2:",
			boneWeight.boneIndex2,
			" w:",
			boneWeight.weight2,
			" bi3:",
			boneWeight.boneIndex3,
			" w:",
			boneWeight.weight3
		}));
		Debug.Log(string.Concat(new string[]
		{
			"Bone names: bi0: ",
			this.smr.bones[boneWeight.boneIndex0].name,
			" bi1: ",
			this.smr.bones[boneWeight.boneIndex1].name,
			" bi2: ",
			this.smr.bones[boneWeight.boneIndex2].name,
			" bi3: ",
			this.smr.bones[boneWeight.boneIndex3].name
		}));
	}

	// Token: 0x060060F3 RID: 24819 RVA: 0x0024A562 File Offset: 0x00248962
	private void Start()
	{
		this.smr = base.GetComponent<SkinnedMeshRenderer>();
		if (this.smr)
		{
			this.report();
		}
	}

	// Token: 0x060060F4 RID: 24820 RVA: 0x0024A586 File Offset: 0x00248986
	private void Update()
	{
		if (this.reportedIndex != this.vertexIndex)
		{
			this.report();
		}
	}

	// Token: 0x0400507F RID: 20607
	public int vertexIndex;

	// Token: 0x04005080 RID: 20608
	private SkinnedMeshRenderer smr;

	// Token: 0x04005081 RID: 20609
	private int reportedIndex;
}
