using System;
using UnityEngine;
using Valve.VR;

// Token: 0x02000E2C RID: 3628
public class ViveTrackedControllers : MonoBehaviour
{
	// Token: 0x06006F9F RID: 28575 RVA: 0x002A139C File Offset: 0x0029F79C
	public ViveTrackedControllers()
	{
	}

	// Token: 0x1700104D RID: 4173
	// (get) Token: 0x06006FA0 RID: 28576 RVA: 0x002A1438 File Offset: 0x0029F838
	public bool leftTouchedThisFrame
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700104E RID: 4174
	// (get) Token: 0x06006FA1 RID: 28577 RVA: 0x002A143B File Offset: 0x0029F83B
	public bool rightTouchedThisFrame
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700104F RID: 4175
	// (get) Token: 0x06006FA2 RID: 28578 RVA: 0x002A143E File Offset: 0x0029F83E
	public bool leftUntouchedThisFrame
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17001050 RID: 4176
	// (get) Token: 0x06006FA3 RID: 28579 RVA: 0x002A1441 File Offset: 0x0029F841
	public bool rightUntouchedThisFrame
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17001051 RID: 4177
	// (get) Token: 0x06006FA4 RID: 28580 RVA: 0x002A1444 File Offset: 0x0029F844
	public bool leftTouching
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17001052 RID: 4178
	// (get) Token: 0x06006FA5 RID: 28581 RVA: 0x002A1447 File Offset: 0x0029F847
	public bool rightTouching
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17001053 RID: 4179
	// (get) Token: 0x06006FA6 RID: 28582 RVA: 0x002A144A File Offset: 0x0029F84A
	public bool bothTouching
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17001054 RID: 4180
	// (get) Token: 0x06006FA7 RID: 28583 RVA: 0x002A144D File Offset: 0x0029F84D
	public bool leftGrippedThisFrame
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17001055 RID: 4181
	// (get) Token: 0x06006FA8 RID: 28584 RVA: 0x002A1450 File Offset: 0x0029F850
	public bool rightGrippedThisFrame
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17001056 RID: 4182
	// (get) Token: 0x06006FA9 RID: 28585 RVA: 0x002A1453 File Offset: 0x0029F853
	public bool leftUngrippedThisFrame
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17001057 RID: 4183
	// (get) Token: 0x06006FAA RID: 28586 RVA: 0x002A1456 File Offset: 0x0029F856
	public bool rightUngrippedThisFrame
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17001058 RID: 4184
	// (get) Token: 0x06006FAB RID: 28587 RVA: 0x002A1459 File Offset: 0x0029F859
	public bool leftGripping
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17001059 RID: 4185
	// (get) Token: 0x06006FAC RID: 28588 RVA: 0x002A145C File Offset: 0x0029F85C
	public bool rightGripping
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700105A RID: 4186
	// (get) Token: 0x06006FAD RID: 28589 RVA: 0x002A145F File Offset: 0x0029F85F
	public bool leftTouchpadPressedThisFrame
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700105B RID: 4187
	// (get) Token: 0x06006FAE RID: 28590 RVA: 0x002A1462 File Offset: 0x0029F862
	public bool rightTouchpadPressedThisFrame
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700105C RID: 4188
	// (get) Token: 0x06006FAF RID: 28591 RVA: 0x002A1465 File Offset: 0x0029F865
	public bool leftTouchpadUnpressedThisFrame
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700105D RID: 4189
	// (get) Token: 0x06006FB0 RID: 28592 RVA: 0x002A1468 File Offset: 0x0029F868
	public bool rightTouchpadUnpressedThisFrame
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700105E RID: 4190
	// (get) Token: 0x06006FB1 RID: 28593 RVA: 0x002A146B File Offset: 0x0029F86B
	public bool leftTouchpadPressing
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700105F RID: 4191
	// (get) Token: 0x06006FB2 RID: 28594 RVA: 0x002A146E File Offset: 0x0029F86E
	public bool rightTouchpadPressing
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17001060 RID: 4192
	// (get) Token: 0x06006FB3 RID: 28595 RVA: 0x002A1471 File Offset: 0x0029F871
	public bool leftMenuPressedThisFrame
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17001061 RID: 4193
	// (get) Token: 0x06006FB4 RID: 28596 RVA: 0x002A1474 File Offset: 0x0029F874
	public bool rightMenuPressedThisFrame
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17001062 RID: 4194
	// (get) Token: 0x06006FB5 RID: 28597 RVA: 0x002A1477 File Offset: 0x0029F877
	public bool leftMenuUnpressedThisFrame
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17001063 RID: 4195
	// (get) Token: 0x06006FB6 RID: 28598 RVA: 0x002A147A File Offset: 0x0029F87A
	public bool rightMenuUnpressedThisFrame
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17001064 RID: 4196
	// (get) Token: 0x06006FB7 RID: 28599 RVA: 0x002A147D File Offset: 0x0029F87D
	public bool leftMenuPressing
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17001065 RID: 4197
	// (get) Token: 0x06006FB8 RID: 28600 RVA: 0x002A1480 File Offset: 0x0029F880
	public bool rightMenuPressing
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17001066 RID: 4198
	// (get) Token: 0x06006FB9 RID: 28601 RVA: 0x002A1483 File Offset: 0x0029F883
	public bool leftTriggerPressing
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17001067 RID: 4199
	// (get) Token: 0x06006FBA RID: 28602 RVA: 0x002A1486 File Offset: 0x0029F886
	public bool rightTriggerPressing
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17001068 RID: 4200
	// (get) Token: 0x06006FBB RID: 28603 RVA: 0x002A1489 File Offset: 0x0029F889
	public bool leftTriggerPressedThisFrame
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17001069 RID: 4201
	// (get) Token: 0x06006FBC RID: 28604 RVA: 0x002A148C File Offset: 0x0029F88C
	public bool rightTriggerPressedThisFrame
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700106A RID: 4202
	// (get) Token: 0x06006FBD RID: 28605 RVA: 0x002A148F File Offset: 0x0029F88F
	public bool leftTriggerUnpressedThisFrame
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700106B RID: 4203
	// (get) Token: 0x06006FBE RID: 28606 RVA: 0x002A1492 File Offset: 0x0029F892
	public bool rightTriggerUnpressedThisFrame
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700106C RID: 4204
	// (get) Token: 0x06006FBF RID: 28607 RVA: 0x002A1495 File Offset: 0x0029F895
	public float leftTriggerValue
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700106D RID: 4205
	// (get) Token: 0x06006FC0 RID: 28608 RVA: 0x002A149C File Offset: 0x0029F89C
	public float rightTriggerValue
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700106E RID: 4206
	// (get) Token: 0x06006FC1 RID: 28609 RVA: 0x002A14A3 File Offset: 0x0029F8A3
	public bool leftTriggerFullClickThisFrame
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700106F RID: 4207
	// (get) Token: 0x06006FC2 RID: 28610 RVA: 0x002A14A6 File Offset: 0x0029F8A6
	public bool leftTriggerFullUnclickThisFrame
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17001070 RID: 4208
	// (get) Token: 0x06006FC3 RID: 28611 RVA: 0x002A14A9 File Offset: 0x0029F8A9
	public bool rightTriggerFullClickThisFrame
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17001071 RID: 4209
	// (get) Token: 0x06006FC4 RID: 28612 RVA: 0x002A14AC File Offset: 0x0029F8AC
	public bool rightTriggerFullUnclickThisFrame
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17001072 RID: 4210
	// (get) Token: 0x06006FC5 RID: 28613 RVA: 0x002A14AF File Offset: 0x0029F8AF
	public bool rightTriggerIsFullClicking
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17001073 RID: 4211
	// (get) Token: 0x06006FC6 RID: 28614 RVA: 0x002A14B2 File Offset: 0x0029F8B2
	public bool leftTriggerIsFullClicking
	{
		get
		{
			return this._leftTriggerIsFullClicking;
		}
	}

