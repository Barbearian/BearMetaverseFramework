using UnityEngine;

namespace Bear
{
    public class RotateNodeData : INodeData, IOnAttachedToNode
    {
        private NodeView root;
        public void Attached(INode node)
        {
            if (node is NodeView unode)
            {
                root = unode;
                Init(unode);
            }
        }

        private void Init(NodeView unode)
        {

        }

        public void Rotate(Quaternion degree)
        {
            root.transform.rotation *= degree;
        }

    }

    public class ConstantRotateNodeData : INodeData, IOnAttachedToNode,IOnDetachedFromNode
    {
        private IOnFixedUpdateUpdater updater;
        private NodeView root;
        private Quaternion original;
        private Quaternion rotationDegree;
        public void Attached(INode node)
        {
            if (root is NodeView uNode) { 
                root = uNode;
                Init(uNode);
            }

        }

        private void Init(NodeView unode)
        {
            if (root is IOnFixedUpdateUpdater myupdater) {
                updater = myupdater;
                myupdater.DOnFixedUpdate.Subscribe(Update);
                original = unode.transform.localRotation;
            }
        }

        public void Rotate(Quaternion degree)
        {
            root.transform.rotation *= degree;
        }

        private void Update() {
            var current = root.transform.rotation;

            var target = current * rotationDegree;
            root.transform.rotation = Quaternion.Lerp(current,target,Time.deltaTime);
        }

        public void Detached(INode node)
        {
            updater.DOnFixedUpdate.Unsubscribe(Update);
        }

        public void Reset() {
            root.transform.localRotation = original;
        }
    }

    public class CurvedRotationNodeData : INodeData, IOnAttachedToNode,IOnDetachedFromNode
    {
        public AnimationCurve xRotation;
        public AnimationCurve yRotation;
        public AnimationCurve zRotation;
        private NodeView root;
        private IOnFixedUpdateUpdater updater;
        public bool Playing;


        private float life;
        public float MaxLife = 1;
        public void Attached(INode node)
        {
            if (node is NodeView unode)
            {
                root = unode;
                Init(unode);
            }
        }

        private void Init(NodeView unode)
        {
            if (unode is IOnFixedUpdateUpdater myupdater) {
                updater = myupdater;
                myupdater.DOnFixedUpdate.Subscribe(Update);
            }
        }

        public void Update(float percentage) {
            var rotation = new Vector3(
                xRotation.Evaluate(percentage),
                yRotation.Evaluate(percentage),
                zRotation.Evaluate(percentage)
            );

            root.transform.rotation = Quaternion.Euler(rotation);
        }

        public void Update() {
            if (life <= MaxLife && Playing)
            {

                Update(life / MaxLife);
                life += Time.fixedDeltaTime;
                if (life> MaxLife) {
                    Update(1);
                    Playing = false;
                }
            }
            
            
        }

        public void Play() { 
            Reset();
        }

        public void Reset() {
            life = 0;
            Playing = true;
        }

        public void Detached(INode node)
        {
            updater.DOnFixedUpdate.Unsubscribe(Update);
        }
    }
}