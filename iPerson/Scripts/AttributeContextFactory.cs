
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace TenPN.DecisionFlex.Demos
{
    [AddComponentMenu("TenPN/DecisionFlex/Demos/iPerson/Attribute ContextFactory")]
    public class AttributeContextFactory : ConsiderationContextFactory
    {
        // return all person attributes
        public override IList<IConsiderationContext> AllContexts(Logging loggingSetting)
        {
            var context = new ConsiderationContextDictionary();

            foreach(var attribute in m_allAttributes)
            {
                context.SetContext(attribute.Name, attribute.Value);
            }

            return new IConsiderationContext[] { context };
        }

        //////////////////////////////////////////////////

        private IEnumerable<PersonAttribute> m_allAttributes;

        //////////////////////////////////////////////////

        private void Awake()
        {
            m_allAttributes = gameObject.transform.parent
                .GetComponentsInChildren<PersonAttribute>();
        }
    }
}