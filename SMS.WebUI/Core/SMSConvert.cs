using Newtonsoft.Json;
using SMS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMS.WebUI.Core
{
    public class SMSConvert
    {
        public static string SMSJsonSerialize(object data)
        {
            string json = JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return json;
        }
        public static UserDTO SMSJsonDesirializeUserDTO(string data)
        {
            return JsonConvert.DeserializeObject<UserDTO>(data);
        }
    }
}
