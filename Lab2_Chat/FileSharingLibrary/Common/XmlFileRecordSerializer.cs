using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace FileSharingLibrary
{
    public class XmlFileRecordSerializer : IMessageSerializer
    {
        public FileRecord Deserialize(byte[] data)
        {
            var formatter = new XmlSerializer(typeof(FileRecord));

            using (var memoryStream = new MemoryStream(data))
            {
                return (FileRecord)formatter.Deserialize(memoryStream);
            }

        }

        public byte[] Serialize(FileRecord message)
        {
            var formatter = new XmlSerializer(typeof(FileRecord));

            using (var memoryStream = new MemoryStream())
            {
                formatter.Serialize(memoryStream, message);
                return memoryStream.ToArray();
            }
        }
    }
}