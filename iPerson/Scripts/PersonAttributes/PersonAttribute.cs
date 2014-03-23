
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TenPN.DecisionFlex.Demos
{

    public abstract class PersonAttribute : MonoBehaviour
    {
        public float Value
        {
            get { return m_currentValue; }
            protected set 
            { 
                m_currentValue = Mathf.Clamp(value, 0.0f, 1.0f);
            }
        }

        public void BoostAttribute(float boost)
        {
            Value += boost;
        }

        public string Name { get { return m_baseName; } }

        protected virtual void Update()
        {
            gameObject.name = m_baseName + ": " + Value;
        }

        //////////////////////////////////////////////////

        [RangeAttribute(0.0f, 1.0f)]
        [SerializeField] private float m_startingValue;

        private float m_currentValue;

        private string m_baseName;

        //////////////////////////////////////////////////

        private void Awake()
        {
            Value = m_startingValue;
            m_baseName = gameObject.name;
        }

    }
}