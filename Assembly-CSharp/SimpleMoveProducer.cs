using System;
using UnityEngine;

// Token: 0x02000B72 RID: 2930
public class SimpleMoveProducer : MoveProducer
{
	// Token: 0x06005238 RID: 21048 RVA: 0x001DA956 File Offset: 0x001D8D56
	public SimpleMoveProducer()
	{
	}

	// Token: 0x06005239 RID: 21049 RVA: 0x001DA970 File Offset: 0x001D8D70
	protected override void Update()
	{
		if (this.positions != null && this.positions.Length > 1)
		{
			SimpleMoveProducer.State state = this.currentState;
			SimpleMoveProducer.State state2 = this.currentState;
			if (state2 == SimpleMoveProducer.State.Pause)
			{
				if (this.pauseTimer > 0f)
				{
					this.pauseTimer -= Time.deltaTime;
				}
				else
				{
					this.pauseTimer = 0f;
					if (this.reversed)
					{
						if (this.index == 0)
						{
							this.reversed = false;
							this.index++;
						}
						else
						{
							this.index--;
						}
						state = SimpleMoveProducer.State.Transition;
						this.transitionTimer = this.transitionTime;
					}
					else if (this.index == this.positions.Length)
					{
						if (this.loop)
						{
							if (this.reverseAtEnd)
							{
								this.reversed = true;
								this.index--;
								this.transitionTimer = this.transitionTime;
							}
						}
						else
						{
							state = SimpleMoveProducer.State.Complete;
						}
					}
					else
					{
						this.index++;
					}
				}
			}
			this.currentState = state;
		}
	}

	// Token: 0x040041E7 RID: 16871
	public Vector3[] positions;

	// Token: 0x040041E8 RID: 16872
	public Quaternion[] rotations1;

	// Token: 0x040041E9 RID: 16873
	public float[] pauseTimes;

	// Token: 0x040041EA RID: 16874
	public float transitionTime = 1f;

	// Token: 0x040041EB RID: 16875
	public bool loop = true;

	// Token: 0x040041EC RID: 16876
	public bool reverseAtEnd;

	// Token: 0x040041ED RID: 16877
	protected bool reversed;

	// Token: 0x040041EE RID: 16878
	protected int index;

	// Token: 0x040041EF RID: 16879
	protected float pauseTimer;

	// Token: 0x040041F0 RID: 16880
	protected float transitionTimer;

	// Token: 0x040041F1 RID: 16881
	protected SimpleMoveProducer.State currentState;

	// Token: 0x02000B73 RID: 2931
	protected enum State
	{
		// Token: 0x040041F3 RID: 16883
		Pause,
		// Token: 0x040041F4 RID: 16884
		Transition,
		// Token: 0x040041F5 RID: 16885
		Complete
	}
}
