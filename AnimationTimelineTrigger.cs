using System;
using System.Runtime.CompilerServices;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000D96 RID: 3478
public class AnimationTimelineTrigger : Trigger
{
	// Token: 0x06006B31 RID: 27441 RVA: 0x002877DC File Offset: 0x00285BDC
	public AnimationTimelineTrigger()
	{
	}

	// Token: 0x06006B32 RID: 27442 RVA: 0x002877E4 File Offset: 0x00285BE4
	public override JSONClass GetJSON(string subScenePrefix = null)
	{
		JSONClass json = base.GetJSON(subScenePrefix);
		json["startTime"].AsFloat = this._triggerStartTime;
		json["endTime"].AsFloat = this._triggerEndTime;
		return json;
	}

	// Token: 0x06006B33 RID: 27443 RVA: 0x00287828 File Offset: 0x00285C28
	public override void RestoreFromJSON(JSONClass jc, string subScenePrefix, bool isMerge)
	{
		base.RestoreFromJSON(jc, subScenePrefix, isMerge);
		if (jc["startTime"] != null)
		{
			this.triggerStartTime = jc["startTime"].AsFloat;
		}
		if (jc["endTime"] != null)
		{
			this.triggerEndTime = jc["endTime"].AsFloat;
		}
	}

	// Token: 0x06006B34 RID: 27444 RVA: 0x00287896 File Offset: 0x00285C96
	protected void SetTriggerStartTimeToCurrentTime()
	{
		if (this.timeLineHandler != null)
		{
			this.triggerStartTime = this.timeLineHandler.GetCurrentTimeCounter();
		}
	}

	// Token: 0x17000FC4 RID: 4036
	// (get) Token: 0x06006B35 RID: 27445 RVA: 0x002878B4 File Offset: 0x00285CB4
	// (set) Token: 0x06006B36 RID: 27446 RVA: 0x002878BC File Offset: 0x00285CBC
	public float triggerStartTime
	{
		get
		{
			return this._triggerStartTime;
		}
		set
		{
			if (this._triggerStartTime != value)
			{
				this._triggerStartTime = value;
				if (this.triggerStartTimeSlider != null)
				{
					this.triggerStartTimeSlider.value = this._triggerStartTime;
				}
				if (this._triggerEndTime < this._triggerStartTime)
				{
					this.triggerEndTime = this._triggerStartTime;
				}
			}
		}
	}

	// Token: 0x06006B37 RID: 27447 RVA: 0x0028791B File Offset: 0x00285D1B
	protected void SetTriggerEndTimeToCurrentTime()
	{
		if (this.timeLineHandler != null)
		{
			this.triggerEndTime = this.timeLineHandler.GetCurrentTimeCounter();
		}
	}

	// Token: 0x17000FC5 RID: 4037
	// (get) Token: 0x06006B38 RID: 27448 RVA: 0x00287939 File Offset: 0x00285D39
	// (set) Token: 0x06006B39 RID: 27449 RVA: 0x00287944 File Offset: 0x00285D44
	public float triggerEndTime
	{
		get
		{
			return this._triggerEndTime;
		}
		set
		{
			if (this._triggerEndTime != value)
			{
				this._triggerEndTime = value;
				if (this._triggerEndTime < this._triggerStartTime)
				{
					this.triggerEndTime = this._triggerStartTime;
				}
				if (this.triggerEndTimeSlider != null)
				{
					this.triggerEndTimeSlider.value = this._triggerEndTime;
				}
			}
		}
	}

	// Token: 0x17000FC6 RID: 4038
	// (get) Token: 0x06006B3A RID: 27450 RVA: 0x002879A3 File Offset: 0x00285DA3
	// (set) Token: 0x06006B3B RID: 27451 RVA: 0x002879AC File Offset: 0x00285DAC
	public bool selected
	{
		get
		{
			return this._selected;
		}
		set
		{
			if (this._selected != value)
			{
				this._selected = value;
				if (this.selectToggle != null)
				{
					this.selectToggle.isOn = this._selected;
				}
				if (this.selectedIndicator != null)
				{
					this.selectedIndicator.SetActive(this._selected);
				}
				if (this.onSelectedHandlers != null)
				{
					this.onSelectedHandlers(this);
				}
			}
		}
	}

