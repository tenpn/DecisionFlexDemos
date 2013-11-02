
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace TenPN.DecisionFlex.Demos
{

    public class AttributeContextFactory : ConsiderationContextFactory
    {
        // return all person attributes
        public override IEnumerable<IConsiderationContext> AllContexts(Logging loggingSetting)
        {
            var context = new ConsiderationContextDictionary();

            foreach(var attribute in m_allAttributes)
            {
                context.SetContext(attribute.Name, attribute.Value);
            }

            yield return context;
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