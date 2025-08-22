using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Obi;
using UnityEngine;

// Token: 0x02000387 RID: 903
public class ObiActorTeleport : MonoBehaviour
{
	// Token: 0x06001685 RID: 5765 RVA: 0x0007E91F File Offset: 0x0007CD1F
	public ObiActorTeleport()
	{
	}

	// Token: 0x06001686 RID: 5766 RVA: 0x0007E927 File Offset: 0x0007CD27
	private void Update()
	{
		if (Input.anyKeyDown)
		{
			base.StartCoroutine(this.Teleport());
		}
	}

	// Token: 0x06001687 RID: 5767 RVA: 0x0007E940 File Offset: 0x0007CD40
	private IEnumerator Teleport()
	{
		this.actor.enabled = false;
		this.actor.transform.position = UnityEngine.Random.insideUnitSphere * 2f;
		yield return new WaitForFixedUpdate();
		this.actor.enabled = true;
		yield break;
	}

	// Token: 0x040012AB RID: 4779
	public ObiActor actor;

	// Token: 0x02000F41 RID: 3905
	[CompilerGenerated]
	private sealed class <Teleport>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x06007344 RID: 29508 RVA: 0x0007E95B File Offset: 0x0007CD5B
		[DebuggerHidden]
		public <Teleport>c__Iterator0()
		{
		}

		// Token: 0x06007345 RID: 29509 RVA: 0x0007E964 File Offset: 0x0007CD64
		public bool MoveNext()
		{
			uint num = (uint)this.$PC;
			this.$PC = -1;
			switch (num)
			{
			case 0U:
				this.actor.enabled = false;
				this.actor.transform.position = UnityEngine.Random.insideUnitSphere * 2f;
				this.$current = new WaitForFixedUpdate();
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
			case 1U:
				this.actor.enabled = true;
				this.$PC = -1;
				break;
			}
			return false;
		}

		// Token: 0x170010DD RID: 4317
		// (get) Token: 0x06007346 RID: 29510 RVA: 0x0007EA01 File Offset: 0x0007CE01
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x170010DE RID: 4318
		// (get) Token: 0x06007347 RID: 29511 RVA: 0x0007EA09 File Offset: 0x0007CE09
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x06007348 RID: 29512 RVA: 0x0007EA11 File Offset: 0x0007CE11
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x06007349 RID: 29513 RVA: 0x0007EA21 File Offset: 0x0007CE21
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x0400672E RID: 26414
		internal ObiActorTeleport $this;

		// Token: 0x0400672F RID: 26415
		internal object $current;

		// Token: 0x04006730 RID: 26416
		internal bool $disposing;

		// Token: 0x04006731 RID: 26417
		internal int $PC;
	}
}
