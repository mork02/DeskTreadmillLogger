namespace DeskTreadmillLogger.models
{
    internal class UserEntry
    {
        public int id { get; set; }                 
        public string name { get; set; }            
        public double weightKg { get; set; }        
        public int heightCm { get; set; }           
        public DateTime birthdate { get; set; }     
        public string gender { get; set; }          
        public string email { get; set; }           
        public DateTime createdAt { get; set; } = DateTime.UtcNow;

        public UserEntry(int id, string name, double weightKg, int heightCm)
        {
            this.id = id;
            this.name = name;
            this.weightKg = weightKg;
            this.heightCm = heightCm;
            this.createdAt = DateTime.UtcNow;
        }

        public UserEntry() { }

    }
}
