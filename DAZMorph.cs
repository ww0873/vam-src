using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using MeshVR;
using MVR.FileManagement;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000AF1 RID: 2801
[Serializable]
public class DAZMorph
{
	// Token: 0x06004AE6 RID: 19174 RVA: 0x0019E544 File Offset: 0x0019C944
	public DAZMorph()
	{
	}

	// Token: 0x06004AE7 RID: 19175 RVA: 0x0019E578 File Offset: 0x0019C978
	public DAZMorph(DAZMorph copyFrom)
	{
		this.CopyParameters(copyFrom, true);
		this.numDeltas = copyFrom.numDeltas;
		this.deltas = new DAZMorphVertex[copyFrom.deltas.Length];
		for (int i = 0; i < this.deltas.Length; i++)
		{
			this.deltas[i] = new DAZMorphVertex();
			this.deltas[i].vertex = copyFrom.deltas[i].vertex;
			this.deltas[i].delta = copyFrom.deltas[i].delta;
		}
		this.formulas = new DAZMorphFormula[copyFrom.formulas.Length];
		for (int j = 0; j < this.formulas.Length; j++)
		{
			this.formulas[j] = new DAZMorphFormula();
			this.formulas[j].targetType = copyFrom.formulas[j].targetType;
			this.formulas[j].target = copyFrom.formulas[j].target;
			this.formulas[j].multiplier = copyFrom.formulas[j].multiplier;
		}
	}

	// Token: 0x17000AA2 RID: 2722
	// (get) Token: 0x06004AE8 RID: 19176 RVA: 0x0019E6BE File Offset: 0x0019CABE
	// (set) Token: 0x06004AE9 RID: 19177 RVA: 0x0019E6ED File Offset: 0x0019CAED
	public string uid
	{
		get
		{
			if (this._uid != null && this._uid != string.Empty)
			{
				return this._uid;
			}
			return this.resolvedDisplayName;
		}
		set
		{
			this._uid = value;
		}
	}

	// Token: 0x17000AA3 RID: 2723
	// (get) Token: 0x06004AEA RID: 19178 RVA: 0x0019E6F6 File Offset: 0x0019CAF6
	public string resolvedDisplayName
	{
		get
		{
			if (this.overrideName != null && this.overrideName != string.Empty)
			{
				return this.overrideName;
			}
			return this.displayName;
		}
	}

	// Token: 0x17000AA4 RID: 2724
	// (get) Token: 0x06004AEB RID: 19179 RVA: 0x0019E725 File Offset: 0x0019CB25
	public string lowerCaseResolvedDisplayName
	{
		get
		{
			if (this._lowerCaseResolvedDisplayName == null)
			{
				if (this.resolvedDisplayName != null)
				{
					this._lowerCaseResolvedDisplayName = this.resolvedDisplayName.ToLower();
				}
				else
				{
					this._lowerCaseResolvedDisplayName = string.Empty;
				}
			}
			return this._lowerCaseResolvedDisplayName;
		}
	}

	// Token: 0x17000AA5 RID: 2725
	// (get) Token: 0x06004AEC RID: 19180 RVA: 0x0019E764 File Offset: 0x0019CB64
	public string resolvedRegionName
	{
		get
		{
			if (this.overrideRegion != null && this.overrideRegion != string.Empty)
			{
				return this.overrideRegion;
			}
			if (this.morphSubBank != null && this.morphSubBank.useOverrideRegionName)
			{
				return this.morphSubBank.overrideRegionName;
			}
			return this.region;
		}
	}

	// Token: 0x17000AA6 RID: 2726
	// (get) Token: 0x06004AEE RID: 19182 RVA: 0x0019E806 File Offset: 0x0019CC06
	// (set) Token: 0x06004AED RID: 19181 RVA: 0x0019E7CB File Offset: 0x0019CBCB
	public float morphValue
	{
		get
		{
			return this._morphValue;
		}
		set
		{
			if (this.jsonFloat != null)
			{
				this.jsonFloat.val = value;
			}
			else
			{
				this._morphValue = value;
				this.activeImmediate = (this._morphValue != 0f);
			}
		}
	}

	// Token: 0x17000AA7 RID: 2727
	// (get) Token: 0x06004AF0 RID: 19184 RVA: 0x0019E85D File Offset: 0x0019CC5D
	// (set) Token: 0x06004AEF RID: 19183 RVA: 0x0019E810 File Offset: 0x0019CC10
	public float morphValueAdjustLimits
	{
		get
		{
			return this._morphValue;
		}
		set
		{
			this.AdjustRange(value);
			if (this.jsonFloat != null)
			{
				this.jsonFloat.val = value;
			}
			else
			{
				this._morphValue = value;
				this.activeImmediate = (this._morphValue != 0f);
			}
		}
	}

	// Token: 0x17000AA8 RID: 2728
	// (get) Token: 0x06004AF1 RID: 19185 RVA: 0x0019E865 File Offset: 0x0019CC65
	// (set) Token: 0x06004AF2 RID: 19186 RVA: 0x0019E86D File Offset: 0x0019CC6D
	public bool activeImmediate
	{
		[CompilerGenerated]
		get
		{
			return this.<activeImmediate>k__BackingField;
		}
		[CompilerGenerated]
		protected set
		{
			this.<activeImmediate>k__BackingField = value;
		}
	}

	// Token: 0x06004AF3 RID: 19187 RVA: 0x0019E878 File Offset: 0x0019CC78
	public void SyncDrivenUI()
	{
		if (this.drivenIndicator != null)
		{
			this.drivenIndicator.gameObject.SetActive(this._isDriven);
		}
		if (this.drivenByText != null)
		{
			this.drivenByText.text = this._drivenBy;
		}
	}

	// Token: 0x17000AA9 RID: 2729
	// (get) Token: 0x06004AF4 RID: 19188 RVA: 0x0019E8CE File Offset: 0x0019CCCE
	public bool isDriven
	{
		get
		{
			return this._isDriven;
		}
	}

	// Token: 0x06004AF5 RID: 19189 RVA: 0x0019E8D8 File Offset: 0x0019CCD8
	public bool SetDriven(bool b, string by, bool syncUI = false)
	{
		bool result = false;
		if (this._isDriven != b)
		{
			this._isDriven = b;
			result = true;
		}
		if (this._drivenBy != by)
		{
			this._drivenBy = by;
			result = true;
		}
		if (syncUI)
		{
			this.SyncDrivenUI();
		}
		return result;
	}

	// Token: 0x17000AAA RID: 2730
	// (get) Token: 0x06004AF6 RID: 19190 RVA: 0x0019E923 File Offset: 0x0019CD23
	public string drivenBy
	{
		get
		{
			return this._drivenBy;
		}
	}

	// Token: 0x17000AAB RID: 2731
	// (get) Token: 0x06004AF7 RID: 19191 RVA: 0x0019E92B File Offset: 0x0019CD2B
	public string formulasString
	{
		get
		{
			return this._formulasString;
		}
	}

	// Token: 0x17000AAC RID: 2732
	// (get) Token: 0x06004AF8 RID: 19192 RVA: 0x0019E933 File Offset: 0x0019CD33
	public bool hasFormulas
	{
		get
		{
			return this._hasFormulas;
		}
	}

	// Token: 0x17000AAD RID: 2733
	// (get) Token: 0x06004AF9 RID: 19193 RVA: 0x0019E93B File Offset: 0x0019CD3B
	public bool hasBoneModificationFormulas
	{
		get
		{
			return this._hasBoneModificationFormulas;
		}
	}

	// Token: 0x17000AAE RID: 2734
	// (get) Token: 0x06004AFA RID: 19194 RVA: 0x0019E943 File Offset: 0x0019CD43
	public bool hasBoneRotationFormulas
	{
		get
		{
			return this._hasBoneRotationFormulas;
		}
	}

	// Token: 0x17000AAF RID: 2735
	// (get) Token: 0x06004AFB RID: 19195 RVA: 0x0019E94B File Offset: 0x0019CD4B
	public bool hasMorphValueFormulas
	{
		get
		{
			return this._hasMorphValueFormulas;
		}
	}

