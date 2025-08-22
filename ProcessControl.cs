using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000C46 RID: 3142
public class ProcessControl : MonoBehaviour
{
	// Token: 0x06005B5B RID: 23387 RVA: 0x00218686 File Offset: 0x00216A86
	public ProcessControl()
	{
	}

	// Token: 0x06005B5C RID: 23388 RVA: 0x00218698 File Offset: 0x00216A98
	private void InitSelector()
	{
		this._prioritySelector.currentValue = this._processPriorityClass.ToString();
		UIPopup prioritySelector = this._prioritySelector;
		prioritySelector.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(prioritySelector.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetProcessPriorityClass));
	}

	// Token: 0x17000D68 RID: 3432
	// (get) Token: 0x06005B5D RID: 23389 RVA: 0x002186E8 File Offset: 0x00216AE8
	// (set) Token: 0x06005B5E RID: 23390 RVA: 0x002186F0 File Offset: 0x00216AF0
	public UIPopup prioritySelector
	{
		get
		{
			return this._prioritySelector;
		}
		set
		{
			if (this._prioritySelector != value)
			{
				if (this._prioritySelector != null)
				{
					UIPopup prioritySelector = this._prioritySelector;
					prioritySelector.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Remove(prioritySelector.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetProcessPriorityClass));
				}
				this._prioritySelector = value;
				this.InitSelector();
			}
		}
	}

	// Token: 0x06005B5F RID: 23391 RVA: 0x00218754 File Offset: 0x00216B54
	private void SetInternalProcessPriorityClass()
	{
		if (!Application.isEditor)
		{
			Process currentProcess = Process.GetCurrentProcess();
			if (currentProcess.PriorityClass != this._processPriorityClass)
			{
				currentProcess.PriorityClass = this._processPriorityClass;
			}
		}
	}

	// Token: 0x17000D69 RID: 3433
	// (get) Token: 0x06005B60 RID: 23392 RVA: 0x0021878E File Offset: 0x00216B8E
	// (set) Token: 0x06005B61 RID: 23393 RVA: 0x00218798 File Offset: 0x00216B98
	public ProcessPriorityClass processPriorityClass
	{
		get
		{
			return this._processPriorityClass;
		}
		set
		{
			if (this._processPriorityClass != value)
			{
				this._processPriorityClass = value;
				this.SetInternalProcessPriorityClass();
				if (this._prioritySelector != null)
				{
					this._prioritySelector.currentValue = this._processPriorityClass.ToString();
				}
			}
		}
	}

	// Token: 0x17000D6A RID: 3434
	// (get) Token: 0x06005B62 RID: 23394 RVA: 0x002187EB File Offset: 0x00216BEB
	// (set) Token: 0x06005B63 RID: 23395 RVA: 0x002187F3 File Offset: 0x00216BF3
	public bool debug
	{
		get
		{
			return this._debug;
		}
		set
		{
			if (this._debug != value)
			{
				this._debug = value;
			}
		}
	}

	// Token: 0x06005B64 RID: 23396 RVA: 0x00218808 File Offset: 0x00216C08
	public void SetProcessPriorityClass(string type)
	{
		this.processPriorityClass = (ProcessPriorityClass)Enum.Parse(typeof(ProcessPriorityClass), type);
	}

	// Token: 0x06005B65 RID: 23397 RVA: 0x00218825 File Offset: 0x00216C25
	private void Start()
	{
		this.SetInternalProcessPriorityClass();
		if (this._prioritySelector != null)
		{
			this.InitSelector();
		}
	}

	// Token: 0x04004B4F RID: 19279
	[SerializeField]
	private UIPopup _prioritySelector;

	// Token: 0x04004B50 RID: 19280
	[SerializeField]
	private ProcessPriorityClass _processPriorityClass = ProcessPriorityClass.Normal;

	// Token: 0x04004B51 RID: 19281
	[SerializeField]
	private bool _debug;
}
