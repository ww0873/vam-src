using System;
using OldMoatGames;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000005 RID: 5
public class CodeExample : MonoBehaviour
{
	// Token: 0x0600003B RID: 59 RVA: 0x00002BE9 File Offset: 0x00000FE9
	public CodeExample()
	{
	}

	// Token: 0x0600003C RID: 60 RVA: 0x00002BF4 File Offset: 0x00000FF4
	public void Awake()
	{
		this.AnimatedGifPlayer = base.GetComponent<AnimatedGifPlayer>();
		this.AnimatedGifPlayer.FileName = "AnimatedGIFPlayerExampe 3.gif";
		this.AnimatedGifPlayer.AutoPlay = false;
		this.AnimatedGifPlayer.OnReady += this.OnGifLoaded;
		this.AnimatedGifPlayer.OnLoadError += this.OnGifLoadError;
		this.AnimatedGifPlayer.Init();
	}

	// Token: 0x0600003D RID: 61 RVA: 0x00002C64 File Offset: 0x00001064
	private void OnGifLoaded()
	{
		this.PlayButton.interactable = true;
		Debug.Log(string.Concat(new object[]
		{
			"GIF size: width: ",
			this.AnimatedGifPlayer.Width,
			"px, height: ",
			this.AnimatedGifPlayer.Height,
			" px"
		}));
	}

	// Token: 0x0600003E RID: 62 RVA: 0x00002CCB File Offset: 0x000010CB
	private void OnGifLoadError()
	{
		Debug.Log("Error Loading GIF");
	}

	// Token: 0x0600003F RID: 63 RVA: 0x00002CD7 File Offset: 0x000010D7
	public void Play()
	{
		this.AnimatedGifPlayer.Play();
		this.PlayButton.interactable = false;
		this.PauseButton.interactable = true;
	}

	// Token: 0x06000040 RID: 64 RVA: 0x00002CFC File Offset: 0x000010FC
	public void Pause()
	{
		this.AnimatedGifPlayer.Pause();
		this.PlayButton.interactable = true;
		this.PauseButton.interactable = false;
	}

	// Token: 0x06000041 RID: 65 RVA: 0x00002D21 File Offset: 0x00001121
	public void OnDisable()
	{
		this.AnimatedGifPlayer.OnReady -= this.Play;
	}

	// Token: 0x04000029 RID: 41
	private AnimatedGifPlayer AnimatedGifPlayer;

	// Token: 0x0400002A RID: 42
	public Button PlayButton;

	// Token: 0x0400002B RID: 43
	public Button PauseButton;
}
