using Optimization.AC;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class MovementAuthoring : MonoBehaviour
{
    public float3 startPosition;
    public float moveSpeed;

    public void Convert(Entity entity, EntityManager dstManager)
    {
        dstManager.AddComponentData(entity, new Transforms { Value = startPosition });
        dstManager.AddComponentData(entity, new Transforms { Value1 = moveSpeed });
    }

    public class MovementAuthoringBaker : Baker<MovementAuthoring>
    {
        public override void Bake(MovementAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity,
                new Transforms { Value = authoring.startPosition, Value1 = authoring.moveSpeed });
        }
    }
}