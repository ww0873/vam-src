using System;
using SimpleJSON;

// Token: 0x02000B6C RID: 2924
public class MotionAnimator : JSONStorable
{
	// Token: 0x06005206 RID: 20998 RVA: 0x001DA704 File Offset: 0x001D8B04
	public MotionAnimator()
	{
	}

	// Token: 0x06005207 RID: 20999 RVA: 0x001DA714 File Offset: 0x001D8B14
	public override JSONClass GetJSON(bool includePhysical = true, bool includeAppearance = true, bool forceStore = false)
	{
		JSONClass json = base.GetJSON(includePhysical, includeAppearance, forceStore);
		if (includePhysical)
		{
		}
		return json;
	}

	// Token: 0x06005208 RID: 21000 RVA: 0x001DA732 File Offset: 0x001D8B32
	public override void RestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, JSONArray presetAtoms = null, bool setMissingToDefault = true)
	{
		base.RestoreFromJSON(jc, restorePhysical, restoreAppearance, presetAtoms, setMissingToDefault);
		if (restorePhysical)
		{
		}
	}

	// Token: 0x06005209 RID: 21001 RVA: 0x001DA747 File Offset: 0x001D8B47
	public void SaveSequence()
	{
	}

	// Token: 0x0600520A RID: 21002 RVA: 0x001DA749 File Offset: 0x001D8B49
	public void LoadSequence()
	{
	}

	// Token: 0x0600520B RID: 21003 RVA: 0x001DA74B File Offset: 0x001D8B4B
	public void SavePattern()
	{
	}

	// Token: 0x0600520C RID: 21004 RVA: 0x001DA74D File Offset: 0x001D8B4D
	public void LoadPattern()
	{
	}

	// Token: 0x0600520D RID: 21005 RVA: 0x001DA750 File Offset: 0x001D8B50
	public void ClearAllAnimation()
	{
		foreach (MotionAnimationControl motionAnimationControl in this.controllers)
		{
			motionAnimationControl.ClearAnimation();
		}
		this._isPlaying = false;
	}

	// Token: 0x0600520E RID: 21006 RVA: 0x001DA78C File Offset: 0x001D8B8C
	public void StartRecord()
	{
		this._recordCounter = 0;
		this._isRecording = true;
		foreach (MotionAnimationControl motionAnimationControl in this.controllers)
		{
			motionAnimationControl.PrepareRecord(this._recordCounter);
		}
	}

	// Token: 0x0600520F RID: 21007 RVA: 0x001DA7D4 File Offset: 0x001D8BD4
	public void FinishRecord()
	{
		this._isRecording = false;
		if (this._recordCounter > this._patternLength)
		{
			this._patternLength = this._recordCounter;
		}
		foreach (MotionAnimationControl motionAnimationControl in this.controllers)
		{
			motionAnimationControl.FinalizeRecord();
		}
		this.StartPlayback();
	}

	// Token: 0x06005210 RID: 21008 RVA: 0x001DA830 File Offset: 0x001D8C30
	public void StartPlayback()
	{
		this._playbackCounter = 0;
		this._isPlaying = true;
	}

	// Token: 0x06005211 RID: 21009 RVA: 0x001DA840 File Offset: 0x001D8C40
	protected void RecordStep()
	{
		foreach (MotionAnimationControl motionAnimationControl in this.controllers)
		{
			motionAnimationControl.RecordStep((float)this._recordCounter, false);
		}
	}

	// Token: 0x06005212 RID: 21010 RVA: 0x001DA87C File Offset: 0x001D8C7C
	protected void PlaybackStep()
	{
		foreach (MotionAnimationControl motionAnimationControl in this.controllers)
		{
			motionAnimationControl.PlaybackStep((float)this._playbackCounter);
		}
	}

	// Token: 0x06005213 RID: 21011 RVA: 0x001DA8B8 File Offset: 0x001D8CB8
	private void Update()
	{
		if (this._isPlaying)
		{
			this._playbackCounter++;
			if (this._playbackCounter > this._patternLength)
			{
				this._playbackCounter = 0;
			}
			this.PlaybackStep();
		}
		else if (this._isRecording)
		{
			this._recordCounter++;
			if (this._recordCounter % this.recordInterval == 0)
			{
				this.RecordStep();
			}
		}
	}

	// Token: 0x040041CA RID: 16842
	public MotionAnimationControl[] controllers;

	// Token: 0x040041CB RID: 16843
	public int recordInterval = 3;

	// Token: 0x040041CC RID: 16844
	protected bool _isRecording;

	// Token: 0x040041CD RID: 16845
	protected bool _isPlaying;

	// Token: 0x040041CE RID: 16846
	protected int _recordCounter;

	// Token: 0x040041CF RID: 16847
	protected int _patternLength;

	// Token: 0x040041D0 RID: 16848
	protected int _playbackCounter;
}
