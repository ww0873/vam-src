using System;
using System.Collections.Generic;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x020006B1 RID: 1713
	public class MultiTypedReference<BaseType, A, B> : MultiTypedReference<BaseType> where BaseType : class where A : BaseType where B : BaseType
	{
		// Token: 0x06002945 RID: 10565 RVA: 0x000E0F24 File Offset: 0x000DF324
		public MultiTypedReference()
		{
		}

		// Token: 0x06002946 RID: 10566 RVA: 0x000E0F4C File Offset: 0x000DF34C
		public override void Clear()
		{
			this._cachedValue = (BaseType)((object)null);
			if (this._index == 0)
			{
				this._a.Clear();
			}
			else if (this._index == 1)
			{
				this._b.Clear();
			}
			this._index = -1;
		}

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x06002947 RID: 10567 RVA: 0x000E0F9E File Offset: 0x000DF39E
		// (set) Token: 0x06002948 RID: 10568 RVA: 0x000E0FC9 File Offset: 0x000DF3C9
		public sealed override BaseType Value
		{
			get
			{
				if (this._cachedValue != null)
				{
					return this._cachedValue;
				}
				this._cachedValue = this.internalGet();
				return this._cachedValue;
			}
			set
			{
				this.Clear();
				this.internalSetAfterClear(value);
				this._cachedValue = value;
			}
		}

		// Token: 0x06002949 RID: 10569 RVA: 0x000E0FE0 File Offset: 0x000DF3E0
		protected virtual BaseType internalGet()
		{
			if (this._index == -1)
			{
				return (BaseType)((object)null);
			}
			if (this._index == 0)
			{
				return this._cachedValue = (BaseType)((object)this._a[0]);
			}
			if (this._index == 1)
			{
				return this._cachedValue = (BaseType)((object)this._b[0]);
			}
			throw new Exception("Invalid index " + this._index);
		}

		// Token: 0x0600294A RID: 10570 RVA: 0x000E1074 File Offset: 0x000DF474
		protected virtual void internalSetAfterClear(BaseType obj)
		{
			if (obj == null)
			{
				this._index = -1;
			}
			else if (obj is A)
			{
				this._a.Add((A)((object)obj));
				this._index = 0;
			}
			else
			{
				if (!(obj is B))
				{
					throw new ArgumentException("The type " + obj.GetType().Name + " is not supported by this reference.");
				}
				this._b.Add((B)((object)obj));
				this._index = 1;
			}
		}

		// Token: 0x040021E5 RID: 8677
		[SerializeField]
		protected int _index = -1;

		// Token: 0x040021E6 RID: 8678
		[SerializeField]
		private List<A> _a = new List<A>();

		// Token: 0x040021E7 RID: 8679
		[SerializeField]
		private List<B> _b = new List<B>();

		// Token: 0x040021E8 RID: 8680
		[NonSerialized]
		protected BaseType _cachedValue;
	}
}
