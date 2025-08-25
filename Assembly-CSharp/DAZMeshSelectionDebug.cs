using System;
using UnityEngine;

// Token: 0x02000AED RID: 2797
[ExecuteInEditMode]
public class DAZMeshSelectionDebug : MonoBehaviour
{
	// Token: 0x06004AE0 RID: 19168 RVA: 0x0019E250 File Offset: 0x0019C650
	public DAZMeshSelectionDebug()
	{
	}

	// Token: 0x06004AE1 RID: 19169 RVA: 0x0019E2AF File Offset: 0x0019C6AF
	private void OnEnable()
	{
		this.dms = base.GetComponent<DAZMeshSelection>();
	}

	// Token: 0x06004AE2 RID: 19170 RVA: 0x0019E2C0 File Offset: 0x0019C6C0
	private void debug(int vertId)
	{
		Vector3[] array;
		Vector3[] array2;
		Vector4[] array3;
		if (this.dms.mesh.drawMorphedUVMappedMesh || this.dms.mesh.drawInEditorWhenNotPlaying)
		{
			array = this.dms.mesh.morphedUVVertices;
			array2 = this.dms.mesh.morphedUVNormals;
			array3 = this.dms.mesh.morphedUVTangents;
		}
		else if (this.dms.mesh.drawUVMappedMesh)
		{
			array = this.dms.mesh.UVVertices;
			array2 = this.dms.mesh.UVNormals;
			array3 = this.dms.mesh.UVTangents;
		}
		else
		{
			array = this.dms.mesh.baseVertices;
			array2 = this.dms.mesh.baseNormals;
			array3 = null;
		}
		if (this.debugVertex && vertId < array.Length)
		{
			MyDebug.DrawWireCube(array[vertId], this.boxSize, this.vertexColor);
		}
		if (this.debugNormal && vertId < array.Length)
		{
			Debug.DrawLine(array[vertId], array[vertId] + array2[vertId] * this.vectorSize, this.normalColor);
		}
		if (this.debugTangent && array3 != null && vertId < array.Length)
		{
			Vector3 a;
			a.x = array3[vertId].x;
			a.y = array3[vertId].y;
			a.z = array3[vertId].z;
			Debug.DrawLine(array[vertId], array[vertId] + a * this.vectorSize, this.tangentColor);
		}
	}

	// Token: 0x06004AE3 RID: 19171 RVA: 0x0019E4AC File Offset: 0x0019C8AC
	private void Update()
	{
		if (this.dms != null && this.on && Application.isEditor)
		{
			foreach (int vertId in this.dms.selectedVertices)
			{
				this.debug(vertId);
			}
		}
	}

	// Token: 0x040039B1 RID: 14769
	public bool on;

	// Token: 0x040039B2 RID: 14770
	public float boxSize = 0.01f;

	// Token: 0x040039B3 RID: 14771
	public float vectorSize = 0.01f;

	// Token: 0x040039B4 RID: 14772
	public Color vertexColor = Color.blue;

	// Token: 0x040039B5 RID: 14773
	public Color normalColor = Color.red;

	// Token: 0x040039B6 RID: 14774
	public Color tangentColor = Color.yellow;

	// Token: 0x040039B7 RID: 14775
	public bool debugVertex = true;

	// Token: 0x040039B8 RID: 14776
	public bool debugNormal = true;

	// Token: 0x040039B9 RID: 14777
	public bool debugTangent = true;

	// Token: 0x040039BA RID: 14778
	public DAZMeshSelection dms;
}
