using Unity.Entities;
 
 namespace DefaultNamespace
 {
     public struct MoveComponent:IComponentData
     {
         public float radius;
         public float speed;
     }
 }