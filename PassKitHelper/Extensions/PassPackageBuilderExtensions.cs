namespace PassKitHelper
{
    using System.IO;
    using System.Runtime.CompilerServices;

    public static class PassPackageBuilderExtensions
    {
        /// <summary>
        /// Sets the <see cref="PassPackageBuilder.AutoDisposeOnBuild"/> property value.
        /// </summary>
        public static PassPackageBuilder AutoDisposeOnBuild(this PassPackageBuilder builder, bool value)
        {
            builder.AutoDisposeOnBuild(value);
            return builder;
        }

        /// <summary>
        /// The image displayed as the background of the front of the pass.
        /// </summary>
        public static PassPackageBuilder Background(this PassPackageBuilder builder, byte[] content)
        {
            builder.AddFile(GetFileName(GetCaller()), content);
            return builder;
        }

        /// <summary>
        /// The image displayed as the background of the front of the pass.
        /// </summary>
        public static PassPackageBuilder Background(this PassPackageBuilder builder, Stream content)
        {
            builder.AddFile(GetFileName(GetCaller()), content);
            return builder;
        }

        /// <summary>
        /// 2X-version of Background.
        /// </summary>
        public static PassPackageBuilder Background2X(this PassPackageBuilder builder, byte[] content)
        {
            builder.AddFile(GetFileName(GetCaller()), content);
            return builder;
        }

        /// <summary>
        /// 2X-version of Background.
        /// </summary>
        public static PassPackageBuilder Background2X(this PassPackageBuilder builder, Stream content)
        {
            builder.AddFile(GetFileName(GetCaller()), content);
            return builder;
        }

        /// <summary>
        /// 3X-version of Background.
        /// </summary>
        public static PassPackageBuilder Background3X(this PassPackageBuilder builder, byte[] content)
        {
            builder.AddFile(GetFileName(GetCaller()), content);
            return builder;
        }

        /// <summary>
        /// 3X-version of Background.
        /// </summary>
        public static PassPackageBuilder Background3X(this PassPackageBuilder builder, Stream content)
        {
            builder.AddFile(GetFileName(GetCaller()), content);
            return builder;
        }

        /// <summary>
        /// The image displayed on the front of the pass near the barcode.
        /// </summary>
        public static PassPackageBuilder Footer(this PassPackageBuilder builder, byte[] content)
        {
            builder.AddFile(GetFileName(GetCaller()), content);
            return builder;
        }

        /// <summary>
        /// The image displayed on the front of the pass near the barcode.
        /// </summary>
        public static PassPackageBuilder Footer(this PassPackageBuilder builder, Stream content)
        {
            builder.AddFile(GetFileName(GetCaller()), content);
            return builder;
        }

        /// <summary>
        /// 2X-version of Footer.
        /// </summary>
        public static PassPackageBuilder Footer2X(this PassPackageBuilder builder, byte[] content)
        {
            builder.AddFile(GetFileName(GetCaller()), content);
            return builder;
        }

        /// <summary>
        /// 2X-version of Footer.
        /// </summary>
        public static PassPackageBuilder Footer2X(this PassPackageBuilder builder, Stream content)
        {
            builder.AddFile(GetFileName(GetCaller()), content);
            return builder;
        }

        /// <summary>
        /// 3X-version of Footer.
        /// </summary>
        public static PassPackageBuilder Footer3X(this PassPackageBuilder builder, byte[] content)
        {
            builder.AddFile(GetFileName(GetCaller()), content);
            return builder;
        }

        /// <summary>
        /// 3X-version of Footer.
        /// </summary>
        public static PassPackageBuilder Footer3X(this PassPackageBuilder builder, Stream content)
        {
            builder.AddFile(GetFileName(GetCaller()), content);
            return builder;
        }

        /// <summary>
        /// The pass’s icon. This is displayed in notifications and in emails that have a pass attached, and on the lock screen.
        /// </summary>
        /// <remarks>
        /// When it is displayed, the icon gets a shine effect and rounded corners.
        /// </remarks>
        public static PassPackageBuilder Icon(this PassPackageBuilder builder, byte[] content)
        {
            builder.AddFile(GetFileName(GetCaller()), content);
            return builder;
        }

        /// <summary>
        /// The pass’s icon. This is displayed in notifications and in emails that have a pass attached, and on the lock screen.
        /// </summary>
        /// <remarks>
        /// When it is displayed, the icon gets a shine effect and rounded corners.
        /// </remarks>
        public static PassPackageBuilder Icon(this PassPackageBuilder builder, Stream content)
        {
            builder.AddFile(GetFileName(GetCaller()), content);
            return builder;
        }

        /// <summary>
        /// 2X-version of Icon.
        /// </summary>
        public static PassPackageBuilder Icon2X(this PassPackageBuilder builder, byte[] content)
        {
            builder.AddFile(GetFileName(GetCaller()), content);
            return builder;
        }

        /// <summary>
        /// 2X-version of Icon.
        /// </summary>
        public static PassPackageBuilder Icon2X(this PassPackageBuilder builder, Stream content)
        {
            builder.AddFile(GetFileName(GetCaller()), content);
            return builder;
        }

        /// <summary>
        /// 3X-version of Icon.
        /// </summary>
        public static PassPackageBuilder Icon3X(this PassPackageBuilder builder, byte[] content)
        {
            builder.AddFile(GetFileName(GetCaller()), content);
            return builder;
        }

        /// <summary>
        /// 3X-version of Icon.
        /// </summary>
        public static PassPackageBuilder Icon3X(this PassPackageBuilder builder, Stream content)
        {
            builder.AddFile(GetFileName(GetCaller()), content);
            return builder;
        }

        /// <summary>
        /// The image displayed on the front of the pass in the top left.
        /// </summary>
        public static PassPackageBuilder Logo(this PassPackageBuilder builder, byte[] content)
        {
            builder.AddFile(GetFileName(GetCaller()), content);
            return builder;
        }

        /// <summary>
        /// The image displayed on the front of the pass in the top left.
        /// </summary>
        public static PassPackageBuilder Logo(this PassPackageBuilder builder, Stream content)
        {
            builder.AddFile(GetFileName(GetCaller()), content);
            return builder;
        }

        /// <summary>
        /// 2X-version of Logo.
        /// </summary>
        public static PassPackageBuilder Logo2X(this PassPackageBuilder builder, byte[] content)
        {
            builder.AddFile(GetFileName(GetCaller()), content);
            return builder;
        }

        /// <summary>
        /// 2X-version of Logo.
        /// </summary>
        public static PassPackageBuilder Logo2X(this PassPackageBuilder builder, Stream content)
        {
            builder.AddFile(GetFileName(GetCaller()), content);
            return builder;
        }

        /// <summary>
        /// 3X-version of Logo.
        /// </summary>
        public static PassPackageBuilder Logo3X(this PassPackageBuilder builder, byte[] content)
        {
            builder.AddFile(GetFileName(GetCaller()), content);
            return builder;
        }

        /// <summary>
        /// 3X-version of Logo.
        /// </summary>
        public static PassPackageBuilder Logo3X(this PassPackageBuilder builder, Stream content)
        {
            builder.AddFile(GetFileName(GetCaller()), content);
            return builder;
        }

        /// <summary>
        /// The image displayed behind the primary fields on the front of the pass.
        /// </summary>
        public static PassPackageBuilder Strip(this PassPackageBuilder builder, byte[] content)
        {
            builder.AddFile(GetFileName(GetCaller()), content);
            return builder;
        }

        /// <summary>
        /// The image displayed behind the primary fields on the front of the pass.
        /// </summary>
        public static PassPackageBuilder Strip(this PassPackageBuilder builder, Stream content)
        {
            builder.AddFile(GetFileName(GetCaller()), content);
            return builder;
        }

        /// <summary>
        /// 2X-version of Strip.
        /// </summary>
        public static PassPackageBuilder Strip2X(this PassPackageBuilder builder, byte[] content)
        {
            builder.AddFile(GetFileName(GetCaller()), content);
            return builder;
        }

        /// <summary>
        /// 2X-version of Strip.
        /// </summary>
        public static PassPackageBuilder Strip2X(this PassPackageBuilder builder, Stream content)
        {
            builder.AddFile(GetFileName(GetCaller()), content);
            return builder;
        }

        /// <summary>
        /// 3X-version of Strip.
        /// </summary>
        public static PassPackageBuilder Strip3X(this PassPackageBuilder builder, byte[] content)
        {
            builder.AddFile(GetFileName(GetCaller()), content);
            return builder;
        }

        /// <summary>
        /// 3X-version of Strip.
        /// </summary>
        public static PassPackageBuilder Strip3X(this PassPackageBuilder builder, Stream content)
        {
            builder.AddFile(GetFileName(GetCaller()), content);
            return builder;
        }

        /// <summary>
        /// An additional image displayed on the front of the pass. For example, on a membership card, the thumbnail could be used to a picture of the cardholder.
        /// </summary>
        public static PassPackageBuilder Thumbnail(this PassPackageBuilder builder, byte[] content)
        {
            builder.AddFile(GetFileName(GetCaller()), content);
            return builder;
        }

        /// <summary>
        /// An additional image displayed on the front of the pass. For example, on a membership card, the thumbnail could be used to a picture of the cardholder.
        /// </summary>
        public static PassPackageBuilder Thumbnail(this PassPackageBuilder builder, Stream content)
        {
            builder.AddFile(GetFileName(GetCaller()), content);
            return builder;
        }

        /// <summary>
        /// 2X-version of Thumbnail.
        /// </summary>
        public static PassPackageBuilder Thumbnail2X(this PassPackageBuilder builder, byte[] content)
        {
            builder.AddFile(GetFileName(GetCaller()), content);
            return builder;
        }

        /// <summary>
        /// 2X-version of Thumbnail.
        /// </summary>
        public static PassPackageBuilder Thumbnail2X(this PassPackageBuilder builder, Stream content)
        {
            builder.AddFile(GetFileName(GetCaller()), content);
            return builder;
        }

        /// <summary>
        /// 3X-version of Thumbnail.
        /// </summary>
        public static PassPackageBuilder Thumbnail3X(this PassPackageBuilder builder, byte[] content)
        {
            builder.AddFile(GetFileName(GetCaller()), content);
            return builder;
        }

        /// <summary>
        /// 3X-version of Thumbnail.
        /// </summary>
        public static PassPackageBuilder Thumbnail3X(this PassPackageBuilder builder, Stream content)
        {
            builder.AddFile(GetFileName(GetCaller()), content);
            return builder;
        }

        private static string GetCaller([CallerMemberName] string caller = "unknown")
        {
            return caller;
        }

        private static string GetFileName(string name)
        {
            if (name.EndsWith("2X"))
            {
                return name.Replace("2X", "@2X").ToLowerInvariant() + ".png";
            }

            if (name.EndsWith("3X"))
            {
                return name.Replace("3X", "@3X").ToLowerInvariant() + ".png";
            }

            return name.ToLowerInvariant() + ".png";
        }
    }
}
