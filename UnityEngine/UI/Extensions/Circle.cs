using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200053F RID: 1343
	public class Circle
	{
		// Token: 0x06002242 RID: 8770 RVA: 0x000C456E File Offset: 0x000C296E
		public Circle(float radius)
		{
			this.xAxis = radius;
			this.yAxis = radius;
			this.steps = 1;
		}

		// Token: 0x06002243 RID: 8771 RVA: 0x000C458B File Offset: 0x000C298B
		public Circle(float radius, int steps)
		{
			this.xAxis = radius;
			this.yAxis = radius;
			this.steps = steps;
		}

		// Token: 0x06002244 RID: 8772 RVA: 0x000C45A8 File Offset: 0x000C29A8
		public Circle(float xAxis, float yAxis)
		{
			this.xAxis = xAxis;
			this.yAxis = yAxis;
			this.steps = 10;
		}

		// Token: 0x06002245 RID: 8773 RVA: 0x000C45C6 File Offset: 0x000C29C6
		public Circle(float xAxis, float yAxis, int steps)
		{
			this.xAxis = xAxis;
			this.yAxis = yAxis;
			this.steps = steps;
		}

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x06002246 RID: 8774 RVA: 0x000C45E3 File Offset: 0x000C29E3
		// (set) Token: 0x06002247 RID: 8775 RVA: 0x000C45EB File Offset: 0x000C29EB
		public float X
		{
			get
			{
				return this.xAxis;
			}
			set
			{
				this.xAxis = value;
			}
		}

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x06002248 RID: 8776 RVA: 0x000C45F4 File Offset: 0x000C29F4
		// (set) Token: 0x06002249 RID: 8777 RVA: 0x000C45FC File Offset: 0x000C29FC
		public float Y
		{
			get
			{
				return this.yAxis;
			}
			set
			{
				this.yAxis = value;
			}
		}

		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x0600224A RID: 8778 RVA: 0x000C4605 File Offset: 0x000C2A05
		// (set) Token: 0x0600224B RID: 8779 RVA: 0x000C460D File Offset: 0x000C2A0D
		public int Steps
		{
			get
			{
				return this.steps;
			}
			set
			{
				this.steps = value;
			}
		}

		// Token: 0x0600224C RID: 8780 RVA: 0x000C4618 File Offset: 0x000C2A18
		public Vector2 Evaluate(float t)
		{
			float num = 360f / (float)this.steps;
			float f = 0.017453292f * num * t;
			float x = Mathf.Sin(f) * this.xAxis;
			float y = Mathf.Cos(f) * this.yAxis;
			return new Vector2(x, y);
		}

		// Token: 0x0600224D RID: 8781 RVA: 0x000C4660 File Offset: 0x000C2A60
		public void Evaluate(float t, out Vector2 eval)
		{
			float num = 360f / (float)this.steps;
			float f = 0.017453292f * num * t;
			eval.x = Mathf.Sin(f) * this.xAxis;
			eval.y = Mathf.Cos(f) * this.yAxis;
		}

		// Token: 0x04001C78 RID: 7288
		[SerializeField]
		private float xAxis;

		// Token: 0x04001C79 RID: 7289
		[SerializeField]
		private float yAxis;

		// Token: 0x04001C7A RID: 7290
		[SerializeField]
		private int steps;
	}
}
