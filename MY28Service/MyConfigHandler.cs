using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MY28Service
{
    public class MyConfigHandler : IConfigurationSectionHandler
    {
        public MyConfigHandler()
        {
        }

        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            NameValueCollection configs;
            NameValueSectionHandler baseHandler = new NameValueSectionHandler();
            configs = (NameValueCollection)baseHandler.Create(parent, configContext, section);
            return configs;
        }
    }
}
