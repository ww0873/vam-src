using System;
using System.Runtime.CompilerServices;

namespace Leap.Unity
{
	// Token: 0x0200069E RID: 1694
	public static class Maybe
	{
		// Token: 0x060028D7 RID: 10455 RVA: 0x000DFD3C File Offset: 0x000DE13C
		public static Maybe<T> Some<T>(T value)
		{
			return new Maybe<T>(value);
		}

		// Token: 0x060028D8 RID: 10456 RVA: 0x000DFD44 File Offset: 0x000DE144
		public static void MatchAll<A, B>(Maybe<A> maybeA, Maybe<B> maybeB, Action<A, B> action)
		{
			Maybe.<MatchAll>c__AnonStorey0<A, B> <MatchAll>c__AnonStorey = new Maybe.<MatchAll>c__AnonStorey0<A, B>();
			<MatchAll>c__AnonStorey.maybeB = maybeB;
			<MatchAll>c__AnonStorey.action = action;
			maybeA.Match(new Action<A>(<MatchAll>c__AnonStorey.<>m__0));
		}

		// Token: 0x060028D9 RID: 10457 RVA: 0x000DFD78 File Offset: 0x000DE178
		public static void MatchAll<A, B, C>(Maybe<A> maybeA, Maybe<B> maybeB, Maybe<C> maybeC, Action<A, B, C> action)
		{
			Maybe.<MatchAll>c__AnonStorey2<A, B, C> <MatchAll>c__AnonStorey = new Maybe.<MatchAll>c__AnonStorey2<A, B, C>();
			<MatchAll>c__AnonStorey.maybeB = maybeB;
			<MatchAll>c__AnonStorey.maybeC = maybeC;
			<MatchAll>c__AnonStorey.action = action;
			maybeA.Match(new Action<A>(<MatchAll>c__AnonStorey.<>m__0));
		}

		// Token: 0x060028DA RID: 10458 RVA: 0x000DFDB4 File Offset: 0x000DE1B4
		public static void MatchAll<A, B, C, D>(Maybe<A> maybeA, Maybe<B> maybeB, Maybe<C> maybeC, Maybe<D> maybeD, Action<A, B, C, D> action)
		{
			Maybe.<MatchAll>c__AnonStorey5<A, B, C, D> <MatchAll>c__AnonStorey = new Maybe.<MatchAll>c__AnonStorey5<A, B, C, D>();
			<MatchAll>c__AnonStorey.maybeB = maybeB;
			<MatchAll>c__AnonStorey.maybeC = maybeC;
			<MatchAll>c__AnonStorey.maybeD = maybeD;
			<MatchAll>c__AnonStorey.action = action;
			maybeA.Match(new Action<A>(<MatchAll>c__AnonStorey.<>m__0));
		}

		// Token: 0x060028DB RID: 10459 RVA: 0x000DFDF8 File Offset: 0x000DE1F8
		// Note: this type is marked as 'beforefieldinit'.
		static Maybe()
		{
		}

		// Token: 0x040021D1 RID: 8657
		public static readonly Maybe.NoneType None = default(Maybe.NoneType);

		// Token: 0x0200069F RID: 1695
		public struct NoneType
		{
		}

		// Token: 0x02000F92 RID: 3986
		[CompilerGenerated]
		private sealed class <MatchAll>c__AnonStorey0<A, B>
		{
			// Token: 0x0600745F RID: 29791 RVA: 0x000DFE13 File Offset: 0x000DE213
			public <MatchAll>c__AnonStorey0()
			{
			}

			// Token: 0x06007460 RID: 29792 RVA: 0x000DFE1C File Offset: 0x000DE21C
			internal void <>m__0(A a)
			{
				Maybe.<MatchAll>c__AnonStorey0<A, B>.<MatchAll>c__AnonStorey1 <MatchAll>c__AnonStorey = new Maybe.<MatchAll>c__AnonStorey0<A, B>.<MatchAll>c__AnonStorey1();
				<MatchAll>c__AnonStorey.<>f__ref$0 = this;
				<MatchAll>c__AnonStorey.a = a;
				this.maybeB.Match(new Action<B>(<MatchAll>c__AnonStorey.<>m__0));
			}

			// Token: 0x0400686E RID: 26734
			internal Maybe<B> maybeB;

			// Token: 0x0400686F RID: 26735
			internal Action<A, B> action;

			// Token: 0x02000F95 RID: 3989
			private sealed class <MatchAll>c__AnonStorey1
			{
				// Token: 0x06007465 RID: 29797 RVA: 0x000DFE54 File Offset: 0x000DE254
				public <MatchAll>c__AnonStorey1()
				{
				}

				// Token: 0x06007466 RID: 29798 RVA: 0x000DFE5C File Offset: 0x000DE25C
				internal void <>m__0(B b)
				{
					this.<>f__ref$0.action(this.a, b);
				}

