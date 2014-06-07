using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using Action = System.Action;

namespace TenPN.DecisionFlex.Demos
{
    // given text, start T, end T will move label from start to end
    [AddComponentMenu("TenPN/DecisionFlex/Demos/Weapon Selection/LabelLerper")]
    public class LabelLerper : MonoBehaviour
    {
        public static void LerpLabelFromTo(string label, Vector3 fromPos, 
                                           Transform toTransform)
        {
            s_instance.StartCoroutine(
                s_instance.DoLerpLabelFromTo(label, fromPos, toTransform));
        }

        //////////////////////////////////////////////////

        static LabelLerper s_instance;

        [SerializeField] float m_smoothTime = 3f;
        [SerializeField] float m_initialPause = 1f;

        int m_nextID = 0;
        Dictionary<int, System.Action> m_renderers = new Dictionary<int, System.Action>();

        //////////////////////////////////////////////////

        void Awake()
        {
            s_instance = this;
        }

        void OnDestroy()
        {
            s_instance = null;
        }
        
        void OnGUI()
        {
            foreach(var renderer in m_renderers.Values)
            {
                renderer();
            }
        }

        IEnumerator DoLerpLabelFromTo(string label, Vector3 fromPos, Transform toTransform)
        {
            int id = m_nextID;
            ++m_nextID;

            Vector3 renderWorldPos = fromPos;
            Vector3 velocity = Vector3.zero;

            System.Action renderer = () => {
                var labelSize = GUI.skin.box.CalcSize(new GUIContent(label));

                var renderScreenPos = Camera.main.WorldToScreenPoint(renderWorldPos);
                renderScreenPos.y = Screen.height - renderScreenPos.y;

                var labelRect = new Rect(renderScreenPos.x - labelSize.y,
                                         renderScreenPos.y,
                                         labelSize.x, labelSize.y);
                GUI.Box(labelRect, label);
            };
            
            m_renderers[id] = renderer;

            yield return new WaitForSeconds(m_initialPause);

            while(true)
            {
                renderWorldPos = Vector3.SmoothDamp(renderWorldPos, toTransform.position,
                                                    ref velocity, m_smoothTime);
                float sqDistToTarget = (toTransform.position - renderWorldPos).sqrMagnitude;
                if (sqDistToTarget < 0.35f)
                {
                    break;
                }
                yield return null;
            }

            m_renderers.Remove(id);
        }
    }
}