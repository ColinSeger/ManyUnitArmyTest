using UnityEngine;


public class ArmyManager : MonoBehaviour
{
    const int ROW_SIZE = 100;
    const int COLUMN_SIZE = 100;
    const int ARMY_SIZE = ROW_SIZE * COLUMN_SIZE;

    Location[] locations;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    void TempSpawn()
    {
        Debug.Log("Actual team size" + ARMY_SIZE);

        locations = new Location[ARMY_SIZE];

        // create army
        for (int z = 0; z < COLUMN_SIZE; z++)
        {
            for (int x = 0; x <= ROW_SIZE -1; x++)
            {
                locations[(z * ROW_SIZE) + x].position = new Vector3(x, 20, z) * 1.5f;
            }
        }
    }
}
