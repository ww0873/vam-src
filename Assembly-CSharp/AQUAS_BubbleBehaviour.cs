using System;
using UnityEngine;

// Token: 0x0200000F RID: 15
public class AQUAS_BubbleBehaviour : MonoBehaviour
{
	// Token: 0x06000099 RID: 153 RVA: 0x00004C7F File Offset: 0x0000307F
	public AQUAS_BubbleBehaviour()
	{
	}

	// Token: 0x0600009A RID: 154 RVA: 0x00004C87 File Offset: 0x00003087
	private void Start()
	{
		this.maxSmallBubbleCount = UnityEngine.Random.Range(20, 30);
		this.smallBubbleCount = 0;
		this.smallBubbleBehaviour = this.smallBubble.GetComponent<AQUAS_SmallBubbleBehaviour>();
	}

	// Token: 0x0600009B RID: 155 RVA: 0x00004CB0 File Offset: 0x000030B0
	private void Update()
	{
		base.transform.Translate(Vector3.up * Time.deltaTime * this.averageUpdrift, Space.World);
		this.SmallBubbleSpawner();
		if (this.mainCamera.transform.position.y > this.waterLevel || base.transform.position.y > this.waterLevel)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x0600009C RID: 156 RVA: 0x00004D38 File Offset: 0x00003138
	private void SmallBubbleSpawner()
	{
		if (this.smallBubbleCount <= this.maxSmallBubbleCount)
		{
			this.smallBubble.transform.localScale = base.transform.localScale * UnityEngine.Random.Range(0.05f, 0.2f);
			this.smallBubbleBehaviour.averageUpdrift = this.averageUpdrift * 0.5f;
			this.smallBubbleBehaviour.waterLevel = this.waterLevel;
			this.smallBubbleBehaviour.mainCamera = this.mainCamera;
			UnityEngine.Object.Instantiate<GameObject>(this.smallBubble, new Vector3(base.transform.position.x + UnityEngine.Random.Range(-0.1f, 0.1f), base.transform.position.y - UnityEngine.Random.Range(0.01f, 1f), base.transform.position.z + UnityEngine.Random.Range(-0.1f, 0.1f)), Quaternion.identity);
			this.smallBubbleCount++;
		}
	}

	// Token: 0x04000085 RID: 133
	public float averageUpdrift;

	// Token: 0x04000086 RID: 134
	public float waterLevel;

	// Token: 0x04000087 RID: 135
	public GameObject mainCamera;

	// Token: 0x04000088 RID: 136
	public GameObject smallBubble;

	// Token: 0x04000089 RID: 137
	private int smallBubbleCount;

	// Token: 0x0400008A RID: 138
	private int maxSmallBubbleCount;

	// Token: 0x0400008B RID: 139
	private AQUAS_SmallBubbleBehaviour smallBubbleBehaviour;
}
