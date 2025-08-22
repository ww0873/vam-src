using System;
using UnityEngine;

// Token: 0x0200076B RID: 1899
[Serializable]
public class SoundFXRef
{
	// Token: 0x060030F3 RID: 12531 RVA: 0x000FED00 File Offset: 0x000FD100
	public SoundFXRef()
	{
	}

	// Token: 0x170005E5 RID: 1509
	// (get) Token: 0x060030F4 RID: 12532 RVA: 0x000FED13 File Offset: 0x000FD113
	public SoundFX soundFX
	{
		get
		{
			if (!this.initialized)
			{
				this.Init();
			}
			return this.soundFXCached;
		}
	}

	// Token: 0x170005E6 RID: 1510
	// (get) Token: 0x060030F5 RID: 12533 RVA: 0x000FED2C File Offset: 0x000FD12C
	// (set) Token: 0x060030F6 RID: 12534 RVA: 0x000FED34 File Offset: 0x000FD134
	public string name
	{
		get
		{
			return this.soundFXName;
		}
		set
		{
			this.soundFXName = value;
			this.Init();
		}
	}

	// Token: 0x060030F7 RID: 12535 RVA: 0x000FED43 File Offset: 0x000FD143
	private void Init()
	{
		this.soundFXCached = AudioManager.FindSoundFX(this.soundFXName, false);
		if (this.soundFXCached == null)
		{
			this.soundFXCached = AudioManager.FindSoundFX(string.Empty, false);
		}
		this.initialized = true;
	}

	// Token: 0x170005E7 RID: 1511
	// (get) Token: 0x060030F8 RID: 12536 RVA: 0x000FED7A File Offset: 0x000FD17A
	public int Length
	{
		get
		{
			return this.soundFX.Length;
		}
	}

	// Token: 0x170005E8 RID: 1512
	// (get) Token: 0x060030F9 RID: 12537 RVA: 0x000FED87 File Offset: 0x000FD187
	public bool IsValid
	{
		get
		{
			return this.soundFX.IsValid;
		}
	}

	// Token: 0x060030FA RID: 12538 RVA: 0x000FED94 File Offset: 0x000FD194
	public AudioClip GetClip()
	{
		return this.soundFX.GetClip();
	}

	// Token: 0x060030FB RID: 12539 RVA: 0x000FEDA1 File Offset: 0x000FD1A1
	public float GetClipLength(int idx)
	{
		return this.soundFX.GetClipLength(idx);
	}

	// Token: 0x060030FC RID: 12540 RVA: 0x000FEDAF File Offset: 0x000FD1AF
	public int PlaySound(float delaySecs = 0f)
	{
		return this.soundFX.PlaySound(delaySecs);
	}

	// Token: 0x060030FD RID: 12541 RVA: 0x000FEDBD File Offset: 0x000FD1BD
	public int PlaySoundAt(Vector3 pos, float delaySecs = 0f, float volume = 1f, float pitchMultiplier = 1f)
	{
		return this.soundFX.PlaySoundAt(pos, delaySecs, volume, pitchMultiplier);
	}

	// Token: 0x060030FE RID: 12542 RVA: 0x000FEDCF File Offset: 0x000FD1CF
	public void SetOnFinished(Action onFinished)
	{
		this.soundFX.SetOnFinished(onFinished);
	}

	// Token: 0x060030FF RID: 12543 RVA: 0x000FEDDD File Offset: 0x000FD1DD
	public void SetOnFinished(Action<object> onFinished, object obj)
	{
		this.soundFX.SetOnFinished(onFinished, obj);
	}

	// Token: 0x06003100 RID: 12544 RVA: 0x000FEDEC File Offset: 0x000FD1EC
	public bool StopSound()
	{
		return this.soundFX.StopSound();
	}

	// Token: 0x06003101 RID: 12545 RVA: 0x000FEDF9 File Offset: 0x000FD1F9
	public void AttachToParent(Transform parent)
	{
		this.soundFX.AttachToParent(parent);
	}

	// Token: 0x06003102 RID: 12546 RVA: 0x000FEE07 File Offset: 0x000FD207
	public void DetachFromParent()
	{
		this.soundFX.DetachFromParent();
	}

	// Token: 0x040024DC RID: 9436
	public string soundFXName = string.Empty;

	// Token: 0x040024DD RID: 9437
	private bool initialized;

	// Token: 0x040024DE RID: 9438
	private SoundFX soundFXCached;
}
