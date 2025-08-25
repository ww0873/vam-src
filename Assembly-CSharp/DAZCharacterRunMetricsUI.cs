using System;
using System.Collections.Generic;
using MVR;
using UnityEngine;

// Token: 0x02000AB0 RID: 2736
public class DAZCharacterRunMetricsUI : MonoBehaviour
{
	// Token: 0x060047D8 RID: 18392 RVA: 0x0015FB83 File Offset: 0x0015DF83
	public DAZCharacterRunMetricsUI()
	{
	}

	// Token: 0x060047D9 RID: 18393 RVA: 0x0015FB94 File Offset: 0x0015DF94
	private void Update()
	{
		if (this.dazCharacterRun != null)
		{
			this.frameCount++;
			if (this.frameCount > this.framesForAvg)
			{
				this.frameCount = 0;
				this.FIXED_metric.CalculateAverage();
				this.UPDATE_metric.CalculateAverage();
				this.LATE_metric.CalculateAverage();
				this.THREAD_metric.CalculateAverage();
				this.MAIN_finishTime_metric.CalculateAverage();
				this.MAIN_prepTime_metric.CalculateAverage();
				this.MAIN_fixedThreadWaitTime_metric.CalculateAverage();
				this.MAIN_updateThreadWaitTime_metric.CalculateAverage();
				this.MAIN_autoColliderFinishTime_metric.CalculateAverage();
				this.MAIN_skinPrepTime_metric.CalculateAverage();
				this.MAIN_skinFinishTime_metric.CalculateAverage();
				this.MAIN_skinDrawTime_metric.CalculateAverage();
				this.MAIN_physicsMeshPrepTime_metric.CalculateAverage();
				this.MAIN_physicsMeshFixedUpdateTime_metric.CalculateAverage();
				this.MAIN_physicsMeshFinishTime_metric.CalculateAverage();
				this.MAIN_morphPrepTime_metric.CalculateAverage();
				this.MAIN_morphFinishTime_metric.CalculateAverage();
				this.MAIN_bonePrepTime_metric.CalculateAverage();
				this.MAIN_otherPrepTime_metric.CalculateAverage();
			}
			this.FIXED_metric.Accumulate(this.dazCharacterRun.FIXED_time);
			this.UPDATE_metric.Accumulate(this.dazCharacterRun.UPDATE_time);
			this.LATE_metric.Accumulate(this.dazCharacterRun.LATE_time);
			this.THREAD_metric.Accumulate(this.dazCharacterRun.THREAD_time);
			this.MAIN_finishTime_metric.Accumulate(this.dazCharacterRun.MAIN_finishTime);
			this.MAIN_prepTime_metric.Accumulate(this.dazCharacterRun.MAIN_prepTime);
			this.MAIN_fixedThreadWaitTime_metric.Accumulate(this.dazCharacterRun.MAIN_fixedThreadWaitTime);
			this.MAIN_updateThreadWaitTime_metric.Accumulate(this.dazCharacterRun.MAIN_updateThreadWaitTime);
			this.MAIN_autoColliderFinishTime_metric.Accumulate(this.dazCharacterRun.MAIN_autoColliderFinishTime);
			this.MAIN_skinPrepTime_metric.Accumulate(this.dazCharacterRun.MAIN_skinPrepTime);
			this.MAIN_skinFinishTime_metric.Accumulate(this.dazCharacterRun.MAIN_skinFinishTime);
			this.MAIN_skinDrawTime_metric.Accumulate(this.dazCharacterRun.MAIN_skinDrawTime);
			this.MAIN_physicsMeshPrepTime_metric.Accumulate(this.dazCharacterRun.MAIN_physicsMeshPrepTime);
			this.MAIN_physicsMeshFixedUpdateTime_metric.Accumulate(this.dazCharacterRun.MAIN_physicsMeshFixedUpdateTime);
			this.MAIN_physicsMeshFinishTime_metric.Accumulate(this.dazCharacterRun.MAIN_physicsMeshFinishTime);
			this.MAIN_morphPrepTime_metric.Accumulate(this.dazCharacterRun.MAIN_morphPrepTime);
			this.MAIN_morphFinishTime_metric.Accumulate(this.dazCharacterRun.MAIN_morphFinishTime);
			this.MAIN_bonePrepTime_metric.Accumulate(this.dazCharacterRun.MAIN_bonePrepTime);
			this.MAIN_otherPrepTime_metric.Accumulate(this.dazCharacterRun.MAIN_otherPrepTime);
		}
	}

