using System;
using UnityEngine;

// Token: 0x02000010 RID: 16
public class AQUAS_BubbleMorph : MonoBehaviour
{
	// Token: 0x0600009D RID: 157 RVA: 0x00004E4B File Offset: 0x0000324B
	public AQUAS_BubbleMorph()
	{
	}

	// Token: 0x0600009E RID: 158 RVA: 0x00004E53 File Offset: 0x00003253
	private void Start()
	{
		this.skinnedMeshRenderer = base.GetComponent<SkinnedMeshRenderer>();
	}

	// Token: 0x0600009F RID: 159 RVA: 0x00004E64 File Offset: 0x00003264
	private void Update()
	{
		this.t += Time.deltaTime;
		this.t2 += Time.deltaTime;
		if (this.t < this.tTarget / 2f)
		{
			this.skinnedMeshRenderer.SetBlendShapeWeight(0, Mathf.Lerp(0f, 50f, this.t / (this.tTarget / 2f)));
			this.skinnedMeshRenderer.SetBlendShapeWeight(1, Mathf.Lerp(50f, 0f, this.t / (this.tTarget / 2f)));
		}
		else if (this.t >= this.tTarget / 2f && this.t < this.tTarget)
		{
			this.skinnedMeshRenderer.SetBlendShapeWeight(0, Mathf.Lerp(50f, 100f, this.t / this.tTarget));
			this.skinnedMeshRenderer.SetBlendShapeWeight(1, Mathf.Lerp(0f, 50f, this.t / this.tTarget));
		}
		else if (this.t >= this.tTarget && this.t < this.tTarget * 1.5f)
		{
			this.skinnedMeshRenderer.SetBlendShapeWeight(0, Mathf.Lerp(100f, 50f, this.t / (this.tTarget * 1.5f)));
			this.skinnedMeshRenderer.SetBlendShapeWeight(1, Mathf.Lerp(50f, 100f, this.t / (this.tTarget * 1.5f)));
		}
		else if (this.t >= this.tTarget * 1.5f && this.t < this.tTarget * 2f)
		{
			this.skinnedMeshRenderer.SetBlendShapeWeight(0, Mathf.Lerp(50f, 0f, this.t / (this.tTarget * 2f)));
			this.skinnedMeshRenderer.SetBlendShapeWeight(1, Mathf.Lerp(100f, 50f, this.t / (this.tTarget * 2f)));
		}
		else
		{
			this.t = 0f;
		}
	}

	// Token: 0x0400008C RID: 140
	private float t;

	// Token: 0x0400008D RID: 141
	private float t2;

	// Token: 0x0400008E RID: 142
	[Space(5f)]
	[Header("Duration of a full morphing cycle")]
	public float tTarget;

	// Token: 0x0400008F RID: 143
	private SkinnedMeshRenderer skinnedMeshRenderer;
}
