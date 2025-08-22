using System;
using UnityEngine.UI;

// Token: 0x02000C52 RID: 3154
public class SubSceneUI : UIProvider
{
	// Token: 0x06005BD0 RID: 23504 RVA: 0x0021CD43 File Offset: 0x0021B143
	public SubSceneUI()
	{
	}

	// Token: 0x04004BBE RID: 19390
	public Button beginBrowseButton;

	// Token: 0x04004BBF RID: 19391
	public Toggle autoSetSubSceneUIDToSignatureOnBrowseLoadToggle;

	// Token: 0x04004BC0 RID: 19392
	public Text packageUidText;

	// Token: 0x04004BC1 RID: 19393
	public Button clearPackageUidButton;

	// Token: 0x04004BC2 RID: 19394
	public InputField creatorNameInputField;

	// Token: 0x04004BC3 RID: 19395
	public Text storedCreatorNameText;

	// Token: 0x04004BC4 RID: 19396
	public Button setToYourCreatorNameButton;

	// Token: 0x04004BC5 RID: 19397
	public InputField signatureInputField;

	// Token: 0x04004BC6 RID: 19398
	public InputField storeNameInputField;

	// Token: 0x04004BC7 RID: 19399
	public Toggle loadOnRestoreFromOtherSubSceneToggle;

	// Token: 0x04004BC8 RID: 19400
	public Button addLooseAtomsToSubSceneButton;

	// Token: 0x04004BC9 RID: 19401
	public Button isolateEditSubSceneButton;

	// Token: 0x04004BCA RID: 19402
	public Toggle drawContainedAtomsLinesToggle;

	// Token: 0x04004BCB RID: 19403
	public UIDynamicButton storeSubSceneButton;

	// Token: 0x04004BCC RID: 19404
	public UIDynamicButton loadSubSceneButton;

	// Token: 0x04004BCD RID: 19405
	public Button clearSubSceneButton;

	// Token: 0x04004BCE RID: 19406
	public Button unparentAllAtomsButton;
}
