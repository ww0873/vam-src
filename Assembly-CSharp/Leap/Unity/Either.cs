using System;

namespace Leap.Unity
{
	// Token: 0x02000695 RID: 1685
	public struct Either<A, B> : IEquatable<Either<A, B>>, IComparable, IComparable<Either<A, B>>
	{
		// Token: 0x060028A0 RID: 10400 RVA: 0x000DF5C0 File Offset: 0x000DD9C0
		public Either(A a)
		{
			if (a == null)
			{
				throw new ArgumentNullException("Cannot initialize an Either with a null value.");
			}
			this.isA = true;
			this._a = a;
			this._b = default(B);
		}

		// Token: 0x060028A1 RID: 10401 RVA: 0x000DF600 File Offset: 0x000DDA00
		public Either(B b)
		{
			if (b == null)
			{
				throw new ArgumentNullException("Cannot initialize an Either with a null value.");
			}
			this.isA = false;
			this._b = b;
			this._a = default(A);
		}

		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x060028A2 RID: 10402 RVA: 0x000DF640 File Offset: 0x000DDA40
		public bool isB
		{
			get
			{
				return !this.isA;
			}
		}

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x060028A3 RID: 10403 RVA: 0x000DF64B File Offset: 0x000DDA4B
		public Maybe<A> a
		{
			get
			{
				if (this.isA)
				{
					return Maybe<A>.Some(this._a);
				}
				return Maybe<A>.None;
			}
		}

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x060028A4 RID: 10404 RVA: 0x000DF669 File Offset: 0x000DDA69
		public Maybe<B> b
		{
			get
			{
				if (this.isA)
				{
					return Maybe<B>.None;
				}
				return Maybe<B>.Some(this._b);
			}
		}

		// Token: 0x060028A5 RID: 10405 RVA: 0x000DF687 File Offset: 0x000DDA87
		public void Match(Action<A> ifA, Action<B> ifB)
		{
			if (this.isA)
			{
				if (ifA != null)
				{
					ifA(this._a);
				}
			}
			else if (ifB != null)
			{
				ifB(this._b);
			}
		}

		// Token: 0x060028A6 RID: 10406 RVA: 0x000DF6BD File Offset: 0x000DDABD
		public bool TryGetA(out A a)
		{
			a = this._a;
			return this.isA;
		}

		// Token: 0x060028A7 RID: 10407 RVA: 0x000DF6D1 File Offset: 0x000DDAD1
		public bool TryGetB(out B b)
		{
			b = this._b;
			return !this.isA;
		}

		// Token: 0x060028A8 RID: 10408 RVA: 0x000DF6E8 File Offset: 0x000DDAE8
		public override int GetHashCode()
		{
			if (this.isA)
			{
				A a = this._a;
				return a.GetHashCode();
			}
			B b = this._b;
			return b.GetHashCode();
		}

		// Token: 0x060028A9 RID: 10409 RVA: 0x000DF729 File Offset: 0x000DDB29
		public override bool Equals(object obj)
		{
			return obj is Either<A, B> && this.Equals((Either<A, B>)obj);
		}

		// Token: 0x060028AA RID: 10410 RVA: 0x000DF744 File Offset: 0x000DDB44
		public bool Equals(Either<A, B> other)
		{
			if (this.isA != other.isA)
			{
				return false;
			}
			if (this.isA)
			{
				A a = this._a;
				return a.Equals(other._a);
			}
			B b = this._b;
			return b.Equals(other._b);
		}

		// Token: 0x060028AB RID: 10411 RVA: 0x000DF7B1 File Offset: 0x000DDBB1
		public int CompareTo(object obj)
		{
			if (!(obj is Either<A, B>))
			{
				throw new ArgumentException();
			}
			return this.CompareTo((Either<A, B>)obj);
		}

		// Token: 0x060028AC RID: 10412 RVA: 0x000DF7D0 File Offset: 0x000DDBD0
		public int CompareTo(Either<A, B> other)
		{
			if (this.isA != other.isA)
			{
				return (!this.isA) ? 1 : -1;
			}
			if (this.isA)
			{
				IComparable<A> comparable = this._a as IComparable<A>;
				if (comparable != null)
				{
					return comparable.CompareTo(other._a);
				}
				IComparable comparable2 = this._a as IComparable;
				if (comparable2 != null)
				{
					return comparable2.CompareTo(other._b);
				}
				return 0;
			}
			else
			{
				IComparable<B> comparable3 = this._b as IComparable<B>;
				if (comparable3 != null)
				{
					return comparable3.CompareTo(other._b);
				}
				IComparable comparable4 = this._b as IComparable;
				if (comparable4 != null)
				{
					return comparable4.CompareTo(other._b);
				}
				return 0;
			}
		}

		// Token: 0x060028AD RID: 10413 RVA: 0x000DF8AE File Offset: 0x000DDCAE
		public static bool operator ==(Either<A, B> either0, Either<A, B> either1)
		{
			return either0.Equals(either1);
		}

		// Token: 0x060028AE RID: 10414 RVA: 0x000DF8B8 File Offset: 0x000DDCB8
		public static bool operator !=(Either<A, B> either0, Either<A, B> either1)
		{
			return !either0.Equals(either1);
		}

		// Token: 0x060028AF RID: 10415 RVA: 0x000DF8C5 File Offset: 0x000DDCC5
		public static bool operator >(Either<A, B> either0, Either<A, B> either1)
		{
			return either0.CompareTo(either1) > 0;
		}

		// Token: 0x060028B0 RID: 10416 RVA: 0x000DF8D2 File Offset: 0x000DDCD2
		public static bool operator >=(Either<A, B> either0, Either<A, B> either1)
		{
			return either0.CompareTo(either1) >= 0;
		}

		// Token: 0x060028B1 RID: 10417 RVA: 0x000DF8E2 File Offset: 0x000DDCE2
		public static bool operator <(Either<A, B> either0, Either<A, B> either1)
		{
			return either0.CompareTo(either1) < 0;
		}

		// Token: 0x060028B2 RID: 10418 RVA: 0x000DF8EF File Offset: 0x000DDCEF
		public static bool operator <=(Either<A, B> either0, Either<A, B> either1)
		{
			return either0.CompareTo(either1) <= 0;
		}

		// Token: 0x060028B3 RID: 10419 RVA: 0x000DF8FF File Offset: 0x000DDCFF
		public static implicit operator Either<A, B>(A a)
		{
			return new Either<A, B>(a);
		}

		// Token: 0x060028B4 RID: 10420 RVA: 0x000DF907 File Offset: 0x000DDD07
		public static implicit operator Either<A, B>(B b)
		{
			return new Either<A, B>(b);
		}

		// Token: 0x040021C7 RID: 8647
		public readonly bool isA;

		// Token: 0x040021C8 RID: 8648
		private readonly A _a;

		// Token: 0x040021C9 RID: 8649
		private readonly B _b;
	}
}
