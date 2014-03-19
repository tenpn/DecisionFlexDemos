using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace TenPN.DecisionFlex.Demos
{
    public class GraphParameters
    {
        public Rect ScreenBounds;
        public Vector2 YValuesMinMax = Vector2.zero;
        public IList<IList<float>> YValuesList;
        public IList<float> XValues;
        public IEnumerator<Color> Cols;
    }

    public class GraphRenderer : MonoBehaviour
    {
        public static GraphRenderer Instance;

        public void RenderGraph(GraphParameters graph)
        {
            m_graphs.Enqueue(graph);
        }

        //////////////////////////////////////////////////

        Queue<GraphParameters> m_graphs = new Queue<GraphParameters>();

        Material s_graphMat;

        Material GraphMat
        {
            get
            {
                if (s_graphMat == null)
                {
                    s_graphMat = new Material("Shader \"Lines/Colored Blended\" { SubShader { Pass { Blend SrcAlpha OneMinusSrcAlpha BindChannels { Bind \"Color\",color } ZWrite On Cull Front Fog { Mode Off } } } }"); 
                    s_graphMat.hideFlags = HideFlags.HideAndDontSave; 
                    s_graphMat.shader.hideFlags = HideFlags.HideAndDontSave;
                }
                return s_graphMat;
            }
        }

        //////////////////////////////////////////////////

        void Awake()
        {
            Instance = this;
        }

        void OnPostRender()
        {
            while(m_graphs.Any())
            {
                var graph = m_graphs.Dequeue();
                PostRenderGraph(graph);
            }
        }

        void PostRenderGraph(GraphParameters graph)
        {
            if (graph.XValues.Count <= 1)
            {
                // not enough to render
                return;
            }

            GL.PushMatrix();
            GraphMat.SetPass(0);
            GL.LoadOrtho();

            float normScreenGraphXMin = graph.ScreenBounds.x/Screen.width;
            float normScreenGraphYMin = (Screen.height - graph.ScreenBounds.y)/Screen.height;
            float normScreenGraphWidth = graph.ScreenBounds.width/Screen.width;
            float normScreenGraphHeight = graph.ScreenBounds.height/Screen.height;

            Rect normalizedGraphScreenRect = 
                new Rect(normScreenGraphXMin, normScreenGraphYMin,
                         normScreenGraphWidth, normScreenGraphHeight);

            bool isYRangeManuallySet = graph.YValuesMinMax != Vector2.zero;
            float minYValue = isYRangeManuallySet ? graph.YValuesMinMax.x
                : graph.YValuesList.SelectMany(yValues => yValues).Min();
            float maxYValue = isYRangeManuallySet ? graph.YValuesMinMax.y
                : graph.YValuesList.SelectMany(yValues => yValues).Max();
            float yScale = normalizedGraphScreenRect.height/(maxYValue - minYValue);

            float xScale = normalizedGraphScreenRect.width
                /(graph.XValues.Last()- graph.XValues[0]);

            Func<float,float,Vector2> graphToGLCoord = (xValue, yValue) => 
                new Vector2(normalizedGraphScreenRect.x + xValue * xScale, 
                            normalizedGraphScreenRect.y - normalizedGraphScreenRect.height 
                            + yValue * yScale);

            // horizontal bars
            GL.Begin(GL.LINES);
            GL.Color(new Color(0.1f, 0.1f, 0.1f, 0.2f));

            GL.Vertex(graphToGLCoord(graph.XValues[0], minYValue));
            GL.Vertex(graphToGLCoord(graph.XValues.Last(), minYValue));
            GL.Vertex(graphToGLCoord(graph.XValues[0], maxYValue));
            GL.Vertex(graphToGLCoord(graph.XValues.Last(), maxYValue));
            GL.End();

            // vertical bars
            GL.Begin(GL.LINES);
            GL.Color(new Color(0.1f, 0.1f, 0.1f, 0.2f));
            foreach(var xValue in graph.XValues)
            {
                GL.Vertex(graphToGLCoord(xValue, minYValue));
                GL.Vertex(graphToGLCoord(xValue, maxYValue));
            }
            GL.End();

            foreach(var yValues in graph.YValuesList)
            {
                if (yValues.Count <= 1)
                {
                    continue;
                }

                GL.Begin(GL.LINES);

                int xIndexOffset = 
                    graph.XValues.Count - Mathf.Min(graph.XValues.Count, yValues.Count);
                
                var prevPoint = graphToGLCoord(graph.XValues[xIndexOffset], yValues[0]);

                graph.Cols.MoveNext();
                var graphColor = graph.Cols.Current;

                for(int valueIndex = 1; valueIndex < yValues.Count; ++valueIndex)
                {
                    GL.Color(graphColor);

                    float xValue = graph.XValues[valueIndex + xIndexOffset];
                    float yValue = yValues[valueIndex];

                    GL.Vertex(prevPoint);
                    var currentPoint = graphToGLCoord(xValue, yValue);
                    GL.Vertex(currentPoint);
                    
                    prevPoint = currentPoint;
                }

                GL.End();
            }

            GL.PopMatrix();
        }


    }
}