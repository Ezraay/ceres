using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ceres.Core.Utility.Json
{
    [JsonArray]
    public class SerializableDictionary<T,U> : Dictionary<T,U>
    {
    }
}