using CoreFX.Abstractions.Contracts.Extensions;
using CoreFX.Abstractions.Contracts.Interfaces;

namespace CoreFX.Abstractions.Contracts
{
    public class SvcResponse : ResponseBase, ISvcResponse
    {
        public SvcResponse() { }
        public SvcResponse(bool isSuccess)
        {
            _ = isSuccess ? this.Success() : this.Error();
        }

        public SvcResponse(ISvcResponse res)
        {
            if (null != res)
            {
                Code = res.Code;
                Msg = res.Msg;
                SubCode = res.SubCode;
                SubMsg = res.SubMsg;
                IsSuccess = res.IsSuccess;
                ExtMap = res.ExtMap;
            }
        }
    }

    /// <summary>
    /// Usage: The generic type could be primitive type or object type, to store actual return data
    ///         Wraper a ResponseBase model so we could give extra information, such as IsSuccess, StatusCode, etc
    /// Example: 
    ///	    SvcResponse<bool>
    ///	    SvcResponse<Account>
    ///	    SvcResponse<Dictionary<string,string>>
    /// </summary>
    public class SvcResponse<T> : ResponseBase, ISvcResponse<T>
    {
        public SvcResponse() { }

        public SvcResponse(bool isSuccess)
        {
            _ = isSuccess ? this.Success() : this.Error();
        }

        public SvcResponse(bool isSuccess, T data) : this(isSuccess)
        {
            Data = data;
        }

        public SvcResponse(ISvcResponse res)
        {
            if (null != res)
            {
                Code = res.Code;
                Msg = res.Msg;
                SubCode = res.SubCode;
                SubMsg = res.SubMsg;
                IsSuccess = res.IsSuccess;
                ExtMap = res.ExtMap;
            }
        }

        public T Data { get; set; }
    }

}
