using System;
using UnityEngine;

namespace Battlehub.UIControls
{
	// Token: 0x02000267 RID: 615
	public class Rotation : MonoBehaviour
	{
		// Token: 0x06000D1B RID: 3355 RVA: 0x0004E640 File Offset: 0x0004CA40
		public Rotation()
		{
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x0004E648 File Offset: 0x0004CA48
		private void Start()
		{
			this.m_rand = UnityEngine.Random.onUnitSphere;
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x0004E658 File Offset: 0x0004CA58
		private void Update()
		{
			if (Time.time - this.m_prevT > 10f)
			{
				this.m_rand = UnityEngine.Random.onUnitSphere;
				this.m_prevT = Time.time;
			}
			base.transform.rotation *= Quaternion.AngleAxis(12.566371f * Time.deltaTime, this.m_rand);
		}

		// Token: 0x04000D21 RID: 3361
		private Vector3 m_rand;

		// Token: 0x04000D22 RID: 3362
		private float m_prevT;
	}
}
