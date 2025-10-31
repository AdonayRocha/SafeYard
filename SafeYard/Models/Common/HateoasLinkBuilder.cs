using Microsoft.AspNetCore.Mvc;
using SafeYard.Models.Common;

namespace SafeYard.Services
{
    public class HateoasLinkBuilder
    {
        private readonly IUrlHelper _urlHelper;

        public HateoasLinkBuilder(IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public List<Link> ForMoto(int id)
        {
            return new List<Link>
            {
                new Link("self",  _urlHelper.Link("GetMotoById", new { id }), "GET"),
                new Link("update",_urlHelper.Link("UpdateMoto", new { id }), "PUT"),
                new Link("delete",_urlHelper.Link("DeleteMoto", new { id }), "DELETE")
            };
        }

        public List<Link> ForCliente(int id)
        {
            return new List<Link>
            {
                new Link("self",  _urlHelper.Link("GetClienteById", new { id }), "GET"),
                new Link("update",_urlHelper.Link("UpdateCliente", new { id }), "PUT"),
                new Link("delete",_urlHelper.Link("DeleteCliente", new { id }), "DELETE")
            };
        }

        public List<Link> ForPatio(int id)
        {
            return new List<Link>
            {
                new Link("self",  _urlHelper.Link("GetPatioById", new { id }), "GET"),
                new Link("update",_urlHelper.Link("UpdatePatio", new { id }), "PUT"),
                new Link("delete",_urlHelper.Link("DeletePatio", new { id }), "DELETE")
            };
        }

        public List<Link> ForCollection(string routeName, int page, int pageSize, int totalPages)
        {
            var links = new List<Link>
            {
                new Link("self", _urlHelper.Link(routeName, new { page, pageSize }), "GET")
            };

            if (page > 1)
                links.Add(new Link("prev", _urlHelper.Link(routeName, new { page = page - 1, pageSize }), "GET"));

            if (page < totalPages)
                links.Add(new Link("next", _urlHelper.Link(routeName, new { page = page + 1, pageSize }), "GET"));

            return links;
        }
    }
}