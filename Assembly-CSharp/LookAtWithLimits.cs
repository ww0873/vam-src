using System;
using UnityEngine;

// Token: 0x02000BB1 RID: 2993
public class LookAtWithLimits : MonoBehaviour
{
	// Token: 0x06005555 RID: 21845 RVA: 0x001F3870 File Offset: 0x001F1C70
	public LookAtWithLimits()
	{
	}

	// Token: 0x06005556 RID: 21846 RVA: 0x001F38A3 File Offset: 0x001F1CA3
	private void Start()
	{
		this.startRot = base.transform.localRotation;
	}

	// Token: 0x06005557 RID: 21847 RVA: 0x001F38B8 File Offset: 0x001F1CB8
	private Vector3 GetAxisVector(OrientAxis oa)
	{
		if (oa == OrientAxis.X)
		{
			return base.transform.right;
		}
		if (oa == OrientAxis.NegX)
		{
			return -base.transform.right;
		}
		if (oa == OrientAxis.Y)
		{
			return base.transform.up;
		}
		if (oa == OrientAxis.NegY)
		{
			return -base.transform.up;
		}
		if (oa == OrientAxis.Z)
		{
			return base.transform.forward;
		}
		return -base.transform.forward;
	}

	// Token: 0x06005558 RID: 21848 RVA: 0x001F3940 File Offset: 0x001F1D40
	private void LookAt()
	{
		base.transform.localRotation = this.startRot;
		if (this.target == null && this.lookAtCameraLocation == CameraTarget.CameraLocation.None)
		{
			return;
		}
		if (this.lookAtCameraLocation != CameraTarget.CameraLocation.None)
		{
			CameraTarget.CameraLocation cameraLocation = this.lookAtCameraLocation;
			if (cameraLocation != CameraTarget.CameraLocation.Center)
			{
				if (cameraLocation != CameraTarget.CameraLocation.Left)
				{
					if (cameraLocation == CameraTarget.CameraLocation.Right)
					{
						if (CameraTarget.rightTarget != null)
						{
							this.target = CameraTarget.rightTarget.transform;
						}
					}
				}
				else if (CameraTarget.leftTarget != null)
				{
					this.target = CameraTarget.leftTarget.transform;
				}
			}
			else if (CameraTarget.centerTarget != null)
			{
				this.target = CameraTarget.centerTarget.transform;
			}
		}
		if (this.target != null)
		{
			Vector3 vector = this.target.position - base.transform.position;
			Vector3 axisVector = this.GetAxisVector(this.UpAxis);
			Vector3 axisVector2 = this.GetAxisVector(this.ForwardAxis);
			if (this.centerForDepthAdjust != null && this.DepthAdjust != 0f)
			{
				Vector3 b = (this.target.position - this.centerForDepthAdjust.position) * this.DepthAdjust;
				vector += b;
			}
			Vector3 axisVector3 = this.GetAxisVector(this.RightAxis);
			float magnitude = vector.magnitude;
			if (magnitude < this.MinEngageDistance)
			{
				vector = axisVector2;
			}
			if (this.debug)
			{
				Debug.DrawRay(base.transform.position, axisVector, Color.green);
				Debug.DrawRay(base.transform.position, axisVector2, Color.blue);
				Debug.DrawRay(base.transform.position, axisVector3, Color.red);
			}
			float num = Vector3.Angle(axisVector2, vector);
			Vector3 vector2 = Vector3.zero;
			float num4;
			if (this.on)
			{
				float d = Vector3.Dot(vector, axisVector);
				Vector3 b2 = axisVector * d;
				vector2 = vector - b2;
				if (this.debug)
				{
					Debug.DrawRay(base.transform.position, vector2, Color.cyan);
				}
				float num2 = Vector3.Angle(axisVector2, vector2);
				float num3 = Vector3.Dot(vector2, axisVector3);
				if (this.debug)
				{
					MonoBehaviour.print(string.Concat(new object[]
					{
						"fullangle ",
						num,
						" left/right angle ",
						num2,
						" Dir is ",
						num3
					}));
				}
				if (num3 < 0f)
				{
					num2 = -num2;
				}
				num2 = Mathf.Clamp(num2 - this.LeftRightAngleAdjust, -this.MaxLeft, this.MaxRight) * this.MoveFactor;
				num4 = Mathf.Lerp(this.lastRightLeft, num2, Time.deltaTime * this.smoothFactor);
			}
			else
			{
				num4 = Mathf.Lerp(this.lastRightLeft, 0f, Time.deltaTime * this.smoothFactor);
			}
			if (float.IsNaN(num4))
			{
				Debug.LogError("left right move angle is NaN for " + base.name);
			}
			else
			{
				this.lastRightLeft = num4;
				if (this.UpAxis == OrientAxis.X)
				{
					base.transform.Rotate(num4, 0f, 0f);
				}
				else if (this.UpAxis == OrientAxis.NegX)
				{
					base.transform.Rotate(-num4, 0f, 0f);
				}
				else if (this.UpAxis == OrientAxis.Y)
				{
					base.transform.Rotate(0f, num4, 0f);
				}
				else if (this.UpAxis == OrientAxis.NegY)
				{
					base.transform.Rotate(0f, -num4, 0f);
				}
				else if (this.UpAxis == OrientAxis.Z)
				{
					base.transform.Rotate(0f, 0f, num4);
				}
				else if (this.UpAxis == OrientAxis.NegZ)
				{
					base.transform.Rotate(0f, 0f, -num4);
				}
			}
			if (this.on)
			{
				Vector3 vector3 = Vector3.Cross(axisVector, vector2);
				if (this.debug)
				{
					Debug.DrawRay(base.transform.position, vector3, Color.gray);
				}
				float d2 = Vector3.Dot(vector, vector3);
				Vector3 b3 = vector3 * d2;
				Vector3 vector4 = vector - b3;
				if (this.debug)
				{
					Debug.DrawRay(base.transform.position, vector4, Color.magenta);
				}
				float num5 = Vector3.Angle(vector2, vector4);
				float num6 = Vector3.Dot(vector4, axisVector);
				if (this.debug)
				{
					MonoBehaviour.print(string.Concat(new object[]
					{
						"fullangle ",
						num,
						" up/down angle ",
						num5,
						" Dir is ",
						num6
					}));
				}
				if (num6 > 0f)
				{
					num5 = -num5;
				}
				num5 = Mathf.Clamp(num5 - this.UpDownAngleAdjust, -this.MaxUp, this.MaxDown) * this.MoveFactor;
				num4 = Mathf.Lerp(this.lastUpDown, num5, Time.deltaTime * this.smoothFactor);
			}
			else
			{
				num4 = Mathf.Lerp(this.lastUpDown, 0f, Time.deltaTime * this.smoothFactor);
			}
			if (float.IsNaN(num4))
			{
				Debug.LogError("up down move angle is NaN for " + base.name);
			}
			else
			{
				this.lastUpDown = num4;
				if (this.RightAxis == OrientAxis.X)
				{
					base.transform.Rotate(num4, 0f, 0f);
				}
				else if (this.RightAxis == OrientAxis.NegX)
				{
					base.transform.Rotate(-num4, 0f, 0f);
				}
				else if (this.RightAxis == OrientAxis.Y)
				{
					base.transform.Rotate(0f, num4, 0f);
				}
				else if (this.RightAxis == OrientAxis.NegY)
				{
					base.transform.Rotate(0f, -num4, 0f);
				}
				else if (this.RightAxis == OrientAxis.Z)
				{
					base.transform.Rotate(0f, 0f, num4);
				}
				else if (this.RightAxis == OrientAxis.NegZ)
				{
					base.transform.Rotate(0f, 0f, -num4);
				}
			}
		}
	}

