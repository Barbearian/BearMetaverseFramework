using UnityEngine;

namespace Bear
{
    public class ParticleRotationObserver : MonoBehaviour
    {
        public SingleParticleObserver m_Observer;
        private void Start()
        {
            m_Observer.DOnObserved.AddListener((x) => {
                transform.root.rotation = Quaternion.Euler(x.rotation3D);
            });
            
        }


    }
}