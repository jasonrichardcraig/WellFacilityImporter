using System;
using System.Data.SqlTypes;
using System.Net.Http;
using System.Xml;
using Microsoft.SqlServer.Server;

public class Geocoder
{
    static Geocoder()
    {
        // Force modern TLS protocols
        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls13;
    }

    [SqlFunction(DataAccess = DataAccessKind.None, SystemDataAccess = SystemDataAccessKind.None)]
    public static AddressDetail GetAddressFromCoordinates(SqlDouble latitude, SqlDouble longitude)
    {
        if (latitude.IsNull || longitude.IsNull)
        {
            return AddressDetail.Null;
        }

        string apiUrl = $"https://nominatim.openstreetmap.org/reverse?format=xml&lat={latitude.Value}&lon={longitude.Value}&addressdetails=1&zoom=18";

        try
        {
            using (HttpClient client = new HttpClient())
            {

                client.DefaultRequestHeaders.Add("User-Agent", "WellFacilityClrUtilities/1.0 (https://github.com/jasonrichardcraig/WellFacilityImporter)");

                var response = client.GetStringAsync(apiUrl).Result;

                // Load the response XML into an XmlDocument
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(response);

                // Extract the address details from the XML
                XmlNode addressNode = doc.SelectSingleNode("//addressparts");

                if (addressNode != null)
                {
                    AddressDetail addressDetail = new AddressDetail
                    {
                        Country = addressNode.SelectSingleNode("country")?.InnerText ?? "",
                        State = addressNode.SelectSingleNode("state")?.InnerText ?? "",
                        County = addressNode.SelectSingleNode("county")?.InnerText ?? "",
                        City = addressNode.SelectSingleNode("city")?.InnerText ?? "",
                        TownBorough = addressNode.SelectSingleNode("town")?.InnerText ??
                                      addressNode.SelectSingleNode("borough")?.InnerText ?? "",
                        VillageSuburb = addressNode.SelectSingleNode("village")?.InnerText ??
                                        addressNode.SelectSingleNode("suburb")?.InnerText ?? "",
                        Neighbourhood = addressNode.SelectSingleNode("neighbourhood")?.InnerText ?? "",
                        AnySettlement = addressNode.SelectSingleNode("settlement")?.InnerText ?? "",
                        MajorStreets = addressNode.SelectSingleNode("road")?.InnerText ?? "",
                        MajorMinorStreets = addressNode.SelectSingleNode("residential")?.InnerText ?? "",
                        Building = addressNode.SelectSingleNode("building")?.InnerText ?? ""
                    };

                    return addressDetail;
                }
                else
                {
                    return AddressDetail.Null;
                }
            }
        }
        catch (Exception)
        {
            return AddressDetail.Null;
        }
    }
}
