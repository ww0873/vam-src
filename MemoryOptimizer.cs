using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AssetBundles;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000C2A RID: 3114
public class MemoryOptimizer : MonoBehaviour
{
	// Token: 0x06005A86 RID: 23174 RVA: 0x002138C5 File Offset: 0x00211CC5
	public MemoryOptimizer()
	{
	}

	// Token: 0x06005A87 RID: 23175 RVA: 0x002138D0 File Offset: 0x00211CD0
	public IEnumerator OptimizeMemoryUsage()
	{
		yield return null;
		if (this.memoryOptimizerCallbacks != null)
		{
			this.memoryOptimizerCallbacks();
		}
		yield return null;
		yield return null;
		yield return Resources.UnloadUnusedAssets();
		GC.Collect();
		yield break;
	}

	// Token: 0x06005A88 RID: 23176 RVA: 0x002138EC File Offset: 0x00211CEC
	protected IEnumerator OptimizeMemoryUsageCo()
	{
		yield return base.StartCoroutine(this.OptimizeMemoryUsage());
		yield return null;
		this.optimizeMemoryUsageCoroutine = null;
		if (this.onMemoryOptimizeComplete != null)
		{
			this.onMemoryOptimizeComplete();
		}
		this.onMemoryOptimizeComplete = this.nextOnMemoryOptimizeComplete;
		if (this.optimizingIndicator != null)
		{
			this.optimizingIndicator.SetActive(false);
		}
		if (this.reportText != null)
		{
			this.reportText.text = "Optimization Complete";
		}
		yield break;
	}

	// Token: 0x06005A89 RID: 23177 RVA: 0x00213908 File Offset: 0x00211D08
	public void TriggerOptimize(MemoryOptimizer.OnMemoryOptimizeComplete onComplete)
	{
		this.optimizeTrigger = true;
		if (onComplete != null)
		{
			if (this.optimizeMemoryUsageCoroutine == null)
			{
				this.onMemoryOptimizeComplete = (MemoryOptimizer.OnMemoryOptimizeComplete)Delegate.Combine(this.onMemoryOptimizeComplete, onComplete);
			}
			else
			{
				this.nextOnMemoryOptimizeComplete = (MemoryOptimizer.OnMemoryOptimizeComplete)Delegate.Combine(this.nextOnMemoryOptimizeComplete, onComplete);
			}
		}
	}

	// Token: 0x06005A8A RID: 23178 RVA: 0x00213960 File Offset: 0x00211D60
	public void TriggerOptimize()
	{
		this.TriggerOptimize(null);
	}

	// Token: 0x06005A8B RID: 23179 RVA: 0x0021396C File Offset: 0x00211D6C
	protected void HandleOptimizeTrigger()
	{
		if (this.optimizeMemoryUsageCoroutine == null && this.optimizeTrigger)
		{
			this.optimizeTrigger = false;
			if (this.optimizingIndicator != null)
			{
				this.optimizingIndicator.SetActive(true);
			}
			if (this.reportText != null)
			{
				this.reportText.text = "Optimizing...";
			}
			base.StartCoroutine(this.OptimizeMemoryUsageCo());
		}
	}

	// Token: 0x06005A8C RID: 23180 RVA: 0x002139E1 File Offset: 0x00211DE1
	public void ClearAtomPool()
	{
		SuperController.singleton.ClearAtomPool();
	}

	// Token: 0x06005A8D RID: 23181 RVA: 0x002139ED File Offset: 0x00211DED
	public void RegisterMemoryOptimizerListener(MemoryOptimizer.MemoryOptimizerCallback memoryOptimizerCallback)
	{
		this.memoryOptimizerCallbacks = (MemoryOptimizer.MemoryOptimizerCallback)Delegate.Combine(this.memoryOptimizerCallbacks, memoryOptimizerCallback);
	}

	// Token: 0x06005A8E RID: 23182 RVA: 0x00213A06 File Offset: 0x00211E06
	public void DeregisterMemoryOptimizerListener(MemoryOptimizer.MemoryOptimizerCallback memoryOptimizerCallback)
	{
		this.memoryOptimizerCallbacks = (MemoryOptimizer.MemoryOptimizerCallback)Delegate.Remove(this.memoryOptimizerCallbacks, memoryOptimizerCallback);
	}

