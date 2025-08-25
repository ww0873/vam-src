using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

// Token: 0x02000D7F RID: 3455
public class MirrorCopyPhysicsComponents : MonoBehaviour
{
	// Token: 0x06006A64 RID: 27236 RVA: 0x00281642 File Offset: 0x0027FA42
	public MirrorCopyPhysicsComponents()
	{
	}

	// Token: 0x06006A65 RID: 27237 RVA: 0x00281660 File Offset: 0x0027FA60
	private Vector3 MirrorVector(Vector3 inVector)
	{
		Vector3 result = inVector;
		MirrorCopyPhysicsComponents.InvertAxis invertAxis = this.invertAxis;
		if (invertAxis != MirrorCopyPhysicsComponents.InvertAxis.X)
		{
			if (invertAxis != MirrorCopyPhysicsComponents.InvertAxis.Y)
			{
				if (invertAxis == MirrorCopyPhysicsComponents.InvertAxis.Z)
				{
					result.z = -result.z;
				}
			}
			else
			{
				result.y = -result.y;
			}
		}
		else
		{
			result.x = -result.x;
		}
		return result;
	}

	// Token: 0x06006A66 RID: 27238 RVA: 0x002816CC File Offset: 0x0027FACC
	private Quaternion MirrorQuaternion(Quaternion inQuat)
	{
		Quaternion result = inQuat;
		MirrorCopyPhysicsComponents.InvertAxis invertAxis = this.invertAxis;
		if (invertAxis != MirrorCopyPhysicsComponents.InvertAxis.X)
		{
			if (invertAxis != MirrorCopyPhysicsComponents.InvertAxis.Y)
			{
				if (invertAxis == MirrorCopyPhysicsComponents.InvertAxis.Z)
				{
					result.x = -result.x;
					result.y = -result.y;
				}
			}
			else
			{
				result.x = -result.x;
				result.z = -result.z;
			}
		}
		else
		{
			result.y = -result.y;
			result.z = -result.z;
		}
		return result;
	}

	// Token: 0x06006A67 RID: 27239 RVA: 0x00281768 File Offset: 0x0027FB68
	public void init()
	{
		this.transformMap = new Dictionary<string, Transform>();
		Transform[] componentsInChildren = base.transform.parent.GetComponentsInChildren<Transform>();
		foreach (Transform transform in componentsInChildren)
		{
			if (!this.transformMap.ContainsKey(transform.name))
			{
				this.transformMap.Add(transform.name, transform);
			}
		}
		this.RBMap = new Dictionary<string, Rigidbody>();
		Rigidbody[] componentsInChildren2 = base.transform.parent.GetComponentsInChildren<Rigidbody>();
		foreach (Rigidbody rigidbody in componentsInChildren2)
		{
			this.RBMap.Add(rigidbody.name, rigidbody);
		}
		this.jointMap = new Dictionary<string, ConfigurableJoint>();
		ConfigurableJoint[] componentsInChildren3 = base.GetComponentsInChildren<ConfigurableJoint>();
		foreach (ConfigurableJoint configurableJoint in componentsInChildren3)
		{
			this.jointMap.Add(configurableJoint.name, configurableJoint);
		}
		if (this.copyFromRoot != null)
		{
			this.copyFromTransformMap = new Dictionary<string, Transform>();
			Transform[] componentsInChildren4 = this.copyFromRoot.GetComponentsInChildren<Transform>();
			foreach (Transform transform2 in componentsInChildren4)
			{
				if (!this.copyFromTransformMap.ContainsKey(transform2.name))
				{
					this.copyFromTransformMap.Add(transform2.name, transform2);
				}
			}
			this.copyFromRBMap = new Dictionary<string, Rigidbody>();
			Rigidbody[] componentsInChildren5 = this.copyFromRoot.GetComponentsInChildren<Rigidbody>();
			foreach (Rigidbody rigidbody2 in componentsInChildren5)
			{
				this.copyFromRBMap.Add(rigidbody2.name, rigidbody2);
			}
			this.copyFromJointMap = new Dictionary<string, ConfigurableJoint>();
			ConfigurableJoint[] componentsInChildren6 = this.copyFromRoot.GetComponentsInChildren<ConfigurableJoint>();
			foreach (ConfigurableJoint configurableJoint2 in componentsInChildren6)
			{
				this.copyFromJointMap.Add(configurableJoint2.name, configurableJoint2);
			}
		}
	}

