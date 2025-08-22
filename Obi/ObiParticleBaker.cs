using System;
using UnityEngine;

namespace Obi
{
	// Token: 0x020003F4 RID: 1012
	[ExecuteInEditMode]
	[RequireComponent(typeof(ObiSolver))]
	public class ObiParticleBaker : MonoBehaviour
	{
		// Token: 0x060019BB RID: 6587 RVA: 0x0008ED33 File Offset: 0x0008D133
		public ObiParticleBaker()
		{
		}

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x060019BC RID: 6588 RVA: 0x0008ED63 File Offset: 0x0008D163
		// (set) Token: 0x060019BD RID: 6589 RVA: 0x0008ED6C File Offset: 0x0008D16C
		public bool Baking
		{
			get
			{
				return this.baking;
			}
			set
			{
				this.baking = value;
				if (this.baking)
				{
					Time.captureFramerate = Mathf.Max(0, this.fixedBakeFramerate);
					this.playing = false;
					this.solver.simulate = true;
					this.solver.RequireRenderablePositions();
				}
				else
				{
					this.framesToSkip = 0;
					Time.captureFramerate = 0;
					this.solver.RelinquishRenderablePositions();
				}
			}
		}

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x060019BE RID: 6590 RVA: 0x0008EDD7 File Offset: 0x0008D1D7
		// (set) Token: 0x060019BF RID: 6591 RVA: 0x0008EDDF File Offset: 0x0008D1DF
		public bool Playing
		{
			get
			{
				return this.playing;
			}
			set
			{
				this.playing = value;
				this.solver.simulate = !this.playing;
				if (this.playing)
				{
					this.baking = false;
				}
			}
		}

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x060019C0 RID: 6592 RVA: 0x0008EE0E File Offset: 0x0008D20E
		// (set) Token: 0x060019C1 RID: 6593 RVA: 0x0008EE16 File Offset: 0x0008D216
		public bool Paused
		{
			get
			{
				return this.paused;
			}
			set
			{
				this.paused = value;
			}
		}

		// Token: 0x060019C2 RID: 6594 RVA: 0x0008EE20 File Offset: 0x0008D220
		private void Awake()
		{
			this.solver = base.GetComponent<ObiSolver>();
			if (Application.isPlaying)
			{
				if (this.bakeOnAwake)
				{
					this.playhead = 0f;
					this.Baking = true;
				}
				else if (this.playOnAwake)
				{
					this.playhead = 0f;
					this.Playing = true;
				}
			}
		}

		// Token: 0x060019C3 RID: 6595 RVA: 0x0008EE82 File Offset: 0x0008D282
		private void OnEnable()
		{
			this.solver.OnFrameEnd += this.Solver_OnFrameEnd;
			this.solver.OnBeforeActorsFrameEnd += this.Solver_OnBeforeActorsFrameEnd;
		}

		// Token: 0x060019C4 RID: 6596 RVA: 0x0008EEB2 File Offset: 0x0008D2B2
		private void OnDisable()
		{
			this.Baking = false;
			this.solver.OnFrameEnd -= this.Solver_OnFrameEnd;
			this.solver.OnBeforeActorsFrameEnd -= this.Solver_OnBeforeActorsFrameEnd;
		}

		// Token: 0x060019C5 RID: 6597 RVA: 0x0008EEEC File Offset: 0x0008D2EC
		private void Solver_OnFrameEnd(object sender, EventArgs e)
		{
			if (this.cache != null && !this.playing && this.baking)
			{
				this.playhead += Time.deltaTime;
				if (this.framesToSkip <= 0)
				{
					this.BakeFrame(this.playhead);
					this.framesToSkip = this.frameSkip;
				}
				else
				{
					this.framesToSkip--;
				}
			}
		}

