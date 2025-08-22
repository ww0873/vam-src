using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using AssetBundles;
using Battlehub.RTCommon;
using DynamicCSharp;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Leap.Unity;
using MeshVR;
using MeshVR.Hands;
using MHLab.PATCH.Settings;
using MHLab.PATCH.Utilities;
using Mono.CSharp;
using MVR.FileManagement;
using MVR.FileManagementSecure;
using MVR.Hub;
using SimpleJSON;
using uFileBrowser;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;
using Valve.VR;

// Token: 0x02000C53 RID: 3155
public class SuperController : MonoBehaviour
{
	// Token: 0x06005BD1 RID: 23505 RVA: 0x0021CD4C File Offset: 0x0021B14C
	public SuperController()
	{
	}

	// Token: 0x17000D73 RID: 3443
	// (get) Token: 0x06005BD2 RID: 23506 RVA: 0x0021D327 File Offset: 0x0021B727
	public static SuperController singleton
	{
		get
		{
			return SuperController._singleton;
		}
	}

	// Token: 0x06005BD3 RID: 23507 RVA: 0x0021D330 File Offset: 0x0021B730
	protected RectTransform CreateDynamicUIElement(RectTransform parent, RectTransform prefab)
	{
		RectTransform rectTransform = UnityEngine.Object.Instantiate<RectTransform>(prefab);
		rectTransform.SetParent(parent, false);
		return rectTransform;
	}

	// Token: 0x06005BD4 RID: 23508 RVA: 0x0021D350 File Offset: 0x0021B750
	public UIDynamicButton CreateDynamicButton(RectTransform parent)
	{
		UIDynamicButton uidynamicButton = null;
		if (this.dynamicButtonPrefab != null)
		{
			RectTransform rectTransform = this.CreateDynamicUIElement(parent, this.dynamicButtonPrefab);
			uidynamicButton = rectTransform.GetComponentInChildren<UIDynamicButton>(true);
			if (uidynamicButton == null)
			{
				UnityEngine.Object.Destroy(rectTransform);
			}
		}
		return uidynamicButton;
	}

	// Token: 0x06005BD5 RID: 23509 RVA: 0x0021D39C File Offset: 0x0021B79C
	public UIDynamicToggle CreateDynamicToggle(RectTransform parent)
	{
		UIDynamicToggle uidynamicToggle = null;
		if (this.dynamicTogglePrefab != null)
		{
			RectTransform rectTransform = this.CreateDynamicUIElement(parent, this.dynamicTogglePrefab);
			uidynamicToggle = rectTransform.GetComponentInChildren<UIDynamicToggle>(true);
			if (uidynamicToggle == null)
			{
				UnityEngine.Object.Destroy(rectTransform);
			}
		}
		return uidynamicToggle;
	}

	// Token: 0x06005BD6 RID: 23510 RVA: 0x0021D3E8 File Offset: 0x0021B7E8
	public UIDynamicSlider CreateDynamicSlider(RectTransform parent)
	{
		UIDynamicSlider uidynamicSlider = null;
		if (this.dynamicSliderPrefab != null)
		{
			RectTransform rectTransform = this.CreateDynamicUIElement(parent, this.dynamicSliderPrefab);
			uidynamicSlider = rectTransform.GetComponentInChildren<UIDynamicSlider>(true);
			if (uidynamicSlider == null)
			{
				UnityEngine.Object.Destroy(rectTransform);
			}
		}
		return uidynamicSlider;
	}

	// Token: 0x17000D74 RID: 3444
	// (get) Token: 0x06005BD7 RID: 23511 RVA: 0x0021D431 File Offset: 0x0021B831
	protected bool advancedSceneEditDisabled
	{
		get
		{
			if (GlobalSceneOptions.singleton != null)
			{
				return GlobalSceneOptions.singleton.disableAdvancedSceneEdit;
			}
			return this.disableAdvancedSceneEdit;
		}
	}

	// Token: 0x17000D75 RID: 3445
	// (get) Token: 0x06005BD8 RID: 23512 RVA: 0x0021D454 File Offset: 0x0021B854
	protected bool saveSceneButtonDisabled
	{
		get
		{
			if (GlobalSceneOptions.singleton != null)
			{
				return GlobalSceneOptions.singleton.disableSaveSceneButton;
			}
			return this.disableSaveSceneButton;
		}
	}

	// Token: 0x17000D76 RID: 3446
	// (get) Token: 0x06005BD9 RID: 23513 RVA: 0x0021D477 File Offset: 0x0021B877
	protected bool loadSceneButtonDisabled
	{
		get
		{
			if (GlobalSceneOptions.singleton != null)
			{
				return GlobalSceneOptions.singleton.disableLoadSceneButton;
			}
			return this.disableLoadSceneButton;
		}
	}

	// Token: 0x17000D77 RID: 3447
	// (get) Token: 0x06005BDA RID: 23514 RVA: 0x0021D49A File Offset: 0x0021B89A
	protected bool customUIDisabled
	{
		get
		{
			if (GlobalSceneOptions.singleton != null)
			{
				return GlobalSceneOptions.singleton.disableCustomUI;
			}
			return this.disableCustomUI;
		}
	}

	// Token: 0x17000D78 RID: 3448
	// (get) Token: 0x06005BDB RID: 23515 RVA: 0x0021D4BD File Offset: 0x0021B8BD
	protected bool browseDisabled
	{
		get
		{
			if (GlobalSceneOptions.singleton != null)
			{
				return GlobalSceneOptions.singleton.disableBrowse;
			}
			return this.disableBrowse;
		}
	}

	// Token: 0x17000D79 RID: 3449
	// (get) Token: 0x06005BDC RID: 23516 RVA: 0x0021D4E0 File Offset: 0x0021B8E0
	protected bool packagesDisabled
	{
		get
		{
			if (GlobalSceneOptions.singleton != null)
			{
				return GlobalSceneOptions.singleton.disablePackages;
			}
			return this.disablePackages;
		}
	}

	// Token: 0x17000D7A RID: 3450
	// (get) Token: 0x06005BDD RID: 23517 RVA: 0x0021D503 File Offset: 0x0021B903
	public bool promotionalDisabled
	{
		get
		{
			if (GlobalSceneOptions.singleton != null)
			{
				return GlobalSceneOptions.singleton.disablePromotional;
			}
			return this.disablePromotional;
		}
	}

	// Token: 0x17000D7B RID: 3451
	// (get) Token: 0x06005BDE RID: 23518 RVA: 0x0021D526 File Offset: 0x0021B926
	public bool keyInformationDisabled
	{
		get
		{
			if (GlobalSceneOptions.singleton != null)
			{
				return GlobalSceneOptions.singleton.disableKeyInformation;
			}
			return this.disableKeyInformation;
		}
	}

	// Token: 0x17000D7C RID: 3452
	// (get) Token: 0x06005BDF RID: 23519 RVA: 0x0021D549 File Offset: 0x0021B949
	public bool hubDisabled
	{
		get
		{
			if (GlobalSceneOptions.singleton != null)
			{
				return GlobalSceneOptions.singleton.disableHub;
			}
			return this.disableHub;
		}
	}

	// Token: 0x17000D7D RID: 3453
	// (get) Token: 0x06005BE0 RID: 23520 RVA: 0x0021D56C File Offset: 0x0021B96C
	public bool termsOfUseDisabled
	{
		get
		{
			if (GlobalSceneOptions.singleton != null)
			{
				return GlobalSceneOptions.singleton.disableTermsOfUse;
			}
			return this.disableTermsOfUse;
		}
	}

	// Token: 0x06005BE1 RID: 23521 RVA: 0x0021D590 File Offset: 0x0021B990
	protected void SyncAdvancedSceneEditModeTransforms()
	{
		if (this.advancedSceneEditOnlyEditModeTransforms != null)
		{
			foreach (Transform transform in this.advancedSceneEditOnlyEditModeTransforms)
			{
				if (transform != null)
				{
					transform.gameObject.SetActive(!this.advancedSceneEditDisabled && this.gameMode == SuperController.GameMode.Edit);
				}
			}
		}
		if (this.advancedSceneEditDisabledEditModeTransforms != null)
		{
			foreach (Transform transform2 in this.advancedSceneEditDisabledEditModeTransforms)
			{
				if (transform2 != null)
				{
					transform2.gameObject.SetActive(this.advancedSceneEditDisabled && this.gameMode == SuperController.GameMode.Edit);
				}
			}
		}
	}

	// Token: 0x06005BE2 RID: 23522 RVA: 0x0021D658 File Offset: 0x0021BA58
	public void SyncUIToUnlockLevel()
	{
		if (this.loadSceneButtons != null)
		{
			foreach (Transform transform in this.loadSceneButtons)
			{
				if (transform != null)
				{
					transform.gameObject.SetActive(!this.loadSceneButtonDisabled);
				}
			}
		}
		if (this.loadSceneDisabledButtons != null)
		{
			foreach (Transform transform2 in this.loadSceneDisabledButtons)
			{
				if (transform2 != null)
				{
					transform2.gameObject.SetActive(this.loadSceneButtonDisabled);
				}
			}
		}
		if (this.onlineBrowseSceneButtons != null)
		{
			foreach (Transform transform3 in this.onlineBrowseSceneButtons)
			{
				if (transform3 != null)
				{
					transform3.gameObject.SetActive(!this.browseDisabled);
				}
			}
		}
		if (this.onlineBrowseSceneDisabledButtons != null)
		{
			foreach (Transform transform4 in this.onlineBrowseSceneDisabledButtons)
			{
				if (transform4 != null)
				{
					transform4.gameObject.SetActive(this.browseDisabled);
				}
			}
		}
		if (this.saveSceneButtons != null)
		{
			foreach (Transform transform5 in this.saveSceneButtons)
			{
				if (transform5 != null)
				{
					transform5.gameObject.SetActive(!this.saveSceneButtonDisabled);
				}
			}
		}
		if (this.saveSceneDisabledButtons != null)
		{
			foreach (Transform transform6 in this.saveSceneDisabledButtons)
			{
				if (transform6 != null)
				{
					transform6.gameObject.SetActive(this.saveSceneButtonDisabled);
				}
			}
		}
		if (this.advancedSceneEditButtons != null)
		{
			foreach (Transform transform7 in this.advancedSceneEditButtons)
			{
				if (transform7 != null)
				{
					transform7.gameObject.SetActive(!this.advancedSceneEditDisabled);
				}
			}
		}
		if (this.advancedSceneEditDisabledButtons != null)
		{
			foreach (Transform transform8 in this.advancedSceneEditDisabledButtons)
			{
				if (transform8 != null)
				{
					transform8.gameObject.SetActive(this.advancedSceneEditDisabled);
				}
			}
		}
		if (this.promotionalTransforms != null)
		{
			foreach (Transform transform9 in this.promotionalTransforms)
			{
				if (transform9 != null)
				{
					transform9.gameObject.SetActive(!this.promotionalDisabled);
				}
			}
		}
		if (this.keyInformationTransforms != null)
		{
			foreach (Transform transform10 in this.keyInformationTransforms)
			{
				if (transform10 != null)
				{
					transform10.gameObject.SetActive(!this.keyInformationDisabled);
				}
			}
		}
		if (this.hubDisabledTransforms != null && this.hubDisabled)
		{
			foreach (Transform transform11 in this.hubDisabledTransforms)
			{
				if (transform11 != null)
				{
					transform11.gameObject.SetActive(false);
				}
			}
			foreach (Transform transform12 in this.hubDisabledEnableTransforms)
			{
				if (transform12 != null)
				{
					transform12.gameObject.SetActive(true);
				}
			}
		}
		if (this.termsOfUseTransforms != null)
		{
			foreach (Transform transform13 in this.termsOfUseTransforms)
			{
				if (transform13 != null)
				{
					transform13.gameObject.SetActive(!this.termsOfUseDisabled);
				}
			}
		}
		this.SyncAdvancedSceneEditModeTransforms();
		this.SyncVamX();
	}

	// Token: 0x06005BE3 RID: 23523 RVA: 0x0021DA98 File Offset: 0x0021BE98
	protected static string GetSha256Hash(SHA256 shaHash, string input, int length)
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

	// Token: 0x17000D7E RID: 3454
	// (get) Token: 0x06005BE4 RID: 23524 RVA: 0x0021DAFB File Offset: 0x0021BEFB
	public string freeKey
	{
		get
		{
			if (GlobalSceneOptions.singleton != null)
			{
				return GlobalSceneOptions.singleton.freeKey;
			}
			return this._freeKey;
		}
	}

	// Token: 0x17000D7F RID: 3455
	// (get) Token: 0x06005BE5 RID: 23525 RVA: 0x0021DB1E File Offset: 0x0021BF1E
	public string teaserKey
	{
		get
		{
			if (GlobalSceneOptions.singleton != null)
			{
				return GlobalSceneOptions.singleton.teaserKey;
			}
			return this._teaserKey;
		}
	}

	// Token: 0x17000D80 RID: 3456
	// (get) Token: 0x06005BE6 RID: 23526 RVA: 0x0021DB41 File Offset: 0x0021BF41
	public string entertainerKey
	{
		get
		{
			if (GlobalSceneOptions.singleton != null)
			{
				return GlobalSceneOptions.singleton.entertainerKey;
			}
			return this._entertainerKey;
		}
	}

	// Token: 0x17000D81 RID: 3457
	// (get) Token: 0x06005BE7 RID: 23527 RVA: 0x0021DB64 File Offset: 0x0021BF64
	public string creatorKey
	{
		get
		{
			if (GlobalSceneOptions.singleton != null)
			{
				return GlobalSceneOptions.singleton.creatorKey;
			}
			return this._creatorKey;
		}
	}

	// Token: 0x17000D82 RID: 3458
	// (get) Token: 0x06005BE8 RID: 23528 RVA: 0x0021DB87 File Offset: 0x0021BF87
	public string steamKey
	{
		get
		{
			if (GlobalSceneOptions.singleton != null)
			{
				return GlobalSceneOptions.singleton.steamKey;
			}
			return this._steamKey;
		}
	}

	// Token: 0x17000D83 RID: 3459
	// (get) Token: 0x06005BE9 RID: 23529 RVA: 0x0021DBAA File Offset: 0x0021BFAA
	public string nsteamKey
	{
		get
		{
			if (GlobalSceneOptions.singleton != null)
			{
				return GlobalSceneOptions.singleton.nsteamKey;
			}
			return this._nsteamKey;
		}
	}

	// Token: 0x17000D84 RID: 3460
	// (get) Token: 0x06005BEA RID: 23530 RVA: 0x0021DBCD File Offset: 0x0021BFCD
	public string retailKey
	{
		get
		{
			if (GlobalSceneOptions.singleton != null)
			{
				return GlobalSceneOptions.singleton.retailKey;
			}
			return this._retailKey;
		}
	}

	// Token: 0x17000D85 RID: 3461
	// (get) Token: 0x06005BEB RID: 23531 RVA: 0x0021DBF0 File Offset: 0x0021BFF0
	public string keyFilePath
	{
		get
		{
			if (GlobalSceneOptions.singleton != null)
			{
				return GlobalSceneOptions.singleton.keyFilePath;
			}
			return this._keyFilePath;
		}
	}

	// Token: 0x17000D86 RID: 3462
	// (get) Token: 0x06005BEC RID: 23532 RVA: 0x0021DC13 File Offset: 0x0021C013
	public string legacySteamKeyFilePath
	{
		get
		{
			if (GlobalSceneOptions.singleton != null)
			{
				return GlobalSceneOptions.singleton.legacySteamKeyFilePath;
			}
			return this._legacySteamKeyFilePath;
		}
	}

	// Token: 0x06005BED RID: 23533 RVA: 0x0021DC38 File Offset: 0x0021C038
	protected SuperController.KeyType IsValidKey(string key)
	{
		char c = key[0];
		SuperController.KeyType result;
		switch (c)
		{
		case 'C':
			goto IL_A3;
		default:
			switch (c)
			{
			case 'R':
				goto IL_9C;
			case 'S':
				goto IL_95;
			case 'T':
				break;
			default:
				switch (c)
				{
				case 'c':
					goto IL_A3;
				default:
					switch (c)
					{
					case 'r':
						goto IL_9C;
					case 's':
						goto IL_95;
					case 't':
						break;
					default:
						if (c != 'N' && c != 'n')
						{
							return SuperController.KeyType.Invalid;
						}
						result = SuperController.KeyType.NSteam;
						goto IL_AC;
					}
					break;
				case 'e':
					goto IL_87;
				case 'f':
					goto IL_79;
				}
				break;
			}
			result = SuperController.KeyType.Teaser;
			goto IL_AC;
			IL_95:
			result = SuperController.KeyType.Steam;
			goto IL_AC;
			IL_9C:
			result = SuperController.KeyType.Retail;
			goto IL_AC;
		case 'E':
			goto IL_87;
		case 'F':
			break;
		}
		IL_79:
		result = SuperController.KeyType.Free;
		goto IL_AC;
		IL_87:
		result = SuperController.KeyType.Entertainer;
		goto IL_AC;
		IL_A3:
		result = SuperController.KeyType.Creator;
		IL_AC:
		SHA256 shaHash = SHA256.Create();
		string sha256Hash = SuperController.GetSha256Hash(shaHash, result.ToString() + key.ToUpper(), 3);
		string str = null;
		string b = null;
		switch (result)
		{
		case SuperController.KeyType.Free:
			str = "F";
			b = this.freeKey;
			break;
		case SuperController.KeyType.Teaser:
			str = "T";
			b = this.teaserKey;
			break;
		case SuperController.KeyType.Entertainer:
			str = "E";
			b = this.entertainerKey;
			break;
		case SuperController.KeyType.NSteam:
			str = "N";
			b = this.nsteamKey;
			break;
		case SuperController.KeyType.Steam:
			str = "S";
			b = this.steamKey;
			break;
		case SuperController.KeyType.Retail:
			str = "R";
			b = this.retailKey;
			break;
		case SuperController.KeyType.Creator:
			str = "C";
			b = this.creatorKey;
			break;
		}
		if (str + sha256Hash == b)
		{
			return result;
		}
		return SuperController.KeyType.Invalid;
	}

	// Token: 0x06005BEE RID: 23534 RVA: 0x0021DDEC File Offset: 0x0021C1EC
	protected IEnumerator SyncToKeyFilePackageRefresh()
	{
		if (this.keySyncingIndicator != null)
		{
			this.keySyncingIndicator.gameObject.SetActive(true);
		}
		yield return null;
		FileManager.Refresh();
		if (this.keySyncingIndicator != null)
		{
			this.keySyncingIndicator.gameObject.SetActive(false);
		}
		yield break;
	}

	// Token: 0x06005BEF RID: 23535 RVA: 0x0021DE08 File Offset: 0x0021C208
	protected void SyncToKeyFile(bool userInvoked = false)
	{
		SuperController.KeyType keyType = SuperController.KeyType.Free;
		if (this.keyFilePath != null && this.keyFilePath != string.Empty)
		{
			if (FileManager.FileExists(this.keyFilePath, false, false))
			{
				try
				{
					string aJSON = FileManager.ReadAllText(this.keyFilePath, true);
					JSONNode jsonnode = JSON.Parse(aJSON);
					if (jsonnode != null)
					{
						JSONClass asObject = jsonnode.AsObject;
						if (asObject != null)
						{
							foreach (string key in asObject.Keys)
							{
								switch (this.IsValidKey(key))
								{
								case SuperController.KeyType.Teaser:
									if (keyType < SuperController.KeyType.Teaser)
									{
										keyType = SuperController.KeyType.Teaser;
									}
									break;
								case SuperController.KeyType.Entertainer:
									if (keyType < SuperController.KeyType.Entertainer)
									{
										keyType = SuperController.KeyType.Entertainer;
									}
									break;
								case SuperController.KeyType.NSteam:
									if (keyType < SuperController.KeyType.NSteam)
									{
										keyType = SuperController.KeyType.NSteam;
									}
									break;
								case SuperController.KeyType.Steam:
									if (keyType < SuperController.KeyType.Steam)
									{
										keyType = SuperController.KeyType.Steam;
									}
									break;
								case SuperController.KeyType.Retail:
									if (keyType < SuperController.KeyType.Retail)
									{
										keyType = SuperController.KeyType.Retail;
									}
									break;
								case SuperController.KeyType.Creator:
									if (keyType < SuperController.KeyType.Creator)
									{
										keyType = SuperController.KeyType.Creator;
									}
									break;
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
						}
					}
					else
					{
						UnityEngine.Debug.LogError("Invalid key file");
						if (this.keyEntryStatus != null)
						{
							this.keyEntryStatus.text = "Invalid key file";
						}
					}
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogError("Exception while syncing key file " + ex.Message);
					if (this.keyEntryStatus != null)
					{
						this.keyEntryStatus.text = "Exception while syncing key file " + ex.Message;
					}
				}
			}
		}
		else if (Application.isEditor)
		{
			keyType = this.editorMimicHighestKey;
		}
		if (this.legacySteamKeyFilePath != null && this.legacySteamKeyFilePath != string.Empty && FileManager.FileExists(this.legacySteamKeyFilePath, false, false))
		{
			try
			{
				string aJSON2 = FileManager.ReadAllText(this.legacySteamKeyFilePath, true);
				JSONNode jsonnode2 = JSON.Parse(aJSON2);
				if (jsonnode2 != null)
				{
					JSONClass asObject2 = jsonnode2.AsObject;
					if (asObject2 != null)
					{
						foreach (string key2 in asObject2.Keys)
						{
							SuperController.KeyType keyType2 = this.IsValidKey(key2);
							if (keyType2 == SuperController.KeyType.Steam)
							{
								if (keyType < SuperController.KeyType.Steam)
								{
									keyType = SuperController.KeyType.Steam;
								}
							}
						}
					}
					else
					{
						UnityEngine.Debug.LogError("Invalid legacy Steam key file");
						if (this.keyEntryStatus != null)
						{
							this.keyEntryStatus.text = "Invalid legacy Steam key file";
						}
					}
				}
				else
				{
					UnityEngine.Debug.LogError("Invalid legacy Steam key file");
					if (this.keyEntryStatus != null)
					{
						this.keyEntryStatus.text = "Invalid legacy Steam key file";
					}
				}
			}
			catch (Exception ex2)
			{
				UnityEngine.Debug.LogError("Exception while syncing legacy Steam key file " + ex2.Message);
				if (this.keyEntryStatus != null)
				{
					this.keyEntryStatus.text = "Exception while syncing legacy Steam key file " + ex2.Message;
				}
			}
		}
		switch (keyType)
		{
		case SuperController.KeyType.Free:
			if (GlobalSceneOptions.singleton != null)
			{
				GlobalSceneOptions.singleton.disableAdvancedSceneEdit = true;
				GlobalSceneOptions.singleton.disableSaveSceneButton = true;
				GlobalSceneOptions.singleton.disableLoadSceneButton = true;
				GlobalSceneOptions.singleton.disableCustomUI = true;
				GlobalSceneOptions.singleton.disableBrowse = true;
				GlobalSceneOptions.singleton.disablePackages = true;
				GlobalSceneOptions.singleton.disablePromotional = false;
				GlobalSceneOptions.singleton.disableKeyInformation = false;
				GlobalSceneOptions.singleton.disableHub = false;
				GlobalSceneOptions.singleton.disableTermsOfUse = false;
			}
			else
			{
				this.disableAdvancedSceneEdit = true;
				this.disableSaveSceneButton = true;
				this.disableLoadSceneButton = true;
				this.disableCustomUI = true;
				this.disableBrowse = true;
				this.disablePackages = true;
				this.disablePromotional = false;
				this.disableKeyInformation = false;
				this.disableHub = false;
				this.disableTermsOfUse = false;
			}
			break;
		case SuperController.KeyType.Teaser:
			if (GlobalSceneOptions.singleton != null)
			{
				GlobalSceneOptions.singleton.disableAdvancedSceneEdit = true;
				GlobalSceneOptions.singleton.disableSaveSceneButton = true;
				GlobalSceneOptions.singleton.disableLoadSceneButton = false;
				GlobalSceneOptions.singleton.disableCustomUI = false;
				GlobalSceneOptions.singleton.disableBrowse = false;
				GlobalSceneOptions.singleton.disablePackages = false;
				GlobalSceneOptions.singleton.disablePromotional = false;
				GlobalSceneOptions.singleton.disableKeyInformation = false;
				GlobalSceneOptions.singleton.disableHub = false;
				GlobalSceneOptions.singleton.disableTermsOfUse = false;
			}
			else
			{
				this.disableAdvancedSceneEdit = true;
				this.disableSaveSceneButton = true;
				this.disableLoadSceneButton = false;
				this.disableCustomUI = false;
				this.disableBrowse = false;
				this.disablePackages = false;
				this.disablePromotional = false;
				this.disableKeyInformation = false;
				this.disableHub = false;
				this.disableTermsOfUse = false;
			}
			break;
		case SuperController.KeyType.Entertainer:
			if (GlobalSceneOptions.singleton != null)
			{
				GlobalSceneOptions.singleton.disableAdvancedSceneEdit = true;
				GlobalSceneOptions.singleton.disableSaveSceneButton = false;
				GlobalSceneOptions.singleton.disableLoadSceneButton = false;
				GlobalSceneOptions.singleton.disableCustomUI = false;
				GlobalSceneOptions.singleton.disableBrowse = false;
				GlobalSceneOptions.singleton.disablePackages = false;
				GlobalSceneOptions.singleton.disablePromotional = false;
				GlobalSceneOptions.singleton.disableKeyInformation = false;
				GlobalSceneOptions.singleton.disableHub = false;
				GlobalSceneOptions.singleton.disableTermsOfUse = false;
			}
			else
			{
				this.disableAdvancedSceneEdit = true;
				this.disableSaveSceneButton = false;
				this.disableLoadSceneButton = false;
				this.disableCustomUI = false;
				this.disableBrowse = false;
				this.disablePackages = false;
				this.disablePromotional = false;
				this.disableKeyInformation = false;
				this.disableHub = false;
				this.disableTermsOfUse = false;
			}
			break;
		case SuperController.KeyType.NSteam:
			if (GlobalSceneOptions.singleton != null)
			{
				GlobalSceneOptions.singleton.disableAdvancedSceneEdit = false;
				GlobalSceneOptions.singleton.disableSaveSceneButton = false;
				GlobalSceneOptions.singleton.disableLoadSceneButton = false;
				GlobalSceneOptions.singleton.disableCustomUI = false;
				GlobalSceneOptions.singleton.disableBrowse = false;
				GlobalSceneOptions.singleton.disablePackages = false;
				GlobalSceneOptions.singleton.disablePromotional = true;
				GlobalSceneOptions.singleton.disableKeyInformation = true;
				GlobalSceneOptions.singleton.disableHub = true;
				GlobalSceneOptions.singleton.disableTermsOfUse = true;
			}
			else
			{
				this.disableAdvancedSceneEdit = false;
				this.disableSaveSceneButton = false;
				this.disableLoadSceneButton = false;
				this.disableCustomUI = false;
				this.disableBrowse = false;
				this.disablePackages = false;
				this.disablePromotional = true;
				this.disableKeyInformation = true;
				this.disableHub = true;
				this.disableTermsOfUse = true;
			}
			break;
		case SuperController.KeyType.Steam:
			if (GlobalSceneOptions.singleton != null)
			{
				GlobalSceneOptions.singleton.disableAdvancedSceneEdit = false;
				GlobalSceneOptions.singleton.disableSaveSceneButton = false;
				GlobalSceneOptions.singleton.disableLoadSceneButton = false;
				GlobalSceneOptions.singleton.disableCustomUI = false;
				GlobalSceneOptions.singleton.disableBrowse = false;
				GlobalSceneOptions.singleton.disablePackages = false;
				GlobalSceneOptions.singleton.disablePromotional = false;
				GlobalSceneOptions.singleton.disableKeyInformation = true;
				GlobalSceneOptions.singleton.disableHub = false;
				GlobalSceneOptions.singleton.disableTermsOfUse = false;
			}
			else
			{
				this.disableAdvancedSceneEdit = false;
				this.disableSaveSceneButton = false;
				this.disableLoadSceneButton = false;
				this.disableCustomUI = false;
				this.disableBrowse = false;
				this.disablePackages = false;
				this.disablePromotional = false;
				this.disableKeyInformation = true;
				this.disableHub = false;
				this.disableTermsOfUse = false;
			}
			break;
		case SuperController.KeyType.Retail:
			if (GlobalSceneOptions.singleton != null)
			{
				GlobalSceneOptions.singleton.disableAdvancedSceneEdit = false;
				GlobalSceneOptions.singleton.disableSaveSceneButton = false;
				GlobalSceneOptions.singleton.disableLoadSceneButton = false;
				GlobalSceneOptions.singleton.disableCustomUI = false;
				GlobalSceneOptions.singleton.disableBrowse = false;
				GlobalSceneOptions.singleton.disablePackages = false;
				GlobalSceneOptions.singleton.disablePromotional = false;
				GlobalSceneOptions.singleton.disableKeyInformation = true;
				GlobalSceneOptions.singleton.disableHub = false;
				GlobalSceneOptions.singleton.disableTermsOfUse = false;
			}
			else
			{
				this.disableAdvancedSceneEdit = false;
				this.disableSaveSceneButton = false;
				this.disableLoadSceneButton = false;
				this.disableCustomUI = false;
				this.disableBrowse = false;
				this.disablePackages = false;
				this.disablePromotional = false;
				this.disableKeyInformation = true;
				this.disableHub = false;
				this.disableTermsOfUse = false;
			}
			break;
		case SuperController.KeyType.Creator:
			if (GlobalSceneOptions.singleton != null)
			{
				GlobalSceneOptions.singleton.disableAdvancedSceneEdit = false;
				GlobalSceneOptions.singleton.disableSaveSceneButton = false;
				GlobalSceneOptions.singleton.disableLoadSceneButton = false;
				GlobalSceneOptions.singleton.disableCustomUI = false;
				GlobalSceneOptions.singleton.disableBrowse = false;
				GlobalSceneOptions.singleton.disablePackages = false;
				GlobalSceneOptions.singleton.disablePromotional = false;
				GlobalSceneOptions.singleton.disableKeyInformation = false;
				GlobalSceneOptions.singleton.disableHub = false;
				GlobalSceneOptions.singleton.disableTermsOfUse = false;
			}
			else
			{
				this.disableAdvancedSceneEdit = false;
				this.disableSaveSceneButton = false;
				this.disableLoadSceneButton = false;
				this.disableCustomUI = false;
				this.disableBrowse = false;
				this.disablePackages = false;
				this.disablePromotional = false;
				this.disableKeyInformation = false;
				this.disableHub = false;
				this.disableTermsOfUse = false;
			}
			break;
		}
		if (this.freeKeyTransform != null)
		{
			this.freeKeyTransform.gameObject.SetActive(keyType == SuperController.KeyType.Free);
		}
		if (this.teaserKeyTransform != null)
		{
			this.teaserKeyTransform.gameObject.SetActive(keyType == SuperController.KeyType.Teaser);
		}
		if (this.entertainerKeyTransform != null)
		{
			this.entertainerKeyTransform.gameObject.SetActive(keyType == SuperController.KeyType.Entertainer);
		}
		if (this.creatorKeyTransform != null)
		{
			this.creatorKeyTransform.gameObject.SetActive(keyType == SuperController.KeyType.Creator);
		}
		this.SyncUIToUnlockLevel();
		FileManager.packagesEnabled = !this.packagesDisabled;
		if (userInvoked)
		{
			base.StartCoroutine(this.SyncToKeyFilePackageRefresh());
		}
		else
		{
			FileManager.Refresh();
		}
	}

	// Token: 0x06005BF0 RID: 23536 RVA: 0x0021E880 File Offset: 0x0021CC80
	public void AddKey()
	{
		if (this.keyFilePath != null && this.keyFilePath != string.Empty && this.keyInputField != null)
		{
			JSONClass jsonclass;
			if (FileManager.FileExists(this.keyFilePath, true, false))
			{
				string aJSON = FileManager.ReadAllText(this.keyFilePath, true);
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
			SuperController.KeyType keyType = this.IsValidKey(text);
			if (keyType != SuperController.KeyType.Invalid)
			{
				if (this.keyEntryStatus != null)
				{
					this.keyEntryStatus.text = "Key accepted";
				}
				jsonclass[text].AsBool = true;
				string text2 = jsonclass.ToString(string.Empty);
				try
				{
					FileManager.CreateDirectory(Path.GetDirectoryName(this.keyFilePath));
					FileManager.WriteAllText(this.keyFilePath, text2);
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogError("Exception while storing key " + ex.Message);
					if (this.keyEntryStatus != null)
					{
						this.keyEntryStatus.text = "Exception while storing key " + ex.Message;
					}
					return;
				}
				this.SyncToKeyFile(true);
			}
			else if (this.keyEntryStatus != null)
			{
				this.keyEntryStatus.text = "Invalid key";
			}
		}
		else if (this.keyEntryStatus != null)
		{
			this.keyEntryStatus.text = "Keys not needed";
		}
	}

	// Token: 0x06005BF1 RID: 23537 RVA: 0x0021EA48 File Offset: 0x0021CE48
	public void OutputEncryptedKey(string keyval)
	{
		char c = keyval[0];
		SuperController.KeyType keyType;
		string str;
		switch (c)
		{
		case 'C':
			goto IL_A0;
		default:
			switch (c)
			{
			case 'R':
				goto IL_C7;
			case 'S':
				goto IL_BA;
			case 'T':
				break;
			default:
				switch (c)
				{
				case 'c':
					goto IL_A0;
				default:
					switch (c)
					{
					case 'r':
						goto IL_C7;
					case 's':
						goto IL_BA;
					case 't':
						break;
					default:
						if (c != 'N' && c != 'n')
						{
							return;
						}
						keyType = SuperController.KeyType.NSteam;
						str = "N";
						goto IL_D5;
					}
					break;
				case 'e':
					goto IL_93;
				case 'f':
					goto IL_79;
				}
				break;
			}
			keyType = SuperController.KeyType.Teaser;
			str = "T";
			goto IL_D5;
			IL_BA:
			keyType = SuperController.KeyType.Steam;
			str = "S";
			goto IL_D5;
			IL_C7:
			keyType = SuperController.KeyType.Retail;
			str = "R";
			goto IL_D5;
		case 'E':
			goto IL_93;
		case 'F':
			break;
		}
		IL_79:
		keyType = SuperController.KeyType.Free;
		str = "F";
		goto IL_D5;
		IL_93:
		keyType = SuperController.KeyType.Entertainer;
		str = "E";
		goto IL_D5;
		IL_A0:
		keyType = SuperController.KeyType.Creator;
		str = "C";
		IL_D5:
		SHA256 shaHash = SHA256.Create();
		string sha256Hash = SuperController.GetSha256Hash(shaHash, keyType.ToString() + keyval.ToUpper(), 3);
		string str2 = str + sha256Hash;
		UnityEngine.Debug.Log("Encrypted value for key " + keyval + " is " + str2);
	}

	// Token: 0x06005BF2 RID: 23538 RVA: 0x0021EB72 File Offset: 0x0021CF72
	public void GetScenePathDialog(FileBrowserCallback callback)
	{
		this.LoadDialog(this.lastScenePathDir, false);
		this.fileBrowserUI.SetTitle("Select File");
		this.fileBrowserUI.Show(callback, true);
	}

	// Token: 0x06005BF3 RID: 23539 RVA: 0x0021EBA0 File Offset: 0x0021CFA0
	protected void GetMediaPathDialogInternal(string filter = "", string suggestedFolder = null, bool fullComputerBrowse = true, bool showDirs = true, bool showKeepOpt = false, string fileRemovePrefix = null, bool hideExtension = false, List<ShortCut> shortCuts = null, bool browseVarFilesAsDirectories = true, bool showInstallFolderInDirectoryList = false)
	{
		string text = this.savesDirResolved + "scene";
		if (suggestedFolder != null && suggestedFolder != string.Empty)
		{
			if (FileManager.DirectoryExists(suggestedFolder, false, false))
			{
				text = suggestedFolder;
			}
			else
			{
				text = ".";
				fullComputerBrowse = true;
			}
		}
		else if (this.lastMediaDir != string.Empty && FileManager.DirectoryExists(this.lastMediaDir, false, false))
		{
			text = this.lastMediaDir;
		}
		List<ShortCut> list = shortCuts;
		if (fullComputerBrowse)
		{
			VarDirectoryEntry varDirectoryEntry = FileManager.GetVarDirectoryEntry(text);
			if (varDirectoryEntry != null)
			{
				text = varDirectoryEntry.Path;
			}
			else
			{
				text = Path.GetFullPath(text);
			}
			if (list == null)
			{
				list = new List<ShortCut>();
				list.Add(new ShortCut
				{
					package = string.Empty,
					displayName = "Default",
					path = text
				});
				list.Add(new ShortCut
				{
					package = string.Empty,
					displayName = "Addon Packages",
					path = "AddonPackages"
				});
			}
		}
		this.mediaFileBrowserUI.fileRemovePrefix = fileRemovePrefix;
		this.mediaFileBrowserUI.hideExtension = hideExtension;
		this.mediaFileBrowserUI.keepOpen = false;
		this.mediaFileBrowserUI.fileFormat = filter;
		this.mediaFileBrowserUI.defaultPath = text;
		this.mediaFileBrowserUI.showDirs = showDirs;
		this.mediaFileBrowserUI.SetShortCuts(list, false);
		this.mediaFileBrowserUI.browseVarFilesAsDirectories = browseVarFilesAsDirectories;
		this.mediaFileBrowserUI.showInstallFolderInDirectoryList = showInstallFolderInDirectoryList;
		this.mediaFileBrowserUI.SetTextEntry(false);
	}

	// Token: 0x06005BF4 RID: 23540 RVA: 0x0021ED34 File Offset: 0x0021D134
	public void GetMediaPathDialog(FileBrowserCallback callback, string filter = "", string suggestedFolder = null, bool fullComputerBrowse = true, bool showDirs = true, bool showKeepOpt = false, string fileRemovePrefix = null, bool hideExtenstion = false, List<ShortCut> shortCuts = null, bool browseVarFilesAsDirectories = true, bool showInstallFolderInDirectoryList = false)
	{
		if (!this.browseDisabled)
		{
			this.GetMediaPathDialogInternal(filter, suggestedFolder, fullComputerBrowse, showDirs, showKeepOpt, fileRemovePrefix, hideExtenstion, shortCuts, browseVarFilesAsDirectories, showInstallFolderInDirectoryList);
			this.mediaFileBrowserUI.Show(callback, true);
		}
		else
		{
			SuperController.LogMessage("Please back this project on Patreon at https://www.patreon.com/meshedvr to unlock this feature!");
			if (callback != null)
			{
				callback(string.Empty);
			}
		}
	}

	// Token: 0x06005BF5 RID: 23541 RVA: 0x0021ED94 File Offset: 0x0021D194
	public void GetMediaPathDialog(FileBrowserFullCallback callback, string filter = "", string suggestedFolder = null, bool fullComputerBrowse = true, bool showDirs = true, bool showKeepOpt = false, string fileRemovePrefix = null, bool hideExtenstion = false, List<ShortCut> shortCuts = null, bool browseVarFilesAsDirectories = true, bool showInstallFolderInDirectoryList = false)
	{
		if (!this.browseDisabled)
		{
			this.GetMediaPathDialogInternal(filter, suggestedFolder, fullComputerBrowse, showDirs, showKeepOpt, fileRemovePrefix, hideExtenstion, shortCuts, browseVarFilesAsDirectories, showInstallFolderInDirectoryList);
			this.mediaFileBrowserUI.Show(callback, true);
		}
		else
		{
			SuperController.LogMessage("Please back this project on Patreon at https://www.patreon.com/meshedvr to unlock this feature!");
			if (callback != null)
			{
				callback(string.Empty, true);
			}
		}
	}

	// Token: 0x06005BF6 RID: 23542 RVA: 0x0021EDF2 File Offset: 0x0021D1F2
	protected void TestDirectoryCallback(string dir)
	{
		UnityEngine.Debug.Log("Selected dir " + dir);
	}

	// Token: 0x06005BF7 RID: 23543 RVA: 0x0021EE04 File Offset: 0x0021D204
	public void TestDirectoryPathBrowse()
	{
		this.GetDirectoryPathDialog(new FileBrowserCallback(this.TestDirectoryCallback), null, null, true);
	}

	// Token: 0x06005BF8 RID: 23544 RVA: 0x0021EE1C File Offset: 0x0021D21C
	public void GetDirectoryPathDialog(FileBrowserCallback callback, string suggestedFolder = null, List<ShortCut> shortCuts = null, bool fullComputerBrowse = true)
	{
		if (!this.browseDisabled)
		{
			string text = this.savesDirResolved + "scene";
			if (suggestedFolder != null && suggestedFolder != string.Empty && FileManager.DirectoryExists(suggestedFolder, false, false))
			{
				text = suggestedFolder;
			}
			else if (this.lastBrowseDir != string.Empty && FileManager.DirectoryExists(this.lastBrowseDir, false, false))
			{
				text = this.lastBrowseDir;
			}
			if (fullComputerBrowse)
			{
				text = Path.GetFullPath(text);
			}
			this.directoryBrowserUI.fileFormat = string.Empty;
			this.directoryBrowserUI.defaultPath = text;
			this.directoryBrowserUI.SetShortCuts(shortCuts, false);
			this.directoryBrowserUI.SetTextEntry(true);
			this.directoryBrowserUI.Show(callback, true);
		}
		else
		{
			SuperController.LogMessage("Please back this project on Patreon at https://www.patreon.com/meshedvr to unlock this feature!");
			if (callback != null)
			{
				callback(string.Empty);
			}
		}
	}

	// Token: 0x17000D87 RID: 3463
	// (get) Token: 0x06005BF9 RID: 23545 RVA: 0x0021EF0D File Offset: 0x0021D30D
	// (set) Token: 0x06005BFA RID: 23546 RVA: 0x0021EF14 File Offset: 0x0021D314
	public string currentSaveDir
	{
		get
		{
			return FileManager.CurrentSaveDir;
		}
		set
		{
			FileManager.SetSaveDir(value, true);
		}
	}

	// Token: 0x17000D88 RID: 3464
	// (get) Token: 0x06005BFB RID: 23547 RVA: 0x0021EF1D File Offset: 0x0021D31D
	// (set) Token: 0x06005BFC RID: 23548 RVA: 0x0021EF24 File Offset: 0x0021D324
	public string currentLoadDir
	{
		get
		{
			return FileManager.CurrentLoadDir;
		}
		set
		{
			FileManager.SetLoadDir(value, true);
		}
	}

	// Token: 0x17000D89 RID: 3465
	// (get) Token: 0x06005BFD RID: 23549 RVA: 0x0021EF2D File Offset: 0x0021D32D
	public string LoadedSceneName
	{
		get
		{
			return this.loadedName;
		}
	}

	// Token: 0x17000D8A RID: 3466
	// (get) Token: 0x06005BFE RID: 23550 RVA: 0x0021EF35 File Offset: 0x0021D335
	public bool isLoading
	{
		get
		{
			return this._isLoading;
		}
	}

	// Token: 0x06005BFF RID: 23551 RVA: 0x0021EF40 File Offset: 0x0021D340
	protected void SetSavesDirFromCommandline()
	{
		if (!Application.isEditor)
		{
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			for (int i = 0; i < commandLineArgs.Length; i++)
			{
				if (commandLineArgs[i] == "-savesdir" && i + 1 < commandLineArgs.Length)
				{
					this.savesDir = commandLineArgs[i + 1];
					if (this.savesDir[this.savesDir.Length - 1] != '\\')
					{
						this.savesDir += "\\";
					}
				}
			}
		}
	}

	// Token: 0x17000D8B RID: 3467
	// (get) Token: 0x06005C00 RID: 23552 RVA: 0x0021EFCE File Offset: 0x0021D3CE
	public string savesDirResolved
	{
		get
		{
			if (Application.isEditor)
			{
				return this.savesDirEditor;
			}
			return this.savesDir;
		}
	}

	// Token: 0x06005C01 RID: 23553 RVA: 0x0021EFE8 File Offset: 0x0021D3E8
	public void StartScene()
	{
		if (this.embeddedJSONScene != null)
		{
			this.LoadFromJSONEmbed(this.embeddedJSONScene, false, false);
		}
		else
		{
			if (File.Exists(this.savesDirResolved + this.startSceneName))
			{
				this.Load(this.savesDirResolved + this.startSceneName);
			}
			else
			{
				this.Load(this.savesDirResolved + this.startSceneAltName);
			}
			this.loadedName = string.Empty;
		}
	}

	// Token: 0x06005C02 RID: 23554 RVA: 0x0021F074 File Offset: 0x0021D474
	public void StartSceneForEdit()
	{
		if (this.embeddedJSONScene != null)
		{
			this.LoadFromJSONEmbed(this.embeddedJSONScene, false, false);
		}
		else
		{
			if (File.Exists(this.savesDirResolved + this.startSceneName))
			{
				this.LoadForEdit(this.savesDirResolved + this.startSceneName);
			}
			else
			{
				this.LoadForEdit(this.savesDirResolved + this.startSceneAltName);
			}
			this.loadedName = string.Empty;
		}
	}

	// Token: 0x06005C03 RID: 23555 RVA: 0x0021F100 File Offset: 0x0021D500
	public void NewScene()
	{
		if (this.newJSONEmbedScene != null)
		{
			this.LoadFromJSONEmbed(this.newJSONEmbedScene, false, true);
		}
		else
		{
			if (File.Exists(this.savesDirResolved + this.newSceneName))
			{
				this.LoadForEdit(this.savesDirResolved + this.newSceneName);
			}
			else
			{
				this.LoadForEdit(this.savesDirResolved + this.newSceneAltName);
			}
			this.loadedName = string.Empty;
		}
	}

	// Token: 0x06005C04 RID: 23556 RVA: 0x0021F18C File Offset: 0x0021D58C
	public void NewScenePlayMode()
	{
		if (this.newJSONEmbedScene != null)
		{
			this.LoadFromJSONEmbed(this.newJSONEmbedScene, false, false);
		}
		else
		{
			if (File.Exists(this.savesDirResolved + this.newSceneName))
			{
				this.Load(this.savesDirResolved + this.newSceneName);
			}
			else
			{
				this.Load(this.savesDirResolved + this.newSceneAltName);
			}
			this.loadedName = string.Empty;
		}
	}

	// Token: 0x06005C05 RID: 23557 RVA: 0x0021F218 File Offset: 0x0021D618
	public void ClearScene()
	{
		if (Application.isPlaying)
		{
			if (!this._isLoading)
			{
				this.onStartScene = false;
				this.gameMode = SuperController.GameMode.Edit;
				this.loadedName = string.Empty;
				this._isLoading = true;
				base.StartCoroutine(this.LoadCo(true, false));
			}
			else
			{
				UnityEngine.Debug.LogWarning("Already loading file " + this.loadedName + ". Can't clear until complete");
			}
		}
	}

	// Token: 0x06005C06 RID: 23558 RVA: 0x0021F288 File Offset: 0x0021D688
	public void SaveSceneDialog()
	{
		this.SaveSceneDialog(new FileBrowserCallback(this.SaveFromDialog));
	}

	// Token: 0x06005C07 RID: 23559 RVA: 0x0021F29C File Offset: 0x0021D69C
	public void SaveSceneLegacyPackageDialog()
	{
		this.SaveSceneDialog(new FileBrowserCallback(this.SaveLegacyPackageFromDialog));
	}

	// Token: 0x06005C08 RID: 23560 RVA: 0x0021F2B0 File Offset: 0x0021D6B0
	public void SaveSceneNewAddonPackageDialog()
	{
		this.SaveSceneDialog(new FileBrowserCallback(this.SaveAndAddToNewPackageFromDialog));
	}

	// Token: 0x06005C09 RID: 23561 RVA: 0x0021F2C4 File Offset: 0x0021D6C4
	public void SaveSceneCurrentAddonPackageDialog()
	{
		this.SaveSceneDialog(new FileBrowserCallback(this.SaveAndAddToCurrentPackageFromDialog));
	}

	// Token: 0x06005C0A RID: 23562 RVA: 0x0021F2D8 File Offset: 0x0021D6D8
	public void OpenPackageBuilder()
	{
		this.activeUI = SuperController.ActiveUI.PackageBuilder;
	}

	// Token: 0x06005C0B RID: 23563 RVA: 0x0021F2E1 File Offset: 0x0021D6E1
	public void OpenPackageManager()
	{
		this.ShowMainHUDAuto();
		this.activeUI = SuperController.ActiveUI.PackageManager;
	}

	// Token: 0x06005C0C RID: 23564 RVA: 0x0021F2F0 File Offset: 0x0021D6F0
	public void OpenPackageDownloader()
	{
		this.ShowMainHUDAuto();
		this.activeUI = SuperController.ActiveUI.PackageDownloader;
	}

	// Token: 0x06005C0D RID: 23565 RVA: 0x0021F300 File Offset: 0x0021D700
	public void OpenPackageInManager(string packageUid)
	{
		packageUid = Regex.Replace(packageUid, ":.*", string.Empty);
		this.OpenPackageManager();
		if (this.packageManager != null)
		{
			this.packageManager.LoadMetaFromPackageUid(packageUid, true);
		}
	}

	// Token: 0x06005C0E RID: 23566 RVA: 0x0021F338 File Offset: 0x0021D738
	public void RescanPackages()
	{
		FileManager.Refresh();
	}

	// Token: 0x06005C0F RID: 23567 RVA: 0x0021F33F File Offset: 0x0021D73F
	protected void ClearFileBrowsersCurrentPath()
	{
		if (this.fileBrowserUI != null)
		{
			this.fileBrowserUI.ClearCurrentPath();
		}
		if (this.fileBrowserWorldUI != null)
		{
			this.fileBrowserWorldUI.ClearCurrentPath();
		}
	}

	// Token: 0x06005C10 RID: 23568 RVA: 0x0021F379 File Offset: 0x0021D779
	protected void OnPackageRefresh()
	{
		this.ClearFileBrowsersCurrentPath();
		this.SyncVamX();
	}

	// Token: 0x06005C11 RID: 23569 RVA: 0x0021F387 File Offset: 0x0021D787
	public void OpenLinkInBrowser(string url)
	{
		if (this.onlineBrowser != null)
		{
			this.activeUI = SuperController.ActiveUI.OnlineBrowser;
			this.onlineBrowser.url = url;
		}
	}

	// Token: 0x06005C12 RID: 23570 RVA: 0x0021F3B0 File Offset: 0x0021D7B0
	public void SaveSceneDialog(FileBrowserCallback callback)
	{
		try
		{
			string text = this.savesDirResolved + "scene";
			this.fileBrowserUI.SetShortCuts(null, false);
			string suggestedBrowserDirectoryFromDirectoryPath = FileManager.GetSuggestedBrowserDirectoryFromDirectoryPath(text, this.lastLoadDir, false);
			if (suggestedBrowserDirectoryFromDirectoryPath != null && FileManager.DirectoryExists(suggestedBrowserDirectoryFromDirectoryPath, false, false))
			{
				text = suggestedBrowserDirectoryFromDirectoryPath;
			}
			if (!FileManager.DirectoryExists(text, true, false))
			{
				FileManager.CreateDirectory(text);
			}
			this.fileBrowserUI.defaultPath = text;
			this.activeUI = SuperController.ActiveUI.None;
			this.fileBrowserUI.SetTitle("Select Save File");
			this.fileBrowserUI.SetTextEntry(true);
			this.fileBrowserUI.fileFormat = "json|vac|zip";
			this.fileBrowserUI.Show(callback, true);
			if (this.fileBrowserUI.fileEntryField != null)
			{
				string text2 = ((int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds).ToString();
				this.fileBrowserUI.fileEntryField.text = text2;
				this.fileBrowserUI.ActivateFileNameField();
			}
		}
		catch (Exception arg)
		{
			SuperController.LogError("Exception during open of save scene dialog: " + arg);
		}
	}

	// Token: 0x06005C13 RID: 23571 RVA: 0x0021F4F8 File Offset: 0x0021D8F8
	public void SaveConfirm(string option)
	{
		if (this._lastActiveUI == SuperController.ActiveUI.MultiButtonPanel)
		{
			this.activeUI = SuperController.ActiveUI.None;
		}
		else
		{
			this.activeUI = this._lastActiveUI;
		}
		this.multiButtonPanel.gameObject.SetActive(false);
		this.multiButtonPanel.buttonCallback = null;
		if (option == "Save New")
		{
			int num = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
			this.loadedName = this.savesDirResolved + "scene\\" + num.ToString() + ".json";
			this.SaveInternal(this.loadedName, null, true, true, null, false, false);
		}
		else if (this.loadedName != null && this.loadedName != string.Empty && option == "Overwrite Current")
		{
			this.SaveInternal(this.loadedName, null, true, true, null, true, false);
		}
	}

	// Token: 0x06005C14 RID: 23572 RVA: 0x0021F5F7 File Offset: 0x0021D9F7
	public void SaveAddDependency(Atom saveAtom)
	{
		if (this._saveQueue != null)
		{
			this._saveQueue.Add(saveAtom);
		}
	}

	// Token: 0x06005C15 RID: 23573 RVA: 0x0021F610 File Offset: 0x0021DA10
	public string NormalizeSavePath(string path)
	{
		string result = path;
		if (path != null && path != string.Empty && path != "/" && path != "NULL")
		{
			result = FileManager.NormalizeSavePath(path);
			if (this.packageMode)
			{
				string fileName = Path.GetFileName(path);
				result = this.AddFileToPackage(path, fileName);
			}
		}
		return result;
	}

	// Token: 0x06005C16 RID: 23574 RVA: 0x0021F678 File Offset: 0x0021DA78
	protected void SaveFromDialog(string saveName)
	{
		SuperController.<SaveFromDialog>c__AnonStorey8 <SaveFromDialog>c__AnonStorey = new SuperController.<SaveFromDialog>c__AnonStorey8();
		<SaveFromDialog>c__AnonStorey.saveName = saveName;
		<SaveFromDialog>c__AnonStorey.$this = this;
		if (<SaveFromDialog>c__AnonStorey.saveName != null && <SaveFromDialog>c__AnonStorey.saveName != string.Empty)
		{
			if (!<SaveFromDialog>c__AnonStorey.saveName.EndsWith(".json"))
			{
				<SaveFromDialog>c__AnonStorey.saveName += ".json";
			}
			if (FileManager.FileExists(<SaveFromDialog>c__AnonStorey.saveName, false, false) && this.overwriteConfirmButton != null && this.overwriteConfirmPanel != null)
			{
				this.overwriteConfirmButton.onClick.RemoveAllListeners();
				this.overwriteConfirmButton.onClick.AddListener(new UnityAction(<SaveFromDialog>c__AnonStorey.<>m__0));
				this.overwriteConfirmPanel.gameObject.SetActive(true);
				if (this.overwriteConfirmPathText != null)
				{
					this.overwriteConfirmPathText.text = <SaveFromDialog>c__AnonStorey.saveName;
				}
			}
			else
			{
				this.SaveInternal(<SaveFromDialog>c__AnonStorey.saveName, null, true, true, null, false, false);
			}
		}
	}

	// Token: 0x06005C17 RID: 23575 RVA: 0x0021F790 File Offset: 0x0021DB90
	protected void SaveLegacyPackageFromDialog(string saveName)
	{
		SuperController.<SaveLegacyPackageFromDialog>c__AnonStorey9 <SaveLegacyPackageFromDialog>c__AnonStorey = new SuperController.<SaveLegacyPackageFromDialog>c__AnonStorey9();
		<SaveLegacyPackageFromDialog>c__AnonStorey.saveName = saveName;
		<SaveLegacyPackageFromDialog>c__AnonStorey.$this = this;
		if (<SaveLegacyPackageFromDialog>c__AnonStorey.saveName != null && <SaveLegacyPackageFromDialog>c__AnonStorey.saveName != string.Empty)
		{
			if (!<SaveLegacyPackageFromDialog>c__AnonStorey.saveName.EndsWith(".vac"))
			{
				<SaveLegacyPackageFromDialog>c__AnonStorey.saveName += ".vac";
			}
			if (FileManager.FileExists(<SaveLegacyPackageFromDialog>c__AnonStorey.saveName, false, false) && this.overwriteConfirmButton != null && this.overwriteConfirmPanel != null)
			{
				this.overwriteConfirmButton.onClick.RemoveAllListeners();
				this.overwriteConfirmButton.onClick.AddListener(new UnityAction(<SaveLegacyPackageFromDialog>c__AnonStorey.<>m__0));
				this.overwriteConfirmPanel.gameObject.SetActive(true);
				if (this.overwriteConfirmPathText != null)
				{
					this.overwriteConfirmPathText.text = <SaveLegacyPackageFromDialog>c__AnonStorey.saveName;
				}
			}
			else
			{
				this.SavePackage(<SaveLegacyPackageFromDialog>c__AnonStorey.saveName, false);
			}
		}
	}

	// Token: 0x06005C18 RID: 23576 RVA: 0x0021F8A4 File Offset: 0x0021DCA4
	protected void SaveAndAddToCurrentPackageFromDialog(string saveName)
	{
		SuperController.<SaveAndAddToCurrentPackageFromDialog>c__AnonStoreyA <SaveAndAddToCurrentPackageFromDialog>c__AnonStoreyA = new SuperController.<SaveAndAddToCurrentPackageFromDialog>c__AnonStoreyA();
		<SaveAndAddToCurrentPackageFromDialog>c__AnonStoreyA.saveName = saveName;
		<SaveAndAddToCurrentPackageFromDialog>c__AnonStoreyA.$this = this;
		if (<SaveAndAddToCurrentPackageFromDialog>c__AnonStoreyA.saveName != null && <SaveAndAddToCurrentPackageFromDialog>c__AnonStoreyA.saveName != string.Empty)
		{
			if (!<SaveAndAddToCurrentPackageFromDialog>c__AnonStoreyA.saveName.EndsWith(".json"))
			{
				<SaveAndAddToCurrentPackageFromDialog>c__AnonStoreyA.saveName += ".json";
			}
			if (FileManager.FileExists(<SaveAndAddToCurrentPackageFromDialog>c__AnonStoreyA.saveName, false, false) && this.overwriteConfirmButton != null && this.overwriteConfirmPanel != null)
			{
				this.overwriteConfirmButton.onClick.RemoveAllListeners();
				this.overwriteConfirmButton.onClick.AddListener(new UnityAction(<SaveAndAddToCurrentPackageFromDialog>c__AnonStoreyA.<>m__0));
				this.overwriteConfirmPanel.gameObject.SetActive(true);
				if (this.overwriteConfirmPathText != null)
				{
					this.overwriteConfirmPathText.text = <SaveAndAddToCurrentPackageFromDialog>c__AnonStoreyA.saveName;
				}
			}
			else
			{
				this.SaveAndAddToCurrentPackage(<SaveAndAddToCurrentPackageFromDialog>c__AnonStoreyA.saveName, false);
			}
		}
	}

	// Token: 0x06005C19 RID: 23577 RVA: 0x0021F9B8 File Offset: 0x0021DDB8
	protected void SaveAndAddToNewPackageFromDialog(string saveName)
	{
		SuperController.<SaveAndAddToNewPackageFromDialog>c__AnonStoreyB <SaveAndAddToNewPackageFromDialog>c__AnonStoreyB = new SuperController.<SaveAndAddToNewPackageFromDialog>c__AnonStoreyB();
		<SaveAndAddToNewPackageFromDialog>c__AnonStoreyB.saveName = saveName;
		<SaveAndAddToNewPackageFromDialog>c__AnonStoreyB.$this = this;
		if (<SaveAndAddToNewPackageFromDialog>c__AnonStoreyB.saveName != null && <SaveAndAddToNewPackageFromDialog>c__AnonStoreyB.saveName != string.Empty)
		{
			if (!<SaveAndAddToNewPackageFromDialog>c__AnonStoreyB.saveName.EndsWith(".json"))
			{
				<SaveAndAddToNewPackageFromDialog>c__AnonStoreyB.saveName += ".json";
			}
			if (FileManager.FileExists(<SaveAndAddToNewPackageFromDialog>c__AnonStoreyB.saveName, false, false) && this.overwriteConfirmButton != null && this.overwriteConfirmPanel != null)
			{
				this.overwriteConfirmButton.onClick.RemoveAllListeners();
				this.overwriteConfirmButton.onClick.AddListener(new UnityAction(<SaveAndAddToNewPackageFromDialog>c__AnonStoreyB.<>m__0));
				this.overwriteConfirmPanel.gameObject.SetActive(true);
				if (this.overwriteConfirmPathText != null)
				{
					this.overwriteConfirmPathText.text = <SaveAndAddToNewPackageFromDialog>c__AnonStoreyB.saveName;
				}
			}
			else
			{
				this.SaveAndAddToNewPackage(<SaveAndAddToNewPackageFromDialog>c__AnonStoreyB.saveName, false);
			}
		}
	}

	// Token: 0x06005C1A RID: 23578 RVA: 0x0021FAC9 File Offset: 0x0021DEC9
	public void Save(string saveName)
	{
		if (saveName != string.Empty)
		{
			this.Save(saveName, null, true, true, null, false);
		}
	}

	// Token: 0x06005C1B RID: 23579 RVA: 0x0021FAE8 File Offset: 0x0021DEE8
	protected void SaveAndAddToCurrentPackage(string saveName, bool isOverwrite = false)
	{
		SuperController.<SaveAndAddToCurrentPackage>c__AnonStoreyC <SaveAndAddToCurrentPackage>c__AnonStoreyC = new SuperController.<SaveAndAddToCurrentPackage>c__AnonStoreyC();
		<SaveAndAddToCurrentPackage>c__AnonStoreyC.saveName = saveName;
		<SaveAndAddToCurrentPackage>c__AnonStoreyC.$this = this;
		if (<SaveAndAddToCurrentPackage>c__AnonStoreyC.saveName != string.Empty)
		{
			this.SaveInternal(<SaveAndAddToCurrentPackage>c__AnonStoreyC.saveName, null, true, true, new SuperController.ScreenShotCallback(<SaveAndAddToCurrentPackage>c__AnonStoreyC.<>m__0), isOverwrite, false);
		}
	}

	// Token: 0x06005C1C RID: 23580 RVA: 0x0021FB3C File Offset: 0x0021DF3C
	protected void SaveAndAddToNewPackage(string saveName, bool isOverwrite = false)
	{
		SuperController.<SaveAndAddToNewPackage>c__AnonStoreyD <SaveAndAddToNewPackage>c__AnonStoreyD = new SuperController.<SaveAndAddToNewPackage>c__AnonStoreyD();
		<SaveAndAddToNewPackage>c__AnonStoreyD.saveName = saveName;
		<SaveAndAddToNewPackage>c__AnonStoreyD.$this = this;
		if (<SaveAndAddToNewPackage>c__AnonStoreyD.saveName != string.Empty)
		{
			this.SaveInternal(<SaveAndAddToNewPackage>c__AnonStoreyD.saveName, null, true, true, new SuperController.ScreenShotCallback(<SaveAndAddToNewPackage>c__AnonStoreyD.<>m__0), isOverwrite, false);
		}
	}

	// Token: 0x06005C1D RID: 23581 RVA: 0x0021FB90 File Offset: 0x0021DF90
	public JSONClass GetSaveJSON(Atom specificAtom = null, bool includePhysical = true, bool includeAppearance = true)
	{
		JSONClass jsonclass = new JSONClass();
		if (this._saveQueue == null)
		{
			this._saveQueue = new List<Atom>();
		}
		else
		{
			this._saveQueue.Clear();
		}
		if (specificAtom == null)
		{
			if (this.headPossessedController != null)
			{
				jsonclass["headPossessedController"] = this.headPossessedController.containingAtom.uid + ":" + this.headPossessedController.name;
			}
			if (this.playerNavCollider != null)
			{
				jsonclass["playerNavCollider"] = this.playerNavCollider.containingAtom.uid + ":" + this.playerNavCollider.name;
			}
			if (this.worldScaleSlider != null)
			{
				SliderControl component = this.worldScaleSlider.GetComponent<SliderControl>();
				if (component == null || component.defaultValue != this.worldScale)
				{
					jsonclass["worldScale"].AsFloat = this.worldScale;
				}
			}
			if (this.playerHeightAdjustSlider != null)
			{
				SliderControl component2 = this.playerHeightAdjustSlider.GetComponent<SliderControl>();
				if (component2 == null || component2.defaultValue != this._playerHeightAdjust)
				{
					jsonclass["playerHeightAdjust"].AsFloat = this._playerHeightAdjust;
				}
			}
			if (this.MonitorCenterCamera != null)
			{
				Vector3 localEulerAngles = this.MonitorCenterCamera.transform.localEulerAngles;
				jsonclass["monitorCameraRotation"]["x"].AsFloat = localEulerAngles.x;
				jsonclass["monitorCameraRotation"]["y"].AsFloat = localEulerAngles.y;
				jsonclass["monitorCameraRotation"]["z"].AsFloat = localEulerAngles.z;
			}
			if (this.useSceneLoadPositionToggle != null)
			{
				jsonclass["useSceneLoadPosition"].AsBool = this._useSceneLoadPosition;
			}
			if (this.useSceneLoadPosition)
			{
				this.MoveToSceneLoadPosition();
			}
			JSONArray value = new JSONArray();
			jsonclass["atoms"] = value;
			foreach (Atom atom in this.atomsList)
			{
				atom.Store(value, true, true);
			}
		}
		else
		{
			JSONArray value2 = new JSONArray();
			jsonclass["atoms"] = value2;
			specificAtom.Store(value2, includePhysical, includeAppearance);
			if (includePhysical)
			{
				foreach (Atom atom2 in this._saveQueue)
				{
					atom2.Store(value2, includePhysical, includeAppearance);
				}
			}
		}
		return jsonclass;
	}

	// Token: 0x06005C1E RID: 23582 RVA: 0x0021FEA4 File Offset: 0x0021E2A4
	public void Save(string saveName = "Saves\\scene\\savefile.json", Atom specificAtom = null, bool includePhysical = true, bool includeAppearance = true, SuperController.ScreenShotCallback callback = null, bool isOverwrite = false)
	{
		this.SaveInternal(saveName, specificAtom, includePhysical, includeAppearance, callback, isOverwrite, true);
	}

	// Token: 0x06005C1F RID: 23583 RVA: 0x0021FEB6 File Offset: 0x0021E2B6
	public void SaveFromAtom(string saveName = "Saves\\scene\\savefile.json", Atom specificAtom = null, bool includePhysical = true, bool includeAppearance = true, SuperController.ScreenShotCallback callback = null, bool isOverwrite = false)
	{
		FileManager.AssertNotCalledFromPlugin();
		this.SaveInternal(saveName, specificAtom, includePhysical, includeAppearance, callback, isOverwrite, false);
	}

	// Token: 0x06005C20 RID: 23584 RVA: 0x0021FED0 File Offset: 0x0021E2D0
	private void SaveInternalFinish(string saveName, Atom specificAtom, bool includePhysical, bool includeAppearance, SuperController.ScreenShotCallback callback, bool isOverwrite)
	{
		if (this.onBeforeSceneSaveHandlers != null)
		{
			this.onBeforeSceneSaveHandlers();
		}
		this.loadedName = saveName;
		int num = saveName.LastIndexOf('\\');
		if (num >= 0)
		{
			string path = saveName.Substring(0, num);
			FileManager.CreateDirectory(path);
		}
		this.lastLoadDir = FileManager.GetDirectoryName(saveName, true);
		if (!isOverwrite)
		{
			this.ClearFileBrowsersCurrentPath();
		}
		FileManager.SetSaveDirFromFilePath(saveName, true);
		FileManager.SetLoadDirFromFilePath(saveName, false);
		JSONClass saveJSON = this.GetSaveJSON(specificAtom, includePhysical, includeAppearance);
		this.SaveJSONInternal(saveJSON, saveName);
		if (this.onSceneSavedHandlers != null)
		{
			this.onSceneSavedHandlers();
		}
		this.DoSaveScreenshot(saveName, callback);
	}

	// Token: 0x06005C21 RID: 23585 RVA: 0x0021FF74 File Offset: 0x0021E374
	private void SaveInternal(string saveName = "Saves\\scene\\savefile.json", Atom specificAtom = null, bool includePhysical = true, bool includeAppearance = true, SuperController.ScreenShotCallback callback = null, bool isOverwrite = false, bool fromPlugin = false)
	{
		SuperController.<SaveInternal>c__AnonStoreyE <SaveInternal>c__AnonStoreyE = new SuperController.<SaveInternal>c__AnonStoreyE();
		<SaveInternal>c__AnonStoreyE.saveName = saveName;
		<SaveInternal>c__AnonStoreyE.specificAtom = specificAtom;
		<SaveInternal>c__AnonStoreyE.includePhysical = includePhysical;
		<SaveInternal>c__AnonStoreyE.includeAppearance = includeAppearance;
		<SaveInternal>c__AnonStoreyE.callback = callback;
		<SaveInternal>c__AnonStoreyE.isOverwrite = isOverwrite;
		<SaveInternal>c__AnonStoreyE.$this = this;
		try
		{
			if (!<SaveInternal>c__AnonStoreyE.saveName.EndsWith(".json"))
			{
				<SaveInternal>c__AnonStoreyE.saveName += ".json";
			}
			if (fromPlugin)
			{
				if (!FileManager.IsSecurePluginWritePath(<SaveInternal>c__AnonStoreyE.saveName))
				{
					throw new Exception("Attempted to save scene at non-secure path " + <SaveInternal>c__AnonStoreyE.saveName);
				}
			}
			else if (!FileManager.IsSecureWritePath(<SaveInternal>c__AnonStoreyE.saveName))
			{
				throw new Exception("Attempted to save scene at non-secure path " + <SaveInternal>c__AnonStoreyE.saveName);
			}
			UnityEngine.Debug.Log("Save " + <SaveInternal>c__AnonStoreyE.saveName);
			this.packageMode = false;
			if (fromPlugin)
			{
				if (File.Exists(<SaveInternal>c__AnonStoreyE.saveName))
				{
					if (!FileManager.IsPluginWritePathThatNeedsConfirm(<SaveInternal>c__AnonStoreyE.saveName))
					{
						this.SaveInternalFinish(<SaveInternal>c__AnonStoreyE.saveName, <SaveInternal>c__AnonStoreyE.specificAtom, <SaveInternal>c__AnonStoreyE.includePhysical, <SaveInternal>c__AnonStoreyE.includeAppearance, <SaveInternal>c__AnonStoreyE.callback, <SaveInternal>c__AnonStoreyE.isOverwrite);
					}
					else
					{
						FileManager.ConfirmPluginActionWithUser("save scene to file " + <SaveInternal>c__AnonStoreyE.saveName, new UserActionCallback(<SaveInternal>c__AnonStoreyE.<>m__0), null);
					}
				}
				else
				{
					this.SaveInternalFinish(<SaveInternal>c__AnonStoreyE.saveName, <SaveInternal>c__AnonStoreyE.specificAtom, <SaveInternal>c__AnonStoreyE.includePhysical, <SaveInternal>c__AnonStoreyE.includeAppearance, <SaveInternal>c__AnonStoreyE.callback, <SaveInternal>c__AnonStoreyE.isOverwrite);
				}
			}
			else
			{
				this.SaveInternalFinish(<SaveInternal>c__AnonStoreyE.saveName, <SaveInternal>c__AnonStoreyE.specificAtom, <SaveInternal>c__AnonStoreyE.includePhysical, <SaveInternal>c__AnonStoreyE.includeAppearance, <SaveInternal>c__AnonStoreyE.callback, <SaveInternal>c__AnonStoreyE.isOverwrite);
			}
		}
		catch (Exception arg)
		{
			SuperController.LogError("Exception during Save: " + arg);
		}
	}

	// Token: 0x06005C22 RID: 23586 RVA: 0x00220168 File Offset: 0x0021E568
	public void SaveJSON(JSONClass jc, string saveName)
	{
		this.SaveJSON(jc, saveName, null, null, null);
	}

	// Token: 0x06005C23 RID: 23587 RVA: 0x00220178 File Offset: 0x0021E578
	public void SaveJSON(JSONClass jc, string saveName, UserActionCallback confirmCallback, UserActionCallback denyCallback, ExceptionCallback exceptionCallback)
	{
		SuperController.<SaveJSON>c__AnonStoreyF <SaveJSON>c__AnonStoreyF = new SuperController.<SaveJSON>c__AnonStoreyF();
		<SaveJSON>c__AnonStoreyF.jc = jc;
		<SaveJSON>c__AnonStoreyF.saveName = saveName;
		<SaveJSON>c__AnonStoreyF.exceptionCallback = exceptionCallback;
		<SaveJSON>c__AnonStoreyF.confirmCallback = confirmCallback;
		<SaveJSON>c__AnonStoreyF.$this = this;
		if (FileManager.IsSecurePluginWritePath(<SaveJSON>c__AnonStoreyF.saveName))
		{
			if (File.Exists(<SaveJSON>c__AnonStoreyF.saveName))
			{
				if (!FileManager.IsPluginWritePathThatNeedsConfirm(<SaveJSON>c__AnonStoreyF.saveName))
				{
					try
					{
						this.SaveJSONInternal(<SaveJSON>c__AnonStoreyF.jc, <SaveJSON>c__AnonStoreyF.saveName);
					}
					catch (Exception e)
					{
						if (<SaveJSON>c__AnonStoreyF.exceptionCallback != null)
						{
							<SaveJSON>c__AnonStoreyF.exceptionCallback(e);
						}
						return;
					}
					if (<SaveJSON>c__AnonStoreyF.confirmCallback != null)
					{
						<SaveJSON>c__AnonStoreyF.confirmCallback();
					}
				}
				else
				{
					FileManager.ConfirmPluginActionWithUser("save json to file " + <SaveJSON>c__AnonStoreyF.saveName, new UserActionCallback(<SaveJSON>c__AnonStoreyF.<>m__0), denyCallback);
				}
			}
			else
			{
				try
				{
					this.SaveJSONInternal(<SaveJSON>c__AnonStoreyF.jc, <SaveJSON>c__AnonStoreyF.saveName);
				}
				catch (Exception e2)
				{
					if (<SaveJSON>c__AnonStoreyF.exceptionCallback != null)
					{
						<SaveJSON>c__AnonStoreyF.exceptionCallback(e2);
					}
					return;
				}
				if (<SaveJSON>c__AnonStoreyF.confirmCallback != null)
				{
					<SaveJSON>c__AnonStoreyF.confirmCallback();
				}
			}
			return;
		}
		Exception ex = new Exception("Attempted to save json file at non-secure path " + <SaveJSON>c__AnonStoreyF.saveName);
		if (<SaveJSON>c__AnonStoreyF.exceptionCallback != null)
		{
			<SaveJSON>c__AnonStoreyF.exceptionCallback(ex);
			return;
		}
		throw ex;
	}

	// Token: 0x06005C24 RID: 23588 RVA: 0x002202EC File Offset: 0x0021E6EC
	private void SaveJSONInternal(JSONClass jc, string saveName)
	{
		try
		{
			StringBuilder stringBuilder = new StringBuilder(100000);
			jc.ToString(string.Empty, stringBuilder);
			string value = stringBuilder.ToString();
			using (StreamWriter streamWriter = FileManager.OpenStreamWriter(saveName))
			{
				streamWriter.Write(value);
			}
		}
		catch (Exception arg)
		{
			SuperController.LogError("Exception during SaveJSON: " + arg);
		}
	}

	// Token: 0x06005C25 RID: 23589 RVA: 0x00220370 File Offset: 0x0021E770
	public JSONNode LoadJSON(string saveName)
	{
		JSONNode result = null;
		try
		{
			FileEntry fileEntry = FileManager.GetFileEntry(saveName, true);
			if (fileEntry != null)
			{
				using (FileEntryStreamReader fileEntryStreamReader = FileManager.OpenStreamReader(fileEntry))
				{
					string aJSON = fileEntryStreamReader.ReadToEnd();
					result = JSON.Parse(aJSON);
				}
			}
			else
			{
				SuperController.LogError("LoadJSON: File " + saveName + " not found");
			}
		}
		catch (Exception arg)
		{
			SuperController.LogError("Exception during LoadJSON: " + arg);
		}
		return result;
	}

	// Token: 0x06005C26 RID: 23590 RVA: 0x00220408 File Offset: 0x0021E808
	public void AddVarPackageRefToVacPackage(string packageUid)
	{
		this.referencedVarPackages.Add(packageUid);
	}

	// Token: 0x06005C27 RID: 23591 RVA: 0x00220418 File Offset: 0x0021E818
	public string AddFileToPackage(string path, string packagepath)
	{
		if (this.zos != null)
		{
			VarFileEntry varFileEntry = FileManager.GetVarFileEntry(path);
			if (varFileEntry == null)
			{
				if (!this.alreadyPackaged.ContainsKey(packagepath))
				{
					this.alreadyPackaged.Add(packagepath, true);
					byte[] buffer = new byte[4096];
					ZipEntry entry = new ZipEntry(packagepath);
					this.zos.PutNextEntry(entry);
					using (FileEntryStream fileEntryStream = FileManager.OpenStream(path, false))
					{
						StreamUtils.Copy(fileEntryStream.Stream, this.zos, buffer);
					}
					this.zos.CloseEntry();
				}
				return packagepath;
			}
			this.referencedVarPackages.Add(varFileEntry.Package.Uid);
		}
		return path;
	}

	// Token: 0x06005C28 RID: 23592 RVA: 0x002204DC File Offset: 0x0021E8DC
	private void SavePackage(string saveName = "Saves\\scene\\savefile.vac", bool isOverwrite = false)
	{
		try
		{
			if (saveName != string.Empty)
			{
				this.alreadyPackaged = new Dictionary<string, bool>();
				this.referencedVarPackages = new HashSet<string>();
				string text = Path.GetFileName(saveName);
				text = text.Replace(".vac", string.Empty);
				if (!saveName.EndsWith(".vac"))
				{
					saveName += ".vac";
				}
				this.packageMode = true;
				UnityEngine.Debug.Log("Save Package " + saveName);
				byte[] buffer = new byte[4096];
				using (this.zos = new ZipOutputStream(File.Create(saveName)))
				{
					this.zos.SetLevel(5);
					JSONClass saveJSON = this.GetSaveJSON(null, true, true);
					ZipEntry entry = new ZipEntry(text + ".json");
					this.zos.PutNextEntry(entry);
					StringBuilder stringBuilder = new StringBuilder(100000);
					saveJSON.ToString(string.Empty, stringBuilder);
					string s = stringBuilder.ToString();
					using (MemoryStream memoryStream = new MemoryStream(Encoding.Default.GetBytes(s)))
					{
						StreamUtils.Copy(memoryStream, this.zos, buffer);
					}
					this.zos.CloseEntry();
					ZipEntry entry2 = new ZipEntry("meta.json");
					this.zos.PutNextEntry(entry2);
					JSONClass jsonclass = new JSONClass();
					JSONClass jsonclass2 = new JSONClass();
					HashSet<string> visited = new HashSet<string>();
					HashSet<VarPackage> allReferencedPackages = new HashSet<VarPackage>();
					HashSet<string> allReferencedPackageUids = new HashSet<string>();
					foreach (string text2 in this.referencedVarPackages)
					{
						VarPackage package = FileManager.GetPackage(text2);
						PackageBuilder.GetPackageDependenciesRecursive(package, text2, visited, allReferencedPackages, allReferencedPackageUids, jsonclass2);
						SuperController.LogMessage("INFO: VAC references VAR package " + text2);
					}
					jsonclass["programVersion"] = this.GetVersion();
					jsonclass["dependencies"] = jsonclass2;
					s = jsonclass.ToString(string.Empty);
					using (MemoryStream memoryStream2 = new MemoryStream(Encoding.Default.GetBytes(s)))
					{
						StreamUtils.Copy(memoryStream2, this.zos, buffer);
					}
					this.zos.CloseEntry();
				}
				if (!isOverwrite)
				{
					this.ClearFileBrowsersCurrentPath();
				}
				this.packageMode = false;
				this.DoSaveScreenshot(saveName, null);
			}
		}
		catch (Exception arg)
		{
			SuperController.LogError("Exception during SavePackage: " + arg);
		}
	}

	// Token: 0x06005C29 RID: 23593 RVA: 0x002207F0 File Offset: 0x0021EBF0
	protected IEnumerator LoadCo(bool clearOnly = false, bool loadMerge = false)
	{
		this.hideWaitTransform = loadMerge;
		if (this.loadFlag != null)
		{
			this.loadFlag.Raise();
		}
		this.loadFlag = new AsyncFlag("Scene Load");
		this.ResetSimulation(this.loadFlag, false);
		if (UserPreferences.singleton != null)
		{
			UserPreferences.singleton.pauseGlow = true;
		}
		this.DeactivateWorldUI();
		if (this.loadingUI != null)
		{
			if (!loadMerge)
			{
				if (this.fileBrowserUI == null || this.fileBrowserUI.IsHidden() || !this.fileBrowserUI.keepOpen)
				{
					this.HideMainHUD();
				}
				HUDAnchor.SetAnchorsToReference();
				this.loadingUI.gameObject.SetActive(true);
				if (this.loadingUIAlt != null && !this._mainHUDAnchoredOnMonitor)
				{
					this.loadingUIAlt.gameObject.SetActive(true);
				}
			}
			if (this.loadingGeometry != null)
			{
				this.loadingGeometry.gameObject.SetActive(true);
			}
		}
		yield return null;
		this.ResetMonitorCenterCamera();
		if (!loadMerge)
		{
			JSONClass jc = new JSONClass();
			this.ClearSelection(true);
			this.ClearAllGrabbedControllers();
			this.ClearPossess();
			this.DisconnectNavRigFromPlayerNavCollider();
			if (this.worldScaleSlider != null)
			{
				SliderControl component = this.worldScaleSlider.GetComponent<SliderControl>();
				if (component != null)
				{
					this.worldScale = component.defaultValue;
				}
			}
			if (this.playerHeightAdjustSlider != null)
			{
				SliderControl component2 = this.playerHeightAdjustSlider.GetComponent<SliderControl>();
				if (component2 != null)
				{
					this.playerHeightAdjust = component2.defaultValue;
				}
			}
			Atom[] atms = new Atom[this.atoms.Count];
			this.atomsList.CopyTo(atms, 0);
			foreach (Atom atom in atms)
			{
				if (atom != null)
				{
					atom.PreRestore();
				}
			}
			this.RemoveNonStartingAtoms();
			yield return null;
			foreach (Atom atom2 in this.atomsList)
			{
				atom2.ClearParentAtom();
			}
			foreach (Atom atom3 in this.atomsList)
			{
				atom3.RestoreTransform(jc, true);
			}
			foreach (Atom atom4 in this.atomsList)
			{
				atom4.RestoreParentAtom(jc);
			}
			FileManager.PushLoadDir(string.Empty, false);
			foreach (Atom atom5 in this.atomsList)
			{
				atom5.Restore(jc, true, true, true, null, true, false, true, false);
			}
			FileManager.PopLoadDir();
			foreach (Atom atom6 in this.atomsList)
			{
				atom6.LateRestore(jc, true, true, true, false, true, false);
			}
			foreach (Atom atom7 in this.atomsList)
			{
				atom7.PostRestore();
			}
			if (UserPreferences.singleton != null && UserPreferences.singleton.optimizeMemoryOnSceneLoad && MemoryOptimizer.singleton != null)
			{
				yield return base.StartCoroutine(MemoryOptimizer.singleton.OptimizeMemoryUsage());
			}
			else
			{
				yield return Resources.UnloadUnusedAssets();
				GC.Collect();
			}
		}
		if (!clearOnly)
		{
			JSONArray jatoms = this.loadJson["atoms"].AsArray;
			if (this.loadJson["worldScale"] != null)
			{
				this.worldScale = this.loadJson["worldScale"].AsFloat;
			}
			else if (this.worldScaleSlider != null)
			{
				SliderControl component3 = this.worldScaleSlider.GetComponent<SliderControl>();
				if (component3 != null)
				{
					this.worldScale = component3.defaultValue;
				}
			}
			if (this.loadJson["environmentHeight"] != null)
			{
				this.playerHeightAdjust = this.loadJson["environmentHeight"].AsFloat;
			}
			else if (this.loadJson["playerHeightAdjust"] != null)
			{
				this.playerHeightAdjust = this.loadJson["playerHeightAdjust"].AsFloat;
			}
			else if (this.playerHeightAdjustSlider != null)
			{
				SliderControl component4 = this.playerHeightAdjustSlider.GetComponent<SliderControl>();
				if (component4 != null)
				{
					this.playerHeightAdjust = component4.defaultValue;
				}
			}
			if (this.loadJson["monitorCameraRotation"] != null)
			{
				Vector3 localEulerAngles;
				localEulerAngles.x = 0f;
				localEulerAngles.y = 0f;
				localEulerAngles.z = 0f;
				if (this.loadJson["monitorCameraRotation"]["x"] != null)
				{
					localEulerAngles.x = this.loadJson["monitorCameraRotation"]["x"].AsFloat;
				}
				if (this.loadJson["monitorCameraRotation"]["y"] != null)
				{
					localEulerAngles.y = this.loadJson["monitorCameraRotation"]["y"].AsFloat;
				}
				if (this.loadJson["monitorCameraRotation"]["z"] != null)
				{
					localEulerAngles.z = this.loadJson["monitorCameraRotation"]["z"].AsFloat;
				}
				if (this.MonitorCenterCamera != null)
				{
					this.MonitorCenterCamera.transform.localEulerAngles = localEulerAngles;
				}
			}
			if (this.loadJson["useSceneLoadPosition"] != null)
			{
				this.useSceneLoadPosition = this.loadJson["useSceneLoadPosition"].AsBool;
			}
			if (this.loadingProgressSlider != null)
			{
				this.loadingProgressSlider.minValue = 0f;
				this.loadingProgressSlider.maxValue = (float)jatoms.Count * 2f + 2f;
				this.loadingProgressSlider.value = 0f;
			}
			if (this.loadingProgressSliderAlt != null)
			{
				this.loadingProgressSliderAlt.minValue = 0f;
				this.loadingProgressSliderAlt.maxValue = (float)jatoms.Count * 2f + 2f;
				this.loadingProgressSliderAlt.value = 0f;
			}
			this.UpdateLoadingStatus("Pre-Restore");
			IEnumerator enumerator7 = jatoms.GetEnumerator();
			try
			{
				while (enumerator7.MoveNext())
				{
					object obj = enumerator7.Current;
					JSONClass jsonclass = (JSONClass)obj;
					string uid = jsonclass["id"];
					Atom atomByUid = this.GetAtomByUid(uid);
					if (atomByUid != null)
					{
						atomByUid.PreRestore();
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator7 as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			this.IncrementLoadingSlider();
			Physics.autoSimulation = false;
			IEnumerator enumerator8 = jatoms.GetEnumerator();
			try
			{
				while (enumerator8.MoveNext())
				{
					object obj2 = enumerator8.Current;
					JSONClass jatom = (JSONClass)obj2;
					string auid = jatom["id"];
					string type = jatom["type"];
					this.UpdateLoadingStatus("Loading Atom " + auid);
					Atom a = this.GetAtomByUid(auid);
					if (a == null)
					{
						yield return base.StartCoroutine(this.AddAtomByType(type, auid, false, false, false));
						a = this.GetAtomByUid(auid);
						if (a != null)
						{
							a.ResetSimulation(this.loadFlag);
						}
					}
					else if (a.type != type)
					{
						this.Error(string.Concat(new string[]
						{
							"Atom ",
							a.name,
							" already exists, but uses different type ",
							a.type,
							" compared to requested ",
							type
						}), true, true);
					}
					if (a != null)
					{
						a.SetOn(true);
					}
					this.IncrementLoadingSlider();
				}
			}
			finally
			{
				IDisposable disposable2;
				if ((disposable2 = (enumerator8 as IDisposable)) != null)
				{
					disposable2.Dispose();
				}
			}
			this.UpdateLoadingStatus("Restoring atom contents. Note large save files could take a while...");
			yield return null;
			Physics.Simulate(0.01f);
			yield return null;
			IEnumerator enumerator9 = jatoms.GetEnumerator();
			try
			{
				while (enumerator9.MoveNext())
				{
					object obj3 = enumerator9.Current;
					JSONClass jsonclass2 = (JSONClass)obj3;
					string text = jsonclass2["id"];
					string str = jsonclass2["type"];
					Atom atomByUid2 = this.GetAtomByUid(text);
					if (atomByUid2 != null)
					{
						atomByUid2.RestoreTransform(jsonclass2, true);
					}
					else
					{
						this.Error("Failed to find atom " + text + " of type " + str, true, true);
					}
				}
			}
			finally
			{
				IDisposable disposable3;
				if ((disposable3 = (enumerator9 as IDisposable)) != null)
				{
					disposable3.Dispose();
				}
			}
			IEnumerator enumerator10 = jatoms.GetEnumerator();
			try
			{
				while (enumerator10.MoveNext())
				{
					object obj4 = enumerator10.Current;
					JSONClass jsonclass3 = (JSONClass)obj4;
					string uid2 = jsonclass3["id"];
					Atom atomByUid3 = this.GetAtomByUid(uid2);
					if (atomByUid3 != null)
					{
						atomByUid3.RestoreParentAtom(jsonclass3);
					}
				}
			}
			finally
			{
				IDisposable disposable4;
				if ((disposable4 = (enumerator10 as IDisposable)) != null)
				{
					disposable4.Dispose();
				}
			}
			IEnumerator enumerator11 = jatoms.GetEnumerator();
			try
			{
				while (enumerator11.MoveNext())
				{
					object obj5 = enumerator11.Current;
					JSONClass jsonclass4 = (JSONClass)obj5;
					string text2 = jsonclass4["id"];
					Atom atomByUid4 = this.GetAtomByUid(text2);
					if (atomByUid4 != null)
					{
						this.UpdateLoadingStatus("Restoring atom " + text2);
						atomByUid4.Restore(jsonclass4, true, true, true, null, false, false, true, false);
					}
					else
					{
						this.Error("Could not find atom by uid " + text2, true, true);
					}
					this.IncrementLoadingSlider();
				}
			}
			finally
			{
				IDisposable disposable5;
				if ((disposable5 = (enumerator11 as IDisposable)) != null)
				{
					disposable5.Dispose();
				}
			}
			this.UpdateLoadingStatus("Post-Restore");
			IEnumerator enumerator12 = jatoms.GetEnumerator();
			try
			{
				while (enumerator12.MoveNext())
				{
					object obj6 = enumerator12.Current;
					JSONClass jsonclass5 = (JSONClass)obj6;
					string text3 = jsonclass5["id"];
					Atom atomByUid5 = this.GetAtomByUid(text3);
					if (atomByUid5 != null)
					{
						atomByUid5.LateRestore(jsonclass5, true, true, true, false, true, false);
					}
					else
					{
						this.Error("Could not find atom by uid " + text3, true, true);
					}
				}
			}
			finally
			{
				IDisposable disposable6;
				if ((disposable6 = (enumerator12 as IDisposable)) != null)
				{
					disposable6.Dispose();
				}
			}
			foreach (Atom atom8 in this.atomsList)
			{
				atom8.PostRestore();
			}
			while (this.CheckHoldLoad())
			{
				this.UpdateLoadingStatus("Waiting for async load from " + this.holdLoadCompleteFlags[0].Name);
				yield return null;
			}
			Physics.autoSimulation = true;
			this.SetSceneLoadPosition();
			this.IncrementLoadingSlider();
			yield return null;
			if (this.loadJson["headPossessedController"] != null)
			{
				FreeControllerV3 freeControllerV = this.FreeControllerNameToFreeController(this.loadJson["headPossessedController"]);
				if (freeControllerV != null)
				{
					this.HeadPossess(freeControllerV, true, true, true);
				}
			}
			if (this.loadJson["playerNavCollider"] != null)
			{
				string text4 = this.loadJson["playerNavCollider"];
				PlayerNavCollider playerNavCollider;
				if (this.pncMap.TryGetValue(text4, out playerNavCollider))
				{
					this.playerNavCollider = playerNavCollider;
					this.ConnectNavRigToPlayerNavCollider();
				}
				else
				{
					this.Error("Could not find playerNavCollider " + text4, true, true);
				}
			}
		}
		if (loadMerge)
		{
			foreach (Atom atom9 in this.atomsList)
			{
				atom9.Validate();
			}
		}
		for (int i = 0; i < 20; i++)
		{
			yield return null;
		}
		this.loadFlag.Raise();
		for (int j = 0; j < 5; j++)
		{
			yield return null;
		}
		this._isLoading = false;
		this.SyncSortedAtomUIDs();
		this.SyncSortedAtomUIDsWithForceProducers();
		this.SyncSortedAtomUIDsWithForceReceivers();
		this.SyncSortedAtomUIDsWithFreeControllers();
		this.SyncSortedAtomUIDsWithRhythmControllers();
		this.SyncSortedAtomUIDsWithAudioSourceControls();
		this.SyncSortedAtomUIDsWithRigidbodies();
		this.SyncHiddenAtoms();
		this.SyncSelectAtomPopup();
		if (this.loadingUI != null)
		{
			for (int k = 0; k < 10; k++)
			{
				yield return null;
			}
			this.loadingUI.gameObject.SetActive(false);
			if (this.loadingUIAlt != null)
			{
				this.loadingUIAlt.gameObject.SetActive(false);
			}
			if (this.loadingGeometry != null)
			{
				this.loadingGeometry.gameObject.SetActive(false);
			}
		}
		if (UserPreferences.singleton != null)
		{
			UserPreferences.singleton.pauseGlow = false;
		}
		if (this.UIDisabled && !loadMerge)
		{
			this.HideMainHUD();
		}
		if (!loadMerge && this.mainHUDAttachPoint != null)
		{
			this.mainHUDAttachPoint.localPosition = this.mainHUDAttachPointStartingPosition;
			this.mainHUDAttachPoint.localRotation = this.mainHUDAttachPointStartingRotation;
		}
		this.SyncVisibility();
		this.hideWaitTransform = false;
		if (this.onSceneLoadedHandlers != null)
		{
			this.onSceneLoadedHandlers();
		}
		yield break;
	}

	// Token: 0x06005C2A RID: 23594 RVA: 0x0022081C File Offset: 0x0021EC1C
	private string ExtractZipFile(string archiveFilenameIn)
	{
		ZipFile zipFile = null;
		string result = null;
		bool flag = false;
		try
		{
			using (FileEntryStream fileEntryStream = FileManager.OpenStream(archiveFilenameIn, true))
			{
				zipFile = new ZipFile(fileEntryStream.Stream);
				string text = FileManager.GetDirectoryName(archiveFilenameIn, false);
				string text2 = Path.GetFileName(archiveFilenameIn);
				text2 = text2.Replace(".zip", string.Empty);
				text2 = text2.Replace(".vac", string.Empty);
				text = text + "/" + text2;
				IEnumerator enumerator = zipFile.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						ZipEntry zipEntry = (ZipEntry)obj;
						if (zipEntry.IsFile)
						{
							string name = zipEntry.Name;
							byte[] buffer = new byte[4096];
							Stream inputStream = zipFile.GetInputStream(zipEntry);
							string text3 = Path.Combine(text, name);
							string fileName = Path.GetFileName(name);
							if (name.EndsWith(".var"))
							{
								text3 = "AddonPackages/" + fileName;
								string packageUidOrPath = fileName.Replace(".var", string.Empty);
								if (File.Exists(text3) || FileManager.GetPackage(packageUidOrPath) != null)
								{
									continue;
								}
								flag = true;
							}
							else
							{
								string directoryName = Path.GetDirectoryName(text3);
								if (directoryName.Length > 0)
								{
									FileManager.CreateDirectory(directoryName);
								}
								if (fileName != "meta.json" && (text3.EndsWith(".vac") || text3.EndsWith(".json")))
								{
									result = text3;
								}
							}
							using (FileStream fileStream = File.Create(text3))
							{
								StreamUtils.Copy(inputStream, fileStream, buffer);
							}
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
			}
			if (flag)
			{
				FileManager.Refresh();
			}
		}
		catch (Exception ex)
		{
			this.Error(string.Concat(new object[]
			{
				"Exception during zip file extract of ",
				archiveFilenameIn,
				": ",
				ex
			}), true, true);
		}
		finally
		{
			if (zipFile != null)
			{
				zipFile.IsStreamOwner = true;
				zipFile.Close();
			}
		}
		return result;
	}

	// Token: 0x06005C2B RID: 23595 RVA: 0x00220AC8 File Offset: 0x0021EEC8
	protected void LoadInternal(string saveName = "savefile", bool loadMerge = false, bool editMode = false)
	{
		if (Application.isPlaying)
		{
			if (!this._isLoading)
			{
				try
				{
					if (saveName != null && saveName != string.Empty && FileManager.FileExists(saveName, false, false))
					{
						if (this.onStartScene)
						{
							this.lastLoadDir = FileManager.GetDirectoryName(this.savesDirResolved + this.startSceneName, true);
						}
						else
						{
							this.lastLoadDir = FileManager.GetDirectoryName(saveName, true);
						}
						this.onStartScene = false;
						if (editMode)
						{
							this.gameMode = SuperController.GameMode.Edit;
						}
						else
						{
							this.gameMode = SuperController.GameMode.Play;
						}
						UnityEngine.Debug.Log("Load " + saveName);
						if (!loadMerge)
						{
							this.ClearSelection(true);
							this.ClearPossess();
							this.ClearAllGrabbedControllers();
							this.DisconnectNavRigFromPlayerNavCollider();
							this._isLoading = true;
							Atom[] array = new Atom[this.atoms.Count];
							this.atomsList.CopyTo(array, 0);
							foreach (Atom atom in array)
							{
								if (atom != null)
								{
									atom.PreRestore();
								}
							}
							this.RemoveNonStartingAtoms();
							this._isLoading = false;
						}
						if (saveName.EndsWith(".zip"))
						{
							string text = this.ExtractZipFile(saveName);
							if (text != null)
							{
								saveName = text;
							}
						}
						if (saveName.EndsWith(".vac"))
						{
							string text2 = this.ExtractZipFile(saveName);
							this.ClearFileBrowsersCurrentPath();
							if (text2 != null && text2.EndsWith(".json"))
							{
								saveName = text2;
							}
						}
						if (saveName.EndsWith(".json") && FileManager.FileExists(saveName, false, false))
						{
							using (FileEntryStreamReader fileEntryStreamReader = FileManager.OpenStreamReader(saveName, true))
							{
								string aJSON = fileEntryStreamReader.ReadToEnd();
								this.loadJson = JSON.Parse(aJSON);
							}
							this.loadedName = saveName;
							FileManager.SetLoadDirFromFilePath(saveName, false);
							this._isLoading = true;
							base.StartCoroutine(this.LoadCo(false, loadMerge));
						}
						else
						{
							this.Error("json file " + saveName + " is missing", true, true);
						}
					}
					else
					{
						this.onStartScene = false;
					}
				}
				catch (Exception arg)
				{
					this._isLoading = false;
					this.Error("Exception during load " + arg, true, true);
				}
			}
			else
			{
				UnityEngine.Debug.LogWarning("Already loading file " + this.loadedName + ". Can't load another until complete");
			}
		}
	}

	// Token: 0x06005C2C RID: 23596 RVA: 0x00220D68 File Offset: 0x0021F168
	protected void LoadFromSceneWorldDialog(string saveName)
	{
		if (saveName != null && saveName != string.Empty)
		{
			this.LoadInternal(saveName, false, false);
		}
		else if (!this.loadSceneWorldDialogActivatedFromWorld)
		{
			this.DeactivateWorldUI();
		}
		this.loadSceneWorldDialogActivatedFromWorld = false;
	}

	// Token: 0x06005C2D RID: 23597 RVA: 0x00220DA6 File Offset: 0x0021F1A6
	protected void LoadFromTemplateWorldDialog(string saveName)
	{
		if (saveName != null && saveName != string.Empty)
		{
			this.LoadInternal(saveName, false, true);
		}
		else if (!this.loadTemplateWorldDialogActivatedFromWorld)
		{
			this.DeactivateWorldUI();
		}
		this.loadTemplateWorldDialogActivatedFromWorld = false;
	}

	// Token: 0x06005C2E RID: 23598 RVA: 0x00220DE4 File Offset: 0x0021F1E4
	protected void LoadFromSceneDialog(string saveName)
	{
		SuperController.<LoadFromSceneDialog>c__AnonStorey10 <LoadFromSceneDialog>c__AnonStorey = new SuperController.<LoadFromSceneDialog>c__AnonStorey10();
		<LoadFromSceneDialog>c__AnonStorey.saveName = saveName;
		<LoadFromSceneDialog>c__AnonStorey.$this = this;
		if (<LoadFromSceneDialog>c__AnonStorey.saveName != null && <LoadFromSceneDialog>c__AnonStorey.saveName != string.Empty)
		{
			if (UserPreferences.singleton != null && UserPreferences.singleton.confirmLoad && this.loadConfirmButton != null && this.loadConfirmPanel != null)
			{
				this.loadConfirmButton.onClick.RemoveAllListeners();
				this.loadConfirmButton.onClick.AddListener(new UnityAction(<LoadFromSceneDialog>c__AnonStorey.<>m__0));
				this.loadConfirmPanel.gameObject.SetActive(true);
				if (this.loadConfirmPathText != null)
				{
					this.loadConfirmPathText.text = <LoadFromSceneDialog>c__AnonStorey.saveName;
				}
			}
			else
			{
				this.Load(<LoadFromSceneDialog>c__AnonStorey.saveName);
			}
		}
	}

	// Token: 0x06005C2F RID: 23599 RVA: 0x00220ED8 File Offset: 0x0021F2D8
	protected void LoadForEditFromSceneDialog(string saveName)
	{
		SuperController.<LoadForEditFromSceneDialog>c__AnonStorey11 <LoadForEditFromSceneDialog>c__AnonStorey = new SuperController.<LoadForEditFromSceneDialog>c__AnonStorey11();
		<LoadForEditFromSceneDialog>c__AnonStorey.saveName = saveName;
		<LoadForEditFromSceneDialog>c__AnonStorey.$this = this;
		if (<LoadForEditFromSceneDialog>c__AnonStorey.saveName != null && <LoadForEditFromSceneDialog>c__AnonStorey.saveName != string.Empty)
		{
			if (UserPreferences.singleton != null && UserPreferences.singleton.confirmLoad && this.loadConfirmButton != null && this.loadConfirmPanel != null)
			{
				this.loadConfirmButton.onClick.RemoveAllListeners();
				this.loadConfirmButton.onClick.AddListener(new UnityAction(<LoadForEditFromSceneDialog>c__AnonStorey.<>m__0));
				this.loadConfirmPanel.gameObject.SetActive(true);
				if (this.loadConfirmPathText != null)
				{
					this.loadConfirmPathText.text = <LoadForEditFromSceneDialog>c__AnonStorey.saveName;
				}
			}
			else
			{
				this.LoadForEdit(<LoadForEditFromSceneDialog>c__AnonStorey.saveName);
			}
		}
	}

	// Token: 0x06005C30 RID: 23600 RVA: 0x00220FCC File Offset: 0x0021F3CC
	protected void LoadMergeFromSceneDialog(string saveName)
	{
		SuperController.<LoadMergeFromSceneDialog>c__AnonStorey12 <LoadMergeFromSceneDialog>c__AnonStorey = new SuperController.<LoadMergeFromSceneDialog>c__AnonStorey12();
		<LoadMergeFromSceneDialog>c__AnonStorey.saveName = saveName;
		<LoadMergeFromSceneDialog>c__AnonStorey.$this = this;
		if (<LoadMergeFromSceneDialog>c__AnonStorey.saveName != null && <LoadMergeFromSceneDialog>c__AnonStorey.saveName != string.Empty)
		{
			if (UserPreferences.singleton != null && UserPreferences.singleton.confirmLoad && this.loadConfirmButton != null && this.loadConfirmPanel != null)
			{
				this.loadConfirmButton.onClick.RemoveAllListeners();
				this.loadConfirmButton.onClick.AddListener(new UnityAction(<LoadMergeFromSceneDialog>c__AnonStorey.<>m__0));
				this.loadConfirmPanel.gameObject.SetActive(true);
				if (this.loadConfirmPathText != null)
				{
					this.loadConfirmPathText.text = <LoadMergeFromSceneDialog>c__AnonStorey.saveName;
				}
			}
			else
			{
				this.LoadMerge(<LoadMergeFromSceneDialog>c__AnonStorey.saveName);
			}
		}
	}

	// Token: 0x06005C31 RID: 23601 RVA: 0x002210BE File Offset: 0x0021F4BE
	public void Load(string saveName = "savefile")
	{
		this.LoadInternal(saveName, false, false);
	}

	// Token: 0x06005C32 RID: 23602 RVA: 0x002210C9 File Offset: 0x0021F4C9
	public void LoadRestoreUI(string saveName = "savefile")
	{
		this.LoadInternal(saveName, false, false);
		this.ShowMainHUDAuto();
	}

	// Token: 0x06005C33 RID: 23603 RVA: 0x002210DA File Offset: 0x0021F4DA
	public void LoadForEdit(string saveName = "savefile")
	{
		this.LoadInternal(saveName, false, true);
		this.ShowMainHUDAuto();
	}

	// Token: 0x06005C34 RID: 23604 RVA: 0x002210EB File Offset: 0x0021F4EB
	public void LoadMerge(string saveName = "savefile")
	{
		this.LoadInternal(saveName, true, false);
	}

	// Token: 0x06005C35 RID: 23605 RVA: 0x002210F8 File Offset: 0x0021F4F8
	public void LoadFromJSONEmbed(JSONEmbed je, bool loadMerge = false, bool editMode = false)
	{
		if (Application.isPlaying)
		{
			this.onStartScene = false;
			if (editMode)
			{
				this.gameMode = SuperController.GameMode.Edit;
			}
			else
			{
				this.gameMode = SuperController.GameMode.Play;
			}
			this.loadJson = JSON.Parse(je.jsonStore);
			this.loadedName = je.name;
			FileManager.SetLoadDir(string.Empty, false);
			this._isLoading = true;
			base.StartCoroutine(this.LoadCo(false, loadMerge));
		}
	}

	// Token: 0x06005C36 RID: 23606 RVA: 0x00221170 File Offset: 0x0021F570
	protected void IncrementLoadingSlider()
	{
		if (this.loadingProgressSlider != null)
		{
			this.loadingProgressSlider.value += 1f;
		}
		if (this.loadingProgressSliderAlt != null)
		{
			this.loadingProgressSliderAlt.value += 1f;
		}
	}

	// Token: 0x06005C37 RID: 23607 RVA: 0x002211CD File Offset: 0x0021F5CD
	protected void UpdateLoadingStatus(string txt)
	{
		if (this.loadingTextStatus != null)
		{
			this.loadingTextStatus.text = txt;
		}
		if (this.loadingTextStatusAlt != null)
		{
			this.loadingTextStatusAlt.text = txt;
		}
	}

	// Token: 0x06005C38 RID: 23608 RVA: 0x0022120C File Offset: 0x0021F60C
	protected void RemoveNonStartingAtoms()
	{
		if (this.startingAtoms != null)
		{
			HashSet<Atom> hashSet = new HashSet<Atom>();
			foreach (Atom atom in this.startingAtoms)
			{
				if (atom != null)
				{
					hashSet.Add(atom);
				}
			}
			this.startingAtoms = hashSet;
			List<Atom> list = new List<Atom>();
			foreach (Atom item in this.atomsList)
			{
				if (!this.startingAtoms.Contains(item))
				{
					list.Add(item);
				}
			}
			foreach (Atom atom2 in list)
			{
				this.RemoveAtom(atom2, false);
			}
			this.atomsList = this.startingAtoms.ToList<Atom>();
		}
	}

	// Token: 0x06005C39 RID: 23609 RVA: 0x00221350 File Offset: 0x0021F750
	protected void LoadDialog(string lastPath, bool resetFileFormat = true)
	{
		string text = this.savesDirResolved + "scene";
		List<ShortCut> shortCutsForDirectory = FileManager.GetShortCutsForDirectory(text, true, false, true, true);
		if (FileManager.DirectoryExists("Saves/Downloads", false, false))
		{
			shortCutsForDirectory.Insert(1, new ShortCut
			{
				displayName = "Saves\\Downloads",
				path = "Saves\\Downloads"
			});
		}
		this.fileBrowserUI.SetShortCuts(shortCutsForDirectory, true);
		string suggestedBrowserDirectoryFromDirectoryPath = FileManager.GetSuggestedBrowserDirectoryFromDirectoryPath(text, lastPath, true);
		if (suggestedBrowserDirectoryFromDirectoryPath != null && FileManager.DirectoryExists(suggestedBrowserDirectoryFromDirectoryPath, false, false))
		{
			text = suggestedBrowserDirectoryFromDirectoryPath;
		}
		if (resetFileFormat)
		{
			this.fileBrowserUI.fileFormat = "json|vac|zip";
		}
		this.fileBrowserUI.defaultPath = text;
		this.activeUI = SuperController.ActiveUI.None;
		this.fileBrowserUI.SetTextEntry(false);
		this.fileBrowserUI.keepOpen = false;
	}

	// Token: 0x06005C3A RID: 23610 RVA: 0x0022141A File Offset: 0x0021F81A
	public void LoadMergeSceneDialog()
	{
		this.LoadDialog(this.lastLoadDir, true);
		this.fileBrowserUI.SetTitle("Select Scene For Merge");
		this.fileBrowserUI.Show(new FileBrowserCallback(this.LoadMergeFromSceneDialog), true);
	}

	// Token: 0x06005C3B RID: 23611 RVA: 0x00221451 File Offset: 0x0021F851
	public void LoadSceneForEditDialog()
	{
		this.LoadDialog(this.lastLoadDir, true);
		this.fileBrowserUI.SetTitle("Select Scene For Edit");
		this.fileBrowserUI.Show(new FileBrowserCallback(this.LoadForEditFromSceneDialog), true);
	}

	// Token: 0x06005C3C RID: 23612 RVA: 0x00221488 File Offset: 0x0021F888
	public void LoadSceneDialog()
	{
		this.LoadDialog(this.lastLoadDir, true);
		this.fileBrowserUI.SetTitle("Select Scene To Load");
		this.fileBrowserUI.Show(new FileBrowserCallback(this.LoadFromSceneDialog), true);
	}

	// Token: 0x06005C3D RID: 23613 RVA: 0x002214C0 File Offset: 0x0021F8C0
	public void LoadSceneWorldDialog(bool fromWorld = true)
	{
		if (this.fileBrowserWorldUI != null)
		{
			this.loadSceneWorldDialogActivatedFromWorld = this.worldUIActivated;
			this.CloseHub();
			this.ActivateWorldUI();
			this.fileBrowserWorldUI.SetTextEntry(false);
			this.fileBrowserWorldUI.keepOpen = false;
			this.fileBrowserWorldUI.hideExtension = true;
			this.fileBrowserWorldUI.SetTitle("Select Scene To Load");
			if (this.loadSceneButtonDisabled)
			{
				string text = this.demoScenesDir;
				this.fileBrowserWorldUI.defaultPath = text;
				this.fileBrowserWorldUI.GotoDirectory(text, null, true, false);
			}
			else
			{
				string text2 = "Saves/scene";
				this.fileBrowserWorldUI.defaultPath = text2;
				this.fileBrowserWorldUI.GotoDirectory(text2, null, true, true);
			}
			this.fileBrowserWorldUI.Show(new FileBrowserCallback(this.LoadFromSceneWorldDialog), false);
		}
	}

	// Token: 0x06005C3E RID: 23614 RVA: 0x00221598 File Offset: 0x0021F998
	public void LoadSceneWorldDialogWithPath(string path = "Saves/scene")
	{
		if (this.fileBrowserWorldUI != null && !this.loadSceneButtonDisabled)
		{
			if (FileManager.IsSecureReadPath(path))
			{
				this.loadSceneWorldDialogActivatedFromWorld = this.worldUIActivated;
				this.CloseHub();
				this.ActivateWorldUI();
				this.fileBrowserWorldUI.SetTextEntry(false);
				this.fileBrowserWorldUI.keepOpen = false;
				this.fileBrowserWorldUI.hideExtension = true;
				this.fileBrowserWorldUI.SetTitle("Select Scene To Load");
				this.fileBrowserWorldUI.defaultPath = path;
				this.fileBrowserWorldUI.GotoDirectory(path, null, true, true);
				this.fileBrowserWorldUI.Show(new FileBrowserCallback(this.LoadFromSceneWorldDialog), false);
			}
			else
			{
				this.Error("Attempted to use LoadSceneWorldDialogWithPath on a path that is not inside game directory", true, true);
			}
		}
	}

	// Token: 0x06005C3F RID: 23615 RVA: 0x00221660 File Offset: 0x0021FA60
	public void LoadTemplateWorldDialog(bool fromWorld = true)
	{
		if (this.templatesFileBrowserWorldUI != null && !this.advancedSceneEditDisabled)
		{
			this.loadTemplateWorldDialogActivatedFromWorld = this.worldUIActivated;
			this.CloseHub();
			this.ActivateWorldUI();
			this.templatesFileBrowserWorldUI.defaultPath = "Saves/scene";
			this.templatesFileBrowserWorldUI.SetTextEntry(false);
			this.templatesFileBrowserWorldUI.keepOpen = false;
			this.templatesFileBrowserWorldUI.hideExtension = true;
			this.templatesFileBrowserWorldUI.SetTitle("Select Template Scene To Load");
			this.templatesFileBrowserWorldUI.GotoDirectory("Saves/scene", null, true, true);
			this.templatesFileBrowserWorldUI.Show(new FileBrowserCallback(this.LoadFromTemplateWorldDialog), false);
		}
	}

	// Token: 0x06005C40 RID: 23616 RVA: 0x00221710 File Offset: 0x0021FB10
	public void CloseTemplateWorldDialog()
	{
	}

	// Token: 0x06005C41 RID: 23617 RVA: 0x00221712 File Offset: 0x0021FB12
	public void OpenWizard()
	{
		if (this.wizardWorldUI != null)
		{
			this.ActivateWorldUI();
			this.wizardWorldUI.gameObject.SetActive(true);
		}
	}

	// Token: 0x17000D8C RID: 3468
	// (get) Token: 0x06005C42 RID: 23618 RVA: 0x0022173C File Offset: 0x0021FB3C
	public bool HubOpen
	{
		get
		{
			return this.hubBrowser != null && this.hubBrowser.IsShowing && this.worldUIActivated;
		}
	}

	// Token: 0x06005C43 RID: 23619 RVA: 0x0022176A File Offset: 0x0021FB6A
	public void OpenHub()
	{
		if (this.hubBrowser != null)
		{
			this.hubOpenedFromWorld = false;
			this.hubBrowser.Show();
		}
	}

	// Token: 0x06005C44 RID: 23620 RVA: 0x0022178F File Offset: 0x0021FB8F
	public void OpenHubFromWorldUI()
	{
		if (this.hubBrowser != null)
		{
			this.hubOpenedFromWorld = true;
			this.hubBrowser.Show();
		}
	}

	// Token: 0x06005C45 RID: 23621 RVA: 0x002217B4 File Offset: 0x0021FBB4
	public void CloseHub()
	{
		if (this.hubBrowser != null)
		{
			this.hubBrowser.Hide();
		}
		if (!this.hubOpenedFromWorld)
		{
			this.DeactivateWorldUI();
		}
		else
		{
			this.hubOpenedFromWorld = false;
		}
	}

	// Token: 0x17000D8D RID: 3469
	// (get) Token: 0x06005C46 RID: 23622 RVA: 0x002217EF File Offset: 0x0021FBEF
	protected bool startSceneEnabled
	{
		get
		{
			if (GlobalSceneOptions.singleton != null)
			{
				return GlobalSceneOptions.singleton.enableStartScene;
			}
			return this.enableStartScene;
		}
	}

	// Token: 0x17000D8E RID: 3470
	// (get) Token: 0x06005C47 RID: 23623 RVA: 0x00221812 File Offset: 0x0021FC12
	protected JSONEmbed embeddedJSONScene
	{
		get
		{
			if (GlobalSceneOptions.singleton != null)
			{
				return GlobalSceneOptions.singleton.startJSONEmbedScene;
			}
			return this.startJSONEmbedScene;
		}
	}

	// Token: 0x17000D8F RID: 3471
	// (get) Token: 0x06005C48 RID: 23624 RVA: 0x00221835 File Offset: 0x0021FC35
	public bool UIDisabled
	{
		get
		{
			if (GlobalSceneOptions.singleton != null)
			{
				return GlobalSceneOptions.singleton.disableUI;
			}
			return this.disableUI;
		}
	}

	// Token: 0x17000D90 RID: 3472
	// (get) Token: 0x06005C49 RID: 23625 RVA: 0x00221858 File Offset: 0x0021FC58
	protected bool pointersAlwaysEnabled
	{
		get
		{
			if (GlobalSceneOptions.singleton != null)
			{
				return GlobalSceneOptions.singleton.alwaysEnablePointers;
			}
			return this.alwaysEnablePointers;
		}
	}

	// Token: 0x17000D91 RID: 3473
	// (get) Token: 0x06005C4A RID: 23626 RVA: 0x0022187B File Offset: 0x0021FC7B
	public bool navigationDisabled
	{
		get
		{
			if (GlobalSceneOptions.singleton != null)
			{
				return GlobalSceneOptions.singleton.disableNavigation;
			}
			return this.disableNavigation;
		}
	}

	// Token: 0x17000D92 RID: 3474
	// (get) Token: 0x06005C4B RID: 23627 RVA: 0x0022189E File Offset: 0x0021FC9E
	protected bool VRDisabled
	{
		get
		{
			if (GlobalSceneOptions.singleton != null)
			{
				return GlobalSceneOptions.singleton.disableVR;
			}
			return this.disableVR;
		}
	}

	// Token: 0x06005C4C RID: 23628 RVA: 0x002218C4 File Offset: 0x0021FCC4
	public void HardReset()
	{
		this.UnregisterAllPrefabsFromAtoms();
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	// Token: 0x17000D93 RID: 3475
	// (get) Token: 0x06005C4D RID: 23629 RVA: 0x002218E9 File Offset: 0x0021FCE9
	protected Transform atomContainerTransform
	{
		get
		{
			if (GlobalSceneOptions.singleton != null)
			{
				return GlobalSceneOptions.singleton.atomContainer;
			}
			return this.atomContainer;
		}
	}

	// Token: 0x06005C4E RID: 23630 RVA: 0x0022190C File Offset: 0x0021FD0C
	protected void InitAtomPool()
	{
		this.typeToAtomPool = new Dictionary<string, List<Atom>>();
		if (this.atomPoolContainer == null)
		{
			GameObject gameObject = new GameObject("AtomPool");
			this.atomPoolContainer = gameObject.transform;
		}
		if (this.atomPoolContainer != null)
		{
			foreach (Atom atom in this.atomPoolContainer.GetComponentsInChildren<Atom>(true))
			{
				atom.inPool = true;
				List<Atom> list;
				if (!this.typeToAtomPool.TryGetValue(atom.type, out list))
				{
					list = new List<Atom>();
					this.typeToAtomPool.Add(atom.type, list);
				}
				list.Add(atom);
			}
		}
	}

	// Token: 0x06005C4F RID: 23631 RVA: 0x002219C4 File Offset: 0x0021FDC4
	protected Atom GetAtomOfTypeFromPool(string atomType)
	{
		List<Atom> list;
		if (this.typeToAtomPool.TryGetValue(atomType, out list))
		{
			foreach (Atom atom in list)
			{
				if (atom.inPool)
				{
					atom.inPool = false;
					atom.gameObject.SetActive(true);
					return atom;
				}
			}
		}
		return null;
	}

	// Token: 0x06005C50 RID: 23632 RVA: 0x00221A50 File Offset: 0x0021FE50
	protected void PutAtomBackInPool(Atom a)
	{
		a.transform.SetParent(this.atomPoolContainer);
		a.PrepareToPutBackInPool();
		a.gameObject.SetActive(false);
		a.inPool = true;
		List<Atom> list;
		if (!this.typeToAtomPool.TryGetValue(a.type, out list))
		{
			list = new List<Atom>();
			this.typeToAtomPool.Add(a.type, list);
		}
		bool flag = false;
		foreach (Atom x in list)
		{
			if (x == a)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			list.Add(a);
		}
	}

	// Token: 0x06005C51 RID: 23633 RVA: 0x00221B1C File Offset: 0x0021FF1C
	public void ClearAtomPool()
	{
		foreach (KeyValuePair<string, List<Atom>> keyValuePair in this.typeToAtomPool)
		{
			List<Atom> list = new List<Atom>();
			foreach (Atom atom in keyValuePair.Value)
			{
				if (atom.inPool)
				{
					list.Add(atom);
				}
			}
			foreach (Atom atom2 in list)
			{
				keyValuePair.Value.Remove(atom2);
				UnityEngine.Object.Destroy(atom2.gameObject);
				SuperController.AtomAsset atomAsset;
				if (atom2.loadedFromBundle && atom2.type != null && atom2.type != string.Empty && this.atomAssetByType.TryGetValue(atom2.type, out atomAsset))
				{
					this.UnregisterPrefab(atomAsset.assetBundleName, atomAsset.assetName);
				}
			}
		}
	}

	// Token: 0x06005C52 RID: 23634 RVA: 0x00221CAC File Offset: 0x002200AC
	public void OpenFolderInExplorer(string path)
	{
		if (this.MonitorRigActive && path != null && FileManager.DirectoryExists(path, true, false))
		{
			string fullPath = Path.GetFullPath(path);
			Process.Start("explorer", fullPath);
		}
	}

	// Token: 0x06005C53 RID: 23635 RVA: 0x00221CEA File Offset: 0x002200EA
	public string NormalizePath(string path)
	{
		return FileManager.NormalizePath(path);
	}

	// Token: 0x06005C54 RID: 23636 RVA: 0x00221CF4 File Offset: 0x002200F4
	public string NormalizeMediaPath(string path)
	{
		string result = path;
		if (path != null && path != string.Empty)
		{
			this.lastMediaDir = FileManager.GetDirectoryName(path, false);
			result = this.NormalizePath(path);
		}
		return result;
	}

	// Token: 0x06005C55 RID: 23637 RVA: 0x00221D30 File Offset: 0x00220130
	public string NormalizeScenePath(string path)
	{
		string result = path;
		if (path != null && path != string.Empty)
		{
			this.lastScenePathDir = FileManager.GetDirectoryName(path, false);
			result = this.NormalizePath(path);
		}
		return result;
	}

	// Token: 0x06005C56 RID: 23638 RVA: 0x00221D6C File Offset: 0x0022016C
	public string NormalizeDirectoryPath(string path)
	{
		string result = path;
		if (path != null && path != string.Empty)
		{
			this.lastBrowseDir = FileManager.GetDirectoryName(path, false);
			result = this.NormalizePath(path);
		}
		return result;
	}

	// Token: 0x06005C57 RID: 23639 RVA: 0x00221DA8 File Offset: 0x002201A8
	public string NormalizeLoadPath(string path)
	{
		string text = path;
		if (path != string.Empty && path != "NULL")
		{
			text = FileManager.NormalizeLoadPath(path);
			if (this.pathMigrationMappings != null && !text.StartsWith("http") && !File.Exists(text))
			{
				foreach (KeyValuePair<string, string> keyValuePair in this.pathMigrationMappings)
				{
					string key = keyValuePair.Key;
					string pattern = "^" + key;
					if (Regex.IsMatch(text, pattern))
					{
						string value = keyValuePair.Value;
						string text2 = Regex.Replace(text, pattern, value);
						text = text2;
					}
				}
			}
		}
		return text;
	}

	// Token: 0x06005C58 RID: 23640 RVA: 0x00221E88 File Offset: 0x00220288
	public void PushOverrideLoadDirFromFilePath(string path)
	{
		FileManager.PushLoadDirFromFilePath(path, true);
	}

	// Token: 0x06005C59 RID: 23641 RVA: 0x00221E91 File Offset: 0x00220291
	public void PopOverrideLoadDir()
	{
		FileManager.PopLoadDir();
	}

	// Token: 0x06005C5A RID: 23642 RVA: 0x00221E99 File Offset: 0x00220299
	public void SetLoadDirFromFilePath(string path)
	{
		FileManager.SetLoadDirFromFilePath(path, true);
	}

	// Token: 0x06005C5B RID: 23643 RVA: 0x00221EA2 File Offset: 0x002202A2
	public void SetSaveDirFromFilePath(string path)
	{
		FileManager.SetSaveDirFromFilePath(path, true);
	}

	// Token: 0x06005C5C RID: 23644 RVA: 0x00221EAB File Offset: 0x002202AB
	public void SetNullSaveDir()
	{
		FileManager.SetNullSaveDir();
	}

	// Token: 0x06005C5D RID: 23645 RVA: 0x00221EB4 File Offset: 0x002202B4
	private bool BuildObsoleteDirectoriesList()
	{
		this.directoriesToRemove = new List<string>();
		bool flag = false;
		if (this.obsoletePathsText != null)
		{
			this.obsoletePathsText.text = string.Empty;
		}
		try
		{
			foreach (string text in this.obsoleteDirectories)
			{
				if (FileManager.DirectoryExists(text, false, false))
				{
					this.directoriesToRemove.Add(text);
					flag = true;
					if (this.obsoletePathsText != null)
					{
						Text text2 = this.obsoletePathsText;
						text2.text = text2.text + "Found obsolete directory " + text + "\n";
					}
				}
			}
			if (this.obsoletePathsPanel != null)
			{
				this.obsoletePathsPanel.gameObject.SetActive(flag);
			}
		}
		catch (Exception arg)
		{
			this.Error("Exception during obsolete directory cleanup " + arg, true, true);
		}
		return flag;
	}

	// Token: 0x06005C5E RID: 23646 RVA: 0x00221FB0 File Offset: 0x002203B0
	public void CancelRemoveObsoleteFiles()
	{
		if (this.obsoletePathsPanel != null)
		{
			this.obsoletePathsPanel.gameObject.SetActive(false);
		}
		if (this.startSceneEnabled && (!this.showMainHUDOnStart || this.UIDisabled || this._onStartupSkipStartScreen))
		{
			base.StartCoroutine(this.DelayStart());
		}
	}

	// Token: 0x06005C5F RID: 23647 RVA: 0x00222018 File Offset: 0x00220418
	public void RemoveObsoleteDirectories()
	{
		try
		{
			foreach (string path in this.directoriesToRemove)
			{
				if (FileManager.DirectoryExists(path, false, false))
				{
					FileManager.DeleteDirectory(path, true);
				}
			}
		}
		catch (Exception arg)
		{
			this.Error("Exception during obsolete directory removal " + arg, true, true);
		}
		this.CancelRemoveObsoleteFiles();
	}

	// Token: 0x06005C60 RID: 23648 RVA: 0x002220B4 File Offset: 0x002204B4
	protected void BuildMigrationMappings()
	{
		this.legacyDirectories = new List<string>();
		this.legacyDirectories.Add("Textures/");
		this.legacyDirectories.Add("Saves/Scripts/");
		this.legacyDirectories.Add("Saves/Assets/");
		this.legacyDirectories.Add("Import/morphs/");
		this.legacyDirectories.Add("Import/");
		this.pathMigrationMappings = new Dictionary<string, string>();
		this.pathMigrationMappings.Add("Textures/", "Custom/Atom/Person/Textures/");
		this.pathMigrationMappings.Add("Saves/Scripts/", "Custom/Scripts/");
		this.pathMigrationMappings.Add("Import/morphs/", "Custom/Atom/Person/Morphs/");
		this.pathMigrationMappings.Add("Saves/Assets/", "Custom/Assets/");
	}

	// Token: 0x06005C61 RID: 23649 RVA: 0x0022217C File Offset: 0x0022057C
	protected bool BuildFilesToMigrateMap()
	{
		this.filesToMigrateMap = new Dictionary<string, string>();
		bool flag = false;
		try
		{
			bool flag2 = false;
			StringBuilder stringBuilder = new StringBuilder(25000);
			StringBuilder stringBuilder2 = new StringBuilder(25000);
			StreamWriter streamWriter = new StreamWriter("migrate.log");
			streamWriter.WriteLine("Report:");
			foreach (KeyValuePair<string, string> keyValuePair in this.pathMigrationMappings)
			{
				string key = keyValuePair.Key;
				string value = keyValuePair.Value;
				if (FileManager.DirectoryExists(key, false, false))
				{
					string[] files = Directory.GetFiles(key, "*", SearchOption.AllDirectories);
					string pattern = "^" + key;
					foreach (string text in files)
					{
						flag = true;
						string text2 = text;
						string text3 = Regex.Replace(text2, pattern, value);
						string value2 = text2;
						string value3 = text3;
						if (text2.Length > text3.Length)
						{
							value3 = text3.PadRight(text2.Length);
						}
						else if (text3.Length > text2.Length)
						{
							value2 = text2.PadRight(text3.Length);
						}
						streamWriter.WriteLine(text2 + " -> " + text3);
						if (stringBuilder.Length < 16000 && stringBuilder2.Length < 16000)
						{
							stringBuilder.AppendLine(value2);
							stringBuilder2.AppendLine(value3);
						}
						else
						{
							flag2 = true;
						}
						this.filesToMigrateMap.Add(text, text3);
					}
				}
			}
			if (flag2)
			{
				stringBuilder.AppendLine("Truncated...too long to display. See migrate.log");
			}
			if (!flag)
			{
				streamWriter.WriteLine("No files found that need migrating");
			}
			streamWriter.Close();
			if (this.oldPathsText != null)
			{
				this.oldPathsText.text = stringBuilder.ToString();
			}
			if (this.newPathsText != null)
			{
				this.newPathsText.text = stringBuilder2.ToString();
			}
			if (flag)
			{
				this.HideMainHUD();
			}
			if (this.migratePathsPanel != null)
			{
				this.migratePathsPanel.gameObject.SetActive(flag);
			}
		}
		catch (Exception arg)
		{
			this.Error("Exception during search for migration file " + arg, true, true);
		}
		return flag;
	}

	// Token: 0x06005C62 RID: 23650 RVA: 0x00222418 File Offset: 0x00220818
	public void CancelMigrateFiles()
	{
		if (this.migratePathsPanel != null)
		{
			this.migratePathsPanel.gameObject.SetActive(false);
		}
		if (this.startSceneEnabled && (!this.showMainHUDOnStart || this.UIDisabled || this._onStartupSkipStartScreen))
		{
			base.StartCoroutine(this.DelayStart());
		}
	}

	// Token: 0x06005C63 RID: 23651 RVA: 0x00222480 File Offset: 0x00220880
	protected void RemoveLegacyDirectories(StreamWriter log)
	{
		try
		{
			foreach (string text in this.legacyDirectories)
			{
				if (FileManager.DirectoryExists(text, false, false))
				{
					string[] files = Directory.GetFiles(text, "*", SearchOption.AllDirectories);
					if (files.Length == 0)
					{
						if (log != null)
						{
							log.WriteLine("Deleting directory " + text);
						}
						UnityEngine.Debug.Log("Delete directory " + text);
						Directory.Delete(text, true);
					}
				}
			}
		}
		catch (Exception arg)
		{
			this.Error("Exception during migrate directory removal " + arg, true, true);
		}
	}

	// Token: 0x06005C64 RID: 23652 RVA: 0x00222550 File Offset: 0x00220950
	public void MigrateFilesInMigrateMap()
	{
		if (this.filesToMigrateMap != null)
		{
			try
			{
				StreamWriter streamWriter = new StreamWriter("migrate.log", true);
				foreach (KeyValuePair<string, string> keyValuePair in this.filesToMigrateMap)
				{
					string key = keyValuePair.Key;
					string value = keyValuePair.Value;
					streamWriter.WriteLine(string.Concat(new string[]
					{
						"Migrate file ",
						key,
						" to ",
						value,
						" ..."
					}));
					UnityEngine.Debug.Log("Migrate file " + key + " to " + value);
					string directoryName = Path.GetDirectoryName(value);
					try
					{
						if (!Directory.Exists(directoryName))
						{
							Directory.CreateDirectory(directoryName);
						}
						File.SetAttributes(key, FileAttributes.Normal);
						if (!File.Exists(value))
						{
							File.Move(key, value);
							streamWriter.WriteLine("  ...File moved");
						}
						else
						{
							streamWriter.WriteLine("  ...File already exists in new location. Just removing old file");
							File.Delete(key);
						}
					}
					catch (Exception ex)
					{
						this.Error(string.Concat(new object[]
						{
							"Exception during migrate of ",
							key,
							" :",
							ex
						}), true, true);
					}
				}
				this.RemoveLegacyDirectories(streamWriter);
				streamWriter.Close();
			}
			catch (Exception arg)
			{
				this.Error("Exception during migrate copy " + arg, true, true);
			}
		}
		this.CancelMigrateFiles();
	}

	// Token: 0x17000D94 RID: 3476
	// (get) Token: 0x06005C65 RID: 23653 RVA: 0x0022271C File Offset: 0x00220B1C
	// (set) Token: 0x06005C66 RID: 23654 RVA: 0x00222724 File Offset: 0x00220B24
	public float loResScreenShotCameraFOV
	{
		get
		{
			return this._loResScreenShotCameraFOV;
		}
		set
		{
			if (this._loResScreenShotCameraFOV != value)
			{
				this._loResScreenShotCameraFOV = value;
				if (this.screenshotCamera != null)
				{
					this.screenshotCamera.fieldOfView = this._loResScreenShotCameraFOV;
				}
				if (this.loResScreenShotCameraFOVSlider != null)
				{
					this.loResScreenShotCameraFOVSlider.value = value;
				}
				if (UserPreferences.singleton != null)
				{
					UserPreferences.singleton.SavePreferences();
				}
			}
		}
	}

	// Token: 0x17000D95 RID: 3477
	// (get) Token: 0x06005C67 RID: 23655 RVA: 0x0022279D File Offset: 0x00220B9D
	// (set) Token: 0x06005C68 RID: 23656 RVA: 0x002227A8 File Offset: 0x00220BA8
	public float hiResScreenShotCameraFOV
	{
		get
		{
			return this._hiResScreenShotCameraFOV;
		}
		set
		{
			if (this._hiResScreenShotCameraFOV != value)
			{
				this._hiResScreenShotCameraFOV = value;
				if (this.hiResScreenshotCamera != null)
				{
					this.hiResScreenshotCamera.fieldOfView = this._hiResScreenShotCameraFOV;
				}
				if (this.hiResScreenShotCameraFOVSlider != null)
				{
					this.hiResScreenShotCameraFOVSlider.value = value;
				}
				if (UserPreferences.singleton != null)
				{
					UserPreferences.singleton.SavePreferences();
				}
			}
		}
	}

	// Token: 0x06005C69 RID: 23657 RVA: 0x00222824 File Offset: 0x00220C24
	private void ProcessSaveScreenshot(bool force = false)
	{
		if (this.GetRightSelect() || this.GetLeftSelect() || this.GetMouseSelect() || force)
		{
			if (this.screenshotCamera != null)
			{
				RenderTexture targetTexture = this.screenshotCamera.targetTexture;
				if (targetTexture != null)
				{
					try
					{
						Texture2D texture2D = new Texture2D(targetTexture.width, targetTexture.height, TextureFormat.ARGB32, false);
						RenderTexture.active = targetTexture;
						texture2D.ReadPixels(new Rect(0f, 0f, (float)targetTexture.width, (float)targetTexture.height), 0, 0);
						texture2D.Apply();
						byte[] bytes = texture2D.EncodeToJPG();
						string text = this.savingName.Replace(".json", ".jpg");
						text = text.Replace(".vac", ".jpg");
						FileManager.WriteAllBytes(text, bytes);
						if (this.fileBrowserUI != null)
						{
							this.fileBrowserUI.ClearCacheImage(text);
						}
						if (this.screenShotCallback != null)
						{
							this.screenShotCallback(text);
							this.screenShotCallback = null;
						}
						UnityEngine.Object.Destroy(texture2D);
					}
					catch (Exception ex)
					{
						SuperController.LogError("Exception during screenshot processing: " + ex.Message);
					}
				}
				this.screenshotCamera.enabled = false;
			}
			this.SelectModeOff();
			this.ShowMainHUDAuto();
		}
		if (this.GetCancel())
		{
			this.SelectModeOff();
			this.ShowMainHUDAuto();
		}
	}

	// Token: 0x06005C6A RID: 23658 RVA: 0x002229A0 File Offset: 0x00220DA0
	private void ProcessHiResScreenshot()
	{
		try
		{
			if ((this.GetRightSelect() || this.GetLeftSelect() || this.GetMouseSelect()) && this.hiResScreenshotCamera != null)
			{
				RenderTexture targetTexture = this.hiResScreenshotCamera.targetTexture;
				if (targetTexture != null)
				{
					Texture2D texture2D = new Texture2D(targetTexture.width, targetTexture.height, TextureFormat.ARGB32, false);
					RenderTexture.active = targetTexture;
					texture2D.ReadPixels(new Rect(0f, 0f, (float)targetTexture.width, (float)targetTexture.height), 0, 0);
					texture2D.Apply();
					byte[] bytes = texture2D.EncodeToJPG(100);
					int num = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
					string text = this.savesDirResolved + "screenshots\\" + num.ToString() + ".jpg";
					int num2 = text.LastIndexOf('\\');
					if (num2 >= 0)
					{
						string path = text.Substring(0, num2);
						FileManager.CreateDirectory(path);
					}
					FileManager.WriteAllBytes(text, bytes);
					if (SkyshopLightController.singleton != null)
					{
						SkyshopLightController.singleton.Flash();
					}
					UnityEngine.Object.Destroy(texture2D);
				}
			}
			if (this.GetCancel())
			{
				this.SelectModeOff();
			}
		}
		catch (Exception ex)
		{
			SuperController.LogError("Exception during process of screenshot: " + ex.Message);
		}
	}

	// Token: 0x06005C6B RID: 23659 RVA: 0x00222B28 File Offset: 0x00220F28
	public void DoSaveScreenshot(string saveName, SuperController.ScreenShotCallback callback = null)
	{
		if (this.screenshotCamera != null)
		{
			this.savingName = saveName;
			this.screenShotCallback = callback;
			if (this.screenshotPreview != null)
			{
				this.screenshotPreview.gameObject.SetActive(true);
			}
			this.ResetSelectionInstances();
			this.ClearSelectionHUDs();
			this.HideMainHUD();
			this.selectMode = SuperController.SelectMode.SaveScreenshot;
			this.helpText = "Aim head and press select to take screenshot for save.";
			this.SyncVisibility();
			this.screenshotCamera.enabled = true;
			if (this.loResScreenShotCameraFOVSlider != null)
			{
				this.screenshotCamera.fieldOfView = this.loResScreenShotCameraFOVSlider.value;
			}
			else
			{
				this.screenshotCamera.fieldOfView = 40f;
			}
		}
	}

	// Token: 0x06005C6C RID: 23660 RVA: 0x00222BEC File Offset: 0x00220FEC
	public string[] GetFilesAtPath(string path, string pattern = null)
	{
		string[] result = null;
		if (FileManager.IsSecureReadPath(path))
		{
			result = FileManager.GetFiles(path, pattern, true);
		}
		else
		{
			this.Error("Attempted to use GetFilesAtPath on a path that is not inside game directory. That is not allowed", true, true);
		}
		return result;
	}

	// Token: 0x06005C6D RID: 23661 RVA: 0x00222C24 File Offset: 0x00221024
	public string[] GetDirectoriesAtPath(string path, string pattern = null)
	{
		string[] result = null;
		if (FileManager.IsSecureReadPath(path))
		{
			result = FileManager.GetDirectories(path, pattern, true);
		}
		else
		{
			this.Error("Attempted to use GetDirectoriesAtPath on a path that is not inside game directory. That is not allowed", true, true);
		}
		return result;
	}

	// Token: 0x06005C6E RID: 23662 RVA: 0x00222C5C File Offset: 0x0022105C
	public string ReadFileIntoString(string path)
	{
		string result = null;
		if (FileManager.IsSecureReadPath(path))
		{
			result = FileManager.ReadAllText(path, true);
		}
		else
		{
			this.Error("Attempted to use ReadFileIntoString on a path that is not inside game directory. That is not allowed", true, true);
		}
		return result;
	}

	// Token: 0x06005C6F RID: 23663 RVA: 0x00222C94 File Offset: 0x00221094
	public void SaveStringIntoFile(string path, string contents)
	{
		SuperController.<SaveStringIntoFile>c__AnonStorey13 <SaveStringIntoFile>c__AnonStorey = new SuperController.<SaveStringIntoFile>c__AnonStorey13();
		<SaveStringIntoFile>c__AnonStorey.path = path;
		<SaveStringIntoFile>c__AnonStorey.contents = contents;
		<SaveStringIntoFile>c__AnonStorey.$this = this;
		if (!FileManager.IsSecurePluginWritePath(<SaveStringIntoFile>c__AnonStorey.path))
		{
			this.Error("Attempted to use SaveStringIntoFile on a path that is not inside game directory Saves or Custom area. That is not allowed", true, true);
		}
		if (File.Exists(<SaveStringIntoFile>c__AnonStorey.path))
		{
			if (!FileManager.IsPluginWritePathThatNeedsConfirm(<SaveStringIntoFile>c__AnonStorey.path))
			{
				try
				{
					FileManager.WriteAllText(<SaveStringIntoFile>c__AnonStorey.path, <SaveStringIntoFile>c__AnonStorey.contents);
				}
				catch (Exception arg)
				{
					this.Error("Exception while saving string into file " + arg, true, true);
				}
			}
			else
			{
				FileManager.ConfirmPluginActionWithUser("save string into file " + <SaveStringIntoFile>c__AnonStorey.path, new UserActionCallback(<SaveStringIntoFile>c__AnonStorey.<>m__0), null);
			}
		}
		else
		{
			try
			{
				FileManager.WriteAllText(<SaveStringIntoFile>c__AnonStorey.path, <SaveStringIntoFile>c__AnonStorey.contents);
			}
			catch (Exception arg2)
			{
				this.Error("Exception while saving string into file " + arg2, true, true);
			}
		}
	}

	// Token: 0x06005C70 RID: 23664 RVA: 0x00222DA0 File Offset: 0x002211A0
	private void SyncGameMode()
	{
		if (this._gameMode == SuperController.GameMode.Edit)
		{
			if (this.editModeToggle != null)
			{
				this.editModeToggle.isOn = true;
			}
			if (this.playModeToggle != null)
			{
				this.playModeToggle.isOn = false;
			}
			if (this._activeUI == SuperController.ActiveUI.SelectedOptions)
			{
				this.activeUI = SuperController.ActiveUI.SelectedOptions;
			}
			if (this.editModeOnlyTransforms != null)
			{
				foreach (Transform transform in this.editModeOnlyTransforms)
				{
					if (transform != null)
					{
						transform.gameObject.SetActive(true);
					}
				}
			}
			if (this.playModeOnlyTransforms != null)
			{
				foreach (Transform transform2 in this.playModeOnlyTransforms)
				{
					if (transform2 != null)
					{
						transform2.gameObject.SetActive(false);
					}
				}
			}
		}
		else
		{
			if (this.editModeToggle != null)
			{
				this.editModeToggle.isOn = false;
			}
			if (this.playModeToggle != null)
			{
				this.playModeToggle.isOn = true;
			}
			if (this._activeUI == SuperController.ActiveUI.SelectedOptions)
			{
				this.activeUI = SuperController.ActiveUI.SelectedOptions;
			}
			if (this.editModeOnlyTransforms != null)
			{
				foreach (Transform transform3 in this.editModeOnlyTransforms)
				{
					if (transform3 != null)
					{
						transform3.gameObject.SetActive(false);
					}
				}
			}
			if (this.playModeOnlyTransforms != null)
			{
				foreach (Transform transform4 in this.playModeOnlyTransforms)
				{
					if (transform4 != null)
					{
						transform4.gameObject.SetActive(true);
					}
				}
			}
		}
		this.SyncAdvancedSceneEditModeTransforms();
		this.SyncVisibility();
	}

	// Token: 0x17000D96 RID: 3478
	// (get) Token: 0x06005C71 RID: 23665 RVA: 0x00222F89 File Offset: 0x00221389
	// (set) Token: 0x06005C72 RID: 23666 RVA: 0x00222F94 File Offset: 0x00221394
	public SuperController.GameMode gameMode
	{
		get
		{
			return this._gameMode;
		}
		set
		{
			if (this._gameMode != value)
			{
				this._gameMode = value;
				this.SyncGameMode();
				if (this._autoFreezeAnimationOnSwitchToEditMode)
				{
					this.SetFreezeAnimation(this._gameMode == SuperController.GameMode.Edit);
				}
				if (this.onGameModeChangedHandlers != null)
				{
					this.onGameModeChangedHandlers(this._gameMode);
				}
			}
		}
	}

	// Token: 0x06005C73 RID: 23667 RVA: 0x00222FF0 File Offset: 0x002213F0
	public void PurgeImageCache()
	{
		if (ImageLoaderThreaded.singleton != null)
		{
			ImageLoaderThreaded.singleton.PurgeAllTextures();
		}
	}

	// Token: 0x06005C74 RID: 23668 RVA: 0x0022300C File Offset: 0x0022140C
	public void UnloadUnusedResources()
	{
		Resources.UnloadUnusedAssets();
	}

	// Token: 0x06005C75 RID: 23669 RVA: 0x00223014 File Offset: 0x00221414
	public void GarbageCollect()
	{
		SuperController.LogMessage("Memory usage before garbage collect " + GC.GetTotalMemory(false));
		GC.Collect();
		SuperController.LogMessage("Memory usage  after garbage collect " + GC.GetTotalMemory(true));
	}

	// Token: 0x06005C76 RID: 23670 RVA: 0x0022304F File Offset: 0x0022144F
	public void ReportLoadedAssetBundles()
	{
		AssetBundleManager.ReportLoadedAssetBundles();
	}

	// Token: 0x06005C77 RID: 23671 RVA: 0x00223056 File Offset: 0x00221456
	public void Quit()
	{
		Application.Quit();
	}

	// Token: 0x06005C78 RID: 23672 RVA: 0x0022305D File Offset: 0x0022145D
	public static void LogError(string err)
	{
		if (SuperController._singleton != null)
		{
			SuperController._singleton.Error(err, true, true);
		}
		else
		{
			UnityEngine.Debug.LogError(err);
		}
	}

	// Token: 0x06005C79 RID: 23673 RVA: 0x00223087 File Offset: 0x00221487
	public static void LogError(string err, bool logToFile)
	{
		if (SuperController._singleton != null)
		{
			SuperController._singleton.Error(err, logToFile, true);
		}
		else
		{
			UnityEngine.Debug.LogError(err);
		}
	}

	// Token: 0x06005C7A RID: 23674 RVA: 0x002230B1 File Offset: 0x002214B1
	public static void LogError(string err, bool logToFile, bool splash)
	{
		if (SuperController._singleton != null)
		{
			SuperController._singleton.Error(err, logToFile, splash);
		}
		else
		{
			UnityEngine.Debug.LogError(err);
		}
	}

	// Token: 0x06005C7B RID: 23675 RVA: 0x002230DB File Offset: 0x002214DB
	public static void LogMessage(string msg)
	{
		if (SuperController._singleton != null)
		{
			SuperController._singleton.Message(msg, true, true);
		}
		else
		{
			UnityEngine.Debug.Log(msg);
		}
	}

	// Token: 0x06005C7C RID: 23676 RVA: 0x00223105 File Offset: 0x00221505
	public static void LogMessage(string msg, bool logToFile)
	{
		if (SuperController._singleton != null)
		{
			SuperController._singleton.Message(msg, logToFile, true);
		}
		else
		{
			UnityEngine.Debug.Log(msg);
		}
	}

	// Token: 0x06005C7D RID: 23677 RVA: 0x0022312F File Offset: 0x0022152F
	public static void LogMessage(string msg, bool logToFile, bool splash)
	{
		if (SuperController._singleton != null)
		{
			SuperController._singleton.Message(msg, logToFile, splash);
		}
		else
		{
			UnityEngine.Debug.Log(msg);
		}
	}

	// Token: 0x06005C7E RID: 23678 RVA: 0x00223159 File Offset: 0x00221559
	public static void AlertUser(string alert, UnityAction okCallback, SuperController.DisplayUIChoice displayUIChoice = SuperController.DisplayUIChoice.Auto)
	{
		if (SuperController._singleton != null)
		{
			SuperController._singleton.Alert(alert, okCallback, displayUIChoice);
		}
	}

	// Token: 0x06005C7F RID: 23679 RVA: 0x00223178 File Offset: 0x00221578
	public static void AlertUser(string alert, UnityAction okCallback, UnityAction cancelCallback, SuperController.DisplayUIChoice displayUIChoice = SuperController.DisplayUIChoice.Auto)
	{
		if (SuperController._singleton != null)
		{
			SuperController._singleton.Alert(alert, okCallback, cancelCallback, displayUIChoice);
		}
	}

	// Token: 0x06005C80 RID: 23680 RVA: 0x00223198 File Offset: 0x00221598
	public void OpenErrorLogPanel()
	{
		if (!this._mainHUDVisible)
		{
			this.ShowMainHUDAuto();
		}
		if (this.errorLogPanel != null)
		{
			this.errorLogPanel.gameObject.SetActive(true);
		}
	}

	// Token: 0x06005C81 RID: 23681 RVA: 0x002231CD File Offset: 0x002215CD
	public void CloseErrorLogPanel()
	{
		if (this.errorLogPanel != null)
		{
			this.errorLogPanel.gameObject.SetActive(false);
		}
	}

	// Token: 0x17000D97 RID: 3479
	// (get) Token: 0x06005C82 RID: 23682 RVA: 0x002231F1 File Offset: 0x002215F1
	// (set) Token: 0x06005C83 RID: 23683 RVA: 0x002231FC File Offset: 0x002215FC
	public int errorCount
	{
		get
		{
			return this._errorCount;
		}
		set
		{
			if (this._errorCount != value)
			{
				this._errorCount = value;
				if (this.allErrorsCountText != null)
				{
					this.allErrorsCountText.text = this._errorCount.ToString();
				}
				if (this.allErrorsCountText2 != null)
				{
					this.allErrorsCountText2.text = this._errorCount.ToString();
				}
			}
		}
	}

	// Token: 0x06005C84 RID: 23684 RVA: 0x00223276 File Offset: 0x00221676
	public void ClearErrors()
	{
		this.errorCount = 0;
		this.errorLog = string.Empty;
		this.errorLogDirty = true;
		this.CloseErrorSplash();
	}

	// Token: 0x06005C85 RID: 23685 RVA: 0x00223298 File Offset: 0x00221698
	protected void SyncAllErrorsInputFields()
	{
		if (this.errorLogDirty)
		{
			if (this.allErrorsInputField != null)
			{
				this.allErrorsInputField.text = this.errorLog;
			}
			if (this.allErrorsInputField2 != null)
			{
				this.allErrorsInputField2.text = this.errorLog;
			}
			this.errorLogDirty = false;
		}
	}

	// Token: 0x17000D98 RID: 3480
	// (get) Token: 0x06005C86 RID: 23686 RVA: 0x002232FB File Offset: 0x002216FB
	// (set) Token: 0x06005C87 RID: 23687 RVA: 0x00223304 File Offset: 0x00221704
	public bool openMainHUDOnError
	{
		get
		{
			return this._openMainHUDOnError;
		}
		set
		{
			if (this._openMainHUDOnError != value)
			{
				this._openMainHUDOnError = value;
				if (this.openMainHUDOnErrorToggle != null)
				{
					this.openMainHUDOnErrorToggle.isOn = value;
				}
				if (UserPreferences.singleton != null)
				{
					UserPreferences.singleton.SavePreferences();
				}
			}
		}
	}

	// Token: 0x06005C88 RID: 23688 RVA: 0x0022335C File Offset: 0x0022175C
	public void Error(string err, bool logToFile = true, bool splash = true)
	{
		this.errorCount++;
		if (splash)
		{
			if (!this.worldUIActivated && !this._mainHUDVisible && this._openMainHUDOnError)
			{
				this.ShowMainHUDAuto();
			}
			this.errorSplashTimeRemaining = this.errorSplashTime;
		}
		if (this.errorLog != null && this.errorLog.Length > this.maxLength)
		{
			this.errorLog = this.errorLog.Substring(0, this.maxLength / 2);
			this.errorLog += "\n\n<Truncated>\n";
		}
		this.errorLog = this.errorLog + "\n!> " + err;
		this.errorLogDirty = true;
		if (logToFile)
		{
			UnityEngine.Debug.LogError(err);
		}
	}

	// Token: 0x06005C89 RID: 23689 RVA: 0x0022342B File Offset: 0x0022182B
	public void OpenMessageLogPanel()
	{
		if (!this._mainHUDVisible)
		{
			this.ShowMainHUDAuto();
		}
		if (this.msgLogPanel != null)
		{
			this.msgLogPanel.gameObject.SetActive(true);
		}
	}

	// Token: 0x06005C8A RID: 23690 RVA: 0x00223460 File Offset: 0x00221860
	public void CloseMessageLogPanel()
	{
		if (this.msgLogPanel != null)
		{
			this.msgLogPanel.gameObject.SetActive(false);
		}
	}

	// Token: 0x17000D99 RID: 3481
	// (get) Token: 0x06005C8B RID: 23691 RVA: 0x00223484 File Offset: 0x00221884
	// (set) Token: 0x06005C8C RID: 23692 RVA: 0x0022348C File Offset: 0x0022188C
	public int msgCount
	{
		get
		{
			return this._msgCount;
		}
		set
		{
			if (this._msgCount != value)
			{
				this._msgCount = value;
				if (this.allMessagesCountText != null)
				{
					this.allMessagesCountText.text = this._msgCount.ToString();
				}
				if (this.allMessagesCountText2 != null)
				{
					this.allMessagesCountText2.text = this._msgCount.ToString();
				}
			}
		}
	}

	// Token: 0x06005C8D RID: 23693 RVA: 0x00223508 File Offset: 0x00221908
	protected void SyncAllMessagesInputFields()
	{
		if (this.msgLogDirty)
		{
			if (this.allMessagesInputField != null)
			{
				this.allMessagesInputField.text = this.msgLog;
			}
			if (this.allMessagesInputField2 != null)
			{
				this.allMessagesInputField2.text = this.msgLog;
			}
			this.msgLogDirty = false;
		}
	}

	// Token: 0x06005C8E RID: 23694 RVA: 0x0022356B File Offset: 0x0022196B
	public void ClearMessages()
	{
		this.msgCount = 0;
		this.msgLog = string.Empty;
		this.msgLogDirty = true;
		this.CloseMessageSplash();
	}

	// Token: 0x06005C8F RID: 23695 RVA: 0x0022358C File Offset: 0x0022198C
	public void Message(string msg, bool logToFile = true, bool splash = true)
	{
		this.msgCount++;
		if (splash)
		{
			this.msgSplashTimeRemaining = this.msgSplashTime;
		}
		if (this.msgLog != null && this.msgLog.Length > this.maxLength)
		{
			this.msgLog = this.msgLog.Substring(0, this.maxLength / 2);
			this.msgLog += "\n\n<Truncated>\n";
		}
		this.msgLog = this.msgLog + "\n" + msg;
		this.msgLogDirty = true;
		if (logToFile)
		{
			UnityEngine.Debug.Log(msg);
		}
	}

	// Token: 0x06005C90 RID: 23696 RVA: 0x00223634 File Offset: 0x00221A34
	public void CloseMessageSplash()
	{
		if (this.msgSplashTimeRemaining > 0f)
		{
			this.msgSplashTimeRemaining = 0.001f;
		}
	}

	// Token: 0x06005C91 RID: 23697 RVA: 0x00223651 File Offset: 0x00221A51
	public void CloseErrorSplash()
	{
		if (this.errorSplashTimeRemaining > 0f)
		{
			this.errorSplashTimeRemaining = 0.001f;
		}
	}

	// Token: 0x06005C92 RID: 23698 RVA: 0x00223670 File Offset: 0x00221A70
	protected void CheckMessageAndErrorQueue()
	{
		if (!this.worldUIActivated)
		{
			if (this.hasPendingErrorSplash)
			{
				if (!this._mainHUDVisible)
				{
					this.ShowMainHUDAuto();
				}
				this.hasPendingErrorSplash = false;
			}
			if (this.errorSplashTimeRemaining > 0f)
			{
				this.errorSplashTimeRemaining -= Time.unscaledDeltaTime;
				if (this.errorSplashTimeRemaining <= 0f)
				{
					if (this.errorSplashTransform != null)
					{
						this.errorSplashTransform.gameObject.SetActive(false);
					}
				}
				else if (this.errorSplashTransform != null)
				{
					this.errorSplashTransform.gameObject.SetActive(true);
				}
			}
			else if (this.msgSplashTimeRemaining > 0f)
			{
				this.msgSplashTimeRemaining -= Time.unscaledDeltaTime;
				if (this.msgSplashTimeRemaining <= 0f)
				{
					if (this.msgSplashTransform != null)
					{
						this.msgSplashTransform.gameObject.SetActive(false);
					}
				}
				else if (this.msgSplashTransform != null)
				{
					this.msgSplashTransform.gameObject.SetActive(true);
				}
			}
		}
		else if (this.errorSplashTimeRemaining > 0f)
		{
			this.hasPendingErrorSplash = true;
		}
	}

	// Token: 0x06005C93 RID: 23699 RVA: 0x002237C4 File Offset: 0x00221BC4
	protected Transform SyncToDisplayChoice(SuperController.DisplayUIChoice displayUIChoice)
	{
		Transform result = null;
		if (displayUIChoice != SuperController.DisplayUIChoice.Auto)
		{
			if (displayUIChoice != SuperController.DisplayUIChoice.Normal)
			{
				if (displayUIChoice == SuperController.DisplayUIChoice.World)
				{
					result = this.worldAlertRoot;
					this.ActivateWorldUI();
				}
			}
			else
			{
				result = this.normalAlertRoot;
				this.DeactivateWorldUI();
				if (!this._mainHUDVisible)
				{
					this.ShowMainHUDAuto();
				}
			}
		}
		else if (this.worldUIActivated && !this._mainHUDVisible)
		{
			result = this.worldAlertRoot;
		}
		else
		{
			result = this.normalAlertRoot;
			if (!this._mainHUDVisible)
			{
				this.ShowMainHUDAuto();
			}
		}
		return result;
	}

	// Token: 0x06005C94 RID: 23700 RVA: 0x00223864 File Offset: 0x00221C64
	public void Alert(string alertMessage, UnityAction okAlertCallback, UnityAction cancelAlertCallback, SuperController.DisplayUIChoice displayUIChoice = SuperController.DisplayUIChoice.Auto)
	{
		if (this.okAndCancelAlertPrefab != null)
		{
			Transform parent = this.SyncToDisplayChoice(displayUIChoice);
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.okAndCancelAlertPrefab, parent, false);
			if (gameObject != null)
			{
				AlertUI component = gameObject.GetComponent<AlertUI>();
				if (component != null)
				{
					component.SetText(alertMessage);
					component.SetOKButton(okAlertCallback);
					component.SetCancelButton(cancelAlertCallback);
				}
			}
		}
	}

	// Token: 0x06005C95 RID: 23701 RVA: 0x002238D0 File Offset: 0x00221CD0
	public void Alert(string alertMessage, UnityAction okAlertCallback, SuperController.DisplayUIChoice displayUIChoice = SuperController.DisplayUIChoice.Auto)
	{
		if (this.okAlertPrefab != null)
		{
			Transform parent = this.SyncToDisplayChoice(displayUIChoice);
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.okAlertPrefab, parent, false);
			if (gameObject != null)
			{
				AlertUI component = gameObject.GetComponent<AlertUI>();
				if (component != null)
				{
					component.SetText(alertMessage);
					component.SetOKButton(okAlertCallback);
				}
			}
		}
	}

	// Token: 0x06005C96 RID: 23702 RVA: 0x00223931 File Offset: 0x00221D31
	public bool IsSimulationResetting()
	{
		return this._resetSimulation;
	}

	// Token: 0x17000D9A RID: 3482
	// (get) Token: 0x06005C97 RID: 23703 RVA: 0x00223939 File Offset: 0x00221D39
	// (set) Token: 0x06005C98 RID: 23704 RVA: 0x00223941 File Offset: 0x00221D41
	protected bool resetSimulation
	{
		get
		{
			return this._resetSimulation;
		}
		set
		{
			if (this._resetSimulation != value)
			{
				this._resetSimulation = value;
			}
		}
	}

	// Token: 0x06005C99 RID: 23705 RVA: 0x00223958 File Offset: 0x00221D58
	protected void CheckResumeSimulation()
	{
		if (this.pauseFrames >= 0)
		{
			this.pauseFrames--;
			if (this.pauseFrames < 0 && this.resetSimulationTimerFlag != null)
			{
				this.resetSimulationTimerFlag.Raise();
			}
		}
		if (this.waitResumeSimulationFlags == null)
		{
			this.waitResumeSimulationFlags = new List<AsyncFlag>();
		}
		bool flag = false;
		if (this.waitResumeSimulationFlags.Count > 0)
		{
			if (this.removeSimulationFlags == null)
			{
				this.removeSimulationFlags = new List<AsyncFlag>();
			}
			else
			{
				this.removeSimulationFlags.Clear();
			}
			foreach (Text text in this.waitReasonTexts)
			{
				text.text = string.Empty;
			}
			int num = 0;
			foreach (AsyncFlag asyncFlag in this.waitResumeSimulationFlags)
			{
				if (num < this.waitReasonTexts.Length)
				{
					this.waitReasonTexts[num].text = asyncFlag.Name;
					num++;
				}
				if (asyncFlag.Raised)
				{
					this.removeSimulationFlags.Add(asyncFlag);
					flag = true;
				}
			}
			foreach (AsyncFlag item in this.removeSimulationFlags)
			{
				this.waitResumeSimulationFlags.Remove(item);
			}
		}
		if (this.waitResumeSimulationFlags.Count > 0)
		{
			this.readyToResumeSimulation = false;
			if (this.waitTransform != null && !this.hideWaitTransform && !this.hiddenReset)
			{
				this.waitTransform.gameObject.SetActive(true);
			}
			this.resetSimulation = true;
		}
		else if (flag)
		{
			this.readyToResumeSimulation = true;
		}
		else if (this.readyToResumeSimulation)
		{
			if (this.waitTransform != null)
			{
				this.waitTransform.gameObject.SetActive(false);
			}
			this.readyToResumeSimulation = false;
			this.resetSimulation = false;
			this.hiddenReset = false;
		}
	}

	// Token: 0x06005C9A RID: 23706 RVA: 0x00223BB0 File Offset: 0x00221FB0
	public void PauseSimulation(AsyncFlag af, bool hidden = false)
	{
		this.ResetSimulation(af, hidden);
	}

	// Token: 0x06005C9B RID: 23707 RVA: 0x00223BBC File Offset: 0x00221FBC
	public void ResetSimulation(AsyncFlag af, bool hidden = false)
	{
		if (this.waitResumeSimulationFlags == null)
		{
			this.waitResumeSimulationFlags = new List<AsyncFlag>();
		}
		if (hidden)
		{
			if (!this.resetSimulation)
			{
				this.hiddenReset = true;
			}
		}
		else
		{
			this.hiddenReset = false;
		}
		if (!this.waitResumeSimulationFlags.Contains(af))
		{
			this.resetSimulation = true;
			this.waitResumeSimulationFlags.Add(af);
			if (this.atoms != null)
			{
				foreach (Atom atom in this.atomsList)
				{
					atom.ResetSimulation(af);
				}
			}
		}
	}

	// Token: 0x06005C9C RID: 23708 RVA: 0x00223C84 File Offset: 0x00222084
	public void PauseSimulation(int numFrames, string pauseName)
	{
		this.ResetSimulation(numFrames, pauseName, false);
	}

	// Token: 0x06005C9D RID: 23709 RVA: 0x00223C8F File Offset: 0x0022208F
	public void PauseSimulation(int numFrames, string pauseName, bool hidden)
	{
		this.ResetSimulation(numFrames, pauseName, hidden);
	}

	// Token: 0x06005C9E RID: 23710 RVA: 0x00223C9C File Offset: 0x0022209C
	public void ResetSimulation(int numFrames, string pauseName, bool hidden = false)
	{
		if (this.pauseFrames < numFrames)
		{
			this.pauseFrames = numFrames;
			if (this.waitResumeSimulationFlags == null)
			{
				this.waitResumeSimulationFlags = new List<AsyncFlag>();
			}
			if (this.resetSimulationTimerFlag == null)
			{
				this.resetSimulationTimerFlag = new AsyncFlag(pauseName);
			}
			else
			{
				this.resetSimulationTimerFlag.Name = pauseName;
			}
			if (!this.waitResumeSimulationFlags.Contains(this.resetSimulationTimerFlag))
			{
				this.resetSimulationTimerFlag.Lower();
				this.ResetSimulation(this.resetSimulationTimerFlag, hidden);
			}
		}
	}

	// Token: 0x17000D9B RID: 3483
	// (get) Token: 0x06005C9F RID: 23711 RVA: 0x00223D28 File Offset: 0x00222128
	// (set) Token: 0x06005CA0 RID: 23712 RVA: 0x00223D30 File Offset: 0x00222130
	public bool autoSimulation
	{
		get
		{
			return this._autoSimulation;
		}
		protected set
		{
			if (this._autoSimulation != value)
			{
				this._autoSimulation = value;
				Physics.autoSimulation = value;
				this.SyncActiveHands();
				if (this._autoSimulation)
				{
					foreach (Atom atom in this.atomsList)
					{
						atom.ResetRigidbodies();
					}
				}
			}
		}
	}

	// Token: 0x06005CA1 RID: 23713 RVA: 0x00223DB8 File Offset: 0x002221B8
	protected void CheckResumeAutoSimulation()
	{
		if (this.waitResumeAutoSimulationFlags == null)
		{
			this.waitResumeAutoSimulationFlags = new List<AsyncFlag>();
		}
		bool flag = false;
		if (this.waitResumeAutoSimulationFlags.Count > 0)
		{
			if (this.removeAutoSimulationFlags == null)
			{
				this.removeAutoSimulationFlags = new List<AsyncFlag>();
			}
			else
			{
				this.removeAutoSimulationFlags.Clear();
			}
			foreach (AsyncFlag asyncFlag in this.waitResumeAutoSimulationFlags)
			{
				if (asyncFlag.Raised)
				{
					this.removeAutoSimulationFlags.Add(asyncFlag);
					flag = true;
				}
			}
			foreach (AsyncFlag item in this.removeAutoSimulationFlags)
			{
				this.waitResumeAutoSimulationFlags.Remove(item);
			}
		}
		if (this.waitResumeAutoSimulationFlags.Count > 0)
		{
			this.autoSimulation = false;
		}
		else if (flag)
		{
			this.autoSimulation = true;
		}
	}

	// Token: 0x06005CA2 RID: 23714 RVA: 0x00223EF4 File Offset: 0x002222F4
	public void PauseAutoSimulation(AsyncFlag af)
	{
		if (this.waitResumeAutoSimulationFlags == null)
		{
			this.waitResumeAutoSimulationFlags = new List<AsyncFlag>();
		}
		if (!this.waitResumeAutoSimulationFlags.Contains(af))
		{
			this.autoSimulation = false;
			this.waitResumeAutoSimulationFlags.Add(af);
		}
	}

	// Token: 0x06005CA3 RID: 23715 RVA: 0x00223F30 File Offset: 0x00222330
	protected void SetPauseAutoSimulation(bool b)
	{
		this.pauseAutoSimulation = b;
	}

	// Token: 0x17000D9C RID: 3484
	// (get) Token: 0x06005CA4 RID: 23716 RVA: 0x00223F39 File Offset: 0x00222339
	// (set) Token: 0x06005CA5 RID: 23717 RVA: 0x00223F44 File Offset: 0x00222344
	public bool pauseAutoSimulation
	{
		get
		{
			return this._pauseAutoSimulation;
		}
		set
		{
			if (this._pauseAutoSimulation != value)
			{
				if (this.pauseAutoSimulationFlag != null)
				{
					this.pauseAutoSimulationFlag.Raise();
				}
				this._pauseAutoSimulation = value;
				if (this.pauseAutoSimulationToggle != null)
				{
					this.pauseAutoSimulationToggle.isOn = this._pauseAutoSimulation;
				}
				if (this._pauseAutoSimulation)
				{
					this.pauseAutoSimulationFlag = new AsyncFlag("Global Pause Auto Simulation");
					this.PauseAutoSimulation(this.pauseAutoSimulationFlag);
				}
			}
		}
	}

	// Token: 0x17000D9D RID: 3485
	// (get) Token: 0x06005CA6 RID: 23718 RVA: 0x00223FC3 File Offset: 0x002223C3
	// (set) Token: 0x06005CA7 RID: 23719 RVA: 0x00223FCC File Offset: 0x002223CC
	public bool render
	{
		get
		{
			return this._render;
		}
		protected set
		{
			if (this._render != value)
			{
				this._render = value;
				if (this.atoms != null)
				{
					foreach (Atom atom in this.atomsList)
					{
						atom.globalDisableRender = !this._render;
					}
				}
			}
		}
	}

	// Token: 0x06005CA8 RID: 23720 RVA: 0x00224050 File Offset: 0x00222450
	protected void CheckResumeRender()
	{
		if (this.waitResumeRenderFlags == null)
		{
			this.waitResumeRenderFlags = new List<AsyncFlag>();
		}
		bool flag = false;
		if (this.waitResumeRenderFlags.Count > 0)
		{
			if (this.removeRenderFlags == null)
			{
				this.removeRenderFlags = new List<AsyncFlag>();
			}
			else
			{
				this.removeRenderFlags.Clear();
			}
			foreach (AsyncFlag asyncFlag in this.waitResumeRenderFlags)
			{
				if (asyncFlag.Raised)
				{
					this.removeRenderFlags.Add(asyncFlag);
					flag = true;
				}
			}
			foreach (AsyncFlag item in this.removeRenderFlags)
			{
				this.waitResumeRenderFlags.Remove(item);
			}
		}
		if (this.waitResumeRenderFlags.Count > 0)
		{
			this.render = false;
		}
		else if (flag)
		{
			this.render = true;
		}
	}

	// Token: 0x06005CA9 RID: 23721 RVA: 0x0022418C File Offset: 0x0022258C
	public void PauseRender(AsyncFlag af)
	{
		if (this.waitResumeRenderFlags == null)
		{
			this.waitResumeRenderFlags = new List<AsyncFlag>();
		}
		if (!this.waitResumeRenderFlags.Contains(af))
		{
			this.render = false;
			this.waitResumeRenderFlags.Add(af);
		}
	}

	// Token: 0x06005CAA RID: 23722 RVA: 0x002241C8 File Offset: 0x002225C8
	protected void SetPauseRender(bool b)
	{
		this.pauseRender = b;
	}

	// Token: 0x17000D9E RID: 3486
	// (get) Token: 0x06005CAB RID: 23723 RVA: 0x002241D1 File Offset: 0x002225D1
	// (set) Token: 0x06005CAC RID: 23724 RVA: 0x002241DC File Offset: 0x002225DC
	public bool pauseRender
	{
		get
		{
			return this._pauseRender;
		}
		set
		{
			if (this._pauseRender != value)
			{
				if (this.pauseRenderFlag != null)
				{
					this.pauseRenderFlag.Raise();
				}
				this._pauseRender = value;
				if (this.pauseRenderToggle != null)
				{
					this.pauseRenderToggle.isOn = this._pauseRender;
				}
				if (this._pauseRender)
				{
					this.pauseRenderFlag = new AsyncFlag("Global Pause Render");
					this.PauseRender(this.pauseRenderFlag);
				}
			}
		}
	}

	// Token: 0x06005CAD RID: 23725 RVA: 0x0022425C File Offset: 0x0022265C
	protected bool CheckHoldLoad()
	{
		if (this.holdLoadCompleteFlags == null)
		{
			this.holdLoadCompleteFlags = new List<AsyncFlag>();
		}
		if (this.holdLoadCompleteFlags.Count > 0)
		{
			if (this.removeHoldFlags == null)
			{
				this.removeHoldFlags = new List<AsyncFlag>();
			}
			else
			{
				this.removeHoldFlags.Clear();
			}
			foreach (AsyncFlag asyncFlag in this.holdLoadCompleteFlags)
			{
				if (asyncFlag.Raised)
				{
					this.removeHoldFlags.Add(asyncFlag);
				}
			}
			foreach (AsyncFlag item in this.removeHoldFlags)
			{
				this.holdLoadCompleteFlags.Remove(item);
			}
		}
		return this.holdLoadCompleteFlags.Count > 0;
	}

	// Token: 0x06005CAE RID: 23726 RVA: 0x0022437C File Offset: 0x0022277C
	public void HoldLoadComplete(AsyncFlag af)
	{
		if (this.holdLoadCompleteFlags == null)
		{
			this.holdLoadCompleteFlags = new List<AsyncFlag>();
		}
		this.holdLoadCompleteFlags.Add(af);
	}

	// Token: 0x06005CAF RID: 23727 RVA: 0x002243A0 File Offset: 0x002227A0
	public void SetLoadFlag()
	{
		if (this.loadFlag != null)
		{
			this.loadFlag.Raise();
		}
	}

	// Token: 0x06005CB0 RID: 23728 RVA: 0x002243B8 File Offset: 0x002227B8
	protected bool CheckLoadingIcon()
	{
		if (this.loadingIconFlags == null)
		{
			this.loadingIconFlags = new List<AsyncFlag>();
		}
		if (this.loadingIconFlags.Count > 0)
		{
			if (this.removeLoadFlags == null)
			{
				this.removeLoadFlags = new List<AsyncFlag>();
			}
			else
			{
				this.removeLoadFlags.Clear();
			}
			foreach (AsyncFlag asyncFlag in this.loadingIconFlags)
			{
				if (asyncFlag.Raised)
				{
					this.removeLoadFlags.Add(asyncFlag);
				}
			}
			foreach (AsyncFlag item in this.removeLoadFlags)
			{
				this.loadingIconFlags.Remove(item);
			}
		}
		if (this.loadingIconFlags.Count > 0)
		{
			if (this.loadingIcon != null)
			{
				this.loadingIcon.gameObject.SetActive(true);
			}
			return true;
		}
		if (this.loadingIcon != null)
		{
			this.loadingIcon.gameObject.SetActive(false);
		}
		return false;
	}

	// Token: 0x06005CB1 RID: 23729 RVA: 0x0022451C File Offset: 0x0022291C
	public void SetLoadingIconFlag(AsyncFlag af)
	{
		if (this.loadingIconFlags == null)
		{
			this.loadingIconFlags = new List<AsyncFlag>();
		}
		this.loadingIconFlags.Add(af);
	}

	// Token: 0x06005CB2 RID: 23730 RVA: 0x00224540 File Offset: 0x00222940
	protected void SyncAutoFreezeAnimationOnSwitchToEditMode(bool b)
	{
		this.autoFreezeAnimationOnSwitchToEditMode = b;
	}

	// Token: 0x17000D9F RID: 3487
	// (get) Token: 0x06005CB3 RID: 23731 RVA: 0x00224549 File Offset: 0x00222949
	// (set) Token: 0x06005CB4 RID: 23732 RVA: 0x00224554 File Offset: 0x00222954
	public bool autoFreezeAnimationOnSwitchToEditMode
	{
		get
		{
			return this._autoFreezeAnimationOnSwitchToEditMode;
		}
		set
		{
			if (this._autoFreezeAnimationOnSwitchToEditMode != value)
			{
				this._autoFreezeAnimationOnSwitchToEditMode = value;
				if (this.autoFreezeAnimationOnSwitchToEditModeToggle != null)
				{
					this.autoFreezeAnimationOnSwitchToEditModeToggle.isOn = value;
				}
				if (UserPreferences.singleton != null)
				{
					UserPreferences.singleton.SavePreferences();
				}
			}
		}
	}

	// Token: 0x06005CB5 RID: 23733 RVA: 0x002245AC File Offset: 0x002229AC
	protected void SyncFreezeAnimation()
	{
		if (this.allAnimators != null)
		{
			foreach (Animator animator in this.allAnimators)
			{
				animator.enabled = !this._freezeAnimation;
			}
		}
	}

	// Token: 0x06005CB6 RID: 23734 RVA: 0x0022461C File Offset: 0x00222A1C
	public void SetFreezeAnimation(bool freeze)
	{
		if (this._freezeAnimation != freeze)
		{
			this._freezeAnimation = freeze;
			this.SyncFreezeAnimation();
			if (this.freezeAnimationToggle != null)
			{
				this.freezeAnimationToggle.isOn = this._freezeAnimation;
			}
			if (this.freezeAnimationToggleAlt != null)
			{
				this.freezeAnimationToggleAlt.isOn = this._freezeAnimation;
			}
		}
	}

	// Token: 0x17000DA0 RID: 3488
	// (get) Token: 0x06005CB7 RID: 23735 RVA: 0x00224686 File Offset: 0x00222A86
	public bool freezeAnimation
	{
		get
		{
			return this._freezeAnimation || this._isLoading || this._resetSimulation || !this._autoSimulation;
		}
	}

	// Token: 0x06005CB8 RID: 23736 RVA: 0x002246B8 File Offset: 0x00222AB8
	public void SyncVersionText()
	{
		string text = "VaM: ";
		if (this.foundVersion)
		{
			text += this.resolvedVersion;
		}
		else
		{
			text += this.version;
		}
		if (this.vamXIntalled)
		{
			text = text + " vamX: 1." + this.vamXVersion;
		}
		if (this.versionText != null)
		{
			this.versionText.text = text;
		}
		if (GlobalSceneOptions.singleton != null && GlobalSceneOptions.singleton.versionText != null)
		{
			GlobalSceneOptions.singleton.versionText.text = text;
		}
	}

	// Token: 0x06005CB9 RID: 23737 RVA: 0x0022476C File Offset: 0x00222B6C
	protected void SyncVersion()
	{
		SettingsManager.APP_PATH = Path.GetFullPath(".");
		SettingsManager.RegeneratePaths();
		string version_FILE_LOCAL_PATH = SettingsManager.VERSION_FILE_LOCAL_PATH;
		this.foundVersion = false;
		if (File.Exists(version_FILE_LOCAL_PATH))
		{
			string cipherText = File.ReadAllText(SettingsManager.PATCH_VERSION_PATH).Replace("\n", string.Empty).Replace("\r", string.Empty);
			this.resolvedVersion = MHLab.PATCH.Utilities.Rijndael.Decrypt(cipherText, SettingsManager.PATCH_VERSION_ENCRYPTION_PASSWORD);
			this.foundVersion = true;
		}
		else if (Application.isEditor)
		{
			this.resolvedVersion = this.editorMimicVersion;
			this.foundVersion = true;
		}
		else
		{
			this.resolvedVersion = this.editorMimicVersion;
		}
		this.resolvedVersionDefines = null;
		string[] array = this.resolvedVersion.Split(new char[]
		{
			'.'
		});
		if (array.Length == 4)
		{
			this.resolvedVersionDefines = new List<string>();
			string text = array[0] + "_" + array[1];
			string item = "VAM_" + text;
			this.resolvedVersionDefines.Add(item);
			item = "VAM_" + text + "_" + array[2];
			this.resolvedVersionDefines.Add(item);
			item = string.Concat(new string[]
			{
				"VAM_",
				text,
				"_",
				array[2],
				"_",
				array[3]
			});
			this.resolvedVersionDefines.Add(item);
			int num;
			if (int.TryParse(array[1], out num))
			{
				for (int i = this.oldestMajorVersion; i <= num; i++)
				{
					item = string.Concat(new object[]
					{
						"VAM_GT_",
						array[0],
						"_",
						i
					});
					this.resolvedVersionDefines.Add(item);
				}
			}
			if (this.specificMilestoneVersionDefines != null)
			{
				foreach (string str in this.specificMilestoneVersionDefines)
				{
					item = "VAM_GT_" + str;
					this.resolvedVersionDefines.Add(item);
				}
			}
		}
		this.SyncVersionText();
	}

	// Token: 0x06005CBA RID: 23738 RVA: 0x00224993 File Offset: 0x00222D93
	public string GetVersion()
	{
		if (this.resolvedVersion != null)
		{
			return this.resolvedVersion;
		}
		return this.version;
	}

	// Token: 0x06005CBB RID: 23739 RVA: 0x002249AD File Offset: 0x00222DAD
	public IEnumerable<string> GetResolvedVersionDefines()
	{
		return this.resolvedVersionDefines;
	}

	// Token: 0x06005CBC RID: 23740 RVA: 0x002249B8 File Offset: 0x00222DB8
	private void SyncSelectAtomPopup()
	{
		if (this.selectAtomPopup != null && !this._isLoading && !this._pauseSyncAtomLists)
		{
			string text = string.Empty;
			if (this.selectedController != null && this.selectedController.containingAtom != null)
			{
				text = this.selectedController.containingAtom.uid;
			}
			bool flag = false;
			List<string> list = this.GetAtomUIDsWithFreeControllers();
			this.selectAtomPopup.currentValueNoCallback = "None";
			this.selectAtomPopup.numPopupValues = list.Count + 1;
			this.selectAtomPopup.setPopupValue(0, "None");
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i] == text)
				{
					flag = true;
					this.selectAtomPopup.currentValueNoCallback = text;
				}
				this.selectAtomPopup.setPopupValue(i + 1, list[i]);
			}
			if (!flag)
			{
				this.selectAtomPopup.currentValue = "None";
				this.SyncControllerPopup("None");
			}
		}
	}

	// Token: 0x06005CBD RID: 23741 RVA: 0x00224AD4 File Offset: 0x00222ED4
	public void SelectLastAddedAtom()
	{
		if (this.visibleAtomUIDsWithFreeControllers != null && this.visibleAtomUIDsWithFreeControllers.Count > 0 && this.selectAtomPopup != null)
		{
			this.selectAtomPopup.currentValue = this.visibleAtomUIDsWithFreeControllers[this.visibleAtomUIDsWithFreeControllers.Count - 1];
		}
	}

	// Token: 0x06005CBE RID: 23742 RVA: 0x00224B34 File Offset: 0x00222F34
	public void CycleSelectAtomOfType(string type)
	{
		if (this.visibleAtomUIDsWithFreeControllers != null)
		{
			List<string> list = this.visibleAtomUIDsWithFreeControllers;
			string text = null;
			if (this.lastCycleSelectAtomType != null && this.lastCycleSelectAtomType == type)
			{
				List<string> list2 = new List<string>();
				int num = 0;
				int num2 = -1;
				foreach (string text2 in list)
				{
					Atom atomByUid = this.GetAtomByUid(text2);
					if (atomByUid.type == type)
					{
						list2.Add(text2);
						if (this.lastCycleSelectAtomUid == text2)
						{
							num2 = num;
						}
						num++;
					}
				}
				if (num2 == -1)
				{
					if (list2.Count > 0)
					{
						text = list2[0];
					}
				}
				else if (num2 == list2.Count - 1)
				{
					text = list2[0];
				}
				else
				{
					text = list2[num2 + 1];
				}
			}
			else
			{
				foreach (string text3 in list)
				{
					Atom atomByUid2 = this.GetAtomByUid(text3);
					if (atomByUid2.type == type)
					{
						text = text3;
						break;
					}
				}
			}
			if (text != null)
			{
				this.lastCycleSelectAtomUid = text;
				this.lastCycleSelectAtomType = type;
				if (this.selectAtomPopup != null)
				{
					this.selectAtomPopup.currentValue = text;
				}
				List<string> freeControllerNamesInAtom = this.GetFreeControllerNamesInAtom(text);
				if (freeControllerNamesInAtom != null && freeControllerNamesInAtom.Count > 1)
				{
					this.selectControllerPopup.currentValue = freeControllerNamesInAtom[0];
				}
				this.activeUI = SuperController.ActiveUI.SelectedOptions;
			}
		}
	}

	// Token: 0x06005CBF RID: 23743 RVA: 0x00224D18 File Offset: 0x00223118
	private void SyncControllerPopup(string nv)
	{
		string currentValue = this.selectAtomPopup.currentValue;
		if (currentValue == "None")
		{
			this.selectControllerPopup.currentValueNoCallback = "None";
			this.selectControllerPopup.numPopupValues = 1;
			this.selectControllerPopup.setPopupValue(0, "None");
		}
		else
		{
			string a = string.Empty;
			if (this.selectedController != null && this.selectedController.containingAtom != null)
			{
				a = this.selectedController.containingAtom.uid;
				if (a != currentValue)
				{
					this.selectControllerPopup.currentValueNoCallback = "None";
				}
			}
			else
			{
				this.selectControllerPopup.currentValueNoCallback = "None";
			}
			List<string> freeControllerNamesInAtom = this.GetFreeControllerNamesInAtom(currentValue);
			if (freeControllerNamesInAtom == null || freeControllerNamesInAtom.Count == 0)
			{
				this.selectControllerPopup.numPopupValues = 1;
				this.selectControllerPopup.setPopupValue(0, "None");
			}
			else
			{
				this.selectControllerPopup.numPopupValues = freeControllerNamesInAtom.Count + 1;
				this.selectControllerPopup.setPopupValue(0, "None");
				for (int i = 0; i < freeControllerNamesInAtom.Count; i++)
				{
					this.selectControllerPopup.setPopupValue(i + 1, freeControllerNamesInAtom[i]);
				}
				if (freeControllerNamesInAtom.Count == 1)
				{
					this.selectControllerPopup.currentValue = freeControllerNamesInAtom[0];
				}
			}
		}
	}

	// Token: 0x06005CC0 RID: 23744 RVA: 0x00224E8C File Offset: 0x0022328C
	private void SyncUIToSelectedController()
	{
		if (this.selectedController == null)
		{
			if (this.selectedControllerNameDisplay != null)
			{
				this.selectedControllerNameDisplay.text = string.Empty;
			}
			if (this.selectAtomPopup != null)
			{
				this.selectAtomPopup.currentValue = "None";
			}
			if (this.selectControllerPopup != null)
			{
				this.selectControllerPopup.currentValueNoCallback = "None";
			}
		}
		else
		{
			Atom containingAtom = this.selectedController.containingAtom;
			if (containingAtom != null)
			{
				if (this.selectedControllerNameDisplay != null)
				{
					this.selectedControllerNameDisplay.text = this.selectedController.containingAtom.uid + ":" + this.selectedController.name;
				}
				if (this.selectAtomPopup != null)
				{
					this.selectAtomPopup.currentValue = containingAtom.uid;
				}
				if (this.selectControllerPopup != null)
				{
					this.selectControllerPopup.currentValueNoCallback = this.selectedController.name;
				}
			}
			else if (this.selectedControllerNameDisplay != null)
			{
				this.selectedControllerNameDisplay.text = this.selectedController.name;
			}
		}
	}

	// Token: 0x06005CC1 RID: 23745 RVA: 0x00224FE0 File Offset: 0x002233E0
	public void SetAlignRotationOffset(string type)
	{
		try
		{
			this.alignRotationOffset = (SuperController.AlignRotationOffset)System.Enum.Parse(typeof(SuperController.AlignRotationOffset), type);
		}
		catch (ArgumentException)
		{
			this.Error("Attempted to align rotation offset type to " + type + " which is not a valid type", true, true);
		}
	}

	// Token: 0x17000DA1 RID: 3489
	// (get) Token: 0x06005CC2 RID: 23746 RVA: 0x0022503C File Offset: 0x0022343C
	// (set) Token: 0x06005CC3 RID: 23747 RVA: 0x00225044 File Offset: 0x00223444
	public SuperController.AlignRotationOffset alignRotationOffset
	{
		get
		{
			return this._alignRotationOffset;
		}
		set
		{
			if (this._alignRotationOffset != value)
			{
				this._alignRotationOffset = value;
				if (this.alignRotationOffsetPopup != null)
				{
					this.alignRotationOffsetPopup.currentValue = this._alignRotationOffset.ToString();
				}
			}
		}
	}

	// Token: 0x06005CC4 RID: 23748 RVA: 0x00225094 File Offset: 0x00223494
	public void AlignRigFacingController(FreeControllerV3 controller, bool rotationOnly = false)
	{
		if (controller != null)
		{
			this.SetMonitorRigPositionZero();
			Transform transform = controller.focusPoint;
			if (transform == null)
			{
				transform = controller.transform;
			}
			Vector3 up = this.navigationRig.up;
			Vector3 toDirection = Vector3.ProjectOnPlane(transform.position - this.motionControllerHead.position, up);
			Vector3 fromDirection = Vector3.ProjectOnPlane(this.motionControllerHead.forward, up);
			Quaternion lhs = Quaternion.FromToRotation(fromDirection, toDirection);
			this.navigationRig.rotation = lhs * this.navigationRig.rotation;
			if (!rotationOnly)
			{
				toDirection = Vector3.ProjectOnPlane(transform.position - this.motionControllerHead.position, up);
				float magnitude = toDirection.magnitude;
				float num;
				if (this.MonitorRigActive)
				{
					num = 3f * this._worldScale;
				}
				else
				{
					num = 1.5f * this._worldScale;
				}
				if (Mathf.Approximately(magnitude, 0f))
				{
					this.navigationRig.position -= this.motionControllerHead.forward * num;
				}
				else
				{
					this.navigationRig.position -= toDirection.normalized * (num - magnitude);
				}
			}
			if (!this._mainHUDAnchoredOnMonitor)
			{
				if (this._alignRotationOffset == SuperController.AlignRotationOffset.Left)
				{
					this.navigationRig.Rotate(0f, 35f, 0f);
				}
				else if (this._alignRotationOffset == SuperController.AlignRotationOffset.Right)
				{
					this.navigationRig.Rotate(0f, -35f, 0f);
				}
			}
			this.SyncMonitorRigPosition();
		}
	}

	// Token: 0x06005CC5 RID: 23749 RVA: 0x0022524D File Offset: 0x0022364D
	public void AlignRigFacingSelectedController(bool rotationOnly = false)
	{
		this.AlignRigFacingController(this.selectedController, rotationOnly);
	}

	// Token: 0x06005CC6 RID: 23750 RVA: 0x0022525C File Offset: 0x0022365C
	protected void SyncWorldUIOverlaySky()
	{
		if (SkyshopLightController.singleton != null)
		{
			SkyshopLightController.singleton.overlaySkyActive = (this.worldUIActivated && this._worldUIShowOverlaySky);
		}
	}

	// Token: 0x17000DA2 RID: 3490
	// (get) Token: 0x06005CC7 RID: 23751 RVA: 0x0022528C File Offset: 0x0022368C
	// (set) Token: 0x06005CC8 RID: 23752 RVA: 0x00225294 File Offset: 0x00223694
	public bool worldUIShowOverlaySky
	{
		get
		{
			return this._worldUIShowOverlaySky;
		}
		set
		{
			if (this._worldUIShowOverlaySky != value)
			{
				this._worldUIShowOverlaySky = value;
				if (this.worldUIShowOverlaySkyToggle != null)
				{
					this.worldUIShowOverlaySkyToggle.isOn = this._worldUIShowOverlaySky;
				}
				this.SyncWorldUIOverlaySky();
			}
		}
	}

	// Token: 0x17000DA3 RID: 3491
	// (get) Token: 0x06005CC9 RID: 23753 RVA: 0x002252D1 File Offset: 0x002236D1
	// (set) Token: 0x06005CCA RID: 23754 RVA: 0x002252D9 File Offset: 0x002236D9
	public bool worldUIActivated
	{
		[CompilerGenerated]
		get
		{
			return this.<worldUIActivated>k__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			this.<worldUIActivated>k__BackingField = value;
		}
	}

	// Token: 0x06005CCB RID: 23755 RVA: 0x002252E4 File Offset: 0x002236E4
	public void ActivateWorldUI()
	{
		if (this.worldUI != null && !this.worldUIActivated)
		{
			this.worldUIActiveFlag = new AsyncFlag("World UI Active");
			this.PauseAutoSimulation(this.worldUIActiveFlag);
			this.PauseRender(this.worldUIActiveFlag);
			this.worldUI.gameObject.SetActive(true);
			this.worldUIActivated = true;
			if (this.MonitorCenterCamera != null)
			{
				this.MonitorCenterCamera.transform.localEulerAngles = Vector3.zero;
			}
			this.SyncMonitorCameraFOV();
			this.SyncWorldUIOverlaySky();
			this.SyncMonitorAuxUI();
		}
		this.HideMainHUD();
	}

	// Token: 0x06005CCC RID: 23756 RVA: 0x0022538C File Offset: 0x0022378C
	public void DeactivateWorldUI()
	{
		if (this.worldUI != null && this.worldUIActivated)
		{
			this.worldUIActiveFlag.Raise();
			this.worldUI.gameObject.SetActive(false);
			this.worldUIActivated = false;
			this.SyncMonitorCameraFOV();
			this.SyncWorldUIOverlaySky();
			this.SyncMonitorAuxUI();
			this.ShowMainHUDAuto();
		}
	}

	// Token: 0x06005CCD RID: 23757 RVA: 0x002253F0 File Offset: 0x002237F0
	public void CloseAllWorldUIPanels()
	{
		if (this.wizardWorldUI != null)
		{
			this.wizardWorldUI.gameObject.SetActive(false);
		}
		if (this.fileBrowserWorldUI != null)
		{
			this.fileBrowserWorldUI.Hide();
		}
		if (this.templatesFileBrowserWorldUI != null)
		{
			this.templatesFileBrowserWorldUI.Hide();
		}
		if (this.hubBrowser != null)
		{
			this.hubBrowser.Hide();
		}
		if (this.topWorldUI != null)
		{
			this.topWorldUI.gameObject.SetActive(false);
		}
	}

	// Token: 0x06005CCE RID: 23758 RVA: 0x00225498 File Offset: 0x00223898
	protected void OpenTopWorldUI()
	{
		if (this.topWorldUI != null)
		{
			this.ActivateWorldUI();
			this.topWorldUI.gameObject.SetActive(true);
		}
		if (this.hubBrowser != null)
		{
			this.hubBrowser.Hide();
		}
	}

	// Token: 0x06005CCF RID: 23759 RVA: 0x002254EC File Offset: 0x002238EC
	protected void SyncWorldUIAnchor()
	{
		if (this.worldUIAnchor != null)
		{
			Vector3 localPosition = this.worldUIAnchor.transform.localPosition;
			if (this.MonitorRigActive)
			{
				localPosition.y = this._worldUIMonitorAnchorHeight;
				localPosition.z = this._worldUIMonitorAnchorDistance;
			}
			else
			{
				localPosition.y = this._worldUIVRAnchorHeight;
				localPosition.z = this._worldUIVRAnchorDistance;
			}
			this.worldUIAnchor.transform.localPosition = localPosition;
		}
	}

	// Token: 0x06005CD0 RID: 23760 RVA: 0x00225570 File Offset: 0x00223970
	protected void SyncWorldUIVRAnchorDistance(float f)
	{
		this.worldUIVRAnchorDistance = f;
	}

	// Token: 0x17000DA4 RID: 3492
	// (get) Token: 0x06005CD1 RID: 23761 RVA: 0x00225579 File Offset: 0x00223979
	// (set) Token: 0x06005CD2 RID: 23762 RVA: 0x00225584 File Offset: 0x00223984
	public float worldUIVRAnchorDistance
	{
		get
		{
			return this._worldUIVRAnchorDistance;
		}
		set
		{
			if (this._worldUIVRAnchorDistance != value)
			{
				this._worldUIVRAnchorDistance = value;
				if (this.worldUIVRAnchorDistanceSlider != null)
				{
					this.worldUIVRAnchorDistanceSlider.value = this._worldUIVRAnchorDistance;
				}
				this.SyncWorldUIAnchor();
				if (UserPreferences.singleton != null)
				{
					UserPreferences.singleton.SavePreferences();
				}
			}
		}
	}

	// Token: 0x06005CD3 RID: 23763 RVA: 0x002255E6 File Offset: 0x002239E6
	protected void SyncWorldUIVRAnchorHeight(float f)
	{
		this.worldUIVRAnchorHeight = f;
	}

	// Token: 0x17000DA5 RID: 3493
	// (get) Token: 0x06005CD4 RID: 23764 RVA: 0x002255EF File Offset: 0x002239EF
	// (set) Token: 0x06005CD5 RID: 23765 RVA: 0x002255F8 File Offset: 0x002239F8
	public float worldUIVRAnchorHeight
	{
		get
		{
			return this._worldUIVRAnchorHeight;
		}
		set
		{
			if (this._worldUIVRAnchorHeight != value)
			{
				this._worldUIVRAnchorHeight = value;
				if (this.worldUIVRAnchorHeightSlider != null)
				{
					this.worldUIVRAnchorHeightSlider.value = this._worldUIVRAnchorHeight;
				}
				this.SyncWorldUIAnchor();
				if (UserPreferences.singleton != null)
				{
					UserPreferences.singleton.SavePreferences();
				}
			}
		}
	}

	// Token: 0x17000DA6 RID: 3494
	// (get) Token: 0x06005CD6 RID: 23766 RVA: 0x0022565A File Offset: 0x00223A5A
	public SuperController.ActiveUI lastActiveUI
	{
		get
		{
			return this._lastActiveUI;
		}
	}

	// Token: 0x06005CD7 RID: 23767 RVA: 0x00225664 File Offset: 0x00223A64
	public void SetCustomUI(Transform cui)
	{
		if (this.customUI != null)
		{
			this.customUI.gameObject.SetActive(false);
		}
		if (!this.customUIDisabled)
		{
			this.customUI = cui;
		}
		else
		{
			this.customUI = this.alternateCustomUI;
		}
		this.activeUI = SuperController.ActiveUI.Custom;
	}

	// Token: 0x06005CD8 RID: 23768 RVA: 0x002256BE File Offset: 0x00223ABE
	public void SetToLastActiveUI()
	{
		this.activeUI = this._lastActiveUI;
	}

	// Token: 0x06005CD9 RID: 23769 RVA: 0x002256CC File Offset: 0x00223ACC
	private void ClearAllUI()
	{
		if (this.mainMenuUI != null)
		{
			this.mainMenuUI.gameObject.SetActive(false);
		}
		if (this.selectedController != null)
		{
			this.selectedController.guihidden = true;
		}
		if (this.multiButtonPanel != null)
		{
			this.multiButtonPanel.gameObject.SetActive(false);
		}
		if (this.sceneControlUI != null)
		{
			this.sceneControlUI.gameObject.SetActive(true);
		}
		if (this.embeddedSceneUI != null)
		{
			this.embeddedSceneUI.gameObject.SetActive(false);
		}
		if (this.customUI != null)
		{
			this.customUI.gameObject.SetActive(false);
		}
		if (this.onlineBrowserUI != null)
		{
			this.onlineBrowserUI.gameObject.SetActive(false);
		}
		if (this.packageBuilderUI != null)
		{
			this.packageBuilderUI.gameObject.SetActive(false);
		}
		if (this.packageManagerUI != null)
		{
			this.packageManagerUI.gameObject.SetActive(false);
		}
		if (this.packageDownloader != null)
		{
			this.packageDownloader.ClosePanel();
		}
		if (this.fileBrowserUI != null)
		{
			this.fileBrowserUI.Hide();
		}
		if (this.mediaFileBrowserUI != null)
		{
			this.mediaFileBrowserUI.Hide();
		}
		if (this.directoryBrowserUI != null)
		{
			this.directoryBrowserUI.Hide();
		}
	}

	// Token: 0x06005CDA RID: 23770 RVA: 0x00225878 File Offset: 0x00223C78
	private void SyncActiveUI()
	{
		switch (this._activeUI)
		{
		case SuperController.ActiveUI.MainMenu:
			if (this.mainMenuUI != null)
			{
				this.mainMenuUI.gameObject.SetActive(true);
			}
			break;
		case SuperController.ActiveUI.MainMenuOnly:
			if (this.mainMenuUI != null)
			{
				this.mainMenuUI.gameObject.SetActive(true);
			}
			if (this.sceneControlUI != null)
			{
				this.sceneControlUI.gameObject.SetActive(false);
			}
			if (this.sceneControlUIAlt != null)
			{
				this.sceneControlUIAlt.gameObject.SetActive(false);
			}
			break;
		case SuperController.ActiveUI.SelectedOptions:
			if (this.selectedController != null && this._mainHUDVisible)
			{
				this.selectedController.guihidden = false;
			}
			break;
		case SuperController.ActiveUI.MultiButtonPanel:
			if (this.multiButtonPanel != null)
			{
				this.multiButtonPanel.gameObject.SetActive(true);
			}
			break;
		case SuperController.ActiveUI.EmbeddedScenePanel:
			if (this.embeddedSceneUI != null)
			{
				this.embeddedSceneUI.gameObject.SetActive(true);
			}
			break;
		case SuperController.ActiveUI.OnlineBrowser:
			if (UserPreferences.singleton == null || UserPreferences.singleton.enableWebBrowser)
			{
				if (this.onlineBrowserUI != null)
				{
					this.onlineBrowserUI.gameObject.SetActive(true);
				}
			}
			else
			{
				this.Error("Web Browsing is disabled. To use this feature you must enable browser in User Preferences -> Security tab", true, true);
				this.activeUI = SuperController.ActiveUI.MainMenu;
				this.SetMainMenuTab("TabUserPrefs");
				this.SetUserPrefsTab("TabSecurity");
			}
			break;
		case SuperController.ActiveUI.PackageBuilder:
			if (this.packageBuilderUI != null)
			{
				this.packageBuilderUI.gameObject.SetActive(true);
			}
			break;
		case SuperController.ActiveUI.PackageManager:
			if (this.packageManagerUI != null)
			{
				this.packageManagerUI.gameObject.SetActive(true);
			}
			break;
		case SuperController.ActiveUI.PackageDownloader:
			if (this.packageDownloader != null)
			{
				this.packageDownloader.OpenPanel();
			}
			break;
		case SuperController.ActiveUI.Custom:
			if (this.customUI != null)
			{
				this.customUI.gameObject.SetActive(true);
			}
			break;
		}
		this.SyncVisibility();
	}

	// Token: 0x17000DA7 RID: 3495
	// (get) Token: 0x06005CDB RID: 23771 RVA: 0x00225AE6 File Offset: 0x00223EE6
	// (set) Token: 0x06005CDC RID: 23772 RVA: 0x00225AEE File Offset: 0x00223EEE
	public SuperController.ActiveUI activeUI
	{
		get
		{
			return this._activeUI;
		}
		set
		{
			if (this._activeUI != value)
			{
				this._lastActiveUI = this._activeUI;
				this._activeUI = value;
			}
			this.ClearAllUI();
			this.SyncActiveUI();
		}
	}

	// Token: 0x06005CDD RID: 23773 RVA: 0x00225B1C File Offset: 0x00223F1C
	public void SetActiveUI(string uiName)
	{
		try
		{
			this.activeUI = (SuperController.ActiveUI)System.Enum.Parse(typeof(SuperController.ActiveUI), uiName);
		}
		catch (ArgumentException)
		{
			this.Error("Attempted to set UI to " + uiName + " which is not a valid UI name", true, true);
		}
	}

	// Token: 0x06005CDE RID: 23774 RVA: 0x00225B78 File Offset: 0x00223F78
	public void SetMainMenuTab(string tabName)
	{
		if (this.mainMenuTabSelector != null)
		{
			this.mainMenuTabSelector.SetActiveTab(tabName);
		}
	}

	// Token: 0x06005CDF RID: 23775 RVA: 0x00225B97 File Offset: 0x00223F97
	public void SetUserPrefsTab(string tabName)
	{
		if (this.userPrefsTabSelector != null)
		{
			this.userPrefsTabSelector.SetActiveTab(tabName);
		}
	}

	// Token: 0x06005CE0 RID: 23776 RVA: 0x00225BB8 File Offset: 0x00223FB8
	private void InitUI()
	{
		if (this.MonitorModeAuxUI != null)
		{
			if (this.isMonitorOnly && !this.UIDisabled)
			{
				this.MonitorModeAuxUI.gameObject.SetActive(true);
			}
			else
			{
				this.MonitorModeAuxUI.gameObject.SetActive(false);
			}
		}
		if (this.loadingUI != null)
		{
			this.loadingUI.gameObject.SetActive(false);
		}
		if (this.loadingUIAlt != null)
		{
			this.loadingUIAlt.gameObject.SetActive(false);
		}
		if (this.loadingGeometry != null)
		{
			this.loadingGeometry.gameObject.SetActive(false);
		}
		if (this.mainHUDAttachPoint != null)
		{
			this.mainHUDAttachPointStartingPosition = this.mainHUDAttachPoint.localPosition;
			this.mainHUDAttachPointStartingRotation = this.mainHUDAttachPoint.localRotation;
		}
		this.activeUI = SuperController.ActiveUI.SelectedOptions;
		if (this.selectionHUDTransform != null)
		{
			this.selectionHUD = this.selectionHUDTransform.GetComponent<SelectionHUD>();
			this.SetSelectionHUDHeader(this.selectionHUD, "Highlighted Controllers");
		}
		if (this.rightSelectionHUDTransform != null)
		{
			this.rightSelectionHUD = this.rightSelectionHUDTransform.GetComponent<SelectionHUD>();
			this.SetSelectionHUDHeader(this.rightSelectionHUD, string.Empty);
		}
		if (this.leftSelectionHUDTransform != null)
		{
			this.leftSelectionHUD = this.leftSelectionHUDTransform.GetComponent<SelectionHUD>();
			this.SetSelectionHUDHeader(this.leftSelectionHUD, string.Empty);
		}
		if (this.worldScaleSlider != null)
		{
			this.worldScaleSlider.minValue = 0.1f;
			this.worldScaleSlider.maxValue = 40f;
			this.worldScaleSlider.value = this._worldScale;
			this.worldScaleSlider.onValueChanged.AddListener(new UnityAction<float>(this.<InitUI>m__0));
			SliderControl component = this.worldScaleSlider.GetComponent<SliderControl>();
			if (component != null)
			{
				component.defaultValue = 1f;
			}
		}
		if (this.worldScaleSliderAlt != null)
		{
			this.worldScaleSliderAlt.minValue = 0.1f;
			this.worldScaleSliderAlt.maxValue = 40f;
			this.worldScaleSliderAlt.value = this._worldScale;
			this.worldScaleSliderAlt.onValueChanged.AddListener(new UnityAction<float>(this.<InitUI>m__1));
			SliderControl component2 = this.worldScaleSliderAlt.GetComponent<SliderControl>();
			if (component2 != null)
			{
				component2.defaultValue = 1f;
			}
		}
		if (this.controllerScaleSlider != null)
		{
			this.controllerScaleSlider.value = this._controllerScale;
			this.controllerScaleSlider.onValueChanged.AddListener(new UnityAction<float>(this.<InitUI>m__2));
			SliderControl component3 = this.controllerScaleSlider.GetComponent<SliderControl>();
			if (component3 != null)
			{
				component3.defaultValue = 1f;
			}
		}
		if (this.playerHeightAdjustSlider != null)
		{
			this.playerHeightAdjustSlider.value = this._playerHeightAdjust;
			this.playerHeightAdjustSlider.onValueChanged.AddListener(new UnityAction<float>(this.<InitUI>m__3));
			SliderControl component4 = this.playerHeightAdjustSlider.GetComponent<SliderControl>();
			if (component4 != null)
			{
				component4.defaultValue = 0f;
			}
		}
		if (this.playerHeightAdjustSliderAlt != null)
		{
			this.playerHeightAdjustSliderAlt.value = this._playerHeightAdjust;
			this.playerHeightAdjustSliderAlt.onValueChanged.AddListener(new UnityAction<float>(this.<InitUI>m__4));
			SliderControl component5 = this.playerHeightAdjustSliderAlt.GetComponent<SliderControl>();
			if (component5 != null)
			{
				component5.defaultValue = 0f;
			}
		}
		if (this.monitorUIScaleSlider != null)
		{
			this.monitorUIScaleSlider.value = this._monitorUIScale;
			this.monitorUIScaleSlider.onValueChanged.AddListener(new UnityAction<float>(this.<InitUI>m__5));
			SliderControl component6 = this.monitorUIScaleSlider.GetComponent<SliderControl>();
			if (component6 != null)
			{
				component6.defaultValue = 1f;
			}
		}
		if (this.monitorUIYOffsetSlider != null)
		{
			this.monitorUIYOffsetSlider.value = this._monitorUIYOffset;
			this.monitorUIYOffsetSlider.onValueChanged.AddListener(new UnityAction<float>(this.<InitUI>m__6));
			SliderControl component7 = this.monitorUIYOffsetSlider.GetComponent<SliderControl>();
			if (component7 != null)
			{
				component7.defaultValue = 0f;
			}
		}
		if (this.monitorCameraFOVSlider != null)
		{
			this.monitorCameraFOVSlider.value = this._monitorCameraFOV;
			this.monitorCameraFOVSlider.onValueChanged.AddListener(new UnityAction<float>(this.<InitUI>m__7));
			SliderControl component8 = this.monitorCameraFOVSlider.GetComponent<SliderControl>();
			if (component8 != null)
			{
				component8.defaultValue = 40f;
			}
		}
		if (this.editModeToggle != null)
		{
			this.editModeToggle.isOn = (this._gameMode == SuperController.GameMode.Edit);
			this.editModeToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__8));
		}
		if (this.playModeToggle != null)
		{
			this.playModeToggle.isOn = (this._gameMode == SuperController.GameMode.Play);
			this.playModeToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__9));
		}
		if (this.selectedControllerNameDisplay != null)
		{
			this.selectedControllerNameDisplay.text = string.Empty;
		}
		if (this.rayLineMaterialLeft != null)
		{
			this.rayLineDrawerLeft = new LineDrawer(this.rayLineMaterialLeft);
		}
		if (this.rayLineMaterialRight != null)
		{
			this.rayLineDrawerRight = new LineDrawer(this.rayLineMaterialRight);
		}
		if (this.twoStageLineMaterial != null)
		{
			this.rightTwoStageLineDrawer = new LineDrawer(this.twoStageLineMaterial);
			this.leftTwoStageLineDrawer = new LineDrawer(this.twoStageLineMaterial);
			this.headTwoStageLineDrawer = new LineDrawer(this.twoStageLineMaterial);
			this.leapRightTwoStageLineDrawer = new LineDrawer(this.twoStageLineMaterial);
			this.leapLeftTwoStageLineDrawer = new LineDrawer(this.twoStageLineMaterial);
			this.tracker1TwoStageLineDrawer = new LineDrawer(this.twoStageLineMaterial);
			this.tracker2TwoStageLineDrawer = new LineDrawer(this.twoStageLineMaterial);
			this.tracker3TwoStageLineDrawer = new LineDrawer(this.twoStageLineMaterial);
			this.tracker4TwoStageLineDrawer = new LineDrawer(this.twoStageLineMaterial);
			this.tracker5TwoStageLineDrawer = new LineDrawer(this.twoStageLineMaterial);
			this.tracker6TwoStageLineDrawer = new LineDrawer(this.twoStageLineMaterial);
			this.tracker7TwoStageLineDrawer = new LineDrawer(this.twoStageLineMaterial);
			this.tracker8TwoStageLineDrawer = new LineDrawer(this.twoStageLineMaterial);
		}
		if (this.UISidePopup != null)
		{
			this.UISidePopup.currentValueNoCallback = this._UISide.ToString();
			UIPopup uisidePopup = this.UISidePopup;
			uisidePopup.onValueChangeHandlers = (UIPopup.OnValueChange)System.Delegate.Combine(uisidePopup.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetUISide));
		}
		if (this.helpToggle != null)
		{
			this.helpToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__A));
		}
		if (this.helpToggleAlt != null)
		{
			this.helpToggleAlt.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__B));
		}
		this.SyncHelpOverlay();
		if (this.lockHeightDuringNavigateToggle != null)
		{
			this.lockHeightDuringNavigateToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__C));
		}
		if (this.lockHeightDuringNavigateToggleAlt != null)
		{
			this.lockHeightDuringNavigateToggleAlt.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__D));
		}
		this.SyncLockHeightDuringNavigate();
		if (this.freeMoveFollowFloorToggle != null)
		{
			this.freeMoveFollowFloorToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__E));
		}
		if (this.freeMoveFollowFloorToggleAlt != null)
		{
			this.freeMoveFollowFloorToggleAlt.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__F));
		}
		this.SyncFreeMoveFollowFloor();
		if (this.disableAllNavigationToggle != null)
		{
			this.disableAllNavigationToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__10));
		}
		this.SyncDisableAllNavigation();
		if (this.disableGrabNavigationToggle != null)
		{
			this.disableGrabNavigationToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__11));
		}
		this.SyncDisableGrabNavigation();
		if (this.disableTeleportToggle != null)
		{
			this.disableTeleportToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__12));
		}
		this.SyncDisableTeleport();
		if (this.disableTeleportDuringPossessToggle != null)
		{
			this.disableTeleportDuringPossessToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__13));
		}
		this.SyncDisableTeleportDuringPossess();
		if (this.teleportAllowRotationToggle != null)
		{
			this.teleportAllowRotationToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__14));
		}
		this.SyncTeleportAllowRotation();
		if (this.freeMoveMultiplierSlider != null)
		{
			this.freeMoveMultiplierSlider.onValueChanged.AddListener(new UnityAction<float>(this.<InitUI>m__15));
		}
		if (this.grabNavigationPositionMultiplierSlider != null)
		{
			this.grabNavigationPositionMultiplierSlider.onValueChanged.AddListener(new UnityAction<float>(this.<InitUI>m__16));
		}
		this.SyncGrabNavigationPositionMultiplier();
		if (this.grabNavigationRotationMultiplierSlider != null)
		{
			this.grabNavigationRotationMultiplierSlider.onValueChanged.AddListener(new UnityAction<float>(this.<InitUI>m__17));
		}
		this.SyncGrabNavigationRotationMultiplier();
		this.SyncUIToUnlockLevel();
		if (this.showNavigationHologridToggle != null)
		{
			this.showNavigationHologridToggle.isOn = this._showNavigationHologrid;
			this.showNavigationHologridToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__18));
		}
		this.SyncHologridTransparency();
		if (this.hologridTransparencySlider != null)
		{
			this.hologridTransparencySlider.value = this._hologridTransparency;
			this.hologridTransparencySlider.onValueChanged.AddListener(new UnityAction<float>(this.<InitUI>m__19));
			SliderControl component9 = this.hologridTransparencySlider.GetComponent<SliderControl>();
			if (component9 != null)
			{
				component9.defaultValue = 0.01f;
			}
		}
		if (this.oculusThumbstickFunctionPopup != null)
		{
			this.oculusThumbstickFunctionPopup.currentValue = this._oculusThumbstickFunction.ToString();
			UIPopup uipopup = this.oculusThumbstickFunctionPopup;
			uipopup.onValueChangeHandlers = (UIPopup.OnValueChange)System.Delegate.Combine(uipopup.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetOculusThumbstickFunctionFromString));
		}
		if (this.allowHeadPossessMousePanAndZoomToggle != null)
		{
			this.allowHeadPossessMousePanAndZoomToggle.isOn = this._allowHeadPossessMousePanAndZoom;
			this.allowHeadPossessMousePanAndZoomToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__1A));
		}
		if (this.allowPossessSpringAdjustmentToggle != null)
		{
			this.allowPossessSpringAdjustmentToggle.isOn = this._allowPossessSpringAdjustment;
			this.allowPossessSpringAdjustmentToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__1B));
		}
		if (this.possessPositionSpringSlider != null)
		{
			this.possessPositionSpringSlider.value = this._possessPositionSpring;
			this.possessPositionSpringSlider.onValueChanged.AddListener(new UnityAction<float>(this.<InitUI>m__1C));
			SliderControl component10 = this.possessPositionSpringSlider.GetComponent<SliderControl>();
			if (component10 != null)
			{
				component10.defaultValue = 10000f;
			}
		}
		if (this.possessRotationSpringSlider != null)
		{
			this.possessRotationSpringSlider.value = this._possessRotationSpring;
			this.possessRotationSpringSlider.onValueChanged.AddListener(new UnityAction<float>(this.<InitUI>m__1D));
			SliderControl component11 = this.possessRotationSpringSlider.GetComponent<SliderControl>();
			if (component11 != null)
			{
				component11.defaultValue = 1000f;
			}
		}
		if (this.generateDepthTextureToggle != null)
		{
			this.generateDepthTextureToggle.isOn = this._generateDepthTexture;
			this.generateDepthTextureToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__1E));
		}
		if (this.useMonitorRigAudioListenerWhenActiveToggle != null)
		{
			this.useMonitorRigAudioListenerWhenActiveToggle.isOn = this._useMonitorRigAudioListenerWhenActive;
			this.useMonitorRigAudioListenerWhenActiveToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__1F));
		}
		if (this.navigationHologrid != null)
		{
			this.navigationHologrid.gameObject.SetActive(false);
		}
		if (this.showMainHUDOnStart && !this.UIDisabled && !this._onStartupSkipStartScreen)
		{
			this.OpenTopWorldUI();
			this.ShowMainHUDAuto();
			this.HideMainHUD();
		}
		else
		{
			this.HideMainHUD();
		}
		this.helpText = string.Empty;
		this.helpColor = Color.white;
		if (this.loResScreenShotCameraFOVSlider != null)
		{
			this.loResScreenShotCameraFOVSlider.value = this._loResScreenShotCameraFOV;
			this.loResScreenShotCameraFOVSlider.onValueChanged.AddListener(new UnityAction<float>(this.<InitUI>m__20));
			SliderControl component12 = this.loResScreenShotCameraFOVSlider.GetComponent<SliderControl>();
			if (component12 != null)
			{
				component12.defaultValue = 40f;
			}
		}
		if (this.hiResScreenShotCameraFOVSlider != null)
		{
			this.hiResScreenShotCameraFOVSlider.value = this._hiResScreenShotCameraFOV;
			this.hiResScreenShotCameraFOVSlider.onValueChanged.AddListener(new UnityAction<float>(this.<InitUI>m__21));
			SliderControl component13 = this.hiResScreenShotCameraFOVSlider.GetComponent<SliderControl>();
			if (component13 != null)
			{
				component13.defaultValue = 40f;
			}
		}
		if (this.selectAtomPopup != null)
		{
			this.selectAtomPopup.currentValue = "None";
			UIPopup uipopup2 = this.selectAtomPopup;
			uipopup2.onValueChangeHandlers = (UIPopup.OnValueChange)System.Delegate.Combine(uipopup2.onValueChangeHandlers, new UIPopup.OnValueChange(this.SyncControllerPopup));
		}
		if (this.selectControllerPopup != null)
		{
			this.selectControllerPopup.currentValue = "None";
			UIPopup uipopup3 = this.selectControllerPopup;
			uipopup3.onValueChangeHandlers = (UIPopup.OnValueChange)System.Delegate.Combine(uipopup3.onValueChangeHandlers, new UIPopup.OnValueChange(this.SelectFreeController));
		}
		if (this.alignRotationOffsetPopup != null)
		{
			this.alignRotationOffsetPopup.currentValue = this._alignRotationOffset.ToString();
			UIPopup uipopup4 = this.alignRotationOffsetPopup;
			uipopup4.onValueChangeHandlers = (UIPopup.OnValueChange)System.Delegate.Combine(uipopup4.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetAlignRotationOffset));
		}
		if (this.worldUIShowOverlaySkyToggle != null)
		{
			this.worldUIShowOverlaySkyToggle.isOn = this._worldUIShowOverlaySky;
			this.worldUIShowOverlaySkyToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__22));
		}
		if (this.worldUIVRAnchorDistanceSlider != null)
		{
			this.worldUIVRAnchorDistanceSlider.value = this._worldUIVRAnchorDistance;
			this.worldUIVRAnchorDistanceSlider.onValueChanged.AddListener(new UnityAction<float>(this.SyncWorldUIVRAnchorDistance));
		}
		if (this.worldUIVRAnchorHeightSlider != null)
		{
			this.worldUIVRAnchorHeightSlider.value = this._worldUIVRAnchorHeight;
			this.worldUIVRAnchorHeightSlider.onValueChanged.AddListener(new UnityAction<float>(this.SyncWorldUIVRAnchorHeight));
		}
		if (this.pauseAutoSimulationToggle != null)
		{
			this.pauseAutoSimulationToggle.isOn = this._pauseAutoSimulation;
			this.pauseAutoSimulationToggle.onValueChanged.AddListener(new UnityAction<bool>(this.SetPauseAutoSimulation));
		}
		if (this.pauseRenderToggle != null)
		{
			this.pauseRenderToggle.isOn = this._pauseRender;
			this.pauseRenderToggle.onValueChanged.AddListener(new UnityAction<bool>(this.SetPauseRender));
		}
		if (this.autoFreezeAnimationOnSwitchToEditModeToggle != null)
		{
			this.autoFreezeAnimationOnSwitchToEditModeToggle.isOn = this._autoFreezeAnimationOnSwitchToEditMode;
			this.autoFreezeAnimationOnSwitchToEditModeToggle.onValueChanged.AddListener(new UnityAction<bool>(this.SyncAutoFreezeAnimationOnSwitchToEditMode));
		}
		if (this.freezeAnimationToggle != null)
		{
			this.freezeAnimationToggle.onValueChanged.AddListener(new UnityAction<bool>(this.SetFreezeAnimation));
		}
		if (this.useSceneLoadPositionToggle != null)
		{
			this.useSceneLoadPositionToggle.isOn = this._useSceneLoadPosition;
			this.useSceneLoadPositionToggle.onValueChanged.AddListener(new UnityAction<bool>(this.SetUseSceneLoadPosition));
		}
		if (this.showHiddenAtomsToggle != null)
		{
			this.showHiddenAtomsToggle.isOn = this._showHiddenAtoms;
			this.showHiddenAtomsToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__23));
		}
		if (this.showHiddenAtomsToggleAlt != null)
		{
			this.showHiddenAtomsToggleAlt.isOn = this._showHiddenAtoms;
			this.showHiddenAtomsToggleAlt.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__24));
		}
		if (this.allowGrabPlusTriggerHandToggleToggle != null)
		{
			this.allowGrabPlusTriggerHandToggleToggle.isOn = this._allowGrabPlusTriggerHandToggle;
			this.allowGrabPlusTriggerHandToggleToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__25));
		}
		if (this.alwaysUseAlternateHandsToggle != null)
		{
			this.alwaysUseAlternateHandsToggle.isOn = this._alwaysUseAlternateHands;
			this.alwaysUseAlternateHandsToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__26));
		}
		if (this.useLegacyWorldScaleChangeToggle != null)
		{
			this.useLegacyWorldScaleChangeToggle.isOn = this._useLegacyWorldScaleChange;
			this.useLegacyWorldScaleChangeToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__27));
		}
		if (this.disableRenderForAtomsNotInIsolatedSubSceneToggle != null)
		{
			this.disableRenderForAtomsNotInIsolatedSubSceneToggle.isOn = this._disableRenderForAtomsNotInIsolatedSubScene;
			this.disableRenderForAtomsNotInIsolatedSubSceneToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__28));
		}
		if (this.freezePhysicsForAtomsNotInIsolatedSubSceneToggle != null)
		{
			this.freezePhysicsForAtomsNotInIsolatedSubSceneToggle.isOn = this._freezePhysicsForAtomsNotInIsolatedSubScene;
			this.freezePhysicsForAtomsNotInIsolatedSubSceneToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__29));
		}
		if (this.disableCollisionForAtomsNotInIsolatedSubSceneToggle != null)
		{
			this.disableCollisionForAtomsNotInIsolatedSubSceneToggle.isOn = this._disableCollisionForAtomsNotInIsolatedSubScene;
			this.disableCollisionForAtomsNotInIsolatedSubSceneToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__2A));
		}
		if (this.endIsolateEditSubSceneButton != null)
		{
			this.endIsolateEditSubSceneButton.onClick.AddListener(new UnityAction(this.EndIsolateEditSubScene));
		}
		if (this.quickSaveIsolatedSubSceneButton != null)
		{
			this.quickSaveIsolatedSubSceneButton.onClick.AddListener(new UnityAction(this.QuickSaveIsolatedSubScene));
		}
		if (this.quickReloadIsolatedSubSceneButton != null)
		{
			this.quickReloadIsolatedSubSceneButton.onClick.AddListener(new UnityAction(this.QuickReloadIsolatedSubScene));
		}
		if (this.selectIsolatedSubSceneButton != null)
		{
			this.selectIsolatedSubSceneButton.onClick.AddListener(new UnityAction(this.SelectIsolatedSubScene));
		}
		if (this.disableRenderForAtomsNotInIsolatedAtomToggle != null)
		{
			this.disableRenderForAtomsNotInIsolatedAtomToggle.isOn = this._disableRenderForAtomsNotInIsolatedAtom;
			this.disableRenderForAtomsNotInIsolatedAtomToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__2B));
		}
		if (this.freezePhysicsForAtomsNotInIsolatedAtomToggle != null)
		{
			this.freezePhysicsForAtomsNotInIsolatedAtomToggle.isOn = this._freezePhysicsForAtomsNotInIsolatedAtom;
			this.freezePhysicsForAtomsNotInIsolatedAtomToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__2C));
		}
		if (this.disableCollisionForAtomsNotInIsolatedAtomToggle != null)
		{
			this.disableCollisionForAtomsNotInIsolatedAtomToggle.isOn = this._disableCollisionForAtomsNotInIsolatedAtom;
			this.disableCollisionForAtomsNotInIsolatedAtomToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__2D));
		}
		if (this.endIsolateEditAtomButton != null)
		{
			this.endIsolateEditAtomButton.onClick.AddListener(new UnityAction(this.EndIsolateEditAtom));
		}
		if (this.selectIsolatedAtomButton != null)
		{
			this.selectIsolatedAtomButton.onClick.AddListener(new UnityAction(this.SelectIsolatedAtom));
		}
		if (this.keyInputFieldAction != null)
		{
			InputFieldAction inputFieldAction = this.keyInputFieldAction;
			inputFieldAction.onSubmitHandlers = (InputFieldAction.OnSubmit)System.Delegate.Combine(inputFieldAction.onSubmitHandlers, new InputFieldAction.OnSubmit(this.AddKey));
		}
		if (this.onStartupSkipStartScreenToggle != null)
		{
			this.onStartupSkipStartScreenToggle.isOn = this._onStartupSkipStartScreen;
			this.onStartupSkipStartScreenToggle.onValueChanged.AddListener(new UnityAction<bool>(this.SyncOnStartupSkipStartScreen));
		}
		if (this.leapMotionEnabledToggle != null)
		{
			this.leapMotionEnabledToggle.isOn = this._leapMotionEnabled;
			this.leapMotionEnabledToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__2E));
		}
		if (this.openMainHUDOnErrorToggle != null)
		{
			this.openMainHUDOnErrorToggle.isOn = this._openMainHUDOnError;
			this.openMainHUDOnErrorToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitUI>m__2F));
		}
	}

	// Token: 0x17000DA8 RID: 3496
	// (get) Token: 0x06005CE1 RID: 23777 RVA: 0x002270E8 File Offset: 0x002254E8
	// (set) Token: 0x06005CE2 RID: 23778 RVA: 0x002270F0 File Offset: 0x002254F0
	public bool helpOverlayOn
	{
		get
		{
			return this._helpOverlayOn;
		}
		set
		{
			if (this._helpOverlayOn != value)
			{
				this._helpOverlayOn = value;
				this.SyncHelpOverlay();
				if (UserPreferences.singleton != null)
				{
					UserPreferences.singleton.SavePreferences();
				}
			}
		}
	}

	// Token: 0x06005CE3 RID: 23779 RVA: 0x00227128 File Offset: 0x00225528
	private void SyncHelpOverlay()
	{
		if (this.helpToggle != null)
		{
			this.helpToggle.isOn = this._helpOverlayOn;
		}
		if (this.helpToggleAlt != null)
		{
			this.helpToggleAlt.isOn = this._helpOverlayOn;
		}
		if (this._helpOverlayOn && this._helpOverlayOnAux)
		{
			if (this.isOVR)
			{
				if (this.helpOverlayOVR != null)
				{
					this.helpOverlayOVR.gameObject.SetActive(true);
				}
				if (this.helpOverlayVive != null)
				{
					this.helpOverlayVive.gameObject.SetActive(false);
				}
			}
			else if (this.isOpenVR)
			{
				if (this.helpOverlayOVR != null)
				{
					this.helpOverlayOVR.gameObject.SetActive(false);
				}
				if (this.helpOverlayVive != null)
				{
					this.helpOverlayVive.gameObject.SetActive(true);
				}
			}
			else
			{
				if (this.helpOverlayOVR != null)
				{
					this.helpOverlayOVR.gameObject.SetActive(false);
				}
				if (this.helpOverlayVive != null)
				{
					this.helpOverlayVive.gameObject.SetActive(false);
				}
			}
		}
		else
		{
			if (this.helpOverlayOVR != null)
			{
				this.helpOverlayOVR.gameObject.SetActive(false);
			}
			if (this.helpOverlayVive != null)
			{
				this.helpOverlayVive.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06005CE4 RID: 23780 RVA: 0x002272C4 File Offset: 0x002256C4
	protected void SyncHelpText()
	{
		if (this.helpHUDText != null)
		{
			if (this.tempHelpText != null)
			{
				this.helpHUDText.text = this.tempHelpText;
				this.helpHUDText.color = Color.white;
			}
			else
			{
				this.helpHUDText.text = this._helpText;
				this.helpHUDText.color = this._helpColor;
			}
		}
	}

	// Token: 0x17000DA9 RID: 3497
	// (get) Token: 0x06005CE5 RID: 23781 RVA: 0x00227335 File Offset: 0x00225735
	// (set) Token: 0x06005CE6 RID: 23782 RVA: 0x0022733D File Offset: 0x0022573D
	public string helpText
	{
		get
		{
			return this._helpText;
		}
		set
		{
			if (this._helpText != value)
			{
				this._helpText = value;
				this.SyncHelpText();
			}
		}
	}

	// Token: 0x17000DAA RID: 3498
	// (get) Token: 0x06005CE7 RID: 23783 RVA: 0x0022735D File Offset: 0x0022575D
	// (set) Token: 0x06005CE8 RID: 23784 RVA: 0x00227365 File Offset: 0x00225765
	public Color helpColor
	{
		get
		{
			return this._helpColor;
		}
		set
		{
			if (this._helpColor != value)
			{
				this._helpColor = value;
				this.SyncHelpText();
			}
		}
	}

	// Token: 0x06005CE9 RID: 23785 RVA: 0x00227385 File Offset: 0x00225785
	public void ShowTempHelp(string text)
	{
		this.tempHelpText = text;
		this.SyncHelpText();
	}

	// Token: 0x06005CEA RID: 23786 RVA: 0x00227394 File Offset: 0x00225794
	public void HideTempHelp()
	{
		this.tempHelpText = null;
		this.SyncHelpText();
	}

	// Token: 0x06005CEB RID: 23787 RVA: 0x002273A4 File Offset: 0x002257A4
	protected void SyncActiveHands()
	{
		if (!this.autoSimulation)
		{
			if (this.leftHandAlternate != null)
			{
				this.leftHandAlternate.gameObject.SetActive(false);
			}
			if (this.leftHand != null)
			{
				this.leftHand.gameObject.SetActive(false);
			}
			if (this.rightHandAlternate != null)
			{
				this.rightHandAlternate.gameObject.SetActive(false);
			}
			if (this.rightHand != null)
			{
				this.rightHand.gameObject.SetActive(false);
			}
		}
		else if (this._alwaysUseAlternateHands)
		{
			if (this.leftHandAlternate != null)
			{
				this.leftHandAlternate.gameObject.SetActive(!this.IsMonitorOnly);
			}
			if (this.leftHand != null)
			{
				this.leftHand.gameObject.SetActive(this._leapHandLeftConnected);
			}
			if (this.rightHandAlternate != null)
			{
				this.rightHandAlternate.gameObject.SetActive(!this.IsMonitorOnly);
			}
			if (this.rightHand != null)
			{
				this.rightHand.gameObject.SetActive(this._leapHandRightConnected);
			}
		}
		else
		{
			if (this.leftHandAlternate != null)
			{
				this.leftHandAlternate.gameObject.SetActive(this._leapHandLeftConnected);
			}
			if (this.leftHand != null)
			{
				this.leftHand.gameObject.SetActive(!this.IsMonitorOnly);
			}
			if (this.rightHandAlternate != null)
			{
				this.rightHandAlternate.gameObject.SetActive(this._leapHandRightConnected);
			}
			if (this.rightHand != null)
			{
				this.rightHand.gameObject.SetActive(!this.IsMonitorOnly);
			}
		}
	}

	// Token: 0x17000DAB RID: 3499
	// (get) Token: 0x06005CEC RID: 23788 RVA: 0x0022759D File Offset: 0x0022599D
	// (set) Token: 0x06005CED RID: 23789 RVA: 0x002275A8 File Offset: 0x002259A8
	public bool alwaysUseAlternateHands
	{
		get
		{
			return this._alwaysUseAlternateHands;
		}
		set
		{
			if (this._alwaysUseAlternateHands != value)
			{
				this._alwaysUseAlternateHands = value;
				if (this.alwaysUseAlternateHandsToggle != null)
				{
					this.alwaysUseAlternateHandsToggle.isOn = this._alwaysUseAlternateHands;
				}
				if (UserPreferences.singleton != null)
				{
					UserPreferences.singleton.SavePreferences();
				}
				this.SyncActiveHands();
			}
		}
	}

	// Token: 0x17000DAC RID: 3500
	// (get) Token: 0x06005CEE RID: 23790 RVA: 0x0022760A File Offset: 0x00225A0A
	public FreeControllerV3 RightGrabbedController
	{
		get
		{
			return this.rightGrabbedController;
		}
	}

	// Token: 0x17000DAD RID: 3501
	// (get) Token: 0x06005CEF RID: 23791 RVA: 0x00227612 File Offset: 0x00225A12
	public FreeControllerV3 LeftGrabbedController
	{
		get
		{
			return this.leftGrabbedController;
		}
	}

	// Token: 0x17000DAE RID: 3502
	// (get) Token: 0x06005CF0 RID: 23792 RVA: 0x0022761A File Offset: 0x00225A1A
	public FreeControllerV3 RightFullGrabbedController
	{
		get
		{
			return this.rightFullGrabbedController;
		}
	}

	// Token: 0x17000DAF RID: 3503
	// (get) Token: 0x06005CF1 RID: 23793 RVA: 0x00227622 File Offset: 0x00225A22
	public FreeControllerV3 LeftFullGrabbedController
	{
		get
		{
			return this.leftFullGrabbedController;
		}
	}

	// Token: 0x17000DB0 RID: 3504
	// (get) Token: 0x06005CF2 RID: 23794 RVA: 0x0022762A File Offset: 0x00225A2A
	// (set) Token: 0x06005CF3 RID: 23795 RVA: 0x00227634 File Offset: 0x00225A34
	public float worldScale
	{
		get
		{
			return this._worldScale;
		}
		set
		{
			if (this._worldScale != value)
			{
				this._worldScale = value;
				if (this.worldScaleSlider != null)
				{
					this.worldScaleSlider.value = this._worldScale;
				}
				if (this.worldScaleSliderAlt != null)
				{
					this.worldScaleSliderAlt.value = this._worldScale;
				}
				this.SplashNavigationHologrid(1f);
				if (this.rayLineLeft != null)
				{
					this.rayLineLeft.startWidth = this.rayLineWidth * this._worldScale;
					this.rayLineLeft.endWidth = this.rayLineLeft.startWidth;
				}
				if (this.rayLineRight != null)
				{
					this.rayLineRight.startWidth = this.rayLineWidth * this._worldScale;
					this.rayLineRight.endWidth = this.rayLineRight.startWidth;
				}
				Vector3 a = Vector3.zero;
				if (this.centerCameraTarget != null)
				{
					a = this.centerCameraTarget.transform.position;
				}
				Vector3 localScale = new Vector3(this._worldScale, this._worldScale, this._worldScale);
				if (this.worldScaleTransform)
				{
					this.worldScaleTransform.localScale = localScale;
				}
				else
				{
					base.transform.localScale = localScale;
				}
				if (this.centerCameraTarget != null && this.navigationRig != null)
				{
					if (this._useLegacyWorldScaleChange)
					{
						Vector3 position = this.centerCameraTarget.transform.position;
						Vector3 b = a - position;
						Vector3 vector = this.navigationRig.position + b;
						Vector3 up = this.navigationRig.up;
						float num = Vector3.Dot(vector - this.navigationRig.position, up);
						vector += up * -num;
						this.navigationRig.position = vector;
					}
					else
					{
						this.playerHeightAdjust = 0f;
						Vector3 position2 = this.centerCameraTarget.transform.position;
						Vector3 vector2 = a - position2;
						Vector3 up2 = this.navigationRig.up;
						float num2 = Vector3.Dot(vector2, up2);
						vector2 += up2 * -num2;
						this.navigationRig.position += vector2;
						this.playerHeightAdjust = num2;
					}
				}
				if (LookInputModule.singleton != null)
				{
					LookInputModule.singleton.worldScale = this._worldScale;
				}
				ScaleChangeReceiver[] componentsInChildren = base.GetComponentsInChildren<ScaleChangeReceiver>(true);
				foreach (ScaleChangeReceiver scaleChangeReceiver in componentsInChildren)
				{
					scaleChangeReceiver.ScaleChanged(this._worldScale);
				}
				this.SyncPlayerHeightAdjust();
			}
		}
	}

	// Token: 0x17000DB1 RID: 3505
	// (get) Token: 0x06005CF4 RID: 23796 RVA: 0x00227907 File Offset: 0x00225D07
	// (set) Token: 0x06005CF5 RID: 23797 RVA: 0x0022790F File Offset: 0x00225D0F
	public float controllerScale
	{
		get
		{
			return this._controllerScale;
		}
		set
		{
			if (this._controllerScale != value)
			{
				this._controllerScale = value;
				if (this.controllerScaleSlider != null)
				{
					this.controllerScaleSlider.value = this._controllerScale;
				}
			}
		}
	}

	// Token: 0x17000DB2 RID: 3506
	// (get) Token: 0x06005CF6 RID: 23798 RVA: 0x00227946 File Offset: 0x00225D46
	// (set) Token: 0x06005CF7 RID: 23799 RVA: 0x00227950 File Offset: 0x00225D50
	public bool useLegacyWorldScaleChange
	{
		get
		{
			return this._useLegacyWorldScaleChange;
		}
		set
		{
			if (this._useLegacyWorldScaleChange != value)
			{
				this._useLegacyWorldScaleChange = value;
				if (this.useLegacyWorldScaleChangeToggle != null)
				{
					this.useLegacyWorldScaleChangeToggle.isOn = this._useLegacyWorldScaleChange;
				}
				if (UserPreferences.singleton != null)
				{
					UserPreferences.singleton.SavePreferences();
				}
			}
		}
	}

	// Token: 0x06005CF8 RID: 23800 RVA: 0x002279AC File Offset: 0x00225DAC
	public void SelectController(FreeControllerV3 controller, bool alignView = false, bool alignRotationOnly = true, bool alignUpDown = true, bool openUI = true)
	{
		if (this.selectedController != controller)
		{
			this.ClearSelection(false);
			this.selectedController = controller;
			this.selectedController.selected = true;
			this.AddPositionRotationHandlesToSelectedController();
			if (this.selectedController != null && this.selectedController.containingAtom != null)
			{
				this.lastCycleSelectAtomUid = this.selectedController.containingAtom.uid;
				this.lastCycleSelectAtomType = this.selectedController.containingAtom.type;
			}
			if (this.selectedControllerNameDisplay != null)
			{
				this.selectedControllerNameDisplay.text = this.selectedController.containingAtom.uid + ":" + this.selectedController.name;
			}
			this.SyncUIToSelectedController();
		}
		if (openUI)
		{
			this.activeUI = SuperController.ActiveUI.SelectedOptions;
		}
		if (alignView)
		{
			this.FocusOnSelectedController(alignRotationOnly, alignUpDown);
		}
	}

	// Token: 0x06005CF9 RID: 23801 RVA: 0x00227AA4 File Offset: 0x00225EA4
	public void SelectController(string atomName, string controllerName, bool alignView = false, bool alignRotationOnly = true, bool alignUpDown = true, bool openUI = true)
	{
		FreeControllerV3 freeControllerV = this.FreeControllerNameToFreeController(atomName + ":" + controllerName);
		if (freeControllerV != null)
		{
			this.SelectController(freeControllerV, alignView, alignRotationOnly, alignUpDown, openUI);
		}
	}

	// Token: 0x06005CFA RID: 23802 RVA: 0x00227AE0 File Offset: 0x00225EE0
	private void SelectFreeController(string cv)
	{
		string currentValue = this.selectAtomPopup.currentValue;
		if (currentValue != "None")
		{
			string currentValue2 = this.selectControllerPopup.currentValue;
			if (currentValue2 != "None")
			{
				bool alignView = false;
				if (this.quickSelectAlignToggle != null && this.quickSelectAlignToggle.isOn)
				{
					alignView = true;
				}
				bool flag = false;
				if (this.quickSelectMoveToggle != null && this.quickSelectMoveToggle.isOn)
				{
					flag = true;
				}
				bool openUI = false;
				if (this.quickSelectOpenUIToggle != null && this.quickSelectOpenUIToggle.isOn)
				{
					openUI = true;
				}
				this.SelectController(currentValue, currentValue2, alignView, !flag, true, openUI);
			}
			else
			{
				this.ClearSelection(false);
			}
		}
		else
		{
			this.ClearSelection(false);
		}
	}

	// Token: 0x06005CFB RID: 23803 RVA: 0x00227BBE File Offset: 0x00225FBE
	public Atom GetSelectedAtom()
	{
		if (this.selectedController != null)
		{
			return this.selectedController.containingAtom;
		}
		return null;
	}

	// Token: 0x06005CFC RID: 23804 RVA: 0x00227BDE File Offset: 0x00225FDE
	public FreeControllerV3 GetSelectedController()
	{
		return this.selectedController;
	}

	// Token: 0x06005CFD RID: 23805 RVA: 0x00227BE8 File Offset: 0x00225FE8
	public void ClearSelection(bool syncSelectedUI = true)
	{
		if (this.selectedController != null)
		{
			this.selectedController.selected = false;
			this.selectedController.hidden = true;
			this.selectedController.guihidden = true;
			if (this.selectedControllerPositionHandle != null)
			{
				this.selectedControllerPositionHandle.ForceDrop();
				this.selectedControllerPositionHandle.controller = null;
				this.selectedControllerPositionHandle.enabled = false;
			}
			if (this.selectedControllerRotationHandle != null)
			{
				this.selectedControllerRotationHandle.ForceDrop();
				this.selectedControllerRotationHandle.controller = null;
				this.selectedControllerRotationHandle.enabled = false;
			}
			this.selectedController = null;
			if (syncSelectedUI)
			{
				this.SyncUIToSelectedController();
			}
		}
		if (LookInputModule.singleton != null)
		{
			LookInputModule.singleton.ClearSelection();
		}
		this.SyncVisibility();
	}

	// Token: 0x06005CFE RID: 23806 RVA: 0x00227CC5 File Offset: 0x002260C5
	public void ToggleRotationMode()
	{
		if (this.selectedController != null)
		{
			this.selectedController.NextControlMode();
		}
	}

	// Token: 0x17000DB3 RID: 3507
	// (get) Token: 0x06005CFF RID: 23807 RVA: 0x00227CE3 File Offset: 0x002260E3
	public SuperController.SelectMode currentSelectMode
	{
		get
		{
			return this.selectMode;
		}
	}

	// Token: 0x06005D00 RID: 23808 RVA: 0x00227CEB File Offset: 0x002260EB
	public void SetOnlyShowControllers(HashSet<FreeControllerV3> onlyControllers)
	{
		this.onlyShowControllers = onlyControllers;
		this.SyncVisibility();
	}

	// Token: 0x06005D01 RID: 23809 RVA: 0x00227CFC File Offset: 0x002260FC
	private void ClearSelectionHUDs()
	{
		if (this.selectionHUD != null)
		{
			this.selectionHUD.gameObject.SetActive(false);
			this.selectionHUD.ClearSelections();
		}
		if (this.rightSelectionHUD != null)
		{
			this.rightSelectionHUD.ClearSelections();
		}
		if (this.leftSelectionHUD != null)
		{
			this.leftSelectionHUD.ClearSelections();
		}
	}

	// Token: 0x06005D02 RID: 23810 RVA: 0x00227D70 File Offset: 0x00226170
	protected void SyncCursor()
	{
		this.cursorLockedLastFrame = false;
		if (this.selectMode == SuperController.SelectMode.FreeMoveMouse)
		{
			Cursor.visible = false;
			if (Cursor.lockState != CursorLockMode.Locked)
			{
				Cursor.lockState = CursorLockMode.Locked;
				this.cursorLockedLastFrame = true;
			}
		}
		else
		{
			Cursor.visible = (this.potentialGrabbedControllerMouse == null);
			Cursor.lockState = CursorLockMode.None;
		}
	}

	// Token: 0x06005D03 RID: 23811 RVA: 0x00227DCC File Offset: 0x002261CC
	public void SyncVisibility()
	{
		bool flag = this._mainHUDVisible || !UserPreferences.singleton.showTargetsMenuOnly;
		if (this.mouseCrosshair != null)
		{
			this.mouseCrosshair.SetActive(this.selectMode == SuperController.SelectMode.FreeMoveMouse);
		}
		if (this.freeMouseMoveModeIndicator != null)
		{
			this.freeMouseMoveModeIndicator.SetActive(this.selectMode == SuperController.SelectMode.FreeMoveMouse);
		}
		bool flag2 = this.selectMode == SuperController.SelectMode.Targets || this.selectMode == SuperController.SelectMode.FilteredTargets;
		SuperController.SelectMode selectMode = this.selectMode;
		if (selectMode != SuperController.SelectMode.Targets)
		{
			if (selectMode != SuperController.SelectMode.FilteredTargets)
			{
				if (this.allControllers != null)
				{
					foreach (FreeControllerV3 freeControllerV in this.allControllers)
					{
						freeControllerV.hidden = true;
					}
				}
				if (this.selectedControllerPositionHandle != null && this.selectedControllerPositionHandle.controller != null)
				{
					this.selectedControllerPositionHandle.enabled = this._mainHUDVisible;
				}
				if (this.selectedControllerRotationHandle != null && this.selectedControllerRotationHandle.controller != null)
				{
					this.selectedControllerRotationHandle.enabled = this._mainHUDVisible;
				}
			}
			else
			{
				foreach (FreeControllerV3 freeControllerV2 in this.allControllers)
				{
					if (this.selectedController == null || this.selectedController != freeControllerV2)
					{
						freeControllerV2.hidden = true;
					}
					else if (this.gameMode == SuperController.GameMode.Edit || freeControllerV2.interactableInPlayMode)
					{
						freeControllerV2.hidden = false;
					}
					else
					{
						freeControllerV2.hidden = true;
					}
				}
				if (this.selectedControllerPositionHandle != null && this.selectedControllerPositionHandle.controller != null)
				{
					this.selectedControllerPositionHandle.enabled = flag;
				}
				if (this.selectedControllerRotationHandle != null && this.selectedControllerRotationHandle.controller != null)
				{
					this.selectedControllerRotationHandle.enabled = flag;
				}
			}
		}
		else if (this.onlyShowControllers != null)
		{
			foreach (FreeControllerV3 freeControllerV3 in this.allControllers)
			{
				if (this.onlyShowControllers.Contains(freeControllerV3))
				{
					freeControllerV3.hidden = false;
				}
				else
				{
					freeControllerV3.hidden = true;
				}
			}
		}
		else
		{
			foreach (FreeControllerV3 freeControllerV4 in this.allControllers)
			{
				if (this.gameMode == SuperController.GameMode.Edit && flag)
				{
					if (UserPreferences.singleton.hideInactiveTargets && freeControllerV4.currentPositionState == FreeControllerV3.PositionState.Off && freeControllerV4.currentRotationState == FreeControllerV3.RotationState.Off)
					{
						freeControllerV4.hidden = true;
					}
					else if (freeControllerV4.containingAtom == null)
					{
						freeControllerV4.hidden = false;
					}
					else if (freeControllerV4.containingAtom.tempHidden)
					{
						freeControllerV4.hidden = true;
					}
					else if (this._showHiddenAtoms || !freeControllerV4.containingAtom.hidden)
					{
						freeControllerV4.hidden = false;
					}
					else
					{
						freeControllerV4.hidden = true;
					}
				}
				else
				{
					freeControllerV4.hidden = true;
				}
			}
			if (this.selectedControllerPositionHandle != null && this.selectedControllerPositionHandle.controller != null)
			{
				this.selectedControllerPositionHandle.enabled = flag;
			}
			if (this.selectedControllerRotationHandle != null && this.selectedControllerRotationHandle.controller != null)
			{
				this.selectedControllerRotationHandle.enabled = flag;
			}
		}
		Atom atom = null;
		if (this.selectedController != null)
		{
			atom = this.selectedController.containingAtom;
		}
		if (this.selectMode == SuperController.SelectMode.FilteredTargets && atom != null)
		{
			foreach (FreeControllerV3 freeControllerV5 in atom.freeControllers)
			{
				if (this.gameMode == SuperController.GameMode.Edit || freeControllerV5.interactableInPlayMode)
				{
					freeControllerV5.hidden = false;
				}
				else
				{
					freeControllerV5.hidden = true;
				}
			}
		}
		if (this.fpMap != null)
		{
			foreach (ForceProducerV2 forceProducerV in this.fpMap.Values)
			{
				forceProducerV.drawLines = false;
				if (atom != null && atom == forceProducerV.containingAtom && flag && flag2)
				{
					forceProducerV.drawLines = true;
				}
			}
		}
		if (this.gpMap != null)
		{
			foreach (GrabPoint grabPoint in this.gpMap.Values)
			{
				grabPoint.drawLines = false;
				if (atom != null && atom == grabPoint.containingAtom && flag && flag2)
				{
					grabPoint.drawLines = true;
				}
			}
		}
		if (this.allAnimationPatterns != null)
		{
			foreach (AnimationPattern animationPattern in this.allAnimationPatterns)
			{
				animationPattern.draw = (this.selectMode == SuperController.SelectMode.Targets && this._mainHUDVisible && !animationPattern.hideCurveUnlessSelected && !animationPattern.containingAtom.hidden && !animationPattern.containingAtom.tempHidden);
				animationPattern.SetDrawColor(Color.red);
				if (((atom != null && atom == animationPattern.containingAtom) || (this._isolatedAtom != null && this._isolatedAtom == animationPattern.containingAtom)) && flag && flag2)
				{
					animationPattern.draw = true;
					animationPattern.SetDrawColor(Color.blue);
					foreach (AnimationStep animationStep in animationPattern.steps)
					{
						if (animationStep.containingAtom != null && animationStep.containingAtom.freeControllers != null)
						{
							foreach (FreeControllerV3 freeControllerV6 in animationStep.containingAtom.freeControllers)
							{
								if (this.gameMode == SuperController.GameMode.Edit || freeControllerV6.interactableInPlayMode)
								{
									freeControllerV6.hidden = false;
								}
								else
								{
									freeControllerV6.hidden = true;
								}
							}
						}
					}
				}
			}
		}
		if (this.allAnimationSteps != null)
		{
			foreach (AnimationStep animationStep2 in this.allAnimationSteps)
			{
				if (animationStep2.animationParent != null && flag && flag2 && ((atom != null && atom == animationStep2.containingAtom) || (this._isolatedAtom != null && this._isolatedAtom == animationStep2.containingAtom)))
				{
					animationStep2.animationParent.draw = true;
					animationStep2.animationParent.SetDrawColor(Color.blue);
					if (animationStep2.animationParent.containingAtom != null && animationStep2.animationParent.containingAtom.freeControllers != null)
					{
						foreach (FreeControllerV3 freeControllerV7 in animationStep2.animationParent.containingAtom.freeControllers)
						{
							if (this.gameMode == SuperController.GameMode.Edit || freeControllerV7.interactableInPlayMode)
							{
								freeControllerV7.hidden = false;
							}
							else
							{
								freeControllerV7.hidden = true;
							}
						}
					}
					foreach (AnimationStep animationStep3 in animationStep2.animationParent.steps)
					{
						if (animationStep3.containingAtom != null && animationStep3.containingAtom.freeControllers != null)
						{
							foreach (FreeControllerV3 freeControllerV8 in animationStep3.containingAtom.freeControllers)
							{
								if (this.gameMode == SuperController.GameMode.Edit || freeControllerV8.interactableInPlayMode)
								{
									freeControllerV8.hidden = false;
								}
								else
								{
									freeControllerV8.hidden = true;
								}
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06005D04 RID: 23812 RVA: 0x00228818 File Offset: 0x00226C18
	protected void SetSelectionHUDHeader(SelectionHUD sh, string txt)
	{
		if (sh != null && sh.headerText != null)
		{
			sh.headerText.text = txt;
		}
	}

	// Token: 0x06005D05 RID: 23813 RVA: 0x00228844 File Offset: 0x00226C44
	private void ResetSelectionInstances()
	{
		if (this.selectionInstances != null)
		{
			foreach (Transform transform in this.selectionInstances)
			{
				if (transform != null)
				{
					UnityEngine.Object.Destroy(transform.gameObject);
				}
			}
		}
		if (this.selectionInstances == null)
		{
			this.selectionInstances = new List<Transform>();
		}
		else
		{
			this.selectionInstances.Clear();
		}
		this.highlightedSelectTargetsLook = null;
		this.highlightedSelectTargetsLeft = null;
		this.highlightedSelectTargetsRight = null;
		this.highlightedSelectTargetsMouse = null;
	}

	// Token: 0x06005D06 RID: 23814 RVA: 0x00228900 File Offset: 0x00226D00
	private void SelectModeCommon(string helpT, string selectionText = "", bool setSelectHUDActive = true, bool resetSelectionInstances = true, bool clearSelection = true, bool clearSelectionHUDs = true)
	{
		this.helpText = helpT;
		if (resetSelectionInstances)
		{
			this.ResetSelectionInstances();
		}
		if (clearSelection)
		{
			this.ClearSelection(true);
		}
		if (clearSelectionHUDs)
		{
			this.ClearSelectionHUDs();
		}
		if (setSelectHUDActive)
		{
			this.SetSelectionHUDHeader(this.selectionHUD, selectionText);
			if (this.selectionHUD != null)
			{
				this.selectionHUD.gameObject.SetActive(true);
			}
		}
		if (this.hiResScreenshotPreview != null)
		{
			this.hiResScreenshotPreview.gameObject.SetActive(false);
		}
		if (this.hiResScreenshotCamera != null)
		{
			this.hiResScreenshotCamera.enabled = false;
		}
		if (this.selectMode == SuperController.SelectMode.SaveScreenshot)
		{
			this.ProcessSaveScreenshot(true);
		}
	}

	// Token: 0x06005D07 RID: 23815 RVA: 0x002289C4 File Offset: 0x00226DC4
	public void SelectModeControllers(SuperController.SelectControllerCallback scc)
	{
		if (this.selectMode != SuperController.SelectMode.Controller)
		{
			this.SelectModeCommon("Press Select to select Controller. Press Remote Grab to cancel.", "Select Controller", true, true, false, true);
			this.selectMode = SuperController.SelectMode.Controller;
			if (this.selectPrefab != null)
			{
				foreach (FreeControllerV3 freeControllerV in this.allControllers)
				{
					Transform transform = UnityEngine.Object.Instantiate<Transform>(this.selectPrefab);
					SelectTarget component = transform.GetComponent<SelectTarget>();
					if (component != null)
					{
						component.selectionName = freeControllerV.containingAtom.uid + ":" + freeControllerV.name;
						if (freeControllerV.deselectedMesh != null)
						{
							component.mesh = freeControllerV.deselectedMesh;
							component.meshScale = freeControllerV.deselectedMeshScale;
						}
					}
					transform.parent = freeControllerV.transform;
					transform.position = freeControllerV.transform.position;
					this.selectionInstances.Add(transform);
				}
			}
			this.selectControllerCallback = scc;
		}
		this.SyncVisibility();
	}

	// Token: 0x06005D08 RID: 23816 RVA: 0x00228AF0 File Offset: 0x00226EF0
	public void SelectModeForceProducers(SuperController.SelectForceProducerCallback sfpc)
	{
		if (this.selectMode != SuperController.SelectMode.ForceProducer)
		{
			this.SelectModeCommon("Press Select to select Force Producer. Press Remote Grab to cancel.", "Select Force Producer", true, true, false, true);
			this.selectMode = SuperController.SelectMode.ForceProducer;
			if (this.selectPrefab != null)
			{
				foreach (string text in this.fpMap.Keys)
				{
					ForceProducerV2 forceProducerV = this.ProducerNameToForceProducer(text);
					if (forceProducerV != null)
					{
						Transform transform = UnityEngine.Object.Instantiate<Transform>(this.selectPrefab);
						SelectTarget component = transform.GetComponent<SelectTarget>();
						if (component != null)
						{
							component.selectionName = text;
						}
						transform.parent = forceProducerV.transform;
						transform.position = forceProducerV.transform.position;
						this.selectionInstances.Add(transform);
					}
				}
			}
			this.selectForceProducerCallback = sfpc;
		}
		this.SyncVisibility();
	}

	// Token: 0x06005D09 RID: 23817 RVA: 0x00228BF8 File Offset: 0x00226FF8
	public void SelectModeForceReceivers(SuperController.SelectForceReceiverCallback sfrc)
	{
		if (this.selectMode != SuperController.SelectMode.ForceReceiver)
		{
			this.SelectModeCommon("Press Select to select Force Receiver. Press Remote Grab to cancel.", "Select Force Receiver", true, true, false, true);
			this.selectMode = SuperController.SelectMode.ForceReceiver;
			if (this.selectPrefab != null)
			{
				foreach (string text in this.frMap.Keys)
				{
					ForceReceiver forceReceiver = this.ReceiverNameToForceReceiver(text);
					if (forceReceiver != null && !forceReceiver.skipUIDrawing)
					{
						Transform transform = UnityEngine.Object.Instantiate<Transform>(this.selectPrefab);
						SelectTarget component = transform.GetComponent<SelectTarget>();
						if (component != null)
						{
							component.selectionName = text;
						}
						transform.parent = forceReceiver.transform;
						transform.position = forceReceiver.transform.position;
						this.selectionInstances.Add(transform);
					}
				}
			}
			this.selectForceReceiverCallback = sfrc;
		}
		this.SyncVisibility();
	}

	// Token: 0x06005D0A RID: 23818 RVA: 0x00228D08 File Offset: 0x00227108
	public void SelectModeRigidbody(SuperController.SelectRigidbodyCallback srbc)
	{
		if (this.selectMode != SuperController.SelectMode.Rigidbody)
		{
			this.SelectModeCommon("Press Select to select Physics Object. Press Remote Grab to cancel.", "Select Physics Object", true, true, false, true);
			this.selectMode = SuperController.SelectMode.Rigidbody;
			if (this.selectPrefab != null)
			{
				foreach (string text in this.rbMap.Keys)
				{
					Rigidbody rigidbody = this.RigidbodyNameToRigidbody(text);
					if (rigidbody != null)
					{
						ForceReceiver component = rigidbody.GetComponent<ForceReceiver>();
						if (component == null || !component.skipUIDrawing)
						{
							Transform transform = UnityEngine.Object.Instantiate<Transform>(this.selectPrefab);
							SelectTarget component2 = transform.GetComponent<SelectTarget>();
							if (component2 != null)
							{
								component2.selectionName = text;
							}
							transform.parent = rigidbody.transform;
							transform.position = rigidbody.transform.position;
							this.selectionInstances.Add(transform);
						}
					}
				}
			}
			this.selectRigidbodyCallback = srbc;
		}
		this.SyncVisibility();
	}

	// Token: 0x06005D0B RID: 23819 RVA: 0x00228E30 File Offset: 0x00227230
	public void SelectModeAtom(SuperController.SelectAtomCallback sac)
	{
		if (this.selectMode != SuperController.SelectMode.Atom)
		{
			this.SelectModeCommon("Press Select to select Atom. Press either Remote Grab to cancel.", "Select Atom", true, true, false, true);
			this.selectMode = SuperController.SelectMode.Atom;
			if (this.selectPrefab != null)
			{
				foreach (string text in this.atomUIDs)
				{
					Atom atomByUid = this.GetAtomByUid(text);
					if (atomByUid != null)
					{
						Transform transform = UnityEngine.Object.Instantiate<Transform>(this.selectPrefab);
						SelectTarget component = transform.GetComponent<SelectTarget>();
						if (component != null)
						{
							component.selectionName = text;
							if (atomByUid.mainController != null && atomByUid.mainController.deselectedMesh != null)
							{
								component.mesh = atomByUid.mainController.deselectedMesh;
								component.meshScale = atomByUid.mainController.deselectedMeshScale;
							}
						}
						if (atomByUid.childAtomContainer != null)
						{
							transform.parent = atomByUid.childAtomContainer;
							transform.position = atomByUid.childAtomContainer.position;
						}
						else
						{
							transform.parent = atomByUid.transform;
							transform.position = atomByUid.transform.position;
						}
						this.selectionInstances.Add(transform);
					}
				}
			}
			this.selectAtomCallback = sac;
		}
		this.SyncVisibility();
	}

	// Token: 0x06005D0C RID: 23820 RVA: 0x00228FBC File Offset: 0x002273BC
	public void SelectModePossess(bool excludeHeadClear = false)
	{
		if (this.selectMode != SuperController.SelectMode.Possess)
		{
			this.SelectModeCommon("Move Controllers or Head into spheres to possess. Press Select when complete, or press Remote Grab to cancel possess mode.", string.Empty, false, true, true, true);
			this.ClearPossess(excludeHeadClear);
			this.selectMode = SuperController.SelectMode.Possess;
			if (this.selectPrefab != null)
			{
				foreach (FreeControllerV3 freeControllerV in this.allControllers)
				{
					if (freeControllerV.possessable && (freeControllerV.canGrabPosition || freeControllerV.canGrabRotation))
					{
						Transform transform = UnityEngine.Object.Instantiate<Transform>(this.selectPrefab);
						SelectTarget component = transform.GetComponent<SelectTarget>();
						if (component != null)
						{
							component.selectionName = freeControllerV.containingAtom.uid + ":" + freeControllerV.name;
							if (freeControllerV.deselectedMesh != null)
							{
								component.mesh = freeControllerV.deselectedMesh;
								component.meshScale = freeControllerV.deselectedMeshScale;
							}
						}
						transform.parent = freeControllerV.transform;
						transform.position = freeControllerV.transform.position;
						this.selectionInstances.Add(transform);
					}
				}
			}
		}
		this.SyncVisibility();
	}

	// Token: 0x06005D0D RID: 23821 RVA: 0x0022910C File Offset: 0x0022750C
	public void SelectModeTwoStagePossess()
	{
		if (this.selectMode != SuperController.SelectMode.TwoStagePossess)
		{
			this.SelectModeCommon("Move Controllers or Head into spheres to select nodes to be possessed. Once all controls are selected, align motion controllers to desired possess from point. Press Select to lock in possess, or press Remote Grab to cancel.", string.Empty, false, true, true, true);
			this.ClearPossess();
			this.selectMode = SuperController.SelectMode.TwoStagePossess;
			if (this.selectPrefab != null)
			{
				foreach (FreeControllerV3 freeControllerV in this.allControllers)
				{
					if (freeControllerV.possessable && (freeControllerV.canGrabPosition || freeControllerV.canGrabRotation))
					{
						Transform transform = UnityEngine.Object.Instantiate<Transform>(this.selectPrefab);
						SelectTarget component = transform.GetComponent<SelectTarget>();
						if (component != null)
						{
							component.selectionName = freeControllerV.containingAtom.uid + ":" + freeControllerV.name;
							if (freeControllerV.deselectedMesh != null)
							{
								component.mesh = freeControllerV.deselectedMesh;
								component.meshScale = freeControllerV.deselectedMeshScale;
							}
						}
						transform.parent = freeControllerV.transform;
						transform.position = freeControllerV.transform.position;
						this.selectionInstances.Add(transform);
					}
				}
			}
		}
		this.SyncVisibility();
	}

	// Token: 0x06005D0E RID: 23822 RVA: 0x0022925C File Offset: 0x0022765C
	public void SelectModeTwoStagePossessNoClear()
	{
		if (this.selectMode != SuperController.SelectMode.TwoStagePossess)
		{
			this.SelectModeCommon("Move Controllers or Head into spheres to select nodes to be possessed. Once all controls are selected, align motion controllers to desired possess from point. Press Select to lock in possess, or press Remote Grab to cancel.", string.Empty, false, true, true, true);
			this.selectMode = SuperController.SelectMode.TwoStagePossess;
			if (this.selectPrefab != null)
			{
				foreach (FreeControllerV3 freeControllerV in this.allControllers)
				{
					if (freeControllerV.possessable && (freeControllerV.canGrabPosition || freeControllerV.canGrabRotation))
					{
						Transform transform = UnityEngine.Object.Instantiate<Transform>(this.selectPrefab);
						SelectTarget component = transform.GetComponent<SelectTarget>();
						if (component != null)
						{
							component.selectionName = freeControllerV.containingAtom.uid + ":" + freeControllerV.name;
							if (freeControllerV.deselectedMesh != null)
							{
								component.mesh = freeControllerV.deselectedMesh;
								component.meshScale = freeControllerV.deselectedMeshScale;
							}
						}
						transform.parent = freeControllerV.transform;
						transform.position = freeControllerV.transform.position;
						this.selectionInstances.Add(transform);
					}
				}
			}
		}
		this.SyncVisibility();
	}

	// Token: 0x06005D0F RID: 23823 RVA: 0x002293A4 File Offset: 0x002277A4
	public void SelectModeUnpossess()
	{
		if (this.selectMode != SuperController.SelectMode.Unpossess)
		{
			this.SelectModeCommon("Point at controller you would like to unpossess and press Select to unpossess. Press Remote Grab to when finished.", string.Empty, true, true, true, true);
			this.selectMode = SuperController.SelectMode.Unpossess;
			if (this.selectPrefab != null)
			{
				foreach (FreeControllerV3 freeControllerV in this.allControllers)
				{
					if (freeControllerV.possessed)
					{
						Transform transform = UnityEngine.Object.Instantiate<Transform>(this.selectPrefab);
						SelectTarget component = transform.GetComponent<SelectTarget>();
						if (component != null)
						{
							component.selectionName = freeControllerV.containingAtom.uid + ":" + freeControllerV.name;
							if (freeControllerV.deselectedMesh != null)
							{
								component.mesh = freeControllerV.deselectedMesh;
								component.meshScale = freeControllerV.deselectedMeshScale;
							}
						}
						transform.parent = freeControllerV.transform;
						transform.position = freeControllerV.transform.position;
						this.selectionInstances.Add(transform);
					}
				}
			}
		}
	}

	// Token: 0x06005D10 RID: 23824 RVA: 0x002294D0 File Offset: 0x002278D0
	public void SelectModePossessAndAlign()
	{
		if (this.selectMode != SuperController.SelectMode.PossessAndAlign)
		{
			this.SelectModeCommon("Press Select to select which controller to move head into, align to, and possess.", string.Empty, false, true, true, true);
			this.ClearPossess();
			this.selectMode = SuperController.SelectMode.PossessAndAlign;
			if (this.selectPrefab != null)
			{
				foreach (FreeControllerV3 freeControllerV in this.allControllers)
				{
					if (freeControllerV.possessable && (freeControllerV.canGrabPosition || freeControllerV.canGrabRotation))
					{
						Transform transform = UnityEngine.Object.Instantiate<Transform>(this.selectPrefab);
						SelectTarget component = transform.GetComponent<SelectTarget>();
						if (component != null)
						{
							component.selectionName = freeControllerV.containingAtom.uid + ":" + freeControllerV.name;
							if (freeControllerV.deselectedMesh != null)
							{
								component.mesh = freeControllerV.deselectedMesh;
								component.meshScale = freeControllerV.deselectedMeshScale;
							}
						}
						transform.parent = freeControllerV.transform;
						transform.position = freeControllerV.transform.position;
						this.selectionInstances.Add(transform);
					}
				}
			}
		}
		this.SyncVisibility();
	}

	// Token: 0x06005D11 RID: 23825 RVA: 0x00229620 File Offset: 0x00227A20
	public void SelectModeAnimationRecord(MotionAnimationMaster animationMaster)
	{
		this.StopRecording();
		this.currentAnimationMaster = animationMaster;
		if (this.selectMode != SuperController.SelectMode.AnimationRecord)
		{
			this.SelectModeCommon("Press Select or Spacebar to start recording", string.Empty, false, true, true, true);
			this.selectMode = SuperController.SelectMode.AnimationRecord;
		}
		this.SyncVisibility();
	}

	// Token: 0x06005D12 RID: 23826 RVA: 0x0022965E File Offset: 0x00227A5E
	public void SelectModeAnimationRecord()
	{
		this.SelectModeAnimationRecord(this.motionAnimationMaster);
	}

	// Token: 0x06005D13 RID: 23827 RVA: 0x0022966C File Offset: 0x00227A6C
	protected bool CheckIfControllerLinkedToMotionControl(Transform t, FreeControllerV3 fc)
	{
		Rigidbody component = t.GetComponent<Rigidbody>();
		return component != null && fc.linkToRB == component;
	}

	// Token: 0x06005D14 RID: 23828 RVA: 0x002296A0 File Offset: 0x00227AA0
	public void SelectModeArmedForRecord(IEnumerable<MotionAnimationControl> macsToChooseFrom)
	{
		this.SelectModeCommon("Press Select to toggle which controllers are armed for record. Green=Armed Red=Not Armed. Press Remote Grab when done.", string.Empty, false, true, false, true);
		this.selectMode = SuperController.SelectMode.ArmedForRecord;
		if (this.selectPrefab != null)
		{
			foreach (MotionAnimationControl motionAnimationControl in macsToChooseFrom)
			{
				Transform transform = UnityEngine.Object.Instantiate<Transform>(this.selectPrefab);
				SelectTarget component = transform.GetComponent<SelectTarget>();
				if (component != null)
				{
					component.selectionName = motionAnimationControl.containingAtom.uid + ":" + motionAnimationControl.name;
					if (motionAnimationControl.controller != null && motionAnimationControl.controller.deselectedMesh != null)
					{
						component.mesh = motionAnimationControl.controller.deselectedMesh;
						component.meshScale = motionAnimationControl.controller.deselectedMeshScale;
					}
					if (motionAnimationControl.armedForRecord)
					{
						component.SetColor(Color.green);
					}
					else
					{
						component.SetColor(Color.red);
					}
				}
				transform.parent = motionAnimationControl.transform;
				transform.position = motionAnimationControl.transform.position;
				this.selectionInstances.Add(transform);
			}
		}
	}

	// Token: 0x06005D15 RID: 23829 RVA: 0x002297F4 File Offset: 0x00227BF4
	public void SelectModeArmedForRecord()
	{
		if (this.motionAnimationMaster != null)
		{
			this.motionAnimationMaster.SelectControllersArmedForRecord();
		}
	}

	// Token: 0x06005D16 RID: 23830 RVA: 0x00229812 File Offset: 0x00227C12
	public void SelectModeTeleport()
	{
		if (this.selectMode != SuperController.SelectMode.Teleport)
		{
			this.SelectModeCommon("Aim controller and touch Select to choose where to teleport to. Press Select to teleport or press Remote Grab to cancel teleport mode.", string.Empty, false, true, false, true);
			this.ClearPossess();
			this.selectMode = SuperController.SelectMode.Teleport;
		}
		this.SyncVisibility();
	}

	// Token: 0x06005D17 RID: 23831 RVA: 0x0022984C File Offset: 0x00227C4C
	public void SelectModeFreeMove()
	{
		if (!this.worldUIActivated)
		{
			if (this.selectMode != SuperController.SelectMode.FreeMove)
			{
				string helpT;
				if (this.isOVR)
				{
					helpT = "Use left and right thumbsticks to move freely. Press Remote Grab to cancel free-move mode.";
				}
				else if (this.isOpenVR)
				{
					string localizedOrigin = this.freeModeMoveAction.GetLocalizedOrigin(SteamVR_Input_Sources.LeftHand);
					string localizedOrigin2 = this.freeModeMoveAction.GetLocalizedOrigin(SteamVR_Input_Sources.RightHand);
					helpT = string.Concat(new string[]
					{
						"Use ",
						localizedOrigin,
						" and ",
						localizedOrigin2,
						" to move freely. Press Grab to cancel free-move mode"
					});
				}
				else
				{
					helpT = string.Empty;
				}
				this.SelectModeCommon(helpT, string.Empty, false, true, false, true);
				this.selectMode = SuperController.SelectMode.FreeMove;
			}
			this.SyncVisibility();
		}
	}

	// Token: 0x06005D18 RID: 23832 RVA: 0x00229904 File Offset: 0x00227D04
	public void ToggleModeFreeMoveMouse()
	{
		if (!this.worldUIActivated && this.MonitorRigActive)
		{
			if (this.selectMode == SuperController.SelectMode.FreeMoveMouse)
			{
				this.SelectModeOff();
			}
			else if (this.selectMode != SuperController.SelectMode.FreeMoveMouse)
			{
				this.SelectModeCommon(string.Empty, string.Empty, false, true, false, true);
				this.selectMode = SuperController.SelectMode.FreeMoveMouse;
			}
			this.SyncVisibility();
		}
	}

	// Token: 0x06005D19 RID: 23833 RVA: 0x00229970 File Offset: 0x00227D70
	public void SelectModeFreeMoveMouse()
	{
		if (!this.worldUIActivated && this.MonitorRigActive)
		{
			if (this.selectMode != SuperController.SelectMode.FreeMoveMouse)
			{
				this.SelectModeCommon(string.Empty, string.Empty, false, true, false, true);
				this.selectMode = SuperController.SelectMode.FreeMoveMouse;
			}
			this.SyncVisibility();
		}
	}

	// Token: 0x06005D1A RID: 23834 RVA: 0x002299C4 File Offset: 0x00227DC4
	public void SelectModeScreenshot()
	{
		if (this.selectMode != SuperController.SelectMode.Screenshot)
		{
			this.SelectModeCommon("Look where you want to take a screenshot and press Select. Press Remote Grab to cancel screenshot mode.", string.Empty, false, true, false, true);
			this.ClearPossess();
			this.selectMode = SuperController.SelectMode.Screenshot;
			this.HideMainHUD();
			if (this.hiResScreenshotCamera != null)
			{
				this.hiResScreenshotCamera.enabled = true;
				if (this.hiResScreenShotCameraFOVSlider != null)
				{
					this.hiResScreenshotCamera.fieldOfView = this.hiResScreenShotCameraFOVSlider.value;
				}
				else
				{
					this.hiResScreenshotCamera.fieldOfView = 40f;
				}
			}
			if (this.hiResScreenshotPreview != null)
			{
				this.hiResScreenshotPreview.gameObject.SetActive(true);
			}
		}
		this.SyncVisibility();
	}

	// Token: 0x06005D1B RID: 23835 RVA: 0x00229A87 File Offset: 0x00227E87
	public void SelectModeCustom(string helpText)
	{
		if (this.selectMode != SuperController.SelectMode.Custom)
		{
			this.SelectModeCommon(helpText, string.Empty, false, true, false, true);
			this.selectMode = SuperController.SelectMode.Custom;
		}
		this.SyncVisibility();
	}

	// Token: 0x06005D1C RID: 23836 RVA: 0x00229AB4 File Offset: 0x00227EB4
	public void SelectModeCustomWithTargetControl(string helpText)
	{
		if (this.selectMode != SuperController.SelectMode.CustomWithTargetControl)
		{
			this.SelectModeCommon(helpText, string.Empty, false, true, false, true);
			this.selectMode = SuperController.SelectMode.CustomWithTargetControl;
		}
		this.SyncVisibility();
	}

	// Token: 0x06005D1D RID: 23837 RVA: 0x00229AE1 File Offset: 0x00227EE1
	public void SelectModeCustomWithVRTargetControl(string helpText)
	{
		if (this.selectMode != SuperController.SelectMode.CustomWithVRTargetControl)
		{
			this.SelectModeCommon(helpText, string.Empty, false, true, false, true);
			this.selectMode = SuperController.SelectMode.CustomWithVRTargetControl;
		}
		this.SyncVisibility();
	}

	// Token: 0x06005D1E RID: 23838 RVA: 0x00229B10 File Offset: 0x00227F10
	public void SelectModeOff()
	{
		if (this.selectMode != SuperController.SelectMode.Off)
		{
			this.ResetSelectionInstances();
			this.selectMode = SuperController.SelectMode.Off;
			this.SetSelectionHUDHeader(this.selectionHUD, "Highlighted Controllers");
			this.selectControllerCallback = null;
			this.selectForceProducerCallback = null;
			this.selectForceReceiverCallback = null;
			this.selectRigidbodyCallback = null;
			this.selectAtomCallback = null;
			if (this.selectionHUD != null)
			{
				this.selectionHUD.gameObject.SetActive(false);
			}
			this.helpText = string.Empty;
			this.helpColor = Color.white;
			this._pointerModeLeft = false;
			this._pointerModeRight = false;
			if (this.screenshotPreview != null)
			{
				this.screenshotPreview.gameObject.SetActive(false);
			}
			if (this.screenshotCamera != null)
			{
				this.screenshotCamera.enabled = false;
			}
			if (this.hiResScreenshotPreview != null)
			{
				this.hiResScreenshotPreview.gameObject.SetActive(false);
			}
			if (this.hiResScreenshotCamera != null)
			{
				this.hiResScreenshotCamera.enabled = false;
			}
		}
		this.SyncVisibility();
	}

	// Token: 0x06005D1F RID: 23839 RVA: 0x00229C34 File Offset: 0x00228034
	private void SelectModeTargets()
	{
		if (this.selectMode != SuperController.SelectMode.Targets)
		{
			this.ResetSelectionInstances();
			this.selectMode = SuperController.SelectMode.Targets;
			this.SetSelectionHUDHeader(this.selectionHUD, "Highlighted Controllers");
			this.selectControllerCallback = null;
			this.selectForceProducerCallback = null;
			this.selectForceReceiverCallback = null;
			this.selectRigidbodyCallback = null;
			this.selectAtomCallback = null;
			if (this.selectionHUD != null)
			{
				this.selectionHUD.gameObject.SetActive(false);
			}
			this.helpText = string.Empty;
			this.helpColor = Color.white;
			this._pointerModeLeft = true;
			this._pointerModeRight = true;
			if (this.hiResScreenshotPreview != null)
			{
				this.hiResScreenshotPreview.gameObject.SetActive(false);
			}
			if (this.hiResScreenshotCamera != null)
			{
				this.hiResScreenshotCamera.enabled = false;
			}
		}
		this.SyncVisibility();
	}

	// Token: 0x06005D20 RID: 23840 RVA: 0x00229D1C File Offset: 0x0022811C
	private void SelectModeFiltered()
	{
		if (this.selectMode != SuperController.SelectMode.FilteredTargets)
		{
			this.ResetSelectionInstances();
			this.selectMode = SuperController.SelectMode.FilteredTargets;
			this.SetSelectionHUDHeader(this.selectionHUD, "Highlighted Controllers");
			this.selectControllerCallback = null;
			this.selectForceProducerCallback = null;
			this.selectForceReceiverCallback = null;
			this.selectRigidbodyCallback = null;
			this.selectAtomCallback = null;
			if (this.selectionHUD != null)
			{
				this.selectionHUD.gameObject.SetActive(false);
			}
			if (this.hiResScreenshotPreview != null)
			{
				this.hiResScreenshotPreview.gameObject.SetActive(false);
			}
			if (this.hiResScreenshotCamera != null)
			{
				this.hiResScreenshotCamera.enabled = false;
			}
		}
		this.SyncVisibility();
	}

	// Token: 0x17000DB4 RID: 3508
	// (get) Token: 0x06005D21 RID: 23841 RVA: 0x00229DDD File Offset: 0x002281DD
	private Transform motionControllerLeft
	{
		get
		{
			if (this.isOVR)
			{
				return this.touchObjectLeft;
			}
			if (this.isOpenVR)
			{
				return this.viveObjectLeft;
			}
			return null;
		}
	}

	// Token: 0x17000DB5 RID: 3509
	// (get) Token: 0x06005D22 RID: 23842 RVA: 0x00229E04 File Offset: 0x00228204
	private Transform handMountLeft
	{
		get
		{
			if (this.isOVR)
			{
				return this.touchHandMountLeft;
			}
			if (this.isOpenVR)
			{
				return this.viveHandMountLeft;
			}
			return null;
		}
	}

	// Token: 0x17000DB6 RID: 3510
	// (get) Token: 0x06005D23 RID: 23843 RVA: 0x00229E2B File Offset: 0x0022822B
	private Transform centerHandLeft
	{
		get
		{
			if (this.isOVR)
			{
				return this.touchCenterHandLeft;
			}
			if (this.isOpenVR)
			{
				return this.viveCenterHandLeft;
			}
			return null;
		}
	}

	// Token: 0x17000DB7 RID: 3511
	// (get) Token: 0x06005D24 RID: 23844 RVA: 0x00229E52 File Offset: 0x00228252
	private Transform motionControllerRight
	{
		get
		{
			if (this.isOVR)
			{
				return this.touchObjectRight;
			}
			if (this.isOpenVR)
			{
				return this.viveObjectRight;
			}
			return null;
		}
	}

	// Token: 0x17000DB8 RID: 3512
	// (get) Token: 0x06005D25 RID: 23845 RVA: 0x00229E79 File Offset: 0x00228279
	private Transform handMountRight
	{
		get
		{
			if (this.isOVR)
			{
				return this.touchHandMountRight;
			}
			if (this.isOpenVR)
			{
				return this.viveHandMountRight;
			}
			return null;
		}
	}

	// Token: 0x17000DB9 RID: 3513
	// (get) Token: 0x06005D26 RID: 23846 RVA: 0x00229EA0 File Offset: 0x002282A0
	private Transform centerHandRight
	{
		get
		{
			if (this.isOVR)
			{
				return this.touchCenterHandRight;
			}
			if (this.isOpenVR)
			{
				return this.viveCenterHandRight;
			}
			return null;
		}
	}

	// Token: 0x17000DBA RID: 3514
	// (get) Token: 0x06005D27 RID: 23847 RVA: 0x00229EC7 File Offset: 0x002282C7
	private Transform motionControllerHead
	{
		get
		{
			if (this.centerCameraTarget != null)
			{
				return this.centerCameraTarget.transform;
			}
			return null;
		}
	}

	// Token: 0x06005D28 RID: 23848 RVA: 0x00229EE8 File Offset: 0x002282E8
	public bool GetMenuShow()
	{
		if (UserPreferences.singleton != null && UserPreferences.singleton.firstTimeUser)
		{
			return false;
		}
		if (this.isOVR)
		{
			return OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.Touch) || OVRInput.GetDown(OVRInput.Button.Four, OVRInput.Controller.Touch);
		}
		return this.isOpenVR && this.menuAction.GetStateDown(SteamVR_Input_Sources.Any);
	}

	// Token: 0x06005D29 RID: 23849 RVA: 0x00229F51 File Offset: 0x00228351
	public bool GetMenuMoveLeft()
	{
		if (this.isOVR)
		{
			return OVRInput.Get(OVRInput.Button.Four, OVRInput.Controller.Touch);
		}
		return this.isOpenVR && this.menuAction.GetState(SteamVR_Input_Sources.LeftHand);
	}

	// Token: 0x06005D2A RID: 23850 RVA: 0x00229F7F File Offset: 0x0022837F
	public bool GetMenuMoveRight()
	{
		if (this.isOVR)
		{
			return OVRInput.Get(OVRInput.Button.Two, OVRInput.Controller.Touch);
		}
		return this.isOpenVR && this.menuAction.GetState(SteamVR_Input_Sources.RightHand);
	}

	// Token: 0x06005D2B RID: 23851 RVA: 0x00229FAD File Offset: 0x002283AD
	public bool GetTeleportStart(bool inTeleportMode = false)
	{
		return this.GetTeleportStartLeft(inTeleportMode) || this.GetTeleportStartRight(inTeleportMode);
	}

	// Token: 0x06005D2C RID: 23852 RVA: 0x00229FC8 File Offset: 0x002283C8
	public bool GetTeleportStartLeft(bool inTeleportMode = false)
	{
		if (!this.isOVR)
		{
			return this.isOpenVR && !this.GUIhitLeft && (inTeleportMode || this.teleportShowAction.GetState(SteamVR_Input_Sources.LeftHand));
		}
		if (inTeleportMode)
		{
			return !this.GUIhitLeft && OVRInput.GetDown(OVRInput.Touch.Three, OVRInput.Controller.Touch);
		}
		return !this.GUIhitLeft && OVRInput.GetDown(OVRInput.Button.Start, OVRInput.Controller.Touch);
	}

	// Token: 0x06005D2D RID: 23853 RVA: 0x0022A044 File Offset: 0x00228444
	public bool GetTeleportStartRight(bool inTeleportMode = false)
	{
		if (this.isOVR)
		{
			return inTeleportMode && !this.GUIhitRight && OVRInput.GetDown(OVRInput.Touch.One, OVRInput.Controller.Touch);
		}
		return this.isOpenVR && !this.GUIhitRight && (inTeleportMode || this.teleportShowAction.GetState(SteamVR_Input_Sources.RightHand));
	}

	// Token: 0x06005D2E RID: 23854 RVA: 0x0022A0A8 File Offset: 0x002284A8
	public bool GetTeleportShow(bool inTeleportMode = false)
	{
		return this.GetTeleportShowLeft(inTeleportMode) || this.GetTeleportShowRight(inTeleportMode);
	}

	// Token: 0x06005D2F RID: 23855 RVA: 0x0022A0C0 File Offset: 0x002284C0
	public bool GetTeleportShowLeft(bool inTeleportMode = false)
	{
		if (!this.isOVR)
		{
			return this.isOpenVR && !this.GUIhitLeft && (inTeleportMode || this.teleportShowAction.GetState(SteamVR_Input_Sources.LeftHand));
		}
		if (inTeleportMode)
		{
			return !this.GUIhitLeft && OVRInput.Get(OVRInput.Touch.Three, OVRInput.Controller.Touch);
		}
		return !this.GUIhitLeft && OVRInput.Get(OVRInput.Button.Start, OVRInput.Controller.Touch);
	}

	// Token: 0x06005D30 RID: 23856 RVA: 0x0022A13C File Offset: 0x0022853C
	public bool GetTeleportShowRight(bool inTeleportMode = false)
	{
		if (this.isOVR)
		{
			return inTeleportMode && !this.GUIhitRight && OVRInput.Get(OVRInput.Touch.One, OVRInput.Controller.Touch);
		}
		return this.isOpenVR && !this.GUIhitRight && (inTeleportMode || this.teleportShowAction.GetState(SteamVR_Input_Sources.RightHand));
	}

	// Token: 0x06005D31 RID: 23857 RVA: 0x0022A1A0 File Offset: 0x002285A0
	public bool GetTeleportFinish(bool inTeleportMode = false)
	{
		return this.GetTeleportFinishLeft(inTeleportMode) || this.GetTeleportFinishRight(inTeleportMode);
	}

	// Token: 0x06005D32 RID: 23858 RVA: 0x0022A1B8 File Offset: 0x002285B8
	public bool GetTeleportFinishLeft(bool inTeleportMode = false)
	{
		if (!this.isOVR)
		{
			return this.isOpenVR && !this.GUIhitLeft && (inTeleportMode || this.teleportAction.GetStateDown(SteamVR_Input_Sources.LeftHand));
		}
		if (inTeleportMode)
		{
			return !this.GUIhitLeft && OVRInput.GetDown(OVRInput.Button.Three, OVRInput.Controller.Touch);
		}
		return !this.GUIhitLeft && OVRInput.GetUp(OVRInput.Button.Start, OVRInput.Controller.Touch);
	}

	// Token: 0x06005D33 RID: 23859 RVA: 0x0022A234 File Offset: 0x00228634
	public bool GetTeleportFinishRight(bool inTeleportMode = false)
	{
		if (this.isOVR)
		{
			return inTeleportMode && !this.GUIhitRight && OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.Touch);
		}
		return this.isOpenVR && !this.GUIhitRight && (inTeleportMode || this.teleportAction.GetStateDown(SteamVR_Input_Sources.RightHand));
	}

	// Token: 0x06005D34 RID: 23860 RVA: 0x0022A298 File Offset: 0x00228698
	public bool GetGrabNavigateStartLeft()
	{
		if (this.isOVR)
		{
			return this.oculusThumbstickFunction != SuperController.ThumbstickFunction.SwapAxis && OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.Touch);
		}
		return this.isOpenVR && !this.GUIhitLeft && this.grabNavigateAction.GetStateDown(SteamVR_Input_Sources.LeftHand);
	}

	// Token: 0x06005D35 RID: 23861 RVA: 0x0022A2F4 File Offset: 0x002286F4
	public bool GetGrabNavigateStartRight()
	{
		if (this.isOVR)
		{
			return this.oculusThumbstickFunction != SuperController.ThumbstickFunction.SwapAxis && OVRInput.GetDown(OVRInput.Button.SecondaryThumbstick, OVRInput.Controller.Touch);
		}
		return this.isOpenVR && !this.GUIhitRight && this.grabNavigateAction.GetStateDown(SteamVR_Input_Sources.RightHand);
	}

	// Token: 0x06005D36 RID: 23862 RVA: 0x0022A350 File Offset: 0x00228750
	public bool GetGrabNavigateLeft()
	{
		if (this.isOVR)
		{
			return this.oculusThumbstickFunction != SuperController.ThumbstickFunction.SwapAxis && OVRInput.Get(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.Touch);
		}
		return this.isOpenVR && this.grabNavigateAction.GetState(SteamVR_Input_Sources.LeftHand);
	}

	// Token: 0x06005D37 RID: 23863 RVA: 0x0022A39C File Offset: 0x0022879C
	public bool GetGrabNavigateRight()
	{
		if (this.isOVR)
		{
			return this.oculusThumbstickFunction != SuperController.ThumbstickFunction.SwapAxis && OVRInput.Get(OVRInput.Button.SecondaryThumbstick, OVRInput.Controller.Touch);
		}
		return this.isOpenVR && this.grabNavigateAction.GetState(SteamVR_Input_Sources.RightHand);
	}

	// Token: 0x06005D38 RID: 23864 RVA: 0x0022A3E8 File Offset: 0x002287E8
	private void CheckSwapAxis()
	{
		if (this.isOVR)
		{
			if (this.oculusThumbstickFunction != SuperController.ThumbstickFunction.GrabWorld && (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.Touch) || OVRInput.GetDown(OVRInput.Button.SecondaryThumbstick, OVRInput.Controller.Touch)))
			{
				this._swapAxis = !this._swapAxis;
			}
		}
		else if (this.isOpenVR && this.swapFreeMoveAxis.GetStateDown(SteamVR_Input_Sources.Any))
		{
			this._swapAxis = !this._swapAxis;
		}
	}

	// Token: 0x06005D39 RID: 23865 RVA: 0x0022A46C File Offset: 0x0022886C
	public Vector4 GetFreeNavigateVector(SteamVR_Action_Vector2 moveAction, bool ignoreDisable = false)
	{
		Vector4 result;
		result.x = 0f;
		result.y = 0f;
		result.z = 0f;
		result.w = 0f;
		if (this.isOVR)
		{
			if (ignoreDisable || !UserPreferences.singleton.oculusDisableFreeMove)
			{
				JoystickControl.Axis axis = this.navigationForwardAxis;
				JoystickControl.Axis axis2 = this.navigationSideAxis;
				JoystickControl.Axis axis3 = this.navigationUpAxis;
				JoystickControl.Axis axis4 = this.navigationTurnAxis;
				if (this._swapAxis)
				{
					axis = this.navigationUpAxis;
					axis2 = this.navigationTurnAxis;
					axis3 = this.navigationForwardAxis;
					axis4 = this.navigationSideAxis;
				}
				if (axis != JoystickControl.Axis.None)
				{
					if (this.invertNavigationForwardAxis)
					{
						result.y = -JoystickControl.GetAxis(axis);
					}
					else
					{
						result.y = JoystickControl.GetAxis(axis);
					}
				}
				if (axis2 != JoystickControl.Axis.None)
				{
					if (this.invertNavigationSideAxis)
					{
						result.x = -JoystickControl.GetAxis(axis2);
					}
					else
					{
						result.x = JoystickControl.GetAxis(axis2);
					}
				}
				if (axis3 != JoystickControl.Axis.None)
				{
					if (this.invertNavigationUpAxis)
					{
						result.w = -JoystickControl.GetAxis(axis3);
					}
					else
					{
						result.w = JoystickControl.GetAxis(axis3);
					}
				}
				if (axis4 != JoystickControl.Axis.None)
				{
					if (this.invertNavigationTurnAxis)
					{
						result.z = -JoystickControl.GetAxis(axis4);
					}
					else
					{
						result.z = JoystickControl.GetAxis(axis4);
					}
				}
			}
		}
		else if (this.isOpenVR)
		{
			Vector2 axis5 = moveAction.GetAxis(SteamVR_Input_Sources.LeftHand);
			Vector2 axis6 = moveAction.GetAxis(SteamVR_Input_Sources.RightHand);
			if (this._swapAxis)
			{
				result.x = axis6.x;
				result.y = axis6.y;
				result.z = axis5.x;
				result.w = axis5.y;
			}
			else
			{
				result.x = axis5.x;
				result.y = axis5.y;
				result.z = axis6.x;
				result.w = axis6.y;
			}
		}
		return result;
	}

	// Token: 0x06005D3A RID: 23866 RVA: 0x0022A678 File Offset: 0x00228A78
	private void HideLeftController()
	{
		if (this.isOVR && this.touchObjectLeft != null)
		{
			foreach (MeshRenderer meshRenderer in this.touchObjectLeftMeshRenderers)
			{
				meshRenderer.enabled = false;
			}
		}
	}

	// Token: 0x06005D3B RID: 23867 RVA: 0x0022A6C8 File Offset: 0x00228AC8
	private void HideRightController()
	{
		if (this.isOVR && this.touchObjectRight != null)
		{
			foreach (MeshRenderer meshRenderer in this.touchObjectRightMeshRenderers)
			{
				meshRenderer.enabled = false;
			}
		}
	}

	// Token: 0x06005D3C RID: 23868 RVA: 0x0022A718 File Offset: 0x00228B18
	private void ShowLeftController()
	{
		if (this.isOVR && this.touchObjectLeft != null)
		{
			foreach (MeshRenderer meshRenderer in this.touchObjectLeftMeshRenderers)
			{
				meshRenderer.enabled = true;
			}
		}
	}

	// Token: 0x06005D3D RID: 23869 RVA: 0x0022A768 File Offset: 0x00228B68
	private void ShowRightController()
	{
		if (this.isOVR && this.touchObjectRight != null)
		{
			foreach (MeshRenderer meshRenderer in this.touchObjectRightMeshRenderers)
			{
				meshRenderer.enabled = true;
			}
		}
	}

	// Token: 0x06005D3E RID: 23870 RVA: 0x0022A7B8 File Offset: 0x00228BB8
	private void ProcessGUIInteract()
	{
		if (this.isOVR)
		{
			bool down = OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.Touch);
			if (this.GUIhitRight && down)
			{
				this.rightGUIInteract = true;
			}
			bool down2 = OVRInput.GetDown(OVRInput.Button.Three, OVRInput.Controller.Touch);
			if (this.GUIhitLeft && down2)
			{
				this.leftGUIInteract = true;
			}
			bool up = OVRInput.GetUp(OVRInput.Button.One, OVRInput.Controller.Touch);
			if (up && this.rightGUIInteract)
			{
				this.rightGUIInteract = false;
			}
			up = OVRInput.GetUp(OVRInput.Button.Three, OVRInput.Controller.Touch);
			if (up && this.leftGUIInteract)
			{
				this.leftGUIInteract = false;
			}
		}
		else if (this.isOpenVR)
		{
			bool stateDown = this.UIInteractAction.GetStateDown(SteamVR_Input_Sources.RightHand);
			if (this.GUIhitRight && stateDown)
			{
				this.rightGUIInteract = true;
			}
			bool stateDown2 = this.UIInteractAction.GetStateDown(SteamVR_Input_Sources.LeftHand);
			if (this.GUIhitLeft && stateDown2)
			{
				this.leftGUIInteract = true;
			}
			bool stateUp = this.UIInteractAction.GetStateUp(SteamVR_Input_Sources.RightHand);
			if (stateUp && this.rightGUIInteract)
			{
				this.rightGUIInteract = false;
			}
			stateUp = this.UIInteractAction.GetStateUp(SteamVR_Input_Sources.LeftHand);
			if (stateUp && this.leftGUIInteract)
			{
				this.leftGUIInteract = false;
			}
		}
	}

	// Token: 0x06005D3F RID: 23871 RVA: 0x0022A8FC File Offset: 0x00228CFC
	public bool GetTargetShow()
	{
		if (this.isOVR)
		{
			bool flag = !this.rightGUIInteract && OVRInput.Get(OVRInput.Touch.One, OVRInput.Controller.Touch);
			bool flag2 = !this.leftGUIInteract && OVRInput.Get(OVRInput.Touch.Three, OVRInput.Controller.Touch);
			return flag || flag2 || this.targetsOnWithButton;
		}
		if (this.isOpenVR)
		{
			if (!this.rightGUIInteract && this.targetShowAction.GetState(SteamVR_Input_Sources.RightHand))
			{
				return true;
			}
			if (!this.leftGUIInteract && this.targetShowAction.GetState(SteamVR_Input_Sources.LeftHand))
			{
				return true;
			}
		}
		return this.targetsOnWithButton;
	}

	// Token: 0x06005D40 RID: 23872 RVA: 0x0022A9A3 File Offset: 0x00228DA3
	public bool GetLeftUIPointerShow()
	{
		if (this.isOVR)
		{
			return OVRInput.Get(OVRInput.Touch.Three, OVRInput.Controller.Touch);
		}
		return this.isOpenVR && this.targetShowAction.GetState(SteamVR_Input_Sources.LeftHand);
	}

	// Token: 0x06005D41 RID: 23873 RVA: 0x0022A9D1 File Offset: 0x00228DD1
	public bool GetRightUIPointerShow()
	{
		if (this.isOVR)
		{
			return OVRInput.Get(OVRInput.Touch.One, OVRInput.Controller.Touch);
		}
		return this.isOpenVR && this.targetShowAction.GetState(SteamVR_Input_Sources.RightHand);
	}

	// Token: 0x06005D42 RID: 23874 RVA: 0x0022A9FF File Offset: 0x00228DFF
	public void SetLeftSelect()
	{
		this._setLeftSelect = true;
	}

	// Token: 0x06005D43 RID: 23875 RVA: 0x0022AA08 File Offset: 0x00228E08
	public bool GetLeftSelect()
	{
		if (this._setLeftSelect)
		{
			this._setLeftSelect = false;
			return true;
		}
		if (this.leftGUIInteract)
		{
			return false;
		}
		if (this.isOVR)
		{
			return OVRInput.GetDown(OVRInput.Button.Three, OVRInput.Controller.Touch);
		}
		return this.isOpenVR && this.selectAction.GetStateDown(SteamVR_Input_Sources.LeftHand);
	}

	// Token: 0x06005D44 RID: 23876 RVA: 0x0022AA62 File Offset: 0x00228E62
	public void SetRightSelect()
	{
		this._setRightSelect = true;
	}

	// Token: 0x06005D45 RID: 23877 RVA: 0x0022AA6C File Offset: 0x00228E6C
	public bool GetRightSelect()
	{
		if (this._setRightSelect)
		{
			this._setRightSelect = false;
			return true;
		}
		if (this.rightGUIInteract)
		{
			return false;
		}
		if (this.isOVR)
		{
			return OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.Touch);
		}
		return this.isOpenVR && this.selectAction.GetStateDown(SteamVR_Input_Sources.RightHand);
	}

	// Token: 0x06005D46 RID: 23878 RVA: 0x0022AAC6 File Offset: 0x00228EC6
	public bool GetMouseSelect()
	{
		return !this.GUIhitMouse && !this.mouseClickUsed && Input.GetMouseButtonDown(0);
	}

	// Token: 0x06005D47 RID: 23879 RVA: 0x0022AAE6 File Offset: 0x00228EE6
	public bool GetMouseRelease()
	{
		return Input.GetMouseButtonUp(0);
	}

	// Token: 0x06005D48 RID: 23880 RVA: 0x0022AAEE File Offset: 0x00228EEE
	public bool GetCancel()
	{
		return this.GetLeftCancel() || this.GetRightCancel() || Input.GetKeyDown(KeyCode.Escape);
	}

	// Token: 0x06005D49 RID: 23881 RVA: 0x0022AB10 File Offset: 0x00228F10
	public bool GetLeftCancel()
	{
		if (this.isOVR)
		{
			return this.GetLeftRemoteGrab();
		}
		return this.isOpenVR && this.GetLeftRemoteGrab();
	}

	// Token: 0x06005D4A RID: 23882 RVA: 0x0022AB37 File Offset: 0x00228F37
	public bool GetRightCancel()
	{
		if (this.isOVR)
		{
			return this.GetRightRemoteGrab();
		}
		return this.isOpenVR && this.GetRightRemoteGrab();
	}

	// Token: 0x06005D4B RID: 23883 RVA: 0x0022AB60 File Offset: 0x00228F60
	public float GetLeftGrabVal()
	{
		if (this.isOVR)
		{
			if (UserPreferences.singleton.oculusSwapGrabAndTrigger)
			{
				return OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.Touch);
			}
			return OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.Touch);
		}
		else
		{
			if (this.isOpenVR)
			{
				return this.grabValAction.GetAxis(SteamVR_Input_Sources.LeftHand);
			}
			return 0f;
		}
	}

	// Token: 0x06005D4C RID: 23884 RVA: 0x0022ABB4 File Offset: 0x00228FB4
	public float GetRightGrabVal()
	{
		if (this.isOVR)
		{
			if (UserPreferences.singleton.oculusSwapGrabAndTrigger)
			{
				return OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger, OVRInput.Controller.Touch);
			}
			return OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger, OVRInput.Controller.Touch);
		}
		else
		{
			if (this.isOpenVR)
			{
				return this.grabValAction.GetAxis(SteamVR_Input_Sources.RightHand);
			}
			return 0f;
		}
	}

	// Token: 0x06005D4D RID: 23885 RVA: 0x0022AC08 File Offset: 0x00229008
	public bool GetLeftGrab()
	{
		if (!this.isOVR)
		{
			return this.isOpenVR && this.grabAction.GetStateDown(SteamVR_Input_Sources.LeftHand);
		}
		if (UserPreferences.singleton.oculusSwapGrabAndTrigger)
		{
			return OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.Touch);
		}
		return OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.Touch);
	}

	// Token: 0x06005D4E RID: 23886 RVA: 0x0022AC60 File Offset: 0x00229060
	public bool GetRightGrab()
	{
		if (!this.isOVR)
		{
			return this.isOpenVR && this.grabAction.GetStateDown(SteamVR_Input_Sources.RightHand);
		}
		if (UserPreferences.singleton.oculusSwapGrabAndTrigger)
		{
			return OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger, OVRInput.Controller.Touch);
		}
		return OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger, OVRInput.Controller.Touch);
	}

	// Token: 0x06005D4F RID: 23887 RVA: 0x0022ACB8 File Offset: 0x002290B8
	public void DisableRemoteHoldGrab()
	{
		this.remoteHoldGrabDisabled = true;
	}

	// Token: 0x06005D50 RID: 23888 RVA: 0x0022ACC1 File Offset: 0x002290C1
	public void EnableRemoteHoldGrab()
	{
		this.remoteHoldGrabDisabled = false;
	}

	// Token: 0x06005D51 RID: 23889 RVA: 0x0022ACCC File Offset: 0x002290CC
	public bool GetLeftRemoteGrab()
	{
		if (this.leftGUIInteract)
		{
			return false;
		}
		if (!this.isOVR)
		{
			return this.isOpenVR && this.remoteGrabAction.GetStateDown(SteamVR_Input_Sources.LeftHand);
		}
		if (UserPreferences.singleton.oculusSwapGrabAndTrigger)
		{
			return OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.Touch);
		}
		return OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.Touch);
	}

	// Token: 0x06005D52 RID: 23890 RVA: 0x0022AD34 File Offset: 0x00229134
	public bool GetRightRemoteGrab()
	{
		if (this.rightGUIInteract)
		{
			return false;
		}
		if (!this.isOVR)
		{
			return this.isOpenVR && this.remoteGrabAction.GetStateDown(SteamVR_Input_Sources.RightHand);
		}
		if (UserPreferences.singleton.oculusSwapGrabAndTrigger)
		{
			return OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger, OVRInput.Controller.Touch);
		}
		return OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger, OVRInput.Controller.Touch);
	}

	// Token: 0x06005D53 RID: 23891 RVA: 0x0022AD9C File Offset: 0x0022919C
	public bool GetLeftGrabRelease()
	{
		if (!this.isOVR)
		{
			return this.isOpenVR && this.grabAction.GetStateUp(SteamVR_Input_Sources.LeftHand);
		}
		if (UserPreferences.singleton.oculusSwapGrabAndTrigger)
		{
			return OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.Touch);
		}
		return OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.Touch);
	}

	// Token: 0x06005D54 RID: 23892 RVA: 0x0022ADF4 File Offset: 0x002291F4
	public bool GetRightGrabRelease()
	{
		if (!this.isOVR)
		{
			return this.isOpenVR && this.grabAction.GetStateUp(SteamVR_Input_Sources.RightHand);
		}
		if (UserPreferences.singleton.oculusSwapGrabAndTrigger)
		{
			return OVRInput.GetUp(OVRInput.Button.SecondaryHandTrigger, OVRInput.Controller.Touch);
		}
		return OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger, OVRInput.Controller.Touch);
	}

	// Token: 0x06005D55 RID: 23893 RVA: 0x0022AE4C File Offset: 0x0022924C
	public bool GetLeftRemoteGrabRelease()
	{
		if (!this.isOVR)
		{
			return this.isOpenVR && this.remoteGrabAction.GetStateUp(SteamVR_Input_Sources.LeftHand);
		}
		if (UserPreferences.singleton.oculusSwapGrabAndTrigger)
		{
			return OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.Touch);
		}
		return OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.Touch);
	}

	// Token: 0x06005D56 RID: 23894 RVA: 0x0022AEA4 File Offset: 0x002292A4
	public bool GetRightRemoteGrabRelease()
	{
		if (!this.isOVR)
		{
			return this.isOpenVR && this.remoteGrabAction.GetStateUp(SteamVR_Input_Sources.RightHand);
		}
		if (UserPreferences.singleton.oculusSwapGrabAndTrigger)
		{
			return OVRInput.GetUp(OVRInput.Button.SecondaryHandTrigger, OVRInput.Controller.Touch);
		}
		return OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger, OVRInput.Controller.Touch);
	}

	// Token: 0x06005D57 RID: 23895 RVA: 0x0022AEFC File Offset: 0x002292FC
	public bool GetLeftHoldGrab()
	{
		if (!this.isOVR)
		{
			return this.isOpenVR && this.holdGrabAction.GetStateDown(SteamVR_Input_Sources.LeftHand);
		}
		if (UserPreferences.singleton.oculusSwapGrabAndTrigger)
		{
			return OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.Touch);
		}
		return OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.Touch);
	}

	// Token: 0x06005D58 RID: 23896 RVA: 0x0022AF54 File Offset: 0x00229354
	public bool GetRightHoldGrab()
	{
		if (!this.isOVR)
		{
			return this.isOpenVR && this.holdGrabAction.GetStateDown(SteamVR_Input_Sources.RightHand);
		}
		if (UserPreferences.singleton.oculusSwapGrabAndTrigger)
		{
			return OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger, OVRInput.Controller.Touch);
		}
		return OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger, OVRInput.Controller.Touch);
	}

	// Token: 0x06005D59 RID: 23897 RVA: 0x0022AFAC File Offset: 0x002293AC
	public bool GetLeftRemoteHoldGrab()
	{
		if (this.leftGUIInteract)
		{
			return false;
		}
		if (!this.isOVR)
		{
			return this.isOpenVR && this.remoteHoldGrabAction.GetStateDown(SteamVR_Input_Sources.LeftHand);
		}
		if (UserPreferences.singleton.oculusSwapGrabAndTrigger)
		{
			return OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.Touch);
		}
		return OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.Touch);
	}

	// Token: 0x06005D5A RID: 23898 RVA: 0x0022B014 File Offset: 0x00229414
	public bool GetRightRemoteHoldGrab()
	{
		if (this.rightGUIInteract)
		{
			return false;
		}
		if (!this.isOVR)
		{
			return this.isOpenVR && this.remoteHoldGrabAction.GetStateDown(SteamVR_Input_Sources.RightHand);
		}
		if (UserPreferences.singleton.oculusSwapGrabAndTrigger)
		{
			return OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger, OVRInput.Controller.Touch);
		}
		return OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger, OVRInput.Controller.Touch);
	}

	// Token: 0x06005D5B RID: 23899 RVA: 0x0022B07C File Offset: 0x0022947C
	public bool GetLeftToggleHand()
	{
		if (this.isOVR)
		{
			return (this._allowGrabPlusTriggerHandToggle && OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.Touch) && OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.Touch)) || (this._allowGrabPlusTriggerHandToggle && OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.Touch) && OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.Touch));
		}
		return this.isOpenVR && this.toggleHandAction.GetStateDown(SteamVR_Input_Sources.LeftHand);
	}

	// Token: 0x06005D5C RID: 23900 RVA: 0x0022B108 File Offset: 0x00229508
	public bool GetRightToggleHand()
	{
		if (this.isOVR)
		{
			return (this._allowGrabPlusTriggerHandToggle && OVRInput.Get(OVRInput.Button.SecondaryHandTrigger, OVRInput.Controller.Touch) && OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger, OVRInput.Controller.Touch)) || (this._allowGrabPlusTriggerHandToggle && OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger, OVRInput.Controller.Touch) && OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger, OVRInput.Controller.Touch));
		}
		return this.isOpenVR && this.toggleHandAction.GetStateDown(SteamVR_Input_Sources.RightHand);
	}

	// Token: 0x06005D5D RID: 23901 RVA: 0x0022B194 File Offset: 0x00229594
	private void ProcessLookAtTrigger()
	{
		if (!this._isLoading)
		{
			this.castRay.origin = this.lookCamera.transform.position;
			this.castRay.direction = this.lookCamera.transform.forward;
			RaycastHit raycastHit;
			if (Physics.Raycast(this.castRay, out raycastHit, 50f, this.lookAtTriggerMask))
			{
				LookAtTrigger component = raycastHit.collider.GetComponent<LookAtTrigger>();
				if (component != null)
				{
					if (this.currentLookAtTrigger != null)
					{
						if (this.currentLookAtTrigger != component)
						{
							this.currentLookAtTrigger.EndLookAt();
							this.currentLookAtTrigger = component;
							this.currentLookAtTrigger.StartLookAt();
						}
					}
					else
					{
						this.currentLookAtTrigger = component;
						this.currentLookAtTrigger.StartLookAt();
					}
				}
				else if (this.currentLookAtTrigger != null)
				{
					this.currentLookAtTrigger.EndLookAt();
					this.currentLookAtTrigger = null;
				}
			}
			else if (this.currentLookAtTrigger != null)
			{
				this.currentLookAtTrigger.EndLookAt();
				this.currentLookAtTrigger = null;
			}
		}
	}

	// Token: 0x06005D5E RID: 23902 RVA: 0x0022B2C4 File Offset: 0x002296C4
	private void UnhighlightControllers(List<FreeControllerV3> highlightList)
	{
		foreach (FreeControllerV3 freeControllerV in highlightList)
		{
			freeControllerV.highlighted = false;
		}
		highlightList.Clear();
	}

	// Token: 0x06005D5F RID: 23903 RVA: 0x0022B324 File Offset: 0x00229724
	private void InitTargets()
	{
		if (this.selectionHUD != null)
		{
			this.selectionHUD.gameObject.SetActive(false);
		}
		this.SyncVisibility();
	}

	// Token: 0x06005D60 RID: 23904 RVA: 0x0022B350 File Offset: 0x00229750
	private void PrepControllers()
	{
		foreach (FreeControllerV3 freeControllerV in this.allControllers)
		{
			freeControllerV.ResetAppliedForces();
		}
	}

	// Token: 0x06005D61 RID: 23905 RVA: 0x0022B3AC File Offset: 0x002297AC
	private void ProcessTargetSelectionDoRaycast(SelectionHUD sh, Ray ray, List<FreeControllerV3> hitsList, bool doHighlight = true, bool includeHidden = false, bool setSelectionHUDTransform = true)
	{
		this.AllocateRaycastHits();
		int num = Physics.RaycastNonAlloc(ray, this.raycastHits, 50f, this.targetColliderMask);
		if (num > 0)
		{
			if (this.wasHitFC == null)
			{
				this.wasHitFC = new Dictionary<FreeControllerV3, bool>();
			}
			else
			{
				this.wasHitFC.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				RaycastHit raycastHit = this.raycastHits[i];
				FreeControllerV3 freeControllerV = raycastHit.transform.GetComponent<FreeControllerV3>();
				if (freeControllerV == null)
				{
					FreeControllerV3Link component = raycastHit.transform.GetComponent<FreeControllerV3Link>();
					if (component != null)
					{
						freeControllerV = component.linkedController;
					}
				}
				if (freeControllerV != null && !this.wasHitFC.ContainsKey(freeControllerV) && (this.gameMode == SuperController.GameMode.Edit || freeControllerV.interactableInPlayMode) && !freeControllerV.possessed)
				{
					if (this.onlyShowControllers == null || this.onlyShowControllers.Contains(freeControllerV))
					{
						if (!freeControllerV.hidden || !(freeControllerV.containingAtom != null) || (!freeControllerV.containingAtom.hidden && !freeControllerV.containingAtom.tempHidden))
						{
							if (!UserPreferences.singleton.hideInactiveTargets || freeControllerV.currentPositionState != FreeControllerV3.PositionState.Off || freeControllerV.currentRotationState != FreeControllerV3.RotationState.Off)
							{
								this.wasHitFC.Add(freeControllerV, true);
								if (!hitsList.Contains(freeControllerV))
								{
									hitsList.Add(freeControllerV);
								}
							}
						}
					}
				}
			}
			if (this.wasHitFC.Count == 0 && UserPreferences.singleton.hideInactiveTargets)
			{
				for (int j = 0; j < num; j++)
				{
					RaycastHit raycastHit2 = this.raycastHits[j];
					FreeControllerV3 freeControllerV2 = raycastHit2.transform.GetComponent<FreeControllerV3>();
					if (freeControllerV2 == null)
					{
						FreeControllerV3Link component2 = raycastHit2.transform.GetComponent<FreeControllerV3Link>();
						if (component2 != null)
						{
							freeControllerV2 = component2.linkedController;
						}
					}
					if (freeControllerV2 != null && !this.wasHitFC.ContainsKey(freeControllerV2) && (this.gameMode == SuperController.GameMode.Edit || freeControllerV2.interactableInPlayMode) && !freeControllerV2.possessed)
					{
						if (this.onlyShowControllers == null || this.onlyShowControllers.Contains(freeControllerV2))
						{
							if (!freeControllerV2.hidden || !(freeControllerV2.containingAtom != null) || (!freeControllerV2.containingAtom.hidden && !freeControllerV2.containingAtom.tempHidden))
							{
								this.wasHitFC.Add(freeControllerV2, true);
								if (!hitsList.Contains(freeControllerV2))
								{
									hitsList.Add(freeControllerV2);
								}
							}
						}
					}
				}
			}
			FreeControllerV3[] array = hitsList.ToArray();
			foreach (FreeControllerV3 freeControllerV3 in array)
			{
				if (!this.wasHitFC.ContainsKey(freeControllerV3))
				{
					freeControllerV3.highlighted = false;
					hitsList.Remove(freeControllerV3);
				}
			}
			if (doHighlight)
			{
				for (int l = 0; l < hitsList.Count; l++)
				{
					FreeControllerV3 freeControllerV4 = hitsList[l];
					if (l == 0)
					{
						freeControllerV4.highlighted = true;
					}
					else
					{
						freeControllerV4.highlighted = false;
					}
				}
			}
			if (sh != null)
			{
				sh.ClearSelections();
				if (hitsList.Count > 0)
				{
					int num2 = 0;
					foreach (FreeControllerV3 freeControllerV5 in hitsList)
					{
						string name;
						if (this.gameMode == SuperController.GameMode.Play && setSelectionHUDTransform)
						{
							name = string.Empty;
						}
						else
						{
							name = freeControllerV5.containingAtom.uid + ":" + freeControllerV5.name;
						}
						sh.SetSelection(num2, freeControllerV5.transform, name);
						num2++;
					}
				}
			}
		}
		else
		{
			if (doHighlight)
			{
				foreach (FreeControllerV3 freeControllerV6 in hitsList)
				{
					freeControllerV6.highlighted = false;
				}
			}
			hitsList.Clear();
		}
		if (sh != null)
		{
			if (hitsList.Count > 0)
			{
				sh.gameObject.SetActive(true);
				if (setSelectionHUDTransform)
				{
					sh.transform.position = hitsList[0].transform.position;
					float magnitude = (sh.transform.position - this.lookCamera.transform.position).magnitude;
					Vector3 localScale;
					localScale.x = magnitude;
					localScale.y = magnitude;
					localScale.z = magnitude;
					sh.transform.localScale = localScale;
					sh.transform.LookAt(this.lookCamera.transform.position, this.lookCamera.transform.up);
				}
			}
			else
			{
				sh.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06005D62 RID: 23906 RVA: 0x0022B92C File Offset: 0x00229D2C
	private void AddPositionRotationHandlesToSelectedController()
	{
		if (this.MonitorCenterCamera != null)
		{
			if (this.selectedControllerPositionHandle != null)
			{
				this.selectedControllerPositionHandle.enabled = this._mainHUDVisible;
				this.selectedControllerPositionHandle.controller = this.selectedController;
			}
			if (this.selectedControllerRotationHandle != null)
			{
				this.selectedControllerRotationHandle.enabled = this._mainHUDVisible;
				this.selectedControllerRotationHandle.controller = this.selectedController;
			}
		}
	}

	// Token: 0x06005D63 RID: 23907 RVA: 0x0022B9B0 File Offset: 0x00229DB0
	private bool ProcessTargetSelectionDoSelect(List<FreeControllerV3> highlightedControllers)
	{
		bool result = false;
		if (highlightedControllers.Count > 0)
		{
			FreeControllerV3 freeControllerV = highlightedControllers[0];
			highlightedControllers.RemoveAt(0);
			highlightedControllers.Add(freeControllerV);
			if (!(this.selectedController != null) || !(this.selectedController == freeControllerV))
			{
				if (this.selectedController != null)
				{
					this.ClearSelection(true);
				}
				freeControllerV.selected = true;
				this.selectedController = freeControllerV;
				if (this.selectedController != null && this.selectedController.containingAtom != null)
				{
					this.lastCycleSelectAtomUid = this.selectedController.containingAtom.uid;
					this.lastCycleSelectAtomType = this.selectedController.containingAtom.type;
				}
				this.AddPositionRotationHandlesToSelectedController();
				this.SyncUIToSelectedController();
				this.activeUI = SuperController.ActiveUI.SelectedOptions;
				result = true;
			}
		}
		else
		{
			this.ClearSelection(true);
		}
		return result;
	}

	// Token: 0x06005D64 RID: 23908 RVA: 0x0022BAA3 File Offset: 0x00229EA3
	public void ProcessTargetSelectionCycleSelectMouse()
	{
		this.ProcessTargetSelectionCycleSelect(this.highlightedControllersMouse);
	}

	// Token: 0x06005D65 RID: 23909 RVA: 0x0022BAB4 File Offset: 0x00229EB4
	private void ProcessTargetSelectionCycleSelect(List<FreeControllerV3> highlightedControllers)
	{
		if (highlightedControllers != null && highlightedControllers.Count > 1)
		{
			FreeControllerV3 item = highlightedControllers[0];
			highlightedControllers.RemoveAt(0);
			highlightedControllers.Add(item);
		}
	}

	// Token: 0x06005D66 RID: 23910 RVA: 0x0022BAEC File Offset: 0x00229EEC
	private void ProcessTargetSelectionCycleBackwardsSelect(List<FreeControllerV3> highlightedControllers)
	{
		if (highlightedControllers != null && highlightedControllers.Count > 1)
		{
			int index = highlightedControllers.Count - 1;
			FreeControllerV3 item = highlightedControllers[index];
			highlightedControllers.RemoveAt(index);
			highlightedControllers.Insert(0, item);
		}
	}

	// Token: 0x06005D67 RID: 23911 RVA: 0x0022BB2C File Offset: 0x00229F2C
	public List<FreeControllerV3> GetOverlappingTargets(Transform processFrom, float overlapRadius = 0.01f)
	{
		if (this.overlappingFcs == null)
		{
			this.overlappingFcs = new List<FreeControllerV3>();
		}
		else
		{
			this.overlappingFcs.Clear();
		}
		this.AllocateOverlappingControls();
		int num = Physics.OverlapSphereNonAlloc(processFrom.position, overlapRadius * this._worldScale, this.overlappingControls, this.targetColliderMask);
		if (num > 0)
		{
			bool flag = false;
			for (int i = 0; i < num; i++)
			{
				Collider collider = this.overlappingControls[i];
				FreeControllerV3 component = collider.GetComponent<FreeControllerV3>();
				if (component != null && (this.gameMode == SuperController.GameMode.Edit || component.interactableInPlayMode) && !component.possessed && (component.currentPositionState == FreeControllerV3.PositionState.On || component.currentRotationState == FreeControllerV3.RotationState.On))
				{
					if (this.onlyShowControllers == null || this.onlyShowControllers.Contains(component))
					{
						if (!(component.containingAtom != null) || (!component.containingAtom.hidden && !component.containingAtom.tempHidden))
						{
							flag = true;
							if (!this.overlappingFcs.Contains(component))
							{
								this.overlappingFcs.Add(component);
							}
							float distanceHolder = Vector3.SqrMagnitude(processFrom.position - collider.transform.position);
							component.distanceHolder = distanceHolder;
						}
					}
				}
			}
			if (!flag)
			{
				for (int j = 0; j < num; j++)
				{
					Collider collider2 = this.overlappingControls[j];
					FreeControllerV3 freeControllerV = null;
					FreeControllerV3Link component2 = collider2.GetComponent<FreeControllerV3Link>();
					if (component2 != null)
					{
						freeControllerV = component2.linkedController;
					}
					if (freeControllerV != null && (this.gameMode == SuperController.GameMode.Edit || freeControllerV.interactableInPlayMode) && !freeControllerV.possessed && (freeControllerV.currentPositionState == FreeControllerV3.PositionState.On || freeControllerV.currentRotationState == FreeControllerV3.RotationState.On))
					{
						if (this.onlyShowControllers == null || this.onlyShowControllers.Contains(freeControllerV))
						{
							if (!(freeControllerV.containingAtom != null) || (!freeControllerV.containingAtom.hidden && !freeControllerV.containingAtom.tempHidden))
							{
								if (!this.overlappingFcs.Contains(freeControllerV))
								{
									this.overlappingFcs.Add(freeControllerV);
								}
								float distanceHolder2 = Vector3.SqrMagnitude(processFrom.position - collider2.transform.position);
								freeControllerV.distanceHolder = distanceHolder2;
							}
						}
					}
				}
			}
			if (!flag)
			{
				for (int k = 0; k < num; k++)
				{
					Collider collider3 = this.overlappingControls[k];
					FreeControllerV3 component3 = collider3.GetComponent<FreeControllerV3>();
					if (component3 != null && (this.gameMode == SuperController.GameMode.Edit || component3.interactableInPlayMode) && !component3.possessed)
					{
						if (this.onlyShowControllers == null || this.onlyShowControllers.Contains(component3))
						{
							if (!component3.hidden || !(component3.containingAtom != null) || (!component3.containingAtom.hidden && !component3.containingAtom.tempHidden))
							{
								flag = true;
								if (!this.overlappingFcs.Contains(component3))
								{
									this.overlappingFcs.Add(component3);
								}
								float distanceHolder3 = Vector3.SqrMagnitude(processFrom.position - collider3.transform.position);
								component3.distanceHolder = distanceHolder3;
							}
						}
					}
				}
			}
			if (!flag)
			{
				for (int l = 0; l < num; l++)
				{
					Collider collider4 = this.overlappingControls[l];
					FreeControllerV3 freeControllerV2 = null;
					FreeControllerV3Link component4 = collider4.GetComponent<FreeControllerV3Link>();
					if (component4 != null)
					{
						freeControllerV2 = component4.linkedController;
					}
					if (freeControllerV2 != null && (this.gameMode == SuperController.GameMode.Edit || freeControllerV2.interactableInPlayMode) && !freeControllerV2.possessed)
					{
						if (this.onlyShowControllers == null || this.onlyShowControllers.Contains(freeControllerV2))
						{
							if (!freeControllerV2.hidden || !(freeControllerV2.containingAtom != null) || (!freeControllerV2.containingAtom.hidden && !freeControllerV2.containingAtom.tempHidden))
							{
								if (!this.overlappingFcs.Contains(freeControllerV2))
								{
									this.overlappingFcs.Add(freeControllerV2);
								}
								float distanceHolder4 = Vector3.SqrMagnitude(processFrom.position - collider4.transform.position);
								freeControllerV2.distanceHolder = distanceHolder4;
							}
						}
					}
				}
			}
		}
		return this.overlappingFcs;
	}

	// Token: 0x06005D68 RID: 23912 RVA: 0x0022C00D File Offset: 0x0022A40D
	protected void AllocateOverlappingControls()
	{
		if (this.overlappingControls == null)
		{
			this.overlappingControls = new Collider[256];
		}
	}

	// Token: 0x06005D69 RID: 23913 RVA: 0x0022C02C File Offset: 0x0022A42C
	public bool ProcessControllerTargetHighlight(SelectionHUD sh, Transform processFromPointer, Transform processFromOverlap, bool ptrMode, List<FreeControllerV3> highlightedControllers, bool uihit, FreeControllerV3 excludeController, out bool isOverlap, float overlapRadius = 0.03f)
	{
		if (this.overlappingFcs == null)
		{
			this.overlappingFcs = new List<FreeControllerV3>();
		}
		else
		{
			this.overlappingFcs.Clear();
		}
		bool result = false;
		this.AllocateOverlappingControls();
		int num = Physics.OverlapSphereNonAlloc(processFromOverlap.position, overlapRadius * this._worldScale, this.overlappingControls, this.targetColliderMask);
		if (num > 0)
		{
			bool flag = false;
			for (int i = 0; i < num; i++)
			{
				Collider collider = this.overlappingControls[i];
				FreeControllerV3 component = collider.GetComponent<FreeControllerV3>();
				if (component != null && (this.gameMode == SuperController.GameMode.Edit || component.interactableInPlayMode) && !component.possessed && component.hidden && (component.currentPositionState == FreeControllerV3.PositionState.On || component.currentRotationState == FreeControllerV3.RotationState.On))
				{
					if (!(excludeController != null) || !(component == excludeController))
					{
						if (this.onlyShowControllers == null || this.onlyShowControllers.Contains(component))
						{
							if (!(component.containingAtom != null) || (!component.containingAtom.hidden && !component.containingAtom.tempHidden))
							{
								flag = true;
								if (!this.overlappingFcs.Contains(component))
								{
									this.overlappingFcs.Add(component);
								}
								float distanceHolder = Vector3.SqrMagnitude(processFromOverlap.position - collider.transform.position);
								component.distanceHolder = distanceHolder;
							}
						}
					}
				}
			}
			if (!flag)
			{
				for (int j = 0; j < num; j++)
				{
					Collider collider2 = this.overlappingControls[j];
					FreeControllerV3 freeControllerV = null;
					FreeControllerV3Link component2 = collider2.GetComponent<FreeControllerV3Link>();
					if (component2 != null)
					{
						freeControllerV = component2.linkedController;
					}
					if (freeControllerV != null && (this.gameMode == SuperController.GameMode.Edit || freeControllerV.interactableInPlayMode) && !freeControllerV.possessed && freeControllerV.hidden && (freeControllerV.currentPositionState == FreeControllerV3.PositionState.On || freeControllerV.currentRotationState == FreeControllerV3.RotationState.On))
					{
						if (!(excludeController != null) || !(freeControllerV == excludeController))
						{
							if (this.onlyShowControllers == null || this.onlyShowControllers.Contains(freeControllerV))
							{
								if (!(freeControllerV.containingAtom != null) || (!freeControllerV.containingAtom.hidden && !freeControllerV.containingAtom.tempHidden))
								{
									if (!this.overlappingFcs.Contains(freeControllerV))
									{
										this.overlappingFcs.Add(freeControllerV);
									}
									float distanceHolder2 = Vector3.SqrMagnitude(processFromOverlap.position - collider2.transform.position);
									freeControllerV.distanceHolder = distanceHolder2;
								}
							}
						}
					}
				}
			}
			if (!flag)
			{
				for (int k = 0; k < num; k++)
				{
					Collider collider3 = this.overlappingControls[k];
					FreeControllerV3 component3 = collider3.GetComponent<FreeControllerV3>();
					if (component3 != null && (this.gameMode == SuperController.GameMode.Edit || component3.interactableInPlayMode) && !component3.possessed)
					{
						if (!(excludeController != null) || !(component3 == excludeController))
						{
							if (this.onlyShowControllers == null || this.onlyShowControllers.Contains(component3))
							{
								if (!component3.hidden || !(component3.containingAtom != null) || (!component3.containingAtom.hidden && !component3.containingAtom.tempHidden))
								{
									flag = true;
									if (!this.overlappingFcs.Contains(component3))
									{
										this.overlappingFcs.Add(component3);
									}
									float distanceHolder3 = Vector3.SqrMagnitude(processFromOverlap.position - collider3.transform.position);
									component3.distanceHolder = distanceHolder3;
								}
							}
						}
					}
				}
			}
			if (!flag)
			{
				for (int l = 0; l < num; l++)
				{
					Collider collider4 = this.overlappingControls[l];
					FreeControllerV3 freeControllerV2 = null;
					FreeControllerV3Link component4 = collider4.GetComponent<FreeControllerV3Link>();
					if (component4 != null)
					{
						freeControllerV2 = component4.linkedController;
					}
					if (freeControllerV2 != null && (this.gameMode == SuperController.GameMode.Edit || freeControllerV2.interactableInPlayMode) && !freeControllerV2.possessed)
					{
						if (!(excludeController != null) || !(freeControllerV2 == excludeController))
						{
							if (this.onlyShowControllers == null || this.onlyShowControllers.Contains(freeControllerV2))
							{
								if (!freeControllerV2.hidden || !(freeControllerV2.containingAtom != null) || (!freeControllerV2.containingAtom.hidden && !freeControllerV2.containingAtom.tempHidden))
								{
									if (!this.overlappingFcs.Contains(freeControllerV2))
									{
										this.overlappingFcs.Add(freeControllerV2);
									}
									float distanceHolder4 = Vector3.SqrMagnitude(processFromOverlap.position - collider4.transform.position);
									freeControllerV2.distanceHolder = distanceHolder4;
								}
							}
						}
					}
				}
			}
		}
		if (sh != null)
		{
			sh.ClearSelections();
			if (this.overlappingFcs.Count > 0)
			{
				highlightedControllers.Clear();
				if (this.alreadyDisplayed == null)
				{
					this.alreadyDisplayed = new List<FreeControllerV3>();
				}
				else
				{
					this.alreadyDisplayed.Clear();
				}
				List<FreeControllerV3> list = this.overlappingFcs;
				if (SuperController.<>f__am$cache0 == null)
				{
					SuperController.<>f__am$cache0 = new Comparison<FreeControllerV3>(SuperController.<ProcessControllerTargetHighlight>m__30);
				}
				list.Sort(SuperController.<>f__am$cache0);
				if (this.gameMode == SuperController.GameMode.Edit)
				{
					sh.gameObject.SetActive(true);
				}
				else
				{
					sh.gameObject.SetActive(false);
				}
				sh.useDrawFromPosition = true;
				sh.drawFrom = processFromOverlap.position;
				int num2 = 0;
				foreach (FreeControllerV3 freeControllerV3 in this.overlappingFcs)
				{
					if (!this.alreadyDisplayed.Contains(freeControllerV3))
					{
						if (num2 == 0)
						{
							highlightedControllers.Add(freeControllerV3);
						}
						string name;
						if (this.gameMode == SuperController.GameMode.Play)
						{
							name = string.Empty;
						}
						else
						{
							name = freeControllerV3.containingAtom.uid + ":" + freeControllerV3.name;
						}
						sh.SetSelection(num2, freeControllerV3.transform, name);
						num2++;
						this.alreadyDisplayed.Add(freeControllerV3);
					}
				}
				sh.transform.position = processFromOverlap.position;
				float magnitude = (sh.transform.position - this.lookCamera.transform.position).magnitude;
				Vector3 localScale;
				localScale.x = magnitude;
				localScale.y = magnitude;
				localScale.z = magnitude;
				sh.transform.localScale = localScale;
				sh.transform.LookAt(this.lookCamera.transform.position, this.lookCamera.transform.up);
			}
			else if (!ptrMode)
			{
				highlightedControllers.Clear();
				sh.gameObject.SetActive(false);
			}
		}
		if ((this.overlappingFcs.Count == 0 && ptrMode) || this.pointersAlwaysEnabled)
		{
			result = !this.MonitorRigActive;
		}
		isOverlap = (this.overlappingFcs.Count != 0);
		if (this.overlappingFcs.Count == 0 && ptrMode)
		{
			this.castRay.origin = processFromPointer.position;
			this.castRay.direction = processFromPointer.forward;
			sh.useDrawFromPosition = true;
			sh.drawFrom = processFromPointer.position;
			if (!uihit)
			{
				this.ProcessTargetSelectionDoRaycast(sh, this.castRay, highlightedControllers, false, false, true);
			}
		}
		return result;
	}

	// Token: 0x06005D6A RID: 23914 RVA: 0x0022C878 File Offset: 0x0022AC78
	private void ClearAllGrabbedControllers()
	{
		if (this.leftFullGrabbedController != null)
		{
			this.leftFullGrabbedController.isGrabbing = false;
			this.leftFullGrabbedController.RestorePreLinkState();
			this.leftFullGrabbedController = null;
		}
		if (this.leftGrabbedController != null)
		{
			this.leftGrabbedController.isGrabbing = false;
			this.leftGrabbedController.RestorePreLinkState();
			this.leftGrabbedController = null;
		}
		if (this.rightFullGrabbedController != null)
		{
			this.rightFullGrabbedController.isGrabbing = false;
			this.rightFullGrabbedController.RestorePreLinkState();
			this.rightFullGrabbedController = null;
		}
		if (this.rightGrabbedController != null)
		{
			this.rightGrabbedController.isGrabbing = false;
			this.rightGrabbedController.RestorePreLinkState();
			this.rightGrabbedController = null;
		}
		if (this.grabbedControllerMouse != null)
		{
			this.grabbedControllerMouse.isGrabbing = false;
			this.grabbedControllerMouse.RestorePreLinkState();
			this.grabbedControllerMouse = null;
		}
	}

	// Token: 0x06005D6B RID: 23915 RVA: 0x0022C970 File Offset: 0x0022AD70
	private bool ProcessTargetSelectionDoGrabRight(Transform rightControl, bool isRemote)
	{
		bool result = false;
		if (this.rightGrabbedController)
		{
			this.rightGrabbedController.RestorePreLinkState();
			this.rightGrabbedController.isGrabbing = false;
			this.rightGrabbedController = null;
		}
		FreeControllerV3 freeControllerV = null;
		for (int i = 0; i < this.highlightedControllersRight.Count; i++)
		{
			FreeControllerV3 freeControllerV2 = this.highlightedControllersRight[i];
			if (freeControllerV2.canGrabPosition || freeControllerV2.canGrabRotation)
			{
				if (!(this.rightFullGrabbedController != null) || !(this.rightFullGrabbedController == freeControllerV2))
				{
					freeControllerV = freeControllerV2;
					this.highlightedControllersRight.RemoveAt(i);
					this.highlightedControllersRight.Add(freeControllerV);
					break;
				}
			}
		}
		if (freeControllerV != null)
		{
			Rigidbody component = rightControl.GetComponent<Rigidbody>();
			this.rightGrabbedController = freeControllerV;
			if (this.playerNavCollider != null && this.playerNavCollider.underlyingControl == freeControllerV)
			{
				this.DisconnectNavRigFromPlayerNavCollider();
			}
			if (this.leftFullGrabbedController == this.rightGrabbedController)
			{
				this.leftFullGrabbedController.RestorePreLinkState();
				this.leftFullGrabbedController = null;
				this.leftHandControl = null;
			}
			if (this.leftGrabbedController == this.rightGrabbedController)
			{
				this.leftGrabbedController.RestorePreLinkState();
				this.leftGrabbedController = null;
			}
			if (component != null)
			{
				bool flag = true;
				FreeControllerV3.SelectLinkState linkState = FreeControllerV3.SelectLinkState.Position;
				if (this.rightGrabbedController.canGrabPosition)
				{
					if (this.rightGrabbedController.canGrabRotation)
					{
						linkState = FreeControllerV3.SelectLinkState.PositionAndRotation;
					}
				}
				else if (this.rightGrabbedController.canGrabRotation)
				{
					linkState = FreeControllerV3.SelectLinkState.Rotation;
				}
				else
				{
					flag = false;
				}
				if (flag)
				{
					result = true;
					this.rightGrabbedController.isGrabbing = true;
					this.rightGrabbedControllerIsRemote = isRemote;
					if (this.rightFullGrabbedController != null)
					{
						Rigidbody followWhenOffRB = this.rightFullGrabbedController.followWhenOffRB;
						if (followWhenOffRB != null)
						{
							this.rightGrabbedController.SelectLinkToRigidbody(followWhenOffRB, linkState, true, true);
						}
						else
						{
							this.rightGrabbedController.SelectLinkToRigidbody(component, linkState, false, true);
						}
					}
					else
					{
						this.rightGrabbedController.SelectLinkToRigidbody(component, linkState, false, true);
					}
				}
			}
		}
		return result;
	}

	// Token: 0x06005D6C RID: 23916 RVA: 0x0022CBB0 File Offset: 0x0022AFB0
	private bool ProcessTargetSelectionDoFullGrabRight(Transform rightControl, bool isRemote, bool isRemoteGrab)
	{
		bool result = false;
		FreeControllerV3 freeControllerV = null;
		for (int i = 0; i < this.highlightedControllersRight.Count; i++)
		{
			FreeControllerV3 freeControllerV2 = this.highlightedControllersRight[i];
			if (freeControllerV2.canGrabPosition || freeControllerV2.canGrabRotation)
			{
				freeControllerV = freeControllerV2;
				this.highlightedControllersRight.RemoveAt(i);
				this.highlightedControllersRight.Add(freeControllerV);
				break;
			}
		}
		if (freeControllerV != null && ((isRemote && isRemoteGrab) || (!isRemote && !isRemoteGrab)))
		{
			Rigidbody component = rightControl.GetComponent<Rigidbody>();
			this.rightFullGrabbedController = freeControllerV;
			if (this.playerNavCollider != null && this.playerNavCollider.underlyingControl == freeControllerV)
			{
				this.DisconnectNavRigFromPlayerNavCollider();
			}
			if (this.leftFullGrabbedController == this.rightFullGrabbedController)
			{
				this.leftFullGrabbedController.RestorePreLinkState();
				this.leftFullGrabbedController = null;
				if (this.leftHandControl != null)
				{
					this.leftHandControl.possessed = false;
				}
				this.leftHandControl = null;
			}
			if (this.leftGrabbedController == this.rightFullGrabbedController)
			{
				this.leftGrabbedController.RestorePreLinkState();
				this.leftGrabbedController = null;
			}
			if (this.rightGrabbedController == this.rightFullGrabbedController)
			{
				this.rightGrabbedController.RestorePreLinkState();
				this.rightGrabbedController = null;
			}
			if (component != null)
			{
				bool flag = true;
				FreeControllerV3.SelectLinkState linkState = FreeControllerV3.SelectLinkState.Position;
				if (this.rightFullGrabbedController.canGrabPosition)
				{
					if (this.rightFullGrabbedController.canGrabRotation)
					{
						linkState = FreeControllerV3.SelectLinkState.PositionAndRotation;
					}
				}
				else if (this.rightFullGrabbedController.canGrabRotation)
				{
					linkState = FreeControllerV3.SelectLinkState.Rotation;
				}
				else
				{
					flag = false;
				}
				if (flag)
				{
					result = true;
					this.rightFullGrabbedController.isGrabbing = true;
					this.rightFullGrabbedControllerIsRemote = isRemote;
					this.rightFullGrabbedController.SelectLinkToRigidbody(component, linkState, false, true);
					this.rightHandControl = this.rightFullGrabbedController.GetComponent<HandControl>();
					if (this.rightHandControl == null)
					{
						HandControlLink component2 = this.rightFullGrabbedController.GetComponent<HandControlLink>();
						if (component2 != null)
						{
							this.rightHandControl = component2.handControl;
						}
					}
					if (this.rightHandControl != null)
					{
						this.rightHandControl.possessed = true;
					}
				}
			}
		}
		else if (this.rightGrabbedController != null && ((this.rightGrabbedControllerIsRemote && isRemoteGrab) || (!this.rightGrabbedControllerIsRemote && !isRemoteGrab)))
		{
			result = true;
			this.rightFullGrabbedControllerIsRemote = this.rightGrabbedControllerIsRemote;
			this.rightFullGrabbedController = this.rightGrabbedController;
			if (this.playerNavCollider != null && this.playerNavCollider.underlyingControl == this.rightFullGrabbedController)
			{
				this.DisconnectNavRigFromPlayerNavCollider();
			}
			this.rightHandControl = this.rightFullGrabbedController.GetComponent<HandControl>();
			if (this.rightHandControl == null)
			{
				HandControlLink component3 = this.rightFullGrabbedController.GetComponent<HandControlLink>();
				if (component3 != null)
				{
					this.rightHandControl = component3.handControl;
				}
			}
			if (this.rightHandControl != null)
			{
				this.rightHandControl.possessed = true;
			}
			this.rightGrabbedController = null;
		}
		return result;
	}

	// Token: 0x06005D6D RID: 23917 RVA: 0x0022CEF8 File Offset: 0x0022B2F8
	private bool ProcessTargetSelectionDoGrabLeft(Transform leftControl, bool isRemote)
	{
		bool result = false;
		if (this.leftGrabbedController)
		{
			this.leftGrabbedController.isGrabbing = false;
			this.leftGrabbedController.RestorePreLinkState();
			this.leftGrabbedController = null;
		}
		FreeControllerV3 freeControllerV = null;
		for (int i = 0; i < this.highlightedControllersLeft.Count; i++)
		{
			FreeControllerV3 freeControllerV2 = this.highlightedControllersLeft[i];
			if (freeControllerV2.canGrabPosition || freeControllerV2.canGrabRotation)
			{
				if (!(this.leftFullGrabbedController != null) || !(this.leftFullGrabbedController == freeControllerV2))
				{
					freeControllerV = freeControllerV2;
					this.highlightedControllersLeft.RemoveAt(i);
					this.highlightedControllersLeft.Add(freeControllerV);
					break;
				}
			}
		}
		if (freeControllerV != null)
		{
			Rigidbody component = leftControl.GetComponent<Rigidbody>();
			this.leftGrabbedController = freeControllerV;
			if (this.playerNavCollider != null && this.playerNavCollider.underlyingControl == freeControllerV)
			{
				this.DisconnectNavRigFromPlayerNavCollider();
			}
			if (this.rightFullGrabbedController == this.leftGrabbedController)
			{
				this.rightFullGrabbedController.RestorePreLinkState();
				this.rightFullGrabbedController = null;
				if (this.rightHandControl != null)
				{
					this.rightHandControl.possessed = false;
				}
				this.rightHandControl = null;
			}
			if (this.rightGrabbedController == this.leftGrabbedController)
			{
				this.rightGrabbedController.RestorePreLinkState();
				this.rightGrabbedController = null;
			}
			if (component != null)
			{
				bool flag = true;
				FreeControllerV3.SelectLinkState linkState = FreeControllerV3.SelectLinkState.Position;
				if (this.leftGrabbedController.canGrabPosition)
				{
					if (this.leftGrabbedController.canGrabRotation)
					{
						linkState = FreeControllerV3.SelectLinkState.PositionAndRotation;
					}
				}
				else if (this.leftGrabbedController.canGrabRotation)
				{
					linkState = FreeControllerV3.SelectLinkState.Rotation;
				}
				else
				{
					flag = false;
				}
				if (flag)
				{
					result = true;
					this.leftGrabbedControllerIsRemote = isRemote;
					this.leftGrabbedController.isGrabbing = true;
					if (this.leftFullGrabbedController != null)
					{
						Rigidbody followWhenOffRB = this.leftFullGrabbedController.followWhenOffRB;
						if (followWhenOffRB != null)
						{
							this.leftGrabbedController.SelectLinkToRigidbody(followWhenOffRB, linkState, true, true);
						}
						else
						{
							this.leftGrabbedController.SelectLinkToRigidbody(component, linkState, false, true);
						}
					}
					else
					{
						this.leftGrabbedController.SelectLinkToRigidbody(component, linkState, false, true);
					}
				}
			}
		}
		return result;
	}

	// Token: 0x06005D6E RID: 23918 RVA: 0x0022D154 File Offset: 0x0022B554
	private bool ProcessTargetSelectionDoFullGrabLeft(Transform leftControl, bool isRemote, bool isRemoteGrab)
	{
		bool result = false;
		FreeControllerV3 freeControllerV = null;
		for (int i = 0; i < this.highlightedControllersLeft.Count; i++)
		{
			FreeControllerV3 freeControllerV2 = this.highlightedControllersLeft[i];
			if (freeControllerV2.canGrabPosition || freeControllerV2.canGrabRotation)
			{
				freeControllerV = freeControllerV2;
				this.highlightedControllersLeft.RemoveAt(i);
				this.highlightedControllersLeft.Add(freeControllerV);
				break;
			}
		}
		if (freeControllerV != null && ((isRemote && isRemoteGrab) || (!isRemote && !isRemoteGrab)))
		{
			Rigidbody component = leftControl.GetComponent<Rigidbody>();
			this.leftFullGrabbedController = freeControllerV;
			if (this.playerNavCollider != null && this.playerNavCollider.underlyingControl == freeControllerV)
			{
				this.DisconnectNavRigFromPlayerNavCollider();
			}
			if (this.rightFullGrabbedController == this.leftFullGrabbedController)
			{
				this.rightFullGrabbedController.RestorePreLinkState();
				this.rightFullGrabbedController = null;
				if (this.rightHandControl != null)
				{
					this.rightHandControl.possessed = false;
				}
				this.rightHandControl = null;
			}
			if (this.rightGrabbedController == this.leftFullGrabbedController)
			{
				this.rightGrabbedController.RestorePreLinkState();
				this.rightGrabbedController = null;
			}
			if (this.leftGrabbedController == this.leftFullGrabbedController)
			{
				this.leftGrabbedController.RestorePreLinkState();
				this.leftGrabbedController = null;
			}
			if (component != null)
			{
				bool flag = true;
				FreeControllerV3.SelectLinkState linkState = FreeControllerV3.SelectLinkState.Position;
				if (this.leftFullGrabbedController.canGrabPosition)
				{
					if (this.leftFullGrabbedController.canGrabRotation)
					{
						linkState = FreeControllerV3.SelectLinkState.PositionAndRotation;
					}
				}
				else if (this.leftFullGrabbedController.canGrabRotation)
				{
					linkState = FreeControllerV3.SelectLinkState.Rotation;
				}
				else
				{
					flag = false;
				}
				if (flag)
				{
					result = true;
					this.leftFullGrabbedController.isGrabbing = true;
					this.leftFullGrabbedControllerIsRemote = isRemote;
					this.leftFullGrabbedController.SelectLinkToRigidbody(component, linkState, false, true);
					this.leftHandControl = this.leftFullGrabbedController.GetComponent<HandControl>();
					if (this.leftHandControl == null)
					{
						HandControlLink component2 = this.leftFullGrabbedController.GetComponent<HandControlLink>();
						if (component2 != null)
						{
							this.leftHandControl = component2.handControl;
						}
					}
					if (this.leftHandControl != null)
					{
						this.leftHandControl.possessed = true;
					}
				}
			}
		}
		else if (this.leftGrabbedController != null && ((this.leftGrabbedControllerIsRemote && isRemoteGrab) || (!this.leftGrabbedControllerIsRemote && !isRemoteGrab)))
		{
			result = true;
			this.leftFullGrabbedControllerIsRemote = this.leftGrabbedControllerIsRemote;
			this.leftFullGrabbedController = this.leftGrabbedController;
			if (this.playerNavCollider != null && this.playerNavCollider.underlyingControl == this.leftFullGrabbedController)
			{
				this.DisconnectNavRigFromPlayerNavCollider();
			}
			this.leftGrabbedController = null;
			this.leftHandControl = this.leftFullGrabbedController.GetComponent<HandControl>();
			if (this.leftHandControl == null)
			{
				HandControlLink component3 = this.leftFullGrabbedController.GetComponent<HandControlLink>();
				if (component3 != null)
				{
					this.leftHandControl = component3.handControl;
				}
			}
			if (this.leftHandControl != null)
			{
				this.leftHandControl.possessed = true;
			}
		}
		return result;
	}

	// Token: 0x17000DBB RID: 3515
	// (get) Token: 0x06005D6F RID: 23919 RVA: 0x0022D49A File Offset: 0x0022B89A
	// (set) Token: 0x06005D70 RID: 23920 RVA: 0x0022D4A4 File Offset: 0x0022B8A4
	public bool allowGrabPlusTriggerHandToggle
	{
		get
		{
			return this._allowGrabPlusTriggerHandToggle;
		}
		set
		{
			if (this._allowGrabPlusTriggerHandToggle != value)
			{
				this._allowGrabPlusTriggerHandToggle = value;
				if (this.allowGrabPlusTriggerHandToggleToggle != null)
				{
					this.allowGrabPlusTriggerHandToggleToggle.isOn = value;
				}
				if (UserPreferences.singleton != null)
				{
					UserPreferences.singleton.SavePreferences();
				}
			}
		}
	}

	// Token: 0x06005D71 RID: 23921 RVA: 0x0022D4FC File Offset: 0x0022B8FC
	private void ProcessCycle()
	{
		this.leftCycleX = 0;
		this.leftCycleY = 0;
		this.rightCycleX = 0;
		this.rightCycleY = 0;
		if (this.isOpenVR)
		{
			Vector2 axis = this.cycleUsingXAxisAction.GetAxis(SteamVR_Input_Sources.LeftHand);
			Vector2 axis2 = this.cycleUsingYAxisAction.GetAxis(SteamVR_Input_Sources.LeftHand);
			if (this.cycleEngageAction.GetStateDown(SteamVR_Input_Sources.LeftHand))
			{
				this._leftCycleOn = true;
				this._leftCycleXPosition = axis.x;
				this._leftCycleYPosition = axis2.y;
			}
			if (this.cycleEngageAction.GetStateUp(SteamVR_Input_Sources.LeftHand))
			{
				this._leftCycleOn = false;
			}
			if (this._leftCycleOn)
			{
				float num = axis.x - this._leftCycleXPosition;
				if ((num > 0f && num > this.cycleClick) || (num < 0f && -num > this.cycleClick))
				{
					this.leftCycleX = (int)(num / this.cycleClick);
					this._leftCycleXPosition = axis.x;
				}
				float num2 = axis2.y - this._leftCycleYPosition;
				if ((num2 > 0f && num2 > this.cycleClick) || (num2 < 0f && -num2 > this.cycleClick))
				{
					this.leftCycleY = (int)(num2 / this.cycleClick);
					this._leftCycleYPosition = axis2.y;
				}
			}
			Vector2 axis3 = this.cycleUsingXAxisAction.GetAxis(SteamVR_Input_Sources.RightHand);
			Vector2 axis4 = this.cycleUsingYAxisAction.GetAxis(SteamVR_Input_Sources.RightHand);
			if (this.cycleEngageAction.GetStateDown(SteamVR_Input_Sources.RightHand))
			{
				this._rightCycleOn = true;
				this._rightCycleXPosition = axis3.x;
				this._rightCycleYPosition = axis4.y;
			}
			if (this.cycleEngageAction.GetStateUp(SteamVR_Input_Sources.RightHand))
			{
				this._rightCycleOn = false;
			}
			if (this._rightCycleOn)
			{
				float num3 = axis3.x - this._rightCycleXPosition;
				if ((num3 > 0f && num3 > this.cycleClick) || (num3 < 0f && -num3 > this.cycleClick))
				{
					this.rightCycleX = (int)(num3 / this.cycleClick);
					this._rightCycleXPosition = axis3.x;
				}
				float num4 = axis4.y - this._rightCycleYPosition;
				if ((num4 > 0f && num4 > this.cycleClick) || (num4 < 0f && -num4 > this.cycleClick))
				{
					this.rightCycleY = (int)(num4 / this.cycleClick);
					this._rightCycleYPosition = axis4.y;
				}
			}
		}
	}

	// Token: 0x06005D72 RID: 23922 RVA: 0x0022D784 File Offset: 0x0022BB84
	private void ProcessMotionControllerTargetHighlight()
	{
		if (this.isMonitorOnly)
		{
			return;
		}
		if (this.highlightedControllersLeft == null)
		{
			this.highlightedControllersLeft = new List<FreeControllerV3>();
		}
		if (this.highlightedControllersRight == null)
		{
			this.highlightedControllersRight = new List<FreeControllerV3>();
		}
		this.centerHandLeft.rotation = this.motionControllerLeft.rotation;
		if (this.ProcessControllerTargetHighlight(this.leftSelectionHUD, this.motionControllerLeft, this.centerHandLeft, this._pointerModeLeft, this.highlightedControllersLeft, this.GUIhitLeft, this.leftFullGrabbedController, out this.isLeftOverlap, 0.03f))
		{
			this.drawRayLineLeft = !this.MonitorRigActive;
		}
		if (this.leftGrabbedController != null || this.leftFullGrabbedController != null || this.leftPossessedController != null)
		{
			this.HideLeftController();
			this.leftSelectionHUD.gameObject.SetActive(false);
		}
		else if (this._mainHUDVisible || !UserPreferences.singleton.showControllersMenuOnly)
		{
			this.ShowLeftController();
		}
		else
		{
			this.HideLeftController();
		}
		this.centerHandRight.rotation = this.motionControllerRight.rotation;
		if (this.ProcessControllerTargetHighlight(this.rightSelectionHUD, this.motionControllerRight, this.centerHandRight, this._pointerModeRight, this.highlightedControllersRight, this.GUIhitRight, this.rightFullGrabbedController, out this.isRightOverlap, 0.03f))
		{
			this.drawRayLineRight = !this.MonitorRigActive;
		}
		if (this.rightGrabbedController != null || this.rightFullGrabbedController != null || this.rightPossessedController != null)
		{
			this.HideRightController();
			this.rightSelectionHUD.gameObject.SetActive(false);
		}
		else if (this._mainHUDVisible || !UserPreferences.singleton.showControllersMenuOnly)
		{
			this.ShowRightController();
		}
		else
		{
			this.HideRightController();
		}
	}

	// Token: 0x06005D73 RID: 23923 RVA: 0x0022D988 File Offset: 0x0022BD88
	private void ProcessMotionControllerTargetControl(bool canSelect = true)
	{
		if (this.isMonitorOnly)
		{
			return;
		}
		if (canSelect && !this.didStartRightNavigate && this.GetRightSelect())
		{
			this.ProcessTargetSelectionDoSelect(this.highlightedControllersRight);
		}
		if (this.commonHandModelControl != null && !this._leapHandRightConnected && this.GetRightToggleHand())
		{
			this.commonHandModelControl.ToggleRightHandEnabled();
		}
		else
		{
			if (!this.isRightOverlap && this.GetRightRemoteGrab())
			{
				this.ProcessTargetSelectionDoGrabRight(this.motionControllerRight, true);
			}
			else if (this.isRightOverlap && this.GetRightGrab())
			{
				this.ProcessTargetSelectionDoGrabRight(this.motionControllerRight, false);
			}
			bool flag = false;
			if (this.GetRightHoldGrab())
			{
				if (this.rightFullGrabbedController)
				{
					flag = true;
					this.rightFullGrabbedController.isGrabbing = false;
					this.rightFullGrabbedController.RestorePreLinkState();
					this.rightFullGrabbedController = null;
					if (this.rightHandControl != null)
					{
						this.rightHandControl.possessed = false;
					}
					this.rightHandControl = null;
				}
				else
				{
					flag = this.ProcessTargetSelectionDoFullGrabRight(this.motionControllerRight, !this.isRightOverlap, false);
				}
			}
			if (!flag && !this.remoteHoldGrabDisabled && this.GetRightRemoteHoldGrab())
			{
				if (this.rightFullGrabbedController)
				{
					this.rightFullGrabbedController.isGrabbing = false;
					this.rightFullGrabbedController.RestorePreLinkState();
					this.rightFullGrabbedController = null;
					if (this.rightHandControl != null)
					{
						this.rightHandControl.possessed = false;
					}
					this.rightHandControl = null;
				}
				else
				{
					this.ProcessTargetSelectionDoFullGrabRight(this.motionControllerRight, !this.isRightOverlap, true);
				}
			}
		}
		if (this.isOpenVR)
		{
			Vector2 axis = this.pushPullAction.GetAxis(SteamVR_Input_Sources.RightHand);
			if (this.rightGrabbedController != null && this.rightGrabbedControllerIsRemote)
			{
				if (!Mathf.Approximately(axis.sqrMagnitude, 0f))
				{
					if (float.IsNaN(axis.y))
					{
						axis.y = 0f;
					}
					axis.y = Mathf.Clamp(axis.y, -100f, 100f);
					this.rightGrabbedController.MoveLinkConnectorTowards(this.motionControllerRight, axis.y * 0.05f);
				}
			}
			else if (this.rightFullGrabbedController != null && this.rightFullGrabbedControllerIsRemote && !Mathf.Approximately(axis.sqrMagnitude, 0f))
			{
				if (float.IsNaN(axis.y))
				{
					axis.y = 0f;
				}
				axis.y = Mathf.Clamp(axis.y, -100f, 100f);
				this.rightFullGrabbedController.MoveLinkConnectorTowards(this.motionControllerRight, axis.y * 0.05f);
			}
		}
		if (!this.GUIhitRight && this.highlightedControllersRight.Count > 1)
		{
			if (this.rightCycleX < 0 || this.rightCycleY < 0)
			{
				this.ProcessTargetSelectionCycleBackwardsSelect(this.highlightedControllersRight);
				float num = (float)(Mathf.Abs(this.rightCycleX) + Mathf.Abs(this.rightCycleY)) * 0.1f;
				this.hapticAction.Execute(0f, num, 1f / num, 1f, SteamVR_Input_Sources.RightHand);
			}
			else if (this.rightCycleX > 0 || this.rightCycleY > 0)
			{
				this.ProcessTargetSelectionCycleSelect(this.highlightedControllersRight);
				float num2 = (float)(Mathf.Abs(this.rightCycleX) + Mathf.Abs(this.rightCycleY)) * 0.1f;
				this.hapticAction.Execute(0f, num2, 1f / num2, 1f, SteamVR_Input_Sources.RightHand);
			}
		}
		if (canSelect && !this.didStartLeftNavigate && this.GetLeftSelect())
		{
			this.ProcessTargetSelectionDoSelect(this.highlightedControllersLeft);
		}
		if (this.commonHandModelControl != null && !this._leapHandLeftConnected && this.GetLeftToggleHand())
		{
			this.commonHandModelControl.ToggleLeftHandEnabled();
		}
		else
		{
			if (!this.isLeftOverlap && this.GetLeftRemoteGrab())
			{
				this.ProcessTargetSelectionDoGrabLeft(this.motionControllerLeft, true);
			}
			else if (this.isLeftOverlap && this.GetLeftGrab())
			{
				this.ProcessTargetSelectionDoGrabLeft(this.motionControllerLeft, false);
			}
			bool flag2 = false;
			if (this.GetLeftHoldGrab())
			{
				if (this.leftFullGrabbedController)
				{
					flag2 = true;
					this.leftFullGrabbedController.isGrabbing = false;
					this.leftFullGrabbedController.RestorePreLinkState();
					this.leftFullGrabbedController = null;
					if (this.leftHandControl != null)
					{
						this.leftHandControl.possessed = false;
					}
					this.leftHandControl = null;
				}
				else
				{
					flag2 = this.ProcessTargetSelectionDoFullGrabLeft(this.motionControllerLeft, !this.isLeftOverlap, false);
				}
			}
			if (!flag2 && !this.remoteHoldGrabDisabled && this.GetLeftRemoteHoldGrab())
			{
				if (this.leftFullGrabbedController)
				{
					this.leftFullGrabbedController.isGrabbing = false;
					this.leftFullGrabbedController.RestorePreLinkState();
					this.leftFullGrabbedController = null;
					if (this.leftHandControl != null)
					{
						this.leftHandControl.possessed = false;
					}
					this.leftHandControl = null;
				}
				else
				{
					flag2 = this.ProcessTargetSelectionDoFullGrabLeft(this.motionControllerLeft, !this.isLeftOverlap, true);
				}
			}
		}
		if (this.isOpenVR)
		{
			Vector2 axis2 = this.pushPullAction.GetAxis(SteamVR_Input_Sources.LeftHand);
			if (this.leftGrabbedController != null && this.leftGrabbedControllerIsRemote)
			{
				if (!Mathf.Approximately(axis2.sqrMagnitude, 0f))
				{
					if (float.IsNaN(axis2.y))
					{
						axis2.y = 0f;
					}
					axis2.y = Mathf.Clamp(axis2.y, -100f, 100f);
					this.leftGrabbedController.MoveLinkConnectorTowards(this.motionControllerLeft, axis2.y * 0.05f);
				}
			}
			else if (this.leftFullGrabbedController != null && this.leftFullGrabbedControllerIsRemote && !Mathf.Approximately(axis2.sqrMagnitude, 0f))
			{
				if (float.IsNaN(axis2.y))
				{
					axis2.y = 0f;
				}
				axis2.y = Mathf.Clamp(axis2.y, -100f, 100f);
				this.leftFullGrabbedController.MoveLinkConnectorTowards(this.motionControllerLeft, axis2.y * 0.05f);
			}
		}
		if (!this.GUIhitLeft && this.highlightedControllersLeft.Count > 1)
		{
			if (this.leftCycleX < 0 || this.leftCycleY < 0)
			{
				this.ProcessTargetSelectionCycleBackwardsSelect(this.highlightedControllersLeft);
				float num3 = (float)(Mathf.Abs(this.leftCycleX) + Mathf.Abs(this.leftCycleY)) * 0.1f;
				this.hapticAction.Execute(0f, num3, 1f / num3, 1f, SteamVR_Input_Sources.LeftHand);
			}
			else if (this.leftCycleX > 0 || this.leftCycleY > 0)
			{
				this.ProcessTargetSelectionCycleSelect(this.highlightedControllersLeft);
				float num4 = (float)(Mathf.Abs(this.leftCycleX) + Mathf.Abs(this.leftCycleY)) * 0.1f;
				this.hapticAction.Execute(0f, num4, 1f / num4, 1f, SteamVR_Input_Sources.LeftHand);
			}
		}
		if (((this.rightGrabbedControllerIsRemote && this.GetRightRemoteGrabRelease()) || (!this.rightGrabbedControllerIsRemote && this.GetRightGrabRelease())) && this.rightGrabbedController)
		{
			this.rightGrabbedController.isGrabbing = false;
			this.rightGrabbedController.RestorePreLinkState();
			this.rightGrabbedController = null;
		}
		if (((this.leftGrabbedControllerIsRemote && this.GetLeftRemoteGrabRelease()) || (!this.leftGrabbedControllerIsRemote && this.GetLeftGrabRelease())) && this.leftGrabbedController)
		{
			this.leftGrabbedController.isGrabbing = false;
			this.leftGrabbedController.RestorePreLinkState();
			this.leftGrabbedController = null;
		}
	}

	// Token: 0x06005D74 RID: 23924 RVA: 0x0022E1F4 File Offset: 0x0022C5F4
	private void ProcessCommonTargetSelection()
	{
		if (this.highlightedControllersLook == null)
		{
			this.highlightedControllersLook = new List<FreeControllerV3>();
		}
		if (this.lookCamera != null && this.useLookSelect)
		{
			if (this.GUIhit)
			{
				this.UnhighlightControllers(this.highlightedControllersLook);
				if (this.selectionHUD != null)
				{
					this.selectionHUD.ClearSelections();
					this.selectionHUD.gameObject.SetActive(false);
				}
			}
			else if (this.selectMode != SuperController.SelectMode.Off)
			{
				Transform transform = this.lookCamera.transform;
				this.castRay.origin = transform.position;
				this.castRay.direction = transform.forward;
				this.ProcessTargetSelectionDoRaycast(this.selectionHUD, this.castRay, this.highlightedControllersLook, true, false, true);
			}
		}
	}

	// Token: 0x06005D75 RID: 23925 RVA: 0x0022E2D0 File Offset: 0x0022C6D0
	private void ProcessTargetShow(bool canSelect = true)
	{
		bool flag = this.gameMode == SuperController.GameMode.Edit && this.GetTargetShow();
		if (canSelect)
		{
			if (this.selectMode != SuperController.SelectMode.Targets && flag)
			{
				this.SelectModeTargets();
			}
			if (this.selectMode != SuperController.SelectMode.Off && !flag)
			{
				this.SelectModeOff();
			}
		}
		else if (flag)
		{
			this._pointerModeLeft = true;
			this._pointerModeRight = true;
			foreach (FreeControllerV3 freeControllerV in this.allControllers)
			{
				if (this.onlyShowControllers != null)
				{
					if (this.onlyShowControllers.Contains(freeControllerV))
					{
						freeControllerV.hidden = false;
					}
					else
					{
						freeControllerV.hidden = true;
					}
				}
				else if (this.gameMode == SuperController.GameMode.Edit || freeControllerV.interactableInPlayMode)
				{
					if (UserPreferences.singleton.hideInactiveTargets && freeControllerV.currentPositionState == FreeControllerV3.PositionState.Off && freeControllerV.currentRotationState == FreeControllerV3.RotationState.Off)
					{
						freeControllerV.hidden = true;
					}
					else if (freeControllerV.containingAtom == null)
					{
						freeControllerV.hidden = false;
					}
					else if (freeControllerV.containingAtom.tempHidden)
					{
						freeControllerV.hidden = true;
					}
					else if (this._showHiddenAtoms || !freeControllerV.containingAtom.hidden)
					{
						freeControllerV.hidden = false;
					}
					else
					{
						freeControllerV.hidden = true;
					}
				}
				else
				{
					freeControllerV.hidden = true;
				}
			}
		}
		else
		{
			this._pointerModeLeft = false;
			this._pointerModeRight = false;
			foreach (FreeControllerV3 freeControllerV2 in this.allControllers)
			{
				freeControllerV2.hidden = true;
			}
		}
	}

	// Token: 0x06005D76 RID: 23926 RVA: 0x0022E4F0 File Offset: 0x0022C8F0
	public void ToggleTargetsOnWithButton()
	{
		this.targetsOnWithButton = !this.targetsOnWithButton;
	}

	// Token: 0x06005D77 RID: 23927 RVA: 0x0022E504 File Offset: 0x0022C904
	private void ProcessControllerTargetSelection()
	{
		if (this.useLookSelect)
		{
			if (this.highlightedControllersLook == null)
			{
				this.highlightedControllersLook = new List<FreeControllerV3>();
			}
			if (this.buttonToggleTargets != null && JoystickControl.GetButtonDown(this.buttonToggleTargets))
			{
				this.ToggleTargetsOnWithButton();
			}
			if (!this.GUIhit)
			{
				if (this.buttonSelect != null && this.buttonSelect != string.Empty && JoystickControl.GetButtonDown(this.buttonSelect))
				{
					this.ProcessTargetSelectionDoSelect(this.highlightedControllersLook);
				}
				if (this.buttonCycleSelection != null && this.buttonCycleSelection != string.Empty && JoystickControl.GetButtonDown(this.buttonCycleSelection) && this.highlightedControllersLook.Count > 0)
				{
					this.ProcessTargetSelectionCycleSelect(this.highlightedControllersLook);
				}
			}
			if (this.buttonUnselect != null && this.buttonUnselect != string.Empty && JoystickControl.GetButtonDown(this.buttonUnselect))
			{
				this.ClearSelection(true);
			}
		}
	}

	// Token: 0x06005D78 RID: 23928 RVA: 0x0022E620 File Offset: 0x0022CA20
	private void ProcessMouseChange()
	{
		this.mouseAxisX = Input.GetAxisRaw("Mouse X");
		this.mouseAxisY = Input.GetAxisRaw("Mouse Y");
		this.currentMousePosition = Input.mousePosition;
		if (this.cursorLockedLastFrame)
		{
			this.lastMousePosition = this.currentMousePosition;
		}
		this.mouseChange = this.currentMousePosition - this.lastMousePosition;
		this.mouseChangeScaled = this.mouseChange * 0.1f;
		this.lastMousePosition = Input.mousePosition;
		if (this.selectMode != SuperController.SelectMode.FreeMoveMouse && this.mouseAxisX == 0f && this.mouseAxisY == 0f && (this.mouseChange.x != 0f || this.mouseChange.y != 0f))
		{
			this.useMouseRDPMode = true;
		}
		else
		{
			this.useMouseRDPMode = false;
		}
	}

	// Token: 0x06005D79 RID: 23929 RVA: 0x0022E711 File Offset: 0x0022CB11
	private float GetMouseXChange()
	{
		if (this.useMouseRDPMode)
		{
			return this.mouseChangeScaled.x;
		}
		return this.mouseAxisX;
	}

	// Token: 0x06005D7A RID: 23930 RVA: 0x0022E730 File Offset: 0x0022CB30
	private float GetMouseYChange()
	{
		if (this.useMouseRDPMode)
		{
			return this.mouseChangeScaled.y;
		}
		return this.mouseAxisY;
	}

	// Token: 0x06005D7B RID: 23931 RVA: 0x0022E750 File Offset: 0x0022CB50
	private void ProcessMouseTargetControl(bool canSelect = true)
	{
		if (this.MonitorCenterCamera != null && this.MonitorRigActive)
		{
			if (this.highlightedControllersMouse == null)
			{
				this.highlightedControllersMouse = new List<FreeControllerV3>();
			}
			bool flag = RuntimeTools.ActiveTool != null;
			bool flag2 = false;
			if (this.selectedControllerPositionHandle != null && this.selectedControllerPositionHandle.HasSelectedAxis)
			{
				flag2 = true;
			}
			if (this.selectedControllerRotationHandle != null && this.selectedControllerRotationHandle.HasSelectedAxis)
			{
				flag2 = true;
			}
			Ray ray = this.MonitorCenterCamera.ScreenPointToRay(Input.mousePosition);
			if (!this.GUIhitMouse && !flag2)
			{
				this.ProcessTargetSelectionDoRaycast(this.mouseSelectionHUD, ray, this.highlightedControllersMouse, true, true, false);
			}
			else
			{
				foreach (FreeControllerV3 freeControllerV in this.highlightedControllersMouse)
				{
					freeControllerV.highlighted = false;
				}
				this.highlightedControllersMouse.Clear();
			}
			if (Input.GetMouseButtonDown(0))
			{
				this.eligibleForMouseSelect = false;
				this.potentialGrabbedControllerMouse = null;
				if (this.grabbedControllerMouse != null)
				{
					this.grabbedControllerMouse.isGrabbing = false;
					this.grabbedControllerMouse.RestorePreLinkState();
					this.grabbedControllerMouse = null;
				}
				if (!this.GUIhitMouse && !flag)
				{
					this.eligibleForMouseSelect = true;
					this.dragActivated = false;
					if (this.highlightedControllersMouse.Count > 0)
					{
						this.potentialGrabbedControllerMouse = this.highlightedControllersMouse[0];
					}
					if (this.potentialGrabbedControllerMouse != null)
					{
						this.mouseClickUsed = true;
					}
					if (this.potentialGrabbedControllerMouse != null)
					{
						this.grabbedControllerMouseDistance = (this.potentialGrabbedControllerMouse.transform.position - ray.origin).magnitude;
						this.mouseDownLastWorldPosition = ray.origin + ray.direction * this.grabbedControllerMouseDistance;
					}
				}
			}
			if (!flag && Input.GetMouseButton(0) && this.potentialGrabbedControllerMouse != null)
			{
				float num = this.GetMouseXChange() * 10f;
				float num2 = this.GetMouseYChange() * 10f;
				if (!this.dragActivated && (Mathf.Abs(num) >= 1f || Mathf.Abs(num2) >= 1f))
				{
					this.dragActivated = true;
					if (this.mouseGrab != null)
					{
						Rigidbody component = this.mouseGrab.GetComponent<Rigidbody>();
						if (component != null)
						{
							bool flag3 = true;
							FreeControllerV3.SelectLinkState linkState = FreeControllerV3.SelectLinkState.Position;
							if (this.potentialGrabbedControllerMouse.canGrabPosition)
							{
								if (this.potentialGrabbedControllerMouse.canGrabRotation)
								{
									linkState = FreeControllerV3.SelectLinkState.PositionAndRotation;
								}
							}
							else if (this.potentialGrabbedControllerMouse.canGrabRotation)
							{
								linkState = FreeControllerV3.SelectLinkState.Rotation;
							}
							else
							{
								flag3 = false;
							}
							if (flag3)
							{
								this.mouseGrab.position = this.potentialGrabbedControllerMouse.transform.position;
								this.mouseGrab.rotation = this.potentialGrabbedControllerMouse.transform.rotation;
								this.grabbedControllerMouse = this.potentialGrabbedControllerMouse;
								this.grabbedControllerMouse.isGrabbing = true;
								this.grabbedControllerMouse.SelectLinkToRigidbody(component, linkState, false, true);
							}
						}
					}
				}
				if (this.dragActivated && this.grabbedControllerMouse != null)
				{
					Vector3 a = ray.origin + ray.direction * this.grabbedControllerMouseDistance;
					bool key = Input.GetKey(KeyCode.LeftControl);
					bool key2 = Input.GetKey(KeyCode.LeftShift);
					if (this.grabbedControllerMouse.canGrabRotation && (key || key2))
					{
						if (key2)
						{
							this.mouseGrab.Rotate(this.MonitorCenterCamera.transform.up, -num, Space.World);
							this.mouseGrab.Rotate(this.MonitorCenterCamera.transform.right, num2, Space.World);
						}
						else
						{
							this.mouseGrab.Rotate(this.MonitorCenterCamera.transform.forward, -num, Space.World);
							this.mouseGrab.Rotate(this.MonitorCenterCamera.transform.right, num2, Space.World);
						}
					}
					if (this.grabbedControllerMouse.canGrabPosition && !key && !key2)
					{
						Vector3 b = a - this.mouseDownLastWorldPosition;
						this.mouseGrab.position += b;
					}
					this.mouseDownLastWorldPosition = a;
				}
			}
			if (Input.GetMouseButtonUp(0))
			{
				this.potentialGrabbedControllerMouse = null;
				if (!this.dragActivated && !flag && !this.GUIhitMouse && this.eligibleForMouseSelect)
				{
					this.ProcessTargetSelectionDoRaycast(null, ray, this.highlightedControllersMouse, true, true, true);
					if (canSelect)
					{
						this.ProcessTargetSelectionDoSelect(this.highlightedControllersMouse);
					}
				}
				if (this.grabbedControllerMouse != null)
				{
					this.grabbedControllerMouse.isGrabbing = false;
					this.grabbedControllerMouse.RestorePreLinkState();
					this.grabbedControllerMouse = null;
				}
			}
		}
	}

	// Token: 0x06005D7C RID: 23932 RVA: 0x0022EC9C File Offset: 0x0022D09C
	protected void SyncUISide()
	{
		if (this.MonitorRigActive)
		{
			UISideAlign.useNeutralRotation = true;
			UISideAlign.globalSide = UISideAlign.Side.Left;
		}
		else
		{
			UISideAlign.useNeutralRotation = false;
			UISideAlign.globalSide = this._UISide;
		}
	}

	// Token: 0x06005D7D RID: 23933 RVA: 0x0022ECCC File Offset: 0x0022D0CC
	public void SetUISide(string side)
	{
		try
		{
			this.UISide = (UISideAlign.Side)System.Enum.Parse(typeof(UISideAlign.Side), side);
		}
		catch (FormatException)
		{
			SuperController.LogError("Tried to set UI side to " + side + " which is not a valid side type");
		}
	}

	// Token: 0x17000DBC RID: 3516
	// (get) Token: 0x06005D7E RID: 23934 RVA: 0x0022ED24 File Offset: 0x0022D124
	// (set) Token: 0x06005D7F RID: 23935 RVA: 0x0022ED2C File Offset: 0x0022D12C
	public UISideAlign.Side UISide
	{
		get
		{
			return this._UISide;
		}
		set
		{
			if (this._UISide != value)
			{
				this._UISide = value;
				if (this.UISidePopup != null)
				{
					this.UISidePopup.currentValueNoCallback = this._UISide.ToString();
				}
				this.SyncUISide();
				if (UserPreferences.singleton != null)
				{
					UserPreferences.singleton.SavePreferences();
				}
			}
		}
	}

	// Token: 0x06005D80 RID: 23936 RVA: 0x0022ED99 File Offset: 0x0022D199
	protected void SyncOnStartupSkipStartScreen(bool b)
	{
		this.onStartupSkipStartScreen = b;
	}

	// Token: 0x17000DBD RID: 3517
	// (get) Token: 0x06005D81 RID: 23937 RVA: 0x0022EDA2 File Offset: 0x0022D1A2
	// (set) Token: 0x06005D82 RID: 23938 RVA: 0x0022EDAC File Offset: 0x0022D1AC
	public bool onStartupSkipStartScreen
	{
		get
		{
			return this._onStartupSkipStartScreen;
		}
		set
		{
			if (this._onStartupSkipStartScreen != value)
			{
				this._onStartupSkipStartScreen = value;
				if (this.onStartupSkipStartScreenToggle != null)
				{
					this.onStartupSkipStartScreenToggle.isOn = value;
				}
				if (UserPreferences.singleton != null)
				{
					UserPreferences.singleton.SavePreferences();
				}
			}
		}
	}

	// Token: 0x17000DBE RID: 3518
	// (get) Token: 0x06005D83 RID: 23939 RVA: 0x0022EE03 File Offset: 0x0022D203
	public bool MainHUDVisible
	{
		get
		{
			return this._mainHUDVisible;
		}
	}

	// Token: 0x17000DBF RID: 3519
	// (get) Token: 0x06005D84 RID: 23940 RVA: 0x0022EE0B File Offset: 0x0022D20B
	public bool MainHUDAnchoredOnMonitor
	{
		get
		{
			return this._mainHUDAnchoredOnMonitor;
		}
	}

	// Token: 0x06005D85 RID: 23941 RVA: 0x0022EE14 File Offset: 0x0022D214
	private void SetMonitorRigPositionZero()
	{
		if (this.MonitorRig != null)
		{
			Vector3 localPosition = this.MonitorRig.localPosition;
			localPosition.x = 0f;
			this.MonitorRig.localPosition = localPosition;
		}
	}

	// Token: 0x06005D86 RID: 23942 RVA: 0x0022EE58 File Offset: 0x0022D258
	private void SetMonitorRigPositionOffset()
	{
		if (this.MonitorRig != null)
		{
			Vector3 localPosition = this.MonitorRig.localPosition;
			localPosition.x = this._monitorRigRightOffsetWhenUIOpen * this.focusDistance;
			this.MonitorRig.localPosition = localPosition;
		}
	}

	// Token: 0x06005D87 RID: 23943 RVA: 0x0022EEA4 File Offset: 0x0022D2A4
	public void SyncMonitorRigPosition()
	{
		if (this._mainHUDVisible && this._mainHUDAnchoredOnMonitor && this.headPossessedController == null && (UserPreferences.singleton == null || UserPreferences.singleton.useMonitorViewOffsetWhenUIOpen))
		{
			this.SetMonitorRigPositionOffset();
		}
		else
		{
			this.SetMonitorRigPositionZero();
		}
	}

	// Token: 0x06005D88 RID: 23944 RVA: 0x0022EF08 File Offset: 0x0022D308
	public void SelectModeOffAndShowMainHUDAuto()
	{
		this.SelectModeOff();
		this.ShowMainHUDAuto();
	}

	// Token: 0x06005D89 RID: 23945 RVA: 0x0022EF16 File Offset: 0x0022D316
	public void ShowMainHUDAuto()
	{
		if (this.MonitorRigActive)
		{
			this.ShowMainHUD(true, true);
		}
		else
		{
			this.ShowMainHUD(true, false);
		}
	}

	// Token: 0x06005D8A RID: 23946 RVA: 0x0022EF38 File Offset: 0x0022D338
	public void ShowMainHUD(bool setAnchors = true, bool forceMonitor = false)
	{
		this.SyncUISide();
		this._mainHUDVisible = true;
		if (this.isMonitorOnly || forceMonitor)
		{
			this._helpOverlayOnAux = false;
			if (this.helpToggle != null)
			{
				this.helpToggle.gameObject.SetActive(false);
			}
			if (this.helpToggleAlt != null)
			{
				this.helpToggleAlt.gameObject.SetActive(false);
			}
			this._mainHUDAnchoredOnMonitor = true;
		}
		else
		{
			this._helpOverlayOnAux = true;
			if (this.helpToggle != null)
			{
				this.helpToggle.gameObject.SetActive(true);
			}
			if (this.helpToggleAlt != null)
			{
				this.helpToggleAlt.gameObject.SetActive(true);
			}
			this._mainHUDAnchoredOnMonitor = false;
		}
		this.SyncHelpOverlay();
		this.SyncMonitorRigPosition();
		if (this.mainHUDPivot != null)
		{
			Vector3 localEulerAngles;
			if (this.isMonitorOnly || forceMonitor)
			{
				localEulerAngles.x = this.mainHUDPivotXRotationMonitor;
			}
			else
			{
				localEulerAngles.x = this.mainHUDPivotXRotationVR;
			}
			localEulerAngles.y = 0f;
			localEulerAngles.z = 0f;
			this.mainHUDPivot.localEulerAngles = localEulerAngles;
		}
		if (this.mainHUD != null)
		{
			this.mainHUD.gameObject.SetActive(true);
		}
		this.SyncActiveUI();
		if (setAnchors)
		{
			HUDAnchor.SetAnchorsToReference();
		}
		if (this.selectedControllerPositionHandle != null && this.selectedControllerPositionHandle.controller != null)
		{
			this.selectedControllerPositionHandle.enabled = true;
		}
		if (this.selectedControllerRotationHandle != null && this.selectedControllerRotationHandle.controller != null)
		{
			this.selectedControllerRotationHandle.enabled = true;
		}
		this.SyncVisibility();
	}

	// Token: 0x06005D8B RID: 23947 RVA: 0x0022F11C File Offset: 0x0022D51C
	public void HideMainHUD()
	{
		this._mainHUDVisible = false;
		this.SyncMonitorRigPosition();
		if (this.mainHUD != null)
		{
			this.mainHUD.gameObject.SetActive(false);
		}
		if (this.selectedController != null)
		{
			this.selectedController.guihidden = true;
		}
		if (this.selectedControllerPositionHandle != null)
		{
			this.selectedControllerPositionHandle.enabled = false;
		}
		if (this.selectedControllerRotationHandle != null)
		{
			this.selectedControllerRotationHandle.enabled = false;
		}
		if (this.customUI != null)
		{
			this.customUI.gameObject.SetActive(false);
		}
		this.HideTempHelp();
		this.SyncVisibility();
	}

	// Token: 0x06005D8C RID: 23948 RVA: 0x0022F1DD File Offset: 0x0022D5DD
	public void MoveMainHUD(Vector3 v)
	{
		if (this.mainHUDAttachPoint != null)
		{
			this.mainHUDAttachPoint.position = v;
		}
	}

	// Token: 0x06005D8D RID: 23949 RVA: 0x0022F1FC File Offset: 0x0022D5FC
	public void MoveMainHUD(Transform t)
	{
		if (this.mainHUDAttachPoint != null && t != null)
		{
			this.mainHUDAttachPoint.position = t.position;
			this.mainHUDAttachPoint.rotation = t.rotation;
		}
	}

	// Token: 0x06005D8E RID: 23950 RVA: 0x0022F248 File Offset: 0x0022D648
	public void SetHelpHUDText(string txt)
	{
		this.helpText = txt;
	}

	// Token: 0x06005D8F RID: 23951 RVA: 0x0022F251 File Offset: 0x0022D651
	public void ToggleMainHUDAuto()
	{
		if (this.mainHUD != null)
		{
			if (this._mainHUDVisible)
			{
				this.HideMainHUD();
			}
			else
			{
				this.ShowMainHUDAuto();
			}
		}
	}

	// Token: 0x06005D90 RID: 23952 RVA: 0x0022F280 File Offset: 0x0022D680
	public void ToggleMainHUD()
	{
		if (this.mainHUD != null)
		{
			if (this._mainHUDVisible)
			{
				this.HideMainHUD();
			}
			else
			{
				this.ShowMainHUD(true, false);
			}
		}
	}

	// Token: 0x06005D91 RID: 23953 RVA: 0x0022F2B1 File Offset: 0x0022D6B1
	public void ToggleMainHUDMonitor()
	{
		if (this.mainHUD != null)
		{
			if (this._mainHUDVisible)
			{
				this.HideMainHUD();
			}
			else
			{
				this.ShowMainHUDMonitor();
			}
		}
	}

	// Token: 0x06005D92 RID: 23954 RVA: 0x0022F2E0 File Offset: 0x0022D6E0
	public void ShowMainHUDMonitor()
	{
		this.ShowMainHUD(true, true);
	}

	// Token: 0x06005D93 RID: 23955 RVA: 0x0022F2EC File Offset: 0x0022D6EC
	private void AssignUICamera(Camera c)
	{
		if (c != null)
		{
			LookInputModule.singleton.referenceCamera = c;
			foreach (Canvas canvas in this.allCanvases)
			{
				if (canvas != null && canvas.renderMode == RenderMode.WorldSpace)
				{
					canvas.worldCamera = c;
				}
			}
		}
		else
		{
			this.Error("Tried to call AssignUICamera with a null camera", true, true);
		}
	}

	// Token: 0x06005D94 RID: 23956 RVA: 0x0022F38C File Offset: 0x0022D78C
	private void ProcessUI()
	{
		if (!this.UIDisabled)
		{
			if (this.GetMenuShow())
			{
				if (this._mainHUDVisible)
				{
					this.HideMainHUD();
				}
				else
				{
					this.ShowMainHUD(true, false);
				}
			}
			if (this._mainHUDVisible)
			{
				if (this.GetMenuMoveLeft())
				{
					this.MoveMainHUD(this.motionControllerLeft);
				}
				if (this.GetMenuMoveRight())
				{
					this.MoveMainHUD(this.motionControllerRight);
				}
			}
		}
		if (LookInputModule.singleton != null)
		{
			if (this.useLookSelect)
			{
				this.AssignUICamera(this.lookCamera);
				LookInputModule.singleton.ProcessMain();
				this.GUIhit = LookInputModule.singleton.guiRaycastHit;
			}
			else if (this.leftControllerCamera != null)
			{
				if (this.leftControllerCamera.gameObject.activeInHierarchy)
				{
					this.AssignUICamera(this.leftControllerCamera);
					LookInputModule.singleton.ProcessMain();
					this.GUIhitLeft = LookInputModule.singleton.guiRaycastHit;
				}
				else
				{
					this.GUIhitLeft = false;
				}
				if (this.GUIhitLeft)
				{
					this.drawRayLineLeft = !this.MonitorRigActive;
				}
				if (this.rightControllerCamera != null)
				{
					if (this.rightControllerCamera.gameObject.activeInHierarchy)
					{
						this.AssignUICamera(this.rightControllerCamera);
						LookInputModule.singleton.ProcessRight();
						this.GUIhitRight = LookInputModule.singleton.guiRaycastHit;
					}
					else
					{
						this.GUIhitRight = false;
					}
					if (this.GUIhitRight)
					{
						this.drawRayLineRight = !this.MonitorRigActive;
					}
				}
				else
				{
					this.Error("Right controller camera is null while processing UI", true, true);
				}
			}
			else if (this.rightControllerCamera != null)
			{
				if (this.rightControllerCamera.gameObject.activeInHierarchy)
				{
					this.AssignUICamera(this.rightControllerCamera);
					LookInputModule.singleton.ProcessRight();
					this.GUIhitRight = LookInputModule.singleton.guiRaycastHit;
				}
				else
				{
					this.GUIhitRight = false;
				}
				if (this.GUIhitRight)
				{
					this.drawRayLineRight = !this.MonitorRigActive;
				}
			}
			this.AssignUICamera(this.MonitorCenterCamera);
			LookInputModule.singleton.ProcessMouseAlt(this.selectMode == SuperController.SelectMode.FreeMoveMouse);
			this.GUIhitMouse = LookInputModule.singleton.mouseRaycastHit;
		}
	}

	// Token: 0x06005D95 RID: 23957 RVA: 0x0022F5EC File Offset: 0x0022D9EC
	private void ProcessUIMove()
	{
		if (!this.UIDisabled && this._mainHUDAnchoredOnMonitor && this.MonitorCenterCamera != null && this.MonitorUIAnchor != null && this.MonitorUIAttachPoint != null)
		{
			Vector3 position;
			position.x = 5f;
			position.y = this._monitorUIYOffset + 60f;
			position.z = (float)this.MonitorCenterCamera.pixelHeight * 0.08f / this._monitorUIScale / this.fixedMonitorUIScale / this.monitorCameraFOV * this.worldScale;
			Vector3 position2 = this.MonitorCenterCamera.ScreenToWorldPoint(position);
			this.MonitorUIAnchor.position = position2;
			this.MoveMainHUD(this.MonitorUIAttachPoint);
			HUDAnchor.SetAnchorsToReference();
		}
	}

	// Token: 0x06005D96 RID: 23958 RVA: 0x0022F6C0 File Offset: 0x0022DAC0
	public void RemoveCanvas(Canvas c)
	{
		this.allCanvases.Remove(c);
	}

	// Token: 0x06005D97 RID: 23959 RVA: 0x0022F6D0 File Offset: 0x0022DAD0
	public void AddCanvas(Canvas c)
	{
		if (this.overrideCanvasSortingLayer)
		{
			IgnoreCanvas component = c.GetComponent<IgnoreCanvas>();
			if (component == null)
			{
				c.sortingLayerName = this.overrideCanvasSortingLayerName;
			}
		}
		this.allCanvases.Add(c);
	}

	// Token: 0x06005D98 RID: 23960 RVA: 0x0022F713 File Offset: 0x0022DB13
	protected void AllocateRaycastHits()
	{
		if (this.raycastHits == null)
		{
			this.raycastHits = new RaycastHit[256];
		}
	}

	// Token: 0x06005D99 RID: 23961 RVA: 0x0022F730 File Offset: 0x0022DB30
	private void ProcessSelectDoRaycast(SelectionHUD sh, Ray ray, List<SelectTarget> hitsList, bool doHighlight = true, bool setSelectionHUDTransform = true)
	{
		this.AllocateRaycastHits();
		int num = Physics.RaycastNonAlloc(ray, this.raycastHits, 50f, this.selectColliderMask);
		if (num > 0)
		{
			if (this.wasHitST == null)
			{
				this.wasHitST = new Dictionary<SelectTarget, bool>();
			}
			else
			{
				this.wasHitST.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				RaycastHit raycastHit = this.raycastHits[i];
				SelectTarget component = raycastHit.transform.GetComponent<SelectTarget>();
				if (component != null && !this.wasHitST.ContainsKey(component))
				{
					this.wasHitST.Add(component, true);
					if (!hitsList.Contains(component))
					{
						hitsList.Add(component);
					}
				}
			}
			SelectTarget[] array = hitsList.ToArray();
			foreach (SelectTarget selectTarget in array)
			{
				if (!this.wasHitST.ContainsKey(selectTarget))
				{
					selectTarget.highlighted = false;
					hitsList.Remove(selectTarget);
				}
			}
			if (doHighlight)
			{
				for (int k = 0; k < hitsList.Count; k++)
				{
					SelectTarget selectTarget2 = hitsList[k];
					if (k == 0)
					{
						selectTarget2.highlighted = true;
					}
					else
					{
						selectTarget2.highlighted = false;
					}
				}
			}
			if (sh != null)
			{
				sh.ClearSelections();
				if (hitsList.Count > 0)
				{
					int num2 = 0;
					foreach (SelectTarget selectTarget3 in hitsList)
					{
						sh.SetSelection(num2, selectTarget3.transform, selectTarget3.selectionName);
						num2++;
					}
				}
			}
		}
		else
		{
			if (doHighlight)
			{
				foreach (SelectTarget selectTarget4 in hitsList)
				{
					selectTarget4.highlighted = false;
				}
			}
			hitsList.Clear();
		}
		if (sh != null)
		{
			if (hitsList.Count > 0)
			{
				sh.gameObject.SetActive(true);
				if (setSelectionHUDTransform)
				{
					sh.transform.position = hitsList[0].transform.position;
					float magnitude = (sh.transform.position - this.lookCamera.transform.position).magnitude;
					Vector3 localScale;
					localScale.x = magnitude;
					localScale.y = magnitude;
					localScale.z = magnitude;
					sh.transform.localScale = localScale;
					sh.transform.LookAt(this.lookCamera.transform.position);
				}
			}
			else
			{
				sh.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06005D9A RID: 23962 RVA: 0x0022FA38 File Offset: 0x0022DE38
	private void ProcessSelectDoSelect(List<SelectTarget> highlightedSelectTargets)
	{
		SelectTarget selectTarget = highlightedSelectTargets[0];
		switch (this.selectMode)
		{
		case SuperController.SelectMode.Controller:
		{
			FreeControllerV3 freeControllerV;
			if (this.fcMap.TryGetValue(selectTarget.selectionName, out freeControllerV))
			{
				this.selectControllerCallback(freeControllerV);
				this.SelectModeOff();
			}
			break;
		}
		case SuperController.SelectMode.ForceReceiver:
		{
			ForceReceiver fr;
			if (this.frMap.TryGetValue(selectTarget.selectionName, out fr))
			{
				this.selectForceReceiverCallback(fr);
				this.SelectModeOff();
			}
			break;
		}
		case SuperController.SelectMode.ForceProducer:
		{
			ForceProducerV2 fp;
			if (this.fpMap.TryGetValue(selectTarget.selectionName, out fp))
			{
				this.selectForceProducerCallback(fp);
				this.SelectModeOff();
			}
			break;
		}
		case SuperController.SelectMode.Rigidbody:
		{
			Rigidbody rb;
			if (this.rbMap.TryGetValue(selectTarget.selectionName, out rb))
			{
				this.selectRigidbodyCallback(rb);
				this.SelectModeOff();
			}
			break;
		}
		case SuperController.SelectMode.Atom:
		{
			Atom a;
			if (this.atoms.TryGetValue(selectTarget.selectionName, out a))
			{
				this.selectAtomCallback(a);
				this.SelectModeOff();
			}
			break;
		}
		case SuperController.SelectMode.PossessAndAlign:
		{
			FreeControllerV3 freeControllerV;
			if (this.fcMap.TryGetValue(selectTarget.selectionName, out freeControllerV))
			{
				this.HeadPossess(freeControllerV, true, true, true);
				if (this.isMonitorOnly)
				{
					this.SelectModeOff();
				}
				else
				{
					this.SelectModePossess(true);
				}
			}
			break;
		}
		case SuperController.SelectMode.Unpossess:
		{
			FreeControllerV3 freeControllerV;
			if (this.fcMap.TryGetValue(selectTarget.selectionName, out freeControllerV))
			{
				this.ClearPossess(false, freeControllerV);
				if (this.GetCancel())
				{
					this.SelectModeOff();
				}
			}
			break;
		}
		case SuperController.SelectMode.ArmedForRecord:
		{
			MotionAnimationControl motionAnimationControl;
			if (this.macMap.TryGetValue(selectTarget.selectionName, out motionAnimationControl))
			{
				motionAnimationControl.armedForRecord = !motionAnimationControl.armedForRecord;
				if (motionAnimationControl.armedForRecord)
				{
					selectTarget.SetColor(Color.green);
				}
				else
				{
					selectTarget.SetColor(Color.red);
				}
			}
			break;
		}
		}
	}

	// Token: 0x06005D9B RID: 23963 RVA: 0x0022FC4C File Offset: 0x0022E04C
	private void ProcessSelectCycleSelect(List<SelectTarget> highlightedSelectTargets)
	{
		if (highlightedSelectTargets != null && highlightedSelectTargets.Count > 1)
		{
			SelectTarget item = highlightedSelectTargets[0];
			highlightedSelectTargets.RemoveAt(0);
			highlightedSelectTargets.Add(item);
		}
	}

	// Token: 0x06005D9C RID: 23964 RVA: 0x0022FC84 File Offset: 0x0022E084
	private void ProcessSelectCycleBackwardsSelect(List<SelectTarget> highlightedSelectTargets)
	{
		if (highlightedSelectTargets != null && highlightedSelectTargets.Count > 1)
		{
			int index = highlightedSelectTargets.Count - 1;
			SelectTarget item = highlightedSelectTargets[index];
			highlightedSelectTargets.RemoveAt(index);
			highlightedSelectTargets.Insert(0, item);
		}
	}

	// Token: 0x06005D9D RID: 23965 RVA: 0x0022FCC4 File Offset: 0x0022E0C4
	private void ProcessSelectTargetHighlight(SelectionHUD sh, Transform processFrom, bool isLeft)
	{
		this.castRay.origin = processFrom.position;
		this.castRay.direction = processFrom.forward;
		if (isLeft)
		{
			this.drawRayLineLeft = !this.MonitorRigActive;
			this.ProcessSelectDoRaycast(sh, this.castRay, this.highlightedSelectTargetsLeft, false, true);
			sh.useDrawFromPosition = true;
			sh.drawFrom = processFrom.position;
		}
		else
		{
			this.drawRayLineRight = !this.MonitorRigActive;
			this.ProcessSelectDoRaycast(sh, this.castRay, this.highlightedSelectTargetsRight, false, true);
			sh.useDrawFromPosition = true;
			sh.drawFrom = processFrom.position;
		}
	}

	// Token: 0x06005D9E RID: 23966 RVA: 0x0022FD6C File Offset: 0x0022E16C
	private void ProcessMotionControllerSelect()
	{
		if (this.isMonitorOnly)
		{
			return;
		}
		if (this.highlightedSelectTargetsLeft == null)
		{
			this.highlightedSelectTargetsLeft = new List<SelectTarget>();
		}
		if (this.highlightedSelectTargetsRight == null)
		{
			this.highlightedSelectTargetsRight = new List<SelectTarget>();
		}
		if (this.motionControllerLeft && !this.GUIhitLeft)
		{
			this.ProcessSelectTargetHighlight(this.leftSelectionHUD, this.motionControllerLeft, true);
		}
		if (this.motionControllerRight && !this.GUIhitRight)
		{
			this.ProcessSelectTargetHighlight(this.rightSelectionHUD, this.motionControllerRight, false);
		}
		if (this.GetLeftSelect() && this.highlightedSelectTargetsLeft.Count > 0)
		{
			this.ProcessSelectDoSelect(this.highlightedSelectTargetsLeft);
		}
		if (this.GetRightSelect() && this.highlightedSelectTargetsRight.Count > 0)
		{
			this.ProcessSelectDoSelect(this.highlightedSelectTargetsRight);
		}
		if (this.isOpenVR)
		{
			if (this.highlightedSelectTargetsLeft != null && this.highlightedSelectTargetsLeft.Count > 1)
			{
				if (this.leftCycleX < 0 || this.leftCycleY < 0)
				{
					this.ProcessSelectCycleBackwardsSelect(this.highlightedSelectTargetsLeft);
					float num = (float)(Mathf.Abs(this.leftCycleX) + Mathf.Abs(this.leftCycleY)) * 0.1f;
					this.hapticAction.Execute(0f, num, 1f / num, 1f, SteamVR_Input_Sources.LeftHand);
				}
				else if (this.leftCycleX > 0 || this.leftCycleY > 0)
				{
					this.ProcessSelectCycleSelect(this.highlightedSelectTargetsLeft);
					float num2 = (float)(Mathf.Abs(this.leftCycleX) + Mathf.Abs(this.leftCycleY)) * 0.1f;
					this.hapticAction.Execute(0f, num2, 1f / num2, 1f, SteamVR_Input_Sources.LeftHand);
				}
			}
			if (this.highlightedSelectTargetsRight != null && this.highlightedSelectTargetsRight.Count > 0)
			{
				if (this.rightCycleX < 0 || this.rightCycleY < 0)
				{
					this.ProcessSelectCycleBackwardsSelect(this.highlightedSelectTargetsRight);
					float num3 = (float)(Mathf.Abs(this.rightCycleX) + Mathf.Abs(this.rightCycleY)) * 0.1f;
					this.hapticAction.Execute(0f, num3, 1f / num3, 1f, SteamVR_Input_Sources.RightHand);
				}
				else if (this.rightCycleX > 0 || this.rightCycleY > 0)
				{
					this.ProcessSelectCycleSelect(this.highlightedSelectTargetsRight);
					float num4 = (float)(Mathf.Abs(this.rightCycleX) + Mathf.Abs(this.rightCycleY)) * 0.1f;
					this.hapticAction.Execute(0f, num4, 1f / num4, 1f, SteamVR_Input_Sources.RightHand);
				}
			}
		}
		if (this.GetCancel())
		{
			this.SelectModeOff();
		}
	}

	// Token: 0x06005D9F RID: 23967 RVA: 0x00230040 File Offset: 0x0022E440
	private void ProcessSelect()
	{
		if (this.highlightedSelectTargetsLook == null)
		{
			this.highlightedSelectTargetsLook = new List<SelectTarget>();
		}
		if (this.useLookSelect && this.lookCamera != null && !this.GUIhit)
		{
			Transform transform = this.lookCamera.transform;
			this.castRay.origin = transform.position;
			this.castRay.direction = transform.forward;
			this.ProcessSelectDoRaycast(this.selectionHUD, this.castRay, this.highlightedSelectTargetsLook, true, true);
			if (this.buttonSelect != null && this.buttonSelect != string.Empty && JoystickControl.GetButtonDown(this.buttonSelect) && this.highlightedSelectTargetsLook.Count > 0)
			{
				this.ProcessSelectDoSelect(this.highlightedSelectTargetsLook);
			}
			if (this.buttonCycleSelection != null && this.buttonCycleSelection != string.Empty && JoystickControl.GetButtonDown(this.buttonCycleSelection) && this.highlightedSelectTargetsLook.Count > 0)
			{
				this.ProcessSelectCycleSelect(this.highlightedSelectTargetsLook);
			}
			if (this.buttonUnselect != null && this.buttonUnselect != string.Empty && JoystickControl.GetButtonDown(this.buttonUnselect))
			{
				this.SelectModeOff();
			}
		}
	}

	// Token: 0x06005DA0 RID: 23968 RVA: 0x002301A4 File Offset: 0x0022E5A4
	private void ProcessMouseSelect()
	{
		if (this.highlightedSelectTargetsMouse == null)
		{
			this.highlightedSelectTargetsMouse = new List<SelectTarget>();
		}
		if (this.MonitorRigActive && !this.GUIhitMouse)
		{
			Ray ray = this.MonitorCenterCamera.ScreenPointToRay(Input.mousePosition);
			this.ProcessSelectDoRaycast(this.mouseSelectionHUD, ray, this.highlightedSelectTargetsMouse, true, false);
			if (Input.GetMouseButtonDown(0) && this.highlightedSelectTargetsMouse.Count > 0)
			{
				this.ProcessSelectDoSelect(this.highlightedSelectTargetsMouse);
			}
			if (Input.GetKeyDown(KeyCode.C))
			{
				this.ProcessSelectCycleSelect(this.highlightedSelectTargetsMouse);
			}
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				this.SelectModeOff();
			}
		}
	}

	// Token: 0x17000DC0 RID: 3520
	// (get) Token: 0x06005DA1 RID: 23969 RVA: 0x00230255 File Offset: 0x0022E655
	// (set) Token: 0x06005DA2 RID: 23970 RVA: 0x0023025D File Offset: 0x0022E65D
	public bool allowHeadPossessMousePanAndZoom
	{
		get
		{
			return this._allowHeadPossessMousePanAndZoom;
		}
		set
		{
			if (this._allowHeadPossessMousePanAndZoom != value)
			{
				this._allowHeadPossessMousePanAndZoom = value;
				if (this.allowHeadPossessMousePanAndZoomToggle != null)
				{
					this.allowHeadPossessMousePanAndZoomToggle.isOn = this._allowHeadPossessMousePanAndZoom;
				}
			}
		}
	}

	// Token: 0x17000DC1 RID: 3521
	// (get) Token: 0x06005DA3 RID: 23971 RVA: 0x00230294 File Offset: 0x0022E694
	// (set) Token: 0x06005DA4 RID: 23972 RVA: 0x0023029C File Offset: 0x0022E69C
	public bool allowPossessSpringAdjustment
	{
		get
		{
			return this._allowPossessSpringAdjustment;
		}
		set
		{
			if (this._allowPossessSpringAdjustment != value)
			{
				this._allowPossessSpringAdjustment = value;
				if (this.allowPossessSpringAdjustmentToggle != null)
				{
					this.allowPossessSpringAdjustmentToggle.isOn = value;
				}
				if (UserPreferences.singleton != null)
				{
					UserPreferences.singleton.SavePreferences();
				}
			}
		}
	}

	// Token: 0x17000DC2 RID: 3522
	// (get) Token: 0x06005DA5 RID: 23973 RVA: 0x002302F3 File Offset: 0x0022E6F3
	// (set) Token: 0x06005DA6 RID: 23974 RVA: 0x002302FC File Offset: 0x0022E6FC
	public float possessPositionSpring
	{
		get
		{
			return this._possessPositionSpring;
		}
		set
		{
			if (this._possessPositionSpring != value)
			{
				this._possessPositionSpring = value;
				if (this.possessPositionSpringSlider != null)
				{
					this.possessPositionSpringSlider.value = value;
				}
				if (UserPreferences.singleton != null)
				{
					UserPreferences.singleton.SavePreferences();
				}
			}
		}
	}

	// Token: 0x17000DC3 RID: 3523
	// (get) Token: 0x06005DA7 RID: 23975 RVA: 0x00230353 File Offset: 0x0022E753
	// (set) Token: 0x06005DA8 RID: 23976 RVA: 0x0023035C File Offset: 0x0022E75C
	public float possessRotationSpring
	{
		get
		{
			return this._possessRotationSpring;
		}
		set
		{
			if (this._possessRotationSpring != value)
			{
				this._possessRotationSpring = value;
				if (this.possessRotationSpringSlider != null)
				{
					this.possessRotationSpringSlider.value = value;
				}
				if (UserPreferences.singleton != null)
				{
					UserPreferences.singleton.SavePreferences();
				}
			}
		}
	}

	// Token: 0x06005DA9 RID: 23977 RVA: 0x002303B4 File Offset: 0x0022E7B4
	private FreeControllerV3 ProcessControllerPossess(Transform processFrom)
	{
		if (processFrom != null)
		{
			this.AllocateOverlappingControls();
			int num = Physics.OverlapSphereNonAlloc(processFrom.position, 0.01f * this._worldScale, this.overlappingControls, this.targetColliderMask);
			if (num > 0)
			{
				if (this.overlappingFcs == null)
				{
					this.overlappingFcs = new List<FreeControllerV3>();
				}
				else
				{
					this.overlappingFcs.Clear();
				}
				for (int i = 0; i < num; i++)
				{
					Collider collider = this.overlappingControls[i];
					FreeControllerV3 component = collider.GetComponent<FreeControllerV3>();
					if (component != null && component.possessable && (component.canGrabPosition || component.canGrabRotation) && !component.possessed && !component.startedPossess)
					{
						if (!this.overlappingFcs.Contains(component))
						{
							this.overlappingFcs.Add(component);
						}
						float distanceHolder = Vector3.SqrMagnitude(processFrom.position - collider.transform.position);
						component.distanceHolder = distanceHolder;
					}
				}
				if (this.overlappingFcs.Count > 0)
				{
					List<FreeControllerV3> list = this.overlappingFcs;
					if (SuperController.<>f__am$cache1 == null)
					{
						SuperController.<>f__am$cache1 = new Comparison<FreeControllerV3>(SuperController.<ProcessControllerPossess>m__31);
					}
					list.Sort(SuperController.<>f__am$cache1);
					return this.overlappingFcs[0];
				}
			}
		}
		return null;
	}

	// Token: 0x06005DAA RID: 23978 RVA: 0x0023051C File Offset: 0x0022E91C
	private void AlignRigAndController(FreeControllerV3 controller)
	{
		Possessor component = this.motionControllerHead.GetComponent<Possessor>();
		Vector3 forwardPossessAxis = controller.GetForwardPossessAxis();
		Vector3 upPossessAxis = controller.GetUpPossessAxis();
		Vector3 up = this.navigationRig.up;
		Vector3 vector = Vector3.ProjectOnPlane(this.motionControllerHead.forward, up);
		Vector3 vector2 = Vector3.ProjectOnPlane(forwardPossessAxis, up);
		float num = Vector3.Dot(vector, vector2);
		if (num < -0.98f)
		{
			this.navigationRig.Rotate(0f, 180f, 0f);
			vector2 = -vector2;
		}
		if (Vector3.Dot(upPossessAxis, up) < 0f && Vector3.Dot(this.motionControllerHead.up, up) > 0f)
		{
			vector2 = -vector2;
		}
		Quaternion lhs = Quaternion.FromToRotation(vector, vector2);
		this.navigationRig.rotation = lhs * this.navigationRig.rotation;
		if (this.MonitorCenterCamera != null)
		{
			this.MonitorCenterCamera.transform.LookAt(this.MonitorCenterCamera.transform.position + forwardPossessAxis);
			Vector3 localEulerAngles = this.MonitorCenterCamera.transform.localEulerAngles;
			localEulerAngles.y = 0f;
			localEulerAngles.z = 0f;
			this.MonitorCenterCamera.transform.localEulerAngles = localEulerAngles;
		}
		Vector3 position = Vector3.zero;
		Quaternion rotation = Quaternion.identity;
		Transform followWhenOff = controller.followWhenOff;
		if (controller.possessPoint != null && followWhenOff != null)
		{
			position = followWhenOff.position;
			rotation = followWhenOff.rotation;
			followWhenOff.position = controller.control.position;
			followWhenOff.rotation = controller.control.rotation;
		}
		if (controller.canGrabRotation)
		{
			controller.AlignTo(component.autoSnapPoint, true);
		}
		Vector3 position2;
		if (controller.possessPoint != null)
		{
			position2 = controller.possessPoint.position;
		}
		else
		{
			position2 = controller.control.position;
		}
		Vector3 b = position2 - component.autoSnapPoint.position;
		Vector3 vector3 = this.navigationRig.position + b;
		float num2 = Vector3.Dot(vector3 - this.navigationRig.position, up);
		vector3 += up * -num2;
		this.navigationRig.position = vector3;
		this.playerHeightAdjust += num2;
		this.headPossessedController.PossessMoveAndAlignTo(component.autoSnapPoint);
		if (controller.possessPoint != null && followWhenOff != null)
		{
			followWhenOff.position = position;
			followWhenOff.rotation = rotation;
		}
	}

	// Token: 0x06005DAB RID: 23979 RVA: 0x002307D8 File Offset: 0x0022EBD8
	private void HeadPossess(FreeControllerV3 headPossess, bool alignRig = false, bool usePossessorSnapPoint = true, bool adjustSpring = true)
	{
		bool flag = headPossess.canGrabPosition || headPossess.canGrabRotation;
		if (flag)
		{
			Possessor component = this.motionControllerHead.GetComponent<Possessor>();
			Rigidbody component2 = this.motionControllerHead.GetComponent<Rigidbody>();
			this.headPossessedController = headPossess;
			if (this.headPossessedActivateTransform != null)
			{
				this.headPossessedActivateTransform.gameObject.SetActive(true);
			}
			if (this.headPossessedText != null)
			{
				if (this.headPossessedController.containingAtom != null)
				{
					this.headPossessedText.text = this.headPossessedController.containingAtom.uid + ":" + this.headPossessedController.name;
				}
				else
				{
					this.headPossessedText.text = this.headPossessedController.name;
				}
			}
			this.headPossessedController.possessed = true;
			if (this.rightFullGrabbedController == this.headPossessedController)
			{
				this.rightFullGrabbedController = null;
				if (this.rightHandControl != null)
				{
					this.rightHandControl.possessed = false;
				}
				this.rightHandControl = null;
			}
			if (this.leftFullGrabbedController == this.headPossessedController)
			{
				this.leftFullGrabbedController = null;
				if (this.leftHandControl != null)
				{
					this.leftHandControl.possessed = false;
				}
				this.leftHandControl = null;
			}
			if (this.leftGrabbedController == this.headPossessedController)
			{
				this.leftGrabbedController = null;
			}
			if (this.rightGrabbedController == this.headPossessedController)
			{
				this.rightGrabbedController = null;
			}
			if (this.headPossessedController.canGrabPosition)
			{
				MotionAnimationControl component3 = this.headPossessedController.GetComponent<MotionAnimationControl>();
				if (component3 != null)
				{
					component3.suspendPositionPlayback = true;
				}
				if (this._allowPossessSpringAdjustment && adjustSpring)
				{
					this.headPossessedController.RBHoldPositionSpring = this._possessPositionSpring;
				}
			}
			if (this.headPossessedController.canGrabRotation)
			{
				MotionAnimationControl component4 = this.headPossessedController.GetComponent<MotionAnimationControl>();
				if (component4 != null)
				{
					component4.suspendRotationPlayback = true;
				}
				if (this._allowPossessSpringAdjustment && adjustSpring)
				{
					this.headPossessedController.RBHoldRotationSpring = this._possessRotationSpring;
				}
			}
			this.SyncMonitorRigPosition();
			if (alignRig)
			{
				this.AlignRigAndController(this.headPossessedController);
			}
			else if (component != null && component.autoSnapPoint != null && usePossessorSnapPoint)
			{
				this.headPossessedController.PossessMoveAndAlignTo(component.autoSnapPoint);
			}
			if (component2 != null)
			{
				FreeControllerV3.SelectLinkState linkState = FreeControllerV3.SelectLinkState.Position;
				if (this.headPossessedController.canGrabPosition)
				{
					if (this.headPossessedController.canGrabRotation)
					{
						linkState = FreeControllerV3.SelectLinkState.PositionAndRotation;
					}
				}
				else if (this.headPossessedController.canGrabRotation)
				{
					linkState = FreeControllerV3.SelectLinkState.Rotation;
				}
				this.headPossessedController.SelectLinkToRigidbody(component2, linkState, false, true);
			}
		}
	}

	// Token: 0x06005DAC RID: 23980 RVA: 0x00230AD0 File Offset: 0x0022EED0
	protected bool MotionControlPossess(Transform motionController, FreeControllerV3 controllerToPossess, bool usePossessorSnapPoint = true, bool adjustSpring = true)
	{
		bool flag = controllerToPossess.canGrabPosition || controllerToPossess.canGrabRotation;
		if (flag)
		{
			Possessor component = motionController.GetComponent<Possessor>();
			Rigidbody component2 = motionController.GetComponent<Rigidbody>();
			if (this.playerNavCollider != null && this.playerNavCollider.underlyingControl == controllerToPossess)
			{
				this.DisconnectNavRigFromPlayerNavCollider();
			}
			controllerToPossess.possessed = true;
			if (this.rightFullGrabbedController == controllerToPossess)
			{
				this.rightFullGrabbedController = null;
				if (this.rightHandControl != null)
				{
					this.rightHandControl.possessed = false;
				}
				this.rightHandControl = null;
			}
			if (this.leftFullGrabbedController == controllerToPossess)
			{
				this.leftFullGrabbedController = null;
				if (this.leftHandControl != null)
				{
					this.leftHandControl.possessed = false;
				}
				this.leftHandControl = null;
			}
			if (this.leftGrabbedController == controllerToPossess)
			{
				this.leftGrabbedController = null;
			}
			if (this.rightGrabbedController == controllerToPossess)
			{
				this.rightGrabbedController = null;
			}
			if (controllerToPossess.canGrabPosition)
			{
				MotionAnimationControl component3 = controllerToPossess.GetComponent<MotionAnimationControl>();
				if (component3 != null)
				{
					component3.suspendPositionPlayback = true;
				}
				if (this._allowPossessSpringAdjustment && adjustSpring)
				{
					controllerToPossess.RBHoldPositionSpring = this._possessPositionSpring;
				}
			}
			if (controllerToPossess.canGrabRotation)
			{
				MotionAnimationControl component4 = controllerToPossess.GetComponent<MotionAnimationControl>();
				if (component4 != null)
				{
					component4.suspendRotationPlayback = true;
				}
				if (this._allowPossessSpringAdjustment && adjustSpring)
				{
					controllerToPossess.RBHoldRotationSpring = this._possessRotationSpring;
				}
			}
			if (component != null && component.autoSnapPoint != null && usePossessorSnapPoint)
			{
				controllerToPossess.PossessMoveAndAlignTo(component.autoSnapPoint);
			}
			if (component2 != null)
			{
				FreeControllerV3.SelectLinkState linkState = FreeControllerV3.SelectLinkState.Position;
				if (controllerToPossess.canGrabPosition)
				{
					if (controllerToPossess.canGrabRotation)
					{
						linkState = FreeControllerV3.SelectLinkState.PositionAndRotation;
					}
				}
				else if (controllerToPossess.canGrabRotation)
				{
					linkState = FreeControllerV3.SelectLinkState.Rotation;
				}
				controllerToPossess.SelectLinkToRigidbody(component2, linkState, false, true);
			}
			return true;
		}
		return false;
	}

	// Token: 0x06005DAD RID: 23981 RVA: 0x00230CE4 File Offset: 0x0022F0E4
	private void ProcessPossess()
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		if (this.rightPossessedController == null)
		{
			FreeControllerV3 freeControllerV = this.ProcessControllerPossess(this.motionControllerRight);
			if (freeControllerV != null && this.MotionControlPossess(this.motionControllerRight, freeControllerV, true, true))
			{
				flag = true;
				if (this.commonHandModelControl != null && !this._leapHandRightConnected)
				{
					this.commonHandModelControl.rightHandEnabled = false;
				}
				if (this.alternateControllerHandModelControl != null)
				{
					this.alternateControllerHandModelControl.rightHandEnabled = false;
				}
				this.rightPossessedController = freeControllerV;
				HandControl x = freeControllerV.GetComponent<HandControl>();
				if (x == null)
				{
					HandControlLink component = freeControllerV.GetComponent<HandControlLink>();
					if (component != null)
					{
						x = component.handControl;
					}
				}
				if (x != null)
				{
					this.rightPossessHandControl = x;
					this.rightPossessHandControl.possessed = true;
				}
			}
		}
		else
		{
			flag = true;
		}
		if (this.leftPossessedController == null)
		{
			FreeControllerV3 freeControllerV2 = this.ProcessControllerPossess(this.motionControllerLeft);
			if (freeControllerV2 != null && this.MotionControlPossess(this.motionControllerLeft, freeControllerV2, true, true))
			{
				flag2 = true;
				if (this.commonHandModelControl != null && !this._leapHandLeftConnected)
				{
					this.commonHandModelControl.leftHandEnabled = false;
				}
				if (this.alternateControllerHandModelControl != null)
				{
					this.alternateControllerHandModelControl.leftHandEnabled = false;
				}
				this.leftPossessedController = freeControllerV2;
				HandControl x2 = freeControllerV2.GetComponent<HandControl>();
				if (x2 == null)
				{
					HandControlLink component2 = freeControllerV2.GetComponent<HandControlLink>();
					if (component2 != null)
					{
						x2 = component2.handControl;
					}
				}
				if (x2 != null)
				{
					this.leftPossessHandControl = x2;
					this.leftPossessHandControl.possessed = true;
				}
			}
		}
		else
		{
			flag2 = true;
		}
		if (this.leapRightPossessedController == null)
		{
			FreeControllerV3 freeControllerV3 = this.ProcessControllerPossess(this.leapHandRight);
			if (freeControllerV3 != null && this.MotionControlPossess(this.leapHandRight, freeControllerV3, true, true))
			{
				flag = true;
				if (this.commonHandModelControl != null)
				{
					this.commonHandModelControl.rightHandEnabled = false;
				}
				this.leapRightPossessedController = freeControllerV3;
				HandControl x3 = freeControllerV3.GetComponent<HandControl>();
				if (x3 == null)
				{
					HandControlLink component3 = freeControllerV3.GetComponent<HandControlLink>();
					if (component3 != null)
					{
						x3 = component3.handControl;
					}
				}
				if (x3 != null)
				{
					this.leapRightPossessHandControl = x3;
					this.leapRightPossessHandControl.possessed = true;
				}
			}
		}
		else
		{
			flag = true;
		}
		if (this.leapLeftPossessedController == null)
		{
			FreeControllerV3 freeControllerV4 = this.ProcessControllerPossess(this.leapHandLeft);
			if (freeControllerV4 != null && this.MotionControlPossess(this.leapHandLeft, freeControllerV4, true, true))
			{
				flag2 = true;
				if (this.commonHandModelControl != null)
				{
					this.commonHandModelControl.leftHandEnabled = false;
				}
				this.leapLeftPossessedController = freeControllerV4;
				HandControl x4 = freeControllerV4.GetComponent<HandControl>();
				if (x4 == null)
				{
					HandControlLink component4 = freeControllerV4.GetComponent<HandControlLink>();
					if (component4 != null)
					{
						x4 = component4.handControl;
					}
				}
				if (x4 != null)
				{
					this.leapLeftPossessHandControl = x4;
					this.leapLeftPossessHandControl.possessed = true;
				}
			}
		}
		else
		{
			flag2 = true;
		}
		if (this.headPossessedController == null)
		{
			FreeControllerV3 freeControllerV5 = this.ProcessControllerPossess(this.motionControllerHead);
			if (freeControllerV5 != null)
			{
				this.HeadPossess(freeControllerV5, false, true, true);
				flag3 = (this.headPossessedController != null);
			}
		}
		else
		{
			flag3 = true;
		}
		if (this.GetCancel())
		{
			this.ClearPossess();
			this.SelectModeOff();
		}
		if ((flag && flag2 && flag3) || this.GetLeftSelect() || this.GetRightSelect() || this.GetMouseSelect())
		{
			this.SelectModeOff();
		}
	}

	// Token: 0x06005DAE RID: 23982 RVA: 0x00231104 File Offset: 0x0022F504
	private void ProcessTwoStagePossess()
	{
		if (this.rightPossessedController == null && this.rightStartPossessedController == null)
		{
			FreeControllerV3 x = this.ProcessControllerPossess(this.motionControllerRight);
			if (x != null)
			{
				this.rightStartPossessedController = x;
				this.rightStartPossessedController.startedPossess = true;
			}
		}
		if (this.leftPossessedController == null && this.leftStartPossessedController == null)
		{
			FreeControllerV3 x2 = this.ProcessControllerPossess(this.motionControllerLeft);
			if (x2 != null)
			{
				this.leftStartPossessedController = x2;
				this.leftStartPossessedController.startedPossess = true;
			}
		}
		if (this.headPossessedController == null && this.headStartPossessedController == null)
		{
			FreeControllerV3 x3 = this.ProcessControllerPossess(this.motionControllerHead);
			if (x3 != null)
			{
				this.headStartPossessedController = x3;
				this.headStartPossessedController.startedPossess = true;
			}
		}
		if (this.leapRightPossessedController == null && this.leapRightStartPossessedController == null)
		{
			FreeControllerV3 x4 = this.ProcessControllerPossess(this.leapHandRight);
			if (x4 != null)
			{
				this.leapRightStartPossessedController = x4;
				this.leapRightStartPossessedController.startedPossess = true;
			}
		}
		if (this.leapLeftPossessedController == null && this.leapLeftStartPossessedController == null)
		{
			FreeControllerV3 x5 = this.ProcessControllerPossess(this.leapHandLeft);
			if (x5 != null)
			{
				this.leapLeftStartPossessedController = x5;
				this.leapLeftStartPossessedController.startedPossess = true;
			}
		}
		if (this.viveTracker1 != null && this.viveTracker1.gameObject.activeSelf && this.tracker1PossessedController == null && this.tracker1StartPossessedController == null)
		{
			FreeControllerV3 x6 = this.ProcessControllerPossess(this.viveTracker1.transform);
			if (x6 != null)
			{
				this.tracker1StartPossessedController = x6;
				this.tracker1StartPossessedController.startedPossess = true;
			}
		}
		if (this.viveTracker2 != null && this.viveTracker2.gameObject.activeSelf && this.tracker2PossessedController == null && this.tracker2StartPossessedController == null)
		{
			FreeControllerV3 x7 = this.ProcessControllerPossess(this.viveTracker2.transform);
			if (x7 != null)
			{
				this.tracker2StartPossessedController = x7;
				this.tracker2StartPossessedController.startedPossess = true;
			}
		}
		if (this.viveTracker3 != null && this.viveTracker3.gameObject.activeSelf && this.tracker3PossessedController == null && this.tracker3StartPossessedController == null)
		{
			FreeControllerV3 x8 = this.ProcessControllerPossess(this.viveTracker3.transform);
			if (x8 != null)
			{
				this.tracker3StartPossessedController = x8;
				this.tracker3StartPossessedController.startedPossess = true;
			}
		}
		if (this.viveTracker4 != null && this.viveTracker4.gameObject.activeSelf && this.tracker4PossessedController == null && this.tracker4StartPossessedController == null)
		{
			FreeControllerV3 x9 = this.ProcessControllerPossess(this.viveTracker4.transform);
			if (x9 != null)
			{
				this.tracker4StartPossessedController = x9;
				this.tracker4StartPossessedController.startedPossess = true;
			}
		}
		if (this.viveTracker5 != null && this.viveTracker5.gameObject.activeSelf && this.tracker5PossessedController == null && this.tracker5StartPossessedController == null)
		{
			FreeControllerV3 x10 = this.ProcessControllerPossess(this.viveTracker5.transform);
			if (x10 != null)
			{
				this.tracker5StartPossessedController = x10;
				this.tracker5StartPossessedController.startedPossess = true;
			}
		}
		if (this.viveTracker6 != null && this.viveTracker6.gameObject.activeSelf && this.tracker6PossessedController == null && this.tracker6StartPossessedController == null)
		{
			FreeControllerV3 x11 = this.ProcessControllerPossess(this.viveTracker6.transform);
			if (x11 != null)
			{
				this.tracker6StartPossessedController = x11;
				this.tracker6StartPossessedController.startedPossess = true;
			}
		}
		if (this.viveTracker7 != null && this.viveTracker7.gameObject.activeSelf && this.tracker7PossessedController == null && this.tracker7StartPossessedController == null)
		{
			FreeControllerV3 x12 = this.ProcessControllerPossess(this.viveTracker7.transform);
			if (x12 != null)
			{
				this.tracker7StartPossessedController = x12;
				this.tracker7StartPossessedController.startedPossess = true;
			}
		}
		if (this.viveTracker8 != null && this.viveTracker8.gameObject.activeSelf && this.tracker8PossessedController == null && this.tracker8StartPossessedController == null)
		{
			FreeControllerV3 x13 = this.ProcessControllerPossess(this.viveTracker8.transform);
			if (x13 != null)
			{
				this.tracker8StartPossessedController = x13;
				this.tracker8StartPossessedController.startedPossess = true;
			}
		}
		if (this.rightStartPossessedController != null && this.rightTwoStageLineDrawer != null)
		{
			this.rightTwoStageLineDrawer.SetLinePoints(this.motionControllerRight.position, this.rightStartPossessedController.transform.position);
			this.rightTwoStageLineDrawer.Draw(base.gameObject.layer);
		}
		if (this.leftStartPossessedController != null && this.leftTwoStageLineDrawer != null)
		{
			this.leftTwoStageLineDrawer.SetLinePoints(this.motionControllerLeft.position, this.leftStartPossessedController.transform.position);
			this.leftTwoStageLineDrawer.Draw(base.gameObject.layer);
		}
		if (this.headStartPossessedController != null && this.headTwoStageLineDrawer != null)
		{
			this.headTwoStageLineDrawer.SetLinePoints(this.motionControllerHead.position, this.headStartPossessedController.transform.position);
			this.headTwoStageLineDrawer.Draw(base.gameObject.layer);
		}
		if (this.leapRightStartPossessedController != null && this.leapRightTwoStageLineDrawer != null)
		{
			this.leapRightTwoStageLineDrawer.SetLinePoints(this.leapHandRight.transform.position, this.leapRightStartPossessedController.transform.position);
			this.leapRightTwoStageLineDrawer.Draw(base.gameObject.layer);
		}
		if (this.leapLeftStartPossessedController != null && this.leapLeftTwoStageLineDrawer != null)
		{
			this.leapLeftTwoStageLineDrawer.SetLinePoints(this.leapHandLeft.transform.position, this.leapLeftStartPossessedController.transform.position);
			this.leapLeftTwoStageLineDrawer.Draw(base.gameObject.layer);
		}
		if (this.tracker1StartPossessedController != null && this.tracker1TwoStageLineDrawer != null)
		{
			this.tracker1TwoStageLineDrawer.SetLinePoints(this.viveTracker1.transform.position, this.tracker1StartPossessedController.transform.position);
			this.tracker1TwoStageLineDrawer.Draw(base.gameObject.layer);
		}
		if (this.tracker2StartPossessedController != null && this.tracker2TwoStageLineDrawer != null)
		{
			this.tracker2TwoStageLineDrawer.SetLinePoints(this.viveTracker2.transform.position, this.tracker2StartPossessedController.transform.position);
			this.tracker2TwoStageLineDrawer.Draw(base.gameObject.layer);
		}
		if (this.tracker3StartPossessedController != null && this.tracker3TwoStageLineDrawer != null)
		{
			this.tracker3TwoStageLineDrawer.SetLinePoints(this.viveTracker3.transform.position, this.tracker3StartPossessedController.transform.position);
			this.tracker3TwoStageLineDrawer.Draw(base.gameObject.layer);
		}
		if (this.tracker4StartPossessedController != null && this.tracker4TwoStageLineDrawer != null)
		{
			this.tracker4TwoStageLineDrawer.SetLinePoints(this.viveTracker4.transform.position, this.tracker4StartPossessedController.transform.position);
			this.tracker4TwoStageLineDrawer.Draw(base.gameObject.layer);
		}
		if (this.tracker5StartPossessedController != null && this.tracker5TwoStageLineDrawer != null)
		{
			this.tracker5TwoStageLineDrawer.SetLinePoints(this.viveTracker5.transform.position, this.tracker5StartPossessedController.transform.position);
			this.tracker5TwoStageLineDrawer.Draw(base.gameObject.layer);
		}
		if (this.tracker6StartPossessedController != null && this.tracker6TwoStageLineDrawer != null)
		{
			this.tracker6TwoStageLineDrawer.SetLinePoints(this.viveTracker6.transform.position, this.tracker6StartPossessedController.transform.position);
			this.tracker6TwoStageLineDrawer.Draw(base.gameObject.layer);
		}
		if (this.tracker7StartPossessedController != null && this.tracker7TwoStageLineDrawer != null)
		{
			this.tracker7TwoStageLineDrawer.SetLinePoints(this.viveTracker7.transform.position, this.tracker7StartPossessedController.transform.position);
			this.tracker7TwoStageLineDrawer.Draw(base.gameObject.layer);
		}
		if (this.tracker8StartPossessedController != null && this.tracker8TwoStageLineDrawer != null)
		{
			this.tracker8TwoStageLineDrawer.SetLinePoints(this.viveTracker8.transform.position, this.tracker8StartPossessedController.transform.position);
			this.tracker8TwoStageLineDrawer.Draw(base.gameObject.layer);
		}
		if (this.GetCancel())
		{
			this.ClearPossess();
			this.SelectModeOff();
		}
		if (this.GetLeftSelect() || this.GetRightSelect() || this.GetMouseSelect())
		{
			this.CompleteTwoStagePossess();
		}
	}

	// Token: 0x06005DAF RID: 23983 RVA: 0x00231B64 File Offset: 0x0022FF64
	protected void CompleteTwoStagePossess()
	{
		if (this.rightStartPossessedController != null)
		{
			if (this.MotionControlPossess(this.motionControllerRight, this.rightStartPossessedController, false, false))
			{
				this.rightPossessedController = this.rightStartPossessedController;
				HandControl x = this.rightPossessedController.GetComponent<HandControl>();
				if (x == null)
				{
					HandControlLink component = this.rightPossessedController.GetComponent<HandControlLink>();
					if (component != null)
					{
						x = component.handControl;
					}
				}
				if (x != null)
				{
					this.rightPossessHandControl = x;
					this.rightPossessHandControl.possessed = true;
				}
			}
			this.rightStartPossessedController.startedPossess = false;
			this.rightStartPossessedController = null;
		}
		if (this.leftStartPossessedController != null)
		{
			if (this.MotionControlPossess(this.motionControllerLeft, this.leftStartPossessedController, false, false))
			{
				this.leftPossessedController = this.leftStartPossessedController;
				HandControl x2 = this.leftPossessedController.GetComponent<HandControl>();
				if (x2 == null)
				{
					HandControlLink component2 = this.leftPossessedController.GetComponent<HandControlLink>();
					if (component2 != null)
					{
						x2 = component2.handControl;
					}
				}
				if (x2 != null)
				{
					this.leftPossessHandControl = x2;
					this.leftPossessHandControl.possessed = true;
				}
			}
			this.leftStartPossessedController.startedPossess = false;
			this.leftStartPossessedController = null;
		}
		if (this.headStartPossessedController != null)
		{
			this.HeadPossess(this.headStartPossessedController, false, false, false);
			this.headStartPossessedController.startedPossess = false;
			this.headStartPossessedController = null;
		}
		if (this.leapRightStartPossessedController != null)
		{
			if (this.MotionControlPossess(this.leapHandRight, this.leapRightStartPossessedController, false, false))
			{
				this.leapRightPossessedController = this.leapRightStartPossessedController;
				HandControl x3 = this.leapRightPossessedController.GetComponent<HandControl>();
				if (x3 == null)
				{
					HandControlLink component3 = this.leapRightPossessedController.GetComponent<HandControlLink>();
					if (component3 != null)
					{
						x3 = component3.handControl;
					}
				}
				if (x3 != null)
				{
					this.leapRightPossessHandControl = x3;
					this.leapRightPossessHandControl.possessed = true;
				}
			}
			this.leapRightStartPossessedController.startedPossess = false;
			this.leapRightStartPossessedController = null;
		}
		if (this.leapLeftStartPossessedController != null)
		{
			if (this.MotionControlPossess(this.leapHandLeft, this.leapLeftStartPossessedController, false, false))
			{
				this.leapLeftPossessedController = this.leapLeftStartPossessedController;
				HandControl x4 = this.leapLeftPossessedController.GetComponent<HandControl>();
				if (x4 == null)
				{
					HandControlLink component4 = this.leapLeftPossessedController.GetComponent<HandControlLink>();
					if (component4 != null)
					{
						x4 = component4.handControl;
					}
				}
				if (x4 != null)
				{
					this.leapLeftPossessHandControl = x4;
					this.leapLeftPossessHandControl.possessed = true;
				}
			}
			this.leapLeftStartPossessedController.startedPossess = false;
			this.leapLeftStartPossessedController = null;
		}
		if (this.tracker1StartPossessedController != null)
		{
			if (this.MotionControlPossess(this.viveTracker1.transform, this.tracker1StartPossessedController, false, false))
			{
				this.tracker1PossessedController = this.tracker1StartPossessedController;
				this.tracker1Visible = false;
			}
			this.tracker1StartPossessedController.startedPossess = false;
			this.tracker1StartPossessedController = null;
		}
		if (this.tracker2StartPossessedController != null)
		{
			if (this.MotionControlPossess(this.viveTracker2.transform, this.tracker2StartPossessedController, false, false))
			{
				this.tracker2PossessedController = this.tracker2StartPossessedController;
				this.tracker2Visible = false;
			}
			this.tracker2StartPossessedController.startedPossess = false;
			this.tracker2StartPossessedController = null;
		}
		if (this.tracker3StartPossessedController != null)
		{
			if (this.MotionControlPossess(this.viveTracker3.transform, this.tracker3StartPossessedController, false, false))
			{
				this.tracker3PossessedController = this.tracker3StartPossessedController;
				this.tracker3Visible = false;
			}
			this.tracker3StartPossessedController.startedPossess = false;
			this.tracker3StartPossessedController = null;
		}
		if (this.tracker4StartPossessedController != null)
		{
			if (this.MotionControlPossess(this.viveTracker4.transform, this.tracker4StartPossessedController, false, false))
			{
				this.tracker4PossessedController = this.tracker4StartPossessedController;
				this.tracker4Visible = false;
			}
			this.tracker4StartPossessedController.startedPossess = false;
			this.tracker4StartPossessedController = null;
		}
		if (this.tracker5StartPossessedController != null)
		{
			if (this.MotionControlPossess(this.viveTracker5.transform, this.tracker5StartPossessedController, false, false))
			{
				this.tracker5PossessedController = this.tracker5StartPossessedController;
				this.tracker5Visible = false;
			}
			this.tracker5StartPossessedController.startedPossess = false;
			this.tracker5StartPossessedController = null;
		}
		if (this.tracker6StartPossessedController != null)
		{
			if (this.MotionControlPossess(this.viveTracker6.transform, this.tracker6StartPossessedController, false, false))
			{
				this.tracker6PossessedController = this.tracker6StartPossessedController;
				this.tracker6Visible = false;
			}
			this.tracker6StartPossessedController.startedPossess = false;
			this.tracker6StartPossessedController = null;
		}
		if (this.tracker7StartPossessedController != null)
		{
			if (this.MotionControlPossess(this.viveTracker7.transform, this.tracker7StartPossessedController, false, false))
			{
				this.tracker7PossessedController = this.tracker7StartPossessedController;
				this.tracker7Visible = false;
			}
			this.tracker7StartPossessedController.startedPossess = false;
			this.tracker7StartPossessedController = null;
		}
		if (this.tracker8StartPossessedController != null)
		{
			if (this.MotionControlPossess(this.viveTracker8.transform, this.tracker8StartPossessedController, false, false))
			{
				this.tracker8PossessedController = this.tracker8StartPossessedController;
				this.tracker8Visible = false;
			}
			this.tracker8StartPossessedController.startedPossess = false;
			this.tracker8StartPossessedController = null;
		}
		this.SelectModeOff();
	}

	// Token: 0x06005DB0 RID: 23984 RVA: 0x002320DE File Offset: 0x002304DE
	public void ClearHeadPossess()
	{
		if (this.headPossessedController != null)
		{
			this.ClearPossess(false, this.headPossessedController);
		}
	}

	// Token: 0x06005DB1 RID: 23985 RVA: 0x002320FE File Offset: 0x002304FE
	public void ClearPossess()
	{
		this.ClearPossess(false, null);
	}

	// Token: 0x06005DB2 RID: 23986 RVA: 0x00232108 File Offset: 0x00230508
	public void ClearPossess(bool excludeHeadClear)
	{
		this.ClearPossess(excludeHeadClear, null);
	}

	// Token: 0x06005DB3 RID: 23987 RVA: 0x00232114 File Offset: 0x00230514
	public void ClearPossess(bool excludeHeadClear, FreeControllerV3 specificController)
	{
		if (this.selectMode == SuperController.SelectMode.Possess || this.selectMode == SuperController.SelectMode.TwoStagePossess || this.selectMode == SuperController.SelectMode.PossessAndAlign)
		{
			this.SelectModeOff();
		}
		if (this.leftPossessedController != null && (specificController == null || this.leftPossessedController == specificController))
		{
			this.leftPossessedController.RestorePreLinkState();
			this.leftPossessedController.possessed = false;
			this.leftPossessedController.startedPossess = false;
			MotionAnimationControl component = this.leftPossessedController.GetComponent<MotionAnimationControl>();
			if (component != null)
			{
				component.suspendPositionPlayback = false;
				component.suspendRotationPlayback = false;
			}
			this.leftPossessedController = null;
			if (this.leftPossessHandControl != null)
			{
				this.leftPossessHandControl.possessed = false;
			}
			this.leftPossessHandControl = null;
			if (this.alternateControllerHandModelControl != null)
			{
				this.alternateControllerHandModelControl.leftHandEnabled = true;
			}
		}
		if (this.leftStartPossessedController != null)
		{
			this.leftStartPossessedController.startedPossess = false;
			this.leftStartPossessedController = null;
		}
		if (this.rightPossessedController != null && (specificController == null || this.rightPossessedController == specificController))
		{
			this.rightPossessedController.RestorePreLinkState();
			this.rightPossessedController.possessed = false;
			this.rightPossessedController.startedPossess = false;
			MotionAnimationControl component2 = this.rightPossessedController.GetComponent<MotionAnimationControl>();
			if (component2 != null)
			{
				component2.suspendPositionPlayback = false;
				component2.suspendRotationPlayback = false;
			}
			this.rightPossessedController = null;
			if (this.rightPossessHandControl != null)
			{
				this.rightPossessHandControl.possessed = false;
			}
			this.rightPossessHandControl = null;
			if (this.alternateControllerHandModelControl != null)
			{
				this.alternateControllerHandModelControl.rightHandEnabled = true;
			}
		}
		if (this.rightStartPossessedController != null)
		{
			this.rightStartPossessedController.startedPossess = false;
			this.rightStartPossessedController = null;
		}
		if (this.leapRightPossessedController != null && (specificController == null || this.leapRightPossessedController == specificController))
		{
			this.leapRightPossessedController.RestorePreLinkState();
			this.leapRightPossessedController.possessed = false;
			this.leapRightPossessedController.startedPossess = false;
			MotionAnimationControl component3 = this.leapRightPossessedController.GetComponent<MotionAnimationControl>();
			if (component3 != null)
			{
				component3.suspendPositionPlayback = false;
				component3.suspendRotationPlayback = false;
			}
			this.leapRightPossessedController = null;
			if (this.leapRightPossessHandControl != null)
			{
				this.leapRightPossessHandControl.possessed = false;
			}
			this.rightPossessHandControl = null;
		}
		if (this.leapRightStartPossessedController != null)
		{
			this.leapRightStartPossessedController.startedPossess = false;
			this.leapRightStartPossessedController = null;
		}
		if (this.leapLeftPossessedController != null && (specificController == null || this.leapLeftPossessedController == specificController))
		{
			this.leapLeftPossessedController.RestorePreLinkState();
			this.leapLeftPossessedController.possessed = false;
			this.leapLeftPossessedController.startedPossess = false;
			MotionAnimationControl component4 = this.leapLeftPossessedController.GetComponent<MotionAnimationControl>();
			if (component4 != null)
			{
				component4.suspendPositionPlayback = false;
				component4.suspendRotationPlayback = false;
			}
			this.leapLeftPossessedController = null;
			if (this.leapLeftPossessHandControl != null)
			{
				this.leapLeftPossessHandControl.possessed = false;
			}
			this.rightPossessHandControl = null;
		}
		if (this.leapLeftStartPossessedController != null)
		{
			this.leapLeftStartPossessedController.startedPossess = false;
			this.leapLeftStartPossessedController = null;
		}
		if (this.tracker1PossessedController != null && (specificController == null || this.tracker1PossessedController == specificController))
		{
			this.tracker1PossessedController.RestorePreLinkState();
			this.tracker1PossessedController.possessed = false;
			this.tracker1PossessedController.startedPossess = false;
			MotionAnimationControl component5 = this.tracker1PossessedController.GetComponent<MotionAnimationControl>();
			if (component5 != null)
			{
				component5.suspendPositionPlayback = false;
				component5.suspendRotationPlayback = false;
			}
			this.tracker1PossessedController = null;
			this.tracker1Visible = true;
		}
		if (this.tracker1StartPossessedController != null)
		{
			this.tracker1StartPossessedController.startedPossess = false;
			this.tracker1StartPossessedController = null;
		}
		if (this.tracker2PossessedController != null && (specificController == null || this.tracker2PossessedController == specificController))
		{
			this.tracker2PossessedController.RestorePreLinkState();
			this.tracker2PossessedController.possessed = false;
			this.tracker2PossessedController.startedPossess = false;
			MotionAnimationControl component6 = this.tracker2PossessedController.GetComponent<MotionAnimationControl>();
			if (component6 != null)
			{
				component6.suspendPositionPlayback = false;
				component6.suspendRotationPlayback = false;
			}
			this.tracker2PossessedController = null;
			this.tracker2Visible = true;
		}
		if (this.tracker2StartPossessedController != null)
		{
			this.tracker2StartPossessedController.startedPossess = false;
			this.tracker2StartPossessedController = null;
		}
		if (this.tracker3PossessedController != null && (specificController == null || this.tracker3PossessedController == specificController))
		{
			this.tracker3PossessedController.RestorePreLinkState();
			this.tracker3PossessedController.possessed = false;
			this.tracker3PossessedController.startedPossess = false;
			MotionAnimationControl component7 = this.tracker3PossessedController.GetComponent<MotionAnimationControl>();
			if (component7 != null)
			{
				component7.suspendPositionPlayback = false;
				component7.suspendRotationPlayback = false;
			}
			this.tracker3PossessedController = null;
			this.tracker3Visible = true;
		}
		if (this.tracker3StartPossessedController != null)
		{
			this.tracker3StartPossessedController.startedPossess = false;
			this.tracker3StartPossessedController = null;
		}
		if (this.tracker4PossessedController != null && (specificController == null || this.tracker4PossessedController == specificController))
		{
			this.tracker4PossessedController.RestorePreLinkState();
			this.tracker4PossessedController.possessed = false;
			this.tracker4PossessedController.startedPossess = false;
			MotionAnimationControl component8 = this.tracker4PossessedController.GetComponent<MotionAnimationControl>();
			if (component8 != null)
			{
				component8.suspendPositionPlayback = false;
				component8.suspendRotationPlayback = false;
			}
			this.tracker4PossessedController = null;
			this.tracker4Visible = true;
		}
		if (this.tracker5StartPossessedController != null)
		{
			this.tracker5StartPossessedController.startedPossess = false;
			this.tracker5StartPossessedController = null;
		}
		if (this.tracker5PossessedController != null && (specificController == null || this.tracker5PossessedController == specificController))
		{
			this.tracker5PossessedController.RestorePreLinkState();
			this.tracker5PossessedController.possessed = false;
			this.tracker5PossessedController.startedPossess = false;
			MotionAnimationControl component9 = this.tracker5PossessedController.GetComponent<MotionAnimationControl>();
			if (component9 != null)
			{
				component9.suspendPositionPlayback = false;
				component9.suspendRotationPlayback = false;
			}
			this.tracker5PossessedController = null;
			this.tracker5Visible = true;
		}
		if (this.tracker6StartPossessedController != null)
		{
			this.tracker6StartPossessedController.startedPossess = false;
			this.tracker6StartPossessedController = null;
		}
		if (this.tracker6PossessedController != null && (specificController == null || this.tracker6PossessedController == specificController))
		{
			this.tracker6PossessedController.RestorePreLinkState();
			this.tracker6PossessedController.possessed = false;
			this.tracker6PossessedController.startedPossess = false;
			MotionAnimationControl component10 = this.tracker6PossessedController.GetComponent<MotionAnimationControl>();
			if (component10 != null)
			{
				component10.suspendPositionPlayback = false;
				component10.suspendRotationPlayback = false;
			}
			this.tracker6PossessedController = null;
			this.tracker6Visible = true;
		}
		if (this.tracker7StartPossessedController != null)
		{
			this.tracker7StartPossessedController.startedPossess = false;
			this.tracker7StartPossessedController = null;
		}
		if (this.tracker7PossessedController != null && (specificController == null || this.tracker7PossessedController == specificController))
		{
			this.tracker7PossessedController.RestorePreLinkState();
			this.tracker7PossessedController.possessed = false;
			this.tracker7PossessedController.startedPossess = false;
			MotionAnimationControl component11 = this.tracker7PossessedController.GetComponent<MotionAnimationControl>();
			if (component11 != null)
			{
				component11.suspendPositionPlayback = false;
				component11.suspendRotationPlayback = false;
			}
			this.tracker7PossessedController = null;
			this.tracker7Visible = true;
		}
		if (this.tracker8StartPossessedController != null)
		{
			this.tracker8StartPossessedController.startedPossess = false;
			this.tracker8StartPossessedController = null;
		}
		if (this.tracker8PossessedController != null && (specificController == null || this.tracker8PossessedController == specificController))
		{
			this.tracker8PossessedController.RestorePreLinkState();
			this.tracker8PossessedController.possessed = false;
			this.tracker8PossessedController.startedPossess = false;
			MotionAnimationControl component12 = this.tracker8PossessedController.GetComponent<MotionAnimationControl>();
			if (component12 != null)
			{
				component12.suspendPositionPlayback = false;
				component12.suspendRotationPlayback = false;
			}
			this.tracker8PossessedController = null;
			this.tracker8Visible = true;
		}
		if (this.headPossessedController != null && !excludeHeadClear && (specificController == null || this.headPossessedController == specificController))
		{
			this.headPossessedController.RestorePreLinkState();
			this.headPossessedController.possessed = false;
			MotionAnimationControl component13 = this.headPossessedController.GetComponent<MotionAnimationControl>();
			if (component13 != null)
			{
				component13.suspendPositionPlayback = false;
				component13.suspendRotationPlayback = false;
			}
			this.headPossessedController = null;
			if (this.headPossessedActivateTransform != null)
			{
				this.headPossessedActivateTransform.gameObject.SetActive(false);
			}
		}
		if (this.headStartPossessedController != null)
		{
			this.headStartPossessedController.startedPossess = false;
			this.headStartPossessedController = null;
		}
	}

	// Token: 0x06005DB4 RID: 23988 RVA: 0x00232AA8 File Offset: 0x00230EA8
	protected void VerifyPossess()
	{
		if (this.rightPossessedController != null)
		{
			Rigidbody component = this.motionControllerRight.GetComponent<Rigidbody>();
			if (component != null && this.rightPossessedController.linkToRB != component)
			{
				this.ClearPossess(true, this.rightPossessedController);
			}
			else if (this.rightPossessedController.currentPositionState != FreeControllerV3.PositionState.ParentLink && this.rightPossessedController.currentPositionState != FreeControllerV3.PositionState.PhysicsLink && this.rightPossessedController.currentRotationState != FreeControllerV3.RotationState.ParentLink && this.rightPossessedController.currentRotationState != FreeControllerV3.RotationState.PhysicsLink)
			{
				this.ClearPossess(true, this.rightPossessedController);
			}
		}
		if (this.leftPossessedController != null)
		{
			Rigidbody component2 = this.motionControllerLeft.GetComponent<Rigidbody>();
			if (component2 != null && this.leftPossessedController.linkToRB != component2)
			{
				this.ClearPossess(true, this.leftPossessedController);
			}
			else if (this.leftPossessedController.currentPositionState != FreeControllerV3.PositionState.ParentLink && this.leftPossessedController.currentPositionState != FreeControllerV3.PositionState.PhysicsLink && this.leftPossessedController.currentRotationState != FreeControllerV3.RotationState.ParentLink && this.leftPossessedController.currentRotationState != FreeControllerV3.RotationState.PhysicsLink)
			{
				this.ClearPossess(true, this.leftPossessedController);
			}
		}
		if (this.headPossessedController != null)
		{
			Rigidbody component3 = this.motionControllerHead.GetComponent<Rigidbody>();
			if (component3 != null && this.headPossessedController.linkToRB != component3)
			{
				this.ClearPossess(true, this.headPossessedController);
			}
			else if (this.headPossessedController.currentPositionState != FreeControllerV3.PositionState.ParentLink && this.headPossessedController.currentPositionState != FreeControllerV3.PositionState.PhysicsLink && this.headPossessedController.currentRotationState != FreeControllerV3.RotationState.ParentLink && this.headPossessedController.currentRotationState != FreeControllerV3.RotationState.PhysicsLink)
			{
				this.ClearPossess(true, this.headPossessedController);
			}
		}
		if (this.leapRightPossessedController != null)
		{
			Rigidbody component4 = this.leapHandRight.GetComponent<Rigidbody>();
			if (component4 != null && this.leapRightPossessedController.linkToRB != component4)
			{
				this.ClearPossess(true, this.leapRightPossessedController);
			}
			else if (this.leapRightPossessedController.currentPositionState != FreeControllerV3.PositionState.ParentLink && this.leapRightPossessedController.currentPositionState != FreeControllerV3.PositionState.PhysicsLink && this.leapRightPossessedController.currentRotationState != FreeControllerV3.RotationState.ParentLink && this.leapRightPossessedController.currentRotationState != FreeControllerV3.RotationState.PhysicsLink)
			{
				this.ClearPossess(true, this.leapRightPossessedController);
			}
		}
		if (this.leapLeftPossessedController != null)
		{
			Rigidbody component5 = this.leapHandLeft.GetComponent<Rigidbody>();
			if (component5 != null && this.leapLeftPossessedController.linkToRB != component5)
			{
				this.ClearPossess(true, this.leapLeftPossessedController);
			}
			else if (this.leapLeftPossessedController.currentPositionState != FreeControllerV3.PositionState.ParentLink && this.leapLeftPossessedController.currentPositionState != FreeControllerV3.PositionState.PhysicsLink && this.leapLeftPossessedController.currentRotationState != FreeControllerV3.RotationState.ParentLink && this.leapLeftPossessedController.currentRotationState != FreeControllerV3.RotationState.PhysicsLink)
			{
				this.ClearPossess(true, this.leapLeftPossessedController);
			}
		}
		if (this.tracker1PossessedController != null)
		{
			Rigidbody component6 = this.viveTracker1.GetComponent<Rigidbody>();
			if (component6 != null && this.tracker1PossessedController.linkToRB != component6)
			{
				this.ClearPossess(true, this.tracker1PossessedController);
			}
			else if (this.tracker1PossessedController.currentPositionState != FreeControllerV3.PositionState.ParentLink && this.tracker1PossessedController.currentPositionState != FreeControllerV3.PositionState.PhysicsLink && this.tracker1PossessedController.currentRotationState != FreeControllerV3.RotationState.ParentLink && this.tracker1PossessedController.currentRotationState != FreeControllerV3.RotationState.PhysicsLink)
			{
				this.ClearPossess(true, this.tracker1PossessedController);
			}
		}
		if (this.tracker2PossessedController != null)
		{
			Rigidbody component7 = this.viveTracker2.GetComponent<Rigidbody>();
			if (component7 != null && this.tracker2PossessedController.linkToRB != component7)
			{
				this.ClearPossess(true, this.tracker2PossessedController);
			}
			else if (this.tracker2PossessedController.currentPositionState != FreeControllerV3.PositionState.ParentLink && this.tracker2PossessedController.currentPositionState != FreeControllerV3.PositionState.PhysicsLink && this.tracker2PossessedController.currentRotationState != FreeControllerV3.RotationState.ParentLink && this.tracker2PossessedController.currentRotationState != FreeControllerV3.RotationState.PhysicsLink)
			{
				this.ClearPossess(true, this.tracker2PossessedController);
			}
		}
		if (this.tracker3PossessedController != null)
		{
			Rigidbody component8 = this.viveTracker3.GetComponent<Rigidbody>();
			if (component8 != null && this.tracker3PossessedController.linkToRB != component8)
			{
				this.ClearPossess(true, this.tracker3PossessedController);
			}
			else if (this.tracker3PossessedController.currentPositionState != FreeControllerV3.PositionState.ParentLink && this.tracker3PossessedController.currentPositionState != FreeControllerV3.PositionState.PhysicsLink && this.tracker3PossessedController.currentRotationState != FreeControllerV3.RotationState.ParentLink && this.tracker3PossessedController.currentRotationState != FreeControllerV3.RotationState.PhysicsLink)
			{
				this.ClearPossess(true, this.tracker3PossessedController);
			}
		}
		if (this.tracker4PossessedController != null)
		{
			Rigidbody component9 = this.viveTracker4.GetComponent<Rigidbody>();
			if (component9 != null && this.tracker4PossessedController.linkToRB != component9)
			{
				this.ClearPossess(true, this.tracker4PossessedController);
			}
			else if (this.tracker4PossessedController.currentPositionState != FreeControllerV3.PositionState.ParentLink && this.tracker4PossessedController.currentPositionState != FreeControllerV3.PositionState.PhysicsLink && this.tracker4PossessedController.currentRotationState != FreeControllerV3.RotationState.ParentLink && this.tracker4PossessedController.currentRotationState != FreeControllerV3.RotationState.PhysicsLink)
			{
				this.ClearPossess(true, this.tracker4PossessedController);
			}
		}
		if (this.tracker5PossessedController != null)
		{
			Rigidbody component10 = this.viveTracker5.GetComponent<Rigidbody>();
			if (component10 != null && this.tracker5PossessedController.linkToRB != component10)
			{
				this.ClearPossess(true, this.tracker5PossessedController);
			}
			else if (this.tracker5PossessedController.currentPositionState != FreeControllerV3.PositionState.ParentLink && this.tracker5PossessedController.currentPositionState != FreeControllerV3.PositionState.PhysicsLink && this.tracker5PossessedController.currentRotationState != FreeControllerV3.RotationState.ParentLink && this.tracker5PossessedController.currentRotationState != FreeControllerV3.RotationState.PhysicsLink)
			{
				this.ClearPossess(true, this.tracker5PossessedController);
			}
		}
		if (this.tracker6PossessedController != null)
		{
			Rigidbody component11 = this.viveTracker6.GetComponent<Rigidbody>();
			if (component11 != null && this.tracker6PossessedController.linkToRB != component11)
			{
				this.ClearPossess(true, this.tracker6PossessedController);
			}
			else if (this.tracker6PossessedController.currentPositionState != FreeControllerV3.PositionState.ParentLink && this.tracker6PossessedController.currentPositionState != FreeControllerV3.PositionState.PhysicsLink && this.tracker6PossessedController.currentRotationState != FreeControllerV3.RotationState.ParentLink && this.tracker6PossessedController.currentRotationState != FreeControllerV3.RotationState.PhysicsLink)
			{
				this.ClearPossess(true, this.tracker6PossessedController);
			}
		}
		if (this.tracker7PossessedController != null)
		{
			Rigidbody component12 = this.viveTracker7.GetComponent<Rigidbody>();
			if (component12 != null && this.tracker7PossessedController.linkToRB != component12)
			{
				this.ClearPossess(true, this.tracker7PossessedController);
			}
			else if (this.tracker7PossessedController.currentPositionState != FreeControllerV3.PositionState.ParentLink && this.tracker7PossessedController.currentPositionState != FreeControllerV3.PositionState.PhysicsLink && this.tracker7PossessedController.currentRotationState != FreeControllerV3.RotationState.ParentLink && this.tracker7PossessedController.currentRotationState != FreeControllerV3.RotationState.PhysicsLink)
			{
				this.ClearPossess(true, this.tracker7PossessedController);
			}
		}
		if (this.tracker8PossessedController != null)
		{
			Rigidbody component13 = this.viveTracker8.GetComponent<Rigidbody>();
			if (component13 != null && this.tracker8PossessedController.linkToRB != component13)
			{
				this.ClearPossess(true, this.tracker8PossessedController);
			}
			else if (this.tracker8PossessedController.currentPositionState != FreeControllerV3.PositionState.ParentLink && this.tracker8PossessedController.currentPositionState != FreeControllerV3.PositionState.PhysicsLink && this.tracker8PossessedController.currentRotationState != FreeControllerV3.RotationState.ParentLink && this.tracker8PossessedController.currentRotationState != FreeControllerV3.RotationState.PhysicsLink)
			{
				this.ClearPossess(true, this.tracker8PossessedController);
			}
		}
	}

	// Token: 0x06005DB5 RID: 23989 RVA: 0x0023330C File Offset: 0x0023170C
	public void StopPlayback()
	{
		if (this.currentAnimationMaster != null)
		{
			if (this.currentAnimationMaster.isRecording)
			{
				this.currentAnimationMaster.StopRecord();
				this.SelectModeOff();
			}
			else
			{
				this.currentAnimationMaster.StopPlayback();
			}
		}
	}

	// Token: 0x06005DB6 RID: 23990 RVA: 0x0023335B File Offset: 0x0023175B
	public void StopRecording()
	{
		if (this.currentAnimationMaster != null && this.currentAnimationMaster.isRecording)
		{
			this.currentAnimationMaster.StopRecord();
			this.SelectModeOff();
		}
	}

	// Token: 0x06005DB7 RID: 23991 RVA: 0x0023338F File Offset: 0x0023178F
	public void StartPlayback()
	{
		if (this.currentAnimationMaster != null)
		{
			this.currentAnimationMaster.StartPlayback();
		}
	}

	// Token: 0x06005DB8 RID: 23992 RVA: 0x002333B0 File Offset: 0x002317B0
	public void ProcessAnimationRecord()
	{
		if (this.currentAnimationMaster != null)
		{
			if (this.currentAnimationMaster.isRecording)
			{
				this.helpText = "Recording...press Select or Spacebar to stop recording\n" + this.currentAnimationMaster.playbackCounter.ToString("F0");
			}
			if (this.GetLeftSelect() || this.GetRightSelect() || this.GetMouseSelect() || Input.GetKeyDown(KeyCode.Space))
			{
				if (this.currentAnimationMaster.isRecording)
				{
					this.StopPlayback();
				}
				else
				{
					this.helpText = "Recording...press Select or Spacebar key to stop recording\n" + this.currentAnimationMaster.playbackCounter.ToString("F0");
					this.helpColor = Color.red;
					if (this.currentAnimationMaster != null)
					{
						this.currentAnimationMaster.StartRecord();
					}
				}
			}
		}
	}

	// Token: 0x06005DB9 RID: 23993 RVA: 0x002334A0 File Offset: 0x002318A0
	public void ArmAllControlledControllersForRecord(ICollection<MotionAnimationControl> filteredMacs = null)
	{
		foreach (FreeControllerV3 freeControllerV in this.allControllers)
		{
			if (freeControllerV.currentPositionState == FreeControllerV3.PositionState.ParentLink || freeControllerV.currentPositionState == FreeControllerV3.PositionState.PhysicsLink || freeControllerV.currentRotationState == FreeControllerV3.RotationState.ParentLink || freeControllerV.currentRotationState == FreeControllerV3.RotationState.PhysicsLink)
			{
				MotionAnimationControl component = freeControllerV.GetComponent<MotionAnimationControl>();
				if (component != null)
				{
					if (filteredMacs == null || filteredMacs.Contains(component))
					{
						if (this.CheckIfControllerLinkedToMotionControl(this.motionControllerLeft, freeControllerV) || this.CheckIfControllerLinkedToMotionControl(this.motionControllerRight, freeControllerV) || this.CheckIfControllerLinkedToMotionControl(this.motionControllerHead, freeControllerV) || (this.viveTracker1 != null && this.CheckIfControllerLinkedToMotionControl(this.viveTracker1.transform, freeControllerV)) || (this.viveTracker2 != null && this.CheckIfControllerLinkedToMotionControl(this.viveTracker2.transform, freeControllerV)) || (this.viveTracker3 != null && this.CheckIfControllerLinkedToMotionControl(this.viveTracker3.transform, freeControllerV)) || (this.viveTracker4 != null && this.CheckIfControllerLinkedToMotionControl(this.viveTracker4.transform, freeControllerV)) || (this.viveTracker5 != null && this.CheckIfControllerLinkedToMotionControl(this.viveTracker5.transform, freeControllerV)) || (this.viveTracker6 != null && this.CheckIfControllerLinkedToMotionControl(this.viveTracker6.transform, freeControllerV)) || (this.viveTracker7 != null && this.CheckIfControllerLinkedToMotionControl(this.viveTracker7.transform, freeControllerV)) || (this.viveTracker8 != null && this.CheckIfControllerLinkedToMotionControl(this.viveTracker8.transform, freeControllerV)))
						{
							component.armedForRecord = true;
						}
					}
				}
			}
		}
	}

	// Token: 0x17000DC4 RID: 3524
	// (get) Token: 0x06005DBA RID: 23994 RVA: 0x002336D8 File Offset: 0x00231AD8
	public bool assetManagerReady
	{
		get
		{
			return this._assetManagerReady;
		}
	}

	// Token: 0x06005DBB RID: 23995 RVA: 0x002336E0 File Offset: 0x00231AE0
	public static IEnumerator AssetManagerReady()
	{
		if (SuperController.<AssetManagerReady>c__Iterator2.<>f__am$cache0 == null)
		{
			SuperController.<AssetManagerReady>c__Iterator2.<>f__am$cache0 = new Func<bool>(SuperController.<AssetManagerReady>c__Iterator2.<>m__0);
		}
		yield return new WaitUntil(SuperController.<AssetManagerReady>c__Iterator2.<>f__am$cache0);
		yield break;
	}

	// Token: 0x06005DBC RID: 23996 RVA: 0x002336F4 File Offset: 0x00231AF4
	private IEnumerator InitAssetManager()
	{
		AssetBundleManager.SetSourceAssetBundleDirectory(Application.streamingAssetsPath + "/");
		AssetBundleLoadManifestOperation request = AssetBundleManager.Initialize();
		if (request != null)
		{
			yield return base.StartCoroutine(request);
		}
		UnityEngine.Debug.Log("Asset Manager Ready");
		this._assetManagerReady = true;
		yield break;
	}

	// Token: 0x06005DBD RID: 23997 RVA: 0x0023370F File Offset: 0x00231B0F
	protected void InitAssetBundleDictionaries()
	{
		if (this.assetBundleAssetNameToPrefab == null)
		{
			this.assetBundleAssetNameToPrefab = new Dictionary<string, GameObject>();
		}
		if (this.assetBundleAssetNameRefCounts == null)
		{
			this.assetBundleAssetNameRefCounts = new Dictionary<string, int>();
		}
	}

	// Token: 0x06005DBE RID: 23998 RVA: 0x00233740 File Offset: 0x00231B40
	public GameObject GetCachedPrefab(string assetBundleName, string assetName)
	{
		string key = assetBundleName + ":" + assetName;
		this.InitAssetBundleDictionaries();
		GameObject result = null;
		if (this.assetBundleAssetNameToPrefab.TryGetValue(assetBundleName + ":" + assetName, out result))
		{
			int num;
			if (this.assetBundleAssetNameRefCounts.TryGetValue(key, out num))
			{
				num++;
				this.assetBundleAssetNameRefCounts.Remove(key);
				this.assetBundleAssetNameRefCounts.Add(key, num);
				AssetBundleManager.RegisterAssetBundleAdditionalUse(assetBundleName);
			}
			else
			{
				UnityEngine.Debug.LogError("Asset bundle ref count dictionary corruption");
			}
		}
		return result;
	}

	// Token: 0x06005DBF RID: 23999 RVA: 0x002337C8 File Offset: 0x00231BC8
	public void RegisterPrefab(string assetBundleName, string assetName, GameObject prefab)
	{
		string key = assetBundleName + ":" + assetName;
		this.InitAssetBundleDictionaries();
		int num;
		if (this.assetBundleAssetNameRefCounts.TryGetValue(key, out num))
		{
			num++;
			this.assetBundleAssetNameRefCounts.Remove(key);
		}
		else
		{
			num = 1;
			this.assetBundleAssetNameToPrefab.Add(key, prefab);
		}
		this.assetBundleAssetNameRefCounts.Add(key, num);
	}

	// Token: 0x06005DC0 RID: 24000 RVA: 0x00233830 File Offset: 0x00231C30
	public void UnregisterPrefab(string assetBundleName, string assetName)
	{
		string text = assetBundleName + ":" + assetName;
		this.InitAssetBundleDictionaries();
		int num;
		if (this.assetBundleAssetNameRefCounts.TryGetValue(text, out num))
		{
			num--;
			this.assetBundleAssetNameRefCounts.Remove(text);
			if (num == 0)
			{
				this.assetBundleAssetNameToPrefab.Remove(text);
			}
			else
			{
				this.assetBundleAssetNameRefCounts.Add(text, num);
			}
			AssetBundleManager.UnloadAssetBundle(assetBundleName);
		}
		else
		{
			SuperController.LogError("Tried to UnregisterPrefab " + text + " that was not registered");
		}
	}

	// Token: 0x06005DC1 RID: 24001 RVA: 0x002338BC File Offset: 0x00231CBC
	protected void UnregisterAllPrefabsFromAtoms()
	{
		if (this.assetBundleAssetNameRefCounts != null)
		{
			foreach (SuperController.AtomAsset atomAsset in this.atomAssetByType.Values)
			{
				string key = atomAsset.assetBundleName + ":" + atomAsset.assetName;
				int num;
				if (this.assetBundleAssetNameRefCounts.TryGetValue(key, out num))
				{
					this.assetBundleAssetNameRefCounts.Remove(key);
					this.assetBundleAssetNameToPrefab.Remove(key);
					for (int i = 0; i < num; i++)
					{
						AssetBundleManager.UnloadAssetBundle(atomAsset.assetBundleName);
					}
				}
			}
		}
	}

	// Token: 0x06005DC2 RID: 24002 RVA: 0x00233988 File Offset: 0x00231D88
	protected IEnumerator LoadAtomFromBundleAsync(SuperController.AtomAsset aa, string useuid = null, bool userInvoked = false, bool forceSelect = false, bool forceFocus = false)
	{
		yield return SuperController.AssetManagerReady();
		float startTime = Time.realtimeSinceStartup;
		GameObject go = this.GetCachedPrefab(aa.assetBundleName, aa.assetName);
		if (go == null)
		{
			AssetBundleLoadAssetOperation request = AssetBundleManager.LoadAssetAsync(aa.assetBundleName, aa.assetName, typeof(GameObject));
			if (request == null)
			{
				this.Error("Failed to load Atom " + aa.assetName, true, true);
				yield break;
			}
			yield return base.StartCoroutine(request);
			go = request.GetAsset<GameObject>();
			if (go != null)
			{
				this.RegisterPrefab(aa.assetBundleName, aa.assetName, go);
			}
			else
			{
				this.Error("Asset " + aa.assetName + " is missing game object", true, true);
			}
		}
		if (go != null)
		{
			Atom component = go.GetComponent<Atom>();
			if (component != null)
			{
				startTime = Time.realtimeSinceStartup;
				Transform transform = this.AddAtom(component, useuid, userInvoked, forceSelect, forceFocus, true);
				if (transform != null)
				{
					Atom component2 = transform.GetComponent<Atom>();
					if (component2 != null)
					{
						component2.loadedFromBundle = true;
					}
				}
			}
			else
			{
				this.Error("Asset " + aa.assetName + " is missing Atom component", true, true);
			}
		}
		yield break;
	}

	// Token: 0x06005DC3 RID: 24003 RVA: 0x002339C8 File Offset: 0x00231DC8
	public void PauseSyncAtomLists()
	{
		this._pauseSyncAtomLists = true;
	}

	// Token: 0x06005DC4 RID: 24004 RVA: 0x002339D1 File Offset: 0x00231DD1
	public void ResumeSyncAtomLists()
	{
		this._pauseSyncAtomLists = false;
		this.SyncSortedAtomUIDs();
		this.SyncSortedAtomUIDsWithForceProducers();
		this.SyncSortedAtomUIDsWithForceReceivers();
		this.SyncSortedAtomUIDsWithFreeControllers();
		this.SyncSortedAtomUIDsWithRhythmControllers();
		this.SyncSortedAtomUIDsWithAudioSourceControls();
		this.SyncSortedAtomUIDsWithRigidbodies();
		this.SyncHiddenAtoms();
		this.SyncSelectAtomPopup();
	}

	// Token: 0x06005DC5 RID: 24005 RVA: 0x00233A10 File Offset: 0x00231E10
	private void SyncSortedAtomUIDs()
	{
		if (!this._isLoading)
		{
			this.sortedAtomUIDs.Sort();
		}
	}

	// Token: 0x06005DC6 RID: 24006 RVA: 0x00233A28 File Offset: 0x00231E28
	private void SyncSortedAtomUIDsWithForceReceivers()
	{
		if (!this._isLoading)
		{
			this.sortedAtomUIDsWithForceReceivers.Sort();
		}
	}

	// Token: 0x06005DC7 RID: 24007 RVA: 0x00233A40 File Offset: 0x00231E40
	private void SyncSortedAtomUIDsWithForceProducers()
	{
		if (!this._isLoading)
		{
			this.sortedAtomUIDsWithForceProducers.Sort();
		}
	}

	// Token: 0x06005DC8 RID: 24008 RVA: 0x00233A58 File Offset: 0x00231E58
	private void SyncSortedAtomUIDsWithRhythmControllers()
	{
		if (!this._isLoading)
		{
			this.sortedAtomUIDsWithRhythmControllers.Sort();
		}
	}

	// Token: 0x06005DC9 RID: 24009 RVA: 0x00233A70 File Offset: 0x00231E70
	private void SyncSortedAtomUIDsWithAudioSourceControls()
	{
		if (!this._isLoading)
		{
			this.sortedAtomUIDsWithAudioSourceControls.Sort();
		}
	}

	// Token: 0x06005DCA RID: 24010 RVA: 0x00233A88 File Offset: 0x00231E88
	private void SyncSortedAtomUIDsWithFreeControllers()
	{
		if (!this._isLoading)
		{
			this.sortedAtomUIDsWithFreeControllers.Sort();
		}
	}

	// Token: 0x06005DCB RID: 24011 RVA: 0x00233AA0 File Offset: 0x00231EA0
	private void SyncSortedAtomUIDsWithRigidbodies()
	{
		if (!this._isLoading)
		{
			this.sortedAtomUIDsWithRigidbodies.Sort();
		}
	}

	// Token: 0x06005DCC RID: 24012 RVA: 0x00233AB8 File Offset: 0x00231EB8
	public void SyncHiddenAtoms()
	{
		if (!this._isLoading)
		{
			if (this.hiddenAtomUIDs == null)
			{
				this.hiddenAtomUIDs = new List<string>();
			}
			else
			{
				this.hiddenAtomUIDs.Clear();
			}
			if (this.visibleAtomUIDs == null)
			{
				this.visibleAtomUIDs = new List<string>();
			}
			else
			{
				this.visibleAtomUIDs.Clear();
			}
			foreach (string text in this.sortedAtomUIDs)
			{
				Atom atomByUid = this.GetAtomByUid(text);
				if (atomByUid != null)
				{
					if (atomByUid.hidden || atomByUid.tempHidden)
					{
						this.hiddenAtomUIDs.Add(text);
					}
					else
					{
						this.visibleAtomUIDs.Add(text);
					}
				}
			}
			if (this.hiddenAtomUIDsWithFreeControllers == null)
			{
				this.hiddenAtomUIDsWithFreeControllers = new List<string>();
			}
			else
			{
				this.hiddenAtomUIDsWithFreeControllers.Clear();
			}
			if (this.visibleAtomUIDsWithFreeControllers == null)
			{
				this.visibleAtomUIDsWithFreeControllers = new List<string>();
			}
			else
			{
				this.visibleAtomUIDsWithFreeControllers.Clear();
			}
			foreach (string text2 in this.sortedAtomUIDsWithFreeControllers)
			{
				Atom atomByUid2 = this.GetAtomByUid(text2);
				if (atomByUid2 != null)
				{
					if (atomByUid2.hidden || atomByUid2.tempHidden)
					{
						this.hiddenAtomUIDsWithFreeControllers.Add(text2);
					}
					else
					{
						this.visibleAtomUIDsWithFreeControllers.Add(text2);
					}
				}
			}
			this.SyncSelectAtomPopup();
			this.SyncVisibility();
		}
	}

	// Token: 0x17000DC5 RID: 3525
	// (get) Token: 0x06005DCD RID: 24013 RVA: 0x00233C94 File Offset: 0x00232094
	// (set) Token: 0x06005DCE RID: 24014 RVA: 0x00233C9C File Offset: 0x0023209C
	public SubScene isolatedSubScene
	{
		get
		{
			return this._isolatedSubScene;
		}
		protected set
		{
			if (this._isolatedSubScene != value)
			{
				this._isolatedSubScene = value;
				if (this._isolatedSubScene != null)
				{
					if (this.quickReloadIsolatedSubSceneButton != null)
					{
						this.quickReloadIsolatedSubSceneButton.interactable = this._isolatedSubScene.CheckExistance();
					}
					if (this.quickSaveIsolatedSubSceneButton != null)
					{
						this.quickSaveIsolatedSubSceneButton.interactable = this._isolatedSubScene.CheckReadyForStore();
					}
				}
				if (this._isolatedSubScene != null && this.isolatedSubSceneLabel != null)
				{
					this.isolatedSubSceneLabel.text = this._isolatedSubScene.containingAtom.uidWithoutSubScenePath;
				}
				if (this.isolatedSceneEditControlPanel != null)
				{
					this.isolatedSceneEditControlPanel.gameObject.SetActive(this._isolatedSubScene != null);
				}
				this.SyncIsolatedSubScene();
			}
		}
	}

	// Token: 0x06005DCF RID: 24015 RVA: 0x00233D90 File Offset: 0x00232190
	protected void DetermineAtomsInAtom(Atom startingAtom, Atom atom, HashSet<Atom> atomsHash, HashSet<Atom> nestedSubSceneAtomsHash, bool isInNestedSubScene = false)
	{
		bool flag = isInNestedSubScene;
		if (!flag && atom != startingAtom && atom.isSubSceneType)
		{
			flag = true;
		}
		foreach (Atom atom2 in atom.GetChildren())
		{
			this.DetermineAtomsInAtom(startingAtom, atom2, atomsHash, nestedSubSceneAtomsHash, flag);
		}
		if (isInNestedSubScene)
		{
			this.nestedAtomsInSubSceneHash.Add(atom);
		}
		else
		{
			atomsHash.Add(atom);
		}
	}

	// Token: 0x06005DD0 RID: 24016 RVA: 0x00233E34 File Offset: 0x00232234
	protected void SyncIsolatedSubScene()
	{
		bool flag = false;
		if (this._isolatedSubScene != null)
		{
			if (this.atomsInSubSceneHash == null)
			{
				this.atomsInSubSceneHash = new HashSet<Atom>();
			}
			else
			{
				this.atomsInSubSceneHash.Clear();
			}
			if (this.nestedAtomsInSubSceneHash == null)
			{
				this.nestedAtomsInSubSceneHash = new HashSet<Atom>();
			}
			else
			{
				this.nestedAtomsInSubSceneHash.Clear();
			}
			this.DetermineAtomsInAtom(this._isolatedSubScene.containingAtom, this._isolatedSubScene.containingAtom, this.atomsInSubSceneHash, this.nestedAtomsInSubSceneHash, false);
			foreach (Atom atom in this.atomsList)
			{
				if (this.atomsInSubSceneHash.Contains(atom))
				{
					if (atom.tempHidden)
					{
						atom.tempHidden = false;
						flag = true;
					}
					atom.tempFreezePhysics = false;
					atom.tempDisableCollision = false;
					atom.tempDisableRender = false;
				}
				else if (this.nestedAtomsInSubSceneHash.Contains(atom))
				{
					if (!atom.tempHidden)
					{
						atom.tempHidden = true;
						flag = true;
					}
					atom.tempFreezePhysics = false;
					atom.tempDisableCollision = false;
					atom.tempDisableRender = false;
				}
				else
				{
					if (!atom.tempHidden)
					{
						atom.tempHidden = true;
						flag = true;
					}
					atom.tempFreezePhysics = this._freezePhysicsForAtomsNotInIsolatedSubScene;
					atom.tempDisableCollision = this._disableCollisionForAtomsNotInIsolatedSubScene;
					atom.tempDisableRender = this._disableRenderForAtomsNotInIsolatedSubScene;
				}
			}
		}
		else
		{
			foreach (Atom atom2 in this.atomsList)
			{
				if (atom2.tempHidden)
				{
					atom2.tempHidden = false;
					flag = true;
				}
				atom2.tempFreezePhysics = false;
				atom2.tempDisableCollision = false;
				atom2.tempDisableRender = false;
			}
		}
		if (flag)
		{
			this.SyncHiddenAtoms();
		}
	}

	// Token: 0x17000DC6 RID: 3526
	// (get) Token: 0x06005DD1 RID: 24017 RVA: 0x00234048 File Offset: 0x00232448
	// (set) Token: 0x06005DD2 RID: 24018 RVA: 0x00234050 File Offset: 0x00232450
	public bool disableRenderForAtomsNotInIsolatedSubScene
	{
		get
		{
			return this._disableRenderForAtomsNotInIsolatedSubScene;
		}
		set
		{
			if (this._disableRenderForAtomsNotInIsolatedSubScene != value)
			{
				this._disableRenderForAtomsNotInIsolatedSubScene = value;
				if (this.disableRenderForAtomsNotInIsolatedSubSceneToggle != null)
				{
					this.disableRenderForAtomsNotInIsolatedSubSceneToggle.isOn = value;
				}
				this.SyncIsolatedSubScene();
			}
		}
	}

	// Token: 0x17000DC7 RID: 3527
	// (get) Token: 0x06005DD3 RID: 24019 RVA: 0x00234088 File Offset: 0x00232488
	// (set) Token: 0x06005DD4 RID: 24020 RVA: 0x00234090 File Offset: 0x00232490
	public bool freezePhysicsForAtomsNotInIsolatedSubScene
	{
		get
		{
			return this._freezePhysicsForAtomsNotInIsolatedSubScene;
		}
		set
		{
			if (this._freezePhysicsForAtomsNotInIsolatedSubScene != value)
			{
				this._freezePhysicsForAtomsNotInIsolatedSubScene = value;
				if (this.freezePhysicsForAtomsNotInIsolatedSubSceneToggle != null)
				{
					this.freezePhysicsForAtomsNotInIsolatedSubSceneToggle.isOn = this._freezePhysicsForAtomsNotInIsolatedSubScene;
				}
				if (!this._freezePhysicsForAtomsNotInIsolatedSubScene && this._disableCollisionForAtomsNotInIsolatedSubScene)
				{
					this.disableCollisionForAtomsNotInIsolatedSubScene = false;
				}
				else
				{
					this.SyncIsolatedSubScene();
				}
			}
		}
	}

	// Token: 0x17000DC8 RID: 3528
	// (get) Token: 0x06005DD5 RID: 24021 RVA: 0x002340FA File Offset: 0x002324FA
	// (set) Token: 0x06005DD6 RID: 24022 RVA: 0x00234104 File Offset: 0x00232504
	public bool disableCollisionForAtomsNotInIsolatedSubScene
	{
		get
		{
			return this._disableCollisionForAtomsNotInIsolatedSubScene;
		}
		set
		{
			if (this._disableCollisionForAtomsNotInIsolatedSubScene != value)
			{
				this._disableCollisionForAtomsNotInIsolatedSubScene = value;
				if (this.disableCollisionForAtomsNotInIsolatedSubSceneToggle != null)
				{
					this.disableCollisionForAtomsNotInIsolatedSubSceneToggle.isOn = this._disableCollisionForAtomsNotInIsolatedSubScene;
				}
				if (this._disableCollisionForAtomsNotInIsolatedSubScene && !this._freezePhysicsForAtomsNotInIsolatedSubScene)
				{
					this.freezePhysicsForAtomsNotInIsolatedSubScene = true;
				}
				else
				{
					this.SyncIsolatedSubScene();
				}
			}
		}
	}

	// Token: 0x06005DD7 RID: 24023 RVA: 0x0023416E File Offset: 0x0023256E
	public void StartIsolateEditSubScene(SubScene subScene)
	{
		this.EndIsolateEditAtom();
		this.EndIsolateEditSubScene();
		this.isolatedSubScene = subScene;
		this.isolatedSubScene.isIsolateEditing = true;
	}

	// Token: 0x06005DD8 RID: 24024 RVA: 0x0023418F File Offset: 0x0023258F
	public void EndIsolateEditSubScene()
	{
		if (this.isolatedSubScene != null)
		{
			this.isolatedSubScene.isIsolateEditing = false;
		}
		this.isolatedSubScene = null;
	}

	// Token: 0x06005DD9 RID: 24025 RVA: 0x002341B5 File Offset: 0x002325B5
	public void QuickSaveIsolatedSubScene()
	{
		if (this._isolatedSubScene != null)
		{
			this._isolatedSubScene.StoreSubScene();
		}
	}

	// Token: 0x06005DDA RID: 24026 RVA: 0x002341D3 File Offset: 0x002325D3
	public void QuickReloadIsolatedSubScene()
	{
		if (this.isolatedSubScene != null)
		{
			this._isolatedSubScene.LoadSubScene();
		}
	}

	// Token: 0x06005DDB RID: 24027 RVA: 0x002341F1 File Offset: 0x002325F1
	public void SelectIsolatedSubScene()
	{
		if (this._isolatedSubScene != null)
		{
			this.SelectController(this._isolatedSubScene.containingAtom.mainController, false, true, true, true);
		}
	}

	// Token: 0x17000DC9 RID: 3529
	// (get) Token: 0x06005DDC RID: 24028 RVA: 0x0023421E File Offset: 0x0023261E
	// (set) Token: 0x06005DDD RID: 24029 RVA: 0x00234228 File Offset: 0x00232628
	public Atom isolatedAtom
	{
		get
		{
			return this._isolatedAtom;
		}
		protected set
		{
			if (this._isolatedAtom != value)
			{
				this._isolatedAtom = value;
				if (this._isolatedAtom != null && this.isolatedAtomLabel != null)
				{
					this.isolatedAtomLabel.text = this._isolatedAtom.uid;
				}
				if (this.isolatedAtomEditControlPanel != null)
				{
					this.isolatedAtomEditControlPanel.gameObject.SetActive(this._isolatedAtom != null);
				}
				this.SyncIsolatedAtom();
			}
		}
	}

	// Token: 0x06005DDE RID: 24030 RVA: 0x002342B8 File Offset: 0x002326B8
	protected void SyncIsolatedAtom()
	{
		bool flag = false;
		if (this._isolatedAtom != null)
		{
			foreach (Atom atom in this.atomsList)
			{
				if (atom == this._isolatedAtom)
				{
					if (atom.tempHidden)
					{
						atom.tempHidden = false;
						flag = true;
					}
					atom.tempFreezePhysics = false;
					atom.tempDisableCollision = false;
					atom.tempDisableRender = false;
				}
				else
				{
					if (!atom.tempHidden)
					{
						atom.tempHidden = true;
						flag = true;
					}
					atom.tempFreezePhysics = this._freezePhysicsForAtomsNotInIsolatedAtom;
					atom.tempDisableCollision = this._disableCollisionForAtomsNotInIsolatedAtom;
					atom.tempDisableRender = this._disableRenderForAtomsNotInIsolatedAtom;
				}
			}
		}
		else
		{
			foreach (Atom atom2 in this.atomsList)
			{
				if (atom2.tempHidden)
				{
					atom2.tempHidden = false;
					flag = true;
				}
				atom2.tempFreezePhysics = false;
				atom2.tempDisableCollision = false;
				atom2.tempDisableRender = false;
			}
		}
		if (flag)
		{
			this.SyncHiddenAtoms();
		}
	}

	// Token: 0x17000DCA RID: 3530
	// (get) Token: 0x06005DDF RID: 24031 RVA: 0x00234418 File Offset: 0x00232818
	// (set) Token: 0x06005DE0 RID: 24032 RVA: 0x00234420 File Offset: 0x00232820
	public bool disableRenderForAtomsNotInIsolatedAtom
	{
		get
		{
			return this._disableRenderForAtomsNotInIsolatedAtom;
		}
		set
		{
			if (this._disableRenderForAtomsNotInIsolatedAtom != value)
			{
				this._disableRenderForAtomsNotInIsolatedAtom = value;
				if (this.disableRenderForAtomsNotInIsolatedAtomToggle != null)
				{
					this.disableRenderForAtomsNotInIsolatedAtomToggle.isOn = value;
				}
				this.SyncIsolatedAtom();
			}
		}
	}

	// Token: 0x17000DCB RID: 3531
	// (get) Token: 0x06005DE1 RID: 24033 RVA: 0x00234458 File Offset: 0x00232858
	// (set) Token: 0x06005DE2 RID: 24034 RVA: 0x00234460 File Offset: 0x00232860
	public bool freezePhysicsForAtomsNotInIsolatedAtom
	{
		get
		{
			return this._freezePhysicsForAtomsNotInIsolatedAtom;
		}
		set
		{
			if (this._freezePhysicsForAtomsNotInIsolatedAtom != value)
			{
				this._freezePhysicsForAtomsNotInIsolatedAtom = value;
				if (this.freezePhysicsForAtomsNotInIsolatedAtomToggle != null)
				{
					this.freezePhysicsForAtomsNotInIsolatedAtomToggle.isOn = this._freezePhysicsForAtomsNotInIsolatedAtom;
				}
				if (!this._freezePhysicsForAtomsNotInIsolatedAtom && this._disableCollisionForAtomsNotInIsolatedAtom)
				{
					this.disableCollisionForAtomsNotInIsolatedAtom = false;
				}
				else
				{
					this.SyncIsolatedAtom();
				}
			}
		}
	}

	// Token: 0x17000DCC RID: 3532
	// (get) Token: 0x06005DE3 RID: 24035 RVA: 0x002344CA File Offset: 0x002328CA
	// (set) Token: 0x06005DE4 RID: 24036 RVA: 0x002344D4 File Offset: 0x002328D4
	public bool disableCollisionForAtomsNotInIsolatedAtom
	{
		get
		{
			return this._disableCollisionForAtomsNotInIsolatedAtom;
		}
		set
		{
			if (this._disableCollisionForAtomsNotInIsolatedAtom != value)
			{
				this._disableCollisionForAtomsNotInIsolatedAtom = value;
				if (this.disableCollisionForAtomsNotInIsolatedAtomToggle != null)
				{
					this.disableCollisionForAtomsNotInIsolatedAtomToggle.isOn = this._disableCollisionForAtomsNotInIsolatedAtom;
				}
				if (this._disableCollisionForAtomsNotInIsolatedAtom && !this._freezePhysicsForAtomsNotInIsolatedAtom)
				{
					this.freezePhysicsForAtomsNotInIsolatedAtom = true;
				}
				else
				{
					this.SyncIsolatedAtom();
				}
			}
		}
	}

	// Token: 0x06005DE5 RID: 24037 RVA: 0x0023453E File Offset: 0x0023293E
	public void StartIsolateEditAtom(Atom atom)
	{
		this.EndIsolateEditAtom();
		this.EndIsolateEditSubScene();
		this.isolatedAtom = atom;
	}

	// Token: 0x06005DE6 RID: 24038 RVA: 0x00234553 File Offset: 0x00232953
	public void EndIsolateEditAtom()
	{
		this.isolatedAtom = null;
	}

	// Token: 0x06005DE7 RID: 24039 RVA: 0x0023455C File Offset: 0x0023295C
	public void SelectIsolatedAtom()
	{
		if (this._isolatedAtom != null)
		{
			this.SelectController(this._isolatedAtom.mainController, false, true, true, true);
		}
	}

	// Token: 0x17000DCD RID: 3533
	// (get) Token: 0x06005DE8 RID: 24040 RVA: 0x00234584 File Offset: 0x00232984
	// (set) Token: 0x06005DE9 RID: 24041 RVA: 0x0023458C File Offset: 0x0023298C
	public bool showHiddenAtoms
	{
		get
		{
			return this._showHiddenAtoms;
		}
		set
		{
			if (this._showHiddenAtoms != value)
			{
				this._showHiddenAtoms = value;
				if (this.showHiddenAtomsToggle != null)
				{
					this.showHiddenAtomsToggle.isOn = value;
				}
				if (this.showHiddenAtomsToggleAlt != null)
				{
					this.showHiddenAtomsToggleAlt.isOn = value;
				}
				this.SyncSelectAtomPopup();
				this.SyncVisibility();
			}
		}
	}

	// Token: 0x06005DEA RID: 24042 RVA: 0x002345F2 File Offset: 0x002329F2
	public void ToggleShowHiddenAtoms()
	{
		this.showHiddenAtoms = !this._showHiddenAtoms;
	}

	// Token: 0x06005DEB RID: 24043 RVA: 0x00234603 File Offset: 0x00232A03
	public List<FreeControllerV3> GetAllFreeControllers()
	{
		return this.allControllers;
	}

	// Token: 0x06005DEC RID: 24044 RVA: 0x0023460B File Offset: 0x00232A0B
	public List<AnimationPattern> GetAllAnimationPatterns()
	{
		return this.allAnimationPatterns;
	}

	// Token: 0x06005DED RID: 24045 RVA: 0x00234613 File Offset: 0x00232A13
	public List<AnimationStep> GetAllAnimationSteps()
	{
		return this.allAnimationSteps;
	}

	// Token: 0x06005DEE RID: 24046 RVA: 0x0023461C File Offset: 0x00232A1C
	private string CreateUID(string name)
	{
		if (!this.uids.ContainsKey(name))
		{
			this.uids.Add(name, true);
			return name;
		}
		for (int i = 2; i < this.maxUID; i++)
		{
			string text = name + "#" + i.ToString();
			if (!this.uids.ContainsKey(text))
			{
				this.uids.Add(text, true);
				return text;
			}
		}
		this.Error(string.Concat(new object[]
		{
			"Exceeded UID limit of ",
			this.maxUID,
			" for ",
			name
		}), true, true);
		return null;
	}

	// Token: 0x06005DEF RID: 24047 RVA: 0x002346D0 File Offset: 0x00232AD0
	public string GetTempUID()
	{
		string text = Guid.NewGuid().ToString();
		while (this.uids.ContainsKey(text))
		{
			text = Guid.NewGuid().ToString();
		}
		this.uids.Add(text, true);
		return text;
	}

	// Token: 0x06005DF0 RID: 24048 RVA: 0x00234729 File Offset: 0x00232B29
	public void ReleaseTempUID(string uid)
	{
		this.uids.Remove(uid);
	}

	// Token: 0x06005DF1 RID: 24049 RVA: 0x00234738 File Offset: 0x00232B38
	private void SyncForceReceiverNames()
	{
		this._forceReceiverNames = new string[this.frMap.Keys.Count];
		this.frMap.Keys.CopyTo(this._forceReceiverNames, 0);
		if (this.onForceReceiverNamesChangedHandlers != null)
		{
			this.onForceReceiverNamesChangedHandlers(this._forceReceiverNames);
		}
	}

	// Token: 0x17000DCE RID: 3534
	// (get) Token: 0x06005DF2 RID: 24050 RVA: 0x00234793 File Offset: 0x00232B93
	public string[] forceReceiverNames
	{
		get
		{
			return this._forceReceiverNames;
		}
	}

	// Token: 0x06005DF3 RID: 24051 RVA: 0x0023479C File Offset: 0x00232B9C
	public List<string> GetForceReceiverNamesInAtom(string atomUID)
	{
		List<string> list = new List<string>();
		Atom atom;
		if (atomUID != null && this.atoms.TryGetValue(atomUID, out atom))
		{
			foreach (ForceReceiver forceReceiver in atom.forceReceivers)
			{
				list.Add(forceReceiver.name);
			}
		}
		return list;
	}

	// Token: 0x06005DF4 RID: 24052 RVA: 0x002347FC File Offset: 0x00232BFC
	public ForceReceiver ReceiverNameToForceReceiver(string receiverName)
	{
		ForceReceiver result;
		if (this.frMap != null && receiverName != null && this.frMap.TryGetValue(receiverName, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06005DF5 RID: 24053 RVA: 0x00234830 File Offset: 0x00232C30
	private void SyncForceProducerNames()
	{
		this._forceProducerNames = new string[this.fpMap.Keys.Count];
		this.fpMap.Keys.CopyTo(this._forceProducerNames, 0);
		if (this.onForceProducerNamesChangedHandlers != null)
		{
			this.onForceProducerNamesChangedHandlers(this._forceProducerNames);
		}
	}

	// Token: 0x17000DCF RID: 3535
	// (get) Token: 0x06005DF6 RID: 24054 RVA: 0x0023488B File Offset: 0x00232C8B
	public string[] forceProducerNames
	{
		get
		{
			return this._forceProducerNames;
		}
	}

	// Token: 0x06005DF7 RID: 24055 RVA: 0x00234894 File Offset: 0x00232C94
	public List<string> GetForceProducerNamesInAtom(string atomUID)
	{
		List<string> list = new List<string>();
		Atom atom;
		if (atomUID != null && this.atoms.TryGetValue(atomUID, out atom))
		{
			foreach (ForceProducerV2 forceProducerV in atom.forceProducers)
			{
				list.Add(forceProducerV.name);
			}
		}
		return list;
	}

	// Token: 0x06005DF8 RID: 24056 RVA: 0x002348F4 File Offset: 0x00232CF4
	public ForceProducerV2 ProducerNameToForceProducer(string producerName)
	{
		ForceProducerV2 result;
		if (this.fpMap != null && producerName != null && this.fpMap.TryGetValue(producerName, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06005DF9 RID: 24057 RVA: 0x00234928 File Offset: 0x00232D28
	private void SyncRhythmControllerNames()
	{
		this._rhythmControllerNames = new string[this.rcMap.Keys.Count];
		this.rcMap.Keys.CopyTo(this._rhythmControllerNames, 0);
		if (this.onRhythmControllerNamesChangedHandlers != null)
		{
			this.onRhythmControllerNamesChangedHandlers(this._rhythmControllerNames);
		}
	}

	// Token: 0x17000DD0 RID: 3536
	// (get) Token: 0x06005DFA RID: 24058 RVA: 0x00234983 File Offset: 0x00232D83
	public string[] rhythmControllerNames
	{
		get
		{
			return this._rhythmControllerNames;
		}
	}

	// Token: 0x06005DFB RID: 24059 RVA: 0x0023498C File Offset: 0x00232D8C
	public List<string> GetRhythmControllerNamesInAtom(string atomUID)
	{
		List<string> list = new List<string>();
		Atom atom;
		if (atomUID != null && this.atoms.TryGetValue(atomUID, out atom))
		{
			foreach (RhythmController rhythmController in atom.rhythmControllers)
			{
				list.Add(rhythmController.name);
			}
		}
		return list;
	}

	// Token: 0x06005DFC RID: 24060 RVA: 0x002349EC File Offset: 0x00232DEC
	public RhythmController RhythmControllerrNameToRhythmController(string controllerName)
	{
		RhythmController result;
		if (this.rcMap != null && controllerName != null && this.rcMap.TryGetValue(controllerName, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06005DFD RID: 24061 RVA: 0x00234A20 File Offset: 0x00232E20
	private void SyncAudioSourceControlNames()
	{
		this._audioSourceControlNames = new string[this.ascMap.Keys.Count];
		this.ascMap.Keys.CopyTo(this._audioSourceControlNames, 0);
		if (this.onAudioSourceControlNamesChangedHandlers != null)
		{
			this.onAudioSourceControlNamesChangedHandlers(this._audioSourceControlNames);
		}
	}

	// Token: 0x17000DD1 RID: 3537
	// (get) Token: 0x06005DFE RID: 24062 RVA: 0x00234A7B File Offset: 0x00232E7B
	public string[] audioSourceControlNames
	{
		get
		{
			return this._audioSourceControlNames;
		}
	}

	// Token: 0x06005DFF RID: 24063 RVA: 0x00234A84 File Offset: 0x00232E84
	public List<string> GetAudioSourceControlNamesInAtom(string atomUID)
	{
		List<string> list = new List<string>();
		Atom atom;
		if (atomUID != null && this.atoms.TryGetValue(atomUID, out atom))
		{
			foreach (AudioSourceControl audioSourceControl in atom.audioSourceControls)
			{
				list.Add(audioSourceControl.name);
			}
		}
		return list;
	}

	// Token: 0x06005E00 RID: 24064 RVA: 0x00234AE4 File Offset: 0x00232EE4
	public AudioSourceControl AudioSourceControlrNameToAudioSourceControl(string controllerName)
	{
		AudioSourceControl result;
		if (this.ascMap != null && controllerName != null && this.ascMap.TryGetValue(controllerName, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06005E01 RID: 24065 RVA: 0x00234B18 File Offset: 0x00232F18
	private void SyncFreeControllerNames()
	{
		this._freeControllerNames = new string[this.fcMap.Keys.Count];
		this.fcMap.Keys.CopyTo(this._freeControllerNames, 0);
		if (this.onFreeControllerNamesChangedHandlers != null)
		{
			this.onFreeControllerNamesChangedHandlers(this._freeControllerNames);
		}
	}

	// Token: 0x17000DD2 RID: 3538
	// (get) Token: 0x06005E02 RID: 24066 RVA: 0x00234B73 File Offset: 0x00232F73
	public string[] freeControllerNames
	{
		get
		{
			return this._freeControllerNames;
		}
	}

	// Token: 0x06005E03 RID: 24067 RVA: 0x00234B7C File Offset: 0x00232F7C
	public List<string> GetFreeControllerNamesInAtom(string atomUID)
	{
		List<string> list = new List<string>();
		Atom atom;
		if (atomUID != null && this.atoms.TryGetValue(atomUID, out atom))
		{
			foreach (FreeControllerV3 freeControllerV in atom.freeControllers)
			{
				list.Add(freeControllerV.name);
			}
		}
		return list;
	}

	// Token: 0x06005E04 RID: 24068 RVA: 0x00234BDC File Offset: 0x00232FDC
	public FreeControllerV3 FreeControllerNameToFreeController(string controllerName)
	{
		FreeControllerV3 result;
		if (this.fcMap != null && controllerName != null && this.fcMap.TryGetValue(controllerName, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06005E05 RID: 24069 RVA: 0x00234C10 File Offset: 0x00233010
	private void SyncRigidbodyNames()
	{
		this._rigidbodyNames = new string[this.rbMap.Keys.Count];
		this.rbMap.Keys.CopyTo(this._rigidbodyNames, 0);
		if (this.onRigidbodyNamesChangedHandlers != null)
		{
			this.onRigidbodyNamesChangedHandlers(this._rigidbodyNames);
		}
	}

	// Token: 0x17000DD3 RID: 3539
	// (get) Token: 0x06005E06 RID: 24070 RVA: 0x00234C6B File Offset: 0x0023306B
	public string[] rigidbodyNames
	{
		get
		{
			return this._rigidbodyNames;
		}
	}

	// Token: 0x06005E07 RID: 24071 RVA: 0x00234C74 File Offset: 0x00233074
	public List<string> GetRigidbodyNamesInAtom(string atomUID)
	{
		List<string> list = new List<string>();
		Atom atom;
		if (atomUID != null && this.atoms.TryGetValue(atomUID, out atom))
		{
			foreach (Rigidbody rigidbody in atom.linkableRigidbodies)
			{
				list.Add(rigidbody.name);
			}
		}
		return list;
	}

	// Token: 0x06005E08 RID: 24072 RVA: 0x00234CD4 File Offset: 0x002330D4
	public Rigidbody RigidbodyNameToRigidbody(string rigidbodyName)
	{
		Rigidbody result;
		if (this.rbMap != null && rigidbodyName != null && this.rbMap.TryGetValue(rigidbodyName, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06005E09 RID: 24073 RVA: 0x00234D08 File Offset: 0x00233108
	public List<string> GetAtomCategories()
	{
		return this.atomCategories;
	}

	// Token: 0x06005E0A RID: 24074 RVA: 0x00234D10 File Offset: 0x00233110
	protected virtual void SetAddAtomAtomPopupValues(string category)
	{
		if (this.atomPrefabPopup != null && category != null)
		{
			List<string> list;
			if (this.atomCategoryToAtomTypes != null && this.atomCategoryToAtomTypes.TryGetValue(category, out list))
			{
				int num = 0;
				this.atomPrefabPopup.numPopupValues = list.Count;
				foreach (string text in list)
				{
					this.atomPrefabPopup.setPopupValue(num, text);
					num++;
				}
				this.atomPrefabPopup.currentValue = "None";
			}
			else
			{
				this.atomPrefabPopup.numPopupValues = 0;
			}
		}
	}

	// Token: 0x06005E0B RID: 24075 RVA: 0x00234DDC File Offset: 0x002331DC
	public List<Atom> GetAtoms()
	{
		return new List<Atom>(this.atomsList);
	}

	// Token: 0x06005E0C RID: 24076 RVA: 0x00234DE9 File Offset: 0x002331E9
	public List<string> GetAtomUIDs()
	{
		if (this.sortAtomUIDs)
		{
			return this.sortedAtomUIDs;
		}
		return this.atomUIDs;
	}

	// Token: 0x06005E0D RID: 24077 RVA: 0x00234E03 File Offset: 0x00233203
	public List<string> GetVisibleAtomUIDs()
	{
		if (this.showHiddenAtoms)
		{
			return this.sortedAtomUIDs;
		}
		if (this.sortAtomUIDs)
		{
			return this.visibleAtomUIDs;
		}
		return this.atomUIDs;
	}

	// Token: 0x06005E0E RID: 24078 RVA: 0x00234E2F File Offset: 0x0023322F
	public List<string> GetAtomUIDsWithForceReceivers()
	{
		if (this.sortAtomUIDs)
		{
			return this.sortedAtomUIDsWithForceReceivers;
		}
		return this.atomUIDsWithForceReceivers;
	}

	// Token: 0x06005E0F RID: 24079 RVA: 0x00234E49 File Offset: 0x00233249
	public List<string> GetAtomUIDsWithForceProducers()
	{
		if (this.sortAtomUIDs)
		{
			return this.sortedAtomUIDsWithForceProducers;
		}
		return this.atomUIDsWithForceProducers;
	}

	// Token: 0x06005E10 RID: 24080 RVA: 0x00234E63 File Offset: 0x00233263
	public List<string> GetAtomUIDsWithRhythmControllers()
	{
		if (this.sortAtomUIDs)
		{
			return this.sortedAtomUIDsWithRhythmControllers;
		}
		return this.atomUIDsWithRhythmControllers;
	}

	// Token: 0x06005E11 RID: 24081 RVA: 0x00234E7D File Offset: 0x0023327D
	public List<string> GetAtomUIDsWithAudioSourceControls()
	{
		if (this.sortAtomUIDs)
		{
			return this.sortedAtomUIDsWithAudioSourceControls;
		}
		return this.atomUIDsWithAudioSourceControls;
	}

	// Token: 0x06005E12 RID: 24082 RVA: 0x00234E97 File Offset: 0x00233297
	public List<string> GetAtomUIDsWithFreeControllers()
	{
		if (this.showHiddenAtoms)
		{
			return this.sortedAtomUIDsWithFreeControllers;
		}
		if (this.sortAtomUIDs)
		{
			return this.visibleAtomUIDsWithFreeControllers;
		}
		return this.atomUIDsWithFreeControllers;
	}

	// Token: 0x06005E13 RID: 24083 RVA: 0x00234EC3 File Offset: 0x002332C3
	public List<string> GetAtomUIDsWithRigidbodies()
	{
		if (this.sortAtomUIDs)
		{
			return this.sortedAtomUIDsWithRigidbodies;
		}
		return this.atomUIDsWithRigidbodies;
	}

	// Token: 0x06005E14 RID: 24084 RVA: 0x00234EE0 File Offset: 0x002332E0
	public Atom GetAtomByUid(string uid)
	{
		Atom result = null;
		if (uid != null)
		{
			this.atoms.TryGetValue(uid, out result);
		}
		return result;
	}

	// Token: 0x06005E15 RID: 24085 RVA: 0x00234F05 File Offset: 0x00233305
	public void NotifySubSceneLoad(SubScene subScene)
	{
		if (this.onSubSceneLoadedHandlers != null)
		{
			this.onSubSceneLoadedHandlers(subScene);
		}
	}

	// Token: 0x06005E16 RID: 24086 RVA: 0x00234F20 File Offset: 0x00233320
	public void AtomParentChanged(Atom atom, Atom newParent)
	{
		if (this.onAtomParentChangedHandlers != null)
		{
			this.onAtomParentChangedHandlers(atom, newParent);
		}
		if (this.motionAnimationMaster != null)
		{
			foreach (MotionAnimationControl motionAnimationControl in this.macMap.Values)
			{
				if (motionAnimationControl.animationMaster == null)
				{
					this.motionAnimationMaster.RegisterAnimationControl(motionAnimationControl);
				}
			}
		}
	}

	// Token: 0x06005E17 RID: 24087 RVA: 0x00234FC0 File Offset: 0x002333C0
	public void AtomSubSceneChanged(Atom atom, SubScene newSubScene)
	{
		if (this.onAtomParentChangedHandlers != null)
		{
			this.onAtomSubSceneChangedHandlers(atom, newSubScene);
		}
	}

	// Token: 0x06005E18 RID: 24088 RVA: 0x00234FDC File Offset: 0x002333DC
	public void AddAtomByPopupValue()
	{
		if (this.atomPrefabPopup != null && this.atomPrefabPopup.currentValue != "None")
		{
			base.StartCoroutine(this.AddAtomByType(this.atomPrefabPopup.currentValue, null, true, false, false));
		}
	}

	// Token: 0x06005E19 RID: 24089 RVA: 0x00235030 File Offset: 0x00233430
	public void OpenAddAtomUIToAtomType(string type)
	{
		bool flag = false;
		foreach (Atom atom in this.atomPrefabs)
		{
			if (type == atom.type)
			{
				this.atomCategoryPopup.currentValue = atom.category;
				this.atomPrefabPopup.currentValue = type;
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			foreach (SuperController.AtomAsset atomAsset in this.atomAssets)
			{
				if (type == atomAsset.assetName)
				{
					this.atomCategoryPopup.currentValue = atomAsset.category;
					this.atomPrefabPopup.currentValue = type;
					break;
				}
			}
		}
		this.activeUI = SuperController.ActiveUI.MainMenu;
		this.SetMainMenuTab("TabAddAtom");
	}

	// Token: 0x06005E1A RID: 24090 RVA: 0x00235108 File Offset: 0x00233508
	public void AddAtomByTypeForceSelect(string type)
	{
		this.AddAtomByType(type, true, true, false);
	}

	// Token: 0x06005E1B RID: 24091 RVA: 0x00235114 File Offset: 0x00233514
	public void AddAtomByType(string type, bool userInvoked = false, bool forceSelect = false, bool forceFocus = false)
	{
		base.StartCoroutine(this.AddAtomByType(type, null, userInvoked, forceSelect, forceFocus));
	}

	// Token: 0x06005E1C RID: 24092 RVA: 0x0023512C File Offset: 0x0023352C
	public IEnumerator AddAtomByType(string type, string useuid = null, bool userInvoked = false, bool forceSelect = false, bool forceFocus = false)
	{
		AsyncFlag loadIconFlag = new AsyncFlag("Load Atom " + type);
		this.SetLoadingIconFlag(loadIconFlag);
		if (type != null && type != string.Empty)
		{
			this.lastAddedAtom = null;
			Atom atom;
			SuperController.AtomAsset aa;
			if (atom = this.GetAtomOfTypeFromPool(type))
			{
				this.AddAtom(atom, useuid, userInvoked, forceSelect, forceFocus, false);
				if (userInvoked)
				{
					this.ResetSimulation(5, "AddAtom", true);
				}
			}
			else if (this.atomAssetByType.TryGetValue(type, out aa))
			{
				yield return base.StartCoroutine(this.LoadAtomFromBundleAsync(aa, useuid, userInvoked, forceSelect, false));
			}
			else if (this.atomPrefabByType.TryGetValue(type, out atom))
			{
				this.AddAtom(atom, useuid, userInvoked, forceSelect, forceFocus, true);
			}
			else
			{
				this.Error("Atom type " + type + " does not exist. Cannot add", true, true);
			}
			if (userInvoked && this.lastAddedAtom != null && this.lastAddedAtom.mainPresetControl != null)
			{
				yield return null;
				this.lastAddedAtom.mainPresetControl.LoadUserDefaults();
			}
		}
		loadIconFlag.Raise();
		yield break;
	}

	// Token: 0x06005E1D RID: 24093 RVA: 0x0023516C File Offset: 0x0023356C
	public Transform AddAtom(Atom atom, string useuid = null, bool userInvoked = false, bool forceSelect = false, bool forceFocus = false, bool instantiate = true)
	{
		string text;
		if (useuid != null)
		{
			text = this.CreateUID(useuid);
		}
		else
		{
			text = this.CreateUID(atom.name);
		}
		if (text != null)
		{
			Transform transform;
			if (instantiate)
			{
				transform = UnityEngine.Object.Instantiate<Transform>(atom.transform);
			}
			else
			{
				atom.destroyed = false;
				transform = atom.transform;
			}
			transform.SetParent(this.atomContainerTransform, true);
			Atom component = transform.GetComponent<Atom>();
			component.uid = text;
			component.name = text;
			this.InitAtom(component);
			if (userInvoked)
			{
				SubAtom[] componentsInChildren = atom.GetComponentsInChildren<SubAtom>();
				foreach (SubAtom subAtom in componentsInChildren)
				{
					Atom atomPrefab = subAtom.atomPrefab;
					if (atomPrefab != null)
					{
						Transform transform2 = this.AddAtom(atomPrefab, null, false, false, false, true);
						if (transform2 != null)
						{
							transform2.position = subAtom.transform.position;
							transform2.rotation = subAtom.transform.rotation;
						}
					}
				}
			}
			if (this.onAtomAddedHandlers != null)
			{
				this.onAtomAddedHandlers(component);
			}
			if (this.onAtomUIDsChangedHandlers != null)
			{
				this.onAtomUIDsChangedHandlers(this.GetAtomUIDs());
			}
			if (this.onAtomUIDsWithForceReceiversChangedHandlers != null && component.forceReceivers.Length > 0)
			{
				this.onAtomUIDsWithForceReceiversChangedHandlers(this.GetAtomUIDsWithForceReceivers());
			}
			if (this.onAtomUIDsWithForceProducersChangedHandlers != null && component.forceProducers.Length > 0)
			{
				this.onAtomUIDsWithForceProducersChangedHandlers(this.GetAtomUIDsWithForceProducers());
			}
			if (this.onAtomUIDsWithFreeControllersChangedHandlers != null && component.freeControllers.Length > 0)
			{
				this.onAtomUIDsWithFreeControllersChangedHandlers(this.GetAtomUIDsWithFreeControllers());
			}
			if (this.onAtomUIDsWithRigidbodiesChangedHandlers != null && component.linkableRigidbodies.Length > 0)
			{
				this.onAtomUIDsWithRigidbodiesChangedHandlers(this.GetAtomUIDsWithRigidbodies());
			}
			this.SyncVisibility();
			this.SyncForceReceiverNames();
			this.SyncForceProducerNames();
			this.SyncFreeControllerNames();
			this.SyncRigidbodyNames();
			this.SyncHiddenAtoms();
			this.SyncSelectAtomPopup();
			if (forceSelect || (userInvoked && component.mainController != null && this.selectAtomOnAddToggle != null && this.selectAtomOnAddToggle.isOn))
			{
				this.SelectController(component.mainController, false, true, true, true);
			}
			if (forceFocus || (userInvoked && component.mainController != null && this.focusAtomOnAddToggle != null && this.focusAtomOnAddToggle.isOn))
			{
				this.FocusOnController(component.mainController, false, true);
			}
			this.lastAddedAtom = component;
			return transform;
		}
		return null;
	}

	// Token: 0x06005E1E RID: 24094 RVA: 0x00235424 File Offset: 0x00233824
	public void RenameAtom(Atom atom, string requestedID)
	{
		if (atom != null)
		{
			if (atom.uid == requestedID)
			{
				return;
			}
			string uid = atom.uid;
			string text = this.CreateUID(requestedID);
			this.atoms.Remove(atom.uid);
			this.uids.Remove(atom.uid);
			int num = -1;
			if (this.atomUIDs.Contains(atom.uid))
			{
				num = this.atomUIDs.IndexOf(atom.uid);
				this.atomUIDs.RemoveAt(num);
				this.sortedAtomUIDs.Remove(atom.uid);
			}
			int num2 = -1;
			if (this.atomUIDsWithForceReceivers.Contains(atom.uid))
			{
				num2 = this.atomUIDsWithForceReceivers.IndexOf(atom.uid);
				this.atomUIDsWithForceReceivers.RemoveAt(num2);
				this.sortedAtomUIDsWithForceReceivers.Remove(atom.uid);
			}
			int num3 = -1;
			if (this.atomUIDsWithForceProducers.Contains(atom.uid))
			{
				num3 = this.atomUIDsWithForceProducers.IndexOf(atom.uid);
				this.atomUIDsWithForceProducers.RemoveAt(num3);
				this.sortedAtomUIDsWithForceProducers.Remove(atom.uid);
			}
			int num4 = -1;
			if (this.atomUIDsWithRhythmControllers.Contains(atom.uid))
			{
				num4 = this.atomUIDsWithRhythmControllers.IndexOf(atom.uid);
				this.atomUIDsWithRhythmControllers.RemoveAt(num4);
				this.sortedAtomUIDsWithRhythmControllers.Remove(atom.uid);
			}
			int num5 = -1;
			if (this.atomUIDsWithAudioSourceControls.Contains(atom.uid))
			{
				num5 = this.atomUIDsWithAudioSourceControls.IndexOf(atom.uid);
				this.atomUIDsWithAudioSourceControls.RemoveAt(num5);
				this.sortedAtomUIDsWithAudioSourceControls.Remove(atom.uid);
			}
			int num6 = -1;
			if (this.atomUIDsWithFreeControllers.Contains(atom.uid))
			{
				num6 = this.atomUIDsWithFreeControllers.IndexOf(atom.uid);
				this.atomUIDsWithFreeControllers.RemoveAt(num6);
				this.sortedAtomUIDsWithFreeControllers.Remove(atom.uid);
			}
			int num7 = -1;
			if (this.atomUIDsWithRigidbodies.Contains(atom.uid))
			{
				num7 = this.atomUIDsWithRigidbodies.IndexOf(atom.uid);
				this.atomUIDsWithRigidbodies.RemoveAt(num7);
				this.sortedAtomUIDsWithRigidbodies.Remove(atom.uid);
			}
			foreach (FreeControllerV3 freeControllerV in atom.freeControllers)
			{
				string key = atom.uid + ":" + freeControllerV.name;
				this.fcMap.Remove(key);
			}
			foreach (ForceProducerV2 forceProducerV in atom.forceProducers)
			{
				string key2 = atom.uid + ":" + forceProducerV.name;
				this.fpMap.Remove(key2);
			}
			foreach (RhythmController rhythmController in atom.rhythmControllers)
			{
				string key3 = atom.uid + ":" + rhythmController.name;
				this.rcMap.Remove(key3);
			}
			foreach (AudioSourceControl audioSourceControl in atom.audioSourceControls)
			{
				string key4 = atom.uid + ":" + audioSourceControl.name;
				this.ascMap.Remove(key4);
			}
			foreach (GrabPoint grabPoint in atom.grabPoints)
			{
				string key5 = atom.uid + ":" + grabPoint.name;
				this.gpMap.Remove(key5);
			}
			foreach (ForceReceiver forceReceiver in atom.forceReceivers)
			{
				string key6 = atom.uid + ":" + forceReceiver.name;
				this.frMap.Remove(key6);
			}
			foreach (Rigidbody rigidbody in atom.linkableRigidbodies)
			{
				string key7 = atom.uid + ":" + rigidbody.name;
				this.rbMap.Remove(key7);
			}
			foreach (MotionAnimationControl motionAnimationControl in atom.motionAnimationControls)
			{
				string key8 = atom.uid + ":" + motionAnimationControl.name;
				this.macMap.Remove(key8);
			}
			foreach (PlayerNavCollider playerNavCollider in atom.playerNavColliders)
			{
				string key9 = atom.uid + ":" + playerNavCollider.name;
				this.pncMap.Remove(key9);
			}
			atom.uid = text;
			atom.name = text;
			this.atoms.Add(atom.uid, atom);
			if (num != -1)
			{
				this.atomUIDs.Insert(num, atom.uid);
				this.sortedAtomUIDs.Add(atom.uid);
				this.SyncSortedAtomUIDs();
				if (this.onAtomUIDsChangedHandlers != null)
				{
					this.onAtomUIDsChangedHandlers(this.GetAtomUIDs());
				}
			}
			if (num2 != -1)
			{
				this.atomUIDsWithForceReceivers.Insert(num2, atom.uid);
				this.sortedAtomUIDsWithForceReceivers.Add(atom.uid);
				this.SyncSortedAtomUIDsWithForceReceivers();
				if (this.onAtomUIDsWithForceReceiversChangedHandlers != null)
				{
					this.onAtomUIDsWithForceReceiversChangedHandlers(this.GetAtomUIDsWithForceReceivers());
				}
			}
			if (num3 != -1)
			{
				this.atomUIDsWithForceProducers.Insert(num3, atom.uid);
				this.sortedAtomUIDsWithForceProducers.Add(atom.uid);
				this.SyncSortedAtomUIDsWithForceProducers();
				if (this.onAtomUIDsWithForceProducersChangedHandlers != null)
				{
					this.onAtomUIDsWithForceProducersChangedHandlers(this.GetAtomUIDsWithForceProducers());
				}
			}
			if (num4 != -1)
			{
				this.atomUIDsWithRhythmControllers.Insert(num4, atom.uid);
				this.sortedAtomUIDsWithRhythmControllers.Add(atom.uid);
				this.SyncSortedAtomUIDsWithRhythmControllers();
			}
			if (num5 != -1)
			{
				this.atomUIDsWithAudioSourceControls.Insert(num5, atom.uid);
				this.sortedAtomUIDsWithAudioSourceControls.Add(atom.uid);
				this.SyncSortedAtomUIDsWithAudioSourceControls();
			}
			if (num6 != -1)
			{
				this.atomUIDsWithFreeControllers.Insert(num6, atom.uid);
				this.sortedAtomUIDsWithFreeControllers.Add(atom.uid);
				this.SyncSortedAtomUIDsWithFreeControllers();
				if (this.onAtomUIDsWithFreeControllersChangedHandlers != null)
				{
					this.onAtomUIDsWithFreeControllersChangedHandlers(this.GetAtomUIDsWithFreeControllers());
				}
			}
			if (num7 != -1)
			{
				this.atomUIDsWithRigidbodies.Insert(num7, atom.uid);
				this.sortedAtomUIDsWithRigidbodies.Add(atom.uid);
				this.SyncSortedAtomUIDsWithRigidbodies();
				if (this.onAtomUIDsWithRigidbodiesChangedHandlers != null)
				{
					this.onAtomUIDsWithRigidbodiesChangedHandlers(this.GetAtomUIDsWithRigidbodies());
				}
			}
			this.SyncHiddenAtoms();
			foreach (FreeControllerV3 freeControllerV2 in atom.freeControllers)
			{
				string key10 = atom.uid + ":" + freeControllerV2.name;
				this.fcMap.Add(key10, freeControllerV2);
			}
			foreach (ForceProducerV2 forceProducerV2 in atom.forceProducers)
			{
				string key11 = atom.uid + ":" + forceProducerV2.name;
				this.fpMap.Add(key11, forceProducerV2);
			}
			foreach (ForceReceiver forceReceiver2 in atom.forceReceivers)
			{
				string key12 = atom.uid + ":" + forceReceiver2.name;
				this.frMap.Add(key12, forceReceiver2);
			}
			foreach (RhythmController rhythmController2 in atom.rhythmControllers)
			{
				string key13 = atom.uid + ":" + rhythmController2.name;
				this.rcMap.Add(key13, rhythmController2);
			}
			foreach (AudioSourceControl audioSourceControl2 in atom.audioSourceControls)
			{
				string key14 = atom.uid + ":" + audioSourceControl2.name;
				this.ascMap.Add(key14, audioSourceControl2);
			}
			foreach (GrabPoint grabPoint2 in atom.grabPoints)
			{
				string key15 = atom.uid + ":" + grabPoint2.name;
				this.gpMap.Add(key15, grabPoint2);
			}
			foreach (Rigidbody rigidbody2 in atom.linkableRigidbodies)
			{
				string key16 = atom.uid + ":" + rigidbody2.name;
				this.rbMap.Add(key16, rigidbody2);
			}
			foreach (MotionAnimationControl motionAnimationControl2 in atom.motionAnimationControls)
			{
				string key17 = atom.uid + ":" + motionAnimationControl2.name;
				this.macMap.Add(key17, motionAnimationControl2);
			}
			foreach (PlayerNavCollider playerNavCollider2 in atom.playerNavColliders)
			{
				string key18 = atom.uid + ":" + playerNavCollider2.name;
				this.pncMap.Add(key18, playerNavCollider2);
			}
			if (this.onAtomUIDRenameHandlers != null)
			{
				this.onAtomUIDRenameHandlers(uid, text);
			}
			this.SyncSelectAtomPopup();
		}
	}

	// Token: 0x06005E1F RID: 24095 RVA: 0x00235E4A File Offset: 0x0023424A
	public void RemoveAtom(Atom atom)
	{
		this.RemoveAtom(atom, true);
	}

	// Token: 0x06005E20 RID: 24096 RVA: 0x00235E54 File Offset: 0x00234254
	public void RemoveAtom(Atom atom, bool syncList)
	{
		if (atom != null && !atom.destroyed)
		{
			if (atom == this.isolatedAtom)
			{
				this.EndIsolateEditAtom();
			}
			atom.OnPreRemove();
			List<Atom> list = new List<Atom>();
			foreach (Atom atom2 in this.atomsList)
			{
				if (atom2 != null)
				{
					list.Add(atom2);
				}
			}
			foreach (Atom atom3 in list)
			{
				if (atom3 != null && atom3.parentAtom == atom)
				{
					atom3.parentAtom = null;
				}
			}
			if (this.selectedController != null && this.selectedController.containingAtom != null && this.selectedController.containingAtom == atom)
			{
				this.ClearSelection(true);
			}
			if (atom.parentAtom != null)
			{
				atom.parentAtom = null;
			}
			this.atoms.Remove(atom.uid);
			if (syncList)
			{
				this.atomsList.Remove(atom);
			}
			this.atomUIDs.Remove(atom.uid);
			this.sortedAtomUIDs.Remove(atom.uid);
			this.uids.Remove(atom.uid);
			if (this.playerNavCollider != null && this.playerNavCollider.containingAtom == atom)
			{
				this.DisconnectNavRigFromPlayerNavCollider();
			}
			if (this.onAtomRemovedHandlers != null)
			{
				this.onAtomRemovedHandlers(atom);
			}
			if (this.onAtomUIDsChangedHandlers != null)
			{
				this.onAtomUIDsChangedHandlers(this.GetAtomUIDs());
			}
			if (this.atomUIDsWithForceReceivers.Remove(atom.uid))
			{
				this.sortedAtomUIDsWithForceReceivers.Remove(atom.uid);
				if (this.onAtomUIDsWithForceReceiversChangedHandlers != null)
				{
					this.onAtomUIDsWithForceReceiversChangedHandlers(this.GetAtomUIDsWithForceReceivers());
				}
			}
			if (this.atomUIDsWithForceProducers.Remove(atom.uid))
			{
				this.sortedAtomUIDsWithForceProducers.Remove(atom.uid);
				if (this.onAtomUIDsWithForceProducersChangedHandlers != null)
				{
					this.onAtomUIDsWithForceProducersChangedHandlers(this.GetAtomUIDsWithForceProducers());
				}
			}
			if (this.atomUIDsWithRhythmControllers.Remove(atom.uid))
			{
				this.sortedAtomUIDsWithRhythmControllers.Remove(atom.uid);
			}
			if (this.atomUIDsWithAudioSourceControls.Remove(atom.uid))
			{
				this.sortedAtomUIDsWithAudioSourceControls.Remove(atom.uid);
			}
			if (this.atomUIDsWithFreeControllers.Remove(atom.uid))
			{
				this.sortedAtomUIDsWithFreeControllers.Remove(atom.uid);
				if (this.onAtomUIDsWithFreeControllersChangedHandlers != null)
				{
					this.onAtomUIDsWithFreeControllersChangedHandlers(this.GetAtomUIDsWithFreeControllers());
				}
			}
			if (this.atomUIDsWithRigidbodies.Remove(atom.uid))
			{
				this.sortedAtomUIDsWithRigidbodies.Remove(atom.uid);
				if (this.onAtomUIDsWithRigidbodiesChangedHandlers != null)
				{
					this.onAtomUIDsWithRigidbodiesChangedHandlers(this.GetAtomUIDsWithRigidbodies());
				}
			}
			this.SyncHiddenAtoms();
			foreach (FreeControllerV3 freeControllerV in atom.freeControllers)
			{
				this.allControllers.Remove(freeControllerV);
				string key = atom.uid + ":" + freeControllerV.name;
				this.fcMap.Remove(key);
			}
			foreach (ForceProducerV2 forceProducerV in atom.forceProducers)
			{
				string key2 = atom.uid + ":" + forceProducerV.name;
				this.fpMap.Remove(key2);
			}
			foreach (RhythmController rhythmController in atom.rhythmControllers)
			{
				string key3 = atom.uid + ":" + rhythmController.name;
				this.rcMap.Remove(key3);
			}
			foreach (AudioSourceControl audioSourceControl in atom.audioSourceControls)
			{
				string key4 = atom.uid + ":" + audioSourceControl.name;
				this.ascMap.Remove(key4);
			}
			foreach (GrabPoint grabPoint in atom.grabPoints)
			{
				string key5 = atom.uid + ":" + grabPoint.name;
				this.gpMap.Remove(key5);
			}
			foreach (ForceReceiver forceReceiver in atom.forceReceivers)
			{
				string key6 = atom.uid + ":" + forceReceiver.name;
				this.frMap.Remove(key6);
			}
			foreach (Rigidbody rigidbody in atom.linkableRigidbodies)
			{
				string key7 = atom.uid + ":" + rigidbody.name;
				this.rbMap.Remove(key7);
			}
			foreach (AnimationPattern item in atom.animationPatterns)
			{
				this.allAnimationPatterns.Remove(item);
			}
			foreach (AnimationStep item2 in atom.animationSteps)
			{
				this.allAnimationSteps.Remove(item2);
			}
			foreach (Animator item3 in atom.animators)
			{
				this.allAnimators.Remove(item3);
			}
			foreach (MotionAnimationControl motionAnimationControl in atom.motionAnimationControls)
			{
				string key8 = atom.uid + ":" + motionAnimationControl.name;
				this.macMap.Remove(key8);
			}
			foreach (PlayerNavCollider playerNavCollider in atom.playerNavColliders)
			{
				string key9 = atom.uid + ":" + playerNavCollider.name;
				this.pncMap.Remove(key9);
			}
			foreach (Canvas item4 in atom.canvases)
			{
				this.allCanvases.Remove(item4);
			}
			if (this.motionAnimationMaster != null)
			{
				foreach (MotionAnimationControl mac in atom.motionAnimationControls)
				{
					this.motionAnimationMaster.DeregisterAnimationControl(mac);
				}
			}
			this.SyncForceReceiverNames();
			this.SyncForceProducerNames();
			this.SyncFreeControllerNames();
			this.SyncRigidbodyNames();
			this.SyncSelectAtomPopup();
			atom.OnRemove();
			atom.destroyed = true;
			this.ValidateAllAtoms();
			if (this.atomPoolContainer != null && atom.isPoolable)
			{
				this.PutAtomBackInPool(atom);
			}
			else
			{
				UnityEngine.Object.Destroy(atom.gameObject);
				SuperController.AtomAsset atomAsset;
				if (atom.loadedFromBundle && atom.type != null && atom.type != string.Empty && this.atomAssetByType.TryGetValue(atom.type, out atomAsset))
				{
					this.UnregisterPrefab(atomAsset.assetBundleName, atomAsset.assetName);
				}
			}
		}
	}

	// Token: 0x06005E21 RID: 24097 RVA: 0x002366A4 File Offset: 0x00234AA4
	public void ValidateAllAtoms()
	{
		foreach (Atom atom in this.atomsList)
		{
			atom.Validate();
		}
	}

	// Token: 0x06005E22 RID: 24098 RVA: 0x00236700 File Offset: 0x00234B00
	private void InitAtom(Atom atom)
	{
		this.atoms.Add(atom.uid, atom);
		this.atomsList.Add(atom);
		this.atomUIDs.Add(atom.uid);
		this.sortedAtomUIDs.Add(atom.uid);
		this.SyncSortedAtomUIDs();
		bool flag = false;
		foreach (FreeControllerV3 freeControllerV in atom.freeControllers)
		{
			flag = true;
			this.allControllers.Add(freeControllerV);
			string key = atom.uid + ":" + freeControllerV.name;
			this.fcMap.Add(key, freeControllerV);
		}
		if (flag)
		{
			this.atomUIDsWithFreeControllers.Add(atom.uid);
			this.sortedAtomUIDsWithFreeControllers.Add(atom.uid);
			this.SyncSortedAtomUIDsWithFreeControllers();
		}
		bool flag2 = false;
		foreach (ForceProducerV2 forceProducerV in atom.forceProducers)
		{
			flag2 = true;
			string key2 = atom.uid + ":" + forceProducerV.name;
			this.fpMap.Add(key2, forceProducerV);
		}
		if (flag2)
		{
			this.atomUIDsWithForceProducers.Add(atom.uid);
			this.sortedAtomUIDsWithForceProducers.Add(atom.uid);
			this.SyncSortedAtomUIDsWithForceProducers();
		}
		bool flag3 = false;
		foreach (ForceReceiver forceReceiver in atom.forceReceivers)
		{
			flag3 = true;
			string key3 = atom.uid + ":" + forceReceiver.name;
			this.frMap.Add(key3, forceReceiver);
		}
		if (flag3)
		{
			this.atomUIDsWithForceReceivers.Add(atom.uid);
			this.sortedAtomUIDsWithForceReceivers.Add(atom.uid);
			this.SyncSortedAtomUIDsWithForceReceivers();
		}
		bool flag4 = false;
		foreach (RhythmController rhythmController in atom.rhythmControllers)
		{
			flag4 = true;
			string key4 = atom.uid + ":" + rhythmController.name;
			this.rcMap.Add(key4, rhythmController);
		}
		if (flag4)
		{
			this.atomUIDsWithRhythmControllers.Add(atom.uid);
			this.sortedAtomUIDsWithRhythmControllers.Add(atom.uid);
			this.SyncSortedAtomUIDsWithRhythmControllers();
		}
		bool flag5 = false;
		foreach (AudioSourceControl audioSourceControl in atom.audioSourceControls)
		{
			flag5 = true;
			string key5 = atom.uid + ":" + audioSourceControl.name;
			this.ascMap.Add(key5, audioSourceControl);
		}
		if (flag5)
		{
			this.atomUIDsWithAudioSourceControls.Add(atom.uid);
			this.sortedAtomUIDsWithAudioSourceControls.Add(atom.uid);
			this.SyncSortedAtomUIDsWithAudioSourceControls();
		}
		foreach (GrabPoint grabPoint in atom.grabPoints)
		{
			string key6 = atom.uid + ":" + grabPoint.name;
			this.gpMap.Add(key6, grabPoint);
		}
		bool flag6 = false;
		foreach (Rigidbody rigidbody in atom.rigidbodies)
		{
			rigidbody.maxAngularVelocity = this.maxAngularVelocity;
			rigidbody.maxDepenetrationVelocity = this.maxDepenetrationVelocity;
			rigidbody.solverIterations = this._solverIterations;
		}
		foreach (PhysicsSimulator physicsSimulator in atom.physicsSimulators)
		{
			physicsSimulator.solverIterations = this._solverIterations;
		}
		foreach (PhysicsSimulatorJSONStorable physicsSimulatorJSONStorable in atom.physicsSimulatorsStorable)
		{
			physicsSimulatorJSONStorable.solverIterations = this._solverIterations;
		}
		foreach (Rigidbody rigidbody2 in atom.linkableRigidbodies)
		{
			flag6 = true;
			string key7 = atom.uid + ":" + rigidbody2.name;
			this.rbMap.Add(key7, rigidbody2);
		}
		if (flag6)
		{
			this.atomUIDsWithRigidbodies.Add(atom.uid);
			this.sortedAtomUIDsWithRigidbodies.Add(atom.uid);
			this.SyncSortedAtomUIDsWithRigidbodies();
		}
		foreach (AnimationPattern item in atom.animationPatterns)
		{
			this.allAnimationPatterns.Add(item);
		}
		foreach (AnimationStep item2 in atom.animationSteps)
		{
			this.allAnimationSteps.Add(item2);
		}
		foreach (Animator item3 in atom.animators)
		{
			this.allAnimators.Add(item3);
		}
		foreach (Canvas canvas in atom.canvases)
		{
			if (this.overrideCanvasSortingLayer)
			{
				IgnoreCanvas component = canvas.GetComponent<IgnoreCanvas>();
				if (component == null)
				{
					canvas.sortingLayerName = this.overrideCanvasSortingLayerName;
				}
			}
			this.allCanvases.Add(canvas);
		}
		if (this.motionAnimationMaster != null)
		{
			foreach (MotionAnimationControl motionAnimationControl in atom.motionAnimationControls)
			{
				string key8 = atom.uid + ":" + motionAnimationControl.name;
				this.macMap.Add(key8, motionAnimationControl);
				this.motionAnimationMaster.RegisterAnimationControl(motionAnimationControl);
			}
		}
		foreach (PlayerNavCollider playerNavCollider in atom.playerNavColliders)
		{
			string key9 = atom.uid + ":" + playerNavCollider.name;
			this.pncMap.Add(key9, playerNavCollider);
		}
		this.SyncHiddenAtoms();
		atom.useRigidbodyInterpolation = this._useInterpolation;
	}

	// Token: 0x06005E23 RID: 24099 RVA: 0x00236D6C File Offset: 0x0023516C
	public void SetAtomAssetsFromFile()
	{
		if (this.atomAssetsFile != null && this.atomAssetsFile != string.Empty)
		{
			string text = File.ReadAllText(this.atomAssetsFile);
			string[] array = text.Split(new char[]
			{
				'\n'
			});
			List<SuperController.AtomAsset> list = new List<SuperController.AtomAsset>();
			foreach (string input in array)
			{
				string[] array3 = Regex.Split(input, "\\s+");
				if (array3.Length >= 3)
				{
					list.Add(new SuperController.AtomAsset
					{
						assetBundleName = array3[0],
						assetName = array3[1],
						category = array3[2]
					});
				}
			}
			this.atomAssets = list.ToArray();
		}
	}

	// Token: 0x06005E24 RID: 24100 RVA: 0x00236E34 File Offset: 0x00235234
	private void InitAtoms()
	{
		this.atomPrefabByType = new Dictionary<string, Atom>();
		this.atomAssetByType = new Dictionary<string, SuperController.AtomAsset>();
		foreach (SuperController.AtomAsset atomAsset in this.atomAssets)
		{
			if (atomAsset != null)
			{
				string assetName = atomAsset.assetName;
				if (!this.atomAssetByType.ContainsKey(assetName))
				{
					this.atomAssetByType.Add(assetName, atomAsset);
				}
				else
				{
					this.Error("Atom asset " + assetName + " is a duplicate", true, true);
				}
			}
		}
		foreach (SuperController.AtomAsset atomAsset2 in this.indirectAtomAssets)
		{
			if (atomAsset2 != null)
			{
				string assetName2 = atomAsset2.assetName;
				if (!this.atomAssetByType.ContainsKey(assetName2))
				{
					this.atomAssetByType.Add(assetName2, atomAsset2);
				}
				else
				{
					this.Error("Atom asset " + assetName2 + " is a duplicate", true, true);
				}
			}
		}
		foreach (Atom atom in this.atomPrefabs)
		{
			if (atom != null)
			{
				string type = atom.type;
				if (!this.atomPrefabByType.ContainsKey(type))
				{
					this.atomPrefabByType.Add(type, atom);
				}
				else
				{
					this.Error(string.Concat(new string[]
					{
						"Atom ",
						atom.name,
						" uses type ",
						type,
						" that is already used"
					}), true, true);
				}
			}
		}
		foreach (Atom atom2 in this.indirectAtomPrefabs)
		{
			if (atom2 != null)
			{
				string type2 = atom2.type;
				if (!this.atomPrefabByType.ContainsKey(type2))
				{
					this.atomPrefabByType.Add(type2, atom2);
				}
				else
				{
					this.Error(string.Concat(new string[]
					{
						"Atom ",
						atom2.name,
						" uses type ",
						type2,
						" that is already used"
					}), true, true);
				}
			}
		}
		this.atomTypes = new List<string>();
		this.atomCategories = new List<string>();
		this.atomCategoryToAtomTypes = new Dictionary<string, List<string>>();
		foreach (Atom atom3 in this.atomPrefabs)
		{
			this.atomTypes.Add(atom3.type);
			List<string> list;
			if (this.atomCategoryToAtomTypes.TryGetValue(atom3.category, out list))
			{
				list.Add(atom3.type);
			}
			else
			{
				this.atomCategories.Add(atom3.category);
				list = new List<string>();
				list.Add(atom3.type);
				this.atomCategoryToAtomTypes.Add(atom3.category, list);
			}
		}
		foreach (SuperController.AtomAsset atomAsset3 in this.atomAssets)
		{
			this.atomTypes.Add(atomAsset3.assetName);
			List<string> list2;
			if (this.atomCategoryToAtomTypes.TryGetValue(atomAsset3.category, out list2))
			{
				list2.Add(atomAsset3.assetName);
			}
			else
			{
				this.atomCategories.Add(atomAsset3.category);
				list2 = new List<string>();
				list2.Add(atomAsset3.assetName);
				this.atomCategoryToAtomTypes.Add(atomAsset3.category, list2);
			}
		}
		this.atomTypes.Sort();
		this.atomCategories.Sort();
		foreach (List<string> list3 in this.atomCategoryToAtomTypes.Values)
		{
			list3.Sort();
		}
		if (this.atomCategoryPopup != null)
		{
			this.atomCategoryPopup.currentValue = "None";
			this.atomCategoryPopup.numPopupValues = this.atomCategories.Count;
			for (int num = 0; num < this.atomCategories.Count; num++)
			{
				this.atomCategoryPopup.setPopupValue(num, this.atomCategories[num]);
			}
			UIPopup uipopup = this.atomCategoryPopup;
			uipopup.onValueChangeHandlers = (UIPopup.OnValueChange)System.Delegate.Combine(uipopup.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetAddAtomAtomPopupValues));
		}
		if (this.atomPrefabPopup != null)
		{
			this.atomPrefabPopup.currentValue = "None";
		}
		this.atoms = new Dictionary<string, Atom>();
		this.atomsList = new List<Atom>();
		this.atomUIDs = new List<string>();
		this.atomUIDsWithForceReceivers = new List<string>();
		this.atomUIDsWithForceProducers = new List<string>();
		this.atomUIDsWithRhythmControllers = new List<string>();
		this.atomUIDsWithAudioSourceControls = new List<string>();
		this.atomUIDsWithFreeControllers = new List<string>();
		this.atomUIDsWithRigidbodies = new List<string>();
		this.sortedAtomUIDs = new List<string>();
		this.sortedAtomUIDsWithForceReceivers = new List<string>();
		this.sortedAtomUIDsWithForceProducers = new List<string>();
		this.sortedAtomUIDsWithRhythmControllers = new List<string>();
		this.sortedAtomUIDsWithAudioSourceControls = new List<string>();
		this.sortedAtomUIDsWithFreeControllers = new List<string>();
		this.sortedAtomUIDsWithRigidbodies = new List<string>();
		this.hiddenAtomUIDs = new List<string>();
		this.hiddenAtomUIDsWithFreeControllers = new List<string>();
		this.visibleAtomUIDs = new List<string>();
		this.visibleAtomUIDsWithFreeControllers = new List<string>();
		this.uids = new Dictionary<string, bool>();
		this.frMap = new Dictionary<string, ForceReceiver>();
		this.fpMap = new Dictionary<string, ForceProducerV2>();
		this.rcMap = new Dictionary<string, RhythmController>();
		this.ascMap = new Dictionary<string, AudioSourceControl>();
		this.gpMap = new Dictionary<string, GrabPoint>();
		this.fcMap = new Dictionary<string, FreeControllerV3>();
		this.rbMap = new Dictionary<string, Rigidbody>();
		this.macMap = new Dictionary<string, MotionAnimationControl>();
		this.pncMap = new Dictionary<string, PlayerNavCollider>();
		this.allControllers = new List<FreeControllerV3>();
		this.allAnimationPatterns = new List<AnimationPattern>();
		this.allAnimationSteps = new List<AnimationStep>();
		this.allAnimators = new List<Animator>();
		this.allCanvases = new List<Canvas>();
		Atom[] componentsInChildren = this.atomContainerTransform.GetComponentsInChildren<Atom>();
		this.startingAtoms = new HashSet<Atom>();
		foreach (Atom atom4 in componentsInChildren)
		{
			if (!atom4.exclude)
			{
				string text = this.CreateUID(atom4.name);
				if (text != null)
				{
					atom4.uid = text;
					atom4.name = text;
					this.InitAtom(atom4);
					this.startingAtoms.Add(atom4);
				}
			}
		}
		if (this.onAtomUIDsChangedHandlers != null)
		{
			this.onAtomUIDsChangedHandlers(this.GetAtomUIDs());
		}
		if (this.onAtomUIDsWithForceReceiversChangedHandlers != null)
		{
			this.onAtomUIDsWithForceReceiversChangedHandlers(this.GetAtomUIDsWithForceReceivers());
		}
		if (this.onAtomUIDsWithForceProducersChangedHandlers != null)
		{
			this.onAtomUIDsWithForceProducersChangedHandlers(this.GetAtomUIDsWithForceProducers());
		}
		if (this.onAtomUIDsWithFreeControllersChangedHandlers != null)
		{
			this.onAtomUIDsWithFreeControllersChangedHandlers(this.GetAtomUIDsWithFreeControllers());
		}
		if (this.onAtomUIDsWithRigidbodiesChangedHandlers != null)
		{
			this.onAtomUIDsWithRigidbodiesChangedHandlers(this.GetAtomUIDsWithRigidbodies());
		}
		this.SyncForceReceiverNames();
		this.SyncForceProducerNames();
		this.SyncFreeControllerNames();
		this.SyncRigidbodyNames();
		this.SyncSelectAtomPopup();
	}

	// Token: 0x06005E25 RID: 24101 RVA: 0x002375AC File Offset: 0x002359AC
	protected void SyncNavigationHologridVisibility()
	{
		if (this.navigationHologridShowTime > 0f)
		{
			this.navigationHologridTransparencyMultiplier = 5f;
		}
		else
		{
			this.navigationHologridTransparencyMultiplier = 1f;
		}
		if (this.navigationHologridVisible || this.navigationHologridShowTime > 0f)
		{
			if (!this.navigationHologrid.gameObject.activeSelf)
			{
				this.navigationHologrid.gameObject.SetActive(true);
				this.SyncHologridTransparency();
			}
		}
		else if (this.navigationHologrid.gameObject.activeSelf)
		{
			this.navigationHologrid.gameObject.SetActive(false);
		}
		if (this.navigationHologridShowTime > 0f)
		{
			this.navigationHologridShowTime -= Time.unscaledDeltaTime;
		}
	}

	// Token: 0x06005E26 RID: 24102 RVA: 0x00237678 File Offset: 0x00235A78
	protected void SplashNavigationHologrid(float seconds)
	{
		this.navigationHologridShowTime = seconds;
		this.SyncNavigationHologridVisibility();
	}

	// Token: 0x17000DD4 RID: 3540
	// (get) Token: 0x06005E27 RID: 24103 RVA: 0x00237687 File Offset: 0x00235A87
	// (set) Token: 0x06005E28 RID: 24104 RVA: 0x00237690 File Offset: 0x00235A90
	public bool showNavigationHologrid
	{
		get
		{
			return this._showNavigationHologrid;
		}
		set
		{
			if (this._showNavigationHologrid != value)
			{
				this._showNavigationHologrid = value;
				if (this.showNavigationHologridToggle != null)
				{
					this.showNavigationHologridToggle.isOn = this._showNavigationHologrid;
				}
				if (UserPreferences.singleton != null)
				{
					UserPreferences.singleton.SavePreferences();
				}
			}
		}
	}

	// Token: 0x06005E29 RID: 24105 RVA: 0x002376EC File Offset: 0x00235AEC
	private void SyncHologridTransparency()
	{
		if (this.navigationHologrid != null)
		{
			MeshRenderer component = this.navigationHologrid.GetComponent<MeshRenderer>();
			if (component != null)
			{
				Material material = component.material;
				if (material != null)
				{
					material.SetFloat("_Alpha", this._hologridTransparency * this.navigationHologridTransparencyMultiplier);
				}
			}
		}
	}

	// Token: 0x17000DD5 RID: 3541
	// (get) Token: 0x06005E2A RID: 24106 RVA: 0x0023774D File Offset: 0x00235B4D
	// (set) Token: 0x06005E2B RID: 24107 RVA: 0x00237758 File Offset: 0x00235B58
	public float hologridTransparency
	{
		get
		{
			return this._hologridTransparency;
		}
		set
		{
			if (this._hologridTransparency != value)
			{
				this._hologridTransparency = value;
				this.SyncHologridTransparency();
				if (this.hologridTransparencySlider != null)
				{
					this.hologridTransparencySlider.value = this._hologridTransparency;
				}
				if (UserPreferences.singleton != null)
				{
					UserPreferences.singleton.SavePreferences();
				}
			}
		}
	}

	// Token: 0x06005E2C RID: 24108 RVA: 0x002377BC File Offset: 0x00235BBC
	public void ResetNavigationRigPositionRotation()
	{
		if (this.navigationRig != null)
		{
			this.ClearPossess();
			this.DisconnectNavRigFromPlayerNavCollider();
			this.navigationRig.localPosition = Vector3.zero;
			this.navigationRig.localRotation = Quaternion.identity;
			this.playerHeightAdjust = 0f;
			this.ResetMonitorCenterCamera();
		}
	}

	// Token: 0x06005E2D RID: 24109 RVA: 0x00237818 File Offset: 0x00235C18
	public void SetSceneLoadPosition()
	{
		if (this.navigationRig != null)
		{
			this.sceneLoadPlayerHeightAdjust = this._playerHeightAdjust;
			this.sceneLoadPosition = this.navigationRig.position;
			this.sceneLoadRotation = this.navigationRig.rotation;
			if (this.MonitorCenterCamera != null)
			{
				this.sceneLoadMonitorCameraLocalRotation = this.MonitorCenterCamera.transform.localEulerAngles;
			}
		}
	}

	// Token: 0x06005E2E RID: 24110 RVA: 0x0023788C File Offset: 0x00235C8C
	public void MoveToSceneLoadPosition()
	{
		if (this.navigationRig != null)
		{
			this.navigationRig.position = this.sceneLoadPosition;
			this.navigationRig.rotation = this.sceneLoadRotation;
			this.playerHeightAdjust = this.sceneLoadPlayerHeightAdjust;
			if (this.MonitorCenterCamera != null)
			{
				this.MonitorCenterCamera.transform.localEulerAngles = this.sceneLoadMonitorCameraLocalRotation;
			}
		}
	}

	// Token: 0x06005E2F RID: 24111 RVA: 0x002378FF File Offset: 0x00235CFF
	public void SetUseSceneLoadPosition(bool b)
	{
		this.useSceneLoadPosition = b;
	}

	// Token: 0x17000DD6 RID: 3542
	// (get) Token: 0x06005E30 RID: 24112 RVA: 0x00237908 File Offset: 0x00235D08
	// (set) Token: 0x06005E31 RID: 24113 RVA: 0x00237910 File Offset: 0x00235D10
	public bool useSceneLoadPosition
	{
		get
		{
			return this._useSceneLoadPosition;
		}
		set
		{
			if (this._useSceneLoadPosition != value)
			{
				this._useSceneLoadPosition = value;
				if (this.useSceneLoadPositionToggle != null)
				{
					this.useSceneLoadPositionToggle.isOn = this._useSceneLoadPosition;
				}
			}
		}
	}

	// Token: 0x06005E32 RID: 24114 RVA: 0x00237948 File Offset: 0x00235D48
	private void SyncLockHeightDuringNavigate()
	{
		if (this.lockHeightDuringNavigateToggle != null)
		{
			this.lockHeightDuringNavigateToggle.isOn = this._lockHeightDuringNavigate;
		}
		if (this.lockHeightDuringNavigateToggleAlt != null)
		{
			this.lockHeightDuringNavigateToggleAlt.isOn = this._lockHeightDuringNavigate;
		}
	}

	// Token: 0x17000DD7 RID: 3543
	// (get) Token: 0x06005E33 RID: 24115 RVA: 0x00237999 File Offset: 0x00235D99
	// (set) Token: 0x06005E34 RID: 24116 RVA: 0x002379A1 File Offset: 0x00235DA1
	public bool lockHeightDuringNavigate
	{
		get
		{
			return this._lockHeightDuringNavigate;
		}
		set
		{
			if (this._lockHeightDuringNavigate != value)
			{
				this._lockHeightDuringNavigate = value;
				this.SyncLockHeightDuringNavigate();
				if (UserPreferences.singleton != null)
				{
					UserPreferences.singleton.SavePreferences();
				}
			}
		}
	}

	// Token: 0x06005E35 RID: 24117 RVA: 0x002379D6 File Offset: 0x00235DD6
	private void SyncDisableAllNavigation()
	{
		if (this.disableAllNavigationToggle != null)
		{
			this.disableAllNavigationToggle.isOn = this._disableAllNavigation;
		}
	}

	// Token: 0x06005E36 RID: 24118 RVA: 0x002379FA File Offset: 0x00235DFA
	public void ToggleDisableAllNavigation()
	{
		this.disableAllNavigation = !this._disableAllNavigation;
	}

	// Token: 0x17000DD8 RID: 3544
	// (get) Token: 0x06005E37 RID: 24119 RVA: 0x00237A0B File Offset: 0x00235E0B
	// (set) Token: 0x06005E38 RID: 24120 RVA: 0x00237A13 File Offset: 0x00235E13
	public bool disableAllNavigation
	{
		get
		{
			return this._disableAllNavigation;
		}
		set
		{
			if (this._disableAllNavigation != value)
			{
				this._disableAllNavigation = value;
				this.SyncDisableAllNavigation();
				if (UserPreferences.singleton != null)
				{
					UserPreferences.singleton.SavePreferences();
				}
			}
		}
	}

	// Token: 0x06005E39 RID: 24121 RVA: 0x00237A48 File Offset: 0x00235E48
	private void SyncFreeMoveFollowFloor()
	{
		if (this.freeMoveFollowFloorToggle != null)
		{
			this.freeMoveFollowFloorToggle.isOn = this._freeMoveFollowFloor;
		}
		if (this.freeMoveFollowFloorToggleAlt != null)
		{
			this.freeMoveFollowFloorToggleAlt.isOn = this._freeMoveFollowFloor;
		}
	}

	// Token: 0x17000DD9 RID: 3545
	// (get) Token: 0x06005E3A RID: 24122 RVA: 0x00237A99 File Offset: 0x00235E99
	// (set) Token: 0x06005E3B RID: 24123 RVA: 0x00237AA1 File Offset: 0x00235EA1
	public bool freeMoveFollowFloor
	{
		get
		{
			return this._freeMoveFollowFloor;
		}
		set
		{
			if (this._freeMoveFollowFloor != value)
			{
				this._freeMoveFollowFloor = value;
				this.SyncFreeMoveFollowFloor();
				if (UserPreferences.singleton != null)
				{
					UserPreferences.singleton.SavePreferences();
				}
			}
		}
	}

	// Token: 0x06005E3C RID: 24124 RVA: 0x00237AD6 File Offset: 0x00235ED6
	private void SyncTeleportAllowRotation()
	{
		if (this.teleportAllowRotationToggle != null)
		{
			this.teleportAllowRotationToggle.isOn = this._teleportAllowRotation;
		}
	}

	// Token: 0x17000DDA RID: 3546
	// (get) Token: 0x06005E3D RID: 24125 RVA: 0x00237AFA File Offset: 0x00235EFA
	// (set) Token: 0x06005E3E RID: 24126 RVA: 0x00237B02 File Offset: 0x00235F02
	public bool teleportAllowRotation
	{
		get
		{
			return this._teleportAllowRotation;
		}
		set
		{
			if (this._teleportAllowRotation != value)
			{
				this._teleportAllowRotation = value;
				this.SyncTeleportAllowRotation();
				if (UserPreferences.singleton != null)
				{
					UserPreferences.singleton.SavePreferences();
				}
			}
		}
	}

	// Token: 0x06005E3F RID: 24127 RVA: 0x00237B37 File Offset: 0x00235F37
	private void SyncDisableTeleport()
	{
		if (this.disableTeleportToggle != null)
		{
			this.disableTeleportToggle.isOn = this._disableTeleport;
		}
	}

	// Token: 0x17000DDB RID: 3547
	// (get) Token: 0x06005E40 RID: 24128 RVA: 0x00237B5B File Offset: 0x00235F5B
	// (set) Token: 0x06005E41 RID: 24129 RVA: 0x00237B63 File Offset: 0x00235F63
	public bool disableTeleport
	{
		get
		{
			return this._disableTeleport;
		}
		set
		{
			if (this._disableTeleport != value)
			{
				this._disableTeleport = value;
				this.SyncDisableTeleport();
				if (UserPreferences.singleton != null)
				{
					UserPreferences.singleton.SavePreferences();
				}
			}
		}
	}

	// Token: 0x06005E42 RID: 24130 RVA: 0x00237B98 File Offset: 0x00235F98
	private void SyncDisableTeleportDuringPossess()
	{
		if (this.disableTeleportDuringPossessToggle != null)
		{
			this.disableTeleportDuringPossessToggle.isOn = this._disableTeleportDuringPossess;
		}
	}

	// Token: 0x17000DDC RID: 3548
	// (get) Token: 0x06005E43 RID: 24131 RVA: 0x00237BBC File Offset: 0x00235FBC
	// (set) Token: 0x06005E44 RID: 24132 RVA: 0x00237BC4 File Offset: 0x00235FC4
	public bool disableTeleportDuringPossess
	{
		get
		{
			return this._disableTeleportDuringPossess;
		}
		set
		{
			if (this._disableTeleportDuringPossess != value)
			{
				this._disableTeleportDuringPossess = value;
				this.SyncDisableTeleportDuringPossess();
				if (UserPreferences.singleton != null)
				{
					UserPreferences.singleton.SavePreferences();
				}
			}
		}
	}

	// Token: 0x06005E45 RID: 24133 RVA: 0x00237BF9 File Offset: 0x00235FF9
	private void SyncFreeMoveMultiplier()
	{
		if (this.freeMoveMultiplierSlider != null)
		{
			this.freeMoveMultiplierSlider.value = this._freeMoveMultiplier;
		}
	}

	// Token: 0x17000DDD RID: 3549
	// (get) Token: 0x06005E46 RID: 24134 RVA: 0x00237C1D File Offset: 0x0023601D
	// (set) Token: 0x06005E47 RID: 24135 RVA: 0x00237C25 File Offset: 0x00236025
	public float freeMoveMultiplier
	{
		get
		{
			return this._freeMoveMultiplier;
		}
		set
		{
			if (this._freeMoveMultiplier != value)
			{
				this._freeMoveMultiplier = value;
				this.SyncFreeMoveMultiplier();
				if (UserPreferences.singleton != null)
				{
					UserPreferences.singleton.SavePreferences();
				}
			}
		}
	}

	// Token: 0x06005E48 RID: 24136 RVA: 0x00237C5A File Offset: 0x0023605A
	private void SyncDisableGrabNavigation()
	{
		if (this.disableGrabNavigationToggle != null)
		{
			this.disableGrabNavigationToggle.isOn = this._disableGrabNavigation;
		}
	}

	// Token: 0x17000DDE RID: 3550
	// (get) Token: 0x06005E49 RID: 24137 RVA: 0x00237C7E File Offset: 0x0023607E
	// (set) Token: 0x06005E4A RID: 24138 RVA: 0x00237C86 File Offset: 0x00236086
	public bool disableGrabNavigation
	{
		get
		{
			return this._disableGrabNavigation;
		}
		set
		{
			if (this._disableGrabNavigation != value)
			{
				this._disableGrabNavigation = value;
				this.SyncDisableGrabNavigation();
				if (UserPreferences.singleton != null)
				{
					UserPreferences.singleton.SavePreferences();
				}
			}
		}
	}

	// Token: 0x06005E4B RID: 24139 RVA: 0x00237CBB File Offset: 0x002360BB
	private void SyncGrabNavigationPositionMultiplier()
	{
		if (this.grabNavigationPositionMultiplierSlider != null)
		{
			this.grabNavigationPositionMultiplierSlider.value = this._grabNavigationPositionMultiplier;
		}
	}

	// Token: 0x17000DDF RID: 3551
	// (get) Token: 0x06005E4C RID: 24140 RVA: 0x00237CDF File Offset: 0x002360DF
	// (set) Token: 0x06005E4D RID: 24141 RVA: 0x00237CE7 File Offset: 0x002360E7
	public float grabNavigationPositionMultiplier
	{
		get
		{
			return this._grabNavigationPositionMultiplier;
		}
		set
		{
			if (this._grabNavigationPositionMultiplier != value)
			{
				this._grabNavigationPositionMultiplier = value;
				this.SyncGrabNavigationPositionMultiplier();
				if (UserPreferences.singleton != null)
				{
					UserPreferences.singleton.SavePreferences();
				}
			}
		}
	}

	// Token: 0x06005E4E RID: 24142 RVA: 0x00237D1C File Offset: 0x0023611C
	private void SyncGrabNavigationRotationMultiplier()
	{
		if (this.grabNavigationRotationMultiplierSlider != null)
		{
			this.grabNavigationRotationMultiplierSlider.value = this._grabNavigationRotationMultiplier;
		}
	}

	// Token: 0x17000DE0 RID: 3552
	// (get) Token: 0x06005E4F RID: 24143 RVA: 0x00237D40 File Offset: 0x00236140
	// (set) Token: 0x06005E50 RID: 24144 RVA: 0x00237D48 File Offset: 0x00236148
	public float grabNavigationRotationMultiplier
	{
		get
		{
			return this._grabNavigationRotationMultiplier;
		}
		set
		{
			if (this._grabNavigationRotationMultiplier != value)
			{
				this._grabNavigationRotationMultiplier = value;
				this.SyncGrabNavigationRotationMultiplier();
				if (UserPreferences.singleton != null)
				{
					UserPreferences.singleton.SavePreferences();
				}
			}
		}
	}

	// Token: 0x06005E51 RID: 24145 RVA: 0x00237D80 File Offset: 0x00236180
	private void SyncPlayerHeightAdjust()
	{
		if (this.heightAdjustTransform != null && this.navigationRig != null)
		{
			Vector3 localPosition = this.heightAdjustTransform.localPosition;
			localPosition.y = this._playerHeightAdjust / this._worldScale;
			this.heightAdjustTransform.localPosition = localPosition;
		}
	}

	// Token: 0x17000DE1 RID: 3553
	// (get) Token: 0x06005E52 RID: 24146 RVA: 0x00237DDB File Offset: 0x002361DB
	// (set) Token: 0x06005E53 RID: 24147 RVA: 0x00237DE4 File Offset: 0x002361E4
	public float playerHeightAdjust
	{
		get
		{
			return this._playerHeightAdjust;
		}
		set
		{
			if (this._playerHeightAdjust != value)
			{
				float adj = value - this._playerHeightAdjust;
				HUDAnchor.AdjustAnchorHeights(adj);
				this._playerHeightAdjust = value;
				this.SyncPlayerHeightAdjust();
				if (this.playerHeightAdjustSlider != null)
				{
					while (this._playerHeightAdjust < this.playerHeightAdjustSlider.minValue)
					{
						this.playerHeightAdjustSlider.minValue *= 2f;
					}
					while (this._playerHeightAdjust > this.playerHeightAdjustSlider.maxValue)
					{
						this.playerHeightAdjustSlider.maxValue *= 2f;
					}
					this.playerHeightAdjustSlider.value = this._playerHeightAdjust;
				}
				if (this.playerHeightAdjustSliderAlt != null)
				{
					while (this._playerHeightAdjust < this.playerHeightAdjustSliderAlt.minValue)
					{
						this.playerHeightAdjustSliderAlt.minValue *= 2f;
					}
					while (this._playerHeightAdjust > this.playerHeightAdjustSliderAlt.maxValue)
					{
						this.playerHeightAdjustSliderAlt.maxValue *= 2f;
					}
					this.playerHeightAdjustSliderAlt.value = this._playerHeightAdjust;
				}
			}
		}
	}

	// Token: 0x06005E54 RID: 24148 RVA: 0x00237F25 File Offset: 0x00236325
	public void playerHeightAdjustAdjust(float val)
	{
		this.playerHeightAdjust += val;
	}

	// Token: 0x06005E55 RID: 24149 RVA: 0x00237F38 File Offset: 0x00236338
	private void InitMotionControllerNaviation()
	{
		if (this.navigationPlayArea != null)
		{
			if (this.regularPlayArea != null)
			{
				this.regularPlayAreaMR = this.regularPlayArea.GetComponent<MeshRenderer>();
			}
			if (this.regularPlayAreaMR != null)
			{
				this.regularPlayAreaMR.enabled = false;
			}
			this.navigationPlayAreaMR = this.navigationPlayArea.GetComponent<MeshRenderer>();
			if (this.navigationPlayAreaMR != null)
			{
				this.navigationPlayAreaMR.enabled = false;
			}
			if (this.navigationCurve != null)
			{
				this.navigationCurve.draw = false;
			}
			this.navigationPlayerMR = null;
			this.navigationCameraMR = null;
			if (this.lookCamera != null)
			{
				if (this.navigationPlayer != null)
				{
					this.navigationPlayerMR = this.navigationPlayer.GetComponent<MeshRenderer>();
					if (this.navigationPlayerMR != null)
					{
						this.navigationPlayerMR.enabled = false;
					}
				}
				if (this.navigationCamera != null)
				{
					this.navigationCameraMR = this.navigationCamera.GetComponentInChildren<MeshRenderer>();
					if (this.navigationCameraMR != null)
					{
						this.navigationCameraMR.enabled = false;
					}
				}
			}
		}
	}

	// Token: 0x06005E56 RID: 24150 RVA: 0x00238080 File Offset: 0x00236480
	private void ProcessTeleportMode()
	{
		if (this.navigationPlayArea != null)
		{
			if (this.regularPlayAreaMR != null)
			{
				this.regularPlayAreaMR.enabled = false;
			}
			if (this.navigationPlayAreaMR != null)
			{
				this.navigationPlayAreaMR.enabled = false;
			}
			if (this.navigationPlayerMR != null)
			{
				this.navigationPlayerMR.enabled = false;
			}
			if (this.navigationCameraMR != null)
			{
				this.navigationCameraMR.enabled = false;
			}
			if (this.navigationCurve != null)
			{
				this.navigationCurve.draw = false;
			}
		}
		if (this.GetTeleportStart(true))
		{
			this.ProcessTeleportStart();
		}
		if (this.GetTeleportShowLeft(true))
		{
			this.ProcessTeleportShow(true);
		}
		else if (this.GetTeleportShowRight(true))
		{
			this.ProcessTeleportShow(false);
		}
		if (this.GetTeleportFinish(true))
		{
			this.ProcessTeleportFinish();
		}
		if (this.GetCancel())
		{
			this.SelectModeOff();
		}
	}

	// Token: 0x06005E57 RID: 24151 RVA: 0x0023818F File Offset: 0x0023658F
	private void ProcessTeleportStart()
	{
		if (this.navigationRig != null)
		{
			this.isTeleporting = true;
			this.startNavigateRotation = this.navigationRig.rotation;
		}
	}

	// Token: 0x06005E58 RID: 24152 RVA: 0x002381BC File Offset: 0x002365BC
	private void ProcessTeleportShow(bool isLeft)
	{
		if (this.navigationPlayArea != null && this.navigationRig != null)
		{
			this.navigationPlayArea.rotation = this.navigationRig.rotation;
		}
		if (this.regularPlayAreaMR != null)
		{
			this.regularPlayAreaMR.enabled = true;
		}
		if (this.navigationPlayAreaMR != null)
		{
			this.navigationPlayAreaMR.enabled = true;
		}
		if (this.navigationPlayerMR != null)
		{
			this.navigationPlayerMR.enabled = true;
		}
		if (this.navigationCameraMR != null)
		{
			this.navigationCameraMR.enabled = true;
		}
		bool flag = false;
		if (this.useLookForNavigation && this.lookCamera != null)
		{
			this.castRay.origin = this.lookCamera.transform.position;
			this.castRay.direction = this.lookCamera.transform.forward;
		}
		else if (isLeft)
		{
			this.castRay.origin = this.motionControllerLeft.position;
			this.castRay.direction = this.motionControllerLeft.forward;
		}
		else
		{
			this.castRay.origin = this.motionControllerRight.position;
			this.castRay.direction = this.motionControllerRight.forward;
			flag = true;
		}
		this.AllocateRaycastHits();
		int num = Physics.RaycastNonAlloc(this.castRay, this.raycastHits, this.navigationDistance, this.navigationColliderMask);
		if (num > 0)
		{
			int num2 = -1;
			float num3 = this.navigationDistance;
			for (int i = 0; i < num; i++)
			{
				float magnitude = (this.raycastHits[i].point - this.castRay.origin).magnitude;
				if (magnitude < num3)
				{
					num2 = i;
					num3 = magnitude;
				}
			}
			if (this.lookCamera != null && this.navigationPlayArea != null)
			{
				if (this.navigationPlayer != null)
				{
					Vector3 localPosition = this.lookCamera.transform.localPosition;
					localPosition.y = 0f;
					this.navigationPlayer.localPosition = localPosition;
				}
				if (this.navigationCamera != null)
				{
					this.navigationCamera.localRotation = this.lookCamera.transform.localRotation;
				}
				Vector3 b = this.navigationPlayer.position - this.navigationPlayArea.position;
				this.navigationPlayArea.position = this.raycastHits[num2].point - b;
				Collider collider = this.raycastHits[num2].collider;
				PlayerNavCollider component = collider.GetComponent<PlayerNavCollider>();
				this.teleportPlayerNavCollider = component;
				if (this._teleportAllowRotation)
				{
					this.navigationPlayArea.rotation = this.startNavigateRotation;
					if (component != null)
					{
						Quaternion lhs = Quaternion.FromToRotation(this.navigationPlayArea.up, component.transform.up);
						this.navigationPlayArea.rotation = lhs * this.navigationPlayArea.rotation;
					}
					Vector3 vector;
					if (isLeft)
					{
						vector = Quaternion2Angles.GetAngles(Quaternion.Inverse(this.lookCamera.transform.rotation) * this.motionControllerLeft.rotation, Quaternion2Angles.RotationOrder.ZXY) * 57.29578f;
					}
					else
					{
						vector = Quaternion2Angles.GetAngles(Quaternion.Inverse(this.lookCamera.transform.rotation) * this.motionControllerRight.rotation, Quaternion2Angles.RotationOrder.ZXY) * 57.29578f;
					}
					this.navigationPlayArea.Rotate(Vector3.up, -vector.z * 2f);
				}
				else if (component != null)
				{
					Quaternion lhs2 = Quaternion.FromToRotation(this.navigationPlayArea.up, component.transform.up);
					this.navigationPlayArea.rotation = lhs2 * this.navigationPlayArea.rotation;
				}
			}
			if (this.navigationCurve != null && this.navigationCurve.points != null && this.navigationCurve.points.Length == 3)
			{
				if (this.useLookForNavigation)
				{
					this.navigationCurve.points[0].transform.position = this.lookCamera.transform.position;
				}
				else if (flag)
				{
					this.navigationCurve.points[0].transform.position = this.motionControllerRight.position;
				}
				else
				{
					this.navigationCurve.points[0].transform.position = this.motionControllerLeft.position;
				}
				if (this.navigationPlayer != null)
				{
					this.navigationCurve.points[2].transform.position = this.navigationPlayer.position;
				}
				else
				{
					this.navigationCurve.points[2].transform.position = this.raycastHits[num2].point;
				}
				Vector3 position = (this.navigationCurve.points[0].transform.position + this.navigationCurve.points[2].transform.position) * 0.5f;
				position.y += 1f * this.navigationCurve.transform.lossyScale.y;
				this.navigationCurve.points[1].transform.position = position;
				this.navigationCurve.draw = true;
			}
		}
	}

	// Token: 0x06005E59 RID: 24153 RVA: 0x002387A0 File Offset: 0x00236BA0
	private void DisconnectNavRigFromPlayerNavCollider()
	{
		if (this.playerNavCollider != null)
		{
			this.playerNavTrackerGO.transform.SetParent(null);
			this.playerNavCollider = null;
			Vector3 position = this.navigationRig.position;
			Quaternion rotation = this.navigationRig.rotation;
			this.navigationRigParent.localPosition = Vector3.zero;
			this.navigationRigParent.localRotation = Quaternion.identity;
			this.navigationRig.position = position;
			this.navigationRig.rotation = rotation;
		}
	}

	// Token: 0x06005E5A RID: 24154 RVA: 0x00238828 File Offset: 0x00236C28
	private void ProcessPlayerNavMove()
	{
		if (this.playerNavCollider != null && this.navigationRigParent != null)
		{
			this.navigationRigParent.transform.position = this.playerNavTrackerGO.transform.position;
			this.navigationRigParent.transform.rotation = this.playerNavTrackerGO.transform.rotation;
		}
	}

	// Token: 0x06005E5B RID: 24155 RVA: 0x00238898 File Offset: 0x00236C98
	private void ConnectNavRigToPlayerNavCollider()
	{
		if (this.navigationRigParent != null)
		{
			this.navigationRigParent.position = this.navigationRig.position;
			this.navigationRigParent.rotation = this.navigationRig.rotation;
			this.navigationRig.position = this.navigationRigParent.position;
			this.navigationRig.rotation = this.navigationRigParent.rotation;
			if (this.playerNavCollider)
			{
				this.playerNavTrackerGO.transform.SetParent(this.playerNavCollider.transform);
				this.playerNavTrackerGO.transform.position = this.navigationRigParent.position;
				this.playerNavTrackerGO.transform.rotation = this.navigationRigParent.rotation;
			}
			else
			{
				this.playerNavTrackerGO.transform.SetParent(null);
			}
		}
	}

	// Token: 0x06005E5C RID: 24156 RVA: 0x00238988 File Offset: 0x00236D88
	private void ProcessTeleportFinish()
	{
		if (this.isTeleporting)
		{
			this.navigationRig.position = this.navigationPlayArea.position;
			this.navigationRig.rotation = this.navigationPlayArea.rotation;
			this.playerNavCollider = this.teleportPlayerNavCollider;
			this.ConnectNavRigToPlayerNavCollider();
		}
		this.isTeleporting = false;
	}

	// Token: 0x06005E5D RID: 24157 RVA: 0x002389E8 File Offset: 0x00236DE8
	private void ProcessMotionControllerNavigation()
	{
		if (this.isMonitorOnly)
		{
			return;
		}
		this.didStartLeftNavigate = false;
		this.didStartRightNavigate = false;
		if (this.navigationPlayArea != null)
		{
			if (this.regularPlayAreaMR != null)
			{
				this.regularPlayAreaMR.enabled = false;
			}
			if (this.navigationPlayAreaMR != null)
			{
				this.navigationPlayAreaMR.enabled = false;
			}
			if (this.navigationPlayerMR != null)
			{
				this.navigationPlayerMR.enabled = false;
			}
			if (this.navigationCameraMR != null)
			{
				this.navigationCameraMR.enabled = false;
			}
			if (this.navigationCurve != null)
			{
				this.navigationCurve.draw = false;
			}
		}
		if (!this.navigationDisabled && !this._disableAllNavigation && !this.worldUIActivated && this.navigationRig != null)
		{
			bool flag = this.GetLeftSelect() && this.highlightedControllersLeft.Count > 0;
			bool flag2 = this.GetRightSelect() && this.highlightedControllersRight.Count > 0;
			if (!this._disableGrabNavigation && this.GetGrabNavigateStartLeft() && !flag)
			{
				this.startGrabNavigatePositionLeft = this.motionControllerLeft.position;
				this.startGrabNavigateRotationLeft = this.motionControllerLeft.rotation;
				this.isGrabNavigatingLeft = true;
				this.didStartLeftNavigate = true;
			}
			if (this.isGrabNavigatingLeft && this.GetGrabNavigateLeft())
			{
				Vector3 vector = this.navigationRig.position;
				vector += (this.startGrabNavigatePositionLeft - this.motionControllerLeft.position) * this._grabNavigationPositionMultiplier;
				Vector3 up = this.navigationRig.up;
				float num = Vector3.Dot(vector - this.navigationRig.position, up);
				vector += up * -num;
				this.navigationRig.position = vector;
				if (!this._lockHeightDuringNavigate)
				{
					this.playerHeightAdjust += num;
				}
				this.startGrabNavigatePositionLeft = this.motionControllerLeft.position;
				float num2 = (Quaternion2Angles.GetAngles(this.motionControllerLeft.rotation * Quaternion.Inverse(this.startGrabNavigateRotationLeft), Quaternion2Angles.RotationOrder.ZXY) * 57.29578f).y * this._grabNavigationRotationMultiplier;
				if (num2 > 0f)
				{
					num2 -= this._grabNavigationRotationResistance;
					if (num2 < 0f)
					{
						num2 = 0f;
					}
				}
				if (num2 < 0f)
				{
					num2 += this._grabNavigationRotationResistance;
					if (num2 > 0f)
					{
						num2 = 0f;
					}
				}
				this.navigationRig.RotateAround(this.lookCamera.transform.position, this.navigationRig.up, -num2);
				this.startGrabNavigateRotationLeft = this.motionControllerLeft.rotation;
			}
			else
			{
				this.isGrabNavigatingLeft = false;
			}
			if (!this._disableGrabNavigation && this.GetGrabNavigateStartRight() && !flag2)
			{
				this.startGrabNavigatePositionRight = this.motionControllerRight.position;
				this.startGrabNavigateRotationRight = this.motionControllerRight.rotation;
				this.isGrabNavigatingRight = true;
				this.didStartRightNavigate = true;
			}
			if (this.isGrabNavigatingRight && this.GetGrabNavigateRight())
			{
				Vector3 vector2 = this.navigationRig.position;
				vector2 += (this.startGrabNavigatePositionRight - this.motionControllerRight.position) * this._grabNavigationPositionMultiplier;
				Vector3 up2 = this.navigationRig.up;
				float num3 = Vector3.Dot(vector2 - this.navigationRig.position, up2);
				vector2 += up2 * -num3;
				this.navigationRig.position = vector2;
				if (!this._lockHeightDuringNavigate)
				{
					this.playerHeightAdjust += num3;
				}
				this.startGrabNavigatePositionRight = this.motionControllerRight.position;
				float num4 = (Quaternion2Angles.GetAngles(this.motionControllerRight.rotation * Quaternion.Inverse(this.startGrabNavigateRotationRight), Quaternion2Angles.RotationOrder.ZXY) * 57.29578f).y * this._grabNavigationRotationMultiplier;
				if (num4 > 0f)
				{
					num4 -= this._grabNavigationRotationResistance;
					if (num4 < 0f)
					{
						num4 = 0f;
					}
				}
				if (num4 < 0f)
				{
					num4 += this._grabNavigationRotationResistance;
					if (num4 > 0f)
					{
						num4 = 0f;
					}
				}
				this.navigationRig.RotateAround(this.lookCamera.transform.position, this.navigationRig.up, -num4);
				this.startGrabNavigateRotationRight = this.motionControllerRight.rotation;
			}
			else
			{
				this.isGrabNavigatingRight = false;
			}
			if (this.navigationHologrid != null)
			{
				if (this._showNavigationHologrid && (this.isGrabNavigatingLeft || this.isGrabNavigatingRight))
				{
					this.navigationHologridVisible = true;
				}
				else
				{
					this.navigationHologridVisible = false;
				}
			}
			if (!this._disableTeleport && (!this._disableTeleportDuringPossess || (this.leftPossessedController == null && this.rightPossessedController == null && this.headPossessedController == null)))
			{
				if (this.GetTeleportStartLeft(false) && !flag)
				{
					this.ProcessTeleportStart();
					this.didStartLeftNavigate = true;
				}
				if (this.GetTeleportStartRight(false) && !flag2)
				{
					this.ProcessTeleportStart();
					this.didStartRightNavigate = true;
				}
				if (this.GetTeleportShowLeft(false) && this.highlightedControllersLeft.Count == 0)
				{
					this.ProcessTeleportShow(true);
				}
				else if (this.GetTeleportShowRight(false) && this.highlightedControllersRight.Count == 0)
				{
					this.ProcessTeleportShow(false);
				}
				if (this.GetTeleportFinish(false))
				{
					this.ProcessTeleportFinish();
				}
			}
		}
	}

	// Token: 0x06005E5E RID: 24158 RVA: 0x00239004 File Offset: 0x00237404
	public void AdjustNavigationRigHeight()
	{
		if (this.navigationRig != null && this.lookCamera != null && this._freeMoveFollowFloor)
		{
			Vector3 position = this.navigationRig.position;
			Plane plane = new Plane(this.navigationRig.up, this.navigationRig.transform.position);
			this.castRay.origin = this.lookCamera.transform.position;
			this.castRay.direction = -this.navigationRig.transform.up;
			float distance;
			if (!plane.Raycast(this.castRay, out distance))
			{
				this.castRay.direction = this.navigationRig.transform.up;
				plane.Raycast(this.castRay, out distance);
			}
			this.castRay.origin = (this.castRay.GetPoint(distance) + this.lookCamera.transform.position) * 0.5f;
			this.castRay.direction = -this.navigationRig.up;
			float num = this.navigationDistance;
			Vector3 direction = this.castRay.direction;
			Vector3 a = this.navigationRig.position;
			bool flag = false;
			this.AllocateRaycastHits();
			int num2 = Physics.RaycastNonAlloc(this.castRay, this.raycastHits, this.navigationDistance, this.navigationColliderMask);
			if (num2 > 0)
			{
				flag = true;
				for (int i = 0; i < num2; i++)
				{
					float magnitude = (this.raycastHits[i].point - this.castRay.origin).magnitude;
					if (magnitude < num)
					{
						num = magnitude;
						a = this.raycastHits[i].point;
					}
				}
			}
			this.castRay.direction = this.navigationRig.up;
			num2 = Physics.RaycastNonAlloc(this.castRay, this.raycastHits, this.navigationDistance, this.navigationColliderMask);
			if (num2 > 0)
			{
				for (int j = 0; j < num2; j++)
				{
					float magnitude2 = (this.raycastHits[j].point - this.castRay.origin).magnitude;
					if (magnitude2 < num)
					{
						direction = this.castRay.direction;
						num = magnitude2;
						a = this.raycastHits[j].point;
					}
				}
			}
			if (flag)
			{
				Vector3 b = direction * Vector3.Dot(a - this.navigationRig.position, direction);
				this.navigationRig.position = Vector3.Lerp(this.navigationRig.position, this.navigationRig.position + b, Time.deltaTime * 2f);
			}
		}
	}

	// Token: 0x06005E5F RID: 24159 RVA: 0x00239308 File Offset: 0x00237708
	private void ProcessControllerNavigation(SteamVR_Action_Vector2 moveAction, bool ignoreDisable = false)
	{
		this.CheckSwapAxis();
		if (this.navigationRig != null && this.lookCamera != null && !this.navigationDisabled && !this._disableAllNavigation && !this.worldUIActivated)
		{
			Vector4 freeNavigateVector = this.GetFreeNavigateVector(moveAction, ignoreDisable);
			float num = this._freeMoveMultiplier * this._worldScale;
			bool flag = false;
			if (freeNavigateVector.x > 0.01f || freeNavigateVector.x < -0.01f)
			{
				Vector3 a = Vector3.ProjectOnPlane(this.lookCamera.transform.right, this.navigationRig.up);
				a.Normalize();
				Vector3 vector = this.navigationRig.position;
				vector += a * (freeNavigateVector.x * 0.5f * Time.unscaledDeltaTime) * num;
				this.navigationRig.position = vector;
				flag = true;
			}
			if (freeNavigateVector.y > 0.01f || freeNavigateVector.y < -0.01f)
			{
				Vector3 a2 = Vector3.ProjectOnPlane(this.lookCamera.transform.forward, this.navigationRig.up);
				a2.Normalize();
				Vector3 vector2 = this.navigationRig.position;
				vector2 += a2 * (freeNavigateVector.y * 0.5f * Time.unscaledDeltaTime) * num;
				this.navigationRig.position = vector2;
				flag = true;
			}
			if (freeNavigateVector.z > 0.01f || freeNavigateVector.z < -0.01f)
			{
				this.navigationRig.RotateAround(this.lookCamera.transform.position, this.navigationRig.up, freeNavigateVector.z * 50f * Time.unscaledDeltaTime);
				flag = true;
			}
			if ((freeNavigateVector.w > 0.01f || freeNavigateVector.w < -0.01f) && !this._lockHeightDuringNavigate)
			{
				this.playerHeightAdjust += freeNavigateVector.w * 0.5f * Time.unscaledDeltaTime * num;
				flag = true;
			}
			if (flag)
			{
				this.AdjustNavigationRigHeight();
			}
		}
	}

	// Token: 0x06005E60 RID: 24160 RVA: 0x00239550 File Offset: 0x00237950
	public void FocusOnController(FreeControllerV3 controller, bool rotationOnly = true, bool alignUpDown = true)
	{
		if (this.MonitorCenterCamera != null && controller != null)
		{
			this.AlignRigFacingController(controller, rotationOnly);
			this.SetMonitorRigPositionZero();
			Vector3 position;
			if (controller.focusPoint != null)
			{
				position = controller.focusPoint.position;
				this.focusDistance = (controller.focusPoint.position - this.MonitorCenterCamera.transform.position).magnitude;
			}
			else
			{
				position = controller.transform.position;
				this.focusDistance = (controller.transform.position - this.MonitorCenterCamera.transform.position).magnitude;
			}
			if (this.MonitorCenterCamera != null)
			{
				this.MonitorCenterCamera.transform.LookAt(position);
				Vector3 localEulerAngles = this.MonitorCenterCamera.transform.localEulerAngles;
				if (!alignUpDown)
				{
					localEulerAngles.x = 0f;
				}
				localEulerAngles.y = 0f;
				localEulerAngles.z = 0f;
				this.MonitorCenterCamera.transform.localEulerAngles = localEulerAngles;
			}
			this.SyncMonitorRigPosition();
		}
	}

	// Token: 0x06005E61 RID: 24161 RVA: 0x00239686 File Offset: 0x00237A86
	public void FocusOnSelectedController()
	{
		this.FocusOnSelectedController(true, true);
	}

	// Token: 0x06005E62 RID: 24162 RVA: 0x00239690 File Offset: 0x00237A90
	public void FocusOnSelectedController(bool rotationOnly, bool alignUpDown = true)
	{
		this.FocusOnController(this.selectedController, rotationOnly, alignUpDown);
	}

	// Token: 0x06005E63 RID: 24163 RVA: 0x002396A0 File Offset: 0x00237AA0
	public void ResetFocusPoint()
	{
		this.focusDistance = 1.5f;
		this.SyncMonitorRigPosition();
	}

	// Token: 0x06005E64 RID: 24164 RVA: 0x002396B3 File Offset: 0x00237AB3
	public void ResetMonitorCenterCamera()
	{
		this.ResetFocusPoint();
		if (this.MonitorCenterCamera != null)
		{
			this.MonitorCenterCamera.transform.localEulerAngles = Vector3.zero;
		}
	}

	// Token: 0x06005E65 RID: 24165 RVA: 0x002396E1 File Offset: 0x00237AE1
	private void ProcessFreeMoveNavigation()
	{
		this.ProcessControllerNavigation(this.freeModeMoveAction, true);
		if (this.GetCancel())
		{
			this.SelectModeOff();
		}
	}

	// Token: 0x06005E66 RID: 24166 RVA: 0x00239704 File Offset: 0x00237B04
	private void ProcessKeyBindings()
	{
		if (!this.disableInternalKeyBindings && (LookInputModule.singleton == null || !LookInputModule.singleton.inputFieldActive))
		{
			if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.RightAlt))
			{
				return;
			}
			if (Input.GetKeyDown(KeyCode.M))
			{
				this.ToggleMainMonitor();
			}
			if (Input.GetKeyDown(KeyCode.F1))
			{
				this.ToggleMonitorUI();
			}
			bool key = Input.GetKey(KeyCode.LeftShift);
			if (this.IsMonitorRigActive && !this.navigationDisabled && !this.worldUIActivated)
			{
				if (Input.GetKeyDown(KeyCode.Tab))
				{
					this.ToggleModeFreeMoveMouse();
				}
				if (Input.GetKeyDown(KeyCode.F))
				{
					if (key)
					{
						this.FocusOnSelectedController(false, true);
					}
					else
					{
						this.FocusOnSelectedController();
					}
				}
				if (Input.GetKeyDown(KeyCode.R))
				{
					this.ResetFocusPoint();
				}
			}
			if (!this.UIDisabled)
			{
				if (Input.GetKeyDown(KeyCode.E))
				{
					this.gameMode = SuperController.GameMode.Edit;
				}
				if (Input.GetKeyDown(KeyCode.P))
				{
					this.gameMode = SuperController.GameMode.Play;
				}
				if (Input.GetKeyDown(KeyCode.N))
				{
					this.CycleSelectAtomOfType("Person");
				}
				if (Input.GetKeyDown(KeyCode.U) && (UserPreferences.singleton == null || !UserPreferences.singleton.firstTimeUser))
				{
					this.ToggleMainHUDMonitor();
				}
				if (Input.GetKeyDown(KeyCode.T))
				{
					this.ToggleTargetsOnWithButton();
				}
				if (Input.GetKeyDown(KeyCode.H))
				{
					this.ToggleShowHiddenAtoms();
				}
				if (Input.GetKeyDown(KeyCode.C))
				{
					this.ProcessTargetSelectionCycleSelectMouse();
				}
			}
		}
	}

	// Token: 0x06005E67 RID: 24167 RVA: 0x002398C4 File Offset: 0x00237CC4
	private void ProcessMouseControl()
	{
		this.mouseClickUsed = false;
		if (this.navigationRig != null && this.MonitorRigActive && !this.navigationDisabled && !this.worldUIActivated)
		{
			if (Input.GetMouseButtonDown(1))
			{
			}
			if (Input.GetMouseButtonUp(1))
			{
			}
			bool flag = this.headPossessedController != null;
			if (Input.GetMouseButton(1) && !flag)
			{
				bool key = Input.GetKey(KeyCode.LeftControl);
				bool key2 = Input.GetKey(KeyCode.LeftShift);
				this.SetMonitorRigPositionZero();
				if (!key)
				{
					Vector3 point = this.MonitorCenterCamera.transform.position + this.MonitorCenterCamera.transform.forward * this.focusDistance;
					float num = this.GetMouseXChange();
					num = Mathf.Clamp(num, -10f, 10f);
					if (num > 0.01f || num < -0.01f)
					{
						this.navigationRig.RotateAround(point, this.navigationRig.up, num * 2f);
					}
				}
				if (!key2)
				{
					float num2 = this.GetMouseYChange();
					num2 = Mathf.Clamp(num2, -10f, 10f);
					if (num2 > 0.01f || num2 < -0.01f)
					{
						Vector3 vector = this.MonitorCenterCamera.transform.position + this.MonitorCenterCamera.transform.forward * this.focusDistance;
						Vector3 position = this.MonitorCenterCamera.transform.position;
						Vector3 up = this.navigationRig.up;
						Vector3 a = position - up * num2 * 0.1f * this.focusDistance;
						Vector3 a2 = a - vector;
						a2.Normalize();
						a = vector + a2 * this.focusDistance;
						Vector3 vector2 = a - position;
						Vector3 vector3 = this.navigationRig.position + vector2;
						float num3 = Vector3.Dot(vector2, up);
						vector3 += up * -num3;
						this.navigationRig.position = vector3;
						this.playerHeightAdjust += num3;
						this.MonitorCenterCamera.transform.LookAt(vector);
						Vector3 localEulerAngles = this.MonitorCenterCamera.transform.localEulerAngles;
						localEulerAngles.y = 0f;
						localEulerAngles.z = 0f;
						this.MonitorCenterCamera.transform.localEulerAngles = localEulerAngles;
					}
				}
				this.SyncMonitorRigPosition();
			}
			else if (Input.GetMouseButton(2) && (!flag || this._allowHeadPossessMousePanAndZoom))
			{
				bool key3 = Input.GetKey(KeyCode.LeftControl);
				bool key4 = Input.GetKey(KeyCode.LeftShift);
				bool flag2 = false;
				Vector3 vector4 = this.navigationRig.position;
				if (!key3)
				{
					float num4 = this.GetMouseXChange();
					num4 = Mathf.Clamp(num4, -10f, 10f);
					if (num4 > 0.01f || num4 < -0.01f)
					{
						vector4 += this.MonitorCenterCamera.transform.right * -num4 * 0.03f;
						flag2 = true;
					}
				}
				if (!key4)
				{
					float num5 = this.GetMouseYChange();
					num5 = Mathf.Clamp(num5, -10f, 10f);
					if (num5 > 0.01f || num5 < -0.01f)
					{
						vector4 += this.MonitorCenterCamera.transform.up * -num5 * 0.03f;
						flag2 = true;
					}
				}
				if (flag2)
				{
					Vector3 up2 = this.navigationRig.up;
					Vector3 lhs = vector4 - this.navigationRig.position;
					float num6 = Vector3.Dot(lhs, up2);
					vector4 += up2 * -num6;
					this.navigationRig.position = vector4;
					this.playerHeightAdjust += num6;
				}
			}
			float y = Input.mouseScrollDelta.y;
			if (!this.GUIhitMouse && (y > 0.5f || y < -0.5f) && (!flag || this._allowHeadPossessMousePanAndZoom))
			{
				Vector2 vector5 = this.MonitorCenterCamera.ScreenToViewportPoint(Input.mousePosition);
				if (vector5.x >= 0f && vector5.x <= 1f && vector5.y >= 0f && vector5.y <= 1f)
				{
					float num7 = 0.1f;
					if (y < -0.5f)
					{
						num7 = -num7;
					}
					Vector3 forward = this.MonitorCenterCamera.transform.forward;
					Vector3 vector6 = num7 * forward * this.focusDistance;
					Vector3 vector7 = this.navigationRig.position + vector6;
					this.focusDistance *= 1f - num7;
					Vector3 up3 = this.navigationRig.up;
					float num8 = Vector3.Dot(vector6, up3);
					vector7 += up3 * -num8;
					this.navigationRig.position = vector7;
					this.playerHeightAdjust += num8;
					this.SyncMonitorRigPosition();
				}
			}
		}
	}

	// Token: 0x06005E68 RID: 24168 RVA: 0x00239E3C File Offset: 0x0023823C
	private void ProcessKeyboardFreeNavigation()
	{
		if (!this.disableInternalNavigationKeyBindings && this.navigationRig != null && this.lookCamera != null && (LookInputModule.singleton == null || !LookInputModule.singleton.inputFieldActive) && !this.navigationDisabled && !this.worldUIActivated)
		{
			if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.RightAlt))
			{
				return;
			}
			bool flag = false;
			Vector2 vector;
			vector.x = 0f;
			vector.y = 0f;
			float num = this.freeMoveMultiplier * this._worldScale;
			if (Input.GetKey(KeyCode.LeftShift))
			{
				num *= 3f;
			}
			if (Input.GetKey(KeyCode.W))
			{
				vector.y = num;
			}
			if (Input.GetKey(KeyCode.A))
			{
				vector.x = -num;
			}
			if (Input.GetKey(KeyCode.S))
			{
				vector.y = -num;
			}
			if (Input.GetKey(KeyCode.D))
			{
				vector.x = num;
			}
			if (vector.y != 0f)
			{
				flag = true;
				Vector3 a = Vector3.ProjectOnPlane(this.lookCamera.transform.forward, this.navigationRig.up);
				a.Normalize();
				Vector3 vector2 = this.navigationRig.position;
				vector2 += a * (vector.y * Time.unscaledDeltaTime);
				this.navigationRig.position = vector2;
			}
			if (vector.x != 0f)
			{
				flag = true;
				Vector3 a2 = Vector3.ProjectOnPlane(this.lookCamera.transform.right, this.navigationRig.up);
				a2.Normalize();
				Vector3 vector3 = this.navigationRig.position;
				vector3 += a2 * (vector.x * Time.unscaledDeltaTime);
				this.navigationRig.position = vector3;
			}
			float num2 = 0f;
			if (Input.GetKey(KeyCode.Z))
			{
				num2 = num;
			}
			if (Input.GetKey(KeyCode.X))
			{
				num2 = -num;
			}
			if (num2 != 0f)
			{
				flag = true;
				this.playerHeightAdjust += num2 * 0.5f * Time.unscaledDeltaTime;
			}
			if (flag)
			{
				this.AdjustNavigationRigHeight();
			}
		}
	}

	// Token: 0x06005E69 RID: 24169 RVA: 0x0023A0B8 File Offset: 0x002384B8
	private void ProcessMouseFreeNavigation()
	{
		if (this.navigationRig != null && this.lookCamera != null && !this.navigationDisabled && !this.worldUIActivated && !Input.GetMouseButton(1) && !Input.GetMouseButton(2))
		{
			float num = this.GetMouseXChange();
			num = Mathf.Clamp(num, -10f, 10f);
			if (num > 0.01f || num < -0.01f)
			{
				if (this._mainHUDAnchoredOnMonitor && this.MonitorCenterCamera != null)
				{
					this.navigationRig.RotateAround(this.MonitorCenterCamera.transform.position, this.navigationRig.up, num);
				}
				else if (this.lookCamera != null)
				{
					this.navigationRig.RotateAround(this.lookCamera.transform.position, this.navigationRig.up, num);
				}
			}
			float num2 = this.GetMouseYChange();
			num2 = Mathf.Clamp(num2, -10f, 10f);
			if ((num2 > 0.01f || num2 < -0.01f) && this.MonitorCenterCamera != null)
			{
				Vector3 localEulerAngles = this.MonitorCenterCamera.transform.localEulerAngles;
				if (localEulerAngles.x > 180f)
				{
					localEulerAngles.x -= 360f;
				}
				if (localEulerAngles.x < -180f)
				{
					localEulerAngles.x += 360f;
				}
				localEulerAngles.x -= num2;
				localEulerAngles.x = Mathf.Clamp(localEulerAngles.x, -89f, 89f);
				localEulerAngles.y = 0f;
				localEulerAngles.z = 0f;
				this.MonitorCenterCamera.transform.localEulerAngles = localEulerAngles;
			}
		}
	}

	// Token: 0x17000DE2 RID: 3554
	// (get) Token: 0x06005E6A RID: 24170 RVA: 0x0023A2AE File Offset: 0x002386AE
	// (set) Token: 0x06005E6B RID: 24171 RVA: 0x0023A2B8 File Offset: 0x002386B8
	public int solverIterations
	{
		get
		{
			return this._solverIterations;
		}
		set
		{
			if (this._solverIterations != value)
			{
				this._solverIterations = value;
				foreach (Atom atom in this.atomsList)
				{
					foreach (Rigidbody rigidbody in atom.rigidbodies)
					{
						rigidbody.solverIterations = this._solverIterations;
					}
					foreach (PhysicsSimulator physicsSimulator in atom.physicsSimulators)
					{
						physicsSimulator.solverIterations = this._solverIterations;
					}
					foreach (PhysicsSimulatorJSONStorable physicsSimulatorJSONStorable in atom.physicsSimulatorsStorable)
					{
						physicsSimulatorJSONStorable.solverIterations = this._solverIterations;
					}
				}
			}
		}
	}

	// Token: 0x17000DE3 RID: 3555
	// (get) Token: 0x06005E6C RID: 24172 RVA: 0x0023A3BC File Offset: 0x002387BC
	// (set) Token: 0x06005E6D RID: 24173 RVA: 0x0023A3C4 File Offset: 0x002387C4
	private bool useInterpolation
	{
		get
		{
			return this._useInterpolation;
		}
		set
		{
			if (this._useInterpolation != value)
			{
				this._useInterpolation = value;
				foreach (Atom atom in this.atomsList)
				{
					atom.useRigidbodyInterpolation = this._useInterpolation;
				}
			}
		}
	}

	// Token: 0x06005E6E RID: 24174 RVA: 0x0023A438 File Offset: 0x00238838
	private void ProcessTimeScale()
	{
		if (this._isLoading)
		{
			this.useInterpolation = false;
			return;
		}
		if (Time.timeScale < 1f)
		{
			this.useInterpolation = true;
		}
		else
		{
			bool isPresent = XRDevice.isPresent;
			if (this.isMonitorOnly || !isPresent)
			{
				if (Time.fixedDeltaTime > 0.014f)
				{
					this.useInterpolation = true;
				}
				else
				{
					this.useInterpolation = false;
				}
			}
			else if (Time.fixedDeltaTime <= 0.0069f)
			{
				this.useInterpolation = false;
			}
			else
			{
				float refreshRate = XRDevice.refreshRate;
				if (refreshRate != 0f)
				{
					bool flag = false;
					if (refreshRate <= 59f)
					{
						flag = true;
					}
					else if (refreshRate > 59f && refreshRate < 61f && Time.fixedDeltaTime > 0.0166f && Time.fixedDeltaTime < 0.0167f)
					{
						flag = true;
					}
					else if (refreshRate > 71f && refreshRate < 73f && Time.fixedDeltaTime > 0.0138f && Time.fixedDeltaTime < 0.0139f)
					{
						flag = true;
					}
					else if (refreshRate > 79f && refreshRate < 81f && Time.fixedDeltaTime > 0.0124f && Time.fixedDeltaTime < 0.0126f)
					{
						flag = true;
					}
					else if (refreshRate > 89f && refreshRate < 91f && Time.fixedDeltaTime > 0.0111f && Time.fixedDeltaTime < 0.0112f)
					{
						flag = true;
					}
					else if (refreshRate > 119f && refreshRate < 121f && Time.fixedDeltaTime > 0.0083f && Time.fixedDeltaTime < 0.0084f)
					{
						flag = true;
					}
					else if (refreshRate > 143f && refreshRate < 145f && Time.fixedDeltaTime > 0.0069f && Time.fixedDeltaTime < 0.007f)
					{
						flag = true;
					}
					if (flag)
					{
						this.useInterpolation = false;
					}
					else
					{
						this.useInterpolation = true;
					}
				}
				else if (Time.fixedDeltaTime > 0.012f)
				{
					this.useInterpolation = true;
				}
				else
				{
					this.useInterpolation = false;
				}
			}
		}
	}

	// Token: 0x17000DE4 RID: 3556
	// (get) Token: 0x06005E6F RID: 24175 RVA: 0x0023A68E File Offset: 0x00238A8E
	protected bool leapDisabled
	{
		get
		{
			if (GlobalSceneOptions.singleton != null)
			{
				return GlobalSceneOptions.singleton.disableLeap;
			}
			return this.disableLeap;
		}
	}

	// Token: 0x06005E70 RID: 24176 RVA: 0x0023A6B4 File Offset: 0x00238AB4
	protected void SyncLeapEnabled()
	{
		if (this.LeapRig != null)
		{
			if (this.LeapServiceProviders != null)
			{
				foreach (LeapXRServiceProvider leapXRServiceProvider in this.LeapServiceProviders)
				{
					leapXRServiceProvider.enabled = (!this.leapDisabled && this._leapMotionEnabled);
				}
			}
			this.LeapRig.gameObject.SetActive(!this.leapDisabled && this._leapMotionEnabled);
		}
	}

	// Token: 0x17000DE5 RID: 3557
	// (get) Token: 0x06005E71 RID: 24177 RVA: 0x0023A73A File Offset: 0x00238B3A
	// (set) Token: 0x06005E72 RID: 24178 RVA: 0x0023A744 File Offset: 0x00238B44
	public bool leapMotionEnabled
	{
		get
		{
			return this._leapMotionEnabled;
		}
		set
		{
			if (this._leapMotionEnabled != value)
			{
				this._leapMotionEnabled = value;
				if (this.leapMotionEnabledToggle != null)
				{
					this.leapMotionEnabledToggle.isOn = value;
				}
				this.SyncLeapEnabled();
				if (UserPreferences.singleton != null)
				{
					UserPreferences.singleton.SavePreferences();
				}
			}
		}
	}

	// Token: 0x06005E73 RID: 24179 RVA: 0x0023A7A4 File Offset: 0x00238BA4
	protected void CheckAutoConnectLeapHands()
	{
		if (this.leapHandModelControl != null && this.leapHandMountLeft != null)
		{
			if (this._leapHandLeftConnected)
			{
				if (!this.leapMotionEnabled || !this.leapHandModelControl.leftHandEnabled)
				{
					this.DisconnectLeapHandLeft();
				}
			}
			else if (this.leapMotionEnabled && this.leapHandModelControl.leftHandEnabled && this.leapHandMountLeft.gameObject.activeInHierarchy)
			{
				this.ConnectLeapHandLeft();
			}
		}
		if (this.leapHandModelControl != null && this.leapHandMountRight != null)
		{
			if (this._leapHandRightConnected)
			{
				if (!this.leapMotionEnabled || !this.leapHandModelControl.rightHandEnabled)
				{
					this.DisconnectLeapHandRight();
				}
			}
			else if (this.leapMotionEnabled && this.leapHandModelControl.rightHandEnabled && this.leapHandMountRight.gameObject.activeInHierarchy)
			{
				this.ConnectLeapHandRight();
			}
		}
	}

	// Token: 0x06005E74 RID: 24180 RVA: 0x0023A8C4 File Offset: 0x00238CC4
	protected void ConnectLeapHandLeft()
	{
		if (this.leftHand != null && this.leapHandMountLeft != null)
		{
			this._leapHandLeftConnected = true;
			this.leftHand.transform.SetParent(this.leapHandMountLeft);
			this.leftHand.transform.localPosition = Vector3.zero;
			this.leftHand.transform.localRotation = Quaternion.identity;
		}
		this.SyncActiveHands();
		if (this.commonHandModelControl != null)
		{
			this.commonHandModelControl.ignorePositionRotationLeft = true;
		}
		if (this.ovrHandInputLeft != null)
		{
			this.ovrHandInputLeft.enabled = false;
		}
		if (this.steamVRHandInputLeft != null)
		{
			this.steamVRHandInputLeft.enabled = false;
		}
		if (this.handsContainer != null)
		{
			ConfigurableJointReconnector[] componentsInChildren = this.handsContainer.GetComponentsInChildren<ConfigurableJointReconnector>();
			foreach (ConfigurableJointReconnector configurableJointReconnector in componentsInChildren)
			{
				configurableJointReconnector.Reconnect();
			}
		}
	}

	// Token: 0x06005E75 RID: 24181 RVA: 0x0023A9D4 File Offset: 0x00238DD4
	protected void DisconnectLeapHandLeft()
	{
		this._leapHandLeftConnected = false;
		if (this.leftHand != null)
		{
			this.leftHand.transform.SetParent(this.handMountLeft);
			this.leftHand.transform.localPosition = Vector3.zero;
			this.leftHand.transform.localRotation = Quaternion.identity;
		}
		this.SyncActiveHands();
		if (this.commonHandModelControl != null)
		{
			this.commonHandModelControl.ignorePositionRotationLeft = false;
		}
		if (this.ovrHandInputLeft != null)
		{
			this.ovrHandInputLeft.enabled = true;
		}
		if (this.steamVRHandInputLeft != null)
		{
			this.steamVRHandInputLeft.enabled = true;
		}
		if (this.handsContainer != null)
		{
			ConfigurableJointReconnector[] componentsInChildren = this.handsContainer.GetComponentsInChildren<ConfigurableJointReconnector>();
			foreach (ConfigurableJointReconnector configurableJointReconnector in componentsInChildren)
			{
				configurableJointReconnector.Reconnect();
			}
		}
	}

	// Token: 0x06005E76 RID: 24182 RVA: 0x0023AAD4 File Offset: 0x00238ED4
	protected void ConnectLeapHandRight()
	{
		if (this.rightHand != null && this.leapHandMountRight != null)
		{
			this._leapHandRightConnected = true;
			this.rightHand.transform.SetParent(this.leapHandMountRight);
			this.rightHand.transform.localPosition = Vector3.zero;
			this.rightHand.transform.localRotation = Quaternion.identity;
		}
		this.SyncActiveHands();
		if (this.commonHandModelControl != null)
		{
			this.commonHandModelControl.ignorePositionRotationRight = true;
		}
		if (this.ovrHandInputRight != null)
		{
			this.ovrHandInputRight.enabled = false;
		}
		if (this.steamVRHandInputRight != null)
		{
			this.steamVRHandInputRight.enabled = false;
		}
		if (this.handsContainer != null)
		{
			ConfigurableJointReconnector[] componentsInChildren = this.handsContainer.GetComponentsInChildren<ConfigurableJointReconnector>();
			foreach (ConfigurableJointReconnector configurableJointReconnector in componentsInChildren)
			{
				configurableJointReconnector.Reconnect();
			}
		}
	}

	// Token: 0x06005E77 RID: 24183 RVA: 0x0023ABE4 File Offset: 0x00238FE4
	protected void DisconnectLeapHandRight()
	{
		this._leapHandRightConnected = false;
		if (this.rightHand != null)
		{
			this.rightHand.transform.SetParent(this.handMountRight);
			this.rightHand.transform.localPosition = Vector3.zero;
			this.rightHand.transform.localRotation = Quaternion.identity;
		}
		this.SyncActiveHands();
		if (this.commonHandModelControl != null)
		{
			this.commonHandModelControl.ignorePositionRotationRight = false;
		}
		if (this.ovrHandInputRight != null)
		{
			this.ovrHandInputRight.enabled = true;
		}
		if (this.steamVRHandInputRight != null)
		{
			this.steamVRHandInputRight.enabled = true;
		}
		if (this.handsContainer != null)
		{
			ConfigurableJointReconnector[] componentsInChildren = this.handsContainer.GetComponentsInChildren<ConfigurableJointReconnector>();
			foreach (ConfigurableJointReconnector configurableJointReconnector in componentsInChildren)
			{
				configurableJointReconnector.Reconnect();
			}
		}
	}

	// Token: 0x06005E78 RID: 24184 RVA: 0x0023ACE3 File Offset: 0x002390E3
	protected void SyncTrackerVisibility()
	{
		this.SyncTracker1Visibility();
		this.SyncTracker2Visibility();
		this.SyncTracker3Visibility();
		this.SyncTracker4Visibility();
		this.SyncTracker5Visibility();
		this.SyncTracker6Visibility();
		this.SyncTracker7Visibility();
		this.SyncTracker8Visibility();
	}

	// Token: 0x17000DE6 RID: 3558
	// (get) Token: 0x06005E79 RID: 24185 RVA: 0x0023AD15 File Offset: 0x00239115
	// (set) Token: 0x06005E7A RID: 24186 RVA: 0x0023AD1D File Offset: 0x0023911D
	public bool hideTrackers
	{
		get
		{
			return this._hideTrackers;
		}
		set
		{
			if (this._hideTrackers != value)
			{
				this._hideTrackers = value;
				this.SyncTrackerVisibility();
			}
		}
	}

	// Token: 0x17000DE7 RID: 3559
	// (get) Token: 0x06005E7B RID: 24187 RVA: 0x0023AD38 File Offset: 0x00239138
	// (set) Token: 0x06005E7C RID: 24188 RVA: 0x0023AD40 File Offset: 0x00239140
	protected bool tracker1Visible
	{
		get
		{
			return this._tracker1Visible;
		}
		set
		{
			if (this._tracker1Visible != value)
			{
				this._tracker1Visible = value;
				this.SyncTracker1Visibility();
			}
		}
	}

	// Token: 0x06005E7D RID: 24189 RVA: 0x0023AD5C File Offset: 0x0023915C
	public void SyncTracker1Visibility()
	{
		if (this.viveTracker1 != null && this.viveTracker1Model != null)
		{
			bool flag = this._tracker1Visible && !this._hideTrackers;
			this.viveTracker1Model.enabled = flag;
			IEnumerator enumerator = this.viveTracker1Model.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					transform.gameObject.SetActive(flag);
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
		}
	}

	// Token: 0x17000DE8 RID: 3560
	// (get) Token: 0x06005E7E RID: 24190 RVA: 0x0023AE14 File Offset: 0x00239214
	// (set) Token: 0x06005E7F RID: 24191 RVA: 0x0023AE1C File Offset: 0x0023921C
	protected bool tracker2Visible
	{
		get
		{
			return this._tracker2Visible;
		}
		set
		{
			if (this._tracker2Visible != value)
			{
				this._tracker2Visible = value;
				this.SyncTracker2Visibility();
			}
		}
	}

	// Token: 0x06005E80 RID: 24192 RVA: 0x0023AE38 File Offset: 0x00239238
	public void SyncTracker2Visibility()
	{
		if (this.viveTracker2 != null && this.viveTracker2Model != null)
		{
			bool flag = this._tracker2Visible && !this._hideTrackers;
			this.viveTracker2Model.enabled = flag;
			IEnumerator enumerator = this.viveTracker2Model.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					transform.gameObject.SetActive(flag);
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
		}
	}

	// Token: 0x17000DE9 RID: 3561
	// (get) Token: 0x06005E81 RID: 24193 RVA: 0x0023AEF0 File Offset: 0x002392F0
	// (set) Token: 0x06005E82 RID: 24194 RVA: 0x0023AEF8 File Offset: 0x002392F8
	protected bool tracker3Visible
	{
		get
		{
			return this._tracker3Visible;
		}
		set
		{
			if (this._tracker3Visible != value)
			{
				this._tracker3Visible = value;
				this.SyncTracker3Visibility();
			}
		}
	}

	// Token: 0x06005E83 RID: 24195 RVA: 0x0023AF14 File Offset: 0x00239314
	public void SyncTracker3Visibility()
	{
		if (this.viveTracker3 != null && this.viveTracker3Model != null)
		{
			bool flag = this._tracker3Visible && !this._hideTrackers;
			this.viveTracker3Model.enabled = flag;
			IEnumerator enumerator = this.viveTracker3Model.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					transform.gameObject.SetActive(flag);
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
		}
	}

	// Token: 0x17000DEA RID: 3562
	// (get) Token: 0x06005E84 RID: 24196 RVA: 0x0023AFCC File Offset: 0x002393CC
	// (set) Token: 0x06005E85 RID: 24197 RVA: 0x0023AFD4 File Offset: 0x002393D4
	protected bool tracker4Visible
	{
		get
		{
			return this._tracker4Visible;
		}
		set
		{
			if (this._tracker4Visible != value)
			{
				this._tracker4Visible = value;
				this.SyncTracker4Visibility();
			}
		}
	}

	// Token: 0x06005E86 RID: 24198 RVA: 0x0023AFF0 File Offset: 0x002393F0
	public void SyncTracker4Visibility()
	{
		if (this.viveTracker4 != null && this.viveTracker4Model != null)
		{
			bool flag = this._tracker4Visible && !this._hideTrackers;
			this.viveTracker4Model.enabled = flag;
			IEnumerator enumerator = this.viveTracker4Model.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					transform.gameObject.SetActive(flag);
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
		}
	}

	// Token: 0x17000DEB RID: 3563
	// (get) Token: 0x06005E87 RID: 24199 RVA: 0x0023B0A8 File Offset: 0x002394A8
	// (set) Token: 0x06005E88 RID: 24200 RVA: 0x0023B0B0 File Offset: 0x002394B0
	protected bool tracker5Visible
	{
		get
		{
			return this._tracker5Visible;
		}
		set
		{
			if (this._tracker5Visible != value)
			{
				this._tracker5Visible = value;
				this.SyncTracker5Visibility();
			}
		}
	}

	// Token: 0x06005E89 RID: 24201 RVA: 0x0023B0CC File Offset: 0x002394CC
	public void SyncTracker5Visibility()
	{
		if (this.viveTracker5 != null && this.viveTracker5Model != null)
		{
			bool flag = this._tracker5Visible && !this._hideTrackers;
			this.viveTracker5Model.enabled = flag;
			IEnumerator enumerator = this.viveTracker5Model.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					transform.gameObject.SetActive(flag);
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
		}
	}

	// Token: 0x17000DEC RID: 3564
	// (get) Token: 0x06005E8A RID: 24202 RVA: 0x0023B184 File Offset: 0x00239584
	// (set) Token: 0x06005E8B RID: 24203 RVA: 0x0023B18C File Offset: 0x0023958C
	protected bool tracker6Visible
	{
		get
		{
			return this._tracker6Visible;
		}
		set
		{
			if (this._tracker6Visible != value)
			{
				this._tracker6Visible = value;
				this.SyncTracker6Visibility();
			}
		}
	}

	// Token: 0x06005E8C RID: 24204 RVA: 0x0023B1A8 File Offset: 0x002395A8
	public void SyncTracker6Visibility()
	{
		if (this.viveTracker6 != null && this.viveTracker6Model != null)
		{
			bool flag = this._tracker6Visible && !this._hideTrackers;
			this.viveTracker6Model.enabled = flag;
			IEnumerator enumerator = this.viveTracker6Model.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					transform.gameObject.SetActive(flag);
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
		}
	}

	// Token: 0x17000DED RID: 3565
	// (get) Token: 0x06005E8D RID: 24205 RVA: 0x0023B260 File Offset: 0x00239660
	// (set) Token: 0x06005E8E RID: 24206 RVA: 0x0023B268 File Offset: 0x00239668
	protected bool tracker7Visible
	{
		get
		{
			return this._tracker7Visible;
		}
		set
		{
			if (this._tracker7Visible != value)
			{
				this._tracker7Visible = value;
				this.SyncTracker7Visibility();
			}
		}
	}

	// Token: 0x06005E8F RID: 24207 RVA: 0x0023B284 File Offset: 0x00239684
	public void SyncTracker7Visibility()
	{
		if (this.viveTracker7 != null && this.viveTracker7Model != null)
		{
			bool flag = this._tracker7Visible && !this._hideTrackers;
			this.viveTracker7Model.enabled = flag;
			IEnumerator enumerator = this.viveTracker7Model.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					transform.gameObject.SetActive(flag);
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
		}
	}

	// Token: 0x17000DEE RID: 3566
	// (get) Token: 0x06005E90 RID: 24208 RVA: 0x0023B33C File Offset: 0x0023973C
	// (set) Token: 0x06005E91 RID: 24209 RVA: 0x0023B344 File Offset: 0x00239744
	protected bool tracker8Visible
	{
		get
		{
			return this._tracker8Visible;
		}
		set
		{
			if (this._tracker8Visible != value)
			{
				this._tracker8Visible = value;
				this.SyncTracker8Visibility();
			}
		}
	}

	// Token: 0x06005E92 RID: 24210 RVA: 0x0023B360 File Offset: 0x00239760
	public void SyncTracker8Visibility()
	{
		if (this.viveTracker8 != null && this.viveTracker8Model != null)
		{
			bool flag = this._tracker8Visible && !this._hideTrackers;
			this.viveTracker8Model.enabled = flag;
			IEnumerator enumerator = this.viveTracker8Model.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					transform.gameObject.SetActive(flag);
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
		}
	}

	// Token: 0x06005E93 RID: 24211 RVA: 0x0023B418 File Offset: 0x00239818
	private void ConnectCenterCameraTarget(Transform parent, Vector3 offset, bool isMonitor)
	{
		if (this.centerCameraTarget != null)
		{
			this.centerCameraTarget.transform.SetParent(parent);
			this.centerCameraTarget.transform.localPosition = offset;
			this.centerCameraTarget.transform.localRotation = Quaternion.identity;
			this.centerCameraTarget.FindCamera();
			this.centerCameraTarget.isMonitorCamera = isMonitor;
			if (this.centerCameraTargetDisableWhenMonitor != null)
			{
				foreach (GameObject gameObject in this.centerCameraTargetDisableWhenMonitor)
				{
					gameObject.SetActive(!isMonitor);
				}
			}
		}
	}

	// Token: 0x06005E94 RID: 24212 RVA: 0x0023B4B8 File Offset: 0x002398B8
	protected void SyncGenerateDepthTexture()
	{
		if (this._generateDepthTexture)
		{
			if (this.MonitorCenterCamera != null)
			{
				this.MonitorCenterCamera.depthTextureMode |= DepthTextureMode.Depth;
			}
			if (this.OVRCenterCamera != null)
			{
				this.OVRCenterCamera.depthTextureMode |= DepthTextureMode.Depth;
			}
			if (this.ViveCenterCamera != null)
			{
				this.ViveCenterCamera.depthTextureMode |= DepthTextureMode.Depth;
			}
		}
	}

	// Token: 0x17000DEF RID: 3567
	// (get) Token: 0x06005E95 RID: 24213 RVA: 0x0023B53C File Offset: 0x0023993C
	// (set) Token: 0x06005E96 RID: 24214 RVA: 0x0023B544 File Offset: 0x00239944
	public bool generateDepthTexture
	{
		get
		{
			return this._generateDepthTexture;
		}
		set
		{
			if (this._generateDepthTexture != value)
			{
				this._generateDepthTexture = value;
				if (this.generateDepthTextureToggle != null)
				{
					this.generateDepthTextureToggle.isOn = value;
				}
				this.SyncGenerateDepthTexture();
				if (UserPreferences.singleton != null)
				{
					UserPreferences.singleton.SavePreferences();
				}
			}
		}
	}

	// Token: 0x17000DF0 RID: 3568
	// (get) Token: 0x06005E97 RID: 24215 RVA: 0x0023B5A1 File Offset: 0x002399A1
	// (set) Token: 0x06005E98 RID: 24216 RVA: 0x0023B5AC File Offset: 0x002399AC
	public bool useMonitorRigAudioListenerWhenActive
	{
		get
		{
			return this._useMonitorRigAudioListenerWhenActive;
		}
		set
		{
			if (this._useMonitorRigAudioListenerWhenActive != value)
			{
				this._useMonitorRigAudioListenerWhenActive = value;
				if (this.useMonitorRigAudioListenerWhenActiveToggle != null)
				{
					this.useMonitorRigAudioListenerWhenActiveToggle.isOn = value;
				}
				this.SyncAudioListener();
				if (UserPreferences.singleton != null)
				{
					UserPreferences.singleton.SavePreferences();
				}
			}
		}
	}

	// Token: 0x06005E99 RID: 24217 RVA: 0x0023B60C File Offset: 0x00239A0C
	protected void InitAudioListeners()
	{
		this.additionalAudioListeners = new LinkedList<AudioListener>();
		if (this.MonitorCenterCamera != null)
		{
			this.monitorRigAudioListener = this.MonitorCenterCamera.GetComponent<AudioListener>();
		}
		if (this.OVRCenterCamera != null)
		{
			this.ovrRigAudioListener = this.OVRCenterCamera.GetComponent<AudioListener>();
		}
		if (this.ViveCenterCamera != null)
		{
			this.openVRRigAudioListener = this.ViveCenterCamera.GetComponent<AudioListener>();
		}
		this.SyncAudioListener();
	}

	// Token: 0x17000DF1 RID: 3569
	// (get) Token: 0x06005E9A RID: 24218 RVA: 0x0023B690 File Offset: 0x00239A90
	public AudioListener CurrentAudioListener
	{
		get
		{
			return this.currentAudioListener;
		}
	}

	// Token: 0x06005E9B RID: 24219 RVA: 0x0023B698 File Offset: 0x00239A98
	protected void SyncAudioListener()
	{
		foreach (AudioListener audioListener in this.additionalAudioListeners)
		{
			audioListener.enabled = false;
		}
		if (this.monitorRigAudioListener != null)
		{
			this.monitorRigAudioListener.enabled = false;
		}
		if (this.ovrRigAudioListener != null)
		{
			this.ovrRigAudioListener.enabled = false;
		}
		if (this.openVRRigAudioListener != null)
		{
			this.openVRRigAudioListener.enabled = false;
		}
		if (this.overrideAudioListener != null)
		{
			this.currentAudioListener = this.overrideAudioListener;
			this.overrideAudioListener.enabled = true;
		}
		else if (this.isMonitorOnly || (this.MonitorRigActive && this._useMonitorRigAudioListenerWhenActive))
		{
			if (this.monitorRigAudioListener != null)
			{
				this.monitorRigAudioListener.enabled = true;
			}
			this.currentAudioListener = this.monitorRigAudioListener;
		}
		else if (this.isOVR)
		{
			if (this.ovrRigAudioListener != null)
			{
				this.ovrRigAudioListener.enabled = true;
			}
			this.currentAudioListener = this.ovrRigAudioListener;
		}
		else if (this.isOpenVR)
		{
			if (this.openVRRigAudioListener != null)
			{
				this.openVRRigAudioListener.enabled = true;
			}
			this.currentAudioListener = this.openVRRigAudioListener;
		}
	}

	// Token: 0x06005E9C RID: 24220 RVA: 0x0023B838 File Offset: 0x00239C38
	public void PushAudioListener(AudioListener audioListener)
	{
		if (!this.additionalAudioListeners.Contains(audioListener))
		{
			this.additionalAudioListeners.AddLast(audioListener);
			this.overrideAudioListener = this.additionalAudioListeners.Last.Value;
			this.SyncAudioListener();
		}
	}

	// Token: 0x06005E9D RID: 24221 RVA: 0x0023B874 File Offset: 0x00239C74
	public void RemoveAudioListener(AudioListener audioListener)
	{
		if (this.additionalAudioListeners.Contains(audioListener))
		{
			this.additionalAudioListeners.Remove(audioListener);
			audioListener.enabled = false;
			if (this.additionalAudioListeners.Count > 0)
			{
				this.overrideAudioListener = this.additionalAudioListeners.Last.Value;
			}
			else
			{
				this.overrideAudioListener = null;
			}
			this.SyncAudioListener();
		}
	}

	// Token: 0x17000DF2 RID: 3570
	// (get) Token: 0x06005E9E RID: 24222 RVA: 0x0023B8DF File Offset: 0x00239CDF
	public bool IsMonitorRigActive
	{
		get
		{
			return this.MonitorRigActive;
		}
	}

	// Token: 0x17000DF3 RID: 3571
	// (get) Token: 0x06005E9F RID: 24223 RVA: 0x0023B8E7 File Offset: 0x00239CE7
	public bool IsMonitorOnly
	{
		get
		{
			return this.isMonitorOnly;
		}
	}

	// Token: 0x06005EA0 RID: 24224 RVA: 0x0023B8F0 File Offset: 0x00239CF0
	private void SyncMonitorAuxUI()
	{
		if (this.MonitorModeAuxUI != null)
		{
			this.MonitorModeAuxUI.gameObject.SetActive(this.MonitorRigActive && !this.UIDisabled && !this.worldUIActivated);
		}
	}

	// Token: 0x06005EA1 RID: 24225 RVA: 0x0023B940 File Offset: 0x00239D40
	public void ToggleMonitorUI()
	{
		if (this.MonitorUI != null && !this.UIDisabled)
		{
			if (this.MonitorUI.gameObject.activeSelf)
			{
				this._toggleMonitorSaveMainHUDVisible = this._mainHUDVisible;
				this.MonitorUI.gameObject.SetActive(false);
				this._monitorUIVisible = false;
				this.HideMainHUD();
			}
			else
			{
				this.MonitorUI.gameObject.SetActive(true);
				this._monitorUIVisible = true;
				if (this._toggleMonitorSaveMainHUDVisible)
				{
					this.ShowMainHUDMonitor();
				}
			}
		}
	}

	// Token: 0x06005EA2 RID: 24226 RVA: 0x0023B9D8 File Offset: 0x00239DD8
	public void HideMonitorUI()
	{
		if (this.MonitorUI != null && !this.UIDisabled && this.MonitorUI.gameObject.activeSelf)
		{
			this._toggleMonitorSaveMainHUDVisible = this._mainHUDVisible;
			this._monitorUIVisible = false;
			this.MonitorUI.gameObject.SetActive(false);
			this.HideMainHUD();
		}
	}

	// Token: 0x06005EA3 RID: 24227 RVA: 0x0023BA40 File Offset: 0x00239E40
	public void ShowMonitorUI()
	{
		if (this.MonitorUI != null && !this.UIDisabled && !this.MonitorUI.gameObject.activeSelf)
		{
			this.MonitorUI.gameObject.SetActive(true);
			this._monitorUIVisible = true;
			if (this._toggleMonitorSaveMainHUDVisible)
			{
				this.ShowMainHUDMonitor();
			}
		}
	}

	// Token: 0x17000DF4 RID: 3572
	// (get) Token: 0x06005EA4 RID: 24228 RVA: 0x0023BAA7 File Offset: 0x00239EA7
	// (set) Token: 0x06005EA5 RID: 24229 RVA: 0x0023BAB0 File Offset: 0x00239EB0
	public float monitorUIScale
	{
		get
		{
			return this._monitorUIScale;
		}
		set
		{
			if (this._monitorUIScale != value)
			{
				this._monitorUIScale = value;
				if (UserPreferences.singleton != null)
				{
					UserPreferences.singleton.SavePreferences();
				}
				if (this.monitorUIScaleSlider != null)
				{
					this.monitorUIScaleSlider.value = this._monitorUIScale;
				}
			}
		}
	}

	// Token: 0x17000DF5 RID: 3573
	// (get) Token: 0x06005EA6 RID: 24230 RVA: 0x0023BB0C File Offset: 0x00239F0C
	// (set) Token: 0x06005EA7 RID: 24231 RVA: 0x0023BB14 File Offset: 0x00239F14
	public float monitorUIYOffset
	{
		get
		{
			return this._monitorUIYOffset;
		}
		set
		{
			if (this._monitorUIYOffset != value)
			{
				this._monitorUIYOffset = value;
				if (UserPreferences.singleton != null)
				{
					UserPreferences.singleton.SavePreferences();
				}
				if (this.monitorUIYOffsetSlider != null)
				{
					this.monitorUIYOffsetSlider.value = this._monitorUIYOffset;
				}
			}
		}
	}

	// Token: 0x17000DF6 RID: 3574
	// (get) Token: 0x06005EA8 RID: 24232 RVA: 0x0023BB70 File Offset: 0x00239F70
	public float startingMonitorCameraFOV
	{
		get
		{
			if (GlobalSceneOptions.singleton != null)
			{
				return GlobalSceneOptions.singleton.startingMonitorCameraFOV;
			}
			return this._monitorCameraFOV;
		}
	}

	// Token: 0x06005EA9 RID: 24233 RVA: 0x0023BB94 File Offset: 0x00239F94
	private void SyncMonitorCameraFOV()
	{
		if (this.MonitorCenterCamera != null)
		{
			if (this.worldUIActivated)
			{
				this.MonitorCenterCamera.fieldOfView = 40f;
			}
			else
			{
				this.MonitorCenterCamera.fieldOfView = this._monitorCameraFOV;
			}
		}
	}

	// Token: 0x17000DF7 RID: 3575
	// (get) Token: 0x06005EAA RID: 24234 RVA: 0x0023BBE3 File Offset: 0x00239FE3
	// (set) Token: 0x06005EAB RID: 24235 RVA: 0x0023BBFC File Offset: 0x00239FFC
	public float monitorCameraFOV
	{
		get
		{
			if (this.worldUIActivated)
			{
				return 40f;
			}
			return this._monitorCameraFOV;
		}
		set
		{
			if (this._monitorCameraFOV != value)
			{
				this._monitorCameraFOV = value;
				if (this.monitorCameraFOVSlider != null)
				{
					this.monitorCameraFOVSlider.value = this._monitorCameraFOV;
				}
				this.SyncMonitorCameraFOV();
			}
		}
	}

	// Token: 0x06005EAC RID: 24236 RVA: 0x0023BC3C File Offset: 0x0023A03C
	public void ToggleMainMonitor()
	{
		if (this.MonitorRig != null)
		{
			if (this.MonitorRigActive)
			{
				this.MonitorRigActive = false;
				this.MonitorRig.gameObject.SetActive(false);
				this.SyncAudioListener();
				this.SyncMonitorAuxUI();
				this.SyncWorldUIAnchor();
				if (this.centerCameraTarget != null && !this.isMonitorOnly && this.saveCenterEyeAttachPoint != null)
				{
					this.ConnectCenterCameraTarget(this.saveCenterEyeAttachPoint, Vector3.zero, false);
				}
			}
			else
			{
				if (UserPreferences.singleton != null)
				{
					UserPreferences.singleton.overlayUI = true;
				}
				if (this.commonHandModelControl != null)
				{
					this.commonHandModelControl.useCollision = false;
				}
				if (this.alternateControllerHandModelControl != null)
				{
					this.alternateControllerHandModelControl.useCollision = false;
				}
				this.MonitorRigActive = true;
				this.MonitorRig.gameObject.SetActive(true);
				this.SyncAudioListener();
				this.SyncMonitorAuxUI();
				this.SyncWorldUIAnchor();
				if (this.MonitorUIAttachPoint != null)
				{
					this.MoveMainHUD(this.MonitorUIAttachPoint);
				}
				if (this.centerCameraTarget != null && !this.isMonitorOnly)
				{
					this.saveCenterEyeAttachPoint = this.centerCameraTarget.transform.parent;
					this.ConnectCenterCameraTarget(this.MonitorCenterCamera.transform, this.MonitorCenterCameraOffset, true);
				}
				if (this._mainHUDVisible)
				{
					this.HideMainHUD();
					this.ShowMainHUDAuto();
				}
				else
				{
					this.ShowMainHUDAuto();
					this.HideMainHUD();
				}
			}
			this.SyncUISide();
		}
	}

	// Token: 0x06005EAD RID: 24237 RVA: 0x0023BDE8 File Offset: 0x0023A1E8
	protected void SetMonitorRig()
	{
		this.isOVR = false;
		this.isOpenVR = false;
		this.isMonitorOnly = true;
		if (this.OVRRig != null)
		{
			this.OVRRig.gameObject.SetActive(false);
		}
		if (this.ViveRig != null)
		{
			this.ViveRig.gameObject.SetActive(false);
		}
		if (this.MonitorRig != null)
		{
			this.MonitorRigActive = true;
			this.MonitorRig.gameObject.SetActive(true);
		}
		this.SyncWorldUIAnchor();
		if (this.MonitorModeButton != null)
		{
			this.MonitorModeButton.gameObject.SetActive(false);
		}
		if (this.MonitorCenterCamera != null && this.centerCameraTarget != null)
		{
			this.ConnectCenterCameraTarget(this.MonitorCenterCamera.transform, this.MonitorCenterCameraOffset, true);
		}
		if (this.MonitorUIAttachPoint != null)
		{
			this.MoveMainHUD(this.MonitorUIAttachPoint);
			this.mainHUDAttachPointStartingPosition = this.mainHUDAttachPoint.localPosition;
			this.mainHUDAttachPointStartingRotation = this.mainHUDAttachPoint.localRotation;
		}
		if (this.MonitorUI != null)
		{
			if (this.UIDisabled)
			{
				this.MonitorUI.gameObject.SetActive(false);
			}
			else
			{
				this.MonitorUI.gameObject.SetActive(true);
			}
		}
		this.SyncActiveHands();
	}

	// Token: 0x06005EAE RID: 24238 RVA: 0x0023BF64 File Offset: 0x0023A364
	public void SetOculusThumbstickFunctionFromString(string str)
	{
		if (str != null)
		{
			if (str == "GrabWorld")
			{
				this.oculusThumbstickFunction = SuperController.ThumbstickFunction.GrabWorld;
				return;
			}
			if (str == "SwapAxis")
			{
				this.oculusThumbstickFunction = SuperController.ThumbstickFunction.SwapAxis;
				return;
			}
			if (str == "Both")
			{
				this.oculusThumbstickFunction = SuperController.ThumbstickFunction.Both;
				return;
			}
		}
		UnityEngine.Debug.LogWarning("Tried to set oculusThumbstickFunction to " + str + " which is not a valid type");
	}

	// Token: 0x17000DF8 RID: 3576
	// (get) Token: 0x06005EAF RID: 24239 RVA: 0x0023BFEA File Offset: 0x0023A3EA
	// (set) Token: 0x06005EB0 RID: 24240 RVA: 0x0023BFF4 File Offset: 0x0023A3F4
	public SuperController.ThumbstickFunction oculusThumbstickFunction
	{
		get
		{
			return this._oculusThumbstickFunction;
		}
		set
		{
			if (this._oculusThumbstickFunction != value)
			{
				this._oculusThumbstickFunction = value;
				if (this.oculusThumbstickFunctionPopup != null)
				{
					this.oculusThumbstickFunctionPopup.currentValue = this._oculusThumbstickFunction.ToString();
				}
				if (UserPreferences.singleton != null)
				{
					UserPreferences.singleton.SavePreferences();
				}
			}
		}
	}

	// Token: 0x06005EB1 RID: 24241 RVA: 0x0023C05C File Offset: 0x0023A45C
	protected void SetOculusRig()
	{
		this.isOVR = true;
		this.isOpenVR = false;
		this.isMonitorOnly = false;
		if (this.OVRRig != null)
		{
			this.OVRRig.gameObject.SetActive(true);
		}
		if (this.ViveRig != null)
		{
			this.ViveRig.gameObject.SetActive(false);
		}
		if (this.MonitorRig != null)
		{
			this.MonitorRigActive = false;
			this.MonitorRig.gameObject.SetActive(false);
		}
		this.SyncWorldUIAnchor();
		if (this.MonitorModeButton != null)
		{
			this.MonitorModeButton.gameObject.SetActive(true);
		}
		if (this.MonitorUI != null)
		{
			if (this.UIDisabled)
			{
				this.MonitorUI.gameObject.SetActive(false);
			}
			else
			{
				this.MonitorUI.gameObject.SetActive(true);
			}
		}
		if (this.OVRCenterCamera != null && this.centerCameraTarget != null)
		{
			this.ConnectCenterCameraTarget(this.OVRCenterCamera.transform, Vector3.zero, false);
		}
		if (this.touchObjectLeft != null)
		{
			this.touchObjectLeftMeshRenderers = this.touchObjectLeft.GetComponentsInChildren<MeshRenderer>(true);
			Camera component = this.touchObjectLeft.GetComponent<Camera>();
			if (component != null)
			{
				this.leftControllerCamera = component;
			}
		}
		if (this.touchObjectRight != null)
		{
			this.touchObjectRightMeshRenderers = this.touchObjectRight.GetComponentsInChildren<MeshRenderer>(true);
			Camera component2 = this.touchObjectRight.GetComponent<Camera>();
			if (component2 != null)
			{
				this.rightControllerCamera = component2;
			}
		}
		if (this.leftHand != null)
		{
			this.leftHand.transform.SetParent(this.handMountLeft);
			this.leftHand.transform.localPosition = Vector3.zero;
			this.leftHand.transform.localRotation = Quaternion.identity;
		}
		if (this.leftHandAlternate != null)
		{
			this.leftHandAlternate.transform.SetParent(this.handMountLeft);
			this.leftHandAlternate.transform.localPosition = Vector3.zero;
			this.leftHandAlternate.transform.localRotation = Quaternion.identity;
		}
		if (this.rightHand != null)
		{
			this.rightHand.transform.SetParent(this.handMountRight);
			this.rightHand.transform.localPosition = Vector3.zero;
			this.rightHand.transform.localRotation = Quaternion.identity;
		}
		if (this.rightHandAlternate != null)
		{
			this.rightHandAlternate.transform.SetParent(this.handMountRight);
			this.rightHandAlternate.transform.localPosition = Vector3.zero;
			this.rightHandAlternate.transform.localRotation = Quaternion.identity;
		}
		this.SyncActiveHands();
	}

	// Token: 0x06005EB2 RID: 24242 RVA: 0x0023C360 File Offset: 0x0023A760
	protected void SetOpenVRRig()
	{
		this.isOVR = false;
		this.isOpenVR = true;
		this.isMonitorOnly = false;
		if (this.OVRRig != null)
		{
			this.OVRRig.gameObject.SetActive(false);
		}
		if (this.ViveRig != null)
		{
			this.ViveRig.gameObject.SetActive(true);
		}
		if (this.MonitorRig != null)
		{
			this.MonitorRigActive = false;
			this.MonitorRig.gameObject.SetActive(false);
		}
		this.SyncWorldUIAnchor();
		if (this.MonitorModeButton != null)
		{
			this.MonitorModeButton.gameObject.SetActive(true);
		}
		if (this.MonitorUI != null)
		{
			if (this.UIDisabled)
			{
				this.MonitorUI.gameObject.SetActive(false);
			}
			else
			{
				this.MonitorUI.gameObject.SetActive(true);
			}
		}
		if (this.ViveCenterCamera != null && this.centerCameraTarget != null)
		{
			this.ConnectCenterCameraTarget(this.ViveCenterCamera.transform, Vector3.zero, false);
		}
		if (this.viveObjectLeft != null)
		{
			Camera component = this.viveObjectLeft.GetComponent<Camera>();
			if (component != null)
			{
				this.leftControllerCamera = component;
			}
			else
			{
				this.Error("Could not find camera on left controller", true, true);
			}
		}
		if (this.viveObjectRight != null)
		{
			Camera component2 = this.viveObjectRight.GetComponent<Camera>();
			if (component2 != null)
			{
				this.rightControllerCamera = component2;
			}
			else
			{
				this.Error("Could not find camera on right controller", true, true);
			}
		}
		if (this.leftHand != null)
		{
			this.leftHand.transform.SetParent(this.handMountLeft);
			this.leftHand.transform.localPosition = Vector3.zero;
			this.leftHand.transform.localRotation = Quaternion.identity;
		}
		if (this.leftHandAlternate != null)
		{
			this.leftHandAlternate.transform.SetParent(this.handMountLeft);
			this.leftHandAlternate.transform.localPosition = Vector3.zero;
			this.leftHandAlternate.transform.localRotation = Quaternion.identity;
		}
		if (this.rightHand != null)
		{
			this.rightHand.transform.SetParent(this.handMountRight);
			this.rightHand.transform.localPosition = Vector3.zero;
			this.rightHand.transform.localRotation = Quaternion.identity;
		}
		if (this.rightHandAlternate != null)
		{
			this.rightHandAlternate.transform.SetParent(this.handMountRight);
			this.rightHandAlternate.transform.localPosition = Vector3.zero;
			this.rightHandAlternate.transform.localRotation = Quaternion.identity;
		}
		this.SyncActiveHands();
		if (!Application.isEditor)
		{
			string text = Application.dataPath;
			int num = text.LastIndexOf('/');
			text = text.Remove(num, text.Length - num);
			string pchApplicationManifestFullPath = Path.Combine(text, "vrmanifest");
			EVRApplicationError evrapplicationError = OpenVR.Applications.AddApplicationManifest(pchApplicationManifestFullPath, true);
			if (evrapplicationError != EVRApplicationError.None)
			{
				UnityEngine.Debug.LogError("<b>[SteamVR]</b> Error adding vr manifest file: " + evrapplicationError.ToString());
			}
			else
			{
				UnityEngine.Debug.Log("<b>[SteamVR]</b> Successfully added VR manifest to SteamVR");
			}
			int id = Process.GetCurrentProcess().Id;
			EVRApplicationError evrapplicationError2 = OpenVR.Applications.IdentifyApplication((uint)id, SteamVR_Settings.instance.editorAppKey);
			if (evrapplicationError2 != EVRApplicationError.None)
			{
				UnityEngine.Debug.LogError("<b>[SteamVR]</b> Error identifying application: " + evrapplicationError2.ToString());
			}
			else
			{
				UnityEngine.Debug.Log(string.Format("<b>[SteamVR]</b> Successfully identified process as project to SteamVR ({0})", SteamVR_Settings.instance.editorAppKey));
			}
		}
	}

	// Token: 0x06005EB3 RID: 24243 RVA: 0x0023C73D File Offset: 0x0023AB3D
	public void OpenSteamVRBindingsInBrowser()
	{
		Process.Start("http://localhost:8998/dashboard/controllerbinding.html?app=" + SteamVR_Settings.instance.editorAppKey);
	}

	// Token: 0x06005EB4 RID: 24244 RVA: 0x0023C75C File Offset: 0x0023AB5C
	protected void DetermineVRRig()
	{
		Application.targetFrameRate = 300;
		if (this.VRDisabled)
		{
			this.SetMonitorRig();
		}
		else
		{
			string loadedDeviceName = XRSettings.loadedDeviceName;
			UnityEngine.Debug.Log("XR device active is " + XRSettings.isDeviceActive);
			UnityEngine.Debug.Log("XR device present is " + XRDevice.isPresent);
			UnityEngine.Debug.Log("Loaded XR device is " + loadedDeviceName);
			UnityEngine.Debug.Log("XR device model is " + XRDevice.model);
			UnityEngine.Debug.Log("XR device refresh rate is " + XRDevice.refreshRate);
			if (!XRSettings.isDeviceActive || loadedDeviceName == null || loadedDeviceName == string.Empty)
			{
				this.SetMonitorRig();
			}
			else if (loadedDeviceName == "Oculus")
			{
				this.SetOculusRig();
			}
			else
			{
				this.SetOpenVRRig();
			}
		}
	}

	// Token: 0x06005EB5 RID: 24245 RVA: 0x0023C84C File Offset: 0x0023AC4C
	public string GetMD5Hash(byte[] bytes)
	{
		if (this.md5 == null)
		{
			this.md5 = new MD5CryptoServiceProvider();
		}
		byte[] array = this.md5.ComputeHash(bytes);
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < array.Length; i++)
		{
			stringBuilder.Append(array[i].ToString("x2"));
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06005EB6 RID: 24246 RVA: 0x0023C8B4 File Offset: 0x0023ACB4
	protected void DestroyScriptController(MVRScriptController mvrsc)
	{
		if (mvrsc.script != null)
		{
			try
			{
				UnityEngine.Object.DestroyImmediate(mvrsc.script);
			}
			catch (Exception)
			{
			}
			mvrsc.script = null;
		}
		if (mvrsc.gameObject != null)
		{
			UnityEngine.Object.Destroy(mvrsc.gameObject);
			mvrsc.gameObject = null;
		}
	}

	// Token: 0x06005EB7 RID: 24247 RVA: 0x0023C924 File Offset: 0x0023AD24
	protected MVRScriptController CreateScriptController(string scriptUid, string url, ScriptType type)
	{
		MVRScriptController mvrscriptController = new MVRScriptController();
		GameObject gameObject = new GameObject(scriptUid + "temp");
		gameObject.transform.SetParent(this.bootstrapPluginContainer);
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localScale = Vector3.one;
		mvrscriptController.gameObject = gameObject;
		ScriptProxy scriptProxy = type.CreateInstance(gameObject);
		if (scriptProxy == null)
		{
			SuperController.LogError("Failed to create instance of " + url);
			UnityEngine.Object.Destroy(gameObject);
			return null;
		}
		string name = scriptUid + "_" + scriptProxy.GetInstanceType().ToString();
		gameObject.name = name;
		MVRScript component = gameObject.GetComponent<MVRScript>();
		component.exclude = true;
		mvrscriptController.script = component;
		if (component.ShouldIgnore())
		{
			try
			{
				this.DestroyScriptController(mvrscriptController);
			}
			catch
			{
			}
			mvrscriptController = null;
		}
		else
		{
			try
			{
				component.ForceAwake();
				component.containingAtom = null;
				component.manager = null;
				try
				{
					component.Init();
				}
				catch (Exception arg)
				{
					SuperController.LogError("Exception during plugin script Init: " + arg);
				}
			}
			catch (Exception arg2)
			{
				SuperController.LogError("Exception during plugin script Awake: " + arg2);
				this.DestroyScriptController(mvrscriptController);
				mvrscriptController = null;
			}
		}
		return mvrscriptController;
	}

	// Token: 0x06005EB8 RID: 24248 RVA: 0x0023CA9C File Offset: 0x0023AE9C
	protected void LoadBootstrapPlugin(string url)
	{
		if (this.bootstrapPluginDomain == null)
		{
			this.bootstrapPluginDomain = ScriptDomain.CreateDomain("MVRBootstrapPlugins", true);
			IEnumerable<string> enumerable = this.GetResolvedVersionDefines();
			if (enumerable != null)
			{
				foreach (string symbol in enumerable)
				{
					this.bootstrapPluginDomain.CompilerService.AddConditionalSymbol(symbol);
				}
			}
		}
		if (this.bootstrapPluginScriptControllers == null)
		{
			this.bootstrapPluginScriptControllers = new Dictionary<string, List<MVRScriptController>>();
		}
		try
		{
			Location.Reset();
			string text = url.Replace("/", "_");
			text = text.Replace("\\", "_");
			text = text.Replace(".", "_");
			text = text.Replace(":", "_");
			if (url.EndsWith(".cslist") || url.EndsWith(".dll"))
			{
				ScriptAssembly scriptAssembly = null;
				if (url.EndsWith(".cslist"))
				{
					string directoryName = FileManager.GetDirectoryName(url, false);
					List<string> list = new List<string>();
					bool flag = false;
					using (FileEntryStreamReader fileEntryStreamReader = FileManager.OpenStreamReader(url, true))
					{
						StreamReader streamReader = fileEntryStreamReader.StreamReader;
						string text2;
						while ((text2 = streamReader.ReadLine()) != null)
						{
							string text3 = text2.Trim();
							if (directoryName != string.Empty && !text3.Contains(":/"))
							{
								text3 = directoryName + "/" + text3;
							}
							if (text3 != string.Empty)
							{
								text3 = text3.Replace('/', '\\');
								if (FileManager.IsFileInPackage(text3))
								{
									VarFileEntry varFileEntry = FileManager.GetVarFileEntry(text3);
									if (varFileEntry != null)
									{
										flag = true;
										list.Add(text3);
									}
								}
								else
								{
									list.Add(text3);
								}
							}
						}
					}
					if (list.Count <= 0)
					{
						return;
					}
					try
					{
						if (flag)
						{
							List<string> list2 = new List<string>();
							StringBuilder stringBuilder = new StringBuilder();
							foreach (string path in list)
							{
								string text4 = FileManager.ReadAllText(path, true);
								stringBuilder.Append(text4);
								list2.Add(text4);
							}
							string suggestedAssemblyNamePrefix = "MVRBootstrapPlugin_" + text + "_" + this.GetMD5Hash(Encoding.ASCII.GetBytes(stringBuilder.ToString()));
							this.bootstrapPluginDomain.CompilerService.SetSuggestedAssemblyNamePrefix(suggestedAssemblyNamePrefix);
							scriptAssembly = this.bootstrapPluginDomain.CompileAndLoadScriptSources(list2.ToArray());
						}
						else
						{
							StringBuilder stringBuilder2 = new StringBuilder();
							foreach (string path2 in list)
							{
								string value = FileManager.ReadAllText(path2, true);
								stringBuilder2.Append(value);
							}
							string suggestedAssemblyNamePrefix2 = "MVRBootstrapPlugin_" + text + "_" + this.GetMD5Hash(Encoding.ASCII.GetBytes(stringBuilder2.ToString()));
							this.bootstrapPluginDomain.CompilerService.SetSuggestedAssemblyNamePrefix(suggestedAssemblyNamePrefix2);
							scriptAssembly = this.bootstrapPluginDomain.CompileAndLoadScriptFiles(list.ToArray());
						}
						if (scriptAssembly == null)
						{
							SuperController.LogError("Compile of " + url + " failed. Errors:");
							foreach (string text5 in this.bootstrapPluginDomain.CompilerService.Errors)
							{
								if (!text5.StartsWith("[CS]"))
								{
									SuperController.LogError(text5 + "\n");
								}
							}
							return;
						}
					}
					catch (Exception ex)
					{
						SuperController.LogError(string.Concat(new object[]
						{
							"Compile of ",
							url,
							" failed. Exception: ",
							ex
						}));
						SuperController.LogError("Compile of " + url + " failed. Errors:");
						foreach (string err in this.bootstrapPluginDomain.CompilerService.Errors)
						{
							SuperController.LogError(err);
						}
						return;
					}
				}
				else if (FileManager.IsFileInPackage(url))
				{
					VarFileEntry varFileEntry2 = FileManager.GetVarFileEntry(url);
					if (varFileEntry2 != null)
					{
						VarPackage package = varFileEntry2.Package;
						byte[] array = FileManager.ReadAllBytes(url, true);
						string text6 = "MVRBootstrapPlugin_" + text + "_" + this.GetMD5Hash(array);
						FileManager.RegisterPluginHashToPluginPath(text6, url);
						this.bootstrapPluginDomain.CompilerService.SetSuggestedAssemblyNamePrefix(text6);
						scriptAssembly = this.bootstrapPluginDomain.LoadAssembly(array, null);
					}
				}
				else
				{
					byte[] bytes = FileManager.ReadAllBytes(url, true);
					string text7 = "MVRBootstrapPlugin_" + text + "_" + this.GetMD5Hash(bytes);
					FileManager.RegisterPluginHashToPluginPath(text7, url);
					this.bootstrapPluginDomain.CompilerService.SetSuggestedAssemblyNamePrefix(text7);
					scriptAssembly = this.bootstrapPluginDomain.LoadAssembly(url);
				}
				if (scriptAssembly != null)
				{
					ScriptType[] array2 = scriptAssembly.FindAllSubtypesOf<MVRScript>();
					if (array2.Length > 0)
					{
						List<MVRScriptController> list3 = new List<MVRScriptController>();
						foreach (ScriptType type in array2)
						{
							MVRScriptController mvrscriptController = this.CreateScriptController(text, url, type);
							if (mvrscriptController != null)
							{
								list3.Add(mvrscriptController);
							}
						}
						this.bootstrapPluginScriptControllers.Add(url, list3);
					}
					else
					{
						SuperController.LogError("No MVRScript types found");
					}
				}
				else
				{
					SuperController.LogError("Unable to load assembly from " + url);
				}
			}
			else
			{
				ScriptType scriptType = null;
				try
				{
					if (FileManager.IsFileInPackage(url))
					{
						VarFileEntry varFileEntry3 = FileManager.GetVarFileEntry(url);
						if (varFileEntry3 != null)
						{
							byte[] bytes2 = FileManager.ReadAllBytes(url, true);
							string suggestedAssemblyNamePrefix3 = "MVRBootstrapPlugin_" + text + "_" + this.GetMD5Hash(bytes2);
							this.bootstrapPluginDomain.CompilerService.SetSuggestedAssemblyNamePrefix(suggestedAssemblyNamePrefix3);
							scriptType = this.bootstrapPluginDomain.CompileAndLoadScriptSource(FileManager.ReadAllText(url, false));
						}
					}
					else
					{
						byte[] bytes3 = FileManager.ReadAllBytes(url, true);
						string suggestedAssemblyNamePrefix4 = "MVRBootstrapPlugin_" + text + "_" + this.GetMD5Hash(bytes3);
						this.bootstrapPluginDomain.CompilerService.SetSuggestedAssemblyNamePrefix(suggestedAssemblyNamePrefix4);
						scriptType = this.bootstrapPluginDomain.CompileAndLoadScriptFile(url);
					}
					if (scriptType == null)
					{
						SuperController.LogError("Compile of " + url + " failed. Errors:");
						foreach (string err2 in this.bootstrapPluginDomain.CompilerService.Errors)
						{
							SuperController.LogError(err2);
						}
						return;
					}
				}
				catch (Exception ex2)
				{
					SuperController.LogError(string.Concat(new object[]
					{
						"Compile of ",
						url,
						" failed. Exception: ",
						ex2
					}));
					SuperController.LogError("Compile of " + url + " failed. Errors:");
					foreach (string err3 in this.bootstrapPluginDomain.CompilerService.Errors)
					{
						SuperController.LogError(err3);
					}
					return;
				}
				if (scriptType.IsSubtypeOf<MVRScript>())
				{
					MVRScriptController mvrscriptController2 = this.CreateScriptController(text, url, scriptType);
					if (mvrscriptController2 != null)
					{
						List<MVRScriptController> list4 = new List<MVRScriptController>();
						list4.Add(mvrscriptController2);
						this.bootstrapPluginScriptControllers.Add(url, list4);
					}
				}
				else
				{
					SuperController.LogError("Script loaded at " + url + " must inherit from MVRScript");
				}
			}
		}
		catch (Exception ex3)
		{
			SuperController.LogError(string.Concat(new object[]
			{
				"Exception during compile of ",
				url,
				": ",
				ex3
			}));
		}
	}

	// Token: 0x06005EB9 RID: 24249 RVA: 0x0023D30C File Offset: 0x0023B70C
	protected void UnloadBootstrapPlugin(string url)
	{
		List<MVRScriptController> list;
		if (this.bootstrapPluginScriptControllers != null && this.bootstrapPluginScriptControllers.TryGetValue(url, out list))
		{
			foreach (MVRScriptController mvrsc in list)
			{
				this.DestroyScriptController(mvrsc);
			}
			this.bootstrapPluginScriptControllers.Remove(url);
		}
	}

	// Token: 0x06005EBA RID: 24250 RVA: 0x0023D390 File Offset: 0x0023B790
	protected void UnloadAllBootstrapPlugins()
	{
		if (this.bootstrapPluginScriptControllers != null)
		{
			foreach (KeyValuePair<string, List<MVRScriptController>> keyValuePair in this.bootstrapPluginScriptControllers)
			{
				foreach (MVRScriptController mvrsc in keyValuePair.Value)
				{
					this.DestroyScriptController(mvrsc);
				}
			}
		}
	}

	// Token: 0x17000DF9 RID: 3577
	// (get) Token: 0x06005EBB RID: 24251 RVA: 0x0023D43C File Offset: 0x0023B83C
	public bool vamXIntalled
	{
		get
		{
			return this._vamXInstalled;
		}
	}

	// Token: 0x06005EBC RID: 24252 RVA: 0x0023D444 File Offset: 0x0023B844
	protected void SyncVamX()
	{
		this.vamXVersion = 0;
		string text = this.NormalizeLoadPath(this.vamXBootstrapPluginPath);
		UnityEngine.Debug.Log("Check for vamX at " + text);
		if (FileManager.FileExists(text, false, false))
		{
			VarPackage package = FileManager.GetPackage("vamX.1.latest");
			if (package != null)
			{
				this.vamXVersion = package.Version;
			}
			else
			{
				this.vamXVersion = 0;
			}
			UnityEngine.Debug.Log("vamX was detected");
			this._vamXInstalled = true;
		}
		else
		{
			UnityEngine.Debug.Log("vamX was not detected");
			this._vamXInstalled = false;
		}
		if (this.vamXPanel != null)
		{
			this.vamXPanel.gameObject.SetActive(this._vamXInstalled);
		}
		if (this.vamXEnabledGameObjects != null)
		{
			foreach (GameObject gameObject in this.vamXEnabledGameObjects)
			{
				if (gameObject != null)
				{
					gameObject.SetActive(this._vamXInstalled);
				}
			}
		}
		if (this.vamXEnabledAndAdvancedSceneEditGameObjects != null)
		{
			foreach (GameObject gameObject2 in this.vamXEnabledAndAdvancedSceneEditGameObjects)
			{
				if (gameObject2 != null)
				{
					gameObject2.SetActive(this._vamXInstalled && !this.advancedSceneEditDisabled);
				}
			}
		}
		if (this.vamXDisabledGameObjects != null)
		{
			foreach (GameObject gameObject3 in this.vamXDisabledGameObjects)
			{
				if (gameObject3 != null)
				{
					gameObject3.SetActive(!this._vamXInstalled);
				}
			}
		}
		if (this.vamXDisabledAndAdvancedSceneEditGameObjects != null)
		{
			foreach (GameObject gameObject4 in this.vamXDisabledAndAdvancedSceneEditGameObjects)
			{
				if (gameObject4 != null)
				{
					gameObject4.SetActive(!this._vamXInstalled && !this.advancedSceneEditDisabled);
				}
			}
		}
		if (this.vamXDisabledAndAdvancedSceneEditDisabledGameObjects != null)
		{
			foreach (GameObject gameObject5 in this.vamXDisabledAndAdvancedSceneEditDisabledGameObjects)
			{
				if (gameObject5 != null)
				{
					gameObject5.SetActive(!this._vamXInstalled && this.advancedSceneEditDisabled);
				}
			}
		}
		if (this._vamXInstalled)
		{
			if (this._vamXWasInstalled)
			{
				if (this.lastLoadedvamXBootstrapPath != text)
				{
					UnityEngine.Debug.Log("Reloading vamX plugin due to version change");
					this.UnloadBootstrapPlugin(this.lastLoadedvamXBootstrapPath);
					this.LoadBootstrapPlugin(text);
					this.lastLoadedvamXBootstrapPath = text;
				}
			}
			else
			{
				UnityEngine.Debug.Log("Adding vamX bootstrap plugin at " + text);
				this.LoadBootstrapPlugin(text);
				this.lastLoadedvamXBootstrapPath = text;
				this._vamXWasInstalled = true;
			}
		}
		else if (this._vamXWasInstalled && !this._vamXInstalled)
		{
			UnityEngine.Debug.Log("Removing vamX bootstrap plugin");
			this.UnloadBootstrapPlugin(this.lastLoadedvamXBootstrapPath);
			this._vamXWasInstalled = false;
			this.lastLoadedvamXBootstrapPath = null;
		}
		this.SyncVersionText();
	}

	// Token: 0x06005EBD RID: 24253 RVA: 0x0023D764 File Offset: 0x0023BB64
	public void OpenVamXTutorialScene()
	{
		string text = this.NormalizeLoadPath(this.vamXTutorialScene);
		if (FileManager.FileExists(text, false, false))
		{
			this.Load(text);
		}
		else
		{
			SuperController.LogError("vamX tutorial scene " + text + " does not exist");
		}
	}

	// Token: 0x06005EBE RID: 24254 RVA: 0x0023D7AC File Offset: 0x0023BBAC
	public void OpenVamXMainScene()
	{
		string text = this.NormalizeLoadPath(this.vamXMainScene);
		if (FileManager.FileExists(text, false, false))
		{
			this.Load(text);
		}
		else
		{
			SuperController.LogError("vamX main scene " + text + " does not exist");
		}
	}

	// Token: 0x06005EBF RID: 24255 RVA: 0x0023D7F4 File Offset: 0x0023BBF4
	private void Awake()
	{
		SuperController._singleton = this;
		ZipConstants.DefaultCodePage = 0;
		this.DetermineVRRig();
		this.InitAudioListeners();
		this.SetSceneLoadPosition();
		this.SyncVersion();
		FileManager.ClearSecureReadPaths();
		FileManager.RegisterSecureReadPath(".");
		FileManager.ClearSecureWritePaths();
		FileManager.RegisterInternalSecureWritePath("Keys");
		FileManager.RegisterInternalSecureWritePath("Saves");
		FileManager.RegisterInternalSecureWritePath("Cache");
		FileManager.RegisterInternalSecureWritePath("Custom");
		FileManager.RegisterInternalSecureWritePath("AddonPackages");
		FileManager.RegisterInternalSecureWritePath("AddonPackagesBuilder");
		FileManager.RegisterInternalSecureWritePath("Temp");
		FileManager.RegisterPluginSecureWritePath("Saves", false);
		FileManager.RegisterPluginSecureWritePath("Custom", false);
		FileManager.RegisterPluginSecureWritePath("Saves/PluginData", true);
		FileManager.RegisterPluginSecureWritePath("Custom/PluginData", true);
		FileManager.CreateDirectory("Saves");
		FileManager.CreateDirectory("Saves/PluginData");
		FileManager.CreateDirectory("Saves/scene");
		FileManager.CreateDirectory("Custom");
		FileManager.CreateDirectory("Custom/PluginData");
		FileManager.demoPackagePrefixes = this.demoPackagePrefixes;
		FileManager.RegisterRestrictedReadPath("BrowserProfile");
		FileManager.RegisterRefreshHandler(new OnRefresh(this.OnPackageRefresh));
		if (this.hubBrowser != null)
		{
			HubBrowse hubBrowse = this.hubBrowser;
			hubBrowse.preShowCallbacks = (HubBrowse.PreShowCallback)System.Delegate.Combine(hubBrowse.preShowCallbacks, new HubBrowse.PreShowCallback(this.ActivateWorldUI));
			HubBrowse hubBrowse2 = this.hubBrowser;
			System.Delegate enableHubCallbacks = hubBrowse2.enableHubCallbacks;
			if (SuperController.<>f__am$cache2 == null)
			{
				SuperController.<>f__am$cache2 = new HubBrowse.EnableHubCallback(SuperController.<Awake>m__32);
			}
			hubBrowse2.enableHubCallbacks = (HubBrowse.EnableHubCallback)System.Delegate.Combine(enableHubCallbacks, SuperController.<>f__am$cache2);
			if (UserPreferences.singleton != null)
			{
				this.hubBrowser.HubEnabled = UserPreferences.singleton.enableHub;
			}
			HubBrowse hubBrowse3 = this.hubBrowser;
			System.Delegate enableWebBrowserCallbacks = hubBrowse3.enableWebBrowserCallbacks;
			if (SuperController.<>f__am$cache3 == null)
			{
				SuperController.<>f__am$cache3 = new HubBrowse.EnableWebBrowserCallback(SuperController.<Awake>m__33);
			}
			hubBrowse3.enableWebBrowserCallbacks = (HubBrowse.EnableWebBrowserCallback)System.Delegate.Combine(enableWebBrowserCallbacks, SuperController.<>f__am$cache3);
			if (UserPreferences.singleton != null)
			{
				this.hubBrowser.WebBrowserEnabled = UserPreferences.singleton.enableWebBrowser;
			}
		}
		this.SyncToKeyFile(false);
	}

	// Token: 0x06005EC0 RID: 24256 RVA: 0x0023D9FC File Offset: 0x0023BDFC
	private IEnumerator DelayLoadSessionPlugins()
	{
		yield return null;
		while (UserPreferences.singleton != null && UserPreferences.singleton.firstTimeUser)
		{
			yield return null;
		}
		if (this.sessionPresetManagerControl != null)
		{
			this.sessionPresetManagerControl.LoadUserDefaults();
		}
		yield break;
	}

	// Token: 0x06005EC1 RID: 24257 RVA: 0x0023DA18 File Offset: 0x0023BE18
	private IEnumerator DelayStart()
	{
		this.onStartScene = true;
		yield return null;
		yield return null;
		yield return null;
		this.StartScene();
		yield break;
	}

	// Token: 0x06005EC2 RID: 24258 RVA: 0x0023DA34 File Offset: 0x0023BE34
	private void Start()
	{
		Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
		Caching.ClearCache();
		this.SetSavesDirFromCommandline();
		this.castRay = default(Ray);
		this.playerNavTrackerGO = new GameObject();
		base.StartCoroutine(this.InitAssetManager());
		this.monitorCameraFOV = this.startingMonitorCameraFOV;
		this.SyncMonitorCameraFOV();
		this.InitAtomPool();
		this.InitAtoms();
		this.InitTargets();
		this.InitMotionControllerNaviation();
		if (UserPreferences.singleton != null && UserPreferences.singleton.shouldLoadPrefsFileOnStart)
		{
			UserPreferences.singleton.RestorePreferences();
		}
		this.InitUI();
		this.ResetFocusPoint();
		this.SyncLeapEnabled();
		this.SyncGameMode();
		this.BuildMigrationMappings();
		bool flag = false;
		bool flag2 = false;
		this.SyncVamX();
		if (!this.UIDisabled)
		{
			base.StartCoroutine(this.DelayLoadSessionPlugins());
			flag = this.BuildFilesToMigrateMap();
			flag2 = this.BuildObsoleteDirectoriesList();
			if (flag || flag2)
			{
				this.OpenTopWorldUI();
			}
		}
		bool flag3 = false;
		if (UserPreferences.singleton != null && UserPreferences.singleton.firstTimeUser)
		{
			flag3 = true;
		}
		if (!flag && !flag2 && this.startSceneEnabled && !flag3 && (!this.showMainHUDOnStart || this.UIDisabled || this._onStartupSkipStartScreen))
		{
			base.StartCoroutine(this.DelayStart());
		}
	}

	// Token: 0x06005EC3 RID: 24259 RVA: 0x0023DBAC File Offset: 0x0023BFAC
	private void Update()
	{
		this.CheckResumeSimulation();
		this.CheckResumeAutoSimulation();
		this.CheckResumeRender();
		this.CheckLoadingIcon();
		this.CheckMessageAndErrorQueue();
		this.SyncAllErrorsInputFields();
		this.SyncAllMessagesInputFields();
		if (Time.unscaledDeltaTime > 0.015f)
		{
			DebugHUD.Alert1();
		}
		if (CameraTarget.centerTarget != null && CameraTarget.centerTarget.targetCamera != null)
		{
			this.lookCamera = CameraTarget.centerTarget.targetCamera;
		}
		this.drawRayLineRight = false;
		this.drawRayLineLeft = false;
		this.SyncNavigationHologridVisibility();
		this.PrepControllers();
		this.ProcessTimeScale();
		if (!this.onStartScene)
		{
			this.CheckAutoConnectLeapHands();
			this.ProcessUI();
			this.ProcessGUIInteract();
			this.ProcessPlayerNavMove();
			this.ProcessMouseChange();
			this.ProcessMouseControl();
			this.ProcessKeyBindings();
			this.ProcessKeyboardFreeNavigation();
			this.ProcessCycle();
			this.VerifyPossess();
			if (this.selectMode == SuperController.SelectMode.FreeMoveMouse)
			{
				if (this.mouseCrosshairPointer != null)
				{
					this.mouseCrosshairPointer.anchoredPosition = Input.mousePosition;
				}
				this.ProcessMouseFreeNavigation();
				this.ProcessLookAtTrigger();
				this.ProcessCommonTargetSelection();
				this.ProcessTargetShow(false);
				this.ProcessMouseTargetControl(true);
			}
			else if (this.selectMode == SuperController.SelectMode.FreeMove)
			{
				this.ProcessFreeMoveNavigation();
				this.ProcessLookAtTrigger();
			}
			else if (this.selectMode == SuperController.SelectMode.Teleport)
			{
				this.ProcessTeleportMode();
			}
			else
			{
				this.ProcessControllerNavigation(this.freeMoveAction, false);
				SuperController.SelectMode selectMode = this.selectMode;
				switch (selectMode)
				{
				case SuperController.SelectMode.SaveScreenshot:
					this.drawRayLineLeft = !this.MonitorRigActive;
					this.drawRayLineRight = !this.MonitorRigActive;
					this.ProcessMotionControllerNavigation();
					this.ProcessSaveScreenshot(false);
					break;
				case SuperController.SelectMode.Screenshot:
					this.drawRayLineLeft = !this.MonitorRigActive;
					this.drawRayLineRight = !this.MonitorRigActive;
					this.ProcessMotionControllerNavigation();
					this.ProcessHiResScreenshot();
					break;
				case SuperController.SelectMode.Custom:
					this.drawRayLineLeft = false;
					this.drawRayLineRight = false;
					this.ProcessMotionControllerNavigation();
					break;
				case SuperController.SelectMode.CustomWithTargetControl:
					this.ProcessTargetShow(false);
					this.ProcessMouseTargetControl(false);
					this.ProcessMotionControllerTargetHighlight();
					this.ProcessMotionControllerNavigation();
					this.ProcessMotionControllerTargetControl(false);
					break;
				case SuperController.SelectMode.CustomWithVRTargetControl:
					this.ProcessTargetShow(false);
					this.ProcessMotionControllerTargetHighlight();
					this.ProcessMotionControllerNavigation();
					this.ProcessMotionControllerTargetControl(false);
					break;
				default:
					switch (selectMode)
					{
					case SuperController.SelectMode.Off:
					case SuperController.SelectMode.FilteredTargets:
					case SuperController.SelectMode.Targets:
						this.ProcessLookAtTrigger();
						this.ProcessCommonTargetSelection();
						this.ProcessTargetShow(true);
						this.ProcessMouseTargetControl(true);
						this.ProcessMotionControllerTargetHighlight();
						this.ProcessMotionControllerNavigation();
						this.ProcessMotionControllerTargetControl(true);
						break;
					default:
						switch (selectMode)
						{
						case SuperController.SelectMode.Possess:
							this.ProcessMotionControllerNavigation();
							this.ProcessPossess();
							goto IL_321;
						case SuperController.SelectMode.TwoStagePossess:
							this.ProcessMotionControllerNavigation();
							this.ProcessTwoStagePossess();
							goto IL_321;
						case SuperController.SelectMode.AnimationRecord:
							this.ProcessTargetShow(false);
							this.ProcessMouseTargetControl(false);
							this.ProcessMotionControllerTargetHighlight();
							this.ProcessMotionControllerNavigation();
							this.ProcessMotionControllerTargetControl(false);
							this.ProcessAnimationRecord();
							goto IL_321;
						}
						this.ProcessMotionControllerSelect();
						this.ProcessMotionControllerNavigation();
						this.ProcessMouseSelect();
						break;
					}
					break;
				}
			}
			IL_321:
			this.ProcessUIMove();
			this.SyncCursor();
		}
		if (!this.MonitorRigActive)
		{
			if (this._mainHUDVisible)
			{
				this.drawRayLineLeft = true;
				this.drawRayLineRight = true;
			}
			else if (UserPreferences.singleton.alwaysShowPointersOnTouch)
			{
				this.drawRayLineRight = (this.drawRayLineLeft = this.GetTargetShow());
			}
		}
		if (this.leftControllerCamera != null && !this.leftControllerCamera.gameObject.activeInHierarchy)
		{
			this.drawRayLineLeft = false;
		}
		if (this.rightControllerCamera != null && !this.rightControllerCamera.gameObject.activeInHierarchy)
		{
			this.drawRayLineRight = false;
		}
		if (this.drawRayLineLeft && this.motionControllerLeft != null)
		{
			if (this.rayLineDrawerLeft != null)
			{
				this.rayLineDrawerLeft.SetLinePoints(this.motionControllerLeft.position, this.motionControllerLeft.position + 50f * this.motionControllerLeft.forward);
				this.rayLineDrawerLeft.Draw(base.gameObject.layer);
			}
			if (this.rayLineLeft != null)
			{
				this.rayLineLeft.transform.position = this.motionControllerLeft.position;
				this.rayLineLeft.transform.rotation = this.motionControllerLeft.rotation;
				this.rayLineLeft.gameObject.SetActive(true);
			}
		}
		else if (this.rayLineLeft != null)
		{
			this.rayLineLeft.gameObject.SetActive(false);
		}
		if (this.drawRayLineRight && this.motionControllerRight != null)
		{
			if (this.rayLineDrawerRight != null)
			{
				this.rayLineDrawerRight.SetLinePoints(this.motionControllerRight.position, this.motionControllerRight.position + 50f * this.motionControllerRight.forward);
				this.rayLineDrawerRight.Draw(base.gameObject.layer);
			}
			if (this.rayLineRight != null)
			{
				this.rayLineRight.transform.position = this.motionControllerRight.position;
				this.rayLineRight.transform.rotation = this.motionControllerRight.rotation;
				this.rayLineRight.gameObject.SetActive(true);
			}
		}
		else if (this.rayLineRight != null)
		{
			this.rayLineRight.gameObject.SetActive(false);
		}
	}

	// Token: 0x06005EC4 RID: 24260 RVA: 0x0023E17B File Offset: 0x0023C57B
	private void OnDestroy()
	{
		this.UnloadAllBootstrapPlugins();
		FileManager.UnregisterRefreshHandler(new OnRefresh(this.OnPackageRefresh));
	}

	// Token: 0x06005EC5 RID: 24261 RVA: 0x0023E194 File Offset: 0x0023C594
	[CompilerGenerated]
	private void <InitUI>m__0(float A_1)
	{
		this.worldScale = this.worldScaleSlider.value;
	}

	// Token: 0x06005EC6 RID: 24262 RVA: 0x0023E1A7 File Offset: 0x0023C5A7
	[CompilerGenerated]
	private void <InitUI>m__1(float A_1)
	{
		this.worldScale = this.worldScaleSliderAlt.value;
	}

	// Token: 0x06005EC7 RID: 24263 RVA: 0x0023E1BA File Offset: 0x0023C5BA
	[CompilerGenerated]
	private void <InitUI>m__2(float A_1)
	{
		this.controllerScale = this.controllerScaleSlider.value;
	}

	// Token: 0x06005EC8 RID: 24264 RVA: 0x0023E1CD File Offset: 0x0023C5CD
	[CompilerGenerated]
	private void <InitUI>m__3(float A_1)
	{
		this.playerHeightAdjust = this.playerHeightAdjustSlider.value;
	}

	// Token: 0x06005EC9 RID: 24265 RVA: 0x0023E1E0 File Offset: 0x0023C5E0
	[CompilerGenerated]
	private void <InitUI>m__4(float A_1)
	{
		this.playerHeightAdjust = this.playerHeightAdjustSliderAlt.value;
	}

	// Token: 0x06005ECA RID: 24266 RVA: 0x0023E1F3 File Offset: 0x0023C5F3
	[CompilerGenerated]
	private void <InitUI>m__5(float A_1)
	{
		this.monitorUIScale = this.monitorUIScaleSlider.value;
	}

	// Token: 0x06005ECB RID: 24267 RVA: 0x0023E206 File Offset: 0x0023C606
	[CompilerGenerated]
	private void <InitUI>m__6(float A_1)
	{
		this.monitorUIYOffset = this.monitorUIYOffsetSlider.value;
	}

	// Token: 0x06005ECC RID: 24268 RVA: 0x0023E219 File Offset: 0x0023C619
	[CompilerGenerated]
	private void <InitUI>m__7(float A_1)
	{
		this.monitorCameraFOV = this.monitorCameraFOVSlider.value;
	}

	// Token: 0x06005ECD RID: 24269 RVA: 0x0023E22C File Offset: 0x0023C62C
	[CompilerGenerated]
	private void <InitUI>m__8(bool A_1)
	{
		if (this.editModeToggle.isOn)
		{
			this.gameMode = SuperController.GameMode.Edit;
		}
	}

	// Token: 0x06005ECE RID: 24270 RVA: 0x0023E245 File Offset: 0x0023C645
	[CompilerGenerated]
	private void <InitUI>m__9(bool A_1)
	{
		if (this.playModeToggle.isOn)
		{
			this.gameMode = SuperController.GameMode.Play;
		}
	}

	// Token: 0x06005ECF RID: 24271 RVA: 0x0023E25E File Offset: 0x0023C65E
	[CompilerGenerated]
	private void <InitUI>m__A(bool A_1)
	{
		this.helpOverlayOn = this.helpToggle.isOn;
	}

	// Token: 0x06005ED0 RID: 24272 RVA: 0x0023E271 File Offset: 0x0023C671
	[CompilerGenerated]
	private void <InitUI>m__B(bool A_1)
	{
		this.helpOverlayOn = this.helpToggleAlt.isOn;
	}

	// Token: 0x06005ED1 RID: 24273 RVA: 0x0023E284 File Offset: 0x0023C684
	[CompilerGenerated]
	private void <InitUI>m__C(bool A_1)
	{
		this.lockHeightDuringNavigate = this.lockHeightDuringNavigateToggle.isOn;
	}

	// Token: 0x06005ED2 RID: 24274 RVA: 0x0023E297 File Offset: 0x0023C697
	[CompilerGenerated]
	private void <InitUI>m__D(bool A_1)
	{
		this.lockHeightDuringNavigate = this.lockHeightDuringNavigateToggleAlt.isOn;
	}

	// Token: 0x06005ED3 RID: 24275 RVA: 0x0023E2AA File Offset: 0x0023C6AA
	[CompilerGenerated]
	private void <InitUI>m__E(bool A_1)
	{
		this.freeMoveFollowFloor = this.freeMoveFollowFloorToggle.isOn;
	}

	// Token: 0x06005ED4 RID: 24276 RVA: 0x0023E2BD File Offset: 0x0023C6BD
	[CompilerGenerated]
	private void <InitUI>m__F(bool A_1)
	{
		this.freeMoveFollowFloor = this.freeMoveFollowFloorToggleAlt.isOn;
	}

	// Token: 0x06005ED5 RID: 24277 RVA: 0x0023E2D0 File Offset: 0x0023C6D0
	[CompilerGenerated]
	private void <InitUI>m__10(bool A_1)
	{
		this.disableAllNavigation = this.disableAllNavigationToggle.isOn;
	}

	// Token: 0x06005ED6 RID: 24278 RVA: 0x0023E2E3 File Offset: 0x0023C6E3
	[CompilerGenerated]
	private void <InitUI>m__11(bool A_1)
	{
		this.disableGrabNavigation = this.disableGrabNavigationToggle.isOn;
	}

	// Token: 0x06005ED7 RID: 24279 RVA: 0x0023E2F6 File Offset: 0x0023C6F6
	[CompilerGenerated]
	private void <InitUI>m__12(bool A_1)
	{
		this.disableTeleport = this.disableTeleportToggle.isOn;
	}

	// Token: 0x06005ED8 RID: 24280 RVA: 0x0023E309 File Offset: 0x0023C709
	[CompilerGenerated]
	private void <InitUI>m__13(bool A_1)
	{
		this.disableTeleportDuringPossess = this.disableTeleportDuringPossessToggle.isOn;
	}

	// Token: 0x06005ED9 RID: 24281 RVA: 0x0023E31C File Offset: 0x0023C71C
	[CompilerGenerated]
	private void <InitUI>m__14(bool A_1)
	{
		this.teleportAllowRotation = this.teleportAllowRotationToggle.isOn;
	}

	// Token: 0x06005EDA RID: 24282 RVA: 0x0023E32F File Offset: 0x0023C72F
	[CompilerGenerated]
	private void <InitUI>m__15(float A_1)
	{
		this.freeMoveMultiplier = this.freeMoveMultiplierSlider.value;
	}

	// Token: 0x06005EDB RID: 24283 RVA: 0x0023E342 File Offset: 0x0023C742
	[CompilerGenerated]
	private void <InitUI>m__16(float A_1)
	{
		this.grabNavigationPositionMultiplier = this.grabNavigationPositionMultiplierSlider.value;
	}

	// Token: 0x06005EDC RID: 24284 RVA: 0x0023E355 File Offset: 0x0023C755
	[CompilerGenerated]
	private void <InitUI>m__17(float A_1)
	{
		this.grabNavigationRotationMultiplier = this.grabNavigationRotationMultiplierSlider.value;
	}

	// Token: 0x06005EDD RID: 24285 RVA: 0x0023E368 File Offset: 0x0023C768
	[CompilerGenerated]
	private void <InitUI>m__18(bool A_1)
	{
		this.showNavigationHologrid = this.showNavigationHologridToggle.isOn;
	}

	// Token: 0x06005EDE RID: 24286 RVA: 0x0023E37B File Offset: 0x0023C77B
	[CompilerGenerated]
	private void <InitUI>m__19(float A_1)
	{
		this.hologridTransparency = this.hologridTransparencySlider.value;
	}

	// Token: 0x06005EDF RID: 24287 RVA: 0x0023E38E File Offset: 0x0023C78E
	[CompilerGenerated]
	private void <InitUI>m__1A(bool b)
	{
		this.allowHeadPossessMousePanAndZoom = b;
	}

	// Token: 0x06005EE0 RID: 24288 RVA: 0x0023E397 File Offset: 0x0023C797
	[CompilerGenerated]
	private void <InitUI>m__1B(bool A_1)
	{
		this.allowPossessSpringAdjustment = this.allowPossessSpringAdjustmentToggle.isOn;
	}

	// Token: 0x06005EE1 RID: 24289 RVA: 0x0023E3AA File Offset: 0x0023C7AA
	[CompilerGenerated]
	private void <InitUI>m__1C(float A_1)
	{
		this.possessPositionSpring = this.possessPositionSpringSlider.value;
	}

	// Token: 0x06005EE2 RID: 24290 RVA: 0x0023E3BD File Offset: 0x0023C7BD
	[CompilerGenerated]
	private void <InitUI>m__1D(float A_1)
	{
		this.possessRotationSpring = this.possessRotationSpringSlider.value;
	}

	// Token: 0x06005EE3 RID: 24291 RVA: 0x0023E3D0 File Offset: 0x0023C7D0
	[CompilerGenerated]
	private void <InitUI>m__1E(bool b)
	{
		this.generateDepthTexture = b;
	}

	// Token: 0x06005EE4 RID: 24292 RVA: 0x0023E3D9 File Offset: 0x0023C7D9
	[CompilerGenerated]
	private void <InitUI>m__1F(bool b)
	{
		this.useMonitorRigAudioListenerWhenActive = b;
	}

	// Token: 0x06005EE5 RID: 24293 RVA: 0x0023E3E2 File Offset: 0x0023C7E2
	[CompilerGenerated]
	private void <InitUI>m__20(float A_1)
	{
		this.loResScreenShotCameraFOV = this.loResScreenShotCameraFOVSlider.value;
	}

	// Token: 0x06005EE6 RID: 24294 RVA: 0x0023E3F5 File Offset: 0x0023C7F5
	[CompilerGenerated]
	private void <InitUI>m__21(float A_1)
	{
		this.hiResScreenShotCameraFOV = this.hiResScreenShotCameraFOVSlider.value;
	}

	// Token: 0x06005EE7 RID: 24295 RVA: 0x0023E408 File Offset: 0x0023C808
	[CompilerGenerated]
	private void <InitUI>m__22(bool b)
	{
		this.worldUIShowOverlaySky = b;
	}

	// Token: 0x06005EE8 RID: 24296 RVA: 0x0023E411 File Offset: 0x0023C811
	[CompilerGenerated]
	private void <InitUI>m__23(bool b)
	{
		this.showHiddenAtoms = b;
	}

	// Token: 0x06005EE9 RID: 24297 RVA: 0x0023E41A File Offset: 0x0023C81A
	[CompilerGenerated]
	private void <InitUI>m__24(bool b)
	{
		this.showHiddenAtoms = b;
	}

	// Token: 0x06005EEA RID: 24298 RVA: 0x0023E423 File Offset: 0x0023C823
	[CompilerGenerated]
	private void <InitUI>m__25(bool b)
	{
		this.allowGrabPlusTriggerHandToggle = b;
	}

	// Token: 0x06005EEB RID: 24299 RVA: 0x0023E42C File Offset: 0x0023C82C
	[CompilerGenerated]
	private void <InitUI>m__26(bool b)
	{
		this.alwaysUseAlternateHands = b;
	}

	// Token: 0x06005EEC RID: 24300 RVA: 0x0023E435 File Offset: 0x0023C835
	[CompilerGenerated]
	private void <InitUI>m__27(bool b)
	{
		this.useLegacyWorldScaleChange = b;
	}

	// Token: 0x06005EED RID: 24301 RVA: 0x0023E43E File Offset: 0x0023C83E
	[CompilerGenerated]
	private void <InitUI>m__28(bool b)
	{
		this.disableRenderForAtomsNotInIsolatedSubScene = b;
	}

	// Token: 0x06005EEE RID: 24302 RVA: 0x0023E447 File Offset: 0x0023C847
	[CompilerGenerated]
	private void <InitUI>m__29(bool b)
	{
		this.freezePhysicsForAtomsNotInIsolatedSubScene = b;
	}

	// Token: 0x06005EEF RID: 24303 RVA: 0x0023E450 File Offset: 0x0023C850
	[CompilerGenerated]
	private void <InitUI>m__2A(bool b)
	{
		this.disableCollisionForAtomsNotInIsolatedSubScene = b;
	}

	// Token: 0x06005EF0 RID: 24304 RVA: 0x0023E459 File Offset: 0x0023C859
	[CompilerGenerated]
	private void <InitUI>m__2B(bool b)
	{
		this.disableRenderForAtomsNotInIsolatedAtom = b;
	}

	// Token: 0x06005EF1 RID: 24305 RVA: 0x0023E462 File Offset: 0x0023C862
	[CompilerGenerated]
	private void <InitUI>m__2C(bool b)
	{
		this.freezePhysicsForAtomsNotInIsolatedAtom = b;
	}

	// Token: 0x06005EF2 RID: 24306 RVA: 0x0023E46B File Offset: 0x0023C86B
	[CompilerGenerated]
	private void <InitUI>m__2D(bool b)
	{
		this.disableCollisionForAtomsNotInIsolatedAtom = b;
	}

	// Token: 0x06005EF3 RID: 24307 RVA: 0x0023E474 File Offset: 0x0023C874
	[CompilerGenerated]
	private void <InitUI>m__2E(bool b)
	{
		this.leapMotionEnabled = b;
	}

	// Token: 0x06005EF4 RID: 24308 RVA: 0x0023E47D File Offset: 0x0023C87D
	[CompilerGenerated]
	private void <InitUI>m__2F(bool b)
	{
		this.openMainHUDOnError = b;
	}

	// Token: 0x06005EF5 RID: 24309 RVA: 0x0023E486 File Offset: 0x0023C886
	[CompilerGenerated]
	private static int <ProcessControllerTargetHighlight>m__30(FreeControllerV3 c1, FreeControllerV3 c2)
	{
		return c1.distanceHolder.CompareTo(c2.distanceHolder);
	}

	// Token: 0x06005EF6 RID: 24310 RVA: 0x0023E499 File Offset: 0x0023C899
	[CompilerGenerated]
	private static int <ProcessControllerPossess>m__31(FreeControllerV3 c1, FreeControllerV3 c2)
	{
		return c1.distanceHolder.CompareTo(c2.distanceHolder);
	}

	// Token: 0x06005EF7 RID: 24311 RVA: 0x0023E4AC File Offset: 0x0023C8AC
	[CompilerGenerated]
	private static void <Awake>m__32()
	{
		if (UserPreferences.singleton != null)
		{
			UserPreferences.singleton.enableHub = true;
		}
	}

	// Token: 0x06005EF8 RID: 24312 RVA: 0x0023E4C9 File Offset: 0x0023C8C9
	[CompilerGenerated]
	private static void <Awake>m__33()
	{
		if (UserPreferences.singleton != null)
		{
			UserPreferences.singleton.enableWebBrowser = true;
		}
	}

	// Token: 0x04004BCF RID: 19407
	private static SuperController _singleton;

	// Token: 0x04004BD0 RID: 19408
	public string editorMimicVersion = "1.20.0.3";

	// Token: 0x04004BD1 RID: 19409
	public SuperController.KeyType editorMimicHighestKey;

	// Token: 0x04004BD2 RID: 19410
	public RectTransform dynamicButtonPrefab;

	// Token: 0x04004BD3 RID: 19411
	public RectTransform dynamicTogglePrefab;

	// Token: 0x04004BD4 RID: 19412
	public RectTransform dynamicSliderPrefab;

	// Token: 0x04004BD5 RID: 19413
	[Tooltip("Add Atom Tab, Animation Tab, Audio Tab")]
	public bool disableAdvancedSceneEdit;

	// Token: 0x04004BD6 RID: 19414
	public bool disableSaveSceneButton;

	// Token: 0x04004BD7 RID: 19415
	public bool disableLoadSceneButton;

	// Token: 0x04004BD8 RID: 19416
	public bool disablePackages;

	// Token: 0x04004BD9 RID: 19417
	public bool disableCustomUI;

	// Token: 0x04004BDA RID: 19418
	public bool disableBrowse;

	// Token: 0x04004BDB RID: 19419
	public bool disablePromotional;

	// Token: 0x04004BDC RID: 19420
	public bool disableKeyInformation;

	// Token: 0x04004BDD RID: 19421
	public bool disableHub;

	// Token: 0x04004BDE RID: 19422
	public bool disableTermsOfUse;

	// Token: 0x04004BDF RID: 19423
	public string demoScenesDir = "Saves/scene/MeshedVR/DemoScenes";

	// Token: 0x04004BE0 RID: 19424
	public string[] demoPackagePrefixes;

	// Token: 0x04004BE1 RID: 19425
	public Transform[] loadSceneButtons;

	// Token: 0x04004BE2 RID: 19426
	public Transform[] loadSceneDisabledButtons;

	// Token: 0x04004BE3 RID: 19427
	public Transform[] onlineBrowseSceneButtons;

	// Token: 0x04004BE4 RID: 19428
	public Transform[] onlineBrowseSceneDisabledButtons;

	// Token: 0x04004BE5 RID: 19429
	public Transform[] saveSceneButtons;

	// Token: 0x04004BE6 RID: 19430
	public Transform[] saveSceneDisabledButtons;

	// Token: 0x04004BE7 RID: 19431
	public Transform[] advancedSceneEditButtons;

	// Token: 0x04004BE8 RID: 19432
	public Transform[] advancedSceneEditDisabledButtons;

	// Token: 0x04004BE9 RID: 19433
	public Transform[] advancedSceneEditOnlyEditModeTransforms;

	// Token: 0x04004BEA RID: 19434
	public Transform[] advancedSceneEditDisabledEditModeTransforms;

	// Token: 0x04004BEB RID: 19435
	public Transform[] keyInformationTransforms;

	// Token: 0x04004BEC RID: 19436
	public Transform[] promotionalTransforms;

	// Token: 0x04004BED RID: 19437
	public Transform[] hubDisabledTransforms;

	// Token: 0x04004BEE RID: 19438
	public Transform[] hubDisabledEnableTransforms;

	// Token: 0x04004BEF RID: 19439
	public Transform[] termsOfUseTransforms;

	// Token: 0x04004BF0 RID: 19440
	[SerializeField]
	protected string _freeKey;

	// Token: 0x04004BF1 RID: 19441
	[SerializeField]
	protected string _teaserKey;

	// Token: 0x04004BF2 RID: 19442
	[SerializeField]
	protected string _entertainerKey;

	// Token: 0x04004BF3 RID: 19443
	[SerializeField]
	protected string _creatorKey;

	// Token: 0x04004BF4 RID: 19444
	[SerializeField]
	protected string _steamKey;

	// Token: 0x04004BF5 RID: 19445
	[SerializeField]
	protected string _nsteamKey;

	// Token: 0x04004BF6 RID: 19446
	[SerializeField]
	protected string _retailKey;

	// Token: 0x04004BF7 RID: 19447
	[SerializeField]
	protected string _keyFilePath;

	// Token: 0x04004BF8 RID: 19448
	[SerializeField]
	protected string _legacySteamKeyFilePath;

	// Token: 0x04004BF9 RID: 19449
	public InputField keyInputField;

	// Token: 0x04004BFA RID: 19450
	public InputFieldAction keyInputFieldAction;

	// Token: 0x04004BFB RID: 19451
	public Text keyEntryStatus;

	// Token: 0x04004BFC RID: 19452
	public Transform freeKeyTransform;

	// Token: 0x04004BFD RID: 19453
	public Transform teaserKeyTransform;

	// Token: 0x04004BFE RID: 19454
	public Transform entertainerKeyTransform;

	// Token: 0x04004BFF RID: 19455
	public Transform creatorKeyTransform;

	// Token: 0x04004C00 RID: 19456
	public Transform keySyncingIndicator;

	// Token: 0x04004C01 RID: 19457
	public string savesDir = "Saves\\";

	// Token: 0x04004C02 RID: 19458
	public string savesDirEditor = "Saves\\";

	// Token: 0x04004C03 RID: 19459
	protected string lastLoadDir = string.Empty;

	// Token: 0x04004C04 RID: 19460
	protected string loadedName;

	// Token: 0x04004C05 RID: 19461
	protected bool _isLoading;

	// Token: 0x04004C06 RID: 19462
	public PackageBuilder packageBuilder;

	// Token: 0x04004C07 RID: 19463
	public Transform packageBuilderUI;

	// Token: 0x04004C08 RID: 19464
	public PackageBuilder packageManager;

	// Token: 0x04004C09 RID: 19465
	public Transform packageManagerUI;

	// Token: 0x04004C0A RID: 19466
	public HubDownloader packageDownloader;

	// Token: 0x04004C0B RID: 19467
	public Transform loadConfirmPanel;

	// Token: 0x04004C0C RID: 19468
	public Button loadConfirmButton;

	// Token: 0x04004C0D RID: 19469
	public Text loadConfirmPathText;

	// Token: 0x04004C0E RID: 19470
	public Transform overwriteConfirmPanel;

	// Token: 0x04004C0F RID: 19471
	public Button overwriteConfirmButton;

	// Token: 0x04004C10 RID: 19472
	public Text overwriteConfirmPathText;

	// Token: 0x04004C11 RID: 19473
	protected List<Atom> _saveQueue;

	// Token: 0x04004C12 RID: 19474
	public bool packageMode;

	// Token: 0x04004C13 RID: 19475
	protected ZipOutputStream zos;

	// Token: 0x04004C14 RID: 19476
	protected Dictionary<string, bool> alreadyPackaged;

	// Token: 0x04004C15 RID: 19477
	protected HashSet<string> referencedVarPackages;

	// Token: 0x04004C16 RID: 19478
	public JSONNode loadJson;

	// Token: 0x04004C17 RID: 19479
	private AsyncFlag loadFlag;

	// Token: 0x04004C18 RID: 19480
	protected bool loadSceneWorldDialogActivatedFromWorld;

	// Token: 0x04004C19 RID: 19481
	protected bool loadTemplateWorldDialogActivatedFromWorld;

	// Token: 0x04004C1A RID: 19482
	protected bool hubOpenedFromWorld;

	// Token: 0x04004C1B RID: 19483
	public bool enableStartScene = true;

	// Token: 0x04004C1C RID: 19484
	public JSONEmbed startJSONEmbedScene;

	// Token: 0x04004C1D RID: 19485
	public JSONEmbed newJSONEmbedScene;

	// Token: 0x04004C1E RID: 19486
	public bool disableUI;

	// Token: 0x04004C1F RID: 19487
	public bool alwaysEnablePointers;

	// Token: 0x04004C20 RID: 19488
	public bool disableNavigation;

	// Token: 0x04004C21 RID: 19489
	public bool disableVR;

	// Token: 0x04004C22 RID: 19490
	private bool onStartScene;

	// Token: 0x04004C23 RID: 19491
	public string startSceneName = "scene/default.json";

	// Token: 0x04004C24 RID: 19492
	public string startSceneAltName = "scene/MeshedVR/default.json";

	// Token: 0x04004C25 RID: 19493
	public string newSceneName = "scene/default.json";

	// Token: 0x04004C26 RID: 19494
	public string newSceneAltName = "scene/MeshedVR/default.json";

	// Token: 0x04004C27 RID: 19495
	public string[] editorSceneList;

	// Token: 0x04004C28 RID: 19496
	public PresetManagerControl sessionPresetManagerControl;

	// Token: 0x04004C29 RID: 19497
	public Transform atomContainer;

	// Token: 0x04004C2A RID: 19498
	public Transform atomPoolContainer;

	// Token: 0x04004C2B RID: 19499
	protected Dictionary<string, List<Atom>> typeToAtomPool;

	// Token: 0x04004C2C RID: 19500
	protected string lastMediaDir = string.Empty;

	// Token: 0x04004C2D RID: 19501
	protected string lastScenePathDir = string.Empty;

	// Token: 0x04004C2E RID: 19502
	protected string lastBrowseDir = string.Empty;

	// Token: 0x04004C2F RID: 19503
	protected string[] obsoleteDirectories = new string[]
	{
		"Saves/scene/MeshedVR/1.0Source",
		"Saves/scene/MeshedVR/1.1Source",
		"Saves/scene/MeshedVR/1.2Source",
		"Saves/scene/MeshedVR/1.3Source",
		"Saves/scene/MeshedVR/1.4Source",
		"Saves/scene/MeshedVR/1.5Source",
		"Saves/scene/MeshedVR/1.6Source",
		"Saves/scene/MeshedVR/1.7Source",
		"Saves/scene/MeshedVR/1.8Source",
		"Saves/scene/MeshedVR/1.9Source",
		"Saves/scene/MeshedVR/1.10Source",
		"Saves/scene/MeshedVR/1.11Source",
		"Saves/scene/MeshedVR/1.12Source",
		"Saves/scene/MeshedVR/1.13Source",
		"Saves/scene/MeshedVR/1.15Source",
		"Saves/scene/MeshedVR/1.16Source",
		"Saves/scene/MeshedVR/Bonus",
		"Saves/scene/MeshedVR/Tests",
		"Saves/Person/appearance/MeshedVR",
		"Saves/Person/appearance/NutkinChan Looks Mega Pack",
		"Saves/Person/full/MeshedVR",
		"Saves/Person/pose/MeshedVR",
		"Saves/Person/pose/NutkinChan Pose Mega Pack",
		"Custom/Atom/Person/Appearance/MVR",
		"Custom/Atom/Person/Clothing/MVR",
		"Custom/Atom/Person/Hair/MVR",
		"Custom/Atom/Person/Morphs/MVR"
	};

	// Token: 0x04004C30 RID: 19504
	public Transform obsoletePathsPanel;

	// Token: 0x04004C31 RID: 19505
	public Text obsoletePathsText;

	// Token: 0x04004C32 RID: 19506
	private List<string> directoriesToRemove;

	// Token: 0x04004C33 RID: 19507
	protected Dictionary<string, string> pathMigrationMappings;

	// Token: 0x04004C34 RID: 19508
	private List<string> legacyDirectories;

	// Token: 0x04004C35 RID: 19509
	public Transform migratePathsPanel;

	// Token: 0x04004C36 RID: 19510
	public Text oldPathsText;

	// Token: 0x04004C37 RID: 19511
	public Text newPathsText;

	// Token: 0x04004C38 RID: 19512
	protected Dictionary<string, string> filesToMigrateMap;

	// Token: 0x04004C39 RID: 19513
	public Camera screenshotCamera;

	// Token: 0x04004C3A RID: 19514
	public Transform screenshotPreview;

	// Token: 0x04004C3B RID: 19515
	public Camera hiResScreenshotCamera;

	// Token: 0x04004C3C RID: 19516
	public Transform hiResScreenshotPreview;

	// Token: 0x04004C3D RID: 19517
	public Slider loResScreenShotCameraFOVSlider;

	// Token: 0x04004C3E RID: 19518
	[SerializeField]
	private float _loResScreenShotCameraFOV = 40f;

	// Token: 0x04004C3F RID: 19519
	public Slider hiResScreenShotCameraFOVSlider;

	// Token: 0x04004C40 RID: 19520
	[SerializeField]
	private float _hiResScreenShotCameraFOV = 40f;

	// Token: 0x04004C41 RID: 19521
	protected string savingName;

	// Token: 0x04004C42 RID: 19522
	protected SuperController.ScreenShotCallback screenShotCallback;

	// Token: 0x04004C43 RID: 19523
	[SerializeField]
	private SuperController.GameMode _gameMode;

	// Token: 0x04004C44 RID: 19524
	public Toggle editModeToggle;

	// Token: 0x04004C45 RID: 19525
	public Toggle playModeToggle;

	// Token: 0x04004C46 RID: 19526
	public Transform[] editModeOnlyTransforms;

	// Token: 0x04004C47 RID: 19527
	public Transform[] playModeOnlyTransforms;

	// Token: 0x04004C48 RID: 19528
	public Transform errorLogPanel;

	// Token: 0x04004C49 RID: 19529
	protected string errorLog;

	// Token: 0x04004C4A RID: 19530
	public InputField allErrorsInputField;

	// Token: 0x04004C4B RID: 19531
	public InputField allErrorsInputField2;

	// Token: 0x04004C4C RID: 19532
	public Text allErrorsCountText;

	// Token: 0x04004C4D RID: 19533
	public Text allErrorsCountText2;

	// Token: 0x04004C4E RID: 19534
	public Transform errorSplashTransform;

	// Token: 0x04004C4F RID: 19535
	protected float errorSplashTimeRemaining;

	// Token: 0x04004C50 RID: 19536
	protected float errorSplashTime = 5f;

	// Token: 0x04004C51 RID: 19537
	public Transform msgLogPanel;

	// Token: 0x04004C52 RID: 19538
	protected string msgLog;

	// Token: 0x04004C53 RID: 19539
	public InputField allMessagesInputField;

	// Token: 0x04004C54 RID: 19540
	public InputField allMessagesInputField2;

	// Token: 0x04004C55 RID: 19541
	public Text allMessagesCountText;

	// Token: 0x04004C56 RID: 19542
	public Text allMessagesCountText2;

	// Token: 0x04004C57 RID: 19543
	public Transform msgSplashTransform;

	// Token: 0x04004C58 RID: 19544
	protected float msgSplashTimeRemaining;

	// Token: 0x04004C59 RID: 19545
	protected float msgSplashTime = 5f;

	// Token: 0x04004C5A RID: 19546
	public Transform normalAlertRoot;

	// Token: 0x04004C5B RID: 19547
	public Transform worldAlertRoot;

	// Token: 0x04004C5C RID: 19548
	public GameObject okAlertPrefab;

	// Token: 0x04004C5D RID: 19549
	public GameObject okAndCancelAlertPrefab;

	// Token: 0x04004C5E RID: 19550
	protected int _errorCount;

	// Token: 0x04004C5F RID: 19551
	protected bool errorLogDirty;

	// Token: 0x04004C60 RID: 19552
	protected int maxLength = 10000;

	// Token: 0x04004C61 RID: 19553
	public Toggle openMainHUDOnErrorToggle;

	// Token: 0x04004C62 RID: 19554
	[SerializeField]
	private bool _openMainHUDOnError = true;

	// Token: 0x04004C63 RID: 19555
	protected int _msgCount;

	// Token: 0x04004C64 RID: 19556
	protected bool msgLogDirty;

	// Token: 0x04004C65 RID: 19557
	protected bool hasPendingErrorSplash;

	// Token: 0x04004C66 RID: 19558
	public float maxAngularVelocity = 20f;

	// Token: 0x04004C67 RID: 19559
	public float maxDepenetrationVelocity = 1f;

	// Token: 0x04004C68 RID: 19560
	protected List<AsyncFlag> waitResumeSimulationFlags;

	// Token: 0x04004C69 RID: 19561
	protected bool _resetSimulation;

	// Token: 0x04004C6A RID: 19562
	public Transform waitTransform;

	// Token: 0x04004C6B RID: 19563
	public Text[] waitReasonTexts;

	// Token: 0x04004C6C RID: 19564
	protected int pauseFrames;

	// Token: 0x04004C6D RID: 19565
	protected bool hideWaitTransform;

	// Token: 0x04004C6E RID: 19566
	protected bool hiddenReset;

	// Token: 0x04004C6F RID: 19567
	protected bool readyToResumeSimulation;

	// Token: 0x04004C70 RID: 19568
	protected List<AsyncFlag> removeSimulationFlags;

	// Token: 0x04004C71 RID: 19569
	protected AsyncFlag resetSimulationTimerFlag;

	// Token: 0x04004C72 RID: 19570
	protected List<AsyncFlag> waitResumeAutoSimulationFlags;

	// Token: 0x04004C73 RID: 19571
	protected bool _autoSimulation = true;

	// Token: 0x04004C74 RID: 19572
	protected List<AsyncFlag> removeAutoSimulationFlags;

	// Token: 0x04004C75 RID: 19573
	protected AsyncFlag pauseAutoSimulationFlag;

	// Token: 0x04004C76 RID: 19574
	protected bool _pauseAutoSimulation;

	// Token: 0x04004C77 RID: 19575
	public Toggle pauseAutoSimulationToggle;

	// Token: 0x04004C78 RID: 19576
	protected List<AsyncFlag> waitResumeRenderFlags;

	// Token: 0x04004C79 RID: 19577
	protected bool _render = true;

	// Token: 0x04004C7A RID: 19578
	protected List<AsyncFlag> removeRenderFlags;

	// Token: 0x04004C7B RID: 19579
	protected AsyncFlag pauseRenderFlag;

	// Token: 0x04004C7C RID: 19580
	protected bool _pauseRender;

	// Token: 0x04004C7D RID: 19581
	public Toggle pauseRenderToggle;

	// Token: 0x04004C7E RID: 19582
	protected List<AsyncFlag> removeHoldFlags;

	// Token: 0x04004C7F RID: 19583
	protected List<AsyncFlag> holdLoadCompleteFlags;

	// Token: 0x04004C80 RID: 19584
	public Transform loadingIcon;

	// Token: 0x04004C81 RID: 19585
	protected List<AsyncFlag> removeLoadFlags;

	// Token: 0x04004C82 RID: 19586
	protected List<AsyncFlag> loadingIconFlags;

	// Token: 0x04004C83 RID: 19587
	public Toggle autoFreezeAnimationOnSwitchToEditModeToggle;

	// Token: 0x04004C84 RID: 19588
	protected bool _autoFreezeAnimationOnSwitchToEditMode;

	// Token: 0x04004C85 RID: 19589
	public Toggle freezeAnimationToggle;

	// Token: 0x04004C86 RID: 19590
	public Toggle freezeAnimationToggleAlt;

	// Token: 0x04004C87 RID: 19591
	private bool _freezeAnimation;

	// Token: 0x04004C88 RID: 19592
	public Transform worldUI;

	// Token: 0x04004C89 RID: 19593
	public Transform topWorldUI;

	// Token: 0x04004C8A RID: 19594
	public HUDAnchor worldUIAnchor;

	// Token: 0x04004C8B RID: 19595
	public Transform mainHUD;

	// Token: 0x04004C8C RID: 19596
	public Transform mainMenuUI;

	// Token: 0x04004C8D RID: 19597
	public UITabSelector mainMenuTabSelector;

	// Token: 0x04004C8E RID: 19598
	public UITabSelector userPrefsTabSelector;

	// Token: 0x04004C8F RID: 19599
	public Transform loadingUI;

	// Token: 0x04004C90 RID: 19600
	public Transform loadingUIAlt;

	// Token: 0x04004C91 RID: 19601
	public Transform loadingGeometry;

	// Token: 0x04004C92 RID: 19602
	public Slider loadingProgressSlider;

	// Token: 0x04004C93 RID: 19603
	public Slider loadingProgressSliderAlt;

	// Token: 0x04004C94 RID: 19604
	public Text loadingTextStatus;

	// Token: 0x04004C95 RID: 19605
	public Text loadingTextStatusAlt;

	// Token: 0x04004C96 RID: 19606
	public Transform sceneControlUI;

	// Token: 0x04004C97 RID: 19607
	public Transform sceneControlUIAlt;

	// Token: 0x04004C98 RID: 19608
	public Transform wizardWorldUI;

	// Token: 0x04004C99 RID: 19609
	public FileBrowser fileBrowserUI;

	// Token: 0x04004C9A RID: 19610
	public FileBrowser fileBrowserWorldUI;

	// Token: 0x04004C9B RID: 19611
	public FileBrowser templatesFileBrowserWorldUI;

	// Token: 0x04004C9C RID: 19612
	public FileBrowser mediaFileBrowserUI;

	// Token: 0x04004C9D RID: 19613
	public FileBrowser directoryBrowserUI;

	// Token: 0x04004C9E RID: 19614
	public MultiButtonPanel multiButtonPanel;

	// Token: 0x04004C9F RID: 19615
	public VRWebBrowser onlineBrowser;

	// Token: 0x04004CA0 RID: 19616
	public Transform onlineBrowserUI;

	// Token: 0x04004CA1 RID: 19617
	public HubBrowse hubBrowser;

	// Token: 0x04004CA2 RID: 19618
	public Transform embeddedSceneUI;

	// Token: 0x04004CA3 RID: 19619
	public float targetAlpha;

	// Token: 0x04004CA4 RID: 19620
	public Material rayLineMaterialRight;

	// Token: 0x04004CA5 RID: 19621
	public Material rayLineMaterialLeft;

	// Token: 0x04004CA6 RID: 19622
	private bool drawRayLineLeft;

	// Token: 0x04004CA7 RID: 19623
	private bool drawRayLineRight;

	// Token: 0x04004CA8 RID: 19624
	private LineDrawer rayLineDrawerRight;

	// Token: 0x04004CA9 RID: 19625
	private LineDrawer rayLineDrawerLeft;

	// Token: 0x04004CAA RID: 19626
	public LineRenderer rayLineRight;

	// Token: 0x04004CAB RID: 19627
	public LineRenderer rayLineLeft;

	// Token: 0x04004CAC RID: 19628
	public float rayLineWidth = 0.004f;

	// Token: 0x04004CAD RID: 19629
	public Toggle quickSelectAlignToggle;

	// Token: 0x04004CAE RID: 19630
	public Toggle quickSelectMoveToggle;

	// Token: 0x04004CAF RID: 19631
	public Toggle quickSelectOpenUIToggle;

	// Token: 0x04004CB0 RID: 19632
	public Toggle selectAtomOnAddToggle;

	// Token: 0x04004CB1 RID: 19633
	public Toggle focusAtomOnAddToggle;

	// Token: 0x04004CB2 RID: 19634
	public UIPopup selectAtomPopup;

	// Token: 0x04004CB3 RID: 19635
	public UIPopup selectControllerPopup;

	// Token: 0x04004CB4 RID: 19636
	public Material twoStageLineMaterial;

	// Token: 0x04004CB5 RID: 19637
	private LineDrawer rightTwoStageLineDrawer;

	// Token: 0x04004CB6 RID: 19638
	private LineDrawer leftTwoStageLineDrawer;

	// Token: 0x04004CB7 RID: 19639
	private LineDrawer headTwoStageLineDrawer;

	// Token: 0x04004CB8 RID: 19640
	private LineDrawer leapRightTwoStageLineDrawer;

	// Token: 0x04004CB9 RID: 19641
	private LineDrawer leapLeftTwoStageLineDrawer;

	// Token: 0x04004CBA RID: 19642
	private LineDrawer tracker1TwoStageLineDrawer;

	// Token: 0x04004CBB RID: 19643
	private LineDrawer tracker2TwoStageLineDrawer;

	// Token: 0x04004CBC RID: 19644
	private LineDrawer tracker3TwoStageLineDrawer;

	// Token: 0x04004CBD RID: 19645
	private LineDrawer tracker4TwoStageLineDrawer;

	// Token: 0x04004CBE RID: 19646
	private LineDrawer tracker5TwoStageLineDrawer;

	// Token: 0x04004CBF RID: 19647
	private LineDrawer tracker6TwoStageLineDrawer;

	// Token: 0x04004CC0 RID: 19648
	private LineDrawer tracker7TwoStageLineDrawer;

	// Token: 0x04004CC1 RID: 19649
	private LineDrawer tracker8TwoStageLineDrawer;

	// Token: 0x04004CC2 RID: 19650
	public string version = "UNOFFICIAL";

	// Token: 0x04004CC3 RID: 19651
	public Text versionText;

	// Token: 0x04004CC4 RID: 19652
	public int oldestMajorVersion = 20;

	// Token: 0x04004CC5 RID: 19653
	protected string resolvedVersion;

	// Token: 0x04004CC6 RID: 19654
	public string[] specificMilestoneVersionDefines;

	// Token: 0x04004CC7 RID: 19655
	protected List<string> resolvedVersionDefines;

	// Token: 0x04004CC8 RID: 19656
	protected bool foundVersion;

	// Token: 0x04004CC9 RID: 19657
	protected string lastCycleSelectAtomType;

	// Token: 0x04004CCA RID: 19658
	protected string lastCycleSelectAtomUid;

	// Token: 0x04004CCB RID: 19659
	public UIPopup alignRotationOffsetPopup;

	// Token: 0x04004CCC RID: 19660
	protected SuperController.AlignRotationOffset _alignRotationOffset = SuperController.AlignRotationOffset.Left;

	// Token: 0x04004CCD RID: 19661
	public Toggle worldUIShowOverlaySkyToggle;

	// Token: 0x04004CCE RID: 19662
	protected bool _worldUIShowOverlaySky = true;

	// Token: 0x04004CCF RID: 19663
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private bool <worldUIActivated>k__BackingField;

	// Token: 0x04004CD0 RID: 19664
	protected AsyncFlag worldUIActiveFlag;

	// Token: 0x04004CD1 RID: 19665
	protected float _worldUIMonitorAnchorDistance = 2.5f;

	// Token: 0x04004CD2 RID: 19666
	protected float _worldUIMonitorAnchorHeight = 0.8f;

	// Token: 0x04004CD3 RID: 19667
	public Slider worldUIVRAnchorDistanceSlider;

	// Token: 0x04004CD4 RID: 19668
	protected float _worldUIVRAnchorDistance = 2f;

	// Token: 0x04004CD5 RID: 19669
	public Slider worldUIVRAnchorHeightSlider;

	// Token: 0x04004CD6 RID: 19670
	protected float _worldUIVRAnchorHeight = 0.8f;

	// Token: 0x04004CD7 RID: 19671
	private SuperController.ActiveUI _lastActiveUI;

	// Token: 0x04004CD8 RID: 19672
	private Transform customUI;

	// Token: 0x04004CD9 RID: 19673
	public Transform alternateCustomUI;

	// Token: 0x04004CDA RID: 19674
	private SuperController.ActiveUI _activeUI = SuperController.ActiveUI.SelectedOptions;

	// Token: 0x04004CDB RID: 19675
	public Toggle helpToggle;

	// Token: 0x04004CDC RID: 19676
	public Toggle helpToggleAlt;

	// Token: 0x04004CDD RID: 19677
	public Transform helpOverlayOVR;

	// Token: 0x04004CDE RID: 19678
	public Transform helpOverlayVive;

	// Token: 0x04004CDF RID: 19679
	private bool _helpOverlayOnAux = true;

	// Token: 0x04004CE0 RID: 19680
	[SerializeField]
	private bool _helpOverlayOn = true;

	// Token: 0x04004CE1 RID: 19681
	protected string tempHelpText;

	// Token: 0x04004CE2 RID: 19682
	public Text helpHUDText;

	// Token: 0x04004CE3 RID: 19683
	protected string _helpText;

	// Token: 0x04004CE4 RID: 19684
	protected Color _helpColor;

	// Token: 0x04004CE5 RID: 19685
	public Transform leftHand;

	// Token: 0x04004CE6 RID: 19686
	public Transform leftHandAlternate;

	// Token: 0x04004CE7 RID: 19687
	public Transform rightHand;

	// Token: 0x04004CE8 RID: 19688
	public Transform rightHandAlternate;

	// Token: 0x04004CE9 RID: 19689
	public HandModelControl commonHandModelControl;

	// Token: 0x04004CEA RID: 19690
	public HandModelControl alternateControllerHandModelControl;

	// Token: 0x04004CEB RID: 19691
	private HandControl leftHandControl;

	// Token: 0x04004CEC RID: 19692
	private HandControl rightHandControl;

	// Token: 0x04004CED RID: 19693
	public Transform handsContainer;

	// Token: 0x04004CEE RID: 19694
	public OVRHandInput ovrHandInputLeft;

	// Token: 0x04004CEF RID: 19695
	public OVRHandInput ovrHandInputRight;

	// Token: 0x04004CF0 RID: 19696
	public SteamVRHandInput steamVRHandInputLeft;

	// Token: 0x04004CF1 RID: 19697
	public SteamVRHandInput steamVRHandInputRight;

	// Token: 0x04004CF2 RID: 19698
	public Toggle alwaysUseAlternateHandsToggle;

	// Token: 0x04004CF3 RID: 19699
	[SerializeField]
	protected bool _alwaysUseAlternateHands;

	// Token: 0x04004CF4 RID: 19700
	public Transform mouseGrab;

	// Token: 0x04004CF5 RID: 19701
	public Camera leftControllerCamera;

	// Token: 0x04004CF6 RID: 19702
	public Camera rightControllerCamera;

	// Token: 0x04004CF7 RID: 19703
	private FreeControllerV3 rightGrabbedController;

	// Token: 0x04004CF8 RID: 19704
	private bool rightGrabbedControllerIsRemote;

	// Token: 0x04004CF9 RID: 19705
	private FreeControllerV3 leftGrabbedController;

	// Token: 0x04004CFA RID: 19706
	private bool leftGrabbedControllerIsRemote;

	// Token: 0x04004CFB RID: 19707
	private FreeControllerV3 rightFullGrabbedController;

	// Token: 0x04004CFC RID: 19708
	private bool rightFullGrabbedControllerIsRemote;

	// Token: 0x04004CFD RID: 19709
	private FreeControllerV3 leftFullGrabbedController;

	// Token: 0x04004CFE RID: 19710
	private bool leftFullGrabbedControllerIsRemote;

	// Token: 0x04004CFF RID: 19711
	public Transform worldScaleTransform;

	// Token: 0x04004D00 RID: 19712
	public Slider worldScaleSlider;

	// Token: 0x04004D01 RID: 19713
	public Slider worldScaleSliderAlt;

	// Token: 0x04004D02 RID: 19714
	private float _worldScale = 1f;

	// Token: 0x04004D03 RID: 19715
	public Slider controllerScaleSlider;

	// Token: 0x04004D04 RID: 19716
	private float _controllerScale = 1f;

	// Token: 0x04004D05 RID: 19717
	public Toggle useLegacyWorldScaleChangeToggle;

	// Token: 0x04004D06 RID: 19718
	[SerializeField]
	protected bool _useLegacyWorldScaleChange;

	// Token: 0x04004D07 RID: 19719
	public LayerMask targetColliderMask;

	// Token: 0x04004D08 RID: 19720
	public Transform selectPrefab;

	// Token: 0x04004D09 RID: 19721
	public Mesh selectPrefabStandardMesh;

	// Token: 0x04004D0A RID: 19722
	public float selectPrefabStandardScale = 0.5f;

	// Token: 0x04004D0B RID: 19723
	public LayerMask selectColliderMask;

	// Token: 0x04004D0C RID: 19724
	private FreeControllerV3 selectedController;

	// Token: 0x04004D0D RID: 19725
	public FCPositionHandle selectedControllerPositionHandle;

	// Token: 0x04004D0E RID: 19726
	public FCRotationHandle selectedControllerRotationHandle;

	// Token: 0x04004D0F RID: 19727
	public Text selectedControllerNameDisplay;

	// Token: 0x04004D10 RID: 19728
	private SuperController.SelectMode selectMode;

	// Token: 0x04004D11 RID: 19729
	public Transform selectionHUDTransform;

	// Token: 0x04004D12 RID: 19730
	private SelectionHUD selectionHUD;

	// Token: 0x04004D13 RID: 19731
	public SelectionHUD mouseSelectionHUD;

	// Token: 0x04004D14 RID: 19732
	private List<FreeControllerV3> highlightedControllersLook;

	// Token: 0x04004D15 RID: 19733
	private List<SelectTarget> highlightedSelectTargetsLook;

	// Token: 0x04004D16 RID: 19734
	private List<FreeControllerV3> highlightedControllersMouse;

	// Token: 0x04004D17 RID: 19735
	private List<SelectTarget> highlightedSelectTargetsMouse;

	// Token: 0x04004D18 RID: 19736
	public Transform rightSelectionHUDTransform;

	// Token: 0x04004D19 RID: 19737
	public Transform leftSelectionHUDTransform;

	// Token: 0x04004D1A RID: 19738
	private SelectionHUD rightSelectionHUD;

	// Token: 0x04004D1B RID: 19739
	private SelectionHUD leftSelectionHUD;

	// Token: 0x04004D1C RID: 19740
	private List<FreeControllerV3> highlightedControllersLeft;

	// Token: 0x04004D1D RID: 19741
	private List<FreeControllerV3> highlightedControllersRight;

	// Token: 0x04004D1E RID: 19742
	private List<SelectTarget> highlightedSelectTargetsLeft;

	// Token: 0x04004D1F RID: 19743
	private List<SelectTarget> highlightedSelectTargetsRight;

	// Token: 0x04004D20 RID: 19744
	private HashSet<FreeControllerV3> onlyShowControllers;

	// Token: 0x04004D21 RID: 19745
	protected bool cursorLockedLastFrame;

	// Token: 0x04004D22 RID: 19746
	private List<Transform> selectionInstances;

	// Token: 0x04004D23 RID: 19747
	private SuperController.SelectControllerCallback selectControllerCallback;

	// Token: 0x04004D24 RID: 19748
	private SuperController.SelectForceProducerCallback selectForceProducerCallback;

	// Token: 0x04004D25 RID: 19749
	private SuperController.SelectForceReceiverCallback selectForceReceiverCallback;

	// Token: 0x04004D26 RID: 19750
	private SuperController.SelectRigidbodyCallback selectRigidbodyCallback;

	// Token: 0x04004D27 RID: 19751
	private SuperController.SelectAtomCallback selectAtomCallback;

	// Token: 0x04004D28 RID: 19752
	public SteamVR_Action_Boolean menuAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("Menu", false);

	// Token: 0x04004D29 RID: 19753
	public SteamVR_Action_Boolean UIInteractAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("UIInteract", false);

	// Token: 0x04004D2A RID: 19754
	public SteamVR_Action_Boolean targetShowAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("TargetShow", false);

	// Token: 0x04004D2B RID: 19755
	public SteamVR_Action_Boolean cycleEngageAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("CycleEngage", false);

	// Token: 0x04004D2C RID: 19756
	public SteamVR_Action_Vector2 cycleUsingXAxisAction = SteamVR_Input.GetAction<SteamVR_Action_Vector2>("CycleUsingXAxis", false);

	// Token: 0x04004D2D RID: 19757
	public SteamVR_Action_Vector2 cycleUsingYAxisAction = SteamVR_Input.GetAction<SteamVR_Action_Vector2>("CycleUsingYAxis", false);

	// Token: 0x04004D2E RID: 19758
	public SteamVR_Action_Boolean selectAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("Select", false);

	// Token: 0x04004D2F RID: 19759
	public SteamVR_Action_Vector2 pushPullAction = SteamVR_Input.GetAction<SteamVR_Action_Vector2>("PushPullNode", false);

	// Token: 0x04004D30 RID: 19760
	public SteamVR_Action_Boolean teleportShowAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("TeleportShow", false);

	// Token: 0x04004D31 RID: 19761
	public SteamVR_Action_Boolean teleportAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("Teleport", false);

	// Token: 0x04004D32 RID: 19762
	public SteamVR_Action_Boolean grabNavigateAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabNavigate", false);

	// Token: 0x04004D33 RID: 19763
	public SteamVR_Action_Vector2 freeMoveAction = SteamVR_Input.GetAction<SteamVR_Action_Vector2>("FreeMove", false);

	// Token: 0x04004D34 RID: 19764
	public SteamVR_Action_Vector2 freeModeMoveAction = SteamVR_Input.GetAction<SteamVR_Action_Vector2>("FreeModeMove", false);

	// Token: 0x04004D35 RID: 19765
	public SteamVR_Action_Boolean swapFreeMoveAxis = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("SwapFreeMoveAxis", false);

	// Token: 0x04004D36 RID: 19766
	public SteamVR_Action_Boolean grabAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("Grab", false);

	// Token: 0x04004D37 RID: 19767
	public SteamVR_Action_Boolean holdGrabAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("HoldGrab", false);

	// Token: 0x04004D38 RID: 19768
	public SteamVR_Action_Single grabValAction = SteamVR_Input.GetAction<SteamVR_Action_Single>("GrabVal", false);

	// Token: 0x04004D39 RID: 19769
	public SteamVR_Action_Boolean remoteGrabAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("RemoteGrab", false);

	// Token: 0x04004D3A RID: 19770
	public SteamVR_Action_Boolean remoteHoldGrabAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("RemoteHoldGrab", false);

	// Token: 0x04004D3B RID: 19771
	public SteamVR_Action_Boolean toggleHandAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("ToggleHand", false);

	// Token: 0x04004D3C RID: 19772
	public SteamVR_Action_Vibration hapticAction = SteamVR_Input.GetAction<SteamVR_Action_Vibration>("Haptic", false);

	// Token: 0x04004D3D RID: 19773
	public bool useLookSelect;

	// Token: 0x04004D3E RID: 19774
	public Camera lookCamera;

	// Token: 0x04004D3F RID: 19775
	public string buttonToggleTargets = "ButtonBack";

	// Token: 0x04004D40 RID: 19776
	private bool targetsOnWithButton;

	// Token: 0x04004D41 RID: 19777
	public string buttonSelect = "ButtonA";

	// Token: 0x04004D42 RID: 19778
	public string buttonUnselect = "ButtonB";

	// Token: 0x04004D43 RID: 19779
	public string buttonToggleRotateMode = "ButtonY";

	// Token: 0x04004D44 RID: 19780
	public string buttonCycleSelection = "ButtonX";

	// Token: 0x04004D45 RID: 19781
	public JoystickControl.Axis navigationForwardAxis = JoystickControl.Axis.LeftStickY;

	// Token: 0x04004D46 RID: 19782
	public bool invertNavigationForwardAxis;

	// Token: 0x04004D47 RID: 19783
	public JoystickControl.Axis navigationSideAxis = JoystickControl.Axis.LeftStickX;

	// Token: 0x04004D48 RID: 19784
	public bool invertNavigationSideAxis;

	// Token: 0x04004D49 RID: 19785
	public JoystickControl.Axis navigationTurnAxis = JoystickControl.Axis.RightStickX;

	// Token: 0x04004D4A RID: 19786
	public bool invertNavigationTurnAxis;

	// Token: 0x04004D4B RID: 19787
	public JoystickControl.Axis navigationUpAxis = JoystickControl.Axis.RightStickY;

	// Token: 0x04004D4C RID: 19788
	public bool invertNavigationUpAxis;

	// Token: 0x04004D4D RID: 19789
	public bool invertAxis1 = true;

	// Token: 0x04004D4E RID: 19790
	public bool invertAxis2 = true;

	// Token: 0x04004D4F RID: 19791
	public bool invertAxis3 = true;

	// Token: 0x04004D50 RID: 19792
	private bool _swapAxis;

	// Token: 0x04004D51 RID: 19793
	private bool leftGUIInteract;

	// Token: 0x04004D52 RID: 19794
	private bool rightGUIInteract;

	// Token: 0x04004D53 RID: 19795
	private bool _setLeftSelect;

	// Token: 0x04004D54 RID: 19796
	private bool _setRightSelect;

	// Token: 0x04004D55 RID: 19797
	protected bool remoteHoldGrabDisabled;

	// Token: 0x04004D56 RID: 19798
	public LayerMask lookAtTriggerMask;

	// Token: 0x04004D57 RID: 19799
	private LookAtTrigger currentLookAtTrigger;

	// Token: 0x04004D58 RID: 19800
	private bool _pointerModeLeft;

	// Token: 0x04004D59 RID: 19801
	private bool _pointerModeRight;

	// Token: 0x04004D5A RID: 19802
	protected Dictionary<FreeControllerV3, bool> wasHitFC;

	// Token: 0x04004D5B RID: 19803
	protected List<FreeControllerV3> overlappingFcs;

	// Token: 0x04004D5C RID: 19804
	protected List<FreeControllerV3> alreadyDisplayed;

	// Token: 0x04004D5D RID: 19805
	protected Collider[] overlappingControls;

	// Token: 0x04004D5E RID: 19806
	protected bool _allowGrabPlusTriggerHandToggle = true;

	// Token: 0x04004D5F RID: 19807
	public Toggle allowGrabPlusTriggerHandToggleToggle;

	// Token: 0x04004D60 RID: 19808
	private float cycleClick = 0.25f;

	// Token: 0x04004D61 RID: 19809
	private bool _leftCycleOn;

	// Token: 0x04004D62 RID: 19810
	private float _leftCycleXPosition;

	// Token: 0x04004D63 RID: 19811
	private float _leftCycleYPosition;

	// Token: 0x04004D64 RID: 19812
	private int leftCycleX;

	// Token: 0x04004D65 RID: 19813
	private int leftCycleY;

	// Token: 0x04004D66 RID: 19814
	private bool _rightCycleOn;

	// Token: 0x04004D67 RID: 19815
	private float _rightCycleXPosition;

	// Token: 0x04004D68 RID: 19816
	private float _rightCycleYPosition;

	// Token: 0x04004D69 RID: 19817
	private int rightCycleX;

	// Token: 0x04004D6A RID: 19818
	private int rightCycleY;

	// Token: 0x04004D6B RID: 19819
	private bool isLeftOverlap;

	// Token: 0x04004D6C RID: 19820
	private bool isRightOverlap;

	// Token: 0x04004D6D RID: 19821
	public GameObject mouseCrosshair;

	// Token: 0x04004D6E RID: 19822
	public GameObject freeMouseMoveModeIndicator;

	// Token: 0x04004D6F RID: 19823
	public RectTransform mouseCrosshairPointer;

	// Token: 0x04004D70 RID: 19824
	private FreeControllerV3 potentialGrabbedControllerMouse;

	// Token: 0x04004D71 RID: 19825
	private FreeControllerV3 grabbedControllerMouse;

	// Token: 0x04004D72 RID: 19826
	private float grabbedControllerMouseDistance;

	// Token: 0x04004D73 RID: 19827
	private Vector3 mouseDownLastWorldPosition;

	// Token: 0x04004D74 RID: 19828
	private bool dragActivated;

	// Token: 0x04004D75 RID: 19829
	private bool mouseClickUsed;

	// Token: 0x04004D76 RID: 19830
	private bool eligibleForMouseSelect;

	// Token: 0x04004D77 RID: 19831
	private Vector3 currentMousePosition;

	// Token: 0x04004D78 RID: 19832
	private Vector3 lastMousePosition;

	// Token: 0x04004D79 RID: 19833
	private Vector3 mouseChange;

	// Token: 0x04004D7A RID: 19834
	private Vector3 mouseChangeScaled;

	// Token: 0x04004D7B RID: 19835
	private float mouseAxisX;

	// Token: 0x04004D7C RID: 19836
	private float mouseAxisY;

	// Token: 0x04004D7D RID: 19837
	private bool useMouseRDPMode = true;

	// Token: 0x04004D7E RID: 19838
	public UIPopup UISidePopup;

	// Token: 0x04004D7F RID: 19839
	[SerializeField]
	protected UISideAlign.Side _UISide = UISideAlign.Side.Right;

	// Token: 0x04004D80 RID: 19840
	public Toggle onStartupSkipStartScreenToggle;

	// Token: 0x04004D81 RID: 19841
	protected bool _onStartupSkipStartScreen;

	// Token: 0x04004D82 RID: 19842
	public Transform mainHUDAttachPoint;

	// Token: 0x04004D83 RID: 19843
	public Transform mainHUDPivot;

	// Token: 0x04004D84 RID: 19844
	public float mainHUDPivotXRotationVR = -30f;

	// Token: 0x04004D85 RID: 19845
	public float mainHUDPivotXRotationMonitor;

	// Token: 0x04004D86 RID: 19846
	private Vector3 mainHUDAttachPointStartingPosition;

	// Token: 0x04004D87 RID: 19847
	private Quaternion mainHUDAttachPointStartingRotation;

	// Token: 0x04004D88 RID: 19848
	public bool showMainHUDOnStart;

	// Token: 0x04004D89 RID: 19849
	private bool _mainHUDVisible;

	// Token: 0x04004D8A RID: 19850
	private bool _mainHUDAnchoredOnMonitor;

	// Token: 0x04004D8B RID: 19851
	private float _monitorRigRightOffsetWhenUIOpen = -0.1f;

	// Token: 0x04004D8C RID: 19852
	private bool GUIhit;

	// Token: 0x04004D8D RID: 19853
	private bool GUIhitLeft;

	// Token: 0x04004D8E RID: 19854
	private bool GUIhitRight;

	// Token: 0x04004D8F RID: 19855
	private bool GUIhitMouse;

	// Token: 0x04004D90 RID: 19856
	public bool overrideCanvasSortingLayer;

	// Token: 0x04004D91 RID: 19857
	public string overrideCanvasSortingLayerName;

	// Token: 0x04004D92 RID: 19858
	protected Dictionary<SelectTarget, bool> wasHitST;

	// Token: 0x04004D93 RID: 19859
	protected RaycastHit[] raycastHits;

	// Token: 0x04004D94 RID: 19860
	private FreeControllerV3 rightPossessedController;

	// Token: 0x04004D95 RID: 19861
	private FreeControllerV3 rightStartPossessedController;

	// Token: 0x04004D96 RID: 19862
	private FreeControllerV3 leftPossessedController;

	// Token: 0x04004D97 RID: 19863
	private FreeControllerV3 leftStartPossessedController;

	// Token: 0x04004D98 RID: 19864
	private FreeControllerV3 headPossessedController;

	// Token: 0x04004D99 RID: 19865
	private FreeControllerV3 headStartPossessedController;

	// Token: 0x04004D9A RID: 19866
	private FreeControllerV3 tracker1PossessedController;

	// Token: 0x04004D9B RID: 19867
	private FreeControllerV3 tracker1StartPossessedController;

	// Token: 0x04004D9C RID: 19868
	private FreeControllerV3 tracker2PossessedController;

	// Token: 0x04004D9D RID: 19869
	private FreeControllerV3 tracker2StartPossessedController;

	// Token: 0x04004D9E RID: 19870
	private FreeControllerV3 tracker3PossessedController;

	// Token: 0x04004D9F RID: 19871
	private FreeControllerV3 tracker3StartPossessedController;

	// Token: 0x04004DA0 RID: 19872
	private FreeControllerV3 tracker4PossessedController;

	// Token: 0x04004DA1 RID: 19873
	private FreeControllerV3 tracker4StartPossessedController;

	// Token: 0x04004DA2 RID: 19874
	private FreeControllerV3 tracker5PossessedController;

	// Token: 0x04004DA3 RID: 19875
	private FreeControllerV3 tracker5StartPossessedController;

	// Token: 0x04004DA4 RID: 19876
	private FreeControllerV3 tracker6PossessedController;

	// Token: 0x04004DA5 RID: 19877
	private FreeControllerV3 tracker6StartPossessedController;

	// Token: 0x04004DA6 RID: 19878
	private FreeControllerV3 tracker7PossessedController;

	// Token: 0x04004DA7 RID: 19879
	private FreeControllerV3 tracker7StartPossessedController;

	// Token: 0x04004DA8 RID: 19880
	private FreeControllerV3 tracker8PossessedController;

	// Token: 0x04004DA9 RID: 19881
	private FreeControllerV3 tracker8StartPossessedController;

	// Token: 0x04004DAA RID: 19882
	private HandControl leftPossessHandControl;

	// Token: 0x04004DAB RID: 19883
	private HandControl leapLeftPossessHandControl;

	// Token: 0x04004DAC RID: 19884
	private HandControl rightPossessHandControl;

	// Token: 0x04004DAD RID: 19885
	private HandControl leapRightPossessHandControl;

	// Token: 0x04004DAE RID: 19886
	public Transform headPossessedActivateTransform;

	// Token: 0x04004DAF RID: 19887
	public Text headPossessedText;

	// Token: 0x04004DB0 RID: 19888
	public Toggle allowHeadPossessMousePanAndZoomToggle;

	// Token: 0x04004DB1 RID: 19889
	private bool _allowHeadPossessMousePanAndZoom;

	// Token: 0x04004DB2 RID: 19890
	public Toggle allowPossessSpringAdjustmentToggle;

	// Token: 0x04004DB3 RID: 19891
	[SerializeField]
	private bool _allowPossessSpringAdjustment = true;

	// Token: 0x04004DB4 RID: 19892
	public Slider possessPositionSpringSlider;

	// Token: 0x04004DB5 RID: 19893
	[SerializeField]
	private float _possessPositionSpring = 10000f;

	// Token: 0x04004DB6 RID: 19894
	public Slider possessRotationSpringSlider;

	// Token: 0x04004DB7 RID: 19895
	[SerializeField]
	private float _possessRotationSpring = 1000f;

	// Token: 0x04004DB8 RID: 19896
	public MotionAnimationMaster motionAnimationMaster;

	// Token: 0x04004DB9 RID: 19897
	protected MotionAnimationMaster currentAnimationMaster;

	// Token: 0x04004DBA RID: 19898
	public bool assetManagerSimulateInEditor = true;

	// Token: 0x04004DBB RID: 19899
	private bool _assetManagerReady;

	// Token: 0x04004DBC RID: 19900
	protected Dictionary<string, GameObject> assetBundleAssetNameToPrefab;

	// Token: 0x04004DBD RID: 19901
	protected Dictionary<string, int> assetBundleAssetNameRefCounts;

	// Token: 0x04004DBE RID: 19902
	private Dictionary<string, bool> uids;

	// Token: 0x04004DBF RID: 19903
	private Dictionary<string, Atom> atoms;

	// Token: 0x04004DC0 RID: 19904
	private List<Atom> atomsList;

	// Token: 0x04004DC1 RID: 19905
	private HashSet<Atom> startingAtoms;

	// Token: 0x04004DC2 RID: 19906
	private bool _pauseSyncAtomLists;

	// Token: 0x04004DC3 RID: 19907
	private List<string> atomUIDs;

	// Token: 0x04004DC4 RID: 19908
	private List<string> atomUIDsWithForceReceivers;

	// Token: 0x04004DC5 RID: 19909
	private List<string> atomUIDsWithForceProducers;

	// Token: 0x04004DC6 RID: 19910
	private List<string> atomUIDsWithRhythmControllers;

	// Token: 0x04004DC7 RID: 19911
	private List<string> atomUIDsWithAudioSourceControls;

	// Token: 0x04004DC8 RID: 19912
	private List<string> atomUIDsWithFreeControllers;

	// Token: 0x04004DC9 RID: 19913
	private List<string> atomUIDsWithRigidbodies;

	// Token: 0x04004DCA RID: 19914
	private List<string> sortedAtomUIDs;

	// Token: 0x04004DCB RID: 19915
	private List<string> sortedAtomUIDsWithForceReceivers;

	// Token: 0x04004DCC RID: 19916
	private List<string> sortedAtomUIDsWithForceProducers;

	// Token: 0x04004DCD RID: 19917
	private List<string> sortedAtomUIDsWithRhythmControllers;

	// Token: 0x04004DCE RID: 19918
	private List<string> sortedAtomUIDsWithAudioSourceControls;

	// Token: 0x04004DCF RID: 19919
	private List<string> sortedAtomUIDsWithFreeControllers;

	// Token: 0x04004DD0 RID: 19920
	private List<string> sortedAtomUIDsWithRigidbodies;

	// Token: 0x04004DD1 RID: 19921
	private List<string> hiddenAtomUIDs;

	// Token: 0x04004DD2 RID: 19922
	private List<string> hiddenAtomUIDsWithFreeControllers;

	// Token: 0x04004DD3 RID: 19923
	private List<string> visibleAtomUIDs;

	// Token: 0x04004DD4 RID: 19924
	private List<string> visibleAtomUIDsWithFreeControllers;

	// Token: 0x04004DD5 RID: 19925
	public RectTransform isolatedSceneEditControlPanel;

	// Token: 0x04004DD6 RID: 19926
	public Text isolatedSubSceneLabel;

	// Token: 0x04004DD7 RID: 19927
	protected SubScene _isolatedSubScene;

	// Token: 0x04004DD8 RID: 19928
	protected HashSet<Atom> atomsInSubSceneHash;

	// Token: 0x04004DD9 RID: 19929
	protected HashSet<Atom> nestedAtomsInSubSceneHash;

	// Token: 0x04004DDA RID: 19930
	public Toggle disableRenderForAtomsNotInIsolatedSubSceneToggle;

	// Token: 0x04004DDB RID: 19931
	protected bool _disableRenderForAtomsNotInIsolatedSubScene;

	// Token: 0x04004DDC RID: 19932
	public Toggle freezePhysicsForAtomsNotInIsolatedSubSceneToggle;

	// Token: 0x04004DDD RID: 19933
	protected bool _freezePhysicsForAtomsNotInIsolatedSubScene;

	// Token: 0x04004DDE RID: 19934
	public Toggle disableCollisionForAtomsNotInIsolatedSubSceneToggle;

	// Token: 0x04004DDF RID: 19935
	protected bool _disableCollisionForAtomsNotInIsolatedSubScene;

	// Token: 0x04004DE0 RID: 19936
	public Button endIsolateEditSubSceneButton;

	// Token: 0x04004DE1 RID: 19937
	public Button quickSaveIsolatedSubSceneButton;

	// Token: 0x04004DE2 RID: 19938
	public Button quickReloadIsolatedSubSceneButton;

	// Token: 0x04004DE3 RID: 19939
	public Button selectIsolatedSubSceneButton;

	// Token: 0x04004DE4 RID: 19940
	public RectTransform isolatedAtomEditControlPanel;

	// Token: 0x04004DE5 RID: 19941
	public Text isolatedAtomLabel;

	// Token: 0x04004DE6 RID: 19942
	protected Atom _isolatedAtom;

	// Token: 0x04004DE7 RID: 19943
	public Toggle disableRenderForAtomsNotInIsolatedAtomToggle;

	// Token: 0x04004DE8 RID: 19944
	protected bool _disableRenderForAtomsNotInIsolatedAtom;

	// Token: 0x04004DE9 RID: 19945
	public Toggle freezePhysicsForAtomsNotInIsolatedAtomToggle;

	// Token: 0x04004DEA RID: 19946
	protected bool _freezePhysicsForAtomsNotInIsolatedAtom;

	// Token: 0x04004DEB RID: 19947
	public Toggle disableCollisionForAtomsNotInIsolatedAtomToggle;

	// Token: 0x04004DEC RID: 19948
	protected bool _disableCollisionForAtomsNotInIsolatedAtom;

	// Token: 0x04004DED RID: 19949
	public Button endIsolateEditAtomButton;

	// Token: 0x04004DEE RID: 19950
	public Button selectIsolatedAtomButton;

	// Token: 0x04004DEF RID: 19951
	public bool sortAtomUIDs = true;

	// Token: 0x04004DF0 RID: 19952
	public Toggle showHiddenAtomsToggle;

	// Token: 0x04004DF1 RID: 19953
	public Toggle showHiddenAtomsToggleAlt;

	// Token: 0x04004DF2 RID: 19954
	protected bool _showHiddenAtoms;

	// Token: 0x04004DF3 RID: 19955
	private List<FreeControllerV3> allControllers;

	// Token: 0x04004DF4 RID: 19956
	private List<AnimationPattern> allAnimationPatterns;

	// Token: 0x04004DF5 RID: 19957
	private List<AnimationStep> allAnimationSteps;

	// Token: 0x04004DF6 RID: 19958
	private List<Animator> allAnimators;

	// Token: 0x04004DF7 RID: 19959
	private List<Canvas> allCanvases;

	// Token: 0x04004DF8 RID: 19960
	private Dictionary<string, ForceReceiver> frMap;

	// Token: 0x04004DF9 RID: 19961
	private Dictionary<string, ForceProducerV2> fpMap;

	// Token: 0x04004DFA RID: 19962
	private Dictionary<string, FreeControllerV3> fcMap;

	// Token: 0x04004DFB RID: 19963
	private Dictionary<string, RhythmController> rcMap;

	// Token: 0x04004DFC RID: 19964
	private Dictionary<string, AudioSourceControl> ascMap;

	// Token: 0x04004DFD RID: 19965
	private Dictionary<string, Rigidbody> rbMap;

	// Token: 0x04004DFE RID: 19966
	private Dictionary<string, GrabPoint> gpMap;

	// Token: 0x04004DFF RID: 19967
	private Dictionary<string, MotionAnimationControl> macMap;

	// Token: 0x04004E00 RID: 19968
	private Dictionary<string, PlayerNavCollider> pncMap;

	// Token: 0x04004E01 RID: 19969
	private int maxUID = 1000;

	// Token: 0x04004E02 RID: 19970
	public SuperController.OnForceReceiverNamesChanged onForceReceiverNamesChangedHandlers;

	// Token: 0x04004E03 RID: 19971
	private string[] _forceReceiverNames;

	// Token: 0x04004E04 RID: 19972
	public SuperController.OnForceProducerNamesChanged onForceProducerNamesChangedHandlers;

	// Token: 0x04004E05 RID: 19973
	private string[] _forceProducerNames;

	// Token: 0x04004E06 RID: 19974
	public SuperController.OnRhythmControllerNamesChanged onRhythmControllerNamesChangedHandlers;

	// Token: 0x04004E07 RID: 19975
	private string[] _rhythmControllerNames;

	// Token: 0x04004E08 RID: 19976
	public SuperController.OnAudioSourceControlNamesChanged onAudioSourceControlNamesChangedHandlers;

	// Token: 0x04004E09 RID: 19977
	private string[] _audioSourceControlNames;

	// Token: 0x04004E0A RID: 19978
	public SuperController.OnFreeControllerNamesChanged onFreeControllerNamesChangedHandlers;

	// Token: 0x04004E0B RID: 19979
	private string[] _freeControllerNames;

	// Token: 0x04004E0C RID: 19980
	public SuperController.OnRigidbodyNamesChanged onRigidbodyNamesChangedHandlers;

	// Token: 0x04004E0D RID: 19981
	private string[] _rigidbodyNames;

	// Token: 0x04004E0E RID: 19982
	public string atomAssetsFile;

	// Token: 0x04004E0F RID: 19983
	public SuperController.AtomAsset[] atomAssets;

	// Token: 0x04004E10 RID: 19984
	public SuperController.AtomAsset[] indirectAtomAssets;

	// Token: 0x04004E11 RID: 19985
	public Atom[] atomPrefabs;

	// Token: 0x04004E12 RID: 19986
	public Atom[] indirectAtomPrefabs;

	// Token: 0x04004E13 RID: 19987
	protected Dictionary<string, Atom> atomPrefabByType;

	// Token: 0x04004E14 RID: 19988
	protected Dictionary<string, SuperController.AtomAsset> atomAssetByType;

	// Token: 0x04004E15 RID: 19989
	protected List<string> atomTypes;

	// Token: 0x04004E16 RID: 19990
	protected List<string> atomCategories;

	// Token: 0x04004E17 RID: 19991
	protected Dictionary<string, List<string>> atomCategoryToAtomTypes;

	// Token: 0x04004E18 RID: 19992
	public string atomCategory;

	// Token: 0x04004E19 RID: 19993
	public UIPopup atomCategoryPopup;

	// Token: 0x04004E1A RID: 19994
	public UIPopup atomPrefabPopup;

	// Token: 0x04004E1B RID: 19995
	public SuperController.OnAtomUIDsChanged onAtomUIDsChangedHandlers;

	// Token: 0x04004E1C RID: 19996
	public SuperController.OnAtomUIDsWithForceReceiversChanged onAtomUIDsWithForceReceiversChangedHandlers;

	// Token: 0x04004E1D RID: 19997
	public SuperController.OnAtomUIDsWithForceProducersChanged onAtomUIDsWithForceProducersChangedHandlers;

	// Token: 0x04004E1E RID: 19998
	public SuperController.OnAtomUIDsWithFreeControllersChanged onAtomUIDsWithFreeControllersChangedHandlers;

	// Token: 0x04004E1F RID: 19999
	public SuperController.OnAtomUIDsWithRigidbodiesChanged onAtomUIDsWithRigidbodiesChangedHandlers;

	// Token: 0x04004E20 RID: 20000
	public SuperController.OnAtomUIDRename onAtomUIDRenameHandlers;

	// Token: 0x04004E21 RID: 20001
	public SuperController.OnAtomParentChanged onAtomParentChangedHandlers;

	// Token: 0x04004E22 RID: 20002
	public SuperController.OnAtomSubSceneChanged onAtomSubSceneChangedHandlers;

	// Token: 0x04004E23 RID: 20003
	public SuperController.OnAtomAdded onAtomAddedHandlers;

	// Token: 0x04004E24 RID: 20004
	public SuperController.OnAtomRemoved onAtomRemovedHandlers;

	// Token: 0x04004E25 RID: 20005
	public SuperController.OnGameModeChanged onGameModeChangedHandlers;

	// Token: 0x04004E26 RID: 20006
	public SuperController.OnSceneLoaded onSceneLoadedHandlers;

	// Token: 0x04004E27 RID: 20007
	public SuperController.OnSubSceneLoaded onSubSceneLoadedHandlers;

	// Token: 0x04004E28 RID: 20008
	public SuperController.OnSceneLoaded onBeforeSceneSaveHandlers;

	// Token: 0x04004E29 RID: 20009
	public SuperController.OnSceneLoaded onSceneSavedHandlers;

	// Token: 0x04004E2A RID: 20010
	protected Atom lastAddedAtom;

	// Token: 0x04004E2B RID: 20011
	public Transform navigationPlayArea;

	// Token: 0x04004E2C RID: 20012
	public Transform regularPlayArea;

	// Token: 0x04004E2D RID: 20013
	public Transform navigationRig;

	// Token: 0x04004E2E RID: 20014
	public Transform navigationRigParent;

	// Token: 0x04004E2F RID: 20015
	public Transform navigationPlayer;

	// Token: 0x04004E30 RID: 20016
	public Transform navigationCamera;

	// Token: 0x04004E31 RID: 20017
	public Transform navigationHologrid;

	// Token: 0x04004E32 RID: 20018
	protected bool navigationHologridVisible;

	// Token: 0x04004E33 RID: 20019
	protected float navigationHologridShowTime;

	// Token: 0x04004E34 RID: 20020
	protected float navigationHologridTransparencyMultiplier = 1f;

	// Token: 0x04004E35 RID: 20021
	public Toggle showNavigationHologridToggle;

	// Token: 0x04004E36 RID: 20022
	[SerializeField]
	private bool _showNavigationHologrid = true;

	// Token: 0x04004E37 RID: 20023
	public Slider hologridTransparencySlider;

	// Token: 0x04004E38 RID: 20024
	[SerializeField]
	private float _hologridTransparency = 0.01f;

	// Token: 0x04004E39 RID: 20025
	protected Vector3 sceneLoadMonitorCameraLocalRotation;

	// Token: 0x04004E3A RID: 20026
	protected Vector3 sceneLoadPosition;

	// Token: 0x04004E3B RID: 20027
	protected Quaternion sceneLoadRotation;

	// Token: 0x04004E3C RID: 20028
	protected float sceneLoadPlayerHeightAdjust;

	// Token: 0x04004E3D RID: 20029
	public Toggle useSceneLoadPositionToggle;

	// Token: 0x04004E3E RID: 20030
	protected bool _useSceneLoadPosition;

	// Token: 0x04004E3F RID: 20031
	public CubicBezierCurve navigationCurve;

	// Token: 0x04004E40 RID: 20032
	public float navigationDistance = 100f;

	// Token: 0x04004E41 RID: 20033
	public bool useLookForNavigation = true;

	// Token: 0x04004E42 RID: 20034
	public LayerMask navigationColliderMask;

	// Token: 0x04004E43 RID: 20035
	public Toggle lockHeightDuringNavigateToggle;

	// Token: 0x04004E44 RID: 20036
	public Toggle lockHeightDuringNavigateToggleAlt;

	// Token: 0x04004E45 RID: 20037
	[SerializeField]
	private bool _lockHeightDuringNavigate = true;

	// Token: 0x04004E46 RID: 20038
	public Toggle disableAllNavigationToggle;

	// Token: 0x04004E47 RID: 20039
	[SerializeField]
	private bool _disableAllNavigation;

	// Token: 0x04004E48 RID: 20040
	public Toggle freeMoveFollowFloorToggle;

	// Token: 0x04004E49 RID: 20041
	public Toggle freeMoveFollowFloorToggleAlt;

	// Token: 0x04004E4A RID: 20042
	[SerializeField]
	private bool _freeMoveFollowFloor = true;

	// Token: 0x04004E4B RID: 20043
	public Toggle teleportAllowRotationToggle;

	// Token: 0x04004E4C RID: 20044
	[SerializeField]
	private bool _teleportAllowRotation;

	// Token: 0x04004E4D RID: 20045
	public Toggle disableTeleportToggle;

	// Token: 0x04004E4E RID: 20046
	[SerializeField]
	private bool _disableTeleport;

	// Token: 0x04004E4F RID: 20047
	public Toggle disableTeleportDuringPossessToggle;

	// Token: 0x04004E50 RID: 20048
	[SerializeField]
	private bool _disableTeleportDuringPossess = true;

	// Token: 0x04004E51 RID: 20049
	public Slider freeMoveMultiplierSlider;

	// Token: 0x04004E52 RID: 20050
	[SerializeField]
	private float _freeMoveMultiplier = 1f;

	// Token: 0x04004E53 RID: 20051
	public Toggle disableGrabNavigationToggle;

	// Token: 0x04004E54 RID: 20052
	[SerializeField]
	private bool _disableGrabNavigation;

	// Token: 0x04004E55 RID: 20053
	public Slider grabNavigationPositionMultiplierSlider;

	// Token: 0x04004E56 RID: 20054
	[SerializeField]
	private float _grabNavigationPositionMultiplier = 1f;

	// Token: 0x04004E57 RID: 20055
	public Slider grabNavigationRotationMultiplierSlider;

	// Token: 0x04004E58 RID: 20056
	[SerializeField]
	private float _grabNavigationRotationMultiplier = 0.5f;

	// Token: 0x04004E59 RID: 20057
	private float _grabNavigationRotationResistance = 0.1f;

	// Token: 0x04004E5A RID: 20058
	private Vector3 startNavigatePosition;

	// Token: 0x04004E5B RID: 20059
	private Vector3 startGrabNavigatePositionRight;

	// Token: 0x04004E5C RID: 20060
	private Vector3 startGrabNavigatePositionLeft;

	// Token: 0x04004E5D RID: 20061
	private bool isGrabNavigatingRight;

	// Token: 0x04004E5E RID: 20062
	private bool isGrabNavigatingLeft;

	// Token: 0x04004E5F RID: 20063
	private Quaternion startNavigateRotation;

	// Token: 0x04004E60 RID: 20064
	private Quaternion startGrabNavigateRotationRight;

	// Token: 0x04004E61 RID: 20065
	private Quaternion startGrabNavigateRotationLeft;

	// Token: 0x04004E62 RID: 20066
	private bool startedTeleport;

	// Token: 0x04004E63 RID: 20067
	private PlayerNavCollider teleportPlayerNavCollider;

	// Token: 0x04004E64 RID: 20068
	private PlayerNavCollider playerNavCollider;

	// Token: 0x04004E65 RID: 20069
	private GameObject playerNavTrackerGO;

	// Token: 0x04004E66 RID: 20070
	public Transform heightAdjustTransform;

	// Token: 0x04004E67 RID: 20071
	public Slider playerHeightAdjustSlider;

	// Token: 0x04004E68 RID: 20072
	public Slider playerHeightAdjustSliderAlt;

	// Token: 0x04004E69 RID: 20073
	private float _playerHeightAdjust;

	// Token: 0x04004E6A RID: 20074
	private Ray castRay;

	// Token: 0x04004E6B RID: 20075
	private MeshRenderer regularPlayAreaMR;

	// Token: 0x04004E6C RID: 20076
	private MeshRenderer navigationPlayAreaMR;

	// Token: 0x04004E6D RID: 20077
	private MeshRenderer navigationPlayerMR;

	// Token: 0x04004E6E RID: 20078
	private MeshRenderer navigationCameraMR;

	// Token: 0x04004E6F RID: 20079
	private bool isTeleporting;

	// Token: 0x04004E70 RID: 20080
	private bool didStartLeftNavigate;

	// Token: 0x04004E71 RID: 20081
	private bool didStartRightNavigate;

	// Token: 0x04004E72 RID: 20082
	public float focusDistance = 1.5f;

	// Token: 0x04004E73 RID: 20083
	public bool disableInternalKeyBindings;

	// Token: 0x04004E74 RID: 20084
	public bool disableInternalNavigationKeyBindings;

	// Token: 0x04004E75 RID: 20085
	private int _solverIterations = 15;

	// Token: 0x04004E76 RID: 20086
	private bool _useInterpolation = true;

	// Token: 0x04004E77 RID: 20087
	public bool disableLeap;

	// Token: 0x04004E78 RID: 20088
	public Transform LeapRig;

	// Token: 0x04004E79 RID: 20089
	public Toggle leapMotionEnabledToggle;

	// Token: 0x04004E7A RID: 20090
	protected bool _leapMotionEnabled;

	// Token: 0x04004E7B RID: 20091
	public LeapXRServiceProvider[] LeapServiceProviders;

	// Token: 0x04004E7C RID: 20092
	public Transform leapHandLeft;

	// Token: 0x04004E7D RID: 20093
	public Transform leapHandRight;

	// Token: 0x04004E7E RID: 20094
	public Transform leapHandMountLeft;

	// Token: 0x04004E7F RID: 20095
	public Transform leapHandMountRight;

	// Token: 0x04004E80 RID: 20096
	public LeapHandModelControl leapHandModelControl;

	// Token: 0x04004E81 RID: 20097
	private FreeControllerV3 leapLeftPossessedController;

	// Token: 0x04004E82 RID: 20098
	private FreeControllerV3 leapLeftStartPossessedController;

	// Token: 0x04004E83 RID: 20099
	private FreeControllerV3 leapRightPossessedController;

	// Token: 0x04004E84 RID: 20100
	private FreeControllerV3 leapRightStartPossessedController;

	// Token: 0x04004E85 RID: 20101
	protected bool _leapHandLeftConnected;

	// Token: 0x04004E86 RID: 20102
	protected bool _leapHandRightConnected;

	// Token: 0x04004E87 RID: 20103
	public Transform viveTracker1;

	// Token: 0x04004E88 RID: 20104
	public SteamVR_RenderModel viveTracker1Model;

	// Token: 0x04004E89 RID: 20105
	public Transform viveTracker2;

	// Token: 0x04004E8A RID: 20106
	public SteamVR_RenderModel viveTracker2Model;

	// Token: 0x04004E8B RID: 20107
	public Transform viveTracker3;

	// Token: 0x04004E8C RID: 20108
	public SteamVR_RenderModel viveTracker3Model;

	// Token: 0x04004E8D RID: 20109
	public Transform viveTracker4;

	// Token: 0x04004E8E RID: 20110
	public SteamVR_RenderModel viveTracker4Model;

	// Token: 0x04004E8F RID: 20111
	public Transform viveTracker5;

	// Token: 0x04004E90 RID: 20112
	public SteamVR_RenderModel viveTracker5Model;

	// Token: 0x04004E91 RID: 20113
	public Transform viveTracker6;

	// Token: 0x04004E92 RID: 20114
	public SteamVR_RenderModel viveTracker6Model;

	// Token: 0x04004E93 RID: 20115
	public Transform viveTracker7;

	// Token: 0x04004E94 RID: 20116
	public SteamVR_RenderModel viveTracker7Model;

	// Token: 0x04004E95 RID: 20117
	public Transform viveTracker8;

	// Token: 0x04004E96 RID: 20118
	public SteamVR_RenderModel viveTracker8Model;

	// Token: 0x04004E97 RID: 20119
	protected bool _hideTrackers;

	// Token: 0x04004E98 RID: 20120
	protected bool _tracker1Visible = true;

	// Token: 0x04004E99 RID: 20121
	protected bool _tracker2Visible = true;

	// Token: 0x04004E9A RID: 20122
	protected bool _tracker3Visible = true;

	// Token: 0x04004E9B RID: 20123
	protected bool _tracker4Visible = true;

	// Token: 0x04004E9C RID: 20124
	protected bool _tracker5Visible = true;

	// Token: 0x04004E9D RID: 20125
	protected bool _tracker6Visible = true;

	// Token: 0x04004E9E RID: 20126
	protected bool _tracker7Visible = true;

	// Token: 0x04004E9F RID: 20127
	protected bool _tracker8Visible = true;

	// Token: 0x04004EA0 RID: 20128
	public bool isOVR;

	// Token: 0x04004EA1 RID: 20129
	public bool isOpenVR;

	// Token: 0x04004EA2 RID: 20130
	public CameraTarget centerCameraTarget;

	// Token: 0x04004EA3 RID: 20131
	public GameObject[] centerCameraTargetDisableWhenMonitor;

	// Token: 0x04004EA4 RID: 20132
	public Toggle generateDepthTextureToggle;

	// Token: 0x04004EA5 RID: 20133
	protected bool _generateDepthTexture;

	// Token: 0x04004EA6 RID: 20134
	public Toggle useMonitorRigAudioListenerWhenActiveToggle;

	// Token: 0x04004EA7 RID: 20135
	protected bool _useMonitorRigAudioListenerWhenActive = true;

	// Token: 0x04004EA8 RID: 20136
	protected AudioListener monitorRigAudioListener;

	// Token: 0x04004EA9 RID: 20137
	protected AudioListener ovrRigAudioListener;

	// Token: 0x04004EAA RID: 20138
	protected AudioListener openVRRigAudioListener;

	// Token: 0x04004EAB RID: 20139
	protected LinkedList<AudioListener> additionalAudioListeners;

	// Token: 0x04004EAC RID: 20140
	protected AudioListener overrideAudioListener;

	// Token: 0x04004EAD RID: 20141
	protected AudioListener currentAudioListener;

	// Token: 0x04004EAE RID: 20142
	private bool MonitorRigActive;

	// Token: 0x04004EAF RID: 20143
	private bool isMonitorOnly;

	// Token: 0x04004EB0 RID: 20144
	public Transform MonitorRig;

	// Token: 0x04004EB1 RID: 20145
	public Camera MonitorCenterCamera;

	// Token: 0x04004EB2 RID: 20146
	public Vector3 MonitorCenterCameraOffset = Vector3.zero;

	// Token: 0x04004EB3 RID: 20147
	public Transform MonitorUI;

	// Token: 0x04004EB4 RID: 20148
	public Transform MonitorUIAnchor;

	// Token: 0x04004EB5 RID: 20149
	public Transform MonitorUIAttachPoint;

	// Token: 0x04004EB6 RID: 20150
	public Transform MonitorModeAuxUI;

	// Token: 0x04004EB7 RID: 20151
	public Button MonitorModeButton;

	// Token: 0x04004EB8 RID: 20152
	protected bool _toggleMonitorSaveMainHUDVisible;

	// Token: 0x04004EB9 RID: 20153
	protected bool _monitorUIVisible = true;

	// Token: 0x04004EBA RID: 20154
	public Slider monitorUIScaleSlider;

	// Token: 0x04004EBB RID: 20155
	public float fixedMonitorUIScale = 1f;

	// Token: 0x04004EBC RID: 20156
	private float _monitorUIScale = 1f;

	// Token: 0x04004EBD RID: 20157
	public Slider monitorUIYOffsetSlider;

	// Token: 0x04004EBE RID: 20158
	private float _monitorUIYOffset;

	// Token: 0x04004EBF RID: 20159
	public Slider monitorCameraFOVSlider;

	// Token: 0x04004EC0 RID: 20160
	[SerializeField]
	private float _monitorCameraFOV = 40f;

	// Token: 0x04004EC1 RID: 20161
	private Transform saveCenterEyeAttachPoint;

	// Token: 0x04004EC2 RID: 20162
	public Transform OVRRig;

	// Token: 0x04004EC3 RID: 20163
	public Camera OVRCenterCamera;

	// Token: 0x04004EC4 RID: 20164
	public Transform touchObjectLeft;

	// Token: 0x04004EC5 RID: 20165
	protected MeshRenderer[] touchObjectLeftMeshRenderers;

	// Token: 0x04004EC6 RID: 20166
	public Transform touchHandMountLeft;

	// Token: 0x04004EC7 RID: 20167
	public Transform touchCenterHandLeft;

	// Token: 0x04004EC8 RID: 20168
	public Transform touchObjectRight;

	// Token: 0x04004EC9 RID: 20169
	protected MeshRenderer[] touchObjectRightMeshRenderers;

	// Token: 0x04004ECA RID: 20170
	public Transform touchHandMountRight;

	// Token: 0x04004ECB RID: 20171
	public Transform touchCenterHandRight;

	// Token: 0x04004ECC RID: 20172
	public UIPopup oculusThumbstickFunctionPopup;

	// Token: 0x04004ECD RID: 20173
	[SerializeField]
	protected SuperController.ThumbstickFunction _oculusThumbstickFunction;

	// Token: 0x04004ECE RID: 20174
	public Transform ViveRig;

	// Token: 0x04004ECF RID: 20175
	public Camera ViveCenterCamera;

	// Token: 0x04004ED0 RID: 20176
	public Transform viveObjectLeft;

	// Token: 0x04004ED1 RID: 20177
	public Transform viveHandMountLeft;

	// Token: 0x04004ED2 RID: 20178
	public Transform viveCenterHandLeft;

	// Token: 0x04004ED3 RID: 20179
	public Transform viveObjectRight;

	// Token: 0x04004ED4 RID: 20180
	public Transform viveHandMountRight;

	// Token: 0x04004ED5 RID: 20181
	public Transform viveCenterHandRight;

	// Token: 0x04004ED6 RID: 20182
	protected MD5CryptoServiceProvider md5;

	// Token: 0x04004ED7 RID: 20183
	[SerializeField]
	protected Transform bootstrapPluginContainer;

	// Token: 0x04004ED8 RID: 20184
	protected ScriptDomain bootstrapPluginDomain;

	// Token: 0x04004ED9 RID: 20185
	protected Dictionary<string, List<MVRScriptController>> bootstrapPluginScriptControllers;

	// Token: 0x04004EDA RID: 20186
	public RectTransform vamXPanel;

	// Token: 0x04004EDB RID: 20187
	public Button vamXButton;

	// Token: 0x04004EDC RID: 20188
	public Text vamXButtonText;

	// Token: 0x04004EDD RID: 20189
	public Button vamXTutorialButton;

	// Token: 0x04004EDE RID: 20190
	protected int vamXVersion;

	// Token: 0x04004EDF RID: 20191
	public GameObject[] vamXEnabledGameObjects;

	// Token: 0x04004EE0 RID: 20192
	public GameObject[] vamXEnabledAndAdvancedSceneEditGameObjects;

	// Token: 0x04004EE1 RID: 20193
	public GameObject[] vamXDisabledGameObjects;

	// Token: 0x04004EE2 RID: 20194
	public GameObject[] vamXDisabledAndAdvancedSceneEditGameObjects;

	// Token: 0x04004EE3 RID: 20195
	public GameObject[] vamXDisabledAndAdvancedSceneEditDisabledGameObjects;

	// Token: 0x04004EE4 RID: 20196
	protected bool _vamXWasInstalled;

	// Token: 0x04004EE5 RID: 20197
	protected bool _vamXInstalled;

	// Token: 0x04004EE6 RID: 20198
	protected string lastLoadedvamXBootstrapPath;

	// Token: 0x04004EE7 RID: 20199
	protected string vamXBootstrapPluginPath = "vamX.1.latest:/vamXBootstrap.cs";

	// Token: 0x04004EE8 RID: 20200
	protected string vamXTutorialScene = "vamX.1.latest:/Saves/scene/vamX Tutorials.json";

	// Token: 0x04004EE9 RID: 20201
	protected string vamXMainScene = "vamX.1.latest:/Saves/scene/vamX.json";

	// Token: 0x04004EEA RID: 20202
	[CompilerGenerated]
	private static Comparison<FreeControllerV3> <>f__am$cache0;

	// Token: 0x04004EEB RID: 20203
	[CompilerGenerated]
	private static Comparison<FreeControllerV3> <>f__am$cache1;

	// Token: 0x04004EEC RID: 20204
	[CompilerGenerated]
	private static HubBrowse.EnableHubCallback <>f__am$cache2;

	// Token: 0x04004EED RID: 20205
	[CompilerGenerated]
	private static HubBrowse.EnableWebBrowserCallback <>f__am$cache3;

	// Token: 0x02000C54 RID: 3156
	public enum KeyType
	{
		// Token: 0x04004EEF RID: 20207
		Invalid,
		// Token: 0x04004EF0 RID: 20208
		Free,
		// Token: 0x04004EF1 RID: 20209
		Teaser,
		// Token: 0x04004EF2 RID: 20210
		Entertainer,
		// Token: 0x04004EF3 RID: 20211
		NSteam,
		// Token: 0x04004EF4 RID: 20212
		Steam,
		// Token: 0x04004EF5 RID: 20213
		Retail,
		// Token: 0x04004EF6 RID: 20214
		Creator
	}

	// Token: 0x02000C55 RID: 3157
	// (Invoke) Token: 0x06005EFA RID: 24314
	public delegate void ScreenShotCallback(string imgPath);

	// Token: 0x02000C56 RID: 3158
	public enum GameMode
	{
		// Token: 0x04004EF8 RID: 20216
		Edit,
		// Token: 0x04004EF9 RID: 20217
		Play
	}

	// Token: 0x02000C57 RID: 3159
	public enum DisplayUIChoice
	{
		// Token: 0x04004EFB RID: 20219
		Auto,
		// Token: 0x04004EFC RID: 20220
		Normal,
		// Token: 0x04004EFD RID: 20221
		World
	}

	// Token: 0x02000C58 RID: 3160
	public enum AlignRotationOffset
	{
		// Token: 0x04004EFF RID: 20223
		None,
		// Token: 0x04004F00 RID: 20224
		Right,
		// Token: 0x04004F01 RID: 20225
		Left
	}

	// Token: 0x02000C59 RID: 3161
	public enum ActiveUI
	{
		// Token: 0x04004F03 RID: 20227
		None,
		// Token: 0x04004F04 RID: 20228
		MainMenu,
		// Token: 0x04004F05 RID: 20229
		MainMenuOnly,
		// Token: 0x04004F06 RID: 20230
		SelectedOptions,
		// Token: 0x04004F07 RID: 20231
		MultiButtonPanel,
		// Token: 0x04004F08 RID: 20232
		EmbeddedScenePanel,
		// Token: 0x04004F09 RID: 20233
		OnlineBrowser,
		// Token: 0x04004F0A RID: 20234
		PackageBuilder,
		// Token: 0x04004F0B RID: 20235
		PackageManager,
		// Token: 0x04004F0C RID: 20236
		PackageDownloader,
		// Token: 0x04004F0D RID: 20237
		Custom
	}

	// Token: 0x02000C5A RID: 3162
	public enum SelectMode
	{
		// Token: 0x04004F0F RID: 20239
		Off,
		// Token: 0x04004F10 RID: 20240
		FilteredTargets,
		// Token: 0x04004F11 RID: 20241
		Targets,
		// Token: 0x04004F12 RID: 20242
		Controller,
		// Token: 0x04004F13 RID: 20243
		ForceReceiver,
		// Token: 0x04004F14 RID: 20244
		ForceProducer,
		// Token: 0x04004F15 RID: 20245
		Rigidbody,
		// Token: 0x04004F16 RID: 20246
		Atom,
		// Token: 0x04004F17 RID: 20247
		Possess,
		// Token: 0x04004F18 RID: 20248
		TwoStagePossess,
		// Token: 0x04004F19 RID: 20249
		PossessAndAlign,
		// Token: 0x04004F1A RID: 20250
		Unpossess,
		// Token: 0x04004F1B RID: 20251
		AnimationRecord,
		// Token: 0x04004F1C RID: 20252
		ArmedForRecord,
		// Token: 0x04004F1D RID: 20253
		Teleport,
		// Token: 0x04004F1E RID: 20254
		FreeMove,
		// Token: 0x04004F1F RID: 20255
		FreeMoveMouse,
		// Token: 0x04004F20 RID: 20256
		SaveScreenshot,
		// Token: 0x04004F21 RID: 20257
		Screenshot,
		// Token: 0x04004F22 RID: 20258
		Custom,
		// Token: 0x04004F23 RID: 20259
		CustomWithTargetControl,
		// Token: 0x04004F24 RID: 20260
		CustomWithVRTargetControl
	}

	// Token: 0x02000C5B RID: 3163
	// (Invoke) Token: 0x06005EFE RID: 24318
	public delegate void SelectControllerCallback(FreeControllerV3 fc);

	// Token: 0x02000C5C RID: 3164
	// (Invoke) Token: 0x06005F02 RID: 24322
	public delegate void SelectForceProducerCallback(ForceProducerV2 fp);

	// Token: 0x02000C5D RID: 3165
	// (Invoke) Token: 0x06005F06 RID: 24326
	public delegate void SelectForceReceiverCallback(ForceReceiver fr);

	// Token: 0x02000C5E RID: 3166
	// (Invoke) Token: 0x06005F0A RID: 24330
	public delegate void SelectRigidbodyCallback(Rigidbody rb);

	// Token: 0x02000C5F RID: 3167
	// (Invoke) Token: 0x06005F0E RID: 24334
	public delegate void SelectAtomCallback(Atom a);

	// Token: 0x02000C60 RID: 3168
	// (Invoke) Token: 0x06005F12 RID: 24338
	public delegate void OnForceReceiverNamesChanged(string[] receiverNames);

	// Token: 0x02000C61 RID: 3169
	// (Invoke) Token: 0x06005F16 RID: 24342
	public delegate void OnForceProducerNamesChanged(string[] producerNames);

	// Token: 0x02000C62 RID: 3170
	// (Invoke) Token: 0x06005F1A RID: 24346
	public delegate void OnRhythmControllerNamesChanged(string[] controllerNames);

	// Token: 0x02000C63 RID: 3171
	// (Invoke) Token: 0x06005F1E RID: 24350
	public delegate void OnAudioSourceControlNamesChanged(string[] controlNames);

	// Token: 0x02000C64 RID: 3172
	// (Invoke) Token: 0x06005F22 RID: 24354
	public delegate void OnFreeControllerNamesChanged(string[] controllerNames);

	// Token: 0x02000C65 RID: 3173
	// (Invoke) Token: 0x06005F26 RID: 24358
	public delegate void OnRigidbodyNamesChanged(string[] rigidbodyNames);

	// Token: 0x02000C66 RID: 3174
	[Serializable]
	public class AtomAsset
	{
		// Token: 0x06005F29 RID: 24361 RVA: 0x0023E4E6 File Offset: 0x0023C8E6
		public AtomAsset()
		{
		}

		// Token: 0x04004F25 RID: 20261
		public string assetBundleName;

		// Token: 0x04004F26 RID: 20262
		public string assetName;

		// Token: 0x04004F27 RID: 20263
		public string category;
	}

	// Token: 0x02000C67 RID: 3175
	// (Invoke) Token: 0x06005F2B RID: 24363
	public delegate void OnAtomUIDsChanged(List<string> atomUIDs);

	// Token: 0x02000C68 RID: 3176
	// (Invoke) Token: 0x06005F2F RID: 24367
	public delegate void OnAtomUIDsWithForceReceiversChanged(List<string> atomUIDs);

	// Token: 0x02000C69 RID: 3177
	// (Invoke) Token: 0x06005F33 RID: 24371
	public delegate void OnAtomUIDsWithForceProducersChanged(List<string> atomUIDs);

	// Token: 0x02000C6A RID: 3178
	// (Invoke) Token: 0x06005F37 RID: 24375
	public delegate void OnAtomUIDsWithFreeControllersChanged(List<string> atomUIDs);

	// Token: 0x02000C6B RID: 3179
	// (Invoke) Token: 0x06005F3B RID: 24379
	public delegate void OnAtomUIDsWithRigidbodiesChanged(List<string> atomUIDs);

	// Token: 0x02000C6C RID: 3180
	// (Invoke) Token: 0x06005F3F RID: 24383
	public delegate void OnAtomUIDRename(string oldName, string newName);

	// Token: 0x02000C6D RID: 3181
	// (Invoke) Token: 0x06005F43 RID: 24387
	public delegate void OnAtomParentChanged(Atom atom, Atom newParent);

	// Token: 0x02000C6E RID: 3182
	// (Invoke) Token: 0x06005F47 RID: 24391
	public delegate void OnAtomSubSceneChanged(Atom atom, SubScene newSubScene);

	// Token: 0x02000C6F RID: 3183
	// (Invoke) Token: 0x06005F4B RID: 24395
	public delegate void OnAtomAdded(Atom atom);

	// Token: 0x02000C70 RID: 3184
	// (Invoke) Token: 0x06005F4F RID: 24399
	public delegate void OnAtomRemoved(Atom atom);

	// Token: 0x02000C71 RID: 3185
	// (Invoke) Token: 0x06005F53 RID: 24403
	public delegate void OnGameModeChanged(SuperController.GameMode gameMode);

	// Token: 0x02000C72 RID: 3186
	// (Invoke) Token: 0x06005F57 RID: 24407
	public delegate void OnSceneLoaded();

	// Token: 0x02000C73 RID: 3187
	// (Invoke) Token: 0x06005F5B RID: 24411
	public delegate void OnSubSceneLoaded(SubScene subScene);

	// Token: 0x02000C74 RID: 3188
	// (Invoke) Token: 0x06005F5F RID: 24415
	public delegate void OnBeforeSceneSave();

	// Token: 0x02000C75 RID: 3189
	// (Invoke) Token: 0x06005F63 RID: 24419
	public delegate void OnSceneSaved();

	// Token: 0x02000C76 RID: 3190
	public enum ThumbstickFunction
	{
		// Token: 0x04004F29 RID: 20265
		GrabWorld,
		// Token: 0x04004F2A RID: 20266
		SwapAxis,
		// Token: 0x04004F2B RID: 20267
		Both
	}

	// Token: 0x0200100A RID: 4106
	[CompilerGenerated]
	private sealed class <SyncToKeyFilePackageRefresh>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x060076A0 RID: 30368 RVA: 0x0023E4EE File Offset: 0x0023C8EE
		[DebuggerHidden]
		public <SyncToKeyFilePackageRefresh>c__Iterator0()
		{
		}

		// Token: 0x060076A1 RID: 30369 RVA: 0x0023E4F8 File Offset: 0x0023C8F8
		public bool MoveNext()
		{
			uint num = (uint)this.$PC;
			this.$PC = -1;
			switch (num)
			{
			case 0U:
				if (this.keySyncingIndicator != null)
				{
					this.keySyncingIndicator.gameObject.SetActive(true);
				}
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
			case 1U:
				FileManager.Refresh();
				if (this.keySyncingIndicator != null)
				{
					this.keySyncingIndicator.gameObject.SetActive(false);
				}
				this.$PC = -1;
				break;
			}
			return false;
		}

		// Token: 0x1700118F RID: 4495
		// (get) Token: 0x060076A2 RID: 30370 RVA: 0x0023E5A8 File Offset: 0x0023C9A8
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x17001190 RID: 4496
		// (get) Token: 0x060076A3 RID: 30371 RVA: 0x0023E5B0 File Offset: 0x0023C9B0
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x060076A4 RID: 30372 RVA: 0x0023E5B8 File Offset: 0x0023C9B8
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x060076A5 RID: 30373 RVA: 0x0023E5C8 File Offset: 0x0023C9C8
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x04006A87 RID: 27271
		internal SuperController $this;

		// Token: 0x04006A88 RID: 27272
		internal object $current;

		// Token: 0x04006A89 RID: 27273
		internal bool $disposing;

		// Token: 0x04006A8A RID: 27274
		internal int $PC;
	}

	// Token: 0x0200100B RID: 4107
	[CompilerGenerated]
	private sealed class <SaveFromDialog>c__AnonStorey8
	{
		// Token: 0x060076A6 RID: 30374 RVA: 0x0023E5CF File Offset: 0x0023C9CF
		public <SaveFromDialog>c__AnonStorey8()
		{
		}

		// Token: 0x060076A7 RID: 30375 RVA: 0x0023E5D7 File Offset: 0x0023C9D7
		internal void <>m__0()
		{
			this.$this.overwriteConfirmPanel.gameObject.SetActive(false);
			this.$this.SaveInternal(this.saveName, null, true, true, null, true, false);
		}

		// Token: 0x04006A8B RID: 27275
		internal string saveName;

		// Token: 0x04006A8C RID: 27276
		internal SuperController $this;
	}

	// Token: 0x0200100C RID: 4108
	[CompilerGenerated]
	private sealed class <SaveLegacyPackageFromDialog>c__AnonStorey9
	{
		// Token: 0x060076A8 RID: 30376 RVA: 0x0023E606 File Offset: 0x0023CA06
		public <SaveLegacyPackageFromDialog>c__AnonStorey9()
		{
		}

		// Token: 0x060076A9 RID: 30377 RVA: 0x0023E60E File Offset: 0x0023CA0E
		internal void <>m__0()
		{
			this.$this.overwriteConfirmPanel.gameObject.SetActive(false);
			this.$this.SavePackage(this.saveName, true);
		}

		// Token: 0x04006A8D RID: 27277
		internal string saveName;

		// Token: 0x04006A8E RID: 27278
		internal SuperController $this;
	}

	// Token: 0x0200100D RID: 4109
	[CompilerGenerated]
	private sealed class <SaveAndAddToCurrentPackageFromDialog>c__AnonStoreyA
	{
		// Token: 0x060076AA RID: 30378 RVA: 0x0023E638 File Offset: 0x0023CA38
		public <SaveAndAddToCurrentPackageFromDialog>c__AnonStoreyA()
		{
		}

		// Token: 0x060076AB RID: 30379 RVA: 0x0023E640 File Offset: 0x0023CA40
		internal void <>m__0()
		{
			this.$this.overwriteConfirmPanel.gameObject.SetActive(false);
			this.$this.SaveAndAddToCurrentPackage(this.saveName, true);
		}

		// Token: 0x04006A8F RID: 27279
		internal string saveName;

		// Token: 0x04006A90 RID: 27280
		internal SuperController $this;
	}

	// Token: 0x0200100E RID: 4110
	[CompilerGenerated]
	private sealed class <SaveAndAddToNewPackageFromDialog>c__AnonStoreyB
	{
		// Token: 0x060076AC RID: 30380 RVA: 0x0023E66A File Offset: 0x0023CA6A
		public <SaveAndAddToNewPackageFromDialog>c__AnonStoreyB()
		{
		}

		// Token: 0x060076AD RID: 30381 RVA: 0x0023E672 File Offset: 0x0023CA72
		internal void <>m__0()
		{
			this.$this.overwriteConfirmPanel.gameObject.SetActive(false);
			this.$this.SaveAndAddToNewPackage(this.saveName, true);
		}

		// Token: 0x04006A91 RID: 27281
		internal string saveName;

		// Token: 0x04006A92 RID: 27282
		internal SuperController $this;
	}

	// Token: 0x0200100F RID: 4111
	[CompilerGenerated]
	private sealed class <SaveAndAddToCurrentPackage>c__AnonStoreyC
	{
		// Token: 0x060076AE RID: 30382 RVA: 0x0023E69C File Offset: 0x0023CA9C
		public <SaveAndAddToCurrentPackage>c__AnonStoreyC()
		{
		}

		// Token: 0x060076AF RID: 30383 RVA: 0x0023E6A4 File Offset: 0x0023CAA4
		internal void <>m__0(string A_1)
		{
			this.$this.packageBuilder.AddContentItem(this.saveName);
			this.$this.OpenPackageBuilder();
		}

		// Token: 0x04006A93 RID: 27283
		internal string saveName;

		// Token: 0x04006A94 RID: 27284
		internal SuperController $this;
	}

	// Token: 0x02001010 RID: 4112
	[CompilerGenerated]
	private sealed class <SaveAndAddToNewPackage>c__AnonStoreyD
	{
		// Token: 0x060076B0 RID: 30384 RVA: 0x0023E6C8 File Offset: 0x0023CAC8
		public <SaveAndAddToNewPackage>c__AnonStoreyD()
		{
		}

		// Token: 0x060076B1 RID: 30385 RVA: 0x0023E6D0 File Offset: 0x0023CAD0
		internal void <>m__0(string A_1)
		{
			this.$this.packageBuilder.ClearAll();
			this.$this.packageBuilder.PackageName = Path.GetFileNameWithoutExtension(this.saveName);
			this.$this.packageBuilder.AddContentItem(this.saveName);
			this.$this.OpenPackageBuilder();
		}

		// Token: 0x04006A95 RID: 27285
		internal string saveName;

		// Token: 0x04006A96 RID: 27286
		internal SuperController $this;
	}

	// Token: 0x02001011 RID: 4113
	[CompilerGenerated]
	private sealed class <SaveInternal>c__AnonStoreyE
	{
		// Token: 0x060076B2 RID: 30386 RVA: 0x0023E72A File Offset: 0x0023CB2A
		public <SaveInternal>c__AnonStoreyE()
		{
		}

		// Token: 0x060076B3 RID: 30387 RVA: 0x0023E734 File Offset: 0x0023CB34
		internal void <>m__0()
		{
			try
			{
				this.$this.SaveInternalFinish(this.saveName, this.specificAtom, this.includePhysical, this.includeAppearance, this.callback, this.isOverwrite);
			}
			catch (Exception arg)
			{
				SuperController.LogError("Exception during Save: " + arg);
			}
		}

		// Token: 0x04006A97 RID: 27287
		internal string saveName;

		// Token: 0x04006A98 RID: 27288
		internal Atom specificAtom;

		// Token: 0x04006A99 RID: 27289
		internal bool includePhysical;

		// Token: 0x04006A9A RID: 27290
		internal bool includeAppearance;

		// Token: 0x04006A9B RID: 27291
		internal SuperController.ScreenShotCallback callback;

		// Token: 0x04006A9C RID: 27292
		internal bool isOverwrite;

		// Token: 0x04006A9D RID: 27293
		internal SuperController $this;
	}

	// Token: 0x02001012 RID: 4114
	[CompilerGenerated]
	private sealed class <SaveJSON>c__AnonStoreyF
	{
		// Token: 0x060076B4 RID: 30388 RVA: 0x0023E79C File Offset: 0x0023CB9C
		public <SaveJSON>c__AnonStoreyF()
		{
		}

		// Token: 0x060076B5 RID: 30389 RVA: 0x0023E7A4 File Offset: 0x0023CBA4
		internal void <>m__0()
		{
			try
			{
				this.$this.SaveJSONInternal(this.jc, this.saveName);
			}
			catch (Exception ex)
			{
				if (this.exceptionCallback != null)
				{
					this.exceptionCallback(ex);
					return;
				}
				throw ex;
			}
			if (this.confirmCallback != null)
			{
				this.confirmCallback();
			}
		}

		// Token: 0x04006A9E RID: 27294
		internal JSONClass jc;

		// Token: 0x04006A9F RID: 27295
		internal string saveName;

		// Token: 0x04006AA0 RID: 27296
		internal ExceptionCallback exceptionCallback;

		// Token: 0x04006AA1 RID: 27297
		internal UserActionCallback confirmCallback;

		// Token: 0x04006AA2 RID: 27298
		internal SuperController $this;
	}

	// Token: 0x02001013 RID: 4115
	[CompilerGenerated]
	private sealed class <LoadCo>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x060076B6 RID: 30390 RVA: 0x0023E814 File Offset: 0x0023CC14
		[DebuggerHidden]
		public <LoadCo>c__Iterator1()
		{
		}

		// Token: 0x060076B7 RID: 30391 RVA: 0x0023E81C File Offset: 0x0023CC1C
		public bool MoveNext()
		{
			uint num = (uint)this.$PC;
			this.$PC = -1;
			bool flag = false;
			switch (num)
			{
			case 0U:
				this.hideWaitTransform = loadMerge;
				if (this.loadFlag != null)
				{
					this.loadFlag.Raise();
				}
				this.loadFlag = new AsyncFlag("Scene Load");
				base.ResetSimulation(this.loadFlag, false);
				if (UserPreferences.singleton != null)
				{
					UserPreferences.singleton.pauseGlow = true;
				}
				base.DeactivateWorldUI();
				if (this.loadingUI != null)
				{
					if (!loadMerge)
					{
						if (this.fileBrowserUI == null || this.fileBrowserUI.IsHidden() || !this.fileBrowserUI.keepOpen)
						{
							base.HideMainHUD();
						}
						HUDAnchor.SetAnchorsToReference();
						this.loadingUI.gameObject.SetActive(true);
						if (this.loadingUIAlt != null && !this._mainHUDAnchoredOnMonitor)
						{
							this.loadingUIAlt.gameObject.SetActive(true);
						}
					}
					if (this.loadingGeometry != null)
					{
						this.loadingGeometry.gameObject.SetActive(true);
					}
				}
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
			case 1U:
				base.ResetMonitorCenterCamera();
				if (!loadMerge)
				{
					jc = new JSONClass();
					base.ClearSelection(true);
					base.ClearAllGrabbedControllers();
					base.ClearPossess();
					base.DisconnectNavRigFromPlayerNavCollider();
					if (this.worldScaleSlider != null)
					{
						SliderControl component = this.worldScaleSlider.GetComponent<SliderControl>();
						if (component != null)
						{
							base.worldScale = component.defaultValue;
						}
					}
					if (this.playerHeightAdjustSlider != null)
					{
						SliderControl component2 = this.playerHeightAdjustSlider.GetComponent<SliderControl>();
						if (component2 != null)
						{
							base.playerHeightAdjust = component2.defaultValue;
						}
					}
					atms = new Atom[this.atoms.Count];
					this.atomsList.CopyTo(atms, 0);
					for (int l = 0; l < atms.Length; l++)
					{
						Atom atom = atms[l];
						if (atom != null)
						{
							atom.PreRestore();
						}
					}
					base.RemoveNonStartingAtoms();
					this.$current = null;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				break;
			case 2U:
				enumerator = this.atomsList.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Atom atom2 = enumerator.Current;
						atom2.ClearParentAtom();
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
				enumerator2 = this.atomsList.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						Atom atom3 = enumerator2.Current;
						atom3.RestoreTransform(jc, true);
					}
				}
				finally
				{
					((IDisposable)enumerator2).Dispose();
				}
				enumerator3 = this.atomsList.GetEnumerator();
				try
				{
					while (enumerator3.MoveNext())
					{
						Atom atom4 = enumerator3.Current;
						atom4.RestoreParentAtom(jc);
					}
				}
				finally
				{
					((IDisposable)enumerator3).Dispose();
				}
				FileManager.PushLoadDir(string.Empty, false);
				enumerator4 = this.atomsList.GetEnumerator();
				try
				{
					while (enumerator4.MoveNext())
					{
						Atom atom5 = enumerator4.Current;
						atom5.Restore(jc, true, true, true, null, true, false, true, false);
					}
				}
				finally
				{
					((IDisposable)enumerator4).Dispose();
				}
				FileManager.PopLoadDir();
				enumerator5 = this.atomsList.GetEnumerator();
				try
				{
					while (enumerator5.MoveNext())
					{
						Atom atom6 = enumerator5.Current;
						atom6.LateRestore(jc, true, true, true, false, true, false);
					}
				}
				finally
				{
					((IDisposable)enumerator5).Dispose();
				}
				enumerator6 = this.atomsList.GetEnumerator();
				try
				{
					while (enumerator6.MoveNext())
					{
						Atom atom7 = enumerator6.Current;
						atom7.PostRestore();
					}
				}
				finally
				{
					((IDisposable)enumerator6).Dispose();
				}
				if (UserPreferences.singleton != null && UserPreferences.singleton.optimizeMemoryOnSceneLoad && MemoryOptimizer.singleton != null)
				{
					this.$current = base.StartCoroutine(MemoryOptimizer.singleton.OptimizeMemoryUsage());
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				this.$current = Resources.UnloadUnusedAssets();
				if (!this.$disposing)
				{
					this.$PC = 4;
				}
				return true;
			case 3U:
				break;
			case 4U:
				GC.Collect();
				break;
			case 5U:
				Block_48:
				try
				{
					switch (num)
					{
					case 5U:
						a = base.GetAtomByUid(auid);
						if (a != null)
						{
							a.ResetSimulation(this.loadFlag);
						}
						break;
					default:
						goto IL_CB0;
					}
					IL_C88:
					if (a != null)
					{
						a.SetOn(true);
					}
					base.IncrementLoadingSlider();
					IL_CB0:
					if (enumerator8.MoveNext())
					{
						jatom = (JSONClass)enumerator8.Current;
						auid = jatom["id"];
						type = jatom["type"];
						base.UpdateLoadingStatus("Loading Atom " + auid);
						a = base.GetAtomByUid(auid);
						if (a == null)
						{
							this.$current = base.StartCoroutine(base.AddAtomByType(type, auid, false, false, false));
							if (!this.$disposing)
							{
								this.$PC = 5;
							}
							flag = true;
							return true;
						}
						if (a.type != type)
						{
							base.Error(string.Concat(new string[]
							{
								"Atom ",
								a.name,
								" already exists, but uses different type ",
								a.type,
								" compared to requested ",
								type
							}), true, true);
							goto IL_C88;
						}
						goto IL_C88;
					}
				}
				finally
				{
					if (!flag)
					{
						if ((disposable2 = (enumerator8 as IDisposable)) != null)
						{
							disposable2.Dispose();
						}
					}
				}
				base.UpdateLoadingStatus("Restoring atom contents. Note large save files could take a while...");
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 6;
				}
				return true;
			case 6U:
				Physics.Simulate(0.01f);
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 7;
				}
				return true;
			case 7U:
				enumerator9 = jatoms.GetEnumerator();
				try
				{
					while (enumerator9.MoveNext())
					{
						JSONClass jsonclass = (JSONClass)enumerator9.Current;
						string text = jsonclass["id"];
						string str = jsonclass["type"];
						Atom atomByUid = base.GetAtomByUid(text);
						if (atomByUid != null)
						{
							atomByUid.RestoreTransform(jsonclass, true);
						}
						else
						{
							base.Error("Failed to find atom " + text + " of type " + str, true, true);
						}
					}
				}
				finally
				{
					if ((disposable3 = (enumerator9 as IDisposable)) != null)
					{
						disposable3.Dispose();
					}
				}
				enumerator10 = jatoms.GetEnumerator();
				try
				{
					while (enumerator10.MoveNext())
					{
						JSONClass jsonclass2 = (JSONClass)enumerator10.Current;
						string uid = jsonclass2["id"];
						Atom atomByUid2 = base.GetAtomByUid(uid);
						if (atomByUid2 != null)
						{
							atomByUid2.RestoreParentAtom(jsonclass2);
						}
					}
				}
				finally
				{
					if ((disposable4 = (enumerator10 as IDisposable)) != null)
					{
						disposable4.Dispose();
					}
				}
				enumerator11 = jatoms.GetEnumerator();
				try
				{
					while (enumerator11.MoveNext())
					{
						JSONClass jsonclass3 = (JSONClass)enumerator11.Current;
						string text2 = jsonclass3["id"];
						Atom atomByUid3 = base.GetAtomByUid(text2);
						if (atomByUid3 != null)
						{
							base.UpdateLoadingStatus("Restoring atom " + text2);
							atomByUid3.Restore(jsonclass3, true, true, true, null, false, false, true, false);
						}
						else
						{
							base.Error("Could not find atom by uid " + text2, true, true);
						}
						base.IncrementLoadingSlider();
					}
				}
				finally
				{
					if ((disposable5 = (enumerator11 as IDisposable)) != null)
					{
						disposable5.Dispose();
					}
				}
				base.UpdateLoadingStatus("Post-Restore");
				enumerator12 = jatoms.GetEnumerator();
				try
				{
					while (enumerator12.MoveNext())
					{
						JSONClass jsonclass4 = (JSONClass)enumerator12.Current;
						string text3 = jsonclass4["id"];
						Atom atomByUid4 = base.GetAtomByUid(text3);
						if (atomByUid4 != null)
						{
							atomByUid4.LateRestore(jsonclass4, true, true, true, false, true, false);
						}
						else
						{
							base.Error("Could not find atom by uid " + text3, true, true);
						}
					}
				}
				finally
				{
					if ((disposable6 = (enumerator12 as IDisposable)) != null)
					{
						disposable6.Dispose();
					}
				}
				enumerator13 = this.atomsList.GetEnumerator();
				try
				{
					while (enumerator13.MoveNext())
					{
						Atom atom8 = enumerator13.Current;
						atom8.PostRestore();
					}
				}
				finally
				{
					((IDisposable)enumerator13).Dispose();
				}
				goto IL_1106;
			case 8U:
				goto IL_1106;
			case 9U:
			{
				if (this.loadJson["headPossessedController"] != null)
				{
					FreeControllerV3 freeControllerV = base.FreeControllerNameToFreeController(this.loadJson["headPossessedController"]);
					if (freeControllerV != null)
					{
						base.HeadPossess(freeControllerV, true, true, true);
					}
				}
				if (!(this.loadJson["playerNavCollider"] != null))
				{
					goto IL_123D;
				}
				string text4 = this.loadJson["playerNavCollider"];
				PlayerNavCollider playerNavCollider;
				if (this.pncMap.TryGetValue(text4, out playerNavCollider))
				{
					this.playerNavCollider = playerNavCollider;
					base.ConnectNavRigToPlayerNavCollider();
					goto IL_123D;
				}
				base.Error("Could not find playerNavCollider " + text4, true, true);
				goto IL_123D;
			}
			case 10U:
				i++;
				goto IL_12C4;
			case 11U:
				j++;
				goto IL_1317;
			case 12U:
				k++;
				goto IL_13DE;
			default:
				return false;
			}
			if (!clearOnly)
			{
				jatoms = this.loadJson["atoms"].AsArray;
				if (this.loadJson["worldScale"] != null)
				{
					base.worldScale = this.loadJson["worldScale"].AsFloat;
				}
				else if (this.worldScaleSlider != null)
				{
					SliderControl component3 = this.worldScaleSlider.GetComponent<SliderControl>();
					if (component3 != null)
					{
						base.worldScale = component3.defaultValue;
					}
				}
				if (this.loadJson["environmentHeight"] != null)
				{
					base.playerHeightAdjust = this.loadJson["environmentHeight"].AsFloat;
				}
				else if (this.loadJson["playerHeightAdjust"] != null)
				{
					base.playerHeightAdjust = this.loadJson["playerHeightAdjust"].AsFloat;
				}
				else if (this.playerHeightAdjustSlider != null)
				{
					SliderControl component4 = this.playerHeightAdjustSlider.GetComponent<SliderControl>();
					if (component4 != null)
					{
						base.playerHeightAdjust = component4.defaultValue;
					}
				}
				if (this.loadJson["monitorCameraRotation"] != null)
				{
					Vector3 localEulerAngles;
					localEulerAngles.x = 0f;
					localEulerAngles.y = 0f;
					localEulerAngles.z = 0f;
					if (this.loadJson["monitorCameraRotation"]["x"] != null)
					{
						localEulerAngles.x = this.loadJson["monitorCameraRotation"]["x"].AsFloat;
					}
					if (this.loadJson["monitorCameraRotation"]["y"] != null)
					{
						localEulerAngles.y = this.loadJson["monitorCameraRotation"]["y"].AsFloat;
					}
					if (this.loadJson["monitorCameraRotation"]["z"] != null)
					{
						localEulerAngles.z = this.loadJson["monitorCameraRotation"]["z"].AsFloat;
					}
					if (this.MonitorCenterCamera != null)
					{
						this.MonitorCenterCamera.transform.localEulerAngles = localEulerAngles;
					}
				}
				if (this.loadJson["useSceneLoadPosition"] != null)
				{
					base.useSceneLoadPosition = this.loadJson["useSceneLoadPosition"].AsBool;
				}
				if (this.loadingProgressSlider != null)
				{
					this.loadingProgressSlider.minValue = 0f;
					this.loadingProgressSlider.maxValue = (float)jatoms.Count * 2f + 2f;
					this.loadingProgressSlider.value = 0f;
				}
				if (this.loadingProgressSliderAlt != null)
				{
					this.loadingProgressSliderAlt.minValue = 0f;
					this.loadingProgressSliderAlt.maxValue = (float)jatoms.Count * 2f + 2f;
					this.loadingProgressSliderAlt.value = 0f;
				}
				base.UpdateLoadingStatus("Pre-Restore");
				enumerator7 = jatoms.GetEnumerator();
				try
				{
					while (enumerator7.MoveNext())
					{
						JSONClass jsonclass5 = (JSONClass)enumerator7.Current;
						string uid2 = jsonclass5["id"];
						Atom atomByUid5 = base.GetAtomByUid(uid2);
						if (atomByUid5 != null)
						{
							atomByUid5.PreRestore();
						}
					}
				}
				finally
				{
					if ((disposable = (enumerator7 as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
				base.IncrementLoadingSlider();
				Physics.autoSimulation = false;
				enumerator8 = jatoms.GetEnumerator();
				num = 4294967293U;
				goto Block_48;
			}
			goto IL_123D;
			IL_1106:
			if (!base.CheckHoldLoad())
			{
				Physics.autoSimulation = true;
				base.SetSceneLoadPosition();
				base.IncrementLoadingSlider();
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 9;
				}
				return true;
			}
			base.UpdateLoadingStatus("Waiting for async load from " + this.holdLoadCompleteFlags[0].Name);
			this.$current = null;
			if (!this.$disposing)
			{
				this.$PC = 8;
			}
			return true;
			IL_123D:
			if (loadMerge)
			{
				foreach (Atom atom9 in this.atomsList)
				{
					atom9.Validate();
				}
			}
			i = 0;
			IL_12C4:
			if (i < 20)
			{
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 10;
				}
				return true;
			}
			this.loadFlag.Raise();
			j = 0;
			IL_1317:
			if (j < 5)
			{
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 11;
				}
				return true;
			}
			this._isLoading = false;
			base.SyncSortedAtomUIDs();
			base.SyncSortedAtomUIDsWithForceProducers();
			base.SyncSortedAtomUIDsWithForceReceivers();
			base.SyncSortedAtomUIDsWithFreeControllers();
			base.SyncSortedAtomUIDsWithRhythmControllers();
			base.SyncSortedAtomUIDsWithAudioSourceControls();
			base.SyncSortedAtomUIDsWithRigidbodies();
			base.SyncHiddenAtoms();
			base.SyncSelectAtomPopup();
			if (!(this.loadingUI != null))
			{
				goto IL_1459;
			}
			k = 0;
			IL_13DE:
			if (k < 10)
			{
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 12;
				}
				return true;
			}
			this.loadingUI.gameObject.SetActive(false);
			if (this.loadingUIAlt != null)
			{
				this.loadingUIAlt.gameObject.SetActive(false);
			}
			if (this.loadingGeometry != null)
			{
				this.loadingGeometry.gameObject.SetActive(false);
			}
			IL_1459:
			if (UserPreferences.singleton != null)
			{
				UserPreferences.singleton.pauseGlow = false;
			}
			if (base.UIDisabled && !loadMerge)
			{
				base.HideMainHUD();
			}
			if (!loadMerge && this.mainHUDAttachPoint != null)
			{
				this.mainHUDAttachPoint.localPosition = this.mainHUDAttachPointStartingPosition;
				this.mainHUDAttachPoint.localRotation = this.mainHUDAttachPointStartingRotation;
			}
			base.SyncVisibility();
			this.hideWaitTransform = false;
			if (this.onSceneLoadedHandlers != null)
			{
				this.onSceneLoadedHandlers();
			}
			this.$PC = -1;
			return false;
		}

		// Token: 0x17001191 RID: 4497
		// (get) Token: 0x060076B8 RID: 30392 RVA: 0x0023FEB0 File Offset: 0x0023E2B0
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x17001192 RID: 4498
		// (get) Token: 0x060076B9 RID: 30393 RVA: 0x0023FEB8 File Offset: 0x0023E2B8
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x060076BA RID: 30394 RVA: 0x0023FEC0 File Offset: 0x0023E2C0
		[DebuggerHidden]
		public void Dispose()
		{
			uint num = (uint)this.$PC;
			this.$disposing = true;
			this.$PC = -1;
			switch (num)
			{
			case 5U:
				try
				{
				}
				finally
				{
					if ((disposable2 = (enumerator8 as IDisposable)) != null)
					{
						disposable2.Dispose();
					}
				}
				break;
			}
		}

		// Token: 0x060076BB RID: 30395 RVA: 0x0023FF5C File Offset: 0x0023E35C
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x04006AA3 RID: 27299
		internal bool loadMerge;

		// Token: 0x04006AA4 RID: 27300
		internal JSONClass <jc>__1;

		// Token: 0x04006AA5 RID: 27301
		internal Atom[] <atms>__1;

		// Token: 0x04006AA6 RID: 27302
		internal List<Atom>.Enumerator $locvar0;

		// Token: 0x04006AA7 RID: 27303
		internal List<Atom>.Enumerator $locvar1;

		// Token: 0x04006AA8 RID: 27304
		internal List<Atom>.Enumerator $locvar2;

		// Token: 0x04006AA9 RID: 27305
		internal List<Atom>.Enumerator $locvar3;

		// Token: 0x04006AAA RID: 27306
		internal List<Atom>.Enumerator $locvar4;

		// Token: 0x04006AAB RID: 27307
		internal List<Atom>.Enumerator $locvar5;

		// Token: 0x04006AAC RID: 27308
		internal bool clearOnly;

		// Token: 0x04006AAD RID: 27309
		internal JSONArray <jatoms>__2;

		// Token: 0x04006AAE RID: 27310
		internal IEnumerator $locvar6;

		// Token: 0x04006AAF RID: 27311
		internal IDisposable $locvar7;

		// Token: 0x04006AB0 RID: 27312
		internal IEnumerator $locvar8;

		// Token: 0x04006AB1 RID: 27313
		internal JSONClass <jatom>__3;

		// Token: 0x04006AB2 RID: 27314
		internal IDisposable $locvar9;

		// Token: 0x04006AB3 RID: 27315
		internal string <auid>__4;

		// Token: 0x04006AB4 RID: 27316
		internal string <type>__4;

		// Token: 0x04006AB5 RID: 27317
		internal Atom <a>__4;

		// Token: 0x04006AB6 RID: 27318
		internal IEnumerator $locvarA;

		// Token: 0x04006AB7 RID: 27319
		internal IDisposable $locvarB;

		// Token: 0x04006AB8 RID: 27320
		internal IEnumerator $locvarC;

		// Token: 0x04006AB9 RID: 27321
		internal IDisposable $locvarD;

		// Token: 0x04006ABA RID: 27322
		internal IEnumerator $locvarE;

		// Token: 0x04006ABB RID: 27323
		internal IDisposable $locvarF;

		// Token: 0x04006ABC RID: 27324
		internal IEnumerator $locvar10;

		// Token: 0x04006ABD RID: 27325
		internal IDisposable $locvar11;

		// Token: 0x04006ABE RID: 27326
		internal List<Atom>.Enumerator $locvar12;

		// Token: 0x04006ABF RID: 27327
		internal int <i>__5;

		// Token: 0x04006AC0 RID: 27328
		internal int <i>__6;

		// Token: 0x04006AC1 RID: 27329
		internal int <i>__7;

		// Token: 0x04006AC2 RID: 27330
		internal SuperController $this;

		// Token: 0x04006AC3 RID: 27331
		internal object $current;

		// Token: 0x04006AC4 RID: 27332
		internal bool $disposing;

		// Token: 0x04006AC5 RID: 27333
		internal int $PC;
	}

	// Token: 0x02001014 RID: 4116
	[CompilerGenerated]
	private sealed class <LoadFromSceneDialog>c__AnonStorey10
	{
		// Token: 0x060076BC RID: 30396 RVA: 0x0023FF63 File Offset: 0x0023E363
		public <LoadFromSceneDialog>c__AnonStorey10()
		{
		}

		// Token: 0x060076BD RID: 30397 RVA: 0x0023FF6B File Offset: 0x0023E36B
		internal void <>m__0()
		{
			this.$this.loadConfirmPanel.gameObject.SetActive(false);
			this.$this.Load(this.saveName);
		}

		// Token: 0x04006AC6 RID: 27334
		internal string saveName;

		// Token: 0x04006AC7 RID: 27335
		internal SuperController $this;
	}

	// Token: 0x02001015 RID: 4117
	[CompilerGenerated]
	private sealed class <LoadForEditFromSceneDialog>c__AnonStorey11
	{
		// Token: 0x060076BE RID: 30398 RVA: 0x0023FF94 File Offset: 0x0023E394
		public <LoadForEditFromSceneDialog>c__AnonStorey11()
		{
		}

		// Token: 0x060076BF RID: 30399 RVA: 0x0023FF9C File Offset: 0x0023E39C
		internal void <>m__0()
		{
			this.$this.loadConfirmPanel.gameObject.SetActive(false);
			this.$this.LoadForEdit(this.saveName);
		}

		// Token: 0x04006AC8 RID: 27336
		internal string saveName;

		// Token: 0x04006AC9 RID: 27337
		internal SuperController $this;
	}

	// Token: 0x02001016 RID: 4118
	[CompilerGenerated]
	private sealed class <LoadMergeFromSceneDialog>c__AnonStorey12
	{
		// Token: 0x060076C0 RID: 30400 RVA: 0x0023FFC5 File Offset: 0x0023E3C5
		public <LoadMergeFromSceneDialog>c__AnonStorey12()
		{
		}

		// Token: 0x060076C1 RID: 30401 RVA: 0x0023FFCD File Offset: 0x0023E3CD
		internal void <>m__0()
		{
			this.$this.loadConfirmPanel.gameObject.SetActive(false);
			this.$this.LoadMerge(this.saveName);
		}

		// Token: 0x04006ACA RID: 27338
		internal string saveName;

		// Token: 0x04006ACB RID: 27339
		internal SuperController $this;
	}

	// Token: 0x02001017 RID: 4119
	[CompilerGenerated]
	private sealed class <SaveStringIntoFile>c__AnonStorey13
	{
		// Token: 0x060076C2 RID: 30402 RVA: 0x0023FFF6 File Offset: 0x0023E3F6
		public <SaveStringIntoFile>c__AnonStorey13()
		{
		}

		// Token: 0x060076C3 RID: 30403 RVA: 0x00240000 File Offset: 0x0023E400
		internal void <>m__0()
		{
			try
			{
				FileManager.WriteAllText(this.path, this.contents);
			}
			catch (Exception arg)
			{
				this.$this.Error("Exception while saving string into file " + arg, true, true);
			}
		}

		// Token: 0x04006ACC RID: 27340
		internal string path;

		// Token: 0x04006ACD RID: 27341
		internal string contents;

		// Token: 0x04006ACE RID: 27342
		internal SuperController $this;
	}

	// Token: 0x02001018 RID: 4120
	[CompilerGenerated]
	private sealed class <AssetManagerReady>c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x060076C4 RID: 30404 RVA: 0x00240054 File Offset: 0x0023E454
		[DebuggerHidden]
		public <AssetManagerReady>c__Iterator2()
		{
		}

		// Token: 0x060076C5 RID: 30405 RVA: 0x0024005C File Offset: 0x0023E45C
		public bool MoveNext()
		{
			uint num = (uint)this.$PC;
			this.$PC = -1;
			switch (num)
			{
			case 0U:
				if (SuperController.<AssetManagerReady>c__Iterator2.<>f__am$cache0 == null)
				{
					SuperController.<AssetManagerReady>c__Iterator2.<>f__am$cache0 = new Func<bool>(SuperController.<AssetManagerReady>c__Iterator2.<>m__0);
				}
				this.$current = new WaitUntil(SuperController.<AssetManagerReady>c__Iterator2.<>f__am$cache0);
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
			case 1U:
				this.$PC = -1;
				break;
			}
			return false;
		}

		// Token: 0x17001193 RID: 4499
		// (get) Token: 0x060076C6 RID: 30406 RVA: 0x002400D0 File Offset: 0x0023E4D0
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x17001194 RID: 4500
		// (get) Token: 0x060076C7 RID: 30407 RVA: 0x002400D8 File Offset: 0x0023E4D8
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x060076C8 RID: 30408 RVA: 0x002400E0 File Offset: 0x0023E4E0
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x060076C9 RID: 30409 RVA: 0x002400F0 File Offset: 0x0023E4F0
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x060076CA RID: 30410 RVA: 0x002400F7 File Offset: 0x0023E4F7
		private static bool <>m__0()
		{
			return SuperController._singleton != null && SuperController._singleton.assetManagerReady;
		}

		// Token: 0x04006ACF RID: 27343
		internal object $current;

		// Token: 0x04006AD0 RID: 27344
		internal bool $disposing;

		// Token: 0x04006AD1 RID: 27345
		internal int $PC;

		// Token: 0x04006AD2 RID: 27346
		private static Func<bool> <>f__am$cache0;
	}

	// Token: 0x02001019 RID: 4121
	[CompilerGenerated]
	private sealed class <InitAssetManager>c__Iterator3 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x060076CB RID: 30411 RVA: 0x00240116 File Offset: 0x0023E516
		[DebuggerHidden]
		public <InitAssetManager>c__Iterator3()
		{
		}

		// Token: 0x060076CC RID: 30412 RVA: 0x00240120 File Offset: 0x0023E520
		public bool MoveNext()
		{
			uint num = (uint)this.$PC;
			this.$PC = -1;
			switch (num)
			{
			case 0U:
				AssetBundleManager.SetSourceAssetBundleDirectory(Application.streamingAssetsPath + "/");
				request = AssetBundleManager.Initialize();
				if (request != null)
				{
					this.$current = base.StartCoroutine(request);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				break;
			case 1U:
				break;
			default:
				return false;
			}
			UnityEngine.Debug.Log("Asset Manager Ready");
			this._assetManagerReady = true;
			this.$PC = -1;
			return false;
		}

		// Token: 0x17001195 RID: 4501
		// (get) Token: 0x060076CD RID: 30413 RVA: 0x002401C3 File Offset: 0x0023E5C3
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x17001196 RID: 4502
		// (get) Token: 0x060076CE RID: 30414 RVA: 0x002401CB File Offset: 0x0023E5CB
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x060076CF RID: 30415 RVA: 0x002401D3 File Offset: 0x0023E5D3
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x060076D0 RID: 30416 RVA: 0x002401E3 File Offset: 0x0023E5E3
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x04006AD3 RID: 27347
		internal AssetBundleLoadManifestOperation <request>__0;

		// Token: 0x04006AD4 RID: 27348
		internal SuperController $this;

		// Token: 0x04006AD5 RID: 27349
		internal object $current;

		// Token: 0x04006AD6 RID: 27350
		internal bool $disposing;

		// Token: 0x04006AD7 RID: 27351
		internal int $PC;
	}

	// Token: 0x0200101A RID: 4122
	[CompilerGenerated]
	private sealed class <LoadAtomFromBundleAsync>c__Iterator4 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x060076D1 RID: 30417 RVA: 0x002401EA File Offset: 0x0023E5EA
		[DebuggerHidden]
		public <LoadAtomFromBundleAsync>c__Iterator4()
		{
		}

		// Token: 0x060076D2 RID: 30418 RVA: 0x002401F4 File Offset: 0x0023E5F4
		public bool MoveNext()
		{
			uint num = (uint)this.$PC;
			this.$PC = -1;
			switch (num)
			{
			case 0U:
				this.$current = SuperController.AssetManagerReady();
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
			case 1U:
				startTime = Time.realtimeSinceStartup;
				go = base.GetCachedPrefab(aa.assetBundleName, aa.assetName);
				if (go == null)
				{
					request = AssetBundleManager.LoadAssetAsync(aa.assetBundleName, aa.assetName, typeof(GameObject));
					if (request == null)
					{
						base.Error("Failed to load Atom " + aa.assetName, true, true);
						return false;
					}
					this.$current = base.StartCoroutine(request);
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				break;
			case 2U:
				go = request.GetAsset<GameObject>();
				if (go != null)
				{
					base.RegisterPrefab(aa.assetBundleName, aa.assetName, go);
				}
				else
				{
					base.Error("Asset " + aa.assetName + " is missing game object", true, true);
				}
				break;
			default:
				return false;
			}
			if (go != null)
			{
				Atom component = go.GetComponent<Atom>();
				if (component != null)
				{
					startTime = Time.realtimeSinceStartup;
					Transform transform = base.AddAtom(component, useuid, userInvoked, forceSelect, forceFocus, true);
					if (transform != null)
					{
						Atom component2 = transform.GetComponent<Atom>();
						if (component2 != null)
						{
							component2.loadedFromBundle = true;
						}
					}
				}
				else
				{
					base.Error("Asset " + aa.assetName + " is missing Atom component", true, true);
				}
			}
			this.$PC = -1;
			return false;
		}

		// Token: 0x17001197 RID: 4503
		// (get) Token: 0x060076D3 RID: 30419 RVA: 0x00240440 File Offset: 0x0023E840
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x17001198 RID: 4504
		// (get) Token: 0x060076D4 RID: 30420 RVA: 0x00240448 File Offset: 0x0023E848
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x060076D5 RID: 30421 RVA: 0x00240450 File Offset: 0x0023E850
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x060076D6 RID: 30422 RVA: 0x00240460 File Offset: 0x0023E860
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x04006AD8 RID: 27352
		internal float <startTime>__0;

		// Token: 0x04006AD9 RID: 27353
		internal SuperController.AtomAsset aa;

		// Token: 0x04006ADA RID: 27354
		internal GameObject <go>__0;

		// Token: 0x04006ADB RID: 27355
		internal AssetBundleLoadAssetOperation <request>__1;

		// Token: 0x04006ADC RID: 27356
		internal string useuid;

		// Token: 0x04006ADD RID: 27357
		internal bool userInvoked;

		// Token: 0x04006ADE RID: 27358
		internal bool forceSelect;

		// Token: 0x04006ADF RID: 27359
		internal bool forceFocus;

		// Token: 0x04006AE0 RID: 27360
		internal SuperController $this;

		// Token: 0x04006AE1 RID: 27361
		internal object $current;

		// Token: 0x04006AE2 RID: 27362
		internal bool $disposing;

		// Token: 0x04006AE3 RID: 27363
		internal int $PC;
	}

	// Token: 0x0200101B RID: 4123
	[CompilerGenerated]
	private sealed class <AddAtomByType>c__Iterator5 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x060076D7 RID: 30423 RVA: 0x00240467 File Offset: 0x0023E867
		[DebuggerHidden]
		public <AddAtomByType>c__Iterator5()
		{
		}

		// Token: 0x060076D8 RID: 30424 RVA: 0x00240470 File Offset: 0x0023E870
		public bool MoveNext()
		{
			uint num = (uint)this.$PC;
			this.$PC = -1;
			switch (num)
			{
			case 0U:
				loadIconFlag = new AsyncFlag("Load Atom " + type);
				base.SetLoadingIconFlag(loadIconFlag);
				if (type == null || !(type != string.Empty))
				{
					goto IL_23C;
				}
				this.lastAddedAtom = null;
				if (atom = base.GetAtomOfTypeFromPool(type))
				{
					base.AddAtom(atom, useuid, userInvoked, forceSelect, forceFocus, false);
					if (userInvoked)
					{
						base.ResetSimulation(5, "AddAtom", true);
					}
				}
				else
				{
					if (this.atomAssetByType.TryGetValue(type, out aa))
					{
						this.$current = base.StartCoroutine(base.LoadAtomFromBundleAsync(aa, useuid, userInvoked, forceSelect, false));
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					if (this.atomPrefabByType.TryGetValue(type, out atom))
					{
						base.AddAtom(atom, useuid, userInvoked, forceSelect, forceFocus, true);
					}
					else
					{
						base.Error("Atom type " + type + " does not exist. Cannot add", true, true);
					}
				}
				break;
			case 1U:
				break;
			case 2U:
				this.lastAddedAtom.mainPresetControl.LoadUserDefaults();
				goto IL_23C;
			default:
				return false;
			}
			if (userInvoked && this.lastAddedAtom != null && this.lastAddedAtom.mainPresetControl != null)
			{
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 2;
				}
				return true;
			}
			IL_23C:
			loadIconFlag.Raise();
			this.$PC = -1;
			return false;
		}

		// Token: 0x17001199 RID: 4505
		// (get) Token: 0x060076D9 RID: 30425 RVA: 0x002406CE File Offset: 0x0023EACE
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x1700119A RID: 4506
		// (get) Token: 0x060076DA RID: 30426 RVA: 0x002406D6 File Offset: 0x0023EAD6
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x060076DB RID: 30427 RVA: 0x002406DE File Offset: 0x0023EADE
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x060076DC RID: 30428 RVA: 0x002406EE File Offset: 0x0023EAEE
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x04006AE4 RID: 27364
		internal string type;

		// Token: 0x04006AE5 RID: 27365
		internal AsyncFlag <loadIconFlag>__0;

		// Token: 0x04006AE6 RID: 27366
		internal Atom <atom>__1;

		// Token: 0x04006AE7 RID: 27367
		internal string useuid;

		// Token: 0x04006AE8 RID: 27368
		internal bool userInvoked;

		// Token: 0x04006AE9 RID: 27369
		internal bool forceSelect;

		// Token: 0x04006AEA RID: 27370
		internal bool forceFocus;

		// Token: 0x04006AEB RID: 27371
		internal SuperController.AtomAsset <aa>__1;

		// Token: 0x04006AEC RID: 27372
		internal SuperController $this;

		// Token: 0x04006AED RID: 27373
		internal object $current;

		// Token: 0x04006AEE RID: 27374
		internal bool $disposing;

		// Token: 0x04006AEF RID: 27375
		internal int $PC;
	}

	// Token: 0x0200101C RID: 4124
	[CompilerGenerated]
	private sealed class <DelayLoadSessionPlugins>c__Iterator6 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x060076DD RID: 30429 RVA: 0x002406F5 File Offset: 0x0023EAF5
		[DebuggerHidden]
		public <DelayLoadSessionPlugins>c__Iterator6()
		{
		}

		// Token: 0x060076DE RID: 30430 RVA: 0x00240700 File Offset: 0x0023EB00
		public bool MoveNext()
		{
			uint num = (uint)this.$PC;
			this.$PC = -1;
			switch (num)
			{
			case 0U:
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
			case 1U:
				break;
			case 2U:
				break;
			default:
				return false;
			}
			if (UserPreferences.singleton != null && UserPreferences.singleton.firstTimeUser)
			{
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 2;
				}
				return true;
			}
			if (this.sessionPresetManagerControl != null)
			{
				this.sessionPresetManagerControl.LoadUserDefaults();
			}
			this.$PC = -1;
			return false;
		}

		// Token: 0x1700119B RID: 4507
		// (get) Token: 0x060076DF RID: 30431 RVA: 0x002407BC File Offset: 0x0023EBBC
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x1700119C RID: 4508
		// (get) Token: 0x060076E0 RID: 30432 RVA: 0x002407C4 File Offset: 0x0023EBC4
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x060076E1 RID: 30433 RVA: 0x002407CC File Offset: 0x0023EBCC
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x060076E2 RID: 30434 RVA: 0x002407DC File Offset: 0x0023EBDC
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x04006AF0 RID: 27376
		internal SuperController $this;

		// Token: 0x04006AF1 RID: 27377
		internal object $current;

		// Token: 0x04006AF2 RID: 27378
		internal bool $disposing;

		// Token: 0x04006AF3 RID: 27379
		internal int $PC;
	}

	// Token: 0x0200101D RID: 4125
	[CompilerGenerated]
	private sealed class <DelayStart>c__Iterator7 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x060076E3 RID: 30435 RVA: 0x002407E3 File Offset: 0x0023EBE3
		[DebuggerHidden]
		public <DelayStart>c__Iterator7()
		{
		}

		// Token: 0x060076E4 RID: 30436 RVA: 0x002407EC File Offset: 0x0023EBEC
		public bool MoveNext()
		{
			uint num = (uint)this.$PC;
			this.$PC = -1;
			switch (num)
			{
			case 0U:
				this.onStartScene = true;
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
			case 1U:
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 2;
				}
				return true;
			case 2U:
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 3;
				}
				return true;
			case 3U:
				base.StartScene();
				this.$PC = -1;
				break;
			}
			return false;
		}

		// Token: 0x1700119D RID: 4509
		// (get) Token: 0x060076E5 RID: 30437 RVA: 0x00240894 File Offset: 0x0023EC94
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x1700119E RID: 4510
		// (get) Token: 0x060076E6 RID: 30438 RVA: 0x0024089C File Offset: 0x0023EC9C
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x060076E7 RID: 30439 RVA: 0x002408A4 File Offset: 0x0023ECA4
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x060076E8 RID: 30440 RVA: 0x002408B4 File Offset: 0x0023ECB4
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x04006AF4 RID: 27380
		internal SuperController $this;

		// Token: 0x04006AF5 RID: 27381
		internal object $current;

		// Token: 0x04006AF6 RID: 27382
		internal bool $disposing;

		// Token: 0x04006AF7 RID: 27383
		internal int $PC;
	}
}
