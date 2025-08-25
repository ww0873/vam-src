using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace SimpleJSON
{
	// Token: 0x02000D00 RID: 3328
	public class JSONClass : JSONNode, IEnumerable
	{
		// Token: 0x06006576 RID: 25974 RVA: 0x0026472B File Offset: 0x00262B2B
		public JSONClass()
		{
		}

		// Token: 0x17000EF4 RID: 3828
		public override JSONNode this[string aKey]
		{
			get
			{
				if (this.m_Dict.ContainsKey(aKey))
				{
					return this.m_Dict[aKey];
				}
				return new JSONLazyCreator(this, aKey);
			}
			set
			{
				if (this.m_Dict.ContainsKey(aKey))
				{
					this.m_Dict[aKey] = value;
				}
				else
				{
					this.m_Dict.Add(aKey, value);
				}
			}
		}

		// Token: 0x17000EF5 RID: 3829
		public override JSONNode this[int aIndex]
		{
			get
			{
				if (aIndex < 0 || aIndex >= this.m_Dict.Count)
				{
					return null;
				}
				return this.m_Dict.ElementAt(aIndex).Value;
			}
			set
			{
				if (aIndex < 0 || aIndex >= this.m_Dict.Count)
				{
					return;
				}
				string key = this.m_Dict.ElementAt(aIndex).Key;
				this.m_Dict[key] = value;
			}
		}

		// Token: 0x17000EF6 RID: 3830
		// (get) Token: 0x0600657B RID: 25979 RVA: 0x0026481C File Offset: 0x00262C1C
		public override int Count
		{
			get
			{
				return this.m_Dict.Count;
			}
		}

		// Token: 0x0600657C RID: 25980 RVA: 0x00264829 File Offset: 0x00262C29
		public bool HasKey(string aKey)
		{
			return this.m_Dict.ContainsKey(aKey);
		}

		// Token: 0x0600657D RID: 25981 RVA: 0x00264838 File Offset: 0x00262C38
		public override void Add(string aKey, JSONNode aItem)
		{
			if (!string.IsNullOrEmpty(aKey))
			{
				if (this.m_Dict.ContainsKey(aKey))
				{
					this.m_Dict[aKey] = aItem;
				}
				else
				{
					this.m_Dict.Add(aKey, aItem);
				}
			}
			else
			{
				this.m_Dict.Add(Guid.NewGuid().ToString(), aItem);
			}
		}

		// Token: 0x0600657E RID: 25982 RVA: 0x002648A4 File Offset: 0x00262CA4
		public override JSONNode Remove(string aKey)
		{
			if (!this.m_Dict.ContainsKey(aKey))
			{
				return null;
			}
			JSONNode result = this.m_Dict[aKey];
			this.m_Dict.Remove(aKey);
			return result;
		}

		// Token: 0x0600657F RID: 25983 RVA: 0x002648E0 File Offset: 0x00262CE0
		public override JSONNode Remove(int aIndex)
		{
			if (aIndex < 0 || aIndex >= this.m_Dict.Count)
			{
				return null;
			}
			KeyValuePair<string, JSONNode> keyValuePair = this.m_Dict.ElementAt(aIndex);
			this.m_Dict.Remove(keyValuePair.Key);
			return keyValuePair.Value;
		}

		// Token: 0x06006580 RID: 25984 RVA: 0x00264930 File Offset: 0x00262D30
		public override JSONNode Remove(JSONNode aNode)
		{
			JSONClass.<Remove>c__AnonStorey3 <Remove>c__AnonStorey = new JSONClass.<Remove>c__AnonStorey3();
			<Remove>c__AnonStorey.aNode = aNode;
			JSONNode result;
			try
			{
				KeyValuePair<string, JSONNode> keyValuePair = this.m_Dict.Where(new Func<KeyValuePair<string, JSONNode>, bool>(<Remove>c__AnonStorey.<>m__0)).First<KeyValuePair<string, JSONNode>>();
				this.m_Dict.Remove(keyValuePair.Key);
				result = <Remove>c__AnonStorey.aNode;
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x17000EF7 RID: 3831
		// (get) Token: 0x06006581 RID: 25985 RVA: 0x002649A0 File Offset: 0x00262DA0
		public IEnumerable<string> Keys
		{
			get
			{
				foreach (KeyValuePair<string, JSONNode> N in this.m_Dict)
				{
					yield return N.Key;
				}
				yield break;
			}
		}

		// Token: 0x17000EF8 RID: 3832
		// (get) Token: 0x06006582 RID: 25986 RVA: 0x002649C4 File Offset: 0x00262DC4
		public override IEnumerable<JSONNode> Childs
		{
			get
			{
				foreach (KeyValuePair<string, JSONNode> N in this.m_Dict)
				{
					yield return N.Value;
				}
				yield break;
			}
		}

		// Token: 0x06006583 RID: 25987 RVA: 0x002649E8 File Offset: 0x00262DE8
		public IEnumerator GetEnumerator()
		{
			foreach (KeyValuePair<string, JSONNode> N in this.m_Dict)
			{
				yield return N;
			}
			yield break;
		}

		// Token: 0x06006584 RID: 25988 RVA: 0x00264A04 File Offset: 0x00262E04
		public override string ToString()
		{
			string text = "{";
			foreach (KeyValuePair<string, JSONNode> keyValuePair in this.m_Dict)
			{
				if (text.Length > 2)
				{
					text += ", ";
				}
				string text2 = text;
				text = string.Concat(new string[]
				{
					text2,
					"\"",
					JSONNode.Escape(keyValuePair.Key),
					"\":",
					keyValuePair.Value.ToString()
				});
			}
			text += "}";
			return text;
		}

		// Token: 0x06006585 RID: 25989 RVA: 0x00264AC4 File Offset: 0x00262EC4
		public override string ToString(string aPrefix)
		{
			string text = "{ ";
			foreach (KeyValuePair<string, JSONNode> keyValuePair in this.m_Dict)
			{
				if (text.Length > 3)
				{
					text += ", ";
				}
				text = text + "\n" + aPrefix + "   ";
				string text2 = text;
				text = string.Concat(new string[]
				{
					text2,
					"\"",
					JSONNode.Escape(keyValuePair.Key),
					"\" : ",
					keyValuePair.Value.ToString(aPrefix + "   ")
				});
			}
			text = text + "\n" + aPrefix + "}";
			return text;
		}

		// Token: 0x06006586 RID: 25990 RVA: 0x00264BA8 File Offset: 0x00262FA8
		public override void ToString(string aPrefix, StringBuilder sb)
		{
			bool flag = true;
			sb.Append("{ ");
			foreach (KeyValuePair<string, JSONNode> keyValuePair in this.m_Dict)
			{
				if (!flag)
				{
					sb.Append(", ");
				}
				flag = false;
				sb.Append("\n" + aPrefix + "   ");
				sb.Append("\"");
				JSONNode.EscapeAppend(keyValuePair.Key, sb);
				sb.Append("\" : ");
				keyValuePair.Value.ToString(aPrefix + "   ", sb);
			}
			sb.Append("\n" + aPrefix + "}");
		}

		// Token: 0x06006587 RID: 25991 RVA: 0x00264C8C File Offset: 0x0026308C
		public override void Serialize(BinaryWriter aWriter)
		{
			aWriter.Write(2);
			aWriter.Write(this.m_Dict.Count);
			foreach (string text in this.m_Dict.Keys)
			{
				aWriter.Write(text);
				this.m_Dict[text].Serialize(aWriter);
			}
		}

		// Token: 0x04005505 RID: 21765
		private Dictionary<string, JSONNode> m_Dict = new Dictionary<string, JSONNode>();

		// Token: 0x0200102F RID: 4143
		[CompilerGenerated]
		private sealed class <Remove>c__AnonStorey3
		{
			// Token: 0x06007747 RID: 30535 RVA: 0x00264D18 File Offset: 0x00263118
			public <Remove>c__AnonStorey3()
			{
			}

			// Token: 0x06007748 RID: 30536 RVA: 0x00264D20 File Offset: 0x00263120
			internal bool <>m__0(KeyValuePair<string, JSONNode> k)
			{
				return k.Value == this.aNode;
			}

			// Token: 0x04006B51 RID: 27473
			internal JSONNode aNode;
		}

		// Token: 0x02001030 RID: 4144
		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
		{
			// Token: 0x06007749 RID: 30537 RVA: 0x00264D34 File Offset: 0x00263134
			[DebuggerHidden]
			public <>c__Iterator0()
			{
			}

			// Token: 0x0600774A RID: 30538 RVA: 0x00264D3C File Offset: 0x0026313C
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0U:
					enumerator = this.m_Dict.GetEnumerator();
					num = 4294967293U;
					break;
				case 1U:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						N = enumerator.Current;
						this.$current = N.Key;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						((IDisposable)enumerator).Dispose();
					}
				}
				this.$PC = -1;
				return false;
			}

			// Token: 0x170011B5 RID: 4533
			// (get) Token: 0x0600774B RID: 30539 RVA: 0x00264E14 File Offset: 0x00263214
			string IEnumerator<string>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170011B6 RID: 4534
			// (get) Token: 0x0600774C RID: 30540 RVA: 0x00264E1C File Offset: 0x0026321C
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x0600774D RID: 30541 RVA: 0x00264E24 File Offset: 0x00263224
			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1U:
					try
					{
					}
					finally
					{
						((IDisposable)enumerator).Dispose();
					}
					break;
				}
			}

			// Token: 0x0600774E RID: 30542 RVA: 0x00264E80 File Offset: 0x00263280
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x0600774F RID: 30543 RVA: 0x00264E87 File Offset: 0x00263287
			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<string>.GetEnumerator();
			}

			// Token: 0x06007750 RID: 30544 RVA: 0x00264E90 File Offset: 0x00263290
			[DebuggerHidden]
			IEnumerator<string> IEnumerable<string>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				JSONClass.<>c__Iterator0 <>c__Iterator = new JSONClass.<>c__Iterator0();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}

			// Token: 0x04006B52 RID: 27474
			internal Dictionary<string, JSONNode>.Enumerator $locvar0;

			// Token: 0x04006B53 RID: 27475
			internal KeyValuePair<string, JSONNode> <N>__1;

			// Token: 0x04006B54 RID: 27476
			internal JSONClass $this;

			// Token: 0x04006B55 RID: 27477
			internal string $current;

			// Token: 0x04006B56 RID: 27478
			internal bool $disposing;

			// Token: 0x04006B57 RID: 27479
			internal int $PC;
		}

		// Token: 0x02001031 RID: 4145
		[CompilerGenerated]
		private sealed class <>c__Iterator1 : IEnumerable, IEnumerable<JSONNode>, IEnumerator, IDisposable, IEnumerator<JSONNode>
		{
			// Token: 0x06007751 RID: 30545 RVA: 0x00264EC4 File Offset: 0x002632C4
			[DebuggerHidden]
			public <>c__Iterator1()
			{
			}

			// Token: 0x06007752 RID: 30546 RVA: 0x00264ECC File Offset: 0x002632CC
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0U:
					enumerator = this.m_Dict.GetEnumerator();
					num = 4294967293U;
					break;
				case 1U:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						N = enumerator.Current;
						this.$current = N.Value;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						((IDisposable)enumerator).Dispose();
					}
				}
				this.$PC = -1;
				return false;
			}

			// Token: 0x170011B7 RID: 4535
			// (get) Token: 0x06007753 RID: 30547 RVA: 0x00264FA4 File Offset: 0x002633A4
			JSONNode IEnumerator<JSONNode>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170011B8 RID: 4536
			// (get) Token: 0x06007754 RID: 30548 RVA: 0x00264FAC File Offset: 0x002633AC
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x06007755 RID: 30549 RVA: 0x00264FB4 File Offset: 0x002633B4
			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1U:
					try
					{
					}
					finally
					{
						((IDisposable)enumerator).Dispose();
					}
					break;
				}
			}

			// Token: 0x06007756 RID: 30550 RVA: 0x00265010 File Offset: 0x00263410
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x06007757 RID: 30551 RVA: 0x00265017 File Offset: 0x00263417
			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<SimpleJSON.JSONNode>.GetEnumerator();
			}

			// Token: 0x06007758 RID: 30552 RVA: 0x00265020 File Offset: 0x00263420
			[DebuggerHidden]
			IEnumerator<JSONNode> IEnumerable<JSONNode>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				JSONClass.<>c__Iterator1 <>c__Iterator = new JSONClass.<>c__Iterator1();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}

			// Token: 0x04006B58 RID: 27480
			internal Dictionary<string, JSONNode>.Enumerator $locvar0;

			// Token: 0x04006B59 RID: 27481
			internal KeyValuePair<string, JSONNode> <N>__1;

			// Token: 0x04006B5A RID: 27482
			internal JSONClass $this;

			// Token: 0x04006B5B RID: 27483
			internal JSONNode $current;

			// Token: 0x04006B5C RID: 27484
			internal bool $disposing;

			// Token: 0x04006B5D RID: 27485
			internal int $PC;
		}

		// Token: 0x02001032 RID: 4146
		[CompilerGenerated]
		private sealed class <GetEnumerator>c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x06007759 RID: 30553 RVA: 0x00265054 File Offset: 0x00263454
			[DebuggerHidden]
			public <GetEnumerator>c__Iterator2()
			{
			}

			// Token: 0x0600775A RID: 30554 RVA: 0x0026505C File Offset: 0x0026345C
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0U:
					enumerator = this.m_Dict.GetEnumerator();
					num = 4294967293U;
					break;
				case 1U:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						N = enumerator.Current;
						this.$current = N;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						((IDisposable)enumerator).Dispose();
					}
				}
				this.$PC = -1;
				return false;
			}

			// Token: 0x170011B9 RID: 4537
			// (get) Token: 0x0600775B RID: 30555 RVA: 0x00265134 File Offset: 0x00263534
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170011BA RID: 4538
			// (get) Token: 0x0600775C RID: 30556 RVA: 0x0026513C File Offset: 0x0026353C
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x0600775D RID: 30557 RVA: 0x00265144 File Offset: 0x00263544
			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1U:
					try
					{
					}
					finally
					{
						((IDisposable)enumerator).Dispose();
					}
					break;
				}
			}

			// Token: 0x0600775E RID: 30558 RVA: 0x002651A0 File Offset: 0x002635A0
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x04006B5E RID: 27486
			internal Dictionary<string, JSONNode>.Enumerator $locvar0;

			// Token: 0x04006B5F RID: 27487
			internal KeyValuePair<string, JSONNode> <N>__1;

			// Token: 0x04006B60 RID: 27488
			internal JSONClass $this;

			// Token: 0x04006B61 RID: 27489
			internal object $current;

			// Token: 0x04006B62 RID: 27490
			internal bool $disposing;

			// Token: 0x04006B63 RID: 27491
			internal int $PC;
		}
	}
}
