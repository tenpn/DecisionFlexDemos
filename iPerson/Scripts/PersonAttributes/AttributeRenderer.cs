
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace TenPN.DecisionFlex.Demos
{
    /** graph all attributes over time, on screen */
    [AddComponentMenu("TenPN/DecisionFlex/Demos/iPerson/Attribute Renderer")]
    public class AttributeRenderer : MonoBehaviour
    {
        //////////////////////////////////////////////////

        [Range(0f,2f)]
        [SerializeField] private float m_sampleInterval;

        [SerializeField] private int m_historySize = 50;

        private Dictionary<PersonAttribute, Queue<float>> m_attributeHistories;
        private Dictionary<GameObject, Queue<float>> m_actionScoreHistories = 
            new Dictionary<GameObject, Queue<float>>();
        private Queue<string> m_actionsHistory = new Queue<string>();
        private int m_currentHistorySize;

        enum Source
        {
            Attributes,
            Scores,
        }
        private Source m_currentSource = Source.Attributes;

        private string m_pendingAction;
        private DecisionMaker m_decisionMaker;
        bool m_isPaused = false;

        //////////////////////////////////////////////////

        private void Awake()
        {
            var attributes = GetComponentsInChildren<PersonAttribute>();

            m_attributeHistories = new Dictionary<PersonAttribute, Queue<float>>();
            foreach(var attribute in attributes)
            {
                m_attributeHistories[attribute] = new Queue<float>();
            }
        }

        private void Start()
        {
            m_decisionMaker = transform.parent.GetComponentInChildren<DecisionMaker>();
            m_decisionMaker.OnNewAction += OnAction;

            var allActions = transform.parent.GetComponentsInChildren<Action>();
            foreach(var action in allActions)
            {
                m_actionScoreHistories[action.gameObject] = new Queue<float>();
            }

            StartCoroutine(Sampler());
        }

        private bool IsPaused
        {
            get
            {
                return m_isPaused;
            }
            set
            {
                if (m_isPaused == value)
                {
                    return;
                }

                Time.timeScale = value ? 0f : 1f;
                m_isPaused = value;
            }
        }

        private IEnumerator Sampler()
        {
            // let's run some early decisions to fill out data
            m_currentHistorySize = 3;
            for(int warmupIndex = 0; warmupIndex < m_currentHistorySize; ++warmupIndex) {
                m_decisionMaker.PerformAction();
                RecordActionScores();
                RecordAttributes();
            }

            while(true)
            {
                // could use InvokeRepeating here, but then you couldn't
                // fiddle with the sample interval in the editor during play.
                m_currentHistorySize = Mathf.Min(m_currentHistorySize + 1, m_historySize);
                RecordActionScores();
                RecordAttributes();
                yield return new WaitForSeconds(m_sampleInterval);
                yield return new WaitForEndOfFrame();
            }
        }

        private void RecordAttributes()
        {
            foreach(var attributeHistory in m_attributeHistories)
            {
                var attribute = attributeHistory.Key;
                var history = attributeHistory.Value;

                PushWithRestrictedSize(attribute.Value, history);
            }

            PushWithRestrictedSize(m_pendingAction, m_actionsHistory);
            m_pendingAction = null;
        }

        private void RecordActionScores()
        {
            var allActionScores = m_decisionMaker.AllLastSelections;
            bool isActionTurn = string.IsNullOrEmpty(m_pendingAction) == false;
            
            foreach(var actionScore in allActionScores)
            {
                var action = actionScore.ActionObject;

                Queue<float> recentActionScores = m_actionScoreHistories[action];

                float scoreValue = isActionTurn ? actionScore.Score : 0f;
                PushWithRestrictedSize(scoreValue, recentActionScores);
            }
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
                RenderHistories(m_attributeHistories, baseGraph, att => att.Name);
            }
            else
            {
                RenderHistories(m_actionScoreHistories, baseGraph, act => act.name);
            }

            RenderActionsOnGraph(graphRect);
        }

        private void RenderActionsOnGraph(Rect graphRect)
        {
            // render decisions label vertically
            var pos = new Vector2(graphRect.x - 20f, Screen.height - 5f);
            var quat = Quaternion.identity; 
            quat.eulerAngles = new Vector3(0f, 0f, -90f);
            var oldMatrix = GUI.matrix;
            GUI.matrix = Matrix4x4.TRS(pos, quat, Vector3.one); 

            var actionsLabelRect = new Rect(0f, 0f,
                                            150f, 50f);
            GUI.Label(actionsLabelRect, "Decisions");

            GUI.matrix = oldMatrix;

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

        private void RenderHistories<T>(Dictionary<T,Queue<float>> histories, 
                                        GraphParameters baseGraph,
                                        Func<T,string> toString)
        {
            var yValuesList = histories.Values.Select(
                yValues => yValues.ToArray())
                .ToArray();
            baseGraph.YValuesList = yValuesList;
            GraphRenderer.Instance.RenderGraph(baseGraph);

            foreach(var history in histories)
            {
                var key = history.Key;
                var scores = history.Value;
            
                var lastInterestingIndexValue = LastNotableValueIn(scores);
                var screenCoord = CalculateScreenCoord(lastInterestingIndexValue.Key,
                                                       lastInterestingIndexValue.Value,
                                                       baseGraph.ScreenBounds);

                var labelRect = new Rect(screenCoord.x - 20.0f, 
                                         screenCoord.y, 
                                         150.0f, 50.0f);
                GUI.Label(labelRect, toString(key));
            }
        }

        private KeyValuePair<int,float> LastNotableValueIn(IEnumerable<float> values)
        {
            int index = values.Count();
            foreach(var value in values.Reverse())
            {
                --index;
                if (value > 0f)
                {
                    return new KeyValuePair<int,float>(index, value);
                }
            }

            // just go with last value
            return new KeyValuePair<int,float>(values.Count()-1, values.Last());
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

            GUILayout.FlexibleSpace();

            IsPaused = GUILayout.Toggle(IsPaused, "Pause");

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