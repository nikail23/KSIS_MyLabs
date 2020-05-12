using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSharingLibrary
{
    public interface IMessageSerializer
    {
        byte[] Serialize(FileRecord message);

        FileRecord Deserialize(byte[] data);
    }
}
