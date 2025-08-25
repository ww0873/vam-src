using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D6F RID: 3439
public class HairSimControlUI : UIProvider
{
	// Token: 0x06006A1B RID: 27163 RVA: 0x0027F1FB File Offset: 0x0027D5FB
	public HairSimControlUI()
	{
	}

	// Token: 0x04005B9A RID: 23450
	public Button restoreAllFromDefaultsButton;

	// Token: 0x04005B9B RID: 23451
	public Button saveToStore1Button;

	// Token: 0x04005B9C RID: 23452
	public Button restoreFromStore1Button;

	// Token: 0x04005B9D RID: 23453
	public Button resetAndStartStyleModeButton;

	// Token: 0x04005B9E RID: 23454
	public Button startStyleModeButton;

	// Token: 0x04005B9F RID: 23455
	public Button cancelStyleModeButton;

	// Token: 0x04005BA0 RID: 23456
	public Button keepStyleButton;

	// Token: 0x04005BA1 RID: 23457
	public Toggle styleModelAllowControlOtherNodesToggle;

	// Token: 0x04005BA2 RID: 23458
	public Slider styleJointsSearchDistanceSlider;

	// Token: 0x04005BA3 RID: 23459
	public Button rebuildStyleJointsButton;

	// Token: 0x04005BA4 RID: 23460
	public Button clearStyleJointsButton;

	// Token: 0x04005BA5 RID: 23461
	public Slider styleModeGravityMultiplierSlider;

	// Token: 0x04005BA6 RID: 23462
	public Slider styleModeCollisionRadiusSlider;

	// Token: 0x04005BA7 RID: 23463
	public Slider styleModeCollisionRadiusRootSlider;

	// Token: 0x04005BA8 RID: 23464
	public Toggle styleModeShowCurlsToggle;

	// Token: 0x04005BA9 RID: 23465
	public Slider styleModeUpHairPullStrengthSlider;

	// Token: 0x04005BAA RID: 23466
	public Text simNearbyJointCountText;

	// Token: 0x04005BAB RID: 23467
	public Text styleStatusText;

	// Token: 0x04005BAC RID: 23468
	public Transform styleModePanel;

	// Token: 0x04005BAD RID: 23469
	public Toggle styleModeShowTool1Toggle;

	// Token: 0x04005BAE RID: 23470
	public Toggle styleModeShowTool2Toggle;

	// Token: 0x04005BAF RID: 23471
	public Toggle styleModeShowTool3Toggle;

	// Token: 0x04005BB0 RID: 23472
	public Toggle styleModeShowTool4Toggle;

	// Token: 0x04005BB1 RID: 23473
	public Button copyPhysicsParametersButton;

	// Token: 0x04005BB2 RID: 23474
	public Button pastePhysicsParametersButton;

	// Token: 0x04005BB3 RID: 23475
	public Button undoPastePhysicsParametersButton;

	// Token: 0x04005BB4 RID: 23476
	public Toggle simulationEnabledToggle;

	// Token: 0x04005BB5 RID: 23477
	public Toggle collisionEnabledToggle;

	// Token: 0x04005BB6 RID: 23478
	public Slider collisionRadiusSlider;

	// Token: 0x04005BB7 RID: 23479
	public Slider collisionRadiusRootSlider;

	// Token: 0x04005BB8 RID: 23480
	public Slider dragSlider;

	// Token: 0x04005BB9 RID: 23481
	public Slider weightSlider;

	// Token: 0x04005BBA RID: 23482
	public Toggle usePaintedRigidityToggle;

	// Token: 0x04005BBB RID: 23483
	public RectTransform paintedRigidityIndicatorPanel;

	// Token: 0x04005BBC RID: 23484
	public Slider rootRigiditySlider;

	// Token: 0x04005BBD RID: 23485
	public Slider mainRigiditySlider;

	// Token: 0x04005BBE RID: 23486
	public Slider tipRigiditySlider;

	// Token: 0x04005BBF RID: 23487
	public Slider rigidityRolloffPowerSlider;

	// Token: 0x04005BC0 RID: 23488
	public Slider jointRigiditySlider;

	// Token: 0x04005BC1 RID: 23489
	public Slider frictionSlider;

	// Token: 0x04005BC2 RID: 23490
	public Slider gravityMultiplierSlider;

	// Token: 0x04005BC3 RID: 23491
	public Slider iterationsSlider;

	// Token: 0x04005BC4 RID: 23492
	public Slider clingSlider;

	// Token: 0x04005BC5 RID: 23493
	public Slider clingRolloffSlider;

	// Token: 0x04005BC6 RID: 23494
	public Slider snapSlider;

	// Token: 0x04005BC7 RID: 23495
	public Slider bendResistanceSlider;

