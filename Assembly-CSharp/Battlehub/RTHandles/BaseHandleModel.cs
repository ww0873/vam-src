using System;
using System.Collections;
using Battlehub.RTCommon;
using Battlehub.RTSaveLoad;
using UnityEngine;

namespace Battlehub.RTHandles
{
	// Token: 0x020000F7 RID: 247
	public class BaseHandleModel : MonoBehaviour
	{
		// Token: 0x060005A0 RID: 1440 RVA: 0x0001F35C File Offset: 0x0001D75C
		public BaseHandleModel()
		{
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x0001F3EC File Offset: 0x0001D7EC
		protected virtual void Awake()
		{
			this.m_graphicsLayer = UnityEngine.Object.FindObjectOfType<RuntimeGraphicsLayer>();
			if (this.m_graphicsLayer == null)
			{
				GameObject gameObject = new GameObject();
				gameObject.AddComponent<PersistentIgnore>();
				this.m_graphicsLayer = gameObject.AddComponent<RuntimeGraphicsLayer>();
				this.m_graphicsLayer.name = "RuntimeGraphicsLayer";
			}
			this.SetLayer(base.transform, this.m_graphicsLayer.GraphicsLayer);
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x0001F458 File Offset: 0x0001D858
		private void SetLayer(Transform t, int layer)
		{
			t.gameObject.layer = layer;
			IEnumerator enumerator = t.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform t2 = (Transform)obj;
					this.SetLayer(t2, layer);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x0001F4C8 File Offset: 0x0001D8C8
		public virtual void SetLock(LockObject lockObj)
		{
			if (lockObj == null)
			{
				lockObj = new LockObject();
			}
			this.m_lockObj = lockObj;
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x0001F4DE File Offset: 0x0001D8DE
		public virtual void Select(RuntimeHandleAxis axis)
		{
			this.m_selectedAxis = axis;
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x0001F4E7 File Offset: 0x0001D8E7
		public virtual void SetScale(Vector3 scale)
		{
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x0001F4E9 File Offset: 0x0001D8E9
		protected virtual void Start()
		{
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x0001F4EB File Offset: 0x0001D8EB
		protected virtual void OnDestroy()
		{
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x0001F4ED File Offset: 0x0001D8ED
		protected virtual void Update()
		{
		}

		// Token: 0x04000509 RID: 1289
		[SerializeField]
		protected Color m_xColor = RTHColors.XColor;

		// Token: 0x0400050A RID: 1290
		[SerializeField]
		protected Color m_yColor = RTHColors.YColor;

		// Token: 0x0400050B RID: 1291
		[SerializeField]
		protected Color m_zColor = RTHColors.ZColor;

		// Token: 0x0400050C RID: 1292
		[SerializeField]
		protected Color m_altColor = RTHColors.AltColor;

		// Token: 0x0400050D RID: 1293
		[SerializeField]
		protected Color m_altColor2 = RTHColors.AltColor2;

		// Token: 0x0400050E RID: 1294
		[SerializeField]
		protected Color m_disabledColor = RTHColors.DisabledColor;

		// Token: 0x0400050F RID: 1295
		[SerializeField]
		protected Color m_selectionColor = RTHColors.SelectionColor;

		// Token: 0x04000510 RID: 1296
		protected RuntimeHandleAxis m_selectedAxis;

		// Token: 0x04000511 RID: 1297
		protected LockObject m_lockObj = new LockObject();

		// Token: 0x04000512 RID: 1298
		protected RuntimeGraphicsLayer m_graphicsLayer;
	}
}
