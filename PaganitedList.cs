namespace CourseManagement{
    public class PaganitedList<T> : List<T>{

        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public PaganitedList(IEnumerable<T> source,int count,int pageIndex,int pageSize){
            PageIndex = pageIndex ;
            TotalPages = (int) Math.Ceiling(count / (double) pageSize);
            this.AddRange(source);
             
        }

        public bool HasNextPage => PageIndex < TotalPages;
        public bool HasPreviousPage => PageIndex > 1;

        public static PaganitedList<T> Create(IEnumerable<T> source,int pageIndex,int pageSize){
            int count = source.Count();
            var items = source.Skip((pageIndex-1)*pageSize).Take(pageSize);
            return new PaganitedList<T>(items,count,pageIndex,pageSize);
        } 
        
    }
}