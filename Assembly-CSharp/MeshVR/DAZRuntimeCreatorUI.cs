using System;
using UnityEngine;
using UnityEngine.UI;

namespace MeshVR
{
	// Token: 0x02000AFC RID: 2812
	public class DAZRuntimeCreatorUI : PresetManagerControlUI
	{
		// Token: 0x06004C7A RID: 19578 RVA: 0x001AE489 File Offset: 0x001AC889
		public DAZRuntimeCreatorUI()
		{
		}

		// Token: 0x04003AF0 RID: 15088
		public Text dufFileUrlText;

		// Token: 0x04003AF1 RID: 15089
		public Button dufFileBrowseButton;

		// Token: 0x04003AF2 RID: 15090
		public Button clearButton;

		// Token: 0x04003AF3 RID: 15091
		public Button importButton;

		// Token: 0x04003AF4 RID: 15092
		public Button cancelButton;

		// Token: 0x04003AF5 RID: 15093
		public Text importVertexCountText;

		// Token: 0x04003AF6 RID: 15094
		public Toggle combineMaterialsToggle;

		// Token: 0x04003AF7 RID: 15095
		public Toggle wrapToMorphedVerticesToggle;

		// Token: 0x04003AF8 RID: 15096
		public Toggle disableAnatomyToggle;

		// Token: 0x04003AF9 RID: 15097
		public Text importMessageText;

		// Token: 0x04003AFA RID: 15098
		public Text simVertexCountText;

		// Token: 0x04003AFB RID: 15099
		public Text simJointCountText;

		// Token: 0x04003AFC RID: 15100
		public Text simNearbyJointCountText;

		// Token: 0x04003AFD RID: 15101
		public Button generateSimButton;

		// Token: 0x04003AFE RID: 15102
		public Button cancelGenerateSimButton;

		// Token: 0x04003AFF RID: 15103
		public Button selectClothSimTextureButton;

		// Token: 0x04003B00 RID: 15104
		public Button clearClothSimTextureButton;

		// Token: 0x04003B01 RID: 15105
		public Slider uniformClothSimTextureValueSlider;

		// Token: 0x04003B02 RID: 15106
		public Button setUniformClothSimTextureButton;

		// Token: 0x04003B03 RID: 15107
		public RawImage clothSimTextureRawImage;

		// Token: 0x04003B04 RID: 15108
		public Toggle clothSimUseIndividualSimTexturesToggle;

		// Token: 0x04003B05 RID: 15109
		public Toggle clothSimEnabledToggle;

		// Token: 0x04003B06 RID: 15110
		public Toggle clothSimCreateNearbyJointsToggle;

		// Token: 0x04003B07 RID: 15111
		public Slider clothSimNearbyJointsDistanceSlider;

		// Token: 0x04003B08 RID: 15112
		public UIPopup scalpChooserPopup;

		// Token: 0x04003B09 RID: 15113
		public Slider hairSimSegmentsSlider;

		// Token: 0x04003B0A RID: 15114
		public Slider hairSimSegmentsLengthSlider;

		// Token: 0x04003B0B RID: 15115
		public Slider scalpMaskSelectableSizeSlider;

		// Token: 0x04003B0C RID: 15116
		public Button scalpMaskSetAllButton;

		// Token: 0x04003B0D RID: 15117
		public Button scalpMaskClearAllButton;

		// Token: 0x04003B0E RID: 15118
		public Button startScalpMaskEditModeButton;

		// Token: 0x04003B0F RID: 15119
		public Button cancelScalpMaskEditModeButton;

		// Token: 0x04003B10 RID: 15120
		public Button finishScalpMaskEditModeButton;

		// Token: 0x04003B11 RID: 15121
		public Toggle scalpMaskEditModeHideBackfacesToggle;

		// Token: 0x04003B12 RID: 15122
		public Toggle autoSetStoreFolderNameToDufToggle;

		// Token: 0x04003B13 RID: 15123
		public InputField storeFolderNameField;

		// Token: 0x04003B14 RID: 15124
		public Toggle autoSetStoreNameToDufToggle;

		// Token: 0x04003B15 RID: 15125
		public Text packageNameText;

		// Token: 0x04003B16 RID: 15126
		public Button clearPackageButton;

		// Token: 0x04003B17 RID: 15127
		public InputField storeNameField;

		// Token: 0x04003B18 RID: 15128
		public InputField displayNameField;

		// Token: 0x04003B19 RID: 15129
		public InputField creatorNameField;

		// Token: 0x04003B1A RID: 15130
		public InputField tagsField;

		// Token: 0x04003B1B RID: 15131
		public RectTransform tagsPanel;

		// Token: 0x04003B1C RID: 15132
		public RectTransform regionTagsContent;

		// Token: 0x04003B1D RID: 15133
		public RectTransform typeTagsContent;

		// Token: 0x04003B1E RID: 15134
		public RectTransform otherTagsContent;

		// Token: 0x04003B1F RID: 15135
		public Button openTagsPanelButton;

		// Token: 0x04003B20 RID: 15136
		public Button closeTagsPanelButton;

		// Token: 0x04003B21 RID: 15137
		public Button browseStoreButton;

		// Token: 0x04003B22 RID: 15138
		public UIDynamicButton storeButton;

		// Token: 0x04003B23 RID: 15139
		public UIDynamicButton loadButton;

		// Token: 0x04003B24 RID: 15140
		public Button getScreenshotButton;

		// Token: 0x04003B25 RID: 15141
		public RawImage thumbnailRawImage;

		// Token: 0x04003B26 RID: 15142
		public Text storedCreatorNameText;

		// Token: 0x04003B27 RID: 15143
		public Text creatorStatusText;
	}
}
