using System;
using UnityEngine;

namespace mset
{
	// Token: 0x02000335 RID: 821
	[Serializable]
	public class SkyBlender
	{
		// Token: 0x060013C4 RID: 5060 RVA: 0x00071394 File Offset: 0x0006F794
		public SkyBlender()
		{
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x060013C5 RID: 5061 RVA: 0x000713B2 File Offset: 0x0006F7B2
		// (set) Token: 0x060013C6 RID: 5062 RVA: 0x000713BA File Offset: 0x0006F7BA
		public float BlendTime
		{
			get
			{
				return this.blendTime;
			}
			set
			{
				this.blendTime = value;
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x060013C7 RID: 5063 RVA: 0x000713C3 File Offset: 0x0006F7C3
		// (set) Token: 0x060013C8 RID: 5064 RVA: 0x000713D1 File Offset: 0x0006F7D1
		private float blendTimer
		{
			get
			{
				return this.endStamp - Time.time;
			}
			set
			{
				this.endStamp = Time.time + value;
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x060013C9 RID: 5065 RVA: 0x000713E0 File Offset: 0x0006F7E0
		public float BlendWeight
		{
			get
			{
				return 1f - Mathf.Clamp01(this.blendTimer / this.currentBlendTime);
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x060013CA RID: 5066 RVA: 0x000713FA File Offset: 0x0006F7FA
		public bool IsBlending
		{
			get
			{
				return Time.time < this.endStamp;
			}
		}

		// Token: 0x060013CB RID: 5067 RVA: 0x00071409 File Offset: 0x0006F809
		public bool WasBlending(float secAgo)
		{
			return Time.time - secAgo < this.endStamp;
		}

		// Token: 0x060013CC RID: 5068 RVA: 0x0007141C File Offset: 0x0006F81C
		public void Apply()
		{
			if (this.IsBlending)
			{
				Sky.EnableGlobalProjection(this.CurrentSky.HasDimensions || this.PreviousSky.HasDimensions);
				Sky.EnableGlobalBlending(true);
				this.CurrentSky.Apply(0);
				this.PreviousSky.Apply(1);
				Sky.SetBlendWeight(this.BlendWeight);
			}
			else
			{
				Sky.EnableGlobalProjection(this.CurrentSky.HasDimensions);
				Sky.EnableGlobalBlending(false);
				this.CurrentSky.Apply(0);
			}
		}

		// Token: 0x060013CD RID: 5069 RVA: 0x000714A8 File Offset: 0x0006F8A8
		public void Apply(Material target)
		{
			if (this.IsBlending)
			{
				Sky.EnableBlending(target, true);
				Sky.EnableProjection(target, this.CurrentSky.HasDimensions || this.PreviousSky.HasDimensions);
				this.CurrentSky.Apply(target, 0);
				this.PreviousSky.Apply(target, 1);
				Sky.SetBlendWeight(target, this.BlendWeight);
			}
			else
			{
				Sky.EnableBlending(target, false);
				Sky.EnableProjection(target, this.CurrentSky.HasDimensions);
				this.CurrentSky.Apply(target, 0);
			}
		}

		// Token: 0x060013CE RID: 5070 RVA: 0x0007153C File Offset: 0x0006F93C
		public void Apply(Renderer target, Material[] materials)
		{
			if (this.IsBlending)
			{
				Sky.EnableBlending(target, materials, true);
				Sky.EnableProjection(target, materials, this.CurrentSky.HasDimensions || this.PreviousSky.HasDimensions);
				this.CurrentSky.ApplyFast(target, 0);
				this.PreviousSky.ApplyFast(target, 1);
				Sky.SetBlendWeight(target, this.BlendWeight);
			}
			else
			{
				Sky.EnableBlending(target, materials, false);
				Sky.EnableProjection(target, materials, this.CurrentSky.HasDimensions);
				this.CurrentSky.ApplyFast(target, 0);
			}
		}

		// Token: 0x060013CF RID: 5071 RVA: 0x000715D3 File Offset: 0x0006F9D3
		public void ApplyToTerrain()
		{
			if (this.IsBlending)
			{
				Sky.EnableTerrainBlending(true);
			}
			else
			{
				Sky.EnableTerrainBlending(false);
			}
		}

		// Token: 0x060013D0 RID: 5072 RVA: 0x000715F4 File Offset: 0x0006F9F4
		public void SnapToSky(Sky nusky)
		{
			if (nusky == null)
			{
				return;
			}
			this.PreviousSky = nusky;
			this.CurrentSky = nusky;
			this.blendTimer = 0f;
		}

		// Token: 0x060013D1 RID: 5073 RVA: 0x0007162C File Offset: 0x0006FA2C
		public void BlendToSky(Sky nusky)
		{
			if (nusky == null)
			{
				return;
			}
			if (this.CurrentSky != nusky)
			{
				if (this.CurrentSky == null)
				{
					this.CurrentSky = nusky;
					this.PreviousSky = nusky;
					this.blendTimer = 0f;
				}
				else
				{
					this.PreviousSky = this.CurrentSky;
					this.CurrentSky = nusky;
					this.currentBlendTime = this.blendTime;
					this.blendTimer = this.currentBlendTime;
				}
			}
		}

		// Token: 0x060013D2 RID: 5074 RVA: 0x000716B3 File Offset: 0x0006FAB3
		public void SkipTime(float sec)
		{
			this.blendTimer -= sec;
		}

		// Token: 0x0400111E RID: 4382
		public Sky CurrentSky;

		// Token: 0x0400111F RID: 4383
		public Sky PreviousSky;

		// Token: 0x04001120 RID: 4384
		[SerializeField]
		private float blendTime = 0.25f;

		// Token: 0x04001121 RID: 4385
		private float currentBlendTime = 0.25f;

		// Token: 0x04001122 RID: 4386
		private float endStamp;
	}
}
