using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

// Token: 0x0200035F RID: 863
[ExecuteInEditMode]
public class NonConvexMeshCollider : MonoBehaviour
{
	// Token: 0x06001557 RID: 5463 RVA: 0x00079474 File Offset: 0x00077874
	public NonConvexMeshCollider()
	{
	}

	// Token: 0x06001558 RID: 5464 RVA: 0x0007949C File Offset: 0x0007789C
	public void Calculate()
	{
		Stopwatch stopwatch = Stopwatch.StartNew();
		if (this.boxesPerEdge > 100)
		{
			this.boxesPerEdge = 100;
		}
		if (this.avoidExceedingMesh && this.boxesPerEdge > 50)
		{
			this.boxesPerEdge = 50;
		}
		if (this.boxesPerEdge < 1)
		{
			this.boxesPerEdge = 3;
		}
		GameObject gameObject = base.gameObject;
		MeshFilter component = gameObject.GetComponent<MeshFilter>();
		if (component == null)
		{
			return;
		}
		if (component.sharedMesh == null)
		{
			return;
		}
		Rigidbody component2 = gameObject.GetComponent<Rigidbody>();
		bool flag = false;
		if (component2 != null && !component2.isKinematic)
		{
			flag = true;
			component2.isKinematic = true;
		}
		bool flag2 = false;
		if (component2 != null && component2.useGravity)
		{
			flag2 = true;
			component2.useGravity = false;
		}
		if (!this.createChildGameObject)
		{
			foreach (BoxCollider obj in gameObject.GetComponents<BoxCollider>())
			{
				UnityEngine.Object.DestroyImmediate(obj);
			}
		}
		int layer = gameObject.layer;
		int firstEmptyLayer = this.GetFirstEmptyLayer();
		gameObject.layer = firstEmptyLayer;
		Transform parent = gameObject.transform.parent;
		Vector3 localPosition = gameObject.transform.localPosition;
		Quaternion localRotation = gameObject.transform.localRotation;
		Vector3 localScale = gameObject.transform.localScale;
		GameObject gameObject2 = new GameObject("Temp_CompoundColliderParent");
		gameObject.transform.parent = gameObject2.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
		gameObject.transform.localScale = Vector3.one;
		try
		{
			GameObject gameObject3 = NonConvexMeshCollider.CreateColliderChildGameObject(gameObject, component);
			NonConvexMeshCollider.Box[] array = this.CreateMeshIntersectingBoxes(gameObject3).ToArray<NonConvexMeshCollider.Box>();
			NonConvexMeshCollider.Box[] array2 = (!this.mergeBoxesToReduceNumber) ? array : this.MergeBoxes(array.ToArray<NonConvexMeshCollider.Box>());
			foreach (NonConvexMeshCollider.Box box in array2)
			{
				BoxCollider boxCollider = ((!this.createChildGameObject) ? gameObject : gameObject3).AddComponent<BoxCollider>();
				boxCollider.size = box.Size;
				boxCollider.center = box.Center;
				if (this.physicsMaterialForColliders != null)
				{
					boxCollider.material = this.physicsMaterialForColliders;
				}
			}
			UnityEngine.Debug.Log("NonConvexMeshCollider: " + array2.Length + " box colliders created");
			UnityEngine.Object.DestroyImmediate(gameObject3.GetComponent<MeshFilter>());
			UnityEngine.Object.DestroyImmediate(gameObject3.GetComponent<MeshCollider>());
			UnityEngine.Object.DestroyImmediate(gameObject3.GetComponent<Rigidbody>());
			if (!this.createChildGameObject)
			{
				UnityEngine.Object.DestroyImmediate(gameObject3);
			}
			else if (gameObject3)
			{
				gameObject3.layer = layer;
			}
		}
		finally
		{
			gameObject.transform.parent = parent;
			gameObject.transform.localPosition = localPosition;
			gameObject.transform.localRotation = localRotation;
			gameObject.transform.localScale = localScale;
			gameObject.layer = layer;
			if (flag)
			{
				component2.isKinematic = false;
			}
			if (flag2)
			{
				component2.useGravity = true;
			}
			UnityEngine.Object.DestroyImmediate(gameObject2);
		}
		stopwatch.Stop();
		if (this.outputTimeMeasurements)
		{
			UnityEngine.Debug.Log("Total duration: " + stopwatch.Elapsed);
		}
	}

	// Token: 0x06001559 RID: 5465 RVA: 0x00079818 File Offset: 0x00077C18
	private NonConvexMeshCollider.Box[] MergeBoxes(NonConvexMeshCollider.Box[] boxes)
	{
		Stopwatch stopwatch = Stopwatch.StartNew();
		NonConvexMeshCollider.Vector3Int[] array = new NonConvexMeshCollider.Vector3Int[]
		{
			new NonConvexMeshCollider.Vector3Int(1, 0, 0),
			new NonConvexMeshCollider.Vector3Int(0, 1, 0),
			new NonConvexMeshCollider.Vector3Int(0, 0, 1),
			new NonConvexMeshCollider.Vector3Int(-1, 0, 0),
			new NonConvexMeshCollider.Vector3Int(0, -1, 0),
			new NonConvexMeshCollider.Vector3Int(0, 0, -1)
		};
		bool flag = false;
		do
		{
			foreach (NonConvexMeshCollider.Vector3Int direction in array)
			{
				flag = false;
				foreach (NonConvexMeshCollider.Box box in boxes)
				{
					bool flag2 = box.TryMerge(direction);
					if (flag2)
					{
						flag = true;
					}
				}
				IEnumerable<NonConvexMeshCollider.Box> source = boxes;
				if (NonConvexMeshCollider.<>f__am$cache0 == null)
				{
					NonConvexMeshCollider.<>f__am$cache0 = new Func<NonConvexMeshCollider.Box, NonConvexMeshCollider.Box>(NonConvexMeshCollider.<MergeBoxes>m__0);
				}
				boxes = source.Select(NonConvexMeshCollider.<>f__am$cache0).Distinct<NonConvexMeshCollider.Box>().ToArray<NonConvexMeshCollider.Box>();
			}
		}
		while (flag);
		IEnumerable<NonConvexMeshCollider.Box> source2 = boxes;
		if (NonConvexMeshCollider.<>f__am$cache1 == null)
		{
			NonConvexMeshCollider.<>f__am$cache1 = new Func<NonConvexMeshCollider.Box, NonConvexMeshCollider.Box>(NonConvexMeshCollider.<MergeBoxes>m__1);
		}
		NonConvexMeshCollider.Box[] result = source2.Select(NonConvexMeshCollider.<>f__am$cache1).Distinct<NonConvexMeshCollider.Box>().ToArray<NonConvexMeshCollider.Box>();
		stopwatch.Stop();
		if (this.outputTimeMeasurements)
		{
			UnityEngine.Debug.Log("Merged in " + stopwatch.Elapsed);
		}
		return result;
	}

	// Token: 0x0600155A RID: 5466 RVA: 0x00079964 File Offset: 0x00077D64
	private static GameObject CreateColliderChildGameObject(GameObject go, MeshFilter meshFilter)
	{
		Transform transform = go.transform.Find("Colliders");
		GameObject gameObject;
		if (transform != null)
		{
			gameObject = transform.gameObject;
		}
		else
		{
			gameObject = new GameObject("Colliders");
			gameObject.transform.parent = go.transform;
			gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
			gameObject.transform.localPosition = Vector3.zero;
		}
		gameObject.layer = go.layer;
		foreach (BoxCollider obj in gameObject.GetComponents<BoxCollider>())
		{
			UnityEngine.Object.DestroyImmediate(obj);
		}
		MeshFilter meshFilter2 = gameObject.GetComponent<MeshFilter>();
		if (meshFilter2 != null)
		{
			UnityEngine.Object.DestroyImmediate(meshFilter2);
		}
		MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();
		if (meshCollider != null)
		{
			UnityEngine.Object.DestroyImmediate(meshCollider);
		}
		Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
		if (rigidbody != null)
		{
			UnityEngine.Object.DestroyImmediate(rigidbody);
		}
		rigidbody = gameObject.AddComponent<Rigidbody>();
		rigidbody.isKinematic = true;
		rigidbody.useGravity = false;
		meshFilter2 = gameObject.AddComponent<MeshFilter>();
		meshFilter2.sharedMesh = meshFilter.sharedMesh;
		meshCollider = gameObject.AddComponent<MeshCollider>();
		meshCollider.convex = false;
		return gameObject;
	}

	// Token: 0x0600155B RID: 5467 RVA: 0x00079AA4 File Offset: 0x00077EA4
	private IEnumerable<NonConvexMeshCollider.Box> CreateMeshIntersectingBoxes(GameObject colliderGo)
	{
		NonConvexMeshCollider.<CreateMeshIntersectingBoxes>c__Iterator0.<CreateMeshIntersectingBoxes>c__AnonStorey2 <CreateMeshIntersectingBoxes>c__AnonStorey = new NonConvexMeshCollider.<CreateMeshIntersectingBoxes>c__Iterator0.<CreateMeshIntersectingBoxes>c__AnonStorey2();
		<CreateMeshIntersectingBoxes>c__AnonStorey.<>f__ref$0 = this;
		GameObject go = colliderGo.transform.parent.gameObject;
		int colliderLayer = colliderGo.layer;
		LayerMask colliderLayerMask = 1 << colliderLayer;
		Bounds bounds = NonConvexMeshCollider.CalculateLocalBounds(go);
		Mesh mesh = colliderGo.GetComponent<MeshFilter>().sharedMesh;
		Stopwatch swTree = Stopwatch.StartNew();
		<CreateMeshIntersectingBoxes>c__AnonStorey.tree = new NonConvexMeshCollider.SpatialBinaryTree(mesh, this.spatialTreeLevelDepth);
		swTree.Stop();
		if (this.outputTimeMeasurements)
		{
			UnityEngine.Debug.Log("SpatialTree Built in " + swTree.Elapsed);
		}
		NonConvexMeshCollider.Box[,,] boxes = new NonConvexMeshCollider.Box[this.boxesPerEdge, this.boxesPerEdge, this.boxesPerEdge];
		bool[,,] boxColliderPositions = new bool[this.boxesPerEdge, this.boxesPerEdge, this.boxesPerEdge];
		Vector3 s = bounds.size / (float)this.boxesPerEdge;
		<CreateMeshIntersectingBoxes>c__AnonStorey.halfExtent = s / 2f;
		Vector3[] directionsFromBoxCenterToCorners = new Vector3[]
		{
			new Vector3(1f, 1f, 1f),
			new Vector3(1f, 1f, -1f),
			new Vector3(1f, -1f, 1f),
			new Vector3(1f, -1f, -1f),
			new Vector3(-1f, 1f, 1f),
			new Vector3(-1f, 1f, -1f),
			new Vector3(-1f, -1f, 1f),
			new Vector3(-1f, -1f, -1f)
		};
		<CreateMeshIntersectingBoxes>c__AnonStorey.pointInsideMeshCache = new Dictionary<Vector3, bool>();
		Stopwatch sw = Stopwatch.StartNew();
		Collider[] colliders = new Collider[1000];
		for (int i = 0; i < this.boxesPerEdge; i++)
		{
			for (int j = 0; j < this.boxesPerEdge; j++)
			{
				for (int k = 0; k < this.boxesPerEdge; k++)
				{
					NonConvexMeshCollider.<CreateMeshIntersectingBoxes>c__Iterator0.<CreateMeshIntersectingBoxes>c__AnonStorey1 <CreateMeshIntersectingBoxes>c__AnonStorey2 = new NonConvexMeshCollider.<CreateMeshIntersectingBoxes>c__Iterator0.<CreateMeshIntersectingBoxes>c__AnonStorey1();
					<CreateMeshIntersectingBoxes>c__AnonStorey2.<>f__ref$0 = this;
					<CreateMeshIntersectingBoxes>c__AnonStorey2.<>f__ref$2 = <CreateMeshIntersectingBoxes>c__AnonStorey;
					<CreateMeshIntersectingBoxes>c__AnonStorey2.center = new Vector3(bounds.center.x - bounds.size.x / 2f + s.x * (float)i + <CreateMeshIntersectingBoxes>c__AnonStorey.halfExtent.x, bounds.center.y - bounds.size.y / 2f + s.y * (float)j + <CreateMeshIntersectingBoxes>c__AnonStorey.halfExtent.y, bounds.center.z - bounds.size.z / 2f + s.z * (float)k + <CreateMeshIntersectingBoxes>c__AnonStorey.halfExtent.z);
					if (!this.avoidExceedingMesh)
					{
						if (this.avoidGapsInside)
						{
							bool flag = this.IsInsideMesh(<CreateMeshIntersectingBoxes>c__AnonStorey2.center, <CreateMeshIntersectingBoxes>c__AnonStorey.tree, <CreateMeshIntersectingBoxes>c__AnonStorey.pointInsideMeshCache);
							boxColliderPositions[i, j, k] = flag;
						}
						else
						{
							bool flag2 = Physics.OverlapBoxNonAlloc(<CreateMeshIntersectingBoxes>c__AnonStorey2.center, <CreateMeshIntersectingBoxes>c__AnonStorey.halfExtent, colliders, Quaternion.identity, colliderLayerMask) > 0;
							boxColliderPositions[i, j, k] = flag2;
						}
					}
					else
					{
						bool flag3 = directionsFromBoxCenterToCorners.Select(new Func<Vector3, Vector3>(<CreateMeshIntersectingBoxes>c__AnonStorey2.<>m__0)).All(new Func<Vector3, bool>(<CreateMeshIntersectingBoxes>c__AnonStorey2.<>m__1));
						boxColliderPositions[i, j, k] = flag3;
					}
				}
			}
		}
		sw.Stop();
		if (this.outputTimeMeasurements)
		{
			UnityEngine.Debug.Log("Boxes analyzed in " + sw.Elapsed);
		}
		for (int x = 0; x < this.boxesPerEdge; x++)
		{
			for (int y = 0; y < this.boxesPerEdge; y++)
			{
				for (int z = 0; z < this.boxesPerEdge; z++)
				{
					if (boxColliderPositions[x, y, z])
					{
						Vector3 center = new Vector3(bounds.center.x - bounds.size.x / 2f + s.x * (float)x + s.x / 2f, bounds.center.y - bounds.size.y / 2f + s.y * (float)y + s.y / 2f, bounds.center.z - bounds.size.z / 2f + s.z * (float)z + s.z / 2f);
						NonConvexMeshCollider.Box b = new NonConvexMeshCollider.Box(boxes, new Vector3?(center), new Vector3?(s), new NonConvexMeshCollider.Vector3Int(x, y, z));
						boxes[x, y, z] = b;
						yield return b;
					}
				}
			}
		}
		yield break;
	}

