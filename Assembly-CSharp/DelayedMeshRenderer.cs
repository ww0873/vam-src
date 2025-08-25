using System;
using UnityEngine;

// Token: 0x02000C8D RID: 3213
public class DelayedMeshRenderer : MonoBehaviour
{
	// Token: 0x060060F5 RID: 24821 RVA: 0x0024A59F File Offset: 0x0024899F
	public DelayedMeshRenderer()
	{
	}

	// Token: 0x060060F6 RID: 24822 RVA: 0x0024A5A8 File Offset: 0x002489A8
	private void Start()
	{
		this.meshFilter = base.GetComponent<MeshFilter>();
		this.meshRenderer = base.GetComponent<MeshRenderer>();
		if (this.meshRenderer != null)
		{
			this.meshRenderer.enabled = false;
		}
		this.lastMatrix = base.transform.localToWorldMatrix;
	}

	// Token: 0x060060F7 RID: 24823 RVA: 0x0024A5FC File Offset: 0x002489FC
	private void Update()
	{
		if (this.meshFilter != null && this.meshRenderer != null)
		{
			Mesh mesh = this.meshFilter.mesh;
			for (int i = 0; i < this.meshRenderer.materials.Length; i++)
			{
				Material material = this.meshRenderer.materials[i];
				if (material != null)
				{
					Graphics.DrawMesh(mesh, this.lastMatrix, material, base.gameObject.layer, null, i, null, this.meshRenderer.shadowCastingMode, this.meshRenderer.receiveShadows);
				}
			}
		}
		this.lastMatrix = base.transform.localToWorldMatrix;
	}

	// Token: 0x04005082 RID: 20610
	protected MeshFilter meshFilter;

	// Token: 0x04005083 RID: 20611
	protected MeshRenderer meshRenderer;

	// Token: 0x04005084 RID: 20612
	protected Matrix4x4 lastMatrix;
}
