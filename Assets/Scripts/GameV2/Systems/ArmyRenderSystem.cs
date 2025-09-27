using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;


[System.Serializable]
public struct ArmyRenderSystem
{
    [SerializeField] Mesh meshToRender;
    [SerializeField] Material material;
    NativeArray<Vector3> locations;
    Matrix4x4[] matrix4X4;
    [BurstCompile]
    public void Start(ref NativeArray<Vector3> locations)
    {
        this.locations = locations;
        matrix4X4 = new Matrix4x4[locations.Length];
    }
    [BurstCompile]
    public void Update()
    {
        for (int i = 0; i < locations.Length; i++)
        {
            matrix4X4[i] = float4x4.TRS(locations[i], Quaternion.identity, Vector3.one);
        }
        Graphics.DrawMeshInstanced(meshToRender, 0, material, matrix4X4);
    }
    
}
