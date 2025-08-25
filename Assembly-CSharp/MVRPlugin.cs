using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using MVR.FileManagement;
using MVR.FileManagementSecure;
using UnityEngine;

// Token: 0x02000C33 RID: 3123
public class MVRPlugin
{
	// Token: 0x06005AC4 RID: 23236 RVA: 0x00214B08 File Offset: 0x00212F08
	public MVRPlugin()
	{
		this.scriptControllers = new List<MVRScriptController>();
		this.requestedPackages = new HashSet<VarPackage>();
		this.confirmedOncePackages = new HashSet<VarPackage>();
		this.userConfirmedAPackage = false;
	}

	// Token: 0x06005AC5 RID: 23237 RVA: 0x00214B38 File Offset: 0x00212F38
	public void Reload()
	{
		if (this.pluginURLJSON != null)
		{
			this.pluginURLJSON.Reload();
		}
	}

	// Token: 0x17000D66 RID: 3430
	// (get) Token: 0x06005AC6 RID: 23238 RVA: 0x00214B50 File Offset: 0x00212F50
	// (set) Token: 0x06005AC7 RID: 23239 RVA: 0x00214B58 File Offset: 0x00212F58
	public bool HasRequestedPackages
	{
		[CompilerGenerated]
		get
		{
			return this.<HasRequestedPackages>k__BackingField;
		}
		[CompilerGenerated]
		protected set
		{
			this.<HasRequestedPackages>k__BackingField = value;
		}
	}

	// Token: 0x06005AC8 RID: 23240 RVA: 0x00214B61 File Offset: 0x00212F61
	public void AddRequestedPackage(VarPackage vp)
	{
		this.HasRequestedPackages = true;
		if (!vp.PluginsAlwaysDisabled)
		{
			this.requestedPackages.Add(vp);
		}
	}

	// Token: 0x06005AC9 RID: 23241 RVA: 0x00214B82 File Offset: 0x00212F82
	public bool IsVarPackageConfirmed(VarPackage vp)
	{
		return !vp.PluginsAlwaysDisabled && (vp.PluginsAlwaysEnabled || this.confirmedOncePackages.Contains(vp));
	}

	// Token: 0x06005ACA RID: 23242 RVA: 0x00214BAE File Offset: 0x00212FAE
	protected void CheckUserConfirmDenyComplete()
	{
		if (this.requestedPackages.Count == 0 && this.userConfirmDenyCompleteCallback != null)
		{
			this.userConfirmDenyCompleteCallback(this, this.userConfirmedAPackage);
		}
	}

	// Token: 0x06005ACB RID: 23243 RVA: 0x00214BDD File Offset: 0x00212FDD
	protected void UserConfirmVarPackage(VarPackage vp)
	{
		this.confirmedOncePackages.Add(vp);
		this.userConfirmedAPackage = true;
		this.requestedPackages.Remove(vp);
		this.CheckUserConfirmDenyComplete();
		FileManager.AutoConfirmAllWithSignature(this.GetPrompt(vp));
	}

	// Token: 0x06005ACC RID: 23244 RVA: 0x00214C12 File Offset: 0x00213012
	protected void AutoConfirmVarPackage(VarPackage vp)
	{
		this.confirmedOncePackages.Add(vp);
		this.userConfirmedAPackage = true;
		this.requestedPackages.Remove(vp);
		this.CheckUserConfirmDenyComplete();
	}

	// Token: 0x06005ACD RID: 23245 RVA: 0x00214C3B File Offset: 0x0021303B
	protected void UserConfirmStickyVarPackage(VarPackage vp)
	{
		vp.PluginsAlwaysEnabled = true;
		vp.PluginsAlwaysDisabled = false;
		this.userConfirmedAPackage = true;
		this.requestedPackages.Remove(vp);
		this.CheckUserConfirmDenyComplete();
		FileManager.AutoConfirmAllWithSignature(this.GetPrompt(vp));
	}

	// Token: 0x06005ACE RID: 23246 RVA: 0x00214C71 File Offset: 0x00213071
	protected void UserDenyVarPackage(VarPackage vp)
	{
		this.requestedPackages.Remove(vp);
		this.CheckUserConfirmDenyComplete();
		FileManager.AutoDenyAllWithSignature(this.GetPrompt(vp));
	}

	// Token: 0x06005ACF RID: 23247 RVA: 0x00214C92 File Offset: 0x00213092
	protected void AutoDenyVarPackage(VarPackage vp)
	{
		this.requestedPackages.Remove(vp);
		this.CheckUserConfirmDenyComplete();
	}

	// Token: 0x06005AD0 RID: 23248 RVA: 0x00214CA7 File Offset: 0x002130A7
	protected void UserDenyStickyVarPackage(VarPackage vp)
	{
		vp.PluginsAlwaysEnabled = false;
		vp.PluginsAlwaysDisabled = true;
		this.requestedPackages.Remove(vp);
		this.CheckUserConfirmDenyComplete();
		FileManager.AutoDenyAllWithSignature(this.GetPrompt(vp));
	}

