using System;

namespace Technie.PhysicsCreator.QHull
{
	// Token: 0x02000458 RID: 1112
	public class FaceList
	{
		// Token: 0x06001BA9 RID: 7081 RVA: 0x0009C52F File Offset: 0x0009A92F
		public FaceList()
		{
		}

		// Token: 0x06001BAA RID: 7082 RVA: 0x0009C538 File Offset: 0x0009A938
		public void clear()
		{
			this.head = (this.tail = null);
		}

		// Token: 0x06001BAB RID: 7083 RVA: 0x0009C555 File Offset: 0x0009A955
		public void add(Face vtx)
		{
			if (this.head == null)
			{
				this.head = vtx;
			}
			else
			{
				this.tail.next = vtx;
			}
			vtx.next = null;
			this.tail = vtx;
		}

		// Token: 0x06001BAC RID: 7084 RVA: 0x0009C588 File Offset: 0x0009A988
		public Face first()
		{
			return this.head;
		}

		// Token: 0x06001BAD RID: 7085 RVA: 0x0009C590 File Offset: 0x0009A990
		public bool isEmpty()
		{
			return this.head == null;
		}

		// Token: 0x040017A1 RID: 6049
		private Face head;

		// Token: 0x040017A2 RID: 6050
		private Face tail;
	}
}
