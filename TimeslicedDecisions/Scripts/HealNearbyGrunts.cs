using UnityEngine;

namespace TenPN.DecisionFlex.Demos
{
    [AddComponentMenu("TenPN/DecisionFlex/Demos/Timesliced/HealNearbyGrunts")]
    public class HealNearbyGrunts : MonoBehaviour
    {
        //////////////////////////////////////////////////

        private static readonly int s_maxGrunts = 99;
        private Collider2D[] m_gruntCache = new Collider2D[s_maxGrunts];
        private CircleCollider2D m_healZone;

        [SerializeField]
        private float m_healRate = 5f;

        //////////////////////////////////////////////////

        void Start()
        {
            m_healZone = GetComponent<CircleCollider2D>();
        }

        void Update()
        {
            float restoredHealth = Time.deltaTime * m_healRate;
            HealNearbyGruntsBy(restoredHealth);
        }
        
        void HealNearbyGruntsBy(float restoredHealth)
        {
            int gruntMask = 1 << LayerMask.NameToLayer("Grunt");
            float healRadius = m_healZone.radius;
            int nearbyGruntCount = 
                Physics2D.OverlapCircleNonAlloc(transform.position,
                                                healRadius,
                                                m_gruntCache,
                                                gruntMask);

            for(int gruntIndex = 0; gruntIndex < nearbyGruntCount; ++gruntIndex)
            {
                var nearbyGrunt = m_gruntCache[gruntIndex].GetComponent<Grunt>();
                nearbyGrunt.HP += restoredHealth;
            }
        }
    }
}