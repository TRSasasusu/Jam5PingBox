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

        public enum BoxType {
            BOX1,
            BOX2,
            BOX3,
            BOX_TRISTAR,
        }

        public void Initialize() {
            _box1.SetActive(false);
        }

        public void Load(BoxType boxType) {
            if (boxType == BoxType.BOX1) {
                _box1.SetActive(true);
            }
            _dioramaMachine.SetActive(false);
        }
    }
}
