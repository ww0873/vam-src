using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x02000C48 RID: 3144
public class SceneLoader : MonoBehaviour
{
	// Token: 0x06005B78 RID: 23416 RVA: 0x00218C0B File Offset: 0x0021700B
	public SceneLoader()
	{
	}

	// Token: 0x06005B79 RID: 23417 RVA: 0x00218C44 File Offset: 0x00217044
	public void AddKey()
	{
		if (this.sceneCheckFile != null && this.sceneCheckFile != string.Empty && this.keyInputField != null)
		{
			JSONClass jsonclass;
			if (File.Exists(this.sceneCheckFile))
			{
				StreamReader streamReader = new StreamReader(this.sceneCheckFile);
				string aJSON = streamReader.ReadToEnd();
				streamReader.Close();
				JSONNode jsonnode = JSON.Parse(aJSON);
				if (jsonnode == null)
				{
					jsonclass = new JSONClass();
				}
				else
				{
					jsonclass = jsonnode.AsObject;
					if (jsonclass == null)
					{
						jsonclass = new JSONClass();
					}
				}
			}
			else
			{
				jsonclass = new JSONClass();
			}
			string text = this.keyInputField.text;
			if (text != null && text.Length > 0)
			{
				string str = string.Empty;
				char c = text[0];
				string text2;
				switch (c)
				{
				case 'C':
					goto IL_152;
				default:
					switch (c)
					{
					case 'c':
						goto IL_152;
					default:
						if (c != 'T' && c != 't')
						{
							text2 = "Unknown";
							goto IL_171;
						}
						text2 = "Teaser";
						str = "T";
						goto IL_171;
					case 'e':
						goto IL_13F;
					case 'f':
						break;
					}
					break;
				case 'E':
					goto IL_13F;
				case 'F':
					break;
				}
				text2 = "Free";
				str = "F";
				goto IL_171;
				IL_13F:
				text2 = "Entertainer";
				str = "E";
				goto IL_171;
				IL_152:
				text2 = "Creator";
				str = "C";
				IL_171:
				if (text2 != "Unknown")
				{
					SHA256 shaHash = SHA256.Create();
					string sha256Hash = SceneLoader.GetSha256Hash(shaHash, text2 + text, 3);
					int buildIndexByScenePath = SceneUtility.GetBuildIndexByScenePath(this.sceneCheckFilePathPrefix + "/" + str + sha256Hash);
					if (buildIndexByScenePath != -1)
					{
						if (this.keyEntryStatus != null)
						{
							this.keyEntryStatus.text = "Key accepted";
						}
						jsonclass[text].AsBool = true;
						string value = jsonclass.ToString(string.Empty);
						try
						{
							StreamWriter streamWriter = new StreamWriter(this.sceneCheckFile);
							streamWriter.Write(value);
							streamWriter.Close();
						}
						catch (Exception ex)
						{
							if (this.keyEntryStatus != null)
							{
								this.keyEntryStatus.text = "Exception while storing key " + ex.Message;
							}
						}
						this.GenerateContentLevelToggles();
					}
					else if (this.keyEntryStatus != null)
					{
						this.keyEntryStatus.text = "Invalid key";
					}
				}
				else if (this.keyEntryStatus != null)
				{
					this.keyEntryStatus.text = "Invalid key";
				}
			}
			else if (this.keyEntryStatus != null)
			{
				this.keyEntryStatus.text = "Invalid key";
			}
		}
	}

	// Token: 0x06005B7A RID: 23418 RVA: 0x00218F2C File Offset: 0x0021732C
	public void OutputSceneNameForKey(string keyval)
	{
		char c = keyval[0];
		string str = string.Empty;
		string str2 = string.Empty;
		switch (c)
		{
		case 'C':
			goto IL_8E;
		default:
			switch (c)
			{
			case 'c':
				goto IL_8E;
			default:
				if (c != 'T' && c != 't')
				{
					str2 = "Unknown";
					goto IL_AA;
				}
				str2 = "Teaser";
				str = "T";
				goto IL_AA;
			case 'e':
				goto IL_7D;
			case 'f':
				break;
			}
			break;
		case 'E':
			goto IL_7D;
		case 'F':
			break;
		}
		str2 = "Free";
		str = "F";
		goto IL_AA;
		IL_7D:
		str2 = "Entertainer";
		str = "E";
		goto IL_AA;
		IL_8E:
		str2 = "Creator";
		str = "C";
		IL_AA:
		SHA256 shaHash = SHA256.Create();
		string sha256Hash = SceneLoader.GetSha256Hash(shaHash, str2 + keyval, 3);
		string str3 = str + sha256Hash;
		UnityEngine.Debug.Log("Scene name for key " + keyval + " is " + str3);
	}

