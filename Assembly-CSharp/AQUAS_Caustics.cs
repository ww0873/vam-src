using System;
using UnityEngine;

// Token: 0x02000014 RID: 20
public class AQUAS_Caustics : MonoBehaviour
{
	// Token: 0x060000AB RID: 171 RVA: 0x00005CD6 File Offset: 0x000040D6
	public AQUAS_Caustics()
	{
	}

	// Token: 0x060000AC RID: 172 RVA: 0x00005CE0 File Offset: 0x000040E0
	private void Start()
	{
		this.projector = base.GetComponent<Projector>();
		this.NextFrame();
		base.InvokeRepeating("NextFrame", 1f / this.fps, 1f / this.fps);
		this.projector.material.SetFloat("_WaterLevel", base.transform.parent.transform.position.y);
		this.projector.material.SetFloat("_DepthFade", base.transform.parent.transform.position.y - this.maxCausticDepth);
	}

	// Token: 0x060000AD RID: 173 RVA: 0x00005D90 File Offset: 0x00004190
	private void Update()
	{
		this.projector.material.SetFloat("_DepthFade", base.transform.parent.transform.position.y - this.maxCausticDepth);
	}

	// Token: 0x060000AE RID: 174 RVA: 0x00005DD6 File Offset: 0x000041D6
	private void NextFrame()
	{
		this.projector.material.SetTexture("_Texture", this.frames[this.frameIndex]);
		this.frameIndex = (this.frameIndex + 1) % this.frames.Length;
	}

	// Token: 0x040000A3 RID: 163
	public float fps;

	// Token: 0x040000A4 RID: 164
	public Texture2D[] frames;

	// Token: 0x040000A5 RID: 165
	public float maxCausticDepth;

	// Token: 0x040000A6 RID: 166
	private int frameIndex;

	// Token: 0x040000A7 RID: 167
	private Projector projector;
}
