using EncryptionAttributes;
using System.IO;

namespace ShipBattleAngularApi.Web.Extensions
{
    public static class Serializer
    {
        public static string Serialize<T>(T entity) where T : class
        {
            using (StringWriter stream = new StringWriter())
            {
                var serializer = new MyXmlSerializer(typeof(T));
                serializer.Serialize(stream, entity);
                return stream.ToString();
            }
        }

        public static T Deserialize<T>(string entity, string nameAssemblyModel = null) where T : class
        {
            using (StringReader sr = new StringReader(entity))
            {
                var serializer = new MyXmlSerializer(typeof(T), nameAssemblyModel);
                return serializer.Deserialize(sr) as T;
            }
        }
    }
}
