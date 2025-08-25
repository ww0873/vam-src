using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Leap;

namespace LeapInternal
{
	// Token: 0x020005B8 RID: 1464
	public class Connection
	{
		// Token: 0x060024B5 RID: 9397 RVA: 0x000D4058 File Offset: 0x000D2458
		static Connection()
		{
			Connection.pInfo.size = (uint)Marshal.SizeOf(Connection.pInfo);
			long num = Marshal.OffsetOf(typeof(LEAP_HAND), "palm").ToInt64();
			Connection._handPositionOffset = Marshal.OffsetOf(typeof(LEAP_PALM), "position").ToInt64() + num;
			Connection._handOrientationOffset = Marshal.OffsetOf(typeof(LEAP_PALM), "orientation").ToInt64() + num;
		}

		// Token: 0x060024B6 RID: 9398 RVA: 0x000D4110 File Offset: 0x000D2510
		private Connection(int connectionKey)
		{
			this.ConnectionKey = connectionKey;
			this._leapConnection = IntPtr.Zero;
			this.Frames = new CircularObjectBuffer<LEAP_TRACKING_EVENT>(this._frameBufferLength);
		}

		// Token: 0x060024B7 RID: 9399 RVA: 0x000D418C File Offset: 0x000D258C
		public static Connection GetConnection(int connectionKey = 0)
		{
			Connection connection;
			if (!Connection.connectionDictionary.TryGetValue(connectionKey, out connection))
			{
				connection = new Connection(connectionKey);
				Connection.connectionDictionary.Add(connectionKey, connection);
			}
			return connection;
		}

		// Token: 0x17000453 RID: 1107
		// (get) Token: 0x060024B8 RID: 9400 RVA: 0x000D41BF File Offset: 0x000D25BF
		// (set) Token: 0x060024B9 RID: 9401 RVA: 0x000D41C7 File Offset: 0x000D25C7
		public int ConnectionKey
		{
			[CompilerGenerated]
			get
			{
				return this.<ConnectionKey>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<ConnectionKey>k__BackingField = value;
			}
		}

		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x060024BA RID: 9402 RVA: 0x000D41D0 File Offset: 0x000D25D0
		// (set) Token: 0x060024BB RID: 9403 RVA: 0x000D41D8 File Offset: 0x000D25D8
		public CircularObjectBuffer<LEAP_TRACKING_EVENT> Frames
		{
			[CompilerGenerated]
			get
			{
				return this.<Frames>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Frames>k__BackingField = value;
			}
		}

		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x060024BC RID: 9404 RVA: 0x000D41E1 File Offset: 0x000D25E1
		// (set) Token: 0x060024BD RID: 9405 RVA: 0x000D41E9 File Offset: 0x000D25E9
		public SynchronizationContext EventContext
		{
			[CompilerGenerated]
			get
			{
				return this.<EventContext>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<EventContext>k__BackingField = value;
			}
		}

		// Token: 0x14000091 RID: 145
		// (add) Token: 0x060024BE RID: 9406 RVA: 0x000D41F2 File Offset: 0x000D25F2
		// (remove) Token: 0x060024BF RID: 9407 RVA: 0x000D422E File Offset: 0x000D262E
		public event EventHandler<LeapEventArgs> LeapInit
		{
			add
			{
				this._leapInit = (EventHandler<LeapEventArgs>)Delegate.Combine(this._leapInit, value);
				if (this._leapConnection != IntPtr.Zero)
				{
					value(this, new LeapEventArgs(LeapEvent.EVENT_INIT));
				}
			}
			remove
			{
				this._leapInit = (EventHandler<LeapEventArgs>)Delegate.Remove(this._leapInit, value);
			}
		}

		// Token: 0x14000092 RID: 146
		// (add) Token: 0x060024C0 RID: 9408 RVA: 0x000D4247 File Offset: 0x000D2647
		// (remove) Token: 0x060024C1 RID: 9409 RVA: 0x000D4277 File Offset: 0x000D2677
		public event EventHandler<ConnectionEventArgs> LeapConnection
		{
			add
			{
				this._leapConnectionEvent = (EventHandler<ConnectionEventArgs>)Delegate.Combine(this._leapConnectionEvent, value);
				if (this.IsServiceConnected)
				{
					value(this, new ConnectionEventArgs());
				}
			}
			remove
			{
				this._leapConnectionEvent = (EventHandler<ConnectionEventArgs>)Delegate.Remove(this._leapConnectionEvent, value);
			}
		}

		// Token: 0x060024C2 RID: 9410 RVA: 0x000D4290 File Offset: 0x000D2690
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060024C3 RID: 9411 RVA: 0x000D429F File Offset: 0x000D269F
		protected virtual void Dispose(bool disposing)
		{
			if (this._disposed)
			{
				return;
			}
			if (disposing)
			{
			}
			this.Stop();
			LeapC.DestroyConnection(this._leapConnection);
			this._leapConnection = IntPtr.Zero;
			this._disposed = true;
		}

		// Token: 0x060024C4 RID: 9412 RVA: 0x000D42D8 File Offset: 0x000D26D8
		~Connection()
		{
			this.Dispose(false);
		}

		// Token: 0x060024C5 RID: 9413 RVA: 0x000D4308 File Offset: 0x000D2708
		public void Start()
		{
			if (this._isRunning)
			{
				return;
			}
			eLeapRS eLeapRS;
			if (this._leapConnection == IntPtr.Zero)
			{
				eLeapRS = LeapC.CreateConnection(out this._leapConnection);
				if (eLeapRS != eLeapRS.eLeapRS_Success || this._leapConnection == IntPtr.Zero)
				{
					this.reportAbnormalResults("LeapC CreateConnection call was ", eLeapRS);
					return;
				}
			}
			eLeapRS = LeapC.OpenConnection(this._leapConnection);
			if (eLeapRS != eLeapRS.eLeapRS_Success)
			{
				this.reportAbnormalResults("LeapC OpenConnection call was ", eLeapRS);
				return;
			}
			if (this._pLeapAllocator.allocate == null)
			{
				if (Connection.<>f__mg$cache0 == null)
				{
					Connection.<>f__mg$cache0 = new Allocate(MemoryManager.Pin);
				}
				this._pLeapAllocator.allocate = Connection.<>f__mg$cache0;
			}
			if (this._pLeapAllocator.deallocate == null)
			{
				if (Connection.<>f__mg$cache1 == null)
				{
					Connection.<>f__mg$cache1 = new Deallocate(MemoryManager.Unpin);
				}
				this._pLeapAllocator.deallocate = Connection.<>f__mg$cache1;
			}
			LeapC.SetAllocator(this._leapConnection, ref this._pLeapAllocator);
			this._isRunning = true;
			AppDomain.CurrentDomain.DomainUnload += this.<Start>m__0;
			this._polster = new Thread(new ThreadStart(this.processMessages));
			this._polster.Name = "LeapC Worker";
			this._polster.IsBackground = true;
			this._polster.Start();
		}

