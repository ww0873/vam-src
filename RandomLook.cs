using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000440 RID: 1088
public class RandomLook : MonoBehaviour
{
	// Token: 0x06001B13 RID: 6931 RVA: 0x00096288 File Offset: 0x00094688
	public RandomLook()
	{
	}

	// Token: 0x06001B14 RID: 6932 RVA: 0x00096320 File Offset: 0x00094720
	private void Start()
	{
		if (this.eyeballJoints.Length == 0)
		{
			this.randomLook = false;
			this.eyesTrackTarget = false;
		}
		else if (this.eyeballJoints[0] == null | this.eyeballJoints[1] == null)
		{
			this.randomLook = false;
			this.eyesTrackTarget = false;
		}
		else
		{
			base.StartCoroutine(this.GetStartAngles());
			if (!this.randomLook & this.lookTarget != null)
			{
				this.eyesTrackTarget = true;
			}
			else if (this.randomLook)
			{
				this.InitializeRandomLook();
			}
			else
			{
				this.eyesTrackTarget = false;
			}
		}
		if (!string.IsNullOrEmpty(this.blinkBlendShapeName))
		{
			this.m = this.headMesh.sharedMesh;
			for (int i = 0; i < this.m.blendShapeCount; i++)
			{
				if (this.m.GetBlendShapeName(i) == this.blinkBlendShapeName)
				{
					this.blinkBSindex = i;
					break;
				}
			}
			if (this.blinkBSindex == -1)
			{
				MonoBehaviour.print("Warning: blink blendshape " + this.blinkBlendShapeName + " not found");
				this.randomBlink = false;
			}
		}
		this.lastBlinkTime = 0f;
		this.blinkPeriodRandom = (UnityEngine.Random.Range(0f, 10f) - 5f) / 5f;
		if (this.blinkClosedLimit < this.blinkOpenLimit)
		{
			this.blinkClosedLimit = this.blinkOpenLimit;
		}
	}

	// Token: 0x06001B15 RID: 6933 RVA: 0x000964B0 File Offset: 0x000948B0
	private void FixedUpdate()
	{
		if (this.randomBlink & Time.time - (this.lastBlinkTime + this.blinkPeriodRandom) > this.blinkPeriod)
		{
			this.StartBlink();
			this.lastBlinkTime = Time.time;
			this.blinkPeriodRandom = (UnityEngine.Random.Range(0f, 10f) - 5f) / 5f;
		}
		if (this.randomLook & Time.time - (this.lastLookChangeTime + this.lookChangePeriodRandom) > this.changeLookPeriod)
		{
			this.StartRandomLook();
			this.lastLookChangeTime = Time.time;
			this.lookChangePeriodRandom = (UnityEngine.Random.Range(0f, 10f) - 5f) / 5f;
		}
		else if (this.eyesTrackTarget)
		{
			this.eyeballJoints[0].LookAt(this.lookTarget);
			this.eyeballJoints[0].Rotate(this.lookAngleOffset);
			this.eyeballJoints[1].localRotation = this.eyeballJoints[0].localRotation;
			float num = Quaternion.Angle(this.eyeballJoints[0].localRotation * Quaternion.Euler(this.lookAngleOffset), this.EyeballStartDirection[0]);
			if (num > this.eyeTrackMaxAngle)
			{
				this.eyeballJoints[0].localRotation = Quaternion.Lerp(this.EyeballStartDirection[0], this.eyeballJoints[0].localRotation, this.eyeTrackMaxAngle / num);
				this.eyeballJoints[1].localRotation = this.eyeballJoints[0].localRotation;
			}
		}
	}

