using System;
using CoreFX.Abstractions.Contracts.Interfaces;
using CoreFX.Abstractions.Enums;

namespace CoreFX.Abstractions.Contracts.Extensions
{
    public static class SvcResponse_Extension
    {
        public static bool Any<T>(this ISvcResponse<T> res) =>
           true == res?.IsSuccess && null != res.Data;

        public static bool FailOrEmpty<T>(this ISvcResponse<T> res) =>
            true != res?.IsSuccess || null == res.Data;

        public static ISvcResponse<T> SetStatusCode<T>(this ISvcResponse<T> res, bool isSuccess)
        {
            _ = isSuccess ? res.Success() : res.Error();
            return res;
        }

        public static ISvcResponse<T> SetStatusCode<T>(this ISvcResponse<T> res, SvcCodeEnum code)
        {
            res.Code = (int)code;
            return res;
        }

        public static ISvcResponse<T> SetData<T>(this ISvcResponse<T> res, T data)
        {
            res.Data = data;
            return res;
        }

        public static ISvcResponse<T> SetMsg<T>(this ISvcResponse<T> res, string msg)
        {
            res.Msg = msg;
            return res;
        }

        public static ISvcResponse Error(this ISvcResponse res)
        {
            res.Error(SvcCodeEnum.Error, SvcCodeEnum.Error.ToString());
            return res;
        }

        public static ISvcResponse Error(this ISvcResponse res, SvcCodeEnum code, Exception ex)
        {
            res.Error(code, ex?.Message);
            return res;
        }

        public static ISvcResponse Error(this ISvcResponse res, SvcCodeEnum code, string errMsg)
        {
            res.Code = (int)code;
            res.Msg = string.IsNullOrWhiteSpace(errMsg) ? code.ToString() : errMsg;
            res.IsSuccess = false;

            return res;
        }

        public static ISvcResponse<T> Error<T>(this ISvcResponse<T> res)
        {
            res.Error(SvcCodeEnum.Error, SvcCodeEnum.Error.ToString());
            return res;
        }

        public static ISvcResponse<T> Error<T>(this ISvcResponse<T> res, SvcCodeEnum code, Exception ex)
        {
            res.Error(code, ex?.Message);
            return res;
        }

        public static ISvcResponse<T> Error<T>(this ISvcResponse<T> res, SvcCodeEnum code, string errMsg)
        {
            res.Code = (int)code;
            res.Msg = string.IsNullOrWhiteSpace(errMsg) ? code.ToString() : errMsg;
            res.IsSuccess = false;

            return res;
        }

        public static ISvcResponse Success(this ISvcResponse res)
        {
            res.Success(SvcCodeEnum.Success, SvcCodeEnum.Success.ToString());
            return res;
        }

        public static ISvcResponse Success(this ISvcResponse res, SvcCodeEnum code, string msg = null)
        {
            res.Code = (int)code;
            res.Msg = string.IsNullOrWhiteSpace(msg) ? code.ToString() : msg;
            res.IsSuccess = true;

            return res;
        }

        public static ISvcResponse<T> Success<T>(this ISvcResponse<T> res)
        {
            res.Success(SvcCodeEnum.Success, SvcCodeEnum.Success.ToString());
            return res;
        }

        public static ISvcResponse<T> Success<T>(this ISvcResponse<T> res, SvcCodeEnum code, string msg = null)
        {
            res.Code = (int)code;
            res.Msg = string.IsNullOrWhiteSpace(msg) ? code.ToString() : msg;
            res.IsSuccess = true;

            return res;
        }
    }
}
