namespace ServiceTitan.Core
{
    public class AssemblyAIModel
    {
        public string audio_url { get; set; }
        public bool entity_detection { get; set; }
        public bool auto_highlights { get; set; }
        public bool iab_categories { get; set; }
        public bool sentiment_analysis { get; set; }
        public bool auto_chapters { get; set; }
    }
}