namespace MyNPOI.Helpers
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class NPOIUtility
    {
        public static class JsonUtility
        {
            private static void AddIsoDateTimeConverter(JsonSerializer serializer)
            {
                IsoDateTimeConverter item = new IsoDateTimeConverter {
                    DateTimeFormat = "yyyy-MM-dd"
                };
                serializer.Converters.Add(item);
            }

            public static T DecodeObject<T>(string data)
            {
                JsonSerializer serializer = new JsonSerializer {
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                AddIsoDateTimeConverter(serializer);
                StringReader reader = new StringReader(data);
                return (T) serializer.Deserialize(reader, typeof(T));
            }

            public static List<T> DecodeObjectList<T>(string data)
            {
                JsonSerializer serializer = new JsonSerializer {
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                AddIsoDateTimeConverter(serializer);
                StringReader reader = new StringReader(data);
                return (List<T>) serializer.Deserialize(reader, typeof(List<T>));
            }

            public static string EncodeAjaxResponseJson(string jsonString, string callback)
            {
                if (!string.IsNullOrEmpty(callback))
                {
                    return (callback + "(" + jsonString + ")");
                }
                return jsonString;
            }

            public static string ExtGridSortInfo(string property, string direction)
            {
                return string.Format("[{{\"property\":\"{0}\",\"direction\":\"{1}\"}}]", property, direction);
            }

            public static string ListToJson<T>(List<T> objList)
            {
                string str;
                JsonSerializer serializer = new JsonSerializer();
                SerializerSetting(serializer);
                AddIsoDateTimeConverter(serializer);
                using (TextWriter writer = new StringWriter())
                {
                    using (JsonWriter writer2 = new JsonTextWriter(writer))
                    {
                        serializer.Serialize(writer2, objList);
                        str = writer.ToString();
                    }
                }
                return str;
            }

            public static string ObjectToJson<T>(T obj)
            {
                string str;
                JsonSerializer serializer = new JsonSerializer();
                SerializerSetting(serializer);
                AddIsoDateTimeConverter(serializer);
                using (TextWriter writer = new StringWriter())
                {
                    using (JsonWriter writer2 = new JsonTextWriter(writer))
                    {
                        serializer.Serialize(writer2, obj);
                        str = writer.ToString();
                    }
                }
                return str;
            }

            public static string ReturnFailureMessage(string message, string exMessage)
            {
                return ReturnMessage(false, 0, message, exMessage, "[]");
            }

            public static string ReturnFailureMessageTouch(string message, string exMessage)
            {
                return ("{\"success\":\"false\",\"msg\":\"" + exMessage + "\"}");
            }

            public static string ReturnMessage(bool sucess, int total, string message, string exMessage, string data)
            {
                message = message.Replace("'", "").Replace("\"", "").Replace("<", "").Replace(">", "");
                exMessage = exMessage.Replace("'", "").Replace("\"", "").Replace("<", "").Replace(">", "");
                return string.Format("{{success:{0},total:{1},data:{2},message:\"{3}\",exMessage:\"{4}\"}}", new object[] { sucess.ToString().ToLower(), total, data, message, exMessage });
            }

            public static string ReturnSuccessMessage(string message, string exMessage)
            {
                return ReturnMessage(true, 0, message, exMessage, "[]");
            }

            public static string ReturnSuccessMessage(string message, string exMessage, string data)
            {
                return ReturnMessage(true, 0, message, exMessage, "[" + data + "]");
            }

            public static string ReturnSuccessMessage<T>(int total, string message, string exMessage, T obj)
            {
                string data = ObjectToJson<T>(obj);
                return ReturnMessage(true, total, message, exMessage, data);
            }

            public static string ReturnSuccessMessage<T>(int total, string message, string exMessage, List<T> objList)
            {
                string data = ListToJson<T>(objList);
                return ReturnMessage(true, total, message, exMessage, data);
            }

            public static string ReturnSuccessMessageTouch<T>(T obj)
            {
                return ObjectToJson<T>(obj);
            }

            public static string ReturnSuccessMessageTouch(string message, string exMessage)
            {
                return ("{\"success\":\"true\",\"msg\":\"" + message + "\"}");
            }

            private static void SerializerSetting(JsonSerializer serializer)
            {
                serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            }
        }
    }
}

