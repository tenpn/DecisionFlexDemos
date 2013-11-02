
using UnityEngine;
using System;
using System.Reflection;

namespace TenPN.DecisionFlex.Demos
{

    [AddComponentMenu("Weapon Choice/Inventory")]
    public class Inventory : MonoBehaviour
    {
        //////////////////////////////////////////////////

        [SerializeField] int m_shotgunShells;
        [SerializeField] int m_fiftyCalRounds;

        //////////////////////////////////////////////////

        // push our relevent into into master context
        private void PopulateMasterConsiderationContext(IConsiderationContext masterContext)
        {
            var allInventoryFields = GetType().GetFields(
                BindingFlags.NonPublic | BindingFlags.Instance);

            foreach(var inventoryField in allInventoryFields)
            {
                int inventoryValue = (int)inventoryField.GetValue(this);

                string contextName = inventoryField.Name.StartsWith("m_")
                    ? inventoryField.Name.Substring(2)
                    : inventoryField.Name;

                masterContext.SetContext(contextName,
                                         inventoryValue);
            }

        }
    }
}