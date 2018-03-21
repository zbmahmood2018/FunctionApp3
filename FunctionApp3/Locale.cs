using System.Collections.Generic;

namespace FunctionApp3
{
    public class Locale
    {
        public string Id { get; set; }
        public string Country { get; set; }

        public static List<Locale> GenerateData()
        {
            return new List<Locale>
            {
                new Locale {Id="1", Country="Australia"},
                new Locale {Id="2", Country="United Kingdom"},
                new Locale {Id="3", Country="Germany"}
            };
        }        
    }
}