		// Token: 0x060024C6 RID: 9414 RVA: 0x000D4466 File Offset: 0x000D2866
		public void Stop()
		{
			if (!this._isRunning)
			{
				return;
			}
			this._isRunning = false;
			LeapC.CloseConnection(this._leapConnection);
			this._polster.Join();
		}

		// Token: 0x060024C7 RID: 9415 RVA: 0x000D4494 File Offset: 0x000D2894
		private void processMessages()
		{
			bool flag = false;
			try
			{
				this._leapInit.DispatchOnContext(this, this.EventContext, new LeapEventArgs(LeapEvent.EVENT_INIT));
				while (this._isRunning)
				{
					if (this.LeapBeginProfilingForThread != null && !flag)
					{
						this.LeapBeginProfilingForThread(new BeginProfilingForThreadArgs("Worker Thread", new string[]
						{
							"Handle Event"
						}));
						flag = true;
					}
					LEAP_CONNECTION_MESSAGE leap_CONNECTION_MESSAGE = default(LEAP_CONNECTION_MESSAGE);
					uint timeout = 150U;
					eLeapRS eLeapRS = LeapC.PollConnection(this._leapConnection, timeout, ref leap_CONNECTION_MESSAGE);
					if (eLeapRS != eLeapRS.eLeapRS_Success)
					{
						this.reportAbnormalResults("LeapC PollConnection call was ", eLeapRS);
					}
					else
					{
						if (this.LeapBeginProfilingBlock != null && flag)
						{
							this.LeapBeginProfilingBlock(new BeginProfilingBlockArgs("Handle Event"));
						}
						eLeapEventType type = leap_CONNECTION_MESSAGE.type;
						switch (type)
						{
						case eLeapEventType.eLeapEventType_Tracking:
						{
							LEAP_TRACKING_EVENT leap_TRACKING_EVENT;
							StructMarshal<LEAP_TRACKING_EVENT>.PtrToStruct(leap_CONNECTION_MESSAGE.eventStructPtr, out leap_TRACKING_EVENT);
							this.handleTrackingMessage(ref leap_TRACKING_EVENT);
							break;
						}
						default:
							switch (type)
							{
							case eLeapEventType.eLeapEventType_Connection:
							{
								LEAP_CONNECTION_EVENT leap_CONNECTION_EVENT;
								StructMarshal<LEAP_CONNECTION_EVENT>.PtrToStruct(leap_CONNECTION_MESSAGE.eventStructPtr, out leap_CONNECTION_EVENT);
								this.handleConnection(ref leap_CONNECTION_EVENT);
								break;
							}
							case eLeapEventType.eLeapEventType_ConnectionLost:
							{
								LEAP_CONNECTION_LOST_EVENT leap_CONNECTION_LOST_EVENT;
								StructMarshal<LEAP_CONNECTION_LOST_EVENT>.PtrToStruct(leap_CONNECTION_MESSAGE.eventStructPtr, out leap_CONNECTION_LOST_EVENT);
								this.handleConnectionLost(ref leap_CONNECTION_LOST_EVENT);
								break;
							}
							case eLeapEventType.eLeapEventType_Device:
							{
								LEAP_DEVICE_EVENT leap_DEVICE_EVENT;
								StructMarshal<LEAP_DEVICE_EVENT>.PtrToStruct(leap_CONNECTION_MESSAGE.eventStructPtr, out leap_DEVICE_EVENT);
								this.handleDevice(ref leap_DEVICE_EVENT);
								break;
							}
							case eLeapEventType.eLeapEventType_DeviceFailure:
							{
								LEAP_DEVICE_FAILURE_EVENT leap_DEVICE_FAILURE_EVENT;
								StructMarshal<LEAP_DEVICE_FAILURE_EVENT>.PtrToStruct(leap_CONNECTION_MESSAGE.eventStructPtr, out leap_DEVICE_FAILURE_EVENT);
								this.handleFailedDevice(ref leap_DEVICE_FAILURE_EVENT);
								break;
							}
							case eLeapEventType.eLeapEventType_Policy:
							{
								LEAP_POLICY_EVENT leap_POLICY_EVENT;
								StructMarshal<LEAP_POLICY_EVENT>.PtrToStruct(leap_CONNECTION_MESSAGE.eventStructPtr, out leap_POLICY_EVENT);
								this.handlePolicyChange(ref leap_POLICY_EVENT);
								break;
							}
							}
							break;
						case eLeapEventType.eLeapEventType_LogEvent:
						{
							LEAP_LOG_EVENT leap_LOG_EVENT;
							StructMarshal<LEAP_LOG_EVENT>.PtrToStruct(leap_CONNECTION_MESSAGE.eventStructPtr, out leap_LOG_EVENT);
							this.reportLogMessage(ref leap_LOG_EVENT);
							break;
						}
						case eLeapEventType.eLeapEventType_DeviceLost:
						{
							LEAP_DEVICE_EVENT leap_DEVICE_EVENT2;
							StructMarshal<LEAP_DEVICE_EVENT>.PtrToStruct(leap_CONNECTION_MESSAGE.eventStructPtr, out leap_DEVICE_EVENT2);
							this.handleLostDevice(ref leap_DEVICE_EVENT2);
							break;
						}
						case eLeapEventType.eLeapEventType_ConfigResponse:
							this.handleConfigResponse(ref leap_CONNECTION_MESSAGE);
							break;
						case eLeapEventType.eLeapEventType_ConfigChange:
						{
							LEAP_CONFIG_CHANGE_EVENT leap_CONFIG_CHANGE_EVENT;
							StructMarshal<LEAP_CONFIG_CHANGE_EVENT>.PtrToStruct(leap_CONNECTION_MESSAGE.eventStructPtr, out leap_CONFIG_CHANGE_EVENT);
							this.handleConfigChange(ref leap_CONFIG_CHANGE_EVENT);
							break;
						}
						case eLeapEventType.eLeapEventType_DroppedFrame:
						{
							LEAP_DROPPED_FRAME_EVENT leap_DROPPED_FRAME_EVENT;
							StructMarshal<LEAP_DROPPED_FRAME_EVENT>.PtrToStruct(leap_CONNECTION_MESSAGE.eventStructPtr, out leap_DROPPED_FRAME_EVENT);
							this.handleDroppedFrame(ref leap_DROPPED_FRAME_EVENT);
							break;
						}
						case eLeapEventType.eLeapEventType_Image:
						{
							LEAP_IMAGE_EVENT leap_IMAGE_EVENT;
							StructMarshal<LEAP_IMAGE_EVENT>.PtrToStruct(leap_CONNECTION_MESSAGE.eventStructPtr, out leap_IMAGE_EVENT);
							this.handleImage(ref leap_IMAGE_EVENT);
							break;
						}
						case eLeapEventType.eLeapEventType_PointMappingChange:
						{
							LEAP_POINT_MAPPING_CHANGE_EVENT leap_POINT_MAPPING_CHANGE_EVENT;
							StructMarshal<LEAP_POINT_MAPPING_CHANGE_EVENT>.PtrToStruct(leap_CONNECTION_MESSAGE.eventStructPtr, out leap_POINT_MAPPING_CHANGE_EVENT);
							this.handlePointMappingChange(ref leap_POINT_MAPPING_CHANGE_EVENT);
							break;
						}
						case eLeapEventType.eLeapEventType_HeadPose:
						{
							LEAP_HEAD_POSE_EVENT leap_HEAD_POSE_EVENT;
							StructMarshal<LEAP_HEAD_POSE_EVENT>.PtrToStruct(leap_CONNECTION_MESSAGE.eventStructPtr, out leap_HEAD_POSE_EVENT);
							this.handleHeadPoseChange(ref leap_HEAD_POSE_EVENT);
							break;
						}
						}
						if (this.LeapEndProfilingBlock != null && flag)
						{
							this.LeapEndProfilingBlock(new EndProfilingBlockArgs("Handle Event"));
						}
					}
				}
			}
			catch (Exception arg)
			{
				Logger.Log("Exception: " + arg);
				this._isRunning = false;
			}
			finally
			{
				if (this.LeapEndProfilingForThread != null && flag)
				{
					this.LeapEndProfilingForThread(default(EndProfilingForThreadArgs));
				}
			}
		}

