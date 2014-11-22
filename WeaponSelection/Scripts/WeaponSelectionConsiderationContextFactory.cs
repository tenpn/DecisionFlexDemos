/*
The MIT License (MIT)

Copyright (c) 2014 Andrew Fray

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
 */
using UnityEngine;
using System;
using System.Collections.Generic;

namespace TenPN.DecisionFlex.Demos
{
    /** 
        Every decision, generates one consideration context per enemy.
        This means we can pick the best weapon-enemy pair to shoot with-at.
        Each context also has full ammo information, using a hierarchical context.
    */
    [AddComponentMenu("TenPN/DecisionFlex/Demos/Weapon Selection/" 
                      + "WeaponSelectionConsiderationContextFactory")]
    public class WeaponSelectionConsiderationContextFactory : ContextFactory
    {
        // return all enemies in turn
        public override IList<IConsiderationContext> AllContexts(Logging loggingSetting)
        {
            var masterContext = new ConsiderationContextDictionary();

            var inventoryContents = m_inventory.CalculateInventoryContents();
            foreach(var ammo in inventoryContents)
            {
                masterContext.SetContext(ammo.Name, ammo.GetCount());
            }

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

        Inventory m_inventory;

        //////////////////////////////////////////////////

        void Awake() {
            m_inventory = GetComponent<Inventory>();
        }

    }
}