using System;
using UnityEngine;

namespace VikingCrewDevelopment
{
	// Token: 0x02000567 RID: 1383
	public class RandomMovementBehaviour : MonoBehaviour
	{
		// Token: 0x0600231E RID: 8990 RVA: 0x000C8345 File Offset: 0x000C6745
		public RandomMovementBehaviour()
		{
		}

		// Token: 0x0600231F RID: 8991 RVA: 0x000C8377 File Offset: 0x000C6777
		private void Start()
		{
			this.nextWaypoint = this.GetNextWaypoint();
		}

		// Token: 0x06002320 RID: 8992 RVA: 0x000C8388 File Offset: 0x000C6788
		private void Update()
		{
			base.transform.position = Vector2.MoveTowards(base.transform.position, this.nextWaypoint, this.speed * Time.deltaTime);
			if ((double)Vector2.Distance(base.transform.position, this.nextWaypoint) < 0.25)
			{
				this.nextWaypoint = this.GetNextWaypoint();
			}
		}

		// Token: 0x06002321 RID: 8993 RVA: 0x000C8402 File Offset: 0x000C6802
		private Vector2 GetNextWaypoint()
		{
			return new Vector2(UnityEngine.Random.Range(this.bounds.xMin, this.bounds.xMax), UnityEngine.Random.Range(this.bounds.yMin, this.bounds.yMax));
		}

		// Token: 0x04001D14 RID: 7444
		public Rect bounds = new Rect(-10f, -10f, 20f, 20f);

		// Token: 0x04001D15 RID: 7445
		public float speed = 1f;

		// Token: 0x04001D16 RID: 7446
		private Vector2 nextWaypoint;
	}
}
