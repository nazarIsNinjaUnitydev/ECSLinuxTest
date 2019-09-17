using System;
using System.Net;
using System.Threading.Tasks;
using Network.Core;
using UnityEngine;

namespace DefaultNamespace
{
    public class Testing:MonoBehaviour
    {
        private void Awake()
        {
            try
            {
                var hostListener = new SocketListener();
                
                int port = 27015;
                IPAddress ipAddress = IPAddress.Parse("192.168.88.49");
                Task task = new Task(() => hostListener.StartClientListening(ipAddress, port));
                task.Start();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}