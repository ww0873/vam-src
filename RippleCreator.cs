using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200030C RID: 780
public class RippleCreator : MonoBehaviour
{
	// Token: 0x06001265 RID: 4709 RVA: 0x0006682C File Offset: 0x00064C2C
	public RippleCreator()
	{
	}

	// Token: 0x06001266 RID: 4710 RVA: 0x0006687A File Offset: 0x00064C7A
	private void Start()
	{
		this.t = base.transform;
		this.reversedVelocityQueue = new Queue<RippleCreator.ReversedRipple>();
	}

	// Token: 0x06001267 RID: 4711 RVA: 0x00066894 File Offset: 0x00064C94
	private void FixedUpdate()
	{
		if (this.waterRipples == null)
		{
			return;
		}
		if (this.RandomRipplesInterval > 0.0001f && Time.time - this.randomRipplesCurrentTime > this.RandomRipplesInterval)
		{
			this.randomRipplesCurrentTime = Time.time;
			this.canInstantiateRandomRipple = true;
		}
		if (this.canUpdate)
		{
			this.currentVelocity = (this.t.position - this.oldPos).magnitude / Time.fixedDeltaTime * this.RippleStrenght;
			if (this.currentVelocity > this.MaxVelocity)
			{
				this.currentVelocity = this.MaxVelocity;
			}
			if (this.IsReversedRipple)
			{
				this.currentVelocity = -this.currentVelocity;
			}
			this.reversedVelocityQueue.Enqueue(new RippleCreator.ReversedRipple
			{
				Position = this.t.position,
				Velocity = -this.currentVelocity / (float)this.fadeInVelocity
			});
			this.oldPos = this.t.position;
			this.waterRipples.CreateRippleByPosition(this.t.position, this.currentVelocity / (float)this.fadeInVelocity);
			if (this.canInstantiateRandomRipple)
			{
				this.waterRipples.CreateRippleByPosition(this.t.position, UnityEngine.Random.Range(this.currentVelocity / 5f, this.currentVelocity));
			}
			this.UpdateMovedSplash();
		}
		if (Time.time - this.triggeredTime > this.reversedRippleDelay)
		{
			RippleCreator.ReversedRipple reversedRipple = this.reversedVelocityQueue.Dequeue();
			if (this.IsReversedRipple)
			{
				reversedRipple.Velocity = -reversedRipple.Velocity;
			}
			this.waterRipples.CreateRippleByPosition(reversedRipple.Position, reversedRipple.Velocity);
			if (this.canInstantiateRandomRipple)
			{
				this.waterRipples.CreateRippleByPosition(reversedRipple.Position, UnityEngine.Random.Range(reversedRipple.Velocity / 5f, reversedRipple.Velocity));
			}
		}
		this.fadeInVelocity++;
		if (this.fadeInVelocity > this.fadeInVelocityLimit)
		{
			this.fadeInVelocity = 1;
		}
		if (this.canInstantiateRandomRipple)
		{
			this.canInstantiateRandomRipple = false;
		}
	}

	// Token: 0x06001268 RID: 4712 RVA: 0x00066AC8 File Offset: 0x00064EC8
	private void OnTriggerEnter(Collider collidedObj)
	{
		WaterRipples component = collidedObj.GetComponent<WaterRipples>();
		if (component != null)
		{
			this.waterRipples = component;
			this.canUpdate = true;
			this.reversedVelocityQueue.Clear();
			this.triggeredTime = Time.time;
			this.fadeInVelocity = 1;
			if (this.SplashAudioSource != null)
			{
				this.SplashAudioSource.Play();
			}
			if (this.SplashEffect != null)
			{
				Vector3 offsetByPosition = this.waterRipples.GetOffsetByPosition(this.t.position);
				offsetByPosition.x = this.t.position.x;
				offsetByPosition.z = this.t.position.z;
				GameObject obj = UnityEngine.Object.Instantiate<GameObject>(this.SplashEffect, offsetByPosition, default(Quaternion));
				UnityEngine.Object.Destroy(obj, 2f);
			}
			this.UpdateMovedSplash();
			return;
		}
	}

