using System;
using UnityEngine;

// Token: 0x02000769 RID: 1897
[Serializable]
public class OSPProps
{
	// Token: 0x060030DF RID: 12511 RVA: 0x000FE6D8 File Offset: 0x000FCAD8
	public OSPProps()
	{
		this.enableSpatialization = false;
		this.useFastOverride = false;
		this.gain = 0f;
		this.enableInvSquare = false;
		this.volumetric = 0f;
		this.invSquareFalloff = new Vector2(1f, 25f);
	}

	// Token: 0x040024C3 RID: 9411
	[Tooltip("Set to true to play the sound FX spatialized with binaural HRTF, default = false")]
	public bool enableSpatialization;

	// Token: 0x040024C4 RID: 9412
	[Tooltip("Play the sound FX with reflections, default = false")]
	public bool useFastOverride;

	// Token: 0x040024C5 RID: 9413
	[Tooltip("Boost the gain on the spatialized sound FX, default = 0.0")]
	[Range(0f, 24f)]
	public float gain;

	// Token: 0x040024C6 RID: 9414
	[Tooltip("Enable Inverse Square attenuation curve, default = false")]
	public bool enableInvSquare;

	// Token: 0x040024C7 RID: 9415
	[Tooltip("Change the sound from point source (0.0f) to a spherical volume, default = 0.0")]
	[Range(0f, 1000f)]
	public float volumetric;

	// Token: 0x040024C8 RID: 9416
	[Tooltip("Set the near and far falloff value for the OSP attenuation curve, default = 1.0")]
	[MinMax(1f, 25f, 0f, 250f)]
	public Vector2 invSquareFalloff = new Vector2(1f, 25f);
}