	// Token: 0x06005559 RID: 21849 RVA: 0x001F3FE4 File Offset: 0x001F23E4
	private void Update()
	{
		if (this.updateTime == UpdateTime.Update)
		{
			this.LookAt();
		}
	}

	// Token: 0x0600555A RID: 21850 RVA: 0x001F3FF7 File Offset: 0x001F23F7
	private void LateUpdate()
	{
		if (this.updateTime == UpdateTime.LateUpdate)
		{
			this.LookAt();
		}
	}

	// Token: 0x0600555B RID: 21851 RVA: 0x001F400B File Offset: 0x001F240B
	private void FixedUpdate()
	{
		if (this.updateTime == UpdateTime.FixedUpdate)
		{
			this.LookAt();
		}
	}

	// Token: 0x04004650 RID: 18000
	public bool on = true;

	// Token: 0x04004651 RID: 18001
	public CameraTarget.CameraLocation lookAtCameraLocation;

	// Token: 0x04004652 RID: 18002
	public Transform target;

	// Token: 0x04004653 RID: 18003
	public Transform centerForDepthAdjust;

	// Token: 0x04004654 RID: 18004
	public UpdateTime updateTime;

	// Token: 0x04004655 RID: 18005
	public OrientAxis UpAxis = OrientAxis.Y;

	// Token: 0x04004656 RID: 18006
	public OrientAxis ForwardAxis = OrientAxis.Z;

	// Token: 0x04004657 RID: 18007
	public OrientAxis RightAxis;

	// Token: 0x04004658 RID: 18008
	public float MaxRight;

	// Token: 0x04004659 RID: 18009
	public float MaxLeft;

	// Token: 0x0400465A RID: 18010
	public float MaxUp;

	// Token: 0x0400465B RID: 18011
	public float MaxDown;

	// Token: 0x0400465C RID: 18012
	public float MinEngageDistance;

	// Token: 0x0400465D RID: 18013
	public float smoothFactor = 10f;

	// Token: 0x0400465E RID: 18014
	public float MoveFactor = 1f;

	// Token: 0x0400465F RID: 18015
	public float LeftRightAngleAdjust;

	// Token: 0x04004660 RID: 18016
	public float UpDownAngleAdjust;

	// Token: 0x04004661 RID: 18017
	public float DepthAdjust;

	// Token: 0x04004662 RID: 18018
	public bool debug;

	// Token: 0x04004663 RID: 18019
	private float lastRightLeft;

	// Token: 0x04004664 RID: 18020
	private float lastUpDown;

	// Token: 0x04004665 RID: 18021
	private Quaternion startRot;
}
