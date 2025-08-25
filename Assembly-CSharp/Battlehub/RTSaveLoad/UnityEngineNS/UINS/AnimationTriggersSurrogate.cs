using System;
using System.Runtime.Serialization;
using ProtoBuf;
using UnityEngine.UI;

namespace Battlehub.RTSaveLoad.UnityEngineNS.UINS
{
	// Token: 0x020001E0 RID: 480
	[ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
	public class AnimationTriggersSurrogate : ISerializationSurrogate
	{
		// Token: 0x060009A6 RID: 2470 RVA: 0x0003B7B5 File Offset: 0x00039BB5
		public AnimationTriggersSurrogate()
		{
		}

		// Token: 0x060009A7 RID: 2471 RVA: 0x0003B7C0 File Offset: 0x00039BC0
		public static implicit operator AnimationTriggers(AnimationTriggersSurrogate v)
		{
			return new AnimationTriggers
			{
				normalTrigger = v.normalTrigger,
				highlightedTrigger = v.highlightedTrigger,
				pressedTrigger = v.pressedTrigger,
				disabledTrigger = v.disabledTrigger
			};
		}

		// Token: 0x060009A8 RID: 2472 RVA: 0x0003B804 File Offset: 0x00039C04
		public static implicit operator AnimationTriggersSurrogate(AnimationTriggers v)
		{
			return new AnimationTriggersSurrogate
			{
				normalTrigger = v.normalTrigger,
				highlightedTrigger = v.highlightedTrigger,
				pressedTrigger = v.pressedTrigger,
				disabledTrigger = v.disabledTrigger
			};
		}

		// Token: 0x060009A9 RID: 2473 RVA: 0x0003B848 File Offset: 0x00039C48
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			AnimationTriggers animationTriggers = (AnimationTriggers)obj;
			info.AddValue("normalTrigger", animationTriggers.normalTrigger);
			info.AddValue("highlightedTrigger", animationTriggers.highlightedTrigger);
			info.AddValue("pressedTrigger", animationTriggers.pressedTrigger);
			info.AddValue("disabledTrigger", animationTriggers.disabledTrigger);
		}

		// Token: 0x060009AA RID: 2474 RVA: 0x0003B8A0 File Offset: 0x00039CA0
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			AnimationTriggers animationTriggers = (AnimationTriggers)obj;
			animationTriggers.normalTrigger = (string)info.GetValue("normalTrigger", typeof(string));
			animationTriggers.highlightedTrigger = (string)info.GetValue("highlightedTrigger", typeof(string));
			animationTriggers.pressedTrigger = (string)info.GetValue("pressedTrigger", typeof(string));
			animationTriggers.disabledTrigger = (string)info.GetValue("disabledTrigger", typeof(string));
			return animationTriggers;
		}

		// Token: 0x04000ADB RID: 2779
		public string normalTrigger;

		// Token: 0x04000ADC RID: 2780
		public string highlightedTrigger;

		// Token: 0x04000ADD RID: 2781
		public string pressedTrigger;

		// Token: 0x04000ADE RID: 2782
		public string disabledTrigger;
	}
}
