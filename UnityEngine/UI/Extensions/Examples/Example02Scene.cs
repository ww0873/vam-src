using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x0200048D RID: 1165
	public class Example02Scene : MonoBehaviour
	{
		// Token: 0x06001D9B RID: 7579 RVA: 0x000AA94D File Offset: 0x000A8D4D
		public Example02Scene()
		{
		}

		// Token: 0x06001D9C RID: 7580 RVA: 0x000AA958 File Offset: 0x000A8D58
		private void Start()
		{
			IEnumerable<int> source = Enumerable.Range(0, 20);
			if (Example02Scene.<>f__am$cache0 == null)
			{
				Example02Scene.<>f__am$cache0 = new Func<int, Example02CellDto>(Example02Scene.<Start>m__0);
			}
			List<Example02CellDto> data = source.Select(Example02Scene.<>f__am$cache0).ToList<Example02CellDto>();
			this.scrollView.UpdateData(data);
		}

		// Token: 0x06001D9D RID: 7581 RVA: 0x000AA9A4 File Offset: 0x000A8DA4
		[CompilerGenerated]
		private static Example02CellDto <Start>m__0(int i)
		{
			return new Example02CellDto
			{
				Message = "Cell " + i
			};
		}

		// Token: 0x0400190E RID: 6414
		[SerializeField]
		private Example02ScrollView scrollView;

		// Token: 0x0400190F RID: 6415
		[CompilerGenerated]
		private static Func<int, Example02CellDto> <>f__am$cache0;
	}
}
