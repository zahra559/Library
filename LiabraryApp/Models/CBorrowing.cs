namespace LiabraryApp.Models
{
    public class CBorrowing
    {
        public int ID { get; set; }
        public CBook Book { get; set; }
        public CUser User { get; set; }
    }
}
