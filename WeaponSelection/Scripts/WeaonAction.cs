
using UnityEngine;
using System;

namespace TenPN.DecisionFlex.Demos
{
    public class WeaponAction : UtilityAction
    {
        public override void Perform(IConsiderationContext context)
        {
            var target = context.GetContext<GameObject>("Enemy");
            string label = m_weaponVerb + " ";
            LabelLerper.LerpLabelFromTo(label, transform.position, target.transform);
        }

        //////////////////////////////////////////////////

        [SerializeField] string m_weaponVerb;

        //////////////////////////////////////////////////
    }
}