	// Token: 0x04005BC8 RID: 23496
	public Slider windXSlider;

	// Token: 0x04005BC9 RID: 23497
	public Slider windYSlider;

	// Token: 0x04005BCA RID: 23498
	public Slider windZSlider;

	// Token: 0x04005BCB RID: 23499
	public Button copyLightingParametersButton;

	// Token: 0x04005BCC RID: 23500
	public Button pasteLightingParametersButton;

	// Token: 0x04005BCD RID: 23501
	public Button undoPasteLightingParametersButton;

	// Token: 0x04005BCE RID: 23502
	public UIPopup shaderTypePopup;

	// Token: 0x04005BCF RID: 23503
	public HSVColorPicker rootColorPicker;

	// Token: 0x04005BD0 RID: 23504
	public HSVColorPicker tipColorPicker;

	// Token: 0x04005BD1 RID: 23505
	public HSVColorPicker specularColorPicker;

	// Token: 0x04005BD2 RID: 23506
	public Slider colorRolloffSlider;

	// Token: 0x04005BD3 RID: 23507
	public Slider diffuseSoftnessSlider;

	// Token: 0x04005BD4 RID: 23508
	public Slider primarySpecularSharpnessSlider;

	// Token: 0x04005BD5 RID: 23509
	public Slider secondarySpecularSharpnessSlider;

	// Token: 0x04005BD6 RID: 23510
	public Slider specularShiftSlider;

	// Token: 0x04005BD7 RID: 23511
	public Slider fresnelPowerSlider;

	// Token: 0x04005BD8 RID: 23512
	public Slider fresnelAttenuationSlider;

	// Token: 0x04005BD9 RID: 23513
	public Slider randomColorPowerSlider;

	// Token: 0x04005BDA RID: 23514
	public Slider randomColorOffsetSlider;

	// Token: 0x04005BDB RID: 23515
	public Slider IBLFactorSlider;

	// Token: 0x04005BDC RID: 23516
	public Slider normalRandomizeSlider;

	// Token: 0x04005BDD RID: 23517
	public Button copyLookParametersButton;

	// Token: 0x04005BDE RID: 23518
	public Button pasteLookParametersButton;

	// Token: 0x04005BDF RID: 23519
	public Button undoPasteLookParametersButton;

	// Token: 0x04005BE0 RID: 23520
	public Slider curlXSlider;

	// Token: 0x04005BE1 RID: 23521
	public Slider curlYSlider;

	// Token: 0x04005BE2 RID: 23522
	public Slider curlZSlider;

	// Token: 0x04005BE3 RID: 23523
	public Slider curlScaleSlider;

	// Token: 0x04005BE4 RID: 23524
	public Slider curlScaleRandomnessSlider;

	// Token: 0x04005BE5 RID: 23525
	public Slider curlFrequencySlider;

	// Token: 0x04005BE6 RID: 23526
	public Slider curlFrequencyRandomnessSlider;

	// Token: 0x04005BE7 RID: 23527
	public Toggle curlAllowReverseToggle;

	// Token: 0x04005BE8 RID: 23528
	public Toggle curlAllowFlipAxisToggle;

	// Token: 0x04005BE9 RID: 23529
	public Slider curlNormalAdjustSlider;

	// Token: 0x04005BEA RID: 23530
	public Slider curlRootSlider;

	// Token: 0x04005BEB RID: 23531
	public Slider curlMidSlider;

	// Token: 0x04005BEC RID: 23532
	public Slider curlTipSlider;

	// Token: 0x04005BED RID: 23533
	public Slider curlMidpointSlider;

	// Token: 0x04005BEE RID: 23534
	public Slider curlCurvePowerSlider;

	// Token: 0x04005BEF RID: 23535
	public Slider length1Slider;

	// Token: 0x04005BF0 RID: 23536
	public Slider length2Slider;

	// Token: 0x04005BF1 RID: 23537
	public Slider length3Slider;

	// Token: 0x04005BF2 RID: 23538
	public Slider maxSpreadSlider;

	// Token: 0x04005BF3 RID: 23539
	public Slider spreadRootSlider;

	// Token: 0x04005BF4 RID: 23540
	public Slider spreadMidSlider;

	// Token: 0x04005BF5 RID: 23541
	public Slider spreadTipSlider;

	// Token: 0x04005BF6 RID: 23542
	public Slider spreadMidpointSlider;

	// Token: 0x04005BF7 RID: 23543
	public Slider spreadCurvePowerSlider;

	// Token: 0x04005BF8 RID: 23544
	public Slider widthSlider;

	// Token: 0x04005BF9 RID: 23545
	public Slider densitySlider;

	// Token: 0x04005BFA RID: 23546
	public Slider detailSlider;
}
