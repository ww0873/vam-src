using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GPUTools.Hair.Scripts.Runtime.Commands;
using GPUTools.Hair.Scripts.Runtime.Data;
using GPUTools.Hair.Scripts.Settings;
using UnityEngine;

namespace GPUTools.Hair.Scripts
{
	// Token: 0x02000A09 RID: 2569
	public class HairSettings : GPUCollidersConsumer
	{
		// Token: 0x06004108 RID: 16648 RVA: 0x00135595 File Offset: 0x00133995
		public HairSettings()
		{
		}

		// Token: 0x17000797 RID: 1943
		// (get) Token: 0x0600410A RID: 16650 RVA: 0x001355DD File Offset: 0x001339DD
		// (set) Token: 0x06004109 RID: 16649 RVA: 0x001355D4 File Offset: 0x001339D4
		public RuntimeData RuntimeData
		{
			[CompilerGenerated]
			get
			{
				return this.<RuntimeData>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<RuntimeData>k__BackingField = value;
			}
		}

		// Token: 0x17000798 RID: 1944
		// (get) Token: 0x0600410C RID: 16652 RVA: 0x001355EE File Offset: 0x001339EE
		// (set) Token: 0x0600410B RID: 16651 RVA: 0x001355E5 File Offset: 0x001339E5
		public BuildRuntimeHair HairBuidCommand
		{
			[CompilerGenerated]
			get
			{
				return this.<HairBuidCommand>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<HairBuidCommand>k__BackingField = value;
			}
		}

		// Token: 0x0600410D RID: 16653 RVA: 0x001355F6 File Offset: 0x001339F6
		private void Awake()
		{
			if (!this.ValidateImpl())
			{
				return;
			}
			this.RuntimeData = new RuntimeData();
			this.HairBuidCommand = new BuildRuntimeHair(this);
			this.HairBuidCommand.Build();
		}

		// Token: 0x0600410E RID: 16654 RVA: 0x00135626 File Offset: 0x00133A26
		public void ReStart()
		{
			if (!this.ValidateImpl())
			{
				return;
			}
			if (this.HairBuidCommand != null)
			{
				this.HairBuidCommand.Dispose();
			}
			this.Awake();
		}

		// Token: 0x0600410F RID: 16655 RVA: 0x00135650 File Offset: 0x00133A50
		private void FixedUpdate()
		{
			if (this.HairBuidCommand == null)
			{
				return;
			}
			this.SyncConsumer();
			this.HairBuidCommand.FixedDispatch();
		}

		// Token: 0x06004110 RID: 16656 RVA: 0x0013566F File Offset: 0x00133A6F
		private void LateUpdate()
		{
			if (this.HairBuidCommand == null)
			{
				return;
			}
			this.StandsSettings.Provider.Dispatch();
			this.HairBuidCommand.Dispatch();
		}

		// Token: 0x06004111 RID: 16657 RVA: 0x00135698 File Offset: 0x00133A98
		public void UpdateSettings()
		{
			if (this.HairBuidCommand == null)
			{
				return;
			}
			if (Application.isPlaying)
			{
				this.HairBuidCommand.UpdateSettings();
			}
		}

		// Token: 0x06004112 RID: 16658 RVA: 0x001356BB File Offset: 0x00133ABB
		public void OnDestroy()
		{
			if (this.HairBuidCommand != null)
			{
				this.HairBuidCommand.Dispose();
			}
		}

		// Token: 0x06004113 RID: 16659 RVA: 0x001356D4 File Offset: 0x00133AD4
		private bool ValidateImpl()
		{
			return this.StandsSettings.Validate() && this.PhysicsSettings.Validate() && this.RenderSettings.Validate() && this.LODSettings.Validate() && this.ShadowSettings.Validate();
		}

		// Token: 0x06004114 RID: 16660 RVA: 0x0013572F File Offset: 0x00133B2F
		private void OnDrawGizmos()
		{
			this.StandsSettings.DrawGizmos();
			this.PhysicsSettings.DrawGizmos();
			this.RenderSettings.DrawGizmos();
			this.LODSettings.DrawGizmos();
			this.ShadowSettings.DrawGizmos();
		}

		// Token: 0x06004115 RID: 16661 RVA: 0x00135768 File Offset: 0x00133B68
		protected void SyncConsumer()
		{
			if (this.PhysicsSettings.IsEnabled && !this.consumerRegistered)
			{
				GPUCollidersManager.RegisterConsumer(this);
				this.consumerRegistered = true;
			}
			else if (!this.PhysicsSettings.IsEnabled && this.consumerRegistered)
			{
				GPUCollidersManager.DeregisterConsumer(this);
				this.consumerRegistered = false;
			}
		}

		// Token: 0x06004116 RID: 16662 RVA: 0x001357CA File Offset: 0x00133BCA
		protected override void OnEnable()
		{
			if (Application.isPlaying)
			{
				this.SyncConsumer();
			}
		}

		// Token: 0x06004117 RID: 16663 RVA: 0x001357DC File Offset: 0x00133BDC
		protected override void OnDisable()
		{
			if (Application.isPlaying && this.consumerRegistered)
			{
				GPUCollidersManager.DeregisterConsumer(this);
				this.consumerRegistered = false;
			}
		}

		// Token: 0x040030E3 RID: 12515
		public HairStandsSettings StandsSettings = new HairStandsSettings();

		// Token: 0x040030E4 RID: 12516
		public HairPhysicsSettings PhysicsSettings = new HairPhysicsSettings();

		// Token: 0x040030E5 RID: 12517
		public HairRenderSettings RenderSettings = new HairRenderSettings();

		// Token: 0x040030E6 RID: 12518
		public HairLODSettings LODSettings = new HairLODSettings();

		// Token: 0x040030E7 RID: 12519
		public HairShadowSettings ShadowSettings = new HairShadowSettings();

		// Token: 0x040030E8 RID: 12520
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private RuntimeData <RuntimeData>k__BackingField;

		// Token: 0x040030E9 RID: 12521
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private BuildRuntimeHair <HairBuidCommand>k__BackingField;

		// Token: 0x040030EA RID: 12522
		protected bool consumerRegistered;
	}
}
