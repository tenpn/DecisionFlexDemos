using UnityEngine;

namespace TenPN.DecisionFlex.Demos.Walkthrough
{
    public class DialogWalkthroughScreen : WalkthroughScreen
    {
        public override void RenderScreen()
        {
            // center title. is this the best way? who knows!
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(m_title);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Label(m_dialog);
        }

        //////////////////////////////////////////////////

        [SerializeField] string m_title;
        [Multiline] [SerializeField] string m_dialog;

        //////////////////////////////////////////////////
    }
}