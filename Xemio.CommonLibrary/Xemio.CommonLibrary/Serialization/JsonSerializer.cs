using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Xemio.CommonLibrary.Serialization
{
    public class JsonSerializer : ISerializer
    {
        #region Implementation of ISerializer
        /// <summary>
        /// Serializes the given <paramref name="instance"/> into the given <paramref name="stream"/>.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="stream">The stream.</param>
        public void Serialize(object instance, Stream stream)
        {
            var serializer = new DataContractJsonSerializer(instance.GetType());
            serializer.WriteObject(stream, instance);
        }
        /// <summary>
        /// Deserializes a instance of the given <paramref name="type"/> from the given <paramref name="stream"/>.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="stream">The stream.</param>
        public object Deserialize(Type type, Stream stream)
        {
            var serializer = new DataContractJsonSerializer(type);
            return serializer.ReadObject(stream);
        }
        #endregion
    }
}
