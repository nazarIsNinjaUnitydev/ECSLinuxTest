using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using DefaultNamespace;
using Unity.Entities;
using UnityEngine;
using Utils;

namespace Network.Core
{
    public abstract class BaseSocket : IDisposable
    {
        public readonly ManualResetEvent ReceiveDone =
            new ManualResetEvent(false);

        protected EntityManager _entityManager;
        protected EntityArchetype _entityArchetype;
        public virtual void Dispose()
        {
            ReceiveDone?.Dispose();
        }

        protected void ReadCallback(IAsyncResult ar)
        {
            StateObject state = (StateObject) ar.AsyncState;
            Socket handler = state.WorkSocket;

            if (!handler.Connected)
            {
                UnityMainThreadDispatcher.Instance.Enqueue(() =>
                {
                    Debug.Log("CHO-TO STRASHNOE (Recive data when socket disposed) ");
                });
               
                return;
            }

            var bytesRead = handler.EndReceive(ar);

            if (bytesRead <= 0) return;

            state.Sb.Append(Encoding.ASCII.GetString(state.Buffer, 0, bytesRead));

            var indexOf = state.Sb.IndexOf("<EOF>", 0, true);
            while (indexOf > -1)
            {
                var tmpContent = state.Sb.ToString(0, indexOf + 5);
                state.Sb.Remove(0, tmpContent.Length);
                //обработка сообщения
                UnityMainThreadDispatcher.Instance.Enqueue(() =>
                {
                    var entity = _entityManager.CreateEntity(_entityArchetype);
                    _entityManager.SetSharedComponentData(entity, new MessageComponent(){message = tmpContent});
                });
                indexOf = state.Sb.IndexOf("<EOF>", 0, true);
            }

            handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, ReadCallback, state);
        }
    }
    public class StateObject
    {
        public Socket WorkSocket = null;  
        public const int BufferSize = 1044480;
        public byte[] Buffer = new byte[BufferSize];
        public StringBuilder Sb = new StringBuilder();
    }  
}