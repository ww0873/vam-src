using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace MeshVR
{
	// Token: 0x02000AD1 RID: 2769
	public class DAZDynamicReloader : JSONStorable
	{
		// Token: 0x06004989 RID: 18825 RVA: 0x0017A2A1 File Offset: 0x001786A1
		public DAZDynamicReloader()
		{
		}

		// Token: 0x0600498A RID: 18826 RVA: 0x0017A2AC File Offset: 0x001786AC
		private IEnumerator LoadDelay()
		{
			yield return null;
			this.dd.Load(false);
			if (this.dazImportForReload != null && this.dazImportForReload.materialUIConnectorMaster != null)
			{
				this.dazImportForReload.materialUIConnectorMaster.Rebuild();
			}
			JSONStorableDynamic jsd = base.GetComponentInParent<JSONStorableDynamic>();
			if (jsd != null)
			{
				jsd.enabled = true;
			}
			yield break;
		}

		// Token: 0x0600498B RID: 18827 RVA: 0x0017A2C8 File Offset: 0x001786C8
		protected void Reload()
		{
			if (this.dd != null)
			{
				JSONStorableDynamic componentInParent = base.GetComponentInParent<JSONStorableDynamic>();
				if (componentInParent != null)
				{
					componentInParent.enabled = false;
				}
				this.dd.Clear();
				base.StartCoroutine(this.LoadDelay());
			}
		}

		// Token: 0x0600498C RID: 18828 RVA: 0x0017A318 File Offset: 0x00178718
		protected override void InitUI(Transform t, bool isAlt)
		{
			base.InitUI(t, isAlt);
			if (t != null)
			{
				DAZDynamicReloaderUI componentInChildren = t.GetComponentInChildren<DAZDynamicReloaderUI>(true);
				if (this.dd != null && componentInChildren != null)
				{
					this.reloadAction.RegisterButton(componentInChildren.reloadButton, isAlt);
				}
			}
		}

		// Token: 0x0600498D RID: 18829 RVA: 0x0017A370 File Offset: 0x00178770
		protected virtual void Init()
		{
			if (this.dazImportForReload == null)
			{
				this.dazImportForReload = base.GetComponent<DAZImport>();
			}
			this.dd = base.GetComponent<DAZDynamic>();
			if (this.dd != null)
			{
				this.reloadAction = new JSONStorableAction("Reload", new JSONStorableAction.ActionCallback(this.Reload));
				base.RegisterAction(this.reloadAction);
			}
		}

		// Token: 0x0600498E RID: 18830 RVA: 0x0017A3DF File Offset: 0x001787DF
		protected override void Awake()
		{
			if (!this.awakecalled)
			{
				base.Awake();
				this.Init();
				this.InitUI();
				this.InitUIAlt();
			}
		}

		// Token: 0x04003814 RID: 14356
		protected JSONStorableAction reloadAction;

		// Token: 0x04003815 RID: 14357
		public DAZImport dazImportForReload;

		// Token: 0x04003816 RID: 14358
		protected DAZDynamic dd;

		// Token: 0x02000FCA RID: 4042
		[CompilerGenerated]
		private sealed class <LoadDelay>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x06007531 RID: 30001 RVA: 0x0017A404 File Offset: 0x00178804
			[DebuggerHidden]
			public <LoadDelay>c__Iterator0()
			{
			}

			// Token: 0x06007532 RID: 30002 RVA: 0x0017A40C File Offset: 0x0017880C
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					this.$current = null;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1U:
					this.dd.Load(false);
					if (this.dazImportForReload != null && this.dazImportForReload.materialUIConnectorMaster != null)
					{
						this.dazImportForReload.materialUIConnectorMaster.Rebuild();
					}
					jsd = base.GetComponentInParent<JSONStorableDynamic>();
					if (jsd != null)
					{
						jsd.enabled = true;
					}
					this.$PC = -1;
					break;
				}
				return false;
			}

			// Token: 0x17001151 RID: 4433
			// (get) Token: 0x06007533 RID: 30003 RVA: 0x0017A4E5 File Offset: 0x001788E5
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x17001152 RID: 4434
			// (get) Token: 0x06007534 RID: 30004 RVA: 0x0017A4ED File Offset: 0x001788ED
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x06007535 RID: 30005 RVA: 0x0017A4F5 File Offset: 0x001788F5
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x06007536 RID: 30006 RVA: 0x0017A505 File Offset: 0x00178905
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x0400694B RID: 26955
			internal JSONStorableDynamic <jsd>__0;

			// Token: 0x0400694C RID: 26956
			internal DAZDynamicReloader $this;

			// Token: 0x0400694D RID: 26957
			internal object $current;

			// Token: 0x0400694E RID: 26958
			internal bool $disposing;

			// Token: 0x0400694F RID: 26959
			internal int $PC;
		}
	}
}
