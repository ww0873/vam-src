using System;
using UnityEngine;

// Token: 0x02000983 RID: 2435
public class HandAnimatorManager : MonoBehaviour
{
	// Token: 0x06003CCE RID: 15566 RVA: 0x001267CD File Offset: 0x00124BCD
	public HandAnimatorManager()
	{
	}

	// Token: 0x06003CCF RID: 15567 RVA: 0x001267E4 File Offset: 0x00124BE4
	private void Start()
	{
		this.handAnimator = base.GetComponent<Animator>();
	}

	// Token: 0x06003CD0 RID: 15568 RVA: 0x001267F4 File Offset: 0x00124BF4
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.BackQuote))
		{
			this.currentState = 0;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			this.currentState = 1;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			this.currentState = 2;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			this.currentState = 3;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			this.currentState = 4;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			this.currentState = 5;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha6))
		{
			this.currentState = 6;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha7))
		{
			this.currentState = 7;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha8))
		{
			this.currentState = 8;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha9))
		{
			this.currentState = 9;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha0))
		{
			this.currentState = 10;
		}
		else if (Input.GetKeyDown(KeyCode.I))
		{
			this.currentState = 100;
		}
		if (this.lastState != this.currentState)
		{
			this.lastState = this.currentState;
			this.handAnimator.SetInteger("State", this.currentState);
			this.TurnOnState(this.currentState);
		}
		this.handAnimator.SetBool("Action", Input.GetMouseButton(0));
		this.handAnimator.SetBool("Hold", Input.GetMouseButton(1));
	}

	// Token: 0x06003CD1 RID: 15569 RVA: 0x0012698C File Offset: 0x00124D8C
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

	// Token: 0x04002EA1 RID: 11937
	public StateModel[] stateModels;

	// Token: 0x04002EA2 RID: 11938
	private Animator handAnimator;

	// Token: 0x04002EA3 RID: 11939
	public int currentState = 100;

	// Token: 0x04002EA4 RID: 11940
	private int lastState = -1;
}
