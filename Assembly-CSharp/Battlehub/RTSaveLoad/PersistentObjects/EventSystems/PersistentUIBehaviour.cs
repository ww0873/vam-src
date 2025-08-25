﻿using System;
using Battlehub.RTSaveLoad.PersistentObjects.UI;
using ProtoBuf;

namespace Battlehub.RTSaveLoad.PersistentObjects.EventSystems
{
	// Token: 0x020001D2 RID: 466
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[ProtoInclude(1108, typeof(PersistentEventSystem))]
	[ProtoInclude(1109, typeof(PersistentBaseInput))]
	[ProtoInclude(1110, typeof(PersistentBaseInputModule))]
	[ProtoInclude(1111, typeof(PersistentBaseRaycaster))]
	[ProtoInclude(1112, typeof(PersistentGraphic))]
	[ProtoInclude(1113, typeof(PersistentMask))]
	[ProtoInclude(1114, typeof(PersistentRectMask2D))]
	[ProtoInclude(1115, typeof(PersistentScrollRect))]
	[ProtoInclude(1116, typeof(PersistentSelectable))]
	[ProtoInclude(1117, typeof(PersistentToggleGroup))]
	[ProtoInclude(1118, typeof(PersistentAspectRatioFitter))]
	[ProtoInclude(1119, typeof(PersistentCanvasScaler))]
	[ProtoInclude(1120, typeof(PersistentContentSizeFitter))]
	[ProtoInclude(1121, typeof(PersistentLayoutElement))]
	[ProtoInclude(1122, typeof(PersistentLayoutGroup))]
	[ProtoInclude(1123, typeof(PersistentBaseMeshEffect))]
	[Serializable]
	public class PersistentUIBehaviour : PersistentMonoBehaviour
	{
		// Token: 0x06000976 RID: 2422 RVA: 0x00031661 File Offset: 0x0002FA61
		public PersistentUIBehaviour()
		{
		}
	}
}
