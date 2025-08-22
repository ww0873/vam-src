using System;

namespace Technie.PhysicsCreator.QHull
{
	// Token: 0x02000457 RID: 1111
	public class Face
	{
		// Token: 0x06001B92 RID: 7058 RVA: 0x0009B8A5 File Offset: 0x00099CA5
		public Face()
		{
			this.normal = new Vector3d();
			this.centroid = new Point3d();
			this.mark = 1;
		}

		// Token: 0x06001B93 RID: 7059 RVA: 0x0009B8D4 File Offset: 0x00099CD4
		public void computeCentroid(Point3d centroid)
		{
			centroid.setZero();
			HalfEdge halfEdge = this.he0;
			do
			{
				centroid.add(halfEdge.head().pnt);
				halfEdge = halfEdge.next;
			}
			while (halfEdge != this.he0);
			centroid.scale(1.0 / (double)this.numVerts);
		}

		// Token: 0x06001B94 RID: 7060 RVA: 0x0009B92C File Offset: 0x00099D2C
		public void computeNormal(Vector3d normal, double minArea)
		{
			this.computeNormal(normal);
			if (this.area < minArea)
			{
				HalfEdge halfEdge = null;
				double num = 0.0;
				HalfEdge halfEdge2 = this.he0;
				do
				{
					double num2 = halfEdge2.lengthSquared();
					if (num2 > num)
					{
						halfEdge = halfEdge2;
						num = num2;
					}
					halfEdge2 = halfEdge2.next;
				}
				while (halfEdge2 != this.he0);
				Point3d pnt = halfEdge.head().pnt;
				Point3d pnt2 = halfEdge.tail().pnt;
				double num3 = Math.Sqrt(num);
				double num4 = (pnt.x - pnt2.x) / num3;
				double num5 = (pnt.y - pnt2.y) / num3;
				double num6 = (pnt.z - pnt2.z) / num3;
				double num7 = normal.x * num4 + normal.y * num5 + normal.z * num6;
				normal.x -= num7 * num4;
				normal.y -= num7 * num5;
				normal.z -= num7 * num6;
				normal.normalize();
			}
		}

		// Token: 0x06001B95 RID: 7061 RVA: 0x0009BA40 File Offset: 0x00099E40
		public void computeNormal(Vector3d normal)
		{
			HalfEdge halfEdge = this.he0.next;
			HalfEdge halfEdge2 = halfEdge.next;
			Point3d pnt = this.he0.head().pnt;
			Point3d pnt2 = halfEdge.head().pnt;
			double num = pnt2.x - pnt.x;
			double num2 = pnt2.y - pnt.y;
			double num3 = pnt2.z - pnt.z;
			normal.setZero();
			this.numVerts = 2;
			while (halfEdge2 != this.he0)
			{
				double num4 = num;
				double num5 = num2;
				double num6 = num3;
				pnt2 = halfEdge2.head().pnt;
				num = pnt2.x - pnt.x;
				num2 = pnt2.y - pnt.y;
				num3 = pnt2.z - pnt.z;
				normal.x += num5 * num3 - num6 * num2;
				normal.y += num6 * num - num4 * num3;
				normal.z += num4 * num2 - num5 * num;
				halfEdge2 = halfEdge2.next;
				this.numVerts++;
			}
			this.area = normal.norm();
			normal.scale(1.0 / this.area);
		}

		// Token: 0x06001B96 RID: 7062 RVA: 0x0009BB90 File Offset: 0x00099F90
		private void computeNormalAndCentroid()
		{
			this.computeNormal(this.normal);
			this.computeCentroid(this.centroid);
			this.planeOffset = this.normal.dot(this.centroid);
			int num = 0;
			HalfEdge halfEdge = this.he0;
			do
			{
				num++;
				halfEdge = halfEdge.next;
			}
			while (halfEdge != this.he0);
			if (num != this.numVerts)
			{
				throw new InternalErrorException(string.Concat(new object[]
				{
					"face ",
					this.getVertexString(),
					" numVerts=",
					this.numVerts,
					" should be ",
					num
				}));
			}
		}

		// Token: 0x06001B97 RID: 7063 RVA: 0x0009BC41 File Offset: 0x0009A041
		private void computeNormalAndCentroid(double minArea)
		{
			this.computeNormal(this.normal, minArea);
			this.computeCentroid(this.centroid);
			this.planeOffset = this.normal.dot(this.centroid);
		}

