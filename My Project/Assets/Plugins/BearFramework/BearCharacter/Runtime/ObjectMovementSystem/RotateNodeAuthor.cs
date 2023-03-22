using Bear;
using UnityEngine;

namespace Bear {
    [RequireComponent(typeof(UpdaterNodeView))]
    public class RotateNodeAuthor : MonoBehaviour
    {
        public AnimationCurve x;
        public AnimationCurve y;
        public AnimationCurve z;
        public float MaxLife = 1;
        // Start is called before the first frame update
        void Start()
        {
            GetComponent<UpdaterNodeView>().AddNodeData(new CurvedRotationNodeData() {
                xRotation = x,
                yRotation = y,
                zRotation = z
            });
        }

        [ContextMenu("Rotate")]
        public void Rotate() { 
            
        }

    }
}