using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000AF9 RID: 2809
public class DAZMorphUI : MonoBehaviour
{
	// Token: 0x06004BB7 RID: 19383 RVA: 0x001A5D57 File Offset: 0x001A4157
	public DAZMorphUI()
	{
	}

	// Token: 0x04003A7B RID: 14971
	public Button openInPackageButton;

	// Token: 0x04003A7C RID: 14972
	public Text packageUidText;

	// Token: 0x04003A7D RID: 14973
	public Text packageLicenseText;

	// Token: 0x04003A7E RID: 14974
	public Text versionText;

	// Token: 0x04003A7F RID: 14975
	public Image panelImage;

	// Token: 0x04003A80 RID: 14976
	public Text morphNameText;

	// Token: 0x04003A81 RID: 14977
	public Slider slider;

	// Token: 0x04003A82 RID: 14978
	public Button increaseRangeButton;

	// Token: 0x04003A83 RID: 14979
	public Button resetRangeButton;

	// Token: 0x04003A84 RID: 14980
	public RectTransform animatableWarningIndicator;

	// Token: 0x04003A85 RID: 14981
	public Toggle favoriteToggle;

	// Token: 0x04003A86 RID: 14982
	public RectTransform drivenIndicator;

	// Token: 0x04003A87 RID: 14983
	public Text drivenByText;

	// Token: 0x04003A88 RID: 14984
	public RectTransform hasFormulasIndicator;

	// Token: 0x04003A89 RID: 14985
	public Button zeroKeepChildValuesButton;

	// Token: 0x04003A8A RID: 14986
	public Text formulasText;

	// Token: 0x04003A8B RID: 14987
	public Text categoryText;

	// Token: 0x04003A8C RID: 14988
	public Button copyUidButton;
}
