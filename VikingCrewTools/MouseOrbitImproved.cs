using System;
using UnityEngine;

namespace VikingCrewTools
{
	// Token: 0x02000566 RID: 1382
	[AddComponentMenu("Camera-Control/Mouse Orbit with zoom")]
	public class MouseOrbitImproved : MonoBehaviour
	{
		// Token: 0x0600231A RID: 8986 RVA: 0x000C8100 File Offset: 0x000C6500
		public MouseOrbitImproved()
		{
		}

		// Token: 0x0600231B RID: 8987 RVA: 0x000C8160 File Offset: 0x000C6560
		private void Start()
		{
			Vector3 eulerAngles = base.transform.eulerAngles;
			this.x = eulerAngles.y;
			this.y = eulerAngles.x;
			this._rigidbody = base.GetComponent<Rigidbody>();
			if (this._rigidbody != null)
			{
				this._rigidbody.freezeRotation = true;
			}
		}

		// Token: 0x0600231C RID: 8988 RVA: 0x000C81BC File Offset: 0x000C65BC
		private void LateUpdate()
		{
			if (this.target)
			{
				if (Input.GetMouseButton(1))
				{
					this.x += Input.GetAxis("Mouse X") * this.xSpeed * this.distance * 0.02f;
					this.y -= Input.GetAxis("Mouse Y") * this.ySpeed * 0.02f;
				}
				this.y = MouseOrbitImproved.ClampAngle(this.y, this.yMinLimit, this.yMaxLimit);
				Quaternion rotation = Quaternion.Euler(this.y, this.x, 0f);
				this.distance = Mathf.Clamp(this.distance - Input.GetAxis("Mouse ScrollWheel") * 5f, this.distanceMin, this.distanceMax);
				RaycastHit raycastHit;
				if (Physics.Linecast(this.target.position, base.transform.position, out raycastHit))
				{
					this.distance -= raycastHit.distance;
				}
				Vector3 point = new Vector3(0f, 0f, -this.distance);
				Vector3 position = rotation * point + this.target.position;
				base.transform.rotation = rotation;
				base.transform.position = position;
			}
		}

		// Token: 0x0600231D RID: 8989 RVA: 0x000C8313 File Offset: 0x000C6713
		public static float ClampAngle(float angle, float min, float max)
		{
			if (angle < -360f)
			{
				angle += 360f;
			}
			if (angle > 360f)
			{
				angle -= 360f;
			}
			return Mathf.Clamp(angle, min, max);
		}

		// Token: 0x04001D09 RID: 7433
		public Transform target;

		// Token: 0x04001D0A RID: 7434
		public float distance = 5f;

		// Token: 0x04001D0B RID: 7435
		public float xSpeed = 120f;

		// Token: 0x04001D0C RID: 7436
		public float ySpeed = 120f;

		// Token: 0x04001D0D RID: 7437
		public float yMinLimit = -20f;

		// Token: 0x04001D0E RID: 7438
		public float yMaxLimit = 80f;

		// Token: 0x04001D0F RID: 7439
		public float distanceMin = 0.5f;

		// Token: 0x04001D10 RID: 7440
		public float distanceMax = 15f;

		// Token: 0x04001D11 RID: 7441
		private Rigidbody _rigidbody;

		// Token: 0x04001D12 RID: 7442
		private float x;

		// Token: 0x04001D13 RID: 7443
		private float y;
	}
}
