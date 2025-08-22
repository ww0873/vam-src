using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.Create
{
	// Token: 0x020009F2 RID: 2546
	[Serializable]
	public class GeometryGroupHistory
	{
		// Token: 0x0600401B RID: 16411 RVA: 0x00131230 File Offset: 0x0012F630
		public GeometryGroupHistory()
		{
		}

		// Token: 0x0600401C RID: 16412 RVA: 0x00131244 File Offset: 0x0012F644
		public void Record(List<Vector3> list)
		{
			this.pointer = this.history.Count;
			this.history.Add(list.ToList<Vector3>());
			if (this.history.Count > 10)
			{
				this.history.RemoveAt(0);
			}
		}

		// Token: 0x0600401D RID: 16413 RVA: 0x00131291 File Offset: 0x0012F691
		public List<Vector3> Undo()
		{
			if (this.pointer > 0)
			{
				this.pointer--;
			}
			return this.history[this.pointer].ToList<Vector3>();
		}

		// Token: 0x0600401E RID: 16414 RVA: 0x001312C3 File Offset: 0x0012F6C3
		public List<Vector3> Redo()
		{
			if (this.pointer < this.history.Count - 1)
			{
				this.pointer++;
			}
			return this.history[this.pointer].ToList<Vector3>();
		}

		// Token: 0x0600401F RID: 16415 RVA: 0x00131301 File Offset: 0x0012F701
		public void Clear()
		{
			this.history.Clear();
			this.pointer = 0;
		}

		// Token: 0x1700077F RID: 1919
		// (get) Token: 0x06004020 RID: 16416 RVA: 0x00131315 File Offset: 0x0012F715
		public bool IsUndo
		{
			get
			{
				return this.history.Count > 0 && this.pointer > 0;
			}
		}

		// Token: 0x17000780 RID: 1920
		// (get) Token: 0x06004021 RID: 16417 RVA: 0x00131334 File Offset: 0x0012F734
		public bool IsRedo
		{
			get
			{
				return this.history.Count > 1 && this.pointer < this.history.Count - 1;
			}
		}

		// Token: 0x0400306A RID: 12394
		[SerializeField]
		private readonly List<List<Vector3>> history = new List<List<Vector3>>();

		// Token: 0x0400306B RID: 12395
		[SerializeField]
		private int pointer;
	}
}
