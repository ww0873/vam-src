using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200095F RID: 2399
public class OVRGearVrControllerTest : MonoBehaviour
{
	// Token: 0x06003B98 RID: 15256 RVA: 0x0011FC14 File Offset: 0x0011E014
	public OVRGearVrControllerTest()
	{
	}

	// Token: 0x06003B99 RID: 15257 RVA: 0x0011FC1C File Offset: 0x0011E01C
	private void Start()
	{
		if (this.uiText != null)
		{
			this.uiText.supportRichText = false;
		}
		this.data = new StringBuilder(2048);
		List<OVRGearVrControllerTest.BoolMonitor> list = new List<OVRGearVrControllerTest.BoolMonitor>();
		List<OVRGearVrControllerTest.BoolMonitor> list2 = list;
		string name = "WasRecentered";
		if (OVRGearVrControllerTest.<>f__am$cache0 == null)
		{
			OVRGearVrControllerTest.<>f__am$cache0 = new OVRGearVrControllerTest.BoolMonitor.BoolGenerator(OVRGearVrControllerTest.<Start>m__0);
		}
		list2.Add(new OVRGearVrControllerTest.BoolMonitor(name, OVRGearVrControllerTest.<>f__am$cache0, 0.5f));
		List<OVRGearVrControllerTest.BoolMonitor> list3 = list;
		string name2 = "One";
		if (OVRGearVrControllerTest.<>f__am$cache1 == null)
		{
			OVRGearVrControllerTest.<>f__am$cache1 = new OVRGearVrControllerTest.BoolMonitor.BoolGenerator(OVRGearVrControllerTest.<Start>m__1);
		}
		list3.Add(new OVRGearVrControllerTest.BoolMonitor(name2, OVRGearVrControllerTest.<>f__am$cache1, 0.5f));
		List<OVRGearVrControllerTest.BoolMonitor> list4 = list;
		string name3 = "OneDown";
		if (OVRGearVrControllerTest.<>f__am$cache2 == null)
		{
			OVRGearVrControllerTest.<>f__am$cache2 = new OVRGearVrControllerTest.BoolMonitor.BoolGenerator(OVRGearVrControllerTest.<Start>m__2);
		}
		list4.Add(new OVRGearVrControllerTest.BoolMonitor(name3, OVRGearVrControllerTest.<>f__am$cache2, 0.5f));
		List<OVRGearVrControllerTest.BoolMonitor> list5 = list;
		string name4 = "OneUp";
		if (OVRGearVrControllerTest.<>f__am$cache3 == null)
		{
			OVRGearVrControllerTest.<>f__am$cache3 = new OVRGearVrControllerTest.BoolMonitor.BoolGenerator(OVRGearVrControllerTest.<Start>m__3);
		}
		list5.Add(new OVRGearVrControllerTest.BoolMonitor(name4, OVRGearVrControllerTest.<>f__am$cache3, 0.5f));
		List<OVRGearVrControllerTest.BoolMonitor> list6 = list;
		string name5 = "One (Touch)";
		if (OVRGearVrControllerTest.<>f__am$cache4 == null)
		{
			OVRGearVrControllerTest.<>f__am$cache4 = new OVRGearVrControllerTest.BoolMonitor.BoolGenerator(OVRGearVrControllerTest.<Start>m__4);
		}
		list6.Add(new OVRGearVrControllerTest.BoolMonitor(name5, OVRGearVrControllerTest.<>f__am$cache4, 0.5f));
		List<OVRGearVrControllerTest.BoolMonitor> list7 = list;
		string name6 = "OneDown (Touch)";
		if (OVRGearVrControllerTest.<>f__am$cache5 == null)
		{
			OVRGearVrControllerTest.<>f__am$cache5 = new OVRGearVrControllerTest.BoolMonitor.BoolGenerator(OVRGearVrControllerTest.<Start>m__5);
		}
		list7.Add(new OVRGearVrControllerTest.BoolMonitor(name6, OVRGearVrControllerTest.<>f__am$cache5, 0.5f));
		List<OVRGearVrControllerTest.BoolMonitor> list8 = list;
		string name7 = "OneUp (Touch)";
		if (OVRGearVrControllerTest.<>f__am$cache6 == null)
		{
			OVRGearVrControllerTest.<>f__am$cache6 = new OVRGearVrControllerTest.BoolMonitor.BoolGenerator(OVRGearVrControllerTest.<Start>m__6);
		}
		list8.Add(new OVRGearVrControllerTest.BoolMonitor(name7, OVRGearVrControllerTest.<>f__am$cache6, 0.5f));
		List<OVRGearVrControllerTest.BoolMonitor> list9 = list;
		string name8 = "Two";
		if (OVRGearVrControllerTest.<>f__am$cache7 == null)
		{
			OVRGearVrControllerTest.<>f__am$cache7 = new OVRGearVrControllerTest.BoolMonitor.BoolGenerator(OVRGearVrControllerTest.<Start>m__7);
		}
		list9.Add(new OVRGearVrControllerTest.BoolMonitor(name8, OVRGearVrControllerTest.<>f__am$cache7, 0.5f));
		List<OVRGearVrControllerTest.BoolMonitor> list10 = list;
		string name9 = "TwoDown";
		if (OVRGearVrControllerTest.<>f__am$cache8 == null)
		{
			OVRGearVrControllerTest.<>f__am$cache8 = new OVRGearVrControllerTest.BoolMonitor.BoolGenerator(OVRGearVrControllerTest.<Start>m__8);
		}
		list10.Add(new OVRGearVrControllerTest.BoolMonitor(name9, OVRGearVrControllerTest.<>f__am$cache8, 0.5f));
		List<OVRGearVrControllerTest.BoolMonitor> list11 = list;
		string name10 = "TwoUp";
		if (OVRGearVrControllerTest.<>f__am$cache9 == null)
		{
			OVRGearVrControllerTest.<>f__am$cache9 = new OVRGearVrControllerTest.BoolMonitor.BoolGenerator(OVRGearVrControllerTest.<Start>m__9);
		}
		list11.Add(new OVRGearVrControllerTest.BoolMonitor(name10, OVRGearVrControllerTest.<>f__am$cache9, 0.5f));
		List<OVRGearVrControllerTest.BoolMonitor> list12 = list;
		string name11 = "PrimaryIndexTrigger";
		if (OVRGearVrControllerTest.<>f__am$cacheA == null)
		{
			OVRGearVrControllerTest.<>f__am$cacheA = new OVRGearVrControllerTest.BoolMonitor.BoolGenerator(OVRGearVrControllerTest.<Start>m__A);
		}
		list12.Add(new OVRGearVrControllerTest.BoolMonitor(name11, OVRGearVrControllerTest.<>f__am$cacheA, 0.5f));
		List<OVRGearVrControllerTest.BoolMonitor> list13 = list;
		string name12 = "PrimaryIndexTriggerDown";
		if (OVRGearVrControllerTest.<>f__am$cacheB == null)
		{
			OVRGearVrControllerTest.<>f__am$cacheB = new OVRGearVrControllerTest.BoolMonitor.BoolGenerator(OVRGearVrControllerTest.<Start>m__B);
		}
		list13.Add(new OVRGearVrControllerTest.BoolMonitor(name12, OVRGearVrControllerTest.<>f__am$cacheB, 0.5f));
		List<OVRGearVrControllerTest.BoolMonitor> list14 = list;
		string name13 = "PrimaryIndexTriggerUp";
		if (OVRGearVrControllerTest.<>f__am$cacheC == null)
		{
			OVRGearVrControllerTest.<>f__am$cacheC = new OVRGearVrControllerTest.BoolMonitor.BoolGenerator(OVRGearVrControllerTest.<Start>m__C);
		}
		list14.Add(new OVRGearVrControllerTest.BoolMonitor(name13, OVRGearVrControllerTest.<>f__am$cacheC, 0.5f));
		List<OVRGearVrControllerTest.BoolMonitor> list15 = list;
		string name14 = "PrimaryIndexTrigger (Touch)";
		if (OVRGearVrControllerTest.<>f__am$cacheD == null)
		{
			OVRGearVrControllerTest.<>f__am$cacheD = new OVRGearVrControllerTest.BoolMonitor.BoolGenerator(OVRGearVrControllerTest.<Start>m__D);
		}
		list15.Add(new OVRGearVrControllerTest.BoolMonitor(name14, OVRGearVrControllerTest.<>f__am$cacheD, 0.5f));
		List<OVRGearVrControllerTest.BoolMonitor> list16 = list;
		string name15 = "PrimaryIndexTriggerDown (Touch)";
		if (OVRGearVrControllerTest.<>f__am$cacheE == null)
		{
			OVRGearVrControllerTest.<>f__am$cacheE = new OVRGearVrControllerTest.BoolMonitor.BoolGenerator(OVRGearVrControllerTest.<Start>m__E);
		}
		list16.Add(new OVRGearVrControllerTest.BoolMonitor(name15, OVRGearVrControllerTest.<>f__am$cacheE, 0.5f));
		List<OVRGearVrControllerTest.BoolMonitor> list17 = list;
		string name16 = "PrimaryIndexTriggerUp (Touch)";
		if (OVRGearVrControllerTest.<>f__am$cacheF == null)
		{
			OVRGearVrControllerTest.<>f__am$cacheF = new OVRGearVrControllerTest.BoolMonitor.BoolGenerator(OVRGearVrControllerTest.<Start>m__F);
		}
		list17.Add(new OVRGearVrControllerTest.BoolMonitor(name16, OVRGearVrControllerTest.<>f__am$cacheF, 0.5f));
		List<OVRGearVrControllerTest.BoolMonitor> list18 = list;
		string name17 = "PrimaryHandTrigger";
		if (OVRGearVrControllerTest.<>f__am$cache10 == null)
		{
			OVRGearVrControllerTest.<>f__am$cache10 = new OVRGearVrControllerTest.BoolMonitor.BoolGenerator(OVRGearVrControllerTest.<Start>m__10);
		}
		list18.Add(new OVRGearVrControllerTest.BoolMonitor(name17, OVRGearVrControllerTest.<>f__am$cache10, 0.5f));
		List<OVRGearVrControllerTest.BoolMonitor> list19 = list;
		string name18 = "PrimaryHandTriggerDown";
		if (OVRGearVrControllerTest.<>f__am$cache11 == null)
		{
			OVRGearVrControllerTest.<>f__am$cache11 = new OVRGearVrControllerTest.BoolMonitor.BoolGenerator(OVRGearVrControllerTest.<Start>m__11);
		}
		list19.Add(new OVRGearVrControllerTest.BoolMonitor(name18, OVRGearVrControllerTest.<>f__am$cache11, 0.5f));
		List<OVRGearVrControllerTest.BoolMonitor> list20 = list;
		string name19 = "PrimaryHandTriggerUp";
		if (OVRGearVrControllerTest.<>f__am$cache12 == null)
		{
			OVRGearVrControllerTest.<>f__am$cache12 = new OVRGearVrControllerTest.BoolMonitor.BoolGenerator(OVRGearVrControllerTest.<Start>m__12);
		}
		list20.Add(new OVRGearVrControllerTest.BoolMonitor(name19, OVRGearVrControllerTest.<>f__am$cache12, 0.5f));
		List<OVRGearVrControllerTest.BoolMonitor> list21 = list;
		string name20 = "Up";
		if (OVRGearVrControllerTest.<>f__am$cache13 == null)
		{
			OVRGearVrControllerTest.<>f__am$cache13 = new OVRGearVrControllerTest.BoolMonitor.BoolGenerator(OVRGearVrControllerTest.<Start>m__13);
		}
		list21.Add(new OVRGearVrControllerTest.BoolMonitor(name20, OVRGearVrControllerTest.<>f__am$cache13, 0.5f));
		List<OVRGearVrControllerTest.BoolMonitor> list22 = list;
		string name21 = "Down";
		if (OVRGearVrControllerTest.<>f__am$cache14 == null)
		{
			OVRGearVrControllerTest.<>f__am$cache14 = new OVRGearVrControllerTest.BoolMonitor.BoolGenerator(OVRGearVrControllerTest.<Start>m__14);
		}
		list22.Add(new OVRGearVrControllerTest.BoolMonitor(name21, OVRGearVrControllerTest.<>f__am$cache14, 0.5f));
		List<OVRGearVrControllerTest.BoolMonitor> list23 = list;
		string name22 = "Left";
		if (OVRGearVrControllerTest.<>f__am$cache15 == null)
		{
			OVRGearVrControllerTest.<>f__am$cache15 = new OVRGearVrControllerTest.BoolMonitor.BoolGenerator(OVRGearVrControllerTest.<Start>m__15);
		}
		list23.Add(new OVRGearVrControllerTest.BoolMonitor(name22, OVRGearVrControllerTest.<>f__am$cache15, 0.5f));
		List<OVRGearVrControllerTest.BoolMonitor> list24 = list;
		string name23 = "Right";
		if (OVRGearVrControllerTest.<>f__am$cache16 == null)
		{
			OVRGearVrControllerTest.<>f__am$cache16 = new OVRGearVrControllerTest.BoolMonitor.BoolGenerator(OVRGearVrControllerTest.<Start>m__16);
		}
		list24.Add(new OVRGearVrControllerTest.BoolMonitor(name23, OVRGearVrControllerTest.<>f__am$cache16, 0.5f));
		List<OVRGearVrControllerTest.BoolMonitor> list25 = list;
		string name24 = "Touchpad (Click)";
		if (OVRGearVrControllerTest.<>f__am$cache17 == null)
		{
			OVRGearVrControllerTest.<>f__am$cache17 = new OVRGearVrControllerTest.BoolMonitor.BoolGenerator(OVRGearVrControllerTest.<Start>m__17);
		}
		list25.Add(new OVRGearVrControllerTest.BoolMonitor(name24, OVRGearVrControllerTest.<>f__am$cache17, 0.5f));
		List<OVRGearVrControllerTest.BoolMonitor> list26 = list;
		string name25 = "TouchpadDown (Click)";
		if (OVRGearVrControllerTest.<>f__am$cache18 == null)
		{
			OVRGearVrControllerTest.<>f__am$cache18 = new OVRGearVrControllerTest.BoolMonitor.BoolGenerator(OVRGearVrControllerTest.<Start>m__18);
		}
		list26.Add(new OVRGearVrControllerTest.BoolMonitor(name25, OVRGearVrControllerTest.<>f__am$cache18, 0.5f));
		List<OVRGearVrControllerTest.BoolMonitor> list27 = list;
		string name26 = "TouchpadUp (Click)";
		if (OVRGearVrControllerTest.<>f__am$cache19 == null)
		{
			OVRGearVrControllerTest.<>f__am$cache19 = new OVRGearVrControllerTest.BoolMonitor.BoolGenerator(OVRGearVrControllerTest.<Start>m__19);
		}
		list27.Add(new OVRGearVrControllerTest.BoolMonitor(name26, OVRGearVrControllerTest.<>f__am$cache19, 0.5f));
		List<OVRGearVrControllerTest.BoolMonitor> list28 = list;
		string name27 = "Touchpad (Touch)";
		if (OVRGearVrControllerTest.<>f__am$cache1A == null)
		{
			OVRGearVrControllerTest.<>f__am$cache1A = new OVRGearVrControllerTest.BoolMonitor.BoolGenerator(OVRGearVrControllerTest.<Start>m__1A);
		}
		list28.Add(new OVRGearVrControllerTest.BoolMonitor(name27, OVRGearVrControllerTest.<>f__am$cache1A, 0.5f));
		List<OVRGearVrControllerTest.BoolMonitor> list29 = list;
		string name28 = "TouchpadDown (Touch)";
		if (OVRGearVrControllerTest.<>f__am$cache1B == null)
		{
			OVRGearVrControllerTest.<>f__am$cache1B = new OVRGearVrControllerTest.BoolMonitor.BoolGenerator(OVRGearVrControllerTest.<Start>m__1B);
		}
		list29.Add(new OVRGearVrControllerTest.BoolMonitor(name28, OVRGearVrControllerTest.<>f__am$cache1B, 0.5f));
		List<OVRGearVrControllerTest.BoolMonitor> list30 = list;
		string name29 = "TouchpadUp (Touch)";
		if (OVRGearVrControllerTest.<>f__am$cache1C == null)
		{
			OVRGearVrControllerTest.<>f__am$cache1C = new OVRGearVrControllerTest.BoolMonitor.BoolGenerator(OVRGearVrControllerTest.<Start>m__1C);
		}
		list30.Add(new OVRGearVrControllerTest.BoolMonitor(name29, OVRGearVrControllerTest.<>f__am$cache1C, 0.5f));
		List<OVRGearVrControllerTest.BoolMonitor> list31 = list;
		string name30 = "Start";
		if (OVRGearVrControllerTest.<>f__am$cache1D == null)
		{
			OVRGearVrControllerTest.<>f__am$cache1D = new OVRGearVrControllerTest.BoolMonitor.BoolGenerator(OVRGearVrControllerTest.<Start>m__1D);
		}
		list31.Add(new OVRGearVrControllerTest.BoolMonitor(name30, OVRGearVrControllerTest.<>f__am$cache1D, 0.5f));
		List<OVRGearVrControllerTest.BoolMonitor> list32 = list;
		string name31 = "StartDown";
		if (OVRGearVrControllerTest.<>f__am$cache1E == null)
		{
			OVRGearVrControllerTest.<>f__am$cache1E = new OVRGearVrControllerTest.BoolMonitor.BoolGenerator(OVRGearVrControllerTest.<Start>m__1E);
		}
		list32.Add(new OVRGearVrControllerTest.BoolMonitor(name31, OVRGearVrControllerTest.<>f__am$cache1E, 0.5f));
		List<OVRGearVrControllerTest.BoolMonitor> list33 = list;
		string name32 = "StartUp";
		if (OVRGearVrControllerTest.<>f__am$cache1F == null)
		{
			OVRGearVrControllerTest.<>f__am$cache1F = new OVRGearVrControllerTest.BoolMonitor.BoolGenerator(OVRGearVrControllerTest.<Start>m__1F);
		}
		list33.Add(new OVRGearVrControllerTest.BoolMonitor(name32, OVRGearVrControllerTest.<>f__am$cache1F, 0.5f));
		List<OVRGearVrControllerTest.BoolMonitor> list34 = list;
		string name33 = "Back";
		if (OVRGearVrControllerTest.<>f__am$cache20 == null)
		{
			OVRGearVrControllerTest.<>f__am$cache20 = new OVRGearVrControllerTest.BoolMonitor.BoolGenerator(OVRGearVrControllerTest.<Start>m__20);
		}
		list34.Add(new OVRGearVrControllerTest.BoolMonitor(name33, OVRGearVrControllerTest.<>f__am$cache20, 0.5f));
		List<OVRGearVrControllerTest.BoolMonitor> list35 = list;
		string name34 = "BackDown";
		if (OVRGearVrControllerTest.<>f__am$cache21 == null)
		{
			OVRGearVrControllerTest.<>f__am$cache21 = new OVRGearVrControllerTest.BoolMonitor.BoolGenerator(OVRGearVrControllerTest.<Start>m__21);
		}
		list35.Add(new OVRGearVrControllerTest.BoolMonitor(name34, OVRGearVrControllerTest.<>f__am$cache21, 0.5f));
		List<OVRGearVrControllerTest.BoolMonitor> list36 = list;
		string name35 = "BackUp";
		if (OVRGearVrControllerTest.<>f__am$cache22 == null)
		{
			OVRGearVrControllerTest.<>f__am$cache22 = new OVRGearVrControllerTest.BoolMonitor.BoolGenerator(OVRGearVrControllerTest.<Start>m__22);
		}
		list36.Add(new OVRGearVrControllerTest.BoolMonitor(name35, OVRGearVrControllerTest.<>f__am$cache22, 0.5f));
		List<OVRGearVrControllerTest.BoolMonitor> list37 = list;
		string name36 = "A";
		if (OVRGearVrControllerTest.<>f__am$cache23 == null)
		{
			OVRGearVrControllerTest.<>f__am$cache23 = new OVRGearVrControllerTest.BoolMonitor.BoolGenerator(OVRGearVrControllerTest.<Start>m__23);
		}
		list37.Add(new OVRGearVrControllerTest.BoolMonitor(name36, OVRGearVrControllerTest.<>f__am$cache23, 0.5f));
		List<OVRGearVrControllerTest.BoolMonitor> list38 = list;
		string name37 = "ADown";
		if (OVRGearVrControllerTest.<>f__am$cache24 == null)
		{
			OVRGearVrControllerTest.<>f__am$cache24 = new OVRGearVrControllerTest.BoolMonitor.BoolGenerator(OVRGearVrControllerTest.<Start>m__24);
		}
		list38.Add(new OVRGearVrControllerTest.BoolMonitor(name37, OVRGearVrControllerTest.<>f__am$cache24, 0.5f));
		List<OVRGearVrControllerTest.BoolMonitor> list39 = list;
		string name38 = "AUp";
		if (OVRGearVrControllerTest.<>f__am$cache25 == null)
		{
			OVRGearVrControllerTest.<>f__am$cache25 = new OVRGearVrControllerTest.BoolMonitor.BoolGenerator(OVRGearVrControllerTest.<Start>m__25);
		}
		list39.Add(new OVRGearVrControllerTest.BoolMonitor(name38, OVRGearVrControllerTest.<>f__am$cache25, 0.5f));
		this.monitors = list;
	}