	// Token: 0x0600155C RID: 5468 RVA: 0x00079AD0 File Offset: 0x00077ED0
	private bool IsInsideMesh(Vector3 p, NonConvexMeshCollider.SpatialBinaryTree tree, Dictionary<Vector3, bool> pointInsideMeshCache)
	{
		NonConvexMeshCollider.<IsInsideMesh>c__AnonStorey3 <IsInsideMesh>c__AnonStorey = new NonConvexMeshCollider.<IsInsideMesh>c__AnonStorey3();
		bool result;
		if (pointInsideMeshCache.TryGetValue(Vector3.one, out result))
		{
			return result;
		}
		<IsInsideMesh>c__AnonStorey.r = new Ray(p, new Vector3(1f, 0f, 0f));
		int num = tree.GetTris(<IsInsideMesh>c__AnonStorey.r).Count(new Func<NonConvexMeshCollider.Tri, bool>(<IsInsideMesh>c__AnonStorey.<>m__0));
		bool flag = num % 2 != 0;
		pointInsideMeshCache[p] = flag;
		return flag;
	}

	// Token: 0x0600155D RID: 5469 RVA: 0x00079B48 File Offset: 0x00077F48
	private int GetFirstEmptyLayer()
	{
		for (int i = 8; i <= 31; i++)
		{
			string text = LayerMask.LayerToName(i);
			if (text.Length == 0)
			{
				return i;
			}
		}
		throw new Exception("Didn't find unused layer for temporary assignment");
	}

	// Token: 0x0600155E RID: 5470 RVA: 0x00079B88 File Offset: 0x00077F88
	private static Bounds CalculateLocalBounds(GameObject go)
	{
		Bounds result = new Bounds(go.transform.position, Vector3.zero);
		foreach (Renderer renderer in go.GetComponentsInChildren<Renderer>())
		{
			result.Encapsulate(renderer.bounds);
		}
		Vector3 center = result.center - go.transform.position;
		result.center = center;
		return result;
	}

	// Token: 0x0600155F RID: 5471 RVA: 0x00079BFB File Offset: 0x00077FFB
	[CompilerGenerated]
	private static NonConvexMeshCollider.Box <MergeBoxes>m__0(NonConvexMeshCollider.Box b)
	{
		return b.Root;
	}

	// Token: 0x06001560 RID: 5472 RVA: 0x00079C03 File Offset: 0x00078003
	[CompilerGenerated]
	private static NonConvexMeshCollider.Box <MergeBoxes>m__1(NonConvexMeshCollider.Box b)
	{
		return b.Root;
	}

	// Token: 0x040011F4 RID: 4596
	private bool mergeBoxesToReduceNumber = true;

	// Token: 0x040011F5 RID: 4597
	private int spatialTreeLevelDepth = 9;

	// Token: 0x040011F6 RID: 4598
	public bool outputTimeMeasurements;

	// Token: 0x040011F7 RID: 4599
	[Tooltip("Will create a child game object called 'Colliders' to store the generated colliders in. \n\rThis leads to a cleaner and more organized structure. \n\rPlease note that collisions will then report the child game object. So you may want to check for transform.parent.gameObject on your collision check.")]
	public bool createChildGameObject = true;

	// Token: 0x040011F8 RID: 4600
	[Tooltip("Takes a bit more time to compute, but leads to more performance optimized colliders (less boxes).")]
	public bool avoidGapsInside;

	// Token: 0x040011F9 RID: 4601
	[Tooltip("Makes sure all box colliders are generated completely on the inside of the mesh. More expensive to compute, but desireable if you need to avoid false collisions of objects very close to another, like rings of a chain for example.")]
	public bool avoidExceedingMesh;

	// Token: 0x040011FA RID: 4602
	[Tooltip("The number of boxes your mesh will be segmented into, on each axis (x, y and z). \n\rHigher values lead to more accurate colliders but on the other hand makes computation and collision checks more expensive.")]
	public int boxesPerEdge = 20;

	// Token: 0x040011FB RID: 4603
	[Tooltip("The physics material to apply to the generated compound colliders.")]
	public PhysicMaterial physicsMaterialForColliders;

	// Token: 0x040011FC RID: 4604
	public const bool DebugOutput = false;

	// Token: 0x040011FD RID: 4605
	[CompilerGenerated]
	private static Func<NonConvexMeshCollider.Box, NonConvexMeshCollider.Box> <>f__am$cache0;

	// Token: 0x040011FE RID: 4606
	[CompilerGenerated]
	private static Func<NonConvexMeshCollider.Box, NonConvexMeshCollider.Box> <>f__am$cache1;

	// Token: 0x02000360 RID: 864
	public class BoundingBox
	{
		// Token: 0x06001561 RID: 5473 RVA: 0x00079C0B File Offset: 0x0007800B
		public BoundingBox(params NonConvexMeshCollider.Interval[] intervalsXyz) : this(intervalsXyz[0], intervalsXyz[1], intervalsXyz[2])
		{
		}

