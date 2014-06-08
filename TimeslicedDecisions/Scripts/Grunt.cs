using UnityEngine;
using System;

namespace TenPN.DecisionFlex.Demos
{
    [AddComponentMenu("TenPN/DecisionFlex/Demos/Timesliced/Grunt")]
    public class Grunt : MonoBehaviour
    {
        public float HP
        {
            get { return m_hp; }
            set
            {
                var newHP = Mathf.Clamp(value, 0f, m_maxHP);
                if (newHP == m_hp)
                {
                    return;
                }

                m_hp = newHP;
                if (HPChange != null)
                {
                    HPChange();
                }
            }
        }

        public float NormalizedHP
        {
            get { return HP / m_maxHP; }
        }

        public delegate void HPChangeCallback();
        public event HPChangeCallback HPChange;

        public Vector3 Velocity 
        {
            get; set;
        }

        //////////////////////////////////////////////////

        [SerializeField] 
        private float m_maxHP;
        private float m_hp;

        //////////////////////////////////////////////////

        void Awake()
        {
            m_hp = m_maxHP;
            Velocity = Vector3.zero;
        }
    }
    
}