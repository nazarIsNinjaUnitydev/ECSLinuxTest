using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using DefaultNamespace;
using Unity.Entities;
using UnityEngine;

namespace Network.Core
{
    public sealed class SocketListener:BaseSocket
    {
        private const int SendTimeout = 5000;

        private Socket _listener;
        private readonly ManualResetEvent _allDone = new ManualResetEvent(false);
        private readonly List<StateObject> _stateObjects = new List<StateObject>(6);
        
        public List<StateObject> StateObjects => _stateObjects;

        public SocketListener()
        {
            _entityManager = World.Active.EntityManager;
            _entityArchetype = _entityManager.CreateArchetype
            (
                typeof(MessageComponent)
            );
        }

        public void StartClientListening(IPAddress ipAddress, int port)
        {
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);

            _listener = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            UnityMainThreadDispatcher.Instance.Enqueue(() =>
            {
                Debug.Log($"Open listener IP: {ipAddress}, Port: {port}");
            });
            

            try
            {
                _listener.Bind(localEndPoint);
                _listener.Listen(100);

                while (true)
                {
                    _allDone.Reset();

                    UnityMainThreadDispatcher.Instance.Enqueue(() =>
                    {
                        Debug.Log("Waiting for a connection...");
                    });
                    
                    _listener.BeginAccept(AcceptCallback, _listener);
                    _allDone.WaitOne();
                }
            }
            catch (Exception e)
            {
                UnityMainThreadDispatcher.Instance.Enqueue(() =>
                {
                    Debug.LogException(e);
                });
            }
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            _allDone.Set();

            Socket handler = _listener.EndAccept(ar);

            StateObject state = new StateObject {WorkSocket = handler};
                                    
            state.WorkSocket.SendTimeout = SendTimeout;
            _stateObjects.Add(state);
            
            handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, ReadCallback, state);
        }
    }
}