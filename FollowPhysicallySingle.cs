using System;
using UnityEngine;

// Token: 0x02000D5D RID: 3421
public class FollowPhysicallySingle : MonoBehaviour
{
	// Token: 0x06006927 RID: 26919 RVA: 0x00276D44 File Offset: 0x00275144
	public FollowPhysicallySingle()
	{
	}

	// Token: 0x17000F87 RID: 3975
	// (get) Token: 0x06006928 RID: 26920 RVA: 0x00276E1F File Offset: 0x0027521F
	// (set) Token: 0x06006929 RID: 26921 RVA: 0x00276E27 File Offset: 0x00275227
	public float ForcePercent
	{
		get
		{
			return this._ForcePercent;
		}
		set
		{
			this._ForcePercent = value;
		}
	}

	// Token: 0x17000F88 RID: 3976
	// (get) Token: 0x0600692A RID: 26922 RVA: 0x00276E30 File Offset: 0x00275230
	// (set) Token: 0x0600692B RID: 26923 RVA: 0x00276E38 File Offset: 0x00275238
	public float TorquePercent
	{
		get
		{
			return this._TorquePercent;
		}
		set
		{
			this._TorquePercent = value;
		}
	}

	// Token: 0x17000F89 RID: 3977
	// (get) Token: 0x0600692C RID: 26924 RVA: 0x00276E41 File Offset: 0x00275241
	// (set) Token: 0x0600692D RID: 26925 RVA: 0x00276E48 File Offset: 0x00275248
	public float StaticGlobalForceMultipler
	{
		get
		{
			return FollowPhysicallySingle.GlobalForceMultiplier;
		}
		set
		{
			FollowPhysicallySingle.GlobalForceMultiplier = value;
		}
	}

	// Token: 0x17000F8A RID: 3978
	// (get) Token: 0x0600692E RID: 26926 RVA: 0x00276E50 File Offset: 0x00275250
	// (set) Token: 0x0600692F RID: 26927 RVA: 0x00276E57 File Offset: 0x00275257
	public float StaticGlobalTorqueMultipler
	{
		get
		{
			return FollowPhysicallySingle.GlobalTorqueMultiplier;
		}
		set
		{
			FollowPhysicallySingle.GlobalTorqueMultiplier = value;
		}
	}

	// Token: 0x06006930 RID: 26928 RVA: 0x00276E60 File Offset: 0x00275260
	private void Start()
	{
		this.RB = base.transform.GetComponent<Rigidbody>();
		this.CJ = base.transform.GetComponent<ConfigurableJoint>();
		if (this.lineMaterial)
		{
			this.lineDrawer = new LineDrawer(this.lineMaterial);
		}
		if (this.rotationLineMaterial)
		{
			this.rotationLineDrawer = new LineDrawer(12, this.rotationLineMaterial);
		}
		if (this.CJ)
		{
			this.startingJointDrive = this.CJ.slerpDrive;
			this.controlledJointDrive = default(JointDrive);
			this.controlledJointDrive.positionSpring = this.controlledJointSpring;
			this.controlledJointDrive.positionDamper = this.startingJointDrive.positionDamper;
			this.controlledJointDrive.maximumForce = this.controlledJointMaxForce;
			this.usingStartingJointConditions = true;
		}
		if (this.follow)
		{
			this.previousPosition = this.follow.position;
			this.followFCV3 = this.follow.GetComponent<FreeControllerV3>();
		}
	}

	// Token: 0x06006931 RID: 26929 RVA: 0x00276F74 File Offset: 0x00275374
	private void applyTorque()
	{
		Vector3 a = Vector3.Cross(base.transform.forward, this.follow.forward);
		Vector3 b = Vector3.Cross(base.transform.up, this.follow.up);
		Vector3 a2 = a + b;
		Vector3 vector = a2 * this.TorqueMultiplier * this._TorquePercent;
		if (this.useGlobalTorqueMultiplier)
		{
			vector *= FollowPhysicallySingle.GlobalTorqueMultiplier;
		}
		Vector3 vector2 = Vector3.ClampMagnitude(vector, this.MaxTorque);
		if (this.debugTorque)
		{
			DebugHUD.Msg(string.Concat(new object[]
			{
				base.transform.name,
				" RAW tq: ",
				vector,
				" clamped: ",
				vector2
			}));
		}
		this.RB.AddTorque(vector2, ForceMode.Force);
		this.DebugAppliedTorque = vector2;
	}