	// Token: 0x06006A68 RID: 27240 RVA: 0x0028198C File Offset: 0x0027FD8C
	public ICollection<string> getRBJointNames()
	{
		if (this.RBMap == null)
		{
			this.init();
		}
		return this.RBMap.Keys;
	}

	// Token: 0x06006A69 RID: 27241 RVA: 0x002819B8 File Offset: 0x0027FDB8
	public ICollection<string> getCopyFromRBJointNames()
	{
		if (this.copyFromRBMap == null)
		{
			this.init();
		}
		if (this.copyFromRBMap != null)
		{
			return this.copyFromRBMap.Keys;
		}
		return null;
	}

	// Token: 0x06006A6A RID: 27242 RVA: 0x002819F0 File Offset: 0x0027FDF0
	public ICollection<string> getJointNames()
	{
		if (this.jointMap == null)
		{
			this.init();
		}
		return this.jointMap.Keys;
	}

	// Token: 0x06006A6B RID: 27243 RVA: 0x00281A1C File Offset: 0x0027FE1C
	public ICollection<string> getCopyFromJointNames()
	{
		if (this.copyFromJointMap == null)
		{
			this.init();
		}
		if (this.copyFromJointMap != null)
		{
			return this.copyFromJointMap.Keys;
		}
		return null;
	}