	// Token: 0x06005A8F RID: 23183 RVA: 0x00213A20 File Offset: 0x00211E20
	public void ReportMemoryUsage()
	{
		if (this.memoryReports == null)
		{
			this.memoryReports = new List<string>();
		}
		else
		{
			this.memoryReports.Clear();
		}
		if (this.memoryOptimizerReporters != null)
		{
			this.memoryOptimizerReporters(this.memoryReports);
		}
		if (this.reportText != null)
		{
			this.reportText.text = string.Empty;
		}
		foreach (string str in this.memoryReports)
		{
			if (this.reportText != null)
			{
				Text text = this.reportText;
				text.text = text.text + str + "\n";
			}
		}
	}

	// Token: 0x06005A90 RID: 23184 RVA: 0x00213B08 File Offset: 0x00211F08
	public void RegisterMemoryOptimizerReporter(MemoryOptimizer.MemoryOptimizerReporter memoryOptimizerReporter)
	{
		this.memoryOptimizerReporters = (MemoryOptimizer.MemoryOptimizerReporter)Delegate.Combine(this.memoryOptimizerReporters, memoryOptimizerReporter);
	}

	// Token: 0x06005A91 RID: 23185 RVA: 0x00213B21 File Offset: 0x00211F21
	public void DeregisterMemoryOptimizerReporter(MemoryOptimizer.MemoryOptimizerReporter memoryOptimizerReporter)
	{
		this.memoryOptimizerReporters = (MemoryOptimizer.MemoryOptimizerReporter)Delegate.Remove(this.memoryOptimizerReporters, memoryOptimizerReporter);
	}

	// Token: 0x17000D58 RID: 3416
	// (get) Token: 0x06005A92 RID: 23186 RVA: 0x00213B3A File Offset: 0x00211F3A
	public float WorkingSetSizeInGB
	{
		get
		{
			return this.memUsage.WorkingSetSize * MemoryOptimizer.gByteMult;
		}
	}

	// Token: 0x17000D59 RID: 3417
	// (get) Token: 0x06005A93 RID: 23187 RVA: 0x00213B50 File Offset: 0x00211F50
	public string WorkingSetSizeInGBText
	{
		get
		{
			return (this.memUsage.WorkingSetSize * MemoryOptimizer.gByteMult).ToString("F2") + " GB";
		}
	}

	// Token: 0x17000D5A RID: 3418
	// (get) Token: 0x06005A94 RID: 23188 RVA: 0x00213B85 File Offset: 0x00211F85
	public float PeakWorkingSetSizeInGB
	{
		get
		{
			return this.memUsage.PeakWorkingSetSize * MemoryOptimizer.gByteMult;
		}
	}

	// Token: 0x17000D5B RID: 3419
	// (get) Token: 0x06005A95 RID: 23189 RVA: 0x00213B98 File Offset: 0x00211F98
	public string PeakWorkingSetSizeInGBText
	{
		get
		{
			return (this.memUsage.PeakWorkingSetSize * MemoryOptimizer.gByteMult).ToString("F2") + " GB";
		}
	}

	// Token: 0x17000D5C RID: 3420
	// (get) Token: 0x06005A96 RID: 23190 RVA: 0x00213BCD File Offset: 0x00211FCD
	public float PageFileUsageInGB
	{
		get
		{
			return this.memUsage.PageFileUsage * MemoryOptimizer.gByteMult;
		}
	}

	// Token: 0x17000D5D RID: 3421
	// (get) Token: 0x06005A97 RID: 23191 RVA: 0x00213BE0 File Offset: 0x00211FE0
	public string PageFileUsageInGBText
	{
		get
		{
			return (this.memUsage.PageFileUsage * MemoryOptimizer.gByteMult).ToString("F2") + " GB";
		}
	}

