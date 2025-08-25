using System;
using System.Collections.Generic;
using UnityEngine;

namespace Technie.PhysicsCreator
{
	// Token: 0x02000455 RID: 1109
	[Serializable]
	public class Hull
	{
		// Token: 0x06001B84 RID: 7044 RVA: 0x0009ABDE File Offset: 0x00098FDE
		public Hull()
		{
		}

		// Token: 0x06001B85 RID: 7045 RVA: 0x0009AC0E File Offset: 0x0009900E
		public void Destroy()
		{
		}

		// Token: 0x04001782 RID: 6018
		public string name = "<unnamed hull>";

		// Token: 0x04001783 RID: 6019
		public HullType type = HullType.ConvexHull;

		// Token: 0x04001784 RID: 6020
		public Color colour = Color.white;

		// Token: 0x04001785 RID: 6021
		public PhysicMaterial material;

		// Token: 0x04001786 RID: 6022
		public bool isTrigger;

		// Token: 0x04001787 RID: 6023
		public List<int> selectedFaces = new List<int>();

		// Token: 0x04001788 RID: 6024
		public Mesh collisionMesh;

		// Token: 0x04001789 RID: 6025
		public Mesh faceCollisionMesh;

		// Token: 0x0400178A RID: 6026
		public Bounds collisionBox;

		// Token: 0x0400178B RID: 6027
		public Sphere collisionSphere;

		// Token: 0x0400178C RID: 6028
		public bool hasColliderError;

		// Token: 0x0400178D RID: 6029
		public int numColliderFaces;
	}
}
