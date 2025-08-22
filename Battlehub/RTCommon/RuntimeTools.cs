using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Battlehub.RTCommon
{
	// Token: 0x020000CD RID: 205
	public static class RuntimeTools
	{
		// Token: 0x060003A0 RID: 928 RVA: 0x00015F40 File Offset: 0x00014340
		static RuntimeTools()
		{
			RuntimeTools.Reset();
		}

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x060003A1 RID: 929 RVA: 0x00015F50 File Offset: 0x00014350
		// (remove) Token: 0x060003A2 RID: 930 RVA: 0x00015F84 File Offset: 0x00014384
		public static event RuntimeToolsEvent ToolChanged
		{
			add
			{
				RuntimeToolsEvent runtimeToolsEvent = RuntimeTools.ToolChanged;
				RuntimeToolsEvent runtimeToolsEvent2;
				do
				{
					runtimeToolsEvent2 = runtimeToolsEvent;
					runtimeToolsEvent = Interlocked.CompareExchange<RuntimeToolsEvent>(ref RuntimeTools.ToolChanged, (RuntimeToolsEvent)Delegate.Combine(runtimeToolsEvent2, value), runtimeToolsEvent);
				}
				while (runtimeToolsEvent != runtimeToolsEvent2);
			}
			remove
			{
				RuntimeToolsEvent runtimeToolsEvent = RuntimeTools.ToolChanged;
				RuntimeToolsEvent runtimeToolsEvent2;
				do
				{
					runtimeToolsEvent2 = runtimeToolsEvent;
					runtimeToolsEvent = Interlocked.CompareExchange<RuntimeToolsEvent>(ref RuntimeTools.ToolChanged, (RuntimeToolsEvent)Delegate.Remove(runtimeToolsEvent2, value), runtimeToolsEvent);
				}
				while (runtimeToolsEvent != runtimeToolsEvent2);
			}
		}

		// Token: 0x1400001C RID: 28
		// (add) Token: 0x060003A3 RID: 931 RVA: 0x00015FB8 File Offset: 0x000143B8
		// (remove) Token: 0x060003A4 RID: 932 RVA: 0x00015FEC File Offset: 0x000143EC
		public static event RuntimeToolsEvent PivotRotationChanged
		{
			add
			{
				RuntimeToolsEvent runtimeToolsEvent = RuntimeTools.PivotRotationChanged;
				RuntimeToolsEvent runtimeToolsEvent2;
				do
				{
					runtimeToolsEvent2 = runtimeToolsEvent;
					runtimeToolsEvent = Interlocked.CompareExchange<RuntimeToolsEvent>(ref RuntimeTools.PivotRotationChanged, (RuntimeToolsEvent)Delegate.Combine(runtimeToolsEvent2, value), runtimeToolsEvent);
				}
				while (runtimeToolsEvent != runtimeToolsEvent2);
			}
			remove
			{
				RuntimeToolsEvent runtimeToolsEvent = RuntimeTools.PivotRotationChanged;
				RuntimeToolsEvent runtimeToolsEvent2;
				do
				{
					runtimeToolsEvent2 = runtimeToolsEvent;
					runtimeToolsEvent = Interlocked.CompareExchange<RuntimeToolsEvent>(ref RuntimeTools.PivotRotationChanged, (RuntimeToolsEvent)Delegate.Remove(runtimeToolsEvent2, value), runtimeToolsEvent);
				}
				while (runtimeToolsEvent != runtimeToolsEvent2);
			}
		}

		// Token: 0x1400001D RID: 29
		// (add) Token: 0x060003A5 RID: 933 RVA: 0x00016020 File Offset: 0x00014420
		// (remove) Token: 0x060003A6 RID: 934 RVA: 0x00016054 File Offset: 0x00014454
		public static event RuntimeToolsEvent PivotModeChanged
		{
			add
			{
				RuntimeToolsEvent runtimeToolsEvent = RuntimeTools.PivotModeChanged;
				RuntimeToolsEvent runtimeToolsEvent2;
				do
				{
					runtimeToolsEvent2 = runtimeToolsEvent;
					runtimeToolsEvent = Interlocked.CompareExchange<RuntimeToolsEvent>(ref RuntimeTools.PivotModeChanged, (RuntimeToolsEvent)Delegate.Combine(runtimeToolsEvent2, value), runtimeToolsEvent);
				}
				while (runtimeToolsEvent != runtimeToolsEvent2);
			}
			remove
			{
				RuntimeToolsEvent runtimeToolsEvent = RuntimeTools.PivotModeChanged;
				RuntimeToolsEvent runtimeToolsEvent2;
				do
				{
					runtimeToolsEvent2 = runtimeToolsEvent;
					runtimeToolsEvent = Interlocked.CompareExchange<RuntimeToolsEvent>(ref RuntimeTools.PivotModeChanged, (RuntimeToolsEvent)Delegate.Remove(runtimeToolsEvent2, value), runtimeToolsEvent);
				}
				while (runtimeToolsEvent != runtimeToolsEvent2);
			}
		}

		// Token: 0x1400001E RID: 30
		// (add) Token: 0x060003A7 RID: 935 RVA: 0x00016088 File Offset: 0x00014488
		// (remove) Token: 0x060003A8 RID: 936 RVA: 0x000160BC File Offset: 0x000144BC
		public static event SpawnPrefabChanged SpawnPrefabChanged
		{
			add
			{
				SpawnPrefabChanged spawnPrefabChanged = RuntimeTools.SpawnPrefabChanged;
				SpawnPrefabChanged spawnPrefabChanged2;
				do
				{
					spawnPrefabChanged2 = spawnPrefabChanged;
					spawnPrefabChanged = Interlocked.CompareExchange<SpawnPrefabChanged>(ref RuntimeTools.SpawnPrefabChanged, (SpawnPrefabChanged)Delegate.Combine(spawnPrefabChanged2, value), spawnPrefabChanged);
				}
				while (spawnPrefabChanged != spawnPrefabChanged2);
			}
			remove
			{
				SpawnPrefabChanged spawnPrefabChanged = RuntimeTools.SpawnPrefabChanged;
				SpawnPrefabChanged spawnPrefabChanged2;
				do
				{
					spawnPrefabChanged2 = spawnPrefabChanged;
					spawnPrefabChanged = Interlocked.CompareExchange<SpawnPrefabChanged>(ref RuntimeTools.SpawnPrefabChanged, (SpawnPrefabChanged)Delegate.Remove(spawnPrefabChanged2, value), spawnPrefabChanged);
				}
				while (spawnPrefabChanged != spawnPrefabChanged2);
			}
		}

		// Token: 0x1400001F RID: 31
		// (add) Token: 0x060003A9 RID: 937 RVA: 0x000160F0 File Offset: 0x000144F0
		// (remove) Token: 0x060003AA RID: 938 RVA: 0x00016124 File Offset: 0x00014524
		public static event RuntimeToolsEvent IsViewingChanged
		{
			add
			{
				RuntimeToolsEvent runtimeToolsEvent = RuntimeTools.IsViewingChanged;
				RuntimeToolsEvent runtimeToolsEvent2;
				do
				{
					runtimeToolsEvent2 = runtimeToolsEvent;
					runtimeToolsEvent = Interlocked.CompareExchange<RuntimeToolsEvent>(ref RuntimeTools.IsViewingChanged, (RuntimeToolsEvent)Delegate.Combine(runtimeToolsEvent2, value), runtimeToolsEvent);
				}
				while (runtimeToolsEvent != runtimeToolsEvent2);
			}
			remove
			{
				RuntimeToolsEvent runtimeToolsEvent = RuntimeTools.IsViewingChanged;
				RuntimeToolsEvent runtimeToolsEvent2;
				do
				{
					runtimeToolsEvent2 = runtimeToolsEvent;
					runtimeToolsEvent = Interlocked.CompareExchange<RuntimeToolsEvent>(ref RuntimeTools.IsViewingChanged, (RuntimeToolsEvent)Delegate.Remove(runtimeToolsEvent2, value), runtimeToolsEvent);
				}
				while (runtimeToolsEvent != runtimeToolsEvent2);
			}
		}

		// Token: 0x14000020 RID: 32
		// (add) Token: 0x060003AB RID: 939 RVA: 0x00016158 File Offset: 0x00014558
		// (remove) Token: 0x060003AC RID: 940 RVA: 0x0001618C File Offset: 0x0001458C
		public static event RuntimeToolsEvent ShowSelectionGizmosChanged
		{
			add
			{
				RuntimeToolsEvent runtimeToolsEvent = RuntimeTools.ShowSelectionGizmosChanged;
				RuntimeToolsEvent runtimeToolsEvent2;
				do
				{
					runtimeToolsEvent2 = runtimeToolsEvent;
					runtimeToolsEvent = Interlocked.CompareExchange<RuntimeToolsEvent>(ref RuntimeTools.ShowSelectionGizmosChanged, (RuntimeToolsEvent)Delegate.Combine(runtimeToolsEvent2, value), runtimeToolsEvent);
				}
				while (runtimeToolsEvent != runtimeToolsEvent2);
			}
			remove
			{
				RuntimeToolsEvent runtimeToolsEvent = RuntimeTools.ShowSelectionGizmosChanged;
				RuntimeToolsEvent runtimeToolsEvent2;
				do
				{
					runtimeToolsEvent2 = runtimeToolsEvent;
					runtimeToolsEvent = Interlocked.CompareExchange<RuntimeToolsEvent>(ref RuntimeTools.ShowSelectionGizmosChanged, (RuntimeToolsEvent)Delegate.Remove(runtimeToolsEvent2, value), runtimeToolsEvent);
				}
				while (runtimeToolsEvent != runtimeToolsEvent2);
			}
		}

		// Token: 0x14000021 RID: 33
		// (add) Token: 0x060003AD RID: 941 RVA: 0x000161C0 File Offset: 0x000145C0
		// (remove) Token: 0x060003AE RID: 942 RVA: 0x000161F4 File Offset: 0x000145F4
		public static event RuntimeToolsEvent ShowGizmosChanged
		{
			add
			{
				RuntimeToolsEvent runtimeToolsEvent = RuntimeTools.ShowGizmosChanged;
				RuntimeToolsEvent runtimeToolsEvent2;
				do
				{
					runtimeToolsEvent2 = runtimeToolsEvent;
					runtimeToolsEvent = Interlocked.CompareExchange<RuntimeToolsEvent>(ref RuntimeTools.ShowGizmosChanged, (RuntimeToolsEvent)Delegate.Combine(runtimeToolsEvent2, value), runtimeToolsEvent);
				}
				while (runtimeToolsEvent != runtimeToolsEvent2);
			}
			remove
			{
				RuntimeToolsEvent runtimeToolsEvent = RuntimeTools.ShowGizmosChanged;
				RuntimeToolsEvent runtimeToolsEvent2;
				do
				{
					runtimeToolsEvent2 = runtimeToolsEvent;
					runtimeToolsEvent = Interlocked.CompareExchange<RuntimeToolsEvent>(ref RuntimeTools.ShowGizmosChanged, (RuntimeToolsEvent)Delegate.Remove(runtimeToolsEvent2, value), runtimeToolsEvent);
				}
				while (runtimeToolsEvent != runtimeToolsEvent2);
			}
		}

		// Token: 0x14000022 RID: 34
		// (add) Token: 0x060003AF RID: 943 RVA: 0x00016228 File Offset: 0x00014628
		// (remove) Token: 0x060003B0 RID: 944 RVA: 0x0001625C File Offset: 0x0001465C
		public static event RuntimeToolsEvent AutoFocusChanged
		{
			add
			{
				RuntimeToolsEvent runtimeToolsEvent = RuntimeTools.AutoFocusChanged;
				RuntimeToolsEvent runtimeToolsEvent2;
				do
				{
					runtimeToolsEvent2 = runtimeToolsEvent;
					runtimeToolsEvent = Interlocked.CompareExchange<RuntimeToolsEvent>(ref RuntimeTools.AutoFocusChanged, (RuntimeToolsEvent)Delegate.Combine(runtimeToolsEvent2, value), runtimeToolsEvent);
				}
				while (runtimeToolsEvent != runtimeToolsEvent2);
			}
			remove
			{
				RuntimeToolsEvent runtimeToolsEvent = RuntimeTools.AutoFocusChanged;
				RuntimeToolsEvent runtimeToolsEvent2;
				do
				{
					runtimeToolsEvent2 = runtimeToolsEvent;
					runtimeToolsEvent = Interlocked.CompareExchange<RuntimeToolsEvent>(ref RuntimeTools.AutoFocusChanged, (RuntimeToolsEvent)Delegate.Remove(runtimeToolsEvent2, value), runtimeToolsEvent);
				}
				while (runtimeToolsEvent != runtimeToolsEvent2);
			}
		}

		// Token: 0x14000023 RID: 35
		// (add) Token: 0x060003B1 RID: 945 RVA: 0x00016290 File Offset: 0x00014690
		// (remove) Token: 0x060003B2 RID: 946 RVA: 0x000162C4 File Offset: 0x000146C4
		public static event RuntimeToolsEvent UnitSnappingChanged
		{
			add
			{
				RuntimeToolsEvent runtimeToolsEvent = RuntimeTools.UnitSnappingChanged;
				RuntimeToolsEvent runtimeToolsEvent2;
				do
				{
					runtimeToolsEvent2 = runtimeToolsEvent;
					runtimeToolsEvent = Interlocked.CompareExchange<RuntimeToolsEvent>(ref RuntimeTools.UnitSnappingChanged, (RuntimeToolsEvent)Delegate.Combine(runtimeToolsEvent2, value), runtimeToolsEvent);
				}
				while (runtimeToolsEvent != runtimeToolsEvent2);
			}
			remove
			{
				RuntimeToolsEvent runtimeToolsEvent = RuntimeTools.UnitSnappingChanged;
				RuntimeToolsEvent runtimeToolsEvent2;
				do
				{
					runtimeToolsEvent2 = runtimeToolsEvent;
					runtimeToolsEvent = Interlocked.CompareExchange<RuntimeToolsEvent>(ref RuntimeTools.UnitSnappingChanged, (RuntimeToolsEvent)Delegate.Remove(runtimeToolsEvent2, value), runtimeToolsEvent);
				}
				while (runtimeToolsEvent != runtimeToolsEvent2);
			}
		}

		// Token: 0x14000024 RID: 36
		// (add) Token: 0x060003B3 RID: 947 RVA: 0x000162F8 File Offset: 0x000146F8
		// (remove) Token: 0x060003B4 RID: 948 RVA: 0x0001632C File Offset: 0x0001472C
		public static event RuntimeToolsEvent IsSnappingChanged
		{
			add
			{
				RuntimeToolsEvent runtimeToolsEvent = RuntimeTools.IsSnappingChanged;
				RuntimeToolsEvent runtimeToolsEvent2;
				do
				{
					runtimeToolsEvent2 = runtimeToolsEvent;
					runtimeToolsEvent = Interlocked.CompareExchange<RuntimeToolsEvent>(ref RuntimeTools.IsSnappingChanged, (RuntimeToolsEvent)Delegate.Combine(runtimeToolsEvent2, value), runtimeToolsEvent);
				}
				while (runtimeToolsEvent != runtimeToolsEvent2);
			}
			remove
			{
				RuntimeToolsEvent runtimeToolsEvent = RuntimeTools.IsSnappingChanged;
				RuntimeToolsEvent runtimeToolsEvent2;
				do
				{
					runtimeToolsEvent2 = runtimeToolsEvent;
					runtimeToolsEvent = Interlocked.CompareExchange<RuntimeToolsEvent>(ref RuntimeTools.IsSnappingChanged, (RuntimeToolsEvent)Delegate.Remove(runtimeToolsEvent2, value), runtimeToolsEvent);
				}
				while (runtimeToolsEvent != runtimeToolsEvent2);
			}
		}

		// Token: 0x14000025 RID: 37
		// (add) Token: 0x060003B5 RID: 949 RVA: 0x00016360 File Offset: 0x00014760
		// (remove) Token: 0x060003B6 RID: 950 RVA: 0x00016394 File Offset: 0x00014794
		public static event RuntimeToolsEvent SnappingModeChanged
		{
			add
			{
				RuntimeToolsEvent runtimeToolsEvent = RuntimeTools.SnappingModeChanged;
				RuntimeToolsEvent runtimeToolsEvent2;
				do
				{
					runtimeToolsEvent2 = runtimeToolsEvent;
					runtimeToolsEvent = Interlocked.CompareExchange<RuntimeToolsEvent>(ref RuntimeTools.SnappingModeChanged, (RuntimeToolsEvent)Delegate.Combine(runtimeToolsEvent2, value), runtimeToolsEvent);
				}
				while (runtimeToolsEvent != runtimeToolsEvent2);
			}
			remove
			{
				RuntimeToolsEvent runtimeToolsEvent = RuntimeTools.SnappingModeChanged;
				RuntimeToolsEvent runtimeToolsEvent2;
				do
				{
					runtimeToolsEvent2 = runtimeToolsEvent;
					runtimeToolsEvent = Interlocked.CompareExchange<RuntimeToolsEvent>(ref RuntimeTools.SnappingModeChanged, (RuntimeToolsEvent)Delegate.Remove(runtimeToolsEvent2, value), runtimeToolsEvent);
				}
				while (runtimeToolsEvent != runtimeToolsEvent2);
			}
		}

		// Token: 0x14000026 RID: 38
		// (add) Token: 0x060003B7 RID: 951 RVA: 0x000163C8 File Offset: 0x000147C8
		// (remove) Token: 0x060003B8 RID: 952 RVA: 0x000163FC File Offset: 0x000147FC
		public static event RuntimeToolsEvent LockAxesChanged
		{
			add
			{
				RuntimeToolsEvent runtimeToolsEvent = RuntimeTools.LockAxesChanged;
				RuntimeToolsEvent runtimeToolsEvent2;
				do
				{
					runtimeToolsEvent2 = runtimeToolsEvent;
					runtimeToolsEvent = Interlocked.CompareExchange<RuntimeToolsEvent>(ref RuntimeTools.LockAxesChanged, (RuntimeToolsEvent)Delegate.Combine(runtimeToolsEvent2, value), runtimeToolsEvent);
				}
				while (runtimeToolsEvent != runtimeToolsEvent2);
			}
			remove
			{
				RuntimeToolsEvent runtimeToolsEvent = RuntimeTools.LockAxesChanged;
				RuntimeToolsEvent runtimeToolsEvent2;
				do
				{
					runtimeToolsEvent2 = runtimeToolsEvent;
					runtimeToolsEvent = Interlocked.CompareExchange<RuntimeToolsEvent>(ref RuntimeTools.LockAxesChanged, (RuntimeToolsEvent)Delegate.Remove(runtimeToolsEvent2, value), runtimeToolsEvent);
				}
				while (runtimeToolsEvent != runtimeToolsEvent2);
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060003B9 RID: 953 RVA: 0x00016430 File Offset: 0x00014830
		// (set) Token: 0x060003BA RID: 954 RVA: 0x00016437 File Offset: 0x00014837
		public static bool IsViewing
		{
			get
			{
				return RuntimeTools.m_isViewing;
			}
			set
			{
				if (RuntimeTools.m_isViewing != value)
				{
					RuntimeTools.m_isViewing = value;
					if (RuntimeTools.m_isViewing)
					{
						RuntimeTools.ActiveTool = null;
					}
					if (RuntimeTools.IsViewingChanged != null)
					{
						RuntimeTools.IsViewingChanged();
					}
				}
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060003BB RID: 955 RVA: 0x0001646E File Offset: 0x0001486E
		// (set) Token: 0x060003BC RID: 956 RVA: 0x00016475 File Offset: 0x00014875
		public static bool ShowSelectionGizmos
		{
			get
			{
				return RuntimeTools.m_showSelectionGizmos;
			}
			set
			{
				if (RuntimeTools.m_showSelectionGizmos != value)
				{
					RuntimeTools.m_showSelectionGizmos = value;
					if (RuntimeTools.ShowSelectionGizmosChanged != null)
					{
						RuntimeTools.ShowSelectionGizmosChanged();
					}
				}
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060003BD RID: 957 RVA: 0x0001649C File Offset: 0x0001489C
		// (set) Token: 0x060003BE RID: 958 RVA: 0x000164A3 File Offset: 0x000148A3
		public static bool ShowGizmos
		{
			get
			{
				return RuntimeTools.m_showGizmos;
			}
			set
			{
				if (RuntimeTools.m_showGizmos != value)
				{
					RuntimeTools.m_showGizmos = value;
					if (RuntimeTools.ShowGizmosChanged != null)
					{
						RuntimeTools.ShowGizmosChanged();
					}
				}
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060003BF RID: 959 RVA: 0x000164CA File Offset: 0x000148CA
		// (set) Token: 0x060003C0 RID: 960 RVA: 0x000164D1 File Offset: 0x000148D1
		public static bool AutoFocus
		{
			get
			{
				return RuntimeTools.m_autoFocus;
			}
			set
			{
				if (RuntimeTools.m_autoFocus != value)
				{
					RuntimeTools.m_autoFocus = value;
					if (RuntimeTools.AutoFocusChanged != null)
					{
						RuntimeTools.AutoFocusChanged();
					}
				}
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060003C1 RID: 961 RVA: 0x000164F8 File Offset: 0x000148F8
		// (set) Token: 0x060003C2 RID: 962 RVA: 0x000164FF File Offset: 0x000148FF
		public static bool UnitSnapping
		{
			get
			{
				return RuntimeTools.m_unitSnapping;
			}
			set
			{
				if (RuntimeTools.m_unitSnapping != value)
				{
					RuntimeTools.m_unitSnapping = value;
					if (RuntimeTools.UnitSnappingChanged != null)
					{
						RuntimeTools.UnitSnappingChanged();
					}
				}
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060003C3 RID: 963 RVA: 0x00016526 File Offset: 0x00014926
		// (set) Token: 0x060003C4 RID: 964 RVA: 0x0001652D File Offset: 0x0001492D
		public static bool IsSnapping
		{
			get
			{
				return RuntimeTools.m_isSnapping;
			}
			set
			{
				if (RuntimeTools.m_isSnapping != value)
				{
					RuntimeTools.m_isSnapping = value;
					if (RuntimeTools.IsSnappingChanged != null)
					{
						RuntimeTools.IsSnappingChanged();
					}
				}
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060003C5 RID: 965 RVA: 0x00016554 File Offset: 0x00014954
		// (set) Token: 0x060003C6 RID: 966 RVA: 0x0001655B File Offset: 0x0001495B
		public static SnappingMode SnappingMode
		{
			get
			{
				return RuntimeTools.m_snappingMode;
			}
			set
			{
				if (RuntimeTools.m_snappingMode != value)
				{
					RuntimeTools.m_snappingMode = value;
					if (RuntimeTools.SnappingModeChanged != null)
					{
						RuntimeTools.SnappingModeChanged();
					}
				}
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060003C7 RID: 967 RVA: 0x00016582 File Offset: 0x00014982
		// (set) Token: 0x060003C8 RID: 968 RVA: 0x0001658C File Offset: 0x0001498C
		public static GameObject SpawnPrefab
		{
			get
			{
				return RuntimeTools.m_spawnPrefab;
			}
			set
			{
				if (RuntimeTools.m_spawnPrefab != value)
				{
					GameObject spawnPrefab = RuntimeTools.m_spawnPrefab;
					RuntimeTools.m_spawnPrefab = value;
					if (RuntimeTools.SpawnPrefabChanged != null)
					{
						RuntimeTools.SpawnPrefabChanged(spawnPrefab);
					}
				}
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060003C9 RID: 969 RVA: 0x000165CA File Offset: 0x000149CA
		// (set) Token: 0x060003CA RID: 970 RVA: 0x000165D1 File Offset: 0x000149D1
		public static UnityEngine.Object ActiveTool
		{
			[CompilerGenerated]
			get
			{
				return RuntimeTools.<ActiveTool>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				RuntimeTools.<ActiveTool>k__BackingField = value;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060003CB RID: 971 RVA: 0x000165D9 File Offset: 0x000149D9
		// (set) Token: 0x060003CC RID: 972 RVA: 0x000165E0 File Offset: 0x000149E0
		public static LockObject LockAxes
		{
			get
			{
				return RuntimeTools.m_lockAxes;
			}
			set
			{
				if (RuntimeTools.m_lockAxes != value)
				{
					RuntimeTools.m_lockAxes = value;
					if (RuntimeTools.LockAxesChanged != null)
					{
						RuntimeTools.LockAxesChanged();
					}
				}
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060003CD RID: 973 RVA: 0x00016607 File Offset: 0x00014A07
		// (set) Token: 0x060003CE RID: 974 RVA: 0x0001660E File Offset: 0x00014A0E
		public static RuntimeTool Current
		{
			get
			{
				return RuntimeTools.m_current;
			}
			set
			{
				if (RuntimeTools.m_current != value)
				{
					RuntimeTools.m_current = value;
					if (RuntimeTools.ToolChanged != null)
					{
						RuntimeTools.ToolChanged();
					}
				}
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060003CF RID: 975 RVA: 0x00016635 File Offset: 0x00014A35
		// (set) Token: 0x060003D0 RID: 976 RVA: 0x0001663C File Offset: 0x00014A3C
		public static RuntimePivotRotation PivotRotation
		{
			get
			{
				return RuntimeTools.m_pivotRotation;
			}
			set
			{
				if (RuntimeTools.m_pivotRotation != value)
				{
					RuntimeTools.m_pivotRotation = value;
					if (RuntimeTools.PivotRotationChanged != null)
					{
						RuntimeTools.PivotRotationChanged();
					}
				}
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060003D1 RID: 977 RVA: 0x00016663 File Offset: 0x00014A63
		// (set) Token: 0x060003D2 RID: 978 RVA: 0x0001666A File Offset: 0x00014A6A
		public static RuntimePivotMode PivotMode
		{
			get
			{
				return RuntimeTools.m_pivotMode;
			}
			set
			{
				if (RuntimeTools.m_pivotMode != value)
				{
					RuntimeTools.m_pivotMode = value;
					if (RuntimeTools.PivotModeChanged != null)
					{
						RuntimeTools.PivotModeChanged();
					}
				}
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060003D3 RID: 979 RVA: 0x00016691 File Offset: 0x00014A91
		// (set) Token: 0x060003D4 RID: 980 RVA: 0x00016698 File Offset: 0x00014A98
		public static bool DrawSelectionGizmoRay
		{
			[CompilerGenerated]
			get
			{
				return RuntimeTools.<DrawSelectionGizmoRay>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				RuntimeTools.<DrawSelectionGizmoRay>k__BackingField = value;
			}
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x000166A0 File Offset: 0x00014AA0
		public static void Reset()
		{
			RuntimeTools.ActiveTool = null;
			RuntimeTools.LockAxes = null;
			RuntimeTools.m_isViewing = false;
			RuntimeTools.m_isSnapping = false;
			RuntimeTools.m_showSelectionGizmos = true;
			RuntimeTools.m_showGizmos = true;
			RuntimeTools.m_unitSnapping = false;
			RuntimeTools.m_pivotMode = RuntimePivotMode.Center;
			RuntimeTools.SpawnPrefab = null;
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x000166D8 File Offset: 0x00014AD8
		public static bool IsPointerOverGameObject()
		{
			return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
		}

		// Token: 0x0400040F RID: 1039
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static RuntimeToolsEvent ToolChanged;

		// Token: 0x04000410 RID: 1040
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static RuntimeToolsEvent PivotRotationChanged;

		// Token: 0x04000411 RID: 1041
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static RuntimeToolsEvent PivotModeChanged;

		// Token: 0x04000412 RID: 1042
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static SpawnPrefabChanged SpawnPrefabChanged;

		// Token: 0x04000413 RID: 1043
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static RuntimeToolsEvent IsViewingChanged;

		// Token: 0x04000414 RID: 1044
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static RuntimeToolsEvent ShowSelectionGizmosChanged;

		// Token: 0x04000415 RID: 1045
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static RuntimeToolsEvent ShowGizmosChanged;

		// Token: 0x04000416 RID: 1046
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static RuntimeToolsEvent AutoFocusChanged;

		// Token: 0x04000417 RID: 1047
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static RuntimeToolsEvent UnitSnappingChanged;

		// Token: 0x04000418 RID: 1048
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static RuntimeToolsEvent IsSnappingChanged;

		// Token: 0x04000419 RID: 1049
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static RuntimeToolsEvent SnappingModeChanged;

		// Token: 0x0400041A RID: 1050
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static RuntimeToolsEvent LockAxesChanged;

		// Token: 0x0400041B RID: 1051
		private static RuntimeTool m_current;

		// Token: 0x0400041C RID: 1052
		private static RuntimePivotMode m_pivotMode;

		// Token: 0x0400041D RID: 1053
		private static RuntimePivotRotation m_pivotRotation;

		// Token: 0x0400041E RID: 1054
		private static bool m_isViewing;

		// Token: 0x0400041F RID: 1055
		private static bool m_showSelectionGizmos;

		// Token: 0x04000420 RID: 1056
		private static bool m_showGizmos;

		// Token: 0x04000421 RID: 1057
		private static bool m_autoFocus;

		// Token: 0x04000422 RID: 1058
		private static bool m_unitSnapping;

		// Token: 0x04000423 RID: 1059
		private static bool m_isSnapping;

		// Token: 0x04000424 RID: 1060
		private static SnappingMode m_snappingMode = SnappingMode.Vertex;

		// Token: 0x04000425 RID: 1061
		private static GameObject m_spawnPrefab;

		// Token: 0x04000426 RID: 1062
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static UnityEngine.Object <ActiveTool>k__BackingField;

		// Token: 0x04000427 RID: 1063
		public static LockObject m_lockAxes;

		// Token: 0x04000428 RID: 1064
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static bool <DrawSelectionGizmoRay>k__BackingField;
	}
}
