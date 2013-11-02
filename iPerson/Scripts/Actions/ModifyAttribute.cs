
using UnityEngine;
using System;

namespace TenPN.DecisionFlex.Demos
{

    public class ModifyAttribute : UtilityAction
    {
        public override void Perform(IConsiderationContext context)
        {
            m_target.BoostAttribute(m_boostValue);
        }

        //////////////////////////////////////////////////

        [SerializeField] private float m_boostValue;
        [SerializeField] private PersonAttribute m_target;

        //////////////////////////////////////////////////

    }
}