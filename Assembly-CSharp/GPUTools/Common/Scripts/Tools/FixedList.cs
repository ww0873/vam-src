using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace GPUTools.Common.Scripts.Tools
{
	// Token: 0x020009CB RID: 2507
	public class FixedList<T>
	{
		// Token: 0x06003F42 RID: 16194 RVA: 0x0012ED5E File Offset: 0x0012D15E
		public FixedList(int size)
		{
			this.Data = new T[size];
			this.Size = size;
			this.Count = 0;
		}

		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x06003F44 RID: 16196 RVA: 0x0012ED89 File Offset: 0x0012D189
		// (set) Token: 0x06003F43 RID: 16195 RVA: 0x0012ED80 File Offset: 0x0012D180
		public int Size
		{
			[CompilerGenerated]
			get
			{
				return this.<Size>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Size>k__BackingField = value;
			}
		}

		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x06003F46 RID: 16198 RVA: 0x0012ED9A File Offset: 0x0012D19A
		// (set) Token: 0x06003F45 RID: 16197 RVA: 0x0012ED91 File Offset: 0x0012D191
		public int Count
		{
			[CompilerGenerated]
			get
			{
				return this.<Count>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Count>k__BackingField = value;
			}
		}

		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x06003F48 RID: 16200 RVA: 0x0012EDAB File Offset: 0x0012D1AB
		// (set) Token: 0x06003F47 RID: 16199 RVA: 0x0012EDA2 File Offset: 0x0012D1A2
		public T[] Data
		{
			[CompilerGenerated]
			get
			{
				return this.<Data>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Data>k__BackingField = value;
			}
		}

		// Token: 0x06003F49 RID: 16201 RVA: 0x0012EDB3 File Offset: 0x0012D1B3
		public void Add(T item)
		{
			this.Data[this.Count] = item;
			this.Count++;
		}

		// Token: 0x1700075F RID: 1887
		public T this[int i]
		{
			get
			{
				return this.Data[i];
			}
			set
			{
				this.Data[i] = value;
			}
		}

		// Token: 0x06003F4C RID: 16204 RVA: 0x0012EDF2 File Offset: 0x0012D1F2
		public void Reset()
		{
			this.Count = 0;
		}

		// Token: 0x06003F4D RID: 16205 RVA: 0x0012EDFC File Offset: 0x0012D1FC
		public bool Contains(T item)
		{
			for (int i = 0; i < this.Count; i++)
			{
				if (this.Data[i].Equals(item))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04002FFC RID: 12284
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private int <Size>k__BackingField;

		// Token: 0x04002FFD RID: 12285
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private int <Count>k__BackingField;

		// Token: 0x04002FFE RID: 12286
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private T[] <Data>k__BackingField;
	}
}