	// Token: 0x06005B7B RID: 23419 RVA: 0x0021901C File Offset: 0x0021741C
	protected void GenerateContentLevelToggles()
	{
		if (this.sceneCheckFile != null && this.sceneCheckFile != string.Empty && this.contentLevelParent != null && this.contentLevelTogglePrefab != null)
		{
			IEnumerator enumerator = this.contentLevelParent.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					if (transform.name != "Label")
					{
						UnityEngine.Object.Destroy(transform.gameObject);
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			this.contentLevelParent.gameObject.SetActive(false);
			this.keyNameToKeyVal = new Dictionary<string, string>();
			try
			{
				if (File.Exists(this.sceneCheckFile))
				{
					StreamReader streamReader = new StreamReader(this.sceneCheckFile);
					string aJSON = streamReader.ReadToEnd();
					streamReader.Close();
					JSONNode jsonnode = JSON.Parse(aJSON);
					if (jsonnode != null)
					{
						JSONClass asObject = jsonnode.AsObject;
						if (asObject != null)
						{
							float num = this.contentLevelToggleStartY;
							int num2 = 0;
							Toggle toggle = null;
							this.firstSceneName = null;
							List<string> list = new List<string>(asObject.Keys);
							List<string> list2 = list;
							if (SceneLoader.<>f__am$cache0 == null)
							{
								SceneLoader.<>f__am$cache0 = new Comparison<string>(SceneLoader.<GenerateContentLevelToggles>m__0);
							}
							list2.Sort(SceneLoader.<>f__am$cache0);
							foreach (string text in list)
							{
								char c = text[0];
								string str = string.Empty;
								string text2;
								switch (c)
								{
								case 'C':
									goto IL_208;
								default:
									switch (c)
									{
									case 'c':
										goto IL_208;
									default:
										if (c != 'T' && c != 't')
										{
											text2 = "Unknown";
										}
										else
										{
											text2 = "Teaser";
											str = "T";
										}
										break;
									case 'e':
										goto IL_1F5;
									case 'f':
										goto IL_1CF;
									}
									break;
								case 'E':
									goto IL_1F5;
								case 'F':
									goto IL_1CF;
								}
								IL_227:
								if (text2 != null && text != null && text2 != "Unknown")
								{
									SHA256 shaHash = SHA256.Create();
									string sha256Hash = SceneLoader.GetSha256Hash(shaHash, text2 + text, 3);
									string value = str + sha256Hash;
									if (this.firstSceneName == null)
									{
										this.firstSceneName = value;
									}
									num2++;
									this.keyNameToKeyVal.Add(text2, value);
									Vector2 anchoredPosition;
									anchoredPosition.x = 0f;
									anchoredPosition.y = num;
									Transform transform2 = UnityEngine.Object.Instantiate<Transform>(this.contentLevelTogglePrefab);
									transform2.SetParent(this.contentLevelParent, false);
									RectTransform component = transform2.GetComponent<RectTransform>();
									component.anchoredPosition = anchoredPosition;
									num += component.sizeDelta.y + this.contentLevelToggleBuffer;
									Toggle componentInChildren = transform2.GetComponentInChildren<Toggle>();
									if (componentInChildren != null)
									{
										toggle = componentInChildren;
										componentInChildren.isOn = false;
										componentInChildren.group = this.contentLevelToggleGroup;
									}
									Text componentInChildren2 = transform2.GetComponentInChildren<Text>();
									if (componentInChildren2 != null)
									{
										componentInChildren2.text = text2;
									}
									continue;
								}
								UnityEngine.Debug.LogError("Invalid key file");
								if (this.keyEntryStatus != null)
								{
									this.keyEntryStatus.text = "Invalid key file";
								}
								if (this.keyIssueBanner != null)
								{
									this.keyIssueBanner.gameObject.SetActive(true);
									continue;
								}
								continue;
								IL_1CF:
								text2 = "Free";
								str = "F";
								goto IL_227;
								IL_1F5:
								text2 = "Entertainer";
								str = "E";
								goto IL_227;
								IL_208:
								text2 = "Creator";
								str = "C";
								goto IL_227;
							}
							if (toggle != null)
							{
								toggle.isOn = true;
							}
							if (num2 > 0)
							{
								float y = num + this.contentLevelToggleBuffer;
								Vector2 sizeDelta = this.contentLevelParent.sizeDelta;
								sizeDelta.y = y;
								this.contentLevelParent.sizeDelta = sizeDelta;
							}
							if (num2 == 1)
							{
								this.singleScene = true;
								if (this.singleSceneBanner != null)
								{
									this.singleSceneBanner.gameObject.SetActive(true);
								}
								if (this.multiSceneBanner != null)
								{
									this.multiSceneBanner.gameObject.SetActive(false);
								}
								if (this.singleSceneText != null)
								{
									this.singleSceneText.gameObject.SetActive(true);
								}
								if (this.multiSceneText != null)
								{
									this.multiSceneText.gameObject.SetActive(false);
								}
							}
							else if (num2 >= 1)
							{
								this.singleScene = false;
								this.contentLevelParent.gameObject.SetActive(true);
								if (this.singleSceneBanner != null)
								{
									this.singleSceneBanner.gameObject.SetActive(false);
								}
								if (this.multiSceneBanner != null)
								{
									this.multiSceneBanner.gameObject.SetActive(true);
								}
								if (this.singleSceneText != null)
								{
									this.singleSceneText.gameObject.SetActive(false);
								}
								if (this.multiSceneText != null)
								{
									this.multiSceneText.gameObject.SetActive(true);
								}
							}
							else
							{
								UnityEngine.Debug.LogError("No valid keys found in keys file");
								if (this.keyEntryStatus != null)
								{
									this.keyEntryStatus.text = "No valid keys found in keys file";
								}
								if (this.keyIssueBanner != null)
								{
									this.keyIssueBanner.gameObject.SetActive(true);
								}
							}
						}
						else
						{
							UnityEngine.Debug.LogError("Invalid key file");
							if (this.keyEntryStatus != null)
							{
								this.keyEntryStatus.text = "Invalid key file";
							}
							if (this.keyIssueBanner != null)
							{
								this.keyIssueBanner.gameObject.SetActive(true);
							}
						}
					}
					else
					{
						UnityEngine.Debug.LogError("Invalid key file");
						if (this.keyEntryStatus != null)
						{
							this.keyEntryStatus.text = "Invalid key file";
						}
						if (this.keyIssueBanner != null)
						{
							this.keyIssueBanner.gameObject.SetActive(true);
						}
					}
				}
				else
				{
					UnityEngine.Debug.LogError("Key file missing");
					if (this.keyEntryStatus != null)
					{
						this.keyEntryStatus.text = "Key file missing";
					}
					if (this.keyIssueBanner != null)
					{
						this.keyIssueBanner.gameObject.SetActive(true);
					}
				}
			}
			catch (Exception ex)
			{
				if (this.keyEntryStatus != null)
				{
					this.keyEntryStatus.text = "Exception while processing key file " + ex.Message;
				}
				if (this.keyIssueBanner != null)
				{
					this.keyIssueBanner.gameObject.SetActive(true);
				}
			}
		}
		else if (this.contentLevelParent != null)
		{
			this.contentLevelParent.gameObject.SetActive(false);
		}
	}

	// Token: 0x06005B7C RID: 23420 RVA: 0x00219780 File Offset: 0x00217B80
	private static string GetSha256Hash(SHA256 shaHash, string input, int length)
	{
		byte[] array = shaHash.ComputeHash(Encoding.UTF8.GetBytes(input));
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < array.Length; i++)
		{
			if (i >= length)
			{
				break;
			}
			stringBuilder.Append(array[i].ToString("x2"));
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06005B7D RID: 23421 RVA: 0x002197E4 File Offset: 0x00217BE4
	protected string GetLoadSceneName()
	{
		if (this.sceneCheckFile == null || !(this.sceneCheckFile != string.Empty))
		{
			return this.sceneName;
		}
		if (this.singleScene)
		{
			return this.firstSceneName;
		}
		if (this.contentLevelToggleGroup != null)
		{
			foreach (Toggle toggle in this.contentLevelToggleGroup.ActiveToggles())
			{
				Text componentInChildren = toggle.GetComponentInChildren<Text>();
				if (componentInChildren != null)
				{
					string text = componentInChildren.text;
					string str;
					if (this.keyNameToKeyVal.TryGetValue(text, out str))
					{
						return this.sceneCheckFilePathPrefix + "/" + str;
					}
				}
			}
		}
		return null;
	}

	// Token: 0x06005B7E RID: 23422 RVA: 0x002198D0 File Offset: 0x00217CD0
	private IEnumerator LoadMainScene()
	{
		if (this.startButton != null)
		{
			this.startButton.gameObject.SetActive(false);
		}
		if (this.startButtonAlt != null)
		{
			this.startButtonAlt.gameObject.SetActive(false);
		}
		string loadSceneName = this.GetLoadSceneName();
		if (loadSceneName != null)
		{
			if (this.contentLevelParent != null)
			{
				this.contentLevelParent.gameObject.SetActive(false);
			}
			this.mainAsync = SceneManager.LoadSceneAsync(loadSceneName, LoadSceneMode.Single);
			yield return this.async;
		}
		else
		{
			UnityEngine.Debug.LogError("Load scene name was null. Check key file");
			if (this.keyEntryStatus != null)
			{
				this.keyEntryStatus.text = "Invalid key file";
			}
			if (this.keyIssueBanner != null)
			{
				this.keyIssueBanner.gameObject.SetActive(true);
			}
		}
		yield break;
	}

	// Token: 0x06005B7F RID: 23423 RVA: 0x002198EC File Offset: 0x00217CEC
	private IEnumerator LoadMergeScenesAsync()
	{
		this.isLoading = true;
		yield return null;
		if (this.fullProgressSlider != null)
		{
			this.fullProgressSlider.gameObject.SetActive(true);
			this.fullProgressSlider.value = 0f;
		}
		if (this.individualProgressSlider != null)
		{
			this.individualProgressSlider.gameObject.SetActive(true);
			this.individualProgressSlider.value = 0f;
		}
		if (this.progressMaterial != null && this.progressMaterial.HasProperty(this.progressMaterialFieldName))
		{
			this.progressMaterial.SetFloat(this.progressMaterialFieldName, 0f);
		}
		int fullLength = this.preloadScenes.Length;
		for (int i = 0; i < this.preloadScenes.Length; i++)
		{
			this.async = SceneManager.LoadSceneAsync(this.preloadScenes[i], LoadSceneMode.Additive);
			yield return this.async;
			this.progressTarget = (float)(i + 1) / (float)fullLength;
		}
		this.progressTarget = 1f;
		this.isLoading = false;
		yield break;
	}

	// Token: 0x06005B80 RID: 23424 RVA: 0x00219908 File Offset: 0x00217D08
	protected void LoadScene()
	{
		if (this.startButton != null)
		{
			this.startButton.interactable = false;
		}
		if (this.startButtonAlt != null)
		{
			this.startButtonAlt.interactable = false;
		}
		if (this.loadAsync)
		{
			base.StartCoroutine(this.LoadMergeScenesAsync());
		}
		else
		{
			for (int i = 0; i < this.preloadScenes.Length; i++)
			{
				SceneManager.LoadScene(this.preloadScenes[i], LoadSceneMode.Additive);
			}
			string loadSceneName = this.GetLoadSceneName();
			if (loadSceneName != null)
			{
				SceneManager.LoadScene(loadSceneName, LoadSceneMode.Single);
			}
		}
	}

	// Token: 0x06005B81 RID: 23425 RVA: 0x002199A7 File Offset: 0x00217DA7
	protected void ActivateScene()
	{
		base.StartCoroutine(this.LoadMainScene());
	}

	// Token: 0x06005B82 RID: 23426 RVA: 0x002199B8 File Offset: 0x00217DB8
	private void Start()
	{
		string[] commandLineArgs = Environment.GetCommandLineArgs();
		string a = string.Empty;
		for (int i = 0; i < commandLineArgs.Length; i++)
		{
			if (commandLineArgs[i] == "-benchmark")
			{
				a = commandLineArgs[i + 1];
			}
		}
		if (a != string.Empty)
		{
			SceneManager.LoadScene(a, LoadSceneMode.Single);
		}
		else
		{
			if (this.loadOnStart)
			{
				if (this.startButton != null)
				{
					this.startButton.gameObject.SetActive(true);
					this.startButton.onClick.AddListener(new UnityAction(this.ActivateScene));
					this.startButton.interactable = false;
				}
				if (this.startButtonAlt != null)
				{
					this.startButtonAlt.gameObject.SetActive(true);
					this.startButtonAlt.onClick.AddListener(new UnityAction(this.ActivateScene));
					this.startButtonAlt.interactable = false;
				}
				this.LoadScene();
			}
			else
			{
				if (this.startButton != null)
				{
					this.startButton.gameObject.SetActive(true);
					this.startButton.onClick.AddListener(new UnityAction(this.LoadScene));
				}
				if (this.startButtonAlt != null)
				{
					this.startButtonAlt.gameObject.SetActive(true);
					this.startButtonAlt.onClick.AddListener(new UnityAction(this.LoadScene));
				}
			}
			this.GenerateContentLevelToggles();
		}
	}

	// Token: 0x06005B83 RID: 23427 RVA: 0x00219B48 File Offset: 0x00217F48
	private void Update()
	{
		if (this.isLoading && this.individualProgressSlider != null)
		{
			this.individualProgressSlider.value = this.async.progress * 100f;
		}
		if (this.progressTarget > this.progress)
		{
			this.progress += this.progressMaxSpeed;
			if (this.progress > this.progressTarget)
			{
				this.progress = this.progressTarget;
			}
		}
		if (this.progress == 1f)
		{
			this.progress = this.progressTarget;
			if (this.activateWhenLoaded)
			{
				this.ActivateScene();
			}
			else
			{
				if (this.startButton != null)
				{
					this.startButton.interactable = true;
				}
				if (this.startButtonAlt != null)
				{
					this.startButtonAlt.interactable = true;
				}
			}
		}
		if (this.fullProgressSlider != null)
		{
			this.fullProgressSlider.value = this.progress * 100f;
		}
		if (this.progressMaterial != null)
		{
			if (this.mainAsync != null)
			{
				this.progressMaterial.SetFloat(this.progressMaterialFieldName, 1f - this.mainAsync.progress);
			}
			else if (this.progressMaterial.HasProperty(this.progressMaterialFieldName))
			{
				this.progressMaterial.SetFloat(this.progressMaterialFieldName, this.progress);
			}
		}
	}

	// Token: 0x06005B84 RID: 23428 RVA: 0x00219CD4 File Offset: 0x002180D4
	[CompilerGenerated]
	private static int <GenerateContentLevelToggles>m__0(string s1, string s2)
	{
		char c = s1[0];
		char c2 = s2[0];
		if (c == c2)
		{
			return 0;
		}
		if (c == 'F')
		{
			return -1;
		}
		if (c == 'T')
		{
			if (c2 == 'F')
			{
				return 1;
			}
			return -1;
		}
		else
		{
			if (c != 'E')
			{
				return 1;
			}
			if (c2 == 'F' || c2 == 'T')
			{
				return 1;
			}
			return -1;
		}
	}

	// Token: 0x04004B56 RID: 19286
	public Text statusText;

	// Token: 0x04004B57 RID: 19287
	public Button startButton;

	// Token: 0x04004B58 RID: 19288
	public Button startButtonAlt;

	// Token: 0x04004B59 RID: 19289
	public Slider fullProgressSlider;

	// Token: 0x04004B5A RID: 19290
	public Slider individualProgressSlider;

	// Token: 0x04004B5B RID: 19291
	public string[] preloadScenes;

	// Token: 0x04004B5C RID: 19292
	public string sceneCheckFile;

	// Token: 0x04004B5D RID: 19293
	public string sceneCheckFilePathPrefix;

	// Token: 0x04004B5E RID: 19294
	public string sceneName;

	// Token: 0x04004B5F RID: 19295
	public bool loadOnStart;

	// Token: 0x04004B60 RID: 19296
	public bool activateWhenLoaded = true;

	// Token: 0x04004B61 RID: 19297
	public bool loadAsync = true;

	// Token: 0x04004B62 RID: 19298
	protected bool isLoading;

	// Token: 0x04004B63 RID: 19299
	protected bool isLoadingMainScene;

	// Token: 0x04004B64 RID: 19300
	protected AsyncOperation async;

	// Token: 0x04004B65 RID: 19301
	protected AsyncOperation mainAsync;

	// Token: 0x04004B66 RID: 19302
	public Material progressMaterial;

	// Token: 0x04004B67 RID: 19303
	public string progressMaterialFieldName;

	// Token: 0x04004B68 RID: 19304
	protected float progressTarget;

	// Token: 0x04004B69 RID: 19305
	protected float progress;

	// Token: 0x04004B6A RID: 19306
	public float progressMaxSpeed = 0.005f;

	// Token: 0x04004B6B RID: 19307
	public RectTransform contentLevelParent;

	// Token: 0x04004B6C RID: 19308
	public Transform contentLevelTogglePrefab;

	// Token: 0x04004B6D RID: 19309
	public ToggleGroup contentLevelToggleGroup;

	// Token: 0x04004B6E RID: 19310
	public float contentLevelToggleStartY = 100f;

	// Token: 0x04004B6F RID: 19311
	public float contentLevelToggleBuffer = 5f;

	// Token: 0x04004B70 RID: 19312
	public Transform singleSceneBanner;

	// Token: 0x04004B71 RID: 19313
	public Transform singleSceneText;

	// Token: 0x04004B72 RID: 19314
	public Transform multiSceneBanner;

	// Token: 0x04004B73 RID: 19315
	public Transform multiSceneText;

	// Token: 0x04004B74 RID: 19316
	public Transform keyIssueBanner;

	// Token: 0x04004B75 RID: 19317
	protected Dictionary<string, string> keyNameToKeyVal;

	// Token: 0x04004B76 RID: 19318
	protected string firstSceneName;

	// Token: 0x04004B77 RID: 19319
	protected bool singleScene;

	// Token: 0x04004B78 RID: 19320
	public Text keyInputField;

	// Token: 0x04004B79 RID: 19321
	public Text keyEntryStatus;

	// Token: 0x04004B7A RID: 19322
	[CompilerGenerated]
	private static Comparison<string> <>f__am$cache0;

	// Token: 0x02001007 RID: 4103
	[CompilerGenerated]
	private sealed class <LoadMainScene>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x0600768E RID: 30350 RVA: 0x00219D35 File Offset: 0x00218135
		[DebuggerHidden]
		public <LoadMainScene>c__Iterator0()
		{
		}

		// Token: 0x0600768F RID: 30351 RVA: 0x00219D40 File Offset: 0x00218140
		public bool MoveNext()
		{
			uint num = (uint)this.$PC;
			this.$PC = -1;
			switch (num)
			{
			case 0U:
				if (this.startButton != null)
				{
					this.startButton.gameObject.SetActive(false);
				}
				if (this.startButtonAlt != null)
				{
					this.startButtonAlt.gameObject.SetActive(false);
				}
				loadSceneName = base.GetLoadSceneName();
				if (loadSceneName != null)
				{
					if (this.contentLevelParent != null)
					{
						this.contentLevelParent.gameObject.SetActive(false);
					}
					this.mainAsync = SceneManager.LoadSceneAsync(loadSceneName, LoadSceneMode.Single);
					this.$current = this.async;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				UnityEngine.Debug.LogError("Load scene name was null. Check key file");
				if (this.keyEntryStatus != null)
				{
					this.keyEntryStatus.text = "Invalid key file";
				}
				if (this.keyIssueBanner != null)
				{
					this.keyIssueBanner.gameObject.SetActive(true);
				}
				break;
			case 1U:
				break;
			default:
				return false;
			}
			this.$PC = -1;
			return false;
		}

		// Token: 0x17001189 RID: 4489
		// (get) Token: 0x06007690 RID: 30352 RVA: 0x00219EBA File Offset: 0x002182BA
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x1700118A RID: 4490
		// (get) Token: 0x06007691 RID: 30353 RVA: 0x00219EC2 File Offset: 0x002182C2
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x06007692 RID: 30354 RVA: 0x00219ECA File Offset: 0x002182CA
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x06007693 RID: 30355 RVA: 0x00219EDA File Offset: 0x002182DA
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x04006A58 RID: 27224
		internal string <loadSceneName>__0;

		// Token: 0x04006A59 RID: 27225
		internal SceneLoader $this;

		// Token: 0x04006A5A RID: 27226
		internal object $current;

		// Token: 0x04006A5B RID: 27227
		internal bool $disposing;

		// Token: 0x04006A5C RID: 27228
		internal int $PC;
	}

	// Token: 0x02001008 RID: 4104
	[CompilerGenerated]
	private sealed class <LoadMergeScenesAsync>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x06007694 RID: 30356 RVA: 0x00219EE1 File Offset: 0x002182E1
		[DebuggerHidden]
		public <LoadMergeScenesAsync>c__Iterator1()
		{
		}

		// Token: 0x06007695 RID: 30357 RVA: 0x00219EEC File Offset: 0x002182EC
		public bool MoveNext()
		{
			uint num = (uint)this.$PC;
			this.$PC = -1;
			switch (num)
			{
			case 0U:
				this.isLoading = true;
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
			case 1U:
				if (this.fullProgressSlider != null)
				{
					this.fullProgressSlider.gameObject.SetActive(true);
					this.fullProgressSlider.value = 0f;
				}
				if (this.individualProgressSlider != null)
				{
					this.individualProgressSlider.gameObject.SetActive(true);
					this.individualProgressSlider.value = 0f;
				}
				if (this.progressMaterial != null && this.progressMaterial.HasProperty(this.progressMaterialFieldName))
				{
					this.progressMaterial.SetFloat(this.progressMaterialFieldName, 0f);
				}
				fullLength = this.preloadScenes.Length;
				i = 0;
				break;
			case 2U:
				this.progressTarget = (float)(i + 1) / (float)fullLength;
				i++;
				break;
			default:
				return false;
			}
			if (i < this.preloadScenes.Length)
			{
				this.async = SceneManager.LoadSceneAsync(this.preloadScenes[i], LoadSceneMode.Additive);
				this.$current = this.async;
				if (!this.$disposing)
				{
					this.$PC = 2;
				}
				return true;
			}
			this.progressTarget = 1f;
			this.isLoading = false;
			this.$PC = -1;
			return false;
		}

		// Token: 0x1700118B RID: 4491
		// (get) Token: 0x06007696 RID: 30358 RVA: 0x0021A0EC File Offset: 0x002184EC
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x1700118C RID: 4492
		// (get) Token: 0x06007697 RID: 30359 RVA: 0x0021A0F4 File Offset: 0x002184F4
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x06007698 RID: 30360 RVA: 0x0021A0FC File Offset: 0x002184FC
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x06007699 RID: 30361 RVA: 0x0021A10C File Offset: 0x0021850C
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x04006A5D RID: 27229
		internal int <fullLength>__0;

		// Token: 0x04006A5E RID: 27230
		internal int <i>__1;

		// Token: 0x04006A5F RID: 27231
		internal SceneLoader $this;

		// Token: 0x04006A60 RID: 27232
		internal object $current;

		// Token: 0x04006A61 RID: 27233
		internal bool $disposing;

		// Token: 0x04006A62 RID: 27234
		internal int $PC;
	}
}
