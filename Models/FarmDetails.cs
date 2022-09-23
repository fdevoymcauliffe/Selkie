namespace SELKIE.Models
{
    public static class FarmDetails
    {
        public static string ManufatureName { get; set; }
        public static int NoOfDivices { get; set; }
        public static string TechType { get; set; } = "Wave";
        public static string PowerCurve { get; set; }
        public static string Substructrename { get; set; }
        public static string Substructretype { get; set; }
        public static double Distancebetweendevices { get; set; }
        public static double Tidal_height { get; set; }
        public static double Tidal_hubHeight { get; set; }
    }
}
