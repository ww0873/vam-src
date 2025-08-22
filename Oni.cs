using System;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x020003CA RID: 970
public static class Oni
{
	// Token: 0x0600189E RID: 6302 RVA: 0x0008B0E7 File Offset: 0x000894E7
	public static GCHandle PinMemory(object data)
	{
		return GCHandle.Alloc(data, GCHandleType.Pinned);
	}

	// Token: 0x0600189F RID: 6303 RVA: 0x0008B0F0 File Offset: 0x000894F0
	public static void UnpinMemory(GCHandle handle)
	{
		if (handle.IsAllocated)
		{
			handle.Free();
		}
	}

	// Token: 0x060018A0 RID: 6304
	[DllImport("libOni")]
	public static extern IntPtr CreateCollider();

	// Token: 0x060018A1 RID: 6305
	[DllImport("libOni")]
	public static extern void DestroyCollider(IntPtr collider);

	// Token: 0x060018A2 RID: 6306
	[DllImport("libOni")]
	public static extern IntPtr CreateShape(Oni.ShapeType shapeType);

	// Token: 0x060018A3 RID: 6307
	[DllImport("libOni")]
	public static extern void DestroyShape(IntPtr shape);

	// Token: 0x060018A4 RID: 6308
	[DllImport("libOni")]
	public static extern IntPtr CreateRigidbody();

	// Token: 0x060018A5 RID: 6309
	[DllImport("libOni")]
	public static extern void DestroyRigidbody(IntPtr rigidbody);

	// Token: 0x060018A6 RID: 6310
	[DllImport("libOni")]
	public static extern void UpdateCollider(IntPtr collider, ref Oni.Collider adaptor);

	// Token: 0x060018A7 RID: 6311
	[DllImport("libOni")]
	public static extern void UpdateShape(IntPtr shape, ref Oni.Shape adaptor);

	// Token: 0x060018A8 RID: 6312
	[DllImport("libOni")]
	public static extern void UpdateRigidbody(IntPtr rigidbody, ref Oni.Rigidbody adaptor);

	// Token: 0x060018A9 RID: 6313
	[DllImport("libOni")]
	public static extern void GetRigidbodyVelocity(IntPtr rigidbody, ref Oni.RigidbodyVelocities velocities);

	// Token: 0x060018AA RID: 6314
	[DllImport("libOni")]
	public static extern void SetColliderShape(IntPtr collider, IntPtr shape);

	// Token: 0x060018AB RID: 6315
	[DllImport("libOni")]
	public static extern void SetColliderRigidbody(IntPtr collider, IntPtr rigidbody);

	// Token: 0x060018AC RID: 6316
	[DllImport("libOni")]
	public static extern void SetColliderMaterial(IntPtr collider, IntPtr material);

	// Token: 0x060018AD RID: 6317
	[DllImport("libOni")]
	public static extern IntPtr CreateCollisionMaterial();

	// Token: 0x060018AE RID: 6318
	[DllImport("libOni")]
	public static extern void DestroyCollisionMaterial(IntPtr material);

	// Token: 0x060018AF RID: 6319
	[DllImport("libOni")]
	public static extern void UpdateCollisionMaterial(IntPtr material, ref Oni.CollisionMaterial adaptor);

	// Token: 0x060018B0 RID: 6320
	[DllImport("libOni")]
	public static extern IntPtr CreateSolver(int maxParticles, int maxNeighbours);

	// Token: 0x060018B1 RID: 6321
	[DllImport("libOni")]
	public static extern void DestroySolver(IntPtr solver);

	// Token: 0x060018B2 RID: 6322
	[DllImport("libOni")]
	public static extern void AddCollider(IntPtr solver, IntPtr collider);

	// Token: 0x060018B3 RID: 6323
	[DllImport("libOni")]
	public static extern void RemoveCollider(IntPtr solver, IntPtr collider);

	// Token: 0x060018B4 RID: 6324
	[DllImport("libOni")]
	public static extern void GetBounds(IntPtr solver, ref Vector3 min, ref Vector3 max);

	// Token: 0x060018B5 RID: 6325
	[DllImport("libOni")]
	public static extern int GetParticleGridSize(IntPtr solver);

	// Token: 0x060018B6 RID: 6326
	[DllImport("libOni")]
	public static extern void GetParticleGrid(IntPtr solver, Oni.GridCell[] cells);

	// Token: 0x060018B7 RID: 6327
	[DllImport("libOni")]
	public static extern void SetSolverParameters(IntPtr solver, ref Oni.SolverParameters parameters);

	// Token: 0x060018B8 RID: 6328
	[DllImport("libOni")]
	public static extern void GetSolverParameters(IntPtr solver, ref Oni.SolverParameters parameters);

	// Token: 0x060018B9 RID: 6329
	[DllImport("libOni")]
	public static extern int SetActiveParticles(IntPtr solver, int[] active, int num);

	// Token: 0x060018BA RID: 6330
	[DllImport("libOni")]
	public static extern void AddSimulationTime(IntPtr solver, float step_dt);

	// Token: 0x060018BB RID: 6331
	[DllImport("libOni")]
	public static extern void ResetSimulationTime(IntPtr solver);

	// Token: 0x060018BC RID: 6332
	[DllImport("libOni")]
	public static extern void UpdateSolver(IntPtr solver, float substep_dt);

	// Token: 0x060018BD RID: 6333
	[DllImport("libOni")]
	public static extern void ApplyPositionInterpolation(IntPtr solver, float substep_dt);

	// Token: 0x060018BE RID: 6334
	[DllImport("libOni")]
	public static extern void UpdateSkeletalAnimation(IntPtr solver);

	// Token: 0x060018BF RID: 6335
	[DllImport("libOni")]
	public static extern void GetConstraintsOrder(IntPtr solver, int[] order);

