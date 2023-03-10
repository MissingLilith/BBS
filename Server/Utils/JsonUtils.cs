using System.Text.Json;

namespace SchoolBBS.Server.Utils
{
    public static class JsonUtils
    {
        public static string ToJson(this object obj)
        {
            var json=JsonSerializer.Serialize(obj,new JsonSerializerOptions
            {
                Encoder=System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented= true,
                ReferenceHandler=System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
            });
            return json;
        }
    }
}
