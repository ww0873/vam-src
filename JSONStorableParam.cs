using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using SimpleJSON;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000CE2 RID: 3298
public abstract class JSONStorableParam
{
	// Token: 0x060063C8 RID: 25544 RVA: 0x0025D383 File Offset: 0x0025B783
	protected JSONStorableParam()
	{
	}

	// Token: 0x17000EA9 RID: 3753
	// (get) Token: 0x060063C9 RID: 25545 RVA: 0x0025D399 File Offset: 0x0025B799
	// (set) Token: 0x060063CA RID: 25546 RVA: 0x0025D3A1 File Offset: 0x0025B7A1
	public bool locked
	{
		[CompilerGenerated]
		get
		{
			return this.<locked>k__BackingField;
		}
		[CompilerGenerated]
		protected set
		{
			this.<locked>k__BackingField = value;
		}
	}

	// Token: 0x060063CB RID: 25547 RVA: 0x0025D3AA File Offset: 0x0025B7AA
	public void SetLock(string lockUid)
	{
		if (this.locks == null)
		{
			this.locks = new HashSet<string>();
		}
		if (!this.locks.Contains(lockUid))
		{
			this.locks.Add(lockUid);
		}
		this.locked = true;
	}

	// Token: 0x060063CC RID: 25548 RVA: 0x0025D3E7 File Offset: 0x0025B7E7
	public void ClearLock(string lockUid)
	{
		if (this.locks != null)
		{
			this.locks.Remove(lockUid);
			if (this.locks.Count == 0)
			{
				this.locked = false;
			}
		}
	}

	// Token: 0x060063CD RID: 25549 RVA: 0x0025D418 File Offset: 0x0025B818
	public void ClearAllLocks()
	{
		if (this.locks != null)
		{
			this.locks.Clear();
		}
		this.locked = false;
	}

	// Token: 0x060063CE RID: 25550 RVA: 0x0025D438 File Offset: 0x0025B838
	protected virtual bool NeedsStore(JSONClass jc, bool includePhysical = true, bool includeAppearance = true)
	{
		bool result = false;
		if (this.isStorable)
		{
			switch (this.storeType)
			{
			case JSONStorableParam.StoreType.Appearance:
				result = includeAppearance;
				break;
			case JSONStorableParam.StoreType.Physical:
				result = includePhysical;
				break;
			case JSONStorableParam.StoreType.Any:
				result = (includeAppearance || includePhysical);
				break;
			case JSONStorableParam.StoreType.Full:
				result = (includeAppearance && includePhysical);
				break;
			}
		}
		return result;
	}

	// Token: 0x060063CF RID: 25551
	public abstract bool StoreJSON(JSONClass jc, bool includePhysical = true, bool includeAppearance = true, bool forceStore = false);

	// Token: 0x060063D0 RID: 25552 RVA: 0x0025D4A4 File Offset: 0x0025B8A4
	public virtual bool NeedsRestore(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true)
	{
		bool result = false;
		if (this.restoreTime == JSONStorableParam.RestoreTime.Normal && this.isRestorable && !this.locked)
		{
			switch (this.storeType)
			{
			case JSONStorableParam.StoreType.Appearance:
				result = restoreAppearance;
				break;
			case JSONStorableParam.StoreType.Physical:
				result = restorePhysical;
				break;
			case JSONStorableParam.StoreType.Any:
				result = (restoreAppearance || restorePhysical);
				break;
			case JSONStorableParam.StoreType.Full:
				result = (restoreAppearance && restorePhysical);
				break;
			}
		}
		return result;
	}