		// Token: 0x06001B98 RID: 7064 RVA: 0x0009BC73 File Offset: 0x0009A073
		public static Face createTriangle(Vertex v0, Vertex v1, Vertex v2)
		{
			return Face.createTriangle(v0, v1, v2, 0.0);
		}

		// Token: 0x06001B99 RID: 7065 RVA: 0x0009BC88 File Offset: 0x0009A088
		public static Face createTriangle(Vertex v0, Vertex v1, Vertex v2, double minArea)
		{
			Face face = new Face();
			HalfEdge halfEdge = new HalfEdge(v0, face);
			HalfEdge halfEdge2 = new HalfEdge(v1, face);
			HalfEdge halfEdge3 = new HalfEdge(v2, face);
			halfEdge.prev = halfEdge3;
			halfEdge.next = halfEdge2;
			halfEdge2.prev = halfEdge;
			halfEdge2.next = halfEdge3;
			halfEdge3.prev = halfEdge2;
			halfEdge3.next = halfEdge;
			face.he0 = halfEdge;
			face.computeNormalAndCentroid(minArea);
			return face;
		}

		// Token: 0x06001B9A RID: 7066 RVA: 0x0009BCEC File Offset: 0x0009A0EC
		public static Face create(Vertex[] vtxArray, int[] indices)
		{
			Face face = new Face();
			HalfEdge halfEdge = null;
			for (int i = 0; i < indices.Length; i++)
			{
				HalfEdge halfEdge2 = new HalfEdge(vtxArray[indices[i]], face);
				if (halfEdge != null)
				{
					halfEdge2.setPrev(halfEdge);
					halfEdge.setNext(halfEdge2);
				}
				else
				{
					face.he0 = halfEdge2;
				}
				halfEdge = halfEdge2;
			}
			face.he0.setPrev(halfEdge);
			halfEdge.setNext(face.he0);
			face.computeNormalAndCentroid();
			return face;
		}

		// Token: 0x06001B9B RID: 7067 RVA: 0x0009BD64 File Offset: 0x0009A164
		public HalfEdge getEdge(int i)
		{
			HalfEdge prev = this.he0;
			while (i > 0)
			{
				prev = prev.next;
				i--;
			}
			while (i < 0)
			{
				prev = prev.prev;
				i++;
			}
			return prev;
		}

		// Token: 0x06001B9C RID: 7068 RVA: 0x0009BDA9 File Offset: 0x0009A1A9
		public HalfEdge getFirstEdge()
		{
			return this.he0;
		}

		// Token: 0x06001B9D RID: 7069 RVA: 0x0009BDB4 File Offset: 0x0009A1B4
		public HalfEdge findEdge(Vertex vt, Vertex vh)
		{
			HalfEdge halfEdge = this.he0;
			while (halfEdge.head() != vh || halfEdge.tail() != vt)
			{
				halfEdge = halfEdge.next;
				if (halfEdge == this.he0)
				{
					return null;
				}
			}
			return halfEdge;
		}

		// Token: 0x06001B9E RID: 7070 RVA: 0x0009BDF8 File Offset: 0x0009A1F8
		public double distanceToPlane(Point3d p)
		{
			return this.normal.x * p.x + this.normal.y * p.y + this.normal.z * p.z - this.planeOffset;
		}

		// Token: 0x06001B9F RID: 7071 RVA: 0x0009BE44 File Offset: 0x0009A244
		public Vector3d getNormal()
		{
			return this.normal;
		}

		// Token: 0x06001BA0 RID: 7072 RVA: 0x0009BE4C File Offset: 0x0009A24C
		public Point3d getCentroid()
		{
			return this.centroid;
		}

		// Token: 0x06001BA1 RID: 7073 RVA: 0x0009BE54 File Offset: 0x0009A254
		public int numVertices()
		{
			return this.numVerts;
		}

		// Token: 0x06001BA2 RID: 7074 RVA: 0x0009BE5C File Offset: 0x0009A25C
		public string getVertexString()
		{
			string text = null;
			HalfEdge halfEdge = this.he0;
			do
			{
				if (text == null)
				{
					text = string.Empty + halfEdge.head().index;
				}
				else
				{
					text = text + " " + halfEdge.head().index;
				}
				halfEdge = halfEdge.next;
			}
			while (halfEdge != this.he0);
			return text;
		}

