using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnhollowerRuntimeLib.XrefScans;
using UnityEngine;
using VRC.UI;

namespace emmVRC.Libraries
{
    public static class UIManagerUtils
    {
        public static void QueueHUDMessage(this VRCUiManager manager, string message)
        {
            manager.field_Private_List_1_String_0.Add(message);
        }

        public static VRCUiPage ShowScreen(this VRCUiManager Instance, VRCUiPage page)
        {
            return Instance.Method_Public_VRCUiPage_VRCUiPage_0(page);
        }
        public static void ShowScreen(this VRCUiManager Instance, string pageName)
        {
            VRCUiPage page = Instance.GetPage(pageName);
            if (page != null)
            {
                Instance.ShowScreen(page);
            }
        }

        public static VRCUiPage GetPage(this VRCUiManager Instance, string screenPath)
        {
            GameObject gameObject = GameObject.Find(screenPath);
            VRCUiPage vrcuiPage = null;
            if (gameObject != null)
            {
                vrcuiPage = gameObject.GetComponent<VRCUiPage>();
                if (vrcuiPage == null)
                {
                    Console.WriteLine("Screen Not Found - " + screenPath);
                }
            }
            else
            {
                Console.WriteLine("Screen Not Found - " + screenPath);
            }
            return vrcuiPage;
        }
    }
}
