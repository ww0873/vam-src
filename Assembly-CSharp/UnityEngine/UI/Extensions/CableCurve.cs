using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200053E RID: 1342
	[Serializable]
	public class CableCurve
	{
		// Token: 0x06002232 RID: 8754 RVA: 0x000C3FDC File Offset: 0x000C23DC
		public CableCurve()
		{
			this.points = CableCurve.emptyCurve;
			this.m_start = Vector2.up;
			this.m_end = Vector2.up + Vector2.right;
			this.m_slack = 0.5f;
			this.m_steps = 20;
			this.m_regen = true;
		}

		// Token: 0x06002233 RID: 8755 RVA: 0x000C4034 File Offset: 0x000C2434
		public CableCurve(Vector2[] inputPoints)
		{
			this.points = inputPoints;
			this.m_start = inputPoints[0];
			this.m_end = inputPoints[1];
			this.m_slack = 0.5f;
			this.m_steps = 20;
			this.m_regen = true;
		}

		// Token: 0x06002234 RID: 8756 RVA: 0x000C408C File Offset: 0x000C248C
		public CableCurve(CableCurve v)
		{
			this.points = v.Points();
			this.m_start = v.start;
			this.m_end = v.end;
			this.m_slack = v.slack;
			this.m_steps = v.steps;
			this.m_regen = v.regenPoints;
		}

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x06002235 RID: 8757 RVA: 0x000C40E7 File Offset: 0x000C24E7
		// (set) Token: 0x06002236 RID: 8758 RVA: 0x000C40EF File Offset: 0x000C24EF
		public bool regenPoints
		{
			get
			{
				return this.m_regen;
			}
			set
			{
				this.m_regen = value;
			}
		}

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x06002237 RID: 8759 RVA: 0x000C40F8 File Offset: 0x000C24F8
		// (set) Token: 0x06002238 RID: 8760 RVA: 0x000C4100 File Offset: 0x000C2500
		public Vector2 start
		{
			get
			{
				return this.m_start;
			}
			set
			{
				if (value != this.m_start)
				{
					this.m_regen = true;
				}
				this.m_start = value;
			}
		}

		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x06002239 RID: 8761 RVA: 0x000C4121 File Offset: 0x000C2521
		// (set) Token: 0x0600223A RID: 8762 RVA: 0x000C4129 File Offset: 0x000C2529
		public Vector2 end
		{
			get
			{
				return this.m_end;
			}
			set
			{
				if (value != this.m_end)
				{
					this.m_regen = true;
				}
				this.m_end = value;
			}
		}

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x0600223B RID: 8763 RVA: 0x000C414A File Offset: 0x000C254A
		// (set) Token: 0x0600223C RID: 8764 RVA: 0x000C4152 File Offset: 0x000C2552
		public float slack
		{
			get
			{
				return this.m_slack;
			}
			set
			{
				if (value != this.m_slack)
				{
					this.m_regen = true;
				}
				this.m_slack = Mathf.Max(0f, value);
			}
		}

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x0600223D RID: 8765 RVA: 0x000C4178 File Offset: 0x000C2578
		// (set) Token: 0x0600223E RID: 8766 RVA: 0x000C4180 File Offset: 0x000C2580
		public int steps
		{
			get
			{
				return this.m_steps;
			}
			set
			{
				if (value != this.m_steps)
				{
					this.m_regen = true;
				}
				this.m_steps = Mathf.Max(2, value);
			}
		}

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x0600223F RID: 8767 RVA: 0x000C41A4 File Offset: 0x000C25A4
		public Vector2 midPoint
		{
			get
			{
				Vector2 result = Vector2.zero;
				if (this.m_steps == 2)
				{
					return (this.points[0] + this.points[1]) * 0.5f;
				}
				if (this.m_steps > 2)
				{
					int num = this.m_steps / 2;
					if (this.m_steps % 2 == 0)
					{
						result = (this.points[num] + this.points[num + 1]) * 0.5f;
					}
					else
					{
						result = this.points[num];
					}
				}
				return result;
			}
		}

		// Token: 0x06002240 RID: 8768 RVA: 0x000C4264 File Offset: 0x000C2664
		public Vector2[] Points()
		{
			if (!this.m_regen)
			{
				return this.points;
			}
			if (this.m_steps < 2)
			{
				return CableCurve.emptyCurve;
			}
			float num = Vector2.Distance(this.m_end, this.m_start);
			float num2 = Vector2.Distance(new Vector2(this.m_end.x, this.m_start.y), this.m_start);
			float num3 = num + Mathf.Max(0.0001f, this.m_slack);
			float num4 = 0f;
			float y = this.m_start.y;
			float num5 = num2;
			float y2 = this.end.y;
			if (num5 - num4 == 0f)
			{
				return CableCurve.emptyCurve;
			}
			float num6 = Mathf.Sqrt(Mathf.Pow(num3, 2f) - Mathf.Pow(y2 - y, 2f)) / (num5 - num4);
			int num7 = 30;
			int num8 = 0;
			int num9 = num7 * 10;
			bool flag = false;
			float num10 = 0f;
			float num11 = 100f;
			for (int i = 0; i < num7; i++)
			{
				for (int j = 0; j < 10; j++)
				{
					num8++;
					float num12 = num10 + num11;
					float num13 = (float)Math.Sinh((double)num12) / num12;
					if (!float.IsInfinity(num13))
					{
						if (num13 == num6)
						{
							flag = true;
							num10 = num12;
							break;
						}
						if (num13 > num6)
						{
							break;
						}
						num10 = num12;
						if (num8 > num9)
						{
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					break;
				}
				num11 *= 0.1f;
			}
			float num14 = (num5 - num4) / 2f / num10;
			float num15 = (num4 + num5 - num14 * Mathf.Log((num3 + y2 - y) / (num3 - y2 + y))) / 2f;
			float num16 = (y2 + y - num3 * (float)Math.Cosh((double)num10) / (float)Math.Sinh((double)num10)) / 2f;
			this.points = new Vector2[this.m_steps];
			float num17 = (float)(this.m_steps - 1);
			for (int k = 0; k < this.m_steps; k++)
			{
				float num18 = (float)k / num17;
				Vector2 zero = Vector2.zero;
				zero.x = Mathf.Lerp(this.start.x, this.end.x, num18);
				zero.y = num14 * (float)Math.Cosh((double)((num18 * num2 - num15) / num14)) + num16;
				this.points[k] = zero;
			}
			this.m_regen = false;
			return this.points;
		}

		// Token: 0x06002241 RID: 8769 RVA: 0x000C4520 File Offset: 0x000C2920
		// Note: this type is marked as 'beforefieldinit'.
		static CableCurve()
		{
		}

		// Token: 0x04001C71 RID: 7281
		[SerializeField]
		private Vector2 m_start;

		// Token: 0x04001C72 RID: 7282
		[SerializeField]
		private Vector2 m_end;

		// Token: 0x04001C73 RID: 7283
		[SerializeField]
		private float m_slack;

		// Token: 0x04001C74 RID: 7284
		[SerializeField]
		private int m_steps;

		// Token: 0x04001C75 RID: 7285
		[SerializeField]
		private bool m_regen;

		// Token: 0x04001C76 RID: 7286
		private static Vector2[] emptyCurve = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(0f, 0f)
		};

		// Token: 0x04001C77 RID: 7287
		[SerializeField]
		private Vector2[] points;
	}
}
