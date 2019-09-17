using System;
using Unity.Entities;

namespace DefaultNamespace
{
    public struct MessageComponent : ISharedComponentData, IEquatable<MessageComponent>
    {
        public string message;

        public bool Equals(MessageComponent other)
        {
            if (message == null)
                return false;
            if (other.message == null)
                return false;
            return message.Equals(other.message);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}