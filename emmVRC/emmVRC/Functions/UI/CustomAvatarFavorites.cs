using System;
using Il2CppSystem.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using VRC.Core;
using VRC.UI;
using emmVRC.Utils;
using emmVRC.Libraries;
using System.Linq;
using emmVRC.Objects;
using System.Reflection;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using emmVRC.Objects.ModuleBases;
using emmVRC.TinyJSON;
using MelonLoader;
using UnhollowerBaseLib;
using UnhollowerRuntimeLib.XrefScans;
using VRC;
using Logger = emmVRCLoader.Logger;

namespace emmVRC.Functions.UI
{
    public class CustomAvatarFavorites : MelonLoaderEvents, IWithUpdate
    {
        public enum SortingMode
        {
            DateAdded = 0,
            Alphabetical = 1,
            Creator = 2
        }
        internal static GameObject PublicAvatarList;
        internal static UiAvatarList NewAvatarList;
        private static GameObject avText;
        private static Text avTextText;
        private static GameObject ChangeButton;
        public static Button.ButtonClickedEvent baseChooseEvent;
        private static GameObject FavoriteButton;
        private static GameObject FavoriteButtonNew;
        private static Button FavoriteButtonNewButton;
        private static Text FavoriteButtonNewText;
        //private static GameObject ShowAuthorButton;
        public static GameObject pageAvatar;
        private static PageAvatar currPageAvatar;
        private static bool error = false;
        private static bool errorWarned;
        public static List<ApiAvatar> LoadedAvatars;
        private static GameObject refreshButton;
        private static GameObject backButton;
        private static GameObject forwardButton;
        private static GameObject pageTicker;
        private static GameObject sortButton;
        public static int currentPage = 0;
        private static SortingMode currentSortingMode = SortingMode.DateAdded;
        private static bool sortingInverse = false; // False = First-to-Last, True = Last-to-First
        

        public override void OnUiManagerInit()
        {
            
            if (Configuration.JSONConfig.SortingMode <= 2)
                currentSortingMode = (SortingMode)Configuration.JSONConfig.SortingMode;
            else
                currentSortingMode = 0;
            sortingInverse = Configuration.JSONConfig.SortingInverse;

            VRC.UI.PageAvatar pageAvatarComp = UnityEngine.Resources.FindObjectsOfTypeAll<VRC.UI.PageAvatar>().FirstOrDefault();

            pageAvatar = pageAvatarComp.gameObject;
            FavoriteButton = pageAvatarComp.transform.Find("Favorite Button").gameObject;
            FavoriteButtonNew = UnityEngine.Object.Instantiate<GameObject>(FavoriteButton, pageAvatarComp.transform);
            FavoriteButtonNewButton = FavoriteButtonNew.GetComponent<Button>();
            FavoriteButtonNewButton.onClick.RemoveAllListeners();
            FavoriteButtonNewButton.onClick.AddListener(new System.Action(() =>
            {

                ApiAvatar apiAvatar = pageAvatar.GetComponent<PageAvatar>().field_Public_SimpleAvatarPedestal_0.field_Internal_ApiAvatar_0;
                bool flag = false;
                foreach (var avatar in LoadedAvatars)
                    if (avatar.id == apiAvatar.id)
                        flag = true;
                if (!flag)
                {

                    if (((apiAvatar.releaseStatus == "public" || apiAvatar.authorId == APIUser.CurrentUser.id) && apiAvatar.releaseStatus != null))
                    {
                        FavoriteAvatar(apiAvatar).NoAwait(nameof(FavoriteAvatar));
                    }
                    else
                    {
                        VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", Core.Localization.currentLanguage.FavouriteFailedPrivateMessage, "Dismiss", new System.Action(() => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }));
                    }
                }
                else
                {
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "Are you sure you want to unfavorite the avatar \"" + apiAvatar.name + "\"?", "Yes", new System.Action(() =>
                    {
                        UnfavoriteAvatar(apiAvatar).NoAwait(nameof(UnfavoriteAvatar));
                        VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup();
                    }), "No", () =>
                    {
                        VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup();
                    });
                }
            }));

            FavoriteButtonNew.GetComponentInChildren<RectTransform>().localPosition += new Vector3(0, 165f);
            FavoriteButtonNewText = FavoriteButtonNew.GetComponentInChildren<Text>();
            FavoriteButtonNewText.supportRichText = true;

            FavoriteButtonNew.transform.Find("Horizontal").GetComponentsInChildren<Transform>(true).ToList().ForEach(a =>
            {
                if (a.name != "FavoriteActionText" && a.name != "Horizontal")
                    a.gameObject.SetActive(false);
            });

