using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Jam5PingBox {
    public class DioramaMachine : MonoBehaviour {
        public GameObject _dioramaMachine;
        public GameObject _box1;
        public GameObject _box2;
        public GameObject _box3;
        public GameObject _boxTriStar;

        public enum BoxType {
            BOX1,
            BOX2,
            BOX3,
            BOX_TRISTAR,
        }

        public void Initialize() {
            _dioramaMachine = gameObject;

            _box1.SetActive(false);
        }

        public void Load(BoxType boxType) {
            GameObject box = null;
            if (boxType == BoxType.BOX1) {
                box = _box1;
                box.AddComponent<Box1>().Initialize();
            }
            box.SetActive(true);
            _dioramaMachine.SetActive(false);

            foreach (Transform child in box.GetComponentsInChildren<Transform>()) {
                if(child.name == "Spawn") {
                    var playerRigidbody = Locator.GetPlayerBody();
                    playerRigidbody.WarpToPositionRotation(child.position, child.rotation);
                    playerRigidbody.SetVelocity(PointVelocity(child));
                    break;
                }
            }

            Locator.GetPlayerSuit().RemoveSuit(true);
        }

        Vector3 PointVelocity(Transform point) {
            var parentOWRigidbody = point.GetComponentInParent<OWRigidbody>();
            return parentOWRigidbody.GetVelocity() + parentOWRigidbody.GetPointTangentialVelocity(point.position);
        }
    }
}
