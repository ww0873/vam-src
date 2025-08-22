using System;
using GPUTools.Common.Scripts.Tools.Ranges;
using GPUTools.Hair.Scripts.Settings.Abstract;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Settings
{
	// Token: 0x02000A28 RID: 2600
	[Serializable]
	public class HairLODSettings : HairSettingsBase
	{
		// Token: 0x0600432D RID: 17197 RVA: 0x0013B668 File Offset: 0x00139A68
		public HairLODSettings()
		{
		}

		// Token: 0x0600432E RID: 17198 RVA: 0x0013B6EA File Offset: 0x00139AEA
		public float GetWidth(Vector3 position)
		{
			if (this.UseFixedSettings)
			{
				return this.FixedWidth;
			}
			return this.Width.GetLerp(this.GetDistanceK(position));
		}

		// Token: 0x0600432F RID: 17199 RVA: 0x0013B710 File Offset: 0x00139B10
		public int GetDencity(Vector3 position)
		{
			if (this.UseFixedSettings)
			{
				return this.FixedDensity;
			}
			return (int)this.Density.GetLerp(1f - this.GetDistanceK(position));
		}

		// Token: 0x06004330 RID: 17200 RVA: 0x0013B73D File Offset: 0x00139B3D
		public int GetDetail(Vector3 position)
		{
			if (this.UseFixedSettings)
			{
				return this.FixedDetail;
			}
			return (int)this.Detail.GetLerp(1f - this.GetDistanceK(position));
		}

		// Token: 0x06004331 RID: 17201 RVA: 0x0013B76C File Offset: 0x00139B6C
		public float GetDistanceK(Vector3 position)
		{
			float value = (this.GetDistanceToCamera(position) - this.Distance.Min) / (this.Distance.Max - this.Distance.Min);
			return Mathf.Clamp01(value);
		}

		// Token: 0x06004332 RID: 17202 RVA: 0x0013B7AC File Offset: 0x00139BAC
		public float GetDistanceToCamera(Vector3 position)
		{
			if (this.ViewCamera != null)
			{
				return (position - this.ViewCamera.transform.position).magnitude;
			}
			if (Camera.main != null)
			{
				return (position - Camera.main.transform.position).magnitude;
			}
			return 0f;
		}

		// Token: 0x06004333 RID: 17203 RVA: 0x0013B81C File Offset: 0x00139C1C
		public bool IsPhysicsEnabled(Vector3 position)
		{
			return this.GetDistanceToCamera(position) < this.Distance.Max;
		}

		// Token: 0x040031DF RID: 12767
		public Camera ViewCamera;

		// Token: 0x040031E0 RID: 12768
		public bool UseFixedSettings;

		// Token: 0x040031E1 RID: 12769
		public int FixedDensity = 24;

		// Token: 0x040031E2 RID: 12770
		public int FixedDetail = 24;

		// Token: 0x040031E3 RID: 12771
		public float FixedWidth = 0.0001f;

		// Token: 0x040031E4 RID: 12772
		public FloatRange Distance = new FloatRange(0f, 5f);

		// Token: 0x040031E5 RID: 12773
		public FloatRange Density = new FloatRange(4f, 8f);

		// Token: 0x040031E6 RID: 12774
		public FloatRange Detail = new FloatRange(4f, 16f);

		// Token: 0x040031E7 RID: 12775
		public FloatRange Width = new FloatRange(0.0004f, 0.002f);
	}
}
