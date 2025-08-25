using System;
using uFileBrowser;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000465 RID: 1125
public class directory_example : MonoBehaviour
{
	// Token: 0x06001C25 RID: 7205 RVA: 0x0009FA13 File Offset: 0x0009DE13
	public directory_example()
	{
	}

	// Token: 0x06001C26 RID: 7206 RVA: 0x0009FA1B File Offset: 0x0009DE1B
	public void ShowButtonClick()
	{
		this.browser.Show(new FileBrowserCallback(this.BrowserClosed), true);
	}

	// Token: 0x06001C27 RID: 7207 RVA: 0x0009FA35 File Offset: 0x0009DE35
	public void BrowserClosed(string path)
	{
		if (string.IsNullOrEmpty(path))
		{
			Debug.Log("No path selected.");
			return;
		}
		this.pathLabel.text = "You selected:\n" + path;
	}

	// Token: 0x040017D3 RID: 6099
	public FileBrowser browser;

	// Token: 0x040017D4 RID: 6100
	public Text pathLabel;
}
