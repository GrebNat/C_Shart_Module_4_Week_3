using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using static M4W2.Properties.Settings;

namespace M4W2.Util
{
    public class Properties
    {
        public static string email => Default["email"].ToString();
        public static string url => Default["url"].ToString();
        public static string password => Default["password"].ToString();
       
    }
}