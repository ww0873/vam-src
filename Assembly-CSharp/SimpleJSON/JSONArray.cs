using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace SimpleJSON
{
	// Token: 0x02000CFF RID: 3327
	public class JSONArray : JSONNode, IEnumerable
	{
		// Token: 0x06006567 RID: 25959 RVA: 0x002640C4 File Offset: 0x002624C4
		public JSONArray()
		{
		}

		// Token: 0x17000EF0 RID: 3824
		public override JSONNode this[int aIndex]
		{
			get
			{
				if (aIndex < 0 || aIndex >= this.m_List.Count)
				{
					return new JSONLazyCreator(this);
				}
				return this.m_List[aIndex];
			}
			set
			{
				if (aIndex < 0 || aIndex >= this.m_List.Count)
				{
					this.m_List.Add(value);
				}
				else
				{
					this.m_List[aIndex] = value;
				}
			}
		}

		// Token: 0x17000EF1 RID: 3825
		public override JSONNode this[string aKey]
		{
			get
			{
				return new JSONLazyCreator(this);
			}
			set
			{
				this.m_List.Add(value);
			}
		}

		// Token: 0x17000EF2 RID: 3826
		// (get) Token: 0x0600656C RID: 25964 RVA: 0x00264152 File Offset: 0x00262552
		public override int Count
		{
			get
			{
				return this.m_List.Count;
			}
		}

		// Token: 0x0600656D RID: 25965 RVA: 0x0026415F File Offset: 0x0026255F
		public override void Add(string aKey, JSONNode aItem)
		{
			this.m_List.Add(aItem);
		}

		// Token: 0x0600656E RID: 25966 RVA: 0x00264170 File Offset: 0x00262570
		public override JSONNode Remove(int aIndex)
		{
			if (aIndex < 0 || aIndex >= this.m_List.Count)
			{
				return null;
			}
			JSONNode result = this.m_List[aIndex];
			this.m_List.RemoveAt(aIndex);
			return result;
		}

		// Token: 0x0600656F RID: 25967 RVA: 0x002641B1 File Offset: 0x002625B1
		public override JSONNode Remove(JSONNode aNode)
		{
			this.m_List.Remove(aNode);
			return aNode;
		}

		// Token: 0x17000EF3 RID: 3827
		// (get) Token: 0x06006570 RID: 25968 RVA: 0x002641C4 File Offset: 0x002625C4
		public override IEnumerable<JSONNode> Childs
		{
			get
			{
				foreach (JSONNode N in this.m_List)
				{
					yield return N;
				}
				yield break;
			}
		}

		// Token: 0x06006571 RID: 25969 RVA: 0x002641E8 File Offset: 0x002625E8
		public IEnumerator GetEnumerator()
		{
			foreach (JSONNode N in this.m_List)
			{
				yield return N;
			}
			yield break;
		}

		// Token: 0x06006572 RID: 25970 RVA: 0x00264204 File Offset: 0x00262604
		public override string ToString()
		{
			string text = "[ ";
			foreach (JSONNode jsonnode in this.m_List)
			{
				if (text.Length > 2)
				{
					text += ", ";
				}
				text += jsonnode.ToString();
			}
			text += " ]";
			return text;
		}

		// Token: 0x06006573 RID: 25971 RVA: 0x00264294 File Offset: 0x00262694
		public override string ToString(string aPrefix)
		{
			string text = "[ ";
			foreach (JSONNode jsonnode in this.m_List)
			{
				if (text.Length > 3)
				{
					text += ", ";
				}
				text = text + "\n" + aPrefix + "   ";
				text += jsonnode.ToString(aPrefix + "   ");
			}
			text = text + "\n" + aPrefix + "]";
			return text;
		}

		// Token: 0x06006574 RID: 25972 RVA: 0x00264344 File Offset: 0x00262744
		public override void ToString(string aPrefix, StringBuilder sb)
		{
			sb.Append("[ ");
			bool flag = true;
			foreach (JSONNode jsonnode in this.m_List)
			{
				if (!flag)
				{
					sb.Append(", ");
				}
				flag = false;
				sb.Append("\n" + aPrefix + "   ");
				jsonnode.ToString(aPrefix + "   ", sb);
			}
			sb.Append("\n" + aPrefix + "]");
		}

		// Token: 0x06006575 RID: 25973 RVA: 0x002643FC File Offset: 0x002627FC
		public override void Serialize(BinaryWriter aWriter)
		{
			aWriter.Write(1);
			aWriter.Write(this.m_List.Count);
			for (int i = 0; i < this.m_List.Count; i++)
			{
				this.m_List[i].Serialize(aWriter);
			}
		}

		// Token: 0x04005504 RID: 21764
		private List<JSONNode> m_List = new List<JSONNode>();

		// Token: 0x0200102D RID: 4141
		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<JSONNode>, IEnumerator, IDisposable, IEnumerator<JSONNode>
		{
			// Token: 0x06007739 RID: 30521 RVA: 0x0026444F File Offset: 0x0026284F
			[DebuggerHidden]
			public <>c__Iterator0()
			{
			}

			// Token: 0x0600773A RID: 30522 RVA: 0x00264458 File Offset: 0x00262858
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0U:
					enumerator = this.m_List.GetEnumerator();
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

			// Token: 0x170011B1 RID: 4529
			// (get) Token: 0x0600773B RID: 30523 RVA: 0x0026452C File Offset: 0x0026292C
			JSONNode IEnumerator<JSONNode>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170011B2 RID: 4530
			// (get) Token: 0x0600773C RID: 30524 RVA: 0x00264534 File Offset: 0x00262934
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x0600773D RID: 30525 RVA: 0x0026453C File Offset: 0x0026293C
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

			// Token: 0x0600773E RID: 30526 RVA: 0x00264598 File Offset: 0x00262998
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x0600773F RID: 30527 RVA: 0x0026459F File Offset: 0x0026299F
			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<SimpleJSON.JSONNode>.GetEnumerator();
			}

			// Token: 0x06007740 RID: 30528 RVA: 0x002645A8 File Offset: 0x002629A8
			[DebuggerHidden]
			IEnumerator<JSONNode> IEnumerable<JSONNode>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				JSONArray.<>c__Iterator0 <>c__Iterator = new JSONArray.<>c__Iterator0();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}

			// Token: 0x04006B45 RID: 27461
			internal List<JSONNode>.Enumerator $locvar0;

			// Token: 0x04006B46 RID: 27462
			internal JSONNode <N>__1;

			// Token: 0x04006B47 RID: 27463
			internal JSONArray $this;

			// Token: 0x04006B48 RID: 27464
			internal JSONNode $current;

			// Token: 0x04006B49 RID: 27465
			internal bool $disposing;

			// Token: 0x04006B4A RID: 27466
			internal int $PC;
		}

		// Token: 0x0200102E RID: 4142
		[CompilerGenerated]
		private sealed class <GetEnumerator>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x06007741 RID: 30529 RVA: 0x002645DC File Offset: 0x002629DC
			[DebuggerHidden]
			public <GetEnumerator>c__Iterator1()
			{
			}

			// Token: 0x06007742 RID: 30530 RVA: 0x002645E4 File Offset: 0x002629E4
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0U:
					enumerator = this.m_List.GetEnumerator();
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

			// Token: 0x170011B3 RID: 4531
			// (get) Token: 0x06007743 RID: 30531 RVA: 0x002646B8 File Offset: 0x00262AB8
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170011B4 RID: 4532
			// (get) Token: 0x06007744 RID: 30532 RVA: 0x002646C0 File Offset: 0x00262AC0
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x06007745 RID: 30533 RVA: 0x002646C8 File Offset: 0x00262AC8
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

			// Token: 0x06007746 RID: 30534 RVA: 0x00264724 File Offset: 0x00262B24
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x04006B4B RID: 27467
			internal List<JSONNode>.Enumerator $locvar0;

			// Token: 0x04006B4C RID: 27468
			internal JSONNode <N>__1;

			// Token: 0x04006B4D RID: 27469
			internal JSONArray $this;

			// Token: 0x04006B4E RID: 27470
			internal object $current;

			// Token: 0x04006B4F RID: 27471
			internal bool $disposing;

			// Token: 0x04006B50 RID: 27472
			internal int $PC;
		}
	}
}
