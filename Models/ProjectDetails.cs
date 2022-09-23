namespace SELKIE.Models
{
    public static class ProjectDetails
    {
        public static string ProjectName { get; set; }
        public static int InstallTime { get; set; }
        public static int ProjectLifeTime { get; set; }
        public static int NoOfIterations { get; set; }
        public static double GridSaleRate { get; set; } = 0;
        public static double WakeLoss { get; set; } = 0;
        public static double TransLoss { get; set; } = 0;
        public static double ProdLoss { get; set; } = 0;
    }
}
