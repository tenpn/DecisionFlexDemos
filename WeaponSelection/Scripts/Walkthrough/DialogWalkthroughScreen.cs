using UnityEngine;

namespace TenPN.DecisionFlex.Demos.Walkthrough
{
    public class DialogWalkthroughScreen : WalkthroughScreen
    {
        public override void RenderScreen()
        {
            GUILayout.Label(m_title);
            GUILayout.Label(m_dialog);
        }

        //////////////////////////////////////////////////

        [SerializeField] string m_title;
        [Multiline] [SerializeField] string m_dialog;

        //////////////////////////////////////////////////
    }
}