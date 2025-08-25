using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200035B RID: 859
public class ProgressBar : MonoBehaviour
{
	// Token: 0x0600152C RID: 5420 RVA: 0x00078564 File Offset: 0x00076964
	public ProgressBar()
	{
	}

	// Token: 0x17000255 RID: 597
	// (get) Token: 0x0600152D RID: 5421 RVA: 0x0007856C File Offset: 0x0007696C
	// (set) Token: 0x0600152E RID: 5422 RVA: 0x00078574 File Offset: 0x00076974
	public float Progress
	{
		get
		{
			return this._currentProgress;
		}
		set
		{
			this._currentProgress = value;
		}
	}

	// Token: 0x17000256 RID: 598
	// (get) Token: 0x0600152F RID: 5423 RVA: 0x0007857D File Offset: 0x0007697D
	// (set) Token: 0x06001530 RID: 5424 RVA: 0x00078585 File Offset: 0x00076985
	public float Maximum
	{
		get
		{
			return this._maximumValue;
		}
		set
		{
			this._maximumValue = value;
		}
	}

	// Token: 0x17000257 RID: 599
	// (get) Token: 0x06001531 RID: 5425 RVA: 0x0007858E File Offset: 0x0007698E
	// (set) Token: 0x06001532 RID: 5426 RVA: 0x00078596 File Offset: 0x00076996
	public float Minimum
	{
		get
		{
			return this._minimumValue;
		}
		set
		{
			this._minimumValue = value;
		}
	}

	// Token: 0x17000258 RID: 600
	// (get) Token: 0x06001533 RID: 5427 RVA: 0x0007859F File Offset: 0x0007699F
	// (set) Token: 0x06001534 RID: 5428 RVA: 0x000785A7 File Offset: 0x000769A7
	public float Step
	{
		get
		{
			return this._stepValue;
		}
		set
		{
			this._stepValue = value;
		}
	}

	// Token: 0x06001535 RID: 5429 RVA: 0x000785B0 File Offset: 0x000769B0
	private void Start()
	{
		this._currentProgress = 0f;
		this._maximumValue = 1f;
		this._minimumValue = 0f;
		this._stepValue = 0.1f;
		this._localScale = new Vector3(this._currentProgress / this._maximumValue + this._minimumValue, this._rect.localScale.y, this._rect.localScale.z);
		this.UpdateProgressBar();
	}

	// Token: 0x06001536 RID: 5430 RVA: 0x00078634 File Offset: 0x00076A34
	private void OnGUI()
	{
	}

	// Token: 0x06001537 RID: 5431 RVA: 0x00078638 File Offset: 0x00076A38
	private void UpdateProgressBar()
	{
		this._localScale.x = this._currentProgress / this._maximumValue + this._minimumValue;
		this._rect.localScale = this._localScale;
		if (this._currentProgress >= 1f)
		{
			this._localScale.x = 1f;
			this._rect.localScale = this._localScale;
		}
	}

	// Token: 0x06001538 RID: 5432 RVA: 0x000786A6 File Offset: 0x00076AA6
	public void PerformStep()
	{
		this._currentProgress += this._stepValue;
		this.UpdateProgressBar();
	}

	// Token: 0x06001539 RID: 5433 RVA: 0x000786C1 File Offset: 0x00076AC1
	public void Clear()
	{
		this._currentProgress = 0f;
		this._maximumValue = 1f;
		this._minimumValue = 0f;
		this._stepValue = 0.1f;
	}

	// Token: 0x0600153A RID: 5434 RVA: 0x000786EF File Offset: 0x00076AEF
	public void SetProgressText(string text)
	{
		this._progressText.text = text;
	}

	// Token: 0x040011C4 RID: 4548
	public RectTransform _rect;

	// Token: 0x040011C5 RID: 4549
	public Text _progressText;

	// Token: 0x040011C6 RID: 4550
	private float _currentProgress;

	// Token: 0x040011C7 RID: 4551
	private float _maximumValue;

	// Token: 0x040011C8 RID: 4552
	private float _minimumValue;

	// Token: 0x040011C9 RID: 4553
	private float _stepValue;

	// Token: 0x040011CA RID: 4554
	private Vector3 _localScale;
}
