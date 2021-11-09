using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class SpawnerCoxinelle : MonoBehaviour
{
    [System.Serializable]
    private struct ZoneSpawn
    {
        public Vector2 position;
        public Vector2 etendue;

        public ZoneSpawn(Vector2 _position, Vector2 _etendue)
        {
            position = _position;
            etendue = _etendue;
        }
    }

    [SerializeField] private Coxinelle coxiBase;
    [SerializeField] private List<ZoneSpawn> zonesSpawn = new List<ZoneSpawn>();
    [SerializeField] private float tmpsMinEntreSpawn;
    [SerializeField] private float tmpsMaxEntreSpawn;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (var zoneSpawn in zonesSpawn)
        {
            Gizmos.DrawWireCube(zoneSpawn.position,zoneSpawn.etendue);
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnCoolDown());
    }

    private IEnumerator SpawnCoolDown()
    {
        yield return new WaitForSeconds(Random.Range(tmpsMinEntreSpawn, tmpsMaxEntreSpawn));

        if (ListePucerons.Singleton.LesPuceronsLibres != null) SpawnCoxi();
        StartCoroutine(SpawnCoolDown());
    }

    private void SpawnCoxi()
    {
        if (!coxiBase)
        {
            Debug.LogError("Wesh t'as pas assigner de coxinelle par defo pour le spawner");
            return;
        }

        ZoneSpawn zoneSpawnAlea = zonesSpawn[Random.Range(0, zonesSpawn.Count)];
        Vector3 posAlea = new Vector3
        {
            x =Random.Range(zoneSpawnAlea.position.x - zoneSpawnAlea.etendue.x / 2,
                zoneSpawnAlea.position.x + zoneSpawnAlea.etendue.x / 2),

            y = Random.Range(zoneSpawnAlea.position.y - zoneSpawnAlea.etendue.y / 2,
                zoneSpawnAlea.position.y + zoneSpawnAlea.etendue.y / 2),
            z = transform.position.z
        };

        if (Instantiate(coxiBase.gameObject, posAlea, new Quaternion(), transform).TryGetComponent(out Coxinelle nvlleCoxi))
        {
            nvlleCoxi.AssignerPointSpawn(posAlea);
            nvlleCoxi.AssignerPuceron(ListePucerons.RecupPuceronAleatoire());
        }
    }
}
