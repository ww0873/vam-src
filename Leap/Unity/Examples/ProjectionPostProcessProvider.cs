using System;
using UnityEngine;

namespace Leap.Unity.Examples
{
	// Token: 0x020005B1 RID: 1457
	public class ProjectionPostProcessProvider : PostProcessProvider
	{
		// Token: 0x0600248F RID: 9359 RVA: 0x000D3919 File Offset: 0x000D1D19
		public ProjectionPostProcessProvider()
		{
		}

		// Token: 0x06002490 RID: 9360 RVA: 0x000D3938 File Offset: 0x000D1D38
		public override void ProcessFrame(ref Frame inputFrame)
		{
			Vector3 position = Camera.main.transform.position;
			Quaternion rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up), Vector3.up);
			foreach (Hand hand in inputFrame.Hands)
			{
				Vector3 vector = position + rotation * (new Vector3(0f, -0.2f, -0.1f) + Vector3.left * 0.1f * ((!hand.IsLeft) ? -1f : 1f));
				Vector3 a = hand.PalmPosition.ToVector3() - vector;
				float magnitude = a.magnitude;
				float num = Mathf.Max(0f, magnitude - this.handMergeDistance);
				float d = Mathf.Pow(1f + num, this.projectionExponent);
				hand.SetTransform(vector + a * d, hand.Rotation.ToQuaternion());
			}
		}

		// Token: 0x04001EC1 RID: 7873
		[Header("Projection")]
		[Tooltip("The exponent of the projection of any hand distance from the approximated shoulder beyond the handMergeDistance.")]
		[Range(0f, 5f)]
		public float projectionExponent = 3.5f;

		// Token: 0x04001EC2 RID: 7874
		[Tooltip("The distance from the approximated shoulder beyond which any additional distance is exponentiated by the projectionExponent.")]
		[Range(0f, 1f)]
		public float handMergeDistance = 0.3f;
	}
}
