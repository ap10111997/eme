using EMessagesNew2.Interfaces;
using EMessagesNew2.Models.External;
using EMessagesNew2.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.Responses.profile
{
    public class GetProfileResponse : IResponse
    {
        public ExtUser UserData { get; set; }
        public List<ExtFileData> Images { get; set; }
        public UserProfile UserProfile { get; set; }
    }
}