	// Token: 0x17001074 RID: 4212
	// (get) Token: 0x06006FC7 RID: 28615 RVA: 0x002A14BA File Offset: 0x0029F8BA
	public Vector2 leftTouchDownPosition
	{
		get
		{
			return this._leftTouchDownPosition;
		}
	}

	// Token: 0x17001075 RID: 4213
	// (get) Token: 0x06006FC8 RID: 28616 RVA: 0x002A14C2 File Offset: 0x0029F8C2
	public ViveTrackedControllers.TouchpadDirection leftPressDownTouchpadDirection
	{
		get
		{
			return this._leftPressDownTouchpadDirection;
		}
	}

	// Token: 0x17001076 RID: 4214
	// (get) Token: 0x06006FC9 RID: 28617 RVA: 0x002A14CA File Offset: 0x0029F8CA
	public Vector2 rightTouchDownPosition
	{
		get
		{
			return this._rightTouchDownPosition;
		}
	}

	// Token: 0x17001077 RID: 4215
	// (get) Token: 0x06006FCA RID: 28618 RVA: 0x002A14D2 File Offset: 0x0029F8D2
	public ViveTrackedControllers.TouchpadDirection rightPressDownTouchpadDirection
	{
		get
		{
			return this._rightPressDownTouchpadDirection;
		}
	}

