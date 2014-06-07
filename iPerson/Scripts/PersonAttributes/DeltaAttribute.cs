
using UnityEngine;
using System;

namespace TenPN.DecisionFlex.Demos
{
    [AddComponentMenu("TenPN/DecisionFlex/Demos/iPerson/DeltaAttribute")]
    public class DeltaAttribute : PersonAttribute
    {
        protected override void Update()
        {
            base.Update();
            Value += m_valueDeltaPerSecond * Time.deltaTime;
        }

        //////////////////////////////////////////////////

        [SerializeField] private float m_valueDeltaPerSecond;

        //////////////////////////////////////////////////

    }
}