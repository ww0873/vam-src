using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace Battlehub.RTCommon
{
	// Token: 0x020000C6 RID: 198
	public class RuntimeSelection
	{
		// Token: 0x06000387 RID: 903 RVA: 0x00015C2B File Offset: 0x0001402B
		public RuntimeSelection()
		{
		}

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x06000388 RID: 904 RVA: 0x00015C34 File Offset: 0x00014034
		// (remove) Token: 0x06000389 RID: 905 RVA: 0x00015C68 File Offset: 0x00014068
		public static event RuntimeSelectionChanged SelectionChanged
		{
			add
			{
				RuntimeSelectionChanged runtimeSelectionChanged = RuntimeSelection.SelectionChanged;
				RuntimeSelectionChanged runtimeSelectionChanged2;
				do
				{
					runtimeSelectionChanged2 = runtimeSelectionChanged;
					runtimeSelectionChanged = Interlocked.CompareExchange<RuntimeSelectionChanged>(ref RuntimeSelection.SelectionChanged, (RuntimeSelectionChanged)Delegate.Combine(runtimeSelectionChanged2, value), runtimeSelectionChanged);
				}
				while (runtimeSelectionChanged != runtimeSelectionChanged2);
			}
			remove
			{
				RuntimeSelectionChanged runtimeSelectionChanged = RuntimeSelection.SelectionChanged;
				RuntimeSelectionChanged runtimeSelectionChanged2;
				do
				{
					runtimeSelectionChanged2 = runtimeSelectionChanged;
					runtimeSelectionChanged = Interlocked.CompareExchange<RuntimeSelectionChanged>(ref RuntimeSelection.SelectionChanged, (RuntimeSelectionChanged)Delegate.Remove(runtimeSelectionChanged2, value), runtimeSelectionChanged);
				}
				while (runtimeSelectionChanged != runtimeSelectionChanged2);
			}
		}

		// Token: 0x0600038A RID: 906 RVA: 0x00015C9C File Offset: 0x0001409C
		protected static void RaiseSelectionChanged(UnityEngine.Object[] unselectedObjects)
		{
			if (RuntimeSelection.SelectionChanged != null)
			{
				RuntimeSelection.SelectionChanged(unselectedObjects);
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600038B RID: 907 RVA: 0x00015CB3 File Offset: 0x000140B3
		// (set) Token: 0x0600038C RID: 908 RVA: 0x00015CBF File Offset: 0x000140BF
		public static GameObject activeGameObject
		{
			get
			{
				return RuntimeSelection.activeObject as GameObject;
			}
			set
			{
				RuntimeSelection.activeObject = value;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600038D RID: 909 RVA: 0x00015CC7 File Offset: 0x000140C7
		// (set) Token: 0x0600038E RID: 910 RVA: 0x00015CD0 File Offset: 0x000140D0
		public static UnityEngine.Object activeObject
		{
			get
			{
				return RuntimeSelection.m_activeObject;
			}
			set
			{
				if (RuntimeSelection.m_activeObject != value || (value != null && RuntimeSelection.m_objects != null && RuntimeSelection.m_objects.Length > 1))
				{
					RuntimeUndo.RecordSelection();
					RuntimeSelection.m_activeObject = value;
					UnityEngine.Object[] objects = RuntimeSelection.m_objects;
					if (RuntimeSelection.m_activeObject != null)
					{
						RuntimeSelection.m_objects = new UnityEngine.Object[]
						{
							value
						};
					}
					else
					{
						RuntimeSelection.m_objects = new UnityEngine.Object[0];
					}
					RuntimeSelection.UpdateHS();
					RuntimeUndo.RecordSelection();
					RuntimeSelection.RaiseSelectionChanged(objects);
				}
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x0600038F RID: 911 RVA: 0x00015D60 File Offset: 0x00014160
		// (set) Token: 0x06000390 RID: 912 RVA: 0x00015D67 File Offset: 0x00014167
		public static UnityEngine.Object[] objects
		{
			get
			{
				return RuntimeSelection.m_objects;
			}
			set
			{
				if (RuntimeSelection.IsSelectionChanged(value))
				{
					RuntimeUndo.RecordSelection();
					RuntimeSelection.SetObjects(value);
					RuntimeUndo.RecordSelection();
				}
			}
		}

		// Token: 0x06000391 RID: 913 RVA: 0x00015D84 File Offset: 0x00014184
		public static bool IsSelected(UnityEngine.Object obj)
		{
			return RuntimeSelection.m_selectionHS != null && RuntimeSelection.m_selectionHS.Contains(obj);
		}

		// Token: 0x06000392 RID: 914 RVA: 0x00015D9D File Offset: 0x0001419D
		private static void UpdateHS()
		{
			if (RuntimeSelection.m_objects != null)
			{
				RuntimeSelection.m_selectionHS = new HashSet<UnityEngine.Object>(RuntimeSelection.m_objects);
			}
			else
			{
				RuntimeSelection.m_selectionHS = null;
			}
		}

		// Token: 0x06000393 RID: 915 RVA: 0x00015DC4 File Offset: 0x000141C4
		private static bool IsSelectionChanged(UnityEngine.Object[] value)
		{
			if (RuntimeSelection.m_objects == value)
			{
				return false;
			}
			if (RuntimeSelection.m_objects == null)
			{
				return value.Length != 0;
			}
			if (value == null)
			{
				return RuntimeSelection.m_objects.Length != 0;
			}
			if (RuntimeSelection.m_objects.Length != value.Length)
			{
				return true;
			}
			for (int i = 0; i < RuntimeSelection.m_objects.Length; i++)
			{
				if (RuntimeSelection.m_objects[i] != value[i])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000394 RID: 916 RVA: 0x00015E48 File Offset: 0x00014248
		protected static void SetObjects(UnityEngine.Object[] value)
		{
			if (!RuntimeSelection.IsSelectionChanged(value))
			{
				return;
			}
			UnityEngine.Object[] objects = RuntimeSelection.m_objects;
			if (value == null)
			{
				RuntimeSelection.m_objects = null;
				RuntimeSelection.m_activeObject = null;
			}
			else
			{
				RuntimeSelection.m_objects = value.ToArray<UnityEngine.Object>();
				if (RuntimeSelection.m_activeObject == null || !RuntimeSelection.m_objects.Contains(RuntimeSelection.m_activeObject))
				{
					RuntimeSelection.m_activeObject = RuntimeSelection.m_objects.OfType<UnityEngine.Object>().FirstOrDefault<UnityEngine.Object>();
				}
			}
			RuntimeSelection.UpdateHS();
			RuntimeSelection.RaiseSelectionChanged(objects);
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000395 RID: 917 RVA: 0x00015ECC File Offset: 0x000142CC
		public static GameObject[] gameObjects
		{
			get
			{
				if (RuntimeSelection.m_objects == null)
				{
					return null;
				}
				return RuntimeSelection.m_objects.OfType<GameObject>().ToArray<GameObject>();
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000396 RID: 918 RVA: 0x00015EE9 File Offset: 0x000142E9
		public static Transform activeTransform
		{
			get
			{
				if (RuntimeSelection.m_activeObject == null)
				{
					return null;
				}
				if (RuntimeSelection.m_activeObject is GameObject)
				{
					return ((GameObject)RuntimeSelection.m_activeObject).transform;
				}
				return null;
			}
		}

		// Token: 0x06000397 RID: 919 RVA: 0x00015F1D File Offset: 0x0001431D
		public static void Select(UnityEngine.Object activeObject, UnityEngine.Object[] selection)
		{
			if (RuntimeSelection.IsSelectionChanged(selection))
			{
				RuntimeUndo.RecordSelection();
				RuntimeSelection.m_activeObject = activeObject;
				RuntimeSelection.SetObjects(selection);
				RuntimeUndo.RecordSelection();
			}
		}

		// Token: 0x040003FC RID: 1020
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static RuntimeSelectionChanged SelectionChanged;

		// Token: 0x040003FD RID: 1021
		private static HashSet<UnityEngine.Object> m_selectionHS;

		// Token: 0x040003FE RID: 1022
		protected static UnityEngine.Object m_activeObject;

		// Token: 0x040003FF RID: 1023
		protected static UnityEngine.Object[] m_objects;
	}
}
