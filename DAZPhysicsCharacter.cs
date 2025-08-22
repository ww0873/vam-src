using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

// Token: 0x02000AFA RID: 2810
public class DAZPhysicsCharacter : MonoBehaviour
{
	// Token: 0x06004BB8 RID: 19384 RVA: 0x001A5D5F File Offset: 0x001A415F
	public DAZPhysicsCharacter()
	{
	}

	// Token: 0x06004BB9 RID: 19385 RVA: 0x001A5D74 File Offset: 0x001A4174
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
			capsuleCollider.center = component.center;
			capsuleCollider.radius = component.radius;
			capsuleCollider.height = component.height;
			capsuleCollider.direction = component.direction;
		}
	}

	// Token: 0x06004BBA RID: 19386 RVA: 0x001A5DF8 File Offset: 0x001A41F8
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
			boxCollider.center = component.center;
			boxCollider.size = component.size;
		}
	}

	// Token: 0x06004BBB RID: 19387 RVA: 0x001A5E64 File Offset: 0x001A4264
	public void deleteColliders()
	{
		if (this.copyFrom)
		{
			foreach (string text in this.copyFrom.getRBJointNames())
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
						BoxCollider component2 = rigidbody.GetComponent<BoxCollider>();
						if (component2 != null)
						{
							UnityEngine.Object.DestroyImmediate(component2);
						}
						List<GameObject> list = new List<GameObject>();
						IEnumerator enumerator2 = rigidbody.transform.GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								object obj = enumerator2.Current;
								Transform transform = (Transform)obj;
								Rigidbody component3 = transform.GetComponent<Rigidbody>();
								if (component3 == null)
								{
									CapsuleCollider component4 = transform.GetComponent<CapsuleCollider>();
									BoxCollider component5 = transform.GetComponent<BoxCollider>();
									if (component4 != null || component5 != null)
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
	}

	// Token: 0x06004BBC RID: 19388 RVA: 0x001A6060 File Offset: 0x001A4460
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
	}

	// Token: 0x06004BBD RID: 19389 RVA: 0x001A60C8 File Offset: 0x001A44C8
	public void copyOthers()
	{
		LookAtWithLimits[] componentsInChildren = this.copyFrom.GetComponentsInChildren<LookAtWithLimits>();
		foreach (LookAtWithLimits component in componentsInChildren)
		{
			Transform transform;
			if (this.TransformMap.TryGetValue(component.name, out transform))
			{
				this.copyComponent(component, transform.gameObject);
			}
		}
		SyncMaterialParameters[] componentsInChildren2 = this.copyFrom.GetComponentsInChildren<SyncMaterialParameters>();
		foreach (SyncMaterialParameters component2 in componentsInChildren2)
		{
			Transform transform2;
			if (this.TransformMap.TryGetValue(component2.name, out transform2))
			{
				this.copyComponent(component2, transform2.gameObject);
			}
		}
		AdjustJointTargetAndCOG[] componentsInChildren3 = this.copyFrom.GetComponentsInChildren<AdjustJointTargetAndCOG>();
		foreach (AdjustJointTargetAndCOG component3 in componentsInChildren3)
		{
			Transform transform3;
			if (this.TransformMap.TryGetValue(component3.name, out transform3))
			{
				this.copyComponent(component3, transform3.gameObject);
			}
		}
		BakedSkinMesh[] componentsInChildren4 = this.copyFrom.GetComponentsInChildren<BakedSkinMesh>();
		foreach (BakedSkinMesh component4 in componentsInChildren4)
		{
			Transform transform4;
			if (this.TransformMap.TryGetValue(component4.name, out transform4))
			{
				this.copyComponent(component4, transform4.gameObject);
			}
		}
		DAZHairMesh[] componentsInChildren5 = this.copyFrom.GetComponentsInChildren<DAZHairMesh>();
		foreach (DAZHairMesh component5 in componentsInChildren5)
		{
			Transform transform5;
			if (this.TransformMap.TryGetValue(component5.name, out transform5))
			{
				this.copyComponent(component5, transform5.gameObject);
			}
		}
		ForceReceiver[] componentsInChildren6 = this.copyFrom.GetComponentsInChildren<ForceReceiver>();
		foreach (ForceReceiver component6 in componentsInChildren6)
		{
			Transform transform6;
			if (this.TransformMap.TryGetValue(component6.name, out transform6))
			{
				this.copyComponent(component6, transform6.gameObject);
			}
		}
	}

	// Token: 0x06004BBE RID: 19390 RVA: 0x001A62E0 File Offset: 0x001A46E0
	public void copyRigidbody(bool useLocal = false)
	{
		if (this.copyFrom)
		{
			foreach (string text in this.copyFrom.getRBJointNames())
			{
				Rigidbody rigidbody = null;
				Rigidbody rb = this.copyFrom.getRB(text);
				if (!this.RBMap.TryGetValue(text, out rigidbody))
				{
					Transform transform = this.getTransform(text);
					if (transform == null)
					{
						string name = rb.transform.parent.name;
						Transform transform2 = this.getTransform(name);
						if (transform2 != null)
						{
							GameObject gameObject = new GameObject(text);
							gameObject.name = text;
							if (!this.TransformMap.ContainsKey(text))
							{
								this.TransformMap.Add(text, gameObject.transform);
							}
							gameObject.transform.parent = transform2;
							if (useLocal)
							{
								gameObject.transform.localPosition = rb.transform.localPosition;
								gameObject.transform.localRotation = rb.transform.localRotation;
							}
							else
							{
								gameObject.transform.position = rb.transform.position;
								gameObject.transform.rotation = rb.transform.rotation;
							}
							gameObject.transform.localScale = rb.transform.localScale;
							rigidbody = gameObject.AddComponent<Rigidbody>();
							this.RBMap.Add(text, rigidbody);
						}
						else
						{
							Debug.LogError("could not find parent transform " + name + " during copy");
						}
					}
					else
					{
						rigidbody = transform.gameObject.AddComponent<Rigidbody>();
						this.RBMap.Add(text, rigidbody);
					}
				}
				if (rigidbody != null && rb != null)
				{
					string input = LayerMask.LayerToName(rb.gameObject.layer);
					string text2 = Regex.Replace(input, "^[0-9]", this.layerPrefix);
					int num = LayerMask.NameToLayer(text2);
					if (num != -1)
					{
						rigidbody.gameObject.layer = num;
					}
					else
					{
						Debug.LogError("could not find layer " + text2 + " during RB copy");
					}
					rigidbody.mass = rb.mass;
					rigidbody.drag = rb.drag;
					rigidbody.angularDrag = rb.angularDrag;
					rigidbody.useGravity = rb.useGravity;
					rigidbody.interpolation = rb.interpolation;
					rigidbody.collisionDetectionMode = rb.collisionDetectionMode;
					rigidbody.isKinematic = rb.isKinematic;
					rigidbody.constraints = rb.constraints;
					this.copyCapsuleCollider(rb.gameObject, rigidbody.gameObject);
					this.copyBoxCollider(rb.gameObject, rigidbody.gameObject);
					IEnumerator enumerator2 = rb.transform.GetEnumerator();
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
									IEnumerator enumerator3 = rigidbody.transform.GetEnumerator();
									try
									{
										while (enumerator3.MoveNext())
										{
											object obj2 = enumerator3.Current;
											Transform transform4 = (Transform)obj2;
											if (transform4.name == name2)
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
										gameObject2 = new GameObject(name2);
										gameObject2.name = name2;
										if (!this.TransformMap.ContainsKey(name2))
										{
											this.TransformMap.Add(name2, gameObject2.transform);
										}
										gameObject2.transform.parent = rigidbody.transform;
										if (useLocal)
										{
											gameObject2.transform.localPosition = transform3.localPosition;
											gameObject2.transform.localRotation = transform3.localRotation;
										}
										else
										{
											gameObject2.transform.position = transform3.position;
											gameObject2.transform.rotation = transform3.rotation;
										}
										gameObject2.transform.localScale = transform3.localScale;
									}
									this.copyCapsuleCollider(transform3.gameObject, gameObject2);
									this.copyBoxCollider(transform3.gameObject, gameObject2);
									input = LayerMask.LayerToName(transform3.gameObject.layer);
									text2 = Regex.Replace(input, "^[0-9]", this.layerPrefix);
									num = LayerMask.NameToLayer(text2);
									if (num != -1)
									{
										gameObject2.layer = num;
									}
									else
									{
										Debug.LogError("could not find layer " + text2 + " during RB copy");
									}
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

	// Token: 0x06004BBF RID: 19391 RVA: 0x001A6824 File Offset: 0x001A4C24
	public void createForceReceivers()
	{
		string[] array = new string[]
		{
			"hip",
			"abdomen",
			"abdomen2",
			"chest",
			"lCollar",
			"lShldr",
			"lForeArm",
			"lHand",
			"rCollar",
			"rShldr",
			"rForeArm",
			"rHand",
			"pelvis",
			"lThigh",
			"lShin",
			"lFoot",
			"rThigh",
			"rShin",
			"rFoot"
		};
		foreach (string key in array)
		{
			Transform transform;
			if (this.TransformMap.TryGetValue(key, out transform))
			{
				ForceReceiver component = transform.GetComponent<ForceReceiver>();
				if (component == null)
				{
					transform.gameObject.AddComponent<ForceReceiver>();
				}
			}
		}
	}

	// Token: 0x06004BC0 RID: 19392 RVA: 0x001A692C File Offset: 0x001A4D2C
	public void copyConfigurableJoint()
	{
		if (this.copyFrom)
		{
			foreach (string text in this.copyFrom.getJointNames())
			{
				ConfigurableJoint configurableJoint = null;
				ConfigurableJoint joint = this.copyFrom.getJoint(text);
				if (!this.JointMap.TryGetValue(text, out configurableJoint))
				{
					Transform transform = this.getTransform(text);
					if (transform != null)
					{
						configurableJoint = transform.gameObject.AddComponent<ConfigurableJoint>();
						this.JointMap.Add(text, configurableJoint);
						if (joint.connectedBody)
						{
							string name = joint.connectedBody.name;
							Rigidbody connectedBody;
							if (this.RBMap.TryGetValue(name, out connectedBody))
							{
								configurableJoint.connectedBody = connectedBody;
							}
							else
							{
								Debug.LogError("could not find parentRB " + name + " during copy");
							}
						}
						else
						{
							Debug.LogError("ref joint " + text + " doesn't have a connected body during copy");
						}
					}
					else
					{
						Debug.LogError("could not find transform " + text + " during copy");
					}
				}
				if (configurableJoint != null && joint != null)
				{
					configurableJoint.axis = joint.axis;
					configurableJoint.secondaryAxis = joint.secondaryAxis;
					configurableJoint.xMotion = joint.xMotion;
					configurableJoint.yMotion = joint.yMotion;
					configurableJoint.zMotion = joint.zMotion;
					configurableJoint.angularXMotion = joint.angularXMotion;
					configurableJoint.angularYMotion = joint.angularYMotion;
					configurableJoint.angularZMotion = joint.angularZMotion;
					configurableJoint.linearLimit = joint.linearLimit;
					configurableJoint.lowAngularXLimit = joint.lowAngularXLimit;
					configurableJoint.highAngularXLimit = joint.highAngularXLimit;
					configurableJoint.angularYLimit = joint.angularYLimit;
					configurableJoint.angularZLimit = joint.angularZLimit;
					configurableJoint.targetPosition = joint.targetPosition;
					configurableJoint.targetVelocity = joint.targetVelocity;
					configurableJoint.xDrive = joint.xDrive;
					configurableJoint.yDrive = joint.yDrive;
					configurableJoint.zDrive = joint.zDrive;
					configurableJoint.targetRotation = joint.targetRotation;
					configurableJoint.targetAngularVelocity = joint.targetAngularVelocity;
					configurableJoint.rotationDriveMode = joint.rotationDriveMode;
					configurableJoint.angularXDrive = joint.angularXDrive;
					configurableJoint.angularYZDrive = joint.angularYZDrive;
					configurableJoint.slerpDrive = joint.slerpDrive;
					configurableJoint.projectionAngle = joint.projectionAngle;
					configurableJoint.projectionDistance = joint.projectionDistance;
					configurableJoint.projectionMode = joint.projectionMode;
				}
			}
		}
	}

	// Token: 0x06004BC1 RID: 19393 RVA: 0x001A6BD8 File Offset: 0x001A4FD8
	public void copySetCenterOfGravity()
	{
		if (this.copyFrom)
		{
			foreach (string text in this.copyFrom.getCGJointNames())
			{
				SetCenterOfGravity setCenterOfGravity = null;
				SetCenterOfGravity cg = this.copyFrom.getCG(text);
				if (!this.CGMap.TryGetValue(text, out setCenterOfGravity))
				{
					Transform transform = this.getTransform(text);
					if (transform != null)
					{
						setCenterOfGravity = transform.gameObject.AddComponent<SetCenterOfGravity>();
						this.CGMap.Add(text, setCenterOfGravity);
					}
					else
					{
						Debug.LogError("could not find transform " + text + " during copy");
					}
				}
				if (setCenterOfGravity != null && cg != null)
				{
					setCenterOfGravity.enabled = cg.enabled;
					setCenterOfGravity.liveUpdate = cg.liveUpdate;
					setCenterOfGravity.centerOfGravity = cg.centerOfGravity;
				}
			}
		}
	}

	// Token: 0x06004BC2 RID: 19394 RVA: 0x001A6CE8 File Offset: 0x001A50E8
	public void copyFollowPhysically()
	{
		if (this.copyFrom)
		{
			foreach (string text in this.copyFrom.getFPJointNames())
			{
				FollowPhysicallySingle followPhysicallySingle = null;
				FollowPhysicallySingle fp = this.copyFrom.getFP(text);
				if (!this.FPMap.TryGetValue(text, out followPhysicallySingle))
				{
					Transform transform = this.getTransform(text);
					if (transform != null)
					{
						followPhysicallySingle = transform.gameObject.AddComponent<FollowPhysicallySingle>();
						this.FPMap.Add(text, followPhysicallySingle);
					}
					else
					{
						Debug.LogError("could not find transform " + text + " during copy");
					}
				}
				if (followPhysicallySingle != null && fp != null)
				{
					followPhysicallySingle.on = fp.on;
					followPhysicallySingle.drivePosition = fp.drivePosition;
					followPhysicallySingle.driveRotation = fp.driveRotation;
					followPhysicallySingle.useGlobalForceMultiplier = fp.useGlobalForceMultiplier;
					followPhysicallySingle.useGlobalTorqueMultiplier = fp.useGlobalTorqueMultiplier;
					FreeControllerV3 component;
					if (this.targets && this.TargetsMap.TryGetValue(text, out component))
					{
						followPhysicallySingle.follow = component.transform;
						component.followWhenOff = followPhysicallySingle.transform;
						Transform follow;
						if (this.followTree && this.FollowMap.TryGetValue(text, out follow))
						{
							component.follow = follow;
						}
					}
					else if (this.followTree)
					{
						Transform follow2;
						if (this.FollowMap.TryGetValue(text, out follow2))
						{
							followPhysicallySingle.follow = follow2;
						}
					}
					else
					{
						followPhysicallySingle.follow = fp.follow;
						component = followPhysicallySingle.follow.GetComponent<FreeControllerV3>();
						if (component != null)
						{
							component.followWhenOff = followPhysicallySingle.transform;
						}
					}
					followPhysicallySingle.moveMethod = fp.moveMethod;
					followPhysicallySingle.rotateMethod = fp.rotateMethod;
					followPhysicallySingle.PIDpresentFactorRot = fp.PIDpresentFactorRot;
					followPhysicallySingle.PIDintegralFactorRot = fp.PIDintegralFactorRot;
					followPhysicallySingle.PIDderivFactorRot = fp.PIDderivFactorRot;
					followPhysicallySingle.PIDpresentFactorPos = fp.PIDpresentFactorPos;
					followPhysicallySingle.PIDintegralFactorPos = fp.PIDintegralFactorPos;
					followPhysicallySingle.PIDderivFactorPos = fp.PIDderivFactorPos;
					followPhysicallySingle.forcePosition = fp.forcePosition;
					followPhysicallySingle.ForceMultiplier = fp.ForceMultiplier;
					followPhysicallySingle.TorqueMultiplier = fp.TorqueMultiplier;
					followPhysicallySingle.TorqueMultiplier2 = fp.TorqueMultiplier2;
					followPhysicallySingle.MaxForce = fp.MaxForce;
					followPhysicallySingle.MaxTorque = fp.MaxTorque;
					followPhysicallySingle.freezeMass = fp.freezeMass;
					followPhysicallySingle.controlledJointSpring = fp.controlledJointSpring;
					followPhysicallySingle.controlledJointMaxForce = fp.controlledJointMaxForce;
					followPhysicallySingle.debugForce = fp.debugForce;
					followPhysicallySingle.debugTorque = fp.debugTorque;
					followPhysicallySingle.lineMaterial = fp.lineMaterial;
					followPhysicallySingle.rotationLineMaterial = fp.rotationLineMaterial;
					if (fp.onIfAllFCV3sFollowing != null && this.targets)
					{
						followPhysicallySingle.onIfAllFCV3sFollowing = new FreeControllerV3[fp.onIfAllFCV3sFollowing.Length];
						int num = 0;
						foreach (FreeControllerV3 freeControllerV in fp.onIfAllFCV3sFollowing)
						{
							FreeControllerV3 freeControllerV2;
							if (this.TargetsMap.TryGetValue(freeControllerV.name, out freeControllerV2))
							{
								followPhysicallySingle.onIfAllFCV3sFollowing[num] = freeControllerV2;
							}
							num++;
						}
					}
				}
			}
		}
	}

	// Token: 0x06004BC3 RID: 19395 RVA: 0x001A706C File Offset: 0x001A546C
	public void copy()
	{
		this.copyRigidbody(false);
		this.copyConfigurableJoint();
		this.copySetCenterOfGravity();
		this.copyFollowPhysically();
		this.copyOthers();
	}

	// Token: 0x06004BC4 RID: 19396 RVA: 0x001A7090 File Offset: 0x001A5490
	public ICollection<string> getAllNames()
	{
		if (this.TransformMap == null)
		{
			this.init();
		}
		return this.TransformMap.Keys;
	}

	// Token: 0x06004BC5 RID: 19397 RVA: 0x001A70BC File Offset: 0x001A54BC
	public ICollection<string> getJointNames()
	{
		if (this.JointMap == null)
		{
			this.init();
		}
		return this.JointMap.Keys;
	}

	// Token: 0x06004BC6 RID: 19398 RVA: 0x001A70E8 File Offset: 0x001A54E8
	public ICollection<string> getRBJointNames()
	{
		if (this.RBMap == null)
		{
			this.init();
		}
		return this.RBMap.Keys;
	}

	// Token: 0x06004BC7 RID: 19399 RVA: 0x001A7114 File Offset: 0x001A5514
	public ICollection<string> getCGJointNames()
	{
		if (this.CGMap == null)
		{
			this.init();
		}
		return this.CGMap.Keys;
	}

	// Token: 0x06004BC8 RID: 19400 RVA: 0x001A7140 File Offset: 0x001A5540
	public ICollection<string> getFPJointNames()
	{
		if (this.FPMap == null)
		{
			this.init();
		}
		return this.FPMap.Keys;
	}

	// Token: 0x06004BC9 RID: 19401 RVA: 0x001A716C File Offset: 0x001A556C
	public Transform getTransform(string trans)
	{
		Transform result;
		if (this.TransformMap.TryGetValue(trans, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06004BCA RID: 19402 RVA: 0x001A7190 File Offset: 0x001A5590
	public Rigidbody getRB(string joint)
	{
		Rigidbody result;
		if (this.RBMap.TryGetValue(joint, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06004BCB RID: 19403 RVA: 0x001A71B4 File Offset: 0x001A55B4
	public SetCenterOfGravity getCG(string joint)
	{
		SetCenterOfGravity result;
		if (this.CGMap.TryGetValue(joint, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06004BCC RID: 19404 RVA: 0x001A71D8 File Offset: 0x001A55D8
	public FollowPhysicallySingle getFP(string joint)
	{
		FollowPhysicallySingle result;
		if (this.FPMap.TryGetValue(joint, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06004BCD RID: 19405 RVA: 0x001A71FC File Offset: 0x001A55FC
	public ConfigurableJoint getJoint(string joint)
	{
		ConfigurableJoint result;
		if (this.JointMap.TryGetValue(joint, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06004BCE RID: 19406 RVA: 0x001A7220 File Offset: 0x001A5620
	public float getLowAngularXLimit(string joint)
	{
		ConfigurableJoint configurableJoint;
		if (this.JointMap.TryGetValue(joint, out configurableJoint))
		{
			return configurableJoint.lowAngularXLimit.limit;
		}
		return 0f;
	}

	// Token: 0x06004BCF RID: 19407 RVA: 0x001A7254 File Offset: 0x001A5654
	public void setLowAngularXLimit(string joint, float limit)
	{
		ConfigurableJoint configurableJoint;
		if (this.JointMap.TryGetValue(joint, out configurableJoint))
		{
			SoftJointLimit lowAngularXLimit = configurableJoint.lowAngularXLimit;
			lowAngularXLimit.limit = limit;
			configurableJoint.lowAngularXLimit = lowAngularXLimit;
		}
	}

	// Token: 0x06004BD0 RID: 19408 RVA: 0x001A728C File Offset: 0x001A568C
	public float getHighAngularXLimit(string joint)
	{
		ConfigurableJoint configurableJoint;
		if (this.JointMap.TryGetValue(joint, out configurableJoint))
		{
			return configurableJoint.highAngularXLimit.limit;
		}
		return 0f;
	}

	// Token: 0x06004BD1 RID: 19409 RVA: 0x001A72C0 File Offset: 0x001A56C0
	public void setHighAngularXLimit(string joint, float limit)
	{
		ConfigurableJoint configurableJoint;
		if (this.JointMap.TryGetValue(joint, out configurableJoint))
		{
			SoftJointLimit highAngularXLimit = configurableJoint.highAngularXLimit;
			highAngularXLimit.limit = limit;
			configurableJoint.highAngularXLimit = highAngularXLimit;
		}
	}

	// Token: 0x06004BD2 RID: 19410 RVA: 0x001A72F8 File Offset: 0x001A56F8
	public float getAngularYLimit(string joint)
	{
		ConfigurableJoint configurableJoint;
		if (this.JointMap.TryGetValue(joint, out configurableJoint))
		{
			return configurableJoint.angularYLimit.limit;
		}
		return 0f;
	}

	// Token: 0x06004BD3 RID: 19411 RVA: 0x001A732C File Offset: 0x001A572C
	public void setAngularYLimit(string joint, float limit)
	{
		ConfigurableJoint configurableJoint;
		if (this.JointMap.TryGetValue(joint, out configurableJoint))
		{
			SoftJointLimit angularYLimit = configurableJoint.angularYLimit;
			angularYLimit.limit = limit;
			configurableJoint.angularYLimit = angularYLimit;
		}
	}

	// Token: 0x06004BD4 RID: 19412 RVA: 0x001A7364 File Offset: 0x001A5764
	public float getAngularZLimit(string joint)
	{
		ConfigurableJoint configurableJoint;
		if (this.JointMap.TryGetValue(joint, out configurableJoint))
		{
			return configurableJoint.angularZLimit.limit;
		}
		return 0f;
	}

	// Token: 0x06004BD5 RID: 19413 RVA: 0x001A7398 File Offset: 0x001A5798
	public void setAngularZLimit(string joint, float limit)
	{
		ConfigurableJoint configurableJoint;
		if (this.JointMap.TryGetValue(joint, out configurableJoint))
		{
			SoftJointLimit angularZLimit = configurableJoint.angularZLimit;
			angularZLimit.limit = limit;
			configurableJoint.angularZLimit = angularZLimit;
		}
	}

	// Token: 0x06004BD6 RID: 19414 RVA: 0x001A73D0 File Offset: 0x001A57D0
	public RotationDriveMode getRotationDriveMode(string joint)
	{
		ConfigurableJoint configurableJoint;
		if (this.JointMap.TryGetValue(joint, out configurableJoint))
		{
			return configurableJoint.rotationDriveMode;
		}
		return RotationDriveMode.Slerp;
	}

	// Token: 0x06004BD7 RID: 19415 RVA: 0x001A73F8 File Offset: 0x001A57F8
	public void setRotationDriveMode(string joint, RotationDriveMode mode)
	{
		ConfigurableJoint configurableJoint;
		if (this.JointMap.TryGetValue(joint, out configurableJoint))
		{
			configurableJoint.rotationDriveMode = mode;
		}
	}

	// Token: 0x06004BD8 RID: 19416 RVA: 0x001A7420 File Offset: 0x001A5820
	public float getAngularXDriveSpring(string joint)
	{
		ConfigurableJoint configurableJoint;
		if (this.JointMap.TryGetValue(joint, out configurableJoint))
		{
			return configurableJoint.angularXDrive.positionSpring;
		}
		return 0f;
	}

	// Token: 0x06004BD9 RID: 19417 RVA: 0x001A7454 File Offset: 0x001A5854
	public void setAngularXDriveSpring(string joint, float spring)
	{
		ConfigurableJoint configurableJoint;
		if (this.JointMap.TryGetValue(joint, out configurableJoint))
		{
			JointDrive angularXDrive = configurableJoint.angularXDrive;
			angularXDrive.positionSpring = spring;
			configurableJoint.angularXDrive = angularXDrive;
		}
	}

	// Token: 0x06004BDA RID: 19418 RVA: 0x001A748C File Offset: 0x001A588C
	public float getAngularYZDriveSpring(string joint)
	{
		ConfigurableJoint configurableJoint;
		if (this.JointMap.TryGetValue(joint, out configurableJoint))
		{
			return configurableJoint.angularYZDrive.positionSpring;
		}
		return 0f;
	}

	// Token: 0x06004BDB RID: 19419 RVA: 0x001A74C0 File Offset: 0x001A58C0
	public void setAngularYZDriveSpring(string joint, float spring)
	{
		ConfigurableJoint configurableJoint;
		if (this.JointMap.TryGetValue(joint, out configurableJoint))
		{
			JointDrive angularYZDrive = configurableJoint.angularYZDrive;
			angularYZDrive.positionSpring = spring;
			configurableJoint.angularYZDrive = angularYZDrive;
		}
	}

	// Token: 0x06004BDC RID: 19420 RVA: 0x001A74F8 File Offset: 0x001A58F8
	public float getAngularXDriveDamper(string joint)
	{
		ConfigurableJoint configurableJoint;
		if (this.JointMap.TryGetValue(joint, out configurableJoint))
		{
			return configurableJoint.angularXDrive.positionDamper;
		}
		return 0f;
	}

	// Token: 0x06004BDD RID: 19421 RVA: 0x001A752C File Offset: 0x001A592C
	public void setAngularXDriveDamper(string joint, float damper)
	{
		ConfigurableJoint configurableJoint;
		if (this.JointMap.TryGetValue(joint, out configurableJoint))
		{
			JointDrive angularXDrive = configurableJoint.angularXDrive;
			angularXDrive.positionDamper = damper;
			configurableJoint.angularXDrive = angularXDrive;
		}
	}

	// Token: 0x06004BDE RID: 19422 RVA: 0x001A7564 File Offset: 0x001A5964
	public float getAngularYZDriveDamper(string joint)
	{
		ConfigurableJoint configurableJoint;
		if (this.JointMap.TryGetValue(joint, out configurableJoint))
		{
			return configurableJoint.angularYZDrive.positionDamper;
		}
		return 0f;
	}

	// Token: 0x06004BDF RID: 19423 RVA: 0x001A7598 File Offset: 0x001A5998
	public void setAngularYZDriveDamper(string joint, float damper)
	{
		ConfigurableJoint configurableJoint;
		if (this.JointMap.TryGetValue(joint, out configurableJoint))
		{
			JointDrive angularYZDrive = configurableJoint.angularYZDrive;
			angularYZDrive.positionDamper = damper;
			configurableJoint.angularYZDrive = angularYZDrive;
		}
	}

	// Token: 0x06004BE0 RID: 19424 RVA: 0x001A75D0 File Offset: 0x001A59D0
	public float getAngularXDriveMaxForce(string joint)
	{
		ConfigurableJoint configurableJoint;
		if (this.JointMap.TryGetValue(joint, out configurableJoint))
		{
			return configurableJoint.angularXDrive.maximumForce;
		}
		return 0f;
	}

	// Token: 0x06004BE1 RID: 19425 RVA: 0x001A7604 File Offset: 0x001A5A04
	public void setAngularXDriveMaxForce(string joint, float force)
	{
		ConfigurableJoint configurableJoint;
		if (this.JointMap.TryGetValue(joint, out configurableJoint))
		{
			JointDrive angularXDrive = configurableJoint.angularXDrive;
			angularXDrive.maximumForce = force;
			configurableJoint.angularXDrive = angularXDrive;
		}
	}

	// Token: 0x06004BE2 RID: 19426 RVA: 0x001A763C File Offset: 0x001A5A3C
	public float getAngularYZDriveMaxForce(string joint)
	{
		ConfigurableJoint configurableJoint;
		if (this.JointMap.TryGetValue(joint, out configurableJoint))
		{
			return configurableJoint.angularYZDrive.maximumForce;
		}
		return 0f;
	}

	// Token: 0x06004BE3 RID: 19427 RVA: 0x001A7670 File Offset: 0x001A5A70
	public void setAngularYZDriveMaxForce(string joint, float force)
	{
		ConfigurableJoint configurableJoint;
		if (this.JointMap.TryGetValue(joint, out configurableJoint))
		{
			JointDrive angularYZDrive = configurableJoint.angularYZDrive;
			angularYZDrive.maximumForce = force;
			configurableJoint.angularYZDrive = angularYZDrive;
		}
	}

	// Token: 0x06004BE4 RID: 19428 RVA: 0x001A76A8 File Offset: 0x001A5AA8
	public float getSlerpDriveSpring(string joint)
	{
		ConfigurableJoint configurableJoint;
		if (this.JointMap.TryGetValue(joint, out configurableJoint))
		{
			return configurableJoint.slerpDrive.positionSpring;
		}
		return 0f;
	}

	// Token: 0x06004BE5 RID: 19429 RVA: 0x001A76DC File Offset: 0x001A5ADC
	public void setSlerpDriveSpring(string joint, float spring)
	{
		ConfigurableJoint configurableJoint;
		if (this.JointMap.TryGetValue(joint, out configurableJoint))
		{
			JointDrive slerpDrive = configurableJoint.slerpDrive;
			slerpDrive.positionSpring = spring;
			configurableJoint.slerpDrive = slerpDrive;
		}
	}

	// Token: 0x06004BE6 RID: 19430 RVA: 0x001A7714 File Offset: 0x001A5B14
	public float getSlerpDriveDamper(string joint)
	{
		ConfigurableJoint configurableJoint;
		if (this.JointMap.TryGetValue(joint, out configurableJoint))
		{
			return configurableJoint.slerpDrive.positionDamper;
		}
		return 0f;
	}

	// Token: 0x06004BE7 RID: 19431 RVA: 0x001A7748 File Offset: 0x001A5B48
	public void setSlerpDriveDamper(string joint, float damper)
	{
		ConfigurableJoint configurableJoint;
		if (this.JointMap.TryGetValue(joint, out configurableJoint))
		{
			JointDrive slerpDrive = configurableJoint.slerpDrive;
			slerpDrive.positionDamper = damper;
			configurableJoint.slerpDrive = slerpDrive;
		}
	}

	// Token: 0x06004BE8 RID: 19432 RVA: 0x001A7780 File Offset: 0x001A5B80
	public float getSlerpDriveMaxForce(string joint)
	{
		ConfigurableJoint configurableJoint;
		if (this.JointMap.TryGetValue(joint, out configurableJoint))
		{
			return configurableJoint.slerpDrive.maximumForce;
		}
		return 0f;
	}

	// Token: 0x06004BE9 RID: 19433 RVA: 0x001A77B4 File Offset: 0x001A5BB4
	public void setSlerpDriveMaxForce(string joint, float force)
	{
		ConfigurableJoint configurableJoint;
		if (this.JointMap.TryGetValue(joint, out configurableJoint))
		{
			JointDrive slerpDrive = configurableJoint.slerpDrive;
			slerpDrive.maximumForce = force;
			configurableJoint.slerpDrive = slerpDrive;
		}
	}

	// Token: 0x06004BEA RID: 19434 RVA: 0x001A77EC File Offset: 0x001A5BEC
	public bool isProjectionOn(string joint)
	{
		ConfigurableJoint configurableJoint;
		return this.JointMap.TryGetValue(joint, out configurableJoint) && configurableJoint.projectionMode != JointProjectionMode.None;
	}

	// Token: 0x06004BEB RID: 19435 RVA: 0x001A781C File Offset: 0x001A5C1C
	public float getMass(string joint)
	{
		Rigidbody rigidbody;
		if (this.RBMap.TryGetValue(joint, out rigidbody))
		{
			return rigidbody.mass;
		}
		return 0f;
	}

	// Token: 0x06004BEC RID: 19436 RVA: 0x001A7848 File Offset: 0x001A5C48
	public void setMass(string joint, float mass)
	{
		Rigidbody rigidbody;
		if (this.RBMap.TryGetValue(joint, out rigidbody))
		{
			rigidbody.mass = mass;
		}
	}

	// Token: 0x06004BED RID: 19437 RVA: 0x001A7870 File Offset: 0x001A5C70
	public float getDrag(string joint)
	{
		Rigidbody rigidbody;
		if (this.RBMap.TryGetValue(joint, out rigidbody))
		{
			return rigidbody.drag;
		}
		return 0f;
	}

	// Token: 0x06004BEE RID: 19438 RVA: 0x001A789C File Offset: 0x001A5C9C
	public void setDrag(string joint, float drag)
	{
		Rigidbody rigidbody;
		if (this.RBMap.TryGetValue(joint, out rigidbody))
		{
			rigidbody.drag = drag;
		}
	}

	// Token: 0x06004BEF RID: 19439 RVA: 0x001A78C4 File Offset: 0x001A5CC4
	public float getAngularDrag(string joint)
	{
		Rigidbody rigidbody;
		if (this.RBMap.TryGetValue(joint, out rigidbody))
		{
			return rigidbody.angularDrag;
		}
		return 0f;
	}

	// Token: 0x06004BF0 RID: 19440 RVA: 0x001A78F0 File Offset: 0x001A5CF0
	public void setAngularDrag(string joint, float angDrag)
	{
		Rigidbody rigidbody;
		if (this.RBMap.TryGetValue(joint, out rigidbody))
		{
			rigidbody.angularDrag = angDrag;
		}
	}

	// Token: 0x06004BF1 RID: 19441 RVA: 0x001A7918 File Offset: 0x001A5D18
	public bool getUseGravity(string joint)
	{
		Rigidbody rigidbody;
		return this.RBMap.TryGetValue(joint, out rigidbody) && rigidbody.useGravity;
	}

	// Token: 0x06004BF2 RID: 19442 RVA: 0x001A7940 File Offset: 0x001A5D40
	public void setUseGravity(string joint, bool use)
	{
		Rigidbody rigidbody;
		if (this.RBMap.TryGetValue(joint, out rigidbody))
		{
			rigidbody.useGravity = use;
		}
	}

	// Token: 0x06004BF3 RID: 19443 RVA: 0x001A7968 File Offset: 0x001A5D68
	public bool getSetCenterOfGravity(string joint)
	{
		SetCenterOfGravity setCenterOfGravity;
		return this.CGMap.TryGetValue(joint, out setCenterOfGravity) && setCenterOfGravity.enabled;
	}

	// Token: 0x06004BF4 RID: 19444 RVA: 0x001A7990 File Offset: 0x001A5D90
	public void setSetCenterOfGravity(string joint, bool use)
	{
		SetCenterOfGravity setCenterOfGravity;
		if (this.CGMap.TryGetValue(joint, out setCenterOfGravity))
		{
			setCenterOfGravity.enabled = use;
		}
	}

	// Token: 0x06004BF5 RID: 19445 RVA: 0x001A79B8 File Offset: 0x001A5DB8
	public float getPIDpresentFactorPos(string joint)
	{
		FollowPhysicallySingle followPhysicallySingle;
		if (this.FPMap.TryGetValue(joint, out followPhysicallySingle))
		{
			return followPhysicallySingle.PIDpresentFactorPos;
		}
		return 0f;
	}

	// Token: 0x06004BF6 RID: 19446 RVA: 0x001A79E4 File Offset: 0x001A5DE4
	public void setPIDpresentFactorPos(string joint, float val)
	{
		FollowPhysicallySingle followPhysicallySingle;
		if (this.FPMap.TryGetValue(joint, out followPhysicallySingle))
		{
			followPhysicallySingle.PIDpresentFactorPos = val;
		}
	}

	// Token: 0x06004BF7 RID: 19447 RVA: 0x001A7A0C File Offset: 0x001A5E0C
	public float getPIDintegralFactorPos(string joint)
	{
		FollowPhysicallySingle followPhysicallySingle;
		if (this.FPMap.TryGetValue(joint, out followPhysicallySingle))
		{
			return followPhysicallySingle.PIDintegralFactorPos;
		}
		return 0f;
	}

	// Token: 0x06004BF8 RID: 19448 RVA: 0x001A7A38 File Offset: 0x001A5E38
	public void setPIDintegralFactorPos(string joint, float val)
	{
		FollowPhysicallySingle followPhysicallySingle;
		if (this.FPMap.TryGetValue(joint, out followPhysicallySingle))
		{
			followPhysicallySingle.PIDintegralFactorPos = val;
		}
	}

	// Token: 0x06004BF9 RID: 19449 RVA: 0x001A7A60 File Offset: 0x001A5E60
	public float getPIDderivFactorPos(string joint)
	{
		FollowPhysicallySingle followPhysicallySingle;
		if (this.FPMap.TryGetValue(joint, out followPhysicallySingle))
		{
			return followPhysicallySingle.PIDderivFactorPos;
		}
		return 0f;
	}

	// Token: 0x06004BFA RID: 19450 RVA: 0x001A7A8C File Offset: 0x001A5E8C
	public void setAllPIDderivFactorPosFromUISlider()
	{
	}

	// Token: 0x06004BFB RID: 19451 RVA: 0x001A7A90 File Offset: 0x001A5E90
	public void setPIDderivFactorPos(string joint, float val)
	{
		FollowPhysicallySingle followPhysicallySingle;
		if (this.FPMap.TryGetValue(joint, out followPhysicallySingle))
		{
			followPhysicallySingle.PIDderivFactorPos = val;
		}
	}

	// Token: 0x06004BFC RID: 19452 RVA: 0x001A7AB8 File Offset: 0x001A5EB8
	public float getPIDpresentFactorRot(string joint)
	{
		FollowPhysicallySingle followPhysicallySingle;
		if (this.FPMap.TryGetValue(joint, out followPhysicallySingle))
		{
			return followPhysicallySingle.PIDpresentFactorRot;
		}
		return 0f;
	}

	// Token: 0x06004BFD RID: 19453 RVA: 0x001A7AE4 File Offset: 0x001A5EE4
	public void setPIDpresentFactorRot(string joint, float val)
	{
		FollowPhysicallySingle followPhysicallySingle;
		if (this.FPMap.TryGetValue(joint, out followPhysicallySingle))
		{
			followPhysicallySingle.PIDpresentFactorRot = val;
		}
	}

	// Token: 0x06004BFE RID: 19454 RVA: 0x001A7B0C File Offset: 0x001A5F0C
	public float getPIDintegralFactorRot(string joint)
	{
		FollowPhysicallySingle followPhysicallySingle;
		if (this.FPMap.TryGetValue(joint, out followPhysicallySingle))
		{
			return followPhysicallySingle.PIDintegralFactorRot;
		}
		return 0f;
	}

	// Token: 0x06004BFF RID: 19455 RVA: 0x001A7B38 File Offset: 0x001A5F38
	public void setPIDintegralFactorRot(string joint, float val)
	{
		FollowPhysicallySingle followPhysicallySingle;
		if (this.FPMap.TryGetValue(joint, out followPhysicallySingle))
		{
			followPhysicallySingle.PIDintegralFactorRot = val;
		}
	}

	// Token: 0x06004C00 RID: 19456 RVA: 0x001A7B60 File Offset: 0x001A5F60
	public float getPIDderivFactorRot(string joint)
	{
		FollowPhysicallySingle followPhysicallySingle;
		if (this.FPMap.TryGetValue(joint, out followPhysicallySingle))
		{
			return followPhysicallySingle.PIDderivFactorRot;
		}
		return 0f;
	}

	// Token: 0x06004C01 RID: 19457 RVA: 0x001A7B8C File Offset: 0x001A5F8C
	public void setPIDderivFactorRot(string joint, float val)
	{
		FollowPhysicallySingle followPhysicallySingle;
		if (this.FPMap.TryGetValue(joint, out followPhysicallySingle))
		{
			followPhysicallySingle.PIDderivFactorRot = val;
		}
	}

	// Token: 0x06004C02 RID: 19458 RVA: 0x001A7BB4 File Offset: 0x001A5FB4
	public FollowPhysicallySingle.ForcePosition getForcePosition(string joint)
	{
		FollowPhysicallySingle followPhysicallySingle;
		if (this.FPMap.TryGetValue(joint, out followPhysicallySingle))
		{
			return followPhysicallySingle.forcePosition;
		}
		return FollowPhysicallySingle.ForcePosition.HingePoint;
	}

	// Token: 0x06004C03 RID: 19459 RVA: 0x001A7BDC File Offset: 0x001A5FDC
	public void setForcePosition(string joint, FollowPhysicallySingle.ForcePosition val)
	{
		FollowPhysicallySingle followPhysicallySingle;
		if (this.FPMap.TryGetValue(joint, out followPhysicallySingle))
		{
			followPhysicallySingle.forcePosition = val;
		}
	}

	// Token: 0x06004C04 RID: 19460 RVA: 0x001A7C04 File Offset: 0x001A6004
	public bool getUseGlobalForceMultiplier(string joint)
	{
		FollowPhysicallySingle followPhysicallySingle;
		return this.FPMap.TryGetValue(joint, out followPhysicallySingle) && followPhysicallySingle.useGlobalForceMultiplier;
	}

	// Token: 0x06004C05 RID: 19461 RVA: 0x001A7C2C File Offset: 0x001A602C
	public void setUseGlobalForceMultiplier(string joint, bool val)
	{
		FollowPhysicallySingle followPhysicallySingle;
		if (this.FPMap.TryGetValue(joint, out followPhysicallySingle))
		{
			followPhysicallySingle.useGlobalForceMultiplier = val;
		}
	}

	// Token: 0x06004C06 RID: 19462 RVA: 0x001A7C54 File Offset: 0x001A6054
	public float getForceMultiplier(string joint)
	{
		FollowPhysicallySingle followPhysicallySingle;
		if (this.FPMap.TryGetValue(joint, out followPhysicallySingle))
		{
			return followPhysicallySingle.ForceMultiplier;
		}
		return 0f;
	}

	// Token: 0x06004C07 RID: 19463 RVA: 0x001A7C80 File Offset: 0x001A6080
	public void setForceMultiplier(string joint, float val)
	{
		FollowPhysicallySingle followPhysicallySingle;
		if (this.FPMap.TryGetValue(joint, out followPhysicallySingle))
		{
			followPhysicallySingle.ForceMultiplier = val;
		}
	}

	// Token: 0x06004C08 RID: 19464 RVA: 0x001A7CA8 File Offset: 0x001A60A8
	public bool getUseGlobalTorqueMultiplier(string joint)
	{
		FollowPhysicallySingle followPhysicallySingle;
		return this.FPMap.TryGetValue(joint, out followPhysicallySingle) && followPhysicallySingle.useGlobalTorqueMultiplier;
	}

	// Token: 0x06004C09 RID: 19465 RVA: 0x001A7CD0 File Offset: 0x001A60D0
	public void setUseGlobalTorqueMultiplier(string joint, bool val)
	{
		FollowPhysicallySingle followPhysicallySingle;
		if (this.FPMap.TryGetValue(joint, out followPhysicallySingle))
		{
			followPhysicallySingle.useGlobalTorqueMultiplier = val;
		}
	}

	// Token: 0x06004C0A RID: 19466 RVA: 0x001A7CF8 File Offset: 0x001A60F8
	public float getTorqueMultiplier(string joint)
	{
		FollowPhysicallySingle followPhysicallySingle;
		if (this.FPMap.TryGetValue(joint, out followPhysicallySingle))
		{
			return followPhysicallySingle.TorqueMultiplier;
		}
		return 0f;
	}

	// Token: 0x06004C0B RID: 19467 RVA: 0x001A7D24 File Offset: 0x001A6124
	public void setTorqueMultiplier(string joint, float val)
	{
		FollowPhysicallySingle followPhysicallySingle;
		if (this.FPMap.TryGetValue(joint, out followPhysicallySingle))
		{
			followPhysicallySingle.TorqueMultiplier = val;
		}
	}

	// Token: 0x06004C0C RID: 19468 RVA: 0x001A7D4C File Offset: 0x001A614C
	public float getTorqueMultiplier2(string joint)
	{
		FollowPhysicallySingle followPhysicallySingle;
		if (this.FPMap.TryGetValue(joint, out followPhysicallySingle))
		{
			return followPhysicallySingle.TorqueMultiplier2;
		}
		return 0f;
	}

	// Token: 0x06004C0D RID: 19469 RVA: 0x001A7D78 File Offset: 0x001A6178
	public void setTorqueMultiplier2(string joint, float val)
	{
		FollowPhysicallySingle followPhysicallySingle;
		if (this.FPMap.TryGetValue(joint, out followPhysicallySingle))
		{
			followPhysicallySingle.TorqueMultiplier2 = val;
		}
	}

	// Token: 0x06004C0E RID: 19470 RVA: 0x001A7DA0 File Offset: 0x001A61A0
	public float getMaxForce(string joint)
	{
		FollowPhysicallySingle followPhysicallySingle;
		if (this.FPMap.TryGetValue(joint, out followPhysicallySingle))
		{
			return followPhysicallySingle.MaxForce;
		}
		return 0f;
	}

	// Token: 0x06004C0F RID: 19471 RVA: 0x001A7DCC File Offset: 0x001A61CC
	public void setMaxForce(string joint, float val)
	{
		FollowPhysicallySingle followPhysicallySingle;
		if (this.FPMap.TryGetValue(joint, out followPhysicallySingle))
		{
			followPhysicallySingle.MaxForce = val;
		}
	}

	// Token: 0x06004C10 RID: 19472 RVA: 0x001A7DF4 File Offset: 0x001A61F4
	public float getMaxTorque(string joint)
	{
		FollowPhysicallySingle followPhysicallySingle;
		if (this.FPMap.TryGetValue(joint, out followPhysicallySingle))
		{
			return followPhysicallySingle.MaxTorque;
		}
		return 0f;
	}

	// Token: 0x06004C11 RID: 19473 RVA: 0x001A7E20 File Offset: 0x001A6220
	public void setMaxTorque(string joint, float val)
	{
		FollowPhysicallySingle followPhysicallySingle;
		if (this.FPMap.TryGetValue(joint, out followPhysicallySingle))
		{
			followPhysicallySingle.MaxTorque = val;
		}
	}

	// Token: 0x06004C12 RID: 19474 RVA: 0x001A7E48 File Offset: 0x001A6248
	public void init()
	{
		this.TransformMap = new Dictionary<string, Transform>();
		Transform[] componentsInChildren = base.GetComponentsInChildren<Transform>();
		foreach (Transform transform in componentsInChildren)
		{
			if (!this.TransformMap.ContainsKey(transform.name))
			{
				this.TransformMap.Add(transform.name, transform);
			}
		}
		this.RBMap = new Dictionary<string, Rigidbody>();
		Rigidbody[] componentsInChildren2 = base.GetComponentsInChildren<Rigidbody>();
		foreach (Rigidbody rigidbody in componentsInChildren2)
		{
			this.RBMap.Add(rigidbody.name, rigidbody);
		}
		this.CGMap = new Dictionary<string, SetCenterOfGravity>();
		SetCenterOfGravity[] componentsInChildren3 = base.GetComponentsInChildren<SetCenterOfGravity>();
		foreach (SetCenterOfGravity setCenterOfGravity in componentsInChildren3)
		{
			this.CGMap.Add(setCenterOfGravity.name, setCenterOfGravity);
		}
		this.FPMap = new Dictionary<string, FollowPhysicallySingle>();
		FollowPhysicallySingle[] componentsInChildren4 = base.GetComponentsInChildren<FollowPhysicallySingle>();
		foreach (FollowPhysicallySingle followPhysicallySingle in componentsInChildren4)
		{
			this.FPMap.Add(followPhysicallySingle.name, followPhysicallySingle);
		}
		this.JointMap = new Dictionary<string, ConfigurableJoint>();
		ConfigurableJoint[] componentsInChildren5 = base.GetComponentsInChildren<ConfigurableJoint>();
		foreach (ConfigurableJoint configurableJoint in componentsInChildren5)
		{
			if (!this.JointMap.ContainsKey(configurableJoint.name))
			{
				this.JointMap.Add(configurableJoint.name, configurableJoint);
			}
		}
		this.FollowMap = new Dictionary<string, Transform>();
		if (this.followTree)
		{
			foreach (Transform transform2 in this.followTree.GetComponentsInChildren<Transform>())
			{
				this.FollowMap.Add(transform2.name, transform2);
			}
		}
		this.TargetsMap = new Dictionary<string, FreeControllerV3>();
		if (this.targets)
		{
			foreach (FreeControllerV3 freeControllerV in this.targets.GetComponentsInChildren<FreeControllerV3>())
			{
				this.TargetsMap.Add(freeControllerV.name, freeControllerV);
			}
		}
	}

	// Token: 0x04003A8D RID: 14989
	public DAZPhysicsCharacter copyFrom;

	// Token: 0x04003A8E RID: 14990
	public Transform followTree;

	// Token: 0x04003A8F RID: 14991
	public Transform targets;

	// Token: 0x04003A90 RID: 14992
	public string layerPrefix = "1";

	// Token: 0x04003A91 RID: 14993
	private Dictionary<string, Transform> TransformMap;

	// Token: 0x04003A92 RID: 14994
	private Dictionary<string, Rigidbody> RBMap;

	// Token: 0x04003A93 RID: 14995
	private Dictionary<string, SetCenterOfGravity> CGMap;

	// Token: 0x04003A94 RID: 14996
	private Dictionary<string, FollowPhysicallySingle> FPMap;

	// Token: 0x04003A95 RID: 14997
	private Dictionary<string, Transform> FollowMap;

	// Token: 0x04003A96 RID: 14998
	private Dictionary<string, FreeControllerV3> TargetsMap;

	// Token: 0x04003A97 RID: 14999
	private Dictionary<string, ConfigurableJoint> JointMap;
}
