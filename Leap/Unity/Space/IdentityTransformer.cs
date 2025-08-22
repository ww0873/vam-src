using System;
using UnityEngine;

namespace Leap.Unity.Space
{
	// Token: 0x02000718 RID: 1816
	public class IdentityTransformer : ITransformer
	{
		// Token: 0x06002C3D RID: 11325 RVA: 0x000EDCFA File Offset: 0x000EC0FA
		public IdentityTransformer()
		{
		}

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x06002C3E RID: 11326 RVA: 0x000EDD02 File Offset: 0x000EC102
		public LeapSpaceAnchor anchor
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06002C3F RID: 11327 RVA: 0x000EDD05 File Offset: 0x000EC105
		public Vector3 TransformPoint(Vector3 localRectPos)
		{
			return localRectPos;
		}

		// Token: 0x06002C40 RID: 11328 RVA: 0x000EDD08 File Offset: 0x000EC108
		public Vector3 InverseTransformPoint(Vector3 localWarpedSpace)
		{
			return localWarpedSpace;
		}

		// Token: 0x06002C41 RID: 11329 RVA: 0x000EDD0B File Offset: 0x000EC10B
		public Quaternion TransformRotation(Vector3 localRectPos, Quaternion localRectRot)
		{
			return localRectRot;
		}

		// Token: 0x06002C42 RID: 11330 RVA: 0x000EDD0E File Offset: 0x000EC10E
		public Quaternion InverseTransformRotation(Vector3 localWarpedPos, Quaternion localWarpedRot)
		{
			return localWarpedRot;
		}

		// Token: 0x06002C43 RID: 11331 RVA: 0x000EDD11 File Offset: 0x000EC111
		public Vector3 TransformDirection(Vector3 localRectPos, Vector3 localRectDirection)
		{
			return localRectDirection;
		}

		// Token: 0x06002C44 RID: 11332 RVA: 0x000EDD14 File Offset: 0x000EC114
		public Vector3 InverseTransformDirection(Vector3 localWarpedSpace, Vector3 localWarpedDirection)
		{
			return localWarpedDirection;
		}

		// Token: 0x06002C45 RID: 11333 RVA: 0x000EDD17 File Offset: 0x000EC117
		public Matrix4x4 GetTransformationMatrix(Vector3 localRectPos)
		{
			return Matrix4x4.TRS(localRectPos, Quaternion.identity, Vector3.one);
		}

		// Token: 0x06002C46 RID: 11334 RVA: 0x000EDD29 File Offset: 0x000EC129
		// Note: this type is marked as 'beforefieldinit'.
		static IdentityTransformer()
		{
		}

		// Token: 0x0400236D RID: 9069
		public static readonly IdentityTransformer single = new IdentityTransformer();
	}
}
