using System;

namespace Leap.Unity
{
	// Token: 0x0200071C RID: 1820
	public class StationaryTestLeapProvider : LeapProvider
	{
		// Token: 0x06002C5A RID: 11354 RVA: 0x000EDD94 File Offset: 0x000EC194
		public StationaryTestLeapProvider()
		{
		}

		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x06002C5B RID: 11355 RVA: 0x000EDD9C File Offset: 0x000EC19C
		public override Frame CurrentFrame
		{
			get
			{
				return this._curFrame;
			}
		}

		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x06002C5C RID: 11356 RVA: 0x000EDDA4 File Offset: 0x000EC1A4
		public override Frame CurrentFixedFrame
		{
			get
			{
				return this._curFrame;
			}
		}

		// Token: 0x06002C5D RID: 11357 RVA: 0x000EDDAC File Offset: 0x000EC1AC
		private void Awake()
		{
			this.refreshFrame();
		}

		// Token: 0x06002C5E RID: 11358 RVA: 0x000EDDB4 File Offset: 0x000EC1B4
		private void refreshFrame()
		{
			this._curFrame = new Frame();
			this._leftHand = this.MakeTestHand(true);
			this._rightHand = this.MakeTestHand(false);
			this._curFrame.Hands.Add(this._leftHand);
			this._curFrame.Hands.Add(this._rightHand);
		}

		// Token: 0x06002C5F RID: 11359 RVA: 0x000EDE12 File Offset: 0x000EC212
		private void Update()
		{
			this.refreshFrame();
			base.DispatchUpdateFrameEvent(this._curFrame);
		}

		// Token: 0x06002C60 RID: 11360 RVA: 0x000EDE26 File Offset: 0x000EC226
		private void FixedUpdate()
		{
			base.DispatchFixedFrameEvent(this._curFrame);
		}

		// Token: 0x04002373 RID: 9075
		private Frame _curFrame;

		// Token: 0x04002374 RID: 9076
		private Hand _leftHand;

		// Token: 0x04002375 RID: 9077
		private Hand _rightHand;
	}
}
