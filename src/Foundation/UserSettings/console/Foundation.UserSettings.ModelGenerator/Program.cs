using Sitecore.XConnect.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.UserSettings.ModelGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var json = XdbModelWriter.Serialize(SF.Foundation.Facets.Models.CollectionModel.Model);
            System.IO.File.WriteAllText("UserSettings.Model.json", json);
        }
    }
}
