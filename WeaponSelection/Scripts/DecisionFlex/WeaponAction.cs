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

namespace TenPN.DecisionFlex.Demos
{
    /** 
        Does a gunshot, when chosen by DecisionFlex.
    */
    [AddComponentMenu("TenPN/DecisionFlex/Demos/Weapon Selection/WeaponAction")]
    public class WeaponAction : Action
    {
        /**
           Gets the Enemy from the context, then shoots it
        */
        public override void Perform(IContext context)
        {
            // find the target
            var target = context.GetContext<GameObject>("Enemy");
            // fires a "bullet" across the screen
            LabelLerper.LerpLabelFromTo(m_weaponVerb, transform.position, target.transform);
            // make a note that we fired this gun
            transform.parent.parent.GetComponent<WeaponLog>().LogAction(gameObject.name);
        }

        //////////////////////////////////////////////////

        [SerializeField] string m_weaponVerb;

        //////////////////////////////////////////////////
    }
}
