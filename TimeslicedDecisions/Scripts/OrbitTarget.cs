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

namespace TenPN.DecisionFlex.Demos
{
    //! orbits around enemy, attacking it
    [AddComponentMenu("TenPN/DecisionFlex/Demos/Timesliced/OrbitTarget")]
    public class OrbitTarget : MonoBehaviour
    {
        //////////////////////////////////////////////////

        private Grunt m_grunt;
        private Transform m_target;
        private float m_seed;

        [SerializeField]
        private float m_orbitSpeed = 1f;

        [SerializeField]
        private float m_orbitHeightForce = 1f;

        [SerializeField]
        private float m_orbitSpeedForce = 1f;

        [SerializeField]
        private string m_targetTag = "Enemy";

        //////////////////////////////////////////////////

        private void Start()
        {
            m_target = GameObject.FindWithTag(m_targetTag).transform;
            
            m_grunt = transform.parent.parent.GetComponent<Grunt>();

            m_seed = Random.value * 30f;
        }

        private void Update()
        {
            var gruntPos = m_grunt.transform.position;
            var toEnemy = m_target.position - gruntPos;
            float distanceToEnemy = toEnemy.magnitude;
            var towardsEnemy = 
                distanceToEnemy == 0f ? Vector3.right : toEnemy/distanceToEnemy;

            // move the target around a bit, so we have a bit of wiggle to movement
            float enemyOffset = Mathf.PerlinNoise(Time.time, m_seed) - 0.5f;
            var targetPos = 
                m_target.position 
                + new Vector3(towardsEnemy.y, -towardsEnemy.x) * enemyOffset;
            var towardsTarget = (targetPos - gruntPos).normalized;
            //Debug.DrawLine(m_target.position, targetPos, Color.magenta);
            
            float desiredOrbitDistance = 
                ((CircleCollider2D)m_target.collider2D).radius * 0.75f;
            float deltaToDesiredOrbit = desiredOrbitDistance - distanceToEnemy;
            bool isTooFarAway = deltaToDesiredOrbit < 0f;

            float orbitHeightStrength = isTooFarAway 
                ? Mathf.InverseLerp(0f, desiredOrbitDistance * 0.5f, -deltaToDesiredOrbit)
                : 0; // as we follow tangent, we'll move out anyway
            var forceTowardsOrbit = orbitHeightStrength * m_orbitHeightForce * towardsTarget;
            //Debug.DrawRay(gruntPos, forceTowardsOrbit, Color.blue);

            var orbitDirection = new Vector3(towardsTarget.y, -towardsTarget.x, 0f);
            float currentOrbitSpeed = Vector3.Dot(m_grunt.Velocity, orbitDirection);
            float orbitSpeedDelta = m_orbitSpeed - currentOrbitSpeed;
            bool isTooSlow = orbitSpeedDelta > 0f;
            float orbitSpeedStrength = isTooSlow
                ? Mathf.InverseLerp(0f, m_orbitSpeed, orbitSpeedDelta)
                : -Mathf.InverseLerp(0f, m_orbitSpeed, -orbitSpeedDelta);
            var forceTowardsSpeed = 
                orbitDirection * orbitSpeedStrength * m_orbitSpeedForce;
            Debug.DrawRay(gruntPos, forceTowardsSpeed, Color.yellow);

            var finalSteer = forceTowardsOrbit + forceTowardsSpeed;
            //Debug.DrawRay(gruntPos, finalSteer, Color.magenta);
            m_grunt.Velocity += finalSteer * Time.deltaTime;

            m_grunt.transform.position += m_grunt.Velocity * Time.deltaTime;

            Debug.DrawRay(m_grunt.transform.position, m_grunt.Velocity, Color.green);
        }
    }
}