		// Token: 0x060024C8 RID: 9416 RVA: 0x000D47E0 File Offset: 0x000D2BE0
		private void handleTrackingMessage(ref LEAP_TRACKING_EVENT trackingMsg)
		{
			this.Frames.Put(ref trackingMsg);
			if (this.LeapFrame != null)
			{
				this.LeapFrame.DispatchOnContext(this, this.EventContext, new FrameEventArgs(new Leap.Frame().CopyFrom(ref trackingMsg)));
			}
		}

		// Token: 0x060024C9 RID: 9417 RVA: 0x000D481C File Offset: 0x000D2C1C
		public ulong GetInterpolatedFrameSize(long time)
		{
			ulong result = 0UL;
			eLeapRS frameSize = LeapC.GetFrameSize(this._leapConnection, time, out result);
			this.reportAbnormalResults("LeapC get interpolated frame call was ", frameSize);
			return result;
		}

		// Token: 0x060024CA RID: 9418 RVA: 0x000D4848 File Offset: 0x000D2C48
		public void GetInterpolatedFrame(Leap.Frame toFill, long time)
		{
			ulong interpolatedFrameSize = this.GetInterpolatedFrameSize(time);
			IntPtr intPtr = Marshal.AllocHGlobal((int)interpolatedFrameSize);
			eLeapRS eLeapRS = LeapC.InterpolateFrame(this._leapConnection, time, intPtr, interpolatedFrameSize);
			this.reportAbnormalResults("LeapC get interpolated frame call was ", eLeapRS);
			if (eLeapRS == eLeapRS.eLeapRS_Success)
			{
				LEAP_TRACKING_EVENT leap_TRACKING_EVENT;
				StructMarshal<LEAP_TRACKING_EVENT>.PtrToStruct(intPtr, out leap_TRACKING_EVENT);
				toFill.CopyFrom(ref leap_TRACKING_EVENT);
			}
			Marshal.FreeHGlobal(intPtr);
		}

		// Token: 0x060024CB RID: 9419 RVA: 0x000D48A0 File Offset: 0x000D2CA0
		public void GetInterpolatedFrameFromTime(Leap.Frame toFill, long time, long sourceTime)
		{
			ulong interpolatedFrameSize = this.GetInterpolatedFrameSize(time);
			IntPtr intPtr = Marshal.AllocHGlobal((int)interpolatedFrameSize);
			eLeapRS eLeapRS = LeapC.InterpolateFrameFromTime(this._leapConnection, time, sourceTime, intPtr, interpolatedFrameSize);
			this.reportAbnormalResults("LeapC get interpolated frame from time call was ", eLeapRS);
			if (eLeapRS == eLeapRS.eLeapRS_Success)
			{
				LEAP_TRACKING_EVENT leap_TRACKING_EVENT;
				StructMarshal<LEAP_TRACKING_EVENT>.PtrToStruct(intPtr, out leap_TRACKING_EVENT);
				toFill.CopyFrom(ref leap_TRACKING_EVENT);
			}
			Marshal.FreeHGlobal(intPtr);
		}

		// Token: 0x060024CC RID: 9420 RVA: 0x000D48F8 File Offset: 0x000D2CF8
		public Leap.Frame GetInterpolatedFrame(long time)
		{
			Leap.Frame frame = new Leap.Frame();
			this.GetInterpolatedFrame(frame, time);
			return frame;
		}

		// Token: 0x060024CD RID: 9421 RVA: 0x000D4914 File Offset: 0x000D2D14
		public void GetInterpolatedHeadPose(ref LEAP_HEAD_POSE_EVENT toFill, long time)
		{
			eLeapRS result = LeapC.InterpolateHeadPose(this._leapConnection, time, ref toFill);
			this.reportAbnormalResults("LeapC get interpolated head pose call was ", result);
		}

