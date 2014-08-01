using UnityEngine;
using System;

namespace TenPN.DecisionFlex.Demos
{
    [AddComponentMenu("TenPN/DecisionFlex/Demos/MainMenu")]
    public class MainMenu : MonoBehaviour
    {
        //////////////////////////////////////////////////

        //////////////////////////////////////////////////

        private void OnGUI()
        {
            const float menuWidthProp = 0.5f;
            float menuWidth = Screen.width * menuWidthProp;
            float menuLeft = 0.5f * (Screen.width - menuWidth);

            const float menuHeightProp = 0.8f;
            float menuHeight = Screen.height * menuHeightProp;
            float menuTop = 0.5f * (Screen.height - menuHeight);

            var menuRect = new Rect(menuLeft, menuTop, 
                                    menuWidth, menuHeight);

            GUILayout.BeginArea(menuRect, GUI.skin.box);
            GUILayout.BeginVertical();

            Center(() => GUILayout.Label("DecisionFlex Demo"));

            GUILayout.FlexibleSpace();
            Center(() => GUILayout.Label("Choose a scene:"));

            var loadLevelLayout = GUILayout.MinWidth(menuWidth * 0.9f);
            var levelNames = new string[] { 
                "DecisionFlex_WeaponSelection", 
                "DecisionFlex_iPerson", 
                "DecisionFlex_Grunts" 
            };
            foreach(var levelName in levelNames)
            {
                Center(() => { 
                        var publicName = levelName.Replace("DecisionFlex_", "");
                        if (GUILayout.Button(publicName, loadLevelLayout))
                        {
                            Application.LoadLevel(levelName);
                        }
                    });
            }
            GUILayout.FlexibleSpace();

            GUILayout.EndVertical();
            GUILayout.EndArea();
        }

        void Center(System.Action controls)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            controls();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }
}