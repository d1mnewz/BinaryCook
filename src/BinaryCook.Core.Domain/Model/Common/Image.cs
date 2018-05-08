using System;
using BinaryCook.Core.Data.Entities;
using BinaryCook.Core.Data.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BinaryCook.Core.Domain.Model.Common
{
    [Serializable]
    public class Image : ValueObject<Image>
    {
        private Image()
        {
        }

        public Image(string original, string thumbnail)
        {
            Original = original;
            Thumbnail = thumbnail;
        }

        public string Original { get; private set; }
        public string Thumbnail { get; private set; }

        public static Image None => new Image();
    }

    public class ImageConfiguration
    {
        public static void Configure<TOwner>(ReferenceOwnershipBuilder<TOwner, Image> builder) where TOwner : class
        {
            builder.Property(x => x.Original).NVarChar(512).Optional();
            builder.Property(x => x.Thumbnail).NVarChar(512).Optional();
        }
    }
}