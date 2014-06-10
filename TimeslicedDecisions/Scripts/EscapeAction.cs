using UnityEngine;

namespace TenPN.DecisionFlex.Demos
{
    //! orbits around enemy, attacking it
    [AddComponentMenu("TenPN/DecisionFlex/Demos/Timesliced/EscapeAction")]
    public class EscapeAction : Action
    {
        public override void Perform(IConsiderationContext context)
        {
            Destroy(transform.parent.parent.gameObject);
        }
    }
}