using System.Collections.Generic;

namespace Yol.Punla.ViewModels
{
    public static class AppImages
    {
        public static readonly string PandaAvatar = "https://yolpunlastorage.blob.core.windows.net/yolpunlacontainer/RBF/Contact.Photo/manavatar.png";
        public static readonly string UnicornAvatar = "https://yolpunlastorage.blob.core.windows.net/yolpunlacontainer/RBF/Contact.Photo/womanavatar.png";

        public static List<Avatar> Avatars { get; set; }
        static AppImages() => Init();

        public static void Init()
        {
            Avatars = new List<Avatar>
            {
                new Avatar{ Name = "Fox", SourceUrl = "https://yolpunlastorage.blob.core.windows.net/yolpunlacontainer/RBF/Contact.Photo/fox.png"},
                new Avatar{ Name = "Chicken", SourceUrl = "https://yolpunlastorage.blob.core.windows.net/yolpunlacontainer/RBF/Contact.Photo/chicken.png"},
                new Avatar{ Name = "Monkey", SourceUrl = "https://yolpunlastorage.blob.core.windows.net/yolpunlacontainer/RBF/Contact.Photo/monkey.png"},
                new Avatar{ Name = "Panda", SourceUrl = PandaAvatar},
                new Avatar{ Name = "Unicorn", SourceUrl = UnicornAvatar},
                new Avatar{ Name = "Reinder", SourceUrl = "https://yolpunlastorage.blob.core.windows.net/yolpunlacontainer/RBF/Contact.Photo/reinder.png"},
                new Avatar{ Name = "Rabbit", SourceUrl = "https://yolpunlastorage.blob.core.windows.net/yolpunlacontainer/RBF/Contact.Photo/rabbit.png"},
                new Avatar{ Name = "Bear", SourceUrl = "https://yolpunlastorage.blob.core.windows.net/yolpunlacontainer/RBF/Contact.Photo/bear.png"}
            };
        }
    }
}
