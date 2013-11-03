
using UnityEngine;
using System;

namespace TenPN.DecisionFlex.Demos
{

    public class RandomModifyAttribute : Action
    {
        public override void Perform(IConsiderationContext context)
        {
            m_target.BoostAttribute(GetNextModify());
        }
    
        //////////////////////////////////////////////////

        [Range(-1f,1f)]
        [SerializeField] private float m_minModify;
        [Range(-1f,1f)]
        [SerializeField] private float m_maxModify;

        [SerializeField] private PersonAttribute m_target;

        //////////////////////////////////////////////////

        private float GetNextModify()
        {
            float range = m_maxModify - m_minModify;
            return UnityEngine.Random.value * range + m_minModify;
        }
    }
}