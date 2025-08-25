using System;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x020004A2 RID: 1186
	public class ScrollingCalendar : MonoBehaviour
	{
		// Token: 0x06001DEC RID: 7660 RVA: 0x000AB779 File Offset: 0x000A9B79
		public ScrollingCalendar()
		{
		}

		// Token: 0x06001DED RID: 7661 RVA: 0x000AB784 File Offset: 0x000A9B84
		private void InitializeYears()
		{
			int num = int.Parse(DateTime.Now.ToString("yyyy"));
			int[] array = new int[num + 1 - 1900];
			this.yearsButtons = new GameObject[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = 1900 + i;
				GameObject gameObject = Object.Instantiate<GameObject>(this.yearsButtonPrefab, new Vector3(0f, (float)(i * 80), 0f), Quaternion.Euler(new Vector3(0f, 0f, 0f)));
				gameObject.transform.SetParent(this.yearsScrollingPanel, false);
				gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
				gameObject.GetComponentInChildren<Text>().text = string.Empty + array[i];
				gameObject.name = "Year_" + array[i];
				gameObject.AddComponent<CanvasGroup>();
				this.yearsButtons[i] = gameObject;
			}
		}

		// Token: 0x06001DEE RID: 7662 RVA: 0x000AB89C File Offset: 0x000A9C9C
		private void InitializeMonths()
		{
			int[] array = new int[12];
			this.monthsButtons = new GameObject[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				string text = string.Empty;
				array[i] = i;
				GameObject gameObject = Object.Instantiate<GameObject>(this.monthsButtonPrefab, new Vector3(0f, (float)(i * 80), 0f), Quaternion.Euler(new Vector3(0f, 0f, 0f)));
				gameObject.transform.SetParent(this.monthsScrollingPanel, false);
				gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
				switch (i)
				{
				case 0:
					text = "Jan";
					break;
				case 1:
					text = "Feb";
					break;
				case 2:
					text = "Mar";
					break;
				case 3:
					text = "Apr";
					break;
				case 4:
					text = "May";
					break;
				case 5:
					text = "Jun";
					break;
				case 6:
					text = "Jul";
					break;
				case 7:
					text = "Aug";
					break;
				case 8:
					text = "Sep";
					break;
				case 9:
					text = "Oct";
					break;
				case 10:
					text = "Nov";
					break;
				case 11:
					text = "Dec";
					break;
				}
				gameObject.GetComponentInChildren<Text>().text = text;
				gameObject.name = "Month_" + array[i];
				gameObject.AddComponent<CanvasGroup>();
				this.monthsButtons[i] = gameObject;
			}
		}

		// Token: 0x06001DEF RID: 7663 RVA: 0x000ABA3C File Offset: 0x000A9E3C
		private void InitializeDays()
		{
			int[] array = new int[31];
			this.daysButtons = new GameObject[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = i + 1;
				GameObject gameObject = Object.Instantiate<GameObject>(this.daysButtonPrefab, new Vector3(0f, (float)(i * 80), 0f), Quaternion.Euler(new Vector3(0f, 0f, 0f)));
				gameObject.transform.SetParent(this.daysScrollingPanel, false);
				gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
				gameObject.GetComponentInChildren<Text>().text = string.Empty + array[i];
				gameObject.name = "Day_" + array[i];
				gameObject.AddComponent<CanvasGroup>();
				this.daysButtons[i] = gameObject;
			}
		}

		// Token: 0x06001DF0 RID: 7664 RVA: 0x000ABB28 File Offset: 0x000A9F28
		public void Awake()
		{
			this.InitializeYears();
			this.InitializeMonths();
			this.InitializeDays();
			this.monthsVerticalScroller = new UIVerticalScroller(this.monthsScrollingPanel, this.monthsButtons, this.monthCenter);
			this.yearsVerticalScroller = new UIVerticalScroller(this.yearsScrollingPanel, this.yearsButtons, this.yearsCenter);
			this.daysVerticalScroller = new UIVerticalScroller(this.daysScrollingPanel, this.daysButtons, this.daysCenter);
			this.monthsVerticalScroller.Start();
			this.yearsVerticalScroller.Start();
			this.daysVerticalScroller.Start();
		}

		// Token: 0x06001DF1 RID: 7665 RVA: 0x000ABBC0 File Offset: 0x000A9FC0
		public void SetDate()
		{
			this.daysSet = int.Parse(this.inputFieldDays.text) - 1;
			this.monthsSet = int.Parse(this.inputFieldMonths.text) - 1;
			this.yearsSet = int.Parse(this.inputFieldYears.text) - 1900;
			this.daysVerticalScroller.SnapToElement(this.daysSet);
			this.monthsVerticalScroller.SnapToElement(this.monthsSet);
			this.yearsVerticalScroller.SnapToElement(this.yearsSet);
		}

		// Token: 0x06001DF2 RID: 7666 RVA: 0x000ABC4C File Offset: 0x000AA04C
		private void Update()
		{
			this.monthsVerticalScroller.Update();
			this.yearsVerticalScroller.Update();
			this.daysVerticalScroller.Update();
			string text = this.daysVerticalScroller.GetResults();
			string results = this.monthsVerticalScroller.GetResults();
			string results2 = this.yearsVerticalScroller.GetResults();
			if (text.EndsWith("1") && text != "11")
			{
				text += "st";
			}
			else if (text.EndsWith("2") && text != "12")
			{
				text += "nd";
			}
			else if (text.EndsWith("3") && text != "13")
			{
				text += "rd";
			}
			else
			{
				text += "th";
			}
			this.dateText.text = string.Concat(new string[]
			{
				results,
				" ",
				text,
				" ",
				results2
			});
		}

		// Token: 0x06001DF3 RID: 7667 RVA: 0x000ABD6F File Offset: 0x000AA16F
		public void DaysScrollUp()
		{
			this.daysVerticalScroller.ScrollUp();
		}

		// Token: 0x06001DF4 RID: 7668 RVA: 0x000ABD7C File Offset: 0x000AA17C
		public void DaysScrollDown()
		{
			this.daysVerticalScroller.ScrollDown();
		}

		// Token: 0x06001DF5 RID: 7669 RVA: 0x000ABD89 File Offset: 0x000AA189
		public void MonthsScrollUp()
		{
			this.monthsVerticalScroller.ScrollUp();
		}

		// Token: 0x06001DF6 RID: 7670 RVA: 0x000ABD96 File Offset: 0x000AA196
		public void MonthsScrollDown()
		{
			this.monthsVerticalScroller.ScrollDown();
		}

		// Token: 0x06001DF7 RID: 7671 RVA: 0x000ABDA3 File Offset: 0x000AA1A3
		public void YearsScrollUp()
		{
			this.yearsVerticalScroller.ScrollUp();
		}

		// Token: 0x06001DF8 RID: 7672 RVA: 0x000ABDB0 File Offset: 0x000AA1B0
		public void YearsScrollDown()
		{
			this.yearsVerticalScroller.ScrollDown();
		}

		// Token: 0x04001950 RID: 6480
		public RectTransform monthsScrollingPanel;

		// Token: 0x04001951 RID: 6481
		public RectTransform yearsScrollingPanel;

		// Token: 0x04001952 RID: 6482
		public RectTransform daysScrollingPanel;

		// Token: 0x04001953 RID: 6483
		public GameObject yearsButtonPrefab;

		// Token: 0x04001954 RID: 6484
		public GameObject monthsButtonPrefab;

		// Token: 0x04001955 RID: 6485
		public GameObject daysButtonPrefab;

		// Token: 0x04001956 RID: 6486
		private GameObject[] monthsButtons;

		// Token: 0x04001957 RID: 6487
		private GameObject[] yearsButtons;

		// Token: 0x04001958 RID: 6488
		private GameObject[] daysButtons;

		// Token: 0x04001959 RID: 6489
		public RectTransform monthCenter;

		// Token: 0x0400195A RID: 6490
		public RectTransform yearsCenter;

		// Token: 0x0400195B RID: 6491
		public RectTransform daysCenter;

		// Token: 0x0400195C RID: 6492
		private UIVerticalScroller yearsVerticalScroller;

		// Token: 0x0400195D RID: 6493
		private UIVerticalScroller monthsVerticalScroller;

		// Token: 0x0400195E RID: 6494
		private UIVerticalScroller daysVerticalScroller;

		// Token: 0x0400195F RID: 6495
		public InputField inputFieldDays;

		// Token: 0x04001960 RID: 6496
		public InputField inputFieldMonths;

		// Token: 0x04001961 RID: 6497
		public InputField inputFieldYears;

		// Token: 0x04001962 RID: 6498
		public Text dateText;

		// Token: 0x04001963 RID: 6499
		private int daysSet;

		// Token: 0x04001964 RID: 6500
		private int monthsSet;

		// Token: 0x04001965 RID: 6501
		private int yearsSet;
	}
}
