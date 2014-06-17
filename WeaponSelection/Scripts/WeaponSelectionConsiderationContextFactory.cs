
using UnityEngine;
using System;
using System.Collections.Generic;

namespace TenPN.DecisionFlex.Demos
{
    [AddComponentMenu("TenPN/DecisionFlex/Demos/Weapon Selection/" 
                      + "WeaponSelectionConsiderationContextFactory")]
    public class WeaponSelectionConsiderationContextFactory : ConsiderationContextFactory
    {
        // return all enemies in turn
        public override IList<IConsiderationContext> AllContexts(Logging loggingSetting)
        {
            var masterContext = new ConsiderationContextDictionary();

            SendMessage("PopulateMasterConsiderationContext", masterContext);

            var allEnemies = GameObject.FindGameObjectsWithTag(m_enemyTagName);

            var allContexts = new IConsiderationContext[allEnemies.Length];

            for(int enemyIndex = 0; enemyIndex < allEnemies.Length; ++enemyIndex)
            {
                var enemy = allEnemies[enemyIndex];
                var context = new ConsiderationContextDictionary();
        
                float targetDistance = Vector3.Distance(transform.position,
                                                        enemy.transform.position);

                if (loggingSetting == Logging.Enabled)
                {
                    Debug.Log("considering enemy " + enemy.name + " at distance " 
                              + targetDistance);
                }

                context.SetContext(m_enemyGOContextName, enemy);
                context.SetContext(m_enemyDistanceContextName, targetDistance);

                var parentedContext = 
                    new HierarchicalConsiderationContext(context, masterContext);
                allContexts[enemyIndex] = parentedContext;
            }

            return allContexts;
        }

        //////////////////////////////////////////////////

        [SerializeField] private string m_enemyTagName = "Enemy";
        [SerializeField] private string m_enemyGOContextName = "Enemy";
        [SerializeField] private string m_enemyDistanceContextName = "EnemyDistance";

        //////////////////////////////////////////////////

    }
}