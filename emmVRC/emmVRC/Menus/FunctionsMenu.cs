using emmVRC.Objects.ModuleBases;
using emmVRC.Utils;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace emmVRC.Menus
{
    [Priority(50)]
    public class FunctionsMenu : MelonLoaderEvents
    {
        public static MenuPage basePage;
        private static Tab mainTab;
        internal static ButtonGroup tweaksGroup;
        internal static ButtonGroup featuresGroup;
        internal static ButtonGroup otherGroup;

        private static bool _initialized = false;

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (buildIndex != -1 || _initialized) return;
            basePage = new Utils.MenuPage("emmVRC_MainMenu", "emmVRC", true, false, false, null, "<color=#FF69B4>emmVRC</color> version " + Objects.Attributes.Version, Functions.Core.Resources.onlineSprite, true);
            mainTab = new Utils.Tab(Utils.ButtonAPI.menuTabBase.transform.parent, "emmVRC_MainMenu", "emmVRC", Functions.Core.Resources.TabIcon, () => 
            {
                if (Configuration.JSONConfig.AcceptedEULAVersion != Objects.Attributes.EULAVersion)
                {
                    ButtonAPI.menuTabBase.transform.parent.Find("Page_Dashboard").GetComponent<Button>().onClick.Invoke();
                    ButtonAPI.GetQuickMenuInstance().ShowConfirmDialog("Welcome to emmVRC!", "emmVRC Ported to Quest. By XoX-Toxic", () =>
                    {
                        Configuration.WriteConfigOption("AcceptedEULAVersion", Objects.Attributes.EULAVersion);
                    }, () =>
                    {
                        Configuration.WriteConfigOption("AcceptedEULAVersion", Objects.Attributes.EULAVersion);
                    });
                    return;
                }
                basePage.OpenMenu();
            });

            GameObject textBase = new GameObject("ChangelogText");
            textBase.transform.SetParent(basePage.menuContents);
            textBase.transform.localPosition = Vector3.zero;
            textBase.transform.localRotation = new Quaternion(0, 0, 0, 0);
            textBase.transform.localScale = Vector3.one;
            TextMeshProUGUI textText = textBase.AddComponent<TextMeshProUGUI>();
            textText.margin = new Vector4(25, 0, 50, 0);
            textText.text = "Version " + Objects.Attributes.Version.ToString(3);
            textText.alignment = TextAlignmentOptions.Left;

            Components.EnableDisableListener textListener = textBase.AddComponent<Components.EnableDisableListener>();
            textListener.OnEnabled += () =>
            {
                textText.text = "Version " +Objects.Attributes.Version.ToString(3);
            };

            tweaksGroup = new Utils.ButtonGroup(basePage.menuContents, "Tweaks");
            featuresGroup = new Utils.ButtonGroup(basePage.menuContents, "Features");
            otherGroup = new Utils.ButtonGroup(basePage.menuContents, "Other");
            basePage.menuContents.parent.parent.parent.GetChild(0).Find("RightItemContainer/Button_QM_Expand/Icon").GetComponent<UnityEngine.RectTransform>().sizeDelta = new UnityEngine.Vector2(72, 72);
            basePage.menuContents.parent.parent.parent.GetChild(0).Find("RightItemContainer/Button_QM_Expand/Icon").GetComponent<UnityEngine.RectTransform>().localPosition = new UnityEngine.Vector3(0f, 8f, 0f);
            _initialized = true;
        }
    }
}
