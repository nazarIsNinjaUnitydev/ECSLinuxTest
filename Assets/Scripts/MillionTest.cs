using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class MillionTest:MonoBehaviour
    {
        [SerializeField]
        private Material _material;
        [SerializeField]
        private Mesh _mesh;
        private void Awake()
        {
            //CreateCube();
            var entityManager = World.Active.EntityManager;
            
            EntityArchetype entityArchetype = entityManager.CreateArchetype
            (
                typeof(Translation),
                typeof(RotateComponent),
                typeof(MoveComponent),
                typeof(LocalToWorld)
            );

           // RenderMesh cubeRenderer = GameObject.FindObjectOfType<RenderMeshProxy>().Value;
            
            RenderMesh cubeRenderer = new RenderMesh
            {
                mesh = _mesh,
                material = _material,
                subMesh = 0,
                castShadows = ShadowCastingMode.Off,
                receiveShadows = false
            };
            
            NativeArray<Entity> entities = new NativeArray<Entity>(100000, Allocator.Temp);
            entityManager.CreateEntity(entityArchetype, entities);

            for (int i = 0; i < entities.Length; i++)
            {
                var entity = entities[i];
                entityManager.SetComponentData(entity, new Translation{Value =  new float3()});
                entityManager.SetComponentData(entity, new RotateComponent{speed = 0f});
                entityManager.SetComponentData(entity, new MoveComponent(){radius = Random.value*5 +1, speed = Random.value *3 +1 });
                
                entityManager.AddSharedComponentData(entity, cubeRenderer);
            }
        }
        
        private void CreateCube () {
            Vector3[] vertices = {
                new Vector3 (0, 0, 0),
                new Vector3 (1, 0, 0),
                new Vector3 (1, 1, 0),
                new Vector3 (0, 1, 0),
                new Vector3 (0, 1, 1),
                new Vector3 (1, 1, 1),
                new Vector3 (1, 0, 1),
                new Vector3 (0, 0, 1),
            };

            int[] triangles = {
                0, 2, 1, //face front
                0, 3, 2,
                2, 3, 4, //face top
                2, 4, 5,
                1, 2, 5, //face right
                1, 5, 6,
                0, 7, 4, //face left
                0, 4, 3,
                5, 4, 7, //face back
                5, 7, 6,
                0, 6, 7, //face bottom
                0, 1, 6
            };
			
            _mesh = new Mesh();
            _mesh.Clear ();
            _mesh.vertices = vertices;
            _mesh.triangles = triangles;
            _mesh.Optimize ();
            _mesh.RecalculateNormals ();
        }

        int frameCount = 0;
        double dt = 0.0;
        double fps = 0.0;
        double updateRate = 0.5;  // 4 updates per sec.
 
        private void  Update()
        {
            frameCount++;
            dt += Time.deltaTime;
            if (dt > 1.0/updateRate)
            {
                fps = frameCount / dt ;
                frameCount = 0;
                dt -= 1.0/updateRate;
                Debug.Log(fps);
            }
        }
    }
}