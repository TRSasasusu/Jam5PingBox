using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using NewHorizons;
using NewHorizons.Utility;
using IEnumerator = System.Collections.IEnumerator;

namespace Jam5PingBox {
    public class ObjectModifier {
        const string ENERGY_CABLE_PATH = "CaveTwin_Body/Sector_CaveTwin/Lighting_CaveTwin/Structure_NOM_TLECable";
        const string DIORAMA_INTERFACE_PATH = "DioramaInterface_Body/Sector/";
        const string DIORAMA_WARP_START_PATH = "DioramaInterface_Body/Sector/DioramaMachine/Prefab_NOM_WarpReceiver";
        const string DIORAMA_MACHINE_PATH = "DioramaInterface_Body/Sector/DioramaMachine";
        const string BOX1_PATH = "DioramaInterface_Body/Sector/Box1";
        const string BOX2_PATH = "DioramaInterface_Body/Sector/Box2";
        const string BOX3_PATH = "DioramaInterface_Body/Sector/Box3";
        const string PLATFORM_PATH = "Orclecle_Mod_Platform_Body/Sector";
        const string SHADOW_OF_SPARKS_PATH = "ShadowofSparks_Body/Sector";
        const string BOX_TRISTAR_PATH = "ShadowofSparks_Body/Sector/BoxTriStar";
        const string HIDDEN_PING_SHIP_PATH = "HiddenPingShip_Body";
        static readonly string[] BOX_TRISTER_OBJ_PATHS = new string[] {
            "Hope_Body",
            "Salvation_Body",
            "Faith_Body",
            "ShadowofSparks_Body",
        };
        const string TOWER_PATH = "Orclecle_Mod_Platform_Body/Sector/Tower";
        const string PING_PATH = "ExamplePlatform_Body/Sector/Nomai";
        const string PING_PATH_v105 = "CentralStation_Body/Sector/Nomai";
        const string HIDDEN_PING_PATH = "HiddenPingShip_Body/Sector/Nomai";

        public ObjectModifier() {
            Jam5PingBox.Instance.StartCoroutine(Initialize());
        }

        IEnumerator Initialize() {
            Material originalEnergyCableMaterial = null;
            while (true) {
                var energyCableObj = SearchUtilities.Find(ENERGY_CABLE_PATH);
                if (energyCableObj) {
                    originalEnergyCableMaterial = energyCableObj.GetComponent<Renderer>().sharedMaterial;
                    break;
                }
                yield return null;
            }

            GameObject dioramaInterface = null;
            while(true) {
                dioramaInterface = SearchUtilities.Find(DIORAMA_INTERFACE_PATH);
                if (dioramaInterface) {
                    break;
                }
                yield return null;
            }
            GameObject platform = null;
            while (true) {
                platform = SearchUtilities.Find(PLATFORM_PATH);
                if (platform) {
                    break;
                }
                yield return null;
            }
            GameObject shadowOfSparks = null;
            while (true) {
                shadowOfSparks = SearchUtilities.Find(SHADOW_OF_SPARKS_PATH);
                if (shadowOfSparks) {
                    break;
                }
                yield return null;
            }

            foreach (var sector in new[] { dioramaInterface, platform, shadowOfSparks }) {
                foreach (var child in sector.GetComponentsInChildren<Transform>(true)) {
                    if (child.name.Contains("EnergyCable")) {
                        child.gameObject.AddComponent<SetEnergyCableMat>().Initialize(originalEnergyCableMaterial);
                    }
                    if (child.name.Contains("CableOff")) {
                        child.gameObject.AddComponent<SetCableOffMat>();
                    }
                    if (child.name.Contains("Tractor Beam")) {
                        if (child.name.Contains("Reverse")) {
                            child.GetComponent<TractorBeamController>().SetReversed(true);
                        }
                        else {
                            child.GetComponentInChildren<TractorBeamFluid>().OnValidate();
                        }
                    }
                }
            }

            while (true) {
                var warp = SearchUtilities.Find(DIORAMA_WARP_START_PATH);
                if (warp) {
                    warp.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    break;
                }
                yield return null;
            }

            yield return null;
            GameObject ping;
            while (true) {
                ping = SearchUtilities.Find(PING_PATH);
                if (ping) {
                    break;
                }
                ping = SearchUtilities.Find(PING_PATH_v105);
                if (ping) {
                    break;
                }
                yield return null;
            }
            GameObject hiddenPing;
            while (true) {
                hiddenPing = SearchUtilities.Find(HIDDEN_PING_PATH);
                if (hiddenPing) {
                    break;
                }
                yield return null;
            }

            try {
                foreach (var renderer in ping.GetComponentsInChildren<Renderer>()) {
                    foreach (var hiddenRenderer in hiddenPing.GetComponentsInChildren<Renderer>()) {
                        if (renderer.name == hiddenRenderer.name) {
                            hiddenRenderer.sharedMaterials = renderer.sharedMaterials;
                            //for(int i = 0; i < hiddenRenderer.sharedMaterials.Length; ++i) {
                            //    hiddenRenderer.sharedMaterials[i] = renderer.sharedMaterials[i];
                            //}
                            //hiddenRenderer.material = renderer.sharedMaterial;
                            break;
                        }
                    }
                }
            }
            catch (Exception e) {
                Jam5PingBox.Log(e.Message);
            }

            DioramaMachine dioramaMachine = null;
            while (true) {
                var dioramaMachineObj = SearchUtilities.Find(DIORAMA_MACHINE_PATH);
                if (dioramaMachineObj) {
                    dioramaMachine = dioramaMachineObj.AddComponent<DioramaMachine>();
                    dioramaMachine._box1 = SearchUtilities.Find(BOX1_PATH);
                    dioramaMachine._box2 = SearchUtilities.Find(BOX2_PATH);
                    dioramaMachine._box3 = SearchUtilities.Find(BOX3_PATH);
                    dioramaMachine._hiddenPingShip = SearchUtilities.Find(HIDDEN_PING_SHIP_PATH);

                    dioramaMachine._boxTriStar = SearchUtilities.Find(BOX_TRISTAR_PATH);
                    dioramaMachine._boxTriStarObjs = BOX_TRISTER_OBJ_PATHS.Select(x => SearchUtilities.Find(x)).ToList();

                    dioramaMachine.Initialize();
                    break;
                }
                yield return null;
            }

            while (true) {
                var towerObj = SearchUtilities.Find(TOWER_PATH);
                if (towerObj) {
                    towerObj.AddComponent<Tower>().Initialize();
                    break;
                }
                yield return null;
            }
        }
    }
}
