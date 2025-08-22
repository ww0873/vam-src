using System;
using UnityEngine;

// Token: 0x0200043E RID: 1086
[Serializable]
public class RhythmTool
{
	// Token: 0x06001AFC RID: 6908 RVA: 0x00095B38 File Offset: 0x00093F38
	public RhythmTool()
	{
	}

	// Token: 0x170002F9 RID: 761
	// (get) Token: 0x06001AFD RID: 6909 RVA: 0x00095B4B File Offset: 0x00093F4B
	public int TotalFrames
	{
		get
		{
			return this.totalFrames;
		}
	}

	// Token: 0x170002FA RID: 762
	// (get) Token: 0x06001AFE RID: 6910 RVA: 0x00095B53 File Offset: 0x00093F53
	public int CurrentFrame
	{
		get
		{
			return this.currentFrame;
		}
	}

	// Token: 0x170002FB RID: 763
	// (get) Token: 0x06001AFF RID: 6911 RVA: 0x00095B5B File Offset: 0x00093F5B
	public float Interpolation
	{
		get
		{
			return this.interpolation;
		}
	}

	// Token: 0x170002FC RID: 764
	// (get) Token: 0x06001B00 RID: 6912 RVA: 0x00095B63 File Offset: 0x00093F63
	public int LastFrame
	{
		get
		{
			return this.lastFrame;
		}
	}

	// Token: 0x170002FD RID: 765
	// (get) Token: 0x06001B01 RID: 6913 RVA: 0x00095B6B File Offset: 0x00093F6B
	// (set) Token: 0x06001B02 RID: 6914 RVA: 0x00095B73 File Offset: 0x00093F73
	public int Lead
	{
		get
		{
			return this.lead;
		}
		set
		{
			this.lead = Mathf.Max(this.lead, 40);
		}
	}

	// Token: 0x170002FE RID: 766
	// (get) Token: 0x06001B03 RID: 6915 RVA: 0x00095B88 File Offset: 0x00093F88
	public bool IsDone
	{
		get
		{
			return this.isDone;
		}
	}

	// Token: 0x170002FF RID: 767
	// (get) Token: 0x06001B04 RID: 6916 RVA: 0x00095B90 File Offset: 0x00093F90
	public bool Initialized
	{
		get
		{
			return this.initialized;
		}
	}

	// Token: 0x06001B05 RID: 6917 RVA: 0x00095B98 File Offset: 0x00093F98
	public AudioSource Init(MonoBehaviour script, bool preCalculate)
	{
		this.preCalculate = preCalculate;
		this.floatSamples = new float[4096];
		this.doubleSamples = new double[4096];
		this.magnitude = new float[2047];
		this.fft = new LomontFFT();
		this.audioSource = script.gameObject.GetComponent<AudioSource>();
		if (this.audioSource == null)
		{
			this.audioSource = script.gameObject.AddComponent<AudioSource>();
		}
		if (!this.advancedAnalyses)
		{
			this.analyses = new Analysis[3];
			Analysis analysis = new Analysis();
			analysis.start = 0;
			analysis.end = 12;
			analysis.name = "Low";
			this.analyses[0] = analysis;
			analysis = new Analysis();
			analysis.start = 30;
			analysis.end = 200;
			analysis.name = "Mid";
			this.analyses[1] = analysis;
			analysis = new Analysis();
			analysis.start = 300;
			analysis.end = 550;
			analysis.name = "High";
			this.analyses[2] = analysis;
		}
		if (this.analyses.Length <= 0)
		{
			Debug.LogWarning("No analysis configured");
			return null;
		}
		this.initialized = true;
		return this.audioSource;
	}

	// Token: 0x06001B06 RID: 6918 RVA: 0x00095CE0 File Offset: 0x000940E0
	public bool NewSong(string songPath)
	{
		if (!this.initialized)
		{
			return false;
		}
		this.audioSource.Stop();
		this.audioSource.clip = Mp3Importer.Import(songPath);
		this.totalSamples = this.audioSource.clip.samples;
		this.totalFrames = this.totalSamples / 1500;
		this.currentFrame = 0;
		foreach (Analysis analysis in this.analyses)
		{
			analysis.Init(this.totalFrames, this.advancedAnalyses);
		}
		this.isDone = false;
		this.lastFrame = 0;
		this.initialized = true;
		return true;
	}

	// Token: 0x06001B07 RID: 6919 RVA: 0x00095D8C File Offset: 0x0009418C
	public bool NewSong(AudioClip audioClip)
	{
		if (!this.initialized)
		{
			return false;
		}
		this.audioSource.Stop();
		this.audioSource.clip = audioClip;
		this.audioSource.time = 0f;
		this.totalSamples = this.audioSource.clip.samples;
		this.totalFrames = this.totalSamples / 1500;
		this.currentFrame = 0;
		foreach (Analysis analysis in this.analyses)
		{
			analysis.Init(this.totalFrames, this.advancedAnalyses);
		}
		this.isDone = false;
		this.lastFrame = 0;
		this.initialized = true;
		return true;
	}

	// Token: 0x17000300 RID: 768
	// (get) Token: 0x06001B08 RID: 6920 RVA: 0x00095E43 File Offset: 0x00094243
	public bool IsPlaying
	{
		get
		{
			return this.audioSource != null && this.audioSource.isPlaying;
		}
	}