	// Token: 0x17000D5E RID: 3422
	// (get) Token: 0x06005A98 RID: 23192 RVA: 0x00213C15 File Offset: 0x00212015
	public float PeakPageFileUsageInGB
	{
		get
		{
			return this.memUsage.PeakPageFileUsage * MemoryOptimizer.gByteMult;
		}
	}

	// Token: 0x17000D5F RID: 3423
	// (get) Token: 0x06005A99 RID: 23193 RVA: 0x00213C28 File Offset: 0x00212028
	public string PeakPageFileUsageInGBText
	{
		get
		{
			return (this.memUsage.PeakPageFileUsage * MemoryOptimizer.gByteMult).ToString("F2") + " GB";
		}
	}

	// Token: 0x17000D60 RID: 3424
	// (get) Token: 0x06005A9A RID: 23194 RVA: 0x00213C5D File Offset: 0x0021205D
	public float HeapSizeInGB
	{
		get
		{
			return (float)GC.GetTotalMemory(false) * MemoryOptimizer.gByteMult;
		}
	}

	// Token: 0x17000D61 RID: 3425
	// (get) Token: 0x06005A9B RID: 23195 RVA: 0x00213C6C File Offset: 0x0021206C
	public string HeapSizeInGBText
	{
		get
		{
			return ((float)GC.GetTotalMemory(false) * MemoryOptimizer.gByteMult).ToString("F2") + " GB";
		}
	}

	// Token: 0x17000D62 RID: 3426
	// (get) Token: 0x06005A9C RID: 23196 RVA: 0x00213C9D File Offset: 0x0021209D
	public float SystemMemorySizeInGB
	{
		get
		{
			return this.systemMemorySize;
		}
	}

	// Token: 0x17000D63 RID: 3427
	// (get) Token: 0x06005A9D RID: 23197 RVA: 0x00213CA5 File Offset: 0x002120A5
	public string SystemMemorySizeInGBText
	{
		get
		{
			return this.systemMemorySize.ToString("F2") + " GB";
		}
	}

	// Token: 0x06005A9E RID: 23198
	[DllImport("MemoryReporter")]
	protected static extern MemoryOptimizer.MemUsage GetMemoryUsage();

	// Token: 0x06005A9F RID: 23199 RVA: 0x00213CC4 File Offset: 0x002120C4
	protected void Awake()
	{
		MemoryOptimizer.singleton = this;
		this.systemMemorySize = (float)SystemInfo.systemMemorySize / 1024f;
		if (this.reportButton != null)
		{
			this.reportButton.onClick.AddListener(new UnityAction(this.ReportMemoryUsage));
		}
		if (this.optimizeButton != null)
		{
			this.optimizeButton.onClick.AddListener(new UnityAction(this.TriggerOptimize));
		}
		if (this.clearAtomPoolButton != null)
		{
			this.clearAtomPoolButton.onClick.AddListener(new UnityAction(this.ClearAtomPool));
		}
	}

	// Token: 0x17000D64 RID: 3428
	// (get) Token: 0x06005AA0 RID: 23200 RVA: 0x00213D70 File Offset: 0x00212170
	public static float GByteMult
	{
		get
		{
			return MemoryOptimizer.gByteMult;
		}
	}

	// Token: 0x06005AA1 RID: 23201 RVA: 0x00213D78 File Offset: 0x00212178
	protected void Update()
	{
		this.memUsage = MemoryOptimizer.GetMemoryUsage();
		this.HandleOptimizeTrigger();
		if (this.memoryOptimizerUI != null && this.memoryOptimizerUI.activeInHierarchy)
		{
			if (this.systemMemorySizeText != null)
			{
				this.systemMemorySizeText.text = this.SystemMemorySizeInGBText;
			}
			if (this.workingSetSizeText != null)
			{
				this.workingSetSizeText.text = this.WorkingSetSizeInGBText;
			}
			if (this.peakWorkingSetSizeText != null)
			{
				this.peakWorkingSetSizeText.text = this.PeakWorkingSetSizeInGBText;
			}
			if (this.pageFileUsageText != null)
			{
				this.pageFileUsageText.text = this.PageFileUsageInGBText;
			}
			if (this.peakPageFileUsageText != null)
			{
				this.peakPageFileUsageText.text = this.PeakPageFileUsageInGBText;
			}
			if (this.heapUsageText != null)
			{
				this.heapUsageText.text = this.HeapSizeInGBText;
			}
			if (this.loadedBundlesText != null)
			{
				this.loadedBundlesText.text = AssetBundleManager.GetNumberOfLoadedAssetBundles().ToString();
			}
		}
	}

