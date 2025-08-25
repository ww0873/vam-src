using System;
using UnityEngine;

namespace Leap.Unity.Space
{
	// Token: 0x02000712 RID: 1810
	public interface IRadialTransformer : ITransformer
	{
		// Token: 0x06002C1C RID: 11292
		Vector4 GetVectorRepresentation(Transform element);
	}
}
