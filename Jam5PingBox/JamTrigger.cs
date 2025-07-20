using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Jam5PingBox {
    public class JamTrigger : MonoBehaviour {
        void OnTriggerEnter(Collider other) {
            if (other.gameObject == Locator._playerBody.gameObject) {
                DioramaMachine.Instance.EnterJam();
            }
        }
    }
}
