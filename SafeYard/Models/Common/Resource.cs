using System.Collections.Generic;

namespace SafeYard.Models.Common
{
    public class Resource<T>
    {
        public T Data { get; set; }
        public List<Link> Links { get; set; } = new();

        public Resource(T data)
        {
            Data = data;
        }
    }
}