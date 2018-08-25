using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json.Linq;


namespace WeatherServiceApp
{

    class DataThingyToEnhance
    {
        public int Dataid;
        public double Latitude;
        public double Longitude;
    }

    class Enhancement
    {
        public float? Winddirection;
        public float Windspeed;
        public float Pressure;
    }

    class Program
    {

        static void Main(string[] args)
        {
            var ToEnhanceList = GetDataToEnhanceFromDb();

            foreach (var item in ToEnhanceList)
            {
                Enhancement e = GetWeatherEnhancementForCoordinates(item);
                var IdOfStoredElement = StoreEnhancementToDb(e);
                UpdateEnhancedData(item.Dataid, IdOfStoredElement);
            }
        }

        private static void UpdateEnhancedData(int DataId ,int IdOfStoredElement)
        {
                /*
                UPDATE SensorData
                SET enhancementid = IdOfStoredElement
                WHERE dataid = DataId;
                */
        }

        private static HttpClient Client = new HttpClient();


        private static List<DataThingyToEnhance> GetDataToEnhanceFromDb()
        {
            /*
                SELECT data.dataid, sens.longitude, sens.latitude
                FROM SensorData AS data
                JOIN Sensor AS sens ON sens.sensorid = data.sensorid
                WHERE 1=1
                AND data.enhancementid IS NULL
                AND data.timestamp > NOW()-60*60; --1hour
             */

            DataThingyToEnhance x = new DataThingyToEnhance();
            x.Latitude = 47.32423;
            x.Longitude = 8.44;
            x.Dataid = 333;
            return new List<DataThingyToEnhance>() { x };
        }

        private static Enhancement GetWeatherEnhancementForCoordinates(DataThingyToEnhance data)
        {
            String apikey = "b7315d7fc73fbd756db96bdd85b8b9dc";

            var latitude = data.Latitude;
            var longitude = data.Longitude;

            Client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");
            var response = Client.GetStringAsync(new Uri("http://api.openweathermap.org/data/2.5/weather?lat=" + latitude + "&lon=" + longitude + "&appid=" + apikey)).Result;

            JObject json = JObject.Parse(response);

            Enhancement Enhancement = new Enhancement();
            Enhancement.Pressure = json["main"].Value<float>("pressure");
            Enhancement.Windspeed = json["wind"].Value<float>("speed");
            // wind direction is not mandatory as some examples showed...
            Enhancement.Winddirection = json["wind"].Value<float?>("deg");

            return Enhancement;
        }

        private static int StoreEnhancementToDb(Enhancement e)
        {
            /*
                INSERT INTO SensorDataEnhancement (winddirection, windspeed, pressure)
                VALUES (e.Winddirection, e.Windspeed, e.Pressure);

                --> return enhancementid
             */
             var EnhancementId = 9999;

             return EnhancementId;
        }

    }
}