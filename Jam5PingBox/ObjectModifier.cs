using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

namespace Jam5PingBox {
    public class ObjectModifier {
        const string ENERGY_CABLE_PATH = "CaveTwin_Body/Sector_CaveTwin/Lighting_CaveTwin/Structure_NOM_TLECable";
        const string DIORAMA_INTERFACE_PATH = "DioramaInterface_Body/Sector/";
        const string DIORAMA_WARP_START_PATH = "DioramaInterface_Body/Sector/Prefab_NOM_WarpReceiver";

        public ObjectModifier() {
            Jam5PingBox.Instance.StartCoroutine(Initialize());
        }

        IEnumerator Initialize() {
            Material originalEnergyCableMaterial = null;
            while (true) {
                var energyCableObj = GameObject.Find(ENERGY_CABLE_PATH);
                if (energyCableObj) {
                    originalEnergyCableMaterial = energyCableObj.GetComponent<Renderer>().sharedMaterial;
                    break;
                }
                yield return null;
            }

            GameObject dioramaInterface = null;
            while(true) {
                dioramaInterface = GameObject.Find(DIORAMA_INTERFACE_PATH);
                if (dioramaInterface) {
                    break;
                }
                yield return null;
            }
            foreach(var child in dioramaInterface.GetComponentsInChildren<Transform>()) {
                if (child.name.Contains("EnergyCable")) {
                    child.gameObject.AddComponent<SetEnergyCableMat>().Initialize(originalEnergyCableMaterial);
                }
            }

            while (true) {
                var warp = GameObject.Find(DIORAMA_WARP_START_PATH);
                if (warp) {
                    warp.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    break;
                }
                yield return null;
            }
        }
    }
}