	// Token: 0x17000AB0 RID: 2736
	// (get) Token: 0x06004AFC RID: 19196 RVA: 0x0019E953 File Offset: 0x0019CD53
	public bool hasMCMFormulas
	{
		get
		{
			return this._hasMCMFormulas;
		}
	}

	// Token: 0x17000AB1 RID: 2737
	// (get) Token: 0x06004AFD RID: 19197 RVA: 0x0019E95B File Offset: 0x0019CD5B
	// (set) Token: 0x06004AFE RID: 19198 RVA: 0x0019E963 File Offset: 0x0019CD63
	public bool wasZeroedKeepChildValues
	{
		get
		{
			return this._wasZeroedKeepChildValues;
		}
		set
		{
			this._wasZeroedKeepChildValues = value;
		}
	}

	// Token: 0x06004AFF RID: 19199 RVA: 0x0019E96C File Offset: 0x0019CD6C
	protected void ZeroKeepChildValues()
	{
		this.morphValueAdjustLimits = 0f;
		this._wasZeroedKeepChildValues = true;
	}

	// Token: 0x17000AB2 RID: 2738
	// (get) Token: 0x06004B00 RID: 19200 RVA: 0x0019E980 File Offset: 0x0019CD80
	// (set) Token: 0x06004B01 RID: 19201 RVA: 0x0019E988 File Offset: 0x0019CD88
	public Button zeroKeepChildValuesButton
	{
		get
		{
			return this._zeroKeepChildValuesButton;
		}
		set
		{
			if (this._zeroKeepChildValuesButton != value)
			{
				if (this._zeroKeepChildValuesButton != null)
				{
					this._zeroKeepChildValuesButton.onClick.RemoveListener(new UnityAction(this.ZeroKeepChildValues));
				}
				this._zeroKeepChildValuesButton = value;
				if (this._zeroKeepChildValuesButton != null)
				{
					this._zeroKeepChildValuesButton.gameObject.SetActive(this._hasMorphValueFormulas);
					this._zeroKeepChildValuesButton.onClick.AddListener(new UnityAction(this.ZeroKeepChildValues));
				}
			}
		}
	}

	// Token: 0x06004B02 RID: 19202 RVA: 0x0019EA1D File Offset: 0x0019CE1D
	public void CopyUid()
	{
		GUIUtility.systemCopyBuffer = this.uid;
		DAZMorph.uidCopy = this.uid;
	}

	// Token: 0x17000AB3 RID: 2739
	// (get) Token: 0x06004B03 RID: 19203 RVA: 0x0019EA35 File Offset: 0x0019CE35
	// (set) Token: 0x06004B04 RID: 19204 RVA: 0x0019EA40 File Offset: 0x0019CE40
	public Button copyUidButton
	{
		get
		{
			return this._copyUidButton;
		}
		set
		{
			if (this._copyUidButton != value)
			{
				if (this._copyUidButton != null)
				{
					this._copyUidButton.onClick.RemoveListener(new UnityAction(this.CopyUid));
				}
				this._copyUidButton = value;
				if (this._copyUidButton != null)
				{
					this._copyUidButton.onClick.AddListener(new UnityAction(this.CopyUid));
				}
			}
		}
	}

	// Token: 0x17000AB4 RID: 2740
	// (get) Token: 0x06004B05 RID: 19205 RVA: 0x0019EABF File Offset: 0x0019CEBF
	// (set) Token: 0x06004B06 RID: 19206 RVA: 0x0019EAC8 File Offset: 0x0019CEC8
	public RectTransform animatableWarningIndicator
	{
		get
		{
			return this._animatableWarningIndicator;
		}
		set
		{
			if (this._animatableWarningIndicator != value)
			{
				this._animatableWarningIndicator = value;
				if (this._animatableWarningIndicator != null)
				{
					this._animatableWarningIndicator.gameObject.SetActive(this._hasBoneModificationFormulas);
				}
			}
		}
	}

	// Token: 0x17000AB5 RID: 2741
	// (get) Token: 0x06004B07 RID: 19207 RVA: 0x0019EB14 File Offset: 0x0019CF14
	// (set) Token: 0x06004B08 RID: 19208 RVA: 0x0019EB1C File Offset: 0x0019CF1C
	public RectTransform animatableWarningIndicatorAlt
	{
		get
		{
			return this._animatableWarningIndicatorAlt;
		}
		set
		{
			if (this._animatableWarningIndicatorAlt != value)
			{
				this._animatableWarningIndicatorAlt = value;
				if (this._animatableWarningIndicatorAlt != null)
				{
					this._animatableWarningIndicatorAlt.gameObject.SetActive(this._hasBoneModificationFormulas);
				}
			}
		}
	}

	// Token: 0x17000AB6 RID: 2742
	// (get) Token: 0x06004B09 RID: 19209 RVA: 0x0019EB68 File Offset: 0x0019CF68
	// (set) Token: 0x06004B0A RID: 19210 RVA: 0x0019EB6B File Offset: 0x0019CF6B
	public bool animatable
	{
		get
		{
			return true;
		}
		set
		{
		}
	}

	// Token: 0x17000AB7 RID: 2743
	// (get) Token: 0x06004B0B RID: 19211 RVA: 0x0019EB6D File Offset: 0x0019CF6D
	// (set) Token: 0x06004B0C RID: 19212 RVA: 0x0019EB75 File Offset: 0x0019CF75
	public DAZMorphBank morphBank
	{
		get
		{
			return this._morphBank;
		}
		set
		{
			this._morphBank = value;
		}
	}

	// Token: 0x17000AB8 RID: 2744
	// (get) Token: 0x06004B0D RID: 19213 RVA: 0x0019EB7E File Offset: 0x0019CF7E
	// (set) Token: 0x06004B0E RID: 19214 RVA: 0x0019EB86 File Offset: 0x0019CF86
	public DAZMorphSubBank morphSubBank
	{
		get
		{
			return this._morphSubBank;
		}
		set
		{
			this._morphSubBank = value;
		}
	}

	// Token: 0x17000AB9 RID: 2745
	// (get) Token: 0x06004B0F RID: 19215 RVA: 0x0019EB8F File Offset: 0x0019CF8F
	// (set) Token: 0x06004B10 RID: 19216 RVA: 0x0019EB98 File Offset: 0x0019CF98
	public Image panelImage
	{
		get
		{
			return this._panelImage;
		}
		set
		{
			if (this._panelImage != value)
			{
				if (this._panelImage != null)
				{
					this._panelImage.color = DAZMorph.normalColor;
				}
				this._panelImage = value;
				if (this._panelImage != null)
				{
					if (this.isTransient)
					{
						this._panelImage.color = DAZMorph.transientColor;
					}
					else if (this.isInPackage)
					{
						this._panelImage.color = DAZMorph.packageColor;
					}
					else if (this.isRuntime)
					{
						this._panelImage.color = DAZMorph.runtimeColor;
					}
					else
					{
						this._panelImage.color = DAZMorph.normalColor;
					}
				}
			}
		}
	}

	// Token: 0x17000ABA RID: 2746
	// (get) Token: 0x06004B11 RID: 19217 RVA: 0x0019EC5F File Offset: 0x0019D05F
	// (set) Token: 0x06004B12 RID: 19218 RVA: 0x0019EC68 File Offset: 0x0019D068
	public Toggle favoriteToggle
	{
		get
		{
			return this._favoriteToggle;
		}
		set
		{
			if (this._favoriteToggle != value)
			{
				if (this._favoriteToggle != null)
				{
					this._favoriteToggle.onValueChanged.RemoveListener(new UnityAction<bool>(this.SetFavorite));
				}
				this._favoriteToggle = value;
				if (this._favoriteToggle != null)
				{
					this._favoriteToggle.isOn = this._favorite;
					this._favoriteToggle.onValueChanged.AddListener(new UnityAction<bool>(this.SetFavorite));
				}
			}
		}
	}

	// Token: 0x06004B13 RID: 19219 RVA: 0x0019ECF8 File Offset: 0x0019D0F8
	protected void SetFavorite(bool b)
	{
		this.favorite = b;
	}

