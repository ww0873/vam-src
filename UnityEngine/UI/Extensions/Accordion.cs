using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020004A3 RID: 1187
	[RequireComponent(typeof(VerticalLayoutGroup), typeof(ContentSizeFitter), typeof(ToggleGroup))]
	[AddComponentMenu("UI/Extensions/Accordion/Accordion Group")]
	public class Accordion : MonoBehaviour
	{
		// Token: 0x06001DF9 RID: 7673 RVA: 0x000ABDBD File Offset: 0x000AA1BD
		public Accordion()
		{
		}

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06001DFA RID: 7674 RVA: 0x000ABDD0 File Offset: 0x000AA1D0
		// (set) Token: 0x06001DFB RID: 7675 RVA: 0x000ABDD8 File Offset: 0x000AA1D8
		public Accordion.Transition transition
		{
			get
			{
				return this.m_Transition;
			}
			set
			{
				this.m_Transition = value;
			}
		}

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06001DFC RID: 7676 RVA: 0x000ABDE1 File Offset: 0x000AA1E1
		// (set) Token: 0x06001DFD RID: 7677 RVA: 0x000ABDE9 File Offset: 0x000AA1E9
		public float transitionDuration
		{
			get
			{
				return this.m_TransitionDuration;
			}
			set
			{
				this.m_TransitionDuration = value;
			}
		}

		// Token: 0x04001966 RID: 6502
		[SerializeField]
		private Accordion.Transition m_Transition;

		// Token: 0x04001967 RID: 6503
		[SerializeField]
		private float m_TransitionDuration = 0.3f;

		// Token: 0x020004A4 RID: 1188
		public enum Transition
		{
			// Token: 0x04001969 RID: 6505
			Instant,
			// Token: 0x0400196A RID: 6506
			Tween
		}
	}
}