				// Token: 0x04006877 RID: 26743
				internal A a;

				// Token: 0x04006878 RID: 26744
				internal Maybe.<MatchAll>c__AnonStorey0<A, B> <>f__ref$0;
			}
		}

		// Token: 0x02000F93 RID: 3987
		[CompilerGenerated]
		private sealed class <MatchAll>c__AnonStorey2<A, B, C>
		{
			// Token: 0x06007461 RID: 29793 RVA: 0x000DFE75 File Offset: 0x000DE275
			public <MatchAll>c__AnonStorey2()
			{
			}

			// Token: 0x06007462 RID: 29794 RVA: 0x000DFE80 File Offset: 0x000DE280
			internal void <>m__0(A a)
			{
				Maybe.<MatchAll>c__AnonStorey2<A, B, C>.<MatchAll>c__AnonStorey3 <MatchAll>c__AnonStorey = new Maybe.<MatchAll>c__AnonStorey2<A, B, C>.<MatchAll>c__AnonStorey3();
				<MatchAll>c__AnonStorey.<>f__ref$2 = this;
				<MatchAll>c__AnonStorey.a = a;
				this.maybeB.Match(new Action<B>(<MatchAll>c__AnonStorey.<>m__0));
			}

			// Token: 0x04006870 RID: 26736
			internal Maybe<B> maybeB;

			// Token: 0x04006871 RID: 26737
			internal Maybe<C> maybeC;

			// Token: 0x04006872 RID: 26738
			internal Action<A, B, C> action;

			// Token: 0x02000F96 RID: 3990
			private sealed class <MatchAll>c__AnonStorey3
			{
				// Token: 0x06007467 RID: 29799 RVA: 0x000DFEB8 File Offset: 0x000DE2B8
				public <MatchAll>c__AnonStorey3()
				{
				}

				// Token: 0x06007468 RID: 29800 RVA: 0x000DFEC0 File Offset: 0x000DE2C0
				internal void <>m__0(B b)
				{
					Maybe.<MatchAll>c__AnonStorey2<A, B, C>.<MatchAll>c__AnonStorey3.<MatchAll>c__AnonStorey4 <MatchAll>c__AnonStorey = new Maybe.<MatchAll>c__AnonStorey2<A, B, C>.<MatchAll>c__AnonStorey3.<MatchAll>c__AnonStorey4();
					<MatchAll>c__AnonStorey.<>f__ref$2 = this.<>f__ref$2;
					<MatchAll>c__AnonStorey.<>f__ref$3 = this;
					<MatchAll>c__AnonStorey.b = b;
					this.<>f__ref$2.maybeC.Match(new Action<C>(<MatchAll>c__AnonStorey.<>m__0));
				}

				// Token: 0x04006879 RID: 26745
				internal A a;

				// Token: 0x0400687A RID: 26746
				internal Maybe.<MatchAll>c__AnonStorey2<A, B, C> <>f__ref$2;

				// Token: 0x02000F97 RID: 3991
				private sealed class <MatchAll>c__AnonStorey4
				{
					// Token: 0x06007469 RID: 29801 RVA: 0x000DFF09 File Offset: 0x000DE309
					public <MatchAll>c__AnonStorey4()
					{
					}

					// Token: 0x0600746A RID: 29802 RVA: 0x000DFF11 File Offset: 0x000DE311
					internal void <>m__0(C c)
					{
						this.<>f__ref$2.action(this.<>f__ref$3.a, this.b, c);
					}

					// Token: 0x0400687B RID: 26747
					internal B b;

					// Token: 0x0400687C RID: 26748
					internal Maybe.<MatchAll>c__AnonStorey2<A, B, C> <>f__ref$2;

					// Token: 0x0400687D RID: 26749
					internal Maybe.<MatchAll>c__AnonStorey2<A, B, C>.<MatchAll>c__AnonStorey3 <>f__ref$3;
				}
			}
		}

		// Token: 0x02000F94 RID: 3988
		[CompilerGenerated]
		private sealed class <MatchAll>c__AnonStorey5<A, B, C, D>
		{
			// Token: 0x06007463 RID: 29795 RVA: 0x000DFF35 File Offset: 0x000DE335
			public <MatchAll>c__AnonStorey5()
			{
			}

			// Token: 0x06007464 RID: 29796 RVA: 0x000DFF40 File Offset: 0x000DE340
			internal void <>m__0(A a)
			{
				Maybe.<MatchAll>c__AnonStorey5<A, B, C, D>.<MatchAll>c__AnonStorey6 <MatchAll>c__AnonStorey = new Maybe.<MatchAll>c__AnonStorey5<A, B, C, D>.<MatchAll>c__AnonStorey6();
				<MatchAll>c__AnonStorey.<>f__ref$5 = this;
				<MatchAll>c__AnonStorey.a = a;
				this.maybeB.Match(new Action<B>(<MatchAll>c__AnonStorey.<>m__0));
			}

