
using UnityEngine;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace TenPN.DecisionFlex.Demos
{
    public class Inventory : MonoBehaviour
    {
        //////////////////////////////////////////////////

        [SerializeField] int m_shotgunShells;
        [SerializeField] int m_fiftyCalRounds;

        //////////////////////////////////////////////////

        private struct Ammo
        {
            public string Name;

            public int GetCount()
            {
                return (int) Field.GetValue(Inventory);
            }

            public void SetCount(int newCount)
            {
                Field.SetValue(Inventory, newCount);
            }

            public FieldInfo Field;
            public Inventory Inventory;
        }

        private IEnumerable<Ammo> CalculateInventoryContents()
        {
            var contents = new List<Ammo>();

            var allInventoryFields = GetType().GetFields(
                BindingFlags.NonPublic | BindingFlags.Instance);

            foreach(var inventoryField in allInventoryFields)
            {
                string contextName = inventoryField.Name.StartsWith("m_")
                    ? inventoryField.Name.Substring(2)
                    : inventoryField.Name;

                contents.Add(new Ammo {
                        Name = contextName,
                        Field = inventoryField,
                        Inventory = this,
                    });
            }

            return contents;
        }

        // push our relevent into into master context
        private void PopulateMasterConsiderationContext(IConsiderationContext masterContext)
        {
            var contents = CalculateInventoryContents();

            foreach(var ammo in contents)
            {
                masterContext.SetContext(ammo.Name, ammo.GetCount());
            }
        }

        private void OnGUI()
        {
            var soldierScreenPos = Camera.main.WorldToScreenPoint(transform.position);
            soldierScreenPos.y = Screen.height - soldierScreenPos.y;

            float inventoryUIHeight = 80f;
            float inventoryUIWidth = 200f;
            float inventoryUIRightOffset = 30f;

            var inventoryUIRect = new Rect(soldierScreenPos.x + inventoryUIRightOffset, 
                                           soldierScreenPos.y - inventoryUIHeight,
                                           inventoryUIWidth, inventoryUIHeight);

            GUILayout.BeginArea(inventoryUIRect, GUI.skin.box);

            GUILayout.Label("Ammo");
            GUILayout.FlexibleSpace();

            var contents = CalculateInventoryContents();
            foreach(var ammo in contents)
            {
                GUILayout.BeginHorizontal();

                int currentCount = ammo.GetCount();
                GUILayout.Label(ammo.Name + ": " + currentCount);

                int minAmmo = 0;
                int maxAmmo = 20;
                int newCount = (int)GUILayout.HorizontalSlider(currentCount, 
                                                               minAmmo, maxAmmo);
                ammo.SetCount(newCount);
                                
                GUILayout.EndHorizontal();
            }

            GUILayout.EndArea();
            
        }
    }
}