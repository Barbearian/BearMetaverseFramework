using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.ParticleSystem;

namespace Bear
{
    [RequireComponent(typeof(ParticleSystem))]
    public class SingleParticleObserver : MonoBehaviour
    {
        ParticleSystem system;
        private Particle[] particles = new Particle[1];
        public UnityEvent<Particle> DOnObserved;
        public void Awake()
        {
            system = GetComponent<ParticleSystem>();
        }
        private void FixedUpdate()
        {
            if (system.GetParticles(particles) == 1) {
                DOnObserved.Invoke(particles[0]);
            }
        }
    }
}