	// Token: 0x060018C0 RID: 6336
	[DllImport("libOni")]
	public static extern void SetConstraintsOrder(IntPtr solver, int[] order);

	// Token: 0x060018C1 RID: 6337
	[DllImport("libOni")]
	public static extern int GetConstraintCount(IntPtr solver, int type);

	// Token: 0x060018C2 RID: 6338
	[DllImport("libOni")]
	public static extern void GetActiveConstraintIndices(IntPtr solver, int[] indices, int num, int type);

	// Token: 0x060018C3 RID: 6339
	[DllImport("libOni")]
	public static extern int SetRenderableParticlePositions(IntPtr solver, Vector4[] positions, int num, int destOffset);

	// Token: 0x060018C4 RID: 6340
	[DllImport("libOni")]
	public static extern int GetRenderableParticlePositions(IntPtr solver, Vector4[] positions, int num, int sourceOffset);

	// Token: 0x060018C5 RID: 6341
	[DllImport("libOni")]
	public static extern int SetParticlePhases(IntPtr solver, int[] phases, int num, int destOffset);

	// Token: 0x060018C6 RID: 6342
	[DllImport("libOni")]
	public static extern int SetParticlePositions(IntPtr solver, Vector4[] positions, int num, int destOffset);

	// Token: 0x060018C7 RID: 6343
	[DllImport("libOni")]
	public static extern int GetParticlePositions(IntPtr solver, Vector4[] positions, int num, int sourceOffset);

	// Token: 0x060018C8 RID: 6344
	[DllImport("libOni")]
	public static extern int SetParticleInverseMasses(IntPtr solver, float[] invMasses, int num, int destOffset);

	// Token: 0x060018C9 RID: 6345
	[DllImport("libOni")]
	public static extern int SetParticleSolidRadii(IntPtr solver, float[] radii, int num, int destOffset);

	// Token: 0x060018CA RID: 6346
	[DllImport("libOni")]
	public static extern int SetParticleVelocities(IntPtr solver, Vector4[] velocities, int num, int destOffset);

	// Token: 0x060018CB RID: 6347
	[DllImport("libOni")]
	public static extern int GetParticleVelocities(IntPtr solver, Vector4[] velocities, int num, int sourceOffset);

	// Token: 0x060018CC RID: 6348
	[DllImport("libOni")]
	public static extern void AddParticleExternalForces(IntPtr solver, Vector4[] forces, int[] indices, int num);

	// Token: 0x060018CD RID: 6349
	[DllImport("libOni")]
	public static extern void AddParticleExternalForce(IntPtr solver, ref Vector4 force, int[] indices, int num);

	// Token: 0x060018CE RID: 6350
	[DllImport("libOni")]
	public static extern int GetParticleVorticities(IntPtr solver, Vector4[] vorticities, int num, int sourceOffset);

	// Token: 0x060018CF RID: 6351
	[DllImport("libOni")]
	public static extern int GetParticleDensities(IntPtr solver, float[] densities, int num, int sourceOffset);

	// Token: 0x060018D0 RID: 6352
	[DllImport("libOni")]
	public static extern int GetDeformableTriangleCount(IntPtr solver);

	// Token: 0x060018D1 RID: 6353
	[DllImport("libOni")]
	public static extern void SetDeformableTriangles(IntPtr solver, int[] indices, int num, int destOffset);

	// Token: 0x060018D2 RID: 6354
	[DllImport("libOni")]
	public static extern int RemoveDeformableTriangles(IntPtr solver, int num, int sourceOffset);

	// Token: 0x060018D3 RID: 6355
	[DllImport("libOni")]
	public static extern void SetConstraintGroupParameters(IntPtr solver, int type, ref Oni.ConstraintParameters parameters);

	// Token: 0x060018D4 RID: 6356
	[DllImport("libOni")]
	public static extern void GetConstraintGroupParameters(IntPtr solver, int type, ref Oni.ConstraintParameters parameters);

	// Token: 0x060018D5 RID: 6357
	[DllImport("libOni")]
	public static extern void SetCollisionMaterials(IntPtr solver, IntPtr[] materials, int[] indices, int num);

	// Token: 0x060018D6 RID: 6358
	[DllImport("libOni")]
	public static extern int SetRestPositions(IntPtr solver, Vector4[] positions, int num, int destOffset);

	// Token: 0x060018D7 RID: 6359
	[DllImport("libOni")]
	public static extern void SetFluidMaterials(IntPtr solver, Oni.FluidMaterial[] materials, int num, int destOffset);

	// Token: 0x060018D8 RID: 6360
	[DllImport("libOni")]
	public static extern int SetFluidMaterialIndices(IntPtr solver, int[] indices, int num, int destOffset);

	// Token: 0x060018D9 RID: 6361
	[DllImport("libOni")]
	public static extern IntPtr CreateDeformableMesh(IntPtr solver, IntPtr halfEdge, IntPtr skinConstraintBatch, [MarshalAs(UnmanagedType.LPArray, SizeConst = 16)] float[] worldToLocal, IntPtr particleIndices, int vertexCapacity, int vertexCount);

	// Token: 0x060018DA RID: 6362
	[DllImport("libOni")]
	public static extern void DestroyDeformableMesh(IntPtr solver, IntPtr mesh);

	// Token: 0x060018DB RID: 6363
	[DllImport("libOni")]
	public static extern bool TearDeformableMeshAtVertex(IntPtr mesh, int vertexIndex, ref Vector3 planePoint, ref Vector3 planeNormal, int[] updated_edges, ref int num_edges);

	// Token: 0x060018DC RID: 6364
	[DllImport("libOni")]
	public static extern void SetDeformableMeshTBNUpdate(IntPtr mesh, Oni.NormalsUpdate normalsUpdate, [MarshalAs(UnmanagedType.I1)] bool skinTangents);

