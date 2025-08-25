using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;
using UnityEngine.UI;

namespace Battlehub.RTSaveLoad.PersistentObjects.UI
{
	// Token: 0x0200021D RID: 541
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentInputField : PersistentSelectable
	{
		// Token: 0x06000ADD RID: 2781 RVA: 0x00043561 File Offset: 0x00041961
		public PersistentInputField()
		{
		}

		// Token: 0x06000ADE RID: 2782 RVA: 0x0004356C File Offset: 0x0004196C
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			InputField inputField = (InputField)obj;
			inputField.shouldHideMobileInput = this.shouldHideMobileInput;
			inputField.text = this.text;
			inputField.caretBlinkRate = this.caretBlinkRate;
			inputField.caretWidth = this.caretWidth;
			inputField.textComponent = (Text)objects.Get(this.textComponent);
			inputField.placeholder = (Graphic)objects.Get(this.placeholder);
			inputField.caretColor = this.caretColor;
			inputField.customCaretColor = this.customCaretColor;
			inputField.selectionColor = this.selectionColor;
			base.Write<InputField.SubmitEvent>(inputField.onEndEdit, this.onEndEdit, objects);
			base.Write<InputField.OnChangeEvent>(inputField.onValueChanged, this.onValueChanged, objects);
			inputField.onValidateInput = this.onValidateInput;
			inputField.characterLimit = this.characterLimit;
			inputField.contentType = this.contentType;
			inputField.lineType = this.lineType;
			inputField.inputType = this.inputType;
			inputField.keyboardType = this.keyboardType;
			inputField.characterValidation = this.characterValidation;
			inputField.readOnly = this.readOnly;
			inputField.asteriskChar = this.asteriskChar;
			inputField.caretPosition = this.caretPosition;
			inputField.selectionAnchorPosition = this.selectionAnchorPosition;
			inputField.selectionFocusPosition = this.selectionFocusPosition;
			return inputField;
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x000436D0 File Offset: 0x00041AD0
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			InputField inputField = (InputField)obj;
			this.shouldHideMobileInput = inputField.shouldHideMobileInput;
			this.text = inputField.text;
			this.caretBlinkRate = inputField.caretBlinkRate;
			this.caretWidth = inputField.caretWidth;
			this.textComponent = inputField.textComponent.GetMappedInstanceID();
			this.placeholder = inputField.placeholder.GetMappedInstanceID();
			this.caretColor = inputField.caretColor;
			this.customCaretColor = inputField.customCaretColor;
			this.selectionColor = inputField.selectionColor;
			base.Read(this.onEndEdit, inputField.onEndEdit);
			base.Read(this.onValueChanged, inputField.onValueChanged);
			this.onValidateInput = inputField.onValidateInput;
			this.characterLimit = inputField.characterLimit;
			this.contentType = inputField.contentType;
			this.lineType = inputField.lineType;
			this.inputType = inputField.inputType;
			this.keyboardType = inputField.keyboardType;
			this.characterValidation = inputField.characterValidation;
			this.readOnly = inputField.readOnly;
			this.asteriskChar = inputField.asteriskChar;
			this.caretPosition = inputField.caretPosition;
			this.selectionAnchorPosition = inputField.selectionAnchorPosition;
			this.selectionFocusPosition = inputField.selectionFocusPosition;
		}

		// Token: 0x06000AE0 RID: 2784 RVA: 0x00043820 File Offset: 0x00041C20
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.textComponent, dependencies, objects, allowNulls);
			base.AddDependency<T>(this.placeholder, dependencies, objects, allowNulls);
			if (this.onEndEdit != null)
			{
				this.onEndEdit.FindDependencies<T>(dependencies, objects, allowNulls);
			}
			if (this.onValueChanged != null)
			{
				this.onValueChanged.FindDependencies<T>(dependencies, objects, allowNulls);
			}
		}

		// Token: 0x06000AE1 RID: 2785 RVA: 0x00043888 File Offset: 0x00041C88
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			InputField inputField = (InputField)obj;
			base.AddDependency(inputField.textComponent, dependencies);
			base.AddDependency(inputField.placeholder, dependencies);
			PersistentUnityEventBase persistentUnityEventBase = new PersistentUnityEventBase();
			persistentUnityEventBase.GetDependencies(inputField.onEndEdit, dependencies);
			persistentUnityEventBase.GetDependencies(inputField.onValueChanged, dependencies);
		}

		// Token: 0x04000C11 RID: 3089
		public bool shouldHideMobileInput;

		// Token: 0x04000C12 RID: 3090
		public string text;

		// Token: 0x04000C13 RID: 3091
		public float caretBlinkRate;

		// Token: 0x04000C14 RID: 3092
		public int caretWidth;

		// Token: 0x04000C15 RID: 3093
		public long textComponent;

		// Token: 0x04000C16 RID: 3094
		public long placeholder;

		// Token: 0x04000C17 RID: 3095
		public Color caretColor;

		// Token: 0x04000C18 RID: 3096
		public bool customCaretColor;

		// Token: 0x04000C19 RID: 3097
		public Color selectionColor;

		// Token: 0x04000C1A RID: 3098
		public PersistentUnityEventBase onEndEdit;

		// Token: 0x04000C1B RID: 3099
		public PersistentUnityEventBase onValueChanged;

		// Token: 0x04000C1C RID: 3100
		public InputField.OnValidateInput onValidateInput;

		// Token: 0x04000C1D RID: 3101
		public int characterLimit;

		// Token: 0x04000C1E RID: 3102
		public InputField.ContentType contentType;

		// Token: 0x04000C1F RID: 3103
		public InputField.LineType lineType;

		// Token: 0x04000C20 RID: 3104
		public InputField.InputType inputType;

		// Token: 0x04000C21 RID: 3105
		public TouchScreenKeyboardType keyboardType;

		// Token: 0x04000C22 RID: 3106
		public InputField.CharacterValidation characterValidation;

		// Token: 0x04000C23 RID: 3107
		public bool readOnly;

		// Token: 0x04000C24 RID: 3108
		public char asteriskChar;

		// Token: 0x04000C25 RID: 3109
		public int caretPosition;

		// Token: 0x04000C26 RID: 3110
		public int selectionAnchorPosition;

		// Token: 0x04000C27 RID: 3111
		public int selectionFocusPosition;
	}
}
