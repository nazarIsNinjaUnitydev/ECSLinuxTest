using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace DefaultNamespace.Systems
{
    public class MoveSystem: ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((ref Translation translation, ref MoveComponent moveComponent) =>
                {
                    translation.Value.x = math.sin(Time.time * moveComponent.speed)*moveComponent.radius;
                    translation.Value.y = math.cos(Time.time * moveComponent.speed)*moveComponent.radius;
                });
        }
    }
}