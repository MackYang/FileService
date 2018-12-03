using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileInAPI.UEditor
{
    public class ConfigHandler : Handler
    {
        public ConfigHandler(HttpContextBase context) : base(context) { }

        public override void Process()
        {
            WriteJson(Config.Items);
        }
    }
}