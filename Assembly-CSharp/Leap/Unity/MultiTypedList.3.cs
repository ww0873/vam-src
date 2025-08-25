using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x020006A8 RID: 1704
	[Serializable]
	public class MultiTypedList<BaseType, A, B> : MultiTypedList<BaseType> where A : BaseType where B : BaseType
	{
		// Token: 0x0600291E RID: 10526 RVA: 0x000E0A03 File Offset: 0x000DEE03
		public MultiTypedList()
		{
		}

		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x0600291F RID: 10527 RVA: 0x000E0A2C File Offset: 0x000DEE2C
		public override int Count
		{
			get
			{
				return this._table.Count;
			}
		}

		// Token: 0x06002920 RID: 10528 RVA: 0x000E0A39 File Offset: 0x000DEE39
		public override void Add(BaseType obj)
		{
			this._table.Add(this.addInternal(obj));
		}

		// Token: 0x06002921 RID: 10529 RVA: 0x000E0A4D File Offset: 0x000DEE4D
		public override void Clear()
		{
			this._table.Clear();
			this._a.Clear();
			this._b.Clear();
		}

		// Token: 0x06002922 RID: 10530 RVA: 0x000E0A70 File Offset: 0x000DEE70
		public override void Insert(int index, BaseType obj)
		{
			this._table.Insert(index, this.addInternal(obj));
		}

		// Token: 0x06002923 RID: 10531 RVA: 0x000E0A88 File Offset: 0x000DEE88
		public override void RemoveAt(int index)
		{
			MultiTypedList.Key key = this._table[index];
			this._table.RemoveAt(index);
			this.getList(key.id).RemoveAt(key.index);
			for (int i = 0; i < this._table.Count; i++)
			{
				MultiTypedList.Key value = this._table[i];
				if (value.id == key.id && value.index > key.index)
				{
					value.index--;
					this._table[i] = value;
				}
			}
		}

		// Token: 0x17000523 RID: 1315
		public override BaseType this[int index]
		{
			get
			{
				MultiTypedList.Key key = this._table[index];
				return (BaseType)((object)this.getList(key.id)[key.index]);
			}
			set
			{
				MultiTypedList.Key key = this._table[index];
				this.getList(key.id).RemoveAt(key.index);
				MultiTypedList.Key value2 = this.addInternal(value);
				this._table[index] = value2;
			}
		}

		// Token: 0x06002926 RID: 10534 RVA: 0x000E0BB4 File Offset: 0x000DEFB4
		protected MultiTypedList.Key addHelper(IList list, BaseType instance, int id)
		{
			MultiTypedList.Key result = new MultiTypedList.Key
			{
				id = id,
				index = list.Count
			};
			list.Add(instance);
			return result;
		}

		// Token: 0x06002927 RID: 10535 RVA: 0x000E0BF0 File Offset: 0x000DEFF0
		protected virtual MultiTypedList.Key addInternal(BaseType obj)
		{
			if (obj is A)
			{
				return this.addHelper(this._a, obj, 0);
			}
			if (obj is B)
			{
				return this.addHelper(this._b, obj, 1);
			}
			throw new ArgumentException("This multi typed list does not support type " + obj.GetType().Name);
		}

		// Token: 0x06002928 RID: 10536 RVA: 0x000E0C5C File Offset: 0x000DF05C
		protected virtual IList getList(int id)
		{
			if (id == 0)
			{
				return this._a;
			}
			if (id == 1)
			{
				return this._b;
			}
			throw new Exception("This multi typed list does not have a list with id " + id);
		}

		// Token: 0x040021DC RID: 8668
		[SerializeField]
		private List<MultiTypedList.Key> _table = new List<MultiTypedList.Key>();

		// Token: 0x040021DD RID: 8669
		[SerializeField]
		private List<A> _a = new List<A>();

		// Token: 0x040021DE RID: 8670
		[SerializeField]
		private List<B> _b = new List<B>();
	}
}
