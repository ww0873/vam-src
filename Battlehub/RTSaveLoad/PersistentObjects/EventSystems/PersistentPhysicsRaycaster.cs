using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Battlehub.RTSaveLoad.PersistentObjects.EventSystems
{
	// Token: 0x0200019B RID: 411
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[ProtoInclude(1128, typeof(PersistentPhysics2DRaycaster))]
	[Serializable]
	public class PersistentPhysicsRaycaster : PersistentBaseRaycaster
	{
		// Token: 0x060008BE RID: 2238 RVA: 0x000377D9 File Offset: 0x00035BD9
		public PersistentPhysicsRaycaster()
		{
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x000377E4 File Offset: 0x00035BE4
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			PhysicsRaycaster physicsRaycaster = (PhysicsRaycaster)obj;
			physicsRaycaster.eventMask = this.eventMask;
			return physicsRaycaster;
		}

		// Token: 0x060008C0 RID: 2240 RVA: 0x00037818 File Offset: 0x00035C18
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			PhysicsRaycaster physicsRaycaster = (PhysicsRaycaster)obj;
			this.eventMask = physicsRaycaster.eventMask;
		}

		// Token: 0x060008C1 RID: 2241 RVA: 0x00037846 File Offset: 0x00035C46
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x0400098D RID: 2445
		public LayerMask eventMask;
	}
}
