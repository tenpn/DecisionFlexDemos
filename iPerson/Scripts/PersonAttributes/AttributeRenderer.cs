
using UnityEngine;
using System;
using System.Linq;
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

        private Dictionary<PersonAttribute, Queue<float>> m_attributeHistories;    
        private int m_currentHistorySize;

        enum Source
        {
            Attributes,
            Scores,
        }
        private Source m_currentSource = Source.Attributes;

        //////////////////////////////////////////////////

        private void Awake()
        {
            var attributes = GetComponentsInChildren<PersonAttribute>();

            m_attributeHistories = new Dictionary<PersonAttribute, Queue<float>>();
            foreach(var attribute in attributes)
            {
                m_attributeHistories[attribute] = new Queue<float>();
            }
        
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
            ++m_currentHistorySize;
            m_currentHistorySize = Mathf.Min(m_currentHistorySize, m_historySize);

            foreach(var attributeHistory in m_attributeHistories)
            {
                var attribute = attributeHistory.Key;
                var history = attributeHistory.Value;

                history.Enqueue(attribute.Value);
                if (history.Count > m_historySize)
                {
                    history.Dequeue();
                }
            }
        }

        private void OnGUI()
        {
            int lastOffset = m_currentHistorySize - 1;

            if (lastOffset < 0)
            {
                return;
            }

            const float graphWidthProp = 0.9f;
            const float graphHeighProp = 0.85f;
            float graphScreenWidth = Screen.width * graphWidthProp;
            float graphScrenHeight = Screen.height * graphHeighProp;

            var graphRect = new Rect(0.5f * (Screen.width - graphScreenWidth), 
                                     0.5f * (Screen.height - graphScrenHeight), 
                                     graphScreenWidth, graphScrenHeight);

            RenderControlAroundGraph(graphRect);

            var xValues = Enumerable.Range(0, m_currentHistorySize)
                .Select(i => (float)i).ToArray();
            var baseGraph = new GraphParameters {
                ScreenBounds = graphRect,
                YValuesMinMax = new Vector2(0f, 1f),
                XValues = xValues,
                Cols = ColorGenerator(),
            };

            if (m_currentSource == Source.Attributes)
            {
                RenderAttributesGraph(baseGraph);
            }
            else
            {
                
            }
        }

        private void RenderAttributesGraph(GraphParameters baseGraph)
        {
            var yValuesList = m_attributeHistories.Values.Select(
                yValues => yValues.ToArray())
                .ToArray();
            baseGraph.YValuesList = yValuesList;
            GraphRenderer.Instance.RenderGraph(baseGraph);

            foreach(var attributeHistory in m_attributeHistories)
            {
                var attribute = attributeHistory.Key;
                var history = attributeHistory.Value;
            
                float lastValue = history.Last();
                var screenCoord = CalculateScreenCoord(lastValue, baseGraph.ScreenBounds);

                var labelRect = new Rect(screenCoord.x - 20.0f, 
                                         screenCoord.y, 
                                         150.0f, 50.0f);
                GUI.Label(labelRect, attribute.Name);
            }
        }

        private void RenderControlAroundGraph(Rect graphRect)
        {
            const float gap = 2f;
            
            var controlsRect = new Rect(graphRect.xMin, 2f, 
                                        graphRect.width, graphRect.yMin - gap * 2f);

            GUILayout.BeginArea(controlsRect);
            GUILayout.BeginHorizontal();

            var sourceNames = Enum.GetNames(typeof(Source));
            m_currentSource =  (Source)GUILayout.SelectionGrid((int)m_currentSource, 
                                                               sourceNames, 
                                                               sourceNames.Length);
            GUILayout.EndHorizontal();
            GUILayout.EndArea();

            
        }

        private void OnPostRender()
        {
        }

        private Vector2 CalculateScreenCoord(float value, 
                                             Rect graphScreenRect)
        {
            return new Vector2(
                graphScreenRect.xMax,
                graphScreenRect.yMax - graphScreenRect.height * value
                );
        }

        private int HistoryOffsetToIndex(int offset)
        {
            return offset;
        }

        private IEnumerator<Color> ColorGenerator()
        {
            var colorRand = new System.Random(5);

            while(true)
            {
                var colour = new Color((float)colorRand.NextDouble(),
                                       (float)colorRand.NextDouble(),
                                       (float)colorRand.NextDouble(),
                                       0.85f);
                yield return colour;
            }
        }
    }
}