	// Token: 0x17000ABB RID: 2747
	// (get) Token: 0x06004B14 RID: 19220 RVA: 0x0019ED01 File Offset: 0x0019D101
	// (set) Token: 0x06004B15 RID: 19221 RVA: 0x0019ED0C File Offset: 0x0019D10C
	public bool favorite
	{
		get
		{
			return this._favorite;
		}
		set
		{
			if (this._favorite != value && this._morphBank != null && this._morphBank.autoImportFolder != null && this._morphBank.autoImportFolder != string.Empty)
			{
				try
				{
					this._favorite = value;
					string text = this._morphBank.autoImportFolder + "/favorites";
					string path = text + "/" + this.resolvedDisplayName + ".fav";
					if (!FileManager.DirectoryExists(text, false, false))
					{
						FileManager.CreateDirectory(text);
					}
					if (FileManager.FileExists(path, true, true))
					{
						if (!value)
						{
							FileManager.DeleteFile(path);
						}
					}
					else if (value)
					{
						FileManager.WriteAllText(path, string.Empty);
					}
					if (this.favoriteToggle != null)
					{
						this.favoriteToggle.isOn = this._favorite;
					}
					this._morphBank.NotifyMorphFavoriteChanged(this);
				}
				catch (Exception arg)
				{
					UnityEngine.Debug.LogError("Exception during favorite set " + arg);
				}
			}
		}
	}

	// Token: 0x06004B16 RID: 19222 RVA: 0x0019EE30 File Offset: 0x0019D230
	public void SyncJSON()
	{
		this.jsonFloat.valNoCallback = this._morphValue;
	}

	// Token: 0x06004B17 RID: 19223 RVA: 0x0019EE43 File Offset: 0x0019D243
	public bool SetValue(float v)
	{
		if (this.jsonFloat.val != v)
		{
			this.SetValueInternal(v);
			return true;
		}
		return false;
	}

	// Token: 0x06004B18 RID: 19224 RVA: 0x0019EE60 File Offset: 0x0019D260
	public bool SetValueThreadSafe(float v)
	{
		float num = v;
		if (num < this.min)
		{
			num = this.min;
		}
		if (num > this.max)
		{
			num = this.max;
		}
		if (this._morphValue != num)
		{
			this._morphValue = num;
			this.activeImmediate = (this._morphValue != 0f);
			return true;
		}
		return false;
	}

	// Token: 0x06004B19 RID: 19225 RVA: 0x0019EEC4 File Offset: 0x0019D2C4
	protected void AdjustRange(float v)
	{
		if (v < this.min)
		{
			float num = Mathf.Floor(v);
			this.min = num;
			this.jsonFloat.min = num;
		}
		else if (v > this.max)
		{
			float num2 = Mathf.Ceil(v);
			this.max = num2;
			this.jsonFloat.max = num2;
		}
	}

	// Token: 0x06004B1A RID: 19226 RVA: 0x0019EF22 File Offset: 0x0019D322
	protected void SetValueInternal(float v)
	{
		this.AdjustRange(v);
		this._morphValue = v;
		this.activeImmediate = (this._morphValue != 0f);
	}

	// Token: 0x06004B1B RID: 19227 RVA: 0x0019EF48 File Offset: 0x0019D348
	public void Init()
	{
		this.startValue = this._morphValue;
		this.startingMin = this.min;
		this.startingMax = this.max;
		this.jsonFloat = new JSONStorableFloat(this.resolvedDisplayName, this._morphValue, new JSONStorableFloat.SetFloatCallback(this.SetValueInternal), this.min, this.max, false, true);
		this.jsonFloat.isStorable = false;
		this.jsonFloat.isRestorable = false;
		if (this._morphBank != null && FileManager.FileExists(this._morphBank.autoImportFolder + "/favorites/" + this.resolvedDisplayName + ".fav", true, true))
		{
			this._favorite = true;
		}
		else
		{
			this._favorite = false;
		}
		this._formulasString = string.Empty;
		HashSet<string> hashSet = new HashSet<string>();
		this._hasFormulas = (this.formulas.Length != 0);
		DAZMorphFormula[] array = this.formulas;
		int i = 0;
		while (i < array.Length)
		{
			DAZMorphFormula dazmorphFormula = array[i];
			switch (dazmorphFormula.targetType)
			{
			case DAZMorphFormulaTargetType.MorphValue:
			{
				DAZMorph morph = this.morphBank.GetMorph(dazmorphFormula.target);
				if (morph != null)
				{
					this._hasMorphValueFormulas = true;
					this._formulasString = this._formulasString + "Drives: " + morph.resolvedDisplayName + "\n";
				}
				break;
			}
			case DAZMorphFormulaTargetType.BoneCenterX:
			case DAZMorphFormulaTargetType.BoneCenterY:
			case DAZMorphFormulaTargetType.BoneCenterZ:
			case DAZMorphFormulaTargetType.OrientationX:
			case DAZMorphFormulaTargetType.OrientationY:
			case DAZMorphFormulaTargetType.OrientationZ:
				this._hasBoneModificationFormulas = true;
				break;
			case DAZMorphFormulaTargetType.MCM:
			{
				DAZMorph morph = this.morphBank.GetMorph(dazmorphFormula.target);
				if (morph != null)
				{
					this._hasMCMFormulas = true;
					this._formulasString = this._formulasString + "MCM driven by: " + morph.resolvedDisplayName + "\n";
				}
				break;
			}
			case DAZMorphFormulaTargetType.MCMMult:
			{
				DAZMorph morph = this.morphBank.GetMorph(dazmorphFormula.target);
				if (morph != null)
				{
					this._hasMCMFormulas = true;
					this._formulasString = this._formulasString + "MCMMult driven by: " + morph.resolvedDisplayName + "\n";
				}
				break;
			}
			case DAZMorphFormulaTargetType.RotationX:
			case DAZMorphFormulaTargetType.RotationY:
			case DAZMorphFormulaTargetType.RotationZ:
			{
				DAZMorph morph = this.morphBank.GetMorph(dazmorphFormula.target);
				if (morph != null)
				{
					this._hasBoneRotationFormulas = true;
					string text = "Rotates bone: " + morph.resolvedDisplayName + "\n";
					if (!hashSet.Contains(text))
					{
						this._formulasString += text;
						hashSet.Add(text);
					}
				}
				break;
			}
			}
			IL_28E:
			i++;
			continue;
			goto IL_28E;
		}
		if (this._hasBoneModificationFormulas)
		{
			this._formulasString += "Has bone morphs.\n Animation not recommended\n";
		}
	}

	// Token: 0x06004B1C RID: 19228 RVA: 0x0019F214 File Offset: 0x0019D614
	protected void OpenInPackageManager()
	{
		SuperController.singleton.OpenPackageInManager(this.packageUid);
	}

	// Token: 0x17000ABC RID: 2748
	// (get) Token: 0x06004B1D RID: 19229 RVA: 0x0019F226 File Offset: 0x0019D626
	// (set) Token: 0x06004B1E RID: 19230 RVA: 0x0019F230 File Offset: 0x0019D630
	public Button openInPackageManagerButton
	{
		get
		{
			return this._openInPackageManagerButton;
		}
		set
		{
			if (this._openInPackageManagerButton != value)
			{
				if (this._openInPackageManagerButton != null)
				{
					this._openInPackageManagerButton.onClick.RemoveListener(new UnityAction(this.OpenInPackageManager));
				}
				this._openInPackageManagerButton = value;
				if (this._openInPackageManagerButton != null)
				{
					this._openInPackageManagerButton.gameObject.SetActive(this.isInPackage);
					this._openInPackageManagerButton.onClick.AddListener(new UnityAction(this.OpenInPackageManager));
				}
			}
		}
	}

	// Token: 0x17000ABD RID: 2749
	// (get) Token: 0x06004B1F RID: 19231 RVA: 0x0019F2C5 File Offset: 0x0019D6C5
	// (set) Token: 0x06004B20 RID: 19232 RVA: 0x0019F2D0 File Offset: 0x0019D6D0
	public Button openInPackageManagerButtonAlt
	{
		get
		{
			return this._openInPackageManagerButtonAlt;
		}
		set
		{
			if (this._openInPackageManagerButtonAlt != value)
			{
				if (this._openInPackageManagerButtonAlt != null)
				{
					this._openInPackageManagerButtonAlt.onClick.RemoveListener(new UnityAction(this.OpenInPackageManager));
				}
				this._openInPackageManagerButtonAlt = value;
				if (this._openInPackageManagerButtonAlt != null)
				{
					this._openInPackageManagerButtonAlt.gameObject.SetActive(this.isInPackage);
					this._openInPackageManagerButtonAlt.onClick.AddListener(new UnityAction(this.OpenInPackageManager));
				}
			}
		}
	}

