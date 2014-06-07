using UnityEngine;

namespace TenPN.DecisionFlex.Demos
{
    [AddComponentMenu("TenPN/DecisionFlex/Demos/Weapon Selection/FollowWaypoints")]
    public class FollowWaypoints : MonoBehaviour
    {
        public bool IsPaused { get; set; }

        //////////////////////////////////////////////////

        [SerializeField] Transform[] m_waypoints;
        [SerializeField] float m_smoothTime = 1f;

        Vector3 m_velocity;
        int m_currentWaypoint = 0;

        //////////////////////////////////////////////////

        void Update()
        {
            if (IsPaused)
            {
                return;
            }

            var targetPos = m_waypoints[m_currentWaypoint].position;
            
            var newPos = Vector3.SmoothDamp(transform.position, targetPos, 
                                            ref m_velocity, m_smoothTime);
            transform.position = newPos;

            float sqDistToTarget = (targetPos - newPos).sqrMagnitude;
            if (sqDistToTarget < 0.25f)
            {
                int nextWaypoint = m_currentWaypoint + 1;
                m_currentWaypoint = nextWaypoint % m_waypoints.Length;
            }
        }
    }
}