using System;
using System.Collections.Generic;

namespace Technie.PhysicsCreator.QHull
{
	// Token: 0x0200045D RID: 1117
	public class QuickHull3D
	{
		// Token: 0x06001BC2 RID: 7106 RVA: 0x0009CC90 File Offset: 0x0009B090
		public QuickHull3D()
		{
		}

		// Token: 0x06001BC3 RID: 7107 RVA: 0x0009CD30 File Offset: 0x0009B130
		public QuickHull3D(double[] coords)
		{
			this.build(coords, coords.Length / 3);
		}

		// Token: 0x06001BC4 RID: 7108 RVA: 0x0009CDDC File Offset: 0x0009B1DC
		public QuickHull3D(Point3d[] points)
		{
			this.build(points, points.Length);
		}

		// Token: 0x06001BC5 RID: 7109 RVA: 0x0009CE86 File Offset: 0x0009B286
		public bool getDebug()
		{
			return this.debug;
		}

		// Token: 0x06001BC6 RID: 7110 RVA: 0x0009CE8E File Offset: 0x0009B28E
		public void setDebug(bool enable)
		{
			this.debug = enable;
		}

		// Token: 0x06001BC7 RID: 7111 RVA: 0x0009CE97 File Offset: 0x0009B297
		public double getDistanceTolerance()
		{
			return this.tolerance;
		}

		// Token: 0x06001BC8 RID: 7112 RVA: 0x0009CE9F File Offset: 0x0009B29F
		public void setExplicitDistanceTolerance(double tol)
		{
			this.explicitTolerance = tol;
		}

		// Token: 0x06001BC9 RID: 7113 RVA: 0x0009CEA8 File Offset: 0x0009B2A8
		public double getExplicitDistanceTolerance()
		{
			return this.explicitTolerance;
		}

		// Token: 0x06001BCA RID: 7114 RVA: 0x0009CEB0 File Offset: 0x0009B2B0
		private void addPointToFace(Vertex vtx, Face face)
		{
			vtx.face = face;
			if (face.outside == null)
			{
				this.claimed.add(vtx);
			}
			else
			{
				this.claimed.insertBefore(vtx, face.outside);
			}
			face.outside = vtx;
		}

		// Token: 0x06001BCB RID: 7115 RVA: 0x0009CEF0 File Offset: 0x0009B2F0
		private void removePointFromFace(Vertex vtx, Face face)
		{
			if (vtx == face.outside)
			{
				if (vtx.next != null && vtx.next.face == face)
				{
					face.outside = vtx.next;
				}
				else
				{
					face.outside = null;
				}
			}
			this.claimed.delete(vtx);
		}

		// Token: 0x06001BCC RID: 7116 RVA: 0x0009CF4C File Offset: 0x0009B34C
		private Vertex removeAllPointsFromFace(Face face)
		{
			if (face.outside != null)
			{
				Vertex vertex = face.outside;
				while (vertex.next != null && vertex.next.face == face)
				{
					vertex = vertex.next;
				}
				this.claimed.delete(face.outside, vertex);
				vertex.next = null;
				return face.outside;
			}
			return null;
		}

		// Token: 0x06001BCD RID: 7117 RVA: 0x0009CFB4 File Offset: 0x0009B3B4
		private HalfEdge findHalfEdge(Vertex tail, Vertex head)
		{
			foreach (Face face in this.faces)
			{
				HalfEdge halfEdge = face.findEdge(tail, head);
				if (halfEdge != null)
				{
					return halfEdge;
				}
			}
			return null;
		}

		// Token: 0x06001BCE RID: 7118 RVA: 0x0009D024 File Offset: 0x0009B424
		protected void setHull(double[] coords, int nump, int[][] faceIndices, int numf)
		{
			this.initBuffers(nump);
			this.setPoints(coords, nump);
			this.computeMaxAndMin();
			for (int i = 0; i < numf; i++)
			{
				Face face = Face.create(this.pointBuffer, faceIndices[i]);
				HalfEdge halfEdge = face.he0;
				do
				{
					HalfEdge halfEdge2 = this.findHalfEdge(halfEdge.head(), halfEdge.tail());
					if (halfEdge2 != null)
					{
						halfEdge.setOpposite(halfEdge2);
					}
					halfEdge = halfEdge.next;
				}
				while (halfEdge != face.he0);
				this.faces.Add(face);
			}
		}

