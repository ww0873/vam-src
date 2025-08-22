using System;

namespace Leap
{
	// Token: 0x020005DE RID: 1502
	public interface IController : IDisposable
	{
		// Token: 0x060025D8 RID: 9688
		Frame Frame(int history = 0);

		// Token: 0x060025D9 RID: 9689
		Frame GetTransformedFrame(LeapTransform trs, int history = 0);

		// Token: 0x060025DA RID: 9690
		Frame GetInterpolatedFrame(long time);

		// Token: 0x060025DB RID: 9691
		void SetPolicy(Controller.PolicyFlag policy);

		// Token: 0x060025DC RID: 9692
		void ClearPolicy(Controller.PolicyFlag policy);

		// Token: 0x060025DD RID: 9693
		bool IsPolicySet(Controller.PolicyFlag policy);

		// Token: 0x060025DE RID: 9694
		long Now();

		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x060025DF RID: 9695
		bool IsConnected { get; }

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x060025E0 RID: 9696
		Config Config { get; }

		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x060025E1 RID: 9697
		DeviceList Devices { get; }

		// Token: 0x140000A7 RID: 167
		// (add) Token: 0x060025E2 RID: 9698
		// (remove) Token: 0x060025E3 RID: 9699
		event EventHandler<ConnectionEventArgs> Connect;

		// Token: 0x140000A8 RID: 168
		// (add) Token: 0x060025E4 RID: 9700
		// (remove) Token: 0x060025E5 RID: 9701
		event EventHandler<ConnectionLostEventArgs> Disconnect;

		// Token: 0x140000A9 RID: 169
		// (add) Token: 0x060025E6 RID: 9702
		// (remove) Token: 0x060025E7 RID: 9703
		event EventHandler<FrameEventArgs> FrameReady;

		// Token: 0x140000AA RID: 170
		// (add) Token: 0x060025E8 RID: 9704
		// (remove) Token: 0x060025E9 RID: 9705
		event EventHandler<DeviceEventArgs> Device;

		// Token: 0x140000AB RID: 171
		// (add) Token: 0x060025EA RID: 9706
		// (remove) Token: 0x060025EB RID: 9707
		event EventHandler<DeviceEventArgs> DeviceLost;

		// Token: 0x140000AC RID: 172
		// (add) Token: 0x060025EC RID: 9708
		// (remove) Token: 0x060025ED RID: 9709
		event EventHandler<DeviceFailureEventArgs> DeviceFailure;

		// Token: 0x140000AD RID: 173
		// (add) Token: 0x060025EE RID: 9710
		// (remove) Token: 0x060025EF RID: 9711
		event EventHandler<LogEventArgs> LogMessage;

		// Token: 0x140000AE RID: 174
		// (add) Token: 0x060025F0 RID: 9712
		// (remove) Token: 0x060025F1 RID: 9713
		event EventHandler<PolicyEventArgs> PolicyChange;

		// Token: 0x140000AF RID: 175
		// (add) Token: 0x060025F2 RID: 9714
		// (remove) Token: 0x060025F3 RID: 9715
		event EventHandler<ConfigChangeEventArgs> ConfigChange;

		// Token: 0x140000B0 RID: 176
		// (add) Token: 0x060025F4 RID: 9716
		// (remove) Token: 0x060025F5 RID: 9717
		event EventHandler<DistortionEventArgs> DistortionChange;

		// Token: 0x140000B1 RID: 177
		// (add) Token: 0x060025F6 RID: 9718
		// (remove) Token: 0x060025F7 RID: 9719
		event EventHandler<ImageEventArgs> ImageReady;

		// Token: 0x140000B2 RID: 178
		// (add) Token: 0x060025F8 RID: 9720
		// (remove) Token: 0x060025F9 RID: 9721
		event EventHandler<PointMappingChangeEventArgs> PointMappingChange;

		// Token: 0x140000B3 RID: 179
		// (add) Token: 0x060025FA RID: 9722
		// (remove) Token: 0x060025FB RID: 9723
		event EventHandler<HeadPoseEventArgs> HeadPoseChange;
	}
}
