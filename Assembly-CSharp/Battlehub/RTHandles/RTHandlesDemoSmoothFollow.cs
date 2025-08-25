using System;
using UnityEngine;

namespace Battlehub.RTHandles
{
	// Token: 0x020000F5 RID: 245
	public class RTHandlesDemoSmoothFollow : MonoBehaviour
	{
		// Token: 0x0600055C RID: 1372 RVA: 0x0001DDF0 File Offset: 0x0001C1F0
		public RTHandlesDemoSmoothFollow()
		{
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x0600055D RID: 1373 RVA: 0x0001DE0E File Offset: 0x0001C20E
		// (set) Token: 0x0600055E RID: 1374 RVA: 0x0001DE18 File Offset: 0x0001C218
		[SerializeField]
		public Transform target
		{
			get
			{
				return this.m_target;
			}
			set
			{
				this.m_target = value;
				float num = this.rotationDamping;
				float num2 = this.heightDamping;
				this.rotationDamping = float.MaxValue;
				this.heightDamping = float.MaxValue;
				this.Follow();
				this.heightDamping = num2;
				this.rotationDamping = num;
			}
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x0001DE64 File Offset: 0x0001C264
		private void Start()
		{
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x0001DE66 File Offset: 0x0001C266
		private void LateUpdate()
		{
			if (!this.target)
			{
				return;
			}
			this.Follow();
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x0001DE80 File Offset: 0x0001C280
		private void Follow()
		{
			float y = this.target.eulerAngles.y;
			float b = this.target.position.y + this.height;
			float num = base.transform.eulerAngles.y;
			float num2 = base.transform.position.y;
			num = Mathf.LerpAngle(num, y, this.rotationDamping * Time.deltaTime);
			num2 = Mathf.Lerp(num2, b, this.heightDamping * Time.deltaTime);
			Quaternion rotation = Quaternion.Euler(0f, num, 0f);
			base.transform.position = this.target.position;
			base.transform.position -= rotation * Vector3.forward * this.distance;
			base.transform.position = new Vector3(base.transform.position.x, num2, base.transform.position.z);
			base.transform.LookAt(this.target);
		}

		// Token: 0x040004EF RID: 1263
		private Transform m_target;

		// Token: 0x040004F0 RID: 1264
		[SerializeField]
		private float distance = 10f;

		// Token: 0x040004F1 RID: 1265
		[SerializeField]
		private float height = 5f;

		// Token: 0x040004F2 RID: 1266
		[SerializeField]
		private float rotationDamping;

		// Token: 0x040004F3 RID: 1267
		[SerializeField]
		private float heightDamping;
	}
}
