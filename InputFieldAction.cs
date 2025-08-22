using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000DCB RID: 3531
public class InputFieldAction : MonoBehaviour, ISelectHandler, IEventSystemHandler
{
	// Token: 0x06006D70 RID: 28016 RVA: 0x0029311C File Offset: 0x0029151C
	public InputFieldAction()
	{
	}

	// Token: 0x06006D71 RID: 28017 RVA: 0x00293124 File Offset: 0x00291524
	public void Submit()
	{
		if (this.onSubmitHandlers != null)
		{
			this.onSubmitHandlers();
		}
	}

	// Token: 0x06006D72 RID: 28018 RVA: 0x0029313C File Offset: 0x0029153C
	public void Select()
	{
		if (LookInputModule.singleton != null)
		{
			LookInputModule.singleton.Select(base.gameObject);
		}
	}

	// Token: 0x06006D73 RID: 28019 RVA: 0x0029315E File Offset: 0x0029155E
	public void OnSelect(BaseEventData eventData)
	{
		if (this.onSelectedHandlers != null)
		{
			this.onSelectedHandlers();
		}
	}

	// Token: 0x06006D74 RID: 28020 RVA: 0x00293176 File Offset: 0x00291576
	public void Up()
	{
		if (this.onUpHandlers != null)
		{
			this.onUpHandlers();
		}
	}

	// Token: 0x06006D75 RID: 28021 RVA: 0x0029318E File Offset: 0x0029158E
	public void Down()
	{
		if (this.onDownHandlers != null)
		{
			this.onDownHandlers();
		}
	}

	// Token: 0x06006D76 RID: 28022 RVA: 0x002931A6 File Offset: 0x002915A6
	private void Awake()
	{
		if (this.inputField == null)
		{
			this.inputField = base.GetComponent<InputField>();
		}
	}

	// Token: 0x06006D77 RID: 28023 RVA: 0x002931C8 File Offset: 0x002915C8
	private void Update()
	{
		if (this.wasFocusedLastFrame)
		{
			if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
			{
				this.Submit();
			}
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				this.Up();
			}
			if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				this.Down();
			}
		}
		this.wasFocusedLastFrame = (this.inputField != null && this.inputField.isFocused);
	}

	// Token: 0x04005EDA RID: 24282
	public InputFieldAction.OnSubmit onSubmitHandlers;

	// Token: 0x04005EDB RID: 24283
	public InputFieldAction.OnSelected onSelectedHandlers;

	// Token: 0x04005EDC RID: 24284
	public InputFieldAction.OnUp onUpHandlers;

	// Token: 0x04005EDD RID: 24285
	public InputFieldAction.OnDown onDownHandlers;

	// Token: 0x04005EDE RID: 24286
	public InputField inputField;

	// Token: 0x04005EDF RID: 24287
	protected bool wasFocusedLastFrame;

	// Token: 0x02000DCC RID: 3532
	// (Invoke) Token: 0x06006D79 RID: 28025
	public delegate void OnSubmit();

	// Token: 0x02000DCD RID: 3533
	// (Invoke) Token: 0x06006D7D RID: 28029
	public delegate void OnSelected();

	// Token: 0x02000DCE RID: 3534
	// (Invoke) Token: 0x06006D81 RID: 28033
	public delegate void OnUp();

	// Token: 0x02000DCF RID: 3535
	// (Invoke) Token: 0x06006D85 RID: 28037
	public delegate void OnDown();
}
