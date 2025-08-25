using System;
using Leap.Unity.Attributes;
using UnityEngine;
using UnityEngine.Rendering;

namespace Leap.Unity
{
	// Token: 0x02000700 RID: 1792
	public class LeapXRServiceProvider : LeapServiceProvider
	{
		// Token: 0x06002B72 RID: 11122 RVA: 0x000EA188 File Offset: 0x000E8588
		public LeapXRServiceProvider()
		{
		}

		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x06002B73 RID: 11123 RVA: 0x000EA1E8 File Offset: 0x000E85E8
		// (set) Token: 0x06002B74 RID: 11124 RVA: 0x000EA1F0 File Offset: 0x000E85F0
		public LeapXRServiceProvider.DeviceOffsetMode deviceOffsetMode
		{
			get
			{
				return this._deviceOffsetMode;
			}
			set
			{
				this._deviceOffsetMode = value;
				if (this._deviceOffsetMode == LeapXRServiceProvider.DeviceOffsetMode.Default || this._deviceOffsetMode == LeapXRServiceProvider.DeviceOffsetMode.Transform)
				{
					this.deviceOffsetYAxis = 0f;
					this.deviceOffsetZAxis = 0.12f;
					this.deviceTiltXAxis = 5f;
				}
				if (this._deviceOffsetMode == LeapXRServiceProvider.DeviceOffsetMode.Transform && this._temporalWarpingMode != LeapXRServiceProvider.TemporalWarpingMode.Off)
				{
					this._temporalWarpingMode = LeapXRServiceProvider.TemporalWarpingMode.Off;
				}
			}
		}

		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x06002B75 RID: 11125 RVA: 0x000EA25B File Offset: 0x000E865B
		// (set) Token: 0x06002B76 RID: 11126 RVA: 0x000EA263 File Offset: 0x000E8663
		public float deviceOffsetYAxis
		{
			get
			{
				return this._deviceOffsetYAxis;
			}
			set
			{
				this._deviceOffsetYAxis = value;
			}
		}

		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x06002B77 RID: 11127 RVA: 0x000EA26C File Offset: 0x000E866C
		// (set) Token: 0x06002B78 RID: 11128 RVA: 0x000EA274 File Offset: 0x000E8674
		public float deviceOffsetZAxis
		{
			get
			{
				return this._deviceOffsetZAxis;
			}
			set
			{
				this._deviceOffsetZAxis = value;
			}
		}

		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x06002B79 RID: 11129 RVA: 0x000EA27D File Offset: 0x000E867D
		// (set) Token: 0x06002B7A RID: 11130 RVA: 0x000EA285 File Offset: 0x000E8685
		public float deviceTiltXAxis
		{
			get
			{
				return this._deviceTiltXAxis;
			}
			set
			{
				this._deviceTiltXAxis = value;
			}
		}

		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x06002B7B RID: 11131 RVA: 0x000EA28E File Offset: 0x000E868E
		// (set) Token: 0x06002B7C RID: 11132 RVA: 0x000EA296 File Offset: 0x000E8696
		public Transform deviceOrigin
		{
			get
			{
				return this._deviceOrigin;
			}
			set
			{
				this._deviceOrigin = value;
			}
		}

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x06002B7D RID: 11133 RVA: 0x000EA29F File Offset: 0x000E869F
		public int warpingAdjustment
		{
			get
			{
				if (this._temporalWarpingMode == LeapXRServiceProvider.TemporalWarpingMode.Manual)
				{
					return this._customWarpAdjustment;
				}
				return 17;
			}
		}

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x06002B7E RID: 11134 RVA: 0x000EA2B6 File Offset: 0x000E86B6
		// (set) Token: 0x06002B7F RID: 11135 RVA: 0x000EA2BE File Offset: 0x000E86BE
		public bool updateHandInPrecull
		{
			get
			{
				return this._updateHandInPrecull;
			}
			set
			{
				this.resetShaderTransforms();
				this._updateHandInPrecull = value;
			}
		}

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x06002B80 RID: 11136 RVA: 0x000EA2CD File Offset: 0x000E86CD
		private Camera cachedCamera
		{
			get
			{
				if (this._cachedCamera == null)
				{
					this._cachedCamera = base.GetComponent<Camera>();
				}
				return this._cachedCamera;
			}
		}

