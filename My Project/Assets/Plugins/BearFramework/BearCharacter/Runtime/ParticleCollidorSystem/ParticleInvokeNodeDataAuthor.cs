
using UnityEngine;
namespace Bear
{
    [RequireComponent(typeof(CollisionObserver))]
    public class ParticleInvokeNodeDataAuthor : MonoBehaviour
    {
        public ParticleSystem ParticleSystem;
        CollisionObserver collisionObserver;
        private void Awake()
        {
            collisionObserver = GetComponent<CollisionObserver>();
            var particleInvoker = new ParticleInvokeNodeData() { 
                particleSystem = ParticleSystem
            };
            collisionObserver.DOnCollisionEnter += particleInvoker.Play;
        }


    }
}