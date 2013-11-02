
using UnityEngine;
using System;

namespace TenPN.DecisionFlex.Demos
{
    public class KnifeAction : UtilityAction
    {
        public override void Perform(IConsiderationContext context)
        {
            Debug.Log("STAB STAB STAB at " + context.GetContext<GameObject>("Enemy"));
        }

        //////////////////////////////////////////////////

        //////////////////////////////////////////////////
    }
}