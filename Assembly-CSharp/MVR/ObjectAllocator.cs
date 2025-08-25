using System;
using System.Collections.Generic;
using UnityEngine;

namespace MVR
{
	// Token: 0x02000C3D RID: 3133
	public class ObjectAllocator : MonoBehaviour
	{
		// Token: 0x06005B3B RID: 23355 RVA: 0x0017D687 File Offset: 0x0017BA87
		public ObjectAllocator()
		{
		}

		// Token: 0x06005B3C RID: 23356 RVA: 0x0017D68F File Offset: 0x0017BA8F
		protected void RegisterAllocatedObject(UnityEngine.Object o)
		{
			if (Application.isPlaying)
			{
				if (this.allocatedObjects == null)
				{
					this.allocatedObjects = new List<UnityEngine.Object>();
				}
				this.allocatedObjects.Add(o);
			}
		}

		// Token: 0x06005B3D RID: 23357 RVA: 0x0017D6C0 File Offset: 0x0017BAC0
		protected void DestroyAllocatedObjects()
		{
			if (Application.isPlaying && this.allocatedObjects != null)
			{
				foreach (UnityEngine.Object obj in this.allocatedObjects)
				{
					UnityEngine.Object.Destroy(obj);
				}
			}
		}

		// Token: 0x06005B3E RID: 23358 RVA: 0x0017D730 File Offset: 0x0017BB30
		protected virtual void OnDestroy()
		{
			this.DestroyAllocatedObjects();
		}

		// Token: 0x04004B38 RID: 19256
		protected List<UnityEngine.Object> allocatedObjects;
	}
}
