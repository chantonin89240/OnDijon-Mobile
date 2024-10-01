using OnDijon.Common.Entities.Dto;
using OnDijon.Common.Entities.Request;
using OnDijon.Common.Entities.Response;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace OnDijon.Common.Utils.Services.Interfaces
{
    public interface IHttpService
    {
        Task<T> GetAsync<T>(string uri, HttpMethod method, string jsonContent) where T : class;
        Task<T> GetBisAsync<T>(string uri, HttpMethod method) where T : class;
        Task<T> PostBisAsync<T>(string path, HttpMethod method, string jsonContent) where T : class;


        Task<DtoResponse<T>> PostAsync<T>(Uri path, DtoRequest request) where T : Dto, new();
        Task<T> PostAsync<T>(Uri path, string jsonContent) where T : WsDMDto, new();

        Task<T> PutAsync<T>(string path, HttpMethod method, string jsonContent) where T : class;

        Task<T> DeleteAsync<T>(string uri, HttpMethod method, string jsonContent) where T : class;

    }
}