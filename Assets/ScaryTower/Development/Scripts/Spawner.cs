using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Collections;
using Unity.Physics;
using Unity.Physics.Systems;

public class Spawner : MonoBehaviour
{
    //[SerializeField] private GameObject prefab;
    //[SerializeField] private int poolSize;
    //[SerializeField] private float spawnRate;
    //[SerializeField] private float3 spawnRange;

    //private float spawnTimer;
    //private EntityManager entityManager;
    //private Entity prefabEntity;
    //private NativeArray<Entity> entities;
    //private int entityIndex = 0;

    //private void Start()
    //{
    //    entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

    //    // Convert the game object prefab into an entity prefab
    //    GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
    //    prefabEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(prefab, settings);

    //    // Create a pool of entities
    //    entities = new NativeArray<Entity>(poolSize, Allocator.Persistent);
    //    for (int i = 0; i < poolSize; i++)
    //    {
    //        entities[i] = entityManager.Instantiate(prefabEntity);
    //        entityManager.AddComponentData(entities[i], new Translation { Value = new float3(1000f, 1000f, 1000f) });
    //        entityManager.AddComponentData(entities[i], new Rotation { Value = quaternion.identity });
    //        entityManager.AddComponentData(entities[i], new PhysicsVelocity { Linear = float3.zero });
    //        entityManager.AddComponentData(entities[i], new PhysicsCollider { Value = PhysicsCollider.Authoring.CreateCapsule(0.5f, 1f) });
    //        entityManager.AddComponent(entities[i], typeof(RenderBounds));
    //        entityManager.AddComponent(entities[i], typeof(LocalToWorld));
    //        entityManager.AddComponent(entities[i], typeof(PhysicsMass));
    //        entityManager.AddComponent(entities[i], typeof(RenderMesh));
    //        entityManager.SetComponentData(entities[i], new PhysicsMass { InverseMass = 1f });
    //    }
    //}

    //private void Update()
    //{
    //    spawnTimer += Time.deltaTime;

    //    if (spawnTimer > spawnRate)
    //    {
    //        spawnTimer = 0f;

    //        // Spawn a new entity from the pool
    //        Entity entity = entities[entityIndex];
    //        entityManager.SetComponentData(entity, new Translation { Value = transform.position + new float3(UnityEngine.Random.Range(-spawnRange.x, spawnRange.x), UnityEngine.Random.Range(-spawnRange.y, spawnRange.y), UnityEngine.Random.Range(-spawnRange.z, spawnRange.z)) });
    //        entityManager.SetComponentData(entity, new Rotation { Value = quaternion.Euler(UnityEngine.Random.Range(-180f, 180f), UnityEngine.Random.Range(-180f, 180f), UnityEngine.Random.Range(-180f, 180f)) });

    //        entityIndex++;
    //        if (entityIndex >= poolSize)
    //        {
    //            entityIndex = 0;
    //        }
    //    }
    //}

    //private void OnDestroy()
    //{
    //    // Destroy the pool of entities
    //    entities.Dispose();
    //}
}