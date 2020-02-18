using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TinyCrmApi.Model;

namespace TinyCrmApi.Options
{
    public class ApiResult<T>
    {
        public T Data { get; set; }
        public string ErrorText { get; set; }
        public StatusCode ErrorCode { get; set; }
        public bool Success => ErrorCode == StatusCode.Success;

        public ApiResult()
        { }

        public ApiResult(StatusCode code,
            string errorText)
        {
            ErrorCode = code;
            ErrorText = errorText;
        }

        public static ApiResult<T> Create<Y>(
            ApiResult<Y> result)
        {
            return new ApiResult<T>()
            {
                ErrorCode = result.ErrorCode,
                ErrorText = result.ErrorText
            };
        }
        public static ApiResult<T> CreateSuccessful(
            T data)
        {
            return new ApiResult<T>()
            {
                ErrorCode = StatusCode.Success,
                Data = data
            };
        }

        public static ApiResult<T> CreateUnsuccessful(
            StatusCode statusCode, string errorText)
        {
            return new ApiResult<T>()
            {
                ErrorCode = statusCode,
                ErrorText = errorText
            };
        }

        public static ApiResult<T> CreateBadRequest(
            T data)
        {
            return new ApiResult<T>()
            {
                ErrorCode = StatusCode.BadRequest,
                Data = data
            };
        }
    }
}
