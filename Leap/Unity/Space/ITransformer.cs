using System;
using UnityEngine;

namespace Leap.Unity.Space
{
	// Token: 0x02000717 RID: 1815
	public interface ITransformer
	{
		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x06002C35 RID: 11317
		LeapSpaceAnchor anchor { get; }

		// Token: 0x06002C36 RID: 11318
		Vector3 TransformPoint(Vector3 localRectPos);

		// Token: 0x06002C37 RID: 11319
		Vector3 InverseTransformPoint(Vector3 localWarpedSpace);

		// Token: 0x06002C38 RID: 11320
		Quaternion TransformRotation(Vector3 localRectPos, Quaternion localRectRot);

		// Token: 0x06002C39 RID: 11321
		Quaternion InverseTransformRotation(Vector3 localWarpedPos, Quaternion localWarpedRot);

		// Token: 0x06002C3A RID: 11322
		Vector3 TransformDirection(Vector3 localRectPos, Vector3 localRectDirection);

		// Token: 0x06002C3B RID: 11323
		Vector3 InverseTransformDirection(Vector3 localWarpedSpace, Vector3 localWarpedDirection);

		// Token: 0x06002C3C RID: 11324
		Matrix4x4 GetTransformationMatrix(Vector3 localRectPos);
	}
}
