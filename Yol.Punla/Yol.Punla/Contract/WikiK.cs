namespace Yol.Punla.Contract
{
    public class WikiK
    {
        public string Title { get; set; }
        public string IconPath { get; set; }
        public string Content { get; set; }

        //chito.this is hack so that we wont call again the endpoint just for this
        public string ForceToVersionNo { get; set; }
        public string ForceToVersionNoIOS { get; set; }
    }
}
