using System;
using Realms;

namespace realm_tutorial_dotnet
{
    public class Member : RealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public string Id { get; set; }

        [MapTo("name")]
        public string Name { get; set; }
    }
}