	// Token: 0x06001B16 RID: 6934 RVA: 0x00096654 File Offset: 0x00094A54
	public void StartRandomLook()
	{
		if (this.randomLookTarget == null)
		{
			this.InitializeRandomLook();
		}
		float num = 1f;
		float num2 = UnityEngine.Random.Range(-this.randomLookMaxAngle, this.randomLookMaxAngle) * 3.1415927f / 180f;
		float num3 = UnityEngine.Random.Range(-this.randomLookMaxAngle, this.randomLookMaxAngle) * 3.1415927f / 180f;
		this.randomLookTarget.localPosition = new Vector3(num / Mathf.Tan(1.5707964f - num2), num / Mathf.Tan(1.5707964f - num3), num);
		this.eyeballJoints[0].LookAt(this.randomLookTarget);
		this.eyeballJoints[0].Rotate(this.lookAngleOffset);
		this.eyeballJoints[1].localRotation = this.eyeballJoints[0].localRotation;
	}

	// Token: 0x06001B17 RID: 6935 RVA: 0x0009672C File Offset: 0x00094B2C
	private void InitializeRandomLook()
	{
		this.eyesTrackTarget = false;
		this.EyesCenterObject = new GameObject("EyesCenter");
		this.EyesCenterObject.transform.position = (this.eyeballJoints[0].position + this.eyeballJoints[1].position) / 2f;
		this.EyesCenterObject.transform.forward = this.eyesForwardVector;
		this.EyesCenterObject.transform.parent = this.eyeballJoints[0].parent;
		this.RandomLookTarget = new GameObject("RandomLookTarget");
		this.randomLookTarget = this.RandomLookTarget.transform;
		this.randomLookTarget.SetParent(this.EyesCenterObject.transform, true);
	}

	// Token: 0x06001B18 RID: 6936 RVA: 0x000967F4 File Offset: 0x00094BF4
	public void StartBlink()
	{
		if (!this.blinking)
		{
			base.StartCoroutine(this.BlinkBlend());
		}
	}

	// Token: 0x06001B19 RID: 6937 RVA: 0x00096810 File Offset: 0x00094C10
	private IEnumerator BlinkBlend()
	{
		this.blinking = true;
		float dt = Time.deltaTime;
		int steps = Mathf.RoundToInt(0.1f / (this.blinkSpeed * 3f + 0.01f) / dt * (this.blinkClosedLimit - this.blinkOpenLimit));
		this.headMesh.SetBlendShapeWeight(this.blinkBSindex, 0f);
		for (int i = 0; i < steps; i++)
		{
			float amount = (float)i / (float)steps * 100f;
			this.headMesh.SetBlendShapeWeight(this.blinkBSindex, amount);
			if (i % 2 == 0)
			{
				yield return new WaitForSeconds(dt);
			}
		}
		this.headMesh.SetBlendShapeWeight(this.blinkBSindex, this.blinkClosedLimit * 100f);
		yield return new WaitForSeconds(this.blinkHoldTime);
		for (int j = 0; j < steps; j++)
		{
			float amount2 = (float)j / (float)steps * 100f;
			this.headMesh.SetBlendShapeWeight(this.blinkBSindex, this.blinkClosedLimit * 100f - amount2);
			if (j % 2 == 0)
			{
				yield return new WaitForSeconds(dt);
			}
		}
		this.headMesh.SetBlendShapeWeight(this.blinkBSindex, 0f);
		this.blinking = false;
		yield break;
	}

	// Token: 0x06001B1A RID: 6938 RVA: 0x0009682C File Offset: 0x00094C2C
	private IEnumerator GetStartAngles()
	{
		yield return new WaitForSeconds(0.3f);
		this.EyeballStartDirection[0] = this.eyeballJoints[0].localRotation;
		this.EyeballStartDirection[1] = this.eyeballJoints[1].localRotation;
		yield break;
	}

	// Token: 0x040016AC RID: 5804
	public Transform[] eyeballJoints;

	// Token: 0x040016AD RID: 5805
	public SkinnedMeshRenderer headMesh;

	// Token: 0x040016AE RID: 5806
	public string blinkBlendShapeName;

	// Token: 0x040016AF RID: 5807
	[Tooltip("Eyes look target. Leave blank for random")]
	public Transform lookTarget;

	// Token: 0x040016B0 RID: 5808
	public bool randomLook = true;

	// Token: 0x040016B1 RID: 5809
	public bool eyesTrackTarget;

