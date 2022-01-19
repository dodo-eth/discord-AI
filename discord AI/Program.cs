using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Threading;
using Newtonsoft.Json; 
using System.Collections;
using System.Collections.Generic; 
using System.Text;
using System.Diagnostics; 
using System.Collections.ObjectModel;
using System.Reflection;
using System.Runtime.Serialization;
using System.Data;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Specialized; 
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Net;
using System.Dynamic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters; 

namespace discord_AI
{
    class Program
    {
        static string message_content;
        static void Main(string[] args)
        {





            Console.WriteLine("guild_id");
            string guild_id = Console.ReadLine();
            Console.WriteLine("Дебик введи частоту рации Пашы");
            string chanall = Console.ReadLine();
            Console.WriteLine("calldown");
            int calldown =Convert.ToInt32( Console.ReadLine());

            while (true)
            {
                string message_id = get_message_id(chanall); //получаем od на овтет
                string ai_message_content = ai_message();

                Console.WriteLine("Получаем сообщение");
                message_content = message_content.Replace("%20", " ");
                Console.WriteLine(message_content);
                Console.WriteLine("Получаем ответ");
                Console.WriteLine(ai_message_content);

                send_message(ai_message_content, guild_id, chanall, message_id);
                Thread.Sleep(calldown*1000);
            }

        }


       
        static string get_message_id(string chanall_id)
        {
            var url = "https://discord.com/api/v9/channels/"+ chanall_id + "/messages?limit=10";

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);

            httpRequest.Headers["authorization"] = "mfa.zbAeNV6fLBtH11fzv0Q2-R8H78psXMFFPl0attxF-UAF74QDGJ1pOVgxJJ-_TbygvYYgY7GvLGR4qDdYndG3";


            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            string result;
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                 result = streamReader.ReadToEnd();
            }

            Console.WriteLine(httpResponse.StatusCode);
            Random rnd = new Random();             
            int value = rnd.Next(0, 10);
             
           var des = JsonConvert.DeserializeObject<List<messages_from_chat.Root>>(result);
            message_content = des[value].Content.ToString();
             

                return des[value].Id;
        }
         
        static string ai_message()
        {
            string phrase = message_content;
            phrase = phrase.Replace(" ", "%20");
            var url = "https://icap.iconiq.ai/talk";

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.ContentType = "application/x-www-form-urlencoded";

            var data = "input=" + phrase + "&botkey=icH-VVd4uNBhjUid30-xM9QhnvAaVS3wVKA3L8w2mmspQ-hoUB3ZK153sEG3MX-Z8bKchASVLAo~&channel=7&sessionid=482114903&client_name=uuiprod-un18e6d73c-user-100759&id=true";

            using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
            }

            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            string result;
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            discord_AI.Root message_respo = JsonConvert.DeserializeObject<discord_AI.Root>(result);
            string message = message_respo.Responses[0];
            return message;
        }
        static string send_message(string message_content_send, string guild_id, string chanall_id, string message_id)
        {
            try
            {
                
                var url = "https://discord.com/api/v9/channels/" + chanall_id + "/messages";

                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "POST";

                httpRequest.Headers["authorization"] = "mfa.zbAeNV6fLBtH11fzv0Q2-R8H78psXMFFPl0attxF-UAF74QDGJ1pOVgxJJ-_TbygvYYgY7GvLGR4qDdYndG3";
                httpRequest.ContentType = "application/json";

                string data = @"{""content"":"""+message_content_send+""",""nonce"":""true"",""tts"":false,""message_reference"":{""guild_id"":""" + guild_id + ",""channel_id"":"""+ chanall_id + ",""message_id"":"""+ message_id + """}}";
                

                if (message_content_send == null)
                {
                    message_content_send = "agree";
                } 
                data.Replace("933011641816186920", chanall_id);
                data.Replace("933172455651037205", message_id);
                Thread.Sleep(1500);
                using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                {
                    streamWriter.Write(data);
                }
                
                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                string result;
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }

                discord_AI.Root message_sended = JsonConvert.DeserializeObject<discord_AI.Root>(result);
                return httpResponse.StatusCode.ToString();
            }
            catch
            {
                return "error";
            }

        }
    }
}
