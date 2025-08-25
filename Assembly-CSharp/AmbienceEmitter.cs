using System;
using UnityEngine;

// Token: 0x0200075B RID: 1883
public class AmbienceEmitter : MonoBehaviour
{
	// Token: 0x06003075 RID: 12405 RVA: 0x000FBFBC File Offset: 0x000FA3BC
	public AmbienceEmitter()
	{
	}

	// Token: 0x06003076 RID: 12406 RVA: 0x000FC024 File Offset: 0x000FA424
	private void Awake()
	{
		if (this.autoActivate)
		{
			this.activated = true;
			this.nextPlayTime = Time.time + UnityEngine.Random.Range(this.randomRetriggerDelaySecs.x, this.randomRetriggerDelaySecs.y);
		}
		foreach (Transform x in this.playPositions)
		{
			if (x == null)
			{
				Debug.LogWarning("[AmbienceEmitter] Invalid play positions in " + base.name);
				this.playPositions = new Transform[0];
				break;
			}
		}
	}

	// Token: 0x06003077 RID: 12407 RVA: 0x000FC0BC File Offset: 0x000FA4BC
	private void Update()
	{
		if (this.activated && (this.playingIdx == -1 || this.autoRetrigger) && Time.time >= this.nextPlayTime)
		{
			this.Play();
			if (!this.autoRetrigger)
			{
				this.activated = false;
			}
		}
	}

	// Token: 0x06003078 RID: 12408 RVA: 0x000FC113 File Offset: 0x000FA513
	public void OnTriggerEnter(Collider col)
	{
		this.activated = !this.activated;
	}

	// Token: 0x06003079 RID: 12409 RVA: 0x000FC124 File Offset: 0x000FA524
	public void Play()
	{
		Transform transform = base.transform;
		if (this.playPositions.Length > 0)
		{
			int num = UnityEngine.Random.Range(0, this.playPositions.Length);
			while (this.playPositions.Length > 1 && num == this.lastPosIdx)
			{
				num = UnityEngine.Random.Range(0, this.playPositions.Length);
			}
			transform = this.playPositions[num];
			this.lastPosIdx = num;
		}
		this.playingIdx = this.ambientSounds[UnityEngine.Random.Range(0, this.ambientSounds.Length)].PlaySoundAt(transform.position, 0f, 1f, 1f);
		if (this.playingIdx != -1)
		{
			AudioManager.FadeInSound(this.playingIdx, this.fadeTime);
			this.nextPlayTime = Time.time + UnityEngine.Random.Range(this.randomRetriggerDelaySecs.x, this.randomRetriggerDelaySecs.y);
		}
	}

	// Token: 0x0600307A RID: 12410 RVA: 0x000FC20C File Offset: 0x000FA60C
	public void EnableEmitter(bool enable)
	{
		this.activated = enable;
		if (enable)
		{
			this.Play();
		}
		else if (this.playingIdx != -1)
		{
			AudioManager.FadeOutSound(this.playingIdx, this.fadeTime);
		}
	}

	// Token: 0x0400245D RID: 9309
	public SoundFXRef[] ambientSounds = new SoundFXRef[0];

	// Token: 0x0400245E RID: 9310
	public bool autoActivate = true;

	// Token: 0x0400245F RID: 9311
	[Tooltip("Automatically play the sound randomly again when checked.  Should be OFF for looping sounds")]
	public bool autoRetrigger = true;

	// Token: 0x04002460 RID: 9312
	[MinMax(2f, 4f, 0.1f, 10f)]
	public Vector2 randomRetriggerDelaySecs = new Vector2(2f, 4f);

	// Token: 0x04002461 RID: 9313
	[Tooltip("If defined, the sounds will randomly play from these transform positions, otherwise the sound will play from this transform")]
	public Transform[] playPositions = new Transform[0];

	// Token: 0x04002462 RID: 9314
	private bool activated;

	// Token: 0x04002463 RID: 9315
	private int playingIdx = -1;

	// Token: 0x04002464 RID: 9316
	private float nextPlayTime;

	// Token: 0x04002465 RID: 9317
	private float fadeTime = 0.25f;

	// Token: 0x04002466 RID: 9318
	private int lastPosIdx = -1;
}
