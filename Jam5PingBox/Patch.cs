using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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
            else if(item.name == "Box2Item") {
                dioramaMachine.Load(DioramaMachine.BoxType.BOX2);
            }
            else if(item.name == "Box3Item") {
                dioramaMachine.Load(DioramaMachine.BoxType.BOX3);
            }
            else if(item.name == "BoxTriStarItem") {
                dioramaMachine.Load(DioramaMachine.BoxType.BOX_TRISTAR);
            }
        }

        //[HarmonyPrefix]
        //[HarmonyPatch(typeof(MapController), nameof(MapController.MapInoperable))]
        //public static bool MapController_MapInoperable_Prefix(MapController __instance, ref bool __result) {
        //    if(DioramaMachine.IsMapRestricted) {
        //        __instance._playerMapRestricted = true;
        //        __result = true;
        //        return false;
        //    }
        //    return true;
        //}

        [HarmonyPostfix]
        [HarmonyPatch(typeof(DeathManager), nameof(DeathManager.KillPlayer))]
        public static void DeathManager_KillPlayer_Postfix(DeathType deathType) {
            if(deathType == DeathType.Meditation) {
                DioramaMachine._isMeditating = true;
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(DestructionVolume), nameof(DestructionVolume.VanishProbe))]
        public static bool DestructionVolume_VanishProbe_Prefix(DestructionVolume __instance, OWRigidbody probeBody) {
            //Jam5PingBox.Log(__instance.name);
            //Jam5PingBox.Log(__instance.transform.root.name);
            if(__instance.transform.root) {
                var rootName = __instance.transform.root.name;
                if(rootName == "Salvation_Body" || rootName == "Hope_Body" || rootName == "Faith_Body") {
                    GameObject.Destroy(probeBody.gameObject);
                    return false;
                }
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(HazardDetector), nameof(HazardDetector.IsInvulnerable))]
        public static bool HazardDetector_IsInvulnerable_Prefix(HazardDetector __instance, ref bool __result) {
            if (__instance._insulatingVolumes != null && __instance._insulatingVolumes.Any(x => x.name == "HeatInsulatingVolume")) {
                __result = true;
                return false;
            }
            return true;
        }

        //[HarmonyPrefix]
        //[HarmonyPatch(typeof(HazardDetector), nameof(HazardDetector.AddInsulatingVolume))]
        //public static bool HazardDetector_AddInsulatingVolume_Prefix(HazardDetector __instance, InsulatingVolume insulatingVolume) {
        //    if(insulatingVolume.name != "HeatInsulatingVolume") {
        //        return true;
        //    }
        //    if (!__instance._insulatingVolumes.Contains(insulatingVolume)) {
        //        __instance._insulatingVolumes.Add(insulatingVolume);
        //        __instance._hazardMask &= -5;
        //    }
        //    return false;
        //}

        //[HarmonyPrefix]
        //[HarmonyPatch(typeof(HazardDetector), nameof(HazardDetector.RemoveInsulatingVolume))]
        //public static bool HazardDetector_RemoveInsulatingVolume_Prefix(HazardDetector __instance, InsulatingVolume insulatingVolume) {
        //    if(insulatingVolume.name != "HeatInsulatingVolume") {
        //        return true;
        //    }
        //    if(__instance._insulatingVolumes.Contains(insulatingVolume)) {
        //        __instance._insulatingVolumes.Remove(insulatingVolume);
        //        for(int i = 0; i < __instance._activeVolumes.Count; ++i) {
        //            if((__instance._activeVolumes[i] as HazardVolume).GetHazardType() == HazardVolume.HazardType.HEAT) {
        //                __instance._hazardMask |= 4;
        //            }
        //        }
        //    }
        //    return false;
        //}

        //[HarmonyPostfix]
        //[HarmonyPatch(typeof(HazardDetector), nameof(HazardDetector.OnVolumeRemoved))]
        //public static void HazardDetector_OnVolumeRemoved_Postfix(HazardDetector __instance) {
        //    if (__instance._insulatingVolumes != null && __instance._insulatingVolumes.Any(x => x.name == "HeatInsulatingVolume")) {
        //        __instance._hazardMask &= -5;
        //    }
        //    // below codes cannot be called by CS0079, but all OnHazardsUpdated are related to DarkMatter, so it is no problem.
        //    //if (__instance.OnHazardsUpdated != null) {
        //    //    __instance.OnHazardsUpdated();
        //    //}
        //}

        //[HarmonyPrefix]
        //[HarmonyPatch(typeof(HazardDetector), nameof(HazardDetector.OnVolumeAdded))]
        //public static bool HazardDetector_OnVolumeAdded_Prefix(HazardDetector __instance, EffectVolume eVolume) {
        //    if (__instance._insulatingVolumes != null && __instance._insulatingVolumes.Any(x => x.name == "HeatInsulatingVolume")) {
        //        HazardVolume hazardVolume = eVolume as HazardVolume;
        //        if (hazardVolume != null || hazardVolume.GetHazardType() == HazardVolume.HazardType.HEAT) {
        //            return false;
        //        }
        //    }
        //    return true;
        //}
    }
}
