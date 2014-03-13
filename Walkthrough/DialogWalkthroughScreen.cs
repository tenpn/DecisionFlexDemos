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

            RenderContents();

            GUILayout.Label(m_dialog);
        }

        protected virtual void RenderContents() { }

        //////////////////////////////////////////////////

        [SerializeField] string m_title;
        [Multiline] [SerializeField] string m_dialog;

        //////////////////////////////////////////////////
    }
}