	// Token: 0x060047DA RID: 18394 RVA: 0x0015FE4C File Offset: 0x0015E24C
	private Metric CreateMetric(string metricName, DAZCharacterRunMetricsUI.Side side = DAZCharacterRunMetricsUI.Side.Left)
	{
		if (!this.metricNameToMetric.ContainsKey(metricName))
		{
			Metric metric = new Metric(metricName, "F2");
			if (this.metricUIPrefab != null && this.leftSideMetricContainer != null && this.rightSideMetricContainer != null)
			{
				Transform transform = UnityEngine.Object.Instantiate<RectTransform>(this.metricUIPrefab);
				if (side == DAZCharacterRunMetricsUI.Side.Left)
				{
					transform.SetParent(this.leftSideMetricContainer, false);
				}
				else
				{
					transform.SetParent(this.rightSideMetricContainer, false);
				}
				MetricUI component = transform.GetComponent<MetricUI>();
				metric.UI = component;
			}
			this.metricNameToMetric.Add(metricName, metric);
			return metric;
		}
		return null;
	}

	// Token: 0x060047DB RID: 18395 RVA: 0x0015FEF8 File Offset: 0x0015E2F8
	private void UpdateMetricValue(string metricName, float value)
	{
		Metric metric;
		if (this.metricNameToMetric.TryGetValue(metricName, out metric))
		{
			metric.Value = value;
		}
	}

	// Token: 0x060047DC RID: 18396 RVA: 0x0015FF20 File Offset: 0x0015E320
	private void UpdateMetricAverageValue(string metricName, float averageValue)
	{
		Metric metric;
		if (this.metricNameToMetric.TryGetValue(metricName, out metric))
		{
			metric.AverageValue = averageValue;
		}
	}

	// Token: 0x060047DD RID: 18397 RVA: 0x0015FF48 File Offset: 0x0015E348
	private void Start()
	{
		this.metricNameToMetric = new Dictionary<string, Metric>();
		this.FIXED_metric = this.CreateMetric("FIXED", DAZCharacterRunMetricsUI.Side.Left);
		this.MAIN_fixedThreadWaitTime_metric = this.CreateMetric("  Thread Wait Fixed", DAZCharacterRunMetricsUI.Side.Left);
		this.MAIN_physicsMeshFixedUpdateTime_metric = this.CreateMetric("  Soft Body Fixed", DAZCharacterRunMetricsUI.Side.Left);
		this.UPDATE_metric = this.CreateMetric("UPDATE", DAZCharacterRunMetricsUI.Side.Left);
		this.MAIN_updateThreadWaitTime_metric = this.CreateMetric("  Thread Wait Update", DAZCharacterRunMetricsUI.Side.Left);
		this.MAIN_finishTime_metric = this.CreateMetric("  Finish Run", DAZCharacterRunMetricsUI.Side.Left);
		this.MAIN_morphFinishTime_metric = this.CreateMetric("    Morph Finish", DAZCharacterRunMetricsUI.Side.Left);
		this.MAIN_autoColliderFinishTime_metric = this.CreateMetric("    Skin Collider Finish", DAZCharacterRunMetricsUI.Side.Left);
		this.MAIN_physicsMeshFinishTime_metric = this.CreateMetric("    Soft Body Finish", DAZCharacterRunMetricsUI.Side.Left);
		this.MAIN_skinFinishTime_metric = this.CreateMetric("    Skin Finish", DAZCharacterRunMetricsUI.Side.Left);
		this.MAIN_prepTime_metric = this.CreateMetric("  Prep Next Run", DAZCharacterRunMetricsUI.Side.Left);
		this.MAIN_skinPrepTime_metric = this.CreateMetric("    Skin Prep", DAZCharacterRunMetricsUI.Side.Left);
		this.MAIN_physicsMeshPrepTime_metric = this.CreateMetric("    Soft Body Prep", DAZCharacterRunMetricsUI.Side.Left);
		this.MAIN_morphPrepTime_metric = this.CreateMetric("    Morph Prep", DAZCharacterRunMetricsUI.Side.Left);
		this.MAIN_bonePrepTime_metric = this.CreateMetric("    Bone Prep", DAZCharacterRunMetricsUI.Side.Left);
		this.MAIN_otherPrepTime_metric = this.CreateMetric("    Other Prep", DAZCharacterRunMetricsUI.Side.Left);
		this.LATE_metric = this.CreateMetric("LATE", DAZCharacterRunMetricsUI.Side.Left);
		this.MAIN_skinDrawTime_metric = this.CreateMetric("  Skin Draw", DAZCharacterRunMetricsUI.Side.Left);
		this.THREAD_metric = this.CreateMetric("THREAD", DAZCharacterRunMetricsUI.Side.Right);
	}

