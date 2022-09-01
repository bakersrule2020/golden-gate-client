using System;
using Il2CppSystem.Reflection;
using Il2CppSystem.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnhollowerBaseLib.Attributes;
using UnityEngine;
using UnhollowerRuntimeLib.XrefScans;

namespace emmVRC.Libraries
{
    public static class PopupManagerUtils
    {
        // Special thanks to Knah for xref scanning, as well as UiExpansionKit
        public static void HideCurrentPopup(this VRCUiPopupManager vrcUiPopupManager)
        {
            VRCUiManager.prop_VRCUiManager_0.HideScreen("POPUP"); // Old code from build 864
        }

        public static void ShowStandardPopup(this VRCUiPopupManager vrcUiPopupManager, string title, string content, Action<VRCUiPopup> onCreated = null) =>
            vrcUiPopupManager.Method_Public_Void_String_String_Action_1_VRCUiPopup_0(title, content, onCreated);

        public static void ShowStandardPopup(this VRCUiPopupManager vrcUiPopupManager, string title, string content, string buttonText, Action buttonAction, Action<VRCUiPopup> onCreated = null) =>
            vrcUiPopupManager.Method_Public_Void_String_String_String_Action_Action_1_VRCUiPopup_1(title, content, buttonText, buttonAction, onCreated);

        public static void ShowStandardPopup(this VRCUiPopupManager vrcUiPopupManager, string title, string content, string button1Text, Action button1Action, string button2Text, Action button2Action, Action<VRCUiPopup> onCreated = null) =>
            vrcUiPopupManager.Method_Public_Void_String_String_String_Action_String_Action_Action_1_VRCUiPopup_0(title, content, button1Text, button1Action, button2Text, button2Action, onCreated);

        public static void ShowStandardPopupV2(this VRCUiPopupManager vrcUiPopupManager, string title, string content, string buttonText, Action buttonAction, Action<VRCUiPopup> onCreated = null)
        {
            vrcUiPopupManager.Method_Public_Void_String_String_String_Action_Action_1_VRCUiPopup_1(title, content, buttonText, buttonAction, onCreated);
        }

        public static void ShowStandardPopupV2(this VRCUiPopupManager vrcUiPopupManager, string title, string content, string button1Text, Action button1Action, string button2Text, Action button2Action, Action<VRCUiPopup> onCreated = null) =>
            vrcUiPopupManager.Method_Public_Void_String_String_String_Action_String_Action_Action_1_VRCUiPopup_1(title, content, button1Text, button1Action, button2Text, button2Action, onCreated);

        public static void ShowInputPopup(this VRCUiPopupManager vrcUiPopupManager, string title, string preFilledText, UnityEngine.UI.InputField.InputType inputType, bool keypad, string buttonText, Il2CppSystem.Action<string, List<KeyCode>, UnityEngine.UI.Text> buttonAction, Il2CppSystem.Action cancelAction, string boxText = "Enter text....", bool closeOnAccept = true, Action<VRCUiPopup> onCreated = null, bool startOnLeft = false, int characterLimit = 0) =>
            vrcUiPopupManager.Method_Public_Void_String_String_InputType_Boolean_String_Action_3_String_List_1_KeyCode_Text_Action_String_Boolean_Action_1_VRCUiPopup_Boolean_Int32_0(title, preFilledText, (TMPro.TMP_InputField.InputType)inputType, keypad, buttonText, buttonAction, cancelAction, boxText, closeOnAccept, onCreated, startOnLeft, characterLimit);
        public static void ShowAlert(this VRCUiPopupManager vrcUiPopupManager, string title, string content, float timeout) =>
            vrcUiPopupManager.Method_Public_Void_String_String_Single_0(title, content, timeout);

    }
}
