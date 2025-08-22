using System;
using UnityEngine;

// Token: 0x0200096E RID: 2414
public class OVRProgressIndicator : MonoBehaviour
{
	// Token: 0x06003C53 RID: 15443 RVA: 0x001243FB File Offset: 0x001227FB
	public OVRProgressIndicator()
	{
	}

	// Token: 0x06003C54 RID: 15444 RVA: 0x0012440E File Offset: 0x0012280E
	private void Awake()
	{
		this.progressImage.sortingOrder = 150;
	}

	// Token: 0x06003C55 RID: 15445 RVA: 0x00124420 File Offset: 0x00122820
	private void Update()
	{
		this.progressImage.sharedMaterial.SetFloat("_AlphaCutoff", 1f - this.currentProgress);
	}

	// Token: 0x04002E3C RID: 11836
	public MeshRenderer progressImage;

	// Token: 0x04002E3D RID: 11837
	[Range(0f, 1f)]
	public float currentProgress = 0.7f;
}