		// Token: 0x06001562 RID: 5474 RVA: 0x00079C1C File Offset: 0x0007801C
		public BoundingBox(NonConvexMeshCollider.Interval intervalX, NonConvexMeshCollider.Interval intervalY, NonConvexMeshCollider.Interval intervalZ)
		{
			this.IntervalX = intervalX;
			this.IntervalY = intervalY;
			this.IntervalZ = intervalZ;
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06001563 RID: 5475 RVA: 0x00079C39 File Offset: 0x00078039
		// (set) Token: 0x06001564 RID: 5476 RVA: 0x00079C41 File Offset: 0x00078041
		public NonConvexMeshCollider.Interval IntervalX
		{
			[CompilerGenerated]
			get
			{
				return this.<IntervalX>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<IntervalX>k__BackingField = value;
			}
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06001565 RID: 5477 RVA: 0x00079C4A File Offset: 0x0007804A
		// (set) Token: 0x06001566 RID: 5478 RVA: 0x00079C52 File Offset: 0x00078052
		public NonConvexMeshCollider.Interval IntervalY
		{
			[CompilerGenerated]
			get
			{
				return this.<IntervalY>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<IntervalY>k__BackingField = value;
			}
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06001567 RID: 5479 RVA: 0x00079C5B File Offset: 0x0007805B
		// (set) Token: 0x06001568 RID: 5480 RVA: 0x00079C63 File Offset: 0x00078063
		public NonConvexMeshCollider.Interval IntervalZ
		{
			[CompilerGenerated]
			get
			{
				return this.<IntervalZ>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<IntervalZ>k__BackingField = value;
			}
		}

		// Token: 0x06001569 RID: 5481 RVA: 0x00079C6C File Offset: 0x0007806C
		public bool IntersectsRayToPositiveX(Vector3 origin)
		{
			float val = this.IntervalX.Min - origin.x;
			float val2 = this.IntervalX.Max - origin.x;
			float num = Math.Min(val, val2);
			float num2 = Math.Max(val, val2);
			num = Math.Max(num, 0f);
			num2 = Math.Min(num2, 0f);
			return num2 >= num;
		}

		// Token: 0x0600156A RID: 5482 RVA: 0x00079CD0 File Offset: 0x000780D0
		public bool Intersects(Ray r)
		{
			float num = 1f / r.direction.x;
			float num2 = 1f / r.direction.y;
			float num3 = 1f / r.direction.z;
			float val = (this.IntervalX.Min - r.origin.x) * num;
			float val2 = (this.IntervalX.Max - r.origin.x) * num;
			float val3 = (this.IntervalY.Min - r.origin.y) * num2;
			float val4 = (this.IntervalY.Max - r.origin.y) * num2;
			float val5 = (this.IntervalZ.Min - r.origin.z) * num3;
			float val6 = (this.IntervalZ.Max - r.origin.z) * num3;
			float num4 = Math.Max(Math.Max(Math.Min(val, val2), Math.Min(val3, val4)), Math.Min(val5, val6));
			float num5 = Math.Min(Math.Min(Math.Max(val, val2), Math.Max(val3, val4)), Math.Max(val5, val6));
			return num5 >= 0f && num4 <= num5;
		}

		// Token: 0x0600156B RID: 5483 RVA: 0x00079E4C File Offset: 0x0007824C
		public bool Intersects(NonConvexMeshCollider.BoundingBox other)
		{
			return this.IntervalX.Intersects(other.IntervalX) && this.IntervalY.Intersects(other.IntervalY) && this.IntervalZ.Intersects(other.IntervalZ);
		}

		// Token: 0x0600156C RID: 5484 RVA: 0x00079E9C File Offset: 0x0007829C
		public override string ToString()
		{
			return string.Format("X: {0:N4}-{1:N4}, Y: {2:N4}-{3:N4}, Z: {4:N4}-{5:N4}", new object[]
			{
				this.IntervalX.Min,
				this.IntervalX.Max,
				this.IntervalY.Min,
				this.IntervalY.Max,
				this.IntervalZ.Min,
				this.IntervalZ.Max
			});
		}

		// Token: 0x040011FF RID: 4607
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private NonConvexMeshCollider.Interval <IntervalX>k__BackingField;

		// Token: 0x04001200 RID: 4608
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private NonConvexMeshCollider.Interval <IntervalY>k__BackingField;

		// Token: 0x04001201 RID: 4609
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private NonConvexMeshCollider.Interval <IntervalZ>k__BackingField;
	}

	// Token: 0x02000361 RID: 865
	public class Tri
	{
		// Token: 0x0600156D RID: 5485 RVA: 0x00079F2B File Offset: 0x0007832B
		public Tri(Vector3 a, Vector3 b, Vector3 c)
		{
			this.A = a;
			this.B = b;
			this.C = c;
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x0600156E RID: 5486 RVA: 0x00079F48 File Offset: 0x00078348
		// (set) Token: 0x0600156F RID: 5487 RVA: 0x00079F50 File Offset: 0x00078350
		public Vector3 A
		{
			[CompilerGenerated]
			get
			{
				return this.<A>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<A>k__BackingField = value;
			}
		}

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06001570 RID: 5488 RVA: 0x00079F59 File Offset: 0x00078359
		// (set) Token: 0x06001571 RID: 5489 RVA: 0x00079F61 File Offset: 0x00078361
		public Vector3 B
		{
			[CompilerGenerated]
			get
			{
				return this.<B>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<B>k__BackingField = value;
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06001572 RID: 5490 RVA: 0x00079F6A File Offset: 0x0007836A
		// (set) Token: 0x06001573 RID: 5491 RVA: 0x00079F72 File Offset: 0x00078372
		public Vector3 C
		{
			[CompilerGenerated]
			get
			{
				return this.<C>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<C>k__BackingField = value;
			}
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06001574 RID: 5492 RVA: 0x00079F7C File Offset: 0x0007837C
		public NonConvexMeshCollider.BoundingBox Bounds
		{
			get
			{
				if (this.bounds == null)
				{
					NonConvexMeshCollider.BoundingBox boundingBox = new NonConvexMeshCollider.BoundingBox(NonConvexMeshCollider.Interval.From(this.A.x, this.B.x, this.C.x), NonConvexMeshCollider.Interval.From(this.A.y, this.B.y, this.C.y), NonConvexMeshCollider.Interval.From(this.A.z, this.B.z, this.C.z));
					this.bounds = boundingBox;
				}
				return this.bounds;
			}
		}

		// Token: 0x06001575 RID: 5493 RVA: 0x0007A03C File Offset: 0x0007843C
		public bool Intersect(Ray ray)
		{
			Vector3 vector = this.B - this.A;
			Vector3 vector2 = this.C - this.A;
			Vector3 rhs = Vector3.Cross(ray.direction, vector2);
			float num = Vector3.Dot(vector, rhs);
			if (num > -1E-06f && num < 1E-06f)
			{
				return false;
			}
			float num2 = 1f / num;
			Vector3 lhs = ray.origin - this.A;
			float num3 = Vector3.Dot(lhs, rhs) * num2;
			if (num3 < 0f || num3 > 1f)
			{
				return false;
			}
			Vector3 rhs2 = Vector3.Cross(lhs, vector);
			float num4 = Vector3.Dot(ray.direction, rhs2) * num2;
			return num4 >= 0f && num3 + num4 <= 1f && Vector3.Dot(vector2, rhs2) * num2 > 1E-06f;
		}

		// Token: 0x06001576 RID: 5494 RVA: 0x0007A134 File Offset: 0x00078534
		public static bool Intersect(Vector3 p1, Vector3 p2, Vector3 p3, Ray ray)
		{
			Vector3 vector = p2 - p1;
			Vector3 vector2 = p3 - p1;
			Vector3 rhs = Vector3.Cross(ray.direction, vector2);
			float num = Vector3.Dot(vector, rhs);
			if (num > -1E-06f && num < 1E-06f)
			{
				return false;
			}
			float num2 = 1f / num;
			Vector3 lhs = ray.origin - p1;
			float num3 = Vector3.Dot(lhs, rhs) * num2;
			if (num3 < 0f || num3 > 1f)
			{
				return false;
			}
			Vector3 rhs2 = Vector3.Cross(lhs, vector);
			float num4 = Vector3.Dot(ray.direction, rhs2) * num2;
			return num4 >= 0f && num3 + num4 <= 1f && Vector3.Dot(vector2, rhs2) * num2 > 1E-06f;
		}

		// Token: 0x04001202 RID: 4610
		private NonConvexMeshCollider.BoundingBox bounds;

		// Token: 0x04001203 RID: 4611
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Vector3 <A>k__BackingField;

		// Token: 0x04001204 RID: 4612
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Vector3 <B>k__BackingField;

		// Token: 0x04001205 RID: 4613
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Vector3 <C>k__BackingField;
	}

	// Token: 0x02000362 RID: 866
	public class SpatialBinaryTree
	{
		// Token: 0x06001577 RID: 5495 RVA: 0x0007A210 File Offset: 0x00078610
		public SpatialBinaryTree(Mesh m, int maxLevels)
		{
			IEnumerable<Vector3> vertices = m.vertices;
			if (NonConvexMeshCollider.SpatialBinaryTree.<>f__am$cache0 == null)
			{
				NonConvexMeshCollider.SpatialBinaryTree.<>f__am$cache0 = new Func<Vector3, float>(NonConvexMeshCollider.SpatialBinaryTree.<SpatialBinaryTree>m__0);
			}
			float min = vertices.Min(NonConvexMeshCollider.SpatialBinaryTree.<>f__am$cache0);
			IEnumerable<Vector3> vertices2 = m.vertices;
			if (NonConvexMeshCollider.SpatialBinaryTree.<>f__am$cache1 == null)
			{
				NonConvexMeshCollider.SpatialBinaryTree.<>f__am$cache1 = new Func<Vector3, float>(NonConvexMeshCollider.SpatialBinaryTree.<SpatialBinaryTree>m__1);
			}
			NonConvexMeshCollider.Interval intervalX = new NonConvexMeshCollider.Interval(min, vertices2.Max(NonConvexMeshCollider.SpatialBinaryTree.<>f__am$cache1));
			IEnumerable<Vector3> vertices3 = m.vertices;
			if (NonConvexMeshCollider.SpatialBinaryTree.<>f__am$cache2 == null)
			{
				NonConvexMeshCollider.SpatialBinaryTree.<>f__am$cache2 = new Func<Vector3, float>(NonConvexMeshCollider.SpatialBinaryTree.<SpatialBinaryTree>m__2);
			}
			float min2 = vertices3.Min(NonConvexMeshCollider.SpatialBinaryTree.<>f__am$cache2);
			IEnumerable<Vector3> vertices4 = m.vertices;
			if (NonConvexMeshCollider.SpatialBinaryTree.<>f__am$cache3 == null)
			{
				NonConvexMeshCollider.SpatialBinaryTree.<>f__am$cache3 = new Func<Vector3, float>(NonConvexMeshCollider.SpatialBinaryTree.<SpatialBinaryTree>m__3);
			}
			NonConvexMeshCollider.Interval intervalY = new NonConvexMeshCollider.Interval(min2, vertices4.Max(NonConvexMeshCollider.SpatialBinaryTree.<>f__am$cache3));
			IEnumerable<Vector3> vertices5 = m.vertices;
			if (NonConvexMeshCollider.SpatialBinaryTree.<>f__am$cache4 == null)
			{
				NonConvexMeshCollider.SpatialBinaryTree.<>f__am$cache4 = new Func<Vector3, float>(NonConvexMeshCollider.SpatialBinaryTree.<SpatialBinaryTree>m__4);
			}
			float min3 = vertices5.Min(NonConvexMeshCollider.SpatialBinaryTree.<>f__am$cache4);
			IEnumerable<Vector3> vertices6 = m.vertices;
			if (NonConvexMeshCollider.SpatialBinaryTree.<>f__am$cache5 == null)
			{
				NonConvexMeshCollider.SpatialBinaryTree.<>f__am$cache5 = new Func<Vector3, float>(NonConvexMeshCollider.SpatialBinaryTree.<SpatialBinaryTree>m__5);
			}
			NonConvexMeshCollider.BoundingBox bounds = new NonConvexMeshCollider.BoundingBox(intervalX, intervalY, new NonConvexMeshCollider.Interval(min3, vertices6.Max(NonConvexMeshCollider.SpatialBinaryTree.<>f__am$cache5)));
			this.root = new NonConvexMeshCollider.SpatialBinaryTreeNode(0, maxLevels, bounds);
			int num = m.triangles.Length / 3;
			for (int i = 0; i < num; i++)
			{
				Vector3 a = m.vertices[m.triangles[i * 3]];
				Vector3 b = m.vertices[m.triangles[i * 3 + 1]];
				Vector3 c = m.vertices[m.triangles[i * 3 + 2]];
				NonConvexMeshCollider.Tri t = new NonConvexMeshCollider.Tri(a, b, c);
				this.Add(t);
			}
		}

		// Token: 0x06001578 RID: 5496 RVA: 0x0007A3BE File Offset: 0x000787BE
		public void Add(NonConvexMeshCollider.Tri t)
		{
			this.root.Add(t);
		}

		// Token: 0x06001579 RID: 5497 RVA: 0x0007A3CC File Offset: 0x000787CC
		public IEnumerable<NonConvexMeshCollider.Tri> GetTris(Ray r)
		{
			return new HashSet<NonConvexMeshCollider.Tri>(this.root.GetTris(r));
		}

		// Token: 0x0600157A RID: 5498 RVA: 0x0007A3DF File Offset: 0x000787DF
		[CompilerGenerated]
		private static float <SpatialBinaryTree>m__0(Vector3 v)
		{
			return v.x;
		}

		// Token: 0x0600157B RID: 5499 RVA: 0x0007A3E8 File Offset: 0x000787E8
		[CompilerGenerated]
		private static float <SpatialBinaryTree>m__1(Vector3 v)
		{
			return v.x;
		}

		// Token: 0x0600157C RID: 5500 RVA: 0x0007A3F1 File Offset: 0x000787F1
		[CompilerGenerated]
		private static float <SpatialBinaryTree>m__2(Vector3 v)
		{
			return v.y;
		}

		// Token: 0x0600157D RID: 5501 RVA: 0x0007A3FA File Offset: 0x000787FA
		[CompilerGenerated]
		private static float <SpatialBinaryTree>m__3(Vector3 v)
		{
			return v.y;
		}

		// Token: 0x0600157E RID: 5502 RVA: 0x0007A403 File Offset: 0x00078803
		[CompilerGenerated]
		private static float <SpatialBinaryTree>m__4(Vector3 v)
		{
			return v.z;
		}

		// Token: 0x0600157F RID: 5503 RVA: 0x0007A40C File Offset: 0x0007880C
		[CompilerGenerated]
		private static float <SpatialBinaryTree>m__5(Vector3 v)
		{
			return v.z;
		}

		// Token: 0x04001206 RID: 4614
		private readonly NonConvexMeshCollider.SpatialBinaryTreeNode root;

		// Token: 0x04001207 RID: 4615
		[CompilerGenerated]
		private static Func<Vector3, float> <>f__am$cache0;

		// Token: 0x04001208 RID: 4616
		[CompilerGenerated]
		private static Func<Vector3, float> <>f__am$cache1;

		// Token: 0x04001209 RID: 4617
		[CompilerGenerated]
		private static Func<Vector3, float> <>f__am$cache2;

		// Token: 0x0400120A RID: 4618
		[CompilerGenerated]
		private static Func<Vector3, float> <>f__am$cache3;

		// Token: 0x0400120B RID: 4619
		[CompilerGenerated]
		private static Func<Vector3, float> <>f__am$cache4;

		// Token: 0x0400120C RID: 4620
		[CompilerGenerated]
		private static Func<Vector3, float> <>f__am$cache5;
	}

	// Token: 0x02000363 RID: 867
	public class SpatialBinaryTreeNode
	{
		// Token: 0x06001580 RID: 5504 RVA: 0x0007A418 File Offset: 0x00078818
		public SpatialBinaryTreeNode(int level, int maxLevels, NonConvexMeshCollider.BoundingBox bounds)
		{
			this.level = level;
			this.maxLevels = maxLevels;
			this.bounds = bounds;
			if (level >= maxLevels)
			{
				this.tris = new List<NonConvexMeshCollider.Tri>();
			}
			else
			{
				int num = level % 3;
				this.boundsChildA = new NonConvexMeshCollider.BoundingBox((num != 0) ? bounds.IntervalX : bounds.IntervalX.LowerHalf, (num != 1) ? bounds.IntervalY : bounds.IntervalY.LowerHalf, (num != 2) ? bounds.IntervalZ : bounds.IntervalZ.LowerHalf);
				this.boundsChildB = new NonConvexMeshCollider.BoundingBox((num != 0) ? bounds.IntervalX : bounds.IntervalX.UpperHalf, (num != 1) ? bounds.IntervalY : bounds.IntervalY.UpperHalf, (num != 2) ? bounds.IntervalZ : bounds.IntervalZ.UpperHalf);
			}
		}

		// Token: 0x06001581 RID: 5505 RVA: 0x0007A520 File Offset: 0x00078920
		public void Add(NonConvexMeshCollider.Tri t)
		{
			if (this.tris != null)
			{
				this.tris.Add(t);
			}
			else
			{
				if (this.boundsChildA.Intersects(t.Bounds))
				{
					if (this.childA == null)
					{
						this.childA = new NonConvexMeshCollider.SpatialBinaryTreeNode(this.level + 1, this.maxLevels, this.boundsChildA);
					}
					this.childA.Add(t);
				}
				if (this.boundsChildB.Intersects(t.Bounds))
				{
					if (this.childB == null)
					{
						this.childB = new NonConvexMeshCollider.SpatialBinaryTreeNode(this.level + 1, this.maxLevels, this.boundsChildB);
					}
					this.childB.Add(t);
				}
			}
		}

		// Token: 0x06001582 RID: 5506 RVA: 0x0007A5E4 File Offset: 0x000789E4
		public IEnumerable<NonConvexMeshCollider.Tri> GetTris(Ray r)
		{
			if (!this.bounds.Intersects(r))
			{
				yield break;
			}
			if (this.tris != null)
			{
				foreach (NonConvexMeshCollider.Tri t in this.tris)
				{
					yield return t;
				}
			}
			else
			{
				if (this.childA != null)
				{
					foreach (NonConvexMeshCollider.Tri t2 in this.childA.GetTris(r))
					{
						yield return t2;
					}
				}
				if (this.childB != null)
				{
					foreach (NonConvexMeshCollider.Tri t3 in this.childB.GetTris(r))
					{
						yield return t3;
					}
				}
			}
			yield break;
		}

		// Token: 0x06001583 RID: 5507 RVA: 0x0007A60E File Offset: 0x00078A0E
		public override string ToString()
		{
			if (this.tris != null)
			{
				return "Leaf node: " + this.tris.Count + " tris";
			}
			return this.bounds.ToString();
		}

		// Token: 0x0400120D RID: 4621
		private readonly int level;

		// Token: 0x0400120E RID: 4622
		private readonly int maxLevels;

		// Token: 0x0400120F RID: 4623
		private NonConvexMeshCollider.SpatialBinaryTreeNode childA;

		// Token: 0x04001210 RID: 4624
		private NonConvexMeshCollider.SpatialBinaryTreeNode childB;

		// Token: 0x04001211 RID: 4625
		private readonly List<NonConvexMeshCollider.Tri> tris;

		// Token: 0x04001212 RID: 4626
		private readonly NonConvexMeshCollider.BoundingBox bounds;

		// Token: 0x04001213 RID: 4627
		private readonly NonConvexMeshCollider.BoundingBox boundsChildA;

		// Token: 0x04001214 RID: 4628
		private readonly NonConvexMeshCollider.BoundingBox boundsChildB;

		// Token: 0x02000F39 RID: 3897
		[CompilerGenerated]
		private sealed class <GetTris>c__Iterator0 : IEnumerable, IEnumerable<NonConvexMeshCollider.Tri>, IEnumerator, IDisposable, IEnumerator<NonConvexMeshCollider.Tri>
		{
			// Token: 0x06007312 RID: 29458 RVA: 0x0007A646 File Offset: 0x00078A46
			[DebuggerHidden]
			public <GetTris>c__Iterator0()
			{
			}

			// Token: 0x06007313 RID: 29459 RVA: 0x0007A650 File Offset: 0x00078A50
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0U:
					if (!this.bounds.Intersects(r))
					{
						return false;
					}
					if (this.tris != null)
					{
						enumerator = this.tris.GetEnumerator();
						num = 4294967293U;
					}
					else
					{
						if (this.childA != null)
						{
							enumerator2 = this.childA.GetTris(r).GetEnumerator();
							num = 4294967293U;
							goto Block_6;
						}
						goto IL_190;
					}
					break;
				case 1U:
					break;
				case 2U:
					goto IL_11C;
				case 3U:
					Block_8:
					try
					{
						switch (num)
						{
						}
						if (enumerator3.MoveNext())
						{
							t3 = enumerator3.Current;
							this.$current = t3;
							if (!this.$disposing)
							{
								this.$PC = 3;
							}
							flag = true;
							return true;
						}
					}
					finally
					{
						if (!flag)
						{
							if (enumerator3 != null)
							{
								enumerator3.Dispose();
							}
						}
					}
					goto IL_238;
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
						t = enumerator.Current;
						this.$current = t;
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
				goto IL_238;
				Block_6:
				try
				{
					IL_11C:
					switch (num)
					{
					}
					if (enumerator2.MoveNext())
					{
						t2 = enumerator2.Current;
						this.$current = t2;
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator2 != null)
						{
							enumerator2.Dispose();
						}
					}
				}
				IL_190:
				if (this.childB != null)
				{
					enumerator3 = this.childB.GetTris(r).GetEnumerator();
					num = 4294967293U;
					goto Block_8;
				}
				IL_238:
				this.$PC = -1;
				return false;
			}

			// Token: 0x170010D1 RID: 4305
			// (get) Token: 0x06007314 RID: 29460 RVA: 0x0007A8C8 File Offset: 0x00078CC8
			NonConvexMeshCollider.Tri IEnumerator<NonConvexMeshCollider.Tri>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170010D2 RID: 4306
			// (get) Token: 0x06007315 RID: 29461 RVA: 0x0007A8D0 File Offset: 0x00078CD0
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x06007316 RID: 29462 RVA: 0x0007A8D8 File Offset: 0x00078CD8
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
				case 2U:
					try
					{
					}
					finally
					{
						if (enumerator2 != null)
						{
							enumerator2.Dispose();
						}
					}
					break;
				case 3U:
					try
					{
					}
					finally
					{
						if (enumerator3 != null)
						{
							enumerator3.Dispose();
						}
					}
					break;
				}
			}

			// Token: 0x06007317 RID: 29463 RVA: 0x0007A998 File Offset: 0x00078D98
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x06007318 RID: 29464 RVA: 0x0007A99F File Offset: 0x00078D9F
			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<NonConvexMeshCollider.Tri>.GetEnumerator();
			}

			// Token: 0x06007319 RID: 29465 RVA: 0x0007A9A8 File Offset: 0x00078DA8
			[DebuggerHidden]
			IEnumerator<NonConvexMeshCollider.Tri> IEnumerable<NonConvexMeshCollider.Tri>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				NonConvexMeshCollider.SpatialBinaryTreeNode.<GetTris>c__Iterator0 <GetTris>c__Iterator = new NonConvexMeshCollider.SpatialBinaryTreeNode.<GetTris>c__Iterator0();
				<GetTris>c__Iterator.$this = this;
				<GetTris>c__Iterator.r = r;
				return <GetTris>c__Iterator;
			}

			// Token: 0x040066FD RID: 26365
			internal Ray r;

			// Token: 0x040066FE RID: 26366
			internal List<NonConvexMeshCollider.Tri>.Enumerator $locvar0;

			// Token: 0x040066FF RID: 26367
			internal NonConvexMeshCollider.Tri <t>__1;

			// Token: 0x04006700 RID: 26368
			internal IEnumerator<NonConvexMeshCollider.Tri> $locvar1;

			// Token: 0x04006701 RID: 26369
			internal NonConvexMeshCollider.Tri <t>__2;

			// Token: 0x04006702 RID: 26370
			internal IEnumerator<NonConvexMeshCollider.Tri> $locvar2;

			// Token: 0x04006703 RID: 26371
			internal NonConvexMeshCollider.Tri <t>__3;

			// Token: 0x04006704 RID: 26372
			internal NonConvexMeshCollider.SpatialBinaryTreeNode $this;

			// Token: 0x04006705 RID: 26373
			internal NonConvexMeshCollider.Tri $current;

			// Token: 0x04006706 RID: 26374
			internal bool $disposing;

			// Token: 0x04006707 RID: 26375
			internal int $PC;
		}
	}

