using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Jam5PingBox {
    public class Tower : MonoBehaviour {
        public void Initialize() {
            var box1Done = PlayerData._currentGameSave.shipLogFactSaves.ContainsKey("diorama_box1_complete") && PlayerData._currentGameSave.shipLogFactSaves["diorama_box1_complete"].revealOrder > -1;
            var box2Done = PlayerData._currentGameSave.shipLogFactSaves.ContainsKey("diorama_box2_complete") && PlayerData._currentGameSave.shipLogFactSaves["diorama_box2_complete"].revealOrder > -1;
            var box3Done = PlayerData._currentGameSave.shipLogFactSaves.ContainsKey("diorama_box3_complete") && PlayerData._currentGameSave.shipLogFactSaves["diorama_box3_complete"].revealOrder > -1;
            foreach (Transform child in GetComponentsInChildren<Transform>(true)) {
                if(child.name == "ShortColumn (7)") {
                    Disable(child, box1Done);
                }
                else if(child.name == "ShortColumn (5)") {
                    Disable(child, box2Done);
                }
                else if(child.name == "ShortColumn (4)") {
                    Disable(child, box3Done);
                }
            }

            if (!box1Done || !box2Done || !box3Done) {
                transform.parent.Find("BoxTriStarItem").gameObject.SetActive(false);
            }
        }

        void Disable(Transform child, bool disableOff) {
            foreach(Transform childchild in child.GetComponentsInChildren<Transform>(true)) {
                if(disableOff) {
                    if(childchild.name == "PatternedPlatform (2)") {
                        childchild.gameObject.SetActive(false);
                        break;
                    }
                }
                else {
                    if(childchild.name == "PatternedPlatform (2) EnergyCable") {
                        childchild.gameObject.SetActive(false);
                        break;
                    }
                }
            }
        }
    }
}