	// Token: 0x06001B09 RID: 6921 RVA: 0x00095E63 File Offset: 0x00094263
	public void Play()
	{
		if (this.audioSource != null)
		{
			this.audioSource.Play();
		}
	}

	// Token: 0x06001B0A RID: 6922 RVA: 0x00095E81 File Offset: 0x00094281
	public void Stop()
	{
		if (this.audioSource != null)
		{
			this.audioSource.Stop();
		}
	}

	// Token: 0x06001B0B RID: 6923 RVA: 0x00095E9F File Offset: 0x0009429F
	private void EndOfAnalysis()
	{
		if (this.preCalculate)
		{
		}
		this.isDone = true;
	}

	// Token: 0x06001B0C RID: 6924 RVA: 0x00095EB4 File Offset: 0x000942B4
	public void Update()
	{
		if (!this.initialized)
		{
			Debug.LogWarning("RhythmTool not initialized");
			return;
		}
		this.sampleIndex = this.audioSource.timeSamples;
		float num = (float)this.sampleIndex / (float)this.totalSamples;
		this.currentFrame = (int)(num * (float)this.totalFrames);
		this.interpolation = num * (float)this.totalFrames;
		this.interpolation -= (float)this.currentFrame;
		if (this.isDone)
		{
			return;
		}
		if (this.preCalculate)
		{
			this.lead += 500;
		}
		this.lead = Mathf.Max(40, this.lead);
		for (int i = this.lastFrame + 1; i < this.currentFrame + this.lead; i++)
		{
			if (i >= this.totalFrames)
			{
				this.EndOfAnalysis();
				break;
			}
			if (this.audioSource.clip == null)
			{
				break;
			}
			this.audioSource.clip.GetData(this.floatSamples, i * 1500);
			Util.FloatsToDoubles(this.floatSamples, this.doubleSamples);
			this.fft.RealFFT(this.doubleSamples, true);
			Util.SpectrumMagnitude(this.doubleSamples, this.magnitude);
			foreach (Analysis analysis in this.analyses)
			{
				analysis.Analyze(this.magnitude, i);
			}
			this.lastFrame = i;
		}
	}

	// Token: 0x06001B0D RID: 6925 RVA: 0x00096048 File Offset: 0x00094448
	public Frame[] GetResults(string name)
	{
		foreach (Analysis analysis in this.analyses)
		{
			if (analysis.name == name)
			{
				return analysis.Frames;
			}
		}
		Debug.LogWarning("Analysis " + name + " not found.");
		return null;
	}

	// Token: 0x06001B0E RID: 6926 RVA: 0x000960A4 File Offset: 0x000944A4
	public void DrawDebugLines()
	{
		this.audioSource.clip.GetData(this.floatSamples, this.currentFrame * 1500);
		Util.FloatsToDoubles(this.floatSamples, this.doubleSamples);
		this.fft.RealFFT(this.doubleSamples, true);
		Util.SpectrumMagnitude(this.doubleSamples, this.magnitude);
		for (int i = 0; i < this.magnitude.Length - 1; i++)
		{
			Vector3 start = new Vector3((float)(i * -1), this.magnitude[i] * (1f + (float)i * 0.05f), 0f);
			Vector3 end = new Vector3((float)((i + 1) * -1), this.magnitude[i + 1] * (1f + (float)i * 0.05f), 0f);
			Debug.DrawLine(start, end, Color.green);
		}
		for (int j = 0; j < this.analyses.Length; j++)
		{
			this.analyses[j].DrawDebugLines(this.currentFrame, j);
		}
	}

	// Token: 0x04001699 RID: 5785
	private AudioSource audioSource;

	// Token: 0x0400169A RID: 5786
	public const int fftWindowSize = 4096;

	// Token: 0x0400169B RID: 5787
	public const int frameSpacing = 1500;

	// Token: 0x0400169C RID: 5788
	[SerializeField]
	private int totalFrames;

	// Token: 0x0400169D RID: 5789
	[SerializeField]
	private int currentFrame;

	// Token: 0x0400169E RID: 5790
	[SerializeField]
	private float interpolation;

	// Token: 0x0400169F RID: 5791
	[SerializeField]
	private int lastFrame;

	// Token: 0x040016A0 RID: 5792
	[SerializeField]
	private int lead = 300;

	// Token: 0x040016A1 RID: 5793
	[SerializeField]
	private bool isDone;

	// Token: 0x040016A2 RID: 5794
	[SerializeField]
	private bool advancedAnalyses;

	// Token: 0x040016A3 RID: 5795
	[AnalysisList]
	public Analysis[] analyses;

	// Token: 0x040016A4 RID: 5796
	private bool preCalculate;

	// Token: 0x040016A5 RID: 5797
	private bool initialized;

	// Token: 0x040016A6 RID: 5798
	private int totalSamples;

	// Token: 0x040016A7 RID: 5799
	private int sampleIndex;

	// Token: 0x040016A8 RID: 5800
	private float[] floatSamples;

	// Token: 0x040016A9 RID: 5801
	private double[] doubleSamples;

	// Token: 0x040016AA RID: 5802
	private float[] magnitude;

	// Token: 0x040016AB RID: 5803
	private LomontFFT fft;
}
