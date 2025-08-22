using System;
using Leap.Unity;
using UnityEngine;

namespace Leap
{
	// Token: 0x020006F4 RID: 1780
	public static class LeapTestProviderExtensions
	{
		// Token: 0x06002B1E RID: 11038 RVA: 0x000E8E44 File Offset: 0x000E7244
		public static LeapTransform GetLeapTransform(Vector3 position, Quaternion rotation)
		{
			Vector scale = new Vector(LeapTestProviderExtensions.MM_TO_M, LeapTestProviderExtensions.MM_TO_M, LeapTestProviderExtensions.MM_TO_M);
			LeapTransform result = new LeapTransform(position.ToVector(), rotation.ToLeapQuaternion(), scale);
			result.MirrorZ();
			return result;
		}

		// Token: 0x06002B1F RID: 11039 RVA: 0x000E8E83 File Offset: 0x000E7283
		public static void TransformToUnityUnits(this Hand hand)
		{
			hand.Transform(LeapTestProviderExtensions.GetLeapTransform(Vector3.zero, Quaternion.identity));
		}

		// Token: 0x06002B20 RID: 11040 RVA: 0x000E8E9B File Offset: 0x000E729B
		// Note: this type is marked as 'beforefieldinit'.
		static LeapTestProviderExtensions()
		{
		}

		// Token: 0x040022E1 RID: 8929
		public static readonly float MM_TO_M = 0.001f;
	}
}
