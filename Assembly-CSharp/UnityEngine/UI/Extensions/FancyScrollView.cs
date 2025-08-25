using System;
using System.Collections.Generic;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200050A RID: 1290
	public class FancyScrollView<TData, TContext> : MonoBehaviour where TContext : class
	{
		// Token: 0x06002094 RID: 8340 RVA: 0x000AA4CA File Offset: 0x000A88CA
		public FancyScrollView()
		{
		}

		// Token: 0x06002095 RID: 8341 RVA: 0x000AA4E8 File Offset: 0x000A88E8
		protected void Awake()
		{
			this.cellBase.SetActive(false);
		}

		// Token: 0x06002096 RID: 8342 RVA: 0x000AA4F8 File Offset: 0x000A88F8
		protected void SetContext(TContext context)
		{
			this.context = context;
			for (int i = 0; i < this.cells.Count; i++)
			{
				this.cells[i].SetContext(context);
			}
		}

		// Token: 0x06002097 RID: 8343 RVA: 0x000AA53C File Offset: 0x000A893C
		private FancyScrollViewCell<TData, TContext> CreateCell()
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.cellBase);
			gameObject.SetActive(true);
			FancyScrollViewCell<TData, TContext> component = gameObject.GetComponent<FancyScrollViewCell<TData, TContext>>();
			RectTransform rectTransform = component.transform as RectTransform;
			Vector3 localScale = component.transform.localScale;
			Vector2 sizeDelta = Vector2.zero;
			Vector2 offsetMin = Vector2.zero;
			Vector2 offsetMax = Vector2.zero;
			if (rectTransform)
			{
				sizeDelta = rectTransform.sizeDelta;
				offsetMin = rectTransform.offsetMin;
				offsetMax = rectTransform.offsetMax;
			}
			component.transform.SetParent(this.cellBase.transform.parent);
			component.transform.localScale = localScale;
			if (rectTransform)
			{
				rectTransform.sizeDelta = sizeDelta;
				rectTransform.offsetMin = offsetMin;
				rectTransform.offsetMax = offsetMax;
			}
			component.SetContext(this.context);
			component.SetVisible(false);
			return component;
		}

		// Token: 0x06002098 RID: 8344 RVA: 0x000AA614 File Offset: 0x000A8A14
		private void UpdateCellForIndex(FancyScrollViewCell<TData, TContext> cell, int dataIndex)
		{
			if (this.loop)
			{
				dataIndex = this.GetLoopIndex(dataIndex, this.cellData.Count);
			}
			else if (dataIndex < 0 || dataIndex > this.cellData.Count - 1)
			{
				cell.SetVisible(false);
				return;
			}
			cell.SetVisible(true);
			cell.DataIndex = dataIndex;
			cell.UpdateContent(this.cellData[dataIndex]);
		}

		// Token: 0x06002099 RID: 8345 RVA: 0x000AA687 File Offset: 0x000A8A87
		private int GetLoopIndex(int index, int length)
		{
			if (index < 0)
			{
				index = length - 1 + (index + 1) % length;
			}
			else if (index > length - 1)
			{
				index %= length;
			}
			return index;
		}

		// Token: 0x0600209A RID: 8346 RVA: 0x000AA6AF File Offset: 0x000A8AAF
		protected void UpdateContents()
		{
			this.UpdatePosition(this.currentPosition);
		}

		// Token: 0x0600209B RID: 8347 RVA: 0x000AA6C0 File Offset: 0x000A8AC0
		protected void UpdatePosition(float position)
		{
			this.currentPosition = position;
			float num = position - this.cellOffset / this.cellInterval;
			float num2 = (Mathf.Ceil(num) - num) * this.cellInterval;
			int num3 = Mathf.CeilToInt(num);
			int i = 0;
			float num4 = num2;
			while (num4 <= 1f)
			{
				if (i >= this.cells.Count)
				{
					this.cells.Add(this.CreateCell());
				}
				num4 += this.cellInterval;
				i++;
			}
			i = 0;
			int loopIndex;
			for (float num5 = num2; num5 <= 1f; num5 += this.cellInterval)
			{
				int num6 = num3 + i;
				loopIndex = this.GetLoopIndex(num6, this.cells.Count);
				if (this.cells[loopIndex].gameObject.activeSelf)
				{
					this.cells[loopIndex].UpdatePosition(num5);
				}
				this.UpdateCellForIndex(this.cells[loopIndex], num6);
				i++;
			}
			loopIndex = this.GetLoopIndex(num3 + i, this.cells.Count);
			while (i < this.cells.Count)
			{
				this.cells[loopIndex].SetVisible(false);
				i++;
				loopIndex = this.GetLoopIndex(num3 + i, this.cells.Count);
			}
		}

		// Token: 0x04001B48 RID: 6984
		[SerializeField]
		[Range(1E-45f, 1f)]
		private float cellInterval;

		// Token: 0x04001B49 RID: 6985
		[SerializeField]
		[Range(0f, 1f)]
		private float cellOffset;

		// Token: 0x04001B4A RID: 6986
		[SerializeField]
		private bool loop;

		// Token: 0x04001B4B RID: 6987
		[SerializeField]
		private GameObject cellBase;

		// Token: 0x04001B4C RID: 6988
		private float currentPosition;

		// Token: 0x04001B4D RID: 6989
		private readonly List<FancyScrollViewCell<TData, TContext>> cells = new List<FancyScrollViewCell<TData, TContext>>();

		// Token: 0x04001B4E RID: 6990
		protected TContext context;

		// Token: 0x04001B4F RID: 6991
		protected List<TData> cellData = new List<TData>();
	}
}
