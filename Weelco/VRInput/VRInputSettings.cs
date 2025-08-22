using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Weelco.VRInput
{
	// Token: 0x02000593 RID: 1427
	public class VRInputSettings : MonoBehaviour
	{
		// Token: 0x060023DD RID: 9181 RVA: 0x000CFEC4 File Offset: 0x000CE2C4
		public VRInputSettings()
		{
		}

		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x060023DE RID: 9182 RVA: 0x000CFF1C File Offset: 0x000CE31C
		public static VRInputSettings instance
		{
			get
			{
				return VRInputSettings._instance;
			}
		}

		// Token: 0x060023DF RID: 9183 RVA: 0x000CFF23 File Offset: 0x000CE323
		private void Awake()
		{
			if (VRInputSettings._instance != null)
			{
				Debug.LogWarning("Trying to instantiate multiple VRInputSystems.");
				UnityEngine.Object.DestroyImmediate(base.gameObject);
			}
			VRInputSettings._instance = this;
		}

		// Token: 0x060023E0 RID: 9184 RVA: 0x000CFF50 File Offset: 0x000CE350
		private void Start()
		{
			this._pointersList = new List<IUIPointer>();
			this.createEventSystem(this.ControlMethod);
			switch (this.ControlMethod)
			{
			case VRInputSettings.InputControlMethod.GAZE:
				this.createGazePointer();
				break;
			}
		}

		// Token: 0x060023E1 RID: 9185 RVA: 0x000CFFB4 File Offset: 0x000CE3B4
		private void Update()
		{
			foreach (IUIPointer iuipointer in this._pointersList)
			{
				iuipointer.Update();
			}
		}

		// Token: 0x060023E2 RID: 9186 RVA: 0x000D0010 File Offset: 0x000CE410
		private void OnDestroy()
		{
			if (VRInputModule.instance != null)
			{
				foreach (IUIPointer controller in this._pointersList)
				{
					VRInputModule.instance.RemoveController(controller);
				}
				this._pointersList.Clear();
			}
		}

		// Token: 0x060023E3 RID: 9187 RVA: 0x000D008C File Offset: 0x000CE48C
		private void createEventSystem(VRInputSettings.InputControlMethod method)
		{
			GameObject gameObject = null;
			if (method.Equals(VRInputSettings.InputControlMethod.MOUSE) || method.Equals(VRInputSettings.InputControlMethod.GOOGLEVR))
			{
				return;
			}
			if (UnityEngine.Object.FindObjectOfType<EventSystem>() != null)
			{
				gameObject = UnityEngine.Object.FindObjectOfType<EventSystem>().gameObject;
			}
			if (gameObject == null)
			{
				gameObject = new GameObject("EventSystem");
			}
			MonoBehaviour[] components = gameObject.GetComponents<MonoBehaviour>();
			foreach (MonoBehaviour monoBehaviour in components)
			{
				if (!(monoBehaviour is EventSystem))
				{
					monoBehaviour.enabled = false;
				}
			}
			if (gameObject.GetComponent<VRInputModule>() == null && VRInputModule.instance == null)
			{
				VRInputModule vrinputModule;
				switch (method)
				{
				case VRInputSettings.InputControlMethod.GOOGLEVR:
				case VRInputSettings.InputControlMethod.GAZE:
					vrinputModule = gameObject.AddComponent<VRGazeInputModule>();
					break;
				case VRInputSettings.InputControlMethod.VIVE:
				case VRInputSettings.InputControlMethod.OCULUS_INPUT:
				case VRInputSettings.InputControlMethod.OCULUS_TOUCH:
					vrinputModule = gameObject.AddComponent<VRHitInputModule>();
					break;
				default:
					vrinputModule = gameObject.AddComponent<VRInputModule>();
					break;
				}
				vrinputModule.enabled = true;
			}
		}

		// Token: 0x060023E4 RID: 9188 RVA: 0x000D01AC File Offset: 0x000CE5AC
		private void createGazePointer()
		{
			this.initPointer(new UIGazePointer
			{
				GazeCanvas = this.gazeCanvas,
				GazeProgressBar = this.gazeProgressBar,
				GazeClickTimer = this.gazeClickTimer,
				GazeClickTimerDelay = this.gazeClickTimerDelay
			});
		}

		// Token: 0x060023E5 RID: 9189 RVA: 0x000D01F6 File Offset: 0x000CE5F6
		private void initPointer(IUIPointer pointer)
		{
			VRInputModule.instance.AddController(pointer);
			pointer.Initialize();
			this._pointersList.Add(pointer);
		}

		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x060023E6 RID: 9190 RVA: 0x000D0215 File Offset: 0x000CE615
		// (set) Token: 0x060023E7 RID: 9191 RVA: 0x000D021D File Offset: 0x000CE61D
		public float LaserThickness
		{
			get
			{
				return this.laserThickness;
			}
			set
			{
				this.laserThickness = value;
			}
		}

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x060023E8 RID: 9192 RVA: 0x000D0226 File Offset: 0x000CE626
		// (set) Token: 0x060023E9 RID: 9193 RVA: 0x000D022E File Offset: 0x000CE62E
		public float LaserHitScale
		{
			get
			{
				return this.laserHitScale;
			}
			set
			{
				this.laserHitScale = value;
			}
		}

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x060023EA RID: 9194 RVA: 0x000D0237 File Offset: 0x000CE637
		// (set) Token: 0x060023EB RID: 9195 RVA: 0x000D023F File Offset: 0x000CE63F
		public Color LaserColor
		{
			get
			{
				return this.laserColor;
			}
			set
			{
				this.laserColor = value;
			}
		}

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x060023EC RID: 9196 RVA: 0x000D0248 File Offset: 0x000CE648
		// (set) Token: 0x060023ED RID: 9197 RVA: 0x000D0250 File Offset: 0x000CE650
		public bool UseHapticPulse
		{
			get
			{
				return this.useHapticPulse;
			}
			set
			{
				this.useHapticPulse = value;
			}
		}

		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x060023EE RID: 9198 RVA: 0x000D0259 File Offset: 0x000CE659
		// (set) Token: 0x060023EF RID: 9199 RVA: 0x000D0261 File Offset: 0x000CE661
		public bool UseCustomLaserPointer
		{
			get
			{
				return this.useCustomLaserPointer;
			}
			set
			{
				this.useCustomLaserPointer = value;
			}
		}

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x060023F0 RID: 9200 RVA: 0x000D026A File Offset: 0x000CE66A
		// (set) Token: 0x060023F1 RID: 9201 RVA: 0x000D0272 File Offset: 0x000CE672
		public bool HitAlwaysOn
		{
			get
			{
				return this.hitAlwaysOn;
			}
			set
			{
				this.hitAlwaysOn = value;
			}
		}

		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x060023F2 RID: 9202 RVA: 0x000D027B File Offset: 0x000CE67B
		// (set) Token: 0x060023F3 RID: 9203 RVA: 0x000D0283 File Offset: 0x000CE683
		public VRInputSettings.Hand UsedHand
		{
			get
			{
				return this.usedHand;
			}
			set
			{
				this.usedHand = value;
			}
		}

		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x060023F4 RID: 9204 RVA: 0x000D028C File Offset: 0x000CE68C
		// (set) Token: 0x060023F5 RID: 9205 RVA: 0x000D0294 File Offset: 0x000CE694
		public float GazeClickTimer
		{
			get
			{
				return this.gazeClickTimer;
			}
			set
			{
				this.gazeClickTimer = value;
			}
		}

		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x060023F6 RID: 9206 RVA: 0x000D029D File Offset: 0x000CE69D
		// (set) Token: 0x060023F7 RID: 9207 RVA: 0x000D02A5 File Offset: 0x000CE6A5
		public float GazeClickTimerDelay
		{
			get
			{
				return this.gazeClickTimerDelay;
			}
			set
			{
				this.gazeClickTimerDelay = value;
			}
		}

		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x060023F8 RID: 9208 RVA: 0x000D02AE File Offset: 0x000CE6AE
		// (set) Token: 0x060023F9 RID: 9209 RVA: 0x000D02B6 File Offset: 0x000CE6B6
		public Transform GazeCanvas
		{
			get
			{
				return this.gazeCanvas;
			}
			set
			{
				this.gazeCanvas = value;
			}
		}

		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x060023FA RID: 9210 RVA: 0x000D02BF File Offset: 0x000CE6BF
		// (set) Token: 0x060023FB RID: 9211 RVA: 0x000D02C7 File Offset: 0x000CE6C7
		public Image GazeProgressBar
		{
			get
			{
				return this.gazeProgressBar;
			}
			set
			{
				this.gazeProgressBar = value;
			}
		}

		// Token: 0x060023FC RID: 9212 RVA: 0x000D02D0 File Offset: 0x000CE6D0
		// Note: this type is marked as 'beforefieldinit'.
		static VRInputSettings()
		{
		}

		// Token: 0x04001E34 RID: 7732
		public VRInputSettings.InputControlMethod ControlMethod;

		// Token: 0x04001E35 RID: 7733
		[SerializeField]
		private VRInputSettings.Hand usedHand = VRInputSettings.Hand.Right;

		// Token: 0x04001E36 RID: 7734
		[SerializeField]
		private Transform gazeCanvas;

		// Token: 0x04001E37 RID: 7735
		[SerializeField]
		private Image gazeProgressBar;

		// Token: 0x04001E38 RID: 7736
		[SerializeField]
		private float gazeClickTimer = 1f;

		// Token: 0x04001E39 RID: 7737
		[SerializeField]
		private float gazeClickTimerDelay = 1f;

		// Token: 0x04001E3A RID: 7738
		[SerializeField]
		private float laserThickness = 0.01f;

		// Token: 0x04001E3B RID: 7739
		[SerializeField]
		private float laserHitScale = 1f;

		// Token: 0x04001E3C RID: 7740
		[SerializeField]
		private Color laserColor = Color.red;

		// Token: 0x04001E3D RID: 7741
		[SerializeField]
		private bool useHapticPulse;

		// Token: 0x04001E3E RID: 7742
		[SerializeField]
		private bool useCustomLaserPointer;

		// Token: 0x04001E3F RID: 7743
		[SerializeField]
		private bool hitAlwaysOn = true;

		// Token: 0x04001E40 RID: 7744
		private List<IUIPointer> _pointersList;

		// Token: 0x04001E41 RID: 7745
		private static VRInputSettings _instance;

		// Token: 0x02000594 RID: 1428
		public enum InputControlMethod
		{
			// Token: 0x04001E43 RID: 7747
			MOUSE,
			// Token: 0x04001E44 RID: 7748
			GOOGLEVR,
			// Token: 0x04001E45 RID: 7749
			GAZE,
			// Token: 0x04001E46 RID: 7750
			VIVE,
			// Token: 0x04001E47 RID: 7751
			OCULUS_INPUT,
			// Token: 0x04001E48 RID: 7752
			OCULUS_TOUCH
		}

		// Token: 0x02000595 RID: 1429
		public enum Hand
		{
			// Token: 0x04001E4A RID: 7754
			Both,
			// Token: 0x04001E4B RID: 7755
			Right,
			// Token: 0x04001E4C RID: 7756
			Left
		}
	}
}
