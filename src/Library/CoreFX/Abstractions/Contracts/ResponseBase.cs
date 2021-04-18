using System;
using System.Collections.Generic;
using CoreFX.Abstractions.Enums;

namespace CoreFX.Abstractions.Contracts
{
    /// <summary>
    /// Purpose: Sometimes we got confused from return value
    ///     int Execute(); => what does 0 mean ? initial value ? count() ? or value is 0 ?
    ///     bool Execute(); => what does FALSE mean ? initial value ? error occurs ? or value is FALSE ?
    ///     Entity Execute(); => what does NULL mean ? initial value ? error occurs ? or value is NULL ?
    /// Idea:    
    ///     Need a model returns extra information after function, procedure, service call 
    ///     So probably return a generic mixed object, such as ResponseBase<int>, ResponseBase<bool>, ResponseBase<Entity>
    /// </summary>
    public class ResponseBase
    {
        public ResponseBase()
        {
            Initialize();
        }

        /// <summary>
        /// Notice:
        ///     Default return code = -0001
        ///     Default IsSuccess = false
        /// </summary>
        public void Initialize()
        {
            Code = (int)SvcCodeEnum.UnSet;
            Msg = string.Empty;
            SubCode = string.Empty;
            SubMsg = string.Empty;
            IsSuccess = false;
            MsgId = Guid.NewGuid().ToString();
            ExtMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Code = -0001 : undefined
        /// Code = 0000 : general error
        /// Code = 0001 : general success
        /// Code > 0001 : other error
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Message
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// MessageId
        /// </summary>
        public string MsgId { get; set; }

        /// <summary>
        /// The most important Successful flag after function, procedure, service call
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Internal status code
        /// </summary>
        public string SubCode { get; set; }

        /// <summary>
        /// Internal message or error message
        /// </summary>
        public string SubMsg { get; set; }

        /// <summary>
        /// Extension Key,Value Map
        /// </summary>
        public Dictionary<string, string> ExtMap { get; set; }
    }

}
