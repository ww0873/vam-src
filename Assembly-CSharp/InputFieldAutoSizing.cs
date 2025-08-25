using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000DD0 RID: 3536
public class InputFieldAutoSizing : InputField
{
	// Token: 0x06006D88 RID: 28040 RVA: 0x00293250 File Offset: 0x00291650
	public InputFieldAutoSizing()
	{
	}

	// Token: 0x06006D89 RID: 28041 RVA: 0x00293260 File Offset: 0x00291660
	protected void SyncPreferredHeight()
	{
		if (this.textGenerator != null && this.parentLayoutElement != null)
		{
			Vector2 size = base.textComponent.rectTransform.rect.size;
			TextGenerationSettings generationSettings = base.textComponent.GetGenerationSettings(size);
			generationSettings.generateOutOfBounds = false;
			float preferredHeight = this.textGenerator.GetPreferredHeight(base.text, generationSettings);
			if (preferredHeight > base.textComponent.rectTransform.rect.height)
			{
				this.parentLayoutElement.preferredHeight = preferredHeight;
			}
			else if (preferredHeight < base.textComponent.rectTransform.rect.height)
			{
				this.parentLayoutElement.preferredHeight = preferredHeight;
			}
			if (this.keepAtBottom > 0)
			{
				this.scrollRect.verticalNormalizedPosition = 0f;
			}
		}
	}

	// Token: 0x06006D8A RID: 28042 RVA: 0x00293341 File Offset: 0x00291741
	protected void OnValueChange(string s)
	{
		if (this.scrollFollowIfAtBottom && this.scrollRect.verticalScrollbar.value < 0.01f)
		{
			this.keepAtBottom = 2;
		}
		this.preferredHeightDirty = true;
	}

	// Token: 0x06006D8B RID: 28043 RVA: 0x00293378 File Offset: 0x00291778
	private void Update()
	{
		if (Application.isPlaying)
		{
			if (this.preferredHeightDirty)
			{
				this.SyncPreferredHeight();
				this.preferredHeightDirty = false;
			}
			if (this.keepCursorVisible && base.isFocused && this.lastCaretPosition != base.caretPosition)
			{
				this.lastCaretPosition = base.caretPosition;
				Vector2 size = base.textComponent.rectTransform.rect.size;
				TextGenerationSettings generationSettings = base.textComponent.GetGenerationSettings(size);
				generationSettings.generateOutOfBounds = false;
				float preferredHeight = this.textGenerator.GetPreferredHeight(base.text, generationSettings);
				float num = preferredHeight * 0.5f;
				float num2 = this.textGenerator.characters[base.caretPosition].cursorPos.y + num;
				this.scrollRect.verticalNormalizedPosition = num2 / preferredHeight;
			}
			if (this.scrollFollowIfAtBottom && this.keepAtBottom > 0)
			{
				this.keepAtBottom--;
				this.scrollRect.verticalNormalizedPosition = 0f;
			}
		}
	}

	// Token: 0x06006D8C RID: 28044 RVA: 0x00293494 File Offset: 0x00291894
	protected override void OnEnable()
	{
		base.OnEnable();
		if (Application.isPlaying && this.scrollFollowIfAtBottom)
		{
			this.scrollRect.verticalNormalizedPosition = 0f;
			this.scrollRect.verticalScrollbar.value = 0f;
		}
	}

	// Token: 0x06006D8D RID: 28045 RVA: 0x002934E4 File Offset: 0x002918E4
	protected override void Awake()
	{
		base.Awake();
		if (Application.isPlaying)
		{
			this.textGenerator = new TextGenerator();
			if (base.transform.parent != null)
			{
				this.parentLayoutElement = base.transform.parent.GetComponent<LayoutElement>();
			}
			base.onValueChanged.AddListener(new UnityAction<string>(this.OnValueChange));
			if (this.scrollFollowIfAtBottom)
			{
				this.keepAtBottom = 2;
				this.scrollRect.verticalNormalizedPosition = 0f;
				this.scrollRect.verticalScrollbar.value = 0f;
			}
			this.SyncPreferredHeight();
		}
	}

	// Token: 0x04005EE0 RID: 24288
	public ScrollRect scrollRect;

	// Token: 0x04005EE1 RID: 24289
	public bool keepCursorVisible = true;

	// Token: 0x04005EE2 RID: 24290
	public bool scrollFollowIfAtBottom;

	// Token: 0x04005EE3 RID: 24291
	protected TextGenerator textGenerator;

	// Token: 0x04005EE4 RID: 24292
	protected LayoutElement parentLayoutElement;

	// Token: 0x04005EE5 RID: 24293
	protected int keepAtBottom;

	// Token: 0x04005EE6 RID: 24294
	protected bool preferredHeightDirty;

	// Token: 0x04005EE7 RID: 24295
	protected int lastCaretPosition;

	// Token: 0x04005EE8 RID: 24296
	protected bool scrolledToBottom;
}
