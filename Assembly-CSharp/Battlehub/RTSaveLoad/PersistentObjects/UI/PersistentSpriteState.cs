using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;
using UnityEngine.UI;

namespace Battlehub.RTSaveLoad.PersistentObjects.UI
{
	// Token: 0x02000225 RID: 549
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentSpriteState : PersistentData
	{
		// Token: 0x06000B02 RID: 2818 RVA: 0x00044180 File Offset: 0x00042580
		public PersistentSpriteState()
		{
		}

		// Token: 0x06000B03 RID: 2819 RVA: 0x00044188 File Offset: 0x00042588
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			SpriteState spriteState = (SpriteState)obj;
			spriteState.highlightedSprite = (Sprite)objects.Get(this.highlightedSprite);
			spriteState.pressedSprite = (Sprite)objects.Get(this.pressedSprite);
			spriteState.disabledSprite = (Sprite)objects.Get(this.disabledSprite);
			return spriteState;
		}

		// Token: 0x06000B04 RID: 2820 RVA: 0x000441FC File Offset: 0x000425FC
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			SpriteState spriteState = (SpriteState)obj;
			this.highlightedSprite = spriteState.highlightedSprite.GetMappedInstanceID();
			this.pressedSprite = spriteState.pressedSprite.GetMappedInstanceID();
			this.disabledSprite = spriteState.disabledSprite.GetMappedInstanceID();
		}

		// Token: 0x04000C5C RID: 3164
		public long highlightedSprite;

		// Token: 0x04000C5D RID: 3165
		public long pressedSprite;

		// Token: 0x04000C5E RID: 3166
		public long disabledSprite;
	}
}
