using Bear;
using UnityEngine;

namespace Bear
{
    public class ParticleInvokeNodeData : INodeData
    {
        public ParticleSystem particleSystem;
        public void Play(Collision collision) {
            var point = collision.GetContact(0);
            particleSystem.transform.position = point.point;
            particleSystem.Play();

        }

        
    }
}