	// Token: 0x060018DD RID: 6365
	[DllImport("libOni")]
	public static extern void SetDeformableMeshTransform(IntPtr mesh, [MarshalAs(UnmanagedType.LPArray, SizeConst = 16)] float[] worldToLocal);

	// Token: 0x060018DE RID: 6366
	[DllImport("libOni")]
	public static extern void SetDeformableMeshSkinMap(IntPtr mesh, IntPtr sourceMesh, IntPtr triangleSkinMap);

	// Token: 0x060018DF RID: 6367
	[DllImport("libOni")]
	public static extern void SetDeformableMeshParticleIndices(IntPtr mesh, IntPtr particleIndices);

	// Token: 0x060018E0 RID: 6368
	[DllImport("libOni")]
	public static extern void SetDeformableMeshData(IntPtr mesh, IntPtr triangles, IntPtr vertices, IntPtr normals, IntPtr tangents, IntPtr colors, IntPtr uv1, IntPtr uv2, IntPtr uv3, IntPtr uv4);

	// Token: 0x060018E1 RID: 6369
	[DllImport("libOni")]
	public static extern void SetDeformableMeshAnimationData(IntPtr mesh, float[] bindPoses, Oni.BoneWeights[] weights, int numBones);

	// Token: 0x060018E2 RID: 6370
	[DllImport("libOni")]
	public static extern void SetDeformableMeshBoneTransforms(IntPtr mesh, float[] boneTransforms);

	// Token: 0x060018E3 RID: 6371
	[DllImport("libOni")]
	public static extern void ForceDeformableMeshSkeletalSkinning(IntPtr mesh);

	// Token: 0x060018E4 RID: 6372
	[DllImport("libOni")]
	public static extern IntPtr CreateBatch(int type, [MarshalAs(UnmanagedType.I1)] bool cooked);

	// Token: 0x060018E5 RID: 6373
	[DllImport("libOni")]
	public static extern void DestroyBatch(IntPtr batch);

	// Token: 0x060018E6 RID: 6374
	[DllImport("libOni")]
	public static extern IntPtr AddBatch(IntPtr solver, IntPtr batch, [MarshalAs(UnmanagedType.I1)] bool sharesParticles);

	// Token: 0x060018E7 RID: 6375
	[DllImport("libOni")]
	public static extern void RemoveBatch(IntPtr solver, IntPtr batch);

	// Token: 0x060018E8 RID: 6376
	[DllImport("libOni")]
	public static extern bool EnableBatch(IntPtr batch, [MarshalAs(UnmanagedType.I1)] bool enabled);

	// Token: 0x060018E9 RID: 6377
	[DllImport("libOni")]
	public static extern int GetBatchConstraintCount(IntPtr batch);

	// Token: 0x060018EA RID: 6378
	[DllImport("libOni")]
	public static extern int GetBatchConstraintForces(IntPtr batch, float[] forces, int num, int destOffset);

	// Token: 0x060018EB RID: 6379
	[DllImport("libOni")]
	public static extern int GetBatchPhaseCount(IntPtr batch);

	// Token: 0x060018EC RID: 6380
	[DllImport("libOni")]
	public static extern void GetBatchPhaseSizes(IntPtr batch, int[] phaseSizes);

	// Token: 0x060018ED RID: 6381
	[DllImport("libOni")]
	public static extern void SetBatchPhaseSizes(IntPtr batch, int[] phaseSizes, int num);

	// Token: 0x060018EE RID: 6382
	[DllImport("libOni")]
	public static extern bool CookBatch(IntPtr batch);

	// Token: 0x060018EF RID: 6383
	[DllImport("libOni")]
	public static extern int SetActiveConstraints(IntPtr batch, int[] active, int num);

	// Token: 0x060018F0 RID: 6384
	[DllImport("libOni")]
	public static extern void SetDistanceConstraints(IntPtr batch, int[] indices, float[] restLengths, Vector2[] stiffnesses, int num);

	// Token: 0x060018F1 RID: 6385
	[DllImport("libOni")]
	public static extern void GetDistanceConstraints(IntPtr batch, int[] indices, float[] restLengths, Vector2[] stiffnesses);

	// Token: 0x060018F2 RID: 6386
	[DllImport("libOni")]
	public static extern void SetBendingConstraints(IntPtr batch, int[] indices, float[] restBends, Vector2[] bendingStiffnesses, int num);

	// Token: 0x060018F3 RID: 6387
	[DllImport("libOni")]
	public static extern void GetBendingConstraints(IntPtr batch, int[] indices, float[] restBends, Vector2[] bendingStiffnesses);

	// Token: 0x060018F4 RID: 6388
	[DllImport("libOni")]
	public static extern void SetSkinConstraints(IntPtr batch, int[] indices, Vector4[] points, Vector4[] normals, float[] radiiBackstops, float[] stiffnesses, int num);

	// Token: 0x060018F5 RID: 6389
	[DllImport("libOni")]
	public static extern void GetSkinConstraints(IntPtr batch, int[] indices, Vector4[] points, Vector4[] normals, float[] radiiBackstops, float[] stiffnesses);

	// Token: 0x060018F6 RID: 6390
	[DllImport("libOni")]
	public static extern void SetAerodynamicConstraints(IntPtr batch, int[] particleIndices, float[] aerodynamicCoeffs, int num);

	// Token: 0x060018F7 RID: 6391
	[DllImport("libOni")]
	public static extern void SetVolumeConstraints(IntPtr batch, int[] triangleIndices, int[] firstTriangle, int[] numTriangles, float[] restVolumes, Vector2[] pressureStiffnesses, int num);

	// Token: 0x060018F8 RID: 6392
	[DllImport("libOni")]
	public static extern void SetTetherConstraints(IntPtr batch, int[] indices, Vector2[] maxLenghtsScales, float[] stiffnesses, int num);

