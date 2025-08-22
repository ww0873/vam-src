using System;
using System.Runtime.CompilerServices;
using Leap.Unity.Attributes;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x020006FC RID: 1788
	public class LeapServiceProvider : LeapProvider
	{
		// Token: 0x06002B52 RID: 11090 RVA: 0x000E9822 File Offset: 0x000E7C22
		public LeapServiceProvider()
		{
		}

		// Token: 0x140000B8 RID: 184
		// (add) Token: 0x06002B53 RID: 11091 RVA: 0x000E985C File Offset: 0x000E7C5C
		// (remove) Token: 0x06002B54 RID: 11092 RVA: 0x000E98F0 File Offset: 0x000E7CF0
		public event Action<Device> OnDeviceSafe
		{
			add
			{
				if (this._leapController != null && this._leapController.IsConnected)
				{
					foreach (Device obj in this._leapController.Devices)
					{
						value(obj);
					}
				}
				this._onDeviceSafe = (Action<Device>)Delegate.Combine(this._onDeviceSafe, value);
			}
			remove
			{
				this._onDeviceSafe = (Action<Device>)Delegate.Remove(this._onDeviceSafe, value);
			}
		}

		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x06002B55 RID: 11093 RVA: 0x000E9909 File Offset: 0x000E7D09
		public override Frame CurrentFrame
		{
			get
			{
				if (this._frameOptimization == LeapServiceProvider.FrameOptimizationMode.ReusePhysicsForUpdate)
				{
					return this._transformedFixedFrame;
				}
				return this._transformedUpdateFrame;
			}
		}

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x06002B56 RID: 11094 RVA: 0x000E9924 File Offset: 0x000E7D24
		public override Frame CurrentFixedFrame
		{
			get
			{
				if (this._frameOptimization == LeapServiceProvider.FrameOptimizationMode.ReuseUpdateForPhysics)
				{
					return this._transformedUpdateFrame;
				}
				return this._transformedFixedFrame;
			}
		}

		// Token: 0x06002B57 RID: 11095 RVA: 0x000E993F File Offset: 0x000E7D3F
		protected virtual void Reset()
		{
			this.editTimePose = TestHandFactory.TestHandPose.DesktopModeA;
		}

		// Token: 0x06002B58 RID: 11096 RVA: 0x000E9948 File Offset: 0x000E7D48
		protected virtual void Awake()
		{
			this._fixedOffset.delay = 0.4f;
			this._smoothedTrackingLatency.SetBlend(0.99f, 0.0111f);
		}

		// Token: 0x06002B59 RID: 11097 RVA: 0x000E996F File Offset: 0x000E7D6F
		protected virtual void Start()
		{
			this.createController();
			this._transformedUpdateFrame = new Frame();
			this._transformedFixedFrame = new Frame();
			this._untransformedUpdateFrame = new Frame();
			this._untransformedFixedFrame = new Frame();
		}

		// Token: 0x06002B5A RID: 11098 RVA: 0x000E99A4 File Offset: 0x000E7DA4
		protected virtual void Update()
		{
			if (this._workerThreadProfiling)
			{
				LeapProfiling.Update();
			}
			if (!this.checkConnectionIntegrity())
			{
				return;
			}
			this._fixedOffset.Update(Time.time - Time.fixedTime, Time.deltaTime);
			if (this._frameOptimization == LeapServiceProvider.FrameOptimizationMode.ReusePhysicsForUpdate)
			{
				base.DispatchUpdateFrameEvent(this._transformedFixedFrame);
				return;
			}
			if (this._useInterpolation)
			{
				this._smoothedTrackingLatency.value = Mathf.Min(this._smoothedTrackingLatency.value, 30000f);
				this._smoothedTrackingLatency.Update((float)(this._leapController.Now() - this._leapController.FrameTimestamp(0)), Time.deltaTime);
				long num = this.CalculateInterpolationTime(false) + (long)(this.ExtrapolationAmount * 1000);
				this._unityToLeapOffset = num - (long)((double)Time.time * 1000000.0);
				this._leapController.GetInterpolatedFrameFromTime(this._untransformedUpdateFrame, num, this.CalculateInterpolationTime(false) - (long)(this.BounceAmount * 1000));
			}
			else
			{
				this._leapController.Frame(this._untransformedUpdateFrame, 0);
			}
			if (this._untransformedUpdateFrame != null)
			{
				this.transformFrame(this._untransformedUpdateFrame, this._transformedUpdateFrame);
				base.DispatchUpdateFrameEvent(this._transformedUpdateFrame);
			}
		}

		// Token: 0x06002B5B RID: 11099 RVA: 0x000E9AF0 File Offset: 0x000E7EF0
		protected virtual void FixedUpdate()
		{
			if (this._frameOptimization == LeapServiceProvider.FrameOptimizationMode.ReuseUpdateForPhysics)
			{
				base.DispatchFixedFrameEvent(this._transformedUpdateFrame);
				return;
			}
			if (this._useInterpolation)
			{
				LeapServiceProvider.FrameOptimizationMode frameOptimization = this._frameOptimization;
				long time;
				if (frameOptimization != LeapServiceProvider.FrameOptimizationMode.None)
				{
					if (frameOptimization != LeapServiceProvider.FrameOptimizationMode.ReusePhysicsForUpdate)
					{
						throw new InvalidOperationException("Unexpected frame optimization mode: " + this._frameOptimization);
					}
					time = this.CalculateInterpolationTime(false) + (long)(this.ExtrapolationAmount * 1000);
				}
				else
				{
					float num = Time.fixedTime + this.CalculatePhysicsExtrapolation();
					time = (long)((double)num * 1000000.0) + this._unityToLeapOffset;
				}
				this._leapController.GetInterpolatedFrame(this._untransformedFixedFrame, time);
			}
			else
			{
				this._leapController.Frame(this._untransformedFixedFrame, 0);
			}
			if (this._untransformedFixedFrame != null)
			{
				this.transformFrame(this._untransformedFixedFrame, this._transformedFixedFrame);
				base.DispatchFixedFrameEvent(this._transformedFixedFrame);
			}
		}

		// Token: 0x06002B5C RID: 11100 RVA: 0x000E9BE9 File Offset: 0x000E7FE9
		protected virtual void OnDestroy()
		{
			this.destroyController();
			this._isDestroyed = true;
		}

		// Token: 0x06002B5D RID: 11101 RVA: 0x000E9BF8 File Offset: 0x000E7FF8
		protected virtual void OnApplicationPause(bool isPaused)
		{
			if (this._leapController != null)
			{
				if (isPaused)
				{
					this._leapController.StopConnection();
				}
				else
				{
					this._leapController.StartConnection();
				}
			}
		}

		// Token: 0x06002B5E RID: 11102 RVA: 0x000E9C26 File Offset: 0x000E8026
		protected virtual void OnApplicationQuit()
		{
			this.destroyController();
			this._isDestroyed = true;
		}

		// Token: 0x06002B5F RID: 11103 RVA: 0x000E9C38 File Offset: 0x000E8038
		public float CalculatePhysicsExtrapolation()
		{
			switch (this._physicsExtrapolation)
			{
			case LeapServiceProvider.PhysicsExtrapolationMode.None:
				return 0f;
			case LeapServiceProvider.PhysicsExtrapolationMode.Auto:
				return Time.fixedDeltaTime;
			case LeapServiceProvider.PhysicsExtrapolationMode.Manual:
				return this._physicsExtrapolationTime;
			default:
				throw new InvalidOperationException("Unexpected physics extrapolation mode: " + this._physicsExtrapolation);
			}
		}

		// Token: 0x06002B60 RID: 11104 RVA: 0x000E9C90 File Offset: 0x000E8090
		public Controller GetLeapController()
		{
			return this._leapController;
		}

		// Token: 0x06002B61 RID: 11105 RVA: 0x000E9C98 File Offset: 0x000E8098
		public bool IsConnected()
		{
			return this.GetLeapController().IsConnected;
		}

		// Token: 0x06002B62 RID: 11106 RVA: 0x000E9CA5 File Offset: 0x000E80A5
		public void RetransformFrames()
		{
			this.transformFrame(this._untransformedUpdateFrame, this._transformedUpdateFrame);
			this.transformFrame(this._untransformedFixedFrame, this._transformedFixedFrame);
		}

		// Token: 0x06002B63 RID: 11107 RVA: 0x000E9CCB File Offset: 0x000E80CB
		public void CopySettingsToLeapXRServiceProvider(LeapXRServiceProvider leapXRServiceProvider)
		{
			leapXRServiceProvider._frameOptimization = this._frameOptimization;
			leapXRServiceProvider._physicsExtrapolation = this._physicsExtrapolation;
			leapXRServiceProvider._physicsExtrapolationTime = this._physicsExtrapolationTime;
			leapXRServiceProvider._workerThreadProfiling = this._workerThreadProfiling;
		}

		// Token: 0x06002B64 RID: 11108 RVA: 0x000E9CFD File Offset: 0x000E80FD
		protected virtual long CalculateInterpolationTime(bool endOfFrame = false)
		{
			if (this._leapController != null)
			{
				return this._leapController.Now() - (long)this._smoothedTrackingLatency.value;
			}
			return 0L;
		}

		// Token: 0x06002B65 RID: 11109 RVA: 0x000E9D25 File Offset: 0x000E8125
		protected virtual void initializeFlags()
		{
			if (this._leapController == null)
			{
				return;
			}
			this._leapController.ClearPolicy(Controller.PolicyFlag.POLICY_DEFAULT);
		}

		// Token: 0x06002B66 RID: 11110 RVA: 0x000E9D40 File Offset: 0x000E8140
		protected void createController()
		{
			if (this._leapController != null)
			{
				return;
			}
			this._leapController = new Controller();
			this._leapController.Device += this.<createController>m__0;
			if (this._leapController.IsConnected)
			{
				this.initializeFlags();
			}
			else
			{
				this._leapController.Device += new EventHandler<DeviceEventArgs>(this.onHandControllerConnect);
			}
			if (this._workerThreadProfiling)
			{
				Controller leapController = this._leapController;
				if (LeapServiceProvider.<>f__mg$cache0 == null)
				{
					LeapServiceProvider.<>f__mg$cache0 = new Action<EndProfilingBlockArgs>(LeapProfiling.EndProfilingBlock);
				}
				leapController.EndProfilingBlock += LeapServiceProvider.<>f__mg$cache0;
				Controller leapController2 = this._leapController;
				if (LeapServiceProvider.<>f__mg$cache1 == null)
				{
					LeapServiceProvider.<>f__mg$cache1 = new Action<BeginProfilingBlockArgs>(LeapProfiling.BeginProfilingBlock);
				}
				leapController2.BeginProfilingBlock += LeapServiceProvider.<>f__mg$cache1;
				Controller leapController3 = this._leapController;
				if (LeapServiceProvider.<>f__mg$cache2 == null)
				{
					LeapServiceProvider.<>f__mg$cache2 = new Action<EndProfilingForThreadArgs>(LeapProfiling.EndProfilingForThread);
				}
				leapController3.EndProfilingForThread += LeapServiceProvider.<>f__mg$cache2;
				Controller leapController4 = this._leapController;
				if (LeapServiceProvider.<>f__mg$cache3 == null)
				{
					LeapServiceProvider.<>f__mg$cache3 = new Action<BeginProfilingForThreadArgs>(LeapProfiling.BeginProfilingForThread);
				}
				leapController4.BeginProfilingForThread += LeapServiceProvider.<>f__mg$cache3;
			}
		}

		// Token: 0x06002B67 RID: 11111 RVA: 0x000E9E58 File Offset: 0x000E8258
		protected void destroyController()
		{
			if (this._leapController != null)
			{
				if (this._leapController.IsConnected)
				{
					this._leapController.ClearPolicy(Controller.PolicyFlag.POLICY_OPTIMIZE_HMD);
				}
				this._leapController.StopConnection();
				this._leapController = null;
			}
		}

		// Token: 0x06002B68 RID: 11112 RVA: 0x000E9E94 File Offset: 0x000E8294
		protected bool checkConnectionIntegrity()
		{
			if (this._leapController.IsServiceConnected)
			{
				this._framesSinceServiceConnectionChecked = 0;
				this._numberOfReconnectionAttempts = 0;
				return true;
			}
			if (this._numberOfReconnectionAttempts < 5)
			{
				this._framesSinceServiceConnectionChecked++;
				if (this._framesSinceServiceConnectionChecked > 180)
				{
					this._framesSinceServiceConnectionChecked = 0;
					this._numberOfReconnectionAttempts++;
					Debug.LogWarning(string.Concat(new object[]
					{
						"Leap Service not connected; attempting to reconnect for try ",
						this._numberOfReconnectionAttempts,
						"/",
						5,
						"..."
					}), this);
					using (new ProfilerSample("Reconnection Attempt"))
					{
						this.destroyController();
						this.createController();
					}
				}
			}
			return false;
		}

		// Token: 0x06002B69 RID: 11113 RVA: 0x000E9F7C File Offset: 0x000E837C
		protected void onHandControllerConnect(object sender, LeapEventArgs args)
		{
			this.initializeFlags();
			if (this._leapController != null)
			{
				this._leapController.Device -= new EventHandler<DeviceEventArgs>(this.onHandControllerConnect);
			}
		}

		// Token: 0x06002B6A RID: 11114 RVA: 0x000E9FA6 File Offset: 0x000E83A6
		protected virtual void transformFrame(Frame source, Frame dest)
		{
			dest.CopyFrom(source).Transform(base.transform.GetLeapMatrix());
		}

		// Token: 0x06002B6B RID: 11115 RVA: 0x000E9FC0 File Offset: 0x000E83C0
		[CompilerGenerated]
		private void <createController>m__0(object s, DeviceEventArgs e)
		{
			if (this._onDeviceSafe != null)
			{
				this._onDeviceSafe(e.Device);
			}
		}

		// Token: 0x040022FE RID: 8958
		protected const double NS_TO_S = 1E-06;

		// Token: 0x040022FF RID: 8959
		protected const double S_TO_NS = 1000000.0;

		// Token: 0x04002300 RID: 8960
		protected const string HAND_ARRAY_GLOBAL_NAME = "_LeapHandTransforms";

		// Token: 0x04002301 RID: 8961
		protected const int MAX_RECONNECTION_ATTEMPTS = 5;

		// Token: 0x04002302 RID: 8962
		protected const int RECONNECTION_INTERVAL = 180;

		// Token: 0x04002303 RID: 8963
		[Tooltip("When enabled, the provider will only calculate one leap frame instead of two.")]
		[SerializeField]
		protected LeapServiceProvider.FrameOptimizationMode _frameOptimization;

		// Token: 0x04002304 RID: 8964
		[Tooltip("The mode to use when extrapolating physics.\n None - No extrapolation is used at all.\n Auto - Extrapolation is chosen based on the fixed timestep.\n Manual - Extrapolation time is chosen manually by the user.")]
		[SerializeField]
		protected LeapServiceProvider.PhysicsExtrapolationMode _physicsExtrapolation = LeapServiceProvider.PhysicsExtrapolationMode.Auto;

		// Token: 0x04002305 RID: 8965
		[Tooltip("The amount of time (in seconds) to extrapolate the physics data by.")]
		[SerializeField]
		protected float _physicsExtrapolationTime = 0.011111111f;

		// Token: 0x04002306 RID: 8966
		[Tooltip("When checked, profiling data from the LeapCSharp worker thread will be used to populate the UnityProfiler.")]
		[EditTimeOnly]
		[SerializeField]
		protected bool _workerThreadProfiling;

		// Token: 0x04002307 RID: 8967
		protected bool _useInterpolation = true;

		// Token: 0x04002308 RID: 8968
		protected int ExtrapolationAmount;

		// Token: 0x04002309 RID: 8969
		protected int BounceAmount;

		// Token: 0x0400230A RID: 8970
		protected Controller _leapController;

		// Token: 0x0400230B RID: 8971
		protected bool _isDestroyed;

		// Token: 0x0400230C RID: 8972
		protected SmoothedFloat _fixedOffset = new SmoothedFloat();

		// Token: 0x0400230D RID: 8973
		protected SmoothedFloat _smoothedTrackingLatency = new SmoothedFloat();

		// Token: 0x0400230E RID: 8974
		protected long _unityToLeapOffset;

		// Token: 0x0400230F RID: 8975
		protected Frame _untransformedUpdateFrame;

		// Token: 0x04002310 RID: 8976
		protected Frame _transformedUpdateFrame;

		// Token: 0x04002311 RID: 8977
		protected Frame _untransformedFixedFrame;

		// Token: 0x04002312 RID: 8978
		protected Frame _transformedFixedFrame;

		// Token: 0x04002313 RID: 8979
		private Action<Device> _onDeviceSafe;

		// Token: 0x04002314 RID: 8980
		private int _framesSinceServiceConnectionChecked;

		// Token: 0x04002315 RID: 8981
		private int _numberOfReconnectionAttempts;

		// Token: 0x04002316 RID: 8982
		[CompilerGenerated]
		private static Action<EndProfilingBlockArgs> <>f__mg$cache0;

		// Token: 0x04002317 RID: 8983
		[CompilerGenerated]
		private static Action<BeginProfilingBlockArgs> <>f__mg$cache1;

		// Token: 0x04002318 RID: 8984
		[CompilerGenerated]
		private static Action<EndProfilingForThreadArgs> <>f__mg$cache2;

		// Token: 0x04002319 RID: 8985
		[CompilerGenerated]
		private static Action<BeginProfilingForThreadArgs> <>f__mg$cache3;

		// Token: 0x020006FD RID: 1789
		public enum FrameOptimizationMode
		{
			// Token: 0x0400231B RID: 8987
			None,
			// Token: 0x0400231C RID: 8988
			ReuseUpdateForPhysics,
			// Token: 0x0400231D RID: 8989
			ReusePhysicsForUpdate
		}

		// Token: 0x020006FE RID: 1790
		public enum PhysicsExtrapolationMode
		{
			// Token: 0x0400231F RID: 8991
			None,
			// Token: 0x04002320 RID: 8992
			Auto,
			// Token: 0x04002321 RID: 8993
			Manual
		}
	}
}
