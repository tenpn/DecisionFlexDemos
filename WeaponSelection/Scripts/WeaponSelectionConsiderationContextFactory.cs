
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
        public override IEnumerable<IConsiderationContext> AllContexts(Logging loggingSetting)
        {
            var masterContext = new ConsiderationContextDictionary();

            SendMessage("PopulateMasterConsiderationContext", masterContext);

            var allEnemies = GameObject.FindGameObjectsWithTag(m_enemyTagName);
            foreach(var enemy in allEnemies)
            {
                var context = new ConsiderationContextDictionary(masterContext);
        
                float targetDistance = Vector3.Distance(transform.position,
                                                        enemy.transform.position);

                if (loggingSetting == Logging.Enabled)
                {
                    Debug.Log("considering enemy " + enemy.name + " at distance " 
                              + targetDistance);
                }

                context.SetContext(m_enemyGOContextName, enemy);
                context.SetContext(m_enemyDistanceContextName, targetDistance);
                yield return context;
            }
        }

        //////////////////////////////////////////////////

        [SerializeField] private string m_enemyTagName = "Enemy";
        [SerializeField] private string m_enemyGOContextName = "Enemy";
        [SerializeField] private string m_enemyDistanceContextName = "EnemyDistance";

        //////////////////////////////////////////////////

    }
}