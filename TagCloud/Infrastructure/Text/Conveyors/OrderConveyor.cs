using System.Collections.Generic;
using System.Linq;
using TagCloud.Infrastructure.Text.Information;

namespace TagCloud.Infrastructure.Text.Conveyors
{
    public class OrderConveyor : IConveyor<string>
    {
        public IEnumerable<(string token, TokenInfo info)> Handle(IEnumerable<(string token, TokenInfo info)> tokens)
        {
            return tokens.OrderByDescending(x => x.info.Frequency);
        }
    }
}