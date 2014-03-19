
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
        private Queue<string> m_actionsHistory = new Queue<string>();
        private int m_currentHistorySize;

        enum Source
        {
            Attributes,
            Scores,
        }
        private Source m_currentSource = Source.Attributes;

        private string m_pendingAction;

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

        private void Start()
        {
            var decisionMaker = 
                transform.parent.GetComponentInChildren<TenPN.DecisionFlex.DecisionMaker>();
            decisionMaker.OnNewAction += OnAction;
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
            m_currentHistorySize = Mathf.Min(m_currentHistorySize + 1, m_historySize);

            foreach(var attributeHistory in m_attributeHistories)
            {
                var attribute = attributeHistory.Key;
                var history = attributeHistory.Value;

                PushWithRestrictedSize(attribute.Value, history);
            }

            PushWithRestrictedSize(m_pendingAction, m_actionsHistory);
            m_pendingAction = null;
        }

        private void PushWithRestrictedSize<T>(T data, Queue<T> store)
        {
            store.Enqueue(data);
            while (store.Count > m_historySize)
            {
                store.Dequeue();
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

            RenderActionsOnGraph(graphRect);
        }

        private void RenderActionsOnGraph(Rect graphRect)
        {
            int historyIndex = 0;
            foreach(var action in m_actionsHistory)
            {
                if (action != null)
                {
                    var actionScreenPos = 
                        CalculateScreenCoord(historyIndex, 0f, graphRect);
                    var actionLabelRect = new Rect(actionScreenPos.x - 20f,
                                                   actionScreenPos.y + 10f,
                                                   150f, 50f);
                    GUI.Label(actionLabelRect, action);
                }

                ++historyIndex;
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
                var screenCoord = CalculateScreenCoord(m_currentHistorySize - 1, 
                                                       lastValue, 
                                                       baseGraph.ScreenBounds);

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

        private void OnAction(float score, 
                              GameObject actionsObject, 
                              IConsiderationContext context)
        {
            m_pendingAction = actionsObject.name;
        }

        private Vector2 CalculateScreenCoord(int historyIndex,
                                             float value,
                                             Rect graphScreenRect)
        {
            return new Vector2(
                graphScreenRect.x 
                + (historyIndex + 1)/((float)m_currentHistorySize) * graphScreenRect.width,
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