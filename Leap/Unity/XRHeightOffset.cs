using System;
using Leap.Unity.Attributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Leap.Unity
{
	// Token: 0x02000753 RID: 1875
	[ExecuteInEditMode]
	public class XRHeightOffset : MonoBehaviour
	{
		// Token: 0x0600303E RID: 12350 RVA: 0x000F9E38 File Offset: 0x000F8238
		public XRHeightOffset()
		{
		}

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x0600303F RID: 12351 RVA: 0x000F9E94 File Offset: 0x000F8294
		// (set) Token: 0x06003040 RID: 12352 RVA: 0x000F9E9C File Offset: 0x000F829C
		public float roomScaleHeightOffset
		{
			get
			{
				return this._roomScaleHeightOffset;
			}
			set
			{
				this._roomScaleHeightOffset = value;
				base.transform.position += base.transform.up * (this._roomScaleHeightOffset - this._lastKnownHeightOffset);
				this._lastKnownHeightOffset = value;
			}
		}

		// Token: 0x06003041 RID: 12353 RVA: 0x000F9EEC File Offset: 0x000F82EC
		private void Start()
		{
			this._lastKnownHeightOffset = this._roomScaleHeightOffset;
			if (XRSupportUtil.IsRoomScale())
			{
				base.transform.position -= base.transform.up * this._roomScaleHeightOffset;
			}
			if (this.recenterOnStart)
			{
				XRSupportUtil.Recenter();
			}
		}

		// Token: 0x06003042 RID: 12354 RVA: 0x000F9F4C File Offset: 0x000F834C
		private void Update()
		{
			if (Application.isPlaying)
			{
				bool flag = XRSupportUtil.IsXRDevicePresent();
				if (flag)
				{
					if (this.enableRuntimeAdjustment)
					{
						if (Input.GetKeyDown(this.stepUpKey))
						{
							this.roomScaleHeightOffset += this.stepSize;
						}
						if (Input.GetKeyDown(this.stepDownKey))
						{
							this.roomScaleHeightOffset -= this.stepSize;
						}
					}
					if (this.recenterOnUserPresence && !XRSupportUtil.IsRoomScale())
					{
						bool flag2 = XRSupportUtil.IsUserPresent(true);
						if (this._lastUserPresence != flag2)
						{
							if (flag2)
							{
								XRSupportUtil.Recenter();
							}
							this._lastUserPresence = flag2;
						}
					}
					if (this.recenterOnKey && Input.GetKeyDown(this.recenterKey))
					{
						XRSupportUtil.Recenter();
					}
				}
			}
		}

		// Token: 0x06003043 RID: 12355 RVA: 0x000FA01C File Offset: 0x000F841C
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.Lerp(Color.magenta, Color.white, 0.3f).WithAlpha(0.5f);
			float roomScaleHeightOffset = this.roomScaleHeightOffset;
			int num = 32;
			float num2 = roomScaleHeightOffset * (float)num;
			float d = roomScaleHeightOffset / num2;
			Vector3 a = base.transform.position;
			Vector3 vector = base.transform.rotation * Vector3.down;
			if (Application.isPlaying && XRSupportUtil.IsRoomScale())
			{
				Vector3 b = Vector3.up * roomScaleHeightOffset;
				a += b;
			}
			int num3 = 0;
			while ((float)num3 < num2)
			{
				Vector3 from = a + vector * d * (float)num3;
				Vector3 to = a + vector * d * (float)(num3 + 1);
				Gizmos.DrawLine(from, to);
				num3 += 2;
			}
			Vector3 position = a + vector * roomScaleHeightOffset;
			this.drawCircle(position, vector, 0.01f);
			Gizmos.color = Gizmos.color.WithAlpha(0.3f);
			this.drawCircle(position, vector, 0.1f);
			Gizmos.color = Gizmos.color.WithAlpha(0.2f);
			this.drawCircle(position, vector, 0.2f);
		}

		// Token: 0x06003044 RID: 12356 RVA: 0x000FA16C File Offset: 0x000F856C
		private void drawCircle(Vector3 position, Vector3 normal, float radius)
		{
			Vector3 vector = normal.Perpendicular() * radius;
			Quaternion rotation = Quaternion.AngleAxis(11.25f, normal);
			for (int i = 0; i < 32; i++)
			{
				Vector3 vector2 = rotation * vector;
				Gizmos.DrawLine(position + vector, position + vector2);
				vector = vector2;
			}
		}

		// Token: 0x0400241C RID: 9244
		[Header("Room-scale Height Offset")]
		[SerializeField]
		[OnEditorChange("roomScaleHeightOffset")]
		[Tooltip("This height offset allows you to place your Rig's base location at the approximate head position of your player during edit-time, while still providing correct cross-platform XR rig heights. If the tracking space type is detected as RoomScale, the Rig will be shifted DOWN by this height on Start, matching the expected floor height for, e.g., SteamVR, while the rig remains unchanged for Android VR and Oculus single-camera targets. Use the magenta gizmo as a reference; the circles represent where your floor will be in a Room-scale experience.")]
		[MinValue(0f)]
		private float _roomScaleHeightOffset = 1.6f;

		// Token: 0x0400241D RID: 9245
		private float _lastKnownHeightOffset;

		// Token: 0x0400241E RID: 9246
		[Header("Auto Recenter")]
		[FormerlySerializedAs("autoRecenterOnUserPresence")]
		[Tooltip("If the detected XR device is present and supports userPresence, checking this option will detect when userPresence changes from false to true and call InputTracking.Recenter. Supported in 2017.2 and newer.")]
		public bool recenterOnUserPresence = true;

		// Token: 0x0400241F RID: 9247
		[Tooltip("Calls InputTracking.Recenter on Start().")]
		public bool recenterOnStart = true;

		// Token: 0x04002420 RID: 9248
		[Tooltip("If enabled, InputTracking.Recenter will be called when the assigned key is pressed.")]
		public bool recenterOnKey;

		// Token: 0x04002421 RID: 9249
		[Tooltip("When this key is pressed, InputTracking.Recenter will be called.")]
		public KeyCode recenterKey = KeyCode.R;

		// Token: 0x04002422 RID: 9250
		private bool _lastUserPresence;

		// Token: 0x04002423 RID: 9251
		[Header("Runtime Height Adjustment")]
		[Tooltip("If enabled, then you can use the chosen keys to step the player's height up and down at runtime.")]
		public bool enableRuntimeAdjustment = true;

		// Token: 0x04002424 RID: 9252
		[DisableIf("enableRuntimeAdjustment", false, null)]
		[Tooltip("Press this key on the keyboard to adjust the height offset up by stepSize.")]
		public KeyCode stepUpKey = KeyCode.UpArrow;

		// Token: 0x04002425 RID: 9253
		[DisableIf("enableRuntimeAdjustment", false, null)]
		[Tooltip("Press this key on the keyboard to adjust the height offset down by stepSize.")]
		public KeyCode stepDownKey = KeyCode.DownArrow;

		// Token: 0x04002426 RID: 9254
		[DisableIf("enableRuntimeAdjustment", false, null)]
		public float stepSize = 0.1f;
	}
}
