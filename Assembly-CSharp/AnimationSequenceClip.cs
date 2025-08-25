using System;
using UnityEngine;

// Token: 0x02000B5A RID: 2906
public class AnimationSequenceClip
{
	// Token: 0x0600511E RID: 20766 RVA: 0x001D4908 File Offset: 0x001D2D08
	public AnimationSequenceClip(string name, bool useCrossFade, float crossFadeTime)
	{
		this.nameJSON = new JSONStorableString("name", name);
		this.useCrossFadeJSON = new JSONStorableBool("useCrossFade", useCrossFade);
		this.crossFadeTimeJSON = new JSONStorableFloat("crossFadeTime", crossFadeTime, 0f, 5f, true, false);
		this.isPlayingJSON = new JSONStorableBool("isPlaying", false);
		this.playProgressJSON = new JSONStorableFloat("playProgress", 0f, 0f, 1f, true, false);
		this.removeAction = new JSONStorableAction("Remove", new JSONStorableAction.ActionCallback(this.Remove));
		this.moveBackwardAction = new JSONStorableAction("MoveBackward", new JSONStorableAction.ActionCallback(this.MoveBackward));
		this.moveForwardAction = new JSONStorableAction("MoveForward", new JSONStorableAction.ActionCallback(this.MoveForward));
	}

	// Token: 0x17000BC9 RID: 3017
	// (get) Token: 0x0600511F RID: 20767 RVA: 0x001D49E0 File Offset: 0x001D2DE0
	public string Name
	{
		get
		{
			return this.nameJSON.val;
		}
	}

	// Token: 0x17000BCA RID: 3018
	// (get) Token: 0x06005120 RID: 20768 RVA: 0x001D49ED File Offset: 0x001D2DED
	public bool UseCrossFade
	{
		get
		{
			return this.useCrossFadeJSON.val;
		}
	}

	// Token: 0x17000BCB RID: 3019
	// (get) Token: 0x06005121 RID: 20769 RVA: 0x001D49FA File Offset: 0x001D2DFA
	public float CrossFadeTime
	{
		get
		{
			return this.crossFadeTimeJSON.val;
		}
	}

	// Token: 0x17000BCC RID: 3020
	// (get) Token: 0x06005122 RID: 20770 RVA: 0x001D4A07 File Offset: 0x001D2E07
	// (set) Token: 0x06005123 RID: 20771 RVA: 0x001D4A14 File Offset: 0x001D2E14
	public bool IsPlaying
	{
		get
		{
			return this.isPlayingJSON.val;
		}
		set
		{
			this.isPlayingJSON.val = value;
		}
	}

	// Token: 0x17000BCD RID: 3021
	// (get) Token: 0x06005124 RID: 20772 RVA: 0x001D4A22 File Offset: 0x001D2E22
	// (set) Token: 0x06005125 RID: 20773 RVA: 0x001D4A2F File Offset: 0x001D2E2F
	public float PlayProgress
	{
		get
		{
			return this.playProgressJSON.val;
		}
		set
		{
			this.playProgressJSON.val = value;
		}
	}

	// Token: 0x06005126 RID: 20774 RVA: 0x001D4A3D File Offset: 0x001D2E3D
	public void Remove()
	{
		if (this.removeCallback != null)
		{
			this.removeCallback();
		}
	}

	// Token: 0x06005127 RID: 20775 RVA: 0x001D4A55 File Offset: 0x001D2E55
	public void MoveBackward()
	{
		if (this.moveBackwardCallback != null)
		{
			this.moveBackwardCallback();
		}
	}

	// Token: 0x06005128 RID: 20776 RVA: 0x001D4A6D File Offset: 0x001D2E6D
	public void MoveForward()
	{
		if (this.moveForwardCallback != null)
		{
			this.moveForwardCallback();
		}
	}

	// Token: 0x17000BCE RID: 3022
	// (get) Token: 0x06005129 RID: 20777 RVA: 0x001D4A85 File Offset: 0x001D2E85
	// (set) Token: 0x0600512A RID: 20778 RVA: 0x001D4A90 File Offset: 0x001D2E90
	public Transform UI
	{
		get
		{
			return this._ui;
		}
		set
		{
			if (this._ui != value)
			{
				if (this._ui != null)
				{
					this.nameJSON.text = null;
					this.useCrossFadeJSON.toggle = null;
					this.crossFadeTimeJSON.slider = null;
					this.isPlayingJSON.indicator = null;
					this.playProgressJSON.slider = null;
					this.removeAction.button = null;
					this.moveBackwardAction.button = null;
					this.moveForwardAction.button = null;
				}
				this._ui = value;
				if (this._ui != null)
				{
					AnimationSequenceClipUI component = this._ui.GetComponent<AnimationSequenceClipUI>();
					if (component != null)
					{
						this.nameJSON.text = component.nameText;
						this.useCrossFadeJSON.indicator = component.useCrossFadeIndicator;
						this.crossFadeTimeJSON.slider = component.crossFadeTimeSlider;
						this.isPlayingJSON.indicator = component.isPlayingIndicator;
						this.playProgressJSON.slider = component.playProgressSlider;
						this.removeAction.button = component.removeButton;
						this.moveBackwardAction.button = component.moveBackwardButton;
						this.moveForwardAction.button = component.moveForwardButton;
					}
				}
			}
		}
	}

	// Token: 0x040040ED RID: 16621
	protected JSONStorableString nameJSON;

	// Token: 0x040040EE RID: 16622
	protected JSONStorableBool useCrossFadeJSON;

	// Token: 0x040040EF RID: 16623
	protected JSONStorableFloat crossFadeTimeJSON;

	// Token: 0x040040F0 RID: 16624
	protected JSONStorableBool isPlayingJSON;

	// Token: 0x040040F1 RID: 16625
	protected JSONStorableFloat playProgressJSON;

	// Token: 0x040040F2 RID: 16626
	public AnimationSequenceClip.RemoveCallback removeCallback;

	// Token: 0x040040F3 RID: 16627
	protected JSONStorableAction removeAction;

	// Token: 0x040040F4 RID: 16628
	public AnimationSequenceClip.MoveBackwardCallback moveBackwardCallback;

	// Token: 0x040040F5 RID: 16629
	protected JSONStorableAction moveBackwardAction;

	// Token: 0x040040F6 RID: 16630
	public AnimationSequenceClip.MoveForwardCallback moveForwardCallback;

	// Token: 0x040040F7 RID: 16631
	protected JSONStorableAction moveForwardAction;

	// Token: 0x040040F8 RID: 16632
	protected Transform _ui;

	// Token: 0x02000B5B RID: 2907
	// (Invoke) Token: 0x0600512C RID: 20780
	public delegate void RemoveCallback();

	// Token: 0x02000B5C RID: 2908
	// (Invoke) Token: 0x06005130 RID: 20784
	public delegate void MoveBackwardCallback();

	// Token: 0x02000B5D RID: 2909
	// (Invoke) Token: 0x06005134 RID: 20788
	public delegate void MoveForwardCallback();
}
