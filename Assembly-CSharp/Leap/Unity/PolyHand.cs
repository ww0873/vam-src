using System;

namespace Leap.Unity
{
	// Token: 0x02000759 RID: 1881
	public class PolyHand : HandModel
	{
		// Token: 0x0600306C RID: 12396 RVA: 0x000FBE48 File Offset: 0x000FA248
		public PolyHand()
		{
		}

		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x0600306D RID: 12397 RVA: 0x000FBE50 File Offset: 0x000FA250
		public override ModelType HandModelType
		{
			get
			{
				return ModelType.Graphics;
			}
		}

		// Token: 0x0600306E RID: 12398 RVA: 0x000FBE53 File Offset: 0x000FA253
		public override bool SupportsEditorPersistence()
		{
			return true;
		}

		// Token: 0x0600306F RID: 12399 RVA: 0x000FBE58 File Offset: 0x000FA258
		public override void InitHand()
		{
			this.SetPalmOrientation();
			for (int i = 0; i < this.fingers.Length; i++)
			{
				if (this.fingers[i] != null)
				{
					this.fingers[i].fingerType = (Finger.FingerType)i;
					this.fingers[i].InitFinger();
				}
			}
		}

		// Token: 0x06003070 RID: 12400 RVA: 0x000FBEB4 File Offset: 0x000FA2B4
		public override void UpdateHand()
		{
			this.SetPalmOrientation();
			for (int i = 0; i < this.fingers.Length; i++)
			{
				if (this.fingers[i] != null)
				{
					this.fingers[i].UpdateFinger();
				}
			}
		}

		// Token: 0x06003071 RID: 12401 RVA: 0x000FBF00 File Offset: 0x000FA300
		protected void SetPalmOrientation()
		{
			if (this.palm != null)
			{
				this.palm.position = base.GetPalmPosition();
				this.palm.rotation = base.GetPalmRotation();
			}
		}
	}
}
