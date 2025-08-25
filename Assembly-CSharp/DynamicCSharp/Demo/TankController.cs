using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace DynamicCSharp.Demo
{
	// Token: 0x020002C9 RID: 713
	public abstract class TankController : MonoBehaviour
	{
		// Token: 0x06001072 RID: 4210 RVA: 0x0005C55F File Offset: 0x0005A95F
		protected TankController()
		{
		}

		// Token: 0x06001073 RID: 4211
		public abstract void TankMain();

		// Token: 0x06001074 RID: 4212 RVA: 0x0005C588 File Offset: 0x0005A988
		public void OnCollisionEnter2D(Collision2D collision)
		{
			Collider2D collider = collision.collider;
			if (collider.name == "DamagedWall" || collider.name == "Wall")
			{
				this.crash = true;
			}
		}

		// Token: 0x06001075 RID: 4213 RVA: 0x0005C5CD File Offset: 0x0005A9CD
		public void RunTank()
		{
			base.StartCoroutine(this.RunTankRoutine());
		}

		// Token: 0x06001076 RID: 4214 RVA: 0x0005C5DC File Offset: 0x0005A9DC
		public void MoveForward(float amount = 1f)
		{
			this.tankTasks.Enqueue(new TankEvent(TankEventType.Move, amount));
		}

		// Token: 0x06001077 RID: 4215 RVA: 0x0005C5F0 File Offset: 0x0005A9F0
		public void MoveBackward(float amount = 1f)
		{
			this.tankTasks.Enqueue(new TankEvent(TankEventType.Move, -amount));
		}

		// Token: 0x06001078 RID: 4216 RVA: 0x0005C605 File Offset: 0x0005AA05
		public void RotateClockwise(float amount = 90f)
		{
			this.tankTasks.Enqueue(new TankEvent(TankEventType.Rotate, amount));
		}

		// Token: 0x06001079 RID: 4217 RVA: 0x0005C619 File Offset: 0x0005AA19
		public void RotateCounterClockwise(float amount = 90f)
		{
			this.tankTasks.Enqueue(new TankEvent(TankEventType.Rotate, -amount));
		}

		// Token: 0x0600107A RID: 4218 RVA: 0x0005C62E File Offset: 0x0005AA2E
		public void Shoot()
		{
			this.tankTasks.Enqueue(new TankEvent(TankEventType.Shoot, 0f));
		}

		// Token: 0x0600107B RID: 4219 RVA: 0x0005C648 File Offset: 0x0005AA48
		private IEnumerator RunTankRoutine()
		{
			this.TankMain();
			while (this.tankTasks.Count > 0)
			{
				if (this.crash)
				{
					UnityEngine.Debug.Log("Crashed!");
					this.tankTasks.Clear();
					yield break;
				}
				TankEvent e = this.tankTasks.Dequeue();
				TankEventType eventType = e.eventType;
				if (eventType != TankEventType.Move)
				{
					if (eventType != TankEventType.Rotate)
					{
						if (eventType == TankEventType.Shoot)
						{
							yield return base.StartCoroutine(this.ShootRoutine());
						}
					}
					else
					{
						yield return base.StartCoroutine(this.RotateRoutine(e.eventValue));
					}
				}
				else
				{
					yield return base.StartCoroutine(this.MoveRoutine(e.eventValue));
				}
			}
			yield break;
		}

		// Token: 0x0600107C RID: 4220 RVA: 0x0005C664 File Offset: 0x0005AA64
		private IEnumerator MoveRoutine(float amount)
		{
			Vector2 destination = base.transform.position + base.transform.up * amount;
			while (Vector2.Distance(base.transform.position, destination) > 0.05f)
			{
				if (this.crash)
				{
					yield break;
				}
				base.transform.position = Vector2.MoveTowards(base.transform.position, destination, Time.deltaTime * this.moveSpeed);
				yield return null;
			}
			yield break;
		}

		// Token: 0x0600107D RID: 4221 RVA: 0x0005C688 File Offset: 0x0005AA88
		private IEnumerator RotateRoutine(float amount)
		{
			Quaternion target = Quaternion.Euler(0f, 0f, base.transform.eulerAngles.z - amount);
			while (base.transform.rotation != target)
			{
				if (this.crash)
				{
					yield break;
				}
				float delta = Mathf.Abs(base.transform.eulerAngles.z - target.eulerAngles.z);
				if (delta < 0.2f)
				{
					yield break;
				}
				base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, target, Time.deltaTime * (this.rotateSpeed * 60f));
				yield return null;
			}
			yield break;
		}

		// Token: 0x0600107E RID: 4222 RVA: 0x0005C6AC File Offset: 0x0005AAAC
		private IEnumerator ShootRoutine()
		{
			Vector2 startPos = base.transform.position + base.transform.up * 0.8f;
			TankShell shell = TankShell.Shoot(this.bulletObject, startPos, base.transform.up);
			while (!shell.Step())
			{
				yield return null;
			}
			shell.Destroy();
			yield break;
		}

		// Token: 0x04000EA3 RID: 3747
		private Queue<TankEvent> tankTasks = new Queue<TankEvent>();

		// Token: 0x04000EA4 RID: 3748
		private bool crash;

		// Token: 0x04000EA5 RID: 3749
		[HideInInspector]
		public GameObject bulletObject;

		// Token: 0x04000EA6 RID: 3750
		[HideInInspector]
		public float moveSpeed = 1f;

		// Token: 0x04000EA7 RID: 3751
		[HideInInspector]
		public float rotateSpeed = 3f;

		// Token: 0x02000EE4 RID: 3812
		[CompilerGenerated]
		private sealed class <RunTankRoutine>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x06007217 RID: 29207 RVA: 0x0005C6C7 File Offset: 0x0005AAC7
			[DebuggerHidden]
			public <RunTankRoutine>c__Iterator0()
			{
			}

			// Token: 0x06007218 RID: 29208 RVA: 0x0005C6D0 File Offset: 0x0005AAD0
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					this.TankMain();
					break;
				case 1U:
					break;
				case 2U:
					break;
				case 3U:
					break;
				default:
					return false;
				}
				while (this.tankTasks.Count > 0)
				{
					if (this.crash)
					{
						UnityEngine.Debug.Log("Crashed!");
						this.tankTasks.Clear();
						return false;
					}
					e = this.tankTasks.Dequeue();
					eventType = e.eventType;
					if (eventType != TankEventType.Move)
					{
						if (eventType != TankEventType.Rotate)
						{
							if (eventType != TankEventType.Shoot)
							{
								continue;
							}
							this.$current = base.StartCoroutine(base.ShootRoutine());
							if (!this.$disposing)
							{
								this.$PC = 3;
							}
						}
						else
						{
							this.$current = base.StartCoroutine(base.RotateRoutine(e.eventValue));
							if (!this.$disposing)
							{
								this.$PC = 2;
							}
						}
					}
					else
					{
						this.$current = base.StartCoroutine(base.MoveRoutine(e.eventValue));
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
					}
					return true;
				}
				this.$PC = -1;
				return false;
			}

			// Token: 0x170010AD RID: 4269
			// (get) Token: 0x06007219 RID: 29209 RVA: 0x0005C869 File Offset: 0x0005AC69
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170010AE RID: 4270
			// (get) Token: 0x0600721A RID: 29210 RVA: 0x0005C871 File Offset: 0x0005AC71
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x0600721B RID: 29211 RVA: 0x0005C879 File Offset: 0x0005AC79
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x0600721C RID: 29212 RVA: 0x0005C889 File Offset: 0x0005AC89
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x040065ED RID: 26093
			internal TankEvent <e>__1;

			// Token: 0x040065EE RID: 26094
			internal TankEventType $locvar0;

			// Token: 0x040065EF RID: 26095
			internal TankController $this;

			// Token: 0x040065F0 RID: 26096
			internal object $current;

			// Token: 0x040065F1 RID: 26097
			internal bool $disposing;

			// Token: 0x040065F2 RID: 26098
			internal int $PC;
		}

		// Token: 0x02000EE5 RID: 3813
		[CompilerGenerated]
		private sealed class <MoveRoutine>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x0600721D RID: 29213 RVA: 0x0005C890 File Offset: 0x0005AC90
			[DebuggerHidden]
			public <MoveRoutine>c__Iterator1()
			{
			}

			// Token: 0x0600721E RID: 29214 RVA: 0x0005C898 File Offset: 0x0005AC98
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					destination = base.transform.position + base.transform.up * amount;
					break;
				case 1U:
					break;
				default:
					return false;
				}
				if (Vector2.Distance(base.transform.position, destination) <= 0.05f)
				{
					this.$PC = -1;
				}
				else if (!this.crash)
				{
					base.transform.position = Vector2.MoveTowards(base.transform.position, destination, Time.deltaTime * this.moveSpeed);
					this.$current = null;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				return false;
			}

			// Token: 0x170010AF RID: 4271
			// (get) Token: 0x0600721F RID: 29215 RVA: 0x0005C9B0 File Offset: 0x0005ADB0
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170010B0 RID: 4272
			// (get) Token: 0x06007220 RID: 29216 RVA: 0x0005C9B8 File Offset: 0x0005ADB8
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x06007221 RID: 29217 RVA: 0x0005C9C0 File Offset: 0x0005ADC0
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x06007222 RID: 29218 RVA: 0x0005C9D0 File Offset: 0x0005ADD0
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x040065F3 RID: 26099
			internal float amount;

			// Token: 0x040065F4 RID: 26100
			internal Vector2 <destination>__0;

			// Token: 0x040065F5 RID: 26101
			internal TankController $this;

			// Token: 0x040065F6 RID: 26102
			internal object $current;

			// Token: 0x040065F7 RID: 26103
			internal bool $disposing;

			// Token: 0x040065F8 RID: 26104
			internal int $PC;
		}

		// Token: 0x02000EE6 RID: 3814
		[CompilerGenerated]
		private sealed class <RotateRoutine>c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x06007223 RID: 29219 RVA: 0x0005C9D7 File Offset: 0x0005ADD7
			[DebuggerHidden]
			public <RotateRoutine>c__Iterator2()
			{
			}

			// Token: 0x06007224 RID: 29220 RVA: 0x0005C9E0 File Offset: 0x0005ADE0
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					target = Quaternion.Euler(0f, 0f, base.transform.eulerAngles.z - amount);
					break;
				case 1U:
					break;
				default:
					return false;
				}
				if (!(base.transform.rotation != target))
				{
					this.$PC = -1;
				}
				else if (!this.crash)
				{
					delta = Mathf.Abs(base.transform.eulerAngles.z - target.eulerAngles.z);
					if (delta >= 0.2f)
					{
						base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, target, Time.deltaTime * (this.rotateSpeed * 60f));
						this.$current = null;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
				}
				return false;
			}

			// Token: 0x170010B1 RID: 4273
			// (get) Token: 0x06007225 RID: 29221 RVA: 0x0005CB2F File Offset: 0x0005AF2F
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170010B2 RID: 4274
			// (get) Token: 0x06007226 RID: 29222 RVA: 0x0005CB37 File Offset: 0x0005AF37
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x06007227 RID: 29223 RVA: 0x0005CB3F File Offset: 0x0005AF3F
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x06007228 RID: 29224 RVA: 0x0005CB4F File Offset: 0x0005AF4F
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x040065F9 RID: 26105
			internal float amount;

			// Token: 0x040065FA RID: 26106
			internal Quaternion <target>__0;

			// Token: 0x040065FB RID: 26107
			internal float <delta>__1;

			// Token: 0x040065FC RID: 26108
			internal TankController $this;

			// Token: 0x040065FD RID: 26109
			internal object $current;

			// Token: 0x040065FE RID: 26110
			internal bool $disposing;

			// Token: 0x040065FF RID: 26111
			internal int $PC;
		}

		// Token: 0x02000EE7 RID: 3815
		[CompilerGenerated]
		private sealed class <ShootRoutine>c__Iterator3 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x06007229 RID: 29225 RVA: 0x0005CB56 File Offset: 0x0005AF56
			[DebuggerHidden]
			public <ShootRoutine>c__Iterator3()
			{
			}

			// Token: 0x0600722A RID: 29226 RVA: 0x0005CB60 File Offset: 0x0005AF60
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					startPos = base.transform.position + base.transform.up * 0.8f;
					shell = TankShell.Shoot(this.bulletObject, startPos, base.transform.up);
					break;
				case 1U:
					break;
				default:
					return false;
				}
				if (!shell.Step())
				{
					this.$current = null;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				shell.Destroy();
				this.$PC = -1;
				return false;
			}

			// Token: 0x170010B3 RID: 4275
			// (get) Token: 0x0600722B RID: 29227 RVA: 0x0005CC3E File Offset: 0x0005B03E
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170010B4 RID: 4276
			// (get) Token: 0x0600722C RID: 29228 RVA: 0x0005CC46 File Offset: 0x0005B046
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x0600722D RID: 29229 RVA: 0x0005CC4E File Offset: 0x0005B04E
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x0600722E RID: 29230 RVA: 0x0005CC5E File Offset: 0x0005B05E
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x04006600 RID: 26112
			internal Vector2 <startPos>__0;

			// Token: 0x04006601 RID: 26113
			internal TankShell <shell>__0;

			// Token: 0x04006602 RID: 26114
			internal TankController $this;

			// Token: 0x04006603 RID: 26115
			internal object $current;

			// Token: 0x04006604 RID: 26116
			internal bool $disposing;

			// Token: 0x04006605 RID: 26117
			internal int $PC;
		}
	}
}
