using System;
using UnityEngine;

namespace Weelco.VRInput
{
	// Token: 0x02000588 RID: 1416
	public abstract class IUILaserPointer : IUIHitPointer
	{
		// Token: 0x060023A3 RID: 9123 RVA: 0x000CEF6A File Offset: 0x000CD36A
		protected IUILaserPointer()
		{
		}

		// Token: 0x060023A4 RID: 9124 RVA: 0x000CEF74 File Offset: 0x000CD374
		public override void Initialize()
		{
			if (this.UseCustomLaserPointer)
			{
				this.pointer = GameObject.CreatePrimitive(PrimitiveType.Cube);
				this.pointer.transform.SetParent(this.target.transform, false);
				this.pointer.transform.localScale = new Vector3(this.LaserThickness, this.LaserThickness, 100f);
				this.pointer.transform.localPosition = new Vector3(0f, 0f, 50f);
				UnityEngine.Object.DestroyImmediate(this.pointer.GetComponent<BoxCollider>());
				Material material = new Material(Shader.Find("Unlit/Color"));
				material.SetColor("_Color", this.LaserColor);
				this.pointer.GetComponent<MeshRenderer>().material = material;
			}
			else if (this.target)
			{
				Transform transform = this.target.Find("LaserPointer/LaserBeam");
				if (transform)
				{
					this.pointer = transform.gameObject;
				}
			}
			base.Initialize();
		}

		// Token: 0x060023A5 RID: 9125 RVA: 0x000CF084 File Offset: 0x000CD484
		protected override void UpdateRaycasting(bool isHit, float distance)
		{
			if (this.pointer)
			{
				this.pointer.transform.localScale = new Vector3(this.pointer.transform.localScale.x, this.pointer.transform.localScale.y, distance);
				this.pointer.transform.localPosition = new Vector3(0f, 0f, distance * 0.5f);
			}
			base.UpdateRaycasting(isHit, distance);
		}

		// Token: 0x04001E1D RID: 7709
		public bool IsRightHand;

		// Token: 0x04001E1E RID: 7710
		public bool UseHapticPulse;

		// Token: 0x04001E1F RID: 7711
		public float LaserThickness;

		// Token: 0x04001E20 RID: 7712
		private GameObject pointer;

		// Token: 0x04001E21 RID: 7713
		private float _distanceLimit;
	}
}
