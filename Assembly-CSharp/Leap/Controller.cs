using System;
using System.Threading;
using LeapInternal;

namespace Leap
{
	// Token: 0x020005B9 RID: 1465
	public class Controller : IController, IDisposable
	{
		// Token: 0x060024EE RID: 9454 RVA: 0x000D5742 File Offset: 0x000D3B42
		public Controller() : this(0)
		{
		}

		// Token: 0x060024EF RID: 9455 RVA: 0x000D574C File Offset: 0x000D3B4C
		public Controller(int connectionKey)
		{
			this._connection = Connection.GetConnection(connectionKey);
			this._connection.EventContext = SynchronizationContext.Current;
			this._connection.LeapInit += this.OnInit;
			this._connection.LeapConnection += this.OnConnect;
			Connection connection = this._connection;
			connection.LeapConnectionLost = (EventHandler<ConnectionLostEventArgs>)Delegate.Combine(connection.LeapConnectionLost, new EventHandler<ConnectionLostEventArgs>(this.OnDisconnect));
			this._connection.Start();
		}

		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x060024F0 RID: 9456 RVA: 0x000D57DE File Offset: 0x000D3BDE
		// (set) Token: 0x060024F1 RID: 9457 RVA: 0x000D57EB File Offset: 0x000D3BEB
		public SynchronizationContext EventContext
		{
			get
			{
				return this._connection.EventContext;
			}
			set
			{
				this._connection.EventContext = value;
			}
		}

		// Token: 0x14000093 RID: 147
		// (add) Token: 0x060024F2 RID: 9458 RVA: 0x000D57F9 File Offset: 0x000D3BF9
		// (remove) Token: 0x060024F3 RID: 9459 RVA: 0x000D582B File Offset: 0x000D3C2B
		public event EventHandler<LeapEventArgs> Init
		{
			add
			{
				if (this._hasInitialized)
				{
					value(this, new LeapEventArgs(LeapEvent.EVENT_INIT));
				}
				this._init = (EventHandler<LeapEventArgs>)Delegate.Combine(this._init, value);
			}
			remove
			{
				this._init = (EventHandler<LeapEventArgs>)Delegate.Remove(this._init, value);
			}
		}

		// Token: 0x14000094 RID: 148
		// (add) Token: 0x060024F4 RID: 9460 RVA: 0x000D5844 File Offset: 0x000D3C44
		// (remove) Token: 0x060024F5 RID: 9461 RVA: 0x000D5874 File Offset: 0x000D3C74
		public event EventHandler<ConnectionEventArgs> Connect
		{
			add
			{
				if (this._hasConnected)
				{
					value(this, new ConnectionEventArgs());
				}
				this._connect = (EventHandler<ConnectionEventArgs>)Delegate.Combine(this._connect, value);
			}
			remove
			{
				this._connect = (EventHandler<ConnectionEventArgs>)Delegate.Remove(this._connect, value);
			}
		}

		// Token: 0x14000095 RID: 149
		// (add) Token: 0x060024F6 RID: 9462 RVA: 0x000D588D File Offset: 0x000D3C8D
		// (remove) Token: 0x060024F7 RID: 9463 RVA: 0x000D58AB File Offset: 0x000D3CAB
		public event EventHandler<ConnectionLostEventArgs> Disconnect
		{
			add
			{
				Connection connection = this._connection;
				connection.LeapConnectionLost = (EventHandler<ConnectionLostEventArgs>)Delegate.Combine(connection.LeapConnectionLost, value);
			}
			remove
			{
				Connection connection = this._connection;
				connection.LeapConnectionLost = (EventHandler<ConnectionLostEventArgs>)Delegate.Remove(connection.LeapConnectionLost, value);
			}
		}

		// Token: 0x14000096 RID: 150
		// (add) Token: 0x060024F8 RID: 9464 RVA: 0x000D58C9 File Offset: 0x000D3CC9
		// (remove) Token: 0x060024F9 RID: 9465 RVA: 0x000D58E7 File Offset: 0x000D3CE7
		public event EventHandler<FrameEventArgs> FrameReady
		{
			add
			{
				Connection connection = this._connection;
				connection.LeapFrame = (EventHandler<FrameEventArgs>)Delegate.Combine(connection.LeapFrame, value);
			}
			remove
			{
				Connection connection = this._connection;
				connection.LeapFrame = (EventHandler<FrameEventArgs>)Delegate.Remove(connection.LeapFrame, value);
			}
		}