            var oldPublicAvatarList = pageAvatarComp.transform.Find("Vertical Scroll View/Viewport/Content/Legacy Avatar List").gameObject;
            oldPublicAvatarList = pageAvatarComp.transform.Find("Vertical Scroll View/Viewport/Content/Legacy Avatar List").gameObject;
            PublicAvatarList = GameObject.Instantiate(oldPublicAvatarList, oldPublicAvatarList.transform.parent);
            PublicAvatarList.transform.SetAsFirstSibling();

            ChangeButton = pageAvatarComp.transform.Find("Change Button").gameObject;
            baseChooseEvent = ChangeButton.GetComponent<Button>().onClick;
            ChangeButton.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            ChangeButton.GetComponent<Button>().onClick.AddListener(new System.Action(() =>
            {
                ApiAvatar selectedAvatar = pageAvatar.GetComponent<PageAvatar>().field_Public_SimpleAvatarPedestal_0.field_Internal_ApiAvatar_0;
                if (LoadedAvatars.ToArray().Any(a => a.id == selectedAvatar.id))
                {
                    //emmVRCLoader.Bootstrapper.Instance.StartCoroutine(CheckAvatar());
                    if (selectedAvatar.releaseStatus == "private" && selectedAvatar.authorId != APIUser.CurrentUser.id)
                    {
                        if (LoadedAvatars.ToArray().Any(a => a.id == selectedAvatar.id))
                            VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", Core.Localization.currentLanguage.AvatarSwitchPrivateUnfavouriteMessage, "Yes", new System.Action(() => { UnfavoriteAvatar(selectedAvatar).NoAwait(nameof(UnfavoriteAvatar)); VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }), "No", new System.Action(() => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }));
                        else
                            VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", Core.Localization.currentLanguage.AvatarSwitchPrivateMessage, "Dismiss", new System.Action(() => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }));
                    }
                    else if (selectedAvatar.releaseStatus == "unavailable")
                    {
                        if (LoadedAvatars.ToArray().Any(a => a.id == selectedAvatar.id))
                            VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", Core.Localization.currentLanguage.AvatarSwitchDeletedUnfavouriteMessage, "Yes", new System.Action(() => { UnfavoriteAvatar(selectedAvatar).NoAwait(nameof(UnfavoriteAvatar)); VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }), "No", new System.Action(() => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }));
                        else
                            VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", Core.Localization.currentLanguage.AvatarSwitchDeletedMessage, "Dismiss", new System.Action(() => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }));
                    }
                    else
                        baseChooseEvent.Invoke();
                }
                else
                {
                    baseChooseEvent.Invoke();
                }
            }));

            avText = PublicAvatarList.transform.Find("Button").gameObject;
            avTextText = avText.GetComponentInChildren<Text>();
            avTextText.text = "(0) emmVRC Favorites";


            currPageAvatar = pageAvatar.GetComponent<PageAvatar>();
            NewAvatarList = PublicAvatarList.GetComponent<UiAvatarList>();
            NewAvatarList.clearUnseenListOnCollapse = false;
            NewAvatarList.field_Public_EnumNPublicSealedvaInPuMiFaSpClPuLi11Unique_0 = UiAvatarList.EnumNPublicSealedvaInPuMiFaSpClPuLi11Unique.SpecificList;

            currPageAvatar.field_Public_SimpleAvatarPedestal_0.field_Public_Single_0 *= 0.85f;


            refreshButton = GameObject.Instantiate(ChangeButton, avText.transform.parent);
            refreshButton.GetComponentInChildren<Text>().text = "↻";
            refreshButton.GetComponent<Button>().onClick.RemoveAllListeners();
            refreshButton.GetComponent<Button>().onClick.AddListener(new System.Action(() =>
            {
                MelonLoader.MelonCoroutines.Start(JumpToStart());
            }));
            refreshButton.GetComponent<RectTransform>().sizeDelta /= new Vector2(4f, 1f);
            refreshButton.transform.SetParent(avText.transform, true);
            refreshButton.GetComponent<RectTransform>().anchoredPosition = avText.transform.Find("ToggleIcon").GetComponent<RectTransform>().anchoredPosition + new Vector2(980f, 0f);

            backButton = GameObject.Instantiate(ChangeButton, avText.transform.parent);
            backButton.GetComponentInChildren<Text>().text = "←";
            backButton.GetComponent<Button>().onClick.RemoveAllListeners();
            backButton.GetComponent<Button>().onClick.AddListener(new System.Action(() =>
            {
                currentPage--;
                MelonLoader.MelonCoroutines.Start(JumpToStart());
            }));
            backButton.GetComponent<RectTransform>().sizeDelta /= new Vector2(4f, 1f);
            backButton.transform.SetParent(avText.transform, true);
            backButton.GetComponent<RectTransform>().anchoredPosition = avText.transform.Find("ToggleIcon").GetComponent<RectTransform>().anchoredPosition + new Vector2(750f, 0f);

