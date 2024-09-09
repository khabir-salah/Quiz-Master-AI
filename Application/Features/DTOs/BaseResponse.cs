

namespace Application.Features.DTOs
{
    public  class BaseResponse<T>
    {
        public bool IsSuccessful { get; set; }
        public T? Result { get; set; }
    }
}
