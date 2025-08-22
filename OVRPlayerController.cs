using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

// Token: 0x0200096B RID: 2411
[RequireComponent(typeof(CharacterController))]
public class OVRPlayerController : MonoBehaviour
{
	// Token: 0x06003C31 RID: 15409 RVA: 0x001231F8 File Offset: 0x001215F8
	public OVRPlayerController()
	{
	}

	// Token: 0x140000C9 RID: 201
	// (add) Token: 0x06003C32 RID: 15410 RVA: 0x001232C0 File Offset: 0x001216C0
	// (remove) Token: 0x06003C33 RID: 15411 RVA: 0x001232F8 File Offset: 0x001216F8
	public event Action<Transform> TransformUpdated
	{
		add
		{
			Action<Transform> action = this.TransformUpdated;
			Action<Transform> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<Transform>>(ref this.TransformUpdated, (Action<Transform>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<Transform> action = this.TransformUpdated;
			Action<Transform> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<Transform>>(ref this.TransformUpdated, (Action<Transform>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	// Token: 0x140000CA RID: 202
	// (add) Token: 0x06003C34 RID: 15412 RVA: 0x00123330 File Offset: 0x00121730
	// (remove) Token: 0x06003C35 RID: 15413 RVA: 0x00123368 File Offset: 0x00121768
	public event Action CameraUpdated
	{
		add
		{
			Action action = this.CameraUpdated;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref this.CameraUpdated, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = this.CameraUpdated;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref this.CameraUpdated, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	// Token: 0x140000CB RID: 203
	// (add) Token: 0x06003C36 RID: 15414 RVA: 0x001233A0 File Offset: 0x001217A0
	// (remove) Token: 0x06003C37 RID: 15415 RVA: 0x001233D8 File Offset: 0x001217D8
	public event Action PreCharacterMove
	{
		add
		{
			Action action = this.PreCharacterMove;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref this.PreCharacterMove, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = this.PreCharacterMove;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref this.PreCharacterMove, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	// Token: 0x170006AE RID: 1710
	// (get) Token: 0x06003C38 RID: 15416 RVA: 0x0012340E File Offset: 0x0012180E
	// (set) Token: 0x06003C39 RID: 15417 RVA: 0x00123416 File Offset: 0x00121816
	public float InitialYRotation
	{
		[CompilerGenerated]
		get
		{
			return this.<InitialYRotation>k__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			this.<InitialYRotation>k__BackingField = value;
		}
	}

	// Token: 0x06003C3A RID: 15418 RVA: 0x00123420 File Offset: 0x00121820
	private void Start()
	{
		Vector3 localPosition = this.CameraRig.transform.localPosition;
		localPosition.z = OVRManager.profile.eyeDepth;
		this.CameraRig.transform.localPosition = localPosition;
	}

	// Token: 0x06003C3B RID: 15419 RVA: 0x00123460 File Offset: 0x00121860
	private void Awake()
	{
		this.Controller = base.gameObject.GetComponent<CharacterController>();
		if (this.Controller == null)
		{
			UnityEngine.Debug.LogWarning("OVRPlayerController: No CharacterController attached.");
		}
		OVRCameraRig[] componentsInChildren = base.gameObject.GetComponentsInChildren<OVRCameraRig>();
		if (componentsInChildren.Length == 0)
		{
			UnityEngine.Debug.LogWarning("OVRPlayerController: No OVRCameraRig attached.");
		}
		else if (componentsInChildren.Length > 1)
		{
			UnityEngine.Debug.LogWarning("OVRPlayerController: More then 1 OVRCameraRig attached.");
		}
		else
		{
			this.CameraRig = componentsInChildren[0];
		}
		this.InitialYRotation = base.transform.rotation.eulerAngles.y;
	}

	// Token: 0x06003C3C RID: 15420 RVA: 0x001234FE File Offset: 0x001218FE
	private void OnEnable()
	{
		OVRManager.display.RecenteredPose += this.ResetOrientation;
		if (this.CameraRig != null)
		{
			this.CameraRig.UpdatedAnchors += this.UpdateTransform;
		}
	}

	// Token: 0x06003C3D RID: 15421 RVA: 0x0012353E File Offset: 0x0012193E
	private void OnDisable()
	{
		OVRManager.display.RecenteredPose -= this.ResetOrientation;
		if (this.CameraRig != null)
		{
			this.CameraRig.UpdatedAnchors -= this.UpdateTransform;
		}
	}

	// Token: 0x06003C3E RID: 15422 RVA: 0x0012357E File Offset: 0x0012197E
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			this.buttonRotation -= this.RotationRatchet;
		}
		if (Input.GetKeyDown(KeyCode.E))
		{
			this.buttonRotation += this.RotationRatchet;
		}
	}

	// Token: 0x06003C3F RID: 15423 RVA: 0x001235C0 File Offset: 0x001219C0
	protected virtual void UpdateController()
	{
		if (this.useProfileData)
		{
			OVRPose? initialPose = this.InitialPose;
			if (initialPose == null)
			{
				this.InitialPose = new OVRPose?(new OVRPose
				{
					position = this.CameraRig.transform.localPosition,
					orientation = this.CameraRig.transform.localRotation
				});
			}
			Vector3 localPosition = this.CameraRig.transform.localPosition;
			if (OVRManager.instance.trackingOriginType == OVRManager.TrackingOrigin.EyeLevel)
			{
				localPosition.y = OVRManager.profile.eyeHeight - 0.5f * this.Controller.height + this.Controller.center.y;
			}
			else if (OVRManager.instance.trackingOriginType == OVRManager.TrackingOrigin.FloorLevel)
			{
				localPosition.y = -(0.5f * this.Controller.height) + this.Controller.center.y;
			}
			this.CameraRig.transform.localPosition = localPosition;
		}
		else
		{
			OVRPose? initialPose2 = this.InitialPose;
			if (initialPose2 != null)
			{
				this.CameraRig.transform.localPosition = this.InitialPose.Value.position;
				this.CameraRig.transform.localRotation = this.InitialPose.Value.orientation;
				this.InitialPose = null;
			}
		}
		this.CameraHeight = this.CameraRig.centerEyeAnchor.localPosition.y;
		if (this.CameraUpdated != null)
		{
			this.CameraUpdated();
		}
		this.UpdateMovement();
		Vector3 vector = Vector3.zero;
		float num = 1f + this.Damping * this.SimulationRate * Time.deltaTime;
		this.MoveThrottle.x = this.MoveThrottle.x / num;
		this.MoveThrottle.y = ((this.MoveThrottle.y <= 0f) ? this.MoveThrottle.y : (this.MoveThrottle.y / num));
		this.MoveThrottle.z = this.MoveThrottle.z / num;
		vector += this.MoveThrottle * this.SimulationRate * Time.deltaTime;
		if (this.Controller.isGrounded && this.FallSpeed <= 0f)
		{
			this.FallSpeed = Physics.gravity.y * (this.GravityModifier * 0.002f);
		}
		else
		{
			this.FallSpeed += Physics.gravity.y * (this.GravityModifier * 0.002f) * this.SimulationRate * Time.deltaTime;
		}
		vector.y += this.FallSpeed * this.SimulationRate * Time.deltaTime;
		if (this.Controller.isGrounded && this.MoveThrottle.y <= base.transform.lossyScale.y * 0.001f)
		{
			float stepOffset = this.Controller.stepOffset;
			Vector3 vector2 = new Vector3(vector.x, 0f, vector.z);
			float d = Mathf.Max(stepOffset, vector2.magnitude);
			vector -= d * Vector3.up;
		}
		if (this.PreCharacterMove != null)
		{
			this.PreCharacterMove();
			this.Teleported = false;
		}
		Vector3 vector3 = Vector3.Scale(this.Controller.transform.localPosition + vector, new Vector3(1f, 0f, 1f));
		this.Controller.Move(vector);
		Vector3 vector4 = Vector3.Scale(this.Controller.transform.localPosition, new Vector3(1f, 0f, 1f));
		if (vector3 != vector4)
		{
			this.MoveThrottle += (vector4 - vector3) / (this.SimulationRate * Time.deltaTime);
		}
	}

	// Token: 0x06003C40 RID: 15424 RVA: 0x00123A14 File Offset: 0x00121E14
	public virtual void UpdateMovement()
	{
		if (this.HaltUpdateMovement)
		{
			return;
		}
		if (this.EnableLinearMovement)
		{
			bool flag = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
			bool flag2 = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
			bool flag3 = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
			bool flag4 = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
			bool flag5 = false;
			if (OVRInput.Get(OVRInput.Button.DpadUp, OVRInput.Controller.Active))
			{
				flag = true;
				flag5 = true;
			}
			if (OVRInput.Get(OVRInput.Button.DpadDown, OVRInput.Controller.Active))
			{
				flag4 = true;
				flag5 = true;
			}
			this.MoveScale = 1f;
			if ((flag && flag2) || (flag && flag3) || (flag4 && flag2) || (flag4 && flag3))
			{
				this.MoveScale = 0.70710677f;
			}
			if (!this.Controller.isGrounded)
			{
				this.MoveScale = 0f;
			}
			this.MoveScale *= this.SimulationRate * Time.deltaTime;
			float num = this.Acceleration * 0.1f * this.MoveScale * this.MoveScaleMultiplier;
			if (flag5 || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
			{
				num *= 2f;
			}
			Vector3 eulerAngles = base.transform.rotation.eulerAngles;
			eulerAngles.z = (eulerAngles.x = 0f);
			Quaternion rotation = Quaternion.Euler(eulerAngles);
			if (flag)
			{
				this.MoveThrottle += rotation * (base.transform.lossyScale.z * num * Vector3.forward);
			}
			if (flag4)
			{
				this.MoveThrottle += rotation * (base.transform.lossyScale.z * num * this.BackAndSideDampen * Vector3.back);
			}
			if (flag2)
			{
				this.MoveThrottle += rotation * (base.transform.lossyScale.x * num * this.BackAndSideDampen * Vector3.left);
			}
			if (flag3)
			{
				this.MoveThrottle += rotation * (base.transform.lossyScale.x * num * this.BackAndSideDampen * Vector3.right);
			}
			num = this.Acceleration * 0.1f * this.MoveScale * this.MoveScaleMultiplier;
			num *= 1f + OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.Active);
			Vector2 vector = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.Active);
			if (this.FixedSpeedSteps > 0)
			{
				vector.y = Mathf.Round(vector.y * (float)this.FixedSpeedSteps) / (float)this.FixedSpeedSteps;
				vector.x = Mathf.Round(vector.x * (float)this.FixedSpeedSteps) / (float)this.FixedSpeedSteps;
			}
			if (vector.y > 0f)
			{
				this.MoveThrottle += rotation * (vector.y * base.transform.lossyScale.z * num * Vector3.forward);
			}
			if (vector.y < 0f)
			{
				this.MoveThrottle += rotation * (Mathf.Abs(vector.y) * base.transform.lossyScale.z * num * this.BackAndSideDampen * Vector3.back);
			}
			if (vector.x < 0f)
			{
				this.MoveThrottle += rotation * (Mathf.Abs(vector.x) * base.transform.lossyScale.x * num * this.BackAndSideDampen * Vector3.left);
			}
			if (vector.x > 0f)
			{
				this.MoveThrottle += rotation * (vector.x * base.transform.lossyScale.x * num * this.BackAndSideDampen * Vector3.right);
			}
		}
		if (this.EnableRotation)
		{
			Vector3 eulerAngles2 = base.transform.rotation.eulerAngles;
			float num2 = this.SimulationRate * Time.deltaTime * this.RotationAmount * this.RotationScaleMultiplier;
			bool flag6 = OVRInput.Get(OVRInput.Button.PrimaryShoulder, OVRInput.Controller.Active);
			if (flag6 && !this.prevHatLeft)
			{
				eulerAngles2.y -= this.RotationRatchet;
			}
			this.prevHatLeft = flag6;
			bool flag7 = OVRInput.Get(OVRInput.Button.SecondaryShoulder, OVRInput.Controller.Active);
			if (flag7 && !this.prevHatRight)
			{
				eulerAngles2.y += this.RotationRatchet;
			}
			this.prevHatRight = flag7;
			eulerAngles2.y += this.buttonRotation;
			this.buttonRotation = 0f;
			if (!this.SkipMouseRotation)
			{
				eulerAngles2.y += Input.GetAxis("Mouse X") * num2 * 3.25f;
			}
			if (this.SnapRotation)
			{
				if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickLeft, OVRInput.Controller.Active))
				{
					if (this.ReadyToSnapTurn)
					{
						eulerAngles2.y -= this.RotationRatchet;
						this.ReadyToSnapTurn = false;
					}
				}
				else if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickRight, OVRInput.Controller.Active))
				{
					if (this.ReadyToSnapTurn)
					{
						eulerAngles2.y += this.RotationRatchet;
						this.ReadyToSnapTurn = false;
					}
				}
				else
				{
					this.ReadyToSnapTurn = true;
				}
			}
			else
			{
				Vector2 vector2 = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick, OVRInput.Controller.Active);
				eulerAngles2.y += vector2.x * num2;
			}
			base.transform.rotation = Quaternion.Euler(eulerAngles2);
		}
	}

	// Token: 0x06003C41 RID: 15425 RVA: 0x001240A8 File Offset: 0x001224A8
	public void UpdateTransform(OVRCameraRig rig)
	{
		Transform trackingSpace = this.CameraRig.trackingSpace;
		Transform centerEyeAnchor = this.CameraRig.centerEyeAnchor;
		if (this.HmdRotatesY && !this.Teleported)
		{
			Vector3 position = trackingSpace.position;
			Quaternion rotation = trackingSpace.rotation;
			base.transform.rotation = Quaternion.Euler(0f, centerEyeAnchor.rotation.eulerAngles.y, 0f);
			trackingSpace.position = position;
			trackingSpace.rotation = rotation;
		}
		this.UpdateController();
		if (this.TransformUpdated != null)
		{
			this.TransformUpdated(trackingSpace);
		}
	}

	// Token: 0x06003C42 RID: 15426 RVA: 0x00124150 File Offset: 0x00122550
	public bool Jump()
	{
		if (!this.Controller.isGrounded)
		{
			return false;
		}
		this.MoveThrottle += new Vector3(0f, base.transform.lossyScale.y * this.JumpForce, 0f);
		return true;
	}

	// Token: 0x06003C43 RID: 15427 RVA: 0x001241AA File Offset: 0x001225AA
	public void Stop()
	{
		this.Controller.Move(Vector3.zero);
		this.MoveThrottle = Vector3.zero;
		this.FallSpeed = 0f;
	}

	// Token: 0x06003C44 RID: 15428 RVA: 0x001241D3 File Offset: 0x001225D3
	public void GetMoveScaleMultiplier(ref float moveScaleMultiplier)
	{
		moveScaleMultiplier = this.MoveScaleMultiplier;
	}

	// Token: 0x06003C45 RID: 15429 RVA: 0x001241DD File Offset: 0x001225DD
	public void SetMoveScaleMultiplier(float moveScaleMultiplier)
	{
		this.MoveScaleMultiplier = moveScaleMultiplier;
	}

	// Token: 0x06003C46 RID: 15430 RVA: 0x001241E6 File Offset: 0x001225E6
	public void GetRotationScaleMultiplier(ref float rotationScaleMultiplier)
	{
		rotationScaleMultiplier = this.RotationScaleMultiplier;
	}

	// Token: 0x06003C47 RID: 15431 RVA: 0x001241F0 File Offset: 0x001225F0
	public void SetRotationScaleMultiplier(float rotationScaleMultiplier)
	{
		this.RotationScaleMultiplier = rotationScaleMultiplier;
	}

	// Token: 0x06003C48 RID: 15432 RVA: 0x001241F9 File Offset: 0x001225F9
	public void GetSkipMouseRotation(ref bool skipMouseRotation)
	{
		skipMouseRotation = this.SkipMouseRotation;
	}

	// Token: 0x06003C49 RID: 15433 RVA: 0x00124203 File Offset: 0x00122603
	public void SetSkipMouseRotation(bool skipMouseRotation)
	{
		this.SkipMouseRotation = skipMouseRotation;
	}

	// Token: 0x06003C4A RID: 15434 RVA: 0x0012420C File Offset: 0x0012260C
	public void GetHaltUpdateMovement(ref bool haltUpdateMovement)
	{
		haltUpdateMovement = this.HaltUpdateMovement;
	}

	// Token: 0x06003C4B RID: 15435 RVA: 0x00124216 File Offset: 0x00122616
	public void SetHaltUpdateMovement(bool haltUpdateMovement)
	{
		this.HaltUpdateMovement = haltUpdateMovement;
	}

	// Token: 0x06003C4C RID: 15436 RVA: 0x00124220 File Offset: 0x00122620
	public void ResetOrientation()
	{
		if (this.HmdResetsY && !this.HmdRotatesY)
		{
			Vector3 eulerAngles = base.transform.rotation.eulerAngles;
			eulerAngles.y = this.InitialYRotation;
			base.transform.rotation = Quaternion.Euler(eulerAngles);
		}
	}

	// Token: 0x04002E17 RID: 11799
	public float Acceleration = 0.1f;

	// Token: 0x04002E18 RID: 11800
	public float Damping = 0.3f;

	// Token: 0x04002E19 RID: 11801
	public float BackAndSideDampen = 0.5f;

	// Token: 0x04002E1A RID: 11802
	public float JumpForce = 0.3f;

	// Token: 0x04002E1B RID: 11803
	public float RotationAmount = 1.5f;

	// Token: 0x04002E1C RID: 11804
	public float RotationRatchet = 45f;

	// Token: 0x04002E1D RID: 11805
	[Tooltip("The player will rotate in fixed steps if Snap Rotation is enabled.")]
	public bool SnapRotation = true;

	// Token: 0x04002E1E RID: 11806
	[Tooltip("How many fixed speeds to use with linear movement? 0=linear control")]
	public int FixedSpeedSteps;

	// Token: 0x04002E1F RID: 11807
	public bool HmdResetsY = true;

	// Token: 0x04002E20 RID: 11808
	public bool HmdRotatesY = true;

	// Token: 0x04002E21 RID: 11809
	public float GravityModifier = 0.379f;

	// Token: 0x04002E22 RID: 11810
	public bool useProfileData = true;

	// Token: 0x04002E23 RID: 11811
	[NonSerialized]
	public float CameraHeight;

	// Token: 0x04002E24 RID: 11812
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<Transform> TransformUpdated;

	// Token: 0x04002E25 RID: 11813
	[NonSerialized]
	public bool Teleported;

	// Token: 0x04002E26 RID: 11814
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action CameraUpdated;

	// Token: 0x04002E27 RID: 11815
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action PreCharacterMove;

	// Token: 0x04002E28 RID: 11816
	public bool EnableLinearMovement = true;

	// Token: 0x04002E29 RID: 11817
	public bool EnableRotation = true;

	// Token: 0x04002E2A RID: 11818
	protected CharacterController Controller;

	// Token: 0x04002E2B RID: 11819
	protected OVRCameraRig CameraRig;

	// Token: 0x04002E2C RID: 11820
	private float MoveScale = 1f;

	// Token: 0x04002E2D RID: 11821
	private Vector3 MoveThrottle = Vector3.zero;

	// Token: 0x04002E2E RID: 11822
	private float FallSpeed;

	// Token: 0x04002E2F RID: 11823
	private OVRPose? InitialPose;

	// Token: 0x04002E30 RID: 11824
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private float <InitialYRotation>k__BackingField;

	// Token: 0x04002E31 RID: 11825
	private float MoveScaleMultiplier = 1f;

	// Token: 0x04002E32 RID: 11826
	private float RotationScaleMultiplier = 1f;

	// Token: 0x04002E33 RID: 11827
	private bool SkipMouseRotation = true;

	// Token: 0x04002E34 RID: 11828
	private bool HaltUpdateMovement;

	// Token: 0x04002E35 RID: 11829
	private bool prevHatLeft;

	// Token: 0x04002E36 RID: 11830
	private bool prevHatRight;

	// Token: 0x04002E37 RID: 11831
	private float SimulationRate = 60f;

	// Token: 0x04002E38 RID: 11832
	private float buttonRotation;

	// Token: 0x04002E39 RID: 11833
	private bool ReadyToSnapTurn;
}