	// Token: 0x060018F9 RID: 6393
	[DllImport("libOni")]
	public static extern void GetTetherConstraints(IntPtr batch, int[] indices, Vector2[] maxLenghtsScales, float[] stiffnesses);

	// Token: 0x060018FA RID: 6394
	[DllImport("libOni")]
	public static extern void SetPinConstraints(IntPtr batch, int[] indices, Vector4[] pinOffsets, IntPtr[] colliders, float[] stiffnesses, int num);

	// Token: 0x060018FB RID: 6395
	[DllImport("libOni")]
	public static extern void SetStitchConstraints(IntPtr batch, int[] indices, float[] stiffnesses, int num);

	// Token: 0x060018FC RID: 6396
	[DllImport("libOni")]
	public static extern void GetCollisionContacts(IntPtr solver, Oni.Contact[] contacts, int n);

	// Token: 0x060018FD RID: 6397
	[DllImport("libOni")]
	public static extern void ClearDiffuseParticles(IntPtr solver);

	// Token: 0x060018FE RID: 6398
	[DllImport("libOni")]
	public static extern int SetDiffuseParticles(IntPtr solver, Vector4[] positions, int num);

	// Token: 0x060018FF RID: 6399
	[DllImport("libOni")]
	public static extern int GetDiffuseParticleVelocities(IntPtr solver, Vector4[] velocities, int num, int sourceOffset);

	// Token: 0x06001900 RID: 6400
	[DllImport("libOni")]
	public static extern void SetDiffuseParticleNeighbourCounts(IntPtr solver, IntPtr neighbourCounts);

	// Token: 0x06001901 RID: 6401
	[DllImport("libOni")]
	public static extern IntPtr CreateHalfEdgeMesh();

	// Token: 0x06001902 RID: 6402
	[DllImport("libOni")]
	public static extern void DestroyHalfEdgeMesh(IntPtr mesh);

	// Token: 0x06001903 RID: 6403
	[DllImport("libOni")]
	public static extern void SetVertices(IntPtr mesh, IntPtr vertices, int n);

	// Token: 0x06001904 RID: 6404
	[DllImport("libOni")]
	public static extern void SetHalfEdges(IntPtr mesh, IntPtr halfedges, int n);

	// Token: 0x06001905 RID: 6405
	[DllImport("libOni")]
	public static extern void SetFaces(IntPtr mesh, IntPtr faces, int n);

	// Token: 0x06001906 RID: 6406
	[DllImport("libOni")]
	public static extern void SetNormals(IntPtr mesh, IntPtr normals);

	// Token: 0x06001907 RID: 6407
	[DllImport("libOni")]
	public static extern void SetTangents(IntPtr mesh, IntPtr tangents);

	// Token: 0x06001908 RID: 6408
	[DllImport("libOni")]
	public static extern void SetInverseOrientations(IntPtr mesh, IntPtr orientations);

	// Token: 0x06001909 RID: 6409
	[DllImport("libOni")]
	public static extern void SetVisualMap(IntPtr mesh, IntPtr map);

	// Token: 0x0600190A RID: 6410
	[DllImport("libOni")]
	public static extern int GetVertexCount(IntPtr mesh);

	// Token: 0x0600190B RID: 6411
	[DllImport("libOni")]
	public static extern int GetHalfEdgeCount(IntPtr mesh);

	// Token: 0x0600190C RID: 6412
	[DllImport("libOni")]
	public static extern int GetFaceCount(IntPtr mesh);

	// Token: 0x0600190D RID: 6413
	[DllImport("libOni")]
	public static extern int GetHalfEdgeMeshInfo(IntPtr mesh, ref Oni.MeshInformation meshInfo);

	// Token: 0x0600190E RID: 6414
	[DllImport("libOni")]
	public static extern void CalculatePrimitiveCounts(IntPtr mesh, Vector3[] vertices, int[] triangles, int vertexCount, int triangleCount);

	// Token: 0x0600190F RID: 6415
	[DllImport("libOni")]
	public static extern void Generate(IntPtr mesh, Vector3[] vertices, int[] triangles, int vertexCount, int triangleCount, ref Vector3 scale);

	// Token: 0x06001910 RID: 6416
	[DllImport("libOni")]
	public static extern int MakePhase(int group, Oni.ParticlePhase flags);

	// Token: 0x06001911 RID: 6417
	[DllImport("libOni")]
	public static extern int GetGroupFromPhase(int phase);

	// Token: 0x06001912 RID: 6418
	[DllImport("libOni")]
	public static extern float BendingConstraintRest(float[] constraintCoordinates);

	// Token: 0x06001913 RID: 6419
	[DllImport("libOni")]
	public static extern IntPtr CreateTriangleSkinMap();

	// Token: 0x06001914 RID: 6420
	[DllImport("libOni")]
	public static extern void DestroyTriangleSkinMap(IntPtr skinmap);

	// Token: 0x06001915 RID: 6421
	[DllImport("libOni")]
	public static extern void Bind(IntPtr skinmap, IntPtr sourcemesh, IntPtr targetmesh, uint[] sourceMasterFlags, uint[] targetSlaveFlags);

	// Token: 0x06001916 RID: 6422
	[DllImport("libOni")]
	public static extern int GetSkinnedVertexCount(IntPtr skinmap);

	// Token: 0x06001917 RID: 6423
	[DllImport("libOni")]
	public static extern void GetSkinInfo(IntPtr skinmap, int[] skinIndices, int[] sourceTriIndices, Vector3[] baryPositions, Vector3[] baryNormals, Vector3[] baryTangents);

	// Token: 0x06001918 RID: 6424
	[DllImport("libOni")]
	public static extern void SetSkinInfo(IntPtr skinmap, int[] skinIndices, int[] sourceTriIndices, Vector3[] baryPositions, Vector3[] baryNormals, Vector3[] baryTangents, int num);

