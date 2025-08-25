using System;
using UnityEngine;

namespace Battlehub.RTCommon
{
	// Token: 0x020000BB RID: 187
	[AddComponentMenu("Camera-Control/Mouse Orbit with zoom")]
	public class MouseOrbit : MonoBehaviour
	{
		// Token: 0x06000316 RID: 790 RVA: 0x00014380 File Offset: 0x00012780
		public MouseOrbit()
		{
		}

		// Token: 0x06000317 RID: 791 RVA: 0x000143E0 File Offset: 0x000127E0
		private void Awake()
		{
			this.m_camera = base.GetComponent<Camera>();
		}

		// Token: 0x06000318 RID: 792 RVA: 0x000143EE File Offset: 0x000127EE
		private void Start()
		{
			this.SyncAngles();
		}

		// Token: 0x06000319 RID: 793 RVA: 0x000143F8 File Offset: 0x000127F8
		public void SyncAngles()
		{
			Vector3 eulerAngles = base.transform.eulerAngles;
			this.m_x = eulerAngles.y;
			this.m_y = eulerAngles.x;
		}

		// Token: 0x0600031A RID: 794 RVA: 0x0001442C File Offset: 0x0001282C
		private void LateUpdate()
		{
			float num = Input.GetAxis("Mouse X");
			float num2 = Input.GetAxis("Mouse Y");
			Vector3 mousePosition = Input.mousePosition;
			Vector3 vector = mousePosition - this.lastMousePosition;
			if (num == 0f && num2 == 0f && (vector.x != 0f || vector.y != 0f))
			{
				num = vector.x * 0.1f;
				num2 = vector.y * 0.1f;
			}
			this.lastMousePosition = mousePosition;
			num *= this.XSpeed;
			num2 *= this.YSpeed;
			this.m_x += num;
			this.m_y -= num2;
			this.m_y = MouseOrbit.ClampAngle(this.m_y, this.YMinLimit, this.YMaxLimit);
			this.Zoom();
		}

		// Token: 0x0600031B RID: 795 RVA: 0x00014510 File Offset: 0x00012910
		public void Zoom()
		{
			Quaternion rotation = Quaternion.Euler(this.m_y, this.m_x, 0f);
			base.transform.rotation = rotation;
			float axis = Input.GetAxis("Mouse ScrollWheel");
			if (this.m_camera.orthographic)
			{
				this.m_camera.orthographicSize -= axis * this.m_camera.orthographicSize;
				if (this.m_camera.orthographicSize < 0.01f)
				{
					this.m_camera.orthographicSize = 0.01f;
				}
			}
			this.Distance = Mathf.Clamp(this.Distance - axis * Mathf.Max(1f, this.Distance), this.DistanceMin, this.DistanceMax);
			Vector3 point = new Vector3(0f, 0f, -this.Distance);
			Vector3 position = rotation * point + this.Target.position;
			base.transform.position = position;
		}

		// Token: 0x0600031C RID: 796 RVA: 0x0001460B File Offset: 0x00012A0B
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

		// Token: 0x040003CD RID: 973
		private Camera m_camera;

		// Token: 0x040003CE RID: 974
		public Transform Target;

		// Token: 0x040003CF RID: 975
		public float Distance = 5f;

		// Token: 0x040003D0 RID: 976
		public float XSpeed = 5f;

		// Token: 0x040003D1 RID: 977
		public float YSpeed = 5f;

		// Token: 0x040003D2 RID: 978
		public float YMinLimit = -360f;

		// Token: 0x040003D3 RID: 979
		public float YMaxLimit = 360f;

		// Token: 0x040003D4 RID: 980
		public float DistanceMin = 0.5f;

		// Token: 0x040003D5 RID: 981
		public float DistanceMax = 5000f;

		// Token: 0x040003D6 RID: 982
		private float m_x;

		// Token: 0x040003D7 RID: 983
		private float m_y;

		// Token: 0x040003D8 RID: 984
		private Vector3 lastMousePosition;
	}
}