		// Token: 0x060024CE RID: 9422 RVA: 0x000D493C File Offset: 0x000D2D3C
		public LEAP_HEAD_POSE_EVENT GetInterpolatedHeadPose(long time)
		{
			LEAP_HEAD_POSE_EVENT result = default(LEAP_HEAD_POSE_EVENT);
			this.GetInterpolatedHeadPose(ref result, time);
			return result;
		}

		// Token: 0x060024CF RID: 9423 RVA: 0x000D495C File Offset: 0x000D2D5C
		public void GetInterpolatedLeftRightTransform(long time, long sourceTime, long leftId, long rightId, out LeapTransform leftTransform, out LeapTransform rightTransform)
		{
			leftTransform = LeapTransform.Identity;
			rightTransform = LeapTransform.Identity;
			ulong interpolatedFrameSize = this.GetInterpolatedFrameSize(time);
			IntPtr intPtr = Marshal.AllocHGlobal((int)interpolatedFrameSize);
			eLeapRS eLeapRS = LeapC.InterpolateFrameFromTime(this._leapConnection, time, sourceTime, intPtr, interpolatedFrameSize);
			this.reportAbnormalResults("LeapC get interpolated frame from time call was ", eLeapRS);
			if (eLeapRS == eLeapRS.eLeapRS_Success)
			{
				LEAP_TRACKING_EVENT leap_TRACKING_EVENT;
				StructMarshal<LEAP_TRACKING_EVENT>.PtrToStruct(intPtr, out leap_TRACKING_EVENT);
				long num = leap_TRACKING_EVENT.pHands.ToInt64();
				long num2 = num + Connection._handIdOffset;
				long num3 = num + Connection._handPositionOffset;
				long num4 = num + Connection._handOrientationOffset;
				int size = StructMarshal<LEAP_HAND>.Size;
				uint nHands = leap_TRACKING_EVENT.nHands;
				while (nHands-- != 0U)
				{
					int num5 = Marshal.ReadInt32(new IntPtr(num2));
					LEAP_VECTOR leap_VECTOR;
					StructMarshal<LEAP_VECTOR>.PtrToStruct(new IntPtr(num3), out leap_VECTOR);
					LEAP_QUATERNION leap_QUATERNION;
					StructMarshal<LEAP_QUATERNION>.PtrToStruct(new IntPtr(num4), out leap_QUATERNION);
					LeapTransform leapTransform = new LeapTransform(leap_VECTOR.ToLeapVector(), leap_QUATERNION.ToLeapQuaternion());
					if ((long)num5 == leftId)
					{
						leftTransform = leapTransform;
					}
					else if ((long)num5 == rightId)
					{
						rightTransform = leapTransform;
					}
					num2 += (long)size;
					num3 += (long)size;
					num4 += (long)size;
				}
			}
			Marshal.FreeHGlobal(intPtr);
		}

		// Token: 0x060024D0 RID: 9424 RVA: 0x000D4A8F File Offset: 0x000D2E8F
		private void handleConnection(ref LEAP_CONNECTION_EVENT connectionMsg)
		{
			if (this._leapConnectionEvent != null)
			{
				this._leapConnectionEvent.DispatchOnContext(this, this.EventContext, new ConnectionEventArgs());
			}
		}

		// Token: 0x060024D1 RID: 9425 RVA: 0x000D4AB3 File Offset: 0x000D2EB3
		private void handleConnectionLost(ref LEAP_CONNECTION_LOST_EVENT connectionMsg)
		{
			if (this.LeapConnectionLost != null)
			{
				this.LeapConnectionLost.DispatchOnContext(this, this.EventContext, new ConnectionLostEventArgs());
			}
		}

		// Token: 0x060024D2 RID: 9426 RVA: 0x000D4AD8 File Offset: 0x000D2ED8
		private void handleDevice(ref LEAP_DEVICE_EVENT deviceMsg)
		{
			IntPtr handle = deviceMsg.device.handle;
			if (handle == IntPtr.Zero)
			{
				return;
			}
			LEAP_DEVICE_INFO leap_DEVICE_INFO = default(LEAP_DEVICE_INFO);
			IntPtr hDevice;
			eLeapRS eLeapRS = LeapC.OpenDevice(deviceMsg.device, out hDevice);
			if (eLeapRS != eLeapRS.eLeapRS_Success)
			{
				return;
			}
			leap_DEVICE_INFO.serial = IntPtr.Zero;
			leap_DEVICE_INFO.size = (uint)Marshal.SizeOf(leap_DEVICE_INFO);
			eLeapRS = LeapC.GetDeviceInfo(hDevice, ref leap_DEVICE_INFO);
			if (eLeapRS != eLeapRS.eLeapRS_Success)
			{
				return;
			}
			leap_DEVICE_INFO.serial = Marshal.AllocCoTaskMem((int)leap_DEVICE_INFO.serial_length);
			if (LeapC.GetDeviceInfo(hDevice, ref leap_DEVICE_INFO) == eLeapRS.eLeapRS_Success)
			{
				Device device = new Device(handle, leap_DEVICE_INFO.h_fov, leap_DEVICE_INFO.v_fov, leap_DEVICE_INFO.range / 1000f, leap_DEVICE_INFO.baseline / 1000f, (Device.DeviceType)leap_DEVICE_INFO.type, leap_DEVICE_INFO.status == eLeapDeviceStatus.eLeapDeviceStatus_Streaming, Marshal.PtrToStringAnsi(leap_DEVICE_INFO.serial));
				Marshal.FreeCoTaskMem(leap_DEVICE_INFO.serial);
				this._devices.AddOrUpdate(device);
				if (this.LeapDevice != null)
				{
					this.LeapDevice.DispatchOnContext(this, this.EventContext, new DeviceEventArgs(device));
				}
			}
		}

		// Token: 0x060024D3 RID: 9427 RVA: 0x000D4C00 File Offset: 0x000D3000
		private void handleLostDevice(ref LEAP_DEVICE_EVENT deviceMsg)
		{
			Device device = this._devices.FindDeviceByHandle(deviceMsg.device.handle);
			if (device != null)
			{
				this._devices.Remove(device);
				if (this.LeapDeviceLost != null)
				{
					this.LeapDeviceLost.DispatchOnContext(this, this.EventContext, new DeviceEventArgs(device));
				}
			}
		}