	// Token: 0x17000ABE RID: 2750
	// (get) Token: 0x06004B21 RID: 19233 RVA: 0x0019F365 File Offset: 0x0019D765
	// (set) Token: 0x06004B22 RID: 19234 RVA: 0x0019F36D File Offset: 0x0019D76D
	public Text packageUidText
	{
		get
		{
			return this._packageUidText;
		}
		set
		{
			if (this._packageUidText != value)
			{
				this._packageUidText = value;
				if (this._packageUidText != null)
				{
					this._packageUidText.text = this.packageUid;
				}
			}
		}
	}

	// Token: 0x17000ABF RID: 2751
	// (get) Token: 0x06004B23 RID: 19235 RVA: 0x0019F3A9 File Offset: 0x0019D7A9
	// (set) Token: 0x06004B24 RID: 19236 RVA: 0x0019F3B1 File Offset: 0x0019D7B1
	public Text packageUidTextAlt
	{
		get
		{
			return this._packageUidTextAlt;
		}
		set
		{
			if (this._packageUidTextAlt != value)
			{
				this._packageUidTextAlt = value;
				if (this._packageUidTextAlt != null)
				{
					this._packageUidTextAlt.text = this.packageUid;
				}
			}
		}
	}

	// Token: 0x17000AC0 RID: 2752
	// (get) Token: 0x06004B25 RID: 19237 RVA: 0x0019F3ED File Offset: 0x0019D7ED
	// (set) Token: 0x06004B26 RID: 19238 RVA: 0x0019F3F5 File Offset: 0x0019D7F5
	public Text packageLicenseText
	{
		get
		{
			return this._packageLicenseText;
		}
		set
		{
			if (this._packageLicenseText != value)
			{
				this._packageLicenseText = value;
				if (this._packageLicenseText != null)
				{
					this._packageLicenseText.text = this.packageLicense;
				}
			}
		}
	}

	// Token: 0x17000AC1 RID: 2753
	// (get) Token: 0x06004B27 RID: 19239 RVA: 0x0019F431 File Offset: 0x0019D831
	// (set) Token: 0x06004B28 RID: 19240 RVA: 0x0019F439 File Offset: 0x0019D839
	public Text packageLicenseTextAlt
	{
		get
		{
			return this._packageLicenseTextAlt;
		}
		set
		{
			if (this._packageLicenseTextAlt != value)
			{
				this._packageLicenseTextAlt = value;
				if (this._packageLicenseTextAlt != null)
				{
					this._packageLicenseTextAlt.text = this.packageLicense;
				}
			}
		}
	}

	// Token: 0x17000AC2 RID: 2754
	// (get) Token: 0x06004B29 RID: 19241 RVA: 0x0019F475 File Offset: 0x0019D875
	// (set) Token: 0x06004B2A RID: 19242 RVA: 0x0019F480 File Offset: 0x0019D880
	public Text versionText
	{
		get
		{
			return this._versionText;
		}
		set
		{
			if (this._versionText != value)
			{
				this._versionText = value;
				if (this._versionText != null)
				{
					if (this.version != null && this.version != string.Empty)
					{
						this._versionText.text = this.version;
						this._versionText.gameObject.SetActive(true);
					}
					else
					{
						this._versionText.gameObject.SetActive(false);
					}
				}
			}
		}
	}

	// Token: 0x17000AC3 RID: 2755
	// (get) Token: 0x06004B2B RID: 19243 RVA: 0x0019F50E File Offset: 0x0019D90E
	// (set) Token: 0x06004B2C RID: 19244 RVA: 0x0019F518 File Offset: 0x0019D918
	public Text versionTextAlt
	{
		get
		{
			return this._versionTextAlt;
		}
		set
		{
			if (this._versionTextAlt != value)
			{
				this._versionTextAlt = value;
				if (this._versionTextAlt != null)
				{
					if (this.version != null && this.version != string.Empty)
					{
						this._versionTextAlt.text = this.version;
						this._versionTextAlt.gameObject.SetActive(true);
					}
					else
					{
						this._versionTextAlt.gameObject.SetActive(false);
					}
				}
			}
		}
	}

	// Token: 0x17000AC4 RID: 2756
	// (get) Token: 0x06004B2D RID: 19245 RVA: 0x0019F5A6 File Offset: 0x0019D9A6
	// (set) Token: 0x06004B2E RID: 19246 RVA: 0x0019F5B0 File Offset: 0x0019D9B0
	public Button increaseRangeButton
	{
		get
		{
			return this._increaseRangeButton;
		}
		set
		{
			if (this._increaseRangeButton != value)
			{
				if (this._increaseRangeButton != null)
				{
					this._increaseRangeButton.onClick.RemoveListener(new UnityAction(this.IncreaseRange));
				}
				this._increaseRangeButton = value;
				if (this._increaseRangeButton != null)
				{
					this._increaseRangeButton.onClick.AddListener(new UnityAction(this.IncreaseRange));
				}
			}
		}
	}

	// Token: 0x17000AC5 RID: 2757
	// (get) Token: 0x06004B2F RID: 19247 RVA: 0x0019F62F File Offset: 0x0019DA2F
	// (set) Token: 0x06004B30 RID: 19248 RVA: 0x0019F638 File Offset: 0x0019DA38
	public Button increaseRangeButtonAlt
	{
		get
		{
			return this._increaseRangeButtonAlt;
		}
		set
		{
			if (this._increaseRangeButtonAlt != value)
			{
				if (this._increaseRangeButtonAlt != null)
				{
					this._increaseRangeButtonAlt.onClick.RemoveListener(new UnityAction(this.IncreaseRange));
				}
				this._increaseRangeButtonAlt = value;
				if (this._increaseRangeButtonAlt != null)
				{
					this._increaseRangeButtonAlt.onClick.AddListener(new UnityAction(this.IncreaseRange));
				}
			}
		}
	}

	// Token: 0x06004B31 RID: 19249 RVA: 0x0019F6B8 File Offset: 0x0019DAB8
	public void IncreaseRange()
	{
		this.min = Mathf.Floor(this.min - 1f);
		this.jsonFloat.min = this.min;
		this.max = Mathf.Ceil(this.max + 1f);
		this.jsonFloat.max = this.max;
	}

	// Token: 0x17000AC6 RID: 2758
	// (get) Token: 0x06004B32 RID: 19250 RVA: 0x0019F715 File Offset: 0x0019DB15
	// (set) Token: 0x06004B33 RID: 19251 RVA: 0x0019F720 File Offset: 0x0019DB20
	public Button resetRangeButton
	{
		get
		{
			return this._resetRangeButton;
		}
		set
		{
			if (this._resetRangeButton != value)
			{
				if (this._resetRangeButton != null)
				{
					this._resetRangeButton.onClick.RemoveListener(new UnityAction(this.ResetRange));
				}
				this._resetRangeButton = value;
				if (this._resetRangeButton != null)
				{
					this._resetRangeButton.onClick.AddListener(new UnityAction(this.ResetRange));
				}
			}
		}
	}

	// Token: 0x17000AC7 RID: 2759
	// (get) Token: 0x06004B34 RID: 19252 RVA: 0x0019F79F File Offset: 0x0019DB9F
	// (set) Token: 0x06004B35 RID: 19253 RVA: 0x0019F7A8 File Offset: 0x0019DBA8
	public Button resetRangeButtonAlt
	{
		get
		{
			return this._resetRangeButtonAlt;
		}
		set
		{
			if (this._resetRangeButtonAlt != value)
			{
				if (this._resetRangeButtonAlt != null)
				{
					this._resetRangeButtonAlt.onClick.RemoveListener(new UnityAction(this.ResetRange));
				}
				this._resetRangeButtonAlt = value;
				if (this._resetRangeButtonAlt != null)
				{
					this._resetRangeButtonAlt.onClick.AddListener(new UnityAction(this.ResetRange));
				}
			}
		}
	}

