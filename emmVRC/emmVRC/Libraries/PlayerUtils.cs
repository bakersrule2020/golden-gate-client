using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRC;
using VRC.SDKBase;
namespace emmVRC.Libraries
{
    public class PlayerUtils
    {
        public static void GetEachPlayer(System.Action<Player> act)
        {
            foreach (Player plr in PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0)
            {
                act.Invoke(plr);
            }
        }
        
    }
}