            forwardButton = GameObject.Instantiate(ChangeButton, avText.transform.parent);
            forwardButton.GetComponentInChildren<Text>().text = "→";
            forwardButton.GetComponent<Button>().onClick.RemoveAllListeners();
            forwardButton.GetComponent<Button>().onClick.AddListener(new System.Action(() =>
            {
                currentPage++;
                MelonLoader.MelonCoroutines.Start(JumpToStart());
            }));
            forwardButton.GetComponent<RectTransform>().sizeDelta /= new Vector2(4f, 1f);
            forwardButton.transform.SetParent(avText.transform, true);
            forwardButton.GetComponent<RectTransform>().anchoredPosition = avText.transform.Find("ToggleIcon").GetComponent<RectTransform>().anchoredPosition + new Vector2(900f, 0f);

            pageTicker = GameObject.Instantiate(ChangeButton, avText.transform.parent);
            pageTicker.GetComponentInChildren<Text>().text = "0 / 0";
            GameObject.Destroy(pageTicker.GetComponent<Button>());
            GameObject.Destroy(pageTicker.GetComponent<Image>());
            pageTicker.GetComponent<RectTransform>().sizeDelta /= new Vector2(4f, 1f);
            pageTicker.transform.SetParent(avText.transform, true);
            pageTicker.GetComponent<RectTransform>().anchoredPosition = avText.transform.Find("ToggleIcon").GetComponent<RectTransform>().anchoredPosition + new Vector2(825f, 0f);

            sortButton = GameObject.Instantiate(ChangeButton, avText.transform.parent);
            switch (currentSortingMode)
            {
                case SortingMode.DateAdded:
                    sortButton.GetComponentInChildren<Text>().text = "Date " + (sortingInverse ? "↑" : "↓");
                    break;
                case SortingMode.Alphabetical:
                    sortButton.GetComponentInChildren<Text>().text = "ABC " + (sortingInverse ? "↑" : "↓");
                    break;
                case SortingMode.Creator:
                    sortButton.GetComponentInChildren<Text>().text = "Creator " + (sortingInverse ? "↑" : "↓");
                    break;
            }
            sortButton.GetComponent<Button>().onClick.RemoveAllListeners();
            sortButton.GetComponent<Button>().onClick.AddListener(new System.Action(() =>
            {
                if (!sortingInverse)
                    sortingInverse = true;
                else
                {
                    if (currentSortingMode != SortingMode.Creator)
                        currentSortingMode++;
                    else
                        currentSortingMode = SortingMode.DateAdded;
                    sortingInverse = false;
                }
                switch (currentSortingMode)
                {
                    case SortingMode.DateAdded:
                        sortButton.GetComponentInChildren<Text>().text = "Date " + (sortingInverse ? "↑" : "↓");
                        break;
                    case SortingMode.Alphabetical:
                        sortButton.GetComponentInChildren<Text>().text = "ABC " + (sortingInverse ? "↑" : "↓");
                        break;
                    case SortingMode.Creator:
                        sortButton.GetComponentInChildren<Text>().text = "Creator " + (sortingInverse ? "↑" : "↓");
                        break;
                }
                currentPage = 0;
                Configuration.WriteConfigOption("SortingMode", (int)currentSortingMode);
                Configuration.WriteConfigOption("SortingInverse", sortingInverse);
                MelonLoader.MelonCoroutines.Start(JumpToStart());
            }));
            sortButton.GetComponent<RectTransform>().sizeDelta /= new Vector2(2f, 1f);
            sortButton.transform.SetParent(avText.transform, true);
            sortButton.GetComponent<RectTransform>().anchoredPosition = avText.transform.Find("ToggleIcon").GetComponent<RectTransform>().anchoredPosition + new Vector2(635f, 0f);

            pageAvatar.transform.Find("AvatarPreviewBase").transform.localPosition += new Vector3(0f, 60f, 0f);
            pageAvatar.transform.Find("AvatarPreviewBase").transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
            foreach (PropertyInfo inf in typeof(PageAvatar).GetProperties().Where(a => a.PropertyType == typeof(Vector3) && ((Vector3)a.GetValue(currPageAvatar)).x <= -461f && ((int)((Vector3)a.GetValue(currPageAvatar)).y) == -200))
            {
                Vector3 position = ((Vector3)inf.GetValue(currPageAvatar));
                inf.SetValue(currPageAvatar, new Vector3(position.x, position.y + 80f, position.z));
            }
            foreach (PropertyInfo inf in typeof(PageAvatar).GetProperties().Where(a => a.PropertyType == typeof(Vector3) && ((int)((Vector3)a.GetValue(currPageAvatar)).x) == -91))
            {
                Vector3 position = ((Vector3)inf.GetValue(currPageAvatar));
                inf.SetValue(currPageAvatar, new Vector3(position.x, position.y + 80f, position.z));
            }

