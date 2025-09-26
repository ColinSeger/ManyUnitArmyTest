using UnityEngine;

public class ArmyRenderSystem
{
    [SerializeField] Mesh meshToRender;
    [SerializeField] Material material;
    Location[] locations;
    Matrix4x4[] matrix4X4;
    void Start(uint army_size, ref Location[] locations)
    {
        matrix4X4 = new Matrix4x4[army_size];
    }
    void Update()
    {
        for (int i = 0; i < locations.Length; i++)
        {
            locations[i].position.z += Time.deltaTime;
        }

        for (int i = 0; i < locations.Length; i++)
        {
            matrix4X4[i] = Matrix4x4.TRS(locations[i].position, Quaternion.identity, Vector3.one);
        }
        Graphics.DrawMeshInstanced(meshToRender, 0, material, matrix4X4);
    }
}