	// Token: 0x06006932 RID: 26930 RVA: 0x00277060 File Offset: 0x00275460
	private void applyTorquePID()
	{
		float fixedDeltaTime = Time.fixedDeltaTime;
		Vector3 a = Vector3.Cross(base.transform.forward, this.follow.forward);
		Vector3 b = Vector3.Cross(base.transform.up, this.follow.up);
		Vector3 vector = a + b;
		this.integralRotation += vector * fixedDeltaTime;
		Vector3 vector2 = (vector - this.lastErrorRotation) / fixedDeltaTime;
		this.lastErrorRotation = vector;
		Vector3 vector3 = (vector * this.PIDpresentFactorRot + this.integralRotation * this.PIDintegralFactorRot + vector2 * this.PIDderivFactorRot) * this.TorqueMultiplier * this._TorquePercent;
		if (this.useGlobalTorqueMultiplier)
		{
			vector3 *= FollowPhysicallySingle.GlobalTorqueMultiplier;
		}
		Vector3 vector4 = Vector3.ClampMagnitude(vector3, this.MaxTorque);
		if (this.debugTorque)
		{
			DebugHUD.Msg(string.Concat(new object[]
			{
				base.transform.name,
				" RTq: ",
				vector3,
				" CTq: ",
				vector4,
				" A:",
				vector,
				" D:",
				vector2
			}));
		}
		this.RB.AddTorque(vector4, ForceMode.Force);
		this.DebugAppliedTorque = vector4;
	}

	// Token: 0x06006933 RID: 26931 RVA: 0x002771E4 File Offset: 0x002755E4
	private void applyTorquePID2()
	{
		float fixedDeltaTime = Time.fixedDeltaTime;
		Vector3 a = Vector3.Cross(base.transform.up, this.follow.up);
		this.integralRotation += a * fixedDeltaTime;
		Vector3 a2 = (a - this.lastErrorRotation) / fixedDeltaTime;
		Vector3 b = this.lastErrorRotation;
		this.lastErrorRotation = a;
		Vector3 a3 = a * this.PIDpresentFactorRot;
		Vector3 b2 = this.integralRotation * this.PIDintegralFactorRot;
		Vector3 vector = a2 * this.PIDderivFactorRot;
		Vector3 b3 = vector;
		Vector3 vector2 = (a3 + b2 + b3) * this.TorqueMultiplier * this._TorquePercent;
		if (this.useGlobalTorqueMultiplier)
		{
			vector2 *= FollowPhysicallySingle.GlobalTorqueMultiplier;
		}
		Vector3 vector3 = Vector3.ClampMagnitude(vector2, this.MaxTorque);
		if (this.debugTorque)
		{
			Debug.Log(string.Concat(new string[]
			{
				base.transform.name,
				" alignup last error: ",
				b.ToString("F2"),
				" align:",
				a.ToString("F2"),
				" deriv:",
				(a - b).ToString("F2"),
				" aError ",
				a3.ToString("F2"),
				" dError ",
				vector.ToString("F2")
			}));
		}
		this.RB.AddTorque(vector3, ForceMode.Force);
		this.DebugAppliedTorque = vector3;
		Vector3 a4 = Vector3.Cross(base.transform.forward, this.follow.forward);
		this.integralRotationForward += a4 * fixedDeltaTime;
		Vector3 a5 = (a4 - this.lastErrorRotationForward) / fixedDeltaTime;
		this.lastErrorRotationForward = a4;
		Vector3 a6 = a4 * this.PIDpresentFactorRot;
		Vector3 b4 = this.integralRotationForward * this.PIDintegralFactorRot;
		Vector3 vector4 = a5 * this.PIDderivFactorRot;
		Vector3 b5 = vector4;
		Vector3 vector5 = (a6 + b4 + b5) * this.TorqueMultiplier2 * this._TorquePercent;
		if (this.useGlobalTorqueMultiplier)
		{
			vector5 *= FollowPhysicallySingle.GlobalTorqueMultiplier;
		}
		Vector3 vector6 = Vector3.ClampMagnitude(vector5, this.MaxTorque);
		if (this.debugTorque)
		{
			Debug.Log(string.Concat(new object[]
			{
				base.transform.name,
				" apply alignforward raw torque ",
				vector5,
				" clamped torque ",
				vector6
			}));
		}
		this.RB.AddTorque(vector6, ForceMode.Force);
		this.DebugAppliedTorque2 = vector6;
	}

