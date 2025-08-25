using System;
using GPUTools.Physics.Scripts.Behaviours;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Tools
{
	// Token: 0x02000A7A RID: 2682
	public class BoneCollidersCopyPaster : MonoBehaviour
	{
		// Token: 0x060045A2 RID: 17826 RVA: 0x0013F189 File Offset: 0x0013D589
		public BoneCollidersCopyPaster()
		{
		}

		// Token: 0x060045A3 RID: 17827 RVA: 0x0013F191 File Offset: 0x0013D591
		[ContextMenu("CopyPaste")]
		public void CopyPaste()
		{
			this.CopyPasteRecursive(this.from, this.to);
		}

		// Token: 0x060045A4 RID: 17828 RVA: 0x0013F1A8 File Offset: 0x0013D5A8
		private void CopyPasteRecursive(Transform from, Transform to)
		{
			this.CopyPasteForBone(from, to);
			for (int i = 0; i < from.childCount; i++)
			{
				Transform child = from.GetChild(i);
				Transform child2 = to.GetChild(i);
				this.CopyPasteRecursive(child, child2);
			}
		}

		// Token: 0x060045A5 RID: 17829 RVA: 0x0013F1EC File Offset: 0x0013D5EC
		private void CopyPasteForBone(Transform from, Transform to)
		{
			foreach (LineSphereCollider lineSphereCollider in from.GetComponents<LineSphereCollider>())
			{
				LineSphereCollider lineSphereCollider2 = to.gameObject.AddComponent<LineSphereCollider>();
				lineSphereCollider2.WorldA = lineSphereCollider.WorldA;
				lineSphereCollider2.WorldB = lineSphereCollider.WorldB;
				lineSphereCollider2.WorldRadiusA = lineSphereCollider.WorldRadiusA;
				lineSphereCollider2.WorldRadiusB = lineSphereCollider.WorldRadiusB;
			}
			foreach (SphereCollider sphereCollider in from.GetComponents<SphereCollider>())
			{
				SphereCollider sphereCollider2 = to.gameObject.AddComponent<SphereCollider>();
				sphereCollider2.center = sphereCollider.center;
				sphereCollider2.radius = sphereCollider.radius;
			}
		}

		// Token: 0x04003347 RID: 13127
		[SerializeField]
		private Transform from;

		// Token: 0x04003348 RID: 13128
		[SerializeField]
		private Transform to;
	}
}
