using GPUTools.Hair.Scripts.Runtime.Data;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Runtime.Physics
{
    public class  GPHairPhysics : MonoBehaviour
    {
        private HairPhysicsWorld world;

        public void Initialize(HairDataFacade data)
        {
            world = new HairPhysicsWorld(data);
        }

        public void FixedDispatch()
        {
            world.FixedDispatch();
        }

        public void Dispatch()
        {
            world.Dispatch();
        }

        private void OnDestroy()
        {
            world.Dispose();
        }

        private void OnDrawGizmos()
        {
            world.DebugDraw();
        }
    }
}
