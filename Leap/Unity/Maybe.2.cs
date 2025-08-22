using System;
using Leap.Unity.Query;

namespace Leap.Unity
{
	// Token: 0x020006A0 RID: 1696
	public struct Maybe<T> : IEquatable<Maybe<T>>, IComparable, IComparable<Maybe<T>>
	{
		// Token: 0x060028DC RID: 10460 RVA: 0x000E0060 File Offset: 0x000DE460
		public Maybe(T t)
		{
			if (Type<T>.isValueType)
			{
				this.hasValue = true;
			}
			else
			{
				this.hasValue = (t != null);
			}
			this._t = t;
		}

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x060028DD RID: 10461 RVA: 0x000E0094 File Offset: 0x000DE494
		public T valueOrDefault
		{
			get
			{
				T result;
				if (this.TryGetValue(out result))
				{
					return result;
				}
				return default(T);
			}
		}

		// Token: 0x060028DE RID: 10462 RVA: 0x000E00B9 File Offset: 0x000DE4B9
		public static Maybe<T> Some(T t)
		{
			if (!Type<T>.isValueType && t == null)
			{
				throw new ArgumentNullException("Cannot use Some with a null argument.");
			}
			return new Maybe<T>(t);
		}

		// Token: 0x060028DF RID: 10463 RVA: 0x000E00E1 File Offset: 0x000DE4E1
		public bool TryGetValue(out T t)
		{
			t = this._t;
			return this.hasValue;
		}

		// Token: 0x060028E0 RID: 10464 RVA: 0x000E00F5 File Offset: 0x000DE4F5
		public void Match(Action<T> ifValue)
		{
			if (this.hasValue)
			{
				ifValue(this._t);
			}
		}

		// Token: 0x060028E1 RID: 10465 RVA: 0x000E010E File Offset: 0x000DE50E
		public void Match(Action<T> ifValue, Action ifNot)
		{
			if (this.hasValue)
			{
				if (ifValue != null)
				{
					ifValue(this._t);
				}
			}
			else
			{
				ifNot();
			}
		}

		// Token: 0x060028E2 RID: 10466 RVA: 0x000E0138 File Offset: 0x000DE538
		public K Match<K>(Func<T, K> ifValue, Func<K> ifNot)
		{
			if (!this.hasValue)
			{
				return ifNot();
			}
			if (ifValue != null)
			{
				return ifValue(this._t);
			}
			return default(K);
		}

		// Token: 0x060028E3 RID: 10467 RVA: 0x000E0173 File Offset: 0x000DE573
		public T ValueOr(T customDefault)
		{
			if (this.hasValue)
			{
				return this._t;
			}
			return customDefault;
		}

		// Token: 0x060028E4 RID: 10468 RVA: 0x000E0188 File Offset: 0x000DE588
		public Maybe<T> ValueOr(Maybe<T> maybeCustomDefault)
		{
			if (this.hasValue)
			{
				return this;
			}
			return maybeCustomDefault;
		}

		// Token: 0x060028E5 RID: 10469 RVA: 0x000E019D File Offset: 0x000DE59D
		public Query<T> Query()
		{
			if (this.hasValue)
			{
				return Values.Single<T>(this._t);
			}
			return Values.Empty<T>();
		}

		// Token: 0x060028E6 RID: 10470 RVA: 0x000E01BC File Offset: 0x000DE5BC
		public override int GetHashCode()
		{
			int result;
			if (this.hasValue)
			{
				T t = this._t;
				result = t.GetHashCode();
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x060028E7 RID: 10471 RVA: 0x000E01EE File Offset: 0x000DE5EE
		public override bool Equals(object obj)
		{
			return obj is Maybe<T> && this.Equals((Maybe<T>)obj);
		}

		// Token: 0x060028E8 RID: 10472 RVA: 0x000E020C File Offset: 0x000DE60C
		public bool Equals(Maybe<T> other)
		{
			if (this.hasValue != other.hasValue)
			{
				return false;
			}
			if (this.hasValue)
			{
				T t = this._t;
				return t.Equals(other._t);
			}
			return true;
		}

		// Token: 0x060028E9 RID: 10473 RVA: 0x000E025A File Offset: 0x000DE65A
		public int CompareTo(object obj)
		{
			if (!(obj is Maybe<T>))
			{
				throw new ArgumentException();
			}
			return this.CompareTo((Maybe<T>)obj);
		}

		// Token: 0x060028EA RID: 10474 RVA: 0x000E027C File Offset: 0x000DE67C
		public int CompareTo(Maybe<T> other)
		{
			if (this.hasValue != other.hasValue)
			{
				return (!this.hasValue) ? -1 : 1;
			}
			if (!this.hasValue)
			{
				return 0;
			}
			IComparable<T> comparable = this._t as IComparable<T>;
			if (comparable != null)
			{
				return comparable.CompareTo(other._t);
			}
			IComparable comparable2 = this._t as IComparable;
			if (comparable2 != null)
			{
				return comparable2.CompareTo(other._t);
			}
			return 0;
		}

		// Token: 0x060028EB RID: 10475 RVA: 0x000E030B File Offset: 0x000DE70B
		public static bool operator ==(Maybe<T> maybe0, Maybe<T> maybe1)
		{
			return maybe0.Equals(maybe1);
		}

		// Token: 0x060028EC RID: 10476 RVA: 0x000E0315 File Offset: 0x000DE715
		public static bool operator !=(Maybe<T> maybe0, Maybe<T> maybe1)
		{
			return !maybe0.Equals(maybe1);
		}

		// Token: 0x060028ED RID: 10477 RVA: 0x000E0322 File Offset: 0x000DE722
		public static bool operator >(Maybe<T> maybe0, Maybe<T> maybe1)
		{
			return maybe0.CompareTo(maybe1) > 0;
		}

		// Token: 0x060028EE RID: 10478 RVA: 0x000E032F File Offset: 0x000DE72F
		public static bool operator >=(Maybe<T> maybe0, Maybe<T> maybe1)
		{
			return maybe0.CompareTo(maybe1) >= 0;
		}

		// Token: 0x060028EF RID: 10479 RVA: 0x000E033F File Offset: 0x000DE73F
		public static bool operator <(Maybe<T> maybe0, Maybe<T> maybe1)
		{
			return maybe0.CompareTo(maybe1) < 0;
		}

		// Token: 0x060028F0 RID: 10480 RVA: 0x000E034C File Offset: 0x000DE74C
		public static bool operator <=(Maybe<T> maybe0, Maybe<T> maybe1)
		{
			return maybe0.CompareTo(maybe1) <= 0;
		}

		// Token: 0x060028F1 RID: 10481 RVA: 0x000E035C File Offset: 0x000DE75C
		public static implicit operator Maybe<T>(T t)
		{
			return new Maybe<T>(t);
		}

		// Token: 0x060028F2 RID: 10482 RVA: 0x000E0364 File Offset: 0x000DE764
		public static implicit operator Maybe<T>(Maybe.NoneType none)
		{
			return Maybe<T>.None;
		}

		// Token: 0x060028F3 RID: 10483 RVA: 0x000E036C File Offset: 0x000DE76C
		// Note: this type is marked as 'beforefieldinit'.
		static Maybe()
		{
		}

		// Token: 0x040021D2 RID: 8658
		public static readonly Maybe<T> None = default(Maybe<T>);

		// Token: 0x040021D3 RID: 8659
		public readonly bool hasValue;

		// Token: 0x040021D4 RID: 8660
		private readonly T _t;
	}
}
