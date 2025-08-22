using System;
using System.Collections.Generic;

namespace Leap
{
	// Token: 0x020005DC RID: 1500
	[Serializable]
	public class Frame : IEquatable<Frame>
	{
		// Token: 0x060025C8 RID: 9672 RVA: 0x000D6FC9 File Offset: 0x000D53C9
		public Frame()
		{
			this.Hands = new List<Hand>();
		}

		// Token: 0x060025C9 RID: 9673 RVA: 0x000D6FDC File Offset: 0x000D53DC
		public Frame(long id, long timestamp, float fps, List<Hand> hands)
		{
			this.Id = id;
			this.Timestamp = timestamp;
			this.CurrentFramesPerSecond = fps;
			this.Hands = hands;
		}

		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x060025CA RID: 9674 RVA: 0x000D7001 File Offset: 0x000D5401
		[Obsolete]
		public int SerializeLength
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x060025CB RID: 9675 RVA: 0x000D7008 File Offset: 0x000D5408
		[Obsolete]
		public byte[] Serialize
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x060025CC RID: 9676 RVA: 0x000D700F File Offset: 0x000D540F
		[Obsolete]
		public void Deserialize(byte[] arg)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060025CD RID: 9677 RVA: 0x000D7018 File Offset: 0x000D5418
		public Hand Hand(int id)
		{
			int count = this.Hands.Count;
			while (count-- != 0)
			{
				if (this.Hands[count].Id == id)
				{
					return this.Hands[count];
				}
			}
			return null;
		}

		// Token: 0x060025CE RID: 9678 RVA: 0x000D7065 File Offset: 0x000D5465
		public bool Equals(Frame other)
		{
			return this.Id == other.Id && this.Timestamp == other.Timestamp;
		}

		// Token: 0x060025CF RID: 9679 RVA: 0x000D7089 File Offset: 0x000D5489
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Frame id: ",
				this.Id,
				" timestamp: ",
				this.Timestamp
			});
		}

		// Token: 0x060025D0 RID: 9680 RVA: 0x000D70C4 File Offset: 0x000D54C4
		internal void ResizeHandList(int count)
		{
			if (Frame._handPool == null)
			{
				Frame._handPool = new Queue<Hand>();
			}
			while (this.Hands.Count < count)
			{
				Hand item;
				if (Frame._handPool.Count > 0)
				{
					item = Frame._handPool.Dequeue();
				}
				else
				{
					item = new Hand();
				}
				this.Hands.Add(item);
			}
			while (this.Hands.Count > count)
			{
				Hand item2 = this.Hands[this.Hands.Count - 1];
				this.Hands.RemoveAt(this.Hands.Count - 1);
				Frame._handPool.Enqueue(item2);
			}
		}

		// Token: 0x04001F7D RID: 8061
		[ThreadStatic]
		private static Queue<Hand> _handPool;

		// Token: 0x04001F7E RID: 8062
		public long Id;

		// Token: 0x04001F7F RID: 8063
		public long Timestamp;

		// Token: 0x04001F80 RID: 8064
		public float CurrentFramesPerSecond;

		// Token: 0x04001F81 RID: 8065
		public List<Hand> Hands;
	}
}