		// Token: 0x14000097 RID: 151
		// (add) Token: 0x060024FA RID: 9466 RVA: 0x000D5905 File Offset: 0x000D3D05
		// (remove) Token: 0x060024FB RID: 9467 RVA: 0x000D5923 File Offset: 0x000D3D23
		public event EventHandler<InternalFrameEventArgs> InternalFrameReady
		{
			add
			{
				Connection connection = this._connection;
				connection.LeapInternalFrame = (EventHandler<InternalFrameEventArgs>)Delegate.Combine(connection.LeapInternalFrame, value);
			}
			remove
			{
				Connection connection = this._connection;
				connection.LeapInternalFrame = (EventHandler<InternalFrameEventArgs>)Delegate.Remove(connection.LeapInternalFrame, value);
			}
		}

		// Token: 0x14000098 RID: 152
		// (add) Token: 0x060024FC RID: 9468 RVA: 0x000D5941 File Offset: 0x000D3D41
		// (remove) Token: 0x060024FD RID: 9469 RVA: 0x000D595F File Offset: 0x000D3D5F
		public event EventHandler<DeviceEventArgs> Device
		{
			add
			{
				Connection connection = this._connection;
				connection.LeapDevice = (EventHandler<DeviceEventArgs>)Delegate.Combine(connection.LeapDevice, value);
			}
			remove
			{
				Connection connection = this._connection;
				connection.LeapDevice = (EventHandler<DeviceEventArgs>)Delegate.Remove(connection.LeapDevice, value);
			}
		}

		// Token: 0x14000099 RID: 153
		// (add) Token: 0x060024FE RID: 9470 RVA: 0x000D597D File Offset: 0x000D3D7D
		// (remove) Token: 0x060024FF RID: 9471 RVA: 0x000D599B File Offset: 0x000D3D9B
		public event EventHandler<DeviceEventArgs> DeviceLost
		{
			add
			{
				Connection connection = this._connection;
				connection.LeapDeviceLost = (EventHandler<DeviceEventArgs>)Delegate.Combine(connection.LeapDeviceLost, value);
			}
			remove
			{
				Connection connection = this._connection;
				connection.LeapDeviceLost = (EventHandler<DeviceEventArgs>)Delegate.Remove(connection.LeapDeviceLost, value);
			}
		}

		// Token: 0x1400009A RID: 154
		// (add) Token: 0x06002500 RID: 9472 RVA: 0x000D59B9 File Offset: 0x000D3DB9
		// (remove) Token: 0x06002501 RID: 9473 RVA: 0x000D59D7 File Offset: 0x000D3DD7
		public event EventHandler<DeviceFailureEventArgs> DeviceFailure
		{
			add
			{
				Connection connection = this._connection;
				connection.LeapDeviceFailure = (EventHandler<DeviceFailureEventArgs>)Delegate.Combine(connection.LeapDeviceFailure, value);
			}
			remove
			{
				Connection connection = this._connection;
				connection.LeapDeviceFailure = (EventHandler<DeviceFailureEventArgs>)Delegate.Remove(connection.LeapDeviceFailure, value);
			}
		}

		// Token: 0x1400009B RID: 155
		// (add) Token: 0x06002502 RID: 9474 RVA: 0x000D59F5 File Offset: 0x000D3DF5
		// (remove) Token: 0x06002503 RID: 9475 RVA: 0x000D5A13 File Offset: 0x000D3E13
		public event EventHandler<LogEventArgs> LogMessage
		{
			add
			{
				Connection connection = this._connection;
				connection.LeapLogEvent = (EventHandler<LogEventArgs>)Delegate.Combine(connection.LeapLogEvent, value);
			}
			remove
			{
				Connection connection = this._connection;
				connection.LeapLogEvent = (EventHandler<LogEventArgs>)Delegate.Remove(connection.LeapLogEvent, value);
			}
		}

