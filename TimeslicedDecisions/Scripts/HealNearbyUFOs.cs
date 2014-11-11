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
    [AddComponentMenu("TenPN/DecisionFlex/Demos/Timesliced/HealNearbyUFOs")]
    public class HealNearbyUFOs : MonoBehaviour
    {
        //////////////////////////////////////////////////

        private static readonly int s_maxUFOs = 99;
        private Collider2D[] m_ufoCache = new Collider2D[s_maxUFOs];
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
            HealNearbyUFOsBy(restoredHealth);
        }
        
        void HealNearbyUFOsBy(float restoredHealth)
        {
            int ufoMask = 1 << LayerMask.NameToLayer("UFO");
            float healRadius = m_healZone.radius;
            int nearbyUFOCount = 
                Physics2D.OverlapCircleNonAlloc(transform.position,
                                                healRadius,
                                                m_ufoCache,
                                                ufoMask);

            for(int ufoIndex = 0; ufoIndex < nearbyUFOCount; ++ufoIndex)
            {
                var nearbyUFO = m_ufoCache[ufoIndex].GetComponent<UFO>();
                nearbyUFO.HP += restoredHealth;
            }
        }
    }
}