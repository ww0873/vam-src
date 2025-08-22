using System;
using UnityEngine;

namespace Leap.Unity.RuntimeGizmos
{
	// Token: 0x02000741 RID: 1857
	public class RuntimeColliderGizmos : MonoBehaviour, IRuntimeGizmoComponent
	{
		// Token: 0x06002D54 RID: 11604 RVA: 0x000F17C9 File Offset: 0x000EFBC9
		public RuntimeColliderGizmos()
		{
		}

		// Token: 0x06002D55 RID: 11605 RVA: 0x000F17EA File Offset: 0x000EFBEA
		private void Start()
		{
		}

		// Token: 0x06002D56 RID: 11606 RVA: 0x000F17EC File Offset: 0x000EFBEC
		public void OnDrawRuntimeGizmos(RuntimeGizmoDrawer drawer)
		{
			if (!base.gameObject.activeInHierarchy || !base.enabled)
			{
				return;
			}
			drawer.color = this.color;
			drawer.DrawColliders(base.gameObject, this.useWireframe, this.traverseHierarchy, this.drawTriggers);
		}

		// Token: 0x040023D3 RID: 9171
		public Color color = Color.white;

		// Token: 0x040023D4 RID: 9172
		public bool useWireframe = true;

		// Token: 0x040023D5 RID: 9173
		public bool traverseHierarchy = true;

		// Token: 0x040023D6 RID: 9174
		public bool drawTriggers;
	}
}
