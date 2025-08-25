using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MeshVR
{
	// Token: 0x02000E16 RID: 3606
	public class PerfMon : MonoBehaviour
	{
		// Token: 0x06006F17 RID: 28439 RVA: 0x0029AFDF File Offset: 0x002993DF
		public PerfMon()
		{
		}

		// Token: 0x06006F18 RID: 28440 RVA: 0x0029B001 File Offset: 0x00299401
		public static void ReportWaitTime(float t)
		{
			PerfMon.waitTime += t;
		}

		// Token: 0x1700103D RID: 4157
		// (get) Token: 0x06006F19 RID: 28441 RVA: 0x0029B00F File Offset: 0x0029940F
		// (set) Token: 0x06006F1A RID: 28442 RVA: 0x0029B018 File Offset: 0x00299418
		public bool on
		{
			get
			{
				return this._on;
			}
			set
			{
				if (this._on != value)
				{
					this._on = value;
					if (this.perfMonUI != null)
					{
						this.perfMonUI.gameObject.SetActive(this._on);
					}
					if (this.perfMonUIAlt != null)
					{
						this.perfMonUIAlt.gameObject.SetActive(this._on);
					}
					if (this.onToggle != null)
					{
						this.onToggle.isOn = this._on;
					}
				}
			}
		}

		// Token: 0x06006F1B RID: 28443 RVA: 0x0029B0A8 File Offset: 0x002994A8
		public void RestartAverageCalc()
		{
			this.avgCalcStartFrame = this._totFrames;
			this._totPhysicsTime = 0f;
			this._totIntenalPhysicsTime = 0f;
			this._totScriptsTime = 0f;
			this._totTotalTime = 0f;
			this._totRenderTime = 0f;
			this._totFpsTime = 0f;
			this._totWaitTime = 0f;
		}

		// Token: 0x06006F1C RID: 28444 RVA: 0x0029B110 File Offset: 0x00299510
		protected void DoUpdate()
		{
			this._frameStopTime = GlobalStopwatch.GetElapsedMilliseconds();
			float num = this._frameStopTime - this._lastFrameTime;
			this._fpsAccumTime += 1000f / num;
			this._lastFrameTime = this._frameStopTime;
			this._fpsFrames++;
			if (PerfMonCamera.wasSet)
			{
				this._renderTime = this._frameStopTime - PerfMonCamera.renderStartTime;
			}
			else
			{
				this._renderTime = 0f;
			}
			this._totalTime = this._preRenderTime + this._renderTime;
			PerfMon.physicsTime = this._physicsTime;
			PerfMon.scriptsTime = this._scriptsTime;
			PerfMon.preRenderTime = this._preRenderTime;
			PerfMon.renderTime = this._renderTime;
			PerfMon.totalTime = this._totalTime;
			if (SuperController.singleton != null && !SuperController.singleton.isLoading && !SuperController.singleton.IsSimulationResetting() && !GlobalSceneOptions.IsLoading)
			{
				this._totFrames++;
				int num2 = this.avgCalcStartFrame + this.avgCalcNumFrames;
				if (this._totFrames > this.avgCalcStartFrame && this._totFrames <= num2)
				{
					this._totFpsTime += 1000f / num;
					this._totPhysicsTime += this._physicsTime;
					this._totIntenalPhysicsTime += this._internalPhysicsTime;
					this._totScriptsTime += this._scriptsTime;
					this._totTotalTime += this._totalTime;
					this._totRenderTime += this._renderTime;
					this._totWaitTime += PerfMon.waitTime;
					float num3 = 1f / (float)(this._totFrames - this.avgCalcStartFrame);
					this._avgPhysicsTime = this._totPhysicsTime * num3;
					this._avgInternalPhysicsTime = this._totIntenalPhysicsTime * num3;
					this._avgScriptsTime = this._totScriptsTime * num3;
					this._avgRenderTime = this._totRenderTime * num3;
					this._avgWaitTime = this._totWaitTime * num3;
					this._avgTotalTime = this._totTotalTime * num3;
					this._avgFps = this._totFpsTime * num3;
					if (this._totFrames == num2)
					{
						UnityEngine.Debug.Log(string.Concat(new string[]
						{
							"Benchmark complete. Avg. tot time: ",
							this._avgTotalTime.ToString("F2"),
							" Avg. physics time: ",
							this._avgPhysicsTime.ToString("F2"),
							" Avg. internal physics time: ",
							this._avgInternalPhysicsTime.ToString("F2"),
							" Avg. scripts time: ",
							this._avgScriptsTime.ToString("F2"),
							" Avg. render time: ",
							this._avgRenderTime.ToString("F2"),
							" Avg. wait time: ",
							this._avgWaitTime.ToString("F2"),
							" Avg. FPS: ",
							this._avgFps.ToString("F2")
						}));
					}
				}
			}
			if (this.cnt == 0)
			{
				this.fps = (this._fpsAccumTime / (float)this._fpsFrames).ToString("F2");
				if (this.fpsText)
				{
					this.fpsText.text = this.fps;
				}
				if (this.fpsTextAlt)
				{
					this.fpsTextAlt.text = this.fps;
				}
				this._fpsAccumTime = 0f;
				this._fpsFrames = 0;
				if (this.perfMonUI != null && this.perfMonUI.gameObject.activeInHierarchy)
				{
					if (this.totalTimeText != null)
					{
						this.totalTimeText.text = this._totalTime.ToString("F2");
					}
					if (this.renderTimeText != null)
					{
						this.renderTimeText.text = this._renderTime.ToString("F2");
					}
					if (this.scriptsTimeText != null)
					{
						this.scriptsTimeText.text = this._scriptsTime.ToString("F2");
					}
					if (this.physicsTimeText != null)
					{
						this.physicsTimeText.text = this._physicsTime.ToString("F2");
					}
					if (this.waitTimeText != null)
					{
						this.waitTimeText.text = PerfMon.waitTime.ToString("F2");
					}
					if (this.avgTotalTimeText != null)
					{
						this.avgTotalTimeText.text = this._avgTotalTime.ToString("F2");
					}
					if (this.avgRenderTimeText != null)
					{
						this.avgRenderTimeText.text = this._avgRenderTime.ToString("F2");
					}
					if (this.avgScriptsTimeText != null)
					{
						this.avgScriptsTimeText.text = this._avgScriptsTime.ToString("F2");
					}
					if (this.avgPhysicsTimeText != null)
					{
						this.avgPhysicsTimeText.text = this._avgPhysicsTime.ToString("F2");
					}
					if (this.avgWaitTimeText != null)
					{
						this.avgWaitTimeText.text = this._avgWaitTime.ToString("F2");
					}
					if (this.avgFpsText != null)
					{
						this.avgFpsText.text = this._avgFps.ToString("F2");
					}
					if (MemoryOptimizer.singleton != null)
					{
						if (this.physicalMemoryUsageText != null)
						{
							this.physicalMemoryUsageText.text = MemoryOptimizer.singleton.WorkingSetSizeInGBText;
						}
						if (this.pagedMemoryUsageText != null)
						{
							this.pagedMemoryUsageText.text = MemoryOptimizer.singleton.PageFileUsageInGBText;
						}
					}
				}
				if (this.perfMonUIAlt != null && this.perfMonUIAlt.gameObject.activeInHierarchy)
				{
					if (this.totalTimeTextAlt != null)
					{
						this.totalTimeTextAlt.text = this._totalTime.ToString("F2");
					}
					if (this.renderTimeTextAlt != null)
					{
						this.renderTimeTextAlt.text = this._renderTime.ToString("F2");
					}
					if (this.scriptsTimeTextAlt != null)
					{
						this.scriptsTimeTextAlt.text = this._scriptsTime.ToString("F2");
					}
					if (this.physicsTimeTextAlt != null)
					{
						this.physicsTimeTextAlt.text = this._physicsTime.ToString("F2");
					}
					if (this.waitTimeTextAlt != null)
					{
						this.waitTimeTextAlt.text = PerfMon.waitTime.ToString("F2");
					}
					if (this.avgTotalTimeTextAlt != null)
					{
						this.avgTotalTimeTextAlt.text = this._avgTotalTime.ToString("F2");
					}
					if (this.avgRenderTimeTextAlt != null)
					{
						this.avgRenderTimeTextAlt.text = this._avgRenderTime.ToString("F2");
					}
					if (this.avgScriptsTimeTextAlt != null)
					{
						this.avgScriptsTimeTextAlt.text = this._avgScriptsTime.ToString("F2");
					}
					if (this.avgPhysicsTimeTextAlt != null)
					{
						this.avgPhysicsTimeTextAlt.text = this._avgPhysicsTime.ToString("F2");
					}
					if (this.avgWaitTimeTextAlt != null)
					{
						this.avgWaitTimeTextAlt.text = this._avgWaitTime.ToString("F2");
					}
					if (this.avgFpsTextAlt != null)
					{
						this.avgFpsTextAlt.text = this._avgFps.ToString("F2");
					}
					if (MemoryOptimizer.singleton != null)
					{
						if (this.physicalMemoryUsageTextAlt != null)
						{
							this.physicalMemoryUsageTextAlt.text = MemoryOptimizer.singleton.WorkingSetSizeInGBText;
						}
						if (this.pagedMemoryUsageTextAlt != null)
						{
							this.pagedMemoryUsageTextAlt.text = MemoryOptimizer.singleton.PageFileUsageInGBText;
						}
					}
				}
			}
			PerfMon.waitTime = 0f;
		}

		// Token: 0x06006F1D RID: 28445 RVA: 0x0029B97A File Offset: 0x00299D7A
		private void FixedUpdate()
		{
			this._internalPhysicsStopTime = GlobalStopwatch.GetElapsedMilliseconds();
			this._internalPhysicsTime = this._internalPhysicsStopTime - PerfMonPre.physicsStartTime;
		}

		// Token: 0x06006F1E RID: 28446 RVA: 0x0029B99C File Offset: 0x00299D9C
		private void LateUpdate()
		{
			this._frameStartTime = PerfMonPre.frameStartTime;
			this._preRenderStopTime = GlobalStopwatch.GetElapsedMilliseconds();
			this._preRenderTime = this._preRenderStopTime - this._frameStartTime;
			this._physicsTime = PerfMonPre.physicsTime;
			this._scriptsTime = this._preRenderTime - this._physicsTime;
			this.cnt++;
			if (this.cnt == this.framesBetweenUpdate)
			{
				this.cnt = 0;
			}
		}

		// Token: 0x06006F1F RID: 28447 RVA: 0x0029BA18 File Offset: 0x00299E18
		public IEnumerator Start()
		{
			if (GlobalSceneOptions.singleton != null && GlobalSceneOptions.singleton.enablePerfMonOnStart)
			{
				this._on = true;
			}
			if (this.perfMonUI != null)
			{
				this.perfMonUI.gameObject.SetActive(this._on);
			}
			if (this.perfMonUIAlt != null)
			{
				this.perfMonUIAlt.gameObject.SetActive(this._on);
			}
			if (this.onToggle != null)
			{
				this.onToggle.isOn = this._on;
				this.onToggle.onValueChanged.AddListener(new UnityAction<bool>(base.<>m__0));
			}
			for (;;)
			{
				yield return new WaitForEndOfFrame();
				this.DoUpdate();
			}
			yield break;
		}

		// Token: 0x0400600F RID: 24591
		public static float physicsTime;

		// Token: 0x04006010 RID: 24592
		public static float scriptsTime;

		// Token: 0x04006011 RID: 24593
		public static float preRenderTime;

		// Token: 0x04006012 RID: 24594
		public static float renderTime;

		// Token: 0x04006013 RID: 24595
		public static float waitTime;

		// Token: 0x04006014 RID: 24596
		public static float totalTime;

		// Token: 0x04006015 RID: 24597
		public Toggle onToggle;

		// Token: 0x04006016 RID: 24598
		[SerializeField]
		protected bool _on;

		// Token: 0x04006017 RID: 24599
		public Transform perfMonUI;

		// Token: 0x04006018 RID: 24600
		public Transform perfMonUIAlt;

		// Token: 0x04006019 RID: 24601
		public Text totalTimeText;

		// Token: 0x0400601A RID: 24602
		public Text totalTimeTextAlt;

		// Token: 0x0400601B RID: 24603
		public Text scriptsTimeText;

		// Token: 0x0400601C RID: 24604
		public Text scriptsTimeTextAlt;

		// Token: 0x0400601D RID: 24605
		public Text renderTimeText;

		// Token: 0x0400601E RID: 24606
		public Text renderTimeTextAlt;

		// Token: 0x0400601F RID: 24607
		public Text physicsTimeText;

		// Token: 0x04006020 RID: 24608
		public Text physicsTimeTextAlt;

		// Token: 0x04006021 RID: 24609
		public Text waitTimeText;

		// Token: 0x04006022 RID: 24610
		public Text waitTimeTextAlt;

		// Token: 0x04006023 RID: 24611
		public Text avgTotalTimeText;

		// Token: 0x04006024 RID: 24612
		public Text avgTotalTimeTextAlt;

		// Token: 0x04006025 RID: 24613
		public Text avgScriptsTimeText;

		// Token: 0x04006026 RID: 24614
		public Text avgScriptsTimeTextAlt;

		// Token: 0x04006027 RID: 24615
		public Text avgRenderTimeText;

		// Token: 0x04006028 RID: 24616
		public Text avgRenderTimeTextAlt;

		// Token: 0x04006029 RID: 24617
		public Text avgPhysicsTimeText;

		// Token: 0x0400602A RID: 24618
		public Text avgPhysicsTimeTextAlt;

		// Token: 0x0400602B RID: 24619
		public Text avgWaitTimeText;

		// Token: 0x0400602C RID: 24620
		public Text avgWaitTimeTextAlt;

		// Token: 0x0400602D RID: 24621
		public int framesBetweenUpdate = 10;

		// Token: 0x0400602E RID: 24622
		public float _frameStartTime;

		// Token: 0x0400602F RID: 24623
		public float _frameStopTime;

		// Token: 0x04006030 RID: 24624
		public float _preRenderStopTime;

		// Token: 0x04006031 RID: 24625
		protected float _internalPhysicsStopTime;

		// Token: 0x04006032 RID: 24626
		public float _physicsTime;

		// Token: 0x04006033 RID: 24627
		public float _internalPhysicsTime;

		// Token: 0x04006034 RID: 24628
		public float _totalTime;

		// Token: 0x04006035 RID: 24629
		public float _preRenderTime;

		// Token: 0x04006036 RID: 24630
		public float _scriptsTime;

		// Token: 0x04006037 RID: 24631
		public float _renderTime;

		// Token: 0x04006038 RID: 24632
		protected float _totPhysicsTime;

		// Token: 0x04006039 RID: 24633
		protected float _totIntenalPhysicsTime;

		// Token: 0x0400603A RID: 24634
		protected float _totTotalTime;

		// Token: 0x0400603B RID: 24635
		protected float _totRenderTime;

		// Token: 0x0400603C RID: 24636
		protected float _totScriptsTime;

		// Token: 0x0400603D RID: 24637
		protected float _totWaitTime;

		// Token: 0x0400603E RID: 24638
		protected int _totFrames;

		// Token: 0x0400603F RID: 24639
		public float _avgPhysicsTime;

		// Token: 0x04006040 RID: 24640
		public float _avgInternalPhysicsTime;

		// Token: 0x04006041 RID: 24641
		public float _avgTotalTime;

		// Token: 0x04006042 RID: 24642
		public float _avgRenderTime;

		// Token: 0x04006043 RID: 24643
		public float _avgScriptsTime;

		// Token: 0x04006044 RID: 24644
		public float _avgWaitTime;

		// Token: 0x04006045 RID: 24645
		public int avgCalcStartFrame = -1;

		// Token: 0x04006046 RID: 24646
		public int avgCalcNumFrames = 900;

		// Token: 0x04006047 RID: 24647
		protected int cnt;

		// Token: 0x04006048 RID: 24648
		public string fps;

		// Token: 0x04006049 RID: 24649
		public Text fpsText;

		// Token: 0x0400604A RID: 24650
		public Text fpsTextAlt;

		// Token: 0x0400604B RID: 24651
		public Text avgFpsText;

		// Token: 0x0400604C RID: 24652
		public Text avgFpsTextAlt;

		// Token: 0x0400604D RID: 24653
		public Text physicalMemoryUsageText;

		// Token: 0x0400604E RID: 24654
		public Text physicalMemoryUsageTextAlt;

		// Token: 0x0400604F RID: 24655
		public Text pagedMemoryUsageText;

		// Token: 0x04006050 RID: 24656
		public Text pagedMemoryUsageTextAlt;

		// Token: 0x04006051 RID: 24657
		protected float _fpsAccumTime;

		// Token: 0x04006052 RID: 24658
		protected int _fpsFrames;

		// Token: 0x04006053 RID: 24659
		protected float _lastFrameTime;

		// Token: 0x04006054 RID: 24660
		protected float _totFpsTime;

		// Token: 0x04006055 RID: 24661
		public float _avgFps;

		// Token: 0x02001040 RID: 4160
		[CompilerGenerated]
		private sealed class <Start>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x06007799 RID: 30617 RVA: 0x0029BA33 File Offset: 0x00299E33
			[DebuggerHidden]
			public <Start>c__Iterator0()
			{
			}

			// Token: 0x0600779A RID: 30618 RVA: 0x0029BA3C File Offset: 0x00299E3C
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					if (GlobalSceneOptions.singleton != null && GlobalSceneOptions.singleton.enablePerfMonOnStart)
					{
						this._on = true;
					}
					if (this.perfMonUI != null)
					{
						this.perfMonUI.gameObject.SetActive(this._on);
					}
					if (this.perfMonUIAlt != null)
					{
						this.perfMonUIAlt.gameObject.SetActive(this._on);
					}
					if (this.onToggle != null)
					{
						this.onToggle.isOn = this._on;
						this.onToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<>m__0));
					}
					break;
				case 1U:
					base.DoUpdate();
					break;
				default:
					return false;
				}
				this.$current = new WaitForEndOfFrame();
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
			}

			// Token: 0x170011CB RID: 4555
			// (get) Token: 0x0600779B RID: 30619 RVA: 0x0029BB8C File Offset: 0x00299F8C
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170011CC RID: 4556
			// (get) Token: 0x0600779C RID: 30620 RVA: 0x0029BB94 File Offset: 0x00299F94
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x0600779D RID: 30621 RVA: 0x0029BB9C File Offset: 0x00299F9C
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x0600779E RID: 30622 RVA: 0x0029BBAC File Offset: 0x00299FAC
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x0600779F RID: 30623 RVA: 0x0029BBB3 File Offset: 0x00299FB3
			internal void <>m__0(bool A_1)
			{
				base.on = this.onToggle.isOn;
			}

			// Token: 0x04006B9B RID: 27547
			internal PerfMon $this;

			// Token: 0x04006B9C RID: 27548
			internal object $current;

			// Token: 0x04006B9D RID: 27549
			internal bool $disposing;

			// Token: 0x04006B9E RID: 27550
			internal int $PC;
		}
	}
}