	// Token: 0x04003581 RID: 13697
	public DAZCharacterRun dazCharacterRun;

	// Token: 0x04003582 RID: 13698
	public RectTransform leftSideMetricContainer;

	// Token: 0x04003583 RID: 13699
	public RectTransform rightSideMetricContainer;

	// Token: 0x04003584 RID: 13700
	public RectTransform metricUIPrefab;

	// Token: 0x04003585 RID: 13701
	protected Metric FIXED_metric;

	// Token: 0x04003586 RID: 13702
	protected Metric UPDATE_metric;

	// Token: 0x04003587 RID: 13703
	protected Metric LATE_metric;

	// Token: 0x04003588 RID: 13704
	protected Metric THREAD_metric;

	// Token: 0x04003589 RID: 13705
	protected Metric MAIN_finishTime_metric;

	// Token: 0x0400358A RID: 13706
	protected Metric MAIN_prepTime_metric;

	// Token: 0x0400358B RID: 13707
	protected Metric MAIN_fixedThreadWaitTime_metric;

	// Token: 0x0400358C RID: 13708
	protected Metric MAIN_updateThreadWaitTime_metric;

	// Token: 0x0400358D RID: 13709
	protected Metric MAIN_autoColliderFinishTime_metric;

	// Token: 0x0400358E RID: 13710
	protected Metric MAIN_skinPrepTime_metric;

	// Token: 0x0400358F RID: 13711
	protected Metric MAIN_skinFinishTime_metric;

	// Token: 0x04003590 RID: 13712
	protected Metric MAIN_skinDrawTime_metric;

	// Token: 0x04003591 RID: 13713
	protected Metric MAIN_physicsMeshPrepTime_metric;

	// Token: 0x04003592 RID: 13714
	protected Metric MAIN_physicsMeshFixedUpdateTime_metric;

	// Token: 0x04003593 RID: 13715
	protected Metric MAIN_physicsMeshFinishTime_metric;

	// Token: 0x04003594 RID: 13716
	protected Metric MAIN_morphPrepTime_metric;

	// Token: 0x04003595 RID: 13717
	protected Metric MAIN_morphFinishTime_metric;

	// Token: 0x04003596 RID: 13718
	protected Metric MAIN_bonePrepTime_metric;

	// Token: 0x04003597 RID: 13719
	protected Metric MAIN_otherPrepTime_metric;

	// Token: 0x04003598 RID: 13720
	private int frameCount;

	// Token: 0x04003599 RID: 13721
	private int framesForAvg = 50;

	// Token: 0x0400359A RID: 13722
	private Dictionary<string, Metric> metricNameToMetric;

	// Token: 0x02000AB1 RID: 2737
	private enum Side
	{
		// Token: 0x0400359C RID: 13724
		Left,
		// Token: 0x0400359D RID: 13725
		Right
	}
}
