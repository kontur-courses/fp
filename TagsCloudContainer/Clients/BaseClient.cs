using System;
using System.Drawing;
using TagsCloudContainer.Cloud;
using TagsCloudContainer.Functional;
using TagsCloudContainer.Savers;

namespace TagsCloudContainer.Clients
{
    public abstract class BaseClient
    {
        protected readonly TagsCloudSettings CloudSettings;
        private readonly Func<TagsCloudGenerator> cloudFactory;
        private readonly FileImageSaver saver;

        protected BaseClient(
            TagsCloudSettings cloudSettings,
            Func<TagsCloudGenerator> cloudFactory,
            FileImageSaver saver)
        {
            CloudSettings = cloudSettings;
            this.cloudFactory = cloudFactory;
            this.saver = saver;
        }

        public abstract Result<None> Run();

        protected Result<Bitmap> CreateTagsCloud(TagsCloudSettings settings)
        {
            return Result.Of(cloudFactory)
                .Then(cloud => cloud.Create(settings));
        }

        protected Result<None> SaveTagsCloud(string path, Image image)
        {
            return Result.OfAction(() => saver.Save(path, image));
        }
    }
}