using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Battlehub.Utils;
using UnityEngine;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x0200024D RID: 589
	[DisallowMultipleComponent]
	[ExecuteInEditMode]
	public class PersistentIgnore : MonoBehaviour
	{
		// Token: 0x06000C3A RID: 3130 RVA: 0x0004ACEC File Offset: 0x000490EC
		public PersistentIgnore()
		{
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000C3B RID: 3131 RVA: 0x0004ACFB File Offset: 0x000490FB
		public bool IsRuntime
		{
			get
			{
				return this.m_isRuntime;
			}
		}

		// Token: 0x06000C3C RID: 3132 RVA: 0x0004AD04 File Offset: 0x00049104
		private void Awake()
		{
			if (!Application.isPlaying)
			{
				this.m_isRuntime = false;
				if (!string.IsNullOrEmpty(this.m_guid))
				{
					Guid key = new Guid(this.m_guid);
					if (PersistentIgnore.m_instances.ContainsKey(key))
					{
						if (PersistentIgnore.m_instances[key] != this)
						{
							key = Guid.NewGuid();
							PersistentIgnore.m_instances.Add(key, this);
							this.m_guid = key.ToString();
						}
					}
					else
					{
						PersistentIgnore.m_instances.Add(key, this);
					}
				}
				else
				{
					Guid key2 = Guid.NewGuid();
					PersistentIgnore.m_instances.Add(key2, this);
					this.m_guid = key2.ToString();
				}
			}
			else if (string.IsNullOrEmpty(this.m_guid))
			{
				this.m_isRuntime = true;
			}
			else
			{
				if (PersistentIgnore.m_instances == null)
				{
					PersistentIgnore.m_instances = new Dictionary<Guid, PersistentIgnore>();
				}
				Guid key3 = new Guid(this.m_guid);
				if (PersistentIgnore.m_instances.ContainsKey(key3))
				{
					this.m_isRuntime = true;
				}
				else
				{
					this.m_isRuntime = false;
					PersistentIgnore.m_instances.Add(key3, null);
				}
			}
		}

		// Token: 0x06000C3D RID: 3133 RVA: 0x0004AE38 File Offset: 0x00049238
		private void Reset()
		{
			if (!Application.isPlaying)
			{
				IEnumerable<KeyValuePair<Guid, PersistentIgnore>> source = PersistentIgnore.m_instances.Where(new Func<KeyValuePair<Guid, PersistentIgnore>, bool>(this.<Reset>m__0));
				if (PersistentIgnore.<>f__am$cache0 == null)
				{
					PersistentIgnore.<>f__am$cache0 = new Func<KeyValuePair<Guid, PersistentIgnore>, Guid>(PersistentIgnore.<Reset>m__1);
				}
				Guid key = source.Select(PersistentIgnore.<>f__am$cache0).FirstOrDefault<Guid>();
				if (PersistentIgnore.m_instances.ContainsKey(key))
				{
					this.m_guid = key.ToString();
				}
				this.m_isRuntime = false;
			}
		}

		// Token: 0x06000C3E RID: 3134 RVA: 0x0004AEB7 File Offset: 0x000492B7
		private void Start()
		{
		}

		// Token: 0x06000C3F RID: 3135 RVA: 0x0004AEB9 File Offset: 0x000492B9
		private void OnDestroy()
		{
			if (!string.IsNullOrEmpty(this.m_guid))
			{
				PersistentIgnore.m_instances.Remove(new Guid(this.m_guid));
			}
		}

		// Token: 0x06000C40 RID: 3136 RVA: 0x0004AEE4 File Offset: 0x000492E4
		public GameObject InstantiatePrefab(GameObject prefab, Vector3 position, Quaternion rotation)
		{
			if (!prefab.IsPrefab())
			{
				throw new ArgumentException("is not a prefab", "prefab");
			}
			return UnityEngine.Object.Instantiate<GameObject>(prefab, position, rotation);
		}

		// Token: 0x06000C41 RID: 3137 RVA: 0x0004AF16 File Offset: 0x00049316
		// Note: this type is marked as 'beforefieldinit'.
		static PersistentIgnore()
		{
		}

		// Token: 0x06000C42 RID: 3138 RVA: 0x0004AF22 File Offset: 0x00049322
		[CompilerGenerated]
		private bool <Reset>m__0(KeyValuePair<Guid, PersistentIgnore> kvp)
		{
			return kvp.Value == this;
		}

		// Token: 0x06000C43 RID: 3139 RVA: 0x0004AF31 File Offset: 0x00049331
		[CompilerGenerated]
		private static Guid <Reset>m__1(KeyValuePair<Guid, PersistentIgnore> kvp)
		{
			return kvp.Key;
		}

		// Token: 0x04000CDA RID: 3290
		[ReadOnly]
		[SerializeField]
		private string m_guid;

		// Token: 0x04000CDB RID: 3291
		private static Dictionary<Guid, PersistentIgnore> m_instances = new Dictionary<Guid, PersistentIgnore>();

		// Token: 0x04000CDC RID: 3292
		[ReadOnly]
		[SerializeField]
		private bool m_isRuntime = true;

		// Token: 0x04000CDD RID: 3293
		[CompilerGenerated]
		private static Func<KeyValuePair<Guid, PersistentIgnore>, Guid> <>f__am$cache0;
	}
}
