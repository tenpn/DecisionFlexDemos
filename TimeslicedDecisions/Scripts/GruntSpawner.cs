using UnityEngine;

namespace TenPN.DecisionFlex.Demos
{
    [AddComponentMenu("TenPN/DecisionFlex/Demos/Timesliced/GruntSpawner")]
    public class GruntSpawner : MonoBehaviour
    {
        //////////////////////////////////////////////////

        [SerializeField]
        private float m_spawnRate;

        [SerializeField]
        private float m_max;

        [SerializeField]
        private GameObject m_prefab;

        private float m_timeSinceLastSpawn;
        private CircleCollider2D m_spawnZone;

        //////////////////////////////////////////////////

        void Start()
        {
            m_spawnZone = GetComponent<CircleCollider2D>();
        }

        void Update()
        {
            if (transform.childCount >= m_max)
            {
                return;
            }

            m_timeSinceLastSpawn += Time.deltaTime;
            float spawnInterval = 1f/m_spawnRate;

            if (m_timeSinceLastSpawn >= spawnInterval)
            {
                SpawnGrunt();
                m_timeSinceLastSpawn -= spawnInterval;
            }
        }

        void SpawnGrunt()
        {
            var spawnPos = Random.insideUnitCircle.normalized * m_spawnZone.radius;
            var newGrunt = (GameObject)Instantiate(m_prefab, spawnPos, Quaternion.identity);
            newGrunt.transform.parent = transform;
        }
    }
}