namespace BBKBootcampSocial.Core.Paging
{
    public class BasePaging
    {
        public BasePaging()
        {
            CurrentPage = 1;
            TakePages = 10;
        }
        public int CurrentPage { get; set; }
        public int SkipPages { get; set; }
        public int TakePages { get; set; }
    }
}
