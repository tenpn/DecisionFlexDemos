using UnityEngine;
using System.Collections.Generic;

namespace TenPN.DecisionFlex.Demos.Walkthrough
{
    public class Walkthrough : MonoBehaviour {
        
        //////////////////////////////////////////////////

        int m_currentScreenIndex = 0;
        IList<WalkthroughScreen> m_screens;
        bool m_isMaximised = true;

        [SerializeField] float m_widthProp = 0.75f;
        [SerializeField] float m_heightProp = 0.75f;
        [SerializeField] float m_minimisedHeight = 30f;

        //////////////////////////////////////////////////

        private void Start()
        {
            m_screens = GetComponentsInChildren<WalkthroughScreen>();
        }

        private void OnGUI()
        {
            if (m_isMaximised)
            {
                RenderMaximisedDialog();
            }
            else
            {
                RenderMinimisedDialog();
            }
        }

        private void RenderMinimisedDialog()
        {
            float bgWidth = Screen.width * m_widthProp;
            float bgLeftX = (Screen.width - bgWidth) * 0.5f;
            float bgTopY = Screen.height - m_minimisedHeight;
            var minimisedRect = new Rect(bgLeftX, bgTopY,
                                         bgWidth, m_minimisedHeight);
            GUILayout.BeginArea(minimisedRect, GUI.skin.box);
            
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Maximise"))
            {
                m_isMaximised = true;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            GUILayout.EndArea();
        }

        private void RenderMaximisedDialog()
        {
            float bgWidth = Screen.width * m_widthProp;
            float bgLeftX = (Screen.width - bgWidth) * 0.5f;
            float bgHeight = Screen.height * m_heightProp;
            float bgTopY = (Screen.height - bgHeight) * 0.5f;
            var bgRect = new Rect(bgLeftX, bgTopY,
                                  bgWidth, bgHeight);

            GUILayout.BeginArea(bgRect, GUI.skin.box);

            GUILayout.BeginVertical();

            m_screens[m_currentScreenIndex].RenderScreen();

            GUILayout.FlexibleSpace();

            RenderControls();
            
            GUILayout.EndVertical();
            GUILayout.EndArea();

        }

        private void RenderControls()
        {
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Minimise"))
            {
                m_isMaximised = false;
            }

            GUILayout.FlexibleSpace();
            
            bool isNotFirstScreen = m_currentScreenIndex > 0;
            GUI.enabled = isNotFirstScreen;
            if (GUILayout.Button("Prev"))
            {
                --m_currentScreenIndex;
            }

            bool isNotLastScreen = m_currentScreenIndex < m_screens.Count - 1;
            GUI.enabled = isNotLastScreen;
            if (GUILayout.Button("Next"))
            {
                ++m_currentScreenIndex;
            }

            GUI.enabled = true;

            GUILayout.EndHorizontal();            
        }
    }
}