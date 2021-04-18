using System;
using Hello5.Domain.Common.Consts;
using Hello5.Domain.Contract.Models.Echo;

namespace Hello5.Domain.Contract.Models.Extensions
{
    public static class HelloEchoRequestDto_Extension
    {
        public static HelloEchoRequestDto PreProcess(this HelloEchoRequestDto requestDto)
        {
            if (requestDto != null)
            {
#if DEBUG
                if (string.IsNullOrEmpty(requestDto.Send) || requestDto.Send.Equals("string", StringComparison.OrdinalIgnoreCase))
                {
                    requestDto.Send = HelloConst.EchoCommand;
                }
#endif
            }

            return requestDto;
        }
    }
}
