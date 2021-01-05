using System;
using Realms;

namespace realm_tutorial_dotnet
{
    public class Member : RealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public string id { get; set; }

        [MapTo("name")]
        public string name { get; set; }
    }
}
