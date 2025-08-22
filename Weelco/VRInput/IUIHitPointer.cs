using System;
using UnityEngine;

namespace Weelco.VRInput
{
	// Token: 0x02000587 RID: 1415
	public abstract class IUIHitPointer : IUIPointer
	{
		// Token: 0x0600239E RID: 9118 RVA: 0x000CEDD3 File Offset: 0x000CD1D3
		protected IUIHitPointer()
		{
		}

		// Token: 0x0600239F RID: 9119
		public abstract bool ButtonDown();

		// Token: 0x060023A0 RID: 9120
		public abstract bool ButtonUp();

		// Token: 0x060023A1 RID: 9121 RVA: 0x000CEDDC File Offset: 0x000CD1DC
		public override void Initialize()
		{
			if (this.UseCustomLaserPointer)
			{
				this.hitPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				this.hitPoint.transform.SetParent(this.target.transform, false);
				this.hitPoint.transform.localScale = new Vector3(this.LaserHitScale, this.LaserHitScale, this.LaserHitScale);
				this.hitPoint.transform.localPosition = new Vector3(0f, 0f, 100f);
				UnityEngine.Object.DestroyImmediate(this.hitPoint.GetComponent<SphereCollider>());
				Material material = new Material(Shader.Find("Unlit/Color"));
				material.SetColor("_Color", this.LaserColor);
				this.hitPoint.GetComponent<MeshRenderer>().material = material;
			}
			else if (this.target)
			{
				Transform transform = this.target.Find("LaserPointer/LaserBeamDot");
				if (transform)
				{
					this.hitPoint = transform.gameObject;
				}
			}
			if (this.hitPoint && !this.HitAlwaysOn)
			{
				this.hitPoint.SetActive(false);
			}
		}

		// Token: 0x060023A2 RID: 9122 RVA: 0x000CEF10 File Offset: 0x000CD310
		protected override void UpdateRaycasting(bool isHit, float distance)
		{
			if (this.hitPoint)
			{
				if (isHit)
				{
					this.hitPoint.transform.localPosition = new Vector3(0f, 0f, distance);
				}
				if (!this.HitAlwaysOn)
				{
					this.hitPoint.SetActive(isHit);
				}
			}
		}

		// Token: 0x04001E18 RID: 7704
		public float LaserHitScale;

		// Token: 0x04001E19 RID: 7705
		public Color LaserColor;

		// Token: 0x04001E1A RID: 7706
		public bool HitAlwaysOn;

		// Token: 0x04001E1B RID: 7707
		public bool UseCustomLaserPointer;

		// Token: 0x04001E1C RID: 7708
		private GameObject hitPoint;
	}
}
