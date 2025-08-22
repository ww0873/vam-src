using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace PrefabEvolution
{
	// Token: 0x02000400 RID: 1024
	[Serializable]
	public class PEExposedProperties : ISerializationCallbackReceiver
	{
		// Token: 0x060019FD RID: 6653 RVA: 0x00092105 File Offset: 0x00090505
		public PEExposedProperties()
		{
		}

		// Token: 0x060019FE RID: 6654 RVA: 0x0009212E File Offset: 0x0009052E
		public void OnBeforeSerialize()
		{
		}

		// Token: 0x060019FF RID: 6655 RVA: 0x00092130 File Offset: 0x00090530
		public void OnAfterDeserialize()
		{
		}

		// Token: 0x06001A00 RID: 6656 RVA: 0x00092134 File Offset: 0x00090534
		public IEnumerable<BaseExposedData> GetInheritedProperties()
		{
			if (this.InheritedProperties == null)
			{
				this.InheritedProperties = new List<BaseExposedData>();
				if (this.PrefabScript == null)
				{
					return this.InheritedProperties;
				}
				if (this.PrefabScript.ParentPrefab != null)
				{
					PEPrefabScript component = this.PrefabScript.ParentPrefab.GetComponent<PEPrefabScript>();
					if (component == null)
					{
						Debug.Log("Inherited property Error: Prefab script not found on", this.PrefabScript);
						return this.InheritedProperties;
					}
					List<BaseExposedData> inheritedProperties = this.InheritedProperties;
					IEnumerable<BaseExposedData> items = component.Properties.Items;
					if (PEExposedProperties.<>f__am$cache0 == null)
					{
						PEExposedProperties.<>f__am$cache0 = new Func<BaseExposedData, bool>(PEExposedProperties.<GetInheritedProperties>m__0);
					}
					inheritedProperties.AddRange(items.Where(PEExposedProperties.<>f__am$cache0).Select(new Func<BaseExposedData, BaseExposedData>(this.<GetInheritedProperties>m__1)));
					List<ExposedProperty> properties = this.Properties;
					if (PEExposedProperties.<>f__am$cache1 == null)
					{
						PEExposedProperties.<>f__am$cache1 = new Predicate<ExposedProperty>(PEExposedProperties.<GetInheritedProperties>m__2);
					}
					properties.RemoveAll(PEExposedProperties.<>f__am$cache1);
					List<ExposedPropertyGroup> groups = this.Groups;
					if (PEExposedProperties.<>f__am$cache2 == null)
					{
						PEExposedProperties.<>f__am$cache2 = new Predicate<ExposedPropertyGroup>(PEExposedProperties.<GetInheritedProperties>m__3);
					}
					groups.RemoveAll(PEExposedProperties.<>f__am$cache2);
					this.Hidden.RemoveAll(new Predicate<int>(this.<GetInheritedProperties>m__4));
					foreach (ExposedProperty exposedProperty in this.InheritedProperties.OfType<ExposedProperty>())
					{
						PELinkage.Link link = this.PrefabScript.Links[component.Links[exposedProperty.Target]];
						exposedProperty.Target = ((link != null) ? link.InstanceTarget : null);
						if (exposedProperty.Target == null)
						{
							Debug.Log("Inherited property Error: Local target is not found Path:" + exposedProperty.PropertyPath, this.PrefabScript);
						}
					}
				}
			}
			return this.InheritedProperties;
		}

		// Token: 0x06001A01 RID: 6657 RVA: 0x00092324 File Offset: 0x00090724
		public bool HasProperty(UnityEngine.Object target, string propertyPath)
		{
			PEExposedProperties.<HasProperty>c__AnonStorey1 <HasProperty>c__AnonStorey = new PEExposedProperties.<HasProperty>c__AnonStorey1();
			<HasProperty>c__AnonStorey.target = target;
			<HasProperty>c__AnonStorey.propertyPath = propertyPath;
			return this.Properties.Any(new Func<ExposedProperty, bool>(<HasProperty>c__AnonStorey.<>m__0));
		}

		// Token: 0x06001A02 RID: 6658 RVA: 0x0009235C File Offset: 0x0009075C
		public void Remove(UnityEngine.Object target, string propertyPath)
		{
			PEExposedProperties.<Remove>c__AnonStorey2 <Remove>c__AnonStorey = new PEExposedProperties.<Remove>c__AnonStorey2();
			<Remove>c__AnonStorey.target = target;
			<Remove>c__AnonStorey.propertyPath = propertyPath;
			this.Properties.RemoveAll(new Predicate<ExposedProperty>(<Remove>c__AnonStorey.<>m__0));
		}

		// Token: 0x06001A03 RID: 6659 RVA: 0x00092398 File Offset: 0x00090798
		public void Add(BaseExposedData exposed)
		{
			exposed.Container = this;
			ExposedProperty exposedProperty = exposed as ExposedProperty;
			if (exposedProperty != null)
			{
				this.Add(exposedProperty);
			}
			else
			{
				this.Add(exposed as ExposedPropertyGroup);
			}
		}

		// Token: 0x06001A04 RID: 6660 RVA: 0x000923D1 File Offset: 0x000907D1
		public void Add(ExposedProperty exposed)
		{
			exposed.Container = this;
			if (!this.Properties.Contains(exposed))
			{
				this.Properties.Add(exposed);
			}
		}

		// Token: 0x06001A05 RID: 6661 RVA: 0x000923F7 File Offset: 0x000907F7
		public void Add(ExposedPropertyGroup exposed)
		{
			exposed.Container = this;
			if (!this.Groups.Contains(exposed))
			{
				this.Groups.Add(exposed);
			}
		}

		// Token: 0x06001A06 RID: 6662 RVA: 0x00092420 File Offset: 0x00090820
		public void Remove(int id)
		{
			PEExposedProperties.<Remove>c__AnonStorey3 <Remove>c__AnonStorey = new PEExposedProperties.<Remove>c__AnonStorey3();
			<Remove>c__AnonStorey.id = id;
			this.Properties.RemoveAll(new Predicate<ExposedProperty>(<Remove>c__AnonStorey.<>m__0));
			this.Groups.RemoveAll(new Predicate<ExposedPropertyGroup>(<Remove>c__AnonStorey.<>m__1));
		}

		// Token: 0x06001A07 RID: 6663 RVA: 0x0009246C File Offset: 0x0009086C
		public ExposedProperty FindProperty(string label)
		{
			PEExposedProperties.<FindProperty>c__AnonStorey4 <FindProperty>c__AnonStorey = new PEExposedProperties.<FindProperty>c__AnonStorey4();
			<FindProperty>c__AnonStorey.label = label;
			return this.Items.OfType<ExposedProperty>().FirstOrDefault(new Func<ExposedProperty, bool>(<FindProperty>c__AnonStorey.<>m__0));
		}

		// Token: 0x06001A08 RID: 6664 RVA: 0x000924A4 File Offset: 0x000908A4
		public ExposedProperty FindProperty(int id)
		{
			PEExposedProperties.<FindProperty>c__AnonStorey5 <FindProperty>c__AnonStorey = new PEExposedProperties.<FindProperty>c__AnonStorey5();
			<FindProperty>c__AnonStorey.id = id;
			return this.Items.OfType<ExposedProperty>().FirstOrDefault(new Func<ExposedProperty, bool>(<FindProperty>c__AnonStorey.<>m__0));
		}

		// Token: 0x06001A09 RID: 6665 RVA: 0x000924DC File Offset: 0x000908DC
		public ExposedProperty FindProperty(uint id)
		{
			PEExposedProperties.<FindProperty>c__AnonStorey6 <FindProperty>c__AnonStorey = new PEExposedProperties.<FindProperty>c__AnonStorey6();
			<FindProperty>c__AnonStorey.id = id;
			return this.Items.OfType<ExposedProperty>().FirstOrDefault(new Func<ExposedProperty, bool>(<FindProperty>c__AnonStorey.<>m__0));
		}

		// Token: 0x170002DC RID: 732
		public BaseExposedData this[int id]
		{
			get
			{
				PEExposedProperties.<>c__AnonStorey7 <>c__AnonStorey = new PEExposedProperties.<>c__AnonStorey7();
				<>c__AnonStorey.id = id;
				return this.Items.FirstOrDefault(new Func<BaseExposedData, bool>(<>c__AnonStorey.<>m__0));
			}
		}

		// Token: 0x170002DD RID: 733
		public BaseExposedData this[string label]
		{
			get
			{
				PEExposedProperties.<>c__AnonStorey8 <>c__AnonStorey = new PEExposedProperties.<>c__AnonStorey8();
				<>c__AnonStorey.label = label;
				return this.OrderedItems.FirstOrDefault(new Func<BaseExposedData, bool>(<>c__AnonStorey.<>m__0));
			}
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06001A0C RID: 6668 RVA: 0x00092579 File Offset: 0x00090979
		public IEnumerable<BaseExposedData> Items
		{
			get
			{
				return this.GetInheritedProperties().Concat(this.Properties.OfType<BaseExposedData>().Concat(this.Groups.OfType<BaseExposedData>()));
			}
		}

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06001A0D RID: 6669 RVA: 0x000925A4 File Offset: 0x000909A4
		public IEnumerable<BaseExposedData> OrderedItems
		{
			get
			{
				BaseExposedData.Comparer comparer = default(BaseExposedData.Comparer);
				List<BaseExposedData> list = this.Items.ToList<BaseExposedData>();
				list.Sort(comparer);
				return list;
			}
		}

		// Token: 0x06001A0E RID: 6670 RVA: 0x000925D4 File Offset: 0x000909D4
		public bool GetInherited(int id)
		{
			PEExposedProperties.<GetInherited>c__AnonStorey9 <GetInherited>c__AnonStorey = new PEExposedProperties.<GetInherited>c__AnonStorey9();
			<GetInherited>c__AnonStorey.id = id;
			return this.GetInheritedProperties().Any(new Func<BaseExposedData, bool>(<GetInherited>c__AnonStorey.<>m__0));
		}

		// Token: 0x06001A0F RID: 6671 RVA: 0x00092608 File Offset: 0x00090A08
		public bool GetHidden(int id)
		{
			PEExposedProperties.<GetHidden>c__AnonStoreyA <GetHidden>c__AnonStoreyA = new PEExposedProperties.<GetHidden>c__AnonStoreyA();
			<GetHidden>c__AnonStoreyA.id = id;
			return this.Hidden.Any(new Func<int, bool>(<GetHidden>c__AnonStoreyA.<>m__0));
		}

		// Token: 0x06001A10 RID: 6672 RVA: 0x0009263C File Offset: 0x00090A3C
		public void SetHide(BaseExposedData property, bool state)
		{
			if (state == this.Hidden.Contains(property.Id))
			{
				return;
			}
			if (state)
			{
				this.Hidden.Add(property.Id);
			}
			else
			{
				this.Hidden.Remove(property.Id);
			}
			this.Hidden.Sort();
		}

		// Token: 0x06001A11 RID: 6673 RVA: 0x0009269A File Offset: 0x00090A9A
		[CompilerGenerated]
		private static bool <GetInheritedProperties>m__0(BaseExposedData i)
		{
			return !i.Hidden;
		}

		// Token: 0x06001A12 RID: 6674 RVA: 0x000926A8 File Offset: 0x00090AA8
		[CompilerGenerated]
		private BaseExposedData <GetInheritedProperties>m__1(BaseExposedData p)
		{
			BaseExposedData baseExposedData = p.Clone();
			baseExposedData.Container = this;
			return baseExposedData;
		}

		// Token: 0x06001A13 RID: 6675 RVA: 0x000926C4 File Offset: 0x00090AC4
		[CompilerGenerated]
		private static bool <GetInheritedProperties>m__2(ExposedProperty p)
		{
			return p.Inherited;
		}

		// Token: 0x06001A14 RID: 6676 RVA: 0x000926CC File Offset: 0x00090ACC
		[CompilerGenerated]
		private static bool <GetInheritedProperties>m__3(ExposedPropertyGroup p)
		{
			return p.Inherited;
		}

		// Token: 0x06001A15 RID: 6677 RVA: 0x000926D4 File Offset: 0x00090AD4
		[CompilerGenerated]
		private bool <GetInheritedProperties>m__4(int p)
		{
			PEExposedProperties.<GetInheritedProperties>c__AnonStorey0 <GetInheritedProperties>c__AnonStorey = new PEExposedProperties.<GetInheritedProperties>c__AnonStorey0();
			<GetInheritedProperties>c__AnonStorey.p = p;
			return this.Items.All(new Func<BaseExposedData, bool>(<GetInheritedProperties>c__AnonStorey.<>m__0));
		}

		// Token: 0x0400151C RID: 5404
		[NonSerialized]
		internal List<BaseExposedData> InheritedProperties;

		// Token: 0x0400151D RID: 5405
		[NonSerialized]
		public PEPrefabScript PrefabScript;

		// Token: 0x0400151E RID: 5406
		public List<ExposedProperty> Properties = Utils.Create<List<ExposedProperty>>();

		// Token: 0x0400151F RID: 5407
		public List<ExposedPropertyGroup> Groups = Utils.Create<List<ExposedPropertyGroup>>();

		// Token: 0x04001520 RID: 5408
		[SerializeField]
		private List<int> Hidden = Utils.Create<List<int>>();

		// Token: 0x04001521 RID: 5409
		[CompilerGenerated]
		private static Func<BaseExposedData, bool> <>f__am$cache0;

		// Token: 0x04001522 RID: 5410
		[CompilerGenerated]
		private static Predicate<ExposedProperty> <>f__am$cache1;

		// Token: 0x04001523 RID: 5411
		[CompilerGenerated]
		private static Predicate<ExposedPropertyGroup> <>f__am$cache2;

		// Token: 0x02000F4C RID: 3916
		[CompilerGenerated]
		private sealed class <HasProperty>c__AnonStorey1
		{
			// Token: 0x06007377 RID: 29559 RVA: 0x00092705 File Offset: 0x00090B05
			public <HasProperty>c__AnonStorey1()
			{
			}

			// Token: 0x06007378 RID: 29560 RVA: 0x0009270D File Offset: 0x00090B0D
			internal bool <>m__0(ExposedProperty p)
			{
				return p.Target == this.target && p.PropertyPath == this.propertyPath;
			}

			// Token: 0x04006769 RID: 26473
			internal UnityEngine.Object target;

			// Token: 0x0400676A RID: 26474
			internal string propertyPath;
		}

		// Token: 0x02000F4D RID: 3917
		[CompilerGenerated]
		private sealed class <Remove>c__AnonStorey2
		{
			// Token: 0x06007379 RID: 29561 RVA: 0x00092739 File Offset: 0x00090B39
			public <Remove>c__AnonStorey2()
			{
			}

			// Token: 0x0600737A RID: 29562 RVA: 0x00092741 File Offset: 0x00090B41
			internal bool <>m__0(ExposedProperty p)
			{
				return p.Target == this.target && p.PropertyPath == this.propertyPath;
			}

			// Token: 0x0400676B RID: 26475
			internal UnityEngine.Object target;

			// Token: 0x0400676C RID: 26476
			internal string propertyPath;
		}

		// Token: 0x02000F4E RID: 3918
		[CompilerGenerated]
		private sealed class <Remove>c__AnonStorey3
		{
			// Token: 0x0600737B RID: 29563 RVA: 0x0009276D File Offset: 0x00090B6D
			public <Remove>c__AnonStorey3()
			{
			}

			// Token: 0x0600737C RID: 29564 RVA: 0x00092775 File Offset: 0x00090B75
			internal bool <>m__0(ExposedProperty p)
			{
				return p.Id == this.id;
			}

			// Token: 0x0600737D RID: 29565 RVA: 0x00092785 File Offset: 0x00090B85
			internal bool <>m__1(ExposedPropertyGroup p)
			{
				return p.Id == this.id;
			}

			// Token: 0x0400676D RID: 26477
			internal int id;
		}

		// Token: 0x02000F4F RID: 3919
		[CompilerGenerated]
		private sealed class <FindProperty>c__AnonStorey4
		{
			// Token: 0x0600737E RID: 29566 RVA: 0x00092795 File Offset: 0x00090B95
			public <FindProperty>c__AnonStorey4()
			{
			}

			// Token: 0x0600737F RID: 29567 RVA: 0x0009279D File Offset: 0x00090B9D
			internal bool <>m__0(ExposedProperty p)
			{
				return p.Label == this.label;
			}

			// Token: 0x0400676E RID: 26478
			internal string label;
		}

		// Token: 0x02000F50 RID: 3920
		[CompilerGenerated]
		private sealed class <FindProperty>c__AnonStorey5
		{
			// Token: 0x06007380 RID: 29568 RVA: 0x000927B0 File Offset: 0x00090BB0
			public <FindProperty>c__AnonStorey5()
			{
			}

			// Token: 0x06007381 RID: 29569 RVA: 0x000927B8 File Offset: 0x00090BB8
			internal bool <>m__0(ExposedProperty p)
			{
				return p.Id == this.id;
			}

			// Token: 0x0400676F RID: 26479
			internal int id;
		}

		// Token: 0x02000F51 RID: 3921
		[CompilerGenerated]
		private sealed class <FindProperty>c__AnonStorey6
		{
			// Token: 0x06007382 RID: 29570 RVA: 0x000927C8 File Offset: 0x00090BC8
			public <FindProperty>c__AnonStorey6()
			{
			}

			// Token: 0x06007383 RID: 29571 RVA: 0x000927D0 File Offset: 0x00090BD0
			internal bool <>m__0(ExposedProperty p)
			{
				return p.Id == (int)this.id;
			}

			// Token: 0x04006770 RID: 26480
			internal uint id;
		}

		// Token: 0x02000F52 RID: 3922
		[CompilerGenerated]
		private sealed class <>c__AnonStorey7
		{
			// Token: 0x06007384 RID: 29572 RVA: 0x000927E0 File Offset: 0x00090BE0
			public <>c__AnonStorey7()
			{
			}

			// Token: 0x06007385 RID: 29573 RVA: 0x000927E8 File Offset: 0x00090BE8
			internal bool <>m__0(BaseExposedData p)
			{
				return p.Id == this.id;
			}

			// Token: 0x04006771 RID: 26481
			internal int id;
		}

		// Token: 0x02000F53 RID: 3923
		[CompilerGenerated]
		private sealed class <>c__AnonStorey8
		{
			// Token: 0x06007386 RID: 29574 RVA: 0x000927F8 File Offset: 0x00090BF8
			public <>c__AnonStorey8()
			{
			}

			// Token: 0x06007387 RID: 29575 RVA: 0x00092800 File Offset: 0x00090C00
			internal bool <>m__0(BaseExposedData p)
			{
				return p.Label == this.label;
			}

			// Token: 0x04006772 RID: 26482
			internal string label;
		}

		// Token: 0x02000F54 RID: 3924
		[CompilerGenerated]
		private sealed class <GetInherited>c__AnonStorey9
		{
			// Token: 0x06007388 RID: 29576 RVA: 0x00092813 File Offset: 0x00090C13
			public <GetInherited>c__AnonStorey9()
			{
			}

			// Token: 0x06007389 RID: 29577 RVA: 0x0009281B File Offset: 0x00090C1B
			internal bool <>m__0(BaseExposedData i)
			{
				return i.Id == this.id;
			}

			// Token: 0x04006773 RID: 26483
			internal int id;
		}

		// Token: 0x02000F55 RID: 3925
		[CompilerGenerated]
		private sealed class <GetHidden>c__AnonStoreyA
		{
			// Token: 0x0600738A RID: 29578 RVA: 0x0009282B File Offset: 0x00090C2B
			public <GetHidden>c__AnonStoreyA()
			{
			}

			// Token: 0x0600738B RID: 29579 RVA: 0x00092833 File Offset: 0x00090C33
			internal bool <>m__0(int i)
			{
				return i == this.id;
			}

			// Token: 0x04006774 RID: 26484
			internal int id;
		}

		// Token: 0x02000F56 RID: 3926
		[CompilerGenerated]
		private sealed class <GetInheritedProperties>c__AnonStorey0
		{
			// Token: 0x0600738C RID: 29580 RVA: 0x0009283E File Offset: 0x00090C3E
			public <GetInheritedProperties>c__AnonStorey0()
			{
			}

			// Token: 0x0600738D RID: 29581 RVA: 0x00092846 File Offset: 0x00090C46
			internal bool <>m__0(BaseExposedData item)
			{
				return item.Id != this.p;
			}

			// Token: 0x04006775 RID: 26485
			internal int p;
		}
	}
}
