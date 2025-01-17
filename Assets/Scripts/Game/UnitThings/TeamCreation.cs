using UnityEngine;

public class TeamCreation : MonoBehaviour
{
    [SerializeField] int teamSize;
    [SerializeField] GameObject prefab;
    void Start()
    {
        for(int rows = 0; rows < teamSize; rows++){
            for(int collum = 0; collum < teamSize; collum++){
                Transform spawnLocation = this.transform;
                Vector3 offset = new Vector3(-collum, 0, rows);
                spawnLocation.position -= offset;
                GameObject unitSpawned = Instantiate(prefab, spawnLocation);
                unitSpawned.name = prefab.name + "_" + rows + "_" + collum;
            }
        }
    }
}
