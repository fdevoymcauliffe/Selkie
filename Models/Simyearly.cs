namespace SELKIE.SimModels
{
    public class Simyearly
    {
        
    }

    public class Installyearly
    {
        public int Year { get; set; }
        public int Iteration { get; set; }
        public double Total_install_cost { get; set; }
        public double T_fuelcost { get; set; }
        public double T_fixedcost { get; set; }
        public double TechCost { get; set; }
        public double BaseCost { get; set; }
        public double AdditionalCost { get; set; }
    }
}
