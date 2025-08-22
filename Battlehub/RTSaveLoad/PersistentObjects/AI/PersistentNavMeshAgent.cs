using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;
using UnityEngine.AI;

namespace Battlehub.RTSaveLoad.PersistentObjects.AI
{
	// Token: 0x0200018E RID: 398
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentNavMeshAgent : PersistentBehaviour
	{
		// Token: 0x06000890 RID: 2192 RVA: 0x00036D45 File Offset: 0x00035145
		public PersistentNavMeshAgent()
		{
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x00036D50 File Offset: 0x00035150
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			NavMeshAgent navMeshAgent = (NavMeshAgent)obj;
			navMeshAgent.destination = this.destination;
			navMeshAgent.stoppingDistance = this.stoppingDistance;
			navMeshAgent.velocity = this.velocity;
			navMeshAgent.nextPosition = this.nextPosition;
			navMeshAgent.baseOffset = this.baseOffset;
			navMeshAgent.autoTraverseOffMeshLink = this.autoTraverseOffMeshLink;
			navMeshAgent.autoBraking = this.autoBraking;
			navMeshAgent.autoRepath = this.autoRepath;
			navMeshAgent.isStopped = this.isStopped;
			navMeshAgent.path = this.path;
			navMeshAgent.areaMask = this.areaMask;
			navMeshAgent.speed = this.speed;
			navMeshAgent.angularSpeed = this.angularSpeed;
			navMeshAgent.acceleration = this.acceleration;
			navMeshAgent.updatePosition = this.updatePosition;
			navMeshAgent.updateRotation = this.updateRotation;
			navMeshAgent.updateUpAxis = this.updateUpAxis;
			navMeshAgent.radius = this.radius;
			navMeshAgent.height = this.height;
			navMeshAgent.obstacleAvoidanceType = (ObstacleAvoidanceType)this.obstacleAvoidanceType;
			navMeshAgent.avoidancePriority = this.avoidancePriority;
			return navMeshAgent;
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x00036E74 File Offset: 0x00035274
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			NavMeshAgent navMeshAgent = (NavMeshAgent)obj;
			this.destination = navMeshAgent.destination;
			this.stoppingDistance = navMeshAgent.stoppingDistance;
			this.velocity = navMeshAgent.velocity;
			this.nextPosition = navMeshAgent.nextPosition;
			this.baseOffset = navMeshAgent.baseOffset;
			this.autoTraverseOffMeshLink = navMeshAgent.autoTraverseOffMeshLink;
			this.autoBraking = navMeshAgent.autoBraking;
			this.autoRepath = navMeshAgent.autoRepath;
			this.isStopped = navMeshAgent.isStopped;
			this.path = navMeshAgent.path;
			this.areaMask = navMeshAgent.areaMask;
			this.speed = navMeshAgent.speed;
			this.angularSpeed = navMeshAgent.angularSpeed;
			this.acceleration = navMeshAgent.acceleration;
			this.updatePosition = navMeshAgent.updatePosition;
			this.updateRotation = navMeshAgent.updateRotation;
			this.updateUpAxis = navMeshAgent.updateUpAxis;
			this.radius = navMeshAgent.radius;
			this.height = navMeshAgent.height;
			this.obstacleAvoidanceType = (uint)navMeshAgent.obstacleAvoidanceType;
			this.avoidancePriority = navMeshAgent.avoidancePriority;
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x00036F92 File Offset: 0x00035392
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x0400094C RID: 2380
		public Vector3 destination;

		// Token: 0x0400094D RID: 2381
		public float stoppingDistance;

		// Token: 0x0400094E RID: 2382
		public Vector3 velocity;

		// Token: 0x0400094F RID: 2383
		public Vector3 nextPosition;

		// Token: 0x04000950 RID: 2384
		public float baseOffset;

		// Token: 0x04000951 RID: 2385
		public bool autoTraverseOffMeshLink;

		// Token: 0x04000952 RID: 2386
		public bool autoBraking;

		// Token: 0x04000953 RID: 2387
		public bool autoRepath;

		// Token: 0x04000954 RID: 2388
		public bool isStopped;

		// Token: 0x04000955 RID: 2389
		public NavMeshPath path;

		// Token: 0x04000956 RID: 2390
		public int areaMask;

		// Token: 0x04000957 RID: 2391
		public float speed;

		// Token: 0x04000958 RID: 2392
		public float angularSpeed;

		// Token: 0x04000959 RID: 2393
		public float acceleration;

		// Token: 0x0400095A RID: 2394
		public bool updatePosition;

		// Token: 0x0400095B RID: 2395
		public bool updateRotation;

		// Token: 0x0400095C RID: 2396
		public bool updateUpAxis;

		// Token: 0x0400095D RID: 2397
		public float radius;

		// Token: 0x0400095E RID: 2398
		public float height;

		// Token: 0x0400095F RID: 2399
		public uint obstacleAvoidanceType;

		// Token: 0x04000960 RID: 2400
		public int avoidancePriority;
	}
}
