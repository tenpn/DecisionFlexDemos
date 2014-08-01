using UnityEngine;
using System.Collections.Generic;

namespace TenPN.DecisionFlex.Demos.Walkthrough
{
    [AddComponentMenu("TenPN/DecisionFlex/Demos/Walkthrough/Walkthrough")]
    public class Walkthrough : MonoBehaviour 
    {
        //////////////////////////////////////////////////

        int m_currentScreenIndex = 0;
        IList<WalkthroughScreen> m_screens;
        bool m_isMaximised = true;
        GUIStyle m_boxStyle;

        [SerializeField] [Range(-1f,1f)] float m_rightShiftProp = 0.0f;
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
            if (m_boxStyle == null)
            {
                m_boxStyle = new GUIStyle(GUI.skin.box);
                m_boxStyle.normal.background = MakeTex(2, 2, new Color(0f, 0f, 0f, 1f));
            }

            if (m_isMaximised)
            {
                RenderMaximisedDialog();
            }
            else
            {
                RenderMinimisedDialog();
            }
        }

        private Vector2 DialogLeftWidth
        {
            get
            {
                float bgWidth = Screen.width * m_widthProp;
                float bgBuffer = (Screen.width - bgWidth) * 0.5f;
                float normalizedProp = (m_rightShiftProp + 1f) * 0.5f;
                float rightShift = Mathf.Lerp(-bgBuffer, bgBuffer, normalizedProp);
                float bgLeftX = bgBuffer + rightShift;
                return new Vector2(bgLeftX, bgWidth);
            }
        }

        private void RenderMinimisedDialog()
        {
            var bgLeftWidth = DialogLeftWidth;
            float bgTopY = 0f;
            var minimisedRect = new Rect(bgLeftWidth.x, bgTopY,
                                         bgLeftWidth.y, m_minimisedHeight);
            GUILayout.BeginArea(minimisedRect, GUI.skin.box);
            
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Maximise"))
            {
                m_isMaximised = true;
            }

            GUILayout.FlexibleSpace();

            RenderProgress();

            GUILayout.EndHorizontal();
            
            GUILayout.EndArea();
        }

        private void RenderMaximisedDialog()
        {
            var bgLeftWidth = DialogLeftWidth;
            float bgHeight = Screen.height * m_heightProp;
            float bgTopY = (Screen.height - bgHeight) * 0.5f;
            var bgRect = new Rect(bgLeftWidth.x, bgTopY,
                                  bgLeftWidth.y, bgHeight);

            GUILayout.BeginArea(bgRect, m_boxStyle);

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

            if (GUILayout.Button("Back to main menu"))
            {
                Time.timeScale = 1f;
                Application.LoadLevel(0);
            }

            GUILayout.FlexibleSpace();
            
            bool isNotFirstScreen = m_currentScreenIndex > 0;
            GUI.enabled = isNotFirstScreen;
            if (GUILayout.Button("Prev"))
            {
                --m_currentScreenIndex;
            }

            GUI.enabled = true;
            RenderProgress();

            bool isNotLastScreen = m_currentScreenIndex < m_screens.Count - 1;
            GUI.enabled = isNotLastScreen;
            if (GUILayout.Button("Next"))
            {
                ++m_currentScreenIndex;
            }

            GUI.enabled = true;

            GUILayout.EndHorizontal();            
        }

        private void RenderProgress()
        {
            var progressText = string.Format("{0}/{1}", 
                                             m_currentScreenIndex + 1,
                                             m_screens.Count);
            GUILayout.Label(progressText);            
        }

        // http://forum.unity3d.com/threads/change-gui-box-color.174609/
        private Texture2D MakeTex( int width, int height, Color col )
        {
            Color[] pix = new Color[width * height];
            for( int i = 0; i < pix.Length; ++i )
            {
                pix[ i ] = col;
            }
            Texture2D result = new Texture2D( width, height );
            result.SetPixels( pix );
            result.Apply();
            return result;
        }

    }
}