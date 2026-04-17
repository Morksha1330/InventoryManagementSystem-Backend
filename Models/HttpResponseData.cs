namespace InventoryMgtSystem.Models
{
    public class HttpResponseData<T>
    {
        public T Result { get; set; }
        public List<T> Results { get; set; }
        public int ResponsCode { get; set; }
        public string? Error { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}
