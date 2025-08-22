using System;
using System.Collections.Generic;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x020004A0 RID: 1184
	[RequireComponent(typeof(UILineRenderer))]
	public class LineRendererOrbit : MonoBehaviour
	{
		// Token: 0x06001DDE RID: 7646 RVA: 0x000AB55A File Offset: 0x000A995A
		public LineRendererOrbit()
		{
		}

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x06001DDF RID: 7647 RVA: 0x000AB580 File Offset: 0x000A9980
		// (set) Token: 0x06001DE0 RID: 7648 RVA: 0x000AB588 File Offset: 0x000A9988
		public float xAxis
		{
			get
			{
				return this._xAxis;
			}
			set
			{
				this._xAxis = value;
				this.GenerateOrbit();
			}
		}

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06001DE1 RID: 7649 RVA: 0x000AB597 File Offset: 0x000A9997
		// (set) Token: 0x06001DE2 RID: 7650 RVA: 0x000AB59F File Offset: 0x000A999F
		public float yAxis
		{
			get
			{
				return this._yAxis;
			}
			set
			{
				this._yAxis = value;
				this.GenerateOrbit();
			}
		}

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06001DE3 RID: 7651 RVA: 0x000AB5AE File Offset: 0x000A99AE
		// (set) Token: 0x06001DE4 RID: 7652 RVA: 0x000AB5B6 File Offset: 0x000A99B6
		public int Steps
		{
			get
			{
				return this._steps;
			}
			set
			{
				this._steps = value;
				this.GenerateOrbit();
			}
		}

		// Token: 0x06001DE5 RID: 7653 RVA: 0x000AB5C5 File Offset: 0x000A99C5
		private void Awake()
		{
			this.lr = base.GetComponent<UILineRenderer>();
			this.orbitGOrt = this.OrbitGO.GetComponent<RectTransform>();
			this.GenerateOrbit();
		}

		// Token: 0x06001DE6 RID: 7654 RVA: 0x000AB5EC File Offset: 0x000A99EC
		private void Update()
		{
			this.orbitTime = ((this.orbitTime <= (float)this._steps) ? (this.orbitTime + Time.deltaTime) : (this.orbitTime = 0f));
			this.orbitGOrt.localPosition = this.circle.Evaluate(this.orbitTime);
		}

		// Token: 0x06001DE7 RID: 7655 RVA: 0x000AB654 File Offset: 0x000A9A54
		private void GenerateOrbit()
		{
			this.circle = new Circle(this._xAxis, this._yAxis, this._steps);
			List<Vector2> list = new List<Vector2>();
			for (int i = 0; i < this._steps; i++)
			{
				list.Add(this.circle.Evaluate((float)i));
			}
			list.Add(this.circle.Evaluate(0f));
			this.lr.Points = list.ToArray();
		}

		// Token: 0x06001DE8 RID: 7656 RVA: 0x000AB6D5 File Offset: 0x000A9AD5
		private void OnValidate()
		{
			if (this.lr != null)
			{
				this.GenerateOrbit();
			}
		}

		// Token: 0x04001945 RID: 6469
		private UILineRenderer lr;

		// Token: 0x04001946 RID: 6470
		private Circle circle;

		// Token: 0x04001947 RID: 6471
		public GameObject OrbitGO;

		// Token: 0x04001948 RID: 6472
		private RectTransform orbitGOrt;

		// Token: 0x04001949 RID: 6473
		private float orbitTime;

		// Token: 0x0400194A RID: 6474
		[SerializeField]
		private float _xAxis = 3f;

		// Token: 0x0400194B RID: 6475
		[SerializeField]
		private float _yAxis = 3f;

		// Token: 0x0400194C RID: 6476
		[SerializeField]
		private int _steps = 10;
	}
}
