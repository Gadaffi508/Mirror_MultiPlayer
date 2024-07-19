using System.Runtime.CompilerServices;
using Unity.Entities;
using Unity.Mathematics;

namespace Optimization.AC
{
    public partial class ECSMovementSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float deltaTime = UnityEngine.Time.deltaTime;
            Entities.ForEach((ref Transforms position) =>
            {
                position = new Transforms()
                {
                    Value = position.Value + position.Value1 * deltaTime
                };

            }).ScheduleParallel();
        }
    }
    
    public struct Transforms : IComponentData
    {
        public float3 Value;
        public float Value1;
    }
}