		// Token: 0x06002B81 RID: 11137 RVA: 0x000EA2F2 File Offset: 0x000E86F2
		protected override void Reset()
		{
			base.Reset();
			this.editTimePose = TestHandFactory.TestHandPose.HeadMountedB;
		}

		// Token: 0x06002B82 RID: 11138 RVA: 0x000EA301 File Offset: 0x000E8701
		protected virtual void OnValidate()
		{
			if (this._deviceOffsetMode == LeapXRServiceProvider.DeviceOffsetMode.Transform && this._temporalWarpingMode != LeapXRServiceProvider.TemporalWarpingMode.Off)
			{
				this._temporalWarpingMode = LeapXRServiceProvider.TemporalWarpingMode.Off;
			}
		}

		// Token: 0x06002B83 RID: 11139 RVA: 0x000EA322 File Offset: 0x000E8722
		protected virtual void OnEnable()
		{
			this.resetShaderTransforms();
		}

		// Token: 0x06002B84 RID: 11140 RVA: 0x000EA32A File Offset: 0x000E872A
		protected virtual void OnDisable()
		{
			this.resetShaderTransforms();
		}

		// Token: 0x06002B85 RID: 11141 RVA: 0x000EA334 File Offset: 0x000E8734
		protected override void Start()
		{
			base.Start();
			this._cachedCamera = base.GetComponent<Camera>();
			if (this._deviceOffsetMode == LeapXRServiceProvider.DeviceOffsetMode.Transform && this._deviceOrigin == null)
			{
				Debug.LogError("Cannot use the Transform device offset mode without specifying a Transform to use as the device origin.", this);
				this._deviceOffsetMode = LeapXRServiceProvider.DeviceOffsetMode.Default;
			}
		}

		// Token: 0x06002B86 RID: 11142 RVA: 0x000EA382 File Offset: 0x000E8782
		protected override void Update()
		{
			this.manualUpdateHasBeenCalledSinceUpdate = false;
			base.Update();
			this.imageTimeStamp = this._leapController.FrameTimestamp(0);
		}

		// Token: 0x06002B87 RID: 11143 RVA: 0x000EA3A4 File Offset: 0x000E87A4
		private void LateUpdate()
		{
			Matrix4x4 projectionMatrix = this._cachedCamera.projectionMatrix;
			GraphicsDeviceType graphicsDeviceType = SystemInfo.graphicsDeviceType;
			if (graphicsDeviceType == GraphicsDeviceType.Direct3D11 || graphicsDeviceType == GraphicsDeviceType.Direct3D12)
			{
				for (int i = 0; i < 4; i++)
				{
					projectionMatrix[1, i] = -projectionMatrix[1, i];
				}
				for (int j = 0; j < 4; j++)
				{
					projectionMatrix[2, j] = projectionMatrix[2, j] * 0.5f + projectionMatrix[3, j] * 0.5f;
				}
			}
			Vector3 vector;
			Quaternion quaternion;
			this.transformHistory.SampleTransform(this.imageTimeStamp - (long)((float)this.warpingAdjustment * 1000f), out vector, out quaternion);
			Quaternion xrnodeCenterEyeLocalRotation = XRSupportUtil.GetXRNodeCenterEyeLocalRotation();
			Quaternion rhs = (this._temporalWarpingMode == LeapXRServiceProvider.TemporalWarpingMode.Off) ? xrnodeCenterEyeLocalRotation : quaternion;
			Quaternion q = Quaternion.Inverse(xrnodeCenterEyeLocalRotation) * rhs;
			q = Quaternion.Euler(q.eulerAngles.x, q.eulerAngles.y, -q.eulerAngles.z);
			Matrix4x4 value = projectionMatrix * Matrix4x4.TRS(Vector3.zero, q, Vector3.one) * projectionMatrix.inverse;
			Shader.SetGlobalMatrix("_LeapGlobalWarpedOffset", value);
		}

		// Token: 0x06002B88 RID: 11144 RVA: 0x000EA4F8 File Offset: 0x000E88F8
		private void OnPreCull()
		{
			if (this._cachedCamera == null)
			{
				return;
			}
			Pose b = new Pose(XRSupportUtil.GetXRNodeCenterEyeLocalPosition(), XRSupportUtil.GetXRNodeCenterEyeLocalRotation());
			if (this._trackingBaseDeltaPose == null)
			{
				this._trackingBaseDeltaPose = new Pose?(this._cachedCamera.transform.ToLocalPose() * b.inverse);
			}
			Pose curPose = this._trackingBaseDeltaPose.Value * b;
			this.transformHistory.UpdateDelay(curPose, this._leapController.Now());
			this.OnPreCullHandTransforms(this._cachedCamera);
		}

