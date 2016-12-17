/*
The MIT License (MIT)

Copyright (c) 2014 Andrew Fray

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
 */

using UnityEngine;
using System.Reflection;
using System.Collections.Generic;

namespace TenPN.DecisionFlex.Demos
{
    /** holds ammo for solider */
    [AddComponentMenu("TenPN/DecisionFlex/Demos/Weapon Selection/Inventory")]
    public class Inventory : MonoBehaviour
    {
        public struct Ammo
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

        public IEnumerable<Ammo> CalculateInventoryContents()
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

        //////////////////////////////////////////////////

        [SerializeField] int m_shotgunShells;
        [SerializeField] int m_fiftyCalRounds;

        //////////////////////////////////////////////////

        private void OnGUI()
        {
            float inventoryUIHeight = 80f;
            float inventoryUIWidth = 200f;
            float inventoryUIPadding = 10f;

            float inventoryUIRectX = inventoryUIPadding;
            float inventoryUIRectY = Screen.height - inventoryUIPadding - inventoryUIHeight;

            var inventoryUIRect = new Rect(inventoryUIRectX, inventoryUIRectY,
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
