using Shop_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace Shop_Core.DTOS
{
    public class OrderJsonConverter : JsonConverter<Order>
    {
        public override Order Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // هنا يتم تحويل JSON إلى Order
            return JsonSerializer.Deserialize<Order>(ref reader, options);
        }

        public override void Write(Utf8JsonWriter writer, Order value, JsonSerializerOptions options)
        {
            // هنا يتم كتابة كائن Order إلى JSON
            JsonSerializer.Serialize(writer, value, options);
        }
    }
}
