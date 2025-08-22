using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Leap.Unity.Attributes;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x020006D6 RID: 1750
	public class ProximityDetector : Detector
	{
		// Token: 0x06002A20 RID: 10784 RVA: 0x000E383C File Offset: 0x000E1C3C
		public ProximityDetector()
		{
		}

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x06002A21 RID: 10785 RVA: 0x000E3877 File Offset: 0x000E1C77
		public GameObject CurrentObject
		{
			get
			{
				return this._currentObj;
			}
		}

		// Token: 0x06002A22 RID: 10786 RVA: 0x000E387F File Offset: 0x000E1C7F
		protected virtual void OnValidate()
		{
			if (this.OffDistance < this.OnDistance)
			{
				this.OffDistance = this.OnDistance;
			}
		}

		// Token: 0x06002A23 RID: 10787 RVA: 0x000E38A0 File Offset: 0x000E1CA0
		private void Awake()
		{
			this.proximityWatcherCoroutine = this.proximityWatcher();
			if (this.TagName != string.Empty)
			{
				GameObject[] array = GameObject.FindGameObjectsWithTag(this.TagName);
				List<GameObject> list = new List<GameObject>(array.Length + this.TargetObjects.Length);
				for (int i = 0; i < this.TargetObjects.Length; i++)
				{
					list.Add(this.TargetObjects[i]);
				}
				for (int j = 0; j < array.Length; j++)
				{
					list.Add(array[j]);
				}
				this.TargetObjects = list.ToArray();
			}
		}

		// Token: 0x06002A24 RID: 10788 RVA: 0x000E393C File Offset: 0x000E1D3C
		private void OnEnable()
		{
			base.StopCoroutine(this.proximityWatcherCoroutine);
			base.StartCoroutine(this.proximityWatcherCoroutine);
		}

		// Token: 0x06002A25 RID: 10789 RVA: 0x000E3957 File Offset: 0x000E1D57
		private void OnDisable()
		{
			base.StopCoroutine(this.proximityWatcherCoroutine);
			this.Deactivate();
		}

		// Token: 0x06002A26 RID: 10790 RVA: 0x000E396C File Offset: 0x000E1D6C
		private IEnumerator proximityWatcher()
		{
			bool proximityState = false;
			for (;;)
			{
				float onSquared = this.OnDistance * this.OnDistance;
				float offSquared = this.OffDistance * this.OffDistance;
				if (this._currentObj != null)
				{
					if (this.distanceSquared(this._currentObj) > offSquared)
					{
						this._currentObj = null;
						proximityState = false;
					}
				}
				else if (this.UseLayersNotList)
				{
					Collider[] array = Physics.OverlapSphere(base.transform.position, this.OnDistance, this.Layer);
					if (array.Length > 0)
					{
						this._currentObj = array[0].gameObject;
						proximityState = true;
						this.OnProximity.Invoke(this._currentObj);
					}
				}
				else
				{
					for (int i = 0; i < this.TargetObjects.Length; i++)
					{
						GameObject gameObject = this.TargetObjects[i];
						if (this.distanceSquared(gameObject) < onSquared)
						{
							this._currentObj = gameObject;
							proximityState = true;
							this.OnProximity.Invoke(this._currentObj);
							break;
						}
					}
				}
				if (proximityState)
				{
					this.Activate();
				}
				else
				{
					this.Deactivate();
				}
				yield return new WaitForSeconds(this.Period);
			}
			yield break;
		}

		// Token: 0x06002A27 RID: 10791 RVA: 0x000E3988 File Offset: 0x000E1D88
		private float distanceSquared(GameObject target)
		{
			Collider component = target.GetComponent<Collider>();
			Vector3 a;
			if (component != null)
			{
				a = component.ClosestPointOnBounds(base.transform.position);
			}
			else
			{
				a = target.transform.position;
			}
			return (a - base.transform.position).sqrMagnitude;
		}

		// Token: 0x0400225A RID: 8794
		[Tooltip("Dispatched when close enough to a target.")]
		public ProximityEvent OnProximity;

		// Token: 0x0400225B RID: 8795
		[Units("seconds")]
		[MinValue(0f)]
		[Tooltip("The interval in seconds at which to check this detector's conditions.")]
		public float Period = 0.1f;

		// Token: 0x0400225C RID: 8796
		[Header("Detector Targets")]
		[Tooltip("The list of target objects.")]
		[DisableIf("UseLayersNotList", true, null)]
		public GameObject[] TargetObjects;

		// Token: 0x0400225D RID: 8797
		[Tooltip("Objects with this tag are added to the list of targets.")]
		[DisableIf("UseLayersNotList", true, null)]
		public string TagName = string.Empty;

		// Token: 0x0400225E RID: 8798
		[Tooltip("Use a Layer instead of the target list.")]
		public bool UseLayersNotList;

		// Token: 0x0400225F RID: 8799
		[Tooltip("The Layer containing the objects to check.")]
		[DisableIf("UseLayersNotList", false, null)]
		public LayerMask Layer;

		// Token: 0x04002260 RID: 8800
		[Header("Distance Settings")]
		[Tooltip("The target distance in meters to activate the detector.")]
		[MinValue(0f)]
		public float OnDistance = 0.01f;

		// Token: 0x04002261 RID: 8801
		[Tooltip("The distance in meters at which to deactivate the detector.")]
		public float OffDistance = 0.015f;

		// Token: 0x04002262 RID: 8802
		[Header("")]
		[Tooltip("Draw this detector's Gizmos, if any. (Gizmos must be on in Unity edtor, too.)")]
		public bool ShowGizmos = true;

		// Token: 0x04002263 RID: 8803
		private IEnumerator proximityWatcherCoroutine;

		// Token: 0x04002264 RID: 8804
		private GameObject _currentObj;

		// Token: 0x02000F9E RID: 3998
		[CompilerGenerated]
		private sealed class <proximityWatcher>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x06007483 RID: 29827 RVA: 0x000E39E4 File Offset: 0x000E1DE4
			[DebuggerHidden]
			public <proximityWatcher>c__Iterator0()
			{
			}

			// Token: 0x06007484 RID: 29828 RVA: 0x000E39EC File Offset: 0x000E1DEC
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					proximityState = false;
					break;
				case 1U:
					break;
				default:
					return false;
				}
				onSquared = this.OnDistance * this.OnDistance;
				offSquared = this.OffDistance * this.OffDistance;
				if (this._currentObj != null)
				{
					if (base.distanceSquared(this._currentObj) > offSquared)
					{
						this._currentObj = null;
						proximityState = false;
					}
				}
				else if (this.UseLayersNotList)
				{
					Collider[] array = Physics.OverlapSphere(base.transform.position, this.OnDistance, this.Layer);
					if (array.Length > 0)
					{
						this._currentObj = array[0].gameObject;
						proximityState = true;
						this.OnProximity.Invoke(this._currentObj);
					}
				}
				else
				{
					for (int i = 0; i < this.TargetObjects.Length; i++)
					{
						GameObject gameObject = this.TargetObjects[i];
						if (base.distanceSquared(gameObject) < onSquared)
						{
							this._currentObj = gameObject;
							proximityState = true;
							this.OnProximity.Invoke(this._currentObj);
							break;
						}
					}
				}
				if (proximityState)
				{
					this.Activate();
				}
				else
				{
					this.Deactivate();
				}
				this.$current = new WaitForSeconds(this.Period);
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
			}

			// Token: 0x17001127 RID: 4391
			// (get) Token: 0x06007485 RID: 29829 RVA: 0x000E3C03 File Offset: 0x000E2003
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x17001128 RID: 4392
			// (get) Token: 0x06007486 RID: 29830 RVA: 0x000E3C0B File Offset: 0x000E200B
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x06007487 RID: 29831 RVA: 0x000E3C13 File Offset: 0x000E2013
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x06007488 RID: 29832 RVA: 0x000E3C23 File Offset: 0x000E2023
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x0400689B RID: 26779
			internal bool <proximityState>__0;

			// Token: 0x0400689C RID: 26780
			internal float <onSquared>__1;

			// Token: 0x0400689D RID: 26781
			internal float <offSquared>__1;

			// Token: 0x0400689E RID: 26782
			internal ProximityDetector $this;

			// Token: 0x0400689F RID: 26783
			internal object $current;

			// Token: 0x040068A0 RID: 26784
			internal bool $disposing;

			// Token: 0x040068A1 RID: 26785
			internal int $PC;
		}
	}
}
