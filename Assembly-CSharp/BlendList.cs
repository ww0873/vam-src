using System;
using mset;
using UnityEngine;

// Token: 0x02000317 RID: 791
public class BlendList : MonoBehaviour
{
	// Token: 0x0600129A RID: 4762 RVA: 0x000692C6 File Offset: 0x000676C6
	public BlendList()
	{
	}

	// Token: 0x0600129B RID: 4763 RVA: 0x000692E4 File Offset: 0x000676E4
	private void Start()
	{
		this.manager = SkyManager.Get();
		this.manager.BlendToGlobalSky(this.skyList[this.currSky], this.blendTime);
		this.blendStamp = Time.time;
	}

	// Token: 0x0600129C RID: 4764 RVA: 0x0006931C File Offset: 0x0006771C
	private void Update()
	{
		if (Time.time - this.blendStamp > this.blendTime + this.waitTime)
		{
			this.currSky = (this.currSky + 1) % this.skyList.Length;
			this.blendStamp = Time.time;
			this.manager.BlendToGlobalSky(this.skyList[this.currSky], this.blendTime);
		}
	}

	// Token: 0x04001034 RID: 4148
	public Sky[] skyList;

	// Token: 0x04001035 RID: 4149
	public float blendTime = 1f;

	// Token: 0x04001036 RID: 4150
	public float waitTime = 3f;

	// Token: 0x04001037 RID: 4151
	private float blendStamp;

	// Token: 0x04001038 RID: 4152
	private int currSky;

	// Token: 0x04001039 RID: 4153
	private SkyManager manager;
}
