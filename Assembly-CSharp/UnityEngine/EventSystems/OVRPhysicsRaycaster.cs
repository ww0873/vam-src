using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UnityEngine.EventSystems
{
	// Token: 0x0200096A RID: 2410
	[RequireComponent(typeof(OVRCameraRig))]
	public class OVRPhysicsRaycaster : BaseRaycaster
	{
		// Token: 0x06003C25 RID: 15397 RVA: 0x00122EBE File Offset: 0x001212BE
		protected OVRPhysicsRaycaster()
		{
		}

		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x06003C26 RID: 15398 RVA: 0x00122EDA File Offset: 0x001212DA
		public override Camera eventCamera
		{
			get
			{
				return base.GetComponent<OVRCameraRig>().leftEyeCamera;
			}
		}

		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x06003C27 RID: 15399 RVA: 0x00122EE7 File Offset: 0x001212E7
		public virtual int depth
		{
			get
			{
				return (!(this.eventCamera != null)) ? 16777215 : ((int)this.eventCamera.depth);
			}
		}

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x06003C28 RID: 15400 RVA: 0x00122F10 File Offset: 0x00121310
		public override int sortOrderPriority
		{
			get
			{
				return this.sortOrder;
			}
		}

		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x06003C29 RID: 15401 RVA: 0x00122F18 File Offset: 0x00121318
		public int finalEventMask
		{
			get
			{
				return (!(this.eventCamera != null)) ? -1 : (this.eventCamera.cullingMask & this.m_EventMask);
			}
		}

		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x06003C2A RID: 15402 RVA: 0x00122F48 File Offset: 0x00121348
		// (set) Token: 0x06003C2B RID: 15403 RVA: 0x00122F50 File Offset: 0x00121350
		public LayerMask eventMask
		{
			get
			{
				return this.m_EventMask;
			}
			set
			{
				this.m_EventMask = value;
			}
		}

		// Token: 0x06003C2C RID: 15404 RVA: 0x00122F5C File Offset: 0x0012135C
		public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
		{
			if (this.eventCamera == null)
			{
				return;
			}
			if (!eventData.IsVRPointer())
			{
				return;
			}
			Ray ray = eventData.GetRay();
			float maxDistance = this.eventCamera.farClipPlane - this.eventCamera.nearClipPlane;
			RaycastHit[] array = Physics.RaycastAll(ray, maxDistance, this.finalEventMask);
			if (array.Length > 1)
			{
				RaycastHit[] array2 = array;
				if (OVRPhysicsRaycaster.<>f__am$cache0 == null)
				{
					OVRPhysicsRaycaster.<>f__am$cache0 = new Comparison<RaycastHit>(OVRPhysicsRaycaster.<Raycast>m__0);
				}
				Array.Sort<RaycastHit>(array2, OVRPhysicsRaycaster.<>f__am$cache0);
			}
			if (array.Length != 0)
			{
				int i = 0;
				int num = array.Length;
				while (i < num)
				{
					RaycastResult item = new RaycastResult
					{
						gameObject = array[i].collider.gameObject,
						module = this,
						distance = array[i].distance,
						index = (float)resultAppendList.Count,
						worldPosition = array[0].point,
						worldNormal = array[0].normal
					};
					resultAppendList.Add(item);
					i++;
				}
			}
		}

		// Token: 0x06003C2D RID: 15405 RVA: 0x0012307C File Offset: 0x0012147C
		public void Spherecast(PointerEventData eventData, List<RaycastResult> resultAppendList, float radius)
		{
			if (this.eventCamera == null)
			{
				return;
			}
			if (!eventData.IsVRPointer())
			{
				return;
			}
			Ray ray = eventData.GetRay();
			float maxDistance = this.eventCamera.farClipPlane - this.eventCamera.nearClipPlane;
			RaycastHit[] array = Physics.SphereCastAll(ray, radius, maxDistance, this.finalEventMask);
			if (array.Length > 1)
			{
				RaycastHit[] array2 = array;
				if (OVRPhysicsRaycaster.<>f__am$cache1 == null)
				{
					OVRPhysicsRaycaster.<>f__am$cache1 = new Comparison<RaycastHit>(OVRPhysicsRaycaster.<Spherecast>m__1);
				}
				Array.Sort<RaycastHit>(array2, OVRPhysicsRaycaster.<>f__am$cache1);
			}
			if (array.Length != 0)
			{
				int i = 0;
				int num = array.Length;
				while (i < num)
				{
					RaycastResult item = new RaycastResult
					{
						gameObject = array[i].collider.gameObject,
						module = this,
						distance = array[i].distance,
						index = (float)resultAppendList.Count,
						worldPosition = array[0].point,
						worldNormal = array[0].normal
					};
					resultAppendList.Add(item);
					i++;
				}
			}
		}

		// Token: 0x06003C2E RID: 15406 RVA: 0x0012319C File Offset: 0x0012159C
		public Vector2 GetScreenPos(Vector3 worldPosition)
		{
			return this.eventCamera.WorldToScreenPoint(worldPosition);
		}

		// Token: 0x06003C2F RID: 15407 RVA: 0x001231B0 File Offset: 0x001215B0
		[CompilerGenerated]
		private static int <Raycast>m__0(RaycastHit r1, RaycastHit r2)
		{
			return r1.distance.CompareTo(r2.distance);
		}

		// Token: 0x06003C30 RID: 15408 RVA: 0x001231D4 File Offset: 0x001215D4
		[CompilerGenerated]
		private static int <Spherecast>m__1(RaycastHit r1, RaycastHit r2)
		{
			return r1.distance.CompareTo(r2.distance);
		}

		// Token: 0x04002E12 RID: 11794
		protected const int kNoEventMaskSet = -1;

		// Token: 0x04002E13 RID: 11795
		[SerializeField]
		protected LayerMask m_EventMask = -1;

		// Token: 0x04002E14 RID: 11796
		public int sortOrder = 20;

		// Token: 0x04002E15 RID: 11797
		[CompilerGenerated]
		private static Comparison<RaycastHit> <>f__am$cache0;

		// Token: 0x04002E16 RID: 11798
		[CompilerGenerated]
		private static Comparison<RaycastHit> <>f__am$cache1;
	}
}
