namespace LWCCWebsite.ViewModels
{
    //This will hold the twitter tweet data for rendering the latest tweets
    public class TwitterViewModel
    {
        public string TwitterHandle { get; set; }
        public bool error { get; set; }
        public string json { get; set; }
        public string Message { get; set; }
    }
}
