using UnityEngine;
using System.Collections.Generic;

namespace TenPN.DecisionFlex.Demos
{
    [AddComponentMenu("TenPN/DecisionFlex/Demos/Weapon Selection/EnemiesControl")]
    public class EnemiesControl : MonoBehaviour
    {
        //////////////////////////////////////////////////
        
        bool m_isPaused;
        IEnumerable<FollowWaypoints> m_enemies;

        //////////////////////////////////////////////////

        void Awake()
        {
            m_enemies = GetComponentsInChildren<FollowWaypoints>();
        }
        
        void OnGUI()
        {
            var togglePauseRect = new Rect(20f, 20f,
                                             400f, 40f);
            var newIsPaused = GUI.Toggle(togglePauseRect, m_isPaused, "Pause enemies");
            if (newIsPaused != m_isPaused)
            {
                foreach(var enemy in m_enemies)
                {
                    enemy.IsPaused = newIsPaused;
                }
                m_isPaused = newIsPaused;
            }
        }
    }
}