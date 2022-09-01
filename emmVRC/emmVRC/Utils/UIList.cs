using System;
using Il2CppSystem.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using VRC.Core;

namespace emmVRC.Utils
{
    public static class UIList
    {
        internal static void RenderElement(this UiVRCList uivrclist, List<ApiAvatar> AvatarList)
        {
            if (!uivrclist.gameObject.activeInHierarchy || !uivrclist.isActiveAndEnabled || uivrclist.isOffScreen || !uivrclist.enabled) return;
            uivrclist.Method_Protected_Void_List_1_T_Int32_Boolean_VRCUiContentButton_0(AvatarList, 0, true, null);
        }
        internal static void RenderElement(this UiVRCList uivrclist, List<string> idList)
        {
            if (!uivrclist.gameObject.activeInHierarchy || !uivrclist.isActiveAndEnabled || uivrclist.isOffScreen || !uivrclist.enabled) return;
            uivrclist.Method_Protected_Void_List_1_T_Int32_Boolean_VRCUiContentButton_0(idList, 0, true, null);
        }
    }
}
