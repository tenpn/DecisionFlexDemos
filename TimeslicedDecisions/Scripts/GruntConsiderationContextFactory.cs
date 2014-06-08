using UnityEngine;
using System.Collections.Generic;

namespace TenPN.DecisionFlex.Demos
{
    [AddComponentMenu("TenPN/DecisionFlex/Demos/Timesliced/GruntConsiderationContextFactory")]
    public class GruntConsiderationContextFactory : ConsiderationContextFactory
    {
        // returns considerations of this grunt
        public override IEnumerable<IConsiderationContext> AllContexts(Logging loggingSetting)
        {
            var context = new ConsiderationContextDictionary();
            
            var distanceToHealth = 
                Mathf.Max(0f, 
                          (m_healthArea.transform.position - transform.position).magnitude 
                          - m_healthArea.radius);
            context.SetContext("DistanceToHealth", distanceToHealth);

            var distanceToEnemy = 
                Mathf.Max(0f, 
                          (m_enemyArea.transform.position - transform.position).magnitude 
                          - m_enemyArea.radius);
            context.SetContext("DistanceToEnemy", distanceToEnemy);

            context.SetContext("HP", m_grunt.NormalizedHP);
            
            yield return context;
        }

        //////////////////////////////////////////////////

        private CircleCollider2D m_healthArea;
        private CircleCollider2D m_enemyArea;
        private Grunt m_grunt;

        //////////////////////////////////////////////////

        private void Start()
        {
            m_healthArea = (CircleCollider2D)GameObject.FindWithTag("Health").collider2D;
            m_enemyArea = (CircleCollider2D)GameObject.FindWithTag("Enemy").collider2D;
            m_grunt = transform.parent.GetComponent<Grunt>();
        }
    }
}