		// Token: 0x060024D4 RID: 9428 RVA: 0x000D4C5C File Offset: 0x000D305C
		private void handleFailedDevice(ref LEAP_DEVICE_FAILURE_EVENT deviceMsg)
		{
			string serial = "Unavailable";
			eLeapDeviceStatus status = deviceMsg.status;
			string message;
			switch (status + 402587647U)
			{
			case (eLeapDeviceStatus)0U:
				message = "Bad Calibration. Device failed because of a bad calibration record.";
				break;
			case eLeapDeviceStatus.eLeapDeviceStatus_Streaming:
				message = "Bad Firmware. Device failed because of a firmware error.";
				break;
			case eLeapDeviceStatus.eLeapDeviceStatus_Paused:
				message = "Bad Transport. Device failed because of a USB communication error.";
				break;
			case (eLeapDeviceStatus)3U:
				message = "Bad Control Interface. Device failed because of a USB control interface error.";
				break;
			default:
				message = "Device failed for an unknown reason";
				break;
			}
			Device device = this._devices.FindDeviceByHandle(deviceMsg.hDevice);
			if (device != null)
			{
				this._devices.Remove(device);
			}
			if (this.LeapDeviceFailure != null)
			{
				this.LeapDeviceFailure.DispatchOnContext(this, this.EventContext, new DeviceFailureEventArgs((uint)deviceMsg.status, message, serial));
			}
		}

		// Token: 0x060024D5 RID: 9429 RVA: 0x000D4D20 File Offset: 0x000D3120
		private void handleConfigChange(ref LEAP_CONFIG_CHANGE_EVENT configEvent)
		{
			string empty = string.Empty;
			this._configRequests.TryGetValue(configEvent.requestId, out empty);
			if (empty != null)
			{
				this._configRequests.Remove(configEvent.requestId);
			}
			if (this.LeapConfigChange != null)
			{
				this.LeapConfigChange.DispatchOnContext(this, this.EventContext, new ConfigChangeEventArgs(empty, configEvent.status, configEvent.requestId));
			}
		}

		// Token: 0x060024D6 RID: 9430 RVA: 0x000D4D94 File Offset: 0x000D3194
		private void handleConfigResponse(ref LEAP_CONNECTION_MESSAGE configMsg)
		{
			LEAP_CONFIG_RESPONSE_EVENT leap_CONFIG_RESPONSE_EVENT;
			StructMarshal<LEAP_CONFIG_RESPONSE_EVENT>.PtrToStruct(configMsg.eventStructPtr, out leap_CONFIG_RESPONSE_EVENT);
			string empty = string.Empty;
			this._configRequests.TryGetValue(leap_CONFIG_RESPONSE_EVENT.requestId, out empty);
			if (empty != null)
			{
				this._configRequests.Remove(leap_CONFIG_RESPONSE_EVENT.requestId);
			}
			uint requestId = leap_CONFIG_RESPONSE_EVENT.requestId;
			Config.ValueType dataType;
			object value;
			if (leap_CONFIG_RESPONSE_EVENT.value.type != eLeapValueType.eLeapValueType_String)
			{
				switch (leap_CONFIG_RESPONSE_EVENT.value.type)
				{
				case eLeapValueType.eLeapValueType_Boolean:
					dataType = Config.ValueType.TYPE_BOOLEAN;
					value = leap_CONFIG_RESPONSE_EVENT.value.boolValue;
					break;
				case eLeapValueType.eLeapValueType_Int32:
					dataType = Config.ValueType.TYPE_INT32;
					value = leap_CONFIG_RESPONSE_EVENT.value.intValue;
					break;
				case eLeapValueType.eLeapValueType_Float:
					dataType = Config.ValueType.TYPE_FLOAT;
					value = leap_CONFIG_RESPONSE_EVENT.value.floatValue;
					break;
				default:
					dataType = Config.ValueType.TYPE_UNKNOWN;
					value = new object();
					break;
				}
			}
			else
			{
				LEAP_CONFIG_RESPONSE_EVENT_WITH_REF_TYPE leap_CONFIG_RESPONSE_EVENT_WITH_REF_TYPE;
				StructMarshal<LEAP_CONFIG_RESPONSE_EVENT_WITH_REF_TYPE>.PtrToStruct(configMsg.eventStructPtr, out leap_CONFIG_RESPONSE_EVENT_WITH_REF_TYPE);
				dataType = Config.ValueType.TYPE_STRING;
				value = leap_CONFIG_RESPONSE_EVENT_WITH_REF_TYPE.value.stringValue;
			}
			SetConfigResponseEventArgs eventArgs = new SetConfigResponseEventArgs(empty, dataType, value, requestId);
			if (this.LeapConfigResponse != null)
			{
				this.LeapConfigResponse.DispatchOnContext(this, this.EventContext, eventArgs);
			}
		}

		// Token: 0x060024D7 RID: 9431 RVA: 0x000D4ECC File Offset: 0x000D32CC
		private void reportLogMessage(ref LEAP_LOG_EVENT logMsg)
		{
			if (this.LeapLogEvent != null)
			{
				this.LeapLogEvent.DispatchOnContext(this, this.EventContext, new LogEventArgs(this.publicSeverity(logMsg.severity), logMsg.timestamp, Marshal.PtrToStringAnsi(logMsg.message)));
			}
		}

		// Token: 0x060024D8 RID: 9432 RVA: 0x000D4F18 File Offset: 0x000D3318
		private MessageSeverity publicSeverity(eLeapLogSeverity leapCSeverity)
		{
			switch (leapCSeverity)
			{
			case eLeapLogSeverity.eLeapLogSeverity_Unknown:
				return MessageSeverity.MESSAGE_UNKNOWN;
			case eLeapLogSeverity.eLeapLogSeverity_Critical:
				return MessageSeverity.MESSAGE_CRITICAL;
			case eLeapLogSeverity.eLeapLogSeverity_Warning:
				return MessageSeverity.MESSAGE_WARNING;
			case eLeapLogSeverity.eLeapLogSeverity_Information:
				return MessageSeverity.MESSAGE_INFORMATION;
			default:
				return MessageSeverity.MESSAGE_UNKNOWN;
			}
		}

		// Token: 0x060024D9 RID: 9433 RVA: 0x000D4F3E File Offset: 0x000D333E
		private void handlePointMappingChange(ref LEAP_POINT_MAPPING_CHANGE_EVENT pointMapping)
		{
			if (this.LeapPointMappingChange != null)
			{
				this.LeapPointMappingChange.DispatchOnContext(this, this.EventContext, new PointMappingChangeEventArgs(pointMapping.frame_id, pointMapping.timestamp, pointMapping.nPoints));
			}
		}

