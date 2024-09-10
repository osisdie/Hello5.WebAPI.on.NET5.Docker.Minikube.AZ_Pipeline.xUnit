using System;
using System.Threading.Tasks;
using Hello8.Domain.Common.Consts;
using Hello8.Domain.Contract.Models.Echo;

namespace Hello8.Domain.Contract.Models.Extensions
{
    public static class HelloEchoRequestDto_Extension
    {
        public static async Task<HelloEchoRequestDto> PreProcess(this HelloEchoRequestDto requestDto)
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

            await Task.CompletedTask;
            return requestDto;
        }
    }
}