		// Token: 0x06002B89 RID: 11145 RVA: 0x000EA594 File Offset: 0x000E8994
		protected override long CalculateInterpolationTime(bool endOfFrame = false)
		{
			if (this._leapController != null)
			{
				return this._leapController.Now() - (long)this._smoothedTrackingLatency.value + ((!this.updateHandInPrecull || endOfFrame) ? 0L : ((long)((double)Time.smoothDeltaTime * 1000000.0 / (double)Time.timeScale)));
			}
			return 0L;
		}

		// Token: 0x06002B8A RID: 11146 RVA: 0x000EA5F8 File Offset: 0x000E89F8
		protected override void initializeFlags()
		{
			if (this._leapController == null)
			{
				return;
			}
			this._leapController.SetPolicy(Controller.PolicyFlag.POLICY_OPTIMIZE_HMD);
		}

		// Token: 0x06002B8B RID: 11147 RVA: 0x000EA614 File Offset: 0x000E8A14
		protected override void transformFrame(Frame source, Frame dest)
		{
			LeapTransform warpedMatrix = this.GetWarpedMatrix(source.Timestamp, true);
			dest.CopyFrom(source).Transform(warpedMatrix);
		}

		// Token: 0x06002B8C RID: 11148 RVA: 0x000EA63D File Offset: 0x000E8A3D
		protected void resetShaderTransforms()
		{
			this._transformArray[0] = Matrix4x4.identity;
			this._transformArray[1] = Matrix4x4.identity;
			Shader.SetGlobalMatrixArray("_LeapHandTransforms", this._transformArray);
		}

		// Token: 0x06002B8D RID: 11149 RVA: 0x000EA67C File Offset: 0x000E8A7C
		protected virtual LeapTransform GetWarpedMatrix(long timestamp, bool updateTemporalCompensation = true)
		{
			if (Application.isPlaying && updateTemporalCompensation && this.transformHistory.history.IsFull && this._temporalWarpingMode != LeapXRServiceProvider.TemporalWarpingMode.Off)
			{
				this.transformHistory.SampleTransform(timestamp - (long)((float)this.warpingAdjustment * 1000f) - ((this._temporalWarpingMode != LeapXRServiceProvider.TemporalWarpingMode.Images) ? 0L : -20000L), out this.warpedPosition, out this.warpedRotation);
			}
			Pose otherPose = Pose.identity;
			if (this._deviceOffsetMode == LeapXRServiceProvider.DeviceOffsetMode.Transform && this.deviceOrigin != null)
			{
				otherPose.position = this.deviceOrigin.position;
				otherPose.rotation = this.deviceOrigin.rotation;
			}
			else if (!Application.isPlaying)
			{
				otherPose.position = otherPose.rotation * Vector3.up * this.deviceOffsetYAxis + otherPose.rotation * Vector3.forward * this.deviceOffsetZAxis;
				otherPose.rotation = Quaternion.Euler(this.deviceTiltXAxis, 0f, 0f);
				otherPose = base.transform.ToLocalPose().Then(otherPose);
			}
			else
			{
				this.transformHistory.SampleTransform(timestamp, out otherPose.position, out otherPose.rotation);
			}
			bool flag = this._temporalWarpingMode == LeapXRServiceProvider.TemporalWarpingMode.Off || !Application.isPlaying;
			this.warpedPosition = ((!flag) ? this.warpedPosition : otherPose.position);
			this.warpedRotation = ((!flag) ? this.warpedRotation : otherPose.rotation);
			if (Application.isPlaying)
			{
				if (this._deviceOffsetMode != LeapXRServiceProvider.DeviceOffsetMode.Transform)
				{
					this.warpedPosition += this.warpedRotation * Vector3.up * this.deviceOffsetYAxis + this.warpedRotation * Vector3.forward * this.deviceOffsetZAxis;
					this.warpedRotation *= Quaternion.Euler(this.deviceTiltXAxis, 0f, 0f);
				}
				this.warpedRotation *= Quaternion.Euler(-90f, 180f, 0f);
			}
			LeapTransform result;
			if (base.transform.parent != null && this._deviceOffsetMode != LeapXRServiceProvider.DeviceOffsetMode.Transform)
			{
				result = new LeapTransform(base.transform.parent.TransformPoint(this.warpedPosition).ToVector(), (base.transform.parent.rotation * this.warpedRotation).ToLeapQuaternion(), base.transform.lossyScale.ToVector() * 0.001f);
			}
			else
			{
				result = new LeapTransform(this.warpedPosition.ToVector(), this.warpedRotation.ToLeapQuaternion(), base.transform.lossyScale.ToVector() * 0.001f);
			}
			result.MirrorZ();
			return result;
		}