	// Token: 0x06001919 RID: 6425
	[DllImport("libOni")]
	public static extern void WaitForAllTasks();

	// Token: 0x0600191A RID: 6426
	[DllImport("libOni")]
	public static extern void ClearTasks();

	// Token: 0x0600191B RID: 6427
	[DllImport("libOni")]
	public static extern int GetMaxSystemConcurrency();

	// Token: 0x0600191C RID: 6428
	[DllImport("libOni")]
	public static extern void SignalFrameStart();

	// Token: 0x0600191D RID: 6429
	[DllImport("libOni")]
	public static extern double SignalFrameEnd();

	// Token: 0x0600191E RID: 6430
	[DllImport("libOni")]
	public static extern void EnableProfiler([MarshalAs(UnmanagedType.I1)] bool cooked);

	// Token: 0x0600191F RID: 6431
	[DllImport("libOni")]
	public static extern int GetProfilingInfoCount();

	// Token: 0x06001920 RID: 6432
	[DllImport("libOni")]
	public static extern void GetProfilingInfo([Out] Oni.ProfileInfo[] info, int num);

	// Token: 0x020003CB RID: 971
	public enum ConstraintType
	{
		// Token: 0x040013FF RID: 5119
		Tether,
		// Token: 0x04001400 RID: 5120
		Pin,
		// Token: 0x04001401 RID: 5121
		Volume,
		// Token: 0x04001402 RID: 5122
		Bending,
		// Token: 0x04001403 RID: 5123
		Distance,
		// Token: 0x04001404 RID: 5124
		ParticleCollision,
		// Token: 0x04001405 RID: 5125
		Density,
		// Token: 0x04001406 RID: 5126
		Collision,
		// Token: 0x04001407 RID: 5127
		Skin,
		// Token: 0x04001408 RID: 5128
		Aerodynamics,
		// Token: 0x04001409 RID: 5129
		Stitch,
		// Token: 0x0400140A RID: 5130
		ShapeMatching
	}

	// Token: 0x020003CC RID: 972
	public enum ParticlePhase
	{
		// Token: 0x0400140C RID: 5132
		SelfCollide = 16777216,
		// Token: 0x0400140D RID: 5133
		Fluid = 33554432
	}

	// Token: 0x020003CD RID: 973
	public enum ShapeType
	{
		// Token: 0x0400140F RID: 5135
		Sphere,
		// Token: 0x04001410 RID: 5136
		Box,
		// Token: 0x04001411 RID: 5137
		Capsule,
		// Token: 0x04001412 RID: 5138
		Heightmap,
		// Token: 0x04001413 RID: 5139
		TriangleMesh,
		// Token: 0x04001414 RID: 5140
		EdgeMesh
	}

	// Token: 0x020003CE RID: 974
	public enum MaterialCombineMode
	{
		// Token: 0x04001416 RID: 5142
		Average,
		// Token: 0x04001417 RID: 5143
		Minimium,
		// Token: 0x04001418 RID: 5144
		Multiply,
		// Token: 0x04001419 RID: 5145
		Maximum
	}

	// Token: 0x020003CF RID: 975
	public enum NormalsUpdate
	{
		// Token: 0x0400141B RID: 5147
		Recalculate,
		// Token: 0x0400141C RID: 5148
		Skin
	}

	// Token: 0x020003D0 RID: 976
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct ProfileInfo
	{
		// Token: 0x0400141D RID: 5149
		public double start;

		// Token: 0x0400141E RID: 5150
		public double end;

		// Token: 0x0400141F RID: 5151
		public double childDuration;

		// Token: 0x04001420 RID: 5152
		public int threadID;

		// Token: 0x04001421 RID: 5153
		public int level;

		// Token: 0x04001422 RID: 5154
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string name;
	}

	// Token: 0x020003D1 RID: 977
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct GridCell
	{
		// Token: 0x04001423 RID: 5155
		public Vector3 center;

		// Token: 0x04001424 RID: 5156
		public Vector3 size;

		// Token: 0x04001425 RID: 5157
		public int count;
	}

	// Token: 0x020003D2 RID: 978
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct SolverParameters
	{
		// Token: 0x06001921 RID: 6433 RVA: 0x0008B108 File Offset: 0x00089508
		public SolverParameters(Oni.SolverParameters.Interpolation interpolation, Vector4 gravity)
		{
			this.mode = Oni.SolverParameters.Mode.Mode3D;
			this.gravity = gravity;
			this.interpolation = interpolation;
			this.damping = 0f;
			this.fluidDenoising = 0f;
			this.advectionRadius = 0.5f;
			this.sleepThreshold = 0.001f;
		}

		// Token: 0x04001426 RID: 5158
		[Tooltip("In 2D mode, particles are simulated on the XY plane only. For use in conjunction with Unity's 2D mode.")]
		public Oni.SolverParameters.Mode mode;

		// Token: 0x04001427 RID: 5159
		[Tooltip("Same as Rigidbody.interpolation. Set to INTERPOLATE for cloth that is applied on a main character or closely followed by a camera. NONE for everything else.")]
		public Oni.SolverParameters.Interpolation interpolation;

		// Token: 0x04001428 RID: 5160
		public Vector3 gravity;

		// Token: 0x04001429 RID: 5161
		[Tooltip("Percentage of velocity lost per second, between 0% (0) and 100% (1).")]
		[Range(0f, 1f)]
		public float damping;

		// Token: 0x0400142A RID: 5162
		[Tooltip("Intensity of laplacian smoothing applied to fluids. High values yield more uniform fluid particle distributions.")]
		[Range(0f, 1f)]
		public float fluidDenoising;

		// Token: 0x0400142B RID: 5163
		[Tooltip("Radius of diffuse particle advection. Large values yield better quality but are more expensive.")]
		public float advectionRadius;

		// Token: 0x0400142C RID: 5164
		[Tooltip("Kinetic energy below which particle positions arent updated. Energy values are mass-normalized, so all particles in the solver have the same threshold.")]
		public float sleepThreshold;

		// Token: 0x020003D3 RID: 979
		public enum Interpolation
		{
			// Token: 0x0400142E RID: 5166
			None,
			// Token: 0x0400142F RID: 5167
			Interpolate
		}

		// Token: 0x020003D4 RID: 980
		public enum Mode
		{
			// Token: 0x04001431 RID: 5169
			Mode3D,
			// Token: 0x04001432 RID: 5170
			Mode2D
		}
	}

