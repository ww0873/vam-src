using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;

namespace MeshVR
{
	// Token: 0x02000E29 RID: 3625
	public class SteamVRActivator : MonoBehaviour
	{
		// Token: 0x06006F90 RID: 28560 RVA: 0x002A0AF7 File Offset: 0x0029EEF7
		public SteamVRActivator()
		{
		}

		// Token: 0x1700104B RID: 4171
		// (get) Token: 0x06006F91 RID: 28561 RVA: 0x002A0AFF File Offset: 0x0029EEFF
		public bool isActive
		{
			get
			{
				if (this.trackedObject != null)
				{
					return this.trackedObject.isActive;
				}
				return base.gameObject.activeInHierarchy;
			}
		}

		// Token: 0x1700104C RID: 4172
		// (get) Token: 0x06006F92 RID: 28562 RVA: 0x002A0B29 File Offset: 0x0029EF29
		public bool isPoseValid
		{
			get
			{
				return this.trackedObject != null && this.trackedObject.isValid;
			}
		}

		// Token: 0x06006F93 RID: 28563 RVA: 0x002A0B4C File Offset: 0x0029EF4C
		protected void Activate()
		{
			foreach (GameObject gameObject in this.objectsToActivateOnTracking)
			{
				if (gameObject != null)
				{
					gameObject.SetActive(true);
				}
			}
		}

		// Token: 0x06006F94 RID: 28564 RVA: 0x002A0B8C File Offset: 0x0029EF8C
		protected void Deactivate()
		{
			foreach (GameObject gameObject in this.objectsToActivateOnTracking)
			{
				if (gameObject != null)
				{
					gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x06006F95 RID: 28565 RVA: 0x002A0BCC File Offset: 0x0029EFCC
		protected void OnConnectedChanged(SteamVR_Behaviour_Pose pose, SteamVR_Input_Sources sources, bool connected)
		{
			if (connected)
			{
				UnityEngine.Debug.Log("Device " + sources + " connected");
				this.Activate();
			}
			else
			{
				UnityEngine.Debug.Log("Device " + sources + " disconnected");
				this.Deactivate();
			}
		}

		// Token: 0x06006F96 RID: 28566 RVA: 0x002A0C24 File Offset: 0x0029F024
		protected virtual void Awake()
		{
			if (this.trackedObject == null)
			{
				this.trackedObject = base.gameObject.GetComponent<SteamVR_Behaviour_Pose>();
			}
			if (this.trackedObject != null)
			{
				this.trackedObject.onConnectedChanged.AddListener(new UnityAction<SteamVR_Behaviour_Pose, SteamVR_Input_Sources, bool>(this.OnConnectedChanged));
			}
		}

		// Token: 0x06006F97 RID: 28567 RVA: 0x002A0C80 File Offset: 0x0029F080
		protected virtual IEnumerator Start()
		{
			while (!this.isPoseValid)
			{
				yield return null;
			}
			this.Activate();
			yield break;
		}

		// Token: 0x04006153 RID: 24915
		public SteamVR_Behaviour_Pose trackedObject;

		// Token: 0x04006154 RID: 24916
		public GameObject[] objectsToActivateOnTracking;

		// Token: 0x02001041 RID: 4161
		[CompilerGenerated]
		private sealed class <Start>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x060077A0 RID: 30624 RVA: 0x002A0C9B File Offset: 0x0029F09B
			[DebuggerHidden]
			public <Start>c__Iterator0()
			{
			}

			// Token: 0x060077A1 RID: 30625 RVA: 0x002A0CA4 File Offset: 0x0029F0A4
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					break;
				case 1U:
					break;
				default:
					return false;
				}
				if (!base.isPoseValid)
				{
					this.$current = null;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				base.Activate();
				this.$PC = -1;
				return false;
			}

			// Token: 0x170011CD RID: 4557
			// (get) Token: 0x060077A2 RID: 30626 RVA: 0x002A0D1C File Offset: 0x0029F11C
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170011CE RID: 4558
			// (get) Token: 0x060077A3 RID: 30627 RVA: 0x002A0D24 File Offset: 0x0029F124
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x060077A4 RID: 30628 RVA: 0x002A0D2C File Offset: 0x0029F12C
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x060077A5 RID: 30629 RVA: 0x002A0D3C File Offset: 0x0029F13C
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x04006B9F RID: 27551
			internal SteamVRActivator $this;

			// Token: 0x04006BA0 RID: 27552
			internal object $current;

			// Token: 0x04006BA1 RID: 27553
			internal bool $disposing;

			// Token: 0x04006BA2 RID: 27554
			internal int $PC;
		}
	}
}
