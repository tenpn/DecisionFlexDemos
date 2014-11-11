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

        void LateUpdate()
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