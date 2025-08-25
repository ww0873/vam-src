using System;
using UnityEngine;

// Token: 0x02000095 RID: 149
public class ExampleWheelController : MonoBehaviour
{
	// Token: 0x06000223 RID: 547 RVA: 0x0001034D File Offset: 0x0000E74D
	public ExampleWheelController()
	{
	}

	// Token: 0x06000224 RID: 548 RVA: 0x00010355 File Offset: 0x0000E755
	private void Start()
	{
		this.m_Rigidbody = base.GetComponent<Rigidbody>();
		this.m_Rigidbody.maxAngularVelocity = 100f;
	}

	// Token: 0x06000225 RID: 549 RVA: 0x00010374 File Offset: 0x0000E774
	private void Update()
	{
		if (Input.GetKey(KeyCode.UpArrow))
		{
			this.m_Rigidbody.AddRelativeTorque(new Vector3(-1f * this.acceleration, 0f, 0f), ForceMode.Acceleration);
		}
		else if (Input.GetKey(KeyCode.DownArrow))
		{
			this.m_Rigidbody.AddRelativeTorque(new Vector3(1f * this.acceleration, 0f, 0f), ForceMode.Acceleration);
		}
		float value = -this.m_Rigidbody.angularVelocity.x / 100f;
		if (this.motionVectorRenderer)
		{
			this.motionVectorRenderer.material.SetFloat(ExampleWheelController.Uniforms._MotionAmount, Mathf.Clamp(value, -0.25f, 0.25f));
		}
	}

	// Token: 0x04000314 RID: 788
	public float acceleration;

	// Token: 0x04000315 RID: 789
	public Renderer motionVectorRenderer;

	// Token: 0x04000316 RID: 790
	private Rigidbody m_Rigidbody;

	// Token: 0x02000096 RID: 150
	private static class Uniforms
	{
		// Token: 0x06000226 RID: 550 RVA: 0x00010442 File Offset: 0x0000E842
		// Note: this type is marked as 'beforefieldinit'.
		static Uniforms()
		{
		}

		// Token: 0x04000317 RID: 791
		internal static readonly int _MotionAmount = Shader.PropertyToID("_MotionAmount");
	}
}
