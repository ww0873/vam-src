using System;
using UnityEngine;

// Token: 0x020007DB RID: 2011
public class OVRLipSyncContextTextureFlip : MonoBehaviour
{
	// Token: 0x060032E4 RID: 13028 RVA: 0x001086E5 File Offset: 0x00106AE5
	public OVRLipSyncContextTextureFlip()
	{
	}

	// Token: 0x060032E5 RID: 13029 RVA: 0x00108708 File Offset: 0x00106B08
	private void Start()
	{
		this.lipsyncContext = base.GetComponent<OVRLipSyncContextBase>();
		if (this.lipsyncContext == null)
		{
			Debug.Log("LipSyncContextTextureFlip.Start WARNING: No lip sync context component set to object");
		}
	}

	// Token: 0x060032E6 RID: 13030 RVA: 0x00108734 File Offset: 0x00106B34
	private void Update()
	{
		if (this.lipsyncContext != null && this.material != null)
		{
			OVRLipSync.Frame currentPhonemeFrame = this.lipsyncContext.GetCurrentPhonemeFrame();
			if (currentPhonemeFrame != null)
			{
				for (int i = 0; i < currentPhonemeFrame.Visemes.Length; i++)
				{
					this.oldFrame.Visemes[i] = this.oldFrame.Visemes[i] * this.smoothing + currentPhonemeFrame.Visemes[i] * (1f - this.smoothing);
				}
				this.SetVisemeToTexture();
			}
		}
	}

	// Token: 0x060032E7 RID: 13031 RVA: 0x001087CC File Offset: 0x00106BCC
	private void SetVisemeToTexture()
	{
		int num = -1;
		float num2 = 0f;
		for (int i = 0; i < this.oldFrame.Visemes.Length; i++)
		{
			if (this.oldFrame.Visemes[i] > num2)
			{
				num = i;
				num2 = this.oldFrame.Visemes[i];
			}
		}
		if (num != -1 && num < this.Textures.Length)
		{
			Texture texture = this.Textures[num];
			if (texture != null)
			{
				this.material.SetTexture("_MainTex", texture);
			}
		}
	}

	// Token: 0x040026E3 RID: 9955
	public Material material;

	// Token: 0x040026E4 RID: 9956
	public Texture[] Textures = new Texture[OVRLipSync.VisemeCount];

	// Token: 0x040026E5 RID: 9957
	public float smoothing;

	// Token: 0x040026E6 RID: 9958
	private OVRLipSyncContextBase lipsyncContext;

	// Token: 0x040026E7 RID: 9959
	private OVRLipSync.Frame oldFrame = new OVRLipSync.Frame();
}
