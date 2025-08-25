using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser
{
	// Token: 0x020005AB RID: 1451
	public class TestDownloadServer : MonoBehaviour
	{
		// Token: 0x06002465 RID: 9317 RVA: 0x000D24C6 File Offset: 0x000D08C6
		public TestDownloadServer()
		{
		}

		// Token: 0x06002466 RID: 9318 RVA: 0x000D24E4 File Offset: 0x000D08E4
		public void OnEnable()
		{
			this.server = new HttpListener();
			this.server.Prefixes.Add("http://localhost:" + this.port + "/");
			this.server.Start();
			this.serverEnabled = true;
			new Thread(new ThreadStart(this.ListenThread)).Start();
		}

		// Token: 0x06002467 RID: 9319 RVA: 0x000D2550 File Offset: 0x000D0950
		private void ListenThread()
		{
			while (this.serverEnabled)
			{
				HttpListenerContext context = this.server.GetContext();
				new Thread(new ParameterizedThreadStart(this.ResponseThread)).Start(context);
			}
		}

		// Token: 0x06002468 RID: 9320 RVA: 0x000D2594 File Offset: 0x000D0994
		private void ResponseThread(object obj)
		{
			TestDownloadServer.<ResponseThread>c__AnonStorey0 <ResponseThread>c__AnonStorey = new TestDownloadServer.<ResponseThread>c__AnonStorey0();
			HttpListenerContext httpListenerContext = (HttpListenerContext)obj;
			<ResponseThread>c__AnonStorey.res = httpListenerContext.Response;
			<ResponseThread>c__AnonStorey.res.StatusCode = 200;
			<ResponseThread>c__AnonStorey.output = new StreamWriter(<ResponseThread>c__AnonStorey.res.OutputStream);
			Action action = new Action(<ResponseThread>c__AnonStorey.<>m__0);
			string absolutePath = httpListenerContext.Request.Url.AbsolutePath;
			switch (absolutePath)
			{
			case "/basicFile":
				action();
				goto IL_3B4;
			case "/bigFile":
			{
				string text = "Lorem ipsum dolor sit amet.\n";
				long num2 = 104857600L;
				<ResponseThread>c__AnonStorey.res.AddHeader("Content-length", ((long)text.Length * num2).ToString());
				<ResponseThread>c__AnonStorey.res.AddHeader("Content-type", "application/octet-stream");
				byte[] bytes = Encoding.ASCII.GetBytes(text);
				byte[] array = new byte[1024 * bytes.Length];
				for (int i = 0; i < 1024; i++)
				{
					Array.Copy(bytes, 0, array, i * bytes.Length, bytes.Length);
				}
				int num3 = 0;
				while ((long)num3 < num2 / 1024L)
				{
					<ResponseThread>c__AnonStorey.res.OutputStream.Write(array, 0, array.Length);
					num3++;
				}
				goto IL_3B4;
			}
			case "/slowFile":
			case "/slowPage":
			{
				string text2 = "Lorem ipsum dolor sit amet.\n";
				int num4 = 1048576;
				HttpListenerResponse res = <ResponseThread>c__AnonStorey.res;
				string name = "Content-length";
				int num = text2.Length * num4;
				res.AddHeader(name, num.ToString());
				<ResponseThread>c__AnonStorey.res.AddHeader("Content-type", (!(absolutePath == "/slowFile")) ? "text/plain" : "application/octet-stream");
				for (int j = 0; j < num4; j++)
				{
					<ResponseThread>c__AnonStorey.output.Write(text2);
					Thread.Sleep(1);
				}
				goto IL_3B4;
			}
			case "/textFile":
				<ResponseThread>c__AnonStorey.res.AddHeader("Content-type", "text/plain");
				for (int k = 0; k < 100; k++)
				{
					<ResponseThread>c__AnonStorey.output.Write("This is some text!\n");
				}
				goto IL_3B4;
			case "/textFileDownload":
				<ResponseThread>c__AnonStorey.res.AddHeader("Content-type", "text/plain");
				<ResponseThread>c__AnonStorey.res.AddHeader("Content-Disposition", "attachment; filename=\"A Great Document Full of Text.txt\"");
				for (int l = 0; l < 100; l++)
				{
					<ResponseThread>c__AnonStorey.output.Write("This is some text!\n");
				}
				goto IL_3B4;
			case "/ǝpoɔıun«ñämé»":
			case "/%C7%9Dpo%C9%94%C4%B1un%C2%AB%C3%B1%C3%A4m%C3%A9%C2%BB":
				action();
				goto IL_3B4;
			case "/redirectedFile":
				<ResponseThread>c__AnonStorey.res.StatusCode = 302;
				<ResponseThread>c__AnonStorey.res.AddHeader("Location", "/some/other/file/i/was/redirected/to/redirectedResult");
				goto IL_3B4;
			case "/some/other/file/i/was/redirected/to/redirectedResult":
				action();
				goto IL_3B4;
			}
			httpListenerContext.Response.StatusCode = 404;
			<ResponseThread>c__AnonStorey.output.Write("Not found");
			IL_3B4:
			<ResponseThread>c__AnonStorey.output.Close();
		}

		// Token: 0x06002469 RID: 9321 RVA: 0x000D2960 File Offset: 0x000D0D60
		public void OnDisable()
		{
			this.serverEnabled = false;
			this.server.Stop();
		}

		// Token: 0x04001E9C RID: 7836
		private HttpListener server;

		// Token: 0x04001E9D RID: 7837
		public int port = 8083;

		// Token: 0x04001E9E RID: 7838
		private volatile bool serverEnabled = true;

		// Token: 0x04001E9F RID: 7839
		[CompilerGenerated]
		private static Dictionary<string, int> <>f__switch$map0;

		// Token: 0x02000F89 RID: 3977
		[CompilerGenerated]
		private sealed class <ResponseThread>c__AnonStorey0
		{
			// Token: 0x06007445 RID: 29765 RVA: 0x000D2976 File Offset: 0x000D0D76
			public <ResponseThread>c__AnonStorey0()
			{
			}

			// Token: 0x06007446 RID: 29766 RVA: 0x000D2980 File Offset: 0x000D0D80
			internal void <>m__0()
			{
				string text = "Lorem ipsum dolor sit amet.\n";
				int num = 1024;
				this.res.AddHeader("Content-length", (text.Length * num).ToString());
				this.res.AddHeader("Content-type", "application/octet-stream");
				for (int i = 0; i < num; i++)
				{
					this.output.Write(text);
					Thread.Sleep(1);
				}
			}

			// Token: 0x04006855 RID: 26709
			internal HttpListenerResponse res;

			// Token: 0x04006856 RID: 26710
			internal StreamWriter output;
		}
	}
}
