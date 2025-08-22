using System;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x02000497 RID: 1175
	public class UpdateScrollSnap : MonoBehaviour
	{
		// Token: 0x06001DBC RID: 7612 RVA: 0x000AAEDB File Offset: 0x000A92DB
		public UpdateScrollSnap()
		{
		}

		// Token: 0x06001DBD RID: 7613 RVA: 0x000AAEE4 File Offset: 0x000A92E4
		public void AddButton()
		{
			if (this.HSS)
			{
				GameObject go = Object.Instantiate<GameObject>(this.HorizontalPagePrefab);
				this.HSS.AddChild(go);
			}
			if (this.VSS)
			{
				GameObject go2 = Object.Instantiate<GameObject>(this.VerticalPagePrefab);
				this.VSS.AddChild(go2);
			}
		}

		// Token: 0x06001DBE RID: 7614 RVA: 0x000AAF44 File Offset: 0x000A9344
		public void RemoveButton()
		{
			if (this.HSS)
			{
				GameObject gameObject;
				this.HSS.RemoveChild(this.HSS.CurrentPage, out gameObject);
				gameObject.SetActive(false);
			}
			if (this.VSS)
			{
				GameObject gameObject2;
				this.VSS.RemoveChild(this.VSS.CurrentPage, out gameObject2);
				gameObject2.SetActive(false);
			}
		}

		// Token: 0x06001DBF RID: 7615 RVA: 0x000AAFB0 File Offset: 0x000A93B0
		public void JumpToPage()
		{
			int screenIndex = int.Parse(this.JumpPage.text);
			if (this.HSS)
			{
				this.HSS.GoToScreen(screenIndex);
			}
			if (this.VSS)
			{
				this.VSS.GoToScreen(screenIndex);
			}
		}

		// Token: 0x06001DC0 RID: 7616 RVA: 0x000AB006 File Offset: 0x000A9406
		public void SelectionStartChange()
		{
			Debug.Log("Scroll Snap change started");
		}

		// Token: 0x06001DC1 RID: 7617 RVA: 0x000AB012 File Offset: 0x000A9412
		public void SelectionEndChange()
		{
			Debug.Log("Scroll Snap change finished");
		}

		// Token: 0x06001DC2 RID: 7618 RVA: 0x000AB01E File Offset: 0x000A941E
		public void PageChange(int page)
		{
			Debug.Log(string.Format("Scroll Snap page changed to {0}", page));
		}

		// Token: 0x06001DC3 RID: 7619 RVA: 0x000AB038 File Offset: 0x000A9438
		public void RemoveAll()
		{
			GameObject[] array;
			this.HSS.RemoveAllChildren(out array);
			this.VSS.RemoveAllChildren(out array);
		}

		// Token: 0x06001DC4 RID: 7620 RVA: 0x000AB05F File Offset: 0x000A945F
		public void JumpToSelectedToggle(int page)
		{
			this.HSS.GoToScreen(page);
		}

		// Token: 0x04001927 RID: 6439
		public HorizontalScrollSnap HSS;

		// Token: 0x04001928 RID: 6440
		public VerticalScrollSnap VSS;

		// Token: 0x04001929 RID: 6441
		public GameObject HorizontalPagePrefab;

		// Token: 0x0400192A RID: 6442
		public GameObject VerticalPagePrefab;

		// Token: 0x0400192B RID: 6443
		public InputField JumpPage;
	}
}