	// Token: 0x040016B2 RID: 5810
	public bool randomBlink = true;

	// Token: 0x040016B3 RID: 5811
	[Header("\tLook Settings")]
	[Tooltip("Average time between changing random look direction (s)")]
	[Range(0.5f, 10f)]
	public float changeLookPeriod = 3f;

	// Token: 0x040016B4 RID: 5812
	[Tooltip("Maximum random look direction angle (degrees, < 90)")]
	[Range(0f, 70f)]
	public float randomLookMaxAngle = 20f;

	// Token: 0x040016B5 RID: 5813
	[Tooltip("Cutoff angle for eye tracking (degrees, < 90)")]
	[Range(0f, 70f)]
	public float eyeTrackMaxAngle = 10f;

	// Token: 0x040016B6 RID: 5814
	[Header("\tBlink Settings")]
	[Tooltip("Average blink time (s)")]
	[Range(0.5f, 10f)]
	public float blinkPeriod = 3f;

	// Token: 0x040016B7 RID: 5815
	[Tooltip("Blink speed")]
	[Range(0f, 1f)]
	public float blinkSpeed = 0.7f;

	// Token: 0x040016B8 RID: 5816
	[Tooltip("Blink hold closed time")]
	[Range(0f, 0.2f)]
	public float blinkHoldTime = 0.03f;

	// Token: 0x040016B9 RID: 5817
	[Tooltip("Blink closed amount")]
	[Range(0f, 1f)]
	public float blinkClosedLimit = 1f;

	// Token: 0x040016BA RID: 5818
	[Tooltip("Blink open amount")]
	[Range(0f, 1f)]
	public float blinkOpenLimit;

	// Token: 0x040016BB RID: 5819
	public Vector3 lookAngleOffset = Vector3.zero;

	// Token: 0x040016BC RID: 5820
	public Vector3 eyesForwardVector = Vector3.forward;

	// Token: 0x040016BD RID: 5821
	private int blinkBSindex = -1;

	// Token: 0x040016BE RID: 5822
	private float lastBlinkTime;

	// Token: 0x040016BF RID: 5823
	private float lastLookChangeTime;

	// Token: 0x040016C0 RID: 5824
	private float blinkPeriodRandom;

	// Token: 0x040016C1 RID: 5825
	private float lookChangePeriodRandom;

	// Token: 0x040016C2 RID: 5826
	public bool blinking;

	// Token: 0x040016C3 RID: 5827
	private Quaternion[] EyeballStartDirection = new Quaternion[2];

	// Token: 0x040016C4 RID: 5828
	private GameObject RandomLookTarget;

	// Token: 0x040016C5 RID: 5829
	private GameObject EyesCenterObject;

	// Token: 0x040016C6 RID: 5830
	private Transform randomLookTarget;

	// Token: 0x040016C7 RID: 5831
	private Mesh m;

	// Token: 0x02000F59 RID: 3929
	[CompilerGenerated]
	private sealed class <BlinkBlend>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x06007392 RID: 29586 RVA: 0x00096847 File Offset: 0x00094C47
		[DebuggerHidden]
		public <BlinkBlend>c__Iterator0()
		{
		}

