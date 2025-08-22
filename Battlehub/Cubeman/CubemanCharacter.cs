using System;
using UnityEngine;

namespace Battlehub.Cubeman
{
	// Token: 0x020000A2 RID: 162
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(CapsuleCollider))]
	[RequireComponent(typeof(Animator))]
	public class CubemanCharacter : MonoBehaviour
	{
		// Token: 0x0600025E RID: 606 RVA: 0x00011488 File Offset: 0x0000F888
		public CubemanCharacter()
		{
		}

		// Token: 0x0600025F RID: 607 RVA: 0x000114F4 File Offset: 0x0000F8F4
		private void Awake()
		{
			this.m_Animator = base.GetComponent<Animator>();
			this.m_Rigidbody = base.GetComponent<Rigidbody>();
			this.m_Capsule = base.GetComponent<CapsuleCollider>();
			this.m_CapsuleHeight = this.m_Capsule.height;
			this.m_CapsuleCenter = this.m_Capsule.center;
			this.m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
			this.m_OrigGroundCheckDistance = this.m_GroundCheckDistance;
		}

		// Token: 0x06000260 RID: 608 RVA: 0x00011560 File Offset: 0x0000F960
		private void Start()
		{
			if (this.Enabled)
			{
				this.CheckGroundStatus();
			}
			else
			{
				this.m_GroundNormal = Vector3.up;
				this.m_IsGrounded = true;
				this.m_Animator.applyRootMotion = true;
			}
		}

		// Token: 0x06000261 RID: 609 RVA: 0x00011598 File Offset: 0x0000F998
		public void Move(Vector3 move, bool crouch, bool jump)
		{
			if (this.Enabled)
			{
				if (move.magnitude > 1f)
				{
					move.Normalize();
				}
				move = base.transform.InverseTransformDirection(move);
				this.CheckGroundStatus();
				move = Vector3.ProjectOnPlane(move, this.m_GroundNormal);
				this.m_TurnAmount = Mathf.Atan2(move.x, move.z);
				this.m_ForwardAmount = move.z;
				this.ApplyExtraTurnRotation();
				if (this.m_IsGrounded)
				{
					this.HandleGroundedMovement(crouch, jump);
				}
				else
				{
					this.HandleAirborneMovement();
				}
				this.ScaleCapsuleForCrouching(crouch);
				this.PreventStandingInLowHeadroom();
			}
			if (this.m_Animator != null && this.m_Animator.isInitialized)
			{
				this.UpdateAnimator(move);
			}
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0001166C File Offset: 0x0000FA6C
		private void ScaleCapsuleForCrouching(bool crouch)
		{
			if (this.m_IsGrounded && crouch)
			{
				if (this.m_Crouching)
				{
					return;
				}
				this.m_Capsule.height = this.m_Capsule.height / 1.5f;
				this.m_Capsule.center = this.m_Capsule.center / 1.5f;
				this.m_Crouching = true;
			}
			else
			{
				Ray ray = new Ray(this.m_Rigidbody.position + Vector3.up * this.m_Capsule.radius * 0.5f, Vector3.up);
				float maxDistance = this.m_CapsuleHeight - this.m_Capsule.radius * 0.5f;
				if (Physics.SphereCast(ray, this.m_Capsule.radius * 0.5f, maxDistance, -1, QueryTriggerInteraction.Ignore))
				{
					this.m_Crouching = true;
					return;
				}
				this.m_Capsule.height = this.m_CapsuleHeight;
				this.m_Capsule.center = this.m_CapsuleCenter;
				this.m_Crouching = false;
			}
		}

		// Token: 0x06000263 RID: 611 RVA: 0x00011784 File Offset: 0x0000FB84
		private void PreventStandingInLowHeadroom()
		{
			if (!this.m_Crouching)
			{
				Ray ray = new Ray(this.m_Rigidbody.position + Vector3.up * this.m_Capsule.radius * 0.5f, Vector3.up);
				float maxDistance = this.m_CapsuleHeight - this.m_Capsule.radius * 0.5f;
				if (Physics.SphereCast(ray, this.m_Capsule.radius * 0.5f, maxDistance, -1, QueryTriggerInteraction.Ignore))
				{
					this.m_Crouching = true;
				}
			}
		}

		// Token: 0x06000264 RID: 612 RVA: 0x00011818 File Offset: 0x0000FC18
		private void UpdateAnimator(Vector3 move)
		{
			this.m_Animator.SetFloat("Forward", this.m_ForwardAmount, 0.1f, Time.deltaTime);
			this.m_Animator.SetFloat("Turn", this.m_TurnAmount, 0.1f, Time.deltaTime);
			this.m_Animator.SetBool("Crouch", this.m_Crouching);
			this.m_Animator.SetBool("OnGround", this.m_IsGrounded);
			if (!this.m_IsGrounded)
			{
				this.m_Animator.SetFloat("Jump", this.m_Rigidbody.velocity.y);
			}
			float num = Mathf.Repeat(this.m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + this.m_RunCycleLegOffset, 1f);
			float value = (float)((num >= 0.5f) ? -1 : 1) * this.m_ForwardAmount;
			if (this.m_IsGrounded)
			{
				this.m_Animator.SetFloat("JumpLeg", value);
			}
			if (this.m_IsGrounded && move.magnitude > 0f)
			{
				this.m_Animator.speed = this.m_AnimSpeedMultiplier;
			}
			else
			{
				this.m_Animator.speed = 1f;
			}
		}

		// Token: 0x06000265 RID: 613 RVA: 0x00011960 File Offset: 0x0000FD60
		private void HandleAirborneMovement()
		{
			Vector3 force = Physics.gravity * this.m_GravityMultiplier - Physics.gravity;
			this.m_Rigidbody.AddForce(force);
			this.m_GroundCheckDistance = ((this.m_Rigidbody.velocity.y >= 0f) ? 0.01f : this.m_OrigGroundCheckDistance);
		}

		// Token: 0x06000266 RID: 614 RVA: 0x000119C8 File Offset: 0x0000FDC8
		private void HandleGroundedMovement(bool crouch, bool jump)
		{
			if (jump && !crouch && this.m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
			{
				this.m_Rigidbody.velocity = new Vector3(this.m_Rigidbody.velocity.x, this.m_JumpPower, this.m_Rigidbody.velocity.z);
				this.m_IsGrounded = false;
				this.m_Animator.applyRootMotion = false;
				this.m_GroundCheckDistance = 0.1f;
			}
		}

		// Token: 0x06000267 RID: 615 RVA: 0x00011A5C File Offset: 0x0000FE5C
		private void ApplyExtraTurnRotation()
		{
			float num = Mathf.Lerp(this.m_StationaryTurnSpeed, this.m_MovingTurnSpeed, this.m_ForwardAmount);
			base.transform.Rotate(0f, this.m_TurnAmount * num * Time.deltaTime, 0f);
		}

		// Token: 0x06000268 RID: 616 RVA: 0x00011AA4 File Offset: 0x0000FEA4
		public void OnAnimatorMove()
		{
			if (this.m_IsGrounded && Time.deltaTime > 0f)
			{
				Vector3 velocity = this.m_Animator.deltaPosition * this.m_MoveSpeedMultiplier / Time.deltaTime;
				if (this.m_Rigidbody != null && !this.m_Rigidbody.isKinematic)
				{
					velocity.y = this.m_Rigidbody.velocity.y;
					this.m_Rigidbody.velocity = velocity;
				}
			}
		}

		// Token: 0x06000269 RID: 617 RVA: 0x00011B34 File Offset: 0x0000FF34
		private void CheckGroundStatus()
		{
			RaycastHit raycastHit;
			if (Physics.Raycast(base.transform.position + Vector3.up * 0.1f, Vector3.down, out raycastHit, this.m_GroundCheckDistance))
			{
				this.m_GroundNormal = raycastHit.normal;
				this.m_IsGrounded = true;
				this.m_Animator.applyRootMotion = true;
			}
			else
			{
				this.m_IsGrounded = false;
				this.m_GroundNormal = Vector3.up;
				this.m_Animator.applyRootMotion = false;
			}
		}

		// Token: 0x04000339 RID: 825
		[SerializeField]
		private float m_MovingTurnSpeed = 360f;

		// Token: 0x0400033A RID: 826
		[SerializeField]
		private float m_StationaryTurnSpeed = 180f;

		// Token: 0x0400033B RID: 827
		[SerializeField]
		private float m_JumpPower = 12f;

		// Token: 0x0400033C RID: 828
		[Range(1f, 4f)]
		[SerializeField]
		private float m_GravityMultiplier = 2f;

		// Token: 0x0400033D RID: 829
		[SerializeField]
		private float m_RunCycleLegOffset = 0.2f;

		// Token: 0x0400033E RID: 830
		[SerializeField]
		private float m_MoveSpeedMultiplier = 1f;

		// Token: 0x0400033F RID: 831
		[SerializeField]
		private float m_AnimSpeedMultiplier = 1f;

		// Token: 0x04000340 RID: 832
		[SerializeField]
		private float m_GroundCheckDistance = 0.1f;

		// Token: 0x04000341 RID: 833
		private Rigidbody m_Rigidbody;

		// Token: 0x04000342 RID: 834
		private Animator m_Animator;

		// Token: 0x04000343 RID: 835
		private bool m_IsGrounded;

		// Token: 0x04000344 RID: 836
		private float m_OrigGroundCheckDistance;

		// Token: 0x04000345 RID: 837
		private const float k_Half = 0.5f;

		// Token: 0x04000346 RID: 838
		private float m_TurnAmount;

		// Token: 0x04000347 RID: 839
		private float m_ForwardAmount;

		// Token: 0x04000348 RID: 840
		private Vector3 m_GroundNormal;

		// Token: 0x04000349 RID: 841
		private float m_CapsuleHeight;

		// Token: 0x0400034A RID: 842
		private Vector3 m_CapsuleCenter;

		// Token: 0x0400034B RID: 843
		private CapsuleCollider m_Capsule;

		// Token: 0x0400034C RID: 844
		private bool m_Crouching;

		// Token: 0x0400034D RID: 845
		public bool Enabled;
	}
}