		// Token: 0x06001BCF RID: 7119 RVA: 0x0009D0AE File Offset: 0x0009B4AE
		public void build(double[] coords)
		{
			this.build(coords, coords.Length / 3);
		}

		// Token: 0x06001BD0 RID: 7120 RVA: 0x0009D0BC File Offset: 0x0009B4BC
		public void build(double[] coords, int nump)
		{
			if (nump < 4)
			{
				throw new SystemException("Less than four input points specified");
			}
			if (coords.Length / 3 < nump)
			{
				throw new SystemException("Coordinate array too small for specified number of points");
			}
			this.initBuffers(nump);
			this.setPoints(coords, nump);
			this.buildHull();
		}

		// Token: 0x06001BD1 RID: 7121 RVA: 0x0009D0FB File Offset: 0x0009B4FB
		public void build(Point3d[] points)
		{
			this.build(points, points.Length);
		}

		// Token: 0x06001BD2 RID: 7122 RVA: 0x0009D107 File Offset: 0x0009B507
		public void build(Point3d[] points, int nump)
		{
			if (nump < 4)
			{
				throw new SystemException("Less than four input points specified");
			}
			if (points.Length < nump)
			{
				throw new SystemException("Point array too small for specified number of points");
			}
			this.initBuffers(nump);
			this.setPoints(points, nump);
			this.buildHull();
		}

		// Token: 0x06001BD3 RID: 7123 RVA: 0x0009D144 File Offset: 0x0009B544
		public void triangulate()
		{
			double minArea = 1000.0 * this.charLength * 2.220446049250313E-16;
			this.newFaces.clear();
			foreach (Face face in this.faces)
			{
				if (face.mark == 1)
				{
					face.triangulate(this.newFaces, minArea);
				}
			}
			for (Face face2 = this.newFaces.first(); face2 != null; face2 = face2.next)
			{
				this.faces.Add(face2);
			}
		}

		// Token: 0x06001BD4 RID: 7124 RVA: 0x0009D204 File Offset: 0x0009B604
		protected void initBuffers(int nump)
		{
			if (this.pointBuffer.Length < nump)
			{
				Vertex[] array = new Vertex[nump];
				this.vertexPointIndices = new int[nump];
				for (int i = 0; i < this.pointBuffer.Length; i++)
				{
					array[i] = this.pointBuffer[i];
				}
				for (int j = this.pointBuffer.Length; j < nump; j++)
				{
					array[j] = new Vertex();
				}
				this.pointBuffer = array;
			}
			this.faces.Clear();
			this.claimed.clear();
			this.numFaces = 0;
			this.numPoints = nump;
		}

		// Token: 0x06001BD5 RID: 7125 RVA: 0x0009D2A4 File Offset: 0x0009B6A4
		protected void setPoints(double[] coords, int nump)
		{
			for (int i = 0; i < nump; i++)
			{
				Vertex vertex = this.pointBuffer[i];
				vertex.pnt.set(coords[i * 3], coords[i * 3 + 1], coords[i * 3 + 2]);
				vertex.index = i;
			}
		}

		// Token: 0x06001BD6 RID: 7126 RVA: 0x0009D2F4 File Offset: 0x0009B6F4
		protected void setPoints(Point3d[] pnts, int nump)
		{
			for (int i = 0; i < nump; i++)
			{
				Vertex vertex = this.pointBuffer[i];
				vertex.pnt.set(pnts[i]);
				vertex.index = i;
			}
		}