	// Token: 0x17001078 RID: 4216
	// (get) Token: 0x06006FCB RID: 28619 RVA: 0x002A14DA File Offset: 0x0029F8DA
	public Vector2 leftTouchPosition
	{
		get
		{
			return this._leftTouchPosition;
		}
	}

	// Token: 0x17001079 RID: 4217
	// (get) Token: 0x06006FCC RID: 28620 RVA: 0x002A14E2 File Offset: 0x0029F8E2
	public Vector2 rightTouchPosition
	{
		get
		{
			return this._rightTouchPosition;
		}
	}

	// Token: 0x1700107A RID: 4218
	// (get) Token: 0x06006FCD RID: 28621 RVA: 0x002A14EA File Offset: 0x0029F8EA
	public ViveTrackedControllers.TouchpadDirection leftTouchpadDirection
	{
		get
		{
			return this._leftTouchpadDirection;
		}
	}

	// Token: 0x1700107B RID: 4219
	// (get) Token: 0x06006FCE RID: 28622 RVA: 0x002A14F2 File Offset: 0x0029F8F2
	public ViveTrackedControllers.TouchpadDirection rightTouchpadDirection
	{
		get
		{
			return this._rightTouchpadDirection;
		}
	}

	// Token: 0x06006FCF RID: 28623 RVA: 0x002A14FC File Offset: 0x0029F8FC
	public Vector2 GetLeftTouchDelta(bool hapticFeedback = true, ViveTrackedControllers.HapticAxis hapticAxis = ViveTrackedControllers.HapticAxis.X)
	{
		Vector2 result;
		if (this.leftTouching)
		{
			result.x = this._leftTouchPosition.x - this._leftLastTouchPosition.x;
			result.y = this._leftTouchPosition.y - this._leftLastTouchPosition.y;
		}
		else
		{
			result.x = 0f;
			result.y = 0f;
		}
		return result;
	}

	// Token: 0x06006FD0 RID: 28624 RVA: 0x002A1570 File Offset: 0x0029F970
	public Vector2 GetRightTouchDelta(bool hapticFeedback = true, ViveTrackedControllers.HapticAxis hapticAxis = ViveTrackedControllers.HapticAxis.X)
	{
		Vector2 result;
		if (this.rightTouching)
		{
			result.x = this._rightTouchPosition.x - this._rightLastTouchPosition.x;
			result.y = this._rightTouchPosition.y - this._rightLastTouchPosition.y;
		}
		else
		{
			result.x = 0f;
			result.y = 0f;
		}
		return result;
	}

	// Token: 0x06006FD1 RID: 28625 RVA: 0x002A15E2 File Offset: 0x0029F9E2
	private void ProcessViveControllers()
	{
	}

	// Token: 0x06006FD2 RID: 28626 RVA: 0x002A15E4 File Offset: 0x0029F9E4
	public int GetLeftTouchScroll(bool hapticFeedback = true)
	{
		return 0;
	}

	// Token: 0x06006FD3 RID: 28627 RVA: 0x002A15F4 File Offset: 0x0029F9F4
	public int GetRightTouchScroll(bool hapticFeedback = true)
	{
		return 0;
	}

	// Token: 0x06006FD4 RID: 28628 RVA: 0x002A1604 File Offset: 0x0029FA04
	public void HideLeftController()
	{
	}

