using System;
using System.Text.RegularExpressions;

namespace travel_service.ResourceParameters
{
    public class TouristRouteParameters
    {
        public string keyword { get; set; }
        public string ratingOperator { get; set; }
        public int? ratingValue { get; set; }
        public string rating
        {
            get { return _rating; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    Regex regex = new Regex(@"([A-Za-z0-9\-]+)(\d+)");
                    Match match = regex.Match(value);
                    if (match.Success)
                    {
                        ratingOperator = match.Groups[1].Value;
                        ratingValue = Int32.Parse(match.Groups[2].Value);
                    }
                }

                _rating = value;
            }
        }
        private string _rating;
        public TouristRouteParameters()
        {
        }
    }
}
