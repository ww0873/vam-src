using System;
using UnityEngine;

namespace GPUTools.Common.Scripts.Tools.Demo
{
	// Token: 0x020009CA RID: 2506
	public class DemoCamera : MonoBehaviour
	{
		// Token: 0x06003F3C RID: 16188 RVA: 0x0012EC14 File Offset: 0x0012D014
		public DemoCamera()
		{
		}

		// Token: 0x06003F3D RID: 16189 RVA: 0x0012EC44 File Offset: 0x0012D044
		private void Awake()
		{
			this.radius = base.transform.position.z;
			this.elevation = base.transform.position.y;
		}

		// Token: 0x06003F3E RID: 16190 RVA: 0x0012EC83 File Offset: 0x0012D083
		private void OnEnable()
		{
		}

		// Token: 0x06003F3F RID: 16191 RVA: 0x0012EC88 File Offset: 0x0012D088
		private void Update()
		{
			float x = Mathf.Cos(this.angle) * this.radius;
			float y = this.elevation;
			float z = Mathf.Sin(this.angle) * this.radius;
			base.transform.position = new Vector3(x, y, z);
			base.transform.LookAt(this.lookAt);
			this.HandleWheel();
			this.HandleMove();
		}

		// Token: 0x06003F40 RID: 16192 RVA: 0x0012ECF2 File Offset: 0x0012D0F2
		private void HandleWheel()
		{
			this.radius += Input.GetAxis("Mouse ScrollWheel");
		}

		// Token: 0x06003F41 RID: 16193 RVA: 0x0012ED0C File Offset: 0x0012D10C
		private void HandleMove()
		{
			if (Input.GetMouseButton(0))
			{
				this.angle -= Input.GetAxis("Mouse X") * Time.deltaTime;
				this.elevation -= Input.GetAxis("Mouse Y") * Time.deltaTime;
			}
		}

		// Token: 0x04002FF8 RID: 12280
		[SerializeField]
		private Vector3 lookAt = new Vector3(0f, 0.05f, 0f);

		// Token: 0x04002FF9 RID: 12281
		private float radius;

		// Token: 0x04002FFA RID: 12282
		private float angle = 1.5707964f;

		// Token: 0x04002FFB RID: 12283
		private float elevation;
	}
}
