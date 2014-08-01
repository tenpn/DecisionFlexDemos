
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace TenPN.DecisionFlex.Demos
{
    /**
       \brief
       for the iPerson demo. Pushes the iPerson attributes into a single IConsiderationContext
    */
    [AddComponentMenu("TenPN/DecisionFlex/Demos/iPerson/Attribute ContextFactory")]
    public class AttributeContextFactory : SingleConsiderationContextFactory
    {
        // return all person attributes
        public override IConsiderationContext SingleContext(Logging loggingSetting)
        {
            var context = new FastConsiderationContextDictionary();

            foreach(var attribute in m_allAttributes)
            {
                context.SetContext(attribute.Name, attribute.Value);
            }

            return context;
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