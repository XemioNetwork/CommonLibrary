using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NetXmlSerializer = System.Xml.Serialization.XmlSerializer;

namespace Xemio.CommonLibrary.Serialization
{
    /// <summary>
    /// An <see cref="ISerializer"/> implementation using the <see cref="NetXmlSerializer"/>.
    /// </summary>
    public class XmlSerializer : ISerializer
    {
        #region Implementation of ISerializer
        /// <summary>
        /// Serializes the given <paramref name="instance" /> into the given <paramref name="stream" />.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="stream">The stream.</param>
        public void Serialize(object instance, Stream stream)
        {
            NetXmlSerializer serializer = new NetXmlSerializer(instance.GetType());
            serializer.Serialize(stream, instance);
        }
        /// <summary>
        /// Deserializes a instance of the given <paramref name="type" /> from the given <paramref name="stream" />.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public object Deserialize(Type type, Stream stream)
        {
            NetXmlSerializer serializer = new NetXmlSerializer(type);
            return serializer.Deserialize(stream);
        }
        #endregion
    }
}