	// Token: 0x06006934 RID: 26932 RVA: 0x002774C8 File Offset: 0x002758C8
	private void applyTorquePID3()
	{
		float fixedDeltaTime = Time.fixedDeltaTime;
		Quaternion q = this.follow.rotation * Quaternion.Inverse(base.transform.rotation);
		Vector3 angles = Quaternion2Angles.GetAngles(q, this.rotationOrder);
		Vector3 a = angles * this.TorqueMultiplier * this._TorquePercent;
		this.integralRotation += a * fixedDeltaTime;
		Vector3 a2 = (a - this.lastErrorRotation) / fixedDeltaTime;
		this.lastErrorRotation = a;
		Vector3 vector = (a * this.PIDpresentFactorRot + this.integralRotation * this.PIDintegralFactorRot + a2 * this.PIDderivFactorRot) * this.TorqueMultiplier * this._TorquePercent;
		if (this.useGlobalTorqueMultiplier)
		{
			vector *= FollowPhysicallySingle.GlobalTorqueMultiplier;
		}
		Vector3 vector2 = Vector3.ClampMagnitude(vector, this.MaxTorque);
		this.RB.AddTorque(vector2, ForceMode.Force);
		this.DebugAppliedTorque = vector2;
	}

	// Token: 0x06006935 RID: 26933 RVA: 0x002775E0 File Offset: 0x002759E0
	private void applyForce()
	{
		Vector3 position = this.follow.position;
		Vector3 velocity = this.RB.velocity;
		Vector3 a = position - this.previousPosition;
		this.previousPosition = position;
		Vector3 a2 = a - velocity;
		Vector3 b = a2 * this.VelocityVsDistancePower;
		Vector3 a3 = position - base.transform.position;
		Vector3 a4 = a3 * (1f - this.VelocityVsDistancePower);
		Vector3 a5 = a4 + b;
		Vector3 vector = a5 * this.ForceMultiplier * this._ForcePercent;
		if (this.useGlobalForceMultiplier)
		{
			vector *= FollowPhysicallySingle.GlobalForceMultiplier;
		}
		Vector3 vector2 = Vector3.ClampMagnitude(vector, this.MaxForce);
		if (this.debugForce)
		{
			DebugHUD.Msg(string.Concat(new object[]
			{
				base.transform.name,
				" RAW frc: ",
				vector,
				" clamped: ",
				vector2
			}));
		}
		if (this.forcePosition == FollowPhysicallySingle.ForcePosition.RBCenter)
		{
			this.RB.AddForce(vector2, ForceMode.Force);
		}
		else
		{
			this.RB.AddForceAtPosition(vector2, base.transform.position, ForceMode.Force);
		}
		this.DebugAppliedForce = vector2;
	}

	// Token: 0x06006936 RID: 26934 RVA: 0x00277730 File Offset: 0x00275B30
	private void applyForcePID()
	{
		float fixedDeltaTime = Time.fixedDeltaTime;
		Vector3 a = this.follow.position - base.transform.position;
		this.integralPosition += a * fixedDeltaTime;
		Vector3 a2 = (a - this.lastErrorPosition) / fixedDeltaTime;
		this.lastErrorPosition = a;
		Vector3 vector = (a * this.PIDpresentFactorPos + this.integralPosition * this.PIDintegralFactorPos + a2 * this.PIDderivFactorPos) * this.ForceMultiplier * this._ForcePercent;
		if (this.useGlobalForceMultiplier)
		{
			vector *= FollowPhysicallySingle.GlobalForceMultiplier;
		}
		Vector3 vector2 = Vector3.ClampMagnitude(vector, this.MaxForce);
		if (this.forcePosition == FollowPhysicallySingle.ForcePosition.RBCenter)
		{
			this.RB.AddForce(vector2, ForceMode.Force);
		}
		else
		{
			this.RB.AddForceAtPosition(vector2, base.transform.position, ForceMode.Force);
		}
		this.DebugAppliedForce = vector2;
	}