	// Token: 0x06005AA2 RID: 23202 RVA: 0x00213EB2 File Offset: 0x002122B2
	// Note: this type is marked as 'beforefieldinit'.
	static MemoryOptimizer()
	{
	}

	// Token: 0x04004AC4 RID: 19140
	public static MemoryOptimizer singleton;

	// Token: 0x04004AC5 RID: 19141
	protected bool optimizeTrigger;

	// Token: 0x04004AC6 RID: 19142
	protected Coroutine optimizeMemoryUsageCoroutine;

	// Token: 0x04004AC7 RID: 19143
	protected MemoryOptimizer.OnMemoryOptimizeComplete onMemoryOptimizeComplete;

	// Token: 0x04004AC8 RID: 19144
	protected MemoryOptimizer.OnMemoryOptimizeComplete nextOnMemoryOptimizeComplete;

	// Token: 0x04004AC9 RID: 19145
	protected MemoryOptimizer.MemoryOptimizerCallback memoryOptimizerCallbacks;

	// Token: 0x04004ACA RID: 19146
	protected List<string> memoryReports;

	// Token: 0x04004ACB RID: 19147
	protected MemoryOptimizer.MemoryOptimizerReporter memoryOptimizerReporters;

	// Token: 0x04004ACC RID: 19148
	public GameObject memoryOptimizerUI;

	// Token: 0x04004ACD RID: 19149
	public Text reportText;

	// Token: 0x04004ACE RID: 19150
	public Text systemMemorySizeText;

	// Token: 0x04004ACF RID: 19151
	public Text workingSetSizeText;

	// Token: 0x04004AD0 RID: 19152
	public Text peakWorkingSetSizeText;

	// Token: 0x04004AD1 RID: 19153
	public Text pageFileUsageText;

	// Token: 0x04004AD2 RID: 19154
	public Text peakPageFileUsageText;

	// Token: 0x04004AD3 RID: 19155
	public Text heapUsageText;

	// Token: 0x04004AD4 RID: 19156
	public Text loadedBundlesText;

	// Token: 0x04004AD5 RID: 19157
	public Button reportButton;

	// Token: 0x04004AD6 RID: 19158
	public Button optimizeButton;

	// Token: 0x04004AD7 RID: 19159
	public Button clearAtomPoolButton;

	// Token: 0x04004AD8 RID: 19160
	public GameObject optimizingIndicator;

	// Token: 0x04004AD9 RID: 19161
	protected MemoryOptimizer.MemUsage memUsage;

	// Token: 0x04004ADA RID: 19162
	protected float systemMemorySize;

	// Token: 0x04004ADB RID: 19163
	protected static float gByteMult = 9.313226E-10f;

	// Token: 0x02000C2B RID: 3115
	// (Invoke) Token: 0x06005AA4 RID: 23204
	public delegate void OnMemoryOptimizeComplete();

	// Token: 0x02000C2C RID: 3116
	// (Invoke) Token: 0x06005AA8 RID: 23208
	public delegate void MemoryOptimizerCallback();

	// Token: 0x02000C2D RID: 3117
	// (Invoke) Token: 0x06005AAC RID: 23212
	public delegate void MemoryOptimizerReporter(List<string> reportList);

	// Token: 0x02000C2E RID: 3118
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	protected struct MemUsage
	{
		// Token: 0x04004ADC RID: 19164
		public float WorkingSetSize;

		// Token: 0x04004ADD RID: 19165
		public float PeakWorkingSetSize;

		// Token: 0x04004ADE RID: 19166
		public float PageFileUsage;

		// Token: 0x04004ADF RID: 19167
		public float PeakPageFileUsage;
	}

