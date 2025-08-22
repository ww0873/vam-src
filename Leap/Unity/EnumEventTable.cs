using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Leap.Unity
{
	// Token: 0x02000728 RID: 1832
	[Serializable]
	public class EnumEventTable : ISerializationCallbackReceiver
	{
		// Token: 0x06002CA6 RID: 11430 RVA: 0x000EF914 File Offset: 0x000EDD14
		public EnumEventTable()
		{
		}

		// Token: 0x06002CA7 RID: 11431 RVA: 0x000EF934 File Offset: 0x000EDD34
		public bool HasUnityEvent(int enumValue)
		{
			UnityEvent unityEvent;
			return this._entryMap.TryGetValue(enumValue, out unityEvent) && unityEvent.GetPersistentEventCount() > 0;
		}

		// Token: 0x06002CA8 RID: 11432 RVA: 0x000EF960 File Offset: 0x000EDD60
		public void Invoke(int enumValue)
		{
			UnityEvent unityEvent;
			if (this._entryMap.TryGetValue(enumValue, out unityEvent))
			{
				unityEvent.Invoke();
			}
		}

		// Token: 0x06002CA9 RID: 11433 RVA: 0x000EF986 File Offset: 0x000EDD86
		public void OnBeforeSerialize()
		{
		}

		// Token: 0x06002CAA RID: 11434 RVA: 0x000EF988 File Offset: 0x000EDD88
		public void OnAfterDeserialize()
		{
			if (this._entryMap == null)
			{
				this._entryMap = new Dictionary<int, UnityEvent>();
			}
			else
			{
				this._entryMap.Clear();
			}
			foreach (EnumEventTable.Entry entry in this._entries)
			{
				this._entryMap[entry.enumValue] = entry.callback;
			}
		}

		// Token: 0x040023A0 RID: 9120
		[SerializeField]
		private List<EnumEventTable.Entry> _entries = new List<EnumEventTable.Entry>();

		// Token: 0x040023A1 RID: 9121
		private Dictionary<int, UnityEvent> _entryMap = new Dictionary<int, UnityEvent>();

		// Token: 0x02000729 RID: 1833
		[Serializable]
		private class Entry
		{
			// Token: 0x06002CAB RID: 11435 RVA: 0x000EFA1C File Offset: 0x000EDE1C
			public Entry()
			{
			}

			// Token: 0x040023A2 RID: 9122
			[SerializeField]
			public int enumValue;

			// Token: 0x040023A3 RID: 9123
			[SerializeField]
			public UnityEvent callback;
		}
	}
}
