namespace BBKBootcampSocial.Core.Paging
{
    public class Pager
    {
        public static BasePaging Build(int currentPage, int take)
        {
            if (currentPage <= 1) currentPage = 1;

            return new BasePaging
            {
                CurrentPage = currentPage,
                SkipPages = currentPage * 10,
                TakePages = take
            };
        }
    }
}
