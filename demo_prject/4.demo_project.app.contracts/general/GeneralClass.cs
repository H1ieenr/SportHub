using AutoMapper;
using azicloud.app.contracts;
using System;

namespace demo_project.app.contracts
{
    public class CheckDTO
    {
        public int result { get; set; }
        public long id { get; set; }
        public string message { get; set; }
    }
    public class CreateDTO
    {
        public int result { get; set; }
        public long id { get; set; }
        public string message { get; set; }
    }

    public class DeleteDTO
    {
        public int result { get; set; }
        public long id { get; set; }
        public string message { get; set; }
    }

    public class ActiveDTO
    {
        public int result { get; set; }
        public long id { get; set; }
        public string message { get; set; }
    }

    //public class DeletedRequestDTO:ZaloAppBaseRequestDTO
    //{
    //    public long Id { get; set; }
    //}

    public class ImageInfoDTO
    {
        public string server_files { get; set; } = "";
        public string filepath { get; set; } = "";
        public string filename { get; set; } = "";
        public string url { get=>server_files+filepath+filename;  }
    }

    public class StringToDateTimeConverter : ITypeConverter<string, DateTime>
    {
        public DateTime Convert(string source, DateTime destination, ResolutionContext context)
        {
            return DateTime.Parse(source);
        }
    }

    public class DateTimeToStringConverter : ITypeConverter<DateTime, string>
    {
        public string Convert(DateTime source, string destination, ResolutionContext context)
        {
            return source.ToString();
        }
    }

    public class TenantConsumerBaseRequestDTO : TenantBaseRequestDTO
    {
        public long consumer_id { get; set; }
        public string session_id { get; set; }
        public string zuId { get; set; }
        public string zuToken { get; set; }
        public new long lang_id { get; set; }
    }
}
