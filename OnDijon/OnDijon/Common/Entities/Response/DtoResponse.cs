namespace OnDijon.Common.Entities.Response
{
    public class DtoResponse<T> : Response where T : Dto.Dto
    {
        public T Data { get; set; }
    }
}