		// Token: 0x1400009C RID: 156
		// (add) Token: 0x06002504 RID: 9476 RVA: 0x000D5A31 File Offset: 0x000D3E31
		// (remove) Token: 0x06002505 RID: 9477 RVA: 0x000D5A4F File Offset: 0x000D3E4F
		public event EventHandler<PolicyEventArgs> PolicyChange
		{
			add
			{
				Connection connection = this._connection;
				connection.LeapPolicyChange = (EventHandler<PolicyEventArgs>)Delegate.Combine(connection.LeapPolicyChange, value);
			}
			remove
			{
				Connection connection = this._connection;
				connection.LeapPolicyChange = (EventHandler<PolicyEventArgs>)Delegate.Remove(connection.LeapPolicyChange, value);
			}
		}

		// Token: 0x1400009D RID: 157
		// (add) Token: 0x06002506 RID: 9478 RVA: 0x000D5A6D File Offset: 0x000D3E6D
		// (remove) Token: 0x06002507 RID: 9479 RVA: 0x000D5A8B File Offset: 0x000D3E8B
		public event EventHandler<ConfigChangeEventArgs> ConfigChange
		{
			add
			{
				Connection connection = this._connection;
				connection.LeapConfigChange = (EventHandler<ConfigChangeEventArgs>)Delegate.Combine(connection.LeapConfigChange, value);
			}
			remove
			{
				Connection connection = this._connection;
				connection.LeapConfigChange = (EventHandler<ConfigChangeEventArgs>)Delegate.Remove(connection.LeapConfigChange, value);
			}
		}

		// Token: 0x1400009E RID: 158
		// (add) Token: 0x06002508 RID: 9480 RVA: 0x000D5AA9 File Offset: 0x000D3EA9
		// (remove) Token: 0x06002509 RID: 9481 RVA: 0x000D5AC7 File Offset: 0x000D3EC7
		public event EventHandler<DistortionEventArgs> DistortionChange
		{
			add
			{
				Connection connection = this._connection;
				connection.LeapDistortionChange = (EventHandler<DistortionEventArgs>)Delegate.Combine(connection.LeapDistortionChange, value);
			}
			remove
			{
				Connection connection = this._connection;
				connection.LeapDistortionChange = (EventHandler<DistortionEventArgs>)Delegate.Remove(connection.LeapDistortionChange, value);
			}
		}

		// Token: 0x1400009F RID: 159
		// (add) Token: 0x0600250A RID: 9482 RVA: 0x000D5AE5 File Offset: 0x000D3EE5
		// (remove) Token: 0x0600250B RID: 9483 RVA: 0x000D5B03 File Offset: 0x000D3F03
		public event EventHandler<DroppedFrameEventArgs> DroppedFrame
		{
			add
			{
				Connection connection = this._connection;
				connection.LeapDroppedFrame = (EventHandler<DroppedFrameEventArgs>)Delegate.Combine(connection.LeapDroppedFrame, value);
			}
			remove
			{
				Connection connection = this._connection;
				connection.LeapDroppedFrame = (EventHandler<DroppedFrameEventArgs>)Delegate.Remove(connection.LeapDroppedFrame, value);
			}
		}

		// Token: 0x140000A0 RID: 160
		// (add) Token: 0x0600250C RID: 9484 RVA: 0x000D5B21 File Offset: 0x000D3F21
		// (remove) Token: 0x0600250D RID: 9485 RVA: 0x000D5B3F File Offset: 0x000D3F3F
		public event EventHandler<ImageEventArgs> ImageReady
		{
			add
			{
				Connection connection = this._connection;
				connection.LeapImage = (EventHandler<ImageEventArgs>)Delegate.Combine(connection.LeapImage, value);
			}
			remove
			{
				Connection connection = this._connection;
				connection.LeapImage = (EventHandler<ImageEventArgs>)Delegate.Remove(connection.LeapImage, value);
			}
		}

		// Token: 0x140000A1 RID: 161
		// (add) Token: 0x0600250E RID: 9486 RVA: 0x000D5B5D File Offset: 0x000D3F5D
		// (remove) Token: 0x0600250F RID: 9487 RVA: 0x000D5B7B File Offset: 0x000D3F7B
		public event Action<BeginProfilingForThreadArgs> BeginProfilingForThread
		{
			add
			{
				Connection connection = this._connection;
				connection.LeapBeginProfilingForThread = (Action<BeginProfilingForThreadArgs>)Delegate.Combine(connection.LeapBeginProfilingForThread, value);
			}
			remove
			{
				Connection connection = this._connection;
				connection.LeapBeginProfilingForThread = (Action<BeginProfilingForThreadArgs>)Delegate.Remove(connection.LeapBeginProfilingForThread, value);
			}
		}

