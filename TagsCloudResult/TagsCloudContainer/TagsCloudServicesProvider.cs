// using System;
// using Microsoft.Extensions.DependencyInjection;
// using TagsCloudContainer.Settings;
//
// namespace TagsCloudContainer
// {
//     public class TagsCloudServicesProvider
//     {
//         private readonly RenderSettings renderSettings;
//         public TagsCloudServicesProvider(RenderSettings renderSettings)
//         {
//             this.renderSettings = renderSettings;
//         }
//
//         public IServiceProvider  BuildProvider()
//         {
//             return new ServiceCollection().BuildServiceProvider();
//         }
//     }
// }