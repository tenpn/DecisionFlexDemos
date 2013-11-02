
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

namespace TenPN.DecisionFlex.Demos
{

    /** graph all attributes over time, on screen */
    public class AttributeRenderer : MonoBehaviour
    {
        //////////////////////////////////////////////////

        [Range(0f,2f)]
        [SerializeField] private float m_sampleInterval;

        [SerializeField] private int m_historySize = 50;

        private Dictionary<PersonAttribute, float[]> m_attributeHistories;    
        private int m_currentStartIndex;
        private int m_currentHistorySize;

        //////////////////////////////////////////////////

        private void Awake()
        {
            var attributes = GetComponentsInChildren<PersonAttribute>();

            m_attributeHistories = new Dictionary<PersonAttribute, float[]>();
            foreach(var attribute in attributes)
            {
                m_attributeHistories[attribute] = new float[m_historySize];
            }
        
            m_currentStartIndex = m_currentHistorySize = 0;

            StartCoroutine(Sampler());
        }

        private IEnumerator Sampler()
        {
            while(true)
            {
                // could use InvokeRepeating here, but then you couldn't
                // fiddle with the sample interval in the editor during play.
                RecordAttributes();
                yield return new WaitForSeconds(m_sampleInterval);
            }
        }

        private void RecordAttributes()
        {
            int newHistorySlot = 0;

            if (m_currentHistorySize < m_historySize)
            {
                newHistorySlot = m_currentStartIndex + m_currentHistorySize;
                ++m_currentHistorySize;
            }
            else
            {
                newHistorySlot = m_currentStartIndex;
                m_currentStartIndex = (m_currentStartIndex + 1) % m_historySize;
            }

            foreach(var attributeHistory in m_attributeHistories)
            {
                var attribute = attributeHistory.Key;
                var history = attributeHistory.Value;

                history[newHistorySlot] = attribute.Value;
            }
        }

        private void OnGUI()
        {
            int lastOffset = m_currentHistorySize - 1;

            if (lastOffset < 0)
            {
                return;
            }

            int lastIndex = HistoryOffsetToIndex(lastOffset);

            foreach(var attributeHistory in m_attributeHistories)
            {
                var attribute = attributeHistory.Key;
                var history = attributeHistory.Value;
            
                float lastValue = history[lastIndex];
                var screenCoord = CalculateScreenCoord(lastOffset, lastValue);

                var labelRect = new Rect(screenCoord.x - 40.0f, 
                                         Screen.height - screenCoord.y, 
                                         150.0f, 50.0f);
                GUI.Label(labelRect, attribute.Name);
            }
        }

        private Vector2 CalculateScreenCoord(int historyOffset, float value)
        {
            return new Vector2(
                Screen.width * (historyOffset / (float)m_currentHistorySize),
                Screen.height * value
                );
        }

        private int HistoryOffsetToIndex(int offset)
        {
            return (m_currentStartIndex + offset) % m_historySize;
        }

        private void Update()
        {
            var colorRand = new System.Random(5);

            foreach(var history in m_attributeHistories.Values)
            {
                var colour = new Color((float)colorRand.NextDouble(),
                                       (float)colorRand.NextDouble(),
                                       (float)colorRand.NextDouble(),
                                       0.85f);

                var bottomLeftRay = Camera.main.ScreenPointToRay(Vector3.zero);
                var prevCoord = bottomLeftRay.origin + bottomLeftRay.direction*10f;

                for(int historyOffset = 0; historyOffset < m_currentHistorySize; ++historyOffset)
                {
                    int currentHistoryIndex = HistoryOffsetToIndex(historyOffset);
                    float historyValue = history[currentHistoryIndex];
     
                    var screenCoord = CalculateScreenCoord(historyOffset, historyValue);
                
                    var screenRay = Camera.main.ScreenPointToRay((Vector3)screenCoord);
                    var world = screenRay.origin + screenRay.direction * 10f;

                    Debug.DrawLine(prevCoord, world, colour);
                    prevCoord = world;
                }
            }
        }
    }
}