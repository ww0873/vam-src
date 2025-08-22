using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

// Token: 0x02000C02 RID: 3074
public class AsyncFlag
{
	// Token: 0x060058BB RID: 22715 RVA: 0x002085D0 File Offset: 0x002069D0
	public AsyncFlag(string name)
	{
		this.Name = name;
	}

	// Token: 0x17000D08 RID: 3336
	// (get) Token: 0x060058BC RID: 22716 RVA: 0x002085DF File Offset: 0x002069DF
	// (set) Token: 0x060058BD RID: 22717 RVA: 0x002085E7 File Offset: 0x002069E7
	public string Name
	{
		[CompilerGenerated]
		get
		{
			return this.<Name>k__BackingField;
		}
		[CompilerGenerated]
		set
		{
			this.<Name>k__BackingField = value;
		}
	}

	// Token: 0x17000D09 RID: 3337
	// (get) Token: 0x060058BE RID: 22718 RVA: 0x002085F0 File Offset: 0x002069F0
	// (set) Token: 0x060058BF RID: 22719 RVA: 0x002085F8 File Offset: 0x002069F8
	public bool Raised
	{
		[CompilerGenerated]
		get
		{
			return this.<Raised>k__BackingField;
		}
		[CompilerGenerated]
		protected set
		{
			this.<Raised>k__BackingField = value;
		}
	}

	// Token: 0x060058C0 RID: 22720 RVA: 0x00208601 File Offset: 0x00206A01
	public void Raise()
	{
		this.Raised = true;
	}

	// Token: 0x060058C1 RID: 22721 RVA: 0x0020860A File Offset: 0x00206A0A
	public void Lower()
	{
		this.Raised = false;
	}

	// Token: 0x0400490B RID: 18699
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private string <Name>k__BackingField;

	// Token: 0x0400490C RID: 18700
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private bool <Raised>k__BackingField;
}
