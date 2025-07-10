using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Jam5PingBox {
    public class SetCableOffMat : MonoBehaviour {
        Material _cableOffMaterial;

        public void Initialize() {
            var zSize = transform.localScale.z;
            _cableOffMaterial = GetComponent<MeshRenderer>().material;
            _cableOffMaterial.mainTextureScale = new Vector2(1, 60 * (zSize / 10));
        }

        void OnDestroy() {
            Destroy(_cableOffMaterial);
        }
    }
}
