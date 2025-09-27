using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;


[BurstCompile]
public struct ArmyMoveSystem
{
    LayerMask layerMask;
    NativeArray<Vector3> locations;
    
    public void Start(ref NativeArray<Vector3> locations)
    {
        this.locations = locations;
        layerMask = LayerMask.GetMask("Terrain");
        for (int i = 0; i < locations.Length; i++)
        {
            // RayCastToGround(500, i);
        }
    }
    [BurstCompile]
    public void Update()
    {
        var results = new NativeArray<RaycastHit>(locations.Length, Allocator.TempJob);
        RayCastToGround(ref results);
        for (int i = 0; i < locations.Length; i++)
        {
            locations[i] = results[i].point;
            locations[i] += new Vector3(0, 0, Time.deltaTime);
        }
        results.Dispose();
    }
    [BurstCompile]
    void RayCastToGround(ref NativeArray<RaycastHit> result){
        // Allocate raycast commands and results
        var commands = new NativeArray<RaycastCommand>(locations.Length, Allocator.TempJob);
        QueryParameters queryParameters = new QueryParameters
        {
            layerMask = layerMask,
            hitMultipleFaces = false,
            hitBackfaces = false,
            hitTriggers = QueryTriggerInteraction.Ignore
        };
        for (int i = 0; i < locations.Length; i++)
        {
            Vector3 origin = locations[i] + Vector3.up;
            commands[i] = new RaycastCommand(origin, Vector3.down, queryParameters);
        }
        JobHandle handle = RaycastCommand.ScheduleBatch(commands, result, 1, default);
        handle.Complete();
        commands.Dispose();
    }
}
