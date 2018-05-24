using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace App1
{
    public class Houses
    {
        public List<options> listing { get; set; }        
        public string country { get; set; }
        public string area_name { get; set; }
        public string longitude { get; set; }
        public string result_count { get; set; }
    }

    public class options
    {
        public string county { get; set; }
        public string thumbnail_url { get; set; }
        public string country_code { get; set; }
        public string num_floors { get; set; }
        public string listing_status { get; set; }
        public string num_bedrooms { get; set; }
        public string location_is_approximate { get; set; }
        public string latitude { get; set; }
        public string furnished_state { get; set; }
        public string agent_address { get; set; }
        public string category { get; set; }
        public string property_type { get; set; }
        public string description { get; set; }
        public string post_town { get; set; }
        public string details_url { get; set; }
        public string short_description { get; set; }
        public string outcode { get; set; }
        public string property_report_url { get; set; }
        public string price { get; set; }
        public string listing_id { get; set; }
        public string image_caption { get; set; }
        public string status { get; set; }
        public string agent_name { get; set; }
        public string num_recepts { get; set; }
        public string first_published_date { get; set; }
        public string displayable_address { get; set; }
        public string street_name { get; set; }
        public string num_bathrooms { get; set; }
        public string agent_logo { get; set; }
        public string agent_phone { get; set; }
        public string image_url { get; set; }
        public string last_published_date { get; set; }
        public string error_code { get; set; }
        public string letting_fees { get; set; }
        public string _longitude { get; set; }
        public Boolean favorited;


        public void setFavorite(Boolean favorite)
        {
            this.favorited = favorite;
        }

        public Boolean getFavorite()
        {
            return this.favorited;
        }

    }
}
