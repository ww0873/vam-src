using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x02000696 RID: 1686
	[Serializable]
	public struct Hash : IEnumerable, IEquatable<Hash>
	{
		// Token: 0x060028B5 RID: 10421 RVA: 0x000DF90F File Offset: 0x000DDD0F
		public Hash(int hash)
		{
			this._hash = hash;
		}

		// Token: 0x060028B6 RID: 10422 RVA: 0x000DF918 File Offset: 0x000DDD18
		public void Add<T>(T t)
		{
			int num = (t != null) ? t.GetHashCode() : 647155961;
			this._hash ^= num + 1043823033 + (this._hash << 6) + (this._hash >> 2);
		}

		// Token: 0x060028B7 RID: 10423 RVA: 0x000DF970 File Offset: 0x000DDD70
		public void AddRange<T>(List<T> sequence)
		{
			for (int i = 0; i < sequence.Count; i++)
			{
				this.Add<T>(sequence[i]);
			}
		}

		// Token: 0x060028B8 RID: 10424 RVA: 0x000DF9A4 File Offset: 0x000DDDA4
		public static Hash GetHierarchyHash(Transform root)
		{
			Hash dataHash = Hash.GetDataHash(root);
			int childCount = root.childCount;
			for (int i = 0; i < childCount; i++)
			{
				dataHash.Add<Hash>(Hash.GetHierarchyHash(root.GetChild(i)));
			}
			root.GetComponents<Behaviour>(Hash._behaviourCache);
			for (int j = 0; j < Hash._behaviourCache.Count; j++)
			{
				Behaviour behaviour = Hash._behaviourCache[j];
				if (behaviour != null)
				{
					dataHash.Add<bool>(behaviour.enabled);
				}
			}
			return dataHash;
		}

		// Token: 0x060028B9 RID: 10425 RVA: 0x000DFA34 File Offset: 0x000DDE34
		public static Hash GetDataHash(Transform transform)
		{
			Hash result = new Hash
			{
				transform,
				transform.gameObject.activeSelf,
				transform.localPosition,
				transform.localRotation,
				transform.localScale
			};
			if (transform is RectTransform)
			{
				RectTransform rectTransform = transform as RectTransform;
				result.Add<Rect>(rectTransform.rect);
			}
			return result;
		}

		// Token: 0x060028BA RID: 10426 RVA: 0x000DFAAC File Offset: 0x000DDEAC
		public IEnumerator GetEnumerator()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060028BB RID: 10427 RVA: 0x000DFAB3 File Offset: 0x000DDEB3
		public override int GetHashCode()
		{
			return this._hash;
		}

		// Token: 0x060028BC RID: 10428 RVA: 0x000DFABC File Offset: 0x000DDEBC
		public override bool Equals(object obj)
		{
			return obj is Hash && ((Hash)obj)._hash == this._hash;
		}

		// Token: 0x060028BD RID: 10429 RVA: 0x000DFAEC File Offset: 0x000DDEEC
		public bool Equals(Hash other)
		{
			return this._hash == other._hash;
		}

		// Token: 0x060028BE RID: 10430 RVA: 0x000DFAFD File Offset: 0x000DDEFD
		public static implicit operator Hash(int hash)
		{
			return new Hash(hash);
		}

		// Token: 0x060028BF RID: 10431 RVA: 0x000DFB05 File Offset: 0x000DDF05
		public static implicit operator int(Hash hash)
		{
			return hash._hash;
		}

		// Token: 0x060028C0 RID: 10432 RVA: 0x000DFB0E File Offset: 0x000DDF0E
		// Note: this type is marked as 'beforefieldinit'.
		static Hash()
		{
		}

		// Token: 0x040021CA RID: 8650
		private int _hash;

		// Token: 0x040021CB RID: 8651
		private static List<Behaviour> _behaviourCache = new List<Behaviour>();
	}
}
