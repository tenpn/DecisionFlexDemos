using UnityEngine;

namespace TenPN.DecisionFlex.Demos
{
    [AddComponentMenu("TenPN/DecisionFlex/Demos/Timesliced/HPBarController")]
    public class HPBarController : MonoBehaviour
    {
        //////////////////////////////////////////////////

        [SerializeField]
        private Transform m_fillBar;

        private Grunt m_grunt;

        //////////////////////////////////////////////////

        void Start()
        {
            m_grunt = transform.parent.GetComponent<Grunt>();
            m_grunt.HPChange += OnHPChange;
        }

        void OnDestroy()
        {
            if (m_grunt != null)
            {
                m_grunt.HPChange -= OnHPChange;
            }
        }

        void OnHPChange()
        {
            float fillWidth = m_grunt.NormalizedHP;
            var fillLocalScale = m_fillBar.localScale;
            fillLocalScale.x = fillWidth;
            m_fillBar.localScale = fillLocalScale;

            float fillRightOffset = (1 - m_grunt.NormalizedHP) * -0.5f;
            var fillLocalPos = m_fillBar.localPosition;
            fillLocalPos.x = fillRightOffset;
            m_fillBar.localPosition = fillLocalPos;
        }
    }
}