	// Token: 0x02001001 RID: 4097
	[CompilerGenerated]
	private sealed class <OptimizeMemoryUsage>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x0600766F RID: 30319 RVA: 0x00213EBE File Offset: 0x002122BE
		[DebuggerHidden]
		public <OptimizeMemoryUsage>c__Iterator0()
		{
		}

		// Token: 0x06007670 RID: 30320 RVA: 0x00213EC8 File Offset: 0x002122C8
		public bool MoveNext()
		{
			uint num = (uint)this.$PC;
			this.$PC = -1;
			switch (num)
			{
			case 0U:
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
			case 1U:
				if (this.memoryOptimizerCallbacks != null)
				{
					this.memoryOptimizerCallbacks();
				}
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 2;
				}
				return true;
			case 2U:
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 3;
				}
				return true;
			case 3U:
				this.$current = Resources.UnloadUnusedAssets();
				if (!this.$disposing)
				{
					this.$PC = 4;
				}
				return true;
			case 4U:
				GC.Collect();
				this.$PC = -1;
				break;
			}
			return false;
		}

		// Token: 0x17001183 RID: 4483
		// (get) Token: 0x06007671 RID: 30321 RVA: 0x00213FA1 File Offset: 0x002123A1
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x17001184 RID: 4484
		// (get) Token: 0x06007672 RID: 30322 RVA: 0x00213FA9 File Offset: 0x002123A9
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x06007673 RID: 30323 RVA: 0x00213FB1 File Offset: 0x002123B1
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x06007674 RID: 30324 RVA: 0x00213FC1 File Offset: 0x002123C1
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x04006A46 RID: 27206
		internal MemoryOptimizer $this;

		// Token: 0x04006A47 RID: 27207
		internal object $current;

		// Token: 0x04006A48 RID: 27208
		internal bool $disposing;

		// Token: 0x04006A49 RID: 27209
		internal int $PC;
	}

	// Token: 0x02001002 RID: 4098
	[CompilerGenerated]
	private sealed class <OptimizeMemoryUsageCo>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x06007675 RID: 30325 RVA: 0x00213FC8 File Offset: 0x002123C8
		[DebuggerHidden]
		public <OptimizeMemoryUsageCo>c__Iterator1()
		{
		}

		// Token: 0x06007676 RID: 30326 RVA: 0x00213FD0 File Offset: 0x002123D0
		public bool MoveNext()
		{
			uint num = (uint)this.$PC;
			this.$PC = -1;
			switch (num)
			{
			case 0U:
				this.$current = base.StartCoroutine(base.OptimizeMemoryUsage());
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
			case 1U:
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 2;
				}
				return true;
			case 2U:
				this.optimizeMemoryUsageCoroutine = null;
				if (this.onMemoryOptimizeComplete != null)
				{
					this.onMemoryOptimizeComplete();
				}
				this.onMemoryOptimizeComplete = this.nextOnMemoryOptimizeComplete;
				if (this.optimizingIndicator != null)
				{
					this.optimizingIndicator.SetActive(false);
				}
				if (this.reportText != null)
				{
					this.reportText.text = "Optimization Complete";
				}
				this.$PC = -1;
				break;
			}
			return false;
		}

		// Token: 0x17001185 RID: 4485
		// (get) Token: 0x06007677 RID: 30327 RVA: 0x002140EB File Offset: 0x002124EB
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x17001186 RID: 4486
		// (get) Token: 0x06007678 RID: 30328 RVA: 0x002140F3 File Offset: 0x002124F3
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x06007679 RID: 30329 RVA: 0x002140FB File Offset: 0x002124FB
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x0600767A RID: 30330 RVA: 0x0021410B File Offset: 0x0021250B
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x04006A4A RID: 27210
		internal MemoryOptimizer $this;

		// Token: 0x04006A4B RID: 27211
		internal object $current;

		// Token: 0x04006A4C RID: 27212
		internal bool $disposing;

		// Token: 0x04006A4D RID: 27213
		internal int $PC;
	}
}
