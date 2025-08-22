using System;
using uFileBrowser;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000466 RID: 1126
public class generic_example : MonoBehaviour
{
	// Token: 0x06001C28 RID: 7208 RVA: 0x0009FA63 File Offset: 0x0009DE63
	public generic_example()
	{
	}

	// Token: 0x06001C29 RID: 7209 RVA: 0x0009FA6B File Offset: 0x0009DE6B
	public void ShowButtonClick()
	{
		this.browser.Show(new FileBrowserCallback(this.BrowserClosed), true);
	}

	// Token: 0x06001C2A RID: 7210 RVA: 0x0009FA85 File Offset: 0x0009DE85
	public void BrowserClosed(string path)
	{
		if (string.IsNullOrEmpty(path))
		{
			Debug.Log("No file selected.");
			return;
		}
		this.file.text = "You selected:\n" + path;
	}

	// Token: 0x040017D5 RID: 6101
	public FileBrowser browser;

	// Token: 0x040017D6 RID: 6102
	public Text file;
}
