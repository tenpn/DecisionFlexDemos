using UnityEngine;

namespace TenPN.DecisionFlex.Demos
{
    public class ShowName : MonoBehaviour
    {
        //////////////////////////////////////////////////

        //////////////////////////////////////////////////

        void OnGUI()
        {
            var ourScreenPos = Camera.main.WorldToScreenPoint(transform.position);
            ourScreenPos.y = Screen.height - ourScreenPos.y;

            var label = gameObject.name;
            
            var labelSize = GUI.skin.box.CalcSize(new GUIContent(label));
            var rect = new Rect(ourScreenPos.x - labelSize.x * 0.5f,
                                ourScreenPos.y + 15f,
                                labelSize.x, labelSize.y);
            GUI.Box(rect, label);
        }
    }
}