		// Token: 0x060019C6 RID: 6598 RVA: 0x0008EF6C File Offset: 0x0008D36C
		private void Solver_OnBeforeActorsFrameEnd(object sender, EventArgs e)
		{
			if (this.cache != null && this.playing)
			{
				if (!this.paused)
				{
					this.playhead += Time.deltaTime;
					if (this.loopPlayback)
					{
						this.playhead = ((this.cache.Duration != 0f) ? (this.playhead % this.cache.Duration) : 0f);
					}
					else if (this.playhead > this.cache.Duration)
					{
						this.playhead = this.cache.Duration;
					}
				}
				this.PlaybackFrame(this.playhead);
			}
		}

		// Token: 0x060019C7 RID: 6599 RVA: 0x0008F02C File Offset: 0x0008D42C
		public void BakeFrame(float time)
		{
			if (this.cache == null)
			{
				return;
			}
			ObiParticleCache.Frame frame = new ObiParticleCache.Frame();
			frame.time = time;
			for (int i = 0; i < this.solver.renderablePositions.Length; i++)
			{
				ObiSolver.ParticleInActor particleInActor = this.solver.particleToActor[i];
				if (particleInActor != null && particleInActor.actor.active[particleInActor.indexInActor])
				{
					frame.indices.Add(i);
					if (this.cache.localSpace)
					{
						frame.positions.Add(this.solver.transform.InverseTransformPoint(this.solver.renderablePositions[i]));
					}
					else
					{
						frame.positions.Add(this.solver.renderablePositions[i]);
					}
				}
			}
			this.cache.AddFrame(frame);
		}

		// Token: 0x060019C8 RID: 6600 RVA: 0x0008F130 File Offset: 0x0008D530
		private void PlaybackFrame(float time)
		{
			if (this.cache == null || this.cache.Duration == 0f)
			{
				return;
			}
			this.cache.GetFrame(time, this.interpolate, ref this.frame);
			if (this.solver.AllocParticleCount < this.frame.indices.Count)
			{
				Debug.LogError("The ObiSolver doesn't have enough allocated particles to playback this cache.");
				this.Playing = false;
				return;
			}
			Matrix4x4 matrix4x = (!this.cache.localSpace) ? Matrix4x4.identity : this.solver.transform.localToWorldMatrix;
			for (int i = 0; i < this.frame.indices.Count; i++)
			{
				if (this.frame.indices[i] >= 0 && this.frame.indices[i] < this.solver.renderablePositions.Length)
				{
					this.solver.renderablePositions[this.frame.indices[i]] = matrix4x.MultiplyPoint3x4(this.frame.positions[i]);
				}
			}
			Oni.SetParticlePositions(this.solver.OniSolver, this.solver.renderablePositions, this.solver.renderablePositions.Length, 0);
			this.solver.UpdateActiveParticles();
		}

		// Token: 0x040014EE RID: 5358
		public ObiParticleCache cache;

		// Token: 0x040014EF RID: 5359
		public float playhead;

		// Token: 0x040014F0 RID: 5360
		public int frameSkip = 8;

		// Token: 0x040014F1 RID: 5361
		public int fixedBakeFramerate = 60;

		// Token: 0x040014F2 RID: 5362
		public bool interpolate = true;

		// Token: 0x040014F3 RID: 5363
		public bool loopPlayback = true;

		// Token: 0x040014F4 RID: 5364
		public bool bakeOnAwake;

		// Token: 0x040014F5 RID: 5365
		public bool playOnAwake;

		// Token: 0x040014F6 RID: 5366
		private bool baking;

		// Token: 0x040014F7 RID: 5367
		private bool playing;

		// Token: 0x040014F8 RID: 5368
		private bool paused;

		// Token: 0x040014F9 RID: 5369
		private int framesToSkip;

		// Token: 0x040014FA RID: 5370
		private ObiSolver solver;

		// Token: 0x040014FB RID: 5371
		private ObiParticleCache.Frame frame = new ObiParticleCache.Frame();
	}
}
