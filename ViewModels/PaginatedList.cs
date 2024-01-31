public class PaginatedList<T> : List<T>{
    public int PageIndex {get; private set;}
    public int TotalPage {get; private set;}
    public PaginatedList(IEnumerable<T> items,int PageIndex,int TotalPage){
        this.AddRange(items);
        this.PageIndex = PageIndex;
        this.TotalPage = TotalPage;
    }

    public bool HasNextPage => PageIndex < TotalPage ;
    public bool HasPreviousPage => PageIndex >1;

    public static PaginatedList<T> Create(IEnumerable<T> sourses,int PageIndex,int PageSize){
        int count = sourses.Count();
        int TotalPage =(int) Math.Ceiling( count / (double) PageSize );
        var items = sourses.Skip((PageIndex - 1)*PageSize).Take(PageSize);
        return new PaginatedList<T>(items,PageIndex,TotalPage) ;

    }
}