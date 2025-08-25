using System;
using Leap.Unity.Attributes;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x02000752 RID: 1874
	[RequireComponent(typeof(LeapXRServiceProvider))]
	public class LeapEyeDislocator : MonoBehaviour
	{
		// Token: 0x06003035 RID: 12341 RVA: 0x000F9A7E File Offset: 0x000F7E7E
		public LeapEyeDislocator()
		{
		}

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x06003036 RID: 12342 RVA: 0x000F9AA1 File Offset: 0x000F7EA1
		private Camera _camera
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

		// Token: 0x06003037 RID: 12343 RVA: 0x000F9AC6 File Offset: 0x000F7EC6
		private void onDevice(Device device)
		{
			this._deviceBaseline = Maybe.Some<float>(device.Baseline);
		}

		// Token: 0x06003038 RID: 12344 RVA: 0x000F9ADC File Offset: 0x000F7EDC
		private void OnEnable()
		{
			this._provider = base.GetComponent<LeapServiceProvider>();
			if (this._provider == null)
			{
				this._provider = base.GetComponentInChildren<LeapServiceProvider>();
				if (this._provider == null)
				{
					base.enabled = false;
					return;
				}
			}
			this._provider.OnDeviceSafe += this.onDevice;
		}

		// Token: 0x06003039 RID: 12345 RVA: 0x000F9B42 File Offset: 0x000F7F42
		private void OnDisable()
		{
			this._camera.ResetStereoViewMatrices();
			this._provider.OnDeviceSafe -= this.onDevice;
		}

		// Token: 0x0600303A RID: 12346 RVA: 0x000F9B66 File Offset: 0x000F7F66
		private void Update()
		{
			this._camera.ResetStereoViewMatrices();
			this._hasVisitedPreCull = false;
		}

		// Token: 0x0600303B RID: 12347 RVA: 0x000F9B7C File Offset: 0x000F7F7C
		private void OnPreCull()
		{
			if (this._hasVisitedPreCull)
			{
				return;
			}
			this._hasVisitedPreCull = true;
			Maybe<float> maybe = Maybe.None;
			if (this._useCustomBaseline)
			{
				maybe = Maybe.Some<float>(this._customBaselineValue);
			}
			else
			{
				maybe = this._deviceBaseline;
			}
			float num;
			if (maybe.TryGetValue(out num))
			{
				num *= 0.001f;
				Matrix4x4 stereoViewMatrix = this._camera.GetStereoViewMatrix(Camera.StereoscopicEye.Left);
				Matrix4x4 stereoViewMatrix2 = this._camera.GetStereoViewMatrix(Camera.StereoscopicEye.Right);
				Vector3 a = stereoViewMatrix.inverse.MultiplyPoint3x4(Vector3.zero);
				Vector3 b = stereoViewMatrix2.inverse.MultiplyPoint3x4(Vector3.zero);
				float num2 = Vector3.Distance(a, b);
				float baselineAdjust = num - num2;
				this.adjustViewMatrix(Camera.StereoscopicEye.Left, baselineAdjust);
				this.adjustViewMatrix(Camera.StereoscopicEye.Right, baselineAdjust);
			}
		}

		// Token: 0x0600303C RID: 12348 RVA: 0x000F9C4C File Offset: 0x000F804C
		private void adjustViewMatrix(Camera.StereoscopicEye eye, float baselineAdjust)
		{
			float d = (float)((eye != Camera.StereoscopicEye.Left) ? -1 : 1);
			Vector3 b = d * Vector3.right * baselineAdjust * 0.5f;
			Vector3 a = Vector3.zero;
			Vector3 vector = Vector3.zero;
			Quaternion q = Quaternion.Euler(0f, 180f, 0f);
			if (this._provider is LeapXRServiceProvider)
			{
				LeapXRServiceProvider leapXRServiceProvider = this._provider as LeapXRServiceProvider;
				a = Vector3.forward * leapXRServiceProvider.deviceOffsetZAxis;
				vector = -Vector3.up * leapXRServiceProvider.deviceOffsetYAxis;
				q = Quaternion.AngleAxis(leapXRServiceProvider.deviceTiltXAxis, Vector3.right);
			}
			else
			{
				Matrix4x4 value = this._camera.projectionMatrix * Matrix4x4.TRS(Vector3.zero, q, Vector3.one) * this._camera.projectionMatrix.inverse;
				Shader.SetGlobalMatrix("_LeapGlobalWarpedOffset", value);
			}
			Matrix4x4 stereoViewMatrix = this._camera.GetStereoViewMatrix(eye);
			this._camera.SetStereoViewMatrix(eye, Matrix4x4.TRS(Vector3.zero, q, Vector3.one) * Matrix4x4.Translate(a + b) * Matrix4x4.Translate(vector) * stereoViewMatrix);
		}

		// Token: 0x0600303D RID: 12349 RVA: 0x000F9D9C File Offset: 0x000F819C
		private void OnDrawGizmos()
		{
			if (this._showEyePositions && Application.isPlaying)
			{
				Matrix4x4 stereoViewMatrix = this._camera.GetStereoViewMatrix(Camera.StereoscopicEye.Left);
				Matrix4x4 stereoViewMatrix2 = this._camera.GetStereoViewMatrix(Camera.StereoscopicEye.Right);
				Vector3 vector = stereoViewMatrix.inverse.MultiplyPoint3x4(Vector3.zero);
				Vector3 vector2 = stereoViewMatrix2.inverse.MultiplyPoint3x4(Vector3.zero);
				Gizmos.color = Color.white;
				Gizmos.DrawSphere(vector, 0.02f);
				Gizmos.DrawSphere(vector2, 0.02f);
				Gizmos.color = Color.blue;
				Gizmos.DrawLine(vector, vector2);
			}
		}

		// Token: 0x04002415 RID: 9237
		[SerializeField]
		private bool _useCustomBaseline;

		// Token: 0x04002416 RID: 9238
		[MinValue(0f)]
		[Units("MM")]
		[InspectorName("Baseline")]
		[SerializeField]
		private float _customBaselineValue = 64f;

		// Token: 0x04002417 RID: 9239
		[SerializeField]
		private bool _showEyePositions;

		// Token: 0x04002418 RID: 9240
		private LeapServiceProvider _provider;

		// Token: 0x04002419 RID: 9241
		private Maybe<float> _deviceBaseline = Maybe.None;

		// Token: 0x0400241A RID: 9242
		private bool _hasVisitedPreCull;

		// Token: 0x0400241B RID: 9243
		private Camera _cachedCamera;
	}
}
