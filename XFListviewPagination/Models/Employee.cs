using Newtonsoft.Json;

namespace XFListviewPagination.Models
{
    public class Employee
    {
        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonIgnore]
        public string FullName => $"{FirstName} {LastName}";
    }
}