		// Token: 0x06001BD7 RID: 7127 RVA: 0x0009D334 File Offset: 0x0009B734
		protected void computeMaxAndMin()
		{
			Vector3d vector3d = new Vector3d();
			Vector3d vector3d2 = new Vector3d();
			for (int i = 0; i < 3; i++)
			{
				this.maxVtxs[i] = (this.minVtxs[i] = this.pointBuffer[0]);
			}
			vector3d.set(this.pointBuffer[0].pnt);
			vector3d2.set(this.pointBuffer[0].pnt);
			for (int j = 1; j < this.numPoints; j++)
			{
				Point3d pnt = this.pointBuffer[j].pnt;
				if (pnt.x > vector3d.x)
				{
					vector3d.x = pnt.x;
					this.maxVtxs[0] = this.pointBuffer[j];
				}
				else if (pnt.x < vector3d2.x)
				{
					vector3d2.x = pnt.x;
					this.minVtxs[0] = this.pointBuffer[j];
				}
				if (pnt.y > vector3d.y)
				{
					vector3d.y = pnt.y;
					this.maxVtxs[1] = this.pointBuffer[j];
				}
				else if (pnt.y < vector3d2.y)
				{
					vector3d2.y = pnt.y;
					this.minVtxs[1] = this.pointBuffer[j];
				}
				if (pnt.z > vector3d.z)
				{
					vector3d.z = pnt.z;
					this.maxVtxs[2] = this.pointBuffer[j];
				}
				else if (pnt.z < vector3d2.z)
				{
					vector3d2.z = pnt.z;
					this.minVtxs[2] = this.pointBuffer[j];
				}
			}
			this.charLength = Math.Max(vector3d.x - vector3d2.x, vector3d.y - vector3d2.y);
			this.charLength = Math.Max(vector3d.z - vector3d2.z, this.charLength);
			if (this.explicitTolerance == -1.0)
			{
				this.tolerance = 6.661338147750939E-16 * (Math.Max(Math.Abs(vector3d.x), Math.Abs(vector3d2.x)) + Math.Max(Math.Abs(vector3d.y), Math.Abs(vector3d2.y)) + Math.Max(Math.Abs(vector3d.z), Math.Abs(vector3d2.z)));
			}
			else
			{
				this.tolerance = this.explicitTolerance;
			}
		}

