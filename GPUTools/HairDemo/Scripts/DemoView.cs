using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GPUTools.HairDemo.Scripts
{
	// Token: 0x020009E8 RID: 2536
	public class DemoView : MonoBehaviour
	{
		// Token: 0x06003FD5 RID: 16341 RVA: 0x001305C2 File Offset: 0x0012E9C2
		public DemoView()
		{
		}

		// Token: 0x06003FD6 RID: 16342 RVA: 0x001305CC File Offset: 0x0012E9CC
		private void Start()
		{
			this.SetStartStyle();
			this.play.onClick.AddListener(new UnityAction(this.OnClickPlay));
			this.stop.onClick.AddListener(new UnityAction(this.OnClickStop));
			this.next.onClick.AddListener(new UnityAction(this.OnClickNext));
			this.prev.onClick.AddListener(new UnityAction(this.OnClickPrev));
			this.rotate.onClick.AddListener(new UnityAction(this.OnClickRotate));
		}

		// Token: 0x06003FD7 RID: 16343 RVA: 0x0013066B File Offset: 0x0012EA6B
		private void OnClickRotate()
		{
			this.rotation.Speed += 200f;
			if (this.rotation.Speed >= 800f)
			{
				this.rotation.Speed = 0f;
			}
		}

		// Token: 0x06003FD8 RID: 16344 RVA: 0x001306A9 File Offset: 0x0012EAA9
		private void OnClickPrev()
		{
			this.CurrentStyleIndex--;
		}

		// Token: 0x06003FD9 RID: 16345 RVA: 0x001306B9 File Offset: 0x0012EAB9
		private void OnClickNext()
		{
			this.CurrentStyleIndex++;
		}

		// Token: 0x06003FDA RID: 16346 RVA: 0x001306C9 File Offset: 0x0012EAC9
		private void OnClickStop()
		{
			this.CurrentStyle.GetComponent<Animator>().enabled = false;
		}

		// Token: 0x06003FDB RID: 16347 RVA: 0x001306DC File Offset: 0x0012EADC
		private void OnClickPlay()
		{
			this.CurrentStyle.GetComponent<Animator>().enabled = true;
		}

		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x06003FDC RID: 16348 RVA: 0x001306EF File Offset: 0x0012EAEF
		private GameObject CurrentStyle
		{
			get
			{
				return this.styles[this.currentStyleIndex];
			}
		}

		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x06003FDE RID: 16350 RVA: 0x00130752 File Offset: 0x0012EB52
		// (set) Token: 0x06003FDD RID: 16349 RVA: 0x00130700 File Offset: 0x0012EB00
		private int CurrentStyleIndex
		{
			get
			{
				return this.currentStyleIndex;
			}
			set
			{
				this.currentStyleIndex = value;
				if (this.currentStyleIndex < 0)
				{
					this.currentStyleIndex = this.styles.Length - 1;
				}
				if (this.currentStyleIndex > this.styles.Length - 1)
				{
					this.currentStyleIndex = 0;
				}
				this.ApplyStyle();
			}
		}

		// Token: 0x06003FDF RID: 16351 RVA: 0x0013075C File Offset: 0x0012EB5C
		private void ApplyStyle()
		{
			for (int i = 0; i < this.styles.Length; i++)
			{
				GameObject gameObject = this.styles[i];
				gameObject.SetActive(i == this.currentStyleIndex);
			}
		}

		// Token: 0x06003FE0 RID: 16352 RVA: 0x0013079C File Offset: 0x0012EB9C
		private void SetStartStyle()
		{
			for (int i = 0; i < this.styles.Length; i++)
			{
				GameObject gameObject = this.styles[i];
				if (gameObject.activeSelf)
				{
					this.CurrentStyleIndex = i;
				}
			}
		}

		// Token: 0x0400303B RID: 12347
		[SerializeField]
		private Button play;

		// Token: 0x0400303C RID: 12348
		[SerializeField]
		private Button stop;

		// Token: 0x0400303D RID: 12349
		[SerializeField]
		private Button next;

		// Token: 0x0400303E RID: 12350
		[SerializeField]
		private Button prev;

		// Token: 0x0400303F RID: 12351
		[SerializeField]
		private Button rotate;

		// Token: 0x04003040 RID: 12352
		[SerializeField]
		private GameObject[] styles;

		// Token: 0x04003041 RID: 12353
		[SerializeField]
		private ConstantRotation rotation;

		// Token: 0x04003042 RID: 12354
		private int currentStyleIndex;
	}
}
