using System;
using UnityEngine;

// Token: 0x02000D5C RID: 3420
public class ExtendedCapsuleCollider
{
	// Token: 0x06006924 RID: 26916 RVA: 0x00276A13 File Offset: 0x00274E13
	public ExtendedCapsuleCollider()
	{
	}

	// Token: 0x06006925 RID: 26917 RVA: 0x00276A1B File Offset: 0x00274E1B
	public void UpdateEndpoints()
	{
		this.endPoint1 = this.collider.transform.TransformPoint(this.endPoint1Local);
		this.endPoint2 = this.collider.transform.TransformPoint(this.endPoint2Local);
	}

	// Token: 0x06006926 RID: 26918 RVA: 0x00276A58 File Offset: 0x00274E58
	public void RecalculateVars()
	{
		if (this.collider != null)
		{
			float x = this.collider.transform.lossyScale.x;
			this.localCenter = this.collider.center;
			this.unscaledRadius = this.collider.radius;
			this.radius = this.unscaledRadius * x;
			this.radiusSquared = this.radius * this.radius;
			this.unscaledLength = this.collider.height - 2f * this.unscaledRadius;
			this.length = this.unscaledLength * x;
			this.oneOverLength = 1f / this.length;
			this.halfLength = this.length * 0.5f;
			this.unscaledHalfLength = this.unscaledLength * 0.5f;
			this.lengthSquared = this.length * this.length;
			this.oneOverLengthSquared = 1f / this.lengthSquared;
			int direction = this.collider.direction;
			if (direction != 0)
			{
				if (direction != 1)
				{
					if (direction == 2)
					{
						this.endPoint1Local.x = this.localCenter.x;
						this.endPoint1Local.y = this.localCenter.y;
						this.endPoint1Local.z = this.localCenter.z + this.unscaledHalfLength;
						this.endPoint2Local.x = this.localCenter.x;
						this.endPoint2Local.y = this.localCenter.y;
						this.endPoint2Local.z = this.localCenter.z - this.unscaledHalfLength;
					}
				}
				else
				{
					this.endPoint1Local.x = this.localCenter.x;
					this.endPoint1Local.y = this.localCenter.y + this.unscaledHalfLength;
					this.endPoint1Local.z = this.localCenter.z;
					this.endPoint2Local.x = this.localCenter.x;
					this.endPoint2Local.y = this.localCenter.y - this.unscaledHalfLength;
					this.endPoint2Local.z = this.localCenter.z;
				}
			}
			else
			{
				this.endPoint1Local.x = this.localCenter.x + this.unscaledHalfLength;
				this.endPoint1Local.y = this.localCenter.y;
				this.endPoint1Local.z = this.localCenter.z;
				this.endPoint2Local.x = this.localCenter.x - this.unscaledHalfLength;
				this.endPoint2Local.y = this.localCenter.y;
				this.endPoint2Local.z = this.localCenter.z;
			}
		}
	}

	// Token: 0x040059E6 RID: 23014
	public CapsuleCollider collider;

	// Token: 0x040059E7 RID: 23015
	public Vector3 endPoint1;

	// Token: 0x040059E8 RID: 23016
	public Vector3 endPoint2;

	// Token: 0x040059E9 RID: 23017
	public Vector3 endPoint1Local;

	// Token: 0x040059EA RID: 23018
	public Vector3 endPoint2Local;

	// Token: 0x040059EB RID: 23019
	public Vector3 localCenter;

	// Token: 0x040059EC RID: 23020
	public float unscaledRadius;

	// Token: 0x040059ED RID: 23021
	public float radius;

	// Token: 0x040059EE RID: 23022
	public float radiusSquared;

	// Token: 0x040059EF RID: 23023
	public float unscaledLength;

	// Token: 0x040059F0 RID: 23024
	public float length;

	// Token: 0x040059F1 RID: 23025
	public float oneOverLength;

	// Token: 0x040059F2 RID: 23026
	public float unscaledHalfLength;

	// Token: 0x040059F3 RID: 23027
	public float halfLength;

	// Token: 0x040059F4 RID: 23028
	public float lengthSquared;

	// Token: 0x040059F5 RID: 23029
	public float oneOverLengthSquared;
}
