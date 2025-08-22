using System;
using mset;
using UnityEngine;

// Token: 0x02000318 RID: 792
public class FreeCamera : MonoBehaviour
{
	// Token: 0x0600129D RID: 4765 RVA: 0x00069388 File Offset: 0x00067788
	public FreeCamera()
	{
	}

	// Token: 0x0600129E RID: 4766 RVA: 0x0006946C File Offset: 0x0006786C
	public void Start()
	{
		Vector3 eulerAngles = base.transform.eulerAngles;
		this.euler.x = eulerAngles.y;
		this.euler.y = eulerAngles.x;
		this.euler.y = Mathf.Repeat(this.euler.y + 180f, 360f) - 180f;
		this.target = new GameObject("_FreeCameraTarget")
		{
			hideFlags = (HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.DontSaveInEditor | HideFlags.NotEditable | HideFlags.DontSaveInBuild | HideFlags.DontUnloadUnusedAsset)
		}.transform;
		if (this.usePivotPoint)
		{
			this.target.position = this.pivotPoint;
			this.targetDist = (base.transform.position - this.target.position).magnitude;
		}
		else if (this.pivotTransform != null)
		{
			this.usePivotPoint = true;
			Vector3 point = base.transform.worldToLocalMatrix.MultiplyPoint3x4(this.pivotTransform.position);
			point.x = 0f;
			point.y = 0f;
			this.targetDist = point.z;
			this.target.position = base.transform.localToWorldMatrix.MultiplyPoint3x4(point);
		}
		else
		{
			this.target.position = base.transform.position + base.transform.forward * this.distance;
			this.targetDist = this.distance;
		}
		this.targetRot = base.transform.rotation;
		this.targetLookAt = this.target.position;
	}

