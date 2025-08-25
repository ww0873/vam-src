using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Leap.Unity
{
	// Token: 0x020006CE RID: 1742
	public class DetectorLogicGate : Detector
	{
		// Token: 0x060029FA RID: 10746 RVA: 0x000E294C File Offset: 0x000E0D4C
		public DetectorLogicGate()
		{
		}

		// Token: 0x060029FB RID: 10747 RVA: 0x000E295B File Offset: 0x000E0D5B
		public void AddDetector(Detector detector)
		{
			if (!this.Detectors.Contains(detector))
			{
				this.Detectors.Add(detector);
				this.activateDetector(detector);
			}
		}

		// Token: 0x060029FC RID: 10748 RVA: 0x000E2981 File Offset: 0x000E0D81
		public void RemoveDetector(Detector detector)
		{
			detector.OnActivate.RemoveListener(new UnityAction(this.CheckDetectors));
			detector.OnDeactivate.RemoveListener(new UnityAction(this.CheckDetectors));
			this.Detectors.Remove(detector);
		}

		// Token: 0x060029FD RID: 10749 RVA: 0x000E29C0 File Offset: 0x000E0DC0
		public void AddAllSiblingDetectors()
		{
			Detector[] components = base.GetComponents<Detector>();
			for (int i = 0; i < components.Length; i++)
			{
				if (components[i] != this && components[i].enabled)
				{
					this.AddDetector(components[i]);
				}
			}
		}

		// Token: 0x060029FE RID: 10750 RVA: 0x000E2A0C File Offset: 0x000E0E0C
		private void Awake()
		{
			for (int i = 0; i < this.Detectors.Count; i++)
			{
				this.activateDetector(this.Detectors[i]);
			}
			if (this.AddAllSiblingDetectorsOnAwake)
			{
				this.AddAllSiblingDetectors();
			}
		}

		// Token: 0x060029FF RID: 10751 RVA: 0x000E2A58 File Offset: 0x000E0E58
		private void activateDetector(Detector detector)
		{
			detector.OnActivate.RemoveListener(new UnityAction(this.CheckDetectors));
			detector.OnDeactivate.RemoveListener(new UnityAction(this.CheckDetectors));
			detector.OnActivate.AddListener(new UnityAction(this.CheckDetectors));
			detector.OnDeactivate.AddListener(new UnityAction(this.CheckDetectors));
		}

		// Token: 0x06002A00 RID: 10752 RVA: 0x000E2AC1 File Offset: 0x000E0EC1
		private void OnEnable()
		{
			this.CheckDetectors();
		}

		// Token: 0x06002A01 RID: 10753 RVA: 0x000E2AC9 File Offset: 0x000E0EC9
		private void OnDisable()
		{
			this.Deactivate();
		}

		// Token: 0x06002A02 RID: 10754 RVA: 0x000E2AD4 File Offset: 0x000E0ED4
		protected void CheckDetectors()
		{
			if (this.Detectors.Count < 1)
			{
				return;
			}
			bool flag = this.Detectors[0].IsActive;
			for (int i = 1; i < this.Detectors.Count; i++)
			{
				if (this.GateType == LogicType.AndGate)
				{
					flag = (flag && this.Detectors[i].IsActive);
				}
				else
				{
					flag = (flag || this.Detectors[i].IsActive);
				}
			}
			if (this.Negate)
			{
				flag = !flag;
			}
			if (flag)
			{
				this.Activate();
			}
			else
			{
				this.Deactivate();
			}
		}

		// Token: 0x04002224 RID: 8740
		[SerializeField]
		[Tooltip("The list of observed detectors.")]
		private List<Detector> Detectors;

		// Token: 0x04002225 RID: 8741
		[Tooltip("Add all detectors on this object automatically.")]
		public bool AddAllSiblingDetectorsOnAwake = true;

		// Token: 0x04002226 RID: 8742
		[Tooltip("The type of logic used to combine detector state.")]
		public LogicType GateType;

		// Token: 0x04002227 RID: 8743
		[Tooltip("Whether to negate the gate output.")]
		public bool Negate;
	}
}
