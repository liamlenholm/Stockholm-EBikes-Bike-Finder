# SCRIPT TO GET ALL STOCKHOLM EBIKES STATIONS LONGITUDE AND LATITUDE
# https://github.com/liamlenholm/Stockholm-EBikes-Bike-Finder

import urllib.request
import json

urls = ["https://stockholmebikes.se/map/detail/c0ce6e46-8340-475e-88c4-0085cb132904",
        "https://stockholmebikes.se/map/detail/40b879f6-1fee-40a0-8c8b-6b9ce245a41f",
        "https://stockholmebikes.se/map/detail/90beb6ce-1e2b-4586-9514-4cd9ef0aff7c",
        "https://stockholmebikes.se/map/detail/f9033694-e456-4e2d-8f0a-2118934407b5",
        "https://stockholmebikes.se/map/detail/4a029af8-7575-49d5-9142-dfc467bd4960",
        "https://stockholmebikes.se/map/detail/551ad7c7-a918-4c69-aa7e-6110d567c395",
        "https://stockholmebikes.se/map/detail/fa1bf7f5-fa86-45c2-b9a4-9584f909bf34",
        "https://stockholmebikes.se/map/detail/c40527cb-d84c-4704-93d3-2b2ee7eed99f",
        "https://stockholmebikes.se/map/detail/d874a261-90c5-4dc6-b120-17fd378a461e",
        "https://stockholmebikes.se/map/detail/63253761-1360-40b1-aff1-2d18cd054180",
        "https://stockholmebikes.se/map/detail/60120bb7-f90a-48f6-aa3c-6efe92195fd0",
        "https://stockholmebikes.se/map/detail/71e0ac89-db41-4e7b-9404-582767594026",
        "https://stockholmebikes.se/map/detail/e0e79e75-93bd-4398-b3e0-5db2c81c349b",
        "https://stockholmebikes.se/map/detail/8ba3983b-cee2-41cd-a24c-01517af95a47",
        "https://stockholmebikes.se/map/detail/c4714522-e691-4aec-98ae-2f7db39655f3",
        "https://stockholmebikes.se/map/detail/faed2f51-897e-46f4-af3f-e4ee4b493b46",
        "https://stockholmebikes.se/map/detail/8ad34ecd-993a-4142-a8ef-ff663f54bfa0",
        "https://stockholmebikes.se/map/detail/5555380e-c81f-4e7f-a303-32d39396e50c",
        "https://stockholmebikes.se/map/detail/907dcd54-cdd9-4521-975e-b60cee9da28c",
        "https://stockholmebikes.se/map/detail/1c299c9c-aa80-43a1-ac97-3d8effeb10cf",
        "https://stockholmebikes.se/map/detail/3e5d9a88-0468-4167-ab9a-2711787a116b",
        "https://stockholmebikes.se/map/detail/21f117a0-b615-47a1-8b38-c0b6f41bca6c",
        "https://stockholmebikes.se/map/detail/88f6643f-1e4f-4703-a545-ed6f24f9161f",
        "https://stockholmebikes.se/map/detail/003b4566-1a1d-4aa5-9b2c-35972a0ac219",
        "https://stockholmebikes.se/map/detail/4c0012b4-1eb8-4943-bc48-32691ab58ca7",
        "https://stockholmebikes.se/map/detail/d71df9b7-0b02-4930-b333-d4bb13f43243",
        "https://stockholmebikes.se/map/detail/af8c27f2-b431-42cc-b659-46ae4922097f",
        "https://stockholmebikes.se/map/detail/e14e5a32-47f1-4a84-85ae-07a2ec1cc958",
        "https://stockholmebikes.se/map/detail/3f53309a-f5e8-4407-8aaf-42d823f87846",
        "https://stockholmebikes.se/map/detail/1c887652-e924-4f34-b568-e4567ed10926",
        "https://stockholmebikes.se/map/detail/fe39a54a-3513-4437-9c59-85736807c7ff",
        "https://stockholmebikes.se/map/detail/c71cc763-98b0-4688-a4f0-ce5235804432",
        "https://stockholmebikes.se/map/detail/ecfdb7c8-9852-44d2-a4cf-c13bfd4a6a30",
        "https://stockholmebikes.se/map/detail/4d8587eb-fec6-46e7-8e9f-6cd2b5d7eb90",
        "https://stockholmebikes.se/map/detail/837328e1-c654-47c4-8d24-e40c142fe5c1",
        "https://stockholmebikes.se/map/detail/75ea3735-378b-48f1-8248-66bf1ab65187",
        "https://stockholmebikes.se/map/detail/798e3261-2012-4c65-af00-b4d6b8e96b9a",
        "https://stockholmebikes.se/map/detail/1bbd622b-3658-4b38-910e-d8dff887bcdd",
        "https://stockholmebikes.se/map/detail/fa2fe2eb-912f-489a-9ece-1ad7cecb507c",
        "https://stockholmebikes.se/map/detail/159e8622-7572-43fb-a47e-44c9b4d272f8",
        "https://stockholmebikes.se/map/detail/850cfddf-a65e-41c7-9a25-d86239d3b7bf",
        "https://stockholmebikes.se/map/detail/734749f2-832a-485c-86a3-14de1d1c5958",
        "https://stockholmebikes.se/map/detail/159e8622-7572-43fb-a47e-44c9b4d272f8",
        "https://stockholmebikes.se/map/detail/fa2fe2eb-912f-489a-9ece-1ad7cecb507c",
        "https://stockholmebikes.se/map/detail/10b15a0e-4e7d-436b-bd49-b8e793a99a72",
        "https://stockholmebikes.se/map/detail/1850edf4-c993-4882-a88f-35da22a4f0c2",
        "https://stockholmebikes.se/map/detail/004f0690-df63-4b3c-9949-11261b199720",
        "https://stockholmebikes.se/map/detail/ddad0994-42a6-4e6a-bf67-0ff3925d150b",
        "https://stockholmebikes.se/map/detail/734749f2-832a-485c-86a3-14de1d1c5958",
        "https://stockholmebikes.se/map/detail/7847e3a8-d4a2-4140-93e6-685ca34ebb10",
        "https://stockholmebikes.se/map/detail/1bbd622b-3658-4b38-910e-d8dff887bcdd",
        "https://stockholmebikes.se/map/detail/2eae80d8-9009-4724-b1bd-c8506c433ed1",
        "https://stockholmebikes.se/map/detail/6ff30f53-843b-4247-920e-cae742159287",
        "https://stockholmebikes.se/map/detail/bf66def3-b5f7-415e-8b5c-d3275910eb35",
        "https://stockholmebikes.se/map/detail/28ef3974-af0f-4b72-913b-1138d78e1bb5",
        "https://stockholmebikes.se/map/detail/38f57ed3-51dd-4df0-9541-c6eefd1ff8f5",
        "https://stockholmebikes.se/map/detail/bda1ee35-685f-4e9d-8a73-f4eb4ffbcc70",
        "https://stockholmebikes.se/map/detail/1ea26b04-192b-490b-984f-f9cb30198176",
        "https://stockholmebikes.se/map/detail/b617f6b7-9385-4091-90a5-5cb14bf4394c",
        "https://stockholmebikes.se/map/detail/691d4e1c-0620-47ee-a5cd-901df077e3d0",
        "https://stockholmebikes.se/map/detail/e55d9f5f-cff5-445d-a72d-dd8ce1473514",
        "https://stockholmebikes.se/map/detail/1dd1330c-a172-4abf-83ba-37362e42e2d4",
        "https://stockholmebikes.se/map/detail/37350b74-9532-4f65-a727-53f64db7b406",
        "https://stockholmebikes.se/map/detail/1dd1330c-a172-4abf-83ba-37362e42e2d4",
        "https://stockholmebikes.se/map/detail/d5aa45e5-88ba-45a5-8e00-771e5592ab74",
        "https://stockholmebikes.se/map/detail/68a0292b-1a94-4b02-a856-ee39072c2ef5",
        "https://stockholmebikes.se/map/detail/014cf32d-c432-48e8-a32f-e8c85c143f5a",
        "https://stockholmebikes.se/map/detail/ffb197fc-9534-42eb-8090-6ff4ccecf7f3",
        "https://stockholmebikes.se/map/detail/84b645f4-490c-45ad-8616-de9a4db70dd5",
        "https://stockholmebikes.se/map/detail/4ec3714c-04f8-4ab5-b276-ea41a44ca3b9",
        "https://stockholmebikes.se/map/detail/e439da44-4c3f-4324-a718-638dde5626e8",
        "https://stockholmebikes.se/map/detail/b19f3928-7779-4967-857f-1700c9c2ca27",
        "https://stockholmebikes.se/map/detail/a679c4c4-01e6-4c44-9cb7-13b2c34ecbc2",
        "https://stockholmebikes.se/map/detail/5d5600f3-9ead-4b8f-94a6-167de179d0be",
        "https://stockholmebikes.se/map/detail/35eded4e-3fff-4bcc-bc6d-86a365f0c288",
        "https://stockholmebikes.se/map/detail/e039190d-bd99-4d11-9980-1ac28aee91a7",
        "https://stockholmebikes.se/map/detail/3243d447-7e18-4e07-8fa2-af8dc6cfce0b",
        "https://stockholmebikes.se/map/detail/3dbec17a-45e4-46cb-9b3c-e99f1e2a2ae8",
        "https://stockholmebikes.se/map/detail/7b294b75-737c-4351-93dd-20e31f0469ee",
        "https://stockholmebikes.se/map/detail/829ccce2-e70a-49ff-9bfd-f98d0d5c70dc",
        "https://stockholmebikes.se/map/detail/67504343-938a-48e9-98f2-195060d747a0"]


UrlExt = "?_data=routes%2Fmap%2Fdetail.%24optionId"
counter = 0
df = open('output.txt', 'w')


for url in urls:
    request = urllib.request.Request(url + UrlExt)
    response = urllib.request.urlopen(request).read()
    cont = json.loads(response)
    counter += 1
    latitude = cont["mobilityOption"]["station"]["location"]["latitude"]
    longitude = cont["mobilityOption"]["station"]["location"]["longitude"]
    longitude = str(longitude)
    latitude = str(latitude)
    currentcount = str(counter)
    print("Current line: " + str(currentcount))
    df.write('new Station() { Url = "' + url +
             '", Longitude = ' + longitude + ', Latitude = ' + latitude + ' },')
    df.write('\n')


df.close()