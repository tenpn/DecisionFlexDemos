using UnityEngine;
using System.Collections.Generic;

namespace TenPN.DecisionFlex.Demos
{
    [AddComponentMenu("TenPN/DecisionFlex/Demos/Timesliced/GruntConsiderationContextFactory")]
    public class GruntConsiderationContextFactory : ConsiderationContextFactory
    {
        // returns considerations of this grunt
        public override IList<IConsiderationContext> AllContexts(Logging loggingSetting)
        {
            var distanceToHealth = 
                Mathf.Max(0f, 
                          (m_healthArea.transform.position - transform.position).magnitude 
                          - m_healthArea.radius);
            m_context.SetContext("DistanceToHealth", distanceToHealth);

            var distanceToEnemy = 
                Mathf.Max(0f, 
                          (m_enemyArea.transform.position - transform.position).magnitude 
                          - m_enemyArea.radius);
            m_context.SetContext("DistanceToEnemy", distanceToEnemy);

            m_context.SetContext("HP", m_grunt.NormalizedHP);
            
            return m_allContexts;
        }

        //////////////////////////////////////////////////

        // avoid allocations by caching everything we can reuse
        private ConsiderationContextDictionary m_context = 
            new ConsiderationContextDictionary();
        private IConsiderationContext[] m_allContexts = new IConsiderationContext[1];

        private CircleCollider2D m_healthArea;
        private CircleCollider2D m_enemyArea;
        private Grunt m_grunt;

        //////////////////////////////////////////////////

        private void Awake()
        {
            m_healthArea = (CircleCollider2D)GameObject.FindWithTag("Health").collider2D;
            m_enemyArea = (CircleCollider2D)GameObject.FindWithTag("Enemy").collider2D;
            m_grunt = transform.parent.GetComponent<Grunt>();
            m_allContexts[0] = m_context;
        }
    }
}
