using System;
using UnityEngine;

namespace Weelco.VRInput
{
	// Token: 0x02000589 RID: 1417
	public abstract class IUIPointer
	{
		// Token: 0x060023A6 RID: 9126 RVA: 0x000CED09 File Offset: 0x000CD109
		protected IUIPointer()
		{
		}

		// Token: 0x060023A7 RID: 9127 RVA: 0x000CED14 File Offset: 0x000CD114
		public virtual void Update()
		{
			Ray ray = new Ray(this.target.transform.position, this.target.transform.forward);
			RaycastHit raycastHit;
			bool flag = Physics.Raycast(ray, out raycastHit);
			float num = 100f;
			if (flag)
			{
				num = raycastHit.distance;
			}
			if (this._distanceLimit > 0f)
			{
				num = Mathf.Min(num, this._distanceLimit);
				flag = true;
			}
			this.UpdateRaycasting(flag, num);
			this._distanceLimit = -1f;
		}

		// Token: 0x060023A8 RID: 9128 RVA: 0x000CED97 File Offset: 0x000CD197
		public void LimitLaserDistance(float distance)
		{
			if (distance < 0f)
			{
				return;
			}
			if (this._distanceLimit < 0f)
			{
				this._distanceLimit = distance;
			}
			else
			{
				this._distanceLimit = Mathf.Min(this._distanceLimit, distance);
			}
		}

		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x060023A9 RID: 9129
		public abstract Transform target { get; }

		// Token: 0x060023AA RID: 9130
		public abstract void Initialize();

		// Token: 0x060023AB RID: 9131
		public abstract void OnEnterControl(GameObject control);

		// Token: 0x060023AC RID: 9132
		public abstract void OnExitControl(GameObject control);

		// Token: 0x060023AD RID: 9133
		protected abstract void UpdateRaycasting(bool isHit, float distance);

		// Token: 0x04001E22 RID: 7714
		private float _distanceLimit;
	}
}
