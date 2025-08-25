using System;
using UnityEngine;

namespace Obi.Characters.ThirdPerson
{
	// Token: 0x0200038C RID: 908
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(CapsuleCollider))]
	[RequireComponent(typeof(Animator))]
	public class ThirdPersonCharacter : MonoBehaviour
	{
		// Token: 0x06001694 RID: 5780 RVA: 0x0007EE10 File Offset: 0x0007D210
		public ThirdPersonCharacter()
		{
		}

		// Token: 0x06001695 RID: 5781 RVA: 0x0007EE7C File Offset: 0x0007D27C
		private void Start()
		{
			Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), LayerMask.NameToLayer("Ignore Raycast"), true);
			this.m_Animator = base.GetComponent<Animator>();
			this.m_Rigidbody = base.GetComponent<Rigidbody>();
			this.m_Capsule = base.GetComponent<CapsuleCollider>();
			this.m_CapsuleHeight = this.m_Capsule.height;
			this.m_CapsuleCenter = this.m_Capsule.center;
			this.m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
			this.m_OrigGroundCheckDistance = this.m_GroundCheckDistance;
		}

		// Token: 0x06001696 RID: 5782 RVA: 0x0007EF04 File Offset: 0x0007D304
		public void Move(Vector3 move, bool crouch, bool jump)
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
			this.UpdateAnimator(move);
		}

		// Token: 0x06001697 RID: 5783 RVA: 0x0007EFAC File Offset: 0x0007D3AC
		private void ScaleCapsuleForCrouching(bool crouch)
		{
			if (this.m_IsGrounded && crouch)
			{
				if (this.m_Crouching)
				{
					return;
				}
				this.m_Capsule.height = this.m_Capsule.height / 2f;
				this.m_Capsule.center = this.m_Capsule.center / 2f;
				this.m_Crouching = true;
			}
			else
			{
				Ray ray = new Ray(this.m_Rigidbody.position + Vector3.up * this.m_Capsule.radius * 0.5f, Vector3.up);
				float maxDistance = this.m_CapsuleHeight - this.m_Capsule.radius * 0.5f;
				if (Physics.SphereCast(ray, this.m_Capsule.radius * 0.5f, maxDistance, -5, QueryTriggerInteraction.Ignore))
				{
					this.m_Crouching = true;
					return;
				}
				this.m_Capsule.height = this.m_CapsuleHeight;
				this.m_Capsule.center = this.m_CapsuleCenter;
				this.m_Crouching = false;
			}
		}

		// Token: 0x06001698 RID: 5784 RVA: 0x0007F0C4 File Offset: 0x0007D4C4
		private void PreventStandingInLowHeadroom()
		{
			if (!this.m_Crouching)
			{
				Ray ray = new Ray(this.m_Rigidbody.position + Vector3.up * this.m_Capsule.radius * 0.5f, Vector3.up);
				float maxDistance = this.m_CapsuleHeight - this.m_Capsule.radius * 0.5f;
				if (Physics.SphereCast(ray, this.m_Capsule.radius * 0.5f, maxDistance, -5, QueryTriggerInteraction.Ignore))
				{
					this.m_Crouching = true;
				}
			}
		}

		// Token: 0x06001699 RID: 5785 RVA: 0x0007F158 File Offset: 0x0007D558
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

		// Token: 0x0600169A RID: 5786 RVA: 0x0007F2A0 File Offset: 0x0007D6A0
		private void HandleAirborneMovement()
		{
			Vector3 force = Physics.gravity * this.m_GravityMultiplier - Physics.gravity;
			this.m_Rigidbody.AddForce(force);
			this.m_GroundCheckDistance = ((this.m_Rigidbody.velocity.y >= 0f) ? 0.01f : this.m_OrigGroundCheckDistance);
		}

		// Token: 0x0600169B RID: 5787 RVA: 0x0007F308 File Offset: 0x0007D708
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

		// Token: 0x0600169C RID: 5788 RVA: 0x0007F39C File Offset: 0x0007D79C
		private void ApplyExtraTurnRotation()
		{
			float num = Mathf.Lerp(this.m_StationaryTurnSpeed, this.m_MovingTurnSpeed, this.m_ForwardAmount);
			base.transform.Rotate(0f, this.m_TurnAmount * num * Time.deltaTime, 0f);
		}

		// Token: 0x0600169D RID: 5789 RVA: 0x0007F3E4 File Offset: 0x0007D7E4
		public void OnAnimatorMove()
		{
			if (this.m_IsGrounded && Time.deltaTime > 0f)
			{
				Vector3 velocity = this.m_Animator.deltaPosition * this.m_MoveSpeedMultiplier / Time.deltaTime;
				velocity.y = this.m_Rigidbody.velocity.y;
				this.m_Rigidbody.velocity = velocity;
			}
		}

		// Token: 0x0600169E RID: 5790 RVA: 0x0007F454 File Offset: 0x0007D854
		private void CheckGroundStatus()
		{
			RaycastHit raycastHit;
			if (Physics.Raycast(base.transform.position + Vector3.up * 0.1f, Vector3.down, out raycastHit, this.m_GroundCheckDistance, -5))
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

		// Token: 0x040012BB RID: 4795
		[SerializeField]
		private float m_MovingTurnSpeed = 360f;

		// Token: 0x040012BC RID: 4796
		[SerializeField]
		private float m_StationaryTurnSpeed = 180f;

		// Token: 0x040012BD RID: 4797
		[SerializeField]
		private float m_JumpPower = 12f;

		// Token: 0x040012BE RID: 4798
		[Range(1f, 4f)]
		[SerializeField]
		private float m_GravityMultiplier = 2f;

		// Token: 0x040012BF RID: 4799
		[SerializeField]
		private float m_RunCycleLegOffset = 0.2f;

		// Token: 0x040012C0 RID: 4800
		[SerializeField]
		private float m_MoveSpeedMultiplier = 1f;

		// Token: 0x040012C1 RID: 4801
		[SerializeField]
		private float m_AnimSpeedMultiplier = 1f;

		// Token: 0x040012C2 RID: 4802
		[SerializeField]
		private float m_GroundCheckDistance = 0.1f;

		// Token: 0x040012C3 RID: 4803
		private Rigidbody m_Rigidbody;

		// Token: 0x040012C4 RID: 4804
		private Animator m_Animator;

		// Token: 0x040012C5 RID: 4805
		private bool m_IsGrounded;

		// Token: 0x040012C6 RID: 4806
		private float m_OrigGroundCheckDistance;

		// Token: 0x040012C7 RID: 4807
		private const float k_Half = 0.5f;

		// Token: 0x040012C8 RID: 4808
		private float m_TurnAmount;

		// Token: 0x040012C9 RID: 4809
		private float m_ForwardAmount;

		// Token: 0x040012CA RID: 4810
		private Vector3 m_GroundNormal;

		// Token: 0x040012CB RID: 4811
		private float m_CapsuleHeight;

		// Token: 0x040012CC RID: 4812
		private Vector3 m_CapsuleCenter;

		// Token: 0x040012CD RID: 4813
		private CapsuleCollider m_Capsule;

		// Token: 0x040012CE RID: 4814
		private bool m_Crouching;
	}
}
