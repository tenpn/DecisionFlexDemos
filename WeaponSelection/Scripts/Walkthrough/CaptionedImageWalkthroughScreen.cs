using UnityEngine;

namespace TenPN.DecisionFlex.Demos.Walkthrough
{
    public class CaptionedImageWalkthroughScreen : DialogWalkthroughScreen
    {
        protected override void RenderContents()
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(m_image);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        //////////////////////////////////////////////////

        [SerializeField] Texture m_image;

        //////////////////////////////////////////////////
    }
}