		// Token: 0x060024DA RID: 9434 RVA: 0x000D4F74 File Offset: 0x000D3374
		private void handleDroppedFrame(ref LEAP_DROPPED_FRAME_EVENT droppedFrame)
		{
			if (this.LeapDroppedFrame != null)
			{
				this.LeapDroppedFrame.DispatchOnContext(this, this.EventContext, new DroppedFrameEventArgs(droppedFrame.frame_id, droppedFrame.reason));
			}
		}

		// Token: 0x060024DB RID: 9435 RVA: 0x000D4FA4 File Offset: 0x000D33A4
		private void handleHeadPoseChange(ref LEAP_HEAD_POSE_EVENT headPose)
		{
			if (this.LeapHeadPoseChange != null)
			{
				this.LeapHeadPoseChange.DispatchOnContext(this, this.EventContext, new HeadPoseEventArgs(headPose.head_position, headPose.head_orientation));
			}
		}

		// Token: 0x060024DC RID: 9436 RVA: 0x000D4FD4 File Offset: 0x000D33D4
		private DistortionData createDistortionData(LEAP_IMAGE image, Image.CameraType camera)
		{
			DistortionData distortionData = new DistortionData();
			distortionData.Version = image.matrix_version;
			distortionData.Width = (float)LeapC.DistortionSize;
			distortionData.Height = (float)LeapC.DistortionSize;
			distortionData.Data = new float[(int)(distortionData.Width * distortionData.Height * 2f)];
			Marshal.Copy(image.distortionMatrix, distortionData.Data, 0, distortionData.Data.Length);
			if (this.LeapDistortionChange != null)
			{
				this.LeapDistortionChange.DispatchOnContext(this, this.EventContext, new DistortionEventArgs(distortionData, camera));
			}
			return distortionData;
		}

		// Token: 0x060024DD RID: 9437 RVA: 0x000D506C File Offset: 0x000D346C
		private void handleImage(ref LEAP_IMAGE_EVENT imageMsg)
		{
			if (this.LeapImage != null)
			{
				if (this._currentLeftDistortionData.Version != imageMsg.leftImage.matrix_version || !this._currentLeftDistortionData.IsValid)
				{
					this._currentLeftDistortionData = this.createDistortionData(imageMsg.leftImage, Image.CameraType.LEFT);
				}
				if (this._currentRightDistortionData.Version != imageMsg.rightImage.matrix_version || !this._currentRightDistortionData.IsValid)
				{
					this._currentRightDistortionData = this.createDistortionData(imageMsg.rightImage, Image.CameraType.RIGHT);
				}
				ImageData leftImage = new ImageData(Image.CameraType.LEFT, imageMsg.leftImage, this._currentLeftDistortionData);
				ImageData rightImage = new ImageData(Image.CameraType.RIGHT, imageMsg.rightImage, this._currentRightDistortionData);
				Image image = new Image(imageMsg.info.frame_id, imageMsg.info.timestamp, leftImage, rightImage);
				this.LeapImage.DispatchOnContext(this, this.EventContext, new ImageEventArgs(image));
			}
		}

		// Token: 0x060024DE RID: 9438 RVA: 0x000D515C File Offset: 0x000D355C
		private void handlePolicyChange(ref LEAP_POLICY_EVENT policyMsg)
		{
			if (this.LeapPolicyChange != null)
			{
				this.LeapPolicyChange.DispatchOnContext(this, this.EventContext, new PolicyEventArgs((ulong)policyMsg.current_policy, this._activePolicies));
			}
			this._activePolicies = (ulong)policyMsg.current_policy;
			if (this._activePolicies != this._requestedPolicies)
			{
			}
		}

		// Token: 0x060024DF RID: 9439 RVA: 0x000D51B8 File Offset: 0x000D35B8
		public void SetPolicy(Controller.PolicyFlag policy)
		{
			ulong num = (ulong)this.flagForPolicy(policy);
			this._requestedPolicies |= num;
			num = this._requestedPolicies;
			ulong clear = ~this._requestedPolicies;
			eLeapRS result = LeapC.SetPolicyFlags(this._leapConnection, num, clear);
			this.reportAbnormalResults("LeapC SetPolicyFlags call was ", result);
		}

		// Token: 0x060024E0 RID: 9440 RVA: 0x000D5208 File Offset: 0x000D3608
		public void ClearPolicy(Controller.PolicyFlag policy)
		{
			ulong num = (ulong)this.flagForPolicy(policy);
			this._requestedPolicies &= ~num;
			eLeapRS result = LeapC.SetPolicyFlags(this._leapConnection, this._requestedPolicies, ~this._requestedPolicies);
			this.reportAbnormalResults("LeapC SetPolicyFlags call was ", result);
		}

		// Token: 0x060024E1 RID: 9441 RVA: 0x000D5254 File Offset: 0x000D3654
		private eLeapPolicyFlag flagForPolicy(Controller.PolicyFlag singlePolicy)
		{
			switch (singlePolicy)
			{
			case Controller.PolicyFlag.POLICY_DEFAULT:
				return (eLeapPolicyFlag)0U;
			case Controller.PolicyFlag.POLICY_BACKGROUND_FRAMES:
				return eLeapPolicyFlag.eLeapPolicyFlag_BackgroundFrames;
			case Controller.PolicyFlag.POLICY_IMAGES:
				return eLeapPolicyFlag.eLeapPolicyFlag_Images;
			default:
				if (singlePolicy != Controller.PolicyFlag.POLICY_MAP_POINTS)
				{
					return (eLeapPolicyFlag)0U;
				}
				return eLeapPolicyFlag.eLeapPolicyFlag_MapPoints;
			case Controller.PolicyFlag.POLICY_OPTIMIZE_HMD:
				return eLeapPolicyFlag.eLeapPolicyFlag_OptimizeHMD;
			case Controller.PolicyFlag.POLICY_ALLOW_PAUSE_RESUME:
				return eLeapPolicyFlag.eLeapPolicyFlag_AllowPauseResume;
			}
		}

		// Token: 0x060024E2 RID: 9442 RVA: 0x000D52AC File Offset: 0x000D36AC
		public bool IsPolicySet(Controller.PolicyFlag policy)
		{
			ulong num = (ulong)this.flagForPolicy(policy);
			return (this._activePolicies & num) == num;
		}