		// Token: 0x140000A2 RID: 162
		// (add) Token: 0x06002510 RID: 9488 RVA: 0x000D5B99 File Offset: 0x000D3F99
		// (remove) Token: 0x06002511 RID: 9489 RVA: 0x000D5BB7 File Offset: 0x000D3FB7
		public event Action<EndProfilingForThreadArgs> EndProfilingForThread
		{
			add
			{
				Connection connection = this._connection;
				connection.LeapEndProfilingForThread = (Action<EndProfilingForThreadArgs>)Delegate.Combine(connection.LeapEndProfilingForThread, value);
			}
			remove
			{
				Connection connection = this._connection;
				connection.LeapEndProfilingForThread = (Action<EndProfilingForThreadArgs>)Delegate.Remove(connection.LeapEndProfilingForThread, value);
			}
		}

		// Token: 0x140000A3 RID: 163
		// (add) Token: 0x06002512 RID: 9490 RVA: 0x000D5BD5 File Offset: 0x000D3FD5
		// (remove) Token: 0x06002513 RID: 9491 RVA: 0x000D5BF3 File Offset: 0x000D3FF3
		public event Action<BeginProfilingBlockArgs> BeginProfilingBlock
		{
			add
			{
				Connection connection = this._connection;
				connection.LeapBeginProfilingBlock = (Action<BeginProfilingBlockArgs>)Delegate.Combine(connection.LeapBeginProfilingBlock, value);
			}
			remove
			{
				Connection connection = this._connection;
				connection.LeapBeginProfilingBlock = (Action<BeginProfilingBlockArgs>)Delegate.Remove(connection.LeapBeginProfilingBlock, value);
			}
		}

		// Token: 0x140000A4 RID: 164
		// (add) Token: 0x06002514 RID: 9492 RVA: 0x000D5C11 File Offset: 0x000D4011
		// (remove) Token: 0x06002515 RID: 9493 RVA: 0x000D5C2F File Offset: 0x000D402F
		public event Action<EndProfilingBlockArgs> EndProfilingBlock
		{
			add
			{
				Connection connection = this._connection;
				connection.LeapEndProfilingBlock = (Action<EndProfilingBlockArgs>)Delegate.Combine(connection.LeapEndProfilingBlock, value);
			}
			remove
			{
				Connection connection = this._connection;
				connection.LeapEndProfilingBlock = (Action<EndProfilingBlockArgs>)Delegate.Remove(connection.LeapEndProfilingBlock, value);
			}
		}

		// Token: 0x140000A5 RID: 165
		// (add) Token: 0x06002516 RID: 9494 RVA: 0x000D5C4D File Offset: 0x000D404D
		// (remove) Token: 0x06002517 RID: 9495 RVA: 0x000D5C6B File Offset: 0x000D406B
		public event EventHandler<PointMappingChangeEventArgs> PointMappingChange
		{
			add
			{
				Connection connection = this._connection;
				connection.LeapPointMappingChange = (EventHandler<PointMappingChangeEventArgs>)Delegate.Combine(connection.LeapPointMappingChange, value);
			}
			remove
			{
				Connection connection = this._connection;
				connection.LeapPointMappingChange = (EventHandler<PointMappingChangeEventArgs>)Delegate.Remove(connection.LeapPointMappingChange, value);
			}
		}

		// Token: 0x140000A6 RID: 166
		// (add) Token: 0x06002518 RID: 9496 RVA: 0x000D5C89 File Offset: 0x000D4089
		// (remove) Token: 0x06002519 RID: 9497 RVA: 0x000D5CA7 File Offset: 0x000D40A7
		public event EventHandler<HeadPoseEventArgs> HeadPoseChange
		{
			add
			{
				Connection connection = this._connection;
				connection.LeapHeadPoseChange = (EventHandler<HeadPoseEventArgs>)Delegate.Combine(connection.LeapHeadPoseChange, value);
			}
			remove
			{
				Connection connection = this._connection;
				connection.LeapHeadPoseChange = (EventHandler<HeadPoseEventArgs>)Delegate.Remove(connection.LeapHeadPoseChange, value);
			}
		}