	// Token: 0x06006937 RID: 26935 RVA: 0x0027783F File Offset: 0x00275C3F
	private void setRotation()
	{
		this.RB.MoveRotation(this.follow.rotation);
	}

	// Token: 0x06006938 RID: 26936 RVA: 0x00277857 File Offset: 0x00275C57
	private void setPosition()
	{
		this.RB.MovePosition(this.follow.position);
	}

	// Token: 0x06006939 RID: 26937 RVA: 0x0027786F File Offset: 0x00275C6F
	private void drawPositionLines()
	{
		this.lineDrawer.SetLinePoints(base.transform.position, this.follow.position);
		this.lineDrawer.Draw(0);
	}

	// Token: 0x0600693A RID: 26938 RVA: 0x002778A0 File Offset: 0x00275CA0
	private void drawRotationLines()
	{
		float d = 0.02f;
		Vector3 vector = base.transform.forward * d;
		Vector3 vector2 = this.follow.forward * d;
		Vector3 position = base.transform.position;
		Vector3 point = position + vector;
		Vector3 point2 = position + vector2;
		Vector3 point3 = position + vector * 2f;
		Vector3 point4 = position + vector2 * 2f;
		Vector3 point5 = position + vector * 3f;
		Vector3 point6 = position + vector2 * 3f;
		Vector3 vector3 = position + vector * 4f;
		Vector3 point7 = position + vector2 * 4f;
		this.rotationLineDrawer.SetLinePoints(0, position, vector3);
		this.rotationLineDrawer.SetLinePoints(1, position, point7);
		this.rotationLineDrawer.SetLinePoints(2, point, point2);
		this.rotationLineDrawer.SetLinePoints(3, point3, point4);
		this.rotationLineDrawer.SetLinePoints(4, point5, point6);
		this.rotationLineDrawer.SetLinePoints(5, vector3, point7);
		this.rotationLineDrawer.Draw(0);
		Vector3 vector4 = base.transform.up * d;
		Vector3 vector5 = this.follow.up * d;
		Vector3 position2 = base.transform.position;
		Vector3 point8 = position2 + vector4;
		Vector3 point9 = position2 + vector5;
		Vector3 point10 = position2 + vector4 * 2f;
		Vector3 point11 = position2 + vector5 * 2f;
		Vector3 point12 = position2 + vector4 * 3f;
		Vector3 point13 = position2 + vector5 * 3f;
		Vector3 vector6 = position2 + vector4 * 4f;
		Vector3 point14 = position2 + vector5 * 4f;
		this.rotationLineDrawer.SetLinePoints(6, position2, vector6);
		this.rotationLineDrawer.SetLinePoints(7, position2, point14);
		this.rotationLineDrawer.SetLinePoints(8, point8, point9);
		this.rotationLineDrawer.SetLinePoints(9, point10, point11);
		this.rotationLineDrawer.SetLinePoints(10, point12, point13);
		this.rotationLineDrawer.SetLinePoints(11, vector6, point14);
		this.rotationLineDrawer.Draw(0);
	}

	// Token: 0x0600693B RID: 26939 RVA: 0x00277B08 File Offset: 0x00275F08
	private void Update()
	{
		if (this.lineDrawer != null && this.drivePosition && this.RB != null && this.follow != null && this.followFCV3 != null && this.followFCV3.isPositionOn && !this.followFCV3.hidden)
		{
			this.drawPositionLines();
		}
		if (this.rotationLineDrawer != null && this.driveRotation && this.RB != null && this.follow != null && this.followFCV3 != null && this.followFCV3.isRotationOn && !this.followFCV3.hidden)
		{
			this.drawRotationLines();
		}
	}

