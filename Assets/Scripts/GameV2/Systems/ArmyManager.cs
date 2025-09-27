using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;


public class ArmyManager : MonoBehaviour
{
    const int ROW_SIZE = 100;
    const int COLUMN_SIZE = 10;
    const int ARMY_SIZE = ROW_SIZE * COLUMN_SIZE;

    [SerializeField] ArmyRenderSystem renderSystem = new();
    [SerializeField] ArmyMoveSystem moveSystem = new();

    NativeArray<Vector3> locations = new ((int)ARMY_SIZE, Allocator.Persistent);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TempSpawn();
        renderSystem.Start(ref locations);
        moveSystem.Start(ref locations);
    }

    // Update is called once per frame
    void Update()
    {
        moveSystem.Update();
        renderSystem.Update();
    }

    
    void TempSpawn()
    {
        Debug.Log("Actual team size" + ARMY_SIZE);

        // locations = new NativeArray<Vector3>((int)ARMY_SIZE, Allocator.Persistent);

        // create army
        for (int z = 0; z < COLUMN_SIZE; z++)
        {
            for (int x = 0; x <= ROW_SIZE -1; x++)
            {
                int index = (z * ROW_SIZE) + x;
                locations[index] = new Vector3(x, 20, z) * 1.5f;
            }
        }
    }
}
