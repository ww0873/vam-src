#region Header

/************************************************************************************************
Plugin name: FindPlugins 

Write by cjyhot

Version 2.0 2023-10-03
	
Thanks to JayJayWon 

************************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

#endregion


namespace CjyhotHelper
{
    public class FindPlugins : MVRScript
    {

        List<UIDynamicButton> dynamicButtons = new List<UIDynamicButton>();

        public void Start()
        {
            UIDynamicButton RefreshButton = GUICreateButton(this, "<<Refresh>>", new Color(0.78F, 0.78F, 0.899F), false, false, Refresh);
            UIDynamicButton SaveLookButton = GUICreateButton(this, "<<Occupy space>>", new Color(0.78F, 0.78F, 0.899F), false, true, null);

            ListAllPlugins();
        }

        private void Refresh()
        {
            foreach (var button in dynamicButtons)
            {
                RemoveButton(button);
            }
            ListAllPlugins();
        }

        private void ListAllPlugins()
        {
            dynamicButtons.Clear();
            foreach (var sceneAtom in GetSceneAtoms())
            {
                foreach (var storableID in sceneAtom.GetStorableIDs())
                {
                    try
                    {
                        if (storableID.Contains("plugin#"))
                        {
                            string name = storableID.Split('.')[1];
                            UIDynamicButton buttonLeft = GUICreateButton(this, $"Find {sceneAtom.ToString()}.{name}", new Color(0.78F, 0.78F, 0.899F), false, false, () => { SelectAndOpenUI(sceneAtom, storableID, false); });
                            UIDynamicButton buttonRight = GUICreateButton(this, $"Open {name} UI", new Color(0.78F, 0.78F, 0.899F), false, true, () => { SelectAndOpenUI(sceneAtom, storableID, true); });
                            dynamicButtons.Add(buttonLeft);
                            dynamicButtons.Add(buttonRight);
                        }
                    }
                    catch { }
                }
            }
        }

        public UIDynamicButton GUICreateButton(MVRScript dll, string label, Color _color, bool useColor, bool rightside, UnityAction callback)
        {
            UIDynamicButton gui = dll.CreateButton(label, rightside);
            if (useColor) { gui.buttonColor = _color; }
            gui.button.onClick.AddListener(callback);
            return gui;
        }


        public void SelectAndOpenUI(Atom atom, string pluginRef, bool openUI)
        {
            if (atom.name == "CoreControl" || atom.type == "SessionPluginManager")
            {
                SuperController.singleton.ShowMainHUDMonitor();
                SuperController.singleton.activeUI = SuperController.ActiveUI.MainMenu;
            }
            else
            {
                SuperController.singleton.SelectController(atom.mainController, false, false, true);
                SuperController.singleton.ShowMainHUDMonitor();
            }

            SuperController.singleton.StartCoroutine(WaitForUI(atom, pluginRef, openUI));

        }

        public void SelectAndOpenUI(Atom atom, int pluginSlot, bool openUI)
        {
            string pluginRef = "plugin#" + pluginSlot.ToString() + "_";
            SelectAndOpenUI(atom, pluginRef, openUI);
        }

        private IEnumerator WaitForUI(Atom atom, string pluginRef, bool openUI)
        {
            var expiration = Time.unscaledTime + 1f;
            UITabSelector selector;
            while (Time.unscaledTime < expiration)
            {
                yield return 0;
                if (atom.name == "CoreControl" || atom.type == "SessionPluginManager") selector = SuperController.singleton.mainMenuTabSelector;
                else selector = atom.gameObject.GetComponentInChildren<UITabSelector>();
                if (selector == null) continue;

                if (atom.type == "SessionPluginManager") SuperController.singleton.mainMenuTabSelector.SetActiveTab("TabSessionPlugins");
                else if (atom.name == "CoreControl") SuperController.singleton.mainMenuTabSelector.SetActiveTab("TabScenePlugins");
                else selector.SetActiveTab("Plugins");

                IEnumerator enumerator1 = selector.transform.GetEnumerator();

                while (enumerator1.MoveNext())
                {
                    UITab component = ((Component)enumerator1.Current).GetComponent<UITab>();

                    if ((UnityEngine.Object)component != (UnityEngine.Object)null)
                    {
                        foreach (var scriptUI in component.GetComponentsInChildren<MVRScriptUI>())
                        {
                            scriptUI.closeButton?.onClick.Invoke();
                        }
                        if (openUI)
                        {
                            foreach (var scriptController in component.GetComponentsInChildren<MVRScriptControllerUI>())
                            {
                                if (scriptController.label.text.StartsWith(pluginRef))
                                {
                                    scriptController.openUIButton?.onClick?.Invoke();
                                    break;
                                }
                            }
                        }
                    }
                }
                yield break;
            }
        }
    }
}
