using System;
using UnityEngine;
using UnityEngine.UI;

namespace MVR.FileManagement
{
	// Token: 0x02000BEA RID: 3050
	public class PackageBuilderUI : UIProvider
	{
		// Token: 0x060057B3 RID: 22451 RVA: 0x00203ADD File Offset: 0x00201EDD
		public PackageBuilderUI()
		{
		}

		// Token: 0x04004829 RID: 18473
		public Button clearAllButton;

		// Token: 0x0400482A RID: 18474
		public Button loadMetaFromExistingPackageButton;

		// Token: 0x0400482B RID: 18475
		public InputField creatorField;

		// Token: 0x0400482C RID: 18476
		public InputField packageNameField;

		// Token: 0x0400482D RID: 18477
		public InputField versionField;

		// Token: 0x0400482E RID: 18478
		public Text statusText;

		// Token: 0x0400482F RID: 18479
		public Toggle showDisabledToggle;

		// Token: 0x04004830 RID: 18480
		public RectTransform packageCategoryPanel;

		// Token: 0x04004831 RID: 18481
		public RectTransform packagesContainer;

		// Token: 0x04004832 RID: 18482
		public RectTransform missingPackagesContainer;

		// Token: 0x04004833 RID: 18483
		public Button scanHubForMissingPackagesButton;

		// Token: 0x04004834 RID: 18484
		public Button selectCurrentScenePackageButton;

		// Token: 0x04004835 RID: 18485
		public Button promotionalButton;

		// Token: 0x04004836 RID: 18486
		public Text promotionalButtonText;

		// Token: 0x04004837 RID: 18487
		public Button copyPromotionalLinkButton;

		// Token: 0x04004838 RID: 18488
		public RectTransform packageReferencesContainer;

		// Token: 0x04004839 RID: 18489
		public Toggle packageEnabledToggle;

		// Token: 0x0400483A RID: 18490
		public Button deletePackageButton;

		// Token: 0x0400483B RID: 18491
		public RectTransform confirmDeletePackagePanel;

		// Token: 0x0400483C RID: 18492
		public Text confirmDeletePackageText;

		// Token: 0x0400483D RID: 18493
		public Button confirmDeletePackageButton;

		// Token: 0x0400483E RID: 18494
		public Button cancelDeletePackageButton;

		// Token: 0x0400483F RID: 18495
		public InputField userNotesField;

		// Token: 0x04004840 RID: 18496
		public Toggle pluginsAlwaysEnabledToggle;

		// Token: 0x04004841 RID: 18497
		public Toggle pluginsAlwaysDisabledToggle;

		// Token: 0x04004842 RID: 18498
		public Toggle ignoreMissingDependencyErrorsToggle;

		// Token: 0x04004843 RID: 18499
		public RectTransform packPanel;

		// Token: 0x04004844 RID: 18500
		public Slider packProgressSlider;

		// Token: 0x04004845 RID: 18501
		public Button unpackButton;

		// Token: 0x04004846 RID: 18502
		public RectTransform confirmUnpackPanel;

		// Token: 0x04004847 RID: 18503
		public Button confirmUnpackButton;

		// Token: 0x04004848 RID: 18504
		public Button cancelUnpackButton;

		// Token: 0x04004849 RID: 18505
		public Button repackButton;

		// Token: 0x0400484A RID: 18506
		public Button restoreFromOriginalButton;

		// Token: 0x0400484B RID: 18507
		public RectTransform confirmRestoreFromOriginalPanel;

		// Token: 0x0400484C RID: 18508
		public Button confirmRestoreFromOriginalButton;

		// Token: 0x0400484D RID: 18509
		public Button cancelRestoreFromOriginalButton;

		// Token: 0x0400484E RID: 18510
		public GameObject currentPackageIsOnHubIndicator;

		// Token: 0x0400484F RID: 18511
		public GameObject currentPackageIsOnHubIndicator2;

		// Token: 0x04004850 RID: 18512
		public Button openOnHubButton;

		// Token: 0x04004851 RID: 18513
		public Button openInHubDownloaderButton;

		// Token: 0x04004852 RID: 18514
		public GameObject currentPackageHasSceneIndicator;

		// Token: 0x04004853 RID: 18515
		public Button openSceneButton;

		// Token: 0x04004854 RID: 18516
		public GameObject hadReferenceIssuesIndicator;

		// Token: 0x04004855 RID: 18517
		public RectTransform contentContainer;

		// Token: 0x04004856 RID: 18518
		public Button addDirectoryButton;

		// Token: 0x04004857 RID: 18519
		public Button addFileButton;

		// Token: 0x04004858 RID: 18520
		public Button removeSelectedButton;

		// Token: 0x04004859 RID: 18521
		public Button removeAllButton;

		// Token: 0x0400485A RID: 18522
		public RectTransform referencesContainer;

		// Token: 0x0400485B RID: 18523
		public UIPopup standardReferenceVersionOptionPopup;

		// Token: 0x0400485C RID: 18524
		public UIPopup scriptReferenceVersionOptionPopup;

		// Token: 0x0400485D RID: 18525
		public UIDynamicButton prepPackageButton;

		// Token: 0x0400485E RID: 18526
		public Button fixReferencesButton;

		// Token: 0x0400485F RID: 18527
		public RectTransform licenseReportContainer;

		// Token: 0x04004860 RID: 18528
		public Text licenseReportIssueText;

		// Token: 0x04004861 RID: 18529
		public Text nonCommercialLicenseReportIssueText;

		// Token: 0x04004862 RID: 18530
		public InputField descriptionField;

		// Token: 0x04004863 RID: 18531
		public UIDynamicToggle[] customOptionToggles;

		// Token: 0x04004864 RID: 18532
		public InputField creditsField;

		// Token: 0x04004865 RID: 18533
		public InputField instructionsField;

		// Token: 0x04004866 RID: 18534
		public InputField promotionalField;

		// Token: 0x04004867 RID: 18535
		public UIPopup licensePopup;

		// Token: 0x04004868 RID: 18536
		public UIPopup secondaryLicensePopup;

		// Token: 0x04004869 RID: 18537
		public UIPopup EAYearPopup;

		// Token: 0x0400486A RID: 18538
		public UIPopup EAMonthPopup;

		// Token: 0x0400486B RID: 18539
		public UIPopup EADayPopup;

		// Token: 0x0400486C RID: 18540
		public Text licenseDescriptionText;

		// Token: 0x0400486D RID: 18541
		public RectTransform openPrepFolderInExplorerNotice;

		// Token: 0x0400486E RID: 18542
		public Button openPrepFolderInExplorerButton;

		// Token: 0x0400486F RID: 18543
		public RectTransform finalizeCheckPanel;

		// Token: 0x04004870 RID: 18544
		public RectTransform finalizingPanel;

		// Token: 0x04004871 RID: 18545
		public Button finalizeCheckCancelButton;

		// Token: 0x04004872 RID: 18546
		public Button finalizeCheckConfirmButton;

		// Token: 0x04004873 RID: 18547
		public Button finalizeButton;

		// Token: 0x04004874 RID: 18548
		public Slider finalizeProgressSlider;

		// Token: 0x04004875 RID: 18549
		public Button cancelFinalizeButton;
	}
}
