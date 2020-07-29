using EMessagesNew2.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMessagesNew2.Models.External
{
    /// <summary>
    /// Представляет внешнюю модель параметра уведомления
    /// </summary>
    public class ExtUserEventParam
    {
        public string Value { get; set; }
        public string Alias { get; set; }

        public static explicit operator ExtUserEventParam(UserEventParam eventParam)
        {
            if (eventParam == null)
            {
                return null;
            }

            return new ExtUserEventParam()
            {
                Alias = eventParam.Alias,
                Value = eventParam.Value
            };
        }
    }
}