		// Token: 0x06001BD8 RID: 7128 RVA: 0x0009D5C8 File Offset: 0x0009B9C8
		protected void createInitialSimplex()
		{
			double num = 0.0;
			int num2 = 0;
			for (int i = 0; i < 3; i++)
			{
				double num3 = this.maxVtxs[i].pnt.get(i) - this.minVtxs[i].pnt.get(i);
				if (num3 > num)
				{
					num = num3;
					num2 = i;
				}
			}
			if (num <= this.tolerance)
			{
				throw new SystemException("Input points appear to be coincident");
			}
			Vertex[] array = new Vertex[4];
			array[0] = this.maxVtxs[num2];
			array[1] = this.minVtxs[num2];
			Vector3d vector3d = new Vector3d();
			Vector3d vector3d2 = new Vector3d();
			Vector3d vector3d3 = new Vector3d();
			Vector3d vector3d4 = new Vector3d();
			double num4 = 0.0;
			vector3d.sub(array[1].pnt, array[0].pnt);
			vector3d.normalize();
			for (int j = 0; j < this.numPoints; j++)
			{
				vector3d2.sub(this.pointBuffer[j].pnt, array[0].pnt);
				vector3d4.cross(vector3d, vector3d2);
				double num5 = vector3d4.normSquared();
				if (num5 > num4 && this.pointBuffer[j] != array[0] && this.pointBuffer[j] != array[1])
				{
					num4 = num5;
					array[2] = this.pointBuffer[j];
					vector3d3.set(vector3d4);
				}
			}
			if (Math.Sqrt(num4) <= 100.0 * this.tolerance)
			{
				throw new SystemException("Input points appear to be colinear");
			}
			vector3d3.normalize();
			double num6 = 0.0;
			double num7 = array[2].pnt.dot(vector3d3);
			for (int k = 0; k < this.numPoints; k++)
			{
				double num8 = Math.Abs(this.pointBuffer[k].pnt.dot(vector3d3) - num7);
				if (num8 > num6 && this.pointBuffer[k] != array[0] && this.pointBuffer[k] != array[1] && this.pointBuffer[k] != array[2])
				{
					num6 = num8;
					array[3] = this.pointBuffer[k];
				}
			}
			if (Math.Abs(num6) <= 100.0 * this.tolerance)
			{
				throw new SystemException("Input points appear to be coplanar");
			}
			Face[] array2 = new Face[4];
			if (array[3].pnt.dot(vector3d3) - num7 < 0.0)
			{
				array2[0] = Face.createTriangle(array[0], array[1], array[2]);
				array2[1] = Face.createTriangle(array[3], array[1], array[0]);
				array2[2] = Face.createTriangle(array[3], array[2], array[1]);
				array2[3] = Face.createTriangle(array[3], array[0], array[2]);
				for (int l = 0; l < 3; l++)
				{
					int num9 = (l + 1) % 3;
					array2[l + 1].getEdge(1).setOpposite(array2[num9 + 1].getEdge(0));
					array2[l + 1].getEdge(2).setOpposite(array2[0].getEdge(num9));
				}
			}
			else
			{
				array2[0] = Face.createTriangle(array[0], array[2], array[1]);
				array2[1] = Face.createTriangle(array[3], array[0], array[1]);
				array2[2] = Face.createTriangle(array[3], array[1], array[2]);
				array2[3] = Face.createTriangle(array[3], array[2], array[0]);
				for (int m = 0; m < 3; m++)
				{
					int num10 = (m + 1) % 3;
					array2[m + 1].getEdge(0).setOpposite(array2[num10 + 1].getEdge(1));
					array2[m + 1].getEdge(2).setOpposite(array2[0].getEdge((3 - m) % 3));
				}
			}
			for (int n = 0; n < 4; n++)
			{
				this.faces.Add(array2[n]);
			}
			for (int num11 = 0; num11 < this.numPoints; num11++)
			{
				Vertex vertex = this.pointBuffer[num11];
				if (vertex != array[0] && vertex != array[1] && vertex != array[2] && vertex != array[3])
				{
					num6 = this.tolerance;
					Face face = null;
					for (int num12 = 0; num12 < 4; num12++)
					{
						double num13 = array2[num12].distanceToPlane(vertex.pnt);
						if (num13 > num6)
						{
							face = array2[num12];
							num6 = num13;
						}
					}
					if (face != null)
					{
						this.addPointToFace(vertex, face);
					}
				}
			}
		}

		// Token: 0x06001BD9 RID: 7129 RVA: 0x0009DA98 File Offset: 0x0009BE98
		public int getNumVertices()
		{
			return this.numVertices;
		}

		// Token: 0x06001BDA RID: 7130 RVA: 0x0009DAA0 File Offset: 0x0009BEA0
		public Point3d[] getVertices()
		{
			Point3d[] array = new Point3d[this.numVertices];
			for (int i = 0; i < this.numVertices; i++)
			{
				array[i] = this.pointBuffer[this.vertexPointIndices[i]].pnt;
			}
			return array;
		}

		// Token: 0x06001BDB RID: 7131 RVA: 0x0009DAE8 File Offset: 0x0009BEE8
		public int getVertices(double[] coords)
		{
			for (int i = 0; i < this.numVertices; i++)
			{
				Point3d pnt = this.pointBuffer[this.vertexPointIndices[i]].pnt;
				coords[i * 3] = pnt.x;
				coords[i * 3 + 1] = pnt.y;
				coords[i * 3 + 2] = pnt.z;
			}
			return this.numVertices;
		}

