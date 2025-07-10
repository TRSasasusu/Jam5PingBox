using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Jam5PingBox {
    public class Box1 : MonoBehaviour {
        Transform _door;
        Transform _doorOpen;
        Transform _doorClose;
        BoxButton _doorButton;

        bool _isDoorOpen = false;

        public void Initialize() {
            foreach (Transform child in GetComponentsInChildren<Transform>(true)) {
                if(child.name == "BoxButtonDoor") {
                    _doorButton = child.gameObject.AddComponent<BoxButton>();
                }
                else if(child.name == "Door") {
                    _door = child.Find("DoorBody");
                    _doorOpen = child.Find("Open");
                    _doorClose = child.Find("Close");
                }
            }

            _doorButton._onAction = () => {
                _isDoorOpen = true;
            };
            _doorButton._offAction = () => {
                _isDoorOpen = false;
            };
            _doorButton.Initialize();
        }

        void Update() {
            if(_door) {
                var pos = _isDoorOpen ? _doorOpen : _doorClose;
                _door.transform.localPosition = Vector3.Lerp(_door.transform.localPosition, pos.localPosition, 0.1f);
            }
        }
    }
}