	// Token: 0x06003B9A RID: 15258 RVA: 0x001203D0 File Offset: 0x0011E7D0
	private void Update()
	{
		OVRInput.Controller activeController = OVRInput.GetActiveController();
		this.data.Length = 0;
		byte controllerRecenterCount = OVRInput.GetControllerRecenterCount(OVRInput.Controller.Active);
		this.data.AppendFormat("RecenterCount: {0}\n", controllerRecenterCount);
		byte controllerBatteryPercentRemaining = OVRInput.GetControllerBatteryPercentRemaining(OVRInput.Controller.Active);
		this.data.AppendFormat("Battery: {0}\n", controllerBatteryPercentRemaining);
		float appFramerate = OVRPlugin.GetAppFramerate();
		this.data.AppendFormat("Framerate: {0:F2}\n", appFramerate);
		string arg = activeController.ToString();
		this.data.AppendFormat("Active: {0}\n", arg);
		string arg2 = OVRInput.GetConnectedControllers().ToString();
		this.data.AppendFormat("Connected: {0}\n", arg2);
		this.data.AppendFormat("PrevConnected: {0}\n", OVRGearVrControllerTest.prevConnected);
		OVRGearVrControllerTest.controllers.Update();
		OVRGearVrControllerTest.controllers.AppendToStringBuilder(ref this.data);
		OVRGearVrControllerTest.prevConnected = arg2;
		Quaternion localControllerRotation = OVRInput.GetLocalControllerRotation(activeController);
		this.data.AppendFormat("Orientation: ({0:F2}, {1:F2}, {2:F2}, {3:F2})\n", new object[]
		{
			localControllerRotation.x,
			localControllerRotation.y,
			localControllerRotation.z,
			localControllerRotation.w
		});
		Vector3 localControllerAngularVelocity = OVRInput.GetLocalControllerAngularVelocity(activeController);
		this.data.AppendFormat("AngVel: ({0:F2}, {1:F2}, {2:F2})\n", localControllerAngularVelocity.x, localControllerAngularVelocity.y, localControllerAngularVelocity.z);
		Vector3 localControllerAngularAcceleration = OVRInput.GetLocalControllerAngularAcceleration(activeController);
		this.data.AppendFormat("AngAcc: ({0:F2}, {1:F2}, {2:F2})\n", localControllerAngularAcceleration.x, localControllerAngularAcceleration.y, localControllerAngularAcceleration.z);
		Vector3 localControllerPosition = OVRInput.GetLocalControllerPosition(activeController);
		this.data.AppendFormat("Position: ({0:F2}, {1:F2}, {2:F2})\n", localControllerPosition.x, localControllerPosition.y, localControllerPosition.z);
		Vector3 localControllerVelocity = OVRInput.GetLocalControllerVelocity(activeController);
		this.data.AppendFormat("Vel: ({0:F2}, {1:F2}, {2:F2})\n", localControllerVelocity.x, localControllerVelocity.y, localControllerVelocity.z);
		Vector3 localControllerAcceleration = OVRInput.GetLocalControllerAcceleration(activeController);
		this.data.AppendFormat("Acc: ({0:F2}, {1:F2}, {2:F2})\n", localControllerAcceleration.x, localControllerAcceleration.y, localControllerAcceleration.z);
		Vector2 vector = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad, OVRInput.Controller.Active);
		this.data.AppendFormat("PrimaryTouchpad: ({0:F2}, {1:F2})\n", vector.x, vector.y);
		Vector2 vector2 = OVRInput.Get(OVRInput.Axis2D.SecondaryTouchpad, OVRInput.Controller.Active);
		this.data.AppendFormat("SecondaryTouchpad: ({0:F2}, {1:F2})\n", vector2.x, vector2.y);
		float num = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.Active);
		this.data.AppendFormat("PrimaryIndexTriggerAxis1D: ({0:F2})\n", num);
		float num2 = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.Active);
		this.data.AppendFormat("PrimaryHandTriggerAxis1D: ({0:F2})\n", num2);
		for (int i = 0; i < this.monitors.Count; i++)
		{
			this.monitors[i].Update();
			this.monitors[i].AppendToStringBuilder(ref this.data);
		}
		if (this.uiText != null)
		{
			this.uiText.text = this.data.ToString();
		}
	}

	// Token: 0x06003B9B RID: 15259 RVA: 0x00120785 File Offset: 0x0011EB85
	// Note: this type is marked as 'beforefieldinit'.
	static OVRGearVrControllerTest()
	{
	}

	// Token: 0x06003B9C RID: 15260 RVA: 0x001207B1 File Offset: 0x0011EBB1
	[CompilerGenerated]
	private static bool <Start>m__0()
	{
		return OVRInput.GetControllerWasRecentered(OVRInput.Controller.Active);
	}

	// Token: 0x06003B9D RID: 15261 RVA: 0x001207BD File Offset: 0x0011EBBD
	[CompilerGenerated]
	private static bool <Start>m__1()
	{
		return OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.Active);
	}

	// Token: 0x06003B9E RID: 15262 RVA: 0x001207CA File Offset: 0x0011EBCA
	[CompilerGenerated]
	private static bool <Start>m__2()
	{
		return OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.Active);
	}

	// Token: 0x06003B9F RID: 15263 RVA: 0x001207D7 File Offset: 0x0011EBD7
	[CompilerGenerated]
	private static bool <Start>m__3()
	{
		return OVRInput.GetUp(OVRInput.Button.One, OVRInput.Controller.Active);
	}

	// Token: 0x06003BA0 RID: 15264 RVA: 0x001207E4 File Offset: 0x0011EBE4
	[CompilerGenerated]
	private static bool <Start>m__4()
	{
		return OVRInput.Get(OVRInput.Touch.One, OVRInput.Controller.Active);
	}

	// Token: 0x06003BA1 RID: 15265 RVA: 0x001207F1 File Offset: 0x0011EBF1
	[CompilerGenerated]
	private static bool <Start>m__5()
	{
		return OVRInput.GetDown(OVRInput.Touch.One, OVRInput.Controller.Active);
	}

	// Token: 0x06003BA2 RID: 15266 RVA: 0x001207FE File Offset: 0x0011EBFE
	[CompilerGenerated]
	private static bool <Start>m__6()
	{
		return OVRInput.GetUp(OVRInput.Touch.One, OVRInput.Controller.Active);
	}

	// Token: 0x06003BA3 RID: 15267 RVA: 0x0012080B File Offset: 0x0011EC0B
	[CompilerGenerated]
	private static bool <Start>m__7()
	{
		return OVRInput.Get(OVRInput.Button.Two, OVRInput.Controller.Active);
	}

	// Token: 0x06003BA4 RID: 15268 RVA: 0x00120818 File Offset: 0x0011EC18
	[CompilerGenerated]
	private static bool <Start>m__8()
	{
		return OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.Active);
	}

	// Token: 0x06003BA5 RID: 15269 RVA: 0x00120825 File Offset: 0x0011EC25
	[CompilerGenerated]
	private static bool <Start>m__9()
	{
		return OVRInput.GetUp(OVRInput.Button.Two, OVRInput.Controller.Active);
	}

	// Token: 0x06003BA6 RID: 15270 RVA: 0x00120832 File Offset: 0x0011EC32
	[CompilerGenerated]
	private static bool <Start>m__A()
	{
		return OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.Active);
	}

	// Token: 0x06003BA7 RID: 15271 RVA: 0x00120843 File Offset: 0x0011EC43
	[CompilerGenerated]
	private static bool <Start>m__B()
	{
		return OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.Active);
	}

	// Token: 0x06003BA8 RID: 15272 RVA: 0x00120854 File Offset: 0x0011EC54
	[CompilerGenerated]
	private static bool <Start>m__C()
	{
		return OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.Active);
	}

	// Token: 0x06003BA9 RID: 15273 RVA: 0x00120865 File Offset: 0x0011EC65
	[CompilerGenerated]
	private static bool <Start>m__D()
	{
		return OVRInput.Get(OVRInput.Touch.PrimaryIndexTrigger, OVRInput.Controller.Active);
	}

	// Token: 0x06003BAA RID: 15274 RVA: 0x00120876 File Offset: 0x0011EC76
	[CompilerGenerated]
	private static bool <Start>m__E()
	{
		return OVRInput.GetDown(OVRInput.Touch.PrimaryIndexTrigger, OVRInput.Controller.Active);
	}

	// Token: 0x06003BAB RID: 15275 RVA: 0x00120887 File Offset: 0x0011EC87
	[CompilerGenerated]
	private static bool <Start>m__F()
	{
		return OVRInput.GetUp(OVRInput.Touch.PrimaryIndexTrigger, OVRInput.Controller.Active);
	}

	// Token: 0x06003BAC RID: 15276 RVA: 0x00120898 File Offset: 0x0011EC98
	[CompilerGenerated]
	private static bool <Start>m__10()
	{
		return OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.Active);
	}

	// Token: 0x06003BAD RID: 15277 RVA: 0x001208A9 File Offset: 0x0011ECA9
	[CompilerGenerated]
	private static bool <Start>m__11()
	{
		return OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.Active);
	}

	// Token: 0x06003BAE RID: 15278 RVA: 0x001208BA File Offset: 0x0011ECBA
	[CompilerGenerated]
	private static bool <Start>m__12()
	{
		return OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.Active);
	}

	// Token: 0x06003BAF RID: 15279 RVA: 0x001208CB File Offset: 0x0011ECCB
	[CompilerGenerated]
	private static bool <Start>m__13()
	{
		return OVRInput.Get(OVRInput.Button.Up, OVRInput.Controller.Active);
	}

	// Token: 0x06003BB0 RID: 15280 RVA: 0x001208DC File Offset: 0x0011ECDC
	[CompilerGenerated]
	private static bool <Start>m__14()
	{
		return OVRInput.Get(OVRInput.Button.Down, OVRInput.Controller.Active);
	}

	// Token: 0x06003BB1 RID: 15281 RVA: 0x001208ED File Offset: 0x0011ECED
	[CompilerGenerated]
	private static bool <Start>m__15()
	{
		return OVRInput.Get(OVRInput.Button.Left, OVRInput.Controller.Active);
	}

	// Token: 0x06003BB2 RID: 15282 RVA: 0x001208FE File Offset: 0x0011ECFE
	[CompilerGenerated]
	private static bool <Start>m__16()
	{
		return OVRInput.Get(OVRInput.Button.Right, OVRInput.Controller.Active);
	}

	// Token: 0x06003BB3 RID: 15283 RVA: 0x0012090F File Offset: 0x0011ED0F
	[CompilerGenerated]
	private static bool <Start>m__17()
	{
		return OVRInput.Get(OVRInput.Button.PrimaryTouchpad, OVRInput.Controller.Active);
	}

	// Token: 0x06003BB4 RID: 15284 RVA: 0x00120920 File Offset: 0x0011ED20
	[CompilerGenerated]
	private static bool <Start>m__18()
	{
		return OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad, OVRInput.Controller.Active);
	}

	// Token: 0x06003BB5 RID: 15285 RVA: 0x00120931 File Offset: 0x0011ED31
	[CompilerGenerated]
	private static bool <Start>m__19()
	{
		return OVRInput.GetUp(OVRInput.Button.PrimaryTouchpad, OVRInput.Controller.Active);
	}

	// Token: 0x06003BB6 RID: 15286 RVA: 0x00120942 File Offset: 0x0011ED42
	[CompilerGenerated]
	private static bool <Start>m__1A()
	{
		return OVRInput.Get(OVRInput.Touch.PrimaryTouchpad, OVRInput.Controller.Active);
	}

	// Token: 0x06003BB7 RID: 15287 RVA: 0x00120953 File Offset: 0x0011ED53
	[CompilerGenerated]
	private static bool <Start>m__1B()
	{
		return OVRInput.GetDown(OVRInput.Touch.PrimaryTouchpad, OVRInput.Controller.Active);
	}

	// Token: 0x06003BB8 RID: 15288 RVA: 0x00120964 File Offset: 0x0011ED64
	[CompilerGenerated]
	private static bool <Start>m__1C()
	{
		return OVRInput.GetUp(OVRInput.Touch.PrimaryTouchpad, OVRInput.Controller.Active);
	}

	// Token: 0x06003BB9 RID: 15289 RVA: 0x00120975 File Offset: 0x0011ED75
	[CompilerGenerated]
	private static bool <Start>m__1D()
	{
		return OVRInput.Get(OVRInput.RawButton.Start, OVRInput.Controller.Active);
	}

	// Token: 0x06003BBA RID: 15290 RVA: 0x00120986 File Offset: 0x0011ED86
	[CompilerGenerated]
	private static bool <Start>m__1E()
	{
		return OVRInput.GetDown(OVRInput.RawButton.Start, OVRInput.Controller.Active);
	}

	// Token: 0x06003BBB RID: 15291 RVA: 0x00120997 File Offset: 0x0011ED97
	[CompilerGenerated]
	private static bool <Start>m__1F()
	{
		return OVRInput.GetUp(OVRInput.RawButton.Start, OVRInput.Controller.Active);
	}

	// Token: 0x06003BBC RID: 15292 RVA: 0x001209A8 File Offset: 0x0011EDA8
	[CompilerGenerated]
	private static bool <Start>m__20()
	{
		return OVRInput.Get(OVRInput.RawButton.Back, OVRInput.Controller.Active);
	}

	// Token: 0x06003BBD RID: 15293 RVA: 0x001209B9 File Offset: 0x0011EDB9
	[CompilerGenerated]
	private static bool <Start>m__21()
	{
		return OVRInput.GetDown(OVRInput.RawButton.Back, OVRInput.Controller.Active);
	}

	// Token: 0x06003BBE RID: 15294 RVA: 0x001209CA File Offset: 0x0011EDCA
	[CompilerGenerated]
	private static bool <Start>m__22()
	{
		return OVRInput.GetUp(OVRInput.RawButton.Back, OVRInput.Controller.Active);
	}

	// Token: 0x06003BBF RID: 15295 RVA: 0x001209DB File Offset: 0x0011EDDB
	[CompilerGenerated]
	private static bool <Start>m__23()
	{
		return OVRInput.Get(OVRInput.RawButton.A, OVRInput.Controller.Active);
	}

	// Token: 0x06003BC0 RID: 15296 RVA: 0x001209E8 File Offset: 0x0011EDE8
	[CompilerGenerated]
	private static bool <Start>m__24()
	{
		return OVRInput.GetDown(OVRInput.RawButton.A, OVRInput.Controller.Active);
	}

	// Token: 0x06003BC1 RID: 15297 RVA: 0x001209F5 File Offset: 0x0011EDF5
	[CompilerGenerated]
	private static bool <Start>m__25()
	{
		return OVRInput.GetUp(OVRInput.RawButton.A, OVRInput.Controller.Active);
	}

	// Token: 0x06003BC2 RID: 15298 RVA: 0x00120A04 File Offset: 0x0011EE04
	[CompilerGenerated]
	private static bool <controllers>m__26()
	{
		return OVRInput.GetConnectedControllers().ToString() != OVRGearVrControllerTest.prevConnected;
	}

	// Token: 0x04002D86 RID: 11654
	public Text uiText;

	// Token: 0x04002D87 RID: 11655
	private List<OVRGearVrControllerTest.BoolMonitor> monitors;

	// Token: 0x04002D88 RID: 11656
	private StringBuilder data;

	// Token: 0x04002D89 RID: 11657
	private static string prevConnected = string.Empty;

	// Token: 0x04002D8A RID: 11658
	private static OVRGearVrControllerTest.BoolMonitor controllers = new OVRGearVrControllerTest.BoolMonitor("Controllers Changed", new OVRGearVrControllerTest.BoolMonitor.BoolGenerator(OVRGearVrControllerTest.<controllers>m__26), 0.5f);

	// Token: 0x04002D8B RID: 11659
	[CompilerGenerated]
	private static OVRGearVrControllerTest.BoolMonitor.BoolGenerator <>f__am$cache0;

	// Token: 0x04002D8C RID: 11660
	[CompilerGenerated]
	private static OVRGearVrControllerTest.BoolMonitor.BoolGenerator <>f__am$cache1;

	// Token: 0x04002D8D RID: 11661
	[CompilerGenerated]
	private static OVRGearVrControllerTest.BoolMonitor.BoolGenerator <>f__am$cache2;

	// Token: 0x04002D8E RID: 11662
	[CompilerGenerated]
	private static OVRGearVrControllerTest.BoolMonitor.BoolGenerator <>f__am$cache3;

	// Token: 0x04002D8F RID: 11663
	[CompilerGenerated]
	private static OVRGearVrControllerTest.BoolMonitor.BoolGenerator <>f__am$cache4;

	// Token: 0x04002D90 RID: 11664
	[CompilerGenerated]
	private static OVRGearVrControllerTest.BoolMonitor.BoolGenerator <>f__am$cache5;

	// Token: 0x04002D91 RID: 11665
	[CompilerGenerated]
	private static OVRGearVrControllerTest.BoolMonitor.BoolGenerator <>f__am$cache6;

	// Token: 0x04002D92 RID: 11666
	[CompilerGenerated]
	private static OVRGearVrControllerTest.BoolMonitor.BoolGenerator <>f__am$cache7;

	// Token: 0x04002D93 RID: 11667
	[CompilerGenerated]
	private static OVRGearVrControllerTest.BoolMonitor.BoolGenerator <>f__am$cache8;

	// Token: 0x04002D94 RID: 11668
	[CompilerGenerated]
	private static OVRGearVrControllerTest.BoolMonitor.BoolGenerator <>f__am$cache9;

	// Token: 0x04002D95 RID: 11669
	[CompilerGenerated]
	private static OVRGearVrControllerTest.BoolMonitor.BoolGenerator <>f__am$cacheA;

	// Token: 0x04002D96 RID: 11670
	[CompilerGenerated]
	private static OVRGearVrControllerTest.BoolMonitor.BoolGenerator <>f__am$cacheB;

	// Token: 0x04002D97 RID: 11671
	[CompilerGenerated]
	private static OVRGearVrControllerTest.BoolMonitor.BoolGenerator <>f__am$cacheC;

	// Token: 0x04002D98 RID: 11672
	[CompilerGenerated]
	private static OVRGearVrControllerTest.BoolMonitor.BoolGenerator <>f__am$cacheD;

	// Token: 0x04002D99 RID: 11673
	[CompilerGenerated]
	private static OVRGearVrControllerTest.BoolMonitor.BoolGenerator <>f__am$cacheE;

	// Token: 0x04002D9A RID: 11674
	[CompilerGenerated]
	private static OVRGearVrControllerTest.BoolMonitor.BoolGenerator <>f__am$cacheF;

	// Token: 0x04002D9B RID: 11675
	[CompilerGenerated]
	private static OVRGearVrControllerTest.BoolMonitor.BoolGenerator <>f__am$cache10;

	// Token: 0x04002D9C RID: 11676
	[CompilerGenerated]
	private static OVRGearVrControllerTest.BoolMonitor.BoolGenerator <>f__am$cache11;

	// Token: 0x04002D9D RID: 11677
	[CompilerGenerated]
	private static OVRGearVrControllerTest.BoolMonitor.BoolGenerator <>f__am$cache12;

	// Token: 0x04002D9E RID: 11678
	[CompilerGenerated]
	private static OVRGearVrControllerTest.BoolMonitor.BoolGenerator <>f__am$cache13;

	// Token: 0x04002D9F RID: 11679
	[CompilerGenerated]
	private static OVRGearVrControllerTest.BoolMonitor.BoolGenerator <>f__am$cache14;

	// Token: 0x04002DA0 RID: 11680
	[CompilerGenerated]
	private static OVRGearVrControllerTest.BoolMonitor.BoolGenerator <>f__am$cache15;

	// Token: 0x04002DA1 RID: 11681
	[CompilerGenerated]
	private static OVRGearVrControllerTest.BoolMonitor.BoolGenerator <>f__am$cache16;

	// Token: 0x04002DA2 RID: 11682
	[CompilerGenerated]
	private static OVRGearVrControllerTest.BoolMonitor.BoolGenerator <>f__am$cache17;

	// Token: 0x04002DA3 RID: 11683
	[CompilerGenerated]
	private static OVRGearVrControllerTest.BoolMonitor.BoolGenerator <>f__am$cache18;

	// Token: 0x04002DA4 RID: 11684
	[CompilerGenerated]
	private static OVRGearVrControllerTest.BoolMonitor.BoolGenerator <>f__am$cache19;

	// Token: 0x04002DA5 RID: 11685
	[CompilerGenerated]
	private static OVRGearVrControllerTest.BoolMonitor.BoolGenerator <>f__am$cache1A;

	// Token: 0x04002DA6 RID: 11686
	[CompilerGenerated]
	private static OVRGearVrControllerTest.BoolMonitor.BoolGenerator <>f__am$cache1B;

	// Token: 0x04002DA7 RID: 11687
	[CompilerGenerated]
	private static OVRGearVrControllerTest.BoolMonitor.BoolGenerator <>f__am$cache1C;

	// Token: 0x04002DA8 RID: 11688
	[CompilerGenerated]
	private static OVRGearVrControllerTest.BoolMonitor.BoolGenerator <>f__am$cache1D;

	// Token: 0x04002DA9 RID: 11689
	[CompilerGenerated]
	private static OVRGearVrControllerTest.BoolMonitor.BoolGenerator <>f__am$cache1E;

	// Token: 0x04002DAA RID: 11690
	[CompilerGenerated]
	private static OVRGearVrControllerTest.BoolMonitor.BoolGenerator <>f__am$cache1F;

	// Token: 0x04002DAB RID: 11691
	[CompilerGenerated]
	private static OVRGearVrControllerTest.BoolMonitor.BoolGenerator <>f__am$cache20;

	// Token: 0x04002DAC RID: 11692
	[CompilerGenerated]
	private static OVRGearVrControllerTest.BoolMonitor.BoolGenerator <>f__am$cache21;

	// Token: 0x04002DAD RID: 11693
	[CompilerGenerated]
	private static OVRGearVrControllerTest.BoolMonitor.BoolGenerator <>f__am$cache22;

	// Token: 0x04002DAE RID: 11694
	[CompilerGenerated]
	private static OVRGearVrControllerTest.BoolMonitor.BoolGenerator <>f__am$cache23;

	// Token: 0x04002DAF RID: 11695
	[CompilerGenerated]
	private static OVRGearVrControllerTest.BoolMonitor.BoolGenerator <>f__am$cache24;

	// Token: 0x04002DB0 RID: 11696
	[CompilerGenerated]
	private static OVRGearVrControllerTest.BoolMonitor.BoolGenerator <>f__am$cache25;

	// Token: 0x02000960 RID: 2400
	public class BoolMonitor
	{
		// Token: 0x06003BC3 RID: 15299 RVA: 0x00120A2E File Offset: 0x0011EE2E
		public BoolMonitor(string name, OVRGearVrControllerTest.BoolMonitor.BoolGenerator generator, float displayTimeout = 0.5f)
		{
			this.m_name = name;
			this.m_generator = generator;
			this.m_displayTimeout = displayTimeout;
		}

		// Token: 0x06003BC4 RID: 15300 RVA: 0x00120A58 File Offset: 0x0011EE58
		public void Update()
		{
			this.m_prevValue = this.m_currentValue;
			this.m_currentValue = this.m_generator();
			if (this.m_currentValue != this.m_prevValue)
			{
				this.m_currentValueRecentlyChanged = true;
				this.m_displayTimer = this.m_displayTimeout;
			}
			if (this.m_displayTimer > 0f)
			{
				this.m_displayTimer -= Time.deltaTime;
				if (this.m_displayTimer <= 0f)
				{
					this.m_currentValueRecentlyChanged = false;
					this.m_displayTimer = 0f;
				}
			}
		}

		// Token: 0x06003BC5 RID: 15301 RVA: 0x00120AEC File Offset: 0x0011EEEC
		public void AppendToStringBuilder(ref StringBuilder sb)
		{
			sb.Append(this.m_name);
			if (this.m_currentValue && this.m_currentValueRecentlyChanged)
			{
				sb.Append(": *True*\n");
			}
			else if (this.m_currentValue)
			{
				sb.Append(":  True \n");
			}
			else if (!this.m_currentValue && this.m_currentValueRecentlyChanged)
			{
				sb.Append(": *False*\n");
			}
			else if (!this.m_currentValue)
			{
				sb.Append(":  False \n");
			}
		}

		// Token: 0x04002DB1 RID: 11697
		private string m_name = string.Empty;

		// Token: 0x04002DB2 RID: 11698
		private OVRGearVrControllerTest.BoolMonitor.BoolGenerator m_generator;

		// Token: 0x04002DB3 RID: 11699
		private bool m_prevValue;

		// Token: 0x04002DB4 RID: 11700
		private bool m_currentValue;

		// Token: 0x04002DB5 RID: 11701
		private bool m_currentValueRecentlyChanged;

		// Token: 0x04002DB6 RID: 11702
		private float m_displayTimeout;

		// Token: 0x04002DB7 RID: 11703
		private float m_displayTimer;

		// Token: 0x02000961 RID: 2401
		// (Invoke) Token: 0x06003BC7 RID: 15303
		public delegate bool BoolGenerator();
	}
}
