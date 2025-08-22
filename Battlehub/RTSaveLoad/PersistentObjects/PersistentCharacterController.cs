using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000151 RID: 337
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentCharacterController : PersistentCollider
	{
		// Token: 0x06000798 RID: 1944 RVA: 0x00033769 File Offset: 0x00031B69
		public PersistentCharacterController()
		{
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x00033774 File Offset: 0x00031B74
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			CharacterController characterController = (CharacterController)obj;
			characterController.radius = this.radius;
			characterController.height = this.height;
			characterController.center = this.center;
			characterController.slopeLimit = this.slopeLimit;
			characterController.stepOffset = this.stepOffset;
			characterController.skinWidth = this.skinWidth;
			characterController.minMoveDistance = this.minMoveDistance;
			characterController.detectCollisions = this.detectCollisions;
			characterController.enableOverlapRecovery = this.enableOverlapRecovery;
			return characterController;
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x00033808 File Offset: 0x00031C08
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			CharacterController characterController = (CharacterController)obj;
			this.radius = characterController.radius;
			this.height = characterController.height;
			this.center = characterController.center;
			this.slopeLimit = characterController.slopeLimit;
			this.stepOffset = characterController.stepOffset;
			this.skinWidth = characterController.skinWidth;
			this.minMoveDistance = characterController.minMoveDistance;
			this.detectCollisions = characterController.detectCollisions;
			this.enableOverlapRecovery = characterController.enableOverlapRecovery;
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x00033896 File Offset: 0x00031C96
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x04000830 RID: 2096
		public float radius;

		// Token: 0x04000831 RID: 2097
		public float height;

		// Token: 0x04000832 RID: 2098
		public Vector3 center;

		// Token: 0x04000833 RID: 2099
		public float slopeLimit;

		// Token: 0x04000834 RID: 2100
		public float stepOffset;

		// Token: 0x04000835 RID: 2101
		public float skinWidth;

		// Token: 0x04000836 RID: 2102
		public float minMoveDistance;

		// Token: 0x04000837 RID: 2103
		public bool detectCollisions;

		// Token: 0x04000838 RID: 2104
		public bool enableOverlapRecovery;
	}
}
