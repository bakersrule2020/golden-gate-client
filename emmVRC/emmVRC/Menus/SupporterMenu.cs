using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using emmVRC.Utils;
using emmVRC.Objects;
using emmVRC.Objects.ModuleBases;
using UnityEngine;
using TMPro;
using UnityEngine.XR;

namespace emmVRC.Menus
{
    public class Supporter
    {
        public string name;
        public string color;
    }
    [Priority(55)]
    public class SupporterMenu : MelonLoaderEvents
    {
        private static MenuPage supporterPage;
        private static SingleButton supporterPageButton;
        private static TextMeshProUGUI text;

        private static bool _initialized = false;
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (buildIndex != -1 || _initialized) return;

            supporterPage = new MenuPage("emmVRC_Supporters", "Supporters", false, true);
            supporterPageButton = new SingleButton(FunctionsMenu.otherGroup, "Supporters", LoadMenu, "View all of the donators to the emmVRC project", Functions.Core.Resources.SupporterIcon);

            supporterPage.menuContents.GetComponent<UnityEngine.UI.VerticalLayoutGroup>().childControlHeight = true;
            GameObject textBase = new GameObject("SupportersText");

            textBase.transform.SetParent(supporterPage.menuContents);
            textBase.transform.localPosition = Vector3.zero;
            textBase.transform.localRotation = new Quaternion(0, 0, 0, 0);
            textBase.transform.localScale = Vector3.one;
            TextMeshProUGUI textText = textBase.AddComponent<TextMeshProUGUI>();
            textText.margin = new Vector4(25, 0, 50, 0);
            textText.text = "";

            text = textText;
            _initialized = true;
        }
        public static void LoadMenu()
        {
            try
            {
                supporterPage.OpenMenu();
                EnterMenu(text);
            }
            catch (Exception ex)
            {
                emmVRCLoader.Logger.LogError(ex.ToString());
            }
        }
        public static void EnterMenu(TextMeshProUGUI text)
        {
            text.text = "<size=50>Empty till fixxed WebRequest!</size>\n\n";
        }
    }
}
