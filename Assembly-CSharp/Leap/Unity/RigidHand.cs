using System;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x020006EE RID: 1774
	public class RigidHand : SkeletalHand
	{
		// Token: 0x06002B00 RID: 11008 RVA: 0x000E83FF File Offset: 0x000E67FF
		public RigidHand()
		{
		}

		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x06002B01 RID: 11009 RVA: 0x000E8412 File Offset: 0x000E6812
		public override ModelType HandModelType
		{
			get
			{
				return ModelType.Physics;
			}
		}

		// Token: 0x06002B02 RID: 11010 RVA: 0x000E8415 File Offset: 0x000E6815
		public override bool SupportsEditorPersistence()
		{
			return true;
		}

		// Token: 0x06002B03 RID: 11011 RVA: 0x000E8418 File Offset: 0x000E6818
		public override void InitHand()
		{
			base.InitHand();
		}

		// Token: 0x06002B04 RID: 11012 RVA: 0x000E8420 File Offset: 0x000E6820
		public override void UpdateHand()
		{
			for (int i = 0; i < this.fingers.Length; i++)
			{
				if (this.fingers[i] != null)
				{
					this.fingers[i].UpdateFinger();
				}
			}
			if (this.palm != null)
			{
				Rigidbody component = this.palm.GetComponent<Rigidbody>();
				if (component)
				{
					component.MovePosition(base.GetPalmCenter());
					component.MoveRotation(base.GetPalmRotation());
				}
				else
				{
					this.palm.position = base.GetPalmCenter();
					this.palm.rotation = base.GetPalmRotation();
				}
			}
			if (this.forearm != null)
			{
				CapsuleCollider component2 = this.forearm.GetComponent<CapsuleCollider>();
				if (component2 != null)
				{
					component2.direction = 2;
					this.forearm.localScale = new Vector3(1f / base.transform.lossyScale.x, 1f / base.transform.lossyScale.y, 1f / base.transform.lossyScale.z);
					component2.radius = base.GetArmWidth() / 2f;
					component2.height = base.GetArmLength() + base.GetArmWidth();
				}
				Rigidbody component3 = this.forearm.GetComponent<Rigidbody>();
				if (component3)
				{
					component3.MovePosition(base.GetArmCenter());
					component3.MoveRotation(base.GetArmRotation());
				}
				else
				{
					this.forearm.position = base.GetArmCenter();
					this.forearm.rotation = base.GetArmRotation();
				}
			}
		}

		// Token: 0x040022D8 RID: 8920
		public float filtering = 0.5f;
	}
}