		// Token: 0x06001BDC RID: 7132 RVA: 0x0009DB4C File Offset: 0x0009BF4C
		public int[] getVertexPointIndices()
		{
			int[] array = new int[this.numVertices];
			for (int i = 0; i < this.numVertices; i++)
			{
				array[i] = this.vertexPointIndices[i];
			}
			return array;
		}

		// Token: 0x06001BDD RID: 7133 RVA: 0x0009DB88 File Offset: 0x0009BF88
		public int getNumFaces()
		{
			return this.faces.Count;
		}

		// Token: 0x06001BDE RID: 7134 RVA: 0x0009DB95 File Offset: 0x0009BF95
		public int[][] getFaces()
		{
			return this.getFaces(0);
		}

		// Token: 0x06001BDF RID: 7135 RVA: 0x0009DBA0 File Offset: 0x0009BFA0
		public int[][] getFaces(int indexFlags)
		{
			int[][] array = new int[this.faces.Count][];
			int num = 0;
			foreach (Face face in this.faces)
			{
				array[num] = new int[face.numVertices()];
				this.getFaceIndices(array[num], face, indexFlags);
				num++;
			}
			return array;
		}

		// Token: 0x06001BE0 RID: 7136 RVA: 0x0009DC28 File Offset: 0x0009C028
		private void getFaceIndices(int[] indices, Face face, int flags)
		{
			bool flag = (flags & 1) == 0;
			bool flag2 = (flags & 2) != 0;
			bool flag3 = (flags & 8) != 0;
			HalfEdge halfEdge = face.he0;
			int num = 0;
			do
			{
				int num2 = halfEdge.head().index;
				if (flag3)
				{
					num2 = this.vertexPointIndices[num2];
				}
				if (flag2)
				{
					num2++;
				}
				indices[num++] = num2;
				halfEdge = ((!flag) ? halfEdge.prev : halfEdge.next);
			}
			while (halfEdge != face.he0);
		}

		// Token: 0x06001BE1 RID: 7137 RVA: 0x0009DCB4 File Offset: 0x0009C0B4
		protected void resolveUnclaimedPoints(FaceList newFaces)
		{
			Vertex vertex = this.unclaimed.first();
			for (Vertex vertex2 = vertex; vertex2 != null; vertex2 = vertex)
			{
				vertex = vertex2.next;
				double num = this.tolerance;
				Face face = null;
				for (Face face2 = newFaces.first(); face2 != null; face2 = face2.next)
				{
					if (face2.mark == 1)
					{
						double num2 = face2.distanceToPlane(vertex2.pnt);
						if (num2 > num)
						{
							num = num2;
							face = face2;
						}
						if (num > 1000.0 * this.tolerance)
						{
							break;
						}
					}
				}
				if (face != null)
				{
					this.addPointToFace(vertex2, face);
					if (!this.debug || vertex2.index == this.findIndex)
					{
					}
				}
				else if (!this.debug || vertex2.index == this.findIndex)
				{
				}
			}
		}

		// Token: 0x06001BE2 RID: 7138 RVA: 0x0009DD9C File Offset: 0x0009C19C
		protected void deleteFacePoints(Face face, Face absorbingFace)
		{
			Vertex vertex = this.removeAllPointsFromFace(face);
			if (vertex != null)
			{
				if (absorbingFace == null)
				{
					this.unclaimed.addAll(vertex);
				}
				else
				{
					Vertex vertex2 = vertex;
					for (Vertex vertex3 = vertex2; vertex3 != null; vertex3 = vertex2)
					{
						vertex2 = vertex3.next;
						double num = absorbingFace.distanceToPlane(vertex3.pnt);
						if (num > this.tolerance)
						{
							this.addPointToFace(vertex3, absorbingFace);
						}
						else
						{
							this.unclaimed.add(vertex3);
						}
					}
				}
			}
		}

		// Token: 0x06001BE3 RID: 7139 RVA: 0x0009DE18 File Offset: 0x0009C218
		protected double oppFaceDistance(HalfEdge he)
		{
			return he.face.distanceToPlane(he.opposite.face.getCentroid());
		}

