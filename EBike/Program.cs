﻿using Newtonsoft.Json;
using System.Net;


Console.WriteLine("Location?");
Console.WriteLine("1. Östermalm \n2. Gärdet \n3. Odenplan");
int choosenLocation = Convert.ToInt32(Console.ReadLine());

generateLocations(choosenLocation);


static void generateLocations(int choosenLocation)
{

    string urlExt = "?_data=routes%2Fmap%2Fdetail.%24optionId";



    string[] östermalmUrls =
    {
        "https://stockholmebikes.se/map/detail/a679c4c4-01e6-4c44-9cb7-13b2c34ecbc2",
        "https://stockholmebikes.se/map/detail/e439da44-4c3f-4324-a718-638dde5626e8",
        "https://stockholmebikes.se/map/detail/4ec3714c-04f8-4ab5-b276-ea41a44ca3b9",
        "https://stockholmebikes.se/map/detail/84b645f4-490c-45ad-8616-de9a4db70dd5",
        "https://stockholmebikes.se/map/detail/b19f3928-7779-4967-857f-1700c9c2ca27",
    };

    string[] gärdetUrls =
    {
        "https://stockholmebikes.se/map/detail/35eded4e-3fff-4bcc-bc6d-86a365f0c288",
        "https://stockholmebikes.se/map/detail/a679c4c4-01e6-4c44-9cb7-13b2c34ecbc2",
        "https://stockholmebikes.se/map/detail/5d5600f3-9ead-4b8f-94a6-167de179d0be",
    };

    string[] odenplanUrls =
    {
        "https://stockholmebikes.se/map/detail/b617f6b7-9385-4091-90a5-5cb14bf4394c",
        "https://stockholmebikes.se/map/detail/691d4e1c-0620-47ee-a5cd-901df077e3d0",
        "https://stockholmebikes.se/map/detail/37350b74-9532-4f65-a727-53f64db7b406",
        "https://stockholmebikes.se/map/detail/e55d9f5f-cff5-445d-a72d-dd8ce1473514"
    };
    var setLocation = (String[])null;
    switch (choosenLocation)
    {
        case 1:
            setLocation = östermalmUrls;
            break;
        case 2:
            setLocation = gärdetUrls;
            break;
        case 3:
            setLocation = odenplanUrls;
            break;

    }


    foreach (var url in setLocation)
    {
        string fullUrl = url + urlExt;
        using (WebClient client = new WebClient())
        {
            string strPageCode = client.DownloadString(fullUrl.Trim());
            dynamic dobj = JsonConvert.DeserializeObject<dynamic>(strPageCode);
            string getName = dobj["mobilityOption"]["station"]["name"];
            var Locations = new List<Location>() {

            new Location
            {
                Area = "Odenplan",
                Url = fullUrl,
                Name = getName
            }
        };




            foreach (var l in Locations)
            {
                if (l.Area.Contains("Odenplan", StringComparison.InvariantCultureIgnoreCase))
                {
                    string strPageCode2 = client.DownloadString(l.Url);
                    dynamic dobj2 = JsonConvert.DeserializeObject<dynamic>(strPageCode2);
                    string temp2 = dobj2["mobilityOption"]["station"]["occupancy"];
                    Console.WriteLine(l.Name + ": " + temp2 + "\r\n");
                }
            }
        }
    }
}
public class Location
{
    public string Area { get; set; }
    public string Url { get; set; }

    public string Name { get; set; }

}
