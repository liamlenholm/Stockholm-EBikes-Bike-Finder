using EBike;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;

Console.WriteLine("Location?");
Console.WriteLine("1. Östermalm \n2. Gärdet \n3. Odenplan \n4. Slussen/Medis \n5. Skanstull");
int choosenLocation = Convert.ToInt32(Console.ReadLine());
generateLocations(choosenLocation);




static void getCurrentLocation()
{
    Console.WriteLine("Address: ");
    var address = Convert.ToString(Console.ReadLine());
    //REPLACE SPACE WITH %20 (URL ENCODING)
    address = address.Replace(" ", "%20");
    //ADD STOCKHOLM SWEDEN TO MAKE SURE THERE ISNT A STREET WITH THE SAME NAME IN ANOTHER COUNTRY
    address = address + "%20Stockholm%20Sweden";

    //API KEY GET ONE FOR FREE AT POSITIONSTACK.COM



    string fullUrl = ("http://api.positionstack.com/v1/forward?access_key=" + APIKeys.MAP_API_KEY + "&query=" + address);


    WebClient client = new WebClient();
    string request = client.DownloadString(fullUrl);
    dynamic response = JsonConvert.DeserializeObject<dynamic>(request);
    string longitude = response["data"][0]["longitude"];
    string latitude = response["data"][0]["latitude"];

    Console.WriteLine("LONG: " + longitude + "\nLatitude: " + latitude);
}


static void testArea()
{
    //TestArea
}

static void generateLocations(int choosenLocation)
{
    //URL EXT FOR JSON DATA
    string urlExt = "?_data=routes%2Fmap%2Fdetail.%24optionId";



    //IMPORT URLS FOR LOCATION
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

    string[] slussenmedisUrls =
    {
        "https://stockholmebikes.se/map/detail/e0e79e75-93bd-4398-b3e0-5db2c81c349b",
        "https://stockholmebikes.se/map/detail/71e0ac89-db41-4e7b-9404-582767594026",
        "https://stockholmebikes.se/map/detail/8ba3983b-cee2-41cd-a24c-01517af95a47",
        "https://stockholmebikes.se/map/detail/60120bb7-f90a-48f6-aa3c-6efe92195fd0"
    };

    string[] skanstullUrls =
    {
        "https://stockholmebikes.se/map/detail/63253761-1360-40b1-aff1-2d18cd054180",
        "https://stockholmebikes.se/map/detail/d874a261-90c5-4dc6-b120-17fd378a461e",
        "https://stockholmebikes.se/map/detail/c40527cb-d84c-4704-93d3-2b2ee7eed99f",
        "https://stockholmebikes.se/map/detail/551ad7c7-a918-4c69-aa7e-6110d567c395",
        "https://stockholmebikes.se/map/detail/60120bb7-f90a-48f6-aa3c-6efe92195fd0"
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
        case 4:
            setLocation = slussenmedisUrls;
            break;
        case 5:
            setLocation = skanstullUrls;
            break;

    }

    int indexnum = 0;
    List<string> ListOfURLS = new List<string>();


    foreach (var url in setLocation)
    {
        string fullUrl = url + urlExt;
        using (WebClient client = new WebClient())
        {
            string strPageCode = client.DownloadString(fullUrl.Trim());
            dynamic dobj = JsonConvert.DeserializeObject<dynamic>(strPageCode);
            string getName = dobj["mobilityOption"]["station"]["name"];
            indexnum++;
            var Locations = new List<LocationArea>() {

            new LocationArea
            {
                Url = fullUrl,
                Name = getName,
                Index = indexnum.ToString()
        }
        };

            foreach (var l in Locations)
            {
                string strPageCode2 = client.DownloadString(l.Url);
                dynamic dobj2 = JsonConvert.DeserializeObject<dynamic>(strPageCode2);
                string bikesAvailable = dobj2["mobilityOption"]["station"]["occupancy"];

                Console.WriteLine(l.Name + ": " + "BIKES AVAILABLE: " + bikesAvailable + "  INDEX = " + l.Index);

                ListOfURLS.Add(l.Index + " " + l.Url);
            }

        }
    }


    Console.WriteLine("Would you like to see more info about specific location? Y/N");
    string answerYesOrNo = Convert.ToString(Console.ReadLine());

    if (answerYesOrNo.Contains("y", StringComparison.InvariantCultureIgnoreCase))
    {
        Console.WriteLine("Which location index would you like to check?");
        int indexNum = Convert.ToInt32(Console.ReadLine());

        // SPLIT TO TAKE ONLY THE URL
        var urlForFunc = ListOfURLS[indexNum - 1].Split(" ");
        MoreInfo(urlForFunc[1]);
    }
}


static void MoreInfo(string url)
{
    WebClient client = new WebClient();
    string strPageCode = client.DownloadString(url);
    dynamic dobj = JsonConvert.DeserializeObject<dynamic>(strPageCode);
    JArray items = (JArray)dobj["mobilityOption"]["station"]["vehicles"];
    int length = items.Count;
    Console.WriteLine(url + "\n \n");

    for (int i = 0; i < length; i++)
    {

        var licPlate = dobj["mobilityOption"]["station"]["vehicles"][i]["licensePlate"];
        var batteryLevel = dobj["mobilityOption"]["station"]["vehicles"][i]["energyGauge"];
        if (licPlate != null && batteryLevel != null)
        {
            Console.WriteLine("BIKE ID: " + licPlate + "\nBATTERY LEVEL: " + batteryLevel + "\n \n");
        }
    }
}



public class LocationArea
{
    public string Url { get; set; }

    public string Name { get; set; }
    public string Index { get; set; }

}
