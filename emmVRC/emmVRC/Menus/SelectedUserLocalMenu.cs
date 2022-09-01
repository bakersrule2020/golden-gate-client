using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Utils;
using emmVRC.Components;
using emmVRC.Objects.ModuleBases;
using UnityEngine;
using UnityEngine.UI;
using VRC.DataModel;
using VRC.DataModel.Core;
using VRC.Core;
using VRC;
using VRC.UI.Elements.Menus;

namespace emmVRC.Menus
{
    [Priority(55)]
    public class SelectedUserLocalMenu : MelonLoaderEvents
    {
        private static ButtonGroup selectedUserGroup;
        private static SimpleSingleButton avatarOptionsButton;
        private static SimpleSingleButton playerNotesButton;
        private static SimpleSingleButton teleportButton;
        private static SimpleSingleButton favouriteAvatarButton;

        private static bool _initialized = false;
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (buildIndex != -1 || _initialized) return;
            Transform baseMenuTransform = ButtonAPI.menuPageBase.transform.parent.Find("Menu_SelectedUser_Local/ScrollRect/Viewport/VerticalLayoutGroup");
            selectedUserGroup = new ButtonGroup(baseMenuTransform, "emmVRC Actions");
            avatarOptionsButton = new SimpleSingleButton(selectedUserGroup, "Avatar\nOptions", () =>
            {
                AvatarOptionsMenu.OpenMenu(IUserExtension.GetVRCPlayer());
            }, "Shows various options for the selected avatar, including toggling components and global dynamic bones");
            playerNotesButton = new SimpleSingleButton(selectedUserGroup, "Player\nNotes", () =>
            {
                APIUser selectedUser = IUserExtension.GetAPIUser();
                Functions.UI.PlayerNotes.LoadNoteQM(selectedUser.id, selectedUser.GetName());
            }, "View the notes for the selected player");
            teleportButton = new SimpleSingleButton(selectedUserGroup, "Teleport", () =>
            {
                if (Configuration.JSONConfig.RiskyFunctionsEnabled)
                    VRCPlayer.field_Internal_Static_VRCPlayer_0.field_Private_VRCPlayerApi_0.TeleportTo(IUserExtension.GetVRCPlayer().transform.position, IUserExtension.GetVRCPlayer().transform.rotation);
            }, "Teleport to the selected player (requires Risky Functions to be enabled and allowed)");

            EnableDisableListener listener = teleportButton.gameObject.AddComponent<EnableDisableListener>();
            listener.OnEnabled += () =>
            {
                teleportButton.SetInteractable(Configuration.JSONConfig.RiskyFunctionsEnabled);
            };
            favouriteAvatarButton = new SimpleSingleButton(selectedUserGroup, "Favorite\nAvatar", () => {
                bool flag = false;
                APIUser selectedAPIUser = IUserExtension.GetAPIUser();
                VRCPlayer selectedVRCPlayer = IUserExtension.GetVRCPlayer();
                ApiAvatar selectedApiAvatar = IUserExtension.GetApiAvatar();
                if (selectedVRCPlayer == null) return;

                if (selectedAPIUser.allowAvatarCopying && selectedVRCPlayer._player.prop_ApiAvatar_0.releaseStatus == "public")
                {
                    foreach (ApiAvatar avtr in Functions.UI.CustomAvatarFavorites.LoadedAvatars)
                    {
                        if (avtr.id == selectedVRCPlayer._player.prop_ApiAvatar_0.id)
                            flag = true;
                    }
                    if (flag)
                        ButtonAPI.GetQuickMenuInstance().ShowOKDialog("emmVRC", "You already have this avatar favorited");
                    else
                        Functions.UI.CustomAvatarFavorites.FavoriteAvatar(selectedVRCPlayer._player.prop_ApiAvatar_0).NoAwait(nameof(Functions.UI.CustomAvatarFavorites.FavoriteAvatar));
                }
                else
                    ButtonAPI.GetQuickMenuInstance().ShowOKDialog("emmVRC", "This avatar is not public, or the user does not have cloning turned on.");
            }, "Favorite this user's avatar to your emmVRC Favorite list");
            _initialized = true;
        }
    }
}
