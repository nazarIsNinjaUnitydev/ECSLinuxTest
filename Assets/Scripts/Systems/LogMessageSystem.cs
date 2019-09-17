//using System.Collections.Generic;
//using Unity.Entities;
//using UnityEngine;
//
//namespace DefaultNamespace
//{
//    public class LogMessageSystem:ComponentSystem
//    {
//        protected override void OnUpdate()
//        {
//            List<MessageComponent> messageComponents = new List<MessageComponent>();
//            EntityManager.GetAllUniqueSharedComponentData(messageComponents);
//            foreach (var messageComponent in messageComponents)
//            {
//                if(messageComponent.message!=null)
//                    Debug.Log(messageComponent.message);    
//            }
//        }
//    }
//}