namespace CommonDTO
{
    public class PaginationDTO
    {
        public int CurrentPage {  get; set; }
        public int PageSize { get; set; }
    }

    public class PaginationLotSearchDTO
    {
        public string? Name { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }

    public class PaginationUserSearchDTO
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }

}
