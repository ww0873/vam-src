using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace Battlehub.RTCommon
{
	// Token: 0x020000BF RID: 191
	public static class RuntimeEditorApplication
	{
		// Token: 0x06000332 RID: 818 RVA: 0x0001487E File Offset: 0x00012C7E
		static RuntimeEditorApplication()
		{
			RuntimeEditorApplication.Reset();
		}

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06000333 RID: 819 RVA: 0x00014890 File Offset: 0x00012C90
		// (remove) Token: 0x06000334 RID: 820 RVA: 0x000148C4 File Offset: 0x00012CC4
		public static event RuntimeEditorEvent PlaymodeStateChanging
		{
			add
			{
				RuntimeEditorEvent runtimeEditorEvent = RuntimeEditorApplication.PlaymodeStateChanging;
				RuntimeEditorEvent runtimeEditorEvent2;
				do
				{
					runtimeEditorEvent2 = runtimeEditorEvent;
					runtimeEditorEvent = Interlocked.CompareExchange<RuntimeEditorEvent>(ref RuntimeEditorApplication.PlaymodeStateChanging, (RuntimeEditorEvent)Delegate.Combine(runtimeEditorEvent2, value), runtimeEditorEvent);
				}
				while (runtimeEditorEvent != runtimeEditorEvent2);
			}
			remove
			{
				RuntimeEditorEvent runtimeEditorEvent = RuntimeEditorApplication.PlaymodeStateChanging;
				RuntimeEditorEvent runtimeEditorEvent2;
				do
				{
					runtimeEditorEvent2 = runtimeEditorEvent;
					runtimeEditorEvent = Interlocked.CompareExchange<RuntimeEditorEvent>(ref RuntimeEditorApplication.PlaymodeStateChanging, (RuntimeEditorEvent)Delegate.Remove(runtimeEditorEvent2, value), runtimeEditorEvent);
				}
				while (runtimeEditorEvent != runtimeEditorEvent2);
			}
		}

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06000335 RID: 821 RVA: 0x000148F8 File Offset: 0x00012CF8
		// (remove) Token: 0x06000336 RID: 822 RVA: 0x0001492C File Offset: 0x00012D2C
		public static event RuntimeEditorEvent PlaymodeStateChanged
		{
			add
			{
				RuntimeEditorEvent runtimeEditorEvent = RuntimeEditorApplication.PlaymodeStateChanged;
				RuntimeEditorEvent runtimeEditorEvent2;
				do
				{
					runtimeEditorEvent2 = runtimeEditorEvent;
					runtimeEditorEvent = Interlocked.CompareExchange<RuntimeEditorEvent>(ref RuntimeEditorApplication.PlaymodeStateChanged, (RuntimeEditorEvent)Delegate.Combine(runtimeEditorEvent2, value), runtimeEditorEvent);
				}
				while (runtimeEditorEvent != runtimeEditorEvent2);
			}
			remove
			{
				RuntimeEditorEvent runtimeEditorEvent = RuntimeEditorApplication.PlaymodeStateChanged;
				RuntimeEditorEvent runtimeEditorEvent2;
				do
				{
					runtimeEditorEvent2 = runtimeEditorEvent;
					runtimeEditorEvent = Interlocked.CompareExchange<RuntimeEditorEvent>(ref RuntimeEditorApplication.PlaymodeStateChanged, (RuntimeEditorEvent)Delegate.Remove(runtimeEditorEvent2, value), runtimeEditorEvent);
				}
				while (runtimeEditorEvent != runtimeEditorEvent2);
			}
		}

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x06000337 RID: 823 RVA: 0x00014960 File Offset: 0x00012D60
		// (remove) Token: 0x06000338 RID: 824 RVA: 0x00014994 File Offset: 0x00012D94
		public static event RuntimeEditorEvent ActiveWindowChanged
		{
			add
			{
				RuntimeEditorEvent runtimeEditorEvent = RuntimeEditorApplication.ActiveWindowChanged;
				RuntimeEditorEvent runtimeEditorEvent2;
				do
				{
					runtimeEditorEvent2 = runtimeEditorEvent;
					runtimeEditorEvent = Interlocked.CompareExchange<RuntimeEditorEvent>(ref RuntimeEditorApplication.ActiveWindowChanged, (RuntimeEditorEvent)Delegate.Combine(runtimeEditorEvent2, value), runtimeEditorEvent);
				}
				while (runtimeEditorEvent != runtimeEditorEvent2);
			}
			remove
			{
				RuntimeEditorEvent runtimeEditorEvent = RuntimeEditorApplication.ActiveWindowChanged;
				RuntimeEditorEvent runtimeEditorEvent2;
				do
				{
					runtimeEditorEvent2 = runtimeEditorEvent;
					runtimeEditorEvent = Interlocked.CompareExchange<RuntimeEditorEvent>(ref RuntimeEditorApplication.ActiveWindowChanged, (RuntimeEditorEvent)Delegate.Remove(runtimeEditorEvent2, value), runtimeEditorEvent);
				}
				while (runtimeEditorEvent != runtimeEditorEvent2);
			}
		}

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x06000339 RID: 825 RVA: 0x000149C8 File Offset: 0x00012DC8
		// (remove) Token: 0x0600033A RID: 826 RVA: 0x000149FC File Offset: 0x00012DFC
		public static event RuntimeEditorEvent PointerOverWindowChanged
		{
			add
			{
				RuntimeEditorEvent runtimeEditorEvent = RuntimeEditorApplication.PointerOverWindowChanged;
				RuntimeEditorEvent runtimeEditorEvent2;
				do
				{
					runtimeEditorEvent2 = runtimeEditorEvent;
					runtimeEditorEvent = Interlocked.CompareExchange<RuntimeEditorEvent>(ref RuntimeEditorApplication.PointerOverWindowChanged, (RuntimeEditorEvent)Delegate.Combine(runtimeEditorEvent2, value), runtimeEditorEvent);
				}
				while (runtimeEditorEvent != runtimeEditorEvent2);
			}
			remove
			{
				RuntimeEditorEvent runtimeEditorEvent = RuntimeEditorApplication.PointerOverWindowChanged;
				RuntimeEditorEvent runtimeEditorEvent2;
				do
				{
					runtimeEditorEvent2 = runtimeEditorEvent;
					runtimeEditorEvent = Interlocked.CompareExchange<RuntimeEditorEvent>(ref RuntimeEditorApplication.PointerOverWindowChanged, (RuntimeEditorEvent)Delegate.Remove(runtimeEditorEvent2, value), runtimeEditorEvent);
				}
				while (runtimeEditorEvent != runtimeEditorEvent2);
			}
		}

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x0600033B RID: 827 RVA: 0x00014A30 File Offset: 0x00012E30
		// (remove) Token: 0x0600033C RID: 828 RVA: 0x00014A64 File Offset: 0x00012E64
		public static event RuntimeEditorEvent IsOpenedChanged
		{
			add
			{
				RuntimeEditorEvent runtimeEditorEvent = RuntimeEditorApplication.IsOpenedChanged;
				RuntimeEditorEvent runtimeEditorEvent2;
				do
				{
					runtimeEditorEvent2 = runtimeEditorEvent;
					runtimeEditorEvent = Interlocked.CompareExchange<RuntimeEditorEvent>(ref RuntimeEditorApplication.IsOpenedChanged, (RuntimeEditorEvent)Delegate.Combine(runtimeEditorEvent2, value), runtimeEditorEvent);
				}
				while (runtimeEditorEvent != runtimeEditorEvent2);
			}
			remove
			{
				RuntimeEditorEvent runtimeEditorEvent = RuntimeEditorApplication.IsOpenedChanged;
				RuntimeEditorEvent runtimeEditorEvent2;
				do
				{
					runtimeEditorEvent2 = runtimeEditorEvent;
					runtimeEditorEvent = Interlocked.CompareExchange<RuntimeEditorEvent>(ref RuntimeEditorApplication.IsOpenedChanged, (RuntimeEditorEvent)Delegate.Remove(runtimeEditorEvent2, value), runtimeEditorEvent);
				}
				while (runtimeEditorEvent != runtimeEditorEvent2);
			}
		}

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x0600033D RID: 829 RVA: 0x00014A98 File Offset: 0x00012E98
		// (remove) Token: 0x0600033E RID: 830 RVA: 0x00014ACC File Offset: 0x00012ECC
		public static event RuntimeEditorEvent ActiveSceneCameraChanged
		{
			add
			{
				RuntimeEditorEvent runtimeEditorEvent = RuntimeEditorApplication.ActiveSceneCameraChanged;
				RuntimeEditorEvent runtimeEditorEvent2;
				do
				{
					runtimeEditorEvent2 = runtimeEditorEvent;
					runtimeEditorEvent = Interlocked.CompareExchange<RuntimeEditorEvent>(ref RuntimeEditorApplication.ActiveSceneCameraChanged, (RuntimeEditorEvent)Delegate.Combine(runtimeEditorEvent2, value), runtimeEditorEvent);
				}
				while (runtimeEditorEvent != runtimeEditorEvent2);
			}
			remove
			{
				RuntimeEditorEvent runtimeEditorEvent = RuntimeEditorApplication.ActiveSceneCameraChanged;
				RuntimeEditorEvent runtimeEditorEvent2;
				do
				{
					runtimeEditorEvent2 = runtimeEditorEvent;
					runtimeEditorEvent = Interlocked.CompareExchange<RuntimeEditorEvent>(ref RuntimeEditorApplication.ActiveSceneCameraChanged, (RuntimeEditorEvent)Delegate.Remove(runtimeEditorEvent2, value), runtimeEditorEvent);
				}
				while (runtimeEditorEvent != runtimeEditorEvent2);
			}
		}

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x0600033F RID: 831 RVA: 0x00014B00 File Offset: 0x00012F00
		// (remove) Token: 0x06000340 RID: 832 RVA: 0x00014B34 File Offset: 0x00012F34
		public static event RuntimeEditorEvent SaveSelectedObjectsRequired
		{
			add
			{
				RuntimeEditorEvent runtimeEditorEvent = RuntimeEditorApplication.SaveSelectedObjectsRequired;
				RuntimeEditorEvent runtimeEditorEvent2;
				do
				{
					runtimeEditorEvent2 = runtimeEditorEvent;
					runtimeEditorEvent = Interlocked.CompareExchange<RuntimeEditorEvent>(ref RuntimeEditorApplication.SaveSelectedObjectsRequired, (RuntimeEditorEvent)Delegate.Combine(runtimeEditorEvent2, value), runtimeEditorEvent);
				}
				while (runtimeEditorEvent != runtimeEditorEvent2);
			}
			remove
			{
				RuntimeEditorEvent runtimeEditorEvent = RuntimeEditorApplication.SaveSelectedObjectsRequired;
				RuntimeEditorEvent runtimeEditorEvent2;
				do
				{
					runtimeEditorEvent2 = runtimeEditorEvent;
					runtimeEditorEvent = Interlocked.CompareExchange<RuntimeEditorEvent>(ref RuntimeEditorApplication.SaveSelectedObjectsRequired, (RuntimeEditorEvent)Delegate.Remove(runtimeEditorEvent2, value), runtimeEditorEvent);
				}
				while (runtimeEditorEvent != runtimeEditorEvent2);
			}
		}

		// Token: 0x06000341 RID: 833 RVA: 0x00014B68 File Offset: 0x00012F68
		public static void SaveSelectedObjects()
		{
			if (RuntimeEditorApplication.SaveSelectedObjectsRequired != null)
			{
				RuntimeEditorApplication.SaveSelectedObjectsRequired();
			}
		}

		// Token: 0x06000342 RID: 834 RVA: 0x00014B80 File Offset: 0x00012F80
		public static void Reset()
		{
			RuntimeEditorApplication.m_windows = new List<RuntimeEditorWindow>();
			RuntimeEditorApplication.m_pointerOverWindow = null;
			RuntimeEditorApplication.m_activeWindow = null;
			RuntimeEditorApplication.m_activeCameraIndex = 0;
			RuntimeEditorApplication.GameCameras = null;
			RuntimeEditorApplication.SceneCameras = null;
			RuntimeEditorApplication.m_isOpened = false;
			RuntimeEditorApplication.m_isPlaying = false;
			RuntimeSelection.objects = null;
			RuntimeUndo.Reset();
			RuntimeTools.Reset();
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000343 RID: 835 RVA: 0x00014BD1 File Offset: 0x00012FD1
		public static RuntimeEditorWindow PointerOverWindow
		{
			get
			{
				return RuntimeEditorApplication.m_pointerOverWindow;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000344 RID: 836 RVA: 0x00014BD8 File Offset: 0x00012FD8
		public static RuntimeWindowType PointerOverWindowType
		{
			get
			{
				if (RuntimeEditorApplication.m_pointerOverWindow == null)
				{
					return RuntimeWindowType.None;
				}
				return RuntimeEditorApplication.m_pointerOverWindow.WindowType;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000345 RID: 837 RVA: 0x00014BF6 File Offset: 0x00012FF6
		public static RuntimeEditorWindow ActiveWindow
		{
			get
			{
				return RuntimeEditorApplication.m_activeWindow;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000346 RID: 838 RVA: 0x00014BFD File Offset: 0x00012FFD
		public static RuntimeWindowType ActiveWindowType
		{
			get
			{
				if (RuntimeEditorApplication.m_activeWindow == null)
				{
					return RuntimeWindowType.None;
				}
				return RuntimeEditorApplication.m_activeWindow.WindowType;
			}
		}

		// Token: 0x06000347 RID: 839 RVA: 0x00014C1C File Offset: 0x0001301C
		public static RuntimeEditorWindow GetWindow(RuntimeWindowType type)
		{
			RuntimeEditorApplication.<GetWindow>c__AnonStorey0 <GetWindow>c__AnonStorey = new RuntimeEditorApplication.<GetWindow>c__AnonStorey0();
			<GetWindow>c__AnonStorey.type = type;
			return RuntimeEditorApplication.m_windows.Where(new Func<RuntimeEditorWindow, bool>(<GetWindow>c__AnonStorey.<>m__0)).FirstOrDefault<RuntimeEditorWindow>();
		}

		// Token: 0x06000348 RID: 840 RVA: 0x00014C51 File Offset: 0x00013051
		public static void ActivateWindow(RuntimeEditorWindow window)
		{
			if (RuntimeEditorApplication.m_activeWindow != window)
			{
				RuntimeEditorApplication.m_activeWindow = window;
				if (RuntimeEditorApplication.ActiveWindowChanged != null)
				{
					RuntimeEditorApplication.ActiveWindowChanged();
				}
			}
		}

		// Token: 0x06000349 RID: 841 RVA: 0x00014C7D File Offset: 0x0001307D
		public static void ActivateWindow(RuntimeWindowType type)
		{
			RuntimeEditorApplication.ActivateWindow(RuntimeEditorApplication.GetWindow(type));
		}

		// Token: 0x0600034A RID: 842 RVA: 0x00014C8A File Offset: 0x0001308A
		public static void PointerEnter(RuntimeEditorWindow window)
		{
			if (RuntimeEditorApplication.m_pointerOverWindow != window)
			{
				RuntimeEditorApplication.m_pointerOverWindow = window;
				if (RuntimeEditorApplication.PointerOverWindowChanged != null)
				{
					RuntimeEditorApplication.PointerOverWindowChanged();
				}
			}
		}

		// Token: 0x0600034B RID: 843 RVA: 0x00014CB6 File Offset: 0x000130B6
		public static void PointerExit(RuntimeEditorWindow window)
		{
			if (RuntimeEditorApplication.m_pointerOverWindow == window && RuntimeEditorApplication.m_pointerOverWindow != null)
			{
				RuntimeEditorApplication.m_pointerOverWindow = null;
				if (RuntimeEditorApplication.PointerOverWindowChanged != null)
				{
					RuntimeEditorApplication.PointerOverWindowChanged();
				}
			}
		}

		// Token: 0x0600034C RID: 844 RVA: 0x00014CF2 File Offset: 0x000130F2
		public static bool IsPointerOverWindow(RuntimeWindowType type)
		{
			return RuntimeEditorApplication.PointerOverWindowType == type;
		}

		// Token: 0x0600034D RID: 845 RVA: 0x00014CFC File Offset: 0x000130FC
		public static bool IsPointerOverWindow(RuntimeEditorWindow window)
		{
			return RuntimeEditorApplication.m_pointerOverWindow == window;
		}

		// Token: 0x0600034E RID: 846 RVA: 0x00014D09 File Offset: 0x00013109
		public static bool IsActiveWindow(RuntimeWindowType type)
		{
			return RuntimeEditorApplication.ActiveWindowType == type;
		}

		// Token: 0x0600034F RID: 847 RVA: 0x00014D13 File Offset: 0x00013113
		public static bool IsActiveWindow(RuntimeEditorWindow window)
		{
			return RuntimeEditorApplication.m_activeWindow == window;
		}

		// Token: 0x06000350 RID: 848 RVA: 0x00014D20 File Offset: 0x00013120
		public static void AddWindow(RuntimeEditorWindow window)
		{
			RuntimeEditorApplication.m_windows.Add(window);
		}

		// Token: 0x06000351 RID: 849 RVA: 0x00014D2D File Offset: 0x0001312D
		public static void RemoveWindow(RuntimeEditorWindow window)
		{
			if (RuntimeEditorApplication.m_windows != null)
			{
				RuntimeEditorApplication.m_windows.Remove(window);
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000352 RID: 850 RVA: 0x00014D45 File Offset: 0x00013145
		// (set) Token: 0x06000353 RID: 851 RVA: 0x00014D4C File Offset: 0x0001314C
		public static Camera[] GameCameras
		{
			[CompilerGenerated]
			get
			{
				return RuntimeEditorApplication.<GameCameras>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				RuntimeEditorApplication.<GameCameras>k__BackingField = value;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000354 RID: 852 RVA: 0x00014D54 File Offset: 0x00013154
		// (set) Token: 0x06000355 RID: 853 RVA: 0x00014D5B File Offset: 0x0001315B
		public static Camera[] SceneCameras
		{
			[CompilerGenerated]
			get
			{
				return RuntimeEditorApplication.<SceneCameras>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				RuntimeEditorApplication.<SceneCameras>k__BackingField = value;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000356 RID: 854 RVA: 0x00014D63 File Offset: 0x00013163
		// (set) Token: 0x06000357 RID: 855 RVA: 0x00014D6A File Offset: 0x0001316A
		public static int ActiveSceneCameraIndex
		{
			get
			{
				return RuntimeEditorApplication.m_activeCameraIndex;
			}
			set
			{
				if (RuntimeEditorApplication.m_activeCameraIndex != value)
				{
					RuntimeEditorApplication.m_activeCameraIndex = value;
					if (RuntimeEditorApplication.ActiveSceneCameraChanged != null)
					{
						RuntimeEditorApplication.ActiveSceneCameraChanged();
					}
				}
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000358 RID: 856 RVA: 0x00014D91 File Offset: 0x00013191
		public static Camera ActiveSceneCamera
		{
			get
			{
				if (RuntimeEditorApplication.SceneCameras == null || RuntimeEditorApplication.SceneCameras.Length == 0)
				{
					return null;
				}
				return RuntimeEditorApplication.SceneCameras[RuntimeEditorApplication.ActiveSceneCameraIndex];
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000359 RID: 857 RVA: 0x00014DB6 File Offset: 0x000131B6
		// (set) Token: 0x0600035A RID: 858 RVA: 0x00014DBD File Offset: 0x000131BD
		public static bool IsOpened
		{
			get
			{
				return RuntimeEditorApplication.m_isOpened;
			}
			set
			{
				if (RuntimeEditorApplication.m_isOpened != value)
				{
					RuntimeEditorApplication.m_isOpened = value;
					if (!RuntimeEditorApplication.m_isOpened)
					{
						RuntimeEditorApplication.ActivateWindow(RuntimeEditorApplication.GetWindow(RuntimeWindowType.GameView));
					}
					if (RuntimeEditorApplication.IsOpenedChanged != null)
					{
						RuntimeEditorApplication.IsOpenedChanged();
					}
				}
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x0600035B RID: 859 RVA: 0x00014DF9 File Offset: 0x000131F9
		public static bool IsPlaymodeStateChanging
		{
			get
			{
				return RuntimeEditorApplication.m_isPlayModeStateChanging;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600035C RID: 860 RVA: 0x00014E00 File Offset: 0x00013200
		// (set) Token: 0x0600035D RID: 861 RVA: 0x00014E08 File Offset: 0x00013208
		public static bool IsPlaying
		{
			get
			{
				return RuntimeEditorApplication.m_isPlaying;
			}
			set
			{
				if (RuntimeEditorApplication.m_isPlaying != value)
				{
					RuntimeEditorApplication.m_isPlaying = value;
					RuntimeEditorApplication.m_isPlayModeStateChanging = true;
					if (RuntimeEditorApplication.PlaymodeStateChanging != null)
					{
						RuntimeEditorApplication.PlaymodeStateChanging();
					}
					if (RuntimeEditorApplication.PlaymodeStateChanged != null)
					{
						RuntimeEditorApplication.PlaymodeStateChanged();
					}
					RuntimeEditorApplication.m_isPlayModeStateChanging = false;
				}
			}
		}

		// Token: 0x040003DC RID: 988
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static RuntimeEditorEvent PlaymodeStateChanging;

		// Token: 0x040003DD RID: 989
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static RuntimeEditorEvent PlaymodeStateChanged;

		// Token: 0x040003DE RID: 990
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static RuntimeEditorEvent ActiveWindowChanged;

		// Token: 0x040003DF RID: 991
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static RuntimeEditorEvent PointerOverWindowChanged;

		// Token: 0x040003E0 RID: 992
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static RuntimeEditorEvent IsOpenedChanged;

		// Token: 0x040003E1 RID: 993
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static RuntimeEditorEvent ActiveSceneCameraChanged;

		// Token: 0x040003E2 RID: 994
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static RuntimeEditorEvent SaveSelectedObjectsRequired;

		// Token: 0x040003E3 RID: 995
		private static List<RuntimeEditorWindow> m_windows = new List<RuntimeEditorWindow>();

		// Token: 0x040003E4 RID: 996
		private static RuntimeEditorWindow m_pointerOverWindow;

		// Token: 0x040003E5 RID: 997
		private static RuntimeEditorWindow m_activeWindow;

		// Token: 0x040003E6 RID: 998
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Camera[] <GameCameras>k__BackingField;

		// Token: 0x040003E7 RID: 999
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Camera[] <SceneCameras>k__BackingField;

		// Token: 0x040003E8 RID: 1000
		private static int m_activeCameraIndex;

		// Token: 0x040003E9 RID: 1001
		private static bool m_isOpened;

		// Token: 0x040003EA RID: 1002
		private static bool m_isPlayModeStateChanging;

		// Token: 0x040003EB RID: 1003
		private static bool m_isPlaying;

		// Token: 0x02000EA3 RID: 3747
		[CompilerGenerated]
		private sealed class <GetWindow>c__AnonStorey0
		{
			// Token: 0x06007161 RID: 29025 RVA: 0x00014E5A File Offset: 0x0001325A
			public <GetWindow>c__AnonStorey0()
			{
			}

			// Token: 0x06007162 RID: 29026 RVA: 0x00014E62 File Offset: 0x00013262
			internal bool <>m__0(RuntimeEditorWindow wnd)
			{
				return wnd != null && wnd.WindowType == this.type;
			}

			// Token: 0x04006536 RID: 25910
			internal RuntimeWindowType type;
		}
	}
}