		// Token: 0x06002B8E RID: 11150 RVA: 0x000EA9A4 File Offset: 0x000E8DA4
		protected void transformHands(ref LeapTransform LeftHand, ref LeapTransform RightHand)
		{
			LeapTransform warpedMatrix = this.GetWarpedMatrix(0L, false);
			LeftHand = new LeapTransform(warpedMatrix.TransformPoint(LeftHand.translation), warpedMatrix.TransformQuaternion(LeftHand.rotation));
			RightHand = new LeapTransform(warpedMatrix.TransformPoint(RightHand.translation), warpedMatrix.TransformQuaternion(RightHand.rotation));
		}

		// Token: 0x06002B8F RID: 11151 RVA: 0x000EA9FC File Offset: 0x000E8DFC
		protected void OnPreCullHandTransforms(Camera camera)
		{
			if (this.updateHandInPrecull)
			{
				CameraType cameraType = camera.cameraType;
				if (cameraType == CameraType.Preview || cameraType == CameraType.Reflection || cameraType == CameraType.SceneView)
				{
					return;
				}
				if (Application.isPlaying && !this.manualUpdateHasBeenCalledSinceUpdate && this._leapController != null)
				{
					this.manualUpdateHasBeenCalledSinceUpdate = true;
					Hand hand = null;
					Hand hand2 = null;
					LeapTransform identity = LeapTransform.Identity;
					LeapTransform identity2 = LeapTransform.Identity;
					for (int i = 0; i < this.CurrentFrame.Hands.Count; i++)
					{
						Hand hand3 = this.CurrentFrame.Hands[i];
						if (hand3.IsLeft && hand == null)
						{
							hand = hand3;
						}
						else if (hand3.IsRight && hand2 == null)
						{
							hand2 = hand3;
						}
					}
					long num = this.CalculateInterpolationTime(false);
					this._leapController.GetInterpolatedLeftRightTransform(num + (long)(this.ExtrapolationAmount * 1000), num - (long)(this.BounceAmount * 1000), (hand == null) ? 0 : hand.Id, (hand2 == null) ? 0 : hand2.Id, out identity, out identity2);
					bool flag = identity.translation != Vector.Zero;
					bool flag2 = identity2.translation != Vector.Zero;
					this.transformHands(ref identity, ref identity2);
					if (hand2 != null && flag2)
					{
						this._transformArray[0] = Matrix4x4.TRS(identity2.translation.ToVector3(), identity2.rotation.ToQuaternion(), Vector3.one) * Matrix4x4.Inverse(Matrix4x4.TRS(hand2.PalmPosition.ToVector3(), hand2.Rotation.ToQuaternion(), Vector3.one));
					}
					if (hand != null && flag)
					{
						this._transformArray[1] = Matrix4x4.TRS(identity.translation.ToVector3(), identity.rotation.ToQuaternion(), Vector3.one) * Matrix4x4.Inverse(Matrix4x4.TRS(hand.PalmPosition.ToVector3(), hand.Rotation.ToQuaternion(), Vector3.one));
					}
					Shader.SetGlobalMatrixArray("_LeapHandTransforms", this._transformArray);
				}
			}
		}

		// Token: 0x04002329 RID: 9001
		private const float DEFAULT_DEVICE_OFFSET_Y_AXIS = 0f;

		// Token: 0x0400232A RID: 9002
		private const float DEFAULT_DEVICE_OFFSET_Z_AXIS = 0.12f;

		// Token: 0x0400232B RID: 9003
		private const float DEFAULT_DEVICE_TILT_X_AXIS = 5f;

