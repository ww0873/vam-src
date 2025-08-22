using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x0200012A RID: 298
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentAnimator : PersistentBehaviour
	{
		// Token: 0x06000711 RID: 1809 RVA: 0x00031121 File Offset: 0x0002F521
		public PersistentAnimator()
		{
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x0003112C File Offset: 0x0002F52C
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			Animator animator = (Animator)obj;
			animator.rootPosition = this.rootPosition;
			animator.rootRotation = this.rootRotation;
			animator.applyRootMotion = this.applyRootMotion;
			animator.linearVelocityBlending = this.linearVelocityBlending;
			animator.updateMode = (AnimatorUpdateMode)this.updateMode;
			animator.stabilizeFeet = this.stabilizeFeet;
			animator.feetPivotActive = this.feetPivotActive;
			animator.speed = this.speed;
			animator.cullingMode = (AnimatorCullingMode)this.cullingMode;
			animator.recorderStartTime = this.recorderStartTime;
			animator.recorderStopTime = this.recorderStopTime;
			animator.runtimeAnimatorController = (RuntimeAnimatorController)objects.Get(this.runtimeAnimatorController);
			animator.avatar = (Avatar)objects.Get(this.avatar);
			animator.layersAffectMassCenter = this.layersAffectMassCenter;
			animator.logWarnings = this.logWarnings;
			animator.fireEvents = this.fireEvents;
			return animator;
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x0003122C File Offset: 0x0002F62C
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			Animator animator = (Animator)obj;
			this.rootPosition = animator.rootPosition;
			this.rootRotation = animator.rootRotation;
			this.applyRootMotion = animator.applyRootMotion;
			this.linearVelocityBlending = animator.linearVelocityBlending;
			this.updateMode = (uint)animator.updateMode;
			this.stabilizeFeet = animator.stabilizeFeet;
			this.feetPivotActive = animator.feetPivotActive;
			this.speed = animator.speed;
			this.cullingMode = (uint)animator.cullingMode;
			this.recorderStartTime = animator.recorderStartTime;
			this.recorderStopTime = animator.recorderStopTime;
			this.runtimeAnimatorController = animator.runtimeAnimatorController.GetMappedInstanceID();
			this.avatar = animator.avatar.GetMappedInstanceID();
			this.layersAffectMassCenter = animator.layersAffectMassCenter;
			this.logWarnings = animator.logWarnings;
			this.fireEvents = animator.fireEvents;
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x00031318 File Offset: 0x0002F718
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.runtimeAnimatorController, dependencies, objects, allowNulls);
			base.AddDependency<T>(this.avatar, dependencies, objects, allowNulls);
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x00031344 File Offset: 0x0002F744
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			Animator animator = (Animator)obj;
			base.AddDependency(animator.runtimeAnimatorController, dependencies);
			base.AddDependency(animator.avatar, dependencies);
		}

		// Token: 0x0400076E RID: 1902
		public Vector3 rootPosition;

		// Token: 0x0400076F RID: 1903
		public Quaternion rootRotation;

		// Token: 0x04000770 RID: 1904
		public bool applyRootMotion;

		// Token: 0x04000771 RID: 1905
		public bool linearVelocityBlending;

		// Token: 0x04000772 RID: 1906
		public uint updateMode;

		// Token: 0x04000773 RID: 1907
		public bool stabilizeFeet;

		// Token: 0x04000774 RID: 1908
		public float feetPivotActive;

		// Token: 0x04000775 RID: 1909
		public float speed;

		// Token: 0x04000776 RID: 1910
		public uint cullingMode;

		// Token: 0x04000777 RID: 1911
		public float recorderStartTime;

		// Token: 0x04000778 RID: 1912
		public float recorderStopTime;

		// Token: 0x04000779 RID: 1913
		public long runtimeAnimatorController;

		// Token: 0x0400077A RID: 1914
		public long avatar;

		// Token: 0x0400077B RID: 1915
		public bool layersAffectMassCenter;

		// Token: 0x0400077C RID: 1916
		public bool logWarnings;

		// Token: 0x0400077D RID: 1917
		public bool fireEvents;
	}
}
