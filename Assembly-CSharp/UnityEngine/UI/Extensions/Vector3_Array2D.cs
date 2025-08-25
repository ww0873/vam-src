using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020004EE RID: 1262
	[Serializable]
	public struct Vector3_Array2D
	{
		// Token: 0x170003B1 RID: 945
		public Vector3 this[int _idx]
		{
			get
			{
				return this.array[_idx];
			}
			set
			{
				this.array[_idx] = value;
			}
		}

		// Token: 0x04001ADE RID: 6878
		[SerializeField]
		public Vector3[] array;
	}
}
