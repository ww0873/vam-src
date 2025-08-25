using System;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Behaviours
{
	// Token: 0x02000A46 RID: 2630
	public class PlaneCollider : MonoBehaviour
	{
		// Token: 0x060043B7 RID: 17335 RVA: 0x0013D24A File Offset: 0x0013B64A
		public PlaneCollider()
		{
		}

		// Token: 0x060043B8 RID: 17336 RVA: 0x0013D254 File Offset: 0x0013B654
		public Vector4 GetWorldData()
		{
			Vector3 up = base.transform.up;
			Vector3 position = base.transform.position;
			float num = position.x * up.x + position.y * up.y + position.z * up.z;
			return new Vector4(up.x, up.y, up.z, -num);
		}

		// Token: 0x060043B9 RID: 17337 RVA: 0x0013D2C5 File Offset: 0x0013B6C5
		private void OnEnable()
		{
			if (Application.isPlaying)
			{
				GPUCollidersManager.RegisterPlaneCollider(this);
			}
		}

		// Token: 0x060043BA RID: 17338 RVA: 0x0013D2D7 File Offset: 0x0013B6D7
		private void OnDisable()
		{
			if (Application.isPlaying)
			{
				GPUCollidersManager.DeregisterPlaneCollider(this);
			}
		}
	}
}
