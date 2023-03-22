using UnityEngine;

namespace Bear
{
    public struct SpaceEdge
    {
        public Quaternion deltaRotation;
        public Vector3 deltaPosition;

        public static SpaceEdge operator +(SpaceEdge edge)
        {
            return edge;
        }

        public static SpaceEdge operator -(SpaceEdge edge)
        {
            return edge.Inverse();
        }

        public static SpaceEdge operator +(SpaceEdge edgeA,SpaceEdge edgeB) {
            return new SpaceEdge()
            {
                deltaPosition = edgeA.deltaPosition+edgeB.deltaPosition,
                deltaRotation = edgeA.deltaRotation*edgeB.deltaRotation
            };
        }

        public static SpaceEdge operator -(SpaceEdge edgeA, SpaceEdge edgeB)
        {
            return edgeA + edgeB.Inverse();
        }

        public static SpaceEdge Identity() {
            return new SpaceEdge()
            {
                deltaRotation = Quaternion.identity,
                deltaPosition = Vector3.zero,
            };
        }
    }

    public static class SpaceEdgeSystem {

        public static SpaceEdge Inverse(this SpaceEdge edge) {
            return new SpaceEdge()
            {
                deltaPosition = -edge.deltaPosition,
                deltaRotation = Quaternion.Inverse(edge.deltaRotation)
            };
        }


        public static void Transite(this Transform transform, SpaceEdge edge) { 
            transform.rotation *= edge.deltaRotation;
            transform.position += edge.deltaPosition;
        }

        public static SpaceEdge CalculateSpaceEdge(this Transform source, Transform target) {
            return new SpaceEdge() {
                deltaRotation = target.rotation * Quaternion.Inverse(source.rotation),
                deltaPosition = target.position - source.position
            };
        }
    }
}