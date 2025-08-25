using System;
using System.Collections;

namespace Obi
{
	// Token: 0x020003BC RID: 956
	public class EditorCoroutine
	{
		// Token: 0x06001868 RID: 6248 RVA: 0x0008A5F3 File Offset: 0x000889F3
		private EditorCoroutine(IEnumerator _routine)
		{
			this.routine = _routine;
		}

		// Token: 0x06001869 RID: 6249 RVA: 0x0008A604 File Offset: 0x00088A04
		public static EditorCoroutine StartCoroutine(IEnumerator _routine)
		{
			EditorCoroutine editorCoroutine = new EditorCoroutine(_routine);
			editorCoroutine.Start();
			return editorCoroutine;
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x0600186A RID: 6250 RVA: 0x0008A61F File Offset: 0x00088A1F
		public object Result
		{
			get
			{
				return this.result;
			}
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x0600186B RID: 6251 RVA: 0x0008A627 File Offset: 0x00088A27
		public bool IsDone
		{
			get
			{
				return this.isDone;
			}
		}

		// Token: 0x0600186C RID: 6252 RVA: 0x0008A62F File Offset: 0x00088A2F
		private void Start()
		{
			this.isDone = false;
			this.result = null;
			this.Update();
		}

		// Token: 0x0600186D RID: 6253 RVA: 0x0008A645 File Offset: 0x00088A45
		public void Stop()
		{
			this.isDone = true;
		}

		// Token: 0x0600186E RID: 6254 RVA: 0x0008A650 File Offset: 0x00088A50
		private void Update()
		{
			bool flag = this.routine.MoveNext();
			this.result = this.routine.Current;
			if (!flag)
			{
				this.Stop();
			}
		}

		// Token: 0x0600186F RID: 6255 RVA: 0x0008A686 File Offset: 0x00088A86
		public static void ShowCoroutineProgressBar(string title, EditorCoroutine coroutine)
		{
		}

		// Token: 0x040013D1 RID: 5073
		private readonly IEnumerator routine;

		// Token: 0x040013D2 RID: 5074
		private object result;

		// Token: 0x040013D3 RID: 5075
		private bool isDone;
	}
}
