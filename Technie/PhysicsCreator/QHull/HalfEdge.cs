using System;

namespace Technie.PhysicsCreator.QHull
{
	// Token: 0x02000459 RID: 1113
	public class HalfEdge
	{
		// Token: 0x06001BAE RID: 7086 RVA: 0x0009C59B File Offset: 0x0009A99B
		public HalfEdge(Vertex v, Face f)
		{
			this.vertex = v;
			this.face = f;
		}

		// Token: 0x06001BAF RID: 7087 RVA: 0x0009C5B1 File Offset: 0x0009A9B1
		public HalfEdge()
		{
		}

		// Token: 0x06001BB0 RID: 7088 RVA: 0x0009C5B9 File Offset: 0x0009A9B9
		public void setNext(HalfEdge edge)
		{
			this.next = edge;
		}

		// Token: 0x06001BB1 RID: 7089 RVA: 0x0009C5C2 File Offset: 0x0009A9C2
		public HalfEdge getNext()
		{
			return this.next;
		}

		// Token: 0x06001BB2 RID: 7090 RVA: 0x0009C5CA File Offset: 0x0009A9CA
		public void setPrev(HalfEdge edge)
		{
			this.prev = edge;
		}

		// Token: 0x06001BB3 RID: 7091 RVA: 0x0009C5D3 File Offset: 0x0009A9D3
		public HalfEdge getPrev()
		{
			return this.prev;
		}

		// Token: 0x06001BB4 RID: 7092 RVA: 0x0009C5DB File Offset: 0x0009A9DB
		public Face getFace()
		{
			return this.face;
		}

		// Token: 0x06001BB5 RID: 7093 RVA: 0x0009C5E3 File Offset: 0x0009A9E3
		public HalfEdge getOpposite()
		{
			return this.opposite;
		}

		// Token: 0x06001BB6 RID: 7094 RVA: 0x0009C5EB File Offset: 0x0009A9EB
		public void setOpposite(HalfEdge edge)
		{
			this.opposite = edge;
			edge.opposite = this;
		}

		// Token: 0x06001BB7 RID: 7095 RVA: 0x0009C5FB File Offset: 0x0009A9FB
		public Vertex head()
		{
			return this.vertex;
		}

		// Token: 0x06001BB8 RID: 7096 RVA: 0x0009C603 File Offset: 0x0009AA03
		public Vertex tail()
		{
			return (this.prev == null) ? null : this.prev.vertex;
		}

		// Token: 0x06001BB9 RID: 7097 RVA: 0x0009C621 File Offset: 0x0009AA21
		public Face oppositeFace()
		{
			return (this.opposite == null) ? null : this.opposite.face;
		}

		// Token: 0x06001BBA RID: 7098 RVA: 0x0009C640 File Offset: 0x0009AA40
		public string getVertexString()
		{
			if (this.tail() != null)
			{
				return string.Concat(new object[]
				{
					string.Empty,
					this.tail().index,
					"-",
					this.head().index
				});
			}
			return "?-" + this.head().index;
		}

		// Token: 0x06001BBB RID: 7099 RVA: 0x0009C6B4 File Offset: 0x0009AAB4
		public double length()
		{
			if (this.tail() != null)
			{
				return this.head().pnt.distance(this.tail().pnt);
			}
			return -1.0;
		}

		// Token: 0x06001BBC RID: 7100 RVA: 0x0009C6E6 File Offset: 0x0009AAE6
		public double lengthSquared()
		{
			if (this.tail() != null)
			{
				return this.head().pnt.distanceSquared(this.tail().pnt);
			}
			return -1.0;
		}

		// Token: 0x040017A3 RID: 6051
		public Vertex vertex;

		// Token: 0x040017A4 RID: 6052
		public Face face;

		// Token: 0x040017A5 RID: 6053
		public HalfEdge next;

		// Token: 0x040017A6 RID: 6054
		public HalfEdge prev;

		// Token: 0x040017A7 RID: 6055
		public HalfEdge opposite;
	}
}
