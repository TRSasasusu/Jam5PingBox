using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jam5PingBox {
    [HarmonyPatch]
    public static class Patch {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(OWItemSocket), nameof(OWItemSocket.PlaceIntoSocket))]
        public static void OWItemSocket_PlaceIntoSocket_Postfix(OWItemSocket __instance, OWItem item) {
            var dioramaMachine = __instance.GetComponentInParent<DioramaMachine>();
            if(!dioramaMachine) {
                return;
            }

            //Jam5PingBox.Log(item.name);
            if(item.name == "Box1Item") {
                dioramaMachine.Load(DioramaMachine.BoxType.BOX1);
            }
        }
    }
}
