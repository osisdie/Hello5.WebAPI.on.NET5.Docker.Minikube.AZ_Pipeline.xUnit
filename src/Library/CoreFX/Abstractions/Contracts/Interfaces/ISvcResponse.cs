using System.Collections.Generic;

namespace CoreFX.Abstractions.Contracts.Interfaces
{
    public interface ISvcResponse
    {
        int Code { get; set; }

        /// <summary>
        /// Message
        /// </summary>
        string Msg { get; set; }

        /// <summary>
        /// Message Id
        /// </summary>
        string MsgId { get; set; }

        /// <summary>
        /// The most important Successful flag after function, procedure, service call
        /// </summary>
        bool IsSuccess { get; set; }

        /// <summary>
        /// Internal status code
        /// </summary>
        string SubCode { get; set; }

        /// <summary>
        /// Internal message or error message
        /// </summary>
        string SubMsg { get; set; }

        /// <summary>
        /// Extension Key,Value Map
        /// </summary>
        Dictionary<string, string> ExtMap { get; set; }
    }

    public interface ISvcResponse<T> : ISvcResponse
    {
        T Data { get; set; }
    }
}
