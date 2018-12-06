using System;
using Microsoft.AspNetCore.Http;

namespace LoveLife.API.Controllers.Helpers
{
    public static class Extentions
    {
        public static void ApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Control", "*");
        }

        public static int CalculateAge(this DateTime theDateTime)
        {
            var age = DateTime.Today.Year - theDateTime.Year ;
            if(theDateTime.AddYears(age) > DateTime.Today)
                age--;
            return age;
        }
    }
}