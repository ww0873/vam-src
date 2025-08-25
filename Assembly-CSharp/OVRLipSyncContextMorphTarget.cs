using System;
using UnityEngine;

// Token: 0x020007DA RID: 2010
public class OVRLipSyncContextMorphTarget : MonoBehaviour
{
	// Token: 0x060032DE RID: 13022 RVA: 0x001084A9 File Offset: 0x001068A9
	public OVRLipSyncContextMorphTarget()
	{
	}

	// Token: 0x060032DF RID: 13023 RVA: 0x001084D8 File Offset: 0x001068D8
	private void Start()
	{
		if (this.skinnedMeshRenderer == null)
		{
			Debug.Log("LipSyncContextMorphTarget.Start WARNING: Please set required public components!");
			return;
		}
		this.lipsyncContext = base.GetComponent<OVRLipSyncContextBase>();
		if (this.lipsyncContext == null)
		{
			Debug.Log("LipSyncContextMorphTarget.Start WARNING: No phoneme context component set to object");
		}
		this.lipsyncContext.Smoothing = this.SmoothAmount;
	}

	// Token: 0x060032E0 RID: 13024 RVA: 0x0010853C File Offset: 0x0010693C
	private void Update()
	{
		if (this.lipsyncContext != null && this.skinnedMeshRenderer != null)
		{
			OVRLipSync.Frame currentPhonemeFrame = this.lipsyncContext.GetCurrentPhonemeFrame();
			if (currentPhonemeFrame != null)
			{
				this.SetVisemeToMorphTarget(currentPhonemeFrame);
			}
			this.CheckForKeys();
		}
	}

	// Token: 0x060032E1 RID: 13025 RVA: 0x0010858C File Offset: 0x0010698C
	private void CheckForKeys()
	{
		if (this.enableVisemeSignals)
		{
			this.CheckVisemeKey(KeyCode.Alpha1, 0, 100);
			this.CheckVisemeKey(KeyCode.Alpha2, 1, 100);
			this.CheckVisemeKey(KeyCode.Alpha3, 2, 100);
			this.CheckVisemeKey(KeyCode.Alpha4, 3, 100);
			this.CheckVisemeKey(KeyCode.Alpha5, 4, 100);
			this.CheckVisemeKey(KeyCode.Alpha6, 5, 100);
			this.CheckVisemeKey(KeyCode.Alpha7, 6, 100);
			this.CheckVisemeKey(KeyCode.Alpha8, 7, 100);
			this.CheckVisemeKey(KeyCode.Alpha9, 8, 100);
			this.CheckVisemeKey(KeyCode.Alpha0, 9, 100);
			this.CheckVisemeKey(KeyCode.Q, 10, 100);
			this.CheckVisemeKey(KeyCode.W, 11, 100);
			this.CheckVisemeKey(KeyCode.E, 12, 100);
			this.CheckVisemeKey(KeyCode.R, 13, 100);
			this.CheckVisemeKey(KeyCode.T, 14, 100);
		}
	}

	// Token: 0x060032E2 RID: 13026 RVA: 0x00108650 File Offset: 0x00106A50
	private void SetVisemeToMorphTarget(OVRLipSync.Frame frame)
	{
		for (int i = 0; i < this.VisemeToBlendTargets.Length; i++)
		{
			if (this.VisemeToBlendTargets[i] != -1)
			{
				this.skinnedMeshRenderer.SetBlendShapeWeight(this.VisemeToBlendTargets[i], frame.Visemes[i] * 100f);
			}
		}
	}

	// Token: 0x060032E3 RID: 13027 RVA: 0x001086A5 File Offset: 0x00106AA5
	private void CheckVisemeKey(KeyCode key, int viseme, int amount)
	{
		if (Input.GetKeyDown(key))
		{
			this.lipsyncContext.SetVisemeBlend(this.KeySendVisemeSignal[viseme], amount);
		}
		if (Input.GetKeyUp(key))
		{
			this.lipsyncContext.SetVisemeBlend(this.KeySendVisemeSignal[viseme], 0);
		}
	}

	// Token: 0x040026DD RID: 9949
	public SkinnedMeshRenderer skinnedMeshRenderer;

	// Token: 0x040026DE RID: 9950
	public int[] VisemeToBlendTargets = new int[OVRLipSync.VisemeCount];

	// Token: 0x040026DF RID: 9951
	public bool enableVisemeSignals;

	// Token: 0x040026E0 RID: 9952
	public int[] KeySendVisemeSignal = new int[10];

	// Token: 0x040026E1 RID: 9953
	public int SmoothAmount = 100;

	// Token: 0x040026E2 RID: 9954
	private OVRLipSyncContextBase lipsyncContext;
}