		// Token: 0x06001BE4 RID: 7140 RVA: 0x0009DE38 File Offset: 0x0009C238
		private bool doAdjacentMerge(Face face, int mergeType)
		{
			HalfEdge halfEdge = face.he0;
			bool flag = true;
			for (;;)
			{
				Face face2 = halfEdge.oppositeFace();
				bool flag2 = false;
				if (mergeType == 2)
				{
					if (this.oppFaceDistance(halfEdge) > -this.tolerance || this.oppFaceDistance(halfEdge.opposite) > -this.tolerance)
					{
						flag2 = true;
					}
				}
				else if (face.area > face2.area)
				{
					double num = this.oppFaceDistance(halfEdge);
					if (num > -this.tolerance)
					{
						flag2 = true;
					}
					else if (this.oppFaceDistance(halfEdge.opposite) > -this.tolerance)
					{
						flag = false;
					}
				}
				else if (this.oppFaceDistance(halfEdge.opposite) > -this.tolerance)
				{
					flag2 = true;
				}
				else if (this.oppFaceDistance(halfEdge) > -this.tolerance)
				{
					flag = false;
				}
				if (flag2)
				{
					break;
				}
				halfEdge = halfEdge.next;
				if (halfEdge == face.he0)
				{
					goto Block_10;
				}
			}
			if (this.debug)
			{
			}
			int num2 = face.mergeAdjacentFace(halfEdge, this.discardedFaces);
			for (int i = 0; i < num2; i++)
			{
				this.deleteFacePoints(this.discardedFaces[i], face);
			}
			if (this.debug)
			{
			}
			return true;
			Block_10:
			if (!flag)
			{
				face.mark = 2;
			}
			return false;
		}

		// Token: 0x06001BE5 RID: 7141 RVA: 0x0009DF88 File Offset: 0x0009C388
		protected void calculateHorizon(Point3d eyePnt, HalfEdge edge0, Face face, List<HalfEdge> horizon)
		{
			this.deleteFacePoints(face, null);
			face.mark = 3;
			if (this.debug)
			{
			}
			HalfEdge halfEdge;
			if (edge0 == null)
			{
				edge0 = face.getEdge(0);
				halfEdge = edge0;
			}
			else
			{
				halfEdge = edge0.getNext();
			}
			do
			{
				Face face2 = halfEdge.oppositeFace();
				if (face2.mark == 1)
				{
					if (face2.distanceToPlane(eyePnt) > this.tolerance)
					{
						this.calculateHorizon(eyePnt, halfEdge.getOpposite(), face2, horizon);
					}
					else
					{
						horizon.Add(halfEdge);
						if (this.debug)
						{
						}
					}
				}
				halfEdge = halfEdge.getNext();
			}
			while (halfEdge != edge0);
		}

		// Token: 0x06001BE6 RID: 7142 RVA: 0x0009E028 File Offset: 0x0009C428
		private HalfEdge addAdjoiningFace(Vertex eyeVtx, HalfEdge he)
		{
			Face face = Face.createTriangle(eyeVtx, he.tail(), he.head());
			this.faces.Add(face);
			face.getEdge(-1).setOpposite(he.getOpposite());
			return face.getEdge(0);
		}

		// Token: 0x06001BE7 RID: 7143 RVA: 0x0009E070 File Offset: 0x0009C470
		protected void addNewFaces(FaceList newFaces, Vertex eyeVtx, List<HalfEdge> horizon)
		{
			newFaces.clear();
			HalfEdge halfEdge = null;
			HalfEdge halfEdge2 = null;
			foreach (HalfEdge he in horizon)
			{
				HalfEdge halfEdge3 = this.addAdjoiningFace(eyeVtx, he);
				if (this.debug)
				{
				}
				if (halfEdge != null)
				{
					halfEdge3.next.setOpposite(halfEdge);
				}
				else
				{
					halfEdge2 = halfEdge3;
				}
				newFaces.add(halfEdge3.getFace());
				halfEdge = halfEdge3;
			}
			halfEdge2.next.setOpposite(halfEdge);
		}