	// Token: 0x06006A6C RID: 27244 RVA: 0x00281A54 File Offset: 0x0027FE54
	public ConfigurableJoint getJoint(string joint)
	{
		if (this.jointMap == null)
		{
			this.init();
		}
		ConfigurableJoint result;
		if (this.jointMap.TryGetValue(joint, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06006A6D RID: 27245 RVA: 0x00281A88 File Offset: 0x0027FE88
	public ConfigurableJoint getCopyFromJoint(string joint)
	{
		if (this.copyFromJointMap == null)
		{
			this.init();
		}
		ConfigurableJoint result;
		if (this.copyFromJointMap != null && this.copyFromJointMap.TryGetValue(joint, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06006A6E RID: 27246 RVA: 0x00281AC8 File Offset: 0x0027FEC8
	public Transform getTransform(string trans)
	{
		if (this.transformMap == null)
		{
			this.init();
		}
		Transform result;
		if (this.transformMap.TryGetValue(trans, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06006A6F RID: 27247 RVA: 0x00281AFC File Offset: 0x0027FEFC
	public Transform getCopyFromTransform(string trans)
	{
		if (this.copyFromTransformMap == null)
		{
			this.init();
		}
		Transform result;
		if (this.copyFromTransformMap != null && this.copyFromTransformMap.TryGetValue(trans, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06006A70 RID: 27248 RVA: 0x00281B3C File Offset: 0x0027FF3C
	public Rigidbody getRB(string joint)
	{
		Rigidbody result;
		if (this.RBMap.TryGetValue(joint, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06006A71 RID: 27249 RVA: 0x00281B60 File Offset: 0x0027FF60
	public Rigidbody getCopyFromRB(string joint)
	{
		if (this.copyFromRBMap == null)
		{
			this.init();
		}
		Rigidbody result;
		if (this.copyFromRBMap != null && this.copyFromRBMap.TryGetValue(joint, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06006A72 RID: 27250 RVA: 0x00281BA0 File Offset: 0x0027FFA0
	private void copyCapsuleCollider(GameObject refgo, GameObject go)
	{
		CapsuleCollider component = refgo.GetComponent<CapsuleCollider>();
		CapsuleCollider capsuleCollider = go.GetComponent<CapsuleCollider>();
		if (component != null)
		{
			if (capsuleCollider == null)
			{
				capsuleCollider = go.AddComponent<CapsuleCollider>();
			}
			capsuleCollider.isTrigger = component.isTrigger;
			capsuleCollider.sharedMaterial = component.sharedMaterial;
			Vector3 center = this.MirrorVector(component.center);
			capsuleCollider.center = center;
			capsuleCollider.radius = component.radius;
			capsuleCollider.height = component.height;
			capsuleCollider.direction = component.direction;
		}
	}

	// Token: 0x06006A73 RID: 27251 RVA: 0x00281C2C File Offset: 0x0028002C
	private void copyBoxCollider(GameObject refgo, GameObject go)
	{
		BoxCollider component = refgo.GetComponent<BoxCollider>();
		BoxCollider boxCollider = go.GetComponent<BoxCollider>();
		if (component != null)
		{
			if (boxCollider == null)
			{
				boxCollider = go.AddComponent<BoxCollider>();
			}
			boxCollider.isTrigger = component.isTrigger;
			boxCollider.sharedMaterial = component.sharedMaterial;
			Vector3 center = this.MirrorVector(component.center);
			boxCollider.center = center;
			boxCollider.size = component.size;
		}
	}

	// Token: 0x06006A74 RID: 27252 RVA: 0x00281CA0 File Offset: 0x002800A0
	public void deleteColliders()
	{
		foreach (string text in this.getRBJointNames())
		{
			Rigidbody rigidbody = null;
			if (this.RBMap.TryGetValue(text, out rigidbody))
			{
				if (rigidbody == null)
				{
					Debug.LogError("RB " + text + " in RBMap was null");
					this.RBMap.Remove(text);
				}
				else
				{
					CapsuleCollider component = rigidbody.GetComponent<CapsuleCollider>();
					if (component != null)
					{
						UnityEngine.Object.DestroyImmediate(component);
					}
					SphereCollider component2 = rigidbody.GetComponent<SphereCollider>();
					if (component2 != null)
					{
						UnityEngine.Object.DestroyImmediate(component2);
					}
					BoxCollider component3 = rigidbody.GetComponent<BoxCollider>();
					if (component3 != null)
					{
						UnityEngine.Object.DestroyImmediate(component3);
					}
					List<GameObject> list = new List<GameObject>();
					IEnumerator enumerator2 = rigidbody.transform.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							object obj = enumerator2.Current;
							Transform transform = (Transform)obj;
							Rigidbody component4 = transform.GetComponent<Rigidbody>();
							if (component4 == null)
							{
								CapsuleCollider component5 = transform.GetComponent<CapsuleCollider>();
								BoxCollider component6 = transform.GetComponent<BoxCollider>();
								if (component5 != null || component6 != null)
								{
									list.Add(transform.gameObject);
								}
							}
						}
					}
					finally
					{
						IDisposable disposable;
						if ((disposable = (enumerator2 as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
					foreach (GameObject obj2 in list)
					{
						UnityEngine.Object.DestroyImmediate(obj2);
					}
				}
			}
		}
	}

	// Token: 0x06006A75 RID: 27253 RVA: 0x00281EA4 File Offset: 0x002802A4
	private void copyComponent(Component src, GameObject dest)
	{
		Type type = src.GetType();
		Component component = dest.GetComponent(type);
		if (component == null)
		{
			component = dest.AddComponent(type);
		}
		FieldInfo[] fields = type.GetFields();
		foreach (FieldInfo fieldInfo in fields)
		{
			fieldInfo.SetValue(component, fieldInfo.GetValue(src));
		}
		PropertyInfo[] properties = type.GetProperties();
		foreach (PropertyInfo propertyInfo in properties)
		{
			if (propertyInfo.CanWrite && propertyInfo.Name != "name")
			{
				propertyInfo.SetValue(component, propertyInfo.GetValue(src, null), null);
			}
		}
	}

	// Token: 0x06006A76 RID: 27254 RVA: 0x00281F6C File Offset: 0x0028036C
	public void copyRigidbody()
	{
		if (this.copyFromRoot)
		{
			foreach (string text in this.getCopyFromRBJointNames())
			{
				Rigidbody rigidbody = null;
				Rigidbody copyFromRB = this.getCopyFromRB(text);
				string text2 = Regex.Replace(text, this.replaceRegexp, this.withString);
				if (!this.RBMap.TryGetValue(text2, out rigidbody))
				{
					Transform transform = this.getTransform(text2);
					if (transform == null)
					{
						string name = copyFromRB.transform.parent.name;
						string text3 = Regex.Replace(name, this.replaceRegexp, this.withString);
						Transform transform2 = this.getTransform(text3);
						if (transform2 != null)
						{
							GameObject gameObject = new GameObject(text2);
							gameObject.name = text2;
							if (!this.transformMap.ContainsKey(text2))
							{
								this.transformMap.Add(text2, gameObject.transform);
							}
							gameObject.transform.parent = transform2;
							gameObject.transform.localPosition = this.MirrorVector(copyFromRB.transform.localPosition);
							gameObject.transform.localRotation = copyFromRB.transform.localRotation;
							gameObject.transform.localScale = copyFromRB.transform.localScale;
							rigidbody = gameObject.AddComponent<Rigidbody>();
							this.RBMap.Add(text2, rigidbody);
						}
						else
						{
							Debug.LogError("could not find parent transform " + text3 + " during copy");
						}
					}
					else
					{
						rigidbody = transform.gameObject.AddComponent<Rigidbody>();
						this.RBMap.Add(text2, rigidbody);
					}
				}
				if (rigidbody != null && copyFromRB != null)
				{
					rigidbody.gameObject.layer = copyFromRB.gameObject.layer;
					rigidbody.mass = copyFromRB.mass;
					rigidbody.drag = copyFromRB.drag;
					rigidbody.angularDrag = copyFromRB.angularDrag;
					rigidbody.useGravity = copyFromRB.useGravity;
					rigidbody.interpolation = copyFromRB.interpolation;
					rigidbody.collisionDetectionMode = copyFromRB.collisionDetectionMode;
					rigidbody.isKinematic = copyFromRB.isKinematic;
					rigidbody.constraints = copyFromRB.constraints;
					this.copyCapsuleCollider(copyFromRB.gameObject, rigidbody.gameObject);
					this.copyBoxCollider(copyFromRB.gameObject, rigidbody.gameObject);
					IEnumerator enumerator2 = copyFromRB.transform.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							object obj = enumerator2.Current;
							Transform transform3 = (Transform)obj;
							Rigidbody component = transform3.GetComponent<Rigidbody>();
							if (component == null)
							{
								CapsuleCollider component2 = transform3.GetComponent<CapsuleCollider>();
								BoxCollider component3 = transform3.GetComponent<BoxCollider>();
								if (component2 != null || component3 != null)
								{
									string name2 = transform3.name;
									GameObject gameObject2 = null;
									string text4 = Regex.Replace(name2, this.replaceRegexp, this.withString);
									IEnumerator enumerator3 = rigidbody.transform.GetEnumerator();
									try
									{
										while (enumerator3.MoveNext())
										{
											object obj2 = enumerator3.Current;
											Transform transform4 = (Transform)obj2;
											if (transform4.name == text4)
											{
												gameObject2 = transform4.gameObject;
											}
										}
									}
									finally
									{
										IDisposable disposable;
										if ((disposable = (enumerator3 as IDisposable)) != null)
										{
											disposable.Dispose();
										}
									}
									if (gameObject2 == null)
									{
										gameObject2 = new GameObject(text4);
										gameObject2.name = text4;
										if (!this.transformMap.ContainsKey(text4))
										{
											this.transformMap.Add(text4, gameObject2.transform);
										}
										gameObject2.transform.parent = rigidbody.transform;
										gameObject2.transform.localPosition = this.MirrorVector(transform3.localPosition);
										gameObject2.transform.localRotation = this.MirrorQuaternion(transform3.localRotation);
										gameObject2.transform.localScale = transform3.localScale;
									}
									this.copyCapsuleCollider(transform3.gameObject, gameObject2);
									this.copyBoxCollider(transform3.gameObject, gameObject2);
									gameObject2.layer = transform3.gameObject.layer;
								}
							}
						}
					}
					finally
					{
						IDisposable disposable2;
						if ((disposable2 = (enumerator2 as IDisposable)) != null)
						{
							disposable2.Dispose();
						}
					}
				}
			}
		}
	}

	// Token: 0x06006A77 RID: 27255 RVA: 0x00282404 File Offset: 0x00280804
	public void copyConfigurableJoint()
	{
		if (this.copyFromRoot)
		{
			foreach (string text in this.getCopyFromJointNames())
			{
				ConfigurableJoint configurableJoint = null;
				ConfigurableJoint copyFromJoint = this.getCopyFromJoint(text);
				string text2 = Regex.Replace(text, this.replaceRegexp, this.withString);
				if (!this.jointMap.TryGetValue(text2, out configurableJoint))
				{
					Transform transform = this.getTransform(text2);
					if (transform != null)
					{
						configurableJoint = transform.gameObject.AddComponent<ConfigurableJoint>();
						this.jointMap.Add(text2, configurableJoint);
						if (copyFromJoint.connectedBody)
						{
							string name = copyFromJoint.connectedBody.name;
							string text3 = Regex.Replace(name, this.replaceRegexp, this.withString);
							Rigidbody connectedBody;
							if (this.RBMap.TryGetValue(text3, out connectedBody))
							{
								configurableJoint.connectedBody = connectedBody;
							}
							else
							{
								Debug.LogError("could not find parentRB " + text3 + " during copy");
							}
						}
						else
						{
							Debug.LogError("ref joint " + text2 + " doesn't have a connected body during copy");
						}
					}
					else
					{
						Debug.LogError("could not find transform " + text2 + " during copy");
					}
				}
				if (configurableJoint != null && copyFromJoint != null)
				{
					configurableJoint.axis = copyFromJoint.axis;
					configurableJoint.secondaryAxis = copyFromJoint.secondaryAxis;
					configurableJoint.xMotion = copyFromJoint.xMotion;
					configurableJoint.yMotion = copyFromJoint.yMotion;
					configurableJoint.zMotion = copyFromJoint.zMotion;
					configurableJoint.angularXMotion = copyFromJoint.angularXMotion;
					configurableJoint.angularYMotion = copyFromJoint.angularYMotion;
					configurableJoint.angularZMotion = copyFromJoint.angularZMotion;
					configurableJoint.linearLimit = copyFromJoint.linearLimit;
					if ((configurableJoint.axis.x == 1f && this.invertAxis != MirrorCopyPhysicsComponents.InvertAxis.X) || (configurableJoint.axis.y == 1f && this.invertAxis != MirrorCopyPhysicsComponents.InvertAxis.Y) || (configurableJoint.axis.z == 1f && this.invertAxis != MirrorCopyPhysicsComponents.InvertAxis.Z))
					{
						SoftJointLimit softJointLimit = copyFromJoint.lowAngularXLimit;
						softJointLimit.limit = -softJointLimit.limit;
						configurableJoint.highAngularXLimit = softJointLimit;
						softJointLimit = copyFromJoint.highAngularXLimit;
						softJointLimit.limit = -softJointLimit.limit;
						configurableJoint.lowAngularXLimit = softJointLimit;
					}
					else
					{
						configurableJoint.lowAngularXLimit = copyFromJoint.lowAngularXLimit;
						configurableJoint.highAngularXLimit = copyFromJoint.highAngularXLimit;
					}
					configurableJoint.angularYLimit = copyFromJoint.angularYLimit;
					configurableJoint.angularZLimit = copyFromJoint.angularZLimit;
					configurableJoint.targetPosition = copyFromJoint.targetPosition;
					configurableJoint.targetVelocity = copyFromJoint.targetVelocity;
					configurableJoint.xDrive = copyFromJoint.xDrive;
					configurableJoint.yDrive = copyFromJoint.yDrive;
					configurableJoint.zDrive = copyFromJoint.zDrive;
					configurableJoint.targetRotation = copyFromJoint.targetRotation;
					configurableJoint.targetAngularVelocity = copyFromJoint.targetAngularVelocity;
					configurableJoint.rotationDriveMode = copyFromJoint.rotationDriveMode;
					configurableJoint.angularXDrive = copyFromJoint.angularXDrive;
					configurableJoint.angularYZDrive = copyFromJoint.angularYZDrive;
					configurableJoint.slerpDrive = copyFromJoint.slerpDrive;
					configurableJoint.projectionAngle = copyFromJoint.projectionAngle;
					configurableJoint.projectionDistance = copyFromJoint.projectionDistance;
					configurableJoint.projectionMode = copyFromJoint.projectionMode;
				}
			}
		}
	}

	// Token: 0x06006A78 RID: 27256 RVA: 0x00282784 File Offset: 0x00280B84
	public void copyOthers()
	{
		List<Component> list = new List<Component>();
		IgnoreChildColliders[] componentsInChildren = this.copyFromRoot.GetComponentsInChildren<IgnoreChildColliders>();
		foreach (IgnoreChildColliders item in componentsInChildren)
		{
			list.Add(item);
		}
		AdjustJointSpring[] componentsInChildren2 = this.copyFromRoot.GetComponentsInChildren<AdjustJointSpring>();
		foreach (AdjustJointSpring item2 in componentsInChildren2)
		{
			list.Add(item2);
		}
		AdjustJointSprings[] componentsInChildren3 = this.copyFromRoot.GetComponentsInChildren<AdjustJointSprings>();
		foreach (AdjustJointSprings item3 in componentsInChildren3)
		{
			list.Add(item3);
		}
		AdjustJointTarget[] componentsInChildren4 = this.copyFromRoot.GetComponentsInChildren<AdjustJointTarget>();
		foreach (AdjustJointTarget item4 in componentsInChildren4)
		{
			list.Add(item4);
		}
		AdjustJointTargets[] componentsInChildren5 = this.copyFromRoot.GetComponentsInChildren<AdjustJointTargets>();
		foreach (AdjustJointTargets item5 in componentsInChildren5)
		{
			list.Add(item5);
		}
		ForceReceiver[] componentsInChildren6 = this.copyFromRoot.GetComponentsInChildren<ForceReceiver>();
		foreach (ForceReceiver item6 in componentsInChildren6)
		{
			list.Add(item6);
		}
		foreach (Component component in list)
		{
			string name = component.name;
			string key = Regex.Replace(name, this.replaceRegexp, this.withString);
			Transform transform;
			if (this.transformMap.TryGetValue(key, out transform))
			{
				this.copyComponent(component, transform.gameObject);
			}
		}
	}

	// Token: 0x06006A79 RID: 27257 RVA: 0x0028296C File Offset: 0x00280D6C
	public void copy()
	{
		this.copyRigidbody();
		this.copyConfigurableJoint();
		this.copyOthers();
	}

	// Token: 0x04005C6E RID: 23662
	public MirrorCopyPhysicsComponents.InvertAxis invertAxis;

	// Token: 0x04005C6F RID: 23663
	public Transform copyFromRoot;

	// Token: 0x04005C70 RID: 23664
	private Dictionary<string, Rigidbody> copyFromRBMap;

	// Token: 0x04005C71 RID: 23665
	private Dictionary<string, Rigidbody> RBMap;

	// Token: 0x04005C72 RID: 23666
	private Dictionary<string, Transform> copyFromTransformMap;

	// Token: 0x04005C73 RID: 23667
	private Dictionary<string, Transform> transformMap;

	// Token: 0x04005C74 RID: 23668
	private Dictionary<string, ConfigurableJoint> copyFromJointMap;

	// Token: 0x04005C75 RID: 23669
	private Dictionary<string, ConfigurableJoint> jointMap;

	// Token: 0x04005C76 RID: 23670
	public string replaceRegexp = "^r";

	// Token: 0x04005C77 RID: 23671
	public string withString = "l";

	// Token: 0x02000D80 RID: 3456
	public enum InvertAxis
	{
		// Token: 0x04005C79 RID: 23673
		X,
		// Token: 0x04005C7A RID: 23674
		Y,
		// Token: 0x04005C7B RID: 23675
		Z
	}
}
