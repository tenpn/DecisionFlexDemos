
using UnityEngine;
using System;

namespace TenPN.DecisionFlex.Demos
{

    [AddComponentMenu("Weapon Choice/Shotgun")]
    public class ShotgunAction : UtilityAction
    {
        public override void Perform(IConsiderationContext context)
        {
            Debug.Log("CH-CH BOOM " + context.GetContext<GameObject>("Enemy"));
        }

        //////////////////////////////////////////////////

        //////////////////////////////////////////////////
    }
}