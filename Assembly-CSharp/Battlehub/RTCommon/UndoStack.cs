using System;
using System.Collections;

namespace Battlehub.RTCommon
{
	// Token: 0x020000D1 RID: 209
	public class UndoStack<T> : IEnumerable
	{
		// Token: 0x060003E4 RID: 996 RVA: 0x0001675F File Offset: 0x00014B5F
		public UndoStack(int size)
		{
			if (size == 0)
			{
				throw new ArgumentException("size should be greater than 0", "size");
			}
			this.m_buffer = new T[size];
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060003E5 RID: 997 RVA: 0x00016789 File Offset: 0x00014B89
		public int Count
		{
			get
			{
				return this.m_count;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060003E6 RID: 998 RVA: 0x00016791 File Offset: 0x00014B91
		public bool CanPop
		{
			get
			{
				return this.m_count > 0;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060003E7 RID: 999 RVA: 0x0001679C File Offset: 0x00014B9C
		public bool CanRestore
		{
			get
			{
				return this.m_count < this.m_totalCount;
			}
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x000167AC File Offset: 0x00014BAC
		public T Push(T item)
		{
			T result = this.m_buffer[this.m_tosIndex];
			this.m_buffer[this.m_tosIndex] = item;
			this.m_tosIndex++;
			this.m_tosIndex %= this.m_buffer.Length;
			if (this.m_count < this.m_buffer.Length)
			{
				this.m_count++;
				result = default(T);
			}
			this.m_totalCount = this.m_count;
			return result;
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x00016838 File Offset: 0x00014C38
		public T Restore()
		{
			if (!this.CanRestore)
			{
				throw new InvalidOperationException("nothing to restore");
			}
			if (this.m_count < this.m_totalCount)
			{
				this.m_tosIndex++;
				this.m_tosIndex %= this.m_buffer.Length;
				this.m_count++;
			}
			return this.Peek();
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x000168A4 File Offset: 0x00014CA4
		public T Peek()
		{
			if (this.m_count == 0)
			{
				throw new InvalidOperationException("Stack is empty");
			}
			int num = this.m_tosIndex - 1;
			if (num < 0)
			{
				num = this.m_buffer.Length - 1;
			}
			return this.m_buffer[num];
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x000168F0 File Offset: 0x00014CF0
		public T Pop()
		{
			if (this.m_count == 0)
			{
				throw new InvalidOperationException("Stack is empty");
			}
			this.m_count--;
			this.m_tosIndex--;
			if (this.m_tosIndex < 0)
			{
				this.m_tosIndex = this.m_buffer.Length - 1;
			}
			return this.m_buffer[this.m_tosIndex];
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0001695C File Offset: 0x00014D5C
		public void Clear()
		{
			this.m_tosIndex = 0;
			this.m_count = 0;
			this.m_totalCount = 0;
			for (int i = 0; i < this.m_buffer.Length; i++)
			{
				this.m_buffer[i] = default(T);
			}
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x000169AC File Offset: 0x00014DAC
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.m_buffer.GetEnumerator();
		}

		// Token: 0x0400042D RID: 1069
		private int m_tosIndex;

		// Token: 0x0400042E RID: 1070
		private T[] m_buffer;

		// Token: 0x0400042F RID: 1071
		private int m_count;

		// Token: 0x04000430 RID: 1072
		private int m_totalCount;
	}
}
