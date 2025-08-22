using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;
using UnityEngine.AI;

namespace Battlehub.RTSaveLoad.PersistentObjects.AI
{
	// Token: 0x02000190 RID: 400
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentNavMeshObstacle : PersistentBehaviour
	{
		// Token: 0x06000895 RID: 2197 RVA: 0x00036FA5 File Offset: 0x000353A5
		public PersistentNavMeshObstacle()
		{
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x00036FB0 File Offset: 0x000353B0
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			NavMeshObstacle navMeshObstacle = (NavMeshObstacle)obj;
			navMeshObstacle.height = this.height;
			navMeshObstacle.radius = this.radius;
			navMeshObstacle.velocity = this.velocity;
			navMeshObstacle.carving = this.carving;
			navMeshObstacle.carveOnlyStationary = this.carveOnlyStationary;
			navMeshObstacle.carvingMoveThreshold = this.carvingMoveThreshold;
			navMeshObstacle.carvingTimeToStationary = this.carvingTimeToStationary;
			navMeshObstacle.shape = (NavMeshObstacleShape)this.shape;
			navMeshObstacle.center = this.center;
			navMeshObstacle.size = this.size;
			return navMeshObstacle;
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x00037050 File Offset: 0x00035450
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			NavMeshObstacle navMeshObstacle = (NavMeshObstacle)obj;
			this.height = navMeshObstacle.height;
			this.radius = navMeshObstacle.radius;
			this.velocity = navMeshObstacle.velocity;
			this.carving = navMeshObstacle.carving;
			this.carveOnlyStationary = navMeshObstacle.carveOnlyStationary;
			this.carvingMoveThreshold = navMeshObstacle.carvingMoveThreshold;
			this.carvingTimeToStationary = navMeshObstacle.carvingTimeToStationary;
			this.shape = (uint)navMeshObstacle.shape;
			this.center = navMeshObstacle.center;
			this.size = navMeshObstacle.size;
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x000370EA File Offset: 0x000354EA
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x04000961 RID: 2401
		public float height;

		// Token: 0x04000962 RID: 2402
		public float radius;

		// Token: 0x04000963 RID: 2403
		public Vector3 velocity;

		// Token: 0x04000964 RID: 2404
		public bool carving;

		// Token: 0x04000965 RID: 2405
		public bool carveOnlyStationary;

		// Token: 0x04000966 RID: 2406
		public float carvingMoveThreshold;

		// Token: 0x04000967 RID: 2407
		public float carvingTimeToStationary;

		// Token: 0x04000968 RID: 2408
		public uint shape;

		// Token: 0x04000969 RID: 2409
		public Vector3 center;

		// Token: 0x0400096A RID: 2410
		public Vector3 size;
	}
}
