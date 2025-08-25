using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Obi.CrossPlatformInput.PlatformSpecific;
using UnityEngine;

namespace Obi.CrossPlatformInput
{
	// Token: 0x02000372 RID: 882
	public static class CrossPlatformInputManager
	{
		// Token: 0x060015FA RID: 5626 RVA: 0x0007D3B4 File Offset: 0x0007B7B4
		static CrossPlatformInputManager()
		{
			CrossPlatformInputManager.activeInput = CrossPlatformInputManager.s_HardwareInput;
		}

		// Token: 0x060015FB RID: 5627 RVA: 0x0007D3D4 File Offset: 0x0007B7D4
		public static void SwitchActiveInputMethod(CrossPlatformInputManager.ActiveInputMethod activeInputMethod)
		{
			if (activeInputMethod != CrossPlatformInputManager.ActiveInputMethod.Hardware)
			{
				if (activeInputMethod == CrossPlatformInputManager.ActiveInputMethod.Touch)
				{
					CrossPlatformInputManager.activeInput = CrossPlatformInputManager.s_TouchInput;
				}
			}
			else
			{
				CrossPlatformInputManager.activeInput = CrossPlatformInputManager.s_HardwareInput;
			}
		}

		// Token: 0x060015FC RID: 5628 RVA: 0x0007D406 File Offset: 0x0007B806
		public static bool AxisExists(string name)
		{
			return CrossPlatformInputManager.activeInput.AxisExists(name);
		}

		// Token: 0x060015FD RID: 5629 RVA: 0x0007D413 File Offset: 0x0007B813
		public static bool ButtonExists(string name)
		{
			return CrossPlatformInputManager.activeInput.ButtonExists(name);
		}

		// Token: 0x060015FE RID: 5630 RVA: 0x0007D420 File Offset: 0x0007B820
		public static void RegisterVirtualAxis(CrossPlatformInputManager.VirtualAxis axis)
		{
			CrossPlatformInputManager.activeInput.RegisterVirtualAxis(axis);
		}

		// Token: 0x060015FF RID: 5631 RVA: 0x0007D42D File Offset: 0x0007B82D
		public static void RegisterVirtualButton(CrossPlatformInputManager.VirtualButton button)
		{
			CrossPlatformInputManager.activeInput.RegisterVirtualButton(button);
		}

		// Token: 0x06001600 RID: 5632 RVA: 0x0007D43A File Offset: 0x0007B83A
		public static void UnRegisterVirtualAxis(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			CrossPlatformInputManager.activeInput.UnRegisterVirtualAxis(name);
		}

		// Token: 0x06001601 RID: 5633 RVA: 0x0007D458 File Offset: 0x0007B858
		public static void UnRegisterVirtualButton(string name)
		{
			CrossPlatformInputManager.activeInput.UnRegisterVirtualButton(name);
		}

		// Token: 0x06001602 RID: 5634 RVA: 0x0007D465 File Offset: 0x0007B865
		public static CrossPlatformInputManager.VirtualAxis VirtualAxisReference(string name)
		{
			return CrossPlatformInputManager.activeInput.VirtualAxisReference(name);
		}

		// Token: 0x06001603 RID: 5635 RVA: 0x0007D472 File Offset: 0x0007B872
		public static float GetAxis(string name)
		{
			return CrossPlatformInputManager.GetAxis(name, false);
		}

		// Token: 0x06001604 RID: 5636 RVA: 0x0007D47B File Offset: 0x0007B87B
		public static float GetAxisRaw(string name)
		{
			return CrossPlatformInputManager.GetAxis(name, true);
		}

		// Token: 0x06001605 RID: 5637 RVA: 0x0007D484 File Offset: 0x0007B884
		private static float GetAxis(string name, bool raw)
		{
			return CrossPlatformInputManager.activeInput.GetAxis(name, raw);
		}

		// Token: 0x06001606 RID: 5638 RVA: 0x0007D492 File Offset: 0x0007B892
		public static bool GetButton(string name)
		{
			return CrossPlatformInputManager.activeInput.GetButton(name);
		}

		// Token: 0x06001607 RID: 5639 RVA: 0x0007D49F File Offset: 0x0007B89F
		public static bool GetButtonDown(string name)
		{
			return CrossPlatformInputManager.activeInput.GetButtonDown(name);
		}

		// Token: 0x06001608 RID: 5640 RVA: 0x0007D4AC File Offset: 0x0007B8AC
		public static bool GetButtonUp(string name)
		{
			return CrossPlatformInputManager.activeInput.GetButtonUp(name);
		}

		// Token: 0x06001609 RID: 5641 RVA: 0x0007D4B9 File Offset: 0x0007B8B9
		public static void SetButtonDown(string name)
		{
			CrossPlatformInputManager.activeInput.SetButtonDown(name);
		}

