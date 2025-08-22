using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

// Token: 0x02000017 RID: 23
[Serializable]
public class AQUAS_Parameters
{
	// Token: 0x060000BB RID: 187 RVA: 0x000076B4 File Offset: 0x00005AB4
	public AQUAS_Parameters()
	{
	}

	// Token: 0x02000018 RID: 24
	[Serializable]
	public class UnderWaterParameters
	{
		// Token: 0x060000BC RID: 188 RVA: 0x000076BC File Offset: 0x00005ABC
		public UnderWaterParameters()
		{
		}

		// Token: 0x040000E2 RID: 226
		[Header("The following parameters apply for underwater only!")]
		[Space(5f)]
		public float fogDensity = 0.1f;

		// Token: 0x040000E3 RID: 227
		public Color fogColor;

		// Token: 0x040000E4 RID: 228
		[Space(5f)]
		[Header("Post Processing Profiles (Must NOT be empty!)")]
		[Space(5f)]
		public PostProcessingProfile underwaterProfile;

		// Token: 0x040000E5 RID: 229
		public PostProcessingProfile defaultProfile;
	}

	// Token: 0x02000019 RID: 25
	[Serializable]
	public class GameObjects
	{
		// Token: 0x060000BD RID: 189 RVA: 0x000076CF File Offset: 0x00005ACF
		public GameObjects()
		{
		}

		// Token: 0x040000E6 RID: 230
		[Header("Set the game objects required for underwater mode.")]
		[Space(5f)]
		public GameObject mainCamera;

		// Token: 0x040000E7 RID: 231
		public GameObject waterLens;

		// Token: 0x040000E8 RID: 232
		public GameObject airLens;

		// Token: 0x040000E9 RID: 233
		public GameObject bubble;

		// Token: 0x040000EA RID: 234
		[Space(5f)]
		[Header("Set waterplanes array size = number of waterplanes")]
		public List<GameObject> waterPlanes = new List<GameObject>();

		// Token: 0x040000EB RID: 235
		public bool useSquaredPlanes;
	}

	// Token: 0x0200001A RID: 26
	[Serializable]
	public class WetLens
	{
		// Token: 0x060000BE RID: 190 RVA: 0x000076E2 File Offset: 0x00005AE2
		public WetLens()
		{
		}

		// Token: 0x040000EC RID: 236
		[Header("Set how long the lens stays wet after diving up.")]
		public float wetTime = 1f;

		// Token: 0x040000ED RID: 237
		[Space(5f)]
		[Header("Set how long the lens needs to dry.")]
		public float dryingTime = 1.5f;

		// Token: 0x040000EE RID: 238
		[Space(5f)]
		public Texture2D[] sprayFrames;

		// Token: 0x040000EF RID: 239
		public Texture2D[] sprayFramesCutout;

		// Token: 0x040000F0 RID: 240
		public float rundownSpeed = 72f;
	}

	// Token: 0x0200001B RID: 27
	[Serializable]
	public class CausticSettings
	{
		// Token: 0x060000BF RID: 191 RVA: 0x0000770B File Offset: 0x00005B0B
		public CausticSettings()
		{
		}

		// Token: 0x040000F1 RID: 241
		[Header("The following values are 'Afloat'/'Underwater'")]
		public Vector2 causticIntensity = new Vector2(0.6f, 0.2f);

		// Token: 0x040000F2 RID: 242
		public Vector2 causticTiling = new Vector2(300f, 100f);

		// Token: 0x040000F3 RID: 243
		public float maxCausticDepth;
	}

	// Token: 0x0200001C RID: 28
	[Serializable]
	public class Audio
	{
		// Token: 0x060000C0 RID: 192 RVA: 0x0000773D File Offset: 0x00005B3D
		public Audio()
		{
		}

		// Token: 0x040000F4 RID: 244
		public AudioClip[] sounds;

		// Token: 0x040000F5 RID: 245
		[Range(0f, 1f)]
		public float underwaterVolume;

		// Token: 0x040000F6 RID: 246
		[Range(0f, 1f)]
		public float surfacingVolume;

		// Token: 0x040000F7 RID: 247
		[Range(0f, 1f)]
		public float diveVolume;
	}

	// Token: 0x0200001D RID: 29
	[Serializable]
	public class BubbleSpawnCriteria
	{
		// Token: 0x060000C1 RID: 193 RVA: 0x00007748 File Offset: 0x00005B48
		public BubbleSpawnCriteria()
		{
		}

		// Token: 0x040000F8 RID: 248
		[Header("Spawn Criteria for big bubbles")]
		public int minBubbleCount = 20;

		// Token: 0x040000F9 RID: 249
		public int maxBubbleCount = 40;

		// Token: 0x040000FA RID: 250
		[Space(5f)]
		public float maxSpawnDistance = 1f;

		// Token: 0x040000FB RID: 251
		public float averageUpdrift = 3f;

		// Token: 0x040000FC RID: 252
		[Space(5f)]
		public float baseScale = 0.06f;

		// Token: 0x040000FD RID: 253
		public float avgScaleSummand = 0.15f;

		// Token: 0x040000FE RID: 254
		[Space(5f)]
		[Header("Spawn Timer for initial dive")]
		public float minSpawnTimer = 0.005f;

		// Token: 0x040000FF RID: 255
		public float maxSpawnTimer = 0.03f;

		// Token: 0x04000100 RID: 256
		[Space(5f)]
		[Header("Spawn Timer for long dive")]
		public float minSpawnTimerL = 0.1f;

		// Token: 0x04000101 RID: 257
		public float maxSpawnTimerL = 0.5f;
	}
}
