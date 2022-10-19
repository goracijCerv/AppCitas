using Apcitas.WebService.Helpers;
using AutoMapper.Configuration.Conventions;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Apcitas.WebService.Extensions;

public static class HttpExtensions
{
    public static void AddPaginationHeader(this HttpResponse response, int currentPage,
        int itmesPerPage, int totalItems,int totalPages)
    {
        var paginationHeader = new PaginationHeader(currentPage, itmesPerPage, totalItems, totalPages);
        response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeader));
        response.Headers.Add("Acces-Control-Expose-Headers", "Pagination");
    }
}
