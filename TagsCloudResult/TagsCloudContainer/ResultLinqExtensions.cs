// using System.Collections.Generic;
// using System.Linq;
//
// namespace TagsCloudContainer
// {
//     public static class ResultLinqExtensions
//     {
//         public static Result<IEnumerable<T>> CombineResults<T>(this IEnumerable<Result<T>> results)
//         {
//             var resultsList = results.ToList();
//             var filedResult = resultsList.Find(r => !r.Success);
//             return filedResult != null
//                 ? new Result<IEnumerable<T>>(filedResult.Exception)
//                 : new Result<IEnumerable<T>>(resultsList.Select(r => r.Value));
//         }
//     }
// }