	// Token: 0x06004B36 RID: 19254 RVA: 0x0019F827 File Offset: 0x0019DC27
	public void ResetRange()
	{
		this.min = this.startingMin;
		this.max = this.startingMax;
		this.jsonFloat.min = this.startingMin;
		this.jsonFloat.max = this.startingMax;
	}

	// Token: 0x06004B37 RID: 19255 RVA: 0x0019F864 File Offset: 0x0019DC64
	public void InitUI(Transform UITransform)
	{
		DAZMorphUI component = UITransform.GetComponent<DAZMorphUI>();
		if (component != null)
		{
			this.jsonFloat.slider = component.slider;
			this.increaseRangeButton = component.increaseRangeButton;
			this.openInPackageManagerButton = component.openInPackageButton;
			this.packageUidText = component.packageUidText;
			this.packageLicenseText = component.packageLicenseText;
			this.versionText = component.versionText;
			this.resetRangeButton = component.resetRangeButton;
			if (component.morphNameText != null)
			{
				component.morphNameText.text = this.resolvedDisplayName;
			}
			this.favoriteToggle = component.favoriteToggle;
			this.animatableWarningIndicator = component.animatableWarningIndicator;
			this.panelImage = component.panelImage;
			this.drivenIndicator = component.drivenIndicator;
			this.drivenByText = component.drivenByText;
			this.SyncDrivenUI();
			if (component.hasFormulasIndicator != null)
			{
				component.hasFormulasIndicator.gameObject.SetActive(this._hasMorphValueFormulas || this._hasMCMFormulas || this._hasBoneModificationFormulas);
			}
			if (component.formulasText != null)
			{
				component.formulasText.text = this.formulasString;
			}
			this.zeroKeepChildValuesButton = component.zeroKeepChildValuesButton;
			this.copyUidButton = component.copyUidButton;
			if (component.categoryText != null)
			{
				component.categoryText.text = this.resolvedRegionName;
			}
		}
	}

	// Token: 0x06004B38 RID: 19256 RVA: 0x0019F9E0 File Offset: 0x0019DDE0
	public void DeregisterUI()
	{
		this.jsonFloat.slider = null;
		this.openInPackageManagerButton = null;
		this.packageUidText = null;
		this.packageLicenseText = null;
		this.versionText = null;
		this.increaseRangeButton = null;
		this.resetRangeButton = null;
		this.favoriteToggle = null;
		this.animatableWarningIndicator = null;
		this.panelImage = null;
		this.drivenIndicator = null;
		this.drivenByText = null;
		this.zeroKeepChildValuesButton = null;
	}

	// Token: 0x06004B39 RID: 19257 RVA: 0x0019FA50 File Offset: 0x0019DE50
	public void InitUIAlt(Transform UITransform)
	{
		DAZMorphUI component = UITransform.GetComponent<DAZMorphUI>();
		if (component != null)
		{
			this.jsonFloat.sliderAlt = component.slider;
			this.openInPackageManagerButtonAlt = component.openInPackageButton;
			this.packageUidTextAlt = component.packageUidText;
			this.packageLicenseTextAlt = component.packageLicenseText;
			this.versionTextAlt = component.versionText;
			this.increaseRangeButtonAlt = component.increaseRangeButton;
			this.resetRangeButtonAlt = component.resetRangeButton;
			if (component.morphNameText != null)
			{
				component.morphNameText.text = this.resolvedDisplayName;
			}
			this.animatableWarningIndicatorAlt = component.animatableWarningIndicator;
		}
	}

	// Token: 0x06004B3A RID: 19258 RVA: 0x0019FAF7 File Offset: 0x0019DEF7
	public void DeregisterUIAlt()
	{
		this.jsonFloat.sliderAlt = null;
		this.openInPackageManagerButtonAlt = null;
		this.packageUidTextAlt = null;
		this.packageLicenseTextAlt = null;
		this.versionTextAlt = null;
		this.increaseRangeButtonAlt = null;
		this.resetRangeButtonAlt = null;
		this.animatableWarningIndicatorAlt = null;
	}

	// Token: 0x06004B3B RID: 19259 RVA: 0x0019FB38 File Offset: 0x0019DF38
	public bool StoreJSON(JSONClass jc, bool forceStore = false)
	{
		bool flag = false;
		jc["uid"] = this.uid;
		jc["name"] = this.resolvedDisplayName;
		if (this.jsonFloat.val != this.jsonFloat.defaultVal || forceStore)
		{
			jc["value"].AsFloat = this._morphValue;
			flag = true;
		}
		if (flag && this.isTransient && this.morphBank != null)
		{
			string text = this.morphBank.autoImportFolder + "/AUTO/" + Path.GetFileName(this.metaLoadPath);
			bool flag2 = false;
			if (!FileManager.FileExists(text, true, true))
			{
				try
				{
					string directoryName = Path.GetDirectoryName(text);
					FileManager.CreateDirectory(directoryName);
					FileManager.CopyFile(this.metaLoadPath, text, false);
				}
				catch (Exception arg)
				{
					flag2 = true;
					UnityEngine.Debug.LogError("Could not copy transient morph to import folder " + arg);
				}
			}
			string text2 = this.morphBank.autoImportFolder + "/AUTO/" + Path.GetFileName(this.deltasLoadPath);
			if (!FileManager.FileExists(text2, false, false))
			{
				try
				{
					FileManager.CopyFile(this.deltasLoadPath, text2, false);
				}
				catch (Exception arg2)
				{
					flag2 = true;
					UnityEngine.Debug.LogError("Could not copy transient morph to import folder " + arg2);
				}
			}
			if (!flag2 && this._morphSubBank != null)
			{
				FileEntry fileEntry = FileManager.GetFileEntry(text, false);
				if (fileEntry != null)
				{
					jc["uid"] = fileEntry.Uid;
				}
			}
		}
		return flag;
	}

	// Token: 0x06004B3C RID: 19260 RVA: 0x0019FCF0 File Offset: 0x0019E0F0
	public void RestoreFromJSON(JSONClass jc)
	{
		if (jc["value"] != null)
		{
			this.jsonFloat.val = jc["value"].AsFloat;
		}
	}

	// Token: 0x06004B3D RID: 19261 RVA: 0x0019FD23 File Offset: 0x0019E123
	public void Reset()
	{
		this.ResetRange();
		this.jsonFloat.val = this.jsonFloat.defaultVal;
	}

	// Token: 0x06004B3E RID: 19262 RVA: 0x0019FD41 File Offset: 0x0019E141
	public void SetDefaultValue()
	{
		this.jsonFloat.val = this.jsonFloat.defaultVal;
	}

	// Token: 0x06004B3F RID: 19263 RVA: 0x0019FD5C File Offset: 0x0019E15C
	public void CopyParameters(DAZMorph copyFrom, bool setValue = true)
	{
		this.group = copyFrom.group;
		this.region = copyFrom.region;
		this.overrideRegion = copyFrom.overrideRegion;
		this.morphName = copyFrom.morphName;
		this.displayName = copyFrom.displayName;
		this.overrideName = copyFrom.overrideName;
		this.preserveValueOnReimport = copyFrom.preserveValueOnReimport;
		this.min = copyFrom.min;
		this.max = copyFrom.max;
		this.visible = copyFrom.visible;
		this.disable = copyFrom.disable;
		this.isPoseControl = copyFrom.isPoseControl;
		if (setValue)
		{
			this._morphValue = copyFrom.morphValue;
			this.appliedValue = copyFrom.appliedValue;
		}
		this.triggerNormalRecalc = copyFrom.triggerNormalRecalc;
		this.triggerTangentRecalc = copyFrom.triggerTangentRecalc;
	}

