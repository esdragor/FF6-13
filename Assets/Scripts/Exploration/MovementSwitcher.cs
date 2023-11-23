using UnityEngine;
using UnityEngine.Serialization;

namespace Exploration
{
    public class MovementSwitcher : MonoBehaviour
    {
        [field:SerializeField] public PNJMovement PnjMovement { get; private set; }
        [field:SerializeField] public Entity Entity { get; private set; }
        
        public void SwitchToPNJMode()
        {
            Entity.enabled = false;
            PnjMovement.enabled = true;
            PnjMovement.ResetMat();
        }
        
        public void SwitchToPlayerMode()
        {
            PnjMovement.enabled = false;
            Entity.enabled = true;
            
            if (Entity as PlayerEntity) ((PlayerEntity)Entity).ResetWantedPosition();
            
            Entity.ResetMat();
        }
    }
}