		// Token: 0x0600160A RID: 5642 RVA: 0x0007D4C6 File Offset: 0x0007B8C6
		public static void SetButtonUp(string name)
		{
			CrossPlatformInputManager.activeInput.SetButtonUp(name);
		}

		// Token: 0x0600160B RID: 5643 RVA: 0x0007D4D3 File Offset: 0x0007B8D3
		public static void SetAxisPositive(string name)
		{
			CrossPlatformInputManager.activeInput.SetAxisPositive(name);
		}

		// Token: 0x0600160C RID: 5644 RVA: 0x0007D4E0 File Offset: 0x0007B8E0
		public static void SetAxisNegative(string name)
		{
			CrossPlatformInputManager.activeInput.SetAxisNegative(name);
		}

		// Token: 0x0600160D RID: 5645 RVA: 0x0007D4ED File Offset: 0x0007B8ED
		public static void SetAxisZero(string name)
		{
			CrossPlatformInputManager.activeInput.SetAxisZero(name);
		}

		// Token: 0x0600160E RID: 5646 RVA: 0x0007D4FA File Offset: 0x0007B8FA
		public static void SetAxis(string name, float value)
		{
			CrossPlatformInputManager.activeInput.SetAxis(name, value);
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x0600160F RID: 5647 RVA: 0x0007D508 File Offset: 0x0007B908
		public static Vector3 mousePosition
		{
			get
			{
				return CrossPlatformInputManager.activeInput.MousePosition();
			}
		}

		// Token: 0x06001610 RID: 5648 RVA: 0x0007D514 File Offset: 0x0007B914
		public static void SetVirtualMousePositionX(float f)
		{
			CrossPlatformInputManager.activeInput.SetVirtualMousePositionX(f);
		}

		// Token: 0x06001611 RID: 5649 RVA: 0x0007D521 File Offset: 0x0007B921
		public static void SetVirtualMousePositionY(float f)
		{
			CrossPlatformInputManager.activeInput.SetVirtualMousePositionY(f);
		}

		// Token: 0x06001612 RID: 5650 RVA: 0x0007D52E File Offset: 0x0007B92E
		public static void SetVirtualMousePositionZ(float f)
		{
			CrossPlatformInputManager.activeInput.SetVirtualMousePositionZ(f);
		}

		// Token: 0x04001255 RID: 4693
		private static VirtualInput activeInput;

		// Token: 0x04001256 RID: 4694
		private static VirtualInput s_TouchInput = new MobileInput();

		// Token: 0x04001257 RID: 4695
		private static VirtualInput s_HardwareInput = new StandaloneInput();

		// Token: 0x02000373 RID: 883
		public enum ActiveInputMethod
		{
			// Token: 0x04001259 RID: 4697
			Hardware,
			// Token: 0x0400125A RID: 4698
			Touch
		}

		// Token: 0x02000374 RID: 884
		public class VirtualAxis
		{
			// Token: 0x06001613 RID: 5651 RVA: 0x0007D53B File Offset: 0x0007B93B
			public VirtualAxis(string name) : this(name, true)
			{
			}

			// Token: 0x06001614 RID: 5652 RVA: 0x0007D545 File Offset: 0x0007B945
			public VirtualAxis(string name, bool matchToInputSettings)
			{
				this.name = name;
				this.matchWithInputManager = matchToInputSettings;
			}

			// Token: 0x17000280 RID: 640
			// (get) Token: 0x06001615 RID: 5653 RVA: 0x0007D55B File Offset: 0x0007B95B
			// (set) Token: 0x06001616 RID: 5654 RVA: 0x0007D563 File Offset: 0x0007B963
			public string name
			{
				[CompilerGenerated]
				get
				{
					return this.<name>k__BackingField;
				}
				[CompilerGenerated]
				private set
				{
					this.<name>k__BackingField = value;
				}
			}

			// Token: 0x17000281 RID: 641
			// (get) Token: 0x06001617 RID: 5655 RVA: 0x0007D56C File Offset: 0x0007B96C
			// (set) Token: 0x06001618 RID: 5656 RVA: 0x0007D574 File Offset: 0x0007B974
			public bool matchWithInputManager
			{
				[CompilerGenerated]
				get
				{
					return this.<matchWithInputManager>k__BackingField;
				}
				[CompilerGenerated]
				private set
				{
					this.<matchWithInputManager>k__BackingField = value;
				}
			}

			// Token: 0x06001619 RID: 5657 RVA: 0x0007D57D File Offset: 0x0007B97D
			public void Remove()
			{
				CrossPlatformInputManager.UnRegisterVirtualAxis(this.name);
			}

			// Token: 0x0600161A RID: 5658 RVA: 0x0007D58A File Offset: 0x0007B98A
			public void Update(float value)
			{
				this.m_Value = value;
			}

			// Token: 0x17000282 RID: 642
			// (get) Token: 0x0600161B RID: 5659 RVA: 0x0007D593 File Offset: 0x0007B993
			public float GetValue
			{
				get
				{
					return this.m_Value;
				}
			}

			// Token: 0x17000283 RID: 643
			// (get) Token: 0x0600161C RID: 5660 RVA: 0x0007D59B File Offset: 0x0007B99B
			public float GetValueRaw
			{
				get
				{
					return this.m_Value;
				}
			}

			// Token: 0x0400125B RID: 4699
			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private string <name>k__BackingField;

			// Token: 0x0400125C RID: 4700
			private float m_Value;

			// Token: 0x0400125D RID: 4701
			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private bool <matchWithInputManager>k__BackingField;
		}

		// Token: 0x02000375 RID: 885
		public class VirtualButton
		{
			// Token: 0x0600161D RID: 5661 RVA: 0x0007D5A3 File Offset: 0x0007B9A3
			public VirtualButton(string name) : this(name, true)
			{
			}

			// Token: 0x0600161E RID: 5662 RVA: 0x0007D5AD File Offset: 0x0007B9AD
			public VirtualButton(string name, bool matchToInputSettings)
			{
				this.name = name;
				this.matchWithInputManager = matchToInputSettings;
			}

			// Token: 0x17000284 RID: 644
			// (get) Token: 0x0600161F RID: 5663 RVA: 0x0007D5D3 File Offset: 0x0007B9D3
			// (set) Token: 0x06001620 RID: 5664 RVA: 0x0007D5DB File Offset: 0x0007B9DB
			public string name
			{
				[CompilerGenerated]
				get
				{
					return this.<name>k__BackingField;
				}
				[CompilerGenerated]
				private set
				{
					this.<name>k__BackingField = value;
				}
			}

			// Token: 0x17000285 RID: 645
			// (get) Token: 0x06001621 RID: 5665 RVA: 0x0007D5E4 File Offset: 0x0007B9E4
			// (set) Token: 0x06001622 RID: 5666 RVA: 0x0007D5EC File Offset: 0x0007B9EC
			public bool matchWithInputManager
			{
				[CompilerGenerated]
				get
				{
					return this.<matchWithInputManager>k__BackingField;
				}
				[CompilerGenerated]
				private set
				{
					this.<matchWithInputManager>k__BackingField = value;
				}
			}

			// Token: 0x06001623 RID: 5667 RVA: 0x0007D5F5 File Offset: 0x0007B9F5
			public void Pressed()
			{
				if (this.m_Pressed)
				{
					return;
				}
				this.m_Pressed = true;
				this.m_LastPressedFrame = Time.frameCount;
			}

			// Token: 0x06001624 RID: 5668 RVA: 0x0007D615 File Offset: 0x0007BA15
			public void Released()
			{
				this.m_Pressed = false;
				this.m_ReleasedFrame = Time.frameCount;
			}

			// Token: 0x06001625 RID: 5669 RVA: 0x0007D629 File Offset: 0x0007BA29
			public void Remove()
			{
				CrossPlatformInputManager.UnRegisterVirtualButton(this.name);
			}

			// Token: 0x17000286 RID: 646
			// (get) Token: 0x06001626 RID: 5670 RVA: 0x0007D636 File Offset: 0x0007BA36
			public bool GetButton
			{
				get
				{
					return this.m_Pressed;
				}
			}

			// Token: 0x17000287 RID: 647
			// (get) Token: 0x06001627 RID: 5671 RVA: 0x0007D63E File Offset: 0x0007BA3E
			public bool GetButtonDown
			{
				get
				{
					return this.m_LastPressedFrame - Time.frameCount == -1;
				}
			}

			// Token: 0x17000288 RID: 648
			// (get) Token: 0x06001628 RID: 5672 RVA: 0x0007D64F File Offset: 0x0007BA4F
			public bool GetButtonUp
			{
				get
				{
					return this.m_ReleasedFrame == Time.frameCount - 1;
				}
			}

			// Token: 0x0400125E RID: 4702
			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private string <name>k__BackingField;

			// Token: 0x0400125F RID: 4703
			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private bool <matchWithInputManager>k__BackingField;

			// Token: 0x04001260 RID: 4704
			private int m_LastPressedFrame = -5;

			// Token: 0x04001261 RID: 4705
			private int m_ReleasedFrame = -5;

			// Token: 0x04001262 RID: 4706
			private bool m_Pressed;
		}
	}
}
