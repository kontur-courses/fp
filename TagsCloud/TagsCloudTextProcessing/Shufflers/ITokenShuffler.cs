using System.Collections.Generic;
using TagCloudResult;

namespace TagsCloudTextProcessing.Shufflers
 {
     public interface ITokenShuffler
     {
         Result<List<Token>> Shuffle(IEnumerable<Token> tokens);
     }
 }