	// Token: 0x06001269 RID: 4713 RVA: 0x00066BBC File Offset: 0x00064FBC
	private void UpdateMovedSplash()
	{
		if (this.splashMovedInstance != null)
		{
			Vector3 offsetByPosition = this.waterRipples.GetOffsetByPosition(this.t.position);
			offsetByPosition.x = this.t.position.x;
			offsetByPosition.z = this.t.position.z;
			this.splashMovedInstance.transform.position = offsetByPosition;
			this.splashParticleSystem.main.startSize = this.currentVelocity * this.splashSizeMultiplier;
		}
		else if (this.SplashEffectMoved != null)
		{
			this.splashMovedInstance = UnityEngine.Object.Instantiate<GameObject>(this.SplashEffectMoved, this.t.position, default(Quaternion));
			this.splashMovedInstance.transform.parent = this.waterRipples.transform;
			Vector3 offsetByPosition2 = this.waterRipples.GetOffsetByPosition(this.t.position);
			offsetByPosition2.x = this.t.position.x;
			offsetByPosition2.z = this.t.position.z;
			this.splashMovedInstance.transform.position = offsetByPosition2;
			this.splashParticleSystem = this.splashMovedInstance.GetComponentInChildren<ParticleSystem>();
			this.splashParticleSystem.main.startSize = this.currentVelocity * this.splashSizeMultiplier;
		}
	}

	// Token: 0x0600126A RID: 4714 RVA: 0x00066D49 File Offset: 0x00065149
	private void OnEnable()
	{
		this.waterRipples = null;
		this.canUpdate = false;
		if (this.splashMovedInstance != null)
		{
			UnityEngine.Object.Destroy(this.splashMovedInstance);
		}
	}

	// Token: 0x0600126B RID: 4715 RVA: 0x00066D75 File Offset: 0x00065175
	private void OnDisable()
	{
		if (this.splashMovedInstance != null)
		{
			UnityEngine.Object.Destroy(this.splashMovedInstance);
		}
	}

	// Token: 0x0600126C RID: 4716 RVA: 0x00066D93 File Offset: 0x00065193
	private void OnDestroy()
	{
		if (this.splashMovedInstance != null)
		{
			UnityEngine.Object.Destroy(this.splashMovedInstance);
		}
	}

	// Token: 0x04000FC2 RID: 4034
	public bool IsReversedRipple;

	// Token: 0x04000FC3 RID: 4035
	public float RippleStrenght = 0.1f;

	// Token: 0x04000FC4 RID: 4036
	public float MaxVelocity = 1.5f;

	// Token: 0x04000FC5 RID: 4037
	public float RandomRipplesInterval;

	// Token: 0x04000FC6 RID: 4038
	public float reversedRippleDelay = 0.2f;

	// Token: 0x04000FC7 RID: 4039
	public GameObject SplashEffect;

	// Token: 0x04000FC8 RID: 4040
	public GameObject SplashEffectMoved;

	// Token: 0x04000FC9 RID: 4041
	public AudioSource SplashAudioSource;

	// Token: 0x04000FCA RID: 4042
	private int fadeInVelocityLimit = 10;

	// Token: 0x04000FCB RID: 4043
	private int fadeInVelocity = 1;

	// Token: 0x04000FCC RID: 4044
	private WaterRipples waterRipples;

	// Token: 0x04000FCD RID: 4045
	private Vector3 oldPos;

	// Token: 0x04000FCE RID: 4046
	private float currentVelocity;

	// Token: 0x04000FCF RID: 4047
	private Transform t;

	// Token: 0x04000FD0 RID: 4048
	private Queue<RippleCreator.ReversedRipple> reversedVelocityQueue;

	// Token: 0x04000FD1 RID: 4049
	private float triggeredTime;

	// Token: 0x04000FD2 RID: 4050
	private bool canUpdate;

	// Token: 0x04000FD3 RID: 4051
	private float randomRipplesCurrentTime;

	// Token: 0x04000FD4 RID: 4052
	private bool canInstantiateRandomRipple;

	// Token: 0x04000FD5 RID: 4053
	private GameObject splashMovedInstance;

	// Token: 0x04000FD6 RID: 4054
	private ParticleSystem splashParticleSystem;

	// Token: 0x04000FD7 RID: 4055
	public float splashSizeMultiplier = 2f;

	// Token: 0x0200030D RID: 781
	private class ReversedRipple
	{
		// Token: 0x0600126D RID: 4717 RVA: 0x00066DB1 File Offset: 0x000651B1
		public ReversedRipple()
		{
		}

		// Token: 0x04000FD8 RID: 4056
		public Vector3 Position;

		// Token: 0x04000FD9 RID: 4057
		public float Velocity;
	}
}
