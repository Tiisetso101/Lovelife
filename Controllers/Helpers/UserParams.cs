namespace LoveLife.API.Controllers.Helpers
{
    public class UserParams
    {
        private const int MaxPageSize = 50;
       // private int pageSize { get; set; }
        
        public int PageNumber { get; set; } =1;

        
        public int pageSize
        {
            get {return pageSize;}
            set {pageSize = (value > MaxPageSize) ? MaxPageSize : value ;}
        }

        public int UserId { get; set; }
        public string Gender { get; set; }
        public int minAge { get; set; } = 18;
        public int maxage { get; set; } = 99;
        public string OrderBy { get; set; }
        
    }
}