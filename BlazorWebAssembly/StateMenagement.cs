namespace BlazorWebAssembly
{
    public class StateMenagement
    {
        public string utente { get; set; } = "";

        public string api_url = "http://api.admincoreblazor.it";

        public string JsonNormalized(string json)
        {
            json = json.ToString().Replace("\\u0022", "\"");
            json = json.ToString().Replace("\"{", "{");
            json = json.ToString().Replace("}\"", "}");

            return json;
        }
    }
}
