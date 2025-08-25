using System;
using UnityEngine;

namespace MVR
{
	// Token: 0x02000B74 RID: 2932
	public class TransformRotator : MonoBehaviour
	{
		// Token: 0x0600523A RID: 21050 RVA: 0x001DAAA7 File Offset: 0x001D8EA7
		public TransformRotator()
		{
		}

		// Token: 0x0600523B RID: 21051 RVA: 0x001DAABC File Offset: 0x001D8EBC
		private void Update()
		{
			float num = this.speed * Time.deltaTime;
			switch (this.axis)
			{
			case TransformRotator.Axis.X:
				base.transform.Rotate(num, 0f, 0f);
				break;
			case TransformRotator.Axis.NegX:
				base.transform.Rotate(-num, 0f, 0f);
				break;
			case TransformRotator.Axis.Y:
				base.transform.Rotate(0f, num, 0f);
				break;
			case TransformRotator.Axis.NegY:
				base.transform.Rotate(0f, -num, 0f);
				break;
			case TransformRotator.Axis.Z:
				base.transform.Rotate(0f, 0f, num);
				break;
			case TransformRotator.Axis.NegZ:
				base.transform.Rotate(0f, 0f, -num);
				break;
			}
		}

		// Token: 0x040041F6 RID: 16886
		public TransformRotator.Axis axis;

		// Token: 0x040041F7 RID: 16887
		public float speed = 1f;

		// Token: 0x02000B75 RID: 2933
		public enum Axis
		{
			// Token: 0x040041F9 RID: 16889
			X,
			// Token: 0x040041FA RID: 16890
			NegX,
			// Token: 0x040041FB RID: 16891
			Y,
			// Token: 0x040041FC RID: 16892
			NegY,
			// Token: 0x040041FD RID: 16893
			Z,
			// Token: 0x040041FE RID: 16894
			NegZ
		}
	}
}
