using System;
using System.Collections.Generic;
using System.Text;

namespace ProductivityApp.Constants
{
    public static class Constants
    {
        public static string PyEndpoint { get; set; } = "https://weatherapp20210725210906.azurewebsites.net/api/RoomController";
        public static string WetherEndpoint { get; set; } = "https://api.openweathermap.org/data/2.5/weather?zip=107345,ro&appid=44afe348069b1812b24e39c82be13c9e";
        public static string WetherImages { get; set; } = "https://openweathermap.org/img/wn/";

    }
}
