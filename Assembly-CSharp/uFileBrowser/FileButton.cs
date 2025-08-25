using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace uFileBrowser
{
	// Token: 0x02000470 RID: 1136
	public class FileButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
	{
		// Token: 0x06001CCE RID: 7374 RVA: 0x000A463F File Offset: 0x000A2A3F
		public FileButton()
		{
		}

		// Token: 0x06001CCF RID: 7375 RVA: 0x000A4647 File Offset: 0x000A2A47
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.browser)
			{
				this.browser.OnFilePointerEnter(this);
			}
		}

		// Token: 0x06001CD0 RID: 7376 RVA: 0x000A4665 File Offset: 0x000A2A65
		public void OnPointerExit(PointerEventData eventData)
		{
			if (this.browser)
			{
				this.browser.OnFilePointerExit(this);
			}
		}

		// Token: 0x06001CD1 RID: 7377 RVA: 0x000A4683 File Offset: 0x000A2A83
		public void Select()
		{
			this.button.transition = Selectable.Transition.None;
			this.buttonImage.overrideSprite = this.selectedSprite;
		}

		// Token: 0x06001CD2 RID: 7378 RVA: 0x000A46A2 File Offset: 0x000A2AA2
		public void Unselect()
		{
			this.button.transition = Selectable.Transition.SpriteSwap;
			this.buttonImage.overrideSprite = null;
		}

		// Token: 0x06001CD3 RID: 7379 RVA: 0x000A46BC File Offset: 0x000A2ABC
		public void OnClick()
		{
			if (this.browser)
			{
				this.browser.OnFileClick(this);
			}
		}

		// Token: 0x06001CD4 RID: 7380 RVA: 0x000A46DA File Offset: 0x000A2ADA
		public void OnRenameClick()
		{
			if (this.browser)
			{
				this.browser.OnRenameClick(this);
			}
		}

		// Token: 0x06001CD5 RID: 7381 RVA: 0x000A46F8 File Offset: 0x000A2AF8
		public void OnDeleteClick()
		{
			if (this.browser)
			{
				this.browser.OnDeleteClick(this);
			}
		}

		// Token: 0x06001CD6 RID: 7382 RVA: 0x000A4716 File Offset: 0x000A2B16
		public void OnHiddenChange(bool b)
		{
			if (this.browser)
			{
				this.browser.OnHiddenChange(this, b);
			}
		}

		// Token: 0x06001CD7 RID: 7383 RVA: 0x000A4735 File Offset: 0x000A2B35
		public void OnFavoriteChange(bool b)
		{
			if (this.browser)
			{
				this.browser.OnFavoriteChange(this, b);
			}
		}

		// Token: 0x06001CD8 RID: 7384 RVA: 0x000A4754 File Offset: 0x000A2B54
		public void OnUseFileAsTemplateChange(bool b)
		{
			if (this.browser)
			{
				this.browser.OnUseFileAsTemplateChange(this, b);
			}
		}

		// Token: 0x06001CD9 RID: 7385 RVA: 0x000A4774 File Offset: 0x000A2B74
		public void Set(FileBrowser b, string txt, string path, bool dir, bool writeable, bool hidden, bool hiddenModifiable, bool favorite, bool allowUseFileAsTemplateSelect, bool isTemplate, bool isTemplateModifiable)
		{
			this.rectTransform = base.GetComponent<RectTransform>();
			this.browser = b;
			this.text = txt;
			this.textLowerInvariant = txt.ToLowerInvariant();
			this.fullPath = path;
			this.isDir = dir;
			this.label.text = this.text;
			if (this.fullPathLabel != null)
			{
				this.fullPathLabel.text = this.fullPath;
			}
			if (this.isDir)
			{
				this.fileIcon.sprite = b.folderIcon;
			}
			else
			{
				this.fileIcon.sprite = b.GetFileIcon(txt);
			}
			if (this.deleteButton != null)
			{
				this.deleteButton.gameObject.SetActive(writeable);
			}
			if (this.renameButton != null)
			{
				this.renameButton.gameObject.SetActive(writeable);
			}
			if (this.hiddenToggle != null)
			{
				this.hiddenToggle.isOn = hidden;
				if (hiddenModifiable)
				{
					this.hiddenToggle.interactable = true;
					this.hiddenToggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnHiddenChange));
				}
				else
				{
					this.hiddenToggle.interactable = false;
				}
			}
			if (this.favoriteToggle != null)
			{
				this.favoriteToggle.gameObject.SetActive(!dir);
				if (!dir)
				{
					this.favoriteToggle.isOn = favorite;
					this.favoriteToggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnFavoriteChange));
				}
			}
			if (this.useFileAsTemplateToggle != null)
			{
				this.useFileAsTemplateToggle.gameObject.SetActive(allowUseFileAsTemplateSelect && !dir);
				if (allowUseFileAsTemplateSelect)
				{
					this.useFileAsTemplateToggle.isOn = isTemplate;
					if (isTemplateModifiable)
					{
						this.useFileAsTemplateToggle.interactable = true;
						this.useFileAsTemplateToggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnUseFileAsTemplateChange));
					}
					else
					{
						this.useFileAsTemplateToggle.interactable = false;
					}
				}
			}
		}

		// Token: 0x0400186A RID: 6250
		public Button button;

		// Token: 0x0400186B RID: 6251
		public Image buttonImage;

		// Token: 0x0400186C RID: 6252
		public Image fileIcon;

		// Token: 0x0400186D RID: 6253
		public RawImage altIcon;

		// Token: 0x0400186E RID: 6254
		public Text label;

		// Token: 0x0400186F RID: 6255
		public Sprite selectedSprite;

		// Token: 0x04001870 RID: 6256
		public Button renameButton;

		// Token: 0x04001871 RID: 6257
		public Button deleteButton;

		// Token: 0x04001872 RID: 6258
		public Toggle favoriteToggle;

		// Token: 0x04001873 RID: 6259
		public Toggle hiddenToggle;

		// Token: 0x04001874 RID: 6260
		public Toggle useFileAsTemplateToggle;

		// Token: 0x04001875 RID: 6261
		public Text fullPathLabel;

		// Token: 0x04001876 RID: 6262
		public RectTransform rectTransform;

		// Token: 0x04001877 RID: 6263
		[HideInInspector]
		public string text;

		// Token: 0x04001878 RID: 6264
		[HideInInspector]
		public string textLowerInvariant;

		// Token: 0x04001879 RID: 6265
		[HideInInspector]
		public string fullPath;

		// Token: 0x0400187A RID: 6266
		[HideInInspector]
		public string removedPrefix;

		// Token: 0x0400187B RID: 6267
		[HideInInspector]
		public bool isDir;

		// Token: 0x0400187C RID: 6268
		[HideInInspector]
		public string imgPath;

		// Token: 0x0400187D RID: 6269
		private FileBrowser browser;
	}
}