		// Token: 0x06001BE8 RID: 7144 RVA: 0x0009E118 File Offset: 0x0009C518
		protected Vertex nextPointToAdd()
		{
			if (!this.claimed.isEmpty())
			{
				Face face = this.claimed.first().face;
				Vertex result = null;
				double num = 0.0;
				Vertex vertex = face.outside;
				while (vertex != null && vertex.face == face)
				{
					double num2 = face.distanceToPlane(vertex.pnt);
					if (num2 > num)
					{
						num = num2;
						result = vertex;
					}
					vertex = vertex.next;
				}
				return result;
			}
			return null;
		}

		// Token: 0x06001BE9 RID: 7145 RVA: 0x0009E198 File Offset: 0x0009C598
		protected void addPointToHull(Vertex eyeVtx)
		{
			this.horizon.Clear();
			this.unclaimed.clear();
			if (this.debug)
			{
			}
			this.removePointFromFace(eyeVtx, eyeVtx.face);
			this.calculateHorizon(eyeVtx.pnt, null, eyeVtx.face, this.horizon);
			this.newFaces.clear();
			this.addNewFaces(this.newFaces, eyeVtx, this.horizon);
			for (Face face = this.newFaces.first(); face != null; face = face.next)
			{
				if (face.mark == 1)
				{
					while (this.doAdjacentMerge(face, 1))
					{
					}
				}
			}
			for (Face face2 = this.newFaces.first(); face2 != null; face2 = face2.next)
			{
				if (face2.mark == 2)
				{
					face2.mark = 1;
					while (this.doAdjacentMerge(face2, 2))
					{
					}
				}
			}
			this.resolveUnclaimedPoints(this.newFaces);
		}

		// Token: 0x06001BEA RID: 7146 RVA: 0x0009E298 File Offset: 0x0009C698
		protected void buildHull()
		{
			int num = 0;
			this.computeMaxAndMin();
			this.createInitialSimplex();
			Vertex eyeVtx;
			while ((eyeVtx = this.nextPointToAdd()) != null)
			{
				this.addPointToHull(eyeVtx);
				num++;
				if (this.debug)
				{
				}
			}
			this.reindexFacesAndVertices();
			if (this.debug)
			{
			}
		}

		// Token: 0x06001BEB RID: 7147 RVA: 0x0009E2EC File Offset: 0x0009C6EC
		private void markFaceVertices(Face face, int mark)
		{
			HalfEdge firstEdge = face.getFirstEdge();
			HalfEdge halfEdge = firstEdge;
			do
			{
				halfEdge.head().index = mark;
				halfEdge = halfEdge.next;
			}
			while (halfEdge != firstEdge);
		}

		// Token: 0x06001BEC RID: 7148 RVA: 0x0009E31C File Offset: 0x0009C71C
		protected void reindexFacesAndVertices()
		{
			for (int i = 0; i < this.numPoints; i++)
			{
				this.pointBuffer[i].index = -1;
			}
			this.numFaces = 0;
			for (int j = 0; j < this.faces.Count; j++)
			{
				Face face = this.faces[j];
				if (face.mark != 1)
				{
					this.faces.RemoveAt(j);
					j--;
				}
				else
				{
					this.markFaceVertices(face, 0);
					this.numFaces++;
				}
			}
			this.numVertices = 0;
			for (int k = 0; k < this.numPoints; k++)
			{
				Vertex vertex = this.pointBuffer[k];
				if (vertex.index == 0)
				{
					this.vertexPointIndices[this.numVertices] = k;
					vertex.index = this.numVertices++;
				}
			}
		}

