using System;
using mset;
using UnityEngine;

// Token: 0x0200031C RID: 796
public class SkySwab : MonoBehaviour
{
	// Token: 0x060012B7 RID: 4791 RVA: 0x0006AC58 File Offset: 0x00069058
	public SkySwab()
	{
	}

	// Token: 0x060012B8 RID: 4792 RVA: 0x0006ACE0 File Offset: 0x000690E0
	private void Start()
	{
		this.baseRot = base.transform.localRotation;
		this.scale = this.littleScale;
		this.manager = SkyManager.Get();
		if (!this.manager)
		{
			Debug.LogError("Failed to find SkyManager in scene. You'll probably want one of those.");
		}
	}

	// Token: 0x060012B9 RID: 4793 RVA: 0x0006AD2F File Offset: 0x0006912F
	private void OnMouseDown()
	{
		if (this.targetSky)
		{
			this.manager.BlendToGlobalSky(this.targetSky);
		}
	}

	// Token: 0x060012BA RID: 4794 RVA: 0x0006AD52 File Offset: 0x00069152
	private void OnMouseOver()
	{
		this.scale = this.hoverScale;
	}

	// Token: 0x060012BB RID: 4795 RVA: 0x0006AD60 File Offset: 0x00069160
	private void OnMouseExit()
	{
		this.scale = this.littleScale;
	}

	// Token: 0x060012BC RID: 4796 RVA: 0x0006AD70 File Offset: 0x00069170
	private void Update()
	{
		if (this.manager.GlobalSky == this.targetSky)
		{
			base.transform.Rotate(0f, 200f * Time.deltaTime, 0f);
			base.transform.localScale = this.bigScale;
		}
		else
		{
			base.transform.localRotation = this.baseRot;
			base.transform.localScale = this.scale;
		}
	}

	// Token: 0x04001072 RID: 4210
	public Sky targetSky;

	// Token: 0x04001073 RID: 4211
	public SkyManager manager;

	// Token: 0x04001074 RID: 4212
	private Vector3 scale = new Vector3(1f, 1.01f, 1f);

	// Token: 0x04001075 RID: 4213
	private Quaternion baseRot = Quaternion.identity;

	// Token: 0x04001076 RID: 4214
	public Vector3 bigScale = new Vector3(1.2f, 1.21f, 1.2f);

	// Token: 0x04001077 RID: 4215
	public Vector3 hoverScale = new Vector3(1f, 1f, 1f);

	// Token: 0x04001078 RID: 4216
	public Vector3 littleScale = new Vector3(0.75f, 0.76f, 0.75f);
}
