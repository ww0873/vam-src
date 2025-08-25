using System;
using UnityEngine;

namespace Leap.Unity.Space
{
	// Token: 0x02000719 RID: 1817
	public static class ITransformerExtensions
	{
		// Token: 0x06002C47 RID: 11335 RVA: 0x000EDD38 File Offset: 0x000EC138
		public static void WorldSpaceUnwarp(this ITransformer transformer, Vector3 worldWarpedPosition, Quaternion worldWarpedRotation, out Vector3 worldRectilinearPosition, out Quaternion worldRectilinearRotation)
		{
			Transform transform = transformer.anchor.space.transform;
			Vector3 vector = transform.InverseTransformPoint(worldWarpedPosition);
			Quaternion localWarpedRot = transform.InverseTransformRotation(worldWarpedRotation);
			Vector3 position = transformer.InverseTransformPoint(vector);
			worldRectilinearPosition = transform.TransformPoint(position);
			Quaternion rotation = transformer.InverseTransformRotation(vector, localWarpedRot);
			worldRectilinearRotation = transform.TransformRotation(rotation);
		}
	}
}
