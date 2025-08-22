using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace Battlehub.RTCommon
{
	// Token: 0x020000D3 RID: 211
	public static class RuntimeUndo
	{
		// Token: 0x060003F2 RID: 1010 RVA: 0x000169B9 File Offset: 0x00014DB9
		static RuntimeUndo()
		{
			RuntimeUndo.Reset();
		}

		// Token: 0x14000027 RID: 39
		// (add) Token: 0x060003F3 RID: 1011 RVA: 0x000169C0 File Offset: 0x00014DC0
		// (remove) Token: 0x060003F4 RID: 1012 RVA: 0x000169F4 File Offset: 0x00014DF4
		public static event RuntimeUndoEventHandler BeforeUndo
		{
			add
			{
				RuntimeUndoEventHandler runtimeUndoEventHandler = RuntimeUndo.BeforeUndo;
				RuntimeUndoEventHandler runtimeUndoEventHandler2;
				do
				{
					runtimeUndoEventHandler2 = runtimeUndoEventHandler;
					runtimeUndoEventHandler = Interlocked.CompareExchange<RuntimeUndoEventHandler>(ref RuntimeUndo.BeforeUndo, (RuntimeUndoEventHandler)Delegate.Combine(runtimeUndoEventHandler2, value), runtimeUndoEventHandler);
				}
				while (runtimeUndoEventHandler != runtimeUndoEventHandler2);
			}
			remove
			{
				RuntimeUndoEventHandler runtimeUndoEventHandler = RuntimeUndo.BeforeUndo;
				RuntimeUndoEventHandler runtimeUndoEventHandler2;
				do
				{
					runtimeUndoEventHandler2 = runtimeUndoEventHandler;
					runtimeUndoEventHandler = Interlocked.CompareExchange<RuntimeUndoEventHandler>(ref RuntimeUndo.BeforeUndo, (RuntimeUndoEventHandler)Delegate.Remove(runtimeUndoEventHandler2, value), runtimeUndoEventHandler);
				}
				while (runtimeUndoEventHandler != runtimeUndoEventHandler2);
			}
		}

		// Token: 0x14000028 RID: 40
		// (add) Token: 0x060003F5 RID: 1013 RVA: 0x00016A28 File Offset: 0x00014E28
		// (remove) Token: 0x060003F6 RID: 1014 RVA: 0x00016A5C File Offset: 0x00014E5C
		public static event RuntimeUndoEventHandler UndoCompleted
		{
			add
			{
				RuntimeUndoEventHandler runtimeUndoEventHandler = RuntimeUndo.UndoCompleted;
				RuntimeUndoEventHandler runtimeUndoEventHandler2;
				do
				{
					runtimeUndoEventHandler2 = runtimeUndoEventHandler;
					runtimeUndoEventHandler = Interlocked.CompareExchange<RuntimeUndoEventHandler>(ref RuntimeUndo.UndoCompleted, (RuntimeUndoEventHandler)Delegate.Combine(runtimeUndoEventHandler2, value), runtimeUndoEventHandler);
				}
				while (runtimeUndoEventHandler != runtimeUndoEventHandler2);
			}
			remove
			{
				RuntimeUndoEventHandler runtimeUndoEventHandler = RuntimeUndo.UndoCompleted;
				RuntimeUndoEventHandler runtimeUndoEventHandler2;
				do
				{
					runtimeUndoEventHandler2 = runtimeUndoEventHandler;
					runtimeUndoEventHandler = Interlocked.CompareExchange<RuntimeUndoEventHandler>(ref RuntimeUndo.UndoCompleted, (RuntimeUndoEventHandler)Delegate.Remove(runtimeUndoEventHandler2, value), runtimeUndoEventHandler);
				}
				while (runtimeUndoEventHandler != runtimeUndoEventHandler2);
			}
		}

		// Token: 0x14000029 RID: 41
		// (add) Token: 0x060003F7 RID: 1015 RVA: 0x00016A90 File Offset: 0x00014E90
		// (remove) Token: 0x060003F8 RID: 1016 RVA: 0x00016AC4 File Offset: 0x00014EC4
		public static event RuntimeUndoEventHandler BeforeRedo
		{
			add
			{
				RuntimeUndoEventHandler runtimeUndoEventHandler = RuntimeUndo.BeforeRedo;
				RuntimeUndoEventHandler runtimeUndoEventHandler2;
				do
				{
					runtimeUndoEventHandler2 = runtimeUndoEventHandler;
					runtimeUndoEventHandler = Interlocked.CompareExchange<RuntimeUndoEventHandler>(ref RuntimeUndo.BeforeRedo, (RuntimeUndoEventHandler)Delegate.Combine(runtimeUndoEventHandler2, value), runtimeUndoEventHandler);
				}
				while (runtimeUndoEventHandler != runtimeUndoEventHandler2);
			}
			remove
			{
				RuntimeUndoEventHandler runtimeUndoEventHandler = RuntimeUndo.BeforeRedo;
				RuntimeUndoEventHandler runtimeUndoEventHandler2;
				do
				{
					runtimeUndoEventHandler2 = runtimeUndoEventHandler;
					runtimeUndoEventHandler = Interlocked.CompareExchange<RuntimeUndoEventHandler>(ref RuntimeUndo.BeforeRedo, (RuntimeUndoEventHandler)Delegate.Remove(runtimeUndoEventHandler2, value), runtimeUndoEventHandler);
				}
				while (runtimeUndoEventHandler != runtimeUndoEventHandler2);
			}
		}

		// Token: 0x1400002A RID: 42
		// (add) Token: 0x060003F9 RID: 1017 RVA: 0x00016AF8 File Offset: 0x00014EF8
		// (remove) Token: 0x060003FA RID: 1018 RVA: 0x00016B2C File Offset: 0x00014F2C
		public static event RuntimeUndoEventHandler RedoCompleted
		{
			add
			{
				RuntimeUndoEventHandler runtimeUndoEventHandler = RuntimeUndo.RedoCompleted;
				RuntimeUndoEventHandler runtimeUndoEventHandler2;
				do
				{
					runtimeUndoEventHandler2 = runtimeUndoEventHandler;
					runtimeUndoEventHandler = Interlocked.CompareExchange<RuntimeUndoEventHandler>(ref RuntimeUndo.RedoCompleted, (RuntimeUndoEventHandler)Delegate.Combine(runtimeUndoEventHandler2, value), runtimeUndoEventHandler);
				}
				while (runtimeUndoEventHandler != runtimeUndoEventHandler2);
			}
			remove
			{
				RuntimeUndoEventHandler runtimeUndoEventHandler = RuntimeUndo.RedoCompleted;
				RuntimeUndoEventHandler runtimeUndoEventHandler2;
				do
				{
					runtimeUndoEventHandler2 = runtimeUndoEventHandler;
					runtimeUndoEventHandler = Interlocked.CompareExchange<RuntimeUndoEventHandler>(ref RuntimeUndo.RedoCompleted, (RuntimeUndoEventHandler)Delegate.Remove(runtimeUndoEventHandler2, value), runtimeUndoEventHandler);
				}
				while (runtimeUndoEventHandler != runtimeUndoEventHandler2);
			}
		}

		// Token: 0x1400002B RID: 43
		// (add) Token: 0x060003FB RID: 1019 RVA: 0x00016B60 File Offset: 0x00014F60
		// (remove) Token: 0x060003FC RID: 1020 RVA: 0x00016B94 File Offset: 0x00014F94
		public static event RuntimeUndoEventHandler StateChanged
		{
			add
			{
				RuntimeUndoEventHandler runtimeUndoEventHandler = RuntimeUndo.StateChanged;
				RuntimeUndoEventHandler runtimeUndoEventHandler2;
				do
				{
					runtimeUndoEventHandler2 = runtimeUndoEventHandler;
					runtimeUndoEventHandler = Interlocked.CompareExchange<RuntimeUndoEventHandler>(ref RuntimeUndo.StateChanged, (RuntimeUndoEventHandler)Delegate.Combine(runtimeUndoEventHandler2, value), runtimeUndoEventHandler);
				}
				while (runtimeUndoEventHandler != runtimeUndoEventHandler2);
			}
			remove
			{
				RuntimeUndoEventHandler runtimeUndoEventHandler = RuntimeUndo.StateChanged;
				RuntimeUndoEventHandler runtimeUndoEventHandler2;
				do
				{
					runtimeUndoEventHandler2 = runtimeUndoEventHandler;
					runtimeUndoEventHandler = Interlocked.CompareExchange<RuntimeUndoEventHandler>(ref RuntimeUndo.StateChanged, (RuntimeUndoEventHandler)Delegate.Remove(runtimeUndoEventHandler2, value), runtimeUndoEventHandler);
				}
				while (runtimeUndoEventHandler != runtimeUndoEventHandler2);
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060003FD RID: 1021 RVA: 0x00016BC8 File Offset: 0x00014FC8
		// (set) Token: 0x060003FE RID: 1022 RVA: 0x00016BCF File Offset: 0x00014FCF
		public static bool Enabled
		{
			[CompilerGenerated]
			get
			{
				return RuntimeUndo.<Enabled>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				RuntimeUndo.<Enabled>k__BackingField = value;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060003FF RID: 1023 RVA: 0x00016BD7 File Offset: 0x00014FD7
		public static bool CanUndo
		{
			get
			{
				return RuntimeUndo.m_stack.CanPop;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000400 RID: 1024 RVA: 0x00016BE3 File Offset: 0x00014FE3
		public static bool CanRedo
		{
			get
			{
				return RuntimeUndo.m_stack.CanRestore;
			}
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x00016BEF File Offset: 0x00014FEF
		public static void Reset()
		{
			RuntimeUndo.Enabled = true;
			RuntimeUndo.m_group = null;
			RuntimeUndo.m_stack = new UndoStack<Record[]>(8192);
			RuntimeUndo.m_stacks = new Stack<UndoStack<Record[]>>();
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000402 RID: 1026 RVA: 0x00016C16 File Offset: 0x00015016
		public static bool IsRecording
		{
			get
			{
				return RuntimeUndo.m_group != null;
			}
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x00016C23 File Offset: 0x00015023
		public static void BeginRecord()
		{
			if (!RuntimeUndo.Enabled)
			{
				return;
			}
			RuntimeUndo.m_group = new List<Record>();
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x00016C3C File Offset: 0x0001503C
		public static void EndRecord()
		{
			if (!RuntimeUndo.Enabled)
			{
				return;
			}
			if (RuntimeUndo.m_group != null)
			{
				Record[] array = RuntimeUndo.m_stack.Push(RuntimeUndo.m_group.ToArray());
				if (array != null)
				{
					for (int i = 0; i < array.Length; i++)
					{
						array[i].Purge();
					}
				}
				if (RuntimeUndo.StateChanged != null)
				{
					RuntimeUndo.StateChanged();
				}
			}
			RuntimeUndo.m_group = null;
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x00016CB0 File Offset: 0x000150B0
		private static void RecordActivateDeactivate(GameObject g, RuntimeUndo.BoolState value)
		{
			if (RuntimeUndo.<>f__am$cache0 == null)
			{
				RuntimeUndo.<>f__am$cache0 = new ApplyCallback(RuntimeUndo.<RecordActivateDeactivate>m__0);
			}
			ApplyCallback applyCallback = RuntimeUndo.<>f__am$cache0;
			if (RuntimeUndo.<>f__am$cache1 == null)
			{
				RuntimeUndo.<>f__am$cache1 = new PurgeCallback(RuntimeUndo.<RecordActivateDeactivate>m__1);
			}
			RuntimeUndo.RecordObject(g, value, applyCallback, RuntimeUndo.<>f__am$cache1);
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x00016CFE File Offset: 0x000150FE
		public static void BeginRegisterCreateObject(GameObject g)
		{
			if (!RuntimeUndo.Enabled)
			{
				return;
			}
			RuntimeUndo.RecordActivateDeactivate(g, new RuntimeUndo.BoolState(false));
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x00016D18 File Offset: 0x00015118
		public static void RegisterCreatedObject(GameObject g)
		{
			if (!RuntimeUndo.Enabled)
			{
				return;
			}
			ExposeToEditor component = g.GetComponent<ExposeToEditor>();
			if (component)
			{
				component.MarkAsDestroyed = false;
			}
			else
			{
				g.SetActive(true);
			}
			RuntimeUndo.RecordActivateDeactivate(g, new RuntimeUndo.BoolState(true));
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x00016D61 File Offset: 0x00015161
		public static void BeginDestroyObject(GameObject g)
		{
			if (!RuntimeUndo.Enabled)
			{
				return;
			}
			RuntimeUndo.RecordActivateDeactivate(g, new RuntimeUndo.BoolState(true));
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x00016D7C File Offset: 0x0001517C
		public static void DestroyObject(GameObject g)
		{
			if (!RuntimeUndo.Enabled)
			{
				return;
			}
			ExposeToEditor component = g.GetComponent<ExposeToEditor>();
			if (component)
			{
				component.MarkAsDestroyed = true;
			}
			else
			{
				g.SetActive(false);
			}
			RuntimeUndo.RecordActivateDeactivate(g, new RuntimeUndo.BoolState(false));
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x00016DC8 File Offset: 0x000151C8
		private static object GetValue(object target, MemberInfo m)
		{
			PropertyInfo propertyInfo = m as PropertyInfo;
			if (propertyInfo != null)
			{
				return propertyInfo.GetValue(target, null);
			}
			FieldInfo fieldInfo = m as FieldInfo;
			if (fieldInfo != null)
			{
				return fieldInfo.GetValue(target);
			}
			throw new ArgumentException("member is not FieldInfo and is not PropertyInfo", "m");
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x00016E10 File Offset: 0x00015210
		private static void SetValue(object target, MemberInfo m, object value)
		{
			PropertyInfo propertyInfo = m as PropertyInfo;
			if (propertyInfo != null)
			{
				propertyInfo.SetValue(target, value, null);
				return;
			}
			FieldInfo fieldInfo = m as FieldInfo;
			if (fieldInfo != null)
			{
				fieldInfo.SetValue(target, value);
				return;
			}
			throw new ArgumentException("member is not FieldInfo and is not PropertyInfo", "m");
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x00016E5C File Offset: 0x0001525C
		private static Array DuplicateArray(Array array)
		{
			Array array2 = (Array)Activator.CreateInstance(array.GetType(), new object[]
			{
				array.Length
			});
			if (array != null)
			{
				for (int i = 0; i < array2.Length; i++)
				{
					array2.SetValue(array.GetValue(i), i);
				}
			}
			return array;
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x00016EBC File Offset: 0x000152BC
		public static void RecordValue(object target, MemberInfo memberInfo)
		{
			RuntimeUndo.<RecordValue>c__AnonStorey0 <RecordValue>c__AnonStorey = new RuntimeUndo.<RecordValue>c__AnonStorey0();
			<RecordValue>c__AnonStorey.memberInfo = memberInfo;
			if (!RuntimeUndo.Enabled)
			{
				return;
			}
			if (!(<RecordValue>c__AnonStorey.memberInfo is PropertyInfo) && !(<RecordValue>c__AnonStorey.memberInfo is FieldInfo))
			{
				UnityEngine.Debug.LogWarning("Unable to record value");
				return;
			}
			object value = RuntimeUndo.GetValue(target, <RecordValue>c__AnonStorey.memberInfo);
			if (value != null && value is Array)
			{
				object value2 = RuntimeUndo.DuplicateArray((Array)value);
				RuntimeUndo.SetValue(target, <RecordValue>c__AnonStorey.memberInfo, value2);
			}
			object state = value;
			ApplyCallback applyCallback = new ApplyCallback(<RecordValue>c__AnonStorey.<>m__0);
			if (RuntimeUndo.<>f__am$cache2 == null)
			{
				RuntimeUndo.<>f__am$cache2 = new PurgeCallback(RuntimeUndo.<RecordValue>m__2);
			}
			RuntimeUndo.RecordObject(target, state, applyCallback, RuntimeUndo.<>f__am$cache2);
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x00016F74 File Offset: 0x00015374
		public static void RecordTransform(Transform target, Transform parent = null, int siblingIndex = -1)
		{
			if (!RuntimeUndo.Enabled)
			{
				return;
			}
			RuntimeUndo.TransformRecord transformRecord = new RuntimeUndo.TransformRecord
			{
				position = target.position,
				rotation = target.rotation,
				scale = target.localScale
			};
			transformRecord.parent = parent;
			transformRecord.siblingIndex = siblingIndex;
			object state = transformRecord;
			if (RuntimeUndo.<>f__am$cache3 == null)
			{
				RuntimeUndo.<>f__am$cache3 = new ApplyCallback(RuntimeUndo.<RecordTransform>m__3);
			}
			ApplyCallback applyCallback = RuntimeUndo.<>f__am$cache3;
			if (RuntimeUndo.<>f__am$cache4 == null)
			{
				RuntimeUndo.<>f__am$cache4 = new PurgeCallback(RuntimeUndo.<RecordTransform>m__4);
			}
			RuntimeUndo.RecordObject(target, state, applyCallback, RuntimeUndo.<>f__am$cache4);
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x00017008 File Offset: 0x00015408
		public static void RecordSelection()
		{
			if (!RuntimeUndo.Enabled)
			{
				return;
			}
			object target = null;
			object state = new RuntimeUndo.SelectionRecord
			{
				objects = RuntimeSelection.objects,
				activeObject = RuntimeSelection.activeObject
			};
			if (RuntimeUndo.<>f__am$cache5 == null)
			{
				RuntimeUndo.<>f__am$cache5 = new ApplyCallback(RuntimeUndo.<RecordSelection>m__5);
			}
			ApplyCallback applyCallback = RuntimeUndo.<>f__am$cache5;
			if (RuntimeUndo.<>f__am$cache6 == null)
			{
				RuntimeUndo.<>f__am$cache6 = new PurgeCallback(RuntimeUndo.<RecordSelection>m__6);
			}
			RuntimeUndo.RecordObject(target, state, applyCallback, RuntimeUndo.<>f__am$cache6);
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x00017080 File Offset: 0x00015480
		public static void RecordComponent(MonoBehaviour component)
		{
			Type type = component.GetType();
			if (type == typeof(MonoBehaviour))
			{
				return;
			}
			List<FieldInfo> list = new List<FieldInfo>();
			while (type != typeof(MonoBehaviour))
			{
				list.AddRange(type.GetSerializableFields());
				type = type.BaseType();
			}
			bool flag = false;
			if (!RuntimeUndo.IsRecording)
			{
				flag = true;
				RuntimeUndo.BeginRecord();
			}
			for (int i = 0; i < list.Count; i++)
			{
				RuntimeUndo.RecordValue(component, list[i]);
			}
			if (flag)
			{
				RuntimeUndo.EndRecord();
			}
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x00017118 File Offset: 0x00015518
		public static void RecordObject(object target, object state, ApplyCallback applyCallback, PurgeCallback purgeCallback = null)
		{
			if (!RuntimeUndo.Enabled)
			{
				return;
			}
			if (purgeCallback == null)
			{
				if (RuntimeUndo.<>f__am$cache7 == null)
				{
					RuntimeUndo.<>f__am$cache7 = new PurgeCallback(RuntimeUndo.<RecordObject>m__7);
				}
				purgeCallback = RuntimeUndo.<>f__am$cache7;
			}
			if (RuntimeUndo.m_group != null)
			{
				RuntimeUndo.m_group.Add(new Record(target, state, applyCallback, purgeCallback));
			}
			else
			{
				Record[] array = RuntimeUndo.m_stack.Push(new Record[]
				{
					new Record(target, state, applyCallback, purgeCallback)
				});
				if (array != null)
				{
					for (int i = 0; i < array.Length; i++)
					{
						array[i].Purge();
					}
				}
				if (RuntimeUndo.StateChanged != null)
				{
					RuntimeUndo.StateChanged();
				}
			}
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x000171CC File Offset: 0x000155CC
		public static void Redo()
		{
			if (!RuntimeUndo.Enabled)
			{
				return;
			}
			if (!RuntimeUndo.m_stack.CanRestore)
			{
				return;
			}
			if (RuntimeUndo.BeforeRedo != null)
			{
				RuntimeUndo.BeforeRedo();
			}
			bool flag;
			do
			{
				flag = false;
				foreach (Record record in RuntimeUndo.m_stack.Restore())
				{
					flag |= record.Apply();
				}
			}
			while (!flag && RuntimeUndo.m_stack.CanRestore);
			if (RuntimeUndo.RedoCompleted != null)
			{
				RuntimeUndo.RedoCompleted();
			}
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x00017260 File Offset: 0x00015660
		public static void Undo()
		{
			if (!RuntimeUndo.Enabled)
			{
				return;
			}
			if (!RuntimeUndo.m_stack.CanPop)
			{
				return;
			}
			if (RuntimeUndo.BeforeUndo != null)
			{
				RuntimeUndo.BeforeUndo();
			}
			bool flag;
			do
			{
				flag = false;
				foreach (Record record in RuntimeUndo.m_stack.Pop())
				{
					flag |= record.Apply();
				}
			}
			while (!flag && RuntimeUndo.m_stack.CanPop);
			if (RuntimeUndo.UndoCompleted != null)
			{
				RuntimeUndo.UndoCompleted();
			}
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x000172F4 File Offset: 0x000156F4
		public static void Purge()
		{
			if (!RuntimeUndo.Enabled)
			{
				return;
			}
			IEnumerator enumerator = ((IEnumerable)RuntimeUndo.m_stack).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Record[] array = (Record[])obj;
					if (array != null)
					{
						foreach (Record record in array)
						{
							record.Purge();
						}
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			RuntimeUndo.m_stack.Clear();
			if (RuntimeUndo.StateChanged != null)
			{
				RuntimeUndo.StateChanged();
			}
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x000173A0 File Offset: 0x000157A0
		public static void Store()
		{
			if (!RuntimeUndo.Enabled)
			{
				return;
			}
			RuntimeUndo.m_stacks.Push(RuntimeUndo.m_stack);
			RuntimeUndo.m_stack = new UndoStack<Record[]>(8192);
			if (RuntimeUndo.StateChanged != null)
			{
				RuntimeUndo.StateChanged();
			}
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x000173E0 File Offset: 0x000157E0
		public static void Restore()
		{
			if (!RuntimeUndo.Enabled)
			{
				return;
			}
			if (RuntimeUndo.m_stack != null)
			{
				RuntimeUndo.m_stack.Clear();
			}
			if (RuntimeUndo.m_stacks.Count > 0)
			{
				RuntimeUndo.m_stack = RuntimeUndo.m_stacks.Pop();
				if (RuntimeUndo.StateChanged != null)
				{
					RuntimeUndo.StateChanged();
				}
			}
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x00017440 File Offset: 0x00015840
		[CompilerGenerated]
		private static bool <RecordActivateDeactivate>m__0(Record record)
		{
			GameObject gameObject = (GameObject)record.Target;
			RuntimeUndo.BoolState boolState = (RuntimeUndo.BoolState)record.State;
			if (gameObject && gameObject.activeSelf != boolState.value)
			{
				ExposeToEditor component = gameObject.GetComponent<ExposeToEditor>();
				if (component)
				{
					component.MarkAsDestroyed = !boolState.value;
				}
				else
				{
					gameObject.SetActive(boolState.value);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x000174B8 File Offset: 0x000158B8
		[CompilerGenerated]
		private static void <RecordActivateDeactivate>m__1(Record record)
		{
			RuntimeUndo.BoolState boolState = (RuntimeUndo.BoolState)record.State;
			if (boolState.value)
			{
				return;
			}
			GameObject gameObject = (GameObject)record.Target;
			if (gameObject)
			{
				ExposeToEditor component = gameObject.GetComponent<ExposeToEditor>();
				if (component)
				{
					if (component.MarkAsDestroyed)
					{
						UnityEngine.Object.DestroyImmediate(gameObject);
					}
				}
				else if (!gameObject.activeSelf)
				{
					UnityEngine.Object.DestroyImmediate(gameObject);
				}
			}
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x0001752D File Offset: 0x0001592D
		[CompilerGenerated]
		private static void <RecordValue>m__2(Record record)
		{
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x00017530 File Offset: 0x00015930
		[CompilerGenerated]
		private static bool <RecordTransform>m__3(Record record)
		{
			Transform transform = (Transform)record.Target;
			if (!transform)
			{
				return false;
			}
			RuntimeUndo.TransformRecord transformRecord = (RuntimeUndo.TransformRecord)record.State;
			bool flag = transform.position != transformRecord.position || transform.rotation != transformRecord.rotation || transform.localScale != transformRecord.scale;
			bool flag2 = transformRecord.siblingIndex == -1;
			if (!flag2)
			{
				flag = (flag || transform.parent != transformRecord.parent || transform.GetSiblingIndex() != transformRecord.siblingIndex);
			}
			if (flag)
			{
				Transform parent = transform.parent;
				if (!flag2)
				{
					transform.SetParent(transformRecord.parent, true);
					transform.SetSiblingIndex(transformRecord.siblingIndex);
				}
				transform.position = transformRecord.position;
				transform.rotation = transformRecord.rotation;
				transform.localScale = transformRecord.scale;
			}
			return flag;
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x00017636 File Offset: 0x00015A36
		[CompilerGenerated]
		private static void <RecordTransform>m__4(Record record)
		{
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x00017638 File Offset: 0x00015A38
		[CompilerGenerated]
		private static bool <RecordSelection>m__5(Record record)
		{
			RuntimeUndo.SelectionRecord selectionRecord = (RuntimeUndo.SelectionRecord)record.State;
			bool flag = false;
			if (selectionRecord.objects != null && RuntimeSelection.objects != null)
			{
				if (selectionRecord.objects.Length != RuntimeSelection.objects.Length)
				{
					flag = true;
				}
				else
				{
					for (int i = 0; i < RuntimeSelection.objects.Length; i++)
					{
						if (selectionRecord.objects[i] != RuntimeSelection.objects[i])
						{
							flag = true;
							break;
						}
					}
				}
			}
			else if (selectionRecord.objects == null)
			{
				flag = (RuntimeSelection.objects != null && RuntimeSelection.objects.Length != 0);
			}
			else if (RuntimeSelection.objects == null)
			{
				flag = (selectionRecord.objects != null && selectionRecord.objects.Length != 0);
			}
			if (flag)
			{
				if (selectionRecord.objects != null)
				{
					List<UnityEngine.Object> list = selectionRecord.objects.ToList<UnityEngine.Object>();
					if (selectionRecord.activeObject != null)
					{
						if (list.Contains(selectionRecord.activeObject))
						{
							list.Remove(selectionRecord.activeObject);
							list.Insert(0, selectionRecord.activeObject);
						}
						else
						{
							list.Insert(0, selectionRecord.activeObject);
						}
					}
					RuntimeUndo.RTSelectionInternalsAccessor.activeObjectProperty = selectionRecord.activeObject;
					RuntimeUndo.RTSelectionInternalsAccessor.objectsProperty = list.ToArray();
				}
				else
				{
					RuntimeUndo.RTSelectionInternalsAccessor.activeObjectProperty = null;
					RuntimeUndo.RTSelectionInternalsAccessor.objectsProperty = null;
				}
			}
			return flag;
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x000177A7 File Offset: 0x00015BA7
		[CompilerGenerated]
		private static void <RecordSelection>m__6(Record r)
		{
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x000177A9 File Offset: 0x00015BA9
		[CompilerGenerated]
		private static void <RecordObject>m__7(Record record)
		{
		}

		// Token: 0x04000431 RID: 1073
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static RuntimeUndoEventHandler BeforeUndo;

		// Token: 0x04000432 RID: 1074
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static RuntimeUndoEventHandler UndoCompleted;

		// Token: 0x04000433 RID: 1075
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static RuntimeUndoEventHandler BeforeRedo;

		// Token: 0x04000434 RID: 1076
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static RuntimeUndoEventHandler RedoCompleted;

		// Token: 0x04000435 RID: 1077
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static RuntimeUndoEventHandler StateChanged;

		// Token: 0x04000436 RID: 1078
		private static List<Record> m_group;

		// Token: 0x04000437 RID: 1079
		private static UndoStack<Record[]> m_stack;

		// Token: 0x04000438 RID: 1080
		private static Stack<UndoStack<Record[]>> m_stacks;

		// Token: 0x04000439 RID: 1081
		public const int Limit = 8192;

		// Token: 0x0400043A RID: 1082
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static bool <Enabled>k__BackingField;

		// Token: 0x0400043B RID: 1083
		[CompilerGenerated]
		private static ApplyCallback <>f__am$cache0;

		// Token: 0x0400043C RID: 1084
		[CompilerGenerated]
		private static PurgeCallback <>f__am$cache1;

		// Token: 0x0400043D RID: 1085
		[CompilerGenerated]
		private static PurgeCallback <>f__am$cache2;

		// Token: 0x0400043E RID: 1086
		[CompilerGenerated]
		private static ApplyCallback <>f__am$cache3;

		// Token: 0x0400043F RID: 1087
		[CompilerGenerated]
		private static PurgeCallback <>f__am$cache4;

		// Token: 0x04000440 RID: 1088
		[CompilerGenerated]
		private static ApplyCallback <>f__am$cache5;

		// Token: 0x04000441 RID: 1089
		[CompilerGenerated]
		private static PurgeCallback <>f__am$cache6;

		// Token: 0x04000442 RID: 1090
		[CompilerGenerated]
		private static PurgeCallback <>f__am$cache7;

		// Token: 0x020000D4 RID: 212
		private class BoolState
		{
			// Token: 0x0600041F RID: 1055 RVA: 0x000177AB File Offset: 0x00015BAB
			public BoolState(bool v)
			{
				this.value = v;
			}

			// Token: 0x04000443 RID: 1091
			public bool value;
		}

		// Token: 0x020000D5 RID: 213
		private class TransformRecord
		{
			// Token: 0x06000420 RID: 1056 RVA: 0x000177BA File Offset: 0x00015BBA
			public TransformRecord()
			{
			}

			// Token: 0x04000444 RID: 1092
			public Vector3 position;

			// Token: 0x04000445 RID: 1093
			public Quaternion rotation;

			// Token: 0x04000446 RID: 1094
			public Vector3 scale;

			// Token: 0x04000447 RID: 1095
			public Transform parent;

			// Token: 0x04000448 RID: 1096
			public int siblingIndex = -1;
		}

		// Token: 0x020000D6 RID: 214
		private class SelectionRecord
		{
			// Token: 0x06000421 RID: 1057 RVA: 0x000177C9 File Offset: 0x00015BC9
			public SelectionRecord()
			{
			}

			// Token: 0x04000449 RID: 1097
			public UnityEngine.Object[] objects;

			// Token: 0x0400044A RID: 1098
			public UnityEngine.Object activeObject;
		}

		// Token: 0x020000D7 RID: 215
		private class RTSelectionInternalsAccessor : RuntimeSelection
		{
			// Token: 0x06000422 RID: 1058 RVA: 0x000177D1 File Offset: 0x00015BD1
			public RTSelectionInternalsAccessor()
			{
			}

			// Token: 0x170000AB RID: 171
			// (get) Token: 0x06000423 RID: 1059 RVA: 0x000177D9 File Offset: 0x00015BD9
			// (set) Token: 0x06000424 RID: 1060 RVA: 0x000177E0 File Offset: 0x00015BE0
			public static UnityEngine.Object activeObjectProperty
			{
				get
				{
					return RuntimeSelection.m_activeObject;
				}
				set
				{
					RuntimeSelection.m_activeObject = value;
				}
			}

			// Token: 0x170000AC RID: 172
			// (get) Token: 0x06000425 RID: 1061 RVA: 0x000177E8 File Offset: 0x00015BE8
			// (set) Token: 0x06000426 RID: 1062 RVA: 0x000177EF File Offset: 0x00015BEF
			public static UnityEngine.Object[] objectsProperty
			{
				get
				{
					return RuntimeSelection.m_objects;
				}
				set
				{
					RuntimeSelection.SetObjects(value);
				}
			}
		}

		// Token: 0x02000EA5 RID: 3749
		[CompilerGenerated]
		private sealed class <RecordValue>c__AnonStorey0
		{
			// Token: 0x06007163 RID: 29027 RVA: 0x000177F7 File Offset: 0x00015BF7
			public <RecordValue>c__AnonStorey0()
			{
			}

			// Token: 0x06007164 RID: 29028 RVA: 0x00017800 File Offset: 0x00015C00
			internal bool <>m__0(Record record)
			{
				object target = record.Target;
				if (target == null)
				{
					return false;
				}
				if (target is UnityEngine.Object && (UnityEngine.Object)target == null)
				{
					return false;
				}
				object state = record.State;
				object value = RuntimeUndo.GetValue(target, this.memberInfo);
				bool flag = true;
				if (state == null && value == null)
				{
					flag = false;
				}
				else if (state != null && value != null)
				{
					if (state is IEnumerable<object>)
					{
						IEnumerable<object> first = (IEnumerable<object>)state;
						IEnumerable<object> second = (IEnumerable<object>)value;
						flag = !first.SequenceEqual(second);
					}
					else
					{
						flag = !state.Equals(value);
					}
				}
				if (flag)
				{
					RuntimeUndo.SetValue(target, this.memberInfo, state);
				}
				return flag;
			}

			// Token: 0x04006537 RID: 25911
			internal MemberInfo memberInfo;
		}
	}
}
