using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Wind
{
	// Token: 0x02000A87 RID: 2695
	public class WindReceiver
	{
		// Token: 0x060045CB RID: 17867 RVA: 0x0013FB9C File Offset: 0x0013DF9C
		public WindReceiver()
		{
			this.winds = UnityEngine.Object.FindObjectsOfType<WindZone>();
			this.octaves.Add(new NoiseOctave(1f, 1f));
			this.octaves.Add(new NoiseOctave(5f, 0.6f));
			this.octaves.Add(new NoiseOctave(10f, 0.4f));
			this.octaves.Add(new NoiseOctave(20f, 0.3f));
		}

		// Token: 0x1700096E RID: 2414
		// (get) Token: 0x060045CC RID: 17868 RVA: 0x0013FC3D File Offset: 0x0013E03D
		// (set) Token: 0x060045CD RID: 17869 RVA: 0x0013FC45 File Offset: 0x0013E045
		public Vector3 Vector
		{
			[CompilerGenerated]
			get
			{
				return this.<Vector>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Vector>k__BackingField = value;
			}
		}

		// Token: 0x060045CE RID: 17870 RVA: 0x0013FC50 File Offset: 0x0013E050
		public Vector3 GetWind(Vector3 position)
		{
			this.Vector = Vector3.zero;
			foreach (WindZone windZone in this.winds)
			{
				if (windZone.mode == WindZoneMode.Directional)
				{
					this.UpdateDirectionalWind(windZone);
				}
				else
				{
					this.UpdateSphericalWind(windZone, position);
				}
			}
			return this.Vector;
		}

		// Token: 0x060045CF RID: 17871 RVA: 0x0013FCAC File Offset: 0x0013E0AC
		private void UpdateDirectionalWind(WindZone wind)
		{
			Vector3 dirrection = wind.transform.rotation * Vector3.forward;
			this.Vector += this.GetAmplitude(wind, dirrection);
		}

		// Token: 0x060045D0 RID: 17872 RVA: 0x0013FCE8 File Offset: 0x0013E0E8
		private void UpdateSphericalWind(WindZone wind, Vector3 center)
		{
			Vector3 vector = center - wind.transform.position;
			if (vector.magnitude > wind.radius)
			{
				return;
			}
			this.Vector += this.GetAmplitude(wind, vector.normalized);
		}

		// Token: 0x060045D1 RID: 17873 RVA: 0x0013FD3C File Offset: 0x0013E13C
		private Vector3 GetAmplitude(WindZone wind, Vector3 dirrection)
		{
			this.angle += wind.windPulseFrequency;
			float noise = this.GetNoise(this.angle);
			float d = wind.windMain + noise * wind.windPulseMagnitude;
			return dirrection * d;
		}

		// Token: 0x060045D2 RID: 17874 RVA: 0x0013FD80 File Offset: 0x0013E180
		private float GetNoise(float angle)
		{
			return Mathf.Abs(this.perlin.Noise(new Vector2(0f, angle), this.octaves));
		}

		// Token: 0x04003386 RID: 13190
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Vector3 <Vector>k__BackingField;

		// Token: 0x04003387 RID: 13191
		private float angle;

		// Token: 0x04003388 RID: 13192
		private readonly WindZone[] winds;

		// Token: 0x04003389 RID: 13193
		private readonly Perlin2D perlin = new Perlin2D(566);

		// Token: 0x0400338A RID: 13194
		private readonly List<NoiseOctave> octaves = new List<NoiseOctave>();
	}
}
