using UnityEngine;

namespace TenPN.DecisionFlex.Demos
{
    [AddComponentMenu("TenPN/DecisionFlex/Demos/Timesliced/AttackNearbyGrunts")]
    public class AttackNearbyGrunts : MonoBehaviour
    {
        //////////////////////////////////////////////////

        private static readonly int s_maxGrunts = 99;
        private Collider2D[] m_gruntCache = new Collider2D[s_maxGrunts];
        private CircleCollider2D m_attackZone;
        private float m_timeToNextAttack = 0f;

        [SerializeField]
        private float m_attackStrength = 5f;

        [SerializeField]
        private float m_attackInterval = 1f;

        //////////////////////////////////////////////////

        void Start()
        {
            m_attackZone = GetComponent<CircleCollider2D>();
        }

        void Update()
        {
            m_attackInterval = Mathf.Max(m_attackInterval, 1/60f);

            m_timeToNextAttack -= Time.deltaTime;
            while (m_timeToNextAttack < 0f) 
            {
                var target = FindGruntToAttack();
                if (target != null)
                {
                    AttackGrunt(target);
                }
                m_timeToNextAttack += m_attackInterval;
            }
        }
        
        // returns null if no target
        Transform FindGruntToAttack()
        {
            int gruntMask = 1 << LayerMask.NameToLayer("Grunt");
            float attackRadius = m_attackZone.radius;
            int nearbyGruntCount = 
                Physics2D.OverlapCircleNonAlloc(transform.position,
                                                attackRadius,
                                                m_gruntCache,
                                                gruntMask);

            if (nearbyGruntCount == 0)
            {
                return null;
            }
            else
            {
                int victimIndex = Random.Range(0, nearbyGruntCount);
                return m_gruntCache[victimIndex].transform;
            }
        }

        void AttackGrunt(Transform victimTransform)
        {
            var victim = victimTransform.GetComponent<Grunt>();
            victim.HP -= m_attackStrength;
            Debug.DrawLine(transform.position, victimTransform.position, Color.red);
        }
    }
}