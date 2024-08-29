namespace ProjetoTemplate.Domain.Helpers
{
    public class GridViewData<TData>
    {
        public int Count { get; set; }
        public List<TData> Data { get; set; }
    }
}