		// Token: 0x06001BA3 RID: 7075 RVA: 0x0009BEC8 File Offset: 0x0009A2C8
		public void getVertexIndices(int[] idxs)
		{
			HalfEdge halfEdge = this.he0;
			int num = 0;
			do
			{
				idxs[num++] = halfEdge.head().index;
				halfEdge = halfEdge.next;
			}
			while (halfEdge != this.he0);
		}

		// Token: 0x06001BA4 RID: 7076 RVA: 0x0009BF04 File Offset: 0x0009A304
		private Face connectHalfEdges(HalfEdge hedgePrev, HalfEdge hedge)
		{
			Face result = null;
			if (hedgePrev.oppositeFace() == hedge.oppositeFace())
			{
				Face face = hedge.oppositeFace();
				if (hedgePrev == this.he0)
				{
					this.he0 = hedge;
				}
				HalfEdge opposite;
				if (face.numVertices() == 3)
				{
					opposite = hedge.getOpposite().prev.getOpposite();
					face.mark = 3;
					result = face;
				}
				else
				{
					opposite = hedge.getOpposite().next;
					if (face.he0 == opposite.prev)
					{
						face.he0 = opposite;
					}
					opposite.prev = opposite.prev.prev;
					opposite.prev.next = opposite;
				}
				hedge.prev = hedgePrev.prev;
				hedge.prev.next = hedge;
				hedge.opposite = opposite;
				opposite.opposite = hedge;
				face.computeNormalAndCentroid();
			}
			else
			{
				hedgePrev.next = hedge;
				hedge.prev = hedgePrev;
			}
			return result;
		}

		// Token: 0x06001BA5 RID: 7077 RVA: 0x0009BFEC File Offset: 0x0009A3EC
		public void checkConsistency()
		{
			HalfEdge halfEdge = this.he0;
			double num = 0.0;
			int num2 = 0;
			if (this.numVerts < 3)
			{
				throw new InternalErrorException("degenerate face: " + this.getVertexString());
			}
			HalfEdge opposite;
			Face face;
			for (;;)
			{
				opposite = halfEdge.getOpposite();
				if (opposite == null)
				{
					break;
				}
				if (opposite.getOpposite() != halfEdge)
				{
					goto Block_3;
				}
				if (opposite.head() != halfEdge.tail() || halfEdge.head() != opposite.tail())
				{
					goto IL_DA;
				}
				face = opposite.face;
				if (face == null)
				{
					goto Block_5;
				}
				if (face.mark == 3)
				{
					goto Block_6;
				}
				double num3 = Math.Abs(this.distanceToPlane(halfEdge.head().pnt));
				if (num3 > num)
				{
					num = num3;
				}
				num2++;
				halfEdge = halfEdge.next;
				if (halfEdge == this.he0)
				{
					goto Block_8;
				}
			}
			throw new InternalErrorException("face " + this.getVertexString() + ": unreflected half edge " + halfEdge.getVertexString());
			Block_3:
			throw new InternalErrorException(string.Concat(new string[]
			{
				"face ",
				this.getVertexString(),
				": opposite half edge ",
				opposite.getVertexString(),
				" has opposite ",
				opposite.getOpposite().getVertexString()
			}));
			IL_DA:
			throw new InternalErrorException(string.Concat(new string[]
			{
				"face ",
				this.getVertexString(),
				": half edge ",
				halfEdge.getVertexString(),
				" reflected by ",
				opposite.getVertexString()
			}));
			Block_5:
			throw new InternalErrorException("face " + this.getVertexString() + ": no face on half edge " + opposite.getVertexString());
			Block_6:
			throw new InternalErrorException(string.Concat(new string[]
			{
				"face ",
				this.getVertexString(),
				": opposite face ",
				face.getVertexString(),
				" not on hull"
			}));
			Block_8:
			if (num2 != this.numVerts)
			{
				throw new InternalErrorException(string.Concat(new object[]
				{
					"face ",
					this.getVertexString(),
					" numVerts=",
					this.numVerts,
					" should be ",
					num2
				}));
			}
		}