			// Token: 0x04006873 RID: 26739
			internal Maybe<B> maybeB;

			// Token: 0x04006874 RID: 26740
			internal Maybe<C> maybeC;

			// Token: 0x04006875 RID: 26741
			internal Maybe<D> maybeD;

			// Token: 0x04006876 RID: 26742
			internal Action<A, B, C, D> action;

			// Token: 0x02000F98 RID: 3992
			private sealed class <MatchAll>c__AnonStorey6
			{
				// Token: 0x0600746B RID: 29803 RVA: 0x000DFF78 File Offset: 0x000DE378
				public <MatchAll>c__AnonStorey6()
				{
				}

				// Token: 0x0600746C RID: 29804 RVA: 0x000DFF80 File Offset: 0x000DE380
				internal void <>m__0(B b)
				{
					Maybe.<MatchAll>c__AnonStorey5<A, B, C, D>.<MatchAll>c__AnonStorey6.<MatchAll>c__AnonStorey7 <MatchAll>c__AnonStorey = new Maybe.<MatchAll>c__AnonStorey5<A, B, C, D>.<MatchAll>c__AnonStorey6.<MatchAll>c__AnonStorey7();
					<MatchAll>c__AnonStorey.<>f__ref$5 = this.<>f__ref$5;
					<MatchAll>c__AnonStorey.<>f__ref$6 = this;
					<MatchAll>c__AnonStorey.b = b;
					this.<>f__ref$5.maybeC.Match(new Action<C>(<MatchAll>c__AnonStorey.<>m__0));
				}

				// Token: 0x0400687E RID: 26750
				internal A a;

				// Token: 0x0400687F RID: 26751
				internal Maybe.<MatchAll>c__AnonStorey5<A, B, C, D> <>f__ref$5;

				// Token: 0x02000F99 RID: 3993
				private sealed class <MatchAll>c__AnonStorey7
				{
					// Token: 0x0600746D RID: 29805 RVA: 0x000DFFC9 File Offset: 0x000DE3C9
					public <MatchAll>c__AnonStorey7()
					{
					}

					// Token: 0x0600746E RID: 29806 RVA: 0x000DFFD4 File Offset: 0x000DE3D4
					internal void <>m__0(C c)
					{
						Maybe.<MatchAll>c__AnonStorey5<A, B, C, D>.<MatchAll>c__AnonStorey6.<MatchAll>c__AnonStorey7.<MatchAll>c__AnonStorey8 <MatchAll>c__AnonStorey = new Maybe.<MatchAll>c__AnonStorey5<A, B, C, D>.<MatchAll>c__AnonStorey6.<MatchAll>c__AnonStorey7.<MatchAll>c__AnonStorey8();
						<MatchAll>c__AnonStorey.<>f__ref$5 = this.<>f__ref$5;
						<MatchAll>c__AnonStorey.<>f__ref$6 = this.<>f__ref$6;
						<MatchAll>c__AnonStorey.<>f__ref$7 = this;
						<MatchAll>c__AnonStorey.c = c;
						this.<>f__ref$5.maybeD.Match(new Action<D>(<MatchAll>c__AnonStorey.<>m__0));
					}

					// Token: 0x04006880 RID: 26752
					internal B b;

					// Token: 0x04006881 RID: 26753
					internal Maybe.<MatchAll>c__AnonStorey5<A, B, C, D> <>f__ref$5;

					// Token: 0x04006882 RID: 26754
					internal Maybe.<MatchAll>c__AnonStorey5<A, B, C, D>.<MatchAll>c__AnonStorey6 <>f__ref$6;

					// Token: 0x02000F9A RID: 3994
					private sealed class <MatchAll>c__AnonStorey8
					{
						// Token: 0x0600746F RID: 29807 RVA: 0x000E0029 File Offset: 0x000DE429
						public <MatchAll>c__AnonStorey8()
						{
						}

						// Token: 0x06007470 RID: 29808 RVA: 0x000E0031 File Offset: 0x000DE431
						internal void <>m__0(D d)
						{
							this.<>f__ref$5.action(this.<>f__ref$6.a, this.<>f__ref$7.b, this.c, d);
						}

						// Token: 0x04006883 RID: 26755
						internal C c;

						// Token: 0x04006884 RID: 26756
						internal Maybe.<MatchAll>c__AnonStorey5<A, B, C, D> <>f__ref$5;

						// Token: 0x04006885 RID: 26757
						internal Maybe.<MatchAll>c__AnonStorey5<A, B, C, D>.<MatchAll>c__AnonStorey6 <>f__ref$6;

						// Token: 0x04006886 RID: 26758
						internal Maybe.<MatchAll>c__AnonStorey5<A, B, C, D>.<MatchAll>c__AnonStorey6.<MatchAll>c__AnonStorey7 <>f__ref$7;
					}
				}
			}
		}
	}
}
