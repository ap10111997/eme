﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.Requests.chat
{
    public class GetChatsRequest
    {
        [Range(1, int.MaxValue)]
        public int Part { get; set; }
    }
}
