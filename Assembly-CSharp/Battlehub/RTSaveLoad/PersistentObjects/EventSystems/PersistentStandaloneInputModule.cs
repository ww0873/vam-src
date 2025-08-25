using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Battlehub.RTSaveLoad.PersistentObjects.EventSystems
{
	// Token: 0x020001C1 RID: 449
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentStandaloneInputModule : PersistentPointerInputModule
	{
		// Token: 0x06000935 RID: 2357 RVA: 0x00039370 File Offset: 0x00037770
		public PersistentStandaloneInputModule()
		{
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x00039378 File Offset: 0x00037778
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			StandaloneInputModule standaloneInputModule = (StandaloneInputModule)obj;
			standaloneInputModule.forceModuleActive = this.forceModuleActive;
			standaloneInputModule.inputActionsPerSecond = this.inputActionsPerSecond;
			standaloneInputModule.repeatDelay = this.repeatDelay;
			standaloneInputModule.horizontalAxis = this.horizontalAxis;
			standaloneInputModule.verticalAxis = this.verticalAxis;
			standaloneInputModule.submitButton = this.submitButton;
			standaloneInputModule.cancelButton = this.cancelButton;
			return standaloneInputModule;
		}

		// Token: 0x06000937 RID: 2359 RVA: 0x000393F4 File Offset: 0x000377F4
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			StandaloneInputModule standaloneInputModule = (StandaloneInputModule)obj;
			this.forceModuleActive = standaloneInputModule.forceModuleActive;
			this.inputActionsPerSecond = standaloneInputModule.inputActionsPerSecond;
			this.repeatDelay = standaloneInputModule.repeatDelay;
			this.horizontalAxis = standaloneInputModule.horizontalAxis;
			this.verticalAxis = standaloneInputModule.verticalAxis;
			this.submitButton = standaloneInputModule.submitButton;
			this.cancelButton = standaloneInputModule.cancelButton;
		}

		// Token: 0x06000938 RID: 2360 RVA: 0x0003946A File Offset: 0x0003786A
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x04000A3C RID: 2620
		public bool forceModuleActive;

		// Token: 0x04000A3D RID: 2621
		public float inputActionsPerSecond;

		// Token: 0x04000A3E RID: 2622
		public float repeatDelay;

		// Token: 0x04000A3F RID: 2623
		public string horizontalAxis;

		// Token: 0x04000A40 RID: 2624
		public string verticalAxis;

		// Token: 0x04000A41 RID: 2625
		public string submitButton;

		// Token: 0x04000A42 RID: 2626
		public string cancelButton;
	}
}
