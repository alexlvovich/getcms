using GetCms.Models.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace GetCms.Models
{
    public static class Languages
    {
        public static Language English => new Language() { Id = 1, Name = "English", Code = "en" };

        public static Language German => new Language() { Id = 2, Name = "German", Code = "de" };

        public static Language Chinese => new Language() { Id = 3, Name = "Chinese", Code = "cmn" };

        public static Language Spanish => new Language() { Id = 4, Name = "Spanish", Code = "es" };

        public static Language French => new Language() { Id = 5, Name = "French", Code = "fr" };

    }
}
