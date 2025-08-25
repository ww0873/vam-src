using System;
using UnityEngine;

// Token: 0x02000C4F RID: 3151
[ExecuteInEditMode]
public class SelectTarget : MonoBehaviour
{
	// Token: 0x06005BA1 RID: 23457 RVA: 0x0021A858 File Offset: 0x00218C58
	public SelectTarget()
	{
	}

	// Token: 0x06005BA2 RID: 23458 RVA: 0x0021A8CA File Offset: 0x00218CCA
	private void Awake()
	{
		if (this.material != null)
		{
			this.localMaterial = UnityEngine.Object.Instantiate<Material>(this.material);
		}
	}

	// Token: 0x06005BA3 RID: 23459 RVA: 0x0021A8EE File Offset: 0x00218CEE
	public void SetColor(Color c)
	{
		this.manualColorSet = true;
		this.manualColor = c;
	}

	// Token: 0x06005BA4 RID: 23460 RVA: 0x0021A900 File Offset: 0x00218D00
	private void Update()
	{
		Matrix4x4 localToWorldMatrix = base.transform.localToWorldMatrix;
		float num = this.meshScale;
		if (this.highlighted)
		{
			num *= this.highlightedScale;
		}
		else
		{
			num *= this.unhighlightedScale;
		}
		Vector3 s = new Vector3(num, num, num);
		Matrix4x4 identity = Matrix4x4.identity;
		identity.SetTRS(Vector3.zero, Quaternion.identity, s);
		Matrix4x4 matrix = localToWorldMatrix * identity;
		if (SelectTarget.useGlobalAlpha)
		{
			this.highlightColor.a = SelectTarget.globalAlpha;
			this.normalColor.a = SelectTarget.globalAlpha;
		}
		if (this.mesh != null && this.localMaterial != null)
		{
			if (this.highlighted)
			{
				this.localMaterial.color = this.highlightColor;
			}
			else if (this.manualColorSet)
			{
				this.localMaterial.color = this.manualColor;
			}
			else
			{
				this.localMaterial.color = this.normalColor;
			}
			Graphics.DrawMesh(this.mesh, matrix, this.localMaterial, base.gameObject.layer, null, 0, null, false, false);
		}
	}

	// Token: 0x06005BA5 RID: 23461 RVA: 0x0021AA2F File Offset: 0x00218E2F
	private void OnDestroy()
	{
		if (this.localMaterial != null)
		{
			UnityEngine.Object.Destroy(this.localMaterial);
		}
	}

	// Token: 0x06005BA6 RID: 23462 RVA: 0x0021AA4D File Offset: 0x00218E4D
	// Note: this type is marked as 'beforefieldinit'.
	static SelectTarget()
	{
	}

	// Token: 0x04004B91 RID: 19345
	public static bool useGlobalAlpha;

	// Token: 0x04004B92 RID: 19346
	public static float globalAlpha = 0.5f;

	// Token: 0x04004B93 RID: 19347
	public string selectionName;

	// Token: 0x04004B94 RID: 19348
	public Color normalColor = new Color(0f, 1f, 0f, 0.5f);

	// Token: 0x04004B95 RID: 19349
	public Color highlightColor = new Color(1f, 1f, 0f, 0.5f);

	// Token: 0x04004B96 RID: 19350
	public Material material;

	// Token: 0x04004B97 RID: 19351
	public float meshScale = 0.5f;

	// Token: 0x04004B98 RID: 19352
	public float highlightedScale = 1f;

	// Token: 0x04004B99 RID: 19353
	public float unhighlightedScale = 1f;

	// Token: 0x04004B9A RID: 19354
	public Mesh mesh;

	// Token: 0x04004B9B RID: 19355
	public bool highlighted;

	// Token: 0x04004B9C RID: 19356
	private Material localMaterial;

	// Token: 0x04004B9D RID: 19357
	private bool manualColorSet;

	// Token: 0x04004B9E RID: 19358
	private Color manualColor;
}