            LoadedAvatars = new List<ApiAvatar>();

            Components.EnableDisableListener pageAvatarListener = pageAvatar.AddComponent<Components.EnableDisableListener>();
            pageAvatarListener.OnEnabled += () =>
            {
                if ((!Configuration.JSONConfig.AvatarFavoritesEnabled || !Configuration.JSONConfig.emmVRCNetworkEnabled) && (PublicAvatarList.activeSelf || FavoriteButtonNew.activeSelf))
                {
                    PublicAvatarList.SetActive(false);
                    FavoriteButtonNew.SetActive(false);
                }
                else if ((!PublicAvatarList.activeSelf || !FavoriteButtonNew.activeSelf) && Configuration.JSONConfig.AvatarFavoritesEnabled && Configuration.JSONConfig.emmVRCNetworkEnabled)
                {
                    PublicAvatarList.SetActive(true);
                    FavoriteButtonNew.SetActive(true);
                }
                if (error && !errorWarned)
                {
                    Managers.emmVRCNotificationsManager.AddNotification(new Notification("emmVRC Network", Functions.Core.Resources.errorSprite, Core.Localization.currentLanguage.FavouriteListLoadErrorMessage, true, false, null, "", "", true, null, "Dismiss"));
                    errorWarned = true;
                }
            };
        }

        public static async Task FavoriteAvatar(ApiAvatar apiAvatar)
        {
        }
        public static async Task UnfavoriteAvatar(ApiAvatar apiAvatar)
        {
            
        }
        public static async Task PopulateList()
        {
            
        }
        public static IEnumerator JumpToStart()
        {
            if (Configuration.JSONConfig.AvatarFavoritesJumpToStart)
            {
                while (NewAvatarList.scrollRect.normalizedPosition.x > 0)
                {
                    NewAvatarList.scrollRect.normalizedPosition = new Vector2(NewAvatarList.scrollRect.normalizedPosition.x - 0.1f, 0);
                    yield return new WaitForEndOfFrame();
                }
            }
        }
        
        public void OnUpdate()
        {

            if (PublicAvatarList == null || !PublicAvatarList.activeInHierarchy) return;

            if (Configuration.JSONConfig.AvatarFavoritesEnabled && Configuration.JSONConfig.emmVRCNetworkEnabled)
            {
                NewAvatarList.collapsedCount = Configuration.JSONConfig.FavoriteRenderLimit + Configuration.JSONConfig.SearchRenderLimit;
                NewAvatarList.expandedCount = Configuration.JSONConfig.FavoriteRenderLimit + Configuration.JSONConfig.SearchRenderLimit;

                if (currPageAvatar != null && currPageAvatar.field_Public_SimpleAvatarPedestal_0 != null && currPageAvatar.field_Public_SimpleAvatarPedestal_0.field_Internal_ApiAvatar_0 != null && LoadedAvatars != null && FavoriteButtonNew != null)
                {
                    FavoriteButtonNewText.text = "<color=#FF69B4>emmVRC</color> Favorite";
                    foreach (ApiAvatar avatar in LoadedAvatars)
                    {
                        if (avatar.id == currPageAvatar.field_Public_SimpleAvatarPedestal_0.field_Internal_ApiAvatar_0.id)
                        {
                            FavoriteButtonNewText.text = "<color=#FF69B4>emmVRC</color> Unfavorite";
                            break;
                        }
                    }
                }
            }

        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (buildIndex != -1) return;
            CheckForAvatarPedestals().NoAwait();
        }

        public static async Task CheckForAvatarPedestals()
        {
            if (!Configuration.JSONConfig.emmVRCNetworkEnabled)
                return;
           
            if (!Configuration.JSONConfig.SubmitAvatarPedestals)
                return;
            
            while (RoomManager.field_Internal_Static_ApiWorld_0 == null)
                await Main.AwaitUpdate.Yield();
            
            var currentWorld = RoomManager.field_Internal_Static_ApiWorld_0;
            if (!currentWorld.IsPublicPublishedWorld)
                return;

            foreach (var pedestal in Resources.FindObjectsOfTypeAll<AvatarPedestal>())
            {
                if (pedestal == null)
                    continue;
                
                await Main.AwaitUpdate.Yield();
                var apiAvatar = pedestal.field_Private_ApiAvatar_0;
                if (apiAvatar == null)
                    continue;

                if (apiAvatar.releaseStatus != "public")
                    continue;

                if (string.IsNullOrEmpty(apiAvatar.assetUrl))
                    continue;
                
                Logger.LogDebug($"Found pedestal {apiAvatar.name}, putting...");
                await Task.Delay(500);
            }
        }
        
        public static async Task ExportAvatars()
        {
           
        }
    }
}
