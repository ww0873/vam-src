using System;
using UnityEngine;

// Token: 0x02000309 RID: 777
[ExecuteInEditMode]
public class ProjectorMatrix : MonoBehaviour
{
	// Token: 0x06001254 RID: 4692 RVA: 0x0006609F File Offset: 0x0006449F
	public ProjectorMatrix()
	{
	}

	// Token: 0x06001255 RID: 4693 RVA: 0x000660AE File Offset: 0x000644AE
	private void Start()
	{
		this.t = base.transform;
		if (this.UpdateOnStart)
		{
			this.UpdateMatrix();
		}
	}

	// Token: 0x06001256 RID: 4694 RVA: 0x000660CD File Offset: 0x000644CD
	private void Update()
	{
		if (!this.UpdateOnStart)
		{
			this.UpdateMatrix();
		}
	}

	// Token: 0x06001257 RID: 4695 RVA: 0x000660E0 File Offset: 0x000644E0
	public void UpdateMatrix()
	{
		if (!this.CanUpdate)
		{
			return;
		}
		if (this.ProjectiveTranform != null)
		{
			Shader.SetGlobalMatrix(this.GlobalMatrixName.ToString(), this.ProjectiveTranform.worldToLocalMatrix * this.t.localToWorldMatrix);
		}
	}

	// Token: 0x04000FB2 RID: 4018
	public ProjectorMatrix.matrixName GlobalMatrixName;

	// Token: 0x04000FB3 RID: 4019
	public Transform ProjectiveTranform;

	// Token: 0x04000FB4 RID: 4020
	public bool UpdateOnStart;

	// Token: 0x04000FB5 RID: 4021
	public bool CanUpdate = true;

	// Token: 0x04000FB6 RID: 4022
	private Transform t;

	// Token: 0x0200030A RID: 778
	public enum matrixName
	{
		// Token: 0x04000FB8 RID: 4024
		_projectiveMatrWaves,
		// Token: 0x04000FB9 RID: 4025
		_projectiveMatrCausticScale
	}
}
