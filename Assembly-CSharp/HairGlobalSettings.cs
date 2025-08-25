using System;
using UnityEngine;

// Token: 0x02000C91 RID: 3217
public class HairGlobalSettings
{
	// Token: 0x06006102 RID: 24834 RVA: 0x0024ACEC File Offset: 0x002490EC
	public HairGlobalSettings()
	{
	}

	// Token: 0x0400509C RID: 20636
	public HairStripV2.HairDrawType hairDrawType = HairStripV2.HairDrawType.GPULines;

	// Token: 0x0400509D RID: 20637
	public int numberSegments = 10;

	// Token: 0x0400509E RID: 20638
	public float invNumberSegments;

	// Token: 0x0400509F RID: 20639
	public int numHairsMin = 1;

	// Token: 0x040050A0 RID: 20640
	public int numHairsMax = 1;

	// Token: 0x040050A1 RID: 20641
	public HairStripV2.HairBundleType bundleType = HairStripV2.HairBundleType.Circular;

	// Token: 0x040050A2 RID: 20642
	public float subHairXOffsetMax = 0.01f;

	// Token: 0x040050A3 RID: 20643
	public float subHairYOffsetMax = 0.01f;

	// Token: 0x040050A4 RID: 20644
	public float subHairZOffsetBend;

	// Token: 0x040050A5 RID: 20645
	public Collider[] colliders;

	// Token: 0x040050A6 RID: 20646
	public bool useExtendedColliders;

	// Token: 0x040050A7 RID: 20647
	public ExtendedCapsuleCollider[] extendedColliders;

	// Token: 0x040050A8 RID: 20648
	public Vector3[] colliderCenters;

	// Token: 0x040050A9 RID: 20649
	public bool createTangents;

	// Token: 0x040050AA RID: 20650
	public bool ownMesh = true;

	// Token: 0x040050AB RID: 20651
	public float scale = 1f;

	// Token: 0x040050AC RID: 20652
	public float oneoverscale = 1f;

	// Token: 0x040050AD RID: 20653
	public float deltaTime;

	// Token: 0x040050AE RID: 20654
	public float deltaTimeSqr;

	// Token: 0x040050AF RID: 20655
	public float invDeltaTime;

	// Token: 0x040050B0 RID: 20656
	public bool drawFromAnchor = true;

	// Token: 0x040050B1 RID: 20657
	public float hairLength = 0.15f;

	// Token: 0x040050B2 RID: 20658
	public float segmentLength;

	// Token: 0x040050B3 RID: 20659
	public float quarterSegmentLength;

	// Token: 0x040050B4 RID: 20660
	public float hairWidth = 0.0005f;

	// Token: 0x040050B5 RID: 20661
	public float hairHalfWidth;

	// Token: 0x040050B6 RID: 20662
	public bool roundSheetHairs = true;

	// Token: 0x040050B7 RID: 20663
	public float sheetHairRoundness = 0.5f;

	// Token: 0x040050B8 RID: 20664
	public Material hairMaterial;

	// Token: 0x040050B9 RID: 20665
	public Vector3 gravityForce;

	// Token: 0x040050BA RID: 20666
	public bool staticFriction;

	// Token: 0x040050BB RID: 20667
	public float staticMoveDistance = 0.001f;

	// Token: 0x040050BC RID: 20668
	public float staticMoveDistanceSqr;

	// Token: 0x040050BD RID: 20669
	public float velocityFactor = 0.98f;

	// Token: 0x040050BE RID: 20670
	public float stiffnessRoot;

	// Token: 0x040050BF RID: 20671
	public float stiffnessEnd;

	// Token: 0x040050C0 RID: 20672
	public float stiffnessVariance;

	// Token: 0x040050C1 RID: 20673
	public bool enableCollision = true;

	// Token: 0x040050C2 RID: 20674
	public bool enableSimulation = true;

	// Token: 0x040050C3 RID: 20675
	public float slowCollidingPoints = 0.5f;

	// Token: 0x040050C4 RID: 20676
	public float dampenFactor = 0.9f;

	// Token: 0x040050C5 RID: 20677
	public float invdtdampen;

	// Token: 0x040050C6 RID: 20678
	public bool clampAcceleration = true;

	// Token: 0x040050C7 RID: 20679
	public bool clampVelocity = true;

	// Token: 0x040050C8 RID: 20680
	public float accelerationClamp = 0.015f;

	// Token: 0x040050C9 RID: 20681
	public float velocityClamp = 0.1f;

	// Token: 0x040050CA RID: 20682
	public bool castShadows = true;

	// Token: 0x040050CB RID: 20683
	public bool receiveShadows = true;

	// Token: 0x040050CC RID: 20684
	public float debugWidth = 0.005f;
}
