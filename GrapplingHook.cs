using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Obi;
using UnityEngine;

// Token: 0x02000386 RID: 902
public class GrapplingHook : MonoBehaviour
{
	// Token: 0x0600167F RID: 5759 RVA: 0x0007E4E3 File Offset: 0x0007C8E3
	public GrapplingHook()
	{
	}

	// Token: 0x06001680 RID: 5760 RVA: 0x0007E4F8 File Offset: 0x0007C8F8
	private void Awake()
	{
		this.rope = base.gameObject.AddComponent<ObiRope>();
		this.curve = base.gameObject.AddComponent<ObiCatmullRomCurve>();
		this.solver = base.gameObject.AddComponent<ObiSolver>();
		this.rope.Solver = this.solver;
		this.rope.ropePath = this.curve;
		this.rope.GetComponent<MeshRenderer>().material = this.material;
		this.rope.resolution = 0.1f;
		this.rope.BendingConstraints.stiffness = 0.2f;
		this.rope.UVScale = new Vector2(1f, 5f);
		this.rope.NormalizeV = false;
		this.rope.UVAnchor = 1f;
		this.solver.distanceConstraintParameters.iterations = 15;
		this.solver.pinConstraintParameters.iterations = 15;
		this.solver.bendingConstraintParameters.iterations = 1;
		this.cursor = this.rope.gameObject.AddComponent<ObiRopeCursor>();
		this.cursor.rope = this.rope;
		this.cursor.normalizedCoord = 0f;
		this.cursor.direction = true;
	}

	// Token: 0x06001681 RID: 5761 RVA: 0x0007E644 File Offset: 0x0007CA44
	private void LaunchHook()
	{
		Vector3 mousePosition = Input.mousePosition;
		mousePosition.z = base.transform.position.z - Camera.main.transform.position.z;
		Vector3 a = Camera.main.ScreenToWorldPoint(mousePosition);
		Ray ray = new Ray(base.transform.position, a - base.transform.position);
		if (Physics.Raycast(ray, out this.hookAttachment))
		{
			base.StartCoroutine(this.AttachHook());
		}
	}

	// Token: 0x06001682 RID: 5762 RVA: 0x0007E6D8 File Offset: 0x0007CAD8
	private IEnumerator AttachHook()
	{
		Vector3 localHit = this.curve.transform.InverseTransformPoint(this.hookAttachment.point);
		this.curve.controlPoints.Clear();
		this.curve.controlPoints.Add(Vector3.zero);
		this.curve.controlPoints.Add(Vector3.zero);
		this.curve.controlPoints.Add(localHit);
		this.curve.controlPoints.Add(localHit);
		yield return this.rope.GeneratePhysicRepresentationForMesh();
		this.rope.AddToSolver(null);
		this.rope.GetComponent<MeshRenderer>().enabled = true;
		this.attached = true;
		yield break;
	}

	// Token: 0x06001683 RID: 5763 RVA: 0x0007E6F3 File Offset: 0x0007CAF3
	private void DetachHook()
	{
		this.rope.RemoveFromSolver(null);
		this.rope.GetComponent<MeshRenderer>().enabled = false;
		this.attached = false;
	}

	// Token: 0x06001684 RID: 5764 RVA: 0x0007E71C File Offset: 0x0007CB1C
	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (!this.attached)
			{
				this.LaunchHook();
			}
			else
			{
				this.DetachHook();
			}
		}
		if (Input.GetKey(KeyCode.W))
		{
			this.cursor.ChangeLength(this.rope.RestLength - this.hookExtendRetractSpeed * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.S))
		{
			this.cursor.ChangeLength(this.rope.RestLength + this.hookExtendRetractSpeed * Time.deltaTime);
		}
	}

	// Token: 0x040012A2 RID: 4770
	public Collider character;

	// Token: 0x040012A3 RID: 4771
	public float hookExtendRetractSpeed = 2f;

	// Token: 0x040012A4 RID: 4772
	public Material material;

	// Token: 0x040012A5 RID: 4773
	private ObiRope rope;

	// Token: 0x040012A6 RID: 4774
	private ObiCatmullRomCurve curve;

	// Token: 0x040012A7 RID: 4775
	private ObiSolver solver;

	// Token: 0x040012A8 RID: 4776
	private ObiRopeCursor cursor;

	// Token: 0x040012A9 RID: 4777
	private RaycastHit hookAttachment;

	// Token: 0x040012AA RID: 4778
	private bool attached;

	// Token: 0x02000F40 RID: 3904
	[CompilerGenerated]
	private sealed class <AttachHook>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x0600733E RID: 29502 RVA: 0x0007E7AE File Offset: 0x0007CBAE
		[DebuggerHidden]
		public <AttachHook>c__Iterator0()
		{
		}

		// Token: 0x0600733F RID: 29503 RVA: 0x0007E7B8 File Offset: 0x0007CBB8
		public bool MoveNext()
		{
			uint num = (uint)this.$PC;
			this.$PC = -1;
			switch (num)
			{
			case 0U:
				localHit = this.curve.transform.InverseTransformPoint(this.hookAttachment.point);
				this.curve.controlPoints.Clear();
				this.curve.controlPoints.Add(Vector3.zero);
				this.curve.controlPoints.Add(Vector3.zero);
				this.curve.controlPoints.Add(localHit);
				this.curve.controlPoints.Add(localHit);
				this.$current = this.rope.GeneratePhysicRepresentationForMesh();
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
			case 1U:
				this.rope.AddToSolver(null);
				this.rope.GetComponent<MeshRenderer>().enabled = true;
				this.attached = true;
				this.$PC = -1;
				break;
			}
			return false;
		}

		// Token: 0x170010DB RID: 4315
		// (get) Token: 0x06007340 RID: 29504 RVA: 0x0007E8F8 File Offset: 0x0007CCF8
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x170010DC RID: 4316
		// (get) Token: 0x06007341 RID: 29505 RVA: 0x0007E900 File Offset: 0x0007CD00
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x06007342 RID: 29506 RVA: 0x0007E908 File Offset: 0x0007CD08
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x06007343 RID: 29507 RVA: 0x0007E918 File Offset: 0x0007CD18
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x04006729 RID: 26409
		internal Vector3 <localHit>__0;

		// Token: 0x0400672A RID: 26410
		internal GrapplingHook $this;

		// Token: 0x0400672B RID: 26411
		internal object $current;

		// Token: 0x0400672C RID: 26412
		internal bool $disposing;

		// Token: 0x0400672D RID: 26413
		internal int $PC;
	}
}