		// Token: 0x06001BA6 RID: 7078 RVA: 0x0009C220 File Offset: 0x0009A620
		public int mergeAdjacentFace(HalfEdge hedgeAdj, Face[] discarded)
		{
			Face face = hedgeAdj.oppositeFace();
			int result = 0;
			discarded[result++] = face;
			face.mark = 3;
			HalfEdge opposite = hedgeAdj.getOpposite();
			HalfEdge prev = hedgeAdj.prev;
			HalfEdge halfEdge = hedgeAdj.next;
			HalfEdge prev2 = opposite.prev;
			HalfEdge halfEdge2 = opposite.next;
			while (prev.oppositeFace() == face)
			{
				prev = prev.prev;
				halfEdge2 = halfEdge2.next;
			}
			while (halfEdge.oppositeFace() == face)
			{
				prev2 = prev2.prev;
				halfEdge = halfEdge.next;
			}
			for (HalfEdge halfEdge3 = halfEdge2; halfEdge3 != prev2.next; halfEdge3 = halfEdge3.next)
			{
				halfEdge3.face = this;
			}
			if (hedgeAdj == this.he0)
			{
				this.he0 = halfEdge;
			}
			Face face2 = this.connectHalfEdges(prev2, halfEdge);
			if (face2 != null)
			{
				discarded[result++] = face2;
			}
			face2 = this.connectHalfEdges(prev, halfEdge2);
			if (face2 != null)
			{
				discarded[result++] = face2;
			}
			this.computeNormalAndCentroid();
			this.checkConsistency();
			return result;
		}

		// Token: 0x06001BA7 RID: 7079 RVA: 0x0009C330 File Offset: 0x0009A730
		private double areaSquared(HalfEdge hedge0, HalfEdge hedge1)
		{
			Point3d pnt = hedge0.tail().pnt;
			Point3d pnt2 = hedge0.head().pnt;
			Point3d pnt3 = hedge1.head().pnt;
			double num = pnt2.x - pnt.x;
			double num2 = pnt2.y - pnt.y;
			double num3 = pnt2.z - pnt.z;
			double num4 = pnt3.x - pnt.x;
			double num5 = pnt3.y - pnt.y;
			double num6 = pnt3.z - pnt.z;
			double num7 = num2 * num6 - num3 * num5;
			double num8 = num3 * num4 - num * num6;
			double num9 = num * num5 - num2 * num4;
			return num7 * num7 + num8 * num8 + num9 * num9;
		}

		// Token: 0x06001BA8 RID: 7080 RVA: 0x0009C3F0 File Offset: 0x0009A7F0
		public void triangulate(FaceList newFaces, double minArea)
		{
			if (this.numVertices() < 4)
			{
				return;
			}
			Vertex v = this.he0.head();
			HalfEdge halfEdge = this.he0.next;
			HalfEdge opposite = halfEdge.opposite;
			Face face = null;
			for (halfEdge = halfEdge.next; halfEdge != this.he0.prev; halfEdge = halfEdge.next)
			{
				Face face2 = Face.createTriangle(v, halfEdge.prev.head(), halfEdge.head(), minArea);
				face2.he0.next.setOpposite(opposite);
				face2.he0.prev.setOpposite(halfEdge.opposite);
				opposite = face2.he0;
				newFaces.add(face2);
				if (face == null)
				{
					face = face2;
				}
			}
			halfEdge = new HalfEdge(this.he0.prev.prev.head(), this);
			halfEdge.setOpposite(opposite);
			halfEdge.prev = this.he0;
			halfEdge.prev.next = halfEdge;
			halfEdge.next = this.he0.prev;
			halfEdge.next.prev = halfEdge;
			this.computeNormalAndCentroid(minArea);
			this.checkConsistency();
			for (Face face3 = face; face3 != null; face3 = face3.next)
			{
				face3.checkConsistency();
			}
		}

		// Token: 0x04001794 RID: 6036
		public HalfEdge he0;

		// Token: 0x04001795 RID: 6037
		private Vector3d normal;

		// Token: 0x04001796 RID: 6038
		public double area;

		// Token: 0x04001797 RID: 6039
		private Point3d centroid;

		// Token: 0x04001798 RID: 6040
		public double planeOffset;

		// Token: 0x04001799 RID: 6041
		public int index;

		// Token: 0x0400179A RID: 6042
		public int numVerts;

		// Token: 0x0400179B RID: 6043
		public Face next;

		// Token: 0x0400179C RID: 6044
		public const int VISIBLE = 1;

		// Token: 0x0400179D RID: 6045
		public const int NON_CONVEX = 2;

		// Token: 0x0400179E RID: 6046
		public const int DELETED = 3;

		// Token: 0x0400179F RID: 6047
		public int mark = 1;

		// Token: 0x040017A0 RID: 6048
		public Vertex outside;
	}
}
