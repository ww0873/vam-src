using System;

namespace Technie.PhysicsCreator.QHull
{
	// Token: 0x02000461 RID: 1121
	internal class VertexList
	{
		// Token: 0x06001C0C RID: 7180 RVA: 0x0009E71B File Offset: 0x0009CB1B
		public VertexList()
		{
		}

		// Token: 0x06001C0D RID: 7181 RVA: 0x0009E724 File Offset: 0x0009CB24
		public void clear()
		{
			this.head = (this.tail = null);
		}

		// Token: 0x06001C0E RID: 7182 RVA: 0x0009E741 File Offset: 0x0009CB41
		public void add(Vertex vtx)
		{
			if (this.head == null)
			{
				this.head = vtx;
			}
			else
			{
				this.tail.next = vtx;
			}
			vtx.prev = this.tail;
			vtx.next = null;
			this.tail = vtx;
		}

		// Token: 0x06001C0F RID: 7183 RVA: 0x0009E780 File Offset: 0x0009CB80
		public void addAll(Vertex vtx)
		{
			if (this.head == null)
			{
				this.head = vtx;
			}
			else
			{
				this.tail.next = vtx;
			}
			vtx.prev = this.tail;
			while (vtx.next != null)
			{
				vtx = vtx.next;
			}
			this.tail = vtx;
		}

		// Token: 0x06001C10 RID: 7184 RVA: 0x0009E7DC File Offset: 0x0009CBDC
		public void delete(Vertex vtx)
		{
			if (vtx.prev == null)
			{
				this.head = vtx.next;
			}
			else
			{
				vtx.prev.next = vtx.next;
			}
			if (vtx.next == null)
			{
				this.tail = vtx.prev;
			}
			else
			{
				vtx.next.prev = vtx.prev;
			}
		}

		// Token: 0x06001C11 RID: 7185 RVA: 0x0009E844 File Offset: 0x0009CC44
		public void delete(Vertex vtx1, Vertex vtx2)
		{
			if (vtx1.prev == null)
			{
				this.head = vtx2.next;
			}
			else
			{
				vtx1.prev.next = vtx2.next;
			}
			if (vtx2.next == null)
			{
				this.tail = vtx1.prev;
			}
			else
			{
				vtx2.next.prev = vtx1.prev;
			}
		}

		// Token: 0x06001C12 RID: 7186 RVA: 0x0009E8AB File Offset: 0x0009CCAB
		public void insertBefore(Vertex vtx, Vertex next)
		{
			vtx.prev = next.prev;
			if (next.prev == null)
			{
				this.head = vtx;
			}
			else
			{
				next.prev.next = vtx;
			}
			vtx.next = next;
			next.prev = vtx;
		}

		// Token: 0x06001C13 RID: 7187 RVA: 0x0009E8EA File Offset: 0x0009CCEA
		public Vertex first()
		{
			return this.head;
		}

		// Token: 0x06001C14 RID: 7188 RVA: 0x0009E8F2 File Offset: 0x0009CCF2
		public bool isEmpty()
		{
			return this.head == null;
		}

		// Token: 0x040017CB RID: 6091
		private Vertex head;

		// Token: 0x040017CC RID: 6092
		private Vertex tail;
	}
}
