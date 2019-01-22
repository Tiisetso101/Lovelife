namespace LoveLife.API.Controllers.Helpers
{
    public class UserParams
    {
        private const int MaxPageSize = 50;
        private int pageSize { get; set; }
        
        public int PageNumber { get; set; } =1;

        
        public int pageSize
        {
            get {return pageSize;}
            set {pageSize = (value > MaxPageSize) ? MaxPageSize : value ;}
        }
    }
}