	// Token: 0x020003D5 RID: 981
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct ConstraintParameters
	{
		// Token: 0x06001922 RID: 6434 RVA: 0x0008B15B File Offset: 0x0008955B
		public ConstraintParameters(bool enabled, Oni.ConstraintParameters.EvaluationOrder order, int iterations)
		{
			this.enabled = enabled;
			this.iterations = iterations;
			this.evaluationOrder = order;
			this.SORFactor = 1f;
		}

		// Token: 0x04001433 RID: 5171
		[Tooltip("Whether this constraint group is solved or not.")]
		[MarshalAs(UnmanagedType.I1)]
		public bool enabled;

		// Token: 0x04001434 RID: 5172
		[Tooltip("Order in which constraints are evaluated. SEQUENTIAL converges faster but is not very stable. PARALLEL is very stable but converges slowly, requiring more iterations to achieve the same result.")]
		public Oni.ConstraintParameters.EvaluationOrder evaluationOrder;

		// Token: 0x04001435 RID: 5173
		[Tooltip("Number of relaxation iterations performed by the constraint solver. A low number of iterations will perform better, but be less accurate.")]
		public int iterations;

		// Token: 0x04001436 RID: 5174
		[Tooltip("Over (or under if < 1) relaxation factor used. At 1, no overrelaxation is performed. At 2, constraints double their relaxation rate. High values reduce stability but improve convergence.")]
		[Range(0.1f, 2f)]
		public float SORFactor;

		// Token: 0x020003D6 RID: 982
		public enum EvaluationOrder
		{
			// Token: 0x04001438 RID: 5176
			Sequential,
			// Token: 0x04001439 RID: 5177
			Parallel
		}
	}