	// Token: 0x06006FD5 RID: 28629 RVA: 0x002A1606 File Offset: 0x0029FA06
	public void ShowLeftController()
	{
	}

	// Token: 0x06006FD6 RID: 28630 RVA: 0x002A1608 File Offset: 0x0029FA08
	public void HideRightController()
	{
	}

	// Token: 0x06006FD7 RID: 28631 RVA: 0x002A160A File Offset: 0x0029FA0A
	public void ShowRightController()
	{
	}

	// Token: 0x06006FD8 RID: 28632 RVA: 0x002A160C File Offset: 0x0029FA0C
	private void Update()
	{
		this.ProcessViveControllers();
	}

	// Token: 0x06006FD9 RID: 28633 RVA: 0x002A1614 File Offset: 0x0029FA14
	private void Awake()
	{
		ViveTrackedControllers.singleton = this;
	}

	// Token: 0x04006174 RID: 24948
	public static ViveTrackedControllers singleton;

	// Token: 0x04006175 RID: 24949
	public SteamVR_TrackedObject viveObjectLeft;

	// Token: 0x04006176 RID: 24950
	public SteamVR_TrackedObject viveObjectRight;

	// Token: 0x04006177 RID: 24951
	public float scrollClick = 0.25f;

	// Token: 0x04006178 RID: 24952
	private GameObject viveObjectLeftModelGO;

	// Token: 0x04006179 RID: 24953
	private GameObject viveObjectLeftCanvasGO;

	// Token: 0x0400617A RID: 24954
	private GameObject viveObjectRightModelGO;

	// Token: 0x0400617B RID: 24955
	private GameObject viveObjectRightCanvasGO;

	// Token: 0x0400617C RID: 24956
	private float _lastLeftTriggerValue;

	// Token: 0x0400617D RID: 24957
	private float _lastRightTriggerValue;

	// Token: 0x0400617E RID: 24958
	private bool _leftTriggerIsFullClicking;

	// Token: 0x0400617F RID: 24959
	private Vector2 _leftTouchDownPosition = new Vector2(0f, 0f);

	// Token: 0x04006180 RID: 24960
	private ViveTrackedControllers.TouchpadDirection _leftPressDownTouchpadDirection;

	// Token: 0x04006181 RID: 24961
	private Vector2 _rightTouchDownPosition = new Vector2(0f, 0f);

	// Token: 0x04006182 RID: 24962
	private ViveTrackedControllers.TouchpadDirection _rightPressDownTouchpadDirection;

	// Token: 0x04006183 RID: 24963
	private Vector2 _leftTouchPosition = new Vector2(0f, 0f);

	// Token: 0x04006184 RID: 24964
	private Vector2 _rightTouchPosition = new Vector2(0f, 0f);

	// Token: 0x04006185 RID: 24965
	private ViveTrackedControllers.TouchpadDirection _leftTouchpadDirection;

	// Token: 0x04006186 RID: 24966
	private ViveTrackedControllers.TouchpadDirection _rightTouchpadDirection;

	// Token: 0x04006187 RID: 24967
	private Vector2 _leftLastTouchPosition = new Vector2(0f, 0f);

	// Token: 0x04006188 RID: 24968
	private Vector2 _rightLastTouchPosition = new Vector2(0f, 0f);

	// Token: 0x04006189 RID: 24969
	private Vector2 _leftScrollPosition;

	// Token: 0x0400618A RID: 24970
	private Vector2 _rightScrollPosition;

	// Token: 0x02000E2D RID: 3629
	public enum TouchpadDirection
	{
		// Token: 0x0400618C RID: 24972
		None,
		// Token: 0x0400618D RID: 24973
		Up,
		// Token: 0x0400618E RID: 24974
		Down,
		// Token: 0x0400618F RID: 24975
		Right,
		// Token: 0x04006190 RID: 24976
		Left
	}

	// Token: 0x02000E2E RID: 3630
	public enum HapticAxis
	{
		// Token: 0x04006192 RID: 24978
		X,
		// Token: 0x04006193 RID: 24979
		Y,
		// Token: 0x04006194 RID: 24980
		XY
	}
}