		// Token: 0x0600251A RID: 9498 RVA: 0x000D5CC5 File Offset: 0x000D40C5
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600251B RID: 9499 RVA: 0x000D5CD4 File Offset: 0x000D40D4
		protected virtual void Dispose(bool disposing)
		{
			if (this._disposed)
			{
				return;
			}
			this._disposed = true;
		}

		// Token: 0x0600251C RID: 9500 RVA: 0x000D5CE9 File Offset: 0x000D40E9
		public void StartConnection()
		{
			this._connection.Start();
		}

		// Token: 0x0600251D RID: 9501 RVA: 0x000D5CF6 File Offset: 0x000D40F6
		public void StopConnection()
		{
			this._connection.Stop();
		}

		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x0600251E RID: 9502 RVA: 0x000D5D03 File Offset: 0x000D4103
		public bool IsServiceConnected
		{
			get
			{
				return this._connection.IsServiceConnected;
			}
		}

		// Token: 0x0600251F RID: 9503 RVA: 0x000D5D10 File Offset: 0x000D4110
		public void SetPolicy(Controller.PolicyFlag policy)
		{
			this._connection.SetPolicy(policy);
		}

		// Token: 0x06002520 RID: 9504 RVA: 0x000D5D1E File Offset: 0x000D411E
		public void ClearPolicy(Controller.PolicyFlag policy)
		{
			this._connection.ClearPolicy(policy);
		}

		// Token: 0x06002521 RID: 9505 RVA: 0x000D5D2C File Offset: 0x000D412C
		public bool IsPolicySet(Controller.PolicyFlag policy)
		{
			return this._connection.IsPolicySet(policy);
		}

		// Token: 0x06002522 RID: 9506 RVA: 0x000D5D3C File Offset: 0x000D413C
		public Frame Frame(int history = 0)
		{
			Frame frame = new Frame();
			this.Frame(frame, history);
			return frame;
		}

		// Token: 0x06002523 RID: 9507 RVA: 0x000D5D58 File Offset: 0x000D4158
		public void Frame(Frame toFill, int history = 0)
		{
			LEAP_TRACKING_EVENT leap_TRACKING_EVENT;
			this._connection.Frames.Get(out leap_TRACKING_EVENT, history);
			toFill.CopyFrom(ref leap_TRACKING_EVENT);
		}

		// Token: 0x06002524 RID: 9508 RVA: 0x000D5D84 File Offset: 0x000D4184
		public long FrameTimestamp(int history = 0)
		{
			LEAP_TRACKING_EVENT leap_TRACKING_EVENT;
			this._connection.Frames.Get(out leap_TRACKING_EVENT, history);
			return leap_TRACKING_EVENT.info.timestamp;
		}

		// Token: 0x06002525 RID: 9509 RVA: 0x000D5DB0 File Offset: 0x000D41B0
		public Frame GetTransformedFrame(LeapTransform trs, int history = 0)
		{
			return new Frame().CopyFrom(this.Frame(history)).Transform(trs);
		}

		// Token: 0x06002526 RID: 9510 RVA: 0x000D5DC9 File Offset: 0x000D41C9
		public Frame GetInterpolatedFrame(long time)
		{
			return this._connection.GetInterpolatedFrame(time);
		}

		// Token: 0x06002527 RID: 9511 RVA: 0x000D5DD7 File Offset: 0x000D41D7
		public void GetInterpolatedFrame(Frame toFill, long time)
		{
			this._connection.GetInterpolatedFrame(toFill, time);
		}

		// Token: 0x06002528 RID: 9512 RVA: 0x000D5DE6 File Offset: 0x000D41E6
		public LEAP_HEAD_POSE_EVENT GetInterpolatedHeadPose(long time)
		{
			return this._connection.GetInterpolatedHeadPose(time);
		}

		// Token: 0x06002529 RID: 9513 RVA: 0x000D5DF4 File Offset: 0x000D41F4
		public void GetInterpolatedHeadPose(ref LEAP_HEAD_POSE_EVENT toFill, long time)
		{
			this._connection.GetInterpolatedHeadPose(ref toFill, time);
		}

		// Token: 0x0600252A RID: 9514 RVA: 0x000D5E03 File Offset: 0x000D4203
		public void TelemetryProfiling(ref LEAP_TELEMETRY_DATA telemetryData)
		{
			this._connection.TelemetryProfiling(ref telemetryData);
		}

