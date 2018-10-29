
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace BitMEX_API
{
    internal class Program
    {
        
        private static string bitmexKey = "FO_DwD6WrxiP2oz4dOr-sOlI";
        private static string bitmexSecret = "hOMcB0cin9hMqIMMCJbWAfcK8XkgNFKgBaU1Z6eNe2CAigg4";

        private static void Main(string[] args)
        {
            Program p = new Program();
            p.Run(args);
        }

        private void Run(string[] args)
        {
            string test = "test";
            BitMEXApi bitmex = new BitMEXApi(bitmexKey, bitmexSecret);
            // var orderBook = bitmex.GetOrderBook("XBTUSD", 3);
            var jsonStream = bitmex.GetOrders(test);
            if(jsonStream.IndexOf("[") == 0)
            {
                jsonStream = jsonStream.Substring(1);
                jsonStream = jsonStream.Remove(jsonStream.Length - 1);
            }
            if(jsonStream.IndexOf("error") == 2)
            {
                jsonStream = jsonStream.Substring(9);
                jsonStream = jsonStream.Remove(jsonStream.Length - 1);
            }

            jsonStream = jsonStream.Replace("},", "}");
            //var orders = bitmex.DeleteOrders();

            var temp = jsonStream;
            temp = temp.Replace("}", "}\n\n");
            Console.WriteLine(temp);

            IList<BitMEXOrder> orderlist = new List<BitMEXOrder>();
            JsonTextReader reader = new JsonTextReader(new StringReader(jsonStream));
            reader.SupportMultipleContent = true;

            while (true)
            {
                if (!reader.Read())
                {
                    break;
                }

                JsonSerializer serializer = new JsonSerializer
                {
                    NullValueHandling = NullValueHandling.Ignore
                };
                BitMEXOrder order = serializer.Deserialize<BitMEXOrder>(reader);

                orderlist.Add(order);
            }

            foreach (BitMEXOrder order in orderlist)
            {
                Console.WriteLine(order.currency);
            }

            
            System.Threading.Thread.Sleep(500000);
         
        }
    }
}