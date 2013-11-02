
using UnityEngine;
using System;

namespace TenPN.DecisionFlex.Demos
{

    [AddComponentMenu("Weapon Choice/Sniper")]
    public class SniperAction : UtilityAction
    {
        public override void Perform(IConsiderationContext context)
        {
            Debug.Log("*breathe* CRACK " + context.GetContext<GameObject>("Enemy"));
        }

        //////////////////////////////////////////////////

        //////////////////////////////////////////////////
    }
}