	// Token: 0x02000364 RID: 868
	public class Interval
	{
		// Token: 0x06001584 RID: 5508 RVA: 0x0007A9E8 File Offset: 0x00078DE8
		public Interval(float min, float max)
		{
			this.Min = min;
			this.Max = max;
			this.Center = (min + max) / 2f;
		}

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06001585 RID: 5509 RVA: 0x0007AA0D File Offset: 0x00078E0D
		// (set) Token: 0x06001586 RID: 5510 RVA: 0x0007AA15 File Offset: 0x00078E15
		public float Min
		{
			[CompilerGenerated]
			get
			{
				return this.<Min>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Min>k__BackingField = value;
			}
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06001587 RID: 5511 RVA: 0x0007AA1E File Offset: 0x00078E1E
		// (set) Token: 0x06001588 RID: 5512 RVA: 0x0007AA26 File Offset: 0x00078E26
		public float Max
		{
			[CompilerGenerated]
			get
			{
				return this.<Max>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Max>k__BackingField = value;
			}
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06001589 RID: 5513 RVA: 0x0007AA2F File Offset: 0x00078E2F
		// (set) Token: 0x0600158A RID: 5514 RVA: 0x0007AA37 File Offset: 0x00078E37
		public float Center
		{
			[CompilerGenerated]
			get
			{
				return this.<Center>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Center>k__BackingField = value;
			}
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x0600158B RID: 5515 RVA: 0x0007AA40 File Offset: 0x00078E40
		public float Size
		{
			get
			{
				return this.Max - this.Min;
			}
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x0600158C RID: 5516 RVA: 0x0007AA4F File Offset: 0x00078E4F
		public NonConvexMeshCollider.Interval LowerHalf
		{
			get
			{
				return new NonConvexMeshCollider.Interval(this.Min, this.Center);
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x0600158D RID: 5517 RVA: 0x0007AA62 File Offset: 0x00078E62
		public NonConvexMeshCollider.Interval UpperHalf
		{
			get
			{
				return new NonConvexMeshCollider.Interval(this.Center, this.Max);
			}
		}

		// Token: 0x0600158E RID: 5518 RVA: 0x0007AA75 File Offset: 0x00078E75
		public bool Contains(float v)
		{
			return v >= this.Min && v < this.Max;
		}

		// Token: 0x0600158F RID: 5519 RVA: 0x0007AA94 File Offset: 0x00078E94
		public bool IsInLeftHalf(float v)
		{
			return v >= this.Min && v < this.Center;
		}

		// Token: 0x06001590 RID: 5520 RVA: 0x0007AAAE File Offset: 0x00078EAE
		public bool IsInRightHalf(float v)
		{
			return v > this.Center && v < this.Max;
		}

		// Token: 0x06001591 RID: 5521 RVA: 0x0007AAC8 File Offset: 0x00078EC8
		public bool Intersects(NonConvexMeshCollider.Interval other)
		{
			return this.Min <= other.Max && other.Min <= this.Max;
		}

		// Token: 0x06001592 RID: 5522 RVA: 0x0007AAEF File Offset: 0x00078EEF
		public static NonConvexMeshCollider.Interval From(float a, float b, float c)
		{
			return new NonConvexMeshCollider.Interval(Math.Min(Math.Min(a, b), c), Math.Max(Math.Max(a, b), c));
		}

		// Token: 0x06001593 RID: 5523 RVA: 0x0007AB10 File Offset: 0x00078F10
		public static NonConvexMeshCollider.Interval From(float a, float b)
		{
			return new NonConvexMeshCollider.Interval(Math.Min(a, b), Math.Max(a, b));
		}

		// Token: 0x04001215 RID: 4629
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private float <Min>k__BackingField;

		// Token: 0x04001216 RID: 4630
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private float <Max>k__BackingField;

		// Token: 0x04001217 RID: 4631
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private float <Center>k__BackingField;
	}

	// Token: 0x02000365 RID: 869
	public class Box
	{
		// Token: 0x06001594 RID: 5524 RVA: 0x0007AB25 File Offset: 0x00078F25
		public Box(NonConvexMeshCollider.Box[,,] boxes, Vector3? center = null, Vector3? size = null, NonConvexMeshCollider.Vector3Int lastLevelGridPos = null)
		{
			this.boxes = boxes;
			this.lastLevelGridPos = lastLevelGridPos;
			this.center = center;
			this.size = size;
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06001595 RID: 5525 RVA: 0x0007AB4C File Offset: 0x00078F4C
		public Vector3 Center
		{
			get
			{
				Vector3? vector = this.center;
				if (vector == null)
				{
					if (this.Children == null)
					{
						throw new Exception("Last level child box needs a center position");
					}
					Vector3 vector2 = Vector3.zero;
					foreach (NonConvexMeshCollider.Box box in this.LastLevelBoxes)
					{
						vector2 += box.Center;
					}
					vector2 /= (float)this.LastLevelBoxes.Length;
					this.center = new Vector3?(vector2);
				}
				return this.center.Value;
			}
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06001596 RID: 5526 RVA: 0x0007ABE4 File Offset: 0x00078FE4
		public Vector3 Size
		{
			get
			{
				Vector3? vector = this.size;
				if (vector == null)
				{
					if (this.Children == null)
					{
						throw new Exception("Last level child box needs a size");
					}
					Vector3 vector2 = this.LastLevelBoxes[0].Size;
					this.size = new Vector3?(new Vector3((float)this.GridSize.X * vector2.x, (float)this.GridSize.Y * vector2.y, (float)this.GridSize.Z * vector2.z));
				}
				return this.size.Value;
			}
		}

		// Token: 0x06001597 RID: 5527 RVA: 0x0007AC84 File Offset: 0x00079084
		private void MergeWith(NonConvexMeshCollider.Box other)
		{
			NonConvexMeshCollider.Box box = new NonConvexMeshCollider.Box(this.boxes, null, null, null);
			foreach (NonConvexMeshCollider.Box box2 in new NonConvexMeshCollider.Box[]
			{
				this,
				other
			})
			{
				box2.Parent = box;
			}
			box.Children = new NonConvexMeshCollider.Box[]
			{
				this,
				other
			};
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06001598 RID: 5528 RVA: 0x0007ACF7 File Offset: 0x000790F7
		// (set) Token: 0x06001599 RID: 5529 RVA: 0x0007ACFF File Offset: 0x000790FF
		public NonConvexMeshCollider.Box Parent
		{
			[CompilerGenerated]
			get
			{
				return this.<Parent>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Parent>k__BackingField = value;
			}
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x0600159A RID: 5530 RVA: 0x0007AD08 File Offset: 0x00079108
		// (set) Token: 0x0600159B RID: 5531 RVA: 0x0007AD10 File Offset: 0x00079110
		public NonConvexMeshCollider.Box[] Children
		{
			[CompilerGenerated]
			get
			{
				return this.<Children>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Children>k__BackingField = value;
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x0600159C RID: 5532 RVA: 0x0007AD1C File Offset: 0x0007911C
		public IEnumerable<NonConvexMeshCollider.Box> Parents
		{
			get
			{
				NonConvexMeshCollider.Box b = this;
				while (b.Parent != null)
				{
					yield return b.Parent;
					b = b.Parent;
				}
				yield break;
			}
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x0600159D RID: 5533 RVA: 0x0007AD40 File Offset: 0x00079140
		public IEnumerable<NonConvexMeshCollider.Box> SelfAndParents
		{
			get
			{
				yield return this;
				foreach (NonConvexMeshCollider.Box parent in this.Parents)
				{
					yield return parent;
				}
				yield break;
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x0600159E RID: 5534 RVA: 0x0007AD63 File Offset: 0x00079163
		public NonConvexMeshCollider.Box Root
		{
			get
			{
				return (this.Parent != null) ? this.Parent.Root : this;
			}
		}

		// Token: 0x0600159F RID: 5535 RVA: 0x0007AD84 File Offset: 0x00079184
		public bool TryMerge(NonConvexMeshCollider.Vector3Int direction)
		{
			if (this.Parent != null)
			{
				return false;
			}
			foreach (NonConvexMeshCollider.Vector3Int vector3Int in this.CoveredGridPositions)
			{
				NonConvexMeshCollider.Vector3Int vector3Int2 = new NonConvexMeshCollider.Vector3Int(vector3Int.X + direction.X, vector3Int.Y + direction.Y, vector3Int.Z + direction.Z);
				if (vector3Int2.X >= 0 && vector3Int2.Y >= 0 && vector3Int2.Z >= 0)
				{
					if (vector3Int2.X < this.boxes.GetLength(0) && vector3Int2.Y < this.boxes.GetLength(1) && vector3Int2.Z < this.boxes.GetLength(2))
					{
						NonConvexMeshCollider.Box box = this.boxes[vector3Int2.X, vector3Int2.Y, vector3Int2.Z];
						if (box != null)
						{
							box = box.Root;
							if (box != this)
							{
								if (direction.X != 0 || box.GridSize.X == this.GridSize.X)
								{
									if (direction.Y != 0 || box.GridSize.Y == this.GridSize.Y)
									{
										if (direction.Z != 0 || box.GridSize.Z == this.GridSize.Z)
										{
											if (direction.X != 0 || this.MinGridPos.X == box.MinGridPos.X)
											{
												if (direction.Y != 0 || this.MinGridPos.Y == box.MinGridPos.Y)
												{
													if (direction.Z != 0 || this.MinGridPos.Z == box.MinGridPos.Z)
													{
														this.MergeWith(box);
														return true;
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x060015A0 RID: 5536 RVA: 0x0007AFE0 File Offset: 0x000793E0
		public IEnumerable<NonConvexMeshCollider.Box> ChildrenRecursive
		{
			get
			{
				if (this.Children == null)
				{
					yield break;
				}
				foreach (NonConvexMeshCollider.Box c in this.Children)
				{
					yield return c;
					foreach (NonConvexMeshCollider.Box cc in c.ChildrenRecursive)
					{
						yield return cc;
					}
				}
				yield break;
			}
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x060015A1 RID: 5537 RVA: 0x0007B004 File Offset: 0x00079404
		public IEnumerable<NonConvexMeshCollider.Box> SelfAndChildrenRecursive
		{
			get
			{
				yield return this;
				foreach (NonConvexMeshCollider.Box c in this.ChildrenRecursive)
				{
					yield return c;
				}
				yield break;
			}
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x060015A2 RID: 5538 RVA: 0x0007B028 File Offset: 0x00079428
		public NonConvexMeshCollider.Box[] LastLevelBoxes
		{
			get
			{
				if (this.lastLevelBoxes == null)
				{
					IEnumerable<NonConvexMeshCollider.Box> selfAndChildrenRecursive = this.SelfAndChildrenRecursive;
					if (NonConvexMeshCollider.Box.<>f__am$cache0 == null)
					{
						NonConvexMeshCollider.Box.<>f__am$cache0 = new Func<NonConvexMeshCollider.Box, bool>(NonConvexMeshCollider.Box.<get_LastLevelBoxes>m__0);
					}
					this.lastLevelBoxes = selfAndChildrenRecursive.Where(NonConvexMeshCollider.Box.<>f__am$cache0).ToArray<NonConvexMeshCollider.Box>();
				}
				return this.lastLevelBoxes;
			}
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x060015A3 RID: 5539 RVA: 0x0007B079 File Offset: 0x00079479
		private IEnumerable<NonConvexMeshCollider.Vector3Int> CoveredGridPositions
		{
			get
			{
				IEnumerable<NonConvexMeshCollider.Box> source = this.LastLevelBoxes;
				if (NonConvexMeshCollider.Box.<>f__am$cache1 == null)
				{
					NonConvexMeshCollider.Box.<>f__am$cache1 = new Func<NonConvexMeshCollider.Box, NonConvexMeshCollider.Vector3Int>(NonConvexMeshCollider.Box.<get_CoveredGridPositions>m__1);
				}
				return source.Select(NonConvexMeshCollider.Box.<>f__am$cache1);
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x060015A4 RID: 5540 RVA: 0x0007B0A4 File Offset: 0x000794A4
		private int MinGridPosX
		{
			get
			{
				int result;
				if (this.Children == null)
				{
					result = this.lastLevelGridPos.X;
				}
				else
				{
					IEnumerable<NonConvexMeshCollider.Vector3Int> coveredGridPositions = this.CoveredGridPositions;
					if (NonConvexMeshCollider.Box.<>f__am$cache2 == null)
					{
						NonConvexMeshCollider.Box.<>f__am$cache2 = new Func<NonConvexMeshCollider.Vector3Int, int>(NonConvexMeshCollider.Box.<get_MinGridPosX>m__2);
					}
					result = coveredGridPositions.Min(NonConvexMeshCollider.Box.<>f__am$cache2);
				}
				return result;
			}
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x060015A5 RID: 5541 RVA: 0x0007B0F4 File Offset: 0x000794F4
		private int MinGridPosY
		{
			get
			{
				int result;
				if (this.Children == null)
				{
					result = this.lastLevelGridPos.Y;
				}
				else
				{
					IEnumerable<NonConvexMeshCollider.Vector3Int> coveredGridPositions = this.CoveredGridPositions;
					if (NonConvexMeshCollider.Box.<>f__am$cache3 == null)
					{
						NonConvexMeshCollider.Box.<>f__am$cache3 = new Func<NonConvexMeshCollider.Vector3Int, int>(NonConvexMeshCollider.Box.<get_MinGridPosY>m__3);
					}
					result = coveredGridPositions.Min(NonConvexMeshCollider.Box.<>f__am$cache3);
				}
				return result;
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x060015A6 RID: 5542 RVA: 0x0007B144 File Offset: 0x00079544
		private int MinGridPosZ
		{
			get
			{
				int result;
				if (this.Children == null)
				{
					result = this.lastLevelGridPos.Z;
				}
				else
				{
					IEnumerable<NonConvexMeshCollider.Vector3Int> coveredGridPositions = this.CoveredGridPositions;
					if (NonConvexMeshCollider.Box.<>f__am$cache4 == null)
					{
						NonConvexMeshCollider.Box.<>f__am$cache4 = new Func<NonConvexMeshCollider.Vector3Int, int>(NonConvexMeshCollider.Box.<get_MinGridPosZ>m__4);
					}
					result = coveredGridPositions.Min(NonConvexMeshCollider.Box.<>f__am$cache4);
				}
				return result;
			}
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x060015A7 RID: 5543 RVA: 0x0007B194 File Offset: 0x00079594
		private int MaxGridPosX
		{
			get
			{
				int result;
				if (this.Children == null)
				{
					result = this.lastLevelGridPos.X;
				}
				else
				{
					IEnumerable<NonConvexMeshCollider.Vector3Int> coveredGridPositions = this.CoveredGridPositions;
					if (NonConvexMeshCollider.Box.<>f__am$cache5 == null)
					{
						NonConvexMeshCollider.Box.<>f__am$cache5 = new Func<NonConvexMeshCollider.Vector3Int, int>(NonConvexMeshCollider.Box.<get_MaxGridPosX>m__5);
					}
					result = coveredGridPositions.Max(NonConvexMeshCollider.Box.<>f__am$cache5);
				}
				return result;
			}
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x060015A8 RID: 5544 RVA: 0x0007B1E4 File Offset: 0x000795E4
		private int MaxGridPosY
		{
			get
			{
				int result;
				if (this.Children == null)
				{
					result = this.lastLevelGridPos.Y;
				}
				else
				{
					IEnumerable<NonConvexMeshCollider.Vector3Int> coveredGridPositions = this.CoveredGridPositions;
					if (NonConvexMeshCollider.Box.<>f__am$cache6 == null)
					{
						NonConvexMeshCollider.Box.<>f__am$cache6 = new Func<NonConvexMeshCollider.Vector3Int, int>(NonConvexMeshCollider.Box.<get_MaxGridPosY>m__6);
					}
					result = coveredGridPositions.Max(NonConvexMeshCollider.Box.<>f__am$cache6);
				}
				return result;
			}
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x060015A9 RID: 5545 RVA: 0x0007B234 File Offset: 0x00079634
		private int MaxGridPosZ
		{
			get
			{
				int result;
				if (this.Children == null)
				{
					result = this.lastLevelGridPos.Z;
				}
				else
				{
					IEnumerable<NonConvexMeshCollider.Vector3Int> coveredGridPositions = this.CoveredGridPositions;
					if (NonConvexMeshCollider.Box.<>f__am$cache7 == null)
					{
						NonConvexMeshCollider.Box.<>f__am$cache7 = new Func<NonConvexMeshCollider.Vector3Int, int>(NonConvexMeshCollider.Box.<get_MaxGridPosZ>m__7);
					}
					result = coveredGridPositions.Max(NonConvexMeshCollider.Box.<>f__am$cache7);
				}
				return result;
			}
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x060015AA RID: 5546 RVA: 0x0007B284 File Offset: 0x00079684
		private NonConvexMeshCollider.Vector3Int MinGridPos
		{
			get
			{
				NonConvexMeshCollider.Vector3Int result;
				if ((result = this.minGridPos) == null)
				{
					result = (this.minGridPos = new NonConvexMeshCollider.Vector3Int(this.MinGridPosX, this.MinGridPosY, this.MinGridPosZ));
				}
				return result;
			}
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x060015AB RID: 5547 RVA: 0x0007B2C0 File Offset: 0x000796C0
		private NonConvexMeshCollider.Vector3Int MaxGridPos
		{
			get
			{
				NonConvexMeshCollider.Vector3Int result;
				if ((result = this.maxGridPos) == null)
				{
					result = (this.maxGridPos = new NonConvexMeshCollider.Vector3Int(this.MaxGridPosX, this.MaxGridPosY, this.MaxGridPosZ));
				}
				return result;
			}
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x060015AC RID: 5548 RVA: 0x0007B2FC File Offset: 0x000796FC
		private NonConvexMeshCollider.Vector3Int GridSize
		{
			get
			{
				if (this.gridSize == null)
				{
					this.gridSize = ((this.Children != null) ? new NonConvexMeshCollider.Vector3Int(this.MaxGridPos.X - this.MinGridPos.X + 1, this.MaxGridPos.Y - this.MinGridPos.Y + 1, this.MaxGridPos.Z - this.MinGridPos.Z + 1) : NonConvexMeshCollider.Vector3Int.One);
				}
				return this.gridSize;
			}
		}

		// Token: 0x060015AD RID: 5549 RVA: 0x0007B385 File Offset: 0x00079785
		[CompilerGenerated]
		private static bool <get_LastLevelBoxes>m__0(NonConvexMeshCollider.Box c)
		{
			return c.Children == null;
		}

		// Token: 0x060015AE RID: 5550 RVA: 0x0007B390 File Offset: 0x00079790
		[CompilerGenerated]
		private static NonConvexMeshCollider.Vector3Int <get_CoveredGridPositions>m__1(NonConvexMeshCollider.Box c)
		{
			return c.lastLevelGridPos;
		}

		// Token: 0x060015AF RID: 5551 RVA: 0x0007B398 File Offset: 0x00079798
		[CompilerGenerated]
		private static int <get_MinGridPosX>m__2(NonConvexMeshCollider.Vector3Int p)
		{
			return p.X;
		}

		// Token: 0x060015B0 RID: 5552 RVA: 0x0007B3A0 File Offset: 0x000797A0
		[CompilerGenerated]
		private static int <get_MinGridPosY>m__3(NonConvexMeshCollider.Vector3Int p)
		{
			return p.Y;
		}

		// Token: 0x060015B1 RID: 5553 RVA: 0x0007B3A8 File Offset: 0x000797A8
		[CompilerGenerated]
		private static int <get_MinGridPosZ>m__4(NonConvexMeshCollider.Vector3Int p)
		{
			return p.Z;
		}

		// Token: 0x060015B2 RID: 5554 RVA: 0x0007B3B0 File Offset: 0x000797B0
		[CompilerGenerated]
		private static int <get_MaxGridPosX>m__5(NonConvexMeshCollider.Vector3Int p)
		{
			return p.X;
		}

		// Token: 0x060015B3 RID: 5555 RVA: 0x0007B3B8 File Offset: 0x000797B8
		[CompilerGenerated]
		private static int <get_MaxGridPosY>m__6(NonConvexMeshCollider.Vector3Int p)
		{
			return p.Y;
		}

		// Token: 0x060015B4 RID: 5556 RVA: 0x0007B3C0 File Offset: 0x000797C0
		[CompilerGenerated]
		private static int <get_MaxGridPosZ>m__7(NonConvexMeshCollider.Vector3Int p)
		{
			return p.Z;
		}

		// Token: 0x04001218 RID: 4632
		private readonly NonConvexMeshCollider.Box[,,] boxes;

		// Token: 0x04001219 RID: 4633
		private readonly NonConvexMeshCollider.Vector3Int lastLevelGridPos;

		// Token: 0x0400121A RID: 4634
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private NonConvexMeshCollider.Box <Parent>k__BackingField;

		// Token: 0x0400121B RID: 4635
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private NonConvexMeshCollider.Box[] <Children>k__BackingField;

		// Token: 0x0400121C RID: 4636
		private NonConvexMeshCollider.Box[] lastLevelBoxes;

		// Token: 0x0400121D RID: 4637
		private NonConvexMeshCollider.Vector3Int minGridPos;

		// Token: 0x0400121E RID: 4638
		private NonConvexMeshCollider.Vector3Int maxGridPos;

		// Token: 0x0400121F RID: 4639
		private NonConvexMeshCollider.Vector3Int gridSize;

		// Token: 0x04001220 RID: 4640
		private Vector3? center;

		// Token: 0x04001221 RID: 4641
		private Vector3? size;

		// Token: 0x04001222 RID: 4642
		[CompilerGenerated]
		private static Func<NonConvexMeshCollider.Box, bool> <>f__am$cache0;

		// Token: 0x04001223 RID: 4643
		[CompilerGenerated]
		private static Func<NonConvexMeshCollider.Box, NonConvexMeshCollider.Vector3Int> <>f__am$cache1;

		// Token: 0x04001224 RID: 4644
		[CompilerGenerated]
		private static Func<NonConvexMeshCollider.Vector3Int, int> <>f__am$cache2;

		// Token: 0x04001225 RID: 4645
		[CompilerGenerated]
		private static Func<NonConvexMeshCollider.Vector3Int, int> <>f__am$cache3;

		// Token: 0x04001226 RID: 4646
		[CompilerGenerated]
		private static Func<NonConvexMeshCollider.Vector3Int, int> <>f__am$cache4;

		// Token: 0x04001227 RID: 4647
		[CompilerGenerated]
		private static Func<NonConvexMeshCollider.Vector3Int, int> <>f__am$cache5;

		// Token: 0x04001228 RID: 4648
		[CompilerGenerated]
		private static Func<NonConvexMeshCollider.Vector3Int, int> <>f__am$cache6;

		// Token: 0x04001229 RID: 4649
		[CompilerGenerated]
		private static Func<NonConvexMeshCollider.Vector3Int, int> <>f__am$cache7;

		// Token: 0x02000F3A RID: 3898
		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<NonConvexMeshCollider.Box>, IEnumerator, IDisposable, IEnumerator<NonConvexMeshCollider.Box>
		{
			// Token: 0x0600731A RID: 29466 RVA: 0x0007B3C8 File Offset: 0x000797C8
			[DebuggerHidden]
			public <>c__Iterator0()
			{
			}

			// Token: 0x0600731B RID: 29467 RVA: 0x0007B3D0 File Offset: 0x000797D0
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					b = this;
					break;
				case 1U:
					b = b.Parent;
					break;
				default:
					return false;
				}
				if (b.Parent != null)
				{
					this.$current = b.Parent;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				this.$PC = -1;
				return false;
			}

			// Token: 0x170010D3 RID: 4307
			// (get) Token: 0x0600731C RID: 29468 RVA: 0x0007B45F File Offset: 0x0007985F
			NonConvexMeshCollider.Box IEnumerator<NonConvexMeshCollider.Box>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170010D4 RID: 4308
			// (get) Token: 0x0600731D RID: 29469 RVA: 0x0007B467 File Offset: 0x00079867
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x0600731E RID: 29470 RVA: 0x0007B46F File Offset: 0x0007986F
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x0600731F RID: 29471 RVA: 0x0007B47F File Offset: 0x0007987F
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x06007320 RID: 29472 RVA: 0x0007B486 File Offset: 0x00079886
			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<NonConvexMeshCollider.Box>.GetEnumerator();
			}

			// Token: 0x06007321 RID: 29473 RVA: 0x0007B490 File Offset: 0x00079890
			[DebuggerHidden]
			IEnumerator<NonConvexMeshCollider.Box> IEnumerable<NonConvexMeshCollider.Box>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				NonConvexMeshCollider.Box.<>c__Iterator0 <>c__Iterator = new NonConvexMeshCollider.Box.<>c__Iterator0();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}

			// Token: 0x04006708 RID: 26376
			internal NonConvexMeshCollider.Box <b>__0;

			// Token: 0x04006709 RID: 26377
			internal NonConvexMeshCollider.Box $this;

			// Token: 0x0400670A RID: 26378
			internal NonConvexMeshCollider.Box $current;

			// Token: 0x0400670B RID: 26379
			internal bool $disposing;

			// Token: 0x0400670C RID: 26380
			internal int $PC;
		}

		// Token: 0x02000F3B RID: 3899
		[CompilerGenerated]
		private sealed class <>c__Iterator1 : IEnumerable, IEnumerable<NonConvexMeshCollider.Box>, IEnumerator, IDisposable, IEnumerator<NonConvexMeshCollider.Box>
		{
			// Token: 0x06007322 RID: 29474 RVA: 0x0007B4C4 File Offset: 0x000798C4
			[DebuggerHidden]
			public <>c__Iterator1()
			{
			}

			// Token: 0x06007323 RID: 29475 RVA: 0x0007B4CC File Offset: 0x000798CC
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0U:
					this.$current = this;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1U:
					enumerator = base.Parents.GetEnumerator();
					num = 4294967293U;
					break;
				case 2U:
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
						parent = enumerator.Current;
						this.$current = parent;
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				this.$PC = -1;
				return false;
			}

			// Token: 0x170010D5 RID: 4309
			// (get) Token: 0x06007324 RID: 29476 RVA: 0x0007B5C8 File Offset: 0x000799C8
			NonConvexMeshCollider.Box IEnumerator<NonConvexMeshCollider.Box>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170010D6 RID: 4310
			// (get) Token: 0x06007325 RID: 29477 RVA: 0x0007B5D0 File Offset: 0x000799D0
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x06007326 RID: 29478 RVA: 0x0007B5D8 File Offset: 0x000799D8
			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 2U:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			// Token: 0x06007327 RID: 29479 RVA: 0x0007B640 File Offset: 0x00079A40
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x06007328 RID: 29480 RVA: 0x0007B647 File Offset: 0x00079A47
			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<NonConvexMeshCollider.Box>.GetEnumerator();
			}

			// Token: 0x06007329 RID: 29481 RVA: 0x0007B650 File Offset: 0x00079A50
			[DebuggerHidden]
			IEnumerator<NonConvexMeshCollider.Box> IEnumerable<NonConvexMeshCollider.Box>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				NonConvexMeshCollider.Box.<>c__Iterator1 <>c__Iterator = new NonConvexMeshCollider.Box.<>c__Iterator1();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}

			// Token: 0x0400670D RID: 26381
			internal IEnumerator<NonConvexMeshCollider.Box> $locvar0;

			// Token: 0x0400670E RID: 26382
			internal NonConvexMeshCollider.Box <parent>__1;

			// Token: 0x0400670F RID: 26383
			internal NonConvexMeshCollider.Box $this;

			// Token: 0x04006710 RID: 26384
			internal NonConvexMeshCollider.Box $current;

			// Token: 0x04006711 RID: 26385
			internal bool $disposing;

			// Token: 0x04006712 RID: 26386
			internal int $PC;
		}

		// Token: 0x02000F3C RID: 3900
		[CompilerGenerated]
		private sealed class <>c__Iterator2 : IEnumerable, IEnumerable<NonConvexMeshCollider.Box>, IEnumerator, IDisposable, IEnumerator<NonConvexMeshCollider.Box>
		{
			// Token: 0x0600732A RID: 29482 RVA: 0x0007B684 File Offset: 0x00079A84
			[DebuggerHidden]
			public <>c__Iterator2()
			{
			}

			// Token: 0x0600732B RID: 29483 RVA: 0x0007B68C File Offset: 0x00079A8C
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0U:
					if (base.Children == null)
					{
						return false;
					}
					children = base.Children;
					i = 0;
					goto IL_127;
				case 1U:
					enumerator = c.ChildrenRecursive.GetEnumerator();
					num = 4294967293U;
					break;
				case 2U:
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
						cc = enumerator.Current;
						this.$current = cc;
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				i++;
				IL_127:
				if (i < children.Length)
				{
					c = children[i];
					this.$current = c;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				this.$PC = -1;
				return false;
			}

			// Token: 0x170010D7 RID: 4311
			// (get) Token: 0x0600732C RID: 29484 RVA: 0x0007B7F0 File Offset: 0x00079BF0
			NonConvexMeshCollider.Box IEnumerator<NonConvexMeshCollider.Box>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170010D8 RID: 4312
			// (get) Token: 0x0600732D RID: 29485 RVA: 0x0007B7F8 File Offset: 0x00079BF8
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x0600732E RID: 29486 RVA: 0x0007B800 File Offset: 0x00079C00
			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 2U:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			// Token: 0x0600732F RID: 29487 RVA: 0x0007B868 File Offset: 0x00079C68
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x06007330 RID: 29488 RVA: 0x0007B86F File Offset: 0x00079C6F
			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<NonConvexMeshCollider.Box>.GetEnumerator();
			}

			// Token: 0x06007331 RID: 29489 RVA: 0x0007B878 File Offset: 0x00079C78
			[DebuggerHidden]
			IEnumerator<NonConvexMeshCollider.Box> IEnumerable<NonConvexMeshCollider.Box>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				NonConvexMeshCollider.Box.<>c__Iterator2 <>c__Iterator = new NonConvexMeshCollider.Box.<>c__Iterator2();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}

			// Token: 0x04006713 RID: 26387
			internal NonConvexMeshCollider.Box[] $locvar0;

			// Token: 0x04006714 RID: 26388
			internal int $locvar1;

			// Token: 0x04006715 RID: 26389
			internal NonConvexMeshCollider.Box <c>__1;

			// Token: 0x04006716 RID: 26390
			internal IEnumerator<NonConvexMeshCollider.Box> $locvar2;

			// Token: 0x04006717 RID: 26391
			internal NonConvexMeshCollider.Box <cc>__2;

			// Token: 0x04006718 RID: 26392
			internal NonConvexMeshCollider.Box $this;

			// Token: 0x04006719 RID: 26393
			internal NonConvexMeshCollider.Box $current;

			// Token: 0x0400671A RID: 26394
			internal bool $disposing;

			// Token: 0x0400671B RID: 26395
			internal int $PC;
		}

		// Token: 0x02000F3D RID: 3901
		[CompilerGenerated]
		private sealed class <>c__Iterator3 : IEnumerable, IEnumerable<NonConvexMeshCollider.Box>, IEnumerator, IDisposable, IEnumerator<NonConvexMeshCollider.Box>
		{
			// Token: 0x06007332 RID: 29490 RVA: 0x0007B8AC File Offset: 0x00079CAC
			[DebuggerHidden]
			public <>c__Iterator3()
			{
			}

			// Token: 0x06007333 RID: 29491 RVA: 0x0007B8B4 File Offset: 0x00079CB4
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0U:
					this.$current = this;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1U:
					enumerator = base.ChildrenRecursive.GetEnumerator();
					num = 4294967293U;
					break;
				case 2U:
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
						c = enumerator.Current;
						this.$current = c;
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				this.$PC = -1;
				return false;
			}

			// Token: 0x170010D9 RID: 4313
			// (get) Token: 0x06007334 RID: 29492 RVA: 0x0007B9B0 File Offset: 0x00079DB0
			NonConvexMeshCollider.Box IEnumerator<NonConvexMeshCollider.Box>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170010DA RID: 4314
			// (get) Token: 0x06007335 RID: 29493 RVA: 0x0007B9B8 File Offset: 0x00079DB8
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x06007336 RID: 29494 RVA: 0x0007B9C0 File Offset: 0x00079DC0
			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 2U:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			// Token: 0x06007337 RID: 29495 RVA: 0x0007BA28 File Offset: 0x00079E28
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x06007338 RID: 29496 RVA: 0x0007BA2F File Offset: 0x00079E2F
			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<NonConvexMeshCollider.Box>.GetEnumerator();
			}

			// Token: 0x06007339 RID: 29497 RVA: 0x0007BA38 File Offset: 0x00079E38
			[DebuggerHidden]
			IEnumerator<NonConvexMeshCollider.Box> IEnumerable<NonConvexMeshCollider.Box>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				NonConvexMeshCollider.Box.<>c__Iterator3 <>c__Iterator = new NonConvexMeshCollider.Box.<>c__Iterator3();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}

			// Token: 0x0400671C RID: 26396
			internal IEnumerator<NonConvexMeshCollider.Box> $locvar0;

			// Token: 0x0400671D RID: 26397
			internal NonConvexMeshCollider.Box <c>__1;

			// Token: 0x0400671E RID: 26398
			internal NonConvexMeshCollider.Box $this;

			// Token: 0x0400671F RID: 26399
			internal NonConvexMeshCollider.Box $current;

			// Token: 0x04006720 RID: 26400
			internal bool $disposing;

			// Token: 0x04006721 RID: 26401
			internal int $PC;
		}
	}

	// Token: 0x02000366 RID: 870
	public class Vector3Int
	{
		// Token: 0x060015B5 RID: 5557 RVA: 0x0007BA6C File Offset: 0x00079E6C
		public Vector3Int(int x, int y, int z)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x060015B6 RID: 5558 RVA: 0x0007BA89 File Offset: 0x00079E89
		// (set) Token: 0x060015B7 RID: 5559 RVA: 0x0007BA91 File Offset: 0x00079E91
		public int X
		{
			[CompilerGenerated]
			get
			{
				return this.<X>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<X>k__BackingField = value;
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x060015B8 RID: 5560 RVA: 0x0007BA9A File Offset: 0x00079E9A
		// (set) Token: 0x060015B9 RID: 5561 RVA: 0x0007BAA2 File Offset: 0x00079EA2
		public int Y
		{
			[CompilerGenerated]
			get
			{
				return this.<Y>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Y>k__BackingField = value;
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x060015BA RID: 5562 RVA: 0x0007BAAB File Offset: 0x00079EAB
		// (set) Token: 0x060015BB RID: 5563 RVA: 0x0007BAB3 File Offset: 0x00079EB3
		public int Z
		{
			[CompilerGenerated]
			get
			{
				return this.<Z>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Z>k__BackingField = value;
			}
		}

		// Token: 0x060015BC RID: 5564 RVA: 0x0007BABC File Offset: 0x00079EBC
		protected bool Equals(NonConvexMeshCollider.Vector3Int other)
		{
			return this.X == other.X && this.Y == other.Y && this.Z == other.Z;
		}

		// Token: 0x060015BD RID: 5565 RVA: 0x0007BAF1 File Offset: 0x00079EF1
		public override bool Equals(object obj)
		{
			return !object.ReferenceEquals(null, obj) && (object.ReferenceEquals(this, obj) || (obj.GetType() == base.GetType() && this.Equals((NonConvexMeshCollider.Vector3Int)obj)));
		}

		// Token: 0x060015BE RID: 5566 RVA: 0x0007BB30 File Offset: 0x00079F30
		public override int GetHashCode()
		{
			int num = this.X;
			num = (num * 397 ^ this.Y);
			return num * 397 ^ this.Z;
		}

		// Token: 0x060015BF RID: 5567 RVA: 0x0007BB63 File Offset: 0x00079F63
		public override string ToString()
		{
			return string.Format("X: {0}, Y: {1}, Z: {2}", this.X, this.Y, this.Z);
		}

		// Token: 0x060015C0 RID: 5568 RVA: 0x0007BB90 File Offset: 0x00079F90
		// Note: this type is marked as 'beforefieldinit'.
		static Vector3Int()
		{
		}

		// Token: 0x0400122A RID: 4650
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private int <X>k__BackingField;

		// Token: 0x0400122B RID: 4651
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private int <Y>k__BackingField;

		// Token: 0x0400122C RID: 4652
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private int <Z>k__BackingField;

		// Token: 0x0400122D RID: 4653
		public static readonly NonConvexMeshCollider.Vector3Int One = new NonConvexMeshCollider.Vector3Int(1, 1, 1);
	}

	// Token: 0x02000F37 RID: 3895
	[CompilerGenerated]
	private sealed class <CreateMeshIntersectingBoxes>c__Iterator0 : IEnumerable, IEnumerable<NonConvexMeshCollider.Box>, IEnumerator, IDisposable, IEnumerator<NonConvexMeshCollider.Box>
	{
		// Token: 0x06007308 RID: 29448 RVA: 0x0007BB9F File Offset: 0x00079F9F
		[DebuggerHidden]
		public <CreateMeshIntersectingBoxes>c__Iterator0()
		{
		}

		// Token: 0x06007309 RID: 29449 RVA: 0x0007BBA8 File Offset: 0x00079FA8
		public bool MoveNext()
		{
			uint num = (uint)this.$PC;
			this.$PC = -1;
			switch (num)
			{
			case 0U:
				<CreateMeshIntersectingBoxes>c__AnonStorey = new NonConvexMeshCollider.<CreateMeshIntersectingBoxes>c__Iterator0.<CreateMeshIntersectingBoxes>c__AnonStorey2();
				<CreateMeshIntersectingBoxes>c__AnonStorey.<>f__ref$0 = this;
				go = colliderGo.transform.parent.gameObject;
				colliderLayer = colliderGo.layer;
				colliderLayerMask = 1 << colliderLayer;
				bounds = NonConvexMeshCollider.CalculateLocalBounds(go);
				mesh = colliderGo.GetComponent<MeshFilter>().sharedMesh;
				swTree = Stopwatch.StartNew();
				<CreateMeshIntersectingBoxes>c__AnonStorey.tree = new NonConvexMeshCollider.SpatialBinaryTree(mesh, this.spatialTreeLevelDepth);
				swTree.Stop();
				if (this.outputTimeMeasurements)
				{
					UnityEngine.Debug.Log("SpatialTree Built in " + swTree.Elapsed);
				}
				boxes = new NonConvexMeshCollider.Box[this.boxesPerEdge, this.boxesPerEdge, this.boxesPerEdge];
				boxColliderPositions = new bool[this.boxesPerEdge, this.boxesPerEdge, this.boxesPerEdge];
				s = bounds.size / (float)this.boxesPerEdge;
				<CreateMeshIntersectingBoxes>c__AnonStorey.halfExtent = s / 2f;
				directionsFromBoxCenterToCorners = new Vector3[]
				{
					new Vector3(1f, 1f, 1f),
					new Vector3(1f, 1f, -1f),
					new Vector3(1f, -1f, 1f),
					new Vector3(1f, -1f, -1f),
					new Vector3(-1f, 1f, 1f),
					new Vector3(-1f, 1f, -1f),
					new Vector3(-1f, -1f, 1f),
					new Vector3(-1f, -1f, -1f)
				};
				<CreateMeshIntersectingBoxes>c__AnonStorey.pointInsideMeshCache = new Dictionary<Vector3, bool>();
				sw = Stopwatch.StartNew();
				colliders = new Collider[1000];
				for (int i = 0; i < this.boxesPerEdge; i++)
				{
					for (int j = 0; j < this.boxesPerEdge; j++)
					{
						for (int k = 0; k < this.boxesPerEdge; k++)
						{
							NonConvexMeshCollider.<CreateMeshIntersectingBoxes>c__Iterator0.<CreateMeshIntersectingBoxes>c__AnonStorey1 <CreateMeshIntersectingBoxes>c__AnonStorey2 = new NonConvexMeshCollider.<CreateMeshIntersectingBoxes>c__Iterator0.<CreateMeshIntersectingBoxes>c__AnonStorey1();
							<CreateMeshIntersectingBoxes>c__AnonStorey2.<>f__ref$0 = this;
							<CreateMeshIntersectingBoxes>c__AnonStorey2.<>f__ref$2 = <CreateMeshIntersectingBoxes>c__AnonStorey;
							<CreateMeshIntersectingBoxes>c__AnonStorey2.center = new Vector3(bounds.center.x - bounds.size.x / 2f + s.x * (float)i + <CreateMeshIntersectingBoxes>c__AnonStorey.halfExtent.x, bounds.center.y - bounds.size.y / 2f + s.y * (float)j + <CreateMeshIntersectingBoxes>c__AnonStorey.halfExtent.y, bounds.center.z - bounds.size.z / 2f + s.z * (float)k + <CreateMeshIntersectingBoxes>c__AnonStorey.halfExtent.z);
							if (!this.avoidExceedingMesh)
							{
								if (this.avoidGapsInside)
								{
									bool flag = base.IsInsideMesh(<CreateMeshIntersectingBoxes>c__AnonStorey2.center, <CreateMeshIntersectingBoxes>c__AnonStorey.tree, <CreateMeshIntersectingBoxes>c__AnonStorey.pointInsideMeshCache);
									boxColliderPositions[i, j, k] = flag;
								}
								else
								{
									bool flag2 = Physics.OverlapBoxNonAlloc(<CreateMeshIntersectingBoxes>c__AnonStorey2.center, <CreateMeshIntersectingBoxes>c__AnonStorey.halfExtent, colliders, Quaternion.identity, colliderLayerMask) > 0;
									boxColliderPositions[i, j, k] = flag2;
								}
							}
							else
							{
								bool flag3 = directionsFromBoxCenterToCorners.Select(new Func<Vector3, Vector3>(<CreateMeshIntersectingBoxes>c__AnonStorey2.<>m__0)).All(new Func<Vector3, bool>(<CreateMeshIntersectingBoxes>c__AnonStorey2.<>m__1));
								boxColliderPositions[i, j, k] = flag3;
							}
						}
					}
				}
				sw.Stop();
				if (this.outputTimeMeasurements)
				{
					UnityEngine.Debug.Log("Boxes analyzed in " + sw.Elapsed);
				}
				x = 0;
				goto IL_784;
			case 1U:
				IL_72E:
				z++;
				break;
			default:
				return false;
			}
			IL_73C:
			if (z >= this.boxesPerEdge)
			{
				y++;
			}
			else
			{
				if (!boxColliderPositions[x, y, z])
				{
					goto IL_72E;
				}
				center = new Vector3(bounds.center.x - bounds.size.x / 2f + s.x * (float)x + s.x / 2f, bounds.center.y - bounds.size.y / 2f + s.y * (float)y + s.y / 2f, bounds.center.z - bounds.size.z / 2f + s.z * (float)z + s.z / 2f);
				b = new NonConvexMeshCollider.Box(boxes, new Vector3?(center), new Vector3?(s), new NonConvexMeshCollider.Vector3Int(x, y, z));
				boxes[x, y, z] = b;
				this.$current = b;
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
			}
			IL_760:
			if (y < this.boxesPerEdge)
			{
				z = 0;
				goto IL_73C;
			}
			x++;
			IL_784:
			if (x < this.boxesPerEdge)
			{
				y = 0;
				goto IL_760;
			}
			this.$PC = -1;
			return false;
		}

		// Token: 0x170010CF RID: 4303
		// (get) Token: 0x0600730A RID: 29450 RVA: 0x0007C359 File Offset: 0x0007A759
		NonConvexMeshCollider.Box IEnumerator<NonConvexMeshCollider.Box>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x170010D0 RID: 4304
		// (get) Token: 0x0600730B RID: 29451 RVA: 0x0007C361 File Offset: 0x0007A761
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x0600730C RID: 29452 RVA: 0x0007C369 File Offset: 0x0007A769
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x0600730D RID: 29453 RVA: 0x0007C379 File Offset: 0x0007A779
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600730E RID: 29454 RVA: 0x0007C380 File Offset: 0x0007A780
		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.System.Collections.Generic.IEnumerable<NonConvexMeshCollider.Box>.GetEnumerator();
		}

		// Token: 0x0600730F RID: 29455 RVA: 0x0007C388 File Offset: 0x0007A788
		[DebuggerHidden]
		IEnumerator<NonConvexMeshCollider.Box> IEnumerable<NonConvexMeshCollider.Box>.GetEnumerator()
		{
			if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
			{
				return this;
			}
			NonConvexMeshCollider.<CreateMeshIntersectingBoxes>c__Iterator0 <CreateMeshIntersectingBoxes>c__Iterator = new NonConvexMeshCollider.<CreateMeshIntersectingBoxes>c__Iterator0();
			<CreateMeshIntersectingBoxes>c__Iterator.$this = this;
			<CreateMeshIntersectingBoxes>c__Iterator.colliderGo = colliderGo;
			return <CreateMeshIntersectingBoxes>c__Iterator;
		}

		// Token: 0x040066E5 RID: 26341
		internal GameObject colliderGo;

		// Token: 0x040066E6 RID: 26342
		internal GameObject <go>__0;

		// Token: 0x040066E7 RID: 26343
		internal int <colliderLayer>__0;

		// Token: 0x040066E8 RID: 26344
		internal LayerMask <colliderLayerMask>__0;

		// Token: 0x040066E9 RID: 26345
		internal Bounds <bounds>__0;

		// Token: 0x040066EA RID: 26346
		internal Mesh <mesh>__0;

		// Token: 0x040066EB RID: 26347
		internal Stopwatch <swTree>__0;

		// Token: 0x040066EC RID: 26348
		internal NonConvexMeshCollider.Box[,,] <boxes>__0;

		// Token: 0x040066ED RID: 26349
		internal bool[,,] <boxColliderPositions>__0;

		// Token: 0x040066EE RID: 26350
		internal Vector3 <s>__0;

		// Token: 0x040066EF RID: 26351
		internal Vector3[] <directionsFromBoxCenterToCorners>__0;

		// Token: 0x040066F0 RID: 26352
		internal Stopwatch <sw>__0;

		// Token: 0x040066F1 RID: 26353
		internal Collider[] <colliders>__0;

		// Token: 0x040066F2 RID: 26354
		internal int <x>__1;

		// Token: 0x040066F3 RID: 26355
		internal int <y>__2;

		// Token: 0x040066F4 RID: 26356
		internal int <z>__3;

		// Token: 0x040066F5 RID: 26357
		internal Vector3 <center>__4;

		// Token: 0x040066F6 RID: 26358
		internal NonConvexMeshCollider.Box <b>__4;

		// Token: 0x040066F7 RID: 26359
		internal NonConvexMeshCollider $this;

		// Token: 0x040066F8 RID: 26360
		internal NonConvexMeshCollider.Box $current;

		// Token: 0x040066F9 RID: 26361
		internal bool $disposing;

		// Token: 0x040066FA RID: 26362
		internal int $PC;

		// Token: 0x040066FB RID: 26363
		private NonConvexMeshCollider.<CreateMeshIntersectingBoxes>c__Iterator0.<CreateMeshIntersectingBoxes>c__AnonStorey2 $locvar0;

		// Token: 0x02000F3E RID: 3902
		private sealed class <CreateMeshIntersectingBoxes>c__AnonStorey2
		{
			// Token: 0x0600733A RID: 29498 RVA: 0x0007C3C8 File Offset: 0x0007A7C8
			public <CreateMeshIntersectingBoxes>c__AnonStorey2()
			{
			}

			// Token: 0x04006722 RID: 26402
			internal Vector3 halfExtent;

			// Token: 0x04006723 RID: 26403
			internal NonConvexMeshCollider.SpatialBinaryTree tree;

			// Token: 0x04006724 RID: 26404
			internal Dictionary<Vector3, bool> pointInsideMeshCache;

			// Token: 0x04006725 RID: 26405
			internal NonConvexMeshCollider.<CreateMeshIntersectingBoxes>c__Iterator0 <>f__ref$0;
		}

		// Token: 0x02000F3F RID: 3903
		private sealed class <CreateMeshIntersectingBoxes>c__AnonStorey1
		{
			// Token: 0x0600733B RID: 29499 RVA: 0x0007C3D0 File Offset: 0x0007A7D0
			public <CreateMeshIntersectingBoxes>c__AnonStorey1()
			{
			}

			// Token: 0x0600733C RID: 29500 RVA: 0x0007C3D8 File Offset: 0x0007A7D8
			internal Vector3 <>m__0(Vector3 d)
			{
				return new Vector3(this.center.x + this.<>f__ref$2.halfExtent.x * d.x, this.center.y + this.<>f__ref$2.halfExtent.y * d.y, this.center.z + this.<>f__ref$2.halfExtent.z * d.z);
			}

			// Token: 0x0600733D RID: 29501 RVA: 0x0007C456 File Offset: 0x0007A856
			internal bool <>m__1(Vector3 cornerPoint)
			{
				return this.<>f__ref$0.$this.IsInsideMesh(cornerPoint, this.<>f__ref$2.tree, this.<>f__ref$2.pointInsideMeshCache);
			}

			// Token: 0x04006726 RID: 26406
			internal Vector3 center;

			// Token: 0x04006727 RID: 26407
			internal NonConvexMeshCollider.<CreateMeshIntersectingBoxes>c__Iterator0 <>f__ref$0;

			// Token: 0x04006728 RID: 26408
			internal NonConvexMeshCollider.<CreateMeshIntersectingBoxes>c__Iterator0.<CreateMeshIntersectingBoxes>c__AnonStorey2 <>f__ref$2;
		}
	}

	// Token: 0x02000F38 RID: 3896
	[CompilerGenerated]
	private sealed class <IsInsideMesh>c__AnonStorey3
	{
		// Token: 0x06007310 RID: 29456 RVA: 0x0007C47F File Offset: 0x0007A87F
		public <IsInsideMesh>c__AnonStorey3()
		{
		}

		// Token: 0x06007311 RID: 29457 RVA: 0x0007C487 File Offset: 0x0007A887
		internal bool <>m__0(NonConvexMeshCollider.Tri t)
		{
			return t.Intersect(this.r);
		}

		// Token: 0x040066FC RID: 26364
		internal Ray r;
	}
}