	// Token: 0x06004B40 RID: 19264 RVA: 0x0019FE30 File Offset: 0x0019E230
	private bool ProcessFormula(JSONNode fn, DAZMorphFormula formula, string morphName)
	{
		JSONNode jsonnode = fn["operations"];
		string url = jsonnode[0]["url"];
		string text = DAZImport.DAZurlToId(url);
		if (text == morphName + "?value")
		{
			string text2 = jsonnode[2]["op"];
			if (text2 == "mult")
			{
				float asFloat = jsonnode[1]["val"].AsFloat;
				formula.multiplier = asFloat;
				return true;
			}
			UnityEngine.Debug.LogWarning("Morph " + morphName + ": Found unknown formula " + text2);
		}
		else if (formula.target == morphName)
		{
			string text3 = fn["stage"];
			if (text3 != null)
			{
				if (text3 == "mult")
				{
					formula.targetType = DAZMorphFormulaTargetType.MCMMult;
					text = Regex.Replace(text, "\\?.*", string.Empty);
					formula.target = text;
					return true;
				}
				UnityEngine.Debug.LogWarning("Morph " + morphName + ": Found unknown stage " + text3);
			}
			else
			{
				formula.targetType = DAZMorphFormulaTargetType.MCM;
				text = Regex.Replace(text, "\\?.*", string.Empty);
				formula.target = text;
				string text4 = jsonnode[2]["op"];
				if (text4 == "mult")
				{
					float asFloat2 = jsonnode[1]["val"].AsFloat;
					formula.multiplier = asFloat2;
					return true;
				}
				UnityEngine.Debug.LogWarning("Morph " + morphName + ": Found unknown formula " + text4);
			}
		}
		else
		{
			UnityEngine.Debug.LogWarning("Morph " + morphName + ": Found unknown operation url " + text);
		}
		return false;
	}

	// Token: 0x06004B41 RID: 19265 RVA: 0x0019FFF8 File Offset: 0x0019E3F8
	public void Import(JSONNode mn)
	{
		this.morphName = mn["id"];
		this._morphValue = 0f;
		this.appliedValue = 0f;
		if (mn["group"] != null)
		{
			this.group = Regex.Replace(mn["group"], "^/", string.Empty);
		}
		else
		{
			this.group = string.Empty;
		}
		this.displayName = mn["channel"]["label"];
		this.region = mn["region"];
		if (this.region == null)
		{
			this.region = this.group;
		}
		this.min = mn["channel"]["min"].AsFloat;
		if (this.min == -100000f)
		{
			this.min = -1f;
		}
		this.max = mn["channel"]["max"].AsFloat;
		if (this.max == 100000f)
		{
			this.max = 1f;
		}
		if (mn["formulas"].Count > 0)
		{
			List<DAZMorphFormula> list = new List<DAZMorphFormula>();
			IEnumerator enumerator = mn["formulas"].AsArray.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					JSONNode jsonnode = (JSONNode)obj;
					DAZMorphFormula dazmorphFormula = new DAZMorphFormula();
					string text = jsonnode["output"];
					string text2 = Regex.Replace(text, "^.*#", string.Empty);
					text2 = Regex.Replace(text2, "\\?.*", string.Empty);
					text2 = DAZImport.DAZurlFix(text2);
					dazmorphFormula.target = text2;
					string text3 = Regex.Replace(text, "^.*\\?", string.Empty);
					if (text3 == "value")
					{
						dazmorphFormula.targetType = DAZMorphFormulaTargetType.MorphValue;
						if (this.ProcessFormula(jsonnode, dazmorphFormula, this.morphName))
						{
							list.Add(dazmorphFormula);
						}
					}
					else if (text3 == "scale/general")
					{
						dazmorphFormula.targetType = DAZMorphFormulaTargetType.GeneralScale;
						if (this.ProcessFormula(jsonnode, dazmorphFormula, this.morphName))
						{
							list.Add(dazmorphFormula);
						}
					}
					else if (text3 == "scale/x")
					{
						dazmorphFormula.targetType = DAZMorphFormulaTargetType.ScaleX;
						if (this.ProcessFormula(jsonnode, dazmorphFormula, this.morphName))
						{
							list.Add(dazmorphFormula);
						}
					}
					else if (text3 == "scale/y")
					{
						dazmorphFormula.targetType = DAZMorphFormulaTargetType.ScaleY;
						if (this.ProcessFormula(jsonnode, dazmorphFormula, this.morphName))
						{
							list.Add(dazmorphFormula);
						}
					}
					else if (text3 == "scale/z")
					{
						dazmorphFormula.targetType = DAZMorphFormulaTargetType.ScaleZ;
						if (this.ProcessFormula(jsonnode, dazmorphFormula, this.morphName))
						{
							list.Add(dazmorphFormula);
						}
					}
					else if (text3 == "center_point/x")
					{
						dazmorphFormula.targetType = DAZMorphFormulaTargetType.BoneCenterX;
						if (this.ProcessFormula(jsonnode, dazmorphFormula, this.morphName))
						{
							dazmorphFormula.multiplier = -dazmorphFormula.multiplier * 0.01f;
							list.Add(dazmorphFormula);
						}
					}
					else if (text3 == "center_point/y")
					{
						dazmorphFormula.targetType = DAZMorphFormulaTargetType.BoneCenterY;
						if (this.ProcessFormula(jsonnode, dazmorphFormula, this.morphName))
						{
							dazmorphFormula.multiplier *= 0.01f;
							list.Add(dazmorphFormula);
						}
					}
					else if (text3 == "center_point/z")
					{
						dazmorphFormula.targetType = DAZMorphFormulaTargetType.BoneCenterZ;
						if (this.ProcessFormula(jsonnode, dazmorphFormula, this.morphName))
						{
							dazmorphFormula.multiplier *= 0.01f;
							list.Add(dazmorphFormula);
						}
					}
					else if (!Regex.IsMatch(text3, "^end_point"))
					{
						if (text3 == "orientation/x")
						{
							dazmorphFormula.targetType = DAZMorphFormulaTargetType.OrientationX;
							if (this.ProcessFormula(jsonnode, dazmorphFormula, this.morphName))
							{
								list.Add(dazmorphFormula);
							}
						}
						else if (text3 == "orientation/y")
						{
							dazmorphFormula.targetType = DAZMorphFormulaTargetType.OrientationY;
							if (this.ProcessFormula(jsonnode, dazmorphFormula, this.morphName))
							{
								dazmorphFormula.multiplier *= -1f;
								list.Add(dazmorphFormula);
							}
						}
						else if (text3 == "orientation/z")
						{
							dazmorphFormula.targetType = DAZMorphFormulaTargetType.OrientationZ;
							if (this.ProcessFormula(jsonnode, dazmorphFormula, this.morphName))
							{
								dazmorphFormula.multiplier *= -1f;
								list.Add(dazmorphFormula);
							}
						}
						else if (text3 == "rotation/x")
						{
							dazmorphFormula.targetType = DAZMorphFormulaTargetType.RotationX;
							if (this.ProcessFormula(jsonnode, dazmorphFormula, this.morphName))
							{
								list.Add(dazmorphFormula);
							}
						}
						else if (text3 == "rotation/y")
						{
							dazmorphFormula.targetType = DAZMorphFormulaTargetType.RotationY;
							if (this.ProcessFormula(jsonnode, dazmorphFormula, this.morphName))
							{
								list.Add(dazmorphFormula);
							}
						}
						else if (text3 == "rotation/z")
						{
							dazmorphFormula.targetType = DAZMorphFormulaTargetType.RotationZ;
							if (this.ProcessFormula(jsonnode, dazmorphFormula, this.morphName))
							{
								list.Add(dazmorphFormula);
							}
						}
						else
						{
							UnityEngine.Debug.LogWarning("Morph " + this.morphName + " has unknown output type " + text);
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
			this.formulas = list.ToArray();
		}
		else
		{
			this.formulas = new DAZMorphFormula[0];
		}
		this.numDeltas = mn["morph"]["deltas"]["count"].AsInt;
		this.deltas = new DAZMorphVertex[this.numDeltas];
		int num = 0;
		IEnumerator enumerator2 = mn["morph"]["deltas"]["values"].AsArray.GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				object obj2 = enumerator2.Current;
				JSONNode jsonnode2 = (JSONNode)obj2;
				int asInt = jsonnode2[0].AsInt;
				Vector3 delta;
				delta.x = -jsonnode2[1].AsFloat * 0.01f;
				delta.y = jsonnode2[2].AsFloat * 0.01f;
				delta.z = jsonnode2[3].AsFloat * 0.01f;
				DAZMorphVertex dazmorphVertex = new DAZMorphVertex();
				dazmorphVertex.vertex = asInt;
				dazmorphVertex.delta = delta;
				this.deltas[num] = dazmorphVertex;
				num++;
			}
		}
		finally
		{
			IDisposable disposable2;
			if ((disposable2 = (enumerator2 as IDisposable)) != null)
			{
				disposable2.Dispose();
			}
		}
	}