		// Token: 0x06001BED RID: 7149 RVA: 0x0009E414 File Offset: 0x0009C814
		protected bool checkFaceConvexity(Face face, double tol)
		{
			HalfEdge halfEdge = face.he0;
			for (;;)
			{
				face.checkConsistency();
				double num = this.oppFaceDistance(halfEdge);
				if (num > tol)
				{
					break;
				}
				num = this.oppFaceDistance(halfEdge.opposite);
				if (num > tol)
				{
					return false;
				}
				if (halfEdge.next.oppositeFace() == halfEdge.oppositeFace())
				{
					return false;
				}
				halfEdge = halfEdge.next;
				if (halfEdge == face.he0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001BEE RID: 7150 RVA: 0x0009E484 File Offset: 0x0009C884
		protected bool checkFaces(double tol)
		{
			bool result = true;
			foreach (Face face in this.faces)
			{
				if (face.mark == 1 && !this.checkFaceConvexity(face, tol))
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06001BEF RID: 7151 RVA: 0x0009E4F8 File Offset: 0x0009C8F8
		public bool check()
		{
			return this.check(this.getDistanceTolerance());
		}

		// Token: 0x06001BF0 RID: 7152 RVA: 0x0009E508 File Offset: 0x0009C908
		public bool check(double tol)
		{
			double num = 10.0 * tol;
			if (!this.checkFaces(this.tolerance))
			{
				return false;
			}
			for (int i = 0; i < this.numPoints; i++)
			{
				Point3d pnt = this.pointBuffer[i].pnt;
				foreach (Face face in this.faces)
				{
					if (face.mark == 1)
					{
						double num2 = face.distanceToPlane(pnt);
						if (num2 > num)
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x040017A8 RID: 6056
		public const int CLOCKWISE = 1;

		// Token: 0x040017A9 RID: 6057
		public const int INDEXED_FROM_ONE = 2;

		// Token: 0x040017AA RID: 6058
		public const int INDEXED_FROM_ZERO = 4;

		// Token: 0x040017AB RID: 6059
		public const int POINT_RELATIVE = 8;

		// Token: 0x040017AC RID: 6060
		public const double AUTOMATIC_TOLERANCE = -1.0;

		// Token: 0x040017AD RID: 6061
		protected int findIndex = -1;

		// Token: 0x040017AE RID: 6062
		protected double charLength;

		// Token: 0x040017AF RID: 6063
		protected bool debug;

		// Token: 0x040017B0 RID: 6064
		protected Vertex[] pointBuffer = new Vertex[0];

		// Token: 0x040017B1 RID: 6065
		protected int[] vertexPointIndices = new int[0];

		// Token: 0x040017B2 RID: 6066
		private Face[] discardedFaces = new Face[3];

		// Token: 0x040017B3 RID: 6067
		private Vertex[] maxVtxs = new Vertex[3];

		// Token: 0x040017B4 RID: 6068
		private Vertex[] minVtxs = new Vertex[3];

		// Token: 0x040017B5 RID: 6069
		protected List<Face> faces = new List<Face>(16);

		// Token: 0x040017B6 RID: 6070
		protected List<HalfEdge> horizon = new List<HalfEdge>(16);

		// Token: 0x040017B7 RID: 6071
		private FaceList newFaces = new FaceList();

		// Token: 0x040017B8 RID: 6072
		private VertexList unclaimed = new VertexList();

		// Token: 0x040017B9 RID: 6073
		private VertexList claimed = new VertexList();

		// Token: 0x040017BA RID: 6074
		protected int numVertices;

		// Token: 0x040017BB RID: 6075
		protected int numFaces;

		// Token: 0x040017BC RID: 6076
		protected int numPoints;

		// Token: 0x040017BD RID: 6077
		protected double explicitTolerance = -1.0;

		// Token: 0x040017BE RID: 6078
		protected double tolerance;

		// Token: 0x040017BF RID: 6079
		private const double DOUBLE_PREC = 2.220446049250313E-16;

		// Token: 0x040017C0 RID: 6080
		private const int NONCONVEX_WRT_LARGER_FACE = 1;

		// Token: 0x040017C1 RID: 6081
		private const int NONCONVEX = 2;
	}
}