	// Token: 0x06006B3C RID: 27452 RVA: 0x00287A28 File Offset: 0x00285E28
	public override void InitTriggerUI()
	{
		base.InitTriggerUI();
		if (this.triggerPanel != null)
		{
			AnimationTimelineTriggerUI component = this.triggerPanel.GetComponent<AnimationTimelineTriggerUI>();
			if (component != null)
			{
				this.triggerStartTimeSlider = component.triggerStartTimeSlider;
				this.triggerEndTimeSlider = component.triggerEndTimeSlider;
				this.startTimeToCurrentTimeButton = component.startTimeToCurrentTimeButton;
				this.endTimeToCurrentTimeButton = component.endTimeToCurrentTimeButton;
				this.selectToggle = component.selectToggle;
				this.selectedIndicator = component.selectedIndicator;
			}
			if (this.triggerStartTimeSlider != null)
			{
				if (this.timeLineHandler != null)
				{
					this.triggerStartTimeSlider.maxValue = this.timeLineHandler.GetTotalTime();
				}
				this.triggerStartTimeSlider.value = this._triggerStartTime;
				this.triggerStartTimeSlider.onValueChanged.AddListener(new UnityAction<float>(this.<InitTriggerUI>m__0));
			}
			if (this.triggerEndTimeSlider != null)
			{
				if (this.timeLineHandler != null)
				{
					this.triggerEndTimeSlider.maxValue = this.timeLineHandler.GetTotalTime();
				}
				this.triggerEndTimeSlider.value = this._triggerEndTime;
				this.triggerEndTimeSlider.onValueChanged.AddListener(new UnityAction<float>(this.<InitTriggerUI>m__1));
			}
			if (this.startTimeToCurrentTimeButton != null)
			{
				this.startTimeToCurrentTimeButton.onClick.AddListener(new UnityAction(this.SetTriggerStartTimeToCurrentTime));
			}
			if (this.endTimeToCurrentTimeButton != null)
			{
				this.endTimeToCurrentTimeButton.onClick.AddListener(new UnityAction(this.SetTriggerEndTimeToCurrentTime));
			}
			if (this.selectedIndicator != null)
			{
				this.selectedIndicator.SetActive(this._selected);
			}
			if (this.selectToggle != null)
			{
				this.selectToggle.isOn = this._selected;
				this.selectToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitTriggerUI>m__2));
			}
		}
	}

	// Token: 0x06006B3D RID: 27453 RVA: 0x00287C24 File Offset: 0x00286024
	public override void DeregisterUI()
	{
		base.DeregisterUI();
		this.selectedIndicator = null;
		if (this.triggerStartTimeSlider != null)
		{
			this.triggerStartTimeSlider.onValueChanged.RemoveAllListeners();
			this.triggerStartTimeSlider = null;
		}
		if (this.triggerEndTimeSlider != null)
		{
			this.triggerEndTimeSlider.onValueChanged.RemoveAllListeners();
			this.triggerEndTimeSlider = null;
		}
		if (this.startTimeToCurrentTimeButton != null)
		{
			this.startTimeToCurrentTimeButton.onClick.RemoveListener(new UnityAction(this.SetTriggerStartTimeToCurrentTime));
			this.startTimeToCurrentTimeButton = null;
		}
		if (this.endTimeToCurrentTimeButton != null)
		{
			this.endTimeToCurrentTimeButton.onClick.RemoveListener(new UnityAction(this.SetTriggerEndTimeToCurrentTime));
			this.endTimeToCurrentTimeButton = null;
		}
		if (this.selectToggle != null)
		{
			this.selectToggle.onValueChanged.RemoveAllListeners();
			this.selectToggle = null;
		}
	}

	// Token: 0x06006B3E RID: 27454 RVA: 0x00287D20 File Offset: 0x00286120
	public void ResyncMaxStartAndEndTimes()
	{
		if (this.timeLineHandler != null)
		{
			if (this.triggerEndTimeSlider != null)
			{
				this.triggerEndTimeSlider.maxValue = this.timeLineHandler.GetTotalTime();
			}
			if (this.triggerStartTimeSlider != null)
			{
				this.triggerStartTimeSlider.maxValue = this.timeLineHandler.GetTotalTime();
			}
		}
	}

	// Token: 0x06006B3F RID: 27455 RVA: 0x00287D88 File Offset: 0x00286188
	public void Update(bool reverse_playback, float lastPlaybackTime)
	{
		if (this.timeLineHandler != null)
		{
			float currentTimeCounter = this.timeLineHandler.GetCurrentTimeCounter();
			this.reverse = reverse_playback;
			if (this.reverse)
			{
				if (currentTimeCounter > lastPlaybackTime)
				{
					if (this._triggerEndTime <= lastPlaybackTime)
					{
						base.active = true;
					}
					if (this._triggerStartTime <= lastPlaybackTime)
					{
						base.transitionInterpValue = 0f;
						base.active = false;
					}
					if (this._triggerEndTime >= currentTimeCounter)
					{
						base.active = true;
					}
					if (this._triggerStartTime >= currentTimeCounter)
					{
						base.transitionInterpValue = 0f;
						base.active = false;
					}
				}
				else
				{
					if (this._triggerEndTime <= lastPlaybackTime && this._triggerEndTime >= currentTimeCounter)
					{
						base.active = true;
					}
					if (this._triggerStartTime <= lastPlaybackTime && this._triggerStartTime >= currentTimeCounter)
					{
						base.transitionInterpValue = 0f;
						base.active = false;
					}
				}
			}
			else if (currentTimeCounter < lastPlaybackTime)
			{
				if (this._triggerStartTime >= lastPlaybackTime)
				{
					base.active = true;
				}
				if (this._triggerEndTime >= lastPlaybackTime)
				{
					base.transitionInterpValue = 1f;
					base.active = false;
				}
				if (this._triggerStartTime <= currentTimeCounter)
				{
					base.active = true;
				}
				if (this._triggerEndTime <= currentTimeCounter)
				{
					base.transitionInterpValue = 1f;
					base.active = false;
				}
			}
			else
			{
				if (this._triggerStartTime >= lastPlaybackTime && this._triggerStartTime <= currentTimeCounter)
				{
					base.active = true;
				}
				if (this._triggerEndTime >= lastPlaybackTime && this._triggerEndTime <= currentTimeCounter)
				{
					base.transitionInterpValue = 1f;
					base.active = false;
				}
			}
			if (this.triggerEndTime != this.triggerStartTime)
			{
				base.transitionInterpValue = (currentTimeCounter - this.triggerStartTime) / (this.triggerEndTime - this.triggerStartTime);
			}
			else if (base.active)
			{
				base.transitionInterpValue = 0f;
			}
			else
			{
				base.transitionInterpValue = 1f;
			}
		}
	}

	// Token: 0x06006B40 RID: 27456 RVA: 0x00287F89 File Offset: 0x00286389
	[CompilerGenerated]
	private void <InitTriggerUI>m__0(float A_1)
	{
		this.triggerStartTime = this.triggerStartTimeSlider.value;
	}

	// Token: 0x06006B41 RID: 27457 RVA: 0x00287F9C File Offset: 0x0028639C
	[CompilerGenerated]
	private void <InitTriggerUI>m__1(float A_1)
	{
		this.triggerEndTime = this.triggerEndTimeSlider.value;
	}

	// Token: 0x06006B42 RID: 27458 RVA: 0x00287FAF File Offset: 0x002863AF
	[CompilerGenerated]
	private void <InitTriggerUI>m__2(bool b)
	{
		this.selected = b;
	}

	// Token: 0x04005D0E RID: 23822
	public AnimationTimelineTriggerHandler timeLineHandler;

	// Token: 0x04005D0F RID: 23823
	public Button startTimeToCurrentTimeButton;

	// Token: 0x04005D10 RID: 23824
	public Slider triggerStartTimeSlider;

	// Token: 0x04005D11 RID: 23825
	protected float _triggerStartTime;

	// Token: 0x04005D12 RID: 23826
	public Button endTimeToCurrentTimeButton;

	// Token: 0x04005D13 RID: 23827
	public Slider triggerEndTimeSlider;

	// Token: 0x04005D14 RID: 23828
	protected float _triggerEndTime;

	// Token: 0x04005D15 RID: 23829
	public AnimationTimelineTrigger.OnSelected onSelectedHandlers;

	// Token: 0x04005D16 RID: 23830
	public Toggle selectToggle;

	// Token: 0x04005D17 RID: 23831
	public GameObject selectedIndicator;

	// Token: 0x04005D18 RID: 23832
	protected bool _selected;

	// Token: 0x02000D97 RID: 3479
	// (Invoke) Token: 0x06006B44 RID: 27460
	public delegate void OnSelected(AnimationTimelineTrigger att);
}