	// Token: 0x0600129F RID: 4767 RVA: 0x00069620 File Offset: 0x00067A20
	public void Update()
	{
		this.inputBounds.x = (float)base.GetComponent<Camera>().pixelWidth * this.paramInputBounds.x;
		this.inputBounds.y = (float)base.GetComponent<Camera>().pixelHeight * this.paramInputBounds.y;
		this.inputBounds.width = (float)base.GetComponent<Camera>().pixelWidth * this.paramInputBounds.width;
		this.inputBounds.height = (float)base.GetComponent<Camera>().pixelHeight * this.paramInputBounds.height;
		if (this.target && this.inputBounds.Contains(Input.mousePosition))
		{
			float num = Input.GetAxis("Mouse X");
			float num2 = Input.GetAxis("Mouse Y");
			bool flag = Input.GetMouseButton(0) || Input.touchCount == 1;
			bool flag2 = Input.GetMouseButton(1) || Input.touchCount == 2;
			bool flag3 = Input.GetMouseButton(2) || Input.touchCount == 3;
			bool flag4 = Input.touchCount >= 4;
			bool flag5 = flag;
			bool flag6 = flag4 || (flag && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)));
			bool flag7 = flag3 || (flag && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)));
			bool flag8 = flag2;
			if (flag6)
			{
				num = num * this.thetaSpeed * 0.02f;
				SkyManager skyManager = SkyManager.Get();
				if (skyManager && skyManager.GlobalSky)
				{
					skyManager.GlobalSky.transform.Rotate(new Vector3(0f, num, 0f));
				}
			}
			else if (flag7)
			{
				num = num * this.moveSpeed * 0.005f * this.targetDist;
				num2 = num2 * this.moveSpeed * 0.005f * this.targetDist;
				this.targetLookAt -= base.transform.up * num2 + base.transform.right * num;
				if (this.useMoveBounds)
				{
					this.targetLookAt.x = Mathf.Clamp(this.targetLookAt.x, -this.moveBounds, this.moveBounds);
					this.targetLookAt.y = Mathf.Clamp(this.targetLookAt.y, -this.moveBounds, this.moveBounds);
					this.targetLookAt.z = Mathf.Clamp(this.targetLookAt.z, -this.moveBounds, this.moveBounds);
				}
			}
			else if (flag8)
			{
				num2 = num2 * this.zoomSpeed * 0.005f * this.targetDist;
				this.targetDist += num2;
				this.targetDist = Mathf.Max(0.1f, this.targetDist);
			}
			else if (flag5)
			{
				num = num * this.thetaSpeed * 0.02f;
				num2 = num2 * this.phiSpeed * 0.02f;
				this.euler.x = this.euler.x + num;
				this.euler.y = this.euler.y - num2;
				this.euler.y = FreeCamera.ClampAngle(this.euler.y, this.phiBoundMin, this.phiBoundMax);
				this.targetRot = Quaternion.Euler(this.euler.y, this.euler.x, 0f);
			}
			this.targetDist -= Input.GetAxis("Mouse ScrollWheel") * this.zoomSpeed * 0.5f;
			this.targetDist = Mathf.Max(0.1f, this.targetDist);
		}
	}

	// Token: 0x060012A0 RID: 4768 RVA: 0x00069A1C File Offset: 0x00067E1C
	public void FixedUpdate()
	{
		this.distance = this.moveSmoothing * this.targetDist + (1f - this.moveSmoothing) * this.distance;
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, this.targetRot, this.rotateSmoothing);
		this.target.position = Vector3.Lerp(this.target.position, this.targetLookAt, this.moveSmoothing);
		this.distanceVec.z = this.distance;
		base.transform.position = this.target.position - base.transform.rotation * this.distanceVec;
	}

	// Token: 0x060012A1 RID: 4769 RVA: 0x00069AE0 File Offset: 0x00067EE0
	private static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360f)
		{
			angle += 360f;
		}
		if (angle > 360f)
		{
			angle -= 360f;
		}
		return Mathf.Clamp(angle, min, max);
	}

	// Token: 0x0400103A RID: 4154
	public float thetaSpeed = 250f;

	// Token: 0x0400103B RID: 4155
	public float phiSpeed = 120f;

	// Token: 0x0400103C RID: 4156
	public float moveSpeed = 10f;

	// Token: 0x0400103D RID: 4157
	public float zoomSpeed = 30f;

	// Token: 0x0400103E RID: 4158
	public float phiBoundMin = -89f;

	// Token: 0x0400103F RID: 4159
	public float phiBoundMax = 89f;

	// Token: 0x04001040 RID: 4160
	public bool useMoveBounds = true;

	// Token: 0x04001041 RID: 4161
	public float moveBounds = 100f;

	// Token: 0x04001042 RID: 4162
	public float rotateSmoothing = 0.5f;

	// Token: 0x04001043 RID: 4163
	public float moveSmoothing = 0.7f;

	// Token: 0x04001044 RID: 4164
	public float distance = 2f;

	// Token: 0x04001045 RID: 4165
	private Vector2 euler;

	// Token: 0x04001046 RID: 4166
	private Quaternion targetRot;

	// Token: 0x04001047 RID: 4167
	private Vector3 targetLookAt;

	// Token: 0x04001048 RID: 4168
	private float targetDist;

	// Token: 0x04001049 RID: 4169
	private Vector3 distanceVec = new Vector3(0f, 0f, 0f);

	// Token: 0x0400104A RID: 4170
	private Transform target;

	// Token: 0x0400104B RID: 4171
	private Rect inputBounds;

	// Token: 0x0400104C RID: 4172
	public Rect paramInputBounds = new Rect(0f, 0f, 1f, 1f);

	// Token: 0x0400104D RID: 4173
	public bool usePivotPoint = true;

	// Token: 0x0400104E RID: 4174
	public Vector3 pivotPoint = new Vector3(0f, 2f, 0f);

	// Token: 0x0400104F RID: 4175
	public Transform pivotTransform;
}