	// Token: 0x06004B42 RID: 19266 RVA: 0x001A071C File Offset: 0x0019EB1C
	public void LoadMeta()
	{
		if (this.metaJSONFile != null)
		{
			JSONNode jsonnode = DAZImport.ReadJSON(this.metaJSONFile);
			if (jsonnode != null)
			{
				this.LoadMetaFromJSON(jsonnode);
			}
		}
	}

	// Token: 0x06004B43 RID: 19267 RVA: 0x001A0754 File Offset: 0x0019EB54
	public void LoadMetaFromJSON(JSONNode metan)
	{
		this.morphName = metan["id"];
		this.displayName = metan["displayName"];
		this.overrideName = metan["overrideName"];
		this.group = metan["group"];
		this.region = metan["region"];
		this.overrideRegion = metan["overrideRegion"];
		if (metan["min"] != null)
		{
			this.min = metan["min"].AsFloat;
		}
		if (metan["max"] != null)
		{
			this.max = metan["max"].AsFloat;
		}
		if (metan["numDeltas"] != null)
		{
			this.numDeltas = metan["numDeltas"].AsInt;
		}
		if (metan["isPoseControl"] != null)
		{
			this.isPoseControl = metan["isPoseControl"].AsBool;
		}
		if (metan["formulas"] != null)
		{
			List<DAZMorphFormula> list = new List<DAZMorphFormula>();
			IEnumerator enumerator = metan["formulas"].AsArray.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					JSONClass jsonclass = (JSONClass)obj;
					DAZMorphFormula dazmorphFormula = new DAZMorphFormula();
					try
					{
						bool flag = false;
						if (jsonclass["targetType"] != null)
						{
							dazmorphFormula.targetType = (DAZMorphFormulaTargetType)Enum.Parse(typeof(DAZMorphFormulaTargetType), jsonclass["targetType"]);
						}
						else
						{
							flag = true;
						}
						if (jsonclass["target"] != null)
						{
							dazmorphFormula.target = jsonclass["target"];
						}
						else
						{
							flag = true;
						}
						if (jsonclass["multiplier"] != null)
						{
							dazmorphFormula.multiplier = jsonclass["multiplier"].AsFloat;
						}
						else
						{
							flag = true;
						}
						if (!flag)
						{
							list.Add(dazmorphFormula);
						}
					}
					catch (ArgumentException)
					{
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
			this.formulas = list.ToArray();
		}
	}

	// Token: 0x06004B44 RID: 19268 RVA: 0x001A09F8 File Offset: 0x0019EDF8
	public JSONClass GetMetaJSON()
	{
		JSONClass jsonclass = new JSONClass();
		if (this.morphName != null)
		{
			jsonclass["id"] = this.morphName;
		}
		if (this.displayName != null)
		{
			jsonclass["displayName"] = this.displayName;
		}
		if (this.overrideName != null)
		{
			jsonclass["overrideName"] = this.overrideName;
		}
		if (this.group != null)
		{
			jsonclass["group"] = this.group;
		}
		if (this.region != null)
		{
			jsonclass["region"] = this.region;
		}
		if (this.overrideRegion != null)
		{
			jsonclass["overrideRegion"] = this.region;
		}
		jsonclass["min"].AsFloat = this.min;
		jsonclass["max"].AsFloat = this.max;
		jsonclass["numDeltas"].AsInt = this.numDeltas;
		jsonclass["isPoseControl"].AsBool = this.isPoseControl;
		JSONArray jsonarray = new JSONArray();
		foreach (DAZMorphFormula dazmorphFormula in this.formulas)
		{
			JSONClass jsonclass2 = new JSONClass();
			if (dazmorphFormula.targetType == DAZMorphFormulaTargetType.MCM || dazmorphFormula.targetType == DAZMorphFormulaTargetType.MCMMult)
			{
				UnityEngine.Debug.LogError("Morph " + this.morphName + " will not be compiled because it is an MCM morph which can cause unwanted changes to shape");
				return null;
			}
			jsonclass2["targetType"] = dazmorphFormula.targetType.ToString();
			jsonclass2["target"] = dazmorphFormula.target;
			jsonclass2["multiplier"].AsFloat = dazmorphFormula.multiplier;
			jsonarray.Add(jsonclass2);
		}
		jsonclass["formulas"] = jsonarray;
		return jsonclass;
	}

	// Token: 0x06004B45 RID: 19269 RVA: 0x001A0BF5 File Offset: 0x0019EFF5
	public void LoadDeltas()
	{
		if (this.deltasLoadPath != null && this.deltasLoadPath != string.Empty && !this.deltasLoaded)
		{
			this.deltasLoaded = true;
			this.LoadDeltasFromBinaryFile(this.deltasLoadPath);
		}
	}

	// Token: 0x06004B46 RID: 19270 RVA: 0x001A0C35 File Offset: 0x0019F035
	public void UnloadDeltas()
	{
		if (this.isRuntime && this.deltasLoaded)
		{
			this.deltas = null;
			this.deltasLoaded = false;
		}
	}

	// Token: 0x06004B47 RID: 19271 RVA: 0x001A0C5C File Offset: 0x0019F05C
	public void LoadDeltasFromJSON(JSONNode deltan)
	{
		JSONArray asArray = deltan["deltas"].AsArray;
		if (asArray != null)
		{
			this.numDeltas = asArray.Count;
			this.deltas = new DAZMorphVertex[this.numDeltas];
			int num = 0;
			IEnumerator enumerator = asArray.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					JSONNode jsonnode = (JSONNode)obj;
					DAZMorphVertex dazmorphVertex = new DAZMorphVertex();
					Vector3 delta;
					delta.x = jsonnode["x"].AsFloat;
					delta.y = jsonnode["y"].AsFloat;
					delta.z = jsonnode["z"].AsFloat;
					dazmorphVertex.delta = delta;
					dazmorphVertex.vertex = jsonnode["vid"].AsInt;
					this.deltas[num] = dazmorphVertex;
					num++;
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

	// Token: 0x06004B48 RID: 19272 RVA: 0x001A0D70 File Offset: 0x0019F170
	public void LoadDeltasFromBinaryFile(string path)
	{
		try
		{
			using (FileEntryStream fileEntryStream = FileManager.OpenStream(path, true))
			{
				using (BinaryReader binaryReader = new BinaryReader(fileEntryStream.Stream))
				{
					this.numDeltas = binaryReader.ReadInt32();
					this.deltas = new DAZMorphVertex[this.numDeltas];
					for (int i = 0; i < this.numDeltas; i++)
					{
						DAZMorphVertex dazmorphVertex = new DAZMorphVertex();
						dazmorphVertex.vertex = binaryReader.ReadInt32();
						Vector3 delta;
						delta.x = binaryReader.ReadSingle();
						delta.y = binaryReader.ReadSingle();
						delta.z = binaryReader.ReadSingle();
						dazmorphVertex.delta = delta;
						this.deltas[i] = dazmorphVertex;
					}
				}
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError(string.Concat(new object[]
			{
				"Error while loading binary delta file ",
				path,
				" ",
				ex
			}));
		}
	}

	// Token: 0x06004B49 RID: 19273 RVA: 0x001A0E90 File Offset: 0x0019F290
	public void SaveDeltasToBinaryFile(string path)
	{
		using (FileStream fileStream = FileManager.OpenStreamForCreate(path))
		{
			using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
			{
				binaryWriter.Write(this.numDeltas);
				foreach (DAZMorphVertex dazmorphVertex in this.deltas)
				{
					binaryWriter.Write(dazmorphVertex.vertex);
					binaryWriter.Write(dazmorphVertex.delta.x);
					binaryWriter.Write(dazmorphVertex.delta.y);
					binaryWriter.Write(dazmorphVertex.delta.z);
				}
			}
		}
	}

	// Token: 0x06004B4A RID: 19274 RVA: 0x001A0F5C File Offset: 0x0019F35C
	public JSONClass GetDeltasJSON()
	{
		JSONClass jsonclass = new JSONClass();
		JSONArray jsonarray = new JSONArray();
		foreach (DAZMorphVertex dazmorphVertex in this.deltas)
		{
			JSONClass jsonclass2 = new JSONClass();
			jsonclass2["x"].AsFloat = dazmorphVertex.delta.x;
			jsonclass2["y"].AsFloat = dazmorphVertex.delta.y;
			jsonclass2["z"].AsFloat = dazmorphVertex.delta.z;
			jsonclass2["vid"].AsInt = dazmorphVertex.vertex;
			jsonarray.Add(jsonclass2);
		}
		jsonclass["deltas"] = jsonarray;
		return jsonclass;
	}

	// Token: 0x06004B4B RID: 19275 RVA: 0x001A1020 File Offset: 0x0019F420
	// Note: this type is marked as 'beforefieldinit'.
	static DAZMorph()
	{
	}

	// Token: 0x040039D1 RID: 14801
	private const float geoScale = 0.01f;

	// Token: 0x040039D2 RID: 14802
	public bool visible;

	// Token: 0x040039D3 RID: 14803
	public bool preserveValueOnReimport;

	// Token: 0x040039D4 RID: 14804
	public bool disable;

	// Token: 0x040039D5 RID: 14805
	public bool isPoseControl;

	// Token: 0x040039D6 RID: 14806
	protected string _uid;

	// Token: 0x040039D7 RID: 14807
	public string morphName;

	// Token: 0x040039D8 RID: 14808
	public string displayName;

	// Token: 0x040039D9 RID: 14809
	public string overrideName;

	// Token: 0x040039DA RID: 14810
	protected string _lowerCaseResolvedDisplayName;

	// Token: 0x040039DB RID: 14811
	public string region;

	// Token: 0x040039DC RID: 14812
	public string overrideRegion;

	// Token: 0x040039DD RID: 14813
	public string group;

	// Token: 0x040039DE RID: 14814
	public float importValue;

	// Token: 0x040039DF RID: 14815
	public float startValue;

	// Token: 0x040039E0 RID: 14816
	[SerializeField]
	private float _morphValue;

	// Token: 0x040039E1 RID: 14817
	public bool active;

	// Token: 0x040039E2 RID: 14818
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private bool <activeImmediate>k__BackingField;

	// Token: 0x040039E3 RID: 14819
	protected RectTransform drivenIndicator;

	// Token: 0x040039E4 RID: 14820
	protected Text drivenByText;

	// Token: 0x040039E5 RID: 14821
	protected bool _isDriven;

	// Token: 0x040039E6 RID: 14822
	protected string _drivenBy;

	// Token: 0x040039E7 RID: 14823
	public float appliedValue;

	// Token: 0x040039E8 RID: 14824
	public float min;

	// Token: 0x040039E9 RID: 14825
	protected float startingMin;

	// Token: 0x040039EA RID: 14826
	public float max;

	// Token: 0x040039EB RID: 14827
	protected float startingMax;

	// Token: 0x040039EC RID: 14828
	public int numDeltas;

	// Token: 0x040039ED RID: 14829
	public bool triggerNormalRecalc = true;

	// Token: 0x040039EE RID: 14830
	public bool triggerTangentRecalc = true;

	// Token: 0x040039EF RID: 14831
	public DAZMorphVertex[] deltas;

	// Token: 0x040039F0 RID: 14832
	public DAZMorphFormula[] formulas;

	// Token: 0x040039F1 RID: 14833
	protected string _formulasString;

	// Token: 0x040039F2 RID: 14834
	protected bool _hasFormulas;

	// Token: 0x040039F3 RID: 14835
	protected bool _hasBoneModificationFormulas;

	// Token: 0x040039F4 RID: 14836
	protected bool _hasBoneRotationFormulas;

	// Token: 0x040039F5 RID: 14837
	protected bool _hasMorphValueFormulas;

	// Token: 0x040039F6 RID: 14838
	protected bool _hasMCMFormulas;

	// Token: 0x040039F7 RID: 14839
	protected bool _wasZeroedKeepChildValues;

	// Token: 0x040039F8 RID: 14840
	protected Button _zeroKeepChildValuesButton;

	// Token: 0x040039F9 RID: 14841
	public static string uidCopy = string.Empty;

	// Token: 0x040039FA RID: 14842
	protected Button _copyUidButton;

	// Token: 0x040039FB RID: 14843
	[NonSerialized]
	public JSONStorableFloat jsonFloat;

	// Token: 0x040039FC RID: 14844
	protected RectTransform _animatableWarningIndicator;

	// Token: 0x040039FD RID: 14845
	protected RectTransform _animatableWarningIndicatorAlt;

	// Token: 0x040039FE RID: 14846
	protected DAZMorphBank _morphBank;

	// Token: 0x040039FF RID: 14847
	protected DAZMorphSubBank _morphSubBank;

	// Token: 0x04003A00 RID: 14848
	protected static Color highCostColor = Color.red;

	// Token: 0x04003A01 RID: 14849
	protected static Color normalColor = Color.white;

	// Token: 0x04003A02 RID: 14850
	protected static Color transientColor = new Color(1f, 1f, 0.8f);

	// Token: 0x04003A03 RID: 14851
	protected static Color runtimeColor = new Color(0.8f, 0.8f, 1f);

	// Token: 0x04003A04 RID: 14852
	protected static Color packageColor = new Color(1f, 0.85f, 0.9f);

	// Token: 0x04003A05 RID: 14853
	protected Image _panelImage;

	// Token: 0x04003A06 RID: 14854
	protected Toggle _favoriteToggle;

	// Token: 0x04003A07 RID: 14855
	protected bool _favorite;

	// Token: 0x04003A08 RID: 14856
	public bool isTransient;

	// Token: 0x04003A09 RID: 14857
	public bool isRuntime;

	// Token: 0x04003A0A RID: 14858
	public bool isDemandLoaded;

	// Token: 0x04003A0B RID: 14859
	public bool isDemandActivated;

	// Token: 0x04003A0C RID: 14860
	public string packageUid;

	// Token: 0x04003A0D RID: 14861
	public string packageLicense;

	// Token: 0x04003A0E RID: 14862
	public bool isInPackage;

	// Token: 0x04003A0F RID: 14863
	public bool isLatestVersion = true;

	// Token: 0x04003A10 RID: 14864
	public string version;

	// Token: 0x04003A11 RID: 14865
	public string metaLoadPath = string.Empty;

	// Token: 0x04003A12 RID: 14866
	protected Button _openInPackageManagerButton;

	// Token: 0x04003A13 RID: 14867
	protected Button _openInPackageManagerButtonAlt;

	// Token: 0x04003A14 RID: 14868
	protected Text _packageUidText;

	// Token: 0x04003A15 RID: 14869
	protected Text _packageUidTextAlt;

	// Token: 0x04003A16 RID: 14870
	protected Text _packageLicenseText;

	// Token: 0x04003A17 RID: 14871
	protected Text _packageLicenseTextAlt;

	// Token: 0x04003A18 RID: 14872
	protected Text _versionText;

	// Token: 0x04003A19 RID: 14873
	protected Text _versionTextAlt;

	// Token: 0x04003A1A RID: 14874
	protected Button _increaseRangeButton;

	// Token: 0x04003A1B RID: 14875
	protected Button _increaseRangeButtonAlt;

	// Token: 0x04003A1C RID: 14876
	protected Button _resetRangeButton;

	// Token: 0x04003A1D RID: 14877
	protected Button _resetRangeButtonAlt;

	// Token: 0x04003A1E RID: 14878
	public string metaJSONFile;

	// Token: 0x04003A1F RID: 14879
	public bool deltasLoaded;

	// Token: 0x04003A20 RID: 14880
	public string deltasLoadPath = string.Empty;
}