		// Token: 0x0600252B RID: 9515 RVA: 0x000D5E11 File Offset: 0x000D4211
		public ulong TelemetryGetNow()
		{
			return LeapC.TelemetryGetNow();
		}

		// Token: 0x0600252C RID: 9516 RVA: 0x000D5E18 File Offset: 0x000D4218
		public void GetPointMapping(ref PointMapping pointMapping)
		{
			this._connection.GetPointMapping(ref pointMapping);
		}

		// Token: 0x0600252D RID: 9517 RVA: 0x000D5E26 File Offset: 0x000D4226
		public void GetInterpolatedLeftRightTransform(long time, long sourceTime, int leftId, int rightId, out LeapTransform leftTransform, out LeapTransform rightTransform)
		{
			this._connection.GetInterpolatedLeftRightTransform(time, sourceTime, (long)leftId, (long)rightId, out leftTransform, out rightTransform);
		}

		// Token: 0x0600252E RID: 9518 RVA: 0x000D5E3E File Offset: 0x000D423E
		public void GetInterpolatedFrameFromTime(Frame toFill, long time, long sourceTime)
		{
			this._connection.GetInterpolatedFrameFromTime(toFill, time, sourceTime);
		}

		// Token: 0x0600252F RID: 9519 RVA: 0x000D5E4E File Offset: 0x000D424E
		public long Now()
		{
			return LeapC.GetNow();
		}

		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x06002530 RID: 9520 RVA: 0x000D5E55 File Offset: 0x000D4255
		public bool IsConnected
		{
			get
			{
				return this.IsServiceConnected && this.Devices.Count > 0;
			}
		}

		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x06002531 RID: 9521 RVA: 0x000D5E73 File Offset: 0x000D4273
		public Config Config
		{
			get
			{
				if (this._config == null)
				{
					this._config = new Config(this._connection.ConnectionKey);
				}
				return this._config;
			}
		}

		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x06002532 RID: 9522 RVA: 0x000D5E9C File Offset: 0x000D429C
		public DeviceList Devices
		{
			get
			{
				return this._connection.Devices;
			}
		}

		// Token: 0x06002533 RID: 9523 RVA: 0x000D5EA9 File Offset: 0x000D42A9
		public FailedDeviceList FailedDevices()
		{
			return this._connection.FailedDevices;
		}

		// Token: 0x06002534 RID: 9524 RVA: 0x000D5EB6 File Offset: 0x000D42B6
		protected virtual void OnInit(object sender, LeapEventArgs eventArgs)
		{
			this._hasInitialized = true;
		}

		// Token: 0x06002535 RID: 9525 RVA: 0x000D5EBF File Offset: 0x000D42BF
		protected virtual void OnConnect(object sender, ConnectionEventArgs eventArgs)
		{
			this._hasConnected = true;
		}

		// Token: 0x06002536 RID: 9526 RVA: 0x000D5EC8 File Offset: 0x000D42C8
		protected virtual void OnDisconnect(object sender, ConnectionLostEventArgs eventArgs)
		{
			this._hasInitialized = false;
			this._hasConnected = false;
		}

		// Token: 0x04001F0D RID: 7949
		private Connection _connection;

		// Token: 0x04001F0E RID: 7950
		private bool _disposed;

		// Token: 0x04001F0F RID: 7951
		private Config _config;

		// Token: 0x04001F10 RID: 7952
		private bool _hasInitialized;

		// Token: 0x04001F11 RID: 7953
		private EventHandler<LeapEventArgs> _init;

		// Token: 0x04001F12 RID: 7954
		private bool _hasConnected;

		// Token: 0x04001F13 RID: 7955
		private EventHandler<ConnectionEventArgs> _connect;

		// Token: 0x020005BA RID: 1466
		public enum PolicyFlag
		{
			// Token: 0x04001F15 RID: 7957
			POLICY_DEFAULT,
			// Token: 0x04001F16 RID: 7958
			POLICY_BACKGROUND_FRAMES,
			// Token: 0x04001F17 RID: 7959
			POLICY_IMAGES,
			// Token: 0x04001F18 RID: 7960
			POLICY_OPTIMIZE_HMD = 4,
			// Token: 0x04001F19 RID: 7961
			POLICY_ALLOW_PAUSE_RESUME = 8,
			// Token: 0x04001F1A RID: 7962
			POLICY_MAP_POINTS = 128
		}
	}
}