	// Token: 0x020003D7 RID: 983
	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 112)]
	public struct Contact
	{
		// Token: 0x0400143A RID: 5178
		public Vector4 point;

		// Token: 0x0400143B RID: 5179
		public Vector4 normal;

		// Token: 0x0400143C RID: 5180
		public Vector4 tangent;

		// Token: 0x0400143D RID: 5181
		public Vector4 bitangent;

		// Token: 0x0400143E RID: 5182
		public float distance;

		// Token: 0x0400143F RID: 5183
		public float normalImpulse;

		// Token: 0x04001440 RID: 5184
		public float tangentImpulse;

		// Token: 0x04001441 RID: 5185
		public float bitangentImpulse;

		// Token: 0x04001442 RID: 5186
		public float stickImpulse;

		// Token: 0x04001443 RID: 5187
		public int particle;

		// Token: 0x04001444 RID: 5188
		public int other;
	}

	// Token: 0x020003D8 RID: 984
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct BoneWeights
	{
		// Token: 0x06001923 RID: 6435 RVA: 0x0008B180 File Offset: 0x00089580
		public BoneWeights(BoneWeight weight)
		{
			this.bone0 = weight.boneIndex0;
			this.bone1 = weight.boneIndex1;
			this.bone2 = weight.boneIndex2;
			this.bone3 = weight.boneIndex3;
			this.weight0 = weight.weight0;
			this.weight1 = weight.weight1;
			this.weight2 = weight.weight2;
			this.weight3 = weight.weight3;
		}

		// Token: 0x04001445 RID: 5189
		public int bone0;

		// Token: 0x04001446 RID: 5190
		public int bone1;

		// Token: 0x04001447 RID: 5191
		public int bone2;

		// Token: 0x04001448 RID: 5192
		public int bone3;

		// Token: 0x04001449 RID: 5193
		public float weight0;

		// Token: 0x0400144A RID: 5194
		public float weight1;

		// Token: 0x0400144B RID: 5195
		public float weight2;

		// Token: 0x0400144C RID: 5196
		public float weight3;
	}

	// Token: 0x020003D9 RID: 985
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct Rigidbody
	{
		// Token: 0x06001924 RID: 6436 RVA: 0x0008B1F8 File Offset: 0x000895F8
		public void Set(UnityEngine.Rigidbody source, bool kinematicForParticles)
		{
			bool flag = source.isKinematic || kinematicForParticles;
			this.rotation = source.rotation;
			this.linearVelocity = ((!kinematicForParticles) ? source.velocity : Vector3.zero);
			this.angularVelocity = ((!kinematicForParticles) ? source.angularVelocity : Vector3.zero);
			this.centerOfMass = source.transform.position + source.transform.rotation * source.centerOfMass;
			Vector3 point = new Vector3(((source.constraints & RigidbodyConstraints.FreezeRotationX) == RigidbodyConstraints.None) ? (1f / source.inertiaTensor.x) : 0f, ((source.constraints & RigidbodyConstraints.FreezeRotationY) == RigidbodyConstraints.None) ? (1f / source.inertiaTensor.y) : 0f, ((source.constraints & RigidbodyConstraints.FreezeRotationZ) == RigidbodyConstraints.None) ? (1f / source.inertiaTensor.z) : 0f);
			this.inertiaTensor = ((!flag) ? (source.inertiaTensorRotation * point) : Vector3.zero);
			this.inverseMass = ((!flag) ? (1f / source.mass) : 0f);
		}

		// Token: 0x06001925 RID: 6437 RVA: 0x0008B358 File Offset: 0x00089758
		public void Set(Rigidbody2D source, bool kinematicForParticles)
		{
			this.rotation = Quaternion.AngleAxis(source.rotation, Vector3.forward);
			this.linearVelocity = source.velocity;
			this.angularVelocity = new Vector4(0f, 0f, source.angularVelocity * 0.017453292f, 0f);
			this.centerOfMass = source.transform.position + source.transform.rotation * source.centerOfMass;
			this.inertiaTensor = ((!source.isKinematic && !kinematicForParticles) ? new Vector3(0f, 0f, ((source.constraints & RigidbodyConstraints2D.FreezeRotation) == RigidbodyConstraints2D.None) ? (1f / source.inertia) : 0f) : Vector3.zero);
			this.inverseMass = ((!source.isKinematic && !kinematicForParticles) ? (1f / source.mass) : 0f);
		}

		// Token: 0x0400144D RID: 5197
		public Quaternion rotation;

		// Token: 0x0400144E RID: 5198
		public Vector3 linearVelocity;

		// Token: 0x0400144F RID: 5199
		public Vector3 angularVelocity;

		// Token: 0x04001450 RID: 5200
		public Vector3 centerOfMass;

		// Token: 0x04001451 RID: 5201
		public Vector3 inertiaTensor;

		// Token: 0x04001452 RID: 5202
		public float inverseMass;
	}

	// Token: 0x020003DA RID: 986
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct RigidbodyVelocities
	{
		// Token: 0x04001453 RID: 5203
		public Vector3 linearVelocity;

		// Token: 0x04001454 RID: 5204
		public Vector3 angularVelocity;
	}

	// Token: 0x020003DB RID: 987
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct Collider
	{
		// Token: 0x06001926 RID: 6438 RVA: 0x0008B468 File Offset: 0x00089868
		public void Set(UnityEngine.Collider source, int phase, float thickness)
		{
			this.boundsMin = source.bounds.min - Vector3.one * (thickness + source.contactOffset);
			this.boundsMax = source.bounds.max + Vector3.one * (thickness + source.contactOffset);
			this.translation = source.transform.position;
			this.rotation = source.transform.rotation;
			this.scale = new Vector4(source.transform.lossyScale.x, source.transform.lossyScale.y, source.transform.lossyScale.z, 1f);
			this.contactOffset = thickness;
			this.collisionGroup = phase;
			this.trigger = source.isTrigger;
			this.id = source.GetInstanceID();
		}

		// Token: 0x06001927 RID: 6439 RVA: 0x0008B564 File Offset: 0x00089964
		public void Set(Collider2D source, int phase, float thickness)
		{
			this.boundsMin = source.bounds.min - Vector3.one * (thickness + 0.01f);
			this.boundsMax = source.bounds.max + Vector3.one * (thickness + 0.01f);
			this.translation = source.transform.position;
			this.rotation = source.transform.rotation;
			this.scale = new Vector4(source.transform.lossyScale.x, source.transform.lossyScale.y, source.transform.lossyScale.z, 1f);
			this.contactOffset = thickness;
			this.collisionGroup = phase;
			this.trigger = source.isTrigger;
			this.id = source.GetInstanceID();
		}

		// Token: 0x06001928 RID: 6440 RVA: 0x0008B65C File Offset: 0x00089A5C
		public void SetSpaceTransform(Transform space)
		{
			Matrix4x4 worldToLocalMatrix = space.worldToLocalMatrix;
			Vector4 v = worldToLocalMatrix.GetColumn(0) * this.boundsMin.x;
			Vector4 v2 = worldToLocalMatrix.GetColumn(0) * this.boundsMax.x;
			Vector4 v3 = worldToLocalMatrix.GetColumn(1) * this.boundsMin.y;
			Vector4 v4 = worldToLocalMatrix.GetColumn(1) * this.boundsMax.y;
			Vector4 v5 = worldToLocalMatrix.GetColumn(2) * this.boundsMin.z;
			Vector4 v6 = worldToLocalMatrix.GetColumn(2) * this.boundsMax.z;
			Vector3 b = worldToLocalMatrix.GetColumn(3);
			this.boundsMin = Vector3.Min(v, v2) + Vector3.Min(v3, v4) + Vector3.Min(v5, v6) + b;
			this.boundsMax = Vector3.Max(v, v2) + Vector3.Max(v3, v4) + Vector3.Max(v5, v6) + b;
			this.translation = space.worldToLocalMatrix.MultiplyPoint3x4(this.translation);
			this.rotation = Quaternion.Inverse(space.rotation) * this.rotation;
			this.scale.Scale(new Vector4(1f / space.lossyScale.x, 1f / space.lossyScale.y, 1f / space.lossyScale.z, 1f));
		}

		// Token: 0x04001455 RID: 5205
		public Quaternion rotation;

		// Token: 0x04001456 RID: 5206
		public Vector3 translation;

		// Token: 0x04001457 RID: 5207
		public Vector3 scale;

		// Token: 0x04001458 RID: 5208
		public Vector3 boundsMin;

		// Token: 0x04001459 RID: 5209
		public Vector3 boundsMax;

		// Token: 0x0400145A RID: 5210
		public int id;

		// Token: 0x0400145B RID: 5211
		public float contactOffset;

		// Token: 0x0400145C RID: 5212
		public int collisionGroup;

		// Token: 0x0400145D RID: 5213
		[MarshalAs(UnmanagedType.I1)]
		public bool trigger;
	}

	// Token: 0x020003DC RID: 988
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct Shape
	{
		// Token: 0x06001929 RID: 6441 RVA: 0x0008B842 File Offset: 0x00089C42
		public void Set(Vector3 center, float radius)
		{
			this.center = center;
			this.size = Vector3.one * radius;
		}

		// Token: 0x0600192A RID: 6442 RVA: 0x0008B85C File Offset: 0x00089C5C
		public void Set(Vector3 center, Vector3 size)
		{
			this.center = center;
			this.size = size;
		}

		// Token: 0x0600192B RID: 6443 RVA: 0x0008B86C File Offset: 0x00089C6C
		public void Set(Vector3 center, float radius, float height, int direction)
		{
			this.center = center;
			this.size = new Vector3(radius, height, (float)direction);
		}

		// Token: 0x0600192C RID: 6444 RVA: 0x0008B885 File Offset: 0x00089C85
		public void Set(Vector3 size, int resolutionU, int resolutionV, IntPtr data)
		{
			this.size = size;
			this.resolutionU = resolutionU;
			this.resolutionV = resolutionV;
			this.data = data;
			this.dataCount = resolutionU * resolutionV;
		}

		// Token: 0x0600192D RID: 6445 RVA: 0x0008B8AD File Offset: 0x00089CAD
		public void Set(IntPtr data, IntPtr indices, int dataCount, int indicesCount)
		{
			this.data = data;
			this.indices = indices;
			this.dataCount = dataCount;
			this.indexCount = indicesCount;
		}

		// Token: 0x0400145E RID: 5214
		private Vector3 center;

		// Token: 0x0400145F RID: 5215
		private Vector3 size;

		// Token: 0x04001460 RID: 5216
		private IntPtr data;

		// Token: 0x04001461 RID: 5217
		private IntPtr indices;

		// Token: 0x04001462 RID: 5218
		private int dataCount;

		// Token: 0x04001463 RID: 5219
		private int indexCount;

		// Token: 0x04001464 RID: 5220
		private int resolutionU;

		// Token: 0x04001465 RID: 5221
		private int resolutionV;

		// Token: 0x04001466 RID: 5222
		[MarshalAs(UnmanagedType.I1)]
		public bool is2D;
	}

	// Token: 0x020003DD RID: 989
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct CollisionMaterial
	{
		// Token: 0x04001467 RID: 5223
		public float friction;

		// Token: 0x04001468 RID: 5224
		public float stickiness;

		// Token: 0x04001469 RID: 5225
		public float stickDistance;

		// Token: 0x0400146A RID: 5226
		public Oni.MaterialCombineMode frictionCombine;

		// Token: 0x0400146B RID: 5227
		public Oni.MaterialCombineMode stickinessCombine;
	}

	// Token: 0x020003DE RID: 990
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct FluidMaterial
	{
		// Token: 0x0400146C RID: 5228
		public float smoothingRadius;

		// Token: 0x0400146D RID: 5229
		public float restDensity;

		// Token: 0x0400146E RID: 5230
		public float viscosity;

		// Token: 0x0400146F RID: 5231
		public float surfaceTension;

		// Token: 0x04001470 RID: 5232
		public float buoyancy;

		// Token: 0x04001471 RID: 5233
		public float atmosphericDrag;

		// Token: 0x04001472 RID: 5234
		public float atmosphericPressure;

		// Token: 0x04001473 RID: 5235
		public float vorticity;

		// Token: 0x04001474 RID: 5236
		public float elasticRange;

		// Token: 0x04001475 RID: 5237
		public float plasticCreep;

		// Token: 0x04001476 RID: 5238
		public float plasticThreshold;
	}

	// Token: 0x020003DF RID: 991
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct HalfEdge
	{
		// Token: 0x0600192E RID: 6446 RVA: 0x0008B8CC File Offset: 0x00089CCC
		public HalfEdge(int index)
		{
			this.index = index;
			this.indexInFace = -1;
			this.face = -1;
			this.nextHalfEdge = -1;
			this.pair = -1;
			this.endVertex = -1;
		}

		// Token: 0x04001477 RID: 5239
		public int index;

		// Token: 0x04001478 RID: 5240
		public int indexInFace;

		// Token: 0x04001479 RID: 5241
		public int face;

		// Token: 0x0400147A RID: 5242
		public int nextHalfEdge;

		// Token: 0x0400147B RID: 5243
		public int pair;

		// Token: 0x0400147C RID: 5244
		public int endVertex;
	}

	// Token: 0x020003E0 RID: 992
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct Vertex
	{
		// Token: 0x0600192F RID: 6447 RVA: 0x0008B8F8 File Offset: 0x00089CF8
		public Vertex(Vector3 position, int index, int halfEdge)
		{
			this.index = index;
			this.halfEdge = halfEdge;
			this.position = position;
		}

		// Token: 0x0400147D RID: 5245
		public int index;

		// Token: 0x0400147E RID: 5246
		public int halfEdge;

		// Token: 0x0400147F RID: 5247
		public Vector3 position;
	}

	// Token: 0x020003E1 RID: 993
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct Face
	{
		// Token: 0x06001930 RID: 6448 RVA: 0x0008B90F File Offset: 0x00089D0F
		public Face(int index)
		{
			this.index = index;
			this.halfEdge = -1;
		}

		// Token: 0x04001480 RID: 5248
		public int index;

		// Token: 0x04001481 RID: 5249
		public int halfEdge;
	}

	// Token: 0x020003E2 RID: 994
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct MeshInformation
	{
		// Token: 0x04001482 RID: 5250
		public float volume;

		// Token: 0x04001483 RID: 5251
		public float area;

		// Token: 0x04001484 RID: 5252
		public int borderEdgeCount;

		// Token: 0x04001485 RID: 5253
		[MarshalAs(UnmanagedType.I1)]
		public bool closed;

		// Token: 0x04001486 RID: 5254
		[MarshalAs(UnmanagedType.I1)]
		public bool nonManifold;
	}
}