		// Token: 0x0400232C RID: 9004
		[Header("Advanced")]
		[Tooltip("Allow manual adjustment of the Leap device's virtual offset and tilt. These settings can be used to match the physical position and orientation of the Leap Motion sensor on a tracked device it is mounted on (such as a VR headset).  Temporal Warping not supported in Transform Mode.")]
		[SerializeField]
		[OnEditorChange("deviceOffsetMode")]
		private LeapXRServiceProvider.DeviceOffsetMode _deviceOffsetMode;

		// Token: 0x0400232D RID: 9005
		[Tooltip("Adjusts the Leap Motion device's virtual height offset from the tracked headset position. This should match the vertical offset of the physical device with respect to the headset in meters.")]
		[SerializeField]
		[Range(-0.5f, 0.5f)]
		private float _deviceOffsetYAxis;

		// Token: 0x0400232E RID: 9006
		[Tooltip("Adjusts the Leap Motion device's virtual depth offset from the tracked headset position. This should match the forward offset of the physical device with respect to the headset in meters.")]
		[SerializeField]
		[Range(-0.5f, 0.5f)]
		private float _deviceOffsetZAxis = 0.12f;

		// Token: 0x0400232F RID: 9007
		[Tooltip("Adjusts the Leap Motion device's virtual X axis tilt. This should match the tilt of the physical device with respect to the headset in degrees.")]
		[SerializeField]
		[Range(-90f, 90f)]
		private float _deviceTiltXAxis = 5f;

		// Token: 0x04002330 RID: 9008
		[Tooltip("Allows for the manual placement of the Leap Tracking Device.This device offset mode is incompatible with Temporal Warping.")]
		[SerializeField]
		private Transform _deviceOrigin;

		// Token: 0x04002331 RID: 9009
		private const int DEFAULT_WARP_ADJUSTMENT = 17;

		// Token: 0x04002332 RID: 9010
		[Tooltip("Temporal warping prevents the hand coordinate system from 'swimming' or 'bouncing' when the headset moves and the user's hands stay still. This phenomenon is caused by the differing amounts of latencies inherent in the two systems. For PC VR and Android VR, temporal warping should set to 'Auto', as the correct value can be chosen automatically for these platforms. Some non-standard platforms may use 'Manual' mode to adjust their latency compensation amount for temporal warping. Use 'Images' for scenarios that overlay Leap device images on tracked hand data.")]
		[SerializeField]
		private LeapXRServiceProvider.TemporalWarpingMode _temporalWarpingMode;

		// Token: 0x04002333 RID: 9011
		[Tooltip("The time in milliseconds between the current frame's headset position and the time at which the Leap frame was captured.")]
		[SerializeField]
		private int _customWarpAdjustment = 17;

		// Token: 0x04002334 RID: 9012
		[Tooltip("Pass updated transform matrices to hands with materials that utilize the VertexOffsetShader. Won't have any effect on hands that don't take into account shader-global vertex offsets in their material shaders.")]
		[SerializeField]
		protected bool _updateHandInPrecull;

		// Token: 0x04002335 RID: 9013
		protected TransformHistory transformHistory = new TransformHistory(32);

		// Token: 0x04002336 RID: 9014
		protected bool manualUpdateHasBeenCalledSinceUpdate;

		// Token: 0x04002337 RID: 9015
		protected Vector3 warpedPosition = Vector3.zero;

		// Token: 0x04002338 RID: 9016
		protected Quaternion warpedRotation = Quaternion.identity;

		// Token: 0x04002339 RID: 9017
		protected Matrix4x4[] _transformArray = new Matrix4x4[2];

		// Token: 0x0400233A RID: 9018
		private Pose? _trackingBaseDeltaPose;

		// Token: 0x0400233B RID: 9019
		private Camera _cachedCamera;

		// Token: 0x0400233C RID: 9020
		[NonSerialized]
		public long imageTimeStamp;

		// Token: 0x02000701 RID: 1793
		public enum DeviceOffsetMode
		{
			// Token: 0x0400233E RID: 9022
			Default,
			// Token: 0x0400233F RID: 9023
			ManualHeadOffset,
			// Token: 0x04002340 RID: 9024
			Transform
		}

		// Token: 0x02000702 RID: 1794
		public enum TemporalWarpingMode
		{
			// Token: 0x04002342 RID: 9026
			Auto,
			// Token: 0x04002343 RID: 9027
			Manual,
			// Token: 0x04002344 RID: 9028
			Images,
			// Token: 0x04002345 RID: 9029
			Off
		}
	}
}