		// Token: 0x060024E3 RID: 9443 RVA: 0x000D52D0 File Offset: 0x000D36D0
		public uint GetConfigValue(string config_key)
		{
			uint num = 0U;
			eLeapRS result = LeapC.RequestConfigValue(this._leapConnection, config_key, out num);
			this.reportAbnormalResults("LeapC RequestConfigValue call was ", result);
			this._configRequests[num] = config_key;
			return num;
		}

		// Token: 0x060024E4 RID: 9444 RVA: 0x000D5308 File Offset: 0x000D3708
		public uint SetConfigValue<T>(string config_key, T value) where T : IConvertible
		{
			uint num = 0U;
			Type type = value.GetType();
			eLeapRS result;
			if (type == typeof(bool))
			{
				result = LeapC.SaveConfigValue(this._leapConnection, config_key, Convert.ToBoolean(value), out num);
			}
			else if (type == typeof(int))
			{
				result = LeapC.SaveConfigValue(this._leapConnection, config_key, Convert.ToInt32(value), out num);
			}
			else if (type == typeof(float))
			{
				result = LeapC.SaveConfigValue(this._leapConnection, config_key, Convert.ToSingle(value), out num);
			}
			else
			{
				if (type != typeof(string))
				{
					throw new ArgumentException("Only boolean, Int32, float, and string types are supported.");
				}
				result = LeapC.SaveConfigValue(this._leapConnection, config_key, Convert.ToString(value), out num);
			}
			this.reportAbnormalResults("LeapC SaveConfigValue call was ", result);
			this._configRequests[num] = config_key;
			return num;
		}

		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x060024E5 RID: 9445 RVA: 0x000D5408 File Offset: 0x000D3808
		public bool IsServiceConnected
		{
			get
			{
				if (this._leapConnection == IntPtr.Zero)
				{
					return false;
				}
				eLeapRS connectionInfo = LeapC.GetConnectionInfo(this._leapConnection, ref Connection.pInfo);
				this.reportAbnormalResults("LeapC GetConnectionInfo call was ", connectionInfo);
				return Connection.pInfo.status == eLeapConnectionStatus.eLeapConnectionStatus_Connected;
			}
		}

		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x060024E6 RID: 9446 RVA: 0x000D545C File Offset: 0x000D385C
		public DeviceList Devices
		{
			get
			{
				if (this._devices == null)
				{
					this._devices = new DeviceList();
				}
				return this._devices;
			}
		}

		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x060024E7 RID: 9447 RVA: 0x000D547A File Offset: 0x000D387A
		public FailedDeviceList FailedDevices
		{
			get
			{
				if (this._failedDevices == null)
				{
					this._failedDevices = new FailedDeviceList();
				}
				return this._failedDevices;
			}
		}

		// Token: 0x060024E8 RID: 9448 RVA: 0x000D5498 File Offset: 0x000D3898
		public Vector PixelToRectilinear(Image.CameraType camera, Vector pixel)
		{
			LEAP_VECTOR pixel2 = new LEAP_VECTOR(pixel);
			LEAP_VECTOR leap_VECTOR = LeapC.LeapPixelToRectilinear(this._leapConnection, (camera != Image.CameraType.LEFT) ? eLeapPerspectiveType.eLeapPerspectiveType_stereo_right : eLeapPerspectiveType.eLeapPerspectiveType_stereo_left, pixel2);
			return new Vector(leap_VECTOR.x, leap_VECTOR.y, leap_VECTOR.z);
		}

		// Token: 0x060024E9 RID: 9449 RVA: 0x000D54E4 File Offset: 0x000D38E4
		public Vector RectilinearToPixel(Image.CameraType camera, Vector ray)
		{
			LEAP_VECTOR rectilinear = new LEAP_VECTOR(ray);
			LEAP_VECTOR leap_VECTOR = LeapC.LeapRectilinearToPixel(this._leapConnection, (camera != Image.CameraType.LEFT) ? eLeapPerspectiveType.eLeapPerspectiveType_stereo_right : eLeapPerspectiveType.eLeapPerspectiveType_stereo_left, rectilinear);
			return new Vector(leap_VECTOR.x, leap_VECTOR.y, leap_VECTOR.z);
		}

		// Token: 0x060024EA RID: 9450 RVA: 0x000D5530 File Offset: 0x000D3930
		public void TelemetryProfiling(ref LEAP_TELEMETRY_DATA telemetryData)
		{
			eLeapRS result = LeapC.LeapTelemetryProfiling(this._leapConnection, ref telemetryData);
			this.reportAbnormalResults("LeapC TelemetryProfiling call was ", result);
		}

		// Token: 0x060024EB RID: 9451 RVA: 0x000D5558 File Offset: 0x000D3958
		public void GetPointMapping(ref PointMapping pm)
		{
			ulong num = 0UL;
			IntPtr intPtr = IntPtr.Zero;
			eLeapRS pointMapping;
			for (;;)
			{
				pointMapping = LeapC.GetPointMapping(this._leapConnection, intPtr, ref num);
				if (pointMapping != (eLeapRS)3791716355U)
				{
					break;
				}
				if (intPtr != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr);
				}
				intPtr = Marshal.AllocHGlobal((int)num);
			}
			this.reportAbnormalResults("LeapC get point mapping call was ", pointMapping);
			if (pointMapping != eLeapRS.eLeapRS_Success)
			{
				pm.points = null;
				pm.ids = null;
				return;
			}
			LEAP_POINT_MAPPING leap_POINT_MAPPING;
			StructMarshal<LEAP_POINT_MAPPING>.PtrToStruct(intPtr, out leap_POINT_MAPPING);
			int nPoints = (int)leap_POINT_MAPPING.nPoints;
			pm.frameId = leap_POINT_MAPPING.frame_id;
			pm.timestamp = leap_POINT_MAPPING.timestamp;
			pm.points = new Vector[nPoints];
			pm.ids = new uint[nPoints];
			float[] array = new float[3 * nPoints];
			int[] array2 = new int[nPoints];
			Marshal.Copy(leap_POINT_MAPPING.points, array, 0, 3 * nPoints);
			Marshal.Copy(leap_POINT_MAPPING.ids, array2, 0, nPoints);
			int num2 = 0;
			for (int i = 0; i < nPoints; i++)
			{
				pm.points[i].x = array[num2++];
				pm.points[i].y = array[num2++];
				pm.points[i].z = array[num2++];
				pm.ids[i] = (uint)array2[i];
			}
			Marshal.FreeHGlobal(intPtr);
		}