	// Token: 0x060063D1 RID: 25553
	public abstract void RestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, bool setMissingToDefault = true);

	// Token: 0x060063D2 RID: 25554 RVA: 0x0025D528 File Offset: 0x0025B928
	public virtual bool NeedsLateRestore(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true)
	{
		bool result = false;
		if (this.restoreTime == JSONStorableParam.RestoreTime.Late && this.isRestorable && !this.locked)
		{
			switch (this.storeType)
			{
			case JSONStorableParam.StoreType.Appearance:
				result = restoreAppearance;
				break;
			case JSONStorableParam.StoreType.Physical:
				result = restorePhysical;
				break;
			case JSONStorableParam.StoreType.Any:
				result = (restoreAppearance || restorePhysical);
				break;
			case JSONStorableParam.StoreType.Full:
				result = (restoreAppearance && restorePhysical);
				break;
			}
		}
		return result;
	}

	// Token: 0x060063D3 RID: 25555
	public abstract void LateRestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, bool setMissingToDefault = true);

	// Token: 0x060063D4 RID: 25556
	public abstract void SetDefaultFromCurrent();

	// Token: 0x060063D5 RID: 25557
	public abstract void SetValToDefault();

	// Token: 0x060063D6 RID: 25558 RVA: 0x0025D5AA File Offset: 0x0025B9AA
	public void RegisterDefaultButtton(Button b, bool isAlt = false)
	{
		if (isAlt)
		{
			this.defaultButtonAlt = b;
		}
		else
		{
			this.defaultButton = b;
		}
	}

	// Token: 0x17000EAA RID: 3754
	// (get) Token: 0x060063D7 RID: 25559 RVA: 0x0025D5C5 File Offset: 0x0025B9C5
	// (set) Token: 0x060063D8 RID: 25560 RVA: 0x0025D5D0 File Offset: 0x0025B9D0
	public Button defaultButton
	{
		get
		{
			return this._defaultButton;
		}
		set
		{
			if (this._defaultButton != value)
			{
				if (this._defaultButton != null)
				{
					this._defaultButton.onClick.RemoveListener(new UnityAction(this.SetValToDefault));
				}
				this._defaultButton = value;
				if (this._defaultButton != null)
				{
					this._defaultButton.onClick.AddListener(new UnityAction(this.SetValToDefault));
				}
			}
		}
	}

	// Token: 0x17000EAB RID: 3755
	// (get) Token: 0x060063D9 RID: 25561 RVA: 0x0025D651 File Offset: 0x0025BA51
	// (set) Token: 0x060063DA RID: 25562 RVA: 0x0025D65C File Offset: 0x0025BA5C
	public Button defaultButtonAlt
	{
		get
		{
			return this._defaultButtonAlt;
		}
		set
		{
			if (this._defaultButtonAlt != value)
			{
				if (this._defaultButtonAlt != null)
				{
					this._defaultButtonAlt.onClick.RemoveListener(new UnityAction(this.SetValToDefault));
				}
				this._defaultButton = value;
				if (this._defaultButton != null)
				{
					this._defaultButtonAlt.onClick.AddListener(new UnityAction(this.SetValToDefault));
				}
			}
		}
	}

	// Token: 0x040053F4 RID: 21492
	public JSONStorable.Type type;

	// Token: 0x040053F5 RID: 21493
	public bool isStorable = true;

	// Token: 0x040053F6 RID: 21494
	public bool isRestorable = true;

	// Token: 0x040053F7 RID: 21495
	public JSONStorableParam.RestoreTime restoreTime;

	// Token: 0x040053F8 RID: 21496
	public string name;

	// Token: 0x040053F9 RID: 21497
	public string altName;

	// Token: 0x040053FA RID: 21498
	public bool hidden;

	// Token: 0x040053FB RID: 21499
	public bool registeredAltName;

	// Token: 0x040053FC RID: 21500
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private bool <locked>k__BackingField;

	// Token: 0x040053FD RID: 21501
	protected HashSet<string> locks;

	// Token: 0x040053FE RID: 21502
	public JSONStorableParam.StoreType storeType;

	// Token: 0x040053FF RID: 21503
	public JSONStorable storable;

	// Token: 0x04005400 RID: 21504
	protected Button _defaultButton;

	// Token: 0x04005401 RID: 21505
	protected Button _defaultButtonAlt;

	// Token: 0x02000CE3 RID: 3299
	public enum RestoreTime
	{
		// Token: 0x04005403 RID: 21507
		Normal,
		// Token: 0x04005404 RID: 21508
		Late
	}

	// Token: 0x02000CE4 RID: 3300
	public enum StoreType
	{
		// Token: 0x04005406 RID: 21510
		Appearance,
		// Token: 0x04005407 RID: 21511
		Physical,
		// Token: 0x04005408 RID: 21512
		Any,
		// Token: 0x04005409 RID: 21513
		Full
	}
}
