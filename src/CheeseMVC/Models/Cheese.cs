namespace CheeseMVC.Models
{
    public class Cheese
    {
        //Cheese properties
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        //Cheese category properties
        public int CategoryID { get; set; }
        public CheeseCategory Category { get; set; }
        
    }
}
