using System;
using UnityEngine;

// Token: 0x02000985 RID: 2437
public class HandAnimatorManagerVR : MonoBehaviour
{
	// Token: 0x06003CD3 RID: 15571 RVA: 0x00126A09 File Offset: 0x00124E09
	public HandAnimatorManagerVR()
	{
	}

	// Token: 0x06003CD4 RID: 15572 RVA: 0x00126A48 File Offset: 0x00124E48
	private void Start()
	{
		string[] joystickNames = Input.GetJoystickNames();
		foreach (string message in joystickNames)
		{
			Debug.Log(message);
		}
		this.handAnimator = base.GetComponent<Animator>();
	}

	// Token: 0x06003CD5 RID: 15573 RVA: 0x00126A88 File Offset: 0x00124E88
	private void Update()
	{
		if (Input.GetKeyUp(this.changeKey))
		{
			this.currentState = (this.currentState + 1) % (this.numberOfAnimations + 1);
		}
		if (Input.GetAxis(this.holdKey) > 0f)
		{
			this.hold = true;
		}
		else
		{
			this.hold = false;
		}
		if (Input.GetKey(this.actionKey))
		{
			this.action = true;
		}
		else
		{
			this.action = false;
		}
		if (this.lastState != this.currentState)
		{
			this.lastState = this.currentState;
			this.handAnimator.SetInteger("State", this.currentState);
			this.TurnOnState(this.currentState);
		}
		this.handAnimator.SetBool("Action", this.action);
		this.handAnimator.SetBool("Hold", this.hold);
	}

	// Token: 0x06003CD6 RID: 15574 RVA: 0x00126B74 File Offset: 0x00124F74
	private void TurnOnState(int stateNumber)
	{
		foreach (StateModel stateModel in this.stateModels)
		{
			if (stateModel.stateNumber == stateNumber && !stateModel.go.activeSelf)
			{
				stateModel.go.SetActive(true);
			}
			else if (stateModel.go.activeSelf)
			{
				stateModel.go.SetActive(false);
			}
		}
	}

	// Token: 0x04002EA7 RID: 11943
	public StateModel[] stateModels;

	// Token: 0x04002EA8 RID: 11944
	private Animator handAnimator;

	// Token: 0x04002EA9 RID: 11945
	public int currentState = 100;

	// Token: 0x04002EAA RID: 11946
	private int lastState = -1;

	// Token: 0x04002EAB RID: 11947
	public bool action;

	// Token: 0x04002EAC RID: 11948
	public bool hold;

	// Token: 0x04002EAD RID: 11949
	public string changeKey = "joystick button 9";

	// Token: 0x04002EAE RID: 11950
	public string actionKey = "joystick button 15";

	// Token: 0x04002EAF RID: 11951
	public string holdKey = "Axis 12";

	// Token: 0x04002EB0 RID: 11952
	public int numberOfAnimations = 8;
}
