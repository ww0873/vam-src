using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace UnityThreading
{
	// Token: 0x02000342 RID: 834
	public class Channel<T> : IDisposable
	{
		// Token: 0x0600142F RID: 5167 RVA: 0x000749A0 File Offset: 0x00072DA0
		public Channel() : this(1)
		{
		}

		// Token: 0x06001430 RID: 5168 RVA: 0x000749AC File Offset: 0x00072DAC
		public Channel(int bufferSize)
		{
			if (bufferSize < 1)
			{
				throw new ArgumentOutOfRangeException("bufferSize", "Must be greater or equal to 1.");
			}
			this.BufferSize = bufferSize;
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06001431 RID: 5169 RVA: 0x00074A2D File Offset: 0x00072E2D
		// (set) Token: 0x06001432 RID: 5170 RVA: 0x00074A35 File Offset: 0x00072E35
		public int BufferSize
		{
			[CompilerGenerated]
			get
			{
				return this.<BufferSize>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<BufferSize>k__BackingField = value;
			}
		}

		// Token: 0x06001433 RID: 5171 RVA: 0x00074A40 File Offset: 0x00072E40
		~Channel()
		{
			this.Dispose();
		}

		// Token: 0x06001434 RID: 5172 RVA: 0x00074A70 File Offset: 0x00072E70
		public void Resize(int newBufferSize)
		{
			if (newBufferSize < 1)
			{
				throw new ArgumentOutOfRangeException("newBufferSize", "Must be greater or equal to 1.");
			}
			object obj = this.setSyncRoot;
			lock (obj)
			{
				if (!this.disposed)
				{
					if (WaitHandle.WaitAny(new WaitHandle[]
					{
						this.exitEvent,
						this.getEvent
					}) != 0)
					{
						this.buffer.Clear();
						if (newBufferSize != this.BufferSize)
						{
							this.BufferSize = newBufferSize;
						}
					}
				}
			}
		}

		// Token: 0x06001435 RID: 5173 RVA: 0x00074B14 File Offset: 0x00072F14
		public bool Set(T value)
		{
			return this.Set(value, int.MaxValue);
		}

		// Token: 0x06001436 RID: 5174 RVA: 0x00074B24 File Offset: 0x00072F24
		public bool Set(T value, int timeoutInMilliseconds)
		{
			object obj = this.setSyncRoot;
			bool result;
			lock (obj)
			{
				if (this.disposed)
				{
					result = false;
				}
				else
				{
					int num = WaitHandle.WaitAny(new WaitHandle[]
					{
						this.exitEvent,
						this.getEvent
					}, timeoutInMilliseconds);
					if (num == 258 || num == 0)
					{
						result = false;
					}
					else
					{
						this.buffer.Add(value);
						if (this.buffer.Count == this.BufferSize)
						{
							this.setEvent.Set();
							this.getEvent.Reset();
						}
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x06001437 RID: 5175 RVA: 0x00074BE0 File Offset: 0x00072FE0
		public T Get()
		{
			return this.Get(int.MaxValue, default(T));
		}

		// Token: 0x06001438 RID: 5176 RVA: 0x00074C04 File Offset: 0x00073004
		public T Get(int timeoutInMilliseconds, T defaultValue)
		{
			object obj = this.getSyncRoot;
			T result;
			lock (obj)
			{
				if (this.disposed)
				{
					result = defaultValue;
				}
				else
				{
					int num = WaitHandle.WaitAny(new WaitHandle[]
					{
						this.exitEvent,
						this.setEvent
					}, timeoutInMilliseconds);
					if (num == 258 || num == 0)
					{
						result = defaultValue;
					}
					else
					{
						T t = this.buffer[0];
						this.buffer.RemoveAt(0);
						if (this.buffer.Count == 0)
						{
							this.getEvent.Set();
							this.setEvent.Reset();
						}
						result = t;
					}
				}
			}
			return result;
		}

		// Token: 0x06001439 RID: 5177 RVA: 0x00074CC8 File Offset: 0x000730C8
		public void Close()
		{
			object obj = this.disposeRoot;
			lock (obj)
			{
				if (!this.disposed)
				{
					this.exitEvent.Set();
				}
			}
		}

		// Token: 0x0600143A RID: 5178 RVA: 0x00074D1C File Offset: 0x0007311C
		public void Dispose()
		{
			if (this.disposed)
			{
				return;
			}
			object obj = this.disposeRoot;
			lock (obj)
			{
				this.exitEvent.Set();
				object obj2 = this.getSyncRoot;
				lock (obj2)
				{
					object obj3 = this.setSyncRoot;
					lock (obj3)
					{
						this.setEvent.Close();
						this.setEvent = null;
						this.getEvent.Close();
						this.getEvent = null;
						this.exitEvent.Close();
						this.exitEvent = null;
						this.disposed = true;
					}
				}
			}
		}

		// Token: 0x04001178 RID: 4472
		private List<T> buffer = new List<T>();

		// Token: 0x04001179 RID: 4473
		private object setSyncRoot = new object();

		// Token: 0x0400117A RID: 4474
		private object getSyncRoot = new object();

		// Token: 0x0400117B RID: 4475
		private object disposeRoot = new object();

		// Token: 0x0400117C RID: 4476
		private ManualResetEvent setEvent = new ManualResetEvent(false);

		// Token: 0x0400117D RID: 4477
		private ManualResetEvent getEvent = new ManualResetEvent(true);

		// Token: 0x0400117E RID: 4478
		private ManualResetEvent exitEvent = new ManualResetEvent(false);

		// Token: 0x0400117F RID: 4479
		private bool disposed;

		// Token: 0x04001180 RID: 4480
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private int <BufferSize>k__BackingField;
	}
}
