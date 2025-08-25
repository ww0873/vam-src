using System;
using System.Runtime.CompilerServices;
using Leap.Unity;
using UnityEngine;

// Token: 0x02000755 RID: 1877
public class CycleHandPairs : MonoBehaviour
{
	// Token: 0x06003047 RID: 12359 RVA: 0x000FA22B File Offset: 0x000F862B
	public CycleHandPairs()
	{
		KeyCode[] array = new KeyCode[6];
		RuntimeHelpers.InitializeArray(array, fieldof(<PrivateImplementationDetails>.$field-AF693FAC177808B58813043932F8B312986F817A).FieldHandle);
		this.keyCodes = array;
		base..ctor();
	}

	// Token: 0x170005CB RID: 1483
	// (get) Token: 0x06003048 RID: 12360 RVA: 0x000FA24A File Offset: 0x000F864A
	// (set) Token: 0x06003049 RID: 12361 RVA: 0x000FA252 File Offset: 0x000F8652
	public int CurrentGroup
	{
		get
		{
			return this.currentGroup;
		}
		set
		{
			this.disableAllGroups();
			this.currentGroup = value;
			this.HandPool.EnableGroup(this.GroupNames[value]);
		}
	}

	// Token: 0x0600304A RID: 12362 RVA: 0x000FA274 File Offset: 0x000F8674
	private void Start()
	{
		this.HandPool = base.GetComponent<HandModelManager>();
		this.disableAllGroups();
		this.CurrentGroup = 0;
	}

	// Token: 0x0600304B RID: 12363 RVA: 0x000FA290 File Offset: 0x000F8690
	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.RightArrow) && this.CurrentGroup < this.GroupNames.Length - 1)
		{
			this.CurrentGroup++;
		}
		if (Input.GetKeyUp(KeyCode.LeftArrow) && this.CurrentGroup > 0)
		{
			this.CurrentGroup--;
		}
		for (int i = 0; i < this.keyCodes.Length; i++)
		{
			if (Input.GetKeyDown(this.keyCodes[i]))
			{
				this.HandPool.ToggleGroup(this.GroupNames[i]);
			}
		}
		if (Input.GetKeyUp(KeyCode.Alpha0))
		{
			this.disableAllGroups();
		}
	}

	// Token: 0x0600304C RID: 12364 RVA: 0x000FA348 File Offset: 0x000F8748
	private void disableAllGroups()
	{
		for (int i = 0; i < this.GroupNames.Length; i++)
		{
			this.HandPool.DisableGroup(this.GroupNames[i]);
		}
	}

	// Token: 0x04002428 RID: 9256
	public HandModelManager HandPool;

	// Token: 0x04002429 RID: 9257
	public string[] GroupNames;

	// Token: 0x0400242A RID: 9258
	private int currentGroup;

	// Token: 0x0400242B RID: 9259
	private KeyCode[] keyCodes;
}
