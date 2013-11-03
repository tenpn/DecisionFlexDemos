
using UnityEngine;
using System;

namespace TenPN.DecisionFlex.Demos
{
    public class WeaponAction : Action
    {
        public override void Perform(IConsiderationContext context)
        {
            var target = context.GetContext<GameObject>("Enemy");
            LabelLerper.LerpLabelFromTo(m_weaponVerb, transform.position, target.transform);
        }

        //////////////////////////////////////////////////

        [SerializeField] string m_weaponVerb;

        //////////////////////////////////////////////////
    }
}