	// Token: 0x0600693C RID: 26940 RVA: 0x00277BF4 File Offset: 0x00275FF4
	private void FixedUpdate()
	{
		if (this.onIfAllFCV3sFollowing != null && this.onIfAllFCV3sFollowing.Length > 0)
		{
			this.on = true;
			bool flag = true;
			foreach (FreeControllerV3 freeControllerV in this.onIfAllFCV3sFollowing)
			{
				if (freeControllerV.currentRotationState != FreeControllerV3.RotationState.Following || freeControllerV.currentPositionState != FreeControllerV3.PositionState.Following)
				{
					this.on = false;
				}
				if (!freeControllerV.hidden)
				{
					flag = false;
				}
			}
			if (this.on && !flag)
			{
				if (this.lineDrawer != null)
				{
					this.drawPositionLines();
				}
				if (this.rotationLineDrawer != null)
				{
					this.drawRotationLines();
				}
			}
		}
		bool flag2 = false;
		if (this.on)
		{
			if (this.driveRotation && this.RB != null && this.follow != null && (this.followFCV3 == null || this.followFCV3.isRotationOn))
			{
				if (this.rotateMethod == FollowPhysicallySingle.Method.Move)
				{
					this.setRotation();
				}
				else if (this.rotateMethod == FollowPhysicallySingle.Method.Force)
				{
					flag2 = true;
					this.applyTorquePID3();
				}
			}
			if (this.drivePosition && this.RB != null && this.follow != null && (this.followFCV3 == null || this.followFCV3.isPositionOn))
			{
				if (this.moveMethod == FollowPhysicallySingle.Method.Move)
				{
					this.setPosition();
				}
				else if (this.moveMethod == FollowPhysicallySingle.Method.Force)
				{
					this.applyForcePID();
				}
			}
		}
		if (this.useControlJointParams && this.CJ && flag2 && this.usingStartingJointConditions)
		{
			this.usingStartingJointConditions = false;
			this.CJ.slerpDrive = this.controlledJointDrive;
		}
		if (this.CJ && !flag2 && !this.usingStartingJointConditions)
		{
			this.usingStartingJointConditions = true;
			this.CJ.slerpDrive = this.startingJointDrive;
		}
	}

	// Token: 0x0600693D RID: 26941 RVA: 0x00277E1D File Offset: 0x0027621D
	// Note: this type is marked as 'beforefieldinit'.
	static FollowPhysicallySingle()
	{
	}

	// Token: 0x040059F6 RID: 23030
	public static float GlobalForceMultiplier = 5f;

	// Token: 0x040059F7 RID: 23031
	public static float GlobalTorqueMultiplier = 5f;

	// Token: 0x040059F8 RID: 23032
	public bool useGlobalForceMultiplier = true;

	// Token: 0x040059F9 RID: 23033
	public bool useGlobalTorqueMultiplier = true;

	// Token: 0x040059FA RID: 23034
	public bool on = true;

	// Token: 0x040059FB RID: 23035
	public bool drivePosition = true;

	// Token: 0x040059FC RID: 23036
	public bool driveRotation = true;

	// Token: 0x040059FD RID: 23037
	public Transform follow;

	// Token: 0x040059FE RID: 23038
	public FollowPhysicallySingle.Method moveMethod;

	// Token: 0x040059FF RID: 23039
	public FollowPhysicallySingle.Method rotateMethod;

	// Token: 0x04005A00 RID: 23040
	public float PIDpresentFactorRot = 1f;

	// Token: 0x04005A01 RID: 23041
	public float PIDintegralFactorRot;

	// Token: 0x04005A02 RID: 23042
	public float PIDderivFactorRot = 0.1f;

	// Token: 0x04005A03 RID: 23043
	public float PIDpresentFactorPos = 1f;

	// Token: 0x04005A04 RID: 23044
	public float PIDintegralFactorPos;

	// Token: 0x04005A05 RID: 23045
	public float PIDderivFactorPos = 0.1f;

	// Token: 0x04005A06 RID: 23046
	public FollowPhysicallySingle.ForcePosition forcePosition;

	// Token: 0x04005A07 RID: 23047
	public Quaternion2Angles.RotationOrder rotationOrder;

	// Token: 0x04005A08 RID: 23048
	public float ForceMultiplier = 100f;

