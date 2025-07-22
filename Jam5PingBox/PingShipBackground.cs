using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

namespace Jam5PingBox {
    public class PingShipBackground : MonoBehaviour {
        IEnumerator Start() {
            Vector3 rot = transform.localEulerAngles;
            float root3 = Mathf.Sqrt(3);
            while(true) {
                yield return null;
                rot += new Vector3(1, root3, 0) * Time.deltaTime * 5;
                transform.localEulerAngles = rot;
            }
        }
    }
}
