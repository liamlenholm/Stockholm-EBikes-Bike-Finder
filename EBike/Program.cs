using EBike;
using GeoCoordinatePortable;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Net;




Console.WriteLine("1. Find closest available bike based on your location  \n2. Find all available bikes in specific area. Ex Odenplan");
int chooseMethod = Convert.ToInt32(Console.ReadLine());


if (chooseMethod == 1)
{

    getCurrentLocation();
}
else if (chooseMethod == 2)
{
    Console.WriteLine("1. Östermalm \n2. Gärdet \n3. Odenplan \n4. Slussen/Medis \n5. Skanstull");
    int choosenLocation = Convert.ToInt32(Console.ReadLine());
    generateLocations(choosenLocation);

}

static void getCurrentLocation()
{
    Console.WriteLine("Address:    #Example Sveavägen 100");
    var address = Convert.ToString(Console.ReadLine());

    if (address == null)
    {
        Console.WriteLine("No address input");
        getCurrentLocation();
    }
    else
    {
        //REPLACE SPACE WITH %20 (URL ENCODING)
        address = address.Replace(" ", "%20");
        //ADD STOCKHOLM SWEDEN TO MAKE SURE THERE ISNT A STREET WITH THE SAME NAME IN ANOTHER COUNTRY
        address = address + "%20Stockholm%20Sweden";

        //API KEY GET ONE FOR FREE AT POSITIONSTACK.COM
        string fullUrl = ("http://api.positionstack.com/v1/forward?access_key=" + APIKeys.MAP_API_KEY + "&query=" + address);


        WebClient client = new WebClient();
        string request = client.DownloadString(fullUrl);

        dynamic response = JsonConvert.DeserializeObject<dynamic>(request);
        double longitude = response["data"][0]["longitude"];
        double latitude = response["data"][0]["latitude"];

        //RUN METHOD AND SET LONG+LAT
        //FindClosestBike(longitude, latitude);
    }
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

            string request = client.DownloadString(fullUrl.Trim());
            dynamic response = JsonConvert.DeserializeObject<dynamic>(request);
            string getName = response["mobilityOption"]["station"]["name"];

            indexnum++;
            var Locations = new List<LocationArea>() {

            new LocationArea
            {
                Url = fullUrl,
                Name = getName,
                Index = indexnum.ToString()
        }

        };
            if (Locations != null)
            {
                foreach (var l in Locations)
                {
                    string request2 = client.DownloadString(l.Url);
                    dynamic response2 = JsonConvert.DeserializeObject<dynamic>(request2);
                    string bikesAvailable = response2["mobilityOption"]["station"]["occupancy"];

                    Console.WriteLine(l.Name + ": " + "BIKES AVAILABLE: " + bikesAvailable + "  INDEX = " + l.Index);

                    ListOfURLS.Add(l.Index + " " + l.Url);
                }
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

    //FIND HOW MANY JSON ITEMS FOR THE FOR LOOP
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

//JUST FOR TESTING
static void testArea()
{
    //TestArea
}



public class LocationArea
{
    public string Url { get; set; }

    public string Name { get; set; }
    public string Index { get; set; }

}


public class StationsLocation
{
    public string Url { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }
}



/* I USED THIS TO UPLOAD EVERYTHING TO A DATABASE
 * 
* THIS WAS THE FUNCTION TO PUSH IT TO THE DB
void AddListToDB()
{
    SqlConnection sqlConnection;
    string connectionString = @"Server=localhost\SQLEXPRESS;Database=ebike;Trusted_Connection=True;";
    try
    {
        sqlConnection = new SqlConnection(connectionString);
        sqlConnection.Open();
        Console.WriteLine("Connected");
        foreach (var l in StationsLongLat)
        {
            //string query = "INSERT INTO STATIONS(url,longitude,latitude) VALUES('" + l.Url + "'," + l.Longitude + "'," + l.Latitude + ")";
            Console.WriteLine(l.Latitude);

            
            String query = "INSERT INTO STATIONS(url,longitude,latitude) VALUES (@url,@long,@lat)";
            SqlCommand command = new SqlCommand(query, sqlConnection);
            command.Parameters.Add("@url", SqlDbType.VarChar).Value = l.Url;
            command.Parameters.Add("@long", SqlDbType.Float).Value = l.Longitude;
            command.Parameters.Add("@lat", SqlDbType.Float).Value = l.Latitude;
            command.ExecuteNonQuery();
        }   
    } catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }

} 

 * 
 * THE LIST WAS GENERATED USING A MY SCRIPT getLongLat.py
 * 
 * List<StationsLocation> StationsLongLat = new List<StationsLocation>();
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/c0ce6e46-8340-475e-88c4-0085cb132904", Longitude = 18.107227164370393, Latitude = 59.306721187745076 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/40b879f6-1fee-40a0-8c8b-6b9ce245a41f", Longitude = 18.107164446181546, Latitude = 59.31170451854383 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/90beb6ce-1e2b-4586-9514-4cd9ef0aff7c", Longitude = 18.09875166646293, Latitude = 59.30900690757117 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/f9033694-e456-4e2d-8f0a-2118934407b5", Longitude = 18.096799973930665, Latitude = 59.314639593450785 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/4a029af8-7575-49d5-9142-dfc467bd4960", Longitude = 18.090975147667102, Latitude = 59.30784342539341 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/551ad7c7-a918-4c69-aa7e-6110d567c395", Longitude = 18.082443075756878, Latitude = 59.30869912404809 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/fa1bf7f5-fa86-45c2-b9a4-9584f909bf34", Longitude = 18.083695590491168, Latitude = 59.312417508059376 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/c40527cb-d84c-4704-93d3-2b2ee7eed99f", Longitude = 18.077930043497776, Latitude = 59.30780824439966 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/d874a261-90c5-4dc6-b120-17fd378a461e", Longitude = 18.075428662325063, Latitude = 59.309159150344605 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/63253761-1360-40b1-aff1-2d18cd054180", Longitude = 18.073935354350986, Latitude = 59.30759814564792 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/60120bb7-f90a-48f6-aa3c-6efe92195fd0", Longitude = 18.074592413423698, Latitude = 59.311889612599884 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/71e0ac89-db41-4e7b-9404-582767594026", Longitude = 18.072869267674648, Latitude = 59.31497269604937 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/e0e79e75-93bd-4398-b3e0-5db2c81c349b", Longitude = 18.072062223314347, Latitude = 59.3198764575366 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/8ba3983b-cee2-41cd-a24c-01517af95a47", Longitude = 18.065920294993575, Latitude = 59.31434525154654 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/c4714522-e691-4aec-98ae-2f7db39655f3", Longitude = 18.054525404072898, Latitude = 59.312332767639404 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/faed2f51-897e-46f4-af3f-e4ee4b493b46", Longitude = 18.050821913557836, Latitude = 59.316165596883124 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/8ad34ecd-993a-4142-a8ef-ff663f54bfa0", Longitude = 18.043281664919444, Latitude = 59.316531028658936 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/5555380e-c81f-4e7f-a303-32d39396e50c", Longitude = 18.031651406815747, Latitude = 59.31505368867376 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/907dcd54-cdd9-4521-975e-b60cee9da28c", Longitude = 18.031835227980224, Latitude = 59.318375729295894 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/1c299c9c-aa80-43a1-ac97-3d8effeb10cf", Longitude = 18.05184841925692, Latitude = 59.29678135677665 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/3e5d9a88-0468-4167-ab9a-2711787a116b", Longitude = 18.042472189616458, Latitude = 59.2994626479564 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/21f117a0-b615-47a1-8b38-c0b6f41bca6c", Longitude = 18.012585376627786, Latitude = 59.29078668706857 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/88f6643f-1e4f-4703-a545-ed6f24f9161f", Longitude = 17.98805273576369, Latitude = 59.3207080984903 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/003b4566-1a1d-4aa5-9b2c-35972a0ac219", Longitude = 17.993884371679936, Latitude = 59.324599301690995 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/4c0012b4-1eb8-4943-bc48-32691ab58ca7", Longitude = 18.004518046354416, Latitude = 59.336546230182854 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/d71df9b7-0b02-4930-b333-d4bb13f43243", Longitude = 18.01040563808604, Latitude = 59.336912680058546 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/af8c27f2-b431-42cc-b659-46ae4922097f", Longitude = 18.01838659192964, Latitude = 59.32828424371835 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/e14e5a32-47f1-4a84-85ae-07a2ec1cc958", Longitude = 18.027198762021754, Latitude = 59.33178874174456 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/3f53309a-f5e8-4407-8aaf-42d823f87846", Longitude = 18.031905255993983, Latitude = 59.32832674743012 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/1c887652-e924-4f34-b568-e4567ed10926", Longitude = 18.03644315520986, Latitude = 59.333874814597884 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/fe39a54a-3513-4437-9c59-85736807c7ff", Longitude = 18.040559004943212, Latitude = 59.33335359534984 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/c71cc763-98b0-4688-a4f0-ce5235804432", Longitude = 18.044345309006573, Latitude = 59.331417069033584 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/ecfdb7c8-9852-44d2-a4cf-c13bfd4a6a30", Longitude = 18.042947447499348, Latitude = 59.328835258878726 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/4d8587eb-fec6-46e7-8e9f-6cd2b5d7eb90", Longitude = 18.047485452882135, Latitude = 59.332648360197545 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/837328e1-c654-47c4-8d24-e40c142fe5c1", Longitude = 18.051717374437683, Latitude = 59.32710864948076 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/75ea3735-378b-48f1-8248-66bf1ab65187", Longitude = 18.055151847682083, Latitude = 59.32812922730499 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/798e3261-2012-4c65-af00-b4d6b8e96b9a", Longitude = 18.058242614347527, Latitude = 59.33108657457175 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/1bbd622b-3658-4b38-910e-d8dff887bcdd", Longitude = 18.057085606007632, Latitude = 59.332812409413414 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/fa2fe2eb-912f-489a-9ece-1ad7cecb507c", Longitude = 18.06551489759912, Latitude = 59.32949954365521 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/159e8622-7572-43fb-a47e-44c9b4d272f8", Longitude = 18.06673499641138, Latitude = 59.33066979002841 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/850cfddf-a65e-41c7-9a25-d86239d3b7bf", Longitude = 18.06363625380487, Latitude = 59.33259677666643 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/734749f2-832a-485c-86a3-14de1d1c5958", Longitude = 18.065040411062856, Latitude = 59.33311527158431 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/159e8622-7572-43fb-a47e-44c9b4d272f8", Longitude = 18.06673499641138, Latitude = 59.33066979002841 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/fa2fe2eb-912f-489a-9ece-1ad7cecb507c", Longitude = 18.06551489759912, Latitude = 59.32949954365521 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/10b15a0e-4e7d-436b-bd49-b8e793a99a72", Longitude = 18.07000054858849, Latitude = 59.330138377054205 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/1850edf4-c993-4882-a88f-35da22a4f0c2", Longitude = 18.074349356016537, Latitude = 59.329609275573794 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/004f0690-df63-4b3c-9949-11261b199720", Longitude = 18.07380431681029, Latitude = 59.33074081057744 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/ddad0994-42a6-4e6a-bf67-0ff3925d150b", Longitude = 18.073418057299552, Latitude = 59.333171101951244 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/734749f2-832a-485c-86a3-14de1d1c5958", Longitude = 18.065040411062856, Latitude = 59.33311527158431 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/7847e3a8-d4a2-4140-93e6-685ca34ebb10", Longitude = 18.067736598407805, Latitude = 59.33364992699729 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/1bbd622b-3658-4b38-910e-d8dff887bcdd", Longitude = 18.057085606007632, Latitude = 59.332812409413414 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/2eae80d8-9009-4724-b1bd-c8506c433ed1", Longitude = 18.051865564274486, Latitude = 59.33526139084276 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/6ff30f53-843b-4247-920e-cae742159287", Longitude = 18.062708911406965, Latitude = 59.33620897476783 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/bf66def3-b5f7-415e-8b5c-d3275910eb35", Longitude = 18.065024885006217, Latitude = 59.335909439410834 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/28ef3974-af0f-4b72-913b-1138d78e1bb5", Longitude = 18.041121307541456, Latitude = 59.33778738746459 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/38f57ed3-51dd-4df0-9541-c6eefd1ff8f5", Longitude = 18.03875383788498, Latitude = 59.33967138547063 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/bda1ee35-685f-4e9d-8a73-f4eb4ffbcc70", Longitude = 18.040027756827396, Latitude = 59.34005316647985 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/1ea26b04-192b-490b-984f-f9cb30198176", Longitude = 18.036066998742804, Latitude = 59.342637700984525 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/b617f6b7-9385-4091-90a5-5cb14bf4394c", Longitude = 18.039204638655093, Latitude = 59.34523624264204 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/691d4e1c-0620-47ee-a5cd-901df077e3d0", Longitude = 18.051070686321108, Latitude = 59.34305342040771 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/e55d9f5f-cff5-445d-a72d-dd8ce1473514", Longitude = 18.056125494637385, Latitude = 59.343615236537154 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/1dd1330c-a172-4abf-83ba-37362e42e2d4", Longitude = 18.063675673052458, Latitude = 59.34501633104741 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/37350b74-9532-4f65-a727-53f64db7b406", Longitude = 18.05216595909007, Latitude = 59.347785541410396 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/1dd1330c-a172-4abf-83ba-37362e42e2d4", Longitude = 18.063675673052458, Latitude = 59.34501633104741 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/d5aa45e5-88ba-45a5-8e00-771e5592ab74", Longitude = 18.071207276316187, Latitude = 59.345685309078995 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/68a0292b-1a94-4b02-a856-ee39072c2ef5", Longitude = 18.065606131338363, Latitude = 59.34126779900406 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/014cf32d-c432-48e8-a32f-e8c85c143f5a", Longitude = 18.068239168812337, Latitude = 59.34301744183588 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/ffb197fc-9534-42eb-8090-6ff4ccecf7f3", Longitude = 18.070285956214903, Latitude = 59.341595865303304 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/84b645f4-490c-45ad-8616-de9a4db70dd5", Longitude = 18.077242699781273, Latitude = 59.34193164622362 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/4ec3714c-04f8-4ab5-b276-ea41a44ca3b9", Longitude = 18.09104573278821, Latitude = 59.338634163778174 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/e439da44-4c3f-4324-a718-638dde5626e8", Longitude = 18.093170143713742, Latitude = 59.340474917817616 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/b19f3928-7779-4967-857f-1700c9c2ca27", Longitude = 18.09494868684943, Latitude = 59.33162812040267 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/a679c4c4-01e6-4c44-9cb7-13b2c34ecbc2", Longitude = 18.09197187423706, Latitude = 59.34298322851119 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/5d5600f3-9ead-4b8f-94a6-167de179d0be", Longitude = 18.093295687486762, Latitude = 59.34823819678029 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/35eded4e-3fff-4bcc-bc6d-86a365f0c288", Longitude = 18.114563534252785, Latitude = 59.3400756609696 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/e039190d-bd99-4d11-9980-1ac28aee91a7", Longitude = 18.05792439331788, Latitude = 59.3523388660369 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/3243d447-7e18-4e07-8fa2-af8dc6cfce0b", Longitude = 18.087996655014905, Latitude = 59.354012328574385 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/3dbec17a-45e4-46cb-9b3c-e99f1e2a2ae8", Longitude = 18.103247554921893, Latitude = 59.35706499530158 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/7b294b75-737c-4351-93dd-20e31f0469ee", Longitude = 18.090158911239733, Latitude = 59.35820361425726 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/829ccce2-e70a-49ff-9bfd-f98d0d5c70dc", Longitude = 18.058506681292037, Latitude = 59.3618623535828 });
StationsLongLat.Add(new StationsLocation { Url = "https://stockholmebikes.se/map/detail/67504343-938a-48e9-98f2-195060d747a0", Longitude = 18.054469210546028, Latitude = 59.365132602524795 });
*/