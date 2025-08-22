using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000537 RID: 1335
	[RequireComponent(typeof(EventSystem))]
	[AddComponentMenu("Event/Extensions/Tab Navigation Helper")]
	public class TabNavigationHelper : MonoBehaviour
	{
		// Token: 0x060021F8 RID: 8696 RVA: 0x000C2435 File Offset: 0x000C0835
		public TabNavigationHelper()
		{
		}

		// Token: 0x060021F9 RID: 8697 RVA: 0x000C2440 File Offset: 0x000C0840
		private void Start()
		{
			this._system = base.GetComponent<EventSystem>();
			if (this._system == null)
			{
				Debug.LogError("Needs to be attached to the Event System component in the scene");
			}
			if (this.NavigationMode == NavigationMode.Manual && this.NavigationPath.Length > 0)
			{
				this.StartingObject = this.NavigationPath[0].gameObject.GetComponent<Selectable>();
			}
			if (this.StartingObject == null && this.CircularNavigation)
			{
				this.SelectDefaultObject(out this.StartingObject);
			}
		}

		// Token: 0x060021FA RID: 8698 RVA: 0x000C24D0 File Offset: 0x000C08D0
		public void Update()
		{
			Selectable selectable = null;
			if (this.LastObject == null && this._system.currentSelectedGameObject != null)
			{
				selectable = this._system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
				while (selectable != null)
				{
					this.LastObject = selectable;
					selectable = selectable.FindSelectableOnDown();
				}
			}
			if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift))
			{
				if (this.NavigationMode == NavigationMode.Manual && this.NavigationPath.Length > 0)
				{
					for (int i = this.NavigationPath.Length - 1; i >= 0; i--)
					{
						if (!(this._system.currentSelectedGameObject != this.NavigationPath[i].gameObject))
						{
							selectable = ((i != 0) ? this.NavigationPath[i - 1] : this.NavigationPath[this.NavigationPath.Length - 1]);
							break;
						}
					}
				}
				else if (this._system.currentSelectedGameObject != null)
				{
					selectable = this._system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
					if (selectable == null && this.CircularNavigation)
					{
						selectable = this.LastObject;
					}
				}
				else
				{
					this.SelectDefaultObject(out selectable);
				}
			}
			else if (Input.GetKeyDown(KeyCode.Tab))
			{
				if (this.NavigationMode == NavigationMode.Manual && this.NavigationPath.Length > 0)
				{
					for (int j = 0; j < this.NavigationPath.Length; j++)
					{
						if (!(this._system.currentSelectedGameObject != this.NavigationPath[j].gameObject))
						{
							selectable = ((j != this.NavigationPath.Length - 1) ? this.NavigationPath[j + 1] : this.NavigationPath[0]);
							break;
						}
					}
				}
				else if (this._system.currentSelectedGameObject != null)
				{
					selectable = this._system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
					if (selectable == null && this.CircularNavigation)
					{
						selectable = this.StartingObject;
					}
				}
				else
				{
					this.SelectDefaultObject(out selectable);
				}
			}
			else if (this._system.currentSelectedGameObject == null)
			{
				this.SelectDefaultObject(out selectable);
			}
			if (this.CircularNavigation && this.StartingObject == null)
			{
				this.StartingObject = selectable;
			}
			this.selectGameObject(selectable);
		}

		// Token: 0x060021FB RID: 8699 RVA: 0x000C277B File Offset: 0x000C0B7B
		private void SelectDefaultObject(out Selectable next)
		{
			if (this._system.firstSelectedGameObject)
			{
				next = this._system.firstSelectedGameObject.GetComponent<Selectable>();
			}
			else
			{
				next = null;
			}
		}

		// Token: 0x060021FC RID: 8700 RVA: 0x000C27AC File Offset: 0x000C0BAC
		private void selectGameObject(Selectable selectable)
		{
			if (selectable != null)
			{
				InputField component = selectable.GetComponent<InputField>();
				if (component != null)
				{
					component.OnPointerClick(new PointerEventData(this._system));
				}
				this._system.SetSelectedGameObject(selectable.gameObject, new BaseEventData(this._system));
			}
		}

		// Token: 0x04001C3E RID: 7230
		private EventSystem _system;

		// Token: 0x04001C3F RID: 7231
		private Selectable StartingObject;

		// Token: 0x04001C40 RID: 7232
		private Selectable LastObject;

		// Token: 0x04001C41 RID: 7233
		[Tooltip("The path to take when user is tabbing through ui components.")]
		public Selectable[] NavigationPath;

		// Token: 0x04001C42 RID: 7234
		[Tooltip("Use the default Unity navigation system or a manual fixed order using Navigation Path")]
		public NavigationMode NavigationMode;

		// Token: 0x04001C43 RID: 7235
		[Tooltip("If True, this will loop the tab order from last to first automatically")]
		public bool CircularNavigation;
	}
}