	// Token: 0x04005A09 RID: 23049
	public float TorqueMultiplier = 50f;

	// Token: 0x04005A0A RID: 23050
	public float TorqueMultiplier2 = 5f;

	// Token: 0x04005A0B RID: 23051
	public Vector3 DebugAngles;

	// Token: 0x04005A0C RID: 23052
	public Vector3 DebugAppliedForce;

	// Token: 0x04005A0D RID: 23053
	public Vector3 DebugAppliedTorque;

	// Token: 0x04005A0E RID: 23054
	public Vector3 DebugAppliedTorque2;

	// Token: 0x04005A0F RID: 23055
	public Vector3 DebugLastErrorRotation;

	// Token: 0x04005A10 RID: 23056
	[SerializeField]
	private float _ForcePercent = 1f;

	// Token: 0x04005A11 RID: 23057
	[SerializeField]
	private float _TorquePercent = 1f;

	// Token: 0x04005A12 RID: 23058
	public float MaxForce = 100f;

	// Token: 0x04005A13 RID: 23059
	public float MaxTorque = 50f;

	// Token: 0x04005A14 RID: 23060
	public float freezeMass = 100f;

	// Token: 0x04005A15 RID: 23061
	public bool useControlJointParams;

	// Token: 0x04005A16 RID: 23062
	public float controlledJointSpring = 0.005f;

	// Token: 0x04005A17 RID: 23063
	public float controlledJointMaxForce = 0.005f;

	// Token: 0x04005A18 RID: 23064
	public bool debugForce;

	// Token: 0x04005A19 RID: 23065
	public bool debugTorque;

	// Token: 0x04005A1A RID: 23066
	public Material lineMaterial;

	// Token: 0x04005A1B RID: 23067
	public Material rotationLineMaterial;

	// Token: 0x04005A1C RID: 23068
	public FreeControllerV3[] onIfAllFCV3sFollowing;

	// Token: 0x04005A1D RID: 23069
	private float VelocityVsDistancePower = 0.05f;

	// Token: 0x04005A1E RID: 23070
	private Rigidbody RB;

	// Token: 0x04005A1F RID: 23071
	private ConfigurableJoint CJ;

	// Token: 0x04005A20 RID: 23072
	private JointDrive startingJointDrive;

	// Token: 0x04005A21 RID: 23073
	private JointDrive controlledJointDrive;

	// Token: 0x04005A22 RID: 23074
	private Vector3 integralPosition;

	// Token: 0x04005A23 RID: 23075
	private Vector3 lastErrorPosition;

	// Token: 0x04005A24 RID: 23076
	private Vector3 integralRotation;

	// Token: 0x04005A25 RID: 23077
	private Vector3 lastErrorRotation;

	// Token: 0x04005A26 RID: 23078
	private Vector3 integralRotationForward;

	// Token: 0x04005A27 RID: 23079
	private Vector3 lastErrorRotationForward;

	// Token: 0x04005A28 RID: 23080
	private Vector3 previousPosition;

	// Token: 0x04005A29 RID: 23081
	private FreeControllerV3 followFCV3;

	// Token: 0x04005A2A RID: 23082
	private float startingMass;

	// Token: 0x04005A2B RID: 23083
	private bool startingUseGravity;

	// Token: 0x04005A2C RID: 23084
	private RigidbodyConstraints startingConstraints;

	// Token: 0x04005A2D RID: 23085
	private LineDrawer lineDrawer;

	// Token: 0x04005A2E RID: 23086
	private LineDrawer rotationLineDrawer;

	// Token: 0x04005A2F RID: 23087
	private bool usingStartingGravity;

	// Token: 0x04005A30 RID: 23088
	private bool usingStartingJointConditions;

	// Token: 0x02000D5E RID: 3422
	public enum Method
	{
		// Token: 0x04005A32 RID: 23090
		Force,
		// Token: 0x04005A33 RID: 23091
		Move
	}

	// Token: 0x02000D5F RID: 3423
	public enum ForcePosition
	{
		// Token: 0x04005A35 RID: 23093
		RBCenter,
		// Token: 0x04005A36 RID: 23094
		HingePoint
	}
}
