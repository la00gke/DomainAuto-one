using System;
using System.Collections.Generic;
using System.DirectoryServices;
using Newtonsoft.Json;

namespace DomainInfo.lib.json
{
    internal class ObjToJson
    {
        public static string objTojson(SearchResultCollection searchResults)
        {
            Dictionary<string, List<object>> results = new Dictionary<string, List<object>>();

            foreach (SearchResult result in searchResults)
            {
                List<object> resultvalues = new List<object>();
                string resultname = result.Path;
                // Console.WriteLine(resultname);
                DirectoryEntry directoryEntry = result.GetDirectoryEntry();

                Dictionary<string, List<object>> properties = new Dictionary<string, List<object>>();

                foreach (string propertyName in directoryEntry.Properties.PropertyNames)
                {
                    PropertyValueCollection propertyValues = directoryEntry.Properties[propertyName];
                    List<object> values = new List<object>();

                    for (int i = 0; i < propertyValues.Count; i++)
                    {
                        values.Add(propertyValues[i]);
                    }
                    properties.Add(propertyName, values);
                }
                resultvalues.Add(properties);
                results.Add(resultname, resultvalues);
            }
            string Objjson = JsonConvert.SerializeObject(results, Formatting.Indented);
            return Objjson;
        }
    }
}