	// Token: 0x06005AD1 RID: 23249 RVA: 0x00214CD6 File Offset: 0x002130D6
	public void SetupUserConfirmDeny(MVRPlugin.UserConfirmDenyComplete callback)
	{
		this.HasRequestedPackages = false;
		this.requestedPackages.Clear();
		this.confirmedOncePackages.Clear();
		this.userConfirmedAPackage = false;
		this.userConfirmDenyCompleteCallback = callback;
	}

	// Token: 0x06005AD2 RID: 23250 RVA: 0x00214D03 File Offset: 0x00213103
	protected string GetPrompt(VarPackage vp)
	{
		return "Allow load of plugins from addon " + vp.Uid + "? (you should only allow if you trust the source of this plugin)";
	}

	// Token: 0x06005AD3 RID: 23251 RVA: 0x00214D1C File Offset: 0x0021311C
	public void UserConfirm()
	{
		using (HashSet<VarPackage>.Enumerator enumerator = this.requestedPackages.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				MVRPlugin.<UserConfirm>c__AnonStorey0 <UserConfirm>c__AnonStorey = new MVRPlugin.<UserConfirm>c__AnonStorey0();
				<UserConfirm>c__AnonStorey.vp = enumerator.Current;
				<UserConfirm>c__AnonStorey.$this = this;
				FileManager.ConfirmWithUser(this.GetPrompt(<UserConfirm>c__AnonStorey.vp), new UserActionCallback(<UserConfirm>c__AnonStorey.<>m__0), new UserActionCallback(<UserConfirm>c__AnonStorey.<>m__1), new UserActionCallback(<UserConfirm>c__AnonStorey.<>m__2), new UserActionCallback(<UserConfirm>c__AnonStorey.<>m__3), new UserActionCallback(<UserConfirm>c__AnonStorey.<>m__4), new UserActionCallback(<UserConfirm>c__AnonStorey.<>m__5));
			}
		}
	}

	// Token: 0x04004AF3 RID: 19187
	public string uid;

	// Token: 0x04004AF4 RID: 19188
	public JSONStorableUrl pluginURLJSON;

	// Token: 0x04004AF5 RID: 19189
	public List<MVRScriptController> scriptControllers;

	// Token: 0x04004AF6 RID: 19190
	public Transform configUI;

	// Token: 0x04004AF7 RID: 19191
	public RectTransform scriptControllerContent;

	// Token: 0x04004AF8 RID: 19192
	protected HashSet<VarPackage> requestedPackages;

	// Token: 0x04004AF9 RID: 19193
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private bool <HasRequestedPackages>k__BackingField;

	// Token: 0x04004AFA RID: 19194
	protected HashSet<VarPackage> confirmedOncePackages;

	// Token: 0x04004AFB RID: 19195
	protected bool userConfirmedAPackage;

	// Token: 0x04004AFC RID: 19196
	protected MVRPlugin.UserConfirmDenyComplete userConfirmDenyCompleteCallback;

	// Token: 0x02000C34 RID: 3124
	// (Invoke) Token: 0x06005AD5 RID: 23253
	public delegate void UserConfirmDenyComplete(MVRPlugin mvrp, bool didConfirm);

	// Token: 0x02001004 RID: 4100
	[CompilerGenerated]
	private sealed class <UserConfirm>c__AnonStorey0
	{
		// Token: 0x0600767E RID: 30334 RVA: 0x00214DDC File Offset: 0x002131DC
		public <UserConfirm>c__AnonStorey0()
		{
		}

		// Token: 0x0600767F RID: 30335 RVA: 0x00214DE4 File Offset: 0x002131E4
		internal void <>m__0()
		{
			this.$this.UserConfirmVarPackage(this.vp);
		}

		// Token: 0x06007680 RID: 30336 RVA: 0x00214DF7 File Offset: 0x002131F7
		internal void <>m__1()
		{
			this.$this.AutoConfirmVarPackage(this.vp);
		}

		// Token: 0x06007681 RID: 30337 RVA: 0x00214E0A File Offset: 0x0021320A
		internal void <>m__2()
		{
			this.$this.UserConfirmStickyVarPackage(this.vp);
		}

		// Token: 0x06007682 RID: 30338 RVA: 0x00214E1D File Offset: 0x0021321D
		internal void <>m__3()
		{
			this.$this.UserDenyVarPackage(this.vp);
		}

		// Token: 0x06007683 RID: 30339 RVA: 0x00214E30 File Offset: 0x00213230
		internal void <>m__4()
		{
			this.$this.AutoDenyVarPackage(this.vp);
		}

		// Token: 0x06007684 RID: 30340 RVA: 0x00214E43 File Offset: 0x00213243
		internal void <>m__5()
		{
			this.$this.UserDenyStickyVarPackage(this.vp);
		}

		// Token: 0x04006A50 RID: 27216
		internal VarPackage vp;

		// Token: 0x04006A51 RID: 27217
		internal MVRPlugin $this;
	}
}