		// Token: 0x060024EC RID: 9452 RVA: 0x000D56D8 File Offset: 0x000D3AD8
		private void reportAbnormalResults(string context, eLeapRS result)
		{
			if (result != eLeapRS.eLeapRS_Success && result != this._lastResult)
			{
				string message = context + " " + result;
				if (this.LeapLogEvent != null)
				{
					this.LeapLogEvent.DispatchOnContext(this, this.EventContext, new LogEventArgs(MessageSeverity.MESSAGE_CRITICAL, LeapC.GetNow(), message));
				}
				this._lastResult = result;
			}
		}

		// Token: 0x060024ED RID: 9453 RVA: 0x000D5739 File Offset: 0x000D3B39
		[CompilerGenerated]
		private void <Start>m__0(object arg1, EventArgs arg2)
		{
			this.Dispose(true);
		}

		// Token: 0x04001EE0 RID: 7904
		private static Dictionary<int, Connection> connectionDictionary = new Dictionary<int, Connection>();

		// Token: 0x04001EE1 RID: 7905
		private static long _handIdOffset = Marshal.OffsetOf(typeof(LEAP_HAND), "id").ToInt64();

		// Token: 0x04001EE2 RID: 7906
		private static long _handPositionOffset;

		// Token: 0x04001EE3 RID: 7907
		private static long _handOrientationOffset;

		// Token: 0x04001EE4 RID: 7908
		private static LEAP_CONNECTION_INFO pInfo;

		// Token: 0x04001EE5 RID: 7909
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private int <ConnectionKey>k__BackingField;

		// Token: 0x04001EE6 RID: 7910
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private CircularObjectBuffer<LEAP_TRACKING_EVENT> <Frames>k__BackingField;

		// Token: 0x04001EE7 RID: 7911
		private DeviceList _devices = new DeviceList();

		// Token: 0x04001EE8 RID: 7912
		private FailedDeviceList _failedDevices;

		// Token: 0x04001EE9 RID: 7913
		private DistortionData _currentLeftDistortionData = new DistortionData();

		// Token: 0x04001EEA RID: 7914
		private DistortionData _currentRightDistortionData = new DistortionData();

		// Token: 0x04001EEB RID: 7915
		private int _frameBufferLength = 60;

		// Token: 0x04001EEC RID: 7916
		private IntPtr _leapConnection;

		// Token: 0x04001EED RID: 7917
		private bool _isRunning;

		// Token: 0x04001EEE RID: 7918
		private Thread _polster;

		// Token: 0x04001EEF RID: 7919
		private ulong _requestedPolicies;

		// Token: 0x04001EF0 RID: 7920
		private ulong _activePolicies;

		// Token: 0x04001EF1 RID: 7921
		private Dictionary<uint, string> _configRequests = new Dictionary<uint, string>();

		// Token: 0x04001EF2 RID: 7922
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private SynchronizationContext <EventContext>k__BackingField;

		// Token: 0x04001EF3 RID: 7923
		private EventHandler<LeapEventArgs> _leapInit;

		// Token: 0x04001EF4 RID: 7924
		private EventHandler<ConnectionEventArgs> _leapConnectionEvent;

		// Token: 0x04001EF5 RID: 7925
		public EventHandler<ConnectionLostEventArgs> LeapConnectionLost;

		// Token: 0x04001EF6 RID: 7926
		public EventHandler<DeviceEventArgs> LeapDevice;

		// Token: 0x04001EF7 RID: 7927
		public EventHandler<DeviceEventArgs> LeapDeviceLost;

		// Token: 0x04001EF8 RID: 7928
		public EventHandler<DeviceFailureEventArgs> LeapDeviceFailure;

		// Token: 0x04001EF9 RID: 7929
		public EventHandler<PolicyEventArgs> LeapPolicyChange;

		// Token: 0x04001EFA RID: 7930
		public EventHandler<FrameEventArgs> LeapFrame;

		// Token: 0x04001EFB RID: 7931
		public EventHandler<InternalFrameEventArgs> LeapInternalFrame;

		// Token: 0x04001EFC RID: 7932
		public EventHandler<LogEventArgs> LeapLogEvent;

		// Token: 0x04001EFD RID: 7933
		public EventHandler<SetConfigResponseEventArgs> LeapConfigResponse;

		// Token: 0x04001EFE RID: 7934
		public EventHandler<ConfigChangeEventArgs> LeapConfigChange;

		// Token: 0x04001EFF RID: 7935
		public EventHandler<DistortionEventArgs> LeapDistortionChange;

		// Token: 0x04001F00 RID: 7936
		public EventHandler<DroppedFrameEventArgs> LeapDroppedFrame;

		// Token: 0x04001F01 RID: 7937
		public EventHandler<ImageEventArgs> LeapImage;

		// Token: 0x04001F02 RID: 7938
		public EventHandler<PointMappingChangeEventArgs> LeapPointMappingChange;

		// Token: 0x04001F03 RID: 7939
		public EventHandler<HeadPoseEventArgs> LeapHeadPoseChange;

		// Token: 0x04001F04 RID: 7940
		public Action<BeginProfilingForThreadArgs> LeapBeginProfilingForThread;

		// Token: 0x04001F05 RID: 7941
		public Action<EndProfilingForThreadArgs> LeapEndProfilingForThread;

		// Token: 0x04001F06 RID: 7942
		public Action<BeginProfilingBlockArgs> LeapBeginProfilingBlock;

		// Token: 0x04001F07 RID: 7943
		public Action<EndProfilingBlockArgs> LeapEndProfilingBlock;

		// Token: 0x04001F08 RID: 7944
		private bool _disposed;

		// Token: 0x04001F09 RID: 7945
		private LEAP_ALLOCATOR _pLeapAllocator = default(LEAP_ALLOCATOR);

		// Token: 0x04001F0A RID: 7946
		private eLeapRS _lastResult;

		// Token: 0x04001F0B RID: 7947
		[CompilerGenerated]
		private static Allocate <>f__mg$cache0;

		// Token: 0x04001F0C RID: 7948
		[CompilerGenerated]
		private static Deallocate <>f__mg$cache1;
	}
}