		// Token: 0x06007393 RID: 29587 RVA: 0x00096850 File Offset: 0x00094C50
		public bool MoveNext()
		{
			uint num = (uint)this.$PC;
			this.$PC = -1;
			switch (num)
			{
			case 0U:
				this.blinking = true;
				dt = Time.deltaTime;
				steps = Mathf.RoundToInt(0.1f / (this.blinkSpeed * 3f + 0.01f) / dt * (this.blinkClosedLimit - this.blinkOpenLimit));
				this.headMesh.SetBlendShapeWeight(this.blinkBSindex, 0f);
				i = 0;
				break;
			case 1U:
				IL_121:
				i++;
				break;
			case 2U:
				j = 0;
				goto IL_230;
			case 3U:
				IL_222:
				j++;
				goto IL_230;
			default:
				return false;
			}
			if (i >= steps)
			{
				this.headMesh.SetBlendShapeWeight(this.blinkBSindex, this.blinkClosedLimit * 100f);
				this.$current = new WaitForSeconds(this.blinkHoldTime);
				if (!this.$disposing)
				{
					this.$PC = 2;
				}
				return true;
			}
			amount = (float)i / (float)steps * 100f;
			this.headMesh.SetBlendShapeWeight(this.blinkBSindex, amount);
			if (i % 2 == 0)
			{
				this.$current = new WaitForSeconds(dt);
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
			}
			goto IL_121;
			IL_230:
			if (j >= steps)
			{
				this.headMesh.SetBlendShapeWeight(this.blinkBSindex, 0f);
				this.blinking = false;
				this.$PC = -1;
			}
			else
			{
				amount2 = (float)j / (float)steps * 100f;
				this.headMesh.SetBlendShapeWeight(this.blinkBSindex, this.blinkClosedLimit * 100f - amount2);
				if (j % 2 == 0)
				{
					this.$current = new WaitForSeconds(dt);
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				goto IL_222;
			}
			return false;
		}

		// Token: 0x170010EB RID: 4331
		// (get) Token: 0x06007394 RID: 29588 RVA: 0x00096AD4 File Offset: 0x00094ED4
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x170010EC RID: 4332
		// (get) Token: 0x06007395 RID: 29589 RVA: 0x00096ADC File Offset: 0x00094EDC
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x06007396 RID: 29590 RVA: 0x00096AE4 File Offset: 0x00094EE4
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x06007397 RID: 29591 RVA: 0x00096AF4 File Offset: 0x00094EF4
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x04006778 RID: 26488
		internal float <dt>__0;

		// Token: 0x04006779 RID: 26489
		internal int <steps>__0;

		// Token: 0x0400677A RID: 26490
		internal int <i>__1;

		// Token: 0x0400677B RID: 26491
		internal float <amount>__2;

		// Token: 0x0400677C RID: 26492
		internal int <i>__3;

		// Token: 0x0400677D RID: 26493
		internal float <amount>__4;

		// Token: 0x0400677E RID: 26494
		internal RandomLook $this;

		// Token: 0x0400677F RID: 26495
		internal object $current;

		// Token: 0x04006780 RID: 26496
		internal bool $disposing;

		// Token: 0x04006781 RID: 26497
		internal int $PC;
	}

	// Token: 0x02000F5A RID: 3930
	[CompilerGenerated]
	private sealed class <GetStartAngles>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x06007398 RID: 29592 RVA: 0x00096AFB File Offset: 0x00094EFB
		[DebuggerHidden]
		public <GetStartAngles>c__Iterator1()
		{
		}

		// Token: 0x06007399 RID: 29593 RVA: 0x00096B04 File Offset: 0x00094F04
		public bool MoveNext()
		{
			uint num = (uint)this.$PC;
			this.$PC = -1;
			switch (num)
			{
			case 0U:
				this.$current = new WaitForSeconds(0.3f);
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
			case 1U:
				this.EyeballStartDirection[0] = this.eyeballJoints[0].localRotation;
				this.EyeballStartDirection[1] = this.eyeballJoints[1].localRotation;
				this.$PC = -1;
				break;
			}
			return false;
		}

		// Token: 0x170010ED RID: 4333
		// (get) Token: 0x0600739A RID: 29594 RVA: 0x00096BB0 File Offset: 0x00094FB0
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x170010EE RID: 4334
		// (get) Token: 0x0600739B RID: 29595 RVA: 0x00096BB8 File Offset: 0x00094FB8
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x0600739C RID: 29596 RVA: 0x00096BC0 File Offset: 0x00094FC0
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x0600739D RID: 29597 RVA: 0x00096BD0 File Offset: 0x00094FD0
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x04006782 RID: 26498
		internal RandomLook $this;

		// Token: 0x04006783 RID: 26499
		internal object $current;

		// Token: 0x04006784 RID: 26500
		internal bool $disposing;

		// Token: 0x04006785 RID: 26501